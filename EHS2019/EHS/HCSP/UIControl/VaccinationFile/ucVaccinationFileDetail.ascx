<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucVaccinationFileDetail.ascx.vb" Inherits="HCSP.ucVaccinationFileDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<div class="eHSTableHeading">
    <asp:Label ID="lblDStudentFile" runat="server" Text="<%$ Resources: Text, VaccinationFile %>"></asp:Label>
</div>
<table class="tblSFD">
    <tr>
        <td style="width: 260px">
            <asp:Label ID="lblDStudentFileIDText" runat="server" Text="<%$ Resources: Text, VaccinationFileID %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDStudentFileID" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblDSchemeText" runat="server" Text="<%$ Resources: Text, Scheme %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDScheme" runat="server" class="tableText" />
        </td>
    </tr>
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
    <tr>
        <td>
            <asp:Label ID="lblDSubsidyText" runat="server" Text="<%$ Resources: Text, Subsidy %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDSubsidy" runat="server" class="tableText" Style="position:relative;left:0px;top:2px"></asp:Label>
        </td>
    </tr>
    <tr>
        <td colspan ="2">
            <table style="border-collapse:collapse">
                <tr>
                    <td style="width: 260px;height:22px" />
                    <td style="width: 200px">
                        <asp:Label ID="lblDDoseToInject1" runat="server" class="tableText" Text="<%$ Resources: Text, OnlyOr1stDose %>" />
                    </td>
                    <td style="width: 200px">
                        <asp:Label ID="lblDDoseToInject2" runat="server" class="tableText" Text="<%$ Resources: Text, 2ndDose %>" />
                    </td>                                  
                </tr>
                <tr>
                    <td style="width: 260px;height:22px">
                        <asp:Label ID="lblDVaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>" />
                    </td>
                    <td style="width: 200px">
                        <asp:Label ID="lblDVaccinationDate1" runat="server" class="tableText" />
                    </td>
                    <td style="width: 200px">
                        <asp:Label ID="lblDVaccinationDate2" runat="server" class="tableText" />
                    </td>   
                </tr>
                <tr>
                    <td style="width: 260px;height:22px">
                        <asp:Label ID="lblDVaccinationReportGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"/>
                    </td>
                    <td style="width: 200px">
                       <asp:Label ID="lblDVaccinationReportGenerationDate1" runat="server" class="tableText" />
                    </td>
                    <td style="width: 200px">
                       <asp:Label ID="lblDVaccinationReportGenerationDate2" runat="server" class="tableText" />
                    </td> 
                </tr>
                <asp:Panel ID="panD2ndVaccinationDate" runat="server"> 
                    <tr>
                        <td style="width: 260px;height:22px">
                            <asp:Label ID="lblDVaccinationDateText_2" runat="server" Text="<%$ Resources: Text, VaccinationDate %>" />
                        </td>
                        <td style="width: 200px">
                            <asp:Label ID="lblDVaccinationDate1_2" runat="server" CssClass="tableText" />
                        </td>
                        <td style="width: 200px">
                            <asp:Label ID="lblDVaccinationDate2_2" runat="server" CssClass="tableText" />                                                
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 260px;height:22px">
                            <asp:Label ID="lblDVaccinationReportGenerationDateText_2" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>" />
                        </td>
                        <td style="width: 200px">
                            <asp:Label ID="lblDVaccinationReportGenerationDate1_2" runat="server" CssClass="tableText" />
                        </td>
                        <td style="width: 200px">
                            <asp:Label ID="lblDVaccinationReportGenerationDate2_2" runat="server" CssClass="tableText" />
                        </td>
                    </tr>
                </asp:Panel>
            </table>
        </td>
    </tr>
