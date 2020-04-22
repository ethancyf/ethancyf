<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="RecordConfirmation.aspx.vb" Inherits="HCSP.RecordConfirmation" Title="<%$ Resources:Title, DataEntryInputConfirm %>" %>

<%@ Register Src="../ClaimTranEnquiry.ascx" TagName="ClaimTranEnquiry" TagPrefix="uc2" %>
<%@ Register Src="../UIControl/SchemeLegend.ascx" TagName="SchemeLegend" TagPrefix="uc1" %>
<%@ Register Src="../UIControl/DocTypeLegend.ascx" TagName="DocTypeLegend" TagPrefix="uc3" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
        <tr>
            <td>
                <asp:Image ID="imgHeaderRecordConfirmation" runat="server" ImageAlign="AbsMiddle"
                    ImageUrl="<%$ Resources:ImageUrl, DataEntryInputConfirmBanner %>" />
            </td>
        </tr>
        <tr>
            <td>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="780px" />
                        <cc2:MessageBox ID="udcMessageBox" runat="server" Width="780px" />
                        <asp:MultiView ID="mvRecordConfirmation" runat="server">
                            <asp:View ID="vSearchRecord" runat="server">
                                <table style="width: 100%" cellpadding="0" cellspacing="0" border="0">
                                    <tr style="height: 40px">
                                        <td colspan="2">
                                            <asp:Label ID="lblSearchRecord" runat="server" CssClass="tableCaption" Text='<%$ Resources:Text, SearchDataEntryInputRecord %>'></asp:Label>
                                        </td>
                                    </tr>
                                    <tr style="height: 28px">
                                        <td style="width: 200px">
                                            <asp:Label ID="lblConfirmTypeText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, ConfirmType %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rbConfirmTypeList" runat="server" Width="470px" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="tableTitle">
                                                <asp:ListItem Value='<%$ Resources:Text, ClaimTransaction %>' Selected="True"></asp:ListItem>
                                                <asp:ListItem Value='<%$ Resources:Text, eHealthAccount %>'></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                    
                                    <tr style="height: 28px" runat="server" id="trIncludeIncompleteClaim">
                                        
                                        <td  style="width: 200px">
                                        </td>
                                        <td>
                                            <table cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:CheckBox ID="chkIncludeIncompleteClaim" runat="server" CssClass="tableTitle"/>
                                                    </td>
                                                    <td>
                                                        <asp:Label ID="lblIncludeIncompleteClaim" runat="server" CssClass="tableTitle" style="white-space:nowrap;" Text='<%$ Resources: Text, IncludeIncompleteClaim %>'></asp:Label>
                                                    
                                                    </td>
                                                </tr>
                                             </table>
                                        </td>
                                    </tr>
                                    <tr style="height: 28px">
                                        <td style="height: 28px;width: 200px">
                                            <asp:Label ID="lblCutOffDateText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, CutoffDate %>'></asp:Label>
                                        </td>
                                        <td style="height: 28px">
                                            <asp:TextBox ID="txtCutOffDate" runat="server" MaxLength="10" Width="70px" onkeydown="filterDateInputKeyDownHandler(this, event);" onkeyup="filterDateInputKeyUpHandler(this, event);" onchange="filterDateInput(this);" onMouseOver="filterDateInput(this);" onMouseMove="filterDateInput(this);" onblur="filterDateInput(this);"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnCutOffDate" runat="server" AlternateText='<%$ Resources:AlternateText, CalenderBtn %>'
                                                ImageUrl='<%$ Resources:ImageUrl, CalenderBtn %>' />
                                            <asp:Image ID="imgCutOffDateError" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                            <cc1:CalendarExtender ID="calExtCutOffDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnCutOffDate" TargetControlID="txtCutOffDate" Format="dd-MM-yyyy" />
                                            <cc1:FilteredTextBoxExtender ID="filtereditCutOffDate" runat="server" FilterType="Numbers, Custom" TargetControlID="txtCutOffDate" ValidChars="-" />
                                            
                                        </td>
                                    </tr>
                                    <tr style="height: 28px">
                                        <td style="width: 200px">
                                            <asp:Label ID="lblPracticeText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Practice %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPractice" runat="server" Width="450px" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 28px">
                                        <td style="width: 200px">
                                            <asp:Label ID="lblDataEntryText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, DataEntry %>'></asp:Label>                                            
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlDataEntry" runat="server" Width="350px">
                                            </asp:DropDownList>                                           
                                        </td>
                                    </tr>
                                    <tr style="height: 28px">
                                        <td style="width: 200px">
                                            <asp:Label ID="lblSchemeText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Scheme %>'></asp:Label>
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlScheme" runat="server" Width="430" AutoPostBack="true">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr style="height: 28px">
                                        <td>
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl='<%$ Resources:ImageUrl, SearchBtn %>'
                                                AlternateText='<%$ Resources:AlternateText, SearchBtn %>' />
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vClaimTrans" runat="server">
                                <asp:MultiView ID="mvClaimTrans" runat="server">
                                    <asp:View ID="vSearchedResult" runat="server">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                            <tr style="height: 41px">
                                                <td>
                                                    <asp:Label ID="lblClaimRecord" runat="server" CssClass="tableCaption" Text='<%$ Resources:Text, ClaimRecord %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr style="height: 28px">
                                                            <td style="width: 200px">
                                                                <asp:Label ID="lblCutoffDateResultText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, CutoffDate %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 250px">
                                                                <asp:Label ID="lblCutoffDateResult" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="lblPracticeResultText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Practice %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPracticeResult" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                         <tr style="height: 28px">
                                                                    <td style="width: 200px">
                                                                        <asp:Label ID="lblDataEntryResultText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, DataEntry %>'></asp:Label>
                                                                    </td>
                                                                    <td style="width: 250px">
                                                                        <asp:Label ID="lblDataEntryResult" runat="server" CssClass="tableText"></asp:Label>
                                                                    </td>
                                                                    <td style="width: 150px">
                                                                        <asp:Label ID="lblSchemeResultText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Scheme %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblSchemeResult" runat="server" CssClass="tableText"></asp:Label>
                                                                    </td>
                                                           </tr>
                                                            <tr style="height: 28px" runat="server" id="trIncludeIncompleteClaim_vr">
                                                                <td colspan="4">
                                                                    <asp:CheckBox ID="chkIncludeIncompleteClaim_vr" runat="server" CssClass="tableTitle" Text='<%$ Resources: Text, IncludeIncompleteClaim %>' AutoPostBack="true"/>
                                                                </td>
                                                            </tr>                                                           
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                <asp:GridView ID="gvClaimRecord" runat="server" AllowPaging="True" AllowSorting="true"
                                                        Width="1400px" BackColor="White" AutoGenerateColumns="false" OnSelectedIndexChanged="gvClaimRecord_SelectedIndexChanged">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblClaimRecordIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                                                <ItemStyle VerticalAlign="Middle" Width="25px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20px" />
                                                                <HeaderTemplate>
                                                                    <asp:CheckBox ID="chkSelectAll" onclick="javascript: SelectAllCheckboxesByGridId(this);" runat="server" />
                                                                </HeaderTemplate>
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="SchemeCode_TransactionID" HeaderText="<%$ Resources:Text, TransactionNo %>">
                                                                <ItemStyle Wrap="false" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblScheme" runat="server" Text='<%# Eval("Scheme") %>'></asp:Label><br />
                                                                    <asp:LinkButton ID="lbtnTransactionID" runat="server" CommandArgument='<%# Eval("Transaction_ID") %>'></asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="150px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Transaction_Dtm" HeaderText="<%$ Resources:Text, TransactionTime %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransactionDtm" runat="server" Text='<%# Eval("Transaction_Dtm") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="98px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="DocCode_IdentityNum" HeaderText="<%$ Resources:Text, DocTypeIDNL %>" >
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDocType" runat="server" Text='<%# Eval("Doc_Type") %>'></asp:Label>
                                                                   <asp:Label ID="lblIdentityNum" runat="server" Text='<%# Eval("IdentityNum") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="150px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Eng_Name" HeaderText="<%$ Resources:Text, Name %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEngName" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblChiName" runat="server" Visible="false" Font-Names="HA_MingLiu"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="230px" />
                                                            </asp:TemplateField>
                                                            <%-- <asp:TemplateField SortExpression="Total_Unit" HeaderText="<%$ Resources:Text, NoOfUnitRedeem %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalUnit" runat="server" Text='<%# Eval("Total_Unit") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" Width="68px" />
                                                            </asp:TemplateField>--%>
                                                            <%-- <asp:TemplateField SortExpression="Total_Amount" HeaderText='<%$ Resources:Text, TotalAmountSign %>'>--%>
                                                            <asp:TemplateField SortExpression="Total_Amount_RMB" HeaderText='<%$ Resources:Text, TotalRedeemAmountSignRMB %>'>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalAmountRMB" runat="server" Text='<%# Eval("Total_Amount_RMB") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" Width="85px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Total_Amount" HeaderText='<%$ Resources:Text, TotalRedeemAmountSign %>'>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Amount") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" Width="85px" />
                                                            </asp:TemplateField>
                                                            
                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, Status %>" SortExpression="Record_Status">
                                                                <ItemStyle Width="135px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTranListTranStatus" runat="server" Text='<%# Bind("Record_Status") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hfTranListTranStatus" runat="server" Value='<%# Bind("Record_Status") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            
                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, OtherInformationNL %>" Visible="True">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOtherInformation" runat="server" Text="<%$ Resources:Text, Details %>"
                                                                        ForeColor="Blue"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Practice_Name" HeaderText="<%$ Resources:Text, PracticeName %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPracticeName" runat="server" Text='<%# Eval("Practice_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="180px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="DataEntry_By" HeaderText="<%$ Resources:Text, DataEntry %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVia" runat="server" Text='<%$ Resources:Text, Via %>' Visible="false"></asp:Label>
                                                           
                                                                    <asp:Label ID="lblDataEntryBy" runat="server" Text='<%# Eval("DataEntry_By") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="90px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <FooterStyle BackColor="black" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr style="height: 40px">
                                                <td>
                                                    <asp:ImageButton ID="ibtnSearchedResultBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, BackBtn %>" />
                                                    <asp:ImageButton ID="ibtnConfirmSelection" ImageUrl='<%$ Resources:ImageUrl, ConfirmSelectedBtn %>'
                                                        AlternateText='<%$ Resources:AlternateText, ConfirmSelectedBtn %>' runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="vSelectedRecord" runat="server">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr style="height: 40px">
                                                <td>
                                                    <asp:Label ID="lblConfirmRecord" runat="server" CssClass="tableCaption" Text='<%$ Resources:Text, ConfirmRecord %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gvSelectedRecord" runat="server" AllowPaging="True" AllowSorting="true"
                                                        Width="1398px" BackColor="White" AutoGenerateColumns="false">
                                                        <Columns>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblClaimRecordIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle VerticalAlign="Middle" Width="25px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="SchemeCode_TransactionID" HeaderText="<%$ Resources:Text, TransactionNo %>">
                                                                <ItemStyle Wrap="false" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblScheme" runat="server" Text='<%# Eval("Scheme") %>'></asp:Label><br />
                                                                    <asp:Label ID="lblTransactionID" runat="server" Text='<%# Eval("Transaction_ID") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="150px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Transaction_Dtm" HeaderText="<%$ Resources:Text, TransactionTime %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransactionDtm" runat="server" Text='<%# Eval("Transaction_Dtm") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="114px" />
                                                            </asp:TemplateField>
                                                             <asp:TemplateField SortExpression="DocCode_IdentityNum" HeaderText="<%$ Resources:Text, DocTypeIDNL %>" >
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDocType" runat="server" Text='<%# Eval("Doc_Type") %>'></asp:Label>
                                                                   <asp:Label ID="lblIdentityNum" runat="server" Text='<%# Eval("IdentityNum") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="150px" />
                                                            </asp:TemplateField>                                                            
                                                           <%-- <asp:TemplateField SortExpression="Doc_Type" HeaderText="<%$ Resources:Text, DocumentType %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDocType" runat="server" Text='<%# Eval("Doc_Type") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="60px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="IdentityNum" HeaderText="<%$ Resources:Text, IdentityDocNo %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIdentityNum" runat="server" Text='<%# Eval("IdentityNum") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="90px" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField SortExpression="Eng_Name" HeaderText="<%$ Resources:Text, Name %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEngName" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblChiName" runat="server" Visible="false" Font-Names="HA_MingLiu"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="230px" />
                                                            </asp:TemplateField>
                                                            <%-- <asp:TemplateField SortExpression="Total_Unit" HeaderText="<%$ Resources:Text, NoOfUnitRedeem %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalUnit" runat="server" Text='<%# Eval("Total_Unit") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" Width="68px" />
                                                            </asp:TemplateField>--%>
                                                            <%-- <asp:TemplateField SortExpression="Total_Amount" HeaderText='<%$ Resources:Text, TotalAmountSign %>'>--%>
                                                            <asp:TemplateField SortExpression="Total_Amount_RMB" HeaderText="<%$ Resources:Text, TotalRedeemAmountSignRMB %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalAmountRMB" runat="server" Text='<%# Eval("Total_Amount_RMB") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" Width="85px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Total_Amount" HeaderText="<%$ Resources:Text, TotalRedeemAmountSign %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTotalAmount" runat="server" Text='<%# Eval("Total_Amount") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Right" Width="85px" />
                                                            </asp:TemplateField>
                                                            
                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, RecordStatus %>" SortExpression="Record_Status">
                                                                <ItemStyle Width="135px" />
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTranListTranStatus" runat="server" Text='<%# Bind("Record_Status") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hfTranListTranStatus" runat="server" Value='<%# Bind("Record_Status") %>' />
                                                                </ItemTemplate>
                                                            </asp:TemplateField>
                                                            
                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, OtherInformationNL %>"
                                                                Visible="True">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblOtherInformation" runat="server" Text="<%$ Resources:Text, Details %>"
                                                                        ForeColor="Blue"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="50px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Practice_Name" HeaderText="<%$ Resources:Text, PracticeName %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPracticeName" runat="server" Text='<%# Eval("Practice_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="180px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="DataEntry_By" HeaderText="<%$ Resources:Text, DataEntry %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVia" runat="server" Text='Via' ForeColor="Red" Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblDataEntryBy" runat="server" Text='<%# Eval("DataEntry_By") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="90px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr style="height: 80px">
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr>
                                                            <td class="checkboxStyle" valign="top" style="width: 900px">
                                                                <asp:CheckBox ID="chkConfirmTransaction" runat="server" Text='<%$ Resources:Text, ClaimConfirmStatement %>'
                                                                    AutoPostBack="True" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr style="height: 40px">
                                                <td>
                                                    <asp:ImageButton ID="ibtnSelectedRecordBack" runat="server" ImageUrl='<%$ Resources:ImageUrl, BackBtn %>'
                                                        AlternateText='<%$ Resources:AlternateText, BackBtn %>' />
                                                    <asp:ImageButton ID="ibtnConfirmTransaction" runat="server" ImageUrl='<%$ Resources:ImageUrl, ConfirmBtn %>'
                                                        AlternateText='<%$ Resources:AlternateText, ConfirmBtn %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="vConfirmedResult" runat="server">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr style="height: 28px">
                                                <td style="width: 215px">
                                                    <asp:Label ID="lblClaimConfirmationDateText" runat="server" CssClass="tableTitle"
                                                        Text='<%$ Resources:Text, ClaimConfirmationTime %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClaimConfirmationDate" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="height: 28px">
                                                <td>
                                                    <asp:Label ID="lblNoOfTransactionConfirmedText" runat="server" CssClass="tableTitle"
                                                        Text='<%$ Resources:Text, NoOfTransactionConfirmed %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblNoOfTransactionConfirmed" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="height: 40px">
                                                <td colspan="2">
                                                    <asp:ImageButton ID="ibtnConfirmedResultNextRetrieve" runat="server" ImageUrl='<%$ Resources:ImageUrl, ReturnBtn %>'
                                                        AlternateText='<%$ Resources:AlternateText, ReturnBtn %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="vTransactionDetail" runat="server">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr>
                                                <td colspan="3">
                                                    <asp:Label ID="lblClaimInfo" runat="server" CssClass="tableCaption" Text='<%$ Resources:Text, ClaimInfo %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="3">
                                                    <uc2:ClaimTranEnquiry ID="udcClaimTranEnquiry" runat="server">
                                                    </uc2:ClaimTranEnquiry></td>
                                            </tr>
                                            <asp:Panel ID="pnlRejectReason" runat="server" Visible="false">
                                                <tr>
                                                    <td style="width: 205px">
                                                        <asp:Label ID="lblRejectReasonText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, VoidReason %>" Width="100px"></asp:Label></td>
                                                    <td colspan="2" align="left" >
                                                        <asp:Label ID="lblRejectReason" runat="server" CssClass="TextChi" Visible="false"></asp:Label>
                                                        <asp:TextBox ID="txtRejectReason" runat="server" Width="464px" Visible="false" CssClass="textChi"></asp:TextBox>
                                                        <asp:Image ID="imgRejectReasonAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                            Visible="false" /></td></tr>  
                                            </asp:Panel>
                                            <tr style="height: 40px">
                                                <td colspan="3">
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr>
                                                            <td style="width: 185px">
                                                            </td>
                                                            <td align="left" style="width: 620px">
                                                                <asp:ImageButton ID="ibtnTransactionDetailBack" runat="server" ImageUrl='<%$ Resources:ImageUrl, BackBtn %>'
                                                                    AlternateText='<%$ Resources:AlternateText, BackBtn %>' />
                                                                <asp:ImageButton ID="ibtnRejectTransaction" runat="server" ImageUrl='<%$ Resources:ImageUrl, VoidBtn %>'
                                                                    AlternateText='<%$ Resources:AlternateText, VoidBtn %>' />
                                                                <asp:ImageButton ID="ibtnCancelVoidTransaction" runat="server" ImageUrl='<%$ Resources:ImageUrl, CancelBtn %>'
                                                                    AlternateText='<%$ Resources:AlternateText, CancelBtn %>' />
                                                                <asp:ImageButton ID="ibtnSaveRejectTransaction" runat="server" ImageUrl='<%$ Resources:ImageUrl, ConfirmVoidBtn %>'
                                                                    AlternateText='<%$ Resources:AlternateText, ConfirmVoidBtn %>' Visible="false" />
                                                                <asp:ImageButton ID="ibtnConfirmRejectTransaction" runat="server" ImageUrl='<%$ Resources:ImageUrl, ConfirmVoidBtn %>'
                                                                    AlternateText='<%$ Resources:AlternateText, ConfirmVoidBtn %>' Visible="false" />
                                                                <cc2:CustomImageButton ID="ibtnManagement" runat="server" ImageUrl="<%$ Resources:ImageUrl, HCSPManagementBtn %>"
                                                                    AlternateText="<%$ Resources:AlternateText, HCSPManagementBtn %>"  ShowRedirectImage="false"/>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="vRejectResult" runat="server">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr style="height: 28px">
                                                <td style="width: 185px">
                                                    <asp:Label ID="lblRejectDateText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, VoidTranTime %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblRejectDate" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="height: 28px">
                                                <td style="width: 185px">
                                                    <asp:Label ID="lblRejectReferenceNoText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, VoidTranID %>'></asp:Label></td>
                                                <td>
                                                    <asp:Label ID="lblRejectReferenceNo" runat="server" CssClass="tableText" ForeColor="Blue"></asp:Label></td>
                                            </tr>
                                            <tr style="height: 28px">
                                                <td colspan="2">
                                                    <asp:ImageButton ID="ibtnRejectResultNextRetrieve" runat="server" ImageUrl='<%$ Resources:ImageUrl, ReturnBtn %>'
                                                        AlternateText='<%$ Resources:AlternateText, ReturnBtn %>' /></td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                </asp:MultiView>
                            </asp:View>
                            <asp:View ID="vVRAcct" runat="server">
                                <asp:MultiView ID="mvVRAcct" runat="server">
                                    <asp:View ID="vSearchedResult_vr" runat="server">
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                                            <tr style="height: 40px">
                                                <td colspan="2">
                                                    <asp:Label ID="lblVRAcctRecord" runat="server" CssClass="tableCaption" Text='<%$ Resources:Text, VRARecord %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" border="0">
                                                        <tr style="height: 28px">
                                                            <td style="width: 200px">
                                                                <asp:Label ID="lblCutoffDateResultText_vr" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, CutoffDate %>'></asp:Label>
                                                            </td>
                                                            <td style="width: 250px">
                                                                <asp:Label ID="lblCutoffDateResult_vr" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="lblPracticeResultText_vr" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Practice %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblPracticeResult_vr" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr style="height: 28px">
                                                            <td style="width: 200px">
                                                                <asp:Label ID="lblDataEntryResultText_vr" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, DataEntry %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblDataEntryResult_vr" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                            <td style="width: 150px">
                                                                <asp:Label ID="lblSchemeResultText_vr" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, Scheme %>'></asp:Label>
                                                            </td>
                                                            <td>
                                                                <asp:Label ID="lblSchemeResult_vr" runat="server" CssClass="tableText"></asp:Label>
                                                            </td>
                                                       </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;</td>
                                            </tr>
                                            <tr>
                                                <td colspan="2">
                                                    <asp:Panel ID="pnlTempVRAcctRecord" runat="server">
                                                        <table cellpadding="0" cellspacing="0" border="0">
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="gvTempVRAcctRecord" runat="server" AllowPaging="True" AllowSorting="true"
                                                                        BackColor="White" AutoGenerateColumns="false" Width="1190px">
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTempVRAcctRecordIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                                                                <ItemStyle VerticalAlign="Middle" Width="20px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <ItemTemplate>
                                                                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="15px" />
                                                                                <HeaderTemplate>
                                                                                    <asp:CheckBox ID="chkSelectAll" onclick="javascript:SelectAllCheckboxesByGridId(this);" runat="server"/>
                                                                                </HeaderTemplate>
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField SortExpression="Scheme_Display_Code" HeaderText="<%$ Resources:Text, Scheme  %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblScheme" runat="server" Text='<%# Eval("Scheme_Display_Code") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="70px" />
                                                                            </asp:TemplateField>
                                                                            
                                                                            <asp:TemplateField SortExpression="doc_for_sort" HeaderText="<%$ Resources:Text, DocTypeIDNL %>" >
                                                                            <ItemTemplate>
                                                                                <asp:Label ID="lblDocType" runat="server" Text='<%# Eval("doc_display_code") %>'></asp:Label>
                                                                                <asp:HiddenField ID="hfDocType" runat="server" Value='<%# Eval("doc_display_code") %>' ></asp:HiddenField>
                                                                                   <asp:Label ID="lblIdentityNum" runat="server" Text='<%# Eval("IdentityNum") %>'></asp:Label>
                                                                                    <asp:LinkButton ID="lbtnHKID" runat="server" Visible="false"></asp:LinkButton>
                                                                            </ItemTemplate>
                                                                            <ItemStyle Width="150px" />
                                                                        </asp:TemplateField>
                                                            
                                                                          <%--  <asp:TemplateField SortExpression="doc_display_code" HeaderText="<%$ Resources:Text, DocumentType %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDocType" runat="server" Text='<%# Eval("doc_display_code") %>'></asp:Label>
                                                                                    <asp:HiddenField ID="hfDocType" runat="server" Value='<%# Eval("doc_display_code") %>'>
                                                                                    </asp:HiddenField>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="60px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField SortExpression="IdentityNum" HeaderText="<%$ Resources:Text, IdentityDocNo %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblIdentityNum" runat="server" Text='<%# Eval("IdentityNum") %>'></asp:Label>
                                                                                    <asp:LinkButton ID="lbtnHKID" runat="server" Visible="false"></asp:LinkButton>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="95px" />
                                                                            </asp:TemplateField>--%>
                                                                            <asp:TemplateField SortExpression="Date_of_Issue" HeaderText="<%$ Resources:Text, ECDate %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDateOfIssue" runat="server" Text='<%# Eval("Date_of_Issue") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="95px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField SortExpression="Eng_Name" HeaderText="<%$ Resources:Text, Name %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblEngName" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblChiName" runat="server" Visible="false" Font-Names="HA_MingLiu"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="230px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField SortExpression="DOB" HeaderText="<%$ Resources:Text, DOB %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblDOB" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="130px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField SortExpression="Sex" HeaderText="<%$ Resources:Text, Gender %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblSex" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="45px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, OtherInformation %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblECSerialNo" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblECRefNo" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblForeignPassportNo" runat="server"></asp:Label>
                                                                                    <asp:Label ID="lblPermitToRemain" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="240px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField SortExpression="Transaction_Dtm" HeaderText="<%$ Resources:Text, CreationTime %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblTransactionDtm" runat="server" Text='<%# Eval("Transaction_Dtm") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="135px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField SortExpression="Practice_Name" HeaderText="<%$ Resources:Text, PracticeName %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblPractice" runat="server" Text='<%# Eval("Practice_Name") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="150px" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField SortExpression="DataEntry_By" HeaderText="<%$ Resources:Text, DataEntry %>">
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblVia" runat="server" Text='Via' ForeColor="Red" Visible="false"></asp:Label>
                                                                                    <asp:Label ID="lblDataEntryBy" runat="server" Text='<%# Eval("DataEntry_By") %>'></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="100px" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                        <HeaderStyle HorizontalAlign="Center" />
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </asp:Panel>
                                                </td>
                                            </tr>
                                            <tr style="height: 40px">
                                                <td>
                                                    <asp:ImageButton ID="ibtnSearchedResultBack_vr" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, BackBtn %>" />
                                                    <asp:ImageButton ID="ibtnConfirmSelected" ImageUrl="<%$ Resources:ImageUrl, ConfirmSelectedBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, ConfirmSelectedBtn %>" runat="server" />
                                                    <asp:ImageButton ID="ibtnRejectSelected" ImageUrl="<%$ Resources:ImageUrl, RejectSelectedBtn %>"
                                                        AlternateText="<%$ Resources:AlternateText, RejectSelectedBtn %>" runat="server" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="vSelectedRecord_vr" runat="server">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr style="height: 41px">
                                                <td>
                                                    <asp:Label ID="lblConfirmRecord_vr" runat="server" CssClass="tableCaption" Text='<%$ Resources:Text, ConfirmRecord %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:GridView ID="gvSelectedRecord_vr" runat="server" AllowPaging="True" AllowSorting="true"
                                                        Width="1100px" BackColor="White" AutoGenerateColumns="false" OnSelectedIndexChanged="gvSelectedRecord_vr_SelectedIndexChanged">
                                                        <Columns>
                                                            <asp:TemplateField SortExpression="Scheme_Display_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblScheme" runat="server" Text='<%# Eval("Scheme_Display_Code") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="70px" />
                                                            </asp:TemplateField>
                                                               <asp:TemplateField SortExpression="doc_for_sort" HeaderText="<%$ Resources:Text, DocTypeIDNL %>" >
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDocType" runat="server" Text='<%# Eval("doc_display_code") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hfDocType" runat="server" Value='<%# Eval("doc_display_code") %>' ></asp:HiddenField>
                                                                    <asp:Label ID="lblIdentityNum" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="150px" />
                                                            </asp:TemplateField>
                                                            
                                                          <%--  <asp:TemplateField SortExpression="doc_display_code" HeaderText="<%$ Resources:Text, DocumentType %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDocType" runat="server" Text='<%# Eval("doc_display_code") %>'></asp:Label>
                                                                    <asp:HiddenField ID="hfDocType" runat="server" Value='<%# Eval("doc_display_code") %>'>
                                                                    </asp:HiddenField>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="60px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="IdentityNum" HeaderText="<%$ Resources:Text, IdentityDocNo %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblIdentityNum" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="95px" />
                                                            </asp:TemplateField>--%>
                                                            <asp:TemplateField SortExpression="Date_of_Issue" HeaderText="<%$ Resources:Text, ECDate %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDateOfIssue" runat="server" Text='<%# Eval("Date_of_Issue") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="95px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Eng_Name" HeaderText="<%$ Resources:Text, Name %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblEngName" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblChiName" runat="server" Visible="false" Font-Names="HA_MingLiu"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="150px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="DOB" HeaderText="<%$ Resources:Text, DOB %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblDOB" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="130px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Sex" HeaderText="<%$ Resources:Text, Gender %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblSex" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="55px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="<%$ Resources:Text, OtherInformation %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblECSerialNo" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblECRefNo" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblForeignPassportNo" runat="server"></asp:Label>
                                                                    <asp:Label ID="lblPermitToRemain" runat="server"></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="240px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Transaction_Dtm" HeaderText="<%$ Resources:Text, CreationTime %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblTransactionDtm" runat="server" Text='<%# Eval("Transaction_Dtm") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="145px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="Practice_Name" HeaderText="<%$ Resources:Text, PracticeName %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblPractice" runat="server" Text='<%# Eval("Practice_Name") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="150px" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField SortExpression="DataEntry_By" HeaderText="<%$ Resources:Text, DataEntry %>">
                                                                <ItemTemplate>
                                                                    <asp:Label ID="lblVia" runat="server" Text='Via' ForeColor="Red" Visible="false"></asp:Label>
                                                                    <asp:Label ID="lblDataEntryBy" runat="server" Text='<%# Eval("DataEntry_By") %>'></asp:Label>
                                                                </ItemTemplate>
                                                                <ItemStyle Width="100px" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr style="height: 40px">
                                                <td>
                                                    <asp:ImageButton ID="ibtnSelectedRecordBack_vr" runat="server" ImageUrl='<%$ Resources:ImageUrl, BackBtn %>'
                                                        AlternateText='<%$ Resources:AlternateText, BackBtn %>' />
                                                    <asp:ImageButton ID="ibtnConfirmTempVRAcct" runat="server" ImageUrl='<%$ Resources:ImageUrl, ConfirmBtn %>'
                                                        AlternateText='<%$ Resources:AlternateText, ConfirmBtn %>' />
                                                    <asp:ImageButton ID="ibtnRejectTempVRAcct" runat="server" ImageUrl='<%$ Resources:ImageUrl, RejectBtn %>'
                                                        AlternateText='<%$ Resources:AlternateText, RejectBtn %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="vConfirmedResult_vr" runat="server">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr style="height: 28px">
                                                <td style="width: 365px">
                                                    <asp:Label ID="lblVRAcctConfirmationDateText" runat="server" CssClass="tableTitle"
                                                        Text='<%$ Resources:Text, VRAConfirmationTime %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblVRAcctConfirmationDate" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="height: 28px">
                                                <td>
                                                    <asp:Label ID="lblNoOfTempAcctConfirmedText" runat="server" CssClass="tableTitle"
                                                        Text='<%$ Resources:Text, NoOfVRAConfirmed %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblNoOfVRAcctConfirmed" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="height: 40px">
                                                <td colspan="2">
                                                    <asp:ImageButton ID="ibtnConfirmedResultBackToRecordList" runat="server" ImageUrl='<%$ Resources:ImageUrl, ReturnBtn %>'
                                                        AlternateText='<%$ Resources:AlternateText, ReturnBtn %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="vVRAcctInfo" runat="server">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr style="height: 41px">
                                                <td colspan="2">
                                                    <asp:Label ID="lblVRAcctInfo" runat="server" CssClass="tableCaption" Text='<%$ Resources:Text, VRAInfo %>'></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblVRAcctIDTemp" runat="server" CssClass="tableText"></asp:Label>
                                                    <br />
                                                    <asp:Label ID="lblSchemeCodeTemp" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="height: 40px">
                                                <td>
                                                    <asp:ImageButton ID="ibtnVRAcctInfoBack" runat="server" ImageUrl='<%$ Resources:ImageUrl, BackBtn %>'
                                                        AlternateText='<%$ Resources:AlternateText, BackBtn %>' />
                                                    <asp:ImageButton ID="ibtnConfirmRejectTempVRAcct" runat="server" ImageUrl='<%$ Resources:ImageUrl, ConfirmRejectBtn %>'
                                                        AlternateText='<%$ Resources:AlternateText, ConfirmRejectBtn %>' Visible="false" />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="vRejectResult_vr" runat="server">
                                        <table cellpadding="0" cellspacing="0" border="0">
                                            <tr style="height: 28px">
                                                <td style="width: 365px">
                                                    <asp:Label ID="lblRejectDateText_vr" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, VRARejectionTime %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblRejectDate_vr" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="height: 28px">
                                                <td>
                                                    <asp:Label ID="lblNoOfVRAcctRejectedText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, NoOfVRARejected %>'></asp:Label>
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblNoOfVRAcctRejected" runat="server" CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr style="height: 40px">
                                                <td colspan="2">
                                                    <asp:ImageButton ID="ibtnRejectResultBackToRecordList" runat="server" ImageUrl='<%$ Resources:ImageUrl, ReturnBtn %>'
                                                        AlternateText='<%$ Resources:AlternateText, ReturnBtn %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                </asp:MultiView>
                            </asp:View>
                        </asp:MultiView>
                        <%-- Popup for Scheme Name Help --%>
                        <asp:Panel ID="panSchemeNameHelp" runat="server" Style="display: none;">
                            <asp:Panel ID="panSchemeNameHelpHeading" runat="server" Style="cursor: move;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                                    <tr>
                                        <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                        </td>
                                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                                            <asp:Label ID="lblSchemeNameHelpHeading" runat="server" Text="<%$ Resources:Text, Legend %>"></asp:Label></td>
                                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                    <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                                        <asp:Panel ID="panSchemeNameHelpContent" runat="server" ScrollBars="vertical" Height="330px">
                                            <uc1:SchemeLegend ID="udcSchemeLegend" runat="server" />
                                        </asp:Panel>
                                    </td>
                                    <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                    <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                                        <asp:ImageButton ID="ibtnCloseSchemeNameHelp" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseSchemeNameHelp_Click" /></td>
                                    <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
                                    </td>
                                    <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                                        height: 7px">
                                    </td>
                                    <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                                        height: 7px">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button runat="server" ID="btnHiddenSchemeNameHelp" Style="display: none" />
                        <cc1:ModalPopupExtender ID="popupSchemeNameHelp" runat="server" TargetControlID="btnHiddenSchemeNameHelp"
                            PopupControlID="panSchemeNameHelp" BackgroundCssClass="modalBackgroundTransparent"
                            DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panSchemeNameHelpHeading">
                        </cc1:ModalPopupExtender>
                        <%-- End of Popup for Scheme Name Help --%>
                        <%-- Popup for DocType Help --%>
                        <asp:Panel ID="panDocTypeHelp" runat="server" Style="display: none;">
                            <asp:Panel ID="panDocTypeHelpHeading" runat="server" Style="cursor: move;">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                                    <tr>
                                        <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                        </td>
                                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                                            <asp:Label ID="lblDocTypeHelpHeading" runat="server" Text="<%$ Resources:Text, Legend %>"></asp:Label></td>
                                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                    <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                                        <asp:Panel ID="panDocTypeContent" runat="server" ScrollBars="vertical" Height="290px">
                                            <uc3:DocTypeLegend ID="udcDocTypeLegend" runat="server" />
                                        </asp:Panel>
                                    </td>
                                    <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                    <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                                        <asp:ImageButton ID="ibtnCloseDocTypeHelp" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnCloseDocTypeHelp_Click" /></td>
                                    <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                                    </td>
                                </tr>
                                <tr>
                                    <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px">
                                    </td>
                                    <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                                        height: 7px">
                                    </td>
                                    <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                                        height: 7px">
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Button runat="server" ID="btnHiddenDocTypeHelp" Style="display: none" />
                        <cc1:ModalPopupExtender ID="popupDocTypeHelp" runat="server" TargetControlID="btnHiddenDocTypeHelp"
                            PopupControlID="panDocTypeHelp" BackgroundCssClass="modalBackgroundTransparent"
                            DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panDocTypeHelpHeading">
                        </cc1:ModalPopupExtender>
                        <%-- End of Popup for DocType --%>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
