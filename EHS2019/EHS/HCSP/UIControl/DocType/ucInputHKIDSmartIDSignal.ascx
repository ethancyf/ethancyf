<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHKIDSmartIDSignal.ascx.vb"
    Inherits="HCSP.ucInputHKIDSmartIDSignal" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
</script>

<asp:Panel ID="panEnterDetail" runat="server">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td class="tableCellStyle" style="width: 200px;" valign="top">
                <asp:Label ID="lblDocumentTypeText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td class="tableCellStyle" valign="top" colspan="1">
                <asp:Label ID="lblDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 200px" valign="top" class="tableCellStyle">
                <asp:Label ID="lblHKICNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td valign="top" class="tableCellStyle" colspan="1">
                <asp:Label ID="lblHKICNo" runat="server" CssClass="tableText" MaxLength="9"></asp:Label></td>
        </tr>
        <tr>
            <td style="height: 19px; width: 200px;" valign="top" class="tableCellStyle">
                <asp:Label ID="lblENameText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td style="height: 19px;" valign="top" class="tableCellStyle">
                <asp:Label ID="lblEName" runat="server" CssClass="tableText"></asp:Label>
                <asp:Label ID="lblCName" runat="server" CssClass="tableText TextChineseName"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 200px;" valign="top" class="tableCellStyle">
                <asp:Label ID="lblCCCodeText" runat="server" CssClass="tableTitle"></asp:Label></td>
            <td valign="top" class="tableCellStyle">
                <asp:Label ID="lblCCCode1" runat="server" CssClass="tableText" Width="35px"></asp:Label>&nbsp;
                <asp:Label ID="lblCCCode2" runat="server" CssClass="tableText" Width="35px"></asp:Label>&nbsp;
                <asp:Label ID="lblCCCode3" runat="server" CssClass="tableText" Width="35px"></asp:Label>&nbsp;
                <asp:Label ID="lblCCCode4" runat="server" CssClass="tableText" Width="35px"></asp:Label>&nbsp;
                <asp:Label ID="lblCCCode5" runat="server" CssClass="tableText" Width="35px"></asp:Label>&nbsp;
                <asp:Label ID="lblCCCode6" runat="server" CssClass="tableText" Width="35px"></asp:Label></td>
        </tr>
        <tr>
            <td class="tableCellStyle" style="width: 200px" valign="top">
                <asp:Label ID="lblDOBText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td class="tableCellStyle" valign="top">
                <asp:Label ID="lblDOB" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr style="display:none;">
            <td style="width: 350px;vertical-align:top;" class="tableCellStyleLite">
                <asp:RadioButtonList ID="rbGender" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                    RepeatLayout="Flow" AutoPostBack="True">
                    <asp:ListItem Value="F">Female</asp:ListItem>
                    <asp:ListItem Value="M">Male</asp:ListItem>
                </asp:RadioButtonList>&nbsp;                            
        </tr>
        <tr id="trGenderImageInput" runat="server" style="height: 190px;">
            <td style="width: 200px; vertical-align: top" class="tableCellStyleLite">
                <asp:Label ID="lblGender" runat="server" CssClass="tableTitle" Width="180px"></asp:Label></td>
            <td>
                <div  id="divGender" runat="server" style="background-color: #ffff99; padding: 15px; width: 395px;">
                    <table id="tblIGender" runat="server" style="border-spacing: 0px; border-collapse: collapse; padding: 0px">
                        <tr>
                            <td>
                                <div id="divFemale" runat="server" style="width: 180px; outline: solid; outline-width: 2px; outline-color: black; padding-left: 5px; background-color: white; position: relative; cursor: pointer;">
                                    <asp:Image ID="imgFemale" runat="server" AlternateText="<%$ Resources:AlternateText, Female%>"
                                        ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, SmartICFemale%>" />
                                    <asp:Label ID="lblIFemale" runat="server" CssClass="tableText" Style="position: absolute; left: 120px; top: 65px" />
                                    <asp:Label ID="lblIFemaleChi" runat="server" CssClass="tableText" Style="position: absolute; left: 120px; top: 85px" />
                                </div>
                            </td>
                            <td style="width: 20px"></td>
                            <td>
                                <div id="divMale" runat="server" style="width: 180px; outline: solid; outline-width: 2px; outline-color: black; padding-left: 5px; background-color: white; position: relative; cursor: pointer;">
                                    <asp:Image ID="imgMale" runat="server" AlternateText="<%$ Resources:AlternateText, Male%>"
                                        ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, SmartICMale%>" />
                                    <asp:Label ID="lblIMale" runat="server" CssClass="tableText" Style="position: absolute; left: 120px; top: 65px" />
                                    <asp:Label ID="lblIMaleChi" runat="server" CssClass="tableText" Style="position: absolute; left: 120px; top: 85px" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </div>
                <div>
                    <asp:Label ID="lblReadonlyGender" runat="server" CssClass="tableText" Visible="false"></asp:Label>
                </div>
            </td>
            <td>
                <asp:Image ID="imgGender" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                    ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false" />
            </td>                       
        </tr>
        <tr>
            <td valign="top" class="tableCellStyle" style="width: 200px">
                <asp:Label ID="lblDOIText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
            <td valign="top" class="tableCellStyle">
                <asp:Label ID="lblDOI" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
    </table>
</asp:Panel>
