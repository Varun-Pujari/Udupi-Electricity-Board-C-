using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace UEB2
{
    public partial class CustSignup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SignUp(object sender, EventArgs e)
        {
            DialogResult d;
            string b = "No";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Username FROM tblCustomer WHERE Username = '" + Uname.Value + "' COLLATE Latin1_General_CS_AS AND Deleted = '" + b + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                d = MessageBox.Show("Username already exists");
                if (d == DialogResult.OK)
                {
                    dr.Close();
                    con.Close();
                    Response.Redirect("CustomerLogin.aspx");
                }
            }
            dr.Close();

            cmd = new SqlCommand("SELECT * FROM tblCustomer WHERE Email = '" + Email.Value + "' AND Mobile_No = " + Phone.Value + " AND Aadhar_No = " + Aadhar.Value + " AND Customer_No = '" + Cno.Value + "' AND RR_No = '" + RR.Value + "' AND Location_Code = '" + Lcode.Value + "' AND Meter_Code = '" + Mcode.Value + "' AND Sub_Division = '" + SubDiv.Value + "' AND Deleted = '" + b + "'", con);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                cmd = new SqlCommand("UPDATE tblCustomer SET Username='" + Uname.Value + "', Password='" + Password.Value + "' WHERE Customer_No = '" + Cno.Value + "' AND Deleted = '" + b + "'", con);
                dr.Close();
                cmd.ExecuteNonQuery();
                d = MessageBox.Show("You have signed up successfully");
                if(d == DialogResult.OK)
                {
                    con.Close();
                    Response.Redirect("CustomerLogin.aspx");
                }
            }
            else
            {
                MessageBox.Show("The entered credentials are not valid");
            }
            con.Close();
        }
    }
}