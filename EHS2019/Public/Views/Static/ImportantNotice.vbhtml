@Code
    ViewData("Title") = Resource.Text("ImportantNotices")
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="container policy_bigbox" id="divNotice">
    <div class="row notice_topTitleBox">
        <div class="col-xs-12 col-sm-12">
            <span class="notice_topTitleText"><strong>@Resource.Text("ImportantNotices")</strong></span>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12">
            <div class="container-fluid">
                <div class="row notice_titlebox">
                    <div class="col-xs-12 col-sm-12">
                        <span class="notice_boxTitleText"><strong>@Resource.Text("ImportantDisclaimer")</strong></span>
                    </div>
                </div>
                <div class="row policy_cntbox">
                    <div class="col-xs-12 col-sm-12">
                        @Html.Raw(Resource.Text("ImportantDisclaimerCnt"))
                    </div>
                </div>

                <div class="row notice_titlebox">
                    <div class="col-xs-12 col-sm-12">
                        <span class="notice_boxTitleText">
                            <strong>
                                @Resource.Text("ImportantCopyrightNotice")
                            </strong>
                        </span>
                    </div>
                </div>
                <div class="row policy_cntbox">
                    <div class="col-xs-12 col-sm-12">
                        @Html.Raw(Resource.Text("ImportantCopyrightNoticeCnt"))
                    </div>
                </div>
            </div>
        </div>
    </div>

</div>
