<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucTypeOfPracticePopup.ascx.vb"
    Inherits="HCSP.ucTypeOfPracticePopup" %>
<%@ Register Src="../Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc2" %>
<%@ Register Src="ucTypeOfPracticeGrid.ascx" TagName="ucTypeOfPracticeGrid" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<script type="text/javascript">
    function chkDeclarationCheckChanged(objName, btnEnable, btnDisable) {
        if (objName.checked == true ) {
            if (document.getElementById(btnEnable) != null) {
                document.getElementById(btnEnable).style.display="block";  
            }        
            if (document.getElementById(btnDisable) != null) {
                document.getElementById(btnDisable).style.display="none";  
            }
        }
        else {
            if (document.getElementById(btnEnable) != null) {
                document.getElementById(btnEnable).style.display="none";
            }
            if (document.getElementById(btnDisable) != null) {
                document.getElementById(btnDisable).style.display="block";
            }            
        }
    };    
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
                            <asp:Label ID="lblTitle" runat="server" Text="<%$ Resources:Text, UpdatePCD %>"></asp:Label></td>
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
                        <cc2:MessageBox ID="udcTMessageBox" runat="server" />
                        <%------------------- Body -----------------%>
                        <%---------------------------------------------%>
                        <table cellpadding="1" cellspacing="0" style="vertical-align: top; margin-right: 10px;">
                            <%-- Description --%>
                            <tr>
                                <td style="padding-left: 10px; padding-bottom: 10px">
                                    <asp:Label ID="lblDescForJoin" runat="server" CssClass="tableText"><% =Me.GetGlobalResourceObject("Text", "PCDJoinMessage")%></asp:Label>
                                    <asp:Label ID="lblDescForTransfer" runat="server" CssClass="tableText"><% =Me.GetGlobalResourceObject("Text", "PCDTransferMessage")%></asp:Label>
                                </td>
                            </tr>
                            <%-- Description 2--%>
                            <tr>
                                <td style="padding-left: 10px; padding-bottom: 10px">
                                    <asp:Label ID="lblDescForTransferDetails" runat="server" CssClass="tableText"><% =Me.GetGlobalResourceObject("Text", "PCDTransferMessageDetails")%></asp:Label>
                                </td>
                            </tr>
                            <%-- Practice List --%>
                            <tr>
                                <td style="padding-left: 10px;">
                                    <input type="hidden" id="scroll" runat="server" /><br />
                                    <%--<asp:Panel ID="panTypeOfPractice" runat="server"  Width="100%" Height="373px" ScrollBars="Auto">--%>
                                    <%--<div id="divPracticeInfo" style="overflow: auto; height: 373px;" onscroll="javascript:document.getElementById('<%=scroll.ClientID %>').value = this.scrollTop">--%>
                                    <div style="overflow: auto; height: 373px;">
                                        <uc1:ucTypeOfPracticeGrid ID="ucTypeOfPracticeGrid" runat="server" />
                                    </div>
                                    <%--</div>--%>
                                    <%--</asp:Panel>--%>
                                </td>
                            </tr>
                            <%-- Declaration --%>
                            <tr>
                                <td style="padding-left: 10px; padding-top:5px;">
                                    <asp:CheckBox ID="chkDeclaration" runat="server" CssClass="tableText" Text="<%$ Resources:Text, PCDDeclaration %>" />
                                    <asp:LinkButton ID="lblTermsAndCondition" runat="server" CssClass="tableText" ForeColor="blue"><% =Me.GetGlobalResourceObject("Text", "PCDTermsAndConditionText")%></asp:LinkButton>
                                </td>
                            </tr>
                            <%-- Button: Cancel, Create PCD Account --%>
                            <tr>
                                <td colspan="3" align="center" style="padding-top: 10px;">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr valign="top">
                                            <td>
                                                <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CancelBtn %>" ToolTip="<%$ Resources:AlternateText, CancelBtn %>" />&nbsp;
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="ibtnCreate" runat="server" ImageUrl="<%$ Resources:ImageUrl, JoinPCDBtn %>" style="display:none;"
                                                    AlternateText="<%$ Resources:AlternateText, JoinPCDBtn %>" ToolTip="<%$ Resources:AlternateText, JoinPCDBtn %>"/>
                                                <asp:ImageButton ID="ibtnCreateDisable" runat="server" ImageUrl="<%$ Resources:ImageUrl, JoinPCD_DBtn %>"  style="display:block;"
                                                    AlternateText="<%$ Resources:AlternateText, JoinPCDBtn %>" ToolTip="<%$ Resources:AlternateText, JoinPCDBtn %>" OnClientClick="return false;" />
                                                <asp:ImageButton ID="ibtnTransfer" runat="server" ImageUrl="<%$ Resources:ImageUrl, TransferToPCDBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, TransferToPCDBtn %>" ToolTip="<%$ Resources:AlternateText, TransferToPCDBtn %>" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
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
<%-- Notice Poup--%>
<asp:Panel ID="panNoticePopup" runat="server" Width="400px" Style="display: none;">
    <uc2:ucNoticePopUp ID="ucNoticePopUp" runat="server" NoticeMode="Confirmation" MessageText="<%$ Resources:Text, PCDNoticePopupMessage %>" />
    <asp:Button ID="btnNoticePopupDummy" runat="server" Style="display: none" />
</asp:Panel>
<cc1:ModalPopupExtender ID="ModalPopupExtenderNotice" runat="server" TargetControlID="btnNoticePopupDummy"
    PopupControlID="panNoticePopup" BehaviorID="mdlPopup5" BackgroundCssClass="modalBackgroundTransparent"
    DropShadow="False" RepositionMode="none" PopupDragHandleControlID="">
</cc1:ModalPopupExtender>
