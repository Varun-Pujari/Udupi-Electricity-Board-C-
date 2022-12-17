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
    public partial class AdminDelete : System.Web.UI.Page
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
        }

        public void Del(object sender, EventArgs e)
        {
            DialogResult d;
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Username FROM tblAdmin WHERE Username = '" + Username.Value + "' AND Deleted ='No'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (!dr.HasRows)
            {
                d = MessageBox.Show("Invalid user name");
                if (d == DialogResult.OK)
                {
                    dr.Close();
                    con.Close();
                    Response.Redirect("AdminDelete.aspx");
                }
            }
            dr.Close();
            string t = "Sub", n = "No";
            cmd = new SqlCommand("SELECT Name FROM tblAdmin WHERE Username = '" + Session["AdminName"] + "' COLLATE Latin1_General_CS_AS AND Type = '" + t + "' AND Deleted = '" + n + "'", con);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                d = MessageBox.Show("You are not authorized to perform this operation.");
                if (d == DialogResult.OK)
                {
                    dr.Close();
                    con.Close();
                    Response.Redirect("AdminHome.aspx");
                }
            }
            dr.Close();
            con.Close();

            d = MessageBox.Show("Are you sure you want to delete this admin??","Confirmation",MessageBoxButtons.YesNo);
            if (d == DialogResult.Yes)
            {
            }
            else if (d == DialogResult.No)
            {
                Response.Redirect("AdminHome.aspx");
            }

            cmd = new SqlCommand("SELECT Name, Email FROM tblAdmin WHERE Username = '" + Username.Value + "' COLLATE Latin1_General_CS_AS AND Deleted = '" + n + "' ", con);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                try
                {
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    message.From = new MailAddress("udupieb@gmail.com");
                    message.To.Add(new MailAddress(dr.GetString(1)));
                    message.Subject = "Removed from your post";
                    message.IsBodyHtml = true; //to make message body as html  
                    message.Body = "Dear " + dr.GetString(0) + "\n We are sorry to inform you that you have been removed from the admin post in Udupi Electricity Board.";
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com"; //for gmail host  
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("udupieb@gmail.com", "iapiktlufgoykpzi");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                    MessageBox.Show("Mail sent successfully");
                    Session["AssignMobNo"] = "";
                    d = MessageBox.Show("Mail Sent");
                }
                catch (Exception)
                {
                    MessageBox.Show("Mail could not be sent");
                }
            }
            else
            {
                d = MessageBox.Show("No such username exists.");
                if (d == DialogResult.OK)
                {
                    dr.Close();
                    con.Close();
                    Response.Redirect("AdminHome.aspx");
                }
            }
            dr.Close();

            cmd = new SqlCommand("DELETE FROM tblFeedback WHERE Name = '" + Username.Value + "' COLLATE Latin1_General_CS_AS");
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("DELETE FROM tblAdminLogin WHERE Username = '" + Username.Value + "' COLLATE Latin1_General_CS_AS");
            cmd.ExecuteNonQuery();

            string y = "Yes";
            cmd = new SqlCommand("UPDATE tblAdmin SET Deleted = '" + y + "' WHERE Username = '" + Username.Value + "' COLLATE Latin1_General_CS_AS");
            cmd.ExecuteNonQuery();
            con.Close();
            d = MessageBox.Show("Record deleted Successfully");
            if (d == DialogResult.OK)
            {

                Response.Redirect("AdminDelete.aspx");
            }
        }
    }
}