Imports System.Configuration
Imports System.IO

Public Class Core

#Region "Constant"

    Private aryDllExtension As ArrayList
    Private aryExeExtension As ArrayList
    Private aryExcludeExtension As ArrayList
    Private dicProjectFileMapping As Dictionary(Of String, ArrayList)
    Private dicStaticFolderMapping As Dictionary(Of String, ArrayList)
    Private dicProjectBinMapping As Dictionary(Of String, ArrayList)
    Private dicProjectFileMappingReverse As Dictionary(Of String, String)
    Private dicLibraryProject As Dictionary(Of String, String)
    Private dicConsoleProject As Dictionary(Of String, String)
    Private Const ConsolidatedFileName As String = "Back-end-Consolidated"
    Private infoReader As System.IO.FileInfo
    Private intFVerify As Integer = 0
    Private intFNotFound As Integer = 0

    Private intFileNotFound As Integer = 0
    Private intSkip As Integer = 0
    Private intError As Integer = 0
    Private intReplace As Integer = 0
    Private intCopied As Integer = 0

#End Region

#Region "Global Variable"

    Private _udtChangeFileList As New ChangeFileModelCollection
    Private _udtChangeStoreProcList As New ChangeFileModelCollection
    Private _udtChangeTriggerList As New ChangeFileModelCollection
    Private _udtChangeScriptList As New ChangeFileModelCollection
    Private _udtBuildFileList As New BuildFileModelCollection

#End Region

