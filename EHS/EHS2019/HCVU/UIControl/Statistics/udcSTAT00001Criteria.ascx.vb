Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format

Partial Public Class udcSTAT00001Criteria
    Inherits StatisticsCriteriaUC

#Region "Variables"

    'Private _dicSetting As New Dictionary(Of String, Dictionary(Of String, String))

#End Region

#Region "Session and Const"

    Private Class VS
        Public Const District As String = "District"
        Public Const Profession As String = "Profession"
    End Class

    Public Class Field
        Public Const PeriodBreakDown As String = "PeriodBreakDown"
        Public Const FromDate_Creation As String = "FromDateCreation"
        Public Const ToDate_Creation As String = "ToDateCreation"
        Public Const FromDate_AsAt As String = "FromDateAsAt"
        Public Const ToDate_AsAt As String = "ToDateAsAt"
        Public Const MinAge As String = "MinAge"
        Public Const MaxAge As String = "MaxAge"
        Public Const Scheme As String = "Scheme"
        Public Const CountingItem As String = "CountingItem"
        Public Const Subsidy As String = "Subsidy"
        Public Const BreakDownType As String = "BreakDownType"
        Public Const District As String = "District"
        Public Const Profession As String = "Profession"
    End Class

    Public Class CustomFieldSetting
        Public Const EarliestDate As String = "EarliestDate"
        Public Const LatestDate As String = "LatestDate"
        Public Const AllowPastDate As String = "AllowPastDate"
        Public Const AllowFutureDate As String = "AllowFutureDate"
        Public Const RangeMin As String = "RangeMin"
        Public Const RangeMax As String = "RangeMax"
        Public Const UseCutOffDate As String = "UseCutOffDate"
    End Class

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Private Sub ddlTypeOfCount_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTypeOfCount.SelectedIndexChanged
        ' Init subsidy panel visibility
        panSubsidy.Visible = False
        ' Init subsidy dropdown value
        ddlSubsidy.SelectedIndex = -1

        imgErrorSubsidy.Visible = False

        Select Case ddlTypeOfCount.SelectedValue
            Case TypeOfCountingItem.Transaction
                panSubsidy.Visible = False
            Case TypeOfCountingItem.Subsidy
                panSubsidy.Visible = True
            Case Else
                ' No action
        End Select
    End Sub

    Private Sub ddlScheme_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlScheme.SelectedIndexChanged
        ' All selected profession will be cleared when dropdown (scheme) selected item is changed
        ResetProfessionSelection()
        MyBase.ResetSubsidySelection(ddlScheme, ddlSubsidy)
    End Sub

    Private Sub ddlTypeOfBreakDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTypeOfBreakDown.SelectedIndexChanged
        ' Init distirct and profession panel visibility
        panDistrict.Visible = False
        panProfession.Visible = False

        ' All selected profession and district will be cleared
        ' When dropdown (Type of break down) selected item is changed
        ResetProfessionSelection()
        ResetDistrictSelection()

        imgErrorDistrict.Visible = False
        imgErrorProfession.Visible = False

        Select Case ddlTypeOfBreakDown.SelectedValue
            Case TypeOfBreakDown.Profession
                panDistrict.Visible = True
            Case TypeOfBreakDown.District
                panProfession.Visible = True
            Case Else
                ' No action
        End Select

    End Sub

    Private Sub rbtnDistrictType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnDistrictType.SelectedIndexChanged
        Select Case rbtnDistrictType.SelectedIndex
            Case MultiSelectionTypeEnum.Any
                If chkDistrict.Items.Count > 0 Then
                    'For Each boxItem As ListItem In chkDistrict.Items
                    '    boxItem.Selected = True
                    'Next

                    'popupDistrict.Show()

                    'If chkDistrict.Items.Count <= 4 Then
                    '    chkDistrict.RepeatColumns = 1
                    'Else
                    '    chkDistrict.RepeatColumns = 2
                    'End If

                    SetDistrictSelectionToAny()

                End If

            Case MultiSelectionTypeEnum.Specific
                If chkDistrict.Items.Count > 0 Then
                    For Each boxItem As ListItem In chkDistrict.Items
                        boxItem.Selected = False
                    Next

                    popupDistrict.Show()

                    If chkDistrict.Items.Count <= 4 Then
                        chkDistrict.RepeatColumns = 1
                    Else
                        chkDistrict.RepeatColumns = 2
                    End If

                End If

            Case MultiSelectionTypeEnum.NoSelection
                ' Edit
        End Select

    End Sub


    Private Sub rbtnProfessionType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnProfessionType.SelectedIndexChanged
        Select Case rbtnProfessionType.SelectedIndex
            Case MultiSelectionTypeEnum.Any
                If chkProfession.Items.Count > 0 Then
                    'For Each boxItem As ListItem In chkProfession.Items
                    '    boxItem.Selected = True
                    'Next

                    'popupProfession.Show()

                    'If chkProfession.Items.Count <= 4 Then
                    '    chkProfession.RepeatColumns = 1
                    'Else
                    '    chkProfession.RepeatColumns = 2
                    'End If

                    SetProfessionSelectionToAny()

                End If

            Case MultiSelectionTypeEnum.Specific
                If chkProfession.Items.Count > 0 Then
                    For Each boxItem As ListItem In chkProfession.Items
                        boxItem.Selected = False
                    Next

                    popupProfession.Show()

                    If chkProfession.Items.Count <= 4 Then
                        chkProfession.RepeatColumns = 1
                    Else
                        chkProfession.RepeatColumns = 2
                    End If

                End If

            Case MultiSelectionTypeEnum.NoSelection
                ' Edit
        End Select
    End Sub

#End Region

#Region "Popup function"

    Public Sub ibtnAddDistrict_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If chkDistrict.Items.Count > 0 Then
            popupDistrict.Show()

            If chkDistrict.Items.Count <= 4 Then
                chkDistrict.RepeatColumns = 1
            Else
                chkDistrict.RepeatColumns = 2
            End If

        End If
    End Sub

    Public Sub ibtnDistrictPopupOk_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' Add selected value to list 
        AddDistrictIntoList()
    End Sub

    Public Sub ibtnDistrictPopupCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim items As New Dictionary(Of String, Boolean)
        If Not ViewState(VS.District) Is Nothing Then
            items = CType(ViewState(VS.District), Dictionary(Of String, Boolean))
        End If

        If items.Count = 0 Then
            ' First cancel, no value is selected before
            For Each boxItem As ListItem In chkDistrict.Items
                boxItem.Selected = False
            Next

        Else
            ' Have value already
            For Each boxItem As ListItem In chkDistrict.Items
                If items.ContainsKey(boxItem.Value.ToString.Trim) Then
                    boxItem.Selected = items(boxItem.Value.ToString.Trim)
                End If
            Next

        End If

        ' District checkboxlist item

        Dim strChkBoxItemString As String = String.Empty
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkDistrict.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        '' If all items are selected
        'If intIsSelectedCount = chkDistrict.Items.Count Then
        '    rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.Any
        '    strChkBoxItemString = String.Empty
        'Else
        '    ' Have selected items
        '    If intIsSelectedCount > 0 Then
        '        rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.Specific
        '    Else
        '        rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        '    End If

        'End If

        ' Have selected items
        If rbtnDistrictType.SelectedIndex <> MultiSelectionTypeEnum.Any Then
            If intIsSelectedCount > 0 Then
                rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.Specific
            Else
                rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
            End If
        End If

        lblAddDistrictDisplay.Text = strChkBoxItemString

    End Sub

    Public Sub ibtnAddProfession_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If chkProfession.Items.Count > 0 Then
            popupProfession.Show()

            If chkProfession.Items.Count <= 4 Then
                chkProfession.RepeatColumns = 1
            Else
                chkProfession.RepeatColumns = 2
            End If

        End If
    End Sub

    Public Sub ibtnProfessionPopupOk_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' Add selected value to list 
        AddProfessionIntoList()
    End Sub

    Public Sub ibtnProfessionPopupCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim items As New Dictionary(Of String, Boolean)
        If Not ViewState(VS.Profession) Is Nothing Then
            items = CType(ViewState(VS.Profession), Dictionary(Of String, Boolean))
        End If

        If items.Count = 0 Then
            ' First cancel, no value is selected before
            For Each boxItem As ListItem In chkProfession.Items
                boxItem.Selected = False
            Next

        Else
            ' Have value already
            For Each boxItem As ListItem In chkProfession.Items
                If items.ContainsKey(boxItem.Value.ToString.Trim) Then
                    boxItem.Selected = items(boxItem.Value.ToString.Trim)
                End If
            Next

        End If

        ' Health profession checkboxlist item

        Dim strChkBoxItemString As String = String.Empty
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkProfession.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        ' If all items are selected
        'If intIsSelectedCount = chkProfession.Items.Count Then
        '    rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Any
        '    strChkBoxItemString = String.Empty
        'Else
        '    ' Have selected items
        '    If intIsSelectedCount > 0 Then
        '        rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Specific
        '    Else
        '        rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        '    End If

        'End If

        ' Have selected items
        If rbtnProfessionType.SelectedIndex <> MultiSelectionTypeEnum.Any Then
            If intIsSelectedCount > 0 Then
                rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Specific
            Else
                rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
            End If
        End If

        lblAddProfessionDisplay.Text = strChkBoxItemString

    End Sub

#End Region

