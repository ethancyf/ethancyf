Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Format
Imports HCSP.BLL
Imports System.Web.Security.AntiXss


Partial Public Class ucClaimSearch
    Inherits System.Web.UI.UserControl

#Region "Field"

    Private _strDocType As String
    Private _strDocumentIdentityNo As String
    Private _strDOB As String
    Private _strDocumentIdentityNoPrefix As String

    Private _strECAge As String
    Private _strECDOADay As String
    Private _strECDOAMonth As String
    Private _strECDOAYear As String
    Private _blnECDOBSelected As Boolean
    Private _blnShowTips As Boolean
    Private _blnSchemeSelected As Boolean = False
    Private _blnEnabledHKICSymbol As Boolean = False
    Private _blnDisplayHKICSymbol As Boolean = False
    Private _strHKICSymbol As String = String.Empty
    Private _strSchemeCode As String = String.Empty
    Private _blnIDEASComboClientInstalled As Boolean = False
    Private _blnIDEASComboForceToUse As Boolean = False
    Private _blnForceReadSmartID As Boolean = False

#End Region

#Region "Class"

    Public Class ViewStateName
        Public Const DocType As String = "UCCLAIMSEARCH_DOCTYPE"
    End Class

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Enum SearchHKICOption
        ManualInput
        ReadOldSmartIC
        ReadNewSmartIC
        ReadNewSmartICCombo
    End Enum
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

#End Region

