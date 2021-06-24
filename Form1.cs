using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;

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
            try
            {
                for (int i = 0; i <= 3; i++)
                {
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    Uri uri = new Uri("https://updates.bravesoftware.com/service/update2");
                    ServicePointManager.FindServicePoint(uri).ConnectionLimit = 1;
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                    request.Method = "POST";
                    request.UserAgent = "Google Update/1.3.101.0;winhttp";
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
                switch (culture1.TwoLetterISOLanguageName)
                {
                    case "ru":
                        button10.Text = "Выход";
                        button9.Text = "Установить все";
                        label9.Text = "Установить все версии x86 и/или x64";
                        checkBox4.Text = "Игнорировать проверку версии";
                        checkBox3.Text = "Разные версии в отдельных папках";
                        checkBox5.Text = "Создать ярлык на рабочем столе";
                        if (IntPtr.Size != 8)
                        {
                            label9.Text = "Установить все версии x86";
                        }
                        break;
                    case "de":
                        button10.Text = "Beenden";
                        button9.Text = "Alle Installieren";
                        label9.Text = "Alle x86 und oder x64 installieren";
                        checkBox4.Text = "Versionkontrolle ignorieren";
                        checkBox3.Text = "Für jede Version einen eigenen Ordner";
                        checkBox5.Text = "Eine Verknüpfung auf dem Desktop erstellen";
                        if (IntPtr.Size != 8)
                        {
                            label9.Text = "Alle x86 installieren";
                        }
                        break;
                    default:
                        button10.Text = "Quit";
                        button9.Text = "Install all";
                        label9.Text = "Install all x86 and or x64";
                        checkBox4.Text = "Ignore version check";
                        checkBox3.Text = "Create a Folder for each version";
                        checkBox5.Text = "Create a shortcut on the desktop";
                        if (IntPtr.Size != 8)
                        {
                            label9.Text = "Install all x86";
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            foreach (Process proc in Process.GetProcesses())
            {
                if (proc.ProcessName.Equals("Brave"))
                {
                    switch (culture1.TwoLetterISOLanguageName)
                    {
                        case "ru":
                            {
                                MessageBox.Show("Необходимо закрыть Brave перед обновлением.", "Portable Brave Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        case "de":
                            {
                                MessageBox.Show("Bitte schließen Sie den laufenden  Brave-Browser, bevor Sie den Browser aktualisieren.", "Portable Brave Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                        default:
                            {
                                MessageBox.Show("Please close the running Brave browser before updating the browser.", "Portable Brave Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                return;
                            }
                    }
                }
            }
            CheckUpdate();
            CheckLauncher();
        }
        private async void Button1_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                await NewMethod(0, 0, 0, 1);
            }
            else if (!checkBox3.Checked)
            {
                await NewMethod1(0, 0, 1);
            }
        }
        private async void Button2_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                await NewMethod(1, 1, 0, 2);
            }
            if (!checkBox3.Checked)
            {
                await NewMethod1(1, 0, 2);
            }
        }
        private async void Button3_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                await NewMethod(2, 2, 0, 3);
            }
            else if (!checkBox3.Checked)
            {
                await NewMethod1(2, 0, 3);
            }
        }
        private async void Button4_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                await NewMethod(3, 3, 0, 4);
            }
            else if (!checkBox3.Checked)
            {
                await NewMethod1(3, 0, 4);
            }
        }
        private async void Button5_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                await NewMethod(0, 4, 1, 5);
            }
            else if (!checkBox3.Checked)
            {
                await NewMethod1(0, 1, 5);
            }
        }
        private async void Button6_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                await NewMethod(1, 5, 1, 6);
            }
            else if (!checkBox3.Checked)
            {
                await NewMethod1(1, 1, 6);
            }
        }
        private async void Button7_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                await NewMethod(2, 6, 1, 7);
            }
            else if (!checkBox3.Checked)
            {
                await NewMethod1(2, 1, 7);
            }
        }
        private async void Button8_Click(object sender, EventArgs e)
        {
            if (checkBox3.Checked)
            {
                await NewMethod(3, 7, 1, 8);
            }
            else if (!checkBox3.Checked)
            {
                await NewMethod1(3, 1, 8);
            }
        }
        private async void Button9_Click(object sender, EventArgs e)
        {
            await Testing();
        }
        private async Task Testing()
        {
            if ((!Directory.Exists(@"Brave Nightly x86")) && (!Directory.Exists(@"Brave Dev x86")) && (!Directory.Exists(@"Brave Beta x86")) && (!Directory.Exists(@"Brave Stable x86")))
            {
                if (checkBox1.Checked)
                {
                    await DownloadFile(0, 0, 0, 1);
                    await DownloadFile(1, 1, 0, 2);
                    await DownloadFile(2, 2, 0, 3);
                    await DownloadFile(3, 3, 0, 4);
                    checkBox1.Enabled = false;
                }
            }
            await NewMethod2(0, 0, 0, 1);
            await NewMethod2(1, 1, 0, 2);
            await NewMethod2(2, 2, 0, 3);
            await NewMethod2(3, 3, 0, 4);
            if (IntPtr.Size == 8)
            {
                if ((!Directory.Exists(@"Brave Nightly x64")) && (!Directory.Exists(@"Brave Dev x64")) && (!Directory.Exists(@"Brave Beta x64")) && (!Directory.Exists(@"Brave Stable x64")))
                {
                    if (checkBox2.Checked)
                    {
                        await DownloadFile(0, 4, 1, 5);
                        await DownloadFile(1, 5, 1, 6);
                        await DownloadFile(2, 6, 1, 7);
                        await DownloadFile(3, 7, 1, 8);
                        checkBox2.Enabled = false;
                    }
                }
                await NewMethod2(0, 4, 1, 5);
                await NewMethod2(1, 5, 1, 6);
                await NewMethod2(2, 6, 1, 7);
                await NewMethod2(3, 7, 1, 8);
            }
        }
        public async Task DownloadFile(int a, int b, int c, int d)
        {
            GroupBox progressBox = new GroupBox
            {
                Location = new Point(10, button10.Location.Y + button10.Height + 5),
                Size = new Size(groupBox1.Width, 90),
                BackColor = Color.Lavender,
            };
            Label title = new Label
            {
                AutoSize = false,
                Location = new Point(2, 10),
                Size = new Size(progressBox.Width - 4, 25),
                Text = "Brave " + ring2[a] + " " + newVersion[a] + " " + architektur2[c],
                TextAlign = ContentAlignment.BottomCenter
            };
            title.Font = new Font(title.Font.Name, 9.25F, FontStyle.Bold);
            Label downloadLabel = new Label
            {
                AutoSize = false,
                Location = new Point(5, 35),
                Size = new Size(200, 25),
                TextAlign = ContentAlignment.BottomLeft
            };
            Label percLabel = new Label
            {
                AutoSize = false,
                Location = new Point(progressBox.Size.Width - 105, 35),
                Size = new Size(100, 25),
                TextAlign = ContentAlignment.BottomRight
            };
            ProgressBar progressBarneu = new ProgressBar
            {
                Location = new Point(5, 65),
                Size = new Size(progressBox.Size.Width - 10, 7)
            };
            progressBox.Controls.Add(title);
            progressBox.Controls.Add(downloadLabel);
            progressBox.Controls.Add(percLabel);
            progressBox.Controls.Add(progressBarneu);
            Controls.Add(progressBox);
            List<Task> list = new List<Task>();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://updates.bravesoftware.com/service/update2");
                request.Method = "POST";
                request.UserAgent = "Google Update/1.3.101.0;winhttp;cup-ecdsa";
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] byteArray = Encoding.UTF8.GetBytes("<?xml version =\"1.0\" encoding=\"UTF-8\"?><request protocol=\"3.0\" version=\"1.3.99.0\" shell_version=\"1.3.99.0\" ismachine=\"1\" sessionid=\"{11111111-1111-1111-1111-111111111111}\" installsource=\"taggedmi\" testsource=\"auto\" requestid=\"{11111111-1111-1111-1111-111111111111}\" dedup=\"cr\"><os platform=\"win\" version=\"\" sp=\"\" arch=\"x86\"/><app appid=\"{" + arappid[a] + "}\" version=\"\" nextversion=\"\" ap=\"" + arapVersion[d - 1] + "\" lang=\"en\" brand=\"\" client=\"\" installage=\"-1\" installdate=\"-1\"><updatecheck/></app></request>");
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
                        webClient.DownloadProgressChanged += (o, args) =>
                        {
                            Control[] buttons = Controls.Find("button" + d, true);
                            if (buttons.Length > 0)
                            {
                                Button button = (Button)buttons[0];
                                button.BackColor = Color.Orange;
                            }
                            progressBarneu.Value = args.ProgressPercentage;
                            downloadLabel.Text = $"{args.BytesReceived / 1024d / 1024d:0.00} MB's / {args.TotalBytesToReceive / 1024d / 1024d:0.00} MB's";
                            percLabel.Text = $"{args.ProgressPercentage}%";
                        };
                        webClient.DownloadFileCompleted += (o, args) =>
                        {
                            if (args.Cancelled == true)
                            {
                                MessageBox.Show("Download has been canceled.");
                            }
                            else
                            {
                                switch (culture1.TwoLetterISOLanguageName)
                                {
                                    case "ru":
                                        downloadLabel.Text = "Распаковка";
                                        break;
                                    case "de":
                                        downloadLabel.Text = "Entpacken";
                                        break;
                                    default:
                                        downloadLabel.Text = "Unpacking";
                                        break;
                                }
                                string arguments = " x " + "Brave_" + architektur[c] + "_" + buildversion[a] + "_" + ring[a] + ".exe" + " -o" + @"Update\" + entpDir[b] + " -y";
                                Process process = new Process();
                                process.StartInfo.FileName = @"Bin\7zr.exe";
                                process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                                process.StartInfo.Arguments = arguments;
                                process.Start();
                                process.WaitForExit();
                                process.StartInfo.Arguments = " x " + @"Update\" + entpDir[b] + "\\Chrome.7z -o" + @"Update\" + entpDir[b] + " -y";
                                process.Start();
                                process.WaitForExit();
                                if ((File.Exists(@"Update\" + entpDir[b] + "\\chrome-bin\\Brave.exe")) && (File.Exists(instDir[b] + "\\updates\\Version.log")))
                                {
                                    string[] instVersion = File.ReadAllText(instDir[b] + "\\updates\\Version.log").Split(new char[] { '|' });
                                    FileVersionInfo testm = FileVersionInfo.GetVersionInfo(applicationPath + "\\Update\\" + entpDir[b] + "\\chrome-bin\\Brave.exe");
                                    if (checkBox3.Checked)
                                    {
                                        if (testm.FileVersion != instVersion[0])
                                        {
                                            if (Directory.Exists(instDir[b] + "\\" + instVersion[0]))
                                            {
                                                Directory.Delete(instDir[b] + "\\" + instVersion[0], true);
                                            }
                                            Thread.Sleep(2000);
                                            NewMethod4(a, b, c, testm);
                                        }
                                        else if ((testm.FileVersion == instVersion[0]) && (checkBox4.Checked))
                                        {
                                            if (Directory.Exists(instDir[b] + "\\" + instVersion[0]))
                                            {
                                                Directory.Delete(instDir[b] + "\\" + instVersion[0], true);
                                            }
                                            Thread.Sleep(2000);
                                            NewMethod4(a, b, c, testm);
                                        }
                                    }
                                    else if (!checkBox3.Checked)
                                    {
                                        if (Directory.Exists(instDir[b] + "\\" + instVersion[0]))
                                        {
                                            Directory.Delete(instDir[b] + "\\" + instVersion[0], true);
                                        }
                                        Thread.Sleep(2000);
                                        NewMethod4(a, b, c, testm);
                                    }
                                }
                                else
                                {
                                    if (!Directory.Exists(instDir[b]))
                                    {
                                        Directory.CreateDirectory(instDir[b]);
                                    }
                                    NewMethod4(a, b, c, FileVersionInfo.GetVersionInfo(applicationPath + "\\Update\\" + entpDir[b] + "\\chrome-bin\\Brave.exe"));
                                }
                            }
                            if (checkBox5.Checked)
                            {
                                if (!File.Exists(deskDir + "\\" + instDir[b] + ".lnk"))
                                {
                                    NewMethod5(a, b);
                                }
                            }
                            else if (File.Exists(deskDir + "\\" + instDir[b] + ".lnk") && (instDir[b] == "Brave"))
                            {
                                NewMethod5(a, b);
                            }
                            if (!File.Exists(@instDir[b] + " Launcher.exe"))
                            {
                                File.Copy(@"Bin\Launcher\" + instDir[b] + " Launcher.exe", @instDir[b] + " Launcher.exe");
                            }
                            File.Delete("Brave_" + architektur[c] + "_" + buildversion[a] + "_" + ring[a] + ".exe");
                            switch (culture1.TwoLetterISOLanguageName)
                            {
                                case "ru":
                                    downloadLabel.Text = "Распакованный";
                                    break;
                                case "de":
                                    downloadLabel.Text = "Entpackt";
                                    break;
                                default:
                                    downloadLabel.Text = "Unpacked";
                                    break;
                            }
                        };
                        try
                        {
                            var task = webClient.DownloadFileTaskAsync(uri, "Brave_" + architektur[c] + "_" + buildversion[a] + "_" + ring[a] + ".exe");
                            list.Add(task);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            await Task.WhenAll(list);
            await Task.Delay(2000);
            Controls.Remove(progressBox);
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
                        switch (culture1.TwoLetterISOLanguageName)
                        {
                            case "ru":
                                button9.Text = "Обновить все";
                                break;
                            case "de":
                                button9.Text = "Alle Updaten";
                                break;
                            default:
                                button9.Text = "Update all";
                                break;
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
            switch (culture1.TwoLetterISOLanguageName)
            {
                case "ru":
                    MessageBox.Show("Данная версия уже установлена", "Portabel Brave Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                case "de":
                    MessageBox.Show("Die selbe Version ist bereits installiert", "Portabel Brave Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                default:
                    MessageBox.Show("The same version is already installed", "Portabel Brave Updater", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
        private async Task NewMethod(int a, int b, int c, int d)
        {
            if (File.Exists(@instDir[b] + "\\updates\\Version.log"))
            {
                if (File.ReadAllText(instDir[b] + "\\updates\\Version.log").Split(new char[] { '|' })[0] == buildversion[a])
                {
                    if (checkBox4.Checked)
                    {
                        await DownloadFile(a, b, c, d);
                    }
                    else
                    {
                        Message1();
                    }
                }
                else
                {
                    await DownloadFile(a, b, c, d);
                }
            }
            else
            {
                await DownloadFile(a, b, c, d);
            }
        }
        private async Task NewMethod1(int a, int b, int c)
        {
            if (File.Exists(@"Brave\updates\Version.log"))
            {
                string[] instVersion = File.ReadAllText(@"Brave\updates\Version.log").Split(new char[] { '|' });
                if ((instVersion[0] == buildversion[a]) && (instVersion[1] == ring2[a]) && (instVersion[2] == architektur2[b]))
                {
                    if (checkBox4.Checked)
                    {
                        await DownloadFile(a, 8, b, c);
                    }
                    else
                    {
                        Message1();
                    }
                }
                else
                {
                    await DownloadFile(a, 8, b, c);
                }
            }
            else
            {
                await DownloadFile(a, 8, b, c);
            }
        }
        private async Task NewMethod2(int a, int b, int c, int d)
        {
            if (Directory.Exists(instDir[b]))
            {
                if (File.Exists(instDir[b] + "\\updates\\Version.log"))
                {
                    if (File.ReadAllText(instDir[b] + "\\updates\\Version.log").Split(new char[] { '|' })[0] != buildversion[a])
                    {
                        await DownloadFile(a, b, c, d);
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
        private void NewMethod4(int a, int b, int c, FileVersionInfo testm)
        {
            Directory.Move(@"Update\" + entpDir[b] + "\\chrome-bin" + "\\" + testm.FileVersion, instDir[b] + "\\" + testm.FileVersion);
            File.Copy(@"Update\" + entpDir[b] + "\\Chrome-bin\\Brave.exe", instDir[b] + "\\Brave.exe", true);
            File.Copy(@"Update\" + entpDir[b] + "\\Chrome-bin\\chrome_proxy.exe", instDir[b] + "\\chrome_proxy.exe", true);
            if (!Directory.Exists(instDir[b] + "\\updates"))
            {
                Directory.CreateDirectory(instDir[b] + "\\updates");
            }
            File.WriteAllText(instDir[b] + "\\updates\\Version.log", testm.FileVersion + "|" + ring2[a] + "|" + architektur2[c]);
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
        private void NewMethod5(int a, int b)
        {
            IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
            IWshRuntimeLibrary.IWshShortcut link = (IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(deskDir + "\\" + instDir[b] + ".lnk");
            link.IconLocation = applicationPath + "\\" + instDir[b] + "\\Brave.exe" + "," + icon[a];
            link.WorkingDirectory = applicationPath;
            link.TargetPath = applicationPath + "\\" + instDir[b] + " Launcher.exe";
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
        private void CheckUpdate()
        {
            GroupBox groupBoxupdate = new GroupBox
            {
                Location = new Point(groupBox1.Location.X, button10.Location.Y + button10.Size.Height + 5),
                Size = new Size(groupBox1.Width, 90),
                BackColor = Color.Aqua
            };
            Label versionLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.BottomCenter,
                Dock = DockStyle.None,
                Location = new Point(2, 30),
                Size = new Size(groupBoxupdate.Width - 4, 25),
            };
            versionLabel.Font = new Font(versionLabel.Font.Name, 10F, FontStyle.Bold);
            Label infoLabel = new Label
            {
                AutoSize = false,
                TextAlign = ContentAlignment.BottomCenter,
                Dock = DockStyle.None,
                Location = new Point(2, 10),
                Size = new Size(groupBoxupdate.Width - 4, 20),
            };
            infoLabel.Font = new Font(infoLabel.Font.Name, 8.75F);
            Label downLabel = new Label
            {
                TextAlign = ContentAlignment.MiddleRight,
                AutoSize = false,
                Size = new Size(100, 23),
            };
            Button laterButton = new Button
            {
                Size = new Size(40, 23),
                BackColor = Color.FromArgb(224, 224, 224)
            };
            Button updateButton = new Button
            {
                Location = new Point(groupBoxupdate.Width - Width - 10, 60),
                Size = new Size(40, 23),
                BackColor = Color.FromArgb(224, 224, 224)
            };
            updateButton.Location = new Point(groupBoxupdate.Width - updateButton.Width - 10, 60);
            laterButton.Location = new Point(updateButton.Location.X - laterButton.Width - 5, 60);
            downLabel.Location = new Point(laterButton.Location.X - downLabel.Width - 20, 60);
            groupBoxupdate.Controls.Add(updateButton);
            groupBoxupdate.Controls.Add(laterButton);
            groupBoxupdate.Controls.Add(downLabel);
            groupBoxupdate.Controls.Add(infoLabel);
            groupBoxupdate.Controls.Add(versionLabel);
            updateButton.Click += new EventHandler(UpdateButton_Click);
            laterButton.Click += new EventHandler(LaterButton_Click);
            switch (culture1.TwoLetterISOLanguageName)
            {
                case "ru":
                    infoLabel.Text = "Доступна новая версия";
                    laterButton.Text = "нет";
                    updateButton.Text = "Да";
                    downLabel.Text = "ОБНОВИТЬ";
                    break;
                case "de":
                    infoLabel.Text = "Eine neue Version ist verfügbar";
                    laterButton.Text = "Nein";
                    updateButton.Text = "Ja";
                    downLabel.Text = "Jetzt Updaten";
                    break;
                default:
                    infoLabel.Text = "A new version is available";
                    laterButton.Text = "No";
                    updateButton.Text = "Yes";
                    downLabel.Text = "Update now";
                    break;
            }
            void LaterButton_Click(object sender, EventArgs e)
            {
                groupBoxupdate.Dispose();
                Controls.Remove(groupBoxupdate);
                groupBox1.Enabled = true;
            }
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                var request = (WebRequest)HttpWebRequest.Create("https://github.com/UndertakerBen/PorBraveUpd/raw/master/Version.txt");
                var response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var version = reader.ReadToEnd();
                    FileVersionInfo testm = FileVersionInfo.GetVersionInfo(applicationPath + "\\Portable Brave Updater.exe");
                    versionLabel.Text = testm.FileVersion + "  >>> "+ version;
                    if (Convert.ToInt32(version.Replace(".", "")) > Convert.ToInt32(testm.FileVersion.Replace(".", "")))
                    {
                        Controls.Add(groupBoxupdate);
                        groupBox1.Enabled = false;
                    }
                    reader.Close();
                }
            }
            catch (Exception)
            {

            }
            void UpdateButton_Click(object sender, EventArgs e)
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var request2 = (WebRequest)HttpWebRequest.Create("https://github.com/UndertakerBen/PorBraveUpd/raw/master/Version.txt");
                var response2 = request2.GetResponse();
                using (StreamReader reader = new StreamReader(response2.GetResponseStream()))
                {
                    var version = reader.ReadToEnd();
                    reader.Close();
                    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                    using (WebClient myWebClient2 = new WebClient())
                    {
                        myWebClient2.DownloadFile($"https://github.com/UndertakerBen/PorBraveUpd/releases/download/v{version}/Portable.Brave.Updater.v{version}.7z", @"Portable.Brave.Updater.v" + version + ".7z");
                    }
                    File.AppendAllText(@"Update.cmd", "@echo off" + "\n" +
                        "timeout /t 1 /nobreak" + "\n" +
                        "\"" + applicationPath + "\\Bin\\7zr.exe\" e \"" + applicationPath + "\\Portable.Brave.Updater.v" + version + ".7z\" -o\"" + applicationPath + "\" \"Portable Brave Updater.exe\"" + " -y\n" +
                        "call cmd /c Start /b \"\" " + "\"" + applicationPath + "\\Portable Brave Updater.exe\"\n" +
                        "del /f /q \"" + applicationPath + "\\Portable.Brave.Updater.v" + version + ".7z\"\n" +
                        "del /f /q \"" + applicationPath + "\\Update.cmd\" && exit\n" +
                        "exit\n");

                    string arguments = " /c call Update.cmd";
                    Process process = new Process();
                    process.StartInfo.FileName = "cmd.exe";
                    process.StartInfo.Arguments = arguments;
                    process.Start();
                    Close();
                }
            }
        }
        private void CheckLauncher()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                var request = (WebRequest)HttpWebRequest.Create("https://github.com/UndertakerBen/PorBraveUpd/raw/master/Launcher/Version.txt");
                var response = request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var version = reader.ReadToEnd();
                    FileVersionInfo testm = FileVersionInfo.GetVersionInfo(applicationPath + "\\Bin\\Launcher\\Brave Launcher.exe");
                    if (Convert.ToInt32(version.Replace(".", "")) > Convert.ToInt32(testm.FileVersion.Replace(".", "")))
                    {
                        reader.Close();
                        try
                        {
                            using (WebClient myWebClient2 = new WebClient())
                            {
                                myWebClient2.DownloadFile("https://github.com/UndertakerBen/PorBraveUpd/raw/master/Launcher/Launcher.7z", @"Launcher.7z");
                            }
                            string arguments = " x " + @"Launcher.7z" + " -o" + @"Bin\\Launcher" + " -y";
                            Process process = new Process();
                            process.StartInfo.FileName = @"Bin\7zr.exe";
                            process.StartInfo.WindowStyle = ProcessWindowStyle.Minimized;
                            process.StartInfo.Arguments = arguments;
                            process.Start();
                            process.WaitForExit();
                            File.Delete(@"Launcher.7z");
                            foreach (string launcher in instDir)
                            {
                                if (File.Exists(launcher + " Launcher.exe"))
                                {
                                    FileVersionInfo binLauncher = FileVersionInfo.GetVersionInfo(applicationPath + "\\Bin\\Launcher\\" + launcher + " Launcher.exe");
                                    FileVersionInfo istLauncher = FileVersionInfo.GetVersionInfo(applicationPath + "\\" + launcher + " Launcher.exe");
                                    if (Convert.ToDecimal(binLauncher.FileVersion) > Convert.ToDecimal(istLauncher.FileVersion))
                                    {
                                        File.Copy(@"bin\\Launcher\\" + launcher + " Launcher.exe", launcher + " Launcher.exe", true);
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }
    }
}

