'CRE15-005-03 (New PIDVSS scheme) [Start][Chris YIM]
'-----------------------------------------------------------------------------------------
Imports Common
Imports Common.Component
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData


Partial Public Class ucInputPIDVSS
    Inherits ucInputEHSClaimBase

#Region "Properties"
    Public ReadOnly Property DocumentaryProof() As String
        Get
            Return Me.Request.Form(Me.ddlDocumentaryProof.UniqueID)
        End Get
    End Property
#End Region

#Region "Event handlers"
    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        Me.udcClaimVaccineInputPIDVSS.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcClaimVaccineInputPIDVSS.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcClaimVaccineInputPIDVSS.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
        Me.udcClaimVaccineInputPIDVSS.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcClaimVaccineInputPIDVSS.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
        Me.udcClaimVaccineInputPIDVSS.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcClaimVaccineInputPIDVSS.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        Me.udcClaimVaccineInputPIDVSS.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")

        'DocumentaryProof
        Me.lblDocumentaryProofTitle.Text = Me.GetGlobalResourceObject("Text", "TypeOfDocumentaryProof")
    End Sub

    Protected Overrides Sub Setup()
        Dim formatter As Common.Format.Formatter = New Common.Format.Formatter
        Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = MyBase.EHSAccount.EHSPersonalInformationList(0)
        Dim udtEHSClaim As BLL.EHSClaimBLL = New BLL.EHSClaimBLL
        Dim strDOB As String = formatter.formatDOB(udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, Nothing, Nothing)

        Call BindDocumentaryProofDropDown(MyBase.EHSClaimVaccine)

        Me.udcClaimVaccineInputPIDVSS.Build(MyBase.EHSClaimVaccine)

        AddHandler Me.udcClaimVaccineInputPIDVSS.VaccineLegendClicked, AddressOf udcClaimVaccineInputPIDVSS_VaccineLegendClicked
    End Sub

#Region "Set Up Error Image"

    Public Sub SetDocumentaryProofError(ByVal visible As Boolean)
        Me.imgDocumentaryProofError.Visible = visible
    End Sub

#End Region

    Public Overrides Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        Return Me.udcClaimVaccineInputPIDVSS.SetEHSVaccineModelDoseSelectedFromUIInput(udtEHSClaimVaccine)
    End Function

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)

    End Sub

    Public Overrides Sub SetDoseErrorImage(ByVal blnVisible As Boolean)
        'MyBase.SetDoseErrorImage(blnVisible)
        If Not Me.udcClaimVaccineInputPIDVSS Is Nothing Then
            Me.udcClaimVaccineInputPIDVSS.SetDoseErrorImage(blnVisible)
        End If
    End Sub

#End Region

#Region "Events"

    Protected Sub udcClaimVaccineInputPIDVSS_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

#End Region

#Region "Document Proof"

    Private Sub BindDocumentaryProofDropDown(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel)
        Dim udtStaticDataBLL As New StaticDataBLL

        ' Dim dtCoPaymentOption As DataTable
        Dim udtStaticDataModelCollection As StaticDataModelCollection
        Dim intSelectedDocumentaryProofOption As Integer = -1
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.EHSTransaction
        Dim listItem As ListItem
        Dim blnEnableDropDownListDocumentaryProof As Boolean = False

        udtStaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("PIDVSS_DOCUMENTARYPROOF")

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

    End Sub

#End Region

    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox) As Boolean
        Return Validate(blnShowErrorImage, objMsgBox, True, True)
    End Function

#Region "UI Input Validation"
    Public Function Validate(ByVal blnShowErrorImage As Boolean, ByVal objMsgBox As CustomControls.MessageBox, _
                            ByVal blnAllowEmptyCoPaymentFee As Boolean, ByVal blnAllowEmptyReasonForVisit As Boolean) As Boolean
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

        If String.IsNullOrEmpty(Me.DocumentaryProof) = True Then
            Me.imgDocumentaryProofError.Visible = blnShowErrorImage
            Return New ComObject.SystemMessage("990000", "E", "00360") ' Please select "Document Proof"
        End If

        Return Nothing
    End Function

#End Region

#Region "Save"

    Public Sub Save(ByRef udtEHSTransaction As EHSTransactionModel, ByVal udtEHSClaimVaccine As EHSClaimVaccineModel)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllDistinctSchemeClaim_WithSubsidizeGroup().Filter(MyBase.SchemeClaim.SchemeCode)
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        Dim udtTransactAdditionfield As TransactionAdditionalFieldModel
        udtEHSTransaction.TransactionAdditionFields = New TransactionAdditionalFieldModelCollection()

        'Document Proof
        If Me.DocumentaryProof <> String.Empty Then
            ' -----------------------------------------------
            ' Get Latest SchemeSeq Selected
            '------------------------------------------------
            Dim udtSubsidizeLatest As EHSClaimVaccineModel.EHSClaimSubsidizeModel = udtEHSClaimVaccine.GetLatestSelectedSubsidize

            If Not udtSubsidizeLatest Is Nothing Then
                udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.DocumentaryProof
                udtTransactAdditionfield.AdditionalFieldValueCode = Me.DocumentaryProof
                udtTransactAdditionfield.AdditionalFieldValueDesc = String.Empty
                udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
                udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)
            End If
        End If

    End Sub

#End Region

End Class

'CRE15-005-03 (New PIDVSS scheme) [End][Chris YIM]