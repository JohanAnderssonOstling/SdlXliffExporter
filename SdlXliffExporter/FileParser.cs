using SdlXliffExporter.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;


namespace SdlXliffExporter
{
    
    class FileParser
    {
        private string TargetLanguage { get; set; }
        private string[] ProjectExtensions { get; set; }
        public FileParser( string targetLanguage, string[] projectExtensions)
        {
            this.TargetLanguage = targetLanguage;
            this.ProjectExtensions = projectExtensions;
        }
        public TradosObject ParseFile(string filePath)
        {
            TradosObject tradosObject = new TradosObject();
            tradosObject.Path = filePath;
            tradosObject.Name = filePath.Substring(filePath.LastIndexOf("\\") + 1);

            if (ProjectExtensions.Any(filePath.Contains)) //Check if file is a project
            {
                tradosObject = ExtractFileFromProject(tradosObject);
            }
            else
            {
                tradosObject.AddSegmentPath(filePath);
            }
            tradosObject.TargetLanguage = this.TargetLanguage;
            tradosObject = ExtractSegments(tradosObject);
            return tradosObject;
        }
        private TradosObject ExtractFileFromProject(TradosObject tradosObject)
        {
            ZipArchive project = ZipFile.Open(tradosObject.Path, ZipArchiveMode.Read);
            string tempFile;
            foreach(ZipArchiveEntry entry in project.Entries)
            {
                string entryName = entry.FullName;
                if (entryName.StartsWith(TargetLanguage))
                {
                    tempFile = Path.GetTempFileName();
                    entry.ExtractToFile(tempFile, true);
                    tradosObject.SegmentPaths.Add(tempFile);
                    tradosObject.targetFileFound = true;
                }
                else if (entryName.Contains(".sdlxliff")){
                    entryName = entryName.Replace('\\', '/');
                    tradosObject.SourceLanguage = entryName.Substring(0, entryName.IndexOf("/")); //Extract source language-code from path
                }
                
            }
            return tradosObject;
        }
        private TradosObject ExtractSegments(TradosObject tradosObject)
        {
            foreach (string segmentPath in tradosObject.SegmentPaths)
            {
                try
                {
                    XDocument segments = XDocument.Load(segmentPath);
                    XNamespace segmentNamespace = segments.Root.Name.Namespace;
                    foreach (XElement segment in segments.Descendants(segmentNamespace + "trans-unit"))
                    {
                        XElement sourceSegment = segment.Element(segmentNamespace + "source");
                        XElement targetSegment = segment.Element(segmentNamespace + "target");
                        if ((!(sourceSegment is null)) && (!(targetSegment is null)))//If segments are locked the XElement will be null
                        {
                            tradosObject.AddSegmentPairs(ParseSegment(sourceSegment.Value, targetSegment.Value));
                        }
                        else
                        {
                            tradosObject.HasUnparsedSegments = true;
                        }
                    }
                    tradosObject.IsParsed = true;
                }
                catch (XmlException e)
                {
                    tradosObject.XmlError = true;
                } 
            }
            return tradosObject;
        }
        private List<SegmentPair> ParseSegment(string sourceSegment, string targetSegment)//Try to split segment into sentences
        {
            List<SegmentPair> SegmentPairs = new List<SegmentPair>();
            if (!sourceSegment.Contains(".") || !targetSegment.Contains("."))
            {
                SegmentPairs.Add(new SegmentPair(sourceSegment, targetSegment));
                return SegmentPairs;
            }
            string[] sourceSegments = SplitSegment(sourceSegment);
            string[] targetSegments = SplitSegment(targetSegment);

            if (sourceSegments.Length != targetSegments.Length)
            {
                SegmentPairs.Add(new SegmentPair(sourceSegment, targetSegment));
                return SegmentPairs;
            }
            for(int i = 0; i < sourceSegments.Length; i++)
            {
                SegmentPairs.Add(new SegmentPair(sourceSegments[i], targetSegments[i]));
            }
            return SegmentPairs;
        }
        private string[] SplitSegment(string segment)
        {
            List<int> indexes = GetSegmentIndexes(segment);
            string[] Segments = new string[indexes.Count];

            for(int i = 0; i < indexes.Count - 1; i++)
            {
                Segments[i] = segment.Substring(indexes[i], indexes[i + 1] - indexes[i]);
            }
            return Segments;

        }
        private List<int> GetSegmentIndexes(string segment)
        {
            List<int> indexes = new List<int>();
            for (int i = segment.IndexOf("."); i > -1; i = segment.IndexOf(".", i + 1)) // Loop through all dots in string
            {
                if (i - 1 > 0 && i + 2 < segment.Length) //Check if dot is not at start or end of segment
                {
                    if ((!Char.IsDigit(segment[i - 1]) || !Char.IsDigit(segment[i + 1])) && (Char.IsUpper(segment[i + 1]) || Char.IsUpper(segment[i + 2])))
                    {
                        indexes.Add(i);
                    }
                }
            }
            return indexes;
        }
    }
}
