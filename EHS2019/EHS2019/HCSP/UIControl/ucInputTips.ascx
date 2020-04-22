<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputTips.ascx.vb" Inherits="HCSP.ucInputTips" %>
<asp:MultiView ID="mvInputTips" runat="server">
    <!-- Doc Type -->
    <asp:View ID="vSearchDocType" runat="server">
        <table cellpadding="0" cellspacing="0" class="textVersionTable">
            <asp:Panel ID="panSearchTipsHKICSymbol" runat="server" Visible="false">
            <tr>
                <td>
                    <asp:Label ID="lblInputHKICSymbolText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputHKICSymbolHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
            </asp:Panel>
            <tr>
                <td>
                    <asp:Label ID="lblSearchDocIDText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSearchDocIDHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSearchDocDOBText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblSearchDocDOBHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vInputHKIC" runat="server">
        <table cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td>
                    <asp:Label ID="lblInputHKICENameText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputHKICENameHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputHKICDOIText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputHKICDOIHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vInputEC" runat="server">
        <table cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td>
                    <asp:Label ID="lblInputECSerialText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputECSerialHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputECRefText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputECRefHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputECDOIText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputECDOIHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vInputHKBC" runat="server">
        <table cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td>
                    <asp:Label ID="lblInputHKBCENameText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputHKBCENameHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vInputDI" runat="server">
        <table cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td>
                    <asp:Label ID="lblInputDIENameText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputDIENameHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputDIDOIText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputDIDOIHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vInputREPMT" runat="server">
        <table cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td>
                    <asp:Label ID="lblInputREPMTENameText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputREPMTENameHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputREPMTDOIText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputREPMTDOIHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vInputID235B" runat="server">
        <table cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td>
                    <asp:Label ID="lblInputID235BENameText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputID235BENameHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputID235BPermitText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputID235BPermitHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vInputVISA" runat="server">
        <table cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td>
                    <asp:Label ID="lblInputVISAENameText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputVISAENameHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:View>
    <asp:View ID="vInputADOPC" runat="server">
        <table cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td>
                    <asp:Label ID="lblInputADOPCENameText" runat="server" CssClass="tableTitle"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputADOPCENameHint" runat="server" CssClass="tableText"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:View>
        <asp:View ID="vInputSmrtID" runat="server">
        <table border="0" cellpadding="0" cellspacing="0" class="textVersionTable">
            <tr>
                <td style="height: 19px">
                    <asp:Label ID="lblInputSmartIDGenderText" runat="server" CssClass="tableTitle"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:Label ID="lblInputSmartIDGender" runat="server" CssClass="tableText" ></asp:Label></td>
            </tr>
        </table>
    </asp:View>
    <!-- Scheme -->
</asp:MultiView>