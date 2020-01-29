<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="CreateMessage.ascx.vb" Inherits="HCVU.CreateMessage" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/SentOutMessage/ucReadOnlyMessageDetails.ascx" TagName="ucReadOnlyMessageDetails" TagPrefix="uc1" %>

<asp:MultiView ID="mvCreateMessage" runat="server" ActiveViewIndex="0">
    <%-- View 0 - Select Template ----------------------------------------------------%>
    <asp:View ID="viewSelectTemplate" runat="server">
        <div class="headingText">
            <asp:Label ID="lblSelectTemplateText" runat="server" Text="<%$ Resources:Text, SelectTemplate %>" Font-Bold="true"></asp:Label>
        </div>
        <br />
        <asp:GridView ID="gvSelectTemplate" runat="server" AllowPaging="true" AllowSorting="true" AutoGenerateColumns="false"
                      BackColor="White" HeaderStyle-VerticalAlign="Top" OnPageIndexChanging="gvSelectTemplate_PageIndexChanging"
                      OnPreRender="gvSelectTemplate_PreRender" OnRowCommand="gvSelectTemplate_RowCommand" OnRowCreated="gvSelectTemplate_RowCreated"
                      OnRowDataBound="gvSelectTemplate_RowDataBound" OnSorting="gvSelectTemplate_Sorting" Width="100%">
            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
            <Columns>
                <asp:TemplateField HeaderText="<%$ Resources:Text, MessageTemplateID %>" SortExpression="IBMT_MsgTemplate_ID">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="90px" />
                    <ItemTemplate>
                        <asp:LinkButton ID="lbtnTemplateID" runat="server" CausesValidation="false" CommandName="" Text='<%# Eval("IBMT_MsgTemplate_ID") %>'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Text, Subject %>" SortExpression="IBMT_MsgTemplateSubject">
                    <ItemStyle VerticalAlign="Top" Width="500px" />
                    <ItemTemplate>
                        <asp:Label ID="lblSubject_v0" runat="server" Text='<%# Eval("IBMT_MsgTemplateSubject") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Text, Category %>" SortExpression="IBMT_MsgTemplateCategory">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="110px" />
                    <ItemTemplate>
                        <asp:Label ID="lblCategory_v0" runat="server" Text='<%# Eval("IBMT_MsgTemplateCategory") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="<%$ Resources:Text, MessageTemplateCreationDate %>" SortExpression="IBMT_Create_Dtm">
                    <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="100px" />
                    <ItemTemplate>
                        <asp:Label ID="lblTemplateCreationDate_v0" runat="server" Text='<%# Eval("IBMT_Create_Dtm") %>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
        <br />
    </asp:View>
    <%-- View 1 - Preview Template and Input Parameter ----------------------------------------------------%>
    <asp:View ID="viewInputParameter" runat="server">
        <%-- Template Heading ----------------------------------------------------%>
        <table cellpadding="3" cellspacing="0" style="border:1px solid #999999; border-collapse:collapse; width:100%">
            <%-- Template ID ----------------------------------------------------%>
            <tr valign="top">
                <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                    <asp:Label ID="lblTemplateIDText_v1" runat="server" Text="<%$ Resources:Text, MessageTemplateID %>" CssClass="tableTitle"></asp:Label>
                </td>
                <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                    <asp:Label ID="lblTemplateID_v1" runat="server" Text="" CssClass="tableText"></asp:Label>
                    &nbsp;
                    <asp:ImageButton ID="ibtnPreviewTemplate_v1" runat="server" AlternateText="<%$ Resources:AlternateText, PreviewTemplateBtn %>"
                                     ImageUrl="<%$ Resources:ImageUrl, PreviewTemplateBtn %>" ImageAlign="AbsBottom" Visible="true" OnClick="ibtnPreviewTemplate_v1_Click" />
                    <asp:ImageButton ID="ibtnHideTemplate_v1" runat="server" AlternateText="<%$ Resources:AlternateText, HideTemplateBtn %>"
                                     ImageUrl="<%$ Resources:ImageUrl, HideTemplateBtn %>" ImageAlign="AbsBottom" Visible="false" OnClick="ibtnHideTemplate_v1_Click" />
                </td>
            </tr>
            <%-- Category ----------------------------------------------------%>
            <tr valign="top">
                <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                    <asp:Label ID="lblCategoryText_v1" runat="server" Text="<%$ Resources:Text, Category %>" CssClass="tableTitle"></asp:Label>
                </td>
                <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                    <asp:Label ID="lblCategory_v1" runat="server" Text="" CssClass="tableText"></asp:Label>  
                </td>
            </tr>
            <%-- Recipient ----------------------------------------------------%>
            <tr valign="top">
                <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                    <asp:Label ID="lblRecipientText_v1" runat="server" Text="<%$ Resources:Text, Recipient %>" CssClass="tableTitle"></asp:Label>
                </td>
                <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                    <asp:ImageButton ID="ibtnAddRecipient_v1" runat="server" AlternateText="<%$ Resources:AlternateText, AddSBtn %>"
                                     ImageUrl="<%$ Resources:ImageUrl, AddSBtn %>" ImageAlign="AbsBottom" Visible="true" OnClick="ibtnAddRecipient_v1_Click" />
                    <asp:ImageButton ID="ibtnResetRecipient_v1" runat="server" AlternateText="<%$ Resources:AlternateText, ResetSBtn %>"
                                     ImageUrl="<%$ Resources:ImageUrl, ResetSBtn %>" ImageAlign="AbsBottom" Visible="false" OnClick="ibtnResetRecipient_v1_Click" />
                    <asp:Image ID="imgRecipientAlert_v1" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                               ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="AbsBottom" Visible="false" />
                    <table id="tblRecipient_v1" runat="server" border="0" cellpadding="0" cellspacing="0" visible="false">
                        <tr>
                            <td style="padding: 5px 0px 0px 0px">
                                <asp:Label ID="lblRecipient_v1" runat="server" Text="" CssClass="tableText" Visible="false"></asp:Label>
                                <asp:GridView ID="gvSelectRecipient" runat="server" AllowPaging="false" AllowSorting="false" AutoGenerateColumns="false" BackColor="White"
                                              HeaderStyle-VerticalAlign="Top" OnRowCommand="gvSelectRecipient_RowCommand" OnRowDataBound="gvSelectRecipient_RowDataBound">
                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                    <Columns>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, HealthProf %>">
                                            <HeaderStyle />
                                            <ItemStyle VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblHealthProfession_v1" runat="server" Text='<%# Eval("SOMR_Profession") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblHealthProfessionText_v1" runat="server" Text='<%# Eval("SOMR_Profession_DisplayText") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                            <HeaderStyle />
                                            <ItemStyle VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblScheme_v1" runat="server" Text='<%# Eval("SOMR_Scheme") %>' Visible="false"></asp:Label>
                                                <asp:Label ID="lblSchemeText_v1" runat="server" Text='<%# Eval("SOMR_Scheme_DisplayText") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ShowHeader="false">
                                            <HeaderStyle />
                                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:ImageButton ID="ibtnRemove_v1" runat="server" AlternateText="<%$ Resources:AlternateText, RemoveSBtn %>" CausesValidation="false"
                                                                 CommandName="" ImageAlign="AbsBottom" ImageUrl="<%$ Resources:ImageUrl, RemoveSBtn %>" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%-- Subject ----------------------------------------------------%>
            <tr valign="top">
                <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                    <asp:Label ID="lblSubject0Text_v1" runat="server" Text="<%$ Resources:Text, Subject %>" CssClass="tableTitle" Width="110px"></asp:Label>
                </td>
                <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                    <asp:Label ID="lblSubject0_v1" runat="server" Text="" CssClass="tableText" Width="690px"></asp:Label>
                </td>
            </tr>
        </table>
        <br /><br />
        <%-- Preview Template Content ----------------------------------------------------%>
        <asp:Panel ID="panPreviewTemplate" runat="server" ScrollBars="None" Visible="false" Width="100%">
            <div class="headingText">
                <asp:Label ID="lblPreviewTemplateText" runat="server" Text="<%$ Resources:Text, PreviewTemplate %>" Font-Bold="true"></asp:Label>
            </div>
            <br />
            <table cellpadding="3" cellspacing="0" style="border:1px solid #999999; border-collapse:collapse; width:100%">
                <tr valign="top">
                    <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                        <asp:Label ID="lblSubjectText_v1" runat="server" Text="<%$ Resources:Text, Subject %>" CssClass="tableTitle" Width="110px"></asp:Label>
                    </td>
                    <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                        <asp:Label ID="lblSubject_v1" runat="server" Text="" CssClass="tableText" Width="690px"></asp:Label>
                    </td>
                </tr>
            </table>
            <br />
            <table border="0" cellpadding="2" cellspacing="0" width="100%">
                <tr valign="top">
                    <td style="width:800px">
                        <asp:Label ID="lblContent_v1" runat="server" Text="" CssClass="tableText" Width="800px"></asp:Label>
                    </td>
                </tr>
            </table>
            <br /><br />
        </asp:Panel>
        <%-- Input Parameter ----------------------------------------------------%>
        <asp:Panel ID="panInputParameter" runat="server" ScrollBars="None" Visible="true" Width="100%">
            <div class="headingText">
                <asp:Label ID="lblInputParameterText" runat="server" Text="<%$ Resources:Text, InputParameter %>" Font-Bold="true"></asp:Label>
            </div>
            <br />
            <table cellpadding="3" cellspacing="0" style="border:1px solid #999999; border-collapse:collapse; width:100%">
                <tr valign="top">
                    <td style="border:1px solid #999999; border-collapse:collapse; width:110px">
                        <asp:Label ID="lblParameterText_v1" runat="server" Text="<%$ Resources:Text, Parameter %>" CssClass="tableTitle"></asp:Label>
                    </td>
                    <td style="border:1px solid #999999; border-collapse:collapse; width:690px">
                        <asp:PlaceHolder ID="phParam_v1" runat="server"></asp:PlaceHolder>
                    </td>
                </tr>
            </table>
            <br /><br />
        </asp:Panel>
        <%-- Button ----------------------------------------------------%>
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="ibtnCancel_v1" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                     ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnCancel_v1_Click" />
                </td>
                <td style="width:50px">
                </td>
                <td align="center" style="width:650px">
                    <asp:ImageButton ID="ibtnNext_v1" runat="server" AlternateText="<%$ Resources:AlternateText, NextBtn %>"
                                     ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" OnClick="ibtnNext_v1_Click" />
                </td>
            </tr>
        </table>
        <br />
    </asp:View>
    <%-- View 2 - Select Recipient ----------------------------------------------------%>
    <asp:View ID="viewSelectRecipient" runat="server">
    
        <%-- "Select Recipient" had been moved into "View 1 - Preview Template and Input Parameter" using Pop-up Window --%>
              
    </asp:View>
    <%-- View 3 - Confirm to Create Message ----------------------------------------------------%>
    <asp:View ID="viewConfirmCreateMessage" runat="server">
        <uc1:ucReadOnlyMessageDetails ID="ucReadOnlyMessageDetails" runat="server" />
        <br /><br />
        <%-- Button ----------------------------------------------------%>         
        <table>
            <tr>
                <td>
                    <asp:ImageButton ID="ibtnBack_v3" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                     ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnBack_v3_Click" />
                </td>
                <td style="width:50px">
                </td>
                <td align="center" style="width:650px">
                    <asp:ImageButton ID="ibtnConfirm_v3" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                     ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnConfirm_v3_Click" />
                </td>
            </tr>
        </table>
        <br />
    </asp:View>
    <%-- View 4 - Completed to Create Message ----------------------------------------------------%>
    <asp:View ID="viewCompletedCreateMessage" runat="server">
        <div class="headingText">
            <asp:Label ID="lblCompletedCreateMessage" runat="server" Text="<%$ Resources:Text, MessageCreationReference %>" Font-Bold="true"></asp:Label>
        </div>
        <br />
        <table border="0" cellpadding="2" cellspacing="0" width="100%">
            <tr valign="top">
                <td style="width:110px">
                    <asp:Label ID="lblMessageIDText_v4" runat="server" Text="<%$ Resources:Text, MessageID %>" CssClass="tableTitle"></asp:Label>
                </td>
                <td style="width:690px">
                    <asp:Label ID="lblMessageID_v4" runat="server" Text="" CssClass="tableText"></asp:Label>
                </td>
            </tr>
            <tr valign="top">
                <td style="width:110px">
                    <asp:Label ID="lblCreatedDateTimeText_v4" runat="server" Text="<%$ Resources:Text, CreateDtm %>" CssClass="tableTitle"></asp:Label>
                </td>
                <td style="width:690px">
                    <asp:Label ID="lblCreatedDateTime_v4" runat="server" Text="" CssClass="tableText"></asp:Label>
                </td>
            </tr>
        </table>
        <br />
    </asp:View>
