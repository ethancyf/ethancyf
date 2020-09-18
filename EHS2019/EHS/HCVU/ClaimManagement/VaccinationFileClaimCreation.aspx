<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="VaccinationFileClaimCreation.aspx.vb" Inherits="HCVU.VaccinationFileClaimCreation"
    Title="<%$ Resources: Title, VaccinationFileClaimCreation %>" EnableEventValidation="False" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="../UIControl/StudentFile/ucStudentFileDetail.ascx" TagName="ucStudentFileDetail" TagPrefix="uc1" %>

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
        function SaveFilePathToHiddenField() {
            document.getElementById('<%= hfIFile.ClientID %>').value = document.getElementById('<%= flIStudentFile.ClientID %>').value;
        }
    </script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, VaccinationFileClaimCreationBanner %>"
        AlternateText="<%$ Resources: AlternateText, VaccinationFileClaimCreationBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="ibtnDUploadVaccinationClaim" />
            <asp:PostBackTrigger ControlID="ibtnINext" />
            <asp:PostBackTrigger ControlID="ibtnCBack" />
        </Triggers>
        <ContentTemplate>
            <div style="height: 4px"></div>
            <asp:HiddenField ID="hfIFile" runat="server" />
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" style="width:950px;display:block" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" style="width:950px;display:block"  />
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
                            <asp:TemplateField HeaderText="<%$ Resources: Text, SchoolCode %>" SortExpression="School_Code" ItemStyle-Width="100">
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
                        <asp:ImageButton ID="ibtnDShowVaccClaimRecord" runat="server" ImageUrl="<%$ Resources: ImageUrl, ShowVaccinationClaimRecordBtn %>"
                            AlternateText="<%$ Resources: AlternateText, ShowVaccinationClaimRecordBtn %>" OnClick="ibtnDShowVaccClaimRecord_Click" />
                    </div>
                    <uc1:ucStudentFileDetail ID="udcStudentFileDetail" runat="server"></uc1:ucStudentFileDetail>
                    <table>
                        <tr>
                            <td style="width: 260px">
                                <asp:ImageButton ID="ibtnDBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnDUploadVaccinationClaim" runat="server" ImageUrl="<%$ Resources: ImageUrl, UploadVaccinationClaimBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, UploadVaccinationClaimBtn %>" OnClick="ibtnDUploadVaccinationClaim_Click" />
                                <asp:ImageButton ID="ibtnDRemoveVaccinationClaim" runat="server" ImageUrl="<%$ Resources: ImageUrl, RemoveVaccinationClaimBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, RemoveVaccinationClaimBtn %>" OnClick="ibtnDRemoveVaccinationClaim_Click" />
                                <asp:ImageButton ID="ibtnDRemoveVaccinationFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, RemoveVaccinationFileBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, RemoveVaccinationFileBtn %>" OnClick="ibtnDRemoveVaccinationFile_Click" />
                                <asp:ImageButton ID="ibtnDReactivateTransaction" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReactivateTransactionBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReactivateTransactionBtn %>" OnClick="ibtnDReactivateTransaction_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vImport" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblIUploadStudentFile" runat="server" Text="<%$ Resources: Text, UploadVaccinationFile %>"></asp:Label>
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
                                <asp:Label ID="lblISchemeText" runat="server" Text="<%$ Resources: Text, Scheme %>" />
                            </td>
                            <td>
                                <asp:Label ID="lblIScheme" runat="server" class="tableText" />
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
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
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
                                                <asp:Label ID="lblIVaccinationReportGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>">
                                                </asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIVaccinationReportGenerationDate1" runat="server" CssClass="tableText"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIVaccinationReportGenerationDate2" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:TextBox ID="txtIVaccinationReportGenerationDate2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnIVaccinationReportGenerationDate2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                                <asp:Image ID="imgErrorIVaccinationReportGenerationDate2" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                                <cc1:CalendarExtender ID="calIVaccinationReportGenerationDate2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationReportGenerationDate2"
                                                    TargetControlID="txtIVaccinationReportGenerationDate2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                                </cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteIVaccinationReportGenerationDate2" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationReportGenerationDate2" ValidChars="-">
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
                                                <asp:Label ID="lblIVaccinationReportGenerationDate1_2" runat="server" CssClass="tableText" />
