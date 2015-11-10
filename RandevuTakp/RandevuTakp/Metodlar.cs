using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace RandevuTakp
{
    class Metodlar
    {
       
         SqlConnection con = new SqlConnection();
        internal void RandevuTarihi(System.Windows.Forms.MonthCalendar monthCalendar1)
        {
            con.ConnectionString = "Data Source = ERCAN ; database = appointment ; integrated security = true";
            con.Open();


            SqlCommand cmd = new SqlCommand("select RandevuTarihi from PatientsInfo  ", con);

            SqlDataReader dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                monthCalendar1.AddBoldedDate(DateTime.Parse(dr["RandevuTarihi"].ToString()));
            }
            monthCalendar1.UpdateBoldedDates();
            dr.Close();
        }
    }
}
