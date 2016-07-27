using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TelefoniaIP.Views
{
    public partial class Registration : Form
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Zaloguj
            ConnectionView cv = null;

            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Musisz podać nazwę oraz numer!");
                return;
            }

            try
            {
                cv = new ConnectionView(textBox1.Text);
                this.Hide();
                cv.Show();
            }
            catch (Exception)
            {
                MessageBox.Show("Użytkownik o takiej nazwie jest już zarejestrowany, proszę o podanie innej nazwy użytkownika!");
                return;
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsNumber(e.KeyChar))
            {
                e.Handled = e.KeyChar != (char)Keys.Back;
            }
        }
    }
}
