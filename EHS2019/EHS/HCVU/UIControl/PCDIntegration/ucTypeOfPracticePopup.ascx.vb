Imports Common.Component
Imports Common.ComObject
Imports Common.Component.Address
Imports Common.Component.ServiceProvider
Imports Common.Component.Profession
Imports Common.Component.UserAC

Partial Public Class ucTypeOfPracticePopup
    Inherits System.Web.UI.UserControl

    Public Enum enumButtonClick
        Cancel
        CreatePCDAccount
        ERN
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

    Private _objJoinPCDResult As Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult = Nothing
    Public ReadOnly Property JoinPCDResult() As Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult
        Get
            Return _objJoinPCDResult
        End Get
    End Property

    Private _objExistPCDResult As Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult = Nothing
    Public ReadOnly Property ExistPCDResult() As Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult
        Get
            Return _objExistPCDResult
        End Get
    End Property

    Public Sub Reset()
        SetupButton()
        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Session(SESS_PCDPractice) = Nothing
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
    End Sub

#Region "Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Setup()

        Me.ibtnCancel.Attributes.Add("onclick", "if (typeof(winRef) != 'undefined') {winRef.close();}")
    End Sub

    Private Sub ibtnCancel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCancel.Click
        Me._udtAuditLog.WriteLog(AuditLogDesc.CancelButtonClick_ID, AuditLogDesc.CancelButtonClick)

        udcTMessageBox.Visible = False
        RaiseEvent ButtonClick(enumButtonClick.Cancel)
    End Sub

    Private Sub ibtnJoinPCD_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnJoinPCD.Click
        Me._udtAuditLog.WriteLog(AuditLogDesc.JoinPCDButtonClick_ID, AuditLogDesc.JoinPCDButtonClick)

        Dim udtSPBLL As New ServiceProviderBLL

        If PassedTypeOfPracticeValidation() Then
            ' Invoke PCD web service to create account
            Me.InvokePCD_JoinPCD()

            ClearThirdPartyEnrolmentList()

            ' Delete enrollment original record once PCD account successfully created
            If Me.JoinPCDResult.ReturnCode = Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult.enumReturnCode.UploadedSuccessfully Then
                udtSPBLL.DeleteServiceProviderOriginalProfile(GetSP().EnrolRefNo, Nothing)
            End If

            RaiseEvent ButtonClick(enumButtonClick.CreatePCDAccount)
        End If
    End Sub

    'Private Sub ibtnOriginalCopy_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnOriginalCopy.Click
    '    RaiseEvent ButtonClick(enumButtonClick.OriginalCopy)
    'End Sub

