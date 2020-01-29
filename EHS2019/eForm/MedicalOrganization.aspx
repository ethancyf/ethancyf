<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="MedicalOrganization.aspx.vb" Inherits="eForm.MedicalOrganization"
    Title="<%$ Resources:Title, eForm %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="JS/Common.js"></script>

    <script type="text/javascript" language="javascript">
        function enableRemarkTextbox(rbo, txtbox, lbl) 
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
				    document.getElementById(lbl).className = '';
				    //document.getElementById(txtbox).style.backgroundColor='';
			    }
			    else
			    {
				    document.getElementById(txtbox).readOnly = true;
				    document.getElementById(txtbox).value = '';
				    document.getElementById(lbl).className = 'dimText';
				    //document.getElementById(txtbox).style.backgroundColor='#f5f5f5';
			    }
		    }
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
            <%-- <td>
                        <asp:Panel ID="panStep4" runat="server" CssClass="unhighlightTimeline">
                            <asp:Label ID="lblStep4" Text="<%$ Resources:Text, eFormStep4 %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panStep5" runat="server" CssClass="unhighlightTimeline">
                            <asp:Label ID="lblStep5" Text="<%$ Resources:Text, eFormStep5 %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panStep6" runat="server" CssClass="unhighlightTimeline">
                            <asp:Label ID="lblStep6" Text="<%$ Resources:Text, eFormStep6 %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                     <td>
                        <asp:Panel ID="panStep7" runat="server" CssClass="unhighlightTimeline">
                            <asp:Label ID="lblStep7" Text="<%$ Resources:Text, eFormStep7 %>" runat="server"></asp:Label></asp:Panel>
                    </td>--%>
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
                        <asp:Panel ID="panMOInfo" runat="server" CssClass="highlightTab" Width="180px" Height="35px">
                            <asp:Label ID="lblTabMOInfo" Text="<%$ Resources:Text,MedicalOrganizationInfo  %>"
                                runat="server"></asp:Label></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panPractice" runat="server" CssClass="unhighlightUnclickedTab" Width="180px"
                            Height="35px">
                            <asp:Label ID="lblTabPractice" Text="<%$ Resources:Text, PracticeInfo %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panBank" runat="server" CssClass="unhighlightUnclickedTab" Width="180px"
                            Height="35px">
                            <asp:Label ID="lblTabBank" Text="<%$ Resources:Text, BankInfo %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panScheme" runat="server" CssClass="unhighlightUnclickedTab" Width="180px"
                            Height="35px">
                            <asp:Label ID="lblTabScheme" Text="<%$ Resources:Text, SchemeInfo %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td>
                        <asp:GridView ID="gvMo" runat="server" AutoGenerateColumns="False" ShowHeader="True"
                            Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderStyle-Width="85px" HeaderText="<%$ Resources:Text, MONo %>">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRegMOIndex" runat="server" Text='<%# formatPracticeNo(Container.DataItemIndex + 1) %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Width="85px" VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="<%$ Resources:Text, MedicalOrganizationInfo %>">
                                    <ItemTemplate>
                                        <table>
                                            <tr>
                                                <td style="width: 140px; background-color: #f5f5f5" valign="top">
                                                    <asp:Label ID="lblRegMONameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, MedicalOrganizationName %>"></asp:Label></td>
                                                <td style="width: 90px; background-color: #f5f5f5" valign="top">
                                                    (<asp:Label ID="lblRegMOENameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                <td valign="top" style="width: 210px">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRegMOEName" runat="server" MaxLength="100" Width="180px" Text='<%# Bind("MOEName") %>'></asp:TextBox></td>
                                                            <td>
                                                                <asp:Image ID="imgRegMOENameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td style="width: 150px; background-color: #f5f5f5" valign="top" colspan="2">
                                                    (<asp:Label ID="lblRegMOCNameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                                <td valign="top" style="width: 210px">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="height: 24px">
                                                                <asp:TextBox ID="txtRegMOCName" runat="server" MaxLength="100" Width="180px" Text='<%# Bind("MOCName") %>' CssClass="TextBoxChi"></asp:TextBox></td>
                                                            <td style="height: 24px">
                                                                <asp:Image ID="imgRegMOCNameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" rowspan="1" style="background-color: #f5f5f5" valign="top">
                                                    <asp:Label ID="lblRegMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                                                <td valign="top" style="width: 210px">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRegMOBRCode" runat="server" MaxLength="50" Width="180px" Text='<%# Bind("MOBRCode") %>'></asp:TextBox></td>
                                                            <td>
                                                                <asp:Image ID="imgRegMOBRCodeAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td colspan="2" rowspan="1" style="background-color: #f5f5f5; width: 150px;" valign="top">
                                                    <asp:Label ID="lblRegMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                                <td valign="top" style="width: 210px">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRegMOEmail" runat="server" MaxLength="255" Width="180px" Text='<%# Bind("MOEmail") %>'></asp:TextBox></td>
                                                            <td>
                                                                <asp:Image ID="imgRegMOEmailAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" rowspan="1" style="background-color: #f5f5f5" valign="top">
                                                    <asp:Label ID="lblRegMOContactNoText" runat="server" Text="<%$ Resources:Text, MOContactNo %>"></asp:Label></td>
                                                <td valign="top" style="width: 210px">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 100px">
                                                                <asp:TextBox ID="txtRegMOContactNo" runat="server" MaxLength="20" Width="180px" Text='<%# Bind("MOContactNo") %>'></asp:TextBox></td>
                                                            <td style="width: 100px">
                                                                <asp:Image ID="imgRegMOContactNoAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td colspan="2" rowspan="1" style="background-color: #f5f5f5; width: 150px;" valign="top">
                                                    <asp:Label ID="lblRegMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                                <td valign="top" style="width: 210px">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRegMOFax" runat="server" MaxLength="20" Width="180px" Text='<%# Bind("MOFax") %>'></asp:TextBox></td>
                                                            <td>
                                                                <asp:Image ID="imgRegMOFaxAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" rowspan="1" style="background-color: #f5f5f5" valign="top">
                                                    <asp:Label ID="lblRegMOAddressText" runat="server" Text="<%$ Resources:Text, MOAddress %>"></asp:Label></td>
                                                <td valign="top" colspan="4">
                                                    <table style="width: 60%" border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:Label ID="lblRegMORoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>
                                                                <asp:TextBox ID="txtRegMORoom" runat="server" Width="50px" MaxLength="5" Text='<%# Bind("MORoom") %>' ></asp:TextBox></td>
                                                            <td>
                                                                <asp:Label ID="lblRegMOFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                                <asp:TextBox ID="txtRegMOFloor" runat="server" Width="50px" MaxLength="3" Text='<%# Bind("MOFloor") %>' ></asp:TextBox></td>
                                                            <td>
                                                                <asp:Label ID="lblRegMOBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                                <asp:TextBox ID="txtRegMOBlock" runat="server" Width="50px" MaxLength="3" Text='<%# Bind("MOBlock") %>' ></asp:TextBox></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td rowspan="1" style="width: 140px; background-color: #f5f5f5" valign="top">
                                                </td>
                                                <td style="width: 90px; background-color: #f5f5f5" valign="top">
                                                    (<asp:Label ID="lblRegMOEAddressText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                                <td valign="top" colspan="4">
                                                    <table>
                                                        <tr>
                                                            <td>
                                                                <asp:TextBox ID="txtRegMOEAddress" runat="server" Width="540px" MaxLength="100" Text='<%# Bind("MOEAddress") %>'></asp:TextBox>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgRegMOEAddressAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" rowspan="1" style="background-color: #f5f5f5" valign="top">
                                                </td>
                                                <td valign="top" colspan="4">
                                                    <table border="0" cellpadding="0" cellspacing="1">
                                                        <tr>
                                                            <td>
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:DropDownList ID="ddlRegMODistrict" runat="server" AppendDataBoundItems="true"
                                                                                AutoPostBack="false">
                                                                                <asp:ListItem Text="<%$ Resources:Text, SelectDistrict %>" Value=""></asp:ListItem>
                                                                            </asp:DropDownList><asp:HiddenField ID="hfRegMODistrict" runat="server" Value='<%# Bind("MODistrict") %>' />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Image ID="imgRegMODistrictAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="2" rowspan="1" style="background-color: #f5f5f5" valign="top">
                                                    <asp:Label ID="lblRegMORelationText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"></asp:Label></td>
                                                <td valign="top" colspan="4">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td valign="top">
                                                                <asp:RadioButtonList ID="rboRegMORelation" runat="server" RepeatDirection="Horizontal"
                                                                    RepeatColumns="6" RepeatLayout="Flow">
                                                                </asp:RadioButtonList><br />
                                                                <table border="0" cellpadding="0" cellspacing="0">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Label ID="lblPleaseSpecify" runat="server" Text="<% $ Resources:Text, PleaseSpecify %>"></asp:Label>
                                                                        </td>
                                                                        <td>
                                                                            <asp:TextBox ID="txtRegMORelationRemark" runat="server" Width="300px" MaxLength="255"
                                                                                Text='<%# Bind("MORelationRemarks") %>' CssClass="TextBoxChi"></asp:TextBox></td>
                                                                        <td>
                                                                            <asp:Image ID="imgRegMORelationRemarksAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                                Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                                    </tr>
                                                                </table>
                                                                <asp:HiddenField ID="hfRegMORelation" runat="server" Value='<%# Bind("MORelation") %>' />
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Image ID="imgRegMORelationAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxMOName" runat="server" TargetControlID="txtRegMOEName"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>

                                        <cc1:FilteredTextBoxExtender ID="FilteredRegMOBRCode" runat="server" TargetControlID="txtRegMOBRCode"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredRegMORoom" runat="server" TargetControlID="txtRegMORoom"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredRegMOFloor" runat="server" TargetControlID="txtRegMOFloor"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredRegMOBlock" runat="server" TargetControlID="txtRegMOBlock"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>

                                        <cc1:FilteredTextBoxExtender ID="FilteredMOBuilding" runat="server" TargetControlID="txtRegMOEAddress"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxMOTel" runat="server" TargetControlID="txtRegMOContactNo"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="FilteredTextBoxMOFax" runat="server" TargetControlID="txtRegMOFax"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                                        </cc1:FilteredTextBoxExtender>
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="ibtnMODelete" runat="server" AlternateText="<%$ Resources:AlternateText, DeleteSBtn %>"
                                            CausesValidation="False" CommandName="Delete" ImageUrl="<%$ Resources:ImageUrl, DeleteSBtn %>" />
                                    </ItemTemplate>
                                    <ItemStyle VerticalAlign="Top" Width="80px" />
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </td>
                </tr>
                <tr>
                    <td align="right">
                        <table style="width: 100%">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="lnkBtnPage1" runat="server" Text="1"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage2" runat="server" Text="2"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage3" runat="server" Text="3"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage4" runat="server" Text="4"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage5" runat="server" Text="5"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage6" runat="server" Text="6"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage7" runat="server" Text="7"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage8" runat="server" Text="8"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage9" runat="server" Text="9"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage10" runat="server" Text="10"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage11" runat="server" Text="11"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage12" runat="server" Text="12"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage13" runat="server" Text="13"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage14" runat="server" Text="14"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage15" runat="server" Text="15"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage16" runat="server" Text="16"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage17" runat="server" Text="17"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage18" runat="server" Text="18"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage19" runat="server" Text="19"></asp:LinkButton>
                                    <asp:LinkButton ID="lnkBtnPage20" runat="server" Text="20"></asp:LinkButton></td>
                                <td align="left">
                                    <asp:Label ID="lblMOPaging" runat="server"></asp:Label></td>
                                <td align="right">
                                    <asp:ImageButton ID="ibtnMOAdd" runat="server" ImageUrl="<%$ Resources:ImageUrl, AddSBtn %>"
                                        AlternateText="<%$ Resources:AlternateText, AddSBtn %>" OnClick="ibtnMOAdd_Click" /></td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td align="center">
                        <asp:ImageButton ID="ibtnRegMOBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnRegMOBack_Click" />
                        <asp:ImageButton ID="ibtnRegMONext" runat="server" AlternateText="<%$ Resources:AlternateText, NextBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" OnClick="ibtnRegMONext_Click" />
                    </td>
                </tr>
            </table>
            <asp:TextBox ID="txtCopyImageUrl" runat="server" Style="display: none"></asp:TextBox>
            <asp:TextBox ID="txtCopyDisableImageUrl" runat="server" Style="display: none"></asp:TextBox>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
