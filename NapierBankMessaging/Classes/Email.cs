namespace NapierBankMessaging.Classes
{
    /// <summary>
    /// Author: Conor King - 40532386
    /// Class <c>Email</c> stores the information of the message if it's an email.
    /// This class is a subclass of <c>Message</c> class.
    /// </summary>
    public class Email : Message
    {
        private string Sender { get; }
        private string Subject { get; }
        private string MessageText { get; }
        public Email(Message message, string sender, string subject, string messageText) : base(message.MessageHeader, message.MessageBody)
        {
            Sender = sender;
            Subject = subject;
            MessageText = messageText;
        }
        
        public override string ToString()
        {
            return "------Email------\n" +
                   $"Sender: {Sender}\n" +
                   $"Subject: {Subject}\n" +
                   $"Message Text: {MessageText}";
        }
    }
}