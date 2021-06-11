Imports Common
Imports Common.Component
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSAccount
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData


Partial Public Class ucInputVSSDA
    Inherits ucInputEHSClaimBase

    Private _SingleDocumentaryProofValue As String

#Region "Constants"

    'CRE20-009 Declare the constant variable for documentaryProof [Start][Nichole]
    Public Class VSS_DOCUMENTARYPROOF
        Public Const VSS_CSSA_CERT As String = "CSSA_CERT"
        Public Const VSS_ANNEX_PAGE As String = "ANNEX_PAGE"
        Public Const VSS_LETTER_DA As String = "LETTER_DA"
    End Class
    'CRE20-009 Declare the constant variable for documentaryProof [End][Nichole]

#End Region

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
        'CRE20-00 remove the setup to call the databind function for ddldocumentryProof [Start][Nichole]
        'BindDocumentaryProof(MyBase.EHSClaimVaccine)
        'CRE20-00 remove the setup to call the databind function for ddldocumentryProof [End][Nichole]
    End Sub

    Protected Overrides Sub Setup(ByVal blnPostbackRebuild As Boolean)

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
    'CRE20-009 add the selectedIndexChanged function for ddlDocumentaryProof [Start][Nichole]
    Private Sub ddlDocumentaryProof_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDocumentaryProof.SelectedIndexChanged


        If Not ddlDocumentaryProof Is Nothing Then
            If ddlDocumentaryProof.SelectedValue.Trim = VSS_DOCUMENTARYPROOF.VSS_CSSA_CERT Or ddlDocumentaryProof.SelectedValue.Trim = VSS_DOCUMENTARYPROOF.VSS_ANNEX_PAGE Then
                Me.panVSSDAConfirm.Visible = True
            Else
                Me.panVSSDAConfirm.Visible = False
            End If
        End If
    End Sub
    'CRE20-009 add the selectedIndexChanged function for ddlDocumentaryProof [End][Nichole]
#End Region

