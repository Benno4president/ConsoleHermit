using ConsoleHermit.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Console = Colorful.Console;

namespace ConsoleHermit
{
    public class HermitUI
    {
        public event EventHandler<string> CommandEntered;

        private HermitBackend hbe;

        public HermitUI(HermitBackend hbe)
        {
            this.hbe = hbe;
        }

        public bool inUI = false;

        public void Start()
        {
            Console.Clear();

            inUI = true;          

            while (true)
            {
                Console.Clear();
                //Console.ForegroundColor = Color.Chocolate;
                Banner();
                Console.WriteLine("|");

                foreach (var item in hbe.NoteList)
                {
                    var index = hbe.NoteList.IndexOf(item);
                    Color color = index % 2 == 0 ? Color.Chocolate : Color.Turquoise;

                    Console.Write("| ");
                    Console.WriteLine(index + ". " + item, color);
                }

                Console.WriteLine("|___________ ");
                Console.Write("\\> ");
                string input = Console.ReadLine();

                CommandEntered?.Invoke(this, input);
            }
        }

        public void Close()
        {
            Console.Clear();
            Console.ResetColor();
            Environment.Exit(0);
        }

        public void DisplayHelpMsg()
        {
            Console.WriteLine($"" +
                $"\n| {Clr("#Hermit Notes#",3)}            |  {Clr("#Hermit Holdsport#",3)}" +
                $"\n| {Clr("-ui", 3)} Opens the ui          | {Clr("-h", 3)}  View activity by index" +
                $"\n| {Clr("-q", 3)}  Quits                 | {Clr("-ha", 3)} View activities" +
                $"\n| {Clr("-n", 3)}  Create new note       | {Clr("-hm", 3)} View members" +
                $"\n| {Clr("-e", 3)}  Edit note by index    | {Clr("-hv", 3)} View saved names" +
                $"\n| {Clr("-d", 3)}  Delete note by index  | {Clr("-hn", 3)} Add new saved name" +
                $"\n| {Clr("-o", 3)}  Opens the notefile    | {Clr("-hd", 3)} Delete saved name" +
                $"\n| {Clr("-rs", 3)} Restores Session      | " +
                $"\n| {Clr("-vs", 3)} View Session          | " +
                $"\n| {Clr("-s", 3)}  Searches term         | " +
                $"\n|                           |" +
                $"\n| {Clr("#Hermit Path#", 3)}             |" +
                $"\n| {Clr("-p", 3)}  Goto path             |" +
                $"\n| {Clr("-sp", 3)} Save path             |" +
                $"\n| {Clr("-vp", 3)} View path list        |" +
                $"\n| {Clr("-dp", 3)} Delete path           |" +
                $"\n| {Clr("-wp", 3)} sWitch paths          |");



            //Console.Write("| ");
            //Console.WriteLine("    \\Hermit Help/", Color.Chocolate);
            //Console.Write("| "); Console.Write("-ui", Color.Chocolate); Console.WriteLine("  Opens the ui");
            //Console.Write("| "); Console.Write("-q", Color.Chocolate); Console.WriteLine("   Quits");
            //Console.Write("| "); Console.Write("-n", Color.Chocolate); Console.WriteLine("   Create new note");
            //Console.Write("| "); Console.Write("-e", Color.Chocolate); Console.WriteLine("   Edit note by index");
            //Console.Write("| "); Console.Write("-d", Color.Chocolate); Console.WriteLine("   Delete note by index");
            //Console.Write("| "); Console.Write("-o", Color.Chocolate); Console.WriteLine("   Opens the notefile");
            //Console.Write("| "); Console.Write("-rs", Color.Chocolate); Console.WriteLine("  Restores Session");
            //Console.Write("| "); Console.Write("-vs", Color.Chocolate); Console.WriteLine("  View Session");
            //Console.Write("| "); Console.Write("-s", Color.Chocolate); Console.WriteLine("   Searches term");

            //Console.Write("|\n|"); Console.WriteLine("     \\Path Help/", Color.Chocolate);

            //Console.Write("| "); Console.Write("-p", Color.Chocolate); Console.WriteLine("   goto Path");
            //Console.Write("| "); Console.Write("-sp", Color.Chocolate); Console.WriteLine("   Save Path");
            //Console.Write("| "); Console.Write("-vp", Color.Chocolate); Console.WriteLine("  View Path list");
            //Console.Write("| "); Console.Write("-dp", Color.Chocolate); Console.WriteLine("  Delete Path from list");
            //Console.Write("| "); Console.Write("-wp", Color.Chocolate); Console.WriteLine("  sWitch Path");
            
            //Console.Write("| "); Console.Write("-", Color.Chocolate); Console.WriteLine("   ");

            if (inUI)
                Console.ReadKey();
        }

