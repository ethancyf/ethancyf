Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Component.HCVUUser
Imports Common.Validation

Partial Public Class udcExactDatePeriod
    Inherits StatisticsCriteriaUC

#Region "Variables"

    'Private _dicSetting As Dictionary(Of String, Dictionary(Of String, String))
    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Dim udtValidator As New Validator

    Shadows Const FUNCTION_CODE As String = "990000"
    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

#End Region

#Region "Session and Consts"

    Public Class Field
        Public Const Scheme As String = "Scheme"
        Public Const FromDate As String = "FromDate"
        Public Const ToDate As String = "ToDate"

        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Const FromDateLabelWidth As String = "FromDateLabelWidth"
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
    End Class

    Public Class CustomFieldSetting
        Public Const EarliestDate As String = "EarliestDate"
        Public Const LatestDate As String = "LatestDate"
        Public Const AllowPastDate As String = "AllowPastDate"
        Public Const AllowFutureDate As String = "AllowFutureDate"
        Public Const UseCutOffDate As String = "UseCutOffDate"
        'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Const AllowToday As String = "AllowToday"
        Public Const ExactDateMaxRange As String = "ExactDateMaxRange"
        'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Const PastDateMaxRange As String = "PastDateMaxRange"
        Public Const FutureDateMaxRange As String = "FutureDateMaxRange"
        Public Const DetermineReportGenDate As String = "DetermineReportGenDate"
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
    End Class

    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Class Datepart
        Public Const Year As String = "Y"
        Public Const Month As String = "M"
        Public Const DayOfYear As String = "D"
    End Class

    Public Class DatepartDesc
        Public Const Year As String = "year"
        Public Const Month As String = "month"
        Public Const DayOfYear As String = "day"
        Public Const Years As String = "years"
        Public Const Months As String = "months"
        Public Const DaysOfYear As String = "days"
    End Class

    Public Class DateValidationDesc
        Public Const Today As String = "today"
        Public Const FutureDate As String = "future date"
        Public Const PastDate As String = "past date"
    End Class
    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

#End Region

