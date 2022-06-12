namespace NapierBankMessaging.Classes
{
    /// <summary>
    /// Author: Conor King - 40532386
    /// Class <c>SMS</c> stores the information of the sms messages input into the system.
    /// This class is a subclass of <c>Message</c> class.
    /// </summary>
    public class SMS : Message
    {
        private string Sender { get; }
        private string MessageText { get; }
        public SMS(Message message, string sender, string messageText) : base(message.MessageHeader, message.MessageBody)
        {
            Sender = sender;
            MessageText = messageText;
        }
        
        public override string ToString()
        {
            return "------SMS------\n" +
                   $"Sender: {Sender}\n" +
                   $"Message Text: {MessageText}";
        }
    }
}