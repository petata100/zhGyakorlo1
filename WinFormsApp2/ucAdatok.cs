using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class ucAdatok : UserControl
    {
        Models.ReceptContext context = new();

        public ucAdatok()
        {
            InitializeComponent();
            GetFogasok();
            GetHozzavalok();
        }

        public void GetFogasok()
        {
            var fogások = from x in context.Fogasok
                          where x.FogasNev.Contains(textBox1.Text)
                          select x;

            listBox1.DisplayMember = "FogasNev";
            listBox1.DataSource = fogások.ToList();
        }

        public void GetReceptek()
        {
            Models.Fogasok selectedFogas = (Models.Fogasok)listBox1.SelectedItem;

            var receptek = from x in context.Receptek
                           where x.FogasId == selectedFogas.FogasId
                           select new Recept
                           {
                               ReceptId = x.ReceptId,
                               NyersanyagNev = x.Nyersanyag.NyersanyagNev,
                               Mennyiseg4fo = x.Mennyiseg4fo,
                               EgysegNev = x.Nyersanyag.MennyisegiEgyseg.EgysegNev,
                           };

            receptBindingSource.DataSource = receptek.ToList();
        }

        public void GetHozzavalok()
        {
            var hozzávalók = from x in context.Nyersanyagok
                             where x.NyersanyagNev.Contains(textBox2.Text)
                             select x;

            listBox2.DisplayMember = "NyersanyagNev";
            listBox2.DataSource = hozzávalók.ToList();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            GetFogasok();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            GetHozzavalok();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetReceptek();
        }

        private void bTorol_Click(object sender, EventArgs e)
        {
            int torlendoID = ((Recept)receptBindingSource.Current).ReceptId;

            var torlendoRecept = (from x in context.Receptek
                                  where x.ReceptId == torlendoID
                                  select x).FirstOrDefault();

            if (MessageBox.Show("Biztos törlöd?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                try
                {
                    context.Receptek.Remove(torlendoRecept);
                    context.SaveChanges();
                    GetReceptek();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Models.Nyersanyagok selectedNyersanyag = (Models.Nyersanyagok)listBox2.SelectedItem;
            Models.Fogasok selectedFogas = (Models.Fogasok)listBox1.SelectedItem;

            FormHozzaad form123 = new FormHozzaad();

            if (form123.ShowDialog() == DialogResult.OK)
            {
                Models.Receptek ujRecept = new Models.Receptek();
                ujRecept.NyersanyagId = selectedNyersanyag.NyersanyagId;
                ujRecept.FogasId = selectedFogas.FogasId;
                ujRecept.Mennyiseg4fo = double.Parse(form123.textBox1.Text);

                try
                {
                    context.Receptek.Add(ujRecept);
                    context.SaveChanges();
                    GetReceptek();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
