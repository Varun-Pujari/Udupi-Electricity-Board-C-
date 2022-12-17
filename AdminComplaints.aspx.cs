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
    public partial class AdminComplaints : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DialogResult d;
            try
            {
                if (Session["AdminName"].Equals(" "))
                {
                    d = MessageBox.Show("Your session has expired please re-login");
                    if (d == DialogResult.OK)
                    {
                        Response.Redirect("AdminLogin.aspx");
                    }
                }
            }
            catch
            {
                d = MessageBox.Show("Your session has expired please re-login");
                if (d == DialogResult.OK)
                {
                    Response.Redirect("AdminLogin.aspx");
                }
            }
        }
        
        public void Submit(object sender, EventArgs e)
        {
            DialogResult dd;
            string d = DateTime.Now.ToString();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Cust_Name FROM tblComplaint WHERE Cust_Name='" + Cust_Name.Value + "' COLLATE Latin1_General_CS_AS AND Complaint_Date ='" + C_Date.Value + "'",con);
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                dr.Close();
                cmd = new SqlCommand("UPDATE tblComplaint SET Admin_Name = '" + Session["AdminName"] + "', Reply = '" + Complaint_Reply.Value + "', Reply_date = '" + d + "' WHERE Cust_Name = '" + Cust_Name.Value + "' COLLATE Latin1_General_CS_AS AND Complaint_Date = '" + C_Date.Value + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();
                dd = MessageBox.Show("Your complaint has been recorded");
                if (dd == DialogResult.OK)
                    Response.Redirect("AdminHome.aspx");
            }
            else
            {
                dr.Close();
                con.Close();
                dd = MessageBox.Show("Please enter a valid customer name and the corresponding date of the complaint");
                if (dd == DialogResult.OK)
                    Response.Redirect("AdminComplaints.aspx");
            }
        }
    }
}