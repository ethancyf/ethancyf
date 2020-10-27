Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices.Marshal
Imports System.IO

Imports Common.ComFunction
Imports Common.Component

''' <summary>
''' Excel Builder, The Class to Physically Create the Excel File
''' </summary>
''' <remarks></remarks>
Public Class ExcelBuilder

    Private Shared _excelBuilder As ExcelBuilder

    Private Const _generalInteger = "#"
    Private Const _generalNumber = "#,##0.00_);(#,##0.00)"
    Private Const _date = "d mmm yyyy h:mm"

    ' INT12-003 ExcelGenerator tamplate report date format incorrect [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    Private Const _excelTemplateDate = "dd MMM yyyy HH:mm:ss"
    ' INT12-003 ExcelGenerator tamplate report date format incorrect [End][Koala]

    ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
    ' -----------------------------------------------------------------------------------------
    Private Const BlankTemplateFileName As String = "Blank_Template"
    ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]

    ' CRE13-016 - Upgrade to excel 2007 [Start][Tommy L]
    ' -----------------------------------------------------------------------------------------
    Private _intExcelWorkSheetMaxRow As Integer
    ' CRE13-016 - Upgrade to excel 2007 [End][Tommy L]

    ' CRE20-003-02 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
    Public Class WorksheetAction
        Public Const Add As String = "A"
        Public Const Delete As String = "D"
        Public Const Rename As String = "R"
    End Class
    ' CRE20-003-02 Enhancement on Programme or Scheme using batch upload [End][Winnie]

#Region "Constructor"

    Private Sub New()
        ' CRE13-016 - Upgrade to excel 2007 [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------

        Dim udtFileGenerationBLL As New Common.Component.FileGeneration.FileGenerationBLL

        _intExcelWorkSheetMaxRow = udtFileGenerationBLL.GetExcelWorkSheetMaxRow()

        ' CRE13-016 - Upgrade to excel 2007 [End][Tommy L]
    End Sub

    Public Shared Function GetInstance() As ExcelBuilder
        If _excelBuilder Is Nothing Then _excelBuilder = New ExcelBuilder()
        Return _excelBuilder
    End Function

