using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Leaf.xNet;
using System.Text.RegularExpressions;
using System.Collections;
using System.Drawing;
using System.Diagnostics;
using System.Net.Sockets;
using Bunifu.UI.WinForms;

namespace FiveM_Build_Changer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private static readonly HttpClient client = new HttpClient();

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> result =
            new Dictionary<string, string>();
            if (Properties.Settings.Default.serverfolder == false)
            {

                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Choose your server folder.";
                fbd.ShowDialog();
                var foldername = fbd.SelectedPath;
                Properties.Settings.Default.hmhm = foldername;
                bool existscheck = Directory.Exists(foldername + @"\cfx-server-data-master");
                bool existscheck2 = File.Exists(foldername + @"\cfx-server-data-master\server.cfg");
                bool existscheck3 = Directory.Exists(foldername + @"\citizen");

                if (existscheck)
                {
                    if (existscheck2)
                    {
                        if (existscheck3)
                        {


                            Properties.Settings.Default.serverfolder = true;
                            bunifuTextBox1.Visible = true;
                            MessageBox.Show("Server found.");
                            MessageBox.Show("Now type a build version into the text box above.");
                            guna2GradientButton1.Text = "Change the build.";
                        }
                    }

                }

            }
            else
            {


                {
                    using (HttpRequest req = new HttpRequest())
                    {
                        string reqResponse = req.Get("https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/").ToString();
                        var Matches = Regex.Matches(reqResponse, @"..[0-9][0-9][0-9][0-9]-(.*)server.(zip|7z)");
                        foreach (var match in Matches)
                        {
                            try
                            {
                                result.Add(match.ToString().Substring(2, 4), match.ToString().Replace("./", "https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/"));
                            }
                            catch { }
                        }

                        string BuildNumber = bunifuTextBox1.Text;
                        string Link = String.Empty;


                        {
                            if (!result.TryGetValue(BuildNumber, out Link)) MessageBox.Show("An error occurred.");

                            bool fbcexists = Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FBC");
                            if (!fbcexists)
                            {
                                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FBC");
                            }
                            else
                            {
                                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FBC");
                                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FBC");
                            }
                            new WebClient().DownloadFile(Link, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FBC\\server.7z");

                            SevenZipNET.SevenZipExtractor zz = new SevenZipNET.SevenZipExtractor(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FBC\\server.7z");
                            zz.ExtractAll(Properties.Settings.Default.hmhm);
                            MessageBox.Show($"Succesfully downloaded / extracted build: {BuildNumber}\n\nSource: {Link}");
                        }



                    }
                }
            }
        }

        private void guna2TextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2TileButton1_MouseHover(object sender, EventArgs e)
        {
        }

        private void guna2TileButton1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2TileButton2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.serverfolder == true)
            {
                guna2GradientButton1.Text = "Change the build.";
            } else
            {
                bunifuTextBox1.Visible = false;
                guna2GradientButton1.Text = "Choose your server folder.";
            }
        }
    }
}
