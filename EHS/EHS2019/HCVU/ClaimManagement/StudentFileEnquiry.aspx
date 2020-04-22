<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="StudentFileEnquiry.aspx.vb" Inherits="HCVU.StudentFileEnquiry"
    Title="<%$ Resources: Title, StudentFileEnquiry %>" EnableEventValidation="False" %>

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
                height: 35px;
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
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </cc1:ToolkitScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, StudentFileEnquiryBanner %>"
        AlternateText="<%$ Resources: AlternateText, StudentFileEnquiryBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 4px"></div>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="950px" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="950px" />
            <asp:MultiView ID="mvCore" runat="server" ActiveViewIndex="-1">
                <asp:View ID="vSearch" runat="server">
                    <table style="width: 100%" class="tblS">
                        <tr>
                            <td style="width: 160px">
                                <asp:Label ID="lblSStudentFileIDText" runat="server" Text="<%$ Resources: Text, StudentFileID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSStudentFileID" runat="server" Width="200" MaxLength="15"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSSchoolCodeText" runat="server" Text="<%$ Resources: Text, SchoolCode %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSSchoolCode" runat="server" Width="200" MaxLength="10"></asp:TextBox>
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
                        <tr style="height: 30px"></tr>
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
                            <asp:TemplateField HeaderText="<%$ Resources: Text, StudentFileID %>" SortExpression="Student_File_ID" ItemStyle-Width="130">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnGStudentFileID" runat="server" Text='<%# Eval("Student_File_ID") %>'></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, SchoolCode %>" SortExpression="School_Code" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <asp:Label ID="lblGSchoolCode" runat="server" Text='<%# Eval("School_Code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, SPID %>" SortExpression="SP_ID" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblGSPID" runat="server" Text='<%# Eval("SP_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationDate %>" SortExpression="Service_Receive_Dtm" ItemStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="lblGVaccinationDate" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationReportGenerationDate %>" SortExpression="Final_Checking_Report_Generation_Date"
                                ItemStyle-Width="120">
                                <ItemTemplate>
                                    <asp:Label ID="lblGVaccinationReportGenerationDate" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, DoseToInject %>" SortExpression="Dose" ItemStyle-Width="100">
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
                        <tr style="height: 20px"></tr>
                        <tr>
                            <td style="width: 300px">
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
                        <asp:ImageButton ID="ibtnDShowVaccClaimRecord" runat="server" ImageUrl="<%$ Resources: ImageUrl, ShowVaccinationClaimRecordBtn %>"
                            AlternateText="<%$ Resources: AlternateText, ShowVaccinationClaimRecordBtn %>" OnClick="ibtnDShowVaccClaimRecord_Click" />
                    </div>
                    <uc1:ucStudentFileDetail ID="udcStudentFileDetail" runat="server"></uc1:ucStudentFileDetail>
                    <br />
                    <table>
                        <tr style="height: 20px"></tr>
                        <tr>
                            <td style="width: 300px">
                                <asp:ImageButton ID="ibtnDBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnDExportLatestStudentReport" runat="server" ImageUrl="<%$ Resources: ImageUrl, ExportLatestStudentReportBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ExportLatestStudentReportBtn %>" OnClick="ibtnDExportLatestStudentReport_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
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
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
