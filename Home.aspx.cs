using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace UEB2
{
    public partial class Home : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["AdminName"] = "";
            Session["CustName"] = "";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM tblNews",con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    News.InnerText = News.InnerText + " " + dr.GetString(0) + " : " + dr.GetString(1) + ";";
                }
                dr.Close();
                con.Close();
            }
            dr.Close();
            con.Close();
        }
    }
}