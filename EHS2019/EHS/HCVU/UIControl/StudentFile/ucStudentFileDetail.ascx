<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucStudentFileDetail.ascx.vb" Inherits="HCVU.ucStudentFileDetail" %>
<div class="headingText">
    <asp:Label ID="lblDVaccinationFile" runat="server" Text="<%$ Resources: Text, VaccinationFile %>"></asp:Label>
</div>
<table class="tblSFD">
    <tr>
        <td style="width: 260px">
            <asp:Label ID="lblDVaccinationFileIDText" runat="server" Text="<%$ Resources: Text, VaccinationFileID %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDVaccinationFileID" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblDSchemeText" runat="server" Text="<%$ Resources: Text, Scheme %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDScheme" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <asp:Panel ID="panDSchoolRCH" runat="server">
        <tr>
            <td>
                <asp:Label ID="lblDSchoolCodeText" runat="server" Text="<%$ Resources: Text, SchoolCode %>"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblDSchoolCode" runat="server" class="tableText"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblDSchoolNameText" runat="server" Text="<%$ Resources: Text, SchoolName %>"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblDSchoolName" runat="server" class="tableText"></asp:Label>
            </td>
        </tr>
    </asp:Panel>
    <tr>
        <td>
            <asp:Label ID="lblDServiceProviderIDText" runat="server" Text="<%$ Resources: Text, SPID %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDServiceProviderID" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblDServiceProviderNameText" runat="server" Text="<%$ Resources: Text, ServiceProviderName %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDServiceProviderName" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblDPracticeText" runat="server" Text="<%$ Resources: Text, Practice %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDPractice" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <asp:Panel ID="panDVaccinationInfo" runat="server">        
        <tr>
            <td colspan="2">
                <table class="tblSFD" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width:264px"></td>
                        <td style="width:200px">
                            <asp:Label ID="lblDOnlyDoseText" runat="server" CssClass="tableText" Text="<%$ Resources: Text, OnlyOr1stDose %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblD2ndDoseText" runat="server" CssClass="tableText" Text="<%$ Resources: Text, 2ndDose %>"></asp:Label>                                                
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDVaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblDVaccinationDate1" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblDVaccinationDate2" runat="server" CssClass="tableText"></asp:Label>                                                
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDVaccinationReportGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblDVaccinationReportGenerationDate1" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblDVaccinationReportGenerationDate2" runat="server" CssClass="tableText"></asp:Label>

                        </td>
                    </tr>
                    <asp:Panel ID="panD2ndVaccinationDate" runat="server"> 
                        <tr>
                            <td>
                                <asp:Label ID="lblDVaccinationDateText_2" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDVaccinationDate1_2" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDVaccinationDate2_2" runat="server" CssClass="tableText"></asp:Label>                                                
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblDVaccinationReportGenerationDateText_2" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDVaccinationReportGenerationDate1_2" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDVaccinationReportGenerationDate2_2" runat="server" CssClass="tableText"></asp:Label>

                            </td>
                        </tr>
                    </asp:Panel>
                    <tr>
                        <td>
                            <asp:Label ID="lblDSubsidyText" runat="server" Text="<%$ Resources: Text, Subsidy %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblDSubsidy" runat="server" class="tableText"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblDDoseToInjectText" runat="server" Text="<%$ Resources: Text, DoseToInject %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblDDoseToInject" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </asp:Panel>    
    <asp:Panel ID="panDMMR" runat="server">
        <tr>
            <td style="width: 264px">
                <asp:Label ID="lblDVaccinationDateMMRText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>" />
            </td>
            <td>
                <asp:Label ID="lblDVaccinationDateMMR" runat="server" class="tableText" />
            </td>
        </tr>
        <tr>
            <td style="width: 264px">
                <asp:Label ID="lblDSubsidyMMRText" runat="server" Text="<%$ Resources: Text, Subsidy %>" />
            </td>
            <td>
                <asp:Label ID="lblDSubsidyMMR" runat="server" class="tableText" />
            </td>
        </tr>
        <tr>
            <td style="width: 264px">
                <asp:Label ID="lblDDoseOfMMRText" runat="server" Text="<%$ Resources: Text, DoseOfMMR %>" />
            </td>
            <td>
                <asp:Label ID="lblDDoseOfMMR" runat="server" class="tableText" />
            </td>
        </tr>

    </asp:Panel>
    <tr>
        <td>
            <asp:Label ID="lblDUploadedByText" runat="server" Text="<%$ Resources: Text, UploadedBy %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDUploadedBy" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trDLastRectifiedBy" runat="server">
        <td>
            <asp:Label ID="lblDLastRectifiedByText" runat="server" Text="<%$ Resources: Text, LastRectifiedBy %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDLastRectifiedBy" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trDClaimUploadedBy" runat="server">
        <td>
            <asp:Label ID="lblDClaimUploadedByText" runat="server" Text="<%$ Resources: Text, ClaimUploadedBy %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDClaimUploadedBy" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trDClaimReactivatedBy" runat="server">
        <td>
            <asp:Label ID="lblDClaimReactivatedByText" runat="server" Text="<%$ Resources: Text, ClaimReactivatedBy %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDClaimReactivatedBy" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblDStatusText" runat="server" Text="<%$ Resources: Text, Status %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDStatus" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblDNoOfClassText" runat="server" Text="<%$ Resources: Text, NoOfClass %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDNoOfClass" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblDNoOfStudentText" runat="server" Text="<%$ Resources: Text, NoOfStudent %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDNoOfStudent" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trDNoOfWarningRecord" runat="server">
        <td>
            <asp:Label ID="lblDNoOfWarningRecordText" runat="server" Text="<%$ Resources: Text, NoOfWarningRecord %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDNoOfWarningRecord" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trDNoOfStudentInjected" runat="server">
        <td>
            <asp:Label ID="lblDNoOfStudentInjectedText" runat="server" Text="<%$ Resources: Text, NoOfStudentInjected %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDNoOfStudentInjected" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr style="height: 10px">
    </tr>
    <tr style="vertical-align:top" id="trDAcctSummary" runat="server">
        <td>
            <asp:Label ID="lblDAcctSummaryText" runat="server" Text="<%$ Resources: Text, AccountSummary %>"/>
        </td>
        <td>
            <table border="1" style="border-collapse:collapse;border-spacing:1px">
                <tr style="vertical-align:top">
                    <th style="width:190px;height:22px;vertical-align:middle">
                        <asp:Label ID="lblDAcctType" runat="server"  class="tableText" Text="<%$ Resources: Text, AccountType %>"/>
                    </th>
                    <th style="width:190px;height:22px;vertical-align:middle">
                        <asp:Label ID="lblDNoOfRecord" runat="server"  class="tableText" Text="<%$ Resources: Text, NoOfRecords %>"/>
                    </th>
                </tr>
                <tr style="vertical-align:top">
                    <td style="width:190px;height:22px;padding-left:5px;vertical-align:middle">
                        <asp:Label ID="lblDValidatedAcct" runat="server" class="tableText" Text="<%$ Resources: Text, ValidatedAcct %>"/>
                    </td>
                    <td style="width:190px;height:22px;text-align:center;vertical-align:middle">
                        <asp:Label ID="lblDNoOfValidatedAcct" runat="server" class="tableText" />
                    </td>
                </tr>
                <tr style="vertical-align:top">
                    <td style="width:190px;height:22px;padding-left:5px;vertical-align:middle">
                        <asp:Label ID="lblDTempAcct" runat="server" class="tableText" Text="<%$ Resources: Text, TempAcct %>"/>
                    </td>
                    <td style="width:190px;height:22px;text-align:center;vertical-align:middle">
                        <asp:Label ID="lblDNoOfTempAcct" runat="server" class="tableText" />
                    </td>
                </tr>
                <tr style="vertical-align:top">
                    <td style="width:190px;height:22px;padding-left:5px;vertical-align:middle">
                        <asp:Label ID="lblDNoAcct" runat="server" class="tableText" Text="<%$ Resources: Text, WithoutAcct %>"/>
                    </td>
                    <td style="width:190px;height:22px;text-align:center;vertical-align:middle">
                        <asp:Label ID="lblDNoOfNoAcct" runat="server" class="tableText" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trDInjectionSummary" runat="server" style="vertical-align:top">
        <td colspan="2">
            <br />
            <table id="tblInjectionSummary" runat="server" border="1" style="border-collapse:collapse;border-spacing:1px">
                <tr style="vertical-align:top">
                    <th id="thVaccinationFileID" runat="server" style="width:85px;height:22px;vertical-align:middle;">
                        <asp:Label ID="tblHeaderVaccinationFileID" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, VaccinationFileID %>"/>
                    </th>
                    <th style="width:85px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderSubsidy" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, Subsidy %>"/>
                    </th>
                    <th style="width:85px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderClass" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, Category %>"/>
                    </th>
                    <th style="width:85px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderDose" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, DoseToInject %>"/>
                    </th>
                    <th style="width:120px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderVaccinationDate" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, VaccinationDate %>"/>
                    </th>
                    <th style="width:120px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderGenerationDate" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"/>
                    </th>
                    <th style="width:100px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderNoOfStudent" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, NoOfClient %>"/>
                    </th>
                    <th style="width:100px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderActualInjectedYes" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, MarkInject %>"/>&nbsp;-&nbsp;
                        <asp:Label ID="tblHeaderYes" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, Yes %>" />
                    </th>
                    <th style="width:100px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderActualInjectedNo" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, MarkInject %>"/>&nbsp;-&nbsp;
                        <asp:Label ID="tblHeaderNo" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, No %>" />
                    </th>
                    <th style="width:100px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderMatch" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, Match %>"/>
                    </th>
                </tr>
            </table>
        </td>
    </tr>