#Region "Public Event"

    Public Event SearchButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event ReadSmartIDButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event CancelButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event HKICSymbolListClick(ByVal sender As System.Object, ByVal e As EventArgs)

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Event HKICSymbolHelpClick(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event ReadOldSmartIDButtonClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Event ReadNewSmartIDComboButtonClick(ByVal sender As System.Object, ByVal e As EventArgs)
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' INT20-0021 (Add auditlog for click UpdateNow & Fix GetEHSVaccine web service ) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Event UpdateNowLinkButtonClick(ByVal sender As System.Object, ByVal e As EventArgs)
    Public Event HereLinkButtonClick(ByVal sender As System.Object, ByVal e As EventArgs)
    ' INT20-0021 (Add auditlog for click UpdateNow & Fix GetEHSVaccine web service ) [End][Chris YIM]	

#End Region

    Public Sub RenderLanguage(ByVal strDocCode As String)
        Dim strlanguage As String = Session("language").ToString()
        Me.DocType = strDocCode

        Me.lblSearchShortDOB.Text = Me.GetGlobalResourceObject("Text", "DOBLong")

        Me.SetupShortIdentityNoSearch(True)

        Dim udtDocTypeList As DocTypeModelCollection = (New DocTypeBLL).getAllDocType

        'invisible tips 
        Me.lblSearchShortIdentityNoTips.Visible = False
        Me.lblSearchShortDOBTips.Visible = False

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblSearchHKICNoText.Text = udtDocTypeList.Filter(DocTypeCode.HKIC).DocIdentityDesc(strlanguage)
                'Me.tdSmartIDDeclaration.Attributes.Remove("Style")
                'Me.tdSmartIDDeclaration.Attributes.Add("Style", "padding-bottom: 2px; width: 200px;")
                'Me.lblHKICOR.Text = Me.GetGlobalResourceObject("Text", "ORUpper")

            Case DocTypeModel.DocTypeCode.EC
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblECHKIDText.Text = udtDocTypeList.Filter(DocTypeCode.EC).DocIdentityDesc(strlanguage)

                'Me.lblHKIDECHint.Text = Me.GetGlobalResourceObject("Text", "HKICHint")
                Me.lblECDOBText.Text = Me.GetGlobalResourceObject("Text", "DOBYOB")
                Me.lblECDOBOrText.Text = Me.GetGlobalResourceObject("Text", "Or")

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                'If strlanguage.ToUpper.Equals(Common.Component.CultureLanguage.TradChinese.ToUpper()) Then
                '    Me.ECDOARenderLanguage(False)
                'Else
                '    Me.ECDOARenderLanguage(True)
                'End If

                Me.ECDOARenderLanguage(strlanguage)
                'CRE13-019-02 Extend HCVS to China [End][Winnie]

            Case DocTypeModel.DocTypeCode.HKBC

                ' Get the Doc Identity Desc from table "DocType"
                Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeCode.HKBC).DocIdentityDesc(strlanguage)

                If Me._blnShowTips Then
                    Me.lblSearchShortIdentityNoTips.Visible = True
                    Me.lblSearchShortDOBTips.Visible = True

                    Me.lblSearchShortIdentityNoTips.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
                    Me.lblSearchShortDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintHKBC")
                End If

            Case DocTypeModel.DocTypeCode.REPMT
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeCode.REPMT).DocIdentityDesc(strlanguage)

                If Me._blnShowTips Then
                    Me.lblSearchShortIdentityNoTips.Visible = True
                    Me.lblSearchShortDOBTips.Visible = True

                    Me.lblSearchShortDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintREPMT")
                    Me.lblSearchShortIdentityNoTips.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
                End If

            Case DocTypeModel.DocTypeCode.VISA
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeCode.VISA).DocIdentityDesc(strlanguage)

                If Me._blnShowTips Then
                    Me.lblSearchShortIdentityNoTips.Visible = True
                    Me.lblSearchShortDOBTips.Visible = True

                    Me.lblSearchShortDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintVISA")
                    Me.lblSearchShortIdentityNoTips.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
                End If

            Case DocTypeModel.DocTypeCode.ID235B
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeCode.ID235B).DocIdentityDesc(strlanguage)

                If Me._blnShowTips Then
                    Me.lblSearchShortIdentityNoTips.Visible = True
                    Me.lblSearchShortDOBTips.Visible = True

                    Me.lblSearchShortIdentityNoTips.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
                    Me.lblSearchShortDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintID235B")
                End If

            Case DocTypeModel.DocTypeCode.ADOPC
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblADOPCIdentityNoText.Text = udtDocTypeList.Filter(DocTypeCode.ADOPC).DocIdentityDesc(strlanguage)

                If Me._blnShowTips Then
                    Me.lblADOPCIdentityNoTips.Visible = True
                    Me.lblADOPCDOBTips.Visible = True

                    Me.lblADOPCIdentityNoTips.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
                    Me.lblADOPCDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintADOPC")
                Else
                    Me.lblADOPCIdentityNoTips.Visible = False
                    Me.lblADOPCDOBTips.Visible = False
                End If

            Case DocTypeModel.DocTypeCode.DI
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeCode.DI).DocIdentityDesc(strlanguage)

                If Me._blnShowTips Then
                    Me.lblSearchShortIdentityNoTips.Visible = True
                    Me.lblSearchShortDOBTips.Visible = True

                    Me.lblSearchShortIdentityNoTips.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
                    Me.lblSearchShortDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintDI")
                End If

            Case DocTypeModel.DocTypeCode.RFNo8
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeCode.RFNo8).DocIdentityDesc(strlanguage)

            Case DocTypeModel.DocTypeCode.OW
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeCode.OW).DocIdentityDesc(strlanguage)

                ' CRE20-0022 (Immu record) [Start][Martin]

            Case DocTypeModel.DocTypeCode.TW
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeCode.TW).DocIdentityDesc(strlanguage)
                If Me._blnShowTips Then
                    Me.lblSearchShortIdentityNoTips.Visible = True
                    Me.lblSearchShortDOBTips.Visible = True

                    Me.lblSearchShortIdentityNoTips.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
                    Me.lblSearchShortDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintDI")
                End If

            Case DocTypeModel.DocTypeCode.CCIC
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeCode.CCIC).DocIdentityDesc(strlanguage)
                If Me._blnShowTips Then
                    Me.lblSearchShortIdentityNoTips.Visible = True
                    Me.lblSearchShortDOBTips.Visible = True

                    Me.lblSearchShortIdentityNoTips.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
                    Me.lblSearchShortDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintDI")
                End If

            Case DocTypeModel.DocTypeCode.ROP140
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeCode.ROP140).DocIdentityDesc(strlanguage)
                If Me._blnShowTips Then
                    Me.lblSearchShortIdentityNoTips.Visible = True
                    Me.lblSearchShortDOBTips.Visible = True

                    Me.lblSearchShortIdentityNoTips.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
                    Me.lblSearchShortDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintDI")
                End If

            Case DocTypeModel.DocTypeCode.PASS
                ' Get the Doc Identity Desc from table "DocType"
                Me.lblSearchShortIdentityNo.Text = udtDocTypeList.Filter(DocTypeCode.PASS).DocIdentityDesc(strlanguage)
                If Me._blnShowTips Then
                    Me.lblSearchShortIdentityNoTips.Visible = True
                    Me.lblSearchShortDOBTips.Visible = True

                    Me.lblSearchShortIdentityNoTips.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")
                    Me.lblSearchShortDOBTips.Text = Me.GetGlobalResourceObject("Text", "DOBHintDI") '
                End If
                ' CRE20-0022 (Immu record) [End][Martin]

            Case String.Empty
                Me.lblSearchShortIdentityNo.Text = Me.GetGlobalResourceObject("Text", "IdentityDocNo")

                Me.SetupShortIdentityNoSearch(False)

        End Select

    End Sub

    Private Sub SetupShortIdentityNoSearch(ByVal enable As Boolean)
        Me.txtSearchShortIdentityNo.Enabled = enable
        Me.txtSearchShortDOB.Enabled = enable
        Me.btnShortIdentityNoSearch.Enabled = enable

        If enable Then
            Me.txtSearchShortIdentityNo.BackColor = Drawing.Color.White
            Me.lblSearchShortIdentityNo.ForeColor = System.Drawing.ColorTranslator.FromHtml("#666666")

            Me.txtSearchShortDOB.BackColor = Drawing.Color.White
            Me.lblSearchShortDOB.ForeColor = System.Drawing.ColorTranslator.FromHtml("#666666")

            Me.btnShortIdentityNoSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtn")
        Else
            Me.txtSearchShortIdentityNo.BackColor = Drawing.Color.Silver
            Me.lblSearchShortIdentityNo.ForeColor = Drawing.Color.LightGray

            Me.txtSearchShortDOB.BackColor = Drawing.Color.Silver
            Me.lblSearchShortDOB.ForeColor = Drawing.Color.LightGray

            Me.btnShortIdentityNoSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtnDisabled")

            Me.txtSearchShortIdentityNo.Text = String.Empty
            Me.txtSearchShortDOB.Text = String.Empty
        End If

    End Sub

    Public Sub CleanField()

        ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Me.rblHKICSymbol.Items.Clear()
        Me.rblHKICSymbol.SelectedIndex = -1
        Me.rblHKICSymbol.SelectedValue = Nothing
        ' CRE17-010 (OCSSS integration) [End][Chris YIM]
        Me.txtSearchHKICNo.Text = String.Empty
        Me.txtSearchHKICDOB.Text = String.Empty

        Me.txtSearchShortIdentityNo.Text = String.Empty
        Me.txtSearchShortDOB.Text = String.Empty

        Me.txtADOPCIdentityNo.Text = String.Empty
        Me.txtADOPCIdentityNoPrefix.Text = String.Empty
        Me.txtADOPCDOB.Text = String.Empty

        Me.txtECDOAAge.Text = String.Empty
        Me.txtECDOADayChi.Text = String.Empty
        Me.txtECDOADayEn.Text = String.Empty
        Me.txtECDOAYearChi.Text = String.Empty
        Me.txtECDOAYearEn.Text = String.Empty
        Me.txtECDOB.Text = String.Empty
        Me.txtECHKID.Text = String.Empty

        Me.rbECDOB.Checked = True
        Me.rbECDOA.Checked = False
        Me.ddlECDOAMonth.SelectedIndex = -1

        Me.SetHKICError(False)
        Me.SetECError(False)
        Me.SetADOPCError(False)
        Me.SetSearchShortError(False)
    End Sub

    Public Sub Build(ByVal strDocCode As String, Optional ByVal udtEHSPersonalInfo As EHSPersonalInformationModel = Nothing)
        ' Init
        panSearchHKIC.Visible = False
        panSearchEC.Visible = False
        panSearchShortNo.Visible = False
        panSearchADOPC.Visible = False
        EnableSmartIDButton(False)

        Dim udtFormatter As New Formatter

        Me.RenderLanguage(strDocCode)

        Select Case strDocCode
            Case DocTypeModel.DocTypeCode.HKIC
                Me.panSearchHKIC.Visible = True

                Me.txtSearchHKICNo.Width = 85
                Me.txtSearchHKICNo.MaxLength = 11

                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                EnableHKICSymbolRadioButtonList(_blnEnabledHKICSymbol)

                If Not IsNothing(udtEHSPersonalInfo) Then
                    'Change manual input header
                    lblManualInput.Text = Me.GetGlobalResourceObject("Text", "SearchAccountHeader")

                    'Show "Cancel" button
                    ibtnSearchHKICCancel.Visible = True

                    'Not to show tip for download latest SmartIC software
                    tblDownloadComboClient.Visible = False

                    'Show old and new HKIC sample only
                    tdSearchNewSmartICButton.Visible = False
                    tdSearchOldSmartICButton.Visible = False
                    mvOldHKIC.SetActiveView(vOldHKICSample)
                    mvNewHKIC.SetActiveView(vNewHKICSample)

                    'Assign Value and disable it 
                    txtSearchHKICNo.Enabled = False
                    txtSearchHKICDOB.Enabled = False
                    txtSearchHKICNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeModel.DocTypeCode.HKIC, udtEHSPersonalInfo.IdentityNum, False)
                    txtSearchHKICDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, CultureLanguage.English, Nothing, Nothing)

                    'Manual Input Style
                    tblManual.Visible = True
                    tdManual.Style.Add("width", "305px")
                    tblNewIDEAS.Style.Add("position", "relative")
                    tblNewIDEAS.Style.Add("left", "0px")

                Else
                    txtSearchHKICNo.Enabled = True
                    txtSearchHKICDOB.Enabled = True
                    ibtnSearchHKICCancel.Visible = False
                    lblManualInput.Text = Me.GetGlobalResourceObject("Text", "ManualInput")

                    tdSearchNewSmartICButton.Visible = True
                    tdSearchOldSmartICButton.Visible = True

                    tblDownloadComboClient.Visible = False
                    divSmartIDSoftwareNotInstalled.Visible = False

                    If SmartIDHandler.EnableSmartID AndAlso strDocCode.Equals(DocTypeModel.DocTypeCode.HKIC) Then
                        ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        If _blnIDEASComboForceToUse Then
                            'Combo Only Period

                            mvIDEASCombo.SetActiveView(vNewIDEAS)

                            Me.btnShortIdentityNoNewSmartIDCombo.Visible = True
                            Me.btnShortIdentityNoNewSmartIDCombo.Enabled = True

                        Else
                            'Transition Period
                            If _blnIDEASComboClientInstalled Then
                                mvIDEASCombo.SetActiveView(vNewIDEAS)

                                Me.btnShortIdentityNoNewSmartIDCombo.Visible = True
                                Me.btnShortIdentityNoNewSmartIDCombo.Enabled = True

                            Else
                                mvIDEASCombo.SetActiveView(vOldIDEAS)

                                tblDownloadComboClient.Visible = True

                                Me.btnShortIdentityNoNewSmartID.Visible = True
                                Me.btnShortIdentityNoNewSmartID.Enabled = True

                                Me.btnShortIdentityNoOldSmartID.Visible = True
                                Me.btnShortIdentityNoOldSmartID.Enabled = True

                                mvOldHKIC.SetActiveView(vOldHKICSearch)
                                mvNewHKIC.SetActiveView(vNewHKICSearch)

                            End If
                        End If

                        EnableSmartIDButton(True)

                        If _blnEnabledHKICSymbol And _blnDisplayHKICSymbol Then
                            If rblHKICSymbol.SelectedValue = String.Empty Then

                                If btnShortIdentityNoOldSmartID.Visible Then
                                    btnShortIdentityNoOldSmartID.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchDisableBtn")
                                    btnShortIdentityNoOldSmartID.Enabled = False

                                    EnableHKICSearchOption(False, SearchHKICOption.ReadOldSmartIC)
                                End If

                                If btnShortIdentityNoNewSmartID.Visible Then
                                    btnShortIdentityNoNewSmartID.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchDisableBtn")
                                    btnShortIdentityNoNewSmartID.Enabled = False

                                    EnableHKICSearchOption(False, SearchHKICOption.ReadNewSmartIC)
                                End If

                                If btnShortIdentityNoNewSmartIDCombo.Visible Then
                                    btnShortIdentityNoNewSmartIDCombo.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchDisableBtn")
                                    btnShortIdentityNoNewSmartIDCombo.Enabled = False

                                    If _blnIDEASComboForceToUse And Not _blnIDEASComboClientInstalled Then
                                        divSmartIDSoftwareNotInstalled.Visible = True
                                    End If

                                    EnableHKICSearchOption(False, SearchHKICOption.ReadNewSmartICCombo)
                                End If

                            Else

                                If btnShortIdentityNoOldSmartID.Visible Then
                                    btnShortIdentityNoOldSmartID.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchBtn")
                                    btnShortIdentityNoOldSmartID.Enabled = True

                                    EnableHKICSearchOption(True, SearchHKICOption.ReadOldSmartIC)
                                End If

                                If btnShortIdentityNoNewSmartID.Visible Then
                                    btnShortIdentityNoNewSmartID.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchBtn")
                                    btnShortIdentityNoNewSmartID.Enabled = True

                                    EnableHKICSearchOption(True, SearchHKICOption.ReadNewSmartIC)
                                End If

                                If btnShortIdentityNoNewSmartIDCombo.Visible Then
                                    If _blnIDEASComboForceToUse Then
                                        If _blnIDEASComboClientInstalled Then
                                            btnShortIdentityNoNewSmartIDCombo.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchBtn")
                                            btnShortIdentityNoNewSmartIDCombo.Enabled = True
                                        Else
                                            btnShortIdentityNoNewSmartIDCombo.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchDisableBtn")
                                            btnShortIdentityNoNewSmartIDCombo.Enabled = False
                                            divSmartIDSoftwareNotInstalled.Visible = True
                                        End If
                                    Else
                                        btnShortIdentityNoNewSmartIDCombo.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchBtn")
                                        btnShortIdentityNoNewSmartIDCombo.Enabled = True
                                    End If

                                    EnableHKICSearchOption(True, SearchHKICOption.ReadNewSmartICCombo)
                                End If

                            End If
                            ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
                        End If
                        ' CRE19-028 (IDEAS Combo) [End][Chris YIM

                        If _blnForceReadSmartID Then
                            tblManual.Visible = False
                            tdManual.Style.Add("width", "0px")
                            tblNewIDEAS.Style.Add("position", "relative")
                            tblNewIDEAS.Style.Add("left", "-13px")
                        Else
                            tblManual.Visible = True
                            tdManual.Style.Add("width", "305px")
                            tblNewIDEAS.Style.Add("position", "relative")
                            tblNewIDEAS.Style.Add("left", "0px")
                        End If

                    Else
                        ' CRE20-0022 (Immu record) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        mvIDEASCombo.SetActiveView(vOldIDEAS)

                        mvOldHKIC.SetActiveView(vOldHKICSample)
                        mvNewHKIC.SetActiveView(vNewHKICSample)
                        ' CRE20-0022 (Immu record) [End][Chris YIM]

                    End If

                End If
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]

                Me.filteredSearchHKICNo.ValidChars = "()"
                Me.txtSearchHKICNo.Attributes("onChange") = "javascript:formatHKID(this);"

            Case DocTypeModel.DocTypeCode.HKBC, DocTypeModel.DocTypeCode.CCIC, DocTypeModel.DocTypeCode.ROP140
                Me.panSearchShortNo.Visible = True

                Me.txtSearchShortIdentityNo.Width = 85
                Me.txtSearchShortIdentityNo.MaxLength = 11

                If Not IsNothing(udtEHSPersonalInfo) Then
                    txtSearchShortIdentityNo.Enabled = False
                    txtSearchShortDOB.Enabled = False
                    btnShortIdentityNoCancel.Visible = True

                    txtSearchShortIdentityNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(strDocCode, udtEHSPersonalInfo.IdentityNum, False)
                    txtSearchShortDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, CultureLanguage.English, Nothing, Nothing)

                Else
                    txtSearchShortIdentityNo.Enabled = True
                    txtSearchShortDOB.Enabled = True
                    btnShortIdentityNoCancel.Visible = False

                End If

                Me.filteredSearchShortIdentityNo.ValidChars = "()"
                Me.txtSearchShortIdentityNo.Attributes("onChange") = "javascript:formatHKID(this);"

            Case DocTypeModel.DocTypeCode.EC
                Me.panSearchEC.Visible = True

                If Not IsNothing(udtEHSPersonalInfo) Then
                    txtECHKID.Enabled = False
                    rbECDOB.Enabled = False
                    rbECDOA.Enabled = False
                    btnECCancel.Visible = True

                    txtECHKID.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeModel.DocTypeCode.EC, udtEHSPersonalInfo.IdentityNum, False)

                    If udtEHSPersonalInfo.ExactDOB = "A" Then
                        ' Age 99 On DD MMM YYYY
                        rbECDOB.Checked = False
                        rbECDOA.Checked = True
                        SwitchECSearchControl(False)

                        txtECDOAAge.Text = udtEHSPersonalInfo.ECAge

                        Dim dtmDOR As Date = udtEHSPersonalInfo.ECDateOfRegistration
                        txtECDOADayEn.Text = dtmDOR.Day
                        txtECDOADayChi.Text = dtmDOR.Day
                        ddlECDOAMonth.SelectedValue = dtmDOR.Month.ToString.PadLeft(2, "0")
                        txtECDOAYearEn.Text = dtmDOR.Year
                        txtECDOAYearChi.Text = dtmDOR.Year

                        txtECDOAAge.Enabled = False
                        txtECDOADayEn.Enabled = False
                        txtECDOADayChi.Enabled = False
                        ddlECDOAMonth.Enabled = False
                        txtECDOAYearEn.Enabled = False
                        txtECDOAYearChi.Enabled = False

                    Else
                        ' DOB
                        rbECDOB.Checked = True
                        rbECDOA.Checked = False
                        SwitchECSearchControl(True)

                        txtECDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, CultureLanguage.English, Nothing, Nothing)
                        txtECDOB.Enabled = False

                    End If

                Else
                    txtECHKID.Enabled = True
                    rbECDOB.Enabled = True
                    rbECDOA.Enabled = True
                    btnECCancel.Visible = False

                    Me.SwitchECSearchControl(Me.rbECDOB.Checked)

                End If

            Case DocTypeModel.DocTypeCode.REPMT, DocTypeModel.DocTypeCode.DI
                Me.panSearchShortNo.Visible = True

                Me.txtSearchShortIdentityNo.Width = 85
                Me.txtSearchShortIdentityNo.MaxLength = 9

                If Not IsNothing(udtEHSPersonalInfo) Then
                    txtSearchShortIdentityNo.Enabled = False
                    txtSearchShortDOB.Enabled = False
                    btnShortIdentityNoCancel.Visible = True

                    txtSearchShortIdentityNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, False)
                    txtSearchShortDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, CultureLanguage.English, Nothing, Nothing)

                Else
                    txtSearchShortIdentityNo.Enabled = True
                    txtSearchShortDOB.Enabled = True
                    btnShortIdentityNoCancel.Visible = False

                End If

                Me.filteredSearchShortIdentityNo.ValidChars = ""

                Me.txtSearchShortIdentityNo.Attributes("onChange") = "javascript:UpperIndentityNo(this);"

            Case DocTypeModel.DocTypeCode.OW
                Me.panSearchShortNo.Visible = True

                Me.txtSearchShortIdentityNo.Width = 150
                Me.txtSearchShortIdentityNo.MaxLength = 20

                If Not IsNothing(udtEHSPersonalInfo) Then
                    txtSearchShortIdentityNo.Enabled = False
                    txtSearchShortDOB.Enabled = False
                    btnShortIdentityNoCancel.Visible = True

                    txtSearchShortIdentityNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, False)
                    txtSearchShortDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, CultureLanguage.English, Nothing, Nothing)

                Else
                    txtSearchShortIdentityNo.Enabled = True
                    txtSearchShortDOB.Enabled = True
                    btnShortIdentityNoCancel.Visible = False

                End If

                Me.filteredSearchShortIdentityNo.ValidChars = "-()"

                Me.txtSearchShortIdentityNo.Attributes("onChange") = "javascript:UpperIndentityNo(this);"

                ' CRE20-0023 (Immu record) [Start][Martin]
            Case DocTypeModel.DocTypeCode.TW
                Me.panSearchShortNo.Visible = True

                Me.txtSearchShortIdentityNo.Width = 150
                Me.txtSearchShortIdentityNo.MaxLength = 20

                If Not IsNothing(udtEHSPersonalInfo) Then
                    txtSearchShortIdentityNo.Enabled = False
                    txtSearchShortDOB.Enabled = False
                    btnShortIdentityNoCancel.Visible = True

                    txtSearchShortIdentityNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, False)
                    txtSearchShortDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, CultureLanguage.English, Nothing, Nothing)

                Else
                    txtSearchShortIdentityNo.Enabled = True
                    txtSearchShortDOB.Enabled = True
                    btnShortIdentityNoCancel.Visible = False

                End If

                Me.filteredSearchShortIdentityNo.ValidChars = ""
                Me.txtSearchShortIdentityNo.Attributes("onChange") = "javascript:UpperIndentityNo(this);"
                ' CRE20-0023 (Immu record) [Start][Martin]

            Case DocTypeModel.DocTypeCode.ID235B
                Me.panSearchShortNo.Visible = True

                Me.txtSearchShortIdentityNo.Width = 85
                Me.txtSearchShortIdentityNo.MaxLength = 8

                If Not IsNothing(udtEHSPersonalInfo) Then
                    txtSearchShortIdentityNo.Enabled = False
                    txtSearchShortDOB.Enabled = False
                    btnShortIdentityNoCancel.Visible = True

                    txtSearchShortIdentityNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeModel.DocTypeCode.ID235B, udtEHSPersonalInfo.IdentityNum, False)
                    txtSearchShortDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, CultureLanguage.English, Nothing, Nothing)

                Else
                    txtSearchShortIdentityNo.Enabled = True
                    txtSearchShortDOB.Enabled = True
                    btnShortIdentityNoCancel.Visible = False

                End If

                Me.filteredSearchShortIdentityNo.ValidChars = ""
                Me.txtSearchShortIdentityNo.Attributes("onChange") = "javascript:UpperIndentityNo(this);"

            Case DocTypeModel.DocTypeCode.VISA
                Me.panSearchShortNo.Visible = True

                Me.txtSearchShortIdentityNo.Width = 150
                Me.txtSearchShortIdentityNo.MaxLength = 18

                If Not IsNothing(udtEHSPersonalInfo) Then
                    txtSearchShortIdentityNo.Enabled = False
                    txtSearchShortDOB.Enabled = False
                    btnShortIdentityNoCancel.Visible = True

                    txtSearchShortIdentityNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeModel.DocTypeCode.VISA, udtEHSPersonalInfo.IdentityNum, False)
                    txtSearchShortDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, CultureLanguage.English, Nothing, Nothing)

                Else
                    txtSearchShortIdentityNo.Enabled = True
                    txtSearchShortDOB.Enabled = True
                    btnShortIdentityNoCancel.Visible = False

                End If

                Me.filteredSearchShortIdentityNo.ValidChars = "-()"
                Me.txtSearchShortIdentityNo.Attributes("onChange") = "javascript:formatVISA(this);"

            Case DocTypeModel.DocTypeCode.ADOPC
                panSearchADOPC.Visible = True

                If Not IsNothing(udtEHSPersonalInfo) Then
                    txtADOPCIdentityNoPrefix.Enabled = False
                    txtADOPCIdentityNo.Enabled = False
                    txtADOPCDOB.Enabled = False
                    btnADOPCSearchCancel.Visible = True

                    txtADOPCIdentityNoPrefix.Text = udtEHSPersonalInfo.AdoptionPrefixNum
                    txtADOPCIdentityNo.Text = udtEHSPersonalInfo.IdentityNum
                    txtADOPCDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, CultureLanguage.English, Nothing, Nothing)

                Else
                    txtADOPCIdentityNoPrefix.Enabled = True
                    txtADOPCIdentityNo.Enabled = True
                    txtADOPCDOB.Enabled = True
                    btnADOPCSearchCancel.Visible = False

                End If

                Me.txtADOPCIdentityNoPrefix.Attributes("onChange") = "javascript:UpperIndentityNo(this);"

            Case DocTypeModel.DocTypeCode.RFNo8
                Me.panSearchShortNo.Visible = True

                Me.txtSearchShortIdentityNo.Width = 85
                Me.txtSearchShortIdentityNo.MaxLength = 9

                If Not IsNothing(udtEHSPersonalInfo) Then
                    txtSearchShortIdentityNo.Enabled = False
                    txtSearchShortDOB.Enabled = False
                    btnShortIdentityNoCancel.Visible = True

                    txtSearchShortIdentityNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, False)
                    txtSearchShortDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, CultureLanguage.English, Nothing, Nothing)

                Else
                    txtSearchShortIdentityNo.Enabled = True
                    txtSearchShortDOB.Enabled = True
                    btnShortIdentityNoCancel.Visible = False

                End If

                Me.filteredSearchShortIdentityNo.ValidChars = ""
                Me.txtSearchShortIdentityNo.Attributes("onChange") = "javascript:UpperIndentityNo(this);"

                ' CRE20-0022 (Immu record) [Start][Martin]
            Case DocTypeModel.DocTypeCode.PASS
                Me.panSearchShortNo.Visible = True

                Me.txtSearchShortIdentityNo.Width = 150
                Me.txtSearchShortIdentityNo.MaxLength = 20

                If Not IsNothing(udtEHSPersonalInfo) Then
                    txtSearchShortIdentityNo.Enabled = False
                    txtSearchShortDOB.Enabled = False
                    btnShortIdentityNoCancel.Visible = True

                    txtSearchShortIdentityNo.Text = udtFormatter.FormatDocIdentityNoForDisplay(DocTypeModel.DocTypeCode.PASS, udtEHSPersonalInfo.IdentityNum, False)
                    txtSearchShortDOB.Text = udtFormatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, CultureLanguage.English, Nothing, Nothing)

                Else
                    txtSearchShortIdentityNo.Enabled = True
                    txtSearchShortDOB.Enabled = True
                    btnShortIdentityNoCancel.Visible = False

                End If

                Me.filteredSearchShortIdentityNo.ValidChars = "-()"
                Me.txtSearchShortIdentityNo.Attributes("onChange") = "javascript:UpperIndentityNo(this);"
                ' CRE20-0022 (Immu record) [End][Martin]
            Case String.Empty
                ibtnSearchHKICCancel.Visible = False
                btnADOPCSearchCancel.Visible = False
                btnECCancel.Visible = False
                btnShortIdentityNoCancel.Visible = False

                panSearchShortNo.Visible = True

        End Select

        EnableSearchButton()

    End Sub

    Private Sub EnableSearchButton()
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Dim blnEnableInput As Boolean = False
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        If _blnSchemeSelected = False Then
            btnShortIdentityNoSearch.Enabled = False
            ibtnSearchHKIC.Enabled = False
            btnECSearch.Enabled = False
            btnADOPCSearch.Enabled = False

            btnShortIdentityNoSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtnDisabled")
            ibtnSearchHKIC.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtnDisabled")
            btnECSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtnDisabled")
            btnADOPCSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtnDisabled")

        Else
            btnShortIdentityNoSearch.Enabled = True
            ibtnSearchHKIC.Enabled = True
            btnECSearch.Enabled = True
            btnADOPCSearch.Enabled = True

            btnShortIdentityNoSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtn")
            ibtnSearchHKIC.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtn")
            btnECSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtn")
            btnADOPCSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtn")

            ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            blnEnableInput = True
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

            ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
            ' ----------------------------------------------------------
            If _blnEnabledHKICSymbol And _blnDisplayHKICSymbol Then
                If rblHKICSymbol.SelectedValue = String.Empty Then
                    ibtnSearchHKIC.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtnDisabled")
                    ibtnSearchHKIC.Enabled = False

                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    blnEnableInput = False
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                End If
            End If
            ' CRE17-010 (OCSSS integration) [End][Chris YIM]

        End If

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If CheckFromVaccinationRecordEnquiry() Then
            blnEnableInput = True
        End If

        EnableHKICSearchOption(blnEnableInput, SearchHKICOption.ManualInput)
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

    End Sub

