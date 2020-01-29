' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Namespace DynamicControls

    ' A composite [DropDownList] for Date Input
    ' No. of Compulsory Arguments for All Types are 2
    ' No. of Compulsory Arguments for Any Type with Year Component are 4
    ' 1st Argument is Language Setting ("EN" for English or "CH" for Traditional Chinese)
    ' 2nd Argument is Type of the Composition
    '   Common Type:
    '     - "D"             : Day Only (eg. "31")
    '
    '   English Type ("EN"):
    '     - "D_MMM"         : Day and Short Month Name with All Capital Letter in Month (eg. "31 DEC")
    '     - "D_MMM_YYYY"    : Day, Short Month Name and Year with All Capital Letter in Month (eg. "31 DEC 2012")
    '     - "D_MONTH"       : Day and Long Month Name with All Capital Letter in Month (eg. "31 DECEMBER")
    '     - "D_Month"       : Day and Long Month Name (eg. "31 December")
    '     - "D_MONTH_YYYY"  : Day, Long Month Name and Year with All Capital Letter in Month (eg. "31 DECEMBER 2012")
    '     - "D_Month_YYYY"  : Day, Long Month Name and Year (eg. "31 December 2012")
    '     - "MMM"           : Short Month Name Only with All Capital Letter (eg. "DEC")
    '     - "MMM_YYYY"      : Short Month Name and Year with All Capital Letter in Month (eg. "DEC 2012")
    '     - "MONTH"         : Long Month Name Only with All Capital Letter (eg. "DECEMBER")
    '     - "Month"         : Long Month Name Only (eg. "December")
    '     - "MONTH_YYYY"    : Long Month Name and Year with All Capital Letter in Month (eg. "DECEMBER 2012")
    '     - "Month_YYYY"    : Long Month Name and Year (eg. "December 2012")
    '     - "YYYY"          : Year Only (eg. "2012")
    '
    '   Traditional Chinese Type ("CH"):
    '     - "M"             : Month Only (eg. "十二月")
    '     - "M_D"           : Month and Day (eg. "十二月三十一日")
    '     - "Y"             : Year Only (eg. "二Ｏ一二年")
    '     - "Y_M"           : Year and Month (eg. "二Ｏ一二年十二月")
    '     - "Y_M_D"         : Year, Month and Day (eg. "二Ｏ一二年十二月三十一日")
    '
    ' 3rd Argument is Min. No. of Year Buffer to the Current Year for Selection
    ' 4th Argument is Max. No. of Year Buffer to the Current Year for Selection

    Public Class CustomDateByDDL
        Inherits DynamicControlBase

        Public Enum Language
            EN = 0
            CH = 1
        End Enum

        Public Enum Type
            'Common Type
            D = 1

            'EN Type
            D_MMM = 2
            D_MMM_YYYY = 3
            D_MONTH = 4
            D_MONTH_YYYY = 5
            MMM = 6
            MMM_YYYY = 7
            MONTH = 8
            MONTH_YYYY = 9
            YYYY = 10

            'CH Type
            M = 11
            M_D = 12
            Y = 13
            Y_M = 14
            Y_M_D = 15
        End Enum

        Private Const ARG_LANGUAGE As Integer = 0               ' 1st Argument is Language
        Private Const ARG_TYPE As Integer = 1                   ' 2nd Argument is Type of the Composition
        Private Const ARG_MIN_YEAR_BUFFER As Integer = 2        ' 3rd Argument is Min. No. of Year Buffer to the Current Year for Selection
        Private Const ARG_MAX_YEAR_BUFFER As Integer = 3        ' 4th Argument is Max. No. of Year Buffer to the Current Year for Selection
        Private Const MIN_NO_OF_ARGS As Integer = 2             ' No. of Compulsory Arguments for All Types are 2
        Private Const NO_SELECTION As String = "-1"             ' Value to indicate "No Selection of this Dynamic Control"

        Private Const DAY_MAX As Integer = 31                   ' Max. No. of Day within a Month
        Private Const MONTH_MAX As Integer = 12                 ' Max. No. of Month within a Year

        Private _ddlDay As DropDownList = New DropDownList()
        Private _ddlMonth As DropDownList = New DropDownList()
        Private _ddlYear As DropDownList = New DropDownList()

        Private _blnIsAllUpperCase As Boolean = False           ' To indicate the Value of Month Component is Upper Case or Lower Case

        Private _enumLanguage As CustomDateByDDL.Language       ' To indicate the Language Setting of this Dynamic Control
        Private _enumCustomDateType As CustomDateByDDL.Type     ' To indicate the Type of Composition of this Dynamic Control

