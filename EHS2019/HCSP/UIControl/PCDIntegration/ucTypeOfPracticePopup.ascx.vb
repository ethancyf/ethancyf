Imports Common.Component.ServiceProvider
Imports Common.Component.Profession
Imports Common.Component.UserAC
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.PCD.WebService.Interface

Partial Public Class ucTypeOfPracticePopup
    Inherits System.Web.UI.UserControl

    Public Enum enumButtonClick
        Cancel
        CreatePCDAccount
        TransferToPCD
    End Enum

    Public Event ButtonClick(ByVal e As enumButtonClick)

    Private Const VIEWSTATE_SHOWING As String = "SHOWING"

    ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Const SESS_PCDPractice As String = "SESS_PCDPractice"
    ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

#Region "Audit Log Description"
    Public Class AuditLogDesc
        Public Const JoinPCDButtonClick As String = "Type of Practice Popup - Join PCD button click"
        Public Const JoinPCDButtonClick_ID As String = LogID.LOG00300

        Public Const CancelButtonClick As String = "Type of Practice Popup - Cancel button click"
        Public Const CancelButtonClick_ID As String = LogID.LOG00301

        Public Const InvokePCDStart As String = "Type of Practice Popup - Invoke PCD Start"
        Public Const InvokePCDStart_ID As String = LogID.LOG00302

        Public Const InvokePCDEnd As String = "Type of Practice Popup - Invoke PCD End"
        Public Const InvokePCDEnd_ID As String = LogID.LOG00303

        Public Const ValidationFail As String = "Type of Practice Popup - Validation Fail"
        Public Const ValidationFail_ID As String = LogID.LOG00304

    End Class
#End Region

#Region "Private Variables"
    Private _udtAuditLog As AuditLogEntry = Nothing
#End Region

    Public Property Showing() As Boolean
        Get
            Return IIf(Me.ViewState(VIEWSTATE_SHOWING) Is Nothing, False, Me.ViewState(VIEWSTATE_SHOWING))
        End Get
        Set(ByVal value As Boolean)
            Me.ViewState(VIEWSTATE_SHOWING) = value
        End Set
    End Property

    Public Property Mode() As ucTypeOfPracticeGrid.EnumMode
        Get
            Return Me.ucTypeOfPracticeGrid.Mode
        End Get
        Set(ByVal value As ucTypeOfPracticeGrid.EnumMode)
            Me.ucTypeOfPracticeGrid.Mode = value
            SetupHeader()
            SetupButton()
        End Set
    End Property

    ''' <summary>
    ''' Use for handle mouse click and move popup numpad (ModalPopupExtender)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property Header() As Control
        Get
            Return Me.panHeader
        End Get
    End Property

    Public ReadOnly Property ButtonCancel() As ImageButton
        Get
            Return Me.ibtnCancel
        End Get
    End Property

    Private _objPCDCreatePCDSPAcctResult As Common.PCD.WebService.Interface.PCDCreatePCDSPAcctResult = Nothing
    Public ReadOnly Property CreatePCDSPAcctResult() As Common.PCD.WebService.Interface.PCDCreatePCDSPAcctResult
        Get
            Return _objPCDCreatePCDSPAcctResult
        End Get
    End Property

    Private _objTransferPCDResult As Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult = Nothing
    Public ReadOnly Property TransferPCDResult() As Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult
        Get
            Return _objTransferPCDResult
        End Get
    End Property

    Public Sub Reset()
        Me.chkDeclaration.Checked = False
        ClearThirdPartyEnrolmentList()
        SetupButton()
        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Session(SESS_PCDPractice) = Nothing
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
    End Sub

