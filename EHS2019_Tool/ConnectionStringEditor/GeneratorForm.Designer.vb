<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GeneratorForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.folderScanPath = New System.Windows.Forms.TextBox()
        Me.ScanLbl = New System.Windows.Forms.Label()
        Me.GenScanFileBtn = New System.Windows.Forms.Button()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.ExistConfig = New System.Windows.Forms.TabPage()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.SplitContainer2 = New System.Windows.Forms.SplitContainer()
        Me.AppSettingGBox = New System.Windows.Forms.GroupBox()
        Me.ClearCFLBoxBtn = New System.Windows.Forms.Button()
        Me.BrowseScanFolderBtn = New System.Windows.Forms.Button()
        Me.buildTargetBrowseBtn = New System.Windows.Forms.Button()
        Me.SettingFileLbl = New System.Windows.Forms.Label()
        Me.buildTargetTBox = New System.Windows.Forms.TextBox()
        Me.SettingFileTBox = New System.Windows.Forms.TextBox()
        Me.buildTargetLbl = New System.Windows.Forms.Label()
        Me.SettingFileBtn = New System.Windows.Forms.Button()
        Me.SuffixLbl = New System.Windows.Forms.Label()
        Me.SuffixCBox = New System.Windows.Forms.ComboBox()
        Me.ConfigFileListGBox = New System.Windows.Forms.GroupBox()
        Me.configFileListPBar = New System.Windows.Forms.ProgressBar()
        Me.ConfigFileLView = New System.Windows.Forms.ListView()
        Me.FilePathCol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.EditGBox = New System.Windows.Forms.GroupBox()
        Me.applyChangesBtn = New System.Windows.Forms.Button()
        Me.EditorBtn = New System.Windows.Forms.Button()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabControl3 = New System.Windows.Forms.TabControl()
        Me.TabPage4 = New System.Windows.Forms.TabPage()
        Me.FillEncryptionPartCBox = New System.Windows.Forms.CheckBox()
        Me.ClearBtn = New System.Windows.Forms.Button()
        Me.DecryptBtn = New System.Windows.Forms.Button()
        Me.DecryptResultBox = New System.Windows.Forms.RichTextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.DecryptKeyBox = New System.Windows.Forms.RichTextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.DecryptMsgBox = New System.Windows.Forms.RichTextBox()
        Me.TabPage5 = New System.Windows.Forms.TabPage()
        Me.txtEncryptKey = New System.Windows.Forms.RichTextBox()
        Me.txtResult = New System.Windows.Forms.RichTextBox()
        Me.txtKey = New System.Windows.Forms.TextBox()
        Me.txtConnSize = New System.Windows.Forms.TextBox()
        Me.txtDBName = New System.Windows.Forms.TextBox()
        Me.txtSvrName = New System.Windows.Forms.TextBox()
        Me.txtPWD = New System.Windows.Forms.TextBox()
        Me.txtUID = New System.Windows.Forms.TextBox()
        Me.ClearEnBtn = New System.Windows.Forms.Button()
        Me.Encryptbtn = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.TabControl1.SuspendLayout()
        Me.ExistConfig.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer2.Panel1.SuspendLayout()
        Me.SplitContainer2.Panel2.SuspendLayout()
        Me.SplitContainer2.SuspendLayout()
        Me.AppSettingGBox.SuspendLayout()
        Me.ConfigFileListGBox.SuspendLayout()
        Me.EditGBox.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.TabControl3.SuspendLayout()
        Me.TabPage4.SuspendLayout()
        Me.TabPage5.SuspendLayout()
        Me.SuspendLayout()
        '
        'folderScanPath
        '
        Me.folderScanPath.Location = New System.Drawing.Point(198, 69)
        Me.folderScanPath.Name = "folderScanPath"
        Me.folderScanPath.Size = New System.Drawing.Size(405, 20)
        Me.folderScanPath.TabIndex = 0
        '
        'ScanLbl
        '
        Me.ScanLbl.AutoSize = True
        Me.ScanLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.ScanLbl.Location = New System.Drawing.Point(97, 69)
        Me.ScanLbl.Name = "ScanLbl"
        Me.ScanLbl.Size = New System.Drawing.Size(95, 20)
        Me.ScanLbl.TabIndex = 1
        Me.ScanLbl.Text = "Folder Scan"
        '
        'GenScanFileBtn
        '
        Me.GenScanFileBtn.Location = New System.Drawing.Point(233, 198)
        Me.GenScanFileBtn.Name = "GenScanFileBtn"
        Me.GenScanFileBtn.Size = New System.Drawing.Size(89, 23)
        Me.GenScanFileBtn.TabIndex = 2
        Me.GenScanFileBtn.Text = "Generate"
        Me.GenScanFileBtn.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.ExistConfig)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(752, 715)
        Me.TabControl1.TabIndex = 3
        '
        'ExistConfig
        '
        Me.ExistConfig.Controls.Add(Me.Panel1)
        Me.ExistConfig.Location = New System.Drawing.Point(4, 22)
        Me.ExistConfig.Name = "ExistConfig"
        Me.ExistConfig.Padding = New System.Windows.Forms.Padding(3)
        Me.ExistConfig.Size = New System.Drawing.Size(744, 689)
        Me.ExistConfig.TabIndex = 0
        Me.ExistConfig.Text = "Config File"
        Me.ExistConfig.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.SplitContainer2)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(3, 3)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(738, 683)
        Me.Panel1.TabIndex = 3
        '
        'SplitContainer2
        '
        Me.SplitContainer2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer2.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer2.Name = "SplitContainer2"
        Me.SplitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer2.Panel1
        '
        Me.SplitContainer2.Panel1.AutoScroll = True
        Me.SplitContainer2.Panel1.Controls.Add(Me.AppSettingGBox)
        '
        'SplitContainer2.Panel2
        '
        Me.SplitContainer2.Panel2.AutoScroll = True
        Me.SplitContainer2.Panel2.Controls.Add(Me.ConfigFileListGBox)
        Me.SplitContainer2.Panel2.Controls.Add(Me.EditGBox)
        Me.SplitContainer2.Size = New System.Drawing.Size(736, 681)
        Me.SplitContainer2.SplitterDistance = 270
        Me.SplitContainer2.TabIndex = 19
        '
        'AppSettingGBox
        '
        Me.AppSettingGBox.Controls.Add(Me.folderScanPath)
        Me.AppSettingGBox.Controls.Add(Me.GenScanFileBtn)
        Me.AppSettingGBox.Controls.Add(Me.ScanLbl)
        Me.AppSettingGBox.Controls.Add(Me.ClearCFLBoxBtn)
        Me.AppSettingGBox.Controls.Add(Me.BrowseScanFolderBtn)
        Me.AppSettingGBox.Controls.Add(Me.buildTargetBrowseBtn)
        Me.AppSettingGBox.Controls.Add(Me.SettingFileLbl)
        Me.AppSettingGBox.Controls.Add(Me.buildTargetTBox)
        Me.AppSettingGBox.Controls.Add(Me.SettingFileTBox)
        Me.AppSettingGBox.Controls.Add(Me.buildTargetLbl)
        Me.AppSettingGBox.Controls.Add(Me.SettingFileBtn)
        Me.AppSettingGBox.Controls.Add(Me.SuffixLbl)
        Me.AppSettingGBox.Controls.Add(Me.SuffixCBox)
        Me.AppSettingGBox.Location = New System.Drawing.Point(19, 14)
        Me.AppSettingGBox.Name = "AppSettingGBox"
        Me.AppSettingGBox.Size = New System.Drawing.Size(701, 229)
        Me.AppSettingGBox.TabIndex = 16
        Me.AppSettingGBox.TabStop = False
        Me.AppSettingGBox.Text = "Application Setting"
        '
        'ClearCFLBoxBtn
        '
        Me.ClearCFLBoxBtn.Enabled = False
        Me.ClearCFLBoxBtn.Location = New System.Drawing.Point(430, 198)
        Me.ClearCFLBoxBtn.Name = "ClearCFLBoxBtn"
        Me.ClearCFLBoxBtn.Size = New System.Drawing.Size(75, 23)
        Me.ClearCFLBoxBtn.TabIndex = 13
        Me.ClearCFLBoxBtn.Text = "Clear"
        Me.ClearCFLBoxBtn.UseVisualStyleBackColor = True
        '
        'BrowseScanFolderBtn
        '
        Me.BrowseScanFolderBtn.Location = New System.Drawing.Point(609, 66)
        Me.BrowseScanFolderBtn.Name = "BrowseScanFolderBtn"
        Me.BrowseScanFolderBtn.Size = New System.Drawing.Size(75, 23)
        Me.BrowseScanFolderBtn.TabIndex = 3
        Me.BrowseScanFolderBtn.Text = "Browse"
        Me.BrowseScanFolderBtn.UseVisualStyleBackColor = True
        '
        'buildTargetBrowseBtn
        '
        Me.buildTargetBrowseBtn.Location = New System.Drawing.Point(609, 150)
        Me.buildTargetBrowseBtn.Name = "buildTargetBrowseBtn"
        Me.buildTargetBrowseBtn.Size = New System.Drawing.Size(75, 23)
        Me.buildTargetBrowseBtn.TabIndex = 11
        Me.buildTargetBrowseBtn.Text = "Browse"
        Me.buildTargetBrowseBtn.UseVisualStyleBackColor = True
        '
        'SettingFileLbl
        '
        Me.SettingFileLbl.AutoSize = True
        Me.SettingFileLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.SettingFileLbl.Location = New System.Drawing.Point(13, 24)
        Me.SettingFileLbl.Name = "SettingFileLbl"
        Me.SettingFileLbl.Size = New System.Drawing.Size(178, 20)
        Me.SettingFileLbl.TabIndex = 5
        Me.SettingFileLbl.Text = "Setting File of Packager"
        '
        'buildTargetTBox
        '
        Me.buildTargetTBox.Location = New System.Drawing.Point(198, 153)
        Me.buildTargetTBox.Name = "buildTargetTBox"
        Me.buildTargetTBox.Size = New System.Drawing.Size(405, 20)
        Me.buildTargetTBox.TabIndex = 9
        '
        'SettingFileTBox
        '
        Me.SettingFileTBox.Location = New System.Drawing.Point(198, 24)
        Me.SettingFileTBox.Name = "SettingFileTBox"
        Me.SettingFileTBox.Size = New System.Drawing.Size(405, 20)
        Me.SettingFileTBox.TabIndex = 4
        '
        'buildTargetLbl
        '
        Me.buildTargetLbl.AutoSize = True
        Me.buildTargetLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.buildTargetLbl.Location = New System.Drawing.Point(97, 153)
        Me.buildTargetLbl.Name = "buildTargetLbl"
        Me.buildTargetLbl.Size = New System.Drawing.Size(94, 20)
        Me.buildTargetLbl.TabIndex = 10
        Me.buildTargetLbl.Text = "Build Target"
        '
        'SettingFileBtn
        '
        Me.SettingFileBtn.Location = New System.Drawing.Point(609, 21)
        Me.SettingFileBtn.Name = "SettingFileBtn"
        Me.SettingFileBtn.Size = New System.Drawing.Size(75, 23)
        Me.SettingFileBtn.TabIndex = 6
        Me.SettingFileBtn.Text = "Setting File"
        Me.SettingFileBtn.UseVisualStyleBackColor = True
        '
        'SuffixLbl
        '
        Me.SuffixLbl.AutoSize = True
        Me.SuffixLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.SuffixLbl.Location = New System.Drawing.Point(97, 109)
        Me.SuffixLbl.Name = "SuffixLbl"
        Me.SuffixLbl.Size = New System.Drawing.Size(49, 20)
        Me.SuffixLbl.TabIndex = 8
        Me.SuffixLbl.Text = "Suffix"
        '
        'SuffixCBox
        '
        Me.SuffixCBox.FormattingEnabled = True
        Me.SuffixCBox.Location = New System.Drawing.Point(198, 111)
        Me.SuffixCBox.Name = "SuffixCBox"
        Me.SuffixCBox.Size = New System.Drawing.Size(106, 21)
        Me.SuffixCBox.TabIndex = 7
        '
        'ConfigFileListGBox
        '
        Me.ConfigFileListGBox.Controls.Add(Me.configFileListPBar)
        Me.ConfigFileListGBox.Controls.Add(Me.ConfigFileLView)
        Me.ConfigFileListGBox.Location = New System.Drawing.Point(167, 15)
        Me.ConfigFileListGBox.Name = "ConfigFileListGBox"
        Me.ConfigFileListGBox.Size = New System.Drawing.Size(553, 367)
        Me.ConfigFileListGBox.TabIndex = 15
        Me.ConfigFileListGBox.TabStop = False
        Me.ConfigFileListGBox.Text = "Config File List"
        '
        'configFileListPBar
        '
        Me.configFileListPBar.Location = New System.Drawing.Point(166, 127)
        Me.configFileListPBar.Name = "configFileListPBar"
        Me.configFileListPBar.Size = New System.Drawing.Size(241, 23)
        Me.configFileListPBar.TabIndex = 1
        Me.configFileListPBar.Visible = False
        '
        'ConfigFileLView
        '
        Me.ConfigFileLView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.FilePathCol})
        Me.ConfigFileLView.FullRowSelect = True
        Me.ConfigFileLView.GridLines = True
        Me.ConfigFileLView.Location = New System.Drawing.Point(7, 20)
        Me.ConfigFileLView.MultiSelect = False
        Me.ConfigFileLView.Name = "ConfigFileLView"
        Me.ConfigFileLView.Size = New System.Drawing.Size(529, 332)
        Me.ConfigFileLView.TabIndex = 0
        Me.ConfigFileLView.UseCompatibleStateImageBehavior = False
        Me.ConfigFileLView.View = System.Windows.Forms.View.Details
        '
        'FilePathCol
        '
        Me.FilePathCol.Text = "File Path"
        Me.FilePathCol.Width = 1500
        '
        'EditGBox
        '
        Me.EditGBox.Controls.Add(Me.applyChangesBtn)
        Me.EditGBox.Controls.Add(Me.EditorBtn)
        Me.EditGBox.Location = New System.Drawing.Point(19, 15)
        Me.EditGBox.Name = "EditGBox"
        Me.EditGBox.Size = New System.Drawing.Size(127, 365)
        Me.EditGBox.TabIndex = 17
        Me.EditGBox.TabStop = False
        Me.EditGBox.Text = "Editor List"
        '
        'applyChangesBtn
        '
        Me.applyChangesBtn.Enabled = False
        Me.applyChangesBtn.Location = New System.Drawing.Point(7, 60)
        Me.applyChangesBtn.Name = "applyChangesBtn"
        Me.applyChangesBtn.Size = New System.Drawing.Size(114, 23)
        Me.applyChangesBtn.TabIndex = 1
        Me.applyChangesBtn.Text = "Apply Changes"
        Me.applyChangesBtn.UseVisualStyleBackColor = True
        '
        'EditorBtn
        '
        Me.EditorBtn.Enabled = False
        Me.EditorBtn.Location = New System.Drawing.Point(7, 30)
        Me.EditorBtn.Name = "EditorBtn"
        Me.EditorBtn.Size = New System.Drawing.Size(114, 23)
        Me.EditorBtn.TabIndex = 0
        Me.EditorBtn.Text = "Web Config Editor"
        Me.EditorBtn.UseVisualStyleBackColor = True
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.TabControl3)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(744, 689)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "Single Parameter"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabControl3
        '
        Me.TabControl3.Controls.Add(Me.TabPage4)
        Me.TabControl3.Controls.Add(Me.TabPage5)
        Me.TabControl3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl3.Location = New System.Drawing.Point(3, 3)
        Me.TabControl3.Name = "TabControl3"
        Me.TabControl3.SelectedIndex = 0
        Me.TabControl3.Size = New System.Drawing.Size(738, 683)
        Me.TabControl3.TabIndex = 5
        '
        'TabPage4
        '
        Me.TabPage4.AutoScroll = True
        Me.TabPage4.BackColor = System.Drawing.Color.SkyBlue
        Me.TabPage4.Controls.Add(Me.FillEncryptionPartCBox)
        Me.TabPage4.Controls.Add(Me.ClearBtn)
        Me.TabPage4.Controls.Add(Me.DecryptBtn)
        Me.TabPage4.Controls.Add(Me.DecryptResultBox)
        Me.TabPage4.Controls.Add(Me.Label3)
        Me.TabPage4.Controls.Add(Me.Label1)
        Me.TabPage4.Controls.Add(Me.DecryptKeyBox)
        Me.TabPage4.Controls.Add(Me.Label2)
        Me.TabPage4.Controls.Add(Me.DecryptMsgBox)
        Me.TabPage4.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.TabPage4.Location = New System.Drawing.Point(4, 22)
        Me.TabPage4.Name = "TabPage4"
        Me.TabPage4.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage4.Size = New System.Drawing.Size(730, 657)
        Me.TabPage4.TabIndex = 0
        Me.TabPage4.Text = "Decryption"
        '
        'FillEncryptionPartCBox
        '
        Me.FillEncryptionPartCBox.AutoSize = True
        Me.FillEncryptionPartCBox.Location = New System.Drawing.Point(60, 333)
        Me.FillEncryptionPartCBox.Name = "FillEncryptionPartCBox"
        Me.FillEncryptionPartCBox.Size = New System.Drawing.Size(318, 17)
        Me.FillEncryptionPartCBox.TabIndex = 8
        Me.FillEncryptionPartCBox.Text = "Fill in the Connection Parameters to the Encryption "
        Me.FillEncryptionPartCBox.UseVisualStyleBackColor = True
        '
        'ClearBtn
        '
        Me.ClearBtn.Location = New System.Drawing.Point(166, 369)
        Me.ClearBtn.Name = "ClearBtn"
        Me.ClearBtn.Size = New System.Drawing.Size(75, 23)
        Me.ClearBtn.TabIndex = 7
        Me.ClearBtn.Text = "Clear"
        Me.ClearBtn.UseVisualStyleBackColor = True
        '
        'DecryptBtn
        '
        Me.DecryptBtn.Location = New System.Drawing.Point(60, 369)
        Me.DecryptBtn.Name = "DecryptBtn"
        Me.DecryptBtn.Size = New System.Drawing.Size(75, 23)
        Me.DecryptBtn.TabIndex = 6
        Me.DecryptBtn.Text = "Decrypt"
        Me.DecryptBtn.UseVisualStyleBackColor = True
        '
        'DecryptResultBox
        '
        Me.DecryptResultBox.Location = New System.Drawing.Point(60, 255)
        Me.DecryptResultBox.Name = "DecryptResultBox"
        Me.DecryptResultBox.Size = New System.Drawing.Size(558, 72)
        Me.DecryptResultBox.TabIndex = 5
        Me.DecryptResultBox.Text = ""
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label3.Location = New System.Drawing.Point(6, 232)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(148, 20)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Decrypted Result"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label1.Location = New System.Drawing.Point(6, 12)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(148, 20)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Encrypted String "
        '
        'DecryptKeyBox
        '
        Me.DecryptKeyBox.Location = New System.Drawing.Point(60, 144)
        Me.DecryptKeyBox.Name = "DecryptKeyBox"
        Me.DecryptKeyBox.Size = New System.Drawing.Size(558, 72)
        Me.DecryptKeyBox.TabIndex = 2
        Me.DecryptKeyBox.Text = "RVZT"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label2.Location = New System.Drawing.Point(6, 119)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 20)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Encryption Key"
        '
        'DecryptMsgBox
        '
        Me.DecryptMsgBox.Location = New System.Drawing.Point(60, 35)
        Me.DecryptMsgBox.Name = "DecryptMsgBox"
        Me.DecryptMsgBox.Size = New System.Drawing.Size(558, 64)
        Me.DecryptMsgBox.TabIndex = 0
        Me.DecryptMsgBox.Text = ""
        '
        'TabPage5
        '
        Me.TabPage5.AutoScroll = True
        Me.TabPage5.BackColor = System.Drawing.Color.SkyBlue
        Me.TabPage5.Controls.Add(Me.txtEncryptKey)
        Me.TabPage5.Controls.Add(Me.txtResult)
        Me.TabPage5.Controls.Add(Me.txtKey)
        Me.TabPage5.Controls.Add(Me.txtConnSize)
        Me.TabPage5.Controls.Add(Me.txtDBName)
        Me.TabPage5.Controls.Add(Me.txtSvrName)
        Me.TabPage5.Controls.Add(Me.txtPWD)
        Me.TabPage5.Controls.Add(Me.txtUID)
        Me.TabPage5.Controls.Add(Me.ClearEnBtn)
        Me.TabPage5.Controls.Add(Me.Encryptbtn)
        Me.TabPage5.Controls.Add(Me.Label11)
        Me.TabPage5.Controls.Add(Me.Label10)
        Me.TabPage5.Controls.Add(Me.Label9)
        Me.TabPage5.Controls.Add(Me.Label8)
        Me.TabPage5.Controls.Add(Me.Label7)
        Me.TabPage5.Controls.Add(Me.Label6)
        Me.TabPage5.Controls.Add(Me.Label5)
        Me.TabPage5.Controls.Add(Me.Label4)
        Me.TabPage5.Location = New System.Drawing.Point(4, 22)
        Me.TabPage5.Name = "TabPage5"
        Me.TabPage5.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage5.Size = New System.Drawing.Size(730, 657)
        Me.TabPage5.TabIndex = 1
        Me.TabPage5.Text = "Encryption"
        '
        'txtEncryptKey
        '
        Me.txtEncryptKey.Location = New System.Drawing.Point(90, 516)
        Me.txtEncryptKey.Name = "txtEncryptKey"
        Me.txtEncryptKey.Size = New System.Drawing.Size(534, 93)
        Me.txtEncryptKey.TabIndex = 19
        Me.txtEncryptKey.Text = ""
        '
        'txtResult
        '
        Me.txtResult.Location = New System.Drawing.Point(90, 408)
        Me.txtResult.Name = "txtResult"
        Me.txtResult.Size = New System.Drawing.Size(534, 82)
        Me.txtResult.TabIndex = 18
        Me.txtResult.Text = ""
        '
        'txtKey
        '
        Me.txtKey.Location = New System.Drawing.Point(90, 295)
        Me.txtKey.Name = "txtKey"
        Me.txtKey.Size = New System.Drawing.Size(534, 20)
        Me.txtKey.TabIndex = 17
        Me.txtKey.Text = "EVS"
        '
        'txtConnSize
        '
        Me.txtConnSize.Location = New System.Drawing.Point(90, 244)
        Me.txtConnSize.Name = "txtConnSize"
        Me.txtConnSize.Size = New System.Drawing.Size(534, 20)
        Me.txtConnSize.TabIndex = 16
        '
        'txtDBName
        '
        Me.txtDBName.Location = New System.Drawing.Point(90, 193)
        Me.txtDBName.Name = "txtDBName"
        Me.txtDBName.Size = New System.Drawing.Size(534, 20)
        Me.txtDBName.TabIndex = 15
        '
        'txtSvrName
        '
        Me.txtSvrName.Location = New System.Drawing.Point(90, 142)
        Me.txtSvrName.Name = "txtSvrName"
        Me.txtSvrName.Size = New System.Drawing.Size(534, 20)
        Me.txtSvrName.TabIndex = 14
        '
        'txtPWD
        '
        Me.txtPWD.Location = New System.Drawing.Point(90, 91)
        Me.txtPWD.Name = "txtPWD"
        Me.txtPWD.Size = New System.Drawing.Size(534, 20)
        Me.txtPWD.TabIndex = 13
        '
        'txtUID
        '
        Me.txtUID.Location = New System.Drawing.Point(90, 40)
        Me.txtUID.Name = "txtUID"
        Me.txtUID.Size = New System.Drawing.Size(534, 20)
        Me.txtUID.TabIndex = 12
        '
        'ClearEnBtn
        '
        Me.ClearEnBtn.Location = New System.Drawing.Point(129, 332)
        Me.ClearEnBtn.Name = "ClearEnBtn"
        Me.ClearEnBtn.Size = New System.Drawing.Size(75, 23)
        Me.ClearEnBtn.TabIndex = 11
        Me.ClearEnBtn.Text = "Clear"
        Me.ClearEnBtn.UseVisualStyleBackColor = True
        '
        'Encryptbtn
        '
        Me.Encryptbtn.Location = New System.Drawing.Point(29, 332)
        Me.Encryptbtn.Name = "Encryptbtn"
        Me.Encryptbtn.Size = New System.Drawing.Size(75, 23)
        Me.Encryptbtn.TabIndex = 10
        Me.Encryptbtn.Text = "Encrypt"
        Me.Encryptbtn.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label11.Location = New System.Drawing.Point(17, 68)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(86, 20)
        Me.Label11.TabIndex = 9
        Me.Label11.Text = "Password"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label10.Location = New System.Drawing.Point(20, 119)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(211, 20)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Database Server Source "
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label9.Location = New System.Drawing.Point(20, 170)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(126, 20)
        Me.Label9.TabIndex = 7
        Me.Label9.Text = "Inital Catalog  "
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label8.Location = New System.Drawing.Point(20, 221)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(220, 20)
        Me.Label8.TabIndex = 6
        Me.Label8.Text = "Max Database Connection"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label7.Location = New System.Drawing.Point(20, 272)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(128, 20)
        Me.Label7.TabIndex = 5
        Me.Label7.Text = "Encryption Key"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label6.Location = New System.Drawing.Point(17, 385)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(147, 20)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Encrypted Result"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label5.Location = New System.Drawing.Point(17, 493)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(134, 20)
        Me.Label5.TabIndex = 3
        Me.Label5.Text = "Decryption Key "
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label4.Location = New System.Drawing.Point(17, 17)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(103, 20)
        Me.Label4.TabIndex = 2
        Me.Label4.Text = "User Name "
        '
        'GeneratorForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(752, 715)
        Me.Controls.Add(Me.TabControl1)
        Me.Name = "GeneratorForm"
        Me.Text = "Connection String Editor"
        Me.TabControl1.ResumeLayout(False)
        Me.ExistConfig.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel1.ResumeLayout(False)
        Me.SplitContainer2.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer2.ResumeLayout(False)
        Me.AppSettingGBox.ResumeLayout(False)
        Me.AppSettingGBox.PerformLayout()
        Me.ConfigFileListGBox.ResumeLayout(False)
        Me.EditGBox.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.TabControl3.ResumeLayout(False)
        Me.TabPage4.ResumeLayout(False)
        Me.TabPage4.PerformLayout()
        Me.TabPage5.ResumeLayout(False)
        Me.TabPage5.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents folderScanPath As System.Windows.Forms.TextBox
    Friend WithEvents ScanLbl As System.Windows.Forms.Label
    Friend WithEvents GenScanFileBtn As System.Windows.Forms.Button
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents ExistConfig As System.Windows.Forms.TabPage
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents BrowseScanFolderBtn As System.Windows.Forms.Button
    Friend WithEvents SettingFileBtn As System.Windows.Forms.Button
    Friend WithEvents SettingFileTBox As System.Windows.Forms.TextBox
    Friend WithEvents SettingFileLbl As System.Windows.Forms.Label
    Friend WithEvents SuffixCBox As System.Windows.Forms.ComboBox
    Friend WithEvents SuffixLbl As System.Windows.Forms.Label
    Friend WithEvents buildTargetBrowseBtn As System.Windows.Forms.Button
    Friend WithEvents buildTargetTBox As System.Windows.Forms.TextBox
    Friend WithEvents buildTargetLbl As System.Windows.Forms.Label
    Friend WithEvents ClearCFLBoxBtn As System.Windows.Forms.Button
    Friend WithEvents ConfigFileListGBox As System.Windows.Forms.GroupBox
    Friend WithEvents AppSettingGBox As System.Windows.Forms.GroupBox
    Friend WithEvents TabControl3 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage4 As System.Windows.Forms.TabPage
    Friend WithEvents ClearBtn As System.Windows.Forms.Button
    Friend WithEvents DecryptBtn As System.Windows.Forms.Button
    Friend WithEvents DecryptResultBox As System.Windows.Forms.RichTextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents DecryptKeyBox As System.Windows.Forms.RichTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents DecryptMsgBox As System.Windows.Forms.RichTextBox
    Friend WithEvents TabPage5 As System.Windows.Forms.TabPage
    Friend WithEvents txtEncryptKey As System.Windows.Forms.RichTextBox
    Friend WithEvents txtResult As System.Windows.Forms.RichTextBox
    Friend WithEvents txtKey As System.Windows.Forms.TextBox
    Friend WithEvents txtConnSize As System.Windows.Forms.TextBox
    Friend WithEvents txtDBName As System.Windows.Forms.TextBox
    Friend WithEvents txtSvrName As System.Windows.Forms.TextBox
    Friend WithEvents txtPWD As System.Windows.Forms.TextBox
    Friend WithEvents txtUID As System.Windows.Forms.TextBox
    Friend WithEvents ClearEnBtn As System.Windows.Forms.Button
    Friend WithEvents Encryptbtn As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents EditGBox As System.Windows.Forms.GroupBox
    Friend WithEvents EditorBtn As System.Windows.Forms.Button
    Friend WithEvents applyChangesBtn As System.Windows.Forms.Button
    Friend WithEvents ConfigFileLView As System.Windows.Forms.ListView
    Friend WithEvents FilePathCol As System.Windows.Forms.ColumnHeader
    Friend WithEvents SplitContainer2 As System.Windows.Forms.SplitContainer
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents FillEncryptionPartCBox As System.Windows.Forms.CheckBox
    Friend WithEvents configFileListPBar As System.Windows.Forms.ProgressBar

End Class