#Region "Property"
        Public ReadOnly Property LanguageType() As CustomDateByDDL.Language
            Get
                Return _enumLanguage
            End Get
        End Property

        Public ReadOnly Property CustomDateType() As CustomDateByDDL.Type
            Get
                Return _enumCustomDateType
            End Get
        End Property

        Public ReadOnly Property IsAllUpperCase() As Boolean
            Get
                Return _blnIsAllUpperCase
            End Get
        End Property
#End Region

#Region "Abstract Method for the Abstract Class - [DynamicControlBase]"
        Public Overrides Sub Build(ByVal strArg As String)
            If Not strArg Is Nothing Then

                Dim strArgs() As String = strArg.Split(New String() {ARG_DELIMITER}, StringSplitOptions.RemoveEmptyEntries)

                If strArgs.Length >= MIN_NO_OF_ARGS Then

                    Select Case strArgs(ARG_LANGUAGE)

                        Case "EN"
                            _enumLanguage = CustomDateByDDL.Language.EN
                            Build_EN(strArgs)

                        Case "CH"
                            _enumLanguage = CustomDateByDDL.Language.CH
                            Build_CH(strArgs)

                        Case Else
                            Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [Build], Message = Language Type mismatch with the argument passed into this method")

                    End Select

                Else

                    Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [Build], Message = Inappropriate argument passed into this method. Both Language Type and CustomDate Type must be included.")

                End If

            Else

                Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [Build], Message = Missing argument passed into this method")

            End If
        End Sub

        Public Overrides Function GetText() As String
            Dim ddlDateComponent As DropDownList
            Dim strValue As StringBuilder = New StringBuilder()
            Dim strDelimiter As String

            Select Case _enumLanguage
                Case Language.EN
                    strDelimiter = " "

                Case Language.CH
                    strDelimiter = ""

                Case Else
                    Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [GetText], Message = Type mismatch with [CustomControls.DynamicControls.CustomDateByDDL.Language]")

            End Select

            For Each ddlDateComponent In Me.Controls
                strValue.Append(ddlDateComponent.SelectedItem.Text & strDelimiter)
            Next

            Return strValue.ToString().Trim()
        End Function

        Public Overrides Function HasInputValue() As Boolean
            Dim ddlDateComponent As DropDownList

            For Each ddlDateComponent In Me.Controls
                If ddlDateComponent.SelectedValue = NO_SELECTION Then
                    Return False
                End If
            Next

            Return True
        End Function

        Public Overrides Function IsValid() As Boolean
            Dim udtValidator As Common.Validation.Validator = New Common.Validation.Validator()

            If Not HasInputValue() Then
                Return False
            End If

            If _ddlYear.Items.Count > 0 AndAlso _ddlMonth.Items.Count > 0 AndAlso _ddlDay.Items.Count > 0 Then

                If udtValidator.IsValidDateValue(CInt(_ddlYear.SelectedValue), CInt(_ddlMonth.SelectedValue), CInt(_ddlDay.SelectedValue)) Then
                    Return True
                End If

            ElseIf _ddlMonth.Items.Count > 0 AndAlso _ddlDay.Items.Count > 0 Then

                If udtValidator.IsValidDateValue(CInt(_ddlMonth.SelectedValue), CInt(_ddlDay.SelectedValue)) Then
                    Return True
                End If

            Else

                Return True

            End If

            Return False
        End Function
#End Region

