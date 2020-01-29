Imports Common
Imports Common.Component
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData


Partial Public Class ucInputVSSDA
    Inherits ucInputEHSClaimBase

    Private _SingleDocumentaryProofValue As String

#Region "Properties"
    Public ReadOnly Property MulitDocumentaryProof() As String
        Get
            Return Me.Request.Form(Me.ddlDocumentaryProof.UniqueID)
        End Get
    End Property

    Public Property SingleDocumentaryProof() As String
        Get
            Return _SingleDocumentaryProofValue
        End Get
        Set(value As String)
            _SingleDocumentaryProofValue = value
        End Set
    End Property
#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        'DocumentaryProof
        Me.lblDocumentaryProofTitle.Text = Me.GetGlobalResourceObject("Text", "TypeOfDocumentaryProof")
    End Sub

    Protected Overrides Sub Setup()
        BindDocumentaryProof(MyBase.EHSClaimVaccine)

    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetDocumentaryProofError(ByVal visible As Boolean)
        Me.imgDocumentaryProofError.Visible = visible
    End Sub

#End Region

#Region "Set Value"
    Public Sub EnableDocumentaryProof(ByVal blnEnabled As Boolean)
        Me.chkDocumentaryProof.Enabled = blnEnabled
        Me.ddlDocumentaryProof.Enabled = blnEnabled
    End Sub

    Public Sub SetDocumentaryProofOptions(ByVal strSelectedOptions As String)
        Dim udtStaticDataBLL As New StaticDataBLL
        Dim udtStaticDataModelCollection As StaticDataModelCollection

        udtStaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("VSSDA_DOCUMENTARYPROOF")

        If strSelectedOptions <> String.Empty Then
            If udtStaticDataModelCollection.Count > 0 Then
                If udtStaticDataModelCollection.Count > 1 Then
                    Me.ddlDocumentaryProof.SelectedValue = strSelectedOptions
                Else
                    Dim udtStaticData As StaticDataModel = udtStaticDataModelCollection.Item(0)

                    If udtStaticData.ItemNo = strSelectedOptions Then
                        Me.chkDocumentaryProof.Checked = True
                    End If
                End If
            End If
        Else
            If udtStaticDataModelCollection.Count > 0 Then
                If udtStaticDataModelCollection.Count > 1 Then
                    Me.ddlDocumentaryProof.SelectedIndex = -1
                Else
                    Me.chkDocumentaryProof.Checked = False
                End If
            End If
        End If
    End Sub

#End Region

#Region "Events"

#End Region

