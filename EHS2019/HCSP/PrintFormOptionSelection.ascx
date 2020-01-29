<%@ Control Language="vb" AutoEventWireup="false" Codebehind="PrintFormOptionSelection.ascx.vb"
    Inherits="HCSP.PrintFormOptionSelection" %>
<table runat="server" id= "tblSelectPinrOption">
    <tr>
        <td>
            <table cellpadding="0" cellspacing="0" width="100%" style="width: 100%">
                <tr>
                    <td align="left">
                        <asp:Label ID="lblSelectPrintOptionText" runat="server" CssClass="tableText" Text="Please select print options"></asp:Label></td>
                </tr>
                <tr>
                    <td style="height: 15px">
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                            <tr>
                                <td>
                                    <table runat="server" id="tbNotPrint" border="0" cellpadding="0" cellspacing="0" >
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rbNotPrint" runat="server" GroupName="PrintOption"  style="width: 100%"
                                                    CssClass="tableText" /></td>
                                        </tr>
                                        <tr style="width: 5px">
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 20px">
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="txtNotPrintRemind" runat="server"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px">
                                            </td>
                                        </tr>
                                    </table>
                                    <table runat="server" id="tbPrintFull" border="0" cellpadding="0" cellspacing="0" >
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rbPrintFull" runat="server" GroupName="PrintOption"  style="width: 100%"
                                                    CssClass="tableText" /></td>
                                        </tr>
                                        <tr>
                                            <td style="height: 10px">
                                            </td>
                                        </tr>
                                    </table>
                                    <table runat="server" id="tbPrintCondensed" border="0" cellpadding="0" cellspacing="0" >
                                        <tr>
                                            <td>
                                                <asp:RadioButton ID="rbPrintCondensed" runat="server" GroupName="PrintOption"  style="width: 100%"
                                                    CssClass="tableText" /></td>
                                        </tr>
                                        <tr style="width: 5px">
                                        </tr>
                                        <tr>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td style="width: 20px">
                                                        </td>
                                                        <td>
                                                            <asp:Label ID="txtPrintCondensedRemind" runat="server"></asp:Label></td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