#End Region

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

    Private Sub InvokePCD_JoinPCD()
        Dim udtSPBLL As New ServiceProviderBLL
        Dim objPCDWS As New Common.PCD.PCDWebService(CType(Me.Page, BasePage).FunctionCode)
        Dim objResult As Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult = Nothing
        Dim udtSP As ServiceProviderModel = GetSP()

        udtSP.ThirdPartyAdditionalFieldEnrolmentList = Me.ucTypeOfPracticeGrid.GetThirdPartyAdditionalFieldEnrolmentCollection()

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtSP.PracticeList = CType(Session(SESS_PCDPractice), Practice.PracticeModelCollection)
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

        Me._udtAuditLog.AddDescripton("WebMethod", "UploadVerifiedEnrolment")
        Me._udtAuditLog.WriteStartLog(AuditLogDesc.InvokePCDStart_ID, AuditLogDesc.InvokePCDStart)

        _objJoinPCDResult = objPCDWS.PCDUploadVerifiedEnrolment(udtSP)

        Me._udtAuditLog.AddDescripton("WebMethod", "UploadVerifiedEnrolment")
        Me._udtAuditLog.AddDescripton("ReturnCode", _objJoinPCDResult.ReturnCode)
        Me._udtAuditLog.AddDescripton("MessageID", _objJoinPCDResult.MessageID)
        Me._udtAuditLog.WriteEndLog(AuditLogDesc.InvokePCDEnd_ID, AuditLogDesc.InvokePCDEnd)
    End Sub

    Public Sub InvokePCD_CheckExist()
        Dim udtSPBLL As New ServiceProviderBLL
        Dim objPCDWS As New Common.PCD.PCDWebService(CType(Me.Page, BasePage).FunctionCode)
        Dim objResult As Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult = Nothing
        Dim udtSP As ServiceProviderModel = GetSP()

        Me._udtAuditLog.AddDescripton("WebMethod", "CheckAvailableForVerifiedEnrolment")
        Me._udtAuditLog.WriteStartLog(AuditLogDesc.InvokePCDStart_ID, AuditLogDesc.InvokePCDStart)

        _objExistPCDResult = objPCDWS.PCDCheckAvailableForVerifiedEnrolment(udtSP)

        Me._udtAuditLog.AddDescripton("WebMethod", "CheckAvailableForVerifiedEnrolment")
        Me._udtAuditLog.AddDescripton("ReturnCode", _objExistPCDResult.ReturnCode)
        Me._udtAuditLog.AddDescripton("MessageID", _objExistPCDResult.MessageID)
        Me._udtAuditLog.WriteEndLog(AuditLogDesc.InvokePCDEnd_ID, AuditLogDesc.InvokePCDEnd)
    End Sub

    Private Sub ClearThirdPartyEnrolmentList()
        Dim udtSP As ServiceProviderModel = GetSP()
        udtSP.ThirdPartyAdditionalFieldEnrolmentList.Clear()
    End Sub

    Public Sub Setup()
        ' Init audit log object
        Me._udtAuditLog = New AuditLogEntry(CType(Me.Page, BasePage).FunctionCode, Me.Page)
    End Sub

    ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Public Sub LoadPractice()
    Public Sub LoadPractice(ByVal udtPracticeList As Practice.PracticeModelCollection)
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

        Dim udtSP As ServiceProviderModel = GetSP()
        LoadServiceProvider(udtSP)

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Me.ucTypeOfPracticeGrid.LoadPractice(udtSP)
        Session(SESS_PCDPractice) = udtPracticeList
        Me.ucTypeOfPracticeGrid.LoadPractice(udtSP, udtPracticeList)
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
    End Sub

    Public Sub LoadServiceProvider(ByVal udtSP As ServiceProviderModel)
        Dim udtSPBLL As New Common.Component.ServiceProvider.ServiceProviderBLL
        Dim udtFormatter As New Common.Format.Formatter
        Dim strHost As String = Request.Url.ToString.Substring(0, Request.Url.ToString.IndexOf(Request.Path))
        Dim strOpenOriginalCopyLink As String = strHost & Request.ApplicationPath

        strOpenOriginalCopyLink = "spEnrolmentCopy.aspx"

        Me.lblERN.Text = String.Empty
        Me.lnkBtnERN.Text = String.Empty
        If udtSPBLL.CheckServiceProviderOriginalProfileExistByERN(udtSP.EnrolRefNo) Then
            Me.lnkBtnERN.Text = udtFormatter.formatSystemNumber(udtSP.EnrolRefNo)
        Else
            Me.lblERN.Text = udtFormatter.formatSystemNumber(udtSP.EnrolRefNo)
        End If

        Me.lblSPEName.Text = udtSP.EnglishName.Trim
        If udtSP.ChineseName.Trim = String.Empty Then
            Me.lblSPCName.Text = String.Empty
        Else
            Me.lblSPCName.Text = " (" + udtSP.ChineseName.Trim + ")"
        End If



        Me.lblHKIC.Text = udtFormatter.formatHKID(udtSP.HKID, False)
        Me.lblSPAddress.Text = FormatAddress(udtSP.SpAddress)
        Me.lblSPEmail.Text = udtSP.Email

    End Sub

#Region "Formatting functions"
    Protected Function FormatAddress(ByVal udtAddressModel As AddressModel) As String
        Return (New Common.Format.Formatter).formatAddress(udtAddressModel.Room, udtAddressModel.Floor, udtAddressModel.Block, _
                                                           udtAddressModel.Building, udtAddressModel.District, udtAddressModel.AreaCode)
    End Function
#End Region

    Private Function GetSP() As ServiceProviderModel
        Dim bll As New ServiceProviderBLL
        Return bll.GetSP()
    End Function

    Private Sub SetupButton()
        Me.ibtnJoinPCD.Visible = False

        Select Case Me.Mode
            Case HCVU.ucTypeOfPracticeGrid.EnumMode.View
            Case HCVU.ucTypeOfPracticeGrid.EnumMode.Create
            Case HCVU.ucTypeOfPracticeGrid.EnumMode.Transfer
                Me.ibtnJoinPCD.Visible = True
        End Select

    End Sub

    Private Sub lnkBtnERN_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkBtnERN.Click
        RaiseEvent ButtonClick(enumButtonClick.ERN)
    End Sub
End Class