<%--    <tr id="trDLastRectifiedBy" runat="server" visible="false">
        <td>
            <asp:Label ID="lblDLastRectifiedByText" runat="server" Text="<%$ Resources: Text, LastRectifiedBy %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDLastRectifiedBy" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>--%>
    <tr id="trDClaimUploadedBy" runat="server">
        <td>
            <asp:Label ID="lblDClaimUploadedByText" runat="server" Text="<%$ Resources: Text, ClaimUploadedBy %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDClaimUploadedBy" runat="server" class="tableText"></asp:Label>
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
    <tr id="trDAcctSummary" runat="server" style="vertical-align:top">
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
                        <asp:Label ID="lblDValidatedAcct" runat="server"  class="tableText" Text="<%$ Resources: Text, ValidatedAcct %>"/>
                    </td>
                    <td style="width:190px;height:22px;text-align:center;vertical-align:middle">
                        <asp:Label ID="lblDNoOfValidatedAcct" runat="server"  class="tableText" />
                    </td>
                </tr>
                <tr style="vertical-align:top">
                    <td style="width:190px;height:22px;padding-left:5px;vertical-align:middle">
                        <asp:Label ID="lblDTempAcct" runat="server"  class="tableText" Text="<%$ Resources: Text, TempAcct %>"/>
                    </td>
                    <td style="width:190px;height:22px;text-align:center;vertical-align:middle">
                        <asp:Label ID="lblDNoOfTempAcct" runat="server"  class="tableText" />
                    </td>
                </tr>
                <tr style="vertical-align:top">
                    <td style="width:190px;height:22px;padding-left:5px;vertical-align:middle">
                        <asp:Label ID="lblDNoAcct" runat="server"  class="tableText" Text="<%$ Resources: Text, WithoutAcct %>"/>
                    </td>
                    <td style="width:190px;height:22px;text-align:center;vertical-align:middle">
                        <asp:Label ID="lblDNoOfNoAcct" runat="server"  class="tableText" />
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
                    <th style="width:128px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderClass" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, Class %>"/>
                    </th>
                    <th style="width:128px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderNoOfStudent" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, NoOfStudent %>"/>
                    </th>
                    <%--<th style="width:120px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderConfirmedNotToInject" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, ConfirmedNotToInject %>"/>
                    </th>--%>
                    <th style="width:240px;height:22px;vertical-align:middle">
                        <asp:Label ID="lblNoOfInjectedFirstVisit0" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, ActualInjected1stVisit %>"/>
                    </th>
                    <th style="width:240px;height:22px;vertical-align:middle">
                        <asp:Label ID="lblNoOfInjectedSecondVisit0" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, ActualInjected2ndVisit %>"/>
                    </th>
                    <th style="width:160px;height:22px;vertical-align:middle">
                        <asp:Label ID="lblNoOfInjectedYes0" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, ActualInjectedYes %>"/>
                    </th>
                    <th style="width:160px;height:22px;vertical-align:middle">
                        <asp:Label ID="lblNoOfInjectedNo0" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, ActualInjectedNo %>"/>
                    </th>
                    <th style="width:128px;height:22px;vertical-align:middle">
                        <asp:Label ID="tblHeaderMatch" runat="server" class="tableText" Font-Size="14px" Text="<%$ Resources: Text, Match %>"/>
                    </th>
                </tr>
            </table>
        </td>
    </tr>
    <tr style="height: 10px">
    </tr>
</table>
<div id="divDClassAndStudentInformation" runat="server" class="eHSTableHeading">
    <asp:Label ID="lblDClassAndStudentInformation" runat="server" Text="<%$ Resources: Text, ClassAndStudentInformation %>"></asp:Label>
</div>
<asp:Label ID="lblDMessage" runat="server" Style="display: block; padding-top: 5px; padding-left: 5px; height: 60px; font-style: italic"></asp:Label>
<asp:Panel ID="panD" runat="server">
    <table class="tblSFD">
        <tr>
            <td style="width: 260px">
                <asp:Label ID="lblDClassNameText" runat="server" />
            </td>
            <td>
                <asp:DropDownList ID="ddlDClassName" runat="server" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlDClassName_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
    </table>
    <br />
    <div style="min-height: 80px">
        <asp:GridView ID="gvD" runat="server" CssClass="gvTable" Width="1100px" AutoGenerateColumns="False" AllowPaging="True"
            AllowSorting="True" OnRowDataBound="gvD_RowDataBound" OnPreRender="gvD_PreRender" OnSorting="gvD_Sorting"
            OnPageIndexChanging="gvD_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources: Text, SeqNo %>" SortExpression="Student_Seq" ItemStyle-Width="30">
                    <ItemTemplate>
                        <asp:Label ID="lblGSeqNo" runat="server" Text='<%# Eval("Student_Seq")%>' />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, ClassNo %>" SortExpression="Class_No_Sort" ItemStyle-Width="30">
                    <ItemTemplate>
                        <asp:Label ID="lblGClassNo" runat="server" Text='<%# Eval("Class_No") %>' />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, Action %>" ItemStyle-Width="40">
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnGEdit" runat="server" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, DocTypeIDNL %>" SortExpression="DocCode_DocNo" ItemStyle-Width="110">
                    <ItemTemplate>
                        <asp:Label ID="lblGDocType" runat="server" Text='<%# Eval("Doc_Code") %>' />
                        <br />
                        <asp:Label ID="lblGDocNo" runat="server" Text='<%# Eval("Doc_No") %>' />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, ContactNo2 %>" SortExpression="Contact_No" ItemStyle-Width="60">
                    <ItemTemplate>
                        <asp:Label ID="lblGContactNo" runat="server" Text='<%# Eval("Contact_No") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
