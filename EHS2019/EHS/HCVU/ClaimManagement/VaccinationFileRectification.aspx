<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="VaccinationFileRectification.aspx.vb" Inherits="HCVU.VaccinationFileRectification"
    Title="<%$ Resources: Title, VaccinationFileRectification %>" EnableEventValidation="False" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="../UIControl/StudentFile/ucStudentFileDetail.ascx" TagName="ucStudentFileDetail" TagPrefix="uc1" %>
<%@ Register Src="../UIControl/DocTypeHCSP/ucInputDocumentType.ascx" TagName="ucInputDocumentType" TagPrefix="uc2" %>
<%@ Register Src="../UIControl/DocTypeHCSP/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType" TagPrefix="uc3" %>
<%@ Register Src="../UIControl/ChooseCCCode.ascx" TagName="ChooseCCCode" TagPrefix="uc4" %>
<%@ Register Src="../UIControl/SchemeDocTypeLegend.ascx" TagName="SchemeDocTypeLegend" TagPrefix="uc5" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <style type="text/css">
        table.gvTable {
            width: 100%;
        }

            table.gvTable td {
                vertical-align: top;
            }

        table.tblS {
            width: 100%;
        }

            table.tblS > tbody > tr {
                height: 28px;
            }

            table.tblS td {
                vertical-align: top;
            }

        table.tblSFD > tbody > tr {
            height: 25px;
        }

        table.tblSF {
            width: 100%;
        }

            table.tblSF > tbody > tr {
                height: 30px;
            }

            table.tblSF td {
                vertical-align: top;
            }

        table.tblI2DoseToInject td {
            padding-right: 15px;
        }

        table.tblSF2 {
            width: 100%;
        }

            table.tblSF2 > tbody > tr {
                height: 24px;
            }

            table.tblSF2 td {
                vertical-align: top;
            }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../JS/Common.js"></script>
    <script type="text/javascript">
<%--        function SaveFilePathToHiddenField() {
            document.getElementById('<%= hfIFile.ClientID %>').value = document.getElementById('<%= flIStudentFile.ClientID %>').value;
        }--%>
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, VaccinationFileRectificationBanner %>"
        AlternateText="<%$ Resources: AlternateText, VaccinationFileRectificationBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <%--<asp:PostBackTrigger ControlID="ibtnDUploadRectifiedFile" />--%>
            <asp:PostBackTrigger ControlID="ibtnINext" />
            <asp:PostBackTrigger ControlID="ibtnCBack" />
        </Triggers>
        <ContentTemplate>
            <div style="height: 4px"></div>
            <asp:HiddenField ID="hfIFile" runat="server" />
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" style="width:950px;display:block" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" style="width:950px;display:block" />
            <asp:MultiView ID="mvCore" runat="server">
                <asp:View ID="vSearch" runat="server">
                    <table style="width: 100%" class="tblS">
                        <tr>
                            <td style="width: 160px">
                                <asp:Label ID="lblSSchemeText" runat="server" Text="<%$ Resources: Text, Scheme %>"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSScheme" runat="server" Width="300"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSStudentFileIDText" runat="server" Text="<%$ Resources: Text, VaccinationFileID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSStudentFileID" runat="server" Width="200" MaxLength="15"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSSchoolCodeText" runat="server" Text="<%$ Resources: Text, SchoolRCHCode %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSSchoolCode" runat="server" Width="200" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSServiceProviderIDText" runat="server" Text="<%$ Resources: Text, ServiceProviderID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSServiceProviderID" runat="server" Width="200" MaxLength="8"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSVaccinationDate" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSVaccinationDateFrom" runat="server" Width="80" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnSVaccinationDateFrom" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                            <cc1:CalendarExtender ID="calSVaccinationDateFrom" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnSVaccinationDateFrom"
                                                TargetControlID="txtSVaccinationDateFrom" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="fteSVaccinationDateFrom" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="txtSVaccinationDateFrom" ValidChars="-">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Image ID="imgErrorSVaccinationDateFrom" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                        </td>
                                        <td style="padding-left: 15px; padding-right: 15px">
                                            <asp:Label ID="lblSTo" runat="server" Text="<%$ Resources: Text, To %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSVaccinationDateTo" runat="server" Width="80" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnSVaccinationDateTo" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                            <cc1:CalendarExtender ID="calSVaccinationDateTo" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnSVaccinationDateTo"
                                                TargetControlID="txtSVaccinationDateTo" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="txtSVaccinationDateTo" ValidChars="-">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Image ID="imgErrorSVaccinationDateTo" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSStatusText" runat="server" Text="<%$ Resources: Text, Status %>"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSStatus" runat="server" Width="300"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSVaccinationSeasonText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, VaccinationSeason %>'></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblSVaccinationSeason" runat="server" Width="300" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="RadioButtonList" RepeatColumns ="2" Style="position:relative;left:-3px;top:-4px">
                                    <asp:ListItem Text='<%$ Resources:Text, CurrentSeason %>' Selected="True" Value="C"></asp:ListItem>
                                    <asp:ListItem Text='<%$ Resources:Text, PastSeason %>' Value="P"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center">
                                <asp:ImageButton ID="ibtnSSearch" runat="server" ImageUrl="<%$ Resources: ImageUrl, SearchBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SearchBtn %>" OnClick="ibtnSSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vResult" runat="server">
                    <asp:GridView ID="gvR" runat="server" CssClass="gvTable" AutoGenerateColumns="False" AllowPaging="True"
                        AllowSorting="True" OnRowDataBound="gvR_RowDataBound" OnPreRender="gvR_PreRender"
                        OnRowCommand="gvR_RowCommand" OnSorting="gvR_Sorting" OnPageIndexChanging="gvR_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationFileID %>" SortExpression="Student_File_ID" ItemStyle-Width="130">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnGStudentFileID" runat="server" Text='<%# Eval("Student_File_ID") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Scheme %>" SortExpression="Scheme_Code" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblGSchemeCode" runat="server" Text='<%# Eval("Scheme_Display_Code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, SchoolRCHCode %>" SortExpression="School_Code" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:Label ID="lblGSchoolCode" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, SPID %>" SortExpression="SP_ID" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblGSPID" runat="server" Text='<%# Eval("SP_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationDate %>" SortExpression="Service_Receive_Dtm" ItemStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="lblGVaccinationDate" runat="server" />
                                    <br />
                                    <asp:Label ID="lblGVaccinationDate_2" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationReportGenerationDate %>" SortExpression="Final_Checking_Report_Generation_Date"
                                ItemStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="lblGVaccinationReportGenerationDate" runat="server" />
                                    <br />
                                    <asp:Label ID="lblGVaccinationReportGenerationDate_2" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, SubsidyDoseToInject %>" SortExpression="Dose" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:Label ID="lblGDoseToInject" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, UploadByAndTime %>" SortExpression="Upload_Dtm" ItemStyle-Width="140">
                                <ItemTemplate>
                                    <asp:Label ID="lblGUploadByAndTime" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Status %>" SortExpression="Record_Status">
                                <ItemTemplate>
                                    <asp:Label ID="lblGStatus" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnRBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnRBack_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vDetail" runat="server">
                    <div style="width: 980px; text-align: right">
                        <asp:ImageButton ID="ibtnDShowRectification" runat="server" ImageUrl="<%$ Resources: ImageUrl, ShowRectificationRecordBtn %>"
                            AlternateText="<%$ Resources: AlternateText, ShowRectificationRecordBtn %>" OnClick="ibtnDShowRectification_Click" />
                    </div>
                    <uc1:ucStudentFileDetail ID="udcStudentFileDetail" runat="server"></uc1:ucStudentFileDetail>
                    <table>
                        <tr>
                            <td style="width: 260px">
                                <asp:ImageButton ID="ibtnDBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                            </td>
                            <td>