#Region "Implement IStatisticsCriteriaUC"

    Public Overrides Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
        ' Control setting file
        '_dicSetting = dicSetting

        ' Initial control
        'InitControl()
        'SetPeriodFromErrorImageVisibility(False)
        'SetPeriodToErrorImageVisibility(False)
        SetErrorComponentVisibility(False)

        ' Initialize Calendar Extender
        Dim udtFormatter As New Formatter

        calExFromDate_D.Format = udtFormatter.EnterDateFormat
        calExToDate_D.Format = udtFormatter.EnterDateFormat

        MyBase.Build(dicSetting)

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        Dim udtFormatter As New Formatter

        'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Dim blnFromDateMiss As Boolean = False
        'Dim blnToDateMiss As Boolean = False

        Dim blnFromDateValid As Boolean = True
        Dim blnToDateValid As Boolean = True
        Dim blnDateValid As Boolean

        'Dim blnMissInput As Boolean = False
        'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

        Dim blnInvalid As Boolean = False
        'Dim blnIncomplete As Boolean = False

        ' Set period error image
        SetErrorComponentVisibility(False)

        ' Set input date format
        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'txtFromDate_D.Text = udtFormatter.formatDate(txtFromDate_D.Text.Trim)
        'txtToDate_D.Text = udtFormatter.formatDate(txtToDate_D.Text.Trim)
        txtFromDate_D.Text = udtFormatter.formatInputDate(txtFromDate_D.Text.Trim)
        txtToDate_D.Text = udtFormatter.formatInputDate(txtToDate_D.Text.Trim)
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then

                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim sm As SystemMessage = Nothing
                sm = udtValidator.chkInputDate(txtFromDate_D.Text, True, False)

                '' Cannot empty
                'If txtFromDate_D.Text = String.Empty Then
                '    blnMissInput = True
                '    blnFromDateValid = False

                '    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003))
                '    lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                '    lstErrorParam2.Add(String.Empty)
                '    'imgErrorFromDate_D.Visible = True
                '    imgErrorDate_D.Visible = True
                'Else
                '    ' Date validation
                '    If Not IsDate(udtFormatter.convertDate(txtFromDate_D.Text.Trim, String.Empty)) Then
                '        blnInvalid = True
                '        blnFromDateValid = False

                '        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00004))
                '        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                '        lstErrorParam2.Add(String.Empty)
                '        'imgErrorFromDate_D.Visible = True
                '        imgErrorDate_D.Visible = True
                '    End If
                'End If

                If Not sm Is Nothing Then
                    blnFromDateValid = False

                    lstError.Add(sm)
                    lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                    lstErrorParam2.Add(String.Empty)
                    imgErrorDate_D.Visible = True

                End If
                'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]
            End If
        End If

        ' Period To
        'SetPeriodToErrorImageVisibility(False)

        If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim sm As SystemMessage = Nothing
                sm = udtValidator.chkInputDate(txtToDate_D.Text, True, False)

                '' Cannot empty
                'If txtToDate_D.Text = String.Empty Then
                '    blnMissInput = True
                '    blnToDateValid = False

                '    If blnFromDateValid = True Then
                '        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003))
                '        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                '        lstErrorParam2.Add(String.Empty)
                '        'imgErrorToDate_D.Visible = True
                '        imgErrorDate_D.Visible = True
                '    End If
                'Else
                '    ' Date validation
                '    If Not IsDate(udtFormatter.convertDate(txtToDate_D.Text.Trim, String.Empty)) Then
                '        blnInvalid = True
                '        blnToDateValid = False

                '        If blnFromDateValid = True Then
                '            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00004))
                '            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                '            lstErrorParam2.Add(String.Empty)
                '            'imgErrorToDate_D.Visible = True
                '            imgErrorDate_D.Visible = True
                '        End If
                '    End If
                'End If

                If Not sm Is Nothing Then
                    blnInvalid = True
                    blnToDateValid = False

                    If blnFromDateValid = True Then
                        lstError.Add(sm)
                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                        lstErrorParam2.Add(String.Empty)
                        imgErrorDate_D.Visible = True
                    End If
                End If
                'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]
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

        'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        blnDateValid = blnFromDateValid And blnToDateValid
        'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

        ' Check Period From boundary [Start]
        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then

                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If txtFromDate_D.Text <> String.Empty AndAlso blnFromDateValid Then
                If txtFromDate_D.Text <> String.Empty AndAlso blnDateValid Then
                    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                    ' Earliest Date
                    If IsExistValue(Field.FromDate, CustomFieldSetting.EarliestDate) Then
                        Dim strValidationDate As String = GetSetting(Field.FromDate, CustomFieldSetting.EarliestDate)
                        If Not strValidationDate Is String.Empty AndAlso blnFromDateValid Then
                            Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D.Text)
                            If dtmFromDate.CompareTo(dtmValidationDate) < 0 Then
                                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                blnFromDateValid = False
                                ' To date is visible, use From + To Date
                                If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
                                    If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                                        'lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005))
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00370))
                                        'lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)) + " From")
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                        'lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        lstErrorParam2.Add(udtFormatter.convertDate(dtmValidationDate))
                                    Else
                                        'lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005))
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00381))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                        'lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        lstErrorParam2.Add(udtFormatter.convertDate(dtmValidationDate))
                                    End If
                                Else
                                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00381))
                                    lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                    lstErrorParam2.Add(udtFormatter.convertDate(dtmValidationDate))
                                End If
                                'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                                'imgErrorFromDate_D.Visible = True
                                imgErrorDate_D.Visible = True
                            End If
                        End If
                    End If

                    ' Latest Date
                    If IsExistValue(Field.FromDate, CustomFieldSetting.LatestDate) Then
                        Dim strValidationDate As String = GetSetting(Field.FromDate, CustomFieldSetting.LatestDate)
                        If Not strValidationDate Is String.Empty AndAlso blnFromDateValid Then
                            Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D.Text)
                            If dtmFromDate.CompareTo(dtmValidationDate) > 0 Then
                                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                blnFromDateValid = False
                                ' To date is visible, use From + To Date
                                If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
                                    If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                                        'lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00371))
                                        'lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)) + " From")
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                        'lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        lstErrorParam2.Add(udtFormatter.convertDate(dtmValidationDate))
                                    Else
                                        'lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00382))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                        'lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                        lstErrorParam2.Add(udtFormatter.convertDate(dtmValidationDate))
                                    End If
                                Else
                                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00382))
                                    lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                    lstErrorParam2.Add(udtFormatter.convertDate(dtmValidationDate))
                                End If
                                'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                                'imgErrorFromDate_D.Visible = True
                                imgErrorDate_D.Visible = True
                            End If
                        End If
                    End If

                    ' Allow Past Date
                    If IsExistValue(Field.FromDate, CustomFieldSetting.AllowPastDate) Then
                        Dim strAllowPassDate As String = GetSetting(Field.FromDate, CustomFieldSetting.AllowPastDate)
                        If Not strAllowPassDate Is String.Empty AndAlso blnFromDateValid Then
                            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            Dim dtmValidationDate As DateTime? = Nothing
                            Dim blnCheckPastDateMaxRange As Boolean = False
                            Dim strDateInterval As String = String.Empty

                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D.Text)

                            If strAllowPassDate = Condition.YES Then
                                ' If allow past date value is Y, check whether exceed past day limit (If exist)
                                If IsExistValue(Field.FromDate, CustomFieldSetting.PastDateMaxRange) Then
                                    Dim strPastDateMaxRange As String = GetSetting(Field.FromDate, CustomFieldSetting.PastDateMaxRange)

                                    If strPastDateMaxRange <> String.Empty Then
                                        blnCheckPastDateMaxRange = True
                                        dtmValidationDate = GetPastDateBound(strPastDateMaxRange, strDateInterval)
                                    End If
                                End If

                            ElseIf strAllowPassDate = Condition.NO Then
                                ' If allow past date value is N, check whether is past date 
                                dtmValidationDate = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                            End If

                            If dtmValidationDate.HasValue Then
                                If dtmFromDate.CompareTo(dtmValidationDate.Value) < 0 Then
                                    blnFromDateValid = False

                                    ' To date is visible, use From + To Date
                                    If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
                                        If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                                            If blnCheckPastDateMaxRange Then
                                                Dim udtSM As New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00431)
                                                udtSM.AddReplaceMessage("{FieldName}", Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                                udtSM.AddReplaceMessage("{Interval}", strDateInterval)
                                                udtSM.AddReplaceMessage("{BoundDate}", udtFormatter.convertDate(dtmValidationDate))

                                                lstError.Add(udtSM)
                                            Else
                                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00378))
                                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                                lstErrorParam2.Add(DateValidationDesc.PastDate)
                                            End If
                                        Else
                                            If blnCheckPastDateMaxRange Then
                                                Dim udtSM As New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00430)
                                                udtSM.AddReplaceMessage("{FieldName}", Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                                udtSM.AddReplaceMessage("{Interval}", strDateInterval)
                                                udtSM.AddReplaceMessage("{BoundDate}", udtFormatter.convertDate(dtmValidationDate))

                                                lstError.Add(udtSM)
                                            Else
                                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00375))
                                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                                lstErrorParam2.Add(String.Empty)
                                            End If
                                        End If
                                    Else
                                        If blnCheckPastDateMaxRange Then
                                            Dim udtSM As New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00430)
                                            udtSM.AddReplaceMessage("{FieldName}", Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                            udtSM.AddReplaceMessage("{Interval}", strDateInterval)
                                            udtSM.AddReplaceMessage("{BoundDate}", udtFormatter.convertDate(dtmValidationDate))

                                            lstError.Add(udtSM)
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00375))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(String.Empty)
                                        End If
                                    End If

                                    imgErrorDate_D.Visible = True
                                End If
                            End If
                            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
                        End If
                    End If

                    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    ' Allow Today
                    If IsExistValue(Field.FromDate, CustomFieldSetting.AllowToday) Then
                        Dim strAllowToday As String = GetSetting(Field.FromDate, CustomFieldSetting.AllowToday)
                        If Not strAllowToday Is String.Empty AndAlso blnFromDateValid Then
                            Dim dtmValidationDate As DateTime = DateTime.Today
                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D.Text)

                            ' If allow today date value is N, do checking
                            If strAllowToday = Condition.NO Then
                                If dtmFromDate.CompareTo(dtmValidationDate) = 0 Then
                                    blnFromDateValid = False

                                    If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
                                        If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00378))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(DateValidationDesc.Today)

                                            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
                                            ' ----------------------------------------------------------------------------------------
                                            'imgErrorDate_D.Visible = True
                                            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00380))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(String.Empty)
                                        End If
                                    Else
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00380))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                        lstErrorParam2.Add(String.Empty)
                                    End If

                                    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
                                    ' ----------------------------------------------------------------------------------------
                                    imgErrorDate_D.Visible = True
                                    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
                                End If
                            End If
                        End If
                    End If
                    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                    ' Allow Future Date
                    If IsExistValue(Field.FromDate, CustomFieldSetting.AllowFutureDate) Then
                        Dim strAllowFutureDate As String = GetSetting(Field.FromDate, CustomFieldSetting.AllowFutureDate)
                        If Not strAllowFutureDate Is String.Empty AndAlso blnFromDateValid Then
                            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            Dim dtmValidationDate As DateTime? = Nothing                            
                            Dim blnCheckFutureDateMaxRange As Boolean = False
                            Dim strDateInterval As String = String.Empty
                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D.Text)

                            If strAllowFutureDate = Condition.YES Then
                                ' If allow future date value is Y, check whether exceed future day limit (If exist)
                                If IsExistValue(Field.FromDate, CustomFieldSetting.FutureDateMaxRange) Then
                                    Dim strFutureDateMaxRange As String = GetSetting(Field.FromDate, CustomFieldSetting.FutureDateMaxRange)

                                    If strFutureDateMaxRange <> String.Empty Then
                                        blnCheckFutureDateMaxRange = True
                                        dtmValidationDate = GetFutureDateBound(strFutureDateMaxRange, strDateInterval)
                                    End If
                                End If

                            ElseIf strAllowFutureDate = Condition.NO Then
                                ' If allow future date value is N, check whether is future date 
                                dtmValidationDate = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                            End If

                            If dtmValidationDate.HasValue Then
                                If dtmFromDate.CompareTo(dtmValidationDate.Value) > 0 Then
                                    blnFromDateValid = False

                                    '' To date is visible, use From + To Date
                                    If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
                                        If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                                            If blnCheckFutureDateMaxRange Then
                                                Dim udtSM As New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00429)
                                                udtSM.AddReplaceMessage("{FieldName}", Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                                udtSM.AddReplaceMessage("{Interval}", strDateInterval)
                                                udtSM.AddReplaceMessage("{BoundDate}", udtFormatter.convertDate(dtmValidationDate))

                                                lstError.Add(udtSM)

                                            Else
                                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00378))
                                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                                lstErrorParam2.Add(DateValidationDesc.FutureDate)
                                            End If
                                        Else
                                            If blnCheckFutureDateMaxRange Then
                                                Dim udtSM As New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00428)
                                                udtSM.AddReplaceMessage("{FieldName}", Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                                udtSM.AddReplaceMessage("{Interval}", strDateInterval)
                                                udtSM.AddReplaceMessage("{BoundDate}", udtFormatter.convertDate(dtmValidationDate))

                                                lstError.Add(udtSM)
                                            Else
                                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00376))
                                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                                lstErrorParam2.Add(String.Empty)
                                            End If
                                        End If
                                    Else
                                        If blnCheckFutureDateMaxRange Then
                                            Dim udtSM As New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00428)
                                            udtSM.AddReplaceMessage("{FieldName}", Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                            udtSM.AddReplaceMessage("{Interval}", strDateInterval)
                                            udtSM.AddReplaceMessage("{BoundDate}", udtFormatter.convertDate(dtmValidationDate))

                                            lstError.Add(udtSM)
                                        Else
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00376))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(String.Empty)
                                        End If
                                    End If

                                    imgErrorDate_D.Visible = True
                                End If
                            End If
                            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
                        End If
                    End If

                    ' Allow Cut off date
                    If IsExistValue(Field.FromDate, CustomFieldSetting.UseCutOffDate) Then
                        Dim strAllowCutOffDate As String = GetSetting(Field.FromDate, CustomFieldSetting.UseCutOffDate)
                        If Not strAllowCutOffDate Is String.Empty AndAlso blnFromDateValid Then
                            Dim dtmCutOffDate As DateTime = (New StatisticsBLL).GetDataCutOffDate(StatisticsBLL.EnumDataCutOffID.dbEVS_Enquiry)
                            Dim dtmValidationDate As DateTime = New DateTime(dtmCutOffDate.Year, dtmCutOffDate.Month, dtmCutOffDate.Day)
                            Dim dtmFromDate As DateTime = StringToDateTime(txtFromDate_D.Text)

                            ' If allow cut off date value is Y, do checking
                            If strAllowCutOffDate = Condition.YES Then
                                If dtmFromDate.CompareTo(dtmValidationDate) > 0 Then
                                    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                                    '-----------------------------------------------------------------------------------------
                                    blnFromDateValid = False
                                    ' To date is visible, use From + To Date
                                    If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
                                        If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                                            'lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00371))
                                            'lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)) + " From")
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                        Else
                                            'lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00382))
                                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                        End If
                                    Else
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00382))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                        lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                    End If
                                    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                                    'imgErrorFromDate_D.Visible = True
                                    imgErrorDate_D.Visible = True
                                End If
                            End If
                        End If
                    End If
                End If
            End If
        End If
        ' Check Period From boundary [End]

        ' Check Period To boundary [Start]
        If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then

                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If txtToDate_D.Text <> String.Empty AndAlso blnToDateValid Then
                If txtToDate_D.Text <> String.Empty AndAlso blnDateValid Then
                    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]
                    ' Earliest Date
                    If IsExistValue(Field.ToDate, CustomFieldSetting.EarliestDate) Then
                        Dim strValidationDate As String = GetSetting(Field.ToDate, CustomFieldSetting.EarliestDate)
                        If Not strValidationDate Is String.Empty AndAlso blnToDateValid Then
                            Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                            Dim dtmToDate As DateTime = StringToDateTime(txtToDate_D.Text)
                            If dtmToDate.CompareTo(dtmValidationDate) < 0 Then
                                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                blnToDateValid = False
                                '' From date is visible, use From + To Date
                                'If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
                                '    If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then
                                '        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00005))
                                '        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)) + " To")
                                '        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                '    Else
                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00372))
                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                'lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                lstErrorParam2.Add(udtFormatter.convertDate(dtmValidationDate))
                                '    End If
                                'End If
                                'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                                'imgErrorToDate_D.Visible = True
                                imgErrorDate_D.Visible = True
                            End If
                        End If
                    End If

                    ' Latest Date
                    If IsExistValue(Field.ToDate, CustomFieldSetting.LatestDate) Then
                        Dim strValidationDate As String = GetSetting(Field.ToDate, CustomFieldSetting.LatestDate)
                        If Not strValidationDate Is String.Empty AndAlso blnToDateValid Then
                            Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                            Dim dtmToDate As DateTime = StringToDateTime(txtToDate_D.Text)
                            If dtmToDate.CompareTo(dtmValidationDate) > 0 Then
                                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                                '-----------------------------------------------------------------------------------------
                                blnToDateValid = False
                                '' From date is visible, use From + To Date
                                'If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
                                '    If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then
                                '        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                '        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)) + " To")
                                '        lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                '    Else
                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00373))
                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                'lstErrorParam2.Add(dtmValidationDate.ToString((New Formatter).EnterDateFormat))
                                lstErrorParam2.Add(udtFormatter.convertDate(dtmValidationDate))
                                '    End If
                                'End If
                                'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                                'imgErrorToDate_D.Visible = True
                                imgErrorDate_D.Visible = True
                            End If
                        End If
                    End If

                    ' Allow Past Date
                    If IsExistValue(Field.ToDate, CustomFieldSetting.AllowPastDate) Then
                        Dim strAllowPassDate As String = GetSetting(Field.ToDate, CustomFieldSetting.AllowPastDate)
                        If Not strAllowPassDate Is String.Empty AndAlso blnToDateValid Then
                            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            Dim dtmValidationDate As DateTime? = Nothing
                            Dim blnCheckPastDateMaxRange As Boolean = False
                            Dim strDateInterval As String = String.Empty

                            Dim dtmToDate As DateTime = StringToDateTime(txtToDate_D.Text)

                            If strAllowPassDate = Condition.YES Then
                                ' If allow past date value is Y, check whether exceed past day limit (If exist)
                                If IsExistValue(Field.ToDate, CustomFieldSetting.PastDateMaxRange) Then
                                    Dim strPastDateMaxRange As String = GetSetting(Field.ToDate, CustomFieldSetting.PastDateMaxRange)

                                    If strPastDateMaxRange <> String.Empty Then
                                        blnCheckPastDateMaxRange = True
                                        dtmValidationDate = GetPastDateBound(strPastDateMaxRange, strDateInterval)
                                    End If
                                End If

                            ElseIf strAllowPassDate = Condition.NO Then
                                ' If allow past date value is N, check whether is past date 
                                dtmValidationDate = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                            End If

                            If dtmValidationDate.HasValue Then
                                If dtmToDate.CompareTo(dtmValidationDate.Value) < 0 Then
                                    blnToDateValid = False

                                    If blnCheckPastDateMaxRange Then
                                        Dim udtSM As New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00433)
                                        udtSM.AddReplaceMessage("{FieldName}", Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                        udtSM.AddReplaceMessage("{Interval}", strDateInterval)
                                        udtSM.AddReplaceMessage("{BoundDate}", udtFormatter.convertDate(dtmValidationDate))

                                        lstError.Add(udtSM)
                                    Else
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00379))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                        lstErrorParam2.Add(DateValidationDesc.PastDate)
                                    End If

                                    imgErrorDate_D.Visible = True
                                End If
                            End If
                            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
                        End If
                    End If

                    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    ' Allow Today
                    If IsExistValue(Field.ToDate, CustomFieldSetting.AllowToday) Then
                        Dim strAllowToday As String = GetSetting(Field.ToDate, CustomFieldSetting.AllowToday)
                        If Not strAllowToday Is String.Empty AndAlso blnToDateValid Then
                            Dim dtmValidationDate As DateTime = DateTime.Today
                            Dim dtmToDate As DateTime = StringToDateTime(txtToDate_D.Text)

                            ' If allow today date value is N, do checking
                            If strAllowToday = Condition.NO Then
                                If dtmToDate.CompareTo(dtmValidationDate) = 0 Then
                                    blnToDateValid = False
                                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00379))
                                    lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                    lstErrorParam2.Add(DateValidationDesc.Today)
                                    imgErrorDate_D.Visible = True
                                End If
                            End If
                        End If
                    End If
                    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                    ' Allow Future Date
                    If IsExistValue(Field.ToDate, CustomFieldSetting.AllowFutureDate) Then
                        Dim strAllowFutureDate As String = GetSetting(Field.ToDate, CustomFieldSetting.AllowFutureDate)
                        If Not strAllowFutureDate Is String.Empty AndAlso blnToDateValid Then
                            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            Dim dtmValidationDate As DateTime? = Nothing                            
                            Dim blnCheckFutureDateMaxRange As Boolean = False
                            Dim strDateInterval As String = String.Empty

                            Dim dtmToDate As DateTime = StringToDateTime(txtToDate_D.Text)

                            If strAllowFutureDate = Condition.YES Then
                                ' If allow future date value is Y, check whether exceed future day limit (If exist)
                                If IsExistValue(Field.ToDate, CustomFieldSetting.FutureDateMaxRange) Then
                                    Dim strFutureDateMaxRange As String = GetSetting(Field.ToDate, CustomFieldSetting.FutureDateMaxRange)

                                    If strFutureDateMaxRange <> String.Empty Then
                                        blnCheckFutureDateMaxRange = True
                                        dtmValidationDate = GetFutureDateBound(strFutureDateMaxRange, strDateInterval)
                                    End If
                                End If

                            ElseIf strAllowFutureDate = Condition.NO Then
                                ' If allow future date value is N, check whether is future date 
                                dtmValidationDate = New DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day)
                            End If

                            If dtmValidationDate.HasValue Then
                                If dtmToDate.CompareTo(dtmValidationDate) > 0 Then
                                    blnToDateValid = False

                                    If blnCheckFutureDateMaxRange Then
                                        Dim udtSM As New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00432)
                                        udtSM.AddReplaceMessage("{FieldName}", Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                        udtSM.AddReplaceMessage("{Interval}", strDateInterval)
                                        udtSM.AddReplaceMessage("{BoundDate}", udtFormatter.convertDate(dtmValidationDate))

                                        lstError.Add(udtSM)
                                    Else
                                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00379))
                                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                        lstErrorParam2.Add(DateValidationDesc.FutureDate)
                                    End If

                                    imgErrorDate_D.Visible = True
                                End If
                            End If
                            ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
                        End If
                    End If

                    ' Allow Cut off date
                    If IsExistValue(Field.ToDate, CustomFieldSetting.UseCutOffDate) Then
                        Dim strAllowCutOffDate As String = GetSetting(Field.ToDate, CustomFieldSetting.UseCutOffDate)
                        If Not strAllowCutOffDate Is String.Empty AndAlso blnToDateValid Then
                            Dim dtmCutOffDate As DateTime = (New StatisticsBLL).GetDataCutOffDate(StatisticsBLL.EnumDataCutOffID.dbEVS_Enquiry)
                            Dim dtmValidationDate As DateTime = New DateTime(dtmCutOffDate.Year, dtmCutOffDate.Month, dtmCutOffDate.Day)
                            Dim dtmToDate As DateTime = StringToDateTime(txtToDate_D.Text)

                            ' If allow cut off date value is Y, do checking
                            If strAllowCutOffDate = Condition.YES Then
                                If dtmToDate.CompareTo(dtmValidationDate) > 0 Then
                                    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                                    '-----------------------------------------------------------------------------------------
                                    blnToDateValid = False
                                    '' To date is visible, use From + To Date
                                    'If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
                                    '    If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then
                                    '        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                                    '        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)) + " To")
                                    '        lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                    '    Else
                                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00373))
                                    lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                    lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                                    '    End If
                                    'End If
                                    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                                    'imgErrorFromDate_D.Visible = True
                                    imgErrorDate_D.Visible = True
                                End If
                            End If
                        End If
                    End If

                End If
            End If
        End If

        ' Check Period To boundary [End]

        ' Check relation between Period From and Period To [Start]
        If IsExistValue(Field.FromDate, FieldSetting.Visible) AndAlso IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES AndAlso GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then

                If blnFromDateValid AndAlso blnToDateValid Then
                    Dim dtmFromDate As New DateTime
                    Dim dtmToDate As New DateTime

                    dtmFromDate = StringToDateTime(txtFromDate_D.Text)
                    dtmToDate = StringToDateTime(txtToDate_D.Text)

                    ' Check: From Date cannot later than To Date
                    If dtmFromDate.CompareTo(dtmToDate) > 0 Then
                        'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        blnFromDateValid = False
                        blnToDateValid = False

                        'lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00006))
                        'lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)) + " From")
                        'lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)) + " To")

                        lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00374))
                        lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                        lstErrorParam2.Add(String.Empty)
                        'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                        'lstErrorParam2.Add(String.Empty)

                        'imgErrorFromDate_D.Visible = True
                        'imgErrorToDate_D.Visible = True
                        imgErrorDate_D.Visible = True
                    End If
                End If
            End If
        End If
        ' Check relation between Period From and Period To [End]

        'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ' Check date difference between Period From and Period To
        If IsExistValue(Field.FromDate, FieldSetting.Visible) AndAlso IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES AndAlso GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                If IsExistValue(Field.FromDate, CustomFieldSetting.ExactDateMaxRange) AndAlso IsExistValue(Field.ToDate, CustomFieldSetting.ExactDateMaxRange) Then
                    If blnFromDateValid AndAlso blnToDateValid Then

                        Dim strFromExactDateInterval As String = GetSetting(Field.FromDate, CustomFieldSetting.ExactDateMaxRange)
                        Dim strToExactDateInterval As String = GetSetting(Field.ToDate, CustomFieldSetting.ExactDateMaxRange)

                        If strFromExactDateInterval <> String.Empty And strToExactDateInterval <> String.Empty And strFromExactDateInterval = strToExactDateInterval Then
                            Dim strDatePart As String
                            Dim intDateInterval As Integer

                            strDatePart = Left(strFromExactDateInterval, 1)
                            intDateInterval = CInt(Mid(strFromExactDateInterval, 2, strFromExactDateInterval.Length - 1))

                            Dim dtmFromDate As New DateTime
                            Dim dtmToDate As New DateTime
                            Dim dtmEffectiveToDate As New DateTime
                            Dim strDateInterval As String = String.Empty

                            dtmFromDate = StringToDateTime(txtFromDate_D.Text)
                            dtmToDate = StringToDateTime(txtToDate_D.Text)

                            Select Case strDatePart
                                Case Datepart.Year
                                    dtmEffectiveToDate = DateAdd(DateInterval.DayOfYear, -1, DateAdd(DateInterval.Year, intDateInterval, dtmFromDate))
                                    strDateInterval = CStr(intDateInterval) + " " + IIf(intDateInterval > 1, DatepartDesc.Years, DatepartDesc.Year)
                                Case Datepart.Month
                                    dtmEffectiveToDate = DateAdd(DateInterval.DayOfYear, -1, DateAdd(DateInterval.Month, intDateInterval, dtmFromDate))
                                    strDateInterval = CStr(intDateInterval) + " " + IIf(intDateInterval > 1, DatepartDesc.Months, DatepartDesc.Month)
                                Case Datepart.DayOfYear
                                    dtmEffectiveToDate = DateAdd(DateInterval.DayOfYear, -1, DateAdd(DateInterval.DayOfYear, intDateInterval, dtmFromDate))
                                    strDateInterval = CStr(intDateInterval) + " " + IIf(intDateInterval > 1, DatepartDesc.DaysOfYear, DatepartDesc.DayOfYear)
                                Case Else
                                    Throw New Exception("Invalid settings in field value '" + CustomFieldSetting.ExactDateMaxRange + "' of DB table 'StatisticCriteriaAdditionDetail_SCAD'. The value should be either '" + Datepart.Year + "', '" + Datepart.Month + "' or '" + Datepart.DayOfYear + "' with numeric value such as 'Y1', 'M12' or 'D366'.")
                            End Select

                            ' Check: To Date within the pre-defined period
                            If dtmToDate.CompareTo(dtmEffectiveToDate) > 0 Then
                                blnFromDateValid = False
                                blnToDateValid = False

                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00377))
                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource)))
                                lstErrorParam2.Add(strDateInterval)

                                imgErrorDate_D.Visible = True
                            End If
                        End If
                    End If
                End If
            End If
        End If
        'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strDatePeriodValue As String = String.Empty

        ' From Date
        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then
                Dim strFromDateLabel As String = lblFromDate.Text.Trim
                If txtFromDate_D.Text.Trim.Length = 0 Then
                    strDatePeriodValue += String.Empty
                Else
                    strDatePeriodValue += txtFromDate_D.Text.Trim
                End If
            End If
        End If


        ' To Date (Pass value if visibility of To Date is True)
        If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                Dim strToDateLabel As String = lblToDate.Text.Trim

                strDatePeriodValue += " to "

                If txtToDate_D.Text.Trim.Length = 0 Then
                    strDatePeriodValue += String.Empty
                Else
                    strDatePeriodValue += txtToDate_D.Text.Trim
                End If
            End If
        End If

        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then
                Dim strDatePeriodLabel As String = String.Empty
                If IsExistValue(Field.FromDate, FieldSetting.DescResource) Then
                    strDatePeriodLabel = Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource))
                Else
                    strDatePeriodLabel = Me.GetGlobalResourceObject("Text", "DatePeriod")
                End If
                udtParameterList.AddParam(strDatePeriodLabel, strDatePeriodValue)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strDatePeriod As String = ""

        ' From Date
        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then
                Dim strFromDateLabel As String = lblFromDate.Text.Trim
                If txtFromDate_D.Text.Trim.Length = 0 Then
                    strDatePeriod += "---"
                Else
                    strDatePeriod += txtFromDate_D.Text.Trim
                End If
            End If
        End If


        ' To Date (Pass value if visibility of To Date is True)
        If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                Dim strToDateLabel As String = lblToDate.Text.Trim

                strDatePeriod += " to "

                If txtToDate_D.Text.Trim.Length = 0 Then
                    strDatePeriod += "---"
                Else
                    strDatePeriod += txtToDate_D.Text.Trim
                End If
            End If
        End If

        'If _dicSetting.ContainsKey("ToDate") Then
        '    Dim dicFieldSetting As Dictionary(Of String, String) = _dicSetting("ToDate")

        '    If dicFieldSetting.ContainsKey("Visible") Then
        '        If dicFieldSetting("Visible") = "Y" Then

        '        End If
        '    End If

        'End If

        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then
                Dim strDatePeriodLabel As String = String.Empty
                If IsExistValue(Field.FromDate, FieldSetting.DescResource) Then
                    strDatePeriodLabel = Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource))
                Else
                    strDatePeriodLabel = Me.GetGlobalResourceObject("Text", "DatePeriod")
                End If
                udtParameterList.AddParam(strDatePeriodLabel, strDatePeriod)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' Period From
        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then

                Dim dtmPeriodFrom As DateTime = StringToDateTime(txtFromDate_D.Text)
                If IsExistValue(Field.FromDate, FieldSetting.SPParamName) Then
                    Dim strParamPeriodFrom As String = String.Empty
                    strParamPeriodFrom = GetSetting(Field.FromDate, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strParamPeriodFrom, System.Data.SqlDbType.DateTime, 8, DateToString(dtmPeriodFrom))
                End If

            ElseIf GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.FromDate, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.FromDate, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.FromDate, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As DateTime
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = StringToDateTime(GetSetting(Field.FromDate, FieldSetting.DefaultValue))
                            strSPParamName = GetSetting(Field.FromDate, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.DateTime, 8, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        ' Period To
        If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then

                Dim dtmPeriodTo As DateTime = StringToDateTime(txtToDate_D.Text)
                If IsExistValue(Field.ToDate, FieldSetting.SPParamName) Then
                    Dim strParamPeriodTo As String = String.Empty
                    strParamPeriodTo = GetSetting(Field.ToDate, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strParamPeriodTo, System.Data.SqlDbType.DateTime, 8, DateToString(dtmPeriodTo))
                End If

            ElseIf GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.ToDate, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.ToDate, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.ToDate, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As DateTime
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = StringToDateTime(GetSetting(Field.ToDate, FieldSetting.DefaultValue))
                            strSPParamName = GetSetting(Field.ToDate, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.DateTime, 8, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        imgErrorDate_D.Visible = blnVisible
    End Sub

    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Overrides Function GetReportGenerationDate() As DateTime?
        Dim dtmLatestDate As DateTime? = Nothing

        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim dtmCurrentDate As DateTime = udtGeneral.GetSystemDateTime().Date

        ' Period From
        If IsExistValue(Field.FromDate, CustomFieldSetting.DetermineReportGenDate) Then
            If GetSetting(Field.FromDate, CustomFieldSetting.DetermineReportGenDate) = Condition.YES Then

                If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
                    If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then
                        Dim dtmPeriodFrom As DateTime = StringToDateTime(txtFromDate_D.Text)
                        dtmLatestDate = dtmPeriodFrom

                    ElseIf GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.NO Then
                        If IsExistValue(Field.FromDate, FieldSetting.DefaultValue) Then

                            If Not GetSetting(Field.FromDate, FieldSetting.DefaultValue) Is String.Empty Then
                                Dim dtmDefaultValue As DateTime = StringToDateTime(GetSetting(Field.FromDate, FieldSetting.DefaultValue))
                                dtmLatestDate = dtmDefaultValue
                            End If
                        End If
                    End If
                End If
            End If
        End If


        ' Period To
        If IsExistValue(Field.ToDate, CustomFieldSetting.DetermineReportGenDate) Then
            If GetSetting(Field.ToDate, CustomFieldSetting.DetermineReportGenDate) = Condition.YES Then

                If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
                    If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                        Dim dtmPeriodTo As DateTime = StringToDateTime(txtToDate_D.Text)
                        dtmLatestDate = dtmPeriodTo

                    ElseIf GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.NO Then

                        If IsExistValue(Field.ToDate, FieldSetting.DefaultValue) Then
                            If Not GetSetting(Field.ToDate, FieldSetting.DefaultValue) Is String.Empty Then
                                Dim dtmDefaultValue As DateTime = StringToDateTime(GetSetting(Field.ToDate, FieldSetting.DefaultValue))
                                dtmLatestDate = dtmDefaultValue
                            End If
                        End If
                    End If
                End If
            End If
        End If

        If dtmLatestDate.HasValue Then
            ' If input date is today or future date, the report will be generated on next day of latest input date 
            If dtmLatestDate.Value.CompareTo(dtmCurrentDate) >= 0 Then
                Dim dtmReportGenDate As DateTime = DateAdd(DateInterval.Day, 1, dtmLatestDate.Value.Date)
                Return dtmReportGenDate
            End If
        End If

        Return Nothing
    End Function
    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
#End Region

#Region "Supporting Function"

    Public Overrides Sub InitControl()
        ' Set item - From Date
        SetFromDate()
        ' Set item - To Date
        SetToDate()
        ' Set item - Exact Date Period Panel
        SetExactDatePeriodPanel()

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

    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Function GetFutureDateBound(ByVal strFutureDateMaxRange As String, ByRef strDateInterval As String) As DateTime
        Dim dtmEffectiveToDate As New DateTime
        Dim strDatePart As String
        Dim intDateInterval As Integer

        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim dtmCurrentDate As DateTime = udtGeneral.GetSystemDateTime().Date

        strDatePart = Left(strFutureDateMaxRange, 1)
        intDateInterval = CInt(Mid(strFutureDateMaxRange, 2, strFutureDateMaxRange.Length - 1))

        ' Include Today
        Select Case strDatePart
            Case Datepart.Year
                dtmEffectiveToDate = DateAdd(DateInterval.DayOfYear, -1, DateAdd(DateInterval.Year, intDateInterval, dtmCurrentDate))
                strDateInterval = CStr(intDateInterval) + " " + IIf(intDateInterval > 1, DatepartDesc.Years, DatepartDesc.Year)
            Case Datepart.Month
                dtmEffectiveToDate = DateAdd(DateInterval.DayOfYear, -1, DateAdd(DateInterval.Month, intDateInterval, dtmCurrentDate))
                strDateInterval = CStr(intDateInterval) + " " + IIf(intDateInterval > 1, DatepartDesc.Months, DatepartDesc.Month)
            Case Datepart.DayOfYear
                dtmEffectiveToDate = DateAdd(DateInterval.DayOfYear, -1, DateAdd(DateInterval.DayOfYear, intDateInterval, dtmCurrentDate))
                strDateInterval = CStr(intDateInterval) + " " + IIf(intDateInterval > 1, DatepartDesc.DaysOfYear, DatepartDesc.DayOfYear)
            Case Else
                Throw New Exception("Invalid settings in field value '" + CustomFieldSetting.FutureDateMaxRange + "' of DB table 'StatisticCriteriaAdditionDetail_SCAD'. The value should be either '" + Datepart.Year + "', '" + Datepart.Month + "' or '" + Datepart.DayOfYear + "' with numeric value such as 'Y1', 'M12' or 'D366'.")
        End Select

        Return dtmEffectiveToDate
    End Function

    Private Function GetPastDateBound(ByVal strPastDateMaxRange As String, ByRef strDateInterval As String) As DateTime
        Dim dtmEffectiveFromDate As New DateTime
        Dim strDatePart As String
        Dim intDateInterval As Integer

        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim dtmCurrentDate As DateTime = udtGeneral.GetSystemDateTime().Date

        strDatePart = Left(strPastDateMaxRange, 1)
        intDateInterval = CInt(Mid(strPastDateMaxRange, 2, strPastDateMaxRange.Length - 1))

        ' Include Today
        Select Case strDatePart
            Case Datepart.Year
                dtmEffectiveFromDate = DateAdd(DateInterval.DayOfYear, 1, DateAdd(DateInterval.Year, -(intDateInterval), dtmCurrentDate))
                strDateInterval = CStr(intDateInterval) + " " + IIf(intDateInterval > 1, DatepartDesc.Years, DatepartDesc.Year)
            Case Datepart.Month
                dtmEffectiveFromDate = DateAdd(DateInterval.DayOfYear, 1, DateAdd(DateInterval.Month, -(intDateInterval), dtmCurrentDate))
                strDateInterval = CStr(intDateInterval) + " " + IIf(intDateInterval > 1, DatepartDesc.Months, DatepartDesc.Month)
            Case Datepart.DayOfYear
                dtmEffectiveFromDate = DateAdd(DateInterval.DayOfYear, 1, DateAdd(DateInterval.DayOfYear, -(intDateInterval), dtmCurrentDate))
                strDateInterval = CStr(intDateInterval) + " " + IIf(intDateInterval > 1, DatepartDesc.DaysOfYear, DatepartDesc.DayOfYear)
            Case Else
                Throw New Exception("Invalid settings in field value '" + CustomFieldSetting.PastDateMaxRange + "' of DB table 'StatisticCriteriaAdditionDetail_SCAD'. The value should be either '" + Datepart.Year + "', '" + Datepart.Month + "' or '" + Datepart.DayOfYear + "' with numeric value such as 'Y1', 'M12' or 'D366'.")
        End Select

        Return dtmEffectiveFromDate
    End Function
    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

#End Region

#Region "Fields Setting"

    ' Set item - From Date
    Private Sub SetFromDate()
        ' Set field description
        If IsExistValue(Field.FromDate, FieldSetting.DescResource) Then
            lblFromDate.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource))
        End If

        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Set label width
        If IsExistValue(Field.FromDateLabelWidth, FieldSetting.DefaultValue) Then
            If IsNumeric(GetSetting(Field.FromDateLabelWidth, FieldSetting.DefaultValue)) Then
                lblFromDate.Width = GetSetting(Field.FromDateLabelWidth, FieldSetting.DefaultValue)
            End If
        End If
        ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]

        ' Set field visibility
        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            Select Case GetSetting(Field.FromDate, FieldSetting.Visible)
                Case Condition.YES
                    panFromDate.Visible = True
                Case Condition.NO
                    panFromDate.Visible = False
                Case Else
                    panFromDate.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.FromDate, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.FromDate, FieldSetting.DefaultValue) = String.Empty Then
                Dim udtFormatter As New Formatter
                Dim dtmFromDate As DateTime = CType(GetSetting(Field.FromDate, FieldSetting.DefaultValue), DateTime)
                txtFromDate_D.Text = dtmFromDate.ToString(udtFormatter.EnterDateFormat)
            End If
        End If

    End Sub

    ' Set item - To Date
    Private Sub SetToDate()
        ' Set field description
        If IsExistValue(Field.ToDate, FieldSetting.DescResource) Then
            lblToDate.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.ToDate, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            Select Case GetSetting(Field.ToDate, FieldSetting.Visible)
                Case Condition.YES
                    panToDate.Visible = True
                Case Condition.NO
                    panToDate.Visible = False
                Case Else
                    panToDate.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.ToDate, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.ToDate, FieldSetting.DefaultValue) = String.Empty Then
                Dim udtFormatter As New Formatter
                Dim dtmToDate As DateTime = CType(GetSetting(Field.ToDate, FieldSetting.DefaultValue), DateTime)
                txtToDate_D.Text = dtmToDate.ToString(udtFormatter.EnterDateFormat)
            End If
        End If

    End Sub

    ' Set item - Exact Date Period Panel
    Private Sub SetExactDatePeriodPanel()
        Dim strFromDateVisible As String = Condition.NO
        Dim strToDateVisible As String = Condition.NO

        ' Get From Date visibility
        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            strFromDateVisible = GetSetting(Field.FromDate, FieldSetting.Visible)
        End If

        ' Get To Date visibility
        If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            strToDateVisible = GetSetting(Field.ToDate, FieldSetting.Visible)
        End If

        ' Set Exact Date Period Panel visibility
        If strFromDateVisible = Condition.NO AndAlso strToDateVisible = Condition.NO Then
            panExactDatePeriod.Visible = False
        Else
            panExactDatePeriod.Visible = True
        End If

    End Sub

#End Region


End Class