using System;
using System.Collections.Generic;
using System.Linq;

namespace NapierBankMessaging.Classes
{
    /// <summary>
    /// Author: Conor King - 40532386
    /// Class <c>MessageInfo</c> stores the logged information in the system.
    /// This includes all messages, trending list, mentions list, and significant incident reports input into the system.
    /// </summary>
    public class MessageInfo
    {
        public List<Message> AllMessages { get; }
        public Dictionary<string, int> TrendingList { get; }
        public Dictionary<string, int> MentionsList { get; }
        public Dictionary<string, string> SIRIncidentList { get; }

        public MessageInfo()
        {
            AllMessages = new List<Message>();
            TrendingList = new Dictionary<string, int>();
            MentionsList = new Dictionary<string, int>();
            SIRIncidentList = new Dictionary<string, string>();

        }

        public MessageInfo(Dictionary<string, int> trendingList, Dictionary<string, int> mentionsList, Dictionary<string, string> sirIncidentList)
        {
            TrendingList = trendingList;
            MentionsList = mentionsList;
            SIRIncidentList = sirIncidentList;
        }

        /// <summary>
        /// Method <c>AddMessage</c> adds the input message and adds it to the all message list.
        /// </summary>
        /// <param name="message">A Message object</param>
        public void AddMessage(Message message)
        {
            AllMessages.Add(message);
        }

        /// <summary>
        /// Method <c>AddTrendingList</c> checks to see if the hashtag is already in the dictionary if so it add one
        /// to the count. If not then it will add the hashtag to the dictionary.
        /// </summary>
        /// <param name="hashtag">A string with a hashtag from a tweet.</param>
        public void AddTrendingList(string hashtag)
        {
            if (TrendingList.ContainsKey(hashtag))
            {
                TrendingList[hashtag] += 1;
            }
            else
            {
                TrendingList.Add(hashtag, 1);
            }
        }
        
        /// <summary>
        /// Method <c>AddMentionsList</c> checks to see if the twitter id is already in the dictionary if
        /// so it will add one to the count. if not it will add the twitter id.
        /// </summary>
        /// <param name="twitterId">A string with a twitter id mentioned in a tweet.</param>
        public void AddMentionsList(string twitterId)
        {
            if (MentionsList.ContainsKey(twitterId))
            {
                MentionsList[twitterId] += 1;
            }
            else
            {
                MentionsList.Add(twitterId, 1);
            }
        }
        
        /// <summary>
        /// Method <c>AddSIRIncidentList</c> checks if the sort code is already in the dictionary
        /// if so it will return "exists", if not it will add the sort code and nature of incident to
        /// the dictionary.
        /// </summary>
        /// <param name="sortCode">A string with a sort code from a SIR email</param>
        /// <param name="natureOfIncident">A string with a nature of incident from a SIR email</param>
        /// <returns></returns>
        public string AddSIRIncidentList(string sortCode, string natureOfIncident)
        {
            if (SIRIncidentList.ContainsKey(sortCode))
            {
                return "Exists";
            }
            else
            {
                SIRIncidentList.Add(sortCode, natureOfIncident);
            }

            return null;
        }
        
        public override string ToString()
        {
            string fullTrendingList = string.Join(Environment.NewLine, TrendingList.Select(item => $"{item.Key}: {item.Value}"));
            string fullMentionsList = string.Join(Environment.NewLine, MentionsList.Select(item => $"{item.Key}: {item.Value}"));
            string fullSirList = string.Join(Environment.NewLine, SIRIncidentList.Select(item => $"{item.Key}: {item.Value}"));

            return $"Trending List: {fullTrendingList} \n Mentions List: {fullMentionsList} \n SIR Incident List: {fullSirList}";
        }
    }
}