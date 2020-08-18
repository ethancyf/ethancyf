<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master" CodeBehind="InspectionRecordManagement.aspx.vb"
    Inherits="HCVU.InspectionRecordManagement" Title="Inspection Record Management" %>

<%@ Register Src="../UIControl/DocType/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType"
    TagPrefix="uc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/ucCollapsibleSearchCriteriaReview.ascx" TagName="CollapsibleSearchCriteriaReview" TagPrefix="cc3" %>
<%@ Register Src="~/UIControl/Assessories/ucNoticePopUp.ascx" TagName="ucNoticePopUp" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />
    <asp:Image ID="imgHeader" runat="server" AlternateText="<%$ Resources:AlternateText, InspectionRecordManagementBanner %>"
        ImageUrl="<%$ Resources:ImageUrl, InspectionRecordManagementBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <link href="../CSS/CommonStyleInspection.css" rel="Stylesheet" />
            <script type="text/javascript">
                function checkNoOfClaim() {
                    var inOrder = 0;
                    var missingForm = 0;
                    var inconsistent = 0;
                    if (!isNaN(parseInt($('#txtIIRInOrder').val()))) {
                        inOrder = parseInt($('#txtIIRInOrder').val());
                    }
                    if (!isNaN(parseInt($('#txtIIRMissingForm').val()))) {
                        missingForm = parseInt($('#txtIIRMissingForm').val());
                    }
                    if (!isNaN(parseInt($('#txtIIRInconsistent').val()))) {
                        inconsistent = parseInt($('#txtIIRInconsistent').val());
                    }
                    var total = inOrder + missingForm + inconsistent;
                    $('#lblIIRTotalCheck').empty();
                    $('#lblIIRTotalCheck').append(total);
                };
                function HighlightType(obj) {
                    $(obj).find('input').each(function (index, e) {
                        if ($(e).prop("checked")) {
                            $(e).parent().addClass("checkBoxHighlight")
                        }
                        else {
                            $(e).parent().removeClass("checkBoxHighlight")
                        }
                    })
                };
                function HighlightTypeMultiple(obj1,obj2) {
                    HighlightType(document.getElementById(obj1))
                    HighlightType(document.getElementById(obj2))
                };
                function autoFocusNextInput(e, inputBox, orginalID, nextID) {
                    if (!isNaN(e.key)) {
                        if (inputBox.value.length == inputBox.maxLength && inputBox.selectionEnd == inputBox.maxLength && inputBox.selectionStart == inputBox.maxLength) {
                            var nextInputID = inputBox.id.replace(orginalID, nextID)
                            var targetElement = document.getElementById(nextInputID)
                            if (targetElement) {
                                targetElement.focus()
                            }
                        }
                    }
                }
                function OfficerOnKeyDown(e) {
                    if (e.key) {
                        e.target.setAttribute("data-keyboardinput", "true");
                        setTimeout(function () {
                            e.target.removeAttribute("data-keyboardinput")
                        }.bind(e.target), 100)
                    }

                    if ((e.keyCode == 13) && (typeof e.target.form != "undefined")) {
                        // get the element's parent form object
                        // capture the parent form submit event and supress it
                        e.target.form.addEventListener('submit', function (e) {
                            e.preventDefault();
                        }, { "once": true });
                    }
                }
                function OfficerLostFocus(target, method, type) {
                    var options = target.list.options
                    var notFound = true
                    var CaseContactID = "", CaseOfficerID = "", SubjectOfficerID = "", SubjectContactID = ""
                    if (type == 'New') {
                        CaseContactID = "<%=txtCaseContactNo.ClientID%>"
                        CaseOfficerID = "<%=hfCaseOfficer.ClientID%>"
                        SubjectOfficerID = "<%=hfSubjectOfficer.ClientID%>"
                        SubjectContactID = "<%=txtSubjectContactNo.ClientID%>"
                    }
                    else {
                        CaseContactID = "<%=txtEdtCaseContactNo.ClientID%>"
                        CaseOfficerID = "<%=hfEdtCaseOfficer.ClientID%>"
                        SubjectOfficerID = "<%=hfEdtSubjectOfficer.ClientID%>"
                        SubjectContactID = "<%=txtEdtSubjectContactNo.ClientID%>"
                    }
                    for (var i = 0; i < options.length; i++) {

                        if (options[i].value.toUpperCase() === target.value.toUpperCase()) {
                            if (method == "Case") {
                                $('#' + CaseContactID).val(options[i].getAttribute("no"))
                                $('#' + CaseOfficerID).val(options[i].value)
                                $(target).val(options[i].value)
                            }
                            if (method == "Subject") {
                                $('#' + SubjectContactID).val(options[i].getAttribute("no"))
                                $('#' + SubjectOfficerID).val(options[i].value)
                                $(target).val(options[i].value)
                            }
                            notFound = false
                            break
                        }
                    }
                    if (notFound) {
                        $(target).val("")
                        if (method == "Case") {
                            $('#' + CaseOfficerID).val("")
                        }
                        if (method == "Subject") {
                            $('#' + SubjectOfficerID).val("")
                        }
                    }

                }
                function officerOnInput(target, method, type) {
                    if (!target.dataset.keyboardinput) {
                        var options = target.list.options
                        var CaseContactID = "", CaseOfficerID = "", SubjectOfficerID = ""
                        if (type == 'New') {
                            CaseContactID = "<%=txtCaseContactNo.ClientID%>"
                            CaseOfficerID = "<%=hfCaseOfficer.ClientID%>"
                            SubjectOfficerID = "<%=hfSubjectOfficer.ClientID%>"
                            SubjectContactID = "<%=txtSubjectContactNo.ClientID%>"
                        }
                        else {
                            CaseContactID = "<%=txtEdtCaseContactNo.ClientID%>"
                            CaseOfficerID = "<%=hfEdtCaseOfficer.ClientID%>"
                            SubjectOfficerID = "<%=hfEdtSubjectOfficer.ClientID%>"
                            SubjectContactID = "<%=txtEdtSubjectContactNo.ClientID%>"
                        }
                        for (var i = 0; i < options.length; i++) {
                            if (options[i].value === target.value) {
                                if (method == "Case") {
                                    $('#' + CaseContactID).val(options[i].getAttribute("no"))
                                    $('#' + CaseOfficerID).val(options[i].value)
                                }
                                else {
                                    $('#' + SubjectContactID).val(options[i].getAttribute("no"))
                                    $('#' + SubjectOfficerID).val(options[i].value)
                                }

                            }
                        }
                    }
                }

                function clickDownloadButton(e) {

                    if (e.keyCode == 13) {
                        $('#<%= ibtnDownload.ClientID%>').click();
                    }
                }

            </script>
            <asp:Panel ID="panMessageBox" runat="server" Width="950px">
                <cc2:InfoMessageBox ID="udcInfoMsgBox" runat="server" Width="95%" />
                <cc2:MessageBox ID="udcMsgBox" runat="server" Width="95%" />
            </asp:Panel>
            <asp:HiddenField runat="server" ID="hdnStatus" Value="" />
            <asp:HiddenField runat="server" ID="hdnIsRedirect" Value="False" />
            <asp:MultiView ID="MultiViewIRM" runat="server" ActiveViewIndex="0">
                <!--Search-->
                <asp:View ID="vSEARCH" runat="server">
                    <div>
                        <table style="width: 100%">
                            <tr>
                                <td style="width: 200px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, InspectionRecord%>" /></td>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="rdlOwner" RepeatDirection="Horizontal">
                                        <asp:ListItem Text="<%$ Resources: Text, Owner%>" Selected="True" Value="1"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources: Text, Any%>" Value="0"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, InspectionRecordID%>" /></td>
                                <td>
                                    <asp:Literal runat="server" Text="" ID="prefixInspectionRecordID" />
                                    <asp:TextBox ID="txtInspectionID" MaxLength="30" runat="server" Width="195px" CssClass="clsMargin01"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FileReferenceNo%>" /></td>
                                <td>
                                    <asp:Literal runat="server" Text="" ID="prefixFileReferNo" />
                                    <asp:TextBox ID="txtFileReferenceNo" MaxLength="30" runat="server" Width="120px"></asp:TextBox>
                                    <asp:Label runat="server" CssClass="fontBold" Text="<%$ Resources: Text, FileReferenceNoRemind%>" ID="remarkFileReferNo" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" /></td>
                                <td>
                                    <asp:TextBox ID="txtSPID" runat="server" MaxLength="8" Width="223px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, VisitDate%>" /></td>
                                <td>
                                    <asp:TextBox ID="txtStartVisitDate" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="btnStartVisitDate" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                    <cc1:CalendarExtender ID="CalExtStartVisitDate" CssClass="ajax_cal" runat="server" PopupButtonID="btnStartVisitDate"
                                        TargetControlID="txtStartVisitDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                    <cc1:FilteredTextBoxExtender ID="txtStartVisitDateFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtStartVisitDate" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                    <asp:Literal ID="Literal1" runat="server" Text="<%$ Resources: Text, To%>" />
                                    <asp:TextBox ID="txtEndVisitDate" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="btnEndVisitDate" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                    <cc1:CalendarExtender ID="CalExtEndVisitDate" CssClass="ajax_cal" runat="server" PopupButtonID="btnEndVisitDate"
                                        TargetControlID="txtEndVisitDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                    <cc1:FilteredTextBoxExtender ID="txtEndVisitDateFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEndVisitDate" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MainTypeOfInspection%>" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlTypeofInspection" runat="server" Width="226px">
                                    </asp:DropDownList>

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Status%>" /></td>
                                <td>
                                    <asp:DropDownList ID="ddlStatus" runat="server" Width="226px">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                        </table>
                        <cc1:FilteredTextBoxExtender ID="txtSPIDFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtSPID"></cc1:FilteredTextBoxExtender>
                        <cc1:FilteredTextBoxExtender ID="txtInspectionIDFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtInspectionID"></cc1:FilteredTextBoxExtender>
                        <div style="text-align: center; margin-top: 10px">
                            <asp:ImageButton ID="ibtnSearch" runat="server" ImageUrl="<%$ Resources:ImageUrl, SearchBtn %>" />
                            <asp:ImageButton ID="ibtnNewInspection" runat="server" ImageUrl="<%$ Resources:ImageUrl, NewInspectionRecordBtn %>" OnClick="ibtnNewInspection_Click" />
                        </div>
                    </div>
                </asp:View>
                <!--Search results for Management-->
                <asp:View ID="vSearchResult" runat="server">
                    <div>
                        <div class="headingText">
                            <cc3:CollapsibleSearchCriteriaReview ID="udcCollapsibleSearchCriteriaReview" runat="server" TargetControlID="pnlSearchCriteriaReview" />
                        </div>
                        <asp:Panel ID="pnlSearchCriteriaReview" runat="server">
                            <table id="searchMsg" style="width: 100%;">
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, InspectionRecord%>" /></td>
                                    <td colspan="3" class="fontBold">
                                        <asp:Label ID="lblOwner" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, InspectionRecordID%>" /></td>
                                    <td colspan="3" class="fontBold">
                                        <asp:Label ID="lblInspectionID" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, FileReferenceNo%>" /></td>
                                    <td colspan="3" class="fontBold">
                                        <asp:Label ID="lblFileReferenceNo" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" /></td>
                                    <td colspan="3" class="fontBold">
                                        <asp:Label ID="lblSPID" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, MainTypeOfInspection%>" /></td>
                                    <td class="fontBold">
                                        <asp:Label ID="lblTypeofInspection" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td style="width: 200px">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, VisitDate%>" /></td>
                                    <td class="fontBold">
                                        <asp:Label ID="lblStartVisitDate" runat="server" Text="" />
                                        <asp:Literal ID="Literal2" runat="server" Text="<%$ Resources: Text, To%>" />
                                        <asp:Label ID="lblEndVisitDate" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, Status%>" /></td>
                                    <td class="fontBold">
                                        <asp:Label ID="lblStatus" runat="server" Text=""></asp:Label>
                                    </td>
                                    <td></td>
                                    <td class="fontBold"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <div id="divSearchResults" runat="server">
                            <asp:GridView ID="gvSearchResult" runat="server" AllowPaging="True" AllowSorting="True"
                                Width="1300px" AutoGenerateColumns="false">
                                <Columns>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, InspectionRecordID%>" SortExpression="Inspection_ID">
                                        <ItemTemplate>
                                            <asp:LinkButton runat="server" OnClick="ResultLbtn_Click" Text='<%# Eval("Inspection_ID")%>'><%# Eval("Inspection_ID")%></asp:LinkButton>
                                        </ItemTemplate>
                                        <ItemStyle Width="100px" VerticalAlign="Top" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, FileReferenceNo%>" SortExpression="File_Reference_No">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRFileRefNo" runat="server" Text='<%# Eval("File_Reference_No") %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="110px" VerticalAlign="Top" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, SPIDPracticeID%>" SortExpression="SP_ID">
                                        <ItemTemplate>
                                            <%# Eval("SP_ID")%>
                                            (<%# Eval("Practice_Display_Seq")%>)
                                        </ItemTemplate>
                                        <ItemStyle Width="85px" VerticalAlign="Top" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, SPNameShort%>" SortExpression="SP_Eng_Name">
                                        <ItemTemplate>
                                            <asp:Label ID="lblREname" runat="server" Text='<%# Eval("SP_Eng_Name") %>'></asp:Label><br />
                                            <asp:Label ID="lblRCname" runat="server" Text='<%# Eval("SP_Chi_Name")%>' CssClass="TextGridChi"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="150px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Practice%>" SortExpression="Practice_Name">
                                        <ItemTemplate>
                                            <%# Eval("Practice_Name")%>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, MainTypeOfInspection%>" SortExpression="Main_Type_Of_Inspection_Value">
                                        <ItemTemplate>
                                            <%# Eval("Main_Type_Of_Inspection_Value")%>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, VisitDate%>" SortExpression="Visit_Date">
                                        <ItemTemplate>
                                            <%# Eval("Visit_Date_Format")%>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="80px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, CaseOfficer%>" SortExpression="Case_Officer_Value">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRCaseOfficer" runat="server" Text='<%# Eval("Case_Officer_Value")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, SubjectOfficer%>" SortExpression="Subject_Officer_Value">
                                        <ItemTemplate>
                                            <asp:Label ID="lblRSubjectOfficer" runat="server" Text='<%# Eval("Subject_Officer_Value")%>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="100px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, FollowUpAction%>" SortExpression="Follow_Up_Action">
                                        <ItemTemplate>
                                            <%# Eval("Follow_Up_Action")%>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="70px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Status%>" SortExpression="Record_Status">
                                        <ItemTemplate>
                                            <%# Eval("Status_Description")%>
                                        </ItemTemplate>
                                        <ItemStyle VerticalAlign="Top" Width="120px" />
                                        <HeaderStyle VerticalAlign="Top" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div style="margin-top: 15px">
                            <table>
                                <tr>
                                    <td>
                                        <asp:ImageButton ID="ibtnResultBack" runat="server" ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" />
                                    </td>
                                    <td align="center" style="width: 550px">
                                        <asp:ImageButton ID="ibtnEExportReport" runat="server" ImageUrl="<%$ Resources: ImageUrl, ExportReportBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, ExportReportBtn %>" OnClick="ibtnEExportReport_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <asp:HiddenField ID="hfEGenerationID" runat="server" />

                        <%-- Pop up for Export Record --%>
                        <asp:Button ID="btnER" runat="server" Style="display: none" />
                        <cc1:ModalPopupExtender ID="mpeExportReport" runat="server" TargetControlID="btnER"
                            PopupControlID="panER" BackgroundCssClass="modalBackgroundTransparent"
                            DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panERHeading">
                        </cc1:ModalPopupExtender>
                        <asp:Panel ID="panER" runat="server" Style="display: none">
                            <asp:Panel ID="panERHeading" runat="server" Style="cursor: move">
                                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                                    <tr>
                                        <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                                        <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                            <asp:Label ID="lblERTitle" runat="server" Text="<%$ Resources: Text, ExportReport %>"></asp:Label>
                                        </td>
                                        <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                                <tr>
                                    <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y"></td>
                                    <td style="background-color: #FFFFFF; padding: 5px">
                                        <table style="width: 100%">
                                            <tr>
                                                <td align="left" style="width: 45px; vertical-align: top">
                                                    <asp:Image ID="imgERIcon" runat="server" ImageUrl="~/Images/others/questionMark.png" />
                                                </td>
                                                <td align="left">
                                                    <asp:Label ID="lblERMessage" runat="server" Font-Bold="True" Text="<%$ Resources: Text, StudentErrorWarningReportPrompt %>"></asp:Label>
                                                </td>
                                                <td style="width: 10px"></td>
                                            </tr>
                                            <tr>
                                                <td align="center" colspan="3">
                                                    <asp:ImageButton ID="ibtnERDownloadNow" runat="server" ImageUrl="<%$ Resources: ImageUrl, DownloadNowBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, DownloadNowBtn %>" OnClick="ibtnERDownloadNow_Click" />
                                                    <asp:ImageButton ID="ibtnERDownloadLater" runat="server" ImageUrl="<%$ Resources: ImageUrl, DownloadLaterBtn %>"
                                                        AlternateText="<%$ Resources: AlternateText, DownloadLaterBtn %>" OnClick="ibtnERDownloadLater_Click" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                                </tr>
                                <tr>
                                    <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                                    <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                                    <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <%-- End of Pop up for Export Record --%>
                    </div>
                </asp:View>
                <!--Inspection Record Detail-->
                <asp:View ID="vViewInspectionDetail" runat="server">
                    <div>
                        <!--Inspection Record-->
                        <div class="headingText">
                            <asp:Label ID="Label12" runat="server" Text="<%$ Resources: Text, InspectionRecord%>"></asp:Label>
                        </div>

                        <table style="width: 100%" class="visitTargetTable">
                            <tr>
                                <td style="width: 250px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, InspectionRecordID%>" />
                                </td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetInspectionID" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MainTypeOfInspection%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblDetMainTypeofInspection" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, OtherTypeOfInspection%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblDetTypeofInspection" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FileReferenceNo%>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblDetFileNo" runat="server" Text="<%$ Resources: Text, Empty%>" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <span id="spanRefNo" runat="server">
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, ReferredFileReferenceNo%>" />
                                    </td>
                                    <td>
                                        <asp:LinkButton CssClass="fontBold" runat="server" ID="lbDetReferredReferenceNo1" OnClick="lbDetReferredReferenceNo1_Click" Enabled="false" Text="<%$ Resources: Text, Empty%>"></asp:LinkButton>
                                        <asp:HiddenField runat="server" ID="hfDetReferredInspectionID1" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:LinkButton CssClass="fontBold" runat="server" ID="lbDetReferredReferenceNo2" OnClick="lbDetReferredReferenceNo2_Click" Enabled="false" Text="<%$ Resources: Text, Empty%>"></asp:LinkButton>
                                        <asp:HiddenField runat="server" ID="hfDetReferredInspectionID2" />
                                    </td>
                                </tr>
                                <tr>
                                    <td></td>
                                    <td>
                                        <asp:LinkButton CssClass="fontBold" runat="server" ID="lbDetReferredReferenceNo3" OnClick="lbDetReferredReferenceNo3_Click" Enabled="false" Text="<%$ Resources: Text, Empty%>"></asp:LinkButton>
                                        <asp:HiddenField runat="server" ID="hfDetReferredInspectionID3" />
                                    </td>
                                </tr>
                            </span>
                            <tr>
                                <td class="fieldCaption">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, CaseOfficer%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetCaseOfficer" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    &nbsp;&nbsp;
                                    (<asp:Literal runat="server" Text="<%$ Resources: Text,ContactNo2%>" />:
                                    <asp:Label ID="lblDetCaseContactNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, SubjectOfficer%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetSubjectOfficer" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    &nbsp;&nbsp;
                                    (<asp:Literal runat="server" Text="<%$ Resources: Text,ContactNo2%>" />:
                                    <asp:Label ID="lblDetSubjectContactNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                            </tr>
                        </table>
                        <!--Visit Target-->
                        <div class="headingText">
                            <asp:Label ID="Label1" runat="server" Text="<%$ Resources: Text, VisitTarget%>"></asp:Label>
                        </div>
                        <table class="visitTargetTable">
                            <tr>
                                <td style="width: 250px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetSPID" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderName%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetSPName" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblDetSPStatus" runat="server" ForeColor="red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trDetSPContactInfo">
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ContactInformation%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlDetSPTelNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, TelNo%>"></asp:Label>
                                        <asp:Label ID="lblDetSPTelNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlDetSPFaxNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, FaxNo%>"></asp:Label>
                                        <asp:Label ID="lblDetSPFaxNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlDetSPEmail" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, Email%>"></asp:Label>
                                        <asp:Label ID="lblDetSPEmail" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr runat="server" id="trDetHCVSEffectiveDate">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetHCVSEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trDetHCVSDHCEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSDHCEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetHCVSDHCEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trDetHCVSCHNEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSCHNEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetHCVSCHNEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trDetLastVisitDate" runat="server">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LastVisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetLastVisitDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Practice%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlDetPractice" runat="server">
                                        <asp:Label ID="lblDetPractice" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                        <asp:Label ID="lblDetPracticeStatus" runat="server" ForeColor="red"></asp:Label>
                                        <asp:HiddenField ID="hdfIIRPracticeSeq" runat="server" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlDetPractice_Ci" runat="server">
                                        <asp:Label ID="lblDetPractice_Ci" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr id="trDetPracticeAddress" runat="server">
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PCDPracticeAddress%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetPracticeAddress" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />
                                    <asp:Label ID="lblDetPracticeAddress_Ci" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr id="trDetHealthProf" runat="server">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HealthProf%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetHealthProfession" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PhoneNoofPractice%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetPracticePhoneDaytime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>

                            <tr id="trFreezeDate" runat="server" style="font-style: italic;">
                                <td>
                                    <asp:Label runat="server" ID="lblFreezeDate"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <!--Visit Detail Section 1-->
                        <div class="headingText" runat="server" id="headVisitDetail">
                            <asp:Label ID="Label20" runat="server" Text="<%$ Resources: Text, VisitDetails%>"></asp:Label>
                        </div>
                        <table class="commonTable">
                            <tr>
                                <td class="fieldCaption">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, VisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetVisitDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                                <td class="fontBold">(<asp:Literal runat="server" Text="<%$ Resources: Text, VisitTime%>" />:
                                    <asp:Label ID="lblDetStartVisitTime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblDetTo" runat="server" Text="-" />
                                    <asp:Label ID="lblDetEndVisitTime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ConfirmationWith%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetConfirmationWith" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                                <td class="fontBold">(<asp:Literal runat="server" Text="<%$ Resources: Text, ConfirmDate%>" />:
                                    <asp:Label ID="lblDetConfirmDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FormCondition%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblDetFormCondition" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblDetFormConditionRm" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MeansOfCommunication%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblDetMeansofCommunication" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblDetMeansofCommunicationContact" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LowRiskClaim%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblDetLowRiskClaim" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Remarks%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblDetRemarks" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <!--Inspection Result Section-->
                        <div runat="server" id="divInspectionResultDetail">
                            <!--Inspection Result Section-->
                            <div class="headingText">
                                <asp:Label ID="Label11" runat="server" Text="<%$ Resources: Text, InspectionResult%>"></asp:Label>
                            </div>
                            <table class="commonTable">
                                <tr>
                                    <td style="width: 250px" valign="top">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, NoOfClaim01%>" ID="Literal5" /></td>
                                    <td colspan="3">
                                        <table border="1" cellpadding="0" cellspacing="0" class="clsNoOfClaim">
                                            <tr>
                                                <td rowspan="2" class="tdheader">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, InOrder%>" /></td>
                                                <td colspan="2" class="tdheader">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Irregularities%>" /></td>
                                                <td rowspan="2" class="tdheader">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, TotalChecked%>" /></td>
                                            </tr>
                                            <tr>

                                                <td class="tdheader">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MissingForm%>" /></td>
                                                <td class="tdheader">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Inconsistent%>" /></td>
                                            </tr>
                                            <tr>
                                                <td class="tdinput">
                                                    <asp:Label runat="server" ID="lblDetInOrder"></asp:Label>
                                                </td>
                                                <td class="tdinput">
                                                    <asp:Label runat="server" ID="lblDetMissingForm"></asp:Label>
                                                </td>

                                                <td class="tdinput">
                                                    <asp:Label runat="server" ID="lblDetInconsistent"></asp:Label>
                                                </td>
                                                <td class="tdinput">
                                                    <asp:Label runat="server" ID="lblDetTotalCheck" Text="" ClientIDMode="Static"></asp:Label></td>
                                            </tr>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, AnomalousClaims%>" /></td>
                                    <td colspan="3" class="fontBold">

                                        <asp:Label runat="server" ID="lblDetAnomalous" Text=""></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, OverMajorIrregularities%>" /></td>
                                    <td colspan="3" class="fontBold">
                                        <asp:Label runat="server" ID="lblDetOverMajor" Text=""></asp:Label>
                                    </td>

                                </tr>
                                <tr>
                                    <td>
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, CheckingDate%>" /></td>
                                    <td colspan="3" class="fontBold">
                                        <asp:Label runat="server" ID="lblDetCheckingDate"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                            <br />

                            <!--Action to be Token-->
                            <div class="headingText">
                                <asp:Label ID="Label21" runat="server" Text="<%$ Resources: Text, ActionOptional%>"></asp:Label>
                            </div>
                            <table class="commonTable">
                                <tr>
                                    <td class="fieldCaption" valign="top">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, ActionTaken%>" />
                                    </td>
                                    <td colspan="3">
                                        <asp:Label runat="server" Text="<%$ Resources: Text, NA%>" ID="lblFurtherActionDetailEmpty" Visible="false" CssClass="fontBold" />
                                        <table cellpadding="0" cellspacing="0" class="actiontobetakenshow">
                                            <asp:Panel runat="server" ID="headerFurtherActionDetail">
                                                <tr>
                                                    <td class="tdheader" style="width: 150px">
                                                        <asp:Literal runat="server" Text="<%$ Resources: Text, Action%>" /></td>
                                                    <td class="tdheader">
                                                        <asp:Literal runat="server" Text="<%$ Resources: Text, Type%>" /></td>
                                                    <td class="tdheader" style="width: 150px">
                                                        <asp:Literal runat="server" Text="<%$ Resources: Text, Date%>" /></td>
                                                </tr>
                                            </asp:Panel>
                                            <asp:Repeater ID="FurtherActionDetail" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <%#IIf(Eval("Rowspan") > 0, "<td  valign=""top"" rowspan=" + Convert.ToInt16(Eval("Rowspan")).ToString() + " style=""background-color:ivory"">" + Eval("Action") + "</td>", "")%>
                                                        <td>
                                                            <%#Eval("Type")%>
                                                        </td>
                                                        <td>
                                                            <%#Eval("Date")%>
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                        <br />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="fieldCaption" valign="top">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, FollowUpAction%>" /></td>
                                    <td colspan="3">
                                        <asp:Label runat="server" Text="<%$ Resources: Text, NA%>" ID="lblFollowUpActionDetailEmpty" Visible="false" CssClass="fontBold"></asp:Label>
                                        <table cellpadding="0" cellspacing="0" class="actiontobetakenshow">
                                            <asp:Panel ID="headerFollowUpActionDetail" runat="server" Visible="true">
                                                <tr>
                                                    <td class="tdheader" style="width: 50px"></td>
                                                    <td class="tdheader" style="">
                                                        <asp:Literal runat="server" Text="<%$ Resources: Text, Action%>" /></td>
                                                    <td class="tdheader" style="width: 150px">
                                                        <asp:Literal runat="server" Text="<%$ Resources: Text, Date%>" /></td>
                                                </tr>
                                            </asp:Panel>
                                            <asp:Repeater ID="repDetFollowUpAction" runat="server">
                                                <ItemTemplate>
                                                    <tr>
                                                        <td style="text-align: center;">
                                                            <%#Eval("Followup_Action_Seq")%>
                                                        </td>
                                                        <td>
                                                            <%# Eval("Action_Desc").ToString()%>                                                        
                                                        </td>
                                                        <td>
                                                            <%#Eval("Action_Date_Format")%>                                             
                                                        </td>
                                                    </tr>
                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </table>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <hr style="color: #999999; border-style: solid; border-width: 1px 0px 0px 0px; width: 98%; text-align: left" />
                        </div>
                        <!--Visit Detail Section 2-->
                        <table class="commonTable">
                            <tr>
                                <td style="width: 250px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Status%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetStatus" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trReopenReason" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ReopenReason%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetReopenRequestReason" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trRemoveRequest" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, RemoveRequestBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetRemoveRequestBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trRemoveApprove" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, RemoveApproveBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetRemoveApproveBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trReopenRequest" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ReopenRequestBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetReopenRequestBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trReopenApprove" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ReopenApproveBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetReopenApproveBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trCloseRequest" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, CloseRequestBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetCloseRequestBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trCloseApprove" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, CloseApproveBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetCloseApproveBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, CreateBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetCreatedBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, UpdatedBy%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblDetUpdatedBy" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <!--bottom button-->
                        <div class="bottomBtnBox">
                            <table>
                                <tr>
                                    <td align="left" style="width: 300px">
                                        <asp:ImageButton ID="ibtnDetailBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn%>" OnClick="ibtnDetailBack_Click" />
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ibtnEditDetail" runat="server" ImageUrl="<%$ Resources: ImageUrl,EditVisitDetailBtn%>" OnClick="ibtnEditDetail_Click" />
                                        <asp:ImageButton ID="ibtnInputInspectionResult" runat="server" ImageUrl="<%$ Resources: ImageUrl,InputInspectionResultBtn%>" />
                                        <asp:ImageButton ID="ibtnEditInspectionResult" runat="server" ImageUrl="<%$ Resources: ImageUrl,EditResultBtn%>" />
                                        <asp:ImageButton ID="ibtnPrint" runat="server" ImageUrl="<%$ Resources: ImageUrl, PrintBtn%>" OnClick="ibtnPrint_Click" />
                                        <asp:ImageButton ID="ibtnCloseCase" runat="server" ImageUrl="<%$ Resources: ImageUrl, CloseCaseBtn%>" OnClick="ibtnCloseCase_Click"/>
                                        <asp:ImageButton ID="ibtnRemove" runat="server" ImageUrl="<%$ Resources: ImageUrl, RemoveBtn%>" OnClick="ibtnRemove_Click"/>
                                        <asp:ImageButton ID="ibtnReopen" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReopenCaseBtn%>" OnClick="ibtnReopen_Click"/>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </div>
                    <%-- Pop up for Print Report --%>
                    <asp:HiddenField runat="server" ID="hdfShowPop" Value="" />


                    <asp:Button ID="btnPrintReport" runat="server" Style="display: none" />
                    <cc1:ModalPopupExtender ID="mpePrintReport" runat="server" TargetControlID="btnPrintReport"
                        PopupControlID="panPrintReport" BackgroundCssClass="modalBackgroundTransparent"
                        DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panPrintReportHeading">
                    </cc1:ModalPopupExtender>
                    <asp:Panel ID="panPrintReport" runat="server" Style="display: none">
                        <asp:Panel ID="panPrintReportHeading" runat="server" Style="cursor: move">
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                                <tr>
                                    <td class="popTopLeft"></td>
                                    <td class="popTopMid popTitle">
                                        <asp:Label runat="server" Text="<%$ Resources: Text, PrintReport %>"></asp:Label>
                                    </td>
                                    <td class="popTopRight"></td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                            <tr>
                                <td class="popLeft"></td>
                                <td class="popMid">
                                    <div class="headingText">
                                        <asp:Label ID="Label63" runat="server" Text="<%$ Resources: Text,ReportSelection%>"></asp:Label>
                                    </div>
                                    <div>
                                        <!--should get list from DB -->
                                        <asp:RadioButtonList ID="rblPrintContent" runat="server">
                                            <asp:ListItem Value="InternalReference">Internal Reference</asp:ListItem>
                                            <asp:ListItem Value="ConfirmationLetter">Confirmation Letter</asp:ListItem>
                                            <asp:ListItem Value="InspectionSummary">Inspection Summary Report</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </div>
                                    <!--bottom button-->
                                    <div class="bottomBtnBox">
                                        <span>
                                            <asp:ImageButton ID="ibtPdf" runat="server" ImageUrl="<%$ Resources: ImageUrl,PDFBtn%>" /></span>
                                        <span>
                                            <asp:ImageButton ID="ibtWord" runat="server" ImageUrl="<%$ Resources: ImageUrl,WordBtn%>" /></span>
                                        <span>
                                            <asp:ImageButton ID="ibtnCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl,CancelBtn%>" OnClick="ibtnCancel_Click" /></span>
                                    </div>

                                </td>
                                <td class="popRgith"></td>
                            </tr>
                            <tr>
                                <td class="popBottomLeft"></td>
                                <td class="popBottomMid"></td>
                                <td class="popBottomRight"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                    <%-- End of Pop up for Print Record --%>
                </asp:View>
                <!--Edit Visit Detail-->
                <asp:View ID="vEditVisit" runat="server">
                    <div>
                        <!--Inspection Record-->
                        <div class="headingText">
                            <asp:Label ID="Label13" runat="server" Text="<%$ Resources: Text,InspectionRecord%>"></asp:Label>
                        </div>

                        <table style="width: 860px">
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text,InspectionRecordID%>" />
                                </td>
                                <td class="fontBold" colspan="3">
                                    <asp:Label ID="lblEdtInspectionID" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MainTypeOfInspection%>" />
                                </td>
                                <td class="fontBold" colspan="3">
                                    <asp:Label ID="lblEdtMainTypeOfInspection" runat="server" Text="" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 200px" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, OtherTypeOfInspection%>" ID="ltrEditTypeofInspection" /></td>
                                <td colspan="3" style="border-style: solid; border-width: thin; border-color: grey;">
                                    <asp:CheckBoxList ID="chkListEdtTypeofInspection" runat="server" RepeatLayout="Table" CssClass="typIns" RepeatDirection="Vertical">
                                    </asp:CheckBoxList>
                                </td>
                                <td width="10" valign="top">
                                    <asp:Image ID="imgchkListEdtTypeofInspectionErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                            </tr>
                            <tr>

                                <td width="200">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FileReferenceNo%>" />
                                </td>
                                <td class="fontBold" colspan="3">
                                    <asp:Label ID="lblEdtRefNo" runat="server" Text="<%$ Resources: Text, Empty%>" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ReferredFileReferenceNo%>" />
                                </td>
                                <td class="fontBold" colspan="3">(1)
                                    <asp:Label ID="lblEdtRRNAPrefix" runat="server" Text=""></asp:Label>

                                    <asp:TextBox ID="txtEdtRRNA1" runat="server" Width="10" MaxLength="1"></asp:TextBox>
                                    /
                                    <asp:TextBox ID="txtEdtRRNA2" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtEdtRRNA3" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtEdtRRNA4" runat="server" Width="30" MaxLength="3"></asp:TextBox>
                                    - (
                                    <asp:TextBox ID="txtEdtRRNA5" runat="server" Width="20" MaxLength="2" onChange="convertToUpper(this)"></asp:TextBox>
                                    )
                                    <asp:Image ID="imgEdtRRNAErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNA1Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtRRNA1"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNA2Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtRRNA2"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNA3Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtRRNA3"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNA4Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtRRNA4"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNA5Filter" runat="server" FilterType="Custom, Numbers,LowercaseLetters, UppercaseLetters" TargetControlID="txtEdtRRNA5"></cc1:FilteredTextBoxExtender>
                            </tr>
                            <tr>
                                <td></td>
                                <td class="fontBold" colspan="3">(2)
                                    <asp:Label ID="lblEdtRRNBPrefix" runat="server" Text=""></asp:Label>

                                    <asp:TextBox ID="txtEdtRRNB1" runat="server" Width="10" MaxLength="1"></asp:TextBox>
                                    /
                                    <asp:TextBox ID="txtEdtRRNB2" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtEdtRRNB3" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtEdtRRNB4" runat="server" Width="30" MaxLength="3"></asp:TextBox>
                                    - (
                                    <asp:TextBox ID="txtEdtRRNB5" runat="server" Width="20" MaxLength="2" onChange="convertToUpper(this)"></asp:TextBox>
                                    )
                                    <asp:Image ID="imgEdtRRNBErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNB1Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtRRNB1"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNB2Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtRRNB2"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNB3Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtRRNB3"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNB4Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtRRNB4"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNB5Filter" runat="server" FilterType="Custom, Numbers,LowercaseLetters, UppercaseLetters" TargetControlID="txtEdtRRNB5"></cc1:FilteredTextBoxExtender>
                            </tr>
                            <tr>
                                <td></td>
                                <td class="fontBold" colspan="3">(3)
                                    <asp:Label ID="lblEdtRRNCPrefix" runat="server" Text=""></asp:Label>

                                    <asp:TextBox ID="txtEdtRRNC1" runat="server" Width="10" MaxLength="1"></asp:TextBox>
                                    /
                                    <asp:TextBox ID="txtEdtRRNC2" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtEdtRRNC3" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtEdtRRNC4" runat="server" Width="30" MaxLength="3"></asp:TextBox>
                                    - (
                                    <asp:TextBox ID="txtEdtRRNC5" runat="server" Width="20" MaxLength="2" onChange="convertToUpper(this)"></asp:TextBox>
                                    )
                                    <asp:Image ID="imgEdtRRNCErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNC1Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtRRNC1"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNC2Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtRRNC2"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNC3Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtRRNC3"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNC4Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtRRNC4"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtEdtRRNC5Filter" runat="server" FilterType="Custom, Numbers,LowercaseLetters, UppercaseLetters" TargetControlID="txtEdtRRNC5"></cc1:FilteredTextBoxExtender>
                            </tr>
                            <caption>
                                <datalist id="edtOfficerList" runat="server">
                                </datalist>
                                <tr>
                                    <td style="width: 200px">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, CaseOfficer%>" />
                                    </td>
                                    <td style="width: 340px">
                                        <asp:TextBox ID="txtEdtCaseOfficer" runat="server" Width="280px"></asp:TextBox>
                                        <asp:HiddenField ID="hfEdtCaseOfficer" runat="server" />
                                        <asp:Image ID="imgtxtEdtCaseOfficerErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Style="vertical-align: middle;" Visible="False" />
                                    </td>
                                    <td style="width: 80px">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, ContactNo2%>" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtEdtCaseContactNo" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                                        <asp:Image ID="imgtxtEdtCaseContactNoErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Style="vertical-align: middle;" Visible="False" />
                                        <cc1:FilteredTextBoxExtender ID="txtEdtCaseContactNoFilter" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;" TargetControlID="txtEdtCaseContactNo" />
                                    </td>
                                </tr>
                            </caption>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, SubjectOfficer%>" /></td>
                                <td>
                                    <asp:TextBox ID="txtEdtSubjectOfficer" runat="server" Width="280px"></asp:TextBox>
                                    <asp:HiddenField ID="hfEdtSubjectOfficer" runat="server" />
                                    <asp:Image ID="imgtxtEdtSubjectOfficerErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ContactNo2%>" /></td>
                                <td>
                                    <asp:TextBox ID="txtEdtSubjectContactNo" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                                    <asp:Image ID="imgtxtEdtSubjectContactNoErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    <cc1:FilteredTextBoxExtender ID="txtEdtSubjectContactNoFilter" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;" TargetControlID="txtEdtSubjectContactNo"></cc1:FilteredTextBoxExtender>

                                </td>
                            </tr>
                        </table>

                        <div class="headingText">
                            <asp:Label ID="Label2" runat="server" Text="<%$ Resources: Text, VisitTarget%>"></asp:Label>
                        </div>
                        <table class="visitTargetTable" style="width: 100%" runat="server" id="tableVisitTargetReadOnly">
                            <tr>
                                <td width="200">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEdtSPID" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderName%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEdtSPName" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trEdtSPContactInfo">
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ContactInformation%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlEdtSPTelNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, TelNo%>"></asp:Label>
                                        <asp:Label ID="lblEdtSPTelNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlEdtSPFaxNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, FaxNo%>"></asp:Label>
                                        <asp:Label ID="lblEdtSPFaxNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlEdtSPEmail" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, Email%>"></asp:Label>
                                        <asp:Label ID="lblEdtSPEmail" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr runat="server" id="trEdtHCVSEffectiveDate">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEdtHCVSEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trEdtHCVSDHCEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSDHCEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEdtHCVSDHCEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trEdtHCVSCHNEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSCHNEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEdtHCVSCHNEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr id="trEdtLastVisitDate" runat="server">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LastVisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEdtLastVisitDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Practice%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlEdtPractice" runat="server">
                                        <asp:Label ID="lblEdtPractice" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlEdtPractice_Ci" runat="server">
                                        <asp:Label ID="lblEdtPractice_Ci" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PCDPracticeAddress%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEdtPracticeAddress" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />
                                    <asp:Label ID="lblEdtPracticeAddress_Ci" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr id="trEdtHealthProf" runat="server">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HealthProf%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEdtHealthProfession" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PhoneNoofPractice%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEdtPracticePhoneDaytime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <table style="width: 100%" runat="server" id="tableVisitTargetForEdit">
                            <tr>
                                <td style="width: 200px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" ID="Literal777" />
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" Visible="false" ID="Literal8" />
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ReferredFileReferenceNo%>" Visible="false" ID="Literal9" />
                                </td>
                                <td class="fontBold">
                                    <asp:TextBox ID="txtEditSPID" runat="server" MaxLength="8"></asp:TextBox>
                                    <asp:Image ID="imgtxtEditSPIDErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    <asp:ImageButton ID="ibtnEditSearchVisitTarget" runat="server" OnClick="ibtnEditSearchVisitTarget_Click" CssClass="verticalAlignMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, SearchSBtn %>" />
                                    <asp:ImageButton ID="ibtnEditClear" runat="server" CssClass="verticalAlignMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, ClearDisableSBtn %>" OnClick="ibtnEditClear_Click" Enabled="false" />
                                </td>
                            </tr>
                            <tr id="tr2" runat="server" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEditServiceProviderID" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderName%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEditServiceProviderName" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblEditSPStatus" runat="server" ForeColor="red"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trEditSPContactInfo">
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ContactInformation%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEditSPContactInfo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Panel ID="pnlEditSPTelNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, TelNo%>"></asp:Label>
                                        <asp:Label ID="lblEditSPTelNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlEditSPFaxNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, FaxNo%>"></asp:Label>
                                        <asp:Label ID="lblEditSPFaxNo" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlEditSPEmail" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, Email%>"></asp:Label>
                                        <asp:Label ID="lblEditSPEmail" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr runat="server" id="trEditHCVSEffectiveDate">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEditHCVSEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trEditHCVSDHCEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSDHCEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEditHCVSDHCEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trEditHCVSCHNEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSCHNEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEditHCVSCHNEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LastVisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEditLastVisitDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align: left;">
                                    <hr style="color: #999999; border-style: solid; border-width: 1px 0px 0px 0px; width: 530px; text-align: left; margin-top: 5px; margin-bottom: 5px; margin-left: 0px; margin-right: 0px" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Practice%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlEditPractice" runat="server">
                                        <asp:DropDownList ID="ddlEditPractice" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlEditPractice_SelectedIndexChanged" Visible="false" Width="320px">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdfEditPracticeSeq" runat="server" />
                                        <asp:Label ID="lblEditPractice" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                        <asp:Label ID="lblEditPracticeStatus" runat="server" ForeColor="red" Visible="false" />
                                        <asp:Image ID="imgddlEditPracticeErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlEditPractice_Ci" runat="server">
                                        <asp:Label ID="lblEditPractice_Ci" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PCDPracticeAddress%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEditPracticeAddress" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />
                                    <asp:Label ID="lblEditPracticeAddress_Ci" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HealthProf%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEditHealthProfession" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PhoneNoofPractice%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEditPracticePhoneDaytime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <cc1:FilteredTextBoxExtender ID="txtEditSPIDFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEditSPID"></cc1:FilteredTextBoxExtender>

                        <div class="headingText">
                            <asp:Label ID="Label24" runat="server" Text="<%$ Resources: Text, VisitDetails%>"></asp:Label>
                        </div>
                        <table style="width: 100%">

                            <tr>
                                <td width="200px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, VisitDate%>" /></td>
                                <td width="300px">
                                    <asp:TextBox ID="txtEdtVisitDate" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="ibtnEdtVisitDate" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                    <cc1:CalendarExtender ID="CalExtEdtVisitDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnEdtVisitDate"
                                        TargetControlID="txtEdtVisitDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                    <asp:Image ID="imgtxtEdtVisitDateErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    <cc1:FilteredTextBoxExtender ID="txtEdtVisitDateFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtVisitDate" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                </td>
                                <td width="120px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, VisitTime%>" /></td>
                                <td>
                                    <asp:TextBox ID="txtEdtStartVisitTime" runat="server" MaxLength="10" Width="40px"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="MaskedEdtStartVisitTime" runat="server" Mask="99:99" MaskType="Time" UserTimeFormat="TwentyFourHour" TargetControlID="txtEdtStartVisitTime" PromptCharacter="_" AutoCompleteValue="00:00"></cc1:MaskedEditExtender>
                                    <asp:Image ID="imgtxtEdtStartVisitTimeErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />

                                    <asp:Label ID="lbl_To" runat="server" Text="<%$ Resources: Text, To%>" />
                                    <asp:TextBox ID="txtEdtEndVisitTime" runat="server" MaxLength="10" Width="40px"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="MaskedEdtEndVisitTime" runat="server" Mask="99:99" MaskType="Time" UserTimeFormat="TwentyFourHour" TargetControlID="txtEdtEndVisitTime" PromptCharacter="_" AutoCompleteValue="00:00"></cc1:MaskedEditExtender>
                                    <asp:Image ID="imgtxtEdtEndVisitTimeErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />

                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ConfirmationWith%>" /></td>
                                <td>
                                    <asp:TextBox ID="txtEdtConfirmationWith" runat="server" MaxLength="100"></asp:TextBox>
                                    <asp:Image ID="imgtxtEdtConfirmationWithErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />

                                </td>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ConfirmDate%>" /></td>
                                <td>
                                    <asp:TextBox ID="txtEdtConfirmDate" runat="server" MaxLength="10" Width="85px"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="ibtnEdtConfirmDate" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                    <cc1:CalendarExtender ID="CalExtEdtConfirmDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnEdtConfirmDate"
                                        TargetControlID="txtEdtConfirmDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                    <asp:Image ID="imgtxtEdtConfirmDateErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    <cc1:FilteredTextBoxExtender ID="txtEdtConfirmDateFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtEdtConfirmDate" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FormCondition%>" /></td>
                                <td colspan="3">
                                    <asp:Panel ID="pnlEdtFormCondition" runat="server">
                                        <asp:DropDownList ID="ddlEdtFormCondition" runat="server" Width="350px" AutoPostBack="true" OnSelectedIndexChanged="ddlEdtFormCondition_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Image ID="imgddlEdtFormConditionErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />

                                    </asp:Panel>
                                    <asp:Panel ID="pnlEdtFormConditionRm" runat="server" Style="margin-top: 5px" Visible="false">
                                        <asp:Label runat="server" Text="<%$ Resources: Text, Remarks%>" Font-Size="Small"></asp:Label>
                                        <asp:TextBox ID="txtEdtFormConditionRm" runat="server" Width="289px" MaxLength="255"></asp:TextBox>
                                        <asp:Image ID="imgtxtEdtFormConditionRmErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MeansOfCommunication%>" /></td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlEdtMeansofCommunication" Width="350px" runat="server" OnSelectedIndexChanged="ddlEdtMeansofCommunication_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:Image ID="imgddlEdtMeansofCommunicationErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    <asp:Panel ID="pnlEdtMeansofCommunicationFax" runat="server" Style="margin-top: 5px" Visible="true">
                                        <asp:Label runat="server" Text="<%$ Resources: Text, FaxNo%>" Font-Size="Small"></asp:Label>
                                        <asp:TextBox ID="txtEdtMeansofCommunicationFax" MaxLength="20" runat="server" Width="80"></asp:TextBox>
                                        <asp:Image ID="imgtxtEdtMeansofCommunicationFaxErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                        <cc1:FilteredTextBoxExtender ID="FilteredTtxtEdtMeansofCommunicationFax" runat="server" TargetControlID="txtEdtMeansofCommunicationFax"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;"></cc1:FilteredTextBoxExtender>

                                    </asp:Panel>
                                    <asp:Panel ID="pnlEdtMeansofCommunicationEmail" runat="server" Style="margin-top: 5px" Visible="true">
                                        <asp:Label runat="server" Text="<%$ Resources: Text, Email%>" Font-Size="Small"></asp:Label>
                                        <asp:TextBox ID="txtEdtMeansofCommunicationEmail" MaxLength="255" runat="server" Width="200"></asp:TextBox>
                                        <asp:Image ID="imgtxtEdtMeansofCommunicationEmailErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    </asp:Panel>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LowRiskClaim%>" /></td>
                                <td colspan="3">
                                     <asp:RadioButtonList runat="server" ID="rdoEdtLowRiskClaim" RepeatDirection="Horizontal" Width="120px">
                                        <asp:ListItem Text="<%$ Resources: Text, No%>" Value="N"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources: Text, Yes%>" Value="Y"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:Image ID="imgrdoEdtLowRiskClaimErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Remarks%>" /></td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtEdtRemarks" runat="server" MaxLength="255" Width="538px"></asp:TextBox>
                                    <asp:Image ID="imgtxtEdtRemarksErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />

                                </td>
                            </tr>
                        </table>

                        <!--bottom button-->
                        <div class="bottomBtnBox">
                            <span style="position: absolute; left: 0">
                                <asp:ImageButton ID="ibtnEditVisitBack" runat="server" ImageUrl="<%$ Resources: ImageUrl,BackBtn%>" OnClick="ibtnEditVisitBack_Click" /></span>
                            <span>
                                <asp:ImageButton ID="ibtnEditVisitSave" runat="server" ImageUrl="<%$ Resources: ImageUrl,SaveBtn%>" OnClick="ibtnEditVisitSave_Click" /></span>
                        </div>
                    </div>
                </asp:View>
                <!--Edit Visit Confirm-->
                <asp:View ID="vEditVisitConfirm" runat="server">
                    <div>
                        <div class="headingText">
                            <asp:Label ID="Label17" runat="server" Text="<%$ Resources: Text, InspectionRecord%>"></asp:Label>
                        </div>

                        <table style="width: 100%">
                            <tr>
                                <td width="200">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, InspectionRecordID%>" />
                                </td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCInspectionID" runat="server" Text=""></asp:Label>
                                </td>
                                <td></td>
                                <td></td>
                            </tr>
                              <tr>
                                <td class="fieldCaption" width="200">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MainTypeOfInspection%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblEVCMainTypeofInspection" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" width="200" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, OtherTypeOfInspection%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblEVCTypeofInspection" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="tr1">
                                <td width="200">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FileReferenceNo%>" />
                                </td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCFileReferenceNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Label runat="server" Text="<%$ Resources: Text, ReferredFileReferenceNo%>" />
                                </td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCReferenceNo1" Font-Bold="true" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCReferenceNo2" Font-Bold="true" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCReferenceNo3" Font-Bold="true" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, CaseOfficer%>" /></td>
                                <td class="fontBold" colspan="3">
                                    <asp:Label ID="lblEVCCaseOfficer" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    &nbsp;&nbsp;
                                    (<asp:Literal runat="server" Text="<%$ Resources: Text, ContactNo2%>" />:
                                    <asp:Label ID="lblEVCCaseContactNo" runat="server" Text=""></asp:Label>)
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, SubjectOfficer%>" /></td>
                                <td class="fontBold" colspan="3">
                                    <asp:Label ID="lblEVCSubjectOfficer" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    &nbsp;&nbsp;
                                    (<asp:Literal runat="server" Text="<%$ Resources: Text, ContactNo2%>" />:
                                    <asp:Label ID="lblEVCSubjectContactNo" runat="server" Text=""></asp:Label>)
                                </td>
                            </tr>
                        </table>

                        <div class="headingText">
                            <asp:Label ID="Label4" runat="server" Text="<%$ Resources: Text, VisitTarget%>"></asp:Label>
                        </div>
                        <table class="visitTargetTable">

                            <tr>
                                <td width="200">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCSPID" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderName%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCSPName" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblEVCSPStatus" runat="server" ForeColor="red"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trEVCSPContactInfo">
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ContactInformation%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlEVCSPTelNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, TelNo%>"></asp:Label>
                                        <asp:Label ID="lblEVCSPTelNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlEVCSPFaxNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, FaxNo%>"></asp:Label>
                                        <asp:Label ID="lblEVCSPFaxNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlEVCSPEmail" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, Email%>"></asp:Label>
                                        <asp:Label ID="lblEVCSPEmail" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr runat="server" id="trEVCHCVSEffectiveDate">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCHCVSEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trEVCHCVSDHCEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSDHCEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCHCVSDHCEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trEVCHCVSCHNEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSCHNEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCHCVSCHNEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LastVisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCLastVisitDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Practice%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlEVCPractice" runat="server">
                                        <asp:Label ID="lblEVCPractice" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                        <asp:Label ID="lblEVCPracticeStatus" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlEVCPractice_Ci" runat="server">
                                        <asp:Label ID="lblEVCPractice_Ci" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PCDPracticeAddress%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCPracticeAddress" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />
                                    <asp:Label ID="lblEVCPracticeAddress_Ci" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HealthProf%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCHealthProfession" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PhoneNoofPractice%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCPracticePhoneDaytime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />
                                </td>
                            </tr>
                        </table>
                        <div class="headingText">
                            <asp:Label ID="Label23" runat="server" Text="<%$ Resources: Text, VisitDetails%>"></asp:Label>
                        </div>
                        <table style="width: 100%;" class="commonTable">

                            <tr>
                                <td width="200">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, VisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCVisitDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <td class="fontBold">(<asp:Literal runat="server" Text="<%$ Resources: Text, VisitTime%>" />:
                                    <asp:Label ID="lblEVCVisitTime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                    </td>
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ConfirmationWith%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblEVCConfirmationWith" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                                <td class="fontBold">(<asp:Literal runat="server" Text="<%$ Resources: Text, ConfirmDate%>" />:
                                    <asp:Label ID="lblEVCConfirmDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FormCondition%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblEVCFormCondition" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblEVCFormConditionRm" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MeansOfCommunication%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblEVCMeansofCommunication" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblEVCMeansofCommunicationContact" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LowRiskClaim%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblEVCLowRiskClaim" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Remarks%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblEVCRemarks" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <!--bottom button-->
                        <div class="bottomBtnBox">
                            <span style="position: absolute; left: 0">
                                <asp:ImageButton ID="ibtnEditVisitConfirmBack" runat="server" ImageUrl="<%$ Resources: ImageUrl,BackBtn%>" OnClick="ibtnEditVisitConfirmBack_Click" /></span>
                            <span>
                                <asp:ImageButton ID="ibtnEditVisitConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl,ConfirmBtn%>" OnClick="ibtnEditVisitConfirm_Click" /></span>
                        </div>
                    </div>

                </asp:View>
                <!--New Inspection Record-->
                <asp:View ID="vNewInspection" runat="server">
                    <div>
                        <div class="headingText">
                            <asp:Label ID="Label15" runat="server" Text="<%$ Resources: Text, InspectionRecord%>"></asp:Label>
                        </div>
                        <table style="width: 860px">
                             <tr>
                                <td width="200" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MainTypeOfInspection%>" /></td>
                                <td colspan="3" style="border-style: solid; border-width: thin; border-color: grey;">
                                    <asp:RadioButtonList ID="rdoListAddMainTypeofInspection" runat="server" RepeatLayout="Table" CssClass="typIns" AutoPostBack="true" OnSelectedIndexChanged="rdoListAddMainTypeofInspection_SelectedIndexChanged">
                                    </asp:RadioButtonList>
                                </td>
                                <td width="10" valign="top">
                                    <asp:Image ID="imgrdoListAddMainTypeofInspectionErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                            </tr>
                            <tr>
                                <td width="200" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, OtherTypeOfInspection%>" /></td>
                                <td colspan="3" style="border-style: solid; border-width: thin; border-color: grey;">
                                    <asp:CheckBoxList ID="chkListAddTypeofInspection" runat="server" RepeatLayout="Table" CssClass="typIns" RepeatDirection="Vertical">
                                    </asp:CheckBoxList>
                                </td>
                                <td width="10" valign="top">
                                    <asp:Image ID="imgchkListAddTypeofInspectionErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FileReferenceType%>" />
                                </td>
                                <td class="fontBold" colspan="3">
                                    <asp:RadioButtonList runat="server" ID="rdoFileReferenceType" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdoFileReferenceType_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FileReferenceNo%>" />
                                </td>
                                <td class="fontBold" colspan="3">
                                    <asp:Label ID="lblFRNPrefix" runat="server" Text=""></asp:Label>

                                    <asp:TextBox ID="txtFRN1" runat="server" Width="10" MaxLength="1"></asp:TextBox>
                                    /
                                    <asp:TextBox ID="txtFRN2" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtFRN3" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                               
                                    <asp:Label runat="server" ID="lblFileSeqNoAlert" Text="<%$ Resources: Text, FileSeqNoAlert%>" Visible="true"></asp:Label>
                                    <asp:TextBox ID="txtFRN4" runat="server" Width="30" MaxLength="3" Visible="false"></asp:TextBox>

                                    <asp:Label runat="server" ID="lblFilePartNoLeft" Visible="false">- (</asp:Label>
                                    <asp:TextBox ID="txtFRN5" runat="server" Width="20" MaxLength="2" Visible="false" onChange="convertToUpper(this)"></asp:TextBox>
                                    <asp:Label runat="server" ID="lblFilePratNoRight" Visible="false">)</asp:Label>
                                    <asp:Image ID="imgAddFRNErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <cc1:FilteredTextBoxExtender ID="txtFRN1Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtFRN1"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtFRN2Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtFRN2"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtFRN3Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtFRN3"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtFRN4Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtFRN4"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtFRN5Filter" runat="server" FilterType="Custom, Numbers,LowercaseLetters, UppercaseLetters" TargetControlID="txtFRN5"></cc1:FilteredTextBoxExtender>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ReferredFileReferenceNo%>" />
                                </td>
                                <td class="fontBold" colspan="3">(1)
                                    <asp:Label ID="lblRRNAPrefix" runat="server" Text=""></asp:Label>

                                    <asp:TextBox ID="txtRRNA1" runat="server" Width="10" MaxLength="1"></asp:TextBox>
                                    /
                                    <asp:TextBox ID="txtRRNA2" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtRRNA3" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtRRNA4" runat="server" Width="30" MaxLength="3"></asp:TextBox>
                                    - (
                                    <asp:TextBox ID="txtRRNA5" runat="server" Width="20" MaxLength="2" onChange="convertToUpper(this)"></asp:TextBox>
                                    )
                                    <asp:Image ID="imgAddRRNAErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <cc1:FilteredTextBoxExtender ID="txtRRNA1Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtRRNA1"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtRRNA2Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtRRNA2"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtRRNA3Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtRRNA3"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtRRNA4Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtRRNA4"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtRRNA5Filter" runat="server" FilterType="Custom, Numbers,LowercaseLetters, UppercaseLetters" TargetControlID="txtRRNA5"></cc1:FilteredTextBoxExtender>
                            </tr>
                            <tr>
                                <td></td>
                                <td class="fontBold" colspan="3">(2)
                                    <asp:Label ID="lblRRNBPrefix" runat="server" Text=""></asp:Label>

                                    <asp:TextBox ID="txtRRNB1" runat="server" Width="10" MaxLength="1"></asp:TextBox>
                                    /
                                    <asp:TextBox ID="txtRRNB2" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtRRNB3" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtRRNB4" runat="server" Width="30" MaxLength="3"></asp:TextBox>
                                    - (
                                    <asp:TextBox ID="txtRRNB5" runat="server" Width="20" MaxLength="2" onChange="convertToUpper(this)"></asp:TextBox>
                                    )
                                    <asp:Image ID="imgAddRRNBErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <cc1:FilteredTextBoxExtender ID="txtRRNB1Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtRRNB1"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtRRNB2Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtRRNB2"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtRRNB3Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtRRNB3"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtRRNB4Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtRRNB4"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtRRNB5Filter" runat="server" FilterType="Custom, Numbers,LowercaseLetters, UppercaseLetters" TargetControlID="txtRRNB5"></cc1:FilteredTextBoxExtender>
                            </tr>
                            <tr>
                                <td></td>
                                <td class="fontBold" colspan="3">(3)
                                    <asp:Label ID="lblRRNCPrefix" runat="server" Text=""></asp:Label>

                                    <asp:TextBox ID="txtRRNC1" runat="server" Width="10" MaxLength="1"></asp:TextBox>
                                    /
                                    <asp:TextBox ID="txtRRNC2" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtRRNC3" runat="server" Width="20" MaxLength="2"></asp:TextBox>
                                    -
                                    <asp:TextBox ID="txtRRNC4" runat="server" Width="30" MaxLength="3"></asp:TextBox>
                                    - (
                                    <asp:TextBox ID="txtRRNC5" runat="server" Width="20" MaxLength="2" onChange="convertToUpper(this)"></asp:TextBox>
                                    )
                                    <asp:Image ID="imgAddRRNCErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <cc1:FilteredTextBoxExtender ID="txtRRNC1Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtRRNC1"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtRRNC2Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtRRNC2"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtRRNC3Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtRRNC3"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtRRNC4Filter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtRRNC4"></cc1:FilteredTextBoxExtender>
                                <cc1:FilteredTextBoxExtender ID="txtRRNC5Filter" runat="server" FilterType="Custom, Numbers,LowercaseLetters, UppercaseLetters" TargetControlID="txtRRNC5"></cc1:FilteredTextBoxExtender>
                            </tr>
                            <caption>
                                <datalist id="officerList" runat="server">
                                </datalist>
                                <tr>
                                    <td style="width: 200px;">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, CaseOfficer%>" />
                                    </td>
                                    <asp:Image ID="Image2" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Style="vertical-align: middle;" Visible="False" />
                                    <td style="width: 340px;">
                                        <asp:TextBox ID="txtCaseOfficer" runat="server" Width="280px"></asp:TextBox>
                                        <asp:HiddenField ID="hfCaseOfficer" runat="server" />
                                        <asp:Image ID="imgtxtCaseOfficerErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Style="vertical-align: middle;" Visible="False" />
                                    </td>
                                    <td style="width: 80px;">
                                        <asp:Literal runat="server" Text="<%$ Resources: Text, ContactNo2%>" />
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCaseContactNo" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                                        <asp:Image ID="imgtxtCaseContactNoErr" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Style="vertical-align: middle;" Visible="False" />
                                        <cc1:FilteredTextBoxExtender ID="txtCaseContactNoFilter" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;" TargetControlID="txtCaseContactNo" />
                                    </td>
                                </tr>
                            </caption>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, SubjectOfficer%>" /></td>
                                <td>
                                    <asp:TextBox ID="txtSubjectOfficer" runat="server" Width="280px"></asp:TextBox>
                                    <asp:HiddenField ID="hfSubjectOfficer" runat="server" />
                                    <asp:Image ID="imgtxtSubjectOfficerErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ContactNo2%>" /></td>
                                <td>
                                    <asp:TextBox ID="txtSubjectContactNo" runat="server" MaxLength="20" Width="100px"></asp:TextBox>
                                    <asp:Image ID="imgtxtSubjectContactNoErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    <cc1:FilteredTextBoxExtender ID="txtSubjectContactNoFilter" runat="server" FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;" TargetControlID="txtSubjectContactNo"></cc1:FilteredTextBoxExtender>

                                </td>
                            </tr>
                        </table>
                        <div class="headingText">
                            <asp:Label ID="Label6" runat="server" Text="<%$ Resources: Text, VisitTarget%>"></asp:Label>
                        </div>
                        <table class="visitTargetTable">

                            <tr>
                                <td style="width: 200px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" ID="ltrSPIDRefNo" />
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" Visible="false" ID="ltrServiceProviderID" />
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ReferredFileReferenceNo%>" Visible="false" ID="ltrReferredFileReferenceNo" />
                                </td>
                                <td class="fontBold">
                                    <asp:TextBox ID="txtSPIDNew" runat="server" MaxLength="8"></asp:TextBox>
                                    <asp:Image ID="imgtxtSPIDNewErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    <asp:ImageButton ID="ibtnSearchVisitTarget" runat="server" OnClick="ibtnSearchVisitTarget_Click"
                                        ImageUrl="<%$ Resources:ImageUrl, SearchSBtn %>" CssClass="verticalAlignMiddle" />
                                    <asp:ImageButton ID="ibtnClear" runat="server" CssClass="verticalAlignMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, ClearDisableSBtn %>" OnClick="ibtnClear_Click" Enabled="false" />
                                </td>
                            </tr>
                            <tr id="trServiceProviderID" runat="server" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblServiceProviderID" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderName%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblServiceProviderName" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblSPStatus" runat="server" ForeColor="red" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trSPContactInfo">
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ContactInformation%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblSPContactInfo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Panel ID="pnlSPTelNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, TelNo%>"></asp:Label>
                                        <asp:Label ID="lblSPTelNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlSPFaxNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, FaxNo%>"></asp:Label>
                                        <asp:Label ID="lblSPFaxNo" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlSPEmail" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, Email%>"></asp:Label>
                                        <asp:Label ID="lblSPEmail" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr runat="server" id="trHCVSEffectiveDate">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblHCVSEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trHCVSDHCEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSDHCEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblHCVSDHCEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trHCVSCHNEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSCHNEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblHCVSCHNEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LastVisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblLastVisitDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td style="text-align: left;">
                                    <hr style="color: #999999; border-style: solid; border-width: 1px 0px 0px 0px; width: 530px; text-align: left; margin-top: 5px; margin-bottom: 5px; margin-left: 0px; margin-right: 0px" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Practice%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlPractice" runat="server">
                                        <asp:DropDownList ID="ddlPractice" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlPractice_SelectedIndexChanged" Visible="false" Width="320px">
                                        </asp:DropDownList>
                                        <asp:HiddenField ID="hdfPracticeSeq" runat="server" />
                                        <asp:Label ID="lblPractice" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                        <asp:Label ID="lblPracticeStatus" runat="server" Text="" ForeColor="red" Visible="false" />
                                        <asp:Image ID="imgddlPracticeErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlPractice_Ci" runat="server">
                                        <asp:Label ID="lblPractice_Ci" runat="server" Text=""></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PCDPracticeAddress%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblPracticeAddress" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />
                                    <asp:Label ID="lblPracticeAddress_Ci" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HealthProf%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblHealthProfession" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PhoneNoofPractice%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblPracticePhoneDaytime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <cc1:FilteredTextBoxExtender ID="txtSPIDNewFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtSPIDNew"></cc1:FilteredTextBoxExtender>
                        <div class="headingText">
                            <asp:Label ID="Label7" runat="server" Text="<%$ Resources: Text, VisitDetails%>"></asp:Label>
                        </div>
                        <table style="width: 100%">

                            <tr>
                                <td style="width: 200px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, VisitDate%>" /></td>
                                <td style="width: 300px">
                                    <asp:TextBox ID="txtVisitDate" runat="server" MaxLength="10" Width="75px"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="btnVisitDate" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                    <cc1:CalendarExtender ID="CalExtVisitDate" CssClass="ajax_cal" runat="server" PopupButtonID="btnVisitDate"
                                        TargetControlID="txtVisitDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                    <cc1:FilteredTextBoxExtender ID="txtVisitDateFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtVisitDate" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                    <asp:Image ID="imgtxtVisitDateErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <td style="width: 120px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, VisitTime%>" /></td>
                                <td>
                                    <asp:TextBox ID="txtStartVisitTime" runat="server" MaxLength="10" Width="40px"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="maskedtxtStartVisitTime" runat="server" Mask="99:99" MaskType="Time" UserTimeFormat="TwentyFourHour" TargetControlID="txtStartVisitTime" PromptCharacter="_" AutoCompleteValue="00:00"></cc1:MaskedEditExtender>
                                    <asp:Image ID="imgtxtStartVisitTimeErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, To%>" />
                                    <asp:TextBox ID="txtEndVisitTime" runat="server" MaxLength="10" Width="40px"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="maskedtxtEndVisitTime" runat="server" Mask="99:99" MaskType="Time" UserTimeFormat="TwentyFourHour" TargetControlID="txtEndVisitTime" PromptCharacter="_" AutoCompleteValue="00:00"></cc1:MaskedEditExtender>
                                    <asp:Image ID="imgtxtEndVisitTimeErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ConfirmationWith%>" /></td>
                                <td>
                                    <asp:TextBox ID="txtConfirmationWith" runat="server" MaxLength="100"></asp:TextBox>
                                    <asp:Image ID="imgtxtConfirmationWithErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ConfirmDate%>" /></td>
                                <td>
                                    <asp:TextBox ID="txtConfirmDate" runat="server" MaxLength="10" Width="85px"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="btnConfirmDate" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                    <cc1:CalendarExtender ID="CalExtConfirmDate" CssClass="ajax_cal" runat="server" PopupButtonID="btnConfirmDate"
                                        TargetControlID="txtConfirmDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                    <cc1:FilteredTextBoxExtender ID="txtConfirmDateFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtConfirmDate" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                    <asp:Image ID="imgtxtConfirmDateErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FormCondition%>" /></td>
                                <td colspan="3">
                                    <asp:Panel ID="pnlFormCondition" runat="server">
                                        <asp:DropDownList ID="ddlFormCondition" runat="server" Width="350px" AutoPostBack="true" OnSelectedIndexChanged="ddlFormCondition_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Image ID="imgddlFormConditionErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    </asp:Panel>
                                    <asp:Panel ID="pnlFormConditionRemarks" runat="server" Style="margin-top: 5px" Visible="false">
                                        <asp:Label runat="server" Text="<%$ Resources: Text, Remarks%>" Font-Size="Small"></asp:Label>
                                        <asp:TextBox ID="txtFormConditionRm" MaxLength="255" runat="server" Width="289px"></asp:TextBox>
                                        <asp:Image ID="imgtxtFormConditionRmErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MeansOfCommunication%>" /></td>
                                <td colspan="3">
                                    <asp:DropDownList ID="ddlMeansofCommunication" runat="server" Width="350px" OnSelectedIndexChanged="ddlMeansofCommunication_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                                    <asp:Image ID="imgddlMeansofCommunicationErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />

                                    <asp:Panel ID="pnlMeansofCommunicationFax" runat="server" Style="margin-top: 5px" Visible="true">
                                        <asp:Label runat="server" Text="<%$ Resources: Text, FaxNo%>" Font-Size="Small"></asp:Label>
                                        <asp:TextBox ID="txtMeansofCommunicationFax" MaxLength="20" runat="server" Width="80"></asp:TextBox>
                                        <asp:Image ID="ImgtxtMeansofCommunicationFaxErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                        <cc1:FilteredTextBoxExtender ID="FilteredtxtMeansofCommunicationFax" runat="server" TargetControlID="txtMeansofCommunicationFax"
                                            FilterType="Custom, LowercaseLetters, UppercaseLetters, Numbers" ValidChars=" ~!@#$%^&*()_+`-=[]\{}|;':<>?,./&quot;"></cc1:FilteredTextBoxExtender>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlMeansofCommunicationEmail" runat="server" Style="margin-top: 5px" Visible="true">
                                        <asp:Label runat="server" Text="<%$ Resources: Text, Email%>" Font-Size="Small"></asp:Label>
                                        <asp:TextBox ID="txtMeansofCommunicationEmail" MaxLength="255" runat="server" Width="200"></asp:TextBox>
                                        <asp:Image ID="imgtxtMeansofCommunicationEmailErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    </asp:Panel>
                                </td>
                            </tr>
                             <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LowRiskClaim%>" /></td>
                                <td colspan="3">
                                     <asp:RadioButtonList runat="server" ID="rdoAddLowRiskClaim" RepeatDirection="Horizontal" Width="120px">
                                        <asp:ListItem Text="<%$ Resources: Text, No%>" Value="N"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources: Text, Yes%>" Value="Y"></asp:ListItem>
                                    </asp:RadioButtonList>
                                    <asp:Image ID="imgrdoAddLowRiskClaimErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                            </tr>

                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Remarks%>" /></td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtRemarks" runat="server" MaxLength="255" Width="538px"></asp:TextBox>
                                    <asp:Image ID="imgtxtRemarksErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                            </tr>

                        </table>

                        <!--bottom button-->
                        <div class="bottomBtnBox">
                            <span style="position: absolute; left: 0">
                                <asp:ImageButton ID="ibtnSubmitBack" runat="server" ImageUrl="<%$ Resources: ImageUrl,BackBtn%>" OnClick="ibtnSubmitBack_Click" /></span>
                            <span>
                                <asp:ImageButton ID="ibtnNewInspectionSubmit" runat="server" ImageUrl="<%$ Resources: ImageUrl,SubmitBtn%>" OnClick="ibtnNewInspectionSubmit_Click" /></span>
                        </div>
                    </div>
                </asp:View>
                <!--Inspection Record Confirm-->
                <asp:View ID="vInspectionConfirm" runat="server">
                    <div>
                        <div class="headingText">
                            <asp:Label ID="Label16" runat="server" Text="<%$ Resources: Text, InspectionRecord%>"></asp:Label>
                        </div>
                        <table style="width: 100%">
                             <tr>
                                <td class="fieldCaption" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MainTypeOfInspection%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblConMainTypeofInspection" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, OtherTypeOfInspection%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblConTypeofInspection" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FileReferenceType%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblConFileReferenceType" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="200">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FileReferenceNo%>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblFileRefNoConfirm" runat="server" Text="" CssClass="fontBold"></asp:Label>

                                </td>

                            </tr>
                            <tr>
                                <td width="200">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ReferredFileReferenceNo%>" />
                                </td>
                                <td class="fontBold">
                                    <asp:Label ID="lblRefRefNo1Confirm" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="200"></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblRefRefNo2Confirm" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="200"></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblRefRefNo3Confirm" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="fieldCaption">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, CaseOfficer%>" /></td>
                                <td class="fontBold" colspan="3">
                                    <asp:Label ID="lblConCaseOfficer" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    &nbsp;&nbsp;
                                    (<asp:Literal runat="server" Text="<%$ Resources: Text, ContactNo2%>" />:
                                    <asp:Label ID="lblConCaseContactNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, SubjectOfficer%>" /></td>
                                <td class="fontBold" colspan="3">
                                    <asp:Label ID="lblConSubjectOfficer" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    &nbsp;&nbsp;
                                    (<asp:Literal runat="server" Text="<%$ Resources: Text, ContactNo2%>" />:
                                    <asp:Label ID="lblConSubjectContactNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                            </tr>
                        </table>
                        <div class="headingText">
                            <asp:Label ID="Label8" runat="server" Text="<%$ Resources: Text, VisitTarget%>"></asp:Label>
                        </div>
                        <table class="visitTargetTable">
                            <tr>
                                <td width="200">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblConSPID" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderName%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblConSPName" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblConSPStatus" Text="" runat="server" ForeColor="red"></asp:Label>
                                </td>
                            </tr>

                            <tr runat="server" id="trConSPContactInfo">
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ContactInformation%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlConSPTelNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, TelNo%>"></asp:Label>
                                        <asp:Label ID="lblConSPTelNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlConSPFaxNo" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, FaxNo%>"></asp:Label>
                                        <asp:Label ID="lblConSPFaxNo" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlConSPEmail" runat="server">
                                        <asp:Label CssClass="mRight20" runat="server" Text="<%$ Resources: Text, Email%>"></asp:Label>
                                        <asp:Label ID="lblConSPEmail" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr runat="server" id="trConHCVSEffectiveDate">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblConHCVSEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trConHCVSDHCEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSDHCEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblConHCVSDHCEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr runat="server" id="trConHCVSCHNEffectiveDate" visible="false">
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HCVSCHNEffectiveDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblConHCVSCHNEffectiveDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LastVisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblConLastVisitDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Practice%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlConPractice" runat="server">
                                        <asp:Label ID="lblConPractice" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                        <asp:Label ID="lblConPracticeStatus" runat="server" Text="" ForeColor="red"></asp:Label>
                                    </asp:Panel>
                                    <asp:Panel ID="pnlConPractice_Ci" runat="server">
                                        <asp:Label ID="lblConPractice_Ci" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PCDPracticeAddress%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblConPracticeAddress" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />
                                    <asp:Label ID="lblConPracticeAddress_Ci" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, HealthProf%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblConHP" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PhoneNoofPractice%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblConPracticePhoneDaytime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>

                        </table>
                        <div class="headingText">
                            <asp:Label ID="Label27" runat="server" Text="<%$ Resources: Text, VisitDetails%>"></asp:Label>
                        </div>
                        <table class="commonTable">

                            <tr>
                                <td style="width: 200px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, VisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblConVisitDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                                <td class="fontBold">(<asp:Literal runat="server" Text="<%$ Resources: Text, VisitTime%>" />:
                                    <asp:Label ID="lblConVisitTime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ConfirmationWith%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblConConfirmationWith" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                                <td class="fontBold">(<asp:Literal runat="server" Text="<%$ Resources: Text, ConfirmDate%>" />:
                                    <asp:Label ID="lblConConfirmDate" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                                <td></td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FormCondition%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblConFormCondition" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MeansOfCommunication%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblConMeansofCommunication" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    <asp:Label ID="lblConMeansofCommunicationContact" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, LowRiskClaim%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblConLowRiskClaim" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Remarks%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label ID="lblConRemarks" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <!--bottom button-->
                        <div class="bottomBtnBox">
                            <span style="position: absolute; left: 0">
                                <asp:ImageButton ID="ibtnInspectionConBack" runat="server" ImageUrl="<%$ Resources: ImageUrl,BackBtn%>" OnClick="ibtnInspectionConBack_Click" /></span>
                            <span>
                                <asp:ImageButton ID="ibtnConfirmNew" runat="server" ImageUrl="<%$ Resources: ImageUrl,ConfirmBtn%>" OnClick="ibtnConfirmNew_Click" /></span>
                        </div>
                    </div>
                </asp:View>
                <!--Create Successfully View-->
                <asp:View ID="vActionResultBox" runat="server">
                    <cc2:InfoMessageBox ID="boxInfoMessage" runat="server" Width="95%" />
                    <span>
                        <asp:ImageButton ID="ibtnMsgBoxBack" runat="server" ImageUrl="<%$ Resources: ImageUrl,BackBtn%>" OnClick="ibtnMsgBoxBack_Click" />
                    </span>
                </asp:View>
                <!--Input Inspection Result-->
                <asp:View ID="vInputInspectionResult" runat="server">
                    <div>
                        <div class="headingText">
                            <asp:Label ID="Label18" runat="server" Text="<%$ Resources: Text, InspectionRecord%>"></asp:Label>
                        </div>

                        <table style="width: 100%" class="visitTargetTable">
                            <tr>
                                <td style="width: 250px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, InspectionRecordID%>" />
                                </td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRInspectionID" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <!--type of inspection-->
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MainTypeOfInspection%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRMainTypeofInspection" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, OtherTypeOfInspection%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRTypeofInspection" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FileReferenceNo%>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblIIRFileNo" runat="server" Text="<%$ Resources: Text, Empty%>" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <div class="headingText">
                            <asp:Label ID="Label3" runat="server" Text="<%$ Resources: Text, VisitDetails%>"></asp:Label>
                        </div>
                        <table style="width: 100%">
                            <tr>
                                <td width="250">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRSPID" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderName%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRSPName" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />

                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Practice%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlIIRPractice" runat="server">
                                        <asp:Label ID="lblIIRPractice" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>

                                    </asp:Panel>

                                    <asp:Panel ID="pnlIIRPracticeChi" runat="server">
                                        <asp:Label ID="lblIIRPracticeChi" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PCDPracticeAddress%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRPracticeAddress" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />
                                    <asp:Label ID="lblIIRPracticeAddressChi" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>

                            <!--Visit Date-->
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, VisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRVisitDate" runat="server" Text="<%$ Resources: Text, Empty%>" CssClass="padright20"></asp:Label>
                                    (<asp:Literal runat="server" Text="<%$ Resources: Text, VisitTime%>" />
                                    <asp:Label ID="lblIIRVisitTime" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                            </tr>
                        </table>
                        <div class="headingText">
                            <asp:Label ID="Label9" runat="server" Text="<%$ Resources: Text, InspectionResult%>"></asp:Label>
                        </div>
                        <table class="commonTable">
                            <tr>
                                <td width="250" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, NoOfClaim01%>" ID="Literal3" /></td>
                                <td colspan="3">
                                    <table border="1" cellpadding="0" cellspacing="0" class="clsNoOfClaim">
                                        <tr>
                                            <td rowspan="2" class="tdheader">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, InOrder%>" /></td>
                                            <td colspan="2" class="tdheader">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, Irregularities%>" /></td>
                                            <td rowspan="2" class="tdheader">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, TotalChecked%>" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdheader">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, MissingForm%>" /></td>
                                            <td class="tdheader">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, Inconsistent%>" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:TextBox runat="server" ID="txtIIRInOrder" ClientIDMode="Static" Width="50px" MaxLength="4"></asp:TextBox>
                                                <asp:Image ID="imgtxtIIRInOrderErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox runat="server" ID="txtIIRMissingForm" ClientIDMode="Static" Width="50px" MaxLength="4"></asp:TextBox>
                                                <asp:Image ID="imgtxtIIRMissingFormErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                            <td class="tdinput">
                                                <asp:TextBox runat="server" ID="txtIIRInconsistent" ClientIDMode="Static" Width="50px" MaxLength="4"></asp:TextBox>
                                                <asp:Image ID="imgtxtIIRInconsistentErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="lblIIRTotalCheck" Text="" ClientIDMode="Static"></asp:Label></td>
                                        </tr>
                                        <cc1:FilteredTextBoxExtender ID="txtIIRInOrderFilter" runat="server" FilterType="Numbers" TargetControlID="txtIIRInOrder"></cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="txtIIRMissingFormFilter" runat="server" FilterType="Numbers" TargetControlID="txtIIRMissingForm"></cc1:FilteredTextBoxExtender>
                                        <cc1:FilteredTextBoxExtender ID="txtIIRInconsistentFilter" runat="server" FilterType="Numbers" TargetControlID="txtIIRInconsistent"></cc1:FilteredTextBoxExtender>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, AnomalousClaims%>" /></td>
                                <td style="width: 130px;">
                                    <asp:RadioButtonList runat="server" ID="rdoIIAnomalousClaim" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdoIIAnomalousClaim_SelectedIndexChanged" Width="120px">
                                        <asp:ListItem Text="<%$ Resources: Text, No%>" Value="N"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources: Text, Yes%>" Value="Y"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td style="width: 20px;">
                                    <asp:Image ID="imgrdoIIAnomalousClaimErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="<%$ Resources: Text, NoOfRecords%>"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtIINoofAnomalousClaim" Enabled="false" Width="50px" MaxLength="4"></asp:TextBox>
                                    <asp:Image ID="imgtxtIINoofAnomalousClaimErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <cc1:FilteredTextBoxExtender ID="FilteredtxtIINoofAnomalousClaim" runat="server" FilterType="Custom, Numbers" TargetControlID="txtIINoofAnomalousClaim"></cc1:FilteredTextBoxExtender>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text,OverMajorIrregularities%>" /></td>
                                <td>
                                    <asp:RadioButtonList runat="server" ID="rdoIIROverMajor" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rdoIIROverMajor_SelectedIndexChanged" Width="120px">
                                        <asp:ListItem Text="<%$ Resources: Text, No%>" Value="N"></asp:ListItem>
                                        <asp:ListItem Text="<%$ Resources: Text, Yes%>" Value="Y"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td style="width: 20px;">
                                    <asp:Image ID="imgrdoIIROverMajorErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <td>
                                    <asp:Label runat="server" Text="<%$ Resources: Text, NoOfRecords%>"></asp:Label>
                                    <asp:TextBox runat="server" ID="txtIINoofIsOverMajor" Enabled="false" Width="50px" MaxLength="4"></asp:TextBox>
                                    <asp:Image ID="imgtxtIINoofIsOverMajorErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                </td>
                                <cc1:FilteredTextBoxExtender ID="FilteredtxtIINoofIsOverMajor" runat="server" FilterType="Custom, Numbers" TargetControlID="txtIINoofIsOverMajor"></cc1:FilteredTextBoxExtender>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, CheckingDate%>" /></td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtIIRCheckingDate" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                    &nbsp;<asp:ImageButton ID="ibtnCheckingDate" runat="server" ImageAlign="AbsMiddle"
                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                    <cc1:CalendarExtender ID="CalExtIIRCheckingDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnCheckingDate"
                                        TargetControlID="txtIIRCheckingDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                    <cc1:FilteredTextBoxExtender ID="txtIIRCheckingDateFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtIIRCheckingDate" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                    <asp:Image ID="imgtxtIIRCheckingDateErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />

                                </td>
                            </tr>
                        </table>
                        <!--Action-->
                        <div class="headingText">
                            <asp:Label ID="Label22" runat="server" Text="<%$ Resources: Text, ActionOptional%>"></asp:Label>
                        </div>
                        <table class="commonTable">
                            <tr>
                                <td style="width: 250px" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ActionTaken%>" ID="Literal6" />
                                </td>
                                <td colspan="3">
                                    <table border="1" cellpadding="0" cellspacing="0" class="actiontobetaken" align="left">
                                        <tr>
                                            <td rowspan="1" class="tdheader" style="width: 150px">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, Action%>" />
                                            </td>
                                            <td rowspan="1" class="tdheader" style="width: 295px">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, Type%>" />
                                            </td>
                                            <td rowspan="1" class="tdheader" style="width: 205px">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, Date%>" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td rowspan="6" class="tdinput" valign="top" style="background-color: ivory">
                                                <asp:Label runat="server" ID="Label28" Text="<%$ Resources: Text, IssueLetter%>"></asp:Label>
                                            </td>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label29" Text="<%$ Resources: Text, AdvisoryLetter%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label30">
                                                    <asp:TextBox ID="dateIIRAletter" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRAletter" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRAletter" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRAletter"
                                                        TargetControlID="dateIIRAletter" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRAletterFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="dateIIRAletter" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                                </asp:Label>
                                                <asp:Image ID="dateIIRAletterError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label36" Text="<%$ Resources: Text, WarningLetter%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label38">
                                                    <asp:TextBox ID="dateIIRWletter" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRWletter" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRWletter" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRWletter"
                                                        TargetControlID="dateIIRWletter" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRWletterFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="dateIIRWletter" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIRWletterError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label40" Text="<%$ Resources: Text, DelistLetter%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label42">
                                                    <asp:TextBox ID="dateIIRDletter" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRDletter" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRDletter" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRDletter"
                                                        TargetControlID="dateIIRDletter" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRDletterFilter" TargetControlID="dateIIRDletter" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIRDletterError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label14" Text="<%$ Resources: Text, SuspendPaymentLetter%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label25">
                                                    <asp:TextBox ID="dateIIRSPletter" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRSPletter" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRSPletter" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRSPletter"
                                                        TargetControlID="dateIIRSPletter" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRSPletterFilter" TargetControlID="dateIIRSPletter" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIRSPletterError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label32" Text="<%$ Resources: Text, SuspendEHCPAccountLetter%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label33">
                                                    <asp:TextBox ID="dateIIRSEAletter" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRSEAletter" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRSEAletter" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRSEAletter"
                                                        TargetControlID="dateIIRSEAletter" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRSEAletterFilter" TargetControlID="dateIIRSEAletter" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIRSEAletterError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label44" Text="<%$ Resources: Text, Others%>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtIIROthers" MaxLength="200"></asp:TextBox>
                                                <asp:Image ID="txtIIROthersError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label45">
                                                    <asp:TextBox ID="dateIIROther" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIROther" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIROther" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIROther"
                                                        TargetControlID="dateIIROther" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIROtherFilter" TargetControlID="dateIIROther" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIROtherError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td rowspan="7" class="tdinput" valign="top" style="background-color: ivory">
                                                <asp:Label runat="server" ID="Label35" Text="<%$ Resources: Text, ReferParties%>"></asp:Label>
                                            </td>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label39" Text="<%$ Resources: Text, BoardAndCouncil%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label43">
                                                    <asp:TextBox ID="dateIIRBoard" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRBoard" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRBoard" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRBoard"
                                                        TargetControlID="dateIIRBoard" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRBoardFilter" TargetControlID="dateIIRBoard" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                                </asp:Label>
                                                <asp:Image ID="dateIIRBoardError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label46" Text="<%$ Resources: Text, Police%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label47">
                                                    <asp:TextBox ID="dateIIRPolice" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRPolice" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRPolice" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRPolice"
                                                        TargetControlID="dateIIRPolice" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRPoliceFilter" TargetControlID="dateIIRPolice" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIRPoliceError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label34" Text="<%$ Resources: Text, SocialWelfareDepartment%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label37">
                                                    <asp:TextBox ID="dateIIRSWDepart" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRSWDepart" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRSWDepart" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRSWDepart"
                                                        TargetControlID="dateIIRSWDepart" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRSWDepartFilter" TargetControlID="dateIIRSWDepart" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIRSWDepartError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label41" Text="<%$ Resources: Text, HKCustomsAndExciseDepartment%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label55">
                                                    <asp:TextBox ID="dateIIRHKCAEDepart" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRHKCAEDepart" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRHKCAEDepart" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRHKCAEDepart"
                                                        TargetControlID="dateIIRHKCAEDepart" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRHKCAEDepartFilter" TargetControlID="dateIIRHKCAEDepart" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIRHKCAEDepartError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label56" Text="<%$ Resources: Text, ImmigrationDepartment%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label57">
                                                    <asp:TextBox ID="dateIIRIDepart" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRIDepart1" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRIDepart" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRIDepart1"
                                                        TargetControlID="dateIIRIDepart" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRIDepartFilter" TargetControlID="dateIIRIDepart" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIRIDepartError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label58" Text="<%$ Resources: Text, LabourDepartment%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label59">
                                                    <asp:TextBox ID="dateIIRLDepart" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRLDepart" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRLDepart" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRLDepart"
                                                        TargetControlID="dateIIRLDepart" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRLDepartFilter" TargetControlID="dateIIRLDepart" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                                </asp:Label>
                                                <asp:Image ID="dateIIRLDepartError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label48" Text="<%$ Resources: Text, Others%>"></asp:Label>
                                                <asp:TextBox runat="server" ID="txtIIROtherTP" MaxLength="200"></asp:TextBox>
                                                <asp:Image ID="txtIIROtherTPError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label49">
                                                    <asp:TextBox ID="dateIIROthers" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIROthers" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIROthers" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIROthers"
                                                        TargetControlID="dateIIROthers" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIROthersFilter" TargetControlID="dateIIROthers" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIROthersError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>

                                        <tr>
                                            <td rowspan="3" class="tdinput" valign="top" style="background-color: ivory">
                                                <asp:Label runat="server" ID="Label50" Text="<%$ Resources: Text, ActionToEHCP%>"></asp:Label>
                                            </td>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label60" Text="<%$ Resources:Text, SuspendTheEHCPFromHCVS %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label61">
                                                    <asp:TextBox ID="dateIIRSuspendEHCP" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRSuspendEHCP" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRSuspendEHCP" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRSuspendEHCP"
                                                        TargetControlID="dateIIRSuspendEHCP" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRSuspendEHCPFilter" TargetControlID="dateIIRSuspendEHCP" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIRSuspendEHCPError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label51" Text="<%$ Resources: Text, DelistTheEHCP%>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label52">
                                                    <asp:TextBox ID="dateIIRDelist" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRDelist" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRDelist" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRDelist"
                                                        TargetControlID="dateIIRDelist" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRDelistFilter" TargetControlID="dateIIRDelist" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIRDelistError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>

                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="Label53" Text="<%$ Resources:Text, RecoveryOrSuspensionPayment %>"></asp:Label>
                                            </td>
                                            <td>
                                                <asp:Label runat="server" ID="Label54">
                                                    <asp:TextBox ID="dateIIRRecovery" runat="server" MaxLength="10" Width="105px"></asp:TextBox>
                                                    &nbsp;<asp:ImageButton ID="btnIIRRecovery" runat="server" ImageAlign="AbsMiddle"
                                                        ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                    <cc1:CalendarExtender ID="CalExtIIRRecovery" CssClass="ajax_cal" runat="server" PopupButtonID="btnIIRRecovery"
                                                        TargetControlID="dateIIRRecovery" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                        StartDate="01-01-2009"></cc1:CalendarExtender>
                                                    <cc1:FilteredTextBoxExtender ID="dateIIRRecoveryFilter" TargetControlID="dateIIRRecovery" runat="server" FilterType="Custom, Numbers" ValidChars="-"></cc1:FilteredTextBoxExtender>

                                                </asp:Label>
                                                <asp:Image ID="dateIIRRecoveryError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                            </td>
                                        </tr>
                                    </table>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <br />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="fontBold">
                                    <asp:Label runat="server" Text="<%$ Resources: Text, FollowUpActionAlert%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="250" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FollowUpAction%>" ID="Literal7" /></td>
                                <td colspan="3">
                                    <table border="1" cellpadding="0" cellspacing="0" class="actiontobetaken">
                                        <tr>
                                            <td rowspan="1" class="tdheader" style="width: 50px"></td>
                                            <td rowspan="1" class="tdheader">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, Action%>" /></td>
                                            <td rowspan="1" class="tdheader" style="width: 160px">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text,Date%>" /></td>
                                            <td rowspan="1" class="tdheader" style="width: 40px;"></td>
                                        </tr>
                                        <asp:Repeater ID="repFollowUpAction" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td align="center"><%#Eval("Followup_Action_Seq")%>
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtFollowUpAction" Text='<%# Eval("Action_Desc").ToString()%>' Width="340px" MaxLength="200"></asp:TextBox>
                                                        <asp:Image ID="txtFollowUpActionError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                                    </td>
                                                    <td>
                                                        <asp:TextBox runat="server" ID="txtFollowUpDate" Text='<%# Eval("Action_Date")%>' Width="105px"></asp:TextBox>
                                                        &nbsp;<asp:ImageButton ID="ibtnFollowUpDate" runat="server" ImageAlign="AbsMiddle"
                                                            ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>" />
                                                        <cc1:CalendarExtender ID="CalExtFollowUpDate" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnFollowUpDate"
                                                            TargetControlID="txtFollowUpDate" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy" Enabled="True"
                                                            StartDate="01-01-2009"></cc1:CalendarExtender>
                                                        <asp:Image ID="txtFollowUpDateError" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                                        <cc1:FilteredTextBoxExtender ID="txtFollowUpDateFilter" runat="server" FilterType="Custom, Numbers" TargetControlID="txtFollowUpDate" ValidChars="-"></cc1:FilteredTextBoxExtender>
                                                    </td>
                                                    <td>
                                                        <asp:ImageButton ID="lnkAddFollowUp" runat="server" ImageAlign="AbsMiddle" OnClick="lnkAddFollowUp_Click"
                                                            ImageUrl="<%$ Resources: ImageUrl,AddIBtn%>" />
                                                        <asp:ImageButton ID="lnkDelFollowUp" runat="server" ImageAlign="AbsMiddle"
                                                            ImageUrl="<%$ Resources: ImageUrl,RemoveIBtn%>" CausesValidation="false" CommandArgument='<%#Eval("Followup_Action_Seq")%>' OnClick="lnkDelFollowUp_Click" />
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <!--bottom button-->
                        <div class="bottomBtnBox">
                            <span style="position: absolute; left: 0">
                                <asp:ImageButton ID="ibtnIIRBack" runat="server" ImageUrl="<%$ Resources: ImageUrl,BackBtn%>" /></span>
                            <span>
                                <asp:ImageButton ID="ibtnIIRSave" runat="server" ImageUrl="<%$ Resources: ImageUrl,SaveBtn%>" /></span>
                        </div>
                    </div>
                </asp:View>
                <!--Confirm Inspection Result-->
                <asp:View ID="vConfirmInspectionResult" runat="server">
                    <div>

                        <div class="headingText">
                            <asp:Label ID="Label19" runat="server" Text="<%$ Resources: Text, InspectionRecord%>"></asp:Label>
                        </div>

                        <table style="width: 100%">
                            <tr>
                                <td style="width: 250px">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, InspectionRecordID%>" />
                                </td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRInspectionIDConfirm" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <!--type of inspection-->
                             <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, MainTypeOfInspection%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRMainTypeofInspectionConfirm" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, OtherTypeOfInspection%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRTypeofInspectionConfirm" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FileReferenceNo%>" />
                                </td>
                                <td>
                                    <asp:Label ID="lblIIRFileNoConfirm" runat="server" Text="<%$ Resources: Text, Empty%>" Font-Bold="true"></asp:Label>
                                </td>
                            </tr>
                        </table>

                        <div class="headingText">
                            <asp:Label ID="Label10" runat="server" Text="<%$ Resources: Text, VisitDetail%>"></asp:Label>
                        </div>
                        <table style="width: 100%">

                            <tr>
                                <td width="250">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderID%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRSPIDConfirm" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ServiceProviderName%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRSPNameConfirm" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />

                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Practice%>" /></td>
                                <td class="fontBold">
                                    <asp:Panel ID="pnlIIRPracticeConfirm" runat="server">
                                        <asp:Label ID="lblIIRPracticeConfirm" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>

                                    </asp:Panel>
                                    <asp:Panel ID="pnlIIRPracticeChiConfirm" runat="server">
                                        <asp:Label ID="lblIIRPracticeChiConfirm" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>
                                    </asp:Panel>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, PCDPracticeAddress%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRPracticeAddressConfirm" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label><br />
                                    <asp:Label ID="lblIIRPracticeAddressChiConfirm" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>

                            <!--Visit Date-->
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, VisitDate%>" /></td>
                                <td class="fontBold">
                                    <asp:Label ID="lblIIRVisitDateConfirm" runat="server" Text="<%$ Resources: Text, Empty%>" CssClass="padright20"></asp:Label>
                                    <asp:Literal runat="server" Text="(Visit Time" />
                                    <asp:Label ID="lblIIRVisitTimeConfirm" runat="server" Text="<%$ Resources: Text, Empty%>"></asp:Label>)
                                </td>
                            </tr>
                        </table>
                        <div class="headingText">
                            <asp:Label ID="Label26" runat="server" Text="<%$ Resources: Text, InspectionResult%>"></asp:Label>
                        </div>
                        <table class="commonTable">
                            <tr>
                                <td width="250" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, NoOfClaim01%>" ID="Literal4" /></td>
                                <td colspan="3">
                                    <table border="1" cellpadding="0" cellspacing="0" class="clsNoOfClaim">
                                        <tr>
                                            <td rowspan="2" class="tdheader">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, InOrder%>" /></td>
                                            <td colspan="2" class="tdheader">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, Irregularities%>" /></td>
                                            <td rowspan="2" class="tdheader">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, TotalChecked%>" /></td>
                                        </tr>
                                        <tr>

                                            <td class="tdheader">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, MissingForm%>" /></td>
                                            <td class="tdheader">
                                                <asp:Literal runat="server" Text="<%$ Resources: Text, Inconsistent%>" /></td>
                                        </tr>
                                        <tr>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="lblIIRInOrder"></asp:Label>
                                            </td>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="lblIIRMissingForm"></asp:Label>
                                            </td>

                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="lblIIRInconsistent"></asp:Label>
                                            </td>
                                            <td class="tdinput">
                                                <asp:Label runat="server" ID="lblIIRTotalConfirm" Text="" ClientIDMode="Static"></asp:Label></td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, AnomalousClaims%>" /></td>
                                <td colspan="3" class="fontBold">

                                    <asp:Label runat="server" ID="lblIIRAnomalous" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, OverMajorIrregularities%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label runat="server" ID="lblIIROverMajor" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, CheckingDate%>" /></td>
                                <td colspan="3" class="fontBold">
                                    <asp:Label runat="server" ID="lblIIRCheckingDate"></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <div class="headingText">
                            <asp:Label ID="Label31" runat="server" Text="<%$ Resources: Text, ActionOptional%>"></asp:Label>
                        </div>
                        <table class="commonTable">
                            <tr>
                                <td style="width: 250px" valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, ActionTaken%>" ID="Literal111" /></td>
                                <td colspan="3">
                                    <asp:Label runat="server" Text="<%$ Resources: Text, NA%>" ID="lblFurtherActionConfirmEmpty" Visible="false" CssClass="fontBold" />
                                    <table cellpadding="0" cellspacing="0" class="actiontobetakenshow">
                                        <asp:Panel runat="server" ID="headerFurtherActionConfirm" Visible="true">
                                            <tr>
                                                <td class="tdheader" style="width: 150px">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Action%>" /></td>
                                                <td class="tdheader">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Type%>" /></td>
                                                <td class="tdheader" style="width: 150px">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Date%>" /></td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Repeater ID="FurtherActionConfirm" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <%#IIf(Eval("Rowspan") > 0, "<td  valign=""top"" rowspan=" + Convert.ToInt16(Eval("Rowspan")).ToString() + " style=""background-color:ivory"">" + Eval("Action") + "</td>", "")%>
                                                    <td>
                                                        <%#Eval("Type")%>
                                                    </td>
                                                    <td>
                                                        <%#Eval("Date")%>
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                    <br />
                                </td>
                            </tr>

                            <tr>
                                <td valign="top">
                                    <asp:Literal runat="server" Text="<%$ Resources: Text, FollowUpAction%>" /></td>
                                <td colspan="3">
                                    <asp:Label runat="server" Text="<%$ Resources: Text, NA%>" ID="lblFollowUpActionConfirmEmpty" Visible="false" CssClass="fontBold" />
                                    <table cellpadding="0" cellspacing="0" class="actiontobetakenshow">
                                        <asp:Panel runat="server" ID="headerFollowUpActionConfirm" Visible="true">
                                            <tr>
                                                <td class="tdheader" style="width: 50px" align="center"></td>
                                                <td class="tdheader" style="">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Action%>" /></td>
                                                <td class="tdheader" style="width: 150px">
                                                    <asp:Literal runat="server" Text="<%$ Resources: Text, Date%>" /></td>
                                            </tr>
                                        </asp:Panel>
                                        <asp:Repeater ID="repFollowUpActionConfirm" runat="server">
                                            <ItemTemplate>
                                                <tr>
                                                    <td><%#Eval("Followup_Action_Seq")%>
                                                    </td>
                                                    <td>
                                                        <%# Eval("Action_Desc").ToString()%>                                                        
                                                    </td>
                                                    <td>
                                                        <%# Eval("Action_Date_Format")%>                                             
                                                    </td>
                                                </tr>
                                            </ItemTemplate>
                                        </asp:Repeater>
                                    </table>
                                </td>
                            </tr>
                        </table>
                        <!--bottom button-->
                        <div class="bottomBtnBox">
                            <span style="position: absolute; left: 0">
                                <asp:ImageButton ID="ibtnIIRConfirmBack" runat="server" ImageUrl="<%$ Resources: ImageUrl,BackBtn%>" /></span>
                            <span>
                                <asp:ImageButton ID="ibtnIIRConfirmSave" runat="server" ImageUrl="<%$ Resources: ImageUrl,ConfirmBtn%>" /></span>
                        </div>
                    </div>
                </asp:View>

                <!--Update Inspection Result Successfully View-->
                <asp:View ID="vViewUpdateIIRSuccess" runat="server">
                    <cc2:InfoMessageBox ID="InfoMessageBoxUpdateIIRSuccess" runat="server" Width="95%" />
                    <span>
                        <asp:ImageButton ID="ibtnReturnIIRSuccess" runat="server" ImageUrl="<%$ Resources: ImageUrl,ReturnBtn%>" />
                    </span>
                </asp:View>
            </asp:MultiView>
            <asp:Button ID="btnCloseCase" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupConfirmCloseCase" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnCloseCase" PopupControlID="panPopupConfirmCloseCase" PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupConfirmCloseCase" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmCloseCase" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources: AlternateText, ConfirmCloseCase%>" />
            </asp:Panel>

            <asp:Button ID="btnRemove" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupConfirmRemove" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnRemove" PopupControlID="panPopupConfirmRemove" PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel Style="display: none" ID="panPopupConfirmRemove" runat="server" Width="500px">
                <uc2:ucNoticePopUp ID="ucNoticePopUpConfirmRemove" runat="server" NoticeMode="Confirmation" ButtonMode="YesNo" MessageAlignment="Center" MessageText="<%$ Resources: AlternateText, ConfirmRemoveCase%>" />
            </asp:Panel>

            <asp:Button ID="btnReopen" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="ModalPopupConfirmReopen" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnReopen" PopupControlID="panPopupConfirmReopen" PopupDragHandleControlID="" RepositionMode="None">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panPopupConfirmReopen" runat="server" Style="display: none;">
                <asp:Panel ID="panApproveHeading" runat="server" Style="cursor: move;">

                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label62" runat="server" Text="<%$ Resources:Text, Confirmation %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #ffffff">
                            <table style="width: 100%">
                                <tr>
                                    <td colspan="2">
                                        <asp:Panel ID="pnlPopMessage" runat="server">
                                            <cc2:InfoMessageBox ID="reopenInfoMsgBox" runat="server" Width="95%" />
                                            <cc2:MessageBox ID="reopenMsgBox" runat="server" Width="95%" />
                                        </asp:Panel>
                                    </td>

                                </tr>
                                <tr style="height: 50px;">
                                    <td style="width: 120px;">
                                        <asp:Label runat="server" Text="<%$ Resources: Text, ReopenReason%>"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtReopenReason" Width="400px" Columns="3"></asp:TextBox>
                                        <asp:Image ID="imgtxtReopenReasonErr" runat="server" ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>" Visible="False" Style="vertical-align: middle;" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnReopenConfirmPopup" runat="server" ImageUrl="<%$ Resources:ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, ConfirmBtn %>" OnClick="ibtnReopenConfirmPopup_Click" />
                                        <asp:ImageButton ID="ibtnReopenCancel" runat="server" ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources:AlternateText, CancelBtn %>" OnClick="ibtnReopenCancel_Click" /></td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px; height: 7px"></td>
                    </tr>
                </table>
            </asp:Panel>

            <asp:Button Style="display: none" ID="btnHiddenDownload" runat="server" />
            <cc1:ModalPopupExtender ID="mpeDownload" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenDownload" PopupControlID="pnlDownload" RepositionMode="None"
                PopupDragHandleControlID="pnlDownloadHeading" />
            <asp:Panel ID="pnlDownload" runat="server" Style="display: none">
                <div id="ctl00_ContentPlaceHolder1_pnlDownloadHeading" style="cursor: move">

                    <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="padding-left: 2px; font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDownloadTitle" runat="server" Text="<%$ Resources:Text, DownloadLatestReport %>" />
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px; position: relative; left: -2px"></td>
                        </tr>
                    </table>

                </div>

                <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="background-color: #ffffff">
                            <br />
                            <cc2:MessageBox ID="udcDownloadErrorMessage" runat="server" Width="800px" />
                            <cc2:InfoMessageBox ID="udcDownloadInfoMessage" runat="server" Width="800px" />
                            <table cellpadding="0" cellspacing="0" style="width: 800px">
                                <tr>
                                    <td style="width: 150px; height: 36px" valign="top">
                                        <asp:Label ID="lblReportTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ReportType %>" />
                                    </td>
                                    <td style="height: 36px" valign="top">
                                        <asp:Label ID="lblReportType" runat="server" CssClass="tableText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 36px" valign="top">
                                        <asp:Label ID="lblReportNameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ReportName %>" />
                                    </td>
                                    <td style="height: 36px" valign="top">
                                        <asp:Label ID="lblReportName" runat="server" CssClass="tableText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 36px" valign="top">
                                        <asp:Label ID="lblNewPassword" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, SetPassword %>" />
                                    </td>
                                    <td valign="top">
                                        <table cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td align="left" valign="top">
                                                    <asp:TextBox ID="txtNewPassword" runat="server" MaxLength="15" TextMode="Password" Width="200px" />
                                                    <asp:Image ID="imgErrNewPassword" runat="server" AlternateText="<%$ Resources:AlternateText, ErrorBtn %>"
                                                        ImageUrl="<%$ Resources:ImageUrl, ErrorBtn %>" Visible="False" />
                                                </td>
                                                <td valign="top">
                                                    <table cellpadding="0" style="width: 290px">
                                                        <tr>
                                                            <td colspan="5">
                                                                <div id="progressBar" style="border-right: white 1px solid; border-top: white 1px solid; font-size: 1px; border-left: white 1px solid; width: 290px; border-bottom: white 1px solid; height: 10px">
                                                                </div>
                                                            </td>
                                                        </tr>
                                                        <tr style="width: 290px">
                                                            <td align="center" style="width: 30%">
                                                                <span id="strength1"></span>
                                                            </td>
                                                            <td style="width: 5%">
                                                                <span id="direction1"></span>
                                                            </td>
                                                            <td align="center" style="width: 30%">
                                                                <span id="strength2"></span>
                                                            </td>
                                                            <td style="width: 5%">
                                                                <span id="direction2"></span>
                                                            </td>
                                                            <td align="center" style="width: 30%">
                                                                <span id="strength3"></span>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" />
                                    <td valign="top">
                                        <asp:Label ID="lblFilePasswordTipsHeader" runat="server" Text="<%$ Resources:Text, FileDownloadPasswordTips %>" />
                                        <br />
                                        <asp:Label ID="lblFilePasswordTips1" runat="server" Text="<%$ Resources:Text, WebPasswordTips1-3Rule %>" />
                                        <br />
                                        &nbsp; &nbsp;<asp:Label ID="lblFilePasswordTips1a" runat="server" Text="<%$ Resources:Text, WebPasswordTips1a %>" />
                                        <br />
                                        &nbsp; &nbsp;<asp:Label ID="lblFilePasswordTips1b" runat="server" Text="<%$ Resources:Text, WebPasswordTips1b %>" />
                                        <br />
                                        &nbsp; &nbsp;<asp:Label ID="lblFilePasswordTips1c" runat="server" Text="<%$ Resources:Text, WebPasswordTips1c %>" />
                                        <br />
                                        &nbsp; &nbsp;<asp:Label ID="lblFilePasswordTips1d" runat="server" Text="<%$ Resources:Text, WebPasswordTips1d %>" />
                                        <br />
                                        <asp:Label ID="lblFilePasswordTips2" runat="server" Text="<%$ Resources:Text, FilePasswordTips2 %>" />
                                        <br />
                                        <asp:Label ID="lblFilePasswordTips3" runat="server" Text="<%$ Resources:Text, WebPasswordTips3 %>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center" valign="top" style="padding-top: 15px; padding-bottom: 10px">

                                        <asp:ImageButton ID="ibtnDownload" name="ibtnDownload" runat="server" ImageUrl="../Images/button/btn_download.png" alt="Download" Style="border-width: 0px;" />
                                        &nbsp;&nbsp;&nbsp;  
                                        <asp:ImageButton ID="ibtnDownloadClose" name="ibtnDownloadClose" OnClick="ibtnDownloadClose_Click" runat="server" ImageUrl="../Images/button/btn_close.png" alt="Close" Style="border-width: 0px;" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position: relative; left: -2px"></td>
                    </tr>
                </table>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
