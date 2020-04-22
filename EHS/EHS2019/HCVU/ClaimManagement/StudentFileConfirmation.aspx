<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="StudentFileConfirmation.aspx.vb" Inherits="HCVU.StudentFileConfirmation"
    Title="<%$ Resources: Title, StudentFileConfirmation %>" EnableEventValidation="False" %>

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
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../JS/Common.js"></script>
    <cc1:ToolkitScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </cc1:ToolkitScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, StudentFileConfirmationBanner %>"
        AlternateText="<%$ Resources: AlternateText, StudentFileConfirmationBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 4px"></div>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="950px" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="950px" />
            <asp:MultiView ID="mvCore" runat="server">
                <asp:View ID="vGrid" runat="server">
                    <table>
                        <tr style="height: 6px"></tr>
                        <tr>
                            <td style="width: 100px">
                                <asp:Label ID="lblGActionText" runat="server" Text="<%$ Resources: Text, Action %>"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlGAction" runat="server" Width="300"
                                    AutoPostBack="true" OnSelectedIndexChanged="ddlGAction_SelectedIndexChanged">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height: 12px"></tr>
                    </table>
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
                                <asp:ImageButton ID="ibtnDConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnDConfirm_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnDReject" runat="server" ImageUrl="<%$ Resources: ImageUrl, RejectBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, RejectBtn %>" OnClick="ibtnDReject_Click" />
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
            <%-- Pop up for Detail Popup (Confirm/Reject) --%>
            <asp:Button ID="btnDP" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeDetailPopup" runat="server" TargetControlID="btnDP"
                PopupControlID="panDP" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panDPHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panDP" runat="server" Style="display: none">
                <asp:Panel ID="panDPHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDPTitle" runat="server" Text="<%$ Resources: Text, ConfirmBoxTitle %>">
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
                                        <asp:Image ID="imgDPIcon" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblDPText" runat="server" Font-Bold="True" Text="<%$ Resources: Text, ConfirmRejectQ %>">
                                        </asp:Label></td>
                                    <td style="width: 40px"></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnDPConfirmConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnDPConfirmConfirm_Click" />
                                        <asp:ImageButton ID="ibtnDPConfirmReject" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnDPConfirmReject_Click" />
                                        <asp:ImageButton ID="ibtnDPCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnDPCancel_Click" /></td>
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
            <%-- Pop up for Detail Popup (Confirm/Reject) --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
