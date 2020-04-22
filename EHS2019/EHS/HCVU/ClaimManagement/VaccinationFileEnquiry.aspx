<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="VaccinationFileEnquiry.aspx.vb" Inherits="HCVU.VaccinationFileEnquiry"
    Title="<%$ Resources: Title, VaccinationFileEnquiry %>" EnableEventValidation="False" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="../UIControl/StudentFile/ucStudentFileDetail.ascx" TagName="ucStudentFileDetail" TagPrefix="uc1" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="ContentPlaceHolderHead" runat="server">
    <style type="text/css">
        table.gvTable {
            width: 100%;
        }

            table.gvTable td {
                vertical-align: top;
            }

        table.tblS {
            width: 100%;
        }

            table.tblS > tbody > tr {
                height: 28px;
            }

            table.tblS td {
                vertical-align: top;
            }

        table.tblSFD > tbody > tr {
            height: 25px;
        }

        table.tblSF {
            width: 100%;
        }

            table.tblSF > tbody > tr {
                height: 35px;
            }

            table.tblSF td {
                vertical-align: top;
            }

        table.tblI2DoseToInject td {
            padding-right: 15px;
        }

        table.tblSF2 {
            width: 100%;
        }

            table.tblSF2 > tbody > tr {
                height: 24px;
            }

            table.tblSF2 td {
                vertical-align: top;
            }

        .RadioButtonList td { 
            width: 50%;  
        }

    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <script type="text/javascript" src="../JS/Common.js"></script>
    <asp:ScriptManager ID="ScriptManager1" runat="server" AsyncPostBackTimeout="600">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, VaccinationFileEnquiryBanner %>"
        AlternateText="<%$ Resources: AlternateText, VaccinationFileEnquiryBanner %>"></asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="height: 4px"></div>
            <cc2:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="950px" />
            <cc2:MessageBox ID="udcMessageBox" runat="server" Width="950px" />
            <asp:MultiView ID="mvCore" runat="server" ActiveViewIndex="-1">
                <asp:View ID="vSearch" runat="server">
                    <table style="width: 100%" class="tblS">
                        <tr style="height:30px">
                            <td style="width: 160px">
                                <asp:Label ID="lblSFileTypeText" runat="server" Text="<%$ Resources: Text, FileType %>" />
                            </td>
                            <td valign="top">
                                <asp:RadioButtonList ID="rblSFileType" runat="server" AutoPostBack="true" Width="300" RepeatColumns ="2" CssClass="RadioButtonList" Style="position:relative;left:-3px;top:-4px" />
                            </td>
                        </tr>                        
                        <tr>
                            <td>
                                <asp:Label ID="lblSSchemeText" runat="server" Text="<%$ Resources: Text, Scheme %>"></asp:Label>
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlSScheme" runat="server" Width="300" AutoPostBack="true"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSStudentFileIDText" runat="server" Text="<%$ Resources: Text, VaccinationFileID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSStudentFileID" runat="server" Width="200" MaxLength="15"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSSchoolCodeText" runat="server" Text="<%$ Resources: Text, SchoolRCHCode %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSSchoolCode" runat="server" Width="200" MaxLength="30"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:Label ID="lblSServiceProviderIDText" runat="server" Text="<%$ Resources: Text, ServiceProviderID %>"></asp:Label>
                            </td>
                            <td>
                                <asp:TextBox ID="txtSServiceProviderID" runat="server" Width="200" MaxLength="8"></asp:TextBox>
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
                                <asp:DropDownList ID="ddlSStatus" runat="server" Width="300"></asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="trSVaccinationSeason" runat="server">
                            <td>
                                <asp:Label ID="lblSVaccinationSeasonText" runat="server" CssClass="tableTitle" Text='<%$ Resources:Text, VaccinationSeason %>'></asp:Label>
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rblSVaccinationSeason" runat="server" Width="300" RepeatDirection="Horizontal" AutoPostBack="true" CssClass="RadioButtonList" RepeatColumns ="2" Style="position:relative;left:-3px;top:-4px">
                                    <asp:ListItem Text='<%$ Resources:Text, CurrentSeason %>' Selected="True" Value="C"></asp:ListItem>
                                    <asp:ListItem Text='<%$ Resources:Text, PastSeason %>' Value="P"></asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" style="text-align: center">
                                <asp:ImageButton ID="ibtnSSearch" runat="server" ImageUrl="<%$ Resources: ImageUrl, SearchBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, SearchBtn %>" OnClick="ibtnSSearch_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vResult" runat="server">
                    <asp:GridView ID="gvR" runat="server" CssClass="gvTable" AutoGenerateColumns="False" AllowPaging="True" Width="1210px"
                        AllowSorting="True" OnRowDataBound="gvR_RowDataBound" OnPreRender="gvR_PreRender"
                        OnRowCommand="gvR_RowCommand" OnSorting="gvR_Sorting" OnPageIndexChanging="gvR_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationFileID %>" ItemStyle-Width="120" ItemStyle-Height="80">
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
                            <asp:TemplateField HeaderText="<%$ Resources: Text, DownloadLatestReport %>" ItemStyle-Width="120"  ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblRDownloadLatestReport" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>                      
                    <table>
                        <tr>
                            <td style="width: 300px">
                                <asp:ImageButton ID="ibtnRBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnRBack_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vDetail" runat="server">
                    <div style="width: 980px; text-align: right">
                        <asp:ImageButton ID="ibtnDShowRectification" runat="server" ImageUrl="<%$ Resources: ImageUrl, ShowRectificationRecordBtn %>"
                            AlternateText="<%$ Resources: AlternateText, ShowRectificationRecordBtn %>" OnClick="ibtnDShowRectification_Click" />
                        <asp:ImageButton ID="ibtnDShowVaccClaimRecord" runat="server" ImageUrl="<%$ Resources: ImageUrl, ShowVaccinationClaimRecordBtn %>"
                            AlternateText="<%$ Resources: AlternateText, ShowVaccinationClaimRecordBtn %>" OnClick="ibtnDShowVaccClaimRecord_Click" />
                    </div>
                    <uc1:ucStudentFileDetail ID="udcStudentFileDetail" runat="server"></uc1:ucStudentFileDetail>
                    <br />
                    <table>
                        <tr>
                            <td style="width: 300px">
                                <asp:ImageButton ID="ibtnDBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnDBack_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
                <asp:View ID="vPreCheck" runat="server">
                    <asp:GridView ID="gvP" runat="server" CssClass="gvTable" AutoGenerateColumns="False" AllowPaging="True" Width="1100px"
                        AllowSorting="True" OnRowDataBound="gvP_RowDataBound" OnPreRender="gvP_PreRender"
                        OnRowCommand="gvP_RowCommand" OnSorting="gvP_Sorting" OnPageIndexChanging="gvP_PageIndexChanging">
                        <Columns>
                            <asp:TemplateField HeaderText="<%$ Resources: Text, VaccinationFileID %>" ItemStyle-Width="0" ItemStyle-Height="80">
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
                            <asp:TemplateField HeaderText="<%$ Resources: Text, Rectification %>" ItemStyle-Height="80">
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
                            <asp:TemplateField HeaderText="<%$ Resources: Text, DownloadLatestReport %>" ItemStyle-Width="0"  ItemStyle-Height="80">
                                <ItemTemplate>
                                    <asp:Label ID="lblPDownloadLatestReport" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>              
                    <table>
                        <tr>
                            <td style="width: 300px">
                                <asp:ImageButton ID="ibtnPBack" runat="server" ImageUrl="<%$ Resources: ImageUrl, BackBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, BackBtn %>" OnClick="ibtnPBack_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
            <%-- Pop up for Show Rectification Record --%>
            <asp:Button ID="btnPopupShowRectRecord" runat="server" Style="display: none" />
            <cc1:ModalPopupExtender ID="mpeShowRectRecord" runat="server" TargetControlID="btnPopupShowRectRecord"
                PopupControlID="panShowRectRecord" BackgroundCssClass="modalBackgroundTransparent"
                DropShadow="False" RepositionMode="None" PopupDragHandleControlID="panShowRectRecordHeading">
            </cc1:ModalPopupExtender>
            <asp:Panel ID="panShowRectRecord" runat="server" Style="display: none">
                <asp:Panel ID="panShowRectRecordHeading" runat="server" Style="cursor: move">
                    <table border="0" cellpadding="0" cellspacing="0" style="width: 900px">
                        <tr>
                            <td style="background-image: url(../Images/dialog/top-left.png); width: 7px; height: 35px"></td>
                            <td style="font-weight: bold; font-size: 14px; background-image: url(../Images/dialog/top-mid.png); color: #ffffff; background-repeat: repeat-x; height: 35px">
                                <asp:Label ID="Label3" runat="server" Text="<%$ Resources:Text, RectificationRecord %>"></asp:Label>
                            </td>
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px"></td>
                        </tr>
                    </table>
                </asp:Panel>
                <table border="0" cellpadding="0" cellspacing="0" style="width: 900px">
                    <tr>
                        <td style="background-image: url(../Images/dialog/left.png); width: 1px; background-repeat: repeat-y"></td>
                        <td style="background-color: #FFFFFF">
                            <div style="max-height: 650px; padding: 10px; overflow-y: scroll">
                                <uc1:ucStudentFileDetail ID="udcStudentFileDetailPopup" runat="server"></uc1:ucStudentFileDetail>
                                <div style="text-align: center; padding-top: 20px">
                                    <asp:ImageButton ID="ibtnPSClose" runat="server" ImageUrl="<%$ Resources: ImageUrl, CloseBtn %>"
                                        AlternateText="<%$ Resources: AlternateText, CloseBtn %>" OnClick="ibtnPSClose_Click" />
                                </div>
                            </div>
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
            <%-- End of Pop up for Show Rectification Record --%>

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
                            <td style="background-image: url(../Images/dialog/top-right.png); width: 7px; height: 35px">
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
                                        <div id="divPreCheckSummaryClientInformation" runat="server" class="headingText">
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
                        <td style="background-image: url(../Images/dialog/bottom-right.png); width: 7px;
                            height: 7px">
                        </td>
                    </tr>
                </table>
            </asp:Panel>
            <asp:Button Style="display: none" ID="btnHiddenPreCheckSummary" runat="server" />
            <cc1:ModalPopupExtender ID="mpePreCheckSummary" runat="server" BackgroundCssClass="modalBackgroundTransparent"
                TargetControlID="btnHiddenPreCheckSummary" PopupControlID="pnlPreCheckSummary" RepositionMode="None"
                PopupDragHandleControlID="pnlPreCheckSummaryHeading" />
            <%-- End of Pop up for Show Mark Inject Summary --%>  
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
