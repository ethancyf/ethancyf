<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucNoticePopUp.ascx.vb" Inherits="eForm.ucNoticePopUp" %>

<script type="text/javascript">
   
    
</script>

<table id="tblPopup" runat="server" border="0" cellpadding="0" cellspacing="0" width="100%">
    <%--Header --%>
    <tr>
        <td colspan="3">
            <asp:Panel ID="panHeader" runat="server" Style="cursor: move;">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td id ="tdHeadingLeft" runat="server" style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                        </td>
                        <td id ="tdHeadingTop" runat="server" style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                            <asp:Label ID="lblHeader" runat="server"></asp:Label></td>
                        <td id ="tdHeadingRight" runat="server" style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <%-- Content --%>
    <tr>
        <td id ="tdContentLeft" runat="server" style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
        </td>
        <td style="background-color: #ffffff">
            <table cellpadding="0" cellspacing="0" width="100%" style="padding-top:5px; padding-bottom:5px; padding-left:5px; padding-right:5px;">
                <tr>
                    <td>
                        <%------------------- Notification -----------------%>
                        <%---------------------------------------------%>
                        <table>
                            <colgroup />
                            <%-- Display message --%>
                            <tr>
                                <td align="right" style="width: auto; height: 42px; padding-right:10px;" valign="top">
                                    <asp:Image ID="imgIcon" runat="server" ImageUrl="" /></td>
                                <td align="left" style="width: 95%; height: 42px">
                                    <asp:Label ID="lblMsg" runat="server" Style="font-family: Arial;
                                        font-weight: bold; font-size: 14px; color: #666666;" 
                                        BorderStyle="none"></asp:Label>
                                </td>
                            </tr>
                            <%-- Button --%>
                            <tr>
                                <td colspan="2" align="center" style="padding-top:10px;" >
                                    <asp:ImageButton ID="ibtnCancel" runat="server" />
                                    <asp:ImageButton ID="ibtnOK" runat="server" />
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
        <td id ="tdFooterLeft" runat="server" style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
        </td>
        <td id ="tdFooterBottom" runat="server" style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
            height: 7px">
        </td>
        <td id ="tdFooterRight" runat="server" style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
            height: 7px">
        </td>
    </tr>
</table>
