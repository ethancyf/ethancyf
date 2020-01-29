Imports ConnectionStringEditor.Model
Imports EncryptionLib
Imports System.Threading
Imports System.IO

Public Class ParamEditForm
    Private m_bInit As Boolean
    Private m_pConnectionStringModelList As List(Of ConnectionStringModel)
    Private m_pConnectionStringParamList As List(Of String)
    Private m_pGeneratorFormModel = New GeneratorFormModel
    Private m_pConfigFileViewModel As ConfigFileViewModel
    Private m_pParamScanningModel As ParamScanningModel

    Private m_pFeService = New FE_Symmetric


    Sub New(ByVal _pConfigFileViewModel As ConfigFileViewModel, _
            ByVal _pConnectionStringModelList As List(Of ConnectionStringModel), _
            ByVal _pConnectionStringList As List(Of String), _
            ByVal _pGeneratorFormModel As GeneratorFormModel)

        m_pConnectionStringParamList = _pConnectionStringList
        m_bInit = True
        m_pConnectionStringModelList = New List(Of ConnectionStringModel)
        If (_pConnectionStringModelList IsNot Nothing AndAlso _pConnectionStringModelList.Count > 0) Then
            For Each tmpObj As ConnectionStringModel In _pConnectionStringModelList
                m_pConnectionStringModelList.Add(tmpObj.Clone())
            Next
        End If
        m_pConnectionStringParamList = _pConnectionStringList
        m_pGeneratorFormModel = _pGeneratorFormModel

        ' This call is required by the designer.
        InitializeComponent()
        If (m_pGeneratorFormModel.pWebConfigDic Is Nothing OrElse m_pGeneratorFormModel.pWebConfigDic.Count = 0) Then
            AutoDataFillBtn.Enabled = False
        Else
            m_pConfigFileViewModel = _pConfigFileViewModel
            AutoDataFillBtn.Enabled = True
        End If

        InitParameterDGView()
        Control.CheckForIllegalCrossThreadCalls = False
        ' Add any initialization after the InitializeComponent() call.
        m_bInit = False
    End Sub

    Private Sub InitParameterDGView()
        If (m_pConnectionStringModelList IsNot Nothing AndAlso m_pConnectionStringModelList.Count > 0) Then
            For Each tmpConnectionStringModel As ConnectionStringModel In m_pConnectionStringModelList
                parameterDGView.Rows.Add(tmpConnectionStringModel.bVerify, tmpConnectionStringModel.strParamName, tmpConnectionStringModel.bApplyToAll, tmpConnectionStringModel.strUsername, _
                             tmpConnectionStringModel.strUserPwd, tmpConnectionStringModel.strDBServerName, tmpConnectionStringModel.strDBInstanceName, _
                             tmpConnectionStringModel.strPoolSize, tmpConnectionStringModel.strDecryptedResult, tmpConnectionStringModel.strEncryptResult)
                UpdateVerifiedChanges(parameterDGView.Rows.Count - 1)
            Next
            CheckAndSetSaveBtnStatus()
        Else
            ResetParameterDGViewToDefault()
        End If
    End Sub

    Private Sub SaveConnectionStringModelList()
        For rCounter As Int32 = 0 To parameterDGView.Rows.Count - 1
            m_pConnectionStringModelList.Item(rCounter).bVerify = parameterDGView.Rows(rCounter).Cells(0).Value
            m_pConnectionStringModelList.Item(rCounter).bApplyToAll = parameterDGView.Rows(rCounter).Cells(2).Value
            m_pConnectionStringModelList.Item(rCounter).strUsername = parameterDGView.Rows(rCounter).Cells(3).Value
            m_pConnectionStringModelList.Item(rCounter).strUserPwd = parameterDGView.Rows(rCounter).Cells(4).Value
            m_pConnectionStringModelList.Item(rCounter).strDBServerName = parameterDGView.Rows(rCounter).Cells(5).Value
            m_pConnectionStringModelList.Item(rCounter).strDBInstanceName = parameterDGView.Rows(rCounter).Cells(6).Value
            m_pConnectionStringModelList.Item(rCounter).strPoolSize = parameterDGView.Rows(rCounter).Cells(7).Value
            m_pConnectionStringModelList.Item(rCounter).strDecryptedResult = parameterDGView.Rows(rCounter).Cells(8).Value
            m_pConnectionStringModelList.Item(rCounter).strEncryptResult = parameterDGView.Rows(rCounter).Cells(9).Value
        Next
    End Sub

    Public Sub CheckAndSetSaveBtnStatus()
        Dim rowWithoutVerificationChecked = From row In parameterDGView.Rows.Cast(Of DataGridViewRow)()
                                            Where (row.Cells(0).Value = False)
        If (rowWithoutVerificationChecked.Count > 0) Then
            SaveBtn.Enabled = False
        Else
            SaveBtn.Enabled = True
        End If
    End Sub

    Private trd As Thread
    Private Sub ThreadTask()
        Dim bDBCheckPass = True
        Dim strDBCheckErrorMsg = "Connect to Database failed at the following parameters:" + Environment.NewLine
        For rowNum = 0 To parameterDGView.Rows.Count - 1
            If Not parameterDGView.Rows(rowNum).Cells(0).Value Then
                Continue For
            End If
            If (Not DBTester.CheckDBConnection(parameterDGView.Rows(rowNum).Cells(8).Value)) Then
                parameterDGView.Rows(rowNum).Cells(0).Value = False
                parameterDGView.UpdateCellValue(0, rowNum)
                bDBCheckPass = False
                strDBCheckErrorMsg += parameterDGView.Rows(rowNum).Cells(1).Value + Environment.NewLine
            End If
            If (rowNum > 1) Then
                DBCheckPBar.Value = rowNum
            End If
        Next
        DBCheckPBar.Visible = False

        If (bDBCheckPass) Then
            SaveConnectionStringModelList()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        Else
            MessageBox.Show(strDBCheckErrorMsg, "Connect to database fail", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If
        Me.Enabled = True
    End Sub

    Private Sub SaveBtn_Click(sender As Object, e As EventArgs) Handles SaveBtn.Click
        Dim result As Integer = MessageBox.Show("Try to connect database for connection verification", "Check Database Connection", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If result = DialogResult.Yes Then
            Me.Enabled = False
            trd = New Thread(AddressOf ThreadTask)
            trd.IsBackground = True
            DBCheckPBar.Step = parameterDGView.Rows.Count - 1
            DBCheckPBar.Maximum = parameterDGView.Rows.Count - 1
            DBCheckPBar.Value = 1
            DBCheckPBar.Visible = True
            trd.Start()
        Else
            SaveConnectionStringModelList()
            Me.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.Close()
        End If
    End Sub

    Private Sub CancelBtn_Click(sender As Object, e As EventArgs) Handles CancelBtn.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub enableCBox_CheckedChanged(sender As Object, e As EventArgs) Handles enablePwdCBox.CheckedChanged, enableUserIDCBox.CheckedChanged, enablePoolSizeCBox.CheckedChanged, enableDSCBox.CheckedChanged, enableDBNameCBox.CheckedChanged
        If (Not m_bInit) Then
            If (sender Is enableUserIDCBox) Then
                UserIDTBox.Text = ""
                UserIDTBox.Enabled = Not UserIDTBox.Enabled
            ElseIf (sender Is enablePwdCBox) Then
                PwdTBox.Text = ""
                PwdTBox.Enabled = Not PwdTBox.Enabled
            ElseIf (sender Is enableDSCBox) Then
                DSTBox.Text = ""
                DSTBox.Enabled = Not DSTBox.Enabled
            ElseIf (sender Is enableDBNameCBox) Then
                DBNameTBox.Text = ""
                DBNameTBox.Enabled = Not DBNameTBox.Enabled
            ElseIf (sender Is enablePoolSizeCBox) Then
                PoolSizeTBox.Text = ""
                PoolSizeTBox.Enabled = Not PoolSizeTBox.Enabled
            End If
        End If
    End Sub

    Private Sub applyToAllBtn_Click(sender As Object, e As EventArgs) Handles applyToAllBtn.Click
        For rCounter As Int32 = 0 To parameterDGView.Rows.Count - 1
            If (parameterDGView.Rows(rCounter).Cells(2).Value) Then
                If enableUserIDCBox.Checked Then
                    parameterDGView.Rows(rCounter).Cells(3).Value = UserIDTBox.Text
                End If

                If enablePwdCBox.Checked Then
                    parameterDGView.Rows(rCounter).Cells(4).Value = PwdTBox.Text
                End If

                If enableDSCBox.Checked Then
                    parameterDGView.Rows(rCounter).Cells(5).Value = DSTBox.Text
                End If

                If enableDBNameCBox.Checked Then
                    parameterDGView.Rows(rCounter).Cells(6).Value = DBNameTBox.Text
                End If

                If enablePoolSizeCBox.Checked Then
                    parameterDGView.Rows(rCounter).Cells(7).Value = PoolSizeTBox.Text
                End If
                FormatInputComponentsToDecryptValue(parameterDGView.Rows(rCounter).Cells(8).Value, rCounter)
                parameterDGView.Rows(rCounter).Cells(9).Value = GenerateEncryptValue(parameterDGView.Rows(rCounter).Cells(8).Value)
            End If
        Next
    End Sub

    Private Function GenerateEncryptValue(ByVal _inDecryptValue As String) As String
        Return m_pFeService.EncryptData(m_pGeneratorFormModel.strEncryptedKey, _inDecryptValue)
    End Function

    Private Sub FormatInputComponentsToDecryptValue(ByRef _inDecryptValue As String, ByVal _rCounter As Integer)
        _inDecryptValue = String.Format( _
            SysConst.DB_CONNSTRFORMAT, _
            parameterDGView.Rows(_rCounter).Cells(5).Value, _
            parameterDGView.Rows(_rCounter).Cells(6).Value, _
            parameterDGView.Rows(_rCounter).Cells(3).Value, _
            parameterDGView.Rows(_rCounter).Cells(4).Value, _
            parameterDGView.Rows(_rCounter).Cells(7).Value _
            )
    End Sub

    Private Sub parameterDGView_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles parameterDGView.CellLeave, parameterDGView.CellEndEdit
        FormatInputComponentsToDecryptValue(parameterDGView.Rows(e.RowIndex).Cells(8).Value, e.RowIndex)
        parameterDGView.Rows(e.RowIndex).Cells(9).Value = GenerateEncryptValue(parameterDGView.Rows(e.RowIndex).Cells(8).Value)
    End Sub


    Public Property pConnectionStringModelList() As List(Of ConnectionStringModel)
        Get
            Return m_pConnectionStringModelList
        End Get
        Set(ByVal value As List(Of ConnectionStringModel))
            m_pConnectionStringModelList = value
        End Set
    End Property

    Private Sub EnableApplyToAllCBox_CheckedChanged(sender As Object, e As EventArgs) Handles EnableApplyToAllCBox.CheckedChanged
        For rCounter As Int32 = 0 To parameterDGView.Rows.Count - 1
            If (parameterDGView.Rows(rCounter).Cells(0).Value) Then     'verified
                Continue For
            End If
            parameterDGView.Rows(rCounter).Cells(2).Value = EnableApplyToAllCBox.Checked
        Next
    End Sub

    Private Sub parameterDGView_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles parameterDGView.CellContentClick
        If (m_bInit) Then
            Return
        End If
        parameterDGView.CommitEdit(DataGridViewDataErrorContexts.Commit)
    End Sub


    Private Sub parameterDGView_CellValueChanged(sender As Object, e As DataGridViewCellEventArgs) Handles parameterDGView.CellValueChanged
        If m_bInit Then
            Return
        End If

        UpdateVerifiedChanges(e.RowIndex)
        CheckAndSetSaveBtnStatus()
    End Sub

    Private Sub UpdateVerifiedChanges(ByVal _rowIndex As Integer)
        For iii As Integer = 2 To parameterDGView.Rows(_rowIndex).Cells.Count - 1
            parameterDGView.Rows(_rowIndex).Cells(iii).ReadOnly = parameterDGView.Rows(_rowIndex).Cells(0).Value

            If (parameterDGView.Rows(_rowIndex).Cells(0).Value) Then
                parameterDGView.Rows(_rowIndex).DefaultCellStyle.BackColor = Color.LightGray
            Else
                parameterDGView.Rows(_rowIndex).DefaultCellStyle.BackColor = Color.White
            End If

            If parameterDGView.Rows(_rowIndex).Cells(0).Value Then
                parameterDGView.Rows(_rowIndex).Cells(2).Value = False
            End If
        Next
    End Sub

    Private Sub parameterDGView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles parameterDGView.CellDoubleClick
        If (e.ColumnIndex = 8) Then
            MessageBox.Show(parameterDGView.Rows(e.RowIndex).Cells(e.ColumnIndex).Value, "Connection String Value", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub ResetParameterDGViewToDefault()
        parameterDGView.Rows.Clear()
        m_pConnectionStringModelList.Clear()
        Dim tmpConnectionStringModel As ConnectionStringModel
        For Each tmpString As String In m_pConnectionStringParamList
            tmpConnectionStringModel = New ConnectionStringModel()
            tmpConnectionStringModel.strParamName = tmpString.Trim()
            m_pConnectionStringModelList.Add(tmpConnectionStringModel)

            parameterDGView.Rows.Add(False, tmpConnectionStringModel.strParamName, True, "", _
                         "", "", "", _
                         "", "", "")
        Next
    End Sub

    Private Sub AutoDataFillBtn_Click(sender As Object, e As EventArgs) Handles AutoDataFillBtn.Click
        m_pParamScanningModel = New ParamScanningModel
        m_pParamScanningModel.pDefaultSetDic = New Dictionary(Of String, String)
        m_pParamScanningModel.pSynDic = New Dictionary(Of String, Boolean)
        For Each tmpConnStrModel As ConnectionStringModel In m_pConnectionStringModelList
            m_pParamScanningModel.pDefaultSetDic.Add(tmpConnStrModel.strParamName, ConfigFileScanner.ScanForParameterValue(m_pConfigFileViewModel, tmpConnStrModel.strParamName))
            m_pParamScanningModel.pSynDic.Add(tmpConnStrModel.strParamName, ConfigFileScanner.ScanIfAllFileHasTheSameParamValue(m_pConfigFileViewModel, m_pParamScanningModel, tmpConnStrModel.strParamName))
        Next
        Dim UpdateRows = From UIParamList In parameterDGView.Rows.Cast(Of DataGridViewRow)()
                         Join ScanningList In m_pParamScanningModel.pSynDic
                         On UIParamList.Cells(1).Value Equals ScanningList.Key
                         Where ScanningList.Value = True
        If (UpdateRows.Count > 0) Then
            ResetParameterDGViewToDefault()
            For rowNum = 0 To parameterDGView.Rows.Count - 1
                If m_pParamScanningModel.pSynDic(parameterDGView.Rows(rowNum).Cells(1).Value) Then
                    parameterDGView.Rows(rowNum).Cells(9).Value = m_pParamScanningModel.pDefaultSetDic(parameterDGView.Rows(rowNum).Cells(1).Value)
                    parameterDGView.Rows(rowNum).Cells(8).Value = m_pFeService.DecryptString(parameterDGView.Rows(rowNum).Cells(9).Value, m_pGeneratorFormModel.strDecryptedKey)
                    ExtractCoonnectionStringtoInput(rowNum, parameterDGView.Rows(rowNum).Cells(8).Value)
                End If
            Next
        End If
    End Sub

    Private Sub ExtractCoonnectionStringtoInput(ByVal _iRowNum As Integer, ByVal _inStrConnString As String)
        Dim pParamDic = HelperFunctions.ConnectionStringSpliter(_inStrConnString)
        Dim tmpRefString As String = Nothing

        If (pParamDic.TryGetValue(SysConst.DB_CONNSTR_USERID, tmpRefString)) Then
            parameterDGView.Rows(_iRowNum).Cells(3).Value = tmpRefString
        End If

        If (pParamDic.TryGetValue(SysConst.DB_CONNSTR_PWD, tmpRefString)) Then
            parameterDGView.Rows(_iRowNum).Cells(4).Value = tmpRefString
        End If

        If (pParamDic.TryGetValue(SysConst.DB_CONNSTR_DATASOURCE, tmpRefString)) Then
            parameterDGView.Rows(_iRowNum).Cells(5).Value = tmpRefString
        End If

        If (pParamDic.TryGetValue(SysConst.DB_CONNSTR_INITCATEGORY, tmpRefString)) Then
            parameterDGView.Rows(_iRowNum).Cells(6).Value = tmpRefString
        End If

        If (pParamDic.TryGetValue(SysConst.DB_CONNSTR_MAXPOOLSIZE, tmpRefString)) Then
            parameterDGView.Rows(_iRowNum).Cells(7).Value = tmpRefString
        End If
    End Sub

    Private Sub verifyAllCBox_CheckedChanged(sender As Object, e As EventArgs) Handles verifyAllCBox.CheckedChanged
        For rowNum As Integer = 0 To parameterDGView.Rows.Count - 1
            parameterDGView.Rows(rowNum).Cells(0).Value = verifyAllCBox.Checked
        Next
    End Sub

    Private Sub ParamEditForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class