#Region "Set Up Error Image"

    ''HKIC Error-----------------------------------------------------------------------
    'Public Sub SetHKICHKIDError(ByVal isValid As Boolean)
    '    Me.ErrHKICHKID.Visible = isValid
    'End Sub

    'Public Sub SetHKICDOBError(ByVal isValid As Boolean)
    '    Me.ErrHKICDOB.Visible = isValid
    'End Sub

    'EC Error-----------------------------------------------------------------------
    Public Sub SetECError(ByVal visible As Boolean)
        Me.SetECHKIDError(visible)
        Me.SetECDOBError(visible)
        Me.SetECDOAError(visible)
        Me.SetECDOAAgeError(visible)
    End Sub

    Public Sub SetECHKIDError(ByVal visible As Boolean)
        Me.ErrECHKID.Visible = visible
    End Sub

    Public Sub SetECDOBError(ByVal visible As Boolean)
        Me.ErrECDOB.Visible = visible
    End Sub

    Public Sub SetECDOAError(ByVal visible As Boolean)
        Me.ErrECDOA.Visible = visible
    End Sub

    Public Sub SetECDOAAgeError(ByVal visible As Boolean)
        Me.ErrECDOAAge.Visible = visible
    End Sub

    'Search Short document Identity No Error-----------------------------------------------------------------------
    Public Sub SetSearchShortError(ByVal visible As Boolean)
        Me.SetSearchShortIdentityNoError(visible)
        Me.SetSearchShortDOBError(visible)
    End Sub

    Public Sub SetSearchShortIdentityNoError(ByVal visible As Boolean)
        Me.ErrSearchShortIdentityNo.Visible = visible
    End Sub

    Public Sub SetSearchShortDOBError(ByVal visible As Boolean)
        Me.ErrSearchShortDOB.Visible = visible
    End Sub

    'Document Identity Error-----------------------------------------------------------------------
    Public Sub SetADOPCError(ByVal visible As Boolean)
        Me.SetADOPCIdentityNoError(visible)
        Me.SetADOPCDOBError(visible)
    End Sub

    Public Sub SetADOPCIdentityNoError(ByVal visible As Boolean)
        Me.ErrADOPCIdentityNo.Visible = visible
    End Sub

    Public Sub SetADOPCDOBError(ByVal visible As Boolean)
        Me.ErrADOPCDOB.Visible = visible
    End Sub

    'HKIC No Error-----------------------------------------------------------------------
    Public Sub SetHKICError(ByVal visible As Boolean)
        Me.SetHKICNoError(visible)
        Me.SetHKICDOBError(visible)
    End Sub

    Public Sub SetHKICNoError(ByVal visible As Boolean)
        Me.ErrSearchHKICNo.Visible = visible
    End Sub

    Public Sub SetHKICDOBError(ByVal visible As Boolean)
        Me.ErrSearchHKICDOB.Visible = visible
    End Sub

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Sub SetHKICResidentStateError(ByVal visible As Boolean)
        Me.ErrHKICSymbol.Visible = visible
    End Sub
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]
#End Region

