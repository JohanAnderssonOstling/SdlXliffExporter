using System;
using System.Collections.Generic;
using System.Text;

namespace SdlXliffExporter.DataStructures
{
    public class SegmentPair
    {
        public string SourceSegment { get; set;}
        public string TargetSegment { get; set; }

        public SegmentPair(string sourceSegment, string targetSegment)
        {
            this.SourceSegment = sourceSegment;
            this.TargetSegment = targetSegment;
        }
    }
}
