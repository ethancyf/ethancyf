<%@ Page Language="vb" AutoEventWireup="false" Codebehind="main.aspx.vb" Inherits="VoucherBalanceEnquiry.Text.main" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc3" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="WebControlCaptcha" Namespace="WebControlCaptcha" TagPrefix="cc2" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="PageTitle" runat="server"></title>    
    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>     
    <script type="text/javascript">
        function convertToUpper(textbox) { textbox.value = textbox.value.toUpperCase(); }
        function formatHKID(textbox) {
            textbox.value = textbox.value.toUpperCase();
            txt = textbox.value;
            var res = "";
            if ((txt.length == 8) || (txt.length == 9)) {
                if ((txt.indexOf("(") < 0) && (txt.indexOf(")") < 0)) {
                    res = txt.substring(0, txt.length - 1) + "(" + txt.substring(txt.length - 1, txt.length) + ")";
                }
                else {
                    res = txt;
                }
                textbox.value = res;
            }
            return false;
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <div>
            <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td style="width: 100%" valign="top">
                        <asp:Label Font-Bold="False" ID="lblBannerText" runat="server" Height="16px" Text="<%$ Resources:Text, EVoucherSystem %>"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:HyperLink ID="hlFullVersion" runat="server" NavigateUrl="~/main.aspx" Text="<%$ Resources:Text, FullVersion %>" Style="display:inline"/>
                        <asp:LinkButton ID="lnkbtnTradChinese" runat="server">繁體</asp:LinkButton>
                        <asp:LinkButton ID="lnkbtnEnglish" runat="server" Enabled="False">English</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td bgcolor="silver">
                        <asp:Label ID="lblHeaderText" runat="server" Font-Underline="False" Font-Bold="False"
                            Text="<%$ Resources:Text, VoucherEnquiry %>" BackColor="Transparent"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <cc3:TextOnlyMessageBox ID="udcTextOnlyMessageBox" runat="server" />
                        <cc3:TextOnlyInfoMessageBox ID="udcTextOnlyInfoMessageBox" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:MultiView ID="MultiViewEnquiry" runat="server" ActiveViewIndex="0">
                <asp:View ID="ViewSearch" runat="server">
                    <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblHeaderNote" runat="server" Text="<%$ Resources:Text, VoucherBalanceEnquiryNote %>"
                                                Font-Italic="true"></asp:Label><br />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rboHKIDType" runat="server" RepeatDirection="Vertical" AutoPostBack="True">
                                                <asp:ListItem Text="<%$ Resources:Text, HKICLongForm %>" Value="Y" Selected="True"></asp:ListItem>
                                                <asp:ListItem Text="<%$ Resources:Text, ExemptionCert %>" Value="N"></asp:ListItem>
                                            </asp:RadioButtonList>
                                            <br />
                                            <asp:Label ID="lblPatsHKIDText" runat="server" Font-Bold="True" Text="<%$ Resources:Text, HKID %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtPatsHKID" runat="server" MaxLength="11" Width="100px"></asp:TextBox><asp:Label
                                                ID="lblPatsHKIDAlert" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPatSearchHKIDHint" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, HKICHint %>"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblPatsDOBText" runat="server" CssClass="labelText" Font-Bold="True" Text="<%$ Resources:Text, DOBLongForm %>" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Panel ID="pnlHKIDDOB" runat="server">
                                                <asp:TextBox ID="txtHKIDDOB" runat="server" MaxLength="10" Width="100px"></asp:TextBox><asp:Label
                                                    ID="lblHKIDDOBAlert" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                            </asp:Panel>
                                            <asp:Panel ID="pnlECDOB" runat="server">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td>
                                                            <asp:RadioButton ID="rboDOB" runat="server" Text="" AutoPostBack="true" />
                                                        </td>
                                                        <td>
                                                            <asp:TextBox ID="txtPatsDOB" runat="server" MaxLength="10" Width="100px"></asp:TextBox><asp:Label
                                                                ID="lblPatsDOBAlert" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                            <asp:Label ID="lblOr1" runat="server" Text="<%$ Resources:Text, Or %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td valign="top">
                                                            <asp:RadioButton ID="rboECAge" runat="server" Text="" AutoPostBack="true" />
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="lblAge" runat="server" Text="<%$ Resources:Text, Age %>"></asp:Label>
                                                            <asp:TextBox ID="txtPatsAge" runat="server" MaxLength="3" Width="40px"></asp:TextBox>
                                                            <asp:Label ID="lblRegisterOn" runat="server" Text="<%$ Resources:Text, RegisterOn %>"></asp:Label><asp:Panel
                                                                ID="pnlEngECAge" runat="server">
                                                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <tr>
                                                                        <td style="height: 62px">
                                                                            <table border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtRegisterOnDay" runat="server" MaxLength="2" Width="30px"></asp:TextBox>&nbsp;</td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="ddlRegisterOnMonth" runat="server">
                                                                                            <asp:ListItem></asp:ListItem>
                                                                                            <asp:ListItem Value="1">January</asp:ListItem>
                                                                                            <asp:ListItem Value="2">February</asp:ListItem>
                                                                                            <asp:ListItem Value="3">March</asp:ListItem>
                                                                                            <asp:ListItem Value="4">April</asp:ListItem>
                                                                                            <asp:ListItem Value="5">May</asp:ListItem>
                                                                                            <asp:ListItem Value="6">June</asp:ListItem>
                                                                                            <asp:ListItem Value="7">July</asp:ListItem>
                                                                                            <asp:ListItem Value="8">August</asp:ListItem>
                                                                                            <asp:ListItem Value="9">September</asp:ListItem>
                                                                                            <asp:ListItem Value="10">October</asp:ListItem>
                                                                                            <asp:ListItem Value="11">November</asp:ListItem>
                                                                                            <asp:ListItem Value="12">December</asp:ListItem>
                                                                                        </asp:DropDownList>&nbsp;</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtRegisterOnYear" runat="server" MaxLength="4" Width="40px"></asp:TextBox>&nbsp;</td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblECDay_e" runat="server" onclick="javascript:ChangeInputDOBFormat('Age');"
                                                                                            Text="<%$ Resources:Text, Day %>"></asp:Label></td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblECMonth_e" runat="server" Text="<%$ Resources:Text, Month %>"></asp:Label></td>
                                                                                    <td align="center">
                                                                                        <asp:Label ID="lblECYear_e" runat="server" onclick="javascript:ChangeInputDOBFormat('Age');"
                                                                                            Text="<%$ Resources:Text, Year %>"></asp:Label></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:Panel>
                                                            <asp:Panel ID="pnlChiECAge" runat="server">
                                                                <asp:TextBox ID="txtRegisterOnYearChi" runat="server" MaxLength="4" Width="40px"></asp:TextBox>
                                                                <asp:Label ID="lblECYear" runat="server" Text="<%$ Resources:Text, Year %>"></asp:Label>
                                                                <asp:DropDownList ID="ddlRegisterOnMonthChi" runat="server">
                                                                    <asp:ListItem></asp:ListItem>
                                                                    <asp:ListItem Value="1">1</asp:ListItem>
                                                                    <asp:ListItem Value="2">2</asp:ListItem>
                                                                    <asp:ListItem Value="3">3</asp:ListItem>
                                                                    <asp:ListItem Value="4">4</asp:ListItem>
                                                                    <asp:ListItem Value="5">5</asp:ListItem>
                                                                    <asp:ListItem Value="6">6</asp:ListItem>
                                                                    <asp:ListItem Value="7">7</asp:ListItem>
                                                                    <asp:ListItem Value="8">8</asp:ListItem>
                                                                    <asp:ListItem Value="9">9</asp:ListItem>
                                                                    <asp:ListItem Value="10">10</asp:ListItem>
                                                                    <asp:ListItem Value="11">11</asp:ListItem>
                                                                    <asp:ListItem Value="12">12</asp:ListItem>
                                                                </asp:DropDownList><asp:Label ID="lblECMonth" runat="server" Text="<%$ Resources:Text, Month %>"></asp:Label>
                                                                <asp:TextBox ID="txtRegisterOnDayChi" runat="server" MaxLength="2" Width="30px"></asp:TextBox>
                                                                <asp:Label ID="lblECDay" runat="server" Text="<%$ Resources:Text, Day %>"></asp:Label></asp:Panel>
                                                            <asp:Label ID="lblPatsECAgeAlert" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:Panel>
                                            <asp:Label ID="lblPatSearchDOBHint" runat="server" CssClass="tableTitle"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width: 251px">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width: 251px">
                                            <asp:HiddenField ID="hfCaptchaControlAlert" runat="server" />
                                            <cc2:CaptchaControl ID="CaptchaControl" runat="server" IsIntegerOnly="true" CaptchaLineNoise="Low"
                                                CaptchaBackgroundNoise="None" CaptchaHeight="35" CaptchaWidth="240" Text="Type the code shown:"
                                                CaptchaChars="123" Width="240px"></cc2:CaptchaControl>
                                            <asp:Label ID="lblCaptchaControlAlert" runat="server" Text="*" Visible="false" ForeColor="red"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="width: 251px">
                                            <table border="0" cellpadding="0" cellspacing="0" style="width: 240px">
                                                <tr>
                                                    <td align="left">
                                                        <asp:LinkButton ID="lnkbtnOtherCaptcha" runat="server" Width="240px" Text="<%$ Resources:Text, CaptchaTryDiffImage %>"></asp:LinkButton></td>
                                                </tr>
                                                <tr>
                                                    <td align="left">
                                                        <asp:Button ID="btnSearchPat" runat="server" Text="<%$ Resources:Text, Search %>" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="ViewResult" runat="server">
                    <br />
                    <table cellpadding="0" cellspacing="0" style="width:100%;border-style:double;border-width:2px;border-color:lightgreen">
                        <tr>
                            <td>
                                <asp:Label ID="lblDisplayHKIDText" runat="server" CssClass="labelText" Font-Bold="True" style="padding-left:4px"
                                    Text="<%$ Resources:Text, HKID %>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDisplayHKID" runat="server" CssClass="labelText" style="padding-left:4px"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDisplayDOBText" runat="server" CssClass="labelText" Font-Bold="True" style="padding-left:4px"
                                    Text="<%$ Resources:Text, DOBLongForm %>"/>
                            </td>
                        </tr>
                        <tr style="height:35px;vertical-align:top">
                            <td>
                                <asp:Label ID="lblDisplayDOB" runat="server" CssClass="labelText" style="padding-left:4px"/>
                                <asp:Label ID="lblDisplayDOB_Chi" runat="server" CssClass="labelText" style="padding-left:4px"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCurrentPositionHeading" runat="server" CssClass="labelText" Font-Bold="True" style="position:relative;left:4px;padding-right:4px;text-decoration:underline"
                                    Text="<%$ Resources:Text, CurrentPositionHeading %>" />
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                <div style="position:relative;left:4px;padding-right:4px">
                                <asp:Label ID="lblDisplayVoucherValueText" runat="server" CssClass="labelText" Font-Bold="True" 
                                    Text="<%$ Resources:Text, AvailableVoucherVR %>" />
                                <asp:Label ID="lblNoteText" runat="server" CssClass="labelText" Text="<%$ Resources:Text, Note_Short %>" style="position:relative;top:-4px;font-size:12px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDisplayVoucherValue" runat="server" CssClass="labelText" style="padding-left:4px"/>
                            </td>
                        </tr>
                        <tr id="trMaximumVoucherAmount" runat="server" style="vertical-align:top">                            
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMaximumVoucherAmountText" runat="server" CssClass="labelText" Font-Bold="True" style="position:relative;left:4px;padding-right:4px"/>
                                            <asp:Label ID="lblMaximumVoucherAmountText_Chi" runat="server" CssClass="labelText" Font-Bold="True" style="position:relative;left:4px;padding-right:4px"/>
                                            <br />
                                            <asp:Label ID="lblMaximumVoucherAmountNote" runat="server" CssClass="labelText" Font-Size="16px" style="position:relative;left:4px;padding-right:4px"/>
                                            <asp:Label ID="lblMaximumVoucherAmountNote_Chi" runat="server" CssClass="labelText" Font-Size="16px" style="position:relative;left:4px;padding-right:4px"/>
                                         </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblMaximumVoucherAmount" runat="server" CssClass="labelText" style="padding-left:4px"/>
                                        </td>
                                    </tr>
                                </table>                            
                            </td>
                        </tr>

                    </table>
                    <br />
                    <table cellpadding="0" cellspacing="0" style="width:100%;border-style:double;border-width:2px;border-color:#bfa7ce">
                        <tr>
                            <td>
                                <asp:Label ID="lblNextPositionHeading1" runat="server" CssClass="labelText" Font-Bold="True"
                                    Text="<%$ Resources:Text, NextPositionHeading1 %>" style="position:relative;left:4px;padding-right:4px;text-decoration:underline"/>                                
                                <br />
                                <asp:Label ID="lblNextPositionHeading2" runat="server" CssClass="labelText" Font-Bold="True"
                                    Text="<%$ Resources:Text, NextPositionHeading2 %>" style="position:relative;left:4px;padding-right:4px"/>
                                <br />
                                <br />
                                <asp:Label ID="lblAnnualAllotmentText" runat="server" CssClass="labelText" Font-Bold="True" style="position:relative;left:4px;padding-right:4px"/>
                                <asp:Label ID="lblAnnualAllotmentText_Chi" runat="server" CssClass="labelText" Font-Bold="True" style="position:relative;left:4px;padding-right:4px"/>
                                <br />
                                <asp:Label ID="lblAnnualAllotment" runat="server" CssClass="labelText" style="position:relative;left:4px;padding-right:4px"/>
                                <br />
                                <asp:Label ID="lblForfeitVoucherText" runat="server" CssClass="labelText" Font-Bold="True" style="position:relative;left:4px;padding-right:4px"/>
                                <asp:Label ID="lblForfeitVoucherText_Chi" runat="server" CssClass="labelText" Font-Bold="True" style="position:relative;left:4px;padding-right:4px"/>
                                <br />
                                <asp:Label ID="lblForfeitVoucherNote" runat="server" CssClass="labelText" Font-Size="16px" style="position:relative;left:4px;padding-right:4px"/>
                                <asp:Label ID="lblForfeitVoucherNote_Chi" runat="server" CssClass="labelText" Font-Size="16px" style="position:relative;left:4px;padding-right:4px"/>
                                <br />
                                <asp:Label ID="lblForfeitVoucher" runat="server" CssClass="labelText" style="position:relative;left:4px;padding-right:4px"/>
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table id="tblQuotaReference" runat="server" cellpadding="0" cellspacing="0" style="width:100%;border-style:double;border-width:2px;border-color:lightblue">              
                        <tr style="vertical-align:top">                            
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblForReference" runat="server" CssClass="labelText" Font-Bold="True" Text="<%$ Resources:Text, ForReference %>" style="padding-left:4px;padding-right:6px;text-decoration:underline"/>                                
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDisplayAvailableQuotaText" runat="server" CssClass="labelText" Font-Bold="True" style="position:relative;left:4px"/>
                                            <asp:Label ID="lblDisplayAvailableQuotaText_Chi" runat="server" CssClass="labelText" Font-Bold="True" style="position:relative;left:4px"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblDisplayAvailableQuota" runat="server" CssClass="labelText" style="padding-left:4px"/>
                                            <asp:Label ID="lblDisplayAvailableQuotaUpto" runat="server" CssClass="labelText" style="padding-left:4px"/>
                                            <asp:Label ID="lblDisplayAvailableQuotaUpto_Chi" runat="server" CssClass="labelText" style="padding-left:4px"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td width="90%">
                                            <asp:Label ID="lblDisplayQuotaNote" runat="server" CssClass="labelText" Font-Size="16px" Text="<%$ Resources:Text, QuotaNote %>"  style="position:relative;left:4px;padding-right:6px" />
                                        </td>
                                    </tr>
                                </table>                               
                            </td>
                        </tr>
                    </table>
                    <br />
                    <table cellpadding="0" cellspacing="0" style="width: 100%;border-style:double;border-width:0px">
                        <tr>
                            <td align="left">
                                <%--<input type="button" id="ibtnClose" runat="server" value="close" onclick="window.opener='X';window.open('','_parent','');window.close();" />--%>
                                <asp:Button ID="btnComplete" runat="server" Text="<%$ Resources:AlternateText, CompleteBtn %>"/>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <br />
                                <asp:Literal ID="lblNoteValue" runat="server" Text="<%$ Resources:Text, HCVR_Note %>"/>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vReminderObsoleteOS" runat="server">
                <table border="0" cellpadding="0" cellspacing="0" runat="server" >
                    <tr>
                        <td>
                            <table id="tblReminderObsoleteOS" border="0" cellpadding="0" cellspacing="0" runat="server" width="100%">
                                <tr>
                                    <td style="width: 100%">
                                        <span style="font-size: 12pt;">
                                            <asp:Label ID="lblReminderObsoleteOSContent" runat="server" Text="<%$ Resources:Text, ReminderWindowsVersion %>" />
                                        </span>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 4px">
                            <asp:Button ID="btnReminderObsoleteOSOK" runat="server" Text="<%$ Resources:AlternateText, OKBtn%>" />
                        </td>
                    </tr>
                </table>
            </asp:View>
            </asp:MultiView>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td style="height: 19px">
                        <asp:HyperLink ID="hlContactus" runat="server" NavigateUrl="~/text/main.aspx" Visible="False"
                            Text="<%$ Resources:Text, ContactUs %>"></asp:HyperLink>&nbsp;
                        <asp:HyperLink ID="hlFaqs" runat="server" NavigateUrl="~/text/main.aspx" Visible="false"
                            Text="<%$ Resources:Text, Faqs %>"></asp:HyperLink>&nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
