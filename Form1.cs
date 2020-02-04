using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;

namespace Brave_Updater
{
    public partial class Form1 : Form
    {
        public static string[] ring = new string[4] { "Nightly", "Dev", "Beta", "Stable" };
        public static string[] arappid = new string[4] { "C6CB981E-DB30-4876-8639-109F8933582C", "CB2150F2-595F-4633-891A-E39720CE0531", "103BD053-949B-43A8-9120-2E424887DE11", "AFE6A462-C574-4B8A-AF43-4CC60DF4563B" };
        public static string[] arapVersion = new string[8] { "x86-ni", "x86-dev", "x86-be", "x86-rel", "x64-ni", "x64-dev", "x64-be", "x64-rel" };
        public static string[] ring2 = new string[8] { "Nightly", "Developer", "Beta", "Stable", "Nightly", "Developer", "Beta", "Stable" };
        public static string[] buildversion = new string[8];
        public static string[] newVersion = new string[4];
        public static string[] architektur = new string[2] { "X86", "X64" };
        public static string[] architektur2 = new string[2] { "x86", "x64" };
        public static string[] instDir = new string[9] { "Brave Nightly x86", "Brave Dev x86", "Brave Beta x86", "Brave Stable x86", "Brave Nightly x64", "Brave Dev x64", "Brave Beta x64", "Brave Stable x64", "Brave" };
        public static string[] entpDir = new string[9] { "Nightly86", "Dev86", "Beta86", "Stable86", "Nightly64", "Dev64", "Beta64", "Stable64", "Single" };
        public static string[] icon = new string[4] { "4", "8", "9", "0" };
        WebClient webClient;
        readonly string deskDir = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
        readonly string applicationPath = Application.StartupPath;
        readonly CultureInfo culture1 = CultureInfo.CurrentUICulture;
        readonly ToolTip toolTip = new ToolTip();
        public Form1()
        {
            InitializeComponent();
            for (int i = 0; i <= 3; i++)
            {
                WebRequest request = WebRequest.Create("https://updates.bravesoftware.com/service/update2");
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] byteArray = Encoding.UTF8.GetBytes("<?xml version =\"1.0\" encoding=\"UTF-8\"?><request protocol=\"3.0\" version=\"1.3.99.0\" shell_version=\"1.3.99.0\" ismachine=\"1\" sessionid=\"{11111111-1111-1111-1111-111111111111}\" installsource=\"taggedmi\" testsource=\"auto\" requestid=\"{11111111-1111-1111-1111-111111111111}\" dedup=\"cr\"><os platform=\"win\" version=\"\" sp=\"\" arch=\"x86\"/><app appid=\"{" + arappid[i] + "}\" version=\"\" nextversion=\"\" ap=\"" + arapVersion[i] + "\" lang=\"en\" brand=\"\" client=\"\" installage=\"-1\" installdate=\"-1\"><updatecheck/></app></request>");
                request.ContentLength = byteArray.Length;
                Stream dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                WebResponse response = request.GetResponse();
                using (dataStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(dataStream);
                    string responseFromServer = reader.ReadToEnd();
                    string[] URL = responseFromServer.Substring(responseFromServer.IndexOf("manifest version=")).Split(new char[] { '"' });
                    string[] version = URL[1].Split(new char[] { '.' });
                    newVersion[i] = version[1] + "." + version[2] + "." + version[3];
                    buildversion[i] = URL[1];
                    buildversion[i + 4] = URL[1];
                    dataStream.Close();
                }
            }
            label5.Text = newVersion[0];
            label6.Text = newVersion[1];
            label7.Text = newVersion[2];
            label8.Text = newVersion[3];
            button9.Enabled = false;
            checkBox1.Enabled = false;
            checkBox2.Enabled = false;
            if (culture1.Name != "de-DE")
            {
                button10.Text = "Quit";
                button9.Text = "Install all";
                label9.Text = "Install all x86 and or x64";
                checkBox4.Text = "Ignore version check";
                checkBox1.Text = "Create a Folder for each version";
                checkBox5.Text = "Create a shortcut on the desktop";
                if (IntPtr.Size != 8)
                {
                    label9.Text = "Install all x86";
                }
            }
            if (IntPtr.Size == 8)
            {
                if (File.Exists(@"Brave Nightly x64\Brave.exe") || File.Exists(@"Brave Dev x64\Brave.exe") || File.Exists(@"Brave Beta x64\Brave.exe") || File.Exists(@"Brave Stable x64\Brave.exe"))
                {
                    checkBox2.Enabled = false;
                }
                if (File.Exists(@"Brave Nightly x86\Brave.exe") || File.Exists(@"Brave Dev x86\Brave.exe") || File.Exists(@"Brave Beta x86\Brave.exe") || File.Exists(@"Brave Stable x86\Brave.exe"))
                {
                    checkBox1.Enabled = false;
                }
                if (File.Exists(@"Brave Nightly x86\Brave.exe") || File.Exists(@"Brave Dev x86\Brave.exe") || File.Exists(@"Brave Beta x86\Brave.exe") || File.Exists(@"Brave Stable x86\Brave.exe") || File.Exists(@"Brave Nightly x64\Brave.exe") || File.Exists(@"Brave Dev x64\Brave.exe") || File.Exists(@"Brave Beta x64\Brave.exe") || File.Exists(@"Brave Stable x64\Brave.exe"))
                {
                    checkBox3.Checked = true;
                    CheckButton();
                }
                else if (!checkBox3.Checked)
                {
                    checkBox1.Enabled = false;
                    checkBox2.Enabled = false;
                    button9.Enabled = false;
                    button9.BackColor = Color.FromArgb(244, 244, 244);

                    if (File.Exists(@"Brave\Brave.exe"))
                    {
                        CheckButton2();
                    }
                }
            }
            else if (IntPtr.Size != 8)
            {
                button5.Visible = false;
                button6.Visible = false;
                button7.Visible = false;
                button8.Visible = false;
                checkBox2.Visible = false;
                if (File.Exists(@"Brave Nightly x86\Brave.exe") || File.Exists(@"Brave Dev x86\Brave.exe") || File.Exists(@"Brave Beta x86\Brave.exe") || File.Exists(@"Brave Stable x86\Brave.exe"))
                {
                    checkBox3.Checked = true;
                    checkBox1.Enabled = false;
                    CheckButton();
                }
                else if (!checkBox3.Checked)
                {
                    checkBox1.Enabled = false;
                    button9.Enabled = false;
                    button9.BackColor = Color.FromArgb(244, 244, 244);

                    if (File.Exists(@"Brave\Brave.exe"))
                    {
                        CheckButton2();
                    }
                }
            }
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                NewMethod(0, 0, 0, 1, 10, 11, 1);
            }
            else if (!checkBox3.Checked)
            {
                NewMethod1(0, 0, 1);
            }
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                NewMethod(1, 1, 0, 2, 12, 13, 2);
            }
            if (!checkBox3.Checked)
            {
                NewMethod1(1, 0, 2);
            }
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                NewMethod(2, 2, 0, 3, 14, 15, 3);
            }
            else if (!checkBox3.Checked)
            {
                NewMethod1(2, 0, 3);
            }
        }
        private void Button4_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                NewMethod(3, 3, 0, 4, 16, 17, 4);
            }
            else if (!checkBox3.Checked)
            {
                NewMethod1(3, 0, 4);
            }
        }
        private void Button5_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                NewMethod(0, 4, 1, 5, 18, 19, 5);
            }
            else if (!checkBox3.Checked)
            {
                NewMethod1(0, 1, 5);
            }
        }
        private void Button6_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                NewMethod(1, 5, 1, 6, 20, 21, 6);
            }
            else if (!checkBox3.Checked)
            {
                NewMethod1(1, 1, 6);
            }
        }
        private void Button7_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                NewMethod(2, 6, 1, 7, 22, 23, 7);
            }
            else if (!checkBox3.Checked)
            {
                NewMethod1(2, 1, 7);
            }
        }
        private void Button8_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                NewMethod(3, 7, 1, 8, 24, 25, 8);
            }
            else if (!checkBox3.Checked)
            {
                NewMethod1(3, 1, 8);
            }
        }
        private void Button9_Click(object sender, EventArgs e)
        {
            if ((!Directory.Exists(@"Brave Nightly x86")) && (!Directory.Exists(@"Brave Dev x86")) && (!Directory.Exists(@"Brave Beta x86")) && (!Directory.Exists(@"Brave Stable x86")))
            {
                if (checkBox1.Checked)
                {
                    DownloadFile(0, 0, 0, 1, 10, 11, 1);
                    DownloadFile(1, 1, 0, 2, 12, 13, 2);
                    DownloadFile(2, 2, 0, 3, 14, 15, 3);
                    DownloadFile(3, 3, 0, 4, 16, 17, 4);
                    checkBox1.Enabled = false;
                }
            }
            NewMethod2(0, 0, 0, 1, 10, 11, 1);
            NewMethod2(1, 1, 0, 2, 12, 13, 2);
            NewMethod2(2, 2, 0, 3, 14, 15, 3);
            NewMethod2(3, 3, 0, 4, 16, 17, 4);
            if (IntPtr.Size == 8)
            {
                if ((!Directory.Exists(@"Brave Nightly x64")) && (!Directory.Exists(@"Brave Dev x64")) && (!Directory.Exists(@"Brave Beta x64")) && (!Directory.Exists(@"Brave Stable x64")))
                {
                    if (checkBox2.Checked)
                    {
                        DownloadFile(0, 4, 1, 5, 18, 19, 5);
                        DownloadFile(1, 5, 1, 6, 20, 21, 6);
                        DownloadFile(2, 6, 1, 7, 22, 23, 7);
                        DownloadFile(3, 7, 1, 8, 24, 25, 8);
                        checkBox2.Enabled = false;
                    }
                }
                NewMethod2(0, 4, 1, 5, 18, 19, 5);
                NewMethod2(1, 5, 1, 6, 20, 21, 6);
                NewMethod2(2, 6, 1, 7, 22, 23, 7);
                NewMethod2(3, 7, 1, 8, 24, 25, 8);
            }
        }
        public void DownloadFile(int i, int f, int e, int a, int b, int c, int d)
        {
            WebRequest request = WebRequest.Create("https://updates.bravesoftware.com/service/update2");
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            byte[] byteArray = Encoding.UTF8.GetBytes("<?xml version =\"1.0\" encoding=\"UTF-8\"?><request protocol=\"3.0\" version=\"1.3.99.0\" shell_version=\"1.3.99.0\" ismachine=\"1\" sessionid=\"{11111111-1111-1111-1111-111111111111}\" installsource=\"taggedmi\" testsource=\"auto\" requestid=\"{11111111-1111-1111-1111-111111111111}\" dedup=\"cr\"><os platform=\"win\" version=\"\" sp=\"\" arch=\"x86\"/><app appid=\"{" + arappid[i] + "}\" version=\"\" nextversion=\"\" ap=\"" + arapVersion[d - 1] + "\" lang=\"en\" brand=\"\" client=\"\" installage=\"-1\" installdate=\"-1\"><updatecheck/></app></request>");
            request.ContentLength = byteArray.Length;
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            WebResponse response = request.GetResponse();
            using (dataStream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(dataStream);
                string responseFromServer = reader.ReadToEnd();
                string[] tempURL2 = responseFromServer.Substring(responseFromServer.LastIndexOf("codebase=")).Split(new char[] { '"' });
                string[] tempURL4 = responseFromServer.Substring(responseFromServer.IndexOf("name=")).Split(new char[] { '"' });
                Uri uri = new Uri(tempURL2[1] + tempURL4[1]);
                ServicePoint sp = ServicePointManager.FindServicePoint(uri);
                sp.ConnectionLimit = 2;
                dataStream.Close();
                using (webClient = new WebClient())
                {
                    webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
                    webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
                    try
                    {
                        webClient.DownloadFileAsync(uri, "Brave_" + architektur[e] + "_" + buildversion[i] + "_" + ring[i] + ".exe", a + "|" + b + "|" + c + "|" + d + "|" + "Brave_" + architektur[e] + "_" + buildversion[i] + "_" + ring[i] + ".exe" + "|" + architektur2[e] + "|" + i + "|" + f);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            string[] i = e.UserState.ToString().Split(new char[] { '|' });
            Control[] progressBars = Controls.Find("progressBar" + i[0], true);
            Control[] buttons = Controls.Find("button" + i[3], true);
            Control[] label1 = Controls.Find("label" + i[1], true);
            Control[] label2 = Controls.Find("label" + i[2], true);
            if (buttons.Length > 0)
            {
                Button button = (Button)buttons[0];
                button.BackColor = Color.Orange;
            }
            if (progressBars.Length > 0)
            {
                ProgressBar progressBar = (ProgressBar)progressBars[0];
                progressBar.Visible = true;
                progressBar.Value = e.ProgressPercentage;
            }
            if (label1.Length > 0)
            {
                Label label = (Label)label1[0];
                label.Visible = true;
                label.Text = string.Format("{0} MB's / {1} MB's",
                (e.BytesReceived / 1024d / 1024d).ToString("0.00"),
                (e.TotalBytesToReceive / 1024d / 1024d).ToString("0.00"));
            }
            if (label2.Length > 0)
            {
                Label label3 = (Label)label2[0];
                label3.Visible = true;
                label3.Text = e.ProgressPercentage.ToString() + "%";
            }
        }
        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            string[] i = e.UserState.ToString().Split(new char[] { '|' });
            int b = int.Parse(i[1]);
            int d = int.Parse(i[7]);
            Control[] labels = Controls.Find("label" + b, true);
            Label label = (Label)labels[0];
            if (e.Cancelled == true)
            {
                MessageBox.Show("Download has been canceled.");
            }
            else
            {
                if (labels.Length > 0)
                {
                    label.Text = culture1.Name != "de-DE" ? "Unpacking" : "Entpacken";
                    string arguments = " x " + i[4] + " -o" + @"Update\" + entpDir[d] + " -y";
                    Process process = new Process();
                    process.StartInfo.FileName = @"Bin\7zr.exe";
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                    process.StartInfo.Arguments = arguments;
                    process.Start();
                    process.WaitForExit();
                    process.StartInfo.Arguments = " x " + @"Update\" + entpDir[d] + "\\Chrome.7z -o" + @"Update\" + entpDir[d] + " -y";
                    process.Start();
                    process.WaitForExit();
                    int a = int.Parse(i[3]);
                    if ((File.Exists(@"Update\" + entpDir[d] + "\\chrome-bin\\Brave.exe")) && (File.Exists(instDir[d] + "\\updates\\Version.log")))
                    {
                        string[] instVersion = File.ReadAllText(instDir[d] + "\\updates\\Version.log").Split(new char[] { '|' });
                        FileVersionInfo testm = FileVersionInfo.GetVersionInfo(applicationPath + "\\Update\\" + entpDir[d] + "\\chrome-bin\\Brave.exe");
                        if (checkBox3.Checked)
                        {
                            if (testm.FileVersion != instVersion[0])
                            {
                                if (Directory.Exists(instDir[d] + "\\" + instVersion[0]))
                                {
                                    Directory.Delete(instDir[d] + "\\" + instVersion[0], true);
                                }
                                Thread.Sleep(2000);
                                NewMethod4(i, a, testm, d);
                            }
                            else if ((testm.FileVersion == instVersion[0]) && (checkBox4.Checked))
                            {
                                if (Directory.Exists(instDir[d] + "\\" + instVersion[0]))
                                {
                                    Directory.Delete(instDir[d] + "\\" + instVersion[0], true);
                                }
                                Thread.Sleep(2000);
                                NewMethod4(i, a, testm, d);
                            }
                        }
                        else if (!checkBox3.Checked)
                        {
                            if (Directory.Exists(instDir[d] + "\\" + instVersion[0]))
                            {
                                Directory.Delete(instDir[d] + "\\" + instVersion[0], true);
                            }
                            Thread.Sleep(2000);
                            NewMethod4(i, a, testm, d);
                        }
                    }
                    else
                    {
                        if (!Directory.Exists(instDir[d]))
                        {
                            Directory.CreateDirectory(instDir[d]);
                        }
                        NewMethod4(i, a, FileVersionInfo.GetVersionInfo(applicationPath + "\\Update\\" + entpDir[d] + "\\chrome-bin\\Brave.exe"), d);
                    }
                }
            }
            int c = int.Parse(i[6]);
            if (checkBox5.Checked)
            {
                if (!File.Exists(deskDir + "\\" + instDir[d] + ".lnk"))
                {
                    NewMethod5(c, d);
                }
            }
            else if (File.Exists(deskDir + "\\" + instDir[d] + ".lnk") && (instDir[d] == "Brave"))
            {
                NewMethod5(c, d);
            }
            if (!File.Exists(@instDir[d] + " Launcher.exe"))
            {
                File.Copy(@"Bin\Launcher\" + instDir[d] + " Launcher.exe", @instDir[d] + " Launcher.exe");
            }
            File.Delete(i[4]);
            label.Text = culture1.Name != "de-DE" ? "Unpacked" : "Entpackt";
        }
        public void CheckButton()
        {
            NewMethod3();
            for (int i = 0; i <= 7; i++)
            {
                if (File.Exists(@instDir[i] + "\\updates\\Version.log"))
                {
                    Control[] buttons = Controls.Find("button" + (i + 1), true);
                    string[] instVersion = File.ReadAllText(@instDir[i] + "\\updates\\Version.log").Split(new char[] { '|' });
                    if (buildversion[i] == instVersion[0])
                    {
                        if (buttons.Length > 0)
                        {
                            Button button = (Button)buttons[0];
                            button.BackColor = Color.Green;
                        }
                    }
                    else if (buildversion[i] != instVersion[0])
                    {
                        if (culture1.Name != "de-DE")
                        {
                            button9.Text = "Update all";
                        }
                        else
                        {
                            button9.Text = "Alle Updaten";
                        }
                        button9.Enabled = true;
                        button9.BackColor = Color.FromArgb(224, 224, 224);
                        if (buttons.Length > 0)
                        {
                            Button button = (Button)buttons[0];
                            button.BackColor = Color.Red;
                        }
                    }
                }
            }
        }
        public void CheckBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                if (File.Exists(@"Brave Nightly x64\Brave.exe") || File.Exists(@"Brave Dev x64\Brave.exe") || File.Exists(@"Brave Beta x64\Brave.exe") || File.Exists(@"Brave Stable x64\Brave.exe"))
                {
                    checkBox2.Enabled = false;
                }
                else
                {
                    checkBox2.Enabled = true;
                }
                if (File.Exists(@"Brave Nightly x86\Brave.exe") || File.Exists(@"Brave Dev x86\Brave.exe") || File.Exists(@"Brave Beta x86\Brave.exe") || File.Exists(@"Brave Stable x86\Brave.exe"))
                {
                    checkBox1.Enabled = false;
                }
                else
                {
                    checkBox1.Enabled = true;
                }
                if (button9.Enabled)
                {
                    button9.BackColor = Color.FromArgb(224, 224, 224);
                }
                CheckButton();
            }
            if (!checkBox3.Checked)
            {
                checkBox1.Enabled = false;
                checkBox2.Enabled = false;
                button9.Enabled = false;
                button9.BackColor = Color.FromArgb(244, 244, 244);
                CheckButton2();
            }
        }
        public void CheckButton2()
        {
            NewMethod3();
            if (File.Exists(@"Brave\updates\Version.log"))
            {
                string[] instVersion = File.ReadAllText(@"Brave\updates\Version.log").Split(new char[] { '|' });
                switch (instVersion[1])
                {
                    case "Nightly":
                        NewMethod6(instVersion, 1, 5, 0);
                        break;
                    case "Developer":
                        NewMethod6(instVersion, 2, 6, 1);
                        break;
                    case "Beta":
                        NewMethod6(instVersion, 3, 7, 2);
                        break;
                    case "Stable":
                        NewMethod6(instVersion, 4, 8, 3);
                        break;
                }
            }
        }
        private void Button1_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(0, "x86");
        }
        private void Button2_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(1, "x86");
        }
        private void Button3_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(2, "x86");
        }
        private void Button4_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(3, "x86");
        }
        private void Button5_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(4, "x64");
        }
        private void Button6_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(5, "x64");
        }
        private void Button7_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(6, "x64");
        }
        private void Button8_MouseHover(object sender, EventArgs e)
        {
            NewMethod7(7, "x64");
        }
        public void Message1()
        {
            if (culture1.Name != "de-DE")
            {
                MessageBox.Show("The same version is already installed", "Portabel Brave Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else
            {
                MessageBox.Show("Die selbe Version ist bereits installiert", "Portabel Brave Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
        }
        private void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                button9.Enabled = true;
                button9.BackColor = Color.FromArgb(224, 224, 224);
            }
            else if ((!checkBox1.Checked) && (!checkBox2.Checked))
            {
                button9.Enabled = false;
                button9.BackColor = Color.FromArgb(244, 244, 244);
            }
        }
        private void CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                button9.Enabled = true;
                button9.BackColor = Color.FromArgb(224, 224, 224);
            }
            else if ((!checkBox1.Checked) && (!checkBox2.Checked))
            {
                button9.Enabled = false;
                button9.BackColor = Color.FromArgb(244, 244, 244);
            }
        }
        private void Button10_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Directory.Exists(@"Update"))
            {
                Directory.Delete(@"Update", true);
            }
        }
        private void Button9_EnabledChanged(object sender, EventArgs e)
        {
            if (!button9.Enabled)
            {
                button9.BackColor = Color.FromArgb(244, 244, 244);
            }
        }
        private void NewMethod(int a, int b, int c, int d, int e, int f, int g)
        {
            if (File.Exists(@instDir[b] + "\\updates\\Version.log"))
            {
                if (File.ReadAllText(instDir[a] + "\\updates\\Version.log").Split(new char[] { '|' })[0] == buildversion[a])
                {
                    if (checkBox4.Checked)
                    {
                        DownloadFile(a, b, c, d, e, f, g);
                    }
                    else
                    {
                        Message1();
                    }
                }
                else
                {
                    DownloadFile(a, b, c, d, e, f, g);
                }
            }
            else
            {
                DownloadFile(a, b, c, d, e, f, g);
            }
        }
        private void NewMethod1(int a, int b, int c)
        {
            if (File.Exists(@"Brave\updates\Version.log"))
            {
                string[] instVersion = File.ReadAllText(@"Brave\updates\Version.log").Split(new char[] { '|' });
                if ((instVersion[0] == buildversion[a]) && (instVersion[1] == ring2[a]) && (instVersion[2] == architektur2[b]))
                {
                    if (checkBox4.Checked)
                    {
                        DownloadFile(a, 8, b, 1, 10, 11, c);
                    }
                    else
                    {
                        Message1();
                    }
                }
                else
                {
                    DownloadFile(a, 8, b, 1, 10, 11, c);
                }
            }
            else
            {
                DownloadFile(a, 8, b, 1, 10, 11, c);
            }
        }
        private void NewMethod2(int a, int b, int c, int d, int e, int f, int g)
        {
            if (Directory.Exists(instDir[b]))
            {
                if (File.Exists(instDir[b] + "\\updates\\Version.log"))
                {
                    if (File.ReadAllText(instDir[b] + "\\updates\\Version.log").Split(new char[] { '|' })[0] != buildversion[a])
                    {
                        DownloadFile(a, b, c, d, e, f, g);
                    }
                }
            }
        }
        private void NewMethod3()
        {
            for (int i = 1; i <= 8; i++)
            {
                Control[] buttons = Controls.Find("button" + i, true);
                if (buttons.Length > 0)
                {
                    Button button = (Button)buttons[0];
                    button.BackColor = Color.FromArgb(224, 224, 224);
                }
            }
        }
        private void NewMethod4(string[] i2, int a, FileVersionInfo testm, int b)
        {
            Directory.Move(@"Update\" + entpDir[b] + "\\chrome-bin" + "\\" + testm.FileVersion, instDir[b] + "\\" + testm.FileVersion);
            File.Copy(@"Update\" + entpDir[b] + "\\Chrome-bin\\Brave.exe", instDir[b] + "\\Brave.exe", true);
            File.Copy(@"Update\" + entpDir[b] + "\\Chrome-bin\\chrome_proxy.exe", instDir[b] + "\\chrome_proxy.exe", true);
            if (!Directory.Exists(instDir[b] + "\\updates"))
            {
                Directory.CreateDirectory(instDir[b] + "\\updates");
            }
            File.WriteAllText(instDir[b] + "\\updates\\Version.log", testm.FileVersion + "|" + ring2[(a - 1)] + "|" + i2[5]);
            Directory.Delete(@"Update\" + entpDir[b], true);
            if (checkBox3.Checked)
            {
                CheckButton();
            }
            else if (!checkBox3.Checked)
            {
                CheckButton2();
            }
        }
        private void NewMethod5(int c, int d)
        {
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(deskDir + "\\" + instDir[d] + ".lnk");
            link.IconLocation = applicationPath + "\\" + instDir[d] + "\\Brave.exe" + "," + icon[c];
            link.WorkingDirectory = applicationPath;
            link.TargetPath = applicationPath + "\\" + instDir[d] + " Launcher.exe";
            link.Save();
        }
        private void NewMethod6(string[] instVersion, int a, int b, int c)
        {
            Control[] buttons = Controls.Find("button" + a, true);
            Control[] buttons2 = Controls.Find("button" + b, true);
            if (instVersion[0] == buildversion[c])
            {
                if (instVersion[2] == "x86")
                {
                    if (buttons.Length > 0)
                    {
                        Button button = (Button)buttons[0];
                        button.BackColor = Color.Green;
                    }
                }
                else if (instVersion[2] == "x64")
                {
                    if (buttons2.Length > 0)
                    {
                        Button button = (Button)buttons2[0];
                        button.BackColor = Color.Green;
                    }
                }
            }
            else if (instVersion[0] != buildversion[c])
            {
                if (instVersion[2] == "x86")
                {
                    if (buttons.Length > 0)
                    {
                        Button button = (Button)buttons[0];
                        button.BackColor = Color.Red;
                    }
                }
                else if (instVersion[2] == "x64")
                {
                    if (buttons2.Length > 0)
                    {
                        Button button = (Button)buttons2[0];
                        button.BackColor = Color.Red;
                    }
                }
            }
        }
        private void NewMethod7(int a, string arch)
        {
            Control[] buttons = Controls.Find("button" + (a + 1), true);
            Button button = (Button)buttons[0];
            if (!checkBox3.Checked)
            {
                if (File.Exists(@"Brave\updates\Version.log"))
                {
                    NewMethod8(a, arch, button, File.ReadAllText(@"Brave\updates\Version.log").Split(new char[] { '|' }));
                }
                else
                {
                    toolTip.SetToolTip(button, String.Empty);
                }
            }
            if (checkBox3.Checked)
            {
                if (File.Exists(instDir[a] + "\\updates\\Version.log"))
                {
                    NewMethod8(a, arch, button, File.ReadAllText(instDir[a] + "\\updates\\Version.log").Split(new char[] { '|' }));
                }
                else
                {
                    toolTip.SetToolTip(button, String.Empty);
                }
            }
        }
        private void NewMethod8(int a, string arch, Button button, string[] instVersion)
        {
            if ((instVersion[1] == ring2[a]) && (instVersion[2] == arch))
            {
                string[] istVersion = instVersion[0].Split(new char[] { '.' });
                toolTip.SetToolTip(button, istVersion[1] + "." + istVersion[2] + "." + istVersion[3]);
                toolTip.IsBalloon = true;
            }
            else
            {
                toolTip.SetToolTip(button, String.Empty);
            }
        }
    }
}

