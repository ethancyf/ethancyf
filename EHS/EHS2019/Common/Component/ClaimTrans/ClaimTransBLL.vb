Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.Component.ClaimTrans

Namespace Component.ClaimTrans
    Public Class ClaimTransBLL

        Public Const SESS_ClaimTran As String = "ClaimTran"

        Public Function GetClaimTran() As ClaimTransModel
            Dim udtClaimTran As ClaimTransModel
            udtClaimTran = Nothing
            If Not HttpContext.Current.Session(SESS_ClaimTran) Is Nothing Then
                Try
                    udtClaimTran = CType(HttpContext.Current.Session(SESS_ClaimTran), ClaimTransModel)
                Catch ex As Exception
                    Throw New Exception("Invalid Session Claim Tran!")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
            Return udtClaimTran
        End Function

        Public Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_ClaimTran) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ClearSession()
            HttpContext.Current.Session(SESS_ClaimTran) = Nothing
        End Sub


        Public Sub SaveToSession(ByRef udtClaimTran As ClaimTransModel)
            HttpContext.Current.Session(SESS_ClaimTran) = udtClaimTran
        End Sub

        Public Function WriteAudit(ByVal strAction As String) As Boolean
            Dim bln_res As Boolean
            bln_res = False
            Return bln_res
        End Function

    End Class

End Namespace
