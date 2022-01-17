<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucPreCheckDetail.ascx.vb" Inherits="HCSP.ucPreCheckDetail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<div class="eHSTableHeading">
    <asp:Label ID="lblDStudentFile" runat="server" Text="<%$ Resources: Text, VaccinationFile %>"></asp:Label>
</div>
<table class="tblSFD">
    <tr>
        <td style="width: 260px">
            <asp:Label ID="lblDStudentFileIDText" runat="server" Text="<%$ Resources: Text, PreCheckFileID %>"></asp:Label>
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
            <asp:Label ID="lblDSchoolCodeText" runat="server" Text="<%$ Resources: Text, RCHCode %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDSchoolCode" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr style="vertical-align:top">
        <td style="padding-top:2px">
            <asp:Label ID="lblDSchoolNameText" runat="server" Text="<%$ Resources: Text, RCHName %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDSchoolName" runat="server" class="tableText"></asp:Label>
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
    <tr id="trDLastRectifiedBy" runat="server">
        <td>
            <asp:Label ID="lblDLastRectifiedByText" runat="server" Text="<%$ Resources: Text, LastRectifiedBy %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDLastRectifiedBy" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trDStatus" runat="server">
        <td>
            <asp:Label ID="lblDStatusText" runat="server" Text="<%$ Resources: Text, Status %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDStatus" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
    <tr id="trDNoOfClient" runat="server">
        <td>
            <asp:Label ID="lblDNoOfClientText" runat="server" Text="<%$ Resources: Text, NoOfClient %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDNoOfClient" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>
<%--    <tr id="trDNoOfClientNotInject" runat="server">
        <td>
            <asp:Label ID="lblDNoOfClientNotInjectText" runat="server" Text="<%$ Resources: Text, NoOfClientNotInject %>"></asp:Label>
        </td>
        <td>
            <asp:Label ID="lblDNoOfClientNotInject" runat="server" class="tableText"></asp:Label>
        </td>
    </tr>--%>
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
            <asp:Label ID="lblInjectionSummaryWithoutInput" runat="server" CssClass="tableText" visible="false" text="No Mark Injection Record"/>
            <table id="tblInjectionSummary" runat="server" border="1" style="border-collapse:collapse;border-spacing:1px">
                <tr style="vertical-align:top">
                    <th id="thVaccinationFileID" runat="server" style="width:120px;height:22px;vertical-align:middle;display:none">
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
    <%--<tr style="height: 10px">--%>
    <%--</tr>--%>
</table>
<asp:Panel ID="pnlInformation" runat="server">
    <br />
    <div id="divDClassAndStudentInformation" runat="server" class="eHSTableHeading">
        <asp:Label ID="lblDClassAndStudentInformation" runat="server" Text="<%$ Resources: Text, ClientInformation %>"></asp:Label>
    </div>
