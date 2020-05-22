using System;
using System.Collections.Generic;
using System.Text;

namespace CsharpLab5_CoR
{
    /// <summary>
    /// Class <c>Email</c> represents email letter in the mailbox.
    /// </summary>
    public class Email
    {
        private readonly string _message;
        private readonly string _sender;
        private readonly object _attachment;


        /// <summary>
        /// Constructor of class <c>Email</c>, creates new email letter.
        /// </summary>
        /// <param name="m">text message</param>
        /// <param name="s">email sender</param>
        /// <param name="a">email attachment if any</param>
        public Email(string m, string s, object a = null)
        {
            _message = m;
            _sender = s;
            _attachment = a;
        }
        /// <summary>
        /// Property <c>Message</c> represents text message
        /// </summary>
        /// <value>Gets value of _message of type string</value>
        public string Message { get => _message;}
        /// <summary>
        /// Property <c>Sender</c> represents email sender
        /// </summary>
        /// <value>Gets value of _sender of type string</value>
        public string Sender { get => _sender;}
        /// <summary>
        /// Property <c>Attachment</c> represents email attachment
        /// </summary>
        /// <value>Gets value of _attachment of type object</value>
        public object Attachment { get => _attachment;}
        /// <summary>
        /// Method <c>GetEmailInfo</c> prints email information (for testing).
        /// </summary>
        public void GetEmailInfo()
        {
            Console.WriteLine(Sender);
            Console.WriteLine(Message);
            if (Attachment != null)
            { 
                Console.WriteLine(Attachment.ToString());
                if (Attachment.ToString().Contains("virus"))
                    Console.WriteLine("VIRUS");
            }
                
        }
    }
}
