using System.Collections.Generic;
using NUnit.Framework;
using NapierBankMessaging.Classes;

namespace NBM_Unit_Tests
{
    [TestFixture]
    public class ControllerTests
    {
        private Controller _controller = new Controller();

        [Test]
        public void ValidateMessage_Valid_Return_Instance_Message()
        {
            string messageHeader = "S192837465";
            string messageBody = "+447561017503\nThis is a test message for a SMS message and is below 140 characters";

            var result = _controller.ValidateMessage(messageHeader, messageBody);

            Assert.IsInstanceOf<Message>(result);
        }

        [Test]
        public void ValidateMessage_Invalid_MessageHeaderLong_Return_Null()
        {
            string messageHeader = "S1928374651";
            string messageBody = "+447561017503\nThis is a test message for a SMS message and is below 140 characters";

            var result = _controller.ValidateMessage(messageHeader, messageBody);

            Assert.IsNull(result);
        }

        [Test]
        public void ValidateMessage_Invalid_MessageHeaderShort_Return_Null()
        {
            string messageHeader = "S192837";
            string messageBody = "+447561017503\nThis is a test message for a SMS message and is below 140 characters";

            var result = _controller.ValidateMessage(messageHeader, messageBody);

            Assert.IsNull(result);
        }

        [Test]
        public void ValidateMessage_Invalid_MessageHeaderNonNumeric_Return_Null()
        {
            string messageHeader = "SNumbers";
            string messageBody = "+447561017503\nThis is a test message for a SMS message and is below 140 characters";

            var result = _controller.ValidateMessage(messageHeader, messageBody);

            Assert.IsNull(result);
        }

        [Test]
        public void ValidateMessage_Invalid_MessageHeaderNoSignChar_Return_Null()
        {
            string messageHeader = "C123456789";
            string messageBody = "+447561017503\nThis is a test message for a SMS message and is below 140 characters";

            var result = _controller.ValidateMessage(messageHeader, messageBody);

            Assert.IsNull(result);
        }

        [Test]
        public void ValidateMessage_Invalid_MessageBodyEmpty_Return_Null()
        {
            string messageHeader = "SNumbers";
            string messageBody = "";

            var result = _controller.ValidateMessage(messageHeader, messageBody);

            Assert.IsNull(result);
        }

        [Test]
        public void ValidateMessage_Invalid_MessageHeaderEmpty_Return_Null()
        {
            string messageHeader = "";
            string messageBody = "+447561017503\nThis is a test message for a SMS message and is below 140 characters";

            var result = _controller.ValidateMessage(messageHeader, messageBody);

            Assert.IsNull(result);
        }

