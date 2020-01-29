Imports HCVU.ParameteMaintenanceModel
Imports HCVU.ParameteMaintenanceModelCollection
Imports System.Data.SqlClient
Imports Common.Component.HCVUUser
Imports Common.DataAccess
Imports Common.Validation
Imports Common.ComObject
Imports Common.Component

Public Class ParameteMaintenanceBLL

    Private _udtHCVUUser As HCVUUserModel
    Private _udtValidator As Validator

    Public Sub New(ByVal udtHCVUUser As HCVUUserModel)
        _udtValidator = New Validator
        _udtHCVUUser = udtHCVUUser
    End Sub

    Public Sub SaveSystemParametersForExternalUse(ByVal udtParameterMaintenance As ParameteMaintenanceModel)
        Dim udtDB As Database = New Database

        Try
            udtDB.BeginTransaction()

            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@Parameter_ID", SqlDbType.Char, 50, udtParameterMaintenance.ParameterID), _
                udtDB.MakeInParam("@Category", SqlDbType.VarChar, 20, udtParameterMaintenance.Category), _
                udtDB.MakeInParam("@Parameter_Value", SqlDbType.NVarChar, 255, udtParameterMaintenance.ParameterValue1), _
                udtDB.MakeInParam("@Update_By", SqlDbType.VarChar, 20, Me._udtHCVUUser.UserID), _
                udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 20, udtParameterMaintenance.TSMP) _
            }

            udtDB.RunProc("proc_SystemParameters_upd_ParameterValue", prams)

            udtDB.CommitTransaction()

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw New ParameterSaveSQLException(eSQL.Message, eSQL.Number)

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw

        End Try

    End Sub


    Public Function GetSystemParametersForExternalUseDT() As DataTable
        Dim dt As DataTable = New DataTable

        Call (New Database).RunProc("proc_SystemParameters_get_ForExternalUse", dt)

        Return dt

    End Function

    Public Function FillParameteMaintenanceModel(ByVal dt As DataTable) As ParameteMaintenanceModelCollection

        Dim udtParameteMaintenanceModels As ParameteMaintenanceModelCollection = New ParameteMaintenanceModelCollection

        If dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                Dim udtParameteMaintenanceModel As ParameteMaintenanceModel = New ParameteMaintenanceModel
                udtParameteMaintenanceModel.Category = dr("Category")
                udtParameteMaintenanceModel.ParameterID = dr("Parameter_Name")
                udtParameteMaintenanceModel.ParameterDescription = dr("Description")
                udtParameteMaintenanceModel.ParameterValue1 = dr("Parm_Value1")
                udtParameteMaintenanceModel.TSMP = dr("TSMP")

                udtParameteMaintenanceModel.ExternalUse = dr("External_Use")

                If Not IsDBNull(dr("Upper_Limit")) Then
                    udtParameteMaintenanceModel.UpperLimit = dr("Upper_Limit")
                End If
                If Not IsDBNull(dr("Lower_Limit")) Then
                    udtParameteMaintenanceModel.LowerLimit = dr("Lower_Limit")
                End If
                If Not IsDBNull(dr("Apply_Limit")) Then
                    udtParameteMaintenanceModel.ApplyLimit = dr("Apply_Limit")
                End If
                udtParameteMaintenanceModels.Add(udtParameteMaintenanceModel)
            Next
            Return udtParameteMaintenanceModels
        Else
            Return Nothing
        End If
    End Function

    ' CRE15-022 (Change of parameter maintenance) [Start][Winnie]
    Public Function chkParameterValueIsvaild(ByVal udtParameterMaintenance As ParameteMaintenanceModel) As SystemMessage

        If Not udtParameterMaintenance.ApplyLimit Is Nothing Then

            If udtParameterMaintenance.ApplyLimit = Parm_ApplyLimit.Numeric Then
                Dim intParameterValue As Integer = 0

                ' Check Integer
                'Try
                '    intParameterValue = CType(udtParameteMaintenance.ParameterValue1, Integer)
                'Catch ex As Exception
                '    Return New SystemMessage(FunctCode.FUNT010901, SeverityCode.SEVE, MsgCode.MSG00003)
                'End Try

                If Not Me._udtValidator.IsInteger(udtParameterMaintenance.ParameterValue1) Then
                    Return New SystemMessage(FunctCode.FUNT010901, SeverityCode.SEVE, MsgCode.MSG00003)
                Else
                    intParameterValue = Integer.Parse(udtParameterMaintenance.ParameterValue1)
                End If

                ' Check Range
                Try
                    If Not Me._udtValidator.chkNumberWithRange(intParameterValue, udtParameterMaintenance.UpperLimit, udtParameterMaintenance.LowerLimit) Then
                        Return New SystemMessage(FunctCode.FUNT010901, SeverityCode.SEVE, MsgCode.MSG00002)
                    End If

                Catch ex As Exception
                    Throw New Exception("Missing value for the range limit")
                End Try

            ElseIf udtParameterMaintenance.ApplyLimit = Parm_ApplyLimit.BOUserID Then
                ' For UserID
                Dim aryBOUserID As New List(Of String)

                ' Check Format
                For Each UserID As String In udtParameterMaintenance.ParameterValue1.Split(",")
                    If UserID.Equals(String.Empty) Then
                        Return New SystemMessage(FunctCode.FUNT010901, SeverityCode.SEVE, MsgCode.MSG00005)
                    End If

                    If aryBOUserID.Contains(UserID) Then
                        Return New SystemMessage(FunctCode.FUNT010901, SeverityCode.SEVE, MsgCode.MSG00006)
                    End If

                    aryBOUserID.Add(UserID)
                Next
            End If
        End If

        Return Nothing
    End Function
    ' CRE15-022 (Change of parameter maintenance) [End][Winnie]

    Public Class ParameterSaveSQLException
        Inherits Exception

        Private _udtSystemMessage As SystemMessage

        Public Sub New(ByVal strMessageCode As String, ByVal strSQLNumber As Integer)
            MyBase.New(strMessageCode)
            If strSQLNumber = 50000 Then
                Me._udtSystemMessage = New SystemMessage("990001", "D", Me.Message)
            End If
        End Sub

        Public ReadOnly Property SystemMessage() As SystemMessage
            Get
                Return Me._udtSystemMessage
            End Get
        End Property
    End Class

End Class
