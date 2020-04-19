@Code
    ViewData("Title") = Resource.Text("ErrorPageUnexpectedErrorTitle")
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="container" id="divErrorPage">
    <div class="row">
        <div class="col-xs-12 col-sm-12">
            <div class="errorPageDiv scr_errorPageDiv" style="position:relative">
                <img src="~/Image/Static/icon_SystemError.png" class="errorimg scr_errorimg" alt="" />
                <div class="errorText">
                    @Resource.Text("ErrorPageUnexpectedError")
                </div>
                @Html.ActionLink(Resource.Text("AriaVBESearchBack"), "Home", "Home", Nothing, New With {.class = "btn"})
            </div>
        </div>
    </div>
</div>
