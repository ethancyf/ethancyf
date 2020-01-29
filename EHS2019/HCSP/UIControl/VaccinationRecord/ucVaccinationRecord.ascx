<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucVaccinationRecord.ascx.vb"
    Inherits="HCSP.ucVaccinationRecord" %>
<%@ Register Src="../ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<div style="padding-top: 5px">
    <cc2:MessageBox ID="udcMessageBox" runat="server" Width="902px" Visible="False" />
    <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="902px" Visible="False" />
</div>
<table>
    <tr>
        <td class="eHSTableHeading" valign="top">
            <asp:Label ID="lblAccountInformation" runat="server" Text="<%$ Resources:Text, AccountInfo %>"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <asp:MultiView ID="mvDocumentType" runat="server">
                <asp:View ID="ViewControl" runat="server">
                    <uc1:ucReadOnlyDocumnetType ID="udcReadOnlyDocumnetType" runat="server"></uc1:ucReadOnlyDocumnetType>
                </asp:View>
                <asp:View ID="ViewSimple" runat="server">
                    <table cellpadding="0" cellspacing="0">
                        <tr style="height: 24px">
                            <td style="vertical-align: top; width: 160px">
                                <asp:Label ID="lblSDocumentTypeText" runat="server" Text="<%$ Resources: Text, DocumentType %>"
                                    CssClass="tableTitle"></asp:Label>
                            </td>
                            <td colspan="3" style="vertical-align: top">
                                <asp:Label ID="lblSDocumentType" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="vertical-align: top; width: 160px">
                                <asp:Label ID="lblSNameText" runat="server" Text="<%$ Resources: Text, Name %>" CssClass="tableTitle"></asp:Label>
                            </td>
                            <td style="vertical-align: top; width: 300px">
                                <asp:Label ID="lblSEName" runat="server" CssClass="tableText"></asp:Label>
                                <asp:Label ID="lblSCName" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"></asp:Label>
                            </td>
                            <td style="vertical-align: top; width: 160px">
                                <asp:Label ID="lblSDOBGenderText" runat="server" Text="<%$ Resources: Text, DOBLongGender %>"
                                    CssClass="tableTitle"></asp:Label>
                            </td>
                            <td style="vertical-align: top; width: 300px">
                                <asp:Label ID="lblSDOB" runat="server" CssClass="tableText"></asp:Label>
                                <asp:Label ID="lblSDOBGenderSlash" runat="server" Text="/" CssClass="tableText"></asp:Label>
                                <asp:Label ID="lblSGender" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="vertical-align: top">
                                <asp:Label ID="lblSDocumentNoText" runat="server" CssClass="tableTitle"></asp:Label>
                            </td>
                            <td style="vertical-align: top">
                                <asp:Label ID="lblSDocumentNo" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                            <td style="vertical-align: top">
                                <asp:Label ID="lblSECSerialNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ECSerialNo %>">></asp:Label>
                            </td>
                            <td style="vertical-align: top">
                                <asp:Label ID="lblSECSerialNo" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="ViewSimpleNoName" runat="server">
                    <table cellpadding="0" cellspacing="0">
                        <tr style="height: 24px">
                            <td style="vertical-align: top; width: 160px">
                                <asp:Label ID="lblSNDocumentTypeText" runat="server" Text="<%$ Resources: Text, DocumentType %>"
                                    CssClass="tableTitle"></asp:Label>
                            </td>
                            <td colspan="3" style="vertical-align: top">
                                <asp:Label ID="lblSNDocumentType" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                        <tr style="height: 24px">
                            <td style="vertical-align: top">
                                <asp:Label ID="lblSNDocumentNoText" runat="server" CssClass="tableTitle"></asp:Label>
                            </td>
                            <td style="vertical-align: top; width: 300px">
                                <asp:Label ID="lblSNDocumentNo" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                            <td style="vertical-align: top; width: 160px">
                                <asp:Label ID="lblSNDOBText" runat="server" Text="<%$ Resources: Text, DOBLong %>"
                                    CssClass="tableTitle"></asp:Label>
                            </td>
                            <td style="vertical-align: top; width: 300px">
                                <asp:Label ID="lblSNDOB" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </td>
    </tr>
    <tr>
        <td>
            <hr />
        </td>
    </tr>
    <tr>
        <td>
            <table style="width: 930px;border-collapse:collapse;border-spacing:0px;padding:0px">
                <tr>
                    <td class="eHSTableHeading" style="vertical-align: top; width: 25%">
                        <asp:Label ID="lblVaccinationRecord" runat="server" Text="<%$ Resources:Text, VaccinationRecord %>"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left; vertical-align: top; padding-top: 2px">
                        <table>
                            <tr>
                                <td style="width:144px;vertical-align:top">
                                    <asp:Label ID="lblNoOfRecord" runat="server" Text="[No. of records:]" CssClass="tableTitle"></asp:Label>
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
                                <asp:Panel ID="panHA" runat="server">
                                <td style="width:4px"/>
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
                                </asp:Panel>
                                <asp:Panel ID="panDH" runat="server">
                                <td style="width:4px"/>
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
                    <td colspan="2">
                        <asp:Label ID="lblNoVaccinationRecord" runat="server" Text="<%$ Resources: Text, NoVaccinationRecordFound %>"
                            CssClass="tableText"></asp:Label>
                        <asp:GridView ID="gvVaccinationRecord" runat="server" AutoGenerateColumns="False" Width="930px"
                            AllowPaging="True" AllowSorting="True" OnRowDataBound="gvVaccinationRecord_RowDataBound"
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
                                    <ItemStyle Width="115px" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGInjectionDate" runat="server" Text='<%# Bind("ServiceReceiveDtm") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="SubsidizeDesc" HeaderText="<%$ Resources: Text, Vaccines %>">
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemStyle Width="250px" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGVaccination" runat="server" Text='<%# Bind("SubsidizeDesc") %>'></asp:Label>
                                        <asp:Label ID="lblGVaccinationChi" runat="server" Text='<%# Bind("SubsidizeDescChi") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="AvailableItemDesc" HeaderText="<%$ Resources: Text, Dose %>">
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemStyle Width="80px" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGDose" runat="server" Text='<%# Bind("AvailableItemDesc") %>'></asp:Label>
                                        <asp:Label ID="lblGDoseChi" runat="server" Text='<%# Bind("AvailableItemDescChi") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Provider" HeaderText="<%$ Resources: Text, InformationProvider %>">
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemStyle Width="230px" VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGProvider" runat="server" Text='<%# Bind("Provider") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Remark" HeaderText="<%$ Resources: Text, Remarks %>">
                                    <HeaderStyle VerticalAlign="Top" />
                                    <ItemStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <asp:Label ID="lblGRemark" runat="server" Text='<%# Bind("Remark") %>'></asp:Label>
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
