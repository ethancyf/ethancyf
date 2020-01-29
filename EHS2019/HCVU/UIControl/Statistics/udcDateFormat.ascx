<%@ Control Language="vb" AutoEventWireup="false" Codebehind="udcDateFormat.ascx.vb"
    Inherits="HCVU.udcDateFormat" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<table>
    <asp:Panel ID="panDateFormat" runat="server">
        <tr style="height:30px" class ="FormatOfDate_tr">
            <td align="left" style="width: 220px; vertical-align: top" class ="FormatOfDate_LabelWidth">
                <asp:Label ID="lblDateFormatText" runat="server"/>
            </td>
            <td style="text-align:left; width: 600px; vertical-align:top" class ="FormatOfDate_ValueWidth">
                <table cellpadding="0" cellspacing="0">
                    <tr>
                        <td style="width: 250px; vertical-align: top">
                            <asp:RadioButtonList ID="rbtnDateFormat" runat="server" RepeatDirection="Horizontal" CellSpacing="0" CellPadding="0" AutoPostBack="true" CssClass="FormatOfDate_RadioButtonList">
                                <asp:ListItem Text="<%$ Resources:Text, ExactDate %>" Value="E"></asp:ListItem>
                                <asp:ListItem Text="<%$ Resources:Text, MonthAndYear %>" Value="M"></asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                        <td style="width: 20px; vertical-align: top">
                            <asp:Image ID="imgErrorDateFormat" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="position:relative;top:-2px" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr id="trExactDate" style="height:30px" runat="server">
            <td align="left" style="vertical-align: top">
                <asp:Label ID="lblDateText" runat="server" CssClass="tableTitle" Text="Date"></asp:Label>
            </td>
            <td style="vertical-align: top">
                <!-- Real Textbox -->
                <asp:TextBox ID="txtFromDate_D" MaxLength="10" runat="server" Width="70px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                    onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                    onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);" Style="position:relative;top:-2px;"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="filterFromDate_D" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtFromDate_D" ValidChars="-" Enabled="True">
                </cc1:FilteredTextBoxExtender>
                
                <asp:ImageButton ID="btnFromDate_D" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server"  Style="position:relative;top:1px;"/>
                <cc1:CalendarExtender ID="calExFromDate_D" BehaviorID="calExFromDate_D" CssClass="ajax_cal" runat="server" PopupButtonID="btnFromDate_D"
                    TargetControlID="txtFromDate_D" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                </cc1:CalendarExtender>
                &nbsp;&nbsp;
                <asp:Label ID="lblToDate" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, To_S %>" Style="position:relative;top:-2px;"></asp:Label>&nbsp;&nbsp;&nbsp;

                <!-- Real Textbox -->
                <asp:TextBox ID="txtToDate_D" MaxLength="10" runat="server" Width="70px" onkeydown="filterDateInputKeyDownHandler(this, event);"
                    onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);"
                    onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);" Style="position:relative;top:-2px;"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="filterToDate_D" runat="server" FilterType="Custom, Numbers"
                    TargetControlID="txtToDate_D" ValidChars="-" Enabled="True">
                </cc1:FilteredTextBoxExtender>

                <asp:ImageButton ID="btnToDate_D" AlternateText="<%$ Resources:AlternateText, CalenderBtn %>"
                    ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" runat="server" Style="position:relative;top:1px;"/>
                <cc1:CalendarExtender ID="calExToDate_D" BehaviorID="calExToDate_D" CssClass="ajax_cal" runat="server" PopupButtonID="btnToDate_D"
                    TargetControlID="txtToDate_D" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True">
                </cc1:CalendarExtender>                    
                <asp:Image ID="imgErrorDate_D" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Style="vertical-align: top" />
            </td>
        </tr>
        <tr id="trMonthAndYear" style="height:30px" runat="server">
            <td align="left" style="vertical-align: top">
                <asp:Label ID="Label1" runat="server" CssClass="tableTitle" Text="Date"></asp:Label>
            </td>
            <td style="vertical-align: top">
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

                <asp:Label ID="lblToDate_MY" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, To_S %>" style="position:relative;top:-2px;"></asp:Label>&nbsp;&nbsp;&nbsp;

                <!-- Real Textbox -->
                <asp:TextBox ID="txtToDate_MY" MaxLength="10" runat="server" Width="1px" Height="13px" onkeydown="filterDateInputKeyDownHandler(this, event);"
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
                
                <asp:Image ID="imgErrorDate_MY" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                    Visible="false" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" style="z-index:1;position:relative;left:-9px;top:2px;"/>

            </td>
        </tr>
    </asp:Panel>
</table>
