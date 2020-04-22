Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices.Marshal
Imports System.IO

Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Component

Public Class DataExportBLL

    Public Sub New()

    End Sub

    Public Function GetReportData(ByVal strReportID As String, ByVal strPassword As String) As String
        Dim strSheetName As String = String.Empty
        Dim udtReportBLL As Report.ReportBLL = New Report.ReportBLL
        Dim udtReport As Report.ReportModel = Nothing

        Dim strFilePath As String
        Dim strTemplatePath As String
        Dim ds As DataSet
        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        Dim strFileExt As String = Nothing
        'CRE13-016 Upgrade to excel 2007 [End][Karl]

        Try
            udtReport = udtReportBLL.GetReport(strReportID)
            strFilePath = System.Configuration.ConfigurationManager.AppSettings("FileExportPath").ToString()

            'If strFilePath.Substring(strFilePath.Length - 1, 1) <> "\" Then
            '    strFilePath = strFilePath & "\GeneratedXLS\"
            'Else
            '    strFilePath = strFilePath & "GeneratedXLS\"
            'End If

            RemoveFiles(strFilePath)

            strSheetName = Me.GetGeneratedFileName(udtReport.FileNameFormat.Trim, udtReport.MinusDateForFileName)

            'CRE13-016 Upgrade to excel 2007 [Start][Karl]
            strFileExt = (New Common.Format.Formatter).FormatFileExt(udtReport.ReportFileExt)
            'CRE13-016 Upgrade to excel 2007 [End][Karl]

            GenerateZip.Log("Generate: " & strSheetName & strFileExt & " .... ")

            ds = Me.GetDataFromDB(udtReport.ExecSP, udtReport.DBFlag)

            If Not strSheetName.Equals(String.Empty) Then

                'CRE13-016 Upgrade to excel 2007 [Start][Karl]
                strFilePath = strFilePath & strSheetName & strFileExt
                'strFilePath = strFilePath & strSheetName & ".xls"
                'CRE13-016 Upgrade to excel 2007 [End][Karl]

                strTemplatePath = System.Configuration.ConfigurationManager.AppSettings("TemplatePath").ToString()

                'CRE13-016 Upgrade to excel 2007 [Start][Karl]
                strTemplatePath = System.IO.Path.Combine(strTemplatePath, udtReport.TemplateName.Trim & strFileExt)
                'strTemplatePath = System.IO.Path.Combine(strTemplatePath, udtReport.TemplateName.Trim + ".xls")
                'CRE13-016 Upgrade to excel 2007 [End][Karl]

                ' CRE11-029 Add CMS health check log [Start][Tommy]
                Export2Excel(ds, strFilePath, strTemplatePath, strSheetName, udtReport.StartRowNo, udtReport.StartRowNoExt, strPassword)

                ' CRE11-029 Add CMS health check log [End][Tommy]

                GenerateZip.LogLine("OK", False)
            End If


        Catch ex As Exception
            GenerateZip.LogLine("Fail", False)
            Throw ex
        End Try

        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        'Return strSheetName
        Return strSheetName & strFileExt
        'CRE13-016 Upgrade to excel 2007 [End][Karl]
    End Function

    ' CRE11-029 Add CMS health check log [Start][Tommy]

    'Export the datatable to excel with the standard template
    'Private Sub Export2Excel(ByVal dtFormData As DataTable, ByVal generateFileName As String, ByVal templateName As String, ByVal sheetName As String, ByVal intStartRow As Integer)

    '    Dim thisThread As System.Threading.Thread = System.Threading.Thread.CurrentThread
    '    Dim originalCulture As System.Globalization.CultureInfo = thisThread.CurrentCulture

    '    Try
    '        thisThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

    '        Dim oExcel As New Excel.Application
    '        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
    '        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
    '        Dim oCells As Excel.Range

    '        Dim filePath As String
    '        Dim templatePath As String

    '        filePath = generateFileName
    '        templatePath = templateName


    '        'copy the template file to a new excel file

    '        If Not File.Exists(filePath) Then
    '            Dim fileOpen As New FileInfo(templatePath)
    '            If fileOpen.Exists() Then
    '                Dim fileSave As FileInfo = fileOpen.CopyTo(filePath)

    '                If Not fileSave.Exists Then
    '                    Throw New Exception("File Copy Fail!" + fileSave.FullName + "," + fileOpen.FullName)
    '                End If

    '                oExcel.Visible = False : oExcel.DisplayAlerts = False

    '                oBooks = oExcel.Workbooks
    '                oBooks.Open(filePath)
    '                oBook = oBooks.Item(1)

    '                oSheets = oBook.Worksheets
    '                oSheet = CType(oSheets.Item(1), Excel.Worksheet)

    '                oSheet.Name = "eHS_Utitlization_Summary" 'sheetName

    '                'Fill in the data
    '                oCells = DataTable2Excel(dtFormData, oSheet, intStartRow)

    '                'Save in a temporary file
    '                oSheet.SaveAs(filePath)


    '                oBook.Close()

    '                'Quit Excel and thoroughly deallocate everything
    '                oExcel.Quit()

    '                ReleaseComObject(oCells) : ReleaseComObject(oSheet)
    '                ReleaseComObject(oSheets) : ReleaseComObject(oBook)
    '                ReleaseComObject(oBooks) : ReleaseComObject(oExcel)

    '                oExcel = Nothing : oBooks = Nothing : oBook = Nothing
    '                oSheets = Nothing : oSheet = Nothing : oCells = Nothing

    '                System.GC.Collect()

    '            Else
    '                Throw New Exception("Template Not Found!" + fileOpen.FullName)
    '            End If
    '        End If

    '        Dim Copyfile As New System.IO.FileInfo(filePath)

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        thisThread.CurrentCulture = originalCulture
    '    End Try

    'End Sub

    'Export the dataset to excel with the standard template
    'Private Sub Export2Excel(ByVal dsFormData As DataSet, ByVal generateFileName As String, ByVal templateName As String, ByVal sheetName As String, ByVal intStartRow As Integer)
    '    Dim thisThread As System.Threading.Thread = System.Threading.Thread.CurrentThread
    '    Dim originalCulture As System.Globalization.CultureInfo = thisThread.CurrentCulture

    '    Try
    '        thisThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

    '        Dim oExcel As New Excel.Application
    '        Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
    '        Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
    '        Dim oCells As Excel.Range

    '        Dim filePath As String
    '        Dim templatePath As String

    '        filePath = generateFileName
    '        templatePath = templateName
    '        Dim i As Integer = 1

    '        'copy the template file to a new excel file

    '        If Not File.Exists(filePath) Then
    '            Dim fileOpen As New FileInfo(templatePath)
    '            If fileOpen.Exists() Then
    '                Dim fileSave As FileInfo = fileOpen.CopyTo(filePath)

    '                If Not fileSave.Exists Then
    '                    Throw New Exception("File Copy Fail!" + fileSave.FullName + "," + fileOpen.FullName)
    '                End If

    '                oExcel.Visible = False : oExcel.DisplayAlerts = False

    '                oBooks = oExcel.Workbooks
    '                oBooks.Open(filePath)
    '                oBook = oBooks.Item(1)

    '                oSheets = oBook.Worksheets


    '                For Each dtFormData As DataTable In dsFormData.Tables
    '                    oSheet = CType(oSheets.Item(i), Excel.Worksheet)

    '                    'oSheet.Name = "eHS_Utitlization_Summary" & i.ToString 'sheetName

    '                    'Fill in the data
    '                    oCells = DataTable2Excel(dtFormData, oSheet, intStartRow)

    '                    'Save in a temporary file
    '                    oSheet.SaveAs(filePath)

    '                    i = i + 1
    '                Next

    '                oBook.Close()

    '                'Quit Excel and thoroughly deallocate everything
    '                oExcel.Quit()

    '                ReleaseComObject(oCells) : ReleaseComObject(oSheet)
    '                ReleaseComObject(oSheets) : ReleaseComObject(oBook)
    '                ReleaseComObject(oBooks) : ReleaseComObject(oExcel)

    '                oExcel = Nothing : oBooks = Nothing : oBook = Nothing
    '                oSheets = Nothing : oSheet = Nothing : oCells = Nothing

    '                System.GC.Collect()

    '            Else
    '                Throw New Exception("Template Not Found!" + fileOpen.FullName)
    '            End If
    '        End If

    '        Dim Copyfile As New System.IO.FileInfo(filePath)

    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        thisThread.CurrentCulture = originalCulture
    '    End Try


    'End Sub

    ' CRE11-029 Add CMS health check log [End][Tommy]

    'Export the dataset to excel with the standard template

    ' CRE11-029 Add CMS health check log [Start][Tommy]

    Private Sub Export2Excel(ByVal dsFormData As DataSet, ByVal generateFileName As String, ByVal templateName As String, ByVal sheetName As String, ByVal intStartRow() As Integer, ByVal intStartRowExt() As Integer, ByVal strPassword As String)

        ' CRE11-029 Add CMS health check log [End][Tommy]

        Dim thisThread As System.Threading.Thread = System.Threading.Thread.CurrentThread
        Dim originalCulture As System.Globalization.CultureInfo = thisThread.CurrentCulture

        Try
            thisThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

            Dim oExcel As New Excel.Application
            Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook

            ' CRE11-029 Add CMS health check log [Start][Tommy]

            Dim oSheets As Excel.Sheets
            'Dim oSheet As Excel.Worksheet
            'Dim oCells As Excel.Range

            ' CRE11-029 Add CMS health check log [End][Tommy]

            Dim filePath As String
            Dim templatePath As String

            filePath = generateFileName
            templatePath = templateName


            'copy the template file to a new excel file

            If Not File.Exists(filePath) Then
                Dim fileOpen As New FileInfo(templatePath)
                If fileOpen.Exists() Then
                    Dim fileSave As FileInfo = fileOpen.CopyTo(filePath)

                    If Not fileSave.Exists Then
                        Throw New Exception("File Copy Fail!" + fileSave.FullName + "," + fileOpen.FullName)
                    End If

                    oExcel.Visible = False : oExcel.DisplayAlerts = False

                    oBooks = oExcel.Workbooks
                    oBooks.Open(filePath)
                    oBook = oBooks.Item(1)

                    oSheets = oBook.Worksheets

                    ' CRE11-029 Add CMS health check log [Start][Tommy]

                    Dim intSheetIndex As Integer = 1
                    Dim intDataTableIndex As Integer = 0
                    For Each dtFormData As DataTable In dsFormData.Tables
                        Dim intOffset As Integer = 0
                        DataTableToWorkSheet(dtFormData, oSheets, intSheetIndex, intOffset, intStartRow(intDataTableIndex), intStartRowExt(intDataTableIndex))
                        intSheetIndex = intSheetIndex + intOffset + 1
                        intDataTableIndex += 1
                    Next

                    ' CRE11-029 Add CMS health check log [End][Tommy]

                    If Not strPassword.Equals(String.Empty) Then
                        oBook.Password = strPassword
                        oBook.SaveAs(filePath, Password:=strPassword)
                    Else
                        oBook.SaveAs(filePath)
                    End If

                    oBook.Close()

                    'Quit Excel and thoroughly deallocate everything
                    oExcel.Quit()

                    ' CRE11-029 Add CMS health check log [Start][Tommy]

                    'ReleaseComObject(oCells)
                    'ReleaseComObject(oSheet)
                    ReleaseComObject(oSheets)
                    ReleaseComObject(oBook)
                    ReleaseComObject(oBooks)
                    ReleaseComObject(oExcel)

                    'oCells = Nothing
                    'oSheet = Nothing
                    oSheets = Nothing
                    oBook = Nothing
                    oBooks = Nothing
                    oExcel = Nothing

                    ' CRE11-029 Add CMS health check log [End][Tommy]

                    System.GC.Collect()

                Else
                    Throw New Exception("Template Not Found!" + fileOpen.FullName)
                End If
            End If

            Dim Copyfile As New System.IO.FileInfo(filePath)

        Catch ex As Exception
            Throw ex
        Finally
            thisThread.CurrentCulture = originalCulture
        End Try


    End Sub

    ' CRE11-029 Add CMS health check log [Start][Tommy]

    Private Sub DataTableToWorkSheet(ByVal dtFormData As DataTable, ByRef oSheets As Excel.Sheets, ByRef intMainSheetIndex As Integer, ByRef intOffset As Integer, ByVal intStartRow As Integer, ByVal intStartRowExt As Integer)
        ' CRE13-016 - Upgrade to excel 2007 [Start][Karl]
        ' -----------------------------------------------------------------------------------------
        ' Dim intRecordLimit As Integer = System.Configuration.ConfigurationManager.AppSettings("ExcelWorkSheetRowLimit").ToString()
        Dim intRecordLimit As Integer
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction
        Dim udtFileGenerationBLL As New Common.Component.FileGeneration.FileGenerationBLL

        intRecordLimit = udtFileGenerationBLL.GetExcelWorkSheetMaxRow()
        ' CRE13-016 - Upgrade to excel 2007 [End][Karl]

        Dim intSheetCount As Integer = Math.Ceiling(dtFormData.Rows.Count / intRecordLimit)
        Dim dtFormDataTemp As DataTable = New System.Data.DataTable
        dtFormDataTemp = dtFormData.Clone
        Dim intStart As Integer = 0
        Dim intEnd As Integer = intRecordLimit - 1
        Dim intFirstRow As Integer = 1

        For i As Integer = 0 To intSheetCount - 1
            If intEnd > dtFormData.Rows.Count - 1 Then
                intEnd = dtFormData.Rows.Count - 1
            End If
            For j As Integer = intStart To intEnd
                dtFormDataTemp.ImportRow(dtFormData.Rows(j))
            Next
            Dim oSheet As Excel.Worksheet = CType(oSheets.Item(intMainSheetIndex + intOffset), Excel.Worksheet)
            If intOffset > 0 Then
                intFirstRow = intStartRowExt
            Else
                intFirstRow = intStartRow
            End If
            If Not i = intSheetCount - 1 Then
                oSheet.Copy(After:=oSheet)
                intOffset += 1
            End If
            Dim oCells As Excel.Range = DataTable2Excel(dtFormDataTemp, oSheet, intFirstRow)
            dtFormDataTemp.Rows.Clear()
            intStart = intEnd + 1
            intEnd = intEnd + intRecordLimit
        Next

    End Sub

    ' CRE11-029 Add CMS health check log [End][Tommy]

    'Outputs a DataTable to an Excel Worksheet
    Private Function DataTable2Excel(ByVal dt As DataTable, ByVal oSheet As Excel.Worksheet, ByVal intStartRow As Integer) As Excel.Range
        Dim oCells As Excel.Range
        oCells = oSheet.Cells

        Dim dr As DataRow, ary() As Object
        Dim iRow As Integer, iCol As Integer


        'Output Data
        For iRow = 0 To dt.Rows.Count - 1
            dr = dt.Rows.Item(iRow)
            ary = dr.ItemArray

            For iCol = 0 To UBound(ary)
                oCells(iRow + intStartRow, iCol + 1) = ary(iCol).ToString
            Next
        Next

        Return oCells
    End Function


    Public Function GetDataFromDB(ByVal strExecSP As String, ByVal strDBFlag As String) As DataSet
        Dim ds As DataSet = Nothing
        Try
            ds = New DataSet
            Dim udtDB As New Database(strDBFlag)

            ' To Do: Move the Command Time out to XML Setting
            If strDBFlag.ToUpper().Trim().Equals("DBFlag2".ToUpper()) Then
                Dim intDBCommandTimeout As Integer
                intDBCommandTimeout = CInt(System.Configuration.ConfigurationManager.AppSettings("DBReplication_CommandTimeout"))
                udtDB.CommandTimeout = intDBCommandTimeout
            End If

            udtDB.RunProc(strExecSP, ds)

            Return ds
        Catch ex As Exception
            Throw ex
        End Try
        Return ds
    End Function

    Public Function GetGeneratedFileName(ByVal strFileNameFormat As String, ByVal intMinusDateForFileName As Integer) As String
        Dim strRes As String = String.Empty
        If intMinusDateForFileName = 0 Then
            strRes = strFileNameFormat + "_" + Now.ToString("yyyyMMdd")
        Else
            Dim i As Integer
            i = 0 - intMinusDateForFileName
            strRes = strFileNameFormat + "_" + Now.AddDays(i).ToString("yyyyMMdd")
        End If

        Return strRes
    End Function

    Public Sub RemoveFiles(ByVal strPath As String)
        Dim di As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(strPath)
        Dim fiArr As FileInfo() = di.GetFiles
        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        Dim udtReportBLL As Report.ReportBLL = New Report.ReportBLL
        Dim strFileExt() As String = udtReportBLL.GetReportExtList
        Dim intCount As Integer

        'For Each fri As FileInfo In fiArr
        '    If fri.Extension.ToString.Equals(".xls") Then
        '        fri.Delete()
        '    End If
        'Next

        For intCount = 0 To strFileExt.Length - 1
            For Each fri As FileInfo In fiArr
                If fri.Extension.ToString.Equals((New Common.Format.Formatter).FormatFileExt(strFileExt(intCount))) Then
                    fri.Delete()
                End If
            Next
        Next
        'CRE13-016 Upgrade to excel 2007 [End][Karl]

    End Sub

End Class
