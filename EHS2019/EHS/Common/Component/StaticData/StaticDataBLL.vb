Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Component.StaticData

Namespace Component.StaticData
    Public Class StaticDataBLL
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

#Region "Cache"
        Public Function GetStaticDataFullList() As StaticDataModelCollection
            Dim udtStaticDataModelCollection As StaticDataModelCollection = New StaticDataModelCollection
            Dim udtStaticDataModel As StaticDataModel

            If HttpContext.Current.Cache("StaticData") Is Nothing Then
                Dim dt As New DataTable

                Try
                    DB.RunProc("proc_StaticData_get_cache", dt)

                    For Each row As DataRow In dt.Rows
                        'CRE20-009 call the stored procedure to get the related data from staticData table [Start][Nichole]
                        'udtStaticDataModel = New StaticDataModel(CType(row.Item("Column_Name"), String).Trim, _
                        '                                        CType(row.Item("Item_No"), String).Trim, _
                        '                                        CType(row.Item("Data_Value"), String).Trim, _
                        '                                        CStr(IIf(row.Item("Data_Value_Chi") Is DBNull.Value, String.Empty, row.Item("Data_Value_Chi"))), _
                        '                                        CStr(IIf(row.Item("Data_Value_CN") Is DBNull.Value, String.Empty, row.Item("Data_Value_CN"))))

                        udtStaticDataModel = New StaticDataModel(CType(row.Item("Column_Name"), String).Trim, _
                                                                CType(row.Item("Item_No"), String).Trim, _
                                                                CType(row.Item("Data_Value"), String).Trim, _
                                                                CStr(IIf(row.Item("Data_Value_Chi") Is DBNull.Value, String.Empty, row.Item("Data_Value_Chi"))), _
                                                                CStr(IIf(row.Item("Data_Value_CN") Is DBNull.Value, String.Empty, row.Item("Data_Value_CN"))), _
                                                                CStr(IIf(row.Item("Display_Order") Is DBNull.Value, String.Empty, row.Item("Display_Order"))), _
                                                                CStr(IIf(row.Item("Service_Date_Start") Is DBNull.Value, String.Empty, row.Item("Service_Date_Start"))), _
                                                                CStr(IIf(row.Item("Record_Status") Is DBNull.Value, String.Empty, row.Item("Record_Status"))))
                        'CRE20-009 call the stored procedure to get the related data from staticData table [End][Nichole]

                        udtStaticDataModelCollection.add(udtStaticDataModel)
                    Next
                    Common.ComObject.CacheHandler.InsertCache("StaticData", udtStaticDataModelCollection)
                    'HttpContext.Current.Cache("StaticData") = udtStaticDataModelCollection

                Catch ex As Exception
                    Throw ex
                End Try
            Else
                udtStaticDataModelCollection = CType(HttpContext.Current.Cache("StaticData"), StaticDataModelCollection)
            End If

            Return udtStaticDataModelCollection

        End Function

        Public Function GetStaticDataActiveList() As StaticDataModelCollection
            Dim udtStaticDataModelCollection As StaticDataModelCollection = New StaticDataModelCollection

            If HttpContext.Current.Cache("StaticDataActive") Is Nothing Then

                For Each udtStaticDataModel As StaticDataModel In GetStaticDataFullList()
                    If udtStaticDataModel.RecordStatus = StaticDataStatus.Active Then
                        udtStaticDataModelCollection.add(udtStaticDataModel)
                    End If
                Next
                Common.ComObject.CacheHandler.InsertCache("StaticDataActive", udtStaticDataModelCollection)
            Else
                udtStaticDataModelCollection = CType(HttpContext.Current.Cache("StaticDataActive"), StaticDataModelCollection)
            End If

            Return udtStaticDataModelCollection

        End Function

#End Region

#Region "Get by record status"
        Private Function GetStaticDataListByRecordStatus(ByVal blnIsActive As Boolean) As StaticDataModelCollection
            Dim udtStaticDataModelCollection As StaticDataModelCollection = New StaticDataModelCollection

            If blnIsActive = True Then
                udtStaticDataModelCollection = GetStaticDataActiveList()
            Else
                udtStaticDataModelCollection = GetStaticDataFullList()
            End If

            Return udtStaticDataModelCollection

        End Function

