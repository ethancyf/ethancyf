<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucClaimSearch.ascx.vb"
    Inherits="HCSP.ucClaimSearch" %>
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

<style>
.searchOption-container {
    table-layout: fixed;
    width: 970px;
    margin: auto;
    overflow: hidden;
    border-spacing: 20px 0;
    margin-left: -25px; /* remove outer border spacing */
    margin-right: -15px; /* remove outer border spacing */        
}

.searchOption{
    width:100%;
    height:310px;
    background-repeat:no-repeat;
}
</style>

<asp:Panel ID="panSearchEC" runat="server" Visible="False" DefaultButton="btnECSearch">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top" style="width: 176px">
                <asp:Label ID="lblECHKIDText" runat="server" CssClass="tableTitle" Width="175px"></asp:Label></td>
            <td valign="top" style="padding-bottom: 8px;">
                <asp:TextBox ID="txtECHKID" runat="server" AutoCompleteType="Disabled" MaxLength="11"
                    onChange="formatHKID(this);" Width="85px"></asp:TextBox><asp:Image ID="ErrECHKID"
                        runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                        Visible="False" ImageAlign="AbsMiddle" /></td>
            <td valign="top">
                <asp:Label ID="lblHKIDECHint" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td rowspan="2" valign="top" style="width: 176px">
                <asp:Label ID="lblECDOBText" runat="server" CssClass="tableTitle" Width="175px"></asp:Label></td>
            <td style="padding-bottom: 5px;" valign="top">
                <asp:RadioButton ID="rbECDOB" runat="server" AutoPostBack="True" Checked="true" CssClass="tableTitle"
                    GroupName="groupECDOA" Text="" />
                <asp:TextBox ID="txtECDOB" runat="server" AutoCompleteType="Disabled" Enabled="true"
                    MaxLength="10" Width="85px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                    onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                    onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                <asp:Label ID="lblECDOBOrText" runat="server" CssClass="tableTitle"> </asp:Label>
                <asp:Image ID="ErrECDOB" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" /></td>
            <td valign="top">
                <asp:Label ID="lblDOBECHint" runat="server" CssClass="tableTitle"></asp:Label>
                <asp:Label ID="lblDOBECHint2" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td style="padding-bottom: 8px;" valign="top">
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
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" />&nbsp;</td>
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
                <asp:Label ID="lblDOAECHint" runat="server" CssClass="tableTitle"></asp:Label></td>
        </tr>
        <tr>
            <td rowspan="1" valign="top" style="width: 176px">
            </td>
            <td valign="top">
                <asp:ImageButton ID="btnECCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                    AlternateText="<%$ Resources: AlternateText, CancelBtn %>" />
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
    <cc1:FilteredTextBoxExtender ID="filteredECDOAAge" runat="server" FilterType="Numbers"
        TargetControlID="txtECDOAAge">
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
</asp:Panel>
<asp:Panel ID="panSearchShortNo" runat="server" Visible="False" DefaultButton="btnShortIdentityNoSearch">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" style="padding-bottom: 10px; width: 175px">
                            <asp:Label ID="lblSearchShortIdentityNo" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RegistrationNo %>"
                                Width="175px"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 8px; width: 225px;">
                            <asp:TextBox ID="txtSearchShortIdentityNo" runat="server" AutoCompleteType="Cellular"
                                MaxLength="11" Width="85px"></asp:TextBox>
                            <asp:Image ID="ErrSearchShortIdentityNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" />
                        </td>
                        <td style="padding-bottom: 10px;" valign="top">
                            <asp:Label ID="lblSearchShortIdentityNoTips" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding-bottom: 10px; width: 175px">
                            <asp:Label ID="lblSearchShortDOB" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOBLong %>"
                                Width="175px"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 8px; width: 225px;">
                            <asp:TextBox ID="txtSearchShortDOB" runat="server" AutoCompleteType="Disabled" Enabled="true"
                                MaxLength="10" Width="85px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="ErrSearchShortDOB" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" /></td>
                        <td style="padding-bottom: 10px;" valign="top">
                            <asp:Label ID="lblSearchShortDOBTips" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td style="padding-bottom: 10px; width: 175px" valign="top">
                        </td>
                        <td style="padding-bottom: 8px; width: 225px" valign="top">
                            <asp:ImageButton ID="btnShortIdentityNoCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                AlternateText="<%$ Resources: AlternateText, CancelBtn %>" />
                            <asp:ImageButton ID="btnShortIdentityNoSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" TabIndex="3" /></td>
                        <td style="padding-bottom: 10px" valign="top">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filteredSearchShortIdentityNo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtSearchShortIdentityNo" ValidChars="()">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredSearchShortDOB" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtSearchShortDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
