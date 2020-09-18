<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="VaccinationFileManagement.aspx.vb" Inherits="HCSP.VaccinationFileManagement"
    Title="<%$ Resources: Title, VaccinationFileManagement %>" EnableEventValidation="False" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="../UIControl/VaccinationFile/ucVaccinationFileDetail.ascx" TagName="ucVaccinationFileDetail" TagPrefix="uc1" %>
<%@ Register Src="../UIControl/ucInputDocumentType.ascx" TagName="ucInputDocumentType" TagPrefix="uc2" %>
<%@ Register Src="../UIControl/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumnetType" TagPrefix="uc3" %>
<%@ Register Src="../UIControl/ChooseCCCode.ascx" TagName="ChooseCCCode" TagPrefix="uc4" %>
<%@ Register Src="../UIControl/SchemeDocTypeLegend.ascx" TagName="SchemeDocTypeLegend" TagPrefix="uc5" %>
<%@ Register Src="../UIControl/VaccinationFile/ucPreCheckDetail.ascx" TagName="ucPreCheckDetail" TagPrefix="uc6" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../JS/Common.js"></script>

    <script type="text/javascript">
        function FileDownload(url) {
            window.open(url,"_blank","left=0,top=0,width=0,height=0");
        }

        function SelectAll(chk, chkValue, grid_id) {

            var chkList = chk.parentNode.parentNode.parentNode;
            var chks = chkList.getElementsByTagName("input");

            for (var i = 0; i < chks.length; i++) {

                if (chks[i] != chk && chk.checked) {
                    chks[i].checked = false;
                }
            }

            var objGrid = document.getElementById(grid_id)
            for (i = 0; i < objGrid.rows.length; i++) {
                // skip header row
                if (i == 0) { continue; }
                
                var inputList = objGrid.rows[i].getElementsByTagName("input");
                for (var k = 0; k < inputList.length; k++) {

                    if (inputList[k].type == "radio" || (inputList[k].type == "checkbox")) {
                        if (chk.checked) {
                            //If the header checkbox is checked
                            //check all checkboxes
                            if (inputList[k].value == chkValue) {
                                inputList[k].checked = true;
                            }
                            else {
                                inputList[k].checked = false;
                            }
                        }
                    }
                }
            }
  
        }

        function SelectAllYes(chkY_id,chkN_id) {
            //function SelectAllYes(id) {
            var objY = document.getElementById(chkY_id)
            var objN = document.getElementById(chkN_id)
            //alert(obj.id + ',' + obj.checked);

            var frm = document.forms[0];
            
            for (i = 0; i < frm.elements.length; i++) {
                if (frm.elements[i].type == "radio") {
                    //alert(frm);
                    if (objY.checked) {
                        if (frm.elements[i].value == "Y") {
                            frm.elements[i].checked = true;
                            //frm.elements[i].nextSibling.style.color = "red";
                        }      
                    }
                    //else{
                    //    frm.elements[i].checked = obj.checked;
                    //}
                }
            }

            objN.checked = false
        }

        function SelectAllNo(chkY_id,chkN_id) {
            //function SelectAllYes(id) {
            var objY = document.getElementById(chkY_id)
            var objN = document.getElementById(chkN_id)
            //alert(obj.id + ',' + obj.checked);

            var frm = document.forms[0];

            for (i = 0; i < frm.elements.length; i++) {
                if (frm.elements[i].type == "radio") {
                    //alert(frm);
                    if (objN.checked) {
                        if (frm.elements[i].value == "N") {
                            frm.elements[i].checked = true;
                        }
                    }
                    //else{
                    //    frm.elements[i].checked = obj.checked;
                    //}
                }
            }

            objY.checked = false
        }

        function ShowSaved() {
            setTimeout(function () { $find('mpeSavedBehavior').hide(); }, 1500);
            return false;
        }

    </script>
    <%--<cc1:ToolkitScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600" />--%>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, VaccinationFileManagementBanner %>"
        AlternateText="<%$ Resources: AlternateText, VaccinationFileManagementBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 4px"></div>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="950px" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="950px" />
            <asp:MultiView ID="mvCore" runat="server" ActiveViewIndex="-1">
                <asp:View ID="vSearch" runat="server">
                    <table style="width: 100%" class="tblS">
                        <tr>
                            <td>
                                <asp:Label ID="lblSFileTypeText" runat="server" Text="<%$ Resources: Text, FileType %>" />
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblFileType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="rblFileType_SelectedIndexChanged"
                                     Width="300" RepeatColumns ="2" Style="position:relative;left:-3px;top:1px" />
                                <asp:Label ID="lblFileType" runat="server" CssClass="tableText" Text="<%$ Resources: Text, VaccinationFile %>" Visible="false" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSSchemeText" runat="server" Text="<%$ Resources: Text, Scheme %>"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSScheme" runat="server" Width="350" AutoPostBack ="true" OnSelectedIndexChanged="ddlSScheme_SelectedIndexChanged" />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 160px">
                                <asp:Label ID="lblSVaccinationFileIDText" runat="server" Text="<%$ Resources: Text, VaccinationFileID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSVaccinationFileID" runat="server" Width="200" MaxLength="15"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSCodeText" runat="server"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSCode" runat="server" Width="200" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trSVaccinationDate" runat="server">
                            <td>
                                <asp:Label ID="lblSVaccinationDate" runat="server" Text="<%$ Resources: Text, VaccinationDate %>"></asp:Label>
                            </td>
                            <td>
                                <table cellpadding="0" cellspacing="0">
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtSVaccinationDateFrom" runat="server" Width="80" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnSVaccinationDateFrom" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                            <cc1:CalendarExtender ID="calSVaccinationDateFrom" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnSVaccinationDateFrom"
                                                TargetControlID="txtSVaccinationDateFrom" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="fteSVaccinationDateFrom" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="txtSVaccinationDateFrom" ValidChars="-">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Image ID="imgErrorSVaccinationDateFrom" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                        </td>
                                        <td style="padding-left: 15px; padding-right: 15px">
                                            <asp:Label ID="lblSTo" runat="server" Text="<%$ Resources: Text, To %>"></asp:Label>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSVaccinationDateTo" runat="server" Width="80" MaxLength="10"></asp:TextBox>
                                            <asp:ImageButton ID="ibtnSVaccinationDateTo" runat="server" ImageUrl="<%$ Resources:ImageUrl, CalenderBtn %>"
                                                AlternateText="<%$ Resources:AlternateText, CalenderBtn %>" Style="position: relative; top: 3px" />
                                            <cc1:CalendarExtender ID="calSVaccinationDateTo" CssClass="ajax_cal" runat="server" PopupButtonID="ibtnSVaccinationDateTo"
                                                TargetControlID="txtSVaccinationDateTo" Format="dd-MM-yyyy" TodaysDateFormat="d MMMM, yyyy">
                                            </cc1:CalendarExtender>
                                            <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" FilterType="Custom, Numbers"
                                                TargetControlID="txtSVaccinationDateTo" ValidChars="-">
                                            </cc1:FilteredTextBoxExtender>
                                        </td>
                                        <td>
                                            <asp:Image ID="imgErrorSVaccinationDateTo" runat="server" ImageUrl="<%$ Resources: ImageUrl, ErrorBtn %>"
                                                AlternateText="<%$ Resources: AlternateText, ErrorBtn %>" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSStatusText" runat="server" Text="<%$ Resources: Text, Status %>"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSStatus" runat="server" Width="350"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr style="height: 30px"></tr>
                        <tr>
                            <td colspan="2" style="text-align: center">
                                <asp:ImageButton ID="ibtnSSearch" runat="server" ImageUrl="<%$ Resources: ImageUrl, SearchBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SearchBtn %>" OnClick="ibtnSSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vResult" runat="server">
                    <asp:GridView ID="gvR" runat="server" CssClass="gvTable" AutoGenerateColumns="False" AllowPaging="True" Width="1330px"
                        AllowSorting="True" OnRowDataBound="gvR_RowDataBound" OnPreRender="gvR_PreRender"
                        OnRowCommand="gvR_RowCommand" OnSorting="gvR_Sorting" OnPageIndexChanging="gvR_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationFileID %>" ItemStyle-Width="130" >
                                <ItemTemplate>
                                    <asp:Label ID="lblRVaccinationFileID" runat="server" Text='<%# Eval("Student_File_ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, SchoolCode %>" ItemStyle-Width="150" ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblRCode" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, DoseToInject %>" ItemStyle-Width="100" ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblRDoseToInject" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, UploadDate %>" ItemStyle-Width="120" ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblRUploadDate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Rectification %>" ItemStyle-Width="120" ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblRRectificationDate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationReportGenerationDate %>" ItemStyle-Width="120" ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblRVaccinationReportGenerationDate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationDate %>" ItemStyle-Width="120" ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblRVaccinationDate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, CreateClaim %>" ItemStyle-Width="120" ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblRCreateClaimDate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Status %>" ItemStyle-Width="100"  ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblRStatus" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, DownloadLatestReport %>" ItemStyle-Width="222"  ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblRDownloadLatestReport" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>              
                    <table>
                        <tr style="height: 20px"></tr>
                        <tr>
                            <td style="width: 300px">
                                <asp:ImageButton ID="ibtnRBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnRBack_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vDetail" runat="server">
                    <uc1:ucVaccinationFileDetail ID="udcVaccinationFileDetail" runat="server"></uc1:ucVaccinationFileDetail>
                    <table>
                        <tr style="height: 20px" >
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td style="width: 200px">
                                <asp:ImageButton ID="ibtnDBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                            </td>
                            <td style="width: 600px">
                                <div ID="divSaveCurrentPage" runat="server" style="display:inline-block;position:relative;top:2px">
                                    <asp:ImageButton ID="ibtnDSaveCurrentPage" runat="server" ImageUrl="<%$ Resources: ImageUrl, SaveCurrentPageBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SaveCurrentPageBtn %>" OnClick="ibtnDSaveCurrentPage_Click" />&nbsp;&nbsp;
                                </div>
                                <div ID="divSummary" runat="server" style="display:inline-block;position:relative;top:2px">
                                    <asp:ImageButton ID="ibtnDSummary" runat="server" ImageUrl="<%$ Resources: ImageUrl, SummaryBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SummaryBtn %>" OnClick="ibtnSummary_Click" />&nbsp;&nbsp;
                                </div>
                                <div ID="divConfirmClaim" runat="server" style="display:inline-block;position:relative;top:2px">
                                    <asp:ImageButton ID="ibtnDConfirmClaim" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmClaimBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ConfirmClaimBtn %>" OnClick="ibtnConfirmClaim_Click" />
                                </div>
                            </td>
                        </tr>
                    </table>                               
                </asp:View>
                <asp:View ID="vFinish" runat="server">
                    <asp:ImageButton ID="ibtnFReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                        AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="ibtnFReturn_Click" />
                </asp:View>
                <asp:View ID="vPreCheck" runat="server">
                    <asp:GridView ID="gvP" runat="server" CssClass="gvTable" AutoGenerateColumns="False" AllowPaging="True" Width="1100px"
                        AllowSorting="True" OnRowDataBound="gvP_RowDataBound" OnPreRender="gvP_PreRender"
                        OnRowCommand="gvP_RowCommand" OnSorting="gvP_Sorting" OnPageIndexChanging="gvP_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationFileID %>" ItemStyle-Width="0">
                                <ItemTemplate>
                                    <asp:Label ID="lblPVaccinationFileID" runat="server" Text='<%# Eval("Student_File_ID") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, RCHCode %>" ItemStyle-Width="0" ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblPCode" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, UploadDate %>" ItemStyle-Width="0" ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblPUploadDate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Rectification %>" ItemStyle-Height="0">
                                <ItemTemplate>
                                    <asp:Label ID="lblPRectificationDate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
