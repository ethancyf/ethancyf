<%@ Page Language="vb" AutoEventWireup="false" Codebehind="Default.aspx.vb" Inherits="RSAAuthManager._Default" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>RSA Authentication Manager</title>    
    <link rel="stylesheet" href="css/jquery.ui.tooltip.css" />
    <style type="text/css">
    * {
        font-family: Tahoma;
        font-size: 8pt;
    }
    
    body {
        background-color: #EFEFEF;
    }
    
    .TrHead {
        height: 30px;
    }
    
    .TdHeadLeft {
        font-size: 12pt;
        font-weight: bold;
        color: #FFFFFF;
        padding-left: 44px;
        width: 250px;
        background-image: url(img/head_left.png);
        background-repeat: no-repeat;
    }
    
    .TdHeadMid {
        background-image: url(img/head_mid.png);
        background-repeat: repeat-x;
        background-position: bottom;
    }
    
    .TdHeadRight {
        width: 1px;
        background-image: url(img/head_right.png);
        background-repeat: no-repeat;
        background-position: bottom;
    }
    
    .TdHeadLeftAF {
        font-size: 12pt;
        font-weight: bold;
        color: #FFFFFF;
        padding-left: 44px;
        width: 250px;
        background-image: url(img/head_left_af.png);
        background-repeat: no-repeat;
    }
    
    .TdHeadLeftUAT {
        font-size: 12pt;
        font-weight: bold;
        color: #FFFFFF;
        padding-left: 44px;
        width: 250px;
        background-image: url(img/head_left_uat.png);
        background-repeat: no-repeat;
    }
    
    .Execute {
        border: 1px solid #000000;
        background-color: #FFFFFF;
        background-image: url(img/executebtn.png);
        background-position: right;
        background-repeat: no-repeat;
        width: 70px;
        text-align: left;
    }
    
    .Clear {
        border: 1px solid #000000;
        background-color: #FFFFFF;
        background-image: url(img/clearbtn.png);
        background-position: right;
        background-repeat: no-repeat;
        width: 70px;
        text-align: left;
    }
    
    .SPGridView {
        border: 1px solid #000000;
    }
    
    .SPGridView th, .SPGridView td {
        border: 1px solid #000000;
    }
    
    .ui-tooltip {
		background-color: yellow;
		border: 1px solid #BBBBBB;
	}

    .fixed {
      position: fixed;
      top:0; 
      left:0;
      width: 100%; 
      background: #FFFFEF;
      border-bottom: 1px solid #CCCCCC;
    }
    </style>

    <script type="text/javascript" src="js/jquery-1.10.2.js"></script>

    <script type="text/javascript" src="js/jquery.ui.core.js"></script>

    <script type="text/javascript" src="js/jquery.ui.widget.js"></script>

    <script type="text/javascript" src="js/jquery.ui.position.js"></script>

    <script type="text/javascript" src="js/jquery.ui.tooltip.js"></script>

    <script type="text/javascript">
	    function pageLoad() {
		    $(document).tooltip({
		        items: "[tt]",
		        content: function() {
		            return $(this).attr("tt");
		        },
		        show: {
		            effect: "slideDown"
		        },
		        hide: {
		            effect: "explode"
		        },
		    });
		    
		    $("[token=1]").blur(function() {
		        if ($(this).val() != "") {
		            $(this).val($.trim($(this).val()));
		            $(this).val(("000000000000" + $(this).val()).slice(-12));
		        }
		    }).keypress(function(e) {
		        if (e.which == 13) {
		            if ($(this).val() != "") {
		                $(this).val($.trim($(this).val()));
		                $(this).val(("000000000000" + $(this).val()).slice(-12));
		            }
		        }
		    });
		    
		    $("[token=2]").blur(function() {
		        if ($(this).val() != "") {
		            var lines = $(this).val().split(/\n/);
                    var texts = [];
                    for (var i = 0; i < lines.length; i++) {
                        lines[i] = $.trim(lines[i]);
                        
                        if (lines[i] != "") {
                            texts.push(("000000000000" + lines[i]).slice(-12));
                        }
                    }
                    
                    $(this).val("");
		        
		            for (var i = 0; i < texts.length; i++) {
                        if ($(this).val() != "") {
                            $(this).val($(this).val() + "\n");
                        }
                        
                        $(this).val($(this).val() + texts[i]);
                    }
		        }
            });
		    
		    $("[trim=1]").blur(function() {
		        $(this).val($.trim($(this).val()));
		    }).keypress(function(e) {
		        if (e.which == 13) {
		            $(this).val($.trim($(this).val()));
		        }
		    });
		    
		    $("[trigger=1]").children(':first').click(function () {
		        if ($(this).is(':checked')) {
                    $("[triggerdepend=1]").removeAttr('disabled');
                } else {
                    $("[triggerdepend=1]").prop('selectedIndex', 0);
                    $("[triggerdepend=1]").attr("disabled", "disabled");
                }
		    });
		    
		    // Init
		    if ($("[trigger=1]").children(':first').is(':checked')) {
		         $("[triggerdepend=1]").removeAttr('disabled');
            } else {
                $("[triggerdepend=1]").prop('selectedIndex', 0);
                $("[triggerdepend=1]").attr("disabled", "disabled");
            }

	    }

	</script>

