<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReasonForVisit.ascx.vb"
    Inherits="HCSP.UIControl.EHCClaimText.ucReasonForVisit" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>

<style type="text/css">
    .rbChoice input { margin-left: -20px; }
    .rbChoice td { padding-left: 20px; }
</style> 

<table border="0" cellpadding="0" cellspacing="0">
    <tr>
        <td>
            <cc1:TextOnlyMessageBox ID="udcMsgBoxErr" runat="server"/>
        </td>
    </tr>
</table>
<asp:MultiView ID="mvHCVS" runat="server" ActiveViewIndex="0">
    <asp:View ID="vSelectReasonForVisitGroup" runat="server">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="lblSelectReasonForVisitGroupHeader" runat="server" CssClass="tableTitle"
                        Text="<%$ Resources:Text, ReasonVisit %>" />
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td>
                    <asp:Label ID="lblNoReasonForVisit" runat="server" Text="<%$ Resources:AlternateText, NoReasonForVisitSelected %>" CssClass="tableText"></asp:Label>
                </td>
            </tr>
        </table>
        <table id="tblRFV" class="ReasonForVisitTable" cellpadding="0" cellspacing="1" runat="server">
            <tr id="trRFVPrincipal" runat="Server">
                <td>
                    <table id="tblRFVPrincipal" cellpadding="3" cellspacing="0" class="ReasonForVisitTableHeading"
                        runat="server">
                        <tr id="trRFVPrincipalHeading" runat="Server">
                            <td class="VR" colspan="2">
                                <asp:Label ID="lblRFVPrincipalText" runat="server" CssClass="tableText" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trRFVPrincipalContent" runat="Server" class="ReasonForVisitGroupTable1" >
                            <td class="rbSelectRFVGroup">
                                <asp:RadioButton runat="server" GroupName="rbSelectRFVGroup" ID="rdoRFVPrincipal" AutoPostBack="true" CssClass="rbChoice" />
                            </td>
                            <td valign="top">
                                <asp:Label ID="lblRFVPrincipalContentError" runat="server" CssClass="validateFailText" Visible="false" Text="*"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="trRFVSecondary" runat="Server">
                <td>
                    <table id="tblRFVSecondary" cellpadding="3" cellspacing="0" class="ReasonForVisitTableHeading"
                        runat="server">
                        <tr id="trRFVSecondaryHeading" runat="Server">
                            <td class="VR" colspan="2">
                                <asp:Label ID="lblRFVSecondaryText" runat="server" CssClass="tableText"  Font-Bold="true">></asp:Label>
                            </td>
                        </tr>
                        <tr id="trRFVSecondary1Content" runat="Server" class="ReasonForVisitGroupTable1">
                            <td id="trRFVSecondary1ContentOption" class="rbSelectRFVGroup">
                                <asp:RadioButton runat="server" GroupName="rbSelectRFVGroup" ID="rdoRFVSecondary1" AutoPostBack="true" CssClass="rbChoice" />
                            </td>
                            <td id="trRFVSecondary1ContentError" valign="top">
                                <asp:Label ID="lblRFVSecondary1ContentError" runat="server" CssClass="validateFailText" Visible="false" Text="*"></asp:Label>                            
                            </td>                            
                        </tr>
                        <tr id="trRFVSecondary2Content" runat="Server" class="ReasonForVisitGroupTable2">
                            <td id="trRFVSecondary2ContentOption" class="rbSelectRFVGroup">
                                <asp:RadioButton runat="server" GroupName="rbSelectRFVGroup" ID="rdoRFVSecondary2" AutoPostBack="true" CssClass="rbChoice" />
                            </td>
                            <td id="trRFVSecondary2ContentError" valign="top">
                                <asp:Label ID="lblRFVSecondary2ContentError" runat="server" CssClass="validateFailText" Visible="false" Text="*"></asp:Label>                                  
                            </td>                            
                        </tr>   
                        <tr id="trRFVSecondary3Content" runat="Server" class="ReasonForVisitGroupTable1">
                            <td id="trRFVSecondary3ContentOption" class="rbSelectRFVGroup">
                                <asp:RadioButton runat="server" GroupName="rbSelectRFVGroup" ID="rdoRFVSecondary3" AutoPostBack="true" CssClass="rbChoice" />
                            </td>
                            <td id="trRFVSecondary3ContentError" valign="top">
                                <asp:Label ID="lblRFVSecondary3ContentError" runat="server" CssClass="validateFailText" Visible="false" Text="*"></asp:Label>                                  
                            </td>                            
                        </tr>                                                                        
                    </table>
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0" class="textVersionTable">            
            <tr>
                <td style="padding-top: 4px">
                    <asp:Button ID="btnSelectReasonForVisitGroupBack" runat="server" Text="<%$ Resources:AlternateText, BackBtn %>"  />
                    <asp:Button ID="btnSelectReasonForVisitGroupAdd" runat="server" Text="<%$ Resources:AlternateText, AddBtn %>" />
                    <asp:Button ID="btnSelectReasonForVisitGroupEdit" runat="server" Text="<%$ Resources:AlternateText, EditBtn %>" />
                    <asp:Button ID="btnSelectReasonForVisitGroupDelete" runat="server" Text="<%$ Resources:AlternateText, DeleteBtn %>" />
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vSelectReasonForVisitL1" runat="server">
        <table cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td>
                    <asp:Label ID="lblSelectReasonForVisitL1Header" runat="server" CssClass="tableTitle"
                        Text="<%$ Resources:Text, ReasonVisit %>" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButtonList ID="rbSelectReasonForVisitL1" runat="server" CssClass="tableText" />
                </td>
            </tr>
            <tr>
                <td style="padding-top: 4px">
                    <asp:Button ID="btnSelectReasonForVisitL1Back" runat="server" Text="<%$ Resources:AlternateText, CancelBtn %>" />
                    <asp:Button ID="btnSelectReasonForVisitL1Next" runat="server" Text="<%$ Resources:AlternateText, NextBtn %>" />
                    <asp:Button ID="btnSelectReasonForVisitL1Confirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn %>" />
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vSelectReasonForVisitL2" runat="server">
        <table cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td>
                    <asp:Label ID="lblSelectReasonForVisitL2Header" runat="server" CssClass="tableTitle"
                        Text="<%$ Resources:Text, ReasonVisit %>" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSelectReasonForVisitL1Value" runat="server" CssClass="tableTitle" Font-Italic="true" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:RadioButtonList ID="rbSelectReasonForVisitL2" runat="server" CssClass="tableText" />
                </td>
            </tr>
            <tr>
                <td style="padding-top: 4px">
                    <asp:Button ID="btnSelectReasonForVisitL2Back" runat="server" Text="<%$ Resources:AlternateText, BackBtn %>" />
                    <asp:Button ID="btnSelectReasonForVisitL2Confirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn %>" />
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vSelectReasonForVisitGroupDelete" runat="server">
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <asp:Label ID="lblSelectReasonForVisitGroupDeleteHeader" runat="server" CssClass="tableTitle"
                        Text="<%$ Resources:Text, ReasonVisit %>" />
                </td>
            </tr>
            <tr>
                <td>
                    <table cellpadding="0" cellspacing="0" id="tblConfirmBoxContainer" runat="server"
                        class="tableRemark">
                        <tr>
                            <td>
                                <asp:Label ID="lblDeleteWarning" runat="server" Text="<%$ Resources:AlternateText, ReasonForVisitDeleteWarning %>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="padding-top: 5px">
                
                </td>
            </tr>
        </table>
        <table id="Table1" class="ReasonForVisitTable" cellpadding="0" cellspacing="1" runat="server">
            <tr id="trRFVDelete" runat="Server">
                <td>
                    <table id="tblRFVDelete" cellpadding="3" cellspacing="0" class="ReasonForVisitTableHeading"
                        runat="server">
                        <tr id="trRFVDeleteHeading" runat="Server">
                            <td class="VR">
                                <asp:Label ID="lblRFVDeleteText" runat="server" CssClass="tableText" Font-Bold="true"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trRFVDeleteContent" runat="Server" class="ReasonForVisitGroupTable1">
                            <td>
                                <table cellpadding="2" cellspacing="0" class="rbSelectRFVGroupDisplay">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRFVDeleteL1" runat="server" CssClass="tableText" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblRFVDeleteL2" runat="server" CssClass="tableText" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td style="padding-top: 5px">
                    <asp:Button ID="btnSelectReasonForVisitGroupDeleteCancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn %>" />
                    <asp:Button ID="btnSelectReasonForVisitGroupDeleteConfirm" runat="server" Text="<%$ Resources:AlternateText, ConfirmBtn %>" />
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
