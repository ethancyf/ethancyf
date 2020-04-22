@Code
    ViewData("Title") = Resource.Text("TextSizeTitle")
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="container policy_bigbox" id="textSizeBox">
    <div class="container-fluid">

        <div class="row policy_titlebox">
            <div class="col-xs-12 col-sm-12">
                <span class="policy_titletext"><strong>@Html.Raw(Resource.Text("TextSizeTitle"))</strong></span>
            </div>
        </div>
        <div class="row textsize_cntbox">
            <div class="col-xs-12 col-sm-12">
                @Html.Raw(Resource.Text("TextSizeCnt"))
            </div>
        </div>

    </div>
</div>