</asp:Panel>
<asp:Label ID="lblDMessage" runat="server" Style="display: block; padding-top: 5px; padding-left: 5px; height: 60px; font-style: italic"></asp:Label>
<asp:Panel ID="panD" runat="server">
    <table class="tblSFD">
        <tr>
            <td style="width: 260px">
                <asp:Label ID="lblDClassNameText" runat="server" Text="<%$ Resources: Text, Category %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlDClassName" runat="server" Width="200" AutoPostBack="true"
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
                <asp:TemplateField HeaderText="<%$ Resources: Text, RefNoShort %>" SortExpression="Class_No_Sort" ItemStyle-Width="30">
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

                <asp:TemplateField ItemStyle-Width="90">
                    <HeaderTemplate>
                        <asp:Label ID="lblGMarkInjected" runat="server" Text="<%$ Resources: Text, ActualInjected %>" style="position:relative;top:-6px" />
                        <br />
                        <asp:Checkbox ID="chkGMarkAllY" runat="server" AutoPostBack="false" style="position:relative;top:-6px" />
                        <asp:Label ID="lblGMarkAllY" runat="server" Text="<%$ Resources: Text, MarkAllY %>" style="position:relative;top:-8px" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="border-collapse:collapse;border-spacing:0px">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rblGMarkInjected" runat="server" Width ="90" RepeatDirection="Horizontal" RepeatColumns="2">
                                        <asp:ListItem Text="<%$ Resources: Text, SimpleYes %>" Value="Y" />
                                        <asp:ListItem Text="<%$ Resources: Text, SimpleNo %>" Value="N" />
                                    </asp:RadioButtonList>
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
<asp:Panel ID="pnlAssignDate" runat="server">
    <hr />
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
                <asp:TextBox ID="txtAVaccinationDateQIV1" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAVaccinationDateQIV1" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAVaccinationDateQIV1Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px" />
                <cc1:CalendarExtender ID="calAVaccinationDateQIV1" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAVaccinationDateQIV1"
                    TargetControlID="txtAVaccinationDateQIV1" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAVaccinationDateQIV1" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAVaccinationDateQIV1" ValidChars="-"></cc1:FilteredTextBoxExtender>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:TextBox ID="txtAVaccinationDateQIV2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAVaccinationDateQIV2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAVaccinationDateQIV2Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px"/>
                <cc1:CalendarExtender ID="calAVaccinationDateQIV2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAVaccinationDateQIV2"
                    TargetControlID="txtAVaccinationDateQIV2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAVaccinationDateQIV2" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAVaccinationDateQIV2" ValidChars="-"></cc1:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr id="trAssignDateQIV_3" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblAGenerationDateQIV" runat="server" class="tableText" Font-Size="14px"
                     Text="<%$ Resources: Text, VaccinationReportGenerationDate %>" Style="position: relative; Left: 10px" />
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:TextBox ID="txtAGenerationDateQIV1" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAGenerationDateQIV1" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAGenerationDateQIV1Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px" />
                <cc1:CalendarExtender ID="calAGenerationDateQIV1" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAGenerationDateQIV1"
                    TargetControlID="txtAGenerationDateQIV1" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAGenerationDateQIV1" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAGenerationDateQIV1" ValidChars="-"></cc1:FilteredTextBoxExtender>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:TextBox ID="txtAGenerationDateQIV2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAGenerationDateQIV2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAGenerationDateQIV2Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px" />
                <cc1:CalendarExtender ID="calAGenerationDateQIV2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAGenerationDateQIV2"
                    TargetControlID="txtAGenerationDateQIV2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAGenerationDateQIV2" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAGenerationDateQIV2" ValidChars="-"></cc1:FilteredTextBoxExtender>
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
                <asp:TextBox ID="txtAVaccinationDate23vPPV1" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAVaccinationDate23vPPV1" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAVaccinationDate23vPPV1Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px" />
                <cc1:CalendarExtender ID="calAVaccinationDate23vPPV1" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAVaccinationDate23vPPV1"
                    TargetControlID="txtAVaccinationDate23vPPV1" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAVaccinationDate23vPPV1" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAVaccinationDate23vPPV1" ValidChars="-"></cc1:FilteredTextBoxExtender>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
<%--                <asp:TextBox ID="txtAVaccinationDate23vPPV2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAVaccinationDate23vPPV2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAVaccinationDate23vPPV2Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px"/>
                <cc1:CalendarExtender ID="calAVaccinationDate23vPPV2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAVaccinationDate23vPPV2"
                    TargetControlID="txtAVaccinationDate23vPPV2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAVaccinationDate23vPPV2" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAVaccinationDate23vPPV2" ValidChars="-"></cc1:FilteredTextBoxExtender>--%>
            </td>
        </tr>
        <tr id="trAssignDate23vPPV_4" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblAGenerationDate23vPPV" runat="server" class="tableText" Font-Size="14px"
                     Text="<%$ Resources: Text, VaccinationReportGenerationDate %>" Style="position: relative; Left: 10px" />
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:TextBox ID="txtAGenerationDate23vPPV1" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAGenerationDate23vPPV1" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAGenerationDate23vPPV1Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px" />
                <cc1:CalendarExtender ID="calAGenerationDate23vPPV1" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAGenerationDate23vPPV1"
                    TargetControlID="txtAGenerationDate23vPPV1" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAGenerationDate23vPPV1" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAGenerationDate23vPPV1" ValidChars="-"></cc1:FilteredTextBoxExtender>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
