/*  This script is a part of the Henkel project
 *  Author: Branislav Juhás
 *  Date: 2022-12-03
 *  Last update: 2022-12-05
 *  
 *  --  File ( Export.cs ) Description  --
 *
 * This file contains the code to export the faults to the .kmtn or .kmtx file.
 */

using Terminal.Gui;
using System.IO;
using System.Runtime.CompilerServices;

namespace Henkel
{
    internal class Export
    {
        #region Variables

        public static List<Fault> NetsalFaults = new List<Fault>();
        public static List<Fault> XFaults = new List<Fault>();
        public static List<Fault> UndefinedFaults = new List<Fault>();

        public static string XHeader = "HUGQMEWzWBNqOXbKdeEDEzMGtLeDFCEGXebNEoVpKyzRZEPRUJLLreDEUuwVNqhubKkLbfERlKnLDFCyEoCAmNZBjZKjqSdEbjvw\r\n"
                                     + "HB  lEC  ZNV       EEn  tLl  AEy  ONo  bKq       LLy  yEQBdHNDQoBKEUafEXmKzLiTtkERhDKNMYZpKGAYFEuqZq\r\n"
                                     + "Hf  BEl  ZNW  YKKyAWEC   La  ZEk  xN  wHKc  aEMIXLLv  mEoOzINwXlvKJDacEhmrGLgzolEIgCZNGksIKDyCeExZmr\r\n"
                                     + "HT       WNg      ygEl    v  kEX    BVLFKj      kJLe  aEEAbpNrLtDKxoSIEaOgvLTWNKEtIYdNMuIEKLIMJEqalR\r\n"
                                     + "Hq  nEk  VNe  uVKSJREe  LL   eEP  x  tKpKZ  xEkQeFLq  EEzlDcNBnSTKFflrEDgcdLHfcdEjFWrNqFgJKtJNlEjCch\r\n"
                                     + "HI  fEi  pNi       REo  jLR  cEQ  eNG  UKf       MLX       INroFiKAbZvEQPhcLYOnjEzyMsNLihBKyJMxEZsnv\r\n"
                                     + "HYSoxEkOKBNmMIHKgfJpEhsAzLNtdUXTypjNDMXGKvrKdEzoFaLokEpEJuoYNxXqFKRWBJEwIOcLJJTVExAQnNzpfGKtinLExYuo";

        public static string NHeader = "HUGQMEWzWBNqOXbKdeEDEzMGtLeDFCEGXebNEoVpKyzRZEPRUJLLreDEUuwVNqhubKkLbfERlKnLDFCyEoCAmNZBjZKjqSdEbjvw\r\n"
                                     + "HB  lEC  ZNV       EEn  tLl  AEy  ONo  bKq       LLy  yEQBdHNDQoBKEUafEXmKzLiTtkERhDKNMYZpKGAYFEuqZq\r\n"
                                     + "Hf  BEl  ZNW  YKKyAWEC   La  ZEk  xN  wHKc  aEMIXLLv  mEoOzINwXlvKJDacEhmrGLgzolEIgCZNGksIKDyCeExZmr\r\n"
                                     + "HT       WNg      ygEl    v  kEX    BVLFKj      kJLe  aEEAbpNrLtDKxoSIEaOgvLTWNKEtIYdNMuIEKLIMJEqalR\r\n"
                                     + "Hq  nEk  VNe  uVKSJREe  LL   eEP  x  tKpKZ  xEkQeFLq  EEzlDcNBnSTKFflrEDgcdLHfcdEjFWrNqFgJKtJNlEjCch\r\n"
                                     + "HI  fEi  pNi       REo  jLR  cEQ  eNG  UKf       MLX       INroFiKAbZvEQPhcLYOnjEzyMsNLihBKyJMxEZsnv\r\n"
                                     + "HYSoxEkOKBNmMIHKgfJpEhsAzLNtdUNTypjEDMXGTvrKdEzoFaLokEpEJuoYNxXqFKRWBJEwIOcLJJTVExAQnNzpfGKtinLExYuo";

        private static bool Netstal = false;
        private static bool Undef = false;

        #endregion

        #region Environment

        public static Button ExportButton = new Button("Export", true);
        public static Button CancelButton = new Button("Cancel", false);

        // Dialog with all the visual elements of the export dialog
        public static Dialog Exporter = new Dialog("Export Faults", ExportButton, CancelButton)
        {
            X = Pos.Center(),
            Y = Pos.Center(),
            Width = 50,
            Height = 12,
            ColorScheme = Colors.TopLevel,
        };

