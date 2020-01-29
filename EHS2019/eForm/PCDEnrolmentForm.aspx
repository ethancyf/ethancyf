<%@ Page Language="vb" AutoEventWireup="false" Codebehind="PCDEnrolmentForm.aspx.vb"
    Inherits="eForm.PCD_EnrolmentForm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>PCD Enrolment Form</title>

    <script type="text/javascript">
        function Clear() {
        //alert(123);
            // ----- Remove the aspx post data -----
        var v = document.getElementById('__VIEWSTATE');
        if (v != null) {
            v.parentNode.removeChild(v);
        }
        
        v = document.getElementById('__EVENTTARGET');
        if (v != null) {
            v.parentNode.removeChild(v);
        }
        
        document.formPCDEnrolmentForm.submit();
        }
    </script>

</head>
<body>
    <form id="formPCDEnrolmentForm" runat="server" method="post">
        <div style="display:none;">
            <asp:TextBox ID="Data" runat="server"></asp:TextBox>
            <br />
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClientClick="Clear();" />
        </div>
    </form>

    <script type="text/javascript">
        window.onload = function() {document.getElementById('btnSubmit').click();};
    </script>

</body>
</html>
