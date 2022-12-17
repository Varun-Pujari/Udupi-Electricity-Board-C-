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
    public partial class AdminProfile : System.Web.UI.Page
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM tblAdmin WHERE Username='" + Session["AdminName"] + "' COLLATE Latin1_General_CS_AS", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    Name.Value = dr.GetString(0);
                    Mobile.Value = dr.GetDecimal(1).ToString();
                    Email.Value = dr.GetString(2);
                    Address.Value = dr.GetString(3);
                    Username.Value = dr.GetString(4);
                    Password.Value = dr.GetString(5);
                    Aadhar.Value = dr.GetDecimal(6).ToString();
                }
                cmd = new SqlCommand("SELECT AdminImage FROM tblAdmin WHERE Aadhar = '" + Aadhar.Value + "'", con);
                dr.Close();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    CustImage.Src = dr.GetString(0);
                }
                dr.Close();
                con.Close();
            }
        }

        public void EditP(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Password FROM tblAdmin WHERE Username='" + Session["AdminName"] + "' COLLATE Latin1_General_CS_AS", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                Password.Value = dr.GetString(0);
            }
            dr.Close();
            con.Close();
        }

        public void Update(object sender, EventArgs e)
        {
            string filename, filepath;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT AdminImage FROM tblAdmin WHERE Username = '" + Session["AdminName"] + "' COLLATE Latin1_General_CS_AS", con);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
            filepath = dr.GetString(0);
            dr.Close();
            int l = ImageUpload.PostedFile.FileName.Length;
            if(l>0)
            {
                filename = ImageUpload.PostedFile.FileName;
                filepath = "~/assets/images/" + ImageUpload.PostedFile.FileName;
                ImageUpload.PostedFile.SaveAs(Server.MapPath("~/assets/images/") + filename);
            }
            cmd = new SqlCommand("UPDATE tblAdmin SET Name = '" + Name.Value.ToUpper() + "', Mobile_No = '" + Mobile.Value + "', Email = '" + Email.Value + "', Address = '" + Address.Value.ToUpper() + "', Username = '" + Username.Value + "', Password = '" + Password.Value + "', AdminImage='" + filepath + "' WHERE Username = '" + Session["AdminName"] + "' COLLATE Latin1_General_CS_AS", con);
            cmd.ExecuteNonQuery();
            con.Close();
            Session["AdminName"] = Username.Value;
            MessageBox.Show("Updated");
        }
    }
}