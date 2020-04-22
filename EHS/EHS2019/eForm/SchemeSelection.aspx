<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="SchemeSelection.aspx.vb" Inherits="eForm.SchemeSelection" Title="<%$ Resources:Title, eForm %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="UIControl/PCDIntegration/ucTypeOfPracticeGrid.ascx" TagName="ucTypeOfPracticeGrid"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="JS/Common.js"></script>

    <script type="text/javascript" language="javascript">
        function enableRemarkTextbox(chk, txtbox) 
        {
	        var chkObj = document.getElementById(chk);
	        if(chkObj.checked)
		    {
		        document.getElementById(txtbox).readOnly = true;
		        document.getElementById(txtbox).value = '';
		        document.getElementById(txtbox).style.border = 'solid';
		        document.getElementById(txtbox).style.borderWidth = 'thin';
		        document.getElementById(txtbox).style.borderColor = '#bababa';
		        document.getElementById(txtbox).style.backgroundColor = '#e5e5e5';
		    }
		    else
		    {
	            document.getElementById(txtbox).readOnly = false;
	            document.getElementById(txtbox).style.border = 'solid';
	            document.getElementById(txtbox).style.borderWidth = 'thin';
	            document.getElementById(txtbox).style.borderColor = '#666666';
	            document.getElementById(txtbox).style.backgroundColor = '#ffffff';
		    }		
        }
    
    </script>

    <table width="100%">
        <tr>
            <td>
                <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, eFormSpEnrolmentBanner %>"
                    ImageUrl="<%$ Resources:ImageUrl, eFormSpEnrolmentBanner %>" /></td>
            <td align="right">
                <asp:ImageButton ID="iBtnLoadDemoData" runat="server" ImageUrl="~/Images/button/btn_DemoTestingCase.png" /></td>
        </tr>
    </table>
    <table border="0" cellpadding="0" cellspacing="0" width="600">
        <tr>
            <td>
                <asp:Panel ID="panStep1" runat="server" CssClass="highlightTimeline">
                    <asp:Label ID="lblStep1" Text="<%$ Resources:Text, eFormStep1 %>" runat="server"></asp:Label></asp:Panel>
            </td>
            <td>
                <asp:Panel ID="panStep2" runat="server" CssClass="unhighlightTimeline">
                    <asp:Label ID="lblStep2" Text="<%$ Resources:Text, eFormStep2 %>" runat="server"></asp:Label></asp:Panel>
            </td>
            <td>
                <asp:Panel ID="panStep3" runat="server" CssClass="unhighlightTimeline">
                    <asp:Label ID="lblStep3" Text="<%$ Resources:Text, eFormStep3 %>" runat="server"></asp:Label></asp:Panel>
            </td>
        </tr>
    </table>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <cc2:MessageBox ID="udcMsgBox" runat="server" Width="95%" />
            <br />
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkBtnPersonal" Text="<%$ Resources:Text, PersonalParticulars %>"
                            runat="server" CssClass="unhighlightTab" Width="180px" Height="35px" onmouseover="this.className ='highlightTab'"
                            onmouseout="this.className='unhighlightTab'" OnClick="lnkBtnPersonal_Click"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkBtnMO" Text="<%$ Resources:Text, MedicalOrganizationInfo %>"
                            runat="server" CssClass="unhighlightTab" Width="180px" Height="35px" onmouseover="this.className ='highlightTab'"
                            onmouseout="this.className='unhighlightTab'" OnClick="lnkBtnMO_Click"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkBtnPractice" Text="<%$ Resources:Text, PracticeInfo %>" runat="server"
                            CssClass="unhighlightTab" Width="180px" Height="35px" onmouseover="this.className ='highlightTab'"
                            onmouseout="this.className='unhighlightTab'" OnClick="lnkBtnPractice_Click"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="lnkBtnBank" Text="<%$ Resources:Text, BankInfo %>" runat="server"
                            CssClass="unhighlightTab" Width="180px" Height="35px" onmouseover="this.className ='highlightTab'"
                            onmouseout="this.className='unhighlightTab'" OnClick="lnkBtnBank_Click"></asp:LinkButton>
                    </td>
                    <td>
                        <asp:Panel ID="panSchemeInfo" runat="server" CssClass="highlightTab" Width="180px"
                            Height="35px">
                            <asp:Label ID="lblTabSchemeInfo" Text="<%$ Resources:Text, SchemeInfo %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <div class="headingText">
                            <asp:Label ID="lblSchemeSelection" runat="server" Text="<%$ Resources:Text, SchemeInfo %>"></asp:Label></div>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Panel ID="pnlSchemeSelection" runat="server">
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>
                        <br />
                        <asp:Panel ID="pnlPractice" runat="server">
                            <asp:GridView ID="gvPractice" runat="server" AutoGenerateColumns="False" >
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeNo %>">
                                        <ItemTemplate>
                                            <asp:Label ID="lblPracticeIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                            <asp:HiddenField ID="hfFormatedPracticeIndex" runat="server" Value='<%# Bind("PracticeIndex") %>' />
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="90px"/>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeInfo %>">
                                        <ItemTemplate>
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="width: 150px; background-color: #f5f5f5;" valign="top">
                                                        <asp:Label ID="lblRegBankMONameText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblRegBankMOName" runat="server" CssClass="tableText" Text='<%# Bind("MOIndex") %>'></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 150px; background-color: #f5f5f5;" valign="top">
                                                        <asp:Label ID="lblRegBankPracticeENameText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationName %>"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblRegBankPracticeEName" runat="server" Text='<%# Bind("PracticeName") %>'
                                                            CssClass="tableText"></asp:Label><br />
                                                        <asp:Label ID="lblRegBankPracticeCName" runat="server" Text='<%# formatChineseString(Eval("PracticeNameChi")) %>'
                                                            CssClass="TextChi"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 150px; background-color: #f5f5f5;" valign="top">
                                                        <asp:Label ID="lblRegBankPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                                    <td>
                                                        <asp:Label ID="lblRegBankPracticeAddress" runat="server" Text='<%# formatAddress(Eval("Room"), Eval("Floor"), Eval("Block"), Eval("Building"), Eval("District")) %>'
                                                            CssClass="tableText"></asp:Label><br />
                                                        <asp:Label ID="lblRegBankPracticeAddressChi" runat="server" Text='<%# formatChineseString(formatChiAddress(Eval("Room"), Eval("Floor"), Eval("Block"), Eval("ChiBuilding"), Eval("District"))) %>'
                                                            CssClass="TextChi"></asp:Label></td>
                                                </tr>
                                                <tr>
                                                    <td style="width: 150px; background-color: #f5f5f5;" valign="top">
                                                        <asp:Label ID="lblSchemeToEnrollText" runat="server" Text="<%$ Resources:Text, SchemeToEnroll %>"></asp:Label>
                                                    </td>
                                                    <td>                                                       
                                                        <asp:GridView ID="gvSchemeToEnroll" runat="server" AutoGenerateColumns="false" SkinID="GridViewSchemeToEnrollSkin"
                                                            OnRowDataBound="gvSchemeToEnroll_RowDataBound" OnRowCreated="gvSchemeToEnroll_RowCreated"
                                                             ShowHeader="false" >
                                                            <Columns>
                                                                <asp:TemplateField>
                                                                    <ItemTemplate>
                                                                        <asp:label ID="lblSchemeToEnroll" runat="server" CssClass="tableText" Style="position:relative;left:15px"></asp:label>
                                                                    </ItemTemplate>
                                                                    <ItemStyle HorizontalAlign="left" VerticalAlign="Top"/>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </td>
                                                </tr>
                                            </table>
