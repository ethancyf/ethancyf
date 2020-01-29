<%@ Control Language="vb" AutoEventWireup="false" Codebehind="ucTypeOfPracticeGrid.ascx.vb"
    Inherits="HCSP.ucTypeOfPracticeGrid" %>
<style type="text/css">
.RadioButtonListNoWrap tr td label
{
    white-space:nowrap ;
}
</style>
<script type="text/javascript">

function getParentNodeByTagName(obj, tagName) {
    tagName = tagName.toUpperCase();
    while (obj.tagName != tagName) {
        var parent = obj.parentNode;
        obj = parent;
        //alert(obj.tagName);    
    }
    return obj;
}

function chkSelectAllCheckChanged(obj) {
    var row = getParentNodeByTagName(obj, 'table');
    var inputs = row.getElementsByTagName('input');
    for (var j = 0; j < inputs.length; j++) {
        var input = inputs[j];
        if (obj.checked == true){
            if (input.getAttribute('type') == 'checkbox') {
                input.checked = true;
            }
            if (input.getAttribute('type') == 'radio') {
                input.parentNode.disabled = false;
            }
        }
        else {
            if (input.getAttribute('type') == 'checkbox') {
                input.checked = false;
            }
            if (input.getAttribute('type') == 'radio') {
                input.parentNode.disabled = true;
                input.checked = false;
            }        
        }
    }   
};

function TypeOfPracticeRadioSetup(id) {
    //alert(id);
	var chk = document.getElementById(id);
	var row = getParentNodeByTagName(chk, 'tr');
	var inputs = row.getElementsByTagName('input');
	for (var j = 0; j < inputs.length; j++) {
		var input = inputs[j];
		if (input.getAttribute('type') == 'radio') {
			//alert(input.id);
			if (chk.checked == true) {
			    input.parentNode.disabled = false;
			}
			else {
			    input.parentNode.disabled = true;
			    input.checked = false;
			}
		}
	}
}

</script>
<asp:GridView ID="gvPracticeInfo" runat="server" AutoGenerateColumns="False" Width="100%">
    <Columns>
        <%-- CheckBox --%>
        <asp:TemplateField HeaderText="">
            <HeaderStyle Wrap="false" />
            <HeaderTemplate>
                <asp:CheckBox ID="chkSelectAll" runat="server" onClick="javascript:chkSelectAllCheckChanged(this);" />
            </HeaderTemplate>
            <ItemStyle VerticalAlign="Top" />
            <ItemTemplate>
                <asp:CheckBox ID="chkSelect" runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
        <%-- Practice No --%>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label ID="lblPracticeNoHeader" runat="server" Text="<%$ Resources:Text, PracticeNo %>"></asp:Label>
            </HeaderTemplate>
            <HeaderStyle Wrap="false" />
            <ItemStyle VerticalAlign="Top" Width="100px" />
            <ItemTemplate>
                <asp:Label ID="lblPracticeDisplaySeq" runat="server" Text='<%# Eval("DisplaySeq") %>' />
            </ItemTemplate>
        </asp:TemplateField>
        <%-- SP Name, Address, Phone No --%>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label ID="lblPracticeInformationHeader" runat="server" Text="<%$ Resources:Text, PracticeInfo %>"></asp:Label>
            </HeaderTemplate>
            <HeaderStyle Wrap="false" />
            <ItemStyle VerticalAlign="Top" Width="100%" />
            <ItemTemplate>
