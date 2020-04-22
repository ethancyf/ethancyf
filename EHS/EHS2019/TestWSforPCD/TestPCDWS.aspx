<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="TestPCDWS.aspx.vb" Inherits="TestWSforPCD.TestPCDWS" ValidateRequest="false" Debug="true"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Test XML Sending Page</title>
</head>
<body>
    <form id="form1" runat="server">
        
            <asp:label ID="lblSys_CodeText" runat="server" text="Sys_Code:" Visible="False"></asp:label> <asp:TextBox ID="txtSys_Code" runat="server" Visible="False"></asp:TextBox> <br />
            <asp:label ID="lblEnrolment_Ref_NoText" runat="server" text="Enrolment_Ref_No" Visible="False"></asp:label> <asp:TextBox ID="txtEnrolment_Ref_No" runat="server" Visible="False"></asp:TextBox> <br />
            <br />
            <table width="100%" border="0" cellpadding="0" cellspacing="0">
                <tr>
                    <td align="left">
                        <asp:Label Text="Login:" ID="lblLoginText" runat="server"></asp:Label>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="txtLogin" Width="200px" runat="server"></asp:TextBox>
                    </td>
                    <td align="left">
                        <asp:Button ID="btnGetLogin" Text="Get Login from DB" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 24px">
                        <asp:Label Text="Password:" ID="lblPwdText" runat="server"></asp:Label>
                    </td>
                    <td align="left" style="height: 24px">
                        <asp:TextBox ID="txtPwd" Width="200px" runat="server"></asp:TextBox> 
                    </td>
                    <td align="left" style="height: 24px">
                        <asp:Button ID="btnGetPassword" Text="Get Password from DB" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 24px">
                        <asp:Label Text="URI:" id="lblURItext" runat="server"></asp:Label>
                    </td>
                    <td align="left" style="height: 24px">
                        <asp:TextBox ID="txtURI" Width="400px" runat="server"></asp:TextBox>
                    </td>
                    <td align="left" style="height: 24px">
                        <asp:Button ID="btnGetURI" Text="Get URI from DB" runat="server" />
                        <asp:Button ID="btnGetTesterURI" Text="Get PCD Interface Emulator URI" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td align="left" style="height: 24px">
                        <asp:Label ID="lblProxyChoice" Text="Proxy: " runat="server"></asp:Label>
                    </td>
                    <td align="left" style="height: 24px">
                        <asp:RadioButtonList ID="rdolstProxyChoice" runat="server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="PCD Interface" Value="1"></asp:ListItem>
                            <asp:ListItem Text="PCD Interface Emulator" Value="2"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
                    <td>
                    </td>
                </tr>
            </table>
            <hr />
            <hr />
            
            <table width="100%">
                <tr>
                    <td colspan="2">
                        <table width="100%" border="0" cellpadding="1" cellspacing="0">
                            <tr>
                                <td style="width:200px;">
                                    <asp:Label ID='lblSPID' runat="server" Text="SP ID:"></asp:Label>
                                </td>
                                <td style="width:auto">
                                    <asp:TextBox ID='txtSPID' runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="width:200px;">
                                    <asp:Label ID='lblPlatFormCode' runat="server" Text="Platform Code:"></asp:Label>
                                </td>
                                <td style="width:auto">
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td style="width: 300px">
                                                 <asp:RadioButtonList ID='rdolstPlatformCode' RepeatDirection="horizontal" runat="server">
                                                       <asp:ListItem Text="HCSP" Value="01"></asp:ListItem>
                                                       <asp:ListItem Text="HCVU" Value="02"></asp:ListItem>
                                                       <asp:ListItem Text="Other, please specify:" Value="99"></asp:ListItem>
                                                 </asp:RadioButtonList>
                                            </td>
                                            <td align="left">
                                                <asp:TextBox ID="txtPlatFormCode" runat="server" Width="60px"></asp:TextBox>
                                            </td>
                                        </tr>
                                    </table>
                                   
                                     
                                </td>
                            </tr>
                            <tr>
                                <td style="width:200px;" valign="top">
                                    <asp:Label ID='lblTypeOfPractice' runat="server" Text="Type of Practice (PCD):"></asp:Label>
                                </td>
                                <td style="width:auto">
                                    <asp:Label ID='lblTypeOfPracticeHelp' runat="server" Text="(will assign the selected type to all practices)"></asp:Label>
                                    <asp:RadioButtonList ID='rdolstTypeOfPractice' runat="server">
                                           <asp:ListItem Text="Hospital Authority" Value="HA"></asp:ListItem>
                                           <asp:ListItem Text="Non-government Organization" Value="NGO"></asp:ListItem>
                                           <asp:ListItem Text="Private" Value="PRIV"></asp:ListItem>
                                           <asp:ListItem Text="University" Value="UNIV"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblGenRequestXML" runat="server" Text="Generate Request XML for function:"></asp:Label>
                                    <asp:RadioButtonList ID='rdolstGenRequestXML' runat="server" RepeatDirection="Horizontal">
                                           <asp:ListItem Text="UploadEnrolInfoRequest" Value="1"></asp:ListItem>
                                           <asp:ListItem Text="CreatePCDSPAcctRequest" Value="2"></asp:ListItem>
                                           <asp:ListItem Text="TransferPracticeInfoRequest" Value="3"></asp:ListItem>
                                           <asp:ListItem Text="CheckIsActiveSP" Value="4"></asp:ListItem>
                                           <asp:ListItem Text="CheckAvailableForVerifiedEnrolment" Value="5"></asp:ListItem>
                                           <asp:ListItem Text="UploadVerifiedEnrolment" Value="6"></asp:ListItem>
                                           <asp:ListItem Text="CheckAccountStatus" Value="7"></asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <asp:Label ID="lblActions" Text="Actions: " runat="server"></asp:Label>
                                    <asp:Button ID="btnGenerateRequestXML" Text="Generate XML!" runat="server" ToolTip="Generate XML for the PCD Web Service selected above" />
                                    <asp:Label ID="lblArrows" Text="---->" runat="server"></asp:Label>
                                    <asp:Button ID="btnSubmitXML" Text="Submit XML!" runat="server" ToolTip="Submit XML to PCD Web Service selected above!" />
                                    <table cellpadding="1" border="0">
                                        <tr>
                                            <td>
                                                <asp:Label ID="lblBeginTime" runat="server"></asp:Label><br />
                                            </td>
                                            <td>
                                                &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            </td>
                                            <td>
                                                <asp:Label ID="lblEndTime" runat="server"></asp:Label><br />
                                            </td>
                                        </tr>
                                    </table>
                                    <asp:Label ID="lblDuration" runat="server"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: left">
                        <asp:Label ID="lblDebugMsg" runat="server" Text="Debugging Message:" ></asp:Label>
                        <div style="text-align: center">
                            <asp:TextBox ID="txtM_DebugMsg" runat="server" TextMode="MultiLine" Height="30px" Width="1000px"></asp:TextBox>
                        </div>
                    </td>                    
                </tr>
                <tr>
                    <td>
                         <asp:Label ID="lblRequestXML" runat="server" Text="XML Request:"></asp:Label>
                         <asp:Button ID="btnRequest_FormatXML" runat="server" Text="Format XML!" />
                        <br />
                        <asp:TextBox ID="txtM_RequestXML" TextMode="MultiLine" runat="server" Height="400px" Width="500px"></asp:TextBox>
                        <br />
                        
                    </td>
                    <td>
                        <asp:Label ID="lblResultXML" Text="XML Result:" runat="server" ></asp:Label>
                        <asp:Button ID="btnResult_FormatXML" runat="server" Text="Format XML!" /> 
                        <asp:Button ID="btnResult_Clear" runat="server" Text="Clear" />
                        <br />
                        <asp:TextBox ID="txtM_ResultXML" TextMode="MultiLine" runat="server" Height="400px" Width="500px">
                            </asp:TextBox>
                        
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Label ID="lblSubmitXML" runat="server" Text="Click to submit to the PCD web service" Visible="False"></asp:Label>
                        <br />
                        <asp:Button ID="btnUploadEnrolInfoRequest" runat="server" Text="UploadEnrolInfoRequest()" Visible="False"/> 
                        <asp:Button ID="btnCreatePCDSPAcct" runat="server" Text="CreatePCDSPAcct()" Visible="False"/> 
                        <asp:Button ID="btnTransferPracticeInfo" runat="server" Text="TransferPracticeInfo()" Visible="False"/> 
                        <asp:Button ID="btnCheckIsActiveSP" runat="server" Text="CheckIsActiveSP()" Visible="False"/> 
                    </td>
                </tr>
                
            </table>
            <!-- hidden fields -->
            <asp:HiddenField ID="hdnMessageID" Value="" runat="server" />
            <asp:HiddenField ID="hdnSelectedXMLFcn" Value="" runat="server" />
            <asp:HiddenField ID="hdnSelectedTypeOfPractice" Value="" runat="server" />
            
    </form>
</body>
</html>
