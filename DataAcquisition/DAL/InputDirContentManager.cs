using System;
using System.Collections.Generic;
using System.Linq;

namespace Measurement_Evaluator.DAL
{
    public class InputConfigDirectoryContentManager
    {
        /// <summary>
        /// Resturns with list of the files in the given path. The files are orgainized into list according to their extension name
        /// </summary>
        /// <param name="path">the path of the serached directory</param>
        /// <returns></returns>
        public List<List<string>> GetDirectoryContent(string path, string filter)
        {
            List<List<string>> fileList = new List<List<string>>();

            try
            {
                List<string> preFileList = System.IO.Directory.GetFiles(path).ToList();
                List<string> fileExtensionList = new List<string>();

                foreach (string element in preFileList)
                {
                    string extension = System.IO.Path.GetExtension(element);
                    if (extension == null || extension == string.Empty || !extension.Contains(filter))
                        continue;

                    if (!fileExtensionList.Contains(extension, StringComparer.InvariantCultureIgnoreCase))
                        fileExtensionList.Add(extension);
                }

                foreach (string element in fileExtensionList)
                {
                    List<string> actualFiles = preFileList.FindAll(p => Contains(p, element, StringComparison.InvariantCultureIgnoreCase));

                    if (actualFiles != null)
                        fileList.Add(actualFiles);
                }
            }
            catch(Exception ex)
            {
            }

            return fileList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source">source string to search in</param>
        /// <param name="toCheck">the searched string</param>
        /// <param name="comp">string comparison</param>
        /// <returns></returns>
        private bool Contains(string source, string toCheck, StringComparison comp)
        {
            if (string.IsNullOrEmpty(toCheck) || string.IsNullOrEmpty(source))
                return false;

            return source.IndexOf(toCheck, comp) >= 0;
        }


    }
}
