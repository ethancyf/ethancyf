<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="eHealthAccountDeathRecordFileConfirmation.aspx.vb" Inherits="HCVU.eHealthAccountDeathRecordFileConfirmation"
    Title="<%$ Resources: Title, eHealthAccountDeathRecordFileConfirmation %>" EnableEventValidation="False" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/Token/ucInputToken.ascx" TagName="ucInputToken" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, eHealthAccountDeathRecordFileConfirmationBanner %>"
        AlternateText="<%$ Resources: AlternateText, eHealthAccountDeathRecordFileConfirmationBanner %>">
    </asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="950px" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="950px" />
            <asp:MultiView ID="mvCore" runat="server" ActiveViewIndex="-1" OnActiveViewChanged="mvCore_ActiveViewChanged">
                <asp:View ID="vGrid" runat="server">
                    <br />
                    <asp:CheckBox ID="cboShowPendingConfirmationOnly" runat="server" Text="<%$ Resources: Text, ShowPendingConfirmationFilesOnly %>"
                        CssClass="tableText" AutoPostBack="True" OnCheckedChanged="cboShowPendingConfirmationOnly_CheckedChanged" />
                    <br />
                    <br />
                    <asp:GridView ID="gvDeathRecordFile" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                        AllowSorting="True" OnRowDataBound="gvDeathRecordFile_RowDataBound" OnRowCommand="gvDeathRecordFile_RowCommand"
                        OnSelectedIndexChanged="gvDeathRecordFile_SelectedIndexChanged" OnPageIndexChanging="gvDeathRecordFile_PageIndexChanging"
                        OnSorting="gvDeathRecordFile_Sorting" OnPreRender="gvDeathRecordFile_PreRender">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, DeathRecordFileID %>" HeaderStyle-VerticalAlign="Top"
                                HeaderStyle-HorizontalAlign="Center" SortExpression="Death_Record_File_ID">
                                <ItemStyle Width="100px" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblGDeathRecordFileID" runat="server" Text='<%# Eval("Death_Record_File_ID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Description %>" HeaderStyle-VerticalAlign="Top"
                                HeaderStyle-HorizontalAlign="Center" SortExpression="Description">
                                <ItemStyle Width="180px" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblGDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ImportTime %>" HeaderStyle-VerticalAlign="Top"
                                HeaderStyle-HorizontalAlign="Center" SortExpression="Import_Dtm">
                                <ItemStyle Width="130px" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblGImportTime" runat="server" Text='<%# Eval("Import_Dtm") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ImportBy %>" HeaderStyle-VerticalAlign="Top"
                                HeaderStyle-HorizontalAlign="Center" SortExpression="Import_By">
                                <ItemStyle Width="85px" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblGImportBy" runat="server" Text='<%# Eval("Import_By") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ConfirmTime %>" HeaderStyle-VerticalAlign="Top"
                                HeaderStyle-HorizontalAlign="Center" SortExpression="Confirm_Dtm">
                                <ItemStyle Width="130px" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblGConfirmTime" runat="server" Text='<%# Eval("Confirm_Dtm") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, ConfirmBy %>" HeaderStyle-VerticalAlign="Top"
                                HeaderStyle-HorizontalAlign="Center" SortExpression="Confirm_By">
                                <ItemStyle Width="85px" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblGConfirmBy" runat="server" Text='<%# Eval("Confirm_By") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Status %>" HeaderStyle-VerticalAlign="Top"
                                HeaderStyle-HorizontalAlign="Center">
                                <ItemStyle Width="50px" VerticalAlign="Top" HorizontalAlign="Center" />
                                <ItemTemplate>
                                    <asp:ImageButton ID="ibtnGStatus" runat="server" />
                                    <asp:HiddenField ID="hfGStatus" runat="server" Value='<%# Eval("Record_Status") %>' />
                                    <asp:HiddenField ID="hfGProcessing" runat="server" Value='<%# Eval("Processing") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, MatchTime %>" HeaderStyle-VerticalAlign="Top"
                                HeaderStyle-HorizontalAlign="Center" SortExpression="Match_Dtm">
                                <ItemStyle Width="130px" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <asp:Label ID="lblGMatchTime" runat="server" Text='<%# Eval("Match_Dtm") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                    <br />
                    <asp:MultiView ID="mvOuterButton" runat="server" ActiveViewIndex="-1" OnActiveViewChanged="mvOuterButton_ActiveViewChanged">
                        <asp:View ID="ViewNoSelectFile" runat="server">
                        </asp:View>
                        <asp:View ID="ViewSelectFile" runat="server">
                            <center>
                                <table>
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ibtnViewDetail" runat="server" ImageUrl="<%$ Resources: ImageUrl, ViewDetailsBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ViewDetailsBtn %>" OnClick="ibtnViewDetail_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnConfirmFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmFileBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ConfirmFileBtn %>" OnClick="ibtnConfirmFile_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnRemoveFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, RemoveFileBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, RemoveFileBtn %>" OnClick="ibtnRemoveFile_Click" />
                                        </td>
                                    </tr>
                                </table>
                            </center>
                        </asp:View>
                    </asp:MultiView>
                    <br />
                    <asp:Label ID="lblGNote" runat="server" Text="<%$ Resources: Text, DeathRecordFileConfirmationNote %>"></asp:Label>
                    <br />
                </asp:View>
                <asp:View ID="vFinish" runat="server">
                    <asp:ImageButton ID="ibtnFReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                        AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnFReturn_Click" />
                </asp:View>
            </asp:MultiView>
            <%-- Pop up for View Details --%>
            <asp:Button ID="btnHiddenViewDetail" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupViewDetail" runat="server" TargetControlID="btnHiddenViewDetail"
                PopupControlID="panViewDetail" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panViewDetailHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panViewDetail" runat="server" Style="display: none">
                <asp:Panel ID="panViewDetailHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 820px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblViewDetailHeader" runat="server" Text="<%$ Resources: Text, DetailsOfDeathRecordFile %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 820px; height: 450px">
                    <tr style="vertical-align: top">
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #FFFFFF">
                            <table style="width: 806px">
                                <tr>
                                    <td style="padding: 5px 15px 5px 15px">
                                        <cc2:InfoMessageBox ID="udcVDInfoMessageBox" runat="server" />
                                        <table style="width: 765px">
                                            <tr>
                                                <td style="width: 230px">
                                                    <asp:Label ID="lblSVDDeathRecordFileIDText" runat="server" Text="<%$ Resources: Text, DeathRecordFileID %>">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblVDDeathRecordFileID" runat="server" CssClass="tableText"></asp:Label>
                                                    <asp:Label ID="lblVDDeathRecordFileIDFail" runat="server" Text="[(Import Failed)]"
                                                        CssClass="tableText" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblVDTotalNoOfRecordText" runat="server" Text="<%$ Resources: Text, TotalNoOfRecordsInFile %>">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblVDTotalNoOfRecord" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblVDNoOfRecordWithHKIDText" runat="server" Text="<%$ Resources: Text, NoOfRecordsWithHKID %>">
                                                    </asp:Label>
                                                    <asp:Label ID="lblVDNoOfFailRecordText" runat="server" Text="<%$ Resources: Text, NoOfFailedRecords %>">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblVDNoOfRecordWithHKID" runat="server" CssClass="tableText"></asp:Label>
                                                    <asp:Label ID="lblVDNoOfFailRecord" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblVDNoOfRecordWithoutHKIDText" runat="server" Text="<%$ Resources: Text, NoOfRecordsWithoutHKID %>">
                                                    </asp:Label>
                                                    <asp:Label ID="lblVDShowFailRecordOnlyText" runat="server" Text="<%$ Resources: Text, ShowFailedRecordsOnly %>">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblVDNoOfRecordWithoutHKID" runat="server" CssClass="tableText"></asp:Label>
                                                    <asp:CheckBox ID="cboVDShowFailRecordOnly" runat="server" AutoPostBack="True" OnCheckedChanged="cboVDShowFailRecordOnly_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblVDShowRecordWithRowNoInText" runat="server" Text="<%$ Resources: Text, ShowRecordsWithRowNoIn %>">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlVDShowRecordWithRowNoIn" runat="server" Width="130" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlVDShowRecordWithRowNoIn_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 765px">
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:CheckBox ID="cboVDMaskDocumentNo" runat="server" Text="<%$ Resources: Text, MaskIdentityDocumentNo %>"
                                                        AutoPostBack="True" OnCheckedChanged="cboVDMaskDocumentNo_CheckedChanged" />
                                                    <asp:HiddenField ID="hfVDMaskDocumentNo" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="panVDEntryStaging" runat="server" BorderWidth="1px" Height="300px"
                                                        ScrollBars="Vertical">
                                                        <asp:GridView ID="gvVDEntryStaging" runat="server" Width="741px" AutoGenerateColumns="False"
                                                            AllowPaging="False" AllowSorting="False" EnableViewState="False" OnRowDataBound="gvVDEntryStaging_RowDataBound"
                                                            OnPageIndexChanging="gvVDEntryStaging_PageIndexChanging" OnSorting="gvVDEntryStaging_Sorting"
                                                            OnPreRender="gvVDEntryStaging_PreRender">
                                                            <Columns>
                                                                <asp:TemplateField HeaderText="<%$ Resources: Text, RowNo %>" HeaderStyle-VerticalAlign="Top"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemStyle Width="70px" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGRowNo" runat="server" Text='<%# Eval("Seq_No") %>'></asp:Label>
                                                                        <asp:Image ID="imgGRowNo" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                                                            AlternateText="<%$ Resources: Text, DuplicateRecord %>" ToolTip="<%$ Resources: Text, DuplicateRecord %>"
                                                                            Visible="False" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources: Text, HKIdentityCardNo %>" HeaderStyle-VerticalAlign="Top"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemStyle Width="110px" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGHKID" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources: Text, HKIdentityCardNo %>" HeaderStyle-VerticalAlign="Top"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemStyle Width="110px" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGHKIDM" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources: Text, DateOfDeath %>" HeaderStyle-VerticalAlign="Top"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemStyle Width="110px" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGDOD" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources: Text, DateOfRegistration %>" HeaderStyle-VerticalAlign="Top"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemStyle Width="110px" VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGDOR" runat="server"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="<%$ Resources: Text, Name %>" HeaderStyle-VerticalAlign="Top"
                                                                    HeaderStyle-HorizontalAlign="Center">
                                                                    <ItemStyle VerticalAlign="Top" />
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblGEnglishName" runat="server" Text='<%# Eval("English_Name") %>'></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="text-align: center; padding-top: 10px">
                                                    <asp:ImageButton ID="ibtnVDClose" runat="server" ImageUrl="<%$ Resources: ImageUrl, CloseBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, CloseBtn %>" OnClick="ibtnVDClose_Click" />
                                                    <asp:ImageButton ID="ibtnPopupConfirmFileConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnPopupConfirmFileConfirm_Click" />
                                                    <asp:ImageButton ID="ibtnPopupConfirmFileCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnPopupConfirmFileCancel_Click" />
                                                </td>
                                            </tr>
                                        </table>
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
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                            height: 7px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- End of Pop up for View Details --%>
            <%-- Pop up for Remove File --%>
            <asp:Button ID="btnHiddenRemoveFile" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupRemoveFile" runat="server" TargetControlID="btnHiddenRemoveFile"
                PopupControlID="panRemoveFile" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panRemoveFileHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panRemoveFile" runat="server" Style="display: none">
                <asp:Panel ID="panRemoveFileHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblRemoveFileHeader" runat="server" Text="<%$ Resources: Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #FFFFFF">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgPopupRemoveFile" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblPopupRemoveFileText" runat="server" Font-Bold="True" Text="<%$ Resources: Text, ConfirmToRemoveFileQ %>"></asp:Label></td>
                                    <td style="width: 40px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnPopupRemoveFileConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnPopupRemoveFileConfirm_Click" />
                                        <asp:ImageButton ID="ibtnPopupRemoveFileCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnPopupRemoveFileCancel_Click" />
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
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                            height: 7px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <%-- End of Pop up for Remove File --%>
            <%-- Pop up for Unmask --%>
            <asp:Button ID="btnHiddenUnmask" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupUnmask" runat="server" TargetControlID="btnHiddenUnmask"
                PopupControlID="panUnmask" BackgroundCssClass="modalBackgroundTransparent" DropShadow="False"
                RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panUnmask" runat="server" Style="display: none">
                <uc2:ucInputToken ID="udcPUInputToken" runat="server"></uc2:ucInputToken>
            </asp:Panel>
            <%-- End of Pop up for Unmask --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
