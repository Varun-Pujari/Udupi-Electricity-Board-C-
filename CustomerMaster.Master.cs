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
    public partial class CustomerMaster : System.Web.UI.MasterPage
    {
        public void DeleteAcc(object sender, EventArgs e)
        {
            DialogResult d;
            d = MessageBox.Show("Are you sure to delete your account","Confirmation",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if(d == DialogResult.Yes)
            {
                //string y = "Yes";
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("DELETE FROM tblComplaint WHERE Cust_Name = '" + Session["CustName"] + "' COLLATE Latin1_General_CS_AS", con);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("DELETE FROM tblFeedback WHERE Name = '" + Session["CustName"] + "' COLLATE Latin1_General_CS_AS", con);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("DELETE FROM tblCustLogin WHERE Username = '" + Session["CustName"] + "' COLLATE Latin1_General_CS_AS", con);
                cmd.ExecuteNonQuery();

                cmd = new SqlCommand("UPDATE tblCustomer SET Username = NULL, Password = NULL WHERE Customer_No = " + Session["CustNo"] + " AND Deleted = 'No'", con);
                cmd.ExecuteNonQuery();
                con.Close();

                d = MessageBox.Show("Your account has been deleted");
                if(d == DialogResult.OK)
                {
                    Response.Redirect("Home.aspx");
                }
            }
            else if (d == DialogResult.No){ }
        }

        public void DeleteConn(object sender, EventArgs e)
        {
            DialogResult d;
            string n = "No";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Customer_No FROM tblBill WHERE Customer_No = '" + Session["CustNo"] + "' AND Paid ='" + n + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                d = MessageBox.Show("You have not yet paid your bills so, your connection cannot be removed");
                if(d == DialogResult.OK)
                {
                    dr.Close();
                    con.Close();
                    Response.Redirect("CustomerHome.aspx");
                }
            }
            dr.Close();

            d = MessageBox.Show("Are you sure to cut your electricty connection","Cut Electricity Connection", MessageBoxButtons.YesNo);
            if (d == DialogResult.Yes)
            {
                string y = "Yes";
                cmd = new SqlCommand("UPDATE tblCustomer SET RemoveConn = '" + y + "' WHERE (Customer_No = '" + Session["CustNo"] + "')", con);
                cmd.ExecuteNonQuery();
                con.Close();
                d = MessageBox.Show("Your reuest has been sent and will be dealth with ASAP");
                if (d == DialogResult.OK)
                {
                    Response.Redirect("Home.aspx");
                }
                else
                    Response.Redirect("Home.aspx");
            }
            else if (d == DialogResult.No)
                con.Close();
        }

    }

    
}