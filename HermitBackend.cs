using ConsoleHermit.Models;
using DevLib.Input;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using TextCopy;
using Console = Colorful.Console;

namespace ConsoleHermit
{
    public class HermitBackend
    {
        private HermitFileHandler hfh;
        private HermitHttpHandler hhh;
        protected const string holdsportApiStr = "https://api.holdsport.dk/v1/";
        protected const string hsRoklubbenStr = "https://api.holdsport.dk/v1/teams/219649/";

        public HermitBackend(HermitFileHandler hfh, HermitHttpHandler hhh)
        {
            this.hfh = hfh;
            this.hhh = hhh;
        }

        internal List<string> NoteList = new List<string>();

        internal void SetupNewFiles()
        {
            var emptyList = new List<string>();

            hfh.CreateNewFolder("HermitDB");

            hfh.CreateNewFile("NoteList");
            hfh.CreateNewFile("NoteListBackup");
            hfh.CreateNewFile("PathList");
            hfh.CreateNewFile("HoldsportNameList");

            Console.WriteLine("| Setup complete");
        }

        internal List<string> SearchNotes(string term)
        {
            var formattedTerm = term.Trim().ToLower();
            List<string> searchedList = new List<string>();

            foreach (var item in NoteList)
            {
                if (item.ToLower().Contains(formattedTerm))
                {
                    searchedList.Add(item);
                }
            }

            return searchedList;
        }

        internal void LoadNotesFromFile()
        {
            NoteList = hfh.LoadNoteList();
        }

        internal List<string> LoadSessionBackupNotes()
        {
            return hfh.LoadNoteList("backup");
        }

        internal void CreateBackup()
        {
            hfh.SaveChanges(NoteList, "backup");
        }

        internal void RestoreSessionBackup()
        {
            NoteList = hfh.LoadNoteList("backup");
            hfh.SaveChanges(NoteList);
            Console.WriteLine("| Session restored ");
        }

        internal void EditEntry(int itemNum)
        {
            Console.Write("> ", Color.Red);
            Keyboard.Type(NoteList[itemNum], 0); //text will be editable :)
            var EditedNote = Console.ReadLine();
            NoteList[itemNum] = EditedNote;

            hfh.SaveChanges(NoteList);

            Console.WriteLine("| Note edited ");
        }

        internal void NewEntry(string input)
        {
            NoteList.Add(input);
            hfh.SaveChanges(NoteList);
            Console.WriteLine("| Note created ");
        }

        internal void DeleteEntry(int itemNum)
        {
            NoteList.Remove(NoteList[itemNum]);
            hfh.SaveChanges(NoteList);
            Console.WriteLine("| Note deleted ");
        }

        internal void OpenDocument()
        {
            hfh.LaunchExternalFile();
        }

        internal void SetPath(int i = 0)
        {
            string path = Directory.GetCurrentDirectory();
            List<string> PathList = new List<string>();
            PathList = hfh.LoadNoteList("PathList");
            PathList.Insert(i, path);
            hfh.SaveChanges(PathList, "PathList");
            Console.WriteLine("| Path saved");
        }

        internal void GotoPath(int pathIndex = 0)
        {
            string oldClip = ClipboardService.GetText();
            //throws exception on clipboard objects (non text)
            //so setting it to oldClip now will cast the exception before other code fires
            ClipboardService.SetText(oldClip);

            List<string> PathList = new List<string>();
            PathList = hfh.LoadNoteList("PathList");

            var pathToGo = PathList[pathIndex].ToString();

            string togoDirectory = pathToGo.Substring(0, 2);

            if (!pathToGo.Equals(string.Empty))
            {
                string directoryPath = Directory.GetCurrentDirectory();

                Console.WriteLine("| Path jump started");

                if (!directoryPath.StartsWith(togoDirectory))
                {
                    Keyboard.Type(togoDirectory, 10);
                    Keyboard.Type(Key.Enter, 0);
                }

                pathToGo = "cd \"" + pathToGo + "\"";
                ClipboardService.SetText(pathToGo);

                Keyboard.Press(Key.LeftCtrl, 20);
                Keyboard.Press(Key.V, 20);
                Keyboard.Release(Key.LeftCtrl, 20);
                Keyboard.Release(Key.V, 20);

                Keyboard.Type(Key.Enter, 0);

                //Console.Clear();

                ClipboardService.SetText(oldClip);

                //Console.Clear();
            }
            else
            {
                Console.WriteLine("| Path seems empty");
            }
        }

