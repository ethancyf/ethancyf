<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="ConversionRateEnquiry.aspx.vb" Inherits="HCVU.ConversionRateEnquiry"
    Title="<%$ Resources: Title, ConversionRateEnquiry %>" EnableEventValidation="False" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <script type="text/javascript">
        var objCalendar;
        var objOriginalDate;
        var objSelectedDate;

        function pageLoad() {

            objCalendar = $find("calEffectiveDatePeriod");

            if (objCalendar != null) {
                ResetCalendarDelegate(objCalendar);
            }
        }

        function ResetCalendarDelegate(calendar) {
            //we need to reset the original delegate of the month cell. 
            calendar._cell$delegates = {
                mouseover: Function.createDelegate(calendar, calendar._cell_onmouseover),
                mouseout: Function.createDelegate(calendar, calendar._cell_onmouseout),

                click: Function.createDelegate(calendar, function (e) {

                    /// <summary>  
                    /// Handles the click event of a cell 
                    /// </summary> 
                    /// <param name="e" type="Sys.UI.DomEvent">The arguments for the event</param> 
                    e.stopPropagation();
                    e.preventDefault();

                    if (!calendar._enabled) return;

                    var target = e.target;
                    var visibleDate = calendar._getEffectiveVisibleDate();

                    Sys.UI.DomElement.removeCssClass(target.parentNode, "ajax__calendar_hover");

                    switch (target.mode) {
                        case "prev":

                        case "next":
                            calendar._visibleDate = target.date;
                            calendar.set_selectedDate(target.date);
                            break;
                        case "title":
                            switch (calendar._mode) {
                                case "days": calendar._switchMode("months"); break;
                                case "months": calendar._switchMode("years"); break;
                            }
                            break;
                        case "month":
                            //if the mode is month, then stop switching to day mode. 
                            if (target.month == visibleDate.getMonth()) {
                                //this._switchMode("days"); 
                            } else {
                                calendar._visibleDate = target.date;
                                //this._switchMode("days"); 
                            }
                            calendar.set_selectedDate(target.date);
                            calendar._blur.post(true);
                            calendar.raise_dateSelectionChanged();
                            break;
                        case "year":
                            if (target.date.getFullYear() == visibleDate.getFullYear()) {
                                calendar._switchMode("months");
                            } else {
                                calendar._visibleDate = target.date;
                                calendar._switchMode("months");
                            }
                            break;
                            //          case "day":                 
                            //            this.set_selectedDate(target.date);                            
                            //            this._blur.post(true);                 
                            //            this.raiseDateSelectionChanged();                 
                            //            break;                 

                        case "today":
                            calendar.set_selectedDate(target.date);
                            calendar._blur.post(true);
                            calendar.raise_dateSelectionChanged();
                            break;
                    }
                })
            }
        }

        function CalendarShown(sender, args) {
            //set the default mode to month 
            sender._switchMode("months", true);

            objOriginalDate = sender._getEffectiveVisibleDate();

            RedefineHandler(objCalendar);
        }

        function RedefineHandler(calendar) {
            if (calendar._monthsBody) {
                //remove the old handler of each month body. 
                for (var i = 0; i < calendar._monthsBody.rows.length; i++) {
                    var row = calendar._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $common.removeHandlers(row.cells[j].firstChild, calendar._cell$delegates);
                    }
                }

                //add the new handler of each month body. 

                for (var i = 0; i < calendar._monthsBody.rows.length; i++) {
                    var row = calendar._monthsBody.rows[i];
                    for (var j = 0; j < row.cells.length; j++) {
                        $addHandlers(row.cells[j].firstChild, calendar._cell$delegates);
                    }
                }
            }
        }



        function CalendarHidden(sender, args) {
            if (sender.get_selectedDate()) {

                objSelectedDate = sender.get_selectedDate();

                var originalDate = new Date(objOriginalDate);
                var selectedDate = new Date(objSelectedDate);

                var intMonth = selectedDate.getMonth();
                var intYear = selectedDate.getFullYear();
                var idxMonth = intMonth;
                var idxYear;
                
                sender.get_element().value = selectedDate.format(sender._format);

                if (originalDate.format(sender._format) != selectedDate.format(sender._format)) {
                    var objEffectiveDatePeriod = document.getElementById('<%= tblSearchHistory.FindControl("ibtnSearch").ClientID%>');
                    var objDropDownListMonth = document.getElementById('<%= tblSearchHistory.FindControl("ddlMonth").ClientID%>');
                    var objDropDownListYear = document.getElementById('<%= tblSearchHistory.FindControl("ddlYear").ClientID%>');

                    for (var i = 0; i < objDropDownListYear.options.length; i++) {
                        if (objDropDownListYear.options[i].text == intYear.toString()) {
                            idxYear=objDropDownListYear.options[i].index
                        }
                    }

                    objDropDownListMonth.selectedIndex = idxMonth;

                    objDropDownListYear.selectedIndex = idxYear;

                    objEffectiveDatePeriod.click();
                }
            }
        }
    </script> 

    <asp:ScriptManager ID="tsmConversionRateEnquiry" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConversionRateEnquiryBanner %>"
        AlternateText="<%$ Resources: AlternateText, ConversionRateEnquiryBanner %>">
    </asp:Image>
    <asp:UpdatePanel ID="upanConversionRateEnquiry" runat="server">
        <ContentTemplate>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="910px" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="910px" />

            <%-- Master View of Conversion Rate Enquiry --%>
            <asp:MultiView ID="mvConversionRateEnquiry" runat="server" ActiveViewIndex="-1">
                <asp:View ID="vNotice" runat="server">
                    <asp:Table ID="tblNotice" BorderWidth="0px" CellSpacing="0" runat="server">
                        <asp:TableRow Height="210px">
                            <asp:TableCell VerticalAlign="Top">
                                <div class = "headingText">
                                    <asp:Label ID="lblCurrentConversionRateInfo" runat="server" 
                                        Text="<%$ Resources: Text, CurrentConversionRateInfo %>"></asp:Label>
                                </div>
                                <asp:MultiView ID="mvCurrentConversionRateInfo" runat="server" ActiveViewIndex="-1">

                                    <%-- 0. No Conversion Rate Record --%>
                                    <asp:View ID="vNoCurrentConversionRate" runat="server">
                                        <asp:Table ID="tblNoCurrentConversionRate" BorderWidth="0px" Width="430px" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px">
                                                    &nbsp;
                                                </asp:TableCell>
                                                <asp:TableCell VerticalAlign="top" Width="430px" style="border-top-style:solid;border-top-width:1px">
                                                    <asp:Label ID="lblCurrentConversionRateRecord" runat="server" Text="<%$ Resources: Text, NoConversionRateRecord %>"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="middle" Width="100%" Height="4px" ColumnSpan="2"/>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View>

                                    <%-- 1. Effected Conversion Rate Record  --%>
                                    <asp:View ID="vCurrentConversionRate" runat="server">
                                        <asp:Table ID="tblCurrentConversionRate" BorderWidth="0px" CellPadding="0" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px" style="white-space:nowrap">
                                                    <span style="width:6px">&nbsp;</span>
                                                </asp:TableCell>
                                                <asp:TableCell VerticalAlign="top">
                                                    <asp:Table ID="tblCurrentConversionRateDetail" BorderWidth="1px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateIDText" runat="server" Text="<%$ Resources: Text, ConversionRateID %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateID" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentEffectiveDateText" runat="server" Text="<%$ Resources: Text, EffectiveDate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentEffectiveDate" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateText" runat="server" Text="<%$ Resources: Text, ConversionRate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRate" runat="server" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateCreateByText" runat="server" Text="<%$ Resources: Text, CreateBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateCreateBy" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateCreateDtm" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateApprovedByText" runat="server" Text="<%$ Resources: Text, ApprovedBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateApprovedBy" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblCurrentConversionRateApprovedDtm" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>

                                                    </asp:Table>
                                                </asp:TableCell>
                                            </asp:TableRow>

                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="middle" Width="100%" Height="8px" ColumnSpan="2"/>
                                            </asp:TableRow>

                                        </asp:Table>
                                    </asp:View> 

                                </asp:MultiView>
                                <asp:Table ID="tblConversionRateHistoryButton" BorderWidth="0px" CellPadding="0" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell VerticalAlign="top" Width="6px" />
                                        <asp:TableCell VerticalAlign="top">
                                            <asp:ImageButton ID="ibtnConversionRateHistory" runat="server"
                                                ImageUrl="<%$ Resources: ImageUrl, ConversionRateHistoryBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ConversionRateHistoryBtn %>"/>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>
                            </asp:TableCell>

                            <asp:TableCell VerticalAlign="top">
                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                            </asp:TableCell>

                            <asp:TableCell VerticalAlign="Top">
                                <div class = "headingText">
                                    <asp:Label ID="lblNextConversionRateInfo" runat="server" 
                                        Text="<%$ Resources: Text, NextConversionRateInfo %>"></asp:Label>
                                </div>
                                <asp:MultiView ID="mvNextConversionRateInfo" runat="server" ActiveViewIndex="-1">
                                    <%-- 0. No Next Conversion Rate Record --%>
                                    <asp:View ID="vNoNextConversionRate" runat="server">
                                        <asp:Table ID="tblNoNextConversionRate" BorderWidth="0px" Width="430px" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px">
                                                    &nbsp;
                                                </asp:TableCell>
                                                <asp:TableCell VerticalAlign="top" Width="412px" style="border-top-style:solid;border-top-width:1px">
                                                    <asp:Label ID="lblNextConversionRateRecord" runat="server" Text="<%$ Resources: Text, NoConversionRateRecord %>"></asp:Label>
                                                </asp:TableCell>
                                            </asp:TableRow>
                                        </asp:Table>
                                    </asp:View>

                                    <%-- 1. Approved Conversion Rate Record  --%>
                                    <asp:View ID="vNextConversionRate" runat="server">
                                        <asp:Table ID="tblNextConversionRate" BorderWidth="0px" CellPadding="0" runat="server">
                                            <asp:TableRow>
                                                <asp:TableCell VerticalAlign="top" Width="6px">
                                                    <span style="width:6px">&nbsp;</span>
                                                </asp:TableCell>
                                                <asp:TableCell VerticalAlign="top">
                                                    <asp:Table ID="tblNextConversionRateDetail" BorderWidth="1px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateIDText" runat="server" Text="<%$ Resources: Text, ConversionRateID %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateID" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextEffectiveDateText" runat="server" Text="<%$ Resources: Text, EffectiveDate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextEffectiveDate" runat="server"  
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateText" runat="server" Text="<%$ Resources: Text, ConversionRate %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRate" runat="server" 
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateCreateByText" runat="server" Text="<%$ Resources: Text, CreateBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateCreateBy" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateCreateDtm" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateApprovedByText" runat="server" Text="<%$ Resources: Text, ApprovedBy %>"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateApprovedBy" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                            </asp:TableCell>
                                                            <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                <span>&nbsp;&nbsp;&nbsp;</span>
                                                                <asp:Label ID="lblNextConversionRateApprovedDtm" runat="server"
                                                                    style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                            </asp:TableCell>
                                                        </asp:TableRow>
                                                        <asp:TableRow>
                                                            <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                        </asp:TableRow>
                                                    </asp:Table>
                                                </asp:TableCell>
                                            </asp:TableRow>

                                        </asp:Table>
                                    </asp:View> 

                                </asp:MultiView>
                            </asp:TableCell>
                        </asp:TableRow>

                    </asp:Table>
                </asp:View>

                <asp:View ID="vHistory" runat="server">
                    <asp:Table ID="tblHistory" BorderWidth="0px" CellSpacing="0" runat="server">
                        <asp:TableRow>
                            <asp:TableCell VerticalAlign="Top" RowSpan="2">
                                <div class = "headingText">
                                    <asp:Label ID="lblConversionRateHistory" runat="server" 
                                        Text="<%$ Resources: Text, ConversionRateHistory %>"></asp:Label>
                                </div>
                                <asp:Table ID="tblConversionRateHistory" BorderWidth="0px" CellPadding="0" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell VerticalAlign="top" Width="6px"/>
                                        <asp:TableCell VerticalAlign="top">
                                            <asp:Table ID="tblCalendar" BorderWidth="0px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                <asp:TableRow>
                                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="Right" Height="24px">
                                                        <asp:Table ID="tblCalendarHeader" BorderWidth="0px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                            <asp:TableRow BorderWidth="1px" style ="border-bottom:none">
                                                                <asp:TableCell style ="text-align:center;vertical-align:middle" Height="24px">
                                                                    <asp:Label ID="lblCalendarMonthYear" runat="server"></asp:Label>
                                                                    <span>
                                                                        <asp:Textbox ID="txtEffectiveDatePeriod" runat="server" Width="0" Height="0" MaxLength="10" style="display:inline;z-index:-1;position:relative;left:5px;top:2px;"></asp:Textbox>
                                                                        <asp:ImageButton ID="ibtnCalender" runat="server" ImageUrl="<%$ Resources: ImageUrl, CalenderBtn %>"
                                                                            AlternateText="<%$ Resources: AlternateText, CalenderBtn %>" style="vertical-align:top"/>
                                                                        <cc1:CalendarExtender ID="calEffectiveDatePeriod" BehaviorID="calEffectiveDatePeriod" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnCalender"
                                                                            TargetControlID="txtEffectiveDatePeriod" Format="MMMM, yyyy" TodaysDateFormat="d MMMM, yyyy" DefaultView="Months"
                                                                            OnClientShown ="CalendarShown" OnClientHidden="CalendarHidden">
                                                                        </cc1:CalendarExtender>
                                                                    </span>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                        </asp:Table>

                                                        <asp:Table ID="tblCalendarContent" BorderWidth="1px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                        </asp:Table>
                                                    </asp:TableCell>
                                                </asp:TableRow>

                                            </asp:Table>
                                        </asp:TableCell>
                                    </asp:TableRow>

                                </asp:Table>

                                <asp:Table ID="tblRemark" BorderWidth="0px" CellPadding="0" runat="server">
                                    <asp:TableRow Height="18px"/>
                                    <asp:TableRow Height="24px">
                                        <asp:TableCell VerticalAlign="top" Width="6px"/>
                                        <asp:TableCell VerticalAlign="top"><font color="red">*</font></asp:TableCell>
                                        <asp:TableCell VerticalAlign="top" Width="6px"/>
                                        <asp:TableCell VerticalAlign="top">
                                            <asp:Label ID="lblRemarkOne" runat="server" text="<%$ Resources: Text, ConversionRateEnquiryRemarkOne %>" />
                                        </asp:TableCell>
                                    </asp:TableRow>
                                    <asp:TableRow>
                                        <asp:TableCell VerticalAlign="top" Width="6px"/>
                                        <asp:TableCell VerticalAlign="top" Style="border:2px solid red;width:12px"/>
                                        <asp:TableCell VerticalAlign="top" Width="6px"/>
                                        <asp:TableCell VerticalAlign="top">
                                            <asp:Label ID="lblRemarkTwo" runat="server" text="<%$ Resources: Text, ConversionRateEnquiryRemarkTwo %>" />
                                        </asp:TableCell>                                 
                                    </asp:TableRow>
                                    <asp:TableRow Height="24px"/>
                                </asp:Table>
                            </asp:TableCell>

                            <asp:TableCell VerticalAlign="top" RowSpan="2">
                                <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                            </asp:TableCell>

                            <asp:TableCell VerticalAlign="Top" RowSpan="2">
                                <asp:Table ID="tblConversionRateHistoryRightPanel" BorderWidth="0px" CellPadding="0" runat="server">
                                    <asp:TableRow>
                                        <asp:TableCell VerticalAlign="top" >

                                            <%--<div class = "headingText" style="display:initial;">
                                                <asp:Label ID="lblSearchHistory" runat="server" 
                                                    Text="Search Conversion Rate"></asp:Label>
                                            </div>--%>

                                            <asp:Table ID="tblSearchHistory" BorderWidth="0px" CellPadding="0" runat="server">
                                                <asp:TableRow style="display:none;">
                                                    <asp:TableCell VerticalAlign="top" Width="5px"/>                                     
                                                    <asp:TableCell VerticalAlign="top" >
                                                        <asp:dropdownlist ID="ddlMonth" Width="100px" runat="server" style="vertical-align:middle"/>
                                                        <span>/</span>
                                                        <asp:dropdownlist ID="ddlYear" Width="100px" runat="server" style="vertical-align:middle" />
                                                    </asp:TableCell>
                                                    <asp:TableCell VerticalAlign="top" HorizontalAlign="Left">
                                                        <span>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                        <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="<%$ Resources: ImageUrl, SearchSBtn %>"
                                                            AlternateText="<%$ Resources: AlternateText, SearchSBtn %>" style="vertical-align:middle"/>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>

                                            <%--<br>--%>


                                            <div class = "headingText">
                                                <asp:Label ID="lblConversionRateRecord" runat="server" 
                                                    Text="<%$ Resources: Text, ConversionRateRecord %>"></asp:Label>
                                            </div>

                                            <asp:Table ID="tblConversionRateRecord" BorderWidth="0px" CellPadding="0" runat="server">
                                                <asp:TableRow>
                                                    <asp:TableCell VerticalAlign="top" Width="6px">
                                                        <span style="width:6px">&nbsp;</span>
                                                    </asp:TableCell>
                                                    <asp:TableCell VerticalAlign="top">
                                                        <asp:Table ID="tblConversionRateRecordDetail" BorderWidth="1px" Width="100%" style="border-color:#4D4D4D" runat="server">
                                                            <asp:TableRow>
                                                                <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                    <span>&nbsp;&nbsp;&nbsp;</span>
                                                                    <asp:Label ID="lblConversionRateRecordIDText" runat="server" Text="<%$ Resources: Text, ConversionRateID %>"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                    <span>&nbsp;&nbsp;&nbsp;</span>
                                                                    <asp:Label ID="lblConversionRateRecordID" runat="server"  
                                                                        style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                    <span>&nbsp;&nbsp;&nbsp;</span>
                                                                    <asp:Label ID="lblConversionRateRecordEffectiveDateText" runat="server" Text="<%$ Resources: Text, EffectiveDate %>"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                    <span>&nbsp;&nbsp;&nbsp;</span>
                                                                    <asp:Label ID="lblConversionRateRecordEffectiveDate" runat="server"  
                                                                        style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                    <span>&nbsp;&nbsp;&nbsp;</span>
                                                                    <asp:Label ID="lblConversionRateRecordRateText" runat="server" Text="<%$ Resources: Text, ConversionRate %>"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell VerticalAlign="middle" ColumnSpan="2" style="white-space:nowrap">
                                                                    <span>&nbsp;&nbsp;&nbsp;</span>
                                                                    <asp:Label ID="lblConversionRateRecordRate" runat="server" 
                                                                        style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                    <span>&nbsp;&nbsp;&nbsp;</span>
                                                                    <asp:Label ID="lblConversionRateRecordCreateByText" runat="server" Text="<%$ Resources: Text, CreateBy %>"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                    <span>&nbsp;&nbsp;&nbsp;</span>
                                                                    <asp:Label ID="lblConversionRateRecordCreateBy" runat="server"
                                                                        style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                    <span>&nbsp;&nbsp;&nbsp;</span>
                                                                    <asp:Label ID="lblConversionRateRecordCreateDtm" runat="server"
                                                                        style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                    <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell VerticalAlign="middle" Width="30%" Height="22px" style="white-space:nowrap">
                                                                    <span>&nbsp;&nbsp;&nbsp;</span>
                                                                    <asp:Label ID="lblConversionRateRecordApprovedByText" runat="server" Text="<%$ Resources: Text, ApprovedBy %>"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                    <span>&nbsp;&nbsp;&nbsp;</span>
                                                                    <asp:Label ID="lblConversionRateRecordApprovedBy" runat="server"
                                                                        style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                </asp:TableCell>
                                                                <asp:TableCell VerticalAlign="middle" style="white-space:nowrap">
                                                                    <span>&nbsp;&nbsp;&nbsp;</span>
                                                                    <asp:Label ID="lblConversionRateRecordApprovedDtm" runat="server"
                                                                        style="font-size: 16px;color: #4D4D4D;	font-family: Arial;	font-weight: bold;"></asp:Label>
                                                                    <span>&nbsp;&nbsp;&nbsp;&nbsp;</span>
                                                                </asp:TableCell>
                                                            </asp:TableRow>
                                                            <asp:TableRow>
                                                                <asp:TableCell VerticalAlign="middle" Width="100%" Height="10px" ColumnSpan="3"/>
                                                            </asp:TableRow>
                                                        </asp:Table>
                                                    </asp:TableCell>
                                                </asp:TableRow>
                                            </asp:Table>
                                        </asp:TableCell>
                                    </asp:TableRow>
                                </asp:Table>

                            </asp:TableCell>
                        </asp:TableRow>
                    </asp:Table>

                </asp:View>

            </asp:MultiView>

            <span>&nbsp;</span>
            <asp:ImageButton ID="ibtnBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                AlternateText="<%$ Resources: AlternateText, BackBtn %>" Enabled ="false" Visible ="false"/>

        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
