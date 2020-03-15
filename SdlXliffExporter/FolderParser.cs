using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SdlXliffExporter
{
    class FolderParser
    {
        private string[] fileExtensions;
        public FolderParser(string[] fileExtensions)
        {
            this.fileExtensions = fileExtensions;
        }
        public List<string> ParseFolder(string folderPath)
        {
            string[] files = Directory.GetFiles(folderPath);
            List<string> tradosFiles = new List<string>();

            for (int i = 0; i < files.Length; i++)
            {
                if (fileExtensions.Any(files[i].EndsWith))
                { //Check if file is a trados-file
                    tradosFiles.Add(files[i]);
                }
            }
            return tradosFiles;
        }
        public List<string> ParseFolderRecursively(string folderPath)
        {
            List<string> tradosFiles = new List<string>();
            tradosFiles.AddRange(ParseFolder(folderPath));
            string[] directories = Directory.GetDirectories(folderPath);
            
            foreach(string directory in directories)
            {
                tradosFiles.AddRange(ParseFolder(directory));
                tradosFiles.AddRange(ParseFolderRecursively(directory));
            }
            return tradosFiles;
        }
        
    }
}
