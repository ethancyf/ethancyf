<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="InspectionRecordApproval.aspx.vb"
    Inherits="HCVU.InspectionRecordApproval" Title="Inspection Record Approval" %>

<%@ Register Src="../UIControl/DocType/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="cc3" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, InspectionRecordApprovalBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, InspectionRecordApprovalBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <link href="../CSS/CommonStyleInspection.css" rel="Stylesheet" />
            <asp:Panel ID="panMessageBox" runat="server" Width="950px">
                <cc2:InfoMessageBox ID="udcInfoMsgBox" runat="server" Width="95%" />
                <cc2:MessageBox ID="udcMsgBox" runat="server" Width="95%" />
            </asp:Panel>
            <asp:HiddenField runat="server" ID="hdnStatus" Value="" />
            <asp:MultiView ID="MultiViewIRM" runat="server" ActiveViewIndex="0">
                <!--Search-->
                <asp:View ID="vSEARCH" runat="server">
                    <div>
                        <div style="text-align: center; margin-top: 10px">
                            <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="<%$ Resources: ImageUrl, SearchBtn %>" Visible="false" />
                        </div>
                    </div>
                    <div>
                        <table style="width: 100%">
                            <tr>
                                <td style="width:250px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Action%>" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlAction" runat="server" Width="222" OnSelectedIndexChanged="ddlAction_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="trSubjectOfficer" runat="server" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, SubjectOfficer%>" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlSubjectOfficer" runat="server" Width="222" OnSelectedIndexChanged="ddlSubjectOfficer_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <div id="div1" runat="server">
                            <asp:GridView ID="gvApprovalResult" runat="server" AllowPaging="True" AllowSorting="True"
                                Width="1300px" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, InspectionRecordID%>" SortExpression="Inspection_ID">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" OnClick="ResultLbtn_Click" Text='<%# Eval("Inspection_ID")%>'><%# Eval("Inspection_ID")%></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" VerticalAlign="Top" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, FileReferenceNo%>" SortExpression="File_Reference_No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRFileRefNo" runat="server" Text='<%# Eval("File_Reference_No") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="110px" VerticalAlign="Top" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, SPIDPracticeID%>" SortExpression="SP_ID">
                                        <ItemTemplate>
                                            <%# Eval("SP_ID")%>
                                            (<%# Eval("Practice_Display_Seq")%>)
                                        </ItemTemplate>
                                        <ItemStyle Width="85px" VerticalAlign="Top" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, SPNameShort%>" SortExpression="SP_Eng_Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblREname" runat="server" Text='<%# Eval("SP_Eng_Name") %>'></asp:Label><br />
                                            <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("SP_Chi_Name") %>' CssClass="TextGridChi"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="150px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Practice%>" SortExpression="Practice_Name">
                                        <ItemTemplate>
                                            <%# Eval("Practice_Name")%>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, MainTypeOfInspection%>" SortExpression="Main_Type_Of_Inspection_Value">
                                        <ItemTemplate>
                                            <%# Eval("Main_Type_Of_Inspection_Value")%>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, VisitDate%>" SortExpression="Visit_Date">
                                        <ItemTemplate>
                                            <%# Eval("Visit_Date_Format")%>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="80px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                     <asp:TemplateField HeaderText="<%$ Resources: Text, CaseOfficer%>" SortExpression="Case_Officer_Value">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRCaseOfficer" runat="server" Text='<%# Eval("Case_Officer_Value")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, SubjectOfficer%>" SortExpression="Subject_Officer_Value">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRSubjectOfficer" runat="server" Text='<%# Eval("Subject_Officer_Value")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, FollowUpAction%>" SortExpression="Follow_Up_Action">
                                        <ItemTemplate>
                                            <%# Eval("Follow_Up_Action")%>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="70px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Status%>" SortExpression="Record_Status">
                                        <ItemTemplate>
                                            <%# Eval("Status_Description")%>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                    
                </asp:View>

                <!--Inspection Record Detail-->
                <asp:View ID="vViewInspectionDetail" runat="server">
                    <div>
                        <!--Inspection Record-->
                        <div class="headingText">
                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources: Text, InspectionRecord%>"></asp:Label>
                        </div>

                        <table style="width: 100%" class="visitTargetTable">
                            <tr>
                                <td style="width: 250px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, InspectionRecordID%>" />
                                </td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetInspectionID" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MainTypeOfInspection%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblDetMainTypeofInspection" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, OtherTypeOfInspection%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblDetTypeofInspection" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FileReferenceNo%>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblDetFileNo" runat="server" Text="<%$ Resources: Text, Empty%>" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <span id="spanRefNo" runat="server">
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, ReferredFileReferenceNo%>" />
                                    </td>
                                    <td>
                                        <asp:LinkButton CssClass="fontBold" runat="server" ID="lbDetReferredReferenceNo1" Enabled="false" Text="<%$ Resources: Text, Empty%>"></asp:LinkButton>
                                        <asp:HiddenField runat="server" ID="hfDetReferredInspectionID1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:LinkButton CssClass="fontBold" runat="server" ID="lbDetReferredReferenceNo2" Enabled="false" Text="<%$ Resources: Text, Empty%>"></asp:LinkButton>
                                        <asp:HiddenField runat="server" ID="hfDetReferredInspectionID2" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:LinkButton CssClass="fontBold" runat="server" ID="lbDetReferredReferenceNo3" Enabled="false" Text="<%$ Resources: Text, Empty%>"></asp:LinkButton>
                                        <asp:HiddenField runat="server" ID="hfDetReferredInspectionID3" />
                                    </td>
                                </tr>
                            </span>
                            <tr>
                                <td class="fieldCaption">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, CaseOfficer%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetCaseOfficer" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    &nbsp;&nbsp;
                                    (<asp:Literal runat="server" Text="<%$ Resources: Text,ContactNo2%>" />:
                                    <asp:Label ID="lblDetCaseContactNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, SubjectOfficer%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetSubjectOfficer" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    &nbsp;&nbsp;
                                    (<asp:Literal runat="server" Text="<%$ Resources: Text,ContactNo2%>" />:
                                    <asp:Label ID="lblDetSubjectContactNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                            </tr>
                        </table>
                        <!--Visit Target-->
                        <div class="headingText">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources: Text, VisitTarget%>"></asp:Label>
                        </div>
                        <table  class="visitTargetTable">
                            <tr>
                                <td style="width: 250px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetSPID" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderName%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetSPName" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblDetSPStatus" runat="server"  ForeColor="red"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trDetSPContactInfo">
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ContactInformation%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlDetSPTelNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, TelNo%>"></asp:Label>
                                        <asp:Label ID="lblDetSPTelNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlDetSPFaxNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, FaxNo%>"></asp:Label>
                                        <asp:Label ID="lblDetSPFaxNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlDetSPEmail" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, Email%>"></asp:Label>
                                        <asp:Label ID="lblDetSPEmail" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr runat="server" id="trDetHCVSEffectiveDate">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetHCVSEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trDetHCVSDHCEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSDHCEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetHCVSDHCEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trDetHCVSCHNEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSCHNEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetHCVSCHNEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trDetLastVisitDate" runat="server">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LastVisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetLastVisitDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Practice%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlDetPractice" runat="server">
                                        <asp:Label ID="lblDetPractice" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                        <asp:Label ID="lblDetPracticeStatus" runat="server" ForeColor="red"></asp:Label>
                                        <asp:HiddenField ID="hdfIIRPracticeSeq" runat="server" />
                                    </asp:Panel>
                                    <asp:Panel ID="Panel15" runat="server">
                                        <asp:Label ID="lblDetPractice_Ci" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr id="trDetPracticeAddress" runat="server">
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PCDPracticeAddress%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetPracticeAddress" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />
                                    <asp:Label ID="lblDetPracticeAddress_Ci" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr id="trDetHealthProf" runat="server">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HealthProf%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetHealthProfession" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PhoneNoofPractice%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetPracticePhoneDaytime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>

                            <tr id="trFreezeDate" runat="server" style="font-style: italic;">
                                <td>
                                    <asp:Label runat="server" ID="lblFreezeDate"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <!--Visit Detail Section 1-->
                        <div class="headingText" runat="server" id="headVisitDetail">
                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources: Text, VisitDetails%>"></asp:Label>
                        </div>
                        <table class="commonTable">
                            <tr>
                                <td class="fieldCaption">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, VisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetVisitDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                                <td class="fontBold">(<asp:Literal runat="server" Text="<%$ Resources: Text, VisitTime%>" />:
                                    <asp:Label ID="lblDetStartVisitTime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblDetTo" runat="server" Text="-" />
                                    <asp:Label ID="lblDetEndVisitTime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ConfirmationWith%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetConfirmationWith" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                                <td class="fontBold">(<asp:Literal runat="server" Text="<%$ Resources: Text, ConfirmDate%>" />:
                                    <asp:Label ID="lblDetConfirmDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FormCondition%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblDetFormCondition" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblDetFormConditionRm" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MeansOfCommunication%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblDetMeansofCommunication" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblDetMeansofCommunicationContact" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LowRiskClaim%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblDetLowRiskClaim" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Remarks%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblDetRemarks" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <!--Inspection Result Section-->
                        <div runat="server" id="divInspectionResultDetail">
                            <!--Inspection Result Section-->
                            <div class="headingText">
                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources: Text, InspectionResult%>"></asp:Label>
                            </div>
                            <table class="commonTable">
                                <tr>
                                    <td style="width: 250px" valign="top">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, NoOfClaim01%>" ID="Literal5" /></td>
                                    <td colspan="3">
                                        <table border="1" cellpadding="0" cellspacing="0" class="clsNoOfClaim">
                                            <tr>
                                                <td rowspan="2" class="tdheader">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, InOrder%>" /></td>
                                                <td colspan="2" class="tdheader">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Irregularities%>" /></td>
                                                <td rowspan="2" class="tdheader">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, TotalChecked%>" /></td>
                                            </tr>
                                            <tr>

                                                <td class="tdheader">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MissingForm%>" /></td>
                                                <td class="tdheader">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Inconsistent%>" /></td>
                                            </tr>
                                            <tr>
                                                <td class="tdinput">
                                                    <asp:Label runat="server" ID="lblDetInOrder"></asp:Label>
                                                </td>
                                                <td class="tdinput">
                                                    <asp:Label runat="server" ID="lblDetMissingForm"></asp:Label>
                                                </td>

                                                <td class="tdinput">
                                                    <asp:Label runat="server" ID="lblDetInconsistent"></asp:Label>
                                                </td>
                                                <td class="tdinput">
                                                    <asp:Label runat="server" ID="lblDetTotalCheck" Text="" ClientIDMode="Static"></asp:Label></td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, AnomalousClaims%>" /></td>
                                    <td colspan="3" class="fontBold">

                                        <asp:Label runat="server" ID="lblDetAnomalous" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, OverMajorIrregularities%>" /></td>
                                    <td colspan="3" class="fontBold">
                                        <asp:Label runat="server" ID="lblDetOverMajor" Text=""></asp:Label>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, CheckingDate%>" /></td>
                                    <td colspan="3" class="fontBold">
                                        <asp:Label runat="server" ID="lblDetCheckingDate"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />

                            <!--Action to be Token-->
                            <div class="headingText">
                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources: Text, ActionOptional%>"></asp:Label>
                            </div>
                            <table class="commonTable">
                                <tr>
                                    <td class="fieldCaption" valign="top">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, ActionTaken%>" />
                                    </td>
                                    <td colspan="3">
                                        <asp:Label runat="server" Text="<%$ Resources: Text, NA%>" ID="lblFurtherActionDetailEmpty" Visible="false" CssClass="fontBold" />
                                        <table cellpadding="0" cellspacing="0" class="actiontobetakenshow">
                                            <asp:Panel runat="server" ID="headerFurtherActionDetail">
                                                <tr>
                                                    <td class="tdheader" style="width: 150px">
                                                        <asp:Literal runat="server" Text="<%$ Resources: Text, Action%>" /></td>
                                                    <td class="tdheader">
                                                        <asp:Literal runat="server" Text="<%$ Resources: Text, Type%>" /></td>
                                                    <td class="tdheader" style="width: 150px">
                                                        <asp:Literal runat="server" Text="<%$ Resources: Text, Date%>" /></td>
                                                </tr>
                                            </asp:Panel>
                                            <asp:Repeater ID="FurtherActionDetail" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <%#IIf(Eval("Rowspan") > 0, "<td  valign=""top"" rowspan=" + Convert.ToInt16(Eval("Rowspan")).ToString() + " style=""background-color:ivory"">" + Eval("Action") + "</td>", "")%>
                                                        <td>
                                                            <%#Eval("Type")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Date")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="fieldCaption" valign="top">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, FollowUpAction%>" /></td>
                                    <td colspan="3">
                                        <asp:Label runat="server" Text="<%$ Resources: Text, NA%>" ID="lblFollowUpActionDetailEmpty" Visible="false" CssClass="fontBold"></asp:Label>
                                        <table cellpadding="0" cellspacing="0" class="actiontobetakenshow">
                                            <asp:Panel ID="headerFollowUpActionDetail" runat="server" Visible="true">
                                                <tr>
                                                    <td class="tdheader" style="width: 50px"></td>
                                                    <td class="tdheader" style="">
                                                        <asp:Literal runat="server" Text="<%$ Resources: Text, Action%>" /></td>
                                                    <td class="tdheader" style="width: 150px">
                                                        <asp:Literal runat="server" Text="<%$ Resources: Text, Date%>" /></td>
                                                </tr>
                                            </asp:Panel>
                                            <asp:Repeater ID="repDetFollowUpAction" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <%#Eval("Followup_Action_Seq")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Action_Desc").ToString()%>                                                        
                                                        </td>
                                                        <td>
                                                            <%#Eval("Action_Date_Format")%>                                             
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <hr style="color: #999999; border-style: solid; border-width: 1px 0px 0px 0px; width: 98%; text-align: left" />
                        </div>
                        <!--Visit Detail Section 2-->
                        <table class="commonTable">
                            <tr>
                                <td style="width: 250px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Status%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetStatus" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trReopenReason" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ReopenReason%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetReopenRequestReason" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trRemoveRequest" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, RemoveRequestBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetRemoveRequestBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trRemoveApprove" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, RemoveApproveBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetRemoveApproveBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trReopenRequest" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ReopenRequestBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetReopenRequestBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trReopenApprove" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ReopenApproveBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetReopenApproveBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trCloseRequest" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, CloseRequestBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetCloseRequestBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trCloseApprove" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, CloseApproveBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetCloseApproveBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, CreateBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetCreatedBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, UpdatedBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetUpdatedBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <!--bottom button-->
                        <div class="bottomBtnBox">
                            <span style="position: absolute; left: 0">
                                <asp:ImageButton ID="ibtnDetailBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>" OnClick="ibtnDetailBack_Click" />
                            </span>
                            <span>
                                <asp:ImageButton ID="ibtnApprove" runat="server" ImageUrl="<%$ Resources: ImageUrl, ApproveBtn %>" OnClick="ibtnApprove_Click"/>
                            </span>
                            <span>
                                <asp:ImageButton ID="ibtnReject" runat="server" ImageUrl="<%$ Resources: ImageUrl, RejectBtn %>" OnClick="ibtnReject_Click"/>
                            </span>
                        </div>

                </asp:View>


                <!--Create Successfully View-->
                <asp:View ID="vActionResultBox" runat="server">

                    <cc2:InfoMessageBox ID="boxInfoMessage" runat="server" Width="95%" />
                    <span>
                        <asp:ImageButton ID="ibtnMsgBoxBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>" OnClick="ibtnMsgBoxBack_Click" />
                    </span>
                </asp:View>
                <!--Update Successfully View-->

                <!--Update Inspection Result Successfully View-->
                <asp:View ID="vViewUpdateIIRSuccess" runat="server">
                    <cc2:InfoMessageBox ID="InfoMessageBoxUpdateIIRSuccess" runat="server" Width="95%" />
                    <span>
                        <asp:ImageButton ID="ibtnReturnIIRSuccess" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>" />
                    </span>
                </asp:View>
            </asp:MultiView>

            <asp:Button ID="btnApprove" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupConfirmApprove" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnApprove" PopupControlID="panPopupConfirmApprove" PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupConfirmApprove" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmApprove" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources: AlternateText, ConfirmApproveCase%>" />
            </asp:Panel>

            <asp:Button ID="btnReject" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupConfirmReject" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnReject" PopupControlID="panPopupConfirmReject" PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupConfirmReject" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmReject" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources: AlternateText, ConfirmRejectCase%>" />
            </asp:Panel>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
