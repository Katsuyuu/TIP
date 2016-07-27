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

namespace TelefoniaIP.Views
{
    public partial class InComingCall : Form
    {
        public InComingCall()
        {
            InitializeComponent();

            textBox1.Text = ConnectionView.call.DialInfo.CallerID.ToString();
        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            // Odbierz przychodzące połączenie
            Program.inComingCall = false;
            ConnectionView.call.Answer();

            ConversationControllerView ccv = new ConversationControllerView("Rozmawiasz z: " + ConnectionView.call.DialInfo.CallerID.ToString());
            this.Hide();

            ccv.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Odrzuc przychodzace polaczenie
            Program.inComingCall = false;
            ConnectionView.call.Reject();
            this.Hide();
        }

        private void InComingCall_Load(object sender, EventArgs e)
        {

        }
    }
}
