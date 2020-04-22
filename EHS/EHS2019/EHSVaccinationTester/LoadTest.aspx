<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="LoadTest.aspx.vb" Inherits="EHSVaccinationTester.LoadTest" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table width="100%">
            <tr>
                <td width="10%">
                   <asp:Label ID="lblRequestNumber" runat="server" Text="Request Mode:"></asp:Label>
                </td>
                <td>
                    <asp:RadioButton ID="rbSingle" checked="true" AutoPostBack="True" GroupName="rbgRequest" runat="server" Text="Single"/>
                    <asp:RadioButton ID="rbBatch" AutoPostBack="True" GroupName="rbgRequest" runat="server" Text="Batch "/>
                </td>
            </tr>
            <tr>
                <td>
                     <asp:Label ID="lblURLText" runat="server" Text="URL (EHS):"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblURL" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                     <asp:Label ID="lblCMSURLText" runat="server" Text="URL (HA CMS):"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblCMSURL" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                     <asp:Label ID="lblCIMSURLText" runat="server" Text=" URL (DH CIMS):"></asp:Label>
                </td>
                <td>
                    <asp:Label ID="lblCIMSURL" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                     <asp:Label ID="lblRequestSystem" runat="server" Text="Request System:"></asp:Label>
                </td>
                <td>
                    <asp:Button ID="btnCallCMS" runat="server" Text="Call CMS" /> &nbsp; 
                    <asp:Button ID="btnCallCIMS" runat="server" Text="Call CIMS" /> &nbsp; 
                    <asp:Button ID="btnCallEHSCMS" runat="server" Text="Call EHS (From CMS)" /> &nbsp;
                    <asp:Button ID="btnCallEHSCIMS" runat="server" Text="Call EHS (From CIMS)" />
                </td>
            </tr>
             <tr>
                <td>
                   
                </td>
                <td>
                       <asp:Label ID="lblResultCallCMS" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                   
                </td>
                <td>
                       <asp:Label ID="lblRandomFileName" runat="server"></asp:Label>
                </td>
            </tr>
    
        </table>
     </div>
    </form>
</body>
</html>
