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
    public partial class AdminLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        public void Login(object sender, EventArgs e)
        {
            DialogResult dd;
            string d = DateTime.Now.ToString();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Username FROM tblAdmin WHERE Username='" + Username.Value + "' COLLATE Latin1_General_CS_AS AND Deleted = 'No'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                cmd = new SqlCommand("SELECT Password FROM tblAdmin WHERE Password='" + Password.Value + "' COLLATE Latin1_General_CS_AS AND Username = '" + Username.Value + "' COLLATE Latin1_General_CS_AS AND Deleted = 'No'", con);
                dr.Close();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    Session["AdminName"] = Username.Value;
                    cmd = new SqlCommand("INSERT INTO tblAdminLogin VALUES ('" + Username.Value + "', '" + d + "')", con);
                    dr.Close();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Session["show"] = "True";
                    Session["showD"] = "True";
                    Response.Redirect("AdminHome.aspx");
                }
                else
                {
                    dd = MessageBox.Show("Incorrect Password");
                }
            }
            else
            {
                dd = MessageBox.Show("Username does not exist");
            }
            dr.Close();
            con.Close();
        }
    }
}