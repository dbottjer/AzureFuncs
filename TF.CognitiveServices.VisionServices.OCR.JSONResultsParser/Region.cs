using System.Collections.Generic;

namespace TF.CognitiveServices.VisionServices.OCR
{
    public class Region
    {
        public string BoundingBox { get; set; }
        public List<Line> Lines { get; set; }
    }
}
