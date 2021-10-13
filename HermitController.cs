using System;
using System.Collections.Generic;
using System.Diagnostics;
using TextCopy;

namespace ConsoleHermit
{
    public class HermitController
    {
        private HermitBackend hbe;
        private HermitUI hui;

        public HermitController(HermitBackend hbe, HermitUI hui)
        {
            this.hbe = hbe;
            this.hui = hui;
            hui.CommandEntered += ParseCommands;
        }

        public void ParseTerminalArgs(string[] argArr)
        {
            if (Debugger.IsAttached)
            {
                Console.WriteLine("| Type \"setup\" to create new files (overwrite)");
                var str = Console.ReadLine();
                if (str.Equals("setup")) Setup();
            }
            else
            {
                foreach (var arg in argArr)
                {
                    if (arg.Equals("setup"))
                    {
                        Setup();
                    }
                }
            }

            hbe.LoadNotesFromFile();
            hbe.CreateBackup();

            List<string> mod = new List<string>();
            string nonCommandItems = "";

            foreach (string ar in argArr)
            {
                if (ar.StartsWith("-"))
                    mod.Add(ar);
                else
                    nonCommandItems += ar + " ";
            }

            string ParsableString = "";

            foreach (var item in mod)
            {
                ParsableString += item + " ";
            }
            ParsableString += nonCommandItems;

            ParseCommands(this, ParsableString);
        }

        public void ParseCommands(object sender, string command)
        {
            string[] com = command.Split(" ");

            List<string> modifiers = new List<string>();
            string nonCommandItems = "";

            if (command.Equals(string.Empty))
            {
                hui.DisplayHelpMsg();
            }

            foreach (string ar in com)
            {
                if (ar.StartsWith("-"))
                    modifiers.Add(ar);
                else
                    nonCommandItems += ar + " ";
            }

            foreach (var mod in modifiers)
            {
                if (mod.Equals("-q") || mod.Equals("-quit") || mod.Equals("-x"))
                {
                    hui.Close();
                }
                else if (mod.Equals("-n"))
                {
                    NewNoteEntryModifier(nonCommandItems);
                }
                else if (mod.Equals("-e"))
                {
                    EditNoteEntryModifier(nonCommandItems);
                }
                else if (mod.Equals("-d"))
                {
                    DeleteNoteEntryModifier(nonCommandItems);
                }
                else if (mod.Equals("-o"))
                {
                    OpenDocument();
                }
                else if (mod.Equals("-rs"))
                {
                    hbe.RestoreSessionBackup();
                }
                else if (mod.Equals("-vs"))
                {
                    hui.ViewSessionBackup();
                }
                else if (mod.Equals("-s"))
                {
                    hui.SearchNoteList(nonCommandItems);
                }
                else if (mod.Equals("-sp"))
                {
                    SetNewPath(nonCommandItems);
                }
                else if (mod.Equals("-p"))
                {
                    GotoStoredPath(nonCommandItems);
                }
                else if (mod.Equals("-wp"))
                {
                    SwitchPathIndex(nonCommandItems);
                }
                else if (mod.Equals("-vp"))
                {
                    hui.DisplayPathListContent();
                }
                else if (mod.Equals("-dp"))
                {
                    DeletePath(nonCommandItems);
                }
                else if (mod.Equals("-ha"))
                {
                    hui.DisplayRkActivities();
                }
                else if (mod.Equals("-h"))
                {
                    ViewHoldsportActivity(nonCommandItems);
                }
                else if (mod.Equals("-hv"))
                {
                    hui.DisplaySavedHoldsportNames();
                }
                else if (mod.Equals("-hn"))
                {
                    AddHoldsportName(nonCommandItems);
                }
                else if (mod.Equals("-hd"))
                {
                    DeleteHoldsportName(nonCommandItems);
                }
                else if (mod.Equals("-hm"))
                {
                    hui.DisplayRkMembers();
                }
                else if (mod.Equals("-ui")) //Always add commands before -ui
                {
                    hui.Start();
                }
                else
                {
                    Console.WriteLine("|");
                    Console.WriteLine("| " + mod + " not recognized");
                    hui.DisplayHelpMsg();
                }
            }
        }

        private bool NewNoteEntryModifier(string inputText)
        {
            hbe.NewEntry(inputText);
            return true;
        }

        private bool EditNoteEntryModifier(string indexToEdit)
        {
            try
            {
                hbe.EditEntry(int.Parse(indexToEdit));
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                hui.DisplayIndexInputError(indexToEdit);
                return false;
            }
            catch (FormatException)
            {
                hui.DisplayNotANumberError(indexToEdit);
                return false;
            }
        }

