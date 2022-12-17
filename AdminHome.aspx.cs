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
    public partial class AdminHome : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Heading1.InnerText = "Hello" + " " + Session["AdminName"];

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

            if (Session["show"].Equals("True"))
            {
                string n = "No";
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Mobile_No FROM tblCustomer WHERE Assign='" + n + "'",con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    Session["AssignMobNo"] = dr.GetDecimal(0);
                    dr.Close();
                    con.Close();
                    d = MessageBox.Show("You have connections to assign, would you like to assign them now??", "Confirmation", MessageBoxButtons.YesNo);
                    if (d == DialogResult.Yes)
                    {
                        Response.Redirect("AdminAssignDetails.aspx");
                    }
                    else if (d == DialogResult.No)
                    {
                        Session["show"] = "False";
                    }
                }
                dr.Close();
                con.Close();
            }

            if (Session["showD"].Equals("True"))
            {
                string y = "Yes";
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Customer_No FROM tblCustomer WHERE RemoveConn = '" + y + "'",con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    d = MessageBox.Show("You have some connections to cut, do you want to do it now??", "Delete Account", MessageBoxButtons.YesNo);
                    if (d == DialogResult.Yes)
                    {
                        dr.Read();
                        string c = dr.GetDecimal(0).ToString();
                        Session["DelCustNo"] = c;
                        Response.Redirect("CustDelete.aspx");
                    }
                    else if(d == DialogResult.No)
                    {
                        Session["showD"] = "False";
                        Response.Redirect("AdminHome.aspx");
                    }
                }
            }
        }
    }
}