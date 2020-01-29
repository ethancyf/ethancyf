<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPageNonLogin.master" CodeBehind="ChangeEmail.aspx.vb" Inherits="HCSP.ChangeEmail" 
    title="<%$ Resources:Title, ChangeEmail %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>


 <asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
                    &nbsp;
                    <asp:Image ID="imgHeaderAccountActivation" runat="server" AlternateText="<%$ Resources:AlternateText, ChangeEmailConfirmBanner%>"
                        ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ChangeEmailConfirmBanner%>" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>                       
                <table style="width: 100%"  border="0" cellpadding="0" cellspacing="0">
                        <tr style="background-image: url(../Images/master/background.jpg); background-position: bottom; background-repeat: repeat-x; height: 546px" valign="top">
                            <td style="width: 10px">
                            </td>
                            <td>             
                        <cc2:MessageBox ID="udcErrorMessage" runat="server" />
                        <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" />
                                    
                         <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
        <asp:View ID="Step1" runat="server">
            <table style="width: 100%">
                        <tr>
                            <td valign="top" colspan="4">
                            </td>                            
                        </tr>
                        </table>
                                <asp:Label ID="lbl_accAct_step1_desc" runat="server" Text="<%$ Resources:Text, AccountInfo %>" CssClass="tableText" Visible="False"></asp:Label><br />
                        <table style="width: 100%" border="0">
                        <tr>
                            <td valign="top" style="width: 251px">
                                <asp:Label ID="lbl_accAct_SPID" runat="server" Text="<%$ Resources:Text, SPIDOrUsername %>" CssClass="tableTitle"></asp:Label></td>
                            <td valign="top">
                                <asp:TextBox ID="txt_accAct_SPID" runat="server" MaxLength="20" Width="70px" onChange="convertToUpper(this);"></asp:TextBox><asp:Image
                                    ID="img_err_spid" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" />
                                <cc1:FilteredTextBoxExtender ID="filteredit_spID" runat="server" FilterType="Numbers,UpperCaseLetters,LowerCaseLetters"
                                    TargetControlID="txt_accAct_SPID">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                            <tr>
                                <td style="width: 251px; height: auto;" valign="top">
                                    <asp:Label ID="Label1" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Password %>" Height="20px" Visible="False"></asp:Label></td>
                                <td valign="top" style="height: auto">
                                    <asp:TextBox ID="txt_password" runat="server" MaxLength="20" TextMode="Password"
                                        Width="70px" Visible="False"></asp:TextBox><asp:Image
                                    ID="img_err_Password" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                    Visible="False" /></td>
                            </tr>
                        <tr valign="top">
                            <td valign="middle" style="width: 251px">
                                <asp:Label ID="lbl_accAct_tokenPIN" runat="server" Text="<%$ Resources:Text, PinNo %>" CssClass="tableTitle"></asp:Label></td>
                            <td valign="middle" style="height: 70px">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td valign="middle">
                                <asp:TextBox ID="txt_accAct_tokenPIN" runat="server" MaxLength="6" TextMode="Password" Width="70px"></asp:TextBox><asp:Image
                                    ID="img_err_tokenPIN" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                    Visible="False" />
                                            <cc1:FilteredTextBoxExtender ID="filteredit_tokenPIN" runat="server" FilterType="Numbers"
                                    TargetControlID="txt_accAct_tokenPIN" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td valign="top">
                                            &nbsp;&nbsp;
                                <asp:Image ID="Image1" runat="server" ImageUrl="<%$ Resources:ImageUrl, Token %>" AlternateText="<%$ Resources:AlternateText, Token %>" ImageAlign="Bottom"/></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                            <tr valign="top">                                
                                <td valign="middle" colspan="2" align="center">
                                <asp:ImageButton ID="btn_accAct_step1_next" runat="server" ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" AlternateText="<%$ Resources:AlternateText, NextBtn %>" OnClick="btn_accAct_step1_next_Click"/></td>
                            </tr>
                    </table>
            <br />
        </asp:View>
        <asp:View ID="StepFinal" runat="server" EnableTheming="False">
            <table style="width: 100%">
                        <tr>
                            <td valign="top" colspan="4">
                                </td>                            
                        </tr>
                        <tr>
                            <td colspan="4" valign="top">                                
                                <asp:Label ID="lbl_final1" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ConfirmEmailFinal1 %>"></asp:Label>
                                <asp:Label
                                    ID="lbl_final2" runat="server" CssClass="tableTitle" Font-Bold="True" ></asp:Label></td>
                        </tr>
                        <tr>
                            <td colspan="4" style="height: 50px">                                
                                <asp:Label ID="lbl_accAct_finalLink1" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, FinalLink1 %>"></asp:Label>&nbsp<asp:HyperLink ID="lbl_accAct_finalLink2" runat="server"  NavigateUrl="<%$ Resources:Text, FinalLink2 %>" Text="<%$ Resources:Text, FinalLink2 %>"></asp:HyperLink>&nbsp<asp:Label ID="lbl_accAct_finalLink3" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, FinalLink3 %>"></asp:Label></td>
                        </tr>                         
                    </table>
            <br />
        </asp:View>
        <asp:View ID="StepInvalidLink" runat="server">
        </asp:View>
    </asp:MultiView>           
   
                        </td>
                        </tr>
                 </table>                   
            </ContentTemplate>
        </asp:UpdatePanel>

</asp:Content>