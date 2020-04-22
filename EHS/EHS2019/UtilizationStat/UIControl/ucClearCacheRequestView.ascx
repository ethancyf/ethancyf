<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucClearCacheRequestView.ascx.vb" Inherits="UtilizationStat.ucClearCacheRequestView" %>

<asp:Panel ID="pnlClearCacheRequestView" runat="server">
    <table cellpadding="3" cellspacing="1" style="background-color:#999999; width:900px">
        <tr>
            <td style="background-color:white">
                <table cellpadding="2" style="width:100%">
                    <tr>
                        <td>
                            <table style="width:100%">
                                <tr>
                                    <td style="font-weight:bold">
                                        Clear Cache Request
                                    </td>
                                    <td style="text-align:right">
                                        <asp:Label ID="lblEOCLastUpdate" runat="server" Style="font-size:8pt"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color:#DD3333; height:2px">
                        </td>
                    </tr>
                    <tr>
                        <td style="padding:10px">
                            <asp:Button ID="btnEOCRefresh" runat="server" Text="Refresh" OnClick="btnEOCRefresh_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-bottom:10px; padding-left:10px">
                            <asp:GridView ID="gvEOC" runat="server" AllowPaging="false" AutoGenerateColumns="false"
                                          OnRowDataBound="gvEOC_RowDataBound" SkinID="IC">
                                <Columns>
                                    <asp:TemplateField HeaderText="Request Time">
                                        <ItemStyle Width="170px" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblRequestDtm" runat="server" Text='<%# Bind("Request_Dtm") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="App. Server">
                                        <ItemStyle Width="150px" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblApplicationServer" runat="server" Text='<%# Bind("Application_Server") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Cache File">
                                        <ItemStyle Width="140px" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblCacheFile" runat="server" Text='<%# Bind("Cache_File") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemStyle Width="50px" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblRecordStatus" runat="server" Text='<%# Bind("Record_Status") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                                <Columns>
                                    <asp:TemplateField HeaderText="Message">
                                        <ItemStyle Width="320px" />
                                        <ItemTemplate>
                                            <asp:Label ID="lblMessage" runat="server" Text='<%# Bind("Message") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
