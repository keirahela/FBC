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
            // Declaring result 
            Dictionary<string, string> result =
            new Dictionary<string, string>();
            if (Properties.Settings.Default.serverfolder == false)
            {
                // Open folderbrowserdialog, let user choose server folder
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = "Choose your server folder.";
                fbd.ShowDialog();
                var foldername = fbd.SelectedPath;
                bool existscheck = Directory.Exists(foldername + @"\cfx-server-data-master");
                bool existscheck2 = File.Exists(foldername + @"\cfx-server-data-master\server.cfg");
                bool existscheck3 = Directory.Exists(foldername + @"\citizen");

                // Check if the folder has a cfx-server-data-master subdir
                if (existscheck)
                {
                    // Check if the cfx-server-data-master dir has a file called server.cfg
                    if (existscheck2)
                    {
                        // Check if the folder has a citizen subdir (like in existscheck)
                        if (existscheck3)
                        {
                            // Save the selectedpath into Properties.Settings.Default.serverloc
                            Properties.Settings.Default.serverfolder = true;
                            Properties.Settings.Default.serverloc = foldername;
                            // Make the text box visible
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
                    // Use HttpRequest
                    using (HttpRequest req = new HttpRequest())
                    {
                        // Get the response from build server website using HttpRequest
                        string reqResponse = req.Get("https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/").ToString();
                        // Regex to get the versions
                        var Matches = Regex.Matches(reqResponse, @"..[0-9][0-9][0-9][0-9]-(.*)server.(zip|7z)");
                        // foreach
                        foreach (var match in Matches)
                        {
                            // Try
                            try
                            {
                                // use result, that we declared earlier to match tostring, then replace ./ with https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/, so it can be downloaded
                                result.Add(match.ToString().Substring(2, 4), match.ToString().Replace("./", "https://runtime.fivem.net/artifacts/fivem/build_server_windows/master/"));
                            }
                            // Catch
                            catch { }
                        }
                        // get user input version
                        string BuildNumber = bunifuTextBox1.Text;
                        // link = string.empty :bonk:
                        string Link = String.Empty;
                        {
                            if (!result.TryGetValue(BuildNumber, out Link)) MessageBox.Show("An error occurred.");
                            // another check??
                            bool fbcexists = Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FBC");
                            // if it doesn't exist, we must create it
                            if (!fbcexists)
                            {
                                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FBC");
                            }
                            // if it exists, delete it, then create it
                            else
                            {
                                Directory.Delete(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FBC");
                                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FBC");
                            }
                            // download the .7z file into documents\FBC\
                            new WebClient().DownloadFile(Link, Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FBC\\server.7z");
                            // Use SevenZipNET extractor to extract the file that we just downloaded
                            SevenZipNET.SevenZipExtractor zz = new SevenZipNET.SevenZipExtractor(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FBC\\server.7z");
                            // Extract it into the server location.
                            zz.ExtractAll(Properties.Settings.Default.serverloc);
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
