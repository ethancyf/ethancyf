'CRE13-019-02 Extend HCVS to China [Chris YIM]
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.HCVUUser
Imports Common.Component.ExchangeRate
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports System.Drawing

Partial Public Class ConversionRateEnquiry
    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    Inherits BasePageWithControl
    'Inherits BasePageWithGridView
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]
    ' FunctionCode = FunctCode.FUNT010304

    Dim udtConversionRateBLL As ExchangeRateBLL

    Dim udtFormatter As Formatter
    Dim udtGeneralFunction As GeneralFunction

#Region "Private Class"

    Private Class ViewIndexConversionRateEnquiry
        Public Const HiddenNotice As Integer = -1
        Public Const DisplayNotice As Integer = 0
        Public Const DisplayHistory As Integer = 1
    End Class

    Private Class ViewIndexCurrentConversionRateInfo
        Public Const HiddenRecord As Integer = -1
        Public Const NoRecord As Integer = 0
        Public Const CurrentRecord As Integer = 1
    End Class

    Private Class ViewIndexNextConversionRateInfo
        Public Const HiddenRecord As Integer = -1
        Public Const NoRecord As Integer = 0
        Public Const NextRecord As Integer = 1
    End Class

    Private Class ConversionRateCalendar
        Public Const Size As Integer = 7
        Public Const Remark As String = "*"

        Public Const TableCellStyleDefault As Integer = 0
        Public Const TableCellStyleWithDayAndConversionRate As Integer = 1
        Public Const TableCellStyleWithTodayAndConversionRate As Integer = 2
        Public Const TableCellStyleSelectedDate As Integer = 3
        Public Const TableCellStyleUnselectedDate As Integer = 4
    End Class

    Private Class SESS
        Public Const InputedMonth As String = "010412_InputedMonth"
        Public Const InputedYear As String = "010412_InputedYear"

        Public Const SelectedTodayTableCell As String = "010412_SelectedTodayTableCell"
        Public Const SelectedTableCell As String = "010412_SelectedTableCell"
        Public Const SelectedDate As String = "010412_SelectedDate"

        Public Const ConversionRate As String = "010412_ConversionRate"
        Public Const CurrentConversionRate As String = "010412_CurrentConversionRate"
        Public Const NextConversionRate As String = "010412_NextConversionRate"
    End Class

    Private Class DataTableConversionRateForCalendar
        Public Const dcCalendarDay As String = "CalendarDay"
        Public Const dcConversionRateValue As String = "ConversionRate_Value"
        Public Const dcIsEffectiveDate As String = "IsEffectiveDate"

    End Class

    Private Class UIControlPrefix
        Public Const lblCalendarDay As String = "lblCalendarDay_"
        Public Const lblConversionRate As String = "lbtnConversionRate_"
        Public Const lblAsterisk As String = "lblAsterisk_"
        Public Const tblCellCalendarDay As String = "tblCellCalendarDay_"

    End Class

    Private Class MessageBoxHeaderKey
        Public Const ValidationFail As String = "ValidationFail"
    End Class

    Private Class AuditLogDescription
        Public Const LOG00000 As String = "Conversion Rate Enquiry Page Load"

        Public Const LOG00001 As String = "Conversion Rate History click"
        Public Const LOG00002 As String = "Back click"

        Public Const LOG00003 As String = "Conversion Rate History - Calender - Year and Month selected"
        Public Const LOG00004 As String = "Conversion Rate History - Calender - Date selected"
        Public Const LOG00005 As String = "Conversion Rate Record"

    End Class

#End Region


    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
    ' -------------------------------------------------------------------------
#Region "SF Search"

    Protected Overrides Sub SF_InitSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub

    Protected Overrides Function SF_ValidateSearch(ByRef udtAuditLogEntry As Common.ComObject.AuditLogEntry) As Boolean

    End Function

    Protected Overrides Function SF_Search(ByRef udtAuditLogEntry As AuditLogEntry, ByVal blnOverrideResultLimit As Boolean) As Common.Component.BaseBLL.BLLSearchResult
        Return Nothing
    End Function

    Protected Overrides Function SF_BindSearchResult(ByRef udtAuditLogEntry As AuditLogEntry, ByVal udtBLLSearchResult As Common.Component.BaseBLL.BLLSearchResult) As Integer

    End Function

    Protected Overrides Sub SF_ConfirmSearch_Click()

    End Sub

    Protected Overrides Sub SF_FinalizeSearch(ByRef udtAuditLogEntry As AuditLogEntry)

    End Sub

    Protected Overrides Sub SF_CancelSearch_Click()

    End Sub