#Region "Document Proof"
    'CRE20-009 VSS Da with CSSA [Start][Nichole]
    'change the private to public
    Public Sub BindDocumentaryProof(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel, ByVal strServiceDate As String)
        'CRE20-009 VSS Da with CSSA [End][Nichole]
        Dim udtStaticDataBLL As New StaticDataBLL

        ' Dim dtCoPaymentOption As DataTable
        Dim udtStaticDataModelCollection As StaticDataModelCollection
        Dim intSelectedDocumentaryProofOption As Integer = -1
        Dim blnCheckedDocumentaryProofOption As Boolean = False
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.EHSTransaction
        Dim listItem As ListItem
        Dim blnEnableDropDownListDocumentaryProof As Boolean = False


        'udtStaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("VSSDA_DOCUMENTARYPROOF")
        'CRE20-009 VSS Disabled with CSSA [Start][Nichole]
        udtStaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByDocProof("VSSDA_DOCUMENTARYPROOF", strServiceDate)
        'CRE20-009 VSS Disabled with CSSA [End][Nichole]

        If udtStaticDataModelCollection.Count > 0 Then
            If udtStaticDataModelCollection.Count > 1 Then
                Me.ddlDocumentaryProof.Visible = True
                Me.chkDocumentaryProof.Visible = False

                ' Save the User Input before clear it
                If Me.ddlDocumentaryProof.SelectedIndex > -1 AndAlso String.IsNullOrEmpty(Me.ddlDocumentaryProof.SelectedValue) = False Then
                    intSelectedDocumentaryProofOption = ddlDocumentaryProof.SelectedIndex
                End If

                ddlDocumentaryProof.Items.Clear()

                'CRE20-009 VSS DA with CSSA [Start][Nichole]
                ddlDocumentaryProof.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "PleaseSelect"), String.Empty))
                'CRE20-009 VSS DA with CSSA [End][Nichole]

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

                'CRE20-009 make the cssa and Annex checkbox invisible [Start][Nichole]
                Me.panVSSDAConfirm.Visible = False
                'CRE20-009 make the cssa and Annex checkbox invisible [End][Nichole]


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
        'Dim objMsg As ComObject.SystemMessage = Nothing
        'Dim blnResult As Boolean = True

        'objMsg = ValidateDocumentaryProof(blnShowErrorImage)
        'If objMsg IsNot Nothing Then
        '    If objMsgBox IsNot Nothing Then objMsgBox.AddMessage(objMsg)
        '    blnResult = False
        'End If

        'CRE20-009 VSS Da With CSSA [Start][Nichole]
        Dim objMsg As New List(Of ComObject.SystemMessage)
        Dim blnResult As Boolean = True

        objMsg = ValidateDocumentaryProof(blnShowErrorImage)
        If objMsg IsNot Nothing Then
            If objMsgBox IsNot Nothing Then
                ' objMsgBox.AddMessage(objMsg)
                For Each udtSysMsg As Common.ComObject.SystemMessage In objMsg
                    objMsgBox.AddMessage(udtSysMsg)
                Next
            End If

            blnResult = False
        End If
        'CRE20-009 VSS Da With CSSA [End][Nichole]

        Return blnResult
    End Function

    Public Function ValidateDocumentaryProof(ByVal blnShowErrorImage As Boolean) As List(Of ComObject.SystemMessage) 'As ComObject.SystemMessage

        Me.imgDocumentaryProofError.Visible = False
        'CRE20-009 declare system message object & warning image setting [Start][Nichole]
        Me.imgVSSDAConfirmCSSAError.Visible = False
        Me.imgVSSDAConfirmAnnexError.Visible = False
        ' Dim objMsg As ComObject.SystemMessage = Nothing
        Dim objMsg As New List(Of ComObject.SystemMessage)

        Dim blnResult As Boolean = True
        'CRE20-009 declare system message object & warning image setting [End][Nichole]

        If Me.ddlDocumentaryProof.Visible = True Then
            If String.IsNullOrEmpty(Me.MulitDocumentaryProof) = True Then
                Me.imgDocumentaryProofError.Visible = blnShowErrorImage
                objMsg.Add(New ComObject.SystemMessage("990000", "E", "00360")) ' Please select "Document Proof"
                blnResult = False
            End If
        End If

        If Me.chkDocumentaryProof.Visible = True Then
            If Me.chkDocumentaryProof.Checked = False Then
                Me.imgDocumentaryProofError.Visible = blnShowErrorImage
                objMsg.Add(New ComObject.SystemMessage("990000", "E", "00360")) ' Please select "Document Proof"
                blnResult = False
            End If
        End If

        'CRE20-009 add validation on checking the checkbox CSSA & Annex have checked or not [Start][Nichole]
        If Me.ddlDocumentaryProof.SelectedValue = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_CSSA_CERT Or Me.ddlDocumentaryProof.SelectedValue = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_ANNEX_PAGE Then
            If Not Me.chkDocProofCSSA.Checked Then
                Me.imgVSSDAConfirmCSSAError.Visible = blnShowErrorImage
                objMsg.Add(New ComObject.SystemMessage("990000", "E", "00453")) ' Please select agreeement of "The true copy of the mentioned type of documentary proof".
                blnResult = False
            End If
        End If

        If Me.ddlDocumentaryProof.SelectedValue = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_CSSA_CERT Or Me.ddlDocumentaryProof.SelectedValue = ucInputVSSDA.VSS_DOCUMENTARYPROOF.VSS_ANNEX_PAGE Then
            If Not Me.chkDocProofAnnex.Checked Then
                Me.imgVSSDAConfirmAnnexError.Visible = blnShowErrorImage
                objMsg.Add(New ComObject.SystemMessage("990000", "E", "00454")) ' Please select agreeement of "Signed a self-declaration".
                blnResult = False
            End If
        End If

        If blnResult = False Then
            Return objMsg
        End If
        'CRE20-009 add validation on checking the checkbox CSSA & Annex have checked or not [End][Nichole]

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