<asp:Panel ID="panSearchHKIC" runat="server" Visible="False" DefaultButton="ibtnSearchHKIC">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width:390px">
                            <table id="tblHKICSymbol" runat="server" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top" style="padding-bottom: 5px; width: 175px">
                                        <asp:Label ID="lblHKICSymbolText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, HKICSymbolLong %>"></asp:Label>
                                        <asp:ImageButton ID="ImgBtnHKICSymbolHelp" runat="server" EnableViewState="true" AlternateText="<%$ Resources:AlternateText, HelpBtn%>"
                                            ImageUrl="<%$ Resources:ImageUrl, HelpIconBtn %>" Visible="true" Style="top:2px"/>
                                    </td>
                                    <td valign="top" style="padding-bottom: 5px;">
                                        <asp:RadioButtonList ID="rblHKICSymbol" runat="server" AutoPostBack="True" Enabled="true" TabIndex="1"  RepeatDirection="Horizontal" Style="position:relative;left:-5px;top:-3px"/>
                                        <asp:Image ID="ErrHKICSymbol" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td>
                            <table id="tblDownloadComboClient" runat="server" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td style="vertical-align:top;padding-bottom: 1px;">
                                        <div style="position:relative;left:0px;top:-4px;background-color:pink;border-color:red;border-style:solid;border-width:1px;width:554px;height:30px;vertical-align:middle;text-align:center">
                                            <asp:Label ID="lblSmartIDSoftwareAvailableDownload" runat="server" CssClass="tableTitle" 
                                                Text="<%$ Resources:Text, SmartIDSoftwareAvailableDownload %>" Style="position:relative;top:4px;color:#4d4d4d !important" />
                                            <div id="divUpdateNow" runat="server" style="position:relative;top:4px;border-style:solid;border-width:1px;padding:1px 1px 1px 1px;width:100px;display:inline-block;background-color:rgba(255,255,153,1);text-align:center;cursor:pointer;"
                                                onclick="javascript:__doPostBack('ctl00$ContentPlaceHolder1$udcClaimSearch$lbtnUpdateNow',''); return false;">
                                                <asp:LinkButton ID ="lbtnUpdateNow" runat="server" Text ="<%$ Resources:Text, UpdateNow %>" style="font-size:16px;text-decoration:none;color:rgba(0, 102, 204, 1)" />
                                            </div>
                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table class="searchOption-container" cellpadding="0" cellspacing="0" runat="server">
                    <tr>
                        <td style="vertical-align:top;width:305px">                        
                            <table id="tblManual" runat="server" cellpadding="0" cellspacing="0" class="searchOption">
                                <tr style="height:25px">
                                    <td align="center" valign="middle">
                                        <asp:Label ID="lblManualInput" runat="server" Font-Size="16px" style="color:white;"></asp:Label>
                                    </td>
                                </tr>
                                <tr style="height:50px">
                                </tr>
                                <tr style="height:180px">
                                    <td valign="top">
                                        <table style="width: 100%;padding-left:30px; padding-top: 5px;" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td valign="top" style="padding-bottom: 10px;">
                                                    <asp:Label ID="lblSearchHKICNoText" runat="server" CssClass="tableTitle" Width="120px"></asp:Label>
                                                </td>
                                                <td valign="top" style="padding-bottom: 8px; white-space:nowrap">
                                                    <asp:TextBox ID="txtSearchHKICNo" runat="server" AutoCompleteType="Cellular" MaxLength="11"
                                                        Width="85px" TabIndex="2" />
                                                    <asp:Image ID="ErrSearchHKICNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" style="padding-bottom: 20px;">
                                                    <asp:Label ID="lblSearchHKICDOBText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOBLong %>"></asp:Label>
                                                </td>
                                                <td valign="top" style="padding-bottom: 18px;white-space:nowrap">
                                                    <asp:TextBox ID="txtSearchHKICDOB" runat="server" AutoCompleteType="Disabled" Enabled="true"
                                                        MaxLength="10" Width="85px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                                        onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                                        onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"
                                                        TabIndex="3" />
                                                    <asp:Image ID="ErrSearchHKICDOB" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>                                  
                                </tr>
                                <tr style="vertical-align:central;text-align:center; height:40px;">
                                    <td ID="tdSearchButtonGroup" runat="server">
                                        <asp:ImageButton ID="ibtnSearchHKICCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>" 
                                            AlternateText="<%$ Resources: AlternateText, CancelBtn %>" />
                                        <asp:ImageButton ID="ibtnSearchHKIC" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" TabIndex="4" />
                                    </td>
                                </tr>
                                <tr></tr>
                            </table>
                        </td>
                        <td style="vertical-align:top;width:710px">
                            <asp:MultiView ID="mvIDEASCombo" runat="server" ActiveViewIndex="0"> 
                                <asp:View ID="vOldIDEAS" runat="server">
                                    <table cellpadding="0" cellspacing="0" runat="server">
                                        <tr>
                                            <td style="vertical-align:top;width:305px">
                                                <asp:MultiView ID="mvOldHKIC" runat="server" ActiveViewIndex="0"> 
                                                    <asp:View ID="vOldHKICSample" runat="server">
                                                        <table runat="server" cellpadding="0" cellspacing="0" style="vertical-align:middle">
                                                            <tr>
                                                                <td style="padding: 50px 0px 0px 10px; width: 280px; text-align: center;">
                                                                    <asp:Label ID="lblOldHKICSample" runat="server" Text="<%$ Resources:Text, OldHKIC %>" CssClass="tableText"/>                 
                                                                    <asp:Image ID="imgOldHKICSample" runat="server" AlternateText="<%$ Resources:AlternateText, OldSmartICSampleImg %>"
                                                                        ImageUrl="<%$ Resources:ImageUrl, OldSmartICSampleImg %>" ImageAlign="AbsMiddle"/>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                    <asp:View ID="vOldHKICSearch" runat="server">
                                                        <table id="tblOldSmartIC" runat="server" cellpadding="0" cellspacing="0" class="searchOption">
                                                            <tr style="height:25px">
                                                                <td align="center" valign="middle">
                                                                    <asp:Label ID="lblReadOldSmartIC" runat="server" Text="<%$ Resources:Text, ReadOldHKIC %>" Font-Size="16px" style="color:white" />
                                                                </td>
                                                            </tr>
                                                            <tr style="height:50px">
                                                                <td valign="middle" style="padding-left:150px;">
                                                                    <asp:Label ID="lblOldSmartICChipFaceUp" runat="server" Font-Size="14px" style="color:#4d4d4d" Text="<%$ Resources:Text, SmartIDChipFaceUp %>"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="height:180px">
                                                                <td style="vertical-align:top;padding-bottom: 5px; text-align: center;">
                                                                </td>                                    
                                                            </tr>
                                                            <tr style="vertical-align:central;text-align:center; height:40px;">
                                                                <td id="tdSearchOldSmartICButton" runat="server">
                                                                    <asp:ImageButton ID="btnShortIdentityNoOldSmartID" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReadCardAndSearchBtn %>"
                                                                        AlternateText="<%$ Resources:AlternateText, ReadCardAndSearchBtn %>"/>
                                                                    <asp:Label ID="lblReadOldCardAndSearchNA" runat="server" Text="<%$ Resources:Text, ReadCardAndSearchNA %>"
                                                                        CssClass="tableText" Width="250px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr></tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                            <td style="vertical-align:top;width:305px;padding-left:20px">
                                                <asp:MultiView ID="mvNewHKIC" runat="server" ActiveViewIndex="0"> 
                                                    <asp:View ID="vNewHKICSample" runat="server">
                                                        <table runat="server" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="padding: 50px 0px 0px 10px; width: 280px; text-align: center;">
                                                                    <asp:Label ID="lblNewHKICSample" runat="server" Text="<%$ Resources:Text, NewHKIC %>" CssClass="tableText" />
                                                                    <asp:Image ID="imgNewHKICSample" runat="server" AlternateText="<%$ Resources:AlternateText, NewSmartICSampleImg %>"
                                                                        ImageUrl="<%$ Resources:ImageUrl, NewSmartICSampleImg %>" ImageAlign="AbsMiddle" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                    <asp:View ID="vNewHKICSearch" runat="server">
                                                        <table runat="server" id="tblNewSmartIC" cellpadding="0" cellspacing="0" class="searchOption">
                                                            <tr style="height:25px">
                                                                <td style="text-align:center;vertical-align:middle"> 
                                                                    <asp:Label ID="lblReadNewSmartIC" runat="server" Text="<%$ Resources:Text, ReadNewHKIC %>" Font-Size="16px" style="color:white"/>
                                                                </td>
                                                            </tr>
                                                            <tr style="height:50px">
                                                                <td style="vertical-align:middle;padding-left:150px;">
                                                                    <asp:Label ID="lblNewSmartICChipFaceUp" runat="server" Font-Size="14px" style="color:#4d4d4d" Text="<%$ Resources:Text, SmartIDChipFaceUp %>"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="height:180px">
                                                                <td style="vertical-align:top;padding-bottom: 5px; text-align: center;">
                                                                </td>                                    
                                                            </tr>
                                                            <tr style="vertical-align:central;text-align:center; height:40px;">
                                                                <td id="tdSearchNewSmartICButton" runat="server">
                                                                    <asp:ImageButton ID="btnShortIdentityNoNewSmartID" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReadCardAndSearchBtn %>"
                                                                        AlternateText="<%$ Resources:AlternateText, ReadCardAndSearchBtn %>"/>
                                                                    <asp:Label ID="lblReadNewCardAndSearchNA" runat="server" Text="<%$ Resources:Text, ReadCardAndSearchNA %>"
                                                                        CssClass="tableText" Width="250px"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr></tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                                <asp:View ID="vNewIDEAS" runat="server">
                                    <table cellpadding="0" cellspacing="0" runat="server" style="vertical-align:middle">
                                        <tr>
                                            <td style="vertical-align:top;width:624px;">
                                                <table runat="server" id="tblNewSmartICCombo" cellpadding="0" cellspacing="0" class="searchOption">
                                                    <tr style="height:25px">
                                                        <td style="text-align:center;vertical-align:middle"> 
                                                            <asp:Label ID="lblReadNewSmartICCombo" runat="server" Text="<%$ Resources:Text, ReadSmartID %>" Font-Size="16px" style="color:white"/>
                                                        </td>
                                                    </tr>
                                                    <tr style="height:50px">
                                                        <td style="padding-left:350px;vertical-align:middle">
                                                            <asp:Label ID="lblNewSmartICComboChipFaceUp" runat="server" Font-Size="14px" style="color:#4d4d4d" Text="<%$ Resources:Text, SmartIDChipFaceUp %>"/>
                                                        </td>
                                                    </tr>
                                                    <tr style="height:180px">
                                                        <td style="vertical-align:top;padding-bottom: 5px; text-align: center; vertical-align:middle">
                                                            <div id="divSmartIDSoftwareNotInstalled" runat="server" style="padding-left:20px;padding-right:20px">
                                                                <asp:Label ID="lblSmartIDSoftwareNotInstalled" runat="server" Font-Size="16px" style="color:#4d4d4d" Text="<%$ Resources:Text, SmartIDSoftwareNotInstalled %>"/>
                                                            </div>
                                                        </td>                                    
                                                    </tr>
                                                    <tr style="vertical-align:central;text-align:center; height:52px;">
                                                        <td id="td1" runat="server">
                                                            <asp:ImageButton ID="btnShortIdentityNoNewSmartIDCombo" runat="server" ImageUrl="<%$ Resources:ImageUrl, ReadCardAndSearchBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, ReadCardAndSearchBtn %>" style="position:relative;top:-6px"/>
                                                            <asp:Label ID="lblReadNewCardAndSearchComboNA" runat="server" Text="<%$ Resources:Text, ReadCardAndSearchNA %>"
                                                                CssClass="tableText" Width="250px" Visible="false" />
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                    </table>
                                </asp:View>
                            </asp:MultiView>
                        </td>
                    </tr>
                </table>
            </td>    
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filteredSearchHKICNo" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtSearchHKICNo" ValidChars="()">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredSearcHKICDOB" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtSearchHKICDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
<asp:Panel ID="panSearchADOPC" runat="server" Visible="False" DefaultButton="btnADOPCSearch">
    <table cellpadding="0" cellspacing="0">
        <tr>
            <td valign="top">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" style="padding-bottom: 10px; width: 176px">
                            <asp:Label ID="lblADOPCIdentityNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, PassportNo %>"
                                Width="175px"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 8px; width: 225px;">
                            <asp:TextBox ID="txtADOPCIdentityNoPrefix" runat="server" AutoCompleteType="Cellular"
                                MaxLength="7" Width="65px"></asp:TextBox>
                            <asp:Label ID="lblADOPCSlashText" runat="server" CssClass="tableTitle">/</asp:Label>
                            <asp:TextBox ID="txtADOPCIdentityNo" runat="server" AutoCompleteType="Cellular" MaxLength="5"
                                Width="45px"></asp:TextBox>
                            <asp:Image ID="ErrADOPCIdentityNo" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" />
                        </td>
                        <td style="padding-bottom: 10px" valign="top">
                            <asp:Label ID="lblADOPCIdentityNoTips" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" style="padding-bottom: 10px; width: 176px">
                            <asp:Label ID="lblADOPCDOBText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DOBLong %>"
                                Width="175px"></asp:Label></td>
                        <td valign="top" style="padding-bottom: 8px; width: 225px;">
                            <asp:TextBox ID="txtADOPCDOB" runat="server" AutoCompleteType="Disabled" Enabled="true"
                                MaxLength="10" Width="85px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                                onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                                onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                            <asp:Image ID="ErrADOPCDOB" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" ImageAlign="AbsMiddle" /></td>
                        <td style="padding-bottom: 10px" valign="top">
                            <asp:Label ID="lblADOPCDOBTips" runat="server" CssClass="tableTitle"></asp:Label></td>
                    </tr>
                    <tr>
                        <td valign="top" style="width: 176px">
                        </td>
                        <td style="width: 225px" valign="top">
                            <asp:ImageButton ID="btnADOPCSearchCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                AlternateText="<%$ Resources: AlternateText, CancelBtn %>" />
                            <asp:ImageButton ID="btnADOPCSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" TabIndex="3" /></td>
                        <td valign="top">
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <cc1:FilteredTextBoxExtender ID="filteredADOPCIdentityNoPrefix" runat="server" FilterType="Custom, Numbers, UppercaseLetters, LowercaseLetters"
        TargetControlID="txtADOPCIdentityNoPrefix">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredADOPCIdentityNo" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtADOPCIdentityNo">
    </cc1:FilteredTextBoxExtender>
    <cc1:FilteredTextBoxExtender ID="filteredDILongDOB" runat="server" FilterType="Custom, Numbers"
        TargetControlID="txtADOPCDOB" ValidChars="-">
    </cc1:FilteredTextBoxExtender>
</asp:Panel>