<%--                            <asp:TemplateField HeaderText="<%$ Resources: Text, CreateClaim %>" ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblPCreateClaimDate" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Status %>" ItemStyle-Width="0"  ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblPStatus" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, DownloadLatestReport %>" ItemStyle-Width="110"  ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblPDownloadLatestReport" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>              
                    <table>
                        <tr style="height: 20px"></tr>
                        <tr>
                            <td style="width: 300px">
                                <asp:ImageButton ID="ibtnPBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnPBack_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vPreCheckDetail" runat="server">
                    <uc6:ucPreCheckDetail ID="udcPreCheckDetail" runat="server"></uc6:ucPreCheckDetail>
                    <table>
                        <tr style="height: 20px" >
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td style="width: 260px">
                                <asp:ImageButton ID="ibtnPDBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnPDBack_Click" />
                            </td>
                            <td style="width: 600px">
                                <div ID="divPDSaveCurrentPage" runat="server" style="display:inline-block;position:relative;top:2px">
                                    <asp:ImageButton ID="ibtnPDSaveCurrentPage" runat="server" ImageUrl="<%$ Resources: ImageUrl, SaveCurrentPageBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SaveCurrentPageBtn %>" OnClick="ibtnPDSaveCurrentPage_Click" />&nbsp;&nbsp;
                                </div>
                                <div ID="divPDSave" runat="server" style="display:inline-block;position:relative;top:2px">
                                    <asp:ImageButton ID="ibtnPDSave" runat="server" ImageUrl="<%$ Resources: ImageUrl, SaveBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SummaryBtn %>" OnClick="ibtnPDSave_Click" />&nbsp;&nbsp;
                                </div>
                                <div ID="divPDConfirmBatch" runat="server" style="display:inline-block;position:relative;top:2px">
                                    <asp:ImageButton ID="ibtnPDConfirmBatch" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnPDConfirmBatch_Click" />&nbsp;&nbsp;
                                </div>
                                <div ID="divPDSummary" runat="server" style="display:inline-block;position:relative;top:2px">
                                    <asp:ImageButton ID="ibtnPDSummary" runat="server" ImageUrl="<%$ Resources: ImageUrl, SummaryBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SummaryBtn %>" OnClick="ibtnPDSummary_Click" />
                                </div>
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>

            <%-- Pop up for Show Download --%>
            <asp:Panel ID="pnlDownload" runat="server" Style="display: none">
                <asp:Panel ID="pnlDownloadHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="padding-left:2px;font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDownloadTitle" runat="server" Text="<%$ Resources:Text, DownloadLatestReport %>" />
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px; position:relative; left: -2px"></td>
                        </tr>
                    </table>
                </asp:Panel>
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
                                        <asp:Label ID="lblReportTypeText" runat="server" CssClass="tableTitle" text="<%$ Resources:Text, ReportType %>" />
                                    </td>
                                    <td style="height: 36px" valign="top">
                                        <asp:Label ID="lblReportType" runat="server" CssClass="tableText" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="height: 36px" valign="top">
                                        <asp:Label ID="lblReportNameText" runat="server" CssClass="tableTitle" text="<%$ Resources:Text, ReportName %>" />
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
                                                                <div id="progressBar" style="border-right: white 1px solid; border-top: white 1px solid;
                                                                    font-size: 1px; border-left: white 1px solid; width: 290px; border-bottom: white 1px solid;
                                                                    height: 10px">
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
                                    <td colspan="2" align="center" valign="top" style="padding-top:15px;padding-bottom:10px">
                                        <asp:ImageButton ID="ibtnDownloadClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnRDownloadPopupClose_Click" />
                                        &nbsp;&nbsp;&nbsp;            
                                        <asp:ImageButton ID="ibtnDownload" runat="server" AlternateText="<%$ Resources:AlternateText, DownloadBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, DownloadBtn %>" OnClick="ibtnRDownloadPopupDownload_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button Style="display: none" ID="btnHiddenDownload" runat="server" />
            <cc1:ModalPopupExtender ID="mpeDownload" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                    TargetControlID="btnHiddenDownload" PopupControlID="pnlDownload" RepositionMode="None"
                    PopupDragHandleControlID="pnlDownloadHeading" />
            <%-- End of Pop up for Show Download --%>

            <%-- Pop up for Show File Record Edit --%>
            <asp:Panel ID="pnlAcctEdit" runat="server" Style="display: none">
                <asp:Panel ID="pnlAcctEditHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="padding-left:2px;font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblAcctEditTitle" runat="server" Text="<%$ Resources:Text, RectifyVRAcctInfo %>" />
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px; position:relative; left: -2px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td align="center" style="background-color: #ffffff">
                            <br />
                            <cc2:MessageBox ID="udcAcctEditErrorMessage" runat="server" Width="800px" />
                            <cc2:InfoMessageBox ID="udcAcctEditInfoMessage" runat="server" Width="800px" />
                            <table cellpadding="0" cellspacing="0" style="width: 800px">                                        
                                <tr>
                                    <td>
                                        <asp:Label ID="lblRectifyRecipientDetail" runat="server" CssClass="eHSTableHeading" Text="<%$ Resources:Text, ClassAndStudentInformation %>" Style="position:relative;left:-5px;padding-left:20px" />
                                        <br />
                                        <br />
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td style="width: 200px" valign="top" class="tableCellStyle">
                                                    <asp:Label ID="lblRectifyClassNameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ClassName %>"></asp:Label>
                                                </td>
                                                <td valign="top">
                                                    <asp:Label ID="lblClassName" runat="server" CssClass="tableText" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 200px" valign="top" class="tableCellStyle">
                                                    <asp:Label ID="lblRectifyClassNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ClassNo %>"></asp:Label>
                                                </td>
                                                <td valign="top">
                                                    <asp:Label ID="lblClassNo" runat="server" CssClass="tableText" />
                                                </td>
                                            </tr>
                                           <tr>
                                                <td style="width: 200px" valign="top" class="tableCellStyle">                                                   
                                                    <asp:Label ID="lblRectifyChiNameText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ChineseName %>" />
                                                    <span style="font-size:16px">(</span>
                                                    <asp:Label ID="lblRectifyUploadText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Upload %>" />
                                                    <span style="font-size:16px">)</span>
                                                </td>
                                                <td valign="top">
                                                    <asp:Label ID="lblChiName" runat="server" CssClass="tableText" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 200px" valign="top" class="tableCellStyle">
                                                    <asp:Label ID="lblRectifyContactNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ContactNo2 %>"></asp:Label>
                                                </td>
                                                <td valign="top">
                                                    <%--<asp:label ID="lblRectifyContactNo" runat="server" CssClass="tableText" Visible="false"/>--%>
                                                    <asp:textbox ID="txtRectifyContactNo" runat="server" Width="80" MaxLength="20" />
                                                </td>
                                            </tr>
                                            <tr ID="trConfirmNotToInject" runat="server">
                                                <td style="width: 200px" valign="top" class="tableCellStyle">
                                                    <asp:Label ID="lblRectifyConfirmNotToInjectText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ConfirmToInject %>" />
                                                </td>
                                                <td valign="top">
                                                    <%--<asp:label ID="lblRectifyConfirmNotToInject" runat="server" CssClass="tableText" Visible="false"/>--%>
                                                    <asp:checkbox ID="chkRectifyConfirmNotToInject" runat="server" Style="position:relative;left:-3px;top:2px" />
                                                </td>
                                            </tr>
                                        </table>                                                                                                     
                                        <br />
                                        <table border="0" cellpadding="0" cellspacing="0">
                                            <tr style="vertical-align:top">
                                                <td id="tdAcctInfo" runat="server">
                                                    <asp:Label ID="lblRectifyAcct" runat="server" CssClass="eHSTableHeading" Style="position:relative;left:-5px;padding-left:20px" />&nbsp;
                                                    <asp:ImageButton ID="ibtnRectifyAcctInputTips" runat="server" ImageAlign="AbsMiddle" OnClick ="ibtnRectifyAcctInputTips_Click" />
                                                    <br />
                                                    <br />
                                                    <asp:panel ID="pnlModifyAcct" runat="server" Visible="false">
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <asp:Panel ID="pnlRefNo" runat="server" Visible ="false">
                                                            <tr>
                                                                <td style="width: 200px" valign="top" class="tableCellStyle" style="height:19px;padding-bottom:5px">
                                                                    <asp:Label ID="lblRectifyRefNoText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, RefNo %>"></asp:Label>
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblRectifyRefNo" runat="server" CssClass="tableText" />
                                                                    <br />
                                                                    <asp:Label ID="lblCreateByOtherSPText" runat="server" CssClass="tableText"/>
                                                                </td>
                                                            </tr>
                                                            </asp:Panel>
                                                            <tr>
                                                                <td style="width: 200px" valign="top" class="tableCellStyle" style="height:19px;padding-bottom:5px">
                                                                    <asp:Label ID="lblRectifyDocTypeText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, DocumentType %>"></asp:Label>
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblRectifyDocType" runat="server" CssClass="tableText" />
                                                                </td>
                                                            </tr>
                                                        </table>                                               
                                                        <uc2:ucInputDocumentType ID="udcRectifyAccount" runat="server" Visible="true" />
                                                    </asp:panel>
                                                    <asp:panel ID="pnlReadOnlyAcct" runat="server" Visible="false">
                                                        <table border="0" cellpadding="0" cellspacing="0">
                                                            <tr>
                                                                <td style="width: 200px" valign="top" class="tableCellStyle" style="height:19px;padding-bottom:5px">
                                                                    <asp:Label ID="lblRectifyAccountIDText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, AccountID %>"></asp:Label>
                                                                </td>
                                                                <td valign="top">
                                                                    <asp:Label ID="lblRectifyAccountID" runat="server" CssClass="tableText" />
                                                                </td>
                                                            </tr>
                                                        </table>   
                                                        <uc3:ucReadOnlyDocumnetType ID="udcReadOnlyAccount" runat="server" Visible="true" />
                                                    </asp:panel>
                                                    <asp:Panel ID="pnlRecordStatus" runat="server" Visible ="false">
                                                    <table border="0" cellpadding="0" cellspacing="0">
                                                        <tr>
                                                            <td style="width: 200px" valign="top" class="tableCellStyle">
                                                                <asp:Label ID="lblRecordStatusText" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, Status %>"></asp:Label>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:Label ID="lblRecordStatus" runat="server" CssClass="tableText" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </asp:Panel>
                                                </td>
                                                <td id="tdFieldDiff" runat="server">
                                                    <asp:panel ID="pnlDiffUploadInfo" runat="server"  Visible="false">
                                                        <asp:Label ID="lblDiffUploadInfo" runat="server" CssClass="eHSTableHeading" Text="<%$ Resources:Text, DiffUploadInfo %>" Style="position:relative;left:-5px;padding-left:20px" />
                                                        <br />
                                                        <br />
                                                        <div ID="divFieldDiff1" runat="server" style="position:relative;top:72px">
                                                            <asp:Label ID="lblFieldDiff1" runat="server" Text ="Field1" CssClass="tableText" style="font-size:16px" />
                                                        </div>
                                                        <div ID="divFieldDiff2" runat="server" style="position:relative;top:80px">
                                                            <asp:Label ID="lblFieldDiff2" runat="server" Text ="Field2" CssClass="tableText" style="font-size:16px" />
                                                        </div>
                                                        <div ID="divFieldDiff3" runat="server" style="position:relative;top:88px">
                                                            <asp:Label ID="lblFieldDiff3" runat="server" Text ="Field3" CssClass="tableText" style="font-size:16px" />
                                                        </div>
                                                        <div ID="divFieldDiff4" runat="server" style="position:relative;top:96px">
                                                            <asp:Label ID="lblFieldDiff4" runat="server" Text ="Field4" CssClass="tableText" style="font-size:16px" />
                                                        </div>
                                                        <div ID="divFieldDiff5" runat="server" style="position:relative;top:104px">
                                                            <asp:Label ID="lblFieldDiff5" runat="server" Text ="Field5" CssClass="tableText" style="font-size:16px" />
                                                        </div>
                                                        <div ID="divFieldDiff6" runat="server" style="position:relative;top:112px">
                                                            <asp:Label ID="lblFieldDiff6" runat="server" Text ="Field6" CssClass="tableText" style="font-size:16px" />
                                                        </div>
                                                    </asp:panel>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr id="trConfirmEHSAccount" runat="server">
                                    <td >
                                        <br />
                                        <div class="checkboxStyle" style="position:relative;height:42px">
                                            <asp:CheckBox ID="chkConfirmEHSAccount" runat="server" AutoPostBack="false" />
                                            <asp:Label ID="lblConfirmStatement1" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, FieldDiffConfirmStatement1 %>" />
                                            <br />
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label ID="lblConfirmStatement2" runat="server" CssClass="tableTitle" 
                                                Text="<%$ Resources:Text, FieldDiffConfirmStatement2 %>" Style="position:relative;top:3px" />
                                            <br />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center;vertical-align:top;padding-top:15px;padding-bottom:10px">
                                        <asp:ImageButton ID="ibtnEditAcctCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnEditAcctCancel_Click" />
                                        &nbsp;&nbsp;&nbsp;            
                                        <asp:ImageButton ID="ibtnEditAcctSave" runat="server" AlternateText="<%$ Resources:AlternateText, SaveBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, SaveBtn %>" OnClick="ibtnEditAcctSave_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button Style="display: none" ID="btnHiddenAcctEdit" runat="server" />
            <cc1:ModalPopupExtender ID="mpeAcctEdit" runat="server" PopupControlID="pnlAcctEdit" PopupDragHandleControlID="pnlAcctEditHeading"
                    TargetControlID="btnHiddenAcctEdit" BackgroundCssClass="modalBackgroundTransparent" />
            <%-- End of Pop up for Show File Record Edit --%>

            <%-- Pop up for Show CCCode --%>
            <asp:Panel Style="display: none" ID="panChooseCCCode" runat="server">
                <asp:Panel ID="panChooseCCCodeHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblCCCodeHeading" runat="server" Text="<%$ Resources:Text, ChooseCCCodeHeading %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px; position:relative; left: -2px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 500px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="center">
                            <uc4:ChooseCCCode ID="udcCCCode" runat="server"></uc4:ChooseCCCode>
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
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button Style="display: none" ID="btnHiddenCCCode" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="ModalPopupExtenderChooseCCCode" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenCCCode" PopupControlID="panChooseCCCode" RepositionMode="None"
                PopupDragHandleControlID="panChooseCCCodeHeading" />
            <%-- End of Pop up for Show CCCode --%>

            <%-- Pop up for Show Document Type Selection --%>
            <asp:Panel ID="pnlDocTypeSelection" runat="server" Style="display: none">
                <asp:Panel ID="pnlDocTypeSelectionHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblDocumentType" runat="server" Text="<%$ Resources:Text, DocumentType %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px; position:relative; left: -2px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="center">
                            <br />
                            <cc2:MessageBox ID="udcDocTypeSelectionErrorMessage" runat="server" Width="800px" />
                            <cc2:InfoMessageBox ID="udcDocTypeSelectionInfoMessage" runat="server" Width="800px" />
                            <table cellpadding="0" cellspacing="0" style="width: 800px">                                        
                                <tr>
                                    <td>
                                        <cc2:DocumentTypeRadioButtonGroup ID="udcDocumentTypeRadioButtonGroup" runat="server"
                                            HeaderCss="eHSTableHeading" AutoPostBack="true" HeaderText="<%$ Resources:Text, DocumentType%>"
                                            LegendImageURL="<%$ Resources:ImageUrl, Infobtn%>" LegendImageALT="<%$ Resources:Text, AcceptedDocList%>" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center;vertical-align:top;padding-top:15px;padding-bottom:10px">
                                        <asp:ImageButton ID="ibtnDocTypeSelectionCancel" runat="server" AlternateText="<%$ Resources:AlternateText, CancelBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CancelBtn %>" OnClick="ibtnDocTypeSelectionCancel_Click" />
                                        &nbsp;&nbsp;&nbsp;            
                                        <asp:ImageButton ID="ibtnDocTypeSelectionNext" runat="server" AlternateText="<%$ Resources:AlternateText, NextBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, NextBtn %>" OnClick="ibtnDocTypeSelectionNext_Click" />
                                    </td>
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
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button Style="display: none" ID="btnHiddenDocTypeSelection" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="mpeDocTypeSelection" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenDocTypeSelection" PopupControlID="pnlDocTypeSelection" RepositionMode="None"
                PopupDragHandleControlID="pnlDocTypeSelectionHeading" />
            <%-- End of Pop up for Show Document Type Selection --%>

            <%-- Pop up for Show Scheme Document Type Legend --%>
            <asp:Panel Style="display: none" ID="panSchemeDocTypeLegend" runat="server">
                <asp:Panel ID="panSchemeDocTypeLegendHeading" runat="server" Style="cursor: move;">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 920px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblSchemeDocTypeLegnedHeading" runat="server" Text="<%$ Resources:Text, AcceptedDocList %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 920px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff; padding: 0px 0px 5px 10px" align="left">
                            <asp:Panel ID="panSchemeDocTypeLegnedContent" runat="server" ScrollBars="vertical"
                                Height="550px">
                                <uc5:SchemeDocTypeLegend ID="udcSchemeDocTypeLegend" runat="server" />
                            </asp:Panel>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y">
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td align="center" style="height: 30px; background-color: #ffffff" valign="middle">
                            <asp:ImageButton ID="btnSchemeDocTypeLegnedClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
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
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button Style="display: none" ID="btnHiddenSchemeDocTypeLegend" runat="server"></asp:Button>
            <cc1:ModalPopupExtender ID="mpeSchemeDocTypeLegend" runat="server" BackgroundCssClass="modalBackgroundTransparent" 
                TargetControlID="btnHiddenSchemeDocTypeLegend" PopupControlID="panSchemeDocTypeLegend" RepositionMode="None"
                PopupDragHandleControlID="panSchemeDocTypeLegendHeading" />
            <%-- End of Pop up for Show Scheme Document Type Legend --%>

            <%-- Pop up for Show Document Type Selection --%>
            <asp:Panel ID="pnlClassSummary" runat="server" Style="display: none">
                <asp:Panel ID="pnlClassSummaryHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblClassSummaryHeading" runat="server" Text="<%$ Resources:Text, ClassAndStudentInformation %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px; position:relative; left: -2px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="center">
                            <br />
                            <table cellpadding="0" cellspacing="0" style="width:800px">                                        
                                <tr style="vertical-align:top">
                                    <td>
                                        <div id="divClassSummaryClassAndStudentInformation" runat="server" class="eHSTableHeading">
                                            <asp:Label ID="lblClassSummaryClassAndStudentInformation" runat="server" 
                                                Text="<%$ Resources: Text, ClassAndStudentInformation %>" />
                                        </div>
                                        <table class="tblSFD">
                                            <tr>
                                                <td style="width: 260px;height:20px">
                                                    <asp:Label ID="lblClassSummaryClassNameText" runat="server" Text="<%$ Resources: Text, ClassName %>" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClassSummaryClassName" runat="server" class="tableText" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 260px;height:20px">
                                                    <asp:Label ID="lblClassSummaryNoOfStudentText" runat="server" Text="<%$ Resources: Text, NoOfStudent %>" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClassSummaryNoOfStudent" runat="server" class="tableText" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 260px;height:20px">
                                                    <asp:Label ID="lblClassSummaryNoOfStudentNotToInjectText1" runat="server" Text="<%$ Resources: Text, NoOfStudent %>" />
                                                    (<asp:Label ID="lblClassSummaryNoOfStudentNotToInjectText2" runat="server" Text="<%$ Resources: Text, ConfirmedNotToInject %>" />)
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClassSummaryNoOfStudentNotToInject" runat="server" class="tableText" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 260px;height:20px">
                                                    <asp:Label ID="lblClassSummaryNoOfStudentActualInjectedText1" runat="server" Text="<%$ Resources: Text, NoOfStudent %>" />
                                                    (<asp:Label ID="lblClassSummaryNoOfStudentActualInjectedText2" runat="server" Text="<%$ Resources: Text, ActualInjected %>" />)
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblClassSummaryNoOfStudentActualInjected" runat="server" class="tableText" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br>
                                        <asp:Panel ID="pnlGVClassSummary" runat="server" Height="500px" ScrollBars="Auto" BorderColor="DarkGray" BorderWidth="1px">
                                            <asp:GridView ID="gvClassSummary" runat="server" CssClass="gvTable" Width="100%" AutoGenerateColumns="False" AllowPaging="True"
                                                AllowSorting="True" OnRowDataBound="gvClassSummary_RowDataBound" OnPreRender="gvClassSummary_PreRender" OnSorting="gvClassSummary_Sorting"
                                                OnPageIndexChanging="gvClassSummary_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, SeqNo %>" SortExpression="Student_Seq" ItemStyle-Width="35">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClassSummarySeqNo" runat="server" Text='<%# Eval("Student_Seq")%>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, ClassNo %>" SortExpression="Class_No_Sort" ItemStyle-Width="35">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClassSummaryClassNo" runat="server" Text='<%# Eval("Class_No") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, DocTypeIDNL %>" SortExpression="DocCode_DocNo" ItemStyle-Width="110">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClassSummaryDocType" runat="server" Text='<%# Eval("Doc_Code") %>' />
                                                            <br />
                                                            <asp:Label ID="lblClassSummaryDocNo" runat="server" Text='<%# Eval("Doc_No") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, ContactNo2 %>" SortExpression="Contact_No" ItemStyle-Width="60">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClassSummaryContactNo" runat="server" Text='<%# Eval("Contact_No") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Name %>" SortExpression="NameEN_NameCH" ItemStyle-Width="120">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClassSummaryNameEN" runat="server" Text='<%# Eval("Name_EN")%>' />
                                                            <br />
                                                            <asp:Label ID="lblClassSummaryNameCH" runat="server" Text='<%# Eval("Name_CH")%>' Font-Names="HA_MingLiu" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Sex %>" SortExpression="Sex" ItemStyle-Width="30">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClassSummarySex" runat="server" Text='<%# Eval("Sex") %>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, DOB %>" SortExpression="DOB" ItemStyle-Width="110">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClassSummaryDOB" runat="server"></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, ConfirmToInject %>" SortExpression="Reject_Injection" ItemStyle-Width="50">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClassSummaryNotToInject" runat="server" Text='<%# Eval("Reject_Injection")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, AccountID_ReferenceNo %>" SortExpression="Real_Account_ID_Reference_No" ItemStyle-Width="100">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClassSummaryAccountIDReferenceNo" runat="server" Text='<%# Eval("Real_Account_ID_Reference_No")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Injected %>" SortExpression="Injected" ItemStyle-Width="50">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblClassSummaryInjected" runat="server" Text='<%# Eval("Injected")%>'></asp:Label>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center;vertical-align:top;padding-top:15px;padding-bottom:10px">  
                                        <asp:ImageButton ID="ibtnClassSummaryClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnClassSummaryClose_Click" />
                                    </td>
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
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button Style="display: none" ID="btnHiddenClassSummary" runat="server" />
            <cc1:ModalPopupExtender ID="mpeClassSummary" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenClassSummary" PopupControlID="pnlClassSummary" RepositionMode="None"
                PopupDragHandleControlID="pnlClassSummaryHeading" />
            <%-- End of Pop up for Show Document Type Selection --%>   

            <%-- Pop up for Show Mark Inject Summary --%>
            <asp:Panel ID="pnlPreCheckSummary" runat="server" Style="display: none">
                <asp:Panel ID="pnlPreCheckSummaryHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 9px; height: 35px">
                            </td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png);
                                color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblPreCheckSummaryHeading" runat="server" Text="<%$ Resources:Text, ClientInformation %>"></asp:Label></td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px; position:relative; left: -2px">
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 850px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y">
                        </td>
                        <td style="background-color: #ffffff" align="center">
                            <br />
                            <table cellpadding="0" cellspacing="0" style="width:800px">                                        
                                <tr style="vertical-align:top">
                                    <td>
                                        <div id="divPreCheckSummaryClientInformation" runat="server" class="eHSTableHeading">
                                            <asp:Label ID="lblPreCheckSummaryClientInformation" runat="server" 
                                                Text="<%$ Resources: Text, ClientInformation %>" />
                                        </div>
                                        <table class="tblSFD">
                                            <tr>
                                                <td style="width: 260px;height:20px">
                                                    <asp:Label ID="lblPreCheckSummaryCategoryNameText" runat="server" Text="<%$ Resources: Text, Category %>" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPreCheckSummaryCategoryName" runat="server" class="tableText" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 260px;height:20px">
                                                    <asp:Label ID="lblPreCheckSummarySubsidyText" runat="server" Text="<%$ Resources: Text, Subsidy %>" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPreCheckSummarySubsidy" runat="server" class="tableText" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan ="2">
                                                    <table style="border-collapse:collapse">
                                                        <tr>
                                                            <td style="width: 260px;height:22px" >
                                                                <asp:Label ID="lblPreCheckSummaryDoseToInjectText" runat="server" Text="<%$ Resources: Text, DoseToInject %>" />
                                                            </td>
                                                            <td style="width: 200px">
                                                                <asp:Label ID="lblPreCheckSummaryDoseToInject1" runat="server" class="tableText" Text="<%$ Resources: Text, OnlyOr1stDose %>" />
                                                            </td>
                                                            <td style="width: 200px">
                                                                <asp:Label ID="lblPreCheckSummaryDoseToInject2" runat="server" class="tableText" Text="<%$ Resources: Text, 2ndDose %>" />
                                                            </td>                                  
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 260px;height:22px">
                                                                <asp:Label ID="lblPreCheckSummaryVaccinationDateText" runat="server" Text="<%$ Resources: Text, VaccinationDate %>" />
                                                            </td>
                                                            <td style="width: 200px">
                                                                <asp:Label ID="lblPreCheckSummaryVaccinationDate1" runat="server" class="tableText" />
                                                            </td>
                                                            <td style="width: 200px">
                                                                <asp:Label ID="lblPreCheckSummaryVaccinationDate2" runat="server" class="tableText" />
                                                            </td>   
                                                        </tr>
                                                        <tr>
                                                            <td style="width: 260px;height:22px">
                                                                <asp:Label ID="lblPreCheckSummaryGenerationDateText" runat="server" Text="<%$ Resources: Text, VaccinationReportGenerationDate %>"/>
                                                            </td>
                                                            <td style="width: 200px">
                                                               <asp:Label ID="lblPreCheckSummaryGenerationDate1" runat="server" class="tableText" />
                                                            </td>
                                                            <td style="width: 200px">
                                                               <asp:Label ID="lblPreCheckSummaryGenerationDate2" runat="server" class="tableText" />
                                                            </td> 
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="width: 260px;height:20px">
                                                    <asp:Label ID="lblPreCheckSummaryNoOfClientText" runat="server" Text="<%$ Resources: Text, NoOfClient %>" />
                                                </td>
                                                <td>
                                                    <asp:Label ID="lblPreCheckSummaryNoOfClient" runat="server" class="tableText" />
                                                </td>
                                            </tr>
                                        </table>
                                        <br>
                                        <asp:Panel ID="pnlGVPreCheckSummary" runat="server" Height="500px" ScrollBars="Auto" BorderColor="DarkGray" BorderWidth="1px">
                                            <asp:GridView ID="gvPreCheckSummary" runat="server" CssClass="gvTable" Width="100%" AutoGenerateColumns="False" AllowPaging="True"
                                                AllowSorting="True" OnRowDataBound="gvPreCheckSummary_RowDataBound" OnPreRender="gvPreCheckSummary_PreRender" OnSorting="gvPreCheckSummary_Sorting"
                                                OnPageIndexChanging="gvPreCheckSummary_PageIndexChanging">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, SeqNo %>" SortExpression="Student_Seq" ItemStyle-Width="30">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPreCheckSeqNo" runat="server" Text='<%# Eval("Student_Seq")%>' />
                                                        </ItemTemplate>
                                                        <ItemStyle BackColor="White" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, DocTypeIDNL %>" SortExpression="DocCode_DocNo" ItemStyle-Width="110">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPreCheckDocType" runat="server" Text='<%# Eval("Doc_Code") %>' />
                                                            <br />
                                                            <asp:Label ID="lblPreCheckDocNo" runat="server" Text='<%# Eval("Doc_No") %>' />
                                                        </ItemTemplate>
                                                        <ItemStyle BackColor="White" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Name %>" SortExpression="NameEN_NameCH" ItemStyle-Width="120">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPreCheckNameEN" runat="server" Text='<%# Eval("Name_EN")%>' />
                                                            <br />
                                                            <asp:Label ID="lblPreCheckNameCH" runat="server" Text='<%# Eval("Name_CH")%>' Font-Names="HA_MingLiu" />
                                                        </ItemTemplate>
                                                        <ItemStyle BackColor="White" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Sex %>" SortExpression="Sex" ItemStyle-Width="30">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPreCheckSex" runat="server" Text='<%# Eval("Sex") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle BackColor="White" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, OnlyDose %>" SortExpression="OnlyDose" ItemStyle-Width="50">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPreCheckOnlyDose" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle BackColor="White" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, 1stDose2 %>" SortExpression="FirstDose"  ItemStyle-Width="50">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPreCheck1stDose" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle BackColor="White" />
                                                    </asp:TemplateField>

                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, 2ndDose %>" SortExpression="SecondDose" ItemStyle-Width="50">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPreCheck2ndDose" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle BackColor="White" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, Remarks %>" SortExpression="MarkInjectRemark" ItemStyle-Width="50">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPreCheckRemarks" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle BackColor="White" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="<%$ Resources: Text, MarkInject %>" SortExpression="Injected" ItemStyle-Width="50">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblPreCheckSummaryInject" runat="server" />
                                                        </ItemTemplate>
                                                        <ItemStyle BackColor="White" />
                                                    </asp:TemplateField>
                                                </Columns>
                                            </asp:GridView>
                                        </asp:Panel>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center;vertical-align:top;padding-top:15px;padding-bottom:10px">  
                                        <asp:ImageButton ID="ibtnPreCheckSummaryClose" runat="server" AlternateText="<%$ Resources:AlternateText, CloseBtn %>"
                                            ImageUrl="<%$ Resources:ImageUrl, CloseBtn %>" OnClick="ibtnPreCheckSummaryClose_Click" />
                                    </td>
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
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button Style="display: none" ID="btnHiddenPreCheckSummary" runat="server" />
            <cc1:ModalPopupExtender ID="mpePreCheckSummary" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenPreCheckSummary" PopupControlID="pnlPreCheckSummary" RepositionMode="None"
                PopupDragHandleControlID="pnlPreCheckSummaryHeading" />
            <%-- End of Pop up for Show Mark Inject Summary --%>   

            <%-- Pop up for Warning --%>
            <asp:Panel ID="panWarning" runat="server" Style="display: none">
                <asp:Panel ID="panWarningHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 600px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblWarningTitle" runat="server" Text="<%$ Resources: Text, ConfirmBoxTitle %>"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px; position:relative; left: -2px"></td>
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
                                        <asp:Image ID="imgWarningIcon" runat="server" ImageUrl="~/Images/others/questionMark.png" />
                                    </td>
                                    <td align="left">
                                        <asp:Label ID="lblWarningMessage" runat="server" Font-Bold="True" />
                                    </td>
                                    <td style="width: 10px"></td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="3">
                                        <asp:ImageButton ID="ibtnWarningConfirm" runat="server" ImageUrl="<%$ Resources: ImageUrl, ConfirmBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, ConfirmBtn %>" OnClick="ibtnWarningConfirm_Click" />
                                        <asp:ImageButton ID="ibtnWarningCancel" runat="server" ImageUrl="<%$ Resources: ImageUrl, CancelBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, CancelBtn %>" OnClick="ibtnWarningCancel_Click" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button ID="btnHiddenWarning" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeWarning" runat="server" TargetControlID="btnHiddenWarning"
                PopupControlID="panWarning" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panWarningHeading">
            </cc1:ModalPopupExtender>
            
            <%-- End of Pop up for Warning first input --%>

            <%-- Pop up for Saved --%>
            <asp:Panel ID="panSaved" runat="server" Style="display: none">