<%--                <table id="tblPractice" style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblPracticeName" runat="server" Text='<%# Eval("PracticeName") + " <br/>(" + Eval("PracticeNameChi") + ")" %>'
                                CssClass="tableText" />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPracticeAddress" runat="server" Text='<%# "Address: " + formatAddress(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.Building"), Eval("PracticeAddress.District")) %>'
                                CssClass="tableText" />
                            <br />
                            <br />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblPhoneNo" runat="server" Text='<%# "Phone No.: " + Eval("PhoneDaytime") %>'
                                CssClass="tableText" />
                        </td>
                    </tr>
                </table>--%>
                <table id="tblPractice" style="width: 100%">
                    <tr>
                        <td>
                            <asp:Label ID="lblRegBankPracticeEName" runat="server" Text='<%# Eval("PracticeName")  %>'
                                CssClass="tableText"></asp:Label>
                            <asp:Label ID="lblRegBankPracticeCName" runat="server" Text='<%# Eval("PracticeNameChi")%>'
                                CssClass="TextChi"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 10px;">
                            <asp:Label ID="lblRegBankPracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.Building"), Eval("PracticeAddress.District")) %>'
                                CssClass="tableText"></asp:Label><br />
                            <asp:Label ID="lblRegBankPracticeAddressChi" runat="server" Text='<%# formatChineseString(formatChiAddress(Eval("PracticeAddress.Room"), Eval("PracticeAddress.Floor"), Eval("PracticeAddress.Block"), Eval("PracticeAddress.ChiBuilding"), Eval("PracticeAddress.District"))) %>'
                                CssClass="TextChi"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td style="padding-top: 10px;">
                            <asp:Label ID="lblRegBankMOName" runat="server" CssClass="tableText" Text='<%# Bind("PhoneDaytime") %>'></asp:Label>
                        </td>
                    </tr>
                    <tr id="trRegBankPracticeNonClinic" runat="server">
                        <td style="padding-top: 10px;">
                            <asp:Label ID="lblRegBankPracticeNonClinic" runat="server" CssClass="tableText" ></asp:Label>
                        </td>
                    </tr> 
                </table>
            </ItemTemplate>
        </asp:TemplateField>
        <%-- Practice Profession --%>
        <asp:TemplateField>
            <HeaderTemplate>
                    <asp:Label ID="lblHealthProfessionHeader" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
            </HeaderTemplate>
            <HeaderStyle Wrap="false" />
            <ItemStyle VerticalAlign="Top" />
            <ItemTemplate>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblHealthProf" runat="server" Text='<%# GetHealthProfName(Eval("Professional.ServiceCategoryCode")) %>'
                                CssClass="tableText" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblRegCode" runat="server" Text='<%# "(" + Eval("Professional.RegistrationCode") + ")" %>'
                                CssClass="tableText" />
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:TemplateField>
        <%-- Scheme Name --%>
        <asp:TemplateField Visible="false">
            <HeaderTemplate>
                    <asp:Label ID="lblSchemeNameHeader" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
            </HeaderTemplate>
            <HeaderStyle Wrap="false" />
            <ItemStyle VerticalAlign="Top"/>
            <ItemTemplate>
                <table style="width: 100%" cellpadding="0" cellspacing="0">
                    <tr>
                        <td>
                            <asp:Label ID="lblSchemeName" runat="server"  CssClass="tableText" />
                        </td>
                    </tr>
                </table>
            </ItemTemplate>
        </asp:TemplateField>
        <%-- Type of Practice--%>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label ID="lblTypeOfPracticeHeader" runat="server" Text="<%$ Resources:Text, PracticeType %>"></asp:Label>
                <asp:Label ID="lblTypeOfPracticeHeaderRequired" runat="server" Text=""></asp:Label>
            </HeaderTemplate>
            <HeaderStyle Wrap="false" />
            <ItemStyle VerticalAlign="Top" />
            <ItemTemplate>
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td style="white-space:nowrap">
                            <asp:RadioButtonList ID="rdlTypeOfPractice" runat="server" CssClass="RadioButtonListNoWrap"
                                DataTextField="Name" DataValueField="ItemNo" />
                        </td>
                        <td style="padding-left:10px; width:50px; vertical-align:top;">
                            <asp:Image ID="imgAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" ToolTip="<%$ Resources:AlternateText, ErrorImg %>"
                                Visible="False" />
                        </td>
                    </tr>
                </table>   
            </ItemTemplate>
        </asp:TemplateField>
        <%-- Type of Practice View--%>
        <asp:TemplateField>
            <HeaderTemplate>
                <asp:Label ID="lblTypeOfPracticeViewHeader" runat="server" Text="<%$ Resources:Text, PracticeType %>"></asp:Label>   
            </HeaderTemplate>
            <HeaderStyle Wrap="false" />
            <ItemStyle VerticalAlign="Top" />
            <ItemTemplate>
                <asp:Label ID="lblTypeOfPractice" runat="server" CssClass="tableText"></asp:Label>
            </ItemTemplate>
            <ItemStyle Wrap="false" />            
        </asp:TemplateField>        
    </Columns>
    <HeaderStyle BackColor="#666666" />
</asp:GridView>
