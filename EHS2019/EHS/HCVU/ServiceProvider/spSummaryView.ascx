<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="spSummaryView.ascx.vb"
    Inherits="HCVU.spSummaryView" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="MOPracticeLists.ascx" TagName="MOPracticeLists" TagPrefix="uc1" %>
<asp:Panel ID="panDuplicated" runat="server" Style="display: none;">
    <asp:Panel ID="panDuplicatedHeading" runat="server" Style="cursor: move;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
            <tr>
                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                    <asp:Label ID="lblDuplicatedTitle" runat="server" Text="<%$ Resources:Text, Duplicationlist %>"></asp:Label></td>
                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
            </tr>
        </table>
    </asp:Panel>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
        <tr>
            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
            <td style="background-color: #ffffff" align="left">
                <asp:Panel ID="panDuplicatedContent" ScrollBars="Vertical" Height="300px" runat="server"
                    Width="97%">
                    <uc1:MOPracticeLists ID="MOPracticeLists1" runat="server" />
                </asp:Panel>
            </td>
            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
        </tr>
        <tr>
            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
            <td align="center" style="height: 30px; background-color: #ffffff" valign="bottom">
                <asp:ImageButton ID="ibtnDuplicatedClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnDuplicatedClose_Click" /></td>
            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
        </tr>
        <tr>
            <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
            <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
            <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
        </tr>
    </table>
