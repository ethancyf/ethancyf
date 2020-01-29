<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="PersonalPacticulars.aspx.vb" Inherits="eForm.PersonalPacticulars"
    Title="<%$ Resources:Title, eForm %>" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="JS/Common.js"></script>

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
            <cc2:MessageBox ID="udtMsgBox" runat="server" Width="95%" />
            <br />
            <table border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td>
                        <asp:Panel ID="panPersonalParticulars" runat="server" CssClass="highlightTab" Width="180px"
                            Height="35px">
                            <asp:Label ID="lblTabPersonal" Text="<%$ Resources:Text, PersonalParticulars %>"
                                runat="server"></asp:Label></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panMO" runat="server" CssClass="unhighlightUnclickedTab" Width="180px" Height="35px">
                            <asp:Label ID="lblTabMO" Text="<%$ Resources:Text, MedicalOrganizationInfo %>" runat="server"></asp:Label></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panPractice" runat="server" CssClass="unhighlightUnclickedTab" Width="180px"
                            Height="35px">
                            <asp:Label ID="lblTabPractice" Text="<%$ Resources:Text, PracticeInfo %>" runat="server"></asp:Label></asp:Panel>
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
            <br />
            <table>
                <tr>
                    <td colspan="1" style="width: 50px" valign="top">
                        <asp:Label ID="lblRegNameText" runat="server" Text="<%$ Resources:Text, Name %>"
                            CssClass="tableTitle"></asp:Label></td>
                    <td style="width: 150px" valign="top" colspan="2">
                        (<asp:Label ID="lblRegEngNameText" runat="server" Text="<%$ Resources:Text, InEnglish %>"
                            CssClass="tableTitle"></asp:Label>)</td>
                    <td valign="top" align="left" style="width: 300px">
                        <asp:TextBox ID="txtRegSurname" runat="server" Width="100px" onblur="Upper(event,this)"
                            MaxLength="40"></asp:TextBox>,
                        <asp:TextBox ID="txtRegEname" runat="server" onblur="Upper(event,this);ltrim(this);"
                            MaxLength="40"></asp:TextBox>
                        <asp:Image ID="imgEnameAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                    </td>
                    <td align="left" rowspan="2" style="width: 400px" valign="top">
                        <asp:Label ID="lblRegENameTip" runat="server" Text="<%$ Resources:Text, NameHint %>"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="1" style="width: 50px" valign="top">
                    </td>
                    <td style="width: 150px;" valign="top" colspan="2">
                        (<asp:Label ID="lblRegChineseNameText" runat="server" Text="<%$ Resources:Text, InChinese %>"
                            CssClass="tableTitle"></asp:Label>)</td>
                    <td valign="top" align="left">
                        <asp:TextBox ID="txtRegCname" runat="server" MaxLength="6" Width="80px" CssClass="TextBoxChi"></asp:TextBox>
                        <asp:Label ID="lblRegCnameOptional" runat="server" ForeColor="#FF8080" Text="<%$ Resources:Text, optionalField %>"
                            Visible="False"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Label ID="lblRegHKICText" runat="server" Text="<%$ Resources:Text, HKID %>"
                            CssClass="tableTitle"></asp:Label></td>
                    <td align="left" valign="top">
                        <asp:TextBox ID="txtRegHKIDPrefix" runat="server" MaxLength="2" Width="28px" onblur="Upper(event,this)"></asp:TextBox>
                        <asp:TextBox ID="txtRegHKID" runat="server" Width="50px" MaxLength="6"></asp:TextBox>
                        (
                        <asp:TextBox ID="txtRegHKIDdigit" runat="server" MaxLength="1" Width="14px" onblur="Upper(event,this)"></asp:TextBox>
                        )
                        <asp:Image ID="imgHKIdAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                    <td align="left" rowspan="1" style="width: 400px" valign="top">
                        <asp:Label ID="lblRegHKIDTip" runat="server" Text="<%$ Resources:Text, HKIDHint %>"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Label ID="lblRegAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"
                            CssClass="tableTitle"></asp:Label></td>
                    <td align="left" colspan="2" rowspan="1" valign="top">
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 60%">
                            <tr>
                                <td>
                                    <asp:Label ID="lblRegRoomText" runat="server" Text="<%$ Resources:Text, Room %>"
                                        CssClass="tableTitle"></asp:Label>
                                    &nbsp; &nbsp;
                                    <asp:TextBox ID="txtRegRoom" runat="server" Width="50px" MaxLength="5"></asp:TextBox></td>
                                <td>
                                    <asp:Label ID="lblRegFloorText" runat="server" Text="<%$ Resources:Text, Floor %>"
                                        CssClass="tableTitle"></asp:Label>
                                    &nbsp;&nbsp; &nbsp;<asp:TextBox ID="txtRegFloor" runat="server" Width="50px" MaxLength="3"></asp:TextBox></td>
                                <td>
                                    <asp:Label ID="lblRegBlockText" runat="server" Text="<%$ Resources:Text, Block %>"
                                        CssClass="tableTitle"></asp:Label>
                                    &nbsp;&nbsp; &nbsp;<asp:TextBox ID="txtRegBlock" runat="server" Width="50px" MaxLength="3"></asp:TextBox></td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="1" style="width: 50px" valign="top">
                    </td>
                    <td colspan="2" style="width: 150px" valign="top">
                        (<asp:Label ID="lblRegEAddressText" runat="server" Text="<%$ Resources:Text, InEnglish %>"
                            CssClass="tableTitle"></asp:Label>)</td>
                    <td align="left" colspan="2" valign="top">
                        <asp:TextBox ID="txtRegEAddress" runat="server" Width="500px" MaxLength="100"></asp:TextBox>
                        <asp:Image ID="imgEAddressAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                    </td>
                    <td align="left" colspan="2" valign="top">
                        <table border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td style="width: 80px">
                                    <asp:Label ID="lblRegDistrictText" runat="server" Text="<%$ Resources:Text, District %>"
                                        CssClass="tableTitle"></asp:Label></td>
                                <td colspan="5">
                                    <table border="0" cellpadding="0" cellspacing="0">
                                        <tr>
                                            <td>
                                                <asp:DropDownList ID="ddlRegDistrict" runat="server" AutoPostBack="false" AppendDataBoundItems="true">
                                                    <asp:ListItem Text="<%$ Resources:Text, SelectDistrict %>"></asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td>
                                                <asp:Image ID="imgDistrictAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                                                    AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Label ID="lblRegEmailText" runat="server" Text="<%$ Resources:Text, Email %>"
                            CssClass="tableTitle"></asp:Label></td>
                    <td align="left" valign="top">
                        <asp:TextBox ID="txtRegEmail" runat="server" MaxLength="255" Width="200px"></asp:TextBox>
                        <asp:Image ID="imgEmailAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                    <td align="left" rowspan="1" style="width: 400px" valign="top">
                        <asp:Label ID="lblRegEmailHint" runat="server" Text="<%$ Resources:Text, EmailHint %>"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Label ID="lblRegConfirmEmailText" runat="server" Text="<%$ Resources:Text, ConfirmEmail %>"></asp:Label></td>
                    <td align="left" valign="top" colspan="2">
                        <asp:TextBox ID="txtRegConfirmEmail" runat="server" MaxLength="255" Width="200px"
                            onkeydown="disableCopyPaste()"></asp:TextBox>
                        <asp:Image ID="imgConfirmEmailAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" /></td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Label ID="lblRegContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"
                            CssClass="tableTitle"></asp:Label></td>
                    <td valign="top" align="left" colspan="2">
                        <asp:TextBox ID="txtRegContactNo" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
                        <asp:Image ID="imgContactNoAlert" runat="server" ImageUrl="~/Images/others/icon_caution.gif"
                            AlternateText="<%$ Resources:AlternateText, ErrorImg %>" Visible="False" />
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:Label ID="lblRegFaxNoText" runat="server" Text="<%$ Resources:Text, FaxNo %>"
                            CssClass="tableTitle"></asp:Label></td>
                    <td valign="top" align="left" colspan="2">
                        <asp:TextBox ID="txtRegFaxNo" runat="server" MaxLength="20" Width="80px"></asp:TextBox>
                        <asp:Label ID="lblRegFaxNoOptional" runat="server" ForeColor="#FF8080" Text="<%$ Resources:Text, optionalField %>"></asp:Label>
                    </td>
                </tr>
            </table>
            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxRegSurname" runat="server" TargetControlID="txtRegSurname"
                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
            </cc1:FilteredTextBoxExtender>
            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxRegOthername" runat="server" TargetControlID="txtRegEname"
                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
            </cc1:FilteredTextBoxExtender>
            <cc1:FilteredTextBoxExtender ID="FilteredHKIDPrefix" runat="server" TargetControlID="txtRegHKIDPrefix"
                FilterType="Custom, LowercaseLetters, UppercaseLetters">
            </cc1:FilteredTextBoxExtender>
            <cc1:FilteredTextBoxExtender ID="FilteredHKID" runat="server" TargetControlID="txtRegHKID"
                FilterType="Custom, Numbers">
            </cc1:FilteredTextBoxExtender>
            <cc1:FilteredTextBoxExtender ID="FilteredHKIDdigit" runat="server" TargetControlID="txtRegHKIDdigit"
                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers">
            </cc1:FilteredTextBoxExtender>

            <cc1:FilteredTextBoxExtender ID="FilteredRegRoom" runat="server" TargetControlID="txtRegRoom"
                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
            </cc1:FilteredTextBoxExtender>
            <cc1:FilteredTextBoxExtender ID="FilteredRegFloor" runat="server" TargetControlID="txtRegFloor"
                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
            </cc1:FilteredTextBoxExtender>
            <cc1:FilteredTextBoxExtender ID="FilteredRegBlock" runat="server" TargetControlID="txtRegBlock"
                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
            </cc1:FilteredTextBoxExtender>

            <cc1:FilteredTextBoxExtender ID="FilteredSPBuilding" runat="server" TargetControlID="txtRegEAddress"
                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
            </cc1:FilteredTextBoxExtender>
            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxRegContactNo" runat="server" TargetControlID="txtRegContactNo"
                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
            </cc1:FilteredTextBoxExtender>
            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxFax" runat="server" TargetControlID="txtRegFaxNo"
                FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;">
            </cc1:FilteredTextBoxExtender>
            <table style="width: 100%">
                <tr>
                    <td align="center">
                        <asp:ImageButton ID="ibtnRegReset" runat="server" AlternateText="<%$ Resources:AlternateText, ClearBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, ClearBtn %>" OnClick="ibtnRegReset_Click" />
                        <asp:ImageButton ID="ibtnRegNext" runat="server" AlternateText="<%$ Resources:AlternateText, NextBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" OnClick="ibtnRegNext_Click" /></td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
