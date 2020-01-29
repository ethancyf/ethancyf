<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="reimbursementGeneratePaymentFile.aspx.vb" Inherits="HCVU.reimbursementGeneratePaymentFile"
    Title="<%$ Resources:Title, ReimbursementGeneratePaymentFile %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="conMain" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="smScriptManager" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upnlMain" runat="server">
        <ContentTemplate>
            <asp:Image ID="imgReimSetCutoffDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReimbursementGeneratePaymentFileBanner %>"
                AlternateText="<%$ Resources:AlternateText, ReimbursementGeneratePaymentFileBanner %>">
            </asp:Image>
            <cc2:InfoMessageBox ID="udcInfoBox" runat="server" Visible="False" Width="945px">
            </cc2:InfoMessageBox>
            <asp:MultiView ID="mvCore" runat="server">
                <asp:View ID="vNoRecord" runat="server">
                </asp:View>
                <asp:View ID="vScheme" runat="server">
                    <div class="headingText">
                        <asp:Label ID="lblAuthHeading" runat="server" Text="<%$ Resources:Text, ReimbursementAuthorizationSummary %>">
                        </asp:Label>
                    </div>
                    <table>
                        <tr style="height: 8px"></tr>
                        <tr>
                            <td style="width: 150px">
                                <asp:Label ID="lblReimIDText" runat="server" Text="<%$ Resources:Text, ReimbursementID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblReimID" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                        <tr style="height: 10px"></tr>
                    </table>
                    <cc1:TabContainer ID="tcCore" runat="server" CssClass="m_ajax__tab_xp" ActiveTabIndex="0">
                        <cc1:TabPanel ID="tpRequire" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="lblRHeader" runat="server" Text="<%$ Resources: Text, PaymentFileRequired %>"></asp:Label>
                            </HeaderTemplate>
                            <ContentTemplate>
                                <div style="padding: 8px 0 8px 8px">
                                    <cc2:MessageBox ID="udcRErrorBox" runat="server" Visible="False" Width="945px"></cc2:MessageBox>
                                    <cc2:InfoMessageBox ID="udcRInfoBox" runat="server" Visible="False" Width="945px">
                                    </cc2:InfoMessageBox>
                                    <asp:MultiView ID="mvR" runat="server">
                                        <asp:View ID="vRNoRecord" runat="server"></asp:View>
                                        <asp:View ID="vRContent" runat="server">
                                            <asp:GridView ID="gvR" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                                AllowSorting="True" Width="945px" OnRowDataBound="gvR_RowDataBound" OnPreRender="gvR_PreRender"
                                                OnSorting="gvR_Sorting">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Display_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("Display_Code") %>'>
                                                            </asp:Label>
                                                            <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Hold_Dtm" HeaderText="<%$ Resources:Text, AuthorizationHoldTime %>">
                                                        <ItemStyle VerticalAlign="Top" Width="130px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHoldTime" runat="server" Text='<%# Eval("Hold_Dtm") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Hold_By" HeaderText="<%$ Resources:Text, AuthorizationHoldBy %>">
                                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHoldBy" runat="server" Text='<%# Eval("Hold_By") %>'>
                                                            </asp:Label>
                                                            <asp:HiddenField ID="hfHoldBy" runat="server" Value='<%# Eval("Hold_By") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="First_Authorised_Dtm" HeaderText="<%$ Resources:Text, FirstAuthorizedTime %>">
                                                        <ItemStyle VerticalAlign="Top" Width="130px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFirstAuthTime" runat="server" Text='<%# Eval("First_Authorised_Dtm") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="First_Authorised_By" HeaderText="<%$ Resources:Text, FirstAuthorizedBy %>">
                                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFirstAuthBy" runat="server" Text='<%# Eval("First_Authorised_By") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Second_Authorised_Dtm" HeaderText="<%$ Resources:Text, SecondAuthorizedTime %>">
                                                        <ItemStyle VerticalAlign="Top" Width="130px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSecondAuthTime" runat="server" Text='<%# Eval("Second_Authorised_Dtm") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Second_Authorised_By" HeaderText="<%$ Resources:Text, SecondAuthorizedBy %>">
                                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSecondAuthBy" runat="server" Text='<%# Eval("Second_Authorised_By") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <br />
                                            <asp:MultiView ID="mvRPaymentDate" runat="server">
                                                <asp:View ID="vRPEnter" runat="server">
                                                    <table style="width: 100%">
                                                        <tr style="height: 30px">
                                                            <td style="width: 20%">
                                                                <asp:Label ID="lblRPEPaymentDateText" runat="server" Text="<%$ Resources:Text, EnterBankPaymentDay %>">
                                                                </asp:Label>
                                                                **
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtRPEPaymentDate" runat="server" MaxLength="10" Width="70px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                                                    onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                                                    onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                                                <asp:ImageButton ID="ibtnRPEPaymentDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" />
                                                                <asp:Image ID="imgRPEPaymentDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
                                                                <cc1:CalendarExtender ID="calRPEPaymentDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnRPEPaymentDate"
                                                                    TargetControlID="txtRPEPaymentDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" />
                                                                <cc1:FilteredTextBoxExtender ID="filtereditPaymentDate" runat="server" FilterType="Custom, Numbers"
                                                                    TargetControlID="txtRPEPaymentDate" ValidChars="-" />
                                                                <asp:Label ID="lblRPEPaymentDate" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 10px">
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">**<asp:Label ID="lblRPEPaymentDateTips" runat="server" Text="<%$ Resources:Text, BankPaymentDayTips %>">
                                                            </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 10px">
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="text-align: center">
                                                                <asp:ImageButton ID="ibtnRPEGenerate" runat="server" ImageUrl="<%$ Resources:ImageUrl, GeneratePaymentFileBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, GeneratePaymentFileBtn %>" OnClick="ibtnRPEGenerate_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                                <asp:View ID="vRPConfirm" runat="server">
                                                    <table style="width: 100%">
                                                        <tr style="height: 30px">
                                                            <td style="width: 20%">
                                                                <asp:Label ID="lblRPCPaymentDateText" runat="server" Text="<%$ Resources:Text, BankPaymentDay %>">
                                                                </asp:Label>**
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblRPCPaymentDate" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 10px">
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">**<asp:Label ID="lblRPCPaymentDateTips" runat="server" Text="<%$ Resources:Text, BankPaymentDayTips %>">
                                                            </asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="text-align: left; padding-top: 30px; width: 5%">
                                                                            <asp:ImageButton ID="ibtnRPCBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnRPCBack_Click" />
                                                                        </td>
                                                                        <td style="text-align: center; padding-top: 30px">
                                                                            <asp:ImageButton ID="ibtnRPCConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                                                AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnRPCConfirm_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                            </asp:MultiView>
                                        </asp:View>
                                    </asp:MultiView>
                                </div>
                            </ContentTemplate>
                        </cc1:TabPanel>
                        <cc1:TabPanel ID="tpNoRequire" runat="server">
                            <HeaderTemplate>
                                <asp:Label ID="lblNHeader" runat="server" Text="<%$ Resources:Text, NoPaymentFileRequired %>"></asp:Label>
                            </HeaderTemplate>
                            <ContentTemplate>
                                <div style="padding: 8px 0 8px 8px">
                                    <cc2:MessageBox ID="udcNErrorBox" runat="server" Visible="False" Width="945px"></cc2:MessageBox>
                                    <cc2:InfoMessageBox ID="udcNInfoBox" runat="server" Visible="False" Width="945px">
                                    </cc2:InfoMessageBox>
                                    <asp:MultiView ID="mvN" runat="server">
                                        <asp:View ID="vNNoRecord" runat="server"></asp:View>
                                        <asp:View ID="vNContent" runat="server">
                                            <asp:GridView ID="gvN" runat="server" AutoGenerateColumns="False" AllowPaging="False"
                                                AllowSorting="True" Width="945px" OnRowDataBound="gvN_RowDataBound" OnPreRender="gvN_PreRender"
                                                OnSorting="gvN_Sorting">
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Display_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                                        <ItemStyle VerticalAlign="Top" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSchemeCode" runat="server" Text='<%# Eval("Display_Code") %>'>
                                                            </asp:Label>
                                                            <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Hold_Dtm" HeaderText="<%$ Resources:Text, AuthorizationHoldTime %>">
                                                        <ItemStyle VerticalAlign="Top" Width="130px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHoldTime" runat="server" Text='<%# Eval("Hold_Dtm") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Hold_By" HeaderText="<%$ Resources:Text, AuthorizationHoldBy %>">
                                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblHoldBy" runat="server" Text='<%# Eval("Hold_By") %>'>
                                                            </asp:Label>
                                                            <asp:HiddenField ID="hfHoldBy" runat="server" Value='<%# Eval("Hold_By") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="First_Authorised_Dtm" HeaderText="<%$ Resources:Text, FirstAuthorizedTime %>">
                                                        <ItemStyle VerticalAlign="Top" Width="130px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFirstAuthTime" runat="server" Text='<%# Eval("First_Authorised_Dtm") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="First_Authorised_By" HeaderText="<%$ Resources:Text, FirstAuthorizedBy %>">
                                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblFirstAuthBy" runat="server" Text='<%# Eval("First_Authorised_By") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Second_Authorised_Dtm" HeaderText="<%$ Resources:Text, SecondAuthorizedTime %>">
                                                        <ItemStyle VerticalAlign="Top" Width="130px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSecondAuthTime" runat="server" Text='<%# Eval("Second_Authorised_Dtm") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField SortExpression="Second_Authorised_By" HeaderText="<%$ Resources:Text, SecondAuthorizedBy %>">
                                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblSecondAuthBy" runat="server" Text='<%# Eval("Second_Authorised_By") %>'>
                                                            </asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                            <br />
                                            <asp:MultiView ID="mvNPaymentDate" runat="server">
                                                <asp:View ID="vNPEnter" runat="server">
                                                    <table style="width: 100%">
                                                        <tr style="height: 30px">
                                                            <td style="width: 20%">
                                                                <asp:Label ID="lblNPEPaymentDateText" runat="server" Text="<%$ Resources:Text, EnterBankPaymentDay %>">
                                                                </asp:Label>
                                                                #
                                                            </td>
                                                            <td>
                                                                <asp:TextBox ID="txtNPEPaymentDate" runat="server" MaxLength="10" Width="70px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                                                    onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                                                    onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                                                <asp:ImageButton ID="ibtnNPEPaymentDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" />
                                                                <asp:Image ID="imgNPEPaymentDate" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" />
                                                                <cc1:CalendarExtender ID="calNPEPaymentDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnNPEPaymentDate"
                                                                    TargetControlID="txtNPEPaymentDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" />
                                                                <asp:Label ID="lblNPEPaymentDate" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 10px">
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">#<asp:Label ID="lblNPEPaymentDateTips" runat="server" Text="<%$ Resources:Text, BankPaymentDayTipsNoPaymentFile %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 10px">
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="text-align: center">
                                                                <asp:ImageButton ID="ibtnNPECompleteReimbursement" runat="server" ImageUrl="<%$ Resources: ImageUrl, CompleteReimbursementBtn %>"
                                                                    AlternateText="<%$ Resources: AlternateText, CompleteReimbursementBtn %>" OnClick="ibtnNPECompleteReimbursement_Click" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                                <asp:View ID="vNPConfirm" runat="server">
                                                    <table style="width: 100%">
                                                        <tr style="height: 30px">
                                                            <td style="width: 20%">
                                                                <asp:Label ID="lblNPCPaymentDateText" runat="server" Text="<%$ Resources:Text, BankPaymentDay %>">
                                                                </asp:Label>**
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblNPCPaymentDate" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 10px">
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">**<asp:Label ID="lblNPCPaymentDateTips" runat="server" Text="<%$ Resources:Text, BankPaymentDayTipsNoPaymentFile %>">
                                                            </asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="text-align: left; padding-top: 30px; width: 5%">
                                                                            <asp:ImageButton ID="ibtnNPCBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                                                AlternateText="<%$ Resources:AlternateText, BackBtn %>" OnClick="ibtnNPCBack_Click" />
                                                                        </td>
                                                                        <td style="text-align: center; padding-top: 30px">
                                                                            <asp:ImageButton ID="ibtnNPCConfirm" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                                                                AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnNPCConfirm_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:View>
                                            </asp:MultiView>
                                        </asp:View>
                                    </asp:MultiView>
                                </div>
                            </ContentTemplate>
                        </cc1:TabPanel>
                    </cc1:TabContainer>
                </asp:View>
                <asp:View ID="vComplete" runat="server">
                    <table>
                        <tr>
                            <td style="width: 300px">
                                <asp:Label ID="lblNoPaymentFileGeneratedText" runat="server" Text='<%$ Resources:Text, NoOfPaymentFileSubmitted %>'></asp:Label>
                            </td>
                            <td>
                                <asp:Label ID="lblNoPaymentFileGenerated" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 20px">
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:ImageButton ID="ibtnReturn" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ReturnBtn %>" OnClick="ibtnReturn_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