#Region "Core Event"

    Private Sub Core_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        SavePreviousSetting()
    End Sub

    Private Sub Core_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Init()

        Dim strMode As String = ConfigurationManager.AppSettings("Mode")

        Me.Text = String.Format("{0} Packager v1.5.0", strMode)
        lblStatus.Text = "Ready"
    End Sub

    Private Sub Init()
        Dim strMode As String = ConfigurationManager.AppSettings("Mode")
        Dim strSuffixList As String = ConfigurationManager.AppSettings("Suffix")

        For Each strSuffix As String In strSuffixList.Split("|")
            Me.cboSuffix.Items.Add(strSuffix)
        Next
        Me.cboSuffix.SelectedIndex = 0 ' Default select first item

        ' Prepare list
        aryDllExtension = New ArrayList

        For Each strLine As String In ConfigurationManager.AppSettings(String.Format("{0}DllExtension", strMode)).Split("|")
            aryDllExtension.Add(strLine.Trim)
        Next

        aryExeExtension = New ArrayList

        For Each strLine As String In ConfigurationManager.AppSettings(String.Format("{0}ExeExtension", strMode)).Split("|")
            aryExeExtension.Add(strLine.Trim)
        Next

        aryExcludeExtension = New ArrayList

        For Each strLine As String In ConfigurationManager.AppSettings(String.Format("{0}ExcludeExtension", strMode)).Split("|")
            aryExcludeExtension.Add(strLine.Trim)
        Next

        ' Prepare mapping
        PrepareMapping(Me.cboSuffix.Items(0))

        ' Load previous setting
        LoadPreviousSetting()
    End Sub

    Private Sub LoadPreviousSetting()
        Me.txtFFileLocation.Text = AppConfigFileSettings.GetAppSettings("LastSourceLocation")
        Me.txtFCopyToLocation.Text = AppConfigFileSettings.GetAppSettings("LastSourceCopyLocation")
        Me.txtBFileLocation.Text = AppConfigFileSettings.GetAppSettings("LastBuiltLocation")
        Me.txtBCopyToLocation.Text = AppConfigFileSettings.GetAppSettings("LastBuiltCopyLocation")
        Me.txtFSL.Text = AppConfigFileSettings.GetAppSettings("LastFSL")
        Me.txtFSPL.Text = AppConfigFileSettings.GetAppSettings("LastFSPL")
    End Sub

    Private Sub SavePreviousSetting()
        AppConfigFileSettings.UpdateAppSettings("LastSourceLocation", Me.txtFFileLocation.Text)
        AppConfigFileSettings.UpdateAppSettings("LastSourceCopyLocation", Me.txtFCopyToLocation.Text)
        AppConfigFileSettings.UpdateAppSettings("LastBuiltLocation", Me.txtBFileLocation.Text)
        AppConfigFileSettings.UpdateAppSettings("LastBuiltCopyLocation", Me.txtBCopyToLocation.Text)
        AppConfigFileSettings.UpdateAppSettings("LastFSL", Me.txtFSL.Text)
        AppConfigFileSettings.UpdateAppSettings("LastFSPL", Me.txtFSPL.Text)
    End Sub

    Private Sub PrepareMapping(ByVal strSuffix As String)
        Dim strMode As String = ConfigurationManager.AppSettings("Mode")

        Dim strProject As String = Nothing
        Dim strPath As String = Nothing

        dicProjectFileMapping = New Dictionary(Of String, ArrayList)

        For Each strLine As String In ConfigurationManager.AppSettings(String.Format("{0}ProjectFileMapping", strMode)).Split(Environment.NewLine)
            If strLine.Trim = String.Empty Then Continue For

            strProject = strLine.Split(":")(0).Trim
            strPath = strLine.Split(":")(1).Trim
            strPath = strPath.Replace("{suffix}", strSuffix)

            If Not dicProjectFileMapping.ContainsKey(strProject) Then dicProjectFileMapping.Add(strProject, New ArrayList)

            dicProjectFileMapping(strProject).Add(strPath)

        Next

        dicStaticFolderMapping = New Dictionary(Of String, ArrayList)

        For Each strLine As String In ConfigurationManager.AppSettings(String.Format("{0}StaticFolderMapping", strMode)).Split(Environment.NewLine)
            If strLine.Trim = String.Empty Then Continue For

            strProject = strLine.Split(":")(0).Trim
            strPath = strLine.Split(":")(1).Trim
            strPath = strPath.Replace("{suffix}", strSuffix)

            If Not dicStaticFolderMapping.ContainsKey(strProject) Then dicStaticFolderMapping.Add(strProject, New ArrayList)

            dicStaticFolderMapping(strProject).Add(strPath)


        Next

        dicProjectBinMapping = New Dictionary(Of String, ArrayList)

        For Each strLine As String In ConfigurationManager.AppSettings(String.Format("{0}ProjectBinMapping", strMode)).Split(Environment.NewLine)
            If strLine.Trim = String.Empty Then Continue For

            strProject = strLine.Split(":")(0).Trim
            strPath = strLine.Split(":")(1).Trim
            strPath = strPath.Replace("{suffix}", strSuffix)

            If Not dicProjectBinMapping.ContainsKey(strProject) Then dicProjectBinMapping.Add(strProject, New ArrayList)

            dicProjectBinMapping(strProject).Add(strPath)

        Next

        dicProjectFileMappingReverse = New Dictionary(Of String, String)

        For Each strProject In dicProjectFileMapping.Keys
            For Each strLocation As String In dicProjectFileMapping(strProject)
                dicProjectFileMappingReverse.Add(strLocation, strProject + "\")
            Next
        Next

        dicLibraryProject = New Dictionary(Of String, String)

        For Each strLine As String In ConfigurationManager.AppSettings("LibraryProject").Split(Environment.NewLine)
            If strLine.Trim = String.Empty Then Continue For

            strProject = strLine.Trim.ToLower
            If Not dicLibraryProject.ContainsKey(strProject) Then dicLibraryProject.Add(strProject, strProject)
        Next

        dicConsoleProject = New Dictionary(Of String, String)

        For Each strLine As String In ConfigurationManager.AppSettings("ConsoleProject").Split(Environment.NewLine)
            If strLine.Trim = String.Empty Then Continue For

            strProject = strLine.Trim.ToLower
            If Not dicConsoleProject.ContainsKey(strProject) Then dicConsoleProject.Add(strProject, strProject)
        Next
    End Sub
#End Region

#Region "Read Change List"

    Private Sub btnRAddList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRAddList.Click
        gvR.AutoSizeRowsMode = _
        DataGridViewAutoSizeRowsMode.AllCells
        ROpenFileDialog.FileName = String.Empty
        ROpenFileDialog.Filter = "Change List File (*.txt)|*.txt"
        ROpenFileDialog.Multiselect = True

        If ROpenFileDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            For Each strFileName As String In ROpenFileDialog.FileNames
                gvR.Rows.Add(strFileName, Nothing, Nothing, "Open")
            Next
        End If

    End Sub

    Private Sub btnRRead_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRRead.Click
        _udtChangeFileList.Clear()
        _udtChangeStoreProcList.Clear()
        _udtChangeTriggerList.Clear()
        _udtChangeScriptList.Clear()

        Me.gvF.DataSource = Nothing
        Me.gvF.Rows.Clear()
        Me.gvF.Columns.Clear()
        Me.gvF.Refresh()
        Me.gvSP.DataSource = Nothing
        Me.gvSP.Rows.Clear()
        Me.gvSP.Columns.Clear()
        Me.gvSP.Refresh()
        Me.gvT.DataSource = Nothing
        Me.gvT.Rows.Clear()
        Me.gvT.Columns.Clear()
        Me.gvT.Refresh()
        Me.gvS.DataSource = Nothing
        Me.gvS.Rows.Clear()
        Me.gvS.Columns.Clear()
        Me.gvS.Refresh()

        Dim reader As StreamReader = Nothing

        For Each dr As DataGridViewRow In gvR.Rows
            Try
                reader = File.OpenText(dr.Cells("RListName").Value)
                Dim strDescription As String = Nothing

                Dim intLine As Integer = ReadChangeFile(reader, Me._udtChangeFileList, "[SourceCode]", strDescription)

                reader.Close()

                reader = File.OpenText(dr.Cells("RListName").Value)
                Dim intLineSP As Integer = ReadChangeFile(reader, Me._udtChangeStoreProcList, "[StoreProcedure]", strDescription)
                reader.Close()

                reader = File.OpenText(dr.Cells("RListName").Value)
                Dim intLineT As Integer = ReadChangeFile(reader, Me._udtChangeTriggerList, "[Trigger]", strDescription)
                reader.Close()

                reader = File.OpenText(dr.Cells("RListName").Value)
                Dim intLineS As Integer = ReadChangeFile(reader, Me._udtChangeScriptList, "[Script]", strDescription)
                reader.Close()

                dr.Cells("RDescription").Value = strDescription

                dr.Cells("RStatus").Value = String.Format("{1} lines read from Front-end{0}{2} lines read from StoreProcedure{0}{3} lines read from Trigger{0}{4} lines read from Script", Environment.NewLine, intLine, intLineSP, intLineT, intLineS)
                dr.Cells("RStatus").ErrorText = String.Empty

                If intLine = 0 Then
                    dr.Cells("RStatus").Style.ForeColor = Color.DarkOrange
                Else
                    dr.Cells("RStatus").Style.ForeColor = Color.Blue
                End If

            Catch ex As Exception
                dr.Cells("RStatus").Value = "Error"
                dr.Cells("RStatus").ErrorText = ex.Message
                dr.Cells("RStatus").Style.ForeColor = Color.Red

                If reader IsNot Nothing Then
                    reader.Close()
                End If
            End Try

        Next
        Dim currentTime As String
        currentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        Dim iChangedFiles As Integer = _udtChangeFileList.Count + _udtChangeStoreProcList.Count + _udtChangeScriptList.Count + _udtChangeTriggerList.Count
        lblStatus.Text = String.Format("{0} distinct changed files read successfully {1}", iChangedFiles, currentTime)
        lblFFileNum.Text = String.Format("No. of distinct changed files: {0}", iChangedFiles)
        lblBFileNum.Text = String.Format("No. of distinct changed files: {0}", iChangedFiles)
        lblBBuildFileNum.Text = "No. of expected built files: (Unchecked)"

    End Sub

    Private Sub btnRClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRClear.Click
        gvR.Rows.Clear()
    End Sub

    Private Sub gvR_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gvR.CellDoubleClick
        If e.ColumnIndex = 0 Then
            Try
                Process.Start("notepad.exe", gvR.Rows(e.RowIndex).Cells(0).Value)
            Catch ex As Exception
                MsgBox("Please DOUBLECLICK the required list")
            End Try
        End If
    End Sub

    ''' <summary>
    ''' </summary>
    ''' <param name="reader"></param>
    ''' <param name="lstChangeFile"></param>
    ''' <param name="strReadSession">[SourceCode], [StoreProcedure]</param>
    ''' <param name="strDescription"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReadChangeFile(ByVal reader As StreamReader, ByVal lstChangeFile As ChangeFileModelCollection, _
                                    ByVal strReadSession As String, ByRef strDescription As String) As Integer
        Dim intLine As Integer = 0
        Dim strLine As String = Nothing
        Dim strCR As String = Nothing
        Dim blnFirstLine As Boolean = True
        Dim blnReadStart As Boolean = False
        Dim udtChangeFile As ChangeFileModel = Nothing

        While reader.Peek <> -1
            strLine = reader.ReadLine.Trim

            If strLine = String.Empty Then Continue While

            ' First line: Read the description
            If blnFirstLine Then
                strDescription = strLine

                ' Get the CR
                strCR = strDescription.Split(" ")(0).Replace("[", String.Empty).Replace("]", String.Empty).Replace(":", String.Empty)

                blnFirstLine = False
                Continue While

            End If

            If blnReadStart = False Then
                If strLine = strReadSession Then
                    blnReadStart = True
                    Continue While
                    'Else
                    '    Exit While
                End If
            Else
                If strLine.StartsWith("[") Then
                    Exit While
                End If
            End If

            If blnReadStart Then
                ' Read change file
                ' Expected format: {FileName} : {Updater1}, {Updater2}, ...
                If strLine.Contains(":") Then
                    udtChangeFile = lstChangeFile.Filter(strLine.Split(":")(0).Trim)
                    If IsNothing(udtChangeFile) Then udtChangeFile = New ChangeFileModel(strLine.Split(":")(0).Trim)
                Else
                    udtChangeFile = lstChangeFile.Filter(strLine.Trim)
                    If IsNothing(udtChangeFile) Then udtChangeFile = New ChangeFileModel(strLine.Trim)
                End If

                udtChangeFile.AryCR.Add(strCR)

                Dim arr
                If strLine.Contains(":") Then
                    arr = strLine.Split(":")(1).Replace("(", String.Empty).Replace(")", String.Empty).Replace(" ", String.Empty).Split(",")
                    For Each strUpdater As String In arr

                        If strUpdater.Trim.ToLower.Equals("new") Then
                            udtChangeFile.NewFile = True
                        Else
                            If Not udtChangeFile.AryUpdater.Contains(strUpdater) Then udtChangeFile.AryUpdater.Add(strUpdater)
                        End If
                    Next
                Else
                    Dim strUpdater As String = "NULL"


                    If Not udtChangeFile.AryUpdater.Contains(strUpdater) Then udtChangeFile.AryUpdater.Add(strUpdater)
                End If

                lstChangeFile.Add(udtChangeFile)

                intLine += 1
            End If


        End While

        Return intLine

    End Function

#End Region

#Region "Change File"

    Private Sub ClearFileError()
        For Each dr As DataGridViewRow In gvF.Rows
            dr.Cells("FStatus").ErrorText = String.Empty
        Next
        For Each dr As DataGridViewRow In gvSP.Rows
            dr.Cells("gvcSPStatus").ErrorText = String.Empty
        Next
        For Each dr As DataGridViewRow In gvS.Rows
            dr.Cells("gvcSStatus").ErrorText = String.Empty
        Next
        For Each dr As DataGridViewRow In gvT.Rows
            dr.Cells("gvcTStatus").ErrorText = String.Empty
        Next
    End Sub

    Private Sub ClearFileStatus()
        For Each f As ChangeFileModel In _udtChangeFileList.Values
            f.Status = String.Empty
        Next
        For Each f As ChangeFileModel In _udtChangeStoreProcList.Values
            f.Status = String.Empty
        Next
        For Each f As ChangeFileModel In _udtChangeScriptList.Values
            f.Status = String.Empty
        Next
        For Each f As ChangeFileModel In _udtChangeTriggerList.Values
            f.Status = String.Empty
        Next
    End Sub

    Private Sub btnFShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFShow.Click
        ClearFileStatus()

        gvFBind()
        gvSPBind()
        gvTBind()
        gvSBind()
        TabPage5.Show()
        TabPage6.Show()

        ClearFileError()
    End Sub

    Private Sub btnFVerify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFVerify.Click
        If txtFFileLocation.Text = String.Empty Then btnFFileLocationBrowse_Click(Nothing, Nothing)
        If txtFSPL.Text = String.Empty Then btnFSPL_Click(Nothing, Nothing)
        If txtFSL.Text = String.Empty Then btnFSL_Click(Nothing, Nothing)

        Dim strFileLocation As String = txtFFileLocation.Text
        Dim strSPLocation As String = txtFSPL.Text
        Dim strSLocation As String = txtFSL.Text

        If strFileLocation = String.Empty Then Return
        If Not strFileLocation.EndsWith("\") Then strFileLocation += "\"
        If strSPLocation = String.Empty Then Return
        If Not strSPLocation.EndsWith("\") Then strSPLocation += "\"
        If strSLocation = String.Empty Then Return
        If Not strSLocation.EndsWith("\") Then strSLocation += "\"

        btnFShow_Click(Nothing, Nothing)

        VerifyChangeList(strFileLocation, gvF, "FLastWriteTime", "FStatus")
        VerifyChangeList(strSPLocation, gvSP, "gvcSPLastWriteTime", "gvcSPStatus")
        VerifyChangeList(strSPLocation, gvT, "gvcTLastWriteTime", "gvcTStatus")
        VerifyChangeList(strSLocation, gvS, "gvcSLastWriteTime", "gvcSStatus")

        Dim iChangedFiles As Integer = _udtChangeFileList.Count + _udtChangeStoreProcList.Count + _udtChangeScriptList.Count + _udtChangeTriggerList.Count
        Dim currentTime As String
        currentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        lblStatus.Text = String.Format("{0} distinct changed files read successfully: {1} verified, {2} not found {3}", iChangedFiles, intFVerify, intFNotFound, currentTime)
        intFVerify = 0
        intFNotFound = 0
    End Sub

    Private Sub btnFVerifySF_Click(sender As System.Object, e As System.EventArgs) Handles btnFVerifySF.Click
        If txtFFileLocation.Text = String.Empty Then btnFFileLocationBrowse_Click(Nothing, Nothing)
        If txtFSPL.Text = String.Empty Then btnFSPL_Click(Nothing, Nothing)
        If txtFSL.Text = String.Empty Then btnFSL_Click(Nothing, Nothing)

        Dim strFileLocation As String = txtFFileLocation.Text
        Dim strSPLocation As String = txtFSPL.Text
        Dim strSLocation As String = txtFSL.Text

        If strFileLocation = String.Empty Then Return
        If Not strFileLocation.EndsWith("\") Then strFileLocation += "\"
        If strSPLocation = String.Empty Then Return
        If Not strSPLocation.EndsWith("\") Then strSPLocation += "\"
        If strSLocation = String.Empty Then Return
        If Not strSLocation.EndsWith("\") Then strSLocation += "\"

        dgvFBind()
        VerifySourceFile(strFileLocation, dgvF, "dgvFStatus", gvF, "FFileName", "FCheck")
        dgvSPBind()
        VerifySourceFile(strSPLocation, dgvSP, "dgvcSPStatus", gvSP, "gvcSPFileName", "gvcSPCheck")
        dgvTBind()
        VerifySourceFile(strSPLocation, dgvT, "dgvcTStatus", gvT, "gvcTFileName", "gvcTCheck")
        dgvSBind()
        VerifySourceFile(strSLocation, dgvS, "dgvcSStatus", gvS, "gvcSFileName", "gvcSCheck")

        intFVerify = 0
        intFNotFound = 0
    End Sub

    Private Sub VerifyChangeList(ByVal strLocation As String, ByVal e As System.Windows.Forms.DataGridView, ByVal LastWriteTime As String, ByVal Status As String)
        For Each dr As DataGridViewRow In e.Rows
            Dim udtChangeFile As ChangeFileModel = CType(dr.DataBoundItem, ChangeFileModel)

            If File.Exists(String.Format("{0}{1}", strLocation, udtChangeFile.FileName)) Then
                infoReader = My.Computer.FileSystem.GetFileInfo(String.Format("{0}{1}", strLocation, udtChangeFile.FileName))
                dr.Cells(LastWriteTime).Value = infoReader.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                dr.Cells(Status).Value = "Verified"
                dr.Cells(Status).Style.ForeColor = Color.Blue
                dr.Cells(Status).ErrorText = String.Empty
                intFVerify += 1
            Else
                dr.Cells(LastWriteTime).Value = "File Not Found"
                dr.Cells(LastWriteTime).Style.ForeColor = Color.Red
                dr.Cells(Status).Value = "File Not Found"
                dr.Cells(Status).Style.ForeColor = Color.Red
                dr.Cells(Status).ErrorText = String.Format("Could not find file '{0}{1}'", strLocation, udtChangeFile.FileName)
                intFNotFound += 1
            End If

        Next

    End Sub

    Private Sub VerifySourceFile(ByVal strLocation As String, ByVal e As System.Windows.Forms.DataGridView, ByVal Status As String, ByVal e2 As System.Windows.Forms.DataGridView, ByVal FileName As String, ByVal Check As String)

        Dim modelCollection As New VerifySourceFileModelCollection

        Dim list As List(Of String) = GetFilesRecursive(strLocation)

        For Each path As String In list
            path = path.Replace(strLocation, "").Trim()
            Dim model As New VerifySourceFileModel
            model.fileName = path
            For Each dr As DataGridViewRow In e2.Rows
                If model.fileName = dr.Cells(FileName).Value Then
                    model.status = "Verified"
                    dr.Cells(Check).Value = True
                    Exit For
                Else
                    model.status = "Not included in List"
                End If
            Next
            modelCollection.Add(model)
        Next

        For Each dr As DataGridViewRow In e2.Rows
            If dr.Cells(Check).Value = False Then
                Dim model2 As New VerifySourceFileModel
                model2.fileName = dr.Cells(FileName).Value
                model2.status = "File Not Found"
                modelCollection.Add(model2)
            End If
        Next

        e.DataSource = New BindingSource(modelCollection.Values, Nothing)

        For Each dr As DataGridViewRow In e.Rows
            If dr.Cells(Status).Value = "Verified" Then
                dr.Cells(Status).Style.ForeColor = Color.Blue
            ElseIf dr.Cells(Status).Value = "File Not Found" Then
                dr.Cells(Status).Style.ForeColor = Color.Red
            Else
                dr.Cells(Status).Style.ForeColor = Color.Green
            End If
        Next

    End Sub

    Private Function GetFilesRecursive(ByVal initial As String) As List(Of String)
        ' This list stores the results.
        Dim result As New List(Of String)

        ' This stack stores the directories to process.
        Dim stack As New Stack(Of String)

        ' Add the initial directory
        stack.Push(initial)

        ' Continue processing for each stacked directory
        Do While (stack.Count > 0)
            ' Get top directory string
            Dim dir As String = stack.Pop
            Try
                ' Add all immediate file paths
                result.AddRange(Directory.GetFiles(dir, "*.*"))

                ' Loop through all subdirectories and add them to the stack.
                Dim directoryName As String
                For Each directoryName In Directory.GetDirectories(dir)
                    stack.Push(directoryName)
                Next

            Catch ex As Exception
            End Try
        Loop
        ' Return the list
        Return result
    End Function

    Private Sub btnFExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFExport.Click
        MessageBox.Show("Congratulation! This function has not been implemented!")
    End Sub

    Private Sub btnFCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFCopy.Click
        If txtFFileLocation.Text = String.Empty Then
            btnFFileLocationBrowse_Click(Nothing, Nothing)
        End If
        If txtFSPL.Text = String.Empty Then
            btnFSPL_Click(Nothing, Nothing)
        End If
        If txtFSL.Text = String.Empty Then
            btnFSL_Click(Nothing, Nothing)
        End If
        If txtFCopyToLocation.Text = String.Empty Then
            btnFCopyToLocationBrowse_Click(Nothing, Nothing)
        End If

        Dim strFileLocation As String = txtFFileLocation.Text
        If strFileLocation = String.Empty Then Return
        If Not strFileLocation.EndsWith("\") Then strFileLocation += "\"

        Dim strSPLocation As String = txtFSPL.Text
        If strSPLocation = String.Empty Then Return
        If Not strSPLocation.EndsWith("\") Then strSPLocation += "\"

        Dim strSLocation As String = txtFSL.Text
        If strSLocation = String.Empty Then Return
        If Not strSLocation.EndsWith("\") Then strSLocation += "\"

        Dim strCopyToLocation As String = txtFCopyToLocation.Text
        If strCopyToLocation = String.Empty Then Return
        If Not strCopyToLocation.EndsWith("\") Then strCopyToLocation += "\"

        If Me.chkFDeleteFile.Checked Then
            ClearDirectory(strCopyToLocation)
        End If

        Dim iChangedFiles As Integer = _udtChangeFileList.Count + _udtChangeStoreProcList.Count + _udtChangeScriptList.Count + _udtChangeTriggerList.Count

        CopyFile(strFileLocation, strCopyToLocation, gvF, "FFileName", "FStatus", "Front-end")
        CopyFile(strSPLocation, strCopyToLocation, gvSP, "gvcSPFileName", "gvcSPStatus", "Back-end")
        CopyFile(strSPLocation, strCopyToLocation, gvT, "gvcTFileName", "gvcTStatus", "Back-end")
        CopyFile(strSLocation, strCopyToLocation, gvS, "gvcSFileName", "gvcSStatus", "Back-end")

        Dim currentTime As String
        currentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        lblStatus.Text = String.Format("{0} distinct changed files read successfully: {1} copied ({2} replaced), {3} skipped, {4} not found, {5} error {6}", _
                                        New String() {iChangedFiles, _
                                                      intCopied, intReplace, intSkip, intFileNotFound, intError, currentTime})

        intFileNotFound = 0
        intSkip = 0
        intError = 0
        intReplace = 0
        intCopied = 0
    End Sub

    Private Sub CopyFile(ByVal strLocation As String, ByVal strDestination As String, ByVal e As System.Windows.Forms.DataGridView, ByVal fileName As String, ByVal Status As String, ByVal Directory As String)

        For Each dr As DataGridViewRow In e.Rows
            dr.Cells(Status).ErrorText = String.Empty
            Dim strFrom As String
            Dim strTo As String
            strFrom = String.Format("{0}{1}", strLocation, dr.Cells(fileName).Value)
            strTo = String.Format("{0}{2}\{1}", strDestination, dr.Cells(fileName).Value, Directory)

            If Not File.Exists(strFrom) Then
                dr.Cells(Status).Value = "File Not Found"
                dr.Cells(Status).Style.ForeColor = Color.Red
                dr.Cells(Status).ErrorText = String.Format("Could not find file '{0}'", strFrom)
                intFileNotFound += 1
                Continue For

            End If

            ' Skip copy project file
            If Not Me.chkCopyProjectFile.Checked AndAlso strFrom.EndsWith(".vbproj") Then
                dr.Cells(Status).Value = "Skipped"
                dr.Cells(Status).Style.ForeColor = Color.Blue
                dr.Cells(Status).ErrorText = String.Empty
                intSkip += 1
                Continue For
            End If

            ' Skip copy config file
            If Not Me.chkCopyConfigFile.Checked AndAlso strFrom.EndsWith(".config") Then
                dr.Cells(Status).Value = "Skipped"
                dr.Cells(Status).Style.ForeColor = Color.Blue
                dr.Cells(Status).ErrorText = String.Empty
                intSkip += 1
                Continue For
            End If

            Try

                MakeSureDirectoryPathExists(strTo)

                ' Handle exist file
                If File.Exists(strTo) Then
                    If Me.chkFReplaceExist.Checked Then
                        ' Mark not readonly before overwrite
                        File.SetAttributes(strTo, FileAttributes.Normal)
                        intReplace += 1
                    Else
                        dr.Cells(Status).Value = "File Exists in Destination"
                        dr.Cells(Status).Style.ForeColor = Color.Red
                        dr.Cells(Status).ErrorText = String.Format("File '{0}' already exists", strTo)
                        intError += 1
                        Continue For
                    End If
                End If


                File.Copy(strFrom, strTo, chkFReplaceExist.Checked)

                dr.Cells(Status).Value = "Copied"
                dr.Cells(Status).Style.ForeColor = Color.Green
                intCopied += 1

            Catch ex As Exception
                dr.Cells(Status).Value = "Error"
                dr.Cells(Status).ErrorText = ex.Message
                dr.Cells(Status).Style.ForeColor = Color.Red
                intError += 1
                Continue For

            End Try

        Next

    End Sub

    Private Sub ClearDirectory(ByVal strPath As String)
        MarkFileAttributeNormal(strPath)

        Dim di As New DirectoryInfo(strPath)
        For Each di2 As DirectoryInfo In di.GetDirectories
            di2.Delete(True)
        Next

        For Each fi As FileInfo In di.GetFiles
            fi.Delete()
        Next
    End Sub

    ''' <summary>
    ''' Remove Read-only in files before delete
    ''' </summary>
    ''' <param name="strPath"></param>
    ''' <remarks></remarks>
    Private Sub MarkFileAttributeNormal(ByVal strPath As String)
        Dim mainDir As New DirectoryInfo(strPath)
        Dim fInfo As IO.FileInfo() = mainDir.GetFiles("*.*", SearchOption.AllDirectories)

        'now loop through all the files and delete them
        Dim file As IO.FileInfo

        For Each file In fInfo
            If (file.Attributes And FileAttributes.ReadOnly) Then
                file.Attributes = FileAttributes.Normal
            End If
        Next

        ' do the same for the directories
        Dim dInfo As DirectoryInfo() = mainDir.GetDirectories("*.*")
        Dim dir As DirectoryInfo

        For Each dir In dInfo
            If (dir.Attributes And FileAttributes.ReadOnly) Then
                dir.Attributes = FileAttributes.Normal
            End If
        Next
    End Sub

    Private Sub btnFFileLocationBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFFileLocationBrowse.Click
        txtFFileLocation.BackColor = Color.Yellow

        FFileLocationBrowseDialog.Description = "Select File Location"
        FFileLocationBrowseDialog.ShowNewFolderButton = False

        If System.IO.Directory.Exists(txtFFileLocation.Text) Then
            FFileLocationBrowseDialog.SelectedPath = txtFFileLocation.Text
        End If

        If FFileLocationBrowseDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtFFileLocation.Text = FFileLocationBrowseDialog.SelectedPath
        End If

        txtFFileLocation.BackColor = Nothing

    End Sub

    Private Sub btnFCopyToLocationBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFCopyToLocationBrowse.Click
        txtFCopyToLocation.BackColor = Color.Yellow

        FCopyToLocationBrowseDialog.Description = "Select Copy to Location"
        FCopyToLocationBrowseDialog.ShowNewFolderButton = True

        If System.IO.Directory.Exists(txtFCopyToLocation.Text) Then
            FCopyToLocationBrowseDialog.SelectedPath = txtFCopyToLocation.Text
        End If

        If FCopyToLocationBrowseDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtFCopyToLocation.Text = FCopyToLocationBrowseDialog.SelectedPath
        End If

        txtFCopyToLocation.BackColor = Nothing
    End Sub

    Private Sub btnFSPL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFSPL.Click
        txtFSPL.BackColor = Color.Yellow

        FSPLBrowseDialog.Description = "Select File Location"
        FSPLBrowseDialog.ShowNewFolderButton = True

        If System.IO.Directory.Exists(txtFSPL.Text) Then
            FSPLBrowseDialog.SelectedPath = txtFSPL.Text
        End If

        If FSPLBrowseDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtFSPL.Text = FSPLBrowseDialog.SelectedPath
        End If

        txtFSPL.BackColor = Nothing
    End Sub

    Private Sub btnFSL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFSL.Click
        txtFSL.BackColor = Color.Yellow

        FSLBrowseDialog.Description = "Select File Location"
        FSLBrowseDialog.ShowNewFolderButton = True

        If System.IO.Directory.Exists(txtFSL.Text) Then
            FSLBrowseDialog.SelectedPath = txtFSL.Text
        End If

        If FSLBrowseDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtFSL.Text = FSLBrowseDialog.SelectedPath
        End If

        txtFSL.BackColor = Nothing
    End Sub

    Private Sub gvF_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gvF.CellDoubleClick
        If e.ColumnIndex = 0 Then
            Try
                If txtFFileLocation.Text = String.Empty Then
                    btnFFileLocationBrowse_Click(Nothing, Nothing)
                End If

                Dim strFileLocation As String = txtFFileLocation.Text

                If strFileLocation = String.Empty Then Return
                If Not strFileLocation.EndsWith("\") Then strFileLocation += "\"

                Process.Start("notepad.exe", String.Format("{0}{1}", strFileLocation, gvF.Rows(e.RowIndex).Cells(0).Value))
            Catch ex As Exception
                MsgBox("Please DOUBLECLICK the required file")
            End Try
        End If
    End Sub

    Private Sub gvSP_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gvSP.CellDoubleClick
        If e.ColumnIndex = 0 Then
            Try
                If txtFSPL.Text = String.Empty Then
                    btnFSPL_Click(Nothing, Nothing)
                End If

                Dim strFSPL As String = txtFSPL.Text

                If strFSPL = String.Empty Then Return
                If Not strFSPL.EndsWith("\") Then strFSPL += "\"

                Process.Start("notepad.exe", String.Format("{0}{1}", strFSPL, gvSP.Rows(e.RowIndex).Cells(0).Value))

            Catch ex As Exception
                MsgBox("Please DOUBLECLICK the required StoreProc file")
            End Try
        End If
    End Sub

    Private Sub gvS_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gvS.CellDoubleClick
        If e.ColumnIndex = 0 Then
            Try
                If txtFSL.Text = String.Empty Then
                    btnFSL_Click(Nothing, Nothing)
                End If

                Dim strFSL As String = txtFSL.Text

                If strFSL = String.Empty Then Return
                If Not strFSL.EndsWith("\") Then strFSL += "\"

                Process.Start("notepad.exe", String.Format("{0}{1}", strFSL, gvS.Rows(e.RowIndex).Cells(0).Value))

            Catch ex As Exception
                MsgBox("Please DOUBLECLICK the required Script file")
            End Try
        End If
    End Sub

    Private Sub gvT_CellDoubleClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles gvT.CellDoubleClick
        If e.ColumnIndex = 0 Then
            Try
                If txtFSPL.Text = String.Empty Then
                    btnFSPL_Click(Nothing, Nothing)
                End If

                Dim strFSPL As String = txtFSPL.Text

                If strFSPL = String.Empty Then Return
                If Not strFSPL.EndsWith("\") Then strFSPL += "\"

                Process.Start("notepad.exe", String.Format("{0}{1}", strFSPL, gvT.Rows(e.RowIndex).Cells(0).Value))

            Catch ex As Exception
                MsgBox("Please DOUBLECLICK the required Trigger file")
            End Try
        End If
    End Sub

    Private Declare Function MakeSureDirectoryPathExists Lib "imagehlp.dll" (ByVal lpPath As String) As Long

#End Region

#Region "Build"

    Private Sub btnBShow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBShow.Click
        gvBL.Rows.Clear()

        For Each udtChangeFile As ChangeFileModel In _udtChangeFileList.Values
            gvBL.Rows.Add(udtChangeFile.FileName)
        Next
    End Sub

    Private Sub btnBCheck_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBCheck.Click
        PrepareMapping(Me.cboSuffix.SelectedItem)
        btnBShow_Click(Nothing, Nothing)

        _udtBuildFileList.Clear()

        Dim strInName As String = Nothing
        Dim strProject As String = Nothing

        Dim strBuildFileName As String = Nothing
        Dim strBuildFileCopyFrom As String = Nothing

        Dim blnAlreadyIncludeCommon As Boolean = False

        Dim cllnProjectBinDestinationNotFound As Collection = New Collection
        Dim cllnProjectFileMappingNotFound As Collection = New Collection

        For Each dr As DataGridViewRow In gvBL.Rows
            strInName = dr.Cells("BLFileName").Value

            If FileIsInExcludeList(strInName) Then Continue For

            strProject = GetProjectFromFile(strInName)

            If strInName.ToLower.EndsWith(".vb") Or strInName.ToLower.EndsWith(".vbproj") _
                Or strInName.ToLower.EndsWith(".cs") Or strInName.ToLower.EndsWith(".csproj") Then
                If strProject.ToLower = "common" Then
                    If blnAlreadyIncludeCommon Then
                        Continue For
                    Else
                        blnAlreadyIncludeCommon = True
                    End If

                End If

                Dim arrProjectBinDestination As ArrayList = GetProjectBinDestination(strProject)
                If arrProjectBinDestination Is Nothing Then
                    If Not cllnProjectBinDestinationNotFound.Contains(strProject) Then cllnProjectBinDestinationNotFound.Add(strProject, strProject)
                Else
                    For Each strBinDestination As String In GetProjectBinDestination(strProject)
                        If Me.dicLibraryProject.ContainsKey(strProject.ToLower) Then
                            For Each strDll As String In aryDllExtension
                                strBuildFileName = String.Format("{0}{1}.{2}", strBinDestination, strProject, strDll)
                                strBuildFileCopyFrom = String.Format("{0}\bin\release\{1}.{2}", strProject, strProject, strDll)
                                _udtBuildFileList.Add(New BuildFileModel(strBuildFileName, strBuildFileCopyFrom))
                            Next
                        ElseIf Me.dicConsoleProject.ContainsKey(strProject.ToLower) Then
                            For Each strDll As String In aryExeExtension
                                strBuildFileName = String.Format("{0}{1}.{2}", strBinDestination, strProject, strDll)
                                strBuildFileCopyFrom = String.Format("{0}\bin\release\{1}.{2}", strProject, strProject, strDll)
                                _udtBuildFileList.Add(New BuildFileModel(strBuildFileName, strBuildFileCopyFrom))
                            Next
                        Else
                            For Each strDll As String In aryDllExtension
                                strBuildFileName = String.Format("{0}{1}.{2}", strBinDestination, strProject, strDll)
                                strBuildFileCopyFrom = String.Format("{0}\bin\{1}.{2}", strProject, strProject, strDll)
                                _udtBuildFileList.Add(New BuildFileModel(strBuildFileName, strBuildFileCopyFrom))
                            Next
                        End If

                    Next
                End If
                '==================================================================================================

            ElseIf strInName.ToLower.StartsWith("staticpage\") Then
                Dim strSpecificPath As String
                strSpecificPath = GetSpecificPath2Level(strInName)
                For Each strFolderDestination As String In GetStaticFolderDestination(strSpecificPath)

                    strBuildFileName = String.Format("{0}{1}", strFolderDestination, StaticpageGetFileFromFile(strInName, strSpecificPath))

                    strBuildFileCopyFrom = String.Format("{0}\{1}", strProject, GetFileFromFile(strInName))

                    _udtBuildFileList.Add(New BuildFileModel(strBuildFileName, strBuildFileCopyFrom))
                Next

                strSpecificPath = GetSpecificPath3Level(strInName)
                If strSpecificPath <> String.Empty Then
                    For Each strFolderDestination As String In GetStaticFolderDestination(strSpecificPath)

                        strBuildFileName = String.Format("{0}{1}", strFolderDestination, StaticpageGetFileFromFile(strInName, strSpecificPath))

                        strBuildFileCopyFrom = String.Format("{0}\{1}", strProject, GetFileFromFile(strInName))

                        _udtBuildFileList.Add(New BuildFileModel(strBuildFileName, strBuildFileCopyFrom))
                    Next
                End If

                '==================================================================================================

            ElseIf strInName.ToLower.StartsWith("commonbin\") Or strInName.ToLower.StartsWith("common.dll\") Then
                ' [eHS] CommonBin folder is not a project, no compiled file will be copied
                ' [PCD] common.dll folder is not a project, no compiled file will be copied

                '==================================================================================================
            Else
                Dim arrProjectFileDestination As ArrayList = GetProjectFileDestination(strProject)
                If arrProjectFileDestination Is Nothing Then
                    If Not cllnProjectFileMappingNotFound.Contains(strProject) Then cllnProjectFileMappingNotFound.Add(strProject, strProject)
                Else
                    For Each strFileDestination As String In GetProjectFileDestination(strProject)

                        If Me.dicConsoleProject.ContainsKey(strProject.ToLower) And strInName.EndsWith(".config") Then
                            strBuildFileName = String.Format("{0}{1}.exe.config", strFileDestination, strProject)
                            strBuildFileCopyFrom = String.Format("{0}\bin\release\{1}.exe.config", strProject, strProject)
                        Else
                            strBuildFileName = String.Format("{0}{1}", strFileDestination, GetFileFromFile(strInName))
                            strBuildFileCopyFrom = String.Format("{0}\{1}", strProject, GetFileFromFile(strInName))
                        End If

                        _udtBuildFileList.Add(New BuildFileModel(strBuildFileName, strBuildFileCopyFrom))
                    Next
                End If
            End If

        Next

        gvBR.Rows.Clear()

        For Each objBuildFile As BuildFileModel In _udtBuildFileList.Values
            gvBR.Rows.Add(New String() {objBuildFile.BuildFileName, objBuildFile.BuildFileCopyFrom})
        Next

        lblBBuildFileNum.Text = String.Format("No. of expected built files: {0}", _udtBuildFileList.Count)

        ' Show popup message
        Dim strMsg As String = String.Empty
        Dim strMode As String = ConfigurationManager.AppSettings("Mode")
        If cllnProjectFileMappingNotFound.Count > 0 Then
            For Each strProjectNotFound As String In cllnProjectFileMappingNotFound
                strMsg += String.Format("<{0}ProjectFileMapping>{1}", strMode, strProjectNotFound) + vbCrLf
            Next
        End If
        If cllnProjectBinDestinationNotFound.Count > 0 Then
            For Each strProjectNotFound As String In cllnProjectBinDestinationNotFound
                strMsg += String.Format("<{0}ProjectBinDestination>{1}", strMode, strProjectNotFound) + vbCrLf
            Next
        End If

        If strMsg <> String.Empty Then
            MsgBox(strMsg, MsgBoxStyle.Critical, "Project setting not found")
        End If
    End Sub

    Private Sub btnBVerify_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBVerify.Click
        Me.btnBCheck_Click(sender, e)

        Dim intFileNotFound As Integer = 0
        Dim intError As Integer = 0
        Dim intVerify As Integer = 0
        Dim strCopyForm As String = String.Empty
        Dim strBuildFileName As String = String.Empty

        For Each dr As DataGridViewRow In gvBR.Rows
            dr.Cells("BRStatus").ErrorText = String.Empty
            strCopyForm = Path.Combine(Me.txtBFileLocation.Text, dr.Cells("BRCopyFrom").Value)
            strBuildFileName = Path.Combine(Me.txtBCopyToLocation.Text, dr.Cells("BRFileName").Value)
            infoReader = My.Computer.FileSystem.GetFileInfo(strCopyForm)

            If Not File.Exists(strCopyForm) Then
                dr.Cells("BRStatus").Value = "File Not Found"
                dr.Cells("BRLastWriteTime").Value = "File Not Found"
                dr.Cells("BRStatus").Style.ForeColor = Color.Red
                dr.Cells("BRLastWriteTime").Style.ForeColor = Color.Red
                dr.Cells("BRStatus").ErrorText = String.Format("Could not find file '{0}'", strCopyForm)
                intFileNotFound += 1
                Continue For
            Else

            End If

            If File.Exists(strBuildFileName) Then
                dr.Cells("BRLastWriteTime").Value = infoReader.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
                dr.Cells("BRStatus").Value = "File Exists in Destination"
                dr.Cells("BRStatus").Style.ForeColor = Color.Green
                dr.Cells("BRStatus").ErrorText = String.Format("File '{0}' already exists", strBuildFileName)
                intError += 1
                Continue For
            End If
            dr.Cells("BRLastWriteTime").Value = infoReader.LastWriteTime.ToString("yyyy-MM-dd HH:mm")
            dr.Cells("BRStatus").Value = "Verified"
            dr.Cells("BRStatus").Style.ForeColor = Color.Blue
            dr.Cells("BRStatus").ErrorText = String.Empty
            intVerify += 1
        Next
        Dim currentTime As String
        currentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        lblStatus.Text = String.Format("{0} build files read successfully: {1} verified, {2} not found, {3} error {4}", _
                                        New String() {gvBR.Rows.Count, intVerify, intFileNotFound, intError, currentTime})
    End Sub

    Private Sub btnBExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBExport.Click
        MessageBox.Show("Congratulation! This function has not been implemented!")
    End Sub

    Private Sub btnBCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBCopy.Click
        If txtBFileLocation.Text = String.Empty Then
            btnBFileLocationBrowse_Click(Nothing, Nothing)
        End If

        Dim strFileLocation As String = txtBFileLocation.Text

        If strFileLocation = String.Empty Then Return

        If txtBCopyToLocation.Text = String.Empty Then
            btnBCopyToLocationBrowse_Click(Nothing, Nothing)
        End If

        Dim strCopyToLocation As String = txtBCopyToLocation.Text

        If strCopyToLocation = String.Empty Then Return

        If Me.chkBDeleteFile.Checked Then
            ClearDirectory(strCopyToLocation)
        End If

        Dim intCopied As Integer = 0
        Dim intFileNotFound As Integer = 0
        Dim intError As Integer = 0
        Dim intSkip As Integer = 0
        Dim intReplace As Integer = 0

        Dim strFrom As String = Nothing
        Dim strTo As String = Nothing

        For Each dr As DataGridViewRow In gvBR.Rows
            dr.Cells("BRStatus").ErrorText = String.Empty

            strFrom = Path.Combine(strFileLocation, dr.Cells("BRCopyFrom").Value)
            strTo = Path.Combine(strCopyToLocation, dr.Cells("BRFileName").Value)

            If Not Me.chkCopyConfigFile.Checked Then
                If strTo.EndsWith(".config") Then
                    dr.Cells("BRStatus").Value = "Skipped"
                    dr.Cells("BRStatus").Style.ForeColor = Color.Blue
                    intSkip += 1
                    Continue For
                End If
            End If

            If Not File.Exists(strFrom) Then
                dr.Cells("BRStatus").Value = "File Not Found"
                dr.Cells("BRStatus").Style.ForeColor = Color.Red
                dr.Cells("BRStatus").ErrorText = String.Format("Could not find file '{0}'", strFrom)
                intFileNotFound += 1
                Continue For

            End If

            Try
                MakeSureDirectoryPathExists(strTo)

                If File.Exists(strTo) Then
                    If Me.chkBReplaceExist.Checked Then
                        ' Mark not readonly before overwrite
                        File.SetAttributes(strTo, FileAttributes.Normal)
                        intReplace += 1
                    Else
                        dr.Cells("BRStatus").Value = "File Exists in Destination"
                        dr.Cells("BRStatus").Style.ForeColor = Color.Red
                        dr.Cells("BRStatus").ErrorText = String.Format("File '{0}' already exists", strTo)
                        intError += 1
                        Continue For
                    End If
                End If

                File.Copy(strFrom, strTo, Me.chkBReplaceExist.Checked)

                dr.Cells("BRStatus").Value = "Copied"
                dr.Cells("BRStatus").Style.ForeColor = Color.Green
                intCopied += 1
            Catch ex As Exception
                dr.Cells("BRStatus").Value = "Error"
                dr.Cells("BRStatus").ErrorText = ex.Message
                dr.Cells("BRStatus").Style.ForeColor = Color.Red
                intError += 1
                Continue For

            End Try

        Next
        Dim currentTime As String
        currentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        lblStatus.Text = String.Format("{0} expected built files processed successfully: {1} copied ({2} replaced), {3} skipped, {4} not found, {5} error {6}", _
                                        New String() {gvBR.Rows.Count, intCopied, intReplace, intSkip, intFileNotFound, intError, currentTime})

    End Sub

    Private Sub btnBFileLocationBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBFileLocationBrowse.Click
        txtBFileLocation.BackColor = Color.Yellow

        BFileLocationBrowseDialog.Description = "Select File Location"
        BFileLocationBrowseDialog.ShowNewFolderButton = False

        If System.IO.Directory.Exists(txtBFileLocation.Text) Then
            BFileLocationBrowseDialog.SelectedPath = txtBFileLocation.Text
        End If

        If BFileLocationBrowseDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtBFileLocation.Text = BFileLocationBrowseDialog.SelectedPath
        End If

        txtBFileLocation.BackColor = Nothing
    End Sub

    Private Sub btnBCopyToLocationBrowse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBCopyToLocationBrowse.Click
        txtBCopyToLocation.BackColor = Color.Yellow

        BCopyToLocationBrowseDialog.Description = "Select Copy to Location"
        BCopyToLocationBrowseDialog.ShowNewFolderButton = True

        If System.IO.Directory.Exists(txtBCopyToLocation.Text) Then
            BCopyToLocationBrowseDialog.SelectedPath = txtBCopyToLocation.Text
        End If

        If BCopyToLocationBrowseDialog.ShowDialog() = Windows.Forms.DialogResult.OK Then
            txtBCopyToLocation.Text = BCopyToLocationBrowseDialog.SelectedPath
        End If

        txtBCopyToLocation.BackColor = Nothing
    End Sub


    Private Function GetProjectFromFile(ByVal strFileName As String) As String
        Return strFileName.Substring(0, strFileName.IndexOf("\"))
    End Function

    Private Function GetSpecificPath2Level(ByVal strFileName As String) As String
        Dim first As String = strFileName.Split("\")(0).Trim
        Dim second As String = strFileName.Split("\")(1).Trim
        Return first + "\" + second
    End Function

    Private Function GetSpecificPath3Level(ByVal strFileName As String) As String
        If strFileName.Split("\").Length < 3 Then Return String.Empty

        Dim first As String = strFileName.Split("\")(0).Trim
        Dim second As String = strFileName.Split("\")(1).Trim
        Dim thrid As String = strFileName.Split("\")(2).Trim
        Return first + "\" + second + "\" + thrid
    End Function

    Private Function GetFileFromFile(ByVal strFileName As String) As String
        Return strFileName.Substring(strFileName.IndexOf("\") + 1)
    End Function

    Private Function StaticpageGetFileFromFile(ByVal strFileName As String, ByVal strFolderToBeRemoved As String) As String
        If strFileName.StartsWith(strFolderToBeRemoved) Then
            Return strFileName.Substring(strFolderToBeRemoved.Length + IIf(strFolderToBeRemoved.EndsWith("\"), 0, 1))
        Else
            Return strFileName
        End If
        'Dim path As String = strFileName.Substring(strFileName.IndexOf("\") + 1)
        'Dim second As String = strFileName.Split("\")(1).Trim + "\"
        'Return path.Replace(second, Nothing)
    End Function

    Private Function GetProjectFileDestination(ByVal strProject As String) As ArrayList
        If Not dicProjectFileMapping.ContainsKey(strProject.ToLower) Then Return Nothing

        Return dicProjectFileMapping(strProject.ToLower)
    End Function

    Private Function GetStaticFolderDestination(ByVal strProject As String) As ArrayList
        Return dicStaticFolderMapping(strProject.ToLower)
    End Function

    Private Function GetProjectBinDestination(ByVal strProject As String) As ArrayList
        If Not dicProjectBinMapping.ContainsKey(strProject.ToLower) Then Return Nothing

        Return dicProjectBinMapping(strProject.ToLower)
    End Function

    Private Function GetProjectFileSource(ByVal strFileName As String) As String
        For Each strDll As String In aryDllExtension
            If strFileName.EndsWith(String.Format("Common.{0}", strDll)) Then
                Return String.Format("Common\bin\Common.{0}", strDll)
            End If
        Next

        For Each strLocation As String In dicProjectFileMappingReverse.Keys
            If strFileName.StartsWith(strLocation) Then
                Return strFileName.Replace(strLocation, dicProjectFileMappingReverse(strLocation))
            End If

        Next

        Return Nothing

    End Function

    Private Function FileIsInExcludeList(ByVal strFileName As String) As Boolean
        For Each strExtension As String In aryExcludeExtension
            If strFileName.EndsWith(strExtension) Then
                Return True
            End If
        Next

        Return False
    End Function

#End Region

    Private Sub chkCopyConfigFile_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkCopyConfigFile.CheckedChanged

    End Sub

    Private Sub lblFCopyToLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblFCopyToLocation.Click
        OpenFolder(txtFCopyToLocation.Text)
    End Sub

    Private Sub lblFFileLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblFFileLocation.Click
        OpenFolder(Me.txtFFileLocation.Text)
    End Sub

    Private Sub lblBFileLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblBFileLocation.Click
        OpenFolder(Me.txtBFileLocation.Text)
    End Sub

    Private Sub lblBCopyToLocation_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblBCopyToLocation.Click
        OpenFolder(Me.txtBCopyToLocation.Text)
    End Sub

    Private Sub lblFSPL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblFSPL.Click
        OpenFolder(Me.txtFSPL.Text)
    End Sub

    Private Sub lblFSL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblFSL.Click
        OpenFolder(Me.txtFSL.Text)
    End Sub

    Private Sub OpenFolder(ByVal txtFolderPath As String)
        Try
            Dim p As New System.Diagnostics.Process
            Dim s As New System.Diagnostics.ProcessStartInfo(txtFolderPath)
            s.UseShellExecute = True
            s.WindowStyle = ProcessWindowStyle.Normal
            p.StartInfo = s
            p.Start()
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try
    End Sub

    Private Sub gvFBind()
        dgvF.Visible = False
        gvF.Visible = True
        Me.gvF.Columns.Clear()
        Me.FFileName.DisplayIndex = 0
        Me.FLastWriteTime.DisplayIndex = 1
        Me.FUpdater.DisplayIndex = 2
        Me.FCR.DisplayIndex = 3
        Me.FNewFile.DisplayIndex = 4
        Me.FStatus.DisplayIndex = 5
        Me.FCheck.DisplayIndex = 6
        Me.gvF.Columns.AddRange(New DataGridViewColumn() {Me.FFileName, Me.FLastWriteTime, Me.FUpdater, Me.FCR, Me.FNewFile, Me.FStatus, Me.FCheck})
        gvF.DataSource = New BindingSource(_udtChangeFileList.Values, Nothing)
    End Sub

    Private Sub dgvFBind()
        dgvF.Visible = True
        gvF.Visible = False
        Me.dgvF.Columns.Clear()
        Me.dgvFFileName.DisplayIndex = 0
        Me.dgvFStatus.DisplayIndex = 1
        Me.dgvF.Columns.AddRange(New DataGridViewColumn() {Me.dgvFFileName, Me.dgvFStatus})
    End Sub

    Private Sub gvSPBind()
        dgvSP.Visible = False
        gvSP.Visible = True
        Me.gvSP.Columns.Clear()
        Me.gvcSPFileName.DisplayIndex = 0
        Me.gvcSPLastWriteTime.DisplayIndex = 1
        Me.gvcSPUpdater.DisplayIndex = 2
        Me.gvcSPCR.DisplayIndex = 3
        Me.gvcSPNewFile.DisplayIndex = 4
        Me.gvcSPStatus.DisplayIndex = 5
        Me.gvcSPCheck.DisplayIndex = 6
        Me.gvSP.Columns.AddRange(New DataGridViewColumn() {Me.gvcSPFileName, Me.gvcSPLastWriteTime, Me.gvcSPUpdater, Me.gvcSPCR, Me.gvcSPNewFile, Me.gvcSPStatus, Me.gvcSPCheck})
        gvSP.DataSource = New BindingSource(_udtChangeStoreProcList.Values, Nothing)
    End Sub

    Private Sub gvTBind()
        dgvT.Visible = False
        gvT.Visible = True
        Me.gvT.Columns.Clear()
        Me.gvcTFileName.DisplayIndex = 0
        Me.gvcTLastWriteTime.DisplayIndex = 1
        Me.gvcTUpdater.DisplayIndex = 2
        Me.gvcTCR.DisplayIndex = 3
        Me.gvcTNewFile.DisplayIndex = 4
        Me.gvcTStatus.DisplayIndex = 5
        Me.gvcTCheck.DisplayIndex = 6
        Me.gvT.Columns.AddRange(New DataGridViewColumn() {Me.gvcTFileName, Me.gvcTLastWriteTime, Me.gvcTUpdater, Me.gvcTCR, Me.gvcTNewFile, Me.gvcTStatus, Me.gvcTCheck})
        gvT.DataSource = New BindingSource(_udtChangeTriggerList.Values, Nothing)
    End Sub

    Private Sub dgvSPBind()
        dgvSP.Visible = True
        gvSP.Visible = False
        Me.dgvSP.Columns.Clear()
        Me.dgvcSPFileName.DisplayIndex = 0
        Me.dgvcSPStatus.DisplayIndex = 1
        Me.dgvSP.Columns.AddRange(New DataGridViewColumn() {Me.dgvcSPFileName, Me.dgvcSPStatus})
    End Sub

    Private Sub dgvTBind()
        dgvT.Visible = True
        gvT.Visible = False
        Me.dgvT.Columns.Clear()
        Me.dgvcTFileName.DisplayIndex = 0
        Me.dgvcTStatus.DisplayIndex = 1
        Me.dgvT.Columns.AddRange(New DataGridViewColumn() {Me.dgvcTFileName, Me.dgvcTStatus})
    End Sub

    Private Sub gvSBind()
        dgvS.Visible = False
        gvS.Visible = True
        Me.gvS.Columns.Clear()
        Me.gvcSFileName.DisplayIndex = 0
        Me.gvcSLastWriteTime.DisplayIndex = 1
        Me.gvcSUpdater.DisplayIndex = 2
        Me.gvcSCR.DisplayIndex = 3
        Me.gvcSNewFile.DisplayIndex = 4
        Me.gvcSStatus.DisplayIndex = 5
        Me.gvcSCheck.DisplayIndex = 6
        Me.gvS.Columns.AddRange(New DataGridViewColumn() {Me.gvcSFileName, Me.gvcSLastWriteTime, Me.gvcSUpdater, Me.gvcSCR, Me.gvcSNewFile, Me.gvcSStatus, Me.gvcSCheck})
        gvS.DataSource = New BindingSource(_udtChangeScriptList.Values, Nothing)
    End Sub

    Private Sub dgvSBind()
        dgvS.Visible = True
        gvS.Visible = False
        Me.dgvS.Columns.Clear()
        Me.dgvcSFileName.DisplayIndex = 0
        Me.dgvcSStatus.DisplayIndex = 1
        Me.dgvS.Columns.AddRange(New DataGridViewColumn() {Me.dgvcSFileName, Me.dgvcSStatus})
    End Sub

    Private Sub btnFConsolid_Click(sender As System.Object, e As System.EventArgs) Handles btnFConsolid.Click
        Dim strSPLocation As String = txtFSPL.Text
        If strSPLocation = String.Empty Then Return
        If Not strSPLocation.EndsWith("\") Then strSPLocation += "\"
        Dim strSLocation As String = txtFSL.Text
        If strSLocation = String.Empty Then Return
        If Not strSLocation.EndsWith("\") Then strSLocation += "\"
        Dim del As Boolean

        If strSPLocation = strSLocation Then
            del = False
        Else
            del = True
        End If

        GetConsolidatedFile(strSPLocation, gvSP, "gvcSPFileName", True)
        GetConsolidatedFile(strSPLocation, gvT, "gvcTFileName", False)
        GetConsolidatedFile(strSLocation, gvS, "gvcSFileName", del)

        'Dim counter = My.Computer.FileSystem.GetFiles(String.Format("{0}{1}", strSPLocation, ConsolidatedFileName))
        'counter = CStr(counter.Count)
        Dim currentTime As String
        currentTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        lblStatus.Text = String.Format("Text files generated successfully {0}", currentTime)

    End Sub

    Private Sub GetConsolidatedFile(ByVal strLocation As String, ByVal e As System.Windows.Forms.DataGridView, ByVal gvcFileName As String, ByVal clearFile As Boolean)
        System.IO.Directory.CreateDirectory(String.Format("{0}{1}", strLocation, ConsolidatedFileName))

        If clearFile = True Then
            Dim f As String
            For Each f In Directory.GetFiles(String.Format("{0}{1}", strLocation, ConsolidatedFileName))
                File.Delete(f)
            Next

        End If

        For Each dr As DataGridViewRow In e.Rows

            Dim filePath = String.Format("{0}{1}", strLocation, dr.Cells(gvcFileName).Value)
            Dim split As String() = filePath.Split("\")
            Dim parentFolder As String = split(split.Length - 2)
            Dim fileName As String = "\" & split(split.Length - 1)
            Dim subFilePath As String
            If split(split.Length - 1) = dr.Cells(gvcFileName).Value Then
                subFilePath = parentFolder
            Else
                subFilePath = dr.Cells(gvcFileName).Value.Replace(fileName, "").Trim()
                subFilePath = subFilePath.Replace("\", "_").Trim()
            End If
            Dim startDirectory As String = Path.GetDirectoryName(filePath)
            Dim outPutFile As String = String.Format("{0}{1}\{2}{3}", strLocation, ConsolidatedFileName, subFilePath, ".txt")
            Dim txtFile As String

            If File.Exists(filePath) Then
                Dim SW As New IO.StreamWriter(outPutFile)
                Dim txtFiles As String() = IO.Directory.GetFiles(startDirectory, "*.sql")
                For Each txtFile In txtFiles

                    Dim SR As New IO.StreamReader(txtFile)

                    Do Until SR.EndOfStream
                        Dim TempString As String = SR.ReadLine
                        SW.WriteLine(TempString)
                    Loop

                    SW.WriteLine()

                Next

                SW.Flush()
                SW.Close()

            End If
        Next
    End Sub
End Class
