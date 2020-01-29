<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/text/ClaimVoucher.Master" Codebehind="VoidClaimSelectTransaction.aspx.vb" Inherits="HCSP.VoidClaimSelectTransaction" Title="<%$Resources:Title, ReimbursementClaimTransMgt%>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder" runat="server">
    <cc1:TextOnlyMessageBox ID="udcMsgBoxErr" runat="server" />
    <table id="Table1" runat="server" cellpadding="0" cellspacing="0" class="textVersionTable">
        <tr>
            <td>
                <asp:Label ID="lblItemTextFormat" runat="server" CssClass="tableTitle" Visible="false" ></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <table id="Table2" runat="server" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top">
                            <asp:RadioButtonList ID="rbSelectTransaction" runat="server" CellPadding="0" CellSpacing="0" Visible="false">
                            </asp:RadioButtonList>
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblSelectTransactionError" runat="server" ForeColor="Red" Text="*" Visible="False"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <asp:GridView ID="gvTranList" runat="server" AutoGenerateColumns="False" AllowPaging="true" AllowSorting="true">
                    <Columns>
                        <asp:TemplateField>
                            <ItemStyle Width="10px" />
                            <ItemTemplate>
                                <asp:Label ID="lblTranListIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionDate %>" SortExpression="Date">
                            <ItemStyle/>
                            <ItemTemplate>
                                <asp:Button ID="lblTranListTranDtm" runat="server" Text='<%# Bind("Date") %>' CommandArgument='<%# Eval("ID") %>' SkinID="TextOnlyVersionLinkButton"></asp:Button>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderText="<%$ Resources:Text, TransactionNo %>" SortExpression="ID">
                            <ItemStyle/>
                            <ItemTemplate>
                                <asp:Button ID="lbtn_transactionNum" runat="server" Text='<%# Eval("ID") %>' CommandArgument='<%# Eval("ID") %>' SkinID="TextOnlyVersionLinkButton"></asp:Button>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Button ID="btnCancel" runat="server" Text="<%$ Resources:AlternateText, CancelBtn%>" />
                <asp:Button ID="btnSelect" runat="server" Text="<%$ Resources:AlternateText, SelectBtn%>" Visible="false" />
            </td>
        </tr>
    </table>
</asp:Content>
