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
    public partial class CustFeedback : System.Web.UI.Page
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

            Name.Value = Session["CustName"].ToString();
        }

        public void Submit(object sender, EventArgs e)
        {
            DialogResult dd;
            string d = DateTime.Now.ToString();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO tblFeedback VALUES ('" + Name.Value + "', '" + Feedback.Value + "', '" + d + "')", con);
            cmd.ExecuteNonQuery();
            con.Close();

            dd = MessageBox.Show("Your feedback has been recorded");
            if(dd == DialogResult.OK)
            {
                Response.Redirect("CustomerHome.aspx");
            }
        }
    }
}