#End Region

    ''' <summary>
    ''' Excel Column: Count From A-Z, AA-AZ, BA-BZ....
    ''' </summary>
    ''' <param name="intColumn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetExcelColumnName(ByVal intColumn As Integer) As String

        Dim strReturn As String = ""

        If intColumn <= 26 Then
            Select Case (intColumn)
                Case 0
                    strReturn = "Z"
                Case 1
                    strReturn = "A"
                Case 2
                    strReturn = "B"
                Case 3
                    strReturn = "C"
                Case 4
                    strReturn = "D"
                Case 5
                    strReturn = "E"
                Case 6
                    strReturn = "F"
                Case 7
                    strReturn = "G"
                Case 8
                    strReturn = "H"
                Case 9
                    strReturn = "I"
                Case 10
                    strReturn = "J"
                Case 11
                    strReturn = "K"
                Case 12
                    strReturn = "L"
                Case 13
                    strReturn = "M"
                Case 14
                    strReturn = "N"
                Case 15
                    strReturn = "O"
                Case 16
                    strReturn = "P"
                Case 17
                    strReturn = "Q"
                Case 18
                    strReturn = "R"
                Case 19
                    strReturn = "S"
                Case 20
                    strReturn = "T"
                Case 21
                    strReturn = "U"
                Case 22
                    strReturn = "V"
                Case 23
                    strReturn = "W"
                Case 24
                    strReturn = "X"
                Case 25
                    strReturn = "Y"
                Case 26
                    strReturn = "Z"
            End Select
        Else
            ' > 26
            strReturn += Me.GetExcelColumnName((intColumn - 1) \ 26) + Me.GetExcelColumnName((intColumn Mod 26))
        End If

        Return strReturn
    End Function

    ''' <summary>
    ''' Create the Physical File and encrypted with password
    ''' </summary>
    ''' <param name="dsData"></param>
    ''' <param name="strFilePath"></param>
    ''' <param name="strFileName"></param>
    ''' <param name="strPassword"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConstructExcelFile(ByVal dsData As DataSet, ByVal strFilePath As String, ByVal strFileName As String, ByVal strPassword As String) As Boolean

        ' CRE16-020 - Excel Upgrade 2007 to 2013 [Start][Marco]
        'Dim xlsApp As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.ApplicationClass()
        Dim xlsApp As Microsoft.Office.Interop.Excel.Application = New Microsoft.Office.Interop.Excel.Application()
        ' CRE16-020 - Excel Upgrade 2007 to 2013 [End][Marco]

        ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Dim xlsWorkBooks As Microsoft.Office.Interop.Excel.Workbooks = Nothing
        ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
        Dim xlsWorkBook As Microsoft.Office.Interop.Excel.Workbook = Nothing
        Dim xlsWorkSheet As Microsoft.Office.Interop.Excel.Worksheet = Nothing

        ' http://support.microsoft.com/kb/320369
        Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        Try
            xlsApp.DisplayAlerts = False
            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]
            ' Enhance performance for Excel 2013
            xlsApp.ScreenUpdating = False
            ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]

            ' Create a Work Book (Excel File)

            'Dim ci As System.Globalization.CultureInfo = New System.Globalization.CultureInfo("en-US")
            'Dim xlsBooks As Object = xlsApp.Workbooks
            'xlsWorkBook = xlsBooks.GetType().InvokeMember("Add", Reflection.BindingFlags.InvokeMethod, Nothing, xlsBooks, Nothing, ci)

            ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'xlsWorkBook = xlsApp.Workbooks.Add()
            xlsApp.Visible = False

            If File.Exists(strFilePath & strFileName) Then
                Return True
            End If

            Dim fileBlankTemplate As New FileInfo(GetBlankTemplate(strFileName))
            If Not fileBlankTemplate.Exists() Then
                Throw New Exception("Blank Template Not Found!" + fileBlankTemplate.FullName)
            End If

            If Not System.IO.Directory.Exists(strFilePath) Then
                System.IO.Directory.CreateDirectory(strFilePath)
            End If

            Dim fileCopy As FileInfo = fileBlankTemplate.CopyTo(strFilePath & strFileName)
            If Not fileCopy.Exists Then
                Throw New Exception("Blank Template Copy Fail!" + fileCopy.FullName + "," + fileBlankTemplate.FullName)
            End If

            xlsWorkBooks = xlsApp.Workbooks
            xlsWorkBooks.Open(strFilePath & strFileName)
            xlsWorkBook = xlsWorkBooks.Item(1)
            ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]

            ' Get The Work Book's Work Sheets Start From Index 1
            Dim intWorkSheetCounter As Integer = 0
            For Each dtData As DataTable In dsData.Tables

                intWorkSheetCounter = intWorkSheetCounter + 1

                If intWorkSheetCounter > 3 Then
                    'xlsWorkSheet = xlsWorkBook.Worksheets(intWorkSheetCounter)
                    xlsWorkSheet = xlsWorkBook.Worksheets.Add

                Else
                    xlsWorkSheet = xlsWorkBook.Worksheets(intWorkSheetCounter)
                End If

                'xlsWorkSheet.Name = dtData.TableName

                If Not dtData.TableName = "" AndAlso Not dtData.TableName.ToUpper().IndexOf("TABLE") >= 0 Then
                    xlsWorkSheet.Name = dtData.TableName
                End If

                ' Fill Up The Header
                Dim xlsRange As Microsoft.Office.Interop.Excel.Range = xlsWorkSheet.Range("A1", Me.GetExcelColumnName(dtData.Columns.Count) + "1")
                Dim arrStrHeader(dtData.Columns.Count) As String
                For i As Integer = 0 To dtData.Columns.Count - 1
                    arrStrHeader(i) = dtData.Columns(i).ColumnName
                Next
                xlsRange.Value = arrStrHeader

                ' Handle Record Limit: 65536 (header)
                Dim intRecordCount As Integer = 0
                Dim intContinute_Page As Integer = 0

                ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
                ' -----------------------------------------------------------------------------------------
                Dim ary As Object

                xlsRange = xlsWorkSheet.Cells
                ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]

                ' Fill Up Content, Record By Record
                For i As Integer = 0 To dtData.Rows.Count - 1

                    ' CRE13-016 - Upgrade to excel 2007 [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'If intRecordCount >= 65500 Then
                    If intRecordCount >= _intExcelWorkSheetMaxRow Then
                        ' CRE13-016 - Upgrade to excel 2007 [End][Tommy L]
                        ' New Sheet !!!

                        intContinute_Page = intContinute_Page + 1
                        intWorkSheetCounter = intWorkSheetCounter + 1

                        If intWorkSheetCounter > 3 Then
                            xlsWorkSheet = xlsWorkBook.Worksheets.Add()
                        Else
                            xlsWorkSheet = xlsWorkBook.Worksheets(intWorkSheetCounter)
                        End If

                        'xlsWorkSheet.Name = dtData.TableName
                        If Not dtData.TableName = "" AndAlso Not dtData.TableName.ToUpper().IndexOf("TABLE") >= 0 Then
                            xlsWorkSheet.Name = dtData.TableName + "_" + (intContinute_Page + 1).ToString()
                        End If

                        ' Fill Up The Header
                        Dim xlsRange_Sub As Microsoft.Office.Interop.Excel.Range = xlsWorkSheet.Range("A1", Me.GetExcelColumnName(dtData.Columns.Count) + "1")
                        Dim arrStrHeader_Sub(dtData.Columns.Count) As String
                        For i_Sub As Integer = 0 To dtData.Columns.Count - 1
                            arrStrHeader_Sub(i_Sub) = dtData.Columns(i_Sub).ColumnName
                        Next
                        xlsRange_Sub.Value = arrStrHeader

                        intRecordCount = 0

                        ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
                        ' -----------------------------------------------------------------------------------------
                        xlsRange = xlsWorkSheet.Cells
                        ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
                    End If

                    For j As Integer = 0 To dtData.Rows(i).ItemArray.Length - 1
                        ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
                        ' -----------------------------------------------------------------------------------------
                        ary = dtData.Rows.Item(i).ItemArray
                        If TypeOf ary(j) Is DateTime Then
                            xlsRange(intRecordCount + 2, j + 1) = CType(ary(j), DateTime).ToString(_excelTemplateDate)
                        Else
                            xlsRange(intRecordCount + 2, j + 1) = ary(j).ToString()
                        End If

                        'xlsRange = xlsWorkSheet.Range(Me.GetExcelColumnName(j + 1) + (intRecordCount + 2).ToString(), Me.GetExcelColumnName(j + 1) + (intRecordCount + 2).ToString())
                        'xlsRange.Value = dtData.Rows(i).Item(j)

                        'If dtData.Rows(i).Item(j).GetType() Is GetType(DateTime) Then
                        '   xlsRange.NumberFormat = _date
                        'End If
                        ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
                    Next

                    intRecordCount = intRecordCount + 1
                Next

                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkSheet)
                xlsWorkSheet = Nothing
            Next

            'Create The Folder If Not Exist
            ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'If Not System.IO.Directory.Exists(strFilePath) Then
            '   System.IO.Directory.CreateDirectory(strFilePath)
            'End If
            ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]

            ' CRE13-016 - Upgrade to excel 2007 [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'xlsWorkBook.SaveAs(strFilePath + strFileName.Trim(), Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal)
            ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Dim udtGeneralFunction As New GeneralFunction

            'Select Case udtGeneralFunction.GetFileNameExtension(strFileName).ToUpper()
            '   Case DataDownloadFileType.XLSX
            xlsWorkBook.SaveAs(strFilePath + strFileName)
            '   Case DataDownloadFileType.XLS
            '       xlsWorkBook.SaveAs(strFilePath + strFileName.Trim(), Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal)

            '   Case Else
            '       Throw New Exception("Error: Class = [ExcelGenerator.ExcelBuilder], Method = [ConstructExcelFile], Message = The extension of the file name passed into this method is mismatched with the supported MS Excel File Type.")
            'End Select
            ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
            ' CRE13-016 - Upgrade to excel 2007 [End][Tommy L]

            xlsWorkBook.Close(False, Type.Missing, Type.Missing)
            xlsApp.Workbooks.Close()
            xlsApp.Quit()

            ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkBooks)
            xlsWorkBooks = Nothing
            ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkBook)
            xlsWorkBook = Nothing
            System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsApp)
            xlsApp = Nothing

            'Retrieve The Encrypt Password to Encrpty the Excel File
            Common.Encryption.Encrypt.EncryptExcel(strPassword, strFilePath + strFileName.Trim())

            Return True
        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
            'Throw ex
            Return False
        Finally

            ' Reset Locale
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

            ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            If Not xlsWorkBooks Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkBooks)
                xlsWorkBooks = Nothing
            End If
            ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]

            If Not xlsWorkSheet Is Nothing Then
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkSheet)
                xlsWorkSheet = Nothing
            End If

            If Not xlsWorkBook Is Nothing Then
                xlsWorkBook.Close(True, Type.Missing, Type.Missing)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsWorkBook)
                xlsWorkBook = Nothing
            End If

            If Not xlsApp Is Nothing Then
                xlsApp.Workbooks.Close()
                xlsApp.Quit()
                System.Runtime.InteropServices.Marshal.ReleaseComObject(xlsApp)
                xlsApp = Nothing
            End If

            GC.Collect()
            GC.WaitForPendingFinalizers()

        End Try
    End Function

    ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]
    Public Function ConstructExcelFile_StudentFile(ByVal excelGenerator As IExcelGenerable)
        Dim blnSuccess As Boolean = True
        Try

            If excelGenerator.GetTemplate.Equals(String.Empty) Then
                blnSuccess = Me.ConstructExcelFile(excelGenerator.GetDataSet(), excelGenerator.GetFilePath, excelGenerator.GetFileName, excelGenerator.GetEncryptPassword)
            Else
                If IsNothing(excelGenerator.GetXLSParameter) Then
                    Throw New Exception("No XLS parameter")
                Else
                    blnSuccess = Me.ConstructStudentExcelFile(excelGenerator.GetDataSet(), excelGenerator.GetFilePath, excelGenerator.GetFileName, _
                                                       excelGenerator.GetEncryptPassword, excelGenerator.GetTemplate, excelGenerator.GetXLSParameter, _
                                                       excelGenerator.GetWorksheetAction)
                End If
            End If

        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
            Return False
        End Try

        Return blnSuccess
    End Function
    ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]

    ''' <summary>
    ''' [Public] Create the physical File for File
    ''' </summary>
    ''' <param name="excelGenerator"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConstructExcelFile(ByVal excelGenerator As IExcelGenerable)
        Dim blnSuccess As Boolean = True
        Try

            If excelGenerator.GetTemplate.Equals(String.Empty) Then
                blnSuccess = Me.ConstructExcelFile(excelGenerator.GetDataSet(), excelGenerator.GetFilePath, excelGenerator.GetFileName, excelGenerator.GetEncryptPassword)
            Else
                If IsNothing(excelGenerator.GetXLSParameter) Then
                    Throw New Exception("No XLS parameter")
                Else
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Koala]
                    'If StudentFileGenerator.StudentFileGenerateCheck(excelGenerator.GetFileName) Then
                    '    'CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start]	[Marco CHOI]
                    '    blnSuccess = Me.ConstructStudentExcelFile(excelGenerator.GetDataSet(), excelGenerator.GetFilePath, excelGenerator.GetFileName, _
                    '                                       excelGenerator.GetEncryptPassword, excelGenerator.GetTemplate, excelGenerator.GetXLSParameter, _
                    '                                       excelGenerator.GetWorksheetAction)
                    '    'CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End]	[Marco CHOI]
                    'Else
                    blnSuccess = Me.ConstructExcelFile(excelGenerator.GetDataSet(), excelGenerator.GetFilePath, excelGenerator.GetFileName, _
                                                       excelGenerator.GetEncryptPassword, excelGenerator.GetTemplate, excelGenerator.GetXLSParameter, _
                                                       excelGenerator.GetWorksheetAction)
                    'End If
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Koala]
                End If
            End If

        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
            Return False
        End Try

        Return blnSuccess
    End Function

    'Export the dataset to excel with the standard template
    Private Function ConstructExcelFile(ByVal dsData As DataSet, ByVal strFilePath As String, ByVal strFileName As String, ByVal strPassword As String, _
                                        ByVal strTemplate As String, ByVal intStartRow As List(Of Integer), Optional dtWorksheetAction As DataTable = Nothing) As Boolean
        Dim thisThread As System.Threading.Thread = System.Threading.Thread.CurrentThread
        Dim originalCulture As System.Globalization.CultureInfo = thisThread.CurrentCulture

        Try
            thisThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

            Dim oExcel As New Excel.Application
            Dim oBooks As Excel.Workbooks = Nothing
            Dim oBook As Excel.Workbook = Nothing
            Dim oSheets As Excel.Sheets = Nothing
            Dim oSheet As Excel.Worksheet = Nothing
            Dim oCells As Excel.Range = Nothing

            Dim filePath As String
            Dim templatePath As String

            filePath = strFilePath
            filePath = filePath & strFileName

            templatePath = strTemplate
            Dim i As Integer = 1
            Dim iCurrentSheet As Integer = 1

            'copy the template file to a new excel file

            If Not File.Exists(filePath) Then
                Dim fileOpen As New FileInfo(templatePath)
                If fileOpen.Exists() Then
                    Dim fileSave As FileInfo = fileOpen.CopyTo(filePath)

                    If Not fileSave.Exists Then
                        Throw New Exception("File Copy Fail!" + fileSave.FullName + "," + fileOpen.FullName)
                    End If

                    oExcel.Visible = False : oExcel.DisplayAlerts = False
                    ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [Start][Koala]
                    ' Enhance performance for Excel 2013
                    oExcel.ScreenUpdating = False
                    ' CRE17-018-04 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 4 - Claim [End][Koala]

                    oBooks = oExcel.Workbooks
                    oBooks.Open(filePath)
                    oBook = oBooks.Item(1)

                    oSheets = oBook.Worksheets


                    For Each dtFormData As DataTable In dsData.Tables

                        oSheet = CType(oSheets.Item(iCurrentSheet), Excel.Worksheet)

                        oCells = DataTable2Excel(dtFormData, oSheet, intStartRow(i - 1), oSheets, iCurrentSheet)


                        'Fill in the data
                        'If i > intStartRow.Length Then
                        '    oCells = DataTable2Excel(dtFormData, oSheet, 5)
                        'Else
                        '    oCells = DataTable2Excel(dtFormData, oSheet, intStartRow(i - 1))
                        'End If

                        i = i + 1
                    Next

                    ' Worksheet Action
                    If Not IsNothing(dtWorksheetAction) Then
                        ' Sort the Sheet by descending order first
                        Dim dv As DataView = dtWorksheetAction.DefaultView
                        dv.Sort = "Sheet DESC"
                        dtWorksheetAction = dv.ToTable

                        For Each dr As DataRow In dtWorksheetAction.Rows
                            Select Case dr("Action")
                                Case "D"
                                    DirectCast(oSheets(dr("Sheet")), Excel.Worksheet).Delete()

                                Case "R"
                                    DirectCast(oSheets(dr("Sheet")), Excel.Worksheet).Name = dr("Action_Content")

                            End Select

                        Next

                    End If

                    'oSheet.SaveAs(filePath)
                    oBook.SaveAs(filePath)

                    oBook.Close()

                    'Quit Excel and thoroughly deallocate everything
                    oExcel.Quit()

                    If Not IsNothing(oCells) Then ReleaseComObject(oCells)
                    If Not IsNothing(oSheet) Then ReleaseComObject(oSheet)
                    If Not IsNothing(oSheets) Then ReleaseComObject(oSheets)
                    If Not IsNothing(oBook) Then ReleaseComObject(oBook)
                    If Not IsNothing(oBooks) Then ReleaseComObject(oBooks)
                    If Not IsNothing(oExcel) Then ReleaseComObject(oExcel)

                    oExcel = Nothing : oBooks = Nothing : oBook = Nothing
                    oSheets = Nothing : oSheet = Nothing : oCells = Nothing

                    System.GC.Collect()

                Else
                    Throw New Exception("Template Not Found!" + fileOpen.FullName)
                End If
            End If

            'Dim Copyfile As New System.IO.FileInfo(filePath)
            Return True
        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
            'Throw ex
            Return False
        Finally
            thisThread.CurrentCulture = originalCulture

        End Try
    End Function

    'CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start]	[Marco CHOI]
    'Export the dataset to excel with student template
    Private Function ConstructStudentExcelFile(ByVal dsData As DataSet, ByVal strFilePath As String, ByVal strFileName As String, ByVal strPassword As String, _
                                        ByVal strTemplate As String, ByVal intStartRow As List(Of Integer), Optional dtWorksheetAction As DataTable = Nothing) As Boolean
        Dim thisThread As System.Threading.Thread = System.Threading.Thread.CurrentThread
        Dim originalCulture As System.Globalization.CultureInfo = thisThread.CurrentCulture

        Try
            thisThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

            Dim oExcel As New Excel.Application
            Dim oBooks As Excel.Workbooks = Nothing
            Dim oBook As Excel.Workbook = Nothing
            Dim oSheets As Excel.Sheets = Nothing
            Dim oSheet As Excel.Worksheet = Nothing
            Dim oCells As Excel.Range = Nothing

            Dim filePath As String
            Dim templatePath As String

            filePath = strFilePath
            filePath = filePath & strFileName

            templatePath = strTemplate
            Dim i As Integer = 1
            Dim iCurrentSheet As Integer = 1

            'copy the template file to a new excel file

            If Not File.Exists(filePath) Then
                Dim fileOpen As New FileInfo(templatePath)
                If fileOpen.Exists() Then
                    Dim fileSave As FileInfo = fileOpen.CopyTo(filePath)

                    If Not fileSave.Exists Then
                        Throw New Exception("File Copy Fail!" + fileSave.FullName + "," + fileOpen.FullName)
                    End If

                    oExcel.Visible = False : oExcel.DisplayAlerts = False
                    oExcel.ScreenUpdating = False

                    oBooks = oExcel.Workbooks
                    oBooks.Open(filePath)
                    oBook = oBooks.Item(1)

                    oSheets = oBook.Worksheets

                    'XLSParmeter
                    ' ==== For VF0001 ====
                    ' 1st sheet: Batch
                    ' 2nd sheet: Follow Up Client
                    ' 3rd sheet: DynamicGenSheet
                    ' 4th sheet: Remark
                    ' 5th sheet: Change History
                    ' 6th sheet: (Hidden)

                    ' ==== For Others ====
                    ' 1st sheet: Batch
                    ' 2nd sheet: DynamicGenSheet
                    ' 3rd sheet: Remark
                    ' 4th sheet: Change History
                    ' 5th sheet: (Hidden)

                    Dim intDynamicGenSheet As Integer = 0

                    Dim lstdtDynamicGenData As List(Of DataTable)
                    lstdtDynamicGenData = New List(Of DataTable)

                    Dim xlsContentSheet As Excel.Worksheet = Nothing
                    Dim xlsDynamicGenSheet As Excel.Worksheet = Nothing

                    ' CRE20-003-02 Enhancement on Programme or Scheme using batch upload [Start][Winnie]
                    ' -------------------------------------------------------------------------------
                    ' Handle Hidden worksheet
                    Dim intVisibleSheetCount As Integer = 0
                    Dim intDatatable As Integer = 0

                    For i = 1 To oSheets.Count
                        If oSheets(i).Visible <> Excel.XlSheetVisibility.xlSheetVisible Then Continue For
                        intVisibleSheetCount += 1
                        
                        'First sheet
                        If intVisibleSheetCount = 1 Then xlsContentSheet = oSheets.Item(i)

                        'DynamicGenSheet
                        Dim blnIsDynamicSheet As Boolean = False

                        If Not IsNothing(dtWorksheetAction) Then

                            For Each dr As DataRow In dtWorksheetAction.Rows
                                If intVisibleSheetCount = dr("Sheet") AndAlso dr("Action") = WorksheetAction.Add Then
                                    blnIsDynamicSheet = True

                                    xlsDynamicGenSheet = oSheets.Item(i)
                                    intDynamicGenSheet = intVisibleSheetCount

                                    Dim intNoOfDynamicSheet = CInt(dr("Action_Content"))

                                    If intNoOfDynamicSheet > 0 Then

                                        For k As Integer = 1 To intNoOfDynamicSheet
                                            Dim dtFormData As DataTable = dsData.Tables(intDatatable).Copy
                                            lstdtDynamicGenData.Add(dtFormData)
                                            intDatatable += 1
                                        Next

                                    End If

                                    Exit For
                                End If
                            Next
                        End If

                        ' Normal Worksheet Bind Data
                        If blnIsDynamicSheet = False Then
                            Dim dtFormData As DataTable = dsData.Tables(intDatatable).Copy

                            oSheet = CType(oSheets.Item(i), Excel.Worksheet)
                            oCells = DataTable2Excel(dtFormData, oSheet, intStartRow(intVisibleSheetCount - 1), oSheets, iCurrentSheet)

                            intDatatable += 1
                        End If
                        ' CRE20-003-02 Enhancement on Programme or Scheme using batch upload [End][Winnie]
                    Next

                    ' CRE19-001 (VSS 2019) [Start][Winnie]
                    ' -------------------------------------------------------------------------------
                    'DynamicGenSheet Bind Data
                    Dim iCurrentDynamicGenSheet As Integer = xlsDynamicGenSheet.Index
                    For Each dt As DataTable In lstdtDynamicGenData
                        xlsDynamicGenSheet.Copy(After:=oBook.Worksheets(iCurrentDynamicGenSheet - 1))
                        oSheet = Nothing
                        oSheet = oSheets.Item(iCurrentDynamicGenSheet)
                        oSheet.Name = dt.Rows(0)("Class_Name")
                        oCells = DataTable2Excel(dt, oSheet, intStartRow(intDynamicGenSheet - 1), oSheets, iCurrentSheet)

                        iCurrentDynamicGenSheet += 1
                    Next
                    xlsDynamicGenSheet.Delete()
                    ' CRE19-001 (VSS 2019) [End][Winnie]


                    'Re-focus to first sheet
                    oSheet = CType(xlsContentSheet, Excel.Worksheet)
                    oSheet.Select()

                    ' Worksheet Action
                    If Not IsNothing(dtWorksheetAction) Then
                        ' Sort the Sheet by descending order first
                        Dim dv As DataView = dtWorksheetAction.DefaultView
                        dv.Sort = "Sheet DESC"
                        dtWorksheetAction = dv.ToTable

                        For Each dr As DataRow In dtWorksheetAction.Rows
                            Select Case dr("Action")
                                Case WorksheetAction.Delete
                                    DirectCast(oSheets(dr("Sheet")), Excel.Worksheet).Delete()

                                Case WorksheetAction.Rename
                                    DirectCast(oSheets(dr("Sheet")), Excel.Worksheet).Name = dr("Action_Content")

                            End Select

                        Next

                    End If

                    'oSheet.SaveAs(filePath)
                    oBook.SaveAs(filePath)

                    oBook.Close()

                    'Quit Excel and thoroughly deallocate everything
                    oExcel.Quit()

                    If Not IsNothing(oCells) Then ReleaseComObject(oCells)
                    If Not IsNothing(oSheet) Then ReleaseComObject(oSheet)
                    If Not IsNothing(oSheets) Then ReleaseComObject(oSheets)
                    If Not IsNothing(oBook) Then ReleaseComObject(oBook)
                    If Not IsNothing(oBooks) Then ReleaseComObject(oBooks)
                    If Not IsNothing(oExcel) Then ReleaseComObject(oExcel)

                    oExcel = Nothing : oBooks = Nothing : oBook = Nothing
                    oSheets = Nothing : oSheet = Nothing : oCells = Nothing

                    System.GC.Collect()

                Else
                    Throw New Exception("Template Not Found!" + fileOpen.FullName)
                End If
            End If

            'Dim Copyfile As New System.IO.FileInfo(filePath)
            Return True
        Catch ex As Exception
            GeneratorLogger.LogLine(ex.ToString())
            GeneratorLogger.ErrorLog(ex)
            'Throw ex
            Return False
        Finally
            thisThread.CurrentCulture = originalCulture

        End Try
    End Function
    'CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End]	[Marco CHOI]

    'Outputs a DataTable to an Excel Worksheet
    Private Function DataTable2Excel(ByVal dt As DataTable, ByRef oSheet As Excel.Worksheet, ByVal intStartRow As Integer, ByRef oSheets As Excel.Sheets, ByRef iCurrentSheet As Integer) As Excel.Range
        Dim oCells As Excel.Range
        oCells = oSheet.Cells

        Dim dr As DataRow, ary() As Object
        Dim iRow As Integer, iCol As Integer
        ' CRE13-016 - Upgrade to excel 2007 [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Dim iBreakAtRow As Integer = 65500
        Dim iBreakAtRow As Integer = _intExcelWorkSheetMaxRow
        ' CRE13-016 - Upgrade to excel 2007 [End][Tommy L]

        Dim iSheetAdd As Integer = 0

        If dt.Rows.Count > iBreakAtRow Then
            iSheetAdd = (dt.Rows.Count \ iBreakAtRow)
        End If

        ' Add Sheet
        Dim iSheet As Integer = 0
        Dim strSheetName = oSheet.Name
        If iSheetAdd > 0 Then
            For iSheet = 1 To iSheetAdd
                oSheet.Copy(oSheet)
            Next
        End If

        'Output Data
        If iSheetAdd > 0 Then
            Dim iCurrentStartRow As Integer = 0
            Dim iCurrentEndRow As Integer = 0


            For iSheet = 0 To iSheetAdd
                Dim oTempSheet As Excel.Worksheet
                Dim oTempCells As Excel.Range
                iCurrentStartRow = iSheet * iBreakAtRow
                iCurrentEndRow = ((iSheet + 1) * iBreakAtRow) - 1
                oTempSheet = CType(oSheets.Item(iCurrentSheet + iSheet), Excel.Worksheet)
                oTempSheet.Name = strSheetName + "-" + (iSheet + 1).ToString()
                oTempCells = oTempSheet.Cells

                If iCurrentEndRow > dt.Rows.Count Then
                    iCurrentEndRow = dt.Rows.Count - 1
                End If

                Dim iWritingRow As Integer = 0
                iWritingRow = iWritingRow + intStartRow
                For iRow = iCurrentStartRow To iCurrentEndRow
                    dr = dt.Rows.Item(iRow)
                    ary = dr.ItemArray
                    For iCol = 0 To UBound(ary)
                        ' INT12-003 ExcelGenerator tamplate report date format incorrect [Start][Koala]
                        ' -----------------------------------------------------------------------------------------
                        If TypeOf ary(iCol) Is DateTime Then
                            oTempCells(iWritingRow, iCol + 1) = CType(ary(iCol), DateTime).ToString(_excelTemplateDate)
                        Else
                            oTempCells(iWritingRow, iCol + 1) = ary(iCol).ToString
                        End If
                        ' INT12-003 ExcelGenerator tamplate report date format incorrect [End][Koala]
                    Next
                    iWritingRow = iWritingRow + 1
                Next
            Next
        Else
            For iRow = 0 To dt.Rows.Count - 1
                dr = dt.Rows.Item(iRow)
                ary = dr.ItemArray

                For iCol = 0 To UBound(ary)
                    ' INT12-003 ExcelGenerator tamplate report date format incorrect [Start][Koala]
                    ' -----------------------------------------------------------------------------------------
                    If TypeOf ary(iCol) Is DateTime Then
                        oCells(iRow + intStartRow, iCol + 1) = CType(ary(iCol), DateTime).ToString(_excelTemplateDate)
                    Else
                        oCells(iRow + intStartRow, iCol + 1) = ary(iCol).ToString
                    End If
                    ' INT12-003 ExcelGenerator tamplate report date format incorrect [End][Koala]
                Next
            Next
        End If

        iCurrentSheet = iCurrentSheet + iSheetAdd + 1

        Return oCells
    End Function

    ' CRE13-025 - Improve performance in excel report generation [Start][Tommy L]
    ' -----------------------------------------------------------------------------------------
    Private Function GetBlankTemplate(ByVal strFileName As String) As String
        Dim udtGeneralFunction As New GeneralFunction
        Dim strTemplatePath As String = String.Empty
        Dim strTemplateFileExt As String = udtGeneralFunction.GetFileNameExtension(strFileName)

        udtGeneralFunction.getSystemParameter("ExcelWithTemplatePath", strTemplatePath, String.Empty)

        Return System.AppDomain.CurrentDomain.BaseDirectory + strTemplatePath + BlankTemplateFileName + "." + strTemplateFileExt
    End Function
    ' CRE13-025 - Improve performance in excel report generation [End][Tommy L]
End Class
