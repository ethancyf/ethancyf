<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="ConfirmDetails.aspx.vb" Inherits="eForm.ConfirmDetails" Title="<%$ Resources:Title, eForm %>" %>
    
<%@ Register Src="./UIControl/PCDIntegration/ucTypeOfPracticeGrid.ascx" TagName="ucTypeOfPracticeGrid" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<style type="text/css">
    .confirmDetails td { 
            border: 1px solid #AAAAAA;
            font-size: 16px;
            color: #4D4D4D;
            font-family: Arial, 新細明體, 細明體;
            font-weight: bold;            
     }
</style>

    <script type="text/javascript" src="JS/Common.js"></script>

    <script type="text/javascript" language="javascript">
         function chkChanged()
    {
        var chk = document.getElementById('<%=chkConfirmDetails.ClientID%>');
        var chkPCD = document.getElementById('<%=chkConfirmPCDConditions.ClientID%>');
        var ibtn = document.getElementById('<%=ibtnConfirm.ClientID %>');
        
        if (chk.checked)
        {   
            ibtn.disabled=false;
            ibtn.src = document.getElementById('<%=txtConfirmImageUrl.ClientID%>').value.replace(/~/, ".");
        }
        else
        {
            ibtn.disabled=true;
            ibtn.src = document.getElementById('<%=txtConfirmDisableImageUrl.ClientID%>').value.replace(/~/, ".");
        }
        
        if (chkPCD != null) {
            if (chkPCD.style.display!='none' && !chkPCD.checked)
            {
                ibtn.disabled=true;
                ibtn.src = document.getElementById('<%=txtConfirmDisableImageUrl.ClientID%>').value.replace(/~/, ".");            
            }
        }
    }
    </script>

    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, eFormSpEnrolmentBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, eFormSpEnrolmentBanner %>" />
    <table border="0" cellpadding="0" cellspacing="0" width="600">
        <tr>
            <td>
                <asp:Panel ID="panStep1" runat="server" CssClass="unhighlightTimeline">
                    <asp:Label ID="lblStep1" Text="<%$ Resources:Text, eFormStep1 %>" runat="server"></asp:Label></asp:Panel>
            </td>
            <td>
                <asp:Panel ID="panStep2" runat="server" CssClass="highlightTimeline">
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
                <asp:Panel ID="panStep6" runat="server" CssClass="highlightTimeline">
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
            <table width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="panEditPersonalTop" runat="server">
                            <asp:Image ID="imgEditPersonalParticularsTop" runat="server" ImageUrl="~/Images/others/page_edit.png"
                                ImageAlign="AbsMiddle" />
                            <asp:LinkButton ID="lnkEditPersonalParticularsTop" runat="server" Text="<%$ Resources:Text, EditPersonalParticulars %>"
                                CssClass="editLink" OnClick="lnkEditPersonalParticulars_Click"></asp:LinkButton></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panEditMOTop" runat="server">
                            <asp:Image ID="imgEditMOTop" runat="server" ImageUrl="~/Images/others/page_edit.png"
                                ImageAlign="AbsMiddle" />
                            <asp:LinkButton ID="lnkEditMOTop" runat="server" Text="<%$ Resources:Text, EditMOInfo %>"
                                CssClass="editLink" OnClick="lnkEditMO_Click"></asp:LinkButton></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panEditPracticeTop" runat="server">
                            <asp:Image ID="imgEditPracticeTop" runat="server" ImageUrl="~/Images/others/page_edit.png"
                                ImageAlign="AbsMiddle" />
                            <asp:LinkButton ID="lnkEditPracitceTop" runat="server" Text="<%$ Resources:Text, EditPracticeInfo %>"
                                CssClass="editLink" OnClick="lnkEditPracitce_Click"></asp:LinkButton></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panEditBankTop" runat="server">
                            <asp:Image ID="imgEditBankTop" runat="server" ImageUrl="~/Images/others/page_edit.png"
                                ImageAlign="AbsMiddle" />
                            <asp:LinkButton ID="lnkEditBankTop" runat="server" Text="<%$ Resources:Text, EditBankInfo %>"
                                CssClass="editLink" OnClick="lnkEditBank_Click"></asp:LinkButton></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panEditSchemeTop" runat="server">
                            <asp:Image ID="imgEditSchemeTop" runat="server" ImageUrl="~/Images/others/page_edit.png"
                                ImageAlign="AbsMiddle" />
                            <asp:LinkButton ID="lnkEditSchemeTop" runat="server" Text="<%$ Resources:Text, EditSchemeInfo %>"
                                CssClass="editLink" OnClick="lnkEditScheme_Click"></asp:LinkButton></asp:Panel>
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <div class="headingText">
                            <asp:Label ID="lblPersonalHeadingText" runat="server" Text="<%$ Resources:Text, PersonalParticulars %>"></asp:Label></div>
                    </td>
                </tr>
            </table>
            <table>
                <tr>
                    <td colspan="2" valign="top">
                        <asp:Label ID="lblConfirmNameText" runat="server" Text="<%$ Resources:Text, Name %>"></asp:Label></td>
                    <td valign="top">
                        <asp:Label ID="lblConfirmEname" runat="server" CssClass="tableText"></asp:Label>
                        <asp:Label ID="lblConfirmCname" runat="server" CssClass="TextChi"></asp:Label></td>
                </tr>
                <tr>
                    <td style="width: 200px;" colspan="2" valign="top">
                        <asp:Label ID="lblConfirmHKIDText" runat="server" Text="<%$ Resources:Text, HKID %>"></asp:Label></td>
                    <td valign="top">
                        <asp:Label ID="lblConfirmHKID" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 200px" valign="top">
                        <asp:Label ID="lblConfirmAddressText" runat="server" Text="<%$ Resources:Text, SPAddress %>"></asp:Label>
                    </td>
                    <td valign="top">
                        <asp:Label ID="lblConfirmAddress" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 200px" valign="top">
                        <asp:Label ID="lblConfirmEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                    <td valign="top">
                        <asp:Label ID="lblConfirmEmail" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 200px" valign="top">
                        <asp:Label ID="lblConfirmContactNoText" runat="server" Text="<%$ Resources:Text, ContactNo %>"></asp:Label></td>
                    <td valign="top">
                        <asp:Label ID="lblConfirmContactNo" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 200px" valign="top">
                        <asp:Label ID="lblConfirmFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                    <td valign="top">
                        <asp:Label ID="lblConfirmFax" runat="server" CssClass="tableText"></asp:Label></td>
                </tr>
                <tr>
                    <td colspan="2" style="width: 200px" valign="top">
                        <asp:Label ID="lblConfirmSchemeText" runat="server" Text="<%$ Resources:Text, SelectedScheme %>"></asp:Label></td>
                    <td valign="top">
                        <asp:Panel ID="pnlConfirmScheme" CssClass="tableText" runat="server">
                        </asp:Panel>
                        <asp:Label ID="lblConfirmScheme" runat="server" CssClass="tableText" Visible="false"></asp:Label>
                    </td>
                </tr>
            </table>

            <asp:Panel ID="panEditMO" runat="server">
                <br />
                <table width="100%">
                    <tr>
                        <td>
                            <div class="headingText">
                                <asp:Label ID="lblMOHeadingText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label></div>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvMO" runat="server" AutoGenerateColumns="False" ShowHeader="True"
                    Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="85px" HeaderText="<%$ Resources:Text, MONo %>">
                            <ItemTemplate>
                                <asp:Label ID="lblMOIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, MedicalOrganizationInfo %>">
                            <ItemTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblRegBankMOENameText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationName %>"></asp:Label></td>
                                        <td valign="top">
                                            <asp:Label ID="lblMOEName" runat="server" Text='<%# Bind("MOEName") %>' CssClass="tableText"></asp:Label><br />
                                            <asp:Label ID="lblMOCName" runat="server" Text='<%# formatChineseString(Eval("MOCName")) %>'
                                                CssClass="TextChi"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblMOBRCodeText" runat="server" Text="<%$ Resources:Text, BrCode %>"></asp:Label></td>
                                        <td  valign="top">
                                            <asp:Label ID="lblMOBRCode" runat="server" Text='<%# Bind("MOBRCode") %>' CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblMOContactNoText" runat="server" Text="<%$ Resources:Text, MOContactNo %>"></asp:Label></td>
                                        <td valign="top">
                                            <asp:Label ID="lblMOContactNo" runat="server" Text='<%# Bind("MOContactNo") %>' CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblMOEmailText" runat="server" Text="<%$ Resources:Text, Email %>"></asp:Label></td>
                                        <td valign="top">
                                            <asp:Label ID="lblMOEmail" runat="server" Text='<%# Bind("MOEmail") %>' CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblMOFaxText" runat="server" Text="<%$ Resources:Text, FaxNo %>"></asp:Label></td>
                                        <td valign="top">
                                            <asp:Label ID="lblMOFax" runat="server" Text='<%# Bind("MOFax") %>' CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblMOAddressText" runat="server" Text="<%$ Resources:Text, MOAddress %>"></asp:Label></td>
                                        <td  valign="top">
                                            <asp:Label ID="lblMOAddress" runat="server" Text='<%# formatAddress(Eval("MORoom"), Eval("MOFloor"), Eval("MOBlock"), Eval("MOEAddress"), Eval("MODistrict")) %>'
                                                CssClass="tableText"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblMORelationText" runat="server" Text="<%$ Resources:Text, MedicalOrganizationRelationship %>"></asp:Label></td>
                                        <td  valign="top">
                                            <asp:Label ID="lblMORelation" runat="server" Text='<%# GetPracticeTypeName(Eval("MORelation")) %>'
                                                CssClass="tableText"></asp:Label>
                                            <asp:Label ID="lblMORelationRemark" runat="server" Text='<%# formatChineseString(Eval("MORelationRemarks")) %>'
                                                CssClass="TextChi"></asp:Label></td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <asp:Panel ID="panEditPracticeBank" runat="server">
                <br />
                <table width="100%">
                    <tr>
                        <td>
                            <div class="headingText">
                                <asp:Label ID="lblPracticeHeadingText" runat="server" Text="<%$ Resources:Text, Practice %>"></asp:Label></div>
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvPractice" runat="server" AutoGenerateColumns="False" Width="100%">
                    <Columns>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeNo %>">
                            <ItemTemplate>
                                <asp:Label ID="lblPracticeIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle VerticalAlign="Top" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeInfo %>">
                            <ItemTemplate>
                                <table width="100%">
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblPracticeMOText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label></td>
                                        <td valign="top">
                                            <asp:Label ID="lblPracticeMO" runat="server" Text='<%# Bind("MOIndex") %>' CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblPracticeENameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                        <td  valign="top">
                                            <asp:Label ID="lblPracticeEName" runat="server" Text='<%# Bind("PracticeName") %>'
                                                CssClass="tableText"></asp:Label><br />
                                            <asp:Label ID="lblPracticeCName" runat="server" Text='<%# formatChineseString(Eval("PracticeNameChi")) %>'
                                                CssClass="TextChi"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                        <td  valign="top">
                                            <asp:Label ID="lblPracticeAddress" runat="server" Text='<%# formatAddress(Eval("Room"), Eval("Floor"), Eval("Block"), Eval("Building"), Eval("District")) %>'
                                                CssClass="tableText"></asp:Label><br />
                                            <asp:Label ID="lblPracticeAddressChi" runat="server" Text='<%# formatChineseString(formatChiAddress(Eval("Room"), Eval("Floor"), Eval("Block"), Eval("ChiBuilding"), Eval("District"))) %>'
                                                CssClass="TextChi"></asp:Label></td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblPracticeTelText" runat="server" Text="<%$ Resources:Text, PracticeTel %>"></asp:Label></td>
                                        <td  valign="top">
                                            <asp:Label ID="lblPracticeTel" runat="server" Text='<%# Bind("PhoneDaytime") %>'
                                                CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblHealthProfText" runat="server" Text="<%$ Resources:Text, HealthProf %>"></asp:Label></td>
                                        <td  valign="top">
                                            <asp:Label ID="lblHealthProf" runat="server" Text='<%# GetHealthProfName(Eval("ServiceCategoryCode")) %>'
                                                CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblRegCodeText" runat="server" Text="<%$ Resources:Text, RegCode %>"></asp:Label></td>
                                        <td  valign="top">
                                            <asp:Label ID="lblRegCode" runat="server" Text='<%# Bind("RegistrationCode") %>'
                                                CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblSchemeToEnrollText" runat="server" Text="<%$ Resources:Text, SchemeToEnroll %>"></asp:Label>
                                        </td>
                                        <td>                                                       
                                            <asp:GridView ID="gvSchemeToEnroll" runat="server" AutoGenerateColumns="false" SkinID="GridViewSchemeToEnrollSkin"
                                                 OnRowCreated ="gvSchemeToEnroll_RowCreated"
                                                    ShowHeader="false" >
                                                <Columns>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:label ID="lblSchemeToEnroll" runat="server" Text='<%# Bind("Info")%>' CssClass="tableText" Style="position:relative;left:15px"></asp:label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="left" VerticalAlign="Top"/>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr runat="server" id="trServiceFee">
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblServiceFeeText" runat="server" Text="<%$ Resources:Text, ServiceFee %>"></asp:Label></td>
                                        <td valign="top">
                                            <%--<asp:Label ID="lblVSSText" runat="server" Text="" CssClass="tableText" Style="padding-bottom:1px"></asp:Label>
                                            <br>
                                            <div style="padding-left:20px">
                                                <asp:Panel ID="pnlServiceFee" runat="server"></asp:Panel>
                                            </div>--%>
                                            <asp:Panel ID="pnlServiceFee" runat="server"></asp:Panel>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <hr style="width:100%;height:1px;border:none;background-color:#989898;margin-top:2px;margin-bottom:2px">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Label ID="lblBankTitle" CssClass="tableText" runat="server" Text="<%$ Resources:Text, BankInfo %>"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblBankNameText" runat="server" Text="<%$ Resources:Text, BankName %>"></asp:Label></td>
                                        <td valign="top">
                                            <asp:Label ID="lblBankName" runat="server" Text='<%# Bind("Bank") %>' CssClass="TextChi"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblBranchNameText" runat="server" Text="<%$ Resources:Text, BranchName %>"></asp:Label></td>
                                        <td valign="top">
                                            <asp:Label ID="lblBranchName" runat="server" Text='<%# Bind("Branch") %>' CssClass="TextChi"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblBankAccText" runat="server" Text="<%$ Resources:Text, BankAccountNo %>"></asp:Label></td>
                                        <td  valign="top">
                                            <asp:Label ID="lblBankAcc" runat="server" Text='<%# formatBankAcct(Eval("BankCode"), Eval("BranchCode"), Eval("BankAcc")) %>'
                                                CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                            <asp:Label ID="lblBankOwnerText" runat="server" Text="<%$ Resources:Text, BankOwner %>"></asp:Label></td>
                                        <td valign="top">
                                            <asp:Label ID="lblBankOwner" runat="server" Text='<%# Bind("Holder") %>' CssClass="tableText"></asp:Label>
                                        </td>
                                    </tr>
                                </table>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
            <br />
            <asp:Panel ID="panEHRSS" runat="server">
                <asp:Panel ID="panEHRSSHeader" runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="headingTextOtherSystem">
                                    <asp:Label ID="lblEHRSSText" runat="server" Text="<%$ Resources:Text, EHRSS %>"></asp:Label></div>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <asp:Panel ID="panHadJoinEHRSS" runat="server">
                    <table>
                        <tr>
                            <td>
                                <asp:Label ID="lblHadJoinEHRSSText" runat="server" Text="<%$ Resources:Text, HadJoinedEHRSSQ %>"></asp:Label>
                                &nbsp; &nbsp;<asp:Label ID="lblHadJoinEHRSS" runat="server" CssClass="tableText"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </asp:Panel>
            
            <asp:Panel ID="panPCD" runat="server">
                <asp:Panel ID="panPCDHeader" runat="server">
                    <table>
                        <tr>
                            <td>
                                <div class="headingTextOtherSystem">
                                    <asp:Label ID="lblPCDText" runat="server" Text="<%$ Resources:Text, PCD %>"></asp:Label></div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblWillJoinPCDText" runat="server" Text="<%$ Resources:Text, WillJoinPCD %>"></asp:Label>
                                &nbsp; &nbsp;
                                <asp:Label ID="lblWillJoinPCD" runat="server" CssClass="tableText"></asp:Label></td>
                        </tr>
                    </table>
                </asp:Panel>      
                <asp:Panel ID="panSelectPracticeType" runat="server" Visible="True">
                    <uc1:ucTypeOfPracticeGrid ID="ucTypeOfPracticeGrid" runat="server" />
                    <asp:GridView ID="gvPracticeType" runat="server" AutoGenerateColumns="False" Width="100%" Visible="false">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeNo %>">
                                <ItemTemplate>
                                    <asp:Label ID="lblPracticeIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" Width="100px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, PracticeInfo %>">
                                <ItemTemplate>
                                    <table width="100%">
                                        <tr>
                                            <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                                <asp:Label ID="lblPracticeMOText" runat="server" Text="<%$ Resources:Text, MedicalOrganization %>"></asp:Label></td>
                                            <td valign="top">
                                                <asp:Label ID="lblPracticeMO" runat="server" Text='<%# Bind("MOIndex") %>' CssClass="tableText"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                                <asp:Label ID="lblPracticeENameText" runat="server" Text="<%$ Resources:Text, PracticeName %>"></asp:Label></td>
                                            <td valign="top">
                                                <asp:Label ID="lblPracticeEName" runat="server" Text='<%# Bind("PracticeName") %>'
                                                    CssClass="tableText"></asp:Label><br />
                                                <asp:Label ID="lblPracticeCName" runat="server" Text='<%# formatChineseString(Eval("PracticeNameChi")) %>'
                                                    CssClass="TextChi"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td style="width: 200px; background-color: #f5f5f5;" valign="top">
                                                <asp:Label ID="lblPracticeAddressText" runat="server" Text="<%$ Resources:Text, PracticeAddress %>"></asp:Label></td>
                                            <td valign="top">
                                                <asp:Label ID="lblPracticeAddress" runat="server" Text='<%# formatAddress(Eval("Room"), Eval("Floor"), Eval("Block"), Eval("Building"), Eval("District")) %>'
                                                    CssClass="tableText"></asp:Label><br />
                                                <asp:Label ID="lblPracticeAddressChi" runat="server" Text='<%# formatChineseString(formatChiAddress(Eval("Room"), Eval("Floor"), Eval("Block"), Eval("ChiBuilding"), Eval("District"))) %>'
                                                    CssClass="TextChi"></asp:Label></td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <HeaderTemplate>
                                    <asp:Label ID="lblPracticeTypeTitle" runat="server" Text="<%$ Resources:Text, PracticeType %>"></asp:Label>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:Label ID="lblPracticeType" runat="server" CssClass="tableText"></asp:Label>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" Width="300px" HorizontalAlign="left" />
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </asp:Panel>          
            </asp:Panel>         

            <table style="width: 100%">
                <tr style="height:10px;">
                    <td>
                    
                    </td>
                </tr>
                <tr>
                    <td align="left" style="padding-left:20px">
                        <asp:CheckBox ID="chkConfirmPCDConditions" runat="server" Text="<%$ Resources:Text, eFormConfirmPCDConditions %>"
                            CssClass="rbChoice" onclick="chkChanged()" />
                    </td>
                </tr>            
                <tr style="height:10px">
                    <td align="left" style="padding-left:20px">
                        <asp:CheckBox ID="chkConfirmDetails" runat="server" Text="<%$ Resources:Text, eFormConfirmDetails %>"
                            CssClass="rbChoice" onclick="chkChanged()" />
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <asp:ImageButton ID="ibtnConfirmBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="ibtnConfirmBack_Click" />
                        <asp:ImageButton ID="ibtnConfirm" runat="server" AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>"
                            ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>" Enabled="false" OnClick="ibtnConfirm_Click" />
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr>
                    <td>
                        <asp:Panel ID="panEditPersonalBottom" runat="server">
                            <asp:Image ID="imgEditPersonalParticularsBottom" runat="server" ImageUrl="~/Images/others/page_edit.png"
                                ImageAlign="AbsMiddle" />
                            <asp:LinkButton ID="lnkEditPersonalParticularsBottom" runat="server" Text="<%$ Resources:Text, EditPersonalParticulars %>"
                                CssClass="editLink" OnClick="lnkEditPersonalParticulars_Click"></asp:LinkButton></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panEditMOBottom" runat="server">
                            <asp:Image ID="imgEditMOBottom" runat="server" ImageUrl="~/Images/others/page_edit.png"
                                ImageAlign="AbsMiddle" />
                            <asp:LinkButton ID="lnkEditMOBottom" runat="server" Text="<%$ Resources:Text, EditMOInfo %>"
                                OnClick="lnkEditMO_Click" CssClass="editLink"></asp:LinkButton></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panEditPracticeBottom" runat="server">
                            <asp:Image ID="imgEditPracticeBottom" runat="server" ImageUrl="~/Images/others/page_edit.png"
                                ImageAlign="AbsMiddle" />
                            <asp:LinkButton ID="lnkEditPracitceBottom" runat="server" Text="<%$ Resources:Text, EditPracticeInfo %>"
                                CssClass="editLink" OnClick="lnkEditPracitce_Click"></asp:LinkButton></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panEditBankBottom" runat="server">
                            <asp:Image ID="imgEditBankBottom" runat="server" ImageUrl="~/Images/others/page_edit.png"
                                ImageAlign="AbsMiddle" />
                            <asp:LinkButton ID="lnkEditBankBottom" runat="server" Text="<%$ Resources:Text, EditBankInfo %>"
                                CssClass="editLink" OnClick="lnkEditBank_Click"></asp:LinkButton></asp:Panel>
                    </td>
                    <td>
                        <asp:Panel ID="panEditSchemeBottom" runat="server">
                            <asp:Image ID="imgEditSchemeBottom" runat="server" ImageUrl="~/Images/others/page_edit.png"
                                ImageAlign="AbsMiddle" />
                            <asp:LinkButton ID="lnkEditSchemeBottom" runat="server" Text="<%$ Resources:Text, EditSelectionOfScheme %>"
                                CssClass="editLink" OnClick="lnkEditScheme_Click"></asp:LinkButton></asp:Panel>
                    </td>
                </tr>
            </table>
            <asp:TextBox ID="txtConfirmImageUrl" runat="server" Style="display: none"></asp:TextBox>
            <asp:TextBox ID="txtConfirmDisableImageUrl" runat="server" Style="display: none"></asp:TextBox>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