#Region "Private Method"
        ' Build all component of this DynamicControls in English Type
        Private Sub Build_EN(ByVal strArgs() As String)
            Try

                Select Case strArgs(ARG_TYPE)

                    Case "D"
                        _enumCustomDateType = Type.D
                        BuildDayComponent(Type.D)

                    Case "D_MMM"
                        _enumCustomDateType = Type.D_MMM
                        ' INT13-0006 Fix eHA Recification save check [Start][Koala]
                        ' -----------------------------------------------------------------------------------------
                        BuildDayComponent(Type.D)
                        BuildMonthComponent(Type.MMM, True)
                        ' INT13-0006 Fix eHA Recification save check [End][Koala]

                    Case "D_MMM_YYYY"
                        _enumCustomDateType = Type.D_MMM_YYYY
                        BuildDayComponent(Type.D)
                        BuildMonthComponent(Type.MMM, True)
                        BuildYearComponent(Type.YYYY, CInt(strArgs(ARG_MIN_YEAR_BUFFER)), CInt(strArgs(ARG_MAX_YEAR_BUFFER)))

                    Case "D_Month"
                        _enumCustomDateType = Type.D_MONTH
                        BuildDayComponent(Type.D)
                        BuildMonthComponent(Type.MONTH, False)

                    Case "D_Month_YYYY"
                        _enumCustomDateType = Type.D_MONTH_YYYY
                        BuildDayComponent(Type.D)
                        BuildMonthComponent(Type.MONTH, False)
                        BuildYearComponent(Type.YYYY, CInt(strArgs(ARG_MIN_YEAR_BUFFER)), CInt(strArgs(ARG_MAX_YEAR_BUFFER)))

                    Case "D_MONTH"
                        _enumCustomDateType = Type.D_MONTH
                        BuildDayComponent(Type.D)
                        BuildMonthComponent(Type.MONTH, True)

                    Case "D_MONTH_YYYY"
                        _enumCustomDateType = Type.D_MONTH_YYYY
                        BuildDayComponent(Type.D)
                        BuildMonthComponent(Type.MONTH, True)
                        BuildYearComponent(Type.YYYY, CInt(strArgs(ARG_MIN_YEAR_BUFFER)), CInt(strArgs(ARG_MAX_YEAR_BUFFER)))

                    Case "MMM"
                        _enumCustomDateType = Type.MMM
                        BuildMonthComponent(Type.MMM, True)

                    Case "MMM_YYYY"
                        _enumCustomDateType = Type.MMM_YYYY
                        BuildMonthComponent(Type.MMM, True)
                        BuildYearComponent(Type.YYYY, CInt(strArgs(ARG_MIN_YEAR_BUFFER)), CInt(strArgs(ARG_MAX_YEAR_BUFFER)))

                    Case "Month"
                        _enumCustomDateType = Type.MONTH
                        BuildMonthComponent(Type.MONTH, False)

                    Case "Month_YYYY"
                        _enumCustomDateType = Type.MONTH_YYYY
                        BuildMonthComponent(Type.MONTH, False)
                        BuildYearComponent(Type.YYYY, CInt(strArgs(ARG_MIN_YEAR_BUFFER)), CInt(strArgs(ARG_MAX_YEAR_BUFFER)))

                    Case "MONTH"
                        _enumCustomDateType = Type.MONTH
                        BuildMonthComponent(Type.MONTH, True)

                    Case "MONTH_YYYY"
                        _enumCustomDateType = Type.MONTH_YYYY
                        BuildMonthComponent(Type.MONTH, True)
                        BuildYearComponent(Type.YYYY, CInt(strArgs(ARG_MIN_YEAR_BUFFER)), CInt(strArgs(ARG_MAX_YEAR_BUFFER)))

                    Case "YYYY"
                        _enumCustomDateType = Type.YYYY
                        BuildYearComponent(Type.YYYY, CInt(strArgs(ARG_MIN_YEAR_BUFFER)), CInt(strArgs(ARG_MAX_YEAR_BUFFER)))

                    Case Else
                        Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [Build_EN], Message = CustomDate English Type mismatch with the argument passed into this method")

                End Select

            Catch ex As Exception

                Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [Build_EN], Message = Inappropriate argument passed into this method. Min. and Max. year buffer must be a number.")

            End Try
        End Sub

        ' Build all component of this DynamicControls in Traditional Chinese Type
        Private Sub Build_CH(ByVal strArgs() As String)
            Try

                Select Case strArgs(ARG_TYPE)

                    Case "D"
                        _enumCustomDateType = Type.D
                        BuildDayComponent(Type.D)

                    Case "M"
                        _enumCustomDateType = Type.M
                        BuildMonthComponent(Type.M, False)

                    Case "M_D"
                        _enumCustomDateType = Type.M_D
                        BuildMonthComponent(Type.M, False)
                        BuildDayComponent(Type.D)

                    Case "Y_M_D"
                        _enumCustomDateType = Type.Y_M_D
                        BuildYearComponent(Type.Y, CInt(strArgs(ARG_MIN_YEAR_BUFFER)), CInt(strArgs(ARG_MAX_YEAR_BUFFER)))
                        BuildMonthComponent(Type.M, False)
                        BuildDayComponent(Type.D)

                    Case "Y_M"
                        _enumCustomDateType = Type.Y_M
                        BuildYearComponent(Type.Y, CInt(strArgs(ARG_MIN_YEAR_BUFFER)), CInt(strArgs(ARG_MAX_YEAR_BUFFER)))
                        BuildMonthComponent(Type.M, False)

                    Case "Y"
                        _enumCustomDateType = Type.Y
                        BuildYearComponent(Type.Y, CInt(strArgs(ARG_MIN_YEAR_BUFFER)), CInt(strArgs(ARG_MAX_YEAR_BUFFER)))

                    Case Else
                        Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [Build_CH], Message = CustomDate Chinese Type mismatch with the argument passed into this method")

                End Select

            Catch ex As Exception

                Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [Build_CH], Message = Inappropriate argument passed into this method. Min. and Max. year buffer must be a number.")

            End Try
        End Sub

        ' Build Day Component of this DynamicControls
        Private Sub BuildDayComponent(ByVal enumCustomDateType As CustomDateByDDL.Type)
            Dim itmDay As ListItem
            Dim i As Integer
            Dim itmDayText As String = ""
            Dim itmDayValue As String = ""

            _ddlDay.ID = Me.ID & "_ddlDay"

            Select Case enumCustomDateType

                Case Type.D

                    Select Case _enumLanguage

                        Case Language.EN
                            _ddlDay.Items.Add(New ListItem("----", NO_SELECTION))
                            For i = 1 To DAY_MAX
                                itmDayText = i.ToString()
                                itmDayValue = i.ToString()
                                itmDay = New ListItem(itmDayText, itmDayValue)
                                _ddlDay.Items.Add(itmDay)
                            Next

                        Case Language.CH
                            BuildDayComponentList_CH()

                        Case Else
                            Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [BuildDayComponent], Message = Type mismatch with [CustomControls.DynamicControls.CustomDateByDDL.Language]")

                    End Select

                Case Else
                    Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [BuildDayComponent], Message = Type mismatch with [CustomControls.DynamicControls.CustomDateByDDL.Type]")

            End Select

            Controls.Add(_ddlDay)
        End Sub

        ' Build Day Component of this DynamicControls in Traditional Chinese Type
        Private Sub BuildDayComponentList_CH()
            _ddlDay.Items.Add(New ListItem("----------------", NO_SELECTION))
            _ddlDay.Items.Add(New ListItem("一日", "1"))
            _ddlDay.Items.Add(New ListItem("二日", "2"))
            _ddlDay.Items.Add(New ListItem("三日", "3"))
            _ddlDay.Items.Add(New ListItem("四日", "4"))
            _ddlDay.Items.Add(New ListItem("五日", "5"))
            _ddlDay.Items.Add(New ListItem("六日", "6"))
            _ddlDay.Items.Add(New ListItem("七日", "7"))
            _ddlDay.Items.Add(New ListItem("八日", "8"))
            _ddlDay.Items.Add(New ListItem("九日", "9"))
            _ddlDay.Items.Add(New ListItem("十日", "10"))
            _ddlDay.Items.Add(New ListItem("十一日", "11"))
            _ddlDay.Items.Add(New ListItem("十二日", "12"))
            _ddlDay.Items.Add(New ListItem("十三日", "13"))
            _ddlDay.Items.Add(New ListItem("十四日", "14"))
            _ddlDay.Items.Add(New ListItem("十五日", "15"))
            _ddlDay.Items.Add(New ListItem("十六日", "16"))
            _ddlDay.Items.Add(New ListItem("十七日", "17"))
            _ddlDay.Items.Add(New ListItem("十八日", "18"))
            _ddlDay.Items.Add(New ListItem("十九日", "19"))
            _ddlDay.Items.Add(New ListItem("二十日", "20"))
            _ddlDay.Items.Add(New ListItem("二十一日", "21"))
            _ddlDay.Items.Add(New ListItem("二十二日", "22"))
            _ddlDay.Items.Add(New ListItem("二十三日", "23"))
            _ddlDay.Items.Add(New ListItem("二十四日", "24"))
            _ddlDay.Items.Add(New ListItem("二十五日", "25"))
            _ddlDay.Items.Add(New ListItem("二十六日", "26"))
            _ddlDay.Items.Add(New ListItem("二十七日", "27"))
            _ddlDay.Items.Add(New ListItem("二十八日", "28"))
            _ddlDay.Items.Add(New ListItem("二十九日", "29"))
            _ddlDay.Items.Add(New ListItem("三十日", "30"))
            _ddlDay.Items.Add(New ListItem("三十一日", "31"))
        End Sub

        ' Build Month Component of this DynamicControls
        Private Sub BuildMonthComponent(ByVal enumCustomDateType As CustomDateByDDL.Type, ByVal blnIsAllUpperCase As Boolean)
            Dim itmMonth As ListItem
            Dim i As Integer
            Dim itmMonthText As String = ""
            Dim itmMonthValue As String = ""
            Dim strMonth(,) As String = {{"January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"}, _
                                         {"一月", "二月", "三月", "四月", "五月", "六月", "七月", "八月", "九月", "十月", "十一月", "十二月"}}

            _blnIsAllUpperCase = blnIsAllUpperCase

            _ddlMonth.ID = Me.ID & "_ddlMonth"

            Select Case _enumLanguage

                Case Language.EN
                    _ddlMonth.Items.Add(New ListItem("--------------------", NO_SELECTION))

                Case Language.CH
                    _ddlMonth.Items.Add(New ListItem("------------", NO_SELECTION))

                Case Else
                    Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [BuildMonthComponent], Message = Type mismatch with [CustomControls.DynamicControls.CustomDateByDDL.Language]")

            End Select

            For i = 1 To MONTH_MAX

                Select Case enumCustomDateType

                    Case Type.MMM
                        itmMonthText = strMonth(_enumLanguage, i - 1).Substring(0, 3)

                    Case Type.MONTH
                        itmMonthText = strMonth(_enumLanguage, i - 1)

                    Case Type.M
                        itmMonthText = strMonth(_enumLanguage, i - 1)

                    Case Else
                        Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [BuildMonthComponent], Message = Type mismatch with [CustomControls.DynamicControls.CustomDateByDDL.Type]")

                End Select

                If _blnIsAllUpperCase Then

                    itmMonthText = itmMonthText.ToUpper()

                End If

                itmMonthValue = i.ToString()

                itmMonth = New ListItem(itmMonthText, itmMonthValue)
                _ddlMonth.Items.Add(itmMonth)

            Next

            Controls.Add(_ddlMonth)
        End Sub

        ' Build Year Component of this DynamicControls
        Private Sub BuildYearComponent(ByVal enumCustomDateType As CustomDateByDDL.Type, ByVal intMinYearBuffer As Integer, ByVal intMaxYearBuffer As Integer)
            Dim itmYear As ListItem
            Dim i As Integer
            Dim itmYearText As String = ""
            Dim itmYearValue As String = ""
            Dim intThisYear As Integer = Year(Now())

            _ddlYear.ID = Me.ID & "_ddlYear"

            Select Case _enumLanguage

                Case Language.EN
                    _ddlYear.Items.Add(New ListItem("--------", NO_SELECTION))

                Case Language.CH
                    _ddlYear.Items.Add(New ListItem("------------------", NO_SELECTION))

                Case Else
                    Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [BuildYearComponent], Message = Type mismatch with [CustomControls.DynamicControls.CustomDateByDDL.Language]")

            End Select

            For i = intThisYear - intMinYearBuffer To intThisYear + intMaxYearBuffer

                Select Case enumCustomDateType

                    Case Type.YYYY
                        itmYearText = i.ToString()

                    Case Type.Y
                        itmYearText = ConvertYearToCH(i.ToString())

                    Case Else
                        Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [BuildYearComponent], Message = Type mismatch with [CustomControls.DynamicControls.CustomDateByDDL.Type]")

                End Select

                itmYearValue = i.ToString()

                itmYear = New ListItem(itmYearText, itmYearValue)
                _ddlYear.Items.Add(itmYear)

            Next

            Controls.Add(_ddlYear)
        End Sub

        ' Convert the Numeric Year String to Traditional Chinese
        Private Function ConvertYearToCH(ByVal strYear As String) As String
            Dim strNumCH() As String = {"O", "一", "二", "三", "四", "五", "六", "七", "八", "九"}
            Dim strYearCH As StringBuilder = New StringBuilder()
            Dim i As Integer

            If strYear.Length = 4 Then
                For i = 0 To 3
                    strYearCH.Append(strNumCH(CInt(strYear.Substring(i, 1))))
                Next
                strYearCH.Append("年")
            Else
                Throw New Exception("Error: Class = [CustomControls.DynamicControls.CustomDateByDDL], Method = [ConvertYearToCH], Message = Inappropriate parameter passed into this method. The year value must be 4 characters.")
            End If

            Return strYearCH.ToString()
        End Function
#End Region

    End Class

End Namespace
