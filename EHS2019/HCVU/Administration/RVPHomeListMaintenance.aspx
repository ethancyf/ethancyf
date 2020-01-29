<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="RVPHomeListMaintenance.aspx.vb" Inherits="HCVU.RVPHomeListMaintenance" Title="<%$ Resources:Title, RVPHomeListMaintenance %>" %>

<%@ Register Src="~/ServiceProvider/spSummaryView.ascx" TagName="spSummaryView" TagPrefix="uc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc2" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc3" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, RVPHomeListBanner %>"
                            ImageUrl="<%$ Resources:ImageUrl, RVPHomeListBanner %>" /></td>
                </tr>
            </table>
            <cc1:InfoMessageBox ID="CompleteMsgBox" runat="server" Width="95%" />
            <cc1:MessageBox ID="msgBox" runat="server" Width="95%" />
            <asp:Panel ID="pnlEnquiry" runat="server">
                <asp:MultiView ID="MultiViewEnquiry" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewSearchCriteria" runat="server">
                        <table>
                            <tr>
                                <td style="width: 130px">
                                    <asp:Label ID="lblRCHCode" runat="server" Text="<%$ Resources:Text, RCHCode %>"></asp:Label></td>
                                <td style="width: 350px">
                                    <asp:TextBox ID="txtRCHCode" runat="server" MaxLength="6" onBlur="Upper(event,this)"></asp:TextBox>
                                    <asp:Image ID="imgRCHCodeAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                                <td style="width: 130px">
                                    <asp:Label ID="lblRCHName" runat="server" Text="<%$ Resources:Text, RCHName %>"></asp:Label>
                                    <asp:Label ID="lblRCHType" runat="server" Text="<%$ Resources:Text, RCHType %>" Visible="false"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtRCHName" runat="server" MaxLength="255" onBlur="Upper(event,this)" Width="300px"></asp:TextBox>
                                    <asp:DropDownList ID="ddlRCHType" runat="server" AppendDataBoundItems="True" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px">
                                    <asp:Label ID="lblRCHAddr" runat="server" Text="<%$ Resources:Text, RCHAddr %>"></asp:Label></td>
                                <td style="width: 350px">
                                    <asp:TextBox ID="txtRCHAddr" runat="server" MaxLength="255" onBlur="Upper(event,this)" Width="300px"></asp:TextBox></td>
                                <td style="width: 130px">
                                    <asp:Label ID="lblRCHStatus" runat="server" Text="<%$ Resources:Text, RCHStatus %>" Visible="false"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlRCHStatus" runat="server" AppendDataBoundItems="True" Width="155px" Visible="false">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 130px">
                                    </td>
                                <td style="width: 350px">
                                    
                                </td>
                            </tr>
                        </table>
                        <table style="width: 100%">
                            <tr>
                                <td align="center" style="padding-top: 10px">
                                    <asp:ImageButton ID="ibtnSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" OnClick="ibtnSearch_Click" />
                                    <asp:ImageButton ID="ibtnNew" runat="server" AlternateText="<%$ Resources:AlternateText, NewRCHRecordBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, NewRCHRecordBtn %>" OnClick="ibtnNew_Click" /></td>
                            </tr>
                        </table>
                        <cc2:FilteredTextBoxExtender ID="FilteredRCHCode" runat="server" TargetControlID="txtRCHCode"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers">
                        </cc2:FilteredTextBoxExtender>
                    </asp:View>
                    <asp:View ID="ViewSearchResult" runat="server">
                        <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                            Height="0px" Width="0px" OnClientClick="return false;" />
                        
                        <uc3:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="pnlSearchCriteriaReview" />
                        
                        <asp:Panel ID="pnlSearchCriteriaReview" runat="server">
                            <table>
                                <tr>
                                    <td valign="top" style="width: 130px">
                                        <asp:Label ID="lblResultRCHCodeText" runat="server" Text="<%$ Resources:Text, RCHCode %>"></asp:Label></td>
                                    <td valign="top" style="width: 350px">
                                        <asp:Label ID="lblResultRCHCode" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td valign="top" style="width: 130px">
                                         <asp:Label ID="lblResultRCHNameText" runat="server" Text="<%$ Resources:Text, RCHName %>"></asp:Label>
                                        <asp:Label ID="lblResultRCHTypeText" runat="server" Text="<%$ Resources:Text, RCHType %>" Visible="False"></asp:Label></td>
                                    <td valign="top" style="width: 350px">
                                        <asp:Label ID="lblResultRCHName" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:Label ID="lblResultRCHType" runat="server" CssClass="tableText" Visible="False"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        <asp:Label ID="lblResultRCHAddrText" runat="server" Text="<%$ Resources:Text, RCHAddr %>"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblResultRCHAddr" runat="server" CssClass="tableText"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblResultRCHStatusText" runat="server" Text="<%$ Resources:Text, RCHStatus %>" Visible="False"></asp:Label></td>
                                    <td valign="top">
                                        <asp:Label ID="lblResultRCHStatus" runat="server" CssClass="tableText" Visible="False"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        </td>
                                    <td valign="top">
                                        </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            AllowSorting="True" Width="1180">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="RCH_code" HeaderText="<%$ Resources:Text, RCHCode %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnRCHCode" runat="server" Text='<%# Eval("RCH_code") %>' CommandArgument='<%# Eval("RCH_code") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="70px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Type" HeaderText="<%$ Resources:Text, RCHType %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRCHType" runat="server" Text='<%# Eval("Type") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="150px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Homename_Eng" HeaderText="<%$ Resources:Text, RCHNameEng %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRCHNameEng" runat="server" Text='<%# Eval("Homename_Eng") %>'></asp:Label><br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="200px" />
                                </asp:TemplateField>
                                 <asp:TemplateField SortExpression="Homename_Chi" HeaderText="<%$ Resources:Text, RCHNameChi %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRCHNameChi" runat="server" Text='<%# Eval("Homename_Chi") %>' CssClass="textChi"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="170px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Address_Eng" HeaderText="<%$ Resources:Text, RCHAddrEng %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRCHAddrEng" runat="server" Text='<%# Eval("Address_Eng") %>'></asp:Label><br />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="300px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Address_Chi" HeaderText="<%$ Resources:Text, RCHAddrChi %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRCHAddrChi" runat="server" Text='<%# Eval("Address_Chi") %>' CssClass="textChi"></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="200px"/>
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Record_Status" HeaderText="<%$ Resources:Text, RCHStatus %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRCHStatus" runat="server" Text='<%# Eval("Record_Status") %>'></asp:Label>
                                     <%--   <asp:HiddenField ID="hfRTableLocation" runat="server" Value='<%# Eval("Table_Location") %>' />--%>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top"  Width="80px"/>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <table style="width: 100%">
                            <tr>
                                <td align="left">
                                    <asp:ImageButton ID="ibtnSearchResultBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSearchResultBack_Click" /></td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewDetails" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblDetailInfo" runat="server" Text="<%$ Resources:Text,RCHRecord %>"></asp:Label>
                        </div>
                        <table>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, RCHCode %>"></asp:Label></td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailRCHCode" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Label ID="Label2" runat="server" Text="<%$ Resources:Text, RCHType %>"></asp:Label></td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailRCHType" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:DropDownList ID="ddlDetailRCHType" runat="server" AppendDataBoundItems="True" Visible="False">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hfDetailRCHType" runat="server" />
                                    <asp:Image ID="imgRCHTypeAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 95px">
                                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Text, RCHName %>"></asp:Label>
                                </td>
                                <td style="width: 95px" valign="top">
                                    (<asp:Label ID="Label14" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailRCHNameEng" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:TextBox ID="txtDetailRCHNameEng" runat="server" MaxLength="255" onblur="Upper(event,this)"
                                        Visible="False" Width="750px"></asp:TextBox>
                                    <asp:Image ID="imgRCHNameEngAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 95px">
                                </td>
                                <td style="width: 95px" valign="top">
                                    (<asp:Label ID="Label15" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailRCHNameChi" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:TextBox ID="txtDetailRCHNameChi" runat="server" MaxLength="255" onblur="Upper(event,this)"
                                        Visible="False" Width="750px"></asp:TextBox>
                                    <asp:Image ID="imgRCHNameChiAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 95px">
                                    <asp:Label ID="Label4" runat="server" Text="<%$ Resources:Text, RCHAddr %>"></asp:Label></td>
                                <td style="width: 95px" valign="top">
                                    (<asp:Label ID="Label7" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailRCHAddrEng" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:TextBox ID="txtDetailRCHAddrEng" runat="server" MaxLength="1000" onblur="Upper(event,this)"
                                        Visible="False" Width="750px"></asp:TextBox>
                                    <asp:Image ID="imgRCHAddrEngAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 95px">
                                    
                                </td>
                                <td style="width: 95px" valign="top">
                                    (<asp:Label ID="Label17" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailRCHAddrChi" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:TextBox ID="txtDetailRCHAddrChi" runat="server" MaxLength="255" onblur="Upper(event,this)"
                                        Visible="False" Width="750px"></asp:TextBox>
                                    <asp:Image ID="imgRCHAddrChiAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Label ID="Label5" runat="server" Text="<%$ Resources:Text, RCHStatus %>"></asp:Label></td>
                                <td valign="top">
                                    <asp:Label ID="lblDetailRCHStatus" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:DropDownList ID="ddlDetailRCHStatus" runat="server" AppendDataBoundItems="True" Visible="False">
                                    </asp:DropDownList>
                                    <asp:HiddenField ID="hfDetailRCHStatus" runat="server" />
                                    <asp:Image ID="imgRCHStatusAlert" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                        ImageUrl="~/Images/others/icon_caution.gif" Style="position: absolute" Visible="false" />
                                </td>
                            </tr>
                        </table>
                        <br />
                        <table style="width: 100%">
                            <tr>
                                <td valign="top">
                                    <table style="width: 100%">
                                        <tr>
                                            <td style="width: 100px">
                                                <asp:ImageButton ID="ibtnBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnBack_Click" /></td>
                                            <td align="center">
                                                <asp:ImageButton ID="ibtnEdit" runat="server" AlternateText="<%$ Resources:AlternateText, EditBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, EditBtn %>" OnClick="ibtnEdit_Click" />
                                                <asp:ImageButton ID="ibtnSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnSave_Click" />
                                                <asp:ImageButton ID="ibtnCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnCancel_Click" />
                                                <asp:ImageButton ID="ibtnActive" runat="server" AlternateText="<%$ Resources:AlternateText, ActivateBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ActivateBtn %>" OnClick="ibtnActive_Click" />
                                                <asp:ImageButton ID="ibtnDeactive" runat="server" AlternateText="<%$ Resources:AlternateText, DeactivateBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, DeactivateBtn %>" OnClick="ibtnDeactive_Click" />
                                                <asp:ImageButton ID="ibtnRemove" runat="server" AlternateText="<%$ Resources:AlternateText, RemoveBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, RemoveBtn %>" OnClick="ibtnRemove_Click" /></td>
                                                <asp:ImageButton ID="ibtnRemoveDummy" runat="server" Visible=false/>
                                        </tr>
                                    </table>
                                    &nbsp;
                                    <asp:HiddenField ID="hfDetailTSMP" runat="server" />
                                    &nbsp; &nbsp;
                                </td>
                            </tr>
                        </table>
                        <cc2:FilteredTextBoxExtender ID="FilteredDetailRCHNameEng" runat="server" TargetControlID="txtDetailRCHNameEng"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                        </cc2:FilteredTextBoxExtender>
                        <cc2:FilteredTextBoxExtender ID="FilteredDetailRCHAddrEng" runat="server" TargetControlID="txtDetailRCHAddrEng"
                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                        </cc2:FilteredTextBoxExtender>

                    </asp:View>
                    <asp:View ID="ViewConfirm" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblConfirmInfo" runat="server" Text="<%$ Resources:Text,ConfirmDetail %>"></asp:Label>
                        </div>
                        <table>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Label ID="Label6" runat="server" Text="<%$ Resources:Text, RCHCode %>"></asp:Label></td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmRCHCode" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Label ID="Label8" runat="server" Text="<%$ Resources:Text, RCHType %>"></asp:Label></td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmRCHType" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:HiddenField ID="hfConfirmRCHType" runat="server" /></td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 95px">
                                    <asp:Label ID="Label10" runat="server" Text="<%$ Resources:Text, RCHName %>"></asp:Label></td>
                                <td style="width: 95px" valign="top">
                                    (<asp:Label ID="Label9" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmRCHNameEng" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 95px">
                                    &nbsp;</td>
                                <td style="width: 95px" valign="top">
                                    (<asp:Label ID="Label12" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmRCHNameChi" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 95px">
                                    <asp:Label ID="Label13" runat="server" Text="<%$ Resources:Text, RCHAddr %>"></asp:Label>
                                </td>
                                <td style="width: 95px" valign="top">
                                    (<asp:Label ID="Label11" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmRCHAddrEng" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 95px">
                                    &nbsp;</td>
                                <td style="width: 95px" valign="top">
                                    (<asp:Label ID="Label18" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmRCHAddrChi" runat="server" CssClass="tableText"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" valign="top">
                                    <asp:Label ID="Label16" runat="server" Text="<%$ Resources:Text, RCHStatus %>"></asp:Label></td>
                                <td valign="top">
                                    <asp:Label ID="lblConfirmRCHStatus" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:HiddenField ID="hfConfirmRCHStatus" runat="server" /></td>
                            </tr>
                        </table>
                        <br />
                        <table style="width: 100%">
                            <tr>
                                <td valign="top">
                                    <table style="width: 100%">
                                        <tr>
                                            <td align="center">
                                                <asp:ImageButton ID="ibtnConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnConfirm_Click" />
                                                <asp:ImageButton ID="ibtnConfirmBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnConfirmBack_Click" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewMsg" runat="server">
                            &nbsp;<asp:ImageButton ID="ibtnMsgBack" runat="server" AlternateText="<%$ Resources:AlternateText, ReturnBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, ReturnBtn %>" OnClick="ibtnMsgBack_Click" /></asp:View>
                    <asp:View ID="ViewError" runat="server">
                            &nbsp;<asp:ImageButton ID="ibtnErrorBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnErrorBack_Click" /></asp:View>
                </asp:MultiView>
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupConfirmCancel" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnCancel" PopupControlID="panPopupConfirmCancel" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
             <asp:Panel Style="display: none" ID="panPopupConfirmCancel" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirm" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, CancelAlert %>" />
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupConfirmActivate" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnActive" PopupControlID="panPopupConfirmActivate" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
             <asp:Panel Style="display: none" ID="panPopupConfirmActivate" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmActivate" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, ActivateAlert %>" />
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupConfirmDeactivate" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnDeactive" PopupControlID="panPopupConfirmDeactivate" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
             <asp:Panel Style="display: none" ID="panPopupConfirmDeactivate" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmDeactivate" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, DeactivateAlert %>" />
            </asp:Panel>
            <cc2:ModalPopupExtender ID="ModalPopupConfirmRemove" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="ibtnRemoveDummy" PopupControlID="panPopupConfirmRemove" PopupDragHandleControlID="" RepositionMode="None">
            </cc2:ModalPopupExtender>
             <asp:Panel Style="display: none" ID="panPopupConfirmRemove" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmRemove" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources:Text, RemoveAlert %>" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
    &nbsp;
</asp:Content>
