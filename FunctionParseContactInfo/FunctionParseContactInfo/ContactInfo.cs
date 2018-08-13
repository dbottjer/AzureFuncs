using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FunctionParseContactInfo
{
    public class ContactInfo
    {
        // Matched values
        public Dictionary<string, string> Info = new Dictionary<string, string>();
        // Unmatched values
        public List<string> UnKnown = new List<string>();

        #region "Reference Data"
        private readonly List<string> names = new Names().GetNames();
        private readonly List<string> companies = new Names().GetCompanyTypes();
        private readonly List<string> titles = new Names().GetTitles();
        private readonly Dictionary<string, string> states = new Names().GetStates();
        #endregion

        #region "RegEx Patterns"
        const string EMAIL_PATTERN = @"^([a-z0-9_.-]+)@([da-z.-]+).([a-z.]{2,6})";
        const string PHONE_PATTERN = @"\(?\d{3}\)?[-\.]? *\d{3}[-\.]? *[-\.]?\d{4}";
        const string WEBSITE_PATTERN = @"^www.";
        const string FACEBOOK_PATTERN = @"^www.Facebook.com";
        const string TWITTER_PATTERN = "^ @";
        #endregion

        public void ParseJSON(Region r)
        {
            foreach (Line l in r.lines)
            {
                var sb = new StringBuilder();

                foreach (Word w in l.words)
                {
                    if (sb.Length == 0)
                        sb.Append(w.text);
                    else
                        sb.Append(" " + w.text);
                }

                Save(sb.ToString());

            }
            // Website wasn't found so derive website from email
            if (!Info.ContainsKey("Website") && Info.ContainsKey("Email"))
            {
                var website = Info["Email"].Split('@')[1];
                Info.Add("Website", website);
            }
        }

        private void Save(string input)
        {
            if (IsMatch(input, companies))
                Save("Company", input);
            else if (IsMatch(input, names))
                Save("Name", input);
            else if (IsMatch(input, titles))
                Save("Title", input);
            else if (IsMatch(input, states))
                Save("CityStateZip", input);
            else if (IsMatch(input, EMAIL_PATTERN, null))
                Save("Email", input);
            else if (IsMatch(input, PHONE_PATTERN, null))
            {
                input = input.Replace(":", "");

                if (IsMatch(input, "fax"))
                {
                    Save("Fax", input.ToLower().Replace("fax", "").Trim());
                }
                else if (IsMatch(input, "f"))
                {
                    Save("Fax", input.ToLower().Replace("f", "").Trim());
                }
                else if (IsMatch(input, "office"))
                {
                    Save("Phone", input.ToLower().Replace("office", "").Trim());
                }
                else if (IsMatch(input, "o"))
                {
                    Save("Phone", input.ToLower().Replace("o", "").Trim());
                }
                else if (IsMatch(input, "mobile"))
                {
                    Save("Mobile", input.ToLower().Replace("mobile", "").Trim());
                }
                else if (IsMatch(input, "m"))
                {
                    Save("Mobile", input.ToLower().Replace("m", "").Trim());
                }
                else if (IsMatch(input, "mob"))
                {
                    Save("Mobile", input.ToLower().Replace("mob", "").Trim());
                }
                else if (IsMatch(input, "p"))
                {
                    Save("Phone", input.ToLower().Replace("p", "").Trim());
                }
                else if (IsMatch(input, "t"))
                {
                    Save("Phone", input.ToLower().Replace("t", "").Trim());
                }
                else if (IsMatch(input, "tel"))
                {
                    Save("Phone", input.ToLower().Replace("tel", "").Trim());
                }
                else if (IsMatch(input, "c"))
                {
                    Save("Cell", input.ToLower().Replace("c", "").Trim());
                }
                else if (IsMatch(input, "cell"))
                {
                    Save("Mobile", input.ToLower().Replace("cell", "").Trim());
                }
                else
                    Save("Phone", input.Trim());
            }
            else if (IsMatch(input.Replace(" ", ""), WEBSITE_PATTERN, "facebook"))
                Save("Website", input);
            else if (IsMatch(input.Replace(" ", ""), FACEBOOK_PATTERN))
                Save("Facebook", input);
            else if (IsMatch(input.Replace(" ", ""), TWITTER_PATTERN, null))
                Save("Twitter", input);
            else
                UnKnown.Add(input);
        }

        private void Save(string key, string value)
        {
            if (!Info.ContainsKey(key))
                Info.Add(key, value);
        }

        private bool IsMatch(string input, string pattern, string notContains)
        {
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
            {
                if (string.IsNullOrEmpty(notContains))
                    return true;
                else
                {
                    if (!input.Contains(notContains))
                        return true;
                }
            }
            return false;
        }

        private bool IsMatch(string input, string match)
        {
            return Regex.IsMatch(input.ToLower(), string.Format(@"\b{0}\b", Regex.Escape(match.ToLower())));
        }

        private bool IsMatch(string input, List<string> wordList)
        {
            // make sure text isn't an email with the contact's name.
            if (!input.Contains("@"))
            {
                foreach (string w in wordList)
                {
                    // Found Exact word match
                    if (Regex.IsMatch(input.ToLower(), string.Format(@"\b{0}\b", Regex.Escape(w.ToLower()))))
                        return true;
                }
            }
            // Nothing Found
            return false;
        }

        private bool IsMatch(string input, Dictionary<string, string> wordList)
        {
            // make sure text isn't an email with the contact's name.
            if (!input.Contains("@"))
            {
                foreach (var w in wordList)
                {
                    // Found Exact word match
                    if (Regex.IsMatch(input.ToLower(), string.Format(@"\b{0}\b", Regex.Escape(w.Key.ToLower()))))
                        return true;
                    else if (Regex.IsMatch(input.ToLower(), string.Format(@"\b{0}\b", Regex.Escape(w.Value.ToLower()))))
                        return true;
                }
            }
            // Nothing Found
            return false;
        }

    }
}