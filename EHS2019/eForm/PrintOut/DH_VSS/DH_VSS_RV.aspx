<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="DH_VSS_RV.aspx.vb" Inherits="eForm.DH_VSS_RV" %>
<%@ Register Assembly="GrapeCity.ActiveReports.Web.v8, Version=8.2.492.0, Culture=neutral, PublicKeyToken=cc4967777c49a3ff"
    Namespace="GrapeCity.ActiveReports.Web" TagPrefix="ActiveReportsWeb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>            <activereportsweb:webviewer id="wvDH_VSS_RV" runat="server" height="46px" width="345">
            <PdfExportOptions Encrypt="True" />
        </activereportsweb:webviewer></div>
    </form>
</body>
</html>
