Imports System.Data.SqlTypes
Imports System.Globalization
Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format
Imports Common.Validation

Partial Public Class udcDateFormatMonthAndYear
    Inherits StatisticsCriteriaUC

#Region "Variables"

    'Private _dicSetting As New Dictionary(Of String, Dictionary(Of String, String))
    Dim udtFormatter As New Formatter
    Dim udtValidator As New Validator

#End Region

#Region "Session and Const"

    Private Class VS
        Public Const DateFormat As String = "DateFormat"
    End Class

    Public Class Field
        Public Const FromDate As String = "FromDate"
        Public Const ToDate As String = "ToDate"
    End Class

    Public Class CustomFieldSetting
        Public Const UseCutOffDate As String = "UseCutOffDate"
        Public Const AllowFutureDate As String = "AllowFutureDate"
        Public Const AllowPastDate As String = "AllowPastDate"
        Public Const AllowToday As String = "AllowToday"
        Public Const EarliestDate As String = "EarliestDate"
        Public Const ExactDateMaxRange As String = "ExactDateMaxRange"
        Public Const LatestDate As String = "LatestDate"
        Public Const MonthAndYearMaxRange As String = "MonthAndYearMaxRange"
    End Class

    Public Class FunctionCode
        Public Const Funct_010704 As String = "010704"
    End Class

    Public Class rbtnListItem
        Public Const ExactDate As String = "E"
        Public Const MonthAndYear As String = "M"
    End Class

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
        Public Const CurrentMonth As String = "current month"
        Public Const FutureDate As String = "future date"
        Public Const FutureMonth As String = "future month"
        Public Const PastDate As String = "past date"
        Public Const PastMonth As String = "past month"
    End Class

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblFromDateDummy.Attributes.Add("onclick", "FromDateInit('" + lblFromDateDummy.ClientID + "','" + txtFromDate_MY.ClientID + "','" + btnFromDateDummy.ClientID + "','" + btnFromDate_MY.ClientID + "');CalendarFromDateClick();return false;")
        lblFromDateDummy.Attributes.Add("onchange", "ChangeFromDateMMYYYYToDDMMYYYY('" + lblFromDateDummy.ClientID + "','" + txtFromDate_MY.ClientID + "','" + btnFromDateDummy.ClientID + "','" + btnFromDate_MY.ClientID + "')")
        btnFromDateDummy.Attributes.Add("onclick", "FromDateInit('" + lblFromDateDummy.ClientID + "','" + txtFromDate_MY.ClientID + "','" + btnFromDateDummy.ClientID + "','" + btnFromDate_MY.ClientID + "');CalendarFromDateClick();return false;")

        btnFromDate_MY.Style.Add("display", "none")

        lblToDateDummy.Attributes.Add("onclick", "ToDateInit('" + lblToDateDummy.ClientID + "','" + txtToDate_MY.ClientID + "','" + btnToDateDummy.ClientID + "','" + btnToDate_MY.ClientID + "');CalendarToDateClick();return false;")
        lblToDateDummy.Attributes.Add("onchange", "ChangeToDateMMYYYYToDDMMYYYY('" + lblToDateDummy.ClientID + "','" + txtToDate_MY.ClientID + "','" + btnToDateDummy.ClientID + "','" + btnToDate_MY.ClientID + "')")
        btnToDateDummy.Attributes.Add("onclick", "ToDateInit('" + lblToDateDummy.ClientID + "','" + txtToDate_MY.ClientID + "','" + btnToDateDummy.ClientID + "','" + btnToDate_MY.ClientID + "');CalendarToDateClick();return false;")

        btnToDate_MY.Style.Add("display", "none")

        ScriptManager.RegisterStartupScript(Me, GetType(Page), "resetCalendarFrom", "setTimeout(" + Chr(34) + "ResetCalendarFromDate('" & lblFromDateDummy.ClientID & "')" + Chr(34) + ", 1);", True)
        ScriptManager.RegisterStartupScript(Me, GetType(Page), "resetCalendarTo", "setTimeout(" + Chr(34) + "ResetCalendarToDate('" & lblToDateDummy.ClientID & "')" + Chr(34) + ", 1);", True)

    End Sub

