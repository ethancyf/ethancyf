<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucInputHCVSChina.ascx.vb"
    Inherits="HCVU.ucInputHCVSChina" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc2" %>

<table cellpadding="0" cellspacing="0" style="width: 753px">
     <tr id="trExchangeRate" runat="server" style="height: 25px">
         <td valign="top" id="htmlCellExchangeRateTitle"  runat="server"  class="tableCellStyle">
            <asp:Label ID="lblExchangeRateTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, VoucherConversionRate%>"></asp:Label></td>
         <td valign="top" class="tableCellStyle" style="padding-left:2px">
            <asp:Label ID="lblExchangeRate" runat="server" CssClass="tableText"></asp:Label>                        
        </td>
    </tr>
    <tr style="height: 25px">
        <td valign="top" style="width: 198px" class="tableCellStyle">
            <asp:Label ID="lblAvailableVoucherText" runat="server" CssClass="tableTitle" Width="160px"></asp:Label></td>
        <td valign="top"  class="tableCellStyle"  style="padding-left:2px">
            <asp:Label ID="lblAvailableVoucher" runat="server" CssClass="tableText" Width="103px">$0</asp:Label>
            <asp:Label ID="lblHKDAvailableVoucher" runat="server" CssClass="tableText"></asp:Label>                                    
            <asp:TextBox ID="txthidExchangeRateValue" runat="server"  Style="display: none"></asp:TextBox>
        </td>        
    </tr>
    <tr>
        <td valign="top" style="width: 200px" class="tableCellStyle">            
            <asp:Label ID="lblVoucherRedeemText" runat="server" CssClass="tableTitle" Width="160px" Height="25px"></asp:Label>             
        </td>
        <td valign="top" style="padding-left:2px">
            <table cellpadding="0" cellspacing="0">
                <tbody>
                    <tr> <%-- 
                        <td valign="top" id="cellVoucherRedeem" runat="server">
                            <asp:RadioButtonList ID="rbVoucherRedeem" runat="server" AutoPostBack="True" RepeatDirection="Horizontal">
                            </asp:RadioButtonList></td>--%>                            
                          <td valign="top" class="tableCellStyle">
                              <asp:TextBox ID="txthidRedeemAmountHKD" runat="server"  Style="display: none"></asp:TextBox>
                              <asp:TextBox ID="txthidCurrencyMode" runat="server"  Style="display: none"></asp:TextBox>                            
                          <div id="divRMB" runat="server" >                                  
                            <asp:Label ID="lblRedeemAmountTitle" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>"></asp:Label>&nbsp;<asp:TextBox ID="txtRedeemAmount" runat="server" AutoCompleteType="Disabled" AutoPostBack="false"
                                MaxLength="2" Width="74px"></asp:TextBox>   
                              <cc1:FilteredTextBoxExtender ID="filtereditVoucherRedeem" runat="server" FilterType="Numbers, Custom" ValidChars="." 
                                TargetControlID="txtRedeemAmount">
                            </cc1:FilteredTextBoxExtender>&nbsp;&nbsp;
                            <asp:Label ID="lblVoucherRedeemHKD" runat="server" CssClass="tableText" Text="($0)"></asp:Label>                                  
                             <%-- <asp:ImageButton ID="ibtnChangeToInputHKD" runat="server"  ImageUrl="<%$ Resources:ImageUrl, ChangeCurrencyButton %>"
                                                        AlternateText="<%$ Resources:AlternateText, ChangeCurrencyButton %>" 
                                />       --%>                                              
                          </div>                                          
                          <div id ="divHKD"  runat="server"  >
                                <asp:Label ID="lbldivHKDRedeemAmountTitle" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>"></asp:Label>        
                                <asp:Label ID="lbldivHKDRedeemAmount" runat="server" CssClass="tableText" Width="89px"></asp:Label>                                 
                                <asp:Label ID="Label1" runat="server" CssClass="tableText" style="padding-left:1px">($&nbsp;</asp:Label><asp:TextBox ID="txtdivHKDRedeemAmount" runat="server" AutoCompleteType="Disabled" AutoPostBack="false"
                                Width="62px"></asp:TextBox>
                              <asp:Label ID="Label2" runat="server" CssClass="tableText">)</asp:Label>                                          
                              <%--    <asp:ImageButton ID="ibtnChangeToInputRMB" runat="server"  ImageUrl="<%$ Resources:ImageUrl, ChangeCurrencyButton %>"
                                                        AlternateText="<%$ Resources:AlternateText, ChangeCurrencyButton %>" 
                                 />--%>            
                               <cc1:FilteredTextBoxExtender ID="filtereditdivHKDVoucherRedeem" runat="server" FilterType="Numbers"
                                TargetControlID="txtdivHKDRedeemAmount">
                            </cc1:FilteredTextBoxExtender>          
                          </div>
                              
                          </td> 
                        <td valign="top" style="padding-left:3px;padding-right:3px">
                            <asp:Image ID="imgVoucherRedeemError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />    
                        </td>                                                                                                                                                                                             
                        <td valign="top" style="padding-left:2px;">
                            <%--There is function to allow flexibiliy of input in HKD or RMB in initial user requirment
                                user confirm to remove it later
                                the code below is kept for in case the user need it back later 
                                in code behind, the code commented with remark 'Change Currency Mode
                                --%>  
                            <%-- 
							<asp:CheckBox ID="chkChangeCurrencyMode" runat="server" text="<%$ Resources:Text, InputByHKD%>"
                                CssClass="tableTitle" 
                                onmouseover="this.style.cursor='hand'" onmouseout="this.style.cursor='default'" onchange="return false;" Font-Size="medium"/>
								</div>--%>
                            </td>
                    </tr>
                </tbody>
            </table>
        </td>
    </tr>
    <tr id="trCoPaymentFee" runat="server">
        <td valign="top" style="width: 175px;" class="tableCellStyle">
            <asp:Label ID="lblCoPaymentFee" runat="server" CssClass="tableTitle"></asp:Label></td>
        <td valign="top" class="tableCellStyle"  style="padding-left:2px">
            <table cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Label ID="Label3" runat="server" CssClass="tableText" Text="<%$ Resources:Text, RMBDollarSign%>"></asp:Label>&nbsp;<asp:TextBox
                            ID="txtCoPaymentFee" runat="server" AutoCompleteType="Disabled" AutoPostBack="False"
                            MaxLength="6" Width="74px"></asp:TextBox>
                        <cc1:FilteredTextBoxExtender ID="FilteredCoPaymentFee" runat="server" FilterType="Custom,Numbers"
                            ValidChars="." TargetControlID="txtCoPaymentFee">
                        </cc1:FilteredTextBoxExtender>
                    </td>
                   <%--  <td  valign="top" style="padding-left: 4px;">
                       <asp:Label ID="lblCoPaymentFeeHKD" runat="server" CssClass="tableText" Text="($0)"></asp:Label>                                            
                   </td>--%>
                    <td valign="top" style="padding-left: 3px;">
                        <asp:Image ID="imgCoPaymentFeeError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
 <%--   <tr>
        <td valign="top" style="width: 200px" class="tableCellStyle">
            <asp:Label ID="lblTotalReasonVisitText" runat="server" CssClass="tableTitle" Width="160px"></asp:Label></td>
        <td valign="top" class="tableCellStyle">
        </td>
    </tr>--%>
