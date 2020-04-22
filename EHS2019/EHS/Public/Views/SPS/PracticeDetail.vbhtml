@ModelType SPSViewModel
@code
    For Each item In Model.ResultList
        @<text>
            <ul class="practice_detail">
                <li style="display:flex;justify-content:space-between;">
                    <ul class="practice_detail_header" style="color:#585858;">
                        <li style="font-size:18px;">@item.SPName</li>
                        <li style="font-size:15px;color:#195287;">@item.Profession</li>
                        <li class="detail_JoinedScheme">
                            @code
                                Dim isVCC = False
                                If Not String.IsNullOrEmpty(item.JoinedScheme) Then
                                    Dim schemeList As Array = item.JoinedScheme.Split("|")
                                    For Each schemeItem In schemeList
                                        If schemeItem.Equals("HCVS") Then
                                            @<div style="padding-right:15px;">
                                                <img src="~/Image/SPS/icon_HCVS.png" alt="" />
                                                <span>@Resource.Text("HCVSShortName")</span>
                                            </div>
                                        ElseIf Not String.IsNullOrEmpty(schemeItem) Then
                                            isVCC = True
                                            @<div style="padding-right:15px;">
                                                <img src="~/Image/SPS/icon_VSS.png" alt="" />
                                                <span>@Resource.Text("VSSShortName")</span>
                                            </div>
                                        End If
                                    Next
                                End If

                                If item.MobileClinic.Equals("Y") Then
                                    @<div style="padding-right:15px;">
                                        <img src="~/Image/SPS/Icon-mclinic.png" alt="" />
                                        <span>@Resource.Text("SPSResultMobileClinic")</span>
                                    </div>
                                End If
                                If item.NonClinic.Equals("Y") Then
                                    @<div style="padding-right:15px;">
                                        @code
                                            If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.English Then
                                                @<img src="~/Image/SPS/Icon-nonclinic-eng-2x.png" alt="" />
                                            End If
                                            If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.TradChinese Then
                                                @<img src="~/Image/SPS/Icon-nonclinic-chi-2x.png" alt="" />
                                            End If
                                            @<span>@Resource.Text("NonClinic")</span>
                                        End code
                                    </div>
                                End If
                                If item.Remark.Equals("Y") Then
                                    @<div style="padding-right:15px;">
                                        <img src="~/Image/SPS/Icon-remark-top-2x.png" alt="" />
                                        <span>@Resource.Text("Remarks")</span>
                                    </div>
                                End If
                            End code
                        </li>
                    </ul>
                    <div>
                        <button type="button" class="practice_detail_btnClose" data-dismiss="modal">
                            <div style="margin-left:30%;margin-top:-15%;">
                                <span class="glyphicon glyphicon-remove" aria-hidden="true" style="font-size:16px;"></span>
                                <div style="font-size:12px;">@Resource.Text("SPSResultClose")</div>
                            </div>
                        </button>
                    </div>
                </li>
                <li style="background-color:#e6f6ff;">
                    <ul class="practice_detail_scheme">
                        <li>
                            <h1>@Resource.Text("SPSResultNameAddNo")</h1>
                            <h2>@item.PracticeName</h2>
                            <h2>@item.PracticeAddress</h2>
                            <h2>@item.PracticePhoneNo</h2>
                        </li>
                        @If item.MobileClinic.Equals("Y") Or item.NonClinic.Equals("Y") Or item.Remark.Equals("Y") Then
                            @<li>
	                             <h1 style="padding-bottom:2px">@Resource.Text("Remarks")</h1>
	                             @If item.Remark.Equals("Y") Then
	                                 @<h2>@item.RemarkDesc</h2>
	                             End If
	                             @If item.MobileClinic.Equals("Y") Then
	                                 @<h2 style="color:#a5a5a5;">@Resource.Text("MobileClinicNote")</h2>
	                             End If
	                             @If item.NonClinic.Equals("Y") Then
	                                 @<h2 style="color:#a5a5a5;">@Resource.Text("NonClinicNote")</h2>
	                             End If
                            </li>
                        End If
                        <li>
                            <h1>@Resource.Text("SPSTabDistrict")</h1>
                            <h2>@item.DistrictBoardName</h2>
                        </li>
                    </ul>
                </li>
                <li>
                    @If isVCC Then
                        @<ul class="practice_detail_VACCFee">
                            <li style="color:#195287;">
                                <div style="display:flex;">
                                    <img style="margin-right:10px;" src="~/Image/SPS/icon_VSS.png" alt="" />
                                    <span>@Resource.Text("SPSResultVACCServiceFee")</span>
                                </div>
                                <div style="text-align:right;">
                                    <span>
                                        @Resource.Text("SPSResultPrice")
                                    </span>
                                </div>
                            </li>
                            @For Each feeItem In item.FeeList
                                @<li>
                                    <div>
                                        <span>@feeItem.FeeDesc</span>
                                    </div>
                                    <div style="color:green;font-size:15px;">
                                        <span>
                                            @feeItem.Fee
                                        </span>
                                    </div>
                                </li>
                            Next
                        </ul>
                    Else
                        @<div style="height:20px;"></div>
                    End If
                </li>
            </ul>
        </text>
    Next
End Code