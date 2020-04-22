<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucClaimSearch.ascx.vb"
    Inherits="PrefillConsent.ucClaimSearch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<script type="text/javascript">

        function formatHKID(textbox) {
            this.UpperIndentityNo(textbox);
            txt = textbox.value;
            var res=""; 
            if ((txt.length == 8) || (txt.length ==9))
            {  if ((txt.indexOf("(")<0) && (txt.indexOf(")")<0))
                {	
                    res=txt.substring(0,txt.length-1) + "(" + txt.substring(txt.length-1, txt.length) + ")";
                }
               else
                {
                    res = txt;
                }
                textbox.value=res;
            }
            return false;
        }
        
        function UpperIndentityNo(textbox)
        {
            textbox.value=textbox.value.toUpperCase();
            return false;
        }
        
        function formatVISA(textbox)
        {
            this.UpperIndentityNo(textbox);
            txt = textbox.value;
            var res="";
            if (txt.length == 14) 
            {  
                if ((txt.indexOf("-")<0) && (txt.indexOf("(")<0) && (txt.indexOf(")")<0))
                {
                    res=txt.substring(0,4) + "-" + txt.substring(4, 11) + "-" + txt.substring(11, 13) + "(" + txt.substring(13, 14) + ")";
                }
            
                textbox.value=res;
            }
            return false;
        }
        
</script>

<asp:Panel ID="panSearchEC" runat="server" Visible="False">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" style="width: 176px">
                <asp:Label ID="lblECHKIDText" runat="server" CssClass="tableTitle"
                    Width="175px"></asp:Label></td>
            <td valign="top" style="padding-bottom: 10px; width: 1px;">
                <asp:TextBox ID="txtECHKID" runat="server" AutoCompleteType="Disabled" MaxLength="11"
                    onChange="formatHKID(this);" Width="85px"></asp:TextBox><asp:Image ID="ErrECHKID"
                        runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                        Visible="False" ImageAlign="AbsMiddle" /></td>
            <td style="width: 350px" valign="top">
                <asp:Label ID="lblHKIDECHint" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td rowspan="2" valign="top" style="width: 176px">
                <asp:Label ID="lblECDOBText" runat="server" CssClass="tableTitle" Width="175px"></asp:Label></td>
            <td style="width: 1px; padding-bottom: 5px;" valign="top">
                <asp:RadioButton ID="rbECDOB" runat="server" AutoPostBack="True" Checked="true" CssClass="tableTitle"
                    GroupName="groupECDOA" Text="" />
                <asp:TextBox ID="txtECDOB" runat="server" AutoCompleteType="Disabled" Enabled="true"
                    MaxLength="10" Width="85px"></asp:TextBox>
                <asp:Label ID="lblECDOBOrText" runat="server" CssClass="tableTitle"> </asp:Label>
                    <asp:Image ID="ErrECDOB" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" /></td>
            <td valign="top">
                <asp:Label ID="lblDOBECHint" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td style="width: 1px; padding-bottom: 10px;" valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td align="left">
                            <asp:RadioButton ID="rbECDOA" runat="server" AutoPostBack="True" CssClass="tableTitle"
                                GroupName="groupECDOA" />&nbsp;</td>
                        <td align="left">
                            <asp:TextBox ID="txtECDOAAge" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                MaxLength="3" Width="30px"></asp:TextBox>&nbsp;<asp:Image ID="ErrECDOAAge" runat="server"
                                    AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                    Visible="False" ImageAlign="AbsMiddle" />
                        </td>
                        <td align="center">
                            <asp:Label ID="lblECDOAOnText" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;</td>
                        <td align="center">
                            <asp:TextBox ID="txtECDOADayEn" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                MaxLength="2" Width="20px"></asp:TextBox>
                            <asp:TextBox ID="txtECDOAYearChi" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                MaxLength="4" Width="36px"></asp:TextBox>
                            <asp:Label ID="lblECDOAYearChiText" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td align="center">
                            &nbsp;<asp:DropDownList ID="ddlECDOAMonth" runat="server" Enabled="false">
                            </asp:DropDownList><asp:Label ID="lblECDOAMonthChiText" runat="server" CssClass="tableTitle"></asp:Label>&nbsp;</td>
                        <td align="center">
                            <asp:TextBox ID="txtECDOAYearEn" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                MaxLength="4" Width="36px"></asp:TextBox>
                            <asp:TextBox ID="txtECDOADayChi" runat="server" AutoCompleteType="Disabled" Enabled="false"
                                MaxLength="2" Width="20px"></asp:TextBox>
                            <asp:Label ID="lblECDOADayChiText" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td align="center">
                            <asp:Image ID="ErrECDOA" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" /></td>
                    </tr>
                    <tr>
                        <td align="center">
                        </td>
                        <td align="center">
                        </td>
                        <td align="center">
                        </td>
                        <td align="center">
                            <asp:Label ID="lblECDOADayEnText" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td align="center">
                            <asp:Label ID="lblECDOAMonthEnText" runat="server" CssClass="tableTitle"></asp:Label></td>
                        <td align="center">
                            <asp:Label ID="lblECDOAYearEnText" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <asp:Label ID="lblDOAECHint" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ECDOBHint2 %>"></asp:Label></td>
        </tr>
        <tr>
            <td rowspan="1" valign="top" style="width: 176px">
            </td>
            <td style="width: 1px" valign="top">
                <asp:ImageButton ID="btnECSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" TabIndex="3" /></td>
            <td valign="top">
            </td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filteredECHKID" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtECHKID" ValidChars="()">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredECDOB" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtECDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredECDOAAge" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtECDOAAge" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredECDOAYearEn" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtECDOAYearEn">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredECDOADayEn" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtECDOADayEn">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredECDOAYearChi" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtECDOAYearChi">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredECDOADayChi" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtECDOADayChi">
    </cc1:FilteredTextBoxExtender>
    <br />