#End Region

#Region "Implement IStatisticsCriteriaUC"

    Public Overrides Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
        ' Control setting file
        '_dicSetting = dicSetting

        MyBase.Build(dicSetting)

        ' Initial control
        InitControl()

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        SetErrorComponentVisibility(False)

        Dim blnValid As Boolean = True
        Dim blnFromDateValid As Boolean = True
        Dim blnToDateValid As Boolean = True

        ' Check the dates
        Dim strFromDateText As String = String.Empty

        If IsExistValue(Field.FromDate, FieldSetting.DescResource) Then
            strFromDateText = Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource))
        Else
            strFromDateText = Me.GetGlobalResourceObject("Text", "Date")
        End If

        Dim dtmFromOriginal As New DateTime
        Dim dtmToOriginal As New DateTime
        Dim dtmFrom As New DateTime
        Dim dtmTo As New DateTime

        'Format the input
        txtFromDate_MY.Text = udtFormatter.formatInputDate(txtFromDate_MY.Text.Trim)
        txtToDate_MY.Text = udtFormatter.formatInputDate(txtToDate_MY.Text.Trim)

        If blnFromDateValid Then
            If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
                If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then

                    Dim sm As SystemMessage = Nothing
                    sm = udtValidator.chkInputDate(txtFromDate_MY.Text, True, False)

                    If Not sm Is Nothing Then
                        lstError.Add(sm)
                        lstErrorParam1.Add(strFromDateText)
                        lstErrorParam2.Add(String.Empty)
                        imgErrorDate_MY.Visible = True

                        blnFromDateValid = False
                    End If
                End If
            End If
        End If

        If blnFromDateValid And blnToDateValid Then
            If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
                If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then

                    Dim sm As SystemMessage = Nothing
                    sm = udtValidator.chkInputDate(txtToDate_MY.Text, True, False)

                    If Not sm Is Nothing Then
                        lstError.Add(sm)
                        lstErrorParam1.Add(strFromDateText)
                        lstErrorParam2.Add(String.Empty)
                        imgErrorDate_MY.Visible = True

                        blnToDateValid = False
                    End If
                End If
            End If
        End If

        If blnFromDateValid And blnToDateValid Then
            'Convert to DateTime Type
            dtmFromOriginal = StringToDateTime(txtFromDate_MY.Text)
            dtmFrom = (New Date(dtmFromOriginal.Year, dtmFromOriginal.Month, 1))

            If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
                If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                    dtmToOriginal = StringToDateTime(txtToDate_MY.Text)
                    dtmTo = (New Date(dtmToOriginal.Year, dtmToOriginal.Month, 1)).AddMonths(1).AddDays(-1)
                End If
            End If
        Else
            blnFromDateValid = False
            blnToDateValid = False
        End If

        ' From Date
        ' Earliest Date
        If blnFromDateValid Then
            If IsExistValue(Field.FromDate, CustomFieldSetting.EarliestDate) Then
                Dim strValidationDate As String = GetSetting(Field.FromDate, CustomFieldSetting.EarliestDate)
                If Not strValidationDate Is String.Empty Then
                    Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                    If dtmFrom.CompareTo(dtmValidationDate) < 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00370))
                        lstErrorParam1.Add(strFromDateText)
                        lstErrorParam2.Add(dtmValidationDate.ToString("MMMM, yyyy"))
                        imgErrorDate_MY.Visible = True

                        blnFromDateValid = False
                    End If
                End If
            End If
        End If

        ' Latest Date
        If blnFromDateValid Then
            If IsExistValue(Field.FromDate, CustomFieldSetting.LatestDate) Then
                Dim strValidationDate As String = GetSetting(Field.FromDate, CustomFieldSetting.LatestDate)
                If Not strValidationDate Is String.Empty Then
                    Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                    If dtmFrom.CompareTo(dtmValidationDate) > 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00371))
                        lstErrorParam1.Add(strFromDateText)
                        lstErrorParam2.Add(dtmValidationDate.ToString("MMMM, yyyy"))
                        imgErrorDate_MY.Visible = True

                        blnFromDateValid = False
                    End If
                End If
            End If
        End If

        ' Allow Past Date
        If blnFromDateValid Then
            If IsExistValue(Field.FromDate, CustomFieldSetting.AllowPastDate) Then
                Dim strAllowPastDate As String = GetSetting(Field.FromDate, CustomFieldSetting.AllowPastDate)
                If Not strAllowPastDate Is String.Empty Then
                    Dim dtmValidationDate As DateTime = DateTime.Today

                    ' If allow past date value is N, do checking
                    If strAllowPastDate = Condition.NO Then
                        If dtmFrom.CompareTo(dtmValidationDate) < 0 Then
                            ' To date is visible, use From + To Date
                            lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00378))
                            lstErrorParam1.Add(strFromDateText)
                            lstErrorParam2.Add(DateValidationDesc.PastMonth)
                            imgErrorDate_MY.Visible = True

                            blnFromDateValid = False
                        End If
                    End If
                End If
            End If
        End If

        ' Allow Today
        If blnFromDateValid Then
            If IsExistValue(Field.FromDate, CustomFieldSetting.AllowToday) Then
                Dim strAllowToday As String = GetSetting(Field.FromDate, CustomFieldSetting.AllowToday)
                If Not strAllowToday Is String.Empty Then
                    Dim dtmValidationDate As DateTime = New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1)

                    ' If allow future date value is N, do checking
                    If strAllowToday = Condition.NO Then
                        If dtmFrom.CompareTo(dtmValidationDate) = 0 Then
                            lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00378))
                            lstErrorParam1.Add(strFromDateText)
                            lstErrorParam2.Add(DateValidationDesc.CurrentMonth)
                            imgErrorDate_MY.Visible = True

                            blnFromDateValid = False
                        End If
                    End If
                End If
            End If
        End If

        ' Allow Future Date
        If blnFromDateValid Then
            If IsExistValue(Field.FromDate, CustomFieldSetting.AllowFutureDate) Then
                Dim strAllowFutureDate As String = GetSetting(Field.FromDate, CustomFieldSetting.AllowFutureDate)
                If Not strAllowFutureDate Is String.Empty Then
                    Dim dtmValidationDate As DateTime = DateTime.Today

                    ' If allow future date value is N, do checking
                    If strAllowFutureDate = Condition.NO Then
                        If dtmFrom.CompareTo(dtmValidationDate) > 0 Then
                            lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00378))
                            lstErrorParam1.Add(strFromDateText)
                            lstErrorParam2.Add(DateValidationDesc.FutureMonth)
                            imgErrorDate_MY.Visible = True

                            blnFromDateValid = False
                        End If
                    End If
                End If
            End If
        End If

        ' Use Cut off date
        If blnFromDateValid Then
            If IsExistValue(Field.FromDate, CustomFieldSetting.UseCutOffDate) Then
                Dim strAllowCutOffDate As String = GetSetting(Field.FromDate, CustomFieldSetting.UseCutOffDate)
                If Not strAllowCutOffDate Is String.Empty Then
                    Dim dtmCutOffDate As DateTime = (New StatisticsBLL).GetDataCutOffDate(StatisticsBLL.EnumDataCutOffID.dbEVS_Enquiry)
                    Dim dtmValidationDate As DateTime = New DateTime(dtmCutOffDate.Year, dtmCutOffDate.Month, dtmCutOffDate.Day)

                    ' If allow cut off date value is Y, do checking
                    If strAllowCutOffDate = Condition.YES Then
                        If dtmFrom.CompareTo(dtmValidationDate) > 0 Then
                            lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00371))
                            lstErrorParam1.Add(strFromDateText)
                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                            imgErrorDate_MY.Visible = True

                            blnFromDateValid = False
                        End If
                    End If
                End If
            End If
        End If

        ' To Date
        ' Earliest Date
        If blnToDateValid Then
            If IsExistValue(Field.ToDate, CustomFieldSetting.EarliestDate) Then
                Dim strValidationDate As String = GetSetting(Field.ToDate, CustomFieldSetting.EarliestDate)
                If Not strValidationDate Is String.Empty Then
                    Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                    If dtmTo.CompareTo(dtmValidationDate) < 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00372))
                        lstErrorParam1.Add(strFromDateText)
                        lstErrorParam2.Add(dtmValidationDate.ToString("MMMM, yyyy"))
                        imgErrorDate_MY.Visible = True

                        blnToDateValid = False
                    End If
                End If
            End If
        End If

        ' Latest Date
        If blnToDateValid Then
            If IsExistValue(Field.ToDate, CustomFieldSetting.LatestDate) Then
                Dim strValidationDate As String = GetSetting(Field.ToDate, CustomFieldSetting.LatestDate)
                If Not strValidationDate Is String.Empty Then
                    Dim dtmValidationDate As DateTime = CType(strValidationDate, DateTime)

                    If dtmTo.CompareTo(dtmValidationDate) > 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00373))
                        lstErrorParam1.Add(strFromDateText)
                        lstErrorParam2.Add(dtmValidationDate.ToString("MMMM, yyyy"))
                        imgErrorDate_MY.Visible = True

                        blnToDateValid = False
                    End If
                End If
            End If
        End If

        ' Allow Past Date
        If blnToDateValid Then
            If IsExistValue(Field.ToDate, CustomFieldSetting.AllowPastDate) Then
                Dim strAllowPassDate As String = GetSetting(Field.ToDate, CustomFieldSetting.AllowPastDate)
                If Not strAllowPassDate Is String.Empty Then
                    Dim dtmValidationDate As DateTime = DateTime.Today

                    ' If allow past date value is N, do checking
                    If strAllowPassDate = Condition.NO Then
                        If dtmTo.CompareTo(dtmValidationDate) < 0 Then
                            lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00379))
                            lstErrorParam1.Add(strFromDateText)
                            lstErrorParam2.Add(DateValidationDesc.PastMonth)
                            imgErrorDate_MY.Visible = True

                            blnToDateValid = False
                        End If
                    End If
                End If
            End If
        End If

        ' Allow Today
        If blnToDateValid Then
            If IsExistValue(Field.ToDate, CustomFieldSetting.AllowToday) Then
                Dim strAllowToday As String = GetSetting(Field.ToDate, CustomFieldSetting.AllowToday)
                If Not strAllowToday Is String.Empty Then
                    Dim dtmValidationDate As DateTime = New DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(1).AddDays(-1)

                    ' If allow future date value is N, do checking
                    If strAllowToday = Condition.NO Then
                        If dtmTo.CompareTo(dtmValidationDate) = 0 Then
                            lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00379))
                            lstErrorParam1.Add(strFromDateText)
                            lstErrorParam2.Add(DateValidationDesc.CurrentMonth)
                            imgErrorDate_MY.Visible = True

                            blnToDateValid = False
                        End If
                    End If
                End If
            End If
        End If

        ' Allow Future Date
        If blnToDateValid Then
            If IsExistValue(Field.ToDate, CustomFieldSetting.AllowFutureDate) Then
                Dim strAllowFutureDate As String = GetSetting(Field.ToDate, CustomFieldSetting.AllowFutureDate)
                If Not strAllowFutureDate Is String.Empty Then
                    Dim dtmValidationDate As DateTime = DateTime.Today

                    ' If allow future date value is N, do checking
                    If strAllowFutureDate = Condition.NO Then
                        If dtmTo.CompareTo(dtmValidationDate) > 0 Then
                            lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00379))
                            lstErrorParam1.Add(strFromDateText)
                            lstErrorParam2.Add(DateValidationDesc.FutureMonth)
                            imgErrorDate_MY.Visible = True

                            blnToDateValid = False
                        End If
                    End If
                End If
            End If
        End If

        ' Allow Cut off date
        If blnToDateValid Then
            If IsExistValue(Field.ToDate, CustomFieldSetting.UseCutOffDate) Then
                Dim strAllowCutOffDate As String = GetSetting(Field.ToDate, CustomFieldSetting.UseCutOffDate)
                If Not strAllowCutOffDate Is String.Empty Then
                    Dim dtmCutOffDate As DateTime = (New StatisticsBLL).GetDataCutOffDate(StatisticsBLL.EnumDataCutOffID.dbEVS_Enquiry)
                    Dim dtmValidationDate As DateTime = New DateTime(dtmCutOffDate.Year, dtmCutOffDate.Month, dtmCutOffDate.Day)

                    ' If allow cut off date value is Y, do checking
                    If strAllowCutOffDate = Condition.YES Then
                        If dtmTo.CompareTo(dtmValidationDate) > 0 Then
                            lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00373))
                            lstErrorParam1.Add(strFromDateText)
                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", "CutoffDate"))
                            imgErrorDate_MY.Visible = True

                            blnToDateValid = False
                        End If
                    End If
                End If
            End If
        End If

        blnValid = blnFromDateValid And blnToDateValid

        ' Check From Date <= To Date?
        If blnValid Then
            If IsExistValue(Field.FromDate, FieldSetting.Visible) AndAlso IsExistValue(Field.ToDate, FieldSetting.Visible) Then
                If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES AndAlso GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then

                    ' Is From Date <= To Date?
                    If dtmFrom.Subtract(dtmTo).Days > 0 Then
                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00374))
                        lstErrorParam1.Add(strFromDateText)
                        lstErrorParam2.Add(String.Empty)
                        imgErrorDate_MY.Visible = True

                        blnValid = False
                    End If

                End If
            End If
        End If

        ' Check date difference between From Date and To Date
        If blnValid Then
            If IsExistValue(Field.FromDate, FieldSetting.Visible) AndAlso IsExistValue(Field.ToDate, FieldSetting.Visible) Then
                If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES AndAlso GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                    If IsExistValue(Field.FromDate, CustomFieldSetting.MonthAndYearMaxRange) AndAlso IsExistValue(Field.ToDate, CustomFieldSetting.MonthAndYearMaxRange) Then

                        Dim strFromMonthAndYearInterval As String = GetSetting(Field.FromDate, CustomFieldSetting.MonthAndYearMaxRange)
                        Dim strToMonthAndYearInterval As String = GetSetting(Field.ToDate, CustomFieldSetting.MonthAndYearMaxRange)

                        If strFromMonthAndYearInterval <> String.Empty And strToMonthAndYearInterval <> String.Empty And strFromMonthAndYearInterval = strToMonthAndYearInterval Then
                            Dim strDatePart As String
                            Dim intDateInterval As Integer

                            strDatePart = Left(strFromMonthAndYearInterval, 1)
                            intDateInterval = CInt(Mid(strFromMonthAndYearInterval, 2, strFromMonthAndYearInterval.Length - 1))

                            Dim dtmEffectiveToDate As DateTime = DateTime.MinValue
                            Dim strDateInterval As String = String.Empty

                            Select Case strDatePart
                                Case Datepart.Year
                                    dtmEffectiveToDate = DateAdd(DateInterval.DayOfYear, -1, DateAdd(DateInterval.Year, intDateInterval, dtmFrom))
                                    strDateInterval = CStr(intDateInterval) + " " + IIf(intDateInterval > 1, DatepartDesc.Years, DatepartDesc.Year)
                                Case Datepart.Month
                                    dtmEffectiveToDate = DateAdd(DateInterval.DayOfYear, -1, DateAdd(DateInterval.Month, intDateInterval, dtmFrom))
                                    strDateInterval = CStr(intDateInterval) + " " + IIf(intDateInterval > 1, DatepartDesc.Months, DatepartDesc.Month)
                                    'Case Datepart.DayOfYear
                                    '    dtmEffectiveToDate = DateAdd(DateInterval.DayOfYear, -1, DateAdd(DateInterval.DayOfYear, intDateInterval, dtmFrom))
                                    '    strDateInterval = CStr(intDateInterval) + " " + IIf(intDateInterval > 1, DatepartDesc.DaysOfYear, DatepartDesc.DayOfYear)
                                Case Else
                                    Throw New Exception("Invalid settings in field value '" + CustomFieldSetting.MonthAndYearMaxRange + "' of DB table 'StatisticCriteriaAdditionDetail_SCAD'. The value should be either '" + Datepart.Year + "' or '" + Datepart.Month + "' with numeric value such as 'Y1' or 'M12'.")
                            End Select

                            ' Check: To Date within the pre-defined period
                            If dtmTo.Subtract(dtmEffectiveToDate).Days > 0 Then
                                lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00377))
                                lstErrorParam1.Add(strFromDateText)
                                lstErrorParam2.Add(strDateInterval)
                                imgErrorDate_MY.Visible = True

                                blnValid = False
                            End If
                        End If
                    End If
                End If
            End If
        End If

    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strFromDataValue As String = String.Empty
        Dim strToDataValue As String = String.Empty
        Dim blnFromDateVisible As Boolean = False
        Dim blnToDateVisible As Boolean = False
        Dim strDateFormat As String = "Date"

        ' From Date
        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then
                blnFromDateVisible = True

                If txtFromDate_MY.Text.Trim = String.Empty Then
                    strFromDataValue = String.Empty
                Else
                    strFromDataValue = txtFromDate_MY.Text.Trim
                End If

            End If
        End If

        ' To Date
        If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                blnToDateVisible = True

                If txtToDate_MY.Text.Trim = String.Empty Then
                    strToDataValue = String.Empty
                Else
                    strToDataValue = txtToDate_MY.Text.Trim
                End If

                'udtParameterList.AddParam(strToDateFormat, strToDataValue)
            End If
        End If

        'Both From & To Date
        If blnFromDateVisible And blnToDateVisible Then
            udtParameterList.AddParam(strDateFormat, String.Format("{0} to {1}", strFromDataValue, strToDataValue))
        End If

        'Only From Date
        If blnFromDateVisible Then
            udtParameterList.AddParam(strDateFormat, strFromDataValue)
        End If

        'Only To Date
        If blnToDateVisible Then
            udtParameterList.AddParam(strDateFormat, strToDataValue)
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strFromDataValue As String = String.Empty
        Dim strToDataValue As String = String.Empty
        Dim blnFromDateVisible As Boolean = False
        Dim blnToDateVisible As Boolean = False
        Dim strDateFormat As String = "Date"

        ' From Date
        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then
                blnFromDateVisible = True

                If txtFromDate_MY.Text.Trim = String.Empty Then
                    strFromDataValue += String.Empty
                Else
                    strFromDataValue += txtFromDate_MY.Text.Trim
                End If

            End If
        End If

        ' To Date
        If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then
                blnToDateVisible = True

                If txtToDate_MY.Text.Trim = String.Empty Then
                    strToDataValue = String.Empty
                Else
                    strToDataValue = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, StringToDateTime(txtToDate_MY.Text.Trim))).ToString("dd-MM-yyyy")
                End If

            End If
        End If

        'Both From & To Date
        If blnFromDateVisible And blnToDateVisible Then
            udtParameterList.AddParam(strDateFormat, String.Format("{0} to {1}", strFromDataValue, strToDataValue))
        End If

        'Only From Date
        If blnFromDateVisible And Not blnToDateVisible Then
            udtParameterList.AddParam(strDateFormat, strFromDataValue)
        End If

        'Only To Date
        If blnToDateVisible And Not blnFromDateVisible Then
            udtParameterList.AddParam(strDateFormat, strToDataValue)
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' From Date
        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            If GetSetting(Field.FromDate, FieldSetting.Visible) = Condition.YES Then

                Dim dtmPeriodFrom As DateTime
                Dim blnPeriodFrom As Boolean = False

                If txtFromDate_MY.Text.Trim <> String.Empty Then
                    dtmPeriodFrom = StringToDateTime(txtFromDate_MY.Text.Trim)
                    blnPeriodFrom = True
                End If

                If IsExistValue(Field.FromDate, FieldSetting.SPParamName) AndAlso blnPeriodFrom = True Then
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

        ' To Date
        If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            If GetSetting(Field.ToDate, FieldSetting.Visible) = Condition.YES Then

                Dim dtmPeriodTo As DateTime
                Dim blnPeriodTo As Boolean = False

                If txtToDate_MY.Text.Trim <> String.Empty Then
                    dtmPeriodTo = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Month, 1, StringToDateTime(txtToDate_MY.Text.Trim)))
                    blnPeriodTo = True
                End If

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
        'imgErrorFromDate_MY.Visible = blnVisible
        'imgErrorToDate_MY.Visible = blnVisible
        imgErrorDate_MY.Visible = blnVisible
    End Sub

