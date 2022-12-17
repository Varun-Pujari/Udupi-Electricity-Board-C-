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
    public partial class BillRates : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Rate FROM tblRate", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    Rate.Value = dr.GetDecimal(0).ToString();
                }
                dr.Close();
                con.Close();
            }
        }

        public void Update(object sender, EventArgs e)
        {
            string d = DateTime.Now.ToString();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE tblRate SET Rate=" + Rate.Value + ", Date='" + d + "', Username ='" + Session["AdminName"] + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
            MessageBox.Show("Rate updated");
        }
    }
}