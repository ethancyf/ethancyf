<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHKIDSmartID.ascx.vb"
    Inherits="HCSP.UIControl.DocTypeText.ucInputHKIDSmartID" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
</script>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td class="tableCellStyle" valign="top">
                            <asp:Label ID="lbleHealthAccountText" runat="server" CssClass="sectionHeader" Width="100%"></asp:Label></td>
                        <td class="tableCellStyle" valign="top">
                            <asp:Label ID="lblDiffInSmartIDText" runat="server" CssClass="sectionHeader" Width="100%"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" style="height: 19px" valign="top">
                            <asp:Label ID="lblDocumentTypeText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td class="tableCellStyle" style="height: 19px" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" style="height: 19px" valign="top">
                            <asp:Label ID="lblDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
                        <td class="tableCellStyle" style="height: 19px" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" style="height: 19px" valign="top">
                            <asp:Label ID="lblHKICNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td class="tableCellStyle" style="height: 19px" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" style="height: 19px" valign="top">
                            <asp:Label ID="lblHKICNo" runat="server" MaxLength="9" CssClass="tableText"></asp:Label></td>
                        <td class="tableCellStyle" style="height: 19px" valign="top">
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" colspan="2" style="height: 19px" valign="top">
                            <asp:Label ID="lblENameText" runat="server" CssClass="tableTitle" Width="480px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" style="height: 19px; " valign="top">
                            <asp:Label ID="lblEName" runat="server" CssClass="tableText"></asp:Label>
                            <asp:Label ID="lblCName" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"></asp:Label></td>
                        <td class="tableCellStyle" style="height: 19px" valign="top">
                            <asp:Panel ID="panEnameSmartID" runat="server" Width="100%">
                            <asp:Label ID="lblENameSmartID" runat="server" CssClass="tableText"></asp:Label>
                                <asp:Label
                                    ID="lblCNameSmartID" runat="server" CssClass="tableText" Font-Names="HA_MingLiu"></asp:Label></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" colspan="2" style="height: 19px" valign="top">
                            <asp:Label ID="lblCCCodeText" runat="server" CssClass="tableTitle" Width="480px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" valign="top">
                            <asp:Panel ID="panCCCode1" runat="server">
                            <asp:Label ID="lblCCCode1" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                                <asp:Label ID="lblCCCode2" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                                <asp:Label ID="lblCCCode3" runat="server" CssClass="tableText" Width="35px"></asp:Label></asp:Panel>
                            <asp:Panel ID="panCCCode2" runat="server">
                            <asp:Label ID="lblCCCode4" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                                <asp:Label ID="lblCCCode5" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                                <asp:Label ID="lblCCCode6" runat="server" CssClass="tableText" Width="35px"></asp:Label></asp:Panel>
                        </td>
                        <td class="tableCellStyle" valign="top">
                            <asp:Panel ID="panCCCodeSmartID1" runat="server" Width="100%">
                                <asp:Label ID="lblCCCode1SmartID" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                                <asp:Label ID="lblCCCode2SmartID" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                                <asp:Label ID="lblCCCode3SmartID" runat="server" CssClass="tableText" Width="35px"></asp:Label>&nbsp;</asp:Panel>
                            <asp:Panel ID="panCCCodeSmartID2" runat="server">
                                <asp:Label ID="lblCCCode4SmartID" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                                <asp:Label ID="lblCCCode5SmartID" runat="server" CssClass="tableText" Width="35px"></asp:Label>
                                <asp:Label ID="lblCCCode6SmartID" runat="server" CssClass="tableText" Width="35px"></asp:Label></asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" colspan="2" valign="top">
                            <asp:Label ID="lblDOBText" runat="server" CssClass="tableTitle" Width="480px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" valign="top">
                            <asp:Label ID="lblDOB" runat="server" CssClass="tableText"></asp:Label></td>
                        <td class="tableCellStyle" valign="top">
                            <asp:Label ID="lblDOBSmartID" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" colspan="2" valign="top">
                            <asp:Label ID="lblGenderText" runat="server" CssClass="tableTitle" Width="480px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" valign="top">
                            <asp:Label ID="lblGender" runat="server" CssClass="tableText"></asp:Label></td>
                        <td class="tableCellStyle" valign="top">
                            <asp:Label ID="lblGenderSmartID" runat="server" CssClass="tableText" Visible="false"></asp:Label>
                            <asp:RadioButtonList ID="rbGenderSmartID" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" BackColor="#FFFF99" Visible="false">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:Label ID="lblGenderSmartIDError" runat="server" CssClass="validateFailText"
                                Text="*" Visible="False"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" colspan="2" valign="top">
                            <asp:Label ID="lblDOIText" runat="server" CssClass="tableTitle" Width="480px"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" valign="top">
                            <asp:Label ID="lblDOI" runat="server" CssClass="tableText"></asp:Label></td>
                        <td class="tableCellStyle" valign="top">
                            <asp:Label ID="lblDOISmartID" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </table>
