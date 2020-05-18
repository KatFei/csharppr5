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
        public static String[] sendersBlack = { "ShopXXX", "Ozon", "MailruGames" };
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

            // считываем информацию о полученных емейлах из файла Input
            StreamReader sIn = null;
            try
            {
                sIn = new StreamReader(path + "/Input.txt", Encoding.UTF8);
                int n = Int16.Parse(sIn.ReadLine());
                mailbox = new Email[n];
                string srcStart;
                string srcMessage;
                string srcSender;
                string srcRef;

                int i = 0;
                while ((i < n) && (!sIn.EndOfStream))
                {

                    srcStart = sIn.ReadLine();
                    if (srcStart == "Book")
                    {
                        srcSender = sIn.ReadLine();
                        srcMessage = sIn.ReadLine();
                        srcRef = sIn.ReadLine();
                        mailbox[i] = new Email(srcMessage, srcSender, srcRef);
                        i++;
                    }
                };
            }
            catch {
                Console.WriteLine("\nRead from file failed: \n" + path);
                if (!File.Exists(path))
                    Console.WriteLine("File does not exist");//throw new ArgumentNullException("File does not exist", e);
                                                             //else
                                                             //throw new ArgumentException("Some Read Exception", e);
            }
            finally { if (sIn != null) sIn.Close(); }

            //создание цепочки обязанностей
            //на основе пользовательских настроек фильтрации спама
                                //--IHandler[] handlersChain = null;
            IHandler firstHandler = null;
            try
            {
                //цикл
                if (args[0].ToString().ToLower().StartsWith("a"))
                {
                    //firstHandler
                    IHandler h1 = new AttachmentChecker();
                }
                else if (args[0].ToString().ToLower().StartsWith("l"))
                {
                    IHandler h2 = new LinksChecker();
                }
                else if (args[0].ToString().ToLower().StartsWith("t"))
                {
                    IHandler h3 = new TextAnalyzer();
                }
                else
                    Console.WriteLine("Wrong handler name");
            }
            catch
            {
                Console.WriteLine("Fail to read settings from console");
                //для тестирования
                IHandler AttachmentHandler = new AttachmentChecker();
                IHandler LinksHandler = new LinksChecker();
                IHandler TextAnalyzeHandler = new TextAnalyzer();
                //цепочка проверок Attachment->Links->Text
                AttachmentHandler.setNext(LinksHandler); 
                LinksHandler.setNext(TextAnalyzeHandler);

                                    //--handlersChain[0] = AttachmentHandler;
                Globals.settings = new string[] { "attachments", "links", "text" };
            }
        }
    }
}
