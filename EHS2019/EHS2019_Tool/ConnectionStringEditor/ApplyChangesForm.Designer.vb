<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ApplyChangesForm
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ConfigFileLBox = New System.Windows.Forms.ListBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ApplyChangesPBar = New System.Windows.Forms.ProgressBar()
        Me.backBtn = New System.Windows.Forms.Button()
        Me.applyChangesBtn = New System.Windows.Forms.Button()
        Me.ConsoleLogRTBox = New System.Windows.Forms.RichTextBox()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.Panel2.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ConfigFileLBox)
        Me.GroupBox1.Location = New System.Drawing.Point(16, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(845, 216)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Config File List"
        '
        'ConfigFileLBox
        '
        Me.ConfigFileLBox.FormattingEnabled = True
        Me.ConfigFileLBox.HorizontalScrollbar = True
        Me.ConfigFileLBox.Location = New System.Drawing.Point(25, 20)
        Me.ConfigFileLBox.Name = "ConfigFileLBox"
        Me.ConfigFileLBox.Size = New System.Drawing.Size(800, 186)
        Me.ConfigFileLBox.TabIndex = 0
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ApplyChangesPBar)
        Me.GroupBox2.Controls.Add(Me.backBtn)
        Me.GroupBox2.Controls.Add(Me.applyChangesBtn)
        Me.GroupBox2.Controls.Add(Me.ConsoleLogRTBox)
        Me.GroupBox2.Location = New System.Drawing.Point(16, 15)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(844, 332)
        Me.GroupBox2.TabIndex = 2
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Console Log"
        '
        'ApplyChangesPBar
        '
        Me.ApplyChangesPBar.Location = New System.Drawing.Point(24, 253)
        Me.ApplyChangesPBar.Name = "ApplyChangesPBar"
        Me.ApplyChangesPBar.Size = New System.Drawing.Size(800, 23)
        Me.ApplyChangesPBar.TabIndex = 4
        Me.ApplyChangesPBar.Visible = False
        '
        'backBtn
        '
        Me.backBtn.Location = New System.Drawing.Point(24, 289)
        Me.backBtn.Name = "backBtn"
        Me.backBtn.Size = New System.Drawing.Size(75, 23)
        Me.backBtn.TabIndex = 3
        Me.backBtn.Text = "Back"
        Me.backBtn.UseVisualStyleBackColor = True
        '
        'applyChangesBtn
        '
        Me.applyChangesBtn.Location = New System.Drawing.Point(204, 289)
        Me.applyChangesBtn.Name = "applyChangesBtn"
        Me.applyChangesBtn.Size = New System.Drawing.Size(108, 23)
        Me.applyChangesBtn.TabIndex = 2
        Me.applyChangesBtn.Text = "Apply Changes"
        Me.applyChangesBtn.UseVisualStyleBackColor = True
        '
        'ConsoleLogRTBox
        '
        Me.ConsoleLogRTBox.BackColor = System.Drawing.SystemColors.ScrollBar
        Me.ConsoleLogRTBox.DetectUrls = False
        Me.ConsoleLogRTBox.ForeColor = System.Drawing.Color.Red
        Me.ConsoleLogRTBox.Location = New System.Drawing.Point(24, 31)
        Me.ConsoleLogRTBox.Name = "ConsoleLogRTBox"
        Me.ConsoleLogRTBox.ReadOnly = True
        Me.ConsoleLogRTBox.Size = New System.Drawing.Size(800, 215)
        Me.ConsoleLogRTBox.TabIndex = 1
        Me.ConsoleLogRTBox.Text = ""
        '
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 0)
        Me.SplitContainer1.Name = "SplitContainer1"
        Me.SplitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.AutoScroll = True
        Me.SplitContainer1.Panel1.Controls.Add(Me.GroupBox1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.AutoScroll = True
        Me.SplitContainer1.Panel2.Controls.Add(Me.GroupBox2)
        Me.SplitContainer1.Size = New System.Drawing.Size(884, 596)
        Me.SplitContainer1.SplitterDistance = 233
        Me.SplitContainer1.TabIndex = 3
        '
        'ApplyChangesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 596)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Name = "ApplyChangesForm"
        Me.Text = "Apply Changes"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.Panel2.ResumeLayout(False)
        CType(Me.SplitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SplitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ConsoleLogRTBox As System.Windows.Forms.RichTextBox
    Friend WithEvents applyChangesBtn As System.Windows.Forms.Button
    Friend WithEvents ConfigFileLBox As System.Windows.Forms.ListBox
    Friend WithEvents backBtn As System.Windows.Forms.Button
    Friend WithEvents ApplyChangesPBar As System.Windows.Forms.ProgressBar
    Friend WithEvents SplitContainer1 As System.Windows.Forms.SplitContainer
End Class
