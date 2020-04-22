<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="main.aspx.vb" Inherits="VoucherBalanceEnquiry.main" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="WebControlCaptcha" Namespace="WebControlCaptcha" TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc3" %>

<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title id="PageTitle" runat="server"></title>
    <base id="basetag" runat="server" />
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/DialogStyle.css" rel="stylesheet" type="text/css" />
    <script language="javascript" src="JS/Common.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        window.location.replace("https://apps.hcv.gov.hk/public/tc/VBE/Search");
    </script>

    <script language="javascript" type="text/javascript">


        function ChangeInputDOBFormat(str) {
            var rboDOB = document.getElementById('<%=rboDOB.ClientID%>');
            var rboECAge = document.getElementById('<%=rboECAge.ClientID%>');

            var txtPatsDOB = document.getElementById('<%=txtPatsDOB.ClientID%>');
            var txtPatsAge = document.getElementById('<%=txtPatsAge.ClientID%>');
            var txtRegisterOnDay = document.getElementById('<%=txtRegisterOnDay.ClientID%>');
            var ddlRegisterOnMonth = document.getElementById('<%=ddlRegisterOnMonth.ClientID%>');
            var txtRegisterOnYear = document.getElementById('<%=txtRegisterOnYear.ClientID%>');

            var txtRegisterOnDayChi = document.getElementById('<%=txtRegisterOnDayChi.ClientID%>');
            var ddlRegisterOnMonthChi = document.getElementById('<%=ddlRegisterOnMonthChi.ClientID%>');
            var txtRegisterOnYearChi = document.getElementById('<%=txtRegisterOnYearChi.ClientID%>');

            var lblPatSearchDOBHint = document.getElementById('<%=lblPatSearchDOBHint.ClientID%>');
            var lblPatSearchDOBHint1 = document.getElementById('<%=lblPatSearchDOBHint1.ClientID%>');
            var lblECDAgeTip = document.getElementById('<%=lblECDAgeTip.ClientID%>');

            if (str == 'DOB') {
                rboDOB.checked = true;
                rboECAge.checked = false;

                txtPatsDOB.readOnly = false;
                txtPatsDOB.style.backgroundColor = '';

                txtPatsAge.readOnly = true;
                txtPatsAge.value = '';
                txtPatsAge.style.backgroundColor = '#f5f5f5';

                txtRegisterOnDay.readOnly = true;
                txtRegisterOnDay.value = '';
                txtRegisterOnDay.style.backgroundColor = '#f5f5f5';

                txtRegisterOnYear.readOnly = true;
                txtRegisterOnYear.value = '';
                txtRegisterOnYear.style.backgroundColor = '#f5f5f5';

                txtRegisterOnDayChi.readOnly = true;
                txtRegisterOnDayChi.value = '';
                txtRegisterOnDayChi.style.backgroundColor = '#f5f5f5';

                txtRegisterOnYearChi.readOnly = true;
                txtRegisterOnYearChi.value = '';
                txtRegisterOnYearChi.style.backgroundColor = '#f5f5f5';

                ddlRegisterOnMonth.selectedIndex = 0
                ddlRegisterOnMonth.disabled = true;

                ddlRegisterOnMonthChi.selectedIndex = 0
                ddlRegisterOnMonthChi.disabled = true;

                lblPatSearchDOBHint.style.display = "block";
                lblPatSearchDOBHint1.style.display = "block";
                lblECDAgeTip.style.display = "none";

            }
            else if (str == 'Age') {
                rboDOB.checked = false;
                rboECAge.checked = true;

                txtPatsDOB.readOnly = true;
                txtPatsDOB.value = '';
                txtPatsDOB.style.backgroundColor = '#f5f5f5';

                txtPatsAge.readOnly = false;
                txtPatsAge.style.backgroundColor = '';

                txtRegisterOnDay.readOnly = false;
                txtRegisterOnDay.style.backgroundColor = '';

                txtRegisterOnYear.readOnly = false;
                txtRegisterOnYear.style.backgroundColor = '';

                ddlRegisterOnMonth.disabled = false;

                txtRegisterOnDayChi.readOnly = false;
                txtRegisterOnDayChi.style.backgroundColor = '';

                txtRegisterOnYearChi.readOnly = false;
                txtRegisterOnYearChi.style.backgroundColor = '';

                ddlRegisterOnMonthChi.disabled = false;

                lblPatSearchDOBHint.style.display = "none";
                lblPatSearchDOBHint1.style.display = "none";
                lblECDAgeTip.style.display = "block";
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" autocomplete="off">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <table id="tblBanner" border="0" cellpadding="0" cellspacing="0" style="background-image: url(Images/master/banner_header.jpg); width: 994px; background-repeat: no-repeat; height: 100px"
            runat="server">
            <tr>
                <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironment">
                    <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                </td>
                <td align="right" valign="top" style="white-space: nowrap">
                    <table cellpadding="0" cellspacing="0" border="0">
                        <tr>
                            <td>
                                <asp:LinkButton ID="lnkTextOnlyVesion" runat="server" CssClass="languageText" Text="<%$ Resources:Text, TextOnlyVersion %>"
                                    PostBackUrl="~/text/main.aspx"></asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkbtnTradChinese" runat="server" CssClass="languageText">繁體</asp:LinkButton><asp:LinkButton
                                    ID="lnkbtnSimpChinese" runat="server" CssClass="languageText" Visible="false">简体</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton ID="lnkbtnEnglish" runat="server" CssClass="languageText">English</asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <table border="0" cellpadding="0" cellspacing="0" style="width: 990px;">
            <tr>
                <td align="center">
                    <cc3:MessageBox ID="udcMessageBox" runat="server" Width="950px" Visible="false"></cc3:MessageBox>
                    <cc3:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="950px" Visible="false"></cc3:InfoMessageBox>
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="background-image: url(Images/master/top_left.jpg); width: 30px; height: 30px"></td>
                            <td style="background-image: url(Images/master/top.jpg); background-repeat: repeat-x"></td>
                            <td style="background-image: url(Images/master/top_right.jpg); width: 30px; height: 30px"></td>
                        </tr>
                        <tr>
                            <td style="background-image: url(Images/master/left.jpg); width: 30px; background-repeat: repeat-y"></td>
                            <td style="background-color: #fcfcfc" align="left">
                                <asp:Label ID="lblHeaderText" runat="server" CssClass="headerText" Text="<%$ Resources:Text, VoucherEnquiry %>" />
                                <br />
                                &nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label ID="lblHeaderNote" runat="server" ForeColor="CadetBlue" CssClass="tableTitle"
                                            Text="<%$ Resources:Text, VoucherBalanceEnquiryNote %>" Font-Bold="True" Font-Italic="true" />
                                <br />
                                <asp:MultiView ID="MultiViewEnquiry" runat="server" ActiveViewIndex="0" >
                                    <asp:View ID="ViewSearch" runat="server">
                                        <table style="width: 100%">
                                            <tr>
                                                <td colspan="2">
                                                    <asp:RadioButtonList ID="rboHKIDType" runat="server" RepeatDirection="Horizontal"
                                                        AutoPostBack="True">
                                                        <asp:ListItem Text="<%$ Resources:Text, HKICLongForm %>" Value="Y" Selected="True"></asp:ListItem>
                                                        <asp:ListItem Text="<%$ Resources:Text, ExemptionCert %>" Value="N"></asp:ListItem>
                                                    </asp:RadioButtonList></td>
                                                <td rowspan="4" valign="top" style="width: 380px">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:Image ID="imgHKIDSample" runat="server" ImageUrl="<%$ Resources:ImageUrl, HKICSampleImg %>"
                                                                    AlternateText="<%$ Resources:AlternateText, HKICSampleImg %>" />
                                                                <br />
                                                            </td>
                                                            <td>
                                                                <br />
                                                                <br />
                                                                <asp:Label ID="lblPatSearchHKIDHint" runat="server" CssClass="tableTitle"></asp:Label>
                                                                <br />
                                                                <br />
                                                                <asp:Label ID="lblPatSearchDOBHint" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ECDOBHint %>"></asp:Label>
                                                                <asp:Label ID="lblPatSearchDOBHint1" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ECDOBHint2 %>"></asp:Label><br />
                                                                <asp:Label ID="lblECDAgeTip" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ECDORegisterAgeHint %>"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 200px">
                                                    <asp:Label ID="lblPatsHKIDText" runat="server" CssClass="labelText" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                                                <td style="width: 250px">
                                                    <asp:TextBox Font-Size="18px" ID="txtPatsHKID" runat="server" MaxLength="11" onChange="formatHKID(this);"
                                                        Width="140px"></asp:TextBox>
                                                    <asp:Image ID="ImgPatsHKIDAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        Visible="False" />
                                                    <cc1:FilteredTextBoxExtender ID="filtereditPatsHKID" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
                                                        TargetControlID="txtPatsHKID" ValidChars='()'>
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 200px; height: 120px; vertical-align: top">
                                                    <asp:Label ID="lblPatsDOBText" runat="server" CssClass="labelText" Text="<%$ Resources: Text, DOBLongForm %>"></asp:Label></td>
                                                <td style="width: 250px; vertical-align: top">
                                                    <asp:Panel ID="pnlHKIDDOB" runat="server">
                                                        <asp:TextBox Font-Size="18px" ID="txtHKIDDOB" runat="server" MaxLength="10" Width="140px"
                                                            onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                            onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                            onblur="filterDateInput(this);"></asp:TextBox>
                                                        <asp:Image ID="imgHKIDDOBAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                            Visible="False" />
                                                        <cc1:FilteredTextBoxExtender ID="filtereditPatsHKIDDOB" runat="server" FilterType="Custom, Numbers"
                                                            TargetControlID="txtHKIDDOB" ValidChars='-'>
                                                        </cc1:FilteredTextBoxExtender>
                                                    </asp:Panel>
                                                    <asp:Panel ID="pnlECDOB" runat="server">
                                                        <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td valign="middle">
                                                                    <asp:RadioButton ID="rboDOB" runat="server" Text="" onclick="javascript:ChangeInputDOBFormat('DOB');" />
                                                                </td>
                                                                <td valign="top">
                                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox Font-Size="18px" ID="txtPatsDOB" runat="server" MaxLength="10" Width="140px"
                                                                                    onclick="javascript:ChangeInputDOBFormat('DOB');" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                                                                    onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                                                                    onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                                                                <asp:Label ID="lblOr1" runat="server" Text="<%$ Resources:Text, Or %>"></asp:Label>
                                                                                <cc1:FilteredTextBoxExtender ID="filtereditPatsDOB" runat="server" FilterType="Custom, Numbers"
                                                                                    TargetControlID="txtPatsDOB" ValidChars='-'>
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                            <td valign="middle">
                                                                                <asp:Image ID="imgPatsDOBAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                    Visible="False" /></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="middle">
                                                                    <asp:RadioButton ID="rboECAge" runat="server" Text="" onclick="javascript:ChangeInputDOBFormat('Age');" /></td>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblAge" runat="server" Text="<%$ Resources:Text, Age %>" onclick="javascript:ChangeInputDOBFormat('Age');"></asp:Label>
                                                                    <asp:TextBox ID="txtPatsAge" runat="server" Font-Size="18px" MaxLength="3" Width="40px"
                                                                        onclick="javascript:ChangeInputDOBFormat('Age');"></asp:TextBox>
                                                                    <asp:Label ID="lblRegisterOn" runat="server" Text="<%$ Resources:Text, RegisterOn %>"
                                                                        onclick="javascript:ChangeInputDOBFormat('Age');"></asp:Label>
                                                                    <cc1:FilteredTextBoxExtender ID="FilteredAge" runat="server" FilterType="Numbers"
                                                                        TargetControlID="txtPatsAge">
                                                                    </cc1:FilteredTextBoxExtender>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td valign="middle"></td>
                                                                <td valign="top">
                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Panel ID="pnlEngECAge" runat="server">
                                                                                    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                                                                        <tr>
                                                                                            <td>
                                                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                                                    <tr>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtRegisterOnDay" runat="server" Font-Size="18px" MaxLength="2"
                                                                                                                Width="30px" onclick="javascript:ChangeInputDOBFormat('Age');"></asp:TextBox></td>
                                                                                                        <td>
                                                                                                            <asp:DropDownList ID="ddlRegisterOnMonth" runat="server" onclick="javascript:ChangeInputDOBFormat('Age');"
                                                                                                                Font-Size="18px">
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
                                                                                                            </asp:DropDownList></td>
                                                                                                        <td>
                                                                                                            <asp:TextBox ID="txtRegisterOnYear" runat="server" MaxLength="4" onclick="javascript:ChangeInputDOBFormat('Age');"
                                                                                                                Width="40px" Font-Size="18px"></asp:TextBox></td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td align="center">
                                                                                                            <asp:Label ID="lblECDay_e" runat="server" Text="<%$ Resources:Text, Day %>" onclick="javascript:ChangeInputDOBFormat('Age');"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="center">
                                                                                                            <asp:Label ID="lblECMonth_e" runat="server" Text="<%$ Resources:Text, Month %>"></asp:Label>
                                                                                                        </td>
                                                                                                        <td align="center">
                                                                                                            <asp:Label ID="lblECYear_e" runat="server" Text="<%$ Resources:Text, Year %>" onclick="javascript:ChangeInputDOBFormat('Age');"></asp:Label>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                                <cc1:FilteredTextBoxExtender ID="FilteredDay" runat="server" FilterType="Numbers"
                                                                                                    TargetControlID="txtRegisterOnDay">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                                <cc1:FilteredTextBoxExtender ID="FilteredYear" runat="server" FilterType="Numbers"
                                                                                                    TargetControlID="txtRegisterOnYear">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </asp:Panel>
                                                                                <asp:Panel ID="pnlChiECAge" runat="server">
                                                                                    <asp:TextBox ID="txtRegisterOnYearChi" runat="server" MaxLength="4" onclick="javascript:ChangeInputDOBFormat('Age');"
                                                                                        Width="40px" Font-Size="18px"></asp:TextBox>
                                                                                    <asp:Label ID="lblECYear" runat="server" Text="<%$ Resources:Text, Year %>" onclick="javascript:ChangeInputDOBFormat('Age');"></asp:Label>&nbsp;<asp:DropDownList
                                                                                        ID="ddlRegisterOnMonthChi" runat="server" onclick="javascript:ChangeInputDOBFormat('Age');"
                                                                                        Font-Size="18px">
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
                                                                                    </asp:DropDownList>
                                                                                    <asp:Label ID="lblECMonth" runat="server" Text="<%$ Resources:Text, Month %>"></asp:Label>
                                                                                    <asp:TextBox ID="txtRegisterOnDayChi" runat="server" Font-Size="18px" MaxLength="2"
                                                                                        Width="30px" onclick="javascript:ChangeInputDOBFormat('Age');"></asp:TextBox>
                                                                                    <asp:Label ID="lblECDay" runat="server" Text="<%$ Resources:Text, Day %>" onclick="javascript:ChangeInputDOBFormat('Age');"></asp:Label>
                                                                                </asp:Panel>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredChiYear" runat="server" FilterType="Numbers"
                                                                                    TargetControlID="txtRegisterOnYearChi">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                                <cc1:FilteredTextBoxExtender ID="FilteredChiDay" runat="server" FilterType="Numbers"
                                                                                    TargetControlID="txtRegisterOnDayChi">
                                                                                </cc1:FilteredTextBoxExtender>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Image ID="imgECDORAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                    Visible="False" /></td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr>
                                                            <td>
                                                                <cc2:CaptchaControl ID="CaptchaControl" runat="server" CaptchaLineNoise="Low" CaptchaBackgroundNoise="None"
                                                                    CaptchaHeight="35" CaptchaWidth="240" CaptchaChars="123" Text="<%$ Resources:Text, Captcha %>"></cc2:CaptchaControl>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Image ID="imgCaptchaControlAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 200px">
                                                    <asp:LinkButton ID="lnkbtnOtherCaptcha" runat="server" Text="<%$ Resources:Text, CaptchaTryDiffImage %>"></asp:LinkButton></td>
                                                <td>
                                                    <asp:ImageButton ID="ibtnSearchPat" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" TabIndex="3" /></td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="ViewResult" runat="server">
                                        <table style="width:910px;position:relative;top:15px">
                                            <tr>
                                                <td style="text-align:left;vertical-align:top">
                                                    <table id="tblCurrentPosition" runat="server" style="width:480px;border-style:solid;border-color:lightgreen">
                                                        <tr>
                                                            <td align="left" style="width: 340px; height: 25px;padding-top:5px;" valign="top">
                                                                <asp:Label ID="lblDisplayHKIDText" runat="server" CssClass="labelText" style="position:relative;left:4px" Font-Size="18px" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                                                            <td align="left" valign="top" style="padding-top:5px;">
                                                                <asp:Label ID="lblDisplayHKID" runat="server" CssClass="labelText" Font-Size="18px"></asp:Label>&nbsp;</td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="height: 25px;" valign="top">
                                                                <asp:Label ID="lblDisplayDOBText" runat="server" CssClass="labelText" style="position:relative;left:4px" Font-Size="18px" Text="<%$ Resources:Text, DOBLongForm %>"/>
                                                            </td>
                                                            <td align="left" valign="top">
                                                                <asp:Label ID="lblDisplayDOB" runat="server" CssClass="labelText" Font-Size="18px"/>
                                                                <asp:Label ID="lblDisplayDOB_Chi" runat="server" CssClass="labelText" Font-Size="18px"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="height: 40px;text-align:left;background-image:url(./Images/others/circle_blue.png);background-repeat:no-repeat;background-position: 2px center;">
                                                                <asp:Label ID="lblCurrentPositionHeading" runat="server" CssClass="tableHeading" Font-Size="16px" Text="<%$ Resources:Text, CurrentPositionHeading %>"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="text-align:left;vertical-align:top;padding-bottom:14px">
                                                                <asp:Label ID="lblDisplayVoucherValueText" runat="server" CssClass="labelText" style="position:relative;left:4px" Font-Size="18px" Text="<%$ Resources:Text, AvailableVoucherVR %>"/>
                                                                <asp:Label ID="lblNoteText" runat="server" CssClass="labelText" Style="position:relative; top:-10px;" Font-Size="14px" Text="<%$ Resources:Text, Note_Short %>" />
                                                            </td>
                                                            <td style="text-align:left;vertical-align:middle;padding-bottom:12px">
                                                                <asp:Label ID="lblDisplayVoucherValue" runat="server" CssClass="labelText" Font-Size="18px"/>
                                                            </td>
                                                        </tr>
                                                        <tr id="trMaximumVoucherAmount" runat="server">
                                                            <td style="text-align:left;vertical-align:top;padding-left:2px;padding-bottom:14px">
                                                                <asp:Label ID="lblMaximumVoucherAmountText" runat="server" CssClass="labelText" Font-Size="18px" style="position:relative;left:4px"/>
                                                                <asp:Label ID="lblMaximumVoucherAmountText_Chi" runat="server" CssClass="labelText" Font-Size="18px" style="position:relative;left:4px"/>
                                                                <br />
                                                                <asp:Label ID="lblMaximumVoucherAmountNote" runat="server" CssClass="labelText" Font-Size="16px" style="width:320px;display:inline-block;position:relative;left:4px;top:3px"/>
                                                                <asp:Label ID="lblMaximumVoucherAmountNote_Chi" runat="server" CssClass="labelText" Font-Size="16px" style="width:260px;display:inline-block;position:relative;left:4px;top:3px"/>
                                                            </td>
                                                            <td style="text-align:left;vertical-align:middle;padding-bottom:12px">                                           
                                                                <asp:Label ID="lblMaximumVoucherAmount" runat="server" CssClass="labelText" Font-Size="18px"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <br />
                                                    <table id="tblNextPosition" runat="server" style="width:480px;border-style:solid;border-color:#bfa7ce">
                                                        <tr>
                                                            <td colspan="2" align="left" style="height: 50px;padding-top:5px;vertical-align:top;background-image:url(./Images/others/circle_blue.png);background-repeat:no-repeat;background-position: 2px 4px;">
                                                                <asp:Label ID="lblNextPositionHeading1" runat="server" CssClass="tableHeading" Font-Size="16px" Text="<%$ Resources:Text, NextPositionHeading1 %>"/>
                                                                <br />
                                                                <asp:Label ID="lblNextPositionHeading2" runat="server" CssClass="tableHeading" Font-Size="16px" Text="<%$ Resources:Text, NextPositionHeading2 %>"/>
                                                            </td>
                                                        </tr>
                                                        <tr id="trAnnualAllotment" runat="server">
                                                            <td align="left" style="width:340px;padding-left:2px;padding-bottom:14px" valign="top">
                                                                <asp:Label ID="lblAnnualAllotmentText" runat="server" CssClass="labelText" Font-Size="18px" style="position:relative;left:4px"/>
                                                                <asp:Label ID="lblAnnualAllotmentText_Chi" runat="server" CssClass="labelText" Font-Size="18px" style="position:relative;left:4px"/>
                                                            </td>
                                                            <td style="text-align:left;vertical-align:middle;padding-bottom:12px">                                           
                                                                <asp:Label ID="lblAnnualAllotment" runat="server" CssClass="labelText"  Font-Size="18px"/>
                                                            </td>
                                                        </tr>
                                                        <tr id="trForfeitVoucher" runat="server">
                                                            <td style="text-align:left;vertical-align:middle;padding-left:2px;padding-bottom:14px">
                                                                <asp:Label ID="lblForfeitVoucherText" runat="server" CssClass="labelText" Font-Size="18px" style="position:relative;left:4px"/>
                                                                <asp:Label ID="lblForfeitVoucherText_Chi" runat="server" CssClass="labelText" Font-Size="18px" style="width:260px;display:inline-block;position:relative;left:4px"/>
                                                                <br />
                                                                <asp:Label ID="lblForfeitVoucherNote" runat="server" CssClass="labelText" Font-Size="16px" style="position:relative;left:4px;top:3px"/>
                                                                <asp:Label ID="lblForfeitVoucherNote_Chi" runat="server" CssClass="labelText" Font-Size="16px" style="position:relative;left:4px;top:3px"/>
                                                            </td>
                                                            <td style="text-align:left;vertical-align:middle;padding-bottom:12px">                                           
                                                                <asp:Label ID="lblForfeitVoucher" runat="server" CssClass="labelText" Font-Size="18px"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width:10px">&nbsp;</td>
                                                <td style="text-align:left;vertical-align:top;width:400px">
                                                    <asp:Image ID="imgHKIDSymbolSample" runat="server" ImageUrl="<%$ Resources:ImageUrl, HKICSymbolSampleImg %>"
                                                        AlternateText="<%$ Resources:AlternateText, HKICSymbolSampleImg %>" Style="padding-top:10px;padding-bottom:17px" />
                                                    <table id="tblQuotaReference" runat="server" style="width:100%;border-style:solid;border-color:lightblue">
                                                        <tr>
                                                            <td colspan="2" style="padding-top:3px;padding-bottom:5px">
                                                                <asp:Label ID="lblForReference" runat="server" CssClass="labelText" Text="<%$ Resources:Text, ForReference %>" style="FONT-SIZE: 18px; POSITION: relative; LEFT: 4px;text-decoration:underline;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td align="left" style="padding-bottom:3px" valign="top">
                                                                <asp:Label ID="lblDisplayAvailableQuotaText" runat="server" CssClass="labelText" Font-Size="18px" style="position:relative;left:4px"/>
                                                                <asp:Label ID="lblDisplayAvailableQuotaText_Chi" runat="server" CssClass="labelText" Font-Size="18px" style="position:relative;left:4px"/>
                                                            </td>
                                                            <td align="left" style="width:170px;padding-bottom:3px" valign="top">
                                                                <asp:Label ID="lblDisplayAvailableQuota" runat="server" CssClass="labelText"  Font-Size="18px"/>
                                                                <br />
                                                                <asp:Label ID="lblDisplayAvailableQuotaUpto" runat="server" CssClass="labelText"  Font-Size="18px"/>                                                
                                                                <asp:Label ID="lblDisplayAvailableQuotaUpto_Chi" runat="server" CssClass="labelText"  Font-Size="18px"/>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="padding-bottom:14px">
                                                                <asp:Label ID="lblDisplayQuotaNote" runat="server" CssClass="labelText" Font-Size="16px" Text="<%$ Resources:Text, QuotaNote %>" style="position:relative;left:4px;top:3px"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right" style="height: 70px;">
                                                    <asp:ImageButton ID="ibtnComplete" runat="server" ImageUrl="<%$ Resources:ImageUrl, CompleteBtn %>" Style="padding-right:2px"
                                                        AlternateText="<%$ Resources:AlternateText, CompleteBtn %>" TabIndex="3" Visible="true" OnClick="ibtnComplete_Click" />
                                                </td>
                                                <td/>
                                                <td/>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Literal ID="lblNoteValue" runat="server" Text="<%$ Resources:Text, HCVR_Note %>"/>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                </asp:MultiView>
                            </td>
                            <td style="background-image: url(Images/master/right.jpg); width: 30px; background-repeat: repeat-y"></td>
                        </tr>
                        <tr>
                            <td style="background-image: url(Images/master/botton_left.jpg); width: 30px; height: 30px"></td>
                            <td style="background-image: url(Images/master/botton.jpg); background-repeat: repeat-x"></td>
                            <td style="background-image: url(Images/master/botton_right.jpg); width: 30px; height: 30px"></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="height: 145px" valign="bottom" align="center">
                    <table style="width: 100%">
                        <tr>
                            <td style="height: 140px" valign="top" align="center">
                                <asp:ImageButton ID="ibtnEasyGuide" runat="server" ImageUrl="<%$Resources:ImageUrl, HCVREasyGuideBtn%>"
                                    AlternateText="<%$Resources:AlternateText, HCVREasyGuideBtn%>" /></td>
                            <td style="height: 140px" valign="top" align="center">
                                <asp:ImageButton ID="ibtnFAQ" runat="server" ImageUrl="<%$Resources:ImageUrl, FAQsBtn%>"
                                    AlternateText="<%$Resources:AlternateText, FAQsBtn%>" /></td>
                            <td style="height: 140px" valign="top" align="center">
                                <asp:ImageButton ID="ibtnContactUs" runat="server" ImageUrl="<%$Resources:ImageUrl, ContactUsBtn%>"
                                    AlternateText="<%$Resources:AlternateText, ContactUsBtn%>" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="background-repeat: no-repeat; height: 8px; background-color: #d6d6d6; font-size: 14px"
                    valign="bottom">
                    <table style="width: 100%">
                        <tr>
                            <td align="left" style="height: 18px">&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkBtnPrivacyPolicy" runat="server" CssClass="footerText" Text="<%$ Resources:Text, PrivacyPolicy %>"></asp:LinkButton>
                                <asp:Label ID="lblseparator1" runat="server" CssClass="footerText" Text=" | "></asp:Label><asp:LinkButton
                                    ID="lnkBtnDisclaimer" runat="server" CssClass="footerText" Text="<%$ Resources:Text, ImportantNotices %>"></asp:LinkButton>
                                <asp:Label ID="lblseparator2" runat="server" CssClass="footerText" Text=" | "></asp:Label><asp:LinkButton
                                    ID="lnkBtnSysMaint" runat="server" CssClass="footerText" Text="<%$ Resources:Text, SysMaint %>"></asp:LinkButton>
                            </td>
                            <td align="right" style="height: 18px">
                                <asp:Label ID="lblFooterCopyright" runat="server" CssClass="footerText" Text="© Copyright Hospital Authority . All rights reserved."
                                    Visible="False"></asp:Label>
                                &nbsp; &nbsp; &nbsp;</td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>

        <asp:Button runat="server" ID="btnHiddenReminderWindowsVersion" Style="display: none" />
        <asp:Panel Style="display: none" ID="panReminderWindowsVersion" runat="server" Width="540px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpReminderWindowsVersion" runat="server" NoticeMode="Custom" IconMode="Information" ButtonMode="OK" DialogImagePath="Images/dialog/"
                                    HeaderText="<%$ Resources:Text, ReminderTitle %>" MessageText="<%$ Resources:Text, ReminderWindowsVersion %>" />
        </asp:Panel>

        <cc1:ModalPopupExtender ID="ModalPopupExtenderReminderWindowsVersion" runat="server" TargetControlID="btnHiddenReminderWindowsVersion"
            PopupControlID="panReminderWindowsVersion" BehaviorID="mdlPopup3" BackgroundCssClass="modalBackgroundTransparent"
            DropShadow="false" RepositionMode="None" PopupDragHandleControlID="panReminderWindowsVersionHeading">
        </cc1:ModalPopupExtender>
    </form>

    <script type="text/javascript">
        if (document.all) {
            top.window.resizeTo(screen.availWidth, screen.availHeight);
        }
        else if (document.layers || document.getElementById) {
            if (top.window.outerHeight < screen.availHeight) {
                top.window.outerHeight = screen.availHeight;
            }
            if (top.window.outerWidth < screen.availWidth) {
                top.window.outerWidth = screen.availWidth;
            }
        }
    </script>

</body>
</html>