#Region "Set Text Box Display Value"
    'Short document Identity No Display Value-----------------------------------------------------------------------
    Public Sub SetSearchShortIdentityNo(ByVal strIdentityNo As String)
        Me.txtSearchShortIdentityNo.Text = strIdentityNo
    End Sub

    Public Sub SetSearchShortDOB(ByVal strDOB As String)
        Me.txtSearchShortDOB.Text = strDOB
    End Sub

    'Long document Identity No Display Value-----------------------------------------------------------------------
    Public Sub SetDIIdentityNo(ByVal strIdentityNoPrefix As String, ByVal strIdentityNo As String)
        Me.txtADOPCIdentityNoPrefix.Text = strIdentityNoPrefix
        Me.txtADOPCIdentityNo.Text = strIdentityNo
    End Sub

    Public Sub SetDIDOB(ByVal strDOB As String)
        Me.txtADOPCDOB.Text = strDOB
    End Sub

    'HKIC Display Value-----------------------------------------------------------------------
    Public Sub SetSearchHKICNo(ByVal strIdentityNo As String)
        Me.txtSearchHKICNo.Text = strIdentityNo
    End Sub

    Public Sub SetSearchHKICDOB(ByVal strDOB As String)
        Me.txtSearchHKICDOB.Text = strDOB
    End Sub