</asp:Panel>
<asp:Panel ID="panSearchShortNo" runat="server" Visible="False">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" style="padding-bottom: 10px; width: 175px">
                            <asp:Label ID="lblSearchShortIdentityNo" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, RegistrationNo %>" Width="175px"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 10px; width: 225px;">
                            <asp:TextBox ID="txtSearchShortIdentityNo" runat="server" AutoCompleteType="Cellular"
                                MaxLength="11" Width="85px"></asp:TextBox>
                            <asp:Image ID="ErrSearchShortIdentityNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" />
                        </td>
                        <td style="padding-bottom: 10px" valign="top">
                            <asp:Label ID="lblSearchShortIdentityNoTips" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding-bottom: 10px; width: 175px">
                            <asp:Label ID="lblSearchShortDOB" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, DOBLong %>" Width="175px"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 10px; width: 225px;">
                            <asp:TextBox ID="txtSearchShortDOB" runat="server" AutoCompleteType="Disabled" Enabled="true"
                                MaxLength="10" Width="85px"></asp:TextBox>
                            <asp:Image ID="ErrSearchShortDOB" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" /></td>
                        <td style="padding-bottom: 10px" valign="top">
                            <asp:Label ID="lblSearchShortDOBTips" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 175px">
                        </td>
                        <td style="width: 225px" valign="top">
                            <asp:ImageButton ID="btnShortIdentityNoSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" TabIndex="3" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                </table>
            </td>
            <td valign="top">
                <asp:Image ID="imgSearchShortIdentityTips" runat="server" AlternateText="<%$ Resources:AlternateText, HKICSampleImg %>"
                    ImageUrl="<%$ Resources:ImageUrl, HKICSampleImg %>" /></td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filteredSearchShortIdentityNo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtSearchShortIdentityNo" ValidChars="()">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredSearchShortDOB" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtSearchShortDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
<asp:Panel ID="panSearchLongNo" runat="server" Visible="False">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" style="padding-bottom: 10px; width: 176px">
                            <asp:Label ID="lblSearchLongIdentityNo" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, PassportNo %>" Width="175px"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 10px; width: 225px;">
                            <asp:TextBox ID="txtSearchLongIdentityNo" runat="server" AutoCompleteType="Cellular"
                                MaxLength="20" Width="140px"></asp:TextBox>
                            <asp:Image ID="ErrSearchLongIdentityNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" />
                        </td>
                        <td style="padding-bottom: 10px" valign="top">
                            <asp:Label ID="lblSearchLongIdentityNoTips" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding-bottom: 10px; width: 176px">
                            <asp:Label ID="lblSearchLongDOB" runat="server" CssClass="tableTitle"
                                Text="<%$ Resources:Text, DOBLong %>" Width="175px"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 10px; width: 225px;">
                            <asp:TextBox ID="txtSearchLongDOB" runat="server" AutoCompleteType="Disabled" Enabled="true"
                                MaxLength="10" Width="85px"></asp:TextBox>
                            <asp:Image ID="ErrSearchLongDOB" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" /></td>
                        <td style="padding-bottom: 10px" valign="top">
                            <asp:Label ID="lblSearchLongDOBTips" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 176px">
                        </td>
                        <td style="width: 225px" valign="top">
                            <asp:ImageButton ID="btnLongIdentityNoSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" TabIndex="3" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filteredSearchLongIdentityNo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtSearchLongIdentityNo" ValidChars="()">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredSearchLongDOB" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtSearchLongDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
