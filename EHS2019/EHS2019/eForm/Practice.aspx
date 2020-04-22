<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="Practice.aspx.vb" Inherits="eForm.Practice" Title="<%$ Resources:Title, eForm %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="JS/Common.js"></script>

    <script type="text/javascript" language="javascript">

    
     function chkCopyChanged()
    {
        var chkList = document.getElementById('<%=choCopyList.ClientID%>');
        var chk = chkList.getElementsByTagName('input')
        var ibtn = document.getElementById('<%=ibtnCopyConfirm.ClientID %>');        
        var bln = false;
        
        for(i = 0; i < chk.length;i++)  
        {
            if (chk[i].checked )
		    {
			    bln = true;
		    }
        }
        if (bln)
        {
            ibtn.disabled=false;
            ibtn.src = document.getElementById('<%=txtCopyImageUrl.ClientID%>').value.replace(/~/, ".");
        }
        else
        {
            ibtn.disabled=true;
            ibtn.src = document.getElementById('<%=txtCopyDisableImageUrl.ClientID%>').value.replace(/~/, ".");
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
            <%--<td>
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
            <asp:Panel ID="panCopyList" runat="server" Style="display: none;">
                <table border="0" cellpadding="0" cellspacing="0" style="width: 620px">
                    <tr>
                        <td style="background-image: url(Images/dialog/top-left.png); width: 7px; height: 35px">
                        </td>
                        <td style="font-weight: bold; font-size: 14px; background-image: url(Images/dialog/top-mid.png);
                            color: #ffffff; background-repeat: repeat-x; height: 35px">
                            <asp:Label ID="lblCopyListTitle" runat="server" Text="<%$ Resources:Text, CopyList %>"></asp:Label></td>
                        <td style="background-image: url(Images/dialog/top-right.png); width: 7px; height: 35px">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td colspan="2" style="width: 200px">
                                        <asp:Label ID="lblCopyText" runat="server" Text="<%$ Resources:Text, CopyInfoFrom %>"></asp:Label></td>
                                    <td>
                                        <asp:DropDownList ID="ddlPracticeList" runat="server" AutoPostBack="false" Width="200px">
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3">
                                        <asp:CheckBoxList ID="choCopyList" runat="server" RepeatColumns="2" RepeatLayout="Table"
                                            CellPadding="5" CellSpacing="5" >
                                            <asp:ListItem Text="<%$ Resources:Text, MedicalOrganization %>" Value="0"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Text, PracticeName %>" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Text, PracticeAddress %>" Value="2"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Text, PracticeTel %>" Value="3"></asp:ListItem>
                                            <%--<asp:ListItem Text="<%$ Resources:Text, HealthProf %>" Value="4"></asp:ListItem>
                                            <asp:ListItem Text="<%$ Resources:Text, RegCode %>" Value="5"></asp:ListItem>--%>
                                        </asp:CheckBoxList>
                                    </td>
                                </tr>
                            </table>
                            <table style="width: 100%">
                                <tr>
                                    <td align="center">
                                        <asp:ImageButton ID="ibtnCopyConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, CopyBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CopyBtn %>" OnClick="ibtnCopyConfirm_Click" />
                                        <asp:ImageButton ID="ibtnNewPractice" runat="server" AlternateText="<%$ Resources:AlternateText, NewPracticeBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, NewPracticeBtn %>" OnClick="ibtnNewPractice_Click" />
                                        <asp:ImageButton ID="ibtnCopyCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnCopyCancel_Click" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(Images/dialog/bottom-left.png); width: 7px; height: 7px">
                        </td>
                        <td style="background-image: url(Images/dialog/bottom-mid.png); background-repeat: repeat-x;
                            height: 7px">
                        </td>
                        <td style="background-image: url(Images/dialog/bottom-right.png); width: 7px; height: 7px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
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
                        <asp:Panel ID="panPracticeInfo" runat="server" CssClass="highlightTab" Width="180px"
                            Height="35px">
                            <asp:Label ID="lblTabPracticeInfo" Text="<%$ Resources:Text, PracticeInfo %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panBank" runat="server" CssClass="unhighlightUnclickedTab" Width="180px" Height="35px">
                            <asp:Label ID="lblTabBank" Text="<%$ Resources:Text, BankInfo %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panScheme" runat="server" CssClass="unhighlightUnclickedTab" Width="180px"
                            Height="35px">
                            <asp:Label ID="lblTabScheme" Text="<%$ Resources:Text, SchemeInfo %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:Label ID="lblRegPracticeSkipStatement" runat="server" Text="<%$ Resources:Text, RegPracticeSkipStatement %>" Visible="false"></asp:Label>
            <asp:GridView ID="gvRegPractice" runat="server" AutoGenerateColumns="False" ShowHeader="True"
                Width="100%">
                <Columns>
                    <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeNo %>">
                        <ItemTemplate>
                            <asp:Label ID="lblRegPracticeIndex" runat="server" Text='<%# formatPracticeNo(Container.DataItemIndex + 1) %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="100px" VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeInfo %>">
                        <ItemTemplate>
                            <table width="100%">
                                <tr>
                                    <td colspan="2" style="background-color: #f5f5f5" valign="top">
                                        <asp:Label ID="lblMOText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label>
                                    </td>
                                    <td colspan="4" valign="top">
                                        <asp:DropDownList ID="ddlMO" runat="server" AppendDataBoundItems="true">
                                            <asp:ListItem Text="<%$ Resources:Text, SelectMO %>" Value=""></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hfMO" runat="server" Value='<%# Bind("MOIndex") %>' />
                                         <asp:Image ID="imgRegPracticeMONameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" />
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td style="width: 120px; background-color: #f5f5f5" valign="top">
                                        <asp:Label ID="lblRegPracticeNameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                    <td style="width: 80px; background-color: #f5f5f5" valign="top">
                                        (<asp:Label ID="lblRegPracticeENameText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                    <td style="width: 250px">
                                        <asp:TextBox ID="txtRegPracticeName" runat="server" Width="210px" Text='<%# Bind("PracticeName") %>'
                                            MaxLength="100" Style="vertical-align:top"></asp:TextBox>
                                        <asp:Image ID="imgRegPracticeNameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" style="vertical-align:top"/></td>
                                    <td style="width: 300px; background-color: #f5f5f5" valign="top" colspan="2">
                                        (<asp:Label ID="lblRegPracticeCNameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtRegPracticeCName" runat="server" MaxLength="100" Width="120px"
                                            Text='<%# Bind("PracticeNameChi") %>' CssClass="TextBoxChi"></asp:TextBox>
                                        <asp:Image ID="imgRegPracticeCNameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                </tr>
                                <tr>
                                    <td colspan="2" rowspan="1" style="background-color: #f5f5f5" valign="top">
                                        <asp:Label ID="lblRegPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                    <td valign="top" colspan="4">
                                        <table style="width: 60%" border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblRegPracticeRoomText" runat="server" Text="<%$ Resources:Text, Room %>"></asp:Label>
                                                    <asp:TextBox ID="txtRegPracticeRoom" runat="server" Width="50px" Text='<%# Bind("Room") %>'
                                                        MaxLength="5"></asp:TextBox></td>
                                                <td>
                                                    <asp:Label ID="lblRegPracticeFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"></asp:Label>
                                                    <asp:TextBox ID="txtRegPracticeFloor" runat="server" Width="50px" Text='<%# Bind("Floor") %>'
                                                        MaxLength="3"></asp:TextBox></td>
                                                <td>
                                                    <asp:Label ID="lblRegPracticeBlockText" runat="server" Text="<%$ Resources:Text, Block %>"></asp:Label>
                                                    <asp:TextBox ID="txtRegPracticeBlock" runat="server" Width="50px" Text='<%# Bind("Block") %>'
                                                        MaxLength="3"></asp:TextBox></td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr valign="top">
                                    <td rowspan="2" style="width: 120px; background-color: #f5f5f5" valign="top">
                                    </td>
                                    <td style="width: 80px; background-color: #f5f5f5">
                                        (<asp:Label ID="lblRegPracticeEAddressText" runat="server" Text="<%$ Resources:Text, InEnglish %>"></asp:Label>)</td>
                                    <td colspan="4">
                                        <asp:TextBox ID="txtRegPracticeEAddress" runat="server" MaxLength="100" Text='<%# Bind("Building") %>'
                                            Width="500px" style="vertical-align:top"></asp:TextBox>
                                        <asp:Image ID="imgRegPracticeEAddressAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" style="vertical-align:top"/></td>
                                </tr>
                                <tr>
                                    <td style="width: 80px; background-color: #f5f5f5" valign="top">
                                        (<asp:Label ID="lblRegPracticeCAddressText" runat="server" CssClass="tableTitle"
                                            Text="<%$ Resources:Text, InChinese %>"></asp:Label>)</td>
                                    <td valign="top" colspan="4">
                                        <asp:TextBox ID="txtRegPracticeCAddress" runat="server" MaxLength="100" Text='<%# Bind("ChiBuilding") %>'
                                            Width="500px" CssClass="TextBoxChi"></asp:TextBox>
                                        <asp:Image ID="imgRegPracticeCAddressAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                </tr>
                                <tr>
                                    <td style="background-color: #f5f5f5;" valign="top" colspan="2">
                                    </td>
                                    <td valign="top" colspan="4">
                                        <table border="0" cellpadding="0" cellspacing="1">
                                            <tr>
                                                <td>
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td>
                                                                <asp:DropDownList ID="ddlRegPracticeDistrict" runat="server" AutoPostBack="false">
                                                                    <asp:ListItem Text="<%$ Resources:Text, SelectDistrict %>" Value=""></asp:ListItem>
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td>
                                                                <asp:Image ID="imgRegPracticeDistrictAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" style="position:relative;top:-1px"/></td>
                                                        </tr>
                                                    </table>
                                                    <asp:HiddenField ID="hfRegPracticeDistrict" runat="server" Value='<%# Bind("District") %>' />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <tr valign="top">
                                        <td style="background-color: #f5f5f5;" valign="top" colspan="2">
                                            <asp:Label ID="lblRegPracitceHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label>
                                        </td>
                                        <td  colspan="4">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td>
                                                        <asp:DropDownList ID="ddlRegPracticeHealthProf" runat="server" AutoPostBack="true"
                                                            AppendDataBoundItems="true" OnSelectedIndexChanged="ddlRegPracticeHealthProf_SelectedIndexChanged">
                                                            <asp:ListItem Text="<%$ Resources:Text, SelectHealthProf %>" Value=""></asp:ListItem>
                                                        </asp:DropDownList>
                                                    </td>
                                                    <td>
                                                        <asp:Image ID="imgRegPracticeHealthProfAlert" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                                            runat="server" ImageUrl="~/Images/others/icon_caution.gif" style="vertical-align:top" Visible="False" /></td>
                                                </tr>
                                            </table>
                                            <asp:HiddenField ID="hfRegPracticeHealthProf" runat="server" Value='<%# Bind("ServiceCategoryCode") %>' />
                                        </td>
                                    </tr>
                                    <tr valign="top">
                                        <td style="background-color: #f5f5f5;" valign="top" colspan="2">
                                            <asp:Label ID="lblRegPracticeRegCode" runat="server" Text="<%$ Resources:Text, RegCode %>"></asp:Label>
                                        </td>
                                        <td style="width: 250px">
                                            <asp:TextBox ID="txtRegPracticeRegCode" runat="server" Text='<%# Bind("RegistrationCode") %>'  onblur="Upper(event,this)"
                                                Width="120px" MaxLength="15" OnTextChanged="txtRegPracticeRegCode_TextChanged" AutoPostBack="true" style="vertical-align:top"></asp:TextBox>
                                            <asp:Image ID="imgRegPracticeRegCodeAlert" AlternateText="<%$ Resources:AlternateText, ErrorImg %>"
                                                runat="server" ImageUrl="~/Images/others/icon_caution.gif" style="vertical-align:top" Visible="false" /><br />
                                            (<asp:Label ID="lblRegPracticeRegCodeStatement" runat="server" Text="<%$ Resources:Text, ProfRegNoStatement %>"></asp:Label>)
                                        </td>
                                        <td colspan="2" style="background-color: #f5f5f5; width: 300px;" valign="top">
                                            <asp:Label ID="lblRegPracticeTelText" runat="server" Text="<%$ Resources:Text, PracticeTel %>"></asp:Label></td>
                                        <td valign="top">
                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 100px">
                                                        <asp:TextBox ID="txtRegPracticeTel" runat="server" MaxLength="20" Text='<%# Bind("PhoneDaytime") %>'
                                                            Width="120px"></asp:TextBox></td>
                                                    <td style="width: 100px">
                                                        <asp:Image ID="imgRegPracticeTelAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #f5f5f5;" valign="top" colspan="2">
                                            <asp:Label ID="lblRegPracticeSchemeToEnrollText" runat="server" Text="<%$ Resources:Text, SchemeToEnroll %>"></asp:Label>
                                        </td>
                                        <td width="750px" colspan="4" valign="top">
                                            <span id="cblSchemeList" runat="server"/>
                                    
<%--                                            <table border="0" cellpadding="0" cellspacing="0">
                                                <tr>
                                                    <td style="width: 100px">
                                                        <asp:TextBox ID="TextBox2" runat="server" MaxLength="20" Text=''
                                                            Width="120px"></asp:TextBox></td>
                                                    <td style="width: 100px">
                                                        <asp:Image ID="imgSchemeAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                            Visible="False" AlternateText="<%$ Resources:AlternateText, ErrorImg %>" /></td>
                                                </tr>
                                            </table>--%>
                                        </td>
                                    </tr>
                            </table>
                            <cc1:FilteredTextBoxExtender ID="FilteredRegPracticeRoom" runat="server" TargetControlID="txtRegPracticeRoom"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:FilteredTextBoxExtender ID="FilteredRegPracticeFloor" runat="server" TargetControlID="txtRegPracticeFloor"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:FilteredTextBoxExtender ID="FilteredRegPracticeBlock" runat="server" TargetControlID="txtRegPracticeBlock"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                            </cc1:FilteredTextBoxExtender>

                            <cc1:FilteredTextBoxExtender ID="FilteredPracticeBuilding" runat="server" TargetControlID="txtRegPracticeEAddress"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxPracticeName" runat="server" TargetControlID="txtRegPracticeName"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxRegCode" runat="server" TargetControlID="txtRegPracticeRegCode"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                            </cc1:FilteredTextBoxExtender>
                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxPracticeTel" runat="server" TargetControlID="txtRegPracticeTel"
                                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
                            </cc1:FilteredTextBoxExtender>
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" />
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="ibtnRegPracticeDelete" runat="server" AlternateText="<%$ Resources:AlternateText, DeleteSBtn %>"
                                CausesValidation="False" CommandName="Delete" ImageUrl="<%$ Resources:ImageUrl, DeleteSBtn %>" />
                        </ItemTemplate>
                        <ItemStyle VerticalAlign="Top" Width="80px" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
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
                        <asp:Label ID="lblPracticePaging" runat="server"></asp:Label></td>
                    <td align="right">
                        <asp:ImageButton ID="ibtnRegPracticeAdd" runat="server" ImageUrl="<%$ Resources:ImageUrl, AddSBtn %>"
                            AlternateText="<%$ Resources:AlternateText, AddSBtn %>" OnClick="ibtnRegPracticeAdd_Click" /></td>
                </tr>
            </table>
            <table style="width: 100%">
                <tr>
                    <td align="center">
                        <asp:ImageButton ID="ibtnRegPracticeBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnRegPracticeBack_Click" />
                        <asp:ImageButton ID="ibtnRegPracticeNext" runat="server" AlternateText="<%$ Resources:AlternateText, NextBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" OnClick="ibtnRegPracticeNext_Click" /></td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:Label ID="lblPracticeSkip" runat="server" Text='<%$ Resources:Text, SkipStatement  %>' Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>
            <asp:TextBox ID="txtCopyImageUrl" runat="server" Style="display: none"></asp:TextBox>
            <asp:TextBox ID="txtCopyDisableImageUrl" runat="server" Style="display: none"></asp:TextBox>
            <asp:Button runat="server" ID="btnHiddenShowCopyList" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupExtenderCopyList" runat="server" TargetControlID="btnHiddenShowCopyList"
                PopupControlID="panCopyList" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None">
            </cc1:ModalPopupExtender>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