<%--                                                <asp:TextBox ID="txtIVaccinationReportGenerationDate1_2" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                                <asp:ImageButton ID="ibtnIVaccinationReportGenerationDate1_2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                                <asp:Image ID="imgErrorIVaccinationReportGenerationDate1_2" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" style="position:relative;top:4px" />
                                                <cc1:CalendarExtender ID="calIVaccinationReportGenerateDate1_2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationReportGenerationDate1_2"
                                                    TargetControlID="txtIVaccinationReportGenerationDate1_2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteVaccinationReportGenerationDate1_2" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationReportGenerationDate1_2" ValidChars="-"></cc1:FilteredTextBoxExtender>--%>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblIVaccinationReportGenerationDate2_2" runat="server" CssClass="tableText" />
                                                <asp:TextBox ID="txtIVaccinationReportGenerationDate2_2" runat="server" Width="100" MaxLength="10" style="position:relative;top:-1px" />
                                                <asp:ImageButton ID="ibtnIVaccinationReportGenerationDate2_2" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" style="position:relative;top:2px" />
                                                <asp:Image ID="imgErrorIVaccinationReportGenerationDate2_2" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" style="position:relative;top:3px" />
                                                <cc1:CalendarExtender ID="calIVaccinationReportGenerationDate2_2" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnIVaccinationReportGenerationDate2_2"
                                                    TargetControlID="txtIVaccinationReportGenerationDate2_2" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy"></cc1:CalendarExtender>
                                                <cc1:FilteredTextBoxExtender ID="fteIVaccinationReportGenerationDate2_2" runat="server" FilterType="Custom, Numbers"
                                                    TargetControlID="txtIVaccinationReportGenerationDate2_2" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="panIMMR" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lblIGenerationDateMMR" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblIVaccinationReportGenerateDateMMR" runat="server" CssClass="tableText" />
                                </td>
                            </tr>
                        </asp:Panel>
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
                                <asp:Label ID="lblIStudentFileText" runat="server" Text="<%$ Resources: Text, VaccinationFile %>"></asp:Label>
                            </td>
                            <td>
                                <asp:FileUpload ID="flIStudentFile" runat="server" Width="460px" style="position:relative;top:-1px" />
                                <asp:Image ID="imgErrorIStudentFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" style="position:relative;top:3px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblIVaccineFilePwdText" runat="server" Text="<%$ Resources: Text, VaccinationFilePassword %>"/>
                            </td>
                            <td>
                                <asp:TextBox ID="txtIStudentFilePassword" runat="server" Width="150px" TextMode="Password" style="position:relative;top:-1px" />
                                <asp:Image ID="imgErrorIStudentFilePassword" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" style="position:relative;top:3px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblINoOfClassText" runat="server" Text="<%$ Resources: Text, NoOfClass %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblINoOfClass" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
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
                                        <td style="width: 300px">
                                            <asp:ImageButton ID="ibtnICancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnICancel_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnINext" runat="server" ImageUrl="<%$ Resources: ImageUrl, NextBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, NextBtn %>" OnClick="ibtnINext_Click"
                                                OnClientClick="SaveFilePathToHiddenField()" />
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
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationDate2" runat="server" CssClass="tableText"></asp:Label>                                                
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
                                            </td>
                                            <td>
                                                <asp:Label ID="lblCVaccinationDate2_2" runat="server" CssClass="tableText"></asp:Label>                                                
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
                                        <asp:HiddenField ID="hfCVaccinationDate2" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationReportGenerationDate2" runat="server" />

                                        <asp:HiddenField ID="hfCVaccinationDate1_2" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationDate2_2" runat="server" />
                                        <asp:HiddenField ID="hfCVaccinationReportGenerationDate2_2" runat="server" />
                                    </table>                                    
                                </td>
                            </tr>
                        </asp:Panel>
                        <asp:Panel ID="panCMMR" runat="server">
                            <tr>
                                <td>
                                    <asp:Label ID="lblCVaccinationReportGenerateDateMMRText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblCVaccinationReportGenerateDateMMR" runat="server" class="tableText"/>
                                </td>
                            </tr>
                        </asp:Panel>
                        <tr>
                            <td>
                                <asp:Label ID="lblCSubsidyText" runat="server" Text="<%$ Resources: Text, Subsidy %>"></asp:Label>
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
                                <asp:HiddenField ID="hfCDoseToInject" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCStudentFileText" runat="server" Text="<%$ Resources: Text, VaccinationFile %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCStudentFile" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCNoOfClassText" runat="server" Text="<%$ Resources: Text, NoOfClass %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCNoOfClass" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
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
                                        <td style="width: 300px">
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
                        <asp:GridView ID="gvE" runat="server" CssClass="gvTable" Width="600px" AutoGenerateColumns="False" AllowPaging="True"
                            AllowSorting="False" OnRowDataBound="gvE_RowDataBound" OnPreRender="gvE_PreRender" OnPageIndexChanging="gvE_PageIndexChanging">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, SeqNo %>" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGStudentSeq" runat="server" Text='<%# Eval("Student_Seq")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, Injected %>" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGInjected" runat="server" Text='<%# Eval("Injected")%>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, ErrorMessage %>" ItemStyle-Width="200">
                                    <ItemTemplate>
                                        <asp:Label ID="lblGErrorMessage" runat="server" Text='<%# Eval("Upload_Error") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources: Text, WarningMessage %>" ItemStyle-Width="200">
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
            <%-- Pop up for Reactivate Transaction Popup --%>
            <asp:Button ID="btnRT" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeReactivateTransPopup" runat="server" TargetControlID="btnRT"
                PopupControlID="panRT" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panRTHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panRT" runat="server" Style="display: none">
                <asp:Panel ID="panRTHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblRTTitle" runat="server" Text="<%$ Resources: Text, ConfirmBoxTitle %>">
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
                                        <asp:Image ID="imgRTIcon" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblRTText" runat="server" Font-Bold="True" Text="<%$ Resources: Text, ConfirmQ %>">
                                        </asp:Label></td>
                                    <td style="width: 40px"></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnRTConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnRTConfirm_Click" />
                                        <asp:ImageButton ID="ibtnRTCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnRTCancel_Click" /></td>
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
            <%-- Pop up for Reactivate Transaction Popup --%>

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
            <%-- Pop up for Show Vaccination Claim --%>
            <asp:Button ID="btnPopupShowRectRecord" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeShowRectRecord" runat="server" TargetControlID="btnPopupShowRectRecord"
                PopupControlID="panShowRectRecord" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panShowRectRecordHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panShowRectRecord" runat="server" Style="display: none">
                <asp:Panel ID="panShowRectRecordHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 950px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Text, VaccinationClaimRecord %>"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 950px">
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
            <%-- End of Pop up for Show Vaccination Claim --%>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
