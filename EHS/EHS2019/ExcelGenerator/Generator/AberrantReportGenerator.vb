Imports System.Data.SqlClient
Imports Common
Imports Common.DataAccess
Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.Inbox

Namespace Generator

    Public Class AberrantReportGenerator
        Inherits ARReportGeneratorBase
        Implements IExcelGenerable

        Dim _udtFormatter As Format.Formatter = New Format.Formatter()
        Dim _dictParameter As Dictionary(Of String, String) = New Dictionary(Of String, String)

        Sub New(ByVal udtQueue As Common.Component.FileGeneration.FileGenerationQueueModel, ByVal udtFileGenerationModel As Common.Component.FileGeneration.FileGenerationModel)
            MyBase.New(udtQueue, udtFileGenerationModel)

            ' Get Out Put Path
            Dim strPath As String = String.Empty
            Dim udtCommonGenFunction As New Common.ComFunction.GeneralFunction()
            udtCommonGenFunction.getSystemParameter("ExcelWithTemplateDownloadStoragePath", strPath, String.Empty)
            If strPath.Trim() = "" Then
                Throw New ArgumentException("ExcelWithTemplateDownloadStoragePath Empty!")
            End If

            If Not strPath.Trim().EndsWith("\") Then
                strPath = strPath & "\"
            End If
            Me.m_strFileOutPath = strPath

        End Sub

        Public Overrides Sub ConstructMessageParamaterList(ByRef udtDB As Common.DataAccess.Database, ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection)
            Dim udtFileGenerationBLL As New Common.Component.FileGeneration.FileGenerationBLL()
            Dim udtGeneral As New Common.ComFunction.GeneralFunction()
            Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
            Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()

            udtMessageCollection = Nothing
            udtMessageReaderCollection = Nothing

            'Dim udtDB As New Common.DataAccess.Database()
            Dim dtResult As DataTable = udtFileGenerationBLL.GetViewAccessibleUser(udtDB, Me.m_udtQueue.GenerationID)

            ' This report will not send to any users
            If Not IsNothing(dtResult) Then
                If dtResult.Rows.Count > 0 Then

                    udtMessageCollection = New MessageModelCollection()
                    udtMessageReaderCollection = New MessageReaderModelCollection()

                    ' Construct Message & MessageReader
                    ' One Message, To Mutilple User (Other Case May Mutil Message, Each Message to Single User
                    Dim udtMessage As New MessageModel()
                    udtMessage.MessageID = udtGeneral.generateInboxMsgID()

                    Dim paramsSubject As New ParameterCollection()
                    paramsSubject.AddParam("FileName", Me.GetFileName())

                    Dim paramsContent As New ParameterCollection()
                    paramsContent.AddParam("FileName", Me.GetFileName())
                    paramsContent.AddParam("Link", "../ReportAndDownload/Datadownload.aspx")

                    For Each strParameterName As String In _dictParameter.Keys
                        paramsSubject.AddParam(strParameterName, _dictParameter(strParameterName))
                        paramsContent.AddParam(strParameterName, _dictParameter(strParameterName))
                    Next

                    udtMessage.Subject = MyBase.GetMessageBySearchResult(udtParamFunction.GetParsedStringByparameter(Me.m_udtFileGeneration.MessageSubject, paramsSubject))
                    udtMessage.Message = MyBase.GetMessageBySearchResult(udtParamFunction.GetParsedStringByparameter(Me.m_udtFileGeneration.MessageContent, paramsContent))
                    udtMessage.CreateBy = "EHCVS"
                    udtMessage.CreateDtm = dtmCurrent
                    udtMessageCollection.Add(udtMessage)

                    For Each drRow As DataRow In dtResult.Rows

                        Dim strUserId As String = drRow("User_ID").ToString().Trim()
                        Dim strUserName As String = drRow("User_Name").ToString().Trim()

                        Dim udtMessageReader As New MessageReaderModel()
                        udtMessageReader.MessageID = udtMessage.MessageID
                        udtMessageReader.MessageReader = strUserId
                        udtMessageReader.UpdateBy = "EHCVS"
                        udtMessageReader.UpdateDtm = dtmCurrent

                        udtMessageReaderCollection.Add(udtMessageReader)
                    Next

                End If
            End If

        End Sub

        Public Overrides Function GetDataSet() As System.Data.DataSet

            Dim dsData As DataSet = New DataSet()
            Dim udtDB As Database = GetDataBase()

            ' Pass in Request DateTime as Reference Date for the Report
            Dim params(1) As SqlParameter
            params(0) = udtDB.MakeInParam("@request_dtm", SqlDbType.DateTime, 8, Me.m_udtQueue.RequestDtm)

            udtDB.RunProc(Me.m_udtFileGeneration.FileDataSP, params, dsData)

            If Not dsData Is Nothing AndAlso dsData.Tables.Count > 0 Then
                ' Get Report Parameter
                If dsData.Tables(0).Rows.Count > 0 Then
                    Dim dr As DataRow = dsData.Tables(0).Rows(0)
                    Dim strTemp As String = String.Empty

                    For Each dcColumn As DataColumn In dsData.Tables(0).Columns
                        If dr.IsNull(dcColumn) Then
                            strTemp = String.Empty
                        ElseIf dcColumn.ColumnName = "HaveResult" Then
                            ContainSearchResult = IIf(dr(dcColumn).ToString() = "Y", True, False)
                            Continue For
                        Else
                            strTemp = dr(dcColumn).ToString()
                        End If

                        _dictParameter.Add(dcColumn.ColumnName, strTemp)
                    Next
                End If

                dsData.Tables.Remove(dsData.Tables(0))
                dsData.AcceptChanges()
            End If

            Return dsData
        End Function

    End Class

End Namespace
