<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="spDataEntry.aspx.vb" Inherits="HCVU.spDataEntry" Title="<%$ Resources:Title, SPDataEnty %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="spSummaryView.ascx" TagName="spSummaryView" TagPrefix="uc1" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="uc2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>

    <script type="text/javascript" language="javascript">

    function getTableLocation(ERN, table)
    {
        document.getElementById('<%=hfERN.ClientID%>').value = ERN;
        document.getElementById('<%=hfTableLocation.ClientID%>').value = table;
        document.getElementById('<%=btnSpDetails.ClientID %>').click();
        return false;    
    }
    </script>

    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, SPDataEntryBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, SPDataEntryBanner %>" /><asp:UpdatePanel ID="UpdatePanel1"
            runat="server">
            <ContentTemplate>
                <cc1:InfoMessageBox ID="CompleteMsgBox" runat="server" Width="90%" />
                <cc1:MessageBox ID="msgBox" runat="server" Width="90%" />
                <asp:Panel ID="pnlDataEntry" runat="server">
                    <asp:MultiView ID="MultiViewDataEntry" runat="server" ActiveViewIndex="0">
                        <asp:View ID="ViewSearchCriteria" runat="server">
                            <table>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblEnrolRefNoText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                    <td style="width: 250px">
                                        <asp:TextBox ID="txtEnrolRefNo" runat="server" MaxLength="17" onBlur="Upper(event,this)"></asp:TextBox></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtSPID" runat="server" MaxLength="8"></asp:TextBox></td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                    <td style="width: 250px">
                                        <asp:TextBox ID="txtSPHKID" runat="server" MaxLength="11" onBlur="formatHKID(this)"></asp:TextBox></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                    <td>
                                        <asp:TextBox ID="txtSPName" runat="server" MaxLength="40" onBlur="Upper(event,this)"
                                            ToolTip="<%$ Resources:ToolTip, EnglishNameHint %>"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblSPPhone" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                    <td style="width: 250px">
                                        <asp:TextBox ID="txtPhone" runat="server" MaxLength="20"></asp:TextBox></td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblSPHealthProf" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlSPHealthProf" runat="server" AppendDataBoundItems="True">
                                            <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblStatus" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                    <td style="width: 250px">
                                        <asp:DropDownList ID="ddlStatus" Width="155px" runat="server" AppendDataBoundItems="True">
                                            <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="width: 200px">
                                        <asp:Label ID="lblScheme" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlScheme" Width="155px" runat="server" AppendDataBoundItems="True">
                                            <asp:ListItem Text="<%$ Resources:Text, Any %>" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                <tr>
                                    <td align="center" style="padding-top: 15px">
                                        <asp:ImageButton ID="ibtnSearch" runat="server" AlternateText="<%$ Resources:AlternateText, SearchBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" OnClick="ibtnSearch_Click" />
                                    </td>
                                </tr>
                            </table>
                            <cc2:FilteredTextBoxExtender ID="FilteredERN" runat="server" TargetControlID="txtEnrolRefNo"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="-">
                            </cc2:FilteredTextBoxExtender>
                            <cc2:FilteredTextBoxExtender ID="FilteredSPID" runat="server" TargetControlID="txtSPID"
                                FilterType="Custom, Numbers">
                            </cc2:FilteredTextBoxExtender>
                            <cc2:FilteredTextBoxExtender ID="FilteredSPHKID" runat="server" TargetControlID="txtSPHKID"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars="()">
                            </cc2:FilteredTextBoxExtender>
                            <cc2:FilteredTextBoxExtender ID="FilteredSPName" runat="server" TargetControlID="txtSPName"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters " ValidChars="'.-, ">
                            </cc2:FilteredTextBoxExtender>
                        </asp:View>
                        <asp:View ID="ViewSearchResult" runat="server">
                            <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                                Height="0px" Width="0px" OnClientClick="return false;" />
                                
                            <uc2:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="panSearchCriteriaReview" />
                                
                            <asp:Panel ID="panSearchCriteriaReview" runat="server">
                                <table>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                        <td style="width: 250px">
                                            <asp:Label ID="lblResultERN" runat="server" CssClass="tableText"></asp:Label></td>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblResultSPID" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                        <td style="width: 250px">
                                            <asp:Label ID="lblResultSPHKID" runat="server" CssClass="tableText"></asp:Label></td>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblResultSPName" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultPhoneText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                        <td style="width: 250px">
                                            <asp:Label ID="lblResultPhone" runat="server" CssClass="tableText"></asp:Label></td>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblResultHealthProf" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultStatusText" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                        <td style="width: 250px">
                                            <asp:Label ID="lblResultStatus" runat="server" CssClass="tableText"></asp:Label></td>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblResultSchemeText" runat="server" Text="<%$ Resources:Text, Scheme %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblResultScheme" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="panSearchCriteriaProcessing" runat="server">
                                <asp:Label ID="lblSameHKICPText" runat="server" Text="<%$ Resources:Text, DataEntryProcessingSPStatement %>"></asp:Label>
                                <br />
                                <br />
                                <table>
                                    <tr>
                                        <td style="width: 10px">
                                        </td>
                                        <td style="width: 180px">
                                            <asp:Label ID="lblSameHKICPERNSPIDText" runat="server" Text="Enrolment Reference No. / Service Provider ID (to be controlled code-behind)"></asp:Label></td>
                                        <td>
                                            <asp:LinkButton ID="lnkSameHKICPERNSPID" runat="server" CssClass="tableText" OnClick="lnkSameHKIC_Click"
                                                ForeColor="Blue"></asp:LinkButton>
                                            <asp:HiddenField ID="hfSameHKICPSPID" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSameHKICPSPHKICText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSameHKICPSPHKIC" runat="server" CssClass="tableText"></asp:Label>
                                            <asp:HiddenField ID="hfSameHKICPSPHKIC" runat="server" />
                                            </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSameHKICPSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSameHKICPSPName" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSameHKICPStatusText" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSameHKICPStatus" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <asp:Label ID="lblSameHKICPRecordText" runat="server" Text="<%$ Resources:Text, DataEntryUnprocessedSPStatement %>"></asp:Label>
                            </asp:Panel>
                            <asp:Panel ID="panSearchCriteriaEnrolled" runat="server">
                                <asp:Label ID="lblSameHKICEText" runat="server" Text="<%$ Resources:Text, DataEntryEnrolledSPStatement %>"></asp:Label>
                                <br />
                                <br />
                                <table>
                                    <tr>
                                        <td style="width: 10px">
                                        </td>
                                        <td style="width: 180px">
                                            <asp:Label ID="lblSameHKICESPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                        <td>
                                            <asp:LinkButton ID="lnkSameHKICESPID" runat="server" CssClass="tableText" OnClick="lnkSameHKIC_Click"
                                                ForeColor="Blue"></asp:LinkButton>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSameHKICESPHKICText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSameHKICESPHKIC" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSameHKICESPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSameHKICESPName" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Label ID="lblSameHKICEStatusText" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblSameHKICEStatus" runat="server" CssClass="tableText"></asp:Label></td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <asp:Label ID="lblSameHKICERecordText" runat="server" Text="<%$ Resources:Text, DataEntryUnprocessedSPStatement %>"></asp:Label>
                            </asp:Panel>

                            <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                                AllowSorting="true" Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            <asp:HiddenField ID="hfRStatus" runat="server" Value='<%# Eval("Table_Location") %>' />
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cboMerge" runat="server" OnCheckedChanged="gvResult_UncheckOther"
                                                AutoPostBack="True"></asp:CheckBox></ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Enrolment_Ref_No" HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnERN" runat="server" CommandArgument='<%# Eval("Enrolment_Ref_No") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SP_ID" HeaderText="<%$ Resources:Text, ServiceProviderID %>">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnRSPID" runat="server" Text='<%# Eval("SP_ID") %> ' CommandArgument='<%# Eval("Enrolment_Ref_No") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="70px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Enrolment_Dtm" HeaderText="<%$ Resources:Text, EnrolmentTime %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblREnrolDtm" runat="server" Text='<%# Eval("Enrolment_Dtm") %> '></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="90px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Processing_Dtm" HeaderText="<%$ Resources:Text, DataEntryProcessingTime %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRProcessingDtm" runat="server" Text='<%# Eval("Processing_Dtm") %> '></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="90px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SP_HKID" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRSPHKID" runat="server" Text='<%# Eval("SP_HKID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="90px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="SP_Eng_Name" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblREname" runat="server" Text='<%# Eval("SP_Eng_Name") %>'></asp:Label><br />
                                            <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("SP_Chi_Name") %>' CssClass="TextGridChi"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="270px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Phone_Daytime" HeaderText="<%$ Resources:Text, ContactNo %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRPhone" runat="server" Text='<%# Eval("Phone_Daytime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Table_Location" HeaderText="<%$ Resources:Text, Status %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRStatus" runat="server" Text='<%# Eval("Table_Location") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="80px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField SortExpression="Scheme_Code" HeaderText="<%$ Resources:Text, Scheme %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRScheme" runat="server" Text='<%# Eval("Scheme_Code") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <asp:Panel ID="panHKIDNoRecord" runat="server">
                                <asp:Label ID="lblHKIDSearchNoRecord" runat="server" Text="<%$ Resources:Text, HKIDSearchNoRecordFound %>"></asp:Label>
                                <br />
                                <asp:Label ID="lblHKIDSearchNoRecord2" runat="server" Text="<%$ Resources:Text, HKIDSearchNoRecordFound2 %>"></asp:Label>
                                <br />
                                <br />
                            </asp:Panel>
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 5%">
                                        <asp:ImageButton ID="ibtnSearchResultBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSearchResultBack_Click" />
                                    </td>
                                    <td align="center" style="width: 95%">
                                        <asp:ImageButton ID="ibtnNewEnrolment" runat="server" AlternateText="<%$ Resources:AlternateText, NewEnrolBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, NewEnrolBtn %>" OnClick="ibtnNewEnrolment_Click" />
                                        <asp:ImageButton ID="ibtnProceed" runat="server" AlternateText="<%$ Resources:AlternateText, ProceedBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ProceedBtn %>" OnClick="ibtnProceed_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="ViewBatchResult" runat="server">
                            <div class="headingText">
                                <asp:Label ID="lblBatchResult" runat="server" Text="<%$ Resources:Text, SearchResults %>"></asp:Label>
                            </div>
                            <asp:Panel ID="panEnrolledSP" runat="server">
                                <asp:Label ID="lblEnrolledSPStatement" runat="server" Text="<%$ Resources:Text, DataEntryEnrolledSPStatement %>"
                                    Font-Bold="true"></asp:Label>
                                <br>
                                <br />
                                <table cellpadding="2" cellspacing="2">
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblEnrolledSPIDText" runat="server" Text="<%$ Resources:Text, SPID %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblEnrolledSPID" runat="server" CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblEnrolledNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblEnrolledName" runat="server" CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px">
                                            <asp:Label ID="lblEnrolledStatusText" runat="server" Text="<%$ Resources:Text, Status %>"></asp:Label></td>
                                        <td>
                                            <asp:Label ID="lblEnrolledStatus" runat="server" Text="Enrolled" CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                            </asp:Panel>
                            <asp:Label ID="lblUnproceedStatement" runat="server" Text="<%$ Resources:Text, DataEntryUnprocessedSPStatement %>"
                                Font-Bold="true"></asp:Label>
                            <br />
                            <br />
                            <asp:GridView ID="gvBatchResult" runat="server" AutoGenerateColumns="False" AllowPaging="false"
                                AllowSorting="false" Width="100%">
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblResultBatchIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="10px" />
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox ID="chkSelect" runat="server" />
                                        </ItemTemplate>
                                        <ItemStyle Width="15px" VerticalAlign="Top" />
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chkSelectAll" onclick="javascript:SelectAllCheckboxes(this);" runat="server" />
                                        </HeaderTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lnkbtnResultBatchERN" runat="server" CommandArgument='<%# Eval("Enrolment_Ref_No") %>'></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, EnrolmentTime %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblResultBatchEnrolDtm" runat="server" Text='<%# Eval("Enrolment_Dtm") %> '></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblResultBatchSPHKID" runat="server" Text='<%# Eval("SP_HKID") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblResultBatchEname" runat="server" Text='<%# Eval("SP_Eng_Name") %>'></asp:Label><br />
                                            <asp:Label ID="lblResultBatchCname" runat="server" Text='<%# Eval("SP_Chi_Name") %>' CssClass="TextGridChi"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="270px"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, ContactNo %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblResultBatchPhone" runat="server" Text='<%# Eval("Phone_Daytime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, Scheme %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblResultBatchScheme" runat="server" Text='<%# Eval("Scheme_Code") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                            <br />
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 5%">
                                        <asp:ImageButton ID="ibtnSearchResultBatchBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSearchResultBatchBack_Click">
                                        </asp:ImageButton>
                                    </td>
                                    <td align="center" style="width: 95%">
                                        <asp:ImageButton ID="ibtnSearchBatchProceed" runat="server" AlternateText="<%$ Resources:AlternateText, ProceedBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, ProceedBtn %>" OnClick="ibtnSearchBatchProceed_Click">
                                        </asp:ImageButton>
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="ViewMigrateHCVS" runat="server">
                            <br />
                            <asp:Label ID="lblMigrateHCVSHeading" runat="server" Text="<%$ Resources:Text, MigrateHCVSHeading %>"></asp:Label>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td style="width: 10px">
                                    </td>
                                    <td style="width: 180px">
                                        <asp:Label ID="lblMigrateHCVSERNSPIDText" runat="server" Text="Enrolment Reference No. / Service Provider ID (to be controlled code-behind)"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMigrateHCVSERNSPID" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMigrateHCVSSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMigrateHCVSSPName" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:Label ID="lblMigrateHCVSSPNameChi" runat="server" CssClass="TextChi"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMigrateHCVSHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMigrateHCVSHKID" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <asp:Label ID="lblMigrateHCVSPreview" runat="server" Text="<%$ Resources:Text, MigrateHCVSPreview %>"></asp:Label>
                            <br />
                            <asp:Label ID="lblMigrateHCVSMigrate" runat="server" Text="<%$ Resources:Text, MigrateHCVSMigrate %>"></asp:Label>
                            <br />
                            <br />
                            <br />
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 100px">
                                        <asp:ImageButton ID="ibtnMigrateHCVSBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSearchResultBack_Click">
                                        </asp:ImageButton>
                                    </td>
                                    <td align="center">
                                        <asp:ImageButton ID="ibtnMigrateHCVSPreview" runat="server" AlternateText="Preview"
                                            ImageUrl="~/Images/button/btn_preview.png" OnClick="ibtnMigrateHCVSPreview_Click">
                                        </asp:ImageButton>
                                        <asp:ImageButton ID="ibtnMigrateHCVSMigrate" runat="server" AlternateText="<%$ Resources:AlternateText, MigrateBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, MigrateBtn %>" OnClick="ibtnMigrateHCVSMigrate_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="ViewMigrateIVSS" runat="server">
                            <br />
                            <asp:Label ID="lblMigrateIVSSHeading1" runat="server" Text="<%$ Resources:Text, MigrateIVSSHeading1 %>"></asp:Label>
                            <asp:Label ID="lblMigrateIVSSHeading2" runat="server" Text="<%$ Resources:Text, MigrateIVSSHeading2 %>"
                                BackColor="Yellow" Font-Bold="True"></asp:Label>
                            <asp:Label ID="lblMigrateIVSSHeading3" runat="server" Text="<%$ Resources:Text, MigrateIVSSHeading3 %>"></asp:Label>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td style="width: 10px">
                                    </td>
                                    <td style="width: 180px">
                                        <asp:Label ID="lblMigrateIVSSERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMigrateIVSSERN" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMigrateIVSSSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMigrateIVSSSPName" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:Label ID="lblMigrateIVSSSPNameChi" runat="server" CssClass="TextChi"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMigrateIVSSHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMigrateIVSSHKID" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <asp:Label ID="lblMigrateIVSSPreview" runat="server" Text="<%$ Resources:Text, MigrateIVSSPreview %>"></asp:Label>
                            <br />
                            <asp:Label ID="lblMigrateIVSSMigrate" runat="server" Text="<%$ Resources:Text, MigrateIVSSMigrate %>"></asp:Label>
                            <br />
                            <asp:Label ID="lblMigrateIVSSSkip" runat="server" Text="<%$ Resources:Text, MigrateIVSSSkip %>"></asp:Label>
                            <br />
                            <br />
                            <br />
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 100px">
                                        <asp:ImageButton ID="ibtnMigrateIVSSBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSearchResultBack_Click">
                                        </asp:ImageButton>
                                    </td>
                                    <td align="center">
                                        <asp:ImageButton ID="ibtnMigrateIVSSPreview" runat="server" AlternateText="<%$ Resources:AlternateText, PreviewBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, PreviewBtn %>" OnClick="ibtnMigrateIVSSPreview_Click" />
                                        <asp:ImageButton ID="ibtnMigrateIVSSMigrate" runat="server" AlternateText="<%$ Resources:AlternateText, MigrateBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, MigrateBtn %>" OnClick="ibtnMigrateIVSSMigrate_Click" />
                                        <asp:ImageButton ID="ibtnMigrateIVSSSkip" runat="server" AlternateText="<%$ Resources:AlternateText, SkipBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, SkipBtn %>" OnClick="ibtnMigrateIVSSSkip_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="ViewMigrateNeed" runat="server">
                            <br />
                            <asp:Label ID="lblMigrateNeedHeading" runat="server" Text="<%$ Resources:Text, MigrateNeedHeading %>"></asp:Label>
                            <br />
                            <br />
                            <table>
                                <tr>
                                    <td style="width: 10px">
                                    </td>
                                    <td style="width: 180px">
                                        <asp:Label ID="lblMigrateNeedERNSPIDText" runat="server" Text="Enrolment Reference No. / Service Provider ID (to be controlled code-behind)"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMigrateNeedERNSPID" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMigrateNeedSPNameText" runat="server" Text="<%$ Resources:Text, ServiceProviderName %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMigrateNeedSPName" runat="server" CssClass="tableText"></asp:Label>
                                        <asp:Label ID="lblMigrateNeedSPNameChi" runat="server" CssClass="TextChi"></asp:Label></td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblMigrateNeedSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                    <td>
                                        <asp:Label ID="lblMigrateNeedSPHKID" runat="server" CssClass="tableText"></asp:Label></td>
                                </tr>
                            </table>
                            <br />
                            <br />
                            <asp:Label ID="lblMigrateNeedDataMigration" runat="server" Text="<%$ Resources:Text, MigrateNeedDataMigration %>"></asp:Label>
                            <asp:Label ID="lblMigrateNeedDataMigrationNoRight" runat="server" Text="<%$ Resources:Text, MigrateNeedDataMigrationNoRight %>"></asp:Label>
                            <br />
                            <br />
                            <br />
                            <table style="width: 100%">
                                <tr>
                                    <td align="left" style="width: 100px">
                                        <asp:ImageButton ID="ibtnMigrateNeedBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSearchResultBack_Click">
                                        </asp:ImageButton>
                                    </td>
                                    <td align="center">
                                        <asp:ImageButton ID="ibtnMigrateNeedDataMigration" runat="server" AlternateText="<%$ Resources:AlternateText, DataMigrationBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, DataMigrationBtn %>" OnClick="ibtnMigrateNeedDataMigration_Click" />
                                    </td>
                                </tr>
                            </table>
                        </asp:View>
                        <asp:View ID="ViewError" runat="server">
                            &nbsp;<asp:ImageButton ID="ibtnErrorBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnErrorBack_Click" /></asp:View>
                    </asp:MultiView>
                </asp:Panel>
                <asp:HiddenField ID="hfAction" runat="server" />
                <asp:HiddenField ID="hfERN" runat="server" />
                <asp:HiddenField ID="hfTableLocation" runat="server" />
                <asp:HiddenField ID="hfProceed" runat="server" />
                <asp:TextBox ID="txtAction" runat="server" Style="display: none"></asp:TextBox>
                <asp:TextBox ID="txtTSPID" runat="server" Style="display: none"></asp:TextBox>
                <asp:TextBox ID="txtERN" runat="server" Style="display: none"></asp:TextBox>
                <asp:TextBox ID="txtSuffix" runat="server" Style="display: none"></asp:TextBox>
                <asp:TextBox ID="txtTableLocation" runat="server" Style="display: none"></asp:TextBox>
                <asp:Button ID="btnSpDetails" runat="server" Text="" Style="display: none" PostBackUrl="~/ServiceProvider/spProfile.aspx"
                    OnClick="btnSpDetails_Click" />
                <%-- Popup for Preview Migrate HCVS --%>
                <asp:Panel ID="panMigrateHCVSPreview" runat="server" Style="display: none;">
                    <asp:Panel ID="panMigrateHCVSPreviewHeading" runat="server" Style="cursor: move;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 680px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                </td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="lblMigratePreview" runat="server" Text="<%$ Resources: Text, Preview %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 680px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                            </td>
                            <td style="background-color: #ffffff" align="left">
                                <asp:Panel ID="panMigrateHCVSPreviewContent" ScrollBars="Vertical" Height="500px"
                                    runat="server" Width="97%">
                                    <table style="width: 90%">
                                        <tr>
                                            <td valign="top" colspan="2">
                                                <div class="headingText">
                                                    <asp:Label ID="lblPersonalParticulars" runat="server" Text="<%$ Resources:Text, PersonalParticulars %>"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px" valign="top">
                                                <asp:Label ID="lblMigrateHCVSViewSPIDERNText" runat="server" Text="Enrolment Reference No. / Service Provider ID (to be controlled code-behind)"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="lblMigrateHCVSViewSPIDERN" runat="server" CssClass="tableText"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px" valign="top">
                                                <asp:Label ID="lblMigrateHCVSViewSPNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblMigrateHCVSViewSPName" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:Label ID="lblMigrateHCVSViewSPNameChi" runat="server" CssClass="TextChi"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px" valign="top">
                                                <asp:Label ID="lblMigrateHCVSViewHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="lblMigrateHCVSViewHKID" runat="server" CssClass="tableText"></asp:Label></td>
                                        </tr>
                                    </table>
                                    <br />
                                    <div class="headingText">
                                        <asp:Label ID="lblMigrateHCVSMOHeading" runat="server" Text="<%$ Resources:Text, MedicalOrganizationInfo %>"></asp:Label>
                                    </div>
                                    <asp:GridView ID="gvMigrateHCVSMO" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                        Width="100%" OnRowDataBound="gvMigrateHCVSMO_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMOIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblRegBankMOENameText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationName %>"></asp:Label></td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMOEName" runat="server" Text='<%# Bind("MOEngName") %>' CssClass="tableText"></asp:Label><br />
                                                                <asp:Label ID="lblMOCName" runat="server" Text='<%# formatChineseString(Eval("MOChiName")) %>'
                                                                    CssClass="TextChi"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMOBRCode" runat="server" Text='<%# Bind("BrCode") %>' CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMOContactNoText" runat="server" Text="<%$ Resources:Text, MOContactNo %>"></asp:Label>
                                                            </td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMOContactNo" runat="server" Text='<%# Bind("PhoneDaytime") %>'
                                                                    CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label>
                                                            </td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMOEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label>
                                                            </td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMOFax" runat="server" Text='<%# Bind("Fax") %>' CssClass="tableText">
                                                                </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMOAddressText" runat="server" Text="<%$ Resources:Text, MOAddress %>"></asp:Label>
                                                            </td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMOAddress" runat="server" Text='<%# formatAddress(Eval("MOAddress")) %>'
                                                                    CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMORelationText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"></asp:Label></td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMORelation" runat="server" Text='<%# GetPracticeTypeName(Eval("Relationship")) %>'
                                                                    CssClass="tableText"></asp:Label>
                                                                <asp:Label ID="lblMORelationRemark" runat="server" Text='<%# formatChineseString(Eval("RelationshipRemark")) %>'
                                                                    CssClass="TextChi"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <div class="headingText">
                                        <asp:Label ID="lblMigrateHCVSPracticeBankInfo" runat="server" Text="<%$ Resources:Text, PracticeInfo %>"></asp:Label>
                                    </div>
                                    <asp:GridView ID="gvMigrateHCVSPracticeBank" runat="server" AutoGenerateColumns="False"
                                        ShowHeader="False" Width="100%" OnRowDataBound="gvMigrateHCVSPracticeBank_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPracticeBankIndex" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label></ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle VerticalAlign="Top" />
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblPracticeTitle" runat="server" Text="<%$ Resources:Text, Practice %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="padding-left: 15px">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                            <asp:Label ID="lblPracticeMOText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblPracticeMO" runat="server" Text='<%# Eval("MODisplaySeq") %>' CssClass="tableText"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                            <asp:Label ID="lblPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                                CssClass="tableText"></asp:Label><br />
                                                                            <asp:Label ID="lblPracticeNameChi" runat="server" Text='<%# formatChineseString(Eval("PracticeNameChi")) %>'
                                                                                CssClass="TextChi"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                            <asp:Label ID="lblPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblPracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress")) %>'
                                                                                CssClass="tableText"></asp:Label><br />
                                                                            <asp:Label ID="lblPracticeAddressChi" runat="server" Text='<%# formatChineseString(formatChiAddress(Eval("PracticeAddress"))) %>'
                                                                                CssClass="tableText"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                            <asp:Label ID="lblPracticePhoneText" runat="server" Text="<%$ Resources:Text, PracticeTel %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblPracticePhone" runat="server" CssClass="tableText" Text='<%# Eval("PhoneDaytime") %>'></asp:Label></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                            </td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                            </td>
                            <td align="center" style="height: 40px; background-color: #ffffff" valign="middle">
                                <asp:ImageButton ID="ibtnMigrateHCVSPreviewClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" /></td>
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
                <asp:Button ID="btnHiddenMigrateHCVSPreview" runat="server" Style="display: none" />
                <cc2:ModalPopupExtender ID="popupMigrateHCVSPreview" runat="server" TargetControlID="btnHiddenMigrateHCVSPreview"
                    PopupControlID="panMigrateHCVSPreview" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panMigrateHCVSPreviewHeading">
                </cc2:ModalPopupExtender>
                <%-- End of Popup for Preview Migrate HCVS --%>
                <%-- Popup for Preview Migrate IVSS --%>
                <asp:Panel ID="panMigrateIVSSPreview" runat="server" Style="display: none;">
                    <asp:Panel ID="panMigrateIVSSPreviewHeading" runat="server" Style="cursor: move;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 680px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                </td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="lblMigrateIVSSPreviewHeading" runat="server" Text="<%$ Resources: Text, Preview %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 680px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                            </td>
                            <td style="background-color: #ffffff" align="left">
                                <asp:Panel ID="panMigrateIVSSPreviewContent" ScrollBars="Vertical" Height="500px"
                                    runat="server" Width="97%">
                                    <table style="width: 90%">
                                        <tr>
                                            <td valign="top" colspan="2">
                                                <div class="headingText">
                                                    <asp:Label ID="Label1" runat="server" Text="<%$ Resources:Text, PersonalParticulars %>"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px" valign="top">
                                                <asp:Label ID="lblERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="lblERN" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:Image ID="imgERNInfo" runat="server" ImageUrl="~/Images/others/info.png" ImageAlign="AbsMiddle" /></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px" valign="top">
                                                <asp:Label ID="lblDateText" runat="server"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="lblDate" runat="server" CssClass="tableText"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px" valign="top">
                                                <asp:Label ID="lblNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEname" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:Label ID="lblCname" runat="server" CssClass="TextChi"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px" valign="top">
                                                <asp:Label ID="lblHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                                            <td>
                                                <asp:Label ID="lblHKID" runat="server" CssClass="tableText"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px" valign="top">
                                                <asp:Label ID="lblAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblAddress" runat="server" CssClass="tableText"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px" valign="top">
                                                <asp:Label ID="lblEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEmail" runat="server" CssClass="tableText"></asp:Label>
                                                <asp:Image ID="imgEditEmail" runat="server" ImageUrl="~/Images/others/small_edit.png"
                                                    ToolTip="<%$ Resources:ToolTip, WaitingEmailConfirmation %>" AlternateText="<%$ Resources:AlternateText, WaitingEmailConfirmation %>"
                                                    Visible="False" /></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px" valign="top">
                                                <asp:Label ID="lblContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblContactNo" runat="server" CssClass="tableText"></asp:Label></td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px" valign="top">
                                                <asp:Label ID="lblFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label ID="lblFax" runat="server" CssClass="tableText"></asp:Label></td>
                                        </tr>
                                    </table>
                                    <br />
                                    <div class="headingText">
                                        <asp:Label ID="lblMigrateIVSSMOHeading" runat="server" Text="<%$ Resources:Text, MedicalOrganizationInfo %>"></asp:Label>
                                    </div>
                                    <asp:GridView ID="gvMigrateIVSSMO" runat="server" AutoGenerateColumns="False" ShowHeader="False"
                                        Width="100%" OnRowDataBound="gvMigrateIVSSMO_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblMOIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblRegBankMOENameText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationName %>"></asp:Label></td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMOEName" runat="server" Text='<%# Bind("MOEngName") %>' CssClass="tableText"></asp:Label><br />
                                                                <asp:Label ID="lblMOCName" runat="server" Text='<%# formatChineseString(Eval("MOChiName")) %>'
                                                                   CssClass="TextChi"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMOBRCode" runat="server" Text='<%# Bind("BrCode") %>' CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMOContactNoText" runat="server" Text="<%$ Resources:Text, MOContactNo %>"></asp:Label>
                                                            </td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMOContactNo" runat="server" Text='<%# Bind("PhoneDaytime") %>'
                                                                    CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label>
                                                            </td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMOEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label>
                                                            </td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMOFax" runat="server" Text='<%# Bind("Fax") %>' CssClass="tableText">
                                                                </asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMOAddressText" runat="server" Text="<%$ Resources:Text, MOAddress %>"></asp:Label>
                                                            </td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMOAddress" runat="server" Text='<%# formatAddress(Eval("MOAddress")) %>'
                                                                    CssClass="tableText"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblMORelationText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"></asp:Label></td>
                                                            <td style="width: 650px">
                                                                <asp:Label ID="lblMORelation" runat="server" Text='<%# GetPracticeTypeName(Eval("Relationship")) %>'
                                                                    CssClass="tableText"></asp:Label>
                                                                <asp:Label ID="lblMORelationRemark" runat="server" Text='<%# formatChineseString(Eval("RelationshipRemark")) %>'
                                                                    CssClass="TextChi"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                    <br />
                                    <div class="headingText">
                                        <asp:Label ID="lblMigrateIVSSPracticeBankHeading" runat="server" Text="<%$ Resources:Text, PracticeBankInfo %>"></asp:Label>
                                    </div>
                                    <asp:GridView ID="gvMigrateIVSSPracticeBank" runat="server" AutoGenerateColumns="False"
                                        ShowHeader="False" Width="100%" OnRowDataBound="gvMigrateIVSSPracticeBank_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:Label ID="lblPracticeBankIndex" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label></ItemTemplate>
                                                <ItemStyle VerticalAlign="Top" Width="10px" />
                                            </asp:TemplateField>
                                            <asp:TemplateField>
                                                <ItemStyle VerticalAlign="Top" />
                                                <ItemTemplate>
                                                    <table width="100%">
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblPracticeTitle" runat="server" Text="<%$ Resources:Text, Practice %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="padding-left: 15px">
                                                                <table width="100%">
                                                                    <tr>
                                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                            <asp:Label ID="lblPracticeMOText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblPracticeMO" runat="server" Text='<%# Eval("MODisplaySeq") %>' CssClass="tableText"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                            <asp:Label ID="lblPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                                CssClass="tableText"></asp:Label><br />
                                                                            <asp:Label ID="lblPracticeNameChi" runat="server" Text='<%# formatChineseString(Eval("PracticeNameChi")) %>'
                                                                                CssClass="TextChi"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                            <asp:Label ID="lblPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblPracticeAddress" runat="server" Text='<%# formatAddress(Eval("PracticeAddress")) %>'
                                                                                CssClass="tableText"></asp:Label><br />
                                                                            <asp:Label ID="lblPracticeAddressChi" runat="server" Text='<%# formatChineseString(formatChiAddress(Eval("PracticeAddress"))) %>'
                                                                                CssClass="tableText"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                            <asp:Label ID="lblPracticeHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label ID="lblPracticeHealthProf" runat="server" CssClass="tableText" Text='<%# GetHealthProfName(Eval("Professional.ServiceCategoryCode")) %>'></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                            <asp:Label ID="lblPracticeRegCodeText" runat="server" Text="<%$ Resources:Text, RegCode %>"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label ID="lblPracticeRegCode" runat="server" CssClass="tableText" Text='<%# Eval("Professional.RegistrationCode") %>'></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                            <asp:Label ID="lblPracticePhoneText" runat="server" Text="<%$ Resources:Text, PracticeTel %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblPracticePhone" runat="server" CssClass="tableText" Text='<%# Eval("PhoneDaytime") %>'></asp:Label></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <hr style="width: 100%; color: #ff8080; border-top-style: none; border-right-style: none;
                                                                    border-left-style: none; height: 1px; border-bottom-style: none;" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2">
                                                                <asp:Label ID="lblBankTitle" runat="server" Text="<%$ Resources:Text, Bank %>"></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td colspan="2" style="padding-left: 15px">
                                                                <asp:Panel ID="pnlBank" runat="server">
                                                                    <table>
                                                                        <tr>
                                                                            <td style="width: 200px; background-color: #f7f7de;">
                                                                                <asp:Label ID="lblBankNameText" runat="server" Text="<%$ Resources:Text, BankName %>"></asp:Label></td>
                                                                            <td>
                                                                                <asp:Label ID="lblBankName" runat="server" Text='<%# Eval("BankAcct.BankName") %>' 
                                                                                    CssClass="TextChi"></asp:Label></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 200px; background-color: #f7f7de;">
                                                                                <asp:Label ID="lblBranchNameText" runat="server" Text="<%$ Resources:Text, BranchName %>"></asp:Label></td>
                                                                            <td>
                                                                                <asp:Label ID="lblBranchName" runat="server" Text='<%# Eval("BankAcct.BranchName") %>' 
                                                                                    CssClass="TextChi"></asp:Label></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 200px; background-color: #f7f7de;">
                                                                                <asp:Label ID="lblBankAccText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                                                            <td>
                                                                                <asp:Label ID="lblBankAcc" runat="server" Text='<%# Eval("BankAcct.BankAcctNo") %>'
                                                                                    CssClass="tableText"></asp:Label></td>
                                                                        </tr>
                                                                        <tr>
                                                                            <td style="width: 200px; background-color: #f7f7de;">
                                                                                <asp:Label ID="lblBankOwnerText" runat="server" Text="<%$ Resources:Text, BankOwner %>"></asp:Label></td>
                                                                            <td>
                                                                                <asp:Label ID="lblBankOwner" runat="server" Text='<%# Eval("BankAcct.BankAcctOwner") %>'
                                                                                    CssClass="tableText"></asp:Label></td>
                                                                        </tr>
                                                                    </table>
                                                                </asp:Panel>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </asp:Panel>
                            </td>
                            <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                            </td>
                        </tr>
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                            </td>
                            <td align="center" style="height: 40px; background-color: #ffffff" valign="middle">
                                <asp:ImageButton ID="ImageButton3" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                    ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" /></td>
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
                <asp:Button ID="btnHiddenMigrateIVSSPreview" runat="server" Style="display: none" />
                <cc2:ModalPopupExtender ID="popupMigrateIVSSPreview" runat="server" TargetControlID="btnHiddenMigrateIVSSPreview"
                    PopupControlID="panMigrateIVSSPreview" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panMigrateIVSSPreviewHeading">
                </cc2:ModalPopupExtender>
                <%-- End of Popup for Preview Migrate IVSS --%>
                <%-- Popup for Confirm Migrate HCVS --%>
                <asp:Panel ID="panPopupMigrateHCVS" runat="server" Style="display: none;">
                    <asp:Panel ID="panPopupMigrateHCVSHeading" runat="server" Style="cursor: move;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                </td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="lblPopupMigrateHCVSHeading" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                            </td>
                            <td style="background-color: #ffffff">
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left" style="width: 40px; height: 42px" valign="middle">
                                            <asp:Image ID="imgPopupMigrateHCVS" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                        <td align="center" style="height: 42px">
                                            <asp:Label ID="lblPopupMigrateHCVS" runat="server" Text="<%$ Resources:Text, MigrateConfirm %>"
                                                Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:ImageButton ID="ibtnPopupMigrateHCVSConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnPopupMigrateHCVSConfirm_Click" />
                                            <asp:ImageButton ID="ibtnPopupMigrateHCVSCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" /></td>
                                    </tr>
                                </table>
                            </td>
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
                <asp:Button ID="btnHiddenPopupMigrateHCVS" runat="server" Style="display: none" />
                <cc2:ModalPopupExtender ID="popupMigrateHCVS" runat="server" TargetControlID="btnHiddenPopupMigrateHCVS"
                    PopupControlID="panPopupMigrateHCVS" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panPopupMigrateHCVSHeading">
                </cc2:ModalPopupExtender>
                <%-- End of Popup for Confirm Migrate HCVS --%>
                <%-- Popup for Confirm Migrate IVSS --%>
                <asp:Panel ID="panIVSSMerge" runat="server" Style="display: none;">
                    <asp:Panel ID="panIVSSMergeHeading" runat="server" Style="cursor: move;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                </td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="lblPopupMigrateIVSSHeading" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                            </td>
                            <td style="background-color: #ffffff">
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left" style="width: 40px; height: 42px" valign="middle">
                                            <asp:Image ID="imgPopupMigrateIVSS" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                        <td align="center" style="height: 42px">
                                            <asp:Label ID="lblPopupMigrateIVSS" runat="server" Text="<%$ Resources:Text, MigrateConfirm %>"
                                                Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:ImageButton ID="ibtnPopupMigrateIVSSConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnPopupMigrateIVSSConfirm_Click" />
                                            <asp:ImageButton ID="ibtnPopupMigrateIVSSCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" /></td>
                                    </tr>
                                </table>
                            </td>
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
                <asp:Button ID="btnHiddenIVSSMerge" runat="server" Style="display: none" />
                <cc2:ModalPopupExtender ID="popupMigrateIVSS" runat="server" TargetControlID="btnHiddenIVSSMerge"
                    PopupControlID="panIVSSMerge" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panIVSSMergeHeading">
                </cc2:ModalPopupExtender>
                <%-- End of Popup for Migrate IVSS --%>
                <%-- Popup for Confirm Merge --%>
                <asp:Panel ID="panProceedMerge" runat="server" Style="display: none;">
                    <asp:Panel ID="panProceedMergeHeading" runat="server" Style="cursor: move;">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                            <tr>
                                <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                                </td>
                                <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                    color: #ffffff; background-repeat: repeat-x; height: 35px">
                                    <asp:Label ID="lblDeleteMOText" runat="server" Text="<%$ Resources:Text, ConfirmBoxTitle %>"></asp:Label></td>
                                <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                                </td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                            </td>
                            <td style="background-color: #ffffff">
                                <table style="width: 100%">
                                    <tr>
                                        <td align="left" style="width: 40px; height: 42px" valign="middle">
                                            <asp:Image ID="imgProceedMerge" runat="server" ImageUrl="~/Images/others/questionMark.png" /></td>
                                        <td align="center" style="height: 42px">
                                            <asp:Label ID="lblProceedMerge" runat="server" Text="<%$ Resources:Text, DataEntryMergeConfirm %>"
                                                Font-Bold="True"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="center" colspan="2">
                                            <asp:ImageButton ID="ibtnProceedMergeConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnProceedMergeConfirm_Click" />
                                            <asp:ImageButton ID="ibtnProceedMergeCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                                ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" /></td>
                                    </tr>
                                </table>
                            </td>
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
                <asp:Button ID="btnHiddenProceed" runat="server" Style="display: none" />
                <cc2:ModalPopupExtender ID="popupProceed" runat="server" TargetControlID="btnHiddenProceed"
                    PopupControlID="panProceedMerge" BackgroundCssClass="modalBackgroundTransparent"
                    DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panProceedMergeHeading">
                </cc2:ModalPopupExtender>
                <%-- End of Popup for Confirm Merge --%>
            </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>