<%--                <asp:TextBox ID="txtAGenerationDate23vPPV2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAGenerationDate23vPPV2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAGenerationDate23vPPV2Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px" />
                <cc1:CalendarExtender ID="calAGenerationDate23vPPV2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAGenerationDate23vPPV2"
                    TargetControlID="txtAGenerationDate23vPPV2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAGenerationDate23vPPV2" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAGenerationDate23vPPV2" ValidChars="-"></cc1:FilteredTextBoxExtender>--%>
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
                <asp:TextBox ID="txtAVaccinationDatePCV131" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAVaccinationDatePCV131" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAVaccinationDatePCV131Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px" />
                <cc1:CalendarExtender ID="calAVaccinationDatePCV131" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAVaccinationDatePCV131"
                    TargetControlID="txtAVaccinationDatePCV131" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAVaccinationDatePCV131" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAVaccinationDatePCV131" ValidChars="-"></cc1:FilteredTextBoxExtender>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
<%--                <asp:TextBox ID="txtAVaccinationDatePCV132" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAVaccinationDatePCV132" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAVaccinationDatePCV132Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px"/>
                <cc1:CalendarExtender ID="calAVaccinationDatePCV132" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAVaccinationDatePCV132"
                    TargetControlID="txtAVaccinationDatePCV132" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAVaccinationDatePCV132" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAVaccinationDatePCV132" ValidChars="-"></cc1:FilteredTextBoxExtender>--%>
            </td>
        </tr>
        <tr id="trAssignDatePCV13_4" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblAGenerationDatePCV13" runat="server" class="tableText" Font-Size="14px"
                     Text="<%$ Resources: Text, VaccinationReportGenerationDate %>" Style="position: relative; Left: 10px" />
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:TextBox ID="txtAGenerationDatePCV131" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAGenerationDatePCV131" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAGenerationDatePCV131Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px" />
                <cc1:CalendarExtender ID="calAGenerationDatePCV131" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAGenerationDatePCV131"
                    TargetControlID="txtAGenerationDatePCV131" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAGenerationDatePCV131" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAGenerationDatePCV131" ValidChars="-"></cc1:FilteredTextBoxExtender>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
<%--                <asp:TextBox ID="txtAGenerationDatePCV132" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAGenerationDatePCV132" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAGenerationDatePCV132Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px" />
                <cc1:CalendarExtender ID="calAGenerationDatePCV132" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAGenerationDatePCV132"
                    TargetControlID="txtAGenerationDatePCV132" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAGenerationDatePCV132" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAGenerationDatePCV132" ValidChars="-"></cc1:FilteredTextBoxExtender>--%>
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
                <asp:TextBox ID="txtAVaccinationDateMMR1" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAVaccinationDateMMR1" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAVaccinationDateMMR1Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px" />
                <cc1:CalendarExtender ID="calAVaccinationDateMMR1" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAVaccinationDateMMR1"
                    TargetControlID="txtAVaccinationDateMMR1" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAVaccinationDateMMR1" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAVaccinationDateMMR1" ValidChars="-"></cc1:FilteredTextBoxExtender>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:TextBox ID="txtAVaccinationDateMMR2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAVaccinationDateMMR2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAVaccinationDateMMR2Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px"/>
                <cc1:CalendarExtender ID="calAVaccinationDateMMR2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAVaccinationDateMMR2"
                    TargetControlID="txtAVaccinationDateMMR2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAVaccinationDateMMR2" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAVaccinationDateMMR2" ValidChars="-"></cc1:FilteredTextBoxExtender>
            </td>
        </tr>
        <tr id="trAssignDateMMR_4" runat="server">
            <td style="height:22px;vertical-align:middle;text-align:left">
                <asp:Label ID="lblAGenerationDateMMR" runat="server" class="tableText" Font-Size="14px"
                     Text="<%$ Resources: Text, VaccinationReportGenerationDate %>" Style="position: relative; Left: 10px" />
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:TextBox ID="txtAGenerationDateMMR1" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAGenerationDateMMR1" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAGenerationDateMMR1Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px" />
                <cc1:CalendarExtender ID="calAGenerationDateMMR1" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAGenerationDateMMR1"
                    TargetControlID="txtAGenerationDateMMR1" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAGenerationDateMMR1" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAGenerationDateMMR1" ValidChars="-"></cc1:FilteredTextBoxExtender>
            </td>
            <td style="height:22px;vertical-align:middle;text-align:left;padding-left:10px">
                <asp:TextBox ID="txtAGenerationDateMMR2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                <asp:ImageButton ID="ibtnAGenerationDateMMR2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                <asp:Image ID="imgAGenerationDateMMR2Error" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Style="position: relative; top: 4px" />
                <cc1:CalendarExtender ID="calAGenerationDateMMR2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnAGenerationDateMMR2"
                    TargetControlID="txtAGenerationDateMMR2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                <cc1:FilteredTextBoxExtender ID="fteAGenerationDateMMR2" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtAGenerationDateMMR2" ValidChars="-"></cc1:FilteredTextBoxExtender>
            </td>
        </tr>--%>

    </table>
    <br />