#End Region

#Region "Get by column name - return datatable"
        Public Function GetStaticDataList(ByVal strColumnName As String) As DataTable
            Dim udtStaticDataModelCollection As StaticDataModelCollection = Me.GetStaticDataListByColumnName(strColumnName)

            Dim dataTable As DataTable = New DataTable()
            Dim dataRow As DataRow

            dataTable.Columns.Add(New DataColumn(StaticDataModel.Column_Name, GetType(String)))
            dataTable.Columns.Add(New DataColumn(StaticDataModel.Item_No, GetType(String)))
            dataTable.Columns.Add(New DataColumn(StaticDataModel.Data_Value, GetType(String)))
            dataTable.Columns.Add(New DataColumn(StaticDataModel.Data_Value_Chi, GetType(String)))
            dataTable.Columns.Add(New DataColumn(StaticDataModel.Data_Value_CN, GetType(String)))

            For Each udtStaticDataModel As StaticDataModel In udtStaticDataModelCollection

                dataRow = dataTable.NewRow()
                dataRow(StaticDataModel.Column_Name) = udtStaticDataModel.ColumnName
                dataRow(StaticDataModel.Item_No) = udtStaticDataModel.ItemNo
                dataRow(StaticDataModel.Data_Value) = udtStaticDataModel.DataValue
                dataRow(StaticDataModel.Data_Value_Chi) = udtStaticDataModel.DataValueChi
                dataRow(StaticDataModel.Data_Value_CN) = udtStaticDataModel.DataValueCN
                dataTable.Rows.Add(dataRow)
            Next

            Return dataTable

        End Function

        Public Function GetStaticDataListFilter(ByVal strColumnName As String, ByVal strServiceDtm As String) As DataTable
            Dim udtStaticDataModelCollection As StaticDataModelCollection = Me.GetStaticDataListByColumnName(strColumnName)

            Dim dataTable As DataTable = New DataTable()
            Dim dataRow As DataRow
            Dim strConvertedDtm As Date = Date.ParseExact(strServiceDtm.Replace("-", "/"), "dd/MM/yyyy", Nothing)

            dataTable.Columns.Add(New DataColumn(StaticDataModel.Column_Name, GetType(String)))
            dataTable.Columns.Add(New DataColumn(StaticDataModel.Item_No, GetType(String)))
            dataTable.Columns.Add(New DataColumn(StaticDataModel.Data_Value, GetType(String)))
            dataTable.Columns.Add(New DataColumn(StaticDataModel.Data_Value_Chi, GetType(String)))
            dataTable.Columns.Add(New DataColumn(StaticDataModel.Data_Value_CN, GetType(String)))

            For Each udtStaticDataModel As StaticDataModel In udtStaticDataModelCollection

                dataRow = dataTable.NewRow()
                dataRow(StaticDataModel.Column_Name) = udtStaticDataModel.ColumnName
                dataRow(StaticDataModel.Item_No) = udtStaticDataModel.ItemNo
                dataRow(StaticDataModel.Data_Value) = udtStaticDataModel.DataValue
                dataRow(StaticDataModel.Data_Value_Chi) = udtStaticDataModel.DataValueChi
                dataRow(StaticDataModel.Data_Value_CN) = udtStaticDataModel.DataValueCN

                If udtStaticDataModel.ServiceDate = "" Then
                    dataTable.Rows.Add(dataRow)
                Else
                    If DateTime.Compare(strConvertedDtm, udtStaticDataModel.ServiceDate) > 0 Then
                        dataTable.Rows.Add(dataRow)
                    End If
                End If
            Next

            Return dataTable

        End Function
#End Region

