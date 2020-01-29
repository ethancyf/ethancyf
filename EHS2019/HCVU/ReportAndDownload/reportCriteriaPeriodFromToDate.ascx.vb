Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format

Partial Public Class reportCriteriaPeriodFromToDate
    Inherits System.Web.UI.UserControl
    Implements IReportCriteriaUC

#Region "Session and Consts"

    'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Class PeriodFromToDate
        Public Const UpperBoundByDay_ExcludeToday As Integer = -1
        Public Const UpperBoundByDay_IncludeToday As Integer = 0
        Public Const LowerBoundByDay_ExcludeToday As Integer = 1
        Public Const LowerBoundByDay_IncludeToday As Integer = 0
    End Class

    Public Class DateValidationDesc
        Public Const UCaseToday As String = "Today"
        Public Const LCaseToday As String = "today"
        Public Const FutureDate As String = "future date"
        Public Const PastDate As String = "past date"
    End Class
    'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

#End Region

#Region "Implement IReportCriteriaUC"
    'CRE13-003 Token Replacement [Start][Karl]
    'Public Sub Build(ByVal dicSetting As Dictionary(Of String, String)) Implements IReportCriteriaUC.Build
    Public Sub Build(ByVal dicSetting As Dictionary(Of String, String), ByVal strSetting As String) Implements IReportCriteriaUC.Build
        'CRE13-003 Token Replacement [End][Karl]

        ' Retrieve Setting:
        ' E.g. <SettingName> Description [DefaultValue]
        ' <FromDateText> The text of the first label. Also used for displaying error message. [Date From]
        ' <ToDateText> The text of the second label. Also used for displaying error message. [Date To]
        ' <ToDateVisible> The visibility of the second label, as well as its text box and calendar. [T]
        ' <FromTimeVisible> The visibility of the time section of Date From. [T]
        ' <ToTimeVisible> The visibility of the time section of Date To. [T]

        Dim strFromDateText As String = "Date From"
        If dicSetting.ContainsKey("FromDateText") Then strFromDateText = dicSetting("FromDateText")

        Dim strToDateText As String = "Date To"
        If dicSetting.ContainsKey("ToDateText") Then strToDateText = dicSetting("ToDateText")

        Dim blnToDateVisible As Boolean = True
        If dicSetting.ContainsKey("ToDateVisible") Then blnToDateVisible = dicSetting("ToDateVisible") = "T"

        Dim blnFromTimeVisible As Boolean = True
        If dicSetting.ContainsKey("FromTimeVisible") Then blnFromTimeVisible = dicSetting("FromTimeVisible") = "T"

        Dim blnToTimeVisible As Boolean = True
        If dicSetting.ContainsKey("ToTimeVisible") Then blnToTimeVisible = dicSetting("ToTimeVisible") = "T"

        ' Initialize wording
        lblFromDateText.Text = strFromDateText
        lblToDateText.Text = strToDateText

        ' Initialize Calendar Extender
        Dim udtFormatter As New Formatter

        calExFromDate.Format = udtFormatter.EnterDateFormat
        calExToDate.Format = udtFormatter.EnterDateFormat

        ' Show the second label
        If blnToDateVisible Then
            lblToDateText.Visible = True
            txtToDate.Visible = True
            btnToDate.Visible = True
        Else
            lblToDateText.Visible = False
            txtToDate.Visible = False
            btnToDate.Visible = False
        End If

        ' Show time
        If blnFromTimeVisible Then
            txtFromTime.Visible = True
            lblFromTimeRemark.Visible = True
        Else
            txtFromTime.Visible = False
            lblFromTimeRemark.Visible = False
        End If

        If blnToTimeVisible Then
            txtToTime.Visible = True
            lblToTimeRemark.Visible = True
        Else
            txtToTime.Visible = False
            lblToTimeRemark.Visible = False
        End If

        ' Initialize
        txtFromDate.Text = String.Empty
        txtFromTime.Text = String.Empty
        txtToDate.Text = String.Empty
        txtToTime.Text = String.Empty

        imgErrorFromDate.Visible = False
        imgErrorToDate.Visible = False

    End Sub

    Public Sub ValidateCriteriaInput(ByVal strReportID As String, ByVal dicSetting As Dictionary(Of String, String), ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String)) Implements IReportCriteriaUC.ValidateCriteriaInput
        ' Retrieve Setting:
        ' E.g. <SettingName> Description [DefaultValue]
        ' <FromDateText> The text of the first label. Also used for displaying error message. [Date From]
        ' <ToDateText> The text of the second label. Also used for displaying error message. [Date To]
        ' <FromDateLowerBound> The lower bound of the value of the first text box. Leave empty for unbounded. []
        ' <FromDateUpperBound> The upper bound of the value of the first text box. Leave empty for unbounded. []
        ' <ToDateLowerBound> The lower bound of the value of the second text box. Leave empty for unbounded. []
        ' <ToDateUpperBound> The upper bound of the value of the second text box. Leave empty for unbounded. []
        ' <FromDateLowerBoundByDay> The value in first text box cannot be smaller than the N day before today. Leave empty for unlimited. Can be negative. []
        ' <FromDateUpperBoundByDay> The value in first text box cannot be greater than the N day after today. Leave empty for unlimited. Can be negative. []
        ' <ToDateLowerBoundByDay> The value in second text box cannot be smaller than the N day before today. Leave empty for unlimited. Can be negative. []
        ' <ToDateUpperBoundByDay> The value in second text box cannot be greater than the N day after today. Leave empty for unlimited. Can be negative. []
        ' <FromDateAllowEmpty> Whether the Date From is allowed to be empty. [F]
        ' <ToDateAllowEmpty> Whether Date To is allowed to be empty. [F]
        ' <FromTimeAllowEmpty> Whether the time section of Date From is allowed to be empty. [F]
        ' <ToTimeAllowEmpty> Whether the time section of Date To is allowed to be empty. [F]
        ' <FromToLimit> The limit between Date From and Date To. []
        ' <FromToDayDiff> The allow difference between Date From and Date To in term of day. []

        Dim strFromDateText As String = "Date From"
        If dicSetting.ContainsKey("FromDateText") Then strFromDateText = dicSetting("FromDateText")

        Dim strFromDateName As String = "Date From"
        If dicSetting.ContainsKey("FromDateName") Then strFromDateName = dicSetting("FromDateName")

        Dim strToDateText As String = "Date To"
        If dicSetting.ContainsKey("ToDateText") Then strToDateText = dicSetting("ToDateText")

        Dim strToDateName As String = "Date To"
        If dicSetting.ContainsKey("ToDateName") Then strToDateName = dicSetting("ToDateName")

        Dim strFromDateLowerBound As String = String.Empty
        If dicSetting.ContainsKey("FromDateLowerBound") Then strFromDateLowerBound = dicSetting("FromDateLowerBound")

        Dim strFromDateUpperBound As String = String.Empty
        If dicSetting.ContainsKey("FromDateUpperBound") Then strFromDateUpperBound = dicSetting("FromDateUpperBound")

        Dim strToDateLowerBound As String = String.Empty
        If dicSetting.ContainsKey("ToDateLowerBound") Then strToDateLowerBound = dicSetting("ToDateLowerBound")

        Dim strToDateUpperBound As String = String.Empty
        If dicSetting.ContainsKey("ToDateUpperBound") Then strToDateUpperBound = dicSetting("ToDateUpperBound")

        Dim strFromDateLowerBoundByDay As String = String.Empty
        If dicSetting.ContainsKey("FromDateLowerBoundByDay") Then strFromDateLowerBoundByDay = dicSetting("FromDateLowerBoundByDay")

        Dim strFromDateUpperBoundByDay As String = String.Empty
        If dicSetting.ContainsKey("FromDateUpperBoundByDay") Then strFromDateUpperBoundByDay = dicSetting("FromDateUpperBoundByDay")

        Dim strToDateLowerBoundByDay As String = String.Empty
        If dicSetting.ContainsKey("ToDateLowerBoundByDay") Then strToDateLowerBoundByDay = dicSetting("ToDateLowerBoundByDay")

        Dim strToDateUpperBoundByDay As String = String.Empty
        If dicSetting.ContainsKey("ToDateUpperBoundByDay") Then strToDateUpperBoundByDay = dicSetting("ToDateUpperBoundByDay")

        Dim blnFromDateAllowEmpty As Boolean = False
        If dicSetting.ContainsKey("FromDateAllowEmpty") Then blnFromDateAllowEmpty = dicSetting("FromDateAllowEmpty") = "T"

        Dim blnToDateAllowEmpty As Boolean = False
        If dicSetting.ContainsKey("ToDateAllowEmpty") Then blnToDateAllowEmpty = dicSetting("ToDateAllowEmpty") = "T"

        Dim blnFromTimeAllowEmpty As Boolean = False
        If dicSetting.ContainsKey("FromTimeAllowEmpty") Then blnFromTimeAllowEmpty = dicSetting("FromTimeAllowEmpty") = "T"

        Dim blnToTimeAllowEmpty As Boolean = False
        If dicSetting.ContainsKey("ToTimeAllowEmpty") Then blnToTimeAllowEmpty = dicSetting("ToTimeAllowEmpty") = "T"

        Dim strFromToLimit As String = String.Empty
        If dicSetting.ContainsKey("FromToLimit") Then strFromToLimit = dicSetting("FromToLimit")

        'CRE13-001-02 EHAPP Phase 2 [Start][Karl]
        Dim strFromToDayDiff As String = String.Empty
        If dicSetting.ContainsKey("FromToDayDiff") Then strFromToDayDiff = dicSetting("FromToDayDiff")

        If String.IsNullOrEmpty(strFromToDayDiff) = False AndAlso IsNumeric(strFromToDayDiff) = False Then
            Throw New Exception("reportCriteriaPeriodFromToDate.ascx.vb: setting [FromToDayDiff] is not numeric")
        End If
        'CRE13-001-02 EHAPP Phase 2 [End][Karl]


        ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [Start][Tommy L]
        ' -------------------------------------------------------------------------------------------------------------------
        ' Add the feature to allow including Time value or not
        Dim blnFromTimeVisible As Boolean = True
        If dicSetting.ContainsKey("FromTimeVisible") Then blnFromTimeVisible = dicSetting("FromTimeVisible") = "T"

        Dim blnToTimeVisible As Boolean = True
        If dicSetting.ContainsKey("ToTimeVisible") Then blnToTimeVisible = dicSetting("ToTimeVisible") = "T"
        ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [End][Tommy L]


        imgErrorFromDate.Visible = False
        imgErrorToDate.Visible = False

        Dim udtFormatter As New Formatter

        ' --- Check FromDate ---
        Dim blnFromDateMiss As Boolean = False
        Dim blnToDateMiss As Boolean = False
        Dim blnFromDateValid As Boolean = True
        Dim blnToDateValid As Boolean = True

        Dim blnMissInput As Boolean = False
        Dim blnInvalid As Boolean = False
        Dim blnIncomplete As Boolean = False

        ' Cannot empty
        If txtFromDate.Text.Trim = String.Empty Then
            If Not blnFromDateAllowEmpty Then
                blnMissInput = True

                imgErrorFromDate.Visible = True
                blnFromDateValid = False

            End If

        Else
            ' Valid date
            ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [Start][Tommy L]
            ' -------------------------------------------------------------------------------------------------------------------
            ' Allow user to input Date value with 8-digit

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'txtFromDate.Text = udtFormatter.formatDate(txtFromDate.Text.Trim)
            txtFromDate.Text = udtFormatter.formatInputDate(txtFromDate.Text.Trim)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [End][Tommy L]

            If Not IsDate(udtFormatter.convertDate(txtFromDate.Text.Trim, String.Empty)) Then
                blnInvalid = True

                imgErrorFromDate.Visible = True
                blnFromDateValid = False

            Else
                ' Check minimum and maximum date to prevent overflow in SQL with data type - datetime
                If CDate(udtFormatter.convertDate(txtFromDate.Text.Trim, String.Empty)).Year < DateValidation.YearMinValue _
                    OrElse CDate(udtFormatter.convertDate(txtFromDate.Text.Trim, String.Empty)).Year > DateValidation.YearMaxValue Then
                    blnInvalid = True

                    imgErrorFromDate.Visible = True
                    blnFromDateValid = False
                End If

            End If

        End If


        ' --- Check ToTime ---
        If txtFromTime.Text.Trim = String.Empty Then
            If Not blnFromTimeAllowEmpty Then
                If blnFromDateValid Then
                    blnIncomplete = True

                    imgErrorFromDate.Visible = True
                    blnFromDateValid = False

                End If

            End If

        Else
            ' Valid time format
            If Not ValidateTime(txtFromTime.Text.Trim) Then
                blnInvalid = True


                imgErrorFromDate.Visible = True
                blnFromDateValid = False

            End If

        End If


        '' --- Check ToDate ---
        ' Cannot empty
        If txtToDate.Text.Trim = String.Empty Then
            If Not blnToDateAllowEmpty Then
                blnMissInput = True

                imgErrorToDate.Visible = True
                blnToDateValid = False

            End If

        Else
            ' Valid date
            ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [Start][Tommy L]
            ' -------------------------------------------------------------------------------------------------------------------
            ' Allow user to input Date value with 8-digit

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'txtToDate.Text = udtFormatter.formatDate(txtToDate.Text.Trim)
            txtToDate.Text = udtFormatter.formatInputDate(txtToDate.Text.Trim)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [End][Tommy L]

            If Not IsDate(udtFormatter.convertDate(txtToDate.Text.Trim, String.Empty)) Then
                blnInvalid = True

                imgErrorToDate.Visible = True
                blnToDateValid = False

            Else
                ' Check minimum and maximum date to prevent overflow in SQL with data type - datetime
                If CDate(udtFormatter.convertDate(txtToDate.Text.Trim, String.Empty)).Year < DateValidation.YearMinValue _
                    OrElse CDate(udtFormatter.convertDate(txtToDate.Text.Trim, String.Empty)).Year > DateValidation.YearMaxValue Then
                    blnInvalid = True

                    imgErrorToDate.Visible = True
                    blnToDateValid = False
                End If

            End If

        End If


        ' --- Check ToTime ---
        If txtToTime.Text.Trim = String.Empty Then
            If Not blnToTimeAllowEmpty Then
                If blnToDateValid Then
                    blnIncomplete = True

                    imgErrorToDate.Visible = True
                    blnToDateValid = False

                End If

            End If

        Else
            ' Valid time format
            If Not ValidateTime(txtToTime.Text.Trim) Then
                blnInvalid = True


                imgErrorToDate.Visible = True
                blnToDateValid = False

            End If

        End If

        If blnMissInput Then
            lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00023))
            lstErrorParam1.Add(strFromDateText)
            lstErrorParam2.Add(String.Empty)
        End If

        If blnInvalid Then
            lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00024))
            lstErrorParam1.Add(strFromDateText)
            lstErrorParam2.Add(String.Empty)
        End If

        If blnIncomplete Then
            lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00027))
            lstErrorParam1.Add(strFromDateText)
            lstErrorParam2.Add(String.Empty)
        End If

        'If (txtFromDate.Text.Trim = String.Empty And Not blnFromDateAllowEmpty) Then
        '    ' From date Cannot empty
        '    imgErrorFromDate.Visible = True
        '    blnFromDateMiss = True
        'ElseIf txtFromDate.Text.Trim <> String.Empty Then
        '    If Not IsDate(udtFormatter.convertDate(txtFromDate.Text.Trim, String.Empty)) Then
        '        ' Invalid From date
        '        imgErrorFromDate.Visible = True
        '        blnFromDateValid = False
        '    End If
        'End If

        'If (txtToDate.Text.Trim = String.Empty And Not blnToDateAllowEmpty) Then
        '    ' To date Cannot empty
        '    imgErrortoDate.Visible = True
        '    blnToDateMiss = True
        'ElseIf txtToDate.Text.Trim <> String.Empty Then
        '    If Not IsDate(udtFormatter.convertDate(txtToDate.Text.Trim, String.Empty)) Then
        '        ' Invalid To date
        '        imgErrorToDate.Visible = True
        '        blnToDateValid = False
        '    End If
        'End If

        'If blnFromDateMiss Or blnToDateMiss Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00023))
        '    lstErrorParam1.Add(strFromDateText)
        '    lstErrorParam2.Add(String.Empty)
        'End If

        'If Not blnFromDateValid Or Not blnToDateValid Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00024))
        '    lstErrorParam1.Add(strFromDateText)
        '    lstErrorParam2.Add(String.Empty)
        'End If


        '' --- Check FromTime ---
        ''If blnFromDateValid And Not blnFromDateMiss Then
        'If (txtFromTime.Text.Trim = String.Empty And Not blnFromTimeAllowEmpty) Then
        '    imgErrorFromDate.Visible = True
        '    blnFromDateValid = False
        'ElseIf txtFromTime.Text.Trim <> String.Empty Then
        '    If txtFromDate.Text.Trim = String.Empty Then
        '        imgErrorFromDate.Visible = True
        '        blnFromDateValid = False
        '    End If
        'End If
        ''End If

        ''If blnToDateValid And Not blnToDateMiss Then
        'If (txtToTime.Text.Trim = String.Empty And Not blnToTimeAllowEmpty) Then
        '    imgErrorToDate.Visible = True
        '    blnToDateValid = False
        'ElseIf txtToTime.Text.Trim <> String.Empty Then
        '    If txtToDate.Text.Trim = String.Empty Then
        '        imgErrorToDate.Visible = True
        '        blnToDateValid = False
        '    End If
        'End If
        ''End If

        'If (Not blnFromDateValid And Not blnFromDateMiss) Or (Not blnToDateValid And Not blnToDateMiss) Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00027))
        '    lstErrorParam1.Add(strFromDateText)
        '    lstErrorParam2.Add(String.Empty)
        'End If

        ''If blnFromDateValid And Not blnFromDateMiss Then
        '' Valid time format
        'If txtFromTime.Text.Trim <> String.Empty AndAlso Not ValidateTime(txtFromTime.Text.Trim) Then
        '    imgErrorFromDate.Visible = True
        '    blnFromDateValid = False
        'End If
        '' End If

        '' If blnToDateValid And Not blnToDateMiss Then
        'If txtToTime.Text.Trim <> String.Empty AndAlso Not ValidateTime(txtToTime.Text.Trim) Then
        '    imgErrorToDate.Visible = True
        '    blnToDateValid = False
        'End If
        ''End If

        'If Not blnFromDateValid Or Not blnToDateValid Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00024))
        '    lstErrorParam1.Add(strFromDateText)
        '    lstErrorParam2.Add(String.Empty)
        'End If


        ' --- Check FromDateTime boundary ---
        If txtFromDate.Text.Trim <> String.Empty AndAlso blnFromDateValid Then
            Dim dtmFrom As DateTime = StringToDateTime(String.Format("{0} {1}", txtFromDate.Text.Trim, txtFromTime.Text.Trim))
            Dim dtmNow As DateTime = StringToDateTime(DateTime.Now.ToString("dd-MM-yyyy"))

            ' Lower bound
            If strFromDateLowerBound <> String.Empty _
                    AndAlso dtmFrom.CompareTo(StringToDateTime(strFromDateLowerBound)) < 0 Then
                lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00025))
                lstErrorParam1.Add(strFromDateName)
                lstErrorParam2.Add(DateToString(StringToDateTime(strFromDateLowerBound)))

                imgErrorFromDate.Visible = True
                blnFromDateValid = False

            End If

            ' Upper bound
            If strFromDateUpperBound <> String.Empty _
                    AndAlso dtmFrom.CompareTo(StringToDateTime(strFromDateUpperBound)) > 0 Then
                lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00026))
                lstErrorParam1.Add(strFromDateName)
                lstErrorParam2.Add(DateToString(StringToDateTime(strFromDateUpperBound)))

                imgErrorFromDate.Visible = True
                blnFromDateValid = False

            End If

            ' Lower bound by day
            If strFromDateLowerBoundByDay <> String.Empty _
                    AndAlso dtmFrom.CompareTo(dtmNow.AddDays(CInt(strFromDateLowerBoundByDay))) < 0 Then
                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If CInt(strFromDateLowerBoundByDay) = PeriodFromToDate.LowerBoundByDay_ExcludeToday Then
                    If dtmFrom.CompareTo(dtmNow) = 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00378))
                        lstErrorParam1.Add(lblFromDateText.Text)
                        lstErrorParam2.Add(DateValidationDesc.LCaseToday)
                    ElseIf dtmFrom.CompareTo(dtmNow) < 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00378))
                        lstErrorParam1.Add(lblFromDateText.Text)
                        lstErrorParam2.Add(DateValidationDesc.PastDate)
                    End If

                ElseIf CInt(strFromDateLowerBoundByDay) = PeriodFromToDate.LowerBoundByDay_IncludeToday Then
                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00378))
                    lstErrorParam1.Add(strFromDateName)
                    lstErrorParam2.Add(DateValidationDesc.PastDate)

                Else
                    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00025))
                    lstErrorParam1.Add(strFromDateName)
                    ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------------------------------------
                    ' Add the feature to allow including Time value or not
                    lstErrorParam2.Add(DateToString(dtmNow.AddDays(CInt(strFromDateLowerBoundByDay)), blnFromTimeVisible))
                    ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [End][Tommy L]
                End If
                'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                imgErrorFromDate.Visible = True
                blnFromDateValid = False

            End If

            ' Upper bound by day
            If strFromDateUpperBoundByDay <> String.Empty _
                    AndAlso dtmFrom.CompareTo(dtmNow.AddDays(CInt(strFromDateUpperBoundByDay))) > 0 Then
                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If CInt(strFromDateUpperBoundByDay) = PeriodFromToDate.UpperBoundByDay_ExcludeToday Then
                    If dtmFrom.CompareTo(dtmNow) = 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00378))
                        lstErrorParam1.Add(lblFromDateText.Text)
                        lstErrorParam2.Add(DateValidationDesc.LCaseToday)
                    ElseIf dtmFrom.CompareTo(dtmNow) > 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00378))
                        lstErrorParam1.Add(lblFromDateText.Text)
                        lstErrorParam2.Add(DateValidationDesc.FutureDate)
                    End If
                ElseIf CInt(strFromDateUpperBoundByDay) = PeriodFromToDate.UpperBoundByDay_IncludeToday Then
                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00378))
                    lstErrorParam1.Add(strFromDateName)
                    lstErrorParam2.Add(DateValidationDesc.FutureDate)

                Else
                    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00026))
                    lstErrorParam1.Add(strFromDateName)
                    ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------------------------------------
                    ' Add the feature to allow including Time value or not
                    lstErrorParam2.Add(DateToString(dtmNow.AddDays(CInt(strFromDateUpperBoundByDay)), blnFromTimeVisible))
                    ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [End][Tommy L]

                End If
                'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                imgErrorFromDate.Visible = True
                blnFromDateValid = False

            End If

        End If





        ' --- Check ToDateTime boundary ---
        If txtToDate.Text.Trim <> String.Empty AndAlso blnToDateValid Then
            Dim dtmTo As DateTime = StringToDateTime(String.Format("{0} {1}", txtToDate.Text.Trim, txtToTime.Text.Trim))
            Dim dtmNow As DateTime = StringToDateTime(DateTime.Now.ToString("dd-MM-yyyy"))

            ' Lower bound
            If strToDateLowerBound <> String.Empty _
                    AndAlso dtmTo.CompareTo(StringToDateTime(strToDateLowerBound)) < 0 Then
                lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00025))
                lstErrorParam1.Add(strToDateName)
                lstErrorParam2.Add(DateToString(StringToDateTime(strToDateLowerBound)))

                imgErrorToDate.Visible = True
                blnToDateValid = False

            End If

            ' Upper bound
            If strToDateUpperBound <> String.Empty _
                    AndAlso dtmTo.CompareTo(StringToDateTime(strToDateUpperBound)) > 0 Then
                lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00026))
                lstErrorParam1.Add(strToDateName)
                lstErrorParam2.Add(DateToString(StringToDateTime(strToDateUpperBound)))

                imgErrorToDate.Visible = True
                blnToDateValid = False

            End If

            ' Lower bound by day
            If strToDateLowerBoundByDay <> String.Empty _
                    AndAlso dtmTo.CompareTo(dtmNow.AddDays(CInt(strToDateLowerBoundByDay))) < 0 Then

                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If CInt(strToDateLowerBoundByDay) = PeriodFromToDate.LowerBoundByDay_ExcludeToday Then
                    If dtmTo.CompareTo(dtmNow) = 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00379))
                        lstErrorParam1.Add(lblFromDateText.Text)
                        lstErrorParam2.Add(DateValidationDesc.LCaseToday)
                    ElseIf dtmTo.CompareTo(dtmNow) < 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00379))
                        lstErrorParam1.Add(lblFromDateText.Text)
                        lstErrorParam2.Add(DateValidationDesc.PastDate)
                    End If

                ElseIf CInt(strToDateLowerBoundByDay) = PeriodFromToDate.LowerBoundByDay_IncludeToday Then
                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00379))
                    lstErrorParam1.Add(strToDateName)
                    lstErrorParam2.Add(DateValidationDesc.PastDate)

                Else
                    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00025))
                    lstErrorParam1.Add(strToDateName)
                    ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------------------------------------
                    ' Add the feature to allow including Time value or not
                    lstErrorParam2.Add(DateToString(dtmNow.AddDays(CInt(strToDateLowerBoundByDay)), blnToTimeVisible))
                    ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [End][Tommy L]
                End If
                'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                imgErrorToDate.Visible = True
                blnToDateValid = False

            End If

            ' Upper bound by day
            If strToDateUpperBoundByDay <> String.Empty _
                    AndAlso dtmTo.CompareTo(dtmNow.AddDays(CInt(strToDateUpperBoundByDay))) > 0 Then
                'CRE15-016 (Randomly genereate the valid claim transaction) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If CInt(strToDateUpperBoundByDay) = PeriodFromToDate.UpperBoundByDay_ExcludeToday Then
                    If dtmTo.CompareTo(dtmNow) = 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00379))
                        lstErrorParam1.Add(lblFromDateText.Text)
                        lstErrorParam2.Add(DateValidationDesc.LCaseToday)
                    ElseIf dtmTo.CompareTo(dtmNow) > 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00379))
                        lstErrorParam1.Add(lblFromDateText.Text)
                        lstErrorParam2.Add(DateValidationDesc.FutureDate)
                    End If

                ElseIf CInt(strToDateUpperBoundByDay) = PeriodFromToDate.UpperBoundByDay_IncludeToday Then
                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00379))
                    lstErrorParam1.Add(strToDateName)
                    lstErrorParam2.Add(DateValidationDesc.FutureDate)

                Else
                    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00026))
                    lstErrorParam1.Add(strToDateName)
                    ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [Start][Tommy L]
                    ' -------------------------------------------------------------------------------------------------------------------
                    ' Add the feature to allow including Time value or not
                    lstErrorParam2.Add(DateToString(dtmNow.AddDays(CInt(strToDateUpperBoundByDay)), blnToTimeVisible))
                    ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [End][Tommy L]
                End If
                'CRE15-016 (Randomly genereate the valid claim transaction) [End][Chris YIM]

                imgErrorToDate.Visible = True
                blnToDateValid = False

            End If

        End If


        ' --- Check From and To relationship ---
        If Not blnFromDateAllowEmpty AndAlso Not blnToDateAllowEmpty AndAlso blnFromDateValid AndAlso blnToDateValid Then
            Dim dtmFrom As DateTime = StringToDateTime(String.Format("{0} {1}", txtFromDate.Text.Trim, txtFromTime.Text.Trim))
            Dim dtmTo As DateTime = StringToDateTime(String.Format("{0} {1}", txtToDate.Text.Trim, txtToTime.Text.Trim))

            ' Constant rule: Date From should not be later than Date To
            If dtmFrom.CompareTo(dtmTo) > 0 Then
                lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00026))
                lstErrorParam1.Add(strFromDateName)
                lstErrorParam2.Add(strToDateName)

                imgErrorFromDate.Visible = True
                imgErrorToDate.Visible = True

            End If

            ' Limit between 2 dates
            If strFromToLimit.Contains("s") Then
                ' sh: same hour
                ' sd: same day
                ' sm: same month
                ' sy: same year
                Select Case strFromToLimit
                    Case "sh"
                        If Not (dtmFrom.Date = dtmTo.Date AndAlso dtmFrom.Hour = dtmTo.Hour) Then
                            lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00028))
                            lstErrorParam1.Add(strFromDateName)
                            lstErrorParam2.Add(strToDateName)

                            imgErrorFromDate.Visible = True
                            imgErrorToDate.Visible = True
                        End If

                    Case "sd"
                        If Not (dtmFrom.Date = dtmTo.Date) Then
                            lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00029))
                            lstErrorParam1.Add(strFromDateName)
                            lstErrorParam2.Add(strToDateName)

                            imgErrorFromDate.Visible = True
                            imgErrorToDate.Visible = True
                        End If

                    Case "sm"
                        If Not (dtmFrom.Year = dtmTo.Year AndAlso dtmFrom.Month = dtmTo.Month) Then
                            lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00030))
                            lstErrorParam1.Add(strFromDateName)
                            lstErrorParam2.Add(strToDateName)

                            imgErrorFromDate.Visible = True
                            imgErrorToDate.Visible = True
                        End If

                    Case "sy"
                        If Not (dtmFrom.Year = dtmTo.Year) Then
                            lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00031))
                            lstErrorParam1.Add(strFromDateName)
                            lstErrorParam2.Add(strToDateName)

                            imgErrorFromDate.Visible = True
                            imgErrorToDate.Visible = True
                        End If

                End Select

            End If

        End If

        'Check From To Day Diff
        'CRE13-001-02 EHAPP Phase 2 [Start][Karl]
        If String.IsNullOrEmpty(strFromToDayDiff) = False AndAlso blnInvalid = False AndAlso blnMissInput = False AndAlso blnIncomplete = False Then
            Dim dtmFrom As DateTime = StringToDateTime(String.Format("{0} {1}", txtFromDate.Text.Trim, txtFromTime.Text.Trim))
            Dim dtmTo As DateTime = StringToDateTime(String.Format("{0} {1}", txtToDate.Text.Trim, txtToTime.Text.Trim))

            If DateDiff(DateInterval.Day, dtmFrom, dtmTo) + 1 > CInt(strFromToDayDiff) Then
                Dim strError As String = """" & strFromDateName & """ and """ & strToDateName & """"

                lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00032))
                lstErrorParam1.Add(strError)
                lstErrorParam2.Add(strFromToDayDiff)

                imgErrorFromDate.Visible = True
                imgErrorToDate.Visible = True
            End If

        End If

        'CRE13-001-02 EHAPP Phase 2 [End][Karl]

    End Sub

    Public Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage) Implements IReportCriteriaUC.ValidateCriteriaInput
        Throw New NotImplementedException
    End Function

    Public Function GetParameterString(ByVal dicSetting As Dictionary(Of String, String)) As String Implements IReportCriteriaUC.GetParameterString
        Dim strFromDateText As String = "Date From"
        If dicSetting.ContainsKey("FromDateText") Then strFromDateText = dicSetting("FromDateText")

        Dim blnToDateVisible As Boolean = True
        If dicSetting.ContainsKey("ToDateVisible") Then blnToDateVisible = dicSetting("ToDateVisible") = "T"

        If blnToDateVisible Then
            Dim strToDateText As String = "Date To"
            If dicSetting.ContainsKey("ToDateText") Then strToDateText = dicSetting("ToDateText")
            Return String.Format("{0}: {1} {2}, {3}: {4} {5}", strFromDateText, txtFromDate.Text, txtFromTime.Text, strToDateText, txtToDate.Text, txtToTime.Text).Trim
        End If

        Return String.Format("{0}: {1} {2}", strFromDateText, txtFromDate.Text, txtFromTime.Text).Trim

    End Function

    Public Function GetParameterList(ByVal dicSetting As Dictionary(Of String, String)) As ParameterCollection Implements IReportCriteriaUC.GetParameterList
        Dim udtParameterList As New ParameterCollection

        Dim strFromDateText As String = "Date From"
        If dicSetting.ContainsKey("FromDateText") Then strFromDateText = dicSetting("FromDateName")
        udtParameterList.AddParam(strFromDateText, String.Format("{0} {1}", txtFromDate.Text, txtFromTime.Text).Trim)

        Dim blnToDateVisible As Boolean = True
        If dicSetting.ContainsKey("ToDateVisible") Then blnToDateVisible = dicSetting("ToDateVisible") = "T"

        If blnToDateVisible Then
            Dim strToDateText As String = "Date To"
            If dicSetting.ContainsKey("ToDateText") Then strToDateText = dicSetting("ToDateName")
            udtParameterList.AddParam(strToDateText, String.Format("{0} {1}", txtToDate.Text, txtToTime.Text).Trim)
        End If

        Return udtParameterList
    End Function

    Public Function GetCriteriaInput(ByVal dicSetting As Dictionary(Of String, String), ByVal strParameterSuffix As String) As StoreProcParamCollection Implements IReportCriteriaUC.GetCriteriaInput
        Dim udtStoreProcParamList As New StoreProcParamCollection

        Dim udtFormatter As New Formatter
        Dim strFrom As String = String.Empty
        Dim strTo As String = String.Empty

        If txtFromDate.Text.Trim <> String.Empty Then
            strFrom = StringToDateTime(txtFromDate.Text, txtFromTime.Text).ToString("yyyy-MMM-dd HH:mm")
        End If

        udtStoreProcParamList.AddParam(String.Format("@From_Date{0}", strParameterSuffix), System.Data.SqlDbType.VarChar, 17, strFrom)

        Dim blnToDateVisible As Boolean = True
        If dicSetting.ContainsKey("ToDateVisible") Then blnToDateVisible = dicSetting("ToDateVisible") = "T"

        If blnToDateVisible Then
            If txtToDate.Text.Trim <> String.Empty Then
                strTo = StringToDateTime(txtToDate.Text, txtToTime.Text).ToString("yyyy-MMM-dd HH:mm")
            End If

            udtStoreProcParamList.AddParam(String.Format("@To_Date{0}", strParameterSuffix), System.Data.SqlDbType.VarChar, 17, strTo)
        End If

        Return udtStoreProcParamList

    End Function

    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Function GetReportGenerationDate(ByVal dicSetting As Dictionary(Of String, String)) As DateTime? Implements IReportCriteriaUC.GetReportGenerationDate
        Dim dtmLatestDate As DateTime? = Nothing
        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim dtmCurrentDate As DateTime = udtGeneral.GetSystemDateTime().Date

        Dim blnDetermineReportGenDate As Boolean = False
        If dicSetting.ContainsKey("DetermineReportGenDate") Then blnDetermineReportGenDate = dicSetting("DetermineReportGenDate") = "T"

        If blnDetermineReportGenDate Then
            If txtFromDate.Text.Trim <> String.Empty Then
                Dim dtmFrom As DateTime = StringToDateTime(String.Format("{0} {1}", txtFromDate.Text.Trim, txtFromTime.Text.Trim))
                dtmLatestDate = dtmFrom
            End If

            Dim blnToDateVisible As Boolean = True
            If dicSetting.ContainsKey("ToDateVisible") Then blnToDateVisible = dicSetting("ToDateVisible") = "T"

            If blnToDateVisible Then
                If txtToDate.Text.Trim <> String.Empty Then
                    Dim dtmTo As DateTime = StringToDateTime(String.Format("{0} {1}", txtToDate.Text.Trim, txtToTime.Text.Trim))
                    dtmLatestDate = dtmTo
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

    ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [Start][Tommy L]
    ' -------------------------------------------------------------------------------------------------------------------
    ' Add the feature to allow including Time value or not
    Private Function DateToString(ByVal dtmValue As DateTime, Optional ByVal IncludeTime As Boolean = True) As String
        If IncludeTime Then
            Return (New Formatter).convertDateTime(dtmValue, String.Empty)
        Else
            Return (New Formatter).convertDate(dtmValue.ToString("dd-MM-yyyy"), String.Empty)
        End If
    End Function
    ' CRE12-004 Enable eHS back-office to make templates of regular statistics for scheme administration [End][Tommy L]

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

#End Region

End Class