#End Region
    ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

    Protected Overloads Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        Me.udcInfoMessageBox.Visible = False
        Me.udcMessageBox.Visible = False

        '' Get HCVU User to check session expire
        GetCurrentUser()

        If Not Page.IsPostBack Then
            FunctionCode = FunctCode.FUNT010412

            Dim udtAuditLog As New AuditLogEntry(FunctionCode, Me)

            ResetLayout()
            ClearSession()
            GetCurrentConversionRate(udtAuditLog)
            GetNextConversionRate(udtAuditLog)
            RenderingLayout()

            udtAuditLog.WriteLog(LogID.LOG00000, AuditLogDescription.LOG00000)
        End If

        If mvConversionRateEnquiry.ActiveViewIndex = ViewIndexConversionRateEnquiry.DisplayHistory Then
            If Request.Form("__EVENTTARGET").Equals("txtEffectiveDatePeriod") Then
                txtEffectiveDatePeriod_TextChanged()
            Else
                GenerateConversionRateCalendar(Session(SESS.InputedMonth), Session(SESS.InputedYear))
            End If
        End If

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        'If mvConversionRateEnquiry.ActiveViewIndex = ViewIndexConversionRateEnquiry.DisplayHistory Then
        '    GenerateConversionRateCalendar(Session(SESS.InputedMonth), Session(SESS.InputedYear))
        'End If
    End Sub

    Private Function GetCurrentUser() As String
        Return (New HCVUUserBLL).GetHCVUUser.UserID

    End Function

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Return Nothing
    End Function

