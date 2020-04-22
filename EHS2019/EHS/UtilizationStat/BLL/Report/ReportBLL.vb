Imports System.Xml
Imports System.Data

Namespace Report
    Public Class ReportBLL
        Public Function GetReportList() As DataTable
            Dim ds As DataSet = New DataSet

            ds.ReadXml(GetXmlFileLocation())

            Return ds.Tables(0)

        End Function

        Public Function GetReport(ByVal strReportID As String) As ReportModel
            Dim udtReportModel As ReportModel = Nothing
            Dim ds As DataSet = New DataSet
            ds.ReadXml(GetXmlFileLocation())
            For Each dt As DataTable In ds.Tables
                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        If Not dr.Item("ReportID").Equals(String.Empty) Then
                            If CStr(dr.Item("ReportID")).Equals(strReportID) Then
                                'CRE13-016 Upgrade to excel 2007 [Start][Karl]
                                udtReportModel = FillReportModel(dr)

                                'Dim strStartRow() As String = CStr(dr.Item("StartRowNo")).Split(",")

                                ' Dim intStartRow() As Integer = Array.ConvertAll(strStartRow, New Converter(Of String, Integer)(AddressOf ConvertStringToInt))
                                '   Dim intStartRow As List(Of Integer)

                                '   intStartRow = New List(Of Integer)(Array.ConvertAll(Of String, Integer)(CStr(dr.Item("StartRowNo")).Split(","), AddressOf ConvertStringToInt))

                                'udtReportModel = New ReportModel(dr.Item("ReportID"), _
                                '                                      dr.Item("ReportName"), _
                                '                                      dr.Item("ReportDesc"), _
                                '                                      dr.Item("ExecSP"), _
                                '                                      dr.Item("TemplateName"), _
                                '                                      intStartRow, _
                                '                                      dr.Item("FileNameFormat"), _
                                '                                      CInt(dr.Item("MinusDateForFileName")), _
                                '                                      IIf(CStr(dr.Item("DailyGenerate")).Equals("T"), True, False), _
                                '                                      dr.Item("DBFlag"))

                                'CRE13-016 Upgrade to excel 2007 [End][Karl]

                            End If
                        End If
                    Next
                End If
            Next

            Return udtReportModel
        End Function

        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        Public Function GetAllReport() As ReportModelCollection
            Dim udtReportModel As ReportModel = Nothing
            Dim udtReportModelCollection As New ReportModelCollection
            Dim ds As DataSet = New DataSet
            ds.ReadXml(GetXmlFileLocation())

            For Each dt As DataTable In ds.Tables
                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows

                        udtReportModel = FillReportModel(dr)

                        udtReportModelCollection.Add(udtReportModel)
                    Next
                End If
            Next

            Return udtReportModelCollection
        End Function

        Private Function FillReportModel(ByVal dr As DataRow) As ReportModel
            Dim intStartRow As List(Of Integer)
            Dim udtReportModel As ReportModel = Nothing

            intStartRow = New List(Of Integer)(Array.ConvertAll(Of String, Integer)(CStr(dr.Item("StartRowNo")).Split(","), AddressOf ConvertStringToInt))

            udtReportModel = New ReportModel(dr.Item("ReportID"), _
                                                                                  dr.Item("ReportName"), _
                                                                                  dr.Item("ReportDesc"), _
                                                                                  dr.Item("ExecSP"), _
                                                                                  dr.Item("TemplateName"), _
                                                                                  dr.Item("ReportFileExt").ToString.ToLower, _
                                                                                  intStartRow, _
                                                                                  dr.Item("FileNameFormat"), _
                                                                                  CInt(dr.Item("MinusDateForFileName")), _
                                                                                  IIf(CStr(dr.Item("DailyGenerate")).Equals("T"), True, False), _
                                                                                  dr.Item("DBFlag"))


            Return udtReportModel
        End Function

        Public Function GetReportExtList() As String()
            Dim udtReportModelCollection As New ReportModelCollection
            Dim strFileExt() As String = Nothing
            Dim intArrayCtrl As Integer
            Dim intCount As Integer
            Dim blnFileTypeExist As Boolean

            udtReportModelCollection = GetAllReport()

            For Each udtReportModel As ReportModel In udtReportModelCollection
                blnFileTypeExist = False

                If strFileExt Is Nothing OrElse strFileExt.Length < 0 Then
                    ReDim Preserve strFileExt(intArrayCtrl)
                    strFileExt(intArrayCtrl) = udtReportModel.ReportFileExt
                    intArrayCtrl += 1
                Else
                    For intCount = 0 To strFileExt.Length - 1
                        If strFileExt(intCount).ToLower.Trim = udtReportModel.ReportFileExt.ToLower.Trim Then
                            blnFileTypeExist = True
                        End If
                    Next

                    If blnFileTypeExist = False Then
                        ReDim Preserve strFileExt(intArrayCtrl)
                        strFileExt(intArrayCtrl) = udtReportModel.ReportFileExt
                        intArrayCtrl += 1
                    End If

                End If
            Next

            Return strFileExt

        End Function

        'CRE13-016 Upgrade to excel 2007 [End][Karl]
        Public Shared Function ConvertStringToInt(ByVal str As String) As Integer
            Return CInt(str)
        End Function

        Private Function GetXmlFileLocation() As String
            Dim strFileLocation As String = ConfigurationManager.AppSettings("XMLPath")
            Return strFileLocation
        End Function
    End Class

End Namespace
