Imports System

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
            If TypeOf datasource Is DataTable Then

                dt = CType(datasource, DataTable)

            ElseIf TypeOf datasource Is DataView Then

                dt = CType(datasource, DataView).Table

            End If


            If Not dt Is Nothing Then
                Dim dv As DataView = New DataView(dt)
                If GridViewSortExpression <> String.Empty Then
                    If isPageIndexChange Then
                        dv.Sort = String.Format("{0} {1}", GridViewSortExpression, GridViewSortDirection)
                    Else
                        dv.Sort = String.Format("{0} {1}", GridViewSortExpression, GetSortDirection())
                    End If
                End If
                Return dv
            Else
                Return New DataView()
            End If
        End Function

    End Class
End Namespace

