@ModelType VBEResult
@Code
    ViewData("Title") = Resource.Text("VoucherBalance")
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code
<div class="container" id="vbe_result" style="padding-bottom: 20px;">
    <div class="row section-1">
        <div class="row top-gap">
            <div class="col-xs-1 col-sm-1">
            </div>
            <div class="col-xs-4 col-sm-4" style="padding-left: 0px;">
                <button class="btn btn-default icon_btn" type="button" onclick="OnClickBackVRE();">
                    <div class="icon_container">
                        <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                    </div>
                    <span class="icon_content">@Resource.Text("AriaVBESearchBack")</span>
                </button>
            </div>
            <div class="col-xs-6 col-sm-6" style="padding-right: 0px;">
                <div style="display: flex;float: right;">
                    <div>
                        <button class="btn btn-default icon_btn " type="button" style="margin-right: 20px;" onclick="OnClickFAQ()">
                            <div class="icon_container icon_bg_blue">
                                <span class="glyphicon glyphicon-question-sign" aria-hidden="true"></span>
                            </div>
                            <span class="icon_content">@Resource.Text("FAQ")</span>
                        </button>
                    </div>
                    <div>
                        <button class="btn btn-default icon_btn " type="button" onclick="OnClickGuide()">
                            <div class="icon_container icon_bg_blue">
                                <span class="glyphicon glyphicon-book" aria-hidden="true"></span>
                            </div>
                            <span class="icon_content">@Resource.Text("Guide")</span>
                        </button>
                    </div>
                </div>
            </div>
        </div>
        <!--Voucher Balance Review-->
        <div class="row">
            <div class="col-xs-1 col-sm-1">
            </div>
            <div class="col-xs-10 col-sm-10">
                <div>
                    <div class="row section-1-1">
                        <div class="col-xs-12 col-sm-7 header_1">
                            <div class="row text-1">
                                @Resource.Text("VoucherBalanceEnquiry")
                            </div>
                            <div class="row text-5">
                                @Resource.Text("VBENote")
                            </div>
                        </div>
                        <div class="col-xs-5 col-sm-5">
                        </div>
                    </div>
                    <div class="row section-1-3" style="margin-bottom: 20px;">
                        <div class="col-xs-12 col-sm-6 col-md-5 text-7-2 no-padding ">
                            <span>
                                @Resource.Text("VBEInputHKICNo") : 
                            </span>
                            <span>
                                <span class="text-8-2">
                                    @Model.HKIC_No
                                </span>
                            </span>
                        </div>
                        <div class="col-xs-12 col-sm-6 col-md-5 text-7-2 no-padding">
                            <span>
                                @code
                                    If Model.DateType = "DOB" Then
                                    @Resource.Text("VBEInputDOB")
                                    Else
                                    @Resource.Text("VBEInputYOBR")
                                    End If
                                End code
                                :
                            </span>
                            <span class="text-8-2">
                                @code
                                    If Model.DateType = "DOB" Then
                                @Model.DateOfBirth
                                    Else
                                        If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower().Equals(Common.Component.CultureLanguage.English) Then
                                        @<br />
                                        End If
                                        @<span>@Resource.Text("Age")</span>
                                @Model.Age
                                @<span>@Resource.Text("VBEResultOn") </span>@<span>@Model.Day.ToString("0#")</span>@<span>-</span>@Model.Month.ToString("0#")@<span>-</span>@Model.Year
                                    End If
                                End code
                            </span>
                        </div>
                    </div>
                    <div class="row section-1-2" style="margin-bottom:20px">
                        <div class="col-xs-12 col-sm-12">
                            <div class="row line-gap">
                                <div class="col-xs-9 col-sm-6 text-7-2">
                                    @Resource.Text("VBEResultCurrentPosition") :
                                </div>
                                <div class="col-xs-3 hidden-sm hidden-md hidden-lg text-10">
                                    HK$
                                </div>
                            </div>
                            <div class="row line-gap">
                                <div class="col-xs-9 col-sm-9 text-9-2">
                                    @Resource.Text("VBEResultAvailableVoucherBalance") 
                                    @Code
                                        If Model.VBERequestData.InputType = "IC" Then
                                            @<span style='font-size:11px;position:relative;top:-8px;'>@Resource.Text("VBEResultNote")</span>
                                        End If
                                    End code
                                </div>
                                <div class="hidden-xs col-sm-3 text-10">
                                    HK$
                                    @String.Format("{0:N0}", Model.AvailableAmount)
                                </div>
                                <div class="col-xs-3 hidden-sm hidden-md hidden-lg text-10">
                                    @String.Format("{0:N0}", Model.AvailableAmount)
                                </div>
                            </div>
                            <div class="row line-gap">
                                <div class="col-xs-9 col-sm-9 text-9-2">
                                    @Resource.Text("VBEResultCurPosMaximumUsedAmount")
                                </div>
                                <div class="hidden-xs col-sm-3 text-10">
                                    HK$
                                    @String.Format("{0:N0}", Model.MaximumOptometry)
                                </div>
                                <div class="col-xs-3 hidden-sm hidden-md hidden-lg text-10">
                                    @String.Format("{0:N0}", Model.MaximumOptometry)
                                </div>
                            </div>
                            <div class="row dashed_line">
                                <div class="col-xs-9 col-sm-9 text-9">
                                    @Resource.Text("VBEResultCurPosMaximumUsedAmountNote")
                                </div>
                            </div>
						</div>
					</div>
                    <div class="row section-1-2" style="padding-bottom:20px">
                        <div class="col-xs-12 col-sm-12 line-gap">
                            <div class="row">
                                <div class="hidden-xs col-xs-9 col-sm-9 text-7-2">
                                    @Resource.Text("VBEResultProjectPos")
                                </div>
                            </div>
                            <div class="row">
                                <div class="visible-xs col-xs-9 col-sm-9 text-7-2">
                                    @Resource.Text("VBEResultProjectPos")
                                </div>
                                <div class="col-xs-3 hidden-sm hidden-md hidden-lg text-10">
                                    HK$
                                </div>
                            </div>
                            <div class="row line-gap">
                                <div class="col-xs-9 col-sm-9 text-9-2">
                                   @Resource.Text("VBEResultPrjPosProvideAmount")
                                </div>
                                <div class="hidden-xs col-sm-3 text-10">
                                    HK$
                                    @String.Format("{0:N0}", Model.ProjectedAvailableAmount)
                                </div>
                                <div class="col-xs-3 hidden-sm hidden-md hidden-lg text-10">
                                    @String.Format("{0:N0}", Model.ProjectedAvailableAmount)
                                </div>
                            </div>
                            <div class="row line-gap">
                                <div class="col-xs-9 col-sm-9 text-9-2">
                                    @Resource.Text("VBEResultPrjPosExceedLimit")
                                </div>
                                <div class='hidden-xs col-sm-3 text-10 @IIf(Model.ForfeitedAmount>0,"red-text","")'>
                                    HK$
                                    @String.Format("{0:N0}", Model.ForfeitedAmount)
                                </div>
                                <div class='col-xs-3 hidden-sm hidden-md hidden-lg text-10 @IIf(Model.ForfeitedAmount>0,"red-text","")'>
                                    @String.Format("{0:N0}", Model.ForfeitedAmount)
                                </div>
                            </div>
                            <div class="row">
                                <div class="col-xs-9 col-sm-9 text-9">
                                    (@Resource.Text("VBEResultPrjPosExceedLimitNote")
                                    <span>
                                        $@String.Format("{0:N0}", Model.PrjPosExceedLimit))
                                    </span>
                                </div>
                            </div>
                        </div>
                    </div>

	                <!--For Reference-->
	                <div class="row section-1-1 text-7-2 line-gap-20">
	                    @Resource.Text("VBEResultForRefer") :
	                </div>
	                <div class="row hidden-sm hidden-md hidden-lg " style="padding-top: 20px;background: #FFFFFF;">
	                    <div class="col-xs-9 text-7-2">
	                        @Resource.Text("VBEResultCurrentPosition") :
	                    </div>
	                    <div class="col-xs-3 text-10">
	                        HK$
	                    </div>
	                </div>
	                <div class="row" style="background: #FFFFFF;">
	                    <div class="col-xs-8 col-sm-9 col-md-10 text-9-2 line-gap">
	                        @Resource.Text("VBEResultForReferAvailableQuota")
                            @*<span class="hidden-xs">
                                @If Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower = Common.Component.CultureLanguage.English Then
                                    @Resource.Text("ROP")
                                End If
                            </span>*@
                        </div>
                 	    <div class="hidden-xs col-sm-3 col-md-2 text-10 line-gap">
	                        HK$
	                        @String.Format("{0:N0}", Model.AvailableQuotaOptometry)
	                    </div>
	                    <div class="col-xs-4 hidden-sm hidden-md hidden-lg text-10 line-gap">
	                        @String.Format("{0:N0}", Model.AvailableQuotaOptometry) 
                            <br />
                            <span class="text-10 visible-xs" style="padding-bottom:10px;white-space:nowrap;float:right">
                                @Resource.Text("VBEResultUpToDate").Replace("{{DATE}}", Model.UpToDate)
                            </span>
	                    </div>
	                </div>

                    <div class="row hidden-xs" style="padding-bottom: 10px;background: #FFFFFF;">
                        <div class="col-xs-3 text-10" style="width:100%;float:right">
                            @Resource.Text("VBEResultUpToDate").Replace("{{DATE}}", Model.UpToDate)
                        </div>
                    </div>

	                <div class="row" style="padding-bottom: 20px;background: #FFFFFF;">
	                @*<div class="hidden-xs row" style="padding-bottom: 20px;background: #FFFFFF;">*@
	                    <div class="col-xs-6 col-sm-8 col-md-9 text-9-2" style="width:100%">
	                        @Resource.Text("VBEResultForReferAvailableQuotaNote") 
	                    </div>
	                    @*<div class="col-xs-6 col-sm-4 col-md-3 text-10 line-gap">
	                        @Resource.Text("VBEResultUpToDate").Replace("{{DATE}}", Model.UpToDate)
	                    </div>*@
	                </div>

	                @*<div class="visible-xs row" style="padding-bottom: 20px;background: #FFFFFF;">
	                    <div class="col-xs-6 text-9-2 line-gap">
                            @If Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower = Common.Component.CultureLanguage.English Then
                                @Resource.Text("ROP")
                            End If
                        </div>
	                    <div class="col-xs-6 text-10 line-gap">
	                        @Resource.Text("VBEResultUpToDate").Replace("{{DATE}}", Model.UpToDate)
	                    </div>
	                    <div class="col-xs-12 text-9-2 line-gap">
	                        @Resource.Text("VBEResultForReferAvailableQuotaNote") 
	                    </div>
	                </div>*@
	            </div>
	            <div class="col-xs-1 col-sm-1">
	            </div>
	        </div>
	    </div>
	</div>
	
	
	<div class="container-fluid" style="background-color: white; Box-shadow: 0px 0px 40px -15px rgba(0,0,0,1); margin-bottom: 20px;">
	    @If Model.VBERequestData.InputType = "IC" Then
	    @<div class="container">
	        <div class="row" style="padding-bottom: 20px;">
	            <div class="col-xs-1 col-sm-1"></div>
	            <div class="col-xs-10 col-sm-10">
	                <!--Note*-->
	
	                <div class="row text-7-2 line-gap-20">
	                    @Resource.Text("VBEResultNote")
	                </div>
	                <div class="row line-gap">
	                    <div class="col-xs-6 col-sm-3 note-pic" style="padding-left: 0px;">
	                        <img src="~/Image/vbe/web/hkid-male-tutorial.png" class="img-responsive" title="HKID Card Sample" />
	
	                    </div>
	                    <div class="col-xs-6 col-sm-9 pic-arrow">
	                        <div class="left_hint" style="display: inline-flex;">
	                            <div class="col-xs-1 col-sm-1" style="display: flex;align-items: center;padding-left: 5px;">
	                                <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
	                            </div>
	                            <div class="col-xs-11 col-sm-11" style="padding-left:5px;">
	                                <div>
	                                    <span>
	                                        @Resource.Text("VBEResultSymbolStatus")
	                                    </span>
	                                </div>
	                            </div>
	                        </div>
	                        <div class="hidden-xs text-9 line-gap-20 " style="text-align:justify;">
	                            @Resource.Text("VBEResultSymbolStatusContent")
	                        </div>
	                    </div>
	                </div>
	
	                <div class="row visible-xs text-9 line-gap-20" style="text-align:justify;">
	                    @Resource.Text("VBEResultSymbolStatusContent")
	                </div>
	            </div>
	            <div class="col-xs-1 col-sm-1"></div>
	        </div>
	    </div>
	    End If
	</div>
