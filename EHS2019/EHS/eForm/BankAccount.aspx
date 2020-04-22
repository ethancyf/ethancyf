<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="BankAccount.aspx.vb" Inherits="eForm.BankAccount" Title="<%$ Resources:Title, eForm %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="JS/Common.js"></script>

    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, eFormSpEnrolmentBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, eFormSpEnrolmentBanner %>" />
    <table border="0" cellpadding="0" cellspacing="0" width="600">
        <tr>
            <td>
                <asp:Panel ID="panStep1" runat="server" CssClass="highlightTimeline">
                    <asp:Label ID="lblStep1" Text="<%$ Resources:Text, eFormStep1 %>" runat="server"></asp:Label></asp:Panel>
            </td>
            <td>
                <asp:Panel ID="panStep2" runat="server" CssClass="unhighlightTimeline">
                    <asp:Label ID="lblStep2" Text="<%$ Resources:Text, eFormStep2 %>" runat="server"></asp:Label></asp:Panel>
            </td>
            <td>
                <asp:Panel ID="panStep3" runat="server" CssClass="unhighlightTimeline">
                    <asp:Label ID="lblStep3" Text="<%$ Resources:Text, eFormStep3 %>" runat="server"></asp:Label></asp:Panel>
            </td>
            <%-- <td>
                <asp:Panel ID="panStep4" runat="server" CssClass="highlightTimeline">
                    <asp:Label ID="lblStep4" Text="<%$ Resources:Text, eFormStep4 %>" runat="server"></asp:Label></asp:Panel>
            </td>
            <td>
                <asp:Panel ID="panStep5" runat="server" CssClass="unhighlightTimeline">
                    <asp:Label ID="lblStep5" Text="<%$ Resources:Text, eFormStep5 %>" runat="server"></asp:Label></asp:Panel>
            </td>
            <td>
                <asp:Panel ID="panStep6" runat="server" CssClass="unhighlightTimeline">
                    <asp:Label ID="lblStep6" Text="<%$ Resources:Text, eFormStep6 %>" runat="server"></asp:Label></asp:Panel>
            </td>
            <td>
                <asp:Panel ID="panStep7" runat="server" CssClass="unhighlightTimeline">
                    <asp:Label ID="lblStep7" Text="<%$ Resources:Text, eFormStep7 %>" runat="server"></asp:Label></asp:Panel>
            </td>--%>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel ID="panCopyList" runat="server" Style="display: none;">
                <asp:Panel ID="panCopyListHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                        <tr>
                            <td style="background-image: url(Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblCopyListTitle" runat="server" Text="<%$ Resources:Text, CopyList %>"></asp:Label></td>
                            <td style="background-image: url(Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                    <tr>
                        <td style="background-image: url(Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <table cellspacing="4">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblCopyText" runat="server" Text="<%$ Resources:Text, CopyBankInfoFrom %>"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlPracticeList" runat="server" AutoPostBack="true" Width="200px" OnSelectedIndexChanged="ddlPracticeList_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:TextBox ID="txtPracticeIndex" runat="server" Style="display: none"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <table>
                                            <tr>
                                                <td style="width: 150px" valign="top">
                                                    <asp:Label ID="lblCopyBankNameText" runat="server" Text="<%$ Resources:Text, BankName %>"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="lblCopyBankName" runat="server" CssClass="TextChi"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px" valign="top">
                                                    <asp:Label ID="lblCopyBranchNameText" runat="server" Text="<%$ Resources:Text, BranchName %>"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="lblCopyBranchName" runat="server" CssClass="TextChi"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px" valign="top">
                                                    <asp:Label ID="lblCopyBankAccText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="lblCopyBankAcc" runat="server" CssClass="tableText"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 150px" valign="top">
                                                    <asp:Label ID="lblCopyBankOwnerText" runat="server" Text="<%$ Resources:Text, BankOwner %>"></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="lblCopyBankOwner" runat="server" CssClass="TextChi"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                <tr>
                                    <td align="center" colspan="3" valign="bottom">
                                        <asp:ImageButton ID="ibtnDialogConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnDialogConfirm_Click" />
                                        <asp:ImageButton ID="ibtnDialogCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnDialogCancel_Click" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(Images/dialog/bottom-left.png); width: 7px; height: 7px">
                        </td>
                        <td style="background-image: url(Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                            height: 7px">
                        </td>
                        <td style="background-image: url(Images/dialog/bottom-right.png); width: 7px; height: 7px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <cc2:MessageBox ID="udcMsgBox" runat="server" Width="95%" />
            <br />
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkBtnPersonal" Text="<%$ Resources:Text, PersonalParticulars %>"
                            runat="server" CssClass="unhighlightTab" Width="180px" Height="35px" onmouseover="this.className ='highlightTab'"
                            onmouseout="this.className='unhighlightTab'" OnClick="lnkBtnPersonal_Click"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkBtnMO" Text="<%$ Resources:Text, MedicalOrganizationInfo %>"
                            runat="server" CssClass="unhighlightTab" Width="180px" Height="35px" onmouseover="this.className ='highlightTab'"
                            onmouseout="this.className='unhighlightTab'" OnClick="lnkBtnMO_Click"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkBtnPractice" Text="<%$ Resources:Text, PracticeInfo %>" runat="server"
                            CssClass="unhighlightTab" Width="180px" Height="35px" onmouseover="this.className ='highlightTab'"
                            onmouseout="this.className='unhighlightTab'" OnClick="lnkBtnPractice_Click"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:Panel ID="panBankInfo" runat="server" CssClass="highlightTab" Width="180px"
                            Height="35px">
                            <asp:Label ID="lblTabBankInfo" Text="<%$ Resources:Text, BankInfo %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panScheme" runat="server" CssClass="unhighlightUnclickedTab" Width="180px"
                            Height="35px">
                            <asp:Label ID="lblTabScheme" Text="<%$ Resources:Text, SchemeInfo %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblRegBankSkipStatement" runat="server" Text="<%$ Resources:Text, RegBankSkipStatement %>"></asp:Label><br />
            <asp:GridView ID="gvPractice" runat="server" AutoGenerateColumns="False" Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeNo %>">
                        <ItemTemplate>
                            <asp:Label ID="lblPracticeIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            <asp:HiddenField ID="hfFormatedPracticeIndex" runat="server" Value='<%# Bind("PracticeIndex") %>' />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" Width="100px" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeInfo %>">
                        <ItemTemplate>
                            <table style="width: 100%">
                                <tr>
                                    <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                        <asp:Label ID="lblRegBankMONameText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblRegBankMOName" runat="server" CssClass="tableText" Text='<%# Bind("MOIndex") %>'></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                        <asp:Label ID="lblRegBankPracticeENameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblRegBankPracticeEName" runat="server" Text='<%# Bind("PracticeName") %>'
                                            CssClass="tableText"></asp:Label><br />
                                        <asp:Label ID="lblRegBankPracticeCName" runat="server" Text='<%# formatChineseString(Eval("PracticeNameChi")) %>'
                                            CssClass="TextChi"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                        <asp:Label ID="lblRegBankPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblRegBankPracticeAddress" runat="server" Text='<%# formatAddress(Eval("Room"), Eval("Floor"), Eval("Block"), Eval("Building"), Eval("District")) %>'
                                            CssClass="tableText"></asp:Label><br />
                                        <asp:Label ID="lblRegBankPracticeAddressChi" runat="server" Text='<%# formatChineseString(formatChiAddress(Eval("Room"), Eval("Floor"), Eval("Block"), Eval("ChiBuilding"), Eval("District"))) %>'
                                            CssClass="TextChi"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <hr style="width: 100%; color: #ff8080; height: 1px; border-width:thin;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        <asp:Label ID="lblRegBankTitle" runat="server" CssClass="tableText" Text="<%$ Resources:Text, BankInfo %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                        <asp:Label ID="lblRegBankNameText" runat="server" Text="<%$ Resources:Text, BankName %>"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtRegBankName" runat="server" Text='<%# Bind("Bank") %>' Width="260px"
                                            MaxLength="100" CssClass="TextBoxChi"></asp:TextBox>
                                        <asp:Image ID="imgBankNameAlert" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                            runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                        <asp:Label ID="lblRegBranchNameText" runat="server" Text="<%$ Resources:Text, BranchName %>"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtRegBranchName" runat="server" Text='<%# Bind("Branch") %>' Width="260px"
                                            MaxLength="100" CssClass="TextBoxChi"></asp:TextBox>
                                        <asp:Image ID="imgBranchNameAlert" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                            runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" /></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                        <asp:Label ID="lblRegBankAccText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                    <td>
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:TextBox ID="txtRegBankCode" runat="server" MaxLength="3" Text='<%# Bind("BankCode") %>'
                                                        Width="25px"></asp:TextBox></td>
                                                <td>
                                                    -</td>
                                                <td>
                                                    <asp:TextBox ID="txtRegBranchCode" runat="server" MaxLength="3" Text='<%# Bind("BranchCode") %>'
                                                        Width="25px"></asp:TextBox></td>
                                                <td>
                                                    -</td>
                                                <td>
                                                    <asp:TextBox ID="txtRegBankAcc" runat="server" MaxLength="9" Text='<%# Bind("BankAcc") %>'
                                                        Width="65px"></asp:TextBox></td>
                                                <td>
                                                    <asp:Image ID="imgBankAccAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                                        ImageUrl="~/Images/others/icon_caution.gif" Visible="False" /></td>
                                            </tr>
                                        </table>
                                        <asp:Label ID="lbl_bankAcc_tooltip" runat="server" Text="<%$ Resources:Text, BankAcctNoTip %>"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                        <asp:Label ID="lblRegBankOwnerText" runat="server" Text="<%$ Resources:Text, BankOwner %>"></asp:Label><br />
                                        (<asp:Label ID="lblRegEngNameText" runat="server" Text="<%$ Resources:Text, InEnglish %>"
                                            CssClass="tableTitle"></asp:Label>)</td>
                                    <td valign="top">
                                        <asp:Textbox ID="txtRegBankOwner" runat="server" Text='<%# Bind("Holder") %>' Width="520px" Height="80px"
                                            MaxLength="300" TextMode="MultiLine" style="overflow:hidden"></asp:Textbox>
                                        <asp:Image ID="imgBankOwnerAlert" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                            runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" Style="position: relative; top: -58px"/></td>
                                </tr>
                            </table>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" TargetControlID="txtRegBankCode"
                                FilterType="Custom, Numbers">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender2" runat="server" TargetControlID="txtRegBranchCode"
                                FilterType="Custom, Numbers">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender3" runat="server" TargetControlID="txtRegBankAcc"
                                FilterType="Custom, Numbers">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderRegBankOwner" runat="server" TargetControlID="txtRegBankOwner"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                            </cc1:FilteredTextBoxExtender>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="ibtnCopy" runat="server" AlternateText="<%$ Resources:AlternateText, CopyBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CopySBtn %>" OnClick="ibtnCopy_Click" />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" Width="80px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
            <table style="width: 100%">
                <tr>
                    <td align="center">
                        <asp:ImageButton ID="ibtnRegBankBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnRegBankBack_Click" />
                        <asp:ImageButton ID="ibtnRegBankNext" runat="server" AlternateText="<%$ Resources:AlternateText, NextBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" OnClick="ibtnRegBankNext_Click" />
                        <asp:ImageButton ID="ibtnRegBankSkip" runat="server" AlternateText="<%$ Resources:AlternateText, SkipBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, SkipBtn %>" OnClick="ibtnRegBankSkip_Click" /></td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblBankSkip" runat="server" Text="<%$ Resources:Text, SkipStatement  %>"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:Button runat="server" ID="btnHidden" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupCopyList" runat="server" TargetControlID="btnHidden"
                PopupControlID="panCopyList" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panCopyListHeading">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
