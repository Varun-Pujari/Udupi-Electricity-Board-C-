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
    public partial class CustomerLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string d = DateTime.Now.ToString();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            //con.Open();
            //SqlCommand cmd = new SqlCommand("SELECT Password FROM tblCustomer WHERE Password='" + Password.Value + "' COLLATE Latin1_General_CS_AS AND Username='" + Username.Value + "' COLLATE Latin1_General_CS_AS AND Deleted = 'No'", con);
            //SqlDataReader dr = cmd.ExecuteReader();
            //if (dr.HasRows)
            //{
            //    Session["CustName"] = Username.Value;
            //    cmd = new SqlCommand("INSERT INTO tblCustLogin VALUES ('" + Username.Value + "', '" + d + "')", con);
            //    dr.Close();
            //    cmd.ExecuteNonQuery();

            //    cmd = new SqlCommand("SELECT Customer_No FROM tblCustomer WHERE Username='" + Username.Value + "' COLLATE Latin1_General_CS_AS", con);
            //    dr = cmd.ExecuteReader();
            //    if (dr.HasRows)
            //    {
            //        dr.Read();
            //        string c = dr.GetDecimal(0).ToString();
            //        Session["CustNo"] = c;
            //    }
            //    dr.Close();
            //    con.Close();
            //    Response.Redirect("CustomerHome.aspx");
            //}
            //else
            //{
            //    MessageBox.Show("Incorrect Password");
            //}
        }

        public void Submit(object sender, EventArgs e)
        {
            string d = DateTime.Now.ToString();
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Username FROM tblCustomer WHERE Username='" + Username.Value + "' COLLATE Latin1_General_CS_AS AND Deleted = 'No'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                cmd = new SqlCommand("SELECT Password FROM tblCustomer WHERE Password='" + Password.Value + "' COLLATE Latin1_General_CS_AS AND Username='" + Username.Value + "' COLLATE Latin1_General_CS_AS AND Deleted = 'No'", con);
                dr.Close();
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    Session["CustName"] = Username.Value;
                    cmd = new SqlCommand("INSERT INTO tblCustLogin VALUES ('" + Username.Value + "', '" + d + "')", con);
                    dr.Close();
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("SELECT Customer_No FROM tblCustomer WHERE Username='" + Username.Value + "' COLLATE Latin1_General_CS_AS", con);
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        string c = dr.GetDecimal(0).ToString();
                        Session["CustNo"] = c;
                    }
                    dr.Close();
                    con.Close();
                    Response.Redirect("CustomerHome.aspx");
                }
                else
                {
                    MessageBox.Show("Incorrect Password");
                }
            }
            else
            {
                MessageBox.Show("Username does not exist");
            }
            dr.Close();
            con.Close();
        }
    }
}