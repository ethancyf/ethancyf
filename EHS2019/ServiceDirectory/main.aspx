<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="main.aspx.vb" Inherits="ServiceDirectory.main" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc3" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title id="PageTitle" runat="server"></title>
    <script type="text/javascript" src="JS/Common.js"></script>
    <base id="basetag" runat="server" />
    <script type="text/javascript">

        function goNewWin(l) {

            var win;
            var tmp;
            var w = screen.availWidth || screen.width;
            var h = screen.availHeight || screen.height;

            w = 0;
            h = 0;

            var opts;

            opts = 'resizable=yes,status=yes,toolbar=no,location=no,scrollbars=yes,left=0,top=0,width=' + w + ',height=' + h;
            win = window.open(l, '_blank', opts);

            while (!win.open) { }

            window.self.opener = window.self;
            /*window.self.close();*/
        }


    </script>
    <link href="CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <link href="CSS/DialogStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .auto-style2 {
            height: auto;
            width: 190px;
        }

        .auto-style3 {
            width: 190px;
        }
    </style>
</head>
<body style="background-color: #F9F9F9;">
    <form id="form1" runat="server" autocomplete="off">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>

        <!---[CRE17-015] Disallow public using WinXP [2018-04-01] Start--->
        <asp:Button runat="server" ID="btnHiddenReminderWindowsVersion" Style="display: none" />
        <asp:Panel Style="display: none" ID="panReminderWindowsVersion" runat="server" Width="540px">
                <uc1:ucNoticePopUp ID="ucNoticePopUpReminderWindowsVersion" runat="server" NoticeMode="Custom" IconMode="Information" ButtonMode="OK" DialogImagePath="Images/dialog/"
                                 HeaderText="<%$ Resources:Text, ReminderTitle %>" MessageText="<%$ Resources:Text, ReminderWindowsVersion %>" />
        </asp:Panel>

        <cc1:ModalPopupExtender ID="ModalPopupExtenderReminderWindowsVersion" runat="server" TargetControlID="btnHiddenReminderWindowsVersion"
            PopupControlID="panReminderWindowsVersion" BehaviorID="mdlPopup3" BackgroundCssClass="modalBackgroundTransparent"
            DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panReminderWindowsVersionHeading">
        </cc1:ModalPopupExtender>
        <!---[CRE17-015] Disallow public using WinXP [2018-04-01] End--->

        <a id="top" name="top" />
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table id="tblBanner" style="background-image: url(<%$ Resources:imageUrl, IndexBanner %>); background-repeat: no-repeat; width: 985px; height: 100px" border="0" cellpadding="0" cellspacing="0" runat="server" width="100%">
                    <tr>
                        <td id="tdAppEnvironment" runat="server" style="vertical-align: top" class="AppEnvironment">
                            <asp:Label ID="lblAppEnvironment" runat="server" Text="[CodeBehind]"></asp:Label>
                        </td>
                        <td align="right" valign="top" style="padding-right: 10px; white-space: nowrap">
                            <table style="width: 70%; height: 100%;">
                                <tr>
                                    <td align="right" valign="top">
                                        <asp:LinkButton ID="lnkBtnContactUs" runat="server" Text="<%$ Resources:Text, ContactUs %>"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkBtnFAQs" runat="server" Text="<%$ Resources:Text, Faqs %>"></asp:LinkButton>
                                        <asp:LinkButton ID="lnkbtnTradChinese" runat="server" CssClass="languageText">繁體</asp:LinkButton>
                                        <asp:LinkButton ID="lnkbtnSimpChinese" runat="server" CssClass="languageText" Visible="false">簡体</asp:LinkButton>
                                        <asp:LinkButton ID="lnkbtnEnglish" runat="server" CssClass="languageText">English</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
                <table style="width: 100%;">
                    <tr>
                        <td id="Td1" style="background-color: #BAD876; width: 100%; height: 2px; background-repeat: no-repeat; padding-left: 5px; vertical-align: text-bottom;" runat="server"></td>
                    </tr>
                </table>
                <asp:Panel runat="server" ID="pnlSearch" DefaultButton="ibtnSearch">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 985px">
                        <tr align="center">
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 100%; padding-left: 2px;">
                                    <tr>
                                        <td>
                                            <cc3:MessageBox ID="udcMessageBox" runat="server" Width="90%" Visible="false"></cc3:MessageBox>
                                            <cc3:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="90%" Visible="false"></cc3:InfoMessageBox>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr id="filterCriteriaTitleBar" runat="server">
                            <td align="right" valign="top" style="height: 10px; padding-right: 5px;">
                                <asp:Label ID="lblShowHideSearchCriteria" runat="server" Text="" Style="font-family: Arial, HA_MingLiu; font-size: 12pt; font-weight: bold; color: Navy;"></asp:Label>
                                <asp:ImageButton ID="imgShowHideSearchCriteria" runat="server" Style="cursor: hand; display: none;"
                                    ImageUrl="~/Images/others/collapse.png" AlternateText="" OnClick="imgShowHideSearchCriteria_Click" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table id="TblFilterCriteria" runat="server" border="0" cellpadding="0" cellspacing="0" style="width: 880px; padding-left: 2px;">
                                    <tr>
                                        <td colspan="5" id="tdKeywordsSearchGuide">
                                            <asp:Label runat="server" ID="lblKeywordsSearchGuide" Text="<%$ Resources:Text, KeywordsSearchGuide %>" />
                                        </td>
                                    </tr>
                                    <tr id="trKeywordsSearch">
                                        <td colspan="5">
                                            <div id="divMagnifier">
                                                <asp:Image runat="server" ID="imgMagnifierIcon" ImageUrl="<%$ Resources:ImageUrl, MagnifierIcon %>" />
                                            </div>
                                            <div id="divKeywordsSearchTitle">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr valign="top">
                                                        <td id="tdKeywordsSearchTitle">
                                                            <asp:Label runat="server" ID="lblKeywords" Text="<%$ Resources:AlternateText, Keywords %>" ToolTip="<%$ Resources:AlternateText, Keywords %>" />
                                                        </td>
                                                        <td />
                                                        <td align="right">
                                                            <asp:LinkButton runat="server" Class="btnSearchClearClass" ID="lnkbtnKeywordsSearchClear" Text="<%$ Resources:AlternateText, ClearBtn %>"></asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td colspan="3">
                                                            <table id="tblKeywordsSearch" cellpadding="0" cellspacing="0" border="0">
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblServiceProvider" Text="<%$ Resources:Text, SDServiceProviderNameLabel %>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtServiceProvider" MaxLength="40" ></asp:TextBox>
                                                                        <asp:Label runat="server" ID="lblOptionalLabel1" CssClass="lblOptionalLabel"  Text="<%$ Resources:Text, OptionalLabel %>"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblPracticeName" Text="<%$ Resources:Text, SDPracticeNameLabel %>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtPracticeName" MaxLength="100" ></asp:TextBox>
                                                                        <asp:Label runat="server" ID="lblOptionalLabel2"  CssClass="lblOptionalLabel" Text="<%$ Resources:Text, OptionalLabel %>"></asp:Label>
                                                                    </td>
                                                                </tr>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label runat="server" ID="lblPracticeAddr" Text="<%$ Resources:Text, SDPracticeAddressLabel %>"></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:TextBox runat="server" ID="txtPracticeAddr" MaxLength="100" ></asp:TextBox>
                                                                        <asp:Label runat="server" ID="lblOptionalLabel3" CssClass="lblOptionalLabel" Text="<%$ Resources:Text, OptionalLabel %>"></asp:Label>
                                                                    </td>
                                                                    <td />
                                                                </tr>
                                                                <tr>
                                                                    <td></td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <table cellpadding="0" cellspacing="0" border="0" class="tblSearchCriteriaHeader">
                                                <tr>
                                                    <td class="tdSearchCriteriaHeaderIcon">
                                                        <asp:Image runat="server" ID="imgProfessionIcon" ImageUrl="<%$ Resources:ImageUrl, ProfessionIcon %>" />
                                                    </td>
                                                    <td class="tdSearchCriteriaHeaderText">
                                                        <asp:Label runat="server" ID="lblHealthCarePro" Text="<%$ Resources:AlternateText, HealthCareProHeader %>" ToolTip="<%$ Resources:AlternateText, HealthCareProHeader %>" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:LinkButton runat="server" Class="btnSearchClearClass" ID="lnkbtnHealthCareProClear" Text="<%$ Resources:AlternateText, ClearBtn %>"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr class="trSearchCriteriaHeaderBorder">
                                                    <td colspan="3" />
                                                </tr>
                                                <tr class="trSearchCriteriaHeaderBorder">
                                                    <td class="tdSearchCriteriaHeaderBorder" colspan="3" />
                                                </tr>
                                            </table>
                                        </td>
                                        <td colspan="2">
                                            <table cellpadding="0" cellspacing="0" border="0" class="tblSearchCriteriaHeader">
                                                <tr>
                                                    <td class="tdSearchCriteriaHeaderIcon">
                                                        <asp:Image runat="server" ID="imgMapmarkerIcon" ImageUrl="<%$ Resources:ImageUrl, MapmarkerIcon %>" />
                                                    </td>
                                                    <td class="tdSearchCriteriaHeaderText">
                                                        <asp:Label runat="server" ID="lblDistrict" Text="<%$ Resources:AlternateText, DistrictHeader %>" ToolTip="<%$ Resources:AlternateText, DistrictHeader %>" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:LinkButton runat="server" Class="btnSearchClearClass" ID="lnkbtnDistrictClear" Text="<%$ Resources:AlternateText, ClearBtn %>"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr class="trSearchCriteriaHeaderBorder">
                                                    <td colspan="3" />
                                                </tr>
                                                <tr class="trSearchCriteriaHeaderBorder">
                                                    <td class="tdSearchCriteriaHeaderBorder" colspan="3" />
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left" valign="top" colspan="3" style="padding-left: 2px;">
                                            <asp:RadioButtonList ID="rboProfessional" runat="server" AutoPostBack="True" RepeatColumns="2" Width="592px" Style="overflow: scroll;" CssClass="searchCriteriaText" CellPadding="2">
                                            </asp:RadioButtonList>
                                        </td>
                                        <td colspan="2" rowspan="4" style="vertical-align: top; padding: 1px; height: auto">
                                            <table id="tblDistrictList" runat="server" width="370px">
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:CheckBox ID="cb_area_1" runat="server" AutoPostBack="false" onClick="checkChild(this);" CssClass="cb_area_text" />
                                                    </td>
                                                    <td colspan="2" align="left" style="width: 45%;">
                                                        <asp:CheckBox ID="cb_area_3" runat="server" AutoPostBack="false" onClick="checkChild(this);" CssClass="cb_area_text" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 5px; height: auto;">&nbsp;</td>
                                                    <td valign="top" align="left" class="auto-style2">
                                                        <asp:CheckBoxList ID="cbl_area_1" runat="server" AutoPostBack="false" onClick="checkParent(this);" CssClass="searchCriteriaText" CellSpacing="2" CellPadding="2">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                    <td rowspan="4">&nbsp;</td>
                                                    <td rowspan="4" valign="top" align="left">
                                                        <asp:CheckBoxList ID="cbl_area_3" runat="server" AutoPostBack="false" onClick="checkParent(this);" CssClass="searchCriteriaText" CellSpacing="2" CellPadding="2">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" style="height: 8px;"></td>
                                                </tr>
                                                <tr>
                                                    <td colspan="2" align="left">
                                                        <asp:CheckBox ID="cb_area_2" runat="server" AutoPostBack="false" onClick="checkChild(this);" CssClass="cb_area_text" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 5px; height: auto;">&nbsp;</td>
                                                    <td valign="top" align="left" class="auto-style3">
                                                        <asp:CheckBoxList ID="cbl_area_2" runat="server" AutoPostBack="false" onClick="checkParent(this);" CssClass="searchCriteriaText" CellPadding="2" CellSpacing="2">
                                                        </asp:CheckBoxList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3"></td>
                                    </tr>
                                    <tr>
                                        <td colspan="3">
                                            <table cellpadding="0" cellspacing="0" border="0" class="tblSearchCriteriaHeader">
                                                <tr>
                                                    <td class="tdSearchCriteriaHeaderIcon">
                                                        <asp:Image runat="server" ID="imgServiceIcon" ImageUrl="<%$ Resources:ImageUrl, ServiceIcon %>" />
                                                    </td>
                                                    <td class="tdSearchCriteriaHeaderText">
                                                        <asp:Label runat="server" ID="Label1" Text="<%$ Resources:AlternateText, ServiceHeader %>" ToolTip="<%$ Resources:AlternateText, ServiceHeader %>" />
                                                    </td>
                                                    <td align="right">
                                                        <asp:LinkButton runat="server" Class="btnSearchClearClass" ID="lnkbtnServiceClear" Text="<%$ Resources:AlternateText, ClearBtn %>"></asp:LinkButton>
                                                    </td>
                                                </tr>
                                                <tr class="trSearchCriteriaHeaderBorder">
                                                    <td colspan="3" />
                                                </tr>
                                                <tr class="trSearchCriteriaHeaderBorder">
                                                    <td class="tdSearchCriteriaHeaderBorder" colspan="3" />
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="3" align="left" valign="top" style="padding-left: 2px;">
                                            <asp:TreeView ID="TreeViewService" runat="server" Width="578px" ShowCheckBoxes="Leaf" ShowExpandCollapse="False" ShowLines="false" NodeIndent="10" Style="padding: 4px 4px 4px 6px; overflow: auto; cursor: pointer; height: auto;">
                                                <NodeStyle CssClass="searchCriteriaText" NodeSpacing="1px" />
                                                <LeafNodeStyle CssClass="searchCriteriaText" HorizontalPadding="19px" />
                                            </asp:TreeView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="5" style="height: 33px;">
                                            <table id="tblSearchButton" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td width="40%" align="left">
                                                        <asp:ImageButton ID="iBtnReset" runat="server" ImageUrl="<%$ Resources:ImageUrl, ClearAllBtn %>" AlternateText="<%$ Resources:AlternateText, ClearAllBtn %>" />
                                                    </td>
                                                    <td width="auto" align="center">
                                                        <asp:ImageButton ID="iBtnSearch" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" AlternateText="<%$ Resources:AlternateText, SearchBtn %>" />
                                                    </td>
                                                    <td width="40%" align="left" />
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel runat="server" ID="pnlResult" DefaultButton="ibtnResult">
                    <asp:Button runat="server" ID="ibtnResult" Enabled="false"  />
                    <table id="tblResult" runat="server" width="1150px">
                        <tr align="left">
                            <td>
                                <asp:MultiView ID="mvResult" runat="server">
                                    <asp:View ID="vResultwithFee" runat="server">
                                        <table style="width: 970px;">
                                            <tr id="tblRowBlank_withFee" runat="server">
                                                <td colspan="2">
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="middle">
                                                    <asp:Image ID="imgMarker_withFee" runat="server" ImageUrl="~/Images/others/transparent.gif" /><br />
                                                    <asp:Image ID="imgCircleRed" runat="server" ImageAlign="AbsBottom" ImageUrl="~/Images/others/circle_red.png" />
                                                    <asp:Label ID="lblSearchResult" runat="server" Font-Size="14pt"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblResultsPerPage" runat="server" Text="<%$ Resources:Text, ResultsPerPage %>"></asp:Label>&nbsp;
                                                <asp:DropDownList ID="ddlResultPerPage" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>&nbsp;
                                                <asp:LinkButton ID="lnkBtnTop1" runat="server" Text="<%$ Resources:Text, GoToTop %>" OnClientClick="javascript:window.scrollTo(0,0);return false;"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table style="width: 970px;">
                                                        <tr>
                                                            <td id="tdRemarks" runat="server" align="left" valign="top" style="padding-left: 5px;">
                                                                <asp:GridView ID="gvRemarks" runat="server" AllowPaging="false" AllowSorting="false" AutoGenerateColumns="false" BorderWidth="0" BorderColor="transparent" BorderStyle="none" SkinID="GridViewSkin_Remarks">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderStyle-BackColor="transparent" HeaderImageUrl="Images/others/note.png" HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>                                         
                                                                            </ItemTemplate>
                                                                            <ItemStyle VerticalAlign="Top" Width="5px" HorizontalAlign="right" BackColor="transparent" Font-Size="10pt" />
                                                                        </asp:TemplateField>
                                                                         <asp:TemplateField HeaderStyle-ForeColor="black" HeaderStyle-BackColor="transparent">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblRemarks_PointsToNote" runat="server" Text="<%$ Resources: Text, PointsToNote %>"></asp:Label>
                                                                                <div style="padding-top: 10px;text-align: justify;">
                                                                                <asp:Label ID="lblRemarks_Declaration" runat="server" Text="<%$ Resources: Text, PointsToNote_Declaration %>"></asp:Label>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div style="max-height:999px;">	
																					<asp:Label ID="lblRemarksIndex" runat="server" Text='<%# Bind("Num")%>'></asp:Label>
                                                                                </div>																				
                                                                            </ItemTemplate>
                                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" BackColor="transparent" Font-Size="10pt" Width="10px"/>
                                                                            <HeaderStyle HorizontalAlign="Left"/>
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
																				<div style="max-height:999px;">
																					<asp:Label ID="lblRemarks" runat="server" Text='<%# Bind("Description")%>'></asp:Label>
																				</div>
                                                                            </ItemTemplate>
                                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" BackColor="transparent" Font-Size="10pt" />
                                                                        </asp:TemplateField>  
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <div style="max-height:999px;">
                                                                                    <asp:Label ID="lblRemarks_Chi" runat="server" Text='<%# Bind("Description_Chi")%>'></asp:Label>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" BackColor="transparent" Font-Size="10pt" />
                                                                        </asp:TemplateField>                            
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table >
                                            <tr style="padding-left: 5px;">
                                                <td align="left">
                                                    <asp:GridView ID="gvResult" runat="server" CaptionAlign="Top" Font-Names="arial, HA_MingLiu"
                                                        Font-Size="10pt" BorderStyle="None" Width="100%" AllowSorting="true" AllowPaging="true"
                                                        AutoGenerateColumns="false" CellPadding="5" HorizontalAlign="Center" SkinID="GridViewSkin_withFee">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblResultListIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" Width="10px" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, ServiceProvider %>" SortExpression="<%$ Resources: Text, headerSortSPName %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSPname" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, HealthCarePro %>" SortExpression="<%$ Resources: Text, headerSortHealthCarePro %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblHealthCarePro" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="60px" VerticalAlign="Top" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, headerPracticeInfo %>" SortExpression="<%$ Resources: Text, headerSortPracticeInfo %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPracticeInfo" runat="server"></asp:Label>
                                                                    <asp:Image ID="imgJoinedScheme01" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme02" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme03" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme04" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme05" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme06" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme07" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme08" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme09" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme10" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="230px" VerticalAlign="Top" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, District %>" SortExpression="<%$ Resources: Text, headerSortDistrict %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDistrictBoard" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="60px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="[CodeBehind]" SortExpression="subsidize_fee_01_sort_type, subsidize_fee_01_sort">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceFee01" runat="server" Text='<%# Eval("subsidize_fee_01") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="65px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="[CodeBehind]" SortExpression="subsidize_fee_02_sort_type, subsidize_fee_02_sort">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceFee02" runat="server" Text='<%# Eval("subsidize_fee_02") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="65px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="[CodeBehind]" SortExpression="subsidize_fee_03_sort_type, subsidize_fee_03_sort">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceFee03" runat="server" Text='<%# Eval("subsidize_fee_03") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="65px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="[CodeBehind]" SortExpression="subsidize_fee_04_sort_type, subsidize_fee_04_sort">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceFee04" runat="server" Text='<%# Eval("subsidize_fee_04") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="65px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="[CodeBehind]" SortExpression="subsidize_fee_05_sort_type, subsidize_fee_05_sort">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceFee05" runat="server" Text='<%# Eval("subsidize_fee_05") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="65px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="[CodeBehind]" SortExpression="subsidize_fee_06_sort_type, subsidize_fee_06_sort">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceFee06" runat="server" Text='<%# Eval("subsidize_fee_06") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="65px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="[CodeBehind]" SortExpression="subsidize_fee_07_sort_type, subsidize_fee_07_sort">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceFee07" runat="server" Text='<%# Eval("subsidize_fee_07") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="65px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="[CodeBehind]" SortExpression="subsidize_fee_08_sort_type, subsidize_fee_08_sort">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceFee08" runat="server" Text='<%# Eval("subsidize_fee_08") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="65px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="[CodeBehind]" SortExpression="subsidize_fee_09_sort_type, subsidize_fee_09_sort">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceFee09" runat="server" Text='<%# Eval("subsidize_fee_09") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="65px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="[CodeBehind]" SortExpression="subsidize_fee_10_sort_type, subsidize_fee_10_sort">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblServiceFee10" runat="server" Text='<%# Eval("subsidize_fee_10") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="65px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                        </table>
                                        <table style="width: 970px;">
                                            <tr>
                                                <td align="center">
                                                    <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblUpdateDate" runat="server" Text="<%$ Resources:Text, UpdateDate %>"></asp:Label></td>
                                                            <td align="right">
                                                                <asp:LinkButton ID="lnkBtnTop2" runat="server" Text="<%$ Resources:Text, GoToTop %>" OnClientClick="javascript:window.scrollTo(0,0);return false;"></asp:LinkButton>
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="tblRowBlank_withFee1" runat="server">
                                                <td></td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="vResultwithoutFee" runat="server">
                                        <table style="width: 985px">
                                            <tr id="tblRowBlank_noFee" runat="server">
                                                <td colspan="2">
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="left" valign="middle">
                                                    <asp:Image ID="imgMarker_noFee" runat="server" ImageUrl="~/Images/others/transparent.gif" /><br />
                                                    <asp:Image ID="imgCircleRed_noFee" runat="server" ImageAlign="AbsBottom" ImageUrl="~/Images/others/circle_red.png" />
                                                    <asp:Label ID="lblSearchResult_noFee" runat="server" Font-Size="14pt"></asp:Label>
                                                </td>
                                                <td align="right">
                                                    <asp:Label ID="lblResultsPerPage_noFee" runat="server" Text="<%$ Resources:Text, ResultsPerPage %>"></asp:Label>&nbsp;
                                                <asp:DropDownList ID="ddlResultPerPage_noFee" runat="server" AutoPostBack="true">
                                                </asp:DropDownList>&nbsp;
                                                <asp:LinkButton ID="lnkBtnTop1_noFee" runat="server" Text="<%$ Resources:Text, GoToTop %>" OnClientClick="javascript:window.scrollTo(0,0);return false;"></asp:LinkButton>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <table style="width: 100%">
                                                        <tr>
                                                            <td id="tdRemarks_v02" runat="server" align="left" valign="top" style="padding-left: 5px;">
                                                                <asp:GridView ID="gvRemarks_noFee" runat="server" AllowPaging="false" AllowSorting="false" AutoGenerateColumns="false" BorderWidth="0" BorderColor="transparent" BorderStyle="none" SkinID="GridViewSkin_Remarks">
                                                                    <Columns>
                                                                        <asp:TemplateField HeaderStyle-BackColor="transparent" HeaderImageUrl="Images/others/note.png" HeaderStyle-HorizontalAlign="Left" HeaderStyle-VerticalAlign="Top">
                                                                            <ItemTemplate>
                                                                            </ItemTemplate>
                                                                            <HeaderStyle BackColor="Transparent" HorizontalAlign="Left" VerticalAlign="Top" />
                                                                            <ItemStyle VerticalAlign="Top" Width="5px" HorizontalAlign="right" BackColor="transparent" Font-Size="10pt" />
                                                                        </asp:TemplateField> 
                                                                        <asp:TemplateField  HeaderStyle-ForeColor="black" HeaderStyle-BackColor="transparent">
                                                                            <HeaderTemplate>
                                                                                <asp:Label ID="lblRemarks_noFee_PointsToNote" runat="server" Text="<%$ Resources: Text, PointsToNote %>"></asp:Label>
                                                                                <div style="padding-top: 10px;text-align: justify;">
                                                                                <asp:Label ID="lblRemarks_noFee_Declaration" runat="server" Text="<%$ Resources: Text, PointsToNote_Declaration %>"></asp:Label>
                                                                                </div>
                                                                            </HeaderTemplate>
                                                                            <ItemTemplate>
                                                                                <div style="max-height:999px;">
                                                                                    <asp:Label ID="lblRemarksIndex_noFee" runat="server" Text='<%# Bind("Num")%>'></asp:Label>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" BackColor="transparent" Font-Size="10pt"  Width="10px"/>
                                                                            <HeaderStyle HorizontalAlign="Left"/>
                                                                        </asp:TemplateField>                                         
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <div style="max-height:999px;">
                                                                                    <asp:Label ID="lblRemarks_noFee_desc" runat="server" Text='<%# Bind("Description")%>'></asp:Label>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" BackColor="transparent" Font-Size="10pt" />
                                                                        </asp:TemplateField>
                                                                        <asp:TemplateField>
                                                                            <ItemTemplate>
                                                                                <div style="max-height:999px;">
                                                                                    <asp:Label ID="lblRemarks_noFee_desc_chi" runat="server" Text='<%# Bind("Description_Chi")%>'></asp:Label>
                                                                                </div>
                                                                            </ItemTemplate>
                                                                            <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" BackColor="transparent" Font-Size="10pt" />
                                                                        </asp:TemplateField>
                                                                    </Columns>
                                                                </asp:GridView>
                                                            </td>                                                     
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <table width="985px">
                                            <tr style="padding-left: 5px;">
                                                <td align="center">
                                                    <asp:GridView ID="gvResultWithoutFee" runat="server" CaptionAlign="Top" Font-Names="arial, HA_MingLiu" Font-Size="10pt" BorderStyle="None" Width="100%" AllowSorting="true" AllowPaging="true" AutoGenerateColumns="false" CellPadding="2" HorizontalAlign="Center" SkinID="GridViewSkin_noFee">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblResultListIndexWithoutFee" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="10px" VerticalAlign="Top" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, ServiceProvider %>" SortExpression="<%$ Resources: Text, headerSortSPName_noFee %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSPname_noFee" runat="server"></asp:Label><br />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="100px" VerticalAlign="Top" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, HealthCarePro %>" SortExpression="<%$ Resources: Text, headerSortHealthCarePro %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblHealthCarePro_noFee" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="60px" VerticalAlign="Top" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, SDPracticeName %>" SortExpression="<%$ Resources: Text, headerSortPracticeName_noFee %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPracticeName_noFee" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, District %>" SortExpression="<%$ Resources: Text, headerSortDistrict %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDistrictBoard_noFee" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="60px" VerticalAlign="Top" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, Address %>" SortExpression="<%$ Resources:Text, headerSortAddress_noFee %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblAddress_noFee" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Top" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, TelephoneNumber %>" SortExpression="<%$ Resources:Text, headerSortPhone_noFee %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPhone_noFee" runat="server" Text='<%# Eval("phone_daytime") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="60px" VerticalAlign="Top" HorizontalAlign="Left" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources: Text, EnrolledScheme %>" SortExpression="Joined_Scheme_Order">
                                                                <ItemTemplate>
                                                                    <asp:Image ID="imgJoinedScheme01" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme02" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme03" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme04" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme05" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme06" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme07" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme08" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme09" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                    <asp:Image ID="imgJoinedScheme10" runat="server" ImageUrl="<%$ Resources: ImageUrl, Transparent %>" Visible="false" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="125px" VerticalAlign="Top" HorizontalAlign="Left" Font-Names="arial" Font-Size="10pt" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="center">
                                                    <table style="width: 100%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td align="left">
                                                                <asp:Label ID="lblUpdateDate_noFee" runat="server" Text="<%$ Resources:Text, UpdateDate %>"></asp:Label></td>
                                                            <td align="right">
                                                                <asp:LinkButton ID="lnkBtnTop2_noFee" runat="server" Text="<%$ Resources:Text, GoToTop %>" OnClientClick="javascript:window.scrollTo(0,0);return false;"></asp:LinkButton>
                                                            </td>

                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr id="tblRowBlank_noFee1" runat="server">
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                </asp:MultiView>
                            </td>
                        </tr>
                    </table>

                </asp:Panel>
                <table style="width: 100%;" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td colspan="2" style="height: 10px;">&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" style="height: 5px" align="center" valign="bottom">
                            <hr style="width: 98%; color: #d6d6d6; border-top-style: none; border-right-style: none; border-left-style: none; height: 2px; border-bottom-style: none;" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">&nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkBtnPrivacyPolicy" runat="server" CssClass="footerText" Text="<%$ Resources:Text, PrivacyPolicy %>"></asp:LinkButton><asp:Label ID="lblseparator1" runat="server" CssClass="footerText" Text=" | "></asp:Label><asp:LinkButton ID="lnkBtnDisclaimer" runat="server" CssClass="footerText" Text="<%$ Resources:Text, ImportantNotices %>"></asp:LinkButton>
                            <asp:Label ID="lblseparator2" runat="server" CssClass="footerText" Text=" | "></asp:Label><asp:LinkButton ID="lnkBtnSysMaint" runat="server" CssClass="footerText" Text="<%$ Resources:Text, SysMaint %>"></asp:LinkButton>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>

    <script type="text/javascript">

        function ResizeScreen() {
            var w = screen.availWidth || screen.width;
            var h = screen.availHeight || screen.height;

            window.moveTo(0, 0);
            window.resizeTo(w, h);
        }

        ResizeScreen();

    </script>
</body>
</html>
