@ModelType System.Collections.Generic.List(Of AreaList)

<div id="ulSort" class="row" style="text-align:center; position:relative">
    <ul class="nav nav-pills" role="tablist" style="text-align:center;">
        <li id="sortNameTab" role="presentation">
            <a href="#sortNameTabBox" aria-controls="sortNameTabBox" role="tab" data-toggle="tab" id="aSortName">
                <span>@Resource.Text("ResultPageSort"):<span id="spanSortTitle">Provider's Name Asc</span></span>
                <span class="glyphicon glyphicon-chevron-down downArrow none"></span>
                <span class="glyphicon glyphicon-chevron-up upArrow"></span>
            </a>
        </li>
        <li id="sortItemTab" role="presentation" style="width:200px">
            <a href="#sortItemTabBox" aria-controls="sortItemTabBox" role="tab" data-toggle="tab" id="aSortItem">
                <span>
                    <span id="spanSortItemTitle">0/0</span> @Resource.Text("ResultPageItems")
                </span>
                <span class="glyphicon glyphicon-chevron-down downArrow none"></span>
                <span class="glyphicon glyphicon-chevron-up upArrow"></span>
            </a>
        </li>
    </ul>
    <div class="tab-content" style="background-color:white; z-index :30;width:100%">
        <div role="tabpanel" class="tab-pane container-fluid" id="sortNameTabBox">
            <div class="row">
                <div class="col-xs-12">
                    <ul style="text-align:left">
                        <li>
                            <span class="chk_container" style="border:0px">
                                <span>@Resource.Text("ResultPageSortingOrder")</span>
                            </span>
                        </li>
                        <li>
                            <div class="row typeBackground no-margin no-padding">
                                <div class="col-xs-6 col-sm-6">
                                    <div id="divAsc" href="#" class="activeType" onclick="setOrderValue(this, 'divDesc', '@Resource.Text("ResultPageAsc")')" value="Asc" aria_label="@Resource.Text("ResultPageAscending")" tabindex="0">
                                        <a href="javascript:void(0);">@Resource.Text("ResultPageAscending")</a>
                                    </div>
                                </div>
                                <div id="btnTypCE" class="col-xs-6 col-sm-6">
                                    <div id="divDesc" href="#" class="inactiveType" onclick="setOrderValue(this, 'divAsc', '@Resource.Text("ResultPageDesc")')" value="Desc" aria_label="@Resource.Text("ResultPageDescending")" tabindex="0">
                                        <a href="javascript:void(0);">@Resource.Text("ResultPageDescending")</a>
                                    </div>
                                </div>
                                <input type="hidden" id="hidOrder" value="Asc" showvalue="@Resource.Text("ResultPageAsc")" />
                            </div>
                        </li>
                    </ul>

                </div>
                <div class="col-xs-12" id="sortFieldHCVS">
                    <ul style="text-align:left">
                        <li>
                            <span class="chk_container" style="border:0px;padding-bottom:0px;">
                                <span>@Resource.Text("ResultPageDataFields")</span>
                            </span>
                        </li>
                        <li class="district">
                            <div class="chk_container radioLabel" style="color: rgb(1, 113, 186);">
                                <span>@Resource.Text("ResultPageDataFields1")</span>
                                @Html.RadioButton("Fields", "SPName", True, New With {.class = "chk", .id = "Fields1",
     .aria_label = Resource.Text("ResultPageSort") + " " + Resource.Text("ResultPageDataFields1"), .colname = "SPName"})
                            </div>
                        </li>
                        <li class="district">
                            <div class="chk_container radioLabel">
                                <span>@Resource.Text("ResultPageDataFields2")</span>
                                @Html.RadioButton("Fields", "PracticeName", New With {.class = "chk", .id = "Fields2",
                    .aria_label = Resource.Text("ResultPageSort") + " " + Resource.Text("ResultPageDataFields2"), .colname = "PracticeName"})
                            </div>
                        </li>
                        <li class="district">
                            <div class="chk_container radioLabel">
                                <span>@Resource.Text("ResultPageDataFields3")</span>
                                @Html.RadioButton("Fields", "DistrictName", New With {.class = "chk", .id = "Fields3",
                    .aria_label = Resource.Text("ResultPageSort") + " " + Resource.Text("ResultPageDataFields3"), .colname = "DistrictName"})
                            </div>
                        </li>
                        <li class="district">
                            <div class="chk_container radioLabel">
                                <span>@Resource.Text("ResultPageDataFields4")</span>
                                @Html.RadioButton("Fields", "JoinedScheme", New With {.class = "chk", .id = "Fields4",
                    .aria_label = Resource.Text("ResultPageSort") + " " + Resource.Text("ResultPageDataFields4"), .colname = "JoinedScheme"})
                            </div>
                        </li>
                        <li class="district">
                            <div class="chk_container radioLabel">
                                <span>@Resource.Text("ResultPageDataFields5")</span>
                                @Html.RadioButton("Fields", "Profession", New With {.class = "chk", .id = "Fields5",
                    .aria_label = Resource.Text("ResultPageSort") + " " + Resource.Text("ResultPageDataFields5"), .colname = "Profession"})
                            </div>
                        </li>
                    </ul>

                </div>
                <div class="col-xs-12" id="sortFieldSIV">
                    <ul style="text-align:left">
                        @For Each item In Model
                            If item.DistrictBoardCode = 2 Then
                                @<li>
                                    <span class="chk_container" style="border:0px;padding-bottom:0px;">
                                        <span>@Resource.Text("ResultPageServiceFee")</span>
                                    </span>
                                </li>
                                @For i = 0 To item.DistrictBoardList.Count - 1
                                    @<li class="district" sortcode="@item.DistrictBoardList(i).SubsidyCode">
                                        <div class="chk_container radioLabel">
                                            <span>@item.DistrictBoardList(i).Text</span>
                                            @Html.RadioButton("Fields", item.DistrictBoardList(i).Value, New With {.class = "chk", .id = String.Format("ServiceFee{0}", i.ToString()),
                                               .aria_label = item.DistrictBoardList(i).Text, .colname = item.DistrictBoardList(i).SubsidizeFeeColumnName})
                                        </div>
                                    </li>
                                Next


                            End If
                        Next
                    </ul>

                </div>
            </div>
            <div class="row">
                <div Class="col-xs-12">
                    <button type="submit" style="display:none" title="apply1">@Resource.Text("ResultPageApply")</button>
                    <button type="button" title="apply3" class="btn btn-default btn-lg btn-block" onclick="orderApply('aSortName')">@Resource.Text("ResultPageApply")</button>
                </div>
            </div>
        </div>
        <div role="tabpanel" Class="tab-pane container-fluid" id="sortItemTabBox">
            <div class="row">
                <div class="col-sm-3">
                    <ul style="text-align:left" class="pageSizeBoxMibile">
                        <li>
                            <span class="chk_container" style="border:0px;padding-bottom:0px;">
                                <span>@Resource.Text("ResultPageItemsPerPage")</span>
                            </span>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="row">
                <div class="col-xs-12">
                    <button type="submit" style="display:none" title="apply2">@Resource.Text("ResultPageApply")</button>
                    <button type="button" title="apply4" class="btn btn-default btn-lg btn-block" onclick="orderApply('aSortItem')">@Resource.Text("ResultPageApply")</button>
                </div>
            </div>

        </div>
    </div>

</div>

