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
    public partial class GenerateBill1 : System.Web.UI.Page
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
                SqlCommand cmd = new SqlCommand("SELECT Prev_Reading, Cur_Reading FROM tblBill WHERE Customer_No = '" + Session["GenBill"] + "'", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    string p = dr.GetDecimal(1).ToString();
                    cmd = new SqlCommand("UPDATE tblBill SET Prev_Reading = '" + p + "' WHERE Customer_No = '" + Session["GenBill"] + "'", con);
                    dr.Close();
                    cmd.ExecuteNonQuery();
                }
                dr.Close();
                con.Close();
            }
        }

        public void Updated(object sender, EventArgs e)
        {
            DialogResult d;
            string y = "Yes";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("UPDATE tblBill SET Bill = '" + y + "'  WHERE (Customer_No = '" + Session["GenBill"] + "')", con);
            cmd.ExecuteNonQuery();
            cmd = new SqlCommand("UPDATE tblCustomer SET Bill = '" + y + "'  WHERE (Customer_No = '" + Session["GenBill"] + "')", con);
            cmd.ExecuteNonQuery();
            con.Close();

            if (IsPostBack)
            {
                int units;
                int amt = 0, rate;
                con.Open();
                cmd = new SqlCommand("SELECT Prev_Reading, Cur_Reading FROM tblBill WHERE (Customer_No = '" + Session["GenBill"] + "')", con);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    if (dr.GetDecimal(0) > dr.GetDecimal(1))
                    {
                        d = MessageBox.Show("Previous reading cannot be greater than current reading");
                        if (d == DialogResult.OK)
                        {
                            dr.Close();
                            con.Close();
                            Response.Redirect("GenerateBill1.aspx");
                        }
                    }
                }
                    units = (int)dr.GetDecimal(1) - (int)dr.GetDecimal(0);
                    dr.Close();
                    cmd = new SqlCommand("SELECT Rate FROM tblRate",con);
                    dr = cmd.ExecuteReader();
                    dr.Read();
                    rate = (int)dr.GetDecimal(0);
                    dr.Close();
                    if (units <= 150)
                        amt = 200;
                    else if (units <= 200)
                        amt = 200 + (units - 150) * rate;
                    else if (units <= 300)
                        amt = 200 + 5 * rate + (units - 200) * (rate * 2);
                    else if (units > 300)
                        amt = 200 + (50 * rate) + 100 * (rate * 2) + (units - 300) * (rate * 3);

                    if (units > 300 && amt < 500)
                        amt = 500;

                    string p = "No";
                    int a = 50;

                    cmd = new SqlCommand("SELECT Amount FROM tblBill WHERE (Customer_No = '" + Session["GenBill"] + "' AND Paid = '" + p + "')", con);
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                    amt += (int)dr.GetDecimal(0);
                    }
                    dr.Close();
                    cmd = new SqlCommand("SELECT Penalty FROM tblBill WHERE (Customer_No = '" + Session["GenBill"] + "' AND Paid = '" + p + "' )", con);
                    dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        int t = (int)dr.GetDecimal(0) + a;
                        cmd = new SqlCommand("UPDATE tblBill SET Penalty = " + t.ToString() + " WHERE (Customer_No = '" + Session["GenBill"] + "')", con);
                        dr.Close();
                        cmd.ExecuteNonQuery();
                        amt += a;
                    }
                    else
                    {
                        dr.Close();
                        int z = 0;
                        cmd = new SqlCommand("UPDATE tblBill SET Penalty = " + z.ToString() + " WHERE (Customer_No = '" + Session["GenBill"] + "' )", con);
                        cmd.ExecuteNonQuery();
                    }

                    cmd = new SqlCommand("UPDATE tblBill SET Amount = '" + amt + "', Paid='" + p + "' WHERE(Customer_No = '" + Session["GenBill"] + "')", con);
                    cmd.ExecuteNonQuery();

                    string IssuedDate, DueDate;
                //IssuedDate = DateTime.Now;
                IssuedDate = DateTime.Now.ToString().Substring(0, 10);
                //DueDate = DateTime.Now.AddDays(10);
                DueDate = DateTime.Now.AddDays(10).ToString().Substring(0, 10);
                cmd = new SqlCommand("UPDATE tblBill SET Issued_Date = '" + IssuedDate + "', Due_Date = '" + DueDate + "' WHERE(Customer_No = '" + Session["GenBill"] + "')", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Response.Redirect("GenerateBill.aspx");
                }
        }
    }
}