</table>
<asp:Panel ID="pnlAssignDate" runat="server">
    <div class="headingText">
        <asp:Label ID="lblDVaccinationDateAssignment" runat="server" Text="<%$ Resources: Text, VaccinationDateAssignment %>"></asp:Label>
    </div>    
    <table id="tblAssignDate" runat="server" border="0" style="border-collapse:collapse;border-spacing:1px">
        <tr>
            <th style="width:260px;height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblASubsidyHeadingText" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, Subsidy %>"></asp:Label>
            </th>
            <th style="width:200px;height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAOnlyOr1stDoseHeadingText" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, OnlyOr1stDose %>"></asp:Label>
            </th>
            <th style="width:200px;height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblA2ndDoseHeadingText" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, 2ndDose %>"></asp:Label>
            </th>
        </tr>

        <tr id="trAssignDateQIV_1" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblASubsidyQIVText" runat="server" class="tableText" Font-Size="14px" Text="QIV"></asp:Label>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left"/>
            <td style="height:22px;vertical-align:middle;text-align:left"/>
        </tr>
        <tr id="trAssignDateQIV_2" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblAVaccinationDateQIVText" runat="server" class="tableText" Font-Size="14px"
                     Text="<%$ Resources: Text, VaccinationDate %>" Style="position: relative; Left: 10px" />
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAVaccinationDateQIV1" runat="server"  class="tableText"></asp:Label>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAVaccinationDateQIV2" runat="server" class="tableText"></asp:Label>
            </td>
        </tr>
        <tr id="trAssignDateQIV_3" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblAGenerationDateQIV" runat="server" class="tableText" Font-Size="14px"
                     Text="<%$ Resources: Text, VaccinationReportGenerationDate %>" Style="position: relative; Left: 10px" />
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAGenerationDateQIV1" runat="server" class="tableText"></asp:Label>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAGenerationDateQIV2" runat="server" class="tableText"></asp:Label>
            </td>
        </tr>

        <tr id="trAssignDate23vPPV_1" runat="server">
            <td colspan="3" style="height:10px" />
        </tr>
        <tr id="trAssignDate23vPPV_2" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblASubsidy23vPPVText" runat="server" class="tableText" Font-Size="14px" Text="23vPPV"></asp:Label>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left"/>
            <td style="height:22px;vertical-align:middle;text-align:left"/>
        </tr>
        <tr id="trAssignDate23vPPV_3" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblAVaccinationDate23vPPVText" runat="server" class="tableText" Font-Size="14px"
                     Text="<%$ Resources: Text, VaccinationDate %>" Style="position: relative; Left: 10px" />
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAVaccinationDate23vPPV1" runat="server" class="tableText"></asp:Label>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAVaccinationDate23vPPV2" runat="server" class="tableText"></asp:Label>
            </td>
        </tr>
        <tr id="trAssignDate23vPPV_4" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblAGenerationDate23vPPV" runat="server" class="tableText" Font-Size="14px"
                     Text="<%$ Resources: Text, VaccinationReportGenerationDate %>" Style="position: relative; Left: 10px" />
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAGenerationDate23vPPV1" runat="server" class="tableText"></asp:Label>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAGenerationDate23vPPV2" runat="server" class="tableText"></asp:Label>
            </td>
        </tr>

        <tr id="trAssignDatePCV13_1" runat="server">
            <td colspan="3" style="height:10px" />
        </tr>
        <tr id="trAssignDatePCV13_2" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblASubsidyPCV13Text" runat="server" class="tableText" Font-Size="14px" Text="PCV13"></asp:Label>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left"/>
            <td style="height:22px;vertical-align:middle;text-align:left"/>
        </tr>
        <tr id="trAssignDatePCV13_3" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblAVaccinationDatePCV13Text" runat="server" class="tableText" Font-Size="14px"
                     Text="<%$ Resources: Text, VaccinationDate %>" Style="position: relative; Left: 10px" />
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAVaccinationDatePCV131" runat="server" class="tableText"></asp:Label>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAVaccinationDatePCV132" runat="server" class="tableText"></asp:Label>
            </td>
        </tr>
        <tr id="trAssignDatePCV13_4" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblAGenerationDatePCV13" runat="server" class="tableText" Font-Size="14px"
                     Text="<%$ Resources: Text, VaccinationReportGenerationDate %>" Style="position: relative; Left: 10px" />
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAGenerationDatePCV131" runat="server"  class="tableText"></asp:Label>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAGenerationDatePCV132" runat="server"  class="tableText"></asp:Label>
            </td>
        </tr>

