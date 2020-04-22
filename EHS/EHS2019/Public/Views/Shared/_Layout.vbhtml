@code
    Dim langType As LanguageCollection.LanguageType = New LanguageCollection.LanguageType()
End Code
<!DOCTYPE html>
<html lang="@Left(Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower,2)">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>@Resource.Text("eHealthSystem") - @ViewBag.Title </title>
    <link href="~/Content/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" type="text/css" href="~/styles/main_styles.css">
    <link rel="stylesheet" type="text/css" href="~/styles/tabpage.css" />
    <link href="~/Styles/ServiceProviderSearch.css" rel="stylesheet" />
    <link href="~/styles/vbeStyle.css" rel="stylesheet" />
    <link href="~/styles/Home.css" rel="stylesheet" />
    <link href="~/styles/staticStyle.css" rel="stylesheet" />
    <link href="~/styles/responsive.css" rel="stylesheet" />
    <link href="~/styles/Print.css" rel="stylesheet" />
    <script src="~/Scripts/modernizr-2.6.2.js"></script>
    <script src="~/Scripts/jquery-3.4.1.min.js"></script>
    <script src="~/Scripts/bootstrap.min.js"></script>
    <script src="~/js/common.js"></script>

    <script type="text/javascript">
    var rootPath = "@ViewBag.ApplicationPath" == "/" ? "@ViewBag.ApplicationPath" : "@ViewBag.ApplicationPath" + "/";
        var rootLang = "@ViewBag.Lang";
        var currentLanguage = "@Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower";
        var cultureLanguageEnglish = '@Common.Component.CultureLanguage.English';
        var cultureLanguageTradChinese = '@Common.Component.CultureLanguage.TradChinese';
    window.onload = function onloading() {
        if ($(".mainContent").length) {
            $(".mainContent").css("background-image", "url('@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)/Image/SPS/bg-base.png')");
        }
        if ($("#Home").length) {
            $(".mainContent").css("background-image", "url('@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)/Image/common/bg-home.png')")
                .css("background-attachment","fixed");
        }
        //var lang = "@Threading.Thread.CurrentThread.CurrentCulture.Name";
        switch (currentLanguage) {
            case cultureLanguageEnglish:
                $(".en_menu").css("display", "inline-flex");
                $(".ch_menu").css("display", "none");
                $(".en").css("color", "#0171BA");
                $("#en_m_link").css("font-weight", "600");
                break;
            case cultureLanguageTradChinese:
                $(".en_menu").css("display", "none");
                $(".ch_menu").css("display", "inline-flex");
                $("#lang_switch").addClass("lang_ch");
                $("#tc_link").css("color", "#0171BA");
                $("#tc_m_link").css("font-weight", "600");
                break;
        }
        if (currentLanguage == cultureLanguageEnglish) {
            $('#useLang').attr('data-uselang', 'en');
        }
        if (currentLanguage == cultureLanguageTradChinese) {
            $('#useLang').attr('data-uselang', 'tc');
        }
    }
    </script>

