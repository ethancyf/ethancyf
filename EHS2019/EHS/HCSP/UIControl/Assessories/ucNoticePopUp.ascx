<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucNoticePopUp.ascx.vb"
    Inherits="HCSP.ucNoticePopUp" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script type="text/javascript">
   
</script>

<table id="tblPopup" runat="server" border="0" cellpadding="0" cellspacing="0" width="100%">
    <%--Header --%>
    <tr>
        <td colspan="3">
            <asp:Panel ID="panHeader" runat="server" Style="cursor: move;">
                <%--<table style="padding: 0px;border-collapse: collapse; border-spacing: 0px;width: 100%;">--%>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td id ="tdHeadingLeft" runat="server" style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px;">
                        </td>
                        <td id ="tdHeadingTop" runat="server" style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                            color: #ffffff; background-repeat: repeat-x; height: 35px;">
                            <asp:Label ID="lblHeader" runat="server"></asp:Label></td>
                        <td id ="tdHeadingRight" runat="server" style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px;">
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
            <cc2:MessageBox ID="udcMsgBoxErr" runat="server" Width="99%" style="position:relative;left:5px;top:5px"></cc2:MessageBox>
            <table style="padding: 0px;border-collapse: collapse; border-spacing: 0px;width:100%;padding-top:5px; padding-bottom:5px; padding-left:5px; padding-right:5px;">
                <tr>
                    <td>
                        <%------------------- Notification -----------------%>
                        <%---------------------------------------------%>
                        <table>
                            <colgroup />
                            <%-- Display message --%>
                            <tr>
                                <td style="text-align:right;width: 5%; height: 42px; padding-right:10px;" valign="top">
                                    <asp:Image ID="imgIcon" runat="server" ImageUrl="" /></td>
                                <td colspan="2" style="text-align:left;width: 85%; height: 42px">
                                    <asp:Label ID="lblMsg" runat="server" Style="font-family: Arial;
                                        font-weight: bold; font-size: 14px; color: #666666;" 
                                        BorderStyle="none"></asp:Label>
                                </td>
                            </tr>
                            <%-- CheckBox --%>
                            <asp:Panel ID="panDeclaration" runat="server" Visible="false">
                            <tr>
                                <td style="width: 5%; height: 30px;padding-top:15px; padding-right:10px;vertical-align:top;text-align:right" />
                                <td style="width: 3%; height: 30px;padding-top:10px;text-align:left;vertical-align:top">
                                     <asp:Checkbox ID="chkDeclaration" runat="server" AutoPostBack ="true" />  
                                </td>
                                <td style="width: 82%; height: 30px;padding-top:15px;padding-right:10px;text-align:left">
                                     <%--<asp:Panel ID="panDeclarationContent" runat="server" Visible="true" />--%>

                                     <div ID="divDeclaration" runat="server" 
                                         style="position:relative;top:-2px;" />    

                                     <%--<asp:Label ID="lblDeclaration" runat="server" Style="font-family: Arial;
                                            font-weight: bold; font-size: 14px; color: #666666;position:relative;top:-2px;display:inline" 
                                            BorderStyle="none" />--%>
                                </td>
                            </tr>
                            </asp:Panel>
                            <%-- Label - Enquiry --%>
                            <asp:Panel ID="panEnquiry" runat="server" Visible="false">
                            <tr>
                                <td style="width: 5%; vertical-align:top;text-align:right" />
                                <td colspan="2" style="width: 85%; text-align:left">
                                     <div ID="divEnquiryDesc" runat="server" style="position:relative;top:-2px;padding-top:5px" />    
                                </td>
                            </tr>
                            </asp:Panel>
                            <%-- Textbox - Override Reason --%>
                            <asp:Panel ID="panOverrideReason" runat="server" Visible="false">
                            <tr>
                                <td style="width: 5%; vertical-align:top;text-align:right" />
                                <td colspan="2" style="width: 85%; text-align:left">
                                     <div ID="divORReasonDesc" runat="server" style="position:relative;top:-2px;" />    
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 5%; vertical-align:top;text-align:right" />
                                <td colspan="2" style="width: 85%; text-align:left">                                   
                                    <asp:Label ID="lblORReason" runat="server" />
                                    <asp:Textbox ID="txtORReason" runat="server" style="width:400px" MaxLength="100" />

                             <cc1:FilteredTextBoxExtender ID="fteORReasonBlockVerticalBarAndBackslash" runat="server" TargetControlID="txtORReason"
                                      FilterMode="InvalidChars"  InvalidChars="|\&quot;" Enabled="False"></cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            </asp:Panel>
                            <%-- Button --%>
                            <tr>
                                <td colspan="3" style="width: 90%;padding-top:10px;text-align:center" >
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
