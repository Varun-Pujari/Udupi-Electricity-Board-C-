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
    public partial class CustProfile : System.Web.UI.Page
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

            if (!IsPostBack)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Name, Address, Email, Mobile_No, Aadhar_No, Username, Password FROM tblCustomer WHERE Username = '" + Session["CustName"] + "' COLLATE Latin1_General_CS_AS", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    Name.Value = dr.GetString(0);
                    Mobile.Value = dr.GetDecimal(3).ToString();
                    Email.Value = dr.GetString(2);
                    Address.Value = dr.GetString(1);
                    Username.Value = dr.GetString(5);
                    Password.Value = dr.GetString(6);
                    Aadhar.Value = dr.GetDecimal(4).ToString();
                }

                cmd = new SqlCommand("SELECT CustImage FROM tblCustomer WHERE Aadhar_No = '" + Aadhar.Value + "'",con);
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

        public void Update(object sender, EventArgs e)
        {
            string filename, filepath;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT CustImage FROM tblCustomer WHERE Username = '" + Session["CustName"] + "' COLLATE Latin1_General_CS_AS", con);
            SqlDataReader dr = cmd.ExecuteReader();
            dr.Read();
                filepath = dr.GetString(0);
                dr.Close();
                int l = ImageUpload.PostedFile.FileName.Length;
                if (l > 0)
                {
                    filename = ImageUpload.PostedFile.FileName;
                    filepath = "~/assets/images/" + ImageUpload.PostedFile.FileName;
                    ImageUpload.PostedFile.SaveAs(Server.MapPath("~/assets/images/") + filename);
                }

                cmd = new SqlCommand("UPDATE tblCustomer SET Name = '" + Name.Value.ToUpper() + "', Mobile_No = '" + Mobile.Value + "', Email = '" + Email.Value + "', Password = '" + Password.Value + "', CustImage = '" + filepath + "' WHERE Username = '" + Session["CustName"] + "' COLLATE Latin1_General_CS_AS", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show("Updated");
        }
    }
}