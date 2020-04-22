Imports System.Data.SqlClient

Public Class DBTester
    Private m_dbConn As SqlConnection

    Public Sub New()

    End Sub

    Public Shared Function CheckDBConnection(ByVal _strConn As String) As Boolean
        Try
            Using conn = New SqlConnection(_strConn)
                conn.Open()
                conn.Close()
            End Using
            Return True
        Catch ex As Exception

        End Try
        Return False
    End Function
End Class
