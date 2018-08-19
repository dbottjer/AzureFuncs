using System;
using TF.CognitiveServices.VisionServices.OCR;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TF.CognitiveServices.VisionServices.OCR.Tests
{
    [TestClass]
    public class ParserTests
    {
        #region "RegEx Patterns"
        const string EMAIL_PATTERN  = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                                   + "@"
                                   + @"((([\-\w]+\.)+[a-zA-Z]{2,4})|(([0-9]{1,3}\.){3}[0-9]{1,3}))$";

        const string PHONE_PATTERN = @"\(?\d{3}\)?[-\.]? *\d{3}[-\.]? *[-\.]?\d{4}";
        const string WEBSITE_PATTERN = @"^www.";
        const string FACEBOOK_PATTERN = @"^www.Facebook.com";
        const string TWITTER_PATTERN = "^@";
        #endregion

        [TestMethod]
        public void Parse_WithValidEmail_ReturnsFoundTrue()
        {
            // Arrange
            var parser = new Parser("Email",EMAIL_PATTERN);
            
            // Act
            parser.Run("support@thrivefast.com");

            // Assert
            Assert.IsTrue(parser.Found && !String.IsNullOrEmpty(parser.Value));
        }

        [TestMethod]
        public void Parse_WithInvalidEmail_ReturnsFoundFalse()
        {
            // Arrange
            var parser = new Parser("Email", EMAIL_PATTERN);

            // Act
            parser.Run("support@thrivefast");

            // Assert
            Assert.IsTrue(!parser.Found && String.IsNullOrEmpty(parser.Value));
        }

        [TestMethod]
        public void Parse_WithValidCompany_ReturnsFoundTrue()
        {
            // Arrange
            var parser = new Parser("Company", Names.GetCompanyTypes());

            // Act
            parser.Run("ThriveFast, LLC");

            // Assert
            Assert.IsTrue(parser.Found && !String.IsNullOrEmpty(parser.Value));
        }

        [TestMethod]
        public void Parse_WithValidPhoneNumber_ReturnsFoundTrue()
        {
            // Arrange
            var parser = new Parser("Phone", PHONE_PATTERN);

            // Act
            parser.Run("405-834-3607");

            // Assert
            Assert.IsTrue(parser.Found && !String.IsNullOrEmpty(parser.Value));
        }

        [TestMethod]
        public void Parse_WithValidPhoneNumber_ReturnsFoundTrueReplacesCell()
        {
            // Arrange
            var parser = new Parser("Cell", PHONE_PATTERN,new System.Collections.Generic.List<string> { "Cell",":" });

            // Act
            parser.Run("Cell: 555-555-5555");

            // Assert
            Assert.IsTrue(parser.Found && parser.Value == "555-555-5555");
        }

        [TestMethod]
        public void Parse_WithValidTwitterHandle_ReturnsFoundTrue()
        {
            // Arrange
            var parser = new Parser("Twitter", TWITTER_PATTERN);

            // Act
            parser.Run("@ThriveFast");

            // Assert
            Assert.IsTrue(parser.Found && !String.IsNullOrEmpty(parser.Value));
        }

        [TestMethod]
        public void Parse_WithValidFacebookPage_ReturnsFoundTrue()
        {
            // Arrange
            var parser = new Parser("Facebook", FACEBOOK_PATTERN);

            // Act
            parser.Run("www.facebook.com/ThriveFast");

            // Assert
            Assert.IsTrue(parser.Found && !String.IsNullOrEmpty(parser.Value));

        }

        [TestMethod]
        public void Parse_WithValidWebsite_ReturnsFoundTrue()
        {
            // Arrange
            var parser = new Parser("Website", WEBSITE_PATTERN);

            // Act
            parser.Run("www.ThriveFast.com");

            // Assert
            Assert.IsTrue(parser.Found && !String.IsNullOrEmpty(parser.Value));

        }

        [TestMethod]
        public void Parse_WithValidName_ReturnsFoundTrue()
        {
            // Arrange
            var parser = new Parser("Name", Names.GetNames());

            // Act
            parser.Run("Dennis Bottjer");

            // Assert
            Assert.IsTrue(parser.Found && !String.IsNullOrEmpty(parser.Value));

        }

        [TestMethod]
        public void Parse_WithValidTitle_ReturnsFoundTrue()
        {
            // Arrange
            var parser = new Parser("Title", Names.GetTitles());

            // Act
            parser.Run("Solutions Architect");

            // Assert
            Assert.IsTrue(parser.Found && !String.IsNullOrEmpty(parser.Value));

        }

        [TestMethod]
        public void Parse_WithValidCityStateZip_ReturnsFoundTrue()
        {
            // Arrange
            var parser = new Parser("CityStateZip", Names.GetStates());

            // Act
            parser.Run("Tulsa, OK 74135");

            // Assert
            Assert.IsTrue(parser.Found && !String.IsNullOrEmpty(parser.Value));

        }
    }
}
