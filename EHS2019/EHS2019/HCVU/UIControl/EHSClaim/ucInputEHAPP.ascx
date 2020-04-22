<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ucInputEHAPP.ascx.vb" Inherits="HCVU.ucInputEHAPP" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Panel ID="pnlInputEHAPPDetails" runat="server">
    <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td valign="middle" style="width:185px">
                <asp:Label ID="lblCoPayment" runat="server" Text="" CssClass="tableTitle" Height="25px" Width="160px"></asp:Label>
            </td>        
            <td valign="middle">
                  <asp:DropDownList ID="ddlCoPayment" runat="server" Width="200px" AutoPostBack="true"></asp:DropDownList>
            </td>   
              <td valign="middle">
                <asp:Image ID="imgCoPaymentAlert" runat="server" AlternateText="" ImageUrl=""
                           ImageAlign="AbsMiddle" Visible="false" />
            </td>
            <td  valign="top">      <asp:Panel ID="pnlHCV" runat="server">&nbsp;
             <asp:Label ID="lblOpen" runat="server" Text="(" CssClass="tableText" ></asp:Label><asp:Label ID="lblHCVAmount" runat="server" Text="" CssClass="tableText" ></asp:Label>
                 <asp:TextBox ID="txtHCVInputAmount" runat="server" Width="25px" MaxLength="3" ></asp:TextBox>
                         <asp:Image ID="imgHCVInputAmountAlert" runat="server" AlternateText="" ImageUrl=""
                           ImageAlign="AbsMiddle" Visible="false" />
                <cc1:FilteredTextBoxExtender ID="filteredHCVInputAmount" runat="server" FilterType="numbers"
                    TargetControlID="txtHCVInputAmount"> 
                </cc1:FilteredTextBoxExtender>               
                 <asp:Label ID="lblCopaymentAmount" runat="server" Text="" CssClass="tableText" ></asp:Label><asp:Label ID="lblCopaymentInputAmountDisplayOnly" runat="server" Text="" CssClass="tableText" ></asp:Label><asp:Label ID="lblClose" runat="server" Text=")" CssClass="tableText" ></asp:Label>               
                 <div style="display:none" id="InvisibleActualInput">
                 <asp:TextBox ID="txtCopaymentInputAmount" runat="server" Width="27px" MaxLength="3" BackColor="Transparent"  BorderStyle = "None" TabIndex="-1"  CssClass="tableText" style="text-align:center"></asp:TextBox>
                 <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtenderCopaymentInputAmount" runat="server" FilterType="numbers"
                    TargetControlID="txtCopaymentInputAmount"> 
                </cc1:FilteredTextBoxExtender>  
                </div>
                  </asp:Panel>
            </td>
                 
        </tr>
    </table>
</asp:Panel>