#End Region

#Region "Supporting Function"

    Public Overrides Sub InitControl()
        ' Set item - Date Format    
        SetDateFormat()

    End Sub


    Private Function StringToDateTime(ByVal strValue As String) As DateTime
        strValue = strValue.Trim

        Select Case strValue.Length
            Case 10
                ' dd-MM-yyyy
                Dim dtmDate As DateTime

                If DateTime.TryParseExact(strValue, (New Formatter).EnterDateFormat, Nothing, DateTimeStyles.None, dtmDate) Then
                    Return (New Formatter).convertDate(strValue, String.Empty)
                Else
                    Throw New Exception(String.Format("DateFormatFromToDate.StringToDateTime: Unexpected value: {0}", strValue))
                End If

            Case Else
                Throw New Exception(String.Format("DateFormatFromToDate.StringToDateTime: Unexpected value: {0}", strValue))
        End Select

        Return Nothing

    End Function

    Private Function DateToString(ByVal dtmValue As DateTime) As String
        Return (New Formatter).convertDateTime(dtmValue, String.Empty)
    End Function

    Private Sub InitializeDateValue()
        txtFromDate_MY.Text = String.Empty
        txtToDate_MY.Text = String.Empty

        lblFromDateDummy.Text = "&nbsp;"
        lblToDateDummy.Text = "&nbsp;"

        imgErrorDate_MY.Visible = False
    End Sub

