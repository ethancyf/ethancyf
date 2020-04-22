Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format

Partial Public Class udcSTAT00003Criteria
    Inherits StatisticsCriteriaUC

#Region "Variables"

    'Private _dicSetting As New Dictionary(Of String, Dictionary(Of String, String))

#End Region

#Region "Session and Const"

    Private Class VS
        Public Const District As String = "District"
        Public Const Profession As String = "Profession"
        Public Const SubmissionMethod As String = "SubmissionMethod"
    End Class

    Public Class Field
        Public Const PeriodBreakDown As String = "PeriodBreakDown"
        Public Const FromDate_Creation As String = "FromDateCreation"
        Public Const ToDate_Creation As String = "ToDateCreation"
        Public Const Scheme As String = "Scheme"
        Public Const CountingItem As String = "CountingItem"
        Public Const Subsidy As String = "Subsidy"
        Public Const BreakDownType As String = "BreakDownType"
        Public Const District As String = "District"
        Public Const Profession As String = "Profession"
        Public Const DateType As String = "DateType"
        Public Const SubmissionMethod As String = "SubmissionMethod"
    End Class

    Public Class CustomFieldSetting
        Public Const EarliestDate As String = "EarliestDate"
        Public Const LatestDate As String = "LatestDate"
        Public Const AllowPastDate As String = "AllowPastDate"
        Public Const AllowFutureDate As String = "AllowFutureDate"
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

        ' If Break Down Type selected item = District, set profession to enabled
        If ddlTypeOfBreakDown.SelectedItem.Value.ToString.Trim = TypeOfBreakDown.District Then
            If ddlScheme.SelectedIndex = 0 Then
                SetProfessionEnabled(False)
            Else
                SetProfessionEnabled(True)
            End If
        Else
            SetProfessionEnabled(False)
        End If
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

                ' If Scheme selected index = 0 , set profession to disabled
                If ddlScheme.SelectedIndex = 0 Then
                    SetProfessionEnabled(False)
                Else
                    SetProfessionEnabled(True)
                End If
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

    Private Sub rbtnSubmissionMethodType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnSubmissionMethodType.SelectedIndexChanged
        Select Case rbtnSubmissionMethodType.SelectedIndex
            Case MultiSelectionTypeEnum.Any
                If chkSubmissionMethod.Items.Count > 0 Then
                    'For Each boxItem As ListItem In chkSubmissionMethod.Items
                    '    boxItem.Selected = False
                    'Next

                    'popupSubmissionMethod.Show()

                    'If chkSubmissionMethod.Items.Count <= 4 Then
                    '    chkSubmissionMethod.RepeatColumns = 1
                    'Else
                    '    chkSubmissionMethod.RepeatColumns = 2
                    'End If

                    SetSubmissionMethodSelectionToAny()

                End If

            Case MultiSelectionTypeEnum.Specific
                If chkSubmissionMethod.Items.Count > 0 Then
                    For Each boxItem As ListItem In chkSubmissionMethod.Items
                        boxItem.Selected = False
                    Next

                    popupSubmissionMethod.Show()

                    If chkSubmissionMethod.Items.Count <= 8 Then
                        chkSubmissionMethod.RepeatColumns = 1
                    Else
                        chkSubmissionMethod.RepeatColumns = 2
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

    Public Sub ibtnAddSubmissionMethod_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If chkSubmissionMethod.Items.Count > 0 Then
            popupSubmissionMethod.Show()
        End If
    End Sub

    Public Sub ibtnSubmissionMethodPopupOK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' Add selected value to list
        AddSubmissionMethodIntoList()
    End Sub

    Public Sub ibtnSubmissionMethodPopupCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim items As New Dictionary(Of String, Boolean)
        If Not ViewState(VS.SubmissionMethod) Is Nothing Then
            items = CType(ViewState(VS.SubmissionMethod), Dictionary(Of String, Boolean))
        End If

        If items.Count = 0 Then
            ' First cancel, no value is selected before
            For Each boxItem As ListItem In chkSubmissionMethod.Items
                boxItem.Selected = False
            Next

        Else
            ' Have value already
            For Each boxItem As ListItem In chkSubmissionMethod.Items
                If items.ContainsKey(boxItem.Value.ToString.Trim) Then
                    boxItem.Selected = items(boxItem.Value.ToString.Trim)
                End If
            Next

        End If

        ' Submission method checkboxlist item

        Dim strChkBoxItemString As String = ""
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkSubmissionMethod.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        '' If all items are selected
        'If intIsSelectedCount = chkSubmissionMethod.Items.Count Then
        '    rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Any
        '    strChkBoxItemString = String.Empty
        'Else
        '    ' Have selected items
        '    If intIsSelectedCount > 0 Then
        '        rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Specific
        '    Else
        '        rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        '    End If

        'End If

        ' Have selected items (if any, no need to do counting)
        If rbtnSubmissionMethodType.SelectedIndex <> MultiSelectionTypeEnum.Any Then
            If intIsSelectedCount > 0 Then
                rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Specific
            Else
                rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
            End If

        End If

        lblAddSubmissionMethodDisplay.Text = strChkBoxItemString

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

        ' Field 7 - Submission method [Start]
        BuildSubmissionMethodComponent(chkSubmissionMethod)
        ' Field 7 - Submission method [End]

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

        ' Field 5 - TransSubsidy (Break down type) [Start]
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

        ' Field 5 - TransSubsidy (Break down type) [End]

        ' Field 5 - TransSubsidy (Counting Type) [Start]
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

        ' Field 5 - TransSubsidy (Counting Type) [End]


        ' Field 6 - Type of date [Start]
        SetErrorDateTypeVisibility(False)

        ' Date Type
        If IsExistValue(Field.DateType, FieldSetting.Visible) Then
            If GetSetting(Field.DateType, FieldSetting.Visible) = Condition.YES Then
                ' Check dropdown (Scheme)
                If CheckDateTypeHasSelectedValue() = False Then
                    Dim strDateType As String = String.Empty

                    If IsExistValue(Field.DateType, FieldSetting.DescResource) Then
                        strDateType = Me.GetGlobalResourceObject("Text", GetSetting(Field.DateType, FieldSetting.DescResource))
                    Else
                        strDateType = Me.GetGlobalResourceObject("Text", "TypeOfDate")
                    End If

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                    lstErrorParam1.Add(strDateType)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorDateType.Visible = True
                End If

            End If
        End If

        ' Field 6 - Type of date [End]


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

        '' Type of counting item
        'If IsExistValue(Field.CountingItem, FieldSetting.Visible) Then
        '    If GetSetting(Field.CountingItem, FieldSetting.Visible) = Condition.YES Then
        '        ' Check dropdown (type of counting item)
        '        If ddlTypeOfCount.SelectedValue.Trim = DROP_DOWN_EMPTY Then
        '            Dim strTypeOfCountText As String = String.Empty

        '            If IsExistValue(Field.CountingItem, FieldSetting.DescResource) Then
        '                strTypeOfCountText = Me.GetGlobalResourceObject("Text", GetSetting(Field.CountingItem, FieldSetting.DescResource))
        '            Else
        '                strTypeOfCountText = Me.GetGlobalResourceObject("Text", "CountingItem")
        '            End If

        '            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
        '            lstErrorParam1.Add(strTypeOfCountText)
        '            lstErrorParam2.Add(String.Empty)
        '            imgErrorTypeOfCount.Visible = True
        '        End If

        '    End If
        'End If

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


        ' Field 7 - Submission method [Start]
        SetErrorSubmissionMethod(False)

        ' Submission method
        If IsExistValue(Field.SubmissionMethod, FieldSetting.Visible) Then
            If GetSetting(Field.SubmissionMethod, FieldSetting.Visible) = Condition.YES Then
                ' Check checkbox list (Submission method)
                If CheckSubmissionMethodHasSelectedValue() = False Then
                    Dim strSubmissionMethodText As String = String.Empty

                    If IsExistValue(Field.SubmissionMethod, FieldSetting.DescResource) Then
                        strSubmissionMethodText = Me.GetGlobalResourceObject("Text", GetSetting(Field.SubmissionMethod, FieldSetting.DescResource))
                    Else
                        strSubmissionMethodText = Me.GetGlobalResourceObject("Text", "SubmissionMethod")
                    End If

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                    lstErrorParam1.Add(strSubmissionMethodText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorSubmissionMethod.Visible = True
                End If
            End If
        End If

        ' Field 7 - Submission method [End]

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
                Dim strTypeOfBreakDownValue As String = String.Empty

                Select Case ddlTypeOfBreakDown.SelectedValue.Trim
                    Case TypeOfBreakDown.Profession
                        strTypeOfBreakDownValue += ddlTypeOfBreakDown.SelectedValue.ToString.Trim
                    Case TypeOfBreakDown.District
                        strTypeOfBreakDownValue += ddlTypeOfBreakDown.SelectedValue.ToString.Trim
                    Case Else
                        ' Nothing
                        strTypeOfBreakDownValue += ""
                End Select

                udtParameterList.AddParam(strTypeOfBreakdown, strTypeOfBreakDownValue)
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

        ' Field 6 - Type of date [Start]
        ' Date Type
        If IsExistValue(Field.DateType, FieldSetting.Visible) Then
            If GetSetting(Field.DateType, FieldSetting.Visible) = Condition.YES Then
                Dim strDateType As String = lblDateType.Text.Trim
                Dim strDateTypeValue As String = String.Empty
                If rbtnDateType.SelectedItem Is Nothing Then
                    strDateTypeValue = String.Empty
                Else
                    strDateTypeValue = rbtnDateType.SelectedValue.ToString.Trim
                End If

                udtParameterList.AddParam(strDateType, strDateTypeValue)
            End If
        End If

        ' Field 6 - Type of date [End]


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


        ' Field 7 - Submission method [Start]
        If IsExistValue(Field.SubmissionMethod, FieldSetting.Visible) Then
            If GetSetting(Field.SubmissionMethod, FieldSetting.Visible) = Condition.YES Then
                Dim strSubmissionMethodLabel As String = lblSubmissionMethod.Text.Trim
                Dim strChkBoxItemString As String = String.Empty

                Select Case rbtnSubmissionMethodType.SelectedIndex
                    Case MultiSelectionTypeEnum.Any
                        strChkBoxItemString += "Any"

                    Case MultiSelectionTypeEnum.Specific
                        ' Submission method checkboxlist item
                        For Each boxItem As ListItem In chkSubmissionMethod.Items
                            If boxItem.Selected = True Then
                                strChkBoxItemString += String.Format("{0}{1}", boxItem.Value.ToString.Trim, ",")
                            End If
                        Next
                        strChkBoxItemString = strChkBoxItemString.Substring(0, strChkBoxItemString.Length - 1)

                    Case Else
                        ' Nothing
                        strChkBoxItemString += ""
                End Select

                udtParameterList.AddParam(strSubmissionMethodLabel, strChkBoxItemString)
            End If
        End If

        ' Field 7 - Submission method [End]

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

        ' Type of Counting Item (Can configure visibility)
        If IsExistValue(Field.CountingItem, FieldSetting.Visible) Then
            If GetSetting(Field.CountingItem, FieldSetting.Visible) = Condition.YES Then
                Dim strTypeOfCountLabel As String = lblTypeOfCount.Text.Trim
                udtParameterList.AddParam(strTypeOfCountLabel, ddlTypeOfCount.SelectedItem.Text.Trim)
            End If
        End If

        ' Field 6 - Type of date [Start]
        ' Date Type
        If IsExistValue(Field.DateType, FieldSetting.Visible) Then
            If GetSetting(Field.DateType, FieldSetting.Visible) = Condition.YES Then
                Dim strDateType As String = lblDateType.Text.Trim
                If rbtnDateType.SelectedItem Is Nothing Then
                    udtParameterList.AddParam(strDateType, String.Empty)
                Else
                    udtParameterList.AddParam(strDateType, rbtnDateType.SelectedItem.Text.Trim)
                End If
            End If
        End If

        ' Field 6 - Type of date [End]


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
        

        ' Field 5 - TransSubsidy (Big control) [Start]
        ' Scheme (Can configure visibility)
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                Dim strSchemeLabel As String = lblScheme.Text.Trim
                udtParameterList.AddParam(strSchemeLabel, ddlScheme.SelectedItem.Text.Trim)
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
        

        ' Field 7 - Submission method [Start]
        If IsExistValue(Field.SubmissionMethod, FieldSetting.Visible) Then
            If GetSetting(Field.SubmissionMethod, FieldSetting.Visible) = Condition.YES Then
                Dim strSubmissionMethodLabel As String = lblSubmissionMethod.Text.Trim
                Dim strChkBoxItemString As String = String.Empty

                Select Case rbtnSubmissionMethodType.SelectedIndex
                    Case MultiSelectionTypeEnum.Any
                        strChkBoxItemString += "Any"
                        udtParameterList.AddParam(strSubmissionMethodLabel, strChkBoxItemString)

                    Case MultiSelectionTypeEnum.Specific
                        ' Submission method checkboxlist item
                        'strChkBoxItemString += "<ul style='padding-left: 15px; margin: 0px'>"
                        'For Each boxItem As ListItem In chkSubmissionMethod.Items
                        '    If boxItem.Selected = True Then
                        '        strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                        '    End If
                        'Next
                        'strChkBoxItemString += "</ul>"

                        ' Submission method checkboxlist item
                        Dim listParam As New ParameterObjectList(strSubmissionMethodLabel)
                        For Each boxItem As ListItem In chkSubmissionMethod.Items
                            If boxItem.Selected = True Then
                                listParam.ParamValueList.Add(boxItem.Text.ToString.Trim)
                            End If
                        Next

                        udtParameterList.AddParam(listParam)

                    Case Else
                        ' Nothing
                End Select

                'udtParameterList.AddParam(strSubmissionMethodLabel, strChkBoxItemString)
            End If
        End If

        ' Field 7 - Submission method [End]

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
                        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                        ' -------------------------------------------------------------------------
                        ' Bug Fix on Incorrect Data Size
                        'udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, ddlSubsidy.SelectedValue.Trim)
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 25, ddlSubsidy.SelectedValue.Trim)
                        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
                    End If

                ElseIf GetSetting(Field.Subsidy, FieldSetting.Visible) = Condition.NO Then

                    If IsExistValue(Field.Subsidy, FieldSetting.DefaultValue) Then
                        If Not GetSetting(Field.Subsidy, FieldSetting.DefaultValue) Is String.Empty Then

                            If IsExistValue(Field.Subsidy, FieldSetting.SPParamName) Then
                                Dim strDefaultValue As String = String.Empty
                                Dim strSPParamName As String = String.Empty

                                strDefaultValue = GetSetting(Field.Subsidy, FieldSetting.DefaultValue)
                                strSPParamName = GetSetting(Field.Subsidy, FieldSetting.SPParamName)
                                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                                ' -------------------------------------------------------------------------
                                ' Bug Fix on Incorrect Data Size
                                'udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, strDefaultValue)
                                udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 25, strDefaultValue)
                                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
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
                        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                        ' -------------------------------------------------------------------------
                        ' Bug Fix on Incorrect Data Size
                        'udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, "")
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 25, "")
                        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
                    End If

                ElseIf GetSetting(Field.Subsidy, FieldSetting.Visible) = Condition.NO Then

                    If IsExistValue(Field.Subsidy, FieldSetting.DefaultValue) Then
                        If Not GetSetting(Field.Subsidy, FieldSetting.DefaultValue) Is String.Empty Then

                            If IsExistValue(Field.Subsidy, FieldSetting.SPParamName) Then
                                Dim strDefaultValue As String = String.Empty
                                Dim strSPParamName As String = String.Empty

                                strDefaultValue = GetSetting(Field.Subsidy, FieldSetting.DefaultValue)
                                strSPParamName = GetSetting(Field.Subsidy, FieldSetting.SPParamName)
                                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                                ' -------------------------------------------------------------------------
                                ' Bug Fix on Incorrect Data Size
                                'udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, strDefaultValue)
                                udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 25, strDefaultValue)
                                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
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

        ' Field 6 - Type of date [Start]
        ' Date Type
        If IsExistValue(Field.DateType, FieldSetting.Visible) Then
            If GetSetting(Field.DateType, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.DateType, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty

                    strSPParamName = GetSetting(Field.DateType, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 1, rbtnDateType.SelectedValue.Trim)
                End If

            ElseIf GetSetting(Field.DateType, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.DateType, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.DateType, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.DateType, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.DateType, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.DateType, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 20, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        ' Field 6 - Type of date [End]

        ' Field 7 - Submission method [Start]
        ' Submission method
        If IsExistValue(Field.SubmissionMethod, FieldSetting.Visible) Then
            If GetSetting(Field.SubmissionMethod, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.SubmissionMethod, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty
                    strSPParamName = GetSetting(Field.SubmissionMethod, FieldSetting.SPParamName)

                    If rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Any Then
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, Nothing)
                    Else
                        Dim strPassValue As String = String.Empty

                        For Each boxItem As ListItem In chkSubmissionMethod.Items
                            If boxItem.Selected = True Then

                                ' Special case, If value = "WEB-FULL", pass the value "WEB,WEB-FULL"
                                If boxItem.Value.ToString.Trim = "WEB-FULL" Then
                                    strPassValue += "WEB," + boxItem.Value.ToString.Trim + ","
                                Else
                                    strPassValue += boxItem.Value.ToString.Trim + ","
                                End If

                            End If
                        Next

                        strPassValue = strPassValue.Substring(0, strPassValue.Length - 1)
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, strPassValue)
                    End If

                End If

            ElseIf GetSetting(Field.SubmissionMethod, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.SubmissionMethod, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.SubmissionMethod, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.SubmissionMethod, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.SubmissionMethod, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.SubmissionMethod, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        ' Field 7 - Submission method [End]


        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        SetErrorTypeOfDateVisibility(blnVisible)
        SetPeriodCreationErrorImageVisibility(blnVisible)
        SetErrorDateTypeVisibility(blnVisible)
        SetErrorSubmissionMethod(blnVisible)
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

    Protected Overrides Sub BuildSubmissionMethodComponent(ByVal cboxListComponent As CheckBoxList)
        MyBase.BuildSubmissionMethodComponent(cboxListComponent)
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

        ' Field 6 - Type of date [Start]
        ' Set item - Date Type    
        SetDateType()

        ' Field 6 - Type of date [End]

        ' Field 7 - Submission method [Start]
        ' Set item - Submission Method    
        SetSubmissionMethod()
        ' Field 7 - Submission method [End]

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

    Private Function CheckDateTypeHasSelectedValue() As Boolean
        Dim blnHasSelectedValue As Boolean = False

        'If rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Any Then
        '    blnHasSelectedValue = True
        'Else
        '    For Each boxItem As ListItem In chkSubmissionMethod.Items
        '        If boxItem.Selected = True Then
        '            blnHasSelectedValue = True
        '            Exit For
        '        End If
        '    Next
        'End If

        For Each rbtnItem As ListItem In rbtnDateType.Items
            If rbtnItem.Selected = True Then
                blnHasSelectedValue = True
                Exit For
            End If
        Next

        Return blnHasSelectedValue
    End Function

    Private Function CheckSubmissionMethodHasSelectedValue() As Boolean
        Dim blnHasSelectedValue As Boolean = False

        If rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Any Then
            blnHasSelectedValue = True
        Else
            For Each boxItem As ListItem In chkSubmissionMethod.Items
                If boxItem.Selected = True Then
                    blnHasSelectedValue = True
                    Exit For
                End If
            Next
        End If

        Return blnHasSelectedValue
    End Function

    Private Sub AddSubmissionMethodIntoList()
        Dim items As New Dictionary(Of String, Boolean)

        ' Submission method checkboxlist item
        For Each boxItem As ListItem In chkSubmissionMethod.Items
            items.Add(boxItem.Value.ToString.Trim, boxItem.Selected)
        Next

        ViewState(VS.SubmissionMethod) = items

        ' Submission method checkboxlist item

        Dim strChkBoxItemString As String = ""
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkSubmissionMethod.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        'If intIsSelectedCount = chkSubmissionMethod.Items.Count Then
        '    rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Any
        '    strChkBoxItemString = String.Empty
        'Else
        '    ' Have selected items
        '    If intIsSelectedCount > 0 Then
        '        rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Specific
        '    Else
        '        rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        '    End If

        'End If

        ' Have selected items
        If intIsSelectedCount > 0 Then
            rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Specific
        Else
            rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        End If

        lblAddSubmissionMethodDisplay.Text = strChkBoxItemString

    End Sub

    Private Sub SetSubmissionMethodSelectionToAny()
        'BuildProfessionComponent(ddlScheme, chkProfession, Field.Profession)
        For Each boxItem As ListItem In chkSubmissionMethod.Items
            boxItem.Selected = False
        Next

        ViewState(VS.SubmissionMethod) = Nothing

        rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Any

        ' Submission method checkbox list item
        Dim strChkBoxItemString As String = String.Empty

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkSubmissionMethod.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
            End If
        Next
        strChkBoxItemString += "</ul>"

        lblAddSubmissionMethodDisplay.Text = strChkBoxItemString
    End Sub

    Private Sub SetErrorTypeOfDateVisibility(ByVal blnVisible As Boolean)
        imgErrorPeriodBreakDown.Visible = blnVisible
    End Sub

    ' Set Period error image visibility
    Private Sub SetPeriodCreationErrorImageVisibility(ByVal blnVisible As Boolean)
        imgErrorDate_D_Creation.Visible = blnVisible
    End Sub

    Private Sub SetErrorDateTypeVisibility(ByVal blnVisible As Boolean)
        imgErrorDateType.Visible = blnVisible
    End Sub

    Private Sub SetErrorSubmissionMethod(ByVal blnVisible As Boolean)
        imgErrorSubmissionMethod.Visible = blnVisible
    End Sub

    Private Sub SetProfessionEnabled(ByVal blnVisible As Boolean)
        Select Case blnVisible
            Case True
                rbtnProfessionType.Enabled = True
                imgAddProfession.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectSBtn")
                imgAddProfession.AlternateText = Me.GetGlobalResourceObject("AlternateText", "SelectSBtn")
                imgAddProfession.Enabled = True
            Case False
                rbtnProfessionType.Enabled = False
                imgAddProfession.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SelectSDBtn")
                imgAddProfession.AlternateText = Me.GetGlobalResourceObject("AlternateText", "SelectSDBtn")
                imgAddProfession.Enabled = False
        End Select
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

    ' Field 6 - Type of date [Start]
    ' Set item - Date Type
    Private Sub SetDateType()
        ' Set field description
        If IsExistValue(Field.DateType, FieldSetting.DescResource) Then
            lblDateType.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.DateType, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.DateType, FieldSetting.Visible) Then
            Select Case GetSetting(Field.DateType, FieldSetting.Visible)
                Case Condition.YES
                    panDateType.Visible = True
                Case Condition.NO
                    panDateType.Visible = False
                Case Else
                    panDateType.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.DateType, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.DateType, FieldSetting.DefaultValue) = String.Empty Then

                Dim listItem As ListItem = rbtnDateType.Items.FindByValue(GetSetting(Field.DateType, FieldSetting.DefaultValue))
                If Not listItem Is Nothing Then
                    rbtnDateType.SelectedValue = listItem.Value
                End If

            End If
        End If

    End Sub

    ' Field 7 - Submission method [Start]
    ' Set item - Submission Method
    Private Sub SetSubmissionMethod()
        ' Set field description
        If IsExistValue(Field.SubmissionMethod, FieldSetting.DescResource) Then
            lblSubmissionMethod.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.SubmissionMethod, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.SubmissionMethod, FieldSetting.Visible) Then
            Select Case GetSetting(Field.SubmissionMethod, FieldSetting.Visible)
                Case Condition.YES
                    panSubmissionMethod.Visible = True
                Case Condition.NO
                    panSubmissionMethod.Visible = False
                Case Else
                    panSubmissionMethod.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.SubmissionMethod, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.SubmissionMethod, FieldSetting.DefaultValue) = String.Empty Then

                If GetSetting(Field.SubmissionMethod, FieldSetting.DefaultValue) = "ANY" Then
                    For Each listItem As ListItem In chkSubmissionMethod.Items
                        listItem.Selected = True
                    Next
                Else
                    Dim strDefaultValue As String = GetSetting(Field.SubmissionMethod, FieldSetting.DefaultValue)
                    Dim valueList As String() = strDefaultValue.Split(New Char() {","c})

                    For Each valueItem As String In valueList
                        Dim listItem As ListItem = chkSubmissionMethod.Items.FindByValue(valueItem)
                        If Not listItem Is Nothing Then
                            listItem.Selected = True
                        End If
                    Next

                End If

            End If
        End If

        ' Add selected value to list 
        AddSubmissionMethodIntoList()

    End Sub

#End Region

End Class