#Region "Implement IStatisticsCriteriaUC"

    Public Overrides Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
        ' Control setting file
        '_dicSetting = dicSetting

        ' Build period break down component
        BuildPeriodBreakDownComponent(Me.ddlPeriodBreakDown)

        ' Initialize Calendar Extender
        Dim udtFormatter As New Formatter

        calExFromDate_D_Creation.Format = udtFormatter.EnterDateFormat
        calExToDate_D_Creation.Format = udtFormatter.EnterDateFormat
        calExFromDate_D_AsAt.Format = udtFormatter.EnterDateFormat
        calExToDate_D_AsAt.Format = udtFormatter.EnterDateFormat

        ' Field 5 - TransSubsidy (Big control) [Start]
        SetComponentErrorVisibility(False)

        ' Build components
        BuildSchemeComponent(Me.ddlScheme)
        BuildTypeOfCountComponent(Me.ddlTypeOfCount)
        BuildSubsidyComponent(Me.ddlSubsidy)
        BuildTypeOfBreakDownComponent(Me.ddlTypeOfBreakDown)

        ' Build district
        BuildDistrictComponent(chkDistrict, Field.District)
        ' Build profession
        BuildProfessionComponent(ddlScheme, chkProfession, Field.Profession)

        ' Field 5 - TransSubsidy (Big control) [End]

        MyBase.Build(dicSetting)

        ' Initial control
        'InitControl()

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        Dim udtFormatter As New Formatter

        ' Field 5 - TransSubsidy (Set control visibility) [Start]
        SetComponentErrorVisibility(False)
        ' Field 5 - TransSubsidy (Set control visibility) [End]

        SetErrorTypeOfDateVisibility(False)

        ' Field 1 - Period break down (Level of Statistic) [Start]
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.Visible) Then
            If GetSetting(Field.PeriodBreakDown, FieldSetting.Visible) = Condition.YES Then
                ' Check dropdown (Period break down)
                If ddlPeriodBreakDown.SelectedValue.Trim = DROP_DOWN_EMPTY Then
                    Dim strPeriodBreakDownText As String = String.Empty

                    If IsExistValue(Field.PeriodBreakDown, FieldSetting.DescResource) Then
                        strPeriodBreakDownText = Me.GetGlobalResourceObject("Text", GetSetting(Field.PeriodBreakDown, FieldSetting.DescResource))
                    Else
                        strPeriodBreakDownText = Me.GetGlobalResourceObject("Text", "TypeOfStatistic")
                    End If

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                    lstErrorParam1.Add(strPeriodBreakDownText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorPeriodBreakDown.Visible = True
                End If

            End If
        End If
        ' Field 1 - Period break down (Level of Statistic) [End]

        ' Field 5 - TransSubsidy (Break Down Type) [Start]
        ' Type of break down
        If IsExistValue(Field.BreakDownType, FieldSetting.Visible) Then
            If GetSetting(Field.BreakDownType, FieldSetting.Visible) = Condition.YES Then
                ' Check dropdown (Type of break down)
                If ddlTypeOfBreakDown.SelectedValue.Trim = DROP_DOWN_EMPTY Then
                    Dim strTypeOfBreakDownText As String = String.Empty

                    If IsExistValue(Field.BreakDownType, FieldSetting.DescResource) Then
                        strTypeOfBreakDownText = Me.GetGlobalResourceObject("Text", GetSetting(Field.BreakDownType, FieldSetting.DescResource))
                    Else
                        strTypeOfBreakDownText = Me.GetGlobalResourceObject("Text", "BreakDownType")
                    End If

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                    lstErrorParam1.Add(strTypeOfBreakDownText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorTypeOfBreakDown.Visible = True
                End If

            End If
        End If

        ' Field 5 - TransSubsidy (Break Down Type) [End]

        ' Field 2 - Exact date period (Account creation date) [Start]
        Dim blnFromDateMiss_Creation As Boolean = False
        Dim blnToDateMiss_Creation As Boolean = False

        Dim blnFromDateValid_Creation As Boolean = True
        Dim blnToDateValid_Creation As Boolean = True

        Dim blnMissInput_Creation As Boolean = False
        Dim blnInvalid_Creation As Boolean = False
        'Dim blnIncomplete As Boolean = False

        ' Set period error image
        SetPeriodCreationErrorImageVisibility(False)

        ' Set input date format
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'txtFromDate_D_Creation.Text = udtFormatter.formatDate(txtFromDate_D_Creation.Text.Trim)
        'txtToDate_D_Creation.Text = udtFormatter.formatDate(txtToDate_D_Creation.Text.Trim)
        txtFromDate_D_Creation.Text = udtFormatter.formatInputDate(txtFromDate_D_Creation.Text.Trim)
        txtToDate_D_Creation.Text = udtFormatter.formatInputDate(txtToDate_D_Creation.Text.Trim)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES Then
                ' Cannot empty
                If txtFromDate_D_Creation.Text = String.Empty Then
                    blnMissInput_Creation = True
                    blnFromDateValid_Creation = False

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003))
                    lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                    lstErrorParam2.Add(String.Empty)
                    'imgErrorFromDate_D.Visible = True
                    imgErrorDate_D_Creation.Visible = True
                Else
                    ' Date validation
                    If Not IsDate(udtFormatter.convertDate(txtFromDate_D_Creation.Text.Trim, String.Empty)) Then
                        blnInvalid_Creation = True
                        blnFromDateValid_Creation = False

                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00004))
                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                        lstErrorParam2.Add(String.Empty)
                        'imgErrorFromDate_D.Visible = True
                        imgErrorDate_D_Creation.Visible = True
                    End If

                End If

            End If
        End If

        ' Period To
        'SetPeriodToErrorImageVisibility(False)

        If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate_Creation, FieldSetting.Visible) = Condition.YES Then
                ' Cannot empty
                If txtToDate_D_Creation.Text = String.Empty Then
                    blnMissInput_Creation = True
                    blnToDateValid_Creation = False

                    If blnFromDateValid_Creation = True Then
                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003))
                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                        lstErrorParam2.Add(String.Empty)
                        'imgErrorToDate_D.Visible = True
                        imgErrorDate_D_Creation.Visible = True
                    End If
                Else
                    ' Date validation
                    If Not IsDate(udtFormatter.convertDate(txtToDate_D_Creation.Text.Trim, String.Empty)) Then
                        blnInvalid_Creation = True
                        blnToDateValid_Creation = False

                        If blnFromDateValid_Creation = True Then
                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00004))
                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                            lstErrorParam2.Add(String.Empty)
                            'imgErrorToDate_D.Visible = True
                            imgErrorDate_D_Creation.Visible = True
                        End If
                    End If

                End If

            End If
        End If

        'If blnMissInput Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00023))
        '    lstErrorParam1.Add("Date Period")
        '    lstErrorParam2.Add(String.Empty)
        'End If

        'If blnInvalid Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00024))
        '    lstErrorParam1.Add("Date Period")
        '    lstErrorParam2.Add(String.Empty)
        'End If

        'If blnIncomplete Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00027))
        '    lstErrorParam1.Add("Date Period")
        '    lstErrorParam2.Add(String.Empty)
        'End If

        ' Check Period From boundary [Start]
        If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES Then

                If txtFromDate_D_Creation.Text <> String.Empty AndAlso blnFromDateValid_Creation Then
                    ' Earliest Date
                    If IsExistValue(Field.FromDate_Creation, CustomFieldSetting.EarliestDate) Then
                        Dim strValidationDate As String = GetSetting(Field.FromDate_Creation, CustomFieldSetting.EarliestDate)
                        If Not strValidationDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D_Creation.Text)
                            If dtmFromDate.CompareTo(dtmValidationDate) < 0 Then
                                ' To date is visible, use From + To Date
                                If IsExistValue(Field.ToDate_Creation, FieldSetting.Visible) Then
                                    If GetSetting(Field.ToDate_Creation, FieldSetting.Visible) = Condition.YES Then
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)) + " From")
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    Else
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    End If
                                End If

                                'imgErrorFromDate_D.Visible = True
                                imgErrorDate_D_Creation.Visible = True
                            End If
                        End If
                    End If

                    ' Latest Date
                    If IsExistValue(Field.FromDate_Creation, CustomFieldSetting.LatestDate) Then
                        Dim strValidationDate As String = GetSetting(Field.FromDate_Creation, CustomFieldSetting.LatestDate)
                        If Not strValidationDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D_Creation.Text)
                            If dtmFromDate.CompareTo(dtmValidationDate) > 0 Then
                                ' To date is visible, use From + To Date
                                If IsExistValue(Field.ToDate_Creation, FieldSetting.Visible) Then
                                    If GetSetting(Field.ToDate_Creation, FieldSetting.Visible) = Condition.YES Then
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)) + " From")
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    Else
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    End If
                                End If

                                'imgErrorFromDate_D.Visible = True
                                imgErrorDate_D_Creation.Visible = True
                            End If
                        End If
                    End If

                    ' Allow Pass Date
                    If IsExistValue(Field.FromDate_Creation, CustomFieldSetting.AllowPastDate) Then
                        Dim strAllowPassDate As String = GetSetting(Field.FromDate_Creation, CustomFieldSetting.AllowPastDate)
                        If Not strAllowPassDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D_Creation.Text)

                            ' If allow pass date value is N, do checking
                            If strAllowPassDate = Condition.NO Then
                                If dtmFromDate.CompareTo(dtmValidationDate) < 0 Then
                                    ' To date is visible, use From + To Date
                                    If IsExistValue(Field.ToDate_Creation, FieldSetting.Visible) Then
                                        If GetSetting(Field.ToDate_Creation, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00008))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)) + " From")
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00008))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        End If
                                    End If

                                    'imgErrorFromDate_D.Visible = True
                                    imgErrorDate_D_Creation.Visible = True
                                End If
                            End If
                        End If
                    End If

                    ' Allow Future Date
                    If IsExistValue(Field.FromDate_Creation, CustomFieldSetting.AllowFutureDate) Then
                        Dim strAllowFutureDate As String = GetSetting(Field.FromDate_Creation, CustomFieldSetting.AllowFutureDate)
                        If Not strAllowFutureDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D_Creation.Text)

                            ' If allow future date value is N, do checking
                            If strAllowFutureDate = Condition.NO Then
                                If dtmFromDate.CompareTo(dtmValidationDate) > 0 Then
                                    ' To date is visible, use From + To Date
                                    If IsExistValue(Field.ToDate_Creation, FieldSetting.Visible) Then
                                        If GetSetting(Field.ToDate_Creation, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00009))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)) + " From")
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00009))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        End If
                                    End If

                                    'imgErrorFromDate_D.Visible = True
                                    imgErrorDate_D_Creation.Visible = True
                                End If
                            End If
                        End If
                    End If

                    ' Allow cut off date
                    If IsExistValue(Field.FromDate_Creation, CustomFieldSetting.UseCutOffDate) Then
                        Dim strAllowCutOffDate As String = GetSetting(Field.FromDate_Creation, CustomFieldSetting.UseCutOffDate)
                        If Not strAllowCutOffDate Is String.Empty Then
                            Dim dtmCutOffDate As DateTime = (New StatisticsBLL).GetDataCutOffDate(StatisticsBLL.EnumDataCutOffID.dbEVS_Enquiry)
                            Dim dtmValidationDate As DateTime = New DateTime(dtmCutOffDate.Year, dtmCutOffDate.Month, dtmCutOffDate.Day)
                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D_Creation.Text)

                            ' If allow cut off date value is Y, do checking
                            If strAllowCutOffDate = Condition.YES Then
                                If dtmFromDate.CompareTo(dtmValidationDate) > 0 Then
                                    ' To date is visible, use From + To Date
                                    If IsExistValue(Field.ToDate_Creation, FieldSetting.Visible) Then
                                        If GetSetting(Field.ToDate_Creation, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)) + " From")
                                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                        End If
                                    End If

                                    'imgErrorFromDate_D.Visible = True
                                    imgErrorDate_D_Creation.Visible = True
                                End If
                            End If
                        End If
                    End If

                End If

            End If
        End If

        ' Check Period From boundary [End]

        ' Check Period To boundary [Start]
        If IsExistValue(Field.ToDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate_Creation, FieldSetting.Visible) = Condition.YES Then

                If txtToDate_D_Creation.Text <> String.Empty AndAlso blnToDateValid_Creation Then
                    ' Earliest Date
                    If IsExistValue(Field.ToDate_Creation, CustomFieldSetting.EarliestDate) Then
                        Dim strValidationDate As String = GetSetting(Field.ToDate_Creation, CustomFieldSetting.EarliestDate)
                        If Not strValidationDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                            Dim dtmFromDate As DateTime = StringToDateTime(txtToDate_D_Creation.Text)
                            If dtmFromDate.CompareTo(dtmValidationDate) < 0 Then
                                ' From date is visible, use From + To Date
                                If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
                                    If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES Then
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)) + " To")
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    Else
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    End If
                                End If

                                'imgErrorToDate_D.Visible = True
                                imgErrorDate_D_Creation.Visible = True
                            End If
                        End If
                    End If

                    ' Latest Date
                    If IsExistValue(Field.ToDate_Creation, CustomFieldSetting.LatestDate) Then
                        Dim strValidationDate As String = GetSetting(Field.ToDate_Creation, CustomFieldSetting.LatestDate)
                        If Not strValidationDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                            Dim dtmFromDate As DateTime = StringToDateTime(txtToDate_D_Creation.Text)
                            If dtmFromDate.CompareTo(dtmValidationDate) > 0 Then
                                ' From date is visible, use From + To Date
                                If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
                                    If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES Then
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)) + " To")
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    Else
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    End If
                                End If

                                'imgErrorToDate_D.Visible = True
                                imgErrorDate_D_Creation.Visible = True
                            End If
                        End If
                    End If

                    ' Allow Pass Date
                    If IsExistValue(Field.ToDate_Creation, CustomFieldSetting.AllowPastDate) Then
                        Dim strAllowPassDate As String = GetSetting(Field.ToDate_Creation, CustomFieldSetting.AllowPastDate)
                        If Not strAllowPassDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                            Dim dtmFromDate As DateTime = StringToDateTime(txtToDate_D_Creation.Text)

                            ' If allow pass date value is N, do checking
                            If strAllowPassDate = Condition.NO Then
                                If dtmFromDate.CompareTo(dtmValidationDate) < 0 Then
                                    ' From date is visible, use From + To Date
                                    If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
                                        If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00008))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)) + " To")
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00008))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        End If
                                    End If

                                    'imgErrorToDate_D.Visible = True
                                    imgErrorDate_D_Creation.Visible = True
                                End If
                            End If
                        End If
                    End If

                    ' Allow Future Date
                    If IsExistValue(Field.ToDate_Creation, CustomFieldSetting.AllowFutureDate) Then
                        Dim strAllowFutureDate As String = GetSetting(Field.ToDate_Creation, CustomFieldSetting.AllowFutureDate)
                        If Not strAllowFutureDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                            Dim dtmFromDate As DateTime = StringToDateTime(txtToDate_D_Creation.Text)

                            ' If allow future date value is N, do checking
                            If strAllowFutureDate = Condition.NO Then
                                If dtmFromDate.CompareTo(dtmValidationDate) > 0 Then
                                    ' From date is visible, use From + To Date
                                    If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
                                        If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00009))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)) + " To")
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00009))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        End If
                                    End If

                                    'imgErrorToDate_D.Visible = True
                                    imgErrorDate_D_Creation.Visible = True
                                End If
                            End If
                        End If
                    End If

                    ' Allow cut off date
                    If IsExistValue(Field.ToDate_Creation, CustomFieldSetting.UseCutOffDate) Then
                        Dim strAllowCutOffDate As String = GetSetting(Field.ToDate_Creation, CustomFieldSetting.UseCutOffDate)
                        If Not strAllowCutOffDate Is String.Empty Then
                            Dim dtmCutOffDate As DateTime = (New StatisticsBLL).GetDataCutOffDate(StatisticsBLL.EnumDataCutOffID.dbEVS_Enquiry)
                            Dim dtmValidationDate As DateTime = New DateTime(dtmCutOffDate.Year, dtmCutOffDate.Month, dtmCutOffDate.Day)
                            Dim dtmToDate As DateTime = StringToDateTime(txtToDate_D_Creation.Text)

                            ' If allow cut off date value is Y, do checking
                            If strAllowCutOffDate = Condition.YES Then
                                If dtmToDate.CompareTo(dtmValidationDate) > 0 Then
                                    ' To date is visible, use From + To Date
                                    If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
                                        If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)) + " To")
                                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                        End If
                                    End If

                                    'imgErrorFromDate_D.Visible = True
                                    imgErrorDate_D_Creation.Visible = True
                                End If
                            End If
                        End If
                    End If

                End If

            End If
        End If

        ' Check Period To boundary [End]

        ' Check relation between Period From and Period To [Start]
        If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) AndAlso IsExistValue(Field.ToDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES AndAlso GetSetting(Field.ToDate_Creation, FieldSetting.Visible) = Condition.YES Then

                If blnFromDateValid_Creation AndAlso blnToDateValid_Creation Then
                    Dim dtmFromDate As New DateTime
                    Dim dtmToDate As New DateTime

                    dtmFromDate = StringToDateTime(txtFromDate_D_Creation.Text)
                    dtmToDate = StringToDateTime(txtToDate_D_Creation.Text)

                    ' Check: From Date cannot later than To Date
                    If dtmFromDate.CompareTo(dtmToDate) > 0 Then
                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)) + " From")
                        lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource)) + " To")
                        'lstErrorParam2.Add(String.Empty)

                        'imgErrorFromDate_D.Visible = True
                        'imgErrorToDate_D.Visible = True
                        imgErrorDate_D_Creation.Visible = True
                    End If
                End If

            End If
        End If
        ' Check relation between Period From and Period To [End]

        ' Field 2 - Exact date period (Account creation date) [End]

        ' Field 3 - Exact date period (As at) [Start]
        Dim blnFromDateMiss_AsAt As Boolean = False
        Dim blnToDateMiss_AsAt As Boolean = False

        Dim blnFromDateValid_AsAt As Boolean = True
        Dim blnToDateValid_AsAt As Boolean = True

        Dim blnMissInput_AsAt As Boolean = False
        Dim blnInvalid_AsAt As Boolean = False
        'Dim blnIncomplete As Boolean = False

        ' Set period error image
        SetPeriodAsAtErrorImageVisibility(False)

        ' Set input date format
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'txtFromDate_D_AsAt.Text = udtFormatter.formatDate(txtFromDate_D_AsAt.Text.Trim)
        'txtToDate_D_AsAt.Text = udtFormatter.formatDate(txtToDate_D_AsAt.Text.Trim)
        txtFromDate_D_AsAt.Text = udtFormatter.formatInputDate(txtFromDate_D_AsAt.Text.Trim)
        txtToDate_D_AsAt.Text = udtFormatter.formatInputDate(txtToDate_D_AsAt.Text.Trim)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                ' Cannot empty
                If txtFromDate_D_AsAt.Text = String.Empty Then
                    blnMissInput_AsAt = True
                    blnFromDateValid_AsAt = False

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003))
                    lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                    lstErrorParam2.Add(String.Empty)
                    'imgErrorFromDate_D.Visible = True
                    imgErrorDate_D_AsAt.Visible = True
                Else
                    ' Date validation
                    If Not IsDate(udtFormatter.convertDate(txtFromDate_D_AsAt.Text.Trim, String.Empty)) Then
                        blnInvalid_AsAt = True
                        blnFromDateValid_AsAt = False

                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00004))
                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                        lstErrorParam2.Add(String.Empty)
                        'imgErrorFromDate_D.Visible = True
                        imgErrorDate_D_AsAt.Visible = True
                    End If

                End If

            End If
        End If

        ' Period To
        'SetPeriodToErrorImageVisibility(False)

        If IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                ' Cannot empty
                If txtToDate_D_AsAt.Text = String.Empty Then
                    blnMissInput_AsAt = True
                    blnToDateValid_AsAt = False

                    If blnFromDateValid_AsAt = True Then
                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003))
                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                        lstErrorParam2.Add(String.Empty)
                        'imgErrorToDate_D.Visible = True
                        imgErrorDate_D_AsAt.Visible = True
                    End If
                Else
                    ' Date validation
                    If Not IsDate(udtFormatter.convertDate(txtToDate_D_AsAt.Text.Trim, String.Empty)) Then
                        blnInvalid_AsAt = True
                        blnToDateValid_AsAt = False

                        If blnFromDateValid_AsAt = True Then
                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00004))
                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                            lstErrorParam2.Add(String.Empty)
                            'imgErrorToDate_D.Visible = True
                            imgErrorDate_D_AsAt.Visible = True
                        End If
                    End If

                End If

            End If
        End If

        'If blnMissInput Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00023))
        '    lstErrorParam1.Add("Date Period")
        '    lstErrorParam2.Add(String.Empty)
        'End If

        'If blnInvalid Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00024))
        '    lstErrorParam1.Add("Date Period")
        '    lstErrorParam2.Add(String.Empty)
        'End If

        'If blnIncomplete Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00027))
        '    lstErrorParam1.Add("Date Period")
        '    lstErrorParam2.Add(String.Empty)
        'End If

        ' Check Period From boundary [Start]
        If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES Then

                If txtFromDate_D_AsAt.Text <> String.Empty AndAlso blnFromDateValid_AsAt Then
                    ' Earliest Date
                    If IsExistValue(Field.FromDate_AsAt, CustomFieldSetting.EarliestDate) Then
                        Dim strValidationDate As String = GetSetting(Field.FromDate_AsAt, CustomFieldSetting.EarliestDate)
                        If Not strValidationDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D_AsAt.Text)
                            If dtmFromDate.CompareTo(dtmValidationDate) < 0 Then
                                ' To date is visible, use From + To Date
                                If IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
                                    If GetSetting(Field.ToDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)) + " From")
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    Else
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    End If
                                End If

                                'imgErrorFromDate_D.Visible = True
                                imgErrorDate_D_AsAt.Visible = True
                            End If
                        End If
                    End If

                    ' Latest Date
                    If IsExistValue(Field.FromDate_AsAt, CustomFieldSetting.LatestDate) Then
                        Dim strValidationDate As String = GetSetting(Field.FromDate_AsAt, CustomFieldSetting.LatestDate)
                        If Not strValidationDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D_AsAt.Text)
                            If dtmFromDate.CompareTo(dtmValidationDate) > 0 Then
                                ' To date is visible, use From + To Date
                                If IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
                                    If GetSetting(Field.ToDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)) + " From")
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    Else
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    End If
                                End If

                                'imgErrorFromDate_D.Visible = True
                                imgErrorDate_D_AsAt.Visible = True
                            End If
                        End If
                    End If

                    ' Allow Pass Date
                    If IsExistValue(Field.FromDate_AsAt, CustomFieldSetting.AllowPastDate) Then
                        Dim strAllowPassDate As String = GetSetting(Field.FromDate_AsAt, CustomFieldSetting.AllowPastDate)
                        If Not strAllowPassDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D_AsAt.Text)

                            ' If allow pass date value is N, do checking
                            If strAllowPassDate = Condition.NO Then
                                If dtmFromDate.CompareTo(dtmValidationDate) < 0 Then
                                    ' To date is visible, use From + To Date
                                    If IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
                                        If GetSetting(Field.ToDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00008))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)) + " From")
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00008))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        End If
                                    End If

                                    'imgErrorFromDate_D.Visible = True
                                    imgErrorDate_D_AsAt.Visible = True
                                End If
                            End If
                        End If
                    End If

                    ' Allow Future Date
                    If IsExistValue(Field.FromDate_AsAt, CustomFieldSetting.AllowFutureDate) Then
                        Dim strAllowFutureDate As String = GetSetting(Field.FromDate_AsAt, CustomFieldSetting.AllowFutureDate)
                        If Not strAllowFutureDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D_AsAt.Text)

                            ' If allow future date value is N, do checking
                            If strAllowFutureDate = Condition.NO Then
                                If dtmFromDate.CompareTo(dtmValidationDate) > 0 Then
                                    ' To date is visible, use From + To Date
                                    If IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
                                        If GetSetting(Field.ToDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00009))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)) + " From")
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00009))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        End If
                                    End If

                                    'imgErrorFromDate_D.Visible = True
                                    imgErrorDate_D_AsAt.Visible = True
                                End If
                            End If
                        End If
                    End If

                    ' Allow cut off date
                    If IsExistValue(Field.FromDate_AsAt, CustomFieldSetting.UseCutOffDate) Then
                        Dim strAllowCutOffDate As String = GetSetting(Field.FromDate_AsAt, CustomFieldSetting.UseCutOffDate)
                        If Not strAllowCutOffDate Is String.Empty Then
                            Dim dtmCutOffDate As DateTime = (New StatisticsBLL).GetDataCutOffDate(StatisticsBLL.EnumDataCutOffID.dbEVS_Enquiry)
                            Dim dtmValidationDate As DateTime = New DateTime(dtmCutOffDate.Year, dtmCutOffDate.Month, dtmCutOffDate.Day)
                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D_AsAt.Text)

                            ' If allow cut off date value is Y, do checking
                            If strAllowCutOffDate = Condition.YES Then
                                If dtmFromDate.CompareTo(dtmValidationDate) > 0 Then
                                    ' To date is visible, use From + To Date
                                    If IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
                                        If GetSetting(Field.ToDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)) + " From")
                                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                        End If
                                    End If

                                    'imgErrorFromDate_D.Visible = True
                                    imgErrorDate_D_AsAt.Visible = True
                                End If
                            End If
                        End If
                    End If

                End If

            End If
        End If

        ' Check Period From boundary [End]

        ' Check Period To boundary [Start]
        If IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate_AsAt, FieldSetting.Visible) = Condition.YES Then

                If txtToDate_D_AsAt.Text <> String.Empty AndAlso blnToDateValid_AsAt Then
                    ' Earliest Date
                    If IsExistValue(Field.ToDate_AsAt, CustomFieldSetting.EarliestDate) Then
                        Dim strValidationDate As String = GetSetting(Field.ToDate_AsAt, CustomFieldSetting.EarliestDate)
                        If Not strValidationDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                            Dim dtmFromDate As DateTime = StringToDateTime(txtToDate_D_AsAt.Text)
                            If dtmFromDate.CompareTo(dtmValidationDate) < 0 Then
                                ' From date is visible, use From + To Date
                                If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
                                    If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)) + " To")
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    Else
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    End If
                                End If

                                'imgErrorToDate_D.Visible = True
                                imgErrorDate_D_AsAt.Visible = True
                            End If
                        End If
                    End If

                    ' Latest Date
                    If IsExistValue(Field.ToDate_AsAt, CustomFieldSetting.LatestDate) Then
                        Dim strValidationDate As String = GetSetting(Field.ToDate_AsAt, CustomFieldSetting.LatestDate)
                        If Not strValidationDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                            Dim dtmFromDate As DateTime = StringToDateTime(txtToDate_D_AsAt.Text)
                            If dtmFromDate.CompareTo(dtmValidationDate) > 0 Then
                                ' From date is visible, use From + To Date
                                If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
                                    If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)) + " To")
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    Else
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                                        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                    End If
                                End If

                                'imgErrorToDate_D.Visible = True
                                imgErrorDate_D_AsAt.Visible = True
                            End If
                        End If
                    End If

                    ' Allow Pass Date
                    If IsExistValue(Field.ToDate_AsAt, CustomFieldSetting.AllowPastDate) Then
                        Dim strAllowPassDate As String = GetSetting(Field.ToDate_AsAt, CustomFieldSetting.AllowPastDate)
                        If Not strAllowPassDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                            Dim dtmFromDate As DateTime = StringToDateTime(txtToDate_D_AsAt.Text)

                            ' If allow pass date value is N, do checking
                            If strAllowPassDate = Condition.NO Then
                                If dtmFromDate.CompareTo(dtmValidationDate) < 0 Then
                                    ' From date is visible, use From + To Date
                                    If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
                                        If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00008))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)) + " To")
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00008))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        End If
                                    End If

                                    'imgErrorToDate_D.Visible = True
                                    imgErrorDate_D_AsAt.Visible = True
                                End If
                            End If
                        End If
                    End If

                    ' Allow Future Date
                    If IsExistValue(Field.ToDate_AsAt, CustomFieldSetting.AllowFutureDate) Then
                        Dim strAllowFutureDate As String = GetSetting(Field.ToDate_AsAt, CustomFieldSetting.AllowFutureDate)
                        If Not strAllowFutureDate Is String.Empty Then
                            Dim dtmValidationDate As DateTime = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                            Dim dtmFromDate As DateTime = StringToDateTime(txtToDate_D_AsAt.Text)

                            ' If allow future date value is N, do checking
                            If strAllowFutureDate = Condition.NO Then
                                If dtmFromDate.CompareTo(dtmValidationDate) > 0 Then
                                    ' From date is visible, use From + To Date
                                    If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
                                        If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00009))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)) + " To")
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00009))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        End If
                                    End If

                                    'imgErrorToDate_D.Visible = True
                                    imgErrorDate_D_AsAt.Visible = True
                                End If
                            End If
                        End If
                    End If

                    ' Allow cut off date
                    If IsExistValue(Field.ToDate_AsAt, CustomFieldSetting.UseCutOffDate) Then
                        Dim strAllowCutOffDate As String = GetSetting(Field.ToDate_AsAt, CustomFieldSetting.UseCutOffDate)
                        If Not strAllowCutOffDate Is String.Empty Then
                            Dim dtmCutOffDate As DateTime = (New StatisticsBLL).GetDataCutOffDate(StatisticsBLL.EnumDataCutOffID.dbEVS_Enquiry)
                            Dim dtmValidationDate As DateTime = New DateTime(dtmCutOffDate.Year, dtmCutOffDate.Month, dtmCutOffDate.Day)
                            Dim dtmToDate As DateTime = StringToDateTime(txtToDate_D_AsAt.Text)

                            ' If allow cut off date value is Y, do checking
                            If strAllowCutOffDate = Condition.YES Then
                                If dtmToDate.CompareTo(dtmValidationDate) > 0 Then
                                    ' To date is visible, use From + To Date
                                    If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
                                        If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)) + " To")
                                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                        End If
                                    End If

                                    'imgErrorFromDate_D.Visible = True
                                    imgErrorDate_D_AsAt.Visible = True
                                End If
                            End If
                        End If
                    End If

                End If

            End If
        End If

        ' Check Period To boundary [End]

        ' Check relation between Period From and Period To [Start]
        If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) AndAlso IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES AndAlso GetSetting(Field.ToDate_AsAt, FieldSetting.Visible) = Condition.YES Then

                If blnFromDateValid_AsAt AndAlso blnToDateValid_AsAt Then
                    Dim dtmFromDate As New DateTime
                    Dim dtmToDate As New DateTime

                    dtmFromDate = StringToDateTime(txtFromDate_D_AsAt.Text)
                    dtmToDate = StringToDateTime(txtToDate_D_AsAt.Text)

                    ' Check: From Date cannot later than To Date
                    If dtmFromDate.CompareTo(dtmToDate) > 0 Then
                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)) + " From")
                        lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource)) + " To")
                        'lstErrorParam2.Add(String.Empty)

                        'imgErrorFromDate_D.Visible = True
                        'imgErrorToDate_D.Visible = True
                        imgErrorDate_D_AsAt.Visible = True
                    End If
                End If

            End If
        End If
        ' Check relation between Period From and Period To [End]
        ' Field 3 - Exact date period (As at) [End]


        ' Field 4 - Age range [Start]
        Dim blnMinAgeMiss_Age As Boolean = False
        Dim blnMaxAgeMiss_Age As Boolean = False

        Dim blnMinAgeValid_Age As Boolean = True
        Dim blnMaxAgeValid_Age As Boolean = True

        Dim blnMissInput_Age As Boolean = False
        Dim blnInvalid_Age As Boolean = False
        Dim blnIncomplete_Age As Boolean = False

        'SetMinAgeErrorImageVisibility(False)
        'SetMaxAgeErrorImageVisibility(False)
        SetAgeErrorImageVisibility(False)

        ' Check both are empty
        If IsExistValue(Field.MinAge, FieldSetting.Visible) AndAlso IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES AndAlso GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then

                If txtMinAge.Text.Trim = String.Empty AndAlso txtMaxAge.Text.Trim = String.Empty Then
                    'lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00007))
                    'lstErrorParam1.Add(GetSetting(Field.MinAge, FieldSetting.DescResource))
                    'lstErrorParam2.Add(GetSetting(Field.MaxAge, FieldSetting.DescResource))
                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003))
                    lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource)))
                    lstErrorParam2.Add(String.Empty)

                    blnMinAgeValid_Age = False
                    blnMaxAgeValid_Age = False

                    'imgErrorMinAge.Visible = True
                    'imgErrorMaxAge.Visible = True
                    imgErrorAge.Visible = True
                End If

            End If
        End If

        ' Min age
        If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Then
                ' Cannot empty (If max age is invisible)
                If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
                    If GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.NO Then
                        If txtMinAge.Text.Trim = String.Empty Then
                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003))
                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource)))
                            lstErrorParam2.Add(String.Empty)

                            blnMissInput_Age = True
                            blnMinAgeValid_Age = False
                            imgErrorAge.Visible = True
                        End If
                    End If
                End If

            End If
        End If

        ' Max age
        If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then
                ' Cannot empty (if min age is invisible)
                If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
                    If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.NO Then
                        If txtMaxAge.Text.Trim = String.Empty Then
                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003))
                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MaxAge, FieldSetting.DescResource)))
                            lstErrorParam2.Add(String.Empty)

                            blnMissInput_Age = True
                            blnMaxAgeValid_Age = False
                            imgErrorAge.Visible = True
                        End If
                    End If
                End If

            End If
        End If

        'If blnMissInput Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00023))
        '    lstErrorParam1.Add("Age range")
        '    lstErrorParam2.Add(String.Empty)
        'End If

        'If blnInvalid Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00024))
        '    lstErrorParam1.Add("Age range")
        '    lstErrorParam2.Add(String.Empty)
        'End If

        'If blnIncomplete Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00027))
        '    lstErrorParam1.Add("Age range")
        '    lstErrorParam2.Add(String.Empty)
        'End If

        ' Check min age boundary [Start]
        If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Then

                If txtMinAge.Text <> String.Empty AndAlso blnMinAgeValid_Age Then
                    ' Range min
                    If IsExistValue(Field.MinAge, CustomFieldSetting.RangeMin) Then
                        Dim strRangeMin As String = GetSetting(Field.MinAge, CustomFieldSetting.RangeMin)
                        If Not strRangeMin Is String.Empty Then
                            Dim intValidationMinAge As Integer = CType(strRangeMin, Integer)

                            Dim intInputRangeMin As Integer = CType(txtMinAge.Text.Trim, Integer)
                            If intInputRangeMin.CompareTo(intValidationMinAge) < 0 Then
                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00010))
                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource)))
                                lstErrorParam2.Add(intValidationMinAge.ToString)
                                'imgErrorMinAge.Visible = True
                                imgErrorAge.Visible = True
                            End If
                        End If
                    End If

                    ' Range max
                    If IsExistValue(Field.MinAge, CustomFieldSetting.RangeMax) Then
                        Dim strRangeMax As String = GetSetting(Field.MinAge, CustomFieldSetting.RangeMax)
                        If Not strRangeMax Is String.Empty Then
                            Dim intValidationMaxAge As Integer = CType(strRangeMax, Integer)

                            Dim intInputRangeMax As Integer = CType(txtMinAge.Text.Trim, Integer)
                            If intInputRangeMax.CompareTo(intValidationMaxAge) > 0 Then
                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00011))
                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource)))
                                lstErrorParam2.Add(intValidationMaxAge.ToString)
                                'imgErrorMinAge.Visible = True
                                imgErrorAge.Visible = True
                            End If
                        End If
                    End If

                End If

            End If
        End If
        ' Check min age boundary [End]

        ' Check max age boundary [Start]
        If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then

                If txtMaxAge.Text <> String.Empty AndAlso blnMaxAgeValid_Age Then
                    ' Range min
                    If IsExistValue(Field.MaxAge, CustomFieldSetting.RangeMin) Then
                        Dim strRangeMin As String = GetSetting(Field.MaxAge, CustomFieldSetting.RangeMin)
                        If Not strRangeMin Is String.Empty Then
                            Dim intValidationMinAge As Integer = CType(strRangeMin, Integer)

                            Dim intInputRangeMin As Integer = CType(txtMaxAge.Text.Trim, Integer)
                            If intInputRangeMin.CompareTo(intValidationMinAge) < 0 Then
                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00010))
                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MaxAge, FieldSetting.DescResource)))
                                lstErrorParam2.Add(intValidationMinAge.ToString)
                                'imgErrorMaxAge.Visible = True
                                imgErrorAge.Visible = True
                            End If
                        End If
                    End If

                    ' Range max
                    If IsExistValue(Field.MaxAge, CustomFieldSetting.RangeMax) Then
                        Dim strRangeMax As String = GetSetting(Field.MaxAge, CustomFieldSetting.RangeMax)
                        If Not strRangeMax Is String.Empty Then
                            Dim intValidationMaxAge As Integer = CType(strRangeMax, Integer)

                            Dim intInputRangeMax As Integer = CType(txtMaxAge.Text.Trim, Integer)
                            If intInputRangeMax.CompareTo(intValidationMaxAge) > 0 Then
                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00011))
                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MaxAge, FieldSetting.DescResource)))
                                lstErrorParam2.Add(intValidationMaxAge.ToString)
                                'imgErrorMaxAge.Visible = True
                                imgErrorAge.Visible = True
                            End If
                        End If
                    End If

                End If

            End If
        End If
        ' Check max age boundary [End]

        ' Check relation between min age and max age [Start]
        If IsExistValue(Field.MinAge, FieldSetting.Visible) AndAlso IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES AndAlso GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then

                If blnMinAgeValid_Age AndAlso blnMaxAgeValid_Age Then
                    Dim intMinAge As Integer = 0
                    Dim intMaxAge As Integer = 0

                    ' Min age
                    If Not txtMinAge.Text.Trim Is String.Empty Then
                        intMinAge = CType(txtMinAge.Text.Trim, Integer)
                    End If

                    ' Max age
                    If Not txtMaxAge.Text.Trim Is String.Empty Then
                        intMaxAge = CType(txtMaxAge.Text.Trim, Integer)
                    End If

                    ' Check: Min age cannot later than Max age (if two value have input)
                    If Not txtMinAge.Text.Trim Is String.Empty AndAlso Not txtMaxAge.Text.Trim Is String.Empty Then
                        If intMinAge.CompareTo(intMaxAge) > 0 Then
                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00011))
                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource)) + " From")
                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource)) + " To")

                            'imgErrorMinAge.Visible = True
                            'imgErrorMaxAge.Visible = True
                            imgErrorAge.Visible = True
                        End If
                    End If

                End If

            End If
        End If
        ' Check relation between min age and max age [End]
        ' Field 4 - Age range [End]

        ' Field 5 - TransSubisdy (Big control) [Start]
        'SetComponentErrorVisibility(False)

        ' Scheme
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                ' Check dropdown (Scheme)
                If ddlScheme.SelectedValue.Trim = DROP_DOWN_EMPTY Then
                    Dim strSchemeText As String = String.Empty

                    If IsExistValue(Field.Scheme, FieldSetting.DescResource) Then
                        strSchemeText = Me.GetGlobalResourceObject("Text", GetSetting(Field.Scheme, FieldSetting.DescResource))
                    Else
                        strSchemeText = Me.GetGlobalResourceObject("Text", "Scheme")
                    End If

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                    lstErrorParam1.Add(strSchemeText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorScheme.Visible = True
                End If

            End If
        End If

        ' Type of counting item
        If IsExistValue(Field.CountingItem, FieldSetting.Visible) Then
            If GetSetting(Field.CountingItem, FieldSetting.Visible) = Condition.YES Then
                ' Check dropdown (type of counting item)
                If ddlTypeOfCount.SelectedValue.Trim = DROP_DOWN_EMPTY Then
                    Dim strTypeOfCountText As String = String.Empty

                    If IsExistValue(Field.CountingItem, FieldSetting.DescResource) Then
                        strTypeOfCountText = Me.GetGlobalResourceObject("Text", GetSetting(Field.CountingItem, FieldSetting.DescResource))
                    Else
                        strTypeOfCountText = Me.GetGlobalResourceObject("Text", "CountingItem")
                    End If

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                    lstErrorParam1.Add(strTypeOfCountText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorTypeOfCount.Visible = True
                End If

            End If
        End If

        ' Subsidy (do checking if type of counting item is selected and item = Subsidy)
        If ddlTypeOfCount.SelectedValue.Trim = TypeOfCountingItem.Subsidy Then

            If IsExistValue(Field.Subsidy, FieldSetting.Visible) Then
                If GetSetting(Field.Subsidy, FieldSetting.Visible) = Condition.YES Then
                    ' Check dropdown (subsidy)
                    If ddlSubsidy.SelectedValue.Trim = DROP_DOWN_EMPTY Then
                        Dim strSubsidyText As String = String.Empty

                        If IsExistValue(Field.Subsidy, FieldSetting.DescResource) Then
                            strSubsidyText = Me.GetGlobalResourceObject("Text", GetSetting(Field.Subsidy, FieldSetting.DescResource))
                        Else
                            strSubsidyText = Me.GetGlobalResourceObject("Text", "Subsidy")
                        End If

                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                        lstErrorParam1.Add(strSubsidyText)
                        lstErrorParam2.Add(String.Empty)
                        imgErrorSubsidy.Visible = True
                    End If

                End If
            End If

        End If

        '' Type of break down
        'If IsExistValue(Field.BreakDownType, FieldSetting.Visible) Then
        '    If GetSetting(Field.BreakDownType, FieldSetting.Visible) = Condition.YES Then
        '        ' Check dropdown (Type of break down)
        '        If ddlTypeOfBreakDown.SelectedValue.Trim = DROP_DOWN_EMPTY Then
        '            Dim strTypeOfBreakDownText As String = String.Empty

        '            If IsExistValue(Field.BreakDownType, FieldSetting.DescResource) Then
        '                strTypeOfBreakDownText = Me.GetGlobalResourceObject("Text", GetSetting(Field.BreakDownType, FieldSetting.DescResource))
        '            Else
        '                strTypeOfBreakDownText = Me.GetGlobalResourceObject("Text", "BreakDownType")
        '            End If

        '            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
        '            lstErrorParam1.Add(strTypeOfBreakDownText)
        '            lstErrorParam2.Add(String.Empty)
        '            imgErrorTypeOfBreakDown.Visible = True
        '        End If

        '    End If
        'End If

        ' District (do checking if type of break down is selected and item = profession)
        If ddlTypeOfBreakDown.SelectedValue.Trim = TypeOfBreakDown.Profession Then

            If IsExistValue(Field.District, FieldSetting.Visible) Then
                If GetSetting(Field.District, FieldSetting.Visible) = Condition.YES Then
                    ' Check checkbox list (District)
                    If CheckDistrictHasSelectedValue() = False Then
                        Dim strDistrictText As String = String.Empty

                        If IsExistValue(Field.District, FieldSetting.DescResource) Then
                            strDistrictText = Me.GetGlobalResourceObject("Text", GetSetting(Field.District, FieldSetting.DescResource))
                        Else
                            strDistrictText = Me.GetGlobalResourceObject("Text", "District")
                        End If

                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                        lstErrorParam1.Add(strDistrictText)
                        lstErrorParam2.Add(String.Empty)
                        imgErrorDistrict.Visible = True
                    End If
                End If
            End If

        End If

        ' Profession (do checking if type of break down is selected and item = district)
        If ddlTypeOfBreakDown.SelectedValue.Trim = TypeOfBreakDown.District Then

            If IsExistValue(Field.Profession, FieldSetting.Visible) Then
                If GetSetting(Field.Profession, FieldSetting.Visible) = Condition.YES Then
                    ' Check checkbox list (Profession)
                    If CheckProfessionHasSelectedValue() = False Then
                        Dim strProfessionText As String = String.Empty

                        If IsExistValue(Field.Profession, FieldSetting.DescResource) Then
                            strProfessionText = Me.GetGlobalResourceObject("Text", GetSetting(Field.Profession, FieldSetting.DescResource))
                        Else
                            strProfessionText = Me.GetGlobalResourceObject("Text", "HealthProf")
                        End If

                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                        lstErrorParam1.Add(strProfessionText)
                        lstErrorParam2.Add(String.Empty)
                        imgErrorProfession.Visible = True
                    End If
                End If
            End If

        End If

        ' Field 5 - TransSubsidy (Big control) [End]



    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Field 1 - Period break down [Start]
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.Visible) Then
            If GetSetting(Field.PeriodBreakDown, FieldSetting.Visible) = Condition.YES Then
                Dim strPeriodBreakDown As String = lblPeriodBreakDown.Text.Trim

                If ddlPeriodBreakDown.SelectedIndex = 0 Then
                    udtParameterList.AddParam(strPeriodBreakDown, String.Empty)
                Else
                    udtParameterList.AddParam(strPeriodBreakDown, ddlPeriodBreakDown.SelectedValue.ToString.Trim)
                End If
            End If
        End If
        ' Field 1 - Period break down [End]

        ' Type of Breakdown
        If IsExistValue(Field.BreakDownType, FieldSetting.Visible) Then
            If GetSetting(Field.BreakDownType, FieldSetting.Visible) = Condition.YES Then
                Dim strTypeOfBreakdown As String = lblTypeOfBreakDown.Text.Trim
                Dim strTypeOfBreakdownValue As String = String.Empty
                'udtParameterList.AddParam(strTypeOfBreakdown, ddlTypeOfBreakDown.SelectedItem.Text.Trim)

                Select Case ddlTypeOfBreakDown.SelectedValue.Trim
                    Case TypeOfBreakDown.Profession
                        strTypeOfBreakdownValue += ddlTypeOfBreakDown.SelectedValue.ToString.Trim
                    Case TypeOfBreakDown.District
                        strTypeOfBreakdownValue += ddlTypeOfBreakDown.SelectedValue.ToString.Trim
                    Case Else
                        ' Nothing
                        strTypeOfBreakdownValue += ""
                End Select

                udtParameterList.AddParam(strTypeOfBreakdown, strTypeOfBreakdownValue)
            End If
        End If

        ' Field 2 - Exact date period (Account creation date) [Start]
        Dim strDatePeriod_Creation As String = ""

        ' From Date
        If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES Then
                Dim strFromDateLabel As String = lblFromDate_Creation.Text.Trim
                If txtFromDate_D_Creation.Text.Trim.Length = 0 Then
                    strDatePeriod_Creation += ""
                Else
                    strDatePeriod_Creation += txtFromDate_D_Creation.Text.Trim
                End If
            End If
        End If


        ' To Date (Pass value if visibility of To Date is True)
        If IsExistValue(Field.ToDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate_Creation, FieldSetting.Visible) = Condition.YES Then
                Dim strToDateLabel As String = lblToDate_Creation.Text.Trim

                strDatePeriod_Creation += " to "

                If txtToDate_D_Creation.Text.Trim.Length = 0 Then
                    strDatePeriod_Creation += ""
                Else
                    strDatePeriod_Creation += txtToDate_D_Creation.Text.Trim
                End If
            End If
        End If

        If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES Then
                Dim strDatePeriodLabel As String = String.Empty
                If IsExistValue(Field.FromDate_Creation, FieldSetting.DescResource) Then
                    strDatePeriodLabel = Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource))
                Else
                    strDatePeriodLabel = Me.GetGlobalResourceObject("Text", "DatePeriod")
                End If
                udtParameterList.AddParam(strDatePeriodLabel, strDatePeriod_Creation)
            End If
        End If
        ' Field 2 - Exact date period (Account creation date) [End]

        ' Field 3 - Exact date period (As at) [Start]
        Dim strDatePeriod_AsAt As String = ""

        ' From Date
        If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                Dim strFromDateLabel As String = lblFromDate_AsAt.Text.Trim
                If txtFromDate_D_AsAt.Text.Trim.Length = 0 Then
                    strDatePeriod_AsAt += ""
                Else
                    strDatePeriod_AsAt += txtFromDate_D_AsAt.Text.Trim
                End If
            End If
        End If


        ' To Date (Pass value if visibility of To Date is True)
        If IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                Dim strToDateLabel As String = lblToDate_AsAt.Text.Trim

                strDatePeriod_AsAt += " to "

                If txtToDate_D_AsAt.Text.Trim.Length = 0 Then
                    strDatePeriod_AsAt += ""
                Else
                    strDatePeriod_AsAt += txtToDate_D_AsAt.Text.Trim
                End If
            End If
        End If

        If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                Dim strDatePeriodLabel As String = String.Empty
                If IsExistValue(Field.FromDate_AsAt, FieldSetting.DescResource) Then
                    strDatePeriodLabel = Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource))
                Else
                    strDatePeriodLabel = Me.GetGlobalResourceObject("Text", "DatePeriod")
                End If
                udtParameterList.AddParam(strDatePeriodLabel, strDatePeriod_AsAt)
            End If
        End If
        ' Field 3 - Exact date period (As at) [End]

        ' Field 4 - Age range [Start]
        Dim strAgeRange As String = String.Empty

        ' Min age
        If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Then
                If txtMinAge.Text.Trim = String.Empty Then
                    strAgeRange += ""
                Else
                    strAgeRange += CType(txtMinAge.Text.Trim, Integer).ToString
                End If
            End If
        End If

        ' Max age
        If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then
                strAgeRange += " to "

                If txtMaxAge.Text.Trim = String.Empty Then
                    strAgeRange += ""
                Else
                    strAgeRange += CType(txtMaxAge.Text.Trim, Integer).ToString
                End If

            End If
        End If

        If IsExistValue(Field.MinAge, FieldSetting.Visible) Or IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Or GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then

                Dim strAgeRangeLabel As String = Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource))
                udtParameterList.AddParam(strAgeRangeLabel, strAgeRange)

            End If
        End If
        ' Field 4 - Age range [End]

        ' Field 5 - TransSubsidy (Big control) [Start]
        ' Scheme (Can configure visibility)
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                Dim strSchemeLabel As String = lblScheme.Text.Trim
                If ddlScheme.SelectedIndex = 0 Then
                    udtParameterList.AddParam(strSchemeLabel, String.Empty)
                Else
                    udtParameterList.AddParam(strSchemeLabel, ddlScheme.SelectedValue.ToString.Trim)
                End If
            End If
        End If

        ' Type of Counting Item (Can configure visibility)
        If IsExistValue(Field.CountingItem, FieldSetting.Visible) Then
            If GetSetting(Field.CountingItem, FieldSetting.Visible) = Condition.YES Then
                Dim strTypeOfCountLabel As String = lblTypeOfCount.Text.Trim
                If ddlTypeOfCount.SelectedIndex = 0 Then
                    udtParameterList.AddParam(strTypeOfCountLabel, String.Empty)
                Else
                    udtParameterList.AddParam(strTypeOfCountLabel, ddlTypeOfCount.SelectedValue.ToString.Trim)
                End If
            End If
        End If

        ' Subsidy (Pass value if counting item equal to Subsidy)
        If ddlTypeOfCount.SelectedValue = TypeOfCountingItem.Subsidy Then
            Dim strSubsidyLabel As String = lblSubsidy.Text.Trim
            If ddlSubsidy.SelectedIndex = 0 Then
                udtParameterList.AddParam(strSubsidyLabel, String.Empty)
            Else
                udtParameterList.AddParam(strSubsidyLabel, ddlSubsidy.SelectedValue.ToString.Trim)
            End If
        End If

        ' District (Pass value if type of breakdown value equal to Health Profession)
        If ddlTypeOfBreakDown.SelectedValue = TypeOfBreakDown.Profession Then
            Dim strDistrictLabel As String = lblDistrict.Text.Trim
            Dim strChkBoxItemString As String = String.Empty

            Select Case rbtnDistrictType.SelectedIndex
                Case MultiSelectionTypeEnum.Any
                    strChkBoxItemString += "Any"

                Case MultiSelectionTypeEnum.Specific
                    ' District checkboxlist item
                    For Each boxItem As ListItem In chkDistrict.Items
                        If boxItem.Selected = True Then
                            strChkBoxItemString += String.Format("{0}{1}", boxItem.Value.ToString.Trim, ",")
                        End If
                    Next
                    strChkBoxItemString = strChkBoxItemString.Substring(0, strChkBoxItemString.Length - 1)

                Case Else
                    ' Nothing
                    strChkBoxItemString += ""
            End Select

            udtParameterList.AddParam(strDistrictLabel, strChkBoxItemString)
        End If

        ' Health Profession (Pass value if type of breakdown value equal to District)
        If ddlTypeOfBreakDown.SelectedValue = TypeOfBreakDown.District Then
            Dim strProfessionLabel As String = lblProfession.Text.Trim
            Dim strChkBoxItemString As String = String.Empty

            Select Case rbtnProfessionType.SelectedIndex
                Case MultiSelectionTypeEnum.Any
                    strChkBoxItemString += "Any"

                Case MultiSelectionTypeEnum.Specific
                    ' Health Profession checkboxlist item
                    For Each boxItem As ListItem In chkProfession.Items
                        If boxItem.Selected = True Then
                            strChkBoxItemString += String.Format("{0}{1}", boxItem.Value.ToString.Trim, ",")
                        End If
                    Next
                    strChkBoxItemString = strChkBoxItemString.Substring(0, strChkBoxItemString.Length - 1)

                Case Else
                    ' Nothing
                    strChkBoxItemString += ""
            End Select

            udtParameterList.AddParam(strProfessionLabel, strChkBoxItemString)
        End If

        ' Field 5 - TransSubsidy (Big control) [End]

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Field 1 - Period break down [Start]
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.Visible) Then
            If GetSetting(Field.PeriodBreakDown, FieldSetting.Visible) = Condition.YES Then
                Dim strPeriodBreakDown As String = lblPeriodBreakDown.Text.Trim
                udtParameterList.AddParam(strPeriodBreakDown, ddlPeriodBreakDown.SelectedItem.Text.Trim)
            End If
        End If
        ' Field 1 - Period break down [End]

        ' Type of Breakdown
        If IsExistValue(Field.BreakDownType, FieldSetting.Visible) Then
            If GetSetting(Field.BreakDownType, FieldSetting.Visible) = Condition.YES Then
                Dim strTypeOfBreakdown As String = lblTypeOfBreakDown.Text.Trim
                'udtParameterList.AddParam(strTypeOfBreakdown, ddlTypeOfBreakDown.SelectedItem.Text.Trim)

                Dim paraObjectWithLegend As New ParameterObjectWithLegend(strTypeOfBreakdown, ddlTypeOfBreakDown.SelectedItem.Text.Trim)

                Select Case ddlTypeOfBreakDown.SelectedValue.Trim
                    Case TypeOfBreakDown.Profession
                        paraObjectWithLegend.ParamLegendType = LegendType.Profession
                    Case TypeOfBreakDown.District
                        paraObjectWithLegend.ParamLegendType = LegendType.District
                    Case Else
                        ' Nothing
                End Select

                udtParameterList.AddParam(paraObjectWithLegend)
            End If
        End If

        ' Field 2 - Exact date period (Account creation date) [Start]
        Dim strDatePeriod_Creation As String = ""

        ' From Date
        If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES Then
                Dim strFromDateLabel As String = lblFromDate_Creation.Text.Trim
                If txtFromDate_D_Creation.Text.Trim.Length = 0 Then
                    strDatePeriod_Creation += "---"
                Else
                    strDatePeriod_Creation += txtFromDate_D_Creation.Text.Trim
                End If
            End If
        End If


        ' To Date (Pass value if visibility of To Date is True)
        If IsExistValue(Field.ToDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate_Creation, FieldSetting.Visible) = Condition.YES Then
                Dim strToDateLabel As String = lblToDate_Creation.Text.Trim

                strDatePeriod_Creation += " to "

                If txtToDate_D_Creation.Text.Trim.Length = 0 Then
                    strDatePeriod_Creation += "---"
                Else
                    strDatePeriod_Creation += txtToDate_D_Creation.Text.Trim
                End If
            End If
        End If

        If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES Then
                Dim strDatePeriodLabel As String = String.Empty
                If IsExistValue(Field.FromDate_Creation, FieldSetting.DescResource) Then
                    strDatePeriodLabel = Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource))
                Else
                    strDatePeriodLabel = Me.GetGlobalResourceObject("Text", "DatePeriod")
                End If
                udtParameterList.AddParam(strDatePeriodLabel, strDatePeriod_Creation)
            End If
        End If
        ' Field 2 - Exact date period (Account creation date) [End]

        ' Field 3 - Exact date period (As at) [Start]
        Dim strDatePeriod_AsAt As String = ""

        ' From Date
        If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                Dim strFromDateLabel As String = lblFromDate_AsAt.Text.Trim
                If txtFromDate_D_AsAt.Text.Trim.Length = 0 Then
                    strDatePeriod_AsAt += "---"
                Else
                    strDatePeriod_AsAt += txtFromDate_D_AsAt.Text.Trim
                End If
            End If
        End If


        ' To Date (Pass value if visibility of To Date is True)
        If IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                Dim strToDateLabel As String = lblToDate_AsAt.Text.Trim

                strDatePeriod_AsAt += " to "

                If txtToDate_D_AsAt.Text.Trim.Length = 0 Then
                    strDatePeriod_AsAt += "---"
                Else
                    strDatePeriod_AsAt += txtToDate_D_AsAt.Text.Trim
                End If
            End If
        End If

        If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES Then
                Dim strDatePeriodLabel As String = String.Empty
                If IsExistValue(Field.FromDate_AsAt, FieldSetting.DescResource) Then
                    strDatePeriodLabel = Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource))
                Else
                    strDatePeriodLabel = Me.GetGlobalResourceObject("Text", "DatePeriod")
                End If
                udtParameterList.AddParam(strDatePeriodLabel, strDatePeriod_AsAt)
            End If
        End If
        ' Field 3 - Exact date period (As at) [End]

        ' Field 4 - Age range [Start]
        Dim strAgeRange As String = String.Empty

        ' Min age
        If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Then
                If txtMinAge.Text.Trim = String.Empty Then
                    strAgeRange += "0"
                Else
                    strAgeRange += CType(txtMinAge.Text.Trim, Integer).ToString
                End If
            End If
        End If

        ' Max age
        If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then
                strAgeRange += " to "

                If txtMaxAge.Text.Trim = String.Empty Then
                    strAgeRange += "Max Age"
                Else
                    strAgeRange += CType(txtMaxAge.Text.Trim, Integer).ToString
                End If

            End If
        End If

        If IsExistValue(Field.MinAge, FieldSetting.Visible) Or IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Or GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then

                Dim strAgeRangeLabel As String = Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource))
                udtParameterList.AddParam(strAgeRangeLabel, strAgeRange)

            End If
        End If
        ' Field 4 - Age range [End]

        ' Field 5 - TransSubsidy (Big control) [Start]
        ' Scheme (Can configure visibility)
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                Dim strSchemeLabel As String = lblScheme.Text.Trim
                udtParameterList.AddParam(strSchemeLabel, ddlScheme.SelectedItem.Text.Trim)
            End If
        End If

        ' Type of Counting Item (Can configure visibility)
        If IsExistValue(Field.CountingItem, FieldSetting.Visible) Then
            If GetSetting(Field.CountingItem, FieldSetting.Visible) = Condition.YES Then
                Dim strTypeOfCountLabel As String = lblTypeOfCount.Text.Trim
                udtParameterList.AddParam(strTypeOfCountLabel, ddlTypeOfCount.SelectedItem.Text.Trim)
            End If
        End If

        ' Subsidy (Pass value if counting item equal to Subsidy)
        If ddlTypeOfCount.SelectedValue = TypeOfCountingItem.Subsidy Then
            Dim strSubsidyLabel As String = lblSubsidy.Text.Trim
            udtParameterList.AddParam(strSubsidyLabel, ddlSubsidy.SelectedItem.Text.Trim)
        End If

        ' District (Pass value if type of breakdown value equal to Health Profession)
        If ddlTypeOfBreakDown.SelectedValue = TypeOfBreakDown.Profession Then
            Dim strDistrictLabel As String = lblDistrict.Text.Trim
            Dim strChkBoxItemString As String = String.Empty

            Select Case rbtnDistrictType.SelectedIndex
                Case MultiSelectionTypeEnum.Any
                    strChkBoxItemString += "Any"
                    udtParameterList.AddParam(strDistrictLabel, strChkBoxItemString)

                Case MultiSelectionTypeEnum.Specific
                    ' District checkboxlist item
                    'strChkBoxItemString += "<ul style='padding-left: 15px; margin: 0px'>"
                    'For Each boxItem As ListItem In chkDistrict.Items
                    '    If boxItem.Selected = True Then
                    '        strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                    '    End If
                    'Next
                    'strChkBoxItemString += "</ul>"

                    ' District checkboxlist item
                    Dim listParam As New ParameterObjectList(strDistrictLabel)
                    For Each boxItem As ListItem In chkDistrict.Items
                        If boxItem.Selected = True Then
                            listParam.ParamValueList.Add(boxItem.Text.ToString.Trim)
                        End If
                    Next

                    udtParameterList.AddParam(listParam)

                Case Else
                    ' Nothing
            End Select

            'udtParameterList.AddParam(strDistrictLabel, strChkBoxItemString)
        End If

        ' Health Profession (Pass value if type of breakdown value equal to District)
        If ddlTypeOfBreakDown.SelectedValue = TypeOfBreakDown.District Then
            Dim strProfessionLabel As String = lblProfession.Text.Trim
            Dim strChkBoxItemString As String = String.Empty

            Select Case rbtnProfessionType.SelectedIndex
                Case MultiSelectionTypeEnum.Any
                    strChkBoxItemString += "Any"
                    udtParameterList.AddParam(strProfessionLabel, strChkBoxItemString)

                Case MultiSelectionTypeEnum.Specific
                    ' Health Profession checkboxlist item
                    'strChkBoxItemString += "<ul style='padding-left: 15px; margin: 0px'>"
                    'For Each boxItem As ListItem In chkProfession.Items
                    '    If boxItem.Selected = True Then
                    '        strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                    '    End If
                    'Next
                    'strChkBoxItemString += "</ul>"

                    ' Health Profession checkboxlist item
                    Dim listParam As New ParameterObjectList(strProfessionLabel)
                    For Each boxItem As ListItem In chkProfession.Items
                        If boxItem.Selected = True Then
                            listParam.ParamValueList.Add(boxItem.Text.ToString.Trim)
                        End If
                    Next

                    udtParameterList.AddParam(listParam)

                Case Else
                    ' Nothing
            End Select

            'udtParameterList.AddParam(strProfessionLabel, strChkBoxItemString)
        End If

        ' Field 5 - TransSubsidy (Big control) [End]

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' Field 1 - Period break down [Start]
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.Visible) Then
            If GetSetting(Field.PeriodBreakDown, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.PeriodBreakDown, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty

                    strSPParamName = GetSetting(Field.PeriodBreakDown, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 1, ddlPeriodBreakDown.SelectedValue.Trim)
                End If

            ElseIf GetSetting(Field.PeriodBreakDown, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.PeriodBreakDown, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.PeriodBreakDown, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.PeriodBreakDown, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.PeriodBreakDown, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.PeriodBreakDown, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 1, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If
        ' Field 1 - Period break down [End]

        ' Field 2 - Exact date period (Account creation date) [Start]
        ' Period From
        If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.YES Then

                Dim dtmPeriodFrom As DateTime = StringToDateTime(txtFromDate_D_Creation.Text)
                If IsExistValue(Field.FromDate_Creation, FieldSetting.SPParamName) Then
                    Dim strParamPeriodFrom As String = String.Empty
                    strParamPeriodFrom = GetSetting(Field.FromDate_Creation, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strParamPeriodFrom, System.Data.SqlDbType.DateTime, 8, dtmPeriodFrom)
                End If

            ElseIf GetSetting(Field.FromDate_Creation, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.FromDate_Creation, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.FromDate_Creation, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.FromDate_Creation, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As DateTime
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = StringToDateTime(GetSetting(Field.FromDate_Creation, FieldSetting.DefaultValue))
                            strSPParamName = GetSetting(Field.FromDate_Creation, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.DateTime, 8, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        ' Period To
        If IsExistValue(Field.ToDate_Creation, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate_Creation, FieldSetting.Visible) = Condition.YES Then

                Dim dtmPeriodTo As DateTime = StringToDateTime(txtToDate_D_Creation.Text)
                If IsExistValue(Field.ToDate_Creation, FieldSetting.SPParamName) Then
                    Dim strParamPeriodTo As String = String.Empty
                    strParamPeriodTo = GetSetting(Field.ToDate_Creation, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strParamPeriodTo, System.Data.SqlDbType.DateTime, 8, dtmPeriodTo)
                End If

            ElseIf GetSetting(Field.ToDate_Creation, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.ToDate_Creation, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.ToDate_Creation, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.ToDate_Creation, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As DateTime
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = StringToDateTime(GetSetting(Field.ToDate_Creation, FieldSetting.DefaultValue))
                            strSPParamName = GetSetting(Field.ToDate_Creation, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.DateTime, 8, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If
        ' Field 2 - Exact date period (Account creation date) [End]

        ' Field 3 - Exact date period (As at) [Start]
        ' Period From
        If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.YES Then

                Dim dtmPeriodFrom As DateTime = StringToDateTime(txtFromDate_D_AsAt.Text)
                If IsExistValue(Field.FromDate_AsAt, FieldSetting.SPParamName) Then
                    Dim strParamPeriodFrom As String = String.Empty
                    strParamPeriodFrom = GetSetting(Field.FromDate_AsAt, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strParamPeriodFrom, System.Data.SqlDbType.DateTime, 8, dtmPeriodFrom)
                End If

            ElseIf GetSetting(Field.FromDate_AsAt, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.FromDate_AsAt, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.FromDate_AsAt, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.FromDate_AsAt, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As DateTime
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = StringToDateTime(GetSetting(Field.FromDate_AsAt, FieldSetting.DefaultValue))
                            strSPParamName = GetSetting(Field.FromDate_AsAt, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.DateTime, 8, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        ' Period To
        If IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate_AsAt, FieldSetting.Visible) = Condition.YES Then

                Dim dtmPeriodTo As DateTime = StringToDateTime(txtToDate_D_AsAt.Text)
                If IsExistValue(Field.ToDate_AsAt, FieldSetting.SPParamName) Then
                    Dim strParamPeriodTo As String = String.Empty
                    strParamPeriodTo = GetSetting(Field.ToDate_AsAt, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strParamPeriodTo, System.Data.SqlDbType.DateTime, 8, dtmPeriodTo)
                End If

            ElseIf GetSetting(Field.ToDate_AsAt, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.ToDate_AsAt, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.ToDate_AsAt, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.ToDate_AsAt, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As DateTime
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = StringToDateTime(GetSetting(Field.ToDate_AsAt, FieldSetting.DefaultValue))
                            strSPParamName = GetSetting(Field.ToDate_AsAt, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.DateTime, 8, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If
        ' Field 3 - Exact date period (As at) [End]

        ' Field 4 - Age range [Start]
        ' Min age
        If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Then

                Dim intMinAge As Integer = 0
                If IsExistValue(Field.MinAge, FieldSetting.SPParamName) Then
                    Dim strParamMinAge As String = String.Empty
                    strParamMinAge = GetSetting(Field.MinAge, FieldSetting.SPParamName)

                    If txtMinAge.Text.Trim Is String.Empty Then
                        udtStoreProcParamList.AddParam(strParamMinAge, System.Data.SqlDbType.Int, 2, intMinAge)
                    Else
                        intMinAge = CType(txtMinAge.Text.Trim, Integer)
                        udtStoreProcParamList.AddParam(strParamMinAge, System.Data.SqlDbType.Int, 2, intMinAge)
                    End If

                End If

            ElseIf GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.MinAge, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.MinAge, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.MinAge, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As Integer = 0
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = CType(GetSetting(Field.MinAge, FieldSetting.DefaultValue), Integer)
                            strSPParamName = GetSetting(Field.MinAge, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Int, 2, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        ' Max age
        If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then

                Dim intMaxAge As Object
                If IsExistValue(Field.MaxAge, FieldSetting.SPParamName) Then
                    Dim strParamMaxAge As String = String.Empty
                    strParamMaxAge = GetSetting(Field.MaxAge, FieldSetting.SPParamName)

                    If txtMaxAge.Text.Trim Is String.Empty Then
                        udtStoreProcParamList.AddParam(strParamMaxAge, System.Data.SqlDbType.Int, 2, Nothing)
                    Else
                        intMaxAge = CType(txtMaxAge.Text.Trim, Integer)
                        udtStoreProcParamList.AddParam(strParamMaxAge, System.Data.SqlDbType.Int, 2, intMaxAge)
                    End If

                End If

            ElseIf GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.MaxAge, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.MaxAge, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.MaxAge, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As Integer = 0
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = CType(GetSetting(Field.MaxAge, FieldSetting.DefaultValue), Integer)
                            strSPParamName = GetSetting(Field.MaxAge, FieldSetting.SPParamName)

                            If strDefaultValue > 999 Then
                                udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 2, Nothing)
                            Else
                                udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 2, CType(strDefaultValue, Integer))
                            End If

                        End If

                    End If
                End If

            End If
        End If
        ' Field 4 - Age range [End]

        ' Field 5 - TransSubsidy (Big control) [Start]
        ' Scheme
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.Scheme, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty

                    strSPParamName = GetSetting(Field.Scheme, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, ddlScheme.SelectedValue.Trim)
                End If

            ElseIf GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.Scheme, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.Scheme, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.Scheme, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.Scheme, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.Scheme, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If


        ' Type of counting item
        If IsExistValue(Field.CountingItem, FieldSetting.Visible) Then
            If GetSetting(Field.CountingItem, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.CountingItem, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty

                    strSPParamName = GetSetting(Field.CountingItem, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, ddlTypeOfCount.SelectedValue.Trim)
                End If

            ElseIf GetSetting(Field.CountingItem, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.CountingItem, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.CountingItem, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.CountingItem, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.CountingItem, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.CountingItem, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        ' Subsidy Case 1 (pass value if type of counting item is selected and value = Subsidy, and Subsidy in XML is Y)
        If ddlTypeOfCount.SelectedValue.Trim = TypeOfCountingItem.Subsidy Then
            If IsExistValue(Field.Subsidy, FieldSetting.Visible) Then
                If GetSetting(Field.Subsidy, FieldSetting.Visible) = Condition.YES Then

                    If IsExistValue(Field.Subsidy, FieldSetting.SPParamName) Then
                        Dim strSPParamName As String = String.Empty

                        strSPParamName = GetSetting(Field.Subsidy, FieldSetting.SPParamName)
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, ddlSubsidy.SelectedValue.Trim)
                    End If

                ElseIf GetSetting(Field.Subsidy, FieldSetting.Visible) = Condition.NO Then

                    If IsExistValue(Field.Subsidy, FieldSetting.DefaultValue) Then
                        If Not GetSetting(Field.Subsidy, FieldSetting.DefaultValue) Is String.Empty Then

                            If IsExistValue(Field.Subsidy, FieldSetting.SPParamName) Then
                                Dim strDefaultValue As String = String.Empty
                                Dim strSPParamName As String = String.Empty

                                strDefaultValue = GetSetting(Field.Subsidy, FieldSetting.DefaultValue)
                                strSPParamName = GetSetting(Field.Subsidy, FieldSetting.SPParamName)
                                udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, strDefaultValue)
                            End If

                        End If
                    End If

                End If
            End If
        End If

        ' Subsidy Case 2 (pass value if type of counting item is selected and value = Transaction, and Subsidy in XML is Y)
        If ddlTypeOfCount.SelectedValue.Trim = TypeOfCountingItem.Transaction Then
            If IsExistValue(Field.Subsidy, FieldSetting.Visible) Then
                If GetSetting(Field.Subsidy, FieldSetting.Visible) = Condition.YES Then

                    If IsExistValue(Field.Subsidy, FieldSetting.SPParamName) Then
                        Dim strSPParamName As String = String.Empty

                        strSPParamName = GetSetting(Field.Subsidy, FieldSetting.SPParamName)
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, "")
                    End If

                ElseIf GetSetting(Field.Subsidy, FieldSetting.Visible) = Condition.NO Then

                    If IsExistValue(Field.Subsidy, FieldSetting.DefaultValue) Then
                        If Not GetSetting(Field.Subsidy, FieldSetting.DefaultValue) Is String.Empty Then

                            If IsExistValue(Field.Subsidy, FieldSetting.SPParamName) Then
                                Dim strDefaultValue As String = String.Empty
                                Dim strSPParamName As String = String.Empty

                                strDefaultValue = GetSetting(Field.Subsidy, FieldSetting.DefaultValue)
                                strSPParamName = GetSetting(Field.Subsidy, FieldSetting.SPParamName)
                                udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, strDefaultValue)
                            End If

                        End If
                    End If

                End If
            End If
        End If


        ' Type of break down
        If IsExistValue(Field.BreakDownType, FieldSetting.Visible) Then
            If GetSetting(Field.BreakDownType, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.BreakDownType, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty

                    strSPParamName = GetSetting(Field.BreakDownType, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 1, ddlTypeOfBreakDown.SelectedValue.Trim)
                End If

            ElseIf GetSetting(Field.BreakDownType, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.BreakDownType, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.BreakDownType, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.BreakDownType, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.BreakDownType, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.BreakDownType, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 1, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        ' District (pass value if type of break down is selected and value = Profession)
        If ddlTypeOfBreakDown.SelectedValue.Trim = TypeOfBreakDown.Profession Then
            If IsExistValue(Field.District, FieldSetting.Visible) Then
                If GetSetting(Field.District, FieldSetting.Visible) = Condition.YES Then

                    If IsExistValue(Field.District, FieldSetting.SPParamName) Then
                        Dim strSPParamName As String = String.Empty
                        strSPParamName = GetSetting(Field.District, FieldSetting.SPParamName)

                        If rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.Any Then
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, Nothing)
                        Else
                            Dim strPassValue As String = String.Empty

                            For Each boxItem As ListItem In chkDistrict.Items
                                If boxItem.Selected = True Then
                                    strPassValue += boxItem.Value.ToString.Trim + ","
                                End If
                            Next

                            strPassValue = strPassValue.Substring(0, strPassValue.Length - 1)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, strPassValue)
                        End If

                    End If

                ElseIf GetSetting(Field.District, FieldSetting.Visible) = Condition.NO Then

                    If IsExistValue(Field.District, FieldSetting.DefaultValue) Then
                        If Not GetSetting(Field.District, FieldSetting.DefaultValue) Is String.Empty Then

                            If IsExistValue(Field.District, FieldSetting.SPParamName) Then
                                Dim strDefaultValue As String = String.Empty
                                Dim strSPParamName As String = String.Empty

                                strDefaultValue = GetSetting(Field.District, FieldSetting.DefaultValue)
                                strSPParamName = GetSetting(Field.District, FieldSetting.SPParamName)
                                udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, strDefaultValue)
                            End If

                        End If
                    End If

                End If
            End If
        End If

        ' Profession (pass value if type of break down is selected and value = District)
        If ddlTypeOfBreakDown.SelectedValue.Trim = TypeOfBreakDown.District Then
            If IsExistValue(Field.Profession, FieldSetting.Visible) Then
                If GetSetting(Field.Profession, FieldSetting.Visible) = Condition.YES Then

                    If IsExistValue(Field.Profession, FieldSetting.SPParamName) Then
                        Dim strSPParamName As String = String.Empty
                        strSPParamName = GetSetting(Field.Profession, FieldSetting.SPParamName)

                        If rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Any Then
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, Nothing)
                        Else
                            Dim strPassValue As String = String.Empty

                            For Each boxItem As ListItem In chkProfession.Items
                                If boxItem.Selected = True Then
                                    strPassValue += boxItem.Value.ToString.Trim + ","
                                End If
                            Next

                            strPassValue = strPassValue.Substring(0, strPassValue.Length - 1)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, strPassValue)
                        End If

                    End If

                ElseIf GetSetting(Field.Profession, FieldSetting.Visible) = Condition.NO Then

                    If IsExistValue(Field.Profession, FieldSetting.DefaultValue) Then
                        If Not GetSetting(Field.Profession, FieldSetting.DefaultValue) Is String.Empty Then

                            If IsExistValue(Field.Profession, FieldSetting.SPParamName) Then
                                Dim strDefaultValue As String = String.Empty
                                Dim strSPParamName As String = String.Empty

                                strDefaultValue = GetSetting(Field.Profession, FieldSetting.DefaultValue)
                                strSPParamName = GetSetting(Field.Profession, FieldSetting.SPParamName)
                                udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, strDefaultValue)
                            End If

                        End If
                    End If

                End If
            End If
        End If

        ' Field 5 - TransSubsidy (Big control) [End]


        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        SetErrorTypeOfDateVisibility(blnVisible)
        SetPeriodCreationErrorImageVisibility(blnVisible)
        SetPeriodAsAtErrorImageVisibility(blnVisible)
        SetAgeErrorImageVisibility(blnVisible)
    End Sub

#End Region

#Region "Supporting Function"

    Protected Overrides Sub BuildPeriodBreakDownComponent(ByVal ddlComponent As DropDownList)
        MyBase.BuildPeriodBreakDownComponent(ddlComponent)
    End Sub

    Protected Overrides Sub BuildSchemeComponent(ByVal ddlComponent As DropDownList)
        MyBase.BuildSchemeComponent(ddlComponent)
    End Sub

    Protected Overrides Sub BuildTypeOfCountComponent(ByVal ddlComponent As DropDownList)
        MyBase.BuildTypeOfCountComponent(ddlComponent)
    End Sub

    Protected Overrides Sub BuildSubsidyComponent(ByVal ddlComponent As DropDownList)
        MyBase.BuildSubsidyComponent(ddlComponent)
    End Sub

    Protected Overrides Sub BuildTypeOfBreakDownComponent(ByVal ddlComponent As DropDownList)
        MyBase.BuildTypeOfBreakDownComponent(ddlComponent)
    End Sub

    Protected Overrides Sub BuildDistrictComponent(ByVal cboxListComponent As CheckBoxList, ByVal strFieldID As String)
        MyBase.BuildDistrictComponent(cboxListComponent, strFieldID)
    End Sub

    Protected Overrides Sub BuildProfessionComponent(ByVal ddlSchemeComponent As DropDownList, ByVal cboxListComponent As CheckBoxList, ByVal strFieldID As String)
        ' 2 case
        ' Profession with scheme (Get profession data with criteria: Scheme)
        ' Profession only (Get all profession data)
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                MyBase.BuildProfessionComponent(ddlSchemeComponent, cboxListComponent, strFieldID)
            Else
                ' If scheme has default value case
                If IsExistValue(Field.Scheme, FieldSetting.DefaultValue) Then
                    If GetSetting(Field.Scheme, FieldSetting.DefaultValue) <> String.Empty Then
                        ' Case Any, bind get all profession data
                        If GetSetting(Field.Scheme, FieldSetting.DefaultValue) = "ANY" Then
                            MyBase.BuildProfessionComponent(cboxListComponent, strFieldID)
                        Else
                            ' Case other, set ddlScheme selected value and get related data
                            Dim strDefaultValue As String = GetSetting(Field.Scheme, FieldSetting.DefaultValue)
                            'Dim listItem As ListItem = chkProfession.Items.FindByValue(valueItem)
                            Dim listItem As ListItem = ddlSchemeComponent.Items.FindByValue(strDefaultValue)
                            If Not listItem Is Nothing Then
                                listItem.Selected = True
                            End If

                            MyBase.BuildProfessionComponent(ddlSchemeComponent, cboxListComponent, strFieldID)
                        End If
                    Else
                        MyBase.BuildProfessionComponent(cboxListComponent, strFieldID)
                    End If
                Else
                    MyBase.BuildProfessionComponent(cboxListComponent, strFieldID)
                End If
                ' Exit (If scheme has default value case)
            End If
            ' Exit (If Scheme visibility is Yes)
        Else
            ' No scheme.Visible, then get all profession data
            MyBase.BuildProfessionComponent(cboxListComponent, strFieldID)

        End If

    End Sub

    Public Overrides Sub InitControl()
        ' Field 1 - Period Break Down (Type of Statistic) [Start]
        ' Set item - Period break down    
        SetPeriodBreakDown()
        ' Field 1 - Period Break Down (Type of Statistic) [End]


        ' Field 2 - Exact date period (Account creation date) [Start]
        ' Set item - From Date
        SetFromDate_Creation()
        ' Set item - To Date
        SetToDate_Creation()
        ' Set item - Exact Date Period Panel
        SetExactDatePeriodPanel_Creation()
        ' Field 2 - Exact date period (Account creation date) [End]

        ' Field 3 - Exact date period (As at) [Start]
        ' Set item - From Date
        SetFromDate_AsAt()
        ' Set item - To Date
        SetToDate_AsAt()
        ' Set item - Exact Date Period Panel
        SetExactDatePeriodPanel_AsAt()
        ' Field 3 - Exact date period (As at) [End]

        ' Field 4 - Age range [Start]
        ' Set item - Min Age
        SetMinAge()
        ' Set item - Max Age
        SetMaxAge()
        ' Field 4 - Age range [End]

        ' Field 5 - TransSubsidy (Big control) [Start]
        ' Set item - Scheme    
        SetScheme()
        ' Set item - Type of Counting Item
        SetTypeOfCountingItem()
        ' Set item - Subsidy
        SetSubsidy()
        ' Set item - Type of BreakDown
        SetTypeOfBreakDown()
        ' Set item - District
        SetDistrict()
        ' Set item - Health Profession
        SetHealthProfession()

        ' Field 5 - TransSubsidy (Big control) [End]

    End Sub

    Private Function StringToDateTime(ByVal strDate As String, ByVal strTime As String) As DateTime
        Return StringToDateTime(String.Format("{0} {1}", strDate.Trim, strTime.Trim))
    End Function

    Private Function StringToDateTime(ByVal strValue As String) As DateTime
        strValue = strValue.Trim

        Select Case strValue.Length
            Case 10
                ' dd-MM-yyyy
                Return (New Formatter).convertDate(strValue, String.Empty)
            Case 16
                ' dd-MM-yyyy HH:mm
                Return String.Format("{0} {1}", (New Formatter).convertDate(strValue.Substring(0, 10), String.Empty), strValue.Substring(11, 5))
            Case Else
                Throw New Exception(String.Format("reportCriteriaPeriodFromToDate.StringToDateTime: Unexpected value: {0}", strValue))
        End Select

        Return Nothing

    End Function

    Private Function DateToString(ByVal dtmValue As DateTime) As String
        Return (New Formatter).convertDateTime(dtmValue, String.Empty)
    End Function

    Private Function ValidateTime(ByVal strTime As String) As Boolean
        If strTime.Length <> 5 Then Return False

        ' Hour
        Dim intHour As Integer = CInt(strTime.Substring(0, 2))
        If intHour < 0 OrElse intHour >= 24 Then Return False

        ' Minute
        Dim intMin As Integer = CInt(strTime.Substring(3, 2))
        If intMin < 0 OrElse intMin >= 60 Then Return False

        Return True

    End Function

    Private Sub ResetProfessionSelection()
        BuildProfessionComponent(ddlScheme, chkProfession, Field.Profession)

        ViewState(VS.Profession) = Nothing

        rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.NoSelection

        ' Profession checkbox list item
        Dim strChkBoxItemString As String = String.Empty

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkProfession.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
            End If
        Next
        strChkBoxItemString += "</ul>"

        lblAddProfessionDisplay.Text = strChkBoxItemString
    End Sub

    Private Sub ResetDistrictSelection()
        BuildDistrictComponent(chkDistrict, Field.District)

        ViewState(VS.District) = Nothing

        rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.NoSelection

        ' District checkbox list item
        Dim strChkBoxItemString As String = String.Empty

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkDistrict.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
            End If
        Next
        strChkBoxItemString += "</ul>"

        lblAddDistrictDisplay.Text = strChkBoxItemString
    End Sub

    Private Function CheckDistrictHasSelectedValue() As Boolean
        Dim blnHasSelectedValue As Boolean = False

        If rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.Any Then
            blnHasSelectedValue = True
        Else
            For Each boxItem As ListItem In chkDistrict.Items
                If boxItem.Selected = True Then
                    blnHasSelectedValue = True
                    Exit For
                End If
            Next
        End If

        Return blnHasSelectedValue
    End Function

    Private Function CheckProfessionHasSelectedValue() As Boolean
        Dim blnHasSelectedValue As Boolean = False

        If rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Any Then
            blnHasSelectedValue = True
        Else
            For Each boxItem As ListItem In chkProfession.Items
                If boxItem.Selected = True Then
                    blnHasSelectedValue = True
                    Exit For
                End If
            Next
        End If

        Return blnHasSelectedValue
    End Function

    Private Sub SetComponentErrorVisibility(ByVal blnVisible As Boolean)
        imgErrorScheme.Visible = blnVisible
        imgErrorTypeOfCount.Visible = blnVisible
        imgErrorSubsidy.Visible = blnVisible
        imgErrorTypeOfBreakDown.Visible = blnVisible
        imgErrorDistrict.Visible = blnVisible
        imgErrorProfession.Visible = blnVisible
    End Sub

    Private Sub AddDistrictIntoList()
        Dim items As New Dictionary(Of String, Boolean)

        ' District checkboxlist item
        For Each boxItem As ListItem In chkDistrict.Items
            items.Add(boxItem.Value.ToString.Trim, boxItem.Selected)
        Next

        ViewState(VS.District) = items

        ' District checkboxlist item

        Dim strChkBoxItemString As String = String.Empty
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkDistrict.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        ' If all items are selected
        'If intIsSelectedCount = chkDistrict.Items.Count Then
        '    rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.Any
        '    strChkBoxItemString = String.Empty
        'Else
        '    ' Have selected items
        '    If intIsSelectedCount > 0 Then
        '        rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.Specific
        '    Else
        '        rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        '    End If

        'End If

        ' Have selected items
        If intIsSelectedCount > 0 Then
            rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.Specific
        Else
            rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        End If

        lblAddDistrictDisplay.Text = strChkBoxItemString

    End Sub

    Private Sub AddProfessionIntoList()
        Dim items As New Dictionary(Of String, Boolean)

        ' Profession checkboxlist item
        For Each boxItem As ListItem In chkProfession.Items
            items.Add(boxItem.Value.ToString.Trim, boxItem.Selected)
        Next

        ViewState(VS.Profession) = items

        ' Health profession checkboxlist item

        Dim strChkBoxItemString As String = String.Empty
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkProfession.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        ' If all items are selected
        'If intIsSelectedCount = chkProfession.Items.Count Then
        '    rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Any
        '    strChkBoxItemString = String.Empty
        'Else
        '    ' Have selected items
        '    If intIsSelectedCount > 0 Then
        '        rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Specific
        '    Else
        '        rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        '    End If

        'End If

        ' Have selected items
        If intIsSelectedCount > 0 Then
            rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Specific
        Else
            rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        End If

        lblAddProfessionDisplay.Text = strChkBoxItemString

    End Sub

    Private Sub SetDistrictSelectionToAny()
        'BuildProfessionComponent(ddlScheme, chkProfession, Field.Profession)
        For Each boxItem As ListItem In chkDistrict.Items
            boxItem.Selected = False
        Next

        ViewState(VS.District) = Nothing

        rbtnDistrictType.SelectedIndex = MultiSelectionTypeEnum.Any

        ' District checkbox list item
        Dim strChkBoxItemString As String = String.Empty

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkDistrict.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
            End If
        Next
        strChkBoxItemString += "</ul>"

        lblAddDistrictDisplay.Text = strChkBoxItemString
    End Sub

    Private Sub SetProfessionSelectionToAny()
        'BuildProfessionComponent(ddlScheme, chkProfession, Field.Profession)
        For Each boxItem As ListItem In chkProfession.Items
            boxItem.Selected = False
        Next

        ViewState(VS.Profession) = Nothing

        rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Any

        ' Profession checkbox list item
        Dim strChkBoxItemString As String = String.Empty

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkProfession.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
            End If
        Next
        strChkBoxItemString += "</ul>"

        lblAddProfessionDisplay.Text = strChkBoxItemString
    End Sub

    Private Sub SetErrorTypeOfDateVisibility(ByVal blnVisible As Boolean)
        imgErrorPeriodBreakDown.Visible = blnVisible
    End Sub

    ' Set Period error image visibility
    Private Sub SetPeriodCreationErrorImageVisibility(ByVal blnVisible As Boolean)
        imgErrorDate_D_Creation.Visible = blnVisible
    End Sub

    ' Set Period error image visibility
    Private Sub SetPeriodAsAtErrorImageVisibility(ByVal blnVisible As Boolean)
        imgErrorDate_D_AsAt.Visible = blnVisible
    End Sub

    ' Set Age error image visibility
    Private Sub SetAgeErrorImageVisibility(ByVal blnVisible As Boolean)
        imgErrorAge.Visible = blnVisible
    End Sub

#End Region

#Region "Fields Setting"

    ' Field 1 - Period break down (Type of statistic) [Start]
    ' Set item - Period break down
    Private Sub SetPeriodBreakDown()
        ' Set field description
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.DescResource) Then
            lblPeriodBreakDown.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.PeriodBreakDown, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.Visible) Then
            Select Case GetSetting(Field.PeriodBreakDown, FieldSetting.Visible)
                Case Condition.YES
                    panPeriodBreakDown.Visible = True
                Case Condition.NO
                    panPeriodBreakDown.Visible = False
                Case Else
                    panPeriodBreakDown.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.PeriodBreakDown, FieldSetting.DefaultValue) = String.Empty Then

                Dim listItem As ListItem = ddlPeriodBreakDown.Items.FindByValue(GetSetting(Field.PeriodBreakDown, FieldSetting.DefaultValue))
                If Not listItem Is Nothing Then
                    ddlPeriodBreakDown.SelectedValue = listItem.Value
                End If

            End If
        End If

    End Sub

    ' Field 2 - Exact date period (Account creation date) [Start]
    ' Set item - From Date
    Private Sub SetFromDate_Creation()
        ' Set field description
        If IsExistValue(Field.FromDate_Creation, FieldSetting.DescResource) Then
            lblFromDate_Creation.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_Creation, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
            Select Case GetSetting(Field.FromDate_Creation, FieldSetting.Visible)
                Case Condition.YES
                    panFromDate_Creation.Visible = True
                Case Condition.NO
                    panFromDate_Creation.Visible = False
                Case Else
                    panFromDate_Creation.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.FromDate_Creation, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.FromDate_Creation, FieldSetting.DefaultValue) = String.Empty Then
                Dim udtFormatter As New Formatter
                Dim dtmFromDate As DateTime = CType(GetSetting(Field.FromDate_Creation, FieldSetting.DefaultValue), DateTime)
                txtFromDate_D_Creation.Text = dtmFromDate.ToString(udtFormatter.EnterDateFormat)
            End If
        End If

    End Sub

    ' Field 2 - Exact date period (Account creation date) [Start]
    ' Set item - To Date
    Private Sub SetToDate_Creation()
        ' Set field description
        If IsExistValue(Field.ToDate_Creation, FieldSetting.DescResource) Then
            lblToDate_Creation.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.ToDate_Creation, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.ToDate_Creation, FieldSetting.Visible) Then
            Select Case GetSetting(Field.ToDate_Creation, FieldSetting.Visible)
                Case Condition.YES
                    panToDate_Creation.Visible = True
                Case Condition.NO
                    panToDate_Creation.Visible = False
                Case Else
                    panToDate_Creation.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.ToDate_Creation, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.ToDate_Creation, FieldSetting.DefaultValue) = String.Empty Then
                Dim udtFormatter As New Formatter
                Dim dtmToDate As DateTime = CType(GetSetting(Field.ToDate_Creation, FieldSetting.DefaultValue), DateTime)
                txtToDate_D_Creation.Text = dtmToDate.ToString(udtFormatter.EnterDateFormat)
            End If
        End If

    End Sub

    ' Field 2 - Exact date period (Account creation date) [Start]
    ' Set item - Exact Date Period Panel
    Private Sub SetExactDatePeriodPanel_Creation()
        Dim strFromDateVisible As String = Condition.NO
        Dim strToDateVisible As String = Condition.NO

        ' Get From Date visibility
        If IsExistValue(Field.FromDate_Creation, FieldSetting.Visible) Then
            strFromDateVisible = GetSetting(Field.FromDate_Creation, FieldSetting.Visible)
        End If

        ' Get To Date visibility
        If IsExistValue(Field.ToDate_Creation, FieldSetting.Visible) Then
            strToDateVisible = GetSetting(Field.ToDate_Creation, FieldSetting.Visible)
        End If

        ' Set Exact Date Period Panel visibility
        If strFromDateVisible = Condition.NO AndAlso strToDateVisible = Condition.NO Then
            panExactDatePeriod_Creation.Visible = False
        Else
            panExactDatePeriod_Creation.Visible = True
        End If

    End Sub

    ' Field 3 - Exact date period (As at) [Start]
    ' Set item - From Date
    Private Sub SetFromDate_AsAt()
        ' Set field description
        If IsExistValue(Field.FromDate_AsAt, FieldSetting.DescResource) Then
            lblFromDate_AsAt.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate_AsAt, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
            Select Case GetSetting(Field.FromDate_AsAt, FieldSetting.Visible)
                Case Condition.YES
                    panFromDate_AsAt.Visible = True
                Case Condition.NO
                    panFromDate_AsAt.Visible = False
                Case Else
                    panFromDate_AsAt.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.FromDate_AsAt, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.FromDate_AsAt, FieldSetting.DefaultValue) = String.Empty Then
                Dim udtFormatter As New Formatter
                Dim dtmFromDate As DateTime = CType(GetSetting(Field.FromDate_AsAt, FieldSetting.DefaultValue), DateTime)
                txtFromDate_D_AsAt.Text = dtmFromDate.ToString(udtFormatter.EnterDateFormat)
            End If
        End If

    End Sub

    ' Field 3 - Exact date period (As at) [Start]
    ' Set item - To Date
    Private Sub SetToDate_AsAt()
        ' Set field description
        If IsExistValue(Field.ToDate_AsAt, FieldSetting.DescResource) Then
            lblToDate_AsAt.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.ToDate_AsAt, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
            Select Case GetSetting(Field.ToDate_AsAt, FieldSetting.Visible)
                Case Condition.YES
                    panToDate_AsAt.Visible = True
                Case Condition.NO
                    panToDate_AsAt.Visible = False
                Case Else
                    panToDate_AsAt.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.ToDate_AsAt, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.ToDate_AsAt, FieldSetting.DefaultValue) = String.Empty Then
                Dim udtFormatter As New Formatter
                Dim dtmToDate As DateTime = CType(GetSetting(Field.ToDate_AsAt, FieldSetting.DefaultValue), DateTime)
                txtToDate_D_AsAt.Text = dtmToDate.ToString(udtFormatter.EnterDateFormat)
            End If
        End If

    End Sub

    ' Field 3 - Exact date period (As at) [Start]
    ' Set item - Exact Date Period Panel
    Private Sub SetExactDatePeriodPanel_AsAt()
        Dim strFromDateVisible As String = Condition.NO
        Dim strToDateVisible As String = Condition.NO

        ' Get From Date visibility
        If IsExistValue(Field.FromDate_AsAt, FieldSetting.Visible) Then
            strFromDateVisible = GetSetting(Field.FromDate_AsAt, FieldSetting.Visible)
        End If

        ' Get To Date visibility
        If IsExistValue(Field.ToDate_AsAt, FieldSetting.Visible) Then
            strToDateVisible = GetSetting(Field.ToDate_AsAt, FieldSetting.Visible)
        End If

        ' Set Exact Date Period Panel visibility
        If strFromDateVisible = Condition.NO AndAlso strToDateVisible = Condition.NO Then
            panExactDatePeriod_AsAt.Visible = False
        Else
            panExactDatePeriod_AsAt.Visible = True
        End If

    End Sub

    ' Field 4 - Age range [Start]
    ' Set item - Min Age 
    Private Sub SetMinAge()
        ' Set field description
        If IsExistValue(Field.MinAge, FieldSetting.DescResource) Then
            lblMinAge.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
            Select Case GetSetting(Field.MinAge, FieldSetting.Visible)
                Case Condition.YES
                    panMinAge.Visible = True
                Case Condition.NO
                    panMinAge.Visible = False
                Case Else
                    panMinAge.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.MinAge, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.MinAge, FieldSetting.DefaultValue) = String.Empty Then
                txtMinAge.Text = GetSetting(Field.MinAge, FieldSetting.DefaultValue)
            End If
        End If

    End Sub

    ' Field 4 - Age range [Start]
    ' Set item - Max Age
    Private Sub SetMaxAge()
        ' Set field description
        If IsExistValue(Field.MaxAge, FieldSetting.DescResource) Then
            lblMaxAge.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.MaxAge, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            Select Case GetSetting(Field.MaxAge, FieldSetting.Visible)
                Case Condition.YES
                    panMaxAge.Visible = True
                Case Condition.NO
                    panMaxAge.Visible = False
                Case Else
                    panMaxAge.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.MaxAge, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.MaxAge, FieldSetting.DefaultValue) = String.Empty Then
                txtMaxAge.Text = GetSetting(Field.MaxAge, FieldSetting.DefaultValue)
            End If
        End If

    End Sub

    ' Field 5 - TransSubsidy (Big control) [Start]
    ' Set item - Scheme
    Private Sub SetScheme()
        ' Set field description
        If IsExistValue(Field.Scheme, FieldSetting.DescResource) Then
            lblScheme.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.Scheme, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            Select Case GetSetting(Field.Scheme, FieldSetting.Visible)
                Case Condition.YES
                    panScheme.Visible = True
                Case Condition.NO
                    panScheme.Visible = False
                Case Else
                    panScheme.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.Scheme, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.Scheme, FieldSetting.DefaultValue) = String.Empty Then

                Dim listItem As ListItem = ddlScheme.Items.FindByValue(GetSetting(Field.Scheme, FieldSetting.DefaultValue))
                If Not listItem Is Nothing Then
                    ddlScheme.SelectedValue = listItem.Value
                End If

            End If
        End If

    End Sub

    ' Field 5 - TransSubsidy (Big control) [Start]
    ' Set item - Type of Counting Item
    Private Sub SetTypeOfCountingItem()
        ' Set field description
        If IsExistValue(Field.CountingItem, FieldSetting.DescResource) Then
            lblTypeOfCount.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.CountingItem, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.CountingItem, FieldSetting.Visible) Then
            Select Case GetSetting(Field.CountingItem, FieldSetting.Visible)
                Case Condition.YES
                    panTypeOfCount.Visible = True
                Case Condition.NO
                    panTypeOfCount.Visible = False
                Case Else
                    panTypeOfCount.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.CountingItem, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.CountingItem, FieldSetting.DefaultValue) = String.Empty Then

                Dim listItem As ListItem = ddlTypeOfCount.Items.FindByValue(GetSetting(Field.CountingItem, FieldSetting.DefaultValue))
                If Not listItem Is Nothing Then
                    ddlTypeOfCount.SelectedValue = listItem.Value
                End If

            End If
        End If

    End Sub

    ' Field 5 - TransSubsidy (Big control) [Start]
    ' Set item - Subsidy
    Private Sub SetSubsidy()
        ' Set field description
        If IsExistValue(Field.Subsidy, FieldSetting.DescResource) Then
            lblSubsidy.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.Subsidy, FieldSetting.DescResource))
        End If

        ' Set field visibility
        'If dicFieldSetting.ContainsKey("Visible") Then
        '    Select Case dicFieldSetting("Visible")
        '        Case "Y"
        '            panSubsidy.Visible = True
        '        Case "N"
        '            panSubsidy.Visible = False
        '        Case Else
        '            panSubsidy.Visible = False
        '    End Select
        'End If

        ' Set default value
        If IsExistValue(Field.Subsidy, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.Subsidy, FieldSetting.DefaultValue) = String.Empty Then

                Dim listItem As ListItem = ddlSubsidy.Items.FindByValue(GetSetting(Field.Subsidy, FieldSetting.DefaultValue))
                If Not listItem Is Nothing Then
                    ddlSubsidy.SelectedValue = listItem.Value
                End If

            End If
        End If

    End Sub

    ' Field 5 - TransSubsidy (Big control) [Start]
    ' Set item - Type of BreakDown
    Private Sub SetTypeOfBreakDown()
        ' Set field description
        If IsExistValue(Field.BreakDownType, FieldSetting.DescResource) Then
            lblTypeOfBreakDown.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.BreakDownType, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.BreakDownType, FieldSetting.Visible) Then
            Select Case GetSetting(Field.BreakDownType, FieldSetting.Visible)
                Case Condition.YES
                    panTypeOfBreakDown.Visible = True
                Case Condition.NO
                    panTypeOfBreakDown.Visible = False
                Case Else
                    panTypeOfBreakDown.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.BreakDownType, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.BreakDownType, FieldSetting.DefaultValue) = String.Empty Then

                Dim listItem As ListItem = ddlTypeOfBreakDown.Items.FindByValue(GetSetting(Field.BreakDownType, FieldSetting.DefaultValue))
                If Not listItem Is Nothing Then
                    ddlTypeOfBreakDown.SelectedValue = listItem.Value
                End If

            End If
        End If

    End Sub

    ' Field 5 - TransSubsidy (Big control) [Start]
    ' Set item - District
    Private Sub SetDistrict()
        ' Set field description
        If IsExistValue(Field.District, FieldSetting.DescResource) Then
            lblDistrict.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.District, FieldSetting.DescResource))
        End If

        ' Set field visibility
        'If dicFieldSetting.ContainsKey("Visible") Then
        '    Select Case dicFieldSetting("Visible")
        '        Case "Y"
        '            panDistrict.Visible = True
        '        Case "N"
        '            panDistrict.Visible = False
        '        Case Else
        '            panDistrict.Visible = False
        '    End Select
        'End If

        ' Set default value
        If IsExistValue(Field.District, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.District, FieldSetting.DefaultValue) = String.Empty Then

                If GetSetting(Field.District, FieldSetting.DefaultValue) = "ANY" Then
                    For Each listItem As ListItem In chkDistrict.Items
                        listItem.Selected = True
                    Next
                Else
                    Dim strDefaultValue As String = GetSetting(Field.District, FieldSetting.DefaultValue)
                    Dim valueList As String() = strDefaultValue.Split(New Char() {","c})

                    For Each valueItem As String In valueList
                        Dim listItem As ListItem = chkDistrict.Items.FindByValue(valueItem)
                        If Not listItem Is Nothing Then
                            listItem.Selected = True
                        End If
                    Next

                End If

            End If
        End If

        ' Add selected value to list 
        AddDistrictIntoList()

    End Sub

    ' Field 5 - TransSubsidy (Big control) [Start]
    ' Set item - Health Profession
    Private Sub SetHealthProfession()
        ' Set field description
        If IsExistValue(Field.Profession, FieldSetting.DescResource) Then
            lblProfession.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.Profession, FieldSetting.DescResource))
        End If

        ' Set field visibility
        'If dicFieldSetting.ContainsKey("Visible") Then
        '    Select Case dicFieldSetting("Visible")
        '        Case "Y"
        '            panProfession.Visible = True
        '        Case "N"
        '            panProfession.Visible = False
        '        Case Else
        '            panProfession.Visible = False
        '    End Select
        'End If

        ' Set default value
        If IsExistValue(Field.Profession, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.Profession, FieldSetting.DefaultValue) = String.Empty Then

                If GetSetting(Field.Profession, FieldSetting.DefaultValue) = "ANY" Then
                    For Each listItem As ListItem In chkProfession.Items
                        listItem.Selected = True
                    Next
                Else
                    Dim strDefaultValue As String = GetSetting(Field.Profession, FieldSetting.DefaultValue)
                    Dim valueList As String() = strDefaultValue.Split(New Char() {","c})

                    For Each valueItem As String In valueList
                        Dim listItem As ListItem = chkProfession.Items.FindByValue(valueItem)
                        If Not listItem Is Nothing Then
                            listItem.Selected = True
                        End If
                    Next

                End If

            End If
        End If

        ' Add selected value to list 
        AddProfessionIntoList()

    End Sub


#End Region


End Class