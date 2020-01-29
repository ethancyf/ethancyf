<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="PostPaymentCheck.aspx.vb" Inherits="HCVU.PostPaymentCheck" Title="<%$ Resources:Title, PostPaymentCheck %>"
    EnableEventValidation="False" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="~/UIControl/Statistics/StatisticsCriteriaBase.ascx" TagName="ucStatisticsCriteriaBase"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<asp:Content ID="ContentHeader" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <style type="text/css">
        table.TableStyle1 td {
            vertical-align: top;
        }

        table.P2InputTableSP tr {
            height: 26px;
        }

        table.P2InputTableSP td {
            vertical-align: top;
        }

        table.P2SPListHeader td {
            vertical-align: middle;
            text-align: center;
            background-color: #A9A9A9;
            color: white;
            border-right: solid 1px #666666;
            border-top: solid 1px #666666;
        }

            table.P2SPListHeader td:first-child {
                border-left: solid 1px #666666;
            }

        table.P2TargetTransaction tr {
            height: 26px;
        }

        table.P2TargetTransaction td {
            vertical-align: top;
        }

        table.P3SelectionOfSP tr {
            height: 26px;
        }

        table.P3SelectionOfSP td {
            vertical-align: top;
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript">
        var objCalendarFrom;
        var objCalendarTo;
        var objSelectedDate;

        var lblFromDateDummy;
        var txtFromDate;
        var btnFromDateDummy;
        var btnFromDate;

        var lblToDateDummy;
        var txtToDate;
        var btnToDateDummy;
        var btnToDate;

        function pageLoad() {
            //find behaviour ID instead of client ID
            objCalendarFrom = $find("calExFromDate_MY");
            objCalendarTo = $find("calExToDate_MY");

            if (objCalendarFrom != null && objCalendarTo != null) {
                ResetCalendarDelegate(objCalendarFrom);
                ResetCalendarDelegate(objCalendarTo);
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
                            calendar.raiseDateSelectionChanged();
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
                            calendar.raiseDateSelectionChanged();
                            break;
                    }
                })
            }
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

        function CalendarShownFromDate(sender, args) {
            //set the default mode to month 
            sender._switchMode("months", true);

            //RedefineHandler(objCalendarFrom);
            //RedefineHandler(objCalendarTo);
        }

        function CalendarHiddenFromDate(sender, args) {

            if (sender.get_selectedDate()) {

                objSelectedDate = sender.get_selectedDate();

                var selectedDate = new Date(objSelectedDate);

                var intMonth = selectedDate.getMonth() + 1;
                var intYear = selectedDate.getFullYear();

                if (!isNaN(intMonth) && !isNaN(intYear)) {
                    lblFromDateDummy.value = null
                    //if (intMonth < 10) {
                    //    txtFromDateDummy.value = '0' + intMonth + '-' + intYear;
                    //}
                    //else {
                    //    txtFromDateDummy.value = intMonth + '-' + intYear;
                    //}
                    var txtMonth = null
                    switch (intMonth) {
                        case 1:
                            txtMonth = "January"; break;
                        case 2:
                            txtMonth = "February"; break;
                        case 3:
                            txtMonth = "March"; break;
                        case 4:
                            txtMonth = "April"; break;
                        case 5:
                            txtMonth = "May"; break;
                        case 6:
                            txtMonth = "June"; break;
                        case 7:
                            txtMonth = "July"; break;
                        case 8:
                            txtMonth = "August"; break;
                        case 9:
                            txtMonth = "September"; break;
                        case 10:
                            txtMonth = "October"; break;
                        case 11:
                            txtMonth = "November"; break;
                        case 12:
                            txtMonth = "December"; break;
                    }

                    if (txtMonth != null) {
                        lblFromDateDummy.innerText = txtMonth + ", " + intYear;
                    }
                    else {
                        lblFromDateDummy.innerText = "&nbsp;";
                    }
                }
            }
        }

        function ResetCalendarFromDate(objFromDateDummyID) {
            var objFromDateDummy = document.getElementById(objFromDateDummyID);

            if (objCalendarFrom != null && objFromDateDummy != null) {
                //alert(objCalendarFrom.get_selectedDate());
                var selectedDate = new Date(objCalendarFrom.get_selectedDate());

                var intMonth = selectedDate.getMonth() + 1;
                var intYear = selectedDate.getFullYear();

                if (!isNaN(intMonth) && !isNaN(intYear) && intYear != "1970") {
                    objFromDateDummy.innerText = "&nbsp;"

                    var txtMonth = null
                    switch (intMonth) {
                        case 1:
                            txtMonth = "January"; break;
                        case 2:
                            txtMonth = "February"; break;
                        case 3:
                            txtMonth = "March"; break;
                        case 4:
                            txtMonth = "April"; break;
                        case 5:
                            txtMonth = "May"; break;
                        case 6:
                            txtMonth = "June"; break;
                        case 7:
                            txtMonth = "July"; break;
                        case 8:
                            txtMonth = "August"; break;
                        case 9:
                            txtMonth = "September"; break;
                        case 10:
                            txtMonth = "October"; break;
                        case 11:
                            txtMonth = "November"; break;
                        case 12:
                            txtMonth = "December"; break;
                    }

                    if (txtMonth != null) {
                        objFromDateDummy.innerText = txtMonth + ", " + intYear;
                    }
                    else {
                        objFromDateDummy.innerText = "&nbsp;";
                    }
                }
            }
        }


        function CalendarShownToDate(sender, args) {
            //set the default mode to month 
            sender._switchMode("months", true);

            //RedefineHandler(objCalendarFrom);
            //RedefineHandler(objCalendarTo);
        }

        function CalendarHiddenToDate(sender, args) {

            if (sender.get_selectedDate()) {

                objSelectedDate = sender.get_selectedDate();

                var selectedDate = new Date(objSelectedDate);

                var intMonth = selectedDate.getMonth() + 1;
                var intYear = selectedDate.getFullYear();

                if (!isNaN(intMonth) && !isNaN(intYear)) {
                    lblToDateDummy.value = null
                    //if (intMonth < 10) {
                    //    txtToDateDummy.value = '0' + intMonth + '-' + intYear;
                    //}
                    //else {
                    //    txtToDateDummy.value = intMonth + '-' + intYear;
                    //}
                    var txtMonth = null
                    switch (intMonth) {
                        case 1:
                            txtMonth = "January"; break;
                        case 2:
                            txtMonth = "February"; break;
                        case 3:
                            txtMonth = "March"; break;
                        case 4:
                            txtMonth = "April"; break;
                        case 5:
                            txtMonth = "May"; break;
                        case 6:
                            txtMonth = "June"; break;
                        case 7:
                            txtMonth = "July"; break;
                        case 8:
                            txtMonth = "August"; break;
                        case 9:
                            txtMonth = "September"; break;
                        case 10:
                            txtMonth = "October"; break;
                        case 11:
                            txtMonth = "November"; break;
                        case 12:
                            txtMonth = "December"; break;
                    }

                    if (txtMonth != null) {
                        lblToDateDummy.innerText = txtMonth + ", " + intYear;
                    }
                    else {
                        lblToDateDummy.innerText = "&nbsp;";
                    }
                }
            }
        }

        function ResetCalendarToDate(objToDateDummyID) {
            var objToDateDummy = document.getElementById(objToDateDummyID);

            if (objCalendarTo != null && objToDateDummy != null) {
                var selectedDate = new Date(objCalendarTo.get_selectedDate());

                var intMonth = selectedDate.getMonth() + 1;
                var intYear = selectedDate.getFullYear();

                if (!isNaN(intMonth) && !isNaN(intYear) && intYear != "1970") {
                    objToDateDummy.innerText = "&nbsp;"

                    var txtMonth = null
                    switch (intMonth) {
                        case 1:
                            txtMonth = "January"; break;
                        case 2:
                            txtMonth = "February"; break;
                        case 3:
                            txtMonth = "March"; break;
                        case 4:
                            txtMonth = "April"; break;
                        case 5:
                            txtMonth = "May"; break;
                        case 6:
                            txtMonth = "June"; break;
                        case 7:
                            txtMonth = "July"; break;
                        case 8:
                            txtMonth = "August"; break;
                        case 9:
                            txtMonth = "September"; break;
                        case 10:
                            txtMonth = "October"; break;
                        case 11:
                            txtMonth = "November"; break;
                        case 12:
                            txtMonth = "December"; break;
                    }

                    if (txtMonth != null) {
                        objToDateDummy.innerText = txtMonth + ", " + intYear;
                    }
                    else {
                        objToDateDummy.innerText = "&nbsp;";
                    }
                }
            }
        }

        function ChangeFromDateMMYYYYToDDMMYYYY(objTxtFromDateDummy, objTxtFromDate, objBtnFromDateDummy, objBtnFromDate) {
            if (FromDateInit(objTxtFromDateDummy, objTxtFromDate, objBtnFromDateDummy, objBtnFromDate)) {
                btnFromDateDummy.style.display = "none";
                btnFromDate.style.display = "inline";
            }

            var pattern = new RegExp("^(0[1-9]|1[0-2])-?20\\d{2}$", "g");
            if (pattern.test(lblFromDateDummy.value)) {
                txtFromDate.value = null
                var objDate = new Date();

                if (lblFromDateDummy.value.length == 6) {
                    txtFromDate.value = '01-' + lblFromDateDummy.value.substr(0, 2) + '-' + lblFromDateDummy.value.substr(2, 4);
                    objDate.setFullYear(lblFromDateDummy.value.substr(2, 4), lblFromDateDummy.value.substr(0, 2) - 1, 1);
                }
                else {
                    txtFromDate.value = '01-' + lblFromDateDummy.value;
                    objDate.setFullYear(lblFromDateDummy.value.substr(3, 4), lblFromDateDummy.value.substr(0, 2) - 1, 1);
                }

                objCalendarFrom.show();
                objCalendarFrom.set_selectedDate(objDate);
                objCalendarFrom.hide();
            }
            else {
                objCalendarFrom.show();
                objCalendarFrom.set_selectedDate(null);
                objCalendarFrom.hide();
            }
        }

        function FromDateInit(objTxtFromDateDummy, objTxtFromDate, objBtnFromDateDummy, objBtnFromDate) {
            lblFromDateDummy = document.getElementById(objTxtFromDateDummy);
            txtFromDate = document.getElementById(objTxtFromDate);
            btnFromDateDummy = document.getElementById(objBtnFromDateDummy);
            btnFromDate = document.getElementById(objBtnFromDate);

            if (lblFromDateDummy == null || txtFromDate == null || btnFromDateDummy == null || btnFromDate == null) {
                return false;
            }
            else {
                return true;
            }
        }

        function ChangeToDateMMYYYYToDDMMYYYY(objTxtToDateDummy, objTxtToDate, objBtnToDateDummy, objBtnToDate) {
            if (ToDateInit(objTxtToDateDummy, objTxtToDate, objBtnToDateDummy, objBtnToDate)) {
                btnToDateDummy.style.display = "none";
                btnToDate.style.display = "inline";
            }

            var patternTo = new RegExp("^(0[1-9]|1[0-2])-?20\\d{2}$", "g");
            if (patternTo.test(lblToDateDummy.value)) {
                txtToDate.value = null
                var objDate = new Date();

                if (lblToDateDummy.value.length == 6) {
                    txtToDate.value = '01-' + lblToDateDummy.value.substr(0, 2) + '-' + lblToDateDummy.value.substr(2, 4);
                    objDate.setFullYear(lblToDateDummy.value.substr(2, 4), lblToDateDummy.value.substr(0, 2) - 1, 1);
                }
                else {
                    txtToDate.value = '01-' + lblToDateDummy.value;
                    objDate.setFullYear(lblToDateDummy.value.substr(3, 4), lblToDateDummy.value.substr(0, 2) - 1, 1);
                }

                objCalendarTo.show();
                objCalendarTo.set_selectedDate(objDate);
                objCalendarTo.hide();
            }
            else {
                objCalendarTo.show();
                objCalendarTo.set_selectedDate(null);
                objCalendarTo.hide();
            }
        }

        function ToDateInit(objTxtToDateDummy, objTxtToDate, objBtnToDateDummy, objBtnToDate) {
            lblToDateDummy = document.getElementById(objTxtToDateDummy);
            txtToDate = document.getElementById(objTxtToDate);
            btnToDateDummy = document.getElementById(objBtnToDateDummy);
            btnToDate = document.getElementById(objBtnToDate);

            if (lblToDateDummy == null || txtToDate == null || btnToDateDummy == null || btnToDate == null) {
                return false;
            }
            else {
                return true;
            }
        }

        function CalendarFromDateClick() {
            btnFromDateDummy.style.display = "none";
            btnFromDate.style.display = "inline";
            btnFromDate.style.outline = "none";

            if (!objCalendarFrom.get_isOpen()) {

                objCalendarFrom.show();
                objCalendarFrom.focus();
            }
            else {
                objCalendarFrom.hide();
            }
        }

        function CalendarToDateClick() {
            btnToDateDummy.style.display = "none";
            btnToDate.style.display = "inline";
            btnToDate.style.outline = "none";

            if (!objCalendarTo.get_isOpen()) {

                objCalendarTo.show();
                objCalendarTo.focus();
            }
            else {
                objCalendarTo.hide();
            }
        }

        function RemoveButtonMouseIn(img) {
            img.src = img.src.replace("RemoveX", "RemoveX_red");
        }

        function RemoveButtonMouseOut(img) {
            img.src = img.src.replace("RemoveX_red", "RemoveX");
        }

        function CheckboxTextboxRelation(chk, textboxId) {
            var txt = document.getElementById(textboxId);

            if (chk.checked) {
                txt.disabled = '';
            } else {
                txt.value = '';
                txt.disabled = 'disabled';
            }
        }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
            <asp:PostBackTrigger ControlID="ibtnP3Import" />
            <asp:PostBackTrigger ControlID="ibtnP3IVImportFile" />
        </Triggers>
        <ContentTemplate>
            <asp:Image ID="img_header" runat="server" AlternateText="<%$ Resources:AlternateText, PostPaymentCheckBanner %>"
                ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, PostPaymentCheckBanner %>" />
            <div style="height: 5px"></div>
            <cc1:MessageBox ID="udcErrorMessage" runat="server" Width="950px"></cc1:MessageBox>
            <cc1:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="950px"></cc1:InfoMessageBox>
            <asp:Panel ID="pnlReportInfo" runat="server">
                <div class="headingText">
                    <asp:Label ID="lblReoprtInfoText" runat="server" Text="<%$ Resources:Text, ReportInformation %>" Font-Bold="True" />
                </div>
                <table cellpadding="0" cellspacing="4" style="width: auto">
                    <tr class="ReportInfo_tr">
                        <td align="left" valign="top" style="width: 220px" class="ReportInfo_LabelWidth">
                            <asp:Label ID="lblReportIDText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ReportID %>"></asp:Label>
                        </td>
                        <td align="left" style="width: 730px" class="ReportInfo_ValueWidth">
                            <asp:Label ID="lblReportID" runat="server" CssClass="tableText" Text=""></asp:Label>
                            <asp:HiddenField ID="hfReportID" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblReportNameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ReportName %>"></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblReportName" runat="server" CssClass="tableText" Text=""></asp:Label>
                        </td>
                    </tr>
                </table>
                <br>
            </asp:Panel>
            <asp:MultiView ID="mvPostPaymentCheck" runat="server" OnActiveViewChanged="mvPostPaymentCheck_ActiveViewChanged">
                <asp:View ID="vReportList" runat="server">
                    <asp:GridView ID="gvPostPaymentCheck" AllowPaging="True" AutoGenerateColumns="False"
                        Width="900px" AllowSorting="True" EnableTheming="True" runat="server" OnSelectedIndexChanged="gvPostPaymentCheck_SelectedIndexChanged"
                        OnRowDataBound="gvPostPaymentCheck_RowDataBound" OnPageIndexChanging="gvPostPaymentCheck_PageIndexChanging"
                        OnPreRender="gvPostPaymentCheck_PreRender" OnSorting="gvPostPaymentCheck_Sorting">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" Width="25px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, ReportID %>" SortExpression="Display_Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblDisplayCode" runat="server" Text='<%# Eval("Display_Code")%>'></asp:Label>
                                    <asp:HiddenField ID="hfFileID" runat="server" Value='<%# Eval("File_ID")%>' />
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" Width="120px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="File_Name" SortExpression="File_Name" HeaderText="<%$ Resources:Text, ReportName %>">
                                <ItemStyle VerticalAlign="Top" Width="330px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="File_Desc" SortExpression="File_Desc" HeaderText="<%$ Resources:Text, Remark %>">
                                <ItemStyle VerticalAlign="Top" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                    </asp:GridView>
                    <br />
                </asp:View>
                <asp:View ID="vPPC0001" runat="server">
                    <link href="<%=ResolveUrl("~/CSS/PPC/PPC0001Style.css") %>" type="text/css" rel="Stylesheet">
                    <div class="headingText">
                        <asp:Label ID="lblP1GenerationCriteria" runat="server" Text="<%$ Resources: Text, GenerationCriteria %>" />
                    </div>
                    <uc1:ucStatisticsCriteriaBase ID="ucP1CriteriaBase" runat="server" />
                    <table>
                        <tr style="height: 15px"></tr>
                        <tr>
                            <td style="width: 220px">
                                <asp:ImageButton ID="ibtnP1Back" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnP1Back_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnP1Submit" runat="server" ImageUrl="<%$ Resources: ImageUrl, SubmitBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SubmitBtn %>" OnClick="ibtnP1Submit_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vPPC0002" runat="server">
                    <link href="<%=ResolveUrl("~/CSS/PPC/PPC0002Style.css") %>" type="text/css" rel="Stylesheet" />
                    <table id="tblGenerationCriteria" runat="server" cellpadding="0" cellspacing="0" style="width: 1200px" class="TableStyle1">
                        <tr>
                            <td valign="top" style="width: 430px">
                                <div class="headingText">
                                    <asp:Label ID="lblP2SelectionOfServiceProvider" runat="server" Text="<%$ Resources: Text, SelectionOfServiceProvider %>" />
                                </div>
                            </td>
                            <td valign="top" style="width: 10px" />
                            <td valign="top">
                                <div class="headingText" style="position: relative; left: -2px">
                                    <asp:Label ID="lblP2GenerationCriteria" runat="server" Text="<%$ Resources: Text, GenerationCriteria %>" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="border-style: solid; border-width: 1px; border-color: #333333; padding-top: 5px">
                                            <table cellpadding="0" cellspacing="4" class="P2InputTableSP">
                                                <tr>
                                                    <td style="width: 130px">
                                                        <asp:Label ID="lblP2SPIDText" runat="server" Text="<%$ Resources: Text, ServiceProviderID %>"></asp:Label>
                                                    </td>
                                                    <td style="width: 288px">
                                                        <asp:Panel ID="panP2SPID" runat="server" DefaultButton="ibtnP2SPIDSearch">
                                                            <table cellpadding="0" cellspacing="0">
                                                                <tr>
                                                                    <td style="white-space: nowrap; padding-right: 5px">
                                                                        <asp:TextBox ID="txtP2SPID" runat="server" MaxLength="8" Style="width: 70px;">&nbsp;&nbsp;</asp:TextBox>
                                                                        <asp:Image ID="imgP2SPIDError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                                            AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="Bottom" />
                                                                        <cc2:FilteredTextBoxExtender ID="fteP2SPID" runat="server" TargetControlID="txtP2SPID" FilterType="Numbers">
                                                                        </cc2:FilteredTextBoxExtender>
                                                                    </td>
                                                                    <td>
                                                                        <asp:ImageButton ID="ibtnP2SPIDSearch" runat="server" ImageUrl="<%$ Resources: ImageUrl, SearchSBtn %>"
                                                                            AlternateText="<%$ Resources: AlternateText, SearchSBtn %>" OnClick="ibtnP2SPIDSearch_Click" />
                                                                        <asp:ImageButton ID="ibtnP2SPIDClear" runat="server" ImageUrl="<%$ Resources: ImageUrl, ClearSBtn %>"
                                                                            AlternateText="<%$ Resources: AlternateText, ClearBtn %>" OnClick="ibtnP2SPIDClear_Click" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblP2PracticeText" runat="server" Text="<%$ Resources: Text, Practice %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:DropDownList ID="ddlP2Practice" runat="server" Style="width: 250px" AppendDataBoundItems="true">
                                                        </asp:DropDownList>
                                                        <asp:Image ID="imgP2PracticeError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                            AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="Bottom" />
                                                        <asp:HiddenField ID="hfP2PracticeCount" runat="server" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td></td>
                                                    <td>
                                                        <asp:ImageButton ID="ibtnP2SPAdd" runat="server" ImageUrl="<%$ Resources: ImageUrl, AddSBtn %>"
                                                            AlternateText="<%$ Resources: AlternateText, AddSBtn %>" OnClick="ibtnP2SPAdd_Click" />
                                                    </td>
                                                </tr>
                                                <tr style="height: 10px">
                                                    <td colspan="2">
                                                        <hr />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr>
                                                    <td>
                                                        <asp:Label ID="lblP2ListofSPText" runat="server" Text="<%$ Resources: Text, ListOfServiceProviderMax10 %>"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <table id="tbSPListHeading" runat="server" class="P2SPListHeader" cellpadding="0" cellspacing="0">
                                                            <tr style="height: 24px">
                                                                <td style="width: 29px"></td>
                                                                <td style="width: 78px">
                                                                    <asp:Label ID="lblP2SPListHeadSPID" runat="server" Text="<%$ Resources: Text, SPIDShort %>"></asp:Label>
                                                                </td>
                                                                <td style="width: 177px">
                                                                    <asp:Label ID="lblP2SPListHeadPractice" runat="server" Text="<%$ Resources: Text, Practice %>"></asp:Label>
                                                                </td>
                                                                <td style="width: 88px">
                                                                    <asp:Label ID="lblP2SPListHeadStatus" runat="server" Text="<%$ Resources: Text, Status %>"></asp:Label>
                                                                </td>
                                                                <td style="width: 46px" />
                                                            </tr>
                                                        </table>
                                                        <asp:Panel ID="pnlSPList" ClientIDMode="Inherit" runat="server" Width="422px" Height="253px" ScrollBars="Vertical" BorderWidth="1">
                                                            <asp:GridView ID="gvP2SPList" runat="server" Width="100%" AutoGenerateColumns="false"
                                                                OnRowDataBound="gvP2SPList_RowDataBound" OnRowDeleting="gvP2SPList_RowDeleting" ShowHeader="false">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGIndex" runat="server" Text='<%# Eval("Index")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" Width="20px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGSPID" runat="server" Text='<%# Eval("SPID")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" Width="70px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGPractice" runat="server" Text='<%# Eval("PracticeName")%>'></asp:Label>
                                                                            <asp:HiddenField ID="hfGPracticeID" runat="server" Value='<%# Eval("PracticeID")%>' />
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblGPracticeStatus" runat="server" Text='<%# Eval("PracticeStatus")%>'></asp:Label>
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" Width="80px" />
                                                                    </asp:TemplateField>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:ImageButton ID="ibtnGRemove" runat="server" ImageUrl="<%$ Resources: ImageUrl, RemoveXSBtn %>"
                                                                                AlternateText="<%$ Resources: AlternateText, RemoveXSBtn %>" CommandName="Delete"
                                                                                onmouseover="RemoveButtonMouseIn(this);" onmouseout="RemoveButtonMouseOut(this);" />
                                                                        </ItemTemplate>
                                                                        <ItemStyle VerticalAlign="Top" HorizontalAlign="Center" Width="20px" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </asp:Panel>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                            <td></td>
                            <td>
                                <table id="tblGenerationCriteriaContent" runat="server" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td style="border-style: solid; border-width: 1px; border-color: #333333; padding-top: 5px">
                                            <uc1:ucStatisticsCriteriaBase ID="ucP2CriteriaBase" runat="server" />
                                            <table class="P2TargetTransaction" cellpadding="0" cellspacing="4">
                                                <tr>
                                                    <td colspan="2">
                                                        <asp:Label ID="lblP2TargetTranText" runat="server" Text="<%$ Resources: Text, TargetedNumberOfTransaction %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtP2TargetTran" runat="server" Width="30"></asp:TextBox>
                                                        <asp:Label ID="lblP2TargetTranRemark" runat="server" Text="[CodeBehind]"></asp:Label>
                                                        <cc2:FilteredTextBoxExtender ID="fteP2TargetTran" runat="server" TargetControlID="txtP2TargetTran" FilterType="Numbers">
                                                        </cc2:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Image ID="imgP2TargetTranError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                            AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="Bottom" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 20px">
                                                        <asp:CheckBox ID="cboP2TargetTranHighestClaim" runat="server" />
                                                    </td>
                                                    <td style="width: 200px">
                                                        <asp:Label ID="lblP2TargetTranHighestClaim" runat="server" Text="<%$ Resources: Text, NumberOfTransactionWithTheHighestAmountClaim %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtP2TargetTranHighestClaim" runat="server" Width="30"></asp:TextBox>
                                                        <asp:Label ID="lblP2TargetTranHighestClaimRemark" runat="server" Text="[CodeBehind]"></asp:Label>
                                                        <cc2:FilteredTextBoxExtender ID="fteP2TargetTranHighestClaim" runat="server" TargetControlID="txtP2TargetTranHighestClaim" FilterType="Numbers">
                                                        </cc2:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Image ID="imgP2TargetTranHighestClaimError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                            AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="Bottom" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="cboP2TargetTranManualInput" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblP2TargetTranManualInput" runat="server" Text="<%$ Resources: Text, NumberOfManualInput %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtP2TargetTranManualInput" runat="server" Width="30"></asp:TextBox>
                                                        <asp:Label ID="lblP2TargetTranManualInputRemark" runat="server" Text="[CodeBehind]"></asp:Label>
                                                        <cc2:FilteredTextBoxExtender ID="fteP2TargetTranManualInput" runat="server" TargetControlID="txtP2TargetTranManualInput" FilterType="Numbers">
                                                        </cc2:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Image ID="imgP2TargetTranManualInputError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                            AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="Bottom" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="cboP2TargetTranSmartICInput" runat="server" />
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblP2TargetTranSmartICInput" runat="server" Text="<%$ Resources: Text, NumberOfSmartICInput %>"></asp:Label>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox ID="txtP2TargetTranSmartICInput" runat="server" Width="30"></asp:TextBox>
                                                        <asp:Label ID="lblP2TargetTranSmartICInputRemark" runat="server" Text="[CodeBehind]"></asp:Label>
                                                        <cc2:FilteredTextBoxExtender ID="fteP2TargetTranSmartICInput" runat="server" TargetControlID="txtP2TargetTranSmartICInput" FilterType="Numbers">
                                                        </cc2:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:Image ID="imgP2TargetTranSmartICInputError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                            AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="Bottom" />
                                                    </td>
                                                </tr>
                                            </table>
                                            <table>
                                                <tr id="trP2InsufficientTransaction" runat="server">
                                                    <td align="left" valign="top" style="width: 20px">
                                                        <asp:CheckBox ID="cboP2InsuffTran" runat="server" />
                                                    </td>
                                                    <td align="left" valign="top" colspan="3">
                                                        <div style="width: 460px; padding-bottom: 5px">
                                                            <asp:Label ID="lblP2InsufficientTransaction" runat="server" Text="<%$ Resources: Text, PPCInsufficientTransaction %>"></asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                    <table>
                        <tr style="height: 15px"></tr>
                        <tr>
                            <td style="width: 424px">
                                <asp:ImageButton ID="ibtnP2Back" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnP2Back_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnP2Submit" runat="server" ImageUrl="<%$ Resources: ImageUrl, SubmitBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SubmitBtn %>" OnClick="ibtnP2Submit_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vPPC0003" runat="server">
                    <link href="<%=ResolveUrl("~/CSS/PPC/PPC0003Style.css") %>" type="text/css" rel="Stylesheet">
                    <div class="headingText">
                        <asp:Label ID="lblP3GenerationCriteria" runat="server" Text="Generation Criteria" />
                    </div>
                    <table class="P3SelectionOfSP" cellpadding="0" cellspacing="4">
                        <tr>
                            <td style="width: 250px">
                                <asp:Label ID="lblP3SelectionOfServiceProvider" runat="server" Text="<%$ Resources: Text, SelectionOfServiceProvider %>"></asp:Label>
                            </td>
                            <td style="width: 600px">
                                <table cellpadding="0" cellspacing="0" class="TableStyle1">
                                    <tr>
                                        <td>
                                            <asp:Label ID="lblP3ASupplementaryList" runat="server" Text="<%$ Resources: Text, ASupplementaryListOptional %>"></asp:Label>
                                        </td>
                                        <td style="padding-left: 52px">
                                            <asp:Label ID="lblP3NoOfRecordImport" runat="server"></asp:Label>
                                        </td>
                                        <td style="padding-left: 20px">
                                            <asp:ImageButton ID="ibtnP3Import" runat="server" AutoPostBack="True" ImageUrl="<%$ Resources: ImageUrl, ImportSBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, SelectSBtn %>" OnClick="ibtnP3Import_Click" Visible="false" />
                                            <asp:ImageButton ID="ibtnP3View" runat="server" ImageUrl="<%$ Resources: ImageUrl, ViewSBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ViewDetailsBtn %>" OnClick="ibtnP3View_Click" Visible="false" />
                                            <asp:ImageButton ID="ibtnP3Clear" runat="server" ImageUrl="<%$ Resources: ImageUrl, ClearSBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ClearBtn %>" OnClick="ibtnP3Clear_Click" Visible="false" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td>
                                <table cellpadding="0" cellspacing="0" class="TableStyle1">
                                    <tr>
                                        <td style="width: 262px">
                                            <asp:Label ID="lblP3BPercentageOfServiceProvider" runat="server" Text="<%$ Resources: Text, BPercentageOfServiceProvider %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtP3PercentageOfSP" runat="server"></asp:TextBox>
                                            <cc2:FilteredTextBoxExtender ID="fteP3PercentageOfSP" runat="server" TargetControlID="txtP3PercentageOfSP" FilterType="Numbers">
                                            </cc2:FilteredTextBoxExtender>
                                        </td>
                                        <td style="padding-top: 3px; padding-left: 3px">
                                            <asp:Label ID="lblP3PercentageOfSPRange" runat="server" Text="[CodeBehind]"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:Image ID="imgP3PercentageOfSPRangeError" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="Bottom" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                            <td style="padding-left: 20px">
                                <asp:Label ID="lblP3PercentageOfSPRemark" runat="server" Text="(Excluded the Service Provider in Supplementary List)"></asp:Label>
                            </td>
                        </tr>
                    </table>
                    <uc1:ucStatisticsCriteriaBase ID="ucP3CriteriaBase" runat="server" />
                    <table>
                        <tr style="height: 15px"></tr>
                        <tr>
                            <td style="width: 250px">
                                <asp:ImageButton ID="ibtnP3Back" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnP3Back_Click" />
                            </td>
                            <td>
                                <asp:ImageButton ID="ibtnP3Submit" runat="server" ImageUrl="<%$ Resources: ImageUrl, SubmitBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SubmitBtn %>" OnClick="ibtnP3Submit_Click" />
                            </td>
                        </tr>
                    </table>
                    <%--Import Popup Window--%>
                    <asp:Button ID="btnP3Import" runat="server" Style="display: none" />
                    <cc2:ModalPopupExtender ID="mpeP3Import" runat="server" TargetControlID="btnP3Import"
                        PopupControlID="panP3Import" PopupDragHandleControlID="panP3ImportHeader" BackgroundCssClass="modalBackgroundTransparent"
                        DropShadow="False" RepositionMode="None">
                    </cc2:ModalPopupExtender>
                    <asp:Panel ID="panP3Import" runat="server" Style="display: none">
                        <asp:Panel ID="panP3ImportHeader" runat="server" Style="cursor: move">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 800px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                                    <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                        <asp:Label ID="lblP3ImportHeader" runat="server" Text="<%$ Resources: Text, ImportServiceProviderSupplementaryList %>"></asp:Label>
                                    </td>
                                    <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table cellpadding="0" cellspacing="0" style="width: 800px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                                <td style="background-color: #ffffff">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <cc1:MessageBox ID="udcP3ImportMessageBox" runat="server" Width="100%"></cc1:MessageBox>
                                                <cc1:InfoMessageBox ID="udcP3ImportInfoMessageBox" runat="server" Width="100%"></cc1:InfoMessageBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                <asp:MultiView ID="mvP3Import" runat="server" ActiveViewIndex="0">
                                                    <asp:View ID="vP3ImportUploadFile" runat="server">
                                                        <table>
                                                            <tr style="height: 10px"></tr>
                                                            <tr>
                                                                <td style="width: 70px; padding-left: 20px">
                                                                    <asp:Label ID="lblP3IUFileText" runat="server" Text="<%$ Resources: Text, File %>"></asp:Label>
                                                                </td>
                                                                <td style="width: 580px">
                                                                    <asp:FileUpload ID="flP3IUFile" runat="server" Width="550px" />
                                                                    <asp:Image ID="imgP3IUFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                                        AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageAlign="Bottom" />
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 20px"></tr>
                                                        </table>
                                                    </asp:View>
                                                    <asp:View ID="vP3ImportViewRecord" runat="server">
                                                        <table class="TableStyle1" style="width: 100%">
                                                            <tr id="trP3IVFile" runat="server" style="height: 30px">
                                                                <td style="width: 150px">
                                                                    <asp:Label ID="lblP3IVFileText" runat="server" Text="<%$ Resources: Text, File %>"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblP3IVFile" runat="server" CssClass="tableText"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 30px">
                                                                <td style="width: 150px">
                                                                    <asp:Label ID="lblP3IVNoOfRecordText" runat="server" Text="[CodeBehind]"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Label ID="lblP3IVNoOfRecord" runat="server" CssClass="tableText"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:Label ID="lblP3IVListText" runat="server" Text="<%$ Resources: Text, List %>"></asp:Label>
                                                                </td>
                                                                <td>
                                                                    <asp:Panel ID="panP3IVList" ClientIDMode="Inherit" runat="server" Width="600px" Height="300px" ScrollBars="Vertical" BorderWidth="1">
                                                                        <asp:GridView ID="gvP3IVList" runat="server" Width="100%" AutoGenerateColumns="false"
                                                                            OnRowDataBound="gvP3IVList_RowDataBound">
                                                                            <Columns>
                                                                                <asp:TemplateField HeaderText="<%$ Resources: Text, ExcelRowNo %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGRowNo" runat="server" Text='<%# Eval("Row_No")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources: Text, ServiceProviderID %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGSPID" runat="server" Text='<%# Eval("SPID")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle VerticalAlign="Top" Width="140px" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources: Text, ServiceProviderName %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGSPName" runat="server" Text='<%# Eval("SP_Name")%>'></asp:Label>
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle VerticalAlign="Top" />
                                                                                </asp:TemplateField>
                                                                                <asp:TemplateField HeaderText="<%$ Resources: Text, Validation %>">
                                                                                    <ItemTemplate>
                                                                                        <asp:Label ID="lblGValidation" runat="server" Text='<%# Eval("Fail_Type")%>' ForeColor="Red"></asp:Label>
                                                                                        <asp:Image ID="imgGValidation" runat="server" ImageUrl="<%$ Resources: ImageUrl, Warning %>"
                                                                                            AlternateText="<%$ Resources: AlternateText, ErrorImg %>" />
                                                                                    </ItemTemplate>
                                                                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                                                                </asp:TemplateField>
                                                                            </Columns>
                                                                        </asp:GridView>
                                                                    </asp:Panel>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:View>
                                                </asp:MultiView>
                                            </td>
                                        </tr>
                                        <tr style="height: 50px">
                                            <td style="text-align: center; vertical-align: middle">
                                                <asp:ImageButton ID="ibtnP3IVImportFile" runat="server" ImageUrl="<%$ Resources: ImageUrl, ImportFileBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ImportFileBtn %>" OnClick="ibtnP3IVImportFile_Click" />
                                                <asp:ImageButton ID="ibtnP3IVConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnP3IVConfirm_Click" />
                                                <asp:ImageButton ID="ibtnP3IVCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnP3IVCancel_Click" />
                                                <asp:ImageButton ID="ibtnP3IVClose" runat="server" ImageUrl="<%$ Resources: ImageUrl, CloseBtn %>"
                                                    AlternateText="<%$ Resources: AlternateText, CloseBtn %>" OnClick="ibtnP3IVClose_Click" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                            </tr>
                            <tr>
                                <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                                <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                                <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%-- End of Import Popup Window --%>
                </asp:View>
                <asp:View ID="vReturn" runat="server">
                    <table>
                        <tr>
                            <td style="padding-top: 10px">
                                <asp:ImageButton ID="ibtnReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnReturn_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