<%--                                            <table style="width: 100%" border="1">
                                                <tr style="height:1px">
                                                    <td style="border-width: 1px 0px 0px 0px"></td>
                                                </tr>
                                            </table>--%>
                                            <asp:Panel ID="pnlInputServiceFee" runat="server" visible="false">
                                                <hr style ="width:100%;height:1px;border:none;background-color:#989898;margin-top:2px;margin-bottom:2px">
                                                <table style="width: 100%">
                                                    <tr>
                                                        <td style="width: 150px; background-color: #f5f5f5;" valign="top">
                                                            <asp:Label ID="lblSchemeInfoText" runat="server" Text="<%$ Resources:Text, SchemeInfo %>"></asp:Label>
                                                        </td>
                                                        <td>
                                                            <asp:GridView ID="gvPracticeScheme" runat="server" AutoGenerateColumns="false" ShowHeader="false" SkinID="GridViewSchemeToEnrollSkin"
                                                                 OnRowDataBound="gvPracticeScheme_RowDataBound">
                                                                <Columns>
                                                                    <asp:TemplateField>
                                                                        <ItemTemplate>
                                                                            <asp:Label ID="lblSchemeDescText" runat="server" Text="" CssClass="tableText" Style="padding-bottom:1px"></asp:Label>
                                                                            <asp:HiddenField ID="hfFormatedPracticeSchemeIndex" runat="server" />
                                                                            <br>
                                                                            <div style="padding-left:20px">
                                                                                <asp:GridView ID="gvServiceFee" runat="server" AutoGenerateColumns="false"
                                                                                    OnRowDataBound="gvServiceFee_RowDataBound" SkinID="GridViewSubSchemeSkin" ShowHeader="True"
                                                                                    OnRowCreated="gvServiceFee_RowCreated" >
                                                                                    <Columns>
                                                                                        <asp:TemplateField>
                                                                                            <HeaderTemplate>
                                                                                                <asp:Label ID="lblCategoryTitle" runat="server" Text="<%$ Resources:Text, Category %>"></asp:Label>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblCategory" runat="server"/>
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="left" VerticalAlign="Top" Width="260px"/>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <HeaderTemplate>
                                                                                                <asp:Label ID="lblSubsidyTitle" runat="server" Text="<%$ Resources:Text, Vaccine %>"></asp:Label>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>  
                                                                                                <asp:CheckBox ID="chkProvideSubsidy" runat="server" Enabled="false"/>
                                                                                                <asp:Label ID="lblSubsidizeDisplayCode" runat="server" style="position:relative;top:-1px"></asp:Label>
                                                                        
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="left" Width="100px"/>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <HeaderTemplate>
                                                                                                <asp:Label ID="lblServiceFeeTitle" runat="server" Text="<%$ Resources:Text, ServiceFee %>"></asp:Label>
                                                                                                <asp:Label ID="lblServiceFeeStar" runat="server" Text=" *"></asp:Label>
                                                                                            </HeaderTemplate>
                                                                                            <ItemTemplate>
                                                                                                <asp:Label ID="lblDollarSign" runat="server" Width="10px" Text="$"></asp:Label>
                                                                                                <asp:TextBox ID="txtServiceFee" runat="server" Width="30px" MaxLength="4" style="position:relative;top:-1px"
                                                                                                    CssClass="gvTxtBoxReadOnlyBg" onblur="changeInt(this);"></asp:TextBox>
                                                                                                <cc1:FilteredTextBoxExtender ID="FilteredServieFee" runat="server" TargetControlID="txtServiceFee"
                                                                                                    FilterType="Custom, Numbers">
                                                                                                </cc1:FilteredTextBoxExtender>
                                                                                                <asp:CheckBox ID="chkNotProviderServiceFee" runat="server" style="position:relative;top:1px"
                                                                                                    AutoPostBack="true" OnCheckedChanged="chkNotProviderServiceFee_CheckedChanged" Visible="False"/> 
                                                                                                <asp:Label ID="lblNotProviderServiceFee" runat="server" style="position:relative;top:-1px" Visible="False"></asp:Label>
                                                                                                <asp:Image ID="imgServiceFeeAlert" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" CssClass="gvImgAlert"
                                                                                                    runat="server" ImageUrl="~/Images/others/icon_caution.gif" Visible="False" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="left" Width="100px" CssClass="gvTableRowNoWrap"/>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <HeaderStyle CssClass="gvTableRowNoDisplay"/>
                                                                                            <ItemTemplate>
                                                                                                <asp:HiddenField ID="hfSchemeCode" runat="server" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="left" CssClass="gvTableRowNoDisplay"/>
                                                                                        </asp:TemplateField>
                                                                                        <asp:TemplateField>
                                                                                            <HeaderStyle CssClass="gvTableRowNoDisplay"/>
                                                                                            <ItemTemplate>  
                                                                                                <asp:HiddenField ID="hfSubsidizeCode" runat="server" />
                                                                                            </ItemTemplate>
                                                                                            <ItemStyle HorizontalAlign="left" CssClass="gvTableRowNoDisplay"/>
                                                                                        </asp:TemplateField>
                                                                                    </Columns>
                                                                                </asp:GridView>
                                                                            </div>
                                                                        </ItemTemplate>
                                                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top" Width="520px" CssClass="gvTableCell" />
                                                                    </asp:TemplateField>
                                                                </Columns>
                                                            </asp:GridView>
                                                        </td>
                                                    </tr>
                                                </table> 
                                            </asp:Panel>                                           
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="800px"/>
                                    </asp:TemplateField>
                                </Columns>

                            </asp:GridView>
                            <br />
                            <asp:Label ID="lblServiceFeeStatement" runat="server" Text="<%$ Resources:Text, ServiceFeeStatement %>"></asp:Label>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Panel ID="panEHRSS" runat="server" Visible="False">
                <table>
                    <tr>
                        <td>
                            <div class="headingTextOtherSystem">
                                <asp:Label ID="lblEHRSSText" runat="server" Text="<%$ Resources:Text, EHRSS %>"></asp:Label></div>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblHadJoinedEHRSSQ" runat="server" Text="<%$ Resources:Text, HadJoinedEHRSSQ %>"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table border="0" cellpadding="0" cellspacing="0">
                                <tr>
                                    <td valign="top">
                                        <asp:RadioButtonList ID="rboHadJoinedEHRSS" runat="server" RepeatDirection="Vertical">
                                            <asp:ListItem Value="Y" Text="<%$ Resources:Text, Yes %>"></asp:ListItem>
                                            <asp:ListItem Value="N" Text="<%$ Resources:Text, No %>"></asp:ListItem>
                                        </asp:RadioButtonList></td>
                                    <td valign="top">
                                        <asp:Image ID="imgHadJoinedEHRSSAlert" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Panel ID="panPCD" runat="server" >
                <asp:Panel ID="panWillJoinPCD" runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="headingTextOtherSystem">
                                    <asp:Label ID="lblPCDText" runat="server" Text="<%$ Resources:Text, PCD %>"></asp:Label></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblWillJoinPCD" runat="server" Text="<%$ Resources:Text, WillJoinPCD %>"></asp:Label></td>
                        </tr>
                        <tr>
                            <td>
                                <table border="0" cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td valign="top">
                                            <asp:RadioButtonList ID="rboWillJoinPCD" runat="server" RepeatDirection="Vertical"
                                                AutoPostBack="true">
                                                <asp:ListItem Value="Y" Text="<%$ Resources:Text, Yes %>"></asp:ListItem>
                                                <asp:ListItem Value="E" Text="<%$ Resources:Text, No_JoinedPCD %>"></asp:ListItem>
                                                <asp:ListItem Value="N" Text="<%$ Resources:Text, No_NotJoinPCD %>"></asp:ListItem>
                                            </asp:RadioButtonList></td>
                                        <td valign="top">
                                            <asp:Image ID="imgWillJoinPCDAlert" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="panSelectPracticeType" runat="server" Style="display: none;">
                    <table cellpadding="0" cellspacing="0">
                        <tr style="height: 30px;">
                            <td>
                                <asp:Label ID="lblSelectPracticeType" runat="server" Text="<%$ Resources:Text, TypeOfPracticeHeader %>"
                                    CssClass="tableText"></asp:Label>
                                <asp:Image ID="imgPCDTypeOfPracticeAlert" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>"
                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                            </td>
                        </tr>
                    </table>
                    <uc1:ucTypeOfPracticeGrid ID="ucTypeOfPracticeGrid" runat="server" />
                </asp:Panel>
            </asp:Panel>
            <table style="width: 100%; height: 50px">
                <tr>
                    <td align="center">
                        <asp:ImageButton ID="ibtnSchemeSelectBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnSchemeSelectBack_Click" />
                        <asp:ImageButton ID="ibtnSchemeSelectNext" runat="server" AlternateText="<%$ Resources:AlternateText, NextBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" OnClick="ibtnSchemeSelectNext_Click" /></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
