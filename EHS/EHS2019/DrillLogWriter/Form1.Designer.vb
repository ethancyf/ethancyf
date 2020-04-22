<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.components = New System.ComponentModel.Container()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.txtMessage = New System.Windows.Forms.TextBox()
        Me.ddlEventID = New System.Windows.Forms.ComboBox()
        Me.ddlEventSource = New System.Windows.Forms.ComboBox()
        Me.btnWriteLog = New System.Windows.Forms.Button()
        Me.lblSeverity = New System.Windows.Forms.Label()
        Me.lblSeverityText = New System.Windows.Forms.Label()
        Me.lblMessageText = New System.Windows.Forms.Label()
        Me.lblEventIDText = New System.Windows.Forms.Label()
        Me.lblEventSourceText = New System.Windows.Forms.Label()
        Me.txtConsole = New System.Windows.Forms.RichTextBox()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtMessage)
        Me.GroupBox1.Controls.Add(Me.ddlEventID)
        Me.GroupBox1.Controls.Add(Me.ddlEventSource)
        Me.GroupBox1.Controls.Add(Me.btnWriteLog)
        Me.GroupBox1.Controls.Add(Me.lblSeverity)
        Me.GroupBox1.Controls.Add(Me.lblSeverityText)
        Me.GroupBox1.Controls.Add(Me.lblMessageText)
        Me.GroupBox1.Controls.Add(Me.lblEventIDText)
        Me.GroupBox1.Controls.Add(Me.lblEventSourceText)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 260)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(760, 240)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        '
        'txtMessage
        '
        Me.txtMessage.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.txtMessage.Location = New System.Drawing.Point(101, 136)
        Me.txtMessage.Name = "txtMessage"
        Me.txtMessage.Size = New System.Drawing.Size(636, 26)
        Me.txtMessage.TabIndex = 3
        '
        'ddlEventID
        '
        Me.ddlEventID.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ddlEventID.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.ddlEventID.FormattingEnabled = True
        Me.ddlEventID.Location = New System.Drawing.Point(101, 58)
        Me.ddlEventID.Name = "ddlEventID"
        Me.ddlEventID.Size = New System.Drawing.Size(201, 28)
        Me.ddlEventID.TabIndex = 2
        '
        'ddlEventSource
        '
        Me.ddlEventSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ddlEventSource.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.ddlEventSource.FormattingEnabled = True
        Me.ddlEventSource.IntegralHeight = False
        Me.ddlEventSource.Location = New System.Drawing.Point(101, 19)
        Me.ddlEventSource.Name = "ddlEventSource"
        Me.ddlEventSource.Size = New System.Drawing.Size(394, 28)
        Me.ddlEventSource.TabIndex = 1
        '
        'btnWriteLog
        '
        Me.btnWriteLog.Location = New System.Drawing.Point(332, 187)
        Me.btnWriteLog.Name = "btnWriteLog"
        Me.btnWriteLog.Size = New System.Drawing.Size(97, 27)
        Me.btnWriteLog.TabIndex = 1
        Me.btnWriteLog.Text = "Write Log"
        Me.btnWriteLog.UseVisualStyleBackColor = True
        '
        'lblSeverity
        '
        Me.lblSeverity.AutoSize = True
        Me.lblSeverity.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(136, Byte))
        Me.lblSeverity.Location = New System.Drawing.Point(97, 100)
        Me.lblSeverity.Name = "lblSeverity"
        Me.lblSeverity.Size = New System.Drawing.Size(73, 20)
        Me.lblSeverity.TabIndex = 0
        Me.lblSeverity.Text = "[Severity]"
        '
        'lblSeverityText
        '
        Me.lblSeverityText.AutoSize = True
        Me.lblSeverityText.Location = New System.Drawing.Point(16, 105)
        Me.lblSeverityText.Name = "lblSeverityText"
        Me.lblSeverityText.Size = New System.Drawing.Size(48, 13)
        Me.lblSeverityText.TabIndex = 0
        Me.lblSeverityText.Text = "Severity:"
        '
        'lblMessageText
        '
        Me.lblMessageText.AutoSize = True
        Me.lblMessageText.Location = New System.Drawing.Point(16, 144)
        Me.lblMessageText.Name = "lblMessageText"
        Me.lblMessageText.Size = New System.Drawing.Size(53, 13)
        Me.lblMessageText.TabIndex = 0
        Me.lblMessageText.Text = "Message:"
        '
        'lblEventIDText
        '
        Me.lblEventIDText.AutoSize = True
        Me.lblEventIDText.Location = New System.Drawing.Point(16, 66)
        Me.lblEventIDText.Name = "lblEventIDText"
        Me.lblEventIDText.Size = New System.Drawing.Size(52, 13)
        Me.lblEventIDText.TabIndex = 0
        Me.lblEventIDText.Text = "Event ID:"
        '
        'lblEventSourceText
        '
        Me.lblEventSourceText.AutoSize = True
        Me.lblEventSourceText.Location = New System.Drawing.Point(16, 27)
        Me.lblEventSourceText.Name = "lblEventSourceText"
        Me.lblEventSourceText.Size = New System.Drawing.Size(75, 13)
        Me.lblEventSourceText.TabIndex = 0
        Me.lblEventSourceText.Text = "Event Source:"
        '
        'txtConsole
        '
        Me.txtConsole.Location = New System.Drawing.Point(12, 12)
        Me.txtConsole.Name = "txtConsole"
        Me.txtConsole.ReadOnly = True
        Me.txtConsole.Size = New System.Drawing.Size(760, 242)
        Me.txtConsole.TabIndex = 2
        Me.txtConsole.Text = ""
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(784, 512)
        Me.Controls.Add(Me.txtConsole)
        Me.Controls.Add(Me.GroupBox1)
        Me.Name = "Form1"
        Me.Text = "eHS(S) Drill Log Writer"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents txtConsole As System.Windows.Forms.RichTextBox
    Friend WithEvents txtMessage As System.Windows.Forms.TextBox
    Friend WithEvents ddlEventID As System.Windows.Forms.ComboBox
    Friend WithEvents ddlEventSource As System.Windows.Forms.ComboBox
    Friend WithEvents btnWriteLog As System.Windows.Forms.Button
    Friend WithEvents lblSeverity As System.Windows.Forms.Label
    Friend WithEvents lblSeverityText As System.Windows.Forms.Label
    Friend WithEvents lblMessageText As System.Windows.Forms.Label
    Friend WithEvents lblEventIDText As System.Windows.Forms.Label
    Friend WithEvents lblEventSourceText As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer

End Class
