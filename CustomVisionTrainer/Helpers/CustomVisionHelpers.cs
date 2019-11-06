using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training;
using Microsoft.Azure.CognitiveServices.Vision.CustomVision.Training.Models;

namespace CustomVisionTrainer.Helpers
{
    public static class CustomVisionHelpers
    {
        public static async Task<Tag> GetOrCreateTagAsync(this CustomVisionTrainingClient trainingApi, Guid projectId, string tag)
        {
            var tags = await trainingApi.GetTagsWithHttpMessagesAsync(projectId);
            Tag returnTag = tags.Body.FirstOrDefault(x => x.Name == tag);

            if (returnTag == null)
            {
                returnTag = await trainingApi.CreateTagAsync(projectId, tag);
            }

            return returnTag;
        }
    }
}
