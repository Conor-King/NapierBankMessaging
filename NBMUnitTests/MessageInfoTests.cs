using NUnit.Framework;
using NapierBankMessaging.Classes;


namespace NBM_Unit_Tests
{
    [TestFixture]
    public class MessageInfoTests
    {
        private MessageInfo _messageInfo = new MessageInfo();
        
        [Test]
        public void AddMessage_Input_To_List()
        {
            var expected = new SMS(new Message("S123456789", "+447561017503\nThis is the message text for the sms message LOL. I am also" +
                                                                " testing the textspeak abbreviations ROFL."), "+447561017503", "This is the message " +
                                                                "text for the sms message LOL <Laughing out loud>. I am also testing the textspeak abbreviations ROFL.");

            _messageInfo.AddMessage(expected);
            var result = _messageInfo.AllMessages[0];
            
            Assert.AreEqual(expected.ToString(),result.ToString());
        }
        
        [Test]
        public void AddTrendingList_Input_To_Dictionary()
        {
            string hashtag = "#TrendingTest";
            
            _messageInfo.AddTrendingList(hashtag);
            
            Assert.True(_messageInfo.TrendingList.ContainsKey(hashtag));
        }
        
        [Test]
        public void AddMentionsList_Input_To_Dictionary()
        {
            string mention = "@twitterID";
            
            _messageInfo.AddMentionsList(mention);
            bool result = _messageInfo.MentionsList.ContainsKey(mention);
            
            Assert.IsTrue(result);
        }
        
        [Test]
        public void AddSIRIncidentList_ValidInput_To_Dictionary_Returns_Null()
        {
            string sortCode = "22-33-44";
            string natureOfIncident = "Bomb Threat";
            
            _messageInfo.AddSIRIncidentList(sortCode, natureOfIncident);
            var result = _messageInfo.SIRIncidentList.ContainsKey(sortCode);

            Assert.IsTrue(result);
            
        }
        
        [Test]
        public void AddSIRIncidentList_ExistsAlready_Returns_String()
        {
            string sortCode = "12-34-56";
            string natureOfIncident = "Bomb Threat";
            string expected = "Exists";
            
            _messageInfo.AddSIRIncidentList(sortCode, natureOfIncident);
            var result = _messageInfo.AddSIRIncidentList(sortCode, natureOfIncident);
            
            Assert.AreEqual(expected, result);
        }
        
    }
    
}