@ModelType SPSViewModel
@Code
    ViewData("Title") = Resource.Text("LandingPageESPSTitle")
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

@Using Html.BeginForm("GetResult", "SPS", FormMethod.Post, New With {.autocomplete = "off"})
    @<text>
        <div class="container">
            <input type="hidden" id="method_flag" name="method" value="post" />
            <input type="hidden" id="querylanguage" name="querylanguage" value="@Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower" />
            <input type="hidden" id="pageSize" name="pageSize" value="10" />
            <input type="hidden" id="pageActualSize" name="pageActualSize" value="10" />
            <input type="hidden" id="pageIndex" name="pageIndex" value="1" />
            <input type="hidden" id="sortField" name="sortField" value="SPName" />
            <input type="hidden" id="sortColName" name="sortColName" value="SPName" />
            <input type="hidden" id="sortType" name="sortType" value="asc" />
            <input type="hidden" id="requestType" name="requestType" value="criteria" />
            <input type="hidden" id="isReset" name="isReset" value="False" />
            <input type="hidden" id="hasResult" name="hasResult" value="false" />
            <input type="hidden" id="selectedTab" name="selectedTab" value="1" />

            <!--search criteria-->
            <div class="row" id="searchBox" style="padding-top: 20px;">
                <div class="container-fluid">
                    <div class="row hidden-lg">
                        <div class="col-sm-12" style="text-align:right;margin-top:20px;">
                            <button class="btn btn-default icon_btn" title="FAQs" type="button" onclick="OnClickFAQ()">
                                <div class="icon_container_blue">
                                    ?
                                </div>
                                <span class="icon_content">@Resource.Text("FAQ")</span>
                            </button>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-sm-12 SPSearch_title" style="color:#195287;">
                            <h1 id="skiptarget" tabindex="0">@Resource.Text("SPSH1")</h1>
                        </div>
                        <button class="btn btn-default icon_btn visible-lg" type="button" onclick="OnClickFAQ()" style="position:absolute;top:20px;right:25px;">
                            <div class="icon_container_blue">
                                ?
                            </div>
                            <span class="icon_content">@Resource.Text("FAQ")</span>
                        </button>
                    </div>
                    <div class="row">
                        <div class="col-sm-2 col-xs-offset-0">
                        </div>
                        <div class="col-sm-8 col-xs-12 SPSearch_title">
                            <h2 tabindex="0">@Html.Raw(Resource.Text("SPSH2"))</h2>
                        </div>
                        <div class="col-sm-2 col-xs-offset-0">
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="alert alert-danger alert-dismissible" role="alert" style="display:none;" aria-atomic="true">
                                <span id="txtErrMsg" class="alertStyle">Better check yourself, you're not looking too good.</span>
                            </div>
                        </div>
                    </div>
                    <div class="row align-elements" style="color:#195287;">
                        <div class="col-xs-12">
                            <h3 style="margin-top:0;">@Resource.Text("SPSKeywordSearch")</h3>
                        </div>
                    </div>
                    <div class="row align-elements">
                        <div class="col-xs-12">
                            <div class="SPSearch_inputGroup">
                                <div class="SPSearch_icon providerIcon"></div>
                                @Html.TextBoxFor(Function(model) model.InputServiceProviderName, New With {.placeholder = Resource.Text("SPSInputServiceProviderName"), .class = "form-control inputCriteria", .maxLength = "40", .aria_label = Resource.Text("SPSInputServiceProviderName")})
                                <input ismodelfield="true" type="hidden" id="hiddenInputServiceProviderName" name="hiddenInputServiceProviderName" value="" />
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="SPSearch_inputGroup">
                                <div class="SPSearch_icon nameIcon"></div>
                                @Html.TextBoxFor(Function(model) model.InputPracticeName, New With {.placeholder = Resource.Text("SPSInputPracticeName"), .class = "form-control inputCriteria", .maxLength = "100", .aria_label = Resource.Text("SPSInputPracticeName")})
                                <input ismodelfield="true" type="hidden" id="hiddenInputPracticeName" name="hiddenInputPracticeName" value="" />
                            </div>
                        </div>
                        <div class="col-xs-12">
                            <div class="SPSearch_inputGroup">
                                <div class="SPSearch_icon addressIcon"></div>
                                @Html.TextBoxFor(Function(model) model.InputPracticeAddress, New With {.placeholder = Resource.Text("SPSInputPracticeAddress"), .class = "form-control inputCriteria", .maxLength = "100", .aria_label = Resource.Text("SPSInputPracticeAddress")})
                                <input ismodelfield="true" type="hidden" id="hiddenInputPracticeAddress" name="hiddenInputPracticeAddress" value="" />
                            </div>
                        </div>
                    </div>
                    <div class="row align-elements" style="color:#195287;">
                        <div class="col-xs-12" style="margin-top:10px;">
                            <h3 style="margin-top:0;">@Resource.Text("SPSOptionSelect")</h3>
                        </div>
                    </div>
                    <div class="row align-elements">
                        <div class="col-xs-12">
                            <ul id="optSelect" class="nav nav-pills">
                                <li id="professionTab" role="presentation" class="active">
                                    <a href="#Profession" data-toggle="tab">
                                        <div>
                                            <em class="tabIcon professionIcon active" aria-hidden="true"></em> @Resource.Text("SPSTabProfession")
                                            <span class="badge professionBadge active"></span>
                                        </div>
                                        <div class="upDownArrow">
                                            <span class="glyphicon glyphicon-chevron-down downArrow"></span>
                                            <span class="glyphicon glyphicon-chevron-up upArrow none"></span>
                                        </div>
                                    </a>
                                </li>
                                <li id="schemeTab" role="presentation">
                                    <a href="#Scheme" data-toggle="tab">
                                        <div>
                                            <em class="tabIcon schemeIcon" aria-hidden="true"></em> @Resource.Text("SPSTabScheme")
                                            <span class="badge schemeBadge"></span>
                                        </div>
                                        <div class="upDownArrow">
                                            <span class="glyphicon glyphicon-chevron-down downArrow none"></span>
                                            <span class="glyphicon glyphicon-chevron-up upArrow"></span>
                                        </div>
                                    </a>
                                </li>
                                <li id="districtTab" role="presentation" style="margin-left: 0px;">
                                    <a href="#District" data-toggle="tab">
                                        <div>
                                            <em class="tabIcon districtIcon" aria-hidden="true"></em> @Resource.Text("SPSTabDistrict")
                                            <span class="badge districtBadge"></span>
                                        </div>
                                        <div class="upDownArrow">
                                            <span class="glyphicon glyphicon-chevron-down downArrow none"></span>
                                            <span class="glyphicon glyphicon-chevron-up upArrow"></span>
                                        </div>
                                    </a>
                                </li>
                            </ul>
                            <div class="tab-content" style="background-color:white;border-radius:4px">
                                <div class="tab-pane fade in active container-fluid" id="Profession">
                                    <div class="row">
                                        @code
                                            Dim cnt = 0
                                            While cnt < Model.ProfessionList.Count
                                                @<div class="col-sm-6">
                                                    <ul role = "radiogroup">
                                                        @For i As Integer = 1 To 5
                                                            If (cnt >= Model.ProfessionList.Count) Then
                                                                Exit For
                                                            End If
                                                            Dim item = Model.ProfessionList(cnt)
                                                            @<li>
                                                                <div class="chk_container radioLabel">
                                                                    <span>@item.Text</span>
                                                                    @Html.RadioButtonFor(Function(model) model.selectedProfession, item.Value, New With {.class = "chk chkProfession", .id = item.Value, .aria_label = item.Text, .eligible_scheme = item.EligibleScheme})
                                                                </div>
                                                            </li>
                                                            cnt += 1
                                                        Next
                                                    </ul>
                                                </div>
                                            End While
                                        End Code
                                        <input ismodelfield="true" id="hiddenSelectedProfession" name="hiddenSelectedProfession" type="hidden" value="" />
                                    </div>
                                    <div class="row" style="margin-top:10px;">
                                        <div class="col-xs-12">
                                            <button id="btnClearProfession" aria-label="@Resource.Text("SPSTabProfession") @Resource.Text("SPSBtnClear")" class="btn-primary btn-clear" type="button">
                                                @Resource.Text("SPSBtnClear")
                                            </button>
                                        </div>
                                    </div>
                                </div>                                
                                                             
                                <div class="tab-pane fade container-fluid scheme" id="Scheme">
                                    <div class="row">
                                        @For Each item As SchemeList In Model.SchemeList
                                                If item.SchemeCode.Equals("VSS") Then
                                                @<div class="col-sm-6 VSS_container searcVSSbox" style="border-left-color: rgba(221, 221, 221, 1); border-left-width: 1px; border-left-style: solid;">
                                                    <ul>
                                                        <li class="schemeImgContainer">
                                                            <a href="@item.SchemeUrl" target="_blank" ref="noopener noreferrer" style="min-width:50px">
                                                                <img src="~/Image/SPS/icon_VSS.png" alt="@item.SchemeDesc" />&nbsp;
                                                            </a>
                                                            <div class="VSSCheckbox" style="cursor:default;color: #337ab7">
                                                                <span>@item.SchemeDesc</span>
                                                            </div>
                                                        </li>

                                                      
                                                        <li style ="font-weight:bold;">
                                                            <div class="chk_container" style="margin-left:0">
                                                                <span>@Model.IsFreeSubsidizeOption.Data_Value</span>
                                                                @Html.CheckBox("Item_" + Model.IsFreeSubsidizeOption.Item_No, New With {.class = "chk chkFreeSub", .aria_label = Model.IsFreeSubsidizeOption.Item_No, .id = Model.IsFreeSubsidizeOption.Item_No, .name = "FreeSubsidizeItem"})
                                                            </div>
                                                        </li>
                                                        @Html.HiddenFor(Function(model) model.IsFreeSub, New With {.id = "IsFreeSub"})

                                                        <li style="font-weight:bold; padding-top:3%;"><span>@Resource.Text("VSS_Venue_Gp_Name")</span></li>
                                                        <div class="multiple_chk_row_container radioCheckBox_row_container">
                                                            @For Each venueItem As VenueItemList In Model.VenueList.VenueItemList

                                                                @<div class="chk_container multiple_chk_container radioCheckBox_container">
                                                                    <span>@venueItem.Data_Value</span>
                                                                     &nbsp;&nbsp;
                                                                    @Html.RadioButtonFor(Function(model) model.selectedIsNonClinicByForm, venueItem.Item_No, New With {.class = "chk chkClinic", .aria_label = venueItem.Item_No, .id = venueItem.Item_No, .name = "NonClinicItem"})
                                                                </div>



                                                            Next
                                                        </div>
                                                        <input ismodelfield="true" id="hiddenSelectedIsNonClinic" name="hiddenSelectedIsNonClinic" type="hidden" value="" />


                                                        <li style="font-weight:bold; padding-top:3%;" ><span>@Resource.Text("VSS_Vaccine_Gp_Name")</span></li>
                                                            @For Each subsidyItem As SelectListItem In item.SubsidyList
                                                                @<li  align:left>
                                                                    <div class="chk_container" >
                                                                        <span>@subsidyItem.Text</span>
                                                                       
                                                                        @Html.CheckBox("SchemeItem_" + subsidyItem.Value, New With {.class = "chk chkScheme", .aria_label = subsidyItem.Text, .id = subsidyItem.Value, .name = "SchemeItem"})
                                                                    </div>
                                                                </li>
                                                            Next
                                                        </ul>
                                                </div>
                                            End If
                                            If item.SchemeCode.Equals("HCVS") Then
                                                @<div class="col-sm-6 searchHCVSbox">
                                                    <ul>
                                                        <li class="schemeImgContainer">
                                                            <div class="HCVSCheckbox" style="width:100%">
                                                                <a href="@item.SchemeUrl" target="_blank" ref="noopener noreferrer" style="min-width:50px">
                                                                    <img src="~/Image/SPS/icon_HCVS.png" alt="@item.SchemeDesc" />&nbsp;
                                                                </a>

                                                                <span class="HCVSSchemeDesc">@item.SchemeDesc</span>
                                                            </div>
                                                            @For Each subsidyItem As SelectListItem In item.SubsidyList
                                                                @<span style="min-width:10px;text-align:right">
                                                                    @Html.CheckBox("SchemeItem_" + subsidyItem.Value, New With {.class = "chk chkScheme", .aria_label = subsidyItem.Text, .id = subsidyItem.Value, .name = "SchemeItem"})
                                                                </span>
                                                            Next
                                                        </li>
                                                        @*@For Each subsidyItem As SelectListItem In item.SubsidyList
                                                                @<li>
                                                                    <div class="chk_container">
                                                                        <span>@subsidyItem.Text</span>
                                                                        @Html.CheckBox("SchemeItem_" + subsidyItem.Value, New With {.class = "chk chkScheme", .aria_label = subsidyItem.Text, .id = subsidyItem.Value, .name = "SchemeItem"})
                                                                    </div>
                                                                </li>
                                                            Next*@
                                                    </ul>
                                                </div>
                                            End If
                                        Next
                                        @Html.HiddenFor(Function(model) model.selectedScheme, New With {.id = "selectedScheme"})
                                       
                                    </div>
                                    <div class="row" style="margin-top:10px;">
                                        <div class="col-xs-12">
                                            <button id="btnClearScheme" aria-label="@Resource.Text("SPSTabScheme") @Resource.Text("SPSBtnClear")" class="btn-primary btn-clear" type="button">
                                                @Resource.Text("SPSBtnClear")
                                            </button>
                                        </div>
                                    </div>
                                </div>
                                <div class="tab-pane fade container-fluid" id="District">
                                    <div class="row">
                                        @For Each Item As AreaList In Model.AreaList
                                            Dim show As Boolean = False
                                            Dim i As Integer = 0
                                            While i < Item.DistrictBoardList.Count
                                                @<div class="col-sm-3">
                                                    <ul>
                                                        @If Not show Then
                                                            @<li class="area">
                                                                <div class="chk_container">
                                                                    <span>@Item.DistrictBoardDesc</span>
                                                                    @Html.CheckBox("DistrictItem_" + Item.DistrictBoardCode, New With {.class = "chk", .aria_label = Item.DistrictBoardDesc, .id = Item.DistrictBoardCode})
                                                                </div>
                                                            </li>
                                                            show = True
                                                        Else
                                                            @<li class="hidden-xs">
                                                                <div style="padding-top: 10px;padding-bottom: 10px;border-bottom: groove 1px;">
                                                                    <div style="height:22px;"></div>
                                                                </div>
                                                            </li>
                                                        End If
                                                        @For j As Integer = 1 To 5
                                                            If i >= Item.DistrictBoardList.Count Then
                                                                Exit For
                                                            End If
                                                            Dim district As DistrictBoardList = Item.DistrictBoardList(i)
                                                            @<li class="district">
                                                                <div class="chk_container">
                                                                    <span>@district.Text</span>
                                                                    @Html.CheckBox("DistrictItem_" + district.Value, New With {.class = "chk chkDistrict", .aria_label = district.Text, .id = district.Value, .area_code = Item.DistrictBoardCode})
                                                                </div>
                                                            </li>
                                                            i += 1
                                                        Next
                                                    </ul>
                                                </div>
                                            End While
                                        Next
                                        @Html.HiddenFor(Function(model) model.selectedDistrict, New With {.id = "selectedDistrict"})
                                    </div>
                                    <div class="row" style="margin-top:10px;">
                                        <div class="col-xs-12">
                                            <button id="btnClearDistrict" aria-label="@Resource.Text("SPSTabDistrict") @Resource.Text("SPSBtnClear")" class="btn-primary btn-clear" type="button">
                                                @Resource.Text("SPSBtnClear")
                                            </button>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-6 SPSearch_footer" style="text-align:right;">
                            <button class="btn btn-default btnReset" type="button" onclick="ResetAll()">
                                @Resource.Text("SPSBtnReset")
                            </button>
                        </div>
                        <div class="col-xs-6 SPSearch_footer">
                            <button class="btn btn-default btnSearch" type="submit" onclick="return SearchResult('@Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower',true)">
                                @Resource.Text("SPSBtnSearch")
                            </button>
                        </div>
                    </div>
                </div>
            </div>
       
            <!--point to note-->
            <div id="PointToNote" class="row align-elements" style="margin-bottom:20px;display:none;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-xs-12" style="color:#195287;">
                            <h3>@Resource.Text("SPSPointToNote")</h3>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12" style="color:black;text-align: justify">
                            <h4>@Resource.Text("SPSPointToNoteContent1")</h4>
                            <h4>@Resource.Text("SPSPointToNoteContent2")</h4>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12 col-sm-6">
                            <div class="noteTab scr_search_lbox">
                                <div style="display: flex;justify-content: space-between;padding:5px">
                                    <a href="@Resource.Text("hcvURL")" target="_blank" ref="noopener noreferrer">
                                        <img src="~/Image/SPS/icon_HCVS.png" alt="@Resource.Text("HCVSShortName")" />
                                        <span>@Resource.Text("HCVSFullName")</span>
                                    </a>
                                </div>
                                <div class="noteTabText">
                                @code
                                    For Each item As PointToNoteList In Model.PointToNoteList
                                        If item.SchemeCode = "HCVS" Then
                                            @<p>
                                                @(New HtmlString(item.NoteDesc))
                                            </p>
                                        End If
                                    Next
                                End Code
                                </div>
                            </div>
                        </div>
                        <div class="col-xs-12 col-sm-6">
                            <div class="noteTab VSSNoteTab scr_search_rbox">
                                <div style="display: flex;justify-content: space-between;padding:5px">
                                    <div style="display:inline-flex;">
                                        <a href=@Resource.Text("VSSURL") target="_blank" ref="noopener noreferrer">
                                            <img src="~/Image/SPS/icon_VSS.png" alt="@Resource.Text("VSSShortName")" />
                                            <span>@Resource.Text("VSSFullName")</span>
                                        </a>
                                    </div>
                                    <div style="display:inline-flex;">
                                        <button class="icon_container btnPrev disabled" type="button" aria-label="@Resource.Text("SPSResultAriaPrevious")">
                                            <span class="glyphicon glyphicon-chevron-left" aria-hidden="true"></span>
                                        </button>
                                        <button class="icon_container btnNext" type="button" aria-label="@Resource.Text("SPSResultAriaNext")">
                                            <span class="glyphicon glyphicon-chevron-right" aria-hidden="true"></span>
                                        </button>
                                    </div>
                                </div>
                                <div class="scrollContent noteTabText">
                                    @code
                                        Dim VSSList As List(Of PointToNoteList) = Model.PointToNoteList.Where(Function(x) x.SchemeCode = "VSS").ToList()
                                        For Each item As PointToNoteList In VSSList
                                            @If item.Equals(VSSList.First()) Then
                                                @<p class="is-active" id="@(item.SeqNo)vss">
                                                    @(New HtmlString(item.NoteDesc))
                                                </p>
                                            Else
                                                @<p id="@(item.SeqNo)vss">
                                                    @(New HtmlString(item.NoteDesc))
                                                </p>
                                            End If
                                        Next
                                    End Code
                                </div>
                                <div id="liIconButton" style="text-align: right;position: absolute;bottom: 0;right: 20px;">
                                    <ol class="sec_shortcut">
                                        <li class="sec_shortcut_item">
                                            <a tarid="3vss" class="sec_shortcut_link is-active" aria-hidden="true"><span class="access"></span></a>
                                        </li>
                                        <li class="sec_shortcut_item">
                                            <a tarid="4vss" class="sec_shortcut_link" aria-hidden="true"><span class="access"></span></a>
                                        </li>
                                        <li class="sec_shortcut_item">
                                            <a tarid="5vss" class="sec_shortcut_link" aria-hidden="true"><span class="access"></span></a>
                                        </li>
                                    </ol>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            
            
            <!--desktop\tablet result-->
            <div class="container-fluid" style="display:none;" id="desktopResultContainer">
                <div class="row hidden-xs" style="margin-bottom:20px;">
                    <!--Title-->
                    <div class="row align-elements">
                        <div class="col-sm-12 text-1">
                            @Resource.Text("SPSSearchResult") (<span id="RecordTotal">120</span> @Resource.Text("ResultPageItems"))
                        </div>
                    </div>
                    <div class="row align-elements" style="margin-top:10px;">
                        <!--Page Size-->
                        <div class="col-sm-5 col-md-4 col-lg-3 text-5" style="padding-top: 25px;color:#225482;">
                            @Resource.Text("SPSResultPerPage")
                            <div class="btn-group">
                                <button type="button" class="btn btn-primary dropdown-toggle pagesize-ddl" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" style="border-radius: 10px;width:80px;font-size:15px">
                                    <span class="pageSize">10</span> <span class="caret"></span>
                                </button>
                                <ul class="dropdown-menu pageSizeBox">
                                    <li><a href="javascript:void(0);">10</a></li>
                                    <li><a href="javascript:void(0);">20</a></li>
                                    <li><a href="javascript:void(0);">30</a></li>
                                    <li><a href="javascript:void(0);">40</a></li>
                                    <li><a href="javascript:void(0);">50</a></li>
                                </ul>
                            </div>
                        </div>
                        <div class="col-sm-7 col-md-8 col-lg-7 legend" style="padding-left: 0px !important;padding-right: 0px !important" aria-hidden="true">
                            @*<div class="row">*@
                            <div id="legendPart1" class="legend-part1">
                                <div class="clegend-title">@Resource.Text("SPResultLegend")</div>
                                <div class="legend-section legend-section-768">
                                    <div class="legend-section-1-1">
                                        <span><img src="~/Image/SPS/Icon-mclinic-2x.png" alt="" /></span>
                                        <span class="legend-section-text">@Resource.Text("SPSResultMobileClinic")</span>
                                    </div>
                                    <div class="legend-section-1-2">
                                        <a href="@Resource.Text("VSSURL")" target="_blank" ref="noopener noreferrer">
                                            <span><img src="~/Image/SPS/icon_VSS.png" alt="" /></span>
                                            <span class="legend-section-text">@Resource.Text("VSSShortName")</span>
                                        </a>
                                    </div>
                                </div>
                                <div class="legend-section legend-section-768">
                                    <div class="legend-section-1-1">
                                        <span>
                                            @code
                                                If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.English Then
                                                    @<img src="~/Image/SPS/Icon-nonclinic-eng-2x.png" alt="" />
                                                End If
                                            If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.TradChinese Then
                                                    @<img src="~/Image/SPS/Icon-nonclinic-chi-2x.png" alt="" />
                                                End If
                                            end code
                                        </span>
                                        <span class="legend-section-text">@Resource.Text("NonClinic")</span>
                                    </div>
                                    <div class="legend-section-1-2">
                                        <a href="@Resource.Text("hcvURL")" target="_blank" ref="noopener noreferrer">
                                            <span><img src="~/Image/SPS/icon_HCVS.png" alt="" /></span>
                                            <span class="legend-section-text">@Resource.Text("HCVSShortName")</span>
                                        </a>
                                    </div>
                                </div>
                                <div class="legend-section legend-remark">
                                    <span><img src="~/Image/SPS/Icon-remark-top-2x.png" alt="" /></span>
                                    <span class="legend-section-text">@Resource.Text("Remarks")</span>
                                </div>
                            </div>
                            <div id="legendPart2" class="legend-part2">
                                <div class="legend-section">
                                    <a href="@Resource.Text("VSSURL")" target="_blank" ref="noopener noreferrer" aria-label="@Resource.Text("VSSShortName")">
                                        <span><img src="~/Image/SPS/icon_VSS.png" alt="" /></span>
                                        <span class="legend-section-text">@Resource.Text("VSSShortName")</span>
                                    </a>
                                </div>
                                <div class="legend-section">
                                    <a href="@Resource.Text("hcvURL")" target="_blank" ref="noopener noreferrer" aria-label="@Resource.Text("HCVSShortName")">
                                        <span><img src="~/Image/SPS/icon_HCVS.png" alt="" /></span>
                                        <span class="legend-section-text">@Resource.Text("HCVSShortName")</span>
                                    </a>
                                    @*</div>*@
                                </div>
                            </div>
                        </div>

                    </div>
                    <!--Table-->
                    <div class="row">
                        <div>
                            <div id="buttonDiv" class="container hidden-xs" style="position:absolute;width:100%; top:40px;padding-left: 0px;padding-right: 0px;">
                                <nav aria-label="...">
                                    <ul class="pager" style="cursor:pointer">
                                        <li class="previous">
                                            <span aria-hidden="true" class=" glyphicon glyphicon-menu-left " style="float:left;color:#0171BA;background-color: transparent;border: 0px;padding:0px;font-size:16px;padding:100px 0px;position:relative;left:7px"></span>
                                            <span aria-hidden="true" class=" glyphicon glyphicon-menu-left " style="float:left;background-color: transparent;border: 0px;padding:0px;font-size:16px;padding:100px 0px;position:relative;left:7px"></span>
                                        </li>
                                        <li class="next">
                                            <span aria-hidden="true" class=" glyphicon glyphicon-menu-right " style="float:right;color:#0171BA;background-color: transparent;border: 0px;padding:0px;font-size:16px;padding:100px 0px;position:relative;left:7px"></span>
                                            <span aria-hidden="true" class=" glyphicon glyphicon-menu-right " style="float:right;background-color: transparent;border: 0px;padding:0px;font-size:16px;padding:100px 0px;position:relative;left:7px"></span>
                                        </li>
                                    </ul>
                                </nav>
                            </div>
                            <div style="overflow:hidden;margin-left:42px;" id="divResult">
                                <table class="table table-hover SPTable" style="margin-top:10px;border-collapse:separate; border-spacing:0px 3px;width:1100px">
                                 <caption></caption> 
                                    <tr class="tr-header">
                                        <td class="left-cell" colspan="5"> </td>
                                        <th class="right-cell" colspan="8">Service Fee</th>
                                    </tr>
                                    <tr class="tr-header">
                                        <td class="left-cell" colspan="5"></td>
                                        <td>Pregnant Women</td>
                                        <td>Children</td>
                                        <td>Persons aged 50 - 64 years</td>
                                        <td colspan="3">Elders</td>
                                        <td>Persons with Intellectual Disability</td>
                                        <td class="right-cell">Persons receiving Disability Allowance</td>
                                    </tr>
                                    <tr class="tr-header">
                                        <td class="left-cell" style="width:120px;">
                                            Service Provider
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                        <td style="width:360px;">
                                            Practice Name, Address, Telephone Number
                                            <br />
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                        <td style="width:80px;">
                                            District
                                            <br />
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                        <td style="width:80px;">
                                            Enrolled Scheme
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                        <td style="width:80px;">
                                            Professional
                                            <br />
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                        <td style="width:60px;">
                                            SIV
                                            <br />
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                        <td style="width:60px;">
                                            SIV
                                            <br />
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                        <td style="width:60px;">
                                            SIV
                                            <br />
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                        <td style="width:60px;">
                                            SIV
                                            <br />
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                        <td style="width:60px;">
                                            23vPPV
                                            <br />
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                        <td style="width:60px;">
                                            PCV13
                                            <br />
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                        <td style="width:60px;">
                                            SIV
                                            <br />
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                        <td class="right-cell" style="width:60px;">
                                            SIV
                                            <br />
                                            <br />
                                            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="ServiceProvider"></span>
                                            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="ServiceProvider"></span>
                                        </td>
                                    </tr>
                                    <tr class="tr-content">
                                        <td colspan="13">
                                            <h5>No records found</h5>
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </div>
                    </div>
                    <div class="row" id="divPagination">
                        <div class="col-sm-12" style="display: flex; justify-content: center;">
                            <nav aria-label="Page navigation">
                                <ul class="pagination">
                                    <li>
                                        <a href="javascript:void(0);" aria-label="Previous">
                                            <span aria-hidden="true">&laquo;</span>
                                        </a>
                                    </li>
                                    <li class="active"><a href="javascript:void(0);" class="pageNum">1</a></li>
                                    <li><a href="javascript:void(0);" class="pageNum">2</a></li>
                                    <li><a href="javascript:void(0);" class="pageNum">3</a></li>
                                    <li><a href="javascript:void(0);" class="pageNum">4</a></li>
                                    <li><a href="javascript:void(0);" class="pageNum">5</a></li>
                                    <li>
                                        <a href="javascript:void(0);" aria-label="Next">
                                            <span aria-hidden="true">&raquo;</span>
                                        </a>
                                    </li>
                                </ul>
                            </nav>
                        </div>
                    </div>
                </div>
            </div>
            <!--scroll to result-->
            <div class="scrollToResult_container hidden-lg hidden-md hidden-sm hidden">
                <button id="scrollToResult" class="btn btn-default icon_btn" type="button">
                    <div class="icon_container">
                        <span class="glyphicon glyphicon-chevron-down" aria-hidden="true"></span>
                    </div>
                    <span class="icon_content">@Resource.Text("SPSResultSeeTheResult")</span>
                </button>
            </div>
            <!--mobile result-->
            <div class="row visible-xs mobile_result_container" style="margin-bottom:20px;">
                <div class="container-fluid" id="orderQueryBox" style="display:none">
                    @Html.Action("OrderQuery", "SPS")
                </div>
                
                
                
            <!--Legend-->
                <div id="mobileLegend" class="legend" style="padding-left: 10px;padding-right: 10px;margin-left: 15px;margin-right: 15px;margin-top: 15px;float:none;display:none;" aria-hidden="true">
                    @*<div class="row">*@
                    <div id="legendPart3" class="legend-part1">
                        <div class="clegend-title">@Resource.Text("SPResultLegend")</div>
                        <div class="legend-section legend-section-768">
                            <div class="legend-section-1-1">
                                <span><img src="~/Image/SPS/Icon-mclinic-2x.png" alt="" /></span>
                                <span class="legend-section-text">@Resource.Text("SPSResultMobileClinic")</span>
                            </div>
                            <div class="legend-section-1-2">
                                <a href="@Resource.Text("VSSURL")" target="_blank" ref="noopener noreferrer">
                                    <span><img src="~/Image/SPS/icon_VSS.png" alt="" /></span>
                                    <span class="legend-section-text">@Resource.Text("VSSShortName")</span>
                                </a>
                            </div>
                        </div>
                        <div class="legend-section legend-section-768">
                            <div class="legend-section-1-1">
                                <span>
                                    @code
                                        If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.English Then
                                        @<img src="~/Image/SPS/Icon-nonclinic-eng-2x.png" alt="" />
                                    End If
                                    If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.TradChinese Then
                                        @<img src="~/Image/SPS/Icon-nonclinic-chi-2x.png" alt="" />
                                    End If
                                    end code
                                </span>
                                <span class="legend-section-text">@Resource.Text("NonClinic")</span>
                            </div>
                            <div class="legend-section-1-2">
                                <a href="@Resource.Text("hcvURL")" target="_blank" ref="noopener noreferrer">
                                    <span><img src="~/Image/SPS/icon_HCVS.png" alt="" /></span>
                                    <span class="legend-section-text">@Resource.Text("HCVSShortName")</span>
                                </a>
                            </div>
                        </div>
                        <div class="legend-section legend-remark">
                            <span><img src="~/Image/SPS/Icon-remark-top-2x.png" alt="" /></span>
                            <span class="legend-section-text">@Resource.Text("Remarks")</span>
                        </div>
                    </div>
                    <div id="legendPart4" class="legend-part2">
                        <div class="legend-section">
                            <a href="@Resource.Text("VSSURL")" target="_blank" ref="noopener noreferrer">
                                <span><img src="~/Image/SPS/icon_VSS.png" alt="" /></span>
                                <span class="legend-section-text">@Resource.Text("VSSShortName")</span>
                            </a>
                        </div>
                        <div class="legend-section">
                            <a href="@Resource.Text("hcvURL")" target="_blank" ref="noopener noreferrer">
                                <span><img src="~/Image/SPS/icon_HCVS.png" alt="" /></span>
                                <span class="legend-section-text">@Resource.Text("HCVSShortName")</span>
                            </a>
                            @*</div>*@
                        </div>
                    </div>
                </div>
                
                                              
                
                <div class="container result_tab">
                </div>
                <div class="container" id="btnLoadMore" style="display:none;margin-top:10px;">
                    <button class="btn btn-default btnSearch" style="font-size:17px;width:100%;" type="button" onclick="loadMore()">
                        @Resource.Text("SPSResultLoadMore")
                    </button>
                </div>
            </div>
            <!-- Modal -->
            <div class="modal fade bs-example-modal-sm" id="myModal" tabindex="0" role="dialog" aria-label="myModalLabel">
                <div class="modal-dialog" role="document">
                </div>
            </div>
        </div>
    </text>
