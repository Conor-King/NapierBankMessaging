using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using Microsoft.Win32;
using NapierBankMessaging.Pages;
using Newtonsoft.Json;

namespace NapierBankMessaging.Classes
{
    /// <summary>
    /// Author: Conor King - 40532386
    /// Class <c>Controller</c> is the central system for this application.
    /// This class handles the movement and transformation of the data provided by the user.
    /// </summary>
    public class Controller
    {
        private readonly MessageInfo _messageInfo = new MessageInfo();

        /// <summary>
        /// Method <c>MainMethod</c> works as the central method which controls the flow
        /// of the system.
        /// </summary>
        /// <param name="messageHeader">A string with the user input message header</param>
        /// <param name="messageBody">A string with the user input message body</param>
        public void MainMethod(string messageHeader, string messageBody)
        {
            var validatedMessage = ValidateMessage(messageHeader, messageBody);
            if (validatedMessage != null)
            {
                string messageType = DetectMessageType(validatedMessage);
                if (messageType != null)
                {
                    Message parsedMessage = null;
                    switch (messageType)
                    {
                        case "SMS":
                            parsedMessage = ParseSMS(validatedMessage);
                            break;
                        case "Email":
                            parsedMessage = ParseEmail(validatedMessage);
                            break;
                        case "Tweet":
                            parsedMessage = ParseTweet(validatedMessage);
                            break;
                    }

                    if (parsedMessage != null)
                    {
                        LogMessage(parsedMessage);
                    }
                }
            }
        }
        
