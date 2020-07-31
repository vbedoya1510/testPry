using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

namespace CallCenterProyect.Models
{
    public class Process
    {

        /// <summary>
        /// partition the text into conversations and if each have the keyword count your points
        /// </summary>
        /// <returns> Object Answer </returns>
        public Answer getPoints(string file)
        {
            try
            {
                Answer answer = new Answer();
                answer.errors = "";
                answer.successful = true;
                answer.result = 0;

                int totalPoints = 0;
                string[] conversations = file.Split(new string[] { "\r\n\r\n" }, StringSplitOptions.None);
                //get keyword to evalue each conversation
                string keyword = WebConfigurationManager.AppSettings["keyWordToEvaluate"].ToString();

                foreach (var conversation in conversations)
                {
                    if(conversation.Contains(keyword))
                        totalPoints += evalueConversation(conversation.ToUpper());
                }
                answer.result = totalPoints;
                return answer;
            }
            catch (Exception e)
            {
                Answer answer = new Answer();
                answer.errors = "Error al momento de evaluar el las conversaciones enviadas "+e.Message;
                answer.successful = false;
                answer.result = 0;
                return answer;
            }
        }

        /// <summary>
        /// call differents methods to get points 
        /// </summary>
        /// <returns> total points </returns>
        private int evalueConversation(string conversation)
        {
            int totalPoints;
            string wordExcelent = WebConfigurationManager.AppSettings["ExcellentService"].ToString();
            totalPoints = wordExcellentEvalue(conversation, wordExcelent);
            if(totalPoints != 100) {
                string wordUrgent = WebConfigurationManager.AppSettings["wordUrgent"].ToString();
                totalPoints += getLineNumber(conversation);                
                totalPoints += wordUrgentEvalue(conversation, wordUrgent);
                totalPoints += listWordEvalue(conversation);
                totalPoints += timeEvalue(conversation);
            }
            return totalPoints;  
        }

        /// <summary>
        /// get points for number lines
        /// </summary>
        /// <returns> points for number lines </returns>
        private int getLineNumber(string conversation)
        {
            int total = (Regex.Matches(conversation, "\r\n").Count);
            int points = 0;

            if (total == 1)            
                points+= -100;
            
            if (total>5)
            {
                points+= 10;
            }
            else
            {
                points+= 20;
            }
            return points;
        }

        /// <summary>
        /// get points for word Excellent
        /// </summary>
        /// <returns> points for word Excellent </returns>
        private int wordExcellentEvalue(string conversation, string word)
        {          
            if (conversation.Contains(word))
                return 100;
            return 0;
        }

        /// <summary>
        /// get points for word Urgent
        /// </summary>
        /// <returns> points for word Urgent </returns>
        private int wordUrgentEvalue(string conversation, string word)
        {
            int total = Regex.Matches(conversation, word).Count; 
            if (total <= 2)            
                return -5;
            return -10;            
        }

        /// <summary>
        /// if one of the saved words is in the conversation, it gets points
        /// </summary>
        /// <returns> points seved words in configuration </returns>
        private int listWordEvalue(string conversation)
        {
            string[] words = WebConfigurationManager.AppSettings["goodWords"].Split(';');

            int i = 0;
            while (i < words.Length) {
                if(Regex.Matches(conversation, words[i]).Count > 0)
                {
                    return 10;
                }
                i++;
            }

            return 0;
        }

        /// <summary>
        /// get point for time in the conversation
        /// </summary>
        /// <returns> points for time </returns>
        private int timeEvalue(string conversation)
        {

            //delete blanc spaces
            char[] charsToTrim = { '\r','\n', ' '};
            conversation = conversation.Trim(charsToTrim);
            
            string[] lines = conversation.Split('\n');

            int posIni = lines[1].IndexOf(' ');
            int posfin = lines[lines.Length - 1].IndexOf(' ');       

            DateTime date1 = Convert.ToDateTime(lines[1].Substring(0, posIni));
            DateTime date2 = Convert.ToDateTime(lines[lines.Length-1].Substring(0,posfin));

            double seconds = date2.Subtract(date1).TotalSeconds;
            if (seconds == 0)
                return 0;

            if (seconds < 60)
                return 50;
            return 25;
        }



    }
}