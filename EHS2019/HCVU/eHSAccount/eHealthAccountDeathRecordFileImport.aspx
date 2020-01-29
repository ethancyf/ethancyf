<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="eHealthAccountDeathRecordFileImport.aspx.vb" Inherits="HCVU.eHealthAccountDeathRecordFileImport"
    Title="<%$ Resources: Title, eHealthAccountDeathRecordFileImport %>" EnableEventValidation="False" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/Token/ucInputToken.ascx" TagName="ucInputToken" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <script type="text/javascript">
        function cboIPassword_Click() {
            if ($get('<%= cboIPassword.ClientID %>').checked) {
                $get('<%= txtIPassword.ClientID %>').disabled = false;
                $get('<%= txtIPassword.ClientID %>').className = "";
            } else {
                $get('<%= txtIPassword.ClientID %>').disabled = true;
                $get('<%= txtIPassword.ClientID %>').className = "TextBoxDisable";
                $get('<%= txtIPassword.ClientID %>').value = '';
            }
        }
        
        function SaveFilePathToHiddenField() {
            document.getElementById('<%= hfIFile.ClientID %>').value = document.getElementById('<%= flIFile.ClientID %>').value;
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, eHealthAccountDeathRecordFileImportBanner %>"
        AlternateText="<%$ Resources: AlternateText, eHealthAccountDeathRecordFileImportBanner %>">
    </asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="ibtnOImportFile" />
            <asp:PostBackTrigger ControlID="ibtnINext" />
            <asp:PostBackTrigger ControlID="ibtnCBack" />
        </Triggers>
        <ContentTemplate>
            <asp:HiddenField ID="hfIFile" runat="server" />
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="950px" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="950px" />
            <asp:MultiView ID="mvCore" runat="server" ActiveViewIndex="-1" OnActiveViewChanged="mvCore_ActiveViewChanged">
                <asp:View ID="vGrid" runat="server">
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
                                    <asp:ImageButton ID="ibtnGStatus" runat="server" Enabled="False" />
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
                    <asp:MultiView ID="mvImport" runat="server" ActiveViewIndex="-1" OnActiveViewChanged="mvImport_ActiveViewChanged">
                        <asp:View ID="vOuter" runat="server">
                            <table style="width: 100%">
                                <tr>
                                    <td style="text-align: center">
                                        <asp:ImageButton ID="ibtnOImportFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, ImportFileBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, ImportFileBtn %>" OnClick="ibtnOImportFile_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="vSelectFile" runat="server">
                            <table>
                                <tr>
                                    <td style="width: 400px">
                                        <asp:ImageButton ID="ibtnSCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnSCancel_Click" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ibtnSViewDetail" runat="server" ImageUrl="<%$ Resources: ImageUrl, ViewDetailsBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, ViewDetailsBtn %>" OnClick="ibtnSViewDetail_Click" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ibtnSRemoveFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, RemoveFileBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, RemoveFileBtn %>" OnClick="ibtnSRemoveFile_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                    </asp:MultiView>
                    <br />
                    <asp:Label ID="lblGNote" runat="server" Text="<%$ Resources: Text, DeathRecordFileImportNote %>"></asp:Label>
                    <br />
                </asp:View>
                <asp:View ID="vImport" runat="server">
                    <table>
                        <tr>
                            <div class="headingText">
                                <asp:Label ID="lblIImportFile" runat="server" Text="<%$ Resources: Text, ImportFile %>"></asp:Label>
                            </div>
                        </tr>
                        <tr style="height: 25px">
                            <td style="width: 200px; vertical-align: top">
                                <asp:Label ID="lblIDescriptionText" runat="server" Text="<%$ Resources: Text, Description %>">
                                </asp:Label>
                            </td>
                            <td style="vertical-align: top">
                                <asp:TextBox ID="txtIDescription" runat="server" Width="400px" MaxLength="100">
                                </asp:TextBox>
                                <asp:Image ID="imgErrorIDescription" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Visible="False" />
                            </td>
                        </tr>
                        <tr style="height: 25px">
                            <td style="vertical-align: top">
                                <asp:Label ID="lblIFileText" runat="server" Text="<%$ Resources: Text, File %>"></asp:Label>
                            </td>
                            <td style="vertical-align: top">
                                <asp:FileUpload ID="flIFile" runat="server" Width="460px" />
                                <asp:Image ID="imgErrorIFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" Visible="False" />
                            </td>
                        </tr>
                        <tr style="height: 25px">
                            <td style="vertical-align: top">
                                <asp:Label ID="lblIPasswordText" runat="server" Text="<%$ Resources: Text, PasswordToDecryptFile %>">
                                </asp:Label>
                            </td>
                            <td style="vertical-align: top">
                                <asp:CheckBox ID="cboIPassword" runat="server" onClick="cboIPassword_Click()" />
                                <asp:TextBox ID="txtIPassword" runat="server" Width="150px" TextMode="Password">
                                </asp:TextBox>
                            </td>
                        </tr>
                        <tr style="height: 30px">
                        </tr>
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnICancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnICancel_Click" />
                            </td>
                            <td style="text-align: center">
                                <asp:ImageButton ID="ibtnINext" runat="server" ImageUrl="<%$ Resources: ImageUrl, NextBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, NextBtn %>" OnClick="ibtnINext_Click"
                                    OnClientClick="SaveFilePathToHiddenField()" />
                            </td>
                        </tr>
                    </table>
                    <cc1:FilteredTextBoxExtender ID="FilteredIDescription" runat="server" TargetControlID="txtIDescription"
                        FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                    </cc1:FilteredTextBoxExtender>
                </asp:View>
                <asp:View ID="vConfirmImport" runat="server">
                    <table>
                        <tr>
                            <div class="headingText">
                                <asp:Label ID="lblCImportFile" runat="server" Text="<%$ Resources: Text, ImportFile %>"></asp:Label>
                            </div>
                        </tr>
                        <tr style="height: 25px">
                            <td style="width: 230px; vertical-align: top">
                                <asp:Label ID="lblCDescriptionText" runat="server" Text="<%$ Resources: Text, Description %>"></asp:Label>
                            </td>
                            <td style="width: 400px; vertical-align: top">
                                <asp:Label ID="lblCDescription" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 25px">
                            <td style="vertical-align: top">
                                <asp:Label ID="lblCFileNameText" runat="server" Text="<%$ Resources: Text, FileName %>"></asp:Label>
                            </td>
                            <td style="vertical-align: top">
                                <asp:Label ID="lblCFileName" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 25px">
                            <td style="vertical-align: top">
                                <asp:Label ID="lblCNoOfRecordText" runat="server" Text="<%$ Resources: Text, TotalNoOfRecordsInFile %>"></asp:Label>
                            </td>
                            <td style="vertical-align: top">
                                <asp:Label ID="lblCNoOfRecord" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 30px">
                        </tr>
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnCBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnCBack_Click" />
                            </td>
                            <td style="text-align: center">
                                <asp:ImageButton ID="ibtnCConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnCConfirm_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vFinish" runat="server">
                    <asp:Panel ID="panFError" runat="server">
                        <table>
                            <tr>
                                <td style="width: 160px">
                                    <asp:Label ID="lblFNoOfInvalidRecordText" runat="server" Text="<%$ Resources: Text, NoOfInvalidRecords %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblFNoOfInvalidRecord" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr style="height: 10px">
                            </tr>
                        </table>
                        <asp:Panel ID="panFG" runat="server" BorderWidth="1px" Width="757px" Height="300px"
                            ScrollBars="Vertical">
                            <asp:GridView ID="gvF" runat="server" Width="741px" AutoGenerateColumns="False" AllowPaging="False"
                                AllowSorting="False" EnableViewState="False" OnRowDataBound="gvF_RowDataBound"
                                OnPreRender="gvF_PreRender">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, RowNo %>" HeaderStyle-VerticalAlign="Top"
                                        HeaderStyle-HorizontalAlign="Center" SortExpression="Seq_No">
                                        <ItemStyle Width="70px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblGRowNo" runat="server" Text='<%# Eval("Seq_No") %>'></asp:Label>
                                            <asp:Image ID="imgGRowNo" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                                AlternateText="<%$ Resources: Text, DuplicateRecord %>" ToolTip="<%$ Resources: Text, DuplicateRecord %>"
                                                Visible="False" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, HKIdentityCardNo %>" HeaderStyle-VerticalAlign="Top"
                                        HeaderStyle-HorizontalAlign="Center" SortExpression="HKID">
                                        <ItemStyle Width="110px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblGHKID" runat="server" Text='<%# Eval("HKID") %>'></asp:Label>
                                            <asp:Image ID="imgGHKID" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                                AlternateText="<%$ Resources: Text, InvalidHKID %>" ToolTip="<%$ Resources: Text, InvalidHKID %>"
                                                Visible="False" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, DateOfDeath %>" HeaderStyle-VerticalAlign="Top"
                                        HeaderStyle-HorizontalAlign="Center" SortExpression="DOD">
                                        <ItemStyle Width="110px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblGDOD" runat="server" Text='<%# Eval("DOD") %>'></asp:Label>
                                            <asp:Image ID="imgGDOD" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                                AlternateText="<%$ Resources: Text, InvalidDateOfDeath %>" ToolTip="<%$ Resources: Text, InvalidDateOfDeath %>"
                                                Visible="False" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, DateOfRegistration %>" HeaderStyle-VerticalAlign="Top"
                                        HeaderStyle-HorizontalAlign="Center" SortExpression="DOR">
                                        <ItemStyle Width="110px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblGDOR" runat="server" Text='<%# Eval("DOR") %>'></asp:Label>
                                            <asp:Image ID="imgGDOR" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                                AlternateText="<%$ Resources: Text, InvalidDateOfRegistration %>" ToolTip="<%$ Resources: Text, InvalidDateOfRegistration %>"
                                                Visible="False" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Name %>" HeaderStyle-VerticalAlign="Top"
                                        HeaderStyle-HorizontalAlign="Center" SortExpression="English_Name">
                                        <ItemStyle VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblGEnglishName" runat="server" Text='<%# Eval("English_Name") %>'></asp:Label>
                                            <asp:Image ID="imgGEnglishName" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                                AlternateText="<%$ Resources: Text, InvalidName %>" ToolTip="<%$ Resources: Text, InvalidName %>"
                                                Visible="False" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </asp:Panel>
                        <asp:Label ID="lblGVFNote" runat="server"></asp:Label>
                    </asp:Panel>
                    <br />
                    <asp:ImageButton ID="ibtnFReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                        AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnFReturn_Click" />
                </asp:View>
            </asp:MultiView>
            <%-- Pop up for View Details --%>
            <asp:Button ID="btnHiddenSViewDetail" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupSViewDetail" runat="server" TargetControlID="btnHiddenSViewDetail"
                PopupControlID="panSViewDetail" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panSViewDetailHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panSViewDetail" runat="server" Style="display: none">
                <asp:Panel ID="panSViewDetailHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 820px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblSViewDetailHeader" runat="server" Text="<%$ Resources: Text, DetailsOfDeathRecordFile %>">
                                </asp:Label></td>
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
                                        <cc2:InfoMessageBox ID="udcSVDInfoMessageBox" runat="server" />
                                        <table style="width: 765px">
                                            <tr>
                                                <td style="width: 230px">
                                                    <asp:Label ID="lblSVDDeathRecordFileIDText" runat="server" Text="<%$ Resources: Text, DeathRecordFileID %>">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSVDDeathRecordFileID" runat="server" CssClass="tableText"></asp:Label>
                                                    <asp:Label ID="lblSVDDeathRecordFileIDFail" runat="server" Text="[(Import Failed)]"
                                                        CssClass="tableText" ForeColor="Red"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSVDTotalNoOfRecordText" runat="server" Text="<%$ Resources: Text, TotalNoOfRecordsInFile %>">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSVDTotalNoOfRecord" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSVDNoOfRecordWithHKIDText" runat="server" Text="<%$ Resources: Text, NoOfRecordsWithHKID %>">
                                                    </asp:Label>
                                                    <asp:Label ID="lblSVDNoOfFailRecordText" runat="server" Text="<%$ Resources: Text, NoOfFailedRecords %>">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSVDNoOfRecordWithHKID" runat="server" CssClass="tableText"></asp:Label>
                                                    <asp:Label ID="lblSVDNoOfFailRecord" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSVDNoOfRecordWithoutHKIDText" runat="server" Text="<%$ Resources: Text, NoOfRecordsWithoutHKID %>">
                                                    </asp:Label>
                                                    <asp:Label ID="lblSVDShowFailRecordOnlyText" runat="server" Text="<%$ Resources: Text, ShowFailedRecordsOnly %>">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblSVDNoOfRecordWithoutHKID" runat="server" CssClass="tableText"></asp:Label>
                                                    <asp:CheckBox ID="cboSVDShowFailRecordOnly" runat="server" AutoPostBack="True" OnCheckedChanged="cboSVDShowFailRecordOnly_CheckedChanged" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblSVDShowRecordWithRowNoInText" runat="server" Text="<%$ Resources: Text, ShowRecordsWithRowNoIn %>">
                                                    </asp:Label>
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlSVDShowRecordWithRowNoIn" runat="server" Width="130" AutoPostBack="True"
                                                        OnSelectedIndexChanged="ddlSVDShowRecordWithRowNoIn_SelectedIndexChanged">
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 765px">
                                            <tr>
                                                <td style="text-align: right">
                                                    <asp:CheckBox ID="cboSVDMaskDocumentNo" runat="server" Text="<%$ Resources: Text, MaskIdentityDocumentNo %>"
                                                        AutoPostBack="True" OnCheckedChanged="cboSVDMaskDocumentNo_CheckedChanged" />
                                                    <asp:HiddenField ID="hfSVDMaskDocumentNo" runat="server" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Panel ID="panSVDEntryStaging" runat="server" BorderWidth="1px" Height="300px"
                                                        ScrollBars="Vertical">
                                                        <asp:GridView ID="gvSVDEntryStaging" runat="server" Width="741px" AutoGenerateColumns="False"
                                                            AllowPaging="False" AllowSorting="False" EnableViewState="False" OnRowDataBound="gvSVDEntryStaging_RowDataBound"
                                                            OnPreRender="gvSVDEntryStaging_PreRender">
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
                                                    <asp:ImageButton ID="ibtnSVDClose" runat="server" ImageUrl="<%$ Resources: ImageUrl, CloseBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, CloseBtn %>" OnClick="ibtnSVDClose_Click" />
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
            <asp:Button ID="btnHiddenSRemoveFile" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupSRemoveFile" runat="server" TargetControlID="btnHiddenSRemoveFile"
                PopupControlID="panSRemoveFile" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panSRemoveFileHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panSRemoveFile" runat="server" Style="display: none">
                <asp:Panel ID="panSRemoveFileHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblSRemoveFileHeader" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>">
                                </asp:Label></td>
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
                                        <asp:Image ID="imgPopupSRemoveFile" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblPopupSRemoveFileText" runat="server" Font-Bold="True" Text="<%$ Resources: Text, ConfirmToRemoveFileQ %>">
                                        </asp:Label></td>
                                    <td style="width: 40px">
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnPopupSRemoveFileConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnPopupSRemoveFileConfirm_Click" />
                                        <asp:ImageButton ID="ibtnPopupSRemoveFileCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnPopupSRemoveFileCancel_Click" /></td>
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
