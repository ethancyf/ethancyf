<%@ Control Language="vb" AutoEventWireup="false" Codebehind="OutreachListSearch.ascx.vb" Inherits="HCSP.OutreachListSearch" %>
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
        if (postbackElement.id.indexOf('gvSelectedOutreachRecord') > -1) {
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
                <asp:Label ID="lblOutreachListFilter" runat="server" Width="210px" CssClass="tableTitle"></asp:Label></td>
            <td>
                <asp:TextBox ID="txtOutreachListFilterCriteria" runat="server" MaxLength="255"></asp:TextBox><asp:ImageButton ID="ibtnOutreachListFilter" runat="server" ImageUrl="~/Images/button/icon_button/btn_search.png" ImageAlign="AbsMiddle" /></td>
        </tr>
    </table>
   
</div> 
<div id="divScroll" style="width: 955px; height: 560px; overflow: auto; margin:auto">
    <asp:GridView ID="gvSelectedOutreachRecord" runat="server" AllowPaging="True" AllowSorting="true" Width="938px" BackColor="White" AutoGenerateColumns="false" RowStyle-Height="50" EnableTheming="true" PageSize="30" ShowHeader="False">
        <Columns>
            <asp:TemplateField ItemStyle-Width="70px">
                <ItemTemplate>
                    <asp:Label ID="lblOutreachCode" runat="server"></asp:Label>
                    <asp:LinkButton ID="lnkOutreachCode" runat="server" Visible="false"></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle VerticalAlign="Middle" />
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="190px">
                <ItemTemplate>
                    <asp:Label ID="lblOutreachNameEng" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="110px">
                <ItemTemplate>
                    <asp:Label ID="lblOutreachNameChi" runat="server" CssClass="textChi" ></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="310px">
                <ItemTemplate>
                    <asp:Label ID="lblOutreachAddressEng" runat="server"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField ItemStyle-Width="220px">
                <ItemTemplate>
                    <asp:Label ID="lblOutreachAddressChi" runat="server" CssClass="ChineseAddressDisplay"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
        <SelectedRowStyle CssClass="SelectedRowStyle" />
        <HeaderStyle HorizontalAlign="Center"/>
    </asp:GridView>
</div>