</asp:Panel>
<table style="width: 90%">
    <tr>
        <td valign="top" colspan="2">
            <div class="headingText">
                <asp:Label ID="lblPersonalParticulars" runat="server" Text="<%$ Resources:Text, PersonalParticulars %>"></asp:Label>
            </div>
        </td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblERN" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblSPIDText" runat="server" Text="<%$ Resources:Text, SPID %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblSPID" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblSPUsernameText" runat="server" Text="<%$ Resources:Text, AccAlias %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblSPUsername" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblDateText" runat="server"></asp:Label></td>
        <td>
            <asp:Label ID="lblDate" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label>
            <asp:Label runat="server" ID="lblNameInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
        <td>
            <asp:Label ID="lblEname" runat="server" CssClass="tableText"></asp:Label>
            <asp:Label ID="lblCname" runat="server" CssClass="TextChi"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblHKID" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"></asp:Label>
            <asp:Label runat="server" ID="lblAddressInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
        <td>
            <asp:Label ID="lblAddress" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label>
            <asp:Label runat="server" ID="lblEmailInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
        <td>
            <asp:Label ID="lblEmail" runat="server" CssClass="tableText"></asp:Label>
            </td>
    </tr>
    <tr id="trPendingEmail" runat="server" visible="false">
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblPendingEmailText" runat="server" Text="<%$ Resources:Text, PendingEmailAddress %>"></asp:Label></td>
        <td valign="top">
            <asp:Label ID="lblPendingEmail" runat="server" CssClass="tableText"></asp:Label>
            <asp:Image ID="imgEditEmail" runat="server" ImageUrl="~/Images/others/small_edit.png"
                ToolTip="<%$ Resources:ToolTip, WaitingEmailConfirmation %>" AlternateText="<%$ Resources:AlternateText, WaitingEmailConfirmation %>"/></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label>
            <asp:Label runat="server" ID="lblContactNoInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
        <td>
            <asp:Label ID="lblContactNo" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label>
            <asp:Label runat="server" ID="lblFaxInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
        <td>
            <asp:Label ID="lblFax" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblSPStatusText" runat="server" Text="<%$ Resources:Text, SPStatus %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblSPStatus" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblRecordStatusText" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblRecordStatus" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblAccountStatusText" runat="server" Text="<%$ Resources:Text, AccountStatus %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblAccountStatus" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblPCDStatusText" runat="server" Text="<%$ Resources:Text, PCDStatus %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblPCDStatus" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblPCDProfessionalText" runat="server" Text="<%$ Resources:Text, PCDProfessional %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblPCDProfessional" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <!---CRE20-006 DHC Integrartion [Nichole]--->
    <tr id="trEnrolledDHC" runat="server" >
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblEnrolledDHCSPText" runat="server" Text="<%$ Resources:Text, EnrolledDHC %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblEnrolledDHCSP" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblTokenSNText" runat="server" Text="<%$ Resources:Text, TokenSN %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblTokenSN" runat="server" CssClass="tableText"></asp:Label>
            <%--<asp:Label ID="lblTokenIssueDate" runat="server" CssClass="tableText"></asp:Label>--%>
            <asp:Image ID="imgShareToken" runat="server" ImageUrl="<%$ Resources:ImageUrl, ShareTokenBtn_Large %>" />
            <asp:Image ID="imgTokenActivateDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClockBtn %>" />
            <asp:Label ID="lblTokenRemark" runat="server" CssClass="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblTokenReplacedSNText" runat="server" Text="<%$ Resources:Text, ReplacementTokenSerialNo %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblTokenReplacedSN" runat="server" CssClass="tableText"></asp:Label>
            <%--<asp:Label ID="lblTokenReplacedDate" runat="server" CssClass="tableText"></asp:Label>--%>
            <asp:Image ID="imgShareTokenReplacement" runat="server" ImageUrl="<%$ Resources:ImageUrl, ShareTokenBtn_Large %>" />
            <asp:Image ID="imgTokenAssignDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClockBtn %>" />
        </td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblEnrolledSchemeText" runat="server" Text="<%$ Resources:Text, SchemeInfo %>"></asp:Label>
            <asp:Label ID="lblEnrolledSchemeTextInd" runat="server" Text="*" ForeColor="Red"
                Visible="False"></asp:Label>
        </td>
        <td valign="top">
            <asp:Label ID="lblEnrolledSchemeNA" runat="server" Text="<%$ Resources:Text, N/A %>"
                CssClass="tableText" Visible="False"></asp:Label>
            <asp:GridView ID="gvEnrolledScheme" runat="server" AutoGenerateColumns="False" ShowHeader="True"
                SkinID="SchemeGridview" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                        <ItemStyle VerticalAlign="Top" Width="100px" />
                        <ItemTemplate>
                            <asp:Label ID="lblESchemeName" runat="server" Text='<%# Eval("SchemeCode") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Text, Status %>">
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblERecordStatus" runat="server" Text='<%# Eval("RecordStatus") %>' />
                            <%--<asp:HiddenField ID="hfEDelistStatus" runat="server" Value='<%# Eval("DelistStatus") %>' />--%>
                            <asp:Label ID="lblERemark" runat="server" Text='<%# Eval("Remark") %>' Visible="False" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Text, EffectiveTime %>">
                        <ItemStyle VerticalAlign="Top" Width="100px" />
                        <ItemTemplate>
                            <asp:Label ID="lblEEffectiveDtm" runat="server" Text='<%# Eval("EffectiveDtm") %>'
                                CssClass="tableText"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Text, DelistingTime %>">
                        <ItemStyle VerticalAlign="Top" Width="100px" />
                        <ItemTemplate>
                            <asp:Label ID="lblEDelistDtm" runat="server" Text='<%# Eval("DelistDtm") %>' CssClass="tableText"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Text, LogoReturnDate %>">
                        <ItemStyle VerticalAlign="Top" Width="120px" />
                        <ItemTemplate>
                            <asp:Label ID="lblELogoReturnDate" runat="server" Text='<%# Eval("LogoReturnDtm") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td style="width: 200px" valign="top">
            <asp:Label ID="lblTokenReturnText" runat="server" Text="<%$ Resources:Text, TokenReturnDate %>"></asp:Label></td>
        <td>
            <asp:Label ID="lblTokenReturn" runat="server" CssClass="tableText"></asp:Label></td>
    </tr>
