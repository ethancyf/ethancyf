<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ConfigViewForm
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
        Me.BackBtn = New System.Windows.Forms.Button()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.ConnStringGBox = New System.Windows.Forms.GroupBox()
        Me.paramLView = New System.Windows.Forms.ListView()
        Me.FilePathCol = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.configFileFromTBox = New System.Windows.Forms.TextBox()
        Me.configFileAtTBox = New System.Windows.Forms.TextBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.ConnStringGBox.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'BackBtn
        '
        Me.BackBtn.Location = New System.Drawing.Point(28, 448)
        Me.BackBtn.Name = "BackBtn"
        Me.BackBtn.Size = New System.Drawing.Size(75, 23)
        Me.BackBtn.TabIndex = 1
        Me.BackBtn.Text = "Back"
        Me.BackBtn.UseVisualStyleBackColor = True
        '
        'ConnStringGBox
        '
        Me.ConnStringGBox.Controls.Add(Me.paramLView)
        Me.ConnStringGBox.Location = New System.Drawing.Point(10, 102)
        Me.ConnStringGBox.Name = "ConnStringGBox"
        Me.ConnStringGBox.Size = New System.Drawing.Size(728, 340)
        Me.ConnStringGBox.TabIndex = 4
        Me.ConnStringGBox.TabStop = False
        Me.ConnStringGBox.Text = "Connection String List"
        '
        'paramLView
        '
        Me.paramLView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.FilePathCol})
        Me.paramLView.FullRowSelect = True
        Me.paramLView.GridLines = True
        Me.paramLView.Location = New System.Drawing.Point(17, 29)
        Me.paramLView.MultiSelect = False
        Me.paramLView.Name = "paramLView"
        Me.paramLView.Size = New System.Drawing.Size(693, 293)
        Me.paramLView.TabIndex = 19
        Me.paramLView.UseCompatibleStateImageBehavior = False
        Me.paramLView.View = System.Windows.Forms.View.Details
        '
        'FilePathCol
        '
        Me.FilePathCol.Text = "File Path"
        Me.FilePathCol.Width = 748
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(25, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(113, 15)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "Config File From"
        Me.Label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.Label3.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(25, 48)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(92, 15)
        Me.Label1.TabIndex = 16
        Me.Label1.Text = "Config File At"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'configFileFromTBox
        '
        Me.configFileFromTBox.Location = New System.Drawing.Point(153, 19)
        Me.configFileFromTBox.Name = "configFileFromTBox"
        Me.configFileFromTBox.ReadOnly = True
        Me.configFileFromTBox.Size = New System.Drawing.Size(585, 20)
        Me.configFileFromTBox.TabIndex = 17
        Me.configFileFromTBox.Visible = False
        '
        'configFileAtTBox
        '
        Me.configFileAtTBox.Location = New System.Drawing.Point(153, 47)
        Me.configFileAtTBox.Name = "configFileAtTBox"
        Me.configFileAtTBox.ReadOnly = True
        Me.configFileAtTBox.Size = New System.Drawing.Size(585, 20)
        Me.configFileAtTBox.TabIndex = 18
        '
        'Panel1
        '
        Me.Panel1.AutoScroll = True
        Me.Panel1.Controls.Add(Me.Label3)
        Me.Panel1.Controls.Add(Me.configFileAtTBox)
        Me.Panel1.Controls.Add(Me.BackBtn)
        Me.Panel1.Controls.Add(Me.configFileFromTBox)
        Me.Panel1.Controls.Add(Me.ConnStringGBox)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(767, 481)
        Me.Panel1.TabIndex = 20
        '
        'ConfigViewForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(767, 481)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "ConfigViewForm"
        Me.Text = "Config View Form"
        Me.ConnStringGBox.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents BackBtn As System.Windows.Forms.Button
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents ConnStringGBox As System.Windows.Forms.GroupBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents configFileFromTBox As System.Windows.Forms.TextBox
    Friend WithEvents configFileAtTBox As System.Windows.Forms.TextBox
    Friend WithEvents paramLView As System.Windows.Forms.ListView
    Friend WithEvents FilePathCol As System.Windows.Forms.ColumnHeader
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
End Class
