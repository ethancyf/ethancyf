<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    CodeBehind="reportSubmission.aspx.vb" Inherits="HCVU.reportSubmission" Title="<%$ Resources:Title, ReportSubmission %>"
    EnableEventValidation="False" %>

<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc1" %>
<%@ Register Src="reportCriteriaBase.ascx" TagName="ucReportCriteriaBase" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <asp:Image ID="img_header" runat="server" AlternateText="<%$ Resources:AlternateText, ReportSubmissionBanner %>"
                ImageAlign="AbsMiddle" ImageUrl="<%$ Resources:ImageUrl, ReportSubmissionBanner %>" />
            <cc1:MessageBox ID="udcErrorMessage" runat="server" Width="95%"></cc1:MessageBox>
            <cc1:InfoMessageBox ID="udcInfoMessageBox" runat="server" Width="95%"></cc1:InfoMessageBox>
            <asp:MultiView ID="MultiView1" runat="server" ActiveViewIndex="0">
                <asp:View ID="View1" runat="server">
                    <link href="<%=ResolveUrl("~/CSS/ReportSubmission/CommonControlStyle.css")%>" type="text/css" rel="Stylesheet">
                    <script type="text/javascript" src="<%=ResolveUrl("~/JS/ReportSubmission/CommonControl.js")%>"></script>
                    <asp:GridView ID="gvReportSubmission" AllowPaging="True" AutoGenerateColumns="False" PageSize="20"
                        Width="900px" AllowSorting="True" EnableTheming="True" runat="server" OnSelectedIndexChanged="gvReportSubmission_SelectedIndexChanged"
                        OnRowDataBound="gvReportSubmission_RowDataBound" OnPageIndexChanging="gvReportSubmission_PageIndexChanging"
                        OnPreRender="gvReportSubmission_PreRender" OnSorting="gvReportSubmission_Sorting">
                        <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblResultIndex" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" Width="25px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="<%$ Resources:Text, ReportID %>" SortExpression="Display_Code">
                                <ItemTemplate>
                                    <asp:Label ID="lblDisplayCode" runat="server" Text='<%# Eval("Display_Code")%>'></asp:Label>
                                    <asp:HiddenField ID="hfFileID" runat="server" Value='<%# Eval("File_ID")%>' />
                                </ItemTemplate>
                                <ItemStyle VerticalAlign="Top" Width="100px" />
                            </asp:TemplateField>
                            <asp:BoundField DataField="File_Name" SortExpression="File_Name" HeaderText="<%$ Resources:Text, ReportName %>">
                                <ItemStyle VerticalAlign="Top" Width="370px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="File_Desc" SortExpression="File_Desc" HeaderText="<%$ Resources:Text, Remark %>">
                                <ItemStyle VerticalAlign="Top" />
                            </asp:BoundField>
                        </Columns>
                        <SelectedRowStyle CssClass="SelectedRowStyle" />
                    </asp:GridView>
                    <br />
                    <asp:Panel ID="pnlReportCriteria" runat="server">
                        <asp:Label ID="lblReportCriteria" runat="server" Font-Bold="true" Text="<%$ Resources:Text, ReportCriteria %>"></asp:Label>
                        <br />
                        <br />
                        <table cellpadding="0" cellspacing="4" style="width: auto">
                            <tr>
                                <td align="left" valign="top" style="width: 180px">
                                    <asp:Label ID="lblReportIDTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ReportID %>"></asp:Label>
                                </td>
                                <td align="left" style="width: 700px">
                                    <asp:Label ID="lblReportID" runat="server" CssClass="tableText" Text=""></asp:Label>
                                    <asp:HiddenField ID="hfReportID" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" valign="top" style="width: 180px">
                                    <asp:Label ID="lblReportNameTitle" runat="server" CssClass="tableTitle" Text="<%$ Resources:Text, ReportName %>"></asp:Label>
                                </td>
                                <td align="left" style="width: 700px">
                                    <asp:Label ID="lblReportName" runat="server" CssClass="tableText" Text=""></asp:Label>
                                </td>
                            </tr>
                        </table>
                        <table cellpadding="0" cellspacing="4" style="width: auto">
                            <tr>
                                <td colspan="3" style="height: 20px"></td>
                            </tr>
                            <tr>
                                <td colspan="3" width="900px">
                                    <uc1:ucReportCriteriaBase ID="ucReportCriteriaBase" runat="server" />             
                                </td>
                            </tr>
                            <tr>
                                <td colspan="3" style="height: 20px"></td>
                            </tr>
                            <tr>
                                <td align="left" width="150px">
                                    <asp:ImageButton ID="btnBack" runat="server" AlternateText="<%$ Resources:AlternateText, BackBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, BackBtn %>" OnClick="btnBack_Click" />
                                </td>
                                <td align="Center" width="550px">
                                    <asp:ImageButton ID="btnSubmit" runat="server" AlternateText="<%$ Resources:AlternateText, SubmitBtn %>"
                                        ImageUrl="<%$ Resources:ImageUrl, SubmitBtn %>" OnClick="btnSubmit_Click" />
                                </td>
                                <td width="200px"></td>
                            </tr>
                        </table>
                    </asp:Panel>
                </asp:View>
                <asp:View ID="ViewReturn" runat="server">
                    <!-- CRP11-008 -->
                    <table>
                        <tr>
                            <td style="padding-top: 10px">
                                <asp:ImageButton ID="btnReturn" runat="server" ImageUrl="<%$ Resources: ImageUrl, ReturnBtn %>"
                                    AlternateText="<%$ Resources: AlternateText, ReturnBtn %>" OnClick="btnReturn_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:View>
            </asp:MultiView>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
