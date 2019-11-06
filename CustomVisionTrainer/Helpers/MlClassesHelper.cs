using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CustomVisionTrainer.Helpers
{
    public class MlClassesHelper
    {
        public Dictionary<string, List<string>> GetMlClassWithFiles(string root)
        {
            var subdirService = new PathHelper();
            var subDirs = subdirService.GetAllSubDirectories(root);

            var dictionary = new Dictionary<string, List<string>>();

            foreach (var dir in subDirs)
            {
                var files = Directory.GetFiles(dir)
                                     .Where(x => x.IsPhotoFile())
                                     .ToList();

                if (files.Any())
                {
                    var mlClass = dir.MlClassFromPath(root);
                    dictionary.Add(mlClass, files.ToList());
                }
            }

            return dictionary;
        }
    }
}