using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace RandevuTakp
{
    public partial class HastaBilgisi : Form
    {
        SqlConnection con = new SqlConnection();
        public HastaBilgisi()
        {
            InitializeComponent();
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
        private void HastaBilgisi_Load(object sender, EventArgs e)
        {
            open_Connection();
            CbBox();
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tb_Name.Text == "")
            {
                MessageBox.Show("Lütfen Kayıt Girişi Yapın !", "Bilgi");
            }
            else
            {
                SqlCommand add_appointment = new SqlCommand("appointmentproc", con);
                add_appointment.CommandType = CommandType.StoredProcedure;
                add_appointment.Parameters.Add(new SqlParameter("@HastaAdi", tb_Name.Text));
                add_appointment.Parameters.Add(new SqlParameter("@HastaSoyadi", tb_Surname.Text));
                add_appointment.Parameters.Add(new SqlParameter("@HastaSikayeti", tb_complaint.Text));
                add_appointment.Parameters.Add(new SqlParameter("@RandevuTarihi", dtp_date.Value));
                add_appointment.Parameters.Add(new SqlParameter("@RandevuSaati", cb_Hour.Text));
                add_appointment.Parameters.Add(new SqlParameter("@DoktorAdi", tb_DrName.Text));
                add_appointment.Parameters.Add(new SqlParameter("@TelefonNo", tb_Phone.Text));
                add_appointment.ExecuteNonQuery();
                MessageBox.Show("Kayıt Tamamlandı !");
            }
        }
        public void CbBox()
        {
            SqlCommand cmd = new SqlCommand("select CONVERT(VARCHAR(10),HourItems,108) AS CbHour from ComboBox  ", con);
            cb_Hour.DataSource = null;
            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                cb_Hour.Items.Add(dr["CbHour"]);
            }
            dr.Close();
        }
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
