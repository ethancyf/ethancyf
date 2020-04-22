Imports System.IO
Imports System.Configuration
Imports System.Text.RegularExpressions
Imports ConnectionStringEditor.BLL
Imports ConnectionStringEditor.Model
Imports EncryptionLib
Imports System.Threading

Public Class GeneratorForm
    Private m_pGeneratorFormModel = New GeneratorFormModel
    Private m_pAppSetting = LoadAppSetting.GetInstance()
    Private m_pFileBuilder = New FileStructBuilder
    Private m_pConfigFileBuilder As ConfigFileBuilder

    Private m_pConnectionModelList = New List(Of ConnectionStringModel)
    Private m_pConnectionStringParamList = New List(Of String)
    Private m_pConfigFileViewModel = New ConfigFileViewModel


    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        Control.CheckForIllegalCrossThreadCalls = False
        ' Add any initialization after the InitializeComponent() call.
        Init()
    End Sub

    Private Sub Init()
        Dim assembly As System.Reflection.Assembly = System.Reflection.Assembly.GetExecutingAssembly()
        Dim fvi As FileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location)
        Me.Text = "Connection String Editor - v" + fvi.FileVersion

        m_pAppSetting.LoadAppConfigSetting()
        SettingFileTBox.Text = m_pAppSetting.GetMySettingValues("SettingFile", SysConst.MYAPPSETTINGPATH)
        folderScanPath.Text = m_pAppSetting.GetMySettingValues("ProjectFolder", SysConst.MYAPPSETTINGPATH)
        buildTargetTBox.Text = m_pAppSetting.GetMySettingValues("BuildTargetPath", SysConst.MYAPPSETTINGPATH)

        'm_pGeneratorFormModel.strScanFolderPath = folderScanPath.Text

        Dim strTmpConnString As String = m_pAppSetting.GetMySettingValues("ConnectionStringList")
        Dim tmpConnStringArr As String() = strTmpConnString.Split(New Char() {"|"})
        m_pConnectionStringParamList.Clear()
        For Each tmpString As String In tmpConnStringArr
            m_pConnectionStringParamList.Add(tmpString.Trim())
        Next

        If Not String.IsNullOrEmpty(SettingFileTBox.Text) Then
            m_pAppSetting.LoadSettingFile(m_pGeneratorFormModel, folderScanPath.Text, SettingFileTBox.Text)
            InitSuffixCbox()
        End If
    End Sub

    Private Sub SettingFileBtn_Click(sender As Object, e As EventArgs) Handles SettingFileBtn.Click
        Dim openFileDialog1 As New OpenFileDialog()

        If (String.IsNullOrEmpty(SettingFileTBox.Text)) Then
            openFileDialog1.InitialDirectory = "c:\"
        Else
            openFileDialog1.InitialDirectory = Path.GetDirectoryName(SettingFileTBox.Text)
        End If

        openFileDialog1.Filter = "(*.config)|*.config"
        If openFileDialog1.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
            SettingFileTBox.Text = openFileDialog1.FileName
            m_pAppSetting.SetMySettingValues("SettingFile", SettingFileTBox.Text, SysConst.MYAPPSETTINGPATH)

            m_pAppSetting.LoadSettingFile(m_pGeneratorFormModel, folderScanPath.Text, SettingFileTBox.Text)
            InitSuffixCbox()
        End If
    End Sub

    Private Sub BrowseScanFolderBtn_Click(sender As Object, e As EventArgs) Handles BrowseScanFolderBtn.Click
        Dim folderBrowser As FolderBrowserDialog
        folderBrowser = New FolderBrowserDialog
        folderBrowser.ShowNewFolderButton = True
        folderBrowser.SelectedPath = folderScanPath.Text

        Dim result As DialogResult = folderBrowser.ShowDialog()
        If (result = Windows.Forms.DialogResult.OK) Then
            folderScanPath.Text = folderBrowser.SelectedPath
            m_pAppSetting.SetMySettingValues("ProjectFolder", folderScanPath.Text, SysConst.MYAPPSETTINGPATH)
        End If
    End Sub

    Private Sub buildTargetBrowseBtn_Click(sender As Object, e As EventArgs) Handles buildTargetBrowseBtn.Click
        Dim folderBrowser As FolderBrowserDialog
        folderBrowser = New FolderBrowserDialog
        Dim result As DialogResult = folderBrowser.ShowDialog()
        If (result = Windows.Forms.DialogResult.OK) Then
            buildTargetTBox.Text = folderBrowser.SelectedPath
            m_pAppSetting.SetMySettingValues("BuildTargetPath", buildTargetTBox.Text, SysConst.MYAPPSETTINGPATH)
        End If
    End Sub

    Private trd As Thread
    Private Sub ThreadTask()
        Me.Enabled = False
        Try
            m_pAppSetting.LoadSettingFile(m_pGeneratorFormModel, folderScanPath.Text, SettingFileTBox.Text)
            configFileListPBar.Value = 2

            'Build Directory Paths
            m_pGeneratorFormModel.LoadEHSProjectFileMappingList(SuffixCBox.SelectedItem.ToString())
            configFileListPBar.Value = 3

            'Scan Directory for config Files
            m_pFileBuilder.ScanDir(m_pGeneratorFormModel, folderScanPath.Text)
            configFileListPBar.Value = 4

            'Remove sub folders and files
            Dim FolderPathStructure As String = buildTargetTBox.Text + "\" + DateTime.Now.ToString("MM-dd-yyyy_[HH.mm.ss]") + SuffixCBox.SelectedItem.ToString()
            m_pFileBuilder.RemoveOrCreateDirectory(FolderPathStructure)
            configFileListPBar.Value = 5

            'Save Scanned Config File Path to List
            m_pFileBuilder.GenChangeList(m_pGeneratorFormModel, FolderPathStructure, m_pAppSetting.GetMySettingValues("ChangeListFileName"))
            configFileListPBar.Value = 6

            'Generate File List
            m_pGeneratorFormModel.strScanFolderPath = folderScanPath.Text
            m_pFileBuilder.GenFolderStruct(m_pGeneratorFormModel, FolderPathStructure, m_pAppSetting.GetMySettingValues("ChangeListFileName"))
            configFileListPBar.Value = 7

            'Load the Config File Paths to the UI List Box
            m_pFileBuilder.FillInConfigFilePathsForDic(m_pGeneratorFormModel)
            configFileListPBar.Value = 8

            LoadConfigFileList()
            configFileListPBar.Value = 9

            configFileListPBar.Visible = False
            EditorBtn.Enabled = True
            applyChangesBtn.Enabled = True
            ClearCFLBoxBtn.Enabled = True
            GenScanFileBtn.Enabled = False

            SettingFileTBox.Enabled = False
            folderScanPath.Enabled = False
            SuffixCBox.Enabled = False
            buildTargetTBox.Enabled = False

            SettingFileBtn.Enabled = False
            BrowseScanFolderBtn.Enabled = False
            buildTargetBrowseBtn.Enabled = False
        Catch ex As Exception

        End Try
        Me.Enabled = True
    End Sub

    Private Sub GenScanFileBtn_Click(sender As Object, e As EventArgs) Handles GenScanFileBtn.Click
        If (Not String.IsNullOrEmpty(folderScanPath.Text) _
            AndAlso Not String.IsNullOrEmpty(SettingFileTBox.Text) _
            AndAlso Not String.IsNullOrEmpty(buildTargetTBox.Text)) Then
            Try
                trd = New Thread(AddressOf ThreadTask)
                trd.IsBackground = True
                configFileListPBar.Step = 9
                configFileListPBar.Maximum = 9
                configFileListPBar.Value = 1
                configFileListPBar.Visible = True
                trd.Start()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub LoadConfigFileList()
        m_pConfigFileViewModel.pConfigFileRawDetail = New Dictionary(Of String, List(Of ParameterListModel))
        ConfigFileLView.Items.Clear()
        For Each tmpStr As String In m_pGeneratorFormModel.pFileList
            ConfigFileLView.Items.Add(tmpStr)
            Try
                Dim tmpParameterList = GetParameterListModel(tmpStr)
                If (tmpParameterList IsNot Nothing) Then
                    m_pConfigFileViewModel.pConfigFileRawDetail.Add(tmpStr, tmpParameterList)
                    Dim tmpList = From tmpParameter In tmpParameterList Where tmpParameter.eParamStatus <> SysConst.CONFIGFILE_PARAMSTATUS.NORMAL
                    If (tmpList.Count > 0) Then
                        ConfigFileLView.Items(ConfigFileLView.Items.Count - 1).ForeColor = Color.Red
                    Else
                        ConfigFileLView.Items(ConfigFileLView.Items.Count - 1).ForeColor = Color.Green
                    End If
                Else
                    ConfigFileLView.Items(ConfigFileLView.Items.Count - 1).ForeColor = Color.Red
                End If
            Catch ex As Exception
                ConfigFileLView.Items(ConfigFileLView.Items.Count - 1).ForeColor = Color.Red
            End Try
        Next
    End Sub

    Private Function GetParameterListModel(ByVal _strConfigFilePath) As List(Of ParameterListModel)
        Dim pResult = New List(Of ParameterListModel)
        m_pConfigFileBuilder = New ConfigFileBuilder(m_pGeneratorFormModel, _strConfigFilePath)

        'Step 1: Fill up All possible parameter sets with Prefix: "ConnectionString"
        Dim tmpDic As Dictionary(Of String, String) = m_pConfigFileBuilder.GetNewParameterList()
        If (tmpDic Is Nothing) Then
            Return Nothing
        End If

        Dim pTmpParameterListModel As ParameterListModel
        For Each iii As KeyValuePair(Of String, String) In tmpDic
            pTmpParameterListModel = New ParameterListModel
            pTmpParameterListModel.strParamName = iii.Key
            pTmpParameterListModel.strParamValue = iii.Value
            pTmpParameterListModel.eParamStatus = SysConst.CONFIGFILE_PARAMSTATUS.NORMAL
            pResult.Add(pTmpParameterListModel)
        Next

        'Step 2: Fill Up the missing parameters from the List of App Config
        For Each tmpStrParamName As String In m_pConnectionStringParamList
            Dim tmpList = From p In pResult Where p.strParamName = tmpStrParamName
            If (tmpList.Count = 0) Then
                pTmpParameterListModel = New ParameterListModel
                pTmpParameterListModel.strParamName = tmpStrParamName
                pTmpParameterListModel.strParamValue = Nothing
                pTmpParameterListModel.eParamStatus = SysConst.CONFIGFILE_PARAMSTATUS.MISSING
                pResult.Add(pTmpParameterListModel)
            End If
        Next

        'Step 3: Check for Extra parameters with the List of App Config
        For Each pTmpObj As ParameterListModel In pResult
            If (Not m_pConnectionStringParamList.Contains(pTmpObj.strParamName)) Then
                pTmpObj.eParamStatus = SysConst.CONFIGFILE_PARAMSTATUS.EXTRA
            End If
        Next

        Return pResult
    End Function

    Private Sub LoadConfigParamToList()

    End Sub

    Private Sub InitSuffixCbox()
        Try
            SuffixCBox.Items.Clear()
            For Each strTmp As String In m_pGeneratorFormModel.pSuffixArr
                'If (Not String.IsNullOrEmpty(strTmp) AndAlso Not String.IsNullOrWhiteSpace(strTmp)) Then
                SuffixCBox.Items.Add(strTmp)
                'End If
            Next
            SuffixCBox.Text = m_pGeneratorFormModel.pSuffixArr(0)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub DirScan(ByVal _dirPath As String)

        For Each fff As String In Directory.GetFiles(_dirPath)
            If Path.GetExtension(fff).ToLower() = ".config" Then
                Path.GetFileName(fff)
                m_pGeneratorFormModel.pFileList.Add(fff)
            End If
        Next
    End Sub

    Private Sub SettingFileTBox_Leave(sender As Object, e As EventArgs) Handles SettingFileTBox.Leave
        Try
            m_pAppSetting.SetMySettingValues("SettingFile", SettingFileTBox.Text, SysConst.MYAPPSETTINGPATH)
            m_pAppSetting.LoadSettingFile(m_pGeneratorFormModel, folderScanPath.Text, SettingFileTBox.Text)
            InitSuffixCbox()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub folderScanPath_Leave(sender As Object, e As EventArgs) Handles folderScanPath.Leave
        m_pAppSetting.SetMySettingValues("ProjectFolder", folderScanPath.Text, SysConst.MYAPPSETTINGPATH)
    End Sub

    Private Sub buildTargetBrowseBtn_Leave(sender As Object, e As EventArgs) Handles buildTargetBrowseBtn.Leave
        m_pAppSetting.SetMySettingValues("BuildTargetPath", buildTargetTBox.Text, SysConst.MYAPPSETTINGPATH)
    End Sub

    Private Sub EditorBtn_Click(sender As Object, e As EventArgs) Handles EditorBtn.Click
        Try
            Dim tmpParamEditForm = New ParamEditForm(m_pConfigFileViewModel, m_pConnectionModelList, m_pConnectionStringParamList, m_pGeneratorFormModel)
            If tmpParamEditForm.ShowDialog() = System.Windows.Forms.DialogResult.OK Then
                m_pConnectionModelList = tmpParamEditForm.pConnectionStringModelList
            Else

            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub applyChangesBtn_Click(sender As Object, e As EventArgs) Handles applyChangesBtn.Click
        If (m_pConnectionModelList Is Nothing Or Not (m_pConnectionModelList.Count > 0)) Then
            MessageBox.Show("No Changes Saved." + Environment.NewLine + "Please use the ""Web Config Editor"" to apply the changes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        ElseIf (Not (ConfigFileLView.Items.Count > 0)) Then
            MessageBox.Show("No Config File For Changes.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Dim pApplyChangesForm = New ApplyChangesForm(m_pGeneratorFormModel, m_pConnectionModelList)
        If pApplyChangesForm.ShowDialog() = System.Windows.Forms.DialogResult.OK Then

        End If
    End Sub

    Private Sub ClearCFLBoxBtn_Click(sender As Object, e As EventArgs) Handles ClearCFLBoxBtn.Click
        ConfigFileLView.Items.Clear()
        m_pConnectionModelList.clear()

        m_pGeneratorFormModel = New GeneratorFormModel

        EditorBtn.Enabled = False
        applyChangesBtn.Enabled = False
        ClearCFLBoxBtn.Enabled = False
        GenScanFileBtn.Enabled = True

        SettingFileTBox.Enabled = True
        folderScanPath.Enabled = True
        SuffixCBox.Enabled = True
        buildTargetTBox.Enabled = True

        SettingFileBtn.Enabled = True
        BrowseScanFolderBtn.Enabled = True
        buildTargetBrowseBtn.Enabled = True
    End Sub

    Private Sub DecryptBtn_Click(sender As Object, e As EventArgs) Handles DecryptBtn.Click
        Dim gKey As String = String.Empty
        Dim gstrSource As String = Me.DecryptMsgBox.Text
        Dim pFeService = New FE_Symmetric
        Dim strTmp As String

        gKey = Me.DecryptKeyBox.Text
        gKey = pFeService.GetOriginalKey(DecryptKeyBox.Text)
        If gKey = "" Or gstrSource = "" Then
            DecryptResultBox.Text = "Text to be Encrypted / Encryption Key cannot be empty"
            Exit Sub
        End If
        strTmp = pFeService.DecryptData(gKey, gstrSource)
        DecryptResultBox.Text = strTmp

        If (FillEncryptionPartCBox.Checked) Then
            Dim pParamDic = HelperFunctions.ConnectionStringSpliter(DecryptResultBox.Text)
            Dim tmpRefString As String = Nothing

            If (pParamDic.TryGetValue(SysConst.DB_CONNSTR_USERID, tmpRefString)) Then
                txtUID.Text = tmpRefString
            End If

            If (pParamDic.TryGetValue(SysConst.DB_CONNSTR_PWD, tmpRefString)) Then
                txtPWD.Text = tmpRefString
            End If

            If (pParamDic.TryGetValue(SysConst.DB_CONNSTR_DATASOURCE, tmpRefString)) Then
                txtSvrName.Text = tmpRefString
            End If

            If (pParamDic.TryGetValue(SysConst.DB_CONNSTR_INITCATEGORY, tmpRefString)) Then
                txtDBName.Text = tmpRefString
            End If

            If (pParamDic.TryGetValue(SysConst.DB_CONNSTR_MAXPOOLSIZE, tmpRefString)) Then
                txtConnSize.Text = tmpRefString
            End If
        End If
    End Sub

    Private Sub Encryptbtn_Click(sender As Object, e As EventArgs) Handles Encryptbtn.Click
        Dim gKey As String = txtKey.Text
        Dim dbSrc As String = txtSvrName.Text
        Dim dbID As String = txtUID.Text
        Dim dbPwd As String = txtPWD.Text
        Dim dbCat As String = txtDBName.Text
        Dim dbConnSize As String = txtConnSize.Text
        Dim dbStr As String = "data source=" & dbSrc & "; initial catalog=" & dbCat & "; persist security info=False; user id=" & dbID & "; password=" & dbPwd & "; packet size=4096; max pool size=" & dbConnSize


        Dim pFeService = New FE_Symmetric
        txtResult.Text = pFeService.EncryptData(m_pGeneratorFormModel.strEncryptedKey, dbStr)
        Me.txtEncryptKey.Text = pFeService.ScrambleKey(gKey)
    End Sub

    Private Sub ClearEnBtn_Click(sender As Object, e As EventArgs) Handles ClearEnBtn.Click
        txtSvrName.Text = String.Empty
        txtUID.Text = String.Empty
        txtPWD.Text = String.Empty
        txtDBName.Text = String.Empty
        txtConnSize.Text = String.Empty
        txtResult.Text = String.Empty
        txtEncryptKey.Text = String.Empty
    End Sub

    Private Sub buildTargetTBox_Leave(sender As Object, e As EventArgs) Handles buildTargetTBox.Leave
        Try
            m_pAppSetting.SetMySettingValues("BuildTargetPath", buildTargetTBox.Text, SysConst.MYAPPSETTINGPATH)
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ConfigFileLView_DoubleClick(sender As Object, e As EventArgs) Handles ConfigFileLView.DoubleClick
        If Not String.IsNullOrEmpty(ConfigFileLView.SelectedItems(0).Text) Then
            Try
                Dim tmpConfigEditorForm = New ConfigViewForm(m_pConfigFileViewModel, m_pGeneratorFormModel, ConfigFileLView.SelectedItems(0).Text)
                tmpConfigEditorForm.ShowDialog()
            Catch ex As Exception
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub ClearBtn_Click(sender As Object, e As EventArgs) Handles ClearBtn.Click
        DecryptMsgBox.Text = String.Empty
        DecryptResultBox.Text = String.Empty
    End Sub
End Class
