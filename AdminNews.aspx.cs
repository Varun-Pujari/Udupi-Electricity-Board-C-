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
    public partial class AdminNews : System.Web.UI.Page
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
            SqlCommand cmd = new SqlCommand("INSERT INTO tblNews VALUES ('" + NTitle.Value + "', '" + News.Value + "', '" + Session["AdminName"] + "', '" + d + "')", con);
            cmd.ExecuteNonQuery();
            con.Close();

            dd = MessageBox.Show("The news feed has been updated");
            if (dd == DialogResult.OK)
            {
                Response.Redirect("AdminHome.aspx");
            }
        }
    }
}