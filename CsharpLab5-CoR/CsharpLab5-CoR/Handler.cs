using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
namespace CsharpLab5_CoR
{
    /// <summary>
    /// Interface <c>IHandler</c> provides methods to handle incoming emails
    /// </summary>
    interface IHandler
    {
        /// <summary>
        /// Method <c>setNext</c> sets successor to handle request next.
        /// </summary>
        /// <param name="h">handler to be executed next</param>
        void setNext(IHandler h);
        /// <summary>
        /// Method <c>Handle</c> handles incoming email, 
        /// else passes it to the next successor of the handler chain
        /// </summary>
        /// <param name="email">email to handle</param>
        void Handle(Email email);
    }
    /// <summary>
    /// Class <c>BaseHandler</c> provides methods and properties 
    /// to checks incoming emails for spam.
    /// </summary>
    abstract class BaseHandler : IHandler
    {
        private IHandler next;
        protected static double spamScore = 0;
        /// <summary>
        /// Method <c>setNext</c> sets handler to be executed next.
        /// </summary>
        /// <param name="h">next handler</param>
        public void setNext(IHandler h)
        {
            next = h;
        }
        /// <summary>
        /// Method <c>Handle</c> checks spam score and, if 
        /// it is over trust limit, moves email to spam,  
        /// else passes it to the next handler in the chain.
        /// </summary>
        /// <param name="email"></param>
        public virtual void Handle(Email email)
        {
            //Console.WriteLine("spamScore " + spamScore);//
            if (spamScore > Globals.trustLimit)
            {
                Console.WriteLine("Email moved to SPAM");
                spamScore = 0;
            }
            else
                if (next != null)
            { 
                next.Handle(email);
            }
            else
            {
                Console.WriteLine("Email checked");
                spamScore = 0;
            }
                
        }
    }
    /// <summary>
    /// Class <c>AttachmentChecker</c> provides methods and properties to check incoming email for suspicious/virus attachment.
    /// </summary>
    class AttachmentChecker : BaseHandler
    {
        /// <summary>
        /// Method <c>Handle</c> checks if there is an attachment and if it contains suspicious files moves email to junk.
        /// </summary>
        /// <param name="email">email to check</param>
        public override void Handle(Email email)
        {
            Console.WriteLine("AttachmentChecker : execution started");
            //
            if ((email.Attachment != null) && (CheckAttachment(email.Attachment)))
            {
                Console.WriteLine("Email moved to JUNK");//spamScore += Globals.trustLimit+0.01;
            }   
            else      
                base.Handle(email);      
            //Console.WriteLine("handled");
            //Console.WriteLine("spamScore " + spamScore);
            
        }
        private bool CheckAttachment(object a) {
            if (a.ToString().Contains("virus")) 
                return true;
            return false;
        }
    }
    /// <summary>
    /// Class <c>LinksChecker</c> provides methods to check incoming email for links referring to suspicious websites
    /// </summary>
    class LinksChecker : BaseHandler
    {
        private MatchCollection links = null;
        /// <summary>
        /// Method <c>Handle</c> checks if email contains links and if links refer to suspicious sites increase spam score
        /// </summary>
        /// <param name="email">email to check</param>
        public override void Handle(Email email)
        {
            Console.WriteLine("LinksChecker : execution started");
            if (email.Message.Contains("ref")) 
                    spamScore += ParseLinks(email.Message) *.75;
            //Console.WriteLine("handled");
            //Console.WriteLine("spamScore " + spamScore);//
            base.Handle(email);
        }

        private bool CheckLink(string url)
        {
            foreach(string site in Globals.suspicWebSites)
            {
                if (url.Contains(site))
                    return true;
            }
            return false;
        }
        private int ParseLinks(string mystring)
        {
            int count=0;

            Regex urlRx = new Regex(@"(?<url>((http|https|ftp):[/][/]|www.)([a-z]|[A-Z]|[0-9]|[/.]|[~])*)", RegexOptions.IgnoreCase);

            links = urlRx.Matches(mystring);

            foreach (Match match in links)
            {
                var url = match.Groups["url"].Value;
                Console.WriteLine(match.ToString()); //mystring = mystring.Replace(url, string.Format("<a href=\"{0}\">{0}</a>", url));
                if (CheckLink(match.ToString()))
                    count++;
            }
            return count;
        }
    }
    /// <summary>
    /// Class <c>TextAnalyzer</c> provides methods to checks incoming email for spam words.
    /// </summary>
    class TextAnalyzer : BaseHandler
    {
        /// <summary>
        /// Method <c>Handle</c> checks if email contains spam words, increase spamScore
        /// </summary>
        /// <param name="email">email to check</param>
        public override void Handle(Email email)
        {
            Console.WriteLine("TextAnalyzer : execution started");
            if (email.Message.Contains("viagra"))
                spamScore += .25;
            //Console.WriteLine("handled");
            //Console.WriteLine("spamScore " + spamScore);//
            base.Handle(email);
        }
    }
    }
