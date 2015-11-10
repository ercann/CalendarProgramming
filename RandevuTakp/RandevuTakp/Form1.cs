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
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection();
        public Form1()
        {
            InitializeComponent();
        }

        public void open_Connection()
        {
            con.ConnectionString = "Data Source = ERCAN ; database = appointment ; integrated security = true";
            con.Open();
        }
        private void btnAppointmentAdd_Click(object sender, EventArgs e)
        {
            HastaBilgisi open = new HastaBilgisi();
            open.ShowDialog();
            GetRecord();
        }
        //Datagrid e Verileri Çek
        public void GetRecord()
        {
            string today = DateTime.Now.ToString("MM-dd-yyyy");
            SqlCommand cmd = new SqlCommand("SELECT * FROM PatientsInfo WHERE RandevuTarihi = @RandevuTarihi ORDER BY RandevuSaati ASC", con);
            cmd.Parameters.AddWithValue("@RandevuTarihi", today);
            SqlDataAdapter daGrid = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            daGrid.Fill(dt);
            dgDiary.DataSource = dt;
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            // TODO: This line of code loads data into the 'appointmentDataSet.PatientsInfo' table. You can move, or remove it, as needed.
            this.patientsInfoTableAdapter.Fill(this.appointmentDataSet.PatientsInfo);
            open_Connection();
            GetRecord();
            Metodlar metod = new Metodlar();
            try
            {
                metod.RandevuTarihi(monthCalendar1);
            } 
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
        }

        //Tabloyu yenile
        public void btnRefresh_Click(object sender, EventArgs e)
        {
            GetRecord();
        }

        private void BtnExt_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        //Sil Butonu
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (dgDiary.Rows[0].Cells[0].Value == null)
            {
                MessageBox.Show("Lütfen Silinecek Satırı Seçin !", "Bilgi");
            }
            else
            {
            SqlCommand deleteQuery = new SqlCommand("DELETE FROM patientsInfo where ID =@ID", con);
            deleteQuery.Parameters.AddWithValue("@ID", dgDiary.CurrentRow.Cells[0].Value);
            deleteQuery.ExecuteNonQuery();
            Appointment();
            }

        }
        //Güncelle Butonu
        public void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgDiary.Rows[0].Cells[0].Value == null)
            {
                MessageBox.Show("Lütfen Güncellenecek Satırı Seçin !", "Bilgi");
            }
            else
            {
                Guncelle frm = new Guncelle();
                frm.veriler = dgDiary;
                frm.ShowDialog();
            }
        }

        //Soyada göre arama
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SqlCommand SearchSurname = new SqlCommand("Select * FROM patientsInfo Where HastaSoyadi=@HastaSoyadi", con);
            SearchSurname.Parameters.AddWithValue("@HastaSoyadi", tbSurnameSearch.Text);
            SearchSurname.ExecuteNonQuery();
            SqlDataAdapter daSurname = new SqlDataAdapter(SearchSurname);
            DataTable surnametable = new DataTable();
            daSurname.Fill(surnametable);
            dgDiary.DataSource = surnametable;
        }

        //Tarihe göre arama
        private void dtpSearchDate_ValueChanged(object sender, EventArgs e)
        {
            SqlCommand SearchDate = new SqlCommand("select * from PatientsInfo where RandevuTarihi=@RandevuTarihi", con);
            SearchDate.Parameters.AddWithValue("@RandevuTarihi", dtpSearchDate.Value.ToString("MM-dd-yyyy"));
            SearchDate.ExecuteNonQuery();
            SqlDataAdapter daDate = new SqlDataAdapter(SearchDate);
            DataTable datetable = new DataTable();
            daDate.Fill(datetable);
            dgDiary.DataSource = datetable;
        }

        //DateTimePicer da Randevuleri Arama
        private void btnAllLisy_Click(object sender, EventArgs e)
        {
            Appointment();
        }

        //Tüm Randevuler
        public void Appointment()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM PatientsInfo  ORDER BY RandevuSaati ASC", con);
            SqlDataAdapter daGrid = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            daGrid.Fill(dt);
            dgDiary.DataSource = dt;
        }

        //Takvim Üzerinde Etkinlikleri Gösterme
        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            string SelectDate = monthCalendar1.SelectionRange.Start.ToString("MM-dd-yyyy");
            SqlCommand SearchDate = new SqlCommand("select * from PatientsInfo where RandevuTarihi= '" + SelectDate + "'", con);
            SearchDate.ExecuteNonQuery();
            SqlDataAdapter daDate = new SqlDataAdapter(SearchDate);
            DataTable datetable = new DataTable();
            daDate.Fill(datetable);

            dgDiary.DataSource = datetable;
        }
    }
}
