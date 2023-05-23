using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using Serco;
using System.IO;
using TagitEncrypt;
using System.Net.Mail;
using System.Net;

public partial class Login : System.Web.UI.Page
{
    public static int uid;
    public static bool acticeid = false;
    public static string vid;
    public static string uname;
    public static string pword;
    public static string Type;
    public static string TypeDesc;
    public static string Approve;
    public static string Location;
    public static string Custodian;
    CompanyBL objcomp = new CompanyBL();
    public static string respword;
    public static string resuname; public static string path = "";
    protected void Page_Load(object sender, EventArgs e)
    {

        path = Server.MapPath("~/ErrorLog.txt");
        txtusername.Focus();
    }

    protected void BtnClose_Click(object sender, EventArgs e)
    {
        try
        {
            lblError.Text = "";
            txtUser.Text = "";
            txtEmailId.Text = "";
            DetailsPopup.Hide();
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Login.aspx", "BtnClose_Click", path);
        }
    }
    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        try
        {
            if (txtUser.Text.Trim() == "")
            {
                lblError.Text = "Enter User Name.";
                // divsuccess.Visible = true;
                DetailsPopup.Show();
                txtUser.Focus();


                return;
            }
            if (txtEmailId.Text.Trim() == "")
            {
                lblError.Text = "Enter Email Id.";
                DetailsPopup.Show();
                txtEmailId.Focus();
                return;
            }
            if (!CheckUserExists())
            {
                lblError.Text = "Invalid User name / Email ID.";
                DetailsPopup.Show();
                txtUser.Focus();
                return;
            }
            else
            {
                sendpasswordmail(txtEmailId.Text);
                lblError.Text = "Password sent successfully";
                lblError.ForeColor = System.Drawing.Color.Green;
                DetailsPopup.Show();
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Login.aspx", "btnConfirm_Click", path);
        }
    }
    private bool CheckUserExists()
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            con.Open();

            string query = "select U.USER_ID,PASSWORD,U.USER_NAME,U.Type,U.Approve from [dbo].[TBL_USERMST] U where USER_NAME='" + txtUser.Text.ToString() + "' and Email='" + txtEmailId.Text.ToString() + "' and Status =1";
            SqlCommand cmd = new SqlCommand(query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds == null || ds.Tables == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                return false;
            }
            else
            {
                respword = ds.Tables[0].Rows[0]["PASSWORD"].ToString();
                resuname = ds.Tables[0].Rows[0]["USER_NAME"].ToString();
                return true;
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Login.aspx", "CheckUserExists", path);
            return false;
        }
    }
    private void sendpasswordmail(string email)
    {
        try
        {
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            con.Open();

            SqlCommand select = new SqlCommand();
            select.CommandText = "select * from TagitEmailConfig where Application='tagit'";
            select.CommandType = CommandType.Text;
            select.Connection = con;
            SqlDataReader dr1 = select.ExecuteReader();
            EncryptManager em = new EncryptManager();
            if (dr1.Read())
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                string s = dr1.GetString(1);
                message.From = new MailAddress(dr1.GetString(1));
                message.To.Add(new MailAddress(email));
                message.Subject = "Your Password";
                message.IsBodyHtml = true;
                message.Body = "<p>Dear Customer!<br/><br/>The password for username " + resuname + " is " + respword + "<br/><br/>** Please note that this email is auto-generated from Tagit AMS application.**<br/><br/><b>Thank you</b><br/><b>Tagit AMS</b> </p>";
                smtp.Port = 587;
                smtp.Host = dr1.GetString(3);
                smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential(dr1.GetString(1), em.Decode(dr1.GetString(2)));
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Login.aspx", "sendpasswordmail", path);
        }
    }
    // Check User is Valid or not
    protected void btnlogin_Click(object sender, EventArgs e)
    {
        try
        {
            //objcomp.Insertlogmaster("Login:Data Verify Started");
            string un, pass;
            // string dttest = DateTime.Now.ToString("dd-MMM-yyyy");
            un = Convert.ToString(txtusername.Text);
            pass = Convert.ToString(txtpassword.Text);
            SqlConnection con = new SqlConnection();
            con.ConnectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;
            con.Open();
            string query = "select U.USER_ID,PASSWORD,U.USER_NAME,U.Type,UT.Type TypeDesc,U.Approve from [dbo].[TBL_USERMST] U inner join UserType UT on U.TYPE = UT.id where PASSWORD='" + pass + "' and USER_NAME='" + un + "' and Status =1";
            SqlCommand cmd = new SqlCommand(query, con);

            SqlDataReader dr = cmd.ExecuteReader();


            while (dr.Read())
            {
                vid = dr["USER_ID"].ToString();
                uname = dr["USER_NAME"].ToString();
                pword = dr["PASSWORD"].ToString();
                Type = dr["Type"].ToString();
                TypeDesc = dr["TypeDesc"].ToString();
                Approve = dr["Approve"].ToString();
            }
            Session["userid"] = vid;
            Session["UserName"] = uname;
            Session["UserType"] = Type;
            Session["TypeDesc"] = TypeDesc;
            Session["Approve"] = Approve;
            Session["locnamenewdata"] = ""; Session["typenewdata"] = "";
            dr.Close();
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            da.Fill(ds);

            if (ds == null || ds.Tables == null || ds.Tables.Count < 1 || ds.Tables[0].Rows.Count < 1)
            {
                objcomp.Insertlogmaster("Login:Data Verify failed Invalid user");

                lblloginerror.Text = "Invalid User name / Password.";
                return;

            }
            else
            {
                objcomp.Insertlogmaster("Login:Data Verify success,user Found");
                DataTable dt = ds.Tables[0];
                if (Convert.ToInt32(dt.Rows[0]["USER_ID"]) < 1)
                {
                    lblloginerror.Text = "Invalid User name / Password.";
                    Session["memberid"] = null;
                    return;
                }
                else if (un != uname || pass != pword)
                {
                    lblloginerror.Text = "Invalid User name / Password.";
                }
                else
                {
                    objcomp.Insertlogmaster("Login:Page Redirected to Home Page");
                    Response.Redirect("Home.aspx");

                }
            }
        }
        catch (Exception ex)
        {
            Logging.WriteErrorLog(ex.Message.ToString(), ex.StackTrace.ToString(), "Login.aspx", "btnlogin_Click", path);
        }
    }

}