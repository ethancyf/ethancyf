Imports System

'CRE12-016 allow sorting of address in ．List of Enrolled Healthcare Service Providers・ in eHS [Start][Tommy Tse]

Imports System.Threading

'CRE12-016 allow sorting of address in ．List of Enrolled Healthcare Service Providers・ in eHS [End][Tommy Tse]


Namespace ComFunction
    Public Class GridviewFunction

        Private _strSortDirection As String
        Private _strSortExpression As String

        Public Sub New()
            _strSortDirection = "ASC"
            _strSortExpression = String.Empty
        End Sub

        Public Sub New(ByVal direction As String, ByVal expression As String)
            _strSortDirection = direction
            _strSortExpression = expression
        End Sub

        Public Property GridViewSortExpression() As String
            Get
                Return _strSortExpression
            End Get
            Set(ByVal value As String)
                _strSortExpression = value
            End Set
        End Property

        Public Property GridViewSortDirection() As String
            Get
                Return _strSortDirection
            End Get
            Set(ByVal value As String)
                _strSortDirection = value
            End Set
        End Property

        Private Function GetSortDirection() As String
            Select Case GridViewSortDirection
                Case "ASC"
                    GridViewSortDirection = "DESC"
                    _strSortDirection = "DESC"
                Case "DESC"
                    GridViewSortDirection = "ASC"
                    _strSortDirection = "ASC"
                Case Else
                    GridViewSortDirection = "ASC"
                    _strSortDirection = "ASC"
            End Select

            Return GridViewSortDirection

        End Function


        Public Function SortDataTable(ByVal datasource As Object, ByVal isPageIndexChange As Boolean)
            Dim dt As New DataTable
            Dim GridViewSortExpressionWithAppend As String
            If TypeOf datasource Is DataTable Then

                dt = CType(datasource, DataTable)

            ElseIf TypeOf datasource Is DataView Then

                dt = CType(datasource, DataView).Table

            End If

            'CRE12-016 allow sorting of address in ．List of Enrolled Healthcare Service Providers・ in eHS [Start][Tommy Tse]

            If Thread.CurrentThread.CurrentUICulture.Name = "zh-TW" Then
                dt.Locale = New System.Globalization.CultureInfo("zh-TW")
            Else
                dt.Locale = New System.Globalization.CultureInfo("en-US")
            End If

            'CRE12-016 allow sorting of address in ．List of Enrolled Healthcare Service Providers・ in eHS [End][Tommy Tse]

            If Not dt Is Nothing Then
                Dim dv As DataView = New DataView(dt)
                If GridViewSortExpression <> String.Empty Then
                    GridViewSortExpressionWithAppend = ""
                    If GridViewSortExpression = "district_board_shortname_SD" Then
                        GridViewSortExpressionWithAppend = ", sp_name ASC"
                    ElseIf GridViewSortExpression = "district_board_chi" Then
                        GridViewSortExpressionWithAppend = ", sp_chi_name ASC"
                    ElseIf GridViewSortExpression = "sp_name" Then
                        GridViewSortExpressionWithAppend = ", district_board_shortname_SD ASC"
                    ElseIf GridViewSortExpression = "sp_chi_name" Then
                        GridViewSortExpressionWithAppend = ", district_board_chi ASC"
                    ElseIf GridViewSortExpression = "practiceInfo" Then
                        GridViewSortExpressionWithAppend = ", district_board_shortname_SD ASC, sp_name ASC"
                    ElseIf GridViewSortExpression = "practiceInfo_chi" Then
                        GridViewSortExpressionWithAppend = ", district_board_chi ASC, sp_chi_name ASC"
                    ElseIf GridViewSortExpression.IndexOf("fee") > 0 Then
                        'If LCase(strLanguage) = "en-us" Then
                        GridViewSortExpressionWithAppend = ", district_board_shortname_SD ASC, sp_name ASC"
                        'Else
                        '    GridViewSortExpressionWithAppend = ", district_board_chi ASC, sp_chi_name ASC"
                        'End If
                    End If

                    If isPageIndexChange Then
                        dv.Sort = String.Format("{0} {1} {2}", GridViewSortExpression, GridViewSortDirection, GridViewSortExpressionWithAppend)
                    Else
                        dv.Sort = String.Format("{0} {1} {2}", GridViewSortExpression, GetSortDirection(), GridViewSortExpressionWithAppend)
                    End If
                    'If isPageIndexChange Then
                    '    dv.Sort = String.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection)
                    'Else
                    '    dv.Sort = String.Format("{0} {1}", GridViewSortExpression, GetSortDirection())
                    'End If
                End If
                Return dv
            Else
                Return New DataView()
            End If
        End Function

    End Class
End Namespace

