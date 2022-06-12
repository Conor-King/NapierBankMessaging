namespace NapierBankMessaging.Classes
{
    /// <summary>
    /// Author: Conor King - 40532386
    /// Class <c>EmailSIR</c> stores the information of the message if it's an email with a significant incident report.
    /// This class is a subclass of <c>Message</c> class.
    /// </summary>
    public class EmailSIR : Message
    {
        private string Sender { get; }
        private string SIRDate { get; }
        private string SortCode { get; }
        private string NatureOfIncident { get; }
        private string MessageText { get; }
        
        public EmailSIR(Message message, string sender, string sirDate, string sortCode, string natureOfIncident, string sirText) : base(message.MessageHeader, message.MessageBody)
        {
            Sender = sender;
            SIRDate = sirDate;
            SortCode = sortCode;
            NatureOfIncident = natureOfIncident;
            MessageText = sirText;
        }

        public override string ToString()
        {
            return "------Email SIR------\n" +
                   $"Sender: {Sender}\n" +
                   $"SIR Date: {SIRDate}\n" +
                   $"Sort Code: {SortCode}\n" +
                   $"Nature of Incident: {NatureOfIncident}\n" +
                   $"Message Text: {MessageText}";
        }
    }
}