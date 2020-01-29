<%@ Page Language="vb" AutoEventWireup="false" Codebehind="ProperPractice.aspx.vb"
    Inherits="HCSP.ProperPracticeEN" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <meta http-equiv="pragma" content="no-cache" />    
    <base id="basetag" runat="server" />
    <title>eHealth System - Proper Practice</title>
    <link href="../../CSS/CommonStyle.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        a.LinkToDH {
            color: #0066CC;
        }
        
        a.LinkToDH:visited {
            color: #0066CC;
        }
    
        tr.TrStyle1 {
            height: 30px;
        }
    </style>

    <script type="text/javascript" src="../../JS/Common.js"></script>

</head>
<body>
    <form id="form1" runat="server">
        <cc2:ToolkitScriptManager ID="ScriptManager1" runat="server">
        </cc2:ToolkitScriptManager>
        <div>
            <table border="0" cellpadding="0" cellspacing="0" style="width: 982px">
                <tr>
                    <td align="right" style="background-image: url(../../Images/master/banner_header.jpg);
                        background-repeat: no-repeat; height: 100px" valign="top">
                    </td>
                </tr>
                <tr>
                    <td style="background-position: center bottom; background-image: url(../../Images/master/background.jpg);
                        background-repeat: repeat-x; height: 500px;" valign="top">
                        <table style="width: 100%; height: 500px;" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td valign="top" style="width: 10px">
                                </td>
                                <td valign="top">
                                    <table style="width: 100%">
                                        <tr>
                                            <td>
                                                <img src="../../Images/banner/banner_ProperPractice.png" alt="Proper Practice" />
                                            </td>
                                        </tr>
                                        <tr style="height: 5px">
                                        </tr>
                                        <tr>
                                            <td>
                                                <table style="width: 100%">
                                                    <tr class="TrStyle1">
                                                        <td style="width: 30px">
                                                            <img src="../../Images/others/world.png" alt="" />
                                                        </td>
                                                        <td>
                                                            <a href="http://www.hcv.gov.hk/files/pdf/Proper%20practices%20HCVS_Eng.pdf" target="_blank"
                                                                class="LinkToDH" style="font-size: 16px; font-weight: bold">For Health Care Voucher
                                                                Scheme</a>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr style="height: 40px">
                                        </tr>
                                        <tr>
                                            <td align="center">
                                                <img alt="Close" src="../../Images/button/btn_close.png" onclick="javascript:window.close();"
                                                    style="cursor: pointer" />
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="background-color: #d6d6d6;" align="center">
                        <table style="width: 98%" border="0" cellpadding="0" cellspacing="0">
                            <tr>
                                <td colspan="2" style="height: 2px">
                                    <hr style="width: 98%; color: #ffffff; border-top-style: none; border-right-style: none;
                                        border-left-style: none; height: 1px; border-bottom-style: none;" />
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="height: 19px" class="footerText">
                                    <a href="javascript:void(0)" onclick="openNewHTML('../../../en/PrivacyPolicy.htm')"
                                        class="footerText">Privacy Policy</a>&nbsp;|&nbsp; <a href="javascript:void(0)" onclick="openNewHTML('../../../en/Disclaimer.htm')"
                                            class="footerText">Important Notices</a>&nbsp;|&nbsp; <a href="javascript:void(0)"
                                                onclick="openNewHTML('../../../en/SysMaint.htm')" class="footerText">System Maintenance</a>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>

            <script type="text/javascript">
		        function ResizeScreen() {
			        var w = screen.availWidth||screen.width;
			        var h = screen.availHeight||screen.height;
        				
			        window.moveTo(0,0);
			        window.resizeTo(w,h);
		        }
        		
		        ResizeScreen();
            </script>

        </div>
    </form>
</body>
</html>
