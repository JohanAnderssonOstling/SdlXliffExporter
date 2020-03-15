using System;
using System.Collections.Generic;
using System.Text;

namespace SdlXliffExporter.DataStructures
{

    
    public class TradosObject
    {

        public Boolean IsParsed { get; set; } = false;
        public Boolean HasUnparsedSegments { get; set; } = false;
        public Boolean targetFileFound { get; set; } = false;
        public Boolean XmlError { get; set; } = false;
        public List<SegmentPair> SegmentPairs { get; set; } = new List<SegmentPair>();

        public string SourceLanguage { get; set; } = "";
        public string TargetLanguage { get; set; } = "";
        public string Name { get; set; }
        public string Path { get; set; }
        public List<string> SegmentPaths { get; set; } = new List<string>();

        public void AddSegmentPair(string sourceSegment, string targetSegment)
        {
            SegmentPairs.Add(new SegmentPair(sourceSegment, targetSegment));
        }
        public void AddSegmentPairs(List<SegmentPair> SegmentPairs)
        {
            this.SegmentPairs.AddRange(SegmentPairs);
            
        }
        public void AddSegmentPath(string segmentPath)
        {
            SegmentPaths.Add(segmentPath);
        }
    }
}