#End Region

#Region "Property"

    Public Sub SetProperty(ByVal documentType As String)
        Select Case documentType
            Case DocTypeModel.DocTypeCode.HKIC

                Me._strDocumentIdentityNo = Me.txtSearchHKICNo.Text
                Me._strDOB = Me.txtSearchHKICDOB.Text

                ' CRE20-0022 (Immu record) [Start][Martin]
            Case DocTypeModel.DocTypeCode.HKBC, _
                 DocTypeModel.DocTypeCode.REPMT, _
                 DocTypeModel.DocTypeCode.ID235B, _
                 DocTypeModel.DocTypeCode.DI, _
                 DocTypeModel.DocTypeCode.OW, _
                 DocTypeModel.DocTypeCode.TW, _
                 DocTypeModel.DocTypeCode.CCIC, _
                 DocTypeModel.DocTypeCode.ROP140
                ' CRE20-0022 (Immu record) [End][Martin]
                Me._strDocumentIdentityNo = Me.txtSearchShortIdentityNo.Text
                Me._strDOB = Me.txtSearchShortDOB.Text

            Case DocTypeModel.DocTypeCode.ADOPC
                Me._strDocumentIdentityNo = Me.txtADOPCIdentityNo.Text
                Me._strDocumentIdentityNoPrefix = Me.txtADOPCIdentityNoPrefix.Text
                Me._strDOB = Me.txtADOPCDOB.Text

            Case DocTypeModel.DocTypeCode.VISA
                Me._strDocumentIdentityNo = Me.txtSearchShortIdentityNo.Text
                Me._strDOB = Me.txtSearchShortDOB.Text

            Case DocTypeModel.DocTypeCode.EC
                Me._strDocumentIdentityNo = Me.txtECHKID.Text
                Me._blnECDOBSelected = Me.rbECDOB.Checked
                Me._strDOB = Me.txtECDOB.Text
                Me._strECAge = Me.txtECDOAAge.Text

                If Session("language").ToString().ToUpper.Equals(CultureLanguage.TradChinese.ToUpper()) OrElse
                    Session("language").ToString().ToUpper.Equals(CultureLanguage.SimpChinese.ToUpper()) Then
                    Me._strECDOADay = Me.txtECDOADayChi.Text
                    Me._strECDOAYear = Me.txtECDOAYearChi.Text
                Else
                    Me._strECDOADay = Me.txtECDOADayEn.Text
                    Me._strECDOAYear = Me.txtECDOAYearEn.Text
                End If

                Me._strECDOAMonth = Me.ddlECDOAMonth.SelectedValue

            Case DocTypeModel.DocTypeCode.RFNo8
                Me._strDocumentIdentityNo = Me.txtSearchShortIdentityNo.Text
                Me._strDOB = Me.txtSearchShortDOB.Text

                ' CRE20-0022 (Immu record) [Start][Martin]
            Case DocTypeModel.DocTypeCode.PASS
                Me._strDocumentIdentityNo = Me.txtSearchShortIdentityNo.Text
                Me._strDOB = Me.txtSearchShortDOB.Text
                ' CRE20-0022 (Immu record) [End][Martin]

        End Select
    End Sub

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Property SchemeCode() As String
        Get
            Return Me._strSchemeCode
        End Get
        Set(value As String)
            _strSchemeCode = value
        End Set
    End Property
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Property UIEnableHKICSymbol() As Boolean
        Get
            Return Me._blnEnabledHKICSymbol
        End Get
        Set(value As Boolean)
            _blnEnabledHKICSymbol = value
        End Set
    End Property
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public ReadOnly Property UIDisplayHKICSymbol() As Boolean
        Get
            Return Me._blnDisplayHKICSymbol
        End Get
    End Property
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public ReadOnly Property HKICSymbolValue() As String
        Get
            Return Me._strHKICSymbol
        End Get
    End Property
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public ReadOnly Property RawIdentityNo() As String
        Get
            Return Me._strDocumentIdentityNo
        End Get
    End Property
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    Public ReadOnly Property IdentityNo() As String
        Get
            Return Me._strDocumentIdentityNo.Replace("(", String.Empty).Replace(")", String.Empty).Replace("-", String.Empty).Replace("/", String.Empty)
        End Get
    End Property

    Public ReadOnly Property IdentityNoPrefix() As String
        Get
            Return Me._strDocumentIdentityNoPrefix
        End Get
    End Property

    Public ReadOnly Property DOB() As String
        Get
            Return Me._strDOB
        End Get
    End Property

    Public ReadOnly Property ECAge() As String
        Get
            Return Me._strECAge
        End Get
    End Property

    Public ReadOnly Property ECDOADay() As String
        Get
            Return Me._strECDOADay
        End Get
    End Property

    Public ReadOnly Property ECDOAMonth() As String
        Get
            Return Me._strECDOAMonth
        End Get
    End Property

    Public ReadOnly Property ECDOAYear() As String
        Get
            Return Me._strECDOAYear
        End Get
    End Property

    Public ReadOnly Property ECDOBSelected() As String
        Get
            Return Me._blnECDOBSelected
        End Get
    End Property

    Public Property DocType() As String
        Get
            Me._strDocType = Me.ViewState(ViewStateName.DocType)
            Return Me._strDocType
        End Get
        Set(ByVal value As String)
            Me.ViewState(ViewStateName.DocType) = value
        End Set
    End Property

    Public Property ShowInputTips() As Boolean
        Get
            Return Me._blnShowTips
        End Get
        Set(ByVal value As Boolean)
            Me._blnShowTips = value
        End Set
    End Property

    Public Property SchemeSelected() As Boolean
        Get
            Return _blnSchemeSelected
        End Get
        Set(ByVal value As Boolean)
            _blnSchemeSelected = value
        End Set
    End Property

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Property IDEASComboClientInstalled() As Boolean
        Get
            Return _blnIDEASComboClientInstalled
        End Get
        Set(ByVal value As Boolean)
            _blnIDEASComboClientInstalled = value
        End Set
    End Property
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Property IDEASComboClientForceToUse() As Boolean
        Get
            Return _blnIDEASComboForceToUse
        End Get
        Set(ByVal value As Boolean)
            _blnIDEASComboForceToUse = value
        End Set
    End Property
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Property ForceReadSmartIDCard() As Boolean
        Get
            Return _blnForceReadSmartID
        End Get
        Set(ByVal value As Boolean)
            _blnForceReadSmartID = value
        End Set
    End Property
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	
#End Region

#Region "Events"

    Protected Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _
        btnECSearch.Click, btnADOPCSearch.Click, btnShortIdentityNoSearch.Click, ibtnSearchHKIC.Click 'btnHKICSearch.Click,
        RaiseEvent SearchButtonClick(sender, e)
    End Sub

    Protected Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles _
        btnECCancel.Click, btnADOPCSearchCancel.Click, btnShortIdentityNoCancel.Click, ibtnSearchHKICCancel.Click
        RaiseEvent CancelButtonClick(sender, e)
    End Sub

    Protected Sub rbECDOB_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rbECDOB.CheckedChanged, rbECDOA.CheckedChanged
        Me.SwitchECSearchControl(Me.rbECDOB.Checked)
    End Sub

    Private Sub btnShortIdentityNoSmartID_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnShortIdentityNoNewSmartID.Click
        RaiseEvent ReadSmartIDButtonClick(sender, e)
    End Sub

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub btnShortIdentityNoOldSmartID_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnShortIdentityNoOldSmartID.Click
        RaiseEvent ReadOldSmartIDButtonClick(sender, e)
    End Sub
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private Sub btnShortIdentityNoNewSmartIDCombo_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnShortIdentityNoNewSmartIDCombo.Click
        RaiseEvent ReadNewSmartIDComboButtonClick(sender, e)
    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub rblHKICSymbol_DataBound(sender As Object, e As EventArgs) Handles rblHKICSymbol.DataBound
        Dim rbl As RadioButtonList = CType(sender, RadioButtonList)

        For idx As Integer = 0 To rbl.Items.Count - 1
            rbl.Items(idx).Value = rbl.Items(idx).Value.Trim
        Next

    End Sub
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Private Sub rblHKICSymbol_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rblHKICSymbol.SelectedIndexChanged
        Me._strHKICSymbol = AntiXssEncoder.HtmlEncode(Me.Request.Form(Me.rblHKICSymbol.UniqueID), True)

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If Not ibtnSearchHKIC.Enabled Or Not btnShortIdentityNoNewSmartID.Enabled Or Not btnShortIdentityNoOldSmartID.Enabled Then
            ' Manual
            ibtnSearchHKIC.Enabled = True
            ibtnSearchHKIC.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SearchBtn")

            EnableHKICSearchOption(True, SearchHKICOption.ManualInput)

            ' Old Smart IC
            btnShortIdentityNoOldSmartID.Enabled = True
            btnShortIdentityNoOldSmartID.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchBtn")

            EnableHKICSearchOption(True, SearchHKICOption.ReadOldSmartIC)

            ' New Smart IC
            btnShortIdentityNoNewSmartID.Enabled = True
            btnShortIdentityNoNewSmartID.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchBtn")

            EnableHKICSearchOption(True, SearchHKICOption.ReadNewSmartIC)
            ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
        End If

        RaiseEvent HKICSymbolListClick(sender, e)
    End Sub
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub ImgBtnHKICSymbolHelpClick(sender As Object, e As EventArgs) Handles ImgBtnHKICSymbolHelp.Click
        RaiseEvent HKICSymbolHelpClick(sender, e)
    End Sub
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
#End Region

