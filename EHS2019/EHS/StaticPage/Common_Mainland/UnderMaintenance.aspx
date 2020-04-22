<%@ Page Language="VB" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<script runat="server">
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim dtmNow As Date = Date.Now

        lblDateTimeChi.Text = dtmNow.ToString("yyyy年MM月dd日") + " &nbsp; 01:00 - 05:00"

    End Sub
</script>

<html xmlns="http://www.w3.org/1999/xhtml" lang="zh-cn">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=UTF-8" />
    <title>医健通(资助)系統</title>
    <link href="Common/CSS/CommonStyle.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript" src="Common/JS/Common.js"></script>

</head>
<body>
    <table border="0" cellpadding="0" cellspacing="0" style="width: 990px">
        <tr>
            <td align="right" style="background-image: url(Common/Images/banner/banner_header_cn.jpg);
                background-repeat: no-repeat; height: 100px" valign="top">
                <table style="width: 100%; height: 100%">
                    <tr>
                        <td align="right" valign="top">
                            <table>
                                <tr>
                                    <td align="right" valign="top" style="height: 23px">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="background-position: center bottom; background-image: url(Common/Images/others/body_background.jpg);
                background-repeat: repeat-x; height: 460px;" valign="top">
                <table style="width: 100%; height: 460px;" border="0" cellpadding="0" cellspacing="0">
                    <tr>
                        <td valign="top" style="width: 10px">
                        </td>
                        <td valign="top">
                            <table width="100%">
                                <tr>
                                    <td align="center">
                                        <br />
                                        <br />
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td>
                                        <!--<table width="75%">
                                            <tr align="left">
                                                   <td colspan="2"><b>Please Select:</b>
                                                   </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <span style="color: #000099"><strong>Service Provider</strong></span><br/></td>
                                                <td>
                                                    <span style="color: #000099"><strong>Service Provider (Enrolled)</strong></span><br/></td>
                                            </tr>
                                            <tr>
                                                <td><a href="../../eform/en/index.aspx"></a>
                                                </td>
                                                <td><a href="../../hcsp/en/index.aspx"></a>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td><a href="javascript:void(0)" onclick="openNewHTML('FAQs.htm')"></a></td>
                                                <td><a href="javascript:void(0)" onclick="openNewHTML('ContactUs.htm')"></a></td>
                                            </tr>
                                        </table>-->
                                        <img src="Common/Images/others/bg_cn.png" alt="" /><br />
                                        <br />
                                        <table style="width: 50%">
                                            <tr>
                                                <td class="tableText" style="text-align: left; font-size: large; color: #0000ff;">
                                                    系统维护时间:</td>
                                                <td class="tableText" style="text-align: left; font-size: large; color: #0000ff;">
                                                    <asp:Label ID="lblDateTimeChi" runat="server"></asp:Label>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <span style="font-size: 10pt; font-family: Arial">
                                            <br />
                                            <br />
                                            本网页的最佳浏览解像度为 1024 x 768。</span>
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

</body>
</html>
