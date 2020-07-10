<%@ Page Language="vb" AutoEventWireup="false" Codebehind="IDEASComboReader.aspx.vb" Inherits="HCSP.IDEASComboReader" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>eHealth System (Subsidies) - IDEAS Service Verifier</title>
    <style type="text/css">
        body 
        {
	        font-size: 14px;
	        color: #666666;
	        font-family: Arial;
	        margin: 0px 0px 0px 1px;
        }
        
        .title
        {
	        font-size: 20px;
	        color: Red;
	        font-family: Arial;
	        font-weight: bold;
	        white-space:nowrap;
        }

        .tableTitle
        {
             font-size: 16px;
             color: #666666;
             font-family: Arial;
        }
        
        .tableTitleAlert
        {
             font-size: 16px;
             color: red;
             font-family: Arial;
        }
        
        .tableText
        {
	        font-size: 16px;
	        color: #4D4D4D;
	        font-family: Arial;
	        font-weight: bold;
        }
        
        .unhighlightTableWithoutCursor
        {
	        border-right: #d3d3d3 1px solid;
	        border-top: #d3d3d3 1px solid;
	        border-left: #d3d3d3 1px solid;
	        border-bottom: #d3d3d3 1px solid;
	        Padding-Left: 20px;
	        Padding-Right: 20px;
	        Padding-top: 10px;
	        Padding-bottom: 10px;	        
        }
        .auto-style1 {
            font-size: 16px;
            color: #4D4D4D;
            font-family: Arial;
            font-weight: bold;
            height: 23px;
        }
        .auto-style2 {
            font-size: 16px;
            color: #666666;
            font-family: Arial;
            height: 23px;
        }
    </style>

    <%--<script type="text/javascript" src="./JS/ideasComboLib4Ra.js"></script>--%>

    <script type="text/javascript">
        // TODO _param to be confirmed
        //function checkIdeasComboClientSuccessEHS(_param) {
        //    //var param = { result: _param, ideasVer: "", artifactId: "" };
        //    //checkIdeasComboClientSuccessCallback(param);
        //    document.getElementById("btnReadSmartICCombo").disabled = false;
        //}

        //// TODO _param to be confirmed
        //function checkIdeasComboClientFailureEHS(_param) {
        //    //var param = { result: _param, ideasVer: "", artifactId: "" };
        //    //checkIdeasComboClientFailureCallback(param);
        //    document.getElementById("btnReadSmartICCombo").disabled = true;
        //}

        function displayIDEASResult() {
            parent.displayIDEASResult();
        }
        //checkIdeasComboClient(checkIdeasComboClientSuccessEHS, checkIdeasComboClientFailureEHS);
    </script>
</head>
<body>
    <form id="form1" runat="server">
       <%-- <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <img src="Images/master/banner_header.jpg" alt="eHealth System (Subsidies)" />
                </td>
                <td>
                    <span class="title" style="position: relative; left: -990px; top: 20px;">IDEAS Service
                        Verifier </span>
                </td>
            </tr>
        </table>--%>