</table>

<table cellpadding="0" cellspacing="0">
      <tr id="trPaymentType" runat="server">
              <td valign="top" id="htmlCellPaymentType"  runat="server" class="tableCellStyle">
            <asp:Label ID="lblPaymentTypeTitle" runat="server" CssClass="tableTitle"></asp:Label></td>
            <td valign="top" style="padding-top: 3px;">
                  <asp:DropDownList ID="ddlPaymentType" runat="server" Width="230px" AutoPostBack="false"></asp:DropDownList>
            </td>   
             <td valign="top" style="padding-left: 4px;">
                        <asp:Image ID="imgPaymentTypeError" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" />
                    </td>
      </tr>
</table>

<table cellpadding="0" cellspacing="0" width="980px">
    <tr style="display: block">
           <td valign="top" style="width: 200px" class="tableCellStyle">
            <asp:Label ID="lblTotalReasonVisitText" runat="server" CssClass="tableTitle" Width="160px"></asp:Label></td>
        <td valign="top" class="tableCellStyle">
        </td>
        <td colspan="2"  style="padding-top: 3px;">
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
                    <td valign="top" style="padding: 1px; width: 305px">
                        <table cellpadding="0" cellspacing="0">
                            <tr>
                                <td>
                                    <cc2:DropDownListNoValidation ID="ddlReasonVisitFirst" runat="server" AutoPostBack="False"
                                        Font-Size="12px" EnableViewState="true" Width="300px" onchange="">
                                    </cc2:DropDownListNoValidation>
                                    </td>
                                <td>
                                    <asp:Image ID="imgVisitReasonError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="false" /></td>
                                   <%-- <table cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <cc2:DropDownListNoValidation ID="ddlReasonVisitSecond" runat="server" AutoPostBack="False"
                                                    Width="453px" Font-Size="12px">
                                                </cc2:DropDownListNoValidation>
                                            </td>
                                        </tr>
                                    </table>--%>
                                    <cc1:CascadingDropDown ID="cddReasonVisitFirst" runat="server" TargetControlID="ddlReasonVisitFirst"
                                        Category="HCVSChinaReasonVisitFirst" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL1"
                                        ContextKey="EN">
                                    </cc1:CascadingDropDown>
                                    <%--   <cc1:CascadingDropDown ID="cddReasonVisitSecond" runat="server" TargetControlID="ddlReasonVisitSecond"
                                        Category="" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL2"
                                        ParentControlID="ddlReasonVisitFirst" ContextKey="EN">
                                    </cc1:CascadingDropDown>--%>
                                </td>
                            </tr>
                        </table>
                    </td>
                    <!-- Secondary -->
                    <td id="tdReasonForVisitSecondaryContent" runat="server" valign="top" style="width: 336px">
                        <table id="tblReasonForVistS1" runat="server" cellpadding="2" cellspacing="0" style="padding-bottom: 10px;
                            display: block;">
                            <tbody>
                                <tr>
                                    <td style="height: 15px" valign="top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <%--CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]--%>
                                                    <cc2:DropDownListNoValidation ID="ddlReasonVisitFirst_S1" runat="server" Width="300px"
                                                        Font-Size="12px" onchange="" />
                                                    <%--CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]--%>
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgVisitReasonError_S1" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Height="17px" Style="display: none" />
                                                </td>
                                                <td >
                                                    <input type="image" runat="server" id="ibtnAdd_S1" src="<%$ Resources:ImageUrl, AddIBtn %>" title="<%$ Resources:AlternateText, AddIBtn %>"
                                                        onclick="AddReasonForVisit(); return false;" />
                                                </td>
                                                <td >
                                                    <input type="image" runat="server" id="ibtnRemove_S1" src="<%$ Resources:ImageUrl, RemoveIBtn %>" title="<%$ Resources:AlternateText, RemoveIBtn %>"
                                                        style="display: block" />
                                                </td>

                                            </tr>
                                        </table>
                                  <%--      <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <cc2:DropDownListNoValidation ID="ddlReasonVisitSecond_S1" runat="server" Width="453px"
                                                        Font-Size="12px" />
                                                </td>
                                                <td style="width: 18px">
                                                    <input type="image" runat="server" id="ibtnAdd_S1" src="<%$ Resources:ImageUrl, AddIBtn %>" title="<%$ Resources:AlternateText, AddIBtn %>"
                                                        onclick="AddReasonForVisit(); return false;" />
                                                </td>
                                                <td style="width: 18px">
                                                    <input type="image" runat="server" id="ibtnRemove_S1" src="<%$ Resources:ImageUrl, RemoveIBtn %>" title="<%$ Resources:AlternateText, RemoveIBtn %>"
                                                        style="display: block" />
                                                </td>
                                            </tr>
                                        </table>--%>
                                        <cc1:CascadingDropDown ID="cddReasonVisitFirst_S1" runat="server" TargetControlID="ddlReasonVisitFirst_S1"
                                            Category="HCVSChinaReasonVisitFirst_S1" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL1"
                                            ContextKey="EN">
                                        </cc1:CascadingDropDown>
    <%--                                    <cc1:CascadingDropDown ID="cddReasonVisitSecond_S1" runat="server" TargetControlID="ddlReasonVisitSecond_S1"
                                            Category="" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL2"
                                            ParentControlID="ddlReasonVisitFirst_S1" ContextKey="EN">
                                        </cc1:CascadingDropDown>--%>
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
                                                    <%--CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]--%>
                                                    <cc2:DropDownListNoValidation ID="ddlReasonVisitFirst_S2" runat="server" Width="300px"
                                                        Font-Size="12px" onchange="" />
                                                    <%--CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]--%>
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgVisitReasonError_S2" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Height="17px" Style="display: none" />
                                                </td>
                                              <td >
                                                    <input type="image" runat="server" id="ibtnAdd_S2" src="<%$ Resources:ImageUrl, AddIBtn %>" title="<%$ Resources:AlternateText, AddIBtn %>"
                                                        onclick="AddReasonForVisit(); return false;" />
                                                </td>
                                                <td >
                                                    <input type="image" runat="server" id="ibtnRemove_S2" src="<%$ Resources:ImageUrl, RemoveIBtn %>" title="<%$ Resources:AlternateText, RemoveIBtn %>" />
                                                </td>
                                            </tr>
                                        </table>
                                       <%-- <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <cc2:DropDownListNoValidation ID="ddlReasonVisitSecond_S2" runat="server" Width="453px"
                                                        Font-Size="12px" />
                                                </td>
                                                <td style="width: 18px">
                                                    <input type="image" runat="server" id="ibtnAdd_S2" src="<%$ Resources:ImageUrl, AddIBtn %>" title="<%$ Resources:AlternateText, AddIBtn %>"
                                                        onclick="AddReasonForVisit(); return false;" />
                                                </td>
                                                <td style="width: 18px">
                                                    <input type="image" runat="server" id="ibtnRemove_S2" src="<%$ Resources:ImageUrl, RemoveIBtn %>" title="<%$ Resources:AlternateText, RemoveIBtn %>" />
                                                </td>
                                            </tr>
                                        </table>--%>
                                        <cc1:CascadingDropDown ID="cddReasonVisitFirst_S2" runat="server" TargetControlID="ddlReasonVisitFirst_S2"
                                            Category="HCVSChinaReasonVisitFirst_S2" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL1"
                                            ContextKey="EN">
                                        </cc1:CascadingDropDown>
                                    <%--    <cc1:CascadingDropDown ID="cddReasonVisitSecond_S2" runat="server" TargetControlID="ddlReasonVisitSecond_S2"
                                            Category="" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL2"
                                            ParentControlID="ddlReasonVisitFirst_S2" ContextKey="EN">
                                        </cc1:CascadingDropDown>--%>
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
                                                    <%--CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]--%>
                                                    <cc2:DropDownListNoValidation ID="ddlReasonVisitFirst_S3" runat="server" Width="300px"
                                                        Font-Size="12px" onchange="" />
                                                    <%--CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]--%>
                                                </td>
                                                <td>
                                                    <asp:Image ID="imgVisitReasonError_S3" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Height="17px" Style="display: none" />
                                                </td>
                                                <td >
                                                    <input type="image" runat="server" id="ibtnRemove_S3" src="<%$ Resources:ImageUrl, RemoveIBtn %>" title="<%$ Resources:AlternateText, RemoveIBtn %>" />
                                                </td>
                                            </tr>
                                        </table>
                                      <%--  <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <cc2:DropDownListNoValidation ID="ddlReasonVisitSecond_S3" runat="server" Width="453px"
                                                        Font-Size="12px" />
                                                </td>
                                                <td style="width: 18px">
                                                </td>
                                                <td style="width: 18px">
                                                    <input type="image" runat="server" id="ibtnRemove_S3" src="<%$ Resources:ImageUrl, RemoveIBtn %>" title="<%$ Resources:AlternateText, RemoveIBtn %>" />
                                                </td>
                                            </tr>
                                        </table>--%>
                                        <cc1:CascadingDropDown ID="cddReasonVisitFirst_S3" runat="server" TargetControlID="ddlReasonVisitFirst_S3"
                                            Category="HCVSChinaReasonVisitFirst_S3" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL1"
                                            ContextKey="EN">
                                        </cc1:CascadingDropDown>
                                        <%--<cc1:CascadingDropDown ID="cddReasonVisitSecond_S3" runat="server" TargetControlID="ddlReasonVisitSecond_S3"
                                            Category="" PromptText="Please select" LoadingText="Please select" ServiceMethod="GetReasonForVisitL2"
                                            ParentControlID="ddlReasonVisitFirst_S3" ContextKey="EN">
                                        </cc1:CascadingDropDown>--%>
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