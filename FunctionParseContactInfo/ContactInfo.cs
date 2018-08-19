using System.Collections.Generic;
using TF.CognitiveServices.VisionServices.OCR;

namespace FunctionParseContactInfo
{
    public class ContactInfo
    {
        #region "RegEx Patterns"
        const string EMAIL_PATTERN = @"^([a-z0-9_.-]+)@([da-z.-]+).([a-z.]{2,6})";
        const string PHONE_PATTERN = @"\(?\d{3}\)?[-\.]? *\d{3}[-\.]? *[-\.]?\d{4}";
        const string WEBSITE_PATTERN = @"^www.";
        const string FACEBOOK_PATTERN = @"^www.Facebook.com";
        const string TWITTER_PATTERN = "^@";
        #endregion

        #region "Reference Data"
        private readonly List<string> names = Names.GetNames();
        private readonly List<string> companies = Names.GetCompanyTypes();
        private readonly List<string> titles = Names.GetTitles();
        private readonly Dictionary<string, string> states = Names.GetStates();
        #endregion 

        // Unmatched values
        public List<string> UnKnown = new List<string>();

        #region "Define Parsers"

        public List<Parser> Parsers = new List<Parser> {
              new Parser("Company", Names.GetCompanyTypes())
            , new Parser("Name", Names.GetNames())
            , new Parser("Title", Names.GetTitles())
            , new Parser("CityStateZip", Names.GetStates())
            , new Parser("Email", EMAIL_PATTERN)
            , new Parser("Website", WEBSITE_PATTERN, null, new List<string>{"facebook" },null)
            , new Parser("Facebook", FACEBOOK_PATTERN)
            , new Parser("Twitter", TWITTER_PATTERN)
            , new Parser("Phone", PHONE_PATTERN,new List<string>{"phone","office","tel","t","o","p"}, null, new List<string>{"phone","office","tel","t","o","p",":"})
            , new Parser("Fax", PHONE_PATTERN, new List<string>{"fax","f"}, null, new List<string>{"fax","f",":"})
            , new Parser("Cell", PHONE_PATTERN, new List<string>{"cell","mob","m","c"}, null, new List<string>{"cell","mob","m","c",":"})
        };

        #endregion

        public void Parse(Region r)
        {
            var parser = new JSONResultsParser();
            parser.Parse(r, Parsers, UnKnown);
        }

    }
}