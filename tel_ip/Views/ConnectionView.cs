using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Diagnostics;

namespace TelefoniaIP.Views
{
    
    public partial class ConnectionView : Form
    {
        public static ISoftPhone softPhone;
        public static IPhoneLine phoneLine;
        public static RegState phoneLineInformation;
        public static IPhoneCall call;
        public static Microphone microphone = Microphone.GetDefaultDevice();
        public static Speaker speaker = Speaker.GetDefaultDevice();
        public static MediaConnector connector = new MediaConnector();
        public static PhoneCallAudioSender mediaSender = new PhoneCallAudioSender();
        public static PhoneCallAudioReceiver mediaReceiver = new PhoneCallAudioReceiver();
        public bool DevicesRunning = true;
        public static string username;
        public const string _domainHostIP = "192.168.1.109";

        public ConnectionView(string user)
        {
            username = user;

            InitializeComponent();
            InitializeSoftPhone();
            InitTimer();
        }

        public void InitializeSoftPhone()
        {
            try
            {
                softPhone = SoftPhoneFactory.CreateSoftPhone(SoftPhoneFactory.GetLocalIP(), 5700, 5750);
                softPhone.IncomingCall += new EventHandler<VoIPEventArgs<IPhoneCall>>(softPhone_inComingCall);

                SIPAccount sa = new SIPAccount(true, username, username, username, "kappa123", _domainHostIP, 5060);
                
                phoneLine = softPhone.CreatePhoneLine(sa);
                phoneLine.RegistrationStateChanged += phoneLine_PhoneLineInformation;

                softPhone.RegisterPhoneLine(phoneLine);
                
                Program.onHold = false;
                listBox1.Items.Add("Softphone utworzony!");
            }
            catch (Exception ex)
            {
                listBox1.Items.Add("Error: " + ex);
            }
        }


        public void InvokeGUIThread(Action action)
        {
            try
            {
                Invoke(action);
            }
            catch(Exception)
            {
                this.Close();
            }
        }


        public void phoneLine_PhoneLineInformation(object sender, RegistrationStateChangedArgs e)
        {
            phoneLineInformation = e.State;
           
            if (phoneLineInformation != RegState.RegistrationRequested)
            {
                InvokeGUIThread(() =>
                {
                    if (phoneLineInformation == RegState.RegistrationSucceeded)
                    {
                        listBox1.Items.Add("Zarejstrowano pomyślnie!");

                    }
                    else if (phoneLineInformation == RegState.UnregRequested)
                    {
                        this.Close();
                    }
                    else
                    {
                        listBox1.Items.Add("Usp, coś poszło nie tak, spróbuj uruchomić aplikację ponownie.");
                    }

                });
            }
        }

        public void softPhone_inComingCall(object sender, VoIPEventArgs<IPhoneCall> e)
        {
            call = e.Item;
            WireUpCallEvents();
            Program.inComingCall = true;
            if (Program.inComingCall == true)
             {
                InComingCall icc = new InComingCall();
                InvokeGUIThread(() => { icc.Show(); }); 
             }
        }


        public void call_CallStateChanged(object sender, CallStateChangedArgs e)
        {
            InvokeGUIThread(() => { listBox1.Items.Add("Zmiana stanu połączenia." + e.State.ToString()); });

            if (e.State == CallState.Answered)
            {
                mediaReceiver.AttachToCall(call);
                mediaSender.AttachToCall(call);

                InvokeGUIThread(() => { listBox1.Items.Add("Połączenie rozpoczęte."); });
               
            }
            else if (e.State == CallState.InCall)
            {
               
                InvokeGUIThread(() => { this.button11.Enabled = false; }); 
            }
            else if (e.State.IsCallEnded() == true)
            {
                mediaReceiver.Detach();
                mediaSender.Detach();

                WireDownCallEvents();
                call = null;
                InvokeGUIThread(() => { this.button11.Enabled = true; });

                InvokeGUIThread(() => { listBox1.Items.Add("Połączenie zakończone."); }); 
            }
        }

        public void WireUpCallEvents()
        {
            call.CallStateChanged += (call_CallStateChanged);

        }

        public void WireDownCallEvents()
        {
            call.CallStateChanged -= (call_CallStateChanged);
        }

    

        private void button1_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "1";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "2";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "3";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "4";
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "5";
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "6";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "7";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "8";
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "9";
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.textBox1.Text += "0";
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (textBox1.Text.Length > 0)
                textBox1.Text = textBox1.Text.Remove(textBox1.Text.Length - 1);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }

        private void button11_Click(object sender, EventArgs e)
        {
            // Zadzwoń
            if (this.textBox1.Text != "")
            {
                call = softPhone.CreateCallObject(phoneLine, textBox1.Text);
                WireUpCallEvents();
                call.Start();

                ConversationControllerView ccv = new ConversationControllerView("Rozmawiasz z: " + call.DialInfo.Dialed.ToString());

                ccv.Show();
                ccv.Focus();
            }
            else
            {
                MessageBox.Show("Najpierw musisz wybrać numer!");
            }
        }

        private void listBox2_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // Wybranie osoby do ktorej chcemy zadzownić
            int itemIndex = listBox2.IndexFromPoint(e.Location);

            if (itemIndex != System.Windows.Forms.ListBox.NoMatches)
            {
                string text = listBox2.Items[itemIndex].ToString();
                int spaceIndex = text.IndexOf(" ");

                if (spaceIndex > 0)
                    text = text.Substring(0, spaceIndex);

                textBox1.Text = text;
            }
        }

        public static void Print(string msg)
        {
            listBox1.Items.Add(msg);
        }


        public static void PrintContacts(Dictionary<string, string> list)
        {
            listBox2.Items.Clear();
            foreach (var item in list)
            {
                if(item.Key != username)
                    listBox2.Items.Add(item.Key + " (" + item.Value + ")");
            }
        }

        public void GetUsers()
        {
            
            ContactsHandler.StartClient();
        }

        private Timer _GetUsersTimer;
        public void InitTimer()
        {
            _GetUsersTimer = new Timer();
            _GetUsersTimer.Tick += new EventHandler(timer1_Tick);
            _GetUsersTimer.Interval = 10000;
            _GetUsersTimer.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            GetUsers();
        }

        private void ConnectionView_Closing(object sender, CancelEventArgs e)
        {
            _GetUsersTimer.Stop();            
            softPhone.UnregisterPhoneLine(phoneLine);
        }
    }
}
