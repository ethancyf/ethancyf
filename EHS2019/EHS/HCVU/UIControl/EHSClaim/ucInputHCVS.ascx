<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHCVS.ascx.vb"
    Inherits="HCVU.ucInputHCVS" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc2" %>
<table cellpadding="0" cellspacing="0" style="width: 753px" >
    <tr id="trDHCRelatedService" runat="server">
        <td valign="top" class="tableCellStyle">
            <asp:Label ID="lblDHCRelatedServiceTitle" runat="server" Text="<%$ Resources:Text, DHCRelatedService%>" />
        </td>
        <td valign="top" class="tableCellStyle" style="padding-left: 3px">
            <asp:CheckBox ID="chkDHCRelatedService" runat="server" CssClass="tableText" Style="position:relative;left:-2px" AutoPostBack="true"></asp:CheckBox>
            <asp:DropDownList ID="ddlDistrictCode" runat="server"  Enabled="false" AutoPostBack="true" />
             <asp:Image ID="imgDistrictCodeError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
        </td>   
    </tr>
    <tr style="height: 22px">
        <td valign="top" style="width: 200px" class="tableCellStyle">
            <asp:Label ID="lblAvailableVoucherText" runat="server" Width="160px"></asp:Label></td>
        <td valign="top"  class="tableCellStyle" style="padding-left: 3px">
            <asp:Label ID="lblAvailableVoucher" runat="server" CssClass="tableText">$0</asp:Label></td>        
    </tr>
    <asp:Panel ID="pnlAvailableQuota" runat="server" Visible="false">
    <tr style="height: 35px">
        <td style="vertical-align:top;padding-bottom:5px" class="tableCellStyle">
            <table style="padding:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top;width: 10px; padding-left: 6px;">
                        -         
                    </td>
                    <td style="vertical-align:top">
                        <asp:Label ID="lblAvailableQuotaText" runat="server" CssClass="tableTitle"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
        <td style="vertical-align:middle;padding-left:3px" class="tableCellStyle"> 
            <asp:Label ID="lblAvailableQuota" runat="server" CssClass="tableText" />
            <asp:Label ID="lblAvailableQuotaUpTo" runat="server" CssClass="tableText" />           
        </td>
    </tr>
    <tr style="height: 35px">
        <td style="vertical-align:top;padding-bottom:5px" class="tableCellStyle">
            <table style="padding:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top;width: 10px; padding-left: 6px;">
                        -         
                    </td>
                    <td style="vertical-align:top">
                        <asp:Label ID="lblMaximumVoucherAmountText" runat="server" />
                    </td>
                </tr>
            </table>
        </td>
        <td style="vertical-align:middle;padding-left:3px" class="tableCellStyle">
            <asp:Label ID="lblMaximumVoucherAmount" runat="server" CssClass="tableText" />      
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td valign="top" style="width: 200px" class="tableCellStyle">
            <asp:Label ID="lblVoucherRedeemText" runat="server" Width="160px" Height="25px"></asp:Label>
        </td>
        <td valign="top">
            <table cellpadding="0" cellspacing="0">
                <tbody>
                    <tr> <%-- 
                        <td valign="top" id="cellVoucherRedeem" runat="server">
                            <asp:RadioButtonList ID="rbVoucherRedeem" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
                            </asp:RadioButtonList></td>--%>
                        <td valign="top" class="tableCellStyle" style="padding-left: 3px">
                            <asp:Label ID="Label1" runat="server" CssClass="tableText">$&nbsp;</asp:Label><asp:TextBox ID="txtRedeemAmount" runat="server" AutoCompleteType="Disabled" AutoPostBack="false"
                                MaxLength="2" Width="42px"></asp:TextBox></td>
                      <td valign="middle">
                            <span class="tableText">&nbsp;</span></td>
                       <%--  <td valign="middle">
                            <asp:Label ID="lblTotalAmount" runat="server" CssClass="tableText">$0</asp:Label></td>--%>
                        <td style="width: 760px" valign="top">
                            <asp:Image ID="imgVoucherRedeemError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                            <cc1:FilteredTextBoxExtender ID="filtereditVoucherRedeem" runat="server" FilterType="Numbers"
                                TargetControlID="txtRedeemAmount">
                            </cc1:FilteredTextBoxExtender>
                        </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    <tr id="trCoPaymentFee" runat="server">
        <td valign="top" style="width: 175px;" class="tableCellStyle">
            <asp:Label ID="lblCoPaymentFee" runat="server"></asp:Label></td>
        <td valign="top" class="tableCellStyle" style="padding-left: 3px">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" CssClass="tableText">$&nbsp;</asp:Label><asp:TextBox
                            ID="txtCoPaymentFee" runat="server" AutoCompleteType="Disabled" AutoPostBack="False"
                            MaxLength="6" Width="42px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredCoPaymentFee" runat="server" FilterType="Custom,Numbers"
                            ValidChars="" TargetControlID="txtCoPaymentFee">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                    <td style="padding-left: 3px;">
                        <asp:Image ID="imgCoPaymentFeeError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td valign="top" style="width: 200px" class="tableCellStyle">
            <asp:Label ID="lblTotalReasonVisitText" runat="server" Width="160px"></asp:Label></td>
        <td valign="top" class="tableCellStyle">
        </td>
    </tr>
