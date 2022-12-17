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
using Microsoft.VisualBasic;

namespace UEB2
{
    public partial class CustReg : System.Web.UI.Page
    {

        // Instantiate random number generator.  
        readonly Random _random = new Random();

        // Generates a random number within a range.      
        public int RandomNumber(int min, int max)
        {
            return _random.Next(min, max);
        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void Register(object sender, EventArgs e)
        {
            DialogResult d;
            int v;
            v = RandomNumber(0, 100000);
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("udupieb@gmail.com");
                message.To.Add(new MailAddress(Email.Value));
                message.Subject = "Verification code";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = "The verification code is " + v;
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
            d = MessageBox.Show("A verification has been sent to your mail");
            if (d == DialogResult.OK)
            {
                int res = 0;
                while (v != res)
                {
                    try
                    {
                        res = int.Parse(Interaction.InputBox("Enter the verification code sent", "Verification", "0", 250, 50));
                        if (res != v)
                            MessageBox.Show("Incorrect verification code");
                    }
                    catch
                    {
                        MessageBox.Show("Incorrect verification code");
                    }
                }
            }

            string n = "No";
            string filename = ImageUpload.PostedFile.FileName;
            string filepath;
            filepath = "~/assets/images/" + ImageUpload.PostedFile.FileName;
            ImageUpload.PostedFile.SaveAs(Server.MapPath("~/assets/images/") + filename);

            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM tblCustomer WHERE (Email = '" + Email.Value + "' OR Mobile_No = '" + Phone.Value + "' OR Aadhar_No = '" + Aadhar.Value + "') AND Deleted='" + n + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                d = MessageBox.Show("Entered credentials have already been registered");
                if (d == DialogResult.OK)
                {
                    dr.Close();
                    Response.Redirect("CustReg.aspx");
                }
                dr.Close();
            }
            dr.Close();

            string r = RandomNumber(100000000,999999999).ToString();
            cmd = new SqlCommand("INSERT INTO tblCustomer (Name, Address, Pin, Email, Mobile_No, Aadhar_No, CustImage, Customer_No) VALUES ('" + Name.Value.ToUpper() + "', '" + Address.Value.ToUpper() + "'," + Pin.Value + ",'" + Email.Value + "', " + Phone.Value + "," + Aadhar.Value + ", '" + filepath + "', '" + r + "' )", con);
            cmd.ExecuteNonQuery();
            con.Close();

            d = MessageBox.Show("You have registered successfully");
            if (d == DialogResult.OK)
                Response.Redirect("Home.aspx");
        }
    }
}