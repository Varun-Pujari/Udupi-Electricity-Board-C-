using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace UEB2
{
    public partial class AdminAssets : System.Web.UI.Page
    {
        DialogResult d;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["AdminName"].Equals(" "))
                {
                    d = MessageBox.Show("Your session has expired please re-login");
                    if(d == DialogResult.OK)
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
            Username.Value = Convert.ToString(Session["AdminName"]);
        }

        public void Add(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT ASSETS FROM tblAssets WHERE Assets=@Assets", con);
            cmd.Parameters.AddWithValue("@Assets", Assets.Value);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                d = MessageBox.Show("This asset already exists");
                if (d == DialogResult.OK)
                {
                    con.Close();
                    Response.Redirect("AdminAssets.aspx");
                }
            }
            if(Convert.ToInt32(In_Use.Value) > Convert.ToInt32(Total.Value) ||  Convert.ToInt32(Active.Value) > Convert.ToInt32(In_Use.Value))
            {
                d = MessageBox.Show("Incorrect values inserted");
                if(d == DialogResult.OK)
                {
                    Response.Redirect("AdminAssets.aspx");
                }
            }
            int a = int.Parse(In_Use.Value) - int.Parse(Active.Value);
            cmd = new SqlCommand("INSERT INTO tblAssets VALUES('" + Assets.Value + "', " + Total.Value + ", " + In_Use.Value + ", " + Active.Value + ", " + a + ", '" + Username.Value + "')",con);
            cmd.ExecuteNonQuery();
            con.Close();
            Response.Redirect("AdminAssets.aspx");
        }
    }

    
}