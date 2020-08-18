<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcDateFormatMonthAndYear.ascx.vb"
    Inherits="HCVU.udcDateFormatMonthAndYear" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<table>
    <asp:Panel ID="panDateFormat" runat="server">
        <tr style="height:24px" class ="FormatOfDateMY_tr">
            <td style="width: 150px;text-align:left;vertical-align:top" class ="FormatOfDateMY_FormatLabelWidth">
                <asp:Label ID="lblDateFormatText" runat="server" CssClass="FormatOfDateMY_FormatLabel" Text="<%$ Resources:Text, FormatOfDate %>"/>
            </td>
            <td style="text-align:left; width: 600px; vertical-align:top" class ="FormatOfDateMY_FormatValueWidth">
                <asp:Label ID="lblDateFormat" runat="server" CssClass="FormatOfDateMY_FormatValue" Text="<%$ Resources:Text, MonthAndYear %>"/>
            </td>
        </tr>
        <tr id="trMonthAndYear" style="height:24px" class ="FormatOfDateMY_tr" runat="server">
            <td style="text-align:left;vertical-align:top">
                <asp:Label ID="lblDateText" runat="server" CssClass="FormatOfDateMY_DateLabel" Text="Date"></asp:Label>
            </td>
            <td style="vertical-align:top">
                <div class="FormatOfDateMY_DateValue">
                    <!-- Real Textbox -->
                    <asp:TextBox ID="txtFromDate_MY" MaxLength="10" runat="server" Width="1px" Height="13px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                        onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                        onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);" 
                        style="z-index:-1;position:relative;left:0px;top:-1px;"></asp:TextBox>
                    
                    <cc1:FilteredTextBoxExtender ID="txtExFromDate" runat="server" FilterType="Custom, Numbers"
                        TargetControlID="txtFromDate_MY" ValidChars="-" Enabled="True">
                    </cc1:FilteredTextBoxExtender>

                    <!-- Dummy Label -->
                    <asp:Label ID="lblFromDateDummy" runat="server" EnableViewState="false" Width="110px" Height="17px" BorderWidth="1" BorderColor="#666666" Text ="&nbsp;"
                        style="padding-left:2px;padding-top:1px;z-index:1;position:relative;left:-9px;top:-2px;background-color:white;color:black;"></asp:Label>

                    <!-- Dummy ImageButton -->
                    <asp:ImageButton ID="btnFromDateDummy" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server"
                        style="z-index:1;position:relative;left:-9px;top:1px;" />

                    <!-- Real ImageButton -->
                    <asp:ImageButton ID="btnFromDate_MY" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" style="z-index:1;position:relative;left:-9px;top:1px;"/>
                    <cc1:CalendarExtender ID="calExFromDate_MY" BehaviorID="calExFromDate_MY" CssClass="ajax_cal" runat="server" PopupButtonID="btnFromDate_MY"
                        TargetControlID="txtFromDate_MY" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" DefaultView="Months" 
                        OnClientShown="CalendarShownFromDate" OnClientHidden="CalendarHiddenFromDate">
                    </cc1:CalendarExtender> 

    <%--                <asp:Image ID="imgErrorFromDate_MY" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                        Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" style="z-index:1;position:relative;left:-9px;top:2px;"/>--%>

                    <asp:Panel ID="pnlToDate" runat="server" CssClass="FormatOfDateMY_PanelDisplay">
                        <asp:Label ID="lblToDate_MY" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, To_S %>" style="position:relative;top:-2px;"></asp:Label>&nbsp;&nbsp;&nbsp;

                        <!-- Real Textbox -->
                        <asp:TextBox ID="txtToDate_MY" MaxLength="10" runat="server" Width="1px" Height="13px" 
                            onkeydown="filterDateInputKeyDownHandler(this, event);"
                            onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                            onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);" 
                            style="z-index:-1;position:relative;left:0px;top:-1px;"></asp:TextBox>
                    
                        <cc1:FilteredTextBoxExtender ID="filterToDate" runat="server" FilterType="Custom, Numbers"
                            TargetControlID="txtToDate_MY" ValidChars="-" Enabled="True">
                        </cc1:FilteredTextBoxExtender>

                        <!-- Dummy Label -->
                        <asp:Label ID="lblToDateDummy" runat="server" EnableViewState="false"  Width="110px" Height="17px" BorderWidth="1" BorderColor="#666666" Text ="&nbsp;" 
                            style="padding-left:2px;padding-top:1px;z-index:1;position:relative;left:-9px;top:-2px;background-color:white;color:black;"></asp:Label>

                        <!-- Dummy ImageButton -->
                        <asp:ImageButton ID="btnToDateDummy" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" style="z-index:1;position:relative;left:-9px;top:1px;"/>

                        <!-- Real ImageButton -->
                        <asp:ImageButton ID="btnToDate_MY" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" style="z-index:1;position:relative;left:-9px;top:1px;"/>
                        <cc1:CalendarExtender ID="calExToDate_MY" BehaviorID="calExToDate_MY" CssClass="ajax_cal" runat="server" PopupButtonID="btnToDate_MY"
                            TargetControlID="txtToDate_MY" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" DefaultView="Months" 
                            OnClientShown="CalendarShownToDate" OnClientHidden="CalendarHiddenToDate">
                        </cc1:CalendarExtender>

                        <%--<asp:Image ID="imgErrorToDate_MY" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                            Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" style="z-index:1;position:relative;left:-9px;top:2px;"/>--%>
                    </asp:Panel>
                        
                    <asp:Image ID="imgErrorDate_MY" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                            Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" style="z-index:1;position:relative;left:-9px;top:2px;"/>

                </div>
            </td>
        </tr>
    </asp:Panel>
</table>