#Region "Event"

    Private Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        udcTMessageBox.Visible = False
        RaiseEvent ButtonClick(enumButtonClick.Cancel)
    End Sub

    Private Sub ibtnCreate_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCreate.Click

        'Me.ModalPopupExtenderNotice.Show()

        If PassedTypeOfPracticeValidation() Then
            InvokePCD_CreatePCDAccount()
            ClearThirdPartyEnrolmentList()
            RaiseEvent ButtonClick(enumButtonClick.CreatePCDAccount)
        Else
            Return
        End If

    End Sub

    Private Sub ibtnTransfer_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnTransfer.Click

        If PassedTypeOfPracticeValidation() Then
            InvokePCD_TransferPCD()
            ClearThirdPartyEnrolmentList()
            RaiseEvent ButtonClick(enumButtonClick.TransferToPCD)
        Else
            Return
        End If

    End Sub

    Private Sub InvokePCD_CreatePCDAccount()
        Dim udtSPBLL As New ServiceProviderBLL
        Dim objPCDWS As New Common.PCD.PCDWebService(CType(Me.Page, BasePage).FunctionCode)
        Dim objResult As Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult = Nothing
        Dim udtSP As ServiceProviderModel = GetSP()

        udtSP.ThirdPartyAdditionalFieldEnrolmentList = Me.ucTypeOfPracticeGrid.GetThirdPartyAdditionalFieldEnrolmentCollection()

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtSP.PracticeList = CType(Session(SESS_PCDPractice), Practice.PracticeModelCollection)
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

        Me._udtAuditLog.AddDescripton("WebMethod", "PCDCreatePCDSPAcct")
        Me._udtAuditLog.WriteStartLog(AuditLogDesc.InvokePCDStart_ID, AuditLogDesc.InvokePCDStart)

        _objPCDCreatePCDSPAcctResult = objPCDWS.PCDCreatePCDSPAcct(udtSP)

        Me._udtAuditLog.AddDescripton("ReturnCode", _objPCDCreatePCDSPAcctResult.ReturnCode)
        Me._udtAuditLog.AddDescripton("MessageID", _objPCDCreatePCDSPAcctResult.MessageID)

        Me._udtAuditLog.WriteEndLog(AuditLogDesc.InvokePCDEnd_ID, AuditLogDesc.InvokePCDEnd)

        Select Case _objPCDCreatePCDSPAcctResult.ReturnCode
            Case PCDCreatePCDSPAcctResult.enumReturnCode.SuccessWithData, _
                  PCDCreatePCDSPAcctResult.enumReturnCode.ServiceProviderAlreadyExisted
                udtSPBLL.DeleteServiceProviderOriginalProfile(udtSP.EnrolRefNo)
            Case Else
                ' Do Nothing
        End Select
    End Sub

    Private Sub InvokePCD_TransferPCD()
        Dim udtSPBLL As New ServiceProviderBLL
        Dim objPCDWS As New Common.PCD.PCDWebService(CType(Me.Page, BasePage).FunctionCode)
        Dim objResult As Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult = Nothing
        Dim udtSP As ServiceProviderModel = GetSP()

        udtSP.ThirdPartyAdditionalFieldEnrolmentList = Me.ucTypeOfPracticeGrid.GetThirdPartyAdditionalFieldEnrolmentCollection()

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtSP.PracticeList = CType(Session(SESS_PCDPractice), Practice.PracticeModelCollection)
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

        Me._udtAuditLog.AddDescripton("WebMethod", "PCDTransferPracticeInfo")
        Me._udtAuditLog.WriteStartLog(AuditLogDesc.InvokePCDStart_ID, AuditLogDesc.InvokePCDStart)

        _objTransferPCDResult = objPCDWS.PCDTransferPracticeInfo(udtSP)

        Me._udtAuditLog.AddDescripton("ReturnCode", _objTransferPCDResult.ReturnCode)
        Me._udtAuditLog.AddDescripton("MessageID", _objTransferPCDResult.MessageID)

        Me._udtAuditLog.WriteEndLog(AuditLogDesc.InvokePCDEnd_ID, AuditLogDesc.InvokePCDEnd)

    End Sub

    Private Sub ClearThirdPartyEnrolmentList()
        Dim udtSP As ServiceProviderModel = GetSP()
        If udtSP.ThirdPartyAdditionalFieldEnrolmentList IsNot Nothing Then
            udtSP.ThirdPartyAdditionalFieldEnrolmentList.Clear()
        End If
    End Sub

    Private Function PassedTypeOfPracticeValidation() As Boolean

        Dim udtAuditLog As New AuditLogEntry(CType(Me.Page, BasePage).FunctionCode)

        udcTMessageBox.Visible = False

        If Not ucTypeOfPracticeGrid.PracticeSelected Then
            Me._udtAuditLog.AddDescripton("Validation", "No practice selected")
            Me._udtAuditLog.WriteLog(AuditLogDesc.ValidationFail_ID, AuditLogDesc.ValidationFail)

            udcTMessageBox.AddMessage(FunctCode.FUNT990003, SeverityCode.SEVE, MsgCode.MSG00501)
        End If

        If Not ucTypeOfPracticeGrid.TypeOfPracticeSelected Then

            Me._udtAuditLog.AddDescripton("Validation", "No type of practice selected")
            Me._udtAuditLog.WriteLog(AuditLogDesc.ValidationFail_ID, AuditLogDesc.ValidationFail)

            udcTMessageBox.AddMessage(FunctCode.FUNT990003, SeverityCode.SEVE, MsgCode.MSG00502)
        End If

        Me.ucTypeOfPracticeGrid.EnableTypeOfPractice()

        If udcTMessageBox.GetCodeTable.Rows.Count <> 0 Then

            udcTMessageBox.ScrollToHeight = False
            udcTMessageBox.BuildMessageBox("ValidationFail", udtAuditLog, LogID.LOG01301, "Input Type of Practice failed")
            Return False
        Else
            Return True
        End If

    End Function

    Private Sub chkDeclaration_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles chkDeclaration.CheckedChanged
        SetupButton()
    End Sub
