using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TopluMailGonderme
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void listBoxEmails_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        //Txt dosyasındaki mailleri çekme
        private void buttonTxt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            openFile.Multiselect = false;

            DialogResult dr = openFile.ShowDialog();
            if(dr == DialogResult.OK)
            {
                System.IO.StreamReader file=new System.IO.StreamReader(@openFile.FileName);
                string line;
                while((line = file.ReadLine()) != null)
                {
                    listBoxEmails.Items.Add(line);

                }
                file.Close();
            }

        }

        private void pictureBoxLoading_Click(object sender, EventArgs e)
        {

        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            buttonSend.Enabled = false;
            pictureBoxLoading.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            CheckForIllegalCrossThreadCalls = false;
            SmtpClient client = new SmtpClient();
            client.Timeout = 30000;
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            BodyBuilder builder = new BodyBuilder();
            builder.HtmlBody = textBoxHTMLBody.Text;

            MimeMessage mail = new MimeMessage();
            mail.Subject = textBoxSubject.Text;
            mail.Body = builder.ToMessageBody();

            client.Connect(textBoxSMTP.Text, int.Parse(textBoxPORT.Text), checkBoxSSL.Checked);
            client.Authenticate(textBoxUSER.Text, textBoxPASSWORD.Text);

            foreach (string email in listBoxEmails.Items)
            {
                mail.From.Add(new MailboxAddress("Toplu Mail Otomasyonu", textBoxUSER.Text));
                mail.Bcc.Add(new MailboxAddress(null,email));
                client.Send(mail);
                Thread.Sleep(1000);

            }
            client.Disconnect(true);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonSend.Enabled = true;
            pictureBoxLoading.Visible = true;
            MessageBox.Show("Done");

        }
    }
}