</table>
<br />
<div class="headingText">
    <asp:Label ID="lblMOTitle" runat="server" Text="<%$ Resources:Text, MedicalOrganizationInfo %>"></asp:Label>
</div>
<asp:Label ID="lblMONA" runat="server" Text="<%$ Resources:Text, N/A %>" CssClass="tableText"
    Height="40px" Style="padding-left: 50px;" Visible="False"></asp:Label>
<asp:GridView ID="gvMO" runat="server" AutoGenerateColumns="False" ShowHeader="False"
    Width="100%" Visible="False">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Label ID="lblMOIndex" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" Width="16px" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemTemplate>
                <table width="100%">
                    <tr>
                        <td style="width: 250px; background-color: #f7f7de;" valign="top">
                            <asp:Label ID="lblRegBankMOENameText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationName %>"></asp:Label></td>
                        <td style="width: 650px">
                            <asp:Label ID="lblMOEName" runat="server" Text='<%# Bind("MOEngName") %>' CssClass="tableText"></asp:Label>
                            <asp:ImageButton ID="ibtnDuplicateMO" runat="server" ImageUrl="~/Images/others/info.png"
                                AlternateText="<%$ Resources:Text, DuplicateMO %>" Visible='<%# Eval("IsDuplicated") %>'
                                OnClick="ibtnDuplicateMO_Click" /><br />
                            <asp:Label ID="lblMOCName" runat="server" Text='<%# formatChineseString(Eval("MOChiName")) %>'
                                CssClass="TextChi"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 250px; background-color: #f7f7de;" valign="top">
                            <asp:Label ID="lblMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                        <td style="width: 650px">
                            <asp:Label ID="lblMOBRCode" runat="server" Text='<%# Bind("BrCode") %>' CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 250px; background-color: #f7f7de;" valign="top">
                            <asp:Label ID="lblMOContactNoText" runat="server" Text="<%$ Resources:Text, MOContactNo %>"></asp:Label>
                            <asp:Label ID="lblMOContactNoInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                        <td style="width: 650px">
                            <asp:Label ID="lblMOContactNo" runat="server" Text='<%# Bind("PhoneDaytime") %>'
                                CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 250px; background-color: #f7f7de;" valign="top">
                            <asp:Label ID="lblMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label>
                            <asp:Label ID="lblMOEmailInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                        <td style="width: 650px">
                            <asp:Label ID="lblMOEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 250px; background-color: #f7f7de;" valign="top">
                            <asp:Label ID="lblMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label>
                            <asp:Label ID="lblMOFaxInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                        <td style="width: 650px">
                            <asp:Label ID="lblMOFax" runat="server" Text='<%# Bind("Fax") %>' CssClass="tableText">
                            </asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 250px; background-color: #f7f7de;" valign="top">
                            <asp:Label ID="lblMOAddressText" runat="server" Text="<%$ Resources:Text, MOAddress %>"></asp:Label>
                            <asp:Label ID="lblMOAddressInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                        <td style="width: 650px">
                            <asp:Label ID="lblMOAddress" runat="server" Text='<%# formatAddress(Eval("MOAddress")) %>'
                                CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 250px; background-color: #f7f7de;" valign="top">
                            <asp:Label ID="lblMORelationText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"></asp:Label></td>
                        <td style="width: 650px">
                            <asp:Label ID="lblMORelation" runat="server" Text='<%# GetPracticeTypeName(Eval("Relationship")) %>'
                                CssClass="tableText"></asp:Label>
                            <asp:Label ID="lblMORelationRemark" runat="server" Text='<%# formatChineseString(Eval("RelationshipRemark")) %>'
                                CssClass="TextChi"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 250px; background-color: #f7f7de;" valign="top">
                            <asp:Label ID="lblMOStatusText" runat="server" Text="<%$ Resources:Text, MOStatus %>"></asp:Label>
                            <asp:Label ID="lblMOStatusTextInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                        </td>
                        <td style="width: 650px">
                            <asp:Label ID="lblMOStatus" runat="server" Text='<%# Bind("RecordStatus") %>' CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<br />
