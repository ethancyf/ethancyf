Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Component.ProfessionVoucherQuota

Namespace Component.ProfessionVoucherQuota
    Public Class ProfessionVoucherQuotaBLL
        'Database Connection
        Private udtDB As Database = New Database()

        Public Property DB() As Database
            Get
                Return udtDB
            End Get
            Set(ByVal Value As Database)
                udtDB = Value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Function GetProfessionVoucherQuotaList() As ProfessionVoucherQuotaModelCollection
            Dim udtProfessionVoucherQuotaModelCollection As ProfessionVoucherQuotaModelCollection = New ProfessionVoucherQuotaModelCollection
            Dim udtProfessionVoucherQuotaModel As ProfessionVoucherQuotaModel

            If HttpRuntime.Cache("ProfessionVoucherQuota") Is Nothing Then
                Dim dt As New DataTable

                Try
                    DB.RunProc("proc_ProfessionVoucherQuota_get_cache", dt)

                    For Each row As DataRow In dt.Rows

                        Dim dtmExpiryDtm = New Nullable(Of DateTime)
                        If row.Item("Expiry_Dtm") Is DBNull.Value Then
                            dtmExpiryDtm = Nothing
                        Else
                            dtmExpiryDtm = CType(row.Item("Expiry_Dtm"), DateTime)
                        End If

                        udtProfessionVoucherQuotaModel = New ProfessionVoucherQuotaModel(CType(row.Item("Service_Category_Code"), String).Trim, _
                                                                CType(row.Item("Effective_Dtm"), DateTime), _
                                                                dtmExpiryDtm, _
                                                                CInt(row.Item("Quota")), _
                                                                CInt(row.Item("Cumulative_Year")))

                        udtProfessionVoucherQuotaModelCollection.add(udtProfessionVoucherQuotaModel)
                    Next

                    Common.ComObject.CacheHandler.InsertCache("ProfessionVoucherQuota", udtProfessionVoucherQuotaModelCollection)

                Catch ex As Exception
                    Throw
                End Try
            Else
                udtProfessionVoucherQuotaModelCollection = CType(HttpRuntime.Cache("ProfessionVoucherQuota"), ProfessionVoucherQuotaModelCollection)
            End If

            Return udtProfessionVoucherQuotaModelCollection

        End Function

        Public Function GetProfessionVoucherQuota(ByVal strServiceCategoryCode As String, ByVal dtmServiceDate As DateTime) As ProfessionVoucherQuotaModel
            Dim udtProfessionVoucherQuotaModelCollection As ProfessionVoucherQuotaModelCollection = New ProfessionVoucherQuotaModelCollection
            udtProfessionVoucherQuotaModelCollection = GetProfessionVoucherQuotaList()

            Return udtProfessionVoucherQuotaModelCollection.Filter(strServiceCategoryCode, dtmServiceDate)
        End Function

    End Class

End Namespace
