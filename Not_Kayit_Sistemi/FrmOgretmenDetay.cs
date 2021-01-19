using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Security.Cryptography.X509Certificates;

namespace Not_Kayit_Sistemi
{
    public partial class FrmOgretmenDetay : Form
    {
        public FrmOgretmenDetay()
        {
            InitializeComponent();
        }

        public string durum;
        public int gecenSayisi;
        public int kalanSayisi;
        public string numara;

        SqlConnection baglanti = new SqlConnection(@"Data Source=OGUZ\SQLEXPRESS;Initial Catalog=DbNotKayit;Integrated Security=True;");


        private void FrmOgretmenDetay_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dbNotKayitDataSet.TBLDERS' table. You can move, or remove it, as needed.
            this.tBLDERSTableAdapter.Fill(this.dbNotKayitDataSet.TBLDERS);

        }


        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            #region OrtalamaHesabi

            double ortalama, s1, s2, s3;
            s1 = Convert.ToDouble(tbxSinav1.Text);
            s2 = Convert.ToDouble(tbxSinav2.Text);
            s3 = Convert.ToDouble(tbxSinav3.Text);

            ortalama = (s1 + s2 + s3) / 3;
            lblOrtalama.Text = ortalama.ToString();

            numara = mskNumara.Text;


            #endregion

            #region GecmeKalmaDurumu

            
            if (ortalama >= 60)
            {
                durum = "True";
            }
            else if(ortalama < 60)
            {
                durum = "False";
            }

            #endregion

            #region GecenKalanSayisi

            if (ortalama >= 60)
            {
                gecenSayisi++;
                lblGecenSayisi.Text = gecenSayisi.ToString();
            }
            else if(ortalama < 60)
            {
                kalanSayisi++;
                lblKalanSayisi.Text = kalanSayisi.ToString();
            }


            #endregion

            #region GecenKalaniVeritabaninaYazmaKodu

            baglanti.Open();

            SqlCommand komut2 = new SqlCommand("Select * From TBLDERS where OGRNUMARA=@P1", baglanti);
            komut2.Parameters.AddWithValue("@P1", numara);
            komut2.ExecuteNonQuery();
            SqlDataReader dr = komut2.ExecuteReader();

            while (dr.Read())
            {
                 lblGecenSayisi.Text = dr[9].ToString();
                 lblKalanSayisi.Text = dr[10].ToString();
            }


            baglanti.Close();
            

            #endregion 

            Not_Guncelle();
        }



        private void Not_Guncelle()
        {
            baglanti.Open();

            SqlCommand komut =
                new SqlCommand("UPDATE TBLDERS set OGRS1=@P0,OGRS2=@P1,OGRS3=@P2,ORTALAMA=@P3,DURUM=@P4,GECENSAYISI=@P5,KALANSAYISI=@P6 WHERE OGRNUMARA=@P7",
                    baglanti);

            komut.Parameters.AddWithValue("@P0", tbxSinav1.Text);
            komut.Parameters.AddWithValue("@P1", tbxSinav2.Text);
            komut.Parameters.AddWithValue("@P2", tbxSinav3.Text);
            komut.Parameters.AddWithValue("@P3", decimal.Parse(lblOrtalama.Text));
            komut.Parameters.AddWithValue("@P4", durum);
            komut.Parameters.AddWithValue("@P7", mskNumara.Text);
            komut.Parameters.AddWithValue("@P5", gecenSayisi);
            komut.Parameters.AddWithValue("@P6", kalanSayisi);
            komut.ExecuteNonQuery();
            baglanti.Close();

            MessageBox.Show("Ogrenci Notlari Guncellendi");
            this.tBLDERSTableAdapter.Fill(this.dbNotKayitDataSet.TBLDERS);
        }

      

        private void btnKaydet_Click(object sender, EventArgs e)
        {
            baglanti.Open();

            SqlCommand komut = new SqlCommand("INSERT INTO TBLDERS (OGRNUMARA,OGRAD,OGRSOYAD) VALUES (@P1,@P2,@P3)", baglanti);

            komut.Parameters.AddWithValue("@P1", mskNumara.Text);
            komut.Parameters.AddWithValue("@P2", tbxAd.Text);
            komut.Parameters.AddWithValue("@P3", tbxSoyad.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Ogrenci sisteme eklendi.");
            this.tBLDERSTableAdapter.Fill(this.dbNotKayitDataSet.TBLDERS);
        }



        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int secilen = dataGridView1.SelectedCells[0].RowIndex;

            mskNumara.Text = dataGridView1.Rows[secilen].Cells[1].Value.ToString();
            tbxAd.Text = dataGridView1.Rows[secilen].Cells[2].Value.ToString();
            tbxSoyad.Text = dataGridView1.Rows[secilen].Cells[3].Value.ToString();
            tbxSinav1.Text = dataGridView1.Rows[secilen].Cells[4].Value.ToString();
            tbxSinav2.Text = dataGridView1.Rows[secilen].Cells[5].Value.ToString();
            tbxSinav3.Text = dataGridView1.Rows[secilen].Cells[6].Value.ToString();
            lblOrtalama.Text = dataGridView1.Rows[secilen].Cells[7].Value.ToString();
            lblGecenSayisi.Text = dataGridView1.Rows[secilen].Cells[9].Value.ToString();
            lblKalanSayisi.Text = dataGridView1.Rows[secilen].Cells[10].Value.ToString();
            // Ustteki 2 satir kodu yorumdan cikardigimda hata aliyorum ve bu hatayi cozemedim.

        } // hata aldigim kisim.

    }
}
