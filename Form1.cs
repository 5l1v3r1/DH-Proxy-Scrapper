using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;

namespace proxy_scraper
{
    public partial class Form1 : Form
    {
        public Point mouseLocation;

        public Form1()
        {
            InitializeComponent();
        }  
        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtfile.Text = ofd.FileName;
            } 
        }

        private void Btnget_Click(object sender, EventArgs e)
        {           
                if(txtfile != null && !string.IsNullOrWhiteSpace(txtfile.Text))
                {
                    Btnget.Enabled = false;
                    using (StreamReader sr = new StreamReader(txtfile.Text))
                    {
                        string line;
                        while ((line = sr.ReadLine()) != null)
                        {
                            GetPoxy(line);                              
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Browse Your Url File!");
                }           
            Btnget.Enabled = true;
        }

        private void GetPoxy(string url)
        {
            WebClient wc = new WebClient();
            try
            {
                string data = wc.DownloadString(url);
                string pattren = @"(([01]?\d\d?|2[0-4]\d|25[0-5])\.){3}([01]?\d\d?|25[0-5]|2[0-4]\d):0*(?:6553[0-5]|655[0-2][0-9]|65[0-4][0-9]{2}|6[0-4][0-9]{3}|[1-5][0-9]{4}|[1-9][0-9]{1,3}|[0-9])";
                MatchCollection matches = Regex.Matches(data, pattren);

                if (matches.Count > 0)
                {
                    foreach (Match match in matches)
                    {
                        listBox1.Items.Add(match);
                        grpproxy.Text = "Found: " + listBox1.Items.Count.ToString();
                    }
                }
            }
            catch (Exception)
            {
                //
            }
            wc.Dispose();
        }

        private void Btnsave_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count > 0)
            {
                SaveFileDialog fs = new SaveFileDialog();
                fs.RestoreDirectory = true;
                fs.Filter = "txt files (*.txt)|*.txt";
                fs.FilterIndex = 1;
                fs.ShowDialog();

                if (!(fs.FileName == null))
                {
                    using (StreamWriter sw = new StreamWriter(fs.FileName))
                    {
                        foreach (System.Text.RegularExpressions.Match line in listBox1.Items)
                        {
                            sw.WriteLine(line);
                        }
                    }
                    MessageBox.Show(listBox1.Items.Count.ToString() + " Proxies Succesfully Saved");
                }
            }
            else
            {
                MessageBox.Show("You Have 0 Proxies Please Click On Get");
            }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            mouseLocation = new Point(-e.X, -e.Y);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point mousePose = Control.MousePosition;
                mousePose.Offset(mouseLocation.X, mouseLocation.Y);
                Location = mousePose;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
        }

        private void label2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            mBox1.ShowMessage("Content", "Description");
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
