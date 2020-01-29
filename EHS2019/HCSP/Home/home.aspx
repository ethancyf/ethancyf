<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="home.aspx.vb" Inherits="HCSP.home" Title="<%$ Resources:Title, HomePage %>" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="780px" />
            <asp:HiddenField ID="hfCrossScrollerTop" runat="server" Value="0" />

            <script type="text/javascript">
		    //-------------------------------------------------------------
		    // Auto Scrolling Text
		    //-------------------------------------------------------------

		    // Scrollers speed here (larger is faster 1-10)
		    var scrollerspeed=1;
    		
		    var pauseit=1;
		    var copyspeed=scrollerspeed;
		    var iedom=document.all||document.getElementById;
		    var actualheight='';
		    var lefttime;
		    var cross_scroller, ns_scroller;
		    var pausespeed=(pauseit==0)? copyspeed: 0;
		    
		    var NewsTop = document.getElementById("<%=hfCrossScrollerTop.ClientID%>").value;
    		
    		
		    function populateNews()
		    {
			    cross_scroller=document.getElementById("iescroller");
			    //actualheight=cross_scroller.offsetHeight;
			    if ( document.getElementById("<%=dlNewsMessage.ClientID%>") != null){
			        actualheight = document.getElementById("<%=dlNewsMessage.ClientID%>").offsetHeight + 16;
			        lefttime=setInterval("scrollscroller()",80);
			    }			
		    }

		    function scrollscroller()
		    {
		        if (cross_scroller != null){
		            cross_scroller.style.top = document.getElementById("<%=hfCrossScrollerTop.ClientID%>").value;
    		        
		            if (parseInt(cross_scroller.style.top)>(actualheight*(-1)+8))
				        cross_scroller.style.top=parseInt(cross_scroller.style.top)-copyspeed+"px";		
			        else
				        cross_scroller.style.top=parseInt(cross_scroller.style.height)+8+"px";
    				    
				    document.getElementById("<%=hfCrossScrollerTop.ClientID%>").value = cross_scroller.style.top;
		        }		        
		    }
		    
		    Sys.WebForms.PageRequestManager.getInstance().add_pageLoaded(pageLoaded);
        
            function pageLoaded(sender, args) {
                //window.clearInterval(lefttime);
                if ( document.getElementById("iescroller") != null && document.getElementById("<%=hfCrossScrollerTop.ClientID%>") != null){
                    document.getElementById("iescroller").style.top = document.getElementById("<%=hfCrossScrollerTop.ClientID%>").value;
                }
            }
            
            </script>

            <table style="width: 100%">
                <tr>
                    <td>
                        <asp:Image ID="imgLoginInfo" runat="server" ImageUrl="<%$ Resources:ImageUrl, LoginInfoBanner %>"
                            AlternateText="<%$ Resources:AlternateText, LoginInfoBanner %>" /></td>
                </tr>
                <asp:Panel ID="pnlWithLastLogin" runat="server" Visible="true">
                    <tr>
                        <td align="left">
                            <table cellpadding="3" cellspacing="3" border="0" style="width: 700px">
                                <tr>
                                    <td align="left" style="width: 200px">
                                        <asp:Label ID="lblLoginSuccessText" runat="server" CssClass="tableTitle" Text="Last Successful Login"></asp:Label></td>
                                    <td align="left" style="width: 500px">
                                        <asp:Label ID="lblLoginSuccess" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td align="left">
                                        <asp:Label ID="lblLoginFailText" runat="server" CssClass="tableTitle" Text="Last Failure Login"></asp:Label></td>
                                    <td align="left">
                                        <asp:Label ID="lblLoginFail" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <asp:Panel ID="pnlChangePasswordReminder" runat="server" Visible="true">
                                    <tr style="height: 25px">
                                        <td colspan="2" valign="middle" style="border: solid 1px red;" align="left">
                                            <span style="color: #003399;">
                                                <asp:Label ID="lblChangePasswordReminder" runat="server" CssClass="" Style="font-size: 12pt"
                                                    Text="<%$ Resources:Text, ChgPwdReminder %>"></asp:Label>
                                            </span>
                                        </td>
                                    </tr>
                                </asp:Panel>
                            </table>
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="pnlFirstLogin" runat="server" Visible="false">
                    <tr>
                        <td style="height: 50px">
                            &nbsp;&nbsp;&nbsp;
                            <asp:Label ID="lblFirstLogin" runat="server" Text="Welcome to use HCSP!" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                </asp:Panel>
                <asp:Panel ID="pnlNewsMessage" runat="server">
                    <tr>
                        <td>
                            <hr />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Image ID="imgNewsMessage" runat="server" ImageUrl="<%$ Resources:ImageUrl, WhatsNewBanner %>"
                                AlternateText="<%$ Resources:AlternateText, WhatsNewBanner %>" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div onmouseover="copyspeed=pausespeed" style="overflow: hidden; width: 650px; position: relative;
                                height: 90px;" onmouseout="copyspeed=scrollerspeed">
                                <div id="iescroller" style="left: 0px; width: 100%; position: absolute; top: 0px;
                                    height: 80px;" align="left">
                                    <br />
                                    <asp:DataList ID="dlNewsMessage" runat="server" RepeatColumns="1" ShowFooter="False"
                                        ShowHeader="False">
                                        <ItemTemplate>
                                            <table border="0" style="width: 100%">
                                                <tr>
                                                    <td style="width: 120px" valign="top">
                                                        <asp:Label ID="lblCreateDate" runat="server" Text='<%# Bind("EffectiveDtm") %>'></asp:Label>
                                                    </td>
                                                    <td valign="top">
                                                        <asp:Label ID="lblDescription" runat="server" Text='<%# Bind("Description") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                            </table>
                                        </ItemTemplate>
                                    </asp:DataList>
                                </div>
                            </div>
                        </td>
                    </tr>
                </asp:Panel>
                <tr>
                    <td>
                        <hr />
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Image ID="imgTaskList" runat="server" ImageUrl="<%$ Resources:ImageUrl, TaskListBanner %>"
                            AlternateText="<%$ Resources:AlternateText, TaskListBanner %>" /></td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Panel ID="panTaskList" runat="server">
                            <asp:Panel ID="pnlTaskListDetail" runat="server">
                                <table cellpadding="5" cellspacing="5" style="width: 700px">
                                    <asp:Panel ID="pnl2ndLevelMsg" runat="server">
                                        <tr>
                                            <td align="left" colspan="2" class="tableHeadingAlert">
                                                <asp:Label ID="lbl2ndLevelMsgText" runat="server" Text="Voucher Recipient Validation 2nd Level Alert List"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" style="width: 370px">
                                                <asp:Label ID="lbl2ndLevelMsg" runat="server"></asp:Label>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:ImageButton ID="btnGoVR" runat="server" ImageUrl="<%$ Resources:ImageUrl, GoRedBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, GoRedBtn %>" OnClick="btnGoRectify_Click" />
                                            <td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" valign="top"/>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlUnreadMsg" runat="server">
                                        <tr>
                                            <td align="left" colspan="2" class="tableHeading">
                                                <asp:Label ID="lblUnreadMsgText" runat="server" Text="Outstanding Unread Email List"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" style="width: 370px">
                                                <asp:Label ID="lblUnreadMsg" runat="server"></asp:Label>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:ImageButton ID="btnGoInbox" runat="server" ImageUrl="<%$ Resources:ImageUrl, GoBtn %>"
                                                                AlternateText="<%$ Resources:AlternateText, GoBtn %>" OnClick="btnGoInbox_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" valign="top"/>
                                        </tr>
                                    </asp:Panel>
                                    <!-- CRE11-024-02 HCVS Pilot Extension Part 2 [Start] -->
                                    <asp:Panel ID="pnlIncompleteInfoClaims" runat="server">
                                        <tr>
                                            <td align="left" colspan="2" valign="top" class="tableHeading">
                                                <asp:Label ID="lblIncompleteInfoClaimstext" runat="server" Text="Claims With Incomplete Information List"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 370px" valign="top">
                                                <asp:Label ID="lblIncompleteInfoClaims" runat="server"></asp:Label>
                                            </td>
                                            <td align="left" valign="top">
                                                <cc2:CustomImageButton ID="btnGoIncompleteInfoClaims" runat="server" ImageUrl="<%$ Resources:ImageUrl, GoBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, GoBtn %>"  ShowRedirectImage="false"/>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" valign="top"/>
                                        </tr>
                                    </asp:Panel>
                                    <!-- CRE11-024-02 HCVS Pilot Extension Part 2 [End] -->
                                    <asp:Panel ID="pnlVoucherAccountRectification" runat="server">
                                        <tr>
                                            <td align="left" colspan="2" class="tableHeading">
                                                <asp:Label ID="lblVoucherAccountRectificationText" runat="server" Text="Outstanding Voucher Account Rectification List"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" style="width: 370px">
                                                <asp:Label ID="lblVoucherAccountRectification" runat="server"></asp:Label>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:ImageButton ID="btnGoRectify" runat="server" ImageUrl="<%$ Resources:ImageUrl, GoBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, GoBtn %>" OnClick="btnGoRectify_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2"/>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlClaimConfirmation" runat="server">
                                        <tr>
                                            <td align="left" colspan="2" class="tableHeading">
                                                <asp:Label ID="lblClaimConfirmationtext" runat="server" Text="Outstanding Claim Confirmation List"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td align="left" valign="top" style="width: 370px">
                                                <asp:Label ID="lblClaimConfirmation" runat="server"></asp:Label>
                                            </td>
                                            <td align="left" valign="top">
                                                <cc2:CustomImageButton ID="btnGoClaimVerify" runat="server" ImageUrl="<%$ Resources:ImageUrl, GoBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, GoBtn %>"  ShowRedirectImage="false"/></td>
                                        </tr>
                                        <tr>
                                            <td align="left" colspan="2" valign="top"/>
                                        </tr>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlAccountConfirmation" runat="server">
                                        <tr>
                                            <td align="left" colspan="2" valign="top" class="tableHeading">
                                                <asp:Label ID="lblAccountConfirmationtext" runat="server" Text="Outstanding Temporary Voucher Account Creation Confirmation List"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td align="left" style="width: 370px" valign="top">
                                                <asp:Label ID="lblAccountConfirmation" runat="server"></asp:Label>
                                            </td>
                                            <td align="left" valign="top">
                                                <asp:ImageButton ID="btnGoAccountVerify" runat="server" ImageUrl="<%$ Resources:ImageUrl, GoBtn %>"
                                                    AlternateText="<%$ Resources:AlternateText, GoBtn %>" />
                                            </td>
                                        </tr>
                                    </asp:Panel>
                                    
                                </table>
                            </asp:Panel>
                            <table cellpadding="5" cellspacing="5" style="width: 600px">
                                <tr>
                                    <td>
                                        <asp:Label ID="lblNoTask" runat="server" Visible="false" Text=""></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Button ID="btnHiddenShowDialog" runat="server" Style="display: none" /><br />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderConfirmAuthorize" runat="server" TargetControlID="btnHiddenShowDialog"
                PopupControlID="panConfirmMsg" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
            </cc1:ModalPopupExtender>
            <br />
            <asp:Panel ID="panConfirmMsg" runat="server" Style="display:none; width:600px">
                <uc1:ucNoticePopUp ID="udcNoticePopUp" runat="server" />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
