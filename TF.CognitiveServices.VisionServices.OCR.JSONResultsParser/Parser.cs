using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace TF.CognitiveServices.VisionServices.OCR
{
    public class Parser 
    {
        private readonly string _pattern;
        private readonly List<string> _wordList;
        private readonly Dictionary<string, string> _keyValuePairs;
        private readonly List<string> _contains;
        private readonly List<string> _doesNotContain;
        private readonly List<string> _replacements;

        #region Properties
        public string Name { get; set; }
        public string Value { get; set; }
        public bool Found { get; set; }
        #endregion

        #region ctors
        public Parser(string name, string pattern) : this(name, pattern, null)
        { }
        public Parser(string name, string pattern, List<string> replacements = null) : this(name, pattern, null, null, null, null, replacements)
        {
        }

        public Parser(string name, string pattern, List<string> contains=null, List<string> doesnotcontain=null, List<string> replacements = null) : this(name, pattern, contains, doesnotcontain, null, null, replacements)
        {
        }
       
        public Parser(string name, List<string> wordlist, List<string> replacements = null) : this(name, null, null, null, wordlist, null, replacements)
        {

        }

        public Parser(string name, Dictionary<string, string> keyvaluepairs, List<string> replacements = null) : this(name, null, null, null, null, keyvaluepairs, replacements)
        {
            
        }

        public Parser(string name, string pattern = null, List<string> contains = null, List<string> doesnotcontain = null, List<string> wordlist = null, Dictionary<string, string> keyvaluepairs = null, List<string> replacements = null) 
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _pattern = pattern;
            _wordList = wordlist;
            _contains = contains;
            _doesNotContain = doesnotcontain;
            _keyValuePairs = keyvaluepairs;
            _replacements = replacements;
        }

        #endregion
 
        public void Run(string value)
        {
            // Find Match
            if (!string.IsNullOrEmpty(_pattern))
            {
                Found = IsMatch(value, _pattern, _contains, _doesNotContain);
            }
            else if (_wordList != null)
            {
                Found = IsMatch(value, _wordList);
            }
            else if (_keyValuePairs != null)
            {
                Found = IsMatch(value, _keyValuePairs);
            }

            if (Found)
            {
                if (_replacements != null)
                {
                    foreach (var r in _replacements)
                    {
                        value = value.ToLower().Replace(r.ToLower(), "");
                    }
                }
                Value = value.Trim();
            }
        }

        private bool IsMatch(string value, string pattern, List<string> contains, List<string> doesnotcontain)
        {
            if (Regex.IsMatch(input: value, pattern: pattern, options: RegexOptions.IgnoreCase))
            {
                if (contains != null && doesnotcontain != null)
                {
                    if (contains.Any(c => value.ToLower().Contains(c.ToLower())) && doesnotcontain.Any(dc => !value.ToLower().Contains(dc.ToLower())) )
                    {
                        return true;
                    }
                }
                else if (contains != null )
                {
                    if (contains.Any(c => value.ToLower().Contains(c.ToLower())))
                    {
                        return true;
                    }
                }
                else if(doesnotcontain != null)
                {
                    if (doesnotcontain.Any(dc => !value.ToLower().Contains(dc.ToLower())))
                    {
                        return true;
                    }
                }
                else
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsMatch(string value, string match)
        {
            return Regex.IsMatch(value.ToLower(), string.Format(@"\b{0}\b", Regex.Escape(match.ToLower())));
        }
        private bool IsMatch(string value, List<string> wordList)
        {
            // make sure text isn't an email with the contact's name.
            if (!value.Contains("@"))
            {
                foreach (string w in wordList)
                {
                    // Found Exact word match
                    if (Regex.IsMatch(value.ToLower(), string.Format(@"\b{0}\b", Regex.Escape(w.ToLower()))))
                        return true;
                }
            }
            // Nothing Found
            return false;
        }
        private bool IsMatch(string value, Dictionary<string, string> wordList)
        {
            // make sure text isn't an email with the contact's name.
            if (!value.Contains("@"))
            {
                foreach (var w in wordList)
                {
                    // Found Exact word match
                    if (Regex.IsMatch(value.ToLower(), string.Format(@"\b{0}\b", Regex.Escape(w.Key.ToLower()))))
                        return true;
                    else if (Regex.IsMatch(value.ToLower(), string.Format(@"\b{0}\b", Regex.Escape(w.Value.ToLower()))))
                        return true;
                }
            }
            // Nothing Found
            return false;
        }
    }
}