        // File view to select the file to export to
        public static TextField FileNameInput = new TextField(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) + @"\")
        {
            X = 3,
            Y = 1,
            Width = 34,
            ColorScheme = Colors.Menu
        };

        public static Label BorderLabel = new Label()
        {
            X = 2,
            Y = 1,
            Text = "["
        };

        public static Label BorderLabelEnd = new Label()
        {
            X = Pos.Right(FileNameInput),
            Y = 1,
            Text = "]"
        };

        #endregion

        // Function to display and done initialization and exporter dialog
        public static void ExportDialog()
        {
            // Add event handler for Clicked event of ExportButton
            ExportButton.Clicked += () =>
            {
                if (Netstal)
                {
                    if (Undef)
                    {
                        ExportFaults(UndefinedFaults.ToArray(), 1, FileNameInput.Text.ToString() + ".kmtn");
                        UndefinedFaults.Clear();
                    }
                    else
                    {
                        ExportFaults(NetsalFaults.ToArray(), 1, FileNameInput.Text.ToString() + ".kmtn");
                        NetsalFaults.Clear();
                    }
                }
                else
                {
                    if (Undef)
                    {
                        ExportFaults(UndefinedFaults.ToArray(), 0, FileNameInput.Text.ToString() + ".kmtu");
                        UndefinedFaults.Clear();
                    }
                    else
                    {
                        ExportFaults(XFaults.ToArray(), 0, FileNameInput.Text.ToString() + ".kmt");
                        XFaults.Clear();
                    }
                }

                Application.Top.Remove(Exporter);
                Interface.ProcessedFaults.Text = $"Processing Faults: {(Processing.Faults.Count - Processing.Pending.Count).ToString()}  |  Pending: {Processing.Pending.Count.ToString()}  |  Finished: {(Export.NetsalFaults.Count + Export.XFaults.Count + Export.UndefinedFaults.Count).ToString()}";
            };

            CancelButton.Clicked += () =>
            {
                Application.Top.Remove(Exporter);
            };

            Exporter.Border.Effect3D = false;

            Exporter.Add(BorderLabel, FileNameInput, BorderLabelEnd);

            // If there are faults waiting then show dialogs for exporting
            if (NetsalFaults.Count > 0)
            {
                BorderLabelEnd.Text = "] + .kmtn";
                Netstal = true;
                Application.Top.Add(Exporter);
                Exporter.SetFocus();
                FileNameInput.SetFocus();
            }
            if (XFaults.Count > 0)
            {
                BorderLabelEnd.Text = "] + .kmtx";
                Netstal = false;
                Application.Top.Add(Exporter);
                Exporter.SetFocus();
                FileNameInput.SetFocus();
            }
            if (UndefinedFaults.Count > 0)
            {
                // Ask using message box if the user wants to export the undefined faults as netstal or x faults
                int answer = MessageBox.Query("Export undefined faults", "\nSome faults are missing the series\nclassifcation. Shall Netstal Series\nor X Series formatter be used?", "X Format", "Netstal Format");

                // Choose based on user input how to export faults and execute
                if (answer == 0)
                {
                    BorderLabelEnd.Text = "] + .kmtx";
                    Netstal = false;
                }
                else
                {
                    BorderLabelEnd.Text = "] + .kmtn";
                    Netstal = true;
                }

                Undef = true;

                Application.Top.Add(Exporter);
                Exporter.SetFocus();
                FileNameInput.SetFocus();
            }

            // If there are no faults to export, display a message
            else if (NetsalFaults.Count == 0 && XFaults.Count == 0 && UndefinedFaults.Count == 0)

            {
                MessageBox.ErrorQuery("Error", "There are no faults to export.", "OK");
            }

        }

        public static void ExportFaults(Fault[] faults, int type, string filePath)
        {
            string export;

            // If type is 0, set export to the netsal header
            // If type is 1, set export to the x header
            if (type == 0)
            {
                export = XHeader;
            }
            else
            {
                export = NHeader;
            }

            // Store date to string named 'date' using following format 7 Dec 2019
            string date = DateTime.Now.ToString("d MMM yyyy");

            foreach (Fault fault in faults)
            {
                // Add the fault to the export string
                export += $"\r\n{fault.ToString()}\t{Settings.UserName}\t{date}";
            }

            // Write the export string to the file
            WriteToFile(filePath, export);
        }

        // Function to write text to a file with the given path and text
        public static void WriteToFile(string path, string text)
        {
            try
            {
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.Write(text);
                }
            }
            catch (Exception e)
            {
                MessageBox.ErrorQuery("Error", e.Message, "OK");
            }
        }

    }
}