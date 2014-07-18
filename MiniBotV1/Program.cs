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

    /*public class ReturnValue
    {
        public string Name {get; set; }
        public string Msg {get; set; }
        public string Com {get; set; }
        public List<string> Arg {get; set; }
        public ReturnValue(string name, string msg, string com, List<string> arg)
        {
            Name = name;
            Msg = msg;
            Com = com;
            Arg = arg;
        }

    }*/
    class Settings
    {
        
        private bool _debugMode = false;
        private bool _inputAllowed = true;
        private bool _shouldRun = true;
        private string[] _mods = { "minijackb", "bidaman98caty", "tehbossaru", "leocrubellatti", "z3r0_b00t3r", "merlyin" };


        public bool shouldRun
        {
            get
            {
                return _shouldRun;
            }
            set
            {
                if (value.GetType() != true.GetType())
                {
                    throw new OverflowException();
                }
                _shouldRun = value;
            }

        }


        public bool inputAllowed
        {
            get
            {
                return _inputAllowed;
            }
            set
            {
                if (value.GetType() != true.GetType())
                {
                    throw new OverflowException();
                }
                _inputAllowed = value;
            }

        }

        public bool debugMode
        {
            get
            {
                return _debugMode;
            }
            set
            {
                if (value.GetType() != true.GetType())
                {
                    throw new OverflowException();
                }
                _debugMode = value;
            }

        }

        public string[] mods
        {
            get
            {
                return _mods;
            }
            
        }

    }

    class Program
    {

        static bool isMod(string user)
        {
            Settings settings = new Settings();
            foreach (var x in settings.mods)
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




            string user = "Unknown";
            string message = "";
            string command = "";
            string[] args = {};
            string newdata = Regex.Replace(data, @"!.+ :", ":");
            Regex nameExpr = new Regex(@":.*:");
            Match match = nameExpr.Match(newdata);

            if (match.Success)
            {
                user = Regex.Replace(match.Groups[0].Value,":", "");
                //Console.WriteLine(name);
            }

            message = Regex.Replace(newdata, ":.*:", "");
            //Console.WriteLine(name + ": " + message + "\n");

            Regex ComExpr = new Regex(@"!\w*");
            match = ComExpr.Match(message);
            //Console.WriteLine(match.Groups[0].Value);

            if (match.Success)
            {
                command = match.Groups[0].Value;
            }

            command = Regex.Replace(command, " ", "");

            return new string[] { user, message, command };
            

            


            //return new ReturnValue(user, message, command, args);
        }

        static string[] getArgs(string message)
        {
            string[] args = message.Split(' ');
            List<string> tempArgs = new List<string>();
            for (var i = 1; i < args.Length; i++)
            {
                tempArgs.Add(args[i]);
            }
                return tempArgs.ToArray();
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
            Settings settings = new Settings();
            var merlyin = false;
            var bidaman98caty = false;
            string userName = "Mini_b0t"; // username
            string authCode = "oauth:kcm57m24avvtckilp0qvjqs9xtim5ng"; // auth key
            string channel = "mini_b0t".ToLower(); // name of channel you want to connect to
            string[] backTrigger = { "back", "im back", "i'm back", "i am back", "back :d" };
            string[] greetTrigger = {"hai","yo", "hey chat", "sup all", "salut", "herro", "hellu", "hi there", "hey guys", "moi", "hi", "hello", "hay", "hey", "whats up", "hiya", "sup", "haya", "hola", "ola", "buna", "aloha", "what up", "heyo", "hayo" };
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
            
            bool input = false;
            string lastGreet = "";
            string lastFollow = "";
            bool shouldGreet = true;
            List<string> blacklist = new List<string>();
            
            List<string> name = new List<string>();
            List<string> nameUser = new List<string>();
            List<string> gameName = new List<string>();
            List<int> gameAmount = new List<int>();
            var time = false;
            


            while (settings.shouldRun)
            {

                data = Reader.ReadLine();
                if (data != null)
                {

                    if (DateTime.Now.ToString("htt") == "9PM" && !time)
                    {
                        time = true;
                        Writer.WriteLine(prefix + "The time is: " + DateTime.Now.ToString("h:mm:ss tt"));
                        Writer.Flush();
                    }

                    Console.WriteLine(data);

                    string[] re = fixOutput(data);
                    string user = re[0];
                    data = re[1];
                    string fancyUser = user.Substring(0, 1).ToUpper() + user.Substring(1);
                    string command = re[2].ToString().ToLower();
                    string[] ComArgs = getArgs(re[1]);

                    Random rnd = new Random();
                    settings.inputAllowed = !isIn(user, blacklist.ToArray());

                    int Ran = rnd.Next(0,300);
                    if (Ran == 1){
                        Console.Clear();
                    }

                    if (settings.debugMode && !user.Contains("tmi.twitch.tv"))
                    {
                        Writer.WriteLine(prefix + "fancyUser: \"" + fancyUser + "\"");
                        Writer.Flush();
                        Writer.WriteLine(prefix + "message: \"" + data + "\"");
                        Writer.Flush();
                        Writer.WriteLine(prefix + "command: \"" + command + "\"");
                        Writer.Flush();
                        Writer.WriteLine(prefix + "ComArgs amount: " + ComArgs.Length.ToString());
                        Writer.Flush();
                        foreach (var i in ComArgs)
                        {
                            Writer.WriteLine(prefix + "args: \"" + i + "\"");
                            Writer.Flush();
                        }

                    }

                    Writer.Flush();
                    if (settings.inputAllowed)
                    {
                        /*if (data.Length > 1)
                        {
                            Console.WriteLine(data.ToLower() + "!");
                        }*/

                        var mod = isMod(user);

                        if (data != null  && !user.ToLower().Contains("b0t"))// &&  !user.Contains("bida"))
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

                            if (command == "!time")// && !user.Contains("bida") || !user.Contains("leo") || !user.Contains("minijack"))
                            {

                                int rand = rnd.Next(1, 3);
                                //Console.WriteLine(rand + );
                                Writer.WriteLine(prefix + "The current time (GMT 0): " + DateTime.Now.ToString("h:mm:ss tt"));
                                Writer.Flush();
                                //Writer.WriteLine(prefix+"bidaman? nahh, you want minijackb!");
                                Writer.Flush();

                            }
                            else if (command == "!fail")
                            {
                                Writer.Flush();
                                int random = rnd.Next(1, 3);
                                if (random == 1)
                                {
                                    Writer.WriteLine(prefix + "FAIL!");
                                    Writer.Flush();
                                }
                                else
                                {
                                    Writer.WriteLine(prefix + " ...No.");
                                    Writer.Flush();
                                }



                            }
                            else if (command == "!win")
                            {
                                int random = rnd.Next(1, 2);
                                if (random == 1)
                                {
                                    Writer.WriteLine(prefix + "WIN!");
                                    Writer.Flush();
                                }
                                else
                                {
                                    Writer.WriteLine(prefix + " ...No.");
                                    Writer.Flush();
                                }
                            }
                            else if (command == "!game")
                            {
                                Console.WriteLine("said game");
                                if (!submitGame) // if the poll is not open
                                {
                                    if (ComArgs[0] == "open" && mod)
                                    {
                                        Console.WriteLine("turning game on");
                                        //string[] match = data.Split(new Char[] { ' ' });
                                        if (ComArgs.Length > 1)
                                        {
                                            submitGame = true; //users can now submit there choice of game
                                            gameName.Clear(); // clears the previous game names, amount and users
                                            gameAmount.Clear();
                                            users.Clear();
                                        //    Console.WriteLine("found expresion");
                                            List<string> games = new List<string>();
                                            for (var i = 1; i < ComArgs.Length; i++) // loops through the paramaters (except the first) and adds them to the list of games the user can choose
                                            {
                                                games.Add(ComArgs[i]);
                                            }
                                            //name = Regex.Replace(match.Groups[0].Value, ":", "");
                                            //Console.WriteLine(name);
                                            Writer.Flush();
                                            
                                            if (ComArgs.Length >= 1)
                                            {
                                                message = "";
                                                foreach (var i in games)
                                                {
                                                    gameName.Add(i);
                                                    gameAmount.Add(0);
                                                    if (i != "open" && i != "close") // failsafe incase the open or close oporator are in the games array
                                                    {
                                                        message += ", \"" + i+"\"";
                                                    }
                                                }

                                                Writer.WriteLine(prefix + "You can now submit ideas for what game to play using: \"!game (your game choice)\"");
                                                Writer.Flush();
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
                                    else if (ComArgs[0] == "list")
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
                                else // if the poll is open
                                {
                                    if (ComArgs[0] == "close" && mod)
                                    {
                                        submitGame = false;
                                        Writer.WriteLine(prefix + "Voting has closed!! Please dont vote anymore!");
                                        Writer.Flush();

                                        var game = "";
                                        for (var x = 0; x < gameName.Count; x++)
                                        {
                                            game += ", " + gameName[x] + ": " + gameAmount[x];
                                        }
                                        Writer.WriteLine(prefix + "List of Suggested Games: " + game.Substring(2));
                                        Writer.Flush();
                                    }
                                    else if (!isIn(user, users.ToArray())) // adds users choice
                                    {
                                        var exist = false;
                                        for (var i = 0; i < gameName.Count; i++) // loops through games avaliable
                                        {
                                            if (gameName[i] == ComArgs[0]) // if the user choice is in the list of games they are allowed to choose from
                                            {
                                                gameAmount[i] += 1; // add to the amount of times that game has been chosen
                                                exist = true; // record that we have found a match
                                                break; // exit from the loop (this is good practice incase the array of games is huge and taking up tick cycles)
                                            }
                                        }
                                        if (!exist) // if what the user said is not a valid choice
                                        {
                                            Writer.WriteLine(prefix + "Sorry, " + fancyUser + ", what you said is not a valid choice"); // tell them what they did wrong
                                            Writer.Flush();
                                        }
                                        else // if what the user said is vaild
                                        {
                                            users.Add(user); // add them to the list of users so that they cannot vote again
                                            Writer.WriteLine(prefix + "vote added; " + fancyUser + "!" + " You chose: " + ComArgs[0]);
                                            Writer.Flush();
                                        }
                                    }
                                }
                            }
                            else if (command == "!name")
                            {
                                //Console.WriteLine("\""+data.ToLower().Substring(0, 5)+"\"");
                                //Console.WriteLine("\"" + data.ToLower().Substring(6) + "\"");
                                //Console.WriteLine(submitName);
                                if (!submitName)
                                {
                                    //Console.WriteLine("here1");
                                    if (ComArgs[0] == "open" && mod)
                                    {
                                        Writer.WriteLine(prefix + "You can now submit names using \"!name (Your name submission)\"");
                                        Writer.Flush();
                                        //Writer.Flush();
                                        //Writer.WriteLine(prefix + " You can now submit names using \"!name <Your name Submission>\"");
                                        //Writer.Flush();
                                        Console.WriteLine("here2");
                                        submitName = true;
                                        nameUser.Clear();
                                        name.Clear();
                                    }
                                    else if (ComArgs[0] == "list")
                                    {
                                        var names = "";
                                        foreach (var i in name)
                                        {
                                            names += ", " + i;
                                        }
                                        Writer.WriteLine(prefix + "List of names submitted: " + names.Substring(2));
                                        Writer.Flush();
                                    }
                                }
                                else
                                {
                                    if (ComArgs[0] == "close" && isMod(user))
                                    {
                                        submitName = false;
                                        Writer.WriteLine(prefix + "You can no longer submit names!");
                                        Writer.Flush();
                                        var names = "";
                                        foreach (var i in name)
                                        {
                                            names += ", " + i + " ("+nameUser[name.IndexOf(i)]+")";
                                        }
                                        Writer.WriteLine(prefix + "List of names submitted: " + names.Substring(2));
                                        Writer.Flush();
                                    }
                                    else
                                    {
                                        var temp = ComArgs[0].ToString();
                                        name.Add(temp);
                                        nameUser.Add(fancyUser);
                                    }
                                }
                            }
                            else if (command == "!spawn" && mod)
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
                            else if (command == "!wb" && mod)
                            {
                                Writer.WriteLine(prefix + "Welcome back " + data.Substring(4));
                                Writer.Flush();
                            }
                            else if ((isIn(data.ToLower(), backTrigger) || isIn(data.ToLower(), backTrigger, "!") || isIn(data.ToLower(), backTrigger, "*")) && shouldGreet)
                            {
                                Writer.WriteLine(prefix + "Welcome back " + fancyUser + " MrDestructoid /");
                                Writer.Flush();
                            }
                            /*else if (data.ToLower() == "!help" && mod && false)
                            {
                                Writer.WriteLine(prefix + "!time : get current time in GMT 0. ");
                                Writer.Flush();
                                Writer.WriteLine(prefix + "!fail : Tells you wether something was a fail or no. ");
                                Writer.Flush();
                            }*/
                            else if ((isIn(command, greetTrigger) || isIn(data.ToLower(), greetTrigger,"!") || isIn(data.ToLower(), greetTrigger,"*")) && shouldGreet && user != lastGreet) //data.ToLower() == "hi" || data.ToLower() == "hello" || data.ToLower() == "hay" || data.ToLower() == "hey" || data.ToLower() == "hello?")
                            {
                                if (user.ToLower() == "merlyin")
                                {
                                    merlyin = true;
                                    Writer.WriteLine(prefix + "Hay Merlyin!");
                                    Writer.Flush();
                                    Writer.WriteLine(prefix + "/me holds up hand for highfive");
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
                            else if (ComArgs.Length == 1 && command == "!hi" && mod && ComArgs[0] != lastFollow)
                            {
                                var person = data.Substring(4);
                                lastFollow = person;
                                Writer.WriteLine(prefix + "Hi " + person + ". Thanks for the follow Kappa");
                                Writer.Flush();
                            }
                            else if (ComArgs.Length == 1 && command == "!greet" && mod)
                            {
                                int random = rnd.Next(0, greetings.Length);
                                Writer.WriteLine(prefix + greetings[random] + " " + ComArgs[0]);
                                Writer.Flush();
                            }
                            else if (ComArgs.Length == 2 && command.Contains("!conf") && mod)
                            {
                                //Writer.WriteLine(prefix + "config");
                                //Writer.Flush();
                                Console.WriteLine("user: " + user + " is edditing config");
                                Console.WriteLine(command);

                                Console.WriteLine(ComArgs[0]);
                                //Console.WriteLine("changing greeting");
                                if (ComArgs[0] == "greeting")
                                {
                                    if (ComArgs[1] == "on")
                                    {
                                        shouldGreet = true;
                                        Writer.WriteLine(prefix + "Greeting mode on");
                                        Writer.Flush();
                                    }
                                    else if (ComArgs[1] == "off")
                                    {
                                        shouldGreet = false;
                                        Writer.WriteLine(prefix + "Greeting mode off");
                                        Writer.Flush();
                                    }
                                }
                                else if (ComArgs[0] == "debug")
                                {
                                    if (ComArgs[1] == "on")
                                    {
                                        settings.debugMode = true;
                                        Writer.WriteLine(prefix + "Debug mode on");
                                        Writer.Flush();
                                    }
                                    else if (ComArgs[1] == "off")
                                    {
                                        settings.debugMode = false;
                                        Writer.WriteLine(prefix + "Debug mode off");
                                        Writer.Flush();
                                    }
                                }

                            }
                            else if (mod && command == "!blacklist" && mod)
                            {
                                //Writer.WriteLine(prefix + "said blacklist");
                                //Writer.Flush();
                                if (ComArgs.Length == 2 && ComArgs[0] == "add")
                                {
                                    blacklist.Add(ComArgs[1].ToLower());
                                    Writer.WriteLine(prefix+"\""+data.Substring(15) + "\" added to blacklist");
                                    Writer.Flush();
                                }
                                else if (ComArgs[0] == "list")
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
                                    else
                                    {
                                        Writer.WriteLine(prefix + "No one is in the blacklist!");
                                        Writer.Flush();
                                    }
                                }
                                else if (ComArgs[0] == "remove")
                                {
                                    var finding = ComArgs[1];
                                    var i = 0;
                                    bool found = false;
                                    foreach (var x in blacklist.ToArray())
                                    {
                                        if (finding == blacklist[i]){
                                            blacklist.RemoveAt(i);
                                            Writer.WriteLine(prefix+"User: "+finding+ " has been removed from the blacklist!");
                                            Writer.Flush();
                                            found = true;
                                        }
                                    }
                                    if (!found){
                                        Writer.WriteLine(prefix+"no user by the name: " + finding + " was found!");
                                        Writer.Flush();
                                    }
                                }
                            }
                            else if (command == "!exit" && mod)
                            {
                                /*StreamWriter stream = null;
                                try
                                {
                              http://snag.gy/REMZq.jpg      XmlSerializer xmlSerializer = new XmlSerializer
                                }*/
                            }
                            else if (merlyin == true)
                            {
                                if (data.Contains("highfive") && user == "merlyin")
                                {
                                    Writer.WriteLine(prefix + "/me loves Merlyin more than bidaman98caty");
                                    Writer.Flush();
                                    merlyin = false;
                                    bidaman98caty = true;
                                }

                            }
                            else if (user.ToLower().Contains("bidaman") && bidaman98caty)
                            {
                                if (data.ToLower().Contains("love") && data.ToLower().Contains("mini"))
                                {
                                    Writer.WriteLine(prefix + "YOU'RE MY MOTHER BIDAMAN98CATY!");
                                    Writer.Flush();
                                    bidaman98caty = false;
                                }
                            }
                            
                            else if (user.ToLower() == "minijackb" && command == "!ammo"){
                                Writer.WriteLine(prefix+"!b00llets");
                                Writer.Flush();
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