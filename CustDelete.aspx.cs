using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;

namespace UEB2
{
    public partial class CustDelete : System.Web.UI.Page
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

            if(Session["DelCustNo"] != null)
            {
                CustomerNo.Value = Session["DelCustNo"].ToString();
            }
        }

        public void Del(object sender, EventArgs e)
        {
            DialogResult d;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Username FROM tblCustomer WHERE Customer_No = '" + CustomerNo.Value + "' AND Deleted ='No'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                d = MessageBox.Show("Invalid customer number");
                if(d == DialogResult.OK)
                {
                    dr.Close();
                    con.Close();
                    Response.Redirect("CustDelete.aspx");
                }
            }
            dr.Close();
            string t = "Sub", y = "Yes", n = "No";
            cmd = new SqlCommand("SELECT Name FROM tblAdmin WHERE Username = '" + Session["AdminName"] + "' COLLATE Latin1_General_CS_AS AND Type = '" + t + "' AND Deleted = '" + n + "'", con);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                d = MessageBox.Show("You are not authorized to perform this operation");
                if(d == DialogResult.OK)
                {
                    dr.Close();
                    con.Close();
                    Response.Redirect("AminHome.aspx");
                }
            }
            dr.Close();
            con.Close();

            d = MessageBox.Show("Are you sure you want to remove this customer's connection??","Confirmation",MessageBoxButtons.YesNoCancel,MessageBoxIcon.Question);
            if(d == DialogResult.Yes) { }
            else if(d == DialogResult.No)
            {
                cmd = new SqlCommand("SELECT RemoveConn FROM tblCustomer WHERE RemoveConn = '" + y + "' AND Customer_No = '" + CustomerNo.Value + "'", con);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Close();
                    cmd = new SqlCommand("UPDATE tblCustomer SET RemoveConn = '" + n + "' WHERE Customer_No = '" + CustomerNo.Value + "'", con);
                    cmd.ExecuteNonQuery();
                    cmd = new SqlCommand("SELECT Email FROM tblCustomer WHERE Customer_No = '" + CustomerNo.Value + "'", con);
                    dr = cmd.ExecuteReader();
                    dr.Read();

                    try
                    {
                        MailMessage message = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        message.From = new MailAddress("udupieb@gmail.com");
                        message.To.Add(new MailAddress(dr.GetString(0)));
                        message.Subject = "Request declined";
                        message.IsBodyHtml = true; //to make message body as html  
                        message.Body = "Request to remove your electricity connection has been declined. For further queries mail the same.";
                        smtp.Port = 587;
                        smtp.Host = "smtp.gmail.com"; //for gmail host  
                        smtp.EnableSsl = true;
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential("udupieb@gmail.com", "iapiktlufgoykpzi");
                        smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                        smtp.Send(message);
                        MessageBox.Show("Mail sent successfully");
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Mail could not be sent");
                    }
                }
                dr.Close();
                con.Close();
                Response.Redirect("CustDelete.aspx");
            }
            else if(d == DialogResult.Cancel)
            {
                con.Close();
                Response.Redirect("CustDelete.aspx");
            }
            con.Open();
            cmd = new SqlCommand("SELECT Customer_No FROM tblBill WHERE Customer_No = '" + CustomerNo.Value + "' AND Paid ='" + n + "'",con);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                d = MessageBox.Show(dr.GetDecimal(0) + " Has not yet paid his bills, so his.her connection cannot be removed");
                if(d == DialogResult.OK)
                {
                    dr.Close();
                    con.Close();
                    Response.Redirect("CustDelete.aspx");
                }
            }
            dr.Close();

            cmd = new SqlCommand("SELECT Email, Name FROM tblCustomer WHERE Customer_No = '" + CustomerNo.Value + "' AND Deleted = '" + n + "'", con);
            dr = cmd.ExecuteReader();
            dr.Read();
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("udupieb@gmail.com");
                message.To.Add(new MailAddress(dr.GetString(0)));
                message.Subject = "Connection removed successfully";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = dr.GetString(1) + ", Your electricity connection has been successfully removed.";
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com"; //for gmail host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("udupieb@gmail.com", "iapiktlufgoykpzi");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
                MessageBox.Show("Mail sent successfully");
        }
            catch (Exception)
            {
                MessageBox.Show("Mail could not be sent");
            }
    dr.Close();

            cmd = new SqlCommand("SELECT Username FROM tblCustomer WHERE Customer_No = '" + CustomerNo.Value + "' AND Deleted = '" + n + "'", con);
            dr = cmd.ExecuteReader();
            dr.Read();
            string u;
            try
            {
                u = dr.GetString(0);
            }
            catch
            {
                u = " ";
            }
            dr.Close();

            cmd = new SqlCommand("DELETE FROM tblComplaint WHERE Cust_Name = '" + u + "' COLLATE Latin1_General_CS_AS",con);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("DELETE FROM tblFeedback WHERE Name = '" + u + "' COLLATE Latin1_General_CS_AS",con);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("DELETE FROM tblCustLogin WHERE Username = '" + u + "' COLLATE Latin1_General_CS_AS",con);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("INSERT INTO tblPastBill SELECT Name, Customer_No, RR_No, Location_Code, Meter_No, Sub_Division, Prev_Reading, Cur_Reading, Issued_Date, Bill_No, Penalty, Amount, Due_Date FROM tblBill WHERE (Customer_No = '" + CustomerNo.Value + "')",con);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("DELETE FROM tblBill WHERE Customer_No = '" + CustomerNo.Value + "'", con);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("UPDATE tblCustomer SET RemoveConn = '" + n + "', Deleted = '" + y + "' WHERE Customer_No = '" + CustomerNo.Value + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();
            Session["DelCustNo"] = " ";
           d = MessageBox.Show("Record deleted successfully");
           if(d == DialogResult.OK)
            {
                Response.Redirect("CustDelete.aspx");
            }
        }
    }
}