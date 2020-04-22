<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="spProfessionVerification.aspx.vb" Inherits="HCVU.spProfessionVerification"
    Title="<%$ Resources:Title, SPProfessionVerification %>" EnableEventValidation="False" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Image ID="img_header" runat="server" AlternateText="<%$ Resources:AlternateText, SPProfessionVerificationBanner %>"
                ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, SPProfessionVerificationBanner %>" />
            <cc2:MessageBox ID="udcErrorMessage" runat="server" Width="90%"></cc2:MessageBox>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="90%" />
            <cc1:TabContainer ID="tcContainer" runat="server" ActiveTabIndex="1" AutoPostBack="True"
                CssClass="m_ajax__tab_xp" OnActiveTabChanged="tcContainer_ActiveTabChanged">
                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="TabPanel1">
                    <HeaderTemplate>
                        <asp:Label ID="lblSPHPVCatPend" runat="server" Text="<%$ Resources:Text, SPHPVCatPendRecord %>"></asp:Label>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:Label ID="lblProfessionCode" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
                        <asp:DropDownList ID="ddlProcessionCode" runat="server">
                        </asp:DropDownList>
                        <asp:ImageButton ID="btnSearchPend" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>"
                            AlternateText="<%$ Resources:AlternateText, SearchBtn %>" ImageAlign="AbsMiddle" />
                        <br />
                        <br />
                        <asp:MultiView ID="mvPend" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vPend" runat="server">
                                <asp:GridView ID="gvPend" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                                    AllowSorting="True" Width="970px">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("RecordNum") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" Width="35px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="EnrolmentNum" SortExpression="EnrolmentNum" HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                                            <ItemStyle Width="145px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="HKID" SortExpression="HKID " HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                            <ItemStyle Width="145px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:TemplateField SortExpression="SPEngName" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblREname" runat="server" Text='<%# Eval("SPEngName") %>'></asp:Label><br />
                                                <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("SPChiName") %>' CssClass="TextGridChi"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle VerticalAlign="Top" Width="270px" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Profession" SortExpression="Profession" HeaderText="<%$ Resources:Text, HealthProf %>">
                                            <ItemStyle Width="300px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RegistrationCode" SortExpression="RegistrationCode" HeaderText="<%$ Resources:Text, RegCode %>">
                                            <ItemStyle Width="160px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td valign="top" align="center">
                                            <asp:ImageButton ID="btnExport" runat="server" ImageUrl="<%$ Resources:ImageUrl, ExportRecordBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ExportRecordBtn %>" ImageAlign="AbsMiddle" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="TabPanel2">
                    <HeaderTemplate>
                        <asp:Label ID="lblSPHPVCatExportImport" runat="server" Text="<%$ Resources:Text, SPHPVCatExportImport %>"></asp:Label>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:Panel ID="pnlSearch" runat="server" Visible="False">
                            <table style="width: 90%">
                                <tr>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblTitleStatus" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblStatus" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblTitleProfession" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblProfession" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblTitleExportFrom" runat="server" Text="<%$ Resources:Text, ExportFrom %>"></asp:Label></td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblExportFrom" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblTitleExportTo" runat="server" Text="<%$ Resources:Text, ExportTo %>"></asp:Label></td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblExportTo" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <br />
                        <asp:MultiView ID="mvExportImport" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vExportImportSearch" runat="server">
                                <table style="width: 95%">
                                    <tr>
                                        <td style="width: 25%">
                                            <asp:Label ID="lblTitleStatus02" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                        <td style="width: 25%">
                                            <asp:DropDownList ID="ddlViewStatus" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 25%">
                                            <asp:Label ID="lblTitleProfession02" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                        <td style="width: 25%">
                                            <asp:DropDownList ID="ddlProfessionCode02" runat="server">
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%">
                                            <asp:Label ID="lblTitleExportFrom02" runat="server" Text="<%$ Resources:Text, ExportFrom %>"></asp:Label></td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtExportFrom" MaxLength="10" runat="server" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="filterExportFromDate" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="txtExportFrom" ValidChars="-" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>                                           
                                            <asp:ImageButton ID="btnFromDate" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />
                                            <asp:Image ID="imgExportFromError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
                                            <cc1:CalendarExtender ID="calExFromDate" CssClass="ajax_cal" runat="server" PopupButtonID="btnFromDate"
                                                TargetControlID="txtExportFrom" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                                            </cc1:CalendarExtender>
                                        </td>
                                        <td style="width: 25%">
                                            <asp:Label ID="lblTitleExportTo02" runat="server" Text="<%$ Resources:Text, ExportTo %>"></asp:Label></td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtExportTo" MaxLength="10" runat="server" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                            <cc1:FilteredTextBoxExtender ID="filterExportToDate" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="txtExportTo" ValidChars="-" Enabled="True">
                                            </cc1:FilteredTextBoxExtender>                                            
                                            <asp:ImageButton ID="btnToDate" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" />
                                            <asp:Image ID="imgExportToError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
                                            <cc1:CalendarExtender ID="calExToDate" CssClass="ajax_cal" runat="server" PopupButtonID="btnToDate" TargetControlID="txtExportTo" TodaysDateFormat="d MMMM, yyyy"
                                                Enabled="True">
                                            </cc1:CalendarExtender>
                                        </td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="btnSearchExportList" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" OnClick="btnSearchExportList_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vExportImport" runat="server">
                                <asp:GridView ID="gvExportImport" runat="server" Width="970px" EnableTheming="True"
                                    AllowPaging="True" AutoGenerateColumns="False" AllowSorting="True" OnSelectedIndexChanged="gvExportImport_SelectedIndexChanged"
                                    OnRowDataBound="gvExportImport_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="File_Name" SortExpression="File_Name" HeaderText="<%$ Resources:Text, FileName %>">
                                            <ItemStyle Width="130px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Export_Dtm" SortExpression="Export_Dtm" HeaderText="<%$ Resources:Text, ExportDate %>">
                                            <ItemStyle Width="150px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Export_User" SortExpression="Export_User" HeaderText="<%$ Resources:Text, ExportBy %>">
                                            <ItemStyle Width="125px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Profession_Description" SortExpression="Profession_Description"
                                            HeaderText="<%$ Resources:Text, HealthProf %>">
                                            <ItemStyle Width="290px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Import_Dtm" SortExpression="Import_Dtm" HeaderText="<%$ Resources:Text, ImportDate %>">
                                            <ItemStyle Width="150px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Import_User" SortExpression="Import_User" HeaderText="<%$ Resources:Text, ImportBy %>">
                                            <ItemStyle Width="125px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                    </Columns>
                                    <SelectedRowStyle CssClass="SelectedRowStyle" />
                                </asp:GridView>
                            </asp:View>
                            <asp:View ID="vSelected" runat="server">
                                <asp:GridView ID="gvSelected" runat="server" Width="970px" EnableTheming="True" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="File_Name" SortExpression="File_Name" HeaderText="<%$ Resources:Text, FileName %>">
                                            <ItemStyle Width="130px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Export_Dtm" SortExpression="Export_Dtm" HeaderText="<%$ Resources:Text, ExportDate %>">
                                            <ItemStyle Width="150px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Export_User" SortExpression="Export_User" HeaderText="<%$ Resources:Text, ExportBy %>">
                                            <ItemStyle Width="125px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Profession_Description" SortExpression="Profession_Description"
                                            HeaderText="<%$ Resources:Text, HealthProf %>">
                                            <ItemStyle Width="290px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Import_Dtm" SortExpression="Import_Dtm" HeaderText="<%$ Resources:Text, ImportDate %>">
                                            <ItemStyle Width="150px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Import_User" SortExpression="Import_User" HeaderText="<%$ Resources:Text, ImportBy %>">
                                            <ItemStyle Width="125px" VerticalAlign="Top" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                            </asp:View>
                        </asp:MultiView>
                        <asp:Panel ID="pnlImport" runat="server" Style="display: none">
                            <asp:Panel ID="pnlImportUpload" runat="server" Style="display: none">
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:Label ID="lblImport" runat="server"></asp:Label>
                                            <asp:FileUpload ID="fileUploadImport" runat="server" Width="400px" />
                                            <asp:ImageButton ID="btnImport" runat="server" ImageUrl="<%$ Resources:ImageUrl, ImportFileBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ImportFileBtn %>" ImageAlign="AbsMiddle"
                                                OnClick="btnImport_Click" />
                                            <asp:ImageButton ID="btnUploadCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, CancelBtn %>" ImageAlign="AbsMiddle"
                                                OnClick="btnUploadCancel_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="pnlImportBtnNormal" runat="server" Visible="False">
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left">
                                            <asp:ImageButton ID="btnExportBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="btnExportBack_Click" /></td>
                                        <td align="center" style="width: 100%">
                                            <asp:ImageButton ID="btnImportFile" runat="server" ImageUrl="<%$ Resources:ImageUrl, ImportFileBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ImportFileBtn %>" ImageAlign="AbsMiddle"
                                                OnClick="btnImportFile_Click" />
                                            <asp:ImageButton ID="btnViewResult" runat="server" ImageUrl="<%$ Resources:ImageUrl, ViewResultBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ViewResultBtn %>" ImageAlign="AbsMiddle"
                                                OnClick="btnViewResult_Click" />
                                            <asp:ImageButton ID="btnCancelExport" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelExportBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, CancelExportBtn %>" ImageAlign="AbsMiddle"
                                                OnClick="btnCancelExport_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </asp:Panel>
                            <asp:Panel ID="pnlImportBtnAction" runat="server" Visible="False">
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td valign="top" align="center">
                                            <asp:Label ID="lblTitleFileUpload" runat="server" Text="<%$ Resources:Text, UploadFile %>"></asp:Label>
                                            <asp:Label ID="lblFileUpload" runat="server" CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="btnImportConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" ImageAlign="AbsMiddle"
                                                OnClick="btnImportConfirm_Click" />
                                            <asp:ImageButton ID="btnViewUploadResult" runat="server" ImageUrl="<%$ Resources:ImageUrl, ViewResultBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ViewResultBtn %>" ImageAlign="AbsMiddle"
                                                OnClick="btnViewUploadResult_Click" />
                                            <asp:ImageButton ID="btnImportCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, CancelBtn %>" ImageAlign="AbsMiddle"
                                                OnClick="btnImportCancel_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                        </asp:Panel>
                        <br />
                        <asp:Button ID="btnHiddenShowDialog" runat="server" Style="display: none" />
                        <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmCancelExport" runat="server"
                            PopupDragHandleControlID="pnlConfirmMsgHeading" TargetControlID="btnHiddenShowDialog"
                            BackgroundCssClass="modalBackgroundTransparent" PopupControlID="pnlConfirmCancelExport"
                            RepositionMode="None" DynamicServicePath="" Enabled="True">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnlConfirmCancelExport" runat="server" Style="display: none">
                            <asp:Panel ID="pnlConfirmMsgHeading" runat="server" Style="cursor: move;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                                    <tr>
                                        <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                                        </td>
                                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                                            <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/left.png); width: 9px; background-repeat: repeat-y">
                                    </td>
                                    <td style="background-color: #ffffff">
                                        <table style="width: 591">
                                            <tr>
                                                <td align="left" style="width: 40px; height: 42px" valign="middle">
                                                    <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                                <td align="center" style="height: 42px">
                                                    <asp:Label ID="lblMsg" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ConfirmCancelExport %>"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="2">
                                                    <asp:ImageButton ID="ibtnDialogConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="btn_confirm_Click" />
                                                    <asp:ImageButton ID="ibtnDialogCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnDialogCancel_Click" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-image: url(../Images/dialog/bottom-left.png); width: 9px; height: 7px">
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
                        <asp:Button ID="btnHiddenViewResult" runat="server" Style="display: none" />
                        <cc1:ModalPopupExtender ID="ModalPopupExtenderViewResult" runat="server" PopupDragHandleControlID="pnlViewResultHeading"
                            TargetControlID="btnHiddenViewResult" BackgroundCssClass="modalBackgroundTransparent"
                            PopupControlID="pnlViewResult" RepositionMode="None" DynamicServicePath="" Enabled="True">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnlViewResult" runat="server" Style="display: none">
                            <asp:Panel ID="pnlViewResultHeading" runat="server">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 866px">
                                    <tr>
                                        <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                                        </td>
                                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, ViewResult %>"></asp:Label></td>
                                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 866px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/left.png); width: 9px; background-repeat: repeat-y">
                                    </td>
                                    <td style="background-color: #ffffff">
                                        <table style="width: 100%">
                                            <tr>
                                                <td align="center" valign="top" style="height: 380px">
                                                    <asp:GridView ID="gvViewResult" runat="server" Width="850px" AllowPaging="True" AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:BoundField DataField="ReferenceNo" SortExpression="ReferenceNo" HeaderText="<%$ Resources:Text, RefNo %>">
                                                                <ItemStyle Width="200px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="SPHKID" SortExpression="SPHKID" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                                                <ItemStyle Width="130px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="RegistrationCode" SortExpression="RegistrationCode" HeaderText="<%$ Resources:Text, RegCode %>">
                                                                <ItemStyle Width="130px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Surname" SortExpression="Surname" HeaderText="<%$ Resources:Text, Surname %>">
                                                                <ItemStyle Width="60px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="OtherName" SortExpression="OtherName" HeaderText="<%$ Resources:Text, OtherName %>">
                                                                <ItemStyle Width="90px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Result" SortExpression="Result" HeaderText="<%$ Resources:Text, Result %>">
                                                                <ItemStyle Width="40px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="<%$ Resources:Text, Remark %>">
                                                                <ItemStyle Width="150px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="<%$ Resources:Text, Status %>">
                                                                <ItemStyle Width="50px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <asp:ImageButton ID="btnClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="btnClose_Click" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-image: url(../Images/dialog/bottom-left.png); width: 9px; height: 7px">
                                    </td>
                                    <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                                        height: 7px">
                                    </td>
                                    <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; background-position-x: -2px">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="TabPanel3">
                    <HeaderTemplate>
                        <asp:Label ID="lblSPHPVCatVerify" runat="server" Text="<%$ Resources:Text, SPHPVCatVerifyRecord %>"></asp:Label>
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:Panel ID="pnlVerifySearch" runat="server" Visible="False">
                            <table style="width: 90%">
                                <tr>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblTitleVerifyStatus" runat="server" Text="<%$ Resources:Text, Result %>"></asp:Label></td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblVerifyStatus" runat="server" Text="" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblTitleRecordStatus" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblRecordStatus" runat="server" Text="" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblTitlERN" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblERN" runat="server" Text="" CssClass="tableText"></asp:Label></td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblTitleHKID" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                    <td style="width: 25%">
                                        <asp:Label ID="lblHKID" runat="server" Text="" CssClass="tableText"></asp:Label></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:MultiView ID="mvVerify" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vVerifySearch" runat="server">
                                <table style="width: 90%">
                                    <tr>
                                        <td style="width: 25%">
                                            <asp:Label ID="lblTitleVerifyStatus02" runat="server" Text="<%$ Resources:Text, Result %>"></asp:Label></td>
                                        <td style="width: 25%">
                                            <asp:DropDownList ID="ddlVerifyStatus" runat="server" AutoPostBack="false">
                                            </asp:DropDownList></td>
                                        <td style="width: 25%">
                                            <asp:Label ID="lblTitleRecordStatus02" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                        <td style="width: 25%">
                                            <asp:DropDownList ID="ddlRecordStatus" runat="server" AutoPostBack="True">
                                                <asp:ListItem Text="Any" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Active" Value="A"></asp:ListItem>
                                                <asp:ListItem Text="Defer" Value="D"></asp:ListItem>
                                            </asp:DropDownList></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 25%">
                                            <asp:Label ID="lblTitlERN02" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtEnrolRefNo" runat="server" MaxLength="17" onChange="Upper(event,this)"></asp:TextBox></td>
                                        <td style="width: 25%">
                                            <asp:Label ID="lblTitleHKID02" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                        <td style="width: 25%">
                                            <asp:TextBox ID="txtSPHKID" runat="server" MaxLength="11" onChange="formatHKID(this)"></asp:TextBox></td>
                                    </tr>
                                </table>
                                <table style="width: 100%">
                                    <tr>
                                        <td align="center">
                                            <asp:ImageButton ID="btnSearchVerify" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vVerifyValid" runat="server">
                                <br />
                                <br />
                                <asp:Label ID="lblResultValid" runat="server" CssClass="tableText" Text="<%$ Resources:Text, ProfessionalVerificationResultValid %>"></asp:Label>
                                <asp:GridView ID="gvVerifyValid" runat="server" Width="970" AutoGenerateColumns="False"
                                    OnRowDataBound="gvVerifyValid_RowDataBound" AllowPaging="True" AllowSorting="True">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox runat="server" ID="chkHeaderSelectAll" /></HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelected01" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="20" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("RecordNum") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="EnrolmentNum" HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton Visible="False" ID="lnkbtnERN" runat="server" Text='<%# Eval("EnrolmentNum") %>'
                                                    CommandName="SPPopup" CommandArgument='<%# Eval("EnrolmentNum") %>'></asp:LinkButton>
                                                <asp:Label Visible="True" ID="lblERNNo" runat="server" Text='<%# Eval("EnrolmentNum") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="110" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="SPID" HeaderText="<%$ Resources:Text, ServiceProviderID %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSPID" runat="server" Text='<%# Eval("SPID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="HKID" SortExpression="HKID" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:TemplateField SortExpression="SPEngName" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblREname" runat="server" Text='<%# Eval("SPEngName") %>'></asp:Label><br />
                                                <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("SPChiName") %>' CssClass="TextGridChi"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="270" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Profession" SortExpression="Profession" HeaderText="<%$ Resources:Text, HealthProf %>">
                                            <ItemStyle Width="230" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RegistrationCode" SortExpression="RegistrationCode" HeaderText="<%$ Resources:Text, RegCode %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="<%$ Resources:Text, Status %>">
                                            <ItemStyle Width="50" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Result" SortExpression="Result" HeaderText="<%$ Resources:Text, Result %>">
                                            <ItemStyle Width="40" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="<%$ Resources:Text, Remark %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td valign="Top" align="left">
                                            <asp:ImageButton ID="btnVerifyBack01" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="btnVerifyBack_Click" />
                                        </td>
                                        <td valign="top" align="center">
                                            <asp:ImageButton ID="btnValidAccept" runat="server" OnClick="btnValidAccept_Click"
                                                AlternateText="<%$ Resources:AlternateText, AcceptBtn %>" ImageUrl="<%$ Resources:ImageUrl, AcceptBtn %>" />
                                            <asp:ImageButton ID="btnValidDefer" runat="server" OnClick="btnValidDefer_Click"
                                                AlternateText="<%$ Resources:AlternateText, DeferBtn %>" ImageUrl="<%$ Resources:ImageUrl, DeferBtn %>" />
                                            <asp:ImageButton ID="btnValidReject" runat="server" OnClick="btnValidReject_Click"
                                                AlternateText="<%$ Resources:AlternateText, RejectBtn %>" ImageUrl="<%$ Resources:ImageUrl, RejectBtn %>" />
                                            <asp:ImageButton ID="btnValidReturn" runat="server" OnClick="btnValidReturn_Click"
                                                AlternateText="<%$ Resources:AlternateText, ReturnForAmendmentBtn %>" ImageUrl="<%$ Resources:ImageUrl, ReturnForAmendmentBtn %>" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vVerifyInvalid" runat="server">
                                <br />
                                <br />
                                <asp:Label ID="lblResultInvalid" runat="server" CssClass="tableText" Text="<%$ Resources:Text, ProfessionalVerificationResultInValid %>"></asp:Label>
                                <asp:GridView ID="gvVerifyInvalid" runat="server" Width="970" AutoGenerateColumns="False"
                                    OnRowDataBound="gvVerifyInvalid_RowDataBound" AllowPaging="True" AllowSorting="True">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox runat="server" ID="chkHeaderSelectAll" /></HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelected02" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="20" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("RecordNum") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="EnrolmentNum" HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton Visible="False" ID="lnkbtnERN" runat="server" Text='<%# Eval("EnrolmentNum") %>'
                                                    CommandName="SPPopup" CommandArgument='<%# Eval("EnrolmentNum") %>'></asp:LinkButton>
                                                <asp:Label Visible="True" ID="lblERNNo" runat="server" Text='<%# Eval("EnrolmentNum") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="110" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="SPID" HeaderText="<%$ Resources:Text, ServiceProviderID %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSPID" runat="server" Text='<%# Eval("SPID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="HKID" SortExpression="HKID" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:TemplateField SortExpression="SPEngName" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblREname" runat="server" Text='<%# Eval("SPEngName") %>'></asp:Label><br />
                                                <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("SPChiName") %>' CssClass="TextGridChi"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="270" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Profession" SortExpression="Profession" HeaderText="<%$ Resources:Text, HealthProf %>">
                                            <ItemStyle Width="230" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RegistrationCode" SortExpression="RegistrationCode" HeaderText="<%$ Resources:Text, RegCode %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="<%$ Resources:Text, Status %>">
                                            <ItemStyle Width="50" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Result" SortExpression="Result" HeaderText="<%$ Resources:Text, Result %>">
                                            <ItemStyle Width="40" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="<%$ Resources:Text, Remark %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td valign="top" align="left">
                                            <asp:ImageButton ID="btnVerifyBack02" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="btnVerifyBack_Click" />
                                        </td>
                                        <td valign="top" align="center">
                                            <asp:ImageButton ID="btnInValidDefer" runat="server" OnClick="btnInValidDefer_Click"
                                                AlternateText="<%$ Resources:AlternateText, DeferBtn %>" ImageUrl="<%$ Resources:ImageUrl, DeferBtn %>" />
                                            <asp:ImageButton ID="btnInValidReject" runat="server" OnClick="btnInValidReject_Click"
                                                AlternateText="<%$ Resources:AlternateText, RejectBtn %>" ImageUrl="<%$ Resources:ImageUrl, RejectBtn %>" />
                                            <asp:ImageButton ID="btnInValidReturn" runat="server" OnClick="btnInValidReturn_Click"
                                                AlternateText="<%$ Resources:AlternateText, ReturnForAmendmentBtn %>" ImageUrl="<%$ Resources:ImageUrl, ReturnForAmendmentBtn %>" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vVerifySuspect" runat="server">
                                <br />
                                <br />
                                <asp:Label ID="lblResultSuspect" runat="server" CssClass="tableText" Width="970"
                                    Text="<%$ Resources:Text, ProfessionalVerificationResultSuspect %>"></asp:Label>
                                <asp:GridView ID="gvVerifySuspect" runat="server" AutoGenerateColumns="False" OnRowDataBound="gvVerifySuspect_RowDataBound"
                                    AllowPaging="True" AllowSorting="True">
                                    <Columns>
                                        <asp:TemplateField>
                                            <HeaderTemplate>
                                                <asp:CheckBox runat="server" ID="chkHeaderSelectAll" /></HeaderTemplate>
                                            <ItemTemplate>
                                                <asp:CheckBox ID="chkSelected03" runat="server" />
                                            </ItemTemplate>
                                            <ItemStyle Width="20" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("RecordNum") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="EnrolmentNum" HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton Visible="False" ID="lnkbtnERN" runat="server" Text='<%# Eval("EnrolmentNum") %>'
                                                    CommandName="SPPopup" CommandArgument='<%# Eval("EnrolmentNum") %>'></asp:LinkButton>
                                                <asp:Label Visible="True" ID="lblERNNo" runat="server" Text='<%# Eval("EnrolmentNum") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="110" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="SPID" HeaderText="<%$ Resources:Text, ServiceProviderID %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSPID" runat="server" Text='<%# Eval("SPID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="HKID" SortExpression="HKID" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:TemplateField SortExpression="SPEngName" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblREname" runat="server" Text='<%# Eval("SPEngName") %>'></asp:Label><br />
                                                <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("SPChiName") %>' CssClass="TextGridChi"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="270" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Profession" SortExpression="Profession" HeaderText="<%$ Resources:Text, HealthProf %>">
                                            <ItemStyle Width="230" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RegistrationCode" SortExpression="RegistrationCode" HeaderText="<%$ Resources:Text, RegCode %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="<%$ Resources:Text, Status %>">
                                            <ItemStyle Width="50" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Result" SortExpression="Result" HeaderText="<%$ Resources:Text, Result %>">
                                            <ItemStyle Width="40" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="<%$ Resources:Text, Remark %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td valign="top" align="left">
                                            <asp:ImageButton ID="btnVerifyBack03" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="btnVerifyBack_Click" />
                                        </td>
                                        <td valign="top" align="center">
                                            <asp:ImageButton ID="btnSuspectAccept" runat="server" OnClick="btnSuspectAccept_Click"
                                                AlternateText="<%$ Resources:AlternateText, AcceptBtn %>" ImageUrl="<%$ Resources:ImageUrl, AcceptBtn %>" />
                                            <asp:ImageButton ID="btnSuspectDefer" runat="server" OnClick="btnSuspectDefer_Click"
                                                AlternateText="<%$ Resources:AlternateText, DeferBtn %>" ImageUrl="<%$ Resources:ImageUrl, DeferBtn %>" />
                                            <asp:ImageButton ID="btnSuspectReject" runat="server" OnClick="btnSuspectReject_Click"
                                                AlternateText="<%$ Resources:AlternateText, RejectBtn %>" ImageUrl="<%$ Resources:ImageUrl, RejectBtn %>" />
                                            <asp:ImageButton ID="btnSuspectReturn" runat="server" OnClick="btnSuspectReturn_Click"
                                                AlternateText="<%$ Resources:AlternateText, ReturnForAmendmentBtn %>" ImageUrl="<%$ Resources:ImageUrl, ReturnForAmendmentBtn %>" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vVerifyNA" runat="server">
                                <br />
                                <br />
                                <asp:Label ID="lblResultNA" runat="server" CssClass="tableText" Width="970" Text="<%$ Resources:Text, ProfessionalVerificationResultNA %>"></asp:Label>
                                <asp:GridView ID="gvVerifyNA" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                    AllowSorting="True">
                                    <Columns>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                <asp:Label ID="lblRecordNum" runat="server" Text='<%# Eval("RecordNum") %>' Visible="false"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="35px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="EnrolmentNum" HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                                            <ItemTemplate>
                                                <asp:LinkButton Visible="False" ID="lnkbtnERN" runat="server" Text='<%# Eval("EnrolmentNum") %>'
                                                    CommandName="SPPopup" CommandArgument='<%# Eval("EnrolmentNum") %>'></asp:LinkButton>
                                                <asp:Label Visible="True" ID="lblERNNo" runat="server" Text='<%# Eval("EnrolmentNum") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="110" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:TemplateField SortExpression="SPID" HeaderText="<%$ Resources:Text, ServiceProviderID %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSPID" runat="server" Text='<%# Eval("SPID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="HKID" SortExpression="HKID" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:TemplateField SortExpression="SPEngName" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblREname" runat="server" Text='<%# Eval("SPEngName") %>'></asp:Label><br />
                                                <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("SPChiName") %>' CssClass="TextGridChi"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="270" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Profession" SortExpression="Profession" HeaderText="<%$ Resources:Text, HealthProf %>">
                                            <ItemStyle Width="190" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RegistrationCode" SortExpression="RegistrationCode" HeaderText="<%$ Resources:Text, RegCode %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="<%$ Resources:Text, Status %>">
                                            <ItemStyle Width="50" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="FileName" SortExpression="FileName" HeaderText="<%$ Resources:Text, ExportFile %>">
                                            <ItemStyle Width="120" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="ExportDtm" SortExpression="ExportDtm" HeaderText="<%$ Resources:Text, ExportDate %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Result" SortExpression="Result" HeaderText="<%$ Resources:Text, Result %>"
                                            Visible="False" />
                                        <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="<%$ Resources:Text, Remark %>"
                                            Visible="False" />
                                    </Columns>
                                </asp:GridView>
                                <table style="width: 100%">
                                    <tr>
                                        <td valign="top" align="left">
                                            <asp:ImageButton ID="btnVerifyBack04" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="btnVerifyBack_Click" />
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </asp:View>
                            <asp:View ID="vVerifyConfirm" runat="server">
                                <br />
                                <asp:Label ID="lblConfirm" runat="server" CssClass="tableText"></asp:Label>
                                -
                                <asp:Label ID="lblAction" runat="server" CssClass="tableText"></asp:Label>
                                <asp:GridView ID="gvVerifyConfirm" runat="server" Width="970" AutoGenerateColumns="False">
                                    <Columns>
                                        <asp:BoundField DataField="RecordNum" SortExpression="RecordNum" ItemStyle-Width="35"
                                            Visible="False" />
                                        <asp:BoundField DataField="EnrolmentNum" SortExpression="EnrolmentNum" HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                                            <ItemStyle Width="110" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:TemplateField SortExpression="SPID" HeaderText="<%$ Resources:Text, ServiceProviderID %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSPID" runat="server" Text='<%# Eval("SPID") %>'></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="70px" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="HKID" SortExpression="HKID" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:TemplateField SortExpression="SPEngName" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                            <ItemTemplate>
                                                <asp:Label ID="lblREname" runat="server" Text='<%# Eval("SPEngName") %>'></asp:Label><br />
                                                <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("SPChiName") %>' CssClass="TextGridChi"></asp:Label>
                                            </ItemTemplate>
                                            <ItemStyle Width="130" VerticalAlign="Top" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Profession" SortExpression="Profession" HeaderText="<%$ Resources:Text, HealthProf %>">
                                            <ItemStyle Width="230" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="RegistrationCode" SortExpression="RegistrationCode" HeaderText="<%$ Resources:Text, RegCode %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Status" SortExpression="Status" HeaderText="<%$ Resources:Text, Status %>">
                                            <ItemStyle Width="50" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Result" SortExpression="Result" HeaderText="<%$ Resources:Text, Result %>">
                                            <ItemStyle Width="40" VerticalAlign="Top" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="Remark" SortExpression="Remark" HeaderText="<%$ Resources:Text, Remark %>">
                                            <ItemStyle Width="100" VerticalAlign="Top" />
                                        </asp:BoundField>
                                    </Columns>
                                </asp:GridView>
                                <br />
                                <table style="width: 100%">
                                    <tr>
                                        <td valign="top">
                                            <asp:ImageButton ID="btnBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="btnBack_Click" /></td>
                                        <td align="left" valign="top">
                                            <asp:ImageButton ID="btnConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="btnConfirm_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                        </asp:MultiView>
                        <asp:Button ID="btnHiddenViewVerifyRecords" runat="server" Style="display: none" />
                        <cc1:ModalPopupExtender ID="ModalPopupExtenderViewVerifyRecords" runat="server" PopupDragHandleControlID="pnlViewVerifyRecordsHeading"
                            TargetControlID="btnHiddenViewVerifyRecords" BackgroundCssClass="modalBackgroundTransparent"
                            PopupControlID="pnlViewVerifyRecords" RepositionMode="None" DynamicServicePath=""
                            Enabled="True">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="pnlViewVerifyRecords" runat="server" Style="display: none">
                            <asp:Panel ID="pnlViewVerifyRecordsHeading" runat="server">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 866px">
                                    <tr>
                                        <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                                        </td>
                                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Text, VerifyRecords %>"></asp:Label></td>
                                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 866px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/left.png); width: 9px; background-repeat: repeat-y">
                                    </td>
                                    <td style="background-color: #ffffff">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="width: 25%">
                                                    <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                                <td style="width: 25%">
                                                    <asp:Label ID="lblViewVerifyEnrolRefNo" runat="server" CssClass="tableText"></asp:Label></td>
                                                <td style="width: 25%">
                                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                                <td style="width: 25%">
                                                    <asp:Label ID="lblViewVerifySPEName" runat="server" CssClass="tableText"></asp:Label>
                                                    <asp:Label ID="lblViewVerifySPCName" runat="server" CssClass="TextChi"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td colspan='4' align="center" valign="top" style="height: 380px">
                                                    <asp:GridView ID="gvViewVerifyRecords" runat="server" Width="850" AllowPaging="True"
                                                        AutoGenerateColumns="False">
                                                        <Columns>
                                                            <asp:BoundField DataField="Profession_Description" SortExpression="Profession_Description"
                                                                HeaderText="<%$ Resources:Text, HealthProf %>">
                                                                <ItemStyle Width="260" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Registration_Code" SortExpression="Registration_Code"
                                                                HeaderText="<%$ Resources:Text, RegCode %>">
                                                                <ItemStyle Width="140px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Verification_result" SortExpression="Verification_result"
                                                                HeaderText="<%$ Resources:Text, Result %>">
                                                                <ItemStyle Width="50px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Record_Status" SortExpression="Record_Status" HeaderText="<%$ Resources:Text, Status %>">
                                                                <ItemStyle Width="100px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="File_Name" SortExpression="File_Name" HeaderText="<%$ Resources:Text, ExportFile %>">
                                                                <ItemStyle Width="150px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Export_Dtm" SortExpression="Export_Dtm" HeaderText="<%$ Resources:Text, ExportDate %>">
                                                                <ItemStyle Width="150px" VerticalAlign="Top" />
                                                            </asp:BoundField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan='4' align="center">
                                                    <asp:ImageButton ID="btnViewVerifyRecordsClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="btnViewVerifyRecordsClose_Click" /></td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-image: url(../Images/dialog/bottom-left.png); width: 9px; height: 7px">
                                    </td>
                                    <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                                        height: 7px">
                                    </td>
                                    <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; background-position-x:-2px">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
            <asp:MultiView ID="mvBack" runat="server" ActiveViewIndex="-1">
                <asp:View ID="vBack" runat="server">
                    <asp:ImageButton ID="btnBack01" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>" OnClick="btnBack01_Click" />
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