<%--                <asp:Panel ID="panSavedHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 250px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="lblSavedTitle" runat="server" Text="<%$ Resources: AlternateText, SaveBtn %>"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>--%>
                <table border="0" cellpadding="0" cellspacing="0" style="width:250px;height:80px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/top-left_No_Title.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/top-mid_No_Title.png); background-repeat: repeat-x; width: 235px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/top-right_No_Title.png); width: 9px; height: 7px; position:relative; left: -2px"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 7px; background-repeat: repeat-y"></td>
                        <td style="background-color: #FFFFFF; padding: 5px">
                            <table style="width: 100%">
                                <tr>
                                    <td align="center" style="width: 60px; vertical-align: top">
                                        <asp:Image ID="imgSavedIcon" runat="server" ImageUrl="~/Images/others/tick.png" />
                                    </td>
                                    <td align="center">
                                        <asp:Label ID="lblSavedMessage" runat="server" Font-Bold="True" Text="<%$ Resources: Text, SavedSuccessfully %>" />
                                    </td>
                                    <td style="width: 10px"></td>
                                </tr>
                                <tr style="display:none">
                                    <td align="center" colspan="3">
                                        <br />
                                        <asp:ImageButton ID="ibtnSavedOK" runat="server" ImageUrl="<%$ Resources: ImageUrl, OKBtn %>"
                                            AlternateText="<%$ Resources: AlternateText, OKBtn %>" />
                                    </td>
                                </tr>
                            </table>
                        </td>
                        <td style="background-image: url(../Images/dialog/right.png); width: 7px; background-repeat: repeat-y"></td>
                    </tr>
                    <tr>
                        <td style="background-image: url(../Images/dialog/bottom-left.png); width: 7px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-mid.png); background-repeat: repeat-x; width: 235px; height: 7px"></td>
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 9px; height: 7px; position:relative; left: -2px"></td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button ID="btnSaved" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeSaved" runat="server" TargetControlID="btnSaved" BehaviorID="mpeSavedBehavior"
                PopupControlID="panSaved" BackgroundCssClass=""
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="" CancelControlID="ibtnSavedOK">
            </cc1:ModalPopupExtender>            
            <%-- End of Pop up for Saved--%>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
