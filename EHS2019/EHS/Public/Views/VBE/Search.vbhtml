@ModelType VBEValidateResult
@Code
    ViewData("Title") = Resource.Text("LandingPageVBETitle")
    Layout = "~/Views/Shared/_Layout.vbhtml"
    Dim json As String = Newtonsoft.Json.JsonConvert.SerializeObject(Model)
End Code

@Using Html.BeginForm("Result", "VBE", FormMethod.Post)
    @<text>
        <input type="hidden" id="method_flag" name="method" value="post" />
        <div class="container" id="vbe_search" style="min-width:360px;">
            <div class="row vbe-section-1">
                <div class="visible-xs row helpBar">
                    <div class="col-xs-2 helpBar-part-1"></div>
                    <div class="col-xs-1" style="height:100%;">
                        <div class="row helpBar-part-1" style="height:100px;"></div>
                        <div class="row helpBar-Close">
                            @code
                                Dim picHelpCloseName = Threading.Thread.CurrentThread.CurrentCulture.Name + ".png"
                            End code
                            <img src="~/Image/vbe/mobile/HelpBar_Close-@picHelpCloseName" class="img-responsive" style="width:100%" title="@Resource.Text("AriaVBESearchCloseHelpBar")" />
                        </div>
                        <div class="row helpBar-part-1"></div>
                    </div>
                    <div class="col-xs-9 helpBar-part-2">
                        <img src="~/Image/vbe/mobile/HelpBar_img-1.png" class="help-mobile-img img-responsive" title="@Resource.Text("AriaVBESearchHelpBarImg")" />
                    </div>
                </div>
                <div class="row">
                    <div class="row">
                        <div class="col-sm-7">
                            <!--Back Button-->
                            <div class="top-gap" style="display: flex; float: right; margin-right: 30px;">
                                <div class="visible-xs" style="margin-right: 5px;">
                                    <button class="btn btn-default icon_btn" type="button" onclick="OnClickFAQ()">
                                        <div class="icon_container_blue">
                                            ?
                                        </div>
                                        <span class="icon_content">
                                            @Resource.Text("FAQ")
                                        </span>
                                        <span class="sr-only">@Resource.Text("AriaVBESearchFAQ")</span>
                                    </button>
                                </div>
                                <div class="visible-xs">
                                    <button class="btn btn-default icon_btn" type="button" onclick="OnClickGuide()">
                                        <div class="icon_container_blue">
                                            <span class="glyphicon glyphicon-book" aria-hidden="true" style="line-height: 0;top: 3px;right: 1px;"></span>
                                        </div>
                                        <span class="icon_content">@Resource.Text("Guide")</span>
                                        <span class="sr-only">@Resource.Text("AriaVBESearchGuide")</span>
                                    </button>
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-5 col-sm-5 rightSection">
                            <!--FAQ & Guide Button-->
                            <div class="row top-gap">
                                <div class="col-xs-2">
                                </div>
                                <div class="col-xs-4 col-md-3 col-sm-4">
                                    <button class="btn btn-default icon_btn" type="button" onclick="OnClickFAQ()">
                                        <div class="icon_container_blue">
                                            ?
                                        </div>
                                        <span class="icon_content">@Resource.Text("FAQ")</span>
                                        <span class="sr-only">
                                            @Resource.Text("AriaVBESearchFAQ") Link
                                        </span>
                                    </button>
                                </div>
                                <div class="col-xs-4 col-sm-4" id="guideLink">
                                    <button class="btn btn-default icon_btn" type="button" onclick="OnClickGuide()">
                                        <div class="icon_container_blue">
                                            <span class="glyphicon glyphicon-book" aria-hidden="true" style="line-height: 0;top: 3px;right: 1px;"></span>
                                        </div>
                                        <span class="icon_content">@Resource.Text("Guide")</span>
                                        <span class="sr-only">@Resource.Text("AriaVBESearchGuide") Link</span>
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div style="margin-left: 3.8%;">
                        <div class="col-xs-11 col-sm-11">
                            <div class="alert alert-danger alert-dismissible" role="alert" style="display:none;" aria-atomic="true">
                                <span id="txtErrMsg" class="alertStyle">Better check yourself, you're not looking too good.</span>
                            </div>
                        </div>
                    </div>
                    <div class="row ">
                        <div class="col-xs-12 col-sm-7">
                            <div class="row">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-8 col-sm-9">
                                    <h1 id="skiptarget" class="text-1">
                                        @Resource.Text("VoucherBalanceEnquiry")
                                    </h1>
                                </div>
                                <div class="visible-xs col-xs-3 helpBar-Open">
                                    @code
                                        Dim picPath = IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath) + "/Image/vbe/web/help_for_mobile-" + Threading.Thread.CurrentThread.CurrentCulture.Name + ".png"
                                    End Code
                                    <img src="@picPath" class="help_for_mobile" title="@Resource.Text("AriaVBESearchOpenHelpBar")" />
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-10 col-sm-10">
                                    <div id="headlineForIC" class="text-5" style="display:block;">
                                        @Resource.Text("VBENote")
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-10 col-sm-10">
                                    <div class="text-6 line-gap-20">
                                        @Resource.Text("VBESelectID")
                                    </div>
                                </div>
                            </div>
                            <div class="row" id="dob-title" style="display:block">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-10 col-sm-10">
                                    <div class="row typeBackground no-margin no-padding">
                                        <div class="col-xs-6 col-sm-6 activeTitle">
                                            <button type="button" class="hidden-xs hidden-sm btn-lg btn-default activeType" style="width:100%;height:100%;    padding-left: 0px;padding-right: 0px;">
                                                @Resource.Text("VBEHKID")
                                                @*HKID Card*@<span class="sr-only">@Resource.Text("AriaVBESearchInputByHKID")</span>
                                            </button>
                                            <button type="button" class="visible-xs visible-sm btn-lg btn-default activeType" style="width:100%;height:100%">
                                                @Resource.Text("VBEShortHKID")
                                                @*HKID Card*@<span class="sr-only">@Resource.Text("AriaVBESearchInputByHKID") ShortID</span>
                                            </button>
                                        </div>
                                        <div id="btnTypCE" class="col-xs-6 col-sm-6 inactiveTitle">
                                            <a href="javascript:void(0);" class="inactiveType">
                                                <div id="inActiveCEButton" class="inActiveButton">@Resource.Text("VBECertificateOfExemption")</div>
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row" id="yob-title" style="display:none">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-10 col-sm-10">
                                    <div class="row typeBackground no-margin no-padding">
                                        <div id="btnTypID" class="col-xs-6 col-sm-6 inactiveTitle">
                                            <a href="javascript:void(0);" class="hidden-xs hidden-sm inactiveType">
                                                <div class="inActiveButton">@Resource.Text("VBEHKID")</div>
                                            </a>
                                            <a href="javascript:void(0);" class="visible-xs visible-sm inactiveType">
                                                <div class="inActiveButton-shorthkid inActiveButton-ce-ts">@Resource.Text("VBEShortHKID")</div>
                                            </a>
                                        </div>
                                        <div class="col-xs-6 col-sm-6 mobile-gap activeTitle">
                                            <button type="button" class="btn-lg btn-default activeType mobile-gap" style="width:100%;height: 100%;">

                                                @Resource.Text("VBECertificateOfExemption") <span class="sr-only">@Resource.Text("AriaVBESearchInputByCE")</span>
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="row ">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-7 col-sm-10 ">
                                    <div class="text-6 line-gap">
                                        @Resource.Text("VBEInputHKICNo")
                                        <span id="alertHKICMobile" class="visible-xs-inline glyphicon glyphicon-warning-sign alert_symbol_mobile" aria-hidden="true" style="display:none;"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-10 col-sm-10">
                                    <div class=" text-6 line-gap">
                                        <input id="txtHKIC" name="txtHKIC" type="text" class="input-1 form-control " aria-label="@Resource.Text("AriaVBESearchInputHKIC")" placeholder="@Resource.Text("VBEPlaceHolderHKICNo")" autocomplete="off" value="@code If Model IsNot Nothing Then@Model.VBERequestData.Age  End If end code" />
                                    </div>
                                </div>
                                <div class="col-xs-1 col-sm-1 alert_symbol">
                                    <span id="alertHKIC" class="hidden-xs glyphicon glyphicon-warning-sign" aria-hidden="true" style="display:none;"></span>
                                </div>
                            </div>
                            <div class="row" id="Dob_label" style="display:block;">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-7 col-sm-10">
                                    <div class="text-6 line-gap">
                                        @Resource.Text("VBEInputDOB")
                                        <span id="alertDobMobile" class="visible-xs-inline glyphicon glyphicon-warning-sign alert_symbol_mobile" aria-hidden="true" style="display:none;"></span>
                                    </div>
                                </div>
                            </div>

                            <div id="Dob_input" style="display:block;">
                                <div class="row">
                                    <div class="col-xs-1 col-sm-1">
                                    </div>
                                    <div class="col-xs-10 col-sm-10">
                                        <div class="text-6 line-gap">
                                            <input id="txtDob" name="txtDob" type="tel" class="form-control input-1" placeholder="@Resource.Text("VBEPlaceHolderDOB")" autocomplete="off" aria-label="@Resource.Text("AriaVBESearchInputDOB")" />
                                        </div>
                                    </div>
                                    <div class="col-xs-1 col-sm-1 alert_symbol">
                                        <span id="alertDob" class="hidden-xs glyphicon glyphicon-warning-sign" aria-hidden="true" style="display:none;"></span>
                                    </div>
                                </div>

                                <div class="row" style="margin-top:10px;">
                                    <div class="col-xs-1 col-sm-1">
                                    </div>
                                    <div class="col-xs-10 col-sm-10">
                                        <span class="text-9-2">@Resource.Text("VBEDateTips")</span>
                                        <span class="text-10-2" style="color:#015990">@Resource.Text("VBEDateFormatTips")</span>
                                    </div>
                                </div>
                            </div>
                            <!--For Certificate of Exemption-->
                            <div class="row" id="Yob_label" style="display:none;">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-10 col-sm-10">
                                    <div class="text-6 line-gap">
                                        @Resource.Text("VBEInputDOB") / @Resource.Text("VBEInputYOBR")
                                        <span id="alertYobMobile" class="visible-xs-inline glyphicon glyphicon-warning-sign alert_symbol_mobile" aria-hidden="true" style="display:none;"></span>
                                    </div>
                                </div>
                            </div>
                            <div id="Yob_input" style="display:none;">
                                <div class="Yob_for_Screen">
                                    <div class="row">
                                        <div class="col-xs-1 col-sm-1">
                                        </div>
                                        <div class="col-xs-10 col-sm-10">
                                            <div class="row text-6 line-gap" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12 col-sm-12 no-margin no-padding ">
                                                    <select id="selType" name="selType" class="form-control input-2" aria-label="@Resource.Text("AriaVBESearchListDateType")" style="padding-left: 0px;padding-right: 0px;">
                                                        <option value="DOB">
                                                            @Resource.Text("VBEInputDOB")
                                                        </option>
                                                        <option value="YOB">@Resource.Text("VBEInputYOBRShort")</option>
                                                    </select>
                                                </div>

                                                <div class="col-xs-12 visible-xs" style="padding-left: 0px;padding-right: 0px;">
                                                    <div class="row help-text-tips-yob line-gap visible-xs hidden-sm" style="display:none;">
                                                        <div class="help-text help-text-dob-xs" style="display:block; top:0px;">

                                                            <div class="col-xs-12">
                                                                <div class="text-9-2">@Resource.Text("VBEDateTips")</div>
                                                                <div class="text-9-2" style="color:#0171BA">@Resource.Text("VBEDateFormatTips")</div>
                                                            </div>
                                                        </div>
                                                        <div class="help-text help-text-yob-xs" style="display:none;top:0px;">

                                                            <div class="col-xs-12">
                                                                <div class="text-9-2" style="color:#0171BA;">@Resource.Text("VBEAgeTips")</div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row text-6 line-gap" style="margin-left: 0px; margin-right: 0px;">
                                                <div class="col-xs-12 col-sm-12 no-margin no-padding CE_for_Mobile CE_spec_nopaddingright" style="display:block;padding-right: 0px;" id="contDob">
                                                    <input id="txtDob_CE" name="txtDob_CE" type="tel" class="form-control input-1" placeholder="@Resource.Text("VBEPlaceHolderDOB")" autocomplete="off" aria-label="Please input date of birth for certificate exemption." />
                                                </div>
                                                <div style="display:none" id="contYob">
                                                    <div class="col-xs-2 col-sm-2 no-padding">
                                                        <input id="txtAge" name="txtAge" type="number" class="form-control input-2" placeholder="@Resource.Text("Age")" maxlength="3" autocomplete="off"
                                                               value="@code If Model IsNot Nothing Then@Model.VBERequestData.Age  End If end code" title="@Resource.Text("AriaVBESearchInputAge")" style="padding-left: 0px;padding-right: 0px;" />
                                                    </div>
                                                    <div class="col-xs-1 col-sm-1" style=" text-align: center; line-height: 45px;padding-right:0px;">
                                                        @Resource.Text("VBEResultOn")
                                                    </div>
                                                    <div class="col-xs-3 col-sm-3" style="padding-right:0px;">
                                                        <input id="txtDay" name="txtDay" type="number" class="form-control input-2" placeholder="@Resource.Text("Day")" maxlength="2" autocomplete="off" aria-label="@Resource.Text("AriaVBESearchInputDay")" />
                                                    </div>
                                                    <div class="col-xs-3 col-sm-3" style="padding-right:0px;">
                                                        <select id="selMonth" name="selMonth" class="form-control input-2" title="@Resource.Text("AriaVBESearchListMonth")" aria-label="@Resource.Text("AriaVBESearchListMonth")">

                                                            <option value="" disabled selected hidden>@Resource.Text("Month")</option>
                                                            <option value="1" title="@Resource.Text("January")">@Resource.Text("January") </option>
                                                            <option value="2" title="@Resource.Text("February")">@Resource.Text("February")</option>
                                                            <option value="3" title="@Resource.Text("March")">@Resource.Text("March")</option>
                                                            <option value="4" title="@Resource.Text("April")">@Resource.Text("April")</option>
                                                            <option value="5" title="@Resource.Text("May")">@Resource.Text("May")</option>
                                                            <option value="6" title="@Resource.Text("June")">@Resource.Text("June")</option>
                                                            <option value="7" title="@Resource.Text("July")">@Resource.Text("July")</option>
                                                            <option value="8" title="@Resource.Text("August")">@Resource.Text("August")</option>
                                                            <option value="9" title="@Resource.Text("September")">@Resource.Text("September")</option>
                                                            <option value="10" title="@Resource.Text("October")">@Resource.Text("October")</option>
                                                            <option value="11" title="@Resource.Text("November")">@Resource.Text("November")</option>
                                                            <option value="12" title="@Resource.Text("December")">@Resource.Text("December")</option>
                                                        </select>
                                                    </div>
                                                    <div class="col-xs-3 col-sm-3" style="padding-right: 0px;">
                                                        <input id="txtYear" name="txtYear" type="number" class="form-control input-2" placeholder="@Resource.Text("AriaYear")" maxlength="4" autocomplete="off" aria-label="@Resource.Text("AriaYear")" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="col-xs-1 col-sm-1 alert_symbol">
                                            <span id="alertYob" class="hidden-xs glyphicon glyphicon-warning-sign" aria-hidden="true" style="display:none;"></span>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="row">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-7 col-sm-10">
                                    <div class="text-6 line-gap">
                                        @Resource.Text("VBEInputCAPTCHA")
                                        <span id="alertCaptChaMobile" class="visible-xs-inline glyphicon glyphicon-warning-sign alert_symbol_mobile" aria-hidden="true" style="display:none;"></span>
                                    </div>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-4 col-sm-4 col-md-4 col-lg-4" style="padding-right:0px;">
                                    <div class="text-6 line-gap">
                                        <input id="txtCaptcha" type="text" class="form-control input-1" placeholder="@Resource.Text("VBEPlaceHolderCAPTCHA")" autocomplete="off" aria-label="@Resource.Text("AriaVBESearchInputCaptcha")" />
                                    </div>
                                </div>
                                <div class="col-xs-4 col-sm-4 col-md-4 col-lg-3">
                                    <div class="line-gap">
                                        <img src="" class="img-responsive valiCodeImg" style="height:45px" title="@Resource.Text("AriaVBESearchCaptchaPic")" alt="Verification Code" />
                                    </div>
                                </div>
                                <div class="col-xs-2 col-sm-2 col-md-2 col-lg-2 refresh-captcha" style="padding-left:0px;padding-right:0px;">
                                    <div>
                                        <a id="CaptchaVoice" href="javascript:void(0)" tabindex="0" class="glyphicon glyphicon-volume-up" style="margin-top:19px; cursor:pointer; color: #7141be;" aria-label="@Resource.Text("VBEPlayCaptcha")"></a>
                                        <a id="refreshCaptcha" href="javascript:void(0)" tabindex="0" class="glyphicon glyphicon-refresh" style="margin-top:19px; cursor:pointer; color:#7141BE" aria-label="@Resource.Text("VBERefreshCaptcha")"></a>
                                    </div>
                                </div>
                                <div class="col-xs-1 col-sm-1 alert_symbol">
                                    <span id="alertCaptCha" class="hidden-xs glyphicon glyphicon-warning-sign" aria-hidden="true" style="display:none;"></span>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-10 col-sm-10">
                                    <div class="row line-gap-20" style="">
                                        <div class="col-sm-6 col-xs-12 col-md-5">
                                            <button type="submit" class="btn btn-success submitButton" id="btnSubmit">@Resource.Text("VBEBtnCheckBalance") <span class="sr-only">@Resource.Text("AriaVBESearchCheckBalance")</span></button>
                                        </div>
                                        <div class="col-sm-6 col-xs-12 text-HowTo">
                                    <a target="_blank" href="@Resource.Text("VBECreateAccountLinkURL")" style="color:#015990">@Resource.Text("VBECreateAccountLink")</a>
                                </div>
                            </div>                           
                            </div>
                            </div>
                            @*<div class="row visible-xs">
                                <div class="col-xs-1 col-sm-1">
                                </div>
                                <div class="col-xs-10 text-9-2" style="margin-top:5px;">
                                    <a target="_blank" href="@Resource.Text("VBECreateAccountLinkURL")">@Resource.Text("VBECreateAccountLink")</a>
                                </div>
                            </div>*@
                        </div>
                        <div class="col-sm-5 hidden-xs rightSection">
                            <!--FAQ & Guide Button-->
                            <div class="row ">
                                <div class="col-sm-11 col-md-10 help-web-div">
                                    @code
                                        Dim picName = Threading.Thread.CurrentThread.CurrentCulture.Name + ".png"
                                    End code
                                    <img src="~/Image/vbe/web/help-1-@picName" class="img-responsive help-img" title="@Resource.Text("AriaVBESearchHelpBarImg")" aria-hidden="true" />
                                    <div class="help-text-tips hidden-xs" style="display:none;">
                                        <div class="row help-text help-text-dob" style="display:block;">
                                            <div class="text-9-2" style="color:#0171BA;">@Resource.Text("VBEDateTips")</div>
                                            <div class="text-10-2" style="color:#0171BA">@Resource.Text("VBEDateFormatTips")</div>
                                        </div>
                                        <div class="row help-text help-text-yob" style="display:none;">
                                            <div class="text-9-2" style="color:#0171BA;padding-top: 10px;">@Resource.Text("VBEAgeTips")</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <input type="hidden" id="inputType" name="inputType" value="" title="Hidden field">
            <audio id="voice"></audio>
        </div>
    </text>
