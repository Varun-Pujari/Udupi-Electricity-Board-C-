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
    public partial class AdminAssignDetails : System.Web.UI.Page
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

            MobileNo.Value = Convert.ToString(Session["AssignMobNo"]);
            string locC="";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT Pin FROM tblCustomer WHERE Mobile_No='" + MobileNo.Value + "'",con);
            SqlDataReader dr = cmd.ExecuteReader();
            if(dr.HasRows)
            {
                dr.Read();
                if (dr.GetDecimal(0) == 576105)
                    locC = "UDP0007";
                else if (dr.GetDecimal(0) == 576113)
                    locC = "SAN1034";
                else if (dr.GetDecimal(0) == 576114)
                    locC = "KAL4712";
                else if (dr.GetDecimal(0) == 576117)
                    locC = "AMB9372";
                else
                    locC = "RAN0420";
                dr.Close();
                con.Close();
                LocCode.Value = locC;
            }
        }

        public void Assign(object sender, EventArgs e)
        {
            DialogResult d;
            string y = "Yes";
            string n = "No";
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["UEB1ConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SELECT * FROM tblCustomer WHERE RR_No = '" + RRno.Value + "' AND Customer_No = " + CustNo.Value + " AND Sub_Division = '"+SubDivi+"' AND Location_Code = '"+LocCode.Value+"' AND Meter_Code = '"+MtrCode.Value+"' AND Deleted = '"+n+"'",con);
            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.HasRows)
            {
                d = MessageBox.Show("Credentials already registered");
                if(d == DialogResult.OK)
                {
                    dr.Close();
                    con.Close();
                    Response.Redirect("AdminAssignDetails.aspx");
                }
            }
            dr.Close();

            cmd = new SqlCommand("UPDATE tblCustomer SET RR_No = '" + RRno.Value + "' , Customer_No = " + CustNo.Value + ", Sub_Division = '" + SubDivi.Value + "', Location_Code = '" + LocCode.Value + "', Meter_Code = '" + MtrCode.Value + "' WHERE Mobile_No = '" + MobileNo.Value + "'",con);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("UPDATE tblCustomer SET Assign = '" + y + "' WHERE Mobile_No = '" + MobileNo.Value + "'", con);
            cmd.ExecuteNonQuery();

            cmd = new SqlCommand("SELECT Name, Email FROM tblCustomer WHERE Mobile_No = '" + MobileNo.Value + "'", con);
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
                    message.Subject = "Connection Details Assigned";
                    message.IsBodyHtml = true; //to make message body as html  
                    message.Body = "Hello " + dr.GetString(0) + "\n We have assigned you your new connection details. Which are as follows \nCustomer No: " + CustNo.Value +
                 "RR No: " + RRno.Value +
                 "Location Code: " + LocCode.Value +
                 "Meter Code: " + MtrCode.Value +
                 "Sub Division: " + SubDivi.Value +
                 "You can check it In the status tab by entering your registered mobile no. , also make sure that you sign up To avail the various online services available.";
                    smtp.Port = 587;
                    smtp.Host = "smtp.gmail.com"; //for gmail host  
                    smtp.EnableSsl = true;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential("udupieb@gmail.com", "iapiktlufgoykpzi");
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Send(message);
                    MessageBox.Show("Mail sent successfully");
                    Session["AssignMobNo"] = "";
                }
                catch (Exception) 
                {
                    MessageBox.Show("Mail could not be sent");
                }
                dr.Close();
                con.Close();
                d = MessageBox.Show("Record inserted successfully");
                if (d == DialogResult.OK)
                    Response.Redirect("AdminHome.aspx");
            }
        }
    }
}