#End Region

#Region "Fields Setting"

    ' Set item - Date Format
    Private Sub SetDateFormat()
        ' Set field description
        If IsExistValue(Field.FromDate, FieldSetting.DescResource) Then
            lblDateText.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.FromDate, FieldSetting.DescResource))
        End If

        If IsExistValue(Field.ToDate, FieldSetting.DescResource) Then
            lblToDate_MY.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.ToDate, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.FromDate, FieldSetting.Visible) Then
            Select Case GetSetting(Field.FromDate, FieldSetting.Visible)
                Case Condition.YES
                    panDateFormat.Visible = True
                Case Condition.NO
                    panDateFormat.Visible = False
                Case Else
                    panDateFormat.Visible = False
            End Select
        End If

        If IsExistValue(Field.ToDate, FieldSetting.Visible) Then
            Select Case GetSetting(Field.ToDate, FieldSetting.Visible)
                Case Condition.YES
                    pnlToDate.Style.Remove("display")
                Case Condition.NO
                    pnlToDate.Style.Add("display", "none")
                Case Else
                    pnlToDate.Style.Add("display", "none")
            End Select
        End If

        '' Set default value
        'If IsExistValue(Field.DateFormat, FieldSetting.DefaultValue) Then
        '    If Not GetSetting(Field.DateFormat, FieldSetting.DefaultValue) = String.Empty Then

        '        Dim listItem As ListItem = rbtnDateFormat.Items.FindByValue(GetSetting(Field.DateFormat, FieldSetting.DefaultValue))
        '        If Not listItem Is Nothing Then
        '            rbtnDateFormat.SelectedValue = listItem.Value
        '        End If

        '    End If
        'End If

        'If panDateFormat.Visible = True Then
        '    trExactDate.Style.Add("display", "initial")
        '    trMonthAndYear.Style.Add("display", "none")
        'End If
    End Sub

#End Region

End Class