        /// <summary>
        /// Method <c>ValidateMessage</c> checks the input message header and message body to make
        /// sure the input is a valid message with no input errors.
        /// </summary>
        /// <param name="messageHeader">A string with the user input message header</param>
        /// <param name="messageBody">A string with the user input message body</param>
        /// <returns>A new instance of the Message class</returns>
        public Message ValidateMessage(string messageHeader, string messageBody)
        {
            var notNumber = false;
            
            for (var startIndex = 1; startIndex < messageHeader.Length; startIndex++)
            {
                if (!Char.IsNumber(messageHeader[startIndex]))
                {
                    notNumber = true;
                }
            }

            if (notNumber)
            {
                MessageBox.Show("Message Header should only contain numbers after the first character. (Example: E123456789)",
                    "Input Error",MessageBoxButton.OK,MessageBoxImage.Exclamation);
            }
            else
            {
                if (string.IsNullOrEmpty(messageHeader) || string.IsNullOrEmpty(messageBody))
                {
                    MessageBox.Show("Message Header or Message Body cannot be empty! Please try again.",
                        "Input Error", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                else if (messageHeader.Length != 10)
                {
                    MessageBox.Show(
                        $"This message header \"{messageHeader}\" should be 10 characters long. (Example: E123456789)",
                        "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (messageHeader[0] != 'S' && messageHeader[0] != 'E' && messageHeader[0] != 'T')
                {
                    MessageBox.Show(
                        $"This message header \"{messageHeader}\" must have a message identifier as the first character 'S', 'E', or 'T'.",
                        "Input Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    var message = new Message(messageHeader, messageBody);
                    return message;
                }
            }

            return null;
        }

        /// <summary>
        /// Method <c>DetectMessageType</c> detects the type of message input into the system.
        /// Either SMS message, Email message, and Tweet message.
        /// </summary>
        /// <param name="message">Instance of class Message with a message header and message body.</param>
        /// <returns> A string with the value of the message type.</returns>
        public string DetectMessageType(Message message)
        {
            char type = message.MessageHeader[0];
            switch (type)
            {
                case 'S':
                    return "SMS";
                case 'E':
                    return "Email";
                case 'T':
                    return "Tweet";
                default:
                    return null;
            }
        }

        /// <summary>
        /// Method <c>ParseSMS</c> validates the format of the message to match sms message.
        /// </summary>
        /// <param name="message">Instance of class Message with a message header and message body.</param>
        /// <returns>The parsed SMS message</returns>
        public SMS ParseSMS(Message message)
        {
            char[] delims = { '\r', '\n' };
            string[] words = message.MessageBody.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length < 2)
            {
                MessageBox.Show("The format of message body is incorrect for an SMS. \n Format = \n Sender International Phone Number.\n Message Text.", "Format Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                var sender = words[0];
                var messageText = FormatMessageText(words, 1);

                if (messageText.Length > 140)
                {
                    MessageBox.Show("This SMS has more characters than the limit of 140 for the message text (Line 2+) in this message.", "Input Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    
                    messageText = TextspeakParser(messageText);

                    var smsMessage = new SMS(message, sender, messageText);
                    return smsMessage;
                }
            }

            return null;
        }

        /// <summary>
        /// Method <c>ParseEmail</c> validates the format of the message to match email message.
        /// </summary>
        /// <param name="message">Instance of class Message with a message header and message body.</param>
        /// <returns>The parsed Email message.</returns>
        public Message ParseEmail(Message message)
        {
            char[] delims = new[] { '\r', '\n' };
            string[] words = message.MessageBody.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            if (words.Length < 3)
            {
                MessageBox.Show("The format of message body is incorrect for an email. \n See attached report for clarification.", 
                    "Format Error", MessageBoxButton.OK, MessageBoxImage.Stop);
            }
            else
            {
                var sender = words[0];
                string subject = words[1];
                string messageText = FormatMessageText(words, 2);

                if (subject.Length > 20)
                {
                    MessageBox.Show("This email has more characters than the limit 20 for the subject (line 2) in this message", "Input Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (messageText.Length > 1028) 
                {
                    MessageBox.Show("This email has more characters than the limit of 1028 for the message text (Line 3+) in this message.", "Input Error",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    string sirTest = subject.Substring(0, 3);
                    
                    if (sirTest == "SIR")
                    {
                        string sortCode = words[2];
                        string natureOfIncident = words[3];

                        var sirText = new StringBuilder();
                        for (var startIndex = 4; startIndex < words.Length; startIndex++)
                        {
                            sirText.Append(words[startIndex] + "\n");
                        }

                        var sortCodeTest = sortCode.Split(':');
                        if (sortCodeTest.Length > 1)
                        {
                            sortCode = sortCode.Split(':')[1].Trim();
                        }

                        var subjectTest = subject.Split(' ');
                        if (subjectTest.Length > 1)
                        {
                            subject = subject.Split(' ')[1];
                        }

                        var noiTest = natureOfIncident.Split(':');
                        if (noiTest.Length > 1)
                        {
                            natureOfIncident = natureOfIncident.Split(':')[1].Trim();
                        }
                       
                        _messageInfo.AddSIRIncidentList(sortCode, natureOfIncident);
                        
                        string urlSafeText = UrlQuarantine(sirText.ToString());

                        var sirEmail = new EmailSIR(message, sender, subject, sortCode, natureOfIncident, urlSafeText);

                        return sirEmail;

                    }
                    else
                    {
                        string urlSafeText = UrlQuarantine(messageText);
                        var standardEmail = new Email(message, sender, subject, urlSafeText);
                        return standardEmail;
                    }
                }
            }

            return null;
        }
        
        /// <summary>
        /// Method <c>ParseTweet</c> validates the format of the message to match tweet message.
        /// </summary>
        /// <param name="message">Instance of class Message with a message header and message body.</param>
        /// <returns>The parsed Tweet message.</returns>
        public Tweet ParseTweet(Message message)
        {
            var delims = new[] { '\r', '\n' };
            var words = message.MessageBody.Split(delims, StringSplitOptions.RemoveEmptyEntries);

            string twitterID = words[0];
            string messageText = FormatMessageText(words, 1);

            if (twitterID[0] != '@')
            {
                MessageBox.Show("This tweet does not have an \"@\" symbol at the front of the twitter ID (Line 1) for this message.", "Input Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            } 
            else if (twitterID.Length > 16)
            {
                MessageBox.Show("This tweet has more characters than the limit of 15 for the twitter ID (Line 1) for this message.", "Input Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            } 
            else if (messageText.Length > 140)
            {
                MessageBox.Show("This tweet has more characters than the limit of 140 for the message text (Line 2+) in this message.", "Input Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                char[] delimsWithSpace = new[] { '\r', '\n', ' ' };
                string[] messageTextArray = messageText.Split(delimsWithSpace);

                foreach (string word in messageTextArray)
                {
                    if (word[0] == '@')
                    {
                        _messageInfo.AddMentionsList(word);
                    }
                    else if (word[0] == '#')
                    {
                        _messageInfo.AddTrendingList(word);
                    }
                }
                
                messageText = TextspeakParser(messageText);
                var tweetMessage = new Tweet(message, twitterID, messageText);
                return tweetMessage;
            }

            return null;
        }
        
        /// <summary>
        /// Method <c>GetTextspeak</c> Opens and reads the file then puts each value pair
        /// into the dictionary to return.
        /// </summary>
        /// <param name="filename">String with the filename to be opened.</param>
        /// <returns>A dictionary with each abbreviation and expanded version.</returns>
        public Dictionary<string, string> GetTextspeak(string filename)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\" + filename;
            using (var streamReader = new StreamReader(path))
            {
                var textspeakAbrv = new Dictionary<string, string>();

                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    if (line != null)
                    {
                        var entry = line.Split(',');
                        textspeakAbrv.Add(entry[0], entry[1]);
                    }
                }
                return textspeakAbrv;
            }
        }

        /// <summary>
        /// Method <c>TextspeakParser</c> looks throught the input message text to find any abbreviations
        /// then once found will get the expanded version and insert it after the abbreviation.
        /// </summary>
        /// <param name="messageText">A string holding the raw message text.</param>
        /// <returns>A string with the new message text textspeak expanded.</returns>
        public string TextspeakParser(string messageText)
        {
            var textspeakAbrv = GetTextspeak("textwords.csv");
            var count = 0;
            
            var splitWords = messageText.Split(' ');
            var messageWords = new List<string>();
            var wordsWithoutPunctuation = new List<string>();

            foreach (var word in splitWords)
            {
                if (!string.IsNullOrEmpty(word) && char.IsPunctuation(word[word.Length - 1]))
                {
                    string newWord = word.Remove(word.Length - 1, 1);
                    messageWords.Add(newWord);
                    messageWords.Add(word[word.Length - 1].ToString());
                }
                else
                {
                    messageWords.Add(word);
                }

                var stringbuilder = new StringBuilder();
                foreach (var c in word)
                {
                    if (!char.IsPunctuation(c))
                    {
                        stringbuilder.Append(c);
                    }
                }
                
                wordsWithoutPunctuation.Add(stringbuilder.ToString());
            }

            foreach (var word in wordsWithoutPunctuation)
            {
                if (textspeakAbrv.ContainsKey(word))
                {
                    messageWords.Insert(count + 1, $"<{textspeakAbrv[word]}>");
                    count += 1;
                }
                count += 1;
            }

            var stringBuilder = new StringBuilder();
            foreach (var word in messageWords)
            {
                if (string.IsNullOrEmpty(stringBuilder.ToString()) || word == "." || word == ",")
                {
                    stringBuilder.Append(word);
                }
                else
                {
                    stringBuilder.Append(" ");
                    stringBuilder.Append(word);
                }
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Method <c>UrlQuarantine</c> takes in a message text and looks through each word to see
        /// if any match the regex pattern specialised for matching URL addresses.
        /// </summary>
        /// <param name="messageText">A string holding the raw message text</param>
        /// <returns>A string with the new message text with URL address quarantined.</returns>
        public string UrlQuarantine(string messageText)
        {
            char[] delims = { '\r', '\n', ' ' };
            var splitWords = messageText.Split(delims);
            var stringBuilder = new StringBuilder();
            string pattern = @"^(http|https|ftp|)\://|[a-zA-Z0-9\-\.]+\.[a-zA-Z](:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\-\._\?\,\'/\\\+&amp;%\$#\=~])*[^\.\,\)\(\s]$";
            Regex regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

            foreach (var word in splitWords)
            {
                if (string.IsNullOrEmpty(stringBuilder.ToString()))
                {
                    stringBuilder.Append(regex.IsMatch(word) ? "<URL Quarantined>" : word);
                }
                else
                {
                    if (regex.IsMatch(word))
                    {
                        stringBuilder.Append(" ");
                        stringBuilder.Append("<URL Quarantined>");
                    }
                    else
                    {
                        stringBuilder.Append(" ");
                        stringBuilder.Append(word);
                    }
                }
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Method <c>FormatMessageText</c> will take in a string array of sentences and a starting index for a loop.
        /// Then it will create a string from all the sentences in the array from the starting index.
        /// Reinserting the new line where it was previously to format the message.
        /// </summary>
        /// <param name="sentence">A string array with a few sentences</param>
        /// <param name="startIndex">The start index for the loop.</param>
        /// <returns>The formatted message text</returns>
        public string FormatMessageText(string[] sentence, int startIndex)
        {
            var messageTextBuilder = new StringBuilder();
            for (var i = startIndex; i < sentence.Length; i++)
            {
                if (i == sentence.Length - 1)
                {
                    messageTextBuilder.Append(sentence[i]);
                }
                else
                {
                    messageTextBuilder.Append(sentence[i] + "\n");
                }
            }
            return messageTextBuilder.ToString();
        }

        /// <summary>
        /// Method <c>LogMessage</c> takes in the parsed message and adds it to the list of all messages.
        /// </summary>
        /// <param name="parsedMessage">A string with the parsed message text</param>
        public void LogMessage(Message parsedMessage)
        {
            MessageBox.Show(parsedMessage.ToString(),$"Parsed {parsedMessage.GetType()} Message.", MessageBoxButton.OK, MessageBoxImage.Question);
            _messageInfo.AddMessage(parsedMessage);
        }

        /// <summary>
        /// Method <c>InputFile</c> takes in the path of the file the user has selected.
        /// Then opens and reads the messages one by one running each through the system until
        /// all messages are parsed.
        /// </summary>
        /// <param name="path">A string with the path to the file.</param>
        public void InputFile(string path)
        {
            using (var streamReader = new StreamReader(path))
            {
                string messageHeader = null;

                while (!streamReader.EndOfStream)
                {
                    var line = streamReader.ReadLine();
                    if (!string.IsNullOrEmpty(line))
                    {
                        messageHeader = line;
                    }

                    // Empty line in the format
                    line = streamReader.ReadLine();
                    
                    // Loop through each message text line.
                    line = streamReader.ReadLine();
                    var stringBuilder = new StringBuilder();
                    while (!string.IsNullOrEmpty(line))
                    {
                        stringBuilder.Append(line);
                        line = streamReader.ReadLine();
                        if (!string.IsNullOrEmpty(line))
                        {
                            stringBuilder.Append("\n");
                        }
                    }

                    string messageBody = stringBuilder.ToString();
                    
                    MainMethod(messageHeader, messageBody);
                }
            }
        }

        /// <summary>
        /// Method <c>SerialiseToJSON</c> takes in a list of messages. Then opens the save file dialog
        /// for the user to select a file name and directory. Afterwards it will serialize each message
        /// into the selected file.
        /// </summary>
        /// <param name="allMessages">A list of all input messages.</param>
        /// <returns>A boolean where true means the serialization was successful.</returns>
        public bool SerialiseToJSON(List<Message> allMessages)
        {
            
            if (allMessages.Count == 0)
            {
                MessageBox.Show("There have been no message input into the system.");
                return false;
            }
            else
            {
                SaveFileDialog saveFile = new SaveFileDialog();
                saveFile.Title = "Save messages to json file";
                saveFile.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt";
                if (saveFile.ShowDialog() == true)
                {
                    foreach (var obj in allMessages)
                    {
                        // Serialize each message to the selected file.
                        File.AppendAllText(saveFile.FileName, JsonConvert.SerializeObject(obj, Formatting.Indented));
                    }

                    MessageBox.Show("Messages Successfully Serialized", "Saved!", MessageBoxButton.OK, MessageBoxImage.Information);
                    return true;
                }
                else
                {
                    MessageBox.Show("Save Cancelled!", "Cancel Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
        }

        /// <summary>
        /// Method <c>ViewChange</c> controls the view switching in the system. Takes the input action
        /// and runs the specific switch case depending on the action.
        /// </summary>
        /// <param name="mainWindow">The MainWindow class instance</param>
        /// <param name="controller">The Controller class instance</param>
        /// <param name="action">A string with the action of the function.</param>
        public void ViewChange(MainWindow mainWindow, Controller controller, string action)
        {
            switch (action)
            {
                case "InputPage":
                    mainWindow.Content = new InputPage(mainWindow, this);
                    break;
                case "ListPage":
                    mainWindow.Content = new ListPage(mainWindow, this, _messageInfo);
                    break;
                case "Close":
                    var result = SerialiseToJSON(_messageInfo.AllMessages);
                    if (result)
                    {
                        mainWindow.Close();
                    }
                    break;
            }
        }
    }
}