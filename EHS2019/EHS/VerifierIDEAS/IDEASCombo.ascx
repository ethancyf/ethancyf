<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="IDEASCombo.ascx.vb" Inherits="VerifierIDEAS.IDEASCombo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Button ID="btnModalPopupExtenderIDEASComboReader" runat="server" style="display: none"/>
<asp:Button ID="btnDisplayResult" runat="server" Text="Display Result " style="display: none"/>

<script type="text/javascript">
    function displayIDEASResult() {
        document.getElementById("ucIDEASCombo_btnDisplayResult").click();
    }
</script>

<cc1:ModalPopupExtender ID="ModalPopupExtenderIDEASComboReader" runat="server" TargetControlID="btnModalPopupExtenderIDEASComboReader"
PopupControlID="pnlIDEASComboReader" BackgroundCssClass="modalBackground"
BehaviorID="mdlIDEASComboReader" DropShadow="False" RepositionMode="None"/>

<asp:Panel ID="pnlIDEASComboReader" runat="server" Visible="true" style="min-width: 470px;">
    <iframe id="iframeIDEASComboReader" src="" runat="server" frameborder="0"
        style=" height: 300px; width: 100%" bgcolor="transparent"></iframe>
    <%--    <iframe id="iframe1" src="" runat="server" frameborder="0"
        style="border:0" width="100%" height ="100%"></iframe> --%>
</asp:Panel>