#Region "Get by column name - return list"

        Public Function GetStaticDataListByColumnName(ByVal strColumnName As String) As StaticDataModelCollection
            Return GetStaticDataListByColumnName(strColumnName, True)
        End Function

        Public Function GetStaticDataListByColumnName(ByVal strColumnName As String, Optional ByVal blnIsActive As Boolean = True) As StaticDataModelCollection
            Dim udtStaticDataModelCollection As StaticDataModelCollection = New StaticDataModelCollection

            udtStaticDataModelCollection = GetStaticDataListByRecordStatus(blnIsActive)

            Return udtStaticDataModelCollection.Filter(strColumnName)

        End Function

        Public Function GetStaticDataListByDocProof(ByVal strColumnName As String, ByVal strServiceDate As String, Optional ByVal blnIsActive As Boolean = True) As StaticDataModelCollection
            Dim udtStaticDataModelCollection As StaticDataModelCollection = New StaticDataModelCollection

            udtStaticDataModelCollection = GetStaticDataListByRecordStatus(blnIsActive)

            Return udtStaticDataModelCollection.CSSAFilter(strColumnName, strServiceDate)

        End Function

#End Region

#Region "Get by column name & item no. - return model"
        ''' <summary>
        ''' Get the static Data by column name and item no.
        ''' </summary>
        ''' <param name="strColumnName"></param>
        ''' <param name="strItemNo"></param>
        ''' <returns>A static data model</returns>
        ''' <remarks></remarks>
        Public Function GetStaticDataByColumnNameItemNo(ByVal strColumnName As String, ByVal strItemNo As String) As StaticDataModel
            Return GetStaticDataByColumnNameItemNo(strColumnName, strItemNo, True, Nothing)
        End Function

        ''' <summary>
        ''' Get the static Data by column name, item no. and record status
        ''' </summary>
        ''' <param name="strColumnName"></param>
        ''' <param name="strItemNo"></param>
        ''' <param name="blnIsActive"></param>
        ''' <returns>A static data model</returns>
        ''' <remarks></remarks>
        Public Function GetStaticDataByColumnNameItemNo(ByVal strColumnName As String, ByVal strItemNo As String, ByVal blnIsActive As Boolean) As StaticDataModel
            Return GetStaticDataByColumnNameItemNo(strColumnName, strItemNo, blnIsActive, Nothing)
        End Function

        ''' <summary>
        ''' Get the static Data by column name, item no. and service date
        ''' </summary>
        ''' <param name="strColumnName"></param>
        ''' <param name="strItemNo"></param>
        ''' <param name="dtmDate"></param>
        ''' <returns>A static data model</returns>
        ''' <remarks></remarks>
        Public Function GetStaticDataByColumnNameItemNo(ByVal strColumnName As String, ByVal strItemNo As String, ByVal dtmDate As DateTime) As StaticDataModel
            Return GetStaticDataByColumnNameItemNo(strColumnName, strItemNo, True, dtmDate)
        End Function

        ''' <summary>
        ''' Get the static Data by column name and item no.
        ''' </summary>
        ''' <param name="strColumnName"></param>
        ''' <param name="strItemNo"></param>
        ''' <returns>A static data model</returns>
        ''' <remarks></remarks>
        Public Function GetStaticDataByColumnNameItemNo(ByVal strColumnName As String, ByVal strItemNo As String, ByVal blnIsActive As Boolean, ByVal dtmDate As Nullable(Of DateTime)) As StaticDataModel
            Dim udtStaticDataCollectionModel As StaticDataModelCollection = New StaticDataModelCollection

            udtStaticDataCollectionModel = GetStaticDataListByColumnName(strColumnName.Trim, blnIsActive)

            Dim udtStaticDataModel As StaticDataModel = Nothing

            If strItemNo.Trim.Equals(String.Empty) Then

                'CRE20-009 StaticDataModel constructor add service date [Start][Nichole]
                'udtStaticDataModel = New StaticDataModel(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
                udtStaticDataModel = New StaticDataModel(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty)
                'CRE20-009 StaticDataModel constructor add service date [End][Nichole]
            Else
                udtStaticDataModel = udtStaticDataCollectionModel.Item(strColumnName.Trim, strItemNo.Trim)
            End If

            Return udtStaticDataModel

        End Function

#End Region

    End Class

End Namespace
