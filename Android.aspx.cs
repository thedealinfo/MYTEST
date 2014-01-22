using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using System.Collections.Generic;
using System.Xml.Linq;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data.SqlClient.SqlGen;
using System.Data.SqlClient;

namespace TheDealPortal
{
    public partial class Android : System.Web.UI.Page
    {
        private void ValidationScript(string str)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('" + str + "');", true);
        }
        public string mySrc;
        public int MSGID;
        public Int16 SetTime = 5;
        string str;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["RecFilePath"] != null)

            mySrc = Session["RecFilePath"].ToString();


            List<string[]> arrays = new List<string[]>();
            var primeArray = mySrc.Split('|');
            for (int i = 0; i < primeArray.Length; i++)
            {
                if (i == 1)
                {
                    break;
                }
                else
                {
                    string First = primeArray[i];

                    if (First != "")
                    {
                        string[] separators = { "/" };
                        string[] RecordingFilepath = First.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string FinalFile = RecordingFilepath.Last();
                        HiddenField1.Value = FinalFile;
                        string[] separators1 = { "http://local.thedeal1.info/local/TempFile/Recording/" };
                        string[] Role = First.Split(separators1, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole = Role.Last();
                        string[] separators2 = { "/" };
                        string[] RoleDefine = Newrole.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole1 = RoleDefine.First();
                        HiddenField2.Value = Newrole1;
                    }
                    else
                    {
                        break;
                    }
                    string Second = primeArray[i + 1];
                    if (Second != "")
                    {
                        string[] separators = { "/" };
                        string[] RecordingFilepath = Second.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string FinalFile = RecordingFilepath.Last();
                        HiddenField3.Value = FinalFile;
                        string[] separators1 = { "http://local.thedeal1.info/local/TempFile/Recording/" };
                        string[] Role = Second.Split(separators1, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole = Role.Last();
                        string[] separators2 = { "/" };
                        string[] RoleDefine = Newrole.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole1 = RoleDefine.First();
                        HiddenField4.Value = Newrole1;
                    }
                    else
                    {
                        break;
                    }
                    string Third = primeArray[i + 2];
                    if (Third != "")
                    {
                        string[] separators = { "/" };
                        string[] RecordingFilepath = Third.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string FinalFile = RecordingFilepath.Last();
                        HiddenField5.Value = FinalFile;
                        string[] separators1 = { "http://local.thedeal1.info/local/TempFile/Recording/" };
                        string[] Role = Third.Split(separators1, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole = Role.Last();
                        string[] separators2 = { "/" };
                        string[] RoleDefine = Newrole.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole1 = RoleDefine.First();
                        HiddenField6.Value = Newrole1;
                    }
                    else
                    {
                        break;
                    }
                    string Fourth = primeArray[i + 3];
                    if (Fourth != "")
                    {
                        string[] separators = { "/" };
                        string[] RecordingFilepath = Fourth.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string FinalFile = RecordingFilepath.Last();
                        HiddenField7.Value = FinalFile;
                        string[] separators1 = { "http://local.thedeal1.info/local/TempFile/Recording/" };
                        string[] Role = Fourth.Split(separators1, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole = Role.Last();
                        string[] separators2 = { "/" };
                        string[] RoleDefine = Newrole.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole1 = RoleDefine.First();
                        HiddenField8.Value = Newrole1;
                    }
                    else
                    {
                        break;
                    }
                    string Fifth = primeArray[i + 4];
                    if (Fifth != "")
                    {
                        string[] separators = { "/" };
                        string[] RecordingFilepath = Fifth.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string FinalFile = RecordingFilepath.Last();
                        HiddenField9.Value = FinalFile;
                        string[] separators1 = { "http://local.thedeal1.info/local/TempFile/Recording/" };
                        string[] Role = Fifth.Split(separators1, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole = Role.Last();
                        string[] separators2 = { "/" };
                        string[] RoleDefine = Newrole.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole1 = RoleDefine.First();
                        HiddenField10.Value = Newrole1;
                    }
                    else
                    {
                        break;
                    }
                    string Six = primeArray[i + 5];
                    if (Six != "")
                    {
                        string[] separators = { "/" };
                        string[] RecordingFilepath = Six.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string FinalFile = RecordingFilepath.Last();

                        HiddenField11.Value = FinalFile;
                        string[] separators1 = { "http://local.thedeal1.info/local/TempFile/Recording/" };
                        string[] Role = Six.Split(separators1, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole = Role.Last();
                        string[] separators2 = { "/" };
                        string[] RoleDefine = Newrole.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole1 = RoleDefine.First();
                        HiddenField12.Value = Newrole1;
                    }
                    else
                    {
                        break;
                    }
                    string Seven = primeArray[i + 6];
                    if (Seven != "")
                    {
                        string[] separators = { "/" };
                        string[] RecordingFilepath = Seven.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string FinalFile = RecordingFilepath.Last();
                        HiddenField13.Value = FinalFile;
                        string[] separators1 = { "http://local.thedeal1.info/local/TempFile/Recording/" };
                        string[] Role = Seven.Split(separators1, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole = Role.Last();
                        string[] separators2 = { "/" };
                        string[] RoleDefine = Newrole.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole1 = RoleDefine.First();
                        HiddenField14.Value = Newrole1;
                    }
                    else
                    {
                        break;
                    }

                    string Eight = primeArray[i + 7];
                    if (Eight != "")
                    {
                        string[] separators = { "/" };
                        string[] RecordingFilepath = Eight.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string FinalFile = RecordingFilepath.Last();
                        HiddenField15.Value = FinalFile;
                        string[] separators1 = { "http://local.thedeal1.info/local/TempFile/Recording/" };
                        string[] Role = Eight.Split(separators1, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole = Role.Last();
                        string[] separators2 = { "/" };
                        string[] RoleDefine = Newrole.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole1 = RoleDefine.First();
                        HiddenField16.Value = Newrole1;
                    }
                    else
                    {
                        break;
                    }
                    string Nine = primeArray[i + 8];
                    if (Nine != "")
                    {
                        string[] separators = { "/" };
                        string[] RecordingFilepath = Nine.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string FinalFile = RecordingFilepath.Last();
                        HiddenField17.Value = FinalFile;
                        string[] separators1 = { "http://local.thedeal1.info/local/TempFile/Recording/" };
                        string[] Role = Nine.Split(separators1, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole = Role.Last();
                        string[] separators2 = { "/" };
                        string[] RoleDefine = Newrole.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole1 = RoleDefine.First();
                        HiddenField18.Value = Newrole1;
                    }
                    else
                    {
                        break;
                    }
                    string Ten = primeArray[i + 9];
                    if (Ten != "")
                    {
                        string[] separators = { "/" };
                        string[] RecordingFilepath = Ten.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string FinalFile = RecordingFilepath.Last();
                        HiddenField19.Value = FinalFile;
                        string[] separators1 = { "http://local.thedeal1.info/local/TempFile/Recording/" };
                        string[] Role = Ten.Split(separators1, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole = Role.Last();
                        string[] separators2 = { "/" };
                        string[] RoleDefine = Newrole.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string Newrole1 = RoleDefine.First();
                        HiddenField20.Value = Newrole1;
                    }
                    else
                    {
                        break;
                    }
                    string Eleven = primeArray[i + 10];
                    
                }
                Page.RegisterStartupScript("OnLoad", "<script>playFlashVideo(" + mySrc + ");</script>");
            }



            //string[] separators = { "/" };
            //string[] RecordingFilepath = mySrc.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            //string FinalFile = RecordingFilepath.Last();
            //HiddenField1.Value = FinalFile;
            //string[] separators1 = { "http://local.thedeal1.info/local/TempFile/Recording/" };
            //string[] Role = mySrc.Split(separators1, StringSplitOptions.RemoveEmptyEntries);
            //string Newrole = Role.Last();
            //string[] separators2 = { "/" };
            //string[] RoleDefine = Newrole.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            //string Newrole1 = RoleDefine.First();
            //HiddenField2.Value = Newrole1;
            
        }
    }
}