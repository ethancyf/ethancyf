<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="spMigration.aspx.vb" Inherits="HCVU.spMigration" Title="<%$ Resources:Title, SPDataMigration %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>
<%@ Register Src="spSummaryView.ascx" TagName="spSummaryView" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <cc2:ToolKitScriptManager ID="ScriptManager1" runat="server">
    </cc2:ToolKitScriptManager>

    <script type="text/javascript" language="javascript">
    function getERN(ERN, HKID, SPID)
    {
        document.getElementById('<%=hfSearchERN.ClientID%>').value = ERN;
        document.getElementById('<%=hfSearchHKID.ClientID%>').value = HKID;
        document.getElementById('<%=hfSearchSPID.ClientID%>').value = SPID;
        
        document.getElementById('<%=btnSpDetails.ClientID %>').click();
        return false;    
    }
    
    
    function enableRemarkTextbox(rbo, txtbox) 
    {
	    var radioObj = document.getElementById(rbo);
	    var radioList = radioObj.getElementsByTagName('input');
	    for ( var i = 0; i < radioList.length; i++)
	    {
		    if (radioList[i].checked )
		    {
			    if(radioList[i].value == 'O')
			    {
				    document.getElementById(txtbox).readOnly = false;
				    document.getElementById(txtbox).style.backgroundColor='';
			    }
			    else
			    {
				    document.getElementById(txtbox).readOnly = true;
				    document.getElementById(txtbox).value = '';
				    document.getElementById(txtbox).style.backgroundColor='#f5f5f5';
			    }
		    }
	    }
    }
    
    function LoseFocus(ddlElem) 
    {
        ddlElem.blur();
    }

    </script>

    <asp:Image ID="imgHeader" runat="server" AlternateText="Service Provider Migration"
        ImageUrl="<%$ Resources:ImageUrl, SPDataMigrationBanner %>" />
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <cc1:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="90%" />
            <cc1:MessageBox ID="udcErrorMessage" runat="server" Width="90%" />
            <asp:Panel ID="pnlDataEntry" runat="server">
                <asp:MultiView ID="MultiViewSPMigration" runat="server" ActiveViewIndex="0">
                    <asp:View ID="ViewSearch" runat="server">
                        <table>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblEnrolRefNoText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txtEnrolRefNo" runat="server" MaxLength="17" onChange="Upper(event,this)"></asp:TextBox></td>
                                <td style="width: 200px">
                                    <asp:Label ID="txtSPIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderID %>"></asp:Label></td>
                                <td>
                                    <asp:TextBox ID="txtSPID" runat="server" MaxLength="8"></asp:TextBox></td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Label ID="lblSPHKIDText" runat="server" Text="<%$ Resources:Text, ServiceProviderHKID %>"></asp:Label></td>
                                <td style="width: 250px">
                                    <asp:TextBox ID="txtSPHKID" runat="server" MaxLength="11" onChange="formatHKID(this)"></asp:TextBox></td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblMigrationRecordStatusText" runat="server" Text="Migration Record Status"></asp:Label></td>
                                <td>
                                    <asp:DropDownList ID="ddlMigrationStatus" runat="server" AppendDataBoundItems="True"
                                        Width="155px">
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
                        <br />
                    </asp:View>
                    <asp:View ID="ViewSerachResult" runat="server">
                        <div class="headingText">
                            <asp:Label ID="lblSearchResult" runat="server" Text="<%$ Resources:Text, SearchResults %>"></asp:Label>
                        </div>
                        <asp:Button ID="btnHidden" runat="server" BackColor="Transparent" BorderStyle="None"
                            Height="0px" Width="0px" OnClientClick="return false;" />
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
                                    <asp:Label ID="lblResultMigrationRecordStatusText" runat="server" Text="Migration Record Status"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblResultMigrationRecordStatus" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                        </table>
                        <br />
                        <asp:GridView ID="gvResult" runat="server" AutoGenerateColumns="False" AllowPaging="True"
                            Width="100%" AllowSorting="true">
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Enrolment_Ref_No" HeaderText="<%$ Resources:Text, EnrolRefNo %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnERN" runat="server" CommandArgument='<%# Eval("Enrolment_Ref_No") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="140px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="SP_ID" HeaderText="<%$ Resources:Text, ServiceProviderID %>">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkbtnRSPID" runat="server" Text='<%# Eval("SP_ID") %> ' CommandArgument='<%# Eval("Enrolment_Ref_No") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="SP_HKIC" HeaderText="<%$ Resources:Text, ServiceProviderHKID %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRSPHKID" runat="server" Text='<%# Eval("sp_HKIC") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="100px" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="SP_Eng_Name" HeaderText="<%$ Resources:Text, ServiceProviderName %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblREname" runat="server" Text='<%# Eval("SP_Eng_Name") %>'></asp:Label><br />
                                        <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("SP_Chi_Name") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Phone_Daytime" HeaderText="<%$ Resources:Text, ContactNo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRPhone" runat="server" Text='<%# Eval("Phone_Daytime") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField SortExpression="Migration_Status" HeaderText="<%$ Resources:Text, Status %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRStatus" runat="server" Text='<%# Eval("Migration_Status") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <table style="width: 100%">
                            <tr>
                                <td align="left">
                                    <asp:ImageButton ID="ibtnSearchResultBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSearchResultBack_Click" /></td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewSPDetail" runat="server">
                        <table style="width: 90%">
                            <td valign="top" colspan="4">
                                <div class="headingText">
                                    <asp:Label ID="lblPersonalParticulars" runat="server" Text="<%$ Resources:Text, PersonalParticulars %>"></asp:Label>
                                </div>
                            </td>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblERN" runat="server" CssClass="tableText"></asp:Label></td>
                                <td style="text-align: right; padding-right: 20px" valign="top">
                                    <asp:Label ID="lblMigrationStatusText" runat="server" Text="Migration Record Status"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblMigrationStatus" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblSPIDText" runat="server" Text="<%$ Resources:Text, SPID %>"></asp:Label></td>
                                <td colspan="3">
                                    <asp:Label ID="lblSPID" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                                <td colspan="3">
                                    <asp:Label ID="lblEname" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblCname" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                                <td colspan="3">
                                    <asp:Label ID="lblHKID" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"></asp:Label></td>
                                <td colspan="3">
                                    <asp:Label ID="lblAddress" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                <td colspan="3">
                                    <asp:Label ID="lblEmail" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Image ID="imgEditEmail" runat="server" ImageUrl="~/Images/others/small_edit.png"
                                        ToolTip="<%$ Resources:ToolTip, WaitingEmailConfirmation %>" AlternateText="<%$ Resources:AlternateText, WaitingEmailConfirmation %>"
                                        Visible="False" /></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                <td colspan="3">
                                    <asp:Label ID="lblContactNo" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                <td colspan="3">
                                    <asp:Label ID="lblFax" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblSubmittedViaText" runat="server" Text="Submitted Via "></asp:Label></td>
                                <td colspan="3">
                                    <asp:Label ID="lblSubmittedVia" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                        </table>
                        <br />
                        <div class="headingText">
                            <asp:Label ID="lblMOTitle" runat="server" Text="<%$ Resources:Text, MedicalOrganizationInfo %>"></asp:Label>
                        </div>
                        <asp:Label ID="lblMONA" runat="server" Text="<%$ Resources:Text, N/A %>" CssClass="tableText"
                            Height="40px" Style="padding-left: 50px;" Visible="False"></asp:Label>
                        <asp:GridView ID="gvMO" runat="server" AutoGenerateColumns="False" ShowHeader="True"
                            Width="100%" Visible="False">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, MONo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblMOIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, MedicalOrganizationInfo %>">
                                    <ItemTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 220px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblRegBankMOENameText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label></td>
                                                <td style="width: 650px">
                                                    <table>
                                                        <tr>
                                                            <td style="width: 100px" valign="top">
                                                                <asp:Label ID="Label6" runat="server" Text="(in English)"></asp:Label></td>
                                                            <td>
                                                                <asp:TextBox ID="txtMOEName" runat="server" Text='<%# Bind("MOEngName") %>' Width="420px"
                                                                    AutoPostBack="true" MaxLength="100" OnTextChanged="txtMOEName_TextChanged"></asp:TextBox>
                                                                <asp:Image ID="imgEditMOENameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                        </tr>
                                                        <asp:HiddenField ID="hfMOSeq" runat="server" Value='<%# Eval("DisplaySeq") %>' />
                                                        <tr>
                                                            <td style="width: 100px" valign="top">
                                                                <asp:Label ID="Label7" runat="server" Text="(in Chinese)"></asp:Label></td>
                                                            <td>
                                                                <asp:TextBox ID="txtMOCName" runat="server" Text='<%# Bind("MOChiName") %>' Width="420px"
                                                                    MaxLength="100"></asp:TextBox>
                                                                <asp:Label ID="lblMOCNameOptionalText" runat="server" Visible="false" ForeColor="#FF8080"
                                                                    Text="<%$ Resources:Text, optionalField %>"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 220px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                                                <td style="width: 650px" valign="top">
                                                    <asp:TextBox ID="txtMOBRCodeText" runat="server" Text='<%# Bind("BrCode") %>' Width="200px"
                                                        MaxLength="50"></asp:TextBox>
                                                    <asp:LinkButton ID="ibtnMOBRCodeHelp" runat="server" OnClick="ibtnMOBRCodeHelp_Click"
                                                        Visible="false" Width="60px" Height="20px" Text="Reference" />
                                                    <asp:Image ID="imgEditMOBRCodeAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 220px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblMOContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                                <td style="width: 650px">
                                                    <asp:TextBox ID="txtMOContactNoText" runat="server" Text='<%# Bind("PhoneDaytime") %>'
                                                        Width="200px" MaxLength="20"></asp:TextBox>
                                                    <asp:Image ID="imgEditMOContactNoAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 220px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                                <td style="width: 650px">
                                                    <asp:TextBox ID="txtMOEmail" runat="server" Text='<%# Bind("Email") %>' Width="200px"></asp:TextBox>
                                                    <asp:Image ID="imgEditMOEmailAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                        Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                    <asp:Label ID="lblMOEmailOptionalText" runat="server" Visible="false" ForeColor="#FF8080"
                                                        Text="<%$ Resources:Text, optionalField %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 220px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                                <td style="width: 650px">
                                                    <asp:TextBox ID="txtMOFax" runat="server" Text='<%# Bind("Fax") %>' Width="200px"></asp:TextBox>
                                                    <asp:Label ID="lblMOFaxOptionalText" runat="server" Visible="false" ForeColor="#FF8080"
                                                        Text="<%$ Resources:Text, optionalField %>"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 220px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="txtMOAddressText" runat="server" Text="<%$ Resources:Text, MOAddress %>"></asp:Label>
                                                </td>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <table style="width: 80%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblMORoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>
                                                                            <asp:TextBox ID="txtMORoom" runat="server" Width="50px" Text='<%# Eval("MOaddress.Room") %>'
                                                                                MaxLength="5"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:Label ID="lblMOFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                                            <asp:TextBox ID="txtMOFloor" runat="server" Width="50px" Text='<%# Eval("MOaddress.Floor") %>'
                                                                                MaxLength="3"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:Label ID="lblMOBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                                            <asp:TextBox ID="txtMOBlock" runat="server" Width="50px" Text='<%# Eval("MOaddress.Block") %>'
                                                                                MaxLength="3"></asp:TextBox></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <table style="width: 100%" border="0">
                                                                    <tr>
                                                                        <td style="width: 85px">
                                                                            (<asp:Label ID="lblMOBuildingText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtMOBuilding" runat="server" Text='<%# Eval("MOaddress.Building") %>'
                                                                                Width="420px" MaxLength="100"></asp:TextBox>
                                                                            <asp:Image ID="imgMOBuildingAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 85px">
                                                                            <asp:Label ID="lblMODistrictText" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label></td>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlMOEditDistrict" runat="server" AppendDataBoundItems="False"
                                                                                OnSelectedIndexChanged="ddlDistrict_SelectedIndexChanged" AutoPostBack="false"
                                                                                Width="255px">
                                                                                <asp:ListItem Text="<%$ Resources:Text, SelectDistrict %>" Value=""></asp:ListItem>
                                                                            </asp:DropDownList>
                                                                            <asp:HiddenField ID="hfMOEditDistrict" runat="server" Value='<%# Eval("MOaddress.District") %>' />
                                                                            <asp:Image ID="imgMOEditDistrictAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 220px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblMORelationText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"></asp:Label></td>
                                                <td style="width: 650px">
                                                    <asp:RadioButtonList ID="rboEditMORelation" runat="server" RepeatDirection="Horizontal"
                                                        RepeatColumns="6" RepeatLayout="Flow">
                                                    </asp:RadioButtonList>
                                                    <asp:LinkButton ID="ibtnPracticeTypeHelp" runat="server" OnClick="ibtnPracticeTypeHelp_Click"
                                                        Visible="false" Width="20px" Height="20px" Text="Reference" /></br>
                                                    <asp:Label ID="lblPleaseSpecify" runat="server" Text="<% $ Resources:Text, PleaseSpecify %>"></asp:Label>
                                                    <asp:TextBox ID="txtEditMORelationRemark" runat="server" Width="300px" MaxLength="255"
                                                        Text='<%# Eval("RelationshipRemark") %>'></asp:TextBox><asp:Image ID="imgEditMORelationRemarksAlert"
                                                            runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                    <asp:HiddenField ID="hfEditMORelation" runat="server" Value='<%# Eval("Relationship") %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnDeleteMO" runat="server" AlternateText="Delete" ImageUrl="<%$ Resources:ImageUrl, DeleteSBtn %>"
                                            OnClick="ibtnDeleteMO_Click" />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="80px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <div style="text-align: right">
                            <asp:ImageButton ID="ibtnAddMO" runat="server" AlternateText="Add" OnClick="ibtnAddMO_Click"
                                ImageUrl="<%$ Resources:ImageUrl, AddSBtn %>" /></div>
                        <br />
                        <asp:Panel ID="panPracticeBankInfo" runat="server">
                            <div class="headingText">
                                <asp:Label ID="lblPracticeBankInfo" runat="server" Text="<%$ Resources:Text, PracticeInfo %>"></asp:Label>
                            </div>
                        </asp:Panel>
                        <asp:GridView ID="gvPracticeBank" runat="server" AutoGenerateColumns="False" ShowHeader="True"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeNo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPracticeBankIndex" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeInfo %>">
                                    <ItemStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2" style="padding-left: 15px">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblPracticeMOText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label></td>
                                                            <td>
                                                                <asp:DropDownList ID="ddlPracticeMO" runat="server">
                                                                </asp:DropDownList>
                                                                <asp:HiddenField ID="hfPracticeMOSeq" runat="server" Value='<%# Eval("MODisplaySeq") %>' />
                                                                <asp:Image ID="imgEditPracticeMOAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 100px" valign="top">
                                                                            <asp:Label ID="Label3" runat="server" Text="(in English)"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label ID="lblPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                                CssClass="tableText"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 100px" valign="top">
                                                                            <asp:Label ID="Label4" runat="server" Text="(in Chinese)"></asp:Label></td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtPracticeChiName" runat="server" Text='<%# Eval("PracticeNameChi")%>'
                                                                                Width="450px" MaxLength="100"></asp:TextBox>
                                                                            <asp:LinkButton ID="ibtnPracticeChiNameHelp" runat="server" OnClick="ibtnPracticeChiNameHelp_Click"
                                                                                Visible="true" Width="60px" Height="20px" Text="Reference" />
                                                                            <asp:Label ID="lblPracticeChiNameOptionalText" runat="server" Visible="false" ForeColor="#FF8080"
                                                                                Text="<%$ Resources:Text, optionalField %>"></asp:Label></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                                            <td>
                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 80%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblPracticeRoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPracticeRoom" runat="server" Width="50px" MaxLength="5" Text='<%# Eval("PracticeAddress.Room") %>'></asp:TextBox></td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblPracticeFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPracticeFloor" runat="server" Width="50px" MaxLength="3" Text='<%# Eval("PracticeAddress.Floor") %>'></asp:TextBox></td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblPracticeBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                                                        <asp:TextBox ID="txtPracticeBlock" runat="server" Width="50px" MaxLength="3" Text='<%# Eval("PracticeAddress.Block") %>'></asp:TextBox></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <table style="width: 100%" border="0">
                                                                                <tr>
                                                                                    <td style="width: 85px">
                                                                                        (<asp:Label ID="lblPracticeBuildingText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtPracticeBuilding" runat="server" Text='<%# Eval("PracticeAddress.Building") %>'
                                                                                            Width="450px" MaxLength="100"></asp:TextBox>
                                                                                        <asp:Image ID="imgPracticeBuildingAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 100px">
                                                                                        (<asp:Label ID="lblPracticeChiBuildingText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                                                                    <td>
                                                                                        <asp:TextBox ID="txtPracticeChiAddress" runat="server" Text='<%# Eval("PracticeAddress.ChiBuilding") %>'
                                                                                            Width="450px" MaxLength="100"></asp:TextBox>
                                                                                        <asp:LinkButton ID="ibtnPracticeChiAddressHelp" runat="server" OnClick="ibtnPracticeChiAddressHelp_Click"
                                                                                            Visible="true" Width="60px" Height="20px" Text="Reference" />
                                                                                        <asp:Label ID="lblPracticeChiAddressOptionalText" runat="server" Visible="false"
                                                                                            ForeColor="#FF8080" Text="<%$ Resources:Text, optionalField %>"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 85px">
                                                                                        <asp:Label ID="lblPracticeDistrictText" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label></td>
                                                                                    <td>
                                                                                        <asp:DropDownList ID="ddlPracticeEditDistrict" runat="server" AppendDataBoundItems="false"
                                                                                            OnSelectedIndexChanged="ddlPracticeDistrict_SelectedIndexChanged" AutoPostBack="false"
                                                                                            Width="255px">
                                                                                            <asp:ListItem Text="<%$ Resources:Text, SelectDistrict %>" Value=""></asp:ListItem>
                                                                                        </asp:DropDownList>
                                                                                        <asp:HiddenField ID="hfPracticeEditDistrict" runat="server" Value='<%# Eval("PracticeAddress.District") %>' />
                                                                                        <asp:Image ID="imgPracticeEditDistrictAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
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
                                                                <asp:Label ID="lblPracticePhoneText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                                                            <td>
                                                                <asp:TextBox ID="txtPracticePhone" runat="server" Text='<%# Eval("PhoneDaytime") %>'
                                                                    MaxLength="20"></asp:TextBox>
                                                                <asp:LinkButton ID="ibtnPracticePhoneHelp" runat="server" OnClick="ibtnPracticePhoneHelp_Click"
                                                                    Visible="true" Width="60px" Height="20px" Text="Reference" />
                                                                <asp:Image ID="imgEditPracticePhoneAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                            </td>
                                                        </tr>
                                                        <asp:HiddenField ID="hfPracticeDisplaySeq" runat="server" Value='<%# Eval("DisplaySeq") %>' />
                                                        <asp:HiddenField ID="hfPracticeStatus" runat="server" Value='<%# Eval("RecordStatus") %>' />
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <asp:HiddenField ID="hfERN" runat="server" />
                        <asp:HiddenField ID="hfTableLocation" runat="server" />
                        <table style="width: 100%">
                            <tr>
                                <td align="left" style="width: 100px">
                                    <asp:ImageButton ID="ibtnBack" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnCancel_Click" /></td>
                                <td align="center">
                                    <asp:ImageButton ID="ibtnSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnSave_Click" /></td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewPreview" runat="server">
                        <table>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblPRERNText" runat="server" Text="<%$ Resources:Text, EnrolRefNo %>"></asp:Label></td>
                                <td>
                                    <table>
                                        <tr>
                                            <td style="width: 200px" align="left">
                                                <asp:Label ID="lblPRERN" runat="server" CssClass="tableText"></asp:Label></td>
                                            <td style="width: 200px" align="right">
                                                <asp:Label ID="lblPRMigrationRecordStatusText" runat="server" Text="Migration Record Status"></asp:Label></td>
                                            <td style="width: 150px" align="center">
                                                <asp:Label ID="lblPRMigrationStatus" runat="server" CssClass="tableText"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblPRSPIDText" runat="server" Text="<%$ Resources:Text, SPID %>"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblPRSPID" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblPRNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label><asp:Label
                                        runat="server" ID="Label8" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblPREname" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Label ID="lblPRCname" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblPRHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblPRHKID" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblPRAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"></asp:Label><asp:Label
                                        runat="server" ID="Label9" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblPRAddress" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblPREmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label><asp:Label
                                        runat="server" ID="Label10" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblPREmail" runat="server" CssClass="tableText"></asp:Label>
                                    <asp:Image ID="imgPREditEmail" runat="server" ImageUrl="~/Images/others/small_edit.png"
                                        ToolTip="<%$ Resources:ToolTip, WaitingEmailConfirmation %>" AlternateText="<%$ Resources:AlternateText, WaitingEmailConfirmation %>"
                                        Visible="False" /></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblPRContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label><asp:Label
                                        runat="server" ID="lblPRContactNoInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblPRContactNo" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblPRFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label><asp:Label
                                        runat="server" ID="lblPRFaxInd" Text="*" ForeColor="Red" Visible="False"></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblPRFax" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Label ID="lblPRSubmittedViaText" runat="server" Text="Submitted Via "></asp:Label></td>
                                <td>
                                    <asp:Label ID="lblPRSubmittedVia" runat="server" CssClass="tableText"></asp:Label></td>
                            </tr>
                        </table>
                        <br />
                        <div class="headingText">
                            <asp:Label ID="lblPRMOTitle" runat="server" Text="<%$ Resources:Text, MedicalOrganizationInfo %>"></asp:Label>
                        </div>
                        <asp:Label ID="Label11" runat="server" Text="<%$ Resources:Text, N/A %>" CssClass="tableText"
                            Height="40px" Style="padding-left: 50px;" Visible="False"></asp:Label>
                        <asp:GridView ID="gvPRMO" runat="server" AutoGenerateColumns="False" ShowHeader="True"
                            Width="100%" Visible="False">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, MONo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPRMOIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="100px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, MedicalOrganizationInfo %>">
                                    <ItemTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblPRRegBankMOENameText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label></td>
                                                <td style="width: 650px">
                                                    <table>
                                                        <tr>
                                                            <td style="width: 100px" valign="top">
                                                                <asp:Label ID="Label6" runat="server" Text="(in English)"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblMOEName" runat="server" Text='<%# Bind("MOEngName") %>' CssClass="tableText"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 100px" valign="top">
                                                                <asp:Label ID="Label7" runat="server" Text="(in Chinese)"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblMOCName" runat="server" Text='<%# Bind("MOChiName") %>' CssClass="tableText"
                                                                    Width="300px"></asp:Label></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblPRMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                                                <td style="width: 650px" valign="top" align="left">
                                                    <asp:Label ID="lblPRMOBRCode" runat="server" Text='<%# Bind("BrCode") %>' CssClass="tableText"
                                                        Width="300px"></asp:Label></td>
                                            </tr>
                                            <tr>
                                                <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblPRMOContactNoText" runat="server" Text="<%$ Resources:Text, MOContactNo %>"></asp:Label></td>
                                                <td style="width: 650px">
                                                    <asp:Label ID="lblprMOContactNo" runat="server" Text='<%# Bind("PhoneDaytime") %>'
                                                        CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblPRMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                                <td style="width: 650px">
                                                    <asp:Label ID="lblPRMOEmail" runat="server" Text='<%# Bind("Email") %>' CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblPRMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                                <td style="width: 650px">
                                                    <asp:Label ID="lblPRMOFax" runat="server" Text='<%# Bind("Fax") %>' CssClass="tableText"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblPRMOAddressText" runat="server" Text="<%$ Resources:Text, MOAddress %>"></asp:Label></td>
                                                <td style="width: 650px">
                                                    <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                        <tr>
                                                            <td>
                                                                <table style="width: 80%" border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblPRMORoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>
                                                                            <asp:Label ID="lblPRMORoom" runat="server" Width="50px" Text='<%# Eval("MOaddress.Room") %>'
                                                                                CssClass="tableText"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label ID="lblPRMOFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                                            <asp:Label ID="lblPRMOFloor" runat="server" Width="50px" Text='<%# Eval("MOaddress.Floor") %>'
                                                                                CssClass="tableText"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label ID="lblPRMOBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                                            <asp:Label ID="lblPRMOBlock" runat="server" Width="50px" Text='<%# Eval("MOaddress.Block") %>'
                                                                                CssClass="tableText"></asp:Label></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top">
                                                                <table style="width: 100%" border="0">
                                                                    <tr>
                                                                        <td style="width: 85px">
                                                                            (<asp:Label ID="lblPRMOBuildingText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                                        <td>
                                                                            <asp:Label ID="lblPRMOBuilding" runat="server" Text='<%# Eval("MOaddress.Building") %>'
                                                                                CssClass="tableText"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 85px">
                                                                            <asp:Label ID="lblPRMODistrictText" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label ID="lblPRMODistrict" runat="server" Text='<%# Eval("MOaddress.DistrictDesc") %>'
                                                                                CssClass="tableText"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 250px; background-color: #f7f7de;" valign="top">
                                                    <asp:Label ID="lblPRMORelationText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"></asp:Label></td>
                                                <td style="width: 650px">
                                                    <asp:Label ID="lblPRMORelation" runat="server" Text='<%# GetPracticeTypeName(Eval("Relationship")) %>'
                                                        CssClass="tableText"></asp:Label>
                                                    <asp:Label ID="lblPRMORelationRemark" runat="server" Text='<%# formatRemark(Eval("RelationshipRemark")) %>'
                                                        CssClass="tableText"></asp:Label></td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                        <br />
                        <asp:Panel ID="panPRPracticeBankInfo" runat="server">
                            <div class="headingText">
                                <asp:Label ID="lblPRPracticeBankInfo" runat="server" Text="<%$ Resources:Text, PracticeInfo %>"></asp:Label>
                            </div>
                        </asp:Panel>
                        <asp:GridView ID="gvPRPracticeBank" runat="server" AutoGenerateColumns="False" ShowHeader="True"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeNo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblPRPracticeBankIndex" runat="server" Text='<%# Eval("DisplaySeq") %>'></asp:Label></ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="10px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeInfo %>">
                                    <ItemStyle VerticalAlign="Top" />
                                    <ItemTemplate>
                                        <table width="100%">
                                            <tr>
                                                <td colspan="2" style="padding-left: 15px">
                                                    <table width="100%">
                                                        <tr>
                                                            <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblPRPracticeMOText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblPRPracticeMO" runat="server" Text='<%# Eval("MODisplaySeq") %>'
                                                                    CssClass="tableText"></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblPRPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                                            <td>
                                                                <table>
                                                                    <tr>
                                                                        <td style="width: 100px" valign="top">
                                                                            <asp:Label ID="Label3" runat="server" Text="(in English)"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label ID="lblPRPracticeName" runat="server" Text='<%# Eval("PracticeName") %>'
                                                                                CssClass="tableText"></asp:Label></td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td style="width: 100px" valign="top">
                                                                            <asp:Label ID="Label4" runat="server" Text="(in Chinese)"></asp:Label></td>
                                                                        <td>
                                                                            <asp:Label ID="lblPRPracticeNameChi" runat="server" Text='<%# Eval("PracticeNameChi") %>'
                                                                                CssClass="tableText"></asp:Label></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblPRPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                                            <td>
                                                                <table border="0" cellpadding="0" cellspacing="0" width="100%">
                                                                    <tr>
                                                                        <td>
                                                                            <table style="width: 80%" border="0" cellpadding="0" cellspacing="0">
                                                                                <tr>
                                                                                    <td>
                                                                                        <asp:Label ID="lblPracticeRoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>
                                                                                        <asp:Label ID="lblPracticeRoom" runat="server" Width="50px" Text='<%# Eval("PracticeAddress.Room") %>'
                                                                                            CssClass="tableText"></asp:Label></td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblPracticeFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                                                        <asp:Label ID="lblPracticeFloor" runat="server" Width="50px" Text='<%# Eval("PracticeAddress.Floor") %>'
                                                                                            CssClass="tableText"></asp:Label></td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblPracticeBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                                                        <asp:Label ID="lblPracticeBlock" runat="server" Width="50px" Text='<%# Eval("PracticeAddress.Block") %>'
                                                                                            CssClass="tableText"></asp:Label></td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td valign="top">
                                                                            <table style="width: 100%" border="0">
                                                                                <tr>
                                                                                    <td style="width: 85px">
                                                                                        (<asp:Label ID="lblPracticeBuildingText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblPracticeBuilding" runat="server" Text='<%# Eval("PracticeAddress.Building") %>'
                                                                                            CssClass="tableText"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 100px">
                                                                                        (<asp:Label ID="lblPracticeChiBuildingText" runat="server" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblPracticeChiAddress" runat="server" Text='<%# Eval("PracticeAddress.ChiBuilding") %>'
                                                                                            CssClass="tableText"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td style="width: 85px">
                                                                                        <asp:Label ID="lblPracticeDistrictText" runat="server" Text="<%$ Resources:Text, District %>"></asp:Label></td>
                                                                                    <td>
                                                                                        <asp:Label ID="lblPracticeDistrict" runat="server" Text='<%# Eval("PracticeAddress.DistrictDesc") %>'
                                                                                            CssClass="tableText"></asp:Label>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblPRPracticeHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblPRPracticeHealthProf" runat="server" CssClass="tableText" Text='<%# GetHealthProfName(Eval("Professional.ServiceCategoryCode")) %>'></asp:Label>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblPRPracticeRegCodeText" runat="server" Text="<%$ Resources:Text, RegCode %>"></asp:Label></td>
                                                            <td>
                                                                <asp:Label ID="lblPRPracticeRegCode" runat="server" CssClass="tableText" Text='<%# Eval("Professional.RegistrationCode") %>'></asp:Label></td>
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 200px; background-color: #f7f7de;" valign="top">
                                                                <asp:Label ID="lblPRPracticePhoneText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
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
                        <asp:HiddenField ID="hfPRERN" runat="server" />
                        <asp:HiddenField ID="hfPRTableLocation" runat="server" />
                        <table style="width: 100%">
                            <tr>
                                <td align="left" style="width: 100px">
                                    <asp:ImageButton ID="ibtnPRBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnPRBack_Click" /></td>
                                <td align="center">
                                    <asp:ImageButton ID="ibtnPRConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" OnClick="ibtnPRConfirm_Click" /></td>
                            </tr>
                        </table>
                    </asp:View>
                    <asp:View ID="ViewComplete" runat="server">
                        <table style="width: 100%">
                            <tr>
                                <td align="left" style="width: 100px">
                                    <asp:ImageButton ID="ibtnCompleteBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnCompleteBack_Click" /></td>
                            </tr>
                        </table>
                    </asp:View>
                </asp:MultiView>
            </asp:Panel>
            <asp:Panel ID="panBrCode" runat="server" Style="display: none;">
                <asp:Panel ID="panBrCodeHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 300px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblExistingSPProfileTitle" runat="server" Text="Business Registration No"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 300px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="left">
                            <asp:Panel ID="panExistingSPProfileContent" ScrollBars="Vertical" Height="200px"
                                runat="server" Width="97%">
                                <asp:GridView ID="BrCodeView" runat="server" AutoGenerateColumns="False" ShowHeader="True">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Existing BR No under this service provider">
                                            <ItemStyle VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblBrCode" runat="server" Text='<%# Eval("Br_Code") %>' />
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
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="bottom">
                            <asp:ImageButton ID="ibtnExistingSPProfileClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnDataImgratioClose_Click" /></td>
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
            <asp:Button runat="server" ID="btnHiddenShowBrCode" Style="display: none" />
            <cc2:ModalPopupExtender ID="ModalPopupExtenderDataImgration" runat="server" TargetControlID="btnHiddenShowBrCode"
                PopupControlID="panBrCode" BackgroundCssClass="modalBackgroundTransparent" DropShadow="False"
                RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panBrCodeHeading">
            </cc2:ModalPopupExtender>
            <asp:Panel ID="panPracticeType" runat="server" Style="display: none;">
                <asp:Panel ID="panPracticeTypeHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 300px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label12" runat="server" Text="Relationship"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 300px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="left">
                            <asp:Panel ID="panExistingPracticeTypeContent" ScrollBars="Vertical" Height="200px"
                                runat="server" Width="97%">
                                <asp:GridView ID="PracticeTypeView" runat="server" AutoGenerateColumns="False" ShowHeader="True">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Existing relationship under this service provider">
                                            <ItemStyle VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:Label ID="lblPracticeType" runat="server" Text='<%# GetPracticeTypeName(Eval("Practice_Type")) %>' />
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
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="bottom">
                            <asp:ImageButton ID="ibtnExistingPracticeTypeClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnDataImgratioClose_Click" /></td>
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
            <asp:Button runat="server" ID="btnHiddenShowPracticeType" Style="display: none" />
            <cc2:ModalPopupExtender ID="ModalPopupExtenderPracticeType" runat="server" TargetControlID="btnHiddenShowPracticeType"
                PopupControlID="panPracticeType" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panPracticeTypeHeading">
            </cc2:ModalPopupExtender>
            <asp:Panel ID="panPracticeChiName" runat="server" Style="display: none;">
                <asp:Panel ID="panPracticeChiNameheading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label1" runat="server" Text="Practice Chinese Name"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="left">
                            <asp:Panel ID="panPracticeChiNameContent" ScrollBars="Vertical" Height="200px" runat="server"
                                Width="97%">
                                <asp:GridView ID="PracticeChiNameView" runat="server" AutoGenerateColumns="False"
                                    ShowHeader="True" Width="460px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Existing Practice Chinese Name Under This Service Provider">
                                            <ItemStyle VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="ibtnPracticeChiName" runat="server" OnClick="ibtnPracticeChiName_Click"
                                                    Text='<%# Eval("Practice_Name_Chi") %>' />
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
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="bottom">
                            <asp:ImageButton ID="ibtnPracticeChiNameClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnPracticeChiNameClose_Click" /></td>
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
            <asp:Button runat="server" ID="btnHiddenShowPracticeChiName" Style="display: none" />
            <cc2:ModalPopupExtender ID="ModalPopupExtenderPracticeChiName" runat="server" TargetControlID="btnHiddenShowPracticeChiName"
                PopupControlID="panPracticeChiName" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panPracticeChiNameHeading">
            </cc2:ModalPopupExtender>
            <asp:Panel ID="panPracticePhone" runat="server" Style="display: none;">
                <asp:Panel ID="panPracticePhoneHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 300px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label2" runat="server" Text="Daytime Contact Phone No."></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 300px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="left">
                            <asp:Panel ID="panPracticePhoneContent" ScrollBars="Vertical" Height="200px" runat="server"
                                Width="97%">
                                <asp:GridView ID="PracticePhoneView" runat="server" AutoGenerateColumns="False" ShowHeader="True">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Existing Practice Daytime Contact Phone No. Under This Service Provider">
                                            <ItemStyle VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="ibtnPracticePhone" runat="server" OnClick="ibtnPracticePhone_Click"
                                                    Text='<%# Eval("Phone_Daytime") %>' />
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
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="bottom">
                            <asp:ImageButton ID="ibtnPracticePhoneClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnPracticePhoneClose_Click" /></td>
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
            <asp:Button runat="server" ID="btnHiddenShowPracticePhone" Style="display: none" />
            <cc2:ModalPopupExtender ID="ModalPopupExtenderPracticePhone" runat="server" TargetControlID="btnHiddenShowPracticePhone"
                PopupControlID="panPracticePhone" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="RepositionOnWindowScroll" PopupDragHandleControlID="panPracticePhoneHeading">
            </cc2:ModalPopupExtender>
            <asp:Panel ID="panPracticeChiAddress" runat="server" Style="display: none;">
                <asp:Panel ID="panPracticeChiAddressHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label5" runat="server" Text="Practice Chinese Address"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="left">
                            <asp:Panel ID="panPracticeChiAddressContent" ScrollBars="Vertical" Height="200px"
                                runat="server" Width="100%">
                                <asp:GridView ID="PracticeChiAddressView" runat="server" AutoGenerateColumns="False"
                                    ShowHeader="True" Width="460px">
                                    <Columns>
                                        <asp:TemplateField HeaderText="Existing Practice Chinese Address Under This Service Provider">
                                            <ItemStyle VerticalAlign="Top" />
                                            <ItemTemplate>
                                                <asp:LinkButton ID="ibtnPracticeChiAddress" runat="server" OnClick="ibtnPracticeChiAddress_Click"
                                                    Text='<%# Eval("PracticeAddress.ChiBuilding") %>' />
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
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="bottom">
                            <asp:ImageButton ID="ibtnPracticeChiAddressClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnPracticeChiAddressClose_Click" /></td>
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
            <asp:Button runat="server" ID="btnHiddenShowPracticeChiAddress" Style="display: none" />
            <cc2:ModalPopupExtender ID="ModalPopupExtenderPracticeChiAddress" runat="server"
                TargetControlID="btnHiddenShowPracticeChiAddress" PopupControlID="panPracticeChiAddress"
                BackgroundCssClass="modalBackgroundTransparent" DropShadow="False" RepositionMode="RepositionOnWindowScroll"
                PopupDragHandleControlID="panPracticeChiAddressHeading">
            </cc2:ModalPopupExtender>
            <asp:HiddenField ID="hfBackToDataEntry" runat="server" />
            <asp:HiddenField ID="hfGridviewIndex" runat="server" />
            <asp:HiddenField ID="hfPracticeGridviewIndex" runat="server" />
            <asp:HiddenField ID="hfSearchERN" runat="server" />
            <asp:HiddenField ID="hfSearchHKID" runat="server" />
            <asp:HiddenField ID="hfSearchSPID" runat="server" />
            <asp:Button ID="btnSpDetails" runat="server" Text="" Style="display: none" OnClick="btnSpDetails_Click" />
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