        public void DisplayLoadingBar()
        {
            Console.Write("Working... ");
            int spinIndex = 0;
            while (spinIndex != 1000000)
            {
                Console.Write("\b" + @"/-\|"[(spinIndex++) & 3]);
            }
        }

        public void DisplayIndexInputError(string index)
        {
            Console.Write("| No index: ");
            Console.WriteLine(index, Color.Chocolate);
            Console.Write(" | List length is ");
            Console.WriteLine(hbe.NoteList.Count(), Color.Chocolate);
            if (inUI)
                Console.ReadKey();
        }

        internal void SearchNoteList(string searchterm)
        {
            var str = hbe.SearchNotes(searchterm);

            foreach (var item in str)
            {             
                var index = hbe.NoteList.IndexOf(item);
                Color color = index % 2 == 0 ? Color.Chocolate : Color.Turquoise;

                Console.Write("| ");
                Console.WriteLine(index + ". " + item, color);
            }

            if (inUI)
                Console.ReadKey();
        }

        internal void ViewSessionBackup()
        {
            hbe.LoadSessionBackupNotes().ForEach(item => Console.WriteLine("| "+item));
            if (inUI)
                Console.ReadKey();
        }

        public void DisplayNotANumberError(string index)
        {
            Console.Write("| Not a number: ");
            Console.WriteLine(index, Color.Chocolate);
            if (inUI)
                Console.ReadKey();
        }

        internal void DisplayPathListContent()
        {
            List<string> PathList = new List<string>();
            PathList = hbe.LoadPathList();
            var index = 0;
            Color color;

            Console.WriteLine("| ~");
            foreach (var path in PathList)
            {
                index = PathList.IndexOf(path);
                color = index % 2 == 0 ? Color.Chocolate : Color.Turquoise;

                Console.Write("| ");
                Console.WriteLine(index + ": " + path, color);               
            }

            if (inUI)
                Console.ReadKey();

        }

        internal bool ConfirmAction(string item, string action)
        {
            Console.Write("\n| Confirm ");
            Console.Write(action, Color.Chocolate);
            Console.Write(" [Y/N]\n| Item: ");
            Console.WriteLine(item, Color.Chocolate);
            Console.Write("\\>");

            if (Console.ReadKey().Key == ConsoleKey.Y) { 
                Console.WriteLine("");
                return true;
            }
            else
                return false;
        }

        internal void DisplayRkActivities()
        {
            List<HoldsportTeams> rkTeam = hbe.GetTeams();
            List<HoldsportActivities> rkActivities = hbe.GetRkActivies();

            Console.WriteLine($"| {Clr(rkTeam[0].Name, 2)} activities ({DateTime.Now.ToShortDateString()})");
            rkActivities.ForEach(i => Console.WriteLine($"| {rkActivities.IndexOf(i), 2}. {i.Name, -15} kl. {i.Starttime.ToShortTimeString()} - {i.Endtime.ToShortTimeString()}"));
            
        }

        internal void DisplayActivity(int index = 0)
        {
            HoldsportActivities rkActivity = hbe.GetRkActivies()[index];
            List<HoldsportActivityModel> activityAttendies = hbe.GetActivity(rkActivity.Id);
            List<string> savedNames = hbe.LoadSavedHoldsportNamesFromFile();

            Console.WriteLine($"|$| {Clr(rkActivity.Name, 2)}  |{Clr(activityAttendies.Count(i => i.StatusCode == 1).ToString(), 1)}/{Clr(activityAttendies.Count(i => i.StatusCode == 2).ToString(), 0)}/{rkActivity.NoRsvpCount}|");

            foreach (HoldsportActivityModel item in activityAttendies)
            {
                Console.Write($"|{(item.Status == "Tilmeldt" ? $"{Clr("#", 1)}" : $"{Clr("#", 0)}")}|");
                Console.WriteLine($" {(savedNames.Contains(item.Name) ? Clr(item.Name, 3) : item.Name),-30}");
            }
            
            foreach (NoRsvp item in rkActivity.NoRsvp)
            {
                //if(item.)
                Console.WriteLine($"|#| {(savedNames.Contains(item.Name) ? Clr(item.Name, 3) : item.Name),-30}");
            }
        }

        internal void DisplaySavedHoldsportNames()
        {
            List<string> list = hbe.LoadSavedHoldsportNamesFromFile();
            Console.WriteLine("| List of saved Holdsport names");
            list.ForEach(i => Console.WriteLine($"| {list.IndexOf(i),2}. {Clr(i,3)}"));
        }

        internal void DisplayRkMembers()
        {
            var savedNames = hbe.LoadSavedHoldsportNamesFromFile();
            var list = hbe.GetRkMembers();
            Console.WriteLine("| Members");
            foreach (var item in list)
            {
                if(savedNames.Contains(item.Firstname + " " + item.Lastname))
                    Console.WriteLine($"| {list.IndexOf(item),2}. {Clr($"{item.Firstname} {item.Lastname}",3)}");
                else
                    Console.WriteLine($"| {list.IndexOf(item),2}. {$"{item.Firstname} {item.Lastname}"}");
            }
        }

