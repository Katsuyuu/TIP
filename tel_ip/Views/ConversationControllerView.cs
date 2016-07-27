using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ozeki.VoIP;
using Ozeki.Media;
using Ozeki.MediaGateway;
using Ozeki.Network;
using Ozeki.Common;
using Ozeki;
using Ozeki.Vision;
using TelefoniaIP.Views;


namespace TelefoniaIP
{
    public partial class ConversationControllerView : Form
    {
   
        bool DevicesRunning = false;
        public int duration = 0;
        public int i = 0;
       // private bool inComingCall;
        public ConversationControllerView(string msg)
        {
            InitializeComponent();
            ConnectMedia();
            StartDevices();
            listBox1.Items.Add(msg);
            timer();
          
        }

        public void timer()
        {
           
            while (!ConnectionView.call.CallState.IsInCall())
            {
                timer1.Enabled = false;
            };
            timer1.Enabled = true;
            timer1.Start();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Zakończ
            if (ConnectionView.call.CallState == CallState.InCall)
            {
                ConnectionView.call.HangUp();
            }
            
            timer1.Stop();
            StopDevices();
            DisconnectMedia();
            this.Hide();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Wł. Wył urzadzenia
            if (DevicesRunning)
                StopDevices();
            else
                StartDevices();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // Wznow / Zatrzymaj połącznenie
            if (!Program.onHold)
            {
                ConnectionView.call.Hold();
                listBox1.Items.Add("Wstrzymano rozmowę.");
                Program.onHold = true;
                StopDevices();
            }
            else
            {
                ConnectionView.call.Unhold();
                listBox1.Items.Add("Wznowiono rozmowę.");
                Program.onHold = false;
                StartDevices();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void StartDevices()
        {
            if (ConnectionView.microphone != null)
            {
                ConnectionView.microphone.Start();
                ConnectionView.Print("Mikrofon włączony!");
            }

            if (ConnectionView.speaker != null)
            {
                ConnectionView.speaker.Start();
                ConnectionView.Print("Głośnik włączony!");
            }

            DevicesRunning = true;
        }


        private void StopDevices()
        {
            if (ConnectionView.microphone != null)
            {
                ConnectionView.microphone.Stop();
                ConnectionView.Print("Mikrofon wyłączony!");
            }

            if (ConnectionView.speaker != null)
            {
                ConnectionView.speaker.Stop();
                ConnectionView.Print("Głośnik wyłączony!");
            }

            DevicesRunning = false;
        }

        public void ConnectMedia()
        {
            bool checkMicrophone = false;
            bool checkSpeaker = false;

            if (ConnectionView.microphone != null)
            {
                ConnectionView.connector.Connect(ConnectionView.microphone, ConnectionView.mediaSender);
                checkMicrophone = true;
            }

            if (ConnectionView.speaker != null)
            {
                ConnectionView.connector.Connect(ConnectionView.mediaReceiver, ConnectionView.speaker);
                checkSpeaker = true;
            }

            if (checkMicrophone == true && checkSpeaker == true)
            {
                ConnectionView.Print("Podłączono media!");
            }
        }

        private void DisconnectMedia()
        {
            bool checkMicrophone = false;
            bool checkSpeaker = false;

            if (ConnectionView.microphone != null)
            {
                ConnectionView.connector.Disconnect(ConnectionView.microphone, ConnectionView.mediaSender);
                checkMicrophone = true;
            }

            if (ConnectionView.speaker != null)
            {
                ConnectionView.connector.Disconnect(ConnectionView.mediaReceiver, ConnectionView.speaker);
                checkSpeaker = true;
            }

            if (checkMicrophone == true && checkSpeaker == true)
            {
                ConnectionView.Print("Odłączono media!");
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            duration++;
            if(duration == 60)
            {
                duration = 0;
                i++;
                //textBox1.Text = i + ":" + duration;
            }
            textBox1.Text = i + ":" + duration.ToString();
            if (ConnectionView.call == null || ConnectionView.call.CallState.IsCallEnded())
            {
                    timer1.Stop();
                    StopDevices();
                    DisconnectMedia();
                    ConnectionView.call = null;
                    this.Hide();                
            }
        }
    }
}
