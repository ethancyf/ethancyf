@Code
    ViewData("Title") = Resource.Text("Home")
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
<div class="container-fluid">
    <div id="Home" class="container">
        <div class="row">
            <div class="col-sm-12">
                <div class="row">
                    <div id="skiptarget" class="col-sm-12 text-1" style="padding-top: 30px;">
                        @Resource.Text("LandingPageTitle")
                    </div>
                </div>
                <div class="row" style="padding-top: 30px;margin-bottom:40px;">
                    <div class="col-xs-12 col-sm-6">
                        <div class="container-fluid part-1">
                            <div class="row text-3 section-title-1">
                                @Resource.Text("LandingPageSPTitle")
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-6 section-1" style="background: url('@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)/Image/common/artwork-menu-bg-1.png') no-repeat ;background-size:cover;">
                                    <div class="row srcleftImgDiv" style="background:rgba(222,150,63,0.85);">
                                        <div class="visible-xs col-xs-3" style="text-align:center">

                                        <img src="~/Image/common/artwork-menu-1.png" class="img-responsive scr_leftimg" alt="" />
                                    </div>
                                    <div class="col-xs-9 col-sm-12">
                                        <div class="row section-row-1">
                                            <div class="col-xs-12 col-sm-12 text-4">
                                                @Resource.Text("LandingPageEMTitle")
                                            </div>
                                        </div>
                                        <div class="row section-row-2">
                                            <div class="col-xs-12 col-sm-12 text-5" id="LandingPageEMText" style="text-align:left">
                                                @Html.Raw(Resource.Text("LandingPageEMText"))
                                            </div>
                                        </div>
                                        <div class="row section-row-3 hidden-xs">
                                            <div class="hidden-xs col-sm-12 text-6" style="padding-top:30px;padding-bottom:30px;">
                                                <img src="~/Image/common/artwork-menu-1.png" class="img-responsive" style="width: 100px;" alt="" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!--ESPL-->
                            <div class="col-xs-12 col-sm-6 section-2" style="background: url('@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)/Image/common/artwork-menu-bg-2.png') no-repeat ;background-size:cover;">
                                <div class="row srcleftImgDiv" style="background:rgba(33,167,121,0.85);">
                                    <div class="visible-xs col-xs-3" style="text-align:center">

                                        <img src="~/Image/common/artwork-menu-2.png" class="img-responsive scr_leftimg" alt="" />
                                    </div>
                                    <div class="col-xs-9 col-sm-12">
                                        <div class="row section-row-1">
                                            <div class="col-sm-12 text-4">
                                                @Resource.Text("LandingPageESPLTitle")
                                                
                                            </div>
                                        </div>
                                        <div class="row section-row-2">
                                            <div class="col-sm-12 text-5" id="LandingPageESPLText" style="text-align:justify">
                                                @Html.Raw(Resource.Text("LandingPageESPLText"))
                                            </div>
                                        </div>
                                        <div class="row section-row-3 hidden-xs">
                                            <div class="hidden-xs col-sm-12 text-6" style="padding-top:30px;padding-bottom:26px;">
                                                <img src="~/Image/common/artwork-menu-2.png" class="img-responsive" style="width: 100px;" alt="" />
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                </div>
                            </div>
                        </div>
                    </div>


                    <div class="col-xs-12 col-sm-6">
                        <div class="container-fluid part-1">
                            <div class="row text-3 section-title-2">
                                @Resource.Text("LandingPageGPTitle")
                            </div>
                            <div class="row">
                                <div class="col-xs-12 col-sm-6 section-3" style="background: url('@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)/Image/common/artwork-menu-bg-3.png') no-repeat ;background-size:cover;">
                                    <div class="row srcleftImgDiv" style="background:rgba(84,155,201,0.85);">
                                        <div class="visible-xs col-xs-3" style="text-align:center">
                                            <img src="~/Image/common/artwork-menu-3.png" class="img-responsive scr_leftimg" alt="" />
                                        </div>
                                        <div class="col-xs-9 col-sm-12">
                                            <div class="row section-row-1" id="lpageTitle3">
                                                <div class="col-xs-12 col-sm-12 text-4">
                                                    @Html.ActionLink(Resource.Text("LandingPageVBETitle"), "Search", "VBE")
                                                </div>
                                            </div>
                                            <div class="row section-row-2" id="lpageCnt3">
                                                <div class="col-xs-12 col-sm-12 text-5" id="LandingPageVBEText" style="text-align:justify">
                                                    @Html.Raw(Resource.Text("LandingPageVBEText"))
                                                </div>
                                            </div>
                                            <div class="row section-row-3 hidden-xs">
                                                <div class="hidden-xs col-sm-12 text-6" style="padding-top:30px;padding-bottom:30px;">
                                                    <img src="~/Image/common/artwork-menu-3.png" class="img-responsive" style="width: 100px;" alt="" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="col-xs-12 col-sm-6 section-4" style="background: url('@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)/Image/common/artwork-menu-bg-4.png') no-repeat ;background-size:cover;">
                                    <div class="row srcleftImgDiv" style="background: rgba(129,104,216,0.85);">
                                        <div class="visible-xs col-xs-3" style="text-align:center">
                                            <img src="~/Image/common/artwork-menu-4.png" class="img-responsive scr_leftimg" alt="" />
                                        </div>
                                        <div class="col-xs-9 col-sm-12">
                                            <div class="row section-row-1" id="lpageTitle4">
                                                <div class="col-sm-12 text-4">
                                                    @Html.ActionLink(Resource.Text("LandingPageESPSTitle"), "Search", "SPS")
                                                </div>
                                            </div>
                                            <div class="row section-row-2" id="lpageCnt4">
                                                <div class="col-sm-12 text-5" id="LandingPageESPSText" style="text-align:justify">
                                                    @Html.Raw(Resource.Text("LandingPageESPSText"))
                                                </div>
                                            </div>
                                            <div class="row section-row-3 hidden-xs">
                                                <div class="hidden-xs col-sm-12 text-6" style="padding-top:30px;padding-bottom:30px;">
                                                    <img src="~/Image/common/artwork-menu-4.png" class="img-responsive" style="width: 100px;" alt="" />
                                                </div>
                                            </div>

                                    </div>
                                </div>

                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

</div>
<script type="text/javascript">
    $(document).ready(function () {
        $('.section-1').on('click', function (event) {
            window.open('@Resource.Parameter("AppLink")/@Resource.Parameter("eFormLink")?lang=@Left(Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower,2)');
        });
        $('.section-2').on('click', function (event) {
            window.open('@Resource.Parameter("AppLink")/@Resource.Parameter("HCSPLink")?lang=@Left(Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower,2)');
        });
        $('.section-3').on('click', function (event) {
            if (currentLanguage == cultureLanguageEnglish) {
                window.location.href = '@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)/en/VBE/Search';
            }
            else {
                window.location.href = '@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)/tc/VBE/Search';
            }
        });
        $('.section-4').on('click', function (event) {
            if (currentLanguage == cultureLanguageEnglish) {
                window.location.href = '@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)/en/SPS/Search';
            }
            else {
                window.location.href = '@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)/tc/SPS/Search';
            }
        });

        $(window).on('resize', function () {
        });
    });
</script>