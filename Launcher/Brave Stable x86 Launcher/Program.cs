﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.IO;

namespace Brave_Stable_x86_Launcher
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            CultureInfo culture1 = CultureInfo.CurrentUICulture;
            string applicationPath = Application.StartupPath;
            if (File.Exists(applicationPath + "\\Brave Stable x86\\Brave.exe"))
            {
                var sb = new System.Text.StringBuilder();
                string[] CommandLineArgs = Environment.GetCommandLineArgs();
                for (int i = 1; i < CommandLineArgs.Length; i++)
                {
                    if (CommandLineArgs[i].Contains("="))
                    {
                        if (CommandLineArgs[i].Contains("LinkID"))
                        {
                            sb.Append(" " + CommandLineArgs[i]);
                        }
                        else if (CommandLineArgs[i].Contains("http"))
                        {
                            sb.Append(" \"" + CommandLineArgs[i] + "\"");
                        }
                        else
                        {
                            string[] test = CommandLineArgs[i].Split(new char[] { '=' }, 2);
                            sb.Append(" " + test[0] + "=\"" + test[1] + "\"");
                        }
                    }
                    else if (CommandLineArgs[i].Contains(".pdf"))
                    {
                        sb.Append(" \"" + CommandLineArgs[i] + "\"");
                    }
                    else
                    {
                        sb.Append(" " + CommandLineArgs[i]);
                    }
                }
                if (!File.Exists(applicationPath + "\\Brave Stable x86\\Profile.txt"))
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);
                    Application.Run(new Form1());
                    string Arguments = File.ReadAllText(applicationPath + "\\Brave Stable x86\\Profile.txt") + sb.ToString();
                    if (Arguments.Contains("--user-data-dir="))
                    {
                        string[] Arguments2 = Arguments.Split(new char[] { '=' }, 2);
                        string Arguments3 = Arguments2[0] + "='\"" + applicationPath + "\\" + Arguments2[1].Remove(0, 1) + "'";
                        Process.Start(applicationPath + "\\Brave Stable x86\\Brave.exe", Arguments3.Replace("'", ""));
                    }
                    else
                    {
                        Process.Start(applicationPath + "\\Brave Stable x86\\Brave.exe", Arguments);
                    }
                }
                else
                {
                    string Arguments = File.ReadAllText(applicationPath + "\\Brave Stable x86\\Profile.txt") + sb.ToString();
                    if (Arguments.Contains("--user-data-dir="))
                    {
                        string[] Arguments2 = Arguments.Split(new char[] { '=' }, 2);
                        string Arguments3 = Arguments2[0] + "='\"" + applicationPath + "\\" + Arguments2[1].Remove(0, 1) + "'";
                        Process.Start(applicationPath + "\\Brave Stable x86\\Brave.exe", Arguments3.Replace("'", ""));
                    }
                    else
                    {
                        Process.Start(applicationPath + "\\Brave Stable x86\\Brave.exe", Arguments);
                    }
                }
            }
            else if (culture1.TwoLetterISOLanguageName == "de")
            {
                MessageBox.Show("Brave ist nicht installiert", "Brave Stable x86 Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (culture1.TwoLetterISOLanguageName == "ru")
            {
                MessageBox.Show("Brave Portable не найден", "Brave Stable x86 Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                MessageBox.Show("Brave is not installed", "Brave Stable x86 Launcher", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
