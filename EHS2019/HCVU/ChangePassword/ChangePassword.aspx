<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="ChangePassword.aspx.vb" Inherits="HCVU.ChangePassword" 
    title="<%$ Resources:Title, ChangePassword %>" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../JS/Common.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
        <asp:Panel ID="pnlChangePassword" runat="server" DefaultButton="ibtnConfirm">
            <table cellpadding="0" cellspacing="0" style="width: 980px;" border="0">
                <tr>
                    <td style="height: 60px; width: 980px;" valign="top"  colspan="2">
                         <asp:Image ID="imgHeader" runat="server" AlternateText="<%$Resources:AlternateText, ChangePasswordBanner%>"
                            ImageAlign="AbsMiddle" ImageUrl="<%$Resources:ImageUrl, ChangePasswordBanner%>" />
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Type="Complete" Width="780px" />
                        <cc2:MessageBox ID="udcMessageBox" runat="server" Width="780px" />
                    </td>
                </tr>
            </table>
            <asp:Panel ID="pnlChangePasswordContent" runat="server">
                    <table style="WIDTH: 800px;" cellspacing="1" cellpadding="1" border="0">
                        <tr>
                            <td style="WIDTH: 198px; height: 30px;" align="left"><asp:Label ID="lblLoginIDText" runat="server" Text="<%$Resources:Text, LoginID%>"></asp:Label></td>
                            <td style="WIDTH: 493px; height: 30px;" align="left">
	                            <asp:Label id="lblUserName" runat="server" Text="UserA" CssClass="tableText"></asp:Label>
					        </td>
                        </tr>
                        <tr>
                            <td align="left"><asp:Label ID="lblOldPasswordText" runat="server" Text="<%$Resources:Text, OldPassword%>"></asp:Label></td>
                            <td align="left">
	                            <asp:TextBox id="txtOldPassword" runat="server" MaxLength="20" TextMode="Password" Width="200px"></asp:TextBox>
	                            <asp:Image id="imgOldPasswordAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="false"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>"/>
					        </td>    					
                        </tr>
                        <tr style="height: 35px">
                            <td align="left"><asp:Label ID="lblNewPasswordText" runat="server" Text="<%$Resources:Text, NewPassword%>"></asp:Label></td>
                            <td align="left">
	                            <table cellpadding="0" cellspacing="0" border="0" style="width: 550px">
	                                <tr>
	                                    <td style="width: 240px">
	                                        <asp:TextBox id="txtNewPassword" runat="server" MaxLength="20" TextMode="Password" Width="200px"></asp:TextBox>
	                                        <asp:Image id="imgNewPasswordAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="false"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>"/>
	                                    </td>
                                        <td valign="top">
                                        <table style="width: 300px" cellpadding="0">
                                            <tr>
                                                <td colspan="5">
                                                    <div id="progressBar" style="font-size: 1px; height: 10px; width: 290px; border: 1px solid white;" />
                                                </td>
                                            </tr>
                                            <tr style="width: 300px">
                                                <td align="center" style="width:30%">
                                                    <span id="strength1"></span>                                           
                                                </td>
                                                <td style="width:5%">
                                                    <span id="direction1"></span>
                                                </td>
                                                <td align="center" style="width:30%">
                                                    <span id="strength2"></span>
                                                </td>
                                                <td style="width:5%">
                                                    <span id="direction2"></span>
                                                </td>
                                                <td align="center" style="width:30%">
                                                    <span id="strength3"></span>
                                                </td>                                        
                                            </tr>
                                        </table>                               
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="HEIGHT: 30px" align="left"><asp:Label ID="Label2" runat="server" Text="<%$Resources:Text, ConfirmPW%>"></asp:Label></td>
                            <td style="HEIGHT: 31px" align="left">
	                            <asp:TextBox id="txtNewPasswordConfirm" runat="server" MaxLength="20" TextMode="Password" Width="200px"></asp:TextBox>
                                <asp:Image id="imgNewPasswordConfirmAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="false"  AlternateText="<%$ Resources:AlternateText, ErrorImg %>"/>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" align="left">
                                <p><br/>
                                <asp:Label ID="lblWebPasswordTips" runat="server" Text="<%$ Resources:Text, WebPasswordTips %>"></asp:Label>
                                <br />
                                <asp:Label ID="lblWebPasswordTips1" runat="server" Text="<%$ Resources:Text, WebPasswordTips1 %>"></asp:Label><br />
                                &nbsp; &nbsp;&nbsp;<asp:Label ID="lblWebPasswordTips1a" runat="server" Text="<%$ Resources:Text, WebPasswordTips1a %>"></asp:Label><br />
                                &nbsp; &nbsp;&nbsp;<asp:Label ID="lblWebPasswordTips1b" runat="server" Text="<%$ Resources:Text, WebPasswordTips1b %>"></asp:Label><br />
                                &nbsp; &nbsp;&nbsp;<asp:Label ID="lblWebPasswordTips1c" runat="server" Text="<%$ Resources:Text, WebPasswordTips1c %>"></asp:Label><br />
                                &nbsp; &nbsp;&nbsp;<asp:Label ID="lblWebPasswordTips1d" runat="server" Text="<%$ Resources:Text, WebPasswordTips1d %>"></asp:Label><br />
                                <asp:Label ID="lblWebPasswordTips2" runat="server" Text="<%$ Resources:Text, WebPasswordTips2 %>"></asp:Label><br />
                                <asp:Label ID="LabelWebPasswordTips3" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>"></asp:Label>
                                <br /><br />
                            </td>
                        </tr>
                    </table> 
                    <%--<cc1:FilteredTextBoxExtender ID="FilteredOldPassword" runat="server" TargetControlID="txtOldPassword"
                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                    </cc1:FilteredTextBoxExtender>--%>
                    <cc1:FilteredTextBoxExtender ID="FilteredNewPassword" runat="server" TargetControlID="txtNewPassword"
                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                    </cc1:FilteredTextBoxExtender>
                    <cc1:FilteredTextBoxExtender ID="FilteredNewPasswordConfirm" runat="server" TargetControlID="txtNewPasswordConfirm"
                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                    </cc1:FilteredTextBoxExtender>
            </asp:Panel>            
                    
                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                        <tr>
                            <td style="width: 120px">
                                <asp:ImageButton Visible="false" ID="ibtnBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" AlternateText="<%$ Resources:AlternateText, BackBtn %>" />
                            </td>
                            <td align="center" style="width: 700px">
                                <asp:ImageButton ID="ibtnConfirm" runat="server" ImageUrl="~/Images/button/btn_confirm.png" AlternateText="Confirm" />
                            </td>
                        </tr>
                    </table>        
        </asp:Panel>   

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
