using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace hafiza_oyunu
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        int oyuncu1puan=0,oyuncu2puan=0;
        string yol = "resimler\\";
        Random rnd = new Random();
        ArrayList resimler = new ArrayList();
        string[] imgbox_resimler = new string[20];
        int sure = 5;
        int tiklamahakki = 2;
        ArrayList uyusma = new ArrayList();
        ArrayList dogrubilinenler = new ArrayList();




        //Resimleri random pictureboxlara ekleme
        void Resimleri_Atama()
        {
            sure = 5;
            tiklamahakki = 2;

            for (int i = 0; i < 10; i++)
            {
                resimler.Add(yol + i + ".png");
                resimler.Add(yol + i + ".png");
            }
            for (int i = 0; i < 20; i++)
            {
                int rastgele = rnd.Next(0, resimler.Count);
                PictureBox pictureBox = (PictureBox)Controls.Find("pictureBox" + i, true).First();
                pictureBox.Image = Image.FromFile(resimler[rastgele].ToString());
                pictureBox.BackColor = SystemColors.ActiveCaption;
                imgbox_resimler[i] = resimler[rastgele].ToString();
                resimler.RemoveAt(rastgele);
            }
        }


        

        private void timer1_Tick(object sender, EventArgs e)
        {
            sure--;
            if (sure==0)
            {
                Resimleri_Gizle();
                sure = 5;
                tiklamahakki = 2;
                groupBox1.Enabled = true;
            }
            if (tiklamahakki==0)
            {
                Eslesme((string)uyusma[0], (PictureBox)uyusma[1], (string)uyusma[2], (PictureBox)uyusma[3]);
                Resimleri_Gizle();
                tiklamahakki = 2;
                uyusma.Clear();
            }
            if (oyuncu1puan > 11 || oyuncu2puan > 11 || oyuncu1puan + oyuncu2puan == 20)
            {
                OyunBitti();
            }

        }

        
        private void OyunBitti()
        {
            if (oyuncu1puan>oyuncu2puan)
            {
                MessageBox.Show("1. Oyuncu kazandı.");
            }
            else if(oyuncu1puan < oyuncu2puan)
            {
                MessageBox.Show("2. Oyuncu kazandı.");
            }
            else
            {
                MessageBox.Show("Berabere bitti.");
            }
            dogrubilinenler.Clear();

            oyuncu1puan = 0;
            oyuncu2puan = 0;
            lbloyuncu1puan.Text = oyuncu1puan.ToString();
            lbloyuncu2puan.Text = oyuncu2puan.ToString();

            Resimleri_Gizle();
            groupBox1.Enabled = false;
            tiklamahakki = 2;
          
        }

        //pictureboxtaki resimleri kapatma
        private void Resimleri_Gizle()
        {
            foreach (Control item in groupBox1.Controls)
            {
                if (item is PictureBox&&!dogrubilinenler.Contains(item))
                {
                    PictureBox pictureBox = item as PictureBox;
                    pictureBox.Image = null;
                    pictureBox.BackColor = Color.DarkViolet;
                }
            }
            timer1.Stop();
         
            sure = 5;
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {
            timer1.Start();

            if (tiklamahakki != 0)
            {
                PictureBox pictureBox = (PictureBox)sender;
                tiklamahakki--;
                int i;
                if (pictureBox.Name.Length == 11)
                {
                    i = int.Parse(pictureBox.Name.Substring(10, 1));
                }
                else
                {
                    i = int.Parse(pictureBox.Name.Substring(10, 2));
                }
                pictureBox.Image = Image.FromFile(imgbox_resimler[i].Trim().ToString());
                uyusma.Add(imgbox_resimler[i].Trim().ToString());
                uyusma.Add(pictureBox);

                pictureBox.BackColor = SystemColors.ActiveCaption;
            }
       
        }

        //seçilen 2 resim uyuşuyorsa puan ekleme
        private bool Eslesme(String v1, PictureBox p1,String v2,PictureBox p2)
        {
            if (v1.Equals(v2)&&p1!=p2)
            {
                dogrubilinenler.Add(p1);
                dogrubilinenler.Add(p2);
                Oyuncu_Puan();
                return true;
            }
            else
            {
                Oyuncu_Degis();
                return false;
               
            }
        }

        //oyunculara puan ekleme
        private void Oyuncu_Puan()
        {
            if (lbloyuncu1.Visible)
            {
                oyuncu1puan+=2;
                lbloyuncu1puan.Text = oyuncu1puan.ToString();
            }
            else if (lbloyuncu2.Visible)
            {
                oyuncu2puan+=2;
                lbloyuncu2puan.Text = oyuncu2puan.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            groupBox1.Enabled = false;  
            Resimleri_Atama();
            timer1.Start();


        }

        //sıranın hangi oyuncuya geçtiğini ayarlama
        private void Oyuncu_Degis()
        {
            if (lbloyuncu1.Visible)
            {
                lbloyuncu1.Visible = false;
                lbloyuncu2.Visible = true;
            }
            else if (lbloyuncu2.Visible)
            {
                lbloyuncu1.Visible = true;
                lbloyuncu2.Visible =false ;
            }
        }
    }
}
