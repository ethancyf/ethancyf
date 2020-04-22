<%@ Page Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.Master"
    Codebehind="eHealthAccountDeathRecordMaintenance.aspx.vb" Inherits="HCVU.eHealthAccountDeathRecordMaintenance"
    Title="<%$ Resources: Title, eHealthAccountDeathRecordMaintenance %>" %>

<%@ Register Src="../UIControl/eHealthAccountDeathRecord/eHealthAccountDeathRecordMaint.ascx"
    TagName="eHealthAccountDeathRecordMaint" TagPrefix="uc2" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="CustomControls" Namespace="CustomControls" TagPrefix="cc2" %>
<%@ Register Src="~/UIControl/DocType/ucReadOnlyDocumnetType.ascx" TagName="ucReadOnlyDocumentType"
    TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <script type="text/javascript" src="../JS/Common.js"></script>


    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <asp:Image ID="imgHeader" runat="server" ImageUrl="<%$ Resources: ImageUrl, eHealthAccountDeathRecordMaintenanceBanner %>"
        AlternateText="<%$ Resources: AlternateText, eHealthAccountDeathRecordMaintenanceBanner %>">
    </asp:Image>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <uc2:eHealthAccountDeathRecordMaint id="ucMain" runat="server">
            </uc2:eHealthAccountDeathRecordMaint>
            
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