</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:ScriptManager ID="ScriptManager1" runat="server" OnAsyncPostBackError="ScriptManager1_AsyncPostBackError">
            </asp:ScriptManager>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnLogin" />
                    <asp:PostBackTrigger ControlID="btnLogout" />
                </Triggers>
                <ContentTemplate>
                    <asp:Panel ID="panCore" runat="server" DefaultButton="btnNone">
                        <asp:Button ID="btnNone" runat="server" Enabled="false" Style="display: none" />
                        <asp:MultiView ID="mvCore" runat="server" ActiveViewIndex="0">
                            <asp:View ID="vLogin" runat="server">
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr class="TrHead">
                                                    <td class="TdHeadLeft">
                                                        Login
                                                    </td>
                                                    <td class="TdHeadMid">
                                                        &nbsp;</td>
                                                    <td class="TdHeadRight">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #022E5D; padding: 1px">
                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr>
                                                    <td style="background-color: #FFFFFF; padding: 10px; width: 160px; border-right: 1px solid #022E5D;
                                                        vertical-align: top">
                                                        <table>
                                                            <tr style="height: 18px">
                                                                <td style="font-weight: bold; width: 100px; padding-top: 8px; vertical-align: top">
                                                                    Link
                                                                </td>
                                                                <td>
                                                                    <asp:RadioButtonList ID="rblLink" runat="server"  />
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 18px">
                                                                <td style="font-weight: bold">
                                                                    Login ID
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLoginID" runat="server"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr style="height: 18px">
                                                                <td style="font-weight: bold">
                                                                    Password
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnLogin" runat="server" Text="Login" OnClick="btnLogin_Click" class="Execute" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Label ID="lblLoginResult" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </asp:View>
                            <asp:View ID="vContent" runat="server">
                                <div class="fixed">
                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                        <tr style="height: 20px;">
                                            <td style="font-weight: bold;" align="center">
                                                <asp:Label ID="lblServer" runat="server"></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <br />
                                <br />
                                <asp:Panel ID="panSearchToken" runat="server">
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr class="TrHead">
                                                    <td class="TdHeadLeft">
                                                        Search Principal
                                                    </td>
                                                    <td class="TdHeadMid">
                                                        &nbsp;</td>
                                                    <td class="TdHeadRight">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #022E5D; padding: 1px">
                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr>
                                                    <td style="background-color: #FFFFFF; padding: 10px; width: 160px; border-right: 1px solid #022E5D;
                                                        vertical-align: top">
                                                        <table>
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnSPExecute" runat="server" class="Execute" Text="Execute" OnClick="btnSPExecute_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="font-weight: bold">
                                                                    User ID
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlSPUserID" runat="server" Width="100">
                                                                        <asp:ListItem Text="contains" Value="contain"></asp:ListItem>
                                                                        <asp:ListItem Text="starts with" Value="start"></asp:ListItem>
                                                                        <asp:ListItem Text="equals" Value="equal"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:TextBox ID="txtSPUserID" runat="server" trim="1"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="font-weight: bold">
                                                                    Principal Enable
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlSPPEnable" runat="server" Width="100">
                                                                        <asp:ListItem Text="-- Any --" Value="any"></asp:ListItem>
                                                                        <asp:ListItem Text="Yes" Value="yes"></asp:ListItem>
                                                                        <asp:ListItem Text="No" Value="no"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="font-weight: bold">
                                                                    Token
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cboSPListToken" runat="server" Text="List Tokens" Checked="true"
                                                                        trigger="1" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td style="font-weight: bold">
                                                                    Token Enable
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:DropDownList ID="ddlSPTEnable" runat="server" Width="100" triggerdepend="1">
                                                                        <asp:ListItem Text="-- Any --" Value="any"></asp:ListItem>
                                                                        <asp:ListItem Text="Yes" Value="yes"></asp:ListItem>
                                                                        <asp:ListItem Text="No" Value="no"></asp:ListItem>
                                                                    </asp:DropDownList>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="background-color: #FFFFFF; padding: 10px; vertical-align: top">
                                                        <asp:MultiView ID="mvSPContent" runat="server" ActiveViewIndex="0">
                                                            <asp:View ID="vSPContent" runat="server">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td>
                                                                            <asp:Button ID="btnSPClear" runat="server" class="Clear" Text="Clear" OnClick="btnSPClear_Click" />
                                                                            &nbsp; &nbsp;
                                                                            <asp:Label ID="lblSPResult" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td>
                                                                            <asp:GridView ID="gvSP" runat="server" AlternatingRowStyle-BackColor="#FFE5FA" AutoGenerateColumns="false" class="SPGridView" HeaderStyle-Height="25px" OnPreRender="gvSP_PreRender" OnRowDataBound="gvSP_RowDataBound" RowStyle-BackColor="#FFFFFF" RowStyle-HorizontalAlign="Center">
                                                                                <Columns>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            User ID</HeaderTemplate>
                                                                                        <HeaderStyle BackColor="#E7EFF7" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSPGUserID" runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="160" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            Enable</HeaderTemplate>
                                                                                        <HeaderStyle BackColor="#E7EFF7" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSPGEnable" runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="60" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            Fail Count</HeaderTemplate>
                                                                                        <HeaderStyle BackColor="#E7EFF7" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSPGFailCount" runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="70" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            Lockout</HeaderTemplate>
                                                                                        <HeaderStyle BackColor="#E7EFF7" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSPGLockout" runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="60" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            Token Serial No.</HeaderTemplate>
                                                                                        <HeaderStyle BackColor="#FFFF57" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSPGToken" runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="100" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            Enable</HeaderTemplate>
                                                                                        <HeaderStyle BackColor="#FFFF57" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSPGTokenEnable" runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="60" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            Fail Count</HeaderTemplate>
                                                                                        <HeaderStyle BackColor="#FFFF57" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSPGTokenFailCount" runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="70" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            Next Token Mode</HeaderTemplate>
                                                                                        <HeaderStyle BackColor="#FFFF57" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSPGTokenNextTokenMode" runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="110" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            <span style="border-bottom: dotted 1px #000000; cursor: help" tt="0: Not in replacement&lt;br /&gt;1: Has a replacement token&lt;br /&gt;2: Is a replacement token&lt;br /&gt;3: Has been replaced">
                                                                                                Replacement Mode</span></HeaderTemplate>
                                                                                        <HeaderStyle BackColor="#FFFF57" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSPGTokenReplacementMode" runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="120" />
                                                                                    </asp:TemplateField>
                                                                                    <asp:TemplateField>
                                                                                        <HeaderTemplate>
                                                                                            Replacement Serial No.</HeaderTemplate>
                                                                                        <HeaderStyle BackColor="#FFFF57" />
                                                                                        <ItemTemplate>
                                                                                            <asp:Label ID="lblSPGTokenReplacementToken" runat="server"></asp:Label>
                                                                                        </ItemTemplate>
                                                                                        <ItemStyle Width="140" />
                                                                                    </asp:TemplateField>
                                                                                </Columns>
                                                                            </asp:GridView>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:View>
                                                            <asp:View ID="vSPQuestion" runat="server">
                                                                <table style="width: 100%">
                                                                    <tr>
                                                                        <td style="padding-top: 30px; text-align: center">
                                                                            <span style="font-weight: bold; text-decoration: underline">Confirmation</span>
                                                                            <br />
                                                                            <br />
                                                                            You are enquiring
                                                                            <asp:Label ID="lblSPQToken" runat="server" Font-Bold="true"></asp:Label>
                                                                            token records at the same time, this will take a long time. Are you sure?
                                                                            <br />
                                                                            <br />
                                                                            <br />
                                                                            <asp:Button ID="btnSPQNo" runat="server" class="Clear" Text="No" OnClick="btnSPQNo_Click" />
                                                                            &nbsp;
                                                                            <asp:Button ID="btnSPQYes" runat="server" class="Execute" Text="Yes" OnClick="btnSPQYes_Click" />
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </asp:View>
                                                        </asp:MultiView>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                    <tr>
                                        <td>
                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr class="TrHead">
                                                    <td class="TdHeadLeft">
                                                        Search Token
                                                    </td>
                                                    <td class="TdHeadMid">
                                                        &nbsp;</td>
                                                    <td class="TdHeadRight">
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="background-color: #022E5D; padding: 1px">
                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                <tr>
                                                    <td style="background-color: #FFFFFF; padding: 10px; width: 250px; border-right: 1px solid #022E5D">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td style="width: 100px; vertical-align: top">
                                                                    Token Serial No.
                                                                </td>
                                                                <td>
                                                                    <asp:TextBox ID="txtLTToken" runat="server" token="1"></asp:TextBox>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                </td>
                                                                <td>
                                                                    <asp:Button ID="btnLTExecute" runat="server" class="Execute" Text="Execute" OnClick="btnLTExecute_Click" />
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:CheckBox ID="cboLTAdvMode" runat="server" Text="Multiple" ForeColor="#666666"
                                                                        AutoPostBack="true" OnCheckedChanged="cboLTAdvMode_CheckedChanged" />
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                    <td style="background-color: #FFFFFF; padding: 10px; vertical-align: top">
                                                        <table style="width: 100%">
                                                            <tr>
                                                                <td>
                                                                    <asp:Button ID="btnLTClear" runat="server" class="Clear" Text="Clear" OnClick="btnLTClear_Click" />
                                                                    &nbsp; &nbsp;
                                                                    <asp:Label ID="lblLTResult" runat="server"></asp:Label>
                                                                </td>
                                                            </tr>
                                                            <tr>
                                                                <td>
                                                                    <asp:GridView ID="gvLT" runat="server" AutoGenerateColumns="false" class="SPGridView" HeaderStyle-BackColor="#FFFF57" HeaderStyle-Height="25px" OnRowDataBound="gvLT_RowDataBound" RowStyle-BackColor="#FFFFFF" RowStyle-HorizontalAlign="Center">
                                                                        <Columns>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    Serial No.</HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLTGToken" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="100" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    Enable</HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLTGEnable" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="60" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    Fail Count</HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLTGFailCount" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="70" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    Next Token Mode</HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLTGNextTokenMode" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="110" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    <span style="border-bottom: dotted 1px #000000; cursor: help" tt="0: Not in replacement&lt;br /&gt;1: Has a replacement token&lt;br /&gt;2: Is a replacement token&lt;br /&gt;3: Has been replaced">
                                                                                        Replacement Mode</span></HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLTGReplacementMode" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="120" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    Replacement Serial No.</HeaderTemplate>
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLTGReplacementToken" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="140" />
                                                                            </asp:TemplateField>
                                                                            <asp:TemplateField>
                                                                                <HeaderTemplate>
                                                                                    User ID</HeaderTemplate>
                                                                                <HeaderStyle BackColor="#E7EFF7" />
                                                                                <ItemTemplate>
                                                                                    <asp:Label ID="lblLTGUserID" runat="server"></asp:Label>
                                                                                </ItemTemplate>
                                                                                <ItemStyle Width="160" />
                                                                            </asp:TemplateField>
                                                                        </Columns>
                                                                    </asp:GridView>
                                                                </td>
                                                            </tr>
                                                        </table>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                                <br />
                                <br />
                                </asp:Panel>
                                <asp:MultiView ID="mvAF" runat="server" ActiveViewIndex="0">
                                    <asp:View ID="vAFHide" runat="server">
                                        <table cellpadding="0" cellspacing="0" style="width: 100%">
                                            <tr>
                                                <td>
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <tr class="TrHead">
                                                            <td class="TdHeadLeftAF">
                                                                Advanced Features
                                                            </td>
                                                            <td class="TdHeadMid">
                                                                &nbsp;</td>
                                                            <td class="TdHeadRight">
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="background-color: #022E5D; padding: 1px">
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <tr>
                                                            <td style="background-color: #FFFFFF; padding: 10px">
                                                                <table cellpadding="2" cellspacing="2">
                                                                    <tr>
                                                                        <td style="width: 70px">
                                                                            Password:
                                                                        </td>
                                                                        <td style="width: 120px">
                                                                            <asp:TextBox ID="txtAFPassword" runat="server" TextMode="Password" Width="100"></asp:TextBox>
                                                                        </td>
                                                                        <td>
                                                                            <asp:Button ID="btnAFExecute" runat="server" Text="Activate Advanced Features" class="Execute"
                                                                                Width="165" OnClick="btnAFExecute_Click" />
                                                                        </td>
                                                                        <td>
                                                                            <asp:Label ID="lblAFResult" runat="server"></asp:Label>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:View>
                                    <asp:View ID="vAFShow" runat="server">
                                        <asp:MultiView ID="mvAFShow" runat="server" ActiveViewIndex="0">
                                            <asp:View ID="vAFWSShow" runat="server">
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr class="TrHead">
                                                                    <td class="TdHeadLeftAF" style="background-position-y: 7px; padding-top: 5px">
                                                                        Authenticate (WS + C)
                                                                    </td>
                                                                    <td class="TdHeadMid">
                                                                        <table cellpadding="0px" style="padding-bottom: 5px">
                                                                            <tr style="font-weight: bold">
                                                                                <td style="width: 100px">
                                                                                    WS App Pool:
                                                                                </td>
                                                                                <td style="width: 30px">
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td style="width: 180px">
                                                                                    Config Path:<span style="color: red"> *</span>
                                                                                </td>
                                                                                <td style="width: 50px">
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    WS Link:
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblWSAppPool" runat="server" ></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblWSConfPath" runat="server" ></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblWSLink" runat="server" ></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>                                 
                                                                    </td>
                                                                    <td class="TdHeadRight">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #022E5D; padding: 1px">
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr>
                                                                    <td style="background-color: #FFFFFF; padding: 10px">
                                                                        <table>
                                                                            <tr>
                                                                                <td style="width: 90px">
                                                                                    User ID
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAWUserID" runat="server" trim="1"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Token Passcode
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAWPasscode" runat="server" trim="1"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td colspan="3" style="color: #888888">
                                                                                    Note: This function cannot handle Next Token Mode.
                                                                                    <br />
                                                                                    <span style="color: red; margin-left: -9px">* </span>
                                                                                    Note: The Config Path is for reference only, the config file stored in the WS App Pool may not be the same as display.
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:Button ID="btnAWExecute" runat="server" class="Execute" Text="Execute" OnClick="btnAWExecute_Click" />
                                                                                    &nbsp; &nbsp;
                                                                                    <asp:Label ID="lblAWResult" runat="server"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </asp:View>
                                            <asp:View ID="vAFRSAShow" runat="server">
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr class="TrHead">
                                                                    <td class="TdHeadLeftAF">
                                                                        Authenticate (.NET)
                                                                    </td>
                                                                    <td class="TdHeadMid">
                                                                        &nbsp;</td>
                                                                    <td class="TdHeadRight">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #022E5D; padding: 1px">
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr>
                                                                    <td style="background-color: #FFFFFF; padding: 10px">
                                                                        <table>
                                                                            <tr>
                                                                                <td style="width: 90px">
                                                                                    User ID
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAUserID" runat="server" trim="1"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Token Passcode
                                                                                </td>
                                                                                <td>
                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                        <tr>
                                                                                            <td style="width: 160px">
                                                                                                <asp:TextBox ID="txtAPasscode" runat="server" trim="1"></asp:TextBox>
                                                                                            </td>
                                                                                            <td id="tdANextPasscodeText" runat="server" style="width: 90px">
                                                                                                Next Passcode
                                                                                            </td>
                                                                                            <td id="tdANextPasscode" runat="server" style="padding-right: 30px">
                                                                                                <asp:TextBox ID="txtANextPasscode" runat="server" trim="1"></asp:TextBox>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:Button ID="btnACancel" runat="server" class="Clear" Text="Cancel Inputting Next Passcode"
                                                                                        Width="185" Style="padding-right: 20px" OnClick="btnACancel_Click" />
                                                                                    <asp:Label ID="lblACancelSpace" runat="server" Text="&nbsp;"></asp:Label>
                                                                                    <asp:Button ID="btnAExecute" runat="server" class="Execute" Text="Execute" OnClick="btnAExecute_Click" />
                                                                                    &nbsp; &nbsp;
                                                                                    <asp:Label ID="lblAResult" runat="server"></asp:Label>
                                                                                    <asp:HiddenField ID="hfASessionId" runat="server" />
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <br />
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr class="TrHead">
                                                                    <td class="TdHeadLeftAF" style="background-position-y: 7px; padding-top: 5px">
                                                                        Authenticate (Direct C)
                                                                    </td>
                                                                    <td class="TdHeadMid">
                                                                        <table cellpadding="0px" style="padding-bottom: 5px">                                                               
                                                                            <tr style="font-weight: bold">
                                                                                <td style="width: 100px">
                                                                                    App Pool:
                                                                                </td>
                                                                                <td style="width: 30px">
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td style="width: 180px">
                                                                                    Config Path:
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    <asp:Label ID="lblDAppPool" runat="server" ></asp:Label>
                                                                                </td>
                                                                                <td>
                                                                                    &nbsp;
                                                                                </td>
                                                                                <td>
                                                                                    <asp:Label ID="lblDConfPath" runat="server" ></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                    <td class="TdHeadRight">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #022E5D; padding: 1px">
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr>
                                                                    <td style="background-color: #FFFFFF; padding: 10px">
                                                                        <table>
                                                                            <tr>
                                                                                <td style="width: 90px">
                                                                                    User ID
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtACUserID" runat="server" trim="1"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Token Passcode
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtACPasscode" runat="server" trim="1"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td colspan="3" style="color: #888888">
                                                                                    Note: This function cannot handle Next Token Mode.
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td colspan="3">
                                                                                    <asp:Button ID="btnACExecute" runat="server" class="Execute" Text="Execute" OnClick="btnACExecute_Click" />
                                                                                    &nbsp; &nbsp;
                                                                                    <asp:Label ID="lblACResult" runat="server"></asp:Label>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>                                        
                                                <br />
                                                <br />
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr class="TrHead">
                                                                    <td class="TdHeadLeftAF">
                                                                        Add Principal
                                                                    </td>
                                                                    <td class="TdHeadMid">
                                                                        &nbsp;</td>
                                                                    <td class="TdHeadRight">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #022E5D; padding: 1px">
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr>
                                                                    <td style="background-color: #FFFFFF; padding: 10px">
                                                                        <table>
                                                                            <tr>
                                                                                <td style="width: 90px">
                                                                                    User ID
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAPUserID" runat="server" trim="1"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Token Serial No.
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtAPToken" runat="server" token="1"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:MultiView ID="mvAP" runat="server" ActiveViewIndex="0">
                                                                                        <asp:View ID="vAPButton" runat="server">
                                                                                            <asp:Button ID="btnAPExecute" runat="server" class="Execute" Text="Execute" OnClick="btnAPExecute_Click" />
                                                                                            &nbsp; &nbsp;
                                                                                            <asp:Label ID="lblAPResult" runat="server"></asp:Label>
                                                                                        </asp:View>
                                                                                        <asp:View ID="vAPConfirm" runat="server">
                                                                                            <table cellpadding="0" cellspacing="0">
                                                                                                <tr>
                                                                                                    <td style="font-weight: bold">
                                                                                                        Confirm?
                                                                                                    </td>
                                                                                                    <td style="padding-left: 16px">
                                                                                                        <asp:Button ID="btnAPENo" runat="server" class="Clear" Text="No" OnClick="btnAPENo_Click" />
                                                                                                    </td>
                                                                                                    <td style="padding-left: 8px">
                                                                                                        <asp:Button ID="btnAPEYes" runat="server" class="Execute" Text="Yes" OnClick="btnAPEYes_Click" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:View>
                                                                                    </asp:MultiView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <br />
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr class="TrHead">
                                                                    <td class="TdHeadLeftAF">
                                                                        Update Principal / Token
                                                                    </td>
                                                                    <td class="TdHeadMid">
                                                                        &nbsp;</td>
                                                                    <td class="TdHeadRight">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #022E5D; padding: 1px">
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr>
                                                                    <td style="background-color: #FFFFFF; padding: 10px">
                                                                        <table style="width: 100%">
                                                                            <tr>
                                                                                <td>
                                                                                    <table>
                                                                                        <tr>
                                                                                            <td style="width: 90px">
                                                                                                User ID
                                                                                            </td>
                                                                                            <td>
                                                                                                <asp:TextBox ID="txtUPUserID" runat="server" trim="1"></asp:TextBox>
                                                                                            </td>
                                                                                            <td style="padding-left: 15px">
                                                                                                <asp:Button ID="btnUPFind" runat="server" class="Execute" Text="Find" OnClick="btnUPFind_Click" />
                                                                                                &nbsp; &nbsp;
                                                                                                <asp:Label ID="lblUPResult" runat="server"></asp:Label>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                            <tr id="trUPC" runat="server">
                                                                                <td>
                                                                                    <table cellpadding="1" cellspacing="1" style="background-color: #AAAAAA">
                                                                                        <tr>
                                                                                            <td style="background-color: #FFFFFF; padding: 10px">
                                                                                                <table style="width: 100%">
                                                                                                    <tr>
                                                                                                        <td style="width: 100px">Enable </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="cboUPCEnable" runat="server" />
                                                                                                            <asp:HiddenField ID="hfUPCPrincipalGuid" runat="server" />
                                                                                                            <asp:HiddenField ID="hfUPCPrincipalRowVersion" runat="server" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr id="trUPCLockout" runat="server">
                                                                                                        <td>Lockout </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="cboUPCLockout" runat="server" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>Token Serial No. </td>
                                                                                                        <td style="padding-left: 4px">
                                                                                                            <asp:TextBox ID="txtUPCToken" runat="server" token="1"></asp:TextBox>
                                                                                                            <asp:HiddenField ID="hfUPCToken" runat="server" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td>Token Enable </td>
                                                                                                        <td>
                                                                                                            <asp:CheckBox ID="cboUPCTokenEnable" runat="server" />
                                                                                                            <asp:HiddenField ID="hfUPCTokenEnable" runat="server" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                    <tr>
                                                                                                        <td colspan="2" style="text-align: center">
                                                                                                            <asp:MultiView ID="mvUPC" runat="server" ActiveViewIndex="0">
                                                                                                                <asp:View ID="vUPCButton" runat="server">
                                                                                                                    <asp:Button ID="btnUPCCancel" runat="server" class="Clear" OnClick="btnUPCCancel_Click" Text="Cancel" />
                                                                                                                    &nbsp;
                                                                                                                    <asp:Button ID="btnUPCExecute" runat="server" class="Execute" OnClick="btnUPCExecute_Click" Text="Execute" />
                                                                                                                </asp:View>
                                                                                                                <asp:View ID="vUPCConfirm" runat="server">
                                                                                                                    <table cellpadding="0" cellspacing="0">
                                                                                                                        <tr>
                                                                                                                            <td style="font-weight: bold">Confirm? </td>
                                                                                                                            <td style="padding-left: 16px">
                                                                                                                                <asp:Button ID="btnUPCENo" runat="server" class="Clear" OnClick="btnUPCENo_Click" Text="No" />
                                                                                                                            </td>
                                                                                                                            <td style="padding-left: 8px">
                                                                                                                                <asp:Button ID="btnUPCEYes" runat="server" class="Execute" OnClick="btnUPCEYes_Click" Text="Yes" />
                                                                                                                            </td>
                                                                                                                        </tr>
                                                                                                                    </table>
                                                                                                                </asp:View>
                                                                                                            </asp:MultiView>
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </td>
                                                                                        </tr>
                                                                                    </table>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <br />
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr class="TrHead">
                                                                    <td class="TdHeadLeftAF">
                                                                        Delete Principal
                                                                    </td>
                                                                    <td class="TdHeadMid">
                                                                        &nbsp;</td>
                                                                    <td class="TdHeadRight">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #022E5D; padding: 1px">
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr>
                                                                    <td style="background-color: #FFFFFF; padding: 10px">
                                                                        <table>
                                                                            <tr>
                                                                                <td style="width: 90px">
                                                                                    User ID
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtDPUserID" runat="server" trim="1"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:MultiView ID="mvDP" runat="server" ActiveViewIndex="0">
                                                                                        <asp:View ID="vDPButton" runat="server">
                                                                                            <asp:Button ID="btnDPExecute" runat="server" class="Execute" Text="Execute" OnClick="btnDPExecute_Click" />
                                                                                            &nbsp; &nbsp;
                                                                                            <asp:Label ID="lblDPResult" runat="server"></asp:Label>
                                                                                        </asp:View>
                                                                                        <asp:View ID="vDPConfirm" runat="server">
                                                                                            <table cellpadding="0" cellspacing="0">
                                                                                                <tr>
                                                                                                    <td style="font-weight: bold">
                                                                                                        Confirm?
                                                                                                    </td>
                                                                                                    <td style="padding-left: 16px">
                                                                                                        <asp:Button ID="btnDPENo" runat="server" class="Clear" Text="No" OnClick="btnDPENo_Click" />
                                                                                                    </td>
                                                                                                    <td style="padding-left: 8px">
                                                                                                        <asp:Button ID="btnDPEYes" runat="server" class="Execute" Text="Yes" OnClick="btnDPEYes_Click" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:View>
                                                                                    </asp:MultiView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <br />
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr class="TrHead">
                                                                    <td class="TdHeadLeftAF">
                                                                        Clear Token Fail Count
                                                                    </td>
                                                                    <td class="TdHeadMid">
                                                                        &nbsp;</td>
                                                                    <td class="TdHeadRight">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #022E5D; padding: 1px">
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr>
                                                                    <td style="background-color: #FFFFFF; padding: 10px">
                                                                        <table>
                                                                            <tr>
                                                                                <td style="width: 90px">
                                                                                    User ID
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtCTUserID" runat="server" trim="1"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:MultiView ID="mvCT" runat="server" ActiveViewIndex="0">
                                                                                        <asp:View ID="vCTButton" runat="server">
                                                                                            <asp:Button ID="btnCTExecute" runat="server" class="Execute" Text="Execute" OnClick="btnCTExecute_Click" />
                                                                                            &nbsp; &nbsp;
                                                                                            <asp:Label ID="lblCTResult" runat="server"></asp:Label>
                                                                                        </asp:View>
                                                                                        <asp:View ID="vCTConfirm" runat="server">
                                                                                            <table cellpadding="0" cellspacing="0">
                                                                                                <tr>
                                                                                                    <td style="font-weight: bold">
                                                                                                        Confirm?
                                                                                                    </td>
                                                                                                    <td style="padding-left: 16px">
                                                                                                        <asp:Button ID="btnCTENo" runat="server" class="Clear" Text="No" OnClick="btnCTENo_Click" />
                                                                                                    </td>
                                                                                                    <td style="padding-left: 8px">
                                                                                                        <asp:Button ID="btnCTEYes" runat="server" class="Execute" Text="Yes" OnClick="btnCTEYes_Click" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:View>
                                                                                    </asp:MultiView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <br />
                                                <br />
                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                    <tr>
                                                        <td>
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr class="TrHead">
                                                                    <td class="TdHeadLeftAF">
                                                                        Replace Token
                                                                    </td>
                                                                    <td class="TdHeadMid">
                                                                        &nbsp;</td>
                                                                    <td class="TdHeadRight">
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                    <tr>
                                                        <td style="background-color: #022E5D; padding: 1px">
                                                            <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                <tr>
                                                                    <td style="background-color: #FFFFFF; padding: 10px">
                                                                        <table>
                                                                            <tr>
                                                                                <td style="width: 150px">
                                                                                    Token Serial No.
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtRTOldToken" runat="server" token="1"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                    Replacement Token Serial No.
                                                                                </td>
                                                                                <td>
                                                                                    <asp:TextBox ID="txtRTReplaceToken" runat="server" token="1"></asp:TextBox>
                                                                                </td>
                                                                            </tr>
                                                                            <tr>
                                                                                <td>
                                                                                </td>
                                                                                <td>
                                                                                    <asp:MultiView ID="mvRT" runat="server" ActiveViewIndex="0">
                                                                                        <asp:View ID="vRTButton" runat="server">
                                                                                            <asp:Button ID="btnRTExecute" runat="server" class="Execute" Text="Execute" OnClick="btnRTExecute_Click" />
                                                                                            &nbsp; &nbsp;
                                                                                            <asp:Label ID="lblRTResult" runat="server"></asp:Label>
                                                                                        </asp:View>
                                                                                        <asp:View ID="vRTConfirm" runat="server">
                                                                                            <table cellpadding="0" cellspacing="0">
                                                                                                <tr>
                                                                                                    <td style="font-weight: bold">
                                                                                                        Confirm?
                                                                                                    </td>
                                                                                                    <td style="padding-left: 16px">
                                                                                                        <asp:Button ID="btnRTENo" runat="server" class="Clear" Text="No" OnClick="btnRTENo_Click" />
                                                                                                    </td>
                                                                                                    <td style="padding-left: 8px">
                                                                                                        <asp:Button ID="btnRTEYes" runat="server" class="Execute" Text="Yes" OnClick="btnRTEYes_Click" />
                                                                                                    </td>
                                                                                                </tr>
                                                                                            </table>
                                                                                        </asp:View>
                                                                                    </asp:MultiView>
                                                                                </td>
                                                                            </tr>
                                                                        </table>
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:Panel ID="panRUD" runat="server">
                                                    <br />
                                                    <br />
                                                    <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                        <tr>
                                                            <td>
                                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <tr class="TrHead">
                                                                        <td class="TdHeadLeftUAT">
                                                                            Reset UAT Data
                                                                        </td>
                                                                        <td class="TdHeadMid">
                                                                            &nbsp;</td>
                                                                        <td class="TdHeadRight">
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td style="background-color: #022E5D; padding: 1px">
                                                                <table cellpadding="0" cellspacing="0" style="width: 100%">
                                                                    <tr>
                                                                        <td style="background-color: #FFFFFF; padding: 10px">
                                                                            <table>
                                                                                <tr>
                                                                                    <td style="width: 70px; vertical-align: top">
                                                                                        Data
                                                                                    </td>
                                                                                    <td style="vertical-align: top">
                                                                                        <asp:TextBox ID="txtRUDData" runat="server" TextMode="MultiLine" Width="200" Height="100"></asp:TextBox>
                                                                                    </td>
                                                                                    <td style="color: #888888; vertical-align: top">
                                                                                        Format:<br />
                                                                                        &nbsp; {PrincipalID},{Token},{ReplacementToken}<br />
                                                                                        <br />
                                                                                        Example:<br />
                                                                                        &nbsp; 90001234,229641111,<br />
                                                                                        &nbsp; 90005678,229643333,229644444<br />
                                                                                        <br />
                                                                                        Note:<br />
                                                                                        &nbsp; Each line must contain 2 commas
                                                                                    </td>
                                                                                </tr>
                                                                                <tr>
                                                                                    <td>
                                                                                    </td>
                                                                                    <td>
                                                                                        <asp:MultiView ID="mvRUD" runat="server" ActiveViewIndex="0">
                                                                                            <asp:View ID="vRUDButton" runat="server">
                                                                                                <asp:Button ID="btnRUDExecute" runat="server" class="Execute" Text="Execute" OnClick="btnRUDExecute_Click" />
                                                                                                &nbsp; &nbsp;
                                                                                                <asp:Label ID="lblRUDResult" runat="server"></asp:Label>
                                                                                            </asp:View>
                                                                                            <asp:View ID="vRUDConfirm" runat="server">
                                                                                                <table cellpadding="0" cellspacing="0">
                                                                                                    <tr>
                                                                                                        <td style="font-weight: bold">
                                                                                                            Confirm?
                                                                                                        </td>
                                                                                                        <td style="padding-left: 16px">
                                                                                                            <asp:Button ID="btnRUDENo" runat="server" class="Clear" Text="No" OnClick="btnRUDENo_Click" />
                                                                                                        </td>
                                                                                                        <td style="padding-left: 8px">
                                                                                                            <asp:Button ID="btnRUDEYes" runat="server" class="Execute" Text="Yes" OnClick="btnRUDEYes_Click" />
                                                                                                        </td>
                                                                                                    </tr>
                                                                                                </table>
                                                                                            </asp:View>
                                                                                        </asp:MultiView>
                                                                                    </td>
                                                                                </tr>
                                                                            </table>
                                                                        </td>
                                                                    </tr>
                                                                </table>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </asp:Panel>
                                            </asp:View>
                                        </asp:MultiView>                                        
                                        <br />
                                        <asp:Button ID="btnAFHide" runat="server" Text="Hide Advanced Features" class="Clear"
                                            Width="160" OnClick="btnAFHide_Click" />
                                        <br />
                                    </asp:View>                                    
                                </asp:MultiView>
                                <br />
                                <br />
                                <asp:Button ID="btnLogout" runat="server" Text="Logout" OnClick="btnLogout_Click" />
                            </asp:View>
                        </asp:MultiView>
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </form>
</body>
</html>
