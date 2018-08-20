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
        const string MONEY_PATTERN = @"^\-?\(?\$?\s*\-?\s*\(?(((\d{1,3}((\,\d{3})*|\d*))?(\.\d{1,4})?)|((\d{1,3}((\,\d{3})*|\d*))(\.\d{0,4})?))\)?$";
        const string DATE_PATTERN = @"(?n:^(?=\d)((?<month>(0?[13578])|1[02]|(0?[469]|11)(?!.31)|0?2(?(.29)(?=.29.((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|(16|[2468][048]|[3579][26])00))|(?!.3[01])))(?<sep>[-./])(?<day>0?[1-9]|[12]\d|3[01])\k<sep>(?<year>(1[6-9]|[2-9]\d)\d{2})(?(?=\x20\d)\x20|$))?(?<time>((0?[1-9]|1[012])(:[0-5]\d){0,2}(?i:\x20[AP]M))|([01]\d|2[0-3])(:[0-5]\d){1,2})?$)";
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
            , new Parser("Total", MONEY_PATTERN, new List<string>{"Total"})
            , new Parser("Date", DATE_PATTERN)
        };

        #endregion

        public void Parse(Region r)
        {
            var parser = new JSONResultsParser();
            parser.Parse(r, Parsers, UnKnown);
        }

    }
}