</asp:Panel>
<asp:Panel ID="pnlMarkVaccination" runat="server">
    <table class="tblSFD">
        <tr>
            <td style="width: 260px">
                <asp:Label ID="lblMCategoryText" runat="server" Text="<%$ Resources: Text, Category %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlMCategory" runat="server" Width="200" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlMCategory_SelectedIndexChanged" />
            </td>
        </tr>
        <tr>
            <td style="width: 260px">
                <asp:Label ID="lblMSubsidyText" runat="server" Text="<%$ Resources: Text, Subsidy %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlMSubsidy" runat="server" Width="200" AutoPostBack="true"
                    OnSelectedIndexChanged="ddlMSubsidy_SelectedIndexChanged"  />
            </td>
        </tr>
<%--        <tr>
            <td style="width: 260px">
                <asp:Label ID="lblMDoseToInjectText" runat="server" Text="<%$ Resources: Text, DoseToInject %>"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="ddlMDoseToInject" runat="server" Width="200" />
            </td>
        </tr>--%>
        <tr id="trMVaccinationDate" runat="server">
            <td colspan="2">
<%--            <td>
                <asp:Label ID="lblMVaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblMVaccinationDate" runat="server" class="tableText"></asp:Label>
            </td>--%>
                <table style="border-collapse:collapse">
                    <tr>
                        <td style="width: 260px;height:22px">
                            <asp:Label ID="lblMDoseToInjectText" runat="server" Text="<%$ Resources: Text, DoseToInject %>" />
                        </td>
                        <td style="width: 200px">
                            <asp:Label ID="lblMDoseToInject1" runat="server" class="tableText" Text="<%$ Resources: Text, OnlyOr1stDose %>" />
                        </td>
                        <td style="width: 200px">
                            <asp:Label ID="lblMDoseToInject2" runat="server" class="tableText" Text="<%$ Resources: Text, 2ndDose %>" />
                        </td>                                  
                    </tr>
                    <tr>
                        <td style="width: 260px;height:22px">
                            <asp:Label ID="lblMVaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>" />
                        </td>
                        <td style="width: 200px">
                            <asp:Label ID="lblMVaccinationDate1" runat="server" class="tableText" />
                        </td>
                        <td style="width: 200px">
                            <asp:Label ID="lblMVaccinationDate2" runat="server" class="tableText" />
                        </td>   
                    </tr>
                    <tr>
                        <td style="width: 260px;height:22px">
                            <asp:Label ID="lblMGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"/>
                        </td>
                        <td style="width: 200px">
                           <asp:Label ID="lblMGenerationDate1" runat="server" class="tableText" />
                        </td>
                        <td style="width: 200px">
                           <asp:Label ID="lblMGenerationDate2" runat="server" class="tableText" />
                        </td> 
                    </tr>
                </table>
            </td>
        </tr>