#Region "EC Search Setup"
    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    'Rewrite Function
    'Private Sub ECDOARenderLanguage(ByVal blnIsEnglish As Boolean)
    '    Me.rbECDOA.Text = Me.GetGlobalResourceObject("Text", "Age")
    '    Me.lblECDOAOnText.Text = Me.GetGlobalResourceObject("Text", "RegisterOn")
    '    'DOA TextBoxs
    '    Me.txtECDOADayChi.Visible = Not blnIsEnglish
    '    Me.txtECDOAYearChi.Visible = Not blnIsEnglish
    '    Me.txtECDOADayEn.Visible = blnIsEnglish
    '    Me.txtECDOAYearEn.Visible = blnIsEnglish

    '    'DOA Labels
    '    Me.lblECDOAYearChiText.Visible = Not blnIsEnglish
    '    Me.lblECDOAMonthChiText.Visible = Not blnIsEnglish
    '    Me.lblECDOADayChiText.Visible = Not blnIsEnglish

    '    Me.lblECDOAYearEnText.Visible = blnIsEnglish
    '    Me.lblECDOAMonthEnText.Visible = blnIsEnglish
    '    Me.lblECDOADayEnText.Visible = blnIsEnglish

    '    If blnIsEnglish Then
    '        Me.lblECDOAYearEnText.Text = Me.GetGlobalResourceObject("Text", "Year")
    '        Me.lblECDOADayEnText.Text = Me.GetGlobalResourceObject("Text", "Day")
    '        Me.lblECDOAMonthEnText.Text = Me.GetGlobalResourceObject("Text", "Month")

    '        If Me.txtECDOAYearChi.Text <> String.Empty Then
    '            Me.txtECDOAYearEn.Text = Me.txtECDOAYearChi.Text
    '            Me.txtECDOAYearChi.Text = String.Empty
    '        End If

    '        If Me.txtECDOADayChi.Text <> String.Empty Then
    '            Me.txtECDOADayEn.Text = Me.txtECDOADayChi.Text
    '            Me.txtECDOADayChi.Text = String.Empty
    '        End If

    '        Me.BindECDate(Me.ddlECDOAMonth, Common.Component.CultureLanguage.English)
    '    Else
    '        Me.lblECDOAYearChiText.Text = Me.GetGlobalResourceObject("Text", "Year")
    '        Me.lblECDOADayChiText.Text = Me.GetGlobalResourceObject("Text", "Day")
    '        Me.lblECDOAMonthChiText.Text = Me.GetGlobalResourceObject("Text", "Month")

    '        If Me.txtECDOAYearEn.Text <> String.Empty Then
    '            Me.txtECDOAYearChi.Text = Me.txtECDOAYearEn.Text
    '            Me.txtECDOAYearEn.Text = String.Empty
    '        End If

    '        If Me.txtECDOADayEn.Text <> String.Empty Then
    '            Me.txtECDOADayChi.Text = Me.txtECDOADayEn.Text
    '            Me.txtECDOADayEn.Text = String.Empty
    '        End If

    '        Me.BindECDate(Me.ddlECDOAMonth, Common.Component.CultureLanguage.TradChinese)
    '    End If
    'End Sub

    Private Sub ECDOARenderLanguage(ByVal strLanguage As String)

        Dim blnIsEnglish As Boolean

        Me.rbECDOA.Text = Me.GetGlobalResourceObject("Text", "Age")
        Me.lblECDOAOnText.Text = Me.GetGlobalResourceObject("Text", "RegisterOn")


        Select Case strLanguage
            Case CultureLanguage.English
                blnIsEnglish = True

                Me.lblECDOAYearEnText.Text = Me.GetGlobalResourceObject("Text", "Year")
                Me.lblECDOADayEnText.Text = Me.GetGlobalResourceObject("Text", "Day")
                Me.lblECDOAMonthEnText.Text = Me.GetGlobalResourceObject("Text", "Month")

                If Me.txtECDOAYearChi.Text <> String.Empty Then
                    ' I-CRE16-003 Fix XSS [Start][Lawrence]
                    Me.txtECDOAYearEn.Text = AntiXssEncoder.HtmlEncode(txtECDOAYearChi.Text, True)
                    ' I-CRE16-003 Fix XSS [End][Lawrence]
                    Me.txtECDOAYearChi.Text = String.Empty
                End If

                If Me.txtECDOADayChi.Text <> String.Empty Then
                    ' I-CRE16-003 Fix XSS [Start][Lawrence]
                    Me.txtECDOADayEn.Text = AntiXssEncoder.HtmlEncode(txtECDOADayChi.Text, True)
                    ' I-CRE16-003 Fix XSS [End][Lawrence]
                    Me.txtECDOADayChi.Text = String.Empty
                End If
            Case CultureLanguage.TradChinese,
                CultureLanguage.SimpChinese
                blnIsEnglish = False

                Me.lblECDOAYearChiText.Text = Me.GetGlobalResourceObject("Text", "Year")
                Me.lblECDOADayChiText.Text = Me.GetGlobalResourceObject("Text", "Day")
                Me.lblECDOAMonthChiText.Text = Me.GetGlobalResourceObject("Text", "Month")

                If Me.txtECDOAYearEn.Text <> String.Empty Then
                    ' I-CRE16-003 Fix XSS [Start][Lawrence]
                    Me.txtECDOAYearChi.Text = AntiXssEncoder.HtmlEncode(txtECDOAYearEn.Text, True)
                    ' I-CRE16-003 Fix XSS [End][Lawrence]
                    Me.txtECDOAYearEn.Text = String.Empty
                End If

                If Me.txtECDOADayEn.Text <> String.Empty Then
                    ' I-CRE16-003 Fix XSS [Start][Lawrence]
                    Me.txtECDOADayChi.Text = AntiXssEncoder.HtmlEncode(txtECDOADayEn.Text, True)
                    ' I-CRE16-003 Fix XSS [End][Lawrence]
                    Me.txtECDOADayEn.Text = String.Empty
                End If
        End Select

        Me.BindECDate(Me.ddlECDOAMonth, strLanguage)

        'DOA TextBoxs
        Me.txtECDOADayChi.Visible = Not blnIsEnglish
        Me.txtECDOAYearChi.Visible = Not blnIsEnglish
        Me.txtECDOADayEn.Visible = blnIsEnglish
        Me.txtECDOAYearEn.Visible = blnIsEnglish

        'DOA Labels
        Me.lblECDOAYearChiText.Visible = Not blnIsEnglish
        Me.lblECDOAMonthChiText.Visible = Not blnIsEnglish
        Me.lblECDOADayChiText.Visible = Not blnIsEnglish

        Me.lblECDOAYearEnText.Visible = blnIsEnglish
        Me.lblECDOAMonthEnText.Visible = blnIsEnglish
        Me.lblECDOADayEnText.Visible = blnIsEnglish
    End Sub
    'CRE13-019-02 Extend HCVS to China [End][Winnie]

    Private Sub BindECDate(ByVal ddlECDate As DropDownList, ByVal strLanguage As String)
        Dim strECDateMonth As String

        strECDateMonth = ddlECDate.SelectedValue()
        ddlECDate.DataSource = (New GeneralFunction).GetMonthSelection(strLanguage)
        ddlECDate.DataValueField = "Value"
        ddlECDate.DataTextField = "Display"
        ddlECDate.DataBind()

        If Not strECDateMonth Is Nothing Then
            ddlECDate.SelectedValue = strECDateMonth
        End If
    End Sub

    Private Sub SwitchECSearchControl(ByVal blnSearchByDOB As Boolean)
        Me.lblDOBECHint.Visible = blnSearchByDOB
        Me.lblDOBECHint2.Visible = blnSearchByDOB
        Me.txtECDOB.Enabled = blnSearchByDOB

        Me.lblDOAECHint.Visible = Not blnSearchByDOB
        Me.txtECDOAAge.Enabled = Not blnSearchByDOB
        Me.txtECDOADayEn.Enabled = Not blnSearchByDOB
        Me.txtECDOAYearEn.Enabled = Not blnSearchByDOB
        Me.ddlECDOAMonth.Enabled = Not blnSearchByDOB
        Me.txtECDOAYearChi.Enabled = Not blnSearchByDOB
        Me.txtECDOADayChi.Enabled = Not blnSearchByDOB

        Me.lblHKIDECHint.Visible = True
        Me.lblHKIDECHint.Text = Me.GetGlobalResourceObject("Text", "DocumentIdentityNoHint")


        If blnSearchByDOB Then
            Me.lblDOAECHint.Visible = False
            Me.lblDOBECHint.Visible = True
            Me.lblDOBECHint2.Visible = True
            Me.lblDOBECHint.Text = Me.GetGlobalResourceObject("Text", "ECDOBHint")
            Me.lblDOBECHint2.Text = Me.GetGlobalResourceObject("Text", "ECDOBHint2")


            Me.txtECDOAAge.Text = String.Empty
            Me.txtECDOADayEn.Text = String.Empty
            Me.txtECDOAYearEn.Text = String.Empty
            Me.ddlECDOAMonth.SelectedIndex = -1
            Me.txtECDOAYearChi.Text = String.Empty
            Me.txtECDOADayChi.Text = String.Empty

            Me.txtECDOAAge.BackColor = Drawing.Color.Silver
            Me.txtECDOADayEn.BackColor = Drawing.Color.Silver
            Me.txtECDOAYearEn.BackColor = Drawing.Color.Silver
            Me.ddlECDOAMonth.BackColor = Drawing.Color.Silver
            Me.txtECDOAYearChi.BackColor = Drawing.Color.Silver
            Me.txtECDOADayChi.BackColor = Drawing.Color.Silver

            Me.txtECDOB.BackColor = Drawing.Color.White
        Else
            Me.lblDOAECHint.Visible = True
            Me.lblDOBECHint.Visible = False
            Me.lblDOBECHint2.Visible = False
            Me.lblDOAECHint.Text = Me.GetGlobalResourceObject("Text", "ECDORegisterAgeHint")

            Me.txtECDOAAge.BackColor = Drawing.Color.White
            Me.txtECDOADayEn.BackColor = Drawing.Color.White
            Me.txtECDOAYearEn.BackColor = Drawing.Color.White
            Me.ddlECDOAMonth.BackColor = Drawing.Color.White
            Me.txtECDOAYearChi.BackColor = Drawing.Color.White
            Me.txtECDOADayChi.BackColor = Drawing.Color.White

            Me.txtECDOB.Text = String.Empty
            Me.txtECDOB.BackColor = Drawing.Color.Silver
        End If

        Me.SetECDOAAgeError(False)
        Me.SetECDOAError(False)
        Me.SetECDOBError(False)
    End Sub
