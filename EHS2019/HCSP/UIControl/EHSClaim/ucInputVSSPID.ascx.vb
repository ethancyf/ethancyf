Imports Common
Imports Common.Component
Imports Common.Component.ClaimRules.ClaimRulesBLL
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports HCSP.BLL
Imports System.Web.Security.AntiXss

Partial Public Class ucInputVSSPID
    Inherits ucInputEHSClaimBase


    'Events 
    Public Event SearchPIDClick(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    Dim _udtSessionHandler As New SessionHandler

#Region "Constants"
    Public Class PID_DOCUMENTARYPROOF
        Public Const PID_INSTITUTION_CERT As String = "PID_I_CERT"
    End Class

    Public Class RCH_TYPE
        Public Const ALL As String = ""
        Public Const PID As String = "I"
    End Class
#End Region

#Region "Properties"
    Public ReadOnly Property DocumentaryProof() As String
        Get
            Return Me.Request.Form(Me.ddlDocumentaryProof.UniqueID)
        End Get
    End Property

    Public ReadOnly Property PIDInstitutionCode() As String
        Get
            Return Me.Request.Form(Me.txtPIDInstitutionCode.UniqueID)
        End Get
    End Property
#End Region

#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        'DocumentaryProof
        Me.lblDocumentaryProofTitle.Text = Me.GetGlobalResourceObject("Text", "TypeOfDocumentaryProof")

        'PID Instituation Field

        Me.lblPIDInstitutionCodeText.Text = Me.GetGlobalResourceObject("Text", "PIDInstitutionCode")
        Me.lblPIDInstitutionNameText.Text = Me.GetGlobalResourceObject("Text", "PIDInstitutionName")

        If Me.SessionHandler.Language() = Common.Component.CultureLanguage.TradChinese Then
            Me.lblPIDInstitutionName.Visible = False
            Me.lblPIDInstitutionNameChi.Visible = True
            Me.lblPIDInstitutionNameChi.CssClass = "tableTextChi"
        Else
            Me.lblPIDInstitutionName.Visible = True
            Me.lblPIDInstitutionName.CssClass = "tableText"
            Me.lblPIDInstitutionNameChi.Visible = False
        End If
    End Sub

    Protected Overrides Sub Setup()
        'BindDocumentaryProof(MyBase.EHSClaimVaccine)
        If ddlDocumentaryProof.SelectedValue <> String.Empty AndAlso ddlDocumentaryProof.SelectedValue = PID_DOCUMENTARYPROOF.PID_INSTITUTION_CERT Then
            panPIDInstitutionCode.Visible = True
        End If
        'Fill data
        'If Not IsPostBack Then
        '    FillClaimDetail()
        'End If
    End Sub

#End Region

#Region "Set Up Error Image"

    Public Sub SetDocumentaryProofError(ByVal visible As Boolean)
        Me.imgDocumentaryProofError.Visible = visible
    End Sub

    Public Sub SetPIDCodeError(ByVal visible As Boolean)
        Me.imgPIDInstitutionCodeError.Visible = visible
    End Sub

#End Region

#Region "Set Value"
    Public Sub EnableDocumentaryProof(ByVal blnEnabled As Boolean)
        Me.ddlDocumentaryProof.Enabled = blnEnabled
        Me.txtPIDInstitutionCode.Enabled = blnEnabled
        Me.panPIDInstitutionCode.Enabled = blnEnabled
    End Sub

    Public Sub SetDocumentaryProofOptions(ByVal strSelectedOptions As String, Optional ByVal strPIDInstitutionCode As String = "")
        If Me.ddlDocumentaryProof.Enabled = True Then
            If strSelectedOptions <> String.Empty Then
                Me.ddlDocumentaryProof.SelectedValue = strSelectedOptions

                If strSelectedOptions = PID_DOCUMENTARYPROOF.PID_INSTITUTION_CERT Then
                    Me.panPIDInstitutionCode.Visible = True
                    Me.txtPIDInstitutionCode.Text = strPIDInstitutionCode
                    Me.lookUpRCHCode()
                End If
            Else
                Me.ddlDocumentaryProof.SelectedIndex = -1
                Me.panPIDInstitutionCode.Visible = False
                Me.txtPIDInstitutionCode.Text = String.Empty
                Me.lblPIDInstitutionName.Text = String.Empty
                Me.lblPIDInstitutionNameChi.Text = String.Empty
            End If
        Else
            Me.ddlDocumentaryProof.SelectedIndex = -1
            Me.panPIDInstitutionCode.Visible = False
            Me.txtPIDInstitutionCode.Text = String.Empty
            Me.lblPIDInstitutionName.Text = String.Empty
            Me.lblPIDInstitutionNameChi.Text = String.Empty
        End If
    End Sub

    Public Sub FillClaimDetail()

        '    'Select Case mvCategory.ActiveViewIndex
        '    '    Case ViewIndexCategory.VSS_PID
        '    If Not _udtSessionHandler.DocumentaryProofForPIDGetFromSession(ucInputVSS.FunctCode) Is Nothing And _
        '        Not _udtSessionHandler.DocumentaryProofForPIDGetFromSession(ucInputVSS.FunctCode) = String.Empty Then

        '        Dim strDocumentaryProofValue As String = _udtSessionHandler.DocumentaryProofForPIDGetFromSession(ucInputVSS.FunctCode)

        '        If strDocumentaryProofValue = ucInputVSSPID.PID_DOCUMENTARYPROOF.PID_INSTITUTION_CERT Then
        If Not _udtSessionHandler.PIDInstitutionCodeGetFromSession(ucInputVSS.FunctCode) Is Nothing And _
            Not _udtSessionHandler.PIDInstitutionCodeGetFromSession(ucInputVSS.FunctCode) = String.Empty Then

            Dim strPIDInstitutionCode As String = _udtSessionHandler.PIDInstitutionCodeGetFromSession(ucInputVSS.FunctCode)
            Me.SetPIDCode(strPIDInstitutionCode)
            '            Else
            'Me.SetPIDCode(strDocumentaryProofValue)
        End If
        '        Else
        '            Me.SetDocumentaryProofOptions(strDocumentaryProofValue)
        '        End If
        '    End If
        '    'End Select

    End Sub


#End Region

#Region "Events"
    Private Sub btnSearchPID_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles btnSearchPID.Click
        RaiseEvent SearchPIDClick(sender, e)
    End Sub

    Private Sub ddlDocumentaryProof_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlDocumentaryProof.SelectedIndexChanged
        Dim ddlDocumentaryProof As DropDownList = CType(sender, DropDownList)

        If Not ddlDocumentaryProof Is Nothing Then
            If ddlDocumentaryProof.SelectedValue.Trim = PID_DOCUMENTARYPROOF.PID_INSTITUTION_CERT Then
                Me.FillClaimDetail()
                panPIDInstitutionCode.Visible = True
            Else
                panPIDInstitutionCode.Visible = False
            End If

        End If

    End Sub
#End Region

    Private Sub txtRCHCodeText_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPIDInstitutionCode.TextChanged
        If Me.txtPIDInstitutionCode.Text.Trim() = String.Empty Then
            Me.lblPIDInstitutionCode.Text = String.Empty
            Me.lblPIDInstitutionName.Text = String.Empty
            Me.lblPIDInstitutionNameChi.Text = String.Empty

            _udtSessionHandler.PIDInstitutionCodeRemoveFromSession(ucInputVSS.FunctCode)
        Else
            Me.lookUpRCHCode()
        End If
    End Sub

    Private Sub lookUpRCHCode()
        Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
        Dim dtResult As DataTable = udtRVPHomeListBLL.getRVPHomeListActiveByCode(Me.txtPIDInstitutionCode.Text.Trim(), RCH_TYPE.PID)
        'Dim udtSessionHandler As New BLL.SessionHandler()

        If dtResult.Rows.Count > 0 Then
            Me.SetUpRCHInfo(dtResult.Rows(0))
            _udtSessionHandler.PIDInstitutionCodeSaveToSession(dtResult.Rows(0)("RCH_Code").ToString().Trim().ToUpper(), ucInputVSS.FunctCode)
        Else
            Me.lblPIDInstitutionCode.Text = String.Empty
            Me.lblPIDInstitutionName.Text = String.Empty
            Me.lblPIDInstitutionNameChi.Text = String.Empty
            '_udtSessionHandler.PIDInstitutionCodeRemoveFromSession(ucInputVSS.FunctCode)
        End If
    End Sub

    Private Sub SetUpRCHInfo(ByVal drRVPHome As DataRow)

        Me.txtPIDInstitutionCode.Text = drRVPHome("RCH_Code").ToString().Trim().ToUpper()
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Winnie]
        Me.lblPIDInstitutionCode.Text = AntiXssEncoder.HtmlEncode(Me.txtPIDInstitutionCode.Text, True)
        ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Winnie]

        Me.lblPIDInstitutionName.Text = drRVPHome("Homename_Eng").ToString().Trim()
        If drRVPHome.IsNull("Homename_Chi") Then
            Me.lblPIDInstitutionNameChi.Text = Me.lblPIDInstitutionName.Text
            Me.lblPIDInstitutionNameChi.CssClass = "tableText"
        Else
            Me.lblPIDInstitutionNameChi.Text = drRVPHome("Homename_Chi").ToString().Trim()
            Me.lblPIDInstitutionNameChi.CssClass = "tableTextChi"
        End If
    End Sub

