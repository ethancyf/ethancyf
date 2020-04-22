' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

' -----------------------------------------------------------------------------------------

Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Component.VersionControl

Namespace Component.VersionControl
    Public Class VersionControlBLL

        Public Sub New()

        End Sub

        Public Shared Function GetVersionControlList(ByVal db As Database) As VersionControlModelCollection
            Dim udtVersionControlModelCollection As VersionControlModelCollection = New VersionControlModelCollection
            Dim udtVersionControlModel As VersionControlModel

            Dim dtmEffectiveDtm As Nullable(Of DateTime)

            dtmEffectiveDtm = New Nullable(Of DateTime)

            If HttpContext.Current.Cache("VersionControl") Is Nothing Then
                Dim dt As New DataTable

                Try
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'db.RunProc("proc_Interface_VersionControl_get_cache", dt)
                    db.RunProc("proc_VersionControl_get_cache", dt)
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

                    For Each row As DataRow In dt.Rows

                        If row.Item("Effective_Dtm") Is DBNull.Value Then
                            dtmEffectiveDtm = Nothing
                        Else
                            dtmEffectiveDtm = CType(row.Item("Effective_Dtm"), DateTime)
                        End If

                        udtVersionControlModel = New VersionControlModel(CType(row.Item("Logical_Name"), String).Trim, _
                                                                CType(row.Item("Physical_Name"), String).Trim, _
                                                                CStr(IIf(row.Item("Description") Is DBNull.Value, String.Empty, row.Item("Description"))), _
                                                                dtmEffectiveDtm)

                        udtVersionControlModelCollection.add(udtVersionControlModel)
                        'End If
                    Next
                    Common.ComObject.CacheHandler.InsertCache("VersionControl", udtVersionControlModelCollection)

                Catch ex As Exception
                    Throw ex
                End Try
            Else
                udtVersionControlModelCollection = CType(HttpContext.Current.Cache("VersionControl"), VersionControlModelCollection)
            End If

            Return udtVersionControlModelCollection

        End Function
        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
        'Fix for getting too many DB datetime
        Public Shared Function GetVersionControlListByLogicalName(ByVal strLogicalName As String, ByVal db As Database, ByVal dtmDateTime As DateTime) As VersionControlModel
            'Public Shared Function GetVersionControlListByLogicalName(ByVal strLogicalName As String, ByVal db As Database) As VersionControlModel
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]
            Dim udtVersionControlModelCollection As VersionControlModelCollection = New VersionControlModelCollection
            udtVersionControlModelCollection = GetVersionControlList(db)

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
            'Fix for getting too many DB datetime
            udtVersionControlModelCollection = udtVersionControlModelCollection.FilterByPeriod(strLogicalName, dtmDateTime)
            'udtVersionControlModelCollection = udtVersionControlModelCollection.FilterByPeriod(strLogicalName)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

            Dim udtVersionControlModel As VersionControlModel

            If strLogicalName.Trim.Equals(String.Empty) Then
                udtVersionControlModel = New VersionControlModel(String.Empty, String.Empty, String.Empty, Nothing)
            Else
                udtVersionControlModel = udtVersionControlModelCollection.Item(strLogicalName.Trim)
            End If

            Return udtVersionControlModel

        End Function

    End Class


End Namespace

' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

