using System.Text.RegularExpressions;

namespace CustomVisionTrainer
{
    public static class StringExtension
    {

        public static bool IsPhotoFile(this string path)
        {
            var newPath = path.ToLowerInvariant();
            return
                newPath.EndsWith(".jpg") ||
                newPath.EndsWith(".jpeg") ||
                newPath.EndsWith(".png");
        }

        public static string MlClassFromPath(this string path, string rootdir)
        {
            return path.Replace($"{rootdir}", "")
                .Replace(@"/", "_")
                .Replace(@"\", "_")
                .Replace(" ", "_")
                .TrimStart('_')
                .TrimEnd('_')
                .RemoveFragmentsBetween('(', ')');
        }


        public static string RemoveFragmentsBetween(this string rawString, char enter, char exit)
        {
            if (rawString.Contains(enter) && rawString.Contains(exit))
            {
                int substringStartIndex = rawString.IndexOf(enter) + 1;
                int substringLength = rawString.LastIndexOf(exit) - substringStartIndex;

                if (substringLength > 0 && substringStartIndex > 0)
                {
                    string substring = rawString.Substring(substringStartIndex, substringLength).RemoveFragmentsBetween(enter, exit);
                    if (substring.Length != substringLength)
                    {
                        rawString = rawString.Remove(substringStartIndex, substringLength).Insert(substringStartIndex, substring).Trim();
                    }
                }

                Regex regex = new Regex($"\\{enter}.*?\\{exit}");
                return new Regex(" +").Replace(regex.Replace(rawString, string.Empty), " ").Trim(); // Removing duplicate and tailing/leading spaces
            }
            else
            {
                return rawString;
            }
        }

    }
}