<%--        <tr id="trMGenerationDate" runat="server">
            <td>
                <asp:Label ID="lblMGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblMGenerationDate" runat="server" class="tableText"></asp:Label>
            </td>
        </tr>--%>
        <tr id="trMNoOfClient" runat="server">
            <td>
                <asp:Label ID="lblMNoOfClientText" runat="server" Text="<%$ Resources: Text, NoOfClient %>"></asp:Label>
            </td>
            <td>
                <asp:Label ID="lblMNoOfClient" runat="server" class="tableText"></asp:Label>
            </td>
        </tr>
    </table>
    <br />
    <div style="min-height: 80px">
        <asp:GridView ID="gvM" runat="server" CssClass="gvTable" Width="1200" AutoGenerateColumns="False" AllowPaging="True"
            AllowSorting="True" OnRowDataBound="gvM_RowDataBound" OnPreRender="gvM_PreRender" OnSorting="gvM_Sorting"
            OnPageIndexChanging="gvM_PageIndexChanging">
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources: Text, SeqNo %>" SortExpression="Student_Seq" ItemStyle-Width="0">
                    <ItemTemplate>
                        <asp:Label ID="lblMSeqNo" runat="server" Text='<%# Eval("Student_Seq")%>' />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, DocTypeIDNL %>" SortExpression="DocCode_DocNo" ItemStyle-Width="0">
                    <ItemTemplate>
                        <asp:Label ID="lblMDocType" runat="server" Text='<%# Eval("Doc_Code") %>' />
                        <br />
                        <asp:Label ID="lblMDocNo" runat="server" Text='<%# Eval("Doc_No") %>' />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, Name %>" SortExpression="NameEN_NameCH" ItemStyle-Width="0">
                    <ItemTemplate>
                        <asp:Label ID="lblMNameEN" runat="server" Text='<%# Eval("Name_EN")%>' />
                        <br />
                        <asp:Label ID="lblMNameCH" runat="server" Text='<%# Eval("Name_CH")%>' CssClass="TextChineseName" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, Sex %>" SortExpression="Sex" ItemStyle-Width="0">
                    <ItemTemplate>
                        <asp:Label ID="lblMSex" runat="server" Text='<%# Eval("Sex") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, Status %>" SortExpression="Acc_Record_Status_Desc" ItemStyle-Width="0">
                    <ItemTemplate>
                        <asp:Label ID="lblMAccRecordStatus" runat="server" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, CheckDate %>" SortExpression="CheckDate" ItemStyle-Width="0">
                    <ItemTemplate>
                        <asp:Label ID="lblMCheckDate" runat="server" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, OnlyDose %>" SortExpression="OnlyDose" ItemStyle-Width="0">
                    <ItemTemplate>
                        <asp:Label ID="lblMOnlyDose" runat="server" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, 1stDose2 %>" SortExpression="FirstDose"  ItemStyle-Width="50">
                    <ItemTemplate>
                        <asp:Label ID="lblM1stDose" runat="server" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, 2ndDose %>" SortExpression="SecondDose" ItemStyle-Width="0">
                    <ItemTemplate>
                        <asp:Label ID="lblM2ndDose" runat="server" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, Remarks %>" SortExpression="MarkInjectRemark" ItemStyle-Width="0">
                    <ItemTemplate>
                        <asp:Label ID="lblMRemarks" runat="server" />
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
                <asp:TemplateField ItemStyle-Width="0">
                    <HeaderTemplate>
                        <asp:Label ID="lblMMarkInjected" runat="server" Text="<%$ Resources: Text, MarkInject %>" style="position:relative;top:-6px" />
                        <br />
                        <asp:Checkbox ID="chkMMarkAllY" runat="server" AutoPostBack="false" style="position:relative;top:-6px" />
                        <asp:Label ID="lblMMarkAllY" runat="server" Text="<%$ Resources: Text, MarkAllY %>" style="position:relative;top:-8px" />
                    </HeaderTemplate>
                    <ItemTemplate>
                        <table style="border-collapse:collapse;border-spacing:0px">
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rblMMarkInjected" runat="server" Width ="80" RepeatDirection="Horizontal" RepeatColumns="2">
                                        <asp:ListItem Text="<%$ Resources: Text, SimpleYes %>" Value="Y" />
                                        <asp:ListItem Text="<%$ Resources: Text, SimpleNo %>" Value="N" />
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                        </table>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" HorizontalAlign="Center" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources: Text, Injected %>" SortExpression="Injected" ItemStyle-Width="0">
                    <ItemTemplate>
                        <asp:Label ID="lblMInjected" runat="server" Text='<%# Eval("Injected")%>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle BackColor="White" />
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </div>
</asp:Panel>

