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
    public partial class GenerateBill : System.Web.UI.Page
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

            string n = "No";
            int i = 0;
            decimal[] arr = new decimal[100];
            DateTime dt;

            if (!IsPostBack)
            {
                SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT Due_Date, Customer_No FROM tblBill", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        //dt = dr.GetDateTime(0);
                        DateTime.TryParse(dr["Due_Date"].ToString(), out dt);
                        if (dt.CompareTo(DateTime.Now) < 0)
                        {
                            arr[i] = dr.GetDecimal(1);
                            i++;
                        }
                    }
                    dr.Close();
                    for(int a = 0; a<i; a++)
                    {
                        cmd = new SqlCommand("Update tblCustomer SET Bill = '" + n + "' WHERE( Customer_No = '" + arr[a] + "')",con);
                        cmd.ExecuteNonQuery();
                        cmd = new SqlCommand("Update tblBill SET Bill = '" + n + "' WHERE( Customer_No = '" + arr[a] + "')",con);
                        cmd.ExecuteNonQuery();
                    }
                }
                con.Close();
            }
        }

        public void GenBill(object sender, EventArgs e)
        {
            DialogResult d;
            string p = "Yes";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("INSERT INTO tblPastBill SELECT Name, Customer_No, RR_No, Location_Code, Meter_No, Sub_Division, Prev_Reading, Cur_Reading, Issued_Date, Bill_No, Penalty, Amount, Due_Date FROM tblBill WHERE (Customer_No = '" + CustNo.Value + "')", con);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("SELECT Due_Date, Customer_No FROM tblBill WHERE Customer_No = '" + CustNo.Value + "'",con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                DateTime dt;
                //dt = dr.GetDateTime(0);
                DateTime.TryParse(dr["Due_Date"].ToString(), out dt);
                if (dt.CompareTo(DateTime.Now) > 0)
                {
                    d = MessageBox.Show("Bill already generated");
                    if(d == DialogResult.OK)
                    {
                        dr.Close();
                        con.Close();
                        Response.Redirect("GenerateBill.aspx");
                    }
                }
            }
            dr.Close();
            cmd = new SqlCommand("SELECT Customer_No FROM tblBill WHERE Customer_No='" + CustNo.Value + "'",con);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Close();
                Session["GenBill"] = CustNo.Value;
                con.Close();
                Response.Redirect("GenerateBill1.aspx");
            }
            else
            {
                dr.Close();
                string pr = "0", cr = "0";
                cmd = new SqlCommand("INSERT INTO tblBill (Name, Customer_No, RR_No, Location_Code, Meter_No, Sub_Division) SELECT Name, Customer_No, RR_No, Location_Code, Meter_Code, Sub_Division FROM tblCustomer WHERE (Customer_No = '" + CustNo.Value + "')", con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("UPDATE tblBill SET Paid = '" + p + "' WHERE (Customer_No = '" + CustNo.Value + "')",con);
                cmd.ExecuteNonQuery();
                cmd = new SqlCommand("UPDATE tblBill SET Prev_Reading = '" + pr + "', Cur_Reading = '" + cr + "' WHERE Customer_No = '" + CustNo.Value + "'", con);
                cmd.ExecuteNonQuery();
                con.Close();

                Session["GenBill"] = CustNo.Value;
                Response.Redirect("GenerateBill1.aspx");
            }
        }
    }
}