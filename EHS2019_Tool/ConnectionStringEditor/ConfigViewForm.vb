Imports ConnectionStringEditor.Model
Imports ConnectionStringEditor.BLL
Imports System.Threading

Public Class ConfigViewForm

    Private m_pConnectionStringModelDic As Dictionary(Of String, ConnectionStringModel)

    Private m_pGeneratorFormModel As GeneratorFormModel
    Private m_strConfigFilePath As String

    Private m_pConfigFileBuilder As ConfigFileBuilder
    Private m_pWebConfigFileModel As WebConfigFileModel
    Private m_strExceptionError As String
    Private m_pConfigFileViewModel As ConfigFileViewModel


    Public Sub New(ByVal _pConfigFileViewModel As ConfigFileViewModel, _pGeneratorFormModel As GeneratorFormModel, ByVal _strConfigFilePath As String)
        Try
            m_pConfigFileViewModel = _pConfigFileViewModel
            m_pGeneratorFormModel = _pGeneratorFormModel
            m_pConnectionStringModelDic = m_pGeneratorFormModel.pWebConfigDic(_strConfigFilePath)
            m_strConfigFilePath = _strConfigFilePath
            If (m_pConnectionStringModelDic Is Nothing) Then
                m_pConnectionStringModelDic = New Dictionary(Of String, ConnectionStringModel)
            End If
            InitializeComponent()
            FillDataToObjectModel()
            UIInit()
            Control.CheckForIllegalCrossThreadCalls = False
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub FillDataToObjectModel()
        configFileFromTBox.Text = m_strConfigFilePath
        configFileAtTBox.Text = m_pGeneratorFormModel.pNewConfigFilePath(m_strConfigFilePath)
        Try
            m_pConfigFileBuilder = New ConfigFileBuilder(m_pGeneratorFormModel, m_strConfigFilePath)
            m_pWebConfigFileModel = New WebConfigFileModel
            Dim tmpDic As Dictionary(Of String, String) = m_pConfigFileBuilder.GetNewParameterList()
            If (tmpDic.Count > 0) Then
                For Each iii As KeyValuePair(Of String, String) In tmpDic
                    m_pWebConfigFileModel.pNewConnectionStringDir.Add(iii.Key, iii.Value)
                Next
            End If

            tmpDic = m_pConfigFileBuilder.GetOldParameterList()
            If (tmpDic.Count > 0) Then
                For Each iii As KeyValuePair(Of String, String) In tmpDic
                    m_pWebConfigFileModel.pOldConnectionStringDir.Add(iii.Key, iii.Value)
                Next
            End If
        Catch ex As Exception
            Throw New Exception("No Parameter Match")
        End Try
    End Sub

    Private Sub UIInit()
        Try
            For Each iii As ParameterListModel In m_pConfigFileViewModel.pConfigFileRawDetail(m_strConfigFilePath)
                paramLView.Items.Add(iii.strParamName)
                Select Case iii.eParamStatus
                    Case SysConst.CONFIGFILE_PARAMSTATUS.NORMAL
                        paramLView.Items(paramLView.Items.Count - 1).ForeColor = SysConst.NORMALPARAMCOLOR
                    Case SysConst.CONFIGFILE_PARAMSTATUS.MISSING
                        paramLView.Items(paramLView.Items.Count - 1).ForeColor = SysConst.MISSINGPARAMCOLOR
                    Case SysConst.CONFIGFILE_PARAMSTATUS.EXTRA
                        paramLView.Items(paramLView.Items.Count - 1).ForeColor = SysConst.EXTRAPARAMCOLOR
                    Case Else
                End Select
            Next
        Catch ex As Exception
            Throw New Exception(ex.Message)
        End Try
    End Sub

    Private Sub BackBtn_Click(sender As Object, e As EventArgs) Handles BackBtn.Click
        Me.Close()
    End Sub


    Public ReadOnly Property pConfigFileModel() As Dictionary(Of String, ConnectionStringModel)
        Get
            Return m_pConnectionStringModelDic
        End Get
    End Property

    Public Property strExceptionError() As String
        Get
            Return m_strExceptionError
        End Get
        Set(ByVal value As String)
            m_strExceptionError = value
        End Set
    End Property

    Private trd As Thread
    'Private Sub ThreadTask()

    'End Sub

    Private Function LoadSelectedDataToConnectionStringModel() As ConnectionStringModel
        Dim pConnectionStringModelDic = New ConnectionStringModel
        pConnectionStringModelDic.strConfigFilePath = m_strConfigFilePath
        pConnectionStringModelDic.strParamName = paramLView.SelectedItems(0).Text
        pConnectionStringModelDic.strNewParamRawValue = m_pWebConfigFileModel.pNewConnectionStringDir(paramLView.SelectedItems(0).Text)
        pConnectionStringModelDic.strOldParamRawValue = m_pWebConfigFileModel.pOldConnectionStringDir(paramLView.SelectedItems(0).Text)
        pConnectionStringModelDic.strEncryptedKey = m_pGeneratorFormModel.strEncryptedKey
        pConnectionStringModelDic.strDecryptedKey = m_pGeneratorFormModel.strDecryptedKey
        Return pConnectionStringModelDic
    End Function

    Private Sub paramLView_DoubleClick(sender As Object, e As EventArgs) Handles paramLView.DoubleClick
        Try
            If (paramLView.SelectedItems(0) IsNot Nothing AndAlso m_pConnectionStringModelDic IsNot Nothing AndAlso Not m_pConnectionStringModelDic.ContainsKey(paramLView.SelectedItems(0).Text)) Then
                Dim ptmpObj = LoadSelectedDataToConnectionStringModel()
                m_pConnectionStringModelDic.Add(paramLView.SelectedItems(0).Text, ptmpObj)
            End If
            If (paramLView.SelectedItems(0) IsNot Nothing) Then
                Dim tmpParamEditForm = New ParamViewForm(m_pGeneratorFormModel, m_pConnectionStringModelDic(paramLView.SelectedItems(0).Text))
                tmpParamEditForm.ShowDialog()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class