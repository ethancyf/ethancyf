<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHCVS.ascx.vb"
    Inherits="HCSP.ucInputHCVS" %>
<%@ Register Src="~/UIControl/Assessories/ucNumPad.ascx" TagName="ucNumPad" TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc1" %>
<%@ Register Assembly="HCSP" Namespace="HCSP" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<script type="text/javascript">
  
</script>
<table cellpadding="0" cellspacing="0" style="width: 753px" >
    <asp:Panel ID="panDHCRelatedService" runat="server">
        <tr id="trDHCRelatedService" runat="server">
            <td valign="top" class="tableCellStyle">
                <asp:Label ID="lblDHCRelatedServiceTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DHCRelatedService%>" />
            </td>
            <td valign="top" class="tableCellStyle">
                <table id="tblDHCRelatedServiceRead" runat="server" cellpadding="0" cellspacing="0" style="display: none">
                    <tr>
                        <td>
                            <asp:Label ID="lblDHCRelatedService" runat="server" CssClass="tableText"></asp:Label>
                        </td>
                    </tr>
                </table>                           
                <table id="tblDHCRelatedServiceWrite" runat="server" cellpadding="0" cellspacing="0" style="display: block">
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkDHCRelatedService" runat="server" CssClass="tableText" Style="position:relative;left:-2px"></asp:CheckBox>
                        </td>
                    </tr>
                </table>
            </td>   
        </tr>
    </asp:Panel>
    <tr id="trAvailableVoucher" runat="server">
        <td valign="top" id="htmlCellAvailableVoucher" runat="server" class="tableCellStyle">
            <asp:Label ID="lblAvailableVoucherText" runat="server" CssClass="tableTitle"></asp:Label></td>
        <td valign="top" class="tableCellStyle">
            <asp:Label ID="lblAvailableVoucher" runat="server" CssClass="tableText">$0</asp:Label></td>
    </tr>
    <asp:Panel ID="pnlAvailableQuota" runat="server" Visible="false">
    <tr>
        <td id="htmlCellAvailableQuota" runat="server" style="vertical-align:top" class="tableCellStyle">
            <table style="padding:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top;width: 10px; padding-left: 6px;">
                        -         
                    </td>
                    <td>
                        <asp:Label ID="lblAvailableQuotaText" runat="server" CssClass="tableTitle" />
                    </td>
                </tr>
            </table>
        </td>
        <td style="vertical-align:middle" class="tableCellStyle">
            <asp:Label ID="lblAvailableQuota" runat="server" CssClass="tableText" />
            <asp:Label ID="lblAvailableQuotaUpTo" runat="server" CssClass="tableText" />
        </td>
    </tr>
    <tr>
        <td id="htmlCellMaximumVoucherAmount" runat="server" style="vertical-align:top" class="tableCellStyle">
            <table style="padding:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top;width: 10px; padding-left: 6px;">
                        -         
                    </td>
                    <td>
                        <asp:Label ID="lblMaximumVoucherAmountText" runat="server" CssClass="tableTitle" />
                        <asp:Label ID="lblMaximumVoucherAmountText_Chi" runat="server" CssClass="tableTitle" style="width:160px;display:inline-block"/>
                    </td>
                </tr>
            </table>
        </td>
        <td style="vertical-align:middle" class="tableCellStyle">
            <asp:Label ID="lblMaximumVoucherAmount" runat="server" CssClass="tableText" />
        </td>
    </tr>
    </asp:Panel>
    <tr>
        <td valign="middle" id="htmlCellVoucherRedeem"  runat="server"  class="tableCellStyle">
            <asp:Label ID="lblVoucherRedeemText" runat="server" CssClass="tableTitle"  ></asp:Label>&nbsp;<asp:Image ID="imgOneDollar" runat="server" EnableViewState="true"  Height="18px" Width="18px" ImageUrl="<%$ Resources:ImageUrl, OneDollar %>" ImageAlign="bottom"/>
            </td>
        <td valign="top" class="tableCellStyle">
            <table id="tblVoucherRedeemRead" runat="server" cellpadding="0" cellspacing="0" style="display: none">
                <tr>
                    <td>
                        <asp:Label ID="lblVoucherRedeem" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
            <table id="tblVoucherRedeemWrite" runat="server" cellpadding="0" cellspacing="0"
                style="display: block;">
                <tr>
                    <td valign="top">
                        <asp:Label ID="Label1" runat="server" CssClass="tableText">$</asp:Label>
                        <asp:TextBox ID="txtRedeemAmount" runat="server" Width="42px" AutoPostBack="false" MaxLength="2" ></asp:TextBox>
                        <asp:Button ID="DummyNoticeTargetControl" runat="server" Style="display: none" />
                        <cc1:ModalPopupExtender ID="ModalPopupExtenderNotice" runat="server" TargetControlID="DummyNoticeTargetControl"
                            PopupControlID="panNotice" BehaviorID="mdlPopup1" BackgroundCssClass="modalBackgroundTransparent"
                            DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
                        </cc1:ModalPopupExtender>
                        <cc1:FilteredTextBoxExtender ID="filtereditTotalAmount" runat="server" FilterType="numbers"
                            TargetControlID="txtRedeemAmount">
                        </cc1:FilteredTextBoxExtender>  
                    </td>
                       <td valign="top">
                        <asp:Image ID="imgVoucherRedeemError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" /></td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<table cellpadding="0" cellspacing="0">
    <tr style="display: none;">
        <td valign="top" style="width: 200px" class="tableCellStyle">
            <asp:Label ID="lblTotalAmountText" runat="server" CssClass="tableTitle" Width="160px"></asp:Label></td>
        <td valign="top" class="tableCellStyle"  id="htmlCellTotalAmount"  runat="server" >
            <asp:Label ID="lblTotalAmount" runat="server" CssClass="tableText">$0</asp:Label></td>
    </tr>

    <tr id="trCoPaymentFee" runat="server">
        <td valign="top" id="htmlCellCopaymentFee"  runat="server" class="tableCellStyle">
            <asp:Label ID="lblCoPaymentFeeText" runat="server" CssClass="tableTitle"></asp:Label></td>
        <td valign="top" class="tableCellStyle">
            <table id="tblCopaymentFeeRead" runat="server" cellpadding="0" cellspacing="0" style="display: none">
                <tr>
                    <td>
                        <asp:Label ID="lblCopaymentFee" runat="server" CssClass="tableText"></asp:Label>
                    </td>
                </tr>
            </table>
            <table id="tblCopaymentFeeWrite" runat="server" cellpadding="0" cellspacing="0">
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
                        <asp:ImageButton ID="ibtnNumPad" runat="server" ImageUrl="<%$ Resources:ImageUrl, CoPaymentFeeCalculatorBtn %>"
                            AlternateText="<%$ Resources:AlternateText, CoPaymentFeeCalculatorBtn %>" ToolTip="<%$ Resources:AlternateText, CoPaymentFeeCalculatorBtn %>"/></td>
                    <td>
                        <asp:Image ID="imgCoPaymentFeeError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
                    </td>
                </tr>
            </table>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderNumPad" runat="server" TargetControlID="ibtnNumPad"
                PopupControlID="panNumPad" BehaviorID="mdlPopup2" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
        </td>
    </tr>
    <tr>
        <td colspan="2" id="htmlCellTotalReasonVisitText"  runat="server" valign="top" style="width: 200px;" class="tableCellStyle">
            <asp:Label ID="lblTotalReasonVisitText" runat="server" CssClass="tableTitle" Width="160px"
                Visible="true" />
        </td>
    </tr>

    <%-- Table View--%>
