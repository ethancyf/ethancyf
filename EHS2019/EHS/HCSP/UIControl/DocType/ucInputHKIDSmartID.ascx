<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHKIDSmartID.ascx.vb"
    Inherits="HCSP.ucInputHKIDSmartID" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
</script>

<asp:Panel ID="panEnterDetailModify" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 860px">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr runat="server">
                        <td class="tableCellStyle" style="width: 209px; height: 19px;" valign="top">
                        </td>
                        <td class="tableCellStyle" valign="top" style="height: 19px">
                            <asp:Label ID="lbleHealthAccountText" runat="server" CssClass="tableTitleUnderline"
                                Width="200px"></asp:Label></td>
                        <td valign="top" style="height: 19px">
                            <asp:Label ID="lblDiffInSmartIDText" runat="server" CssClass="tableTitleUnderline"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" style="width: 209px;" valign="top">
                            <asp:Label ID="lblDocumentTypeText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td class="tableCellStyle" valign="top" colspan="2">
                            <asp:Label ID="lblDocumentType" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 209px" valign="top" class="tableCellStyle">
                            <asp:Label ID="lblHKICNoText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td valign="top" class="tableCellStyle" colspan="2">
                            <asp:Label ID="lblHKICNo" runat="server" MaxLength="9" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="height: 19px; width: 209px;" valign="top" class="tableCellStyle">
                            <asp:Label ID="lblENameText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td style="height: 19px; width: 280px; word-wrap: break-word; word-break:break-all;" valign="top" class="tableCellStyle">
                            <asp:Label ID="lblEName" runat="server" CssClass="tableText"></asp:Label>
                            <asp:Label ID="lblCName" runat="server" CssClass="tableText TextChineseName"></asp:Label></td>
                        <td class="tableCellStyle" style="width: 400px; height: 19px; word-wrap: break-word; word-break:break-all;" valign="top">
                            <asp:Label ID="lblENameSmartID" runat="server" CssClass="tableText"></asp:Label>
                            <asp:Label ID="lblCNameSmartID" runat="server" CssClass="tableText TextChineseName"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="width: 209px;" valign="top" class="tableCellStyle">
                            <asp:Label ID="lblCCCodeText" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td style="width: 280px;" valign="top" class="tableCellStyle">
                            <asp:Label ID="lblCCCode1" runat="server" Width="35px" CssClass="tableText"></asp:Label>&nbsp;
                            <asp:Label ID="lblCCCode2" runat="server" Width="35px" CssClass="tableText"></asp:Label>&nbsp;
                            <asp:Label ID="lblCCCode3" runat="server" Width="35px" CssClass="tableText"></asp:Label>&nbsp;
                            <asp:Label ID="lblCCCode4" runat="server" Width="35px" CssClass="tableText"></asp:Label>&nbsp;
                            <asp:Label ID="lblCCCode5" runat="server" Width="35px" CssClass="tableText"></asp:Label>&nbsp;
                            <asp:Label ID="lblCCCode6" runat="server" Width="35px" CssClass="tableText"></asp:Label></td>
                        <td class="tableCellStyle" style="width: 400px" valign="top">
                            <asp:Label ID="lblCCCode1SmartID" runat="server" Width="35px" CssClass="tableText"></asp:Label>&nbsp;
                            <asp:Label ID="lblCCCode2SmartID" runat="server" Width="35px" CssClass="tableText"></asp:Label>&nbsp;
                            <asp:Label ID="lblCCCode3SmartID" runat="server" Width="35px" CssClass="tableText"></asp:Label>&nbsp;
                            <asp:Label ID="lblCCCode4SmartID" runat="server" Width="35px" CssClass="tableText"></asp:Label>&nbsp;
                            <asp:Label ID="lblCCCode5SmartID" runat="server" Width="35px" CssClass="tableText"></asp:Label>&nbsp;
                            <asp:Label ID="lblCCCode6SmartID" runat="server" Width="35px" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr>
                        <td class="tableCellStyle" style="width: 209px" valign="top">
                            <asp:Label ID="lblDOBText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td class="tableCellStyle" style="width: 280px" valign="top">
                            <asp:Label ID="lblDOB" runat="server" CssClass="tableText"></asp:Label></td>
                        <td class="tableCellStyle" style="width: 400px" valign="top">
                            <asp:Label ID="lblDOBSmartID" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                    <tr  id="trGender" runat="server" style="height: 190px;">
                        <td valign="top" class="tableCellStyle" style="width: 209px">
                            <asp:Label ID="lblGenderText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 280px" class="tableCellStyle">
                            <asp:Label ID="lblGender" runat="server" CssClass="tableText"></asp:Label></td>
                        <td class="tableCellStyle" style="width: 400px" valign="top">
                            <asp:Label ID="lblGenderSmartID" runat="server" CssClass="tableText" Visible="false"></asp:Label>


                            <asp:RadioButtonList ID="rbGenderSmartID" runat="server" CssClass="tableText" RepeatDirection="Horizontal"
                                RepeatLayout="Flow" AutoPostBack="True" BackColor="#FFFF99" style="display:none">
                                <asp:ListItem Value="F">Female</asp:ListItem>
                                <asp:ListItem Value="M">Male</asp:ListItem>
                            </asp:RadioButtonList>&nbsp;
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
                            
                            
                            
                            <asp:Image ID="imgGenderSmartIDError" runat="server"
                                AlternateText="<%$ Resources:AlternateText, ErrorBtn%>" ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                Visible="false" />                            
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="tableCellStyle" style="width: 209px">
                            <asp:Label ID="lblDOIText" runat="server" CssClass="tableTitle" Width="150px"></asp:Label></td>
                        <td valign="top" style="width: 280px" class="tableCellStyle">
                            <asp:Label ID="lblDOI" runat="server" CssClass="tableText"></asp:Label></td>
                        <td class="tableCellStyle" style="width: 400px" valign="top">
                            <asp:Label ID="lblDOISmartID" runat="server" CssClass="tableText"></asp:Label></td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
</asp:Panel>