</asp:MultiView>
<%-- Pop-up Window for "Add Recipient"  ----------------------------------------------------%>
<asp:Button ID="btnPopUpAddRecipientDummy" runat="server" Text="" Style="display:none" />
<asp:Panel ID="panPopUpAddRecipient" runat="server" Style="display:none">
    <table border="0" cellpadding="0" cellspacing="0">
        <%-- Pop-up Window Header ----------------------------------------------------%>
        <tr>
            <td colspan="3">
                <asp:Panel ID="panPopUpAddRecipientHeader" runat="server" Width="100%" Style="cursor:move">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="background-image:url(../Images/dialog/top-left.png); width:7px; height:35px">
                            </td>
                            <td style="font-weight:bold; font-size:14px; color:#ffffff; background-image:url(../Images/dialog/top-mid.png);
                                       background-repeat:repeat-x; height:35px">
                                <asp:Label ID="lblPopUpAddRecipientTitle" runat="server" Text="<%$ Resources:Text, AddRecipient %>"></asp:Label>
                            </td>
                            <td style="background-image:url(../Images/dialog/top-right.png); width:7px; height:35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <%-- Pop-up Window Content ----------------------------------------------------%>
        <tr>
            <td style="background-image:url(../Images/dialog/left.png); background-repeat:repeat-y; width:7px">
            </td>
            <td style="background-color:#ffffff">
                <cc2:MessageBox ID="udcMessageBoxPopUp" runat="server" Width="668px" />
                <table border="0" cellpadding="10" cellspacing="0">
                    <tr>
                        <td>
                            <%-- SelectRecipient ----------------------------------------------------%>
                            <asp:Panel ID="panSelectRecipient" runat="server" ScrollBars="None" Visible="true">
                                <table border="0" cellpadding="3" cellspacing="0">
                                    <tr valign="top">
                                        <td>
                                            <asp:RadioButton ID="rdoAllEnrolledSP_v2" runat="server" AutoPostBack="true" GroupName="grpSelectRecipient"
                                                             Text="<%$ Resources:Text, AllEnrolledSP %>" OnCheckedChanged="rdoAllEnrolledSP_v2_CheckedChanged" />
                                            &nbsp;
                                            <asp:Image ID="imgSelectRecipientAlert1_v2" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                       ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="AbsBottom" Visible="false" />
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td style="padding-left:20px">
                                            Or
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <%-- Select Recipient by Health Profession and/or Scheme ----------------------------------------------------%>
                            <asp:Panel ID="panSelectRecipientBy_P_S" runat="server" ScrollBars="None" Visible="true">
                                <table border="0" cellpadding="3" cellspacing="0">
                                    <tr valign="top">
                                        <td>
                                            <asp:RadioButton ID="rdoSelectRecipientBy_P_S_v2" runat="server" AutoPostBack="true" GroupName="grpSelectRecipient"
                                                             Text="<%$ Resources:Text, SelectRecipientBy_P_S %>" OnCheckedChanged="rdoSelectRecipientBy_P_S_v2_CheckedChanged" />
                                            &nbsp;
                                            <asp:Image ID="imgSelectRecipientAlert2_v2" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                       ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="AbsBottom" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr valign="top">
                                        <td style="width:30px">
                                        </td>
                                        <td>
                                            <%-- Control for Input of Recipient ----------------------------------------------------%>      
                                            <table cellpadding="4" cellspacing="0" class="tblMatchWithGridView" rules="all">
                                                <tr class="trMatchWithGridViewHeader">
                                                    <td style="width:360px">
                                                        <asp:Label ID="lblHealthProfession_v2" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
                                                    </td>
                                                    <td style="width:180px">
                                                        <asp:Label ID="lblScheme_v2" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr class="tdMatchWithGridViewItem">
                                                    <td style="width:390px">
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="padding-left:6px">
                                                                    <%--<asp:CheckBox ID="chkHealthProfession_v2" runat="server" Enabled="false" />--%>
                                                                </td>
                                                                <td style="padding-left:0px">
                                                                    <asp:DropDownList ID="ddlHealthProfession_v2" runat="server" Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="padding-left:5px; width:30px">
                                                                    <asp:Image ID="imgHealthProfessionAlert_v2" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                               ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="AbsBottom" Visible="false" />
                                                                </td>
                                                             </tr>
                                                        </table>
                                                    </td>
                                                    <td style="width:210px">
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="padding-left:6px">
                                                                    <asp:CheckBox ID="chkScheme_v2" runat="server" Enabled="false" />
                                                                </td>
                                                                <td style="padding-left:10px">
                                                                    <asp:DropDownList ID="ddlScheme_v2" runat="server" Enabled="false">
                                                                    </asp:DropDownList>
                                                                </td>
                                                                <td style="padding-left:5px; width:30px">
                                                                    <asp:Image ID="imgSchemeAlert_v2" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                                               ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="AbsBottom" Visible="false" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <br />
                            <table style="width:100%">
                                <tr>
                                    <td align="center">
                                        <asp:ImageButton ID="ibtnPopUpAddRecipient_Add" runat="server" AlternateText="<%$ Resources:AlternateText, AddBtn %>"
                                                         ImageUrl="<%$ Resources:ImageUrl, AddBtn %>" OnClick="ibtnPopUpAddRecipient_Add_Click" />
                                        <asp:ImageButton ID="ibtnPopUpAddRecipient_Cancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                         ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnPopUpAddRecipient_Cancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td style="background-image:url(../Images/dialog/right.png); background-repeat:repeat-y; width:7px">
            </td>
        </tr>
        <tr>
            <td style="background-image:url(../Images/dialog/bottom-left.png); width:7px; height:7px">
            </td>
            <td style="background-image:url(../Images/dialog/bottom-mid.png); background-repeat:repeat-x; height:7px">
            </td>
            <td style="background-image:url(../Images/dialog/bottom-right.png); width:7px; height:7px">
            </td>
        </tr>
    </table>