</table>
<table cellpadding="0" cellspacing="0" width="920px">
    <tr>
        <td colspan="2">
            <table cellpadding="0" cellspacing="0" border="1px">
                <%-- Header --%>
                <tr align="center" style="background-color: #C0C0C0;">
                    <%-- Principal --%>
                    <td>
                        <b>
                            <asp:Label ID="lblPrinicpal" runat="server"></asp:Label></b>
                    </td>
                    <%-- Secondary --%>
                    <td id="tdReasonForVisitSecondaryHeader" runat="server">
                        <b>
                            <asp:Label ID="lblSecondary" runat="server"></asp:Label></b>
                    </td>
                </tr>
                <%-- Content --%>
                <tr>
                    <%-- Principal --%>
                    <td valign="top" style="padding: 1px; width: 460px">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="height: 25px" valign="top">
                                    <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitFirst" runat="server" AutoPostBack="False"
                                                    Font-Size="12px" EnableViewState="true" Width="300px">
                                                </cc2:DropDownListNoValidation>
                                                <asp:Image ID="imgVisitReasonError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                  ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Height="17px"/>
                                            </td>
                                        </tr>
                                    </table>
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
                                        Category="" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL1"
                                        ContextKey="EN">
                                    </cc1:CascadingDropDown>
                                    <cc1:CascadingDropDown ID="cddReasonVisitSecond" runat="server" TargetControlID="ddlReasonVisitSecond"
                                        ServiceMethod="GetReasonForVisitL2" ParentControlID="ddlReasonVisitFirst">
                                    </cc1:CascadingDropDown>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <%-- Secondary --%>
                    <td id="tdReasonForVisitSecondaryContent" runat="server" valign="top" style="width: 460px">
                        <table id="tblReasonForVistS1" runat="server" cellpadding="2" cellspacing="0" style="padding-bottom: 10px;
                            ">
                            <tbody>
                                <tr>
                                    <td style="height: 25px" valign="top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr style="vertical-align:bottom">
                                                            <td>
                                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitFirst_S1" runat="server" Width="300px"
                                                                    Font-Size="12px"/>
                                                                <asp:Image ID="imgVisitReasonError_S1" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Height="17px" style="display:none"/>
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
                                                <td style="vertical-align:bottom">
                                                    <input type="image" runat="server" id="ibtnAdd_S1" src="<%$ Resources:ImageUrl, AddIBtn %>" title="<%$ Resources:AlternateText, AddIBtn %>"
                                                        onclick="AddReasonForVisit(); return false;"/>
                                                </td>
                                                <td style="vertical-align:bottom; width: 18px">
                                                    <input type="image" runat="server" id="ibtnRemove_S1" src="<%$ Resources:ImageUrl, RemoveIBtn %>" title="<%$ Resources:AlternateText, RemoveIBtn %>" />
                                                </td>
                                            </tr>
                                        </table>
                                        <cc1:CascadingDropDown ID="cddReasonVisitFirst_S1" runat="server" TargetControlID="ddlReasonVisitFirst_S1"
                                            Category="" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL1" 
                                            ContextKey="EN">
                                        </cc1:CascadingDropDown>
                                        <cc1:CascadingDropDown ID="cddReasonVisitSecond_S1" runat="server" TargetControlID="ddlReasonVisitSecond_S1"
                                            ServiceMethod="GetReasonForVisitL2" ParentControlID="ddlReasonVisitFirst_S1">
                                        </cc1:CascadingDropDown>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table id="tblReasonForVistS2" runat="server" cellpadding="2" cellspacing="0" style="padding-bottom: 10px;"
                            >
                            <tbody>
                                <tr>
                                    <td style="height: 25px" valign="top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr style="vertical-align:bottom">
                                                            <td>
                                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitFirst_S2" runat="server" Width="300px"
                                                                    Font-Size="12px"/>
                                                                <asp:Image ID="imgVisitReasonError_S2" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Height="17px" style="display:none" />
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
                                                <td style="vertical-align:bottom">
                                                    <input type="image" runat="server" id="ibtnAdd_S2" src="<%$ Resources:ImageUrl, AddIBtn %>" title="<%$ Resources:AlternateText, AddIBtn %>"
                                                        onclick="AddReasonForVisit(); return false;" />
                                                </td>
                                                <td style="vertical-align:bottom; width: 18px">
                                                    <input type="image" runat="server" id="ibtnRemove_S2" src="<%$ Resources:ImageUrl, RemoveIBtn %>" title="<%$ Resources:AlternateText, RemoveIBtn %>"/>
                                                </td>
                                            </tr>
                                        </table>
                                        <cc1:CascadingDropDown ID="cddReasonVisitFirst_S2" runat="server" TargetControlID="ddlReasonVisitFirst_S2"
                                            Category="" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL1"
                                            ContextKey="EN">
                                        </cc1:CascadingDropDown>
                                        <cc1:CascadingDropDown ID="cddReasonVisitSecond_S2" runat="server" TargetControlID="ddlReasonVisitSecond_S2"
                                            ServiceMethod="GetReasonForVisitL2" ParentControlID="ddlReasonVisitFirst_S2">
                                        </cc1:CascadingDropDown>
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                        <table id="tblReasonForVistS3" runat="server" cellpadding="2" cellspacing="0" style="padding-bottom: 10px;">
                            <tbody>
                                <tr>
                                    <td style="height: 25px" valign="top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitFirst_S3" runat="server" Width="300px"
                                                                    Font-Size="12px"/>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgVisitReasonError_S3" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Height="17px" style="display:none"/>
                                                            </td>
                                                        </tr>
                                                        <tr valign="top">
                                                            <td>
                                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitSecond_S3" runat="server" Width="453px"
                                                                    Font-Size="12px" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="vertical-align:bottom; width: 18px">
                                                    <input type="image" runat="server" id="ibtnRemove_S3" src="<%$ Resources:ImageUrl, RemoveIBtn %>" title="<%$ Resources:AlternateText, RemoveIBtn %>"/>
                                                </td>                                                
                                            </tr>
                                        </table>
                                        <table cellpadding="0" cellspacing="0">

                                        </table>
                                        <cc1:CascadingDropDown ID="cddReasonVisitFirst_S3" runat="server" TargetControlID="ddlReasonVisitFirst_S3"
                                            Category="" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL1"
                                            ContextKey="EN">
                                        </cc1:CascadingDropDown>
                                        <cc1:CascadingDropDown ID="cddReasonVisitSecond_S3" runat="server" TargetControlID="ddlReasonVisitSecond_S3"
                                            ServiceMethod="GetReasonForVisitL2" ParentControlID="ddlReasonVisitFirst_S3">
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
    <%-- List View--%>
</table>
<input type="hidden" id="hidReasonForVisitCount" value="1" runat="server" />
<asp:Panel ID="panNumPad" runat="server" Style="display: none">
    <uc1:ucNumPad ID="ucNumPad" runat="server"/>
</asp:Panel>
<asp:Panel ID="panNotice" runat="server" Style="display: none" Width="500px">
    <uc1:ucNoticePopUp ID="ucNoticePopUp" runat="server" NoticeMode="Notification" ButtonMode="OK" />
</asp:Panel>
