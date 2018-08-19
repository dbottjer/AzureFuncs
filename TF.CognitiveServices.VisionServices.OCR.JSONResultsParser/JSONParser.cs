using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TF.CognitiveServices.VisionServices.OCR
{
    public class JSONResultsParser
    {
        public void Parse(Region r, List<Parser> parsers) => Parse(r, parsers, null);
        public void Parse(Region r, List<Parser> parsers, List<string> unknown)
        {
            // A region and at least one parser are required.
            if (r == null)
            {
                throw new ArgumentNullException(nameof(r));
            }

            if (parsers == null)
            {
                throw new ArgumentNullException(nameof(parsers));
             }

            // A region can have one or more lines of text.
            foreach (Line l in r.Lines)
            {
                var sb = new StringBuilder();  
                // A line is made up of one or more words.
                foreach (Word w in l.Words)
                {
                    if (sb.Length == 0)
                        sb.Append(w.Text);
                    else
                        sb.Append(" " + w.Text);
                }
               
                foreach (var p in parsers.Where(x=>!x.Found))
                {
                    p.Run(sb.ToString());
                    // If a match was found then don't run the remaining parsers on this line.
                    if (p.Found)
                    {
                        l.Found = true;
                        break;
                    }
                }
                if (!l.Found)
                    unknown.Add(sb.ToString());
            }

            // If a website wasn't found we can try to derive it from the email address.
            var website = parsers.Where(p => p.Name.ToLower() == "website" && !p.Found).FirstOrDefault();
            if (website != null)
            {
                var email = parsers.Where(e => e.Name.ToLower() == "email" && e.Found).FirstOrDefault();
                if(email != null)
                {
                    website.Found = true;
                    website.Value = email.Value.Split(separator: '@')[1];
                }

            }
        }
    }
}