#Region "SetValue"

    Public Sub SetPIDCode(ByVal strPIDCode As String)
        Me.txtPIDInstitutionCode.Text = strPIDCode.Trim().ToUpper()
        Me.lookUpRCHCode()
    End Sub

#End Region

#Region "Document Proof"

    Public Sub BindDocumentaryProof(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel)
        Dim udtStaticDataBLL As New StaticDataBLL

        ' Dim dtCoPaymentOption As DataTable
        Dim udtStaticDataModelCollection As StaticDataModelCollection
        Dim intSelectedDocumentaryProofOption As Integer = -1
        Dim blnCheckedDocumentaryProofOption As Boolean = False
        Dim udtEHSTransaction As EHSTransactionModel = MyBase.EHSTransaction
        Dim listItem As ListItem
        Dim blnEnableDropDownListDocumentaryProof As Boolean = False

        udtStaticDataModelCollection = udtStaticDataBLL.GetStaticDataListByColumnName("VSSPID_DOCUMENTARYPROOF")

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

                'If no avaiblie subsidy for claim, the drop-down list is not enabled.
                If Not udtEHSClaimVaccine Is Nothing AndAlso udtEHSClaimVaccine.SubsidizeList.Count > 0 Then
                    For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                        'If udtEHSClaimSubsidize.Available Then
                        blnEnableDropDownListDocumentaryProof = blnEnableDropDownListDocumentaryProof Or udtEHSClaimSubsidize.Available
                        'End If
                    Next

                    If blnEnableDropDownListDocumentaryProof Then
                        chkDocumentaryProof.Enabled = True
                    Else
                        chkDocumentaryProof.Enabled = False
                        chkDocumentaryProof.Checked = False
                    End If
                End If
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
            If objMsgBox IsNot Nothing Then
                objMsgBox.AddMessage(objMsg)
            End If

            blnResult = False
        End If

        Return blnResult
    End Function

    Public Function ValidateDocumentaryProof(ByVal blnShowErrorImage As Boolean) As ComObject.SystemMessage

        Me.imgDocumentaryProofError.Visible = False
        Me.imgPIDInstitutionCodeError.Visible = False

        If String.IsNullOrEmpty(Me.DocumentaryProof) = True Then
            Me.imgDocumentaryProofError.Visible = blnShowErrorImage
            Return New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00360) ' Please select "Document Proof"
        ElseIf Me.DocumentaryProof = PID_DOCUMENTARYPROOF.PID_INSTITUTION_CERT Then
            If txtPIDInstitutionCode.Text = String.Empty Then
                Me.imgPIDInstitutionCodeError.Visible = blnShowErrorImage
                Return New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00387) ' Please input "PID Institution Code"
            Else
                'Check "PID Institution Code" Valid
                Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
                Dim dtResult As DataTable = udtRVPHomeListBLL.getRVPHomeListActiveByCode(txtPIDInstitutionCode.Text.Trim, RCH_TYPE.PID)
                If dtResult.Rows.Count = 0 Then
                    '_udtSessionHandler.RVPRCHCodeRemoveFromSession(ucInputVSS.FunctCode)
                    Me.imgPIDInstitutionCodeError.Visible = blnShowErrorImage
                    Return New ComObject.SystemMessage(Common.Component.FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00388) ' "PID Institution Code" is invalid
                End If
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
                udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
                udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
                udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
                udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                '_udtSessionHandler.DocumentaryProofForPIDSaveToSession(Me.DocumentaryProof, ucInputVSS.FunctCode)

                If Me.DocumentaryProof = PID_DOCUMENTARYPROOF.PID_INSTITUTION_CERT And panPIDInstitutionCode.Visible = True Then
                    udtTransactAdditionfield = New TransactionAdditionalFieldModel()
                    udtTransactAdditionfield.AdditionalFieldID = TransactionAdditionalFieldModel.AdditionalFieldType.PIDInstitutionCode
                    udtTransactAdditionfield.AdditionalFieldValueCode = Me.PIDInstitutionCode
                    udtTransactAdditionfield.AdditionalFieldValueDesc = Nothing
                    udtTransactAdditionfield.SchemeCode = udtSubsidizeLatest.SchemeCode
                    udtTransactAdditionfield.SchemeSeq = udtSubsidizeLatest.SchemeSeq
                    udtTransactAdditionfield.SubsidizeCode = udtSubsidizeLatest.SubsidizeCode
                    udtEHSTransaction.TransactionAdditionFields.Add(udtTransactAdditionfield)

                    _udtSessionHandler.PIDInstitutionCodeSaveToSession(Me.PIDInstitutionCode, ucInputVSS.FunctCode)
                End If
            End If

        End If

    End Sub

#End Region

End Class
