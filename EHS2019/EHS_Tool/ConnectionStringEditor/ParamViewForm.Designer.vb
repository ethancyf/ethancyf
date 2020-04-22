<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ParamViewForm
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
        Me.newConfigGBox = New System.Windows.Forms.GroupBox()
        Me.newConfigTBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.decryptStringLbl = New System.Windows.Forms.Label()
        Me.newDecryptStringRTBox = New System.Windows.Forms.RichTextBox()
        Me.EncryptedStringLbl = New System.Windows.Forms.Label()
        Me.newEncryptedStringRTBox = New System.Windows.Forms.RichTextBox()
        Me.BackBtn = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.configFileFromTBox = New System.Windows.Forms.TextBox()
        Me.ParamTBox = New System.Windows.Forms.TextBox()
        Me.oldConfigGBox = New System.Windows.Forms.GroupBox()
        Me.oldConfigTBox = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.oldDecryptStringRTBox = New System.Windows.Forms.RichTextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.oldEncryptedStringRTBox = New System.Windows.Forms.RichTextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.newConfigGBox.SuspendLayout()
        Me.oldConfigGBox.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'newConfigGBox
        '
        Me.newConfigGBox.Controls.Add(Me.newConfigTBox)
        Me.newConfigGBox.Controls.Add(Me.Label6)
        Me.newConfigGBox.Controls.Add(Me.decryptStringLbl)
        Me.newConfigGBox.Controls.Add(Me.newDecryptStringRTBox)
        Me.newConfigGBox.Controls.Add(Me.EncryptedStringLbl)
        Me.newConfigGBox.Controls.Add(Me.newEncryptedStringRTBox)
        Me.newConfigGBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.newConfigGBox.Location = New System.Drawing.Point(13, 64)
        Me.newConfigGBox.Name = "newConfigGBox"
        Me.newConfigGBox.Size = New System.Drawing.Size(607, 279)
        Me.newConfigGBox.TabIndex = 0
        Me.newConfigGBox.TabStop = False
        Me.newConfigGBox.Text = "New File"
        '
        'newConfigTBox
        '
        Me.newConfigTBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.newConfigTBox.Location = New System.Drawing.Point(89, 31)
        Me.newConfigTBox.Name = "newConfigTBox"
        Me.newConfigTBox.ReadOnly = True
        Me.newConfigTBox.Size = New System.Drawing.Size(510, 20)
        Me.newConfigTBox.TabIndex = 19
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label6.Location = New System.Drawing.Point(12, 31)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(71, 20)
        Me.Label6.TabIndex = 15
        Me.Label6.Text = "File Path"
        '
        'decryptStringLbl
        '
        Me.decryptStringLbl.AutoSize = True
        Me.decryptStringLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.decryptStringLbl.Location = New System.Drawing.Point(16, 171)
        Me.decryptStringLbl.Name = "decryptStringLbl"
        Me.decryptStringLbl.Size = New System.Drawing.Size(128, 20)
        Me.decryptStringLbl.TabIndex = 11
        Me.decryptStringLbl.Text = "Decrypted String"
        '
        'newDecryptStringRTBox
        '
        Me.newDecryptStringRTBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.newDecryptStringRTBox.Location = New System.Drawing.Point(149, 173)
        Me.newDecryptStringRTBox.Name = "newDecryptStringRTBox"
        Me.newDecryptStringRTBox.ReadOnly = True
        Me.newDecryptStringRTBox.Size = New System.Drawing.Size(450, 80)
        Me.newDecryptStringRTBox.TabIndex = 10
        Me.newDecryptStringRTBox.Text = ""
        '
        'EncryptedStringLbl
        '
        Me.EncryptedStringLbl.AutoSize = True
        Me.EncryptedStringLbl.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.EncryptedStringLbl.Location = New System.Drawing.Point(16, 73)
        Me.EncryptedStringLbl.Name = "EncryptedStringLbl"
        Me.EncryptedStringLbl.Size = New System.Drawing.Size(127, 20)
        Me.EncryptedStringLbl.TabIndex = 9
        Me.EncryptedStringLbl.Text = "Encrypted String"
        '
        'newEncryptedStringRTBox
        '
        Me.newEncryptedStringRTBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.newEncryptedStringRTBox.Location = New System.Drawing.Point(149, 73)
        Me.newEncryptedStringRTBox.Name = "newEncryptedStringRTBox"
        Me.newEncryptedStringRTBox.ReadOnly = True
        Me.newEncryptedStringRTBox.Size = New System.Drawing.Size(450, 80)
        Me.newEncryptedStringRTBox.TabIndex = 8
        Me.newEncryptedStringRTBox.Text = ""
        '
        'BackBtn
        '
        Me.BackBtn.Location = New System.Drawing.Point(10, 645)
        Me.BackBtn.Name = "BackBtn"
        Me.BackBtn.Size = New System.Drawing.Size(75, 23)
        Me.BackBtn.TabIndex = 2
        Me.BackBtn.Text = "Close"
        Me.BackBtn.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(23, 34)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(116, 15)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Parameter Name"
        Me.Label2.TextAlign = System.Drawing.ContentAlignment.BottomLeft
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(23, 6)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(113, 15)
        Me.Label3.TabIndex = 14
        Me.Label3.Text = "Config File From"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'configFileFromTBox
        '
        Me.configFileFromTBox.Location = New System.Drawing.Point(148, 6)
        Me.configFileFromTBox.Name = "configFileFromTBox"
        Me.configFileFromTBox.ReadOnly = True
        Me.configFileFromTBox.Size = New System.Drawing.Size(472, 20)
        Me.configFileFromTBox.TabIndex = 15
        '
        'ParamTBox
        '
        Me.ParamTBox.Location = New System.Drawing.Point(152, 34)
        Me.ParamTBox.Name = "ParamTBox"
        Me.ParamTBox.ReadOnly = True
        Me.ParamTBox.Size = New System.Drawing.Size(468, 20)
        Me.ParamTBox.TabIndex = 17
        '
        'oldConfigGBox
        '
        Me.oldConfigGBox.Controls.Add(Me.oldConfigTBox)
        Me.oldConfigGBox.Controls.Add(Me.Label1)
        Me.oldConfigGBox.Controls.Add(Me.Label4)
        Me.oldConfigGBox.Controls.Add(Me.oldDecryptStringRTBox)
        Me.oldConfigGBox.Controls.Add(Me.Label5)
        Me.oldConfigGBox.Controls.Add(Me.oldEncryptedStringRTBox)
        Me.oldConfigGBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.oldConfigGBox.Location = New System.Drawing.Point(13, 360)
        Me.oldConfigGBox.Name = "oldConfigGBox"
        Me.oldConfigGBox.Size = New System.Drawing.Size(607, 279)
        Me.oldConfigGBox.TabIndex = 18
        Me.oldConfigGBox.TabStop = False
        Me.oldConfigGBox.Text = "Backup File"
        '
        'oldConfigTBox
        '
        Me.oldConfigTBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.oldConfigTBox.Location = New System.Drawing.Point(89, 32)
        Me.oldConfigTBox.Name = "oldConfigTBox"
        Me.oldConfigTBox.ReadOnly = True
        Me.oldConfigTBox.Size = New System.Drawing.Size(510, 20)
        Me.oldConfigTBox.TabIndex = 21
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(71, 20)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "File Path"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label4.Location = New System.Drawing.Point(16, 185)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(128, 20)
        Me.Label4.TabIndex = 11
        Me.Label4.Text = "Decrypted String"
        '
        'oldDecryptStringRTBox
        '
        Me.oldDecryptStringRTBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.oldDecryptStringRTBox.Location = New System.Drawing.Point(149, 187)
        Me.oldDecryptStringRTBox.Name = "oldDecryptStringRTBox"
        Me.oldDecryptStringRTBox.ReadOnly = True
        Me.oldDecryptStringRTBox.Size = New System.Drawing.Size(450, 80)
        Me.oldDecryptStringRTBox.TabIndex = 10
        Me.oldDecryptStringRTBox.Text = ""
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.Label5.Location = New System.Drawing.Point(16, 84)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(127, 20)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Encrypted String"
        '
        'oldEncryptedStringRTBox
        '
        Me.oldEncryptedStringRTBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.oldEncryptedStringRTBox.Location = New System.Drawing.Point(149, 84)
        Me.oldEncryptedStringRTBox.Name = "oldEncryptedStringRTBox"
        Me.oldEncryptedStringRTBox.ReadOnly = True
        Me.oldEncryptedStringRTBox.Size = New System.Drawing.Size(450, 80)
        Me.oldEncryptedStringRTBox.TabIndex = 8
        Me.oldEncryptedStringRTBox.Text = ""
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.newConfigGBox)
        Me.Panel1.Controls.Add(Me.oldConfigGBox)
        Me.Panel1.Controls.Add(Me.BackBtn)
        Me.Panel1.Controls.Add(Me.ParamTBox)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.configFileFromTBox)
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(634, 682)
        Me.Panel1.TabIndex = 19
        '
        'ParamViewForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(634, 682)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "ParamViewForm"
        Me.Text = "ParamEditForm"
        Me.newConfigGBox.ResumeLayout(False)
        Me.newConfigGBox.PerformLayout()
        Me.oldConfigGBox.ResumeLayout(False)
        Me.oldConfigGBox.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents newConfigGBox As System.Windows.Forms.GroupBox
    Friend WithEvents BackBtn As System.Windows.Forms.Button
    Friend WithEvents EncryptedStringLbl As System.Windows.Forms.Label
    Friend WithEvents newDecryptStringRTBox As System.Windows.Forms.RichTextBox
    Friend WithEvents decryptStringLbl As System.Windows.Forms.Label
    Friend WithEvents newEncryptedStringRTBox As System.Windows.Forms.RichTextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents configFileFromTBox As System.Windows.Forms.TextBox
    Friend WithEvents ParamTBox As System.Windows.Forms.TextBox
    Friend WithEvents oldConfigGBox As System.Windows.Forms.GroupBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents oldDecryptStringRTBox As System.Windows.Forms.RichTextBox
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents oldEncryptedStringRTBox As System.Windows.Forms.RichTextBox
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents newConfigTBox As System.Windows.Forms.TextBox
    Friend WithEvents oldConfigTBox As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
End Class
