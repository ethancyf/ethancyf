<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucPCDEnrolledPopup.ascx.vb"
    Inherits="HCSP.ucPCDEnrolledPopup" %>
<%@ Register Src="../Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc2" %>
<%@ Register Src="ucTypeOfPracticeGrid.ascx" TagName="ucTypeOfPracticeGrid" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
   
    
</script>

<table border="0" cellpadding="0" cellspacing="0">
    <%-- Header --%>
    <tr>
        <td colspan="3">
            <asp:Panel ID="panHeader" runat="server" Style="cursor: move;" Width="100%">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                        </td>
                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                            <asp:Label ID="lblTitle" runat="server" Text="<%$ Resources:Text, UpdatePCD%>"></asp:Label></td>
                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <%-- Content --%>
    <tr>
        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
            <table width="7px">
                <tr>
                    <td>
                    </td>
                </tr>
            </table>
        </td>
        <td style="background-color: #ffffff">
            <table cellpadding="5" cellspacing="0" style="width: 100%;">
                <tr>
                    <td>
                        <%------------------- Body -----------------%>
                        <%---------------------------------------------%>
                        <table cellpadding="1" cellspacing="0" style="vertical-align: middle;
                            margin-right: 5px; padding-top:5px; padding-left:5px; padding-bottom:5px;">
                            <!-- Description -->
                            <tr>
                                <td align="right" style="width: auto; height: 42px; padding-top: 10px;" valign="middle">
                                    <asp:Image ID="imgIcon" runat="server" ImageUrl="<%$ Resources:ImageUrl, QuestionMarkIcon%>" AlternateText="<%$ Resources:AlternateText, QuestionMarkIcon %>" ToolTip="<%$ Resources:AlternateText, QuestionMarkIcon %>" /></td>
                                <td align="left" style="width: 95%; height: 42px; padding-left: 10px;">
                                    <asp:Label ID="lblMsg" runat="server" Style="font-family: Arial;
                                        font-weight: bold; font-size: 14px; color: #666666;" 
                                        BorderStyle="none" Text="<%$ Resources:Text, PCDEnrolledMessage %>"></asp:Label>
                                </td>
                            </tr>
                            <%-- Button: Cancel, Create PCD Account --%>
                            <tr style="height:10px">
                                <td colspan="2" align="center" style="padding-top: 10px; padding-bottom:0px">
                                    <table cellpadding="3" cellspacing="0">
                                        <tr valign="middle">
                                            <td style="height:10px;">
                                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>"  ToolTip="<%$ Resources:AlternateText, CancelBtn %>"/>
                                            </td>
                                            <td style="height:10px;">
                                                <asp:ImageButton ID="ibtnAddPractice" runat="server" ImageUrl="<%$ Resources:ImageUrl, AddPCDPractice  %>"
                                                    AlternateText="<%$ Resources:AlternateText, AddPCDPracticeBtn %>" ToolTip="<%$ Resources:AlternateText, AddPCDPracticeBtn %>"/>
                                            </td>
                                            <td style="height:10px;">
                                                <asp:ImageButton ID="ibtnLoginPCD" runat="server" ImageUrl="<%$ Resources:ImageUrl, LoginPCD %>"
                                                    AlternateText="<%$ Resources:AlternateText, LoginPCDBtn %>" ToolTip="<%$ Resources:AlternateText, LoginPCDBtn %>" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
    </table> </td>
    <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
        <table width="7px">
            <tr>
                <td>
                </td>
            </tr>
        </table>
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
