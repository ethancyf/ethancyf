<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHKIDSmartIDSignal.ascx.vb"
    Inherits="HCSP.ucInputHKIDSmartIDSignal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
</script>

<asp:Panel ID="panEnterDetail" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td class="tableCellStyle" style="width: 200px;" valign="top">
                <asp:Label ID="lblDocumentTypeText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td class="tableCellStyle" valign="top" colspan="1">
                <asp:Label ID="lblDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 200px" valign="top" class="tableCellStyle">
                <asp:Label ID="lblHKICNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td valign="top" class="tableCellStyle" colspan="1">
                <asp:Label ID="lblHKICNo" runat="server" CssClass="tableText" MaxLength="9"></asp:Label></td>
        </tr>
        <tr>
            <td style="height: 19px; width: 200px;" valign="top" class="tableCellStyle">
                <asp:Label ID="lblENameText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="height: 19px;" valign="top" class="tableCellStyle">
                <asp:Label ID="lblEName" runat="server" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblCName" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 200px;" valign="top" class="tableCellStyle">
                <asp:Label ID="lblCCCodeText" runat="server" CssClass="tableTitle"></asp:Label></td>
            <td valign="top" class="tableCellStyle">
                <asp:Label ID="lblCCCode1" runat="server" CssClass="tableText" Width="35px"></asp:Label>&nbsp;
                <asp:Label ID="lblCCCode2" runat="server" CssClass="tableText" Width="35px"></asp:Label>&nbsp;
                <asp:Label ID="lblCCCode3" runat="server" CssClass="tableText" Width="35px"></asp:Label>&nbsp;
                <asp:Label ID="lblCCCode4" runat="server" CssClass="tableText" Width="35px"></asp:Label>&nbsp;
                <asp:Label ID="lblCCCode5" runat="server" CssClass="tableText" Width="35px"></asp:Label>&nbsp;
                <asp:Label ID="lblCCCode6" runat="server" CssClass="tableText" Width="35px"></asp:Label></td>
        </tr>
        <tr>
            <td class="tableCellStyle" style="width: 200px" valign="top">
                <asp:Label ID="lblDOBText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td class="tableCellStyle" valign="top">
                <asp:Label ID="lblDOB" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td valign="top" class="tableCellStyle" style="width: 200px">
                <asp:Label ID="lblGenderText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td valign="top" class="tableCellStyle">
                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblGender" runat="server" CssClass="tableText" Visible="false"></asp:Label>
                            <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" AutoPostBack="True" BackColor="#FFFF99" Visible="false">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Image ID="imgGenderError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false" /></td>
                        
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td valign="top" class="tableCellStyle" style="width: 200px">
                <asp:Label ID="lblDOIText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td valign="top" class="tableCellStyle">
                <asp:Label ID="lblDOI" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
