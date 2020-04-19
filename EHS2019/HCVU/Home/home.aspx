<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="home.aspx.vb" Inherits="HCVU.home" 
    title="<%$ Resources:Title, HomePage %>" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../JS/Common.js"></script>

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

		function populateNews()
		{
			cross_scroller=document.getElementById("iescroller");
			//actualheight=cross_scroller.offsetHeight;
			if (document.getElementById("<%=dlNewsMessage.ClientID%>") != null){
			    actualheight = document.getElementById("<%=dlNewsMessage.ClientID%>").offsetHeight + 16;
			    lefttime=setInterval("scrollscroller()",80);
			}			
		}

		function scrollscroller()
		{
		    if (cross_scroller != null){
		        if (parseInt(cross_scroller.style.top)>(actualheight*(-1)+8)){
				    cross_scroller.style.top=parseInt(cross_scroller.style.top)-copyspeed+"px";	
		        }	
			    else{
				    cross_scroller.style.top=parseInt(cross_scroller.style.height)+8+"px";
			    }
		    }			
		}
		
		</script>
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <table border="0">
        <tr>
              <td colspan="2" rowspan="1">
                <asp:Image ID="imgLoginInfoBanner" runat="server" ImageUrl="<%$ Resources:ImageUrl, LoginInfoBanner %>" AlternateText="<%$ Resources:AlternateText, LoginInfoBanner %>" /></td>
        </tr>
        <asp:Panel ID="pnlWithLastLogin" runat="server" Visible="true">
        <tr>
            <td style="width: 200px">
                <asp:Label ID="lblSuccessLoginText" runat="server" Text="<%$ Resources:Text, LastSuccessfulLogin %>"></asp:Label></td>
            <td style="width: 700px">
                <asp:Label ID="lblSuccessLogin" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblFailureLoginText" runat="server" Text="<%$ Resources:Text, LastFailureLogin %>"></asp:Label></td>
            <td>
                <asp:Label ID="lblFailureLogin" runat="server" CssClass="tableText"></asp:Label></td>
        </tr>
        <asp:Panel ID="pnlChangePasswordReminder" runat="server" Visible="false">
        <tr style="height: 25px">
            <td colspan="2" valign="middle"style="width: 900px; border: solid 1px red;" align="center">
                <span style="color: #003399;">
                <asp:Label ID="lblChangePasswordReminder" runat="server" Text="<%$ Resources:Text, ChgPwdReminder %>"></asp:Label>
                </span>
            </td>
        </tr>
        </asp:Panel>
        </asp:Panel>
    </table>
    <asp:Panel ID="pnlNewsMessage" runat="server">
    <hr style="width: 99%; color: #ff8080; border-top-style: none; border-right-style: none; border-left-style: none; height: 1px; " />
    <table>
        <tr>
            <td colspan="2" rowspan="1" valign="top">
                <asp:Image ID="imgNewsMessageBanner" runat="server" ImageUrl="<%$ Resources:ImageUrl, WhatsNewBanner %>" AlternateText="<%$ Resources:AlternateText, WhatsNewBanner %>" /></td>
        </tr>
        <tr>
            <td colspan="2" rowspan="1" valign="top">
                <div onmouseover="copyspeed=pausespeed" style="OVERFLOW: hidden; WIDTH: 650px; POSITION: relative; HEIGHT: 90px;"
	                onmouseout="copyspeed=scrollerspeed">
	                    <div id="iescroller" style="LEFT: 0px; WIDTH: 100%; POSITION: absolute; TOP: 0px; HEIGHT: 80px;"
		                    align="left">		                
		                    <asp:label id="lblNews" runat="server" CssClass="ScrollingText" Visible="false"></asp:label>
		                    <br />
		                    <asp:DataList ID="dlNewsMessage" runat="server" RepeatColumns="1" ShowFooter="False" ShowHeader="False" >
		                        <ItemTemplate>
		                            <table border="0" style="width: 100%">
		                                <tr>
		                                    <td style="width: 100px" valign="top">
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
    </table>
    </asp:Panel>
    <asp:Panel ID="pnlTaskList" runat="server">
        <hr style="width: 99%; color: #ff8080; border-top-style: none; border-right-style: none; border-left-style: none; height: 1px; " />
        <table>
            <tr>
                <td colspan="2" rowspan="1" valign="top">
                    <asp:Image ID="imgTaskListBanner" runat="server" ImageUrl="<%$ Resources:ImageUrl, TaskListBanner %>" AlternateText="<%$ Resources:AlternateText, TaskListBanner %>" /></td>
            </tr>
            <asp:PlaceHolder ID="plhTaskLisk" runat="server" EnableViewState="true" Visible="false"></asp:PlaceHolder>
        </table>
        <asp:DataList ID="dlTaskList" runat="server" RepeatColumns="1" ShowFooter="False" ShowHeader="False" >
                <ItemTemplate>
                    <asp:Table ID="tblTask" BorderWidth="0px" Width="630px" runat="server">
                        <asp:TableRow>
                            <asp:TableCell RowSpan="1" ColumnSpan="2" VerticalAlign="Top">
                                <div class = "headingText">
                                <asp:Label ID="lblTaskListTitle" runat="server" Text='<%# Bind("Title") %>'></asp:Label>
                                </div>
                            </asp:TableCell>
                        </asp:TableRow>
                        <asp:TableRow>
                            <asp:TableCell RowSpan="1" VerticalAlign="top">
                                <asp:Label ID="lblTaskList" runat="server" Text='<%# Bind("TaskDescription") %>'></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell HorizontalAlign="right" RowSpan="1" VerticalAlign="top">
                                <asp:ImageButton ID="ibtnTaskList" runat="server" ImageUrl="<%$ Resources:ImageUrl, GoBtn %>" AlternateText="<%$ Resources:AlternateText, GoBtn %>" CommandArgument='<%# Bind("URL") %>' CommandName='<%# Bind("TaskListID") %>'/>
                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>
                </ItemTemplate>
            </asp:DataList>
        <table>
            <tr>
                <td>
                    <asp:Label ID="lblNoTask" runat="server" Visible="false" Text="You have no outstanding task!"></asp:Label>
                </td>
            </tr>
        </table>
    </asp:Panel>

    <%--' CRE15-017 (Reminder to update conversion rate) [Start][Winnie]--%>
    <asp:Button ID="btnHiddenShowDialog" runat="server" Style="display: none" /><br />
    <cc1:ModalPopupExtender ID="ModalPopupExtenderNotice" runat="server" TargetControlID="btnHiddenShowDialog"
        PopupControlID="panNoticePopUp" BackgroundCssClass="modalBackgroundTransparent"
        DropShadow="False" RepositionMode="None" PopupDragHandleControlID="">
    </cc1:ModalPopupExtender>
    <br />
    <asp:Panel ID="panNoticePopUp" runat="server" Style="display:none; width:600px">
        <uc1:ucNoticePopUp ID="udcNoticePopUp" runat="server" ButtonMode="OK" NoticeMode="Attention" />
    </asp:Panel>
    <%--' CRE15-017 (Reminder to update conversion rate) [End][Winnie]--%>
</asp:Content>
