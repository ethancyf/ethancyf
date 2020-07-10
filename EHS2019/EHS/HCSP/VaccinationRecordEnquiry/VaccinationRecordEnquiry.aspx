<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="VaccinationRecordEnquiry.aspx.vb" Inherits="HCSP.VaccinationRecordEnquiry"
    Title="<%$ Resources: Title, VaccinationRecordEnquiry %>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc3" %>
<%@ Register Src="../UIControl/VaccinationRecord/ucVaccinationRecord.ascx" TagName="VaccinationRecord"
    TagPrefix="uc1" %>
<%@ Register Src="../UIControl/DocType/ucClaimSearch.ascx" TagName="ucClaimSearch"
    TagPrefix="uc2" %>
<%@ Register Src="../UIControl/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc3" %>
<%@ Register Src="../UIControl/ucVaccinationRecordProvider.ascx" TagName="ucVaccinationRecordProvider"
    TagPrefix="uc4" %>
<%@ Register Src="../UIControl/ucInputDocumentType.ascx" TagName="ucInputDocumentType"
    TagPrefix="uc5" %>
<%@ Register Src="~/UIControl/IDEASCombo/IDEASCombo.ascx" TagName="IDEASCombo" 
    TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc6:IDEASCombo ID="ucIDEASCombo" runat="server" />
            <table width="100%">
                <tr>
                    <td>
                        <asp:Image ID="imgHeader" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources: ImageUrl, VaccinationRecordEnquiryBanner %>"
                            AlternateText="<%$ Resources: AlternateText, VaccinationRecordEnquiryBanner %>" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="900px" />
                        <cc2:MessageBox ID="udcMessageBox" runat="server" Width="900px" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:MultiView ID="MultiViewVaccinationRecordEnquiry" runat="server">
                            <asp:View ID="ViewNoVaccinationScheme" runat="server">
                            </asp:View>
                            <asp:View ID="ViewSearch" runat="server">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="height: 25px; vertical-align: middle" class="eHSTableCaption">
                                            <asp:Label ID="lblSearchAccount" runat="server" Text="<%$ Resources:Text, SearchTempVRAcct %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <cc2:DocumentTypeRadioButtonGroup ID="udcDocumentTypeRadioButtonGroup" runat="server"
                                                HeaderCss="eHSTableHeading" AutoPostBack="true" HeaderText="<%$ Resources:Text, DocumentType %>"
                                                LegendImageURL="<%$ Resources:ImageUrl, Infobtn %>" LegendImageALT="<%$ Resources:Text, VaccinationRecordProvider %>" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="padding-bottom: 10px">
                                            <asp:Label ID="lblSearchECAcctInputSearch" runat="server" Text="<%$ Resources:Text, InputECSearchAccount %>"
                                                CssClass="tableText"></asp:Label>
                                            <asp:ImageButton ID="ibtnInputTips" runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, HelpBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, HelpBtn %>" OnClick="ibtnInputTips_Click"
                                                Visible="False" />
                                            <asp:ImageButton ID="btnReadSmartIDTips" Visible="false" runat="server" 
                                                ImageAlign="AbsMiddle" ImageUrl="<%$ Resources: ImageUrl, ReadSmartIDTipsBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ReadSmartIDTipsBtn %>" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc2:ucClaimSearch ID="udcClaimSearch" runat="server"></uc2:ucClaimSearch>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewInputRecipientInformation" runat="server">
                                <table style="width: 950px">
                                    <tr>
                                        <td style="height: 25px; vertical-align: middle" class="eHSTableCaption">
                                            <asp:Label ID="lblInputRecipientInformationHead" runat="server" Text="<%$ Resources:Text, InputRecipientInformation %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="eHSTableHeading" style="vertical-align: top">
                                            <asp:Label ID="lblInputRecipientInformationSubhead" runat="server" Text="<%$ Resources:Text, RecipientInformation %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" width="945px">
                                                <tr>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr style="height: 25px">
                                                                <td style="width: 200px; vertical-align: top">
                                                                    <asp:Label ID="lblIADocumentTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DocumentType %>"></asp:Label>
                                                                </td>
                                                                <td style="vertical-align: top">
                                                                    <asp:Label ID="lblIADocumentType" runat="server" CssClass="tableText"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 25px">
                                                                <td style="vertical-align: top">
                                                                    <asp:Label ID="lblIADocumentNoText" runat="server" CssClass="tableTitle"></asp:Label>
                                                                </td>
                                                                <td style="vertical-align: top">
                                                                    <asp:Label ID="lblIADocumentNo" runat="server" CssClass="tableText"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:HiddenField ID="hfDocCode" runat="server" />
                                                        <asp:MultiView ID="mvDOB" runat="server">
                                                            <asp:View ID="ViewNormal" runat="server">
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr style="height: 25px">
                                                                        <td style="vertical-align: top; width: 200px">
                                                                            <asp:Label ID="lblIADOBText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOB %>"></asp:Label>
                                                                        </td>
                                                                        <td style="vertical-align: top">
                                                                            <asp:Label ID="lblIADOB" runat="server" CssClass="tableText"></asp:Label>
                                                                        </td>
                                                                </table>
                                                            </asp:View>
                                                            <asp:View ID="ViewHKBC" runat="server">
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr style="height: 25px">
                                                                        <td style="vertical-align: top; width: 200px">
                                                                            <asp:Label ID="lblIAHKBCDOBText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOB %>"></asp:Label>
                                                                        </td>
                                                                        <td style="vertical-align: top">
                                                                            <asp:TextBox ID="txtHKBCDOB" runat="server" Enabled="False" MaxLength="10" Width="75px"></asp:TextBox>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:View>
                                                            <asp:View ID="ViewEC" runat="server">
                                                                <table cellpadding="0" cellspacing="0">
                                                                    <tr style="height: 25px">
                                                                        <td style="vertical-align: top; width: 200px">
                                                                            <asp:Label ID="lblIAECSerialNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ECSerialNo %>"></asp:Label>
                                                                        </td>
                                                                        <td style="vertical-align: top">
                                                                            <asp:TextBox ID="txtECSerialNo" runat="server" MaxLength="8" onChange="convertToUpper(this);" Width="100px"></asp:TextBox>
                                                                            <asp:CheckBox ID="cboECSerialNoNotProvided" runat="server" Text="<%$ Resources:Text, NotProvided %>"
                                                                                CssClass="tableText" AutoPostBack="True" OnCheckedChanged="cboECSerialNoNotProvided_CheckedChanged" />
                                                                            <asp:Image ID="imgECSerialNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" Visible="false" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr style="height: 25px">
                                                                        <td style="vertical-align: top; width: 200px">
                                                                            <asp:Label ID="lblIAECDOBText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOB %>"></asp:Label>
                                                                        </td>
                                                                        <td style="vertical-align: top">
                                                                            <asp:TextBox ID="txtECDOB" runat="server" Enabled="False" MaxLength="10" Width="75px"></asp:TextBox>
                                                                            <asp:Label ID="lblECDOB" runat="server" CssClass="tableText"></asp:Label>
                                                                            <asp:Image ID="imgECDOBError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="vertical-align: top">
                                                                            <asp:Label ID="lblIAECDOBTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOBType %>"
                                                                                Height="25px"></asp:Label>
                                                                        </td>
                                                                        <td class="tableCellStyle" style="vertical-align: top">
                                                                            <asp:RadioButtonList ID="rblECDOBType" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                                                                RepeatLayout="Flow">
                                                                            </asp:RadioButtonList>
                                                                            <asp:Image ID="imgECDOBTypeError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:View>
                                                        </asp:MultiView>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table cellpadding="0" cellspacing="0">
                                                            <tr style="height: 50px">
                                                                <td style="width: 200px; vertical-align: top">
                                                                    <asp:Label ID="lblIANameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, EnglishName %>"></asp:Label>
                                                                </td>
                                                                <td style="vertical-align: top">
                                                                    <table cellpadding="0" cellspacing="0">
                                                                        <tr>
                                                                            <td>
                                                                                <asp:TextBox ID="txtIANameSurname" runat="server" MaxLength="40" onChange="convertToUpper(this);"
                                                                                    Width="105px"></asp:TextBox></td>
                                                                            <td>
                                                                                <asp:Label ID="lblIANameComma" runat="server" CssClass="largeText" Text="<%$ Resources:Text, Comma %>"></asp:Label>&nbsp;</td>
                                                                            <td>
                                                                                <asp:TextBox ID="txtIANameFirstname" runat="server" MaxLength="40" onChange="convertToUpper(this);"></asp:TextBox></td>
                                                                            <td>
                                                                                <asp:Image ID="imgIANameError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                                                                <cc1:FilteredTextBoxExtender ID="filtereditEnameSurname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
                                                                                    TargetControlID="txtIANameSurname" ValidChars="-' ." />
                                                                                <cc1:FilteredTextBoxExtender ID="filtereditEnameFirstname" runat="server" FilterType="UppercaseLetters, LowercaseLetters, Custom"
                                                                                    TargetControlID="txtIANameFirstname" ValidChars="-' ." />
                                                                            </td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td>
                                                                                <asp:Label ID="lblSurname" runat="server" CssClass="tableTitle"></asp:Label></td>
                                                                            <td>
                                                                            </td>
                                                                            <td>
                                                                                <asp:Label ID="lblGivenName" runat="server" CssClass="tableTitle"></asp:Label></td>
                                                                            <td>
                                                                            </td>
                                                                        </tr>
                                                                    </table>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 25px">
                                                                <td style="vertical-align: top">
                                                                    <asp:Label ID="lblIAGenderText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Gender %>"></asp:Label>
                                                                </td>
                                                                <td style="vertical-align: top">
                                                                    <asp:RadioButtonList ID="rblIAGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                                                        RepeatLayout="Flow">
                                                                        <asp:ListItem Value="F">Female</asp:ListItem>
                                                                        <asp:ListItem Value="M">Male</asp:ListItem>
                                                                    </asp:RadioButtonList>&nbsp;
                                                                    <asp:Image ID="imgIAGenderError" runat="server" ImageAlign="AbsBottom" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="vertical-align: top">
                                                                </td>
                                                                <td style="vertical-align: top">
                                                                    <asp:ImageButton ID="ibtnInputRecipientInformationCancel" runat="server" ImageUrl="<%$ Resources: ImageURL, CancelBtn %>"
                                                                        AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnInputRecipientInformationCancel_Click" />
                                                                    <asp:ImageButton ID="ibtnInputRecipientInformationNext" runat="server" ImageUrl="<%$ Resources: ImageURL, NextBtn %>"
                                                                        AlternateText="<%$ Resources: AlternateText, NextBtn %>" OnClick="ibtnInputRecipientInformationNext_Click" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewInputRecipientInformationSmartID" runat="server">
                                <table style="width: 900px">
                                    <tr>
                                        <td style="height: 25px; vertical-align: middle" class="eHSTableCaption">
                                            <asp:Label ID="lblInputRecipientInformationSmartIDHead" runat="server" Text="<%$ Resources:Text, InputRecipientInformation %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="eHSTableHeading" style="vertical-align: top">
                                            <asp:Label ID="lblInputRecipientInformationSmartIDSubhead" runat="server" Text="<%$ Resources:Text, RecipientInformation %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <uc5:ucInputDocumentType ID="ucInputDocumentType" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="vertical-align: top; padding-left: 200px; padding-top: 15px">
                                            <asp:ImageButton ID="ibtnInputRecipientInformationSmartIDCancel" runat="server" ImageUrl="<%$ Resources: ImageURL, CancelBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnInputRecipientInformationSmartIDCancel_Click" />
                                            <asp:ImageButton ID="ibtnInputRecipientInformationSmartIDProceed" runat="server"
                                                ImageUrl="<%$ Resources: ImageURL, ProceedToEnquiryBtn %>" AlternateText="<%$ Resources: AlternateText, ProceedToEnquiryBtn %>"
                                                OnClick="ibtnInputRecipientInformationSmartIDProceed_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewConfirmRecipientInformation" runat="server">
                                <table style="width: 900px">
                                    <tr>
                                        <td style="height: 25px; vertical-align: middle" class="eHSTableCaption">
                                            <asp:Label ID="lblConfirmRecipientInformationHead" runat="server" Text="<%$ Resources:Text, ConfirmRecipientInformation %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="eHSTableHeading" style="vertical-align: top">
                                            <asp:Label ID="lblConfirmRecipientInformationSubhead" runat="server" Text="<%$ Resources:Text, RecipientInformation %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table>
                                                <tr style="height: 20px">
                                                    <td style="width: 200px; vertical-align: top">
                                                        <asp:Label ID="lblCADocumentTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DocumentType %>"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblCADocumentType" runat="server" CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px" >
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblCADocumentNoText" runat="server" CssClass="tableTitle"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblCADocumentNo" runat="server" CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px" runat="server" id="ViewConfirmRecipientInformation_SerialNoTableRow">
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblCASerialNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ECSerialNo %>">></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblCASerialNo" runat="server" CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px">
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblCADOBText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOB %>"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblCADOB" runat="server" CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px">
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblCANameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, EnglishName %>"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblCAName" runat="server" CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr style="height: 20px">
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblCAGenderText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Gender %>"></asp:Label>
                                                    </td>
                                                    <td style="vertical-align: top">
                                                        <asp:Label ID="lblCAGender" runat="server" CssClass="tableText"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="ibtnConfirmRecipientInformationBack" runat="server" ImageUrl="<%$ Resources: ImageURL, BackBtn %>"
                                                            AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnConfirmRecipientInformationBack_Click" />
                                                        <asp:ImageButton ID="ibtnConfirmRecipientInformationProceed" runat="server" ImageUrl="<%$ Resources: ImageURL, ProceedToEnquiryBtn %>"
                                                            AlternateText="<%$ Resources: AlternateText, ProceedToEnquiryBtn %>" OnClick="ibtnConfirmRecipientInformationProceed_Click" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="ViewResult" runat="server">
                                <table>
                                    <tr>
                                        <td>
                                            <uc1:VaccinationRecord ID="udcVaccinationRecord" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td class="checkboxStyle" style="width: 889px">
                                                        <asp:Label ID="lblDisclaimer" runat="server" Text="<%$ Resources: Text, Disclaimer %>"
                                                            Style="text-decoration: underline"></asp:Label>
                                                        <asp:ImageButton ID="ibtnInfo" runat="server" ImageUrl="<%$ Resources: ImageUrl, Infobtn %>"
                                                            AlternateText="<%$ Resources: Text, VaccinationRecordProvider %>" OnClick="ibtnInfo_Click" />
                                                        <br />
                                                        <asp:Label ID="lblNotCompleteVaccinationRecord" runat="server" Text="<%$ Resources:Text, NotCompleteVaccinationRecord %>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ibtnReturn" runat="server" ImageUrl="<%$ Resources: ImageURL, ReturnBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnReturn_Click" />
                                            <asp:ImageButton ID="ibtnProceedToClaim" runat="server" ImageUrl="<%$ Resources: ImageURL, ProceedClaimBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ProceedClaimBtn %>" OnClick="ibtnProceedToClaim_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                    </td>
                </tr>
            </table>
            <%-- Popup for Vaccination Record Provider --%>
            <asp:Panel Style="display: none" ID="panVaccinationRecordProvider" runat="server">
                <asp:Panel ID="panVaccinationRecordProviderHeading" runat="server" Style="cursor: move;">
                    <table cellpadding="0" cellspacing="0" style="width: 800px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblVaccinationRecordProviderHeading" runat="server" Text="<%$ Resources:Text, VaccinationRecordProvider %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table cellpadding="0" cellspacing="0" style="width: 800px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panVaccinationRecordProviderContent" runat="server" ScrollBars="Auto"
                                Height="420px">
                                <uc4:ucVaccinationRecordProvider ID="ucVaccinationRecordProvider" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="ibtnVaccinationRecordProviderClose" runat="server" ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>"
                                AlternateText="<%$ Resources:AlternateText, CloseBtn %>" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
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
            </asp:Panel>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderVaccinationRecordProvider" runat="server"
                BackgroundCssClass="modalBackgroundTransparent" TargetControlID="btnModalPopupVaccinationRecordProvider"
                PopupControlID="panVaccinationRecordProvider" PopupDragHandleControlID="panVaccinationRecordProviderHeading"
                RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnModalPopupVaccinationRecordProvider" runat="server">
            </asp:Button>
            <%-- End of Popup for Vaccination Record Provider --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
