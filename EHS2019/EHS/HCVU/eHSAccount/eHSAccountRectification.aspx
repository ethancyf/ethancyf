<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="eHSAccountRectification.aspx.vb" Inherits="HCVU.eHSAccountRectification" 
    Title="<%$ Resources:Title, eHSAccountRectification %>" %>
    
<%@ Register Src="../UIControl/DocType/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc1" %>
<%@ Register Src="../UIControl/DocType/ucInputDocumentType.ascx" TagName="ucInputDocumentType"
    TagPrefix="uc2" %>
<%@ Register Src="../UIControl/DocTypeLegend.ascx" TagName="DocTypeLegend" TagPrefix="uc3" %>      
<%@ Register Src="../UIControl/ChooseCCCode.ascx" TagName="ChooseCCCode" TagPrefix="uc3" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>    
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, eHSAccountRectificationBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, eHSAccountRectificationBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
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
            <asp:Panel ID="panConfirmMsg" runat="server" Style="display: none">
                <asp:Panel ID="panConfirmMsgHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 400px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblMsgTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 400px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 40px; height: 42px" valign="middle">
                                        <asp:Image ID="imgMsg" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                    <td align="center" style="height: 42px">
                                        <asp:Label ID="lblConfirmCanel" runat="server" Font-Bold="True" Text="<%$ Resources:Text, CancelAmendment %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:ImageButton ID="ibtnDialogConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnDialogConfirm_Click" />
                                        <asp:ImageButton ID="ibtnDialogCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnDialogCancel_Click" /></td>
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
                &nbsp;
            </asp:Panel>
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
                            <asp:Panel ID="panDocTypeContent" runat="server" ScrollBars="vertical" Height="300px">                        
                                <uc3:DocTypeLegend ID="udcDocTypeLegend" runat="server" />
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
            <cc2:InfoMessageBox ID="udcInfoMsgBox" runat="server" Width="98%" />
            <cc2:MessageBox ID="udcMsgBox" runat="server" Width="98%" />
            
             <asp:Panel ID="panFilter" runat="server">     
                        <div class="headingText">
                            <asp:Label ID="lblSearchAcctListText" runat="server" Font-Bold="True" Text="<%$ Resources:Text, VoucherAccountRecord %>"></asp:Label></div>
                        <br />
                        <asp:MultiView ID="mveFilter" runat="server" ActiveViewIndex="0"> 
                         <asp:View ID="vFilterSPAccOnly" runat="server" >
                            <table width="100%"  >
                                <tr>
                                    <td class="tableText" style="width:200px" > 
                                        <asp:Label ID="lblSearchDocTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                                    <td style="width:400px">
                                         <asp:DropDownList ID="ddlSearchDocType" runat="server" />
                                        </td>
                                    <td></td>
                                </tr>
                                <tr> <td class="tableText" style="width:200px" > 
                                        Identity Document No. </td>
                                    <td style="width:400px">
                                    <asp:TextBox ID="txtIdentityNum" runat="server" MaxLength="20"></asp:TextBox>                                                     
                                            &nbsp;&nbsp;&nbsp;                                                                                     
                                        <asp:CheckBox ID="cboFilterSpecialAccount" runat="server" Checked="false" AutoPostBack="false" CssClass="tableText"
                                        Text="Show Special Account" />  
                                    </td>
                                    <td>                                                                                                                                                         
                                    </td>     
                                </tr>
                                <tr> <td class="tableText" > 
                                        <asp:Label ID="lblAcctStatusText" runat="server" Text="<%$ Resources:Text, AccountStatus %>"></asp:Label></td>
                                    <td>
                                        <asp:dropdownlist ID="ddlAcctStatus" runat="server">
                                        </asp:dropdownlist>
                                     </td>
                                    <td><asp:ImageButton ID="ibtnFilter" runat="server"  OnClick="ibtnFilter_GetRecords" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" AlternateText="<%$ Resources:AlternateText, SearchBtn %>" /></td>
                                    </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="vFilterAll" runat="server">
                            <table width="100%"  >
                               <tr> <td class="tableText" > 
                                         <asp:CheckBox ID="cboShowSpecialAccount" runat="server" Checked="false" AutoPostBack="True" CssClass="tableText"
                                        OnCheckedChanged="cboShowSpecialAccount_CheckedChanged" Text="Show Special Account" />                                                                                         
                                  </td>     
                                </tr>
                            </table>
                        </asp:View>
                        </asp:MultiView>
             </asp:Panel>
            <asp:MultiView ID="mveHSAccount" runat="server" ActiveViewIndex="0"> 
                <asp:View ID="vShowList" runat="server" >
                    <asp:Panel ID="panAccountList" runat="server">           
                      
                        

                        <asp:Panel ID="panAccList" runat="server">                  
                            <asp:GridView ID="gvAcctList" runat="server" AllowPaging="True" AllowSorting="True" 
                                BackColor="White" AutoGenerateColumns="False">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTempAcctRecordIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="20px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Doc_Display_code" HeaderText="<%$ Resources:Text, DocumentType %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDocType" runat="server" Text='<%# Eval("Doc_Display_code") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="60px" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="IdentityNum" HeaderText="<%$ Resources:Text, IdentityDocNo %>">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnIdentityNum" runat="server"></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                   <asp:TemplateField SortExpression="Voucher_Acc_ID" Visible="false" HeaderText="<%$ Resources:Text, AccountID %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccID" runat="server" ></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="180px" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Eng_name" HeaderText="<%$ Resources:Text, VRName %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblName" runat="server" Text='<%# Eval("Eng_name") %>'></asp:Label>
                                            <br></br>
                                            <asp:Label ID="lblCName" runat="server" Text='<%# Eval("Chi_Name") %>' Font-Names="HA_MingLiu"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="270px" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="DOB" HeaderText="<%$ Resources:Text, DOB %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDOB" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="160px" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Sex" HeaderText="<%$ Resources:Text, Gender %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSex" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="65px" HorizontalAlign="Center" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Date_Of_Issue" HeaderText="<%$ Resources:Text, DOILong %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblDateOfIssue" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="160px" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="source" HeaderText="Account Type">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecordType" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Voucher_Acc_ID" HeaderText="<%$ Resources: Text, AccountID_ReferenceNo %>" 
                                        HeaderStyle-VerticalAlign="Top" HeaderStyle-HorizontalAlign="Center" >
                                        <ItemStyle Width="130px" VerticalAlign="Top" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccountID" runat="server" Style="white-space: nowrap"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Account_status" HeaderText="<%$ Resources:Text, AccountStatus %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblAccStatus" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="80px" VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Last_Fail_Validate_Dtm" HeaderText="Last Failed Date">
                                        <ItemTemplate>
                                            <asp:Label ID="lblLastFailedDate" runat="server"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="160px" VerticalAlign="Top" />
                                    </asp:TemplateField>                        
                                  </Columns>
                                <HeaderStyle HorizontalAlign="Center" />
                            </asp:GridView>    
                        </asp:Panel>
                    </asp:Panel>                     
                </asp:View>
                <asp:View ID="vAccountDetail" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblPersonalParticularsText" runat="server" Text="<%$ Resources:Text, PersonalParticulars %>"
                            Font-Bold="True"></asp:Label></div>
                    <asp:Panel ID="panDocumentType" runat="server">
                        <table>   
                            <tr>
                                <td valign="top" style= "width: 220px">
                                    <asp:Label ID="lblDocumentTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                                <td valign="top">
                                    <asp:Label ID="lblDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                        </table> 
                    </asp:Panel>  
                    <uc1:ucReadOnlyDocumnetType ID="ucReadOnlyDocumnetType" runat="server"></uc1:ucReadOnlyDocumnetType>                                                     
                    <uc2:ucInputDocumentType ID="ucInputDocumentType" runat="server" /> 
                    <br />
                    <div class="headingText">
                        <asp:Label ID="lblAcctInfoText" runat="server" Text="<%$ Resources:Text, VoucherAccountInfo %>"
                            Font-Bold="True"></asp:Label></div>
                    <asp:PlaceHolder ID="phReadOnlyAccountInfo" runat="server"></asp:PlaceHolder>
                    <asp:TextBox ID="txtDocCode" runat="server" Style="display: none"></asp:TextBox>
                    <asp:Panel ID="pnlAccountActionBtn" runat="server">
                        <br />
                        <table width="100%">
                            <tr>
                                <td align="left" valign="top" style="width: 220px">
                                    <asp:ImageButton ID="btnBackAcctList" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="btnBackAcctList_Click"></asp:ImageButton></td>
                                <td align="left" valign="bottom" style="width: 739px">
                                    <table border="0" cellpadding="0" cellspacing="0" runat="server" id="tblShowDetailReadOnlyButtonGroup">
                                        <tr>
                                            <td style="width: 730px;" align="center" id="tblReadOnlyButton">
                                                <asp:ImageButton ID="btnEdit" runat="server" AlternateText="<%$ Resources: AlternateText, EditBtn %>"
                                                    ImageUrl="<%$ Resources: ImageURL, EditBtn %>" OnClick="btnEdit_Click"></asp:ImageButton>
                                                <asp:ImageButton ID="btnWithdrawAmendment" runat="server" AlternateText="<%$ Resources:AlternateText, WithdrawAmendmentBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, WithdrawAmendmentBtn %>" OnClick="btnWithdrawAmendment_Click">
                                                </asp:ImageButton>
                                                <asp:ImageButton ID="ibtnRemoveTempAccountByBO" runat="server" AlternateText="<%$ Resources:AlternateText, RemoveBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, RemoveBtn %>" OnClick="ibtnRemoveTempAccountByBO_Click" /></td>
                                        </tr>
                                    </table>
                                    <table id="Table1" border="0" cellpadding="0" cellspacing="0" runat="server">
                                        <tr>
                                            <td style="width: 730px; height: 33px;" align="center" id="tblShowDetailEditButtonGroup">
                                                <asp:ImageButton ID="btnSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="btnSave_Click"></asp:ImageButton>&nbsp;
                                                <asp:ImageButton ID="btnCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                    ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="btnCancel_Click"></asp:ImageButton></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="vConfirmAmendedAccount" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblConfirmAcctInfoText" runat="server" Font-Bold="True" Text="<%$ Resources:Text, ConfirmInfo %>"></asp:Label></div>
                    <uc1:ucReadOnlyDocumnetType ID="udcConfirmAccount" runat="server">
                    </uc1:ucReadOnlyDocumnetType>
                    <table>
                        <tr>
                            <td style="width: 220px" valign="top">
                                <asp:Label ID="lblConfirmReasonText" runat="server" CssClass="tableTitle"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblConfirmReason" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <table width="100%">
                        <tr>
                            <td align="left" style="width: 100px">
                                <asp:ImageButton ID="ibtnConfirmCancelAmendedAccont" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, BackBtn %>"  />
                            </td>        
                            <td align="center" style="width: 700px">
                               <asp:ImageButton ID="ibtnConfirmAmendedAccount" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"  />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vComplete" runat="server">
                    <br />
                    <table width="100%">
                        <tr>
                            <td>
                                <asp:ImageButton ID="ibtnReturn" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ReturnBtn %>" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
            <asp:Button Style="display: none" ID="btnHiddenCCCode" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderChooseCCCode" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenCCCode" PopupControlID="panChooseCCCode" RepositionMode="None"
                PopupDragHandleControlID="panChooseCCCodeHeading">
            </cc1:ModalPopupExtender>
            <asp:Button Style="display: none" ID="btnHiddenWithdraw" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderCancelAmend" runat="server" TargetControlID="btnHiddenWithdraw"
                PopupControlID="panConfirmMsg" BehaviorID="mdlPopup" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" CancelControlID="btnHiddenWithdraw" PopupDragHandleControlID="panConfirmMsgHeading">
            </cc1:ModalPopupExtender>
            <asp:Button runat="server" ID="btnHiddenDocTypeHelp" Style="display: none" />
            <cc1:ModalPopupExtender ID="popupDocTypeHelp" runat="server" TargetControlID="btnHiddenDocTypeHelp"
                PopupControlID="panDocTypeHelp" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDocTypeHelpHeading">
            </cc1:ModalPopupExtender>
        </ContentTemplate> 
    </asp:UpdatePanel>                    
</asp:Content>
