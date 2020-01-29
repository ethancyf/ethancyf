Imports ConnectionStringEditor.Model
Imports ConnectionStringEditor.BLL
Imports System.Threading

Public Class ApplyChangesForm
    Private m_pGeneratorFormModel As GeneratorFormModel
    Private m_pConnectionModelList As List(Of ConnectionStringModel)
    Public Sub New(_pGeneratorFormModel As GeneratorFormModel, _pConnectionModelList As List(Of ConnectionStringModel))
        m_pGeneratorFormModel = _pGeneratorFormModel
        m_pConnectionModelList = _pConnectionModelList
        ' This call is required by the designer.
        InitializeComponent()
        UIInit()
        ' Add any initialization after the InitializeComponent() call.
        Control.CheckForIllegalCrossThreadCalls = False
    End Sub

    Private Sub UIInit()
        ConfigFileLBox.Items.Clear()
        For Each tmpStr As String In m_pGeneratorFormModel.pFileList
            ConfigFileLBox.Items.Add(m_pGeneratorFormModel.pNewConfigFilePath(tmpStr))
        Next
    End Sub

    Private trd As Thread
    Private Sub ThreadTask()
        Dim iProgressCounter = 0
        For Each tmpConfigFilePath As String In m_pGeneratorFormModel.pFileList
            For Each tmpConnectionStringModel As ConnectionStringModel In m_pConnectionModelList
                Try
                    Using tmpConfigFileBuilder = New ConfigFileBuilder(m_pGeneratorFormModel, tmpConfigFilePath)

                        tmpConfigFileBuilder.SetNewParameter(tmpConnectionStringModel.strParamName, tmpConnectionStringModel.strEncryptResult)
                    End Using
                Catch ex As Exception
                    ConsoleLogRTBox.AppendText(String.Format("{0} ::" + Environment.NewLine + Chr(9) + "{1}::" + Environment.NewLine + Chr(9) + Chr(9) + "{2}" + Environment.NewLine, m_pGeneratorFormModel.pNewConfigFilePath(tmpConfigFilePath), tmpConnectionStringModel.strParamName, ex.Message))
                    ConsoleLogRTBox.SelectionStart = ConsoleLogRTBox.Text.Length
                    ConsoleLogRTBox.ScrollToCaret()
                End Try
            Next
            iProgressCounter += 1
            ApplyChangesPBar.Value = iProgressCounter
        Next
        ConsoleLogRTBox.AppendText("Update Completed")
        backBtn.Enabled = True
        applyChangesBtn.Enabled = True
        ApplyChangesPBar.Visible = False
    End Sub

    Private Sub applyChangesBtn_Click(sender As Object, e As EventArgs) Handles applyChangesBtn.Click
        ConsoleLogRTBox.Text = ""
        backBtn.Enabled = False
        applyChangesBtn.Enabled = False
        trd = New Thread(AddressOf ThreadTask)
        trd.IsBackground = True
        ApplyChangesPBar.Step = m_pGeneratorFormModel.pFileList.Count
        ApplyChangesPBar.Maximum = m_pGeneratorFormModel.pFileList.Count
        ApplyChangesPBar.Value = 0
        ApplyChangesPBar.Visible = True
        trd.Start()
    End Sub

    Private Sub backBtn_Click(sender As Object, e As EventArgs) Handles backBtn.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
End Class