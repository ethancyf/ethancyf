<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DPAViewer.aspx.vb" Inherits="HCVU.DPAViewer" %>

<%@ Register Assembly="GrapeCity.ActiveReports.Web.v12, Version=12.2.13986.0, Culture=neutral, PublicKeyToken=cc4967777c49a3ff"
    Namespace="GrapeCity.ActiveReports.Web" TagPrefix="ActiveReportsWeb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <ActiveReportsWeb:WebViewer ID="WebViewer1" runat="server" Height="46" Width="345">
        </ActiveReportsWeb:WebViewer>
    
    </div>
    </form>
</body>
</html>
