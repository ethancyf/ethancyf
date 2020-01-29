<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ParamEditForm
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
        Dim DataGridViewCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PoolSizeTBox = New System.Windows.Forms.TextBox()
        Me.DBNameTBox = New System.Windows.Forms.TextBox()
        Me.DSTBox = New System.Windows.Forms.TextBox()
        Me.PwdTBox = New System.Windows.Forms.TextBox()
        Me.UserIDTBox = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.SaveBtn = New System.Windows.Forms.Button()
        Me.CancelBtn = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ApplytoAllGBox = New System.Windows.Forms.GroupBox()
        Me.enablePoolSizeCBox = New System.Windows.Forms.CheckBox()
        Me.enableDBNameCBox = New System.Windows.Forms.CheckBox()
        Me.enableDSCBox = New System.Windows.Forms.CheckBox()
        Me.enablePwdCBox = New System.Windows.Forms.CheckBox()
        Me.enableUserIDCBox = New System.Windows.Forms.CheckBox()
        Me.applyToAllBtn = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.DBCheckPBar = New System.Windows.Forms.ProgressBar()
        Me.parameterDGView = New System.Windows.Forms.DataGridView()
        Me.Verify = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.paramName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.applyToAllCBox = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.username = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pwd = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.datasource = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.DatabaseName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.maxPoolSize = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.decryptedValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.encryptedValue = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.EnableApplyToAllCBox = New System.Windows.Forms.CheckBox()
        Me.verifyAllCBox = New System.Windows.Forms.CheckBox()
        Me.AutoDataFillBtn = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ApplytoAllGBox.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        CType(Me.parameterDGView, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'PoolSizeTBox
        '
        Me.PoolSizeTBox.Enabled = False
        Me.PoolSizeTBox.Location = New System.Drawing.Point(149, 135)
        Me.PoolSizeTBox.Name = "PoolSizeTBox"
        Me.PoolSizeTBox.Size = New System.Drawing.Size(202, 20)
        Me.PoolSizeTBox.TabIndex = 34
        '
        'DBNameTBox
        '
        Me.DBNameTBox.Enabled = False
        Me.DBNameTBox.Location = New System.Drawing.Point(149, 107)
        Me.DBNameTBox.Name = "DBNameTBox"
        Me.DBNameTBox.Size = New System.Drawing.Size(202, 20)
        Me.DBNameTBox.TabIndex = 33
        '
        'DSTBox
        '
        Me.DSTBox.Enabled = False
        Me.DSTBox.Location = New System.Drawing.Point(149, 80)
        Me.DSTBox.Name = "DSTBox"
        Me.DSTBox.Size = New System.Drawing.Size(202, 20)
        Me.DSTBox.TabIndex = 32
        '
        'PwdTBox
        '
        Me.PwdTBox.Enabled = False
        Me.PwdTBox.Location = New System.Drawing.Point(149, 54)
        Me.PwdTBox.Name = "PwdTBox"
        Me.PwdTBox.Size = New System.Drawing.Size(202, 20)
        Me.PwdTBox.TabIndex = 31
        '
        'UserIDTBox
        '
        Me.UserIDTBox.Enabled = False
        Me.UserIDTBox.Location = New System.Drawing.Point(149, 28)
        Me.UserIDTBox.Name = "UserIDTBox"
        Me.UserIDTBox.Size = New System.Drawing.Size(202, 20)
        Me.UserIDTBox.TabIndex = 30
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(12, 80)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(117, 18)
        Me.Label10.TabIndex = 26
        Me.Label10.Text = "Data Source:  "
        '
        'SaveBtn
        '
        Me.SaveBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SaveBtn.Enabled = False
        Me.SaveBtn.Location = New System.Drawing.Point(421, 598)
        Me.SaveBtn.Name = "SaveBtn"
        Me.SaveBtn.Size = New System.Drawing.Size(75, 23)
        Me.SaveBtn.TabIndex = 38
        Me.SaveBtn.Text = "Save"
        Me.SaveBtn.UseVisualStyleBackColor = True
        '
        'CancelBtn
        '
        Me.CancelBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.CancelBtn.Location = New System.Drawing.Point(20, 598)
        Me.CancelBtn.Name = "CancelBtn"
        Me.CancelBtn.Size = New System.Drawing.Size(75, 23)
        Me.CancelBtn.TabIndex = 39
        Me.CancelBtn.Text = "Cancel"
        Me.CancelBtn.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 18)
        Me.Label1.TabIndex = 40
        Me.Label1.Text = "User ID:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(12, 57)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(88, 18)
        Me.Label2.TabIndex = 41
        Me.Label2.Text = "Password:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(12, 109)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(133, 18)
        Me.Label3.TabIndex = 42
        Me.Label3.Text = "Inital Catalog:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(12, 136)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(122, 18)
        Me.Label4.TabIndex = 43
        Me.Label4.Text = "Max Pool Size:"
        '
        'ApplytoAllGBox
        '
        Me.ApplytoAllGBox.Controls.Add(Me.enablePoolSizeCBox)
        Me.ApplytoAllGBox.Controls.Add(Me.enableDBNameCBox)
        Me.ApplytoAllGBox.Controls.Add(Me.enableDSCBox)
        Me.ApplytoAllGBox.Controls.Add(Me.enablePwdCBox)
        Me.ApplytoAllGBox.Controls.Add(Me.enableUserIDCBox)
        Me.ApplytoAllGBox.Controls.Add(Me.applyToAllBtn)
        Me.ApplytoAllGBox.Controls.Add(Me.PoolSizeTBox)
        Me.ApplytoAllGBox.Controls.Add(Me.Label4)
        Me.ApplytoAllGBox.Controls.Add(Me.Label10)
        Me.ApplytoAllGBox.Controls.Add(Me.Label3)
        Me.ApplytoAllGBox.Controls.Add(Me.UserIDTBox)
        Me.ApplytoAllGBox.Controls.Add(Me.Label2)
        Me.ApplytoAllGBox.Controls.Add(Me.PwdTBox)
        Me.ApplytoAllGBox.Controls.Add(Me.Label1)
        Me.ApplytoAllGBox.Controls.Add(Me.DSTBox)
        Me.ApplytoAllGBox.Controls.Add(Me.DBNameTBox)
        Me.ApplytoAllGBox.Location = New System.Drawing.Point(20, 11)
        Me.ApplytoAllGBox.Name = "ApplytoAllGBox"
        Me.ApplytoAllGBox.Size = New System.Drawing.Size(386, 200)
        Me.ApplytoAllGBox.TabIndex = 44
        Me.ApplytoAllGBox.TabStop = False
        Me.ApplytoAllGBox.Text = "Apply To all"
        '
        'enablePoolSizeCBox
        '
        Me.enablePoolSizeCBox.AutoSize = True
        Me.enablePoolSizeCBox.Location = New System.Drawing.Point(357, 138)
        Me.enablePoolSizeCBox.Name = "enablePoolSizeCBox"
        Me.enablePoolSizeCBox.Size = New System.Drawing.Size(15, 14)
        Me.enablePoolSizeCBox.TabIndex = 49
        Me.enablePoolSizeCBox.UseVisualStyleBackColor = True
        '
        'enableDBNameCBox
        '
        Me.enableDBNameCBox.AutoSize = True
        Me.enableDBNameCBox.Location = New System.Drawing.Point(357, 110)
        Me.enableDBNameCBox.Name = "enableDBNameCBox"
        Me.enableDBNameCBox.Size = New System.Drawing.Size(15, 14)
        Me.enableDBNameCBox.TabIndex = 48
        Me.enableDBNameCBox.UseVisualStyleBackColor = True
        '
        'enableDSCBox
        '
        Me.enableDSCBox.AutoSize = True
        Me.enableDSCBox.Location = New System.Drawing.Point(357, 84)
        Me.enableDSCBox.Name = "enableDSCBox"
        Me.enableDSCBox.Size = New System.Drawing.Size(15, 14)
        Me.enableDSCBox.TabIndex = 47
        Me.enableDSCBox.UseVisualStyleBackColor = True
        '
        'enablePwdCBox
        '
        Me.enablePwdCBox.AutoSize = True
        Me.enablePwdCBox.Location = New System.Drawing.Point(357, 57)
        Me.enablePwdCBox.Name = "enablePwdCBox"
        Me.enablePwdCBox.Size = New System.Drawing.Size(15, 14)
        Me.enablePwdCBox.TabIndex = 46
        Me.enablePwdCBox.UseVisualStyleBackColor = True
        '
        'enableUserIDCBox
        '
        Me.enableUserIDCBox.AutoSize = True
        Me.enableUserIDCBox.Location = New System.Drawing.Point(357, 31)
        Me.enableUserIDCBox.Name = "enableUserIDCBox"
        Me.enableUserIDCBox.Size = New System.Drawing.Size(15, 14)
        Me.enableUserIDCBox.TabIndex = 45
        Me.enableUserIDCBox.UseVisualStyleBackColor = True
        '
        'applyToAllBtn
        '
        Me.applyToAllBtn.Location = New System.Drawing.Point(15, 170)
        Me.applyToAllBtn.Name = "applyToAllBtn"
        Me.applyToAllBtn.Size = New System.Drawing.Size(75, 23)
        Me.applyToAllBtn.TabIndex = 44
        Me.applyToAllBtn.Text = "Apply To All"
        Me.applyToAllBtn.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.DBCheckPBar)
        Me.GroupBox1.Controls.Add(Me.parameterDGView)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Controls.Add(Me.verifyAllCBox)
        Me.GroupBox1.Controls.Add(Me.AutoDataFillBtn)
        Me.GroupBox1.Location = New System.Drawing.Point(20, 217)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(878, 363)
        Me.GroupBox1.TabIndex = 45
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Parameters"
        '
        'DBCheckPBar
        '
        Me.DBCheckPBar.Location = New System.Drawing.Point(240, 172)
        Me.DBCheckPBar.Name = "DBCheckPBar"
        Me.DBCheckPBar.Size = New System.Drawing.Size(370, 23)
        Me.DBCheckPBar.TabIndex = 49
        Me.DBCheckPBar.Visible = False
        '
        'parameterDGView
        '
        Me.parameterDGView.AllowUserToAddRows = False
        Me.parameterDGView.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.parameterDGView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.parameterDGView.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.Verify, Me.paramName, Me.applyToAllCBox, Me.username, Me.pwd, Me.datasource, Me.DatabaseName, Me.maxPoolSize, Me.decryptedValue, Me.encryptedValue})
        DataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle9.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        DataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle9.NullValue = "false"
        DataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.parameterDGView.DefaultCellStyle = DataGridViewCellStyle9
        Me.parameterDGView.Location = New System.Drawing.Point(15, 42)
        Me.parameterDGView.Name = "parameterDGView"
        Me.parameterDGView.RowHeadersVisible = False
        Me.parameterDGView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.parameterDGView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.[False]
        Me.parameterDGView.Size = New System.Drawing.Size(845, 301)
        Me.parameterDGView.TabIndex = 0
        '
        'Verify
        '
        Me.Verify.FalseValue = False
        Me.Verify.HeaderText = "Verified"
        Me.Verify.Name = "Verify"
        Me.Verify.TrueValue = True
        Me.Verify.Width = 50
        '
        'paramName
        '
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.paramName.DefaultCellStyle = DataGridViewCellStyle1
        Me.paramName.HeaderText = "Parameter Name"
        Me.paramName.Name = "paramName"
        Me.paramName.ReadOnly = True
        Me.paramName.Width = 300
        '
        'applyToAllCBox
        '
        Me.applyToAllCBox.FalseValue = "false"
        Me.applyToAllCBox.HeaderText = "Apply To All"
        Me.applyToAllCBox.Name = "applyToAllCBox"
        Me.applyToAllCBox.TrueValue = "true"
        Me.applyToAllCBox.Width = 85
        '
        'username
        '
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.username.DefaultCellStyle = DataGridViewCellStyle2
        Me.username.HeaderText = "Username"
        Me.username.Name = "username"
        Me.username.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.username.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.username.Width = 80
        '
        'pwd
        '
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.pwd.DefaultCellStyle = DataGridViewCellStyle3
        Me.pwd.HeaderText = "Password"
        Me.pwd.Name = "pwd"
        Me.pwd.Resizable = System.Windows.Forms.DataGridViewTriState.[True]
        Me.pwd.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable
        Me.pwd.Width = 80
        '
        'datasource
        '
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.datasource.DefaultCellStyle = DataGridViewCellStyle4
        Me.datasource.HeaderText = "Data Source"
        Me.datasource.Name = "datasource"
        Me.datasource.Width = 150
        '
        'DatabaseName
        '
        DataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.DatabaseName.DefaultCellStyle = DataGridViewCellStyle5
        Me.DatabaseName.HeaderText = "Inital Catalog"
        Me.DatabaseName.Name = "DatabaseName"
        '
        'maxPoolSize
        '
        DataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.maxPoolSize.DefaultCellStyle = DataGridViewCellStyle6
        Me.maxPoolSize.HeaderText = "Max Pool Size"
        Me.maxPoolSize.Name = "maxPoolSize"
        '
        'decryptedValue
        '
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.decryptedValue.DefaultCellStyle = DataGridViewCellStyle7
        Me.decryptedValue.HeaderText = "Decrypted Value"
        Me.decryptedValue.Name = "decryptedValue"
        Me.decryptedValue.ReadOnly = True
        '
        'encryptedValue
        '
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        Me.encryptedValue.DefaultCellStyle = DataGridViewCellStyle8
        Me.encryptedValue.HeaderText = "Encrypted Value"
        Me.encryptedValue.Name = "encryptedValue"
        Me.encryptedValue.ReadOnly = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.EnableApplyToAllCBox)
        Me.GroupBox2.Location = New System.Drawing.Point(366, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(86, 45)
        Me.GroupBox2.TabIndex = 48
        Me.GroupBox2.TabStop = False
        '
        'EnableApplyToAllCBox
        '
        Me.EnableApplyToAllCBox.AutoSize = True
        Me.EnableApplyToAllCBox.Checked = True
        Me.EnableApplyToAllCBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.EnableApplyToAllCBox.FlatAppearance.BorderColor = System.Drawing.Color.Black
        Me.EnableApplyToAllCBox.Location = New System.Drawing.Point(6, 17)
        Me.EnableApplyToAllCBox.Name = "EnableApplyToAllCBox"
        Me.EnableApplyToAllCBox.Size = New System.Drawing.Size(70, 17)
        Me.EnableApplyToAllCBox.TabIndex = 1
        Me.EnableApplyToAllCBox.Text = "Select All"
        Me.EnableApplyToAllCBox.UseVisualStyleBackColor = True
        '
        'verifyAllCBox
        '
        Me.verifyAllCBox.AutoSize = True
        Me.verifyAllCBox.Location = New System.Drawing.Point(16, 19)
        Me.verifyAllCBox.Name = "verifyAllCBox"
        Me.verifyAllCBox.Size = New System.Drawing.Size(66, 17)
        Me.verifyAllCBox.TabIndex = 47
        Me.verifyAllCBox.Text = "Verify All"
        Me.verifyAllCBox.UseVisualStyleBackColor = True
        Me.verifyAllCBox.Visible = False
        '
        'AutoDataFillBtn
        '
        Me.AutoDataFillBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AutoDataFillBtn.Location = New System.Drawing.Point(701, 13)
        Me.AutoDataFillBtn.Name = "AutoDataFillBtn"
        Me.AutoDataFillBtn.Size = New System.Drawing.Size(159, 23)
        Me.AutoDataFillBtn.TabIndex = 46
        Me.AutoDataFillBtn.Text = "Fill in Data From Config Files"
        Me.AutoDataFillBtn.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.ApplytoAllGBox)
        Me.Panel1.Controls.Add(Me.CancelBtn)
        Me.Panel1.Controls.Add(Me.GroupBox1)
        Me.Panel1.Controls.Add(Me.SaveBtn)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(912, 633)
        Me.Panel1.TabIndex = 46
        '
        'ParamEditForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(912, 633)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "ParamEditForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "ParamEditForm"
        Me.ApplytoAllGBox.ResumeLayout(False)
        Me.ApplytoAllGBox.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.parameterDGView, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents PoolSizeTBox As System.Windows.Forms.TextBox
    Friend WithEvents DBNameTBox As System.Windows.Forms.TextBox
    Friend WithEvents DSTBox As System.Windows.Forms.TextBox
    Friend WithEvents PwdTBox As System.Windows.Forms.TextBox
    Friend WithEvents UserIDTBox As System.Windows.Forms.TextBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents SaveBtn As System.Windows.Forms.Button
    Friend WithEvents CancelBtn As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ApplytoAllGBox As System.Windows.Forms.GroupBox
    Friend WithEvents applyToAllBtn As System.Windows.Forms.Button
    Friend WithEvents enableUserIDCBox As System.Windows.Forms.CheckBox
    Friend WithEvents enablePoolSizeCBox As System.Windows.Forms.CheckBox
    Friend WithEvents enableDBNameCBox As System.Windows.Forms.CheckBox
    Friend WithEvents enableDSCBox As System.Windows.Forms.CheckBox
    Friend WithEvents enablePwdCBox As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents parameterDGView As System.Windows.Forms.DataGridView
    Friend WithEvents EnableApplyToAllCBox As System.Windows.Forms.CheckBox
    Friend WithEvents AutoDataFillBtn As System.Windows.Forms.Button
    Friend WithEvents verifyAllCBox As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents Verify As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents paramName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents applyToAllCBox As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents username As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pwd As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents datasource As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents DatabaseName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents maxPoolSize As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents decryptedValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents encryptedValue As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents DBCheckPBar As System.Windows.Forms.ProgressBar
End Class
