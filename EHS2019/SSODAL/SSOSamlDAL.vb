Imports System.Data.SqlClient

Imports Common.DataAccess
Imports SSODataType

Public Class SSOSamlDAL

#Region "Constructor"

    Private Shared _SSOSamlDAL As SSOSamlDAL

    Public Shared Function getInstance() As SSOSamlDAL
        If _SSOSamlDAL Is Nothing Then
            _SSOSamlDAL = New SSOSamlDAL()
        End If
        Return _SSOSamlDAL
    End Function

    Private Sub New()
    End Sub

#End Region

    ''' <summary>
    ''' Get SSO assertion from persistent storage by artifact
    ''' </summary>
    ''' <param name="strSrchArtifact">the key to retrive the SSO assertion</param>
    ''' <returns>a SSOAssertion object associated with the artifact, or null if no SSO Assertion is found</returns>
    ''' <remarks></remarks>
    Public Function getSSOActiveAssertion(ByVal strSrchArtifact As String) As SSOActiveAssertion

        Dim udtDB As New Database()
        Dim objSSOActiveAssertion As SSOActiveAssertion = Nothing
        Dim dt As New DataTable()

        Try

            Dim parms() As SqlParameter = { _
                            udtdb.MakeInParam("@v_in_artifact", SqlDbType.VarChar, 255, strSrchArtifact)}
            udtDB.RunProc("proc_get_sso_active_assertion", parms, dt)

            If dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)

                objSSOActiveAssertion = New SSOActiveAssertion( _
                        CType(dr.Item("txn_id"), String).Trim, _
                        CType(dr.Item("artifact"), String).Trim, _
                        CType(dr.Item("assertion"), String).Trim, _
                        CType(dr.Item("read_count"), Integer), _
                        CType(dr.Item("creation_datetime"), DateTime))

            End If
        Catch ex As SqlException
            Throw ex
        Catch ex As Exception
            Throw ex
        End Try

        Return objSSOActiveAssertion

    End Function

    ''' <summary>
    ''' Save assertion to a persistent storage for later verification in SSO
    ''' </summary>
    ''' <param name="strSSOTxnId">the transaction id of the current SSO operation</param>
    ''' <param name="objSSOActiveAssertion">an object containing the assertion data</param>
    ''' <returns>Success: return an integer >0,  Fail: return an integer = 0 or less than 0</returns>
    ''' <remarks></remarks>
    Public Function saveSSOActiveAssertion(ByVal strSSOTxnId As String, ByVal objSSOActiveAssertion As SSOActiveAssertion) As Integer

        Dim udtDB As New Database()
        Dim intStatus As Integer = -1
        Try
            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@v_in_txn_id", SqlDbType.VarChar, 255, strSSOTxnId), _
                udtDB.MakeInParam("@v_in_artifact", SqlDbType.VarChar, 255, objSSOActiveAssertion.Artifact), _
                udtDB.MakeInParam("@v_in_assertion", SqlDbType.Text, 200000, objSSOActiveAssertion.Assertion), _
                udtDB.MakeInParam("@v_in_read_count", SqlDbType.Int, 8, objSSOActiveAssertion.ReadCount), _
                udtDB.MakeInParam("@v_in_creation_datetime", SqlDbType.DateTime, 8, objSSOActiveAssertion.CreationDateTime)}

            udtDB.RunProc("proc_ins_sso_active_assertion", parms)
            intStatus = 1
        Catch ex As SqlException
            Throw ex
        Catch ex As Exception
            Throw ex
        End Try

        Return intStatus
    End Function

    ''' <summary>
    ''' Check if an Assertion Resolve Requet is valid
    ''' the asertion must exist in the persistent storage and the read count should be 0
    ''' </summary>
    ''' <param name="strSSOArtifact">the key to retrive the SSO assertion</param>
    ''' <returns>true for valid Assertion Resolve request, false otherwise</returns>
    ''' <remarks></remarks>
    Public Function chkSSOActiveAssertionIsValid(ByVal strSSOArtifact As String) As Boolean

        Dim udtDB As New Database()

        Dim intChkRst As Integer = 0
        Dim blnChkRst As Boolean = False

        Dim dt As New DataTable()
        Try
            Dim parms() As SqlParameter = {udtDB.MakeInParam("@v_in_artifact", SqlDbType.VarChar, 255, strSSOArtifact)}
            udtDB.RunProc("proc_chk_valid_sso_active_assertion_resolve_req", parms, dt)

            If dt.Rows.Count > 0 Then
                intChkRst = CType(dt.Rows(0).Item("chk_rst"), Integer)
            End If

            If (intChkRst = 1) Then
                blnChkRst = True
            Else
                blnChkRst = False
            End If

        Catch ex As SqlException
            Throw ex
        Catch ex As Exception
            Throw ex
        End Try

        Return blnChkRst

    End Function

    ''' <summary>
    ''' Update the read count of an assertion stored in persistent storage by 1
    ''' This is done after a assertion has been read
    ''' </summary>
    ''' <param name="strSrchArtifact">he key to retrive the SSO assertion</param>
    ''' <returns>
    ''' if success, return an integer > 0
    ''' if failed, return an integer = 0 or less than 0
    ''' </returns>
    ''' <remarks></remarks>
    Public Function updateSSOActiveAssertionReadCountByOne(ByVal strSrchArtifact As String) As Integer

        Dim intStatus As Integer = -1

        Dim udtDB As New Database()

        Dim parms() As SqlParameter = { _
            udtDB.MakeInParam("@v_in_artifact", SqlDbType.VarChar, 255, strSrchArtifact)}

        Try
            udtDB.RunProc("proc_upd_sso_active_assertion_read_count", parms)
            intStatus = 1
        Catch ex As SqlException
            Throw ex
        Catch ex As Exception
            Throw ex
        End Try

        Return intStatus

    End Function

End Class