End Using
<script type="text/javascript">
    var pageSize = 10;
    var pageIndex = 1;
    var pageTotal = 0;
    var recordTotal = 0;
    var sortField = 'SPName';
    var sortColName = 'SPName';
    var sortType = 'asc';
    var requestType = 'criteria';
    var curMaxPage = 0;
    var pageActualSize = 10;
    var hasVSS = true;

    var InputServiceProviderName = '@Model.InputServiceProviderName';
    var InputPracticeName = '@Model.InputPracticeName';
    var InputPracticeAddress = '@Model.InputPracticeAddress';
    var selectedProfession = '@Model.selectedProfession';
    var selectedScheme = '@Model.selectedScheme';
    var selectedDistrict = '@Model.selectedDistrict';

    var selectedIsNonClinic = '@Model.IsNonClinic';
    var selectedIsFreeSub = '@Model.IsFreeSub';

    var InputServiceProviderNameByForm = '@Model.InputServiceProviderNameByForm';
    var InputPracticeNameByForm = '@Model.InputPracticeNameByForm';
    var InputPracticeAddressByForm = '@Model.InputPracticeAddressByForm';
    var selectedProfessionByForm = '@Model.selectedProfessionByForm';
    var selectedSchemeByForm = '@Model.selectedSchemeByForm';
    var selectedDistrictByForm = '@Model.selectedDistrictByForm';

    var selectedIsFreeSubByForm = '@Model.selectedIsFreeSubByForm';
    var selectedIsNonClinicByForm = '@Model.selectedIsNonClinicByForm';

    var actionReason = '@Model.ActionReason';
    var queryLanguageFromServer = '@Model.queryLang';
    var pageLanguage = '@Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower';
    var isReset = '@Model.IsReset';
    var hasResult = '@Model.hasResult';
    var selectedTab = '@Model.SelectedTab';
    var errMsgList = [];
 


    $(document).ready(function () {
        initMoveTable();
        initPagesizemenu();
        initPageNumberClick();
        initClearButton();
        getErrorMsg(rootLang);
        if (actionReason == 'ChangeLanguage') {
            initPostValues();
            $('#querylanguage').val(queryLanguageFromServer);
            if (hasResult == 'True') {
                GetResult(false);
            }
            actionReason = '';
        }
        setSpanSortTitle();

        $('#point_to_note').css('display', 'none');
        var lastPos = 0;
        var initTop = 40;

        //AdjustLegendStyleForLanguage
        if (pageLanguage == cultureLanguageTradChinese) {
            $('#legendPart1').removeClass('legend-part1');
            $('#legendPart1').addClass('legend-part1-zh');
            $('#legendPart2').removeClass('legend-part2');
            $('#legendPart2').addClass('legend-part2-zh');
        }

        function initPostValues() {
            //initialize hidden field values
            $('#hiddenInputServiceProviderName').val(InputServiceProviderName);
            $('#hiddenInputPracticeName').val(InputPracticeName);
            $('#hiddenInputPracticeAddress').val(InputPracticeAddress);
            $('#hiddenSelectedProfession').val(selectedProfession);
            $('#selectedScheme').val(selectedScheme);

            $('#selectedDistrict').val(selectedDistrict);
            $('#selectedTab').val(selectedTab);

            $('#hiddenSelectedIsNonClinic').val(selectedIsNonClinic);
            $('#IsFreeSub').val(selectedIsFreeSub);

            $('#pageIndex').val('@Model.PageIndex');
            $('#pageSize').val('@Model.PageSize');
            $('#pageActualSize').val('@Model.PageActualSize');
            pageActualSize = $('#pageActualSize').val();
            $('#sortType').val('@Model.SortType');
            $('#sortField').val('@Model.SortField');
            $('#sortColName').val('@Model.SortColName');
            $('#requestType').val('@Model.RequestType');
            if (queryLanguageFromServer != '') {
                $('#querylanguage').val(queryLanguageFromServer);
            }
            $('#isReset').val(isReset);

            //initialize form field values
            $('#InputServiceProviderName').val(InputServiceProviderNameByForm);
            $('#InputPracticeName').val(InputPracticeNameByForm);
            $('#InputPracticeAddress').val(InputPracticeAddressByForm);
            $('#Profession').find('.chk').prop('checked', false);
            if (selectedProfessionByForm != '') {
                $('#' + selectedProfessionByForm).val(selectedProfessionByForm);
                $('#' + selectedProfessionByForm).prop('checked', true);
                $('.professionBadge').append('1');
                $('.professionBadge').addClass('active');

                if ($('#' + selectedProfessionByForm).attr('eligible-scheme') != 'ALL') {
                    $('.VSS_container').find('.chk').prop('checked', false);
                    $('.VSS_container').find('.chk_container').css('color', '#585858');
                    $('.VSS_container').find('.chk_container').addClass('disabled');
                    $('.VSS_container').find('.chk').addClass('disabled');
                    $('.VSS_container').find('.chk').attr('disabled', true);
                    var countChecked = 0;
                    $('.schemeBadge').empty();
                    $('#Scheme').find('.chk').each(function () {
                        if ($(this).prop('checked')) {
                            countChecked++;
                        }
                    });


                    if (countChecked > 0) {
                        $('.schemeBadge').append(countChecked);
                    } else {
                        $('.schemeBadge').empty();
                    }
                } else {
                    $('.VSS_container').find('.chk_container').removeClass('disabled');
                    $('.VSS_container').find('.chk').removeClass('disabled');
                    $('.VSS_container').find('.chk').attr('disabled', false);
                }
            }
            if (selectedSchemeByForm != '') {
                var schemeList = selectedSchemeByForm.split(',');
                for (i = 0; i < schemeList.length; i++) {
                    $('#' + schemeList[i]).prop('checked', true);
                }
                $('.schemeBadge').append(schemeList.length);


            }
            
            //SelectedIsNonClinic
            if (selectedIsNonClinicByForm != '') {

                
                $('#' + selectedIsNonClinicByForm).prop('checked', true);
                $('#' + selectedIsNonClinicByForm).closest('.chk_container').css('color', 'rgb(1, 113, 186)');
                 //console.log(domEle);
                

                if ($('.schemeBadge').is(':empty')) {
                    $('.schemeBadge').html(1);
                }
                else {
                    let currentBadge = parseInt($('.schemeBadge').text());
                    $('.schemeBadge').html(1 + currentBadge);

                }




            }
            

            //SelectedIsFreeSub
            //IsFreeSub schemeBadge + 1
            if (selectedIsFreeSubByForm != '') {

                $('#FreeSub').prop('checked', true);
                $('#FreeSub').closest('.chk_container').css('color', 'rgb(1, 113, 186)');


                if ($('.schemeBadge').is(':empty')) {
                    $('.schemeBadge').html(1);
                }
                else {
                    let currentBadge = parseInt($('.schemeBadge').text());
                    $('.schemeBadge').html(1 + currentBadge);

                }
            }




            //selectedDistrict
            if (selectedDistrictByForm != '') {
                var districtList = selectedDistrictByForm.split(',');
                var countDistrictCode = 0;
                for (i = 0; i < districtList.length; i++) {
                    $('#' + districtList[i]).prop('checked', true);
                    if (districtList[i] != 1 && districtList[i] != 2 && districtList[i] != 3) {
                        countDistrictCode++;
                    }
                }
                $('.districtBadge').append(countDistrictCode);
            }

            //handle selected tab
            $('#professionTab').removeClass('active');
            $('#professionTab').find('i').removeClass('active');
            $('#schemeTab').removeClass('active');
            $('#schemeTab').find('i').removeClass('active');
            $('#districtTab').removeClass('active');
            $('#districtTab').find('i').removeClass('active');

            switch (selectedTab) {
                case '1':
                    $('#professionTab').find('a').click();

                    break;
                case '2':
                    $('#schemeTab').find('a').click();

                    break;
                case '3':
                    $('#districtTab').find('a').click();

                    break;
            }





        };


        //Sample
        function test() {
            if ($('.legend-title').length) {
                var ele = $('.legend-title');
                ele.on('click', function (event) {
                    alert(pageSize);
                });
            }
        }
        $(window).on('resize', function () {
            hideResultLRButton();
            InitFreezeDivWidth();
        });
        $(window).scroll(function () {
            hideResultLRButton();
            var tableTop = $("#divResult").offset().top;
            var menuYloc = $("#buttonDiv").offset().top;
            var divPaginationTop = $("#divPagination").offset().top;
            var offsetTop = $(window).scrollTop();
            var placeHeight = 250;
            var isScrollToTable = offsetTop + placeHeight > tableTop;//scroll is over the result table
            var isScrollToTableBottom = divPaginationTop - 450 > offsetTop;
            if (isScrollToTable && isScrollToTableBottom) {
                $("#buttonDiv").offset({ top: offsetTop + placeHeight });
            } else if (!isScrollToTable) {
                $('#buttonDiv').css('top', 40 + 'px');
            }
        });

    });

    function getErrorMsg(Lang) {
        var applicationPath = '@IIf(Request.ApplicationPath = "/", "", Request.ApplicationPath)';

        if (sessionStorage.getItem("errMsg")) {
            errMsgList = JSON.parse(sessionStorage.getItem("errMsg"))
        }
        else {
            if (!sessionStorage.getItem("lang")) {
                sessionStorage.setItem("lang", Lang);
            }
            $.ajax({
                async: false,
                //url: "@Url.Action("systemmsg", "value")",
                url: applicationPath + "/" + sessionStorage.getItem("lang") + "/value/systemmsg",
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
    function SearchResult(vQueryLang, isBtnSearch) {
        firstGetResult = false;
        pageIndex = 1;
        requestType = 'criteria';
        $('#pageIndex').val(pageIndex);
        $('#requestType').val(requestType);
        $('#isReset').val('False');
        $('#querylanguage').val(vQueryLang);
        GetResult(isBtnSearch);
        return false;
    }

    function showPracticeDetail(e) {
        $('.modal-dialog').empty();
        var detail = e.parent('div').next('.detailInfo').text();
        //alert(detail);
        detail = detail.hexEncode();
        //detail = detail.hexDecode();
        //if ((/[\uff01-\uff60|\u3000]/).test(detail)) {
        //    detail = FullToHalf(detail);
        //}
        //alert(detail);
        $.ajax({
            url: "@Url.Action("PopUpDetail", "SPS")",
            dataType: "html",
            type: "POST",
            data: {
                "JsonDetailHex": $.trim(detail)
            },
            success: function (data) {
                $('.modal-dialog').empty();
                $('.modal-dialog').html(data);
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
    function openOnKeyDown(e, obj) {
        switch (event.keyCode) {
            case 13:
                obj.click();
                break;
            default:
                break;
        }
    }
    function ResetAll() {
        $('#searchBox .chk').prop('checked', false);
        $('#searchBox .chk').removeClass('disabled');
        $('#searchBox .chk').attr('disabled', false);
        $('#searchBox .chk_container').css('color', '#585858');
        $('.badge').empty();
        $('input[type=text]').val('');
        $('.alert').css('display', 'none');
        if ($('.chk_container').hasClass('disabled')) {
            $('.chk_container').removeClass('disabled');
        };
        $('#isReset').val('True');
    }
    function HideMobileResult() {
        $('#PointToNote').css('display', 'none');
        $('#btnLoadMore').css('display', 'none');
        $('#ulSort').css('display', 'none');
        $('.result_tab').css('display', 'none');
    }

    function GetResult(isBtnSearch) {
        $('#hasResult').val('true');
        if (isBtnSearch) {
            actionReason = '';
        }
        if (actionReason == '') { //Other action
            $('#querylanguage').val(pageLanguage);
        }

        if (actionReason != '') {// if change language.
            pageSize = $('#pageSize').val();
            pageActualSize = $('#pageActualSize').val();
            pageIndex = $('#pageIndex').val();
            sortField = $('#sortField').val();
            sortColName = $('#sortColName').val();
            sortType = $('#sortType').val();
            requestType = $('#requestType').val();
        }
        //Handle the fields value into the hidden fields.
        //If it is from search action, get value from UI fields, else, get values from hidden fields directly.
        if (isBtnSearch) {
            $("#hiddenInputServiceProviderName").val($("#InputServiceProviderName").val());
            $("#hiddenInputPracticeName").val($("#InputPracticeName").val());
            $("#hiddenInputPracticeAddress").val($("#InputPracticeAddress").val());
            $("#hiddenSelectedProfession").val('');
            $("#hiddenSelectedIsNonClinic").val('');

            $("#IsFreeSub").val("");
            $("#selectedScheme").val('');
            $('#selectedDistrict').val('');
            $("input[type='radio']:checked").each(function (index, domEle) {
                if ($(domEle).attr('name') == 'selectedProfession') {
                    $("#hiddenSelectedProfession").val($(domEle).val());
                }
            });
            var schemeList = '';
            $(".chkScheme").each(function (index, domEle) {
                if ($(domEle).prop('checked') == true) {
                    schemeList = schemeList + $(domEle).attr('id') + ',';
                    //console.log(domEle);
                }
            });

            $(".chkClinic").each(function (index, domEle) {
                if ($(domEle).prop('checked') == true) {
                    $("#hiddenSelectedIsNonClinic").val($(domEle).attr('id'));
                    //console.log(domEle);
                }
               
            });

            $('#selectedScheme').val(schemeList.substring(0, schemeList.length - 1));
            //console.log(schemeList.substring(0,schemeList.length-1));



            if ($("#FreeSub").prop('checked') == true) {
                $("#IsFreeSub").val("Y");
                //console.log(domEle);
            }



            var districtList = '';
            $(".chkDistrict").each(function (index, domEle) {
                if ($(domEle).prop('checked') == true) {
                    districtList = districtList + $(domEle).attr('id') + ',';
                    //console.log(domEle);
                }
            });
            $('#selectedDistrict').val(districtList.substring(0, districtList.length - 1));
            //console.log(schemeList.substring(0,schemeList.length-1));
            $('#pageActualSize').val($('#pageSize').val());
        }
        var modelData = $('form').serializeArray();
        //var objModel = { "ProviderName": "", "PracticeName": "", "PracticeAddress": "", "Profession": "", "Scheme": "", "District": "", "pageSize": pageSize, "pageActualSize": $('#pageActualSize').val(), "pageIndex": pageIndex, "sortField": sortField, "sortColName": sortColName, "sortType": sortType, "requestType": requestType, "queryLanguage": "", "hasResult": "", "isSearch": isBtnSearch };
        var objModel = { "ProviderName": "", "PracticeName": "", "PracticeAddress": "", "Profession": "", "Scheme": "", "District": "", "pageSize": pageSize, "pageActualSize": $('#pageActualSize').val(), "pageIndex": pageIndex, "sortField": sortField, "sortColName": sortColName, "sortType": sortType, "requestType": requestType, "queryLanguage": "", "hasResult": "", "IsNonClinic": "", "IsFreeSub": "" };
        for (var i = 0; i < modelData.length; i++) {
            if (modelData[i]['name'] == "hiddenSelectedProfession") {
                objModel.Profession = modelData[i]['value'];
            } else if (modelData[i]['name'] == "hiddenInputServiceProviderName") {
                objModel.ProviderName = modelData[i]['value'];
            } else if (modelData[i]['name'] == "hiddenInputPracticeName") {
                objModel.PracticeName = modelData[i]['value'];
            } else if (modelData[i]['name'] == "hiddenInputPracticeAddress") {
                objModel.PracticeAddress = modelData[i]['value'];
            } else if (modelData[i]['name'] == "querylanguage") {
                objModel.queryLanguage = modelData[i]['value'];
            } else if (modelData[i]['name'] == "selectedScheme") {
                objModel.Scheme = modelData[i]['value'];
            } else if (modelData[i]['name'] == "selectedDistrict") {
                objModel.District = modelData[i]['value'];
            }
            else if (modelData[i]['name'] == "hasResult") {
                objModel.hasResult = modelData[i]['value'];
            }

            else if (modelData[i]['name'] == "hiddenSelectedIsNonClinic") {
                objModel.IsNonClinic = modelData[i]['value'];
            }
            else if (modelData[i]['name'] == "IsFreeSub") {
                objModel.IsFreeSub = modelData[i]['value'];
            }
        }
        if (objModel.ProviderName == '' && objModel.PracticeName == '' && objModel.PracticeAddress == '' && objModel.Profession == '' && objModel.Scheme == '' && objModel.District == '' && objModel.IsNonClinic == '' && objModel.IsFreeSub == '') {
            var errMsg = showErrorMsg('040101-E-00004');
            ErrorLog(errMsg)
            HideResultStyle();
        } else {
            $('.alert').css('display', 'none');
            $("#mobileLegend").hide();

            $.ajax({
                async: false,
                url: "@Url.Action("GetResult", "SPS")",
                //cache: false,
                dataType: "html",
                data: {
                    "dataModel": JSON.stringify(objModel)
                },
                success: function (data) {
                    if (data) {
                        var tempContainer = $("<div></div>");
                        tempContainer.append(data);
                        var isValid = tempContainer.find('#isValid').first().text()

                        if (isValid == "True") {
                            recordTotal = tempContainer.find('#RecordTotal').first().text();

                            var mobileList = $('.result_tab');
                            if (mobileList.length == 0) return false;
                            $('#PointToNote').css('display', 'block');
                            $('#btnLoadMore').css('display', 'block');
                            $('#ulSort').css('display', 'block');
                            $('.result_tab').css('display', 'block');
                            $("#mobileLegend").show();
                            if (isBtnSearch) scrollToNoted($('#PointToNote'));
                            if (pageIndex == 1)
                                mobileList.empty();
                            mobileList.append(tempContainer.find('.resultList_container').first().html());//append result on mobile
                            var newDom = $('<div></div>');
                            newDom.append(data);

                            //filter sorting by query
                            var allboxs = $('.VSS_container').find('.chkScheme');
                            var checkedBoxs = $('.VSS_container').find('.chkScheme:not(:checked)');
                            $('#sortFieldSIV li').show();
                            $('#sortFieldSIV li').removeAttr("ishide");
                            if (allboxs.length != checkedBoxs.length) {
                                checkedBoxs.each(function () {
                                    $('li[sortcode="' + $(this).attr("id") + '"]').hide();
                                    $('li[sortcode="' + $(this).attr("id") + '"]').attr("ishide", 1);
                                })
                            }

                            //get pageSize list
                            $.ajax({
                                type: "post",
                                url: '@Url.Action("GetPageSizeList", "SPS")',
                                data: "hasvss=" + true,
                                async: false,
                                success: function (pageSizes) {
                                    $('.pageSizeBox').empty();
                                    $('.pageSizeBoxMibile>.district').remove();
                                    for (var i = 0; i < pageSizes.length; i++) {
                                        var size = pageSizes[i];
                                        $('.pageSizeBox').append('<li><a href="javascript:void(0);">' + size + '</a></li>');

                                        var htm = '<li class="district">' +
                                        '<div class="chk_container radioLabel" style="color: rgb(88, 88, 88);">' +
                                        '<span>' + size + ' @Resource.Text("ResultPageItems")</span>' +
                                        '<input aria-label="' + size + '@Resource.Text("ResultPageItems")" class="chk" id="page' + i + '" name="page" type="radio" value="' + size + '">' +
                                        '</div>' +
                                        '</li>';
                                        $('.pageSizeBoxMibile').append(htm);
                                    }
                                    initPagesizemenu();
                                    initSortTab();
                                }
                            });

                            $("#orderQueryBox").show();


                            var desktopResult = $('#divResult');
                            if (desktopResult.length == 0) return false;
                            desktopResult.empty();
                            desktopResult.html(newDom.find('.SPTable,.lastUpdateBox'));

                            if (tempContainer.find('.hasVSS').first().length > 0) {
                                $('.VSSNoteTab').css('display', 'block');
                                $('#sortFieldSIV').show();
                                hasVSS = true;
                            } else {
                                hasVSS = false;

                                $('.VSSNoteTab').css('display', 'none');
                                desktopResult.find('.showWithVSS').css('display', 'none');
                                $('.HCVSItem').css("border-radius", "0px 10px 10px 0px");
                                $('.HCVSItem').css("border-right", "none");
                                $('#sortFieldSIV').hide();
                                var rdoCount = $("#sortFieldHCVS").find(':radio:checked').length;
                                if (rdoCount == 0) {
                                    var deradio = $("#sortNameTabBox").find(':radio').first();
                                    $("#sortField").val(deradio.val());
                                    $("#sortColName").val(deradio.attr('colname'));
                                }
                            }

                            //if have no sorting,set default sorting
                            var rdoCount = $("#sortNameTabBox li.district[ishide!='1']").find(':radio:checked').length;
                            if (rdoCount == 0) {
                                var deradio = $("#sortNameTabBox").find(':radio').first();
                                $("#sortField").val(deradio.val());
                                $("#sortColName").val(deradio.attr('colname'));
                            }

                            pageTotal = newDom.find('#PageTotal').first().text();
                            pageIndex = newDom.find('#PageIndex').first().text();
                            pageSize = newDom.find('#PageSize').first().text();
                            recordTotal = newDom.find('#RecordTotal').first().text();
                            requestType = newDom.find('#RequestType').first().text();
                            if (parseInt(pageIndex) >= parseInt(pageTotal))
                                $("#btnLoadMore").hide();//last page,hide "load more" button.
                            else
                                $("#btnLoadMore").show();
                            ResetRecordTotal(recordTotal);
                            if (requestType == 'criteria' || requestType == 'pageSize') {
                                ResetPaginator(pageIndex, pageTotal);
                            }
                            if (actionReason != '') {
                                ResetPaginator(pageIndex, pageTotal);
                                $('.pageSize').text(pageSize);
                            }
                            initMobileSortBar();
                            initMoveTable();
                            initPageNumberClick();
                            initSortButton();
                            initTableColOrder();
                            //change language,keep sort icon style is none
                            if (queryLanguageFromServer == '' || queryLanguageFromServer == pageLanguage || (!firstGetResult && queryLanguageFromServer != pageLanguage)) {
                                setSortButtonStyle();
                            }
                            initClinicClick();
                            ShowResultStyle();
                            initSPSearchBox();
                            setSpanSortTitle();
                            setSpanSortItemTitle();
                            freezeCol();
                        }
                        else {
                            var lstErrorCode = tempContainer.find('#lstErrorCode').first().text().split(',')
                            for (var i in lstErrorCode) {
                                showErrorMsg(lstErrorCode[i]);
                                HideResultStyle();
                            }
                        }
                    } else {
                        showErrorMsg('990000-I-00001');
                        HideResultStyle();
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

        function showErrorMsg(errCode) {
            $('#PointToNote').css('display', 'none');
            $('#btnLoadMore').css('display', 'none');
            $('#ulSort').css('display', 'none');
            var mobileList = $('.result_tab');
            mobileList.empty();
            var desktopResult = $('#divResult');
            desktopResult.empty();
            var lang = rootLang;
            var msg = findMsg(errCode)
            var errMsg = msg + '[' + errCode + ']<br>';
            $('#txtErrMsg').html(errMsg);
            $('.alert').css('display', 'block');
            scrollToNoted($('.alert'));
            return errMsg;
        }

        function findMsg(lang, code) {
            for (var i = 0; i < errMsgList.length; i++) {
                if (errMsgList[i]['code'] == code && errMsgList[i]['lang'] == lang) {
                    return errMsgList[i]['desc'];
                }
            }
        }
        function findMsg(code) {
            if (!errMsgList || errMsgList.length == 0) {
                errMsgList = JSON.parse(sessionStorage.getItem("errMsg"))
            }
            // iterate over each element in the array
            for (var i = 0; i < errMsgList.length; i++) {
                // look for the entry with a matching `code` value
                if (errMsgList[i].code == code) {
                    // we found it
                    // obj[i].name is the matched result
                    return errMsgList[i].desc;
                }
            }
            return "Undefined"
        }
    }
    function setOrderValue(element, other, text) {
        $(element).removeClass("inactiveType").addClass('activeType');
        $("#" + other).removeClass("activeType").addClass('inactiveType');
        $("#hidOrder").val($(element).attr("value"));
        $("#hidOrder").attr("showvalue", text);
    }
    function orderApply(a) {
        setSpanSortTitle();//show order col
        $("#" + a).click();
        var selectPageSize = $("input[name='page']:checked").val();
        var selectOrderType = $("#hidOrder").val();
        var orderCol = $("input[name='Fields']:checked").val();
        var orderColName = $("input[name='Fields']:checked").attr('colname');
        pageIndex = 1;
        pageSize = selectPageSize;
        $('#pageSize').val(selectPageSize);
        $('#pageActualSize').val(selectPageSize);
        sortType = selectOrderType.toLowerCase();
        $("#sortType").val(selectOrderType.toLowerCase());
        sortField = orderCol;
        $("#sortField").val(orderCol);
        sortColName = orderColName;
        $("#sortColName").val(orderColName);
        GetResult(false);
        $('#scrollToResult').click();
    }
    function setSpanSortTitle() {
        var fText = $("input[name='Fields']:checked").prev().text();
        $("#spanSortTitle").html(fText + " " + $("#hidOrder").attr("showvalue"));
    }
    function setSpanSortItemTitle() {
        var loadCount = parseInt($('#pageActualSize').val()) > recordTotal ? recordTotal : parseInt($('#pageActualSize').val());
        var pageVal = $("input[name='page']:checked").val();
        $("#spanSortItemTitle").html(loadCount + "/" + recordTotal);
        if (loadCount >= recordTotal) {
            $("#btnLoadMore").hide();
        }
    }
    //"load more" button(next page)
    function loadMore() {
        $('#pageActualSize').val(parseInt($('#pageActualSize').val()) + parseInt($('#pageSize').val()));
        GetResult(false);
    }
    //after change language,init sortBar
    function initMobileSortBar() {
        var sortType = $("#sortType").val();
        if (sortType == "asc")
            $("#divAsc").click();
        else
            $("#divDesc").click();
        var pageRadio = $("#sortItemTabBox").find(':radio[value="' + $("#pageSize").val() + '"]');
        pageRadio && pageRadio.parent() && pageRadio.parent().click();
        var fileRadio = $("#sortNameTabBox").find(':radio[colname="' + $("#sortColName").val() + '"]');
        fileRadio && fileRadio.parent() && fileRadio.parent().click();
    }
    function fillDivtoTD() {
        $(".left-cell").each(function (index, domEle) {
            if ($(domEle).find('div').first().find('.clsSPName').first().height() + 30 >= $(domEle).height()) {
                $(domEle).find('div').first().height($(domEle).height() + 35);
            }
            else {
                $(domEle).find('div').first().height($(domEle).height());
            }
            $(domEle).find('div').first().width($(domEle).width());
            $(domEle).find('div').first().css('min-height', '85px');
        });
    }
    //dropdown-menu
    function initPagesizemenu() {
        if ($('.dropdown-menu>li>a').length) {
            var ele = $('.dropdown-menu>li>a');
            ele.on('click', function (event) {
                $('.pageSize').text($(this).text());
                pageSize = parseInt($(this).text());
                requestType = 'pageSize';
                pageIndex = 1;
                $('#requestType').val(requestType);
                $('#pageIndex').val(pageIndex);
                $('#pageSize').val(pageSize);
                $('#pageActualSize').val(pageSize);
                GetResult();
            });
        }
    }

    function ErrorLog(errMsg) {
        var rtnValue;

        $.ajax({
            async: false,
            type: 'POST',
            url: "@Url.Action("ErrorLog", "SPS")",
            dataType: 'json',
            data: JSON.stringify({
                errMsg: errMsg
            }),
            contentType: 'application/json; charset=utf-8',
            success: function (data) {
                rtnValue = data.Rtn;
            },
            complete: function (XMLHttpRequest, textStatus) {
                var errorUrl = XMLHttpRequest.getResponseHeader("ErrorUrl");
                if (errorUrl) {
                    var u = rootPath + rootLang + errorUrl;
                    location.href = u;
                }
            }
        });
        return rtnValue;
    }


    $("input[type='text'], textarea").unbind('change').change(function () {
        if (this.value.length > 0) {
            this.value = this.value.replace(/<([^<\s>])/ig, "< $1")
            if ((/[\uff01-\uff60|\u3000]/).test(this.value)) {
                this.value = FullToHalf(this.value);
            }
        }
    });
    function FullToHalf(str) {
        var tmp = "";
        for (var i = 0; i < str.length; i++) {
            //Space
            if (str.charCodeAt(i) == 0x3000) {
                tmp += " ";
            }
            else if (str.charCodeAt(i) >= 0xFF01 && str.charCodeAt(i) <= 0xFF5E) {
                tmp += String.fromCharCode(str.charCodeAt(i) - 0xFEE0);
            }
            else if (str.charCodeAt(i) == 0xFF5F) {
                tmp += String.fromCharCode(0x2985);
            }
            else if (str.charCodeAt(i) == 0xFF60) {
                tmp += String.fromCharCode(0x2986);
            }
            else {
                tmp += str[i];
            }
        }
        return tmp
    }

    String.prototype.hexEncode = function () {
        var hex, i;

        var result = "";
        for (i = 0; i < this.length; i++) {
            hex = this.charCodeAt(i).toString(16);
            result += ("000" + hex).slice(-4);
        }

        return result
    }
    String.prototype.hexDecode = function () {
        var j;
        var hexes = this.match(/.{1,4}/g) || [];
        var back = "";
        for (j = 0; j < hexes.length; j++) {
            back += String.fromCharCode(parseInt(hexes[j], 16));
        }

        return back;
    }
</script>
<script src="~/js/serviceProvider.js"></script>