using System.Collections.Generic;

namespace TF.CognitiveServices.VisionServices.OCR
{
    public class Line
    {
        public bool Found { get; set; }
        public string BoundingBox { get; set; }
        public List<Word> Words { get; set; }
    }
}