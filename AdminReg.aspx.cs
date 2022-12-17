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
    public partial class AdminReg : System.Web.UI.Page
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

        public void Register(object sender, EventArgs e)
        {
            DialogResult d;
            string t = "Sub", n = "No";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Name FROM tblAdmin WHERE Username = '" + Session["AdminName"] + "' COLLATE Latin1_General_CS_AS AND Type = '" + t + "' AND Deleted = '" + n + "'", con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                d = MessageBox.Show("You are not authorized to perform this operation.");
                if(d == DialogResult.OK)
                {
                    dr.Close();
                    con.Close();
                    Response.Redirect("AdminHome.aspx");
                }
            }
            dr.Close();

            int c = 0;
            cmd = new SqlCommand("SELECT Username FROM tblAdmin WHERE Deleted = '" + n + "'", con);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    c += 1;
                }
                if (c >= 3)
                {
                    d = MessageBox.Show("You cannot add more than 3 admins");
                    if (d == DialogResult.OK)
                    {
                        Response.Redirect("AdminHome.aspx");
                    }
                }
            }
            dr.Close();
            cmd = new SqlCommand("SELECT Username FROM tblAdmin WHERE Username = '" + Username.Value + "' COLLATE Latin1_General_CS_AS AND Deleted = '" + n + "'", con);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                d = MessageBox.Show("Username already exists");
                if(d == DialogResult.OK)
                {

                }
            }
            dr.Close();
            cmd = new SqlCommand("SELECT * FROM tblAdmin WHERE Mobile_No = '" + Phone.Value + "' OR Email = '" + Email.Value + "' OR Aadhar = '" + Aadhar.Value + "' AND Deleted='" + n + "'", con);
            dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                d = MessageBox.Show("Credentials already registered");
                if(d == DialogResult.OK)
                {
                    dr.Close();
                    Response.Redirect("Admin.Reg.aspx");
                }
            }
            dr.Close();

            string filename = ImageUpload.PostedFile.FileName;
            string filepath = "~/assets/images/" + ImageUpload.PostedFile.FileName;
            ImageUpload.PostedFile.SaveAs(Server.MapPath("~/assets/images/") + filename);

            cmd = new SqlCommand("INSERT INTO tblAdmin (Name, Mobile_No, Email, Address, Username, Password, Aadhar, AdminImage) VALUES ('" + Name.Value.ToUpper() + "', " + Phone.Value + ", '" + Email.Value + "', '" + Address.Value.ToUpper() + "', '" + Username.Value + "', '" + Password.Value + "', " + Aadhar.Value + ", '" + filepath + "'", con);
            cmd.ExecuteNonQuery();
            con.Close();

            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("udupieb@gmail.com");
                message.To.Add(new MailAddress(Email.Value));
                message.Subject = "Appointed as the new admin";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = "Hello " + Name.Value + "\n Congratulations on becoming the new admin of the Udupi Electricity Board. \nUsername: " + Username.Value + "\nPassword: " + Password.Value;
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
    }
}