End Using
<script type="text/javascript">
    var applicationPath = '@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)';
</script>
<script src="~/js/vbe.js"></script>
<script src="~/js/pageTimeout.js"></script>

<script type="text/javascript">
    var picData = {};
    var theString = '@(json)';
    theString = theString.replace(/&quot;/g, "\"");
    var jObject = JSON.parse(theString);
    callJs(jObject);

    timeOut = @ViewBag.PageTimeout * 60 * 1000; //set time
    var captchaAudioFolder = '@ViewBag.CaptchaAudioFolder'
    var played = false;


    $(document).ready(function () {
        $("#txtHKIC").val("");
        $("#txtDob").val("");
        $("#txtCaptcha").val("");
        $("#txtDob_CE").val("");
        getErrorMsg(rootLang);
    });

    function getErrorMsg(Lang) {
        if (sessionStorage.getItem("errMsg")) {
            errMsgList = JSON.parse(sessionStorage.getItem("errMsg"))
        }
        else {
            if (!sessionStorage.getItem("lang")) {
                sessionStorage.setItem("lang", Lang);
            }
            $.ajax({
                async: false,
                url: applicationPath+"/"+sessionStorage.getItem("lang") + "/value/systemmsg",
                dataType: "json",
                success: function (data) {
                    if (data) {
                        errMsgList = data;
                        sessionStorage.setItem("errMsg", JSON.stringify(data))
                    }
                },
                complete: function (XMLHttpRequest, textStatus) {
                    var errorUrl = XMLHttpRequest.getResponseHeader("ErrorUrl");
                    if (errorUrl) {
                        var u = rootPath + rootLang + errorUrl;
                        location.href = u;
                    }
                }
            });
        }
    }
</script>
