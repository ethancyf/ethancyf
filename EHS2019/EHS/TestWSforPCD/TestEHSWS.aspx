<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestEHSWS.aspx.vb" Inherits="TestWSforPCD.TestEHSWS" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Test EHS Interface!</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
            <asp:Label ID="lblTITLE" Text="This Page is for testing EHS's web service for PCD Integration: InterfaceWS/PCDInterface.asmx" runat="server"></asp:Label>
            <br />
            <hr />
            <asp:Label Text="Login:" ID="lblLoginText" runat="server"></asp:Label>
            <asp:TextBox ID="txtLogin" Width="200px" runat="server"></asp:TextBox> <br />
            <asp:Label Text="Password:" ID="lblPwdText" runat="server"></asp:Label>
            <asp:TextBox ID="txtPwd" Width="200px" runat="server"></asp:TextBox> <br />
            <asp:Label Text="URI:" id="lblURItext" runat="server"></asp:Label>
            <asp:TextBox ID="txtURI" Width="400px" runat="server"></asp:TextBox>
            <br />
            <asp:Label Text="HKID:" ID="lblHKIDtext" runat="server"></asp:Label>
            <asp:TextBox ID="txtHKID" runat="server"></asp:TextBox>
            <asp:Button ID="btnCallWS" runat="server" Text="Call Web Service" /> 
                        
            <br />
            <hr />
            <asp:Label Text="Results:" ID="lblResultstext" runat="server"></asp:Label><br />
            
            <hr />
            <asp:Label Text="Request XML (from PCD):" ID="lblRequestXML" runat="server"></asp:Label><br />
            <asp:Literal ID="ltlRequestXML" runat="server"></asp:Literal>
            
            <hr />
            <asp:Label Text="Result XML (generated):" ID="lblResultXML" runat="server"></asp:Label><br />
            <asp:Literal ID="ltlResultXML" runat="server"></asp:Literal>
                   
    </div>
    </form>
</body>
</html>