</table>
<table cellpadding="0" cellspacing="0" width="980px">
    <tr style="display: block">
        <td colspan="2">
            <table cellpadding="0" cellspacing="0" border="1px">
                <!-- Header -->
                <tr align="center" style="background-color: #C0C0C0;">
                    <!-- Principal -->
                    <td>
                        <b>
                            <asp:Label ID="lblPrinicpal" runat="server"></asp:Label></b>
                    </td>
                    <!-- Secondary -->
                    <td id="tdReasonForVisitSecondaryHeader" runat="server">
                        <b>
                            <asp:Label ID="lblSecondary" runat="server"></asp:Label></b>
                    </td>
                </tr>
                <!-- Content -->
                <tr>
                    <!-- Principal -->
                    <td valign="top" style="padding: 1px; width: 465px">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <cc2:DropDownListNoValidation ID="ddlReasonVisitFirst" runat="server" AutoPostBack="False"
                                        Font-Size="12px" EnableViewState="true" Width="300px">
                                    </cc2:DropDownListNoValidation>
                                    <asp:Image ID="imgVisitReasonError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false" />
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitSecond" runat="server" AutoPostBack="False"
                                                    Width="453px" Font-Size="12px">
                                                </cc2:DropDownListNoValidation>
                                            </td>
                                        </tr>
                                    </table>
                                    <cc1:CascadingDropDown ID="cddReasonVisitFirst" runat="server" TargetControlID="ddlReasonVisitFirst"
                                        Category="ReasonVisitFirst" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL1"
                                        ContextKey="EN">
                                    </cc1:CascadingDropDown>
                                    <cc1:CascadingDropDown ID="cddReasonVisitSecond" runat="server" TargetControlID="ddlReasonVisitSecond"
                                        Category="ReasonVisitSecond" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL2"
                                        ParentControlID="ddlReasonVisitFirst" ContextKey="EN">
                                    </cc1:CascadingDropDown>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <!-- Secondary -->
                    <td id="tdReasonForVisitSecondaryContent" runat="server" valign="top" style="width: 460px">
                        <table id="tblReasonForVistS1" runat="server" cellpadding="2" cellspacing="0" style="padding-bottom: 10px;
                            display: block;">
                            <tbody>
                                <tr>
                                    <td style="height: 25px" valign="top">                                        
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitFirst_S1" runat="server" Width="300px"
                                                                    Font-Size="12px" />
                                                                <asp:Image ID="imgVisitReasonError_S1" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Height="17px" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitSecond_S1" runat="server" Width="453px"
                                                                    Font-Size="12px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="vertical-align:bottom; width: 18px">
                                                    <input type="image" runat="server" id="ibtnAdd_S1" src="<%$ Resources:ImageUrl, AddIBtn %>" title="<%$ Resources:AlternateText, AddIBtn %>"
                                                        onclick="AddReasonForVisit(); return false;" />
                                                </td>
                                                <td style="vertical-align:bottom; width: 18px">
                                                    <input type="image" runat="server" id="ibtnRemove_S1" src="<%$ Resources:ImageUrl, RemoveIBtn %>" title="<%$ Resources:AlternateText, RemoveIBtn %>"
                                                        style="display: block" />
                                                </td>
                                            </tr>
                                        </table>
                                        <cc1:CascadingDropDown ID="cddReasonVisitFirst_S1" runat="server" TargetControlID="ddlReasonVisitFirst_S1"
                                            Category="ReasonVisitFirst_S1" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL1"
                                            ContextKey="EN">
                                        </cc1:CascadingDropDown>
                                        <cc1:CascadingDropDown ID="cddReasonVisitSecond_S1" runat="server" TargetControlID="ddlReasonVisitSecond_S1"
                                            Category="ReasonVisitSecond_S1" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL2"
                                            ParentControlID="ddlReasonVisitFirst_S1" ContextKey="EN">
                                        </cc1:CascadingDropDown>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table id="tblReasonForVistS2" runat="server" cellpadding="2" cellspacing="0" style="padding-bottom: 10px;
                            display: none;">
                            <tbody>
                                <tr>
                                    <td style="height: 25px" valign="top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="white-space:nowrap;">
                                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitFirst_S2" runat="server" Width="300px"
                                                                    Font-Size="12px" />
                                                                <asp:Image ID="imgVisitReasonError_S2" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Height="17px" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitSecond_S2" runat="server" Width="453px"
                                                                    Font-Size="12px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="vertical-align:bottom; width: 18px">
                                                    <input type="image" runat="server" id="ibtnAdd_S2" src="<%$ Resources:ImageUrl, AddIBtn %>" title="<%$ Resources:AlternateText, AddIBtn %>"
                                                        onclick="AddReasonForVisit(); return false;" />
                                                </td>
                                                <td style="vertical-align:bottom; width: 18px">
                                                    <input type="image" runat="server" id="ibtnRemove_S2" src="<%$ Resources:ImageUrl, RemoveIBtn %>" title="<%$ Resources:AlternateText, RemoveIBtn %>" />
                                                </td>
                                            </tr>
                                        </table>
                                        <cc1:CascadingDropDown ID="cddReasonVisitFirst_S2" runat="server" TargetControlID="ddlReasonVisitFirst_S2"
                                            Category="ReasonVisitFirst_S2" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL1"
                                            ContextKey="EN">
                                        </cc1:CascadingDropDown>
                                        <cc1:CascadingDropDown ID="cddReasonVisitSecond_S2" runat="server" TargetControlID="ddlReasonVisitSecond_S2"
                                            Category="ReasonVisitSecond_S2" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL2"
                                            ParentControlID="ddlReasonVisitFirst_S2" ContextKey="EN">
                                        </cc1:CascadingDropDown>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table id="tblReasonForVistS3" runat="server" cellpadding="2" cellspacing="0" style="padding-bottom: 10px;
                            display: none;">
                            <tbody>
                                <tr>
                                    <td style="height: 25px" valign="top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="white-space:nowrap;">
                                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitFirst_S3" runat="server" Width="300px"
                                                                    Font-Size="12px" />
                                                                <asp:Image ID="imgVisitReasonError_S3" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Height="17px" Style="display: none" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td>
                                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitSecond_S3" runat="server" Width="453px"
                                                                    Font-Size="12px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="vertical-align:bottom; width: 18px">
                                                </td>
                                                <td style="vertical-align:bottom; width: 18px">
                                                    <input type="image" runat="server" id="ibtnRemove_S3" src="<%$ Resources:ImageUrl, RemoveIBtn %>" title="<%$ Resources:AlternateText, RemoveIBtn %>" />
                                                </td>
                                            </tr>                                            
                                        </table>
                                        <cc1:CascadingDropDown ID="cddReasonVisitFirst_S3" runat="server" TargetControlID="ddlReasonVisitFirst_S3"
                                            Category="ReasonVisitFirst_S3" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL1"
                                            ContextKey="EN">
                                        </cc1:CascadingDropDown>
                                        <cc1:CascadingDropDown ID="cddReasonVisitSecond_S3" runat="server" TargetControlID="ddlReasonVisitSecond_S3"
                                            Category="ReasonVisitSecond_S3" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL2"
                                            ParentControlID="ddlReasonVisitFirst_S3" ContextKey="EN">
                                        </cc1:CascadingDropDown>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <!-- List View-->
</table>
<input type="hidden" id="hidReasonForVisitCount" value="1" runat="server" />