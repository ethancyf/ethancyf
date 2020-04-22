<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucEnrolmentCopyPopup.ascx.vb"
    Inherits="HCVU.ucEnrolmentCopyPopup" %>
<%@ Register Src="ucTypeOfPracticeGrid.ascx" TagName="ucTypeOfPracticeGrid" TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<style type="text/css">
    table.SchemeInformationGrid {
        font-size: 16px;
        color: #4d4d4d;
        font-family: Arial;
        font-weight: bold;
    }

        table.SchemeInformationGrid tr:first-child td {
            background-color: #eeeeee;
            color: #4d4d4d;
            text-align: left;
        }

        table.SchemeInformationGrid td {
            background-color: white;
            border: 1px solid #9d9d9d;
        }
</style>
<table border="0" cellpadding="0" cellspacing="0">
    <!-- Header -->
    <tr>
        <td colspan="3">
            <asp:Panel ID="panHeader" runat="server" Style="cursor: move;" Width="100%">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%;">
                    <tr>
                        <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                            <asp:Label ID="lblTitle" runat="server" Text="<%$ Resources:Title, EnrolmentCopy %>"></asp:Label></td>
                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                    </tr>
                </table>
            </asp:Panel>
        </td>
    </tr>
    <!-- Content -->
    <tr>
        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
            <table width="7px">
                <tr>
                    <td></td>
                </tr>
            </table>
        </td>
        <td style="background-color: #ffffff">
            <asp:Panel ID="panScroll" runat="server" Width="100%" Height="600px" ScrollBars="Auto">
                <table cellpadding="5" cellspacing="0" style="width: 98%;">
                    <tr>
                        <td>
                            <cc2:MessageBox ID="udcTMessageBox" runat="server" />
                            <!--------------------------------------------->
                            <table>
                                <tr>
                                    <td valign="top" style="width: 200px;">
                                        <asp:Label ID="lblERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblERN" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblEnrolDtmText" runat="server" Text="<%$ Resources:Text, EnrolmentTime %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblEnrolDtm" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                            </table>
                            <table width="100%">
                                <tr>
                                    <td>
                                        <div class="headingText">
                                            <asp:Label ID="lblPersonalHeadingText" runat="server" Text="<%$ Resources:Text, PersonalParticulars %>"></asp:Label>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                            <table>
                                <tr>
                                    <td colspan="2" valign="top">
                                        <asp:Label ID="lblConfirmNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblConfirmEname" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:Label ID="lblConfirmCname" runat="server" CssClass="TextChi"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px;" colspan="2" valign="top">
                                        <asp:Label ID="lblConfirmHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblConfirmHKID" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 200px" valign="top">
                                        <asp:Label ID="lblConfirmAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblConfirmAddress" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 200px" valign="top">
                                        <asp:Label ID="lblConfirmEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblConfirmEmail" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 200px" valign="top">
                                        <asp:Label ID="lblConfirmContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblConfirmContactNo" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 200px" valign="top">
                                        <asp:Label ID="lblConfirmFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblConfirmFax" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2" style="width: 200px" valign="top">
                                        <asp:Label ID="lblConfirmSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Panel ID="pnlConfirmScheme" CssClass="tableText" runat="server">
                                        </asp:Panel>
                                        <asp:Label ID="lblConfirmScheme" runat="server" CssClass="tableText" Visible="false"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="panEditMO" runat="server">
                                <br />
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <div class="headingText">
                                                <asp:Label ID="lblMOHeadingText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="gvMO" runat="server" AutoGenerateColumns="False" ShowHeader="True"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, MONo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblMOIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, MedicalOrganizationInfo %>">
                                            <ItemTemplate>
                                                <table>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblRegBankMOENameText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationName %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblMOEName" runat="server" Text='<%# Bind("MOEngName") %>' CssClass="tableText"></asp:Label><br />
                                                            <asp:Label ID="lblMOCName" runat="server" Text='<%# formatChineseString(Eval("MOChiName")) %>'
                                                                CssClass="TextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblMOBRCode" runat="server" Text='<%# Bind("BrCode") %>' CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblMOContactNoText" runat="server" Text="<%$ Resources:Text, MOContactNo %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblMOContactNo" runat="server" Text='<%# Bind("PhoneDaytime") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblMOEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblMOFax" runat="server" Text='<%# Bind("Fax") %>' CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblMOAddressText" runat="server" Text="<%$ Resources:Text, MOAddress %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblMOAddress" runat="server" Text='<%# formatAddress(Eval("MOAddress.Room"), Eval("MOAddress.Floor"), Eval("MOAddress.Block"), Eval("MOAddress.Building"), Eval("MOAddress.District")) %>'
                                                                CssClass="tableText"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblMORelationText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblMORelation" runat="server" Text='<%# GetPracticeTypeName(Eval("Relationship")) %>'
                                                                CssClass="tableText"></asp:Label>
                                                            <asp:Label ID="lblMORelationRemark" runat="server" Text='<%# formatChineseString(Eval("RelationshipRemark")) %>'
                                                                CssClass="TextChi"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <asp:Panel ID="panEditPracticeBank" runat="server">
                                <br />
                                <table width="100%">
                                    <tr>
                                        <td>
                                            <div class="headingText">
                                                <asp:Label ID="lblPracticeHeadingText" runat="server" Text="<%$ Resources:Text, Practice %>"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <asp:GridView ID="gvPractice" runat="server" AutoGenerateColumns="False" Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeNo %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblPracticeIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" Width="100px" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeInfo %>">
                                            <ItemTemplate>
                                                <table width="100%">
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblPracticeMOText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblPracticeMO" runat="server" Text='<%# Bind("MODisplaySeq") %>' CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblPracticeENameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblPracticeEName" runat="server" Text='<%# Bind("PracticeName") %>'
                                                                CssClass="tableText"></asp:Label><br />
                                                            <asp:Label ID="lblPracticeCName" runat="server" Text='<%# formatChineseString(Eval("PracticeNameChi")) %>'
                                                                CssClass="TextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblPracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.Building"), Eval("PracticeAddress.District")) %>'
                                                                CssClass="tableText"></asp:Label><br />
                                                            <asp:Label ID="lblPracticeAddressChi" runat="server" Text='<%# formatChineseString(formatChiAddress(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.ChiBuilding"), Eval("PracticeAddress.District"))) %>'
                                                                CssClass="TextChi"></asp:Label></td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblPracticeTelText" runat="server" Text="<%$ Resources:Text, PracticeTel %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblPracticeTel" runat="server" Text='<%# Bind("PhoneDaytime") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblHealthProf" runat="server" Text='<%# GetHealthProfName(Eval("Professional.ServiceCategoryCode")) %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblRegCodeText" runat="server" Text="<%$ Resources:Text, RegCode %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblRegCode" runat="server" Text='<%# Bind("Professional.RegistrationCode") %>'
                                                                CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #f7f7de; vertical-align: top">
                                                            <asp:Label ID="lblSchemeToEnrollText" runat="server" Text="<%$ Resources: Text, SchemeToEnroll %>"></asp:Label></td>
                                                        <td style="vertical-align: top">
                                                            <asp:Panel ID="pnlSchemeToEnroll" runat="server" CssClass="tableText"></asp:Panel>
                                                        </td>
                                                    </tr>
                                                    <tr id="trSchemeInformation" runat="server" visible="false">
                                                        <td style="background-color: #f7f7de; vertical-align: top">
                                                            <asp:Label ID="lblSchemeInformationText" runat="server" Text="<%$ Resources:Text, SchemeInfo %>"></asp:Label></td>
                                                        <td style="vertical-align: top">
                                                            <div id="divSchemeInformation1" runat="server" style="padding-bottom: 6px" visible="false">
                                                                <asp:Label ID="lblSchemeInformation1" runat="server" CssClass="tableText"></asp:Label>
                                                                <div style="padding-left: 20px">
                                                                    <asp:GridView ID="gvSchemeInformation1" runat="server" AutoGenerateColumns="false"
                                                                        OnPreRender="gvSchemeInformation_PreRender" CssClass="SchemeInformationGrid">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, Category %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="200px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGCategory" runat="server" Text='<%# Bind("Category") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, Subsidy %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="100px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGSubsidy" runat="server" Text='<%# Bind("Subsidy") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, ServiceFee %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="100px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGServiceFee" runat="server" Text='<%# Bind("ServiceFee") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                            <div id="divSchemeInformation2" runat="server" style="padding-bottom: 6px" visible="false">
                                                                <asp:Label ID="lblSchemeInformation2" runat="server" CssClass="tableText"></asp:Label>
                                                                <div style="padding-left: 20px">
                                                                    <asp:GridView ID="gvSchemeInformation2" runat="server" AutoGenerateColumns="false"
                                                                        OnPreRender="gvSchemeInformation_PreRender" CssClass="SchemeInformationGrid">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, Category %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="200px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGCategory" runat="server" Text='<%# Bind("Category") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, Subsidy %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="100px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGSubsidy" runat="server" Text='<%# Bind("Subsidy") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, ServiceFee %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="100px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGServiceFee" runat="server" Text='<%# Bind("ServiceFee") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                            <div id="divSchemeInformation3" runat="server" style="padding-bottom: 6px" visible="false">
                                                                <asp:Label ID="lblSchemeInformation3" runat="server" CssClass="tableText"></asp:Label>
                                                                <div style="padding-left: 20px">
                                                                    <asp:GridView ID="gvSchemeInformation3" runat="server" AutoGenerateColumns="false"
                                                                        OnPreRender="gvSchemeInformation_PreRender" CssClass="SchemeInformationGrid">
                                                                        <Columns>
                                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, Category %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="200px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGCategory" runat="server" Text='<%# Bind("Category") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, Subsidy %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="100px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGSubsidy" runat="server" Text='<%# Bind("Subsidy") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, ServiceFee %>">
                                                                                <ItemStyle VerticalAlign="Top" Width="100px" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblGServiceFee" runat="server" Text='<%# Bind("ServiceFee") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </div>
                                                            </div>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <hr style="width: 100%; color: #ff8080; height: 1px; border-width: thin;" />
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="2">
                                                            <asp:Label ID="lblBankTitle" CssClass="tableText" runat="server" Text="<%$ Resources:Text, BankInfo %>"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblBankNameText" runat="server" Text="<%$ Resources:Text, BankName %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblBankName" runat="server" Text="" CssClass="TextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblBranchNameText" runat="server" Text="<%$ Resources:Text, BranchName %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblBranchName" runat="server" Text="" CssClass="TextChi"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblBankAccText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblBankAcc" runat="server" Text="" CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                            <asp:Label ID="lblBankOwnerText" runat="server" Text="<%$ Resources:Text, BankOwner %>"></asp:Label></td>
                                                        <td valign="top">
                                                            <asp:Label ID="lblBankOwner" runat="server" Text="" CssClass="tableText"></asp:Label>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </asp:Panel>
                            <br />
                            <asp:Panel ID="panEHRSS" runat="server">
                                <asp:Panel ID="panEHRSSHeader" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <div class="headingTextOtherSystem">
                                                    <asp:Label ID="lblEHRSSText" runat="server" Text="<%$ Resources:Text, EHRSS %>"></asp:Label></div>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="panHadJoinEHRSS" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblHadJoinEHRSSText" runat="server" Text="<%$ Resources:Text, HadJoinedEHRSSQ %>"></asp:Label>
                                                &nbsp; &nbsp;<asp:Label ID="lblHadJoinEHRSS" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                            </asp:Panel>
                            <asp:Panel ID="panPCD" runat="server">
                                <asp:Panel ID="panPCDHeader" runat="server">
                                    <table>
                                        <tr>
                                            <td>
                                                <div class="headingTextOtherSystem">
                                                    <asp:Label ID="lblPCDText" runat="server" Text="<%$ Resources:Text, PCD %>"></asp:Label></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblWillJoinPCDText" runat="server" Text="<%$ Resources:Text, WillJoinPCD %>"></asp:Label>
                                                &nbsp; &nbsp;
                                                <asp:Label ID="lblWillJoinPCD" runat="server" CssClass="tableText"></asp:Label></td>
                                        </tr>
                                    </table>
                                </asp:Panel>
                                <asp:Panel ID="panSelectPracticeType" runat="server" Visible="True">
                                    <uc1:ucTypeOfPracticeGrid ID="ucTypeOfPracticeGrid" runat="server" />
                                </asp:Panel>
                            </asp:Panel>
                            <table style="width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:ImageButton ID="ibtnClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                            ToolTip="<%$ Resources:AlternateText, CloseBtn %>" ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
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
