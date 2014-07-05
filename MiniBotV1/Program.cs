using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Xml.Linq;
// first real C# apllication, sorry for any noob mistakes ;)
namespace MiniBotV1
{

    class Program
    {

        static bool isMod(string user)
        {
            string[] mods = {"minijackb", "bidaman98caty", "tehbossaru", "leocrubellatti","z3r0_b00t3r"};
            foreach (var x in mods)
            {
                if (user.ToLower() == x.ToLower())
                {
                    //Console.WriteLine("user is mod");
                    return true;
                }
            }
            //Console.WriteLine("user is not mod");
            return false;
        }

        static bool isIn(string match, string[] list, string looking = "")
        {
            foreach (var x in list)
            {
                //Console.WriteLine("looking for: " + x + looking);
                if (match == x + looking)
                {
                    
                    return true;
                }
            }
            return false;
        }

        static string[] fixOutput(string data)
        {
            //Console.WriteLine(data);
            //string newdata = Regex.Replace(data, @"!.+ :", ":");
            //string newdata = Regex.Replace(data, @".+ :", "");
            //Here we call Regex.Match.
            //Match match = Regex.Match(data,,
            //RegexOptions.IgnoreCase);




            string name = "Unknown";
            string message = "";

            string newdata = Regex.Replace(data, @"!.+ :", ":");
            Regex nameExpr = new Regex(@":.*:");
            Match match = nameExpr.Match(newdata);
            if (match.Success)
            {
                name = Regex.Replace(match.Groups[0].Value,":", "");
                //Console.WriteLine(name);
            }
            message = Regex.Replace(newdata, ":.*:", "");
            //Console.WriteLine(name + ": " + message + "\n");

            string[] re = {name, message};
            return re;
        }
        //static void checkInput()
        //{
        //    while (Console.ReadLine() != "Return")
        //    {
        //        Console.WriteLine("made it here");
        //        if (Console.ReadLine() == "Exit")
        //        {
        //            Environment.Exit(0);
        //        }
        //    }
        //}
        static void Main(string[] args)
        {
            string userName = "Mini_b0t"; // username
            string authCode = "oauth:kcm57m24avvtckilp0qvjqs9xtim5ng"; // auth key
            string channel = "z3r0_b00t3r"; // name of channel you want to connect to
            string[] backTrigger = { "back", "im back", "i'm back", "i am back" };
            string[] greetTrigger = { "hey chat", "sup all", "salut", "herro", "hellu", "hi there", "hey guys", "moi", "hi", "hello", "hay", "hey", "whats up", "hiya", "sup", "haya", "hola", "ola", "buna", "aloha", "what up", "heyo", "hayo" };
            string[] greetings = { "Hi", "Hello", "Hay", "Hey", "Hiya", "Sup", "Haya" };//, "hola", "ola", "buna", "aloha", "what up"};
            List<string> users = new List<string>();
            bool submitName = false;
            bool submitGame = false;

            TcpClient Client = new TcpClient("irc.twitch.tv", 6667); // irc server url + port
            NetworkStream NwStream = Client.GetStream(); //Get The TCP/Network Stream And Assign It To NwStream
            StreamReader Reader = new StreamReader(NwStream, Encoding.GetEncoding("iso8859-1")); //Read The Data From The IRC Server
            StreamWriter Writer = new StreamWriter(NwStream, Encoding.GetEncoding("iso8859-1")); //Write To The Server
            Writer.WriteLine("USER " + userName + " tmi twitch " + userName); //Gives The Server The Required USER Information.
            Writer.Flush();// must flush after writing
            Writer.WriteLine("PASS " + authCode); // tell server the auth key
            Writer.Flush();
            Writer.WriteLine("NICK " + userName); // tell server your display name (must be same as username)

            Writer.Flush();
            Writer.WriteLine("JOIN #" + channel); //Joins The Channel Specified.
            string prefix = "PRIVMSG #" + channel + " :";
            Writer.Flush();
            //Writer.WriteLine(prefix+"hello"); // try to send message to channel
            //Writer.Flush();
            //Writer.WriteLine("test"); // test write to stream

            //string xml = new WebClient().DownloadString("http://api.justin.tv/api/stream/summary.xml?channel="+channel);
            //XDocument doc = XDocument.Parse(xml);

            string data;
            var message = "";
            // string[] ex;
            bool shouldRun = true;
            bool input = false;
            string lastGreet = "";
            string lastFollow = "";
            bool shouldGreet = true;
            List<string> blacklist = new List<string>();
            bool inputAllowed = true;
            List<string> name = new List<string>();
            List<string> gameName = new List<string>();
            List<int> gameAmount = new List<int>();
            while (shouldRun)
            {
                inputAllowed = true;
                //string key = Console.ReadKey().ToString();
                //if (key.ToUpper() == "T")
                //{
                //    Console.WriteLine("lololol");
                //    checkInput();
                //}
                //else
                //{
                data = Reader.ReadLine();
                if (data != null)
                {
                    
                    

                    Console.WriteLine(data);
                    string[] re = fixOutput(data);
                    data = re[1];
                    var user = re[0];
                    string fancyUser = user.Substring(0, 1).ToUpper() + user.Substring(1);
                    Random rnd = new Random();
                    inputAllowed = !isIn(user, blacklist.ToArray());

                    int Ran = rnd.Next(0,300);
                    if (Ran == 1){
                        Console.Clear();
                    }

                    Writer.Flush();
                    if (inputAllowed)
                    {
                        /*if (data.Length > 1)
                        {
                            Console.WriteLine(data.ToLower() + "!");
                        }*/

                        var mod = isMod(user);

                        if (data != null && !input && !user.ToLower().Contains("b0t"))// &&  !user.Contains("bida"))
                        {

                            if (data.Length > 14)
                            {
                                Writer.WriteLine(data.Substring(0, 10));
                                Writer.Flush();
                            }

                            // First we see the input string.
                            //string input = "/content/alternate-1.aspx";
                            //Task task1 = Task.Factory.StartNew(() => checkInput());
                            //Task task2 = Task.Factory.StartNew(() => 
                            //Console.WriteLine(count);

                            //Console.WriteLine(user + ": " + data);

                            if (data.ToLower() == "!time")// && !user.Contains("bida") || !user.Contains("leo") || !user.Contains("minijack"))
                            {

                                int rand = rnd.Next(1, 3);
                                //Console.WriteLine(rand + );
                                Writer.WriteLine(prefix + "The current time (GMT 0): " + DateTime.Now.ToString("h:mm:ss tt"));
                                Writer.Flush();
                                //Writer.WriteLine(prefix+"bidaman? nahh, you want minijackb!");
                                Writer.Flush();

                            }
                            else if (data.ToLower() == "!fail")
                            {
                                Writer.Flush();
                                int random = rnd.Next(1, 3);
                                Console.WriteLine(random);
                                Console.WriteLine(random.GetType());
                                Console.WriteLine(1.GetType());
                                if (random == 1)
                                {
                                    Writer.WriteLine(prefix + "YES!");
                                    Writer.Flush();
                                }
                                else
                                {
                                    Writer.WriteLine(prefix + " ...No.");
                                    Writer.Flush();
                                }



                            }
                            else if (data.Length > 6 && data.ToLower().Substring(0, 5) == "!game")
                            {
                                Console.WriteLine("said game");
                                if (!submitGame)
                                {

                                    if (data.Length > 8 && data.ToLower().Substring(6, 4) == "open" && mod)
                                    {
                                        Console.WriteLine("turning game on");
                                        string[] match = data.Split(new Char[] { ' ' });

                                        if (match.Length != 1)
                                        {
                                            submitGame = true;
                                            gameName.Clear();
                                            gameAmount.Clear();
                                            users.Clear();
                                            Console.WriteLine("found expresion");
                                            List<string> games = new List<string>();
                                            for (var i = 2; i < match.Length; i++)
                                            {
                                                games.Add(match[i]);
                                            }
                                            //name = Regex.Replace(match.Groups[0].Value, ":", "");
                                            //Console.WriteLine(name);
                                            Writer.WriteLine(prefix + "You can now submit ideas for what game to play using \"!game <Your Game Choice>\"");
                                            Writer.Flush();
                                            if (match.Length >= 1)
                                            {
                                                message = "";
                                                foreach (var i in games)
                                                {
                                                    gameName.Add(i);
                                                    gameAmount.Add(0);
                                                    if (i != "on" && i != "off")
                                                    {
                                                        message += ", " + i;
                                                    }

                                                }
                                                Writer.WriteLine(prefix + "The games you can choose from are: " + message.Substring(2) + ". (IS CASE-SENSITIVE)");
                                                Writer.Flush();
                                            }

                                        }
                                        else
                                        {
                                            Writer.Write(prefix + "Somthing went wrong, sorry " + user);
                                            Writer.Flush();
                                        }


                                    }
                                    else if (data.ToLower().Substring(6) == "list")
                                    {
                                        var game = "";
                                        for (var x = 0; x < gameName.Count; x++)
                                        {
                                            game += ", " + gameName[x] + ": " + gameAmount[x];
                                        }
                                        Writer.WriteLine(prefix + "List of Suggested Games: " + game.Substring(2));
                                        Writer.Flush();
                                    }



                                }
                                else
                                {
                                    if (data.ToLower().Substring(6) == "close" && mod)
                                    {
                                        submitGame = false;
                                        Writer.WriteLine(prefix + "Voting has closed!! Please dont vote anymore!");
                                        Writer.Flush();
                                    }
                                    else if (!isIn(user, users.ToArray()))
                                    {
                                        users.Add(user);
                                        Writer.WriteLine(prefix + "vote added; " + fancyUser + "!" + " You choose: " + data.Substring(6).ToString());
                                        Writer.Flush();
                                        var temp = data.Substring(6).ToString();
                                        var exist = false;
                                        for (var i = 0; i < gameName.Count; i++)
                                        {
                                            if (gameName[i] == temp)
                                            {
                                                gameAmount[i] += 1;
                                                exist = true;
                                            }
                                        }
                                        if (!exist)
                                        {
                                            Writer.WriteLine(prefix + "Sorry, " + fancyUser + " what you said is not valid");
                                            Writer.Flush();
                                        }
                                    }
                                }
                            }
                            else if (data.Length > 6 && data.ToLower().Substring(0, 7) == "!plague")
                            {
                                Console.WriteLine("said plague");
                                if (!submitName)
                                {
                                    if (data.ToLower().Substring(8) == "open" && mod)
                                    {
                                        Writer.WriteLine(prefix + " You can now submit names using \"!plague <Your Plague Name>\"");
                                        Writer.Flush();
                                        submitName = true;
                                        name.Clear();
                                    }
                                    else if (data.ToLower().Substring(8) == "list")
                                    {
                                        var names = "";
                                        foreach (var i in name)
                                        {
                                            names += ", " + i;
                                        }
                                        Writer.WriteLine(prefix + "List of plague names submitted: " + names.Substring(2));
                                        Writer.Flush();
                                    }
                                }
                                else
                                {
                                    if (data.ToLower().Substring(8) == "close" && isMod(user))
                                    {
                                        submitName = false;
                                        Writer.WriteLine(prefix + "You can no longer submit plague names!");
                                        Writer.Flush();
                                        var names = "";
                                        foreach (var i in name)
                                        {
                                            names += ", " + i;
                                        }
                                        Writer.WriteLine(prefix + "List of plague names submitted: " + names.Substring(2));
                                        Writer.Flush();
                                    }
                                    else
                                    {
                                        var temp = data.Substring(8).ToString();
                                        name.Add(temp);
                                    }
                                }
                            }
                            else if (data.Length > 6 && data.ToLower().Substring(0, 6) == "!spawn" && mod)
                            {
                                Console.WriteLine("said spawn");// -- working
                                Regex nameExpr = new Regex(@"\s.\s");
                                Match match = nameExpr.Match(data);
                                Console.WriteLine(match.Success);
                                if (match.Success)
                                {
                                    //var spawn = "";
                                    Regex placeExpr = new Regex(@"\s.");
                                    Match match2 = placeExpr.Match(data);
                                    Writer.WriteLine(prefix + fancyUser + " Spawned a " + match.Groups[0] + ". At :");// + spawn );
                                    Writer.Flush();
                                    //Console.WriteLine(name);
                                }
                            }
                            else if (data.Length > 2 && data.ToLower().Substring(0, 3) == "!wb" && mod)
                            {
                                Writer.WriteLine(prefix + "Welcome back " + data.Substring(4));
                                Writer.Flush();
                            }
                            else if ((isIn(data.ToLower(), backTrigger)|| isIn(data.ToLower(), backTrigger,"!")) && shouldGreet)
                            {
                                Writer.WriteLine(prefix + "Welcome back " + fancyUser);
                                Writer.Flush();
                            }
                            /*else if (data.ToLower() == "!help" && mod && false)
                            {
                                Writer.WriteLine(prefix + "!time : get current time in GMT 0. ");
                                Writer.Flush();
                                Writer.WriteLine(prefix + "!fail : Tells you wether something was a fail or no. ");
                                Writer.Flush();
                            }*/
                            else if ((isIn(data.ToLower(), greetTrigger) || isIn(data.ToLower(), greetTrigger,"!")) && shouldGreet && user != lastGreet) //data.ToLower() == "hi" || data.ToLower() == "hello" || data.ToLower() == "hay" || data.ToLower() == "hey" || data.ToLower() == "hello?")
                            {
                                if (user == "merlyin")
                                {
                                    Writer.WriteLine(prefix + "Hay Merlyin! *highfive*!");
                                    Writer.Flush();
                                }
                                else
                                {
                                    int random = rnd.Next(0, greetings.Length);
                                    Writer.WriteLine(prefix + greetings[random] + " " + fancyUser);
                                    Writer.Flush();
                                    
                                }
                                lastGreet = user;
                            }
                            else if (data.Length > 2 && data.ToLower().Substring(0, 3) == "!hi" && mod && data.ToLower().Substring(4) != lastFollow)
                            {

                                var person = data.Substring(4);
                                lastFollow = person;
                                Writer.WriteLine(prefix + "Hi " + person + ". Thanks for the follow Kappa");
                                Writer.Flush();

                            }
                            else if (data.Length > 5 && data.ToLower().Substring(0, 6) == "!greet" && mod)
                            {
                                int random = rnd.Next(0, greetings.Length);
                                Writer.WriteLine(prefix + greetings[random] + " " + data.Substring(6));
                                Writer.Flush();
                            }
                            else if (data.Length > 4 && data.Substring(0, 5) == "!conf")
                            {
                                //Console.WriteLine("user: " + user + " is edditing config");
                                //Console.WriteLine(data.Substring(5, 5));

                                //Console.WriteLine(data.Substring(15));
                                //Console.WriteLine("changing greeting");
                                if (data.Substring(6) == "on")
                                {
                                    shouldGreet = true;
                                    Writer.WriteLine(prefix + "Greeting turned on");
                                    Writer.Flush();
                                }
                                else if (data.Substring(6) == "off")
                                {
                                    shouldGreet = false;
                                    Writer.WriteLine(prefix + "Greeting turned off");
                                    Writer.Flush();
                                }

                            }
                            else if (mod && data.Length > 12 && data.ToLower().Substring(0, 10) == "!blacklist" && mod)
                            {
                                //Writer.WriteLine(prefix + "said blacklist");
                                //Writer.Flush();
                                if (data.Length > 14 && data.ToLower().Substring(11, 3) == "add")
                                {
                                    blacklist.Add(data.Substring(15).ToLower());
                                    Writer.WriteLine(prefix+"\""+data.Substring(15) + "\" added to blacklist");
                                    Writer.Flush();
                                }
                                else if (data.Length > 14 && data.ToLower().Substring(11, 4) == "list" )
                                {
                                    if (blacklist.ToArray().Length != 0)
                                    {
                                        string people = "";
                                        foreach (var i in blacklist.ToArray())
                                        {
                                            people += ", " + i;
                                        }
                                        people = people.Substring(2);
                                        Writer.WriteLine(prefix + "List of blacklisted people: " + people);
                                        Writer.Flush();
                                    }
                                }
                            }
                            if (data.Length == 4 && data.ToLower() == "!exit" && mod)
                            {
                                /*StreamWriter stream = null;
                                try
                                {
                                    XmlSerializer xmlSerializer = new XmlSerializer
                                }*/
                            }
                            Writer.Flush();
                            // Here we check the Match instance.
                            //if (match.Success)
                            //{
                            // Finally, we get the Group value and display it.
                            //    string key = match.Groups[1].Value;
                            //    Console.WriteLine(key);
                            //}

                            //char[] charSeparator = new char[] { ' ' };
                            //ex = data.Split(charSeparator, 5);

                            //if (ex[0] == "PING")
                            //{
                            //Writer.WriteLine("PONG " + ex[1]);
                            //}
                        }
                    }
                }
            }
        }

    }
}
//}