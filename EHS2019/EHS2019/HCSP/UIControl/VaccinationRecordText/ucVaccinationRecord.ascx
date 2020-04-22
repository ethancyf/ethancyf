<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucVaccinationRecord.ascx.vb"
    Inherits="HCSP.ucVaccinationRecord1" %>
<%@ Register Src="../ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
    <tr>
        <td style="width: 100%">
            <asp:Label ID="lblVRVaccinationRecordText" runat="server" CssClass="tableHeader"
                Text="<%$ Resources: Text, VaccinationRecord %>"></asp:Label>
            <br />
            <asp:Label ID="lblNoOfRecord" runat="server" CssClass="tableText"></asp:Label>
            <br />
            <asp:Label ID="lblEHSText" runat="server" Text="[eHealth System]" CssClass="tableText"></asp:Label>
            <asp:Label ID="lblEHS" runat="server" CssClass="tableText"></asp:Label>

            <asp:Label ID="lblHAStart" runat="server" Text="<br />"></asp:Label>
            <asp:Label ID="lblHAText" runat="server" Text="[Hospital Authority]" CssClass="tableText"></asp:Label>
            <asp:Label ID="lblHAOpen" runat="server" CssClass="tableText" Text="["></asp:Label>
            <asp:Label ID="lblHA" runat="server" CssClass="tableText"></asp:Label>
            <asp:Label ID="lblHAClose" runat="server" CssClass="tableText" Text="]"></asp:Label>
            <asp:HiddenField ID="hfHA" runat="server" />

            <asp:Label ID="lblDHStart" runat="server" Text="<br />"></asp:Label>
            <asp:Label ID="lblDHText" runat="server" Text="[Department of Health]" CssClass="tableText"></asp:Label>
            <asp:Label ID="lblDHOpen" runat="server" CssClass="tableText" Text="["></asp:Label>
            <asp:Label ID="lblDH" runat="server" CssClass="tableText"></asp:Label>
            <asp:Label ID="lblDHClose" runat="server" CssClass="tableText" Text="]"></asp:Label>
            <asp:HiddenField ID="hfDH" runat="server" />
        </td>
    </tr>
    <tr>
        <td style="width: 100%; text-align: right">
            <asp:Button ID="btnShowHeader" runat="server" Text="<%$ Resources: AlternateText, ShowHeaderBtn %>"
                SkinID="TextOnlyVersionLinkButtonAlignRight" OnClick="btnShowHeader_Click" />
            <asp:Button ID="btnHideHeader" runat="server" Text="<%$ Resources: AlternateText, HideHeaderBtn %>"
                SkinID="TextOnlyVersionLinkButtonAlignRight" OnClick="btnHideHeader_Click" />
        </td>
    </tr>
    <tr>
        <td style="width: 100%">
            <asp:MultiView ID="mvVaccinationRecordView" runat="server" OnActiveViewChanged="mvVaccinationRecordView_ActiveViewChanged">
                <asp:View ID="viewHeaderShow" runat="server">
                    <asp:GridView ID="gvVaccinationRecordHeader" runat="server" AutoGenerateColumns="False"
                        Width="100%" ShowHeader="False" AllowPaging="False">
                        <Columns>
                            <asp:TemplateField>
                                <ItemStyle Width="18px" BackColor="#D7F2FF" VerticalAlign="Top" />
                                <ItemTemplate>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemStyle BackColor="#D7F2FF" VerticalAlign="Top" />
                                <ItemTemplate>
                                    <table width="100%" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td class="VR">
                                                <asp:Label ID="lblGInjectionDateText" runat="server" Text="<%$ Resources: Text, InjectionDate %>"
                                                    CssClass="tableTitle" Width="100%"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="VR">
                                                <asp:Label ID="lblGVaccinationText" runat="server" Text="<%$ Resources: Text, VaccineDose %>"
                                                    CssClass="tableTitle" Width="100%"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="VR">
                                                <asp:Label ID="lblGProviderText" runat="server" Text="<%$ Resources: Text, InformationProvider %>"
                                                    CssClass="tableTitle" Width="100%"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblGRemarkText" runat="server" Text="<%$ Resources: Text, RemarksIfAny %>"
                                                    CssClass="tableTitle" Width="100%"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:View>
                <asp:View ID="viewHeaderHide" runat="server">
                </asp:View>
            </asp:MultiView>
            <asp:Label ID="lblNoVaccinationRecord" runat="server" Text="<%$ Resources: Text, NoVaccinationRecordFound %>"
                CssClass="tableText"></asp:Label>
            <asp:GridView ID="gvVaccinationRecord" runat="server" AutoGenerateColumns="False"
                Width="100%" ShowHeader="False" AllowPaging="True" OnRowDataBound="gvVaccinationRecord_RowDataBound"
                OnPageIndexChanging="gvVaccinationRecord_PageIndexChanging">
                <Columns>
                    <asp:TemplateField>
                        <ItemStyle Width="18px" VerticalAlign="Top" />
                        <ItemTemplate>
                            <asp:Label ID="lblGIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemStyle VerticalAlign="Top" />
                        <ItemTemplate>
                            <table width="100%" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td class="VR">
                                        <asp:Label ID="lblGInjectionDate" runat="server" Text='<%# Bind("ServiceReceiveDtm") %>'
                                            CssClass="tableText" Width="100%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="VR">
                                        <asp:Label ID="lblGVaccination" runat="server" Text='<%# Bind("SubsidizeDesc") %>'
                                            CssClass="tableText" Width="100%"></asp:Label>
                                        <asp:Label ID="lblGVaccinationChi" runat="server" Text='<%# Bind("SubsidizeDescChi") %>'
                                            CssClass="tableText" Width="100%"></asp:Label>
                                        <asp:HiddenField ID="hfGDose" runat="server" Value='<%# Bind("AvailableItemDesc") %>' />
                                        <asp:HiddenField ID="hfGDoseChi" runat="server" Value='<%# Bind("AvailableItemDescChi") %>' />
                                    </td>
                                </tr>
                                <tr>
                                    <td id="tdGProvider" runat="server" class="VR">
                                        <asp:Label ID="lblGProvider" runat="server" Text='<%# Bind("Provider") %>' Width="100%"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="lblGRemark" runat="server" Text='<%# Bind("Remark") %>' CssClass="tableText"
                                            Width="100%"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </td>
    </tr>
    <tr style="height: 10px">
    </tr>
    <tr>
        <td>
            <asp:Label ID="lblDisclaimerTitle" runat="server" CssClass="tableHeader" Text="<%$ Resources:Text, Disclaimer %>"
                Width="100%"></asp:Label>
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" class="tableRemarkHighlight">
                <tr>
                    <td>
                        <asp:Label ID="lblDisclaimer" runat="server" CssClass="tableText" Text="<%$ Resources:Text, NotCompleteVaccinationRecord %>"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
