using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Heimaverkefni4
{
    public partial class Form1 : Form
    {
        
        public Form1()
        {
            InitializeComponent();
            Database.Connect();
            RefreshCustomers();
            RefreshMovies();
        }

        private void refreshcustomer_Click(object sender, EventArgs e)
        {
            RefreshCustomers();
        }

        private void RefreshMovies()
        {
            UI_allmovies.Items.Clear();
            foreach (string item in Database.AllMovies())
            {
                UI_allmovies.Items.Add(item);
            }
            UI_myndirileigu.Items.Clear();
            foreach (string item in Database.AllRented())
            {
                UI_myndirileigu.Items.Add(item);
            }
            UI_myndirekkiileigu.Items.Clear();
            foreach (string item in Database.AllNotRented())
            {
                UI_myndirekkiileigu.Items.Add(item);
            }
            UI_Myndirtiladleigaut.Items.Clear();
            foreach (string item in Database.AllNotRented())
            {
                UI_Myndirtiladleigaut.Items.Add(item);
            }
        }

        private void RefreshCustomers()
        {
            UI_allcustomers.Items.Clear();
            foreach (string item in Database.AllCustomers())
            {
                UI_allcustomers.Items.Add(item);
            }
            UI_leigavidskiptavinur.Items.Clear();
            foreach (string item in Database.AllCustomersNF())
            {
                UI_leigavidskiptavinur.Items.Add(item);
            }
        }

        private void UI_allcustomers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UI_allcustomers.SelectedIndex != - 1)
            {
                string nafn = null;
                string[] nafnid = null;
                nafn = UI_allcustomers.SelectedItem.ToString();

                char split = ';';

                nafnid = nafn.Split(split);
                UI_uppnafn.Text = nafnid[1];
                UI_uppkennitala.Text = nafnid[2];
                UI_uppsimi.Text = nafnid[3];
                UI_uppsidan.Text = nafnid[4];
                
                //Breyta
                UI_breytavidskiptaid.Text = nafnid[0];
                UI_breytavidskiptanafn.Text = nafnid[1];
                UI_breytavidskiptakennitala.Text = nafnid[2];
                UI_breytavidskiptasimi.Text = nafnid[3];

                //Eyda
                UI_eydakennitala.Text = nafnid[2];
            }
        }

        private void skravidskiptavin_Click(object sender, EventArgs e)
        {
            string kennitala = UI_skravidskiptakenni.Text;
            string nafn = UI_skravidskiptanafn.Text;
            string simi = UI_skravidskiptasimi.Text;

            Database.NewCustomer(kennitala,nafn,simi);

            RefreshCustomers();
            UI_eydakennitala.Text = "";

            UI_skravidskiptakenni.Clear();
            UI_skravidskiptanafn.Clear();
            UI_skravidskiptasimi.Clear();
        }

        private void UI_breyta_Click(object sender, EventArgs e)
        {
            string kennitala = UI_breytavidskiptakennitala.Text;
            string nafn = UI_breytavidskiptanafn.Text;
            string simi = UI_breytavidskiptasimi.Text;
            string id = UI_breytavidskiptaid.Text;

            Database.UpdateCustomer(id,kennitala,nafn,simi);
            RefreshCustomers();
        }

        private void eyda_Click(object sender, EventArgs e)
        {
            string kennitala = UI_eydakennitala.Text;

            Database.DeleteCustomer(kennitala);

            RefreshCustomers();

            UI_breytavidskiptaid.Clear();
            UI_breytavidskiptakennitala.Clear();
            UI_breytavidskiptanafn.Clear();
            UI_breytavidskiptasimi.Clear();
            UI_eydakennitala.Clear();
        }

        private void UI_leigavidskiptavinur_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] vids = UI_leigavidskiptavinur.SelectedItem.ToString().Split(';');
            UI_pid.Text = vids[0];
            UI_pkenni.Text = vids[1];
            UI_pnafn.Text = vids[2];
        }

        private void UI_Myndirtiladleigaut_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string athugasemd = UI_athugasemd.Text;
            string[] vidskiptavinur = UI_leigavidskiptavinur.SelectedItem.ToString().Split(';');
            string[] mynd = UI_Myndirtiladleigaut.SelectedItem.ToString().Split(';');
            if (Database.Rent(vidskiptavinur[0],mynd[0],DateTime.Now.ToString(),DateTime.Now.ToString(),athugasemd))
            {
                MessageBox.Show("Rented out " + mynd[1] + " to " + vidskiptavinur[2]);
            }
        }
    }
}