<%--        <br />
        <div style="margin-left: 10px; display: none">
            <table width="100%">
                <tr>
                    <td align="center">
                        <asp:Panel ID="pnlResult" runat="server">
                            <table class="unhighlightTableWithoutCursor" style="text-align: left;">
                                <colgroup>
                                    <col width="240px" valign="top" />
                                </colgroup>
                                <tr>
                                    <td class="auto-style1" style="white-space: nowrap;">
                                        Token Service URL (IDEAS1):</td>
                                    <td class="auto-style2" style="white-space: nowrap;">
                                        <asp:Label ID="lblTokenServerURL1" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td class="tableText" style="white-space: nowrap;">Token Service URL (IDEAS2):</td>
                                    <td class="tableTitle" style="white-space: nowrap;">
                                        <asp:Label ID="lblTokenServerURL2" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="auto-style1" style="white-space: nowrap;">
                                        Broker Service URL (IDEAS1):</td>
                                    <td class="auto-style2" style="white-space: nowrap;">
                                        <asp:Label ID="lblBrokerServerURL1" runat="server" /></td>
                                </tr>
                                <tr>
                                    <td class="tableText" style="white-space: nowrap;">Broker Service URL (IDEAS2):</td>
                                    <td class="tableTitle" style="white-space: nowrap;">
                                        <asp:Label ID="lblBrokerServerURL2" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableText">
                                        Result:</td>
                                    <td class="tableTitle">
                                        <asp:Label ID="lblResult_Status" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td class="tableText">
                                        Date of Issue:</td>
                                    <td class="tableTitle">
                                        <asp:Label ID="lblResult_DOI" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td class="tableText">
                                        Last Read Time:</td>
                                    <td class="tableTitle">
                                        <asp:Label ID="lblResult_Dtm" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center">
                                        <br />
                                        <asp:Button ID="btnReadSmartIC1" runat="server" Text="Read Smart ID Card (IDEAS1)" />
                                        <asp:Button ID="btnReadSmartIC2" runat="server" Text="Read Smart ID Card (IDEAS2)" />
                                        <asp:Button ID="btnReadSmartIC2_5" runat="server" Text="Read Smart ID Card (IDEAS2 with Gender)" />
                                        <asp:Button ID="btnReadSmartICCombo" runat="server" Text="Read Smart ID Card (Combo)" disabled="true"/>
                                    </td>
                                </tr>
                            </table>
                        </asp:Panel>
                        <asp:Panel ID="pnlResultInternal" runat="server" Visible="true" Width="700px">
                            <table style="width:700px; word-break:break-all;">
                                <colgroup>
                                    <col style="width: 150px;" valign="top" />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
                                        [IdeasRM.TokenResponse]</td>
                                </tr>
                                <tr>
                                    <td>
                                        Error Code:</td>
                                    <td>
                                        <asp:Label ID="lblIdeasTokenResponse_ErrorCode" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        Error Detail:</td>
                                    <td>
                                        <asp:Label ID="lblIdeasTokenResponse_ErrorDetail" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        Error Message:</td>
                                    <td>
                                        <asp:Label ID="lblIdeasTokenResponse_ErrorMessage" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        IdeasMAURL:</td>
                                    <td>
                                        <asp:Label ID="lblIdeasTokenResponse_IdeasMAURL" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        [Read Smart IC]</td>
                                </tr>
                                <tr>
                                    <td>
                                        Status:</td>
                                    <td>
                                        <asp:Label ID="lblQueryStringStatus" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2">
                                        [Get Card Face Data]</td>
                                </tr>
                                <tr>
                                    <td>
                                        Status Code:</td>
                                    <td>
                                        <asp:Label ID="lblStatusCode" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        Status Detail:</td>
                                    <td>
                                        <asp:Label ID="lblStatusDetail" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        Status Message:</td>
                                    <td>
                                        <asp:Label ID="lblStatusMessage" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        Stack Trace:</td>
                                    <td>
                                        <asp:Label ID="lblStackTrace" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        <br />
                                    </td>
                                    <td>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Tester Exception:</td>
                                    <td>
                                        <asp:Label ID="lblError" runat="server" Text="" /></td>
                                </tr>
                            </table>
                            <br />
                            <table>
                                <colgroup>
                                    <col style="width: 150px;" valign="top" />
                                </colgroup>
                                <tr>
                                    <td colspan="2">
                                        [Card Face Data]</td>
                                </tr>
                                <tr>
                                    <td>
                                        HKID</td>
                                    <td>
                                        <asp:Label ID="lblPersionInfo_HKID" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        English Name</td>
                                    <td>
                                        <asp:Label ID="lblPersionInfo_NameEng" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        Chinese Name</td>
                                    <td>
                                        <asp:Label ID="lblPersionInfo_NameChi" runat="server" Text="" />
                                        <asp:Label ID="lblPersionInfo_CCCode1" runat="server" Text="" />
                                        <asp:Label ID="lblPersionInfo_CCCode2" runat="server" Text="" />
                                        <asp:Label ID="lblPersionInfo_CCCode3" runat="server" Text="" />
                                        <asp:Label ID="lblPersionInfo_CCCode4" runat="server" Text="" />
                                        <asp:Label ID="lblPersionInfo_CCCode5" runat="server" Text="" />
                                        <asp:Label ID="lblPersionInfo_CCCode6" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Date of Birth</td>
                                    <td>
                                        <asp:Label ID="lblPersionInfo_DOB" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        Gender</td>
                                    <td>
                                        <asp:Label ID="lblPersionInfo_Gender" runat="server" Text="" /></td>
                                </tr>
                                <tr>
                                    <td>
                                        Date of Issue</td>
                                    <td>
                                        <asp:Label ID="lblPersionInfo_DOI" runat="server" Text="" /></td>
                                </tr>
                            </table>
                        </asp:Panel>
                    </td>
                </tr>
            </table>
        </div>

        --%>
    </form>
</body>
</html>
