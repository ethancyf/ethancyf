@ModelType SystemMaintenanceViewModel
@Code
    ViewData("Title") = Resource.Text("SystemMaintenance")
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="container policy_bigbox" id="divSysMaintenance">
@code
    If Model.UrgentLyModelList.Count > 0 Then       
    @<div class="row notice_topTitleBox">
         <div class="col-xs-12 col-sm-12 maintenance_topTitleText" >
             <span><strong>@Resource.Text("UrgentSystemMaintenanceSchedule")</strong></span>
         </div>
    </div>
    @<div class="row sysmngap">
        <div class="col-xs-12 col-sm-12">
            <div class="container-fluid">
                <div class="row maintenance_titlebox scr_maintenance_titlebox">
                    <div class="col-xs-6 col-sm-6 scr_maintenance_dateTitleAlign">
                        <span class="maintenance_boxTitleText"><strong>@Resource.Text("SMScheduledMaintenanceDate1") </strong></span>
                        @*<span class="maintenance_boxTitleText"><strong>@Resource.Text("SMScheduledMaintenanceDate2")</strong></span>*@
                    </div>
                    <div class="col-xs-6 col-sm-6 scr_maintenance_timeTitleAlign">
                        <span class="maintenance_boxTitleText"><strong>@Resource.Text("SMServiceDownTime1") </strong></span>
                        @*<span class="maintenance_boxTitleText"><strong>@Resource.Text("SMServiceDownTime2")</strong></span>*@
                    </div>
                </div>
                <div class="row maintenance_cntbox scr_maintenance_cntbox">
                    <div class="col-xs-12 col-sm-12">
                        
                            @For Each m As SystemMaintenanceModel In Model.UrgentLyModelList
                            @<div class="row scr_maintenance_dateRow" style="text-align:justify">
                                <div class="col-xs-6 col-sm-6 scr_maintenance_dateTitleAlign hidden-xs">
                                    <span class="" style="font-size:16px">@m.ShowDate</span>
                                </div>
                                <div class="col-xs-6 col-sm-6 scr_maintenance_timeTitleAlign hidden-xs">
                                    <span class="" style="font-size:16px">@m.ShowTime</span>
                                </div>
                                <span class="visible-xs" style="padding-left:15px;font-size:16px;float:left">@m.ShowDate</span>
                                <span class="visible-xs" style="padding-right:15px;font-size:16px;float:right">@m.ShowTime</span>
                            </div>
                            @<hr class="contact_line" />
                            Next

                    </div>
                </div>
            </div>
        </div>
    </div>
End If
End code
    <div class="row notice_topTitleBox">
        <div class="col-xs-12 col-sm-12 maintenance_topTitleText">
            <span><strong>@Resource.Text("RegularSystemMaintenanceSchedule")</strong></span>
        </div>
    </div>
    <div class="row">
        <div class="col-xs-12 col-sm-12">
            <div class="container-fluid">
                <div class="row maintenance_titlebox scr_maintenance_titlebox">
                    <div class="col-xs-6 col-sm-6 scr_maintenance_dateTitleAlign">
                        <span class="maintenance_boxTitleText"><strong>@Resource.Text("SMScheduledMaintenanceDate1") </strong></span>
                        @*<span class="maintenance_boxTitleText"><strong>@Resource.Text("SMScheduledMaintenanceDate2")</strong></span>*@
                    </div>
                    <div class="col-xs-6 col-sm-6 scr_maintenance_timeTitleAlign">
                        <span class="maintenance_boxTitleText"><strong>@Resource.Text("SMServiceDownTime1") </strong></span>
                        @*<span class="maintenance_boxTitleText"><strong>@Resource.Text("SMServiceDownTime2")</strong></span>*@
                    </div>
                </div>
                <div class="row maintenance_cntbox scr_maintenance_cntbox">
                    <div class="col-xs-12 col-sm-12">
                        @code
                            For Each m As SystemMaintenanceModel In Model.MonthlyModelList
                                @<div class="row scr_maintenance_dateRow" style="text-align:justify">
                                    <div class="col-xs-6 col-sm-6 scr_maintenance_dateTitleAlign hidden-xs">
                                        <span class="" style="font-size:16px;">@m.ShowDate</span>
                                    </div>
                                    <div class="col-xs-6 col-sm-6 scr_maintenance_timeTitleAlign hidden-xs">
                                        <span class="" style="font-size:16px;">@m.ShowTime</span>
                                    </div>
                                    <span class="visible-xs" style="padding-left:15px;font-size:16px;float:left">@m.ShowDate</span>
                                    <span class="visible-xs" style="padding-right:15px;font-size:16px;float:right">@m.ShowTime</span>
                                </div>
                                @<hr class="contact_line" />
                            Next
                        End code
                     </div>
                </div>
            </div>
        </div>
    </div>
</div>