</head>
<body>

    <div class="super_container">

        <div class="skip_content">
            <a href="#skiptarget" class="skip_link" title="Skip to main content">@Resource.Text("SkipToMainContent")</a>
        </div>
        <div id="useLang" style="display:none;" data-uselang=""></div>
        <header class="header">
            <div class="header_container" style="height:90px;">
                <div class="header_content d-flex flex-row align-items-center justify-content-start">
                    <div class="logo">
                        @*<a id="logoID" href="@Url.Action("Home", "Home")"><img src="~/Image/logoicon/artwork-ehs-logo.png" alt="eHealth"></a>
                        <a href="@Url.Action("Home", "Home")"><img src="~/Image/logoicon/artwork-ehealth-subsidy.png" alt="eHealth system"></a>*@
                        <a id="logoID" href="@Url.Action("Home", "Home")">
                            <table>
                                <tr>
                                    <td><img src="~/Image/logoicon/artwork-ehs-logo.png" alt="eHealth" class="logo_img1"></td>
                                    <td style="padding-left:10px"><img src="~/Image/logoicon/artwork-ehealth-subsidy.png" alt="eHealth system" class="logo_img2"></td>
                                </tr>
                            </table>
                        </a>
                    </div>
                    <nav class="main_nav">
                        <div class="header_menu destopMenu">
                            <ul class="en_menu ml-auto" style="display: inline-flex ;">
                                <li class="menu_1" style="text-align:left !important">@Html.ActionLink(Resource.Text("Home"), "Home", "Home")</li>
                                <li class="menu_2" style="text-align:left !important">@Html.ActionLink(Resource.Text("VoucherBalanceEnquiry"), "Search", "VBE")</li>
                                <li class="menu_3" style="text-align:left !important">@Html.ActionLink(Resource.Text("SearchProvider"), "Search", "SPS")</li>
                                <li class="menu_5" style="text-align:left !important">@Html.ActionLink(Resource.Text("ContactUs"), "ContactUs", "CU")</li>
                            </ul>
                            <ul class="ch_menu ml-auto" style="display: none;padding-right:20px">
                                <li class="menu_1" style="text-align:left !important">@Html.ActionLink(Resource.Text("Home"), "Home", "Home")</li>
                                <li class="menu_2" style="text-align:left !important">@Html.ActionLink(Resource.Text("VoucherBalanceEnquiry"), "Search", "VBE")</li>
                                <li class="menu_3" style="text-align:left !important">@Html.ActionLink(Resource.Text("SearchProvider"), "Search", "SPS")</li>
                                <li class="menu_5" style="text-align:left !important">@Html.ActionLink(Resource.Text("ContactUs"), "ContactUs", "CU")</li>
                            </ul>


                        </div>
                        <div class="lang" id="lang_switch">
                            <span style="float:left;" class="destopMenu">
                                <a id="textSizeLink" href="@Url.Action("TextSize", "Static")" target="_blank" aria-label="Text Size" style="font-weight:normal">@Resource.Text("TextSizeTitle")</a>
                            </span>
                            <span style="float:left;padding-right:5px;color:#707070;" class="destopMenu">|</span>
                            <span style="float:left;">
                                <a id="tc_link" href="#" class="lnkML tc" data-langc="tc" aria-label="繁體">繁</a>
                            </span>
                            <span style="float:left;padding-right:5px;color:#707070;">|</span>
                            <span>
                                <a href="#" class="lnkML en" data-langc="en" aria-label="English">Eng</a>
                            </span>
                        </div>
                    </nav>
                    <div class="header_extra ml-auto">
                        @*<button class="btn btn-default global" title="@Resource.Text("AriaMobileLanguage")" type="button">
                                <span class="glyphicon glyphicon-globe" aria-hidden="true"></span>
                            </button>*@
                        <button class="btn btn-default hamburger" title="@Resource.Text("AriaMobileMenu")" type="button" alt="@Resource.Text("AriaMobileMenu")">
                            <span class="glyphicon glyphicon-menu-hamburger" aria-hidden="true"></span>
                        </button>
                        <button class="btn btn-default btnclose" title="@Resource.Text("AriaCloseMobileMenu")" type="button" style="display:none;" alt="@Resource.Text("AriaCloseMobileMenu")">
                            <span class="glyphicon glyphicon-remove" aria-hidden="true"></span>
                        </button>
                    </div>
                </div>
            </div>
        </header>

        <nav class="menu trans_300">
            <div class="menu_container">
                <div class="page_menu_content">
                    <ul class="page_menu_nav">
                        <li class="page_menu_item">@Html.ActionLink(Resource.Text("Home"), "Home", "Home")</li>
                        <li class="page_menu_item">@Html.ActionLink(Resource.Text("VoucherBalanceEnquiry"), "Search", "VBE")</li>
                        <li class="page_menu_item">@Html.ActionLink(Resource.Text("SearchProvider"), "Search", "SPS")</li>
                        <li class="page_menu_item">@Html.ActionLink(Resource.Text("ContactUs"), "ContactUs", "CU")</li>
                    </ul>
                </div>
            </div>
        </nav>

        <nav class="langmenu trans_300">
            <div class="menu_container">
                <div class="page_menu_content">
                    <ul class="page_menu_nav">
                        <li class="page_menu_item">
                            <a id="en_m_link" href="#" class="lnkML en" data-langc="en">Eng</a>
                        </li>
                        <li class="page_menu_item">
                            <a id="tc_m_link" href="#" class="lnkML tc" data-langc="tc">繁體</a>
                        </li>

                    </ul>
                </div>
            </div>
        </nav>
        <!--Content-->
        <div class="mainContent">
            <a href="~/Files/FAQs_c.pdf#Public" target="_blank" rel="noopener noreferrer" style="display:none" id="zhFAQ">zhFAQ</a>
            <a href="~/Files/FAQs_e.pdf#Public" target="_blank" rel="noopener noreferrer" style="display:none" id="enFAQ">enFAQ</a>
            <a href="~/Files/EasyGuide_CHI.pps" target="_blank" rel="noopener noreferrer" style="display:none" id="zhGuide">zhGuide</a>
            <a href="~/Files/EasyGuide.pps" target="_blank" rel="noopener noreferrer" style="display:none" id="enGuide">enGuide</a>
            @RenderBody()

            <!--back to top-->
            <div class="container-fluid backtotop_container" style="padding-bottom:25px;display:none">
                <div class="row">
                    <div class="col-12" style="text-align:center">
                        <button class="btn btn-default icon_btn backtotop" type="button">
                            <span class="icon_container">
                                <span class="glyphicon glyphicon-chevron-up" aria-hidden="false"></span>
                            </span>
                            <span class="icon_content">@Resource.Text("BackToTop")</span>
                        </button>
                    </div>
                </div>
            </div>
        </div>

        <!--footer-->
        <footer class="footer footer_background">
            <div class="container-fluid footer_content" style="padding-top: 15px">
                <div class="row" style="margin-top:0px;margin-bottom:15px;font-size:16px;">
                    <div class="col-md-4">
                        <ul>
                            <li>@Html.ActionLink(Resource.Text("Home"), "Home", "Home")</li>
                            <li style="height:30px"></li>
                            <li style="display:block">@Html.ActionLink(Resource.Text("VoucherBalanceEnquiry"), "Search", "VBE")</li>
                            @*<li>@Html.ActionLink(Resource.Text("MainPublic"), "Home", "Home")</li>*@
                            <li style="height:30px"></li>
                            <li>@Html.ActionLink(Resource.Text("SearchProvider"), "Search", "SPS")</li>
                        </ul>
                    </div>
                    <div class="col-md-4">
                        <ul>
                            <li>@Html.ActionLink(Resource.Text("ContactUs"), "ContactUs", "CU")</li>
                            <li>@Resource.Text("HealthCareVoucherUnit")</li>
                            @*<li>@Resource.Text("Email_ShotForm"): <a class="footer_conect" href="mailto:hcvd@dh.gov.hk">hcvd@dh.gov.hk</a></li>
                            <li>@Resource.Text("Tel"): <a class="footer_conect" href="tel:28382311">2838 2311</a></li>
                            <li>@Resource.Text("Site"): <a class="footer_conect" href="@Resource.Text("hcvURL")" target="_blank" rel="noopener noreferrer">http://www.hcv.gov.hk</a></li>*@
                        </ul>
                        <table>
                            <tr>
                                <td>@Resource.Text("Email_ShotForm"):&nbsp;</td>
                                <td><a class="footer_conect" href="mailto:hcvd@dh.gov.hk">hcvd@dh.gov.hk</a></td>
                            </tr>
                            <tr>
                                <td>@Resource.Text("Tel"):&nbsp;</td>
                                <td><a class="footer_conect" href="tel:28382311">2838 2311</a></td>
                            </tr>
                            <tr>
                                <td>@Resource.Text("Site"):&nbsp;</td>
                                <td><a class="footer_conect" href="@Resource.Text("hcvURL")" target="_blank" rel="noopener noreferrer">http://www.hcv.gov.hk</a></td>
                            </tr>
                        </table>
                    </div>
                    <div class="col-md-4">
                        <ul>
                            <li style="display:inline-block"></li>
                            <li>@Resource.Text("ProManagementAndVaccDivision")</li>
                            @*<li>@Resource.Text("Email_ShotForm"): <a class="footer_conect" href="mailto:vacs@dh.gov.hk">vacs@dh.gov.hk</a></li>
                            <li>@Resource.Text("Tel"): <a class="footer_conect" href="tel:21252125">2125 2125</a></li>
                            <li>@Resource.Text("Site"): <a class="footer_conect" href="@Resource.Text("chpURL")" target="_blank" rel="noopener noreferrer">http://www.chp.gov.hk</a></li>*@
                        </ul>
                        <table>
                            <tr>
                                <td>@Resource.Text("Email_ShotForm"):&nbsp;</td>
                                <td><a class="footer_conect" href="mailto:vacs@dh.gov.hk">vacs@dh.gov.hk</a></td>
                            </tr>
                            <tr>
                                <td>@Resource.Text("Tel"):&nbsp;</td>
                                <td><a class="footer_conect" href="tel:21252125">2125 2125</a></td>
                            </tr>
                            <tr>
                                <td>@Resource.Text("Site"):&nbsp;</td>
                                <td><a class="footer_conect" href="@Resource.Text("chpURL")" target="_blank" rel="noopener noreferrer">http://www.chp.gov.hk</a></td>
                            </tr>
                        </table>
                    </div>
                </div>
            </div>
            <div style="background-color:#225482;color:white">
                <div class="container-fluid" style="line-height:40px">
                    <div class="row">
                        <div class="col-sm-4 col-xs-12 copy_right">

                        </div>
                        <div class="col-sm-8 col-xs-12 footer_link">
                            <ul>
                                <li>@Html.ActionLink(Resource.Text("PrivacyPolicy"), "PrivacyPolicy", "Static")</li>
                                <li>@Html.ActionLink(Resource.Text("ImportantNotices"), "ImportantNotice", "Static")</li>
                                <li>@Html.ActionLink(Resource.Text("SystemMaintenance"), "SystemMaintenance", "Static")</li>
                                <li>
                                    <img src="~/Image/logoicon/wcag2.1AA-v.png" alt="Level Double-A conformance icon, W3C-WAI Web Content Accessibility Guidelines 2.1" width="88" height="28">

                                </li>
                            </ul>
                        </div>
                    </div>
                </div>
            </div>
        </footer>
    </div>
</body>
</html>