<%@ Control Language="vb" AutoEventWireup="false" Codebehind="RVPHomeListSearch.ascx.vb" Inherits="HCSP.RVPHomeListSearch" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>

<script type="text/javascript" language="javascript">
    var prm = Sys.WebForms.PageRequestManager.getInstance();
    prm.add_pageLoaded(pageLoaded);
    prm.add_beginRequest(beginRequest);
    var postbackElement;
    var intScorePosition;

    function beginRequest(sender, args) {
        intScorePosition = $get("divScroll").scrollTop;
        postbackElement = args.get_postBackElement();
    }

    function pageLoaded(sender, args) {
        var updatedPanels = args.get_panelsUpdated();

        if (typeof(postbackElement) == "undefined") {
            return;
        }
        if (postbackElement.id.indexOf('gvSelectedRCHRecord') > -1) {
            // setTimeout with 0 second: excute after the page loaded
            setTimeout("loadPosition();", 0);
        }
    }
     
    function loadPosition()
    {
        try {
            $get("divScroll").scrollTop = intScorePosition;
        }
        catch(err) {
        }
    }
</script>
<table border="0" cellpadding="0" cellspacing="0" width="920">
    <tr>
        <td colspan="2">
            <cc2:InfoMessageBox ID="udcMsgBoxInfo" runat="server" Width="99%"></cc2:InfoMessageBox></td>
    </tr>
</table>

<div style="width: 955px; margin:auto">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>
                <asp:Label ID="lblRCHListFilter" runat="server" Text="<%$ Resources:Text, RCHNameFilter %>" Width="210px" CssClass="tableTitle"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtRCHListFilterCriteria" runat="server" MaxLength="255"></asp:TextBox><asp:ImageButton ID="ibtnRCHListFilter" runat="server" ImageUrl="~/Images/button/icon_button/btn_search.png" ImageAlign="AbsMiddle" /></td>
        </tr>
    </table>
   
</div> 
<div id="divScroll" style="width: 955px; height: 560px; overflow: auto; margin:auto">
    <asp:GridView ID="gvSelectedRCHRecord" runat="server" AllowPaging="True" AllowSorting="true" Width="938px" BackColor="White" AutoGenerateColumns="false" RowStyle-Height="50" EnableTheming="true" PageSize="30" ShowHeader="False">
        <Columns>
            <asp:TemplateField ItemStyle-Width="70px">
                <ItemTemplate>
                    <asp:Label ID="lblRCHCode" runat="server" Text='<%# Eval("RCH_Code") %>'></asp:Label>
                    <asp:LinkButton ID="lnkRCHCode" runat="server" Text='<%# Eval("RCH_Code") %>' CommandArgument='<%# Eval("RCH_Code") %>' Visible="false"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="190px">
                <ItemTemplate>
                    <asp:Label ID="lblRCHNameEng" runat="server" Text='<%# Eval("Homename_Eng") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="110px">
                <ItemTemplate>
                    <asp:Label ID="lblRCHNameChi" runat="server" Text='<%# Eval("Homename_Chi") %>'  CssClass="textChi" ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="310px">
                <ItemTemplate>
                    <asp:Label ID="lblRCHAddressEng" runat="server" Text='<%# Eval("Address_Eng") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="220px">
                <ItemTemplate>
                    <asp:Label ID="lblRCHAddressChi" runat="server" Text='<%# Eval("Address_Chi") %>' CssClass="ChineseAddressDisplay"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <SelectedRowStyle CssClass="SelectedRowStyle" />
        <HeaderStyle HorizontalAlign="Center"/>
    </asp:GridView>
</div>
