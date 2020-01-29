<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="spAccountChangeConfirmation.aspx.vb" Inherits="HCVU.sp_AccountChangeConfirmation"
    Title="<%$ Resources:Title, SPAccountChangeConfirmation %>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <script type="text/javascript">
        function SelectAllCheckboxes(chkbox) {
            var elm = chkbox.form.elements;
            for (i = 0; i < elm.length; i++)
                if (elm[i].type == "checkbox" && elm[i].id != chkbox.id) {
                    if (elm[i].checked != chkbox.checked) elm[i].click();
                }
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Image ID="img_header" runat="server" AlternateText='<%$ Resources:AlternateText, AccountChangeMaintenanceBanner %>'
        ImageAlign="AbsMiddle" ImageUrl='<%$ Resources:ImageUrl, SPAccountChangeConfirmationBanner %>' />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="780px" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="780px" />
            <asp:Panel ID="pnlAccountChangeConfirmation" runat="server">
                <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                    Height="0px" Width="0px" OnClientClick="return false;" />
                <asp:MultiView ID="MultiViewAccountChangeConfirmation" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewSearchCriteria" runat="server">
                        <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                            <tr style="height: 40px">
                                <td colspan="2">
                                    <div class="headingText">
                                        <asp:Label ID="lblSearchChangeRecord" runat="server" Text='<%$ Resources:Text, SearchAccountChangeRecord %>'></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 185px">
                                    <asp:Label ID="lblActionText" runat="server" Text='<%$ Resources:Text, Action %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlChangeAccount" runat="server" AppendDataBoundItems="True">
                                        <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr style="height: 16px">
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl='<%$ Resources:ImageUrl, SearchBtn %>'
                                        AlternateText='<%$ Resources:AlternateText, SearchBtn %>' OnClick="ibtnSearch_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewResultLevelOne" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td>
                                    <asp:GridView ID="gvRecordLevelOne" runat="server" AllowPaging="True" AllowSorting="True"
                                        Width="950px" BackColor="White" AutoGenerateColumns="False" OnRowDataBound="gvRecordLevelOne_RowDataBound"
                                        OnPageIndexChanging="gvRecordLevelOne_PageIndexChanging" OnSorting="gvRecordLevelOne_Sorting"
                                        OnPreRender="gvRecordLevelOne_PreRender">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblChangeRecordIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="15px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="15px" />
                                                <HeaderTemplate>
                                                    <asp:CheckBox ID="chkSelectAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server" />
                                                </HeaderTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="SP_ID" HeaderText="<%$ Resources:Text, SPID %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSPID" runat="server" Text='<%# Eval("SP_ID") %>'></asp:Label>
                                                    <asp:HiddenField ID="hfPracticeDisplaySeq" runat="server" Value='<%# Eval("SP_Practice_Display_Seq") %>' />
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="70px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="SP_Eng_name" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSP_Eng_Name" runat="server" Text='<%# Eval("SP_Eng_Name") %>'></asp:Label><br />
                                                    <asp:Label ID="lblSP_Chi_Name" runat="server" Text='<%# Eval("SP_Chi_Name") %>' CssClass="TextGridChi"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="270px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ActionDescription" HeaderText="<%$ Resources:Text, Action %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAction" runat="server" Text='<%# Eval("ActionDescription") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Information" HeaderText="<%$ Resources:Text, Information %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInformation" runat="server"></asp:Label>
                                                    <asp:Image ID="imgIsShareToken" runat="server" ImageUrl="<%$ Resources:ImageUrl, ShareTokenBtn %>" AlternateText="<%$ Resources:AlternateText, ShareToken %>" />
                                                    <br />
                                                    <asp:Label ID="lblInformationTokenReplacement" runat="server" Visible="false"></asp:Label>
                                                    <asp:Image ID="imgIsShareTokenReplacement" runat="server" ImageUrl="<%$ Resources:ImageUrl, ShareTokenBtn %>" AlternateText="<%$ Resources:AlternateText, ShareToken %>" Visible="false" />
                                                    <asp:HiddenField ID="hfSchemeCode" runat="server" Value='<%# Eval("Scheme_Code") %>'></asp:HiddenField>
                                                    <asp:HiddenField ID="hfProject" runat="server" Value='<%# Eval("Project") %>' />
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Update_by" HeaderText="<%$ Resources:Text, Update_by %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUpdate_by" runat="server" Text='<%# Eval("Update_by") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Remark" HeaderText="<%$ Resources:Text, Remarks %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="System_Dtm" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSystem_Dtm" runat="server" Text='<%# Eval("System_Dtm") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Upd_Type" Visible="False">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUpd_Type" runat="server" Text='<%# Eval("Upd_Type") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <table width="950px">
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 10%">
                                    <asp:ImageButton ID="ibtnLevelOneBack" runat="server" ImageUrl='<%$ Resources:ImageUrl, BackBtn %>'
                                        AlternateText='<%$ Resources:AlternateText, BackBtn %>' OnClick="ibtnLevelOneBack_Click" />
                                </td>
                                <td align="center">
                                    <asp:ImageButton ID="ibtnLevelOneConfirm" runat="server" ImageUrl='<%$ Resources:ImageUrl, ConfirmSelectedBtn %>'
                                        AlternateText='<%$ Resources:AlternateText, ConfirmSelectedBtn %>' OnClick="ibtnLevelOneConfirm_Click" />
                                    <asp:ImageButton ID="ibtnLevelOneReject" runat="server" ImageUrl='<%$ Resources:ImageUrl, RejectSelectedBtn %>'
                                        AlternateText='<%$ Resources:AlternateText, RejectSelectedBtn %>' OnClick="ibtnLevelOneReject_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewResultLevelTwo" runat="server">
                        <table>
                            <tr style="height: 40px">
                                <td>
                                    <div class="headingText">
                                        <asp:Label ID="lblConfirm" runat="server" CssClass="tableText" Text="Confirm Change / Reject Change (to be controlled code-behind)"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:GridView ID="gvRecordLevelTwo" runat="server" AllowPaging="False" AllowSorting="True"
                                        Width="950px" BackColor="White" AutoGenerateColumns="False" OnPageIndexChanging="gvRecordLevelTwo_PageIndexChanging"
                                        OnSorting="gvRecordLevelTwo_Sorting" OnRowDataBound="gvRecordLevelTwo_RowDataBound"
                                        OnPreRender="gvRecordLevelTwo_PreRender">
                                        <Columns>
                                            <asp:TemplateField SortExpression="SP_ID" HeaderText="<%$ Resources:Text, SPID %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSPID" runat="server" Text='<%# Eval("SP_ID") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="70px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="SP_Eng_name" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblSP_Eng_Name" runat="server" Text='<%# Eval("SP_Eng_Name") %>'></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lblSP_Chi_Name" runat="server" Text='<%# Eval("SP_Chi_Name") %>' CssClass="TextGridChi"></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="130px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="ActionDescription" HeaderText="<%$ Resources:Text, Action %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblAction" runat="server" Text='<%# Eval("ActionDescription") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Information" HeaderText="<%$ Resources:Text, Information %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblInformation" runat="server"></asp:Label>
                                                    <asp:Image ID="imgIsShareToken" runat="server" ImageUrl="<%$ Resources:ImageUrl, ShareTokenBtn %>" AlternateText="<%$ Resources:AlternateText, ShareToken %>" />
                                                    <br />
                                                    <asp:Label ID="lblInformationTokenReplacement" runat="server" Visible="false"></asp:Label>
                                                    <asp:Image ID="imgIsShareTokenReplacement" runat="server" ImageUrl="<%$ Resources:ImageUrl, ShareTokenBtn %>" AlternateText="<%$ Resources:AlternateText, ShareToken %>" Visible="false" />
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="150px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Update_by" HeaderText="<%$ Resources:Text, Update_by %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblUpdate_by" runat="server" Text='<%# Eval("Update_by") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="100px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField SortExpression="Remark" HeaderText="<%$ Resources:Text, Remarks %>">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblRemark" runat="server" Text='<%# Eval("Remark") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="100px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <HeaderStyle HorizontalAlign="Center" />
                                    </asp:GridView>
                                </td>
                            </tr>
                        </table>
                        <table width="950px">
                            <tr>
                                <td style="height: 10px"></td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 10%">
                                    <asp:ImageButton ID="ibtnLevelTwoBack" runat="server" ImageUrl='<%$ Resources:ImageUrl, BackBtn %>'
                                        AlternateText='<%$ Resources:AlternateText, BackBtn %>' OnClick="ibtnLevelTwoBack_Click" />
                                </td>
                                <td align="center">
                                    <asp:ImageButton ID="ibtnLevelTwoConfirm" runat="server" ImageUrl='<%$ Resources:ImageUrl, ConfirmBtn %>'
                                        AlternateText='<%$ Resources:AlternateText, ConfirmBtn %>' OnClick="ibtnLevelTwoConfirm_Click" />
                                    <asp:ImageButton ID="ibtnLevelTwoReject" runat="server" ImageUrl='<%$ Resources:ImageUrl, RejectBtn %>'
                                        AlternateText='<%$ Resources:AlternateText, RejectBtn %>' OnClick="ibtnLevelTwoReject_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewComplete" runat="server">
                        <table>
                            <tr>
                                <td style="width: 185px; height: 20px">
                                    <asp:Label ID="lblConfirmationDateText" runat="server" Text='<%$ Resources:Text, ConfirmTime %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblConfirmationDate" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 185px; height: 20px">
                                    <asp:Label ID="lblNoOfConfirmedText" runat="server" Text='<%$ Resources:Text, NoOfItemConfirm %>'></asp:Label>
                                </td>
                                <td>
                                    <asp:Label ID="lblNoOfConfirmed" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" style="padding-top: 30px">
                                    <asp:ImageButton ID="ibtnReturn" runat="server" ImageUrl='<%$ Resources:ImageUrl, ReturnBtn %>'
                                        AlternateText='<%$ Resources:AlternateText, ReturnBtn %>' OnClick="ibtnReturn_Click" />
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
