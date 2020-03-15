using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using SdlXliffExporter.DataStructures;
namespace SdlXliffExporter
{
    class Serializer
    {
        string delimeter { get; set; }
        string fileExtension { get; set; }
        string outputFolder { get; set; }
        public Serializer(string delimeter, string fileExtension, string outputFolder)
        {
            this.delimeter = delimeter;
            this.fileExtension = fileExtension;
            this.outputFolder = outputFolder;
            if (!(this.outputFolder.EndsWith("/") || this.outputFolder.Equals("\\")))
                this.outputFolder = this.outputFolder + "/";
        }
        public void Serialize(TradosObject tradosObject)
        {
            
            string outputFile = GetOutputPath(tradosObject);
            StreamWriter writer = File.CreateText(outputFile);
            foreach(SegmentPair SegmentPair in tradosObject.SegmentPairs)
            {
                if (!string.IsNullOrEmpty(SegmentPair.SourceSegment) && !string.IsNullOrEmpty(SegmentPair.TargetSegment) && !SegmentPairIsIdentical(SegmentPair))
                {
                    SegmentPair.SourceSegment = SegmentPair.SourceSegment.Replace(delimeter, " ");
                    SegmentPair.TargetSegment = SegmentPair.TargetSegment.Replace(delimeter, " ");
                    SegmentPair.SourceSegment = SegmentPair.SourceSegment.Replace("\n", " ");
                    SegmentPair.TargetSegment = SegmentPair.TargetSegment.Replace("\n", " ");
                    writer.WriteLine(SegmentPair.SourceSegment + delimeter + SegmentPair.TargetSegment);
                }
            }
            writer.Close();
        }
        private string GetOutputPath(TradosObject tradosObject)
        {
            string outputPath = outputFolder + tradosObject.SourceLanguage + "-" + tradosObject.TargetLanguage;
            Directory.CreateDirectory(outputPath);
            return outputPath + "/" + tradosObject.Name + fileExtension;
        }
        private Boolean SegmentPairIsIdentical(SegmentPair SegmentPair)
        {
            if (SegmentPair.SourceSegment.Equals(SegmentPair.TargetSegment))
            {
                return true;
            }
            return false;
        }
    }
}