</div>
<form action="/en/VBE/Result" method="post" id="hiddenForm" name="hiddenForm">
    <input type="hidden" name="txtHKIC" value="@Model.VBERequestData.HKICNo" />
    <input type="hidden" name="inputType" value="@Model.VBERequestData.InputType" />
    <input type="hidden" name="txtDob" value="@Model.VBERequestData.DateOfBirth" />
    <input type="hidden" name="txtDob_CE" value="@Model.VBERequestData.DateOfBirth_CE" />
    <input type="hidden" name="selType" value="@Model.VBERequestData.DateType" />
    <input type="hidden" name="txtAge" value="@Model.VBERequestData.Age" />
    <input type="hidden" name="txtDay" value="@Model.VBERequestData.Day" />
    <input type="hidden" name="txtYear" value="@Model.VBERequestData.Year" />
    <input type="hidden" name="selMonth" value="@Model.VBERequestData.Month" />
    <input type="hidden" id="method_flag" name="method" value="post" />
    <input type="hidden" id="txtLang" name="txtLang" value="" />
</form>
<script src="~/js/pageTimeout.js"></script>
<script type="text/javascript">
    function OnClickBackVRE() {
        if (currentLanguage==cultureLanguageEnglish) {
            window.location.href = '@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)/en/VBE/Search';
        }
        else {
            window.location.href = '@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)/tc/VBE/Search';
        }
    }
    timeOut = @ViewBag.PageTimeout * 60 * 1000; //set time

</script>