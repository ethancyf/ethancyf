<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucVaccinationRecord.ascx.vb"
    Inherits="HCVU.ucVaccinationRecord" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="../DocType/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc1" %>
<cc2:MessageBox ID="udcMessageBox" runat="server" Width="900px" Visible="False" />
<cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="900px" Visible="False" />
<div class="headingText">
    <asp:Label ID="lblPersonalParticularsText" runat="server" Font-Bold="true" Text="<%$ Resources:Text, PersonalParticulars %>"></asp:Label>
</div>
<uc1:ucReadOnlyDocumnetType ID="udcReadOnlyDocumnetType" runat="server" />
<hr style="width: 99%; color: #ff8080; border-top-style: none; border-right-style: none; border-left-style: none; height: 1px;" />
<div class="headingText">
    <asp:Label ID="lblVaccinationRecordText" runat="server" Font-Bold="True" Text="<%$ Resources:Text, VaccinationRecord %>"></asp:Label>
</div>
<table style="width: 930px;border-collapse:collapse;border-spacing:0px;padding:0px">
    <tr>
        <td>
            <table>
                <tr>
                    <td  style="width:123px;vertical-align:top">
                        <asp:Label ID="lblNoOfRecord" runat="server" CssClass="tableTitle"></asp:Label>
                    </td>
                    <td style="width:253px;vertical-align:top;border:1px solid black">
                        <table style="border-collapse:collapse;border-spacing:0px;padding:0px;background-color:white">
                            <tr>
                                <td style="width:250px;text-align:center">
                                    <asp:Label ID="lblEHSText" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:250px;text-align:center">
                                    <asp:Label ID="lblEHS" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width:4px"/>
                    <asp:Panel ID="panHA" runat="server">
                    <td style="width:253px;vertical-align:top;border:1px solid black">
                        <table style="border-collapse:collapse;border-spacing:0px;padding:0px;background-color:white">
                            <tr>
                                <td style="width:250px;text-align:center">
                                    <asp:Label ID="lblHAText" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:250px;text-align:center">
                                    <asp:Label ID="lblHA" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:HiddenField ID="hfHA" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    <td style="width:4px"/>
                    </asp:Panel>
                    <asp:Panel ID="panDH" runat="server">
                    <td style="width:254px;vertical-align:top;border:1px solid black">
                        <table style="border-collapse:collapse;border-spacing:0px;padding:0px;background-color:white">
                            <tr>
                                <td style="width:250px;text-align:center">
                                    <asp:Label ID="lblDHText" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:248px;text-align:center">
                                    <asp:Label ID="lblDH" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:HiddenField ID="hfDH" runat="server" />
                                </td>
                            </tr>
                        </table>
                    </td>
                    </asp:Panel>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblNoVaccinationRecord" runat="server" Text="<%$ Resources: Text, NoVaccinationRecordFound %>"
                CssClass="tableText"></asp:Label>
            <asp:GridView ID="gvVaccinationRecord" runat="server" AutoGenerateColumns="False"
                Width="1100px" AllowPaging="True" AllowSorting="True" OnRowDataBound="gvVaccinationRecord_RowDataBound"
                OnPageIndexChanging="gvVaccinationRecord_PageIndexChanging" OnPreRender="gvVaccinationRecord_PreRender"
                OnSorting="gvVaccinationRecord_Sorting">
                <Columns>
                    <asp:TemplateField HeaderText="">
                        <HeaderStyle VerticalAlign="Top" />
                        <ItemStyle Width="16px" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblGIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="ServiceReceiveDtm" HeaderText="<%$ Resources: Text, InjectionDate %>">
                        <HeaderStyle VerticalAlign="Top" />
                        <ItemStyle Width="90px" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblGInjectionDate" runat="server" Text='<%# Bind("ServiceReceiveDtm") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="SubsidizeDesc" HeaderText="<%$ Resources: Text, Vaccines %>">
                        <HeaderStyle VerticalAlign="Top" />
                        <ItemStyle Width="240px" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblGVaccination" runat="server" Text='<%# Bind("SubsidizeDesc") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="AvailableItemDesc" HeaderText="<%$ Resources: Text, Dose %>">
                        <HeaderStyle VerticalAlign="Top" />
                        <ItemStyle Width="70px" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblGDose" runat="server" Text='<%# Bind("AvailableItemDesc") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Provider" HeaderText="<%$ Resources: Text, InformationProvider %>">
                        <HeaderStyle VerticalAlign="Top" />
                        <ItemStyle Width="220px" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblGProvider" runat="server" Text='<%# Bind("Provider") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Location" HeaderText="<%$ Resources: Text, Location %>">
                        <HeaderStyle VerticalAlign="Top" />
                        <ItemStyle Width="130px" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblGLocation" runat="server" Text='<%# Bind("Location") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="Remark" HeaderText="<%$ Resources: Text, Remarks %>">
                        <HeaderStyle VerticalAlign="Top" />
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblGRemark" runat="server" Text='<%# Bind("Remark") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField SortExpression="RecordCreationDtm" HeaderText="<%$ Resources: Text, RecordCreationTime %>">
                        <HeaderStyle VerticalAlign="Top" />
                        <ItemStyle Width="145px" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblGRecordCreationDtm" runat="server" Text='<%# Bind("RecordCreationDtm") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblVaccRecordBaseOnIDDoc" runat="server" Text="<%$ Resources: Text, VaccRecordBaseOnIDDoc %>" CssClass="tableTitle"></asp:Label>
        </td>
    </tr>
</table>
