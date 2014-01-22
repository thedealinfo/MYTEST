<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="LiveMonitor.aspx.cs" Inherits="TheDealPortal.LiveMonitor" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<%--<meta http-equiv="refresh" content="1">--%>
    <title></title>
    <style type="text/css">


a:link, a:visited
{
    color: #034af3;
}

    </style>
</head>
<body>
    <form id="form1" runat="server">
   
  
     

  <asp:ScriptManager ID="ScriptManager1" runat="server" />
     <div>
     <asp:Timer ID="Timer1" runat="server" Interval="1000" ontick="Timer1_Tick">

      </asp:Timer>
      </div>

    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">

   
        <ContentTemplate>
            <asp:GridView ID="dgvReport" runat="server" AutoGenerateColumns="False" 
                BorderStyle="Solid" BorderWidth="1px" DataKeyNames="ID" 
                onrowdatabound="dgvReport_RowDataBound" Width="100%">
                <Columns>
                    <asp:BoundField DataField="ID" HeaderText="ID" ItemStyle-Width="230px" 
                        ReadOnly="True" Visible="true">
                    <ItemStyle Width="230px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ChannelNo" HeaderText="Channel" 
                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px">
                    <ItemStyle HorizontalAlign="Center" Width="10px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="ChStatus" HeaderText="ChStatus" 
                        ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10px" ReadOnly="True">
                    <ItemStyle HorizontalAlign="Center" Width="10px" />
                    </asp:BoundField>
                </Columns>
                <EmptyDataTemplate>
                    No Data found
                </EmptyDataTemplate>
                <HeaderStyle Wrap="False" />
                <PagerSettings PageButtonCount="50" />
                <RowStyle Wrap="False" />
            </asp:GridView>
        </ContentTemplate>

   
      <Triggers>
       
        <asp:AsyncPostBackTrigger ControlID="Timer1" EventName="Tick" />

      </Triggers>
      </asp:UpdatePanel>
     
                
     <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
       </asp:ScriptManager>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <asp:Timer ID="Timer1" runat="server" Interval="1000" ontick="Timer1_Tick">
                        </asp:Timer>
                    </ContentTemplate>
                 </asp:UpdatePanel>--%>
                
               
   
    </div>
     
    </form>
    
</body>
</html>
