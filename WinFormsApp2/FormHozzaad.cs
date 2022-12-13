using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class FormHozzaad : Form
    {
        public FormHozzaad()
        {
            InitializeComponent();
        }

        private void bOK_Click(object sender, EventArgs e)
        {
            if (this.ValidateChildren())
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void FormHozzaad_Validating(object sender, CancelEventArgs e)
        {
        }

        private void FormHozzaad_Validated(object sender, EventArgs e)
        {
        }

        private void textBox1_Validating(object sender, CancelEventArgs e)
        {

            Regex r = new Regex("[0-9]");

            if (!r.IsMatch(textBox1.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox1, "Mennyiségnek számot kell megadni");
            }

        }

        private void textBox1_Validated(object sender, EventArgs e)
        {
            errorProvider1.SetError(textBox1, "");
        }
    }
}
