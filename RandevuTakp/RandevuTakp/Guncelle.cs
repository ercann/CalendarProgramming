using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RandevuTakp
{
    public partial class Guncelle : Form
    {
        SqlConnection con = new SqlConnection();

        public Guncelle()
        {
            InitializeComponent();
        }


        private void Guncelle_Load(object sender, EventArgs e)
        {

            open_Connection();
            CbBox();



            //Güncelleme penceresine seçilen kayıdın getirilmesi !

            SqlCommand tBoxQuery = new SqlCommand("select * from PatientsInfo where ID = @ID", con);
            tBoxQuery.Parameters.AddWithValue("@ID", veriler.CurrentRow.Cells[0].Value);
            SqlDataReader tbox = tBoxQuery.ExecuteReader();
            tbox.Read();
            tb_UpdateName.Text = tbox["HastaAdi"].ToString();
            tb_UpdateSurname.Text = tbox["HastaSoyadi"].ToString();
            tb_UpdateComplaint.Text = tbox["HastaSikayeti"].ToString();
            tb_UpdateDrName.Text = tbox["DoktorAdi"].ToString();
            mtb_UpdatePhone.Text = tbox["TelefonNo"].ToString();
            cb_UpdateHour.Text = veriler.CurrentRow.Cells[1].Value.ToString();
            dtp_UpdateDate.Text = veriler.CurrentRow.Cells[5].Value.ToString();
            tbox.Close();
        }

        public void open_Connection()
        {
            try
            {
                con.ConnectionString = "Data Source = ERCAN ; database = appointment ; integrated security = true";
                con.Open();
            }
            catch
            {
                MessageBox.Show("Veritabanına Bağlanılamadı !");
            }
        }

        //ComboBox ın içinin doldurulması
        public void CbBox()
        {
            SqlCommand cmd = new SqlCommand("select CONVERT(VARCHAR(10),HourItems,108) AS CbHour from ComboBox  ", con);
            cb_UpdateHour.DataSource = null;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cb_UpdateHour.Items.Add(dr["CbHour"]);
            }
            dr.Close();
        }

        //Update Butonu

        public void btn_UpdateAdd_Click(object sender, EventArgs e)
        {
                try
                {
                    SqlCommand updateQuary = new SqlCommand("Update patientsInfo Set HastaAdi=@HastaAdi, HastaSoyadi=@HastaSoyadi, HastaSikayeti=@HastaSikayeti, RandevuTarihi=@RandevuTarihi, RandevuSaati=@RandevuSaati, DoktorAdi=@DoktorAdi, TelefonNo=@TelefonNo where ID = @ID", con);
                    updateQuary.Parameters.AddWithValue("@ID", veriler.CurrentRow.Cells[0].Value);
                    updateQuary.Parameters.AddWithValue("@RandevuSaati", cb_UpdateHour.Text);
                    updateQuary.Parameters.AddWithValue("@HastaAdi", tb_UpdateName.Text);
                    updateQuary.Parameters.AddWithValue("@HastaSoyadi", tb_UpdateSurname.Text);
                    updateQuary.Parameters.AddWithValue("@HastaSikayeti", tb_UpdateComplaint.Text);
                    updateQuary.Parameters.AddWithValue("@RandevuTarihi", dtp_UpdateDate.Value.ToString("MM-dd-yyyy"));
                    updateQuary.Parameters.AddWithValue("@DoktorAdi", tb_UpdateDrName.Text);
                    updateQuary.Parameters.AddWithValue("@TelefonNo", mtb_UpdatePhone.Text);
                    updateQuary.ExecuteNonQuery();
                    MessageBox.Show("Güncelleme Yapıldı ! ");

                }
                catch
                {
                    MessageBox.Show("Güncelleme Başarısız !");
                }
            
        }
        //Form1 deki datagridwievden Id nin çekilmesi
        public DataGridView veriler { get; set; }

        //Çıkış butonu
        private void btn_UpdateExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