        //public void DisplayProductNotFound(string product)
        //{
        //    Console.WriteLine($"The requested product ID; {product}");
        //    Console.WriteLine("could not be found");
        //    Console.WriteLine("");
        //    Console.WriteLine("");
        //    Console.WriteLine(" Press ENTER to go back");
        //    Console.WriteLine("<--");
        //    Console.ReadKey();
        //}

        //public void DisplayTooManyArgumentsError(string command)
        //{
        //    Console.WriteLine($"The command: {command}");
        //    Console.WriteLine("does not match the format");
        //    Console.WriteLine("");
        //    Console.WriteLine("The Format is:");
        //    Console.WriteLine("[Username] [Product Id]");
        //    Console.WriteLine("or");
        //    Console.WriteLine("[Username] [Amount] [Product Id]");
        //    Console.WriteLine("for multi purchase");
        //    Console.WriteLine("");
        //    Console.WriteLine("");
        //    Console.WriteLine(" Press ENTER to go back");
        //    Console.WriteLine("<--");
        //    Console.ReadKey();
        //}

        //public void DisplayUserBuysProduct(string transaction)
        //{
        //    Console.WriteLine("Transaction complete");
        //    Console.WriteLine($"User: {transaction}");
        //    Console.WriteLine($"bought: {transaction}");
        //    Console.WriteLine($"for {transaction} kr.");
        //    Console.WriteLine("");
        //    Console.WriteLine($"New balance: {transaction} kr");
        //    Console.WriteLine("");
        //    Console.WriteLine("");
        //    Console.WriteLine(" Press ENTER to go back");
        //    Console.WriteLine("<--");
        //    Console.ReadKey();
        //}

        //public void DisplayUserBuysProduct(int count, string transaction)
        //{
        //    Console.WriteLine("Transaction complete");
        //    Console.WriteLine($"User: {transaction}");
        //    Console.WriteLine($"bought: {count} {transaction}");
        //    Console.WriteLine($"for {transaction} kr. ({transaction}/each)");
        //    Console.WriteLine("");
        //    Console.WriteLine($"New balance: {transaction} kr");
        //    Console.WriteLine("");
        //    Console.WriteLine("");
        //    Console.WriteLine(" Press ENTER to go back");
        //    Console.WriteLine("<--");
        //    Console.ReadKey();
        //}

        //public void DisplayUserInfo(string user)
        //{
        //    //var TransactionList = Sts.GetTransactions(user, 10);

        //    Console.WriteLine($"{user} Balance: {user} kr.");
        //    Console.WriteLine("");

        //    if (16 / 100 < 50)
        //    {
        //        Console.WriteLine("Your balance is low!");
        //        Console.WriteLine("");
        //    }

        //    if (1 > 0)
        //    {
        //        Console.WriteLine("Recent transactions:");
        //    }
        //    Console.WriteLine("");
        //    Console.WriteLine("");
        //    Console.WriteLine(" Press ENTER to go back");
        //    Console.WriteLine("<--");
        //    Console.ReadKey();
        //}

        //public void DisplayUserNotFound(string username)
        //{
        //    Console.WriteLine($"The username; {username}");
        //    Console.WriteLine("does not exits as a registered user.");
        //    Console.WriteLine("Check your spelling");
        //    Console.WriteLine("");
        //    Console.WriteLine("");
        //    Console.WriteLine(" Press ENTER to go back");
        //    Console.WriteLine("<--");
        //    Console.ReadKey();
        //}

        private void Banner()
        {
            Console.WriteLine("|   _    _                     _ _     \n" +
                              "|  | |  | |                   (_) |    \n" +
                              "|  | |__| | ___ _ __ _ __ __ _ _| |_    \n" +
                              "|  |  __  |/ _ \\ '__| '_ ` _ \\| | __|   \n" +
                              "|  | |  | |  __/ |  | | | | | | | |_    \n" +
                              "|  |_|  |_|\\___|_|  |_| |_| |_|_|\\__|  ");
        }

        internal string Clr(string str, int num = -1)
        {
            int[] colors = { 160, 112, 202, 214 };
            string mNum = num == -1 ? "0" : "38;5;" + (colors[num]).ToString();
            //string mNum = num == 0 ? "0" : "38;5;" + (num % 7 + 196).ToString();
            //int theme = 23; //OG: 23
            //string mNum = num == 0 ? "0" : $"38;5;{(num % 7 + 22 + 6 * theme) % 231}";
            return $"\u001b[{mNum}m{str}\u001b[0m";
        }

    }
}