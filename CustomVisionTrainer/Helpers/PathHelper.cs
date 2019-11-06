using System.Collections.Generic;
using System.IO;

namespace CustomVisionTrainer.Helpers
{
    public class PathHelper
    {
        private List<string> _subDirsList;

        public IEnumerable<string> GetAllSubDirectories(string rootDir)
        {
            _subDirsList = new List<string>();
            LoadSubDirs(rootDir);
            return _subDirsList;
        }

        public IEnumerable<string> GetAllFiles(string rootDir)
        {
            var subDirs =  GetAllSubDirectories(rootDir);
            return GetAllFiles(subDirs);
        }

        public IEnumerable<string> GetAllFiles(IEnumerable<string> directories)
        {
            var files = new List<string>();
            foreach (var directory in directories)
            {
                var filesInDir = Directory.GetFiles(directory);
                files.AddRange(filesInDir);
            }

            return files;
        }

        private void LoadSubDirs(string dir)
        {
            var subdirectoryEntries = Directory.GetDirectories(dir);

            if (subdirectoryEntries.Length == 0)
            {
                _subDirsList.Add(dir);
            }

            foreach (var subdirectory in subdirectoryEntries)
            {
                LoadSubDirs(subdirectory);
            }
        }
    }
}