        [Test]
        public void DetectMessageType_Valid_SMS_Return_String_SMS()
        {
            string messageHeader = "S192837465";
            string messageBody = "+447561017503\nThis is a test message for a SMS message and is below 140 characters LOL";
            Message testMessage = new Message(messageHeader, messageBody);
            string expected = "SMS";

            var result = _controller.DetectMessageType(testMessage);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void DetectMessageType_Valid_Email_Return_String_Email()
        {
            string messageHeader = "E192837465";
            string messageBody = "Test-mail@email.com\nthis is 20 character\nThis is the start of the email message text URL's should be quarantined like this one. www.google.com and also this one http://www.something.com. This is some extra text hi mom.";
            Message testMessage = new Message(messageHeader, messageBody);
            string expected = "Email";

            var result = _controller.DetectMessageType(testMessage);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void DetectMessageType_Valid_Tweet_Return_String_Tweet()
        {
            string messageHeader = "T192837465";
            string messageBody = "@(15 characters)\nThis is the message text for the sms message LOL. #Trendy @Conor #WishThisWorks this is 140 #WishThisWorks 140 140 140 140 #EndOfTheLineeeee";
            Message testMessage = new Message(messageHeader, messageBody);
            string expected = "Tweet";

            var result = _controller.DetectMessageType(testMessage);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void DetectMessageType_Invalid_Message_Return_Null()
        {
            string messageHeader = "G192837465";
            string messageBody = "@(15 characters)\nThis is the message text for the sms message LOL. #Trendy @Conor #WishThisWorks this is 140 #WishThisWorks 140 140 140 140 #EndOfTheLineeeee";
            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.DetectMessageType(testMessage);

            Assert.IsNull(result);
        }
        
        [Test]
        public void ParseSMS_Valid_SMS_Return_Instance_SMS()
        {
            string messageHeader = "S192837465";
            string messageBody = "+447561017503\nThis is a test message for a SMS message and is below 140 characters LOL";
            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.ParseSMS(testMessage);
            
            Assert.IsInstanceOf<SMS>(result);
        }

        [Test]
        public void ParseSMS_Invalid_SMSFormat_Return_Null()
        {
            string messageHeader = "S192837465";
            string messageBody = "This is the wrong format for this file";
            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.ParseSMS(testMessage);
            
            Assert.IsNull(result);
        }
        
        [Test]
        public void ParseSMS_Invalid_SMSLong_Return_Null()
        {
            string messageHeader = "S192837465";
            string messageBody = "+447561017503\nThis is a test message for a SMS message and is below 140 characters LOL Extra characters----------------------------------------------------";
            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.ParseSMS(testMessage);
            
            Assert.IsNull(result);
        }
        
        [Test]
        public void ParseEmail_Valid_EmailStandard_Return_Instance_Email()
        {
            string messageHeader = "E192837465";
            string messageBody = "Test-mail@email.com\nthis is 20 character\nThis is the start of the email message text URL's should be quarantined like this one. www.google.com and also this one http://www.something.com. This is some extra text hi mom.";
            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.ParseEmail(testMessage);
            
            Assert.IsInstanceOf<Email>(result);
        }
        
        [Test]
        public void ParseEmail_Valid_EmailSIR_Return_Instance_EmailSIR()
        {
            string messageHeader = "E192837465";
            string messageBody = "Test-mail@email.com\nSIR 22/22/22\nSort Code: 99-99-99\nNature of Incident: Staff Attack\nThere" +
                                 " was an attack at the store a member of staff was injured. Please help.\nNew Line :) www.google.com\nAnd another\none more https://www.hi.co.uk";
            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.ParseEmail(testMessage);
            
            Assert.IsInstanceOf<EmailSIR>(result);
        }
        
        [Test]
        public void ParseEmail_Invalid_EmailFormat_Return_Null()
        {
            string messageHeader = "E192837465";
            string messageBody = "This is not the correct format for an email";
            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.ParseEmail(testMessage);
            
            Assert.IsNull(result);
        }
        
        [Test]
        public void ParseEmail_Invalid_EmailSubjectLong_Return_Null()
        {
            string messageHeader = "E192837465";
            string messageBody = "Test-mail@email.com\nthis is 21 characters\nThis is the start of the email message text URL's should be quarantined like this one. www.google.com and also this one http://www.something.com. This is some extra text hi mom.";

            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.ParseEmail(testMessage);
            
            Assert.IsNull(result);
        }
        
        [Test]
        public void ParseEmail_Invalid_EmailMessageLong_Return_Null()
        {
            string messageHeader = "E192837465";
            string messageBody = "Test-mail@email.com\nthis is 20 character\nThis is the start of the email message text URL's" +
                                 " should be quarantined like this one. www.google.com and also this one http://www.something.com. This is some extra text hi mom.\nMore Text. More Text. " +
                                 "More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. " +
                                 "More Text. More Text. More Text. More Text.\nMore Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text." +
                                 " More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text.\nMore Text. More Text. More Text. More Text." +
                                 " More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text." +
                                 " More Text.\nMore Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. More Text. " +
                                 "More Text. More Text. More Text. More Text. Mor 1029 charac";
            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.ParseEmail(testMessage);
            
            Assert.IsNull(result);
        }
        
        [Test]
        public void ParseTweet_Valid_Tweet_Return_Instance_Tweet()
        {
            string messageHeader = "T192837465";
            string messageBody = "@(15 characters)\nThis is the message text for the sms message LOL. #Trendy @Conor #WishThisWorks this is 140 #WishThisWorks 140 140 140 140 #EndOfTheLineeeee";
            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.ParseTweet(testMessage);

            Assert.IsInstanceOf<Tweet>(result);
        }
        
        [Test]
        public void ParseTweet_Invalid_TweetTwitterID_Return_Null()
        {
            string messageHeader = "T192837465";
            string messageBody = "(15 characters)\nThis is the message text for the sms message LOL. #Trendy @Conor #WishThisWorks this is 140 #WishThisWorks 140 140 140 140 #EndOfTheLineeeee";
            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.ParseTweet(testMessage);

            Assert.IsNull(result);
        }
        
        [Test]
        public void ParseTweet_Invalid_TweetTwitterIDLong_Return_Null()
        {
            string messageHeader = "T192837465";
            string messageBody = "@(16 characters-)\nThis is the message text for the sms message LOL. #Trendy @Conor #WishThisWorks this is 140 #WishThisWorks 140 140 140 140 #EndOfTheLineeeee";
            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.ParseTweet(testMessage);

            Assert.IsNull(result);
        }
        
        [Test]
        public void ParseTweet_Invalid_TweetMessageLong_Return_Null()
        {
            string messageHeader = "T192837465";
            string messageBody = "@(15 characters)\nThis is the message text for the sms message LOL. #Trendy @Conor #WishThisWorks this is 140 #WishThisWorks 140 140 140 140 #EndOfTheLineeeee-";
            Message testMessage = new Message(messageHeader, messageBody);

            var result = _controller.ParseTweet(testMessage);

            Assert.IsNull(result);
        }
        
        [Test]
        public void GetTextspeak_OpenFile_Return_Dict_StringString()
        {
            string fileName = "textwords.csv";

            var result = _controller.GetTextspeak(fileName);
            
            Assert.IsNotEmpty(result);
            Assert.IsInstanceOf<Dictionary<string, string>>(result);
        }

        [Test]
        [TestCase("COB", "LOL", "IRL")]
        [TestCase("TAM", "TBH", "UL")]
        [TestCase("IMO", "JK", "SLAP")]
        [TestCase("YMMV", "PDQ", "OTL")]
        [TestCase("PTL", "QIK", "SMHID")]
        public void TextspeakParser_Abbreviations_Return_String(string abbrev1, string abbrev2, string abbrev3)
        {
            string messageText = $"This is a test {abbrev1} message for a SMS {abbrev2} message and is below {abbrev3} 140 characters";
            var textspeakAbrv = _controller.GetTextspeak("textwords.csv");
            string expected = $"This is a test {abbrev1} <{textspeakAbrv[abbrev1]}> message for a SMS {abbrev2} <{textspeakAbrv[abbrev2]}> message and is below {abbrev3} <{textspeakAbrv[abbrev3]}> 140 characters";
            

            string result = _controller.TextspeakParser(messageText);

            Assert.AreEqual(expected, result);
        }
        
        [Test]
        [TestCase("COB", "LOL", "IRL")]
        [TestCase("TAM", "TBH", "UL")]
        [TestCase("IMO", "JK", "SLAP")]
        [TestCase("YMMV", "PDQ", "OTL")]
        [TestCase("PTL", "QIK", "SMHID")]
        public void TextspeakParser_StartMiddleEnd_Return_String(string abbrev1, string abbrev2, string abbrev3)
        {
            string messageText = $"{abbrev1} This is a test message for a SMS {abbrev2} message and is below 140 characters {abbrev3}";
            var textspeakAbrv = _controller.GetTextspeak("textwords.csv");
            string expected = $"{abbrev1} <{textspeakAbrv[abbrev1]}> This is a test message for a SMS {abbrev2} <{textspeakAbrv[abbrev2]}> message and is below 140 characters {abbrev3} <{textspeakAbrv[abbrev3]}>";
            

            string result = _controller.TextspeakParser(messageText);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void UrlQuarantine_Three_Url_Return_String()
        {
            string messageText = $"This is a test message www.google.com for a Email https://www.ThisisaWebsite.co.uk message and is DifferentWebsite.com testing the url quarantine";
            string expected = $"This is a test message <URL Quarantined> for a Email <URL Quarantined> message and is <URL Quarantined> testing the url quarantine";
            

            string result = _controller.UrlQuarantine(messageText);

            Assert.AreEqual(expected, result);
        }
        
        [Test]
        public void UrlQuarantine_StartMiddleEnd_Return_String()
        {
            string messageText = $"https://www.Startofthestring.com This is a test message for a Email MiddleoftheString.ac.uk message and is testing the url quarantine www.EndofLine.com";
            string expected = $"<URL Quarantined> This is a test message for a Email <URL Quarantined> message and is testing the url quarantine <URL Quarantined>";
                

            string result = _controller.UrlQuarantine(messageText);

            Assert.AreEqual(expected, result);
        }



        [Test]
        public void FormatMessageText_Valid_Return_String()
        {
            string messageBody = "Test-mail@email.com\nSIR 22/22/22\nSort Code: 99-99-99\nNature of Incident: Staff Attack\nThere" +
                                 " was an attack at the store a member of staff was injured. Please help.\nNew Line :) www.google.com\nAnd another\none more https://www.hi.co.uk";
            char[] delims = { '\r', '\n'};
            var lines = messageBody.Split(delims);
            string expected = "There was an attack at the store a member of staff was injured. Please help.\nNew Line :) www.google.com\nAnd another\none more https://www.hi.co.uk";

            string result = _controller.FormatMessageText(lines, 4);
            
            Assert.AreEqual(expected, result);
        }
    }
}