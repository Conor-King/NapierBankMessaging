namespace NapierBankMessaging.Classes
{
    /// <summary>
    /// Author: Conor King - 40532386
    /// Class <c>Tweet</c> stores the information of the message if it's a tweet message input into the system.
    /// This class is a subclass of <c>Message</c> class.
    /// </summary>
    public class Tweet : Message
    {
        private string TwitterID { get; }
        private string MessageText { get; }
        
        public Tweet(Message message, string twitterId, string messageText) : base(message.MessageHeader, message.MessageBody)
        {
            TwitterID = twitterId;
            MessageText = messageText;
        }
        
        public override string ToString()
        {
            return "------Tweet------\n" +
                   $"Twitter ID: {TwitterID}\n" +
                   $"Message Text: {MessageText}";
        }
    }
}