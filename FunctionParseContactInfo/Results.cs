using System;
using System.Collections.Generic;

namespace FunctionParseContactInfo
{
    public class Results
    {
        public Dictionary<string, string> Info = new Dictionary<string, string>();
        public List<string> UnKnown = new List<string>();
        public Results(Dictionary<string, string> info, List<string> unKnown)
        {
            Info = info ?? throw new ArgumentNullException(nameof(info));
            UnKnown = unKnown ?? throw new ArgumentNullException(nameof(unKnown));
        }
    }
}
