<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucReadOnlyEC.ascx.vb"
    Inherits="HCVU.ucReadOnlyEC" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<asp:MultiView ID="MultiViewEC" runat="server">
    <asp:View ID="ViewHorizontal" runat="server">
        <table id="tblHEC" runat="server">
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDocumentTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                <td style="vertical-align: top" colspan="3">
                    <asp:Label ID="lblHDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHEName" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblHCName" runat="server" CssClass="tableText TextChineseName"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHECSerialNoText" runat="server" Text="<%$ Resources:Text, ECSerialNo %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHECSerialNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHECHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHECHKID" runat="server" CssClass="tableText"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHECReferenceNoText" runat="server" Text="<%$ Resources:Text, ECReference %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHECReferenceNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDOBGenderText" runat="server" Text="<%$ Resources:Text, DOBLongGender %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDOB" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblHSlash" runat="server" Text="/" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblHGender" runat="server" CssClass="tableText"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHECDateText" runat="server" Text="<%$ Resources:Text, ECDate %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHECDate" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblHDODText" runat="server" Text="<%$ Resources: Text, DateOfDeath %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="top">
                            <td>
                                <asp:Label ID="lblHDOD" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                            <td style="padding-left: 5px">
                                <cc1:CustomImageButton ID="ibtnHDOD" runat="server" ImageUrl="<%$ Resources:ImageUrl, DeathRecordBtn %>"
                                    ImageUrlDisable="<%$ Resources:ImageUrl, DeathRecordDisableBtn %>" AlternateText="<%$ Resources:AlternateText, DeathRecord %>" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td style="vertical-align: top">
                </td>
                <td style="vertical-align: top">
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="ViewVertical" runat="server">
        <table id="tblVEC" runat="server">
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVDocumentTypeText" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVECHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVECHKID" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVECSerialNoText" runat="server" Text="<%$ Resources:Text, ECSerialNo %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVECSerialNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVECReferenceNoText" runat="server" Text="<%$ Resources:Text, ECReference %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVECReferenceNo" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVECDateText" runat="server" Text="<%$ Resources:Text, ECDate %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVECDate" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVEName" runat="server" CssClass="tableText"></asp:Label>
                    <asp:Label ID="lblVCName" runat="server" CssClass="tableText TextChineseName"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVDOBText" runat="server" Text="<%$ Resources:Text, DOB %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVDOB" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVGenderText" runat="server" Text="<%$ Resources:Text, Gender %>"></asp:Label>
                </td>
                <td style="vertical-align: top">
                    <asp:Label ID="lblVGender" runat="server" CssClass="tableText"></asp:Label></td>
            </tr>
            <tr id="rowDOD">
                <td style="vertical-align: top">
                    <asp:Label ID="lblVDODText" runat="server" Text="<%$ Resources: Text, DateOfDeath %>"></asp:Label></td>
                <td style="vertical-align: top">
                    <table cellpadding="0" cellspacing="0">
                        <tr valign="top">
                            <td>
                                <asp:Label ID="lblVDOD" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                            <td style="padding-left: 5px">
                                <cc1:CustomImageButton ID="ibtnVDOD" runat="server" ImageUrl="<%$ Resources:ImageUrl, DeathRecordBtn %>"
                                    ImageUrlDisable="<%$ Resources:ImageUrl, DeathRecordDisableBtn %>" AlternateText="<%$ Resources:AlternateText, DeathRecord %>" />
                                 <asp:Image ID="imgVDOD" runat="server" ImageUrl="<%$ Resources:ImageUrl, DeathRecordBtn %>"
                                     AlternateText="<%$ Resources:AlternateText, DeathRecord %>" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </asp:View>
</asp:MultiView>