#End Region

    Private Sub ClearSession()
        Session(SESS.InputedMonth) = Nothing
        Session(SESS.InputedYear) = Nothing

        Session(SESS.SelectedDate) = Nothing
        Session(SESS.SelectedTableCell) = Nothing
        Session(SESS.SelectedTodayTableCell) = Nothing

        Session(SESS.ConversionRate) = Nothing
        Session(SESS.CurrentConversionRate) = Nothing
        Session(SESS.NextConversionRate) = Nothing
    End Sub

    Private Sub GetConversionRate(ByVal dtmStartDate As DateTime, ByVal dtmLastDate As DateTime)
        Dim udtConversionRateModel As ExchangeRateModel

        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010412, Me)

        udtConversionRateBLL = New ExchangeRateBLL
        udtFormatter = New Formatter

        'If Session(SESS.ConversionRate) Is Nothing Then
        udtConversionRateModel = udtConversionRateBLL.CreateExchangeRateRecordModel(udtConversionRateBLL.GetExchangeRateValue(dtmStartDate, dtmLastDate))
        Session(SESS.ConversionRate) = udtConversionRateModel
        'Else
        '    udtConversionRateModel = Session(SESS.ConversionRate)
        'End If

        If Not udtConversionRateModel Is Nothing Then
            lblConversionRateRecordID.Text = udtConversionRateModel.ExchangeRateID
            lblConversionRateRecordEffectiveDate.Text = udtFormatter.formatDisplayDate(udtConversionRateModel.EffectiveDate)
            lblConversionRateRecordRate.Text = HttpContext.GetGlobalResourceObject("Text", "ConversionRateFormula") & " " & formatConversionRate(udtConversionRateModel.ExchangeRate)
            lblConversionRateRecordCreateBy.Text = udtConversionRateModel.CreateBy
            lblConversionRateRecordCreateDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.CreateDtm, "") & ")"
            lblConversionRateRecordApprovedBy.Text = udtConversionRateModel.ApproveBy
            lblConversionRateRecordApprovedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.ApproveDtm, "") & ")"

            udtAuditLog.AddDescripton("Conversion Rate ID", udtConversionRateModel.ExchangeRateID)
            udtAuditLog.WriteLog(LogID.LOG00005, AuditLogDescription.LOG00005)
        Else
            udtAuditLog.AddDescripton("Conversion Rate ID", String.Empty)
            udtAuditLog.WriteLog(LogID.LOG00005, AuditLogDescription.LOG00005)
        End If
    End Sub

    Private Sub GetCurrentConversionRate(ByRef udtAuditLog As AuditLogEntry)
        Dim udtConversionRateModel As ExchangeRateModel

        udtConversionRateBLL = New ExchangeRateBLL
        udtFormatter = New Formatter

        If Session(SESS.CurrentConversionRate) Is Nothing Then
            udtConversionRateModel = udtConversionRateBLL.CreateExchangeRateRecordModel(udtConversionRateBLL.GetApprovedExchangeRateRecord(ExchangeRateModel.ER_INFO_TYPE_T))
            Session(SESS.CurrentConversionRate) = udtConversionRateModel
        Else
            udtConversionRateModel = Session(SESS.CurrentConversionRate)
        End If

        If Not udtConversionRateModel Is Nothing Then
            mvCurrentConversionRateInfo.ActiveViewIndex = ViewIndexCurrentConversionRateInfo.CurrentRecord

            lblCurrentConversionRateID.Text = udtConversionRateModel.ExchangeRateID
            lblCurrentEffectiveDate.Text = udtFormatter.formatDisplayDate(udtConversionRateModel.EffectiveDate)
            lblCurrentConversionRate.Text = HttpContext.GetGlobalResourceObject("Text", "ConversionRateFormula") & " " & formatConversionRate(udtConversionRateModel.ExchangeRate)
            lblCurrentConversionRateCreateBy.Text = udtConversionRateModel.CreateBy
            lblCurrentConversionRateCreateDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.CreateDtm, "") & ")"
            lblCurrentConversionRateApprovedBy.Text = udtConversionRateModel.ApproveBy
            lblCurrentConversionRateApprovedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.ApproveDtm, "") & ")"

            udtAuditLog.AddDescripton("Current Conversion Rate", udtConversionRateModel.ExchangeRateID.ToString)
        Else
            udtAuditLog.AddDescripton("Current Conversion Rate", String.Empty)
        End If

    End Sub

    Private Sub GetNextConversionRate(ByRef udtAuditLog As AuditLogEntry)
        Dim udtConversionRateModel As ExchangeRateModel

        udtConversionRateBLL = New ExchangeRateBLL
        udtFormatter = New Formatter

        If Session(SESS.NextConversionRate) Is Nothing Then
            udtConversionRateModel = udtConversionRateBLL.CreateExchangeRateRecordModel(udtConversionRateBLL.GetApprovedExchangeRateRecord(ExchangeRateModel.ER_INFO_TYPE_N))
            Session(SESS.NextConversionRate) = udtConversionRateModel
        Else
            udtConversionRateModel = Session(SESS.NextConversionRate)
        End If

        If Not udtConversionRateModel Is Nothing Then
            mvNextConversionRateInfo.ActiveViewIndex = ViewIndexNextConversionRateInfo.NextRecord

            lblNextConversionRateID.Text = udtConversionRateModel.ExchangeRateID
            lblNextEffectiveDate.Text = udtFormatter.formatDisplayDate(udtConversionRateModel.EffectiveDate)
            lblNextConversionRate.Text = HttpContext.GetGlobalResourceObject("Text", "ConversionRateFormula") & " " & formatConversionRate(udtConversionRateModel.ExchangeRate)
            lblNextConversionRateCreateBy.Text = udtConversionRateModel.CreateBy
            lblNextConversionRateCreateDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.CreateDtm, "") & ")"
            lblNextConversionRateApprovedBy.Text = udtConversionRateModel.ApproveBy
            lblNextConversionRateApprovedDtm.Text = "(" & udtFormatter.formatDateTime(udtConversionRateModel.ApproveDtm, "") & ")"

            udtAuditLog.AddDescripton("Next Conversion Rate", udtConversionRateModel.ExchangeRateID.ToString)
        Else
            udtAuditLog.AddDescripton("Next Conversion Rate", String.Empty)
        End If
    End Sub

    Private Sub ResetLayout()
        udcInfoMessageBox.Visible = False
        udcMessageBox.Visible = False

        mvConversionRateEnquiry.ActiveViewIndex = ViewIndexConversionRateEnquiry.DisplayNotice
        mvCurrentConversionRateInfo.ActiveViewIndex = ViewIndexCurrentConversionRateInfo.NoRecord
        mvNextConversionRateInfo.ActiveViewIndex = ViewIndexNextConversionRateInfo.NoRecord

    End Sub

    Private Sub HideLayout()
        mvCurrentConversionRateInfo.ActiveViewIndex = ViewIndexCurrentConversionRateInfo.HiddenRecord
        mvNextConversionRateInfo.ActiveViewIndex = ViewIndexNextConversionRateInfo.HiddenRecord

    End Sub

    Private Sub ResetAlert()
        udcMessageBox.Visible = False
        udcInfoMessageBox.Visible = False
    End Sub

    Private Sub LoadCalendarSetting()
        Dim udtConversionRateBLL As New ExchangeRateBLL
        Dim intYearOfFirstConversionRateRecord, intMonthOfFirstConversionRateRecord As Integer

        intYearOfFirstConversionRateRecord = udtConversionRateBLL.GetExchangeRateEffectiveDate.Year
        intMonthOfFirstConversionRateRecord = udtConversionRateBLL.GetExchangeRateEffectiveDate.Month

        calEffectiveDatePeriod.StartDate = New DateTime(intYearOfFirstConversionRateRecord, intMonthOfFirstConversionRateRecord, 1)
        calEffectiveDatePeriod.EndDate = DateAdd(DateInterval.Day, -1, DateAdd(DateInterval.Year, 1, New DateTime(Today.Year, 1, 1)))

    End Sub

    Function GetYear() As ICollection
        Dim udtConversionRateBLL As New ExchangeRateBLL
        Dim dt As DataTable = New DataTable()
        Dim intYearOfFirstConversionRateRecord As Integer

        dt.Columns.Add(New DataColumn("ddlYearTextField", GetType(String)))
        dt.Columns.Add(New DataColumn("ddlYearValueField", GetType(String)))

        intYearOfFirstConversionRateRecord = udtConversionRateBLL.GetExchangeRateEffectiveDate.Year

        For intYear As Integer = intYearOfFirstConversionRateRecord To Today.Year
            dt.Rows.Add(CreateDropDownListItem(intYear, intYear, dt))
        Next

        Dim dv As DataView = New DataView(dt)
        Return dv

    End Function

    Function GetMonth() As ICollection
        Dim dt As DataTable = New DataTable()

        dt.Columns.Add(New DataColumn("ddlMonthTextField", GetType(String)))
        dt.Columns.Add(New DataColumn("ddlMonthValueField", GetType(String)))

        dt.Rows.Add(CreateDropDownListItem("January", "1", dt))
        dt.Rows.Add(CreateDropDownListItem("February", "2", dt))
        dt.Rows.Add(CreateDropDownListItem("March", "3", dt))
        dt.Rows.Add(CreateDropDownListItem("April", "4", dt))
        dt.Rows.Add(CreateDropDownListItem("May", "5", dt))
        dt.Rows.Add(CreateDropDownListItem("June", "6", dt))
        dt.Rows.Add(CreateDropDownListItem("July", "7", dt))
        dt.Rows.Add(CreateDropDownListItem("August", "8", dt))
        dt.Rows.Add(CreateDropDownListItem("September", "9", dt))
        dt.Rows.Add(CreateDropDownListItem("October", "10", dt))
        dt.Rows.Add(CreateDropDownListItem("November", "11", dt))
        dt.Rows.Add(CreateDropDownListItem("December", "12", dt))

        Dim dv As DataView = New DataView(dt)
        Return dv

    End Function

    Function GetDataTableForConversionRate() As DataTable
        Dim dt As DataTable = New DataTable()

        dt.Columns.Add(New DataColumn(DataTableConversionRateForCalendar.dcCalendarDay, GetType(String)))
        dt.Columns.Add(New DataColumn(DataTableConversionRateForCalendar.dcConversionRateValue, GetType(Decimal)))
        dt.Columns.Add(New DataColumn(DataTableConversionRateForCalendar.dcIsEffectiveDate, GetType(String)))

        Return dt

    End Function

    Function CreateDropDownListItem(ByVal strDropDownListText As String, ByVal strDropDownListValue As String, ByVal dt As DataTable) As DataRow
        Dim dr As DataRow = dt.NewRow()

        dr(0) = strDropDownListText
        dr(1) = strDropDownListValue

        Return dr

    End Function

    Function CreateDataTableItem(ByVal strCalendarDay As String, ByVal decConversionRate As Decimal, ByVal strIsEffectiveDate As String, ByVal dt As DataTable) As DataRow
        Dim dr As DataRow = dt.NewRow()

        dr(0) = strCalendarDay
        dr(1) = decConversionRate
        dr(2) = strIsEffectiveDate

        Return dr

    End Function

    Sub GenerateConversionRateCalendar(ByVal intMonth As Integer, ByVal intYear As Integer)
        Dim udtConversionRateBLL As New ExchangeRateBLL
        Dim dtmFirstDayOfMonth As DateTime = New DateTime(intYear, intMonth, 1)
        Dim dtmLastDayOfMonth As DateTime = (dtmFirstDayOfMonth.AddMonths(1)).AddDays(-1)

        Dim intRowOfCalendar As Integer = Math.Ceiling((Weekday(dtmFirstDayOfMonth) + dtmLastDayOfMonth.Day - 1) / ConversionRateCalendar.Size)
        Dim dtmConversionRateFirstEffectiveDate As DateTime = udtConversionRateBLL.GetExchangeRateEffectiveDate()
        Dim dtConversionRateList As DataTable = GenerateConversionRate(dtmFirstDayOfMonth, dtmLastDayOfMonth)

        Dim intRowNum As Integer
        Dim intCellNum As Integer

        Dim tblRowHeader As New TableRow()

        'Generate Calendar Title(MMMM, yyyy)
        Dim dvMonth As DataView = GetMonth()
        Dim dtMonth As DataTable = dvMonth.ToTable
        lblCalendarMonthYear.Text = dtMonth.Rows(dtmFirstDayOfMonth.Month - 1).Item("ddlMonthTextField") & ", " & dtmFirstDayOfMonth.Year

        calEffectiveDatePeriod.SelectedDate = dtmFirstDayOfMonth
        calEffectiveDatePeriod.DefaultView = AjaxControlToolkit.CalendarDefaultView.Months

        'Generate Calendar Header
        For intCellNum = 1 To ConversionRateCalendar.Size
            Dim tblCellHeader As New TableCell()

            tblCellHeader.Controls.Add(AddHTMLSpan(1))

            tblCellHeader.Text = GetCalendarWeekday(intCellNum)
            tblCellHeader.ApplyStyle(GetTableHeaderStyle())

            tblRowHeader.Cells.Add(tblCellHeader)
        Next
        tblCalendarContent.Rows.Add(tblRowHeader)

        'Generate Calendar Content
        For intRowNum = 1 To intRowOfCalendar
            Dim tblRow As New TableRow()

            For intCellNum = 1 To ConversionRateCalendar.Size
                Dim tblCell As New TableCell()
                Dim intTblCellStyle As Integer = ConversionRateCalendar.TableCellStyleDefault
                Dim intPosition As Integer = (intRowNum - 1) * ConversionRateCalendar.Size + intCellNum

                tblCell.ID = UIControlPrefix.tblCellCalendarDay & intPosition

                'Get day of the selected month
                Dim strCalendarDay = GetCalendarDay(intPosition, Weekday(dtmFirstDayOfMonth), dtmLastDayOfMonth.Day)

                If Not strCalendarDay.Equals(String.Empty) Then
                    'Add space " "
                    tblCell.Controls.Add(AddHTMLSpan(1))

                    'Add label of day in calendar'
                    tblCell.Controls.Add(AddCalendarLabel(UIControlPrefix.lblCalendarDay & strCalendarDay, strCalendarDay))
                    intTblCellStyle = ConversionRateCalendar.TableCellStyleWithDayAndConversionRate
                    tblCell.ApplyStyle(GetTableContentStyle(ConversionRateCalendar.TableCellStyleUnselectedDate))

                    If CInt(strCalendarDay) > 0 And _
                        CInt(strCalendarDay) <= dtmLastDayOfMonth.Day And _
                        DateAdd(DateInterval.Day, CInt(strCalendarDay) - 1, dtmFirstDayOfMonth) <= Today And _
                        CDec(dtConversionRateList.Rows(strCalendarDay - 1).Item(DataTableConversionRateForCalendar.dcConversionRateValue)) > 0 Then

                        If dtConversionRateList.Rows(strCalendarDay - 1).Item(DataTableConversionRateForCalendar.dcIsEffectiveDate).Equals(YesNo.Yes) Then
                            'Add space " "
                            If strCalendarDay.ToString.Length = 1 Then
                                tblCell.Controls.Add(AddHTMLSpan(5))
                            Else
                                tblCell.Controls.Add(AddHTMLSpan(3))
                            End If

                            'Add "*" in calendar for remark that day has a new exchange rate'
                            tblCell.Controls.Add(AddCalendarLabel(UIControlPrefix.lblAsterisk & strCalendarDay, ConversionRateCalendar.Remark, "#FF0000"))
                        End If

                        'Add space " "
                        tblCell.Controls.Add(AddHTMLSpan(1))

                        'Add link button of exchange rate in calendar'
                        tblCell.Controls.Add(AddCalendarLinkButton(UIControlPrefix.lblConversionRate & strCalendarDay, dtConversionRateList.Rows(strCalendarDay - 1).Item(DataTableConversionRateForCalendar.dcConversionRateValue), strCalendarDay))

                        Dim test As DateTime = Session(SESS.SelectedDate)
                        'If selected date, a orange colour highlight sets on that day.
                        If Not Session(SESS.SelectedDate) = Nothing Then
                            If DateAdd(DateInterval.Day, CInt(strCalendarDay) - 1, dtmFirstDayOfMonth) = Session(SESS.SelectedDate) Then
                                tblCell.ApplyStyle(GetTableContentStyle(ConversionRateCalendar.TableCellStyleSelectedDate))
                            End If
                        End If

                        'If Today's exchange rate, a border of red colour is added.
                        If DateAdd(DateInterval.Day, CInt(strCalendarDay) - 1, dtmFirstDayOfMonth) = Today Then
                            Session(SESS.SelectedTodayTableCell) = tblCell.ID
                            intTblCellStyle = ConversionRateCalendar.TableCellStyleWithTodayAndConversionRate

                            'By default, a orange colour highlight sets on today.
                            If Session(SESS.SelectedTableCell) = Nothing Then
                                tblCell.ApplyStyle(GetTableContentStyle(ConversionRateCalendar.TableCellStyleSelectedDate))
                            End If
                        End If
                    End If
                End If

                'Apply table style
                tblCell.ApplyStyle(GetTableContentStyle(intTblCellStyle))

                tblRow.Cells.Add(tblCell)
            Next
            tblCalendarContent.Rows.Add(tblRow)
        Next

    End Sub

    Sub ClearConversionRateCalendar()
        tblCalendarContent.Rows.Clear()
    End Sub

    Protected Sub HandleSelectedConversionRate(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim udtFormatter As New Formatter
        Dim lbtnConversionRate As LinkButton = CType(sender, LinkButton)

        Dim strSelectedDay As String
        strSelectedDay = lbtnConversionRate.CommandArgument.ToString

        Dim dtmSelectedDate As DateTime = New DateTime(Session(SESS.InputedYear), Session(SESS.InputedMonth), strSelectedDay)
        Dim intWeekdayOfFirstDayOfSelectedMonth = Weekday(New DateTime(Session(SESS.InputedYear), Session(SESS.InputedMonth), 1))

        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010412, Me)
        udtAuditLog.AddDescripton("Target Date", udtFormatter.formatDisplayDate(dtmSelectedDate))
        udtAuditLog.WriteLog(LogID.LOG00004, AuditLogDescription.LOG00004)

        Dim tblCell As TableCell

        Dim dtmPreviousSelectedDate As DateTime

        If Not Session(SESS.SelectedDate) Is Nothing Then
            dtmPreviousSelectedDate = Session(SESS.SelectedDate)
        Else
            dtmPreviousSelectedDate = Today
        End If

        Session(SESS.SelectedDate) = dtmSelectedDate

        If Session(SESS.SelectedTableCell) = Nothing Then
            Session(SESS.SelectedTableCell) = Session(SESS.SelectedTodayTableCell)
        End If

        'Remove selected style
        If dtmPreviousSelectedDate.Year = dtmSelectedDate.Year And dtmPreviousSelectedDate.Month = dtmSelectedDate.Month Then
            tblCell = tblCalendarContent.FindControl(Session(SESS.SelectedTableCell))
            tblCell.ApplyStyle(GetTableContentStyle(ConversionRateCalendar.TableCellStyleUnselectedDate))
        End If

        'Add selected style
        tblCell = tblCalendarContent.FindControl(UIControlPrefix.tblCellCalendarDay & CStr(CInt(strSelectedDay) + intWeekdayOfFirstDayOfSelectedMonth - 1))
        Session(SESS.SelectedTableCell) = tblCell.ID

        tblCell.ApplyStyle(GetTableContentStyle(ConversionRateCalendar.TableCellStyleSelectedDate))

        'Set the selected date on the title, e.g. Exchange Rate Record (On dd MMM yyyy).
        lblConversionRateRecord.Text = HttpContext.GetGlobalResourceObject("Text", "ConversionRateRecord") & " (On " & udtFormatter.formatDisplayDate(dtmSelectedDate) & ")"

        'Get the exchange rate record by selected date
        GetConversionRate(dtmSelectedDate, dtmSelectedDate)

    End Sub

    Private Function GenerateConversionRate(ByVal dtmFirstDayOfMonth As DateTime, ByVal dtmLastDayOfMonth As DateTime) As DataTable
        Dim udtConversionRateBLL As New ExchangeRateBLL
        Dim udtFormatter As New Formatter
        Dim dt As DataTable
        Dim dr() As DataRow
        Dim dtConversionRate As DataTable = GetDataTableForConversionRate()

        dt = udtConversionRateBLL.GetExchangeRateValue(dtmFirstDayOfMonth, dtmLastDayOfMonth)

        For i As Integer = dtmFirstDayOfMonth.Day To dtmLastDayOfMonth.Day
            Dim dtmTargetDate As DateTime = DateAdd(DateInterval.Day, i - 1, dtmFirstDayOfMonth)
            dr = dt.Select("Effective_Date <= '" & dtmTargetDate.Year & "-" & dtmTargetDate.Month & "-" & dtmTargetDate.Day & " 00:00:00.000'", "Effective_Date DESC")
            If dr.Length > 0 Then
                If dr(0).Item("Effective_Date") = dtmTargetDate Then
                    dtConversionRate.Rows.Add(CreateDataTableItem(i, dr(0).Item("ExchangeRate_Value"), YesNo.Yes, dtConversionRate))
                Else
                    dtConversionRate.Rows.Add(CreateDataTableItem(i, dr(0).Item("ExchangeRate_Value"), YesNo.No, dtConversionRate))
                End If
            Else
                dtConversionRate.Rows.Add(CreateDataTableItem(i, Decimal.Zero, YesNo.No, dtConversionRate))
            End If
        Next

        Return dtConversionRate
    End Function

    Function AddHTMLSpan(ByVal intNum As Integer) As HtmlGenericControl
        Dim htmlGenericContorlSpan As New HtmlGenericControl("span")

        For cnt As Integer = 1 To intNum
            htmlGenericContorlSpan.InnerHtml += "&nbsp;"
        Next

        Return htmlGenericContorlSpan
    End Function

    Function AddCalendarLabel(ByVal strID As String, ByVal strText As String, Optional ByVal strColor As String = "#666666") As Label
        Dim lblCalendarDay As New Label

        lblCalendarDay.ID = strID
        lblCalendarDay.Text = strText
        lblCalendarDay.Height = Unit.Pixel(27)
        lblCalendarDay.Style.Add("vertical-align", "bottom")
        lblCalendarDay.Style.Add("color", strColor)

        Return lblCalendarDay
    End Function

    Function AddCalendarLinkButton(ByVal strID As String, ByVal strText As String, Optional ByVal strCmdArg As String = "") As LinkButton
        Dim lbtnConversionRate As New LinkButton

        lbtnConversionRate.ID = strID
        lbtnConversionRate.Text = strText
        lbtnConversionRate.CommandArgument = strCmdArg
        lbtnConversionRate.Height = Unit.Pixel(15)
        lbtnConversionRate.Style.Add("color", "#0066CB")

        AddHandler lbtnConversionRate.Click, AddressOf HandleSelectedConversionRate

        Return lbtnConversionRate
    End Function

    Function GetTableHeaderStyle() As TableItemStyle
        Dim tableStyle As New TableItemStyle()

        tableStyle.BackColor = Color.FromArgb(255, 169, 169, 169)

        tableStyle.BorderWidth = "1"
        tableStyle.BorderColor = Color.FromArgb(255, 102, 102, 102)

        tableStyle.Font.Bold = True
        tableStyle.ForeColor = Color.White

        tableStyle.HorizontalAlign = HorizontalAlign.Left
        tableStyle.VerticalAlign = VerticalAlign.Top

        tableStyle.Height = Unit.Pixel(45)
        tableStyle.Width = Unit.Pixel(42)

        Return tableStyle

    End Function

    Private Function GetTableContentStyle(ByVal intTblCellStyle As Integer) As TableItemStyle
        Dim tableStyle As New TableItemStyle()

        tableStyle.Font.Bold = True

        tableStyle.HorizontalAlign = HorizontalAlign.Left
        tableStyle.VerticalAlign = VerticalAlign.Top

        tableStyle.Height = Unit.Pixel(45)
        tableStyle.Width = Unit.Pixel(42)

        Select Case intTblCellStyle
            Case 0
                tableStyle.BorderColor = Color.FromArgb(255, 102, 102, 102)
                tableStyle.BorderWidth = "1"
                tableStyle.BackColor = Color.FromArgb(255, 237, 237, 237)   'Light Gray
            Case 1
                tableStyle.BorderColor = Color.FromArgb(255, 102, 102, 102)
                tableStyle.BorderWidth = "1"
            Case 2
                tableStyle.BorderColor = Color.FromArgb(255, 255, 0, 0)     'Red
                tableStyle.BorderWidth = "2"
            Case 3
                tableStyle.BackColor = Color.FromArgb(255, 255, 211, 80)    'Light Orange
            Case 4
                tableStyle.BackColor = Color.FromArgb(255, 255, 255, 255)   'White
        End Select

        Return tableStyle

    End Function

    Private Function GetCalendarWeekday(ByVal intWeekday As Integer) As String
        Dim strWeekday As String = String.Empty

        Select Case intWeekday
            Case 1
                strWeekday = "SUN"
            Case 2
                strWeekday = "MON"
            Case 3
                strWeekday = "TUE"
            Case 4
                strWeekday = "WED"
            Case 5
                strWeekday = "THU"
            Case 6
                strWeekday = "FRI"
            Case 7
                strWeekday = "SAT"
        End Select

        Return strWeekday

    End Function

    Private Function GetCalendarDay(ByVal intPosition As Integer, ByVal intWeekdayOfFirstDayOfMonth As Integer, ByVal intLastDayOfMonth As Integer) As String
        Dim strCalendarDay As String = String.Empty
        Dim intCalendarDay As Integer

        intCalendarDay = intPosition - intWeekdayOfFirstDayOfMonth

        If intCalendarDay >= 0 And intCalendarDay < intLastDayOfMonth Then
            strCalendarDay = CStr((intPosition - intWeekdayOfFirstDayOfMonth) + 1)
        End If

        Return strCalendarDay

    End Function

    Private Sub RenderingLayout()

        Select Case mvConversionRateEnquiry.ActiveViewIndex
            Case ViewIndexConversionRateEnquiry.HiddenNotice

            Case ViewIndexConversionRateEnquiry.DisplayNotice
                ibtnBack.Enabled = False
                ibtnBack.Visible = False
            Case ViewIndexConversionRateEnquiry.DisplayHistory
                ddlMonth.DataSource = GetMonth()
                ddlMonth.DataTextField = "ddlMonthTextField"
                ddlMonth.DataValueField = "ddlMonthValueField"

                ddlMonth.DataBind()
                ddlMonth.SelectedIndex = Today.Month - 1

                ddlYear.DataSource = GetYear()
                ddlYear.DataTextField = "ddlYearTextField"
                ddlYear.DataValueField = "ddlYearValueField"

                ddlYear.DataBind()
                ddlYear.SelectedIndex = ddlYear.Items.Count - 1

                GenerateConversionRateCalendar(Today.Month, Today.Year)

                LoadCalendarSetting()

                ibtnBack.Enabled = True
                ibtnBack.Visible = True
            Case Else

        End Select

        Select Case mvCurrentConversionRateInfo.ActiveViewIndex
            Case ViewIndexCurrentConversionRateInfo.NoRecord
                ibtnConversionRateHistory.Enabled = False
                ibtnConversionRateHistory.ImageUrl = HttpContext.GetGlobalResourceObject("ImageURL", "ConversionRateHistoryDisableBtn")
            Case ViewIndexCurrentConversionRateInfo.CurrentRecord
                ibtnConversionRateHistory.Enabled = True
                ibtnConversionRateHistory.ImageUrl = HttpContext.GetGlobalResourceObject("ImageURL", "ConversionRateHistoryBtn")
            Case Else

        End Select

        Select Case mvNextConversionRateInfo.ActiveViewIndex
            Case ViewIndexNextConversionRateInfo.NoRecord

            Case ViewIndexNextConversionRateInfo.NextRecord

            Case Else

        End Select

        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
        ' ------------------------------------------------------------------------
        ' Hide unnecessary info for Call Centre
        Select Case Me.SubPlatform
            Case EnumHCVUSubPlatform.CC
                trCurrentConversionRateID.Visible = False
                trCurrentConversionRateCreateBy.Visible = False
                trCurrentConversionRateApprovedBy.Visible = False

                trNextConversionRateID.Visible = False
                trNextConversionRateCreateBy.Visible = False
                trNextConversionRateApprovedBy.Visible = False

                trConversionRateRecordID.Visible = False
                trConversionRateRecordCreateBy.Visible = False
                trConversionRateRecordApprovedBy.Visible = False
            Case Else
                trCurrentConversionRateID.Visible = True
                trCurrentConversionRateCreateBy.Visible = True
                trCurrentConversionRateApprovedBy.Visible = True

                trNextConversionRateID.Visible = True
                trNextConversionRateCreateBy.Visible = True
                trNextConversionRateApprovedBy.Visible = True

                trConversionRateRecordID.Visible = True
                trConversionRateRecordCreateBy.Visible = True
                trConversionRateRecordApprovedBy.Visible = True

        End Select
        ' CRE19-026 (HCVS hotline service) [End][Winnie]

    End Sub

    Private Function formatConversionRate(ByVal strConversionRate As String) As String
        Dim udtGeneralFunction As New GeneralFunction
        Dim strResult As String = String.Empty
        Dim strZero As String = String.Empty
        Dim arrStrRate As String() = strConversionRate.Trim.Split(".")

        Dim strConversionRateValueDecimalPlace As String = udtGeneralFunction.getSystemParameter("ConversionRateValueDecimalPlace")

        For intLength As Integer = 0 To CInt(strConversionRateValueDecimalPlace) - 1
            strZero += "0"
        Next

        Select Case arrStrRate.Length
            Case 1
                strResult = arrStrRate(0) + "." + strZero
            Case 2
                strResult = arrStrRate(0) + "." + Left(arrStrRate(1) + strZero, CInt(strConversionRateValueDecimalPlace))
            Case Else
                strResult = strConversionRate
        End Select

        Return strResult
    End Function

    Private Sub ibtnConversionRateHistory_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnConversionRateHistory.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010412, Me)
        udtAuditLog.WriteLog(LogID.LOG00001, AuditLogDescription.LOG00001)

        Dim udtFormatter As New Formatter

        mvConversionRateEnquiry.ActiveViewIndex = ViewIndexConversionRateEnquiry.DisplayHistory

        Session(SESS.InputedMonth) = Today.Month
        Session(SESS.InputedYear) = Today.Year
        Session(SESS.SelectedDate) = Today
        Session(SESS.SelectedTableCell) = Nothing
        Session(SESS.SelectedTodayTableCell) = Nothing

        lblConversionRateRecord.Text = HttpContext.GetGlobalResourceObject("Text", "ConversionRateRecord") & " (On " & udtFormatter.formatDisplayDate(Today) & ")"

        GetConversionRate(Today(), Today())

        RenderingLayout()
    End Sub

    Private Sub ibtnBack_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnBack.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010412, Me)
        udtAuditLog.WriteLog(LogID.LOG00002, AuditLogDescription.LOG00002)

        mvConversionRateEnquiry.ActiveViewIndex = ViewIndexConversionRateEnquiry.DisplayNotice

        udtAuditLog = Nothing
        udtAuditLog = New AuditLogEntry(FunctCode.FUNT010412, Me)

        ResetLayout()
        ClearSession()
        ClearConversionRateCalendar()
        GetCurrentConversionRate(udtAuditLog)
        GetNextConversionRate(udtAuditLog)
        RenderingLayout()
    End Sub

    Private Sub ibtnSearch_Click(sender As Object, e As ImageClickEventArgs) Handles ibtnSearch.Click
        Dim udtAuditLog = New AuditLogEntry(FunctCode.FUNT010412, Me)
        udtAuditLog.AddDescripton("YYYY-MM", ddlYear.SelectedValue & "-" & Right("0" & ddlMonth.SelectedValue.ToString, 2))
        udtAuditLog.WriteLog(LogID.LOG00003, AuditLogDescription.LOG00003)

        Session(SESS.InputedMonth) = ddlMonth.SelectedValue
        Session(SESS.InputedYear) = ddlYear.SelectedValue

        ClearConversionRateCalendar()
        GenerateConversionRateCalendar(ddlMonth.SelectedValue, ddlYear.SelectedValue)
    End Sub

    Protected Sub txtEffectiveDatePeriod_TextChanged()
        Dim strEffectiveDatePeriod() As String
        Dim dvYear, dvMonth As DataView
        Dim dtYear, dtMonth As DataTable
        Dim intYear, intMonth As Integer

        dvYear = GetYear()
        dvMonth = GetMonth()

        dtYear = dvYear.ToTable()
        dtMonth = dvMonth.ToTable()

        strEffectiveDatePeriod = Split(txtEffectiveDatePeriod.Text, ",")

        If strEffectiveDatePeriod.Length > 0 Then
            For cnt As Integer = 0 To dtYear.Rows.Count - 1
                If dtYear.Rows(cnt).Item("ddlYearTextField") = strEffectiveDatePeriod(1).Trim Then
                    intYear = dtYear.Rows(cnt).Item("ddlYearValueField")
                End If
            Next

            For cnt As Integer = 0 To dtMonth.Rows.Count - 1
                If dtMonth.Rows(cnt).Item("ddlMonthTextField") = strEffectiveDatePeriod(0).Trim Then
                    intMonth = dtMonth.Rows(cnt).Item("ddlMonthValueField")
                End If
            Next
        End If

        Session(SESS.InputedMonth) = intMonth
        Session(SESS.InputedYear) = intYear

        ClearConversionRateCalendar()
        GenerateConversionRateCalendar(intMonth, intYear)

    End Sub

End Class