<%--                                <asp:ImageButton ID="ibtnDDownloadRectifyReport" runat="server" ImageUrl="<%$ Resources: ImageUrl, DownloadRectifyReportBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, DownloadRectifyReportBtn %>" OnClick="ibtnDDownloadRectifyReport_Click" />--%>
<%--                                <asp:ImageButton ID="ibtnDUploadRectifiedFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, UploadRectifiedFileBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, UploadRectifiedFileBtn %>" OnClick="ibtnDUploadRectifiedFile_Click" />--%>
                                <asp:ImageButton ID="ibtnDEditInformation" runat="server" ImageUrl="<%$ Resources: ImageUrl, EditInformationBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, EditInformationBtn %>" OnClick="ibtnDEditInformation_Click" />
                                <asp:ImageButton ID="ibtnDRemoveRectifiedFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, RemoveRectifiedFileBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, RemoveRectifiedFileBtn %>" OnClick="ibtnDRemoveRectifiedFile_Click" />
                                <asp:ImageButton ID="ibtnDRemoveVaccinationFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, RemoveVaccinationFileBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, RemoveVaccinationFileBtn %>" OnClick="ibtnDRemoveVaccinationFile_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vImport" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblIUploadStudentFile" runat="server" Text="{CodeBehind}"></asp:Label>
                    </div>
                    <div style="height: 6px"></div>
                    <table class="tblSF">
                        <tr>
                            <td style="width: 260px">
                                <asp:Label ID="lblIStudentFileIDText" runat="server" Text="<%$ Resources: Text, VaccinationFileID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblIStudentFileID" runat="server" class="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblISchemeText" runat="server" Text="<%$ Resources: Text, Scheme %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblIScheme" runat="server" class="tableText"></asp:Label>
                            </td>
                        </tr>
                        <asp:Panel ID="panISchoolRCH" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lblISchoolCodeText" runat="server" Text="<%$ Resources: Text, SchoolCode %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblISchoolCode" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblISchoolNameText" runat="server" Text="<%$ Resources: Text, SchoolName %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblISchoolName" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td>
                                <asp:Label ID="lblIServiceProviderIDText" runat="server" Text="<%$ Resources: Text, SPID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblIServiceProviderID" runat="server" class="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblIServiceProviderNameText" runat="server" Text="<%$ Resources: Text, ServiceProviderName %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblIServiceProviderName" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblIPracticeText" runat="server" Text="<%$ Resources: Text, Practice %>"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlIPractice" runat="server" Width="400"></asp:DropDownList>
                                <asp:Image ID="imgErrorIPractice" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                            </td>
                        </tr>
                        <asp:Panel ID="panIVaccinationInfo" runat="server">        
                            <tr>
                                <td colspan="2">
                                    <table class="tblSF" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:264px"></td>
                                            <td style="width:200px">
                                                <asp:Label ID="lblIOnlyDoseText" runat="server" CssClass="tableText" Text="<%$ Resources: Text, OnlyOr1stDose %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblI2ndDoseText" runat="server" CssClass="tableText" Text="<%$ Resources: Text, 2ndDose %>"></asp:Label>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblIVaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIVaccinationDate1" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:TextBox ID="txtIVaccinationDate1" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnIVaccinationDate1" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                                <asp:Image ID="imgErrorIVaccinationDate1" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                                <cc1:CalendarExtender ID="calIVaccinationDate1" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationDate1"
                                                    TargetControlID="txtIVaccinationDate1" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteIVaccinationDate1" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationDate1" ValidChars="-">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIVaccinationDate2" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:TextBox ID="txtIVaccinationDate2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnIVaccinationDate2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                                <asp:Image ID="imgErrorIVaccinationDate2" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                                <cc1:CalendarExtender ID="calIVaccinationDate2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationDate2"
                                                    TargetControlID="txtIVaccinationDate2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteIVaccinationDate2" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationDate2" ValidChars="-">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblIVaccinationReportGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIVaccinationReportGenerateDate1" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:TextBox ID="txtIVaccinationReportGenerateDate1" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnIVaccinationReportGenerateDate1" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                                <asp:Image ID="imgErrorIVaccinationReportGenerationDate1" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                                <cc1:CalendarExtender ID="calIVaccinationReportGenerateDate1" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationReportGenerateDate1"
                                                    TargetControlID="txtIVaccinationReportGenerateDate1" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteIVaccinationReportGenerateDate1" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationReportGenerateDate1" ValidChars="-">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIVaccinationReportGenerateDate2" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:TextBox ID="txtIVaccinationReportGenerateDate2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnIVaccinationReportGenerateDate2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                                <asp:Image ID="imgErrorIVaccinationReportGenerationDate2" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                                <cc1:CalendarExtender ID="calIVaccinationReportGenerateDate2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationReportGenerateDate2"
                                                    TargetControlID="txtIVaccinationReportGenerateDate2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteIVaccinationReportGenerateDate2" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationReportGenerateDate2" ValidChars="-">
                                                </cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr id="tr2ndVaccinationDate" runat="server">
                                            <td>
                                                <asp:Label ID="lblVaccinationDateText_2" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIVaccinationDate1_2" runat="server" CssClass="tableText" />
                                                <asp:TextBox ID="txtIVaccinationDate1_2" runat="server" Width="100" MaxLength="10" style="position:relative;top:-1px" />
                                                <asp:ImageButton ID="ibtnIVaccinationDate1_2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position:relative;top:2px" />
                                                <asp:Image ID="imgErrorIVaccinationDate1_2" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" style="position:relative;top:3px" />
                                                <cc1:CalendarExtender ID="calIVaccinationDate1_2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationDate1_2"
                                                    TargetControlID="txtIVaccinationDate1_2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteIVaccinationDate1_2" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationDate1_2" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIVaccinationDate2_2" runat="server" CssClass="tableText" />
                                                <asp:TextBox ID="txtIVaccinationDate2_2" runat="server" Width="100" MaxLength="10" style="position:relative;top:-1px" />
                                                <asp:ImageButton ID="ibtnIVaccinationDate2_2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position:relative;top:2px" />
                                                <asp:Image ID="imgErrorIVaccinationDate2_2" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" style="position:relative;top:3px" />
                                                <cc1:CalendarExtender ID="calIVaccinationDate2_2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationDate2_2"
                                                    TargetControlID="txtIVaccinationDate2_2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteIVaccinationDate2_2" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationDate2_2" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                        <tr id="tr2ndReportGenerationDate" runat="server">
                                            <td>
                                                <asp:Label ID="lblVaccinationReportGenerationDateText_2" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIVaccinationReportGenerateDate1_2" runat="server" CssClass="tableText" />
                                                <asp:TextBox ID="txtIVaccinationReportGenerateDate1_2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnIVaccinationReportGenerateDate1_2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                                <asp:Image ID="imgErrorIVaccinationReportGenerationDate1_2" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" style="position:relative;top:4px" />
                                                <cc1:CalendarExtender ID="calIVaccinationReportGenerateDate1_2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationReportGenerateDate1_2"
                                                    TargetControlID="txtIVaccinationReportGenerateDate1_2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteVaccinationReportGenerateDate1_2" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationReportGenerateDate1_2" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIVaccinationReportGenerateDate2_2" runat="server" CssClass="tableText" />
                                                <asp:TextBox ID="txtIVaccinationReportGenerateDate2_2" runat="server" Width="100" MaxLength="10" style="position:relative;top:-1px" />
                                                <asp:ImageButton ID="ibtnIVaccinationReportGenerateDate2_2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" style="position:relative;top:2px" />
                                                <asp:Image ID="imgErrorIVaccinationReportGenerationDate2_2" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" style="position:relative;top:3px" />
                                                <cc1:CalendarExtender ID="calIVaccinationReportGenerateDate2_2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationReportGenerateDate2_2"
                                                    TargetControlID="txtIVaccinationReportGenerateDate2_2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteIVaccinationReportGenerateDate2_2" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationReportGenerateDate2_2" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblISubsidyText" runat="server" Text="<%$ Resources: Text, Subsidy %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblISubsidy" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblIDoseToInjectText" runat="server" Text="<%$ Resources: Text, DoseToInject %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblIDoseToInject" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblIStatusText" runat="server" Text="<%$ Resources: Text, Status %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblIStatus" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="panIMMR" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lblIGenerationDateMMR" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtIVaccinationReportGenerateDateMMR" runat="server" Width="100" MaxLength="10" Style="position:relative;top:-3px" />
                                    <asp:ImageButton ID="ibtnIVaccinationReportGenerateDateMMR" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" />
                                    <asp:Image ID="imgErrorIVaccinationReportGenerationDateMMR" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                    <cc1:CalendarExtender ID="calIVaccinationReportGenerateDateMMR" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationReportGenerateDateMMR"
                                        TargetControlID="txtIVaccinationReportGenerateDateMMR" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                    <cc1:FilteredTextBoxExtender ID="fteVaccinationReportGenerateDateMMR" runat="server" FilterType="Custom, Numbers"
                                        TargetControlID="txtIVaccinationReportGenerateDateMMR" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblISubsidyMMRText" runat="server" Text="<%$ Resources: Text, Subsidy %>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblISubsidyMMR" runat="server" class="tableText" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblIDoseOfMMRText" runat="server" Text="<%$ Resources: Text, DoseToInject %>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblIDoseOfMMR" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                        </asp:Panel>
                        <%--<tr id="trIStudentFile" runat="server">
                            <td style="padding-top: 7px">
                                <asp:Label ID="lblIStudentFileText" runat="server" Text="<%$ Resources: Text, VaccinationFile %>"></asp:Label>
                            </td>
                            <td style="vertical-align: top">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:RadioButtonList ID="rblIStudentFile" runat="server" RepeatDirection="Vertical" Style="position:relative;left:-5px;">
                                                            <asp:ListItem Text="<%$ Resources: Text, UploadRectifyStudentFileNo %>" Value="N"></asp:ListItem>
                                                            <asp:ListItem Text="<%$ Resources: Text, UploadRectifyStudentFileYes %>" Value="Y"></asp:ListItem>
                                                        </asp:RadioButtonList>
                                                    </td>
                                                    <td style="padding-left: 5px; padding-top: 5px">
                                                        <asp:Image ID="imgErrorIStudentFileChoice" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                            AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2" style="padding-left: 24px">
                                            <asp:FileUpload ID="flIStudentFile" runat="server" Width="460px" />
                                            <asp:Image ID="imgErrorIStudentFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>--%>
                        <%--<tr id="trIVaccinationFilePassword" runat="server">
                            <td>
                                <asp:Label ID="lblIVaccineFilePwdText" runat="server" Text="<%$ Resources: Text, VaccinationFilePassword %>"></asp:Label>
                            </td>
                            <td style="padding-left: 25px">
                                <asp:TextBox ID="txtIStudentFilePassword" runat="server" Width="150px" TextMode="Password"></asp:TextBox>
                                <asp:Image ID="imgErrorIStudentFilePassword" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                            </td>
                        </tr>--%>
                        <tr id="trINoOfClass" runat="server">
                            <td>
                                <asp:Label ID="lblINoOfClassText" runat="server" Text="<%$ Resources: Text, NoOfClass %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblINoOfClass" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trINoOfStudent" runat="server">
                            <td>
                                <asp:Label ID="lblINoOfStudentText" runat="server" Text="<%$ Resources: Text, NoOfStudent %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblINoOfStudent" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 256px">
                                            <asp:ImageButton ID="ibtnICancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnICancel_Click" style="position:relative;left:-3px" />
                                        </td>
                                        <td>
                                            <%--<asp:ImageButton ID="ibtnINext" runat="server" ImageUrl="<%$ Resources: ImageUrl, NextBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, NextBtn %>" OnClick="ibtnINext_Click"
                                                OnClientClick="SaveFilePathToHiddenField()" />--%>
                                            <asp:ImageButton ID="ibtnINext" runat="server" ImageUrl="<%$ Resources: ImageUrl, NextBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, NextBtn %>" OnClick="ibtnINext_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vConfirm" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblCUploadStudentFile" runat="server" Text="<%$ Resources: Text, UploadVaccinationFile %>"></asp:Label>
                    </div>
                    <div style="height: 6px"></div>
                    <table class="tblSF">
                        <tr>
                            <td style="width: 260px">
                                <asp:Label ID="lblCStudentFileIDText" runat="server" Text="<%$ Resources: Text, VaccinationFileID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCStudentFileID" runat="server" class="tableText"></asp:Label>
                                <asp:HiddenField ID="hfCUploadStudentFileID" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCSchemeText" runat="server" Text="<%$ Resources: Text, Scheme %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCScheme" runat="server" class="tableText"></asp:Label>
                            </td>
                        </tr>
                        <asp:Panel ID="panCSchoolRCH" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCSchoolCodeText" runat="server" Text="<%$ Resources: Text, SchoolCode %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCSchoolCode" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCSchoolNameText" runat="server" Text="<%$ Resources: Text, SchoolName %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCSchoolName" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td>
                                <asp:Label ID="lblCServiceProviderIDText" runat="server" Text="<%$ Resources: Text, SPID %>">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCServiceProviderID" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCServiceProviderNameText" runat="server" Text="<%$ Resources: Text, ServiceProviderName %>">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCServiceProviderName" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCPracticeText" runat="server" Text="<%$ Resources: Text, Practice %>">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCPractice" runat="server" CssClass="tableText"></asp:Label>
                                <asp:HiddenField ID="hfCPractice" runat="server" />
                            </td>
                        </tr>
                        <asp:Panel ID="panCVaccinationInfo" runat="server">
                            <tr style="height:10px"></tr>
                            <tr>
                                <td colspan="2">
                                    <table class="tblSF" border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td style="width:264px"></td>
                                            <td style="width:200px">
                                                <asp:Label ID="lblCOnlyDoseText" runat="server" CssClass="tableText" Text="<%$ Resources: Text, OnlyOr1stDose %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblC2ndDoseText" runat="server" CssClass="tableText" Text="<%$ Resources: Text, 2ndDose %>"></asp:Label>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCVaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationDate1" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:Label ID="lblCVaccinationDate1Remark" runat="server" ForeColor="red" class="tableText" Text="<%$ Resources: Text, PastDate %>" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationDate2" runat="server" CssClass="tableText"></asp:Label>                                                
                                                <asp:Label ID="lblCVaccinationDate2Remark" runat="server" ForeColor="red" class="tableText" Text="<%$ Resources: Text, PastDate %>" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblCVaccinationReportGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationReportGenerationDate1" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationReportGenerationDate2" runat="server" CssClass="tableText"></asp:Label>

                                            </td>
                                        </tr>

                                        <tr id="tr2ndVaccinationDateConfirm" runat="server">
                                            <td>
                                                <asp:Label ID="lblCVaccinationDateText_2" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationDate1_2" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:Label ID="lblCVaccinationDate1_2Remark" runat="server" ForeColor="red" class="tableText" Text="<%$ Resources: Text, PastDate %>" />
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationDate2_2" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:Label ID="lblCVaccinationDate2_2Remark" runat="server" ForeColor="red" class="tableText" Text="<%$ Resources: Text, PastDate %>" />                                                
                                            </td>
                                        </tr>
                                        <tr id="tr2ndReportGenerationDateConfirm" runat="server">
                                            <td>
                                                <asp:Label ID="lblCVaccinationReportGenerationDateText_2" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationReportGenerationDate1_2" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationReportGenerationDate2_2" runat="server" CssClass="tableText"></asp:Label>

                                            </td>
                                        </tr>

                                        <asp:HiddenField ID="hfCVaccinationDate1" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationReportGenerationDate1" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationDate2" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationReportGenerationDate2" runat="server" />

                                        <asp:HiddenField ID="hfCVaccinationDate1_2" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationReportGenerationDate1_2" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationDate2_2" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationReportGenerationDate2_2" runat="server" />
                                    </table>                                    
                                </td>
                            </tr>
                            <tr id="trCSubsidiy" runat="server">
                                <td>
                                    <asp:Label ID="lblCFDSubsidyText" runat="server" Text="<%$ Resources: Text, Subsidy %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCSubsidy" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCDoseToInjectText" runat="server" Text="<%$ Resources: Text, DoseToInject %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCDoseToInject" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="panCMMR" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCGenerationDateMMRText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblCGenerationDateMMR" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCSubsidyMMRText" runat="server" Text="<%$ Resources: Text, Subsidy %>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblCSubsidyMMR" runat="server" class="tableText" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblCDoseOfMMRText" runat="server" Text="<%$ Resources: Text, DoseToInject %>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblCDoseOfMMR" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                            <asp:HiddenField ID="hfCGenerationDateMMR" runat="server" />
                        </asp:Panel>
                        <tr>
                            <td>
                                <asp:Label ID="lblCStatusText" runat="server" Text="<%$ Resources: Text, Status %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCStatus" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trCStudentFile" runat="server">
                            <td>
                                <asp:Label ID="lblCStudentFileText" runat="server" Text="<%$ Resources: Text, VaccinationFile %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCStudentFile" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trCNoOfClass" runat="server">
                            <td>
                                <asp:Label ID="lblCNoOfClassText" runat="server" Text="<%$ Resources: Text, NoOfClass %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCNoOfClass" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trCNoOfStudent" runat="server">
                            <td>
                                <asp:Label ID="lblCNoOfStudentText" runat="server" Text="<%$ Resources: Text, NoOfStudent %>">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCNoOfStudent" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 256px">
                                            <asp:ImageButton ID="ibtnCBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnCBack_Click" />
                                        </td>
                                        <td style="text-align: center">
                                            <asp:ImageButton ID="ibtnCConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnCConfirm_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vErrorWarning" runat="server">
                    <asp:Panel ID="panE" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblEUploadStudentFile" runat="server" Text="<%$ Resources: Text, UploadVaccinationFile %>"></asp:Label>
                        </div>
                        <div style="height: 6px"></div>
                        <table class="tblSF2">
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblENoOfClassText" runat="server" Text="<%$ Resources: Text, NoOfClass %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblENoOfClass" runat="server" class="tableText"></asp:Label>
                                    <asp:HiddenField ID="hfEGenerationID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblENoOfStudentText" runat="server" Text="<%$ Resources: Text, NoOfStudent %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblENoOfStudent" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblENoOfSuccessfulRecordText" runat="server" Text="<%$ Resources: Text, NoOfSuccessfulRecord %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblENoOfSuccessfulRecord" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblENoOfErrorRecordText" runat="server" Text="<%$ Resources: Text, NoOfErrorRecord %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblENoOfErrorRecord" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblENoOfWarningRecordText" runat="server" Text="<%$ Resources: Text, NoOfWarningRecord %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblENoOfWarningRecord" runat="server" class="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trEOverLimit" runat="server">
                                <td colspan="2" style="padding-top: 8px">
                                    <asp:Label ID="lblEOverLimit" runat="server" ForeColor="Red" Text="{Resources: Text, StudentFileErrorOverLimit}"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvE" runat="server" CssClass="gvTable" Width="1150px" AutoGenerateColumns="False" AllowPaging="True"
                            AllowSorting="False" OnRowDataBound="gvE_RowDataBound" OnPreRender="gvE_PreRender" OnPageIndexChanging="gvE_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, SeqNo %>" ItemStyle-Width="30">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGSeqNo" runat="server" Text='<%# Eval("Student_Seq")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, ClassName %>" ItemStyle-Width="40">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGClassName" runat="server" Text='<%# Eval("Class_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, RectifiedFlag %>" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGRectifiedFlag" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, ClassNo %>" ItemStyle-Width="40">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGClassNo" runat="server" Text='<%# Eval("Class_No") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, DocTypeIDNL %>" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGDocType" runat="server" Text='<%# Eval("Doc_Code") %>' />
                                        <br />
                                        <asp:Label ID="lblGDocNo" runat="server" Text='<%# Eval("Doc_No") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, ContactNo2 %>" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGContactNo" runat="server" Text='<%# Eval("Contact_No") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, ChineseName %>" ItemStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGChineseName" runat="server" Text='<%# Eval("Name_CH_Excel") %>' Font-Names="HA_MingLiu"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, EnglishSurname %>" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGSurnameEN" runat="server" Text='<%# Eval("Surname_EN") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, EnglishGivenName %>" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGGivenNameEN" runat="server" Text='<%# Eval("Given_Name_EN") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, Sex %>" ItemStyle-Width="30">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGSex" runat="server" Text='<%# Eval("Sex") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, DOB %>" ItemStyle-Width="80">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGDOB" runat="server"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, OtherField %>" ItemStyle-Width="120">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGOtherField" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, ConfirmToInject %>" ItemStyle-Width="60">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGConfirmToInject" runat="server" Text=''></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, ErrorMessage %>" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGErrorMessage" runat="server" Text='<%# Eval("Upload_Error") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, WarningMessage %>" ItemStyle-Width="150">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGWarningMessage" runat="server" Text='<%# Eval("Upload_Warning") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                    <table>
                        <tr>
                            <td style="width: 300px">
                                <asp:ImageButton ID="ibtnEReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnEReturn_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnEExportReport" runat="server" ImageUrl="<%$ Resources: ImageUrl, ExportReportBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ExportReportBtn %>" OnClick="ibtnEExportReport_Click" />
                                <asp:ImageButton ID="ibtnEConfirmAcceptWarning" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmAndAcceptWarningBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ConfirmAndAcceptWarningBtn %>" OnClick="ibtnEConfirmAcceptWarning_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vFinish" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnFReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnFReturn_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vConcurrentUpdate" runat="server">
                    <table style="width: 100%">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnCUReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnCUReturn_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>

            <%-- Pop up for Remove File --%>
            <asp:Button ID="btnHiddenSRemoveFile" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeRemoveFile" runat="server" TargetControlID="btnHiddenSRemoveFile"
                PopupControlID="panSRemoveFile" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panSRemoveFileHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panSRemoveFile" runat="server" Style="display: none">
                <asp:Panel ID="panSRemoveFileHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblSRemoveFileHeader" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>">
                                </asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y"></td>
                        <td style="background-color: #FFFFFF">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgPopupSRemoveFile" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblPopupSRemoveFileText" runat="server" Font-Bold="True" Text="<%$ Resources: Text, ConfirmToRemoveFileQ %>">
                                        </asp:Label>
                                        <asp:HiddenField ID="hfPRAction" runat="server" />
                                    </td>
                                    <td style="width: 40px"></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnPRConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnPRConfirm_Click" />
                                        <asp:ImageButton ID="ibtnPRCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnPRCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- End of Pop up for Remove File --%>

            <%-- Pop up for Export Warning Error Record --%>
            <asp:Button ID="btnER" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeExportReport" runat="server" TargetControlID="btnER"
                PopupControlID="panER" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panERHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panER" runat="server" Style="display: none">
                <asp:Panel ID="panERHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblERTitle" runat="server" Text="<%$ Resources: Text, ExportReport %>"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y"></td>
                        <td style="background-color: #FFFFFF; padding: 5px">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 45px; vertical-align: top">
                                        <asp:Image ID="imgERIcon" runat="server" ImageUrl="~/Images/others/questionMark.png" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblERMessage" runat="server" Font-Bold="True" Text="<%$ Resources: Text, StudentErrorWarningReportPrompt %>"></asp:Label>
                                    </td>
                                    <td style="width: 10px"></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnERDownloadNow" runat="server" ImageUrl="<%$ Resources: ImageUrl, DownloadNowBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, DownloadNowBtn %>" OnClick="ibtnERDownloadNow_Click" />
                                        <asp:ImageButton ID="ibtnERDownloadLater" runat="server" ImageUrl="<%$ Resources: ImageUrl, DownloadLaterBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, DownloadLaterBtn %>" OnClick="ibtnERDownloadLater_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- End of Pop up for Export Warning Error Record --%>

            <%-- Pop up for Show Rectification Record --%>
            <asp:Button ID="btnPopupShowRectRecord" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeShowRectRecord" runat="server" TargetControlID="btnPopupShowRectRecord"
                PopupControlID="panShowRectRecord" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panShowRectRecordHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panShowRectRecord" runat="server" Style="display: none">
                <asp:Panel ID="panShowRectRecordHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 900px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Text, RectificationRecord %>"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 900px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y"></td>
                        <td style="background-color: #FFFFFF">
                            <div style="max-height: 650px; padding: 10px; overflow-y: scroll">
                                <uc1:ucStudentFileDetail ID="udcStudentFileDetailPopup" runat="server"></uc1:ucStudentFileDetail>
                                <div style="text-align: center; padding-top: 20px">
                                    <asp:ImageButton ID="ibtnPSClose" runat="server" ImageUrl="<%$ Resources: ImageUrl, CloseBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, CloseBtn %>" OnClick="ibtnPSClose_Click" />
                                </div>
                            </div>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- End of Pop up for Show Rectification Record --%>

            <%-- Pop up for Change SP --%>
            <asp:Button ID="btnCS" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeChangeSP" runat="server" TargetControlID="btnCS"
                PopupControlID="panCS" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panCSHeader">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panCS" runat="server" Style="display: none">
                <asp:Panel ID="panCSHeader" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblCSTitle" runat="server" Text="<%$ Resources:Text, ChangeServiceProvider %>"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y"></td>
                        <td style="background-color: #FFFFFF; padding: 10px">
                            <asp:Panel ID="panCSContent" runat="server" DefaultButton="ibtnCSConfirm">
                                <table style="width: 100%">
                                    <tr>
                                        <td colspan="2">
                                            <cc2:MessageBox ID="udcMessageBoxCS" runat="server" Width="100%" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 150px">
                                            <asp:Label ID="lblCSServiceProviderIDText" runat="server" Text="<%$ Resources: Text, SPID %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCSServiceProviderID" runat="server" Width="100" MaxLength="8"></asp:TextBox>
                                            <asp:Image ID="imgErrorCSServiceProviderID" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:ImageButton ID="ibtnCSConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnCSConfirm_Click" />
                                            <asp:ImageButton ID="ibtnCSCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnCSCancel_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- End of Pop up for Change SP --%>

            <%-- Pop up for Warning Message --%>
            <asp:Button ID="btnModalPopupWarningMessage" runat="server" style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderWarningMessage" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnModalPopupWarningMessage" PopupControlID="panPopupWarningMessage"
                PopupDragHandleControlID="panWarningMessageHeading" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupWarningMessage" runat="server" Height="450px">
                <asp:Panel Style="cursor: move" ID="panWarningMessageHeading" runat="server">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 680px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px;">
                                <asp:Label ID="lblWarningMessageHeading" runat="server" Text="<%$ Resources:Text, Warning %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table style="width: 680px" cellspacing="0" cellpadding="0" border="0">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff; padding: 5px 5px 5px 5px" align="left">                            
                            <asp:Panel ID="pnlWarningMsgContent" runat="server">
                                <table width="638px">
                                    <tr>
                                        <td>
                                            <cc2:WarningMessageBox ID="udcWarningMessageBox" runat="server" ShowHeader="false" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle"
                                            colspan="2">
                                            <asp:ImageButton ID="ibtnWarningMessageConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnWarningMessageConfirm_Click" />
                                            <asp:ImageButton ID="ibtnWarningMessageCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnWarningMessageCancel_Click">
                                            </asp:ImageButton>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
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
            <%-- End of Pop up for Warning Message --%>

            <%-- Pop up for Show File Record Edit --%>
            <asp:Panel ID="pnlAcctEdit" runat="server" Style="display: none">
                <asp:Panel ID="pnlAcctEditHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="padding-left:2px;font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblAcctEditTitle" runat="server" Text="<%$ Resources:Text, RectifyVRAcctInfo %>" />
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px; position:relative; left: -2px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="background-color: #ffffff">
                            <br />
                            <cc2:MessageBox ID="udcAcctEditErrorMessage" runat="server" Width="800px" />
                            <cc2:InfoMessageBox ID="udcAcctEditInfoMessage" runat="server" Width="800px" />
                            <asp:Panel ID="pnlAcctEditInfo" runat="server">
                            <table cellpadding="0" cellspacing="0" style="width: 800px">                                        
                                <tr>
                                    <td>
                                        <div class="headingText" style="position:relative;left:-5px">
                                            <asp:Label ID="lblRectifyRecipientDetail" runat="server" Text="<%$ Resources:Text, ClassAndStudentInformation %>" />
                                        </div>
                                        <br />
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 200px" valign="top" class="tableCellStyle">
                                                    <asp:Label ID="lblRectifyClassNameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ClassName %>"></asp:Label>
                                                </td>
                                                <td valign="top">
                                                    <asp:Label ID="lblClassName" runat="server" CssClass="tableText" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 200px" valign="top" class="tableCellStyle">
                                                    <asp:Label ID="lblRectifyClassNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ClassNo %>"></asp:Label>
                                                </td>
                                                <td valign="top">
                                                    <%--<asp:Label ID="lblClassNo" runat="server" CssClass="tableText" />--%>
                                                    <asp:textbox ID="txtRectifyClassNo" runat="server" Width="80" MaxLength="10" />
                                                    <asp:Image ID="imgErrRectifyClassNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Top" Style="position:relative;top:-2px;"/>
                                                </td>
                                            </tr>
                                            <tr id="trRectifyChiName" runat="server">
                                                <td style="width: 200px" valign="top" class="tableCellStyle">                                                   
                                                    <asp:Label ID="lblRectifyChiNameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ChineseName %>" />
                                                    <span style="font-size:16px">(</span>
                                                    <asp:Label ID="lblRectifyUploadText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Upload %>" />
                                                    <span style="font-size:16px">)</span>
                                                </td>
                                                <td valign="top">
                                                    <asp:Label ID="lblRectifyChiName" runat="server" CssClass="tableText" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 200px" valign="top" class="tableCellStyle">
                                                    <asp:Label ID="lblRectifyContactNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ContactNo2 %>"></asp:Label>
                                                </td>
                                                <td valign="top">
                                                    <%--<asp:label ID="lblRectifyContactNo" runat="server" CssClass="tableText" Visible="false"/>--%>
                                                    <asp:textbox ID="txtRectifyContactNo" runat="server" Width="160" MaxLength="20" />
                                                    <asp:Image ID="imgErrRectifyContactNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Top" Style="position:relative;top:-2px;"/>
                                                 </td>
                                            </tr>
                                            <tr ID="trConfirmNotToInject" runat="server">
                                                <td style="width: 200px" valign="top" class="tableCellStyle">
                                                    <asp:Label ID="lblRectifyConfirmNotToInjectText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ConfirmToInject %>" />
                                                </td>
                                                <td valign="top">
                                                    <%--<asp:label ID="lblRectifyConfirmNotToInject" runat="server" CssClass="tableText" Visible="false"/>--%>
                                                    <asp:checkbox ID="chkRectifyConfirmNotToInject" runat="server" Style="position:relative;left:-3px;top:2px" />
                                                </td>
                                            </tr>
                                        </table>                                                                                                     
                                        <br />
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr style="vertical-align:top">
                                                <td id="tdAcctInfo" runat="server">
                                                    <div class="headingText" style="position:relative;left:-5px">
                                                        <asp:Label ID="lblRectifyAcct" runat="server" CssClass="eHSTableHeading" />&nbsp;
                                                        <%--<asp:ImageButton ID="ibtnRectifyAcctInputTips" runat="server" ImageAlign="AbsMiddle" OnClick ="ibtnRectifyAcctInputTips_Click" />--%>
                                                        <asp:ImageButton ID="ibtnChangeDocumentType" runat="server" 
                                                            AlternateText="<%$ Resources:AlternateText, ChangeDocumentTypeSBtn %>"
                                                            ImageUrl="<%$ Resources:ImageUrl, ChangeDocumentTypeSBtn %>" ImageAlign="AbsMiddle" 
                                                            OnClick ="ibtnEditChangeDocumentType_Click" />
                                                    </div>
                                                    <br />

                                                    <asp:panel ID="pnlModifyAcct" runat="server" Visible="false">
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <asp:Panel ID="pnlRefNo" runat="server" Visible ="false">
                                                            <tr>
                                                                <td valign="top" class="tableCellStyle" style="width: 200px;height:19px;padding-bottom:5px">
                                                                    <asp:Label ID="lblRectifyRefNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RefNo %>"></asp:Label>
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblRectifyRefNo" runat="server" CssClass="tableText" />
                                                                    <br />
                                                                    <asp:Label ID="lblCreateByOtherSPText" runat="server" CssClass="tableText"/>
                                                                </td>
                                                            </tr>
                                                            </asp:Panel>
                                                            <tr>
                                                                <td valign="top" class="tableCellStyle" style="width:200px;height:19px;padding-bottom:5px">
                                                                    <asp:Label ID="lblRectifyDocTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DocumentType %>"></asp:Label>
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblRectifyDocType" runat="server" CssClass="tableText" />
                                                                </td>
                                                            </tr>
                                                        </table>                                               
                                                        <uc2:ucInputDocumentType ID="udcRectifyAccount" runat="server" Visible="true" />
                                                    </asp:panel>
                                                    <asp:panel ID="pnlReadOnlyAcct" runat="server" Visible="false">
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width:200px;height:19px;padding-bottom:5px" valign="top" class="tableCellStyle">
                                                                    <asp:Label ID="lblRectifyAccountIDText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountID %>"></asp:Label>
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblRectifyAccountID" runat="server" CssClass="tableText" />
                                                                </td>
                                                            </tr>
                                                        </table>   
                                                        <uc3:ucReadOnlyDocumnetType ID="udcReadOnlyAccount" runat="server" Visible="true" />
                                                    </asp:panel>
                                                    <asp:Panel ID="pnlRecordStatus" runat="server" Visible ="false">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 200px" valign="top" class="tableCellStyle">
                                                                <asp:Label ID="lblRecordStatusText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Status %>"></asp:Label>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="lblRecordStatus" runat="server" CssClass="tableText" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </asp:Panel>
                                                </td>
                                                <td id="tdFieldDiff" runat="server">
                                                    <asp:panel ID="pnlDiffUploadInfo" runat="server"  Visible="false">
                                                        <div class="headingText" style="position:relative;left:-5px;padding-left:0px" >
                                                            <asp:Label ID="lblDiffUploadInfo" runat="server" Text="<%$ Resources:Text, DiffUploadInfo %>" 
                                                                />&nbsp;
                                                        </div>
                                                        <br />
                                                        <br />
                                                        <div ID="divFieldDiff1" runat="server" style="position:relative;top:72px">
                                                            <asp:Label ID="lblFieldDiff1" runat="server" Text ="Field1" CssClass="tableText" style="font-size:16px" />
                                                        </div>
                                                        <div ID="divFieldDiff2" runat="server" style="position:relative;top:80px">
                                                            <asp:Label ID="lblFieldDiff2" runat="server" Text ="Field2" CssClass="tableText" style="font-size:16px" />
                                                        </div>
                                                        <div ID="divFieldDiff3" runat="server" style="position:relative;top:88px">
                                                            <asp:Label ID="lblFieldDiff3" runat="server" Text ="Field3" CssClass="tableText" style="font-size:16px" />
                                                        </div>
                                                        <div ID="divFieldDiff4" runat="server" style="position:relative;top:96px">
                                                            <asp:Label ID="lblFieldDiff4" runat="server" Text ="Field4" CssClass="tableText" style="font-size:16px" />
                                                        </div>
                                                        <div ID="divFieldDiff5" runat="server" style="position:relative;top:104px">
                                                            <asp:Label ID="lblFieldDiff5" runat="server" Text ="Field5" CssClass="tableText" style="font-size:16px" />
                                                        </div>
                                                        <div ID="divFieldDiff6" runat="server" style="position:relative;top:112px">
                                                            <asp:Label ID="lblFieldDiff6" runat="server" Text ="Field6" CssClass="tableText" style="font-size:16px" />
                                                        </div>
                                                    </asp:panel>
                                                </td>
                                            </tr>
                                        </table>
                                        <asp:Panel ID="pnlMMRClientInfo" runat="server" Visible="false">
                                        <hr />
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr id="trRectifyHKICSymbol" runat="server">
                                                <td style="width: 200px" valign="top" class="tableCellStyle">
                                                    <asp:Label ID="lblRectifyHKICSymbolText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, HKICSymbolLong %>" />
                                                </td>
                                                <td valign="top">
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Radiobuttonlist ID="rblRectifyHKICSymbol" runat="server" AutoPostBack="False" Enabled="true"
                                                                        CssClass="tableText" RepeatDirection="Horizontal" Style="position:relative;left:-7px;top:-2px;display:inline-block">
                                                                    <asp:ListItem Text="A" Value="A" />
                                                                    <asp:ListItem Text="C" Value="C" />
                                                                    <asp:ListItem Text="R" Value="R" />
                                                                    <asp:ListItem Text="U" Value="U" />
                                                                    <asp:ListItem Text="Others" Value="O" />
                                                                </asp:Radiobuttonlist>
                                                            </td >
                                                            <td width="30px">
                                                                <asp:Image ID="imgErrRectifyHKICSymbol" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Top" Style="position:relative;top:-2px;"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    <asp:Label ID="lblRectifyHKICSymbol" runat="server" class="tableText" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 200px" valign="top" class="tableCellStyle">
                                                    <asp:Label ID="lblRectifyServiceDateText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ServiceDate %>" />
                                                </td>
                                                <td valign="top">
                                                    <asp:TextBox ID="txtRectifyServiceDate" runat="server" Width="80" MaxLength="10" />
                                                    <asp:Label ID="lblRectifyServiceDate" runat="server" class="tableText" />
                                                    <asp:ImageButton ID="ibtnRectifyServiceDateCalender" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                                    <asp:Image ID="imgErrRectifyServiceDate" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="Top" Style="position:relative;top:-2px;"/>
                                                    <cc1:CalendarExtender ID="calRectifyServiceDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnRectifyServiceDateCalender"
                                                        TargetControlID="txtRectifyServiceDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                    </cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="fteRectifyServiceDate" runat="server" FilterType="Custom, Numbers"
                                                        TargetControlID="txtRectifyServiceDate" ValidChars="-">
                                                    </cc1:FilteredTextBoxExtender>
                                                </td>
                                            </tr>
                                        </table>
                                        <br />
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr id="trConfirmEHSAccount" runat="server">
                                    <td >
                                        <br />
                                        <div class="checkboxStyle" style="position:relative;height:42px">
                                            <asp:CheckBox ID="chkConfirmEHSAccount" runat="server" AutoPostBack="false" />
                                            <asp:Label ID="lblConfirmStatement1" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, FieldDiffConfirmStatement1 %>" />
                                            <br />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblConfirmStatement2" runat="server" CssClass="tableTitle" 
                                                Text="<%$ Resources:Text, FieldDiffConfirmStatement2 %>" Style="position:relative;top:3px" />
                                            <br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center;vertical-align:top;padding-top:15px;padding-bottom:10px">
                                        <asp:ImageButton ID="ibtnEditAcctCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnEditAcctCancel_Click" />
                                        &nbsp;&nbsp;&nbsp;            
                                        <asp:ImageButton ID="ibtnEditAcctSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnEditAcctSave_Click" />
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlConcurrentUpdate" runat="server">
                            <table cellpadding="0" cellspacing="0" style="width: 440px">       
                                <tr>
                                    <td align="right" style="width: 60px; height: 42px" valign="middle">
                                        <asp:Image ID="imgEditAcctMsg" runat="server" ImageUrl="~/Images/others/Information.png" />
                                    </td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblEditAcctMsg" runat="server" Font-Bold="True" />
                                    </td>
                                </tr>                                                                 
                                <tr>
                                    <td style="text-align:center;vertical-align:top;padding-top:0px;padding-bottom:10px" colspan="2">  
                                        <br />
                                        <asp:ImageButton ID="ibtnEditAcctOK" runat="server" AlternateText="<%$ Resources:AlternateText, OKBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, OKBtn %>" OnClick="ibtnEditAcctOK_Click" />
                                    </td>
                                </tr>
                            </table>
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button Style="display: none" ID="btnHiddenAcctEdit" runat="server" />
            <cc1:ModalPopupExtender ID="mpeAcctEdit" runat="server" PopupControlID="pnlAcctEdit" PopupDragHandleControlID="pnlAcctEditHeading"
                    TargetControlID="btnHiddenAcctEdit" BackgroundCssClass="modalBackgroundTransparent" />
            <%-- End of Pop up for Show File Record Edit --%>

            <%-- Pop up for Show CCCode --%>
            <asp:Panel Style="display: none" ID="panChooseCCCode" runat="server">
                <asp:Panel ID="panChooseCCCodeHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblCCCodeHeading" runat="server" Text="<%$ Resources:Text, ChooseCCCodeHeading %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px; position:relative; left: -2px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="center">
                            <uc4:ChooseCCCode ID="udcCCCode" runat="server"></uc4:ChooseCCCode>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                            height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button Style="display: none" ID="btnHiddenCCCode" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderChooseCCCode" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenCCCode" PopupControlID="panChooseCCCode" RepositionMode="None"
                PopupDragHandleControlID="panChooseCCCodeHeading" />
            <%-- End of Pop up for Show CCCode --%>

            <%-- Pop up for Show Document Type Selection --%>
            <asp:Panel ID="pnlDocTypeSelection" runat="server" Style="display: none">
                <asp:Panel ID="pnlDocTypeSelectionHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDocumentType" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px; position:relative; left: -2px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="center">
                            <br />
                            <cc2:MessageBox ID="udcDocTypeSelectionErrorMessage" runat="server" Width="800px" />
                            <cc2:InfoMessageBox ID="udcDocTypeSelectionInfoMessage" runat="server" Width="800px" />
                            <table cellpadding="0" cellspacing="0" style="width: 800px">                                        
                                <tr>
                                    <td>
                                        <cc2:DocumentTypeRadioButtonGroup ID="udcDocumentTypeRadioButtonGroup" runat="server"
                                            HeaderCss="eHSTableHeading" AutoPostBack="true" HeaderText="<%$ Resources:Text, DocumentType%>"
                                            LegendImageURL="<%$ Resources:ImageUrl, Infobtn%>" LegendImageALT="<%$ Resources:Text, AcceptedDocList%>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center;vertical-align:top;padding-top:15px;padding-bottom:10px">
                                        <asp:ImageButton ID="ibtnDocTypeSelectionCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnDocTypeSelectionCancel_Click" />
                                        &nbsp;&nbsp;&nbsp;            
                                        <asp:ImageButton ID="ibtnDocTypeSelectionNext" runat="server" AlternateText="<%$ Resources:AlternateText, NextBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" OnClick="ibtnDocTypeSelectionNext_Click" />
                                    </td>
                                </tr>
                            </table>                          
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                            height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button Style="display: none" ID="btnHiddenDocTypeSelection" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="mpeDocTypeSelection" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenDocTypeSelection" PopupControlID="pnlDocTypeSelection" RepositionMode="None"
                PopupDragHandleControlID="pnlDocTypeSelectionHeading" />
            <%-- End of Pop up for Show Document Type Selection --%>

            <%-- Pop up for Show Scheme Document Type Legend --%>
            <asp:Panel Style="display: none" ID="panSchemeDocTypeLegend" runat="server">
                <asp:Panel ID="panSchemeDocTypeLegendHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 920px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblSchemeDocTypeLegnedHeading" runat="server" Text="<%$ Resources:Text, AcceptedDocList %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 920px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panSchemeDocTypeLegnedContent" runat="server" ScrollBars="vertical"
                                Height="550px">
                                <uc5:SchemeDocTypeLegend ID="udcSchemeDocTypeLegend" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="btnSchemeDocTypeLegnedClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" /></td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                            height: 7px">
                        </td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button Style="display: none" ID="btnHiddenSchemeDocTypeLegend" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="mpeSchemeDocTypeLegend" runat="server" BackgroundCssClass="modalBackgroundTransparent" 
                TargetControlID="btnHiddenSchemeDocTypeLegend" PopupControlID="panSchemeDocTypeLegend" RepositionMode="None"
                PopupDragHandleControlID="panSchemeDocTypeLegendHeading" />
            <%-- End of Pop up for Show Scheme Document Type Legend --%>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