        internal void SwitchPath(int a, int b)
        {
            List<string> PathList = new List<string>();
            PathList = hfh.LoadNoteList("PathList");

            string temp = PathList[a];
            PathList[a] = PathList[b];
            PathList[b] = temp;

            hfh.SaveChanges(PathList, "PathList");
            Console.WriteLine("| Paths switched");
        }

        internal List<string> LoadPathList()
        {
            List<string> PathList = new List<string>();
            PathList = hfh.LoadNoteList("PathList");

            return PathList;
        }

        internal void DeletePath(int pathIndex)
        {
            List<string> PathList = new List<string>();
            PathList = hfh.LoadNoteList("PathList");

            PathList.Remove(PathList[pathIndex]);

            hfh.SaveChanges(PathList, "PathList");
            Console.WriteLine("| Path deleted");
        }

        internal List<string> LoadSavedHoldsportNamesFromFile()
        {
            return hfh.LoadNoteList("HoldsportNames");
        }

        internal void AddHoldsportName(string name)
        {
            List<string> nameList = new List<string>();
            nameList = hfh.LoadNoteList("HoldsportNames");
            nameList.Add(name);
            hfh.SaveChanges(nameList, "HoldsportNames");
            Console.WriteLine("| Name saved (" + name + ")");
        }

        internal void DeleteHoldsportName(int pathIndex)
        {
            List<string> savedList = new List<string>();
            savedList = hfh.LoadNoteList("HoldsportNames");
            string name = savedList[pathIndex];
            savedList.Remove(name);

            hfh.SaveChanges(savedList, "HoldsportNames");
            Console.WriteLine($"| Name deleted ({name})");
        }

        internal List<HoldsportTeams> GetTeams()
        {
            string teams = hhh.GetJsonAsync(holdsportApiStr + "teams").Result;
            List<HoldsportTeams> teamsList = JsonConvert.DeserializeObject<List<HoldsportTeams>>(teams);
            //Console.WriteLine(activitiesArr[0].Name, Color.Green);
            //Console.WriteLine(teams);
            return teamsList;

            //Displays incomming json for analysis | KEEP
            //dynamic tempone = JsonConvert.DeserializeObject(str);
            //Console.WriteLine(tempone, Color.Red);
        }

        internal List<HoldsportActivities> GetRkActivies()
        {
            string rkActivitiesStr = hhh.GetJsonAsync(hsRoklubbenStr + "activities").Result;
            List<HoldsportActivities> rkActivities = JsonConvert.DeserializeObject<List<HoldsportActivities>>(rkActivitiesStr);
            //Console.WriteLine(rkActivities);
            return rkActivities;
        }

        internal List<HoldssportMemberModel> GetRkMembers()
        {
            string rkMembersStr = hhh.GetJsonAsync(hsRoklubbenStr + "members").Result;
            List<HoldssportMemberModel> rkMembers = JsonConvert.DeserializeObject<List<HoldssportMemberModel>>(rkMembersStr);
            return rkMembers;

        }

        internal List<HoldsportActivityModel> GetActivity(int activityID)
        {
            string rkActivityStr = hhh.GetJsonAsync(holdsportApiStr + $"/activities/{activityID}/activities_users").Result;
            var rkActivity = JsonConvert.DeserializeObject<List<HoldsportActivityModel>>(rkActivityStr);
            //Console.WriteLine(rkActivities);

            //dynamic tempone = JsonConvert.DeserializeObject(rkActivityStr);
            //Console.WriteLine(tempone, Color.Red);

            return rkActivity;
        }
    }
}