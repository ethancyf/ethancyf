@ModelType SPSViewModel
@code
    @<div id="isValid">@Model.IsValid</div>
    If Model.IsValid Then
        @<text>
            <div class="resultList_container">
                @If Model.RecordTotal > 0 Then
                    For Each item In Model.ResultList
                @<div class="row">
                    <div class="col-xs-12">
                        <ul class="mobile_SP_result" data-toggle="modal" data-target="#myModal" onclick="showPracticeDetail($(this))" tabindex="0" onkeydown="return openOnKeyDown(event,$(this))">
                            <li class="mobile-Clinic">
                                @If item.MobileClinic.Equals("Y") Then
                                    @<div style="display: inline;z-index: 13;"><img src="~/Image/SPS/Icon-mclinic.png" style=" width: auto;height:25px" alt="@Resource.Text("SPSResultMobileClinic")" /></div>
                                End If
                                @If item.NonClinic.Equals("Y") And item.MobileClinic.Equals("N") Then
                                    @If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.English Then
                                        @<div style="display: inline;z-index: 12;">
                                             <img src="~/Image/SPS/Icon-nonclinic-eng-2x.png" style=" width: auto;height:25px" alt="@Resource.Text("NonClinic")" />
                                        </div>
                                    End If
                                @If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.TradChinese Then
                                        @<div style="display: inline;z-index: 12;">
                                             <img src="~/Image/SPS/Icon-nonclinic-chi-2x.png" style=" width: auto;height:25px" alt="@Resource.Text("NonClinic")" />
                                        </div>
                                    End If                           
                                End If

                                @If item.NonClinic.Equals("Y") And item.MobileClinic.Equals("Y") Then
                                    @If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.English Then
                                        @<div style="display: inline;z-index: 12;margin-left: -12px;">
                                             <img src="~/Image/SPS/Icon-nonclinic-eng-2x.png" style=" width: auto;height:25px" alt="@Resource.Text("NonClinic")" />
                                        </div>
                                    End If
                                
                                    @If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.TradChinese Then
                                        @<div style="display: inline;z-index: 12;margin-left: -12px;">
                                            <img src="~/Image/SPS/Icon-nonclinic-chi-2x.png" style=" width: auto;height:25px" alt="@Resource.Text("NonClinic")" />
                                        </div>
                                    End If
                                
                                End If

                                @If item.Remark.Equals("Y") And (item.MobileClinic.Equals("Y") Or item.NonClinic.Equals("Y")) Then
                                    @<div style="display: inline;z-index: 11;margin-left: -6px;">
                                         <img src="~/Image/SPS/Icon-remark-top-2x.png" style=" width: auto;height:25px;margin-left:-5px;" alt="@Resource.Text("Remarks")" />
                                    </div>
                                End If

                                @If item.Remark.Equals("Y") And (item.MobileClinic.Equals("N") And item.NonClinic.Equals("N")) Then
                                    @<div style="display: inline;z-index: 11;">
                                         <img src="~/Image/SPS/Icon-remark-top-2x.png" style=" width: auto;height:25px;" alt="@Resource.Text("Remarks")" />
                                    </div>
                                End If

                            </li>
                            <li class="result-Doctor" style="display:flex;justify-content: space-between;">
                                <div>
                                    <div style="font-weight: bold;font-size: 16px;">@item.SPName</div>
                                    <div style="color:#195287;font-weight: bold;font-size: 16px;">@item.Profession</div>
                                </div>
                                <div>
                                    @code
                                        If Not String.IsNullOrEmpty(item.JoinedScheme) Then
                                            Dim schemeList As Array = item.JoinedScheme.Split("|")
                                            For Each schemeItem In schemeList
                                            If schemeItem.Equals("HCVS") Then
                                                @<div style="margin-bottom:2px;">
                                                    <img src="~/Image/SPS/icon_HCVS.png" alt="" />
                                                    <span>@Resource.Text("HCVSShortName")</span>
                                                </div>
                                            ElseIf Not String.IsNullOrEmpty(schemeItem) Then
                                                @<div>
                                                    <img src="~/Image/SPS/icon_VSS.png" alt="" />
                                                    <span>@Resource.Text("VSSShortName")</span>
                                                </div>
                                            End If
                                            Next
                                        End If
                                    End code
                                </div>
                            </li>
                            <li style="padding:10px; display:flex;">
                                <div style=" width:60%; padding-right:10px; ">
                                    <div>@item.PracticeName</div>
                                    <div>@item.PracticeAddress</div>
                                    <div>@item.PracticePhoneNo</div>
                                </div>
                                @code
                                    If Not String.IsNullOrEmpty(item.JoinedScheme) Then
                                        Dim schemeList As Array = item.JoinedScheme.Split("|")
                                        For Each schemeItem In schemeList
                                            If schemeItem.Equals("VSS") Then
                                                @<div style="width:50%;text-align:right;">
                                                    <div>@IIf(String.IsNullOrEmpty(Model.SortFieldDesc), Resource.Text("SPSResultPriceRange"), Model.SortFieldDesc)</div>
                                                    <div>(HK$)</div>
                                                    <div>
                                                        <img src="~/Image/SPS/icon_VSS.png" alt="" />
                                                        <span style="font-size:18px;">@IIf(String.IsNullOrEmpty(Model.SortFieldDesc), item.SubsidizeFeeScope, item.PriceTag)</span>
                                                    </div>
                                                </div>
                                            End If
                                        Next
                                    End If
                                End code
                            </li>
                        </ul>
                    </div>
                    <div class="hidden detailInfo">
                        @item.PracticeDetail
                    </div>
                </div>
                    Next
                    If Model.HasVSS Then
                        @<div class="hasVSS hidden"></div>
                    End If
                End If
                <div class="lastUpdateBox visible-xs" style="color:#252525">
                    @Model.LastUpdateDate
                </div>
        </div>
    </text>
    'For Desktop/Tablet Version
    @<text>
        <span id="RecordTotal">@Model.RecordTotal</span>
        <span id="PageIndex">@Model.PageIndex</span>
        <span id="PageSize">@Model.PageSize</span>
        <span id="PageTotal">@Model.PageTotal</span>
        <span id="RequestType">@Model.RequestType</span>
        <table class="table table-hover SPTable" style="margin-top:10px;border-collapse:separate; border-spacing:0px 3px;width:100%;max-width:none">
		    <tr class="tr-header showWithVSS">
		        @If Model.HasVSS Then
                    @<td class="left-cell" style="border-right:0;cursor:default !important"></td>
                    @<td style="border-right:0;cursor:default !important"></td>
                    @<td style="border-right:0;cursor:default !important"></td>
                    @<td style="border-right:0;cursor:default !important"></td>
                    @<td></td>
            		@<td class="right-cell" colspan="@Model.SubHeaderList.Count" id="tdServiceFee">
                
		                @Resource.Text("SPSResultServiceFeeLargeScreen")
		            </td>

            		End If
		    </tr>
		    <tr class="tr-header showWithVSS">
		    
		        @code
		                Dim headerCount = Model.HeaderList.Count
		                Dim hRightCell = ""
		
		                If headerCount > 0 Then
					    @<td class="left-cell" style="border-right:0;"></td>
			                    @<td style="border-right:0;"></td>
			                    @<td style="border-right:0"></td>
			                    @<td style="border-right:0"></td>
			                    @<td></td>
			            		End If
		
		                For j As Integer = 0 To headerCount - 1
		                    If j = headerCount - 1 Then
		                        hRightCell = "right-cell"
		                    End If
		
		                @<td class="@hRightCell" colspan="@Model.HeaderList(j).ColSpan" coltop="tdServiceFee">@Model.HeaderList(j).Header</td>
		                Next
		        End code
		    </tr>
		    <tr class="tr-header">
		        <td class="left-headcell cur_pointer" style="width:100px;">
		            <div class="resultHeader">@Resource.Text("SPSResultServiceProviderLargeScreen")</div>
		            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="SPName" colname="SPName"></span>
		            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="SPName" colname="SPName"></span>
		        </td>
		        <td style="width:200px; min-width:200px" class="cur_pointer">
		            <div class="resultHeader">@Resource.Text("SPSResultPracticeDetailLargeScreen")</div>
		            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="PracticeName" colname="PracticeName"></span>
		            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="PracticeName" colname="PracticeName"></span>
		        </td>
		        <td style="width:60px; min-width:60px" class="cur_pointer">
		            <div class="resultHeader">@Resource.Text("SPSResultDistrictLargeScreen")</div>
		            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="DistrictName" colname="DistrictName"></span>
		            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="DistrictName" colname="DistrictName"></span>
		        </td>
		        <td style="width:80px; min-width:80px" class="cur_pointer">
		            <div class="resultHeader">@Resource.Text("SPSResultEnrollSchemeLargeScreen")</div>
		            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="JoinedScheme" colname="JoinedScheme"></span>
		            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="JoinedScheme" colname="JoinedScheme"></span>
		        </td>
		        <td style="width:60px;min-width:60px" class="HCVSItem cur_pointer">
		            <div class="resultHeader">@Resource.Text("SPSResultHealthcareProfessionalLargeScreen")</div>
		            <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="Profession" colname="Profession"></span>
		            <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="Profession" colname="Profession"></span>
		        </td>
		        @code
		                Dim subHeaderCount = Model.SubHeaderList.Count
		                For j As Integer = 0 To subHeaderCount - 1
		                    Dim Item = Model.SubHeaderList(j)
		                    Dim rightCell = ""
		
		                    If j = subHeaderCount - 1 Then
		                        rightCell = "right-cell"
		                    End If
		
		            @<td class="@rightCell showWithVSS cur_pointer" style="width:60px;min-width:60px" coltype="@Item.Header" coltop="@Item.SortItem" displaytype="schema">
		                <div class="resultHeader">@Item.Header</div>
		                <span class="glyphicon glyphicon-triangle-bottom sort-down" aria-hidden="true" data-column="@Item.SortItem" colname="@Item.SubsidizeFeeColumnName"></span>
		                <span class="glyphicon glyphicon-triangle-top sort-up" aria-hidden="true" data-column="@Item.SortItem" colname="@Item.SubsidizeFeeColumnName"></span>
		            </td>
		                Next
		        End code
		    </tr>
		    @code
		            Dim i As Integer = 0
		        @For Each item In Model.ResultList
		                Dim rowId, clinicId As String
		                rowId = "practice_" + i.ToString()
		                clinicId = "practice_" + i.ToString() + "_clinic"
		                Dim hasRemark = False
		                
		                i = i + 1
		            @<tr class="tr-content" tabindex="0">
		                <td class="left-cell" style="
		            @code
		                Dim styleCursor As String = "cursor:default !important;"
		                If item.MobileClinic <> "Y" And item.NonClinic <> "Y" And item.Remark <> "Y" Then
		            @styleCursor
	                    Else
                            hasRemark = True
                        End If
		            End code
		                ">
		                <div>
		                <div style="text-align:left;">
		                <div style="display:inline;">
											@Html.Raw(If(hasRemark, String.Format("<a href='javascript:void(0)' aria-label='{0}' arialabel='{0}'>", Resource.Text("Remarks")), ""))
		                @code
		                If item.MobileClinic = "Y" Then
		                    @<div style="z-index:12;display:inline;">
		                    <button tabindex="-1" class="img-clinic" type="button" style="background:none; border:none; display:inline;outline:none;top:0px;">
                                <img aria-hidden="true"  src="~/Image/SPS/Icon-mclinic.png" style="display: flex;width: 36px;" id="@rowId" alt="@Resource.Text("SPSResultMobileClinic")" />
		                    </button>
		                    </div>
		                End If
		
		                If item.NonClinic = "Y" Then
		                    @<div style="z-index:11;display:inline;">
		                    <button tabindex="-1" class="img-clinic" type="button" style="background:none; border:none; display:inline;outline:none;top:0px;
		                    @code
		                    Dim style As String = "margin-left:-11px;"
		                    If item.MobileClinic = "Y" Then
		                        @style
		                    End If
		                    End code
		                    ">
		
		                    @If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.English Then
		                        @<img aria-hidden="true" src="~/Image/SPS/Icon-nonclinic-eng-2x.png" style="display: flex;width: 36px;" id="@rowId" alt="@Resource.Text("NonClinic")" />
                            End If
		                    @If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.TradChinese Then
		                        @<img aria-hidden="true" src="~/Image/SPS/Icon-nonclinic-chi-2x.png" style="display: flex;width: 36px;" id="@rowId" alt="@Resource.Text("NonClinic")" />
                            End If
		                    </button>
		                    </div>
		                End If
		
		                If item.Remark = "Y" Then
		                    @<div style="z-index:10;display:inline;">
		                    <button tabindex="-1" class="img-clinic" type="button" style="background:none; border:none; display:inline;outline:none;top:0px;
		                    @code
		                    Dim style As String = "margin-left:-11px;"
		                    If item.MobileClinic = "Y" Or item.NonClinic = "Y" Then
		                        @style
		                    End If
		                    End code
		                    ">

		                    <img src="~/Image/SPS/Icon-remark-top-2x.png" style="display: flex;width: 36px;" id="@rowId" alt="@Resource.Text("Remarks")"/>
		                    </button>
		                    </div>
		                End If
		                End code
		                @Html.Raw(If(hasRemark, "</a>", ""))
		                </div>
		                </div>
		
		                <div class="clsSPName" style="padding-top:8px;padding-left:10px;text-align:left">
		                @item.SPName
		                </div>
		                </div>
		                </td>
		                <td style="text-align:left">
		                    @item.PracticeName
		                    <br />
		                    @item.PracticeAddress
		                    <br />
		                    @item.PracticePhoneNo
		                </td>
		                <td>@item.DistrictBoardName</td>
                         <td>
                             @code
                             If Not String.IsNullOrEmpty(item.JoinedScheme) Then
                             If item.JoinedScheme.Contains("HCVS") Then
                                 @<img src="~/Image/SPS/icon_HCVS.png" class="img-responsive img-column" style="margin-bottom: 5px;" alt="@Resource.Text("HCVSShortName")" />
                             End If
                             If item.JoinedScheme.Contains("VSS") Then
                                 @<img src="~/Image/SPS/icon_VSS.png" class="img-responsive img-column" alt="@Resource.Text("VSSShortName")" />
                             End If
                             End If
                             End code
                         </td>
                <td class="HCVSItem">
                    @item.Profession
                </td>
                <!-- Search Point 3 -->
                @code
                    Dim allHeaders = New List(Of String)()
                    For Each header As SchemeHeader In Model.HeaderList
                        For j As Integer = 0 To header.ColSpan - 1
                            allHeaders.Add(header.Header)
                        Next
                    Next

                Dim subsiCount = item.SubsidizeList.Count
                For j As Integer = 0 To subsiCount - 1
                    Dim isDecimal = False
                    Decimal.TryParse(item.SubsidizeList(j).Fee, isDecimal)
                    'Dim pattern = "^\-{0,1}[0-9]{0,}\.{0,1}[0-9]{1,}$"
                    'Dim isDecimal = System.Text.RegularExpressions.Regex.IsMatch(item.SubsidizeList(j).Fee, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase)
                            Dim unit = If(isDecimal, Resource.Text("AriaHongKongDollar"), "")
                    If j = subsiCount - 1 Then
                    @<td class="right-cell showWithVSS" aria-label="@allHeaders(j) @Model.SubHeaderList(j).SubsidizeDesc @item.SubsidizeList(j).Fee @unit">@item.SubsidizeList(j).Fee</td>
                    Else
                    @<td class="showWithVSS" aria-label="@allHeaders(j) @Model.SubHeaderList(j).SubsidizeDesc @item.SubsidizeList(j).Fee @unit">@item.SubsidizeList(j).Fee</td>
                    End If
                Next
                End code
            </tr>
            @code
                If item.MobileClinic = "Y" Or item.NonClinic = "Y" Or item.Remark = "Y" Then
                @<tr class="tr-content mobile-clinic" style="display:none;border-spacing:0px 0px;" id="@clinicId" tabindex="0">
                    @code
                    Dim cols = If(Model.HasVSS, item.SubsidizeList.Count + 5, 5)
                    End code
                    <td colspan="@cols" style="text-align:left;">
                        <div class="freezeDiv">
                            <div style="padding-bottom:2px;font-size: 14px; font-weight:bold;color: #195287; padding-bottom:8px">@Resource.Text("Remarks")</div>
                            @If item.Remark = "Y" Then
                                @<div style="padding-bottom:8px">
                                    @item.RemarkDesc
                                </div>
                            End If

                            @If item.MobileClinic = "Y" Then
                                @<div style="color:#777575;padding-bottom:8px">@Resource.Text("MobileClinicNote")</div>
                            End If

                            @If item.NonClinic = "Y" Then
                                @<div style="color:#777575;padding-bottom:8px">@Resource.Text("NonClinicNote")</div>




                            End If
                        </div>
                    </td>
                </tr>
                End If
            End code
            Next
    End code
</table>
            <div class="lastUpdateBox freezeDiv">
                @Model.LastUpdateDate
            </div>
    </text>
    Else
        @<div id="lstErrorCode">@String.Join(",", Model.lstErrorCodes)</div>
    End If
End Code