#End Region

#Region "Functions"

    Public Function IsEmpty(ByVal documentType As String) As Boolean
        Select Case documentType
            Case DocTypeModel.DocTypeCode.HKIC
                ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
                ' ----------------------------------------------------------
                If Not Me.txtSearchHKICNo.Text.Trim().Equals(String.Empty) OrElse _
                   Not Me.txtSearchHKICDOB.Text.Trim().Equals(String.Empty) OrElse _
                   Not Me.rblHKICSymbol.SelectedValue = String.Empty Then
                    Return True
                End If
                ' CRE17-010 (OCSSS integration) [End][Chris YIM]


            Case DocTypeModel.DocTypeCode.HKBC, _
                DocTypeModel.DocTypeCode.REPMT, _
                DocTypeModel.DocTypeCode.DI, _
                DocTypeModel.DocTypeCode.ID235B, _
                DocTypeModel.DocTypeCode.VISA


                If Not Me.txtSearchShortIdentityNo.Text.Trim().Equals(String.Empty) OrElse _
                   Not Me.txtSearchShortDOB.Text.Trim().Equals(String.Empty) Then
                    Return True
                End If

            Case DocTypeModel.DocTypeCode.EC
                If Not Me.txtECDOAAge.Text.Trim().Equals(String.Empty) OrElse _
                   Not Me.txtECDOADayChi.Text.Trim().Equals(String.Empty) OrElse _
                    Not Me.txtECDOADayEn.Text.Trim().Equals(String.Empty) OrElse _
                    Not Me.txtECDOAYearChi.Text.Trim().Equals(String.Empty) OrElse _
                   Not Me.txtECDOAYearEn.Text.Trim().Equals(String.Empty) OrElse _
                   Not Me.txtECDOB.Text.Trim().Equals(String.Empty) OrElse _
                   Not Me.txtECHKID.Text.Trim().Equals(String.Empty) OrElse _
                   Not Me.ddlECDOAMonth.SelectedIndex.Equals(0) Then
                    Return True
                End If

            Case DocTypeModel.DocTypeCode.ADOPC

                If Not Me.txtADOPCIdentityNo.Text.Trim().Equals(String.Empty) OrElse _
                    Not Me.txtADOPCIdentityNoPrefix.Text.Trim().Equals(String.Empty) OrElse _
                    Not Me.txtADOPCDOB.Text.Trim().Equals(String.Empty) Then
                    Return True
                End If

            Case String.Empty

        End Select

        Return False
    End Function

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub EnableSmartIDButton(ByVal blnVisible As Boolean)
        If blnVisible Then
            ' Check system parameters to see if enable
            If _blnIDEASComboForceToUse Then
                'IDEAS Combo Only Period 
                If _blnIDEASComboClientInstalled Then
                    'IDEAS Combo Client Installed
                    If SmartIDHandler.TurnOnSmartID Then
                        lblReadNewCardAndSearchComboNA.Visible = False
                        btnShortIdentityNoNewSmartIDCombo.Visible = True
                        btnShortIdentityNoNewSmartIDCombo.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchBtn")

                        EnableHKICSearchOption(True, SearchHKICOption.ReadNewSmartICCombo)

                    Else
                        btnShortIdentityNoNewSmartIDCombo.Visible = False
                        lblReadNewCardAndSearchComboNA.Visible = True
                        lblReadNewCardAndSearchComboNA.Text = Me.GetGlobalResourceObject("Text", "ReadCardAndSearchNA")

                        EnableHKICSearchOption(False, SearchHKICOption.ReadNewSmartICCombo)

                    End If
                Else
                    'IDEAS Combo Client Not Installed
                    divSmartIDSoftwareNotInstalled.Visible = True

                    If SmartIDHandler.TurnOnSmartID Then
                        lblReadNewCardAndSearchComboNA.Visible = False
                        btnShortIdentityNoNewSmartIDCombo.Visible = True
                        btnShortIdentityNoNewSmartIDCombo.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchDisableBtn")
                        btnShortIdentityNoNewSmartIDCombo.Enabled = False

                        EnableHKICSearchOption(True, SearchHKICOption.ReadNewSmartICCombo)

                    Else
                        btnShortIdentityNoNewSmartIDCombo.Visible = False
                        lblReadNewCardAndSearchComboNA.Visible = True
                        lblReadNewCardAndSearchComboNA.Text = Me.GetGlobalResourceObject("Text", "ReadCardAndSearchNA")

                        EnableHKICSearchOption(False, SearchHKICOption.ReadNewSmartICCombo)

                    End If
                End If
            Else
                'Transition Period
                If _blnIDEASComboClientInstalled Then
                    'IDEAS Combo
                    If SmartIDHandler.TurnOnSmartID Then
                        lblReadNewCardAndSearchComboNA.Visible = False
                        btnShortIdentityNoNewSmartIDCombo.Visible = True
                        btnShortIdentityNoNewSmartIDCombo.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchBtn")

                        EnableHKICSearchOption(True, SearchHKICOption.ReadNewSmartICCombo)

                    Else
                        btnShortIdentityNoNewSmartIDCombo.Visible = False
                        lblReadNewCardAndSearchComboNA.Visible = True
                        lblReadNewCardAndSearchComboNA.Text = Me.GetGlobalResourceObject("Text", "ReadCardAndSearchNA")

                        EnableHKICSearchOption(False, SearchHKICOption.ReadNewSmartICCombo)

                    End If
                Else
                    'IDEAS Lite
                    If SmartIDHandler.TurnOnSmartID Then
                        lblReadOldCardAndSearchNA.Visible = False
                        btnShortIdentityNoOldSmartID.Visible = True
                        btnShortIdentityNoOldSmartID.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchBtn")

                        EnableHKICSearchOption(True, SearchHKICOption.ReadOldSmartIC)

                        lblReadNewCardAndSearchNA.Visible = False
                        btnShortIdentityNoNewSmartID.Visible = True
                        btnShortIdentityNoNewSmartID.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ReadCardAndSearchBtn")

                        EnableHKICSearchOption(True, SearchHKICOption.ReadNewSmartIC)

                    Else
                        btnShortIdentityNoNewSmartID.Visible = False
                        lblReadNewCardAndSearchNA.Visible = True
                        lblReadNewCardAndSearchNA.Text = Me.GetGlobalResourceObject("Text", "ReadCardAndSearchNA")

                        btnShortIdentityNoOldSmartID.Visible = False
                        lblReadOldCardAndSearchNA.Visible = True
                        btnShortIdentityNoOldSmartID.ImageUrl = Me.GetGlobalResourceObject("Text", "ReadCardAndSearchNA")

                        EnableHKICSearchOption(False, SearchHKICOption.ReadOldSmartIC)
                        EnableHKICSearchOption(False, SearchHKICOption.ReadNewSmartIC)

                    End If
                End If
            End If


        Else

            If _blnIDEASComboClientInstalled Then
                'IDEAS Combo
                btnShortIdentityNoNewSmartIDCombo.Visible = False
                lblReadNewCardAndSearchComboNA.Visible = False

            Else
                'IDEAS Lite
                btnShortIdentityNoNewSmartID.Visible = False
                lblReadNewCardAndSearchNA.Visible = False

                btnShortIdentityNoOldSmartID.Visible = False
                lblReadOldCardAndSearchNA.Visible = False

                mvOldHKIC.SetActiveView(vOldHKICSample)
                mvNewHKIC.SetActiveView(vNewHKICSample)

            End If

        End If

    End Sub

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Sub EnableHKICSymbolRadioButtonList(ByVal blnShow As Boolean)
        If blnShow Then
            ' Check system parameters to see if enable
            If Common.OCSSS.OCSSSServiceBLL.EnableHKICSymbolInput(_strSchemeCode) Then
                Dim strSelectedValue As String = String.Empty

                'Store selected value into temp variable
                If rblHKICSymbol.SelectedValue <> String.Empty Then
                    strSelectedValue = rblHKICSymbol.SelectedValue
                End If

                'Clear radio button list
                rblHKICSymbol.Items.Clear()
                rblHKICSymbol.SelectedIndex = -1
                rblHKICSymbol.SelectedValue = Nothing

                'Reload radio button list
                rblHKICSymbol.DataSource = Status.GetDescriptionListFromDBEnumCode("HKICSymbol")

                Select Case Session("language")
                    Case CultureLanguage.English
                        rblHKICSymbol.DataTextField = "Status_Description"
                    Case CultureLanguage.TradChinese
                        rblHKICSymbol.DataTextField = "Status_Description_Chi"
                    Case CultureLanguage.SimpChinese
                        rblHKICSymbol.DataTextField = "Status_Description_CN"
                    Case Else
                        rblHKICSymbol.DataTextField = "Status_Description"
                End Select

                rblHKICSymbol.DataValueField = "Status_Value"
                rblHKICSymbol.DataBind()

                'Restore selected value from temp variable
                If strSelectedValue <> String.Empty Then
                    rblHKICSymbol.SelectedValue = strSelectedValue
                End If

                _blnDisplayHKICSymbol = True
                tblHKICSymbol.Style.Remove("display")

                If Me._blnSchemeSelected Then
                    Me.rblHKICSymbol.Enabled = True
                Else
                    Me.rblHKICSymbol.Enabled = False
                End If

            Else
                _blnDisplayHKICSymbol = False
                tblHKICSymbol.Style.Add("display", "none")
            End If

        Else
            _blnDisplayHKICSymbol = False
            tblHKICSymbol.Style.Add("display", "none")

        End If

    End Sub
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE19-028 (IDEAS Combo) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub EnableHKICSearchOption(ByVal blnEnable As Boolean, ByVal eSearchHKICOption As SearchHKICOption)
        If blnEnable Then
            Dim sb As New StringBuilder()

            Select Case eSearchHKICOption
                Case SearchHKICOption.ManualInput
                    tblManual.Style.Add("background-image", String.Format("url({0})", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchManualInputDisabledBanner"))))
                    tblManual.Attributes.Add("onmouseover", String.Format("this.style.backgroundImage= 'url({0})';", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchManualInputBanner"))))
                    tblManual.Attributes.Add("onmouseout", String.Format("this.style.backgroundImage= 'url({0})';", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchManualInputDisabledBanner"))))

                Case SearchHKICOption.ReadOldSmartIC
                    tblOldSmartIC.Style.Add("background-image", String.Format("url({0})", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchOldSmartICDisabledBanner"))))
                    tblOldSmartIC.Attributes.Add("onmouseover", String.Format("this.style.backgroundImage= 'url({0})'; this.style.cursor = 'pointer';", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchOldSmartICBanner"))))
                    tblOldSmartIC.Attributes.Add("onmouseout", String.Format("this.style.backgroundImage= 'url({0})';", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchOldSmartICDisabledBanner"))))

                    sb.Append(Me.Page.ClientScript.GetPostBackEventReference(btnShortIdentityNoOldSmartID, ""))
                    sb.Append("; return false;")
                    tblOldSmartIC.Attributes.Add("onclick", sb.ToString())

                Case SearchHKICOption.ReadNewSmartIC
                    tblNewSmartIC.Style.Add("background-image", String.Format("url({0})", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchNewSmartICDisabledBanner"))))
                    tblNewSmartIC.Attributes.Add("onmouseover", String.Format("this.style.backgroundImage= 'url({0})'; this.style.cursor = 'pointer';", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchNewSmartICBanner"))))
                    tblNewSmartIC.Attributes.Add("onmouseout", String.Format("this.style.backgroundImage= 'url({0})';", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchNewSmartICDisabledBanner"))))

                    sb.Append(Me.Page.ClientScript.GetPostBackEventReference(btnShortIdentityNoNewSmartID, ""))
                    sb.Append("; return false;")
                    tblNewSmartIC.Attributes.Add("onclick", sb.ToString())

                Case SearchHKICOption.ReadNewSmartICCombo
                    If _blnIDEASComboForceToUse Then
                        If _blnIDEASComboClientInstalled Then
                            tblNewSmartICCombo.Style.Add("background-image", String.Format("url({0})", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchNewSmartICComboDisabled"))))
                            tblNewSmartICCombo.Attributes.Add("onmouseover", String.Format("this.style.backgroundImage= 'url({0})'; this.style.cursor = 'pointer';", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchNewSmartICCombo"))))
                            tblNewSmartICCombo.Attributes.Add("onmouseout", String.Format("this.style.backgroundImage= 'url({0})';", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchNewSmartICComboDisabled"))))

                            sb.Append(Me.Page.ClientScript.GetPostBackEventReference(btnShortIdentityNoNewSmartIDCombo, ""))
                            sb.Append("; return false;")
                            tblNewSmartICCombo.Attributes.Add("onclick", sb.ToString())
                        Else
                            tblNewSmartICCombo.Style.Add("background-image", String.Format("url({0})", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "NewSmartIDComboEmptyDisabled"))))
                            tblNewSmartICCombo.Attributes.Add("onmouseover", String.Format("this.style.backgroundImage= 'url({0})';", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "NewSmartIDComboEmpty"))))
                            tblNewSmartICCombo.Attributes.Add("onmouseout", String.Format("this.style.backgroundImage= 'url({0})';", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "NewSmartIDComboEmptyDisabled"))))
                        End If

                    Else
                        tblNewSmartICCombo.Style.Add("background-image", String.Format("url({0})", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchNewSmartICComboDisabled"))))
                        tblNewSmartICCombo.Attributes.Add("onmouseover", String.Format("this.style.backgroundImage= 'url({0})'; this.style.cursor = 'pointer';", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchNewSmartICCombo"))))
                        tblNewSmartICCombo.Attributes.Add("onmouseout", String.Format("this.style.backgroundImage= 'url({0})';", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchNewSmartICComboDisabled"))))

                        sb.Append(Me.Page.ClientScript.GetPostBackEventReference(btnShortIdentityNoNewSmartIDCombo, ""))
                        sb.Append("; return false;")
                        tblNewSmartICCombo.Attributes.Add("onclick", sb.ToString())
                    End If

            End Select

        Else
            Select Case eSearchHKICOption
                Case SearchHKICOption.ManualInput
                    tblManual.Style.Add("background-image", String.Format("url({0})", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchManualInputDisabledBanner"))))
                    tblManual.Attributes.Remove("onmouseover")
                    tblManual.Attributes.Remove("onmouseout")

                Case SearchHKICOption.ReadOldSmartIC
                    tblOldSmartIC.Style.Add("background-image", String.Format("url({0})", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchOldSmartICDisabledBanner"))))
                    tblOldSmartIC.Attributes.Remove("onmouseover")
                    tblOldSmartIC.Attributes.Remove("onmouseout")
                    tblOldSmartIC.Attributes.Remove("onclick")

                Case SearchHKICOption.ReadNewSmartIC
                    tblNewSmartIC.Style.Add("background-image", String.Format("url({0})", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchNewSmartICDisabledBanner"))))
                    tblNewSmartIC.Attributes.Remove("onmouseover")
                    tblNewSmartIC.Attributes.Remove("onmouseout")
                    tblNewSmartIC.Attributes.Remove("onclick")

                Case SearchHKICOption.ReadNewSmartICCombo
                    If _blnIDEASComboForceToUse Then
                        If _blnIDEASComboClientInstalled Then
                            tblNewSmartICCombo.Style.Add("background-image", String.Format("url({0})", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchNewSmartICComboDisabled"))))
                        Else
                            tblNewSmartICCombo.Style.Add("background-image", String.Format("url({0})", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "NewSmartIDComboEmptyDisabled"))))
                        End If
                    Else
                        tblNewSmartICCombo.Style.Add("background-image", String.Format("url({0})", ResolveClientUrl(Me.GetGlobalResourceObject("ImageUrl", "SearchNewSmartICComboDisabled"))))
                    End If

                    tblNewSmartICCombo.Attributes.Remove("onmouseover")
                    tblNewSmartICCombo.Attributes.Remove("onmouseout")
                    tblNewSmartICCombo.Attributes.Remove("onclick")
            End Select

        End If

    End Sub
    ' CRE19-028 (IDEAS Combo) [End][Chris YIM]	

    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Function CheckFromVaccinationRecordEnquiry() As Boolean
        Dim _udtSessionHandler As New SessionHandler
        Return _udtSessionHandler.FromVaccinationRecordEnquiryGetFromSession
    End Function
    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
#End Region

    Private Sub lbtnUpdateNow_Click(sender As Object, e As EventArgs) Handles lbtnUpdateNow.Click

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "UpdateNow", String.Format("javascript:showUpdateNow('{0}');", Session("language")), True)

        RaiseEvent UpdateNowLinkButtonClick(sender, e)

    End Sub

    Private Sub lbtnHere_Click(sender As Object, e As EventArgs) Handles lbtnSmartIDSoftwareNotInstalled2.Click

        ScriptManager.RegisterStartupScript(Me, Page.GetType, "Here", String.Format("javascript:showUpdateNow('{0}');", Session("language")), True)

        RaiseEvent HereLinkButtonClick(sender, e)

    End Sub

End Class