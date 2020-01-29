<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucPCDWarningPopup.ascx.vb"
    Inherits="HCVU.ucPCDWarningPopup" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<table border="0" cellpadding="0" cellspacing="0" width="100%">
    <!-- Header -->
    <tr>
        <td colspan="3">
            <asp:Panel ID="panHeader" runat="server" Style="cursor: move;">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                        </td>
                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                            <asp:Label ID="lblHeader" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label>
                        </td>
                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <!-- Content -->
    <tr>
        <td id ="tdContentLeft" runat="server" style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
        </td>
        <td style="background-color: #ffffff">            
            <table cellpadding="0" cellspacing="0" width="100%" style="padding:5px;">
                <tr>
                    <td>
                        <cc2:MessageBox ID="udcMessageBox" runat="server" />
                        <asp:HiddenField ID="hfPCDWarningType" runat="server" />
                        <!------------------- Confirmation ----------------->
                        <!--------------------------------------------->
                        <table width="95%">
                            <colgroup />
                            <!-- Display message -->
                            <tr>
                                <td align="right" style="width: auto; height: 42px; padding-right:10px;" valign="middle">
                                    <asp:Image ID="imgIcon" runat="server" ImageUrl="<%$ Resources:ImageUrl, QuestionMarkIcon %>" />
                                </td>
                                <td align="left" style="width: 95%; height: 42px">
                                    <asp:Label ID="lblMsg" runat="server" Style="font-family: Arial;
                                        font-weight: bold; font-size: 14px; color: #666666; width: 100%; text-align: justify" 
                                        BorderStyle="none"></asp:Label>
                                </td>
                            </tr>
                            <!-- Checkbox -->
                            <tr id="trConfirmCheckbox" runat="server">                                
                                <td></td>
                                <td align="left" style="height: 20px; text-align: left; padding-top: 20px">
                                    <asp:CheckBox ID="chkConfirm" runat="server" Font-Bold="True" style="text-indent: -20px" />
                                    <asp:Image ID="imgConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Visible="False" />                                 
                                </td>
                            </tr>
                            <!-- Button -->
                            <tr>
                                <td align="center" style="padding-top:20px;" colspan="2">
                                    <asp:ImageButton ID="ibtnConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" />
                                    <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" />
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
        <td id ="tdContentRight" runat="server" style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
        </td>
    </tr>
    <tr>
        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
        </td>
        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
            height: 7px">
        </td>
        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
            height: 7px">
        </td>
    </tr>
</table>