<%--                <asp:TemplateField HeaderText="<%$ Resources: Text, ChineseName %>" SortExpression="Name_CH" ItemStyle-Width="60">
                    <ItemTemplate>
                        <asp:Label ID="lblGChineseName" runat="server" Text='<%# Eval("Name_CH") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, EnglishSurname %>" SortExpression="Surname_EN" ItemStyle-Width="60">
                    <ItemTemplate>
                        <asp:Label ID="lblGSurnameEN" runat="server" Text='<%# Eval("Surname_EN") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, EnglishGivenName %>" SortExpression="Given_Name_EN" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblGGivenNameEN" runat="server" Text='<%# Eval("Given_Name_EN") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="<%$ Resources: Text, Name %>" SortExpression="NameEN_NameCH" ItemStyle-Width="120">
                    <ItemTemplate>
                        <asp:Label ID="lblGNameEN" runat="server" Text='<%# Eval("Name_EN")%>' />
                        <br />
                        <asp:Label ID="lblGNameCH" runat="server" Text='<%# Eval("Name_CH")%>' CssClass="TextChineseName" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, Sex %>" SortExpression="Sex" ItemStyle-Width="30">
                    <ItemTemplate>
                        <asp:Label ID="lblGSex" runat="server" Text='<%# Eval("Sex") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, DOB %>" SortExpression="DOB" ItemStyle-Width="110">
                    <ItemTemplate>
                        <asp:Label ID="lblGDOB" runat="server"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, OtherField %>" ItemStyle-Width="140">
                    <ItemTemplate>
                        <asp:Label ID="lblGOtherInfo" runat="server" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, ConfirmToInject %>" SortExpression="Reject_Injection" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblGConfirmNotToInject" runat="server" Text='<%# Eval("Reject_Injection")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>

                <asp:TemplateField ItemStyle-Width="200">
                    <HeaderTemplate>
                        <asp:Label ID="lblGMarkInjected" runat="server" Text="<%$ Resources: Text, ActualInjected %>" />
                        <br />
                        <table style="border-collapse:collapse;border-spacing:0px;position:relative;left:3px">
                            <tr>
<%--                                <td style="width:50px">
                                    <asp:Checkbox ID="chkGMarkAllY" runat="server" AutoPostBack="false" style="position:relative;left:2px;top:-6px" />
                                    <asp:Label ID="lblGMarkAllY" runat="server" Text="<%$ Resources: Text, Yes %>" style="position:relative;top:-8px" />
                                </td>
                                <td style="width:50px">
                                    <asp:Checkbox ID="chkGMarkAllN" runat="server" AutoPostBack="false" style="position:relative;top:-6px" />
                                    <asp:Label ID="lblGMarkAllN" runat="server" Text="<%$ Resources: Text, No %>" style="position:relative;top:-8px" />
                                </td>--%>
                                <td>
                                    <asp:CheckBoxList ID="cblActualInject" runat="server" AutoPostBack="false" style="position:relative;left:2px;" RepeatDirection="Horizontal">
                                    </asp:CheckBoxList>
                                    <%--<asp:Checkbox ID="chkGMarkAllY" runat="server" AutoPostBack="false" style="position:relative;left:2px;top:-6px" />
                                    <asp:Label ID="lblGMarkAllY" runat="server" Text="<%$ Resources: Text, Yes %>" style="position:relative;top:-8px" />--%>
                                </td>
                                <%--<td style="width:50px">
                                    <asp:Checkbox ID="chkGMarkAllN" runat="server" AutoPostBack="false" style="position:relative;top:-6px" />
                                    <asp:Label ID="lblGMarkAllN" runat="server" Text="<%$ Resources: Text, No %>" style="position:relative;top:-8px" />
                                </td>--%>
                            </tr>
                        </table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="border-collapse:collapse;border-spacing:0px">
                            <tr>
                                <td>
                                    <div id="divMarkInjectedMultiple" runat="server">
                                        <asp:RadioButtonList ID="rblGMarkInjected" runat="server" CellSpacing="0" RepeatDirection="Horizontal" style="position:relative;left:3px; min-width:80px;">
                                        </asp:RadioButtonList>
                                    </div>
                                    <div id="divMarkInjectedSingle" runat="server">
                                        <input type="checkbox" id="chkGMark1stVisit" runat="server" value="1" style="position:relative;top:-6px">
                                        <asp:Label ID="lblGMark1stVisit" runat="server" Text="<%$ Resources: Text, 1stVisit %>" style="position:relative;top:-8px" />
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, Injected %>" SortExpression="Injected" ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblGInjected" runat="server" Text='<%# Eval("Injected")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, TransactionNo %>" SortExpression="Transaction_ID" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblGTransactionNo" runat="server" Text='<%# Eval("Transaction_ID")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, TransactionStatus %>" SortExpression="Transaction_Record_Status" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblGTransactionRecordStatus" runat="server" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, FailReason %>" SortExpression="Fail_Reason" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblGFailReason" runat="server" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="<%$ Resources: Text, WarningMessage %>" SortExpression="Upload_Warning" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblGWarningMessage" runat="server" Text='<%# Eval("Upload_Warning") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, AccountID_ReferenceNo %>" SortExpression="Real_Account_ID_Reference_No" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblGAccountIDReferenceNo" runat="server" Text='<%# Eval("Real_Account_ID_Reference_No")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, Status %>" SortExpression="Acc_Record_Status_Desc" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblGAccRecordStatus" runat="server" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, AccountValidationResult %>" SortExpression="Acc_Validation_Result" ItemStyle-Width="100">
                    <ItemTemplate>
                        <asp:Label ID="lblGAccountValidationResult" runat="server" Text='<%# Eval("Acc_Validation_Result") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, FieldDiff %>" SortExpression="Field_Diff" ItemStyle-Width="40">
                    <ItemTemplate>
                        <asp:Label ID="lblGFieldDiff" runat="server" Text='<%# Eval("Field_Diff")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
</asp:Panel>

