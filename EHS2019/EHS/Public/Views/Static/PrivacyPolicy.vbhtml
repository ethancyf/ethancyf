@Code
    ViewData("Title") = Resource.Text("PrivacyPolicy")
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="container policy_bigbox" id="divPrivacyPolicy">
    <div class="row notice_topTitleBox">
        <div class="col-xs-12 col-sm-12" >
            <h1 id="skiptarget" class="notice_topTitleText"><strong>@Resource.Text("PrivacyPolicy")</strong></h1>
        </div>
    </div>
    <div class="row" style="margin-top:20px;">
        <div class="col-xs-12 col-sm-12">
            <div class="container-fluid">
                @*<div class="row policy_titlebox">
                    <div class="col-xs-12 col-sm-12">
                        <span class="policy_titletext"><strong>@Resource.Text("PrivacyPolicy")</strong></span>
                    </div>
                </div>*@
                <div class="row policy_cntbox" style="border-radius:5px 5px 5px 5px !important;">
                    <div class="col-xs-12 col-sm-12">
                        @Html.Raw(Resource.Text("PrivacyPolicyCnt"))
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