<%--        <tr id="trAssignDateMMR_1" runat="server">
            <td colspan="3" style="height:10px" />
        </tr>
        <tr id="trAssignDateMMR_2" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblASubsidyMMRText" runat="server" class="tableText" Font-Size="14px" Text="MMR"></asp:Label>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left"/>
            <td style="height:22px;vertical-align:middle;text-align:left"/>
        </tr>
        <tr id="trAssignDateMMR_3" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblAVaccinationDateMMRText" runat="server" class="tableText" Font-Size="14px"
                     Text="<%$ Resources: Text, VaccinationDate %>" Style="position: relative; Left: 10px" />
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAVaccinationDateMMR1" runat="server"  class="tableText"></asp:Label>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAVaccinationDateMMR2" runat="server" class="tableText"></asp:Label>
            </td>
        </tr>
        <tr id="trAssignDateMMR_4" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblAGenerationDateMMR" runat="server" class="tableText" Font-Size="14px"
                     Text="<%$ Resources: Text, VaccinationReportGenerationDate %>" Style="position: relative; Left: 10px" />
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAGenerationDateMMR1" runat="server" class="tableText"></asp:Label>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:Label ID="lblAGenerationDateMMR2" runat="server" class="tableText"></asp:Label>
            </td>
        </tr>--%>

    </table>
    <br />