        private bool DeleteNoteEntryModifier(string indexToDelete)
        {
            try
            {
                if (!hui.inUI)
                {
                    var item = hbe.NoteList[int.Parse(indexToDelete)];
                    if (!hui.ConfirmAction(item, "DELETE"))
                    {
                        return false;
                    }
                }

                hbe.DeleteEntry(int.Parse(indexToDelete));
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                hui.DisplayIndexInputError(indexToDelete);
                return false;
            }
            catch (FormatException)
            {
                hui.DisplayNotANumberError(indexToDelete);
                return false;
            }
        }

        private bool OpenDocument()
        {
            hbe.OpenDocument();
            return true;
        }

        private bool GotoStoredPath(string index)
        {
            try
            {
                if (index.Trim() != string.Empty)
                    hbe.GotoPath(int.Parse(index));
                else
                    hbe.GotoPath();
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                hui.DisplayIndexInputError(index);
                return false;
            }
            catch (FormatException)
            {
                hui.DisplayNotANumberError(index);
                return false;
            }
            catch (ArgumentNullException)
            {
                if (hui.ConfirmAction("Your CLIPBOARD will be overwritten", "OVERWRITE"))
                {
                    ClipboardService.SetText("");

                    if (index.Trim() != string.Empty)
                        hbe.GotoPath(int.Parse(index));
                    else
                        hbe.GotoPath();
                    return true;
                }
                return false;
            }
        }

        private bool SetNewPath(string InsertIndex)
        {
            try
            {
                if (InsertIndex.Trim() != string.Empty)
                {
                    hbe.SetPath(int.Parse(InsertIndex));
                }
                else
                    hbe.SetPath();
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                hui.DisplayIndexInputError(InsertIndex);
                return false;
            }
            catch (FormatException)
            {
                hui.DisplayNotANumberError(InsertIndex);
                return false;
            }
        }

        private bool SwitchPathIndex(string indexDuo)
        {
            try
            {
                var indexSplit = indexDuo.Split(" ");
                hbe.SwitchPath(int.Parse(indexSplit[0]), int.Parse(indexSplit[1]));
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                hui.DisplayIndexInputError(indexDuo);
                return false;
            }
            catch (FormatException)
            {
                hui.DisplayNotANumberError(indexDuo);
                return false;
            }
        }

        private bool DeletePath(string indexToDelete)
        {
            try
            {
                if (!hui.inUI)
                {
                    var item = hbe.LoadPathList();
                    if (!hui.ConfirmAction(item[int.Parse(indexToDelete)], "DELETE"))
                    {
                        return false;
                    }
                }

                hbe.DeletePath(int.Parse(indexToDelete));
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                hui.DisplayIndexInputError(indexToDelete);
                return false;
            }
            catch (FormatException)
            {
                hui.DisplayNotANumberError(indexToDelete);
                return false;
            }
        }

        private bool Setup()
        {
            hbe.SetupNewFiles();

            return true;
        }

        private bool ViewHoldsportActivity(string indexToEdit)
        {
            try
            {
                if (indexToEdit.Trim() != String.Empty) hui.DisplayActivity(int.Parse(indexToEdit));
                else hui.DisplayActivity();
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                hui.DisplayIndexInputError(indexToEdit);
                return false;
            }
            catch (FormatException)
            {
                hui.DisplayNotANumberError(indexToEdit);
                return false;
            }
        }

        private bool AddHoldsportName(string indexToAdd)
        {
            try
            {
                List<HoldssportMemberModel> members = hbe.GetRkMembers();
                HoldssportMemberModel mem = members[int.Parse(indexToAdd)];

                hbe.AddHoldsportName(mem.Firstname + " " + mem.Lastname);
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                hui.DisplayIndexInputError(indexToAdd);
                return false;
            }
            catch (FormatException)
            {
                hui.DisplayNotANumberError(indexToAdd);
                return false;
            }
        }

        private bool DeleteHoldsportName(string indexToDelete)
        {
            try
            {
                var item = hbe.LoadSavedHoldsportNamesFromFile();
                if (!hui.ConfirmAction(item[int.Parse(indexToDelete)], "DELETE"))
                {
                    return false;
                }

                hbe.DeleteHoldsportName(int.Parse(indexToDelete));
                return true;
            }
            catch (ArgumentOutOfRangeException)
            {
                hui.DisplayIndexInputError(indexToDelete);
                return false;
            }
            catch (FormatException)
            {
                hui.DisplayNotANumberError(indexToDelete);
                return false;
            }
        }
    }
}