#Region "Document Proof"

    Private Sub BindDocumentaryProof(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel)
        Dim udtStaticDataBLL As New StaticDataBLL

        ' Dim dtCoPaymentOption As DataTable
        Dim udtStaticDataModelCollection As StaticDataModelCollection
        Dim intSelectedDocumentaryProofOption As Integer = -1
        Dim blnCheckedDocumentaryProofOption As Boolean = False
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.EHSTransaction
        Dim listItem As ListItem
        Dim blnEnableDropDownListDocumentaryProof As Boolean = False

        udtStaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("VSSDA_DOCUMENTARYPROOF")

        If udtStaticDataModelCollection.Count > 0 Then
            If udtStaticDataModelCollection.Count > 1 Then
                Me.ddlDocumentaryProof.Visible = True
                Me.chkDocumentaryProof.Visible = False

                ' Save the User Input before clear it
                If Me.ddlDocumentaryProof.SelectedIndex > -1 AndAlso String.IsNullOrEmpty(Me.ddlDocumentaryProof.SelectedValue) = False Then
                    intSelectedDocumentaryProofOption = ddlDocumentaryProof.SelectedIndex
                End If

                ddlDocumentaryProof.Items.Clear()

                ddlDocumentaryProof.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "EHSClaimPleaseSelect"), String.Empty))

                For Each udtStaticData As StaticDataModel In udtStaticDataModelCollection
                    listItem = New ListItem

                    If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                        listItem.Text = udtStaticData.DataValueChi.ToString
                    ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
                        listItem.Text = udtStaticData.DataValueCN.ToString
                    Else
                        listItem.Text = udtStaticData.DataValue.ToString
                    End If

                    listItem.Value = udtStaticData.ItemNo

                    ddlDocumentaryProof.Items.Add(listItem)
                Next

                ' Restore the User Input after clear it
                ddlDocumentaryProof.SelectedIndex = intSelectedDocumentaryProofOption

                ' Special Handling: Retain User Input for the event - [btnStep2bBack_Click] because it would clear all User Input
                If Me.AvaliableForClaim Then
                    If Not udtEHSTransaction Is Nothing Then
                        If Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                            If udtEHSTransaction.TransactionAdditionFields.Count > 0 Then
                                For Each udtTransactionAdditionField As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
                                    Select Case udtTransactionAdditionField.AdditionalFieldID
                                        Case TransactionAdditionalFieldModel.AdditionalFieldType.DocumentaryProof
                                            Me.ddlDocumentaryProof.SelectedValue = udtTransactionAdditionField.AdditionalFieldValueCode
                                    End Select
                                Next
                            End If
                        End If
                    End If
                End If

                'If no avaiblie subsidy for claim, the drop-down list is not enabled.
                If Not udtEHSClaimVaccine Is Nothing AndAlso udtEHSClaimVaccine.SubsidizeList.Count > 0 Then
                    For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                        'If udtEHSClaimSubsidize.Available Then
                        blnEnableDropDownListDocumentaryProof = blnEnableDropDownListDocumentaryProof Or udtEHSClaimSubsidize.Available
                        'End If
                    Next

                    If blnEnableDropDownListDocumentaryProof Then
                        ddlDocumentaryProof.Enabled = True
                    Else
                        ddlDocumentaryProof.Enabled = False
                        ddlDocumentaryProof.SelectedIndex = -1
                    End If
                End If
            Else
                Me.ddlDocumentaryProof.Visible = False
                Me.chkDocumentaryProof.Visible = True

                ' Save the User Input before clear it
                If Me.chkDocumentaryProof.Checked Then
                    blnCheckedDocumentaryProofOption = True
                End If

                Dim udtStaticData As StaticDataModel = udtStaticDataModelCollection.Item(0)

                If MyBase.SessionHandler.Language = CultureLanguage.TradChinese Then
                    chkDocumentaryProof.Text = udtStaticData.DataValueChi.ToString
                ElseIf MyBase.SessionHandler.Language = CultureLanguage.SimpChinese Then
                    chkDocumentaryProof.Text = udtStaticData.DataValueCN.ToString
                Else
                    chkDocumentaryProof.Text = udtStaticData.DataValue.ToString
                End If

                ' Restore the User Input after clear it
                chkDocumentaryProof.Checked = blnCheckedDocumentaryProofOption

                Me.SingleDocumentaryProof = udtStaticData.ItemNo

                '' Special Handling: Retain User Input for the event - [btnStep2bBack_Click] because it would clear all User Input
                'If Me.AvaliableForClaim Then
                '    If Not udtEHSTransaction Is Nothing Then
                '        If Not udtEHSTransaction.TransactionAdditionFields Is Nothing Then
                '            If udtEHSTransaction.TransactionAdditionFields.Count > 0 Then
                '                For Each udtTransactionAdditionField As TransactionAdditionalFieldModel In udtEHSTransaction.TransactionAdditionFields
                '                    Select Case udtTransactionAdditionField.AdditionalFieldID
                '                        Case TransactionAdditionalFieldModel.AdditionalFieldType.DocumentaryProof
                '                            Me.ddlDocumentaryProof.SelectedValue = udtTransactionAdditionField.AdditionalFieldValueCode
                '                    End Select
                '                Next
                '            End If
                '        End If
                '    End If
                'End If

                ''If no avaiblie subsidy for claim, the drop-down list is not enabled.
                'If Not udtEHSClaimVaccine Is Nothing AndAlso udtEHSClaimVaccine.SubsidizeList.Count > 0 Then
                '    For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                '        'If udtEHSClaimSubsidize.Available Then
                '        blnEnableDropDownListDocumentaryProof = blnEnableDropDownListDocumentaryProof Or udtEHSClaimSubsidize.Available
                '        'End If
                '    Next

                '    If blnEnableDropDownListDocumentaryProof Then
                '        ddlDocumentaryProof.Enabled = True
                '    Else
                '        ddlDocumentaryProof.Enabled = False
                '        ddlDocumentaryProof.SelectedIndex = -1
                '    End If
                'End If
            End If
        End If

    End Sub

#End Region

#Region "UI Input Validation"
    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Dim objMsg As ComObject.SystemMessage = Nothing
        Dim blnResult As Boolean = True

        objMsg = ValidateDocumentaryProof(blnShowErrorImage)
        If objMsg IsNot Nothing Then
            If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
            blnResult = False
        End If

        Return blnResult
    End Function

    Public Function ValidateDocumentaryProof(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage

        Me.imgDocumentaryProofError.Visible = False

        If Me.ddlDocumentaryProof.Visible = True Then
            If String.IsNullOrEmpty(Me.MulitDocumentaryProof) = True Then
                Me.imgDocumentaryProofError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage("990000", "E", "00360") ' Please select "Document Proof"
            End If
        End If

        If Me.chkDocumentaryProof.Visible = True Then
            If Me.chkDocumentaryProof.Checked = False Then
                Me.imgDocumentaryProofError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage("990000", "E", "00360") ' Please select "Document Proof"
            End If
        End If

        Return Nothing
    End Function

#End Region

#Region "Save"

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel, ByVal udtEHSClaimVaccine As EHSClaimVaccineModel)
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        Dim strDocumentaryProof As String = String.Empty

        If ddlDocumentaryProof.Visible Then
            strDocumentaryProof = MulitDocumentaryProof
        ElseIf chkDocumentaryProof.Visible Then
            strDocumentaryProof = SingleDocumentaryProof
        End If

        'Document Proof
        If strDocumentaryProof <> String.Empty Then
            ' -----------------------------------------------
            ' Get Latest SchemeSeq Selected
            '------------------------------------------------
            Dim udtSubsidizeLatest As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.GetLatestSelectedSubsidize

            If Not udtSubsidizeLatest Is Nothing Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.DocumentaryProof
                udtTransactAdditionfield.AdditionalFieldValueCode = strDocumentaryProof
                udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
                udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            End If
        End If

    End Sub

#End Region

End Class
