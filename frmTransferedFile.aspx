<%@ Page Title="TheDeal - Recording file list and Playing" Language="C#" MasterPageFile="~/Site.Master"   MaintainScrollPositionOnPostback="true" EnableViewState="true"
    AutoEventWireup="true" CodeBehind="frmTransferedFile.aspx.cs" Inherits="TheDealPortal.frmFileList" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="aspx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script language="javascript" type="text/javascript">
        function RunEXE(file) {
            var oShell = new ActiveXObject("WScript.Shell");
            oShell.Run("C:\\Program Files\\Windows Media Player\\wmplayer.exe");
        }
    </script>
    <style type="text/css">
        .style5
        {
            height: 31px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <%--  <asp:Button runat="server" ID="btnDownload" Text="Download Selected Files" OnClick="btnDownload_Click" />--%>
    <asp:Panel ID="Panel2" runat="server">
        <table cellpadding="1" border="0" frame="border">
            <tr>
                <th>
                    <asp:Button ID="btnplaylist" runat="server" onclick="btnplaylist_Click" 
                                    Text="Add To PlayList" />
                </th>
                <td>
                    <asp:Button ID="btnAddtoDelete" runat="server" onclick="btnAddtoDelete_Click" 
                                    Text="Add To Delete" />
                </td>
                <td>
                    <asp:Button ID="btnAutoProcess" runat="server" onclick="btnAutoProcess_Click" 
                                    Text="Auto Process" />
                </td>
                <td>
                    <asp:Button ID="Button1" runat="server" onclick="Button1_Click" 
                        Text="Android" />
                    </td>
                <td>
                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                <th>
                    &nbsp;From Date :&nbsp;
                </th>
                <td>
                    <%--<aspx:MaskedEditExtender ID="MaskedEditExtender6" runat="server" 
                                    Mask="99:99:99" MaskType="Time" TargetControlID="txtDuration" 
                                    UserTimeFormat="TwentyFourHour">
                    </aspx:MaskedEditExtender>--%>
                    <asp:TextBox ID="txtDelFromDate" runat="server" BorderColor="BlueViolet" 
                                    BorderStyle="Solid" BorderWidth="1" Width="80px"></asp:TextBox>
                    <aspx:CalendarExtender ID="txtDelFromDate_CalendarExtender" runat="server" 
                                    Format="yyyy-MM-dd" TargetControlID="txtDelFromDate" />
                </td>
                <th>
                    To Date :
                </th>
                <td>
                    <%--<aspx:MaskedEditExtender ID="MaskedEditExtender7" runat="server" 
                                    Mask="99:99:99" MaskType="Time" TargetControlID="txtDurationTo" 
                                    UserTimeFormat="TwentyFourHour">
                    </aspx:MaskedEditExtender>--%>
                    <asp:TextBox ID="txtDelToDate" runat="server" BorderColor="BlueViolet" 
                                    BorderStyle="Solid" BorderWidth="1" Width="80px"></asp:TextBox>
                    <aspx:CalendarExtender ID="txtDelToDate_CalendarExtender" runat="server" 
                                    Format="yyyy-MM-dd" TargetControlID="txtDelToDate" />
                </td>
                <td>
                    <asp:Button runat="server" ID="btnDateDelete" Text="Delete File" 
                        OnClick="btnDateDelete_Click" />
                </td>
                <td>
                    <asp:Button ID="btnAllDelete" runat="server" BorderStyle="None" 
                        ForeColor="#CC3300" Height="16px" OnClick="btnAllDelete_Click" Text="-" 
                        Width="18px" />
                </td>
            </tr>
        </table>
    </asp:Panel>
        
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
            <asp:PostBackTrigger ControlID="dgvReport" />
        </Triggers>
        <ContentTemplate>
            <asp:Timer ID="Timer1" runat="server" Interval="1000" Enabled="False" 
                OnTick="Timer1_Tick">
            </asp:Timer>
            <div style="padding: 10px;">
                <asp:CheckBox ID="chkAutoRefresh" runat="server" Text="Auto Refresh" AutoPostBack="true"
                    OnCheckedChanged="chkAutoRefresh_CheckedChanged" ViewStateMode="Enabled" />
                <asp:Button runat="server" ID="btnDownload" Text="-" 
                    OnClick="btnDownload_Click" Height="16px" Width="18px" />
                
                <asp:Button ID="btnClientDelete" runat="server" onclick="btnClientDelete_Click" 
                    Text="Delete Select File" Visible="False" />
                
               
                
                <asp:Button ID="Button2" runat="server" onclick="Button2_Click" Text="Button" />
                
               
                
            </div>
            <table width="100%" runat="server" id="tblCriteria" visible="false">
                <tr>
                    <td>
                        <table cellpadding="5">
                            <tr>
                                <th>
                                    Select Folder :
                                </th>
                                <td>
                                    <asp:CheckBox ID="chkAdmin" runat="server" Text="Admin" AutoPostBack="True" OnCheckedChanged="chkAdmin_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkClient" runat="server" Text="Client" AutoPostBack="True" OnCheckedChanged="chkClient_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkAnnounce" runat="server" Text="Announce" AutoPostBack="True"
                                        OnCheckedChanged="chkAnnounce_CheckedChanged" />
                                </td>
                                <td>
                                    <asp:CheckBox ID="chkSpecial" runat="server" Text="Special" AutoPostBack="True" OnCheckedChanged="chkSpecial_CheckedChanged" />
                                </td>
                                <th>
                                    Duration From:
                                </th>
                                <td>
                                    <asp:TextBox ID="txtDuration" runat="server" BorderStyle="Solid" Width="70px" BorderWidth="1"
                                        BorderColor="BlueViolet"></asp:TextBox>
                                    <aspx:MaskedEditExtender ID="MaskedEditExtender3" runat="server" Mask="99:99:99"
                                        MaskType="Time" TargetControlID="txtDuration" UserTimeFormat="TwentyFourHour">
                                    </aspx:MaskedEditExtender>
                                </td>
                                <th>
                                    Duration To:
                                </th>
                                <td>
                                    <asp:TextBox ID="txtDurationTo" runat="server" BorderStyle="Solid" Width="70px" BorderWidth="1"
                                        BorderColor="BlueViolet"></asp:TextBox>
                                    <aspx:MaskedEditExtender ID="MaskedEditExtender4" runat="server" Mask="99:99:99"
                                        MaskType="Time" TargetControlID="txtDurationTo" UserTimeFormat="TwentyFourHour">
                                    </aspx:MaskedEditExtender>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table cellpadding="5">
                            <tr>
                                <th>
                                    From Date :
                                </th>
                                <td>
                                    <asp:TextBox ID="txtDateFrom" runat="server" BorderStyle="Solid" BorderWidth="1"
                                        BorderColor="BlueViolet" Width="80px"></asp:TextBox>
                                    <aspx:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDateFrom"
                                        Format="yyyy-MM-dd" />
                                </td>
                                <th>
                                    To Date :
                                </th>
                                <td>
                                    <asp:TextBox ID="txtDateTo" runat="server" Width="80px" BorderStyle="Solid" BorderWidth="1"
                                        BorderColor="BlueViolet"></asp:TextBox>
                                    <aspx:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDateTo"
                                        Format="yyyy-MM-dd" />
                                </td>
                                <th>
                                    From Time:
                                </th>
                                <td>
                                    <asp:TextBox ID="txtTimeFrom" runat="server" BorderStyle="Solid" Width="70px" BorderWidth="1"
                                        BorderColor="BlueViolet"></asp:TextBox>
                                    <aspx:MaskedEditExtender ID="MaskedEditExtender1" runat="server" Mask="99:99:99"
                                        MaskType="Time" TargetControlID="txtTimeFrom" UserTimeFormat="TwentyFourHour">
                                    </aspx:MaskedEditExtender>
                                </td>
                                <th>
                                    To Time:
                                </th>
                                <td>
                                    <asp:TextBox ID="txtTimeTo" runat="server" BorderStyle="Solid" Width="70px" BorderWidth="1"
                                        BorderColor="BlueViolet"></asp:TextBox>
                                    <aspx:MaskedEditExtender ID="MaskedEditExtender2" runat="server" Mask="99:99:99"
                                        MaskType="Time" TargetControlID="txtTimeTo" UserTimeFormat="TwentyFourHour">
                                    </aspx:MaskedEditExtender>
                                </td>
                                <th>
                                    GroupName:
                                </th>
                                <td>
                                    <asp:TextBox ID="txtGroupName" runat="server" BorderStyle="Solid" Width="180px" BorderWidth="1"
                                        BorderColor="BlueViolet"></asp:TextBox>
                                    <aspx:MaskedEditExtender ID="MaskedEditExtender5" runat="server" Mask="99:99:99"
                                        MaskType="Time" TargetControlID="txtTimeTo" UserTimeFormat="TwentyFourHour">
                                    </aspx:MaskedEditExtender>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="style5">
                        <table>
                            <tr>
                                <th>
                                    Mobile :
                                </th>
                                <td>
                                    <asp:TextBox ID="txtMobile" runat="server" BorderStyle="Solid" BorderWidth="1" BorderColor="BlueViolet"></asp:TextBox>
                                </td>
                                <th>
                                    Channel No. :
                                </th>
                                <td>
                                    <asp:TextBox ID="txtChannelNo" runat="server" BorderStyle="Solid" BorderWidth="1"
                                        BorderColor="BlueViolet"></asp:TextBox>
                                </td>
                                <th>
                                    User Name :
                                </th>
                                <td>
                                    <asp:TextBox ID="txtUserName" runat="server" BorderStyle="Solid" BorderWidth="1"
                                        BorderColor="BlueViolet" Width="400px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td>
                        <table>
                            <tr>
                                <td>
                                    <asp:Button runat="server" ID="btnSearch" Text="Search" OnClick="btnSearch_Click" />
                                </td>
                                <td>
                                    <asp:Button runat="server" ID="btnDelete" Text="Delete Selected Files" 
                                        OnClick="btnDelete_Click" Visible="False" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="Panel1" runat="server" ScrollBars="Auto" Height="433px" BorderStyle="Solid"
                BorderWidth="1">
                <asp:GridView ID="dgvReport" runat="server" CellPadding="3" AllowSorting="True"
                    AutoGenerateColumns="False" BorderStyle="Solid" BorderWidth="1px" DataKeyNames="ID"
                    OnRowDataBound="dgvReport_RowDataBound" 
                    OnRowCommand="dgvReport_RowCommand"  
                    onpageindexchanging="dgvReport_PageIndexChanging" AllowPaging="True" Width="100%" 
                     >
                    <Columns>
                 
                           <asp:TemplateField ItemStyle-Width="15px">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkAll" runat="server" AutoPostBack="true" OnCheckedChanged="chkAll_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <center>
                                    <asp:CheckBox ID="chkItem" runat="server" />
                                </center>
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                        </asp:TemplateField>

                        
                        <asp:TemplateField HeaderText="Play" ItemStyle-Width="10px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPlay" CommandName="Play" runat="server" CausesValidation="False"
                                    CommandArgument='<%# Eval("ID") %>' Text="Play"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="10px" />
                        </asp:TemplateField>
                       
                        <asp:BoundField DataField="UserName" HeaderText="User Name" ReadOnly="True" Visible="true"
                            ItemStyle-Width="230px" >
                        <ItemStyle Width="230px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Play" ItemStyle-Width="15px" Visible="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkPhone" CommandName="Mark" runat="server" CausesValidation="False"
                                    CommandArgument='<%# Eval("ID") %>' Text='<%# Eval("PhoneNo") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="RecordingChannel" HeaderText="Ch" ItemStyle-Width="10px"
                            ItemStyle-HorizontalAlign="Center" Visible="False" >
                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TalkWithChannel" HeaderText="TW" ReadOnly="True" ItemStyle-Width="10px"
                            ItemStyle-HorizontalAlign="Center" Visible="False" >
                        <ItemStyle HorizontalAlign="Center" Width="10px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="TalkWithRole" HeaderText="TWR" ReadOnly="True" 
                            ItemStyle-Width="20px" Visible="False" >
                        <ItemStyle Width="20px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="StartRecording" HeaderText="Date" NullDisplayText="--"
                            ReadOnly="True" DataFormatString="{0: dd-MM}" ItemStyle-Width="25px" 
                            ItemStyle-HorizontalAlign="Center" >
                        <ItemStyle HorizontalAlign="Center" Width="25px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="StartRecording" HeaderText="Time" NullDisplayText="--"
                            ReadOnly="True" DataFormatString="{0: HH:mm:ss}" ItemStyle-Width="25px" 
                            ItemStyle-HorizontalAlign="Center" >
                        <ItemStyle HorizontalAlign="Center" Width="25px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Duration" HeaderText="Duration" ReadOnly="True" ItemStyle-Width="25px"
                            ItemStyle-HorizontalAlign="Center" DataFormatString="{0: HH:mm:ss}" >
                        <ItemStyle HorizontalAlign="Center" Width="25px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="FileNameWithPath" HeaderText="File Name" ReadOnly="True"
                            Visible="false" />
                        <asp:BoundField DataField="FolderName" HeaderText="Folder" ReadOnly="True" Visible="true"
                            ItemStyle-Width="25px" >
                        <ItemStyle Width="25px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="ID" HeaderText="ID" Visible="False" />
                         <asp:TemplateField ItemStyle-Width="15px">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkAll1" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllUnplay_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <center>
                                    <asp:CheckBox ID="chkItem1" runat="server" />
                                </center>
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                        </asp:TemplateField>
                          <asp:TemplateField ItemStyle-Width="15px">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chkAllDelete" runat="server" AutoPostBack="true" OnCheckedChanged="chkAllDelete_CheckedChanged" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <center>
                                    <asp:CheckBox ID="chkItem2" runat="server" />
                                </center>
                            </ItemTemplate>
                            <ItemStyle Width="15px" />
                        </asp:TemplateField>

                    </Columns>
                    <EmptyDataTemplate>
                        No Data found</EmptyDataTemplate>
                    <HeaderStyle Wrap="False" />
                    <PagerSettings PageButtonCount="50" />
                    <RowStyle Wrap="False" />
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
