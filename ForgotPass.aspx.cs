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
    public partial class ForgorPass : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void SendMail(object sender, EventArgs e)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Email, Password FROM tblCustomer WHERE Email = '" + mail.Value + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                dr.Read();
                try
                {
                    MailMessage message = new MailMessage();
                    SmtpClient smtp = new SmtpClient();
                    message.From = new MailAddress("udupieb@gmail.com");
                    message.To.Add(new MailAddress(dr.GetString(0)));
                    message.Subject = "Your password";
                    message.IsBodyHtml = true;  
                    message.Body = "This is your password " + dr.GetString(1);   
                    smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
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
                con.Close();
            }
            else
            {
                dr.Close();
                con.Close();
                con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
                con.Open();
                cmd = new SqlCommand("SELECT Email, Password FROM tblAdmin WHERE Email = '" + mail.Value + "'", con);
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    try
                    {
                        MailMessage message = new MailMessage();
                        SmtpClient smtp = new SmtpClient();
                        message.From = new MailAddress("udupieb@gmail.com");
                        message.To.Add(new MailAddress(dr.GetString(0)));
                        message.Subject = "Your password";
                        message.IsBodyHtml = true;   
                        message.Body = "This is your password " + dr.GetString(1); 
                        smtp.Port = 587;
                        smtp.Host = "smtp.gmail.com";
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
                    con.Close();
                }
                else
                {
                    
                        dr.Close();
                        con.Close();
                        MessageBox.Show("No such email id has been registed yet");
                }
            }
        }
    }
}