<div class="headingText">
    <asp:Label ID="lblPracticeBankInfo" runat="server" Text="<%$ Resources:Text, PracticeBankInfo %>"></asp:Label>
</div>
<asp:Label ID="lblPracticeBankNA" runat="server" Text="<%$ Resources:Text, N/A %>"
    CssClass="tableText" Height="40px" Style="padding-left: 50px;" Visible="False"></asp:Label>
<asp:GridView ID="gvPracticeBank" runat="server" AutoGenerateColumns="False" ShowHeader="False"
    Width="100%">
    <Columns>
        <asp:TemplateField>
            <ItemTemplate>
                <asp:Label ID="lblPracticeBankIndex" runat="server" Width="16px" Text='<%# Eval("DisplaySeq") %>'></asp:Label>
            </ItemTemplate>
            <ItemStyle VerticalAlign="Top" Width="16px" />
        </asp:TemplateField>
        <asp:TemplateField>
            <ItemStyle VerticalAlign="Top" />
            <ItemTemplate>
                <table width="100%">
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblPracticeTitle" runat="server" Text="<%$ Resources:Text, Practice %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-left: 15px">
                            <table width="100%">
                                <tr>
                                    <td style="Width:190px; background-color: #f7f7de;" valign="top">
                                        <asp:Label ID="lblPracticeMOText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>" Width="190px" Style="display:flex"></asp:Label>
                                        <asp:Label ID="lblPracticeMOTextInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                    </td>
                                    <td style="Width:700px;">
                                        <asp:Label ID="lblPracticeMO" runat="server" Text='<%# Eval("MODisplaySeq") %>' CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="background-color: #f7f7de;" valign="top">
                                        <asp:Label ID="lblPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label>
                                        <asp:Label ID="lblPracticeNameTextInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                            CssClass="tableText"></asp:Label>
                                        <asp:ImageButton ID="ibtnDuplicatePractice" runat="server" ImageUrl="~/Images/others/info.png"
                                            AlternateText="<%$ Resources:Text, DuplicatePractice %>" Visible='<%# Eval("IsDuplicated") %>'
                                            OnClick="ibtnDuplicatePractice_Click" /><br />
                                        <asp:Label ID="lblPracticeNameChi" runat="server" Text='<%# formatChineseString(Eval("PracticeNameChi")) %>'
                                            CssClass="TextChi"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #f7f7de;" valign="top">
                                        <asp:Label ID="lblPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label>
                                        <asp:Label ID="lblPracticeAddressInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblPracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress")) %>'
                                            CssClass="tableText"></asp:Label><br />
                                        <asp:Label ID="lblPracticeAddressChi" runat="server" Text='<%# formatChineseString(formatChiAddress(Eval("PracticeAddress"))) %>'
                                            CssClass="TextChi"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #f7f7de;" valign="top">
                                        <asp:Label ID="lblPracticeHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblPracticeHealthProf" runat="server" CssClass="tableText" Text='<%# GetHealthProfName(Eval("Professional.ServiceCategoryCode")) %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #f7f7de;" valign="top">
                                        <asp:Label ID="lblPracticeRegCodeText" runat="server" Text="<%$ Resources:Text, RegCode %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblPracticeRegCode" runat="server" CssClass="tableText" Text='<%# Eval("Professional.RegistrationCode") %>'></asp:Label></td>
                                </tr>
                                 <!---CRE20-006 DHC Integrartion [Nichole]--->
                                <tr >
                                    <td style="background-color: #f7f7de;" valign="top">
                                        <asp:Label ID="lblEnrolledDHCPracText" runat="server" Text="<%$ Resources:Text, EnrolledDHC %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblEnrolledDHCPrac" runat="server" CssClass="tableText" Text=''></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="background-color: #f7f7de;" valign="top">
                                        <asp:Label ID="lblPracticePhoneText" runat="server" Text="<%$ Resources:Text, PracticeTel %>"></asp:Label>
                                        <asp:Label ID="lblPracticePhoneInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblPracticePhone" runat="server" CssClass="tableText" Text='<%# Eval("PhoneDaytime") %>'></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="background-color: #f7f7de" valign="top">
                                        <asp:Label ID="lblPracticeMobileClinicText" runat="server" Text="<%$ Resources:Text, SPSResultMobileClinic %>"></asp:Label>
                                        <asp:Label ID="lblPracticeMobileClinicInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblPracticeMobileClinic" runat="server" CssClass="tableText" Text='<%# Eval("MobileClinic") %>'></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="background-color: #f7f7de" valign="top">
                                        <asp:Label ID="lblPracticeRemarksText" runat="server" Text="<%$ Resources:Text, Remarks %>"></asp:Label>
                                        <asp:Label ID="lblPracticeRemarksInd" runat="server" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblPracticeRemarks" runat="server" Text ="" CssClass="tableText"></asp:Label><br />
                                        <asp:Label ID="lblPracticeRemarksChi" runat="server" Text ="" CssClass="TextChi"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #f7f7de;" valign="top">
                                        <asp:Label ID="lblPracticeStatusText" runat="server" Text="<%$ Resources:Text, PracticeStatus %>"></asp:Label>
                                        <asp:Label ID="lblPracticeStatusTextInd" runat="server" Text="*" ForeColor="Red"
                                            Visible="False"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPracticeStatus" runat="server" CssClass="tableText" Text='<%# Eval("RecordStatus") %>'></asp:Label>
                                        <%--<asp:HiddenField ID="hfPracticeStatus" runat="server" Value='<%# Eval("RecordStatus") %>' />--%>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-color: #f7f7de;" valign="top">
                                        <asp:Label ID="lblPracticeSchemeText" runat="server" Text="<%$ Resources:Text, SchemeInfo %>"></asp:Label>
                                        <asp:Label ID="lblPracticeSchemeTextInd" runat="server" Text="*" ForeColor="Red"
                                            Visible="False"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblPracticeSchemeInfoNA" runat="server" Text="<%$ Resources:Text, N/A %>"
                                            CssClass="tableText" Visible="False"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:GridView ID="gvPracticeSchemeInfo" runat="server" OnRowDataBound="gvPracticeSchemeInfo_RowDataBound"
                                            OnPreRender="gvPracticeSchemeInfo_PreRender" AutoGenerateColumns="False" SkinID="SchemeGridview"
                                            Width="100%">
                                            <Columns>
                                                <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                                    <ItemStyle VerticalAlign="Top" Width="13%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPracticeSchemeCode" runat="server" CssClass="tableText" 
                                                            Text='<%# Eval("SchemeDisplayCode") %>'
                                                            SchemeCode='<%# Eval("SchemeCode") %>'
                                                            IsCategoryHeader='<%# Eval("IsCategoryHeader") %>' 
                                                            CategoryName='<%# Eval("CategoryName") %>' 
                                                            AllNotProvideService=""
                                                            />
                                                        <%--<asp:HiddenField ID="hfPracticeSchemeCode" runat="server" Value='<%# Eval("SchemeCode") %>' />--%>
                                                        <%--<asp:HiddenField ID="hfGIsCategoryHeader" runat="server" Value='<%# Eval("IsCategoryHeader") %>' />--%>
                                                        <%--<asp:HiddenField ID="hfGCategoryName" runat="server" Value='<%# Eval("CategoryName") %>' />--%>
                                                        <%--<asp:HiddenField ID="hfGAllNotProvideService" runat="server" />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Text, SubsidyAndServiceFee %>">
                                                    <ItemStyle VerticalAlign="Top" Width="10%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPracticeSubsidizeCode" runat="server" CssClass="tableText" 
                                                            Text='<%# Eval("SubsidizeDisplayCode") %>'
                                                            SubsidizeCode='<%# Eval("SubsidizeCode") %>'
                                                            />
                                                        <%--<asp:HiddenField ID="hfPracticeSubsidizeCode" runat="server" Value='<%# Eval("SubsidizeCode") %>' />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField>
                                                    <ItemStyle VerticalAlign="Top" Width="23%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPracticeServiceFee" runat="server"
                                                            CssClass="tableText">
                                                        </asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="Resources: Text, Status (Controlled in CodeBehind)">
                                                    <ItemStyle VerticalAlign="Top" Width="28%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPracticeSchemeStatus" runat="server" CssClass="tableText" />
                                                        <asp:Label ID="lblPracticeSchemeRemark" runat="server" />
                                                        <%--<asp:HiddenField ID="hfPracticeSchemeStatus" runat="server" />--%>
                                                        <%--<asp:HiddenField ID="hfPracticeSchemeDelistStatus" runat="server" />--%>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Text, EffectiveTime %>">
                                                    <ItemStyle VerticalAlign="Top" Width="13%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPracticeSchemeEffectiveDtm" runat="server"
                                                            CssClass="tableText"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="<%$ Resources:Text, DelistingTime %>">
                                                    <ItemStyle VerticalAlign="Top" Width="13%" />
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblPracticeSchemeDelistDtm" runat="server"
                                                            CssClass="tableText"></asp:Label>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <hr style="width: 100%; color: #ff8080; border-top-style: none; border-right-style: none; border-left-style: none; height: 1px; border-bottom-style: none;" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2">
                            <asp:Label ID="lblBankTitle" runat="server" Text="<%$ Resources:Text, Bank %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="padding-left: 15px">
                            <asp:Panel ID="pnlBank" runat="server" Visible="False">
                                <table width="100%">
                                    <tr>
                                        <td style="Width:190px; background-color: #f7f7de;">
                                            <asp:Label ID="lblBankNameText" runat="server" Width="190px" Text="<%$ Resources:Text, BankName %>"></asp:Label></td>
                                        <td style="Width:700px;">
                                            <asp:Label ID="lblBankName" runat="server" Text='<%# Eval("BankAcct.BankName") %>'
                                                CssClass="TextChi" Width="700px"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #f7f7de;">
                                            <asp:Label ID="lblBranchNameText" runat="server" Text="<%$ Resources:Text, BranchName %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblBranchName" runat="server" Text='<%# Eval("BankAcct.BranchName") %>'
                                                CssClass="TextChi"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #f7f7de;">
                                            <asp:Label ID="lblBankAccText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblBankAcc" runat="server" Text='<%# Eval("BankAcct.BankAcctNo") %>'
                                                CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #f7f7de;vertical-align:top;">
                                            <asp:Label ID="lblBankOwnerText" runat="server" Text="<%$ Resources:Text, BankOwner %>"></asp:Label></td>
                                        <td style="vertical-align:top;">
                                            <asp:Label ID="lblBankOwner" runat="server" Text='<%# Eval("BankAcct.BankAcctOwner") %>'
                                                CssClass="tableText" Width="700px" Style="word-wrap:break-word"></asp:Label></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlBankNA" runat="server" Visible="False">
                                <asp:Label ID="lblBankNA" runat="server" Text="<%$ Resources:Text, N/A %>" CssClass="tableText"></asp:Label>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
</asp:GridView>
<asp:HiddenField ID="hfERN" runat="server" />
<asp:HiddenField ID="hfTableLocation" runat="server" />
<asp:HiddenField ID="hfShowDuplicateMark" runat="server" />
<asp:Button runat="server" ID="btnHiddenDuplicated" Style="display: none" />
<cc1:ModalPopupExtender ID="ModalPopupExtenderDuplicated" runat="server" TargetControlID="btnHiddenDuplicated"
    PopupControlID="panDuplicated" BackgroundCssClass="modalBackgroundTransparent"
    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDuplicatedHeading">
</cc1:ModalPopupExtender>