</asp:Panel>
<asp:Panel ID="pnlClientInformation" runat="server">

    <div class="headingText">
        <asp:Label ID="lblDClassAndStudentInformation" runat="server" Text="<%$ Resources: Text, ClassAndStudentInformation %>"></asp:Label>
    </div>
    <asp:Label ID="lblDMessage" runat="server" Style="display: block; padding-top: 5px; padding-left: 5px; height: 60px; font-style: italic"></asp:Label>
    <asp:Panel ID="panD" runat="server">
        <table class="tblSFD">
            <tr>
                <td style="width: 260px">
                    <asp:Label ID="lblDClassNameText" runat="server" Text="<%$ Resources: Text, ClassName %>"></asp:Label>
                </td>
                <td>
                    <asp:DropDownList ID="ddlDClassName" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlDClassName_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="padding-left:56px">
                    <asp:imagebutton ID="ibtnDAddAccount" runat="server" ImageUrl="<%$ Resources: ImageUrl, AddAccountBtn %>" 
                                    AlternateText="<%$ Resources: AlternateText, AddAccountBtn %>" OnClick="ibtnDAddAccount_Click" />
                </td>
            </tr>
        </table>
        <div style="min-height: 80px">
            <br />
            <asp:GridView ID="gvD" runat="server" CssClass="gvTable" Width="1110" AutoGenerateColumns="False" AllowPaging="True"
                AllowSorting="True" OnRowDataBound="gvD_RowDataBound" OnPreRender="gvD_PreRender" OnSorting="gvD_Sorting"
                OnPageIndexChanging="gvD_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, SeqNo %>" SortExpression="Student_Seq" ItemStyle-Width="35">
                        <ItemTemplate>
                            <asp:Label ID="lblGSeqNo" runat="server" Text='<%# Eval("Student_Seq")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, ClassNo %>" SortExpression="Class_No_Sort" ItemStyle-Width="40">
                        <ItemTemplate>
                            <asp:Label ID="lblGClassNo" runat="server" Text='<%# Eval("Class_No") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, Action %>" ItemStyle-Width="40">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnGEdit" runat="server" />
                        </ItemTemplate>
                        <ItemStyle BackColor="White" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, RectifiedFlag %>" SortExpression="Rectified" ItemStyle-Width="40">
                        <ItemTemplate>
                            <asp:Label ID="lblGRectifiedFlag" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, DocTypeIDNL %>" SortExpression="DocCode_DocNo" ItemStyle-Width="110">
                        <ItemTemplate>
                            <asp:Label ID="lblGDocType" runat="server" Text='<%# Eval("Doc_Code") %>' />
                            <br />
                            <asp:Label ID="lblGDocNo" runat="server" Text='<%# Eval("Doc_No") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, ContactNo2 %>" SortExpression="Contact_No" ItemStyle-Width="60">
                        <ItemTemplate>
                            <asp:Label ID="lblGContactNo" runat="server" Text='<%# Eval("Contact_No") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, Name %>" SortExpression="NameEN_NameCH" ItemStyle-Width="120">
                        <ItemTemplate>
                            <asp:Label ID="lblGNameEN" runat="server" Text='<%# Eval("Name_EN")%>' />
                            <br />
                            <asp:Label ID="lblGNameCH" runat="server" Text='<%# Eval("Name_CH")%>' Font-Names="HA_MingLiu" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, Sex %>" SortExpression="Sex" ItemStyle-Width="30">
                        <ItemTemplate>
                            <asp:Label ID="lblGSex" runat="server" Text='<%# Eval("Sex") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, DOB %>" SortExpression="DOB" ItemStyle-Width="90">
                        <ItemTemplate>
                            <asp:Label ID="lblGDOB" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, OtherField %>" ItemStyle-Width="150">
                        <ItemTemplate>
                            <asp:Label ID="lblGOtherField" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, ServiceDate %>" ItemStyle-Width="70">
                        <ItemTemplate>
                            <asp:Label ID="lblGServiceDate" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, ConfirmToInject %>" SortExpression="Reject_Injection" ItemStyle-Width="50">
                        <ItemTemplate>
                            <asp:Label ID="lblGConfirmNotToInject" runat="server" Text='<%# Eval("Reject_Injection")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, Injected %>" SortExpression="Injected" ItemStyle-Width="60">
                        <ItemTemplate>
                            <asp:Label ID="lblGInjected" runat="server" Text='<%# Eval("Injected")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, WarningMessage %>" SortExpression="Upload_Warning" ItemStyle-Width="120">
                        <ItemTemplate>
                            <asp:Label ID="lblGWarningMessage" runat="server" Text='<%# Eval("Upload_Warning") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, TransactionNo %>" SortExpression="Transaction_ID" ItemStyle-Width="100">
                        <ItemTemplate>
                            <asp:Label ID="lblGTransactionNo" runat="server" Text='<%# Eval("Transaction_ID")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, TransactionStatus %>" SortExpression="Transaction_Record_Status" ItemStyle-Width="100">
                        <ItemTemplate>
                            <asp:Label ID="lblGTransactionRecordStatus" runat="server" Text='<%# Eval("Transaction_Record_Status_Desc_EN")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, FailReason %>" SortExpression="Fail_Reason" ItemStyle-Width="100">
                        <ItemTemplate>
                            <asp:Label ID="lblGFailReason" runat="server" Text='<%# Eval("Fail_Reason_EN")%>'/>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, AccountID_ReferenceNo %>" SortExpression="Real_Account_ID_Reference_No" ItemStyle-Width="100">
                        <ItemTemplate>
                            <asp:Label ID="lblGAccountIDReferenceNo" runat="server" Text='<%# Eval("Real_Account_ID_Reference_No") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, Status %>" SortExpression="Acc_Record_Status_Desc" ItemStyle-Width="100">
                        <ItemTemplate>
                            <asp:Label ID="lblGAccRecordStatus" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, AccountValidationResult %>" SortExpression="Acc_Validation_Result_Desc">
                        <ItemTemplate>
                            <asp:Label ID="lblGAccountValidationResult" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, FieldDiff %>" SortExpression="Field_Diff" ItemStyle-Width="40">
                        <ItemTemplate>
                            <asp:Label ID="lblGFieldDiff" runat="server" Text='<%# Eval("Field_Diff")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, CreateBy %>" SortExpression="Create_By" ItemStyle-Width="50">
                        <ItemTemplate>
                            <asp:Label ID="lblGCreateBy" runat="server" Text='<%# Eval("Create_By")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources: Text, CreationTime %>" SortExpression="Create_Dtm" ItemStyle-Width="84">
                        <ItemTemplate>
                            <asp:Label ID="lblGCreateDtm" runat="server" Text='<%# Eval("Create_Dtm")%>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </asp:Panel>
</asp:Panel>
