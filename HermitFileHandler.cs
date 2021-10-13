using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Console = Colorful.Console;

namespace ConsoleHermit
{
    public class HermitFileHandler
    {
        public string CurrentDirectory;
        public string NoteDocPath;
        public string NoteDocBackupPath;
        public string PathDocPath;
        public string HoldsportNameDocPath;

        public HermitFileHandler()
        {
            CurrentDirectory = AppDomain.CurrentDomain.BaseDirectory + "HermitDB\\";

            NoteDocPath = CurrentDirectory + "NoteList" + ".txt";
            NoteDocBackupPath = CurrentDirectory + "NoteListBackup" + ".txt";
            PathDocPath = CurrentDirectory + "PathList" + ".txt";
            HoldsportNameDocPath = CurrentDirectory + "HoldsportNameList" + ".txt";
        }

        //CreateAndWriteToFile("Hermit\\test.txt", mod);

        public void SaveChanges(List<string> str, string type = "who cares")
        {
            if (type == "backup")
                File.WriteAllLines(NoteDocBackupPath, str);
            else if (type == "PathList")
                File.WriteAllLines(PathDocPath, str);
            else if (type == "HoldsportNames")
                File.WriteAllLines(HoldsportNameDocPath, str);
            else
                File.WriteAllLines(NoteDocPath, str);
            //WriteAllTextAsync
        }

        public bool LaunchExternalFile(string fileName = "Who cares")
        {
            if (fileName.Equals("Who cares"))
                fileName = NoteDocPath;

            Process p = new Process();
            ProcessStartInfo pi = new ProcessStartInfo();
            pi.UseShellExecute = true;
            pi.FileName = fileName;
            p.StartInfo = pi;
            p.Start();

            return true;
        }

        public List<string> LoadNoteList(string type = "who cares")
        {
            string[] strs;

            if (type == "backup")
                strs = File.ReadAllLines(NoteDocBackupPath);
            else if (type == "PathList")
                strs = File.ReadAllLines(PathDocPath);
            else if (type == "HoldsportNames")
                strs = File.ReadAllLines(HoldsportNameDocPath);
            else
                strs = File.ReadAllLines(NoteDocPath);

            List<string> Notes = new List<string>(strs);

            return Notes;
        }

        public bool CreateNewFile(string fileName)
        {

            File.Create(CurrentDirectory + fileName + ".txt").Dispose();
            Console.WriteLine("| File created: " + CurrentDirectory + fileName + ".txt");
            return true;
        }

        public bool CreateNewFolder(string folderName)
        {
            Directory.CreateDirectory(CurrentDirectory);
            Console.WriteLine("| Folder created: " + CurrentDirectory + folderName);
            return true;
        }
    }
}