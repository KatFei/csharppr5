using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpLab5_CoR
{
    public class Email
    {
        private readonly string _message;
        private readonly string _sender;
        private object attachment;//readonly? или можно удалять если вирус?
        //private double spamScore = 0;
        private bool spamChecked = false;
        private bool isspam = false;

        //GetMessage
        //GetSender
        public Email(string m, string s, object a = null)
        {
            _message = m;
            _sender = s;
            attachment = a;
            //get links (parsing)
            //create array of links
        }

        public string Message { get => _message;}
        public string Sender { get => _sender;}
        public object Attachment { get => attachment;}

        //public bool Isspam { get => isspam; set => isspam = value; }

        public void isSpam(double spamScore)
        {
            //устанавливаем isspam 1 раз
            if (!spamChecked) if (spamScore > 1)
                {
                    spamChecked = true;
                    isspam = true;
                }
        }
        public bool isSpam()
        {
            return isspam;
        }
    }
}
