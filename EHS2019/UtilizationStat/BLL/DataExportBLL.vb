Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices.Marshal
Imports System.IO

Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Component

Public Class DataExportBLL

    Public Sub New()

    End Sub
    'CRE13-016 Upgrade to excel 2007 [Start][Karl]
    'Public Function GetData(ByVal type As String) As String

    '    Dim dt As DataTable
    '    Dim ds As DataSet
    '    Dim strFilePath As String = String.Empty
    '    Dim strTemplatePath As String = String.Empty
    '    Dim strSheetName As String = String.Empty

    '    Try
    '        Select Case type
    '            Case "Enrol"
    '                strFilePath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath)

    '                If strFilePath.Substring(strFilePath.Length - 1, 1) <> "\" Then
    '                    strFilePath = strFilePath & "\GeneratedXLS\"
    '                Else
    '                    strFilePath = strFilePath & "GeneratedXLS\"
    '                End If

    '                RemoveFiles(strFilePath)

    '                dt = GetUtilizationStat()
    '                strSheetName = GenerateFileName("Enrol")

    '                If Not strSheetName.Equals(String.Empty) Then
    '                    strFilePath = strFilePath & strSheetName & ".xls"

    '                    strTemplatePath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath)

    '                    If strTemplatePath.Substring(strTemplatePath.Length - 1, 1) <> "\" Then
    '                        strTemplatePath = strTemplatePath & "\Template\DataEnrolStat_Template.xls"
    '                    Else
    '                        strTemplatePath = strTemplatePath & "Template\DataEnrolStat_Template.xls"
    '                    End If

    '                    Export2Excel(dt, strFilePath, strTemplatePath, strSheetName, 5)
    '                End If


    '            Case "MO"
    '                strFilePath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath)

    '                If strFilePath.Substring(strFilePath.Length - 1, 1) <> "\" Then
    '                    strFilePath = strFilePath & "\GeneratedXLS\"
    '                Else
    '                    strFilePath = strFilePath & "GeneratedXLS\"
    '                End If

    '                RemoveFiles(strFilePath)

    '                ds = GetMOStat()
    '                strSheetName = GenerateFileName("MO")
    '                If Not strSheetName.Equals(String.Empty) Then
    '                    strFilePath = strFilePath & strSheetName & ".xls"

    '                    strTemplatePath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath)

    '                    If strTemplatePath.Substring(strTemplatePath.Length - 1, 1) <> "\" Then
    '                        strTemplatePath = strTemplatePath & "\Template\MO_Template.xls"
    '                    Else
    '                        strTemplatePath = strTemplatePath & "Template\MO_Template.xls"
    '                    End If

    '                    Export2Excel(ds, strFilePath, strTemplatePath, strSheetName, 5)
    '                End If

    '            Case "EnrolledPractice"
    '                strFilePath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath)

    '                If strFilePath.Substring(strFilePath.Length - 1, 1) <> "\" Then
    '                    strFilePath = strFilePath & "\GeneratedXLS\"
    '                Else
    '                    strFilePath = strFilePath & "GeneratedXLS\"
    '                End If

    '                RemoveFiles(strFilePath)

    '                ds = GetEnrolledPracticeStat()
    '                strSheetName = GenerateFileName("EnrolledPractice")
    '                If Not strSheetName.Equals(String.Empty) Then
    '                    strFilePath = strFilePath & strSheetName & ".xls"

    '                    strTemplatePath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath)

    '                    If strTemplatePath.Substring(strTemplatePath.Length - 1, 1) <> "\" Then
    '                        strTemplatePath = strTemplatePath & "\Template\EnrolledPracticeStat_Template.xls"
    '                    Else
    '                        strTemplatePath = strTemplatePath & "Template\EnrolledPracticeStat_Template.xls"
    '                    End If

    '                    Export2Excel(ds, strFilePath, strTemplatePath, strSheetName, 2)
    '                End If
    '        End Select

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    '    Return strSheetName
    'End Function
    'CRE13-016 Upgrade to excel 2007 [End][Karl]

    Public Function GetReportData(ByVal strReportID As String, ByVal strPassword As String) As String
        Dim strSheetName As String = String.Empty
        Dim udtReportBLL As Report.ReportBLL = New Report.ReportBLL
        Dim udtReport As Report.ReportModel = Nothing

        Dim strFilePath As String
        Dim strTemplatePath As String
        'CRE13-016 Upgrade to excel 2007 [Start][Karl]
        Dim strFileExt As String = Nothing
        'CRE13-016 Upgrade to excel 2007 [End][Karl]
        Dim ds As DataSet

        Try
            udtReport = udtReportBLL.GetReport(strReportID)
            strFilePath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath)

            If strFilePath.Substring(strFilePath.Length - 1, 1) <> "\" Then
                strFilePath = strFilePath & "\GeneratedXLS\"
            Else
                strFilePath = strFilePath & "GeneratedXLS\"
            End If

            'CRE13-016 Upgrade to excel 2007 [Start][Karl]
            strFileExt = (New Common.Format.Formatter).FormatFileExt(udtReport.ReportFileExt)
            'CRE13-016 Upgrade to excel 2007 [End][Karl]

            RemoveFiles(strFilePath)

            ds = Me.GetDataFromDB(udtReport.ExecSP, udtReport.DBFlag)
            strSheetName = Me.GetGeneratedFileName(udtReport.FileNameFormat.Trim, udtReport.MinusDateForFileName)

            If Not strSheetName.Equals(String.Empty) Then
                'CRE13-016 Upgrade to excel 2007 [Start][Karl]
                'strFilePath = strFilePath & strSheetName & ".xlsx"                
                'CRE13-016 Upgrade to excel 2007 [End][Karl]

                strTemplatePath = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath)

                If strTemplatePath.Substring(strTemplatePath.Length - 1, 1) <> "\" Then
                    'CRE13-016 Upgrade to excel 2007 [Start][Karl]
                    ' strTemplatePath = strTemplatePath & "\Template\" + udtReport.TemplateName.Trim + ".xlsx"
                    strTemplatePath = strTemplatePath & "\Template\" & udtReport.TemplateName.Trim & strFileExt
                    'CRE13-016 Upgrade to excel 2007 [End][Karl]
                Else
                    'CRE13-016 Upgrade to excel 2007 [Start][Karl]
                    'strTemplatePath = strTemplatePath & "Template\" + udtReport.TemplateName.Trim + ".xlsx"
                    strTemplatePath = strTemplatePath & "Template\" & udtReport.TemplateName.Trim & strFileExt
                    'CRE13-016 Upgrade to excel 2007 [End][Karl]
                End If

                'CRE13-016 Upgrade to excel 2007 [Start][Karl]
                Dim udtFileGenerationBLL As New Common.Component.FileGeneration.FileGenerationBLL
                Call udtFileGenerationBLL.ConstructExcelFile(ds, strFilePath, strSheetName & strFileExt, strPassword, strTemplatePath, udtReport.StartRowNo)

                'Export2Excel(ds, strFilePath, strTemplatePath, strSheetName, udtReport.StartRowNo, strPassword)
                'CRE13-016 Upgrade to excel 2007 [End][Karl]
            End If


        Catch ex As Exception
            Throw ex
        End Try
        Return strSheetName & strFileExt
    End Function
    'CRE13-016 Upgrade to excel 2007 [Start][Karl]
    'Public Function GetAdhocReportData(ByVal strDBFlag As String, ByVal strStoredProcedureName As String, ByVal strExcelTemplateName As String, ByVal intCommandTimeout As Integer) As String
    '    ' Retrieve data from database
    '    Dim ds As DataSet = GetDataFromDB(strStoredProcedureName, strDBFlag, intCommandTimeout)

    '    ' Remove the .xls files in the output directory
    '    Dim strOutputDirectoryPath As String = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath)
    '    If Not strOutputDirectoryPath.EndsWith("\") Then strOutputDirectoryPath += "\"
    '    strOutputDirectoryPath += "GeneratedXLS\"

    '    RemoveFiles(strOutputDirectoryPath)

    '    ' Prepare output file path
    '    Dim strOutputFilePath As String = strOutputDirectoryPath + Strings.Replace(strExcelTemplateName, "_Template.xls", ".xls", Compare:=CompareMethod.Text)

    '    ' Prepare template file path
    '    Dim strTemplateFilePath As String = HttpContext.Current.Server.MapPath(HttpContext.Current.Request.ApplicationPath)
    '    If Not strTemplateFilePath.EndsWith("\") Then strTemplateFilePath += "\"
    '    strTemplateFilePath += "Template\Adhoc\"
    '    strTemplateFilePath += strExcelTemplateName

    '    ' Prepare the start row (dummy: 1, 1, 1, 1, ...)
    '    Dim aryStartRow(100) As Integer
    '    For i As Integer = 0 To aryStartRow.Length - 1
    '        aryStartRow(i) = 1
    '    Next

    '    Export2Excel(ds, strOutputFilePath, strTemplateFilePath, "tmk791: This variable is not used", aryStartRow, String.Empty)

    '    Return strOutputFilePath

    'End Function
    'CRE13-016 Upgrade to excel 2007 [End][Karl]

    'Export the datatable to excel with the standard template
    Private Sub Export2Excel(ByVal dtFormData As DataTable, ByVal generateFileName As String, ByVal templateName As String, ByVal sheetName As String, ByVal intStartRow As Integer)

        Dim thisThread As System.Threading.Thread = System.Threading.Thread.CurrentThread
        Dim originalCulture As System.Globalization.CultureInfo = thisThread.CurrentCulture

        Try
            thisThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

            Dim oExcel As New Excel.Application
            Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
            Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
            Dim oCells As Excel.Range

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
                    oSheet = CType(oSheets.Item(1), Excel.Worksheet)

                    oSheet.Name = "eHS_Utitlization_Summary" 'sheetName

                    'Fill in the data
                    oCells = DataTable2Excel(dtFormData, oSheet, intStartRow)

                    'Save in a temporary file
                    oSheet.SaveAs(filePath)


                    oBook.Close()

                    'Quit Excel and thoroughly deallocate everything
                    oExcel.Quit()

                    ReleaseComObject(oCells) : ReleaseComObject(oSheet)
                    ReleaseComObject(oSheets) : ReleaseComObject(oBook)
                    ReleaseComObject(oBooks) : ReleaseComObject(oExcel)

                    oExcel = Nothing : oBooks = Nothing : oBook = Nothing
                    oSheets = Nothing : oSheet = Nothing : oCells = Nothing

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

    'Export the dataset to excel with the standard template
    Private Sub Export2Excel(ByVal dsFormData As DataSet, ByVal generateFileName As String, ByVal templateName As String, ByVal sheetName As String, ByVal intStartRow As Integer)
        Dim thisThread As System.Threading.Thread = System.Threading.Thread.CurrentThread
        Dim originalCulture As System.Globalization.CultureInfo = thisThread.CurrentCulture

        Try
            thisThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

            Dim oExcel As New Excel.Application
            Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
            Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
            Dim oCells As Excel.Range

            Dim filePath As String
            Dim templatePath As String

            filePath = generateFileName
            templatePath = templateName
            Dim i As Integer = 1

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


                    For Each dtFormData As DataTable In dsFormData.Tables
                        oSheet = CType(oSheets.Item(i), Excel.Worksheet)

                        'oSheet.Name = "eHS_Utitlization_Summary" & i.ToString 'sheetName

                        'Fill in the data
                        oCells = DataTable2Excel(dtFormData, oSheet, intStartRow)

                        'Save in a temporary file
                        oSheet.SaveAs(filePath)

                        i = i + 1
                    Next

                    oBook.Close()

                    'Quit Excel and thoroughly deallocate everything
                    oExcel.Quit()

                    ReleaseComObject(oCells) : ReleaseComObject(oSheet)
                    ReleaseComObject(oSheets) : ReleaseComObject(oBook)
                    ReleaseComObject(oBooks) : ReleaseComObject(oExcel)

                    oExcel = Nothing : oBooks = Nothing : oBook = Nothing
                    oSheets = Nothing : oSheet = Nothing : oCells = Nothing

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

    'Export the dataset to excel with the standard template
    Private Sub Export2Excel(ByVal dsFormData As DataSet, ByVal generateFileName As String, ByVal templateName As String, ByVal sheetName As String, ByVal intStartRow() As Integer, ByVal strPassword As String)
        Dim thisThread As System.Threading.Thread = System.Threading.Thread.CurrentThread
        Dim originalCulture As System.Globalization.CultureInfo = thisThread.CurrentCulture

        Try
            thisThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

            Dim oExcel As New Excel.Application
            Dim oBooks As Excel.Workbooks, oBook As Excel.Workbook
            Dim oSheets As Excel.Sheets, oSheet As Excel.Worksheet
            Dim oCells As Excel.Range

            Dim filePath As String
            Dim templatePath As String

            filePath = generateFileName
            templatePath = templateName
            Dim i As Integer = 1

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


                    For Each dtFormData As DataTable In dsFormData.Tables
                        oSheet = CType(oSheets.Item(i), Excel.Worksheet)

                        'oSheet.Name = "eHS_Utitlization_Summary" & i.ToString 'sheetName

                        'Fill in the data
                        If i > intStartRow.Length Then
                            oCells = DataTable2Excel(dtFormData, oSheet, 5)
                        Else
                            oCells = DataTable2Excel(dtFormData, oSheet, intStartRow(i - 1))
                        End If


                        'Save in a temporary file
                        'oSheet.SaveAs(filePath)

                        i = i + 1
                    Next

                    If Not strPassword.Equals(String.Empty) Then
                        oBook.Password = strPassword
                        oBook.SaveAs(filePath, Password:=strPassword)
                    Else
                        oBook.SaveAs(filePath)
                    End If

                    oBook.Close()

                    'Quit Excel and thoroughly deallocate everything
                    oExcel.Quit()

                    ReleaseComObject(oCells) : ReleaseComObject(oSheet)
                    ReleaseComObject(oSheets) : ReleaseComObject(oBook)
                    ReleaseComObject(oBooks) : ReleaseComObject(oExcel)

                    oExcel = Nothing : oBooks = Nothing : oBook = Nothing
                    oSheets = Nothing : oSheet = Nothing : oCells = Nothing

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

    Public Function GetUtilizationStat() As DataTable
        Dim dt As DataTable = Nothing
        Try
            dt = New DataTable
            Dim udtDB As New Database
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@no_of_days", SqlDbType.Int, 4, DBNull.Value), _
                                           udtDB.MakeInParam("@end_date", SqlDbType.VarChar, 10, DBNull.Value)}

            udtDB.RunProc("proc_EHS_Utilization_Stat", prams, dt)

            Return dt
        Catch ex As Exception
            Throw ex
        End Try
        Return dt

    End Function

    Public Function GetMOStat() As DataSet
        Dim ds As DataSet = Nothing
        Try
            ds = New DataSet
            Dim udtDB As New Database
            'Dim prams() As SqlParameter = {udtDB.MakeInParam("@no_of_days", SqlDbType.Int, 4, DBNull.Value), _
            '                               udtDB.MakeInParam("@end_date", SqlDbType.VarChar, 10, DBNull.Value)}

            udtDB.RunProc("proc_EHS_MO_Stat", ds)

            Return ds
        Catch ex As Exception
            Throw ex
        End Try
        Return ds

    End Function

    Public Function GetEnrolledPracticeStat() As DataSet
        Dim ds As DataSet = Nothing
        Try
            ds = New DataSet
            Dim udtDB As New Database
            'Dim prams() As SqlParameter = {udtDB.MakeInParam("@no_of_days", SqlDbType.Int, 4, DBNull.Value), _
            '                               udtDB.MakeInParam("@end_date", SqlDbType.VarChar, 10, DBNull.Value)}

            udtDB.RunProc("proc_EHS_EnrolledPractice_Stat", ds)

            Return ds
        Catch ex As Exception
            Throw ex
        End Try
        Return ds

    End Function

    'Generate the file Name with standard format
    Private Function GenerateFileName(ByVal strType As String) As String
        Dim fileName As String = String.Empty
        Select Case strType
            Case "Enrol"
                fileName = "eHS_Utitlization_Summary(" & Now.AddDays(-1).ToString("ddMMyyyy") & ")"
            Case "MO"
                fileName = "eHS_MO_Summary(" & Now.ToString("ddMMyyyy") & ")"
            Case "EnrolledPractice"
                fileName = "eHS_EnrolledPracitce_Summary(" & Now.ToString("ddMMyyyy") & ")"
        End Select

        Return fileName

    End Function

    Public Function GetDataFromDB(ByVal strExecSP As String, ByVal strDBFlag As String, Optional ByVal intCommandTimeout As Integer = 0) As DataSet
        Dim udtDB As New Database(strDBFlag)
        Dim ds As New DataSet

        If strDBFlag.ToUpper().Trim().Equals("DBFlag2".ToUpper()) Then
            Dim intDBCommandTimeout As Integer
            intDBCommandTimeout = CInt(System.Configuration.ConfigurationManager.AppSettings("DBReplication_CommandTimeout"))
            udtDB.CommandTimeout = intDBCommandTimeout
        End If

        If intCommandTimeout <> 0 Then udtDB.CommandTimeout = intCommandTimeout

        udtDB.RunProc(strExecSP, ds)

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

    Private Sub RemoveFiles(ByVal strPath As String)
        Dim di As System.IO.DirectoryInfo = New System.IO.DirectoryInfo(strPath)
        Dim fiArr As FileInfo() = di.GetFiles
        'CRE13-016 Upgrade to excel 2007 [End][Karl]
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
