<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Main.aspx.vb" Inherits="VerifierIDEAS._Default" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register src="IDEASCombo.ascx" tagname="IDEASCombo" tagprefix="uc1" %>

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

    <link href="CSS/DialogStyle.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="./JS/ideasComboLib4Ra.js"></script>

   
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" EnablePartialRendering="true" runat="server" ></asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
        <table cellpadding="0" cellspacing="0">
            <tr>
                <td>
                    <img src="Images/master/banner_header.jpg" alt="eHealth System (Subsidies)" />
                </td>
                <td>
                    <span class="title" style="position: relative; left: -990px; top: 20px;">IDEAS Service
                        Verifier </span>
                </td>
            </tr>
        </table>
        <br />
        <div style="margin-left: 10px;">
            <table style="width=100%; -ms-word-break: break-all;">
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
                                        <asp:DropDownList ID="ddlIDEAS1Url" runat="server" AutoPostBack="True"/>
                                        <asp:Label ID="lblTokenServerURL1" runat="server"  Visible="false"/></td>
                                    <td><asp:Button ID="btnReadSmartIC1" runat="server" Text="Read Smart ID Card (IDEAS1)" /></td>
                                </tr>
                                <tr>
                                    <td colspan="3"><hr /></td>
                                </tr>
                                <tr>
                                    <td class="tableText" style="white-space: nowrap;">Token Service URL (IDEAS2):</td>
                                    <td class="tableTitle" style="white-space: nowrap;">
                                        <asp:DropDownList ID="ddlIDEAS2Url" runat="server" AutoPostBack="True"/>
                                        <asp:Label ID="lblTokenServerURL2" runat="server"  Visible="false"/>
                                    </td>
                                     <td><asp:Button ID="btnReadSmartIC2" runat="server" Text="Read Smart ID Card (IDEAS2)" />
                                         <asp:Button ID="btnReadSmartIC2_5" runat="server" Text="Read Smart ID Card (IDEAS2 with Gender)" />
                                     </td>
                                </tr>
                                <tr>
                                     <td colspan="3"><hr /></td>
                                </tr>
                                <tr style="display:none">
                                    <td class="auto-style1" style="white-space: nowrap;">
                                        Broker Service URL (IDEAS1):</td>
                                    <td class="auto-style2" style="white-space: nowrap;">
                                        
                                        <asp:Label ID="lblBrokerServerURL1" runat="server" /></td>
                                     <td></td>
                                </tr>
                                <tr>
                                    <td class="tableText" style="white-space: nowrap;">Broker Service URL (Combo):</td>
                                    <td class="tableTitle" style="white-space: nowrap;">
                                        <asp:DropDownList ID="ddlIDEASComboUrl" runat="server" AutoPostBack="True"/>
                                        <br />
                                        <asp:Label ID="lblBrokerServerURL2" runat="server" Visible="false" />
                                    </td>
                                     <td><asp:Button ID="btnReadSmartICCombo" runat="server" Text="Read Smart ID Card (Combo)" disabled="true" Visible="false"/>
                                        <asp:Button ID="btnReadSmartICComboiframe" runat="server" Text="Read Smart ID Card (Combo) (iframe)" disabled="true"/></td>
                                </tr>
                                <tr>
                                    <td colspan="3"><hr /></td>
                                </tr>
                                <tr>
                                    <td class="tableText">
                                        Result:</td>
                                    <td  colspan="2" class="tableTitle">
                                        <asp:Label ID="lblResult_Status" runat="server" Text="" />
                                        <br />
                                        <asp:Label ID="lblResult_URL" runat="server" Text="" />
                                    </td>

                                </tr>
                                <tr>
                                    <td class="tableText">
                                        HKIC Version:</td>
                                    <td class="tableTitle">
                                        <asp:Label ID="lblResult_HKICVer" runat="server" Text="" /></td>
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
                                    <td colspan="2"  class="tableTitle">
                                        <asp:Label ID="lblResult_Dtm" runat="server" Text="" /></td>
                                </tr>
                            </table>
                        </asp:Panel>

                     

                        <asp:Panel ID="pnlResultInternal" runat="server" Visible="false" Width="700px">
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
                                        HKIC Version</td>
                                    <td>
                                        <asp:Label ID="lblPersionInfo_HKICver" runat="server" Text="" /></td>
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

        <uc1:IDEASCombo ID="ucIDEASCombo" runat="server" />


        <script type="text/javascript">
            //function displayIDEASResult() {
            //    document.getElementById("btnDisplayResultComboiframe").click();
            //}

            // TODO _param to be confirmed
            function checkIdeasComboClientSuccessEHS(_param) {
                //var param = { result: _param, ideasVer: "", artifactId: "" };
                //checkIdeasComboClientSuccessCallback(param);
                //document.getElementById("btnReadSmartICCombo").disabled = false;
                document.getElementById("btnReadSmartICComboiframe").disabled = false;
            }

            // TODO _param to be confirmed
            function checkIdeasComboClientFailureEHS(_param) {
                //var param = { result: _param, ideasVer: "", artifactId: "" };
                //checkIdeasComboClientFailureCallback(param);
                //document.getElementById("btnReadSmartICCombo").disabled = true;
                document.getElementById("btnReadSmartICComboiframe").disabled = true;
            }

            //checkIdeasComboClient(checkIdeasComboClientSuccessEHS, checkIdeasComboClientFailureEHS);
         </script>
                </ContentTemplate>
        </asp:UpdatePanel>

     
    </form>
</body>
</html>
