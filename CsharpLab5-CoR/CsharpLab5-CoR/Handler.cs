using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpLab5_CoR
{
    interface IHandler
    {
        void setNext(IHandler h);
        void Handle(Email email);
    }

    class BaseHandler : IHandler
    {
        private IHandler next;
        protected static double spamScore = 0;
        public void setNext(IHandler h)
        {
            next = h;
        }

        public void Handle(Email email)
        {
            if (spamScore > Globals.trustLimit)
            {
                Console.WriteLine("Email moved to spam");
            }
            else
                next.Handle(email);
        }
    }

    class AttachmentChecker : BaseHandler
    {
        //  if no attachment go to next Handler
	    //  if virus move to Spam
        new public void Handle(Email email)
        {
            if (email.Attachment != null)
            { spamScore += Globals.trustLimit;}    
            Console.WriteLine("handled");
            base.Handle(email);
        }
        private bool CheckAttachment() {
            return true;
        }
    }
    class LinksChecker : BaseHandler
    {
        //	if virus link move to Spam
	    //  if ad link +=score
        new public void Handle(Email email)
        {
            if (email.Message.Contains("viagra"))
                spamScore += .25;
            Console.WriteLine("handled");
            base.Handle(email);
        }
    }
    class TextAnalyzer : BaseHandler
    {
        new public void Handle(Email email)
        {
            if (email.Message.Contains("viagra"))
                spamScore += .25;
            Console.WriteLine("handled");
            base.Handle(email);
        }
    }
    /*
    public class PerspcriptionDrugFilter : IFilter<string, double>
    {
        public double Execute(string text, Func<string, double> executeNext)
        {
            var score = executeNext(text);

            if (text.Contains("viagra"))
                score += .25;

            return score;
        }
    }
    */
}
