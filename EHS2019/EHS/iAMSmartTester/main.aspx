<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="main.aspx.vb" Inherits="iAMSmartTester.IAS" Title="IAS" %>

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/4.7.0/css/font-awesome.min.css">

<head>
    <title>iAMSmart Tester</title>
    <script type="text/javascript" src="./JS/jquery-3.6.0.js"></script>
    <style>
        body{
            background-color: #343434;
        }
       .css-button {
            font-family: Arial;
            color: #FFFFFF;
            font-size: 16px;
            border-radius: 30px;
            background: #c46b31;
         
            cursor: pointer;
            display: inline-flex;
            align-items: center;
            text-decoration: none;
            font-weight: bold;
        }

        .css-button:hover {
            background: #ac5f2d;
        }

        .css-button-icon {
            padding: 10px 5px 10px 18px;
            
        }

        .css-button-text {
            padding: 10px 18px 10px 5px;
        }
    </style>
</head>

<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true" EnableScriptLocalization="true">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <div>
                    <table style="width: 100%; height: 300px; text-align: center">
                        <tbody>
                            <tr>
                                <td>
                                    <div>
                                         <label style="color: white; font-family:Arial; font-weight:bold">iAM Smart Tester</label>
                                    </div>
                                    <div>
                                        <asp:LinkButton ID="btnQRCode" CssClass="css-button" runat="server" OnClick="btnQRCode_Click">
                                        <span class="css-button-icon"><i class="fa fa-qrcode" aria-hidden="true"></i></span><span class="css-button-text">Get QR Code</span>
                                    </asp:LinkButton>
                                    </div>                     
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </div>
                <%--<asp:Button ID="btnProfile" runat="server" Text="Get Profile" OnClick="btnProfile_Click" />--%>              
                <div style="text-align:center">
                    <table style="width: 800px; min-height: 100px;margin-left:auto;margin-right:auto">
                        <tr style="height:30px;margin: 1px 1px 1px 1px;">
                            <td>
                                <asp:Label ID="lblAESKeyText" runat="server" Text="AES Key:" ForeColor="White" />
                            </td>
                            <td>
                                <asp:Label ID="lblAESKey" runat="server" Text="" ForeColor="White" />
                            </td>
                        </tr>
                        <tr style="height:30px;margin: 1px 1px 1px 1px;display:none">
                            <td>
                                <asp:Label ID="lblSecretKeyText" runat="server" Text="Secret Key:" ForeColor="White" />
                            </td>
                            <td>
                                <asp:Label ID="lblSecretKey" runat="server" Text="" ForeColor="White" />
                            </td>
                        </tr>
                        <tr style="height:30px;margin: 1px 1px 1px 1px;display:none">
                            <td>
                                <asp:Label ID="lblClientIDText" runat="server" Text="Client ID:" ForeColor="White" />
                            </td>
                            <td>
                                <asp:Label ID="lblClientID" runat="server" Text="" ForeColor="White" />
                            </td>
                        </tr>
                        <tr style="height:30px;margin: 1px 1px 1px 1px;">
                            <td>
                                <asp:Label ID="lblAccTokenURLText" runat="server" Text="Access Token URL:" ForeColor="White" />
                            </td>
                            <td>
                                <asp:Label ID="lblAccTokenURL" runat="server" Text="" ForeColor="White" />
                            </td>
                        </tr>
                        <tr style="height:30px;margin: 1px 1px 1px 1px;">
                            <td>
                                <asp:Label ID="lblReturnCodeText" runat="server" Text="Return Code:" ForeColor="White" />
                            </td>
                            <td>
                                <asp:Label ID="lblReturnCode" runat="server" Text="" ForeColor="White" />
                            </td>
                        </tr>
                        <tr style="height:30px;margin: 1px 1px 1px 1px;">
                            <td>
                                <asp:Label ID="lblReturnCodeMsgText" runat="server" Text="Return Code Message:" ForeColor="White" />
                            </td>
                            <td>
                                <asp:Label ID="lblReturnCodeMsg" runat="server" Text="" ForeColor="White" />
                            </td>
                        </tr>
                        <tr style="height:30px;margin: 1px 1px 1px 1px;">
                            <td>
                                <asp:Label ID="lblOpenIDText" runat="server" Text="Open ID:" ForeColor="White" />
                            </td>
                            <td>
                                <asp:Label ID="lblOpenID" runat="server" Text="" ForeColor="White" />
                            </td>
                        </tr>                 
                    </table>
                </div>              
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
</body>
