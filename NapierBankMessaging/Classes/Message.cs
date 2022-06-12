namespace NapierBankMessaging.Classes
{
    /// <summary>
    /// Author: Conor King - 40532386
    /// Class <c>Message</c> stores the information of the input message header and message body from the user.
    /// This class is a superclass for <c>SMS, Email, EmailSIR, Tweet</c> classes.
    /// </summary>
    public class Message
    {
        public string MessageHeader { get; }
        public string MessageBody { get; }

        public Message(string messageHeader, string messageBody)
        {
            this.MessageHeader = messageHeader;
            this.MessageBody = messageBody;
        }
        
        public override string ToString()
        {
            return $"Message Header: {MessageHeader}\n" +
                   $"Message Body: {MessageBody}\n";
        }
    }
}