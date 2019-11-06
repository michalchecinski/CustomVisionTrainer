using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CustomVisionTrainer.Helpers;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace CustomVisionTrainer
{
    class Program
    {
        private const string RootPath = @"G:\Photos";
        private const string TrainingKey = "";
        private const string CustomVisionEndpoint = "https://westeurope.api.cognitive.microsoft.com/";

        static async Task Main(string[] args)
        {
            CustomVisionTrainingClient trainingApi = new CustomVisionTrainingClient()
            {
                ApiKey = TrainingKey,
                Endpoint = CustomVisionEndpoint
            };

            Console.WriteLine("Create new project? [y/N]: ");
            var output = Console.ReadLine();

            Project project;

            if (output.Equals("y") || output.Equals("yes"))
            {
                Console.WriteLine($"Provide new project name (if no value provided name will be created):");
                var projectName = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(projectName))
                {
                    projectName = $"new-project{DateTime.UtcNow:yyyyMMdd-hhmmss}";
                }

                Console.WriteLine($"Creating new project: {projectName}");
                project = await trainingApi.CreateProjectAsync(projectName);
            }
            else
            {
                Console.WriteLine("Select project:");
                var allProjects = await trainingApi.GetProjectsAsync();
                for (int i = 0; i < allProjects.Count; i++)
                {
                    Console.WriteLine($"{i+1}  {allProjects[i].Name}");
                }

                int projectNumber;
                do
                {
                    Console.WriteLine("Provide project number from list:");
                } while (!int.TryParse(Console.ReadLine(), out projectNumber) && projectNumber>allProjects.Count);

                project = await trainingApi.GetProjectAsync(allProjects[projectNumber].Id);
            }

            var mlClassesHelper = new MlClassesHelper();
            var classes = mlClassesHelper.GetMlClassWithFiles(RootPath);

            Console.WriteLine("Uploading classes");

            foreach (var mlClass in classes)
            {
                var mlClassKey = mlClass.Key;
                var files = mlClass.Value;

                var tag = await trainingApi.GetOrCreateTagAsync(project.Id, mlClassKey);

                Console.WriteLine($"{mlClassKey}: {files.Count}");
                var imageFiles = files
                                 .Select(img => new ImageFileCreateEntry(Path.GetFileName(img), File.ReadAllBytes(img)))
                                 .ToList();
                await trainingApi.CreateImagesFromFilesAsync(project.Id,
                    new ImageFileCreateBatch(imageFiles, new List<Guid>() {tag.Id}));

                foreach (var file in files)
                {
                    using (var stream = new MemoryStream(File.ReadAllBytes(file)))
                    {
                        await trainingApi.CreateImagesFromDataAsync(project.Id, stream, new List<Guid>() { tag.Id });
                    }
                }
            }

            Console.WriteLine("Training project...");
            
            Console.WriteLine("Want to wait here for training to end? [y/N]: ");
            if (output.Equals("y") || output.Equals("yes"))
            {
                var iteration = await trainingApi.TrainProjectAsync(project.Id);

                while (iteration.Status == "Training")
                {
                    Console.Write(".");
                    Thread.Sleep(1000);

                    iteration = trainingApi.GetIteration(project.Id, iteration.Id);
                }
            }

            Console.WriteLine("END.");
        }
    }
}