</asp:Panel>
<cc1:ModalPopupExtender ID="ModalPopupExtenderAddRecipient" runat="server" TargetControlID="btnPopUpAddRecipientDummy"
                        PopupControlID="panPopUpAddRecipient" BackgroundCssClass="modalBackgroundTransparent" DropShadow="false"
                        RepositionMode="None" PopupDragHandleControlID="panPopUpAddRecipientHeader">
</cc1:ModalPopupExtender>
<%-- End of Pop-up Window for "Add Recipient" ----------------------------------------------------%>
<%-- Pop-up Window for "Confirmation" of Adding "All Enrolled Service Providers" ----------------------------------------------------%>
<asp:Button ID="btnPopUpConfirmAddAllEnrolledSPDummy" runat="server" Text="" Style="display:none" />
<asp:Panel ID="panPopUpConfirmAddAllEnrolledSP" runat="server" Style="display:none">
    <table border="0" cellpadding="0" cellspacing="0">
        <%-- Pop-up Window Header ----------------------------------------------------%>
        <tr>
            <td colspan="3">
                <asp:Panel ID="panPopUpConfirmAddAllEnrolledSPHeader" runat="server" Width="100%" Style="cursor:move">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                        <tr>
                            <td style="background-image:url(../Images/dialog/top-left.png); width:7px; height:35px">
                            </td>
                            <td style="font-weight:bold; font-size:14px; color:#ffffff; background-image:url(../Images/dialog/top-mid.png);
                                       background-repeat:repeat-x; height:35px">
                                <asp:Label ID="lblPopUpConfirmAddAllEnrolledSPTitle" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label>
                            </td>
                            <td style="background-image:url(../Images/dialog/top-right.png); width:7px; height:35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </td>
        </tr>
        <%-- Pop-up Window Content ----------------------------------------------------%>
        <tr>
            <td style="background-image:url(../Images/dialog/left.png); background-repeat:repeat-y; width:7px">
            </td>
            <td style="background-color:#ffffff">
                <table border="0" cellpadding="10" cellspacing="0">
                    <tr>
                        <td>
                            <table style="width:100%">
                                <tr>
                                    <td style="height:42px; width:40px" valign="middle">
                                        <asp:Image ID="imgQuestionMarkIcon_p2" runat="server" ImageUrl="<%$ Resources:ImageUrl, QuestionMarkIcon %>" />
                                    </td>
                                    <td style="height:42px">
                                        <asp:Label ID="lblMsg_p2" runat="server" Text="<%$ Resources:Text, ConfirmAddAllEnrolledSP %>" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />
                            <table style="width:100%">
                                <tr>
                                    <td align="center">
                                        <asp:ImageButton ID="ibtnPopUpConfirmAddAllEnrolledSP_Confirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                         ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnPopUpConfirmAddAllEnrolledSP_Confirm_Click" />
                                        <asp:ImageButton ID="ibtnPopUpConfirmAddAllEnrolledSP_Cancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                         ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnPopUpConfirmAddAllEnrolledSP_Cancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>  
            </td>
            <td style="background-image:url(../Images/dialog/right.png); background-repeat:repeat-y; width:7px">
            </td>
        </tr>
        <tr>
            <td style="background-image:url(../Images/dialog/bottom-left.png); width:7px; height:7px">
            </td>
            <td style="background-image:url(../Images/dialog/bottom-mid.png); background-repeat:repeat-x; height:7px">
            </td>
            <td style="background-image:url(../Images/dialog/bottom-right.png); width:7px; height:7px">
            </td>
        </tr>
    </table>
</asp:Panel>
<cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmAddAllEnrolledSP" runat="server" TargetControlID="btnPopUpConfirmAddAllEnrolledSPDummy"
                        PopupControlID="panPopUpConfirmAddAllEnrolledSP" BackgroundCssClass="modalBackgroundTransparent" DropShadow="false"
                        RepositionMode="None" PopupDragHandleControlID="panPopUpConfirmAddAllEnrolledSPHeader">
</cc1:ModalPopupExtender>
<%-- End of Pop-up Window for "Confirmation" of Adding "All Enrolled Service Providers" ----------------------------------------------------%>
