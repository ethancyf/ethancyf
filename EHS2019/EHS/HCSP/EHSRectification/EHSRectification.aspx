<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="EHSRectification.aspx.vb" Inherits="HCSP.EHSRectification" Title="<%$ Resources:Title, eHealthAcctRectification%>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" 
    TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" 
    TagPrefix="cc2" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" 
    TagPrefix="cc3" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" 
    TagPrefix="cc3" %>
<%@ Register Src="../UIControl/ucInputDocumentType.ascx" TagName="ucInputDocumentType"
    TagPrefix="uc1" %>
<%@ Register Src="../UIControl/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc2" %>
<%@ Register Src="../UIControl/ChooseCCCode.ascx" TagName="ChooseCCCode" 
    TagPrefix="uc3" %>
<%@ Register Src="../UIControl/SchemeLegend.ascx" TagName="SchemeLegend" 
    TagPrefix="uc4" %>
<%@ Register Src="../UIControl/DocTypeLegend.ascx" TagName="DocTypeLegend" 
    TagPrefix="uc5" %>
<%@ Register Src="../ClaimTranEnquiry.ascx" TagName="ClaimTranEnquiry" 
    TagPrefix="uc6" %>
<%@ Register Src="~/UIControl/IDEASCombo/IDEASCombo.ascx" TagName="IDEASCombo" 
    TagPrefix="uc7" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Image ID="imgHeader" runat="server" ImageAlign="AbsMiddle" AlternateText="<%$ Resources: AlternateText, VRRectification %>"
        ImageUrl="<%$ Resources: ImageURL, VRRectification %>" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <cc1:InfoMessageBox ID="udcInfoMsgBox" runat="server" Width="98%" />
            <cc1:MessageBox ID="udcMsgBoxErr" runat="server" Width="98%" />
            <uc7:IDEASCombo ID="ucIDEASCombo" runat="server" />
            <!-- Popup Panel CCCode -->
            <asp:Panel Style="display: none" ID="panChooseCCCode" runat="server">
                <asp:Panel ID="panChooseCCCodeHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Text, ChooseCCCodeHeading %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="center">
                            <uc3:ChooseCCCode ID="udcCCCode" runat="server"></uc3:ChooseCCCode>
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
            <!-- Popup Panel Confirm Message for removing account -->
            <asp:Panel ID="panRemoveAccMsg" runat="server" Style="display: none">
                <asp:Panel ID="panRemoveAccMsgHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblRemoveAccMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
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
                            <table style="width: 100%">
                                <tr>
                                    <td align="right" style="width: auto; height: 42px" valign="middle">
                                        <asp:Image ID="imgRemoveAccMsg" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="left" style="width: 95%; height: 42px">
                                        <asp:Label ID="lblRemoveAccMsg" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ConfirmVoidVRAcct %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ibtnRemoveAccCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnRemoveAccCancel_Click" />
                                        <asp:ImageButton ID="ibtnRemoveAccConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnRemoveAccConfirm_Click" /></td>
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
            <!-- Popup Panel Confirm Message for removing account -->
            <asp:Panel ID="panModifyMsg" runat="server" Style="display: none">
                <asp:Panel ID="panModifyMsgHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblModifyMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
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
                            <table style="width: 100%">
                                <tr>
                                    <td align="right" style="width: auto; height: 42px" valign="middle">
                                        <asp:Image ID="imglModifyMsg" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="left" style="width: 95%; height: 42px">
                                        <asp:Label ID="lblModifyMsg" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ModifyTempAccDisclaim %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ibtnModifyNo" runat="server" AlternateText="<%$ Resources:AlternateText, NoBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, NoBtn %>" OnClick="ibtnModifyNo_Click" />
                                        <asp:ImageButton ID="ibtnModifyYes" runat="server" AlternateText="<%$ Resources:AlternateText, YesBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, YesBtn %>" OnClick="ibtnModifyYes_Click" /></td>
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
            <!-- Popup Panel Claim declaration -->
            <asp:Panel ID="panClamDeclaration" runat="server" Style="display: none">
                <asp:Panel ID="panClamDeclarationHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblClamDeclarationTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
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
                            <table style="width: 100%">
                                <tr>
                                    <td align="right" style="width: auto; height: 42px" valign="top">
                                        <asp:Image ID="imgClamDeclaration" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="left" style="width: 95%; height: 42px; text-align: justify">
                                        <asp:Label ID="lblClamDeclaration" runat="server" Font-Bold="True" Text="<%$ Resources:Text, DeclaraChildOverSixYearsOldRectify %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ibtnClaimDeclarationCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnClaimDeclarationCancel_Click" />
                                        <asp:ImageButton ID="ibtnClaimDeclarationConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnClaimDeclarationConfirm_Click" /></td>
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
            <asp:MultiView ID="mvRectify" runat="server">
                <asp:View ID="vSearch" runat="server">
                    <table>
                        <tr>
                            <td colspan="3" style="height: 50px">
                                <asp:Label ID="lblAcctSearchList" runat="server" CssClass="tableCaption" Text="<%$ Resources: Text, SearchTempVRAcct %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px; height: 28px">
                                <asp:Label ID="lblAcctStatusText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, AccRectificationList %>"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:DropDownList ID="ddlAcctStatus" runat="server" Width="250px" AppendDataBoundItems="true">
                                    <asp:ListItem Selected="True" Value="" Text="<%$ Resources: Text, Any %>"></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px; height: 28px; vertical-align: top">
                                <asp:Label ID="lblCreationDateText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, CreationDate %>"></asp:Label>
                            </td>
                            <td colspan="2" style="vertical-align: top">
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSearchCreationDate" runat="server" MaxLength="10" Width="75px"
                                                onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                                                onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                                                onblur="filterDateInput(this);"></asp:TextBox>&nbsp;
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="btnSearchCreationDate" runat="server" AlternateText="Calender" ImageAlign="Middle" ImageUrl="~/Images/button/icon_button/btn_calender.png" />
                                            <cc2:CalendarExtender ID="calExtCreationDate" runat="server" CssClass="ajax_cal" PopupButtonID="btnSearchCreationDate" TargetControlID="txtSearchCreationDate" />
                                            <cc2:FilteredTextBoxExtender ID="filtereditSearchCreationDate" runat="server" FilterType="Custom, Numbers" TargetControlID="txtSearchCreationDate" ValidChars="-" />
                                        </td>
                                        <td>
                                            &nbsp;
                                            <asp:Image ID="imgSearchCreationDateError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
                                        </td>
                                    </tr>
                                </table>                                
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px; height: 50px">
                            </td>
                            <td colspan="2">
                                <asp:ImageButton ID="ibtnSearch" runat="server" AlternateText="<%$ Resources: AlternateText, SearchBtn %>"
                                    ImageUrl="<%$ Resources: ImageURL, SearchBtn %>" OnClick="ibtnSearch_Click" /></td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vResultList" runat="server">
                    <table>
                        <tr>
                            <td colspan="2">
                                <asp:Label ID="lblAcctList" runat="server" CssClass="tableCaption" Text="<%$ Resources: Text, eHealthAccountRecord %>"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px; height: 30px">
                                <asp:Label ID="lblDisplayStatusText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, AccRectificationList %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblDisplayStatus" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 250px; height: 30px">
                                <asp:Label ID="lblAcctListCreateDateText" runat="server" CssClass="tableTitle" Text="<%$ Resources: Text, CreationDate %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblAcctListCreateDate" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <asp:GridView ID="gvAcctList" runat="server" AllowPaging="True" AllowSorting="True"
                        Width="1500px" BackColor="White" AutoGenerateColumns="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblTempVRAcctRecordIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                <ItemStyle VerticalAlign="Middle" Width="20px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Scheme_Code" HeaderText="<%$ Resources:Text, Scheme %>"
                                Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblScheme" runat="server" Text='<%# Eval("Scheme_Code") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Doc_Display_code" HeaderText="<%$ Resources:Text, DocumentType %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDocType" runat="server" Text='<%# Eval("Doc_Display_code") %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="65px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="IdentityNum" HeaderText="<%$ Resources:Text, IdentityDocNo %>">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnIdentityNum" runat="server"></asp:LinkButton>
                                    <asp:HiddenField ID="hiddenAdoptionPrefixNum" Value='<%# Eval("Adoption_Prefix_Num") %>'
                                        runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="115px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Date_Of_Issue" HeaderText="<%$ Resources:Text, DOILong %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDateOfIssue" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="80px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Eng_Name" HeaderText="<%$ Resources:Text, Name %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("Eng_Name") %>'></asp:Label><br />
                                    <asp:Label ID="lblCName" runat="server" Text='<%# Eval("Chi_Name") %>' CssClass="TextChineseName" />
                                </ItemTemplate>
                                <ItemStyle Width="230px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="DOB" HeaderText="<%$ Resources:Text, DOB %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDOB" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="105px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Sex" HeaderText="<%$ Resources:Text, Gender %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblSex" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="45px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Display_Acc_ID" HeaderText="<%$ Resources:Text, RefNo %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblVRAcctID" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="120px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Transaction_ID" HeaderText="<%$ Resources:Text, TransactionNo %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblTransactionID" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="105px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Display_Status" HeaderText="<%$ Resources:Text, RecordStatus %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblRecordStatus" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="132px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Create_Dtm" HeaderText="<%$ Resources:Text, CreationDate %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblCreateDtm" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="120px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="Practice_Name" HeaderText="<%$ Resources:Text, PracticeName %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblPractice" runat="server"></asp:Label>
                                    <asp:Label ID="lblPracticeChi" runat="server" CssClass="textChi"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="160px" />
                            </asp:TemplateField>
                            <asp:TemplateField SortExpression="DataEntry_By" HeaderText="<%$ Resources:Text, DataEntry %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblDataEntryBy" runat="server"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle Width="100px" />
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle HorizontalAlign="Center" />
                    </asp:GridView>
                    <table>
                        <tr>
                            <td>
                                <asp:ImageButton ID="btnBackSearch" runat="server" AlternateText="<%$ Resources: AlternateText, BackBtn %>"
                                    ImageUrl="<%$ Resources: ImageURL, BackBtn %>" OnClick="btnBackSearch_Click" /></td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="RectifyAccount" runat="server">
                    <asp:Label ID="lblRectifyAcct" runat="server" CssClass="tableCaption" Text="<%$ Resources: Text, RectifyVRAcctInfo %>"></asp:Label>
                    <br />
                    <br />
                    <table border="0" cellpadding="0" cellspacing="0">
                        <tr>
                            <td style="width: 200px" valign="top" class="tableCellStyle">
                                <asp:Label ID="lblRectifyRefNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RefNo %>"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:Label ID="lblRectifyRefNo" runat="server" CssClass="tableText"></asp:Label><br />
                                <asp:Label ID="lblCreateByOtherSPText" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 200px" valign="top" class="tableCellStyle">
                                <asp:Label ID="lblRectifyDocTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DocumentType %>"></asp:Label>
                            </td>
                            <td valign="top">
                                <asp:Label ID="lblRectifyDocType" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <uc1:ucInputDocumentType ID="udcRectifyAccount" runat="server" Visible="true" />
                    <asp:Panel ID="pnlCreationMethod" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblRectifyCreationMethodText" runat="server" CssClass="tableTitle"
                                        Height="28px" Text="<%$ Resources:Text, CreationMethod %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRectifyCreationMethod" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlTransactionID" runat="server">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblRectifyTransactionIDText" runat="server" CssClass="tableTitle"
                                        Text="<%$ Resources:Text, TransactionNo %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblRectifyTransactionID" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table>
                        <tr>
                            <td style="width: 200px">
                                &nbsp;
                            </td>
                            <td>
                                <table cellpadding="1" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            <asp:ImageButton ID="ibtnRectifyAccountBack" runat="server" ImageUrl="<%$ Resources: ImageURL, BackBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnRectifyAccountBack_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnRectifyAccountSave" runat="server" ImageUrl="<%$ Resources: ImageURL, SaveBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, SaveBtn %>" OnClick="ibtnRectifyAccountSave_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnRectifyAccountModify" runat="server" ImageUrl="<%$ Resources:ImageURL, ModifyBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ModifyBtn %>" OnClick="ibtnRectifyAccountModify_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnRectifyAccountRemove" runat="server" AlternateText="<%$ Resources: AlternateText, RemoveBtn %>"
                                                ImageUrl="<%$ Resources: ImageURL, RemoveBtn %>" OnClick="ibtnRectifyAccountRemove_Click" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnRectifyAccountViewTransaction" runat="server" AlternateText="<%$ Resources: AlternateText, ViewTransactionBtn %>"
                                                ImageUrl="<%$ Resources: ImageURL, ViewTransactionBtn %>" OnClick="ibtnRectifyAccountViewTransaction_Click" />
                                            <cc3:CustomImageButton ID="ibtnManagement" runat="server" ImageUrl="<%$ Resources:ImageUrl, ViewTransactionBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ViewTransactionBtn %>"  ShowRedirectImage="false"/>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5">
                                        </td>
                                    </tr>
                                </table>
                                <table id="tblRectifyReadSmartIC" runat="server" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <table id="tblRectifyReadOldSmartIC" runat="server" cellpadding="0" cellspacing="0" style="width:260px;">
                                                <tr>
                                                    <td style="width: 130px;">
                                                        <asp:Image ID="imgRectifyReadOldSmartIC" runat="server" ImageUrl="<%$ Resources:ImageUrl, OldSmartIDChipFacingUpImg %>"/>
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:Label ID="lblRectifyOldSmartICChipFaceUp" runat="server" Font-Size="13px" style="color:#4d4d4d; font-weight: bold;" Text="<%$ Resources:Text, SmartIDChipFaceUp %>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table id="tblRectifyReadNewSmartIC" runat="server" cellpadding="0" cellspacing="0" style="width:250px;">
                                                <tr>
                                                    <td style="width: 130px;">
                                                        <asp:Image ID="imgRectifyReadNewSmartIC" runat="server" ImageUrl="<%$ Resources:ImageUrl, NewSmartIDChipFacingUpImg %>"/>
                                                    </td>
                                                    <td valign="middle">
                                                        <asp:Label ID="lblRectifyNewSmartICChipFaceUp" runat="server" Font-Size="13px" style="color:#4d4d4d; font-weight: bold;" Text="<%$ Resources:Text, SmartIDChipFaceUp %>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <table id="tblRectifyReadNewSmartICCombo" runat="server" cellpadding="0" cellspacing="0" style="width:250px;">
                                                <tr>
                                                    <td style="width: 130px;">
                                                        <asp:Image ID="imgRectifyReadNewSmartICCombo" runat="server" ImageUrl="<%$ Resources:ImageUrl, NewSmartIDComboChipFacingUpImg %>"/>
                                                    </td>
                                                    <td valign="middle" style="padding-left:8px">
                                                        <asp:Label ID="lblRectifyNewSmartICComboChipFaceUp" runat="server" Font-Size="13px" style="color:#4d4d4d; font-weight: bold;" Text="<%$ Resources:Text, SmartIDChipFaceUp %>"></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnRectifyReadSmartIDTips"
                                                runat="server" ImageAlign="AbsMiddle" ImageUrl="<%$ Resources: ImageUrl, ReadSmartIDTipsBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ReadSmartIDTipsBtn %>" />
                                        </td>
                                    </tr>
                                    <tr id="trSmartIDSoftwareNotInstalled">
                                        <td colspan="3" style="vertical-align:top;padding-bottom: 5px; vertical-align:middle">
                                            <asp:Label ID="lblSmartIDSoftwareNotInstalled1" runat="server" Font-Size="16px" style="color:#4d4d4d" Text="<%$ Resources:Text, SmartIDSoftwareNotInstalled1 %>"/>
                                            <asp:LinkButton ID ="lbtnSmartIDSoftwareNotInstalled2" runat="server" style="font-size:16px;color:rgba(31, 87, 255, 1)" Text ="<%$ Resources:Text, SmartIDSoftwareNotInstalled2 %>" />
                                            <asp:Label ID="lblSmartIDSoftwareNotInstalled3" runat="server" Font-Size="16px" style="color:#4d4d4d" Text="<%$ Resources:Text, SmartIDSoftwareNotInstalled3 %>"/>
                                        </td>    
                                    </tr>
                                    <tr>
                                       <td>
                                           <asp:ImageButton ID="ibtnRectifyReadOldSmartIC" runat="server" AlternateText="<%$ Resources: AlternateText, ReadOldSmartIDCardBtn %>"
                                                ImageUrl="<%$ Resources: ImageURL, ReadOldSmartIDCardBtn %>"/>
                                           <div ID="divReadOldSmartIDCardNA" runat="server" style="width:240px; text-align:center">                                                
                                                <asp:Label ID="lblReadOldSmartIDCardNA" runat="server" Text="<%$ Resources:Text, ReadCardAndSearchNA %>" CssClass="tableText"></asp:Label>
                                           </div>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnRectifyReadNewSmartIC" runat="server" AlternateText="<%$ Resources: AlternateText, ReadNewSmartIDCardBtn %>"
                                                ImageUrl="<%$ Resources: ImageURL, ReadNewSmartIDCardBtn %>"/>
                                            <div ID="divReadNewSmartIDCardNA" runat="server" style="width:240px; text-align:center">
                                                <asp:Label ID="lblReadNewSmartIDCardNA" runat="server" Text="<%$ Resources:Text, ReadCardAndSearchNA %>"
                                                CssClass="tableText"></asp:Label>
                                            </div>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnRectifyReadNewSmartICCombo" runat="server" AlternateText="<%$ Resources: AlternateText, ReadSmartIDBtn %>"
                                                ImageUrl="<%$ Resources: ImageURL, ReadSmartIDBtn %>"/>
                                            <div ID="divReadNewSmartIDComboCardNA" runat="server" style="width:240px; text-align:center">
                                                <asp:Label ID="lblReadNewSmartIDComboCardNA" runat="server" Text="<%$ Resources:Text, ReadCardAndSearchNA %>"
                                                CssClass="tableText"></asp:Label>
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <asp:Panel ID="panReminder" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0" width="680">
                            <tr>
                                <td style="background-image: url(../Images/others/reminder_topleft.png); width: 30px;
                                    background-repeat: no-repeat; height: 30px">
                                </td>
                                <td style="background-image: url(../Images/others/reminder_topmiddle.png); width: 30px;
                                    background-repeat: repeat-x; height: 30px">
                                </td>
                                <td style="background-image: url(../Images/others/reminder_topright.png); width: 30px;
                                    background-repeat: no-repeat; height: 30px">
                                </td>
                            </tr>
                            <tr>
                                <td style="background-image: url(../Images/others/reminder_left.png); width: 30px;
                                    background-repeat: repeat-y">
                                </td>
                                <td style="background-color: #f9f9f9; width: 620px; line-height: 20px; text-align: justify">
                                    <asp:Label ID="lblReminder" runat="server" CssClass="tableText"></asp:Label></td>
                                <td style="background-image: url(../Images/others/reminder_right.png); width: 30px;
                                    background-repeat: repeat-y">
                                </td>
                            </tr>
                            <tr>
                                <td style="background-image: url(../Images/others/reminder_bottomleft.png); width: 30px;
                                    background-repeat: no-repeat; height: 30px">
                                </td>
                                <td style="background-image: url(../Images/others/reminder_bottommiddle.png); width: 30px;
                                    background-repeat: repeat-x; height: 30px">
                                </td>
                                <td style="background-image: url(../Images/others/reminder_bottomright.png); width: 30px;
                                    background-repeat: no-repeat; height: 30px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="vConfirmAccount" runat="server">
                    <asp:Label ID="lblConfirmRectify" runat="server" CssClass="tableCaption" Text="<%$ Resources: Text, ConfirmRectifyInfo %>"></asp:Label>
                    <br />
                    <br />
                    <uc2:ucReadOnlyDocumnetType ID="udcConfirmAccount" runat="server" />
                    <asp:Panel ID="pnlCreateAccDeclaration" runat="server">
                        <table>
                            <tr>
                                <td style="width: 190px">
                                </td>
                                <td class="checkboxStyle" style="width: 600px">
                                    <asp:CheckBox ID="chkDeclaration" runat="server" AutoPostBack="true" Text="<%$ Resources: Text, ProvidedInfoTrueVAForm %>" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table>
                        <tr>
                            <td style="width: 200px">
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnConfirmBack" runat="server" ImageUrl="<%$ Resources: ImageURL, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnConfirmBack_Click" />
                                <asp:ImageButton ID="ibtnConfirm" runat="server" AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>"
                                    ImageUrl="<%$ Resources: ImageURL, ConfirmBtn %>" OnClick="ibtnConfirm_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vComplete" runat="server">
                    <table>
                        <asp:Panel ID="panCompleteVoidTrans" runat="server" Visible="False">
                            <tr style="height: 28px">
                                <td style="width: 185px">
                                    <asp:Label ID="lblRejectDateText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, VoidTranTime %>'></asp:Label>
                                    <td>
                                        <asp:Label ID="lblRejectDate" runat="server" CssClass="tableText"></asp:Label>
                                        <tr style="height: 28px">
                                            <td style="width: 185px">
                                                <asp:Label ID="lblRejectReferenceNoText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, VoidTranID %>'></asp:Label>
                                                <td>
                                                    <asp:Label ID="lblRejectReferenceNo" runat="server" CssClass="tableText" ForeColor="Blue"></asp:Label>
                        </asp:Panel>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="ibtnCompleteReturn" runat="server" AlternateText="<%$ Resources: AlternateText, ReturnBtn %>"
                                    ImageUrl="<%$ Resources: ImageURL, ReturnBtn %>" OnClick="ibtnCompleteReturn_Click" /></td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                            <td>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vPracticeSelection" runat="server">
                    <asp:Label ID="lblPracticeSelectionText" runat="server" Text="<%$ Resources:Text, SelectedPracticeText %>"
                        CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblPracticeSelection" runat="server" CssClass="tableText"></asp:Label>
                    <br />
                    <br />
                    <cc3:PracticeRadioButtonGroup runat="server" ID="PracticeRadioButtonGroup" HeaderText="<%$ Resources:Text, SelectPractice%>"
                        HeaderTextCss="tableText" PracticeRadioButtonCss="tableText" PracticeTextCss="tableText"
                        SchemeLabelCss="tableTitle" SelectButtonURL="~/Images/button/icon_button/btn_Arrow_to_Right.png"
                        MaskBankAccountNo="True" ShowCloseButton="False"/>
                </asp:View>
                <asp:View ID="vTransaction" runat="server">
                    <uc6:ClaimTranEnquiry ID="udcClaimTran" runat="server" />
                    <asp:Panel ID="pnlVoidTran" runat="server">
                        <table cellpadding="0">
                            <tr>
                                <td style="width: 150px">
                                    <asp:Label ID="lblVoidReasonText" CssClass="tableTitle" runat="server" Text="<%$ Resources:Text, VoidReason %>"></asp:Label>
                                </td>
                                <td>
                                    <asp:TextBox ID="txtVoidReason" runat="server" Width="460px" MaxLength="255" CssClass="textChi"></asp:TextBox>
                                    <asp:Image ID="imgVoidReasonErr" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                        Visible="false" />
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td style="width: 160px">
                                </td>
                                <td>
                                    <asp:ImageButton ID="ibtnVoidTranBack" runat="server" AlternateText="<%$ Resources: AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources: ImageURL, CancelBtn %>" OnClick="ibtnVoidTranBack_Click" />
                                    <asp:ImageButton ID="ibtnVoidTranConfirm" runat="server" AlternateText="<%$ Resources: AlternateText, ConfirmVoidBtn %>"
                                        ImageUrl="<%$ Resources: ImageURL, ConfirmVoidBtn %>" OnClick="ibtnVoidTranConfirm_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <asp:Panel ID="pnlTranActionBtn" runat="server">
                        <table>
                            <tr>
                                <td style="width: 160px">
                                </td>
                                <td>
                                    <asp:ImageButton ID="ibtnTranBack" runat="server" AlternateText="<%$ Resources: AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources: ImageURL, BackBtn %>" OnClick="ibtnTranBack_Click" />
                                    <asp:ImageButton ID="ibtnTranVoid" runat="server" AlternateText="<%$ Resources: AlternateText, VoidBtn %>"
                                        ImageUrl="<%$ Resources: ImageURL, VoidBtn %>" OnClick="ibtnTranVoid_Click" /></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="vSmartIDConfirmation" runat="server">
                    <asp:Label ID="lblSmartIDConfirmationConfirmText" runat="server" CssClass="tableCaption"
                        Text="<%$ Resources:Text, ConfirmRectifyInfo %>"></asp:Label><br />
                    <br />
                    <uc1:ucInputDocumentType ID="udcSmartIDConfirmationInputDocumentType" runat="server" />
                    <br />
                    <asp:Panel ID="panSmartIDConfirmationConsent" runat="server">
                        <table>
                            <tr>
                                <td style="width: 200px">
                                </td>
                                <td class="checkboxStyle" style="width: 600px">
                                    <asp:CheckBox ID="chkSmartIDConfirmationConsent" runat="server" AutoPostBack="true"
                                        Text="<%$ Resources: Text, ProvidedInfoTrueVAForm %>" />
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table>
                        <tr>
                            <td style="width: 200px">
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnSmartIDConfirmationBack" runat="server" AlternateText="<%$ Resources: AlternateText, BackBtn %>"
                                    ImageUrl="<%$ Resources: ImageURL, BackBtn %>" />
                                <asp:ImageButton ID="ibtnSmartIDConfirmationConfirm" runat="server" AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>"
                                    ImageUrl="<%$ Resources: ImageURL, ConfirmBtn %>" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
            <asp:Button Style="display: none" ID="btnHiddenCCCode" runat="server"></asp:Button>
            <cc2:ModalPopupExtender ID="ModalPopupExtenderChooseCCCode" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenCCCode" PopupControlID="panChooseCCCode" RepositionMode="None"
                PopupDragHandleControlID="panChooseCCCodeHeading">
            </cc2:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnHiddenRemoveAcc" runat="server" />
            <cc2:ModalPopupExtender ID="ModalPopupExtenderRemoveAcc" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenRemoveAcc" PopupControlID="panRemoveAccMsg" RepositionMode="None"
                PopupDragHandleControlID="panRemoveAccMsgHeading">
            </cc2:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnHiddenModify" runat="server" />
            <cc2:ModalPopupExtender ID="ModalPopupExtenderModify" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenModify" PopupControlID="panModifyMsg" RepositionMode="None"
                PopupDragHandleControlID="panModifyMsgHeading">
            </cc2:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnHiddenClaimDeclaration" runat="server" />
            <cc2:ModalPopupExtender ID="ModalPopupExtenderClaimDEclaration" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenClaimDeclaration" PopupControlID="panClamDeclaration"
                RepositionMode="None" PopupDragHandleControlID="panClamDeclarationHeading">
            </cc2:ModalPopupExtender>
            <asp:TextBox ID="txtDocCode" runat="server" Style="display: none"></asp:TextBox>
            <%-- Popup for Scheme Name Help --%>
            <asp:Panel ID="panSchemeNameHelp" runat="server" Style="display: none;">
                <asp:Panel ID="panSchemeNameHelpHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblSchemeNameHelpHeading" runat="server" Text="<%$ Resources:Text, Legend %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panSchemeNameHelpContent" runat="server" ScrollBars="vertical" Height="330px">
                                <uc4:SchemeLegend ID="udcSchemeLegend" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="ibtnCloseSchemeNameHelp" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseSchemeNameHelp_Click" /></td>
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
            <asp:Button runat="server" ID="btnHiddenSchemeNameHelp" Style="display: none" />
            <cc2:ModalPopupExtender ID="popupSchemeNameHelp" runat="server" TargetControlID="btnHiddenSchemeNameHelp"
                PopupControlID="panSchemeNameHelp" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panSchemeNameHelpHeading">
            </cc2:ModalPopupExtender>
            <%-- End of Popup for Scheme Name Help --%>
            <%-- Popup for DocType Help --%>
            <asp:Panel ID="panDocTypeHelp" runat="server" Style="display: none;">
                <asp:Panel ID="panDocTypeHelpHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDocTypeHelpHeading" runat="server" Text="<%$ Resources:Text, Legend %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panDocTypeContent" runat="server" ScrollBars="vertical" Height="290px">
                                <uc5:DocTypeLegend ID="udcDocTypeLegend" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="ibtnCloseDocTypeHelp" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseDocTypeHelp_Click" /></td>
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
            <asp:Button runat="server" ID="btnHiddenDocTypeHelp" Style="display: none" />
            <cc2:ModalPopupExtender ID="popupDocTypeHelp" runat="server" TargetControlID="btnHiddenDocTypeHelp"
                PopupControlID="panDocTypeHelp" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDocTypeHelpHeading">
            </cc2:ModalPopupExtender>
            <%-- End of Popup for DocType --%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
