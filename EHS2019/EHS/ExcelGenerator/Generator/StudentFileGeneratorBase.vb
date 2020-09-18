Imports System.Configuration
Imports System.Data.SqlClient
Imports Common.ComFunction.ParameterFunction
Imports Common.Component.Scheme

Namespace Generator

    Public MustInherit Class StudentFileGeneratorBase
        Inherits BaseGenerator

        Protected Sub New(ByVal udtQueue As Common.Component.FileGeneration.FileGenerationQueueModel, ByVal udtFileGenerationModel As Common.Component.FileGeneration.FileGenerationModel)
            MyBase.New(udtQueue, udtFileGenerationModel)

        End Sub

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
        'Protected Shared ReadOnly Property StudentFileNameList As String()
        '    Get
        '        Dim strNameList As String = String.Empty
        '        Dim lstNameList As String() = Nothing
        '        Dim udtCommonGenFunction As New Common.ComFunction.GeneralFunction()

        '        udtCommonGenFunction.getSystemParameter("StudentFileNameList", strNameList, String.Empty)
        '        lstNameList = strNameList.Split(New Char() {"|||"}, StringSplitOptions.RemoveEmptyEntries)

        '        Return lstNameList
        '    End Get
        'End Property
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]

        'Public Overrides Function GetXLSParameter() As List(Of Integer)
        '    Dim lstXLSParameter As List(Of Integer) = Nothing
        '    Dim dsDataSource As New DataSet

        '    dsDataSource = Me.GetDataSet()

        '    Dim intDataTableCount As Integer = dsDataSource.Tables.Count
        '    If intDataTableCount > 0 Then
        '        lstXLSParameter = New List(Of Integer)
        '        For i As Integer = 0 To intDataTableCount
        '            Dim intNumber As Integer = 1
        '            lstXLSParameter.Add(intNumber)
        '        Next
        '    End If

        '    Return lstXLSParameter
        'End Function


        Public Overrides Function GetDataSet() As System.Data.DataSet
            Dim dsData As New DataSet()

            Dim udtDB As New Common.DataAccess.Database()
            Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
            Dim udtSPParamCollection As StoreProcParamCollection = udtParamFunction.GetSPParamCollection(Me.m_udtQueue.InParm)

            Dim params(udtSPParamCollection.Count) As SqlParameter

            For i As Integer = 0 To udtSPParamCollection.Count - 1
                Dim udtSPParamObject As StoreProcParamObject = udtSPParamCollection(i)
                params(i) = udtDB.MakeInParam(udtSPParamObject.ParamName, udtSPParamObject.ParamDBType, udtSPParamObject.ParamDBSize, udtSPParamObject.ParamValue)
            Next

            udtDB.RunProc(Me.m_udtFileGeneration.FileDataSP, params, dsData)

            Return dsData
        End Function

        ' CRE19-001-04 (PPP 2019-20) [Start][Koala]
        Public Overrides Function GetTemplate() As String

            Dim strTemplatePath As String = String.Empty

            If Me.m_udtFileGeneration.ReportTemplate Is Nothing OrElse Me.m_udtFileGeneration.ReportTemplate.Trim() = "" Then
                Return String.Empty
            Else

                Dim udtCommonGenFunction As New Common.ComFunction.GeneralFunction()
                udtCommonGenFunction.getSystemParameter("ExcelWithTemplatePath", strTemplatePath, String.Empty)
                Return System.AppDomain.CurrentDomain.BaseDirectory + strTemplatePath + GetReportTemplateNameBySubsidize()
            End If
        End Function

        ''' <summary>
        ''' Get Student File Report Template
        ''' Change "eHSVF001-VaccinationFileTemplate-[%Subsidize%].xlsx" to "eHSVF001-VaccinationFileTemplate-SIV.xlsx"
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetReportTemplateNameBySubsidize() As String
            ' Get Student File Header Model
            Dim udtStudentHeader As Common.Component.StudentFile.StudentFileHeaderModel = GetStudentFileHeader()

            ' Get Subsidize Item Code (e.g. SIV, PV, PV13)
            Dim bllSubsidize As New Common.Component.Scheme.SubsidizeBLL

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            Dim strTemplateName As String = Me.m_udtFileGeneration.ReportTemplate

            If udtStudentHeader.SubsidizeCode <> String.Empty Then
                Dim strSubsidizeItemCode As String = bllSubsidize.GetSubsidizeItemBySubsidize(udtStudentHeader.SubsidizeCode)

                If udtStudentHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                    strTemplateName = strTemplateName.Replace("[%Scheme%]", String.Empty)
                    strTemplateName = strTemplateName.Replace("[%Subsidize%]", String.Empty)
                    strTemplateName = strTemplateName.Replace("[%Scheme_Subsidize%]", String.Format("-{0}-{1}", udtStudentHeader.SchemeCode, strSubsidizeItemCode))

                Else
                    ' CRE20-003 (Batch Upload) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    Select Case udtStudentHeader.SchemeCode
                        Case SchemeClaimModel.RVP
                            strTemplateName = strTemplateName.Replace("[%Scheme%]", String.Format("-{0}", udtStudentHeader.SchemeCode))
                            strTemplateName = strTemplateName.Replace("[%Subsidize%]", String.Format("-{0}", strSubsidizeItemCode))
                            strTemplateName = strTemplateName.Replace("[%Scheme_Subsidize%]", String.Empty)

                        Case Else
                            strTemplateName = strTemplateName.Replace("[%Scheme%]", String.Empty)
                            strTemplateName = strTemplateName.Replace("[%Subsidize%]", String.Format("-{0}", strSubsidizeItemCode))
                            strTemplateName = strTemplateName.Replace("[%Scheme_Subsidize%]", String.Empty)

                    End Select
                    ' CRE20-003 (Batch Upload) [End][Chris YIM]

                End If

            Else
                strTemplateName = strTemplateName.Replace("[%Scheme%]", String.Empty)
                strTemplateName = strTemplateName.Replace("[%Subsidize%]", String.Empty)
                strTemplateName = strTemplateName.Replace("[%Scheme_Subsidize%]", String.Empty)

            End If

            Return strTemplateName
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]
        End Function

        Protected Function GetStudentFileHeader() As Common.Component.StudentFile.StudentFileHeaderModel
            Dim udtDB As New Common.DataAccess.Database()
            Dim udtParamFunction As New Common.ComFunction.ParameterFunction()
            Dim udtSPParamCollection As StoreProcParamCollection = udtParamFunction.GetSPParamCollection(Me.m_udtQueue.InParm)

            ' Get Student File ID
            Dim strStudentFileID As String = udtSPParamCollection(0).ParamValue

            ' Get Student File Header Model
            Dim bllStudentFile As New Common.Component.StudentFile.StudentFileBLL
            Dim udtStudentHeader As Common.Component.StudentFile.StudentFileHeaderModel = bllStudentFile.GetStudentFileHeader(strStudentFileID, False, udtDB)

            Return udtStudentHeader
        End Function
        ' CRE19-001-04 (PPP 2019-20) [End][Koala]
    End Class



End Namespace
