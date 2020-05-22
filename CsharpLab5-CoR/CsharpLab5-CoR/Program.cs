using System;
using System.IO;
using System.Text;

namespace CsharpLab5_CoR
{
    public static class Globals
    {
        public static double trustLimit = 1.5;
        //settings = {}
        //настройки
        //"attachments"			        f(file)
        //"links"				        l
        //"ads"				            a
        //"senders-blacklist"		    s
        //"user-preferences-analyzer"	u

        public static String[] settings = { "attachments", "links", "ads","text", "senders-blacklist"};
        public static String[] preferences = { "Programming", "Ozon", "Cooking", "Games" }; // Modifiable
        public static String[] sendersBlackL = { "ShopXXX", "Ozon", "MailruGames" };
        public static String[] suspicWebSites = { "ShopXXX", "xxx", "adfly" , "horux"};
    }
    
    
    class Program
    {
        static void Main(string[] args)
        {
            //создаем каталог эл. писем
            Email[] mailbox = null;
            string workDir = Environment.CurrentDirectory;
            string path = Directory.GetParent(workDir).Parent.Parent.FullName;
            Console.WriteLine(workDir);
            Console.WriteLine(path + "\\");
            // считываем информацию о полученных емейлах из файла Input
            StreamReader sIn = null;
            try
            {
                sIn = new StreamReader(path + "/Input.txt");
                
                int n = Int16.Parse(sIn.ReadLine());
                mailbox = new Email[n];
                string srcStart;
                string srcMessage;
                string srcSender;
                string srcRef;

                int i = -1;
                while ((i < n) && (!sIn.EndOfStream))
                {
                    Console.WriteLine("while");
                    srcStart = sIn.ReadLine();
                    if (srcStart == "email")
                    {
                        Console.WriteLine("New email recieved");
                        i++;
                        srcMessage = "";
                        srcSender = "";
                        srcRef = null;
                        srcStart = sIn.ReadLine();
                        if (srcStart == "sender")
                        {
                            srcSender = sIn.ReadLine();
                            Console.WriteLine(srcSender);
                        }
                        srcStart = sIn.ReadLine();
                        if (srcStart == "message")
                        {
                            srcStart = sIn.ReadLine();
                            do
                            {
                                srcMessage += "\n" + srcStart;
                                srcStart = sIn.ReadLine();
                            } while (srcStart != "/end");


                        }
                        srcStart = sIn.ReadLine();
                        if (srcStart == "attachment")
                        {
                            srcRef = sIn.ReadLine();
                        }
                        
                        mailbox[i] = new Email(srcMessage, srcSender, srcRef);
                    }
                      
                }
                
            }
            catch {
                Console.WriteLine("\nRead from file failed: \n" + path);
                if (!File.Exists(path+ "/Input.txt"))
                    Console.WriteLine("File does not exist");
                else
                    Console.WriteLine("File exists");
            }
            finally { if (sIn != null) sIn.Close(); }
            //Console.WriteLine(mailbox.Length);
            if (mailbox != null)
            {
                Console.WriteLine("\nMailbox is opened");

                foreach (Email src in mailbox)
                {
                    if (src != null)
                    {
                        Console.WriteLine("New email");
                        src.GetEmailInfo();
                        Console.WriteLine();
                    }
                }
            }
            //создание цепочки обязанностей
            //на основе пользовательских настроек фильтрации спама
            //--IHandler[] handlersChain = null;
            AttachmentChecker firstHandler = null;
            /*try
            {

                //цикл
                if (args[0].ToString().ToLower().StartsWith("a"))
                {
                    //firstHandler
                    firstHandler = new AttachmentChecker();
                }
                else if (args[0].ToString().ToLower().StartsWith("l"))
                {
                    firstHandler = new LinksChecker();
                }
                else if (args[0].ToString().ToLower().StartsWith("t"))
                {
                    firstHandler = new TextAnalyzer();
                }
                else
                    Console.WriteLine("Wrong handler name");
            }
            catch
            {

                //--handlersChain[0] = AttachmentHandler;
                Globals.settings = new string[] { "attachments", "links", "text" };
            }*/
            Console.WriteLine("Fail to read settings from console");
            //для тестирования
            AttachmentChecker AttachmentHandler = new AttachmentChecker();
            LinksChecker LinksHandler = new LinksChecker();
            TextAnalyzer TextAnalyzeHandler = new TextAnalyzer();
            //цепочка проверок Attachment->Links->Text
            AttachmentHandler.setNext(LinksHandler);
            LinksHandler.setNext(TextAnalyzeHandler);
            //ORRRRR
            firstHandler = new AttachmentChecker();
            firstHandler.setNext(LinksHandler);


            if (mailbox != null)
            {
                foreach (Email eml in mailbox)
                {
                    Console.WriteLine("\nChecking email for spam");
                    if (eml != null)
                    {
                        firstHandler.Handle(eml);
                    }
                }
            }
        }
    }
}
