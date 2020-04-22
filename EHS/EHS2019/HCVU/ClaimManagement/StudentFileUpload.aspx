<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="StudentFileUpload.aspx.vb" Inherits="HCVU.StudentFileUpload"
    Title="<%$ Resources: Title, StudentFileUpload %>" EnableEventValidation="False" %>

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

        table.tblDV > tbody > tr {
            height: 24px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../JS/Common.js"></script>
    <script type="text/javascript">
        function SaveFilePathToHiddenField() {
            document.getElementById('<%= hfIFile.ClientID %>').value = document.getElementById('<%= flI2StudentFile.ClientID %>').value;
        }
    </script>
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </cc1:ToolkitScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, StudentFileUploadBanner %>"
        AlternateText="<%$ Resources: AlternateText, StudentFileUploadBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="ibtnI1Next" />
            <asp:PostBackTrigger ControlID="ibtnI2Next" />
            <asp:PostBackTrigger ControlID="ibtnCBack" />
        </Triggers>
        <ContentTemplate>
            <div style="height: 4px"></div>
            <asp:HiddenField ID="hfIFile" runat="server" />
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="950px" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="950px" />
            <asp:MultiView ID="mvCore" runat="server">
                <asp:View ID="vGrid" runat="server">
                    <asp:GridView ID="gvStudentFile" runat="server" CssClass="gvTable" AutoGenerateColumns="False" AllowPaging="True"
                        AllowSorting="True" OnRowDataBound="gvStudentFile_RowDataBound" OnPreRender="gvStudentFile_PreRender"
                        OnRowCommand="gvStudentFile_RowCommand" OnSorting="gvStudentFile_Sorting"
                        OnPageIndexChanging="gvStudentFile_PageIndexChanging">
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
                    <table style="width: 100%">
                        <tr style="height: 20px"></tr>
                        <tr>
                            <td style="text-align: center">
                                <asp:ImageButton ID="ibtnGUploadFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, UploadFileBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, UploadFileBtn %>" OnClick="ibtnGUploadFile_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vDetail" runat="server">
                    <uc1:ucStudentFileDetail ID="udcStudentFileDetail" runat="server"></uc1:ucStudentFileDetail>
                    <table>
                        <tr style="height: 20px"></tr>
                        <tr>
                            <td style="width: 300px">
                                <asp:ImageButton ID="ibtnDBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnDRemoveFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, RemoveStudentFileBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, RemoveStudentFileBtn %>" OnClick="ibtnDRemoveFile_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vImport1" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblI1UploadStudentFile" runat="server" Text="<%$ Resources: Text, UploadStudentFile %>"></asp:Label>
                    </div>
                    <div style="height: 6px"></div>
                    <asp:Panel ID="panI1" runat="server" DefaultButton="ibtnI1Next">
                        <table class="tblSF">
                            <tr>
                                <td style="width: 180px">
                                    <asp:Label ID="lblI1SchoolCodeText" runat="server" Text="<%$ Resources: Text, SchoolCode %>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtI1SchoolCode" runat="server" Width="100px" MaxLength="10">
                                    </asp:TextBox>
                                    <asp:Image ID="imgI1SchoolCodeError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label ID="lblI1ServiceProviderIDText" runat="server" Text="<%$ Resources: Text, SPID %>">
                                    </asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtI1ServiceProviderID" runat="server" Width="100px" MaxLength="8">
                                    </asp:TextBox>
                                    <asp:Image ID="imgI1ServiceProviderIDError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" ImageAlign="Top" />
                                </td>
                            </tr>
                            <tr style="height: 60px">
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <table>
                                        <tr>
                                            <td style="width: 300px">
                                                <asp:ImageButton ID="ibtnI1Cancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnI1Cancel_Click" />
                                            </td>
                                            <td style="text-align: center">
                                                <asp:ImageButton ID="ibtnI1Next" runat="server" ImageUrl="<%$ Resources: ImageUrl, NextBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, NextBtn %>" OnClick="ibtnI1Next_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="vImport2" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblI2UploadStudentFile" runat="server" Text="<%$ Resources: Text, UploadStudentFile %>"></asp:Label>
                    </div>
                    <div style="height: 6px"></div>
                    <table class="tblSF">
                        <tr>
                            <td style="width: 260px">
                                <asp:Label ID="lblI2SchoolCodeText" runat="server" Text="<%$ Resources: Text, SchoolCode %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblI2SchoolCode" runat="server" class="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblI2SchoolNameText" runat="server" Text="<%$ Resources: Text, SchoolName %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblI2SchoolName" runat="server" class="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblI2ServiceProviderIDText" runat="server" Text="<%$ Resources: Text, SPID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblI2ServiceProviderID" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblI2ServiceProviderNameText" runat="server" Text="<%$ Resources: Text, ServiceProviderName %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblI2ServiceProviderName" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblI2PracticeText" runat="server" Text="<%$ Resources: Text, Practice %>"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlI2Practice" runat="server" Width="400"></asp:DropDownList>
                                <asp:Image ID="imgErrorI2Practice" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblI2VaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtI2VaccinationDate" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                <asp:ImageButton ID="ibtnI2VaccinationDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                <asp:Image ID="imgErrorI2VaccinationDate" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                <cc1:CalendarExtender ID="calI2VaccinationDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnI2VaccinationDate"
                                    TargetControlID="txtI2VaccinationDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                </cc1:CalendarExtender>
                                <cc1:FilteredTextBoxExtender ID="fteI2VaccinationDate" runat="server" FilterType="Custom, Numbers"
                                    TargetControlID="txtI2VaccinationDate" ValidChars="-">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblI2VaccinationReportGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>">
                                </asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtI2VaccinationReportGenerateDate" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                <asp:ImageButton ID="ibtnI2VaccinationReportGenerateDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                <asp:Image ID="imgErrorI2VaccinationReportGenerationDate" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                <cc1:CalendarExtender ID="calI2VaccinationReportGenerateDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnI2VaccinationReportGenerateDate"
                                    TargetControlID="txtI2VaccinationReportGenerateDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                </cc1:CalendarExtender>
                                <cc1:FilteredTextBoxExtender ID="fteI2VaccinationReportGenerateDate" runat="server" FilterType="Custom, Numbers"
                                    TargetControlID="txtI2VaccinationReportGenerateDate" ValidChars="-">
                                </cc1:FilteredTextBoxExtender>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblI2DoseToInjectText" runat="server" Text="<%$ Resources: Text, DoseToInject %>"></asp:Label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:RadioButtonList ID="rbtnI2DoseToInject" runat="server" RepeatDirection="Horizontal"
                                                CellPadding="0" CellSpacing="0" CssClass="tblI2DoseToInject tableText">
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:Image ID="imgErrorI2DoseToInject" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblI2StudentFileText" runat="server" Text="<%$ Resources: Text, StudentFile %>"></asp:Label>
                            </td>
                            <td>
                                <asp:FileUpload ID="flI2StudentFile" runat="server" Width="460px" />
                                <asp:Image ID="imgErrorI2StudentFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblI2NoOfStudentText" runat="server" Text="<%$ Resources: Text, NoOfStudent %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtI2NoOfStudent" runat="server" Width="100" MaxLength="10"></asp:TextBox>
                                <asp:Image ID="imgErrorI2NoOfStudent" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                            </td>
                        </tr>
                        <tr style="height: 30px">
                        </tr>
                        <tr>
                            <td colspan="2">
                                <table>
                                    <tr>
                                        <td style="width: 300px">
                                            <asp:ImageButton ID="ibtnI2Back" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnI2Back_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnI2Next" runat="server" ImageUrl="<%$ Resources: ImageUrl, NextBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, NextBtn %>" OnClick="ibtnI2Next_Click"
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
                        <asp:Label ID="lblCUploadStudentFile" runat="server" Text="<%$ Resources: Text, UploadStudentFile %>"></asp:Label>
                    </div>
                    <div style="height: 6px"></div>
                    <table class="tblSF">
                        <tr>
                            <td style="width: 260px">
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
                        <tr>
                            <td>
                                <asp:Label ID="lblCVaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCVaccinationDate" runat="server" CssClass="tableText"></asp:Label>
                                <asp:HiddenField ID="hfCVaccinationDate" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblCVaccinationReportGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCVaccinationReportGenerationDate" runat="server" CssClass="tableText"></asp:Label>
                                <asp:HiddenField ID="hfCVaccinationReportGenerationDate" runat="server" />
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
                                <asp:Label ID="lblCStudentFileText" runat="server" Text="<%$ Resources: Text, StudentFile %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblCStudentFile" runat="server" CssClass="tableText"></asp:Label>
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
                        <tr style="height: 30px">
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
                    <div class="headingText">
                        <asp:Label ID="lblEUploadStudentFile" runat="server" Text="<%$ Resources: Text, UploadStudentFile %>"></asp:Label>
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
                                <asp:Label ID="lblEOverLimit" runat="server" ForeColor="Red" Text="<%$ Resources: Text, StudentFileErrorOverLimit %>"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gvE" runat="server" CssClass="gvTable" Width="100%" AutoGenerateColumns="False" AllowPaging="True"
                        AllowSorting="False" OnRowDataBound="gvE_RowDataBound" OnPreRender="gvE_PreRender" OnPageIndexChanging="gvE_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ClassName %>" ItemStyle-Width="50">
                                <ItemTemplate>
                                    <asp:Label ID="lblGClassName" runat="server" Text='<%# Eval("Class_Name") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ClassNo %>" ItemStyle-Width="50">
                                <ItemTemplate>
                                    <asp:Label ID="lblGClassNo" runat="server" Text='<%# Eval("Class_No") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ContactNo2 %>" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblGContactNo" runat="server" Text='<%# Eval("Contact_No") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ChineseName %>" ItemStyle-Width="70">
                                <ItemTemplate>
                                    <asp:Label ID="lblGChineseName" runat="server" Text='<%# Eval("Name_CH") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, EnglishSurname %>" ItemStyle-Width="70">
                                <ItemTemplate>
                                    <asp:Label ID="lblGSurnameEN" runat="server" Text='<%# Eval("Surname_EN") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, EnglishGivenName %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblGGivenNameEN" runat="server" Text='<%# Eval("Given_Name_EN") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Sex %>" ItemStyle-Width="50">
                                <ItemTemplate>
                                    <asp:Label ID="lblGSex" runat="server" Text='<%# Eval("Sex") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, DOB %>" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblGDOB" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, DocumentType %>" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblGDocType" runat="server" Text='<%# Eval("Doc_Code") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, DocumentNo %>" ItemStyle-Width="90">
                                <ItemTemplate>
                                    <asp:Label ID="lblGDocNo" runat="server" Text='<%# Eval("Doc_No") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ErrorMessage %>" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblGErrorMessage" runat="server" Text='<%# Eval("Upload_Error") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, WarningMessage %>" ItemStyle-Width="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblGWarningMessage" runat="server" Text='<%# Eval("Upload_Warning") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <table>
                        <tr style="height: 10px"></tr>
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
                    <asp:ImageButton ID="ibtnFReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                        AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnFReturn_Click" />
                </asp:View>
                <asp:View ID="vConcurrentUpdate" runat="server">
                    <asp:ImageButton ID="ibtnCUReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                        AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnCUReturn_Click" />
                </asp:View>
            </asp:MultiView>
            <%-- Pop up for Remove File --%>
            <asp:Button ID="btnRF" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeRemoveFile" runat="server" TargetControlID="btnRF"
                PopupControlID="panRF" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panRFHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panRF" runat="server" Style="display: none">
                <asp:Panel ID="panRFHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblRFHeader" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>">
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
                                        <asp:Image ID="imgRFIcon" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblRFText" runat="server" Font-Bold="True" Text="<%$ Resources: Text, ConfirmToRemoveFileQ %>">
                                        </asp:Label></td>
                                    <td style="width: 40px"></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnRFConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnRFConfirm_Click" />
                                        <asp:ImageButton ID="ibtnRFCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnRFCancel_Click" /></td>
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
                                <tr style="height: 10px"></tr>
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
            <%-- Pop up for Duplicate Vaccination Date --%>
            <asp:Button ID="btnDV" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeDV" runat="server" TargetControlID="btnDV"
                PopupControlID="panDV" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panDVHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panDV" runat="server" Style="display: none">
                <asp:Panel ID="panDVHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 700px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDVTitle" runat="server" Text="<%$ Resources: Text, Confirmation %>"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 700px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y"></td>
                        <td style="background-color: #FFFFFF; padding: 5px">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 45px; vertical-align: top">
                                        <asp:Image ID="imgDVIcon" runat="server" ImageUrl="~/Images/others/information.png" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblDVMessage1" runat="server" Font-Bold="True" Text="<%$ Resources: Text, DuplicateSchoolVaccinationMessage1 %>"></asp:Label>
                                        <table class="tblDV">
                                            <tr style="height: 20px"></tr>
                                            <tr>
                                                <td style="width: 250px">
                                                    <asp:Label ID="lblDVStudentFileIDText" runat="server" Text="<%$ Resources: Text, StudentFileID %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVStudentFileID" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVServiceProviderIDText" runat="server" Text="<%$ Resources: Text, SPID %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVServiceProviderID" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVServiceProviderNameText" runat="server" Text="<%$ Resources: Text, ServiceProviderName %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVServiceProviderName" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVPracticeText" runat="server" Text="<%$ Resources: Text, Practice %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVPractice" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVVaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVVaccinationDate" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVVaccinationReportGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVVaccinationReportGenerationDate" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVDoseToInjectText" runat="server" Text="<%$ Resources: Text, DoseToInject %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVDoseToInject" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVUploadedByText" runat="server" Text="<%$ Resources: Text, UploadedBy %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVUploadedBy" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblDVStatusText" runat="server" Text="<%$ Resources: Text, Status %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblDVStatus" runat="server" class="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="height: 20px"></tr>
                                        </table>
                                        <asp:Label ID="lblDVMessage2" runat="server" Font-Bold="True" Text="<%$ Resources: Text, DuplicateSchoolVaccinationMessage2 %>"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="height: 30px"></tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnDVConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnDVConfirm_Click" />
                                        <asp:ImageButton ID="ibtnDVCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnDVCancel_Click" />
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
            <%-- End of Pop up for Duplicate Vaccination Date --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
