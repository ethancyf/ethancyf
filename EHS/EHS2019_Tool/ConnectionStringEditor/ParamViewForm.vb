Imports ConnectionStringEditor.Model
Imports EncryptionLib

Public Class ParamViewForm
    Private m_pConfigFileModel As ConnectionStringModel
    Private m_strConfigFileName As String
    Private m_strParamName As String
    Private m_pFeService = New FE_Symmetric
    Private m_pGeneratorFormModel As GeneratorFormModel

    Public Sub New(ByVal _pGeneratorFormModel As GeneratorFormModel, _pInConfigFileModel As ConnectionStringModel)
        InitializeComponent()
        m_pConfigFileModel = _pInConfigFileModel
        m_strConfigFileName = m_pConfigFileModel.strConfigFilePath
        m_strParamName = m_pConfigFileModel.strParamName
        m_pConfigFileModel.strDecryptedParamValue = m_pFeService.DecryptString(newEncryptedStringRTBox.Text, m_pConfigFileModel.strDecryptedKey)
        m_pGeneratorFormModel = _pGeneratorFormModel

        ParamTBox.Text = m_strParamName
        configFileFromTBox.Text = m_strConfigFileName

        newConfigTBox.Text = m_pGeneratorFormModel.pNewConfigFilePath(m_strConfigFileName)
        oldConfigTBox.Text = m_pGeneratorFormModel.pOldConfigFilePath(m_strConfigFileName)

        newEncryptedStringRTBox.Text = m_pConfigFileModel.strNewParamRawValue
        newDecryptStringRTBox.Text = m_pFeService.DecryptString(newEncryptedStringRTBox.Text, m_pConfigFileModel.strDecryptedKey)

        oldEncryptedStringRTBox.Text = m_pConfigFileModel.strOldParamRawValue
        oldDecryptStringRTBox.Text = m_pFeService.DecryptString(oldEncryptedStringRTBox.Text, m_pConfigFileModel.strDecryptedKey)
    End Sub

    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        Me.Close()
    End Sub

    Public ReadOnly Property pConfigFileModel() As ConnectionStringModel
        Get
            Return m_pConfigFileModel
        End Get
    End Property
End Class