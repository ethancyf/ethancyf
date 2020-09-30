@Code
    ViewData("Title") = Resource.Text("ContactUs")
    Layout = "~/Views/Shared/_Layout.vbhtml"
End Code

<div class="container policy_bigbox" id="divContacts">
    <!--Title 'Contact Us'-->
    <div class="row contact_titlebox">
        <div class="col-xs-12 col-sm-12">
            <h1 class="contact_title" id="skiptarget">@Resource.Text("ContactUs")</h1>
        </div>
    </div>
    <div class="row contact_cntbox">
        <div class="col-xs-12 col-sm-6">
            <div class="container-fluid">
                <div class="row contact_section_1_topbox contact_topbox">
                    <div class="col-xs-12 col-sm-12">
                       
                            <div class="media">
                                <div class="media-left media-middle visible-xs">
                                    <img class="media-object contact_titleimg" src="../../Image/SPS/icon_HCVS.png" alt="icon_HCVS">
                                </div>
                                <div class="media-body media-middle contact_section_1_toptitle contact_titletext">
                                    <div class="center-vertical">
                                        <div class="contact_header">
                                            @If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower = Common.Component.CultureLanguage.English Then
                                                @Html.Raw(Resource.Parameter("ContactUsOfficeName_Voucher_EN"))
                                            Else
                                                @Html.Raw(Resource.Parameter("ContactUsOfficeName_Voucher_CHI"))
                                            End If
                                        </div>
                                    </div>
                                </div>
                            </div>
                       
                    </div>
                </div>
                <div class="row contact_section section_1_cntbox">
                    <div class="col-xs-12 col-sm-12 transLayer">
                        <div class="row contact_section_1_addressbox contact_middlebox">
                            <div class="col-xs-12 col-sm-12 contact_section_1_text">
                                <strong class="visible-xs contact_labeltext">@Resource.Text("Address")<br /></strong>
                                <div class="addresstext">
                                    <div style="min-height:65px">
                                        @If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower = Common.Component.CultureLanguage.English Then
                                            @Html.Raw(Resource.Parameter("ContactUsOfficeAddress_Voucher_EN"))
                                        Else
                                            @Html.Raw(Resource.Parameter("ContactUsOfficeAddress_Voucher_CHI"))
                                        End If
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="hidden-xs" style="padding-bottom:10px">
                            <table>
                                <tr>
                                    <th class="contact_section_1_text" style="min-height:30px;font-weight:normal"><span class="contact_labeltext">@Resource.Text("Email_ShotForm"):&nbsp;</span></th>
                                    <td><a class="footer_conect" href="mailto:hcvd@dh.gov.hk">hcvd@dh.gov.hk</a></td>
                                </tr>
                                <tr>
                                    <td class="contact_section_1_text" style="min-height:30px"><span class="contact_labeltext">@Resource.Text("Tel"):&nbsp;</span></td>
                                    <td><a class="footer_conect" href="tel:35824102">2838 2311</a></td>
                                </tr>
                                <tr>
                                    <td class="contact_section_1_text" style="min-height:30px"><span class="contact_labeltext">@Resource.Text("Site"):&nbsp;</span></td>
                                    <td><a class="footer_conect" href="@Resource.Text("hcvURL")" target="_blank" rel="noopener noreferrer">http://www.hcv.gov.hk</a></td>
                                </tr>
                            </table>
                        </div>

                        <div class="row contact_btbox visible-xs">
                            <div class="col-xs-12 col-sm-12">
                                <div class="row contact_section_1_text contact_section_1_commombox contact_bottomline">
                                    <div class="col-xs-12 col-sm-12">
                                        <span class="contact_labeltext">@Resource.Text("Email_ShotForm")</span><span class="hidden-xs">:</span> <a class="footer_conect contact_rCnt" href="mailto:hcvd@dh.gov.hk">hcvd@dh.gov.hk</a>
                                    </div>
                                </div>
                                <hr class="contact_line visible-xs" />
                                <div class="row contact_section_1_text contact_section_1_commombox contact_bottomline">
                                    <div class="col-xs-12 col-sm-12">
                                        <span class="contact_labeltext">@Resource.Text("Tel")</span><span class="hidden-xs">:</span> <a class="footer_conect contact_rCnt" href="tel:28382311">2838 2311</a>
                                    </div>
                                </div>
                                <hr class="contact_line visible-xs" />
                                <div class="row contact_section_1_text contact_section_1_bottombox">
                                    <div class="col-xs-12 col-sm-12">
                                        <span class="contact_labeltext">@Resource.Text("Site")</span><span class="hidden-xs">:</span> <a class="footer_conect contact_rCnt" href="@Resource.Text("hcvURL")" target="_blank" rel="noopener noreferrer">http://www.hcv.gov.hk</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-xs-12 col-sm-6">
            <div class="container-fluid contact_2box">
                <div class="row contact_section_1_topbox contact_topbox">
                    <div class="col-xs-12 col-sm-12">
                        
                             <div class="media">
                                <div class="media-left  media-middle visible-xs">
                                    <img class="media-object contact_titleimg" src="../../Image/SPS/icon_VSS.png" alt="icon_VSS">
                                </div>
                                <div class="media-body media-middle contact_section_1_toptitle contact_titletext">
                                    <div class="center-vertical">
                                        <div class="contact_header">
                                            @If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower = Common.Component.CultureLanguage.English Then
                                                @Html.Raw(Resource.Parameter("ContactUsOfficeName_VSS_EN"))
                                            Else
                                                @Html.Raw(Resource.Parameter("ContactUsOfficeName_VSS_CHI"))
                                            End If
                                        </div>
                                    </div>
                                </div>
                            </div>
                       
                    </div>
                </div>

                <div class="row contact_section section_2_cntbox">
                    <div class="col-xs-12 col-sm-12 transLayer">
                        <div class="row contact_section_1_addressbox contact_middlebox">
                            <div class="col-xs-12 col-sm-12 contact_section_1_text">
                                <strong class="visible-xs contact_labeltext">@Resource.Text("Address")<br /></strong>
                                <div class="addresstext">
                                    <div style="min-height:65px">
                                        @If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower = Common.Component.CultureLanguage.English Then
                                            @Html.Raw(Resource.Parameter("ContactUsOfficeAddress_VSS_EN"))
                                        Else
                                            @Html.Raw(Resource.Parameter("ContactUsOfficeAddress_VSS_CHI"))
                                        End If
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="hidden-xs" style="padding-bottom:10px">
                            <table>
                                <tr>
                                    <th class="contact_section_1_text" style="min-height:30px;font-weight:normal"><span class="contact_labeltext">@Resource.Text("Email_ShotForm"):&nbsp;</span></th>
                                    <td><a class="footer_conect" href="mailto:vacs@dh.gov.hk">vacs@dh.gov.hk</a></td>
                                </tr>
                                <tr>
                                    <td class="contact_section_1_text" style="min-height:30px"><span class="contact_labeltext">@Resource.Text("Tel"):&nbsp;</span></td>
                                    <td><a class="footer_conect" href="tel:21252125">2125 2125</a></td>
                                </tr>
                                <tr>
                                    <td class="contact_section_1_text" style="min-height:30px"><span class="contact_labeltext">@Resource.Text("Site"):&nbsp;</span></td>
                                    <td><a class="footer_conect" href="@Resource.Text("chpURL")" target="_blank" rel="noopener noreferrer">http://www.chp.gov.hk</a></td>
                                </tr>
                            </table>
                        </div>
                        <div class="row contact_btbox visible-xs">
                            <div class="col-xs-12 col-sm-12">
                                <div class="row contact_section_1_text contact_section_1_commombox contact_bottomline">
                                    <div class="col-xs-12 col-sm-12">
                                        <span class="contact_labeltext">@Resource.Text("Email_ShotForm")</span><span class="hidden-xs">:</span> <a class="footer_conect contact_rCnt" href="mailto:vacs@dh.gov.hk">vacs@dh.gov.hk</a>
                                    </div>
                                </div>
                                <hr class="contact_line visible-xs" />
                                <div class="row contact_section_1_text contact_section_1_commombox contact_bottomline">
                                    <div class="col-xs-12 col-sm-12">
                                        <span class="contact_labeltext">@Resource.Text("Tel")</span><span class="hidden-xs">:</span> <a class="footer_conect contact_rCnt" href="tel:21252125">2125 2125</a>
                                    </div>
                                </div>
                                <hr class="contact_line visible-xs" />
                                <div class="row contact_section_1_text contact_section_1_bottombox">
                                    <div class="col-xs-12 col-sm-12">
                                        <span class="contact_labeltext">@Resource.Text("Site")</span><span class="hidden-xs">:</span> <a class="footer_conect contact_rCnt" href="@Resource.Text("chpURL")" target="_blank" rel="noopener noreferrer">http://www.chp.gov.hk</a>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </div>
</div>