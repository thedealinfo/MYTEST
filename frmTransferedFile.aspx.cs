using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using TheDealPortal.BAL;
using System.Data;
using System.IO;
using TheDealPortal.DAL;
using ICSharpCode.SharpZipLib.Zip;
using System.Threading;
using System.Text;
using System.Web.UI.HtmlControls;
using System.Collections;
using System.Text.RegularExpressions;
using System.Data.SqlClient.SqlGen;
using System.Data.SqlClient;
using System.Net;
using System.Net.Sockets;

namespace TheDealPortal
{
    public partial class frmFileList : System.Web.UI.Page
    {
        public String URL;
        public DataRow oDR;
        public int count1, c;
        public ArrayList arlist = new ArrayList();
        HtmlTextWriter writer;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (Request.Cookies["IsAdmin"].Value == "true")
                    tblCriteria.Visible = true;
                else
                    tblCriteria.Visible = false;

                BAL.clsFileMaster ofm = new BAL.clsFileMaster();
                string MasterCheck = ofm.DeleteAcessData();
                if (MasterCheck == "Success")
                    FetchEmergancyAcess();
                else
                    FetchData();
                ((HtmlAnchor)Master.FindControl("menuTransferedFile")).Attributes.Add("class", "current");
            }
        }

        private void ValidationScript(string Msg)
        {
            ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), Guid.NewGuid().ToString(), "alert('" + Msg + "');", true);
        }
        //Fetch All criteria recording display
        private void FetchData()
        {
            try
            {
                List<T_FileMaster> oFM = new List<T_FileMaster>();
                clsFileMaster ofile = new clsFileMaster();
                oFM = ofile.GetTransferFiles();
                DataTable oDT = new DataTable();
                List<string> lstfolders = new List<string>();
                List<string> lstChannels = new List<string>();
                txtUserName.Text = txtUserName.Text.ToUpper();
                txtGroupName.Text = txtGroupName.Text.ToUpper();

                if (Request.Cookies["IsAdmin"].Value == "true")
                {
                    #region Admin Section

                    if (chkAdmin.Checked) lstfolders.Add("Admin");
                    if (chkAnnounce.Checked) lstfolders.Add("Announce");
                    if (chkClient.Checked) lstfolders.Add("Client");
                    if (chkSpecial.Checked) lstfolders.Add("Special");
                    if (lstfolders.Count == 0)
                    {
                        lstfolders.Add("Admin");
                        lstfolders.Add("Announce");
                        lstfolders.Add("Client");
                        lstfolders.Add("Special");
                    }

                    if (txtChannelNo.Text.Length > 0)
                    {
                        foreach (string ch in txtChannelNo.Text.Split(','))
                            lstChannels.Add(ch);
                    }

                    if (lstfolders.Count == 1)
                        dgvReport.Columns[11].Visible = true;
                    else
                        dgvReport.Columns[11].Visible = true;

                    if (txtUserName.Text.Length > 0 && txtUserName.Text.Split(',').Length == 1)
                        dgvReport.Columns[2].Visible = false;
                    else
                        dgvReport.Columns[2].Visible = true;

                    if (txtMobile.Text.Length > 0 && txtMobile.Text.Split(',').Length == 1)
                        dgvReport.Columns[3].Visible = false;
                    else
                        dgvReport.Columns[3].Visible = false;

                    if (txtChannelNo.Text.Length > 0 && txtChannelNo.Text.Split(',').Length == 1)
                        dgvReport.Columns[4].Visible = false;
                    else
                        dgvReport.Columns[4].Visible = false;

                    TimeSpan gtime1 = TimeSpan.Parse("00:00:01"), ltime1 = TimeSpan.Parse("00:00:01");
                    if (txtTimeFrom.Text.Length > 0 && txtTimeTo.Text.Length > 0)
                    {
                        gtime1 = TimeSpan.Parse(txtTimeTo.Text);
                        ltime1 = TimeSpan.Parse(txtTimeFrom.Text);
                    }

                    TimeSpan gtime2 = TimeSpan.Parse("00:00:01"), ltime2 = TimeSpan.Parse("00:00:01");
                    if (txtDuration.Text.Length > 0 && txtDurationTo.Text.Length > 0)
                    {
                        gtime2 = TimeSpan.Parse(txtDurationTo.Text);
                        ltime2 = TimeSpan.Parse(txtDuration.Text);
                    }

                    var result = from file in oFM
                                 where lstfolders.Contains(file.FolderName) &&
                                    (txtDuration.Text.Length > 0 && txtDurationTo.Text.Length > 0 ? ((TimeSpan.Parse(file.Duration.Substring(0, 8)) >= ltime2) && (TimeSpan.Parse(file.Duration.Substring(0, 8)) <= gtime2)) : file.Duration == file.Duration) &&

                                     ((txtDateFrom.Text.Length > 0 && txtDateTo.Text.Length > 0) ? (DateTime.Parse(txtDateFrom.Text) <= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))) &&
                                     (DateTime.Parse(txtDateTo.Text) >= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))) : file.DateTime.Value.ToString("yyyy-MM-dd") == file.DateTime.Value.ToString("yyyy-MM-dd")) &&

                                     ((txtTimeFrom.Text.Length > 0 && txtTimeTo.Text.Length > 0) ? (DateTime.Parse(txtTimeFrom.Text) <= DateTime.Parse(file.DateTime.Value.ToString("HH:mm:ss"))) &&
                                     (DateTime.Parse(txtTimeTo.Text) >= DateTime.Parse(file.DateTime.Value.ToString("HH:mm:ss"))) : file.DateTime.Value.ToString("HH:mm:ss") == file.DateTime.Value.ToString("HH:mm:ss")) &&

                                     (txtMobile.Text.Length > 0 ? txtMobile.Text.Contains(file.PhoneNo) : file.PhoneNo == file.PhoneNo) &&
                                     (txtChannelNo.Text.Length > 0 ? lstChannels.Contains(file.ClientCh.ToString()) : file.ClientCh.ToString() == file.ClientCh.ToString()) &&
                                     (txtUserName.Text.Length > 0 ? txtUserName.Text.Contains(file.UserName) : file.UserName == file.UserName) &&
                                     (txtGroupName.Text.Length > 0 ? txtGroupName.Text.Contains(file.GroupName) : file.GroupName == file.GroupName)

                                 orderby file.DateTime ascending
                                 select new
                                 {
                                     ID = file.ID,
                                     StartRecording = file.DateTime,
                                     FileName = file.FileName,
                                     RecordingChannel = file.ClientCh,
                                     TalkWithChannel = file.TalkwithCh,
                                     FolderName = file.FolderName,
                                     Duration = file.Duration.Substring(0, 8),
                                     TalkWithRole = file.TalkwithRole,
                                     UserName = file.UserName,
                                     PhoneNo = file.PhoneNo,
                                     Isplay = file.IsPlay,
                                     IsMarked = file.IsMarked
                                 };

                    oDT = Helper.IEnumerableToDataTable(result.ToList());

                    #endregion
                }
                else if (Request.Cookies["Criteria"].Value != null && Request.Cookies["Criteria"].Value != "")
                {
                    #region Client Section

                    string[] criteria = Request.Cookies["Criteria"].Value.Split('@');

                    if (criteria[0].Split('#')[1].Contains("Admin")) lstfolders.Add("Admin");
                    if (criteria[0].Split('#')[1].Contains("Announce")) lstfolders.Add("Announce");
                    if (criteria[0].Split('#')[1].Contains("Client")) lstfolders.Add("Client");
                    if (criteria[0].Split('#')[1].Contains("Special")) lstfolders.Add("Special");
                    if (lstfolders.Count == 0)
                    {
                        lstfolders.Add("Admin");
                        lstfolders.Add("Announce");
                        lstfolders.Add("Client");
                        lstfolders.Add("Special");
                    }

                    if (criteria[8].Split('#')[1].Length > 0)
                    {
                        foreach (string ch in criteria[8].Split('#')[1].Split(','))
                            lstChannels.Add(ch);
                    }

                    TimeSpan gtime1 = TimeSpan.Parse("00:00:01"), ltime1 = TimeSpan.Parse("00:00:01");
                    if (criteria[5].Split('#')[1].Length > 0 && criteria[6].Split('#')[1].Length > 0)
                    {
                        gtime1 = TimeSpan.Parse(criteria[6].Split('#')[1]);
                        ltime1 = TimeSpan.Parse(criteria[5].Split('#')[1]);
                    }

                    TimeSpan gtime2 = TimeSpan.Parse("00:00:01"), ltime2 = TimeSpan.Parse("00:00:01");
                    if (criteria[1].Split('#')[1].Length > 0 && criteria[2].Split('#')[1].Length > 0)
                    {
                        gtime2 = TimeSpan.Parse(criteria[2].Split('#')[1]);
                        ltime2 = TimeSpan.Parse(criteria[1].Split('#')[1]);
                    }


                    var result = from file in oFM
                                 where lstfolders.Contains(file.FolderName) &&
                                     (criteria[1].Split('#')[1].Length > 0 && criteria[2].Split('#')[1].Length > 0 ? ((TimeSpan.Parse(file.Duration.Substring(0, 8)) >= ltime2) && (TimeSpan.Parse(file.Duration.Substring(0, 8)) <= gtime2)) : file.Duration == file.Duration) &&
                                     

                                     ((criteria[3].Split('#')[1].ToString().Length > 0 && criteria[4].Split('#')[1].ToString().Length > 0) ? (DateTime.Parse(criteria[3].Split('#')[1].ToString()) <= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))) &&
                                     (DateTime.Parse(criteria[4].Split('#')[1].ToString()) >= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))) : file.DateTime.Value.ToString("yyyy-MM-dd") == file.DateTime.Value.ToString("yyyy-MM-dd")) &&

                                     ((criteria[5].Split('#')[1].ToString().Length > 0 && criteria[6].Split('#')[1].ToString().Length > 0) ? (DateTime.Parse(criteria[5].Split('#')[1].ToString()) <= DateTime.Parse(file.DateTime.Value.ToString("HH:mm:ss"))) &&
                                     (DateTime.Parse(criteria[6].Split('#')[1].ToString()) >= DateTime.Parse(file.DateTime.Value.ToString("HH:mm:ss"))) : file.DateTime.Value.ToString("HH:mm:ss") == file.DateTime.Value.ToString("HH:mm:ss")) &&

                                     (criteria[7].Split('#')[1].Length > 0 ? criteria[7].Split('#')[1].Contains(file.PhoneNo) : file.PhoneNo == file.PhoneNo) &&
                                     (criteria[8].Split('#')[1].Length > 0 ? lstChannels.Contains(file.ClientCh.ToString()) : file.ClientCh == file.ClientCh) &&

                                     (criteria[9].Split('#')[1].Length > 0 ? criteria[9].Split('#')[1].Contains(file.UserName) : file.UserName == file.UserName) &&
                                     file.DeleteFlag==false &&
                                     (criteria[10].Split('#')[1].Length > 0 ? criteria[10].Split('#')[1].Contains(file.GroupName) : file.GroupName == file.GroupName)

                                     
                                 orderby file.DateTime ascending
                                 select new
                                 {
                                     ID = file.ID,
                                     StartRecording = file.DateTime,
                                     FileName = file.FileName,
                                     RecordingChannel = file.ClientCh,
                                     TalkWithChannel = file.TalkwithCh,
                                     FolderName = file.FolderName,
                                     Duration = file.Duration.Substring(0, 8),
                                     TalkWithRole = file.TalkwithRole,
                                     UserName = file.UserName,
                                     PhoneNo = file.PhoneNo,
                                     Isplay = file.IsPlay,
                                     IsMarked = file.IsMarked,
                                     GroupName=file.GroupName
                                 };


                    oDT = Helper.IEnumerableToDataTable(result.ToList());

                    #endregion
                }

                dgvReport.DataSource = oDT;
                dgvReport.DataBind();
            }
            catch (Exception ex)
            {
                ValidationScript(ex.Message);
            }
        }

        private void FetchEmergancyAcess()
        {
            try
            {
                List<T_FileMaster> oFM = new List<T_FileMaster>();
                clsFileMaster ofile = new clsFileMaster();
                oFM = ofile.GetTransferFiles();
                DataTable oDT = new DataTable();
                List<string> lstfolders = new List<string>();
                List<string> lstChannels = new List<string>();
                txtUserName.Text = txtUserName.Text.ToUpper();
                txtGroupName.Text = txtGroupName.Text.ToUpper();


                 if (Request.Cookies["Criteria"].Value != null && Request.Cookies["Criteria"].Value != "")
                {
                    #region Client Section

                    string[] criteria = Request.Cookies["Criteria"].Value.Split('@');

                    if (criteria[0].Split('#')[1].Contains("Admin")) lstfolders.Add("Admin");
                    if (criteria[0].Split('#')[1].Contains("Announce")) lstfolders.Add("Announce");
                    if (criteria[0].Split('#')[1].Contains("Client")) lstfolders.Add("Client");
                    if (criteria[0].Split('#')[1].Contains("Special")) lstfolders.Add("Special");
                    if (lstfolders.Count == 0)
                    {
                        lstfolders.Add("Admin");
                        lstfolders.Add("Announce");
                        lstfolders.Add("Client");
                        lstfolders.Add("Special");
                    }

                    if (criteria[8].Split('#')[1].Length > 0)
                    {
                        foreach (string ch in criteria[8].Split('#')[1].Split(','))
                            lstChannels.Add(ch);
                    }

                    TimeSpan gtime1 = TimeSpan.Parse("00:00:01"), ltime1 = TimeSpan.Parse("00:00:01");
                    if (criteria[5].Split('#')[1].Length > 0 && criteria[6].Split('#')[1].Length > 0)
                    {
                        gtime1 = TimeSpan.Parse(criteria[6].Split('#')[1]);
                        ltime1 = TimeSpan.Parse(criteria[5].Split('#')[1]);
                    }

                    TimeSpan gtime2 = TimeSpan.Parse("00:00:01"), ltime2 = TimeSpan.Parse("00:00:01");
                    if (criteria[1].Split('#')[1].Length > 0 && criteria[2].Split('#')[1].Length > 0)
                    {
                        gtime2 = TimeSpan.Parse(criteria[2].Split('#')[1]);
                        ltime2 = TimeSpan.Parse(criteria[1].Split('#')[1]);
                    }


                    var result = from file in oFM
                                 where lstfolders.Contains(file.FolderName) &&
                                     (criteria[1].Split('#')[1].Length > 0 && criteria[2].Split('#')[1].Length > 0 ? ((TimeSpan.Parse(file.Duration.Substring(0, 8)) >= ltime2) && (TimeSpan.Parse(file.Duration.Substring(0, 8)) <= gtime2)) : file.Duration == file.Duration) &&


                                     ((criteria[3].Split('#')[1].ToString().Length > 0 && criteria[4].Split('#')[1].ToString().Length > 0) ? (DateTime.Parse(criteria[3].Split('#')[1].ToString()) <= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))) &&
                                     (DateTime.Parse(criteria[4].Split('#')[1].ToString()) >= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))) : file.DateTime.Value.ToString("yyyy-MM-dd") == file.DateTime.Value.ToString("yyyy-MM-dd")) &&

                                     ((criteria[5].Split('#')[1].ToString().Length > 0 && criteria[6].Split('#')[1].ToString().Length > 0) ? (DateTime.Parse(criteria[5].Split('#')[1].ToString()) <= DateTime.Parse(file.DateTime.Value.ToString("HH:mm:ss"))) &&
                                     (DateTime.Parse(criteria[6].Split('#')[1].ToString()) >= DateTime.Parse(file.DateTime.Value.ToString("HH:mm:ss"))) : file.DateTime.Value.ToString("HH:mm:ss") == file.DateTime.Value.ToString("HH:mm:ss")) &&

                                     (criteria[7].Split('#')[1].Length > 0 ? criteria[7].Split('#')[1].Contains(file.PhoneNo) : file.PhoneNo == file.PhoneNo) &&
                                     (criteria[8].Split('#')[1].Length > 0 ? lstChannels.Contains(file.ClientCh.ToString()) : file.ClientCh == file.ClientCh) &&

                                     (criteria[9].Split('#')[1].Length > 0 ? criteria[9].Split('#')[1].Contains(file.UserName) : file.UserName == file.UserName) &&
                                     file.DeleteFlag == false &&
                                     (criteria[10].Split('#')[1].Length > 0 ? criteria[10].Split('#')[1].Contains(file.GroupName) : file.GroupName == file.GroupName)


                                 orderby file.DateTime ascending
                                 select new
                                 {
                                     ID = file.ID,
                                     StartRecording = file.DateTime,
                                     FileName = file.FileName,
                                     RecordingChannel = file.ClientCh,
                                     TalkWithChannel = file.TalkwithCh,
                                     FolderName = file.FolderName,
                                     Duration = file.Duration.Substring(0, 8),
                                     TalkWithRole = file.TalkwithRole,
                                     UserName = file.UserName,
                                     PhoneNo = file.PhoneNo,
                                     Isplay = file.IsPlay,
                                     IsMarked = file.IsMarked,
                                     GroupName = file.GroupName
                                 };


                    oDT = Helper.IEnumerableToDataTable(result.ToList());

                        oDT = Helper.IEnumerableToDataTable(result.ToList());
                    if (oDT.Rows.Count > 0)
                    {
                        foreach (var D in result)
                        {
                            int Id = D.ID;
                            string Filepath = D.FileName;
                            string FolderName = D.FolderName;
                            string SucessorNot = ofile.setAllDelete(Id, true);
                            if (SucessorNot != "Sucess")
                            {
                                goto Fail;
                            }

                        }
                    Fail:
                        FetchData();
                    }
                    else
                    {
                        ValidationScript("Not Any Record found");
                    }
                }
               
                    #endregion
                }

            catch (Exception ex)
            {
                ValidationScript(ex.Message);
            }
        }
        #region Datagridview



        protected void dgvReport_Sorting(object sender, GridViewSortEventArgs e)
        {
            ViewState["SortExp"] = e.SortExpression;
            if (ViewState[ViewState["SortExp"].ToString()] == null)
                ViewState.Add(ViewState["SortExp"].ToString(), "T");

            FetchData();
         

        }
    private string ConvertSortDirectionToSql(SortDirection sortDirection)
    {
      string newSortDirection = String.Empty;

      switch (sortDirection)
      {
        case SortDirection.Ascending:
          newSortDirection = "ASC";
          break;

        case SortDirection.Descending:
          newSortDirection = "DESC";
          break;
      }
      return newSortDirection;
    }
        #endregion

        #region Download

        protected void chkAll_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow oDR in dgvReport.Rows)
                {
                    CheckBox ochk = (CheckBox)oDR.FindControl("chkItem");
                    ochk.Checked = ((CheckBox)sender).Checked;
                }
            }
            catch (Exception)
            {
            }
        }
        protected void chkAllDelete_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow oDR in dgvReport.Rows)
                {
                    CheckBox ochk = (CheckBox)oDR.FindControl("chkItem2");
                    ochk.Checked = ((CheckBox)sender).Checked;
                }
            }
            catch (Exception)
            {
            }
        }
        protected void chkAllUnplay_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (GridViewRow oDR in dgvReport.Rows)
                {
                    clsFileMaster ofile = new clsFileMaster();
                    string ID = ((LinkButton)oDR.FindControl("lnkPlay")).CommandArgument;
                    int NewId = Convert.ToInt16(ID);
                    string Filepath = ofile.SelectUnPlayedFile(NewId);

                    if (Filepath == "Sucess")
                    {
                        CheckBox ochk = (CheckBox)oDR.FindControl("chkItem1");
                        ochk.Checked = ((CheckBox)sender).Checked;

                        
                    }
                }
            }
            catch (Exception)
            {
            }
        }
       
        private void ZipAllFiles()
        {
            try
            {

                byte[] buffer = new byte[4096];

                // the path on the server where the temp file will be created!
              //  var tempFileName = @"C:\HostingSpaces\thedeali\thedeal1.info\wwwroot\bin\TempFiles\" + Guid.NewGuid().ToString() + ".zip";
               //  var tempFileName = @"D:\Hosting\9516783\html\local\bin\TempFiles\" + Guid.NewGuid().ToString() + ".zip";
                //var test1 = @"D:\\Hosting\\9516783\\html\\local\\";
               // var tempFileName = System.Configuration.ConfigurationManager.AppSettings["PhysicalPath"] + Guid.NewGuid().ToString() + ".zip";
                var test1 = Server.MapPath("~");
                string test = Guid.NewGuid().ToString() + ".zip";
                var tempFileName = test1 + test;
              
               // var tempFileName = Server.MapPath("~/local/TempFile/" + tesWriteFilet);
                var zipOutputStream = new ZipOutputStream(File.Create(tempFileName));
                //var filePath = String.Empty;
                //var fileName = String.Empty;
                var readBytes = 0;

                foreach (GridViewRow row in dgvReport.Rows)
                {
                    var isChecked = (row.FindControl("chkItem") as CheckBox).Checked;
                    if (!isChecked) continue;

                    LinkButton ohl = ((LinkButton)row.FindControl("lnkPlay"));
                    var filePath = ohl.Attributes["Path"];

                    if (filePath != null && filePath.Length > 0)
                    {
                        var zipEntry = new ZipEntry(Path.GetFileName(filePath));

                        zipOutputStream.PutNextEntry(zipEntry);

                        using (var fs = File.OpenRead(filePath))
                        {
                            do
                            {
                                readBytes = fs.Read(buffer, 0, buffer.Length);
                                zipOutputStream.Write(buffer, 0, readBytes);

                            } while (readBytes > 0);
                        }
                    }
                }

                if (zipOutputStream.Length == 0)
                {
                    ValidationScript("Please select at least one file!");
                    return;
                }

                zipOutputStream.Finish();
                zipOutputStream.Close();

                Response.ContentType = "application/x-zip-compressed";
                Response.AppendHeader("Content-Disposition", "attachment; filename=YourFile.zip");
                Response.WriteFile(tempFileName);

                Response.Flush();
                Response.Close();

                // delete the temp file 
                if (File.Exists(tempFileName))
                    File.Delete(tempFileName);
            }
            catch (Exception)
            {

            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            int Count = 0; int selected = 0;
            for (int i = 0; i < dgvReport.Rows.Count; i++)
            {
                var isChecked = (dgvReport.Rows[i].FindControl("chkItem") as CheckBox).Checked;
                if (isChecked)
                {
                    if (Count == 0) selected = i;
                    Count++;
                }
            }

            if (Count == 1)
            {
                LinkButton olbl = (LinkButton)dgvReport.Rows[selected].FindControl("lnkPlay");
                string URL = System.Configuration.ConfigurationManager.AppSettings["PhysicalPath"] + dgvReport.Rows[selected].Cells[10].Text + "//" + Path.GetFileName(olbl.Attributes["FileName"]);
                Response.Clear();
                Response.ContentType = "audio/wav";
                Response.AddHeader("content-disposition", "attachment;filename=" + Path.GetFileName(olbl.Attributes["FileName"]));
                Response.WriteFile(URL);
                Response.End();
            }
            else if (Count > 1)
                ZipAllFiles();
        }

        private void fileDownload(string fileName, string fileUrl)
        {
            try
            {
                Page.Response.Clear();
                bool success = ResponseFile(Page.Request, Page.Response, fileName, fileUrl, 1024000);
                if (!success)
                    Response.Write("Downloading Error!");

                Page.Response.End();
            }
            catch (Exception)
            {
            }
        }

        public static bool ResponseFile(HttpRequest _Request, HttpResponse _Response, string _fileName, string _fullPath, long _speed)
        {
            try
            {
                FileStream myFile = new FileStream(_fullPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                BinaryReader br = new BinaryReader(myFile);
                try
                {
                    _Response.AddHeader("Accept-Ranges", "bytes");
                    _Response.Buffer = false;
                    long fileLength = myFile.Length;
                    long startBytes = 0;

                    int pack = 10240; //10K bytes
                    int sleep = (int)Math.Floor((double)(1000 * pack / _speed)) + 1;
                    if (_Request.Headers["Range"] != null)
                    {
                        _Response.StatusCode = 206;
                        string[] range = _Request.Headers["Range"].Split(new char[] { '=', '-' });
                        startBytes = Convert.ToInt64(range[1]);
                    }
                    _Response.AddHeader("Content-Length", (fileLength - startBytes).ToString());
                    if (startBytes != 0)
                    {
                        _Response.AddHeader("Content-Range", string.Format(" bytes {0}-{1}/{2}", startBytes, fileLength - 1, fileLength));
                    }
                    _Response.AddHeader("Connection", "Keep-Alive");
                    _Response.ContentType = "application/octet-stream";
                    _Response.AddHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(_fileName, System.Text.Encoding.UTF8));

                    br.BaseStream.Seek(startBytes, SeekOrigin.Begin);
                    int maxCount = (int)Math.Floor((double)((fileLength - startBytes) / pack)) + 1;

                    for (int i = 0; i < maxCount; i++)
                    {
                        if (_Response.IsClientConnected)
                        {
                            _Response.BinaryWrite(br.ReadBytes(pack));
                            Thread.Sleep(sleep);
                        }
                        else
                        {
                            i = maxCount;
                        }
                    }
                }
                catch
                {
                    return false;
                }
                finally
                {
                    br.Close();
                    myFile.Close();
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FetchData();
        }



        FtpWebRequest ftpRequest1 = null;
        FtpWebResponse response1 = null;


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtDateFrom.Text != "" && txtDateTo.Text != "")
                {
                    clsFileMaster ofile = new clsFileMaster();
                    // (DateTime.Parse(txtDateTo.Text) >= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))
                    DateTime DateFrom = DateTime.Parse(txtDateFrom.Text);
                    DateTime DateTo = DateTime.Parse(txtDateTo.Text);
                    string TimeFrom = (txtTimeFrom.Text);
                    string TimeTo = (txtTimeTo.Text);

                    var result = ofile.DeleteFiles(DateFrom, DateTo, TimeFrom, TimeTo);
                    // MessageBox.Show("Result of operation->" + result);
                }
                else
                {
           
                foreach (GridViewRow row in dgvReport.Rows)
                {
                    // FtpWebRequest ftpRequest1 = null;
                    // FtpWebResponse response1 = null;
                    
                    clsFileMaster ofile = new clsFileMaster();
                    var isChecked = (row.FindControl("chkItem") as CheckBox).Checked;
                    if (!isChecked) continue;

                    string ID = ((LinkButton)row.FindControl("lnkPlay")).CommandArgument;
                    int NewId = Convert.ToInt16(ID);
                    string Filepath = ofile.SelectFile(NewId);
                    string FolderName = ofile.SelectFolder(NewId);

                    string[] separators = { "\\" };
                    string[] RecordingFilepath = Filepath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                    string FinalFile = RecordingFilepath.Last();

                

                    string str = func(Filepath);
                    string Test = "http://local.thedeal1.info/local";
                    string NNN = Test + str;
                    //string Set = Server.MapPath("")+str;
                    //string[] separators1 = { "\\" };
                    //str = Set.Replace('\\', '/');

                    string NewServer = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
                    string NewUserName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
                    string NewPassword = System.Configuration.ConfigurationManager.AppSettings["Password"];
                    string NewServerPath = System.Configuration.ConfigurationManager.AppSettings["FTPPath2"];
                  
                    string Testpath = ofile.CheckFileExistinginPath(FinalFile, NewServerPath + "/" + FolderName, NewServer, NewUserName, NewPassword);

                    if (Testpath == "Sucess")
                    {
                        if (!string.IsNullOrEmpty(Testpath))
                        {
                            //File.Delete(str);
                          //  ValidationScript("Sucess");
                            FtpWebRequest reqFTP;
                            reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://local.thedeal1.info/local/TempFile/Recording/" + FolderName + "/" + FinalFile));
                            reqFTP.UseBinary = true;
                            reqFTP.Credentials = new NetworkCredential("pthedeal", "Psamsung@0");
                            reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;
                            System.Threading.Thread.Sleep(100);
                            WebResponse response = reqFTP.GetResponse();
                            System.Threading.Thread.Sleep(200);
                            response.Close();
                          
                        
                        }

                    }
                    string result = ofile.DeleteRecord(int.Parse(ID), Filepath, FolderName);
                }

            }


                //if (txtDateFrom.Text != "" && txtDateTo.Text != "")
                //{
                //    clsFileMaster ofile = new clsFileMaster();
                //    // (DateTime.Parse(txtDateTo.Text) >= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))
                //    DateTime DateFrom = DateTime.Parse(txtDateFrom.Text);
                //    DateTime DateTo = DateTime.Parse(txtDateTo.Text);
                //    string TimeFrom = (txtTimeFrom.Text);
                //    string TimeTo = (txtTimeTo.Text);

                 //   var result = ofile.DeleteFiles(DateFrom, DateTo, TimeFrom, TimeTo);
                //    // MessageBox.Show("Result of operation->" + result);
                //}
                //else
                //{
                //    // Found:
                //    foreach (GridViewRow oDR in dgvReport.Rows)
                //    {
                //        FtpWebRequest ftpRequest1 = null;
                //        FtpWebResponse response1 = null;

                //        clsFileMaster ofile = new clsFileMaster();
                //        CheckBox ochk = (CheckBox)oDR.FindControl("chkItem");
                //        if (ochk.Checked)
                //        {
                //            string actualFolder = string.Empty;
                //            string ID = ((LinkButton)oDR.FindControl("lnkPlay")).CommandArgument;
                //            int NewId = Convert.ToInt16(ID);
                //            string Filepath = ofile.SelectFile(NewId);
                //            string FolderName = ofile.SelectFolder(NewId);
                //            string[] separators = { "\\" };
                //            string[] RecordingFilepath = Filepath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                //            string FinalFile = RecordingFilepath.Last();
                //           string path = System.Configuration.ConfigurationManager.AppSettings["PhysicalPath"] + FinalFile;
                //            actualFolder = Server.MapPath("../local/TempFile/Recording/Client/");
                //            string imagePath = actualFolder + FolderName;
                //           // string imagePath = Server.MapPath("~/local/TempFile/Recording/Client/" + FinalFile);
                //            //string Path = ((LinkButton)oDR.FindControl("lnkPlay")).Attributes["Path"];


                //            //LinkButton olbl = (LinkButton)dgvReport.Rows[selected].FindControl("lnkPlay");
                //            // string URL = System.Configuration.ConfigurationManager.AppSettings["PhysicalPath"] + "Client" + "//" + FinalFile;
                //            string str = func(Filepath);
                //            string Test = "http://local.thedeal1.info/local";
                //            string NNN = Test + str;

                //            if (ID != null && ID.Length > 0)
                //            {


                //                string result = "Success";
                //                if (result == "Success")
                //                {
                  //                  if (File.Exists(NNN))
                //                    {


                //                        //if (!string.IsNullOrEmpty(NNN))
                //                        //{
                //                        //    ftpRequest1 = (FtpWebRequest)WebRequest.Create("ftp://local.thedeal1.info/local/TempFile/Recording/" + FolderName + "/" + FinalFile);
                //                        //    ftpRequest1.Credentials = new NetworkCredential("thedealc", "TheDeal@2013");
                //                        //    ftpRequest1.Method = WebRequestMethods.Ftp.DeleteFile;
                //                        //    response1 = (FtpWebResponse)ftpRequest1.GetResponse();
                //                        //    System.Threading.Thread.Sleep(3000);
                //                        //    response1.Close();
                //                            string result1 = ofile.DeleteRecordNew(int.Parse(ID));

                //                        //}



                //                        //FileInfo TheFile = new FileInfo(NNN); 
                //                        //ValidationScript("File not delete found");
                //                        ////string display = NNN +"DeLEte";
                //                        ////ValidationScript(display);
                //                        File.Delete(imagePath);


                //                        return;
                //                    }
                //                    else
                //                    {
                //                        if (!string.IsNullOrEmpty(NNN))
                //                        {
                //                            ftpRequest1 = (FtpWebRequest)WebRequest.Create("ftp://local.thedeal1.info/local/TempFile/Recording/" + FolderName + "/" + FinalFile);
                //                            ftpRequest1.Credentials = new NetworkCredential("thedealc", "TheDeal@2013");
                //                            ftpRequest1.Method = WebRequestMethods.Ftp.DeleteFile;
                //                            response1 = (FtpWebResponse)ftpRequest1.GetResponse();
                //                            System.Threading.Thread.Sleep(3000);
                //                            response1.Close();
                //                            string result1 = ofile.DeleteRecordNew(int.Parse(ID));

                //                        }

                //                    }

                //                }
                //                // goto Found;

                //            }
                //        }
                //    }
                //}
              //  Response.AppendHeader("Refresh", "1; URL=http://local.thedeal1.info/local/frmTransferedFile.aspx");
               // Response.Redirect("frmTransferedFile.aspx", false);
                FetchData();
            }
            catch (Exception)
            {
           
                Response.AppendHeader("Refresh", "5; URL=http://local.thedeal1.info/local/frmLogin.aspx");
                Response.Redirect("frmLogin.aspx", false);
            }
        }

        protected void Timer1_Tick(object sender, EventArgs e)
        {
            FetchData();
        }

        protected void chkAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
         
            if (chkAutoRefresh.Checked == true)
                Timer1.Enabled = true;
            else
                Timer1.Enabled = false;
        }

        protected void chkAdmin_CheckedChanged(object sender, EventArgs e)
        {
            FetchData();
        }

        protected void chkClient_CheckedChanged(object sender, EventArgs e)
        {
            FetchData();
        }

        protected void chkAnnounce_CheckedChanged(object sender, EventArgs e)
        {
            FetchData();
        }

        protected void chkSpecial_CheckedChanged(object sender, EventArgs e)
        {
            FetchData();
        }

        protected void dgvReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {

                if (e.CommandName == "Play")
                {
                    if (((LinkButton)(e.CommandSource)).Font.Bold)
                    {
                        ((LinkButton)(e.CommandSource)).Font.Bold = false;
                        ((LinkButton)(e.CommandSource)).Font.Italic = false;
                        ((LinkButton)(e.CommandSource)).Text = "Play";
                        ((LinkButton)(e.CommandSource)).ForeColor = System.Drawing.Color.Green;

                        clsFileMaster obl = new clsFileMaster();

                        //int index = Convert.ToInt32(e.CommandArgument);
                        //GridViewRow gvrow = dgvReport.Rows[index];
                        //LinkButton olbl = (LinkButton)dgvReport.Rows[gvrow.RowIndex].FindControl("lnkPlay");
                        //string ID = ((LinkButton)olbl.FindControl("lnkPlay")).CommandArgument;



                       // GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                       // int RowIndex = gvr.RowIndex;
                       // LinkButton olbl = (LinkButton)dgvReport.Rows[RowIndex].FindControl("lnkPlay");


                       // string ID = ((LinkButton)olbl.FindControl("lnkPlay")).CommandArgument;

                       // int NewId = Convert.ToInt16(ID);
                       // string FolderName = obl.SelectFolder(NewId);
                       // string Filepath = obl.SelectFile(NewId);

                       // string[] separators = { "\\" };
                       // string[] RecordingFilepath = Filepath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                       // string FinalFile = RecordingFilepath.Last();
                       // string str = func(Filepath);
                       // string Test = "http://local.thedeal1.info/local";
                       // string NNN = Test + str;
                       // Session.Clear();
                       // Session.Add("RecFilePath", NNN);
                       
                       //// olbl.Attributes.Add("OnClick", "javascript:window.open('Player.aspx?path=" + NNN + " ','PlayFile','width=320, height=200,menubar=no, scrollbars=no, resizable=no')");
                       // Response.Write("<script type='text/javascript'> window.open('Player.aspx?path=" + NNN + " ','PlayFile','width=320, height=200,menubar=no, scrollbars=no, resizable=no'); </script>");
                       
                        obl.setPlayed(int.Parse(e.CommandArgument.ToString()), false);

                        
                    }
                    else
                    {
                        ((LinkButton)(e.CommandSource)).Font.Bold = true;
                        ((LinkButton)(e.CommandSource)).Font.Italic = true;
                        ((LinkButton)(e.CommandSource)).ForeColor = System.Drawing.Color.Black;
                        ((LinkButton)(e.CommandSource)).Text = "Played";

                        clsFileMaster obl = new clsFileMaster();
                        


                        GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        int RowIndex = gvr.RowIndex;
                        LinkButton olbl = (LinkButton)dgvReport.Rows[RowIndex].FindControl("lnkPlay");


                        string ID = ((LinkButton)olbl.FindControl("lnkPlay")).CommandArgument;

                        int NewId = Convert.ToInt32(ID);
                        string FolderName = obl.SelectFolder(NewId);
                        string Filepath = obl.SelectFile(NewId);

                        string[] separators = { "\\" };
                        string[] RecordingFilepath = Filepath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        string FinalFile = RecordingFilepath.Last();
                        string str = func(Filepath);
                        string Test = "http://local.thedeal1.info/local";
                        string NNN = Test + str;
                        Session.Clear();
                        Session.Add("RecFilePath", NNN);
                        obl.setPlayed(int.Parse(e.CommandArgument.ToString()), true);
                        Response.Redirect("Android.aspx?path=" + NNN + "", true);
                        obl.setPlayed(int.Parse(e.CommandArgument.ToString()), true);
                      //  Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('Player.aspx?path=" + NNN + " ','PlayFile','width=320, height=200,menubar=no, scrollbars=no, resizable=no');", true);


                        //((LinkButton)(e.CommandSource)).Font.Bold = true;
                        //((LinkButton)(e.CommandSource)).Font.Italic = true;
                        //((LinkButton)(e.CommandSource)).ForeColor = System.Drawing.Color.Black;
                        //((LinkButton)(e.CommandSource)).Text = "Played";

                        //clsFileMaster obl = new clsFileMaster();
                        //obl.setPlayed(int.Parse(e.CommandArgument.ToString()), true);

                        //GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                        //int RowIndex = gvr.RowIndex;

                        //LinkButton olbl = (LinkButton)dgvReport.Rows[RowIndex].FindControl("lnkPlay");
                        //string ID = ((LinkButton)olbl.FindControl("lnkPlay")).CommandArgument;

                        //int NewId = Convert.ToInt16(ID);
                        //string FolderName = obl.SelectFolder(NewId);
                        //string Filepath = obl.SelectFile(NewId);

                        //string[] separators = { "\\" };
                        //string[] RecordingFilepath = Filepath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                        //string FinalFile = RecordingFilepath.Last();
                        //string str = func(Filepath);
                        //string URL = System.Configuration.ConfigurationManager.AppSettings["PhysicalPath"] + FolderName + "//" + FinalFile;


                        //FtpWebRequest reqFTP;
                        //ftpRequest1 = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://182.50.130.34//local/TempFile/Recording/" + FolderName + "/" + FinalFile));

                        //ftpRequest1.UseBinary = true;
                        //ftpRequest1.Credentials = new NetworkCredential("pthedeal", "Psamsung@0");

                        //Response.AppendHeader("Content-Disposition", "attachment;filename=\"" + FinalFile + "\"");
                       
                        //ftpRequest1.Method = WebRequestMethods.Ftp.DownloadFile;
                        //System.Threading.Thread.Sleep(100);
                        //WebResponse response = ftpRequest1.GetResponse();
                        //System.Threading.Thread.Sleep(200);
                        //response.Close();

                        //WebClient req = new WebClient();
                        //CredentialCache mycache = new CredentialCache();
                        //mycache.Add(new Uri(URL), "Basic", new NetworkCredential("pthedeal", "Psamsung@0"));
                        //req.Credentials = mycache;
                        //HttpResponse response = HttpContext.Current.Response;
                        //response.Clear();
                        //response.ClearContent();
                        //response.ClearHeaders();
                        //response.Buffer = true;
                        //response.AddHeader("Content-Disposition", "attachment;filename=\"" + FinalFile + "\"");
                        //Response.AddHeader("Content-Type", "audio/mpeg");
                        //byte[] data = req.DownloadData(FinalFile);
                        //response.BinaryWrite(data);
                        //response.End();
                        //return; 


                        //Response.Clear();
                        //Response.AddHeader("content-disposition", "attachment; filename=" + FinalFile);
                        //Response.WriteFile(URL);
                        //Response.ContentType = "";
                        //Response.End();


                        //string URL = System.Configuration.ConfigurationManager.AppSettings["PhysicalPath"] + FolderName + "//" + FinalFile;
                        //Response.Clear();
                        //Response.ContentType = "audio/mp3";
                        //Response.WriteFile(URL);
                        //Response.End();
                    }
                }
                else if (e.CommandName == "Mark")
                {
                    if (((LinkButton)(e.CommandSource)).Font.Bold)
                    {
                        ((LinkButton)(e.CommandSource)).Font.Bold = false;
                        ((LinkButton)(e.CommandSource)).Font.Italic = false;
                        ((LinkButton)(e.CommandSource)).ForeColor = System.Drawing.Color.Gray;

                        clsFileMaster obl = new clsFileMaster();
                        obl.setMarked(int.Parse(e.CommandArgument.ToString()), false);
                    }
                    else
                    {
                        ((LinkButton)(e.CommandSource)).Font.Bold = true;
                        ((LinkButton)(e.CommandSource)).Font.Italic = true;
                        ((LinkButton)(e.CommandSource)).ForeColor = System.Drawing.Color.Black;

                        clsFileMaster obl = new clsFileMaster();
                        obl.setMarked(int.Parse(e.CommandArgument.ToString()), true);
                    }
                }
            }
            catch (Exception ex)
            {
                ValidationScript(ex.Message);
            }
        }
        private void WriteBytesToResponse(byte[] sourceBytes)
        {
            using (var sourceStream = new MemoryStream(sourceBytes, false))
            {
                sourceStream.WriteTo(Response.OutputStream);
            }
        }
        public string func(string str)
        {
            string finalpath = "";
            // str = (string)Eval("FileName");
            Regex re = new Regex(@"\\Recording");
            Match m = re.Match(str);
            str = str.Remove(0, m.Index);
            string[] separators = { "\\AfterMerge" };
            string[] FileN = str.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string FirstPath = FileN.First();
            string LastPath = FileN.Last();
            if (FirstPath != LastPath)
            {
                finalpath = FirstPath + LastPath;
            }
            else
            {
                finalpath = FirstPath;
            }
            str = finalpath.Replace('\\', '/');
            str = str.Insert(0, "/TempFile");
            return str;
        }
        public string func1(string str)
        {
            //  str = (string)Eval("FileName");
            Regex re = new Regex(@"\\Recording");
            Match m = re.Match(str);
            str = str.Remove(0, m.Index);
            str = str.Replace('\\', '/');
            str = str.Insert(0, "/TempFile");
            return str;
        }
        protected void dgvReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    DataRow oDR = ((DataRowView)(e.Row.DataItem)).Row;

                    LinkButton olPhone = new LinkButton();
                    olPhone = (LinkButton)e.Row.FindControl("lnkPhone");
                    if (oDR.ItemArray[11].ToString() == "True")
                    {
                        olPhone.Font.Bold = true;
                        olPhone.Font.Italic = true;
                        olPhone.ForeColor = System.Drawing.Color.Black;
                    }

                    LinkButton olbl = (LinkButton)e.Row.FindControl("lnkPlay");
                    olbl.Attributes.Add("FileName", oDR.ItemArray[2].ToString());

                    string PhysicalPath = System.Configuration.ConfigurationManager.AppSettings["PhysicalPath"] + oDR.ItemArray[5].ToString() + "//" + System.IO.Path.GetFileName(oDR.ItemArray[2].ToString());

                    if (System.IO.File.Exists(PhysicalPath))
                    {
                        if (oDR.ItemArray[10].ToString() == "True")
                        {
                            olbl.Attributes.Add("Path", PhysicalPath);
                            olbl.Font.Bold = true;
                            olbl.Font.Italic = true;
                            olbl.ForeColor = System.Drawing.Color.Black;
                            olbl.Text = "Played";
                        }
                        else
                        {
                            olbl.ForeColor = System.Drawing.Color.Green;
                            olbl.Text = "Play";
                        }

                        olbl.Attributes.Add("Path", PhysicalPath);
                    }
                    else
                        olbl.Visible = false;

                }
                catch (Exception ex)
                {
                    ValidationScript(ex.Message);
                }
            }
        }

        protected void dgvReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvReport.AllowPaging = true;
            dgvReport.PageIndex = e.NewPageIndex;
            FetchData();
        }

        protected void btnClientDelete_Click(object sender, EventArgs e)
        {
         
         //try
         //  {
            foreach (GridViewRow row in dgvReport.Rows)
            {
                FtpWebRequest ftpRequest1 = null;
                FtpWebResponse response1 = null;
                
                clsFileMaster ofile = new clsFileMaster();
                var isChecked = (row.FindControl("chkItem") as CheckBox).Checked;
                if (!isChecked) continue;

                string ID = ((LinkButton)row.FindControl("lnkPlay")).CommandArgument;
                int NewId = Convert.ToInt16(ID);
                string Filepath = ofile.SelectFile(NewId);
                string FolderName = ofile.SelectFolder(NewId);

                string[] separators = { "\\" };
                string[] RecordingFilepath = Filepath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string FinalFile = RecordingFilepath.Last();

                

                string str = func(Filepath);
                string Test = "http://local.thedeal1.info/local";
                string NNN = Test + str;
                //string Set = Server.MapPath("") + str;
                //string[] separators1 = { "\\" };
                //str = Set.Replace('\\', '/');

                //string NewServer = System.Configuration.ConfigurationManager.AppSettings["ServerName"];
                //string NewUserName = System.Configuration.ConfigurationManager.AppSettings["UserName"];
                //string NewPassword = System.Configuration.ConfigurationManager.AppSettings["Password"];
                //string NewServerPath = System.Configuration.ConfigurationManager.AppSettings["FTPPath2"];

                //string Testpath = ofile.CheckFileExistinginPath(FinalFile, NewServerPath + "/" + FolderName, NewServer, NewUserName, NewPassword);
                string Testpath = "Sucess";
                if (Testpath == "Sucess")
                {
                   
                    ValidationScript("Sucess");
                    if (!string.IsNullOrEmpty(Testpath))
                    {
                        File.Delete(NNN);
                      
                        FtpWebRequest reqFTP;
                        ftpRequest1 = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://182.50.130.34//local/TempFile/Recording/" + FolderName + "/" + FinalFile));
                        
                        ftpRequest1.UseBinary = true;
                        ftpRequest1.Credentials = new NetworkCredential("pthedeal", "Psamsung@0");

                        Response.AppendHeader("Refresh", "5; URL=http://local.thedeal1.info/local/Default.aspx");
                        Response.Redirect("Default.aspx", true);
                        ftpRequest1.Method = WebRequestMethods.Ftp.DeleteFile;
                        System.Threading.Thread.Sleep(100);
                        WebResponse response = ftpRequest1.GetResponse();
                        System.Threading.Thread.Sleep(200);
                        response.Close();
                        

                    }

                }
                string result = ofile.DeleteRecord(int.Parse(ID), Filepath, FolderName);
                Response.AppendHeader("Refresh", "5; URL=http://local.thedeal1.info/local/Default.aspx");
                Response.Redirect("Default.aspx", true);
            }
           
            FetchData();
           //}
         //catch (Exception)
         //{

         //    Response.AppendHeader("Refresh", "5; URL=http://local.thedeal1.info/local/frmLogin.aspx");
         //    Response.Redirect("frmLogin.aspx", false);
         //}

        }

        protected void btnplaylist_Click(object sender, EventArgs e)
        {
            
            foreach (GridViewRow row in dgvReport.Rows)
            {
                clsFileMaster ofile = new clsFileMaster();

                var isChecked = (row.FindControl("chkItem") as CheckBox).Checked;

                if (isChecked == true)
                {
                    if (!isChecked) continue;
                }
                else
                {
                    var isChecked1 = (row.FindControl("chkItem1") as CheckBox).Checked;
                    if (!isChecked1) continue;
                }

               


               
                string ID = ((LinkButton)row.FindControl("lnkPlay")).CommandArgument;
                int NewId = Convert.ToInt32(ID);
                string Filepath = ofile.SelectFile(NewId);
                string FolderName = ofile.SelectFolder(NewId);
                ofile.setPlayed(NewId,true);
                string[] separators = { "\\" };
                string[] RecordingFilepath = Filepath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string FinalFile = RecordingFilepath.Last();


               
                string str = func(Filepath);
                string Test = "http://local.thedeal1.info/local";
                string NNN = Test + str;
                arlist.Add(NNN);

            }

            StringBuilder sb = new StringBuilder();
            foreach (object obj in arlist) sb.Append(obj + "|");

            Session.Add("RecFilePath", sb);
            Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('Mp3MultiPlayer.aspx?path=" + sb + " ','PlayFile','width=820, height=180,menubar=no, scrollbars=no, resizable=yes');", true);
           // Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('Mp3MultiPlayer.aspx?path=" + sb + " ','PlayFile','width=1000, height=1000,menubar=no, scrollbars=no, resizable=no');", true);



        }

        protected void btnAddtoDelete_Click(object sender, EventArgs e)
        {

            foreach (GridViewRow row in dgvReport.Rows)
            {
               
                clsFileMaster ofile = new clsFileMaster();
                var isChecked = (row.FindControl("chkItem2") as CheckBox).Checked;
                if (!isChecked) continue;
                clsFileMaster obl = new clsFileMaster();
                string ID = ((LinkButton)row.FindControl("lnkPlay")).CommandArgument;
                int NewId = Convert.ToInt16(ID);
                obl.setDelete(NewId, true);

            }
            FetchData();
        }

        protected void btnAutoProcess_Click(object sender, EventArgs e)
        {
            FetchAutoSelectData();
        }

        private void FetchAutoSelectData()
        {
            try
            {
                if (Request.Cookies["IsAdmin"].Value == "true")
                {
                    List<T_FileMaster> oFM = new List<T_FileMaster>();
                    clsFileMaster ofile = new clsFileMaster();
                    oFM = ofile.GetTransferAutoProcessFiles();
                    DataTable oDT = new DataTable();
                    List<string> lstfolders = new List<string>();
                    List<string> lstChannels = new List<string>();
                    txtUserName.Text = txtUserName.Text.ToUpper();
                    txtGroupName.Text = txtGroupName.Text.ToUpper();

                    #region Admin Section

                    if (chkAdmin.Checked) lstfolders.Add("Admin");
                    if (chkAnnounce.Checked) lstfolders.Add("Announce");
                    if (chkClient.Checked) lstfolders.Add("Client");
                    if (chkSpecial.Checked) lstfolders.Add("Special");
                    if (lstfolders.Count == 0)
                    {
                        lstfolders.Add("Admin");
                        lstfolders.Add("Announce");
                        lstfolders.Add("Client");
                        lstfolders.Add("Special");
                    }

                    if (txtChannelNo.Text.Length > 0)
                    {
                        foreach (string ch in txtChannelNo.Text.Split(','))
                            lstChannels.Add(ch);
                    }

                    if (lstfolders.Count == 1)
                        dgvReport.Columns[11].Visible = true;
                    else
                        dgvReport.Columns[11].Visible = true;

                    if (txtUserName.Text.Length > 0 && txtUserName.Text.Split(',').Length == 1)
                        dgvReport.Columns[2].Visible = false;
                    else
                        dgvReport.Columns[2].Visible = true;

                    if (txtMobile.Text.Length > 0 && txtMobile.Text.Split(',').Length == 1)
                        dgvReport.Columns[3].Visible = false;
                    else
                        dgvReport.Columns[3].Visible = false;

                    if (txtChannelNo.Text.Length > 0 && txtChannelNo.Text.Split(',').Length == 1)
                        dgvReport.Columns[4].Visible = false;
                    else
                        dgvReport.Columns[4].Visible = false;

                    TimeSpan gtime1 = TimeSpan.Parse("00:00:01"), ltime1 = TimeSpan.Parse("00:00:01");
                    if (txtTimeFrom.Text.Length > 0 && txtTimeTo.Text.Length > 0)
                    {
                        gtime1 = TimeSpan.Parse(txtTimeTo.Text);
                        ltime1 = TimeSpan.Parse(txtTimeFrom.Text);
                    }

                    TimeSpan gtime2 = TimeSpan.Parse("00:00:01"), ltime2 = TimeSpan.Parse("00:00:01");
                    if (txtDuration.Text.Length > 0 && txtDurationTo.Text.Length > 0)
                    {
                        gtime2 = TimeSpan.Parse(txtDurationTo.Text);
                        ltime2 = TimeSpan.Parse(txtDuration.Text);
                    }

                    var result = from file in oFM.Take(10)
                                 where lstfolders.Contains(file.FolderName) &&
                                     (txtDuration.Text.Length > 0 && txtDurationTo.Text.Length > 0 ? ((TimeSpan.Parse(file.Duration.Substring(0, 8)) >= ltime2) && (TimeSpan.Parse(file.Duration.Substring(0, 8)) <= gtime2)) : file.Duration == file.Duration) &&

                                     ((txtDateFrom.Text.Length > 0 && txtDateTo.Text.Length > 0) ? (DateTime.Parse(txtDateFrom.Text) <= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))) &&
                                     (DateTime.Parse(txtDateTo.Text) >= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))) : file.DateTime.Value.ToString("yyyy-MM-dd") == file.DateTime.Value.ToString("yyyy-MM-dd")) &&

                                     ((txtTimeFrom.Text.Length > 0 && txtTimeTo.Text.Length > 0) ? (DateTime.Parse(txtTimeFrom.Text) <= DateTime.Parse(file.DateTime.Value.ToString("HH:mm:ss"))) &&
                                     (DateTime.Parse(txtTimeTo.Text) >= DateTime.Parse(file.DateTime.Value.ToString("HH:mm:ss"))) : file.DateTime.Value.ToString("HH:mm:ss") == file.DateTime.Value.ToString("HH:mm:ss")) &&

                                     (txtMobile.Text.Length > 0 ? txtMobile.Text.Contains(file.PhoneNo) : file.PhoneNo == file.PhoneNo) &&
                                     (txtChannelNo.Text.Length > 0 ? lstChannels.Contains(file.ClientCh.ToString()) : file.ClientCh.ToString() == file.ClientCh.ToString()) &&
                                     (txtUserName.Text.Length > 0 ? txtUserName.Text.Contains(file.UserName) : file.UserName == file.UserName) &&
                                      (file.IsPlay == null || file.IsPlay== false) &&
                                     (txtGroupName.Text.Length > 0 ? txtGroupName.Text.Contains(file.GroupName) : file.GroupName == file.GroupName)

                                 orderby file.DateTime ascending
                                 select new
                                 {
                                     ID = file.ID,
                                     StartRecording = file.DateTime,
                                     FileName = file.FileName,
                                     RecordingChannel = file.ClientCh,
                                     TalkWithChannel = file.TalkwithCh,
                                     FolderName = file.FolderName,
                                     Duration = file.Duration.Substring(0, 8),
                                     TalkWithRole = file.TalkwithRole,
                                     UserName = file.UserName,
                                     PhoneNo = file.PhoneNo,
                                     Isplay = file.IsPlay,
                                     IsMarked = file.IsMarked,
                                     GroupName = file.GroupName
                                    
                                 };

                    oDT = Helper.IEnumerableToDataTable(result.ToList());

                    if (oDT.Rows.Count > 0)
                    {
                        foreach (var D in result.ToList())
                        {
                            int Id = D.ID;
                            string Filepath = D.FileName;
                            string FolderName = D.FolderName;
                            ofile.setPlayed(Id, true);
                            string[] separators = { "\\" };
                            string[] RecordingFilepath = Filepath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            string FinalFile = RecordingFilepath.Last();
                            string str = func(Filepath);
                            string Test = "http://local.thedeal1.info/local";
                            string NNN = Test + str;
                            arlist.Add(NNN);
                        }

                        StringBuilder sb = new StringBuilder();
                        foreach (object obj in arlist) sb.Append(obj + "|");
                        oDT = null;
                        Session.Add("RecFilePath", sb);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('Mp3MultiPlayer.aspx?path=" + sb + " ','PlayFile','width=820, height=180,menubar=no, scrollbars=no, resizable=yes');", true);
                    }
                    #endregion
                    // }
                    
                }
                else if (Request.Cookies["Criteria"].Value != null && Request.Cookies["Criteria"].Value != "")
                {
                    List<T_FileMaster> oFM1 = new List<T_FileMaster>();
                    clsFileMaster ofile = new clsFileMaster();
                    oFM1 = ofile.GetTransferAutoProcessClientFiles();
                    DataTable oDT1 = new DataTable();
                    List<string> lstfolders = new List<string>();
                    List<string> lstChannels = new List<string>();
                    txtUserName.Text = txtUserName.Text.ToUpper();
                    txtGroupName.Text = txtGroupName.Text.ToUpper();
                    #region Client Section

                    string[] criteria = Request.Cookies["Criteria"].Value.Split('@');

                    if (criteria[0].Split('#')[1].Contains("Admin")) lstfolders.Add("Admin");
                    if (criteria[0].Split('#')[1].Contains("Announce")) lstfolders.Add("Announce");
                    if (criteria[0].Split('#')[1].Contains("Client")) lstfolders.Add("Client");
                    if (criteria[0].Split('#')[1].Contains("Special")) lstfolders.Add("Special");
                    if (lstfolders.Count == 0)
                    {
                        lstfolders.Add("Admin");
                        lstfolders.Add("Announce");
                        lstfolders.Add("Client");
                        lstfolders.Add("Special");
                    }

                    if (criteria[8].Split('#')[1].Length > 0)
                    {
                        foreach (string ch in criteria[8].Split('#')[1].Split(','))
                            lstChannels.Add(ch);
                    }

                    TimeSpan gtime1 = TimeSpan.Parse("00:00:01"), ltime1 = TimeSpan.Parse("00:00:01");
                    if (criteria[5].Split('#')[1].Length > 0 && criteria[6].Split('#')[1].Length > 0)
                    {
                        gtime1 = TimeSpan.Parse(criteria[6].Split('#')[1]);
                        ltime1 = TimeSpan.Parse(criteria[5].Split('#')[1]);
                    }

                    TimeSpan gtime2 = TimeSpan.Parse("00:00:01"), ltime2 = TimeSpan.Parse("00:00:01");
                    if (criteria[1].Split('#')[1].Length > 0 && criteria[2].Split('#')[1].Length > 0)
                    {
                        gtime2 = TimeSpan.Parse(criteria[2].Split('#')[1]);
                        ltime2 = TimeSpan.Parse(criteria[1].Split('#')[1]);
                    }

                    var result = (from file in oFM1
                                 where lstfolders.Contains(file.FolderName) &&
                                     (criteria[1].Split('#')[1].Length > 0 && criteria[2].Split('#')[1].Length > 0 ? ((TimeSpan.Parse(file.Duration.Substring(0, 8)) >= ltime2) && (TimeSpan.Parse(file.Duration.Substring(0, 8)) <= gtime2)) : file.Duration == file.Duration) &&

                                     ((criteria[3].Split('#')[1].ToString().Length > 0 && criteria[4].Split('#')[1].ToString().Length > 0) ? (DateTime.Parse(criteria[3].Split('#')[1].ToString()) <= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))) &&
                                     (DateTime.Parse(criteria[4].Split('#')[1].ToString()) >= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))) : file.DateTime.Value.ToString("yyyy-MM-dd") == file.DateTime.Value.ToString("yyyy-MM-dd")) &&

                                     ((criteria[5].Split('#')[1].ToString().Length > 0 && criteria[6].Split('#')[1].ToString().Length > 0) ? (DateTime.Parse(criteria[5].Split('#')[1].ToString()) <= DateTime.Parse(file.DateTime.Value.ToString("HH:mm:ss"))) &&
                                     (DateTime.Parse(criteria[6].Split('#')[1].ToString()) >= DateTime.Parse(file.DateTime.Value.ToString("HH:mm:ss"))) : file.DateTime.Value.ToString("HH:mm:ss") == file.DateTime.Value.ToString("HH:mm:ss")) &&

                                     (criteria[7].Split('#')[1].Length > 0 ? criteria[7].Split('#')[1].Contains(file.PhoneNo) : file.PhoneNo == file.PhoneNo) &&
                                     (criteria[8].Split('#')[1].Length > 0 ? lstChannels.Contains(file.ClientCh.ToString()) : file.ClientCh == file.ClientCh) &&

                                     (criteria[9].Split('#')[1].Length > 0 ? criteria[9].Split('#')[1].Contains(file.UserName) : file.UserName == file.UserName) &&
                                     file.DeleteFlag == false &&
                                     (file.IsPlay == null || file.IsPlay == false) &&
                                     (criteria[10].Split('#')[1].Length > 0 ? criteria[10].Split('#')[1].Contains(file.GroupName) : file.GroupName == file.GroupName)


                                 orderby file.DateTime ascending
                                 select new
                                 {
                                     ID = file.ID,
                                     StartRecording = file.DateTime,
                                     FileName = file.FileName,
                                     RecordingChannel = file.ClientCh,
                                     TalkWithChannel = file.TalkwithCh,
                                     FolderName = file.FolderName,
                                     Duration = file.Duration.Substring(0, 8),
                                     TalkWithRole = file.TalkwithRole,
                                     UserName = file.UserName,
                                     PhoneNo = file.PhoneNo,
                                     Isplay = file.IsPlay,
                                     IsMarked = file.IsMarked,
                                     GroupName = file.GroupName
                                 }).Take(10);


                    oDT1 = Helper.IEnumerableToDataTable(result.ToList());

                    if (oDT1.Rows.Count > 0)
                    {
                        foreach (var D in result)
                        {
                            int Id = D.ID;
                            string Filepath = D.FileName;
                            string FolderName = D.FolderName;
                            ofile.setPlayed(Id, true);
                            string[] separators = { "\\" };
                            string[] RecordingFilepath = Filepath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                            string FinalFile = RecordingFilepath.Last();
                            string str = func(Filepath);
                            string Test = "http://local.thedeal1.info/local";
                            string NNN = Test + str;
                            arlist.Add(NNN);
                        }

                        StringBuilder sb = new StringBuilder();
                        foreach (object obj in arlist) sb.Append(obj + "|");
                        oDT1 = null;
                        Session.Add("RecFilePath", sb);
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('Mp3MultiPlayer.aspx?path=" + sb + " ','PlayFile','width=820, height=180,menubar=no, scrollbars=no, resizable=yes');", true);
                    }
                    #endregion
                   
                }
               
                
            }
            catch (Exception ex)
            {
                ValidationScript(ex.Message);
            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
         
            foreach (GridViewRow row in dgvReport.Rows)
            {
                clsFileMaster ofile = new clsFileMaster();

                var isChecked = (row.FindControl("chkItem") as CheckBox).Checked;

                if (isChecked == true)
                {
                    if (!isChecked) continue;
                }
                else
                {
                    var isChecked1 = (row.FindControl("chkItem1") as CheckBox).Checked;
                    if (!isChecked1) continue;
                }





                string ID = ((LinkButton)row.FindControl("lnkPlay")).CommandArgument;
                int NewId = Convert.ToInt32(ID);
                string Filepath = ofile.SelectFile(NewId);
                string FolderName = ofile.SelectFolder(NewId);
                ofile.setPlayed(NewId, true);
                string[] separators = { "\\" };
                string[] RecordingFilepath = Filepath.Split(separators, StringSplitOptions.RemoveEmptyEntries);
                string FinalFile = RecordingFilepath.Last();



                string str = func(Filepath);
                string Test = "http://local.thedeal1.info/local";
                string NNN = Test + str;
                 arlist.Add(NNN);

                }

                StringBuilder sb = new StringBuilder();
                foreach (object obj in arlist) sb.Append(obj + "|");

                Session.Add("RecFilePath", sb);
                Response.Redirect("Android.aspx?path=" + sb + "", true);
                //Response.Redirect("Mp3MultiPlayer.aspx?path=" + sb + "", true);
                // Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('Mp3MultiPlayer.aspx?path=" + sb + " ','PlayFile','width=820, height=180,menubar=no, scrollbars=no, resizable=yes');", true);
           // }
        }

        protected void btnDateDelete_Click(object sender, EventArgs e)
        {
            
            if (Request.Cookies["Criteria"].Value != null && Request.Cookies["Criteria"].Value != "")
            {
                if (txtDelFromDate.Text != "" && txtDelToDate.Text != "")
                {
                    List<T_FileMaster> oFM1 = new List<T_FileMaster>();
                    clsFileMaster ofile = new clsFileMaster();
                    DateTime DateFrom = DateTime.Parse(txtDelFromDate.Text);
                    DateTime DateTo = DateTime.Parse(txtDelToDate.Text);

                    oFM1 = ofile.GetTransferAutoProcessClientFileDeleteFlagUpdate(DateFrom, DateTo);
                    DataTable oDT1 = new DataTable();
                    List<string> lstfolders = new List<string>();
                    List<string> lstChannels = new List<string>();
                    txtUserName.Text = txtUserName.Text.ToUpper();
                    txtGroupName.Text = txtGroupName.Text.ToUpper();



                    string[] criteria = Request.Cookies["Criteria"].Value.Split('@');

                    if (criteria[0].Split('#')[1].Contains("Admin")) lstfolders.Add("Admin");
                    if (criteria[0].Split('#')[1].Contains("Announce")) lstfolders.Add("Announce");
                    if (criteria[0].Split('#')[1].Contains("Client")) lstfolders.Add("Client");
                    if (criteria[0].Split('#')[1].Contains("Special")) lstfolders.Add("Special");
                    if (lstfolders.Count == 0)
                    {
                        lstfolders.Add("Admin");
                        lstfolders.Add("Announce");
                        lstfolders.Add("Client");
                        lstfolders.Add("Special");
                    }

                    if (criteria[8].Split('#')[1].Length > 0)
                    {
                        foreach (string ch in criteria[8].Split('#')[1].Split(','))
                            lstChannels.Add(ch);
                    }

                    TimeSpan gtime1 = TimeSpan.Parse("00:00:01"), ltime1 = TimeSpan.Parse("00:00:01");
                    if (criteria[5].Split('#')[1].Length > 0 && criteria[6].Split('#')[1].Length > 0)
                    {
                        gtime1 = TimeSpan.Parse(criteria[6].Split('#')[1]);
                        ltime1 = TimeSpan.Parse(criteria[5].Split('#')[1]);
                    }

                    TimeSpan gtime2 = TimeSpan.Parse("00:00:01"), ltime2 = TimeSpan.Parse("00:00:01");
                    if (criteria[1].Split('#')[1].Length > 0 && criteria[2].Split('#')[1].Length > 0)
                    {
                        gtime2 = TimeSpan.Parse(criteria[2].Split('#')[1]);
                        ltime2 = TimeSpan.Parse(criteria[1].Split('#')[1]);
                    }

                    var result = (from file in oFM1
                                  where lstfolders.Contains(file.FolderName) &&
                                      (criteria[1].Split('#')[1].Length > 0 && criteria[2].Split('#')[1].Length > 0 ? ((TimeSpan.Parse(file.Duration.Substring(0, 8)) >= ltime2) && (TimeSpan.Parse(file.Duration.Substring(0, 8)) <= gtime2)) : file.Duration == file.Duration) &&

                                      ((criteria[3].Split('#')[1].ToString().Length > 0 && criteria[4].Split('#')[1].ToString().Length > 0) ? (DateTime.Parse(criteria[3].Split('#')[1].ToString()) <= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))) &&
                                      (DateTime.Parse(criteria[4].Split('#')[1].ToString()) >= DateTime.Parse(file.DateTime.Value.ToString("yyyy-MM-dd"))) : file.DateTime.Value.ToString("yyyy-MM-dd") == file.DateTime.Value.ToString("yyyy-MM-dd")) &&

                                      ((criteria[5].Split('#')[1].ToString().Length > 0 && criteria[6].Split('#')[1].ToString().Length > 0) ? (DateTime.Parse(criteria[5].Split('#')[1].ToString()) <= DateTime.Parse(file.DateTime.Value.ToString("HH:mm:ss"))) &&
                                      (DateTime.Parse(criteria[6].Split('#')[1].ToString()) >= DateTime.Parse(file.DateTime.Value.ToString("HH:mm:ss"))) : file.DateTime.Value.ToString("HH:mm:ss") == file.DateTime.Value.ToString("HH:mm:ss")) &&

                                      (criteria[7].Split('#')[1].Length > 0 ? criteria[7].Split('#')[1].Contains(file.PhoneNo) : file.PhoneNo == file.PhoneNo) &&
                                      (criteria[8].Split('#')[1].Length > 0 ? lstChannels.Contains(file.ClientCh.ToString()) : file.ClientCh == file.ClientCh) &&

                                      (criteria[9].Split('#')[1].Length > 0 ? criteria[9].Split('#')[1].Contains(file.UserName) : file.UserName == file.UserName) &&
                                      file.DeleteFlag == false &&
                                      (file.IsPlay == null || file.IsPlay == true) &&
                                      (criteria[10].Split('#')[1].Length > 0 ? criteria[10].Split('#')[1].Contains(file.GroupName) : file.GroupName == file.GroupName)


                                  orderby file.DateTime ascending
                                  select new
                                  {
                                      ID = file.ID,
                                      StartRecording = file.DateTime,
                                      FileName = file.FileName,
                                      RecordingChannel = file.ClientCh,
                                      TalkWithChannel = file.TalkwithCh,
                                      FolderName = file.FolderName,
                                      Duration = file.Duration.Substring(0, 8),
                                      TalkWithRole = file.TalkwithRole,
                                      UserName = file.UserName,
                                      PhoneNo = file.PhoneNo,
                                      Isplay = file.IsPlay,
                                      IsMarked = file.IsMarked,
                                      GroupName = file.GroupName
                                  });


                    oDT1 = Helper.IEnumerableToDataTable(result.ToList());
                    if (oDT1.Rows.Count > 0)
                    {
                        foreach (var D in result)
                        {
                            int Id = D.ID;
                            string Filepath = D.FileName;
                            string FolderName = D.FolderName;
                            string SucessorNot = ofile.setDeleteAllClientFIle(Id, true, DateFrom, DateTo);
                            if (SucessorNot != "Sucess")
                            {
                                goto Fail;
                            }

                        }
                    Fail:
                        FetchData();
                    }
                    else
                    {
                        ValidationScript("Not Any Record found");
                    }
                }
                else
                    ValidationScript("Please select Specific From Date to TO date");
            }

        }

        protected void btnAllDelete_Click(object sender, EventArgs e)
        {
            FetchEmergancyAcess();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.AppendHeader("Refresh", "1; URL=http://local.thedeal1.info/local/LiveMonitor.aspx");
            Response.Redirect("LiveMonitor.aspx", true);
        }


    }
}

#region Coomments

//protected void dgvReport_PageIndexChanging1(object sender, GridViewPageEventArgs e)
//{
//    FetchData();
//    dgvReport.PageIndex = e.NewPageIndex;
//    dgvReport.DataBind();
//}

#endregion