#End Region

    Public Sub Setup()
        'Me.ModalPopupExtenderNotice.PopupDragHandleControlID = Me.ucNoticePopUp.Header.ClientID
        ' Init audit log object
        Me._udtAuditLog = New AuditLogEntry(CType(Me.Page, BasePage).FunctionCode, Me.Page)
    End Sub

    ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Public Sub LoadPractice()
    Public Sub LoadPractice(ByVal udtPracticeList As Practice.PracticeModelCollection)
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

        Dim udtsp As ServiceProviderModel = GetSP()

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Me.ucTypeOfPracticeGrid.LoadPractice(udtsp)
        Session(SESS_PCDPractice) = udtPracticeList
        Me.ucTypeOfPracticeGrid.LoadPractice(udtsp, udtPracticeList)
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
    End Sub

    Private Function GetSP() As ServiceProviderModel
        Dim bll As New ServiceProviderBLL
        Return bll.GetSP()
    End Function

    Private Sub SetupHeader()
        Select Case Me.Mode
            Case HCSP.ucTypeOfPracticeGrid.EnumMode.Create
                Me.lblTitle.Text = Me.GetGlobalResourceObject("Text", "CreatePCD")
            Case HCSP.ucTypeOfPracticeGrid.EnumMode.Transfer
                Me.lblTitle.Text = Me.GetGlobalResourceObject("Text", "UpdatePCD")
            Case HCSP.ucTypeOfPracticeGrid.EnumMode.View
                ' Do Nothing
        End Select

    End Sub

    Private Sub SetupButton()
        lblDescForJoin.Visible = False
        lblDescForTransfer.Visible = False
        lblDescForTransferDetails.Visible = False
        chkDeclaration.Visible = False
        lblTermsAndCondition.Visible = False

        Select Case Me.Mode
            Case HCSP.ucTypeOfPracticeGrid.EnumMode.Create
                lblDescForJoin.Visible = True
                chkDeclaration.Visible = True
                lblTermsAndCondition.Visible = True
            Case HCSP.ucTypeOfPracticeGrid.EnumMode.Transfer
                lblDescForTransfer.Visible = True
                lblDescForTransferDetails.Visible = True
            Case HCSP.ucTypeOfPracticeGrid.EnumMode.View
                ' Do Nothing
        End Select

        ShowImageButton(Me.ibtnCreate, False)
        ShowImageButton(Me.ibtnCreateDisable, False)
        Me.ibtnTransfer.Visible = False

        Dim blnEnabled As Boolean = chkDeclaration.Checked

        Select Case Me.Mode
            Case HCSP.ucTypeOfPracticeGrid.EnumMode.View
            Case HCSP.ucTypeOfPracticeGrid.EnumMode.Create
                ShowImageButton(Me.ibtnCreate, blnEnabled)
                ShowImageButton(Me.ibtnCreateDisable, Not blnEnabled)
            Case HCSP.ucTypeOfPracticeGrid.EnumMode.Transfer
                Me.ibtnTransfer.Visible = True
        End Select
    End Sub

    Public Sub ShowImageButton(ByRef btnName As ImageButton, ByVal btnShow As Boolean)
        If btnShow Then
            btnName.Style.Item("display") = "block"
        Else
            btnName.Style.Item("display") = "none"
        End If
    End Sub

    Private Sub ucNoticePopUp_ButtonClick(ByVal e As ucNoticePopUp.enumButtonClick) Handles ucNoticePopUp.ButtonClick
        Select Case e
            Case HCSP.ucNoticePopUp.enumButtonClick.Cancel
                Me.Showing = False
            Case HCSP.ucNoticePopUp.enumButtonClick.OK
                'TODO: Redirect to PCD
        End Select
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udcGeneralFun As New Common.ComFunction.GeneralFunction
        Dim strURL As String = String.Empty
        If LCase(Session("language")) = "zh-tw" Then
            udcGeneralFun.getSystemParameter("PCDTermsAndConditionsURL_CHI", strURL, String.Empty)
        Else
            udcGeneralFun.getSystemParameter("PCDTermsAndConditionsURL_ENG", strURL, String.Empty)
        End If
        lblTermsAndCondition.OnClientClick = "window.open('" + strURL + "');return false;"

        chkDeclaration.Attributes.Add("onclick", "javascript:chkDeclarationCheckChanged(this, '" & Me.ibtnCreate.ClientID & "', '" & Me.ibtnCreateDisable.ClientID & "')")

        Setup()

    End Sub
End Class