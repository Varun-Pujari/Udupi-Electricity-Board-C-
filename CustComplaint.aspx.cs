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
    public partial class CustComplaint : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            DialogResult d;
            try
            {
                if (Session["CustName"].Equals(" "))
                {
                    d = MessageBox.Show("Your session has expired please re-login");
                    if (d == DialogResult.OK)
                    {
                        Response.Redirect("CustomerLogin.aspx");
                    }
                }
            }
            catch
            {
                d = MessageBox.Show("Your session has expired please re-login");
                if (d == DialogResult.OK)
                {
                    Response.Redirect("CustomerLogin.aspx");
                }
            }
        }

        public void Submit(object sender, EventArgs e)
        {
            string d = DateTime.Now.ToString();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO tblComplaint (Complaint, Complaint_Date, Cust_Name) VALUES ('" + Complaint.Value + "', '" + d + "', '" + Session["CustName"] + "')", con);
            cmd.ExecuteNonQuery();

            DialogResult dd;
            dd = MessageBox.Show("Your complaint has been registered");
            if(dd == DialogResult.OK)
            {
                Response.Redirect("CustomerHome.aspx");
            }
        }
    }
}