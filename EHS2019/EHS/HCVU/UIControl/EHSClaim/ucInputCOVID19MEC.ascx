<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputCOVID19MEC.ascx.vb" Inherits="HCVU.ucInputCOVID19MEC" %>
<%@ Register Assembly="HCVU" Namespace="HCVU" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
    <tr>
        <td colspan="2" style="width:205px;vertical-align:top;padding-top:15px">
            <span style="color:red;padding-right:3px">*</span>
            <asp:Label ID="lblP1MedicalReasonHeadingText" runat="server" CssClass="tableText" Height="25px" style="white-space:nowrap"
                 Text ="<%$ Resources:Text, MedicalExemptionsPartI%>" />
            <asp:Image ID="imgP1MedicalReasonError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" />
        </td>
    </tr>
    <tr>
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblP1MedicalReasonText" runat="server" Height="25px" Width="160px" Text ="<%$ Resources:Text, MedicalReason%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <table ID="tblP1MedicalReason" runat="server" style="width:680px;position:relative;top:-3px;left:-5px;font-size:16px" />
                        <asp:checkbox ID="chkP1ProceedToPart2" AutoPostBack ="true" runat="server" style="position:relative;left:-4px" 
                            Text ="" onclientclick="" />  
                        <asp:Label ID="lblP1ProceedToPart2" runat="server" AssociatedControlID ="chkP1ProceedToPart2" style="position:relative;left:-5px;top:-2px;font-size:16px"
                             Text ="<%$ Resources:Text, MedicalExemptionsProceedToPartII%>" />                                      
                    </td>
                </tr>
            </table>
        </td>
    </tr>
<%--    <tr>
        <td colspan="2" style="width:205px;vertical-align:top">

        </td>
    </tr>--%>
    <tr id="trP2MedicalReasonHeading" runat="server">
        <td colspan="2" style="width:205px;vertical-align:top;padding-top:15px">
            <asp:Label ID="lblP2MedicalReasonHeadingText" runat="server" CssClass="tableText" Height="25px" Width="820px" Text ="<%$ Resources:Text, MedicalExemptionsPartII%>" />
        </td>
    </tr>
    <tr id="trP2MedicalReasonBioNTechHeading" runat="server">
        <td style="width:205px;vertical-align:top">
            <span style="color:red;padding-right:3px">*</span><asp:Label ID="lblP2MedicalReasonBioNTechHeadingText" runat="server" CssClass="tableText" Height="25px" Text ="<%$ Resources:Text, MedicalExemptionsBioNTech%>" />
        </td>
        <td style="vertical-align:top">
            <asp:Image ID="imgP2MedicalReasonBioNTechError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" />
        </td>
    </tr>
    <tr id="trP2MedicalReasonBioNTech" runat="server">
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblP2MedicalReasonBioNTechText" runat="server" Height="25px" Width="160px" Text ="<%$ Resources:Text, MedicalReason%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <table ID="tblP2MedicalReasonBioNTech" runat="server" style="width:680px;position:relative;top:-3px;left:-5px;font-size:16px" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trAND" runat="server">
        <td colspan="2" style="width:205px;vertical-align:top">
            <span style="text-transform:uppercase"><asp:Label ID="lblAnd" runat="server" CssClass="tableText" Height="25px" style="text-decoration:underline" Text ="<%$ Resources:Text, ConjunctionAnd2%>" /></span>
        </td>
    </tr>
    <tr id="trP2MedicalReasonSinovacHeading" runat="server">
        <td style="width:205px;vertical-align:top">
            <span style="color:red;padding-right:3px">*</span><asp:Label ID="lblP2MedicalReasonSinovacHeadingText" runat="server" CssClass="tableText" Height="25px" Text ="<%$ Resources:Text, MedicalExemptionsSinovac%>" />
        </td>
        <td style="vertical-align:top">
            <asp:Image ID="imgP2MedicalReasonSinovacError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                ImageAlign="Top" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" style="position:relative;top:-3px" />
        </td>
    </tr>
    <tr id="trP2MedicalReasonSinovac" runat="server">
        <td style="width:205px;vertical-align:top">
            <asp:Label ID="lblP2MedicalReasonSinovacText" runat="server" Height="25px" Width="160px" Text ="<%$ Resources:Text, MedicalReason%>" />
        </td>
        <td style="vertical-align:top">
            <table style="border:0px;padding:0px;border-spacing:0px;border-collapse:collapse">
                <tr>
                    <td style="vertical-align:top">
                        <table ID="tblP2MedicalReasonSinovac" runat="server" style="width:680px;position:relative;top:-3px;left:-5px;font-size:16px" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="2" style="vertical-align:top;padding-top:15px;padding-bottom:10px">
            <asp:Label ID="lblDeclaration" runat="server" CssClass="tableText" Text ="<%$ Resources:Text, MedicalExemptionsDeclaration%>" style="min-height:25px;width:930px" />
        </td>
    </tr>
    <tr>
        <td style="width:205px;vertical-align:top;padding-bottom:5px">
            <asp:Label ID="lblValidUntilText" runat="server" CssClass="tableText" style="top: 1px; position: relative;width:205px;min-height:25px" Text ="<%$ Resources:Text, MedicalExemptionsValidUntil%>" />
        </td>
        <td style="vertical-align: top">
            <table style="border-collapse: collapse; border-spacing: 0px 0px; margin: 0px">
                <tr>
                    <td style="vertical-align: top; padding-left: 0px">
                        <asp:TextBox ID="txtValidUntil" runat="server" Width="100px" Height="16" 
                            MaxLength="11" AutoPostBack="false"
                            onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);"
                            onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);"
                            onblur="filterDateInput(this);" style="position:relative;left:0px;top:-1px" />
                        <cc2:CalendarExtender runat="server" ID="ceValidUntil" CssClass="ajax_cal" PopupPosition="BottomLeft" TargetControlID="txtValidUntil"
                            PopupButtonID="btnValidUntilCal" Format="dd-MMM-yyyy" TodaysDateFormat="d MMMM, yyyy"
                            
                            /><%--OnClientShown ="CalendarShown" OnClientHidden="CalendarHidden"--%>
                        <cc2:FilteredTextBoxExtender ID="ftbeValidUntil" runat="server" TargetControlID="txtValidUntil"
                            FilterType="Custom, Numbers,UppercaseLetters, LowercaseLetters" ValidChars="-"></cc2:FilteredTextBoxExtender>
                    </td>
                    <td style="padding-left: 5px; vertical-align: central">
                        <asp:ImageButton ID="btnValidUntilCal" runat="server" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" ImageAlign="AbsMiddle" Style="vertical-align: top;position:relative;left:-2px;top:-1px"></asp:ImageButton>
                    </td>
                    <td>
                        <asp:Button ID="btnValidUntilRemark" runat="server" BorderWidth="0" BorderStyle="None" Width="17px" Height="17px"
                            AlternateText="<%$ Resources:Text, Remarks%>" 
                            style="vertical-align:top;position:relative;left:2px;top:-1px;background-color: transparent;background-image:url('../Images/others/info.png');background-repeat: no-repeat;"
                            onmouseover="this.style.cursor='pointer'" />
                    </td>
                    <td style="padding-left: 5px; vertical-align: top">
                        <asp:Image ID="imgValidUntilError" runat="server" Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorBtn%>"
                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="AbsMiddle" Style="vertical-align: top"></asp:Image>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr id="trJoinEHRSS" runat="server">
        <td style="width:200px;height:25px;vertical-align:top;padding-top:5px;padding-bottom:5px">
            <asp:Label ID="lblJoinEHRSSText" runat="server" CssClass="tableText" Text="<%$ Resources:Text, JoinEHRSS%>" Width="160px" />
        </td>
        <td style="height:25px;vertical-align:top;padding-bottom:5px">
            <asp:checkbox ID="chkJoinEHRSS" runat="server" AutoPostBack="false" style="position:relative;left:-5px"/>
        </td>
    </tr>
    <tr id="trContactNo" runat="server">
        <td style="width:200px;height:25px;vertical-align:top">
            <asp:Label ID="lblContactNoText" runat="server" CssClass="tableText" Text="<%$ Resources:Text, MobileContactNo%>" Width="160px" />
        </td>
        <td style="height:25px;vertical-align:top">
            <asp:textbox ID="txtContactNo" runat="server" MaxLength="8" style="position:relative;left:-1px;top:-2px" Width="100px"/>
            <asp:ImageButton ID="imgContactNoError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" ImageAlign="Top" visible="false" />
            <asp:Label ID="lblContactNoRecommendation" runat="server" Text="<%$ Resources:Text, MedicalExemptionsJoinEHRSS_3%>" style="font-size:14px" />
            <cc2:FilteredTextBoxExtender ID="fteContactNo" runat="server" TargetControlID="txtContactNo"
                                        FilterType="Numbers" />
        </td>
    </tr>
</table>

<asp:Panel Style="display: none" ID="panValidUntilRemark" runat="server">
    <asp:Panel ID="panValidUntilRemarkHeading" runat="server" Style="cursor: move;">
        <table border="0" cellpadding="0" cellspacing="0" style="width: 680px">
            <tr>
                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                    <asp:Label ID="lblpanOtherVaccinationRecordRemarkHeading" runat="server" Text="<%$ Resources:Text, Remarks %>" />
                </td>
                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
            </tr>
        </table>
    </asp:Panel>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 680px">
        <tr>
            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
            <td style="text-align:left;background-color: #ffffff; padding: 0px 0px 5px 10px">
                <asp:Panel ID="panOtherVaccinationRecordRemarkContent" runat="server" ScrollBars="None">
                    <div id="divOtherVaccinationRecordRemarkTitle" runat="server" style="width: 620px; margin: 14px 2px 0px 2px">
                    </div>
                    <div id="divOtherVaccinationRecordRemark" runat="server" style="width: 620px; margin: 6px 2px 2px 2px">
                        <asp:Label ID="lblpanOtherVaccinationRecordRemarkContent" runat="server" Text="<%$ Resources:Text, MedicalExemptionValidUntilRemark %>" />                                    
                    </div>
                </asp:Panel>
            </td>
            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
        </tr>
        <tr>
            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
            <td style="height: 30px; background-color: #ffffff;text-align:center;vertical-align:middle">
                <asp:ImageButton ID="btnValidUntilRemarkClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" /></td>
            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
        </tr>
        <tr>
            <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
            <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
            <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
        </tr>
    </table>
</asp:Panel>

<%-- Popup for Valid Until Remark --%>
<cc2:ModalPopupExtender ID="mpeValidUntilRemark" runat="server" BackgroundCssClass="modalBackgroundTransparent"
    TargetControlID="btnValidUntilRemark" PopupControlID="panValidUntilRemark" BehaviorID="panValidUntilRemarkHeading"
    PopupDragHandleControlID="" OkControlID="btnValidUntilRemarkClose" RepositionMode="None">
</cc2:ModalPopupExtender>
<%--<asp:Button ID="btnMpeValidUntilRemark" runat="server" Style="display: none" >
</asp:Button>--%>
<%-- End of Popup for Valid Until Remark --%>
