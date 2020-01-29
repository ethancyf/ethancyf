Imports System.Web.Security.AntiXss
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.Address
Imports Common.Component.BankAcct
Imports Common.Component.HCVUUser
Imports Common.Component.MedicalOrganization
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Professional
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Format
Imports Common.Validation
Imports Common.PCD
Imports Common.PCD.WebService.Interface

Partial Public Class spProfile
    Inherits BasePageWithGridView

    Private Validator As New Validator
    Private udtFormatter As New Formatter
    Private SM As SystemMessage

    Private udtServiceProviderBLL As New ServiceProviderBLL
    Private udtPracticeBLL As New PracticeBLL
    Private udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
    Private udtProfessionalBLL As New ProfessionalBLL
    Private udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
    Private udtSPProfileBLL As New SPProfileBLL

    Private blnAbleToFind As Boolean = False
    Private blnExisingHKID As Boolean = False
    Private strProgress As String = String.Empty

    Private intResidentialAddressFrom As Integer
    Private intResidentialAddressTo As Integer
    Private intBusinessAddressFrom As Integer
    Private intBusinessAddressTo As Integer

    Private udtAuditLogEntry As AuditLogEntry
    Private strFuncCode As String = FunctCode.FUNT010101

    Private Const SESS_Action As String = "EnrolAction"
    Private Const SESS_ERN As String = "Enrolment_Ref_No"
    Private Const SESS_TableLocation As String = "TableLocation"
    Private Const SESS_CheckedScheme As String = "SPDE_CheckedScheme"

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Const SESS_PCDCheckAccountStatusResult As String = "010101_PCDCheckAccountStatusResult"
    Private Const SESS_PCDProfessionalChecked As String = "010101_PCDProfessional_Checked"
    Private Const SESS_PCDStatusChecked As String = "010101_PCDStatus_Checked"
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

#Region "Change Indicator For UI"

    ' To handle subsquence update change indicator
    ' ServiceProviderPermanent: Original Service Provider Record
    ' udtServiceProviderBLL.GetSP(): Newly Modified Service Provider Record

    Public Class ServiceProviderComparator

        Public Const EnglishName As String = "EnglishName"
        Public Const ChineseName As String = "ChineseName"
        Public Const SpAddress As String = "SpAddress"
        Public Const Email As String = "Email"
        Public Const Phone As String = "Phone"
        Public Const Fax As String = "Fax"
        Public Const Address As String = "Address"
        Public Const MedicalOrganization As String = "MedicalOrganization"
        Public Const NameOfPractice As String = "NameOfPractice"

        Public Const strNew As String = "New"
        Public Const strUnderAmendment As String = "Under Amendment"

        Private udtSPOriginal As ServiceProviderModel = Nothing

        Sub New(ByVal udtServiceProviderOriginal As ServiceProviderModel)
            Me.udtSPOriginal = udtServiceProviderOriginal
        End Sub

        Public Function IsServiceProviderChanged(ByVal strField As String, ByRef udtSPNew As ServiceProviderModel) As Boolean
            If udtSPNew Is Nothing Then Throw New ArgumentNullException("Service Provider Permanent Record Not Exist")
            If udtSPOriginal Is Nothing Then Throw New ArgumentNullException("Service Provider Staging Record Not Exist")

            Select Case strField
                Case EnglishName
                    Return udtSPNew.EnglishName.Trim <> udtSPOriginal.EnglishName.Trim
                Case ChineseName
                    Return udtSPNew.ChineseName.Trim <> udtSPOriginal.ChineseName.Trim
                Case SpAddress
                    Return Not udtSPNew.SpAddress.Equals(udtSPOriginal.SpAddress)
                Case Email
                    Return udtSPNew.Email <> udtSPOriginal.Email
                Case Phone
                    Return udtSPNew.Phone <> udtSPOriginal.Phone
                Case Fax
                    Return udtSPNew.Fax <> udtSPOriginal.Fax
                Case Else
                    Throw New ArgumentException(strField + " is an Invalid Parameter")
            End Select

        End Function

        Public Function GetMOChangedField(ByVal intSeq As Integer, ByVal udtSPNew As ServiceProviderModel) As Dictionary(Of String, Boolean)
            If udtSPNew Is Nothing Then Throw New ArgumentNullException("Service Provider Permanent Record Not Exist")
            If udtSPOriginal Is Nothing Then Throw New ArgumentNullException("Service Provider Staging Record Not Exist")

            Dim udtMONew As MedicalOrganizationModel = udtSPNew.MOList(intSeq)
            If IsNothing(udtMONew) Then Return Nothing

            If udtMONew.RecordStatus = MedicalOrganizationStagingStatus.Active Then Return Nothing

            Dim udtMOOrig As MedicalOrganizationModel = udtSPOriginal.MOList(intSeq)

            Dim dicMOChanged As New Dictionary(Of String, Boolean)

            dicMOChanged.Add(Phone, IIf(udtMONew.PhoneDaytime = udtMOOrig.PhoneDaytime, False, True))
            dicMOChanged.Add(Email, IIf(udtMONew.Email = udtMOOrig.Email, False, True))
            dicMOChanged.Add(Fax, IIf(udtMONew.Fax = udtMOOrig.Fax, False, True))
            dicMOChanged.Add(Address, IIf(udtMONew.MOAddress.Equals(udtMOOrig.MOAddress), False, True))

            Return dicMOChanged

        End Function

        Public Function IsPracticeSchemeInfoChanged(ByVal intSeq As Integer, ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, ByVal udtSPNew As ServiceProviderModel) As Boolean
            If udtSPNew Is Nothing Then Throw New ArgumentNullException("Service Provider Permanent Record Not Exist")
            If udtSPOriginal Is Nothing Then Throw New ArgumentNullException("Service Provider Staging Record Not Exist")

            Dim intServiceFeeNew As Nullable(Of Integer) = Nothing
            Dim intServiceFeeOrig As Nullable(Of Integer) = Nothing

            Dim udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
            Dim udtSchemeBOList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup()

            Dim intSchemeDisplaySeq As Integer = udtSchemeBOList.Filter(strSchemeCode).DisplaySeq
            Dim intSubsidizeDisplaySeq As Integer = udtSchemeBOList.Filter(strSchemeCode).SubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode).DisplaySeq

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            ' Add SchemeCode to key
            Dim udtPracticeSchemeNew As PracticeSchemeInfoModel = udtSPNew.PracticeList(intSeq).PracticeSchemeInfoList(intSeq, strSubsidizeCode, intSchemeDisplaySeq, intSubsidizeDisplaySeq, strSchemeCode)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            If Not IsNothing(udtPracticeSchemeNew) Then
                intServiceFeeNew = udtPracticeSchemeNew.ServiceFee
            End If

            If Not IsNothing(udtSPOriginal.PracticeList(intSeq)) Then
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                ' Add SchemeCode to key
                Dim udtPracticeSchemeOriginal As PracticeSchemeInfoModel = udtSPOriginal.PracticeList(intSeq).PracticeSchemeInfoList(intSeq, strSubsidizeCode, intSchemeDisplaySeq, intSubsidizeDisplaySeq, strSchemeCode)
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                If Not IsNothing(udtPracticeSchemeOriginal) Then
                    intServiceFeeOrig = udtPracticeSchemeOriginal.ServiceFee
                End If
            End If

            ' Compare the Service Fee
            If IsNothing(intServiceFeeNew) AndAlso IsNothing(intServiceFeeOrig) Then Return False

            If IsNothing(intServiceFeeNew) AndAlso Not IsNothing(intServiceFeeOrig) _
                OrElse Not IsNothing(intServiceFeeNew) AndAlso IsNothing(intServiceFeeOrig) Then Return True

            If CInt(intServiceFeeNew) <> CInt(intServiceFeeOrig) Then Return True

            Return False

        End Function

        Public Function GetPracticeChangedField(ByVal intSeq As Integer, ByVal udtSPNew As ServiceProviderModel) As Dictionary(Of String, Boolean)
            If udtSPNew Is Nothing Then Throw New ArgumentNullException("Service Provider Permanent Record Not Exist")
            If udtSPOriginal Is Nothing Then Throw New ArgumentNullException("Service Provider Staging Record Not Exist")

            Dim udtPracticeNew As PracticeModel = udtSPNew.PracticeList(intSeq)
            If IsNothing(udtPracticeNew) Then Return Nothing

            If udtPracticeNew.RecordStatus = PracticeStagingStatus.Active Then Return Nothing

            Dim udtPracticeOrig As PracticeModel = udtSPOriginal.PracticeList(intSeq)

            Dim dicPracticeChanged As New Dictionary(Of String, Boolean)

            dicPracticeChanged.Add(MedicalOrganization, IIf(udtPracticeNew.MODisplaySeq = udtPracticeOrig.MODisplaySeq, False, True))
            dicPracticeChanged.Add(NameOfPractice, IIf(udtPracticeNew.PracticeNameChi = udtPracticeOrig.PracticeNameChi, False, True))
            dicPracticeChanged.Add(Address, IIf(udtPracticeNew.PracticeAddress.Equals(udtPracticeOrig.PracticeAddress), False, True))
            dicPracticeChanged.Add(Phone, IIf(udtPracticeNew.PhoneDaytime = udtPracticeOrig.PhoneDaytime, False, True))

            Return dicPracticeChanged

        End Function

        ''' <summary>
        ''' Compare The Staging Practice Record is updated Or not
        ''' </summary>
        ''' <param name="intDisplaySeq"></param>
        ''' <param name="udtServiceProviderNew"></param>
        ''' <returns>True: Updated / Newly Add</returns>
        ''' <remarks></remarks>
        Public Function IsPracticeChanged(ByVal intDisplaySeq As Integer, ByRef udtServiceProviderNew As ServiceProviderModel) As Boolean
            ' Return True if Staging RecordStatus = New Add / Update
            If udtServiceProviderNew Is Nothing Then Throw New ArgumentNullException("Service Provider Permanent Record Not Exist")
            If Me.udtSPOriginal Is Nothing Then Throw New ArgumentNullException("Service Provider Staging Record Not Exist")

            Dim udtPracticeOriginal As PracticeModel = Nothing

            If Not udtServiceProviderNew.PracticeList Is Nothing AndAlso udtServiceProviderNew.PracticeList.Count > 0 Then
                For Each udtTempPractice As PracticeModel In udtServiceProviderNew.PracticeList.Values
                    If udtTempPractice.DisplaySeq = intDisplaySeq Then
                        udtPracticeOriginal = udtTempPractice
                        Exit For
                    End If
                Next
            End If

            If udtPracticeOriginal.RecordStatus = Common.Component.PracticeStagingStatus.Active OrElse udtPracticeOriginal.RecordStatus = Common.Component.PracticeStagingStatus.Update Then
                Return True
            Else
                Return False
            End If
        End Function

    End Class

    Private m_udtSPPermanent As ServiceProviderModel = Nothing
    Private ReadOnly Property ServiceProviderPermanent() As ServiceProviderModel
        Get
            If Me.m_udtSPPermanent Is Nothing Then
                Me.m_udtSPPermanent = Me.udtSPProfileBLL.GetServiceProviderPermanentProfileNoSession(Me.hfERN.Value.Trim())
            End If
            Return Me.m_udtSPPermanent
        End Get
    End Property

    Private Sub ApplyBankAccountChangeIndicator()

        ' Field Change Indicator:
        ' Only For Subsuqence Update, To compare Staging & Permanent
        Dim udtSPModel As ServiceProviderModel = Me.udtServiceProviderBLL.GetSP()

        ' Return For New Enrolment
        If udtSPModel.SPID Is Nothing OrElse udtSPModel.SPID.Trim() = "" Then
            Return
        End If

        ' Return For Load From Permanent
        If Me.hfTableLocation.Value.Trim() = TableLocation.Permanent Then
            Return
        End If

        Dim udtComparator As New ServiceProviderComparator(Me.ServiceProviderPermanent())

        For Each udtPractice As PracticeModel In udtSPModel.PracticeList.Values
            If udtComparator.IsPracticeChanged(udtPractice.DisplaySeq, udtSPModel) Then
                Me.EnableBankChangeIndicator(True, udtPractice.DisplaySeq)
            Else
                Me.EnableBankChangeIndicator(False, udtPractice.DisplaySeq)
            End If
        Next
    End Sub

    Private Sub ApplyPracticeChangeIndicator(ByVal r As GridViewRow, Optional ByVal blnEditMode As Boolean = False)
        Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP()

        ' Return for New Enrolment
        If udtSP.SPID Is Nothing OrElse udtSP.SPID.Trim = String.Empty Then Return

        ' Return for Load from Permanent
        If hfTableLocation.Value.Trim = TableLocation.Permanent Then Return

        If Not blnEditMode Then
            ' If the Status is New, just turn the status to red
            If CType(r.FindControl("lblPracticeStatus"), Label).Text.Trim = ServiceProviderComparator.strNew Then
                CType(r.FindControl("lblPracticeStatusInd"), Label).Visible = True
                CType(r.FindControl("lblPracticeStatus"), Label).ForeColor = Drawing.Color.Red
                Return

            End If

            ' If the Status is Under Amendment, turn the status to red and continue
            If CType(r.FindControl("lblPracticeStatus"), Label).Text.Trim = ServiceProviderComparator.strUnderAmendment Then
                CType(r.FindControl("lblPracticeStatusInd"), Label).Visible = True
                CType(r.FindControl("lblPracticeStatus"), Label).ForeColor = Drawing.Color.Red

            End If

        End If

        Dim udtComparator As New ServiceProviderComparator(ServiceProviderPermanent)

        Dim intSeq As Integer = CInt(CType(r.FindControl(IIf(blnEditMode, "lblEditPracticeDispalySeq", "lblPracticeDispalySeq")), Label).Text)

        Dim dicPracticeChanged As Dictionary(Of String, Boolean) = udtComparator.GetPracticeChangedField(intSeq, udtServiceProviderBLL.GetSP())

        If IsNothing(dicPracticeChanged) Then Return

        If dicPracticeChanged(ServiceProviderComparator.MedicalOrganization) Then CType(r.FindControl("lblPracticeMONameTextInd"), Label).Visible = True
        If dicPracticeChanged(ServiceProviderComparator.NameOfPractice) Then CType(r.FindControl("lblPracticeNameTextInd"), Label).Visible = True
        If dicPracticeChanged(ServiceProviderComparator.Address) Then CType(r.FindControl("lblPracticeAddressInd"), Label).Visible = True
        If dicPracticeChanged(ServiceProviderComparator.Phone) Then CType(r.FindControl("lblRegPhoneInd"), Label).Visible = True

    End Sub

    Private Sub ApplyPersonalChangeIndicator()
        ' Field Change Indicator:
        ' Only For Subsuqence Update, To compare Staging & Permanent

        Dim udtSPModel As ServiceProviderModel = Me.udtServiceProviderBLL.GetSP()

        ' Return For New Enrolment
        If udtSPModel.SPID Is Nothing OrElse udtSPModel.SPID.Trim() = "" Then
            Return
        End If

        ' Return For Load From Permanent
        If Me.hfTableLocation.Value.Trim() = TableLocation.Permanent Then
            Return
        End If

        Dim udtComparator As New ServiceProviderComparator(Me.ServiceProviderPermanent())

        If udtComparator.IsServiceProviderChanged(ServiceProviderComparator.EnglishName, udtSPModel) Then
            Me.EnableServiceProviderChangeIndicator(True, ServiceProviderComparator.EnglishName)
        Else
            Me.EnableServiceProviderChangeIndicator(False, ServiceProviderComparator.EnglishName)
        End If

        If udtComparator.IsServiceProviderChanged(ServiceProviderComparator.ChineseName, udtSPModel) Then
            Me.EnableServiceProviderChangeIndicator(True, ServiceProviderComparator.ChineseName)
        Else
            Me.EnableServiceProviderChangeIndicator(False, ServiceProviderComparator.ChineseName)
        End If

        If udtComparator.IsServiceProviderChanged(ServiceProviderComparator.SpAddress, udtSPModel) Then
            Me.EnableServiceProviderChangeIndicator(True, ServiceProviderComparator.SpAddress)
        Else
            Me.EnableServiceProviderChangeIndicator(False, ServiceProviderComparator.SpAddress)
        End If

        If udtComparator.IsServiceProviderChanged(ServiceProviderComparator.Email, udtSPModel) Then
            Me.EnableServiceProviderChangeIndicator(True, ServiceProviderComparator.Email)
        Else
            Me.EnableServiceProviderChangeIndicator(False, ServiceProviderComparator.Email)
        End If

        If udtComparator.IsServiceProviderChanged(ServiceProviderComparator.Phone, udtSPModel) Then
            Me.EnableServiceProviderChangeIndicator(True, ServiceProviderComparator.Phone)
        Else
            Me.EnableServiceProviderChangeIndicator(False, ServiceProviderComparator.Phone)
        End If

        If udtComparator.IsServiceProviderChanged(ServiceProviderComparator.Fax, udtSPModel) Then
            Me.EnableServiceProviderChangeIndicator(True, ServiceProviderComparator.Fax)
        Else
            Me.EnableServiceProviderChangeIndicator(False, ServiceProviderComparator.Fax)
        End If

    End Sub

    Private Sub ApplyMOChangeIndicator(ByVal r As GridViewRow, Optional ByVal blnEditMode As Boolean = False)
        Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP()

        ' Return for New Enrolment
        If udtSP.SPID Is Nothing OrElse udtSP.SPID.Trim = String.Empty Then Return

        ' Return for Load from Permanent
        If hfTableLocation.Value.Trim = TableLocation.Permanent Then Return

        If Not blnEditMode Then
            ' If the Status is New, just turn the status to red
            If CType(r.FindControl("lblMOStatus"), Label).Text.Trim = ServiceProviderComparator.strNew Then
                CType(r.FindControl("lblMOStatusInd"), Label).Visible = True
                CType(r.FindControl("lblMOStatus"), Label).ForeColor = Drawing.Color.Red
                Return

            End If

            ' If the Status is Under Amendment, turn the status to red and continue
            If CType(r.FindControl("lblMOStatus"), Label).Text.Trim = ServiceProviderComparator.strUnderAmendment Then
                CType(r.FindControl("lblMOStatusInd"), Label).Visible = True
                CType(r.FindControl("lblMOStatus"), Label).ForeColor = Drawing.Color.Red

            End If
        End If

        Dim udtComparator As New ServiceProviderComparator(ServiceProviderPermanent)

        Dim intSeq As Integer = CInt(CType(r.FindControl(IIf(blnEditMode, "lblEditMODispalySeq", "lblMODispalySeq")), Label).Text)

        Dim dicMOChanged As Dictionary(Of String, Boolean) = udtComparator.GetMOChangedField(intSeq, udtServiceProviderBLL.GetSP())

        If IsNothing(dicMOChanged) Then Return

        If dicMOChanged(ServiceProviderComparator.Email) Then CType(r.FindControl("lblRegMOEmailInd"), Label).Visible = True
        If dicMOChanged(ServiceProviderComparator.Phone) Then CType(r.FindControl("lblRegMOPhoneInd"), Label).Visible = True
        If dicMOChanged(ServiceProviderComparator.Fax) Then CType(r.FindControl("lblRegMOFaxInd"), Label).Visible = True
        If dicMOChanged(ServiceProviderComparator.Address) Then CType(r.FindControl("lblMOAddressInd"), Label).Visible = True

    End Sub



    Private Sub EnableServiceProviderChangeIndicator(ByVal blnEnable As Boolean, ByVal strField As String)

        ' User Find Control to find the Control under The FormView
        Dim control As Control = Me.fvPersonalParticulars.FindControl("lbl" + strField + "Ind")
        If Not control Is Nothing Then
            Dim lblIndicator As Label = CType(control, Label)
            'lblIndicator.Font.Bold = True
            'lblIndicator.CssClass = "tableTitle"
            'lblIndicator.Font.Size = New FontUnit(12)
            lblIndicator.Visible = blnEnable
        End If
    End Sub

    'Private Sub EnablePracticeChangeIndicator(ByVal blnEnable As Boolean, ByVal intSeq As Integer)

    '    For Each gvRow As GridViewRow In Me.gvPracticeInfo.Rows
    '        If gvRow.RowType = DataControlRowType.DataRow Then
    '            Dim control As Control = gvRow.FindControl("lblPracticeDispalySeq")
    '            If Not control Is Nothing Then
    '                Dim lblFind As Label = CType(control, Label)
    '                If lblFind.Text = intSeq.ToString() Then

    '                    ' Indicator
    '                    control = gvRow.FindControl("lblPracticeStatusInd")
    '                    Dim lblIndicator As Label = CType(control, Label)

    '                    lblIndicator.CssClass = "tableTitle"
    '                    lblIndicator.Font.Size = New FontUnit(12)
    '                    lblIndicator.Visible = blnEnable

    '                    ' Status Text
    '                    control = gvRow.FindControl("lblPracticeStatus")
    '                    Dim lblStatus As Label = CType(control, Label)

    '                    If blnEnable Then
    '                        lblStatus.ForeColor = Drawing.Color.Red
    '                    Else
    '                        lblStatus.ForeColor = Drawing.Color.Empty
    '                    End If

    '                    Exit For

    '                End If
    '            End If
    '        End If
    '    Next

    'End Sub

    Private Sub EnableBankChangeIndicator(ByVal blnEnable As Boolean, ByVal intPracticeSeq As Integer)
        For Each gvRow As GridViewRow In Me.gvBankInfo.Rows
            Dim control As Control = Nothing

            If Me.gvBankInfo.EditIndex = gvRow.RowIndex Then
                'If gvRow.RowState = DataControlRowState.Edit Then
                control = gvRow.FindControl("lblEditPracticeBankDispalySeq")
            Else
                control = gvRow.FindControl("lblPracticeBankDispalySeq")
            End If

            If Not control Is Nothing Then
                Dim lblFind As Label = CType(control, Label)
                If lblFind.Text = intPracticeSeq.ToString() Then

                    ' Indicator
                    control = gvRow.FindControl("lblBankStatusInd")
                    Dim lblIndicator As Label = CType(control, Label)

                    'lblIndicator.CssClass = "tableTitle"
                    'lblIndicator.Font.Size = New FontUnit(12)
                    lblIndicator.Visible = blnEnable

                    ' Status Text
                    'lblEditBankPracticeStatus

                    If Me.gvBankInfo.EditIndex = gvRow.RowIndex Then
                        control = gvRow.FindControl("lblEditBankPracticeStatus")
                    Else
                        control = gvRow.FindControl("lblBankPracticeStatus")
                    End If

                    Dim lblStatus As Label = CType(control, Label)

                    If blnEnable Then
                        lblStatus.ForeColor = Drawing.Color.Red
                    Else
                        lblStatus.ForeColor = Drawing.Color.Empty
                    End If

                    Exit For

                End If
            End If
        Next
    End Sub

#End Region

#Region "Page Events"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.FunctionCode = strFuncCode
        udtAuditLogEntry = New AuditLogEntry(Me.FunctionCode, Me)

        Response.Expires = -1
        Response.CacheControl = "no-cache"
        Response.AddHeader("Pragma", "no-cache")

        If Not IsPostBack Then
            udtSPProfileBLL.ClearSession()
            If Not IsNothing(Session(SESS_Action)) Then
                Dim strAction As String = Session(SESS_Action)
                Select Case strAction
                    Case "New"
                        ' Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                        udtAuditLogEntry.WriteLog(Common.Component.LogID.LOG00007, "Start New enrolment")

                        ibtnExistingSPProfile.Visible = False
                        fvPersonalParticulars.ChangeMode(FormViewMode.Insert)
                        hfERN.Value = String.Empty
                        hfTableLocation.Value = String.Empty
                        panIDInfo.Visible = False
                        Me.tablMOInfo.Visible = False
                        Me.tabPracticeInfo.Visible = False
                        Me.tabBankAcctInfo.Visible = False
                        Me.tablSchemeInfo.Visible = False

                    Case "Search"
                        If Not IsNothing(Session(SESS_ERN)) AndAlso Not IsNothing(Session(SESS_TableLocation)) Then
                            hfERN.Value = Session(SESS_ERN)
                            hfTableLocation.Value = Session(SESS_TableLocation)
                            blnAbleToFind = udtSPProfileBLL.GetServiceProviderProfile(hfERN.Value.Trim, hfTableLocation.Value.Trim)

                            If blnAbleToFind Then
                                'Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                                udtAuditLogEntry.AddDescripton("ERN", hfERN.Value.ToString)
                                udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
                                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00005, "Select")

                                If Not blnAbleToFind And hfTableLocation.Value.Trim.Equals(TableLocation.Enrolment) Then
                                    CompleteMsgBox.AddMessage("990000", "I", "00015")
                                    CompleteMsgBox.BuildMessageBox()
                                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

                                    Me.MultiViewDataEntry.ActiveViewIndex = 1

                                    Exit Sub
                                End If

                                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00006, "Select Completed.")

                                If udtServiceProviderBLL.Exist Then
                                    If hfTableLocation.Value.Trim.Equals(TableLocation.Enrolment) Then
                                        blnExisingHKID = udtSPProfileBLL.IsHKIDExistingInServiceProviderStagingPermanentByERN(hfERN.Value.Trim)
                                    End If
                                    strProgress = udtSPProfileBLL.GetEnrolmentProcessStatus(hfERN.Value.Trim)
                                    If strProgress Is Nothing Then strProgress = String.Empty
                                End If

                                BindSPProfile()

                                panIDInfo.Visible = True

                            Else
                                'Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                                udtAuditLogEntry.AddDescripton("ERN", hfERN.Value.Trim)
                                udtAuditLogEntry.AddDescripton("TableLocation", hfTableLocation.Value.Trim)

                                msgBox.AddMessage(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00184))
                                msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00102, "Select failed")

                                Me.MultiViewDataEntry.ActiveViewIndex = 1
                            End If



                        Else
                            Response.Redirect("~/serviceprovider/spDataEntry.aspx")
                        End If

                    Case Else
                        Response.Redirect("~/serviceprovider/spDataEntry.aspx")
                End Select
            Else
                Response.Redirect("~/serviceprovider/spDataEntry.aspx")
            End If
            Session.Remove(SESS_Action)

            ' Handle double post-back
            Dim strBrowser As String = String.Empty
            Try
                If Not HttpContext.Current.Request.Browser Is Nothing Then
                    strBrowser = HttpContext.Current.Request.Browser.Type
                End If
            Catch ex As Exception
                strBrowser = String.Empty
            End Try

            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnDialogConfirm)
            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnDeletePracticeConfirm)
            MyBase.preventMultiImgClick(Me.Page.ClientScript, Me.ibtnDeleteMOConfirm)

        End If

        If udtServiceProviderBLL.Exist Then
            If hfTableLocation.Value.Trim.Equals(TableLocation.Enrolment) Then
                blnExisingHKID = udtSPProfileBLL.IsHKIDExistingInServiceProviderStagingPermanentByERN(udtServiceProviderBLL.GetSP.EnrolRefNo.Trim)
            End If

            strProgress = udtSPProfileBLL.GetEnrolmentProcessStatus(udtServiceProviderBLL.GetSP.EnrolRefNo.Trim)
            If strProgress Is Nothing Then strProgress = String.Empty

            panIDInfo.Visible = True

            If udtServiceProviderBLL.GetSP.SPID.Equals(String.Empty) Then
                lblIDText.Text = Me.GetGlobalResourceObject("Text", "EnrolRefNo")
                lblID.Text = udtFormatter.formatSystemNumber(udtServiceProviderBLL.GetSP.EnrolRefNo.Trim)

                lblERNText.Visible = False
                lblERN.Visible = False
                lblEnrolDtm.Text = udtFormatter.convertDateTime(udtServiceProviderBLL.GetSP.EnrolDate)
            Else
                lblIDText.Text = Me.GetGlobalResourceObject("Text", "SPID")
                lblID.Text = udtServiceProviderBLL.GetSP.SPID

                lblERNText.Visible = True
                lblERN.Visible = True
                lblERN.Text = udtFormatter.formatSystemNumber(udtServiceProviderBLL.GetSP.EnrolRefNo.Trim)

                lblEnrolDtm.Text = udtFormatter.convertDateTime(udtServiceProviderBLL.GetSP.EnrolDate)
            End If

            tablMOInfo.Visible = True

            If blnExisingHKID Then
                Me.gvMOInfo.DataSource = udtServiceProviderBLL.GetSP.MOList.Values
                gvMOInfo.EditIndex = -1
                gvMOInfo.DataBind()
            Else
                If IsNothing(udtServiceProviderBLL.GetSP.MOList) Then
                    If Not gvMOInfo.ShowFooter Then
                        Me.ibtnMOAdd_Click(Nothing, Nothing)
                    End If
                Else
                    If udtServiceProviderBLL.GetSP.MOList.Count = 0 Then
                        If Not gvMOInfo.ShowFooter Then
                            Me.ibtnMOAdd_Click(Nothing, Nothing)
                        End If
                    End If
                End If
            End If

            If blnExisingHKID Then
                If udtServiceProviderBLL.GetSP.PracticeList.Count = 0 Then
                    tabPracticeInfo.Visible = False
                    tabBankAcctInfo.Visible = False
                    tablSchemeInfo.Visible = False
                Else
                    Me.tabPracticeInfo.Visible = True
                    tabBankAcctInfo.Visible = True
                    tablSchemeInfo.Visible = True

                    Me.gvPracticeInfo.DataSource = udtServiceProviderBLL.GetSP.PracticeList.Values
                    gvPracticeInfo.EditIndex = -1
                    gvPracticeInfo.DataBind()

                    Me.gvBankInfo.DataSource = udtServiceProviderBLL.GetSP.PracticeList.Values
                    gvBankInfo.EditIndex = -1
                    gvBankInfo.DataBind()

                    gvSI.DataSource = udtServiceProviderBLL.GetSP.PracticeList.Values
                    gvSI.EditIndex = -1
                    gvSI.DataBind()

                End If
            Else

                If udtServiceProviderBLL.GetSP.PracticeList.Count = 0 Then
                    If udtServiceProviderBLL.GetSP.MOList.Count = 0 Then
                        tabPracticeInfo.Visible = False
                        tabBankAcctInfo.Visible = False
                        tablSchemeInfo.Visible = False
                    Else
                        Me.tabPracticeInfo.Visible = True
                        If Not gvPracticeInfo.ShowFooter Then
                            Me.ibtnPracticeAdd_Click(Nothing, Nothing)
                            Me.tabBankAcctInfo.Visible = False
                            Me.tablSchemeInfo.Visible = False
                        End If
                    End If

                Else
                    tabBankAcctInfo.Visible = True
                    Me.tablSchemeInfo.Visible = True
                End If

            End If

        End If

        'Add javascript to get the selected structure address
        Dim selectStructureAddressScript As New StringBuilder
        selectStructureAddressScript.Append("<Script language='JavaScript'>")
        selectStructureAddressScript.Append("var perviousObj = null;")
        selectStructureAddressScript.Append("function getSelectedAddressbyID(row, AddressID, AddressChiID, RecordID, DistrictCode, AreaCode) {")
        selectStructureAddressScript.Append("checkClicked(row);")
        selectStructureAddressScript.Append("var AddressEng = document.getElementById(AddressID).innerText;")
        selectStructureAddressScript.Append("var AddressChi;")
        selectStructureAddressScript.Append("if (document.getElementById(AddressChiID) != null){")
        selectStructureAddressScript.Append("AddressChi = document.getElementById(AddressChiID).innerText;}")
        selectStructureAddressScript.Append("else{AddressChi = '';}")
        selectStructureAddressScript.Append("document.getElementById('" + Me.hfSelectedAddressEng.ClientID + "').value = AddressEng;")
        selectStructureAddressScript.Append("document.getElementById('" + Me.hfSelectedAddressChi.ClientID + "').value = AddressChi;")
        selectStructureAddressScript.Append("document.getElementById('" + Me.hfSelectedAddressRecordID.ClientID + "').value = RecordID;")
        selectStructureAddressScript.Append("document.getElementById('" + Me.hfSelectedAddressDistrictCode.ClientID + "').value = DistrictCode;")
        selectStructureAddressScript.Append("document.getElementById('" + Me.hfSelectedAddressAreaCode.ClientID + "').value = AreaCode;")
        selectStructureAddressScript.Append("}")

        'Set the background when clicking the cell in the gridview
        selectStructureAddressScript.Append("function checkClicked(obj) {")
        selectStructureAddressScript.Append("obj.style.backgroundColor = '#ddeeff';")
        selectStructureAddressScript.Append("if (perviousObj != null){")
        selectStructureAddressScript.Append("if (obj == perviousObj){")
        selectStructureAddressScript.Append("obj.style.backgroundColor = '#ddeeff';} else {")
        selectStructureAddressScript.Append("perviousObj.style.backgroundColor = '';}}")
        selectStructureAddressScript.Append("perviousObj = obj;")
        selectStructureAddressScript.Append("}")

        'Set the douable Click evenet the cell in the gridview
        selectStructureAddressScript.Append("function doubleClickbyID(row, AddressID, AddressChiID, RecordID, DistrictCode, AreaCode) {")
        'selectStructureAddressScript.Append("var AddressEng = document.getElementById(AddressID).innerText;")
        selectStructureAddressScript.Append("getSelectedAddressbyID(row, AddressID, AddressChiID, RecordID, DistrictCode, AreaCode);")
        selectStructureAddressScript.Append("document.getElementById('" + Me.ibtnDialogSelect.ClientID + "').click();")
        selectStructureAddressScript.Append("}")

        selectStructureAddressScript.Append("</Script>")
        ClientScript.RegisterStartupScript(Me.GetType(), "SelectedAddressScript", selectStructureAddressScript.ToString())

        'Add javascript to get the index from gridview where combobox district or rbo area is changed
        Dim getIndexScript As New StringBuilder
        getIndexScript.Append("<Script language='JavaScript'>")
        getIndexScript.Append("function getGridviewIndex(index) {")
        getIndexScript.Append("document.getElementById('" + Me.hfGridviewIndex.ClientID + "').value = index;")
        getIndexScript.Append("}")
        getIndexScript.Append("</Script>")
        ClientScript.RegisterStartupScript(Me.GetType(), "GetIndexScript", getIndexScript.ToString())

        SM = New Common.ComObject.SystemMessage("990000", "Q", "00001")

        Dim strRejectMessage As String
        strRejectMessage = SM.GetMessage
        Me.lblMsg.Text = strRejectMessage

        intResidentialAddressFrom = udtSPProfileBLL.GetResidentialAddressFrom
        intResidentialAddressTo = udtSPProfileBLL.GetResidentialAddressTo
        intBusinessAddressFrom = udtSPProfileBLL.GetBusinessAddressFrom
        intBusinessAddressTo = udtSPProfileBLL.GetBusinessAddressTo

    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Me.MultiViewDataEntry.ActiveViewIndex = 1 Then
            If blnAbleToFind Then
                Me.msgBox.Visible = False
            End If
        Else
            SetActionImg()

            If Me.tablMOInfo.Visible Then
                lblMOInfo.Visible = True
            Else
                lblMOInfo.Visible = False
                imgMOInfo.Visible = False
            End If

            If Me.tabPracticeInfo.Visible Then
                lblPracticeInfo.Visible = True
            Else
                lblPracticeInfo.Visible = False
                imgPracticeInfo.Visible = False
            End If

            If Me.tabBankAcctInfo.Visible Then
                lblBankInfo.Visible = True
            Else
                lblBankInfo.Visible = False
                imgBankInfo.Visible = False
            End If

            If Me.tablSchemeInfo.Visible Then
                lblSchemeInfo.Visible = True
            Else
                lblSchemeInfo.Visible = False
                imgSchemeInfo.Visible = False
            End If

            If udtServiceProviderBLL.Exist Then
                Dim udtSP As ServiceProviderModel

                udtSP = udtServiceProviderBLL.GetSP

                If Not udtSP.SPID.Equals(String.Empty) Then
                    If Me.hfTableLocation.Value.Trim.Equals(TableLocation.Staging) Then
                        ibtnExistingSPProfile.Visible = True
                    Else
                        ibtnExistingSPProfile.Visible = False
                    End If
                End If

                If hfTableLocation.Value.Trim.Equals(TableLocation.Permanent) OrElse _
                   hfTableLocation.Value.Trim.Equals(TableLocation.Enrolment) Then
                    lblProcessingDtm.Visible = False
                    lblProcessingDtmText.Visible = False
                Else
                    lblProcessingDtm.Visible = True
                    lblProcessingDtmText.Visible = True
                    lblProcessingDtm.Text = udtFormatter.convertDateTime(udtSP.CreateDtm)
                End If
            End If


        End If

        If Me.TabContainer1.ActiveTabIndex <> 4 Then
            'Active tab is not in scheme information
            If Me.tablSchemeInfo.Visible Then
                If Not IsNothing(udtPracticeBLL.GetPracticeCollection) Then
                    gvSI.DataSource = udtPracticeBLL.GetPracticeCollection.Values
                    gvSI.DataBind()
                End If
            End If
        ElseIf Me.TabContainer1.ActiveTabIndex = 4 Then
            BindServiceFeeGridview()
        End If

    End Sub

    '

    Private Sub BindServiceFeeGridview()
        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = Nothing
        Dim udtSubsidizeGroupBackOfficeList As SubsidizeGroupBackOfficeModelCollection = Nothing

        udtSchemeBackOfficeList = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
        udtSubsidizeGroupBackOfficeList = udtSchemeBackOfficeBLL.GetAllEffectiveSubsidizeGroupFromCache


        Dim dtCheckedScheme As New DataTable
        dtCheckedScheme = Session(SESS_CheckedScheme)

        Dim strTempSF As New List(Of String)
        Dim strTempNotProvider As New List(Of String)
        Dim strTempSelectSubsidize As New List(Of String)
        Dim strTempdisplaySubsidizeAlert As New List(Of String)
        Dim lstNonClinicScheme As New List(Of String)

        Dim udtSchemeBackOffice As SchemeBackOfficeModel
        Dim udtSubsidizeGroupBackOffice As SubsidizeGroupBackOfficeModel

        Dim blndisplayImage As Boolean = False

        For Each row As GridViewRow In gvSI.Rows

            Dim gvEditPracticeSchemeInfo As GridView = CType(row.FindControl("gvEditPracticeSchemeInfo"), GridView)
            If Not IsNothing(gvEditPracticeSchemeInfo) Then
                For Each gvrO As GridViewRow In gvEditPracticeSchemeInfo.Rows
                    Dim strSchemeCode As String = gvrO.Cells(0).Text.Trim
                    Dim strSubsidizeCode As String = gvrO.Cells(1).Text.Trim

                    udtSchemeBackOffice = udtSchemeBackOfficeList.Filter(strSchemeCode)
                    udtSubsidizeGroupBackOffice = udtSubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode)


                    'CRE15-004 TIV & QIV [Start][Winnie]
                    Dim chkEditSelectSubsidize As CheckBox = CType(gvrO.FindControl("chkEditSelectSubsidize"), CheckBox)
                    Dim imgEditSelectSubsidizeAlert As Image = CType(gvrO.FindControl("imgEditSelectSubsidizeAlert"), Image)


                    If imgEditSelectSubsidizeAlert.Visible Then
                        strTempdisplaySubsidizeAlert.Add("Y")
                    Else
                        strTempdisplaySubsidizeAlert.Add("N")
                    End If

                    If udtSubsidizeGroupBackOffice.SubsidyCompulsory Then
                        strTempSelectSubsidize.Add("Y")
                    Else
                        If chkEditSelectSubsidize.Checked Then
                            strTempSelectSubsidize.Add("Y")
                        Else
                            strTempSelectSubsidize.Add("N")
                        End If
                    End If
                    'CRE15-004 TIV & QIV [End][Winnie]

                    ' Non-clinic
                    Dim chkGNonClinic As CheckBox = gvrO.FindControl("chkGNonClinic")

                    If Not IsNothing(chkGNonClinic) AndAlso chkGNonClinic.Visible AndAlso chkGNonClinic.Checked Then
                        lstNonClinicScheme.Add(strSchemeCode)
                    End If

                    Dim txttemp As TextBox = CType(gvrO.FindControl("txtEditPracticeSchemeServiceFee"), TextBox)
                    Dim chkTemp As CheckBox = CType(gvrO.FindControl("chkEditNotProvideServiceFee"), CheckBox)
                    'CRE14-008 - Range of service fee [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Dim pnlTemp As Panel = CType(gvrO.FindControl("pnlEditPracticeSchemeServiceFee"), Panel)
                    'CRE14-008 - Range of service fee [End][Chris YIM]

                    Dim tempImage As Image = CType(gvrO.FindControl("imgEditServiceFeeAlert"), Image)

                    If tempImage.Visible Then
                        blndisplayImage = True
                    End If

                    If udtSubsidizeGroupBackOffice.ServiceFeeEnabled Then
                        If udtSubsidizeGroupBackOffice.ServiceFeeCompulsory Then
                            strTempNotProvider.Add("N")
                            strTempSF.Add(txttemp.Text.Trim)

                        Else
                            If chkTemp.Checked Then
                                strTempNotProvider.Add("Y")
                                strTempSF.Add(String.Empty)
                            Else
                                strTempNotProvider.Add("N")
                                strTempSF.Add(txttemp.Text.Trim)
                            End If
                        End If


                    Else
                        strTempNotProvider.Add(String.Empty)
                        strTempSF.Add(String.Empty)
                    End If
                Next

                gvEditPracticeSchemeInfo.DataSource = udtSubsidizeGroupBackOfficeList.ToSPProfileDataTable
                gvEditPracticeSchemeInfo.DataBind()

                For Each gvrN As GridViewRow In gvEditPracticeSchemeInfo.Rows
                    Dim chkEditSelect As CheckBox
                    'CRE15-004 TIV & QIV [Start][Winnie]
                    Dim pnlEditPracticeSchemeSubsidize As Panel = DirectCast(gvrN.FindControl("pnlEditPracticeSchemeSubsidize"), Panel)
                    Dim pnlEditPracticeSchemeServiceFeeDisplay As Panel = DirectCast(gvrN.FindControl("pnlEditPracticeSchemeServiceFeeDisplay"), Panel)
                    'CRE15-004 TIV & QIV [End][Winnie]

                    'CRE14-008 - Range of service fee [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    'Dim pnlEditPracticeSchemeServiceFee As Panel
                    'CRE14-008 - Range of service fee [End][Chris YIM]

                    Dim txtEditPracticeSchemeServiceFee As TextBox = DirectCast(gvrN.FindControl("txtEditPracticeSchemeServiceFee"), TextBox)
                    Dim chkEditNotProvideServiceFee As CheckBox = DirectCast(gvrN.FindControl("chkEditNotProvideServiceFee"), CheckBox)
                    Dim imgEditServiceFeeAlert As Image
                    chkEditSelect = CType(gvrN.FindControl("chkEditSelect"), CheckBox)

                    Dim strSchemeCode As String = gvrN.Cells(0).Text.Trim
                    Dim strSubsidizeCode As String = gvrN.Cells(1).Text.Trim

                    udtSchemeBackOffice = udtSchemeBackOfficeList.Filter(strSchemeCode)
                    udtSubsidizeGroupBackOffice = udtSubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode)

                    chkEditSelect.Checked = False
                    Dim dv As DataView = New DataView(dtCheckedScheme)
                    dv.RowFilter = "MScheme_Code = '" + strSchemeCode.Trim + "' and Checked = 'Y'"
                    If dv.Count = 1 Then
                        chkEditSelect.Checked = True
                    Else
                        chkEditSelect.Checked = False
                    End If

                    If chkEditSelect.Checked Then
                        ' Non-clinic
                        If lstNonClinicScheme.Contains(strSchemeCode) Then
                            Dim chkGNonClinic As CheckBox = gvrN.FindControl("chkGNonClinic")

                            If Not IsNothing(chkGNonClinic) AndAlso chkGNonClinic.Visible Then
                                chkGNonClinic.Checked = True
                            End If

                        End If

                        'CRE15-004 TIV & QIV [Start][Winnie]  
                        Dim imgEditSelectSubsidizeAlert As Image = CType(gvrN.FindControl("imgEditSelectSubsidizeAlert"), Image)
                        Dim chkEditSelectSubsidize As CheckBox = CType(gvrN.FindControl("chkEditSelectSubsidize"), CheckBox)

                        If udtSubsidizeGroupBackOffice.SubsidyCompulsory Then
                            chkEditSelectSubsidize.Checked = True
                        Else
                            If strTempSelectSubsidize(gvrN.RowIndex).Trim.Equals("Y") Then
                                chkEditSelectSubsidize.Checked = True
                            Else
                                chkEditSelectSubsidize.Checked = False
                            End If
                        End If

                        If strTempdisplaySubsidizeAlert(gvrN.RowIndex).Trim.Equals("Y") Then
                            imgEditSelectSubsidizeAlert.Visible = True
                        End If

                        If chkEditSelectSubsidize.Checked Then
                            'CRE15-004 TIV & QIV [End][Winnie]

                            'CRE14-008 - Range of service fee [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            'pnlEditPracticeSchemeServiceFee = CType(gvrN.FindControl("pnlEditPracticeSchemeServiceFee"), Panel)
                            'CRE14-008 - Range of service fee [End][Chris YIM]

                            txtEditPracticeSchemeServiceFee = CType(gvrN.FindControl("txtEditPracticeSchemeServiceFee"), TextBox)
                            chkEditNotProvideServiceFee = CType(gvrN.FindControl("chkEditNotProvideServiceFee"), CheckBox)
                            imgEditServiceFeeAlert = CType(gvrN.FindControl("imgEditServiceFeeAlert"), Image)

                            imgEditServiceFeeAlert.Visible = False


                            If udtSubsidizeGroupBackOffice.ServiceFeeEnabled Then
                                If udtSubsidizeGroupBackOffice.ServiceFeeCompulsory Then
                                    'CRE14-008 - Range of service fee [Start][Chris YIM]
                                    '-----------------------------------------------------------------------------------------
                                    'chkEditNotProvideServiceFee.Checked = False
                                    'chkEditNotProvideServiceFee.Enabled = True
                                    'CRE14-008 - Range of service fee [End][Chris YIM]
                                    txtEditPracticeSchemeServiceFee.Text = strTempSF(gvrN.RowIndex).Trim

                                    txtEditPracticeSchemeServiceFee.BackColor = Nothing
                                    txtEditPracticeSchemeServiceFee.Attributes.Remove("readonly")
                                Else
                                    If strTempNotProvider(gvrN.RowIndex).Trim.Equals("Y") Then
                                        chkEditNotProvideServiceFee.Checked = True
                                        'CRE14-008 - Range of service fee [Start][Chris YIM]
                                        '-----------------------------------------------------------------------------------------
                                        chkEditNotProvideServiceFee.Enabled = True
                                        'CRE14-008 - Range of service fee [End][Chris YIM]
                                        txtEditPracticeSchemeServiceFee.Text = String.Empty

                                        txtEditPracticeSchemeServiceFee.BackColor = Drawing.Color.WhiteSmoke
                                        txtEditPracticeSchemeServiceFee.Attributes.Add("readonly", "readonly")

                                    ElseIf strTempNotProvider(gvrN.RowIndex).Trim.Equals("N") Then
                                        'CRE14-008 - Range of service fee [Start][Chris YIM]
                                        '-----------------------------------------------------------------------------------------
                                        chkEditNotProvideServiceFee.Enabled = True
                                        'CRE14-008 - Range of service fee [End][Chris YIM]
                                        chkEditNotProvideServiceFee.Checked = False
                                        txtEditPracticeSchemeServiceFee.Text = strTempSF(gvrN.RowIndex).Trim

                                        txtEditPracticeSchemeServiceFee.BackColor = Nothing
                                        txtEditPracticeSchemeServiceFee.Attributes.Remove("readonly")
                                    End If
                                End If


                                If blndisplayImage Then
                                    If Not chkEditNotProvideServiceFee.Checked Then
                                        SM = Validator.chkServiceFee(txtEditPracticeSchemeServiceFee.Text.Trim)
                                        If Not IsNothing(SM) Then
                                            imgEditServiceFeeAlert.Visible = True

                                        End If
                                    End If
                                End If
                            End If

                            'CRE15-004 TIV & QIV [Start][Winnie]                            
                            pnlEditPracticeSchemeServiceFeeDisplay.Enabled = True
                            'CRE15-004 TIV & QIV [End][Winnie]

                            ApplyPracticeSchemeChangeIndicator(row, True)
                        Else
                            'CRE15-004 TIV & QIV [Start][Winnie]                            
                            pnlEditPracticeSchemeServiceFeeDisplay.Enabled = False
                            chkEditNotProvideServiceFee.Checked = False
                            txtEditPracticeSchemeServiceFee.Text = String.Empty
                            'CRE15-004 TIV & QIV [End][Winnie]
                        End If
                        'CRE15-004 TIV & QIV [Start][Winnie]  
                        pnlEditPracticeSchemeSubsidize.Enabled = True
                    Else
                        pnlEditPracticeSchemeSubsidize.Enabled = False
                    End If 'End of chkEditSelect
                    'CRE15-004 TIV & QIV [End][Winnie]  
                Next
            End If
        Next

        For Each gvr As GridViewRow In gvSI.Rows
            Dim gvPracticeSchemeInfo As GridView = CType(gvr.FindControl("gvPracticeSchemeInfo"), GridView)
            If Not IsNothing(gvPracticeSchemeInfo) Then
                gvPracticeSchemeInfo.DataSource = udtSubsidizeGroupBackOfficeList.ToSPProfileDataTable
                gvPracticeSchemeInfo.DataBind()

                ApplyPracticeSchemeChangeIndicator(gvr)
            End If

        Next
    End Sub

    Private Sub SetActionImg()
        Dim blnAbleToUpdate As Boolean = False
        Dim blnAbleToReject As Boolean = False

        Select Case hfTableLocation.Value.Trim
            Case TableLocation.Enrolment
                If blnExisingHKID Then
                    'If udtSPProfileBLL.IsHKIDExistingInServiceProviderStagingPermanent(hfERN.Value.Trim) Then
                    SM = New Common.ComObject.SystemMessage("010101", "I", "00002")
                    CompleteMsgBox.AddMessage(SM)
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                    blnAbleToUpdate = False
                    blnAbleToReject = True

                    ibtnMOAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSDisableBtn")
                    ibtnMOAdd.Enabled = False

                    ibtnPracticeAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSDisableBtn")
                    ibtnPracticeAdd.Enabled = False

                    ibtnEHRSSEdit.Enabled = False
                    ibtnEHRSSEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                Else
                    blnAbleToUpdate = True
                    blnAbleToReject = True

                    ibtnMOAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSBtn")
                    ibtnMOAdd.Enabled = True

                    ibtnPracticeAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSBtn")
                    ibtnPracticeAdd.Enabled = True
                End If


                'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]
                'Hide eHRSS panel when list is empty
                Dim EHRSSProfCodeList As String = String.Empty
                Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

                udtGeneralFunction.getSystemParameter("AskHadJoinedEHRSSProfCode", EHRSSProfCodeList, String.Empty)
                If EHRSSProfCodeList.Equals(String.Empty) Then
                    panEHRSS.Visible = False
                Else
                    panEHRSS.Visible = True
                End If
                'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]

            Case TableLocation.Staging

                Dim strProgressStatus As String = String.Empty
                strProgressStatus = strProgress 'udtSPProfileBLL.GetEnrolmentProcessStatus(udtServiceProviderBLL.GetSP)

                If Not strProgressStatus.Equals(String.Empty) Then
                    Dim strOld As String() = {"%s"}
                    Dim strNew As String() = {""}

                    If udtServiceProviderBLL.GetSP.SPID.Equals(String.Empty) Then
                        strNew(0) = udtServiceProviderBLL.GetSP.EnglishName + udtFormatter.formatChineseName(udtServiceProviderBLL.GetSP.ChineseName) + " [" + _
                                    udtFormatter.formatSystemNumber(udtServiceProviderBLL.GetSP.EnrolRefNo) + "] "
                    Else
                        strNew(0) = udtServiceProviderBLL.GetSP.EnglishName + udtFormatter.formatChineseName(udtServiceProviderBLL.GetSP.ChineseName) + " [" + _
                                    udtFormatter.formatSystemNumber(udtServiceProviderBLL.GetSP.EnrolRefNo) + "] "
                    End If

                    CompleteMsgBox.AddMessage("990000", "I", strProgressStatus, strOld, strNew)
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                    blnAbleToUpdate = False
                    blnAbleToReject = False

                    ibtnMOAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSDisableBtn")
                    ibtnMOAdd.Enabled = False

                    ibtnPracticeAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSDisableBtn")
                    ibtnPracticeAdd.Enabled = False

                    ibtnEHRSSEdit.Enabled = False
                    ibtnEHRSSEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                Else
                    blnAbleToUpdate = True
                    blnAbleToReject = True

                End If

                If udtServiceProviderBLL.GetSP.SPID.Equals(String.Empty) Then
                    panEHRSS.Visible = True
                Else
                    panEHRSS.Visible = False
                End If

                'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]
                'Hide eHRSS panel when list is empty
                Dim EHRSSProfCodeList As String = String.Empty
                Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

                udtGeneralFunction.getSystemParameter("AskHadJoinedEHRSSProfCode", EHRSSProfCodeList, String.Empty)
                If EHRSSProfCodeList.Equals(String.Empty) Then
                    panEHRSS.Visible = False
                End If
                'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]

            Case TableLocation.Permanent
                blnAbleToUpdate = True
                blnAbleToReject = False

                ibtnMOAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSBtn")
                ibtnMOAdd.Enabled = True

                ibtnPracticeAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSBtn")
                ibtnPracticeAdd.Enabled = True

                panEHRSS.Visible = False

            Case String.Empty
                blnAbleToReject = False
                blnAbleToUpdate = True
        End Select

        If blnAbleToUpdate Then

            imgPersonalParticulars.Visible = False
            imgMOInfo.Visible = False
            imgPracticeInfo.Visible = False
            imgBankInfo.Visible = False
            imgSchemeInfo.Visible = False

            Dim blnRes(5) As Boolean
            blnRes = udtSPProfileBLL.SetPageCheckedImg()

            imgPersonalParticulars.Visible = blnRes(0)
            imgMOInfo.Visible = blnRes(1)
            imgPracticeInfo.Visible = blnRes(2)
            imgBankInfo.Visible = blnRes(3)
            imgSchemeInfo.Visible = blnRes(4)


            'Personal Particulars
            If Me.fvPersonalParticulars.CurrentMode = FormViewMode.ReadOnly Then
                Dim ibtnSPPageChecked As ImageButton = CType(fvPersonalParticulars.FindControl("ibtnSPPageChecked"), ImageButton)
                If blnRes(0) Then
                    ibtnSPPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                    ibtnSPPageChecked.Enabled = False
                Else
                    If hfTableLocation.Value.Trim.Equals(TableLocation.Permanent) Then
                        ibtnSPPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                        ibtnSPPageChecked.Enabled = False
                    Else
                        ibtnSPPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedBtn")
                        ibtnSPPageChecked.Enabled = True
                    End If
                End If
            End If

            'MedicalOrganization
            If Me.gvMOInfo.EditIndex > -1 OrElse Me.gvMOInfo.ShowFooter Then
                Me.ibtnMOPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                ibtnMOPageChecked.Enabled = False

                ibtnMOAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSDisableBtn")
                ibtnMOAdd.Enabled = False
            Else
                If blnRes(1) Then
                    ibtnMOPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                    ibtnMOPageChecked.Enabled = False

                    ibtnMOAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSBtn")
                    ibtnMOAdd.Enabled = True

                Else
                    If hfTableLocation.Value.Trim.Equals(TableLocation.Permanent) Then
                        ibtnMOPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                        ibtnMOPageChecked.Enabled = False
                    Else

                        If udtServiceProviderBLL.Exist Then
                            Dim blnShowBtn As Boolean = True

                            If Not IsNothing(udtServiceProviderBLL.GetSP.MOList) Then
                                If udtServiceProviderBLL.GetSP.MOList.Count > 0 Then
                                    For Each udtMO As MedicalOrganizationModel In udtServiceProviderBLL.GetSP.MOList.Values
                                        If udtMO.BrCode.Trim.Equals(String.Empty) Then
                                            blnShowBtn = False
                                        Else
                                            If Not IsNothing(udtServiceProviderBLL.GetSP.PracticeList) Then
                                                If Not IsNothing(udtServiceProviderBLL.GetSP.PracticeList.FilterByMO(udtMO.DisplaySeq.Value)) Then
                                                    If udtServiceProviderBLL.GetSP.PracticeList.FilterByMO(udtMO.DisplaySeq.Value).Count = 0 Then
                                                        blnShowBtn = False
                                                    End If
                                                Else
                                                    blnShowBtn = False
                                                End If
                                            End If
                                        End If


                                    Next
                                End If
                            End If

                            If blnShowBtn Then
                                ibtnMOPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedBtn")
                                ibtnMOPageChecked.Enabled = True
                            Else
                                ibtnMOPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                                ibtnMOPageChecked.Enabled = False
                            End If
                        End If
                    End If

                    ibtnMOAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSBtn")
                    ibtnMOAdd.Enabled = True
                End If
            End If

            'Practice Information
            If Me.gvPracticeInfo.EditIndex > -1 OrElse gvPracticeInfo.ShowFooter Then
                ibtnPracticePageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                ibtnPracticePageChecked.Enabled = False

                ibtnPracticeAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSDisableBtn")
                ibtnPracticeAdd.Enabled = False
            Else

                If blnRes(2) Then
                    If panEHRSS.Visible Then
                        ' Remove the Page Check if rboHadJoinedEHRSS is not selected
                        If rboHadJoinedEHRSS.Enabled And rboHadJoinedEHRSS.SelectedValue.Equals(String.Empty) Then
                            blnRes(2) = False
                            imgPracticeInfo.Visible = False
                        End If

                        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                        ' --------------------------------------------------------------------------------------------------------------------------------
                        ' Remove the Page Check if rboJoinPCD is not selected
                        If rboJoinPCD.Enabled And rboJoinPCD.SelectedValue.Equals(String.Empty) Then
                            blnRes(2) = False
                            imgPracticeInfo.Visible = False
                        End If
                        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
                    End If
                End If


                If blnRes(2) Then
                    ibtnPracticePageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                    ibtnPracticePageChecked.Enabled = False

                    ibtnPracticeAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSBtn")
                    ibtnPracticeAdd.Enabled = True
                Else
                    If hfTableLocation.Value.Trim.Equals(TableLocation.Permanent) Then
                        ibtnPracticePageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                        ibtnPracticePageChecked.Enabled = False
                    Else
                        If udtServiceProviderBLL.Exist Then
                            Dim blnShowBtn As Boolean = False
                            Dim udtPracticeList As PracticeModelCollection = udtServiceProviderBLL.GetSP.PracticeList.FilterByMO(0)

                            If IsNothing(udtPracticeList) Then
                                blnShowBtn = True
                            Else
                                If udtPracticeList.Count = 0 Then
                                    blnShowBtn = True
                                Else
                                    blnShowBtn = False
                                End If
                            End If
                            If blnShowBtn Then
                                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                                ' --------------------------------------------------------------------------------------------------------------------------------
                                If panEHRSS.Visible And (rboHadJoinedEHRSS.Enabled Or rboJoinPCD.Enabled) Then
                                    ibtnPracticePageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                                    ibtnPracticePageChecked.Enabled = False
                                Else
                                    ibtnPracticePageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedBtn")
                                    ibtnPracticePageChecked.Enabled = True
                                End If
                                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
                            End If
                        End If

                    End If

                    ibtnPracticeAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSBtn")
                    ibtnPracticeAdd.Enabled = True

                End If
            End If

            'Bank Information
            If Me.gvBankInfo.EditIndex > -1 Then
                ibtnBankAcctPageChecked.ImageUrl = GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                ibtnBankAcctPageChecked.Enabled = False
            Else
                If blnRes(3) Then
                    ibtnBankAcctPageChecked.ImageUrl = GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                    ibtnBankAcctPageChecked.Enabled = False
                Else
                    If hfTableLocation.Value.Trim.Equals(TableLocation.Permanent) Then
                        ibtnBankAcctPageChecked.ImageUrl = GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                        ibtnBankAcctPageChecked.Enabled = False
                    Else
                        Dim udtBankAcctBLL As New BankAcctBLL

                        If udtBankAcctBLL.Exist AndAlso udtServiceProviderBLL.Exist Then
                            If udtBankAcctBLL.GetBankAcctCollection.Count <> udtServiceProviderBLL.GetSP.PracticeList.Count Then
                                ibtnBankAcctPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                                ibtnBankAcctPageChecked.Enabled = False
                            Else
                                ibtnBankAcctPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedBtn")
                                ibtnBankAcctPageChecked.Enabled = True
                            End If
                        End If
                    End If

                End If
            End If

            'Scheme Infomation
            If gvSI.EditIndex > -1 Then
                ibtnSchemePageChecked.ImageUrl = GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                ibtnSchemePageChecked.Enabled = False
            Else
                If blnRes(4) Then
                    ibtnSchemePageChecked.ImageUrl = GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                    ibtnSchemePageChecked.Enabled = False
                Else
                    If hfTableLocation.Value.Trim.Equals(TableLocation.Permanent) Then
                        ibtnSchemePageChecked.ImageUrl = GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                        ibtnSchemePageChecked.Enabled = False
                    Else
                        If udtServiceProviderBLL.Exist Then

                            Dim blnNotChecked As Boolean = False

                            If udtServiceProviderBLL.Exist Then
                                'CRE14-002 PPIEPR migration [Start][Karl]
                                'The delisted practice should not check for the scheme /subsidize fee

                                Dim udtNewPracticeList As PracticeModelCollection = New PracticeModelCollection

                                Dim udtNewPractice As PracticeModel

                                For Each udtNewPractice In udtServiceProviderBLL.GetSP.PracticeList.Values
                                    If udtNewPractice.RecordStatus <> PracticeStatus.Delisted Then
                                        udtNewPracticeList.Add(udtNewPractice)
                                    End If
                                Next

                                For Each udtPractice As PracticeModel In udtNewPracticeList.Values
                                    'For Each udtPractice As PracticeModel In udtServiceProviderBLL.GetSP.PracticeList.Values
                                    'CRE14-002 PPIEPR migration [End][Karl]
                                    If IsNothing(udtPractice.PracticeSchemeInfoList) Then
                                        blnNotChecked = True
                                    Else
                                        If udtPractice.PracticeSchemeInfoList.Count = 0 Then
                                            blnNotChecked = True
                                        Else
                                            Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = Nothing
                                            Dim udtSubsidizeGroupBackOfficeList As SubsidizeGroupBackOfficeModelCollection = Nothing

                                            udtSchemeBackOfficeList = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
                                            udtSubsidizeGroupBackOfficeList = udtSchemeBackOfficeBLL.GetAllEffectiveSubsidizeGroupFromCache

                                            For Each udtSchemeBO As SchemeBackOfficeModel In udtSchemeBackOfficeList
                                                If Not IsNothing(udtPractice.PracticeSchemeInfoList.Filter(udtSchemeBO.SchemeCode.Trim)) Then

                                                    If Not IsNothing(udtSchemeBO.SubsidizeGroupBackOfficeList) Then
                                                        For Each udtSubsidizeGroupBO As SubsidizeGroupBackOfficeModel In udtSchemeBO.SubsidizeGroupBackOfficeList
                                                            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

                                                            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                                                            ' Add SchemeCode to key
                                                            udtPracticeSchemeInfo = udtPractice.PracticeSchemeInfoList.Item(udtPractice.DisplaySeq, udtSubsidizeGroupBO.SubsidizeCode, udtSchemeBO.DisplaySeq, udtSubsidizeGroupBO.DisplaySeq, udtSubsidizeGroupBO.SchemeCode)
                                                            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                                                            If IsNothing(udtPracticeSchemeInfo) Then
                                                                'CRE15-004 TIV & QIV [Start][Winnie]
                                                                'blnNotChecked = True
                                                                'CRE15-004 TIV & QIV [End][Winnie]
                                                            Else
                                                                'CRE15-004 TIV & QIV [Start][Winnie]
                                                                If udtPracticeSchemeInfo.ProvideService Then
                                                                    'CRE15-004 TIV & QIV [End][Winnie]
                                                                    If udtSubsidizeGroupBO.ServiceFeeEnabled Then
                                                                        If udtSubsidizeGroupBO.ServiceFeeCompulsory Then
                                                                            If Not udtPracticeSchemeInfo.ServiceFee.HasValue Then
                                                                                blnNotChecked = True
                                                                            End If
                                                                        Else
                                                                            If Not udtPracticeSchemeInfo.ProvideServiceFee.HasValue Then
                                                                                blnNotChecked = True
                                                                            End If

                                                                        End If
                                                                    End If
                                                                    'CRE15-004 TIV & QIV [Start][Winnie]
                                                                End If
                                                                'CRE15-004 TIV & QIV [End][Winnie]
                                                            End If
                                                        Next
                                                    End If
                                                End If
                                            Next
                                        End If
                                    End If
                                Next
                            End If



                            If blnNotChecked Then
                                ibtnSchemePageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                                ibtnSchemePageChecked.Enabled = False
                            Else
                                ibtnSchemePageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedBtn")
                                ibtnSchemePageChecked.Enabled = True
                            End If
                        End If
                    End If

                End If
            End If

            'Set "Proceed To Vetting" Button
            If hfTableLocation.Value.Trim.Equals(TableLocation.Permanent) Then
                ibtnProceedToVetting.Enabled = False
                ibtnProceedToVetting.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToVettingDisableBtn")
            Else
                If blnRes(0) And blnRes(1) And blnRes(2) And blnRes(3) And blnRes(4) Then
                    If udtServiceProviderBLL.Exist Then
                        If udtServiceProviderBLL.GetSP.SPID.Equals(String.Empty) Then

                            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                            If Me.fvPersonalParticulars.CurrentMode = FormViewMode.Edit OrElse _
                                                    gvPracticeInfo.EditIndex > -1 OrElse _
                                                    gvPracticeInfo.ShowFooter OrElse _
                                                    gvBankInfo.EditIndex > -1 OrElse _
                                                    rboHadJoinedEHRSS.Enabled OrElse _
                                                    rboJoinPCD.Enabled Then
                                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

                                ibtnProceedToVetting.Enabled = False
                                ibtnProceedToVetting.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToVettingDisableBtn")
                            Else
                                ibtnProceedToVetting.Enabled = True
                                ibtnProceedToVetting.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToVettingBtn")

                            End If
                        Else
                            If Me.fvPersonalParticulars.CurrentMode = FormViewMode.Edit OrElse _
                                                    gvPracticeInfo.EditIndex > -1 OrElse _
                                                    gvPracticeInfo.ShowFooter OrElse _
                                                    gvBankInfo.EditIndex > -1 Then

                                ibtnProceedToVetting.Enabled = False
                                ibtnProceedToVetting.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToVettingDisableBtn")
                            Else
                                ibtnProceedToVetting.Enabled = True
                                ibtnProceedToVetting.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToVettingBtn")

                            End If
                        End If
                    End If


                    'If Me.fvPersonalParticulars.CurrentMode = FormViewMode.Edit OrElse _
                    '    gvPracticeInfo.EditIndex > -1 OrElse _
                    '    gvPracticeInfo.ShowFooter OrElse _
                    '    gvBankInfo.EditIndex > -1 OrElse _
                    '    Not ibtnPPIePREdit.Visible Then

                    '    ibtnProceedToVetting.Enabled = False
                    '    ibtnProceedToVetting.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToVettingDisableBtn")
                    'Else
                    '    ibtnProceedToVetting.Enabled = True
                    '    ibtnProceedToVetting.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToVettingBtn")

                    'End If
                Else
                    ibtnProceedToVetting.Enabled = False
                    ibtnProceedToVetting.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToVettingDisableBtn")
                End If
            End If

        Else
            imgPersonalParticulars.Visible = False
            imgMOInfo.Visible = False
            imgPracticeInfo.Visible = False
            imgBankInfo.Visible = False
            imgSchemeInfo.Visible = False

            If Me.fvPersonalParticulars.CurrentMode = FormViewMode.ReadOnly Then
                Dim ibtnSPPageChecked As ImageButton = CType(fvPersonalParticulars.FindControl("ibtnSPPageChecked"), ImageButton)
                Dim ibtnSPEdit As ImageButton = CType(fvPersonalParticulars.FindControl("ibtnSPEdit"), ImageButton)

                ibtnSPPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                ibtnSPPageChecked.Enabled = False

                ibtnSPEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditDisableBtn")
                ibtnSPEdit.Enabled = False
            End If

            ibtnMOPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
            ibtnMOPageChecked.Enabled = False

            ibtnPracticePageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
            ibtnPracticePageChecked.Enabled = False

            ibtnBankAcctPageChecked.ImageUrl = GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
            ibtnBankAcctPageChecked.Enabled = False

            ibtnSchemePageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
            ibtnSchemePageChecked.Enabled = False

            ibtnProceedToVetting.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "ProceedToVettingDisableBtn")
            ibtnProceedToVetting.Enabled = False
        End If

        If blnAbleToReject Then
            ibtnReject.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AbortBtn")
            ibtnReject.Enabled = True
        Else
            ibtnReject.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AbortDisableBtn")
            ibtnReject.Enabled = False
        End If
    End Sub

    Private Sub LoadPracticeOthersInfo()
        Dim udtSP As ServiceProviderModel = Nothing
        If udtServiceProviderBLL.Exist Then
            udtSP = udtServiceProviderBLL.GetSP

            If Not udtSP.AlreadyJoinEHR.Equals(JoinEHRSSStatus.NA) Then
                rboHadJoinedEHRSS.SelectedValue = udtSP.AlreadyJoinEHR.Trim
            Else
                'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]
                'Handle NA status
                rboHadJoinedEHRSS.ClearSelection()
            End If
            'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' --------------------------------------------------------------------------------------------------------------------------------
            If Not udtSP.JoinPCD.Equals(JoinPCDStatus.NA) And Not udtSP.JoinPCD.Equals(String.Empty) Then
                rboJoinPCD.SelectedValue = udtSP.JoinPCD.Trim
            Else
                'Handle NA status
                rboJoinPCD.ClearSelection()
            End If
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
        End If
    End Sub

#End Region

    Private Sub BindSPProfile()
        If udtServiceProviderBLL.Exist Then
            Dim udtServiceProviderCollection As ServiceProviderModelCollection = New ServiceProviderModelCollection
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
            udtServiceProviderCollection.Add(udtSP.EnrolRefNo, udtSP)
            fvPersonalParticulars.DataSource = udtServiceProviderCollection.Values
            fvPersonalParticulars.ChangeMode(FormViewMode.ReadOnly)
            fvPersonalParticulars.DataBind()

            If udtSP.MOList.Count = 0 Then
                gvMOInfo.DataSource = udtSPProfileBLL.EmptyMOCollection.Values
                gvMOInfo.DataBind()
            Else
                gvMOInfo.DataSource = udtSP.MOList.Values

                gvMOInfo.EditIndex = setEditIndexMOGridview()

                gvMOInfo.DataBind()
            End If

            If IsNothing(udtSP.PracticeList) Then
                gvPracticeInfo.DataSource = udtSPProfileBLL.EmptyPracticeCollection.Values
                gvPracticeInfo.DataBind()

                gvBankInfo.DataSource = Nothing
                gvBankInfo.DataBind()

                gvSI.DataSource = Nothing
                gvSI.DataBind()
            Else
                If udtSP.PracticeList.Count = 0 Then
                    gvPracticeInfo.DataSource = udtSPProfileBLL.EmptyPracticeCollection.Values
                    gvPracticeInfo.DataBind()

                    gvBankInfo.DataSource = Nothing
                    gvBankInfo.DataBind()

                    gvSI.DataSource = Nothing
                    gvSI.DataBind()
                Else

                    gvPracticeInfo.DataSource = udtPracticeBLL.GetPracticeCollection.Values
                    gvPracticeInfo.EditIndex = Me.setEditIndexPracticeGridview
                    gvPracticeInfo.DataBind()

                    gvBankInfo.DataSource = udtPracticeBLL.GetPracticeCollection.Values
                    gvBankInfo.EditIndex = Me.setEditIndexBankGridview
                    gvBankInfo.DataBind()

                    gvSI.DataSource = udtPracticeBLL.GetPracticeCollection.Values
                    gvSI.EditIndex = Me.setEditIndexSchemeGridview
                    gvSI.DataBind()

                End If

            End If

            ' CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]
            LoadPracticeOthersInfo()
            SetPracticeAdditionalInfo(False)
            ' CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]
        End If

    End Sub

    Private Sub bindDistrict(ByVal ddl As DropDownList, ByVal strAreaCode As String, ByVal blnReset As Boolean)
        ddl.Items.Clear()
        ddl.DataSource = udtSPProfileBLL.GetDistrict(strAreaCode)
        ddl.DataValueField = "District_ID"
        ddl.DataTextField = "District_Name"
        ddl.DataBind()
        ddl.Items.Insert(0, New ListItem(Me.GetGlobalResourceObject("Text", "SelectDistrict"), ""))
        If blnReset Then
            ddl.SelectedIndex = 0
        End If
    End Sub

    Private Function setEditIndexMOGridview() As Integer
        Dim intEditIndex As Integer = -1

        If udtServiceProviderBLL.Exist Then
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP

            For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
                If udtMO.BrCode.Trim.Equals(String.Empty) Then
                    If intEditIndex = -1 Then
                        intEditIndex = udtMO.DisplaySeq.Value - 1
                    End If
                End If
            Next
        End If

        Return intEditIndex
    End Function

    Private Function setEditIndexPracticeGridview() As Integer
        Dim intEditIndex As Integer = -1

        If udtServiceProviderBLL.Exist Then
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP

            For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
                If udtPractice.MODisplaySeq = 0 Then
                    If intEditIndex = -1 Then
                        intEditIndex = udtPractice.DisplaySeq - 1
                    End If
                End If
            Next
        End If

        Return intEditIndex
    End Function

    Private Function setEditIndexBankGridview() As Integer
        Dim intEditIndex As Integer = -1

        If udtServiceProviderBLL.Exist Then
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP

            For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
                If udtPractice.BankAcct.BankName.Trim.Equals(String.Empty) Then
                    If intEditIndex = -1 Then
                        intEditIndex = udtPractice.DisplaySeq - 1
                    End If
                End If
            Next
        End If

        Return intEditIndex
    End Function

    Private Function setEditIndexSchemeGridview() As Integer
        Dim intEditIndex As Integer = -1

        '' Check the Practice has Practice Scheme Info
        '' 1. If no Practice Scheme Info => change the Gridview in Edit Mode
        '' 2. If the Practice Scheme Info List exist 
        ''    a. If subsidy selected, Check service fee, if no Service Fee => change the Gridview in Edit Mode


        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = Nothing
        Dim udtSubsidizeGroupBackOfficeList As SubsidizeGroupBackOfficeModelCollection = Nothing

        udtSchemeBackOfficeList = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
        udtSubsidizeGroupBackOfficeList = udtSchemeBackOfficeBLL.GetAllEffectiveSubsidizeGroupFromCache

        If udtServiceProviderBLL.Exist Then
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP

            For Each udtPractice As PracticeModel In udtSP.PracticeList.Values

                If IsNothing(udtPractice.PracticeSchemeInfoList) Then
                    If intEditIndex = -1 Then
                        intEditIndex = udtPractice.DisplaySeq - 1
                    End If
                Else
                    If udtPractice.PracticeSchemeInfoList.Count = 0 Then
                        If intEditIndex = -1 Then
                            intEditIndex = udtPractice.DisplaySeq - 1
                        End If
                    Else

                        For Each udtSchemeBO As SchemeBackOfficeModel In udtSchemeBackOfficeList
                            Dim udtPracticeSchemeList As PracticeSchemeInfoModelCollection
                            udtPracticeSchemeList = udtPractice.PracticeSchemeInfoList

                            If Not IsNothing(udtPracticeSchemeList.Filter(udtSchemeBO.SchemeCode)) Then

                                For Each udtSubsidizeGroupBO As SubsidizeGroupBackOfficeModel In udtSchemeBO.SubsidizeGroupBackOfficeList
                                    Dim udtPracticeScheme As PracticeSchemeInfoModel

                                    udtPracticeScheme = udtPracticeSchemeList.Filter(udtSubsidizeGroupBO.SchemeCode, udtSubsidizeGroupBO.SubsidizeCode)
                                    Dim blnNeedToSet As Boolean = False

                                    Select Case hfTableLocation.Value.Trim
                                        Case TableLocation.Enrolment
                                            blnNeedToSet = True
                                        Case TableLocation.Staging
                                            Select Case udtSPProfileBLL.GetPracticeSchemeInfoStatus(udtPractice, udtSchemeBO.SchemeCode.Trim, hfTableLocation.Value)
                                                Case PracticeSchemeInfoStagingStatus.Active, PracticeSchemeInfoStagingStatus.Update, PracticeSchemeInfoStagingStatus.Existing
                                                    blnNeedToSet = True
                                            End Select
                                        Case TableLocation.Permanent
                                            Select Case udtSPProfileBLL.GetPracticeSchemeInfoStatus(udtPractice, udtSchemeBO.SchemeCode.Trim, hfTableLocation.Value)
                                                Case PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary, PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary
                                                    blnNeedToSet = False
                                                Case Else
                                                    blnNeedToSet = True
                                            End Select
                                    End Select

                                    If blnNeedToSet Then
                                        If IsNothing(udtPracticeScheme) Then
                                            'CRE15-004 TIV & QIV [Start][Winnie]
                                            If udtSubsidizeGroupBO.SubsidyCompulsory Then
                                                If intEditIndex = -1 Then
                                                    intEditIndex = udtPractice.DisplaySeq - 1
                                                End If
                                            End If
                                            'CRE15-004 TIV & QIV [End][Winnie]
                                        Else
                                            'CRE15-004 TIV & QIV [Start][Winnie]
                                            If udtPracticeScheme.ProvideService Then
                                                If udtSubsidizeGroupBO.ServiceFeeEnabled Then
                                                    If udtSubsidizeGroupBO.ServiceFeeCompulsory Then
                                                        If Not udtPracticeScheme.ServiceFee.HasValue Then
                                                            If intEditIndex = -1 Then
                                                                intEditIndex = udtPracticeScheme.PracticeDisplaySeq - 1
                                                            End If
                                                        End If
                                                    Else
                                                        If Not udtPracticeScheme.ProvideServiceFee.HasValue Then
                                                            If intEditIndex = -1 Then
                                                                intEditIndex = udtPracticeScheme.PracticeDisplaySeq - 1
                                                            End If
                                                        End If
                                                    End If
                                                End If
                                            End If
                                            'CRE15-004 TIV & QIV [End][Winnie]
                                        End If
                                    End If
                                Next
                            End If

                        Next
                    End If

                End If
            Next
        End If

        Return intEditIndex
    End Function

    '

    Protected Sub rboArea_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlDistrict As DropDownList = Nothing
        Dim rboArea As RadioButtonList = Nothing

        If TabContainer1.ActiveTabIndex = 0 Then
            If fvPersonalParticulars.CurrentMode = FormViewMode.Insert Then
                ddlDistrict = CType(fvPersonalParticulars.FindControl("ddlRegDistrict"), DropDownList)
                rboArea = CType(fvPersonalParticulars.FindControl("rboRegArea"), RadioButtonList)

            ElseIf fvPersonalParticulars.CurrentMode = FormViewMode.Edit Then
                ddlDistrict = CType(fvPersonalParticulars.FindControl("ddlSPEditDistrict"), DropDownList)
                rboArea = CType(fvPersonalParticulars.FindControl("rboSPEditArea"), RadioButtonList)
            End If
        ElseIf TabContainer1.ActiveTabIndex = 1 Then
            If Not hfGridviewIndex.Value.Equals(String.Empty) Then
                If gvMOInfo.ShowFooter Then
                    'Add MO 
                    ddlDistrict = gvMOInfo.FooterRow.FindControl("ddlAddMODistrict")
                    rboArea = gvMOInfo.FooterRow.FindControl("rbAddMOArea")
                Else
                    'Edit MO
                    ddlDistrict = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlEditMODistrict")
                    rboArea = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("rbEditMOArea")
                End If

            End If
        ElseIf TabContainer1.ActiveTabIndex = 2 Then
            If Not hfGridviewIndex.Value.Equals(String.Empty) Then
                If gvPracticeInfo.ShowFooter Then
                    'Add Practice 
                    ddlDistrict = gvPracticeInfo.FooterRow.FindControl("ddlAddPracticeDistrict")
                    rboArea = gvPracticeInfo.FooterRow.FindControl("rbAddPracticeArea")
                Else
                    'Edit Practice
                    ddlDistrict = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlEditPracticeDistrict")
                    rboArea = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("rbEditPracticeArea")
                End If

            End If
        End If

        If Not IsNothing(ddlDistrict) And Not IsNothing(rboArea) Then
            'bindDistrict(ddlDistrict, rboArea.SelectedValue, True)

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            'Select Case rboArea.SelectedValue
            '    Case 1
            '        ddlDistrict.SelectedValue = ".H"
            '    Case 2
            '        ddlDistrict.SelectedValue = ".K"
            '    Case 3
            '        ddlDistrict.SelectedValue = ".N"
            'End Select

            ddlDistrict.SelectedValue = "." + rboArea.SelectedValue
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
        End If

    End Sub

#Region "Common - Structural Address"

    Protected Sub ibtnSearchAddress_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As AuditLogEntry = New AuditLogEntry(strFuncCode, Me)
        Dim strStartLog As String = String.Empty
        Dim strEndLog As String = String.Empty
        Dim strError As String = String.Empty
        Dim strLogID As String = String.Empty
        Dim strHKIC As String = String.Empty

        Dim txtBuilding As TextBox = Nothing
        Dim hfAddressCode As HiddenField = Nothing
        Dim ddlDistrict As DropDownList = Nothing
        Dim rboArea As RadioButtonList = Nothing
        Dim imgBuildingAlert As Image = Nothing
        Dim txtRegHKID As TextBox = Nothing

        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        udtAuditLogEntry.WriteLog(LogID.LOG00105, "Service Provider - Search Address")
        ' CRE11-021 log the missed essential information [End]

        If Me.TabContainer1.ActiveTabIndex = 0 Then
            'Tab "Personal Paticulars"
            If fvPersonalParticulars.CurrentMode = FormViewMode.Insert Then
                txtBuilding = CType(fvPersonalParticulars.FindControl("txtRegEAddress"), TextBox)
                hfAddressCode = CType(fvPersonalParticulars.FindControl("hfRegAddressCode"), HiddenField)
                ddlDistrict = CType(fvPersonalParticulars.FindControl("ddlRegDistrict"), DropDownList)
                rboArea = CType(fvPersonalParticulars.FindControl("rboRegArea"), RadioButtonList)
                imgBuildingAlert = CType(fvPersonalParticulars.FindControl("imgEAddressAlert"), Image)

                txtRegHKID = CType(fvPersonalParticulars.FindControl("txtRegHKID"), TextBox)

                udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                strLogID = Common.Component.LogID.LOG00023
                strStartLog = "Search Structure Address in Personal"
                strEndLog = "Search Structure Address in Personal Completed."
                strError = "Search Structure Address in Personal Fail"
                strHKIC = udtFormatter.formatHKIDInternal(txtRegHKID.Text)

            ElseIf fvPersonalParticulars.CurrentMode = FormViewMode.Edit Then
                txtBuilding = CType(fvPersonalParticulars.FindControl("txtSPEditBuilding"), TextBox)
                hfAddressCode = CType(fvPersonalParticulars.FindControl("hfSPEditAddressCode"), HiddenField)
                ddlDistrict = CType(fvPersonalParticulars.FindControl("ddlSPEditDistrict"), DropDownList)
                rboArea = CType(fvPersonalParticulars.FindControl("rboSPEditArea"), RadioButtonList)
                imgBuildingAlert = CType(fvPersonalParticulars.FindControl("imgSPEditBuildingAlert"), Image)

                udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                strLogID = Common.Component.LogID.LOG00023
                strStartLog = "Search Structure Address in Personal"
                strEndLog = "Search Structure Address in Personal Completed."
                strError = "Search Structure Address in Personal Fail"

            End If

            gvStructureAddress.Columns(2).Visible = False
        ElseIf Me.TabContainer1.ActiveTabIndex = 1 Then
            'Tab "Medical Organization"
            If Not hfGridviewIndex.Value.Equals(String.Empty) Then
                If Me.gvMOInfo.ShowFooter Then
                    'Add MO 
                    txtBuilding = gvMOInfo.FooterRow.FindControl("txtAddMOBuilding")
                    hfAddressCode = gvMOInfo.FooterRow.FindControl("hfAddMOAddressCode")
                    ddlDistrict = gvMOInfo.FooterRow.FindControl("ddlAddMODistrict")
                    rboArea = gvMOInfo.FooterRow.FindControl("rbAddMOArea")
                    imgBuildingAlert = gvMOInfo.FooterRow.FindControl("imgAddMOBuildingAlert")

                    udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                    strLogID = Common.Component.LogID.LOG00058
                    strStartLog = "Search Structure Address in MO"
                    strEndLog = "Search Structure Address in MO Completed."
                    strError = "Search Structure Address in MO Fail"
                Else
                    'Edit MO
                    txtBuilding = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtEditMOBuilding")
                    hfAddressCode = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("hfEditMOAddressCode")
                    ddlDistrict = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlEditMODistrict")
                    rboArea = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("rbEditMOArea")
                    imgBuildingAlert = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("imgEditMOBuildingAlert")

                    udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                    strLogID = Common.Component.LogID.LOG00058
                    strStartLog = "Search Structure Address in MO"
                    strEndLog = "Search Structure Address in MO Completed."
                    strError = "Search Structure Address in MO Fail"
                End If

            End If
            gvStructureAddress.Columns(2).Visible = False
        ElseIf Me.TabContainer1.ActiveTabIndex = 2 Then
            'Tab "Practice Information"
            If Not hfGridviewIndex.Value.Equals(String.Empty) Then
                If gvPracticeInfo.ShowFooter Then
                    'Add Practice 
                    txtBuilding = gvPracticeInfo.FooterRow.FindControl("txtAddPracticeBuilding")
                    hfAddressCode = gvPracticeInfo.FooterRow.FindControl("hfAddPracticeAddressCode")
                    ddlDistrict = gvPracticeInfo.FooterRow.FindControl("ddlAddPracticeDistrict")
                    rboArea = gvPracticeInfo.FooterRow.FindControl("rbAddPracticeArea")
                    imgBuildingAlert = gvPracticeInfo.FooterRow.FindControl("imgAddPracticeBuildingAlert")

                    udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                    strLogID = Common.Component.LogID.LOG00027
                    strStartLog = "Search Structure Address in Practice"
                    strEndLog = "Search Structure Address in Practice Completed."
                    strError = "Search Structure Address in Practice Fail"
                Else
                    'Edit Practice
                    txtBuilding = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtEditPracticeBuilding")
                    hfAddressCode = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("hfEditPracticeAddressCode")
                    ddlDistrict = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlEditPracticeDistrict")
                    rboArea = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("rbEditPracticeArea")
                    imgBuildingAlert = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("imgEditPracticeBuildingAlert")

                    udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                    strLogID = Common.Component.LogID.LOG00027
                    strStartLog = "Search Structure Address in Practice"
                    strEndLog = "Search Structure Address in Practice Completed."
                    strError = "Search Structure Address in Practice Fail"
                End If

            End If

            gvStructureAddress.Columns(2).Visible = True
        End If

        Dim strDistrict As String = String.Empty

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'If ddlDistrict.SelectedValue.Trim.Equals(".H") OrElse ddlDistrict.SelectedValue.Trim.Equals(".K") _
        '    OrElse ddlDistrict.SelectedValue.Equals(".N") Then
        If ddlDistrict.SelectedValue.Trim.StartsWith(".") Then
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
            strDistrict = String.Empty
        End If

        udtAuditLogEntry.AddDescripton("Building", txtBuilding.Text.Trim)
        udtAuditLogEntry.AddDescripton("District", strDistrict)
        udtAuditLogEntry.AddDescripton("Area", rboArea.SelectedValue)

        If IsNothing(txtRegHKID) Then
            udtAuditLogEntry.WriteStartLog(strLogID, strStartLog)
        Else
            udtAuditLogEntry.WriteStartLog(strLogID, strStartLog, New Common.ComObject.AuditLogInfo(Nothing, strHKIC, "", "", "", ""))
        End If


        'initialize the "visible" of  error icon to FALSE
        imgBuildingAlert.Visible = False

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        SM = Validator.chkSearchStructureAddress(txtBuilding.Text.Trim)
        If Not SM Is Nothing Then
            imgBuildingAlert.Visible = True
            msgBox.AddMessage(SM, "%d", udtSPProfileBLL.GetMinNoOfWordToSearch)
        End If

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False

            'Dim udtAddressModelCollection As AddressModelCollection = New AddressModelCollection
            Dim dt As DataTable = New DataTable

            Try
                dt = udtSPProfileBLL.SearchStructureAddress(txtBuilding.Text.Trim, rboArea.SelectedValue, strDistrict)
                Session("Address") = dt

                If dt.Rows.Count = 0 Then
                    SM = New Common.ComObject.SystemMessage("990000", "I", "00003")
                    CompleteMsgBox.AddMessage(SM)
                    CompleteMsgBox.BuildMessageBox()
                    CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information

                    If strLogID.Equals(Common.Component.LogID.LOG00023) Then
                        If IsNothing(txtRegHKID) Then
                            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00024, "Search Structure Address in Personal Completed. No records found")
                        Else
                            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00024, "Search Structure Address in Personal Completed. No records found", New Common.ComObject.AuditLogInfo(Nothing, strHKIC, "", "", "", ""))
                        End If

                    ElseIf strLogID.Equals(Common.Component.LogID.LOG00058) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00059, "Search Structure Address in MO Completed. No records found")
                    ElseIf strLogID.Equals(Common.Component.LogID.LOG00027) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00028, "Search Structure Address in Practice Completed. No records found")
                    End If

                Else
                    Me.GridViewDataBind(Me.gvStructureAddress, dt, "", "", False)
                    ModalPopupExtender.Show()

                    If strLogID.Equals(Common.Component.LogID.LOG00023) Then

                        If IsNothing(txtRegHKID) Then
                            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00025, strEndLog)
                        Else
                            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00025, strEndLog, New Common.ComObject.AuditLogInfo(Nothing, strHKIC, "", "", "", ""))
                        End If

                    ElseIf strLogID.Equals(Common.Component.LogID.LOG00058) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00060, strEndLog)
                    ElseIf strLogID.Equals(Common.Component.LogID.LOG00027) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00029, strEndLog)
                    End If

                End If
            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then
                    Dim strmsg As String
                    strmsg = eSQL.Message

                    SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                    msgBox.AddMessage(SM)
                    If msgBox.GetCodeTable.Rows.Count = 0 Then
                        msgBox.Visible = False
                    Else
                        ' msgBox.BuildMessageBox("SearchFail")
                        If strLogID.Equals(Common.Component.LogID.LOG00023) Then
                            If IsNothing(txtRegHKID) Then
                                msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, Common.Component.LogID.LOG00026, strError)
                            Else
                                msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, Common.Component.LogID.LOG00026, strError, New Common.ComObject.AuditLogInfo(Nothing, strHKIC, "", "", "", ""))
                            End If

                        ElseIf strLogID.Equals(Common.Component.LogID.LOG00058) Then
                            msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, Common.Component.LogID.LOG00061, strError)
                        ElseIf strLogID.Equals(Common.Component.LogID.LOG00027) Then
                            msgBox.BuildMessageBox("SearchFail", udtAuditLogEntry, Common.Component.LogID.LOG00030, strError)
                        End If
                    End If

                    'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                Else
                    Throw eSQL
                End If

            Catch ex As Exception
                Throw ex
            End Try

        Else
            'msgBox.BuildMessageBox("ValidationFail")

            If strLogID.Equals(Common.Component.LogID.LOG00023) Then
                If IsNothing(txtRegHKID) Then
                    msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00026, strError)
                Else
                    msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00026, strError, New Common.ComObject.AuditLogInfo(Nothing, strHKIC, "", "", "", ""))
                End If

            ElseIf strLogID.Equals(Common.Component.LogID.LOG00027) Then
                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00030, strError)
            ElseIf strLogID.Equals(Common.Component.LogID.LOG00058) Then
                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00061, strError)
            End If

        End If
        'udtAuditLogEntry.WriteLog("010101", "I", "00005")

    End Sub

    Protected Sub ibtnClearSearchAddress_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim txtBlock As TextBox = Nothing
        Dim txtBuilding As TextBox = Nothing
        Dim txtBuildingChi As TextBox = Nothing
        Dim hfAddressCode As HiddenField = Nothing
        Dim ddlDistrict As DropDownList = Nothing
        Dim rboArea As RadioButtonList = Nothing
        Dim imgBuildingAlert As Image = Nothing
        Dim ibtnAddressSearch As ImageButton = Nothing

        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        udtAuditLogEntry.WriteLog(LogID.LOG00108, "Service Provider - Clear Address Search")
        ' CRE11-021 log the missed essential information [End]

        If Me.TabContainer1.ActiveTabIndex = 0 Then
            'Tab "Personal Paticulars"
            If fvPersonalParticulars.CurrentMode = FormViewMode.Insert Then
                txtBlock = CType(fvPersonalParticulars.FindControl("txtRegBlock"), TextBox)
                txtBuilding = CType(fvPersonalParticulars.FindControl("txtRegEAddress"), TextBox)
                hfAddressCode = CType(fvPersonalParticulars.FindControl("hfRegAddressCode"), HiddenField)
                ddlDistrict = CType(fvPersonalParticulars.FindControl("ddlRegDistrict"), DropDownList)
                rboArea = CType(fvPersonalParticulars.FindControl("rboRegArea"), RadioButtonList)
                imgBuildingAlert = CType(fvPersonalParticulars.FindControl("imgEAddressAlert"), Image)
                ibtnAddressSearch = CType(fvPersonalParticulars.FindControl("ibtnSearchSpAddress"), ImageButton)

            ElseIf fvPersonalParticulars.CurrentMode = FormViewMode.Edit Then
                txtBlock = CType(fvPersonalParticulars.FindControl("txtSPEditBlock"), TextBox)
                txtBuilding = CType(fvPersonalParticulars.FindControl("txtSPEditBuilding"), TextBox)
                hfAddressCode = CType(fvPersonalParticulars.FindControl("hfSPEditAddressCode"), HiddenField)
                ddlDistrict = CType(fvPersonalParticulars.FindControl("ddlSPEditDistrict"), DropDownList)
                rboArea = CType(fvPersonalParticulars.FindControl("rboSPEditArea"), RadioButtonList)
                imgBuildingAlert = CType(fvPersonalParticulars.FindControl("imgSPEditBuildingAlert"), Image)
                ibtnAddressSearch = CType(fvPersonalParticulars.FindControl("ibtnSearchSpEditAddress"), ImageButton)
            End If
        ElseIf Me.TabContainer1.ActiveTabIndex = 1 Then
            'Tab "Medical Organization"
            If Not hfGridviewIndex.Value.Equals(String.Empty) Then
                If gvMOInfo.ShowFooter Then
                    'Add MO 
                    txtBlock = gvMOInfo.FooterRow.FindControl("txtAddMOBlock")
                    txtBuilding = gvMOInfo.FooterRow.FindControl("txtAddMOBuilding")
                    hfAddressCode = gvMOInfo.FooterRow.FindControl("hfAddMOAddressCode")
                    ddlDistrict = gvMOInfo.FooterRow.FindControl("ddlAddMODistrict")
                    rboArea = gvMOInfo.FooterRow.FindControl("rbAddMOArea")
                    imgBuildingAlert = gvMOInfo.FooterRow.FindControl("imgAddMOBuildingAlert")
                    ibtnAddressSearch = gvMOInfo.FooterRow.FindControl("ibtnAddMOSearchAddress")
                Else
                    'Edit MO
                    txtBlock = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtEditMOBlock")
                    txtBuilding = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtEditMOBuilding")
                    hfAddressCode = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("hfEditMOAddressCode")
                    ddlDistrict = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlEditMODistrict")
                    rboArea = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("rbEditMOArea")
                    imgBuildingAlert = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("imgEditMOBuildingAlert")
                    ibtnAddressSearch = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ibtnEditMOSearchAddress")
                End If

            End If
        ElseIf Me.TabContainer1.ActiveTabIndex = 2 Then
            'Tab "Practice Information"
            If Not hfGridviewIndex.Value.Equals(String.Empty) Then
                If gvPracticeInfo.ShowFooter Then
                    'Add Practice 
                    txtBlock = gvPracticeInfo.FooterRow.FindControl("txtAddPracticeBlock")
                    txtBuilding = gvPracticeInfo.FooterRow.FindControl("txtAddPracticeBuilding")
                    txtBuildingChi = gvPracticeInfo.FooterRow.FindControl("txtAddPracticeBuildingChi")
                    hfAddressCode = gvPracticeInfo.FooterRow.FindControl("hfAddPracticeAddressCode")
                    ddlDistrict = gvPracticeInfo.FooterRow.FindControl("ddlAddPracticeDistrict")
                    rboArea = gvPracticeInfo.FooterRow.FindControl("rbAddPracticeArea")
                    imgBuildingAlert = gvPracticeInfo.FooterRow.FindControl("imgAddPracticeBuildingAlert")
                    ibtnAddressSearch = gvPracticeInfo.FooterRow.FindControl("ibtnAddPracticeSearchAddress")
                Else
                    'Edit Practice
                    txtBlock = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtEditPracticeBlock")
                    txtBuilding = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtEditPracticeBuilding")
                    txtBuildingChi = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtEditPracticeBuildingChi")
                    hfAddressCode = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("hfEditPracticeAddressCode")
                    ddlDistrict = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlEditPracticeDistrict")
                    rboArea = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("rbEditPracticeArea")
                    imgBuildingAlert = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("imgEditPracticeBuildingAlert")
                    ibtnAddressSearch = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ibtnEditPracticeSearchAddress")
                End If

            End If
        End If

        If Not IsNothing(txtBuilding) And Not IsNothing(hfAddressCode) And Not IsNothing(ddlDistrict) And Not IsNothing(rboArea) And Not IsNothing(imgBuildingAlert) Then
            txtBuilding.Text = String.Empty

            If Not IsNothing(txtBuildingChi) Then
                txtBuildingChi.Text = String.Empty
                txtBuildingChi.ReadOnly = False
                txtBuildingChi.BackColor = Nothing
            End If

            hfAddressCode.Value = String.Empty

            bindDistrict(ddlDistrict, String.Empty, False)

            rboArea.ClearSelection()

            txtBlock.ReadOnly = False
            txtBuilding.ReadOnly = False
            'txtBuilding.Enabled = True
            ddlDistrict.Enabled = True
            rboArea.Enabled = True

            txtBlock.BackColor = Nothing
            txtBuilding.BackColor = Nothing

            imgBuildingAlert.Visible = False

            ibtnAddressSearch.Enabled = True
            ibtnAddressSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddressSearchSBtn")
        End If
    End Sub

    '

    Private Sub gvStructureAddress_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvStructureAddress.PreRender
        GridViewPreRenderHandler(sender, e, "Address")
    End Sub

    '

    Protected Sub ibtnDialogSelect_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim txtBuilding As TextBox = Nothing
        Dim txtBuildingChi As TextBox = Nothing
        Dim hfAddressCode As HiddenField = Nothing
        Dim ddlDistrict As DropDownList = Nothing
        Dim rboArea As RadioButtonList = Nothing
        Dim txtBlock As TextBox = Nothing
        Dim ibtnAddressSearch As ImageButton = Nothing

        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00106, "Service Provider - Dialog Address Select")
        ' CRE11-021 log the missed essential information [End]

        If Me.TabContainer1.ActiveTabIndex = 0 Then
            'Tab "Personal Particulars" is active
            If fvPersonalParticulars.CurrentMode = FormViewMode.Insert Then
                txtBlock = CType(fvPersonalParticulars.FindControl("txtRegBlock"), TextBox)
                txtBuilding = CType(fvPersonalParticulars.FindControl("txtRegEAddress"), TextBox)
                hfAddressCode = CType(fvPersonalParticulars.FindControl("hfRegAddressCode"), HiddenField)
                ddlDistrict = CType(fvPersonalParticulars.FindControl("ddlRegDistrict"), DropDownList)
                rboArea = CType(fvPersonalParticulars.FindControl("rboRegArea"), RadioButtonList)
                ibtnAddressSearch = CType(fvPersonalParticulars.FindControl("ibtnSearchSpAddress"), ImageButton)


            ElseIf fvPersonalParticulars.CurrentMode = FormViewMode.Edit Then
                txtBlock = CType(fvPersonalParticulars.FindControl("txtSPEditBlock"), TextBox)
                txtBuilding = CType(fvPersonalParticulars.FindControl("txtSPEditBuilding"), TextBox)
                hfAddressCode = CType(fvPersonalParticulars.FindControl("hfSPEditAddressCode"), HiddenField)
                ddlDistrict = CType(fvPersonalParticulars.FindControl("ddlSPEditDistrict"), DropDownList)
                rboArea = CType(fvPersonalParticulars.FindControl("rboSPEditArea"), RadioButtonList)
                ibtnAddressSearch = CType(fvPersonalParticulars.FindControl("ibtnSearchSpEditAddress"), ImageButton)
            End If

        ElseIf Me.TabContainer1.ActiveTabIndex = 1 Then
            'Tab "Medical Organization" is active
            'Tab "Medical Organization Information"
            If Not hfGridviewIndex.Value.Equals(String.Empty) Then
                If Me.gvMOInfo.ShowFooter Then
                    'Add Practice 
                    txtBlock = gvMOInfo.FooterRow.FindControl("txtAddMOBlock")
                    txtBuilding = gvMOInfo.FooterRow.FindControl("txtAddMOBuilding")
                    hfAddressCode = gvMOInfo.FooterRow.FindControl("hfAddMOAddressCode")
                    ddlDistrict = gvMOInfo.FooterRow.FindControl("ddlAddMODistrict")
                    rboArea = gvMOInfo.FooterRow.FindControl("rbAddMOArea")
                    ibtnAddressSearch = gvMOInfo.FooterRow.FindControl("ibtnAddMOSearchAddress")
                Else
                    'Edit Practice
                    txtBlock = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtEditMOBlock")
                    txtBuilding = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtEditMOBuilding")
                    hfAddressCode = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("hfEditMOAddressCode")
                    ddlDistrict = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlEditMODistrict")
                    rboArea = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("rbEditMOArea")
                    ibtnAddressSearch = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ibtnEditMOSearchAddress")
                End If

            End If
        ElseIf Me.TabContainer1.ActiveTabIndex = 2 Then
            'Tab "Practice Infomration" is active
            'Tab "Practice Information"
            If Not hfGridviewIndex.Value.Equals(String.Empty) Then
                If gvPracticeInfo.ShowFooter Then
                    'Add Practice 
                    txtBlock = gvPracticeInfo.FooterRow.FindControl("txtAddPracticeBlock")
                    txtBuilding = gvPracticeInfo.FooterRow.FindControl("txtAddPracticeBuilding")
                    txtBuildingChi = gvPracticeInfo.FooterRow.FindControl("txtAddPracticeBuildingChi")
                    hfAddressCode = gvPracticeInfo.FooterRow.FindControl("hfAddPracticeAddressCode")
                    ddlDistrict = gvPracticeInfo.FooterRow.FindControl("ddlAddPracticeDistrict")
                    rboArea = gvPracticeInfo.FooterRow.FindControl("rbAddPracticeArea")
                    ibtnAddressSearch = gvPracticeInfo.FooterRow.FindControl("ibtnAddPracticeSearchAddress")
                Else
                    'Edit Practice
                    txtBlock = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtEditPracticeBlock")
                    txtBuilding = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtEditPracticeBuilding")
                    txtBuildingChi = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("txtEditPracticeBuildingChi")
                    hfAddressCode = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("hfEditPracticeAddressCode")
                    ddlDistrict = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlEditPracticeDistrict")
                    rboArea = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("rbEditPracticeArea")
                    ibtnAddressSearch = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ibtnEditPracticeSearchAddress")
                End If

            End If
        End If

        If Not Validator.IsEmpty(hfSelectedAddressEng.Value.Trim) AndAlso Not Validator.IsEmpty(hfSelectedAddressRecordID.Value.Trim) AndAlso _
              Not Validator.IsEmpty(hfSelectedAddressDistrictCode.Value.Trim) AndAlso Not Validator.IsEmpty(hfSelectedAddressAreaCode.Value.Trim) Then
            If Not IsNothing(txtBuilding) And Not IsNothing(hfAddressCode) And Not IsNothing(ddlDistrict) And Not IsNothing(rboArea) Then
                txtBuilding.Text = hfSelectedAddressEng.Value
                hfAddressCode.Value = hfSelectedAddressRecordID.Value
                ddlDistrict.SelectedValue = hfSelectedAddressDistrictCode.Value
                rboArea.SelectedValue = hfSelectedAddressAreaCode.Value

                txtBlock.Text = String.Empty

                txtBlock.ReadOnly = True
                txtBuilding.ReadOnly = True
                ddlDistrict.Enabled = False
                rboArea.Enabled = False

                txtBlock.BackColor = Drawing.Color.WhiteSmoke
                txtBuilding.BackColor = Drawing.Color.WhiteSmoke

                If Not IsNothing(txtBuildingChi) Then
                    txtBuildingChi.Text = hfSelectedAddressChi.Value
                    txtBuildingChi.ReadOnly = True
                    txtBuildingChi.BackColor = Drawing.Color.WhiteSmoke
                End If

                ibtnAddressSearch.Enabled = False
                ibtnAddressSearch.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddressSearchDisableSBtn")

                hfSelectedAddressEng.Value = String.Empty
                hfSelectedAddressChi.Value = String.Empty
                hfSelectedAddressRecordID.Value = String.Empty
                hfSelectedAddressDistrictCode.Value = String.Empty
                hfSelectedAddressAreaCode.Value = String.Empty


                ViewState("SortDirection_" & Me.gvStructureAddress.ID) = Nothing
                ViewState("SortExpression_" & Me.gvStructureAddress.ID) = Nothing
                Session("Address") = Nothing

            End If
        End If

        ModalPopupExtender.Hide()

    End Sub

    Protected Sub ibtnDialogClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00107, "Service Provider - Dialog Address Close")
        ' CRE11-021 log the missed essential information [End]

        ViewState("SortDirection_" & Me.gvStructureAddress.ID) = Nothing
        ViewState("SortExpression_" & Me.gvStructureAddress.ID) = Nothing
        Session("Address") = Nothing
        ModalPopupExtender.Hide()
    End Sub

    Private Sub gvStructureAddress_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvStructureAddress.PageIndexChanging
        Me.GridViewPageIndexChangingHandler(sender, e, "Address")
        ModalPopupExtender.Show()
    End Sub

    Private Sub gvStructureAddress_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvStructureAddress.Sorting
        Me.GridViewSortingHandler(sender, e, "Address")
        ModalPopupExtender.Show()

    End Sub

    Private Sub gvStructureAddress_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvStructureAddress.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim lblAddressEng As Label = e.Row.FindControl("lblAddressEng")
            Dim hfRecordID As HiddenField = e.Row.FindControl("hfRecordID")
            Dim hfDistrictCode As HiddenField = e.Row.FindControl("hfDistrictCode")
            Dim hfAreaCode As HiddenField = e.Row.FindControl("hfAreaCode")
            Dim lblAddressType As Label = e.Row.FindControl("lblAddressType")

            Dim lblAddressChi As Label = e.Row.FindControl("lblAddressChi")

            Dim i As Integer = CInt(hfRecordID.Value.Trim)

            If i >= intResidentialAddressFrom And i <= intResidentialAddressTo Then
                lblAddressType.Text = Me.GetGlobalResourceObject("Text", "Residential")
            ElseIf i >= intBusinessAddressFrom And i <= intBusinessAddressTo Then
                lblAddressType.Text = Me.GetGlobalResourceObject("Text", "Business")
            Else
                lblAddressType.Text = Me.GetGlobalResourceObject("Text", "Others")
            End If


            e.Row.Style.Add("cursor", "pointer")
            e.Row.Attributes.Add("onmouseover", "this.style.backgroundColor='#ECF9FF'")
            If e.Row.RowState = DataControlRowState.Alternate Then
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF'")
            Else
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor='#F0EEF7'")
            End If

            e.Row.Cells(1).Attributes.Add("onclick", "javascript:getSelectedAddressbyID(this,'" & lblAddressEng.ClientID & "','" & lblAddressChi.ClientID & "','" & hfRecordID.Value.Trim & "','" & hfDistrictCode.Value.Trim & "', '" & hfAreaCode.Value.Trim & "');")
            e.Row.Cells(1).Attributes.Add("ondblclick", "javascript:doubleClickbyID(this,'" & lblAddressEng.ClientID & "','" & lblAddressChi.ClientID & "','" & hfRecordID.Value.Trim & "','" & hfDistrictCode.Value.Trim & "', '" & hfAreaCode.Value.Trim & "');")
            'e.Row.Cells(1).Attributes.Add("onclick", "javascript:getSelectedAddress(this,'" & lblAddressEng.Text.Trim & "','" & hfRecordID.Value.Trim & "','" & hfDistrictCode.Value.Trim & "', '" & hfAreaCode.Value.Trim & "');")
            e.Row.Cells(2).Attributes.Add("onclick", "javascript:getSelectedAddressbyID(this,'" & lblAddressEng.ClientID & "','" & lblAddressChi.ClientID & "','" & hfRecordID.Value.Trim & "','" & hfDistrictCode.Value.Trim & "', '" & hfAreaCode.Value.Trim & "');")
            e.Row.Cells(2).Attributes.Add("ondblclick", "javascript:doubleClickbyID(this,'" & lblAddressEng.ClientID & "','" & lblAddressChi.ClientID & "','" & hfRecordID.Value.Trim & "','" & hfDistrictCode.Value.Trim & "', '" & hfAreaCode.Value.Trim & "');")
        End If
    End Sub

#End Region

#Region "Common - Original Record"

    Protected Sub ibtnExistingSPProfile_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtSP As ServiceProviderModel
        'Dim udtToken As New TokenModel

        Try
            udtSP = udtSPProfileBLL.GetServiceProviderPermanentProfileNoSession(Me.hfERN.Value.Trim)
            'udtToken = udtSPProfileBLL.GetTokenModelBySPID(udtSP.SPID)
            udcExistingSPProfile.buildSpProfileObject(udtSP, TableLocation.Permanent)
            udcExistingSPProfile.DisplayRecordStatus(False, TableLocation.Permanent)
            Me.ModalPopupExtenderSPProfile.Show()

            'If Me.TabContainer1.ActiveTabIndex = 4 Then
            '    BindServiceFeeGridview()
            'End If
        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Protected Sub ibtnExistingSPProfileClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        udtAuditLogEntry.WriteLog(LogID.LOG00114, "Existing Service Provider Profile - Close Click")
        ' CRE11-021 log the missed essential information [End]

        ModalPopupExtenderSPProfile.Hide()

        'If Me.TabContainer1.ActiveTabIndex = 4 Then
        '    BindServiceFeeGridview()
        'End If
    End Sub

#End Region

#Region "Tab - Personal Particulars"

    Protected Sub ibtnSPEdit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        udtAuditLogEntry.WriteLog(LogID.LOG00104, "Service Provider - Edit Click")
        ' CRE11-021 log the missed essential information [End]

        'udtSPProfileBLL.BindDataToControlForDataEntry(hfERN.Value.Trim, fvPersonalParticulars, gvPracticeInfo, gvMOInfo, gvBankInfo)
        Me.BindSPProfile()
        fvPersonalParticulars.ChangeMode(FormViewMode.Edit)
        fvPersonalParticulars.DataBind()
    End Sub

    Protected Sub ibtnSPEditCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Common.Component.LogID.LOG00008) ''Begin Writing Audit Log
        'udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)
        'If udtServiceProviderBLL.Exist Then
        '    udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
        'Else
        '    udtAuditLogEntry.AddDescripton("SPID", "")
        'End If
        'udtAuditLogEntry.WriteStartLog("Cancel save personal:")
        'udtSPProfileBLL.BindDataToControlForDataEntry(hfERN.Value.Trim, fvPersonalParticulars, gvPracticeInfo, gvMOInfo, gvPracticeInfo)
        Me.msgBox.Visible = False
        Me.BindSPProfile()
        fvPersonalParticulars.ChangeMode(FormViewMode.ReadOnly)
        fvPersonalParticulars.DataBind()

        'udtAuditLogEntry.WriteStartLog("Cancel save personal Completed.")
    End Sub

    Protected Sub ibtnSPEditSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If fvPersonalParticulars.CurrentMode = FormViewMode.Edit Then

            Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log

            Dim txtSPEditSurname As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditSurname"), TextBox)
            Dim txtSPEditOthername As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditOthername"), TextBox)
            Dim txtSPEditCname As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditCname"), TextBox)
            Dim txtSPEditHKID As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditHKID"), TextBox)
            Dim hfSPEditHKID As HiddenField = CType(fvPersonalParticulars.FindControl("hfSPEditHKID"), HiddenField)
            Dim txtSPEditRoom As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditRoom"), TextBox)
            Dim txtSPEditFloor As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditFloor"), TextBox)
            Dim txtSPEditBlock As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditBlock"), TextBox)
            Dim txtSPEditBuilding As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditBuilding"), TextBox)
            Dim hfSPEditAddressCode As HiddenField = CType(fvPersonalParticulars.FindControl("hfSPEditAddressCode"), HiddenField)
            Dim ddlSPEditDistrict As DropDownList = CType(fvPersonalParticulars.FindControl("ddlSPEditDistrict"), DropDownList)
            Dim rboSPEditArea As RadioButtonList = CType(fvPersonalParticulars.FindControl("rboSPEditArea"), RadioButtonList)
            Dim txtSPEditEmail As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditEmail"), TextBox)
            Dim txtSPEditConfirmEmail As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditConfirmEmail"), TextBox)
            Dim txtSPEditContactNo As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditContactNo"), TextBox)
            Dim txtSPEditFaxNo As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditFaxNo"), TextBox)

            Dim imgSPEditEnameAlert As Image = CType(fvPersonalParticulars.FindControl("imgSPEditEnameAlert"), Image)
            Dim imgSPEditHKIdAlert As Image = CType(fvPersonalParticulars.FindControl("imgSPEditHKIdAlert"), Image)
            Dim imgSPEditBuildingAlert As Image = CType(fvPersonalParticulars.FindControl("imgSPEditBuildingAlert"), Image)
            Dim imgSPEditDistrictAlert As Image = CType(fvPersonalParticulars.FindControl("imgSPEditDistrictAlert"), Image)
            Dim imgSPEditAreaAlert As Image = CType(fvPersonalParticulars.FindControl("imgSPEditAreaAlert"), Image)
            Dim imgSPEditEmailAlert As Image = CType(fvPersonalParticulars.FindControl("imgSPEditEmailAlert"), Image)
            Dim imgSPEditConfirmEmailAlert As Image = CType(fvPersonalParticulars.FindControl("imgSPEditConfirmEmailAlert"), Image)
            Dim imgSPEditContactNoAlert As Image = CType(fvPersonalParticulars.FindControl("imgSPEditContactNoAlert"), Image)

            'initialize the "visible" of  error icon to FALSE
            imgSPEditEnameAlert.Visible = False
            imgSPEditHKIdAlert.Visible = False
            imgSPEditBuildingAlert.Visible = False
            imgSPEditDistrictAlert.Visible = False
            imgSPEditAreaAlert.Visible = False
            imgSPEditEmailAlert.Visible = False
            imgSPEditConfirmEmailAlert.Visible = False
            imgSPEditContactNoAlert.Visible = False

            msgBox.Visible = False

            Dim blnAbleToUpdate As Boolean = False

            udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)
            If udtServiceProviderBLL.Exist Then
                udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
            Else
                udtAuditLogEntry.AddDescripton("SPID", "")
            End If
            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00008, "Save personal")

            If txtSPEditHKID.Visible Then
                If Not UCase(txtSPEditHKID.Text.Trim).Equals(udtFormatter.formatHKID(hfSPEditHKID.Value.Trim, False)) Then

                    If udtSPProfileBLL.IsHKIDExistingInServiceProviderStagingPermanentByHKID(UCase(txtSPEditHKID.Text.Trim)) Then
                        blnAbleToUpdate = False
                        SM = New Common.ComObject.SystemMessage("010101", "E", "00001")
                        msgBox.AddMessage(SM)
                        'msgBox.BuildMessageBox("ValidationFail")
                        msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00010, "Save personal Fail.")
                    Else
                        blnAbleToUpdate = True
                    End If
                Else
                    blnAbleToUpdate = True
                End If
            Else
                blnAbleToUpdate = True
            End If


            If blnAbleToUpdate Then
                'Check Name (in English)
                SM = Validator.chkEngName(UCase(txtSPEditSurname.Text.Trim), UCase(txtSPEditOthername.Text.Trim))
                If Not SM Is Nothing Then
                    imgSPEditEnameAlert.Visible = True
                    msgBox.AddMessage(SM)
                End If

                'Check HKID
                SM = Validator.chkHKID(UCase(txtSPEditHKID.Text.Trim))
                If Not SM Is Nothing Then
                    imgSPEditHKIdAlert.Visible = True
                    msgBox.AddMessage(SM)
                End If

                'Check Address
                SM = Validator.chkAddress(txtSPEditBuilding.Text.Trim, ddlSPEditDistrict.SelectedValue, rboSPEditArea.SelectedValue)
                If Not SM Is Nothing Then
                    If txtSPEditBuilding.Text.Trim.Equals(String.Empty) Then
                        imgSPEditBuildingAlert.Visible = True
                    End If

                    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                    'If ddlSPEditDistrict.SelectedValue.Trim.Equals(String.Empty) OrElse _
                    '   ddlSPEditDistrict.SelectedValue.Trim.Equals(".H") OrElse _
                    '   ddlSPEditDistrict.SelectedValue.Trim.Equals(".K") OrElse _
                    '   ddlSPEditDistrict.SelectedValue.Trim.Equals(".N") Then
                    If ddlSPEditDistrict.SelectedValue.Trim.Equals(String.Empty) OrElse _
                       ddlSPEditDistrict.SelectedValue.Trim.StartsWith(".") Then
                        'CRE13-019-02 Extend HCVS to China [End][Winnie]
                        imgSPEditDistrictAlert.Visible = True
                    End If

                    If rboSPEditArea.SelectedValue.Trim.Equals(String.Empty) Then
                        imgSPEditAreaAlert.Visible = True
                    End If
                    msgBox.AddMessage(SM)
                End If

                'Check Email Address
                SM = Validator.chkEmailAddress(txtSPEditEmail.Text.Trim)
                If SM Is Nothing Then
                    SM = Validator.chkConfirmEmail(txtSPEditEmail.Text.Trim, txtSPEditConfirmEmail.Text.Trim)
                    If Not SM Is Nothing Then
                        imgSPEditConfirmEmailAlert.Visible = True
                        msgBox.AddMessage(SM)
                    End If
                Else
                    imgSPEditEmailAlert.Visible = True
                    msgBox.AddMessage(SM)
                End If

                'Check ContactNo
                SM = Validator.chkContactNo(txtSPEditContactNo.Text.Trim)
                If Not SM Is Nothing Then
                    imgSPEditContactNoAlert.Visible = True
                    msgBox.AddMessage(SM)
                End If

                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                    'Save SP personal particulars
                    Dim intAddressCode As Nullable(Of Integer)

                    If hfSPEditAddressCode.Value.Trim.Equals(String.Empty) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt(hfSPEditAddressCode.Value.Trim)
                    End If

                    Try
                        'udtSPProfileBLL.SaveSPProfileToSession(hfTableLocation.Value.Trim)
                        Dim udtSP As ServiceProviderModel = New ServiceProviderModel

                        udtServiceProviderBLL.Clone(udtSP, udtServiceProviderBLL.GetSP)

                        With udtSP
                            .EnglishName = udtFormatter.formatEnglishName(txtSPEditSurname.Text.Trim, txtSPEditOthername.Text.Trim)
                            .ChineseName = txtSPEditCname.Text.Trim
                            .HKID = txtSPEditHKID.Text.Trim.Replace("(", String.Empty).Replace(")", String.Empty)

                            If intAddressCode.HasValue Then
                                .SpAddress = New AddressModel(txtSPEditRoom.Text.Trim, txtSPEditFloor.Text.Trim, txtSPEditBlock.Text.Trim, _
                                                            String.Empty, String.Empty, String.Empty, intAddressCode)
                            Else
                                .SpAddress = New AddressModel(txtSPEditRoom.Text.Trim, txtSPEditFloor.Text.Trim, txtSPEditBlock.Text.Trim, _
                                                            txtSPEditBuilding.Text.Trim, String.Empty, ddlSPEditDistrict.SelectedValue.Trim, intAddressCode)
                            End If

                            If Not .SPID.Trim.Equals(String.Empty) Then

                                ' CRE16-018 Display SP tentative email in HCVU [Start][Winnie]
                                ' Compare the email with permanent record instead of staging
                                Dim udtSPOriginal As ServiceProviderModel = Me.ServiceProviderPermanent()

                                'If .Email.Equals(txtSPEditEmail.Text.Trim) Then
                                If udtSPOriginal.Email.Equals(txtSPEditEmail.Text.Trim) Then
                                    .EmailChanged = EmailChanged.Unchanged
                                Else
                                    .EmailChanged = EmailChanged.Changed
                                End If
                                ' CRE16-018 Display SP tentative email in HCVU [End][Winnie]
                            End If

                            .Email = txtSPEditEmail.Text.Trim
                            .Phone = txtSPEditContactNo.Text.Trim
                            .Fax = txtSPEditFaxNo.Text.Trim

                        End With

                        If udtSPProfileBLL.UpdateServiceProviderParticularStaging(udtSP, hfTableLocation.Value) Then
                            udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)
                            If udtServiceProviderBLL.Exist Then
                                udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
                                udtAuditLogEntry.AddDescripton("SP HKIC", udtServiceProviderBLL.GetSP.HKID)
                            Else
                                udtAuditLogEntry.AddDescripton("SPID", "")
                                udtAuditLogEntry.AddDescripton("SP HKIC", "")
                            End If

                            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00009, "Save personal Completed.")

                            If udtSPProfileBLL.GetServiceProviderProfile(hfERN.Value.Trim, TableLocation.Staging) Then
                                hfTableLocation.Value = TableLocation.Staging
                                'udtSPProfileBLL.BindDataToControlForDataEntry(hfERN.Value.Trim, fvPersonalParticulars, gvPracticeInfo, gvMOInfo, gvBankInfo)

                                BindSPProfile()

                            End If

                        End If


                    Catch eSQL As SqlClient.SqlException
                        If eSQL.Number = 50000 Then
                            Dim strmsg As String
                            strmsg = eSQL.Message

                            If Not strmsg.Trim.Equals(String.Empty) Then
                                SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                                msgBox.AddMessage(SM)
                                If msgBox.GetCodeTable.Rows.Count = 0 Then
                                    msgBox.Visible = False
                                Else

                                    msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00010, "Save personal Fail.")
                                End If
                            Else
                                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00010, "Save personal Fail.")
                            End If

                        Else
                            Throw
                        End If

                    Catch ex As Exception
                        Throw
                    End Try

                Else
                    msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00010, "Save personal Fail.")
                End If
            End If
        End If
    End Sub

    Protected Sub ibtnSPSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If fvPersonalParticulars.CurrentMode = FormViewMode.Insert Then
            Dim udtAuditLogEntryAssign As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
            Dim udtAuditLogEntrySave As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log

            Dim blnAbleToInsert As Boolean = False

            Dim txtRegSurname As TextBox = CType(fvPersonalParticulars.FindControl("txtRegSurname"), TextBox)
            Dim txtRegEname As TextBox = CType(fvPersonalParticulars.FindControl("txtRegEname"), TextBox)
            Dim txtRegCname As TextBox = CType(fvPersonalParticulars.FindControl("txtRegCname"), TextBox)
            Dim txtRegHKID As TextBox = CType(fvPersonalParticulars.FindControl("txtRegHKID"), TextBox)
            Dim txtRegRoom As TextBox = CType(fvPersonalParticulars.FindControl("txtRegRoom"), TextBox)
            Dim txtRegFloor As TextBox = CType(fvPersonalParticulars.FindControl("txtRegFloor"), TextBox)
            Dim txtRegBlock As TextBox = CType(fvPersonalParticulars.FindControl("txtRegBlock"), TextBox)
            Dim txtRegEAddress As TextBox = CType(fvPersonalParticulars.FindControl("txtRegEAddress"), TextBox)
            Dim hfRegAddressCode As HiddenField = CType(fvPersonalParticulars.FindControl("hfRegAddressCode"), HiddenField)
            Dim ddlRegDistrict As DropDownList = CType(fvPersonalParticulars.FindControl("ddlRegDistrict"), DropDownList)
            Dim rboRegArea As RadioButtonList = CType(fvPersonalParticulars.FindControl("rboRegArea"), RadioButtonList)
            Dim txtRegEmail As TextBox = CType(fvPersonalParticulars.FindControl("txtRegEmail"), TextBox)
            Dim txtRegConfirmEmail As TextBox = CType(fvPersonalParticulars.FindControl("txtRegConfirmEmail"), TextBox)
            Dim txtRegContactNo As TextBox = CType(fvPersonalParticulars.FindControl("txtRegContactNo"), TextBox)
            Dim txtRegFaxNo As TextBox = CType(fvPersonalParticulars.FindControl("txtRegFaxNo"), TextBox)

            Dim imgEnameAlert As Image = CType(fvPersonalParticulars.FindControl("imgEnameAlert"), Image)
            Dim imgHKIdAlert As Image = CType(fvPersonalParticulars.FindControl("imgHKIdAlert"), Image)
            Dim imgEAddressAlert As Image = CType(fvPersonalParticulars.FindControl("imgEAddressAlert"), Image)
            Dim imgDistrictAlert As Image = CType(fvPersonalParticulars.FindControl("imgDistrictAlert"), Image)
            Dim imgAreaAlert As Image = CType(fvPersonalParticulars.FindControl("imgAreaAlert"), Image)
            Dim imgEmailAlert As Image = CType(fvPersonalParticulars.FindControl("imgEmailAlert"), Image)
            Dim imgConfirmEmailAlert As Image = CType(fvPersonalParticulars.FindControl("imgConfirmEmailAlert"), Image)
            Dim imgContactNoAlert As Image = CType(fvPersonalParticulars.FindControl("imgContactNoAlert"), Image)

            Dim strHKID As String = udtFormatter.formatHKIDInternal(txtRegHKID.Text)

            'initialize the "visible" of  error icon to FALSE
            imgEnameAlert.Visible = False
            imgHKIdAlert.Visible = False
            imgEAddressAlert.Visible = False
            imgDistrictAlert.Visible = False
            imgAreaAlert.Visible = False
            imgEmailAlert.Visible = False
            imgConfirmEmailAlert.Visible = False
            imgContactNoAlert.Visible = False

            msgBox.Visible = False

            udtAuditLogEntryAssign.AddDescripton("SP HKIC", strHKID)
            udtAuditLogEntryAssign.WriteStartLog(Common.Component.LogID.LOG00011, "Assign Enrolment Reference No.", New Common.ComObject.AuditLogInfo(Nothing, strHKID, "", "", "", ""))

            udtAuditLogEntrySave.AddDescripton("ERN", "")
            udtAuditLogEntrySave.AddDescripton("SPID", "")
            udtAuditLogEntrySave.AddDescripton("SP HKIC", strHKID)
            udtAuditLogEntrySave.WriteStartLog(Common.Component.LogID.LOG00008, "Save personal", New Common.ComObject.AuditLogInfo(Nothing, strHKID, "", "", "", ""))

            If udtSPProfileBLL.IsHKIDExistingInServiceProviderStagingPermanentByHKID(txtRegHKID.Text.Trim) Then

                blnAbleToInsert = False
                SM = New Common.ComObject.SystemMessage("010101", "E", "00001")
                msgBox.AddMessage(SM)
                'msgBox.BuildMessageBox("ValidationFail")
                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntryAssign, Common.Component.LogID.LOG00013, "Assign Enrolment Reference No Fail", New Common.ComObject.AuditLogInfo(Nothing, strHKID, "", "", "", ""))
            Else
                blnAbleToInsert = True
            End If

            If blnAbleToInsert Then
                'Check Name (in English)
                SM = Validator.chkEngName(UCase(txtRegSurname.Text.Trim), UCase(txtRegEname.Text.Trim))
                If Not SM Is Nothing Then
                    imgEnameAlert.Visible = True
                    msgBox.AddMessage(SM)
                End If

                'Check HKID
                SM = Validator.chkHKID(UCase(txtRegHKID.Text.Trim))
                If Not SM Is Nothing Then
                    imgHKIdAlert.Visible = True
                    msgBox.AddMessage(SM)
                End If

                'Check Address
                SM = Validator.chkAddress(txtRegEAddress.Text.Trim, ddlRegDistrict.SelectedValue, rboRegArea.SelectedValue)
                If Not SM Is Nothing Then
                    If txtRegEAddress.Text.Trim.Equals(String.Empty) Then
                        imgEAddressAlert.Visible = True
                    End If

                    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                    'If ddlRegDistrict.SelectedValue.Equals(String.Empty) OrElse _
                    '   ddlRegDistrict.SelectedValue.Trim.Equals(".H") OrElse _
                    '   ddlRegDistrict.SelectedValue.Trim.Equals(".K") OrElse _
                    '   ddlRegDistrict.SelectedValue.Trim.Equals(".N") Then

                    If ddlRegDistrict.SelectedValue.Equals(String.Empty) OrElse _
                       ddlRegDistrict.SelectedValue.Trim.StartsWith(".") Then
                        imgDistrictAlert.Visible = True
                    End If
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]

                    If rboRegArea.SelectedValue.Equals(String.Empty) Then
                        imgAreaAlert.Visible = True
                    End If

                    msgBox.AddMessage(SM)
                End If

                'Check Email Address
                SM = Validator.chkEmailAddress(txtRegEmail.Text.Trim)
                If SM Is Nothing Then
                    SM = Validator.chkConfirmEmail(txtRegEmail.Text.Trim, txtRegConfirmEmail.Text.Trim)
                    If Not SM Is Nothing Then
                        imgConfirmEmailAlert.Visible = True
                        msgBox.AddMessage(SM)
                    End If
                Else
                    imgEmailAlert.Visible = True
                    msgBox.AddMessage(SM)
                End If

                'Check ContactNo
                SM = Validator.chkContactNo(txtRegContactNo.Text.Trim)
                If Not SM Is Nothing Then
                    imgContactNoAlert.Visible = True
                    msgBox.AddMessage(SM)
                End If

                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                    'Save SP personal particulars
                    Dim intAddressCode As Nullable(Of Integer)

                    If hfRegAddressCode.Value.Trim.Equals(String.Empty) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt(hfRegAddressCode.Value.Trim)
                    End If

                    Try
                        Dim strERN As String

                        Dim udtSP As ServiceProviderModel = New ServiceProviderModel

                        With udtSP
                            .EnglishName = udtFormatter.formatEnglishName(txtRegSurname.Text.Trim, txtRegEname.Text.Trim)
                            .ChineseName = txtRegCname.Text.Trim
                            .HKID = txtRegHKID.Text.Trim.Replace("(", String.Empty).Replace(")", String.Empty)

                            If intAddressCode.HasValue Then
                                .SpAddress = New AddressModel(txtRegRoom.Text.Trim, txtRegFloor.Text.Trim, txtRegBlock.Text.Trim, _
                                                            String.Empty, String.Empty, String.Empty, intAddressCode)
                            Else
                                .SpAddress = New AddressModel(txtRegRoom.Text.Trim, txtRegFloor.Text.Trim, txtRegBlock.Text.Trim, _
                                                            txtRegEAddress.Text.Trim, String.Empty, ddlRegDistrict.SelectedValue.Trim, intAddressCode)
                            End If

                            .Email = txtRegEmail.Text.Trim
                            .Phone = txtRegContactNo.Text.Trim
                            .Fax = txtRegFaxNo.Text.Trim
                        End With

                        strERN = udtSPProfileBLL.AddServiceProviderParticularsToStaging(udtSP)

                        If Not strERN.Equals(String.Empty) Then
                            udtAuditLogEntryAssign.AddDescripton("ERN", strERN)
                            'udtAuditLogEntryAssign.AddDescripton("SPID", "")
                            udtAuditLogEntryAssign.WriteEndLog(Common.Component.LogID.LOG00012, "Assign enrolment reference No Completed", New Common.ComObject.AuditLogInfo(Nothing, strHKID, "", "", "", ""))

                            hfERN.Value = strERN
                            udtAuditLogEntrySave.AddDescripton("ERN", strERN)
                            'udtAuditLogEntrySave.AddDescripton("SPID", "")

                            udtAuditLogEntrySave.WriteEndLog(Common.Component.LogID.LOG00009, "Save personal Completed", New Common.ComObject.AuditLogInfo(Nothing, strHKID, "", "", "", ""))

                            If udtSPProfileBLL.GetServiceProviderProfile(strERN, TableLocation.Staging) Then
                                hfTableLocation.Value = TableLocation.Staging

                                BindSPProfile()

                                Page_Load(sender, e)

                                LoadPracticeOthersInfo()
                                SetPracticeAdditionalInfo(False)


                            End If
                        End If
                    Catch ex As Exception
                        Throw
                    End Try

                Else
                    udtAuditLogEntryAssign.WriteEndLog(Common.Component.LogID.LOG00013, "Assign Enrolment Reference No Fail", New Common.ComObject.AuditLogInfo(Nothing, strHKID, "", "", "", ""))
                    msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntrySave, Common.Component.LogID.LOG00010, "Save personal Fail", New Common.ComObject.AuditLogInfo(Nothing, strHKID, "", "", "", ""))
                End If
            End If


        End If
    End Sub

    Private Sub fvPersonalParticulars_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles fvPersonalParticulars.DataBound
        If fvPersonalParticulars.CurrentMode = FormViewMode.Insert Then
            Dim ddlRegDistrict As DropDownList = CType(fvPersonalParticulars.FindControl("ddlRegDistrict"), DropDownList)
            Dim rboRegArea As RadioButtonList = CType(fvPersonalParticulars.FindControl("rboRegArea"), RadioButtonList)

            rboRegArea.DataSource = udtSPProfileBLL.GetArea.Values
            rboRegArea.DataValueField = "Area_ID"
            rboRegArea.DataTextField = "Area_Name"
            rboRegArea.DataBind()

            bindDistrict(ddlRegDistrict, String.Empty, False)

        ElseIf fvPersonalParticulars.CurrentMode = FormViewMode.Edit Then
            Dim txtSPEditSurname As TextBox = CType(Me.fvPersonalParticulars.FindControl("txtSPEditSurname"), TextBox)
            Dim txtSPEditOthername As TextBox = CType(Me.fvPersonalParticulars.FindControl("txtSPEditOthername"), TextBox)

            Dim hfSPEditEName As HiddenField = CType(Me.fvPersonalParticulars.FindControl("hfSPEditEName"), HiddenField)

            Dim ddlSPEditDistrict As DropDownList = CType(fvPersonalParticulars.FindControl("ddlSPEditDistrict"), DropDownList)
            Dim rboSPEditArea As RadioButtonList = CType(fvPersonalParticulars.FindControl("rboSPEditArea"), RadioButtonList)

            Dim hfSPEditDistrict As HiddenField = CType(fvPersonalParticulars.FindControl("hfSPEditDistrict"), HiddenField)
            Dim hfSPEditArea As HiddenField = CType(fvPersonalParticulars.FindControl("hfSPEditArea"), HiddenField)

            Dim hfSPEditAddressCode As HiddenField = CType(fvPersonalParticulars.FindControl("hfSPEditAddressCode"), HiddenField)
            Dim txtSPEditBuilding As TextBox = CType(fvPersonalParticulars.FindControl("txtSPEditBuilding"), TextBox)

            Dim lblSPEditSubmitBy As Label = CType(Me.fvPersonalParticulars.FindControl("lblSPEditSubmitBy"), Label)

            Dim lblSPEditHKID As Label = CType(Me.fvPersonalParticulars.FindControl("lblSPEditHKID"), Label)
            Dim txtSPEditHKID As TextBox = CType(Me.fvPersonalParticulars.FindControl("txtSPEditHKID"), TextBox)

            Dim lblSPEditHKIDTip As Label = CType(Me.fvPersonalParticulars.FindControl("lblSPEditHKIDTip"), Label)
            Dim lblSPEditHKIDEG As Label = CType(Me.fvPersonalParticulars.FindControl("lblSPEditHKIDEG"), Label)

            Dim txtSPEditBlock As TextBox = CType(Me.fvPersonalParticulars.FindControl("txtSPEditBlock"), TextBox)

            Dim ibtnSearchSpEditAddress As ImageButton = CType(Me.fvPersonalParticulars.FindControl("ibtnSearchSpEditAddress"), ImageButton)

            If Me.hfTableLocation.Value.Trim.Equals(TableLocation.Permanent) Then
                lblSPEditHKID.Visible = True
                lblSPEditHKID.Text = udtFormatter.formatHKID(lblSPEditHKID.Text.Trim, False)
                txtSPEditHKID.Visible = False
                lblSPEditHKIDTip.Visible = False
                lblSPEditHKIDEG.Visible = False
            Else
                If udtServiceProviderBLL.Exist Then
                    If udtServiceProviderBLL.GetSP.SPID.Equals(String.Empty) Then
                        lblSPEditHKID.Visible = False
                        txtSPEditHKID.Visible = True
                        lblSPEditHKIDTip.Visible = True
                        lblSPEditHKIDEG.Visible = True
                        txtSPEditHKID.Text = udtFormatter.formatHKID(txtSPEditHKID.Text.Trim, False)
                    Else
                        lblSPEditHKID.Visible = True
                        lblSPEditHKID.Text = udtFormatter.formatHKID(lblSPEditHKID.Text.Trim, False)
                        txtSPEditHKID.Visible = False
                        lblSPEditHKIDTip.Visible = False
                        lblSPEditHKIDEG.Visible = False
                    End If
                End If

            End If

            udtFormatter.seperateEName(hfSPEditEName.Value.Trim, txtSPEditSurname.Text, txtSPEditOthername.Text)

            rboSPEditArea.DataSource = udtSPProfileBLL.GetArea.Values 'AreaBLL.GetAreaList.Values
            rboSPEditArea.DataValueField = "Area_ID"
            rboSPEditArea.DataTextField = "Area_Name"
            rboSPEditArea.DataBind()
            bindDistrict(ddlSPEditDistrict, String.Empty, False)
            If hfSPEditArea.Value.Equals(String.Empty) Then

            Else
                rboSPEditArea.SelectedValue = hfSPEditArea.Value.Trim

                'bindDistrict(ddlSPEditDistrict, rboSPEditArea.SelectedValue, False)
                ddlSPEditDistrict.SelectedValue = hfSPEditDistrict.Value.Trim
            End If

            If Not hfSPEditAddressCode.Value.Trim.Equals(String.Empty) Then
                txtSPEditBlock.ReadOnly = True
                txtSPEditBuilding.ReadOnly = True
                'txtSPEditBuilding.Enabled = False
                ddlSPEditDistrict.Enabled = False
                rboSPEditArea.Enabled = False

                txtSPEditBlock.BackColor = Drawing.Color.WhiteSmoke
                txtSPEditBuilding.BackColor = Drawing.Color.WhiteSmoke
                ibtnSearchSpEditAddress.Enabled = False
                ibtnSearchSpEditAddress.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddressSearchDisableSBtn")
            Else
                txtSPEditBlock.BackColor = Nothing
                txtSPEditBuilding.BackColor = Nothing
                ibtnSearchSpEditAddress.Enabled = True
                ibtnSearchSpEditAddress.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddressSearchSBtn")

            End If


            If lblSPEditSubmitBy.Text.Trim.Equals(SubmitChannel.Paper) Then
                lblSPEditSubmitBy.Text = "Paper"
            ElseIf lblSPEditSubmitBy.Text.Equals(SubmitChannel.Electronic) Then
                lblSPEditSubmitBy.Text = "Electronic"
            End If

        ElseIf Me.fvPersonalParticulars.CurrentMode = FormViewMode.ReadOnly Then
            Dim lblSPSubmitBy As Label = CType(Me.fvPersonalParticulars.FindControl("lblSPSubmitBy"), Label)
            Dim lblSPHKID As Label = CType(Me.fvPersonalParticulars.FindControl("lblSPHKID"), Label)
            Dim lblSPFaxNo As Label = CType(Me.fvPersonalParticulars.FindControl("lblSPFaxNo"), Label)
            Dim lblSPStatus As Label = CType(Me.fvPersonalParticulars.FindControl("lblSPStatus"), Label)

            Select Case Me.hfTableLocation.Value.Trim
                Case TableLocation.Permanent
                    lblSPStatus.Text = "Enrolled"
                Case TableLocation.Staging
                    lblSPStatus.Text = "Processing"
                Case TableLocation.Enrolment
                    lblSPStatus.Text = "Unprocessed"
            End Select

            If lblSPSubmitBy.Text.Trim.Equals(SubmitChannel.Paper) Then
                lblSPSubmitBy.Text = "Paper"
            ElseIf lblSPSubmitBy.Text.Equals(SubmitChannel.Electronic) Then
                lblSPSubmitBy.Text = "Electronic"
            End If

            If lblSPFaxNo.Text.Trim.Equals(String.Empty) Then
                'lblSPFaxNo.Text = Me.GetGlobalResourceObject("Text", "N/A")
            End If

            lblSPHKID.Text = udtFormatter.formatHKID(lblSPHKID.Text.Trim, False)
        End If

        If udtServiceProviderBLL.Exist Then
            Me.ApplyPersonalChangeIndicator()
        End If


    End Sub

#End Region

#Region "Tab - Medical Organization Information"

    Private Sub gvMOInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMOInfo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If (e.Row.RowState And DataControlRowState.Edit) > 0 Then
                'Edit Mode
                Dim hfEditMOAddressCode As HiddenField = CType(e.Row.FindControl("hfEditMOAddressCode"), HiddenField)
                Dim txtEditMOBuilding As TextBox = CType(e.Row.FindControl("txtEditMOBuilding"), TextBox)

                Dim ddlEditMODistrict As DropDownList = CType(e.Row.FindControl("ddlEditMODistrict"), DropDownList)
                Dim hfEditMODistrict As HiddenField = CType(e.Row.FindControl("hfEditMODistrict"), HiddenField)

                Dim rbEditMOArea As RadioButtonList = CType(e.Row.FindControl("rbEditMOArea"), RadioButtonList)
                Dim hfEditMOArea As HiddenField = CType(e.Row.FindControl("hfEditMOArea"), HiddenField)

                Dim rboEditMORelation As RadioButtonList = e.Row.FindControl("rboEditMORelation")
                Dim hfEditMORelation As HiddenField = e.Row.FindControl("hfEditMORelation")
                Dim txtEditMORelationRemark As TextBox = e.Row.FindControl("txtEditMORelationRemark")

                Dim txtEditMOBlock As TextBox = e.Row.FindControl("txtEditMOBlock")

                Dim ibtnEditMOSearchAddress As ImageButton = e.Row.FindControl("ibtnEditMOSearchAddress")
                Dim ibtnEditClearMOSearchAddress As ImageButton = e.Row.FindControl("ibtnEditClearMOSearchAddress")

                Dim lblEditMOName As Label = CType(e.Row.FindControl("lblEditMOName"), Label)
                Dim txtEditMOName As TextBox = CType(e.Row.FindControl("txtEditMOName"), TextBox)

                Dim lblEditMONameChi As Label = CType(e.Row.FindControl("lblEditMONameChi"), Label)
                Dim txtEditMONameChi As TextBox = CType(e.Row.FindControl("txtEditMONameChi"), TextBox)

                Dim lblEditMOBRCode As Label = CType(e.Row.FindControl("lblEditMOBRCode"), Label)
                Dim txtEditMOBRCode As TextBox = CType(e.Row.FindControl("txtEditMOBRCode"), TextBox)

                Dim hfEditMOStatus As HiddenField = CType(e.Row.FindControl("hfEditMOStatus"), HiddenField)

                Dim lblEditMORelationship As Label = CType(e.Row.FindControl("lblEditMORelationship"), Label)
                Dim lblEditMORelationshipRemark As Label = CType(e.Row.FindControl("lblEditMORelationshipRemark"), Label)
                Dim pnlEditMORelationship As Panel = CType(e.Row.FindControl("pnlEditMORelationship"), Panel)

                If hfTableLocation.Value.Trim.Equals(TableLocation.Permanent) OrElse _
                   hfEditMOStatus.Value.Trim.Equals(MedicalOrganizationStagingStatus.Existing) OrElse _
                   hfEditMOStatus.Value.Trim.Equals(MedicalOrganizationStagingStatus.Update) OrElse _
                   hfEditMOStatus.Value.Trim.Equals(MedicalOrganizationStagingStatus.Delisted) Then

                    lblEditMOName.Visible = True
                    txtEditMOName.Visible = False

                    lblEditMONameChi.Visible = True
                    txtEditMONameChi.Visible = False

                    lblEditMOBRCode.Visible = True
                    txtEditMOBRCode.Visible = False

                    pnlEditMORelationship.Visible = False
                    lblEditMORelationship.Visible = True
                    lblEditMORelationshipRemark.Visible = True

                    If lblEditMORelationshipRemark.Text.Trim.Equals(String.Empty) Then
                        lblEditMORelationshipRemark.Visible = False
                    Else
                        lblEditMORelationshipRemark.Visible = True
                        lblEditMORelationshipRemark.Text = " (" + lblEditMORelationshipRemark.Text.Trim + ")"
                    End If
                Else
                    lblEditMOName.Visible = False
                    txtEditMOName.Visible = True

                    lblEditMONameChi.Visible = False
                    txtEditMONameChi.Visible = True

                    lblEditMOBRCode.Visible = False
                    txtEditMOBRCode.Visible = True

                    pnlEditMORelationship.Visible = True
                    lblEditMORelationship.Visible = False
                    lblEditMORelationshipRemark.Visible = False
                End If

                rboEditMORelation.DataSource = udtSPProfileBLL.GetPracticeType
                rboEditMORelation.DataValueField = "ItemNo"

                If Session("language") = "zh-tw" Then
                    rboEditMORelation.DataTextField = "DataValueChi"
                Else
                    rboEditMORelation.DataTextField = "DataValue"
                End If
                rboEditMORelation.DataBind()

                If Not hfEditMORelation.Value.Equals(String.Empty) Then
                    rboEditMORelation.SelectedValue = hfEditMORelation.Value.Trim
                End If

                rboEditMORelation.Attributes.Add("onclick", "javascript:enableRemarkTextbox('" + rboEditMORelation.ClientID + "', '" + txtEditMORelationRemark.ClientID + "')")


                If Not hfEditMORelation.Value.Trim.Equals(String.Empty) Then
                    rboEditMORelation.SelectedValue = hfEditMORelation.Value

                    If hfEditMORelation.Value.Trim.Equals("O") Then
                        txtEditMORelationRemark.BackColor = Nothing
                        txtEditMORelationRemark.Attributes.Remove("readonly")
                    Else
                        txtEditMORelationRemark.BackColor = Drawing.Color.WhiteSmoke
                        txtEditMORelationRemark.Attributes.Add("readonly", "readonly")
                    End If
                Else

                    txtEditMORelationRemark.BackColor = Drawing.Color.WhiteSmoke
                    txtEditMORelationRemark.Attributes.Add("readonly", "readonly")
                End If

                'Area
                rbEditMOArea.DataSource = udtSPProfileBLL.GetArea.Values
                rbEditMOArea.DataValueField = "Area_ID"
                rbEditMOArea.DataTextField = "Area_Name"
                rbEditMOArea.DataBind()
                bindDistrict(ddlEditMODistrict, String.Empty, False)

                If hfEditMOArea.Value.Equals(String.Empty) Then

                Else
                    hfGridviewIndex.Value = String.Empty
                    rbEditMOArea.SelectedValue = hfEditMOArea.Value.Trim
                    'bindDistrict(ddlEditPracticeDistrict, rbEditPracticeArea.SelectedValue, False)
                    ddlEditMODistrict.SelectedValue = hfEditMODistrict.Value.Trim
                End If

                If Not hfEditMOAddressCode.Value.Trim.Equals(String.Empty) Then
                    ddlEditMODistrict.Enabled = False
                    rbEditMOArea.Enabled = False

                    txtEditMOBlock.ReadOnly = True
                    txtEditMOBuilding.ReadOnly = True

                    txtEditMOBlock.BackColor = Drawing.Color.WhiteSmoke
                    txtEditMOBuilding.BackColor = Drawing.Color.WhiteSmoke

                    ibtnEditMOSearchAddress.Enabled = False
                    ibtnEditMOSearchAddress.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddressSearchDisableSBtn")
                Else
                    txtEditMOBlock.BackColor = Nothing
                    txtEditMOBuilding.BackColor = Nothing
                    ibtnEditMOSearchAddress.Enabled = True
                    ibtnEditMOSearchAddress.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddressSearchSBtn")
                End If
                rbEditMOArea.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
                ddlEditMODistrict.Attributes.Add("onfocus", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")

                'Structure Address
                ibtnEditMOSearchAddress.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
                ibtnEditClearMOSearchAddress.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")

                ApplyMOChangeIndicator(e.Row, True)

            Else
                ' ReadOnly Mode
                Dim lblMODispalySeq As Label = CType(e.Row.FindControl("lblMODispalySeq"), Label)

                If lblMODispalySeq.Text.Trim.Equals(String.Empty) Or lblMODispalySeq.Text.Trim.Equals("0") Then
                    e.Row.Visible = False
                Else
                    Dim lblMOStatus As Label = CType(e.Row.FindControl("lblMOStatus"), Label)
                    Select Case hfTableLocation.Value.Trim
                        Case TableLocation.Permanent
                            Status.GetDescriptionFromDBCode(MedicalOrganizationStatus.ClassCode, lblMOStatus.Text.Trim, lblMOStatus.Text, String.Empty)
                        Case TableLocation.Staging
                            Status.GetDescriptionFromDBCode(MedicalOrganizationStagingStatus.ClassCode, lblMOStatus.Text.Trim, lblMOStatus.Text, String.Empty)
                        Case TableLocation.Enrolment

                            lblMOStatus.Text = "Unprocessed"
                    End Select

                    Dim lblRegMORelationshipRemark As Label = CType(e.Row.FindControl("lblRegMORelationshipRemark"), Label)

                    If lblRegMORelationshipRemark.Text.Trim.Equals(String.Empty) Then
                        lblRegMORelationshipRemark.Visible = False
                    Else
                        lblRegMORelationshipRemark.Visible = True
                        lblRegMORelationshipRemark.Text = " (" + lblRegMORelationshipRemark.Text.Trim + ")"
                    End If

                    Dim ibtnMOEdit As ImageButton = CType(e.Row.FindControl("ibtnMOEdit"), ImageButton)
                    Dim ibtnMODelete As ImageButton = CType(e.Row.FindControl("ibtnMODelete"), ImageButton)
                    Dim hfMOStatus As HiddenField = CType(e.Row.FindControl("hfMOStatus"), HiddenField)
                    'Dim lblMODispalySeq As Label = CType(e.Row.FindControl("lblMODispalySeq"), Label)

                    If gvMOInfo.ShowFooter Then
                        ibtnMOEdit.Enabled = False
                        ibtnMODelete.Enabled = False

                        ibtnMOEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                        ibtnMODelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")
                    Else
                        If gvMOInfo.EditIndex > -1 Then
                            ibtnMOEdit.Enabled = False
                            ibtnMODelete.Enabled = False

                            ibtnMOEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                            ibtnMODelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")
                        Else
                            Select Case hfTableLocation.Value.Trim
                                Case TableLocation.Permanent
                                    Select Case hfMOStatus.Value.Trim
                                        Case MedicalOrganizationStatus.Delisted
                                            ibtnMOEdit.Enabled = False
                                            ibtnMOEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")

                                        Case Else
                                            ibtnMOEdit.Enabled = True
                                            ibtnMOEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSBtn")
                                    End Select
                                    ibtnMODelete.Enabled = False
                                    ibtnMODelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")

                                Case TableLocation.Staging
                                    Select Case hfMOStatus.Value.Trim
                                        Case MedicalOrganizationStagingStatus.Delisted
                                            ibtnMODelete.Enabled = False
                                            ibtnMODelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")

                                            ibtnMOEdit.Enabled = False
                                            ibtnMOEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")

                                        Case MedicalOrganizationStagingStatus.Existing
                                            ibtnMODelete.Enabled = False
                                            ibtnMODelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")

                                            ibtnMOEdit.Enabled = True
                                            ibtnMOEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSBtn")

                                        Case MedicalOrganizationStagingStatus.Update
                                            ibtnMODelete.Enabled = False
                                            ibtnMODelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")

                                            ibtnMOEdit.Enabled = True
                                            ibtnMOEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSBtn")
                                        Case Else
                                            ibtnMODelete.Enabled = True
                                            ibtnMODelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSBtn")

                                            ibtnMOEdit.Enabled = True
                                            ibtnMOEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSBtn")
                                    End Select

                                Case TableLocation.Enrolment
                                    If blnExisingHKID Then
                                        ibtnMODelete.Enabled = False
                                        ibtnMODelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")
                                        ibtnMOEdit.Enabled = False
                                        ibtnMOEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                                    Else
                                        ibtnMODelete.Enabled = True
                                        ibtnMODelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSBtn")
                                        ibtnMOEdit.Enabled = True
                                        ibtnMOEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSBtn")
                                    End If

                            End Select

                            'SM = New Common.ComObject.SystemMessage("010101", "Q", "00001")

                            'Dim strDeleteMessage As String = SM.GetMessage()
                            'strDeleteMessage = strDeleteMessage.Replace("%s", lblMODispalySeq.Text.Trim)

                            'ibtnMODelete.OnClientClick = "showConfirm(this,'" & strDeleteMessage & "'); return false;"
                        End If

                    End If

                End If

                ApplyMOChangeIndicator(e.Row)

            End If

        ElseIf e.Row.RowType = DataControlRowType.Footer Then
            Dim ddlAddMODistrict As DropDownList = e.Row.FindControl("ddlAddMODistrict")
            Dim rbAddMOArea As RadioButtonList = e.Row.FindControl("rbAddMOArea")

            Dim ibtnAddMOSearchAddress As ImageButton = e.Row.FindControl("ibtnAddMOSearchAddress")
            Dim ibtnAddClearMOSearchAddress As ImageButton = e.Row.FindControl("ibtnAddClearMOSearchAddress")

            Dim rboAddMORelation As RadioButtonList = e.Row.FindControl("rboAddMORelation")
            Dim txtAddMORelationRemark As TextBox = e.Row.FindControl("txtAddMORelationRemark")

            rbAddMOArea.DataSource = udtSPProfileBLL.GetArea.Values
            rbAddMOArea.DataValueField = "Area_ID"
            rbAddMOArea.DataTextField = "Area_Name"
            rbAddMOArea.DataBind()

            bindDistrict(ddlAddMODistrict, String.Empty, False)

            rbAddMOArea.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
            ddlAddMODistrict.Attributes.Add("onfocus", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")


            rboAddMORelation.DataSource = udtSPProfileBLL.GetPracticeType
            rboAddMORelation.DataValueField = "ItemNo"

            If Session("language") = "zh-tw" Then
                rboAddMORelation.DataTextField = "DataValueChi"
            Else
                rboAddMORelation.DataTextField = "DataValue"
            End If
            rboAddMORelation.DataBind()

            rboAddMORelation.Attributes.Add("onclick", "javascript:enableRemarkTextbox('" + rboAddMORelation.ClientID + "', '" + txtAddMORelationRemark.ClientID + "')")



            If rboAddMORelation.SelectedValue.Trim.Equals("O") Then
                txtAddMORelationRemark.BackColor = Nothing
                txtAddMORelationRemark.Attributes.Remove("readonly")
            Else
                txtAddMORelationRemark.BackColor = Drawing.Color.WhiteSmoke
                txtAddMORelationRemark.Attributes.Add("readonly", "readonly")
            End If

            'Structure Address
            ibtnAddMOSearchAddress.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
            ibtnAddClearMOSearchAddress.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
        End If

    End Sub

    Private Sub gvMOInfo_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvMOInfo.RowCancelingEdit
        msgBox.Visible = False

        gvMOInfo.EditIndex = -1
        gvMOInfo.DataSource = udtServiceProviderBLL.GetSP.MOList.Values
        gvMOInfo.DataBind()
        gvMOInfo.ShowFooter = False
    End Sub

    Private Sub gvMOInfo_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvMOInfo.RowCommand
        If e.CommandName.Equals("Add") Then
            Dim udtAuditLogEntrySave As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
            msgBox.Visible = False

            Dim footerRow As GridViewRow
            footerRow = gvMOInfo.FooterRow

            Dim noOfRow As Integer
            If IsNothing(udtServiceProviderBLL.GetSP.MOList) Then
                noOfRow = 1
            Else
                noOfRow = udtServiceProviderBLL.GetSP.MOList.Count + 1
            End If


            Dim txtAddMOName As TextBox = footerRow.FindControl("txtAddMOName")
            Dim txtAddMONameChi As TextBox = footerRow.FindControl("txtAddMONameChi")
            Dim txtAddMOBRCode As TextBox = footerRow.FindControl("txtAddMOBRCode")
            Dim txtAddMOEmail As TextBox = footerRow.FindControl("txtAddMOEmail")
            Dim txtAddMOPhone As TextBox = footerRow.FindControl("txtAddMOPhone")
            Dim txtAddMOFax As TextBox = footerRow.FindControl("txtAddMOFax")
            Dim txtAddMORoom As TextBox = footerRow.FindControl("txtAddMORoom")
            Dim txtAddMOFloor As TextBox = footerRow.FindControl("txtAddMOFloor")
            Dim txtAddMOBlock As TextBox = footerRow.FindControl("txtAddMOBlock")
            Dim txtAddMOBuilding As TextBox = footerRow.FindControl("txtAddMOBuilding")
            Dim hfAddMOAddressCode As HiddenField = footerRow.FindControl("hfAddMOAddressCode")
            Dim ddlAddMODistrict As DropDownList = footerRow.FindControl("ddlAddMODistrict")
            Dim rbAddMOArea As RadioButtonList = footerRow.FindControl("rbAddMOArea")
            'Dim rbAddPracticeType As RadioButtonList = footerRow.FindControl("rbAddPracticeType")
            Dim rboAddMORelation As RadioButtonList = footerRow.FindControl("rboAddMORelation")
            Dim txtAddMORelationRemark As TextBox = footerRow.FindControl("txtAddMORelationRemark")

            Dim imgAddMONameAlert As Image = footerRow.FindControl("imgAddMONameAlert")
            Dim imgAddMOBRCodeAlert As Image = footerRow.FindControl("imgAddMOBRCodeAlert")
            Dim imgAddMOEmailAlert As Image = footerRow.FindControl("imgAddMOEmailAlert")
            Dim imgAddMOPhoneAlert As Image = footerRow.FindControl("imgAddMOPhoneAlert")
            Dim imgAddMOBuildingAlert As Image = footerRow.FindControl("imgAddMOBuildingAlert")
            Dim imgAddMODistrcitAlert As Image = footerRow.FindControl("imgAddMODistrcitAlert")
            Dim imgAddMOAreaAlert As Image = footerRow.FindControl("imgAddMOAreaAlert")
            Dim imgAddMORelationRemarksAlert As Image = footerRow.FindControl("imgAddMORelationRemarksAlert")
            Dim imgAddMORelationAlert As Image = footerRow.FindControl("imgAddMORelationAlert")

            imgAddMONameAlert.Visible = False
            imgAddMOBRCodeAlert.Visible = False
            imgAddMOEmailAlert.Visible = False
            imgAddMOPhoneAlert.Visible = False
            imgAddMOBuildingAlert.Visible = False
            imgAddMODistrcitAlert.Visible = False
            imgAddMOPhoneAlert.Visible = False
            imgAddMOAreaAlert.Visible = False
            imgAddMORelationRemarksAlert.Visible = False
            imgAddMORelationAlert.Visible = False

            udtAuditLogEntrySave.AddDescripton("ERN", Me.hfERN.Value.Trim)
            If udtServiceProviderBLL.Exist Then
                udtAuditLogEntrySave.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
            Else
                udtAuditLogEntrySave.AddDescripton("SPID", "")
            End If
            udtAuditLogEntrySave.AddDescripton("MO No", udtServiceProviderBLL.GetSP.MOList.Count + 1)
            udtAuditLogEntrySave.WriteStartLog(Common.Component.LogID.LOG00068, "Add MO")

            'Check the name of mo
            SM = Validator.chkMOEnglishName(txtAddMOName.Text.Trim)
            If Not IsNothing(SM) Then
                imgAddMONameAlert.Visible = True
                msgBox.AddMessage(SM, "%s", noOfRow)
            End If

            'SM the business registration number of MO
            SM = Validator.chkMOBRCode(txtAddMOBRCode.Text.Trim)
            If Not IsNothing(SM) Then
                imgAddMOBRCodeAlert.Visible = True
                msgBox.AddMessage(SM, "%s", noOfRow)
            End If

            'check the contact daytime contact tel no of MO
            SM = Validator.chkMOContactNo(txtAddMOPhone.Text.Trim)
            If Not IsNothing(SM) Then
                imgAddMOPhoneAlert.Visible = True
                msgBox.AddMessage(SM, "%s", noOfRow)
            End If

            'check the contact email address of MO
            If Not txtAddMOEmail.Text.Trim.Equals(String.Empty) Then
                SM = Validator.chkMOEmail(txtAddMOEmail.Text.Trim)
                If Not IsNothing(SM) Then
                    imgAddMOEmailAlert.Visible = True
                    msgBox.AddMessage(SM, "%s", noOfRow)
                End If
            End If

            'Check the address of practice
            SM = Validator.chkMOAddress(txtAddMOBuilding.Text.Trim, ddlAddMODistrict.SelectedValue.Trim, rbAddMOArea.SelectedValue.Trim)
            If Not SM Is Nothing Then
                If Validator.IsEmpty(txtAddMOBuilding.Text.Trim) Then
                    imgAddMOBuildingAlert.Visible = True
                End If

                'If Validator.IsEmpty(ddlAddPracticeDistrict.SelectedValue.Trim) Then

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                'If Validator.IsEmpty(ddlAddMODistrict.SelectedValue.Trim) OrElse _
                '   ddlAddMODistrict.SelectedValue.Trim.Equals(".H") OrElse _
                '   ddlAddMODistrict.SelectedValue.Trim.Equals(".K") OrElse _
                '   ddlAddMODistrict.SelectedValue.Trim.Equals(".N") Then
                If Validator.IsEmpty(ddlAddMODistrict.SelectedValue.Trim) OrElse _
                   ddlAddMODistrict.SelectedValue.Trim.StartsWith(".") Then
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]
                    imgAddMODistrcitAlert.Visible = True
                End If

                If Validator.IsEmpty(rbAddMOArea.SelectedValue.Trim) Then
                    imgAddMOAreaAlert.Visible = True
                End If

                msgBox.AddMessage(SM, "%s", noOfRow)
            End If


            ''Check the type of practice
            'SM = Validator.chkPracticeType(rbAddPracticeType.SelectedValue)
            'If Not SM Is Nothing Then
            '    imgAddPracticeTypeAlert.Visible = True
            '    msgBox.AddMessage(SM, "%s", noOfRow)
            'End If
            ''smPractice(2) = SM


            'check the relation of MO
            SM = Validator.chkMORelation(rboAddMORelation.SelectedValue.Trim)
            If Not IsNothing(SM) Then
                imgAddMORelationAlert.Visible = True
                msgBox.AddMessage(SM, "%s", noOfRow)
            End If


            If rboAddMORelation.SelectedValue.Trim.Equals("O") Then
                If Validator.IsEmpty(txtAddMORelationRemark.Text.Trim) Then
                    imgAddMORelationRemarksAlert.Visible = True
                    SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00155)
                    msgBox.AddMessage(SM, "%s", noOfRow)
                End If
            End If

            If msgBox.GetCodeTable.Rows.Count = 0 Then
                msgBox.Visible = False

                Dim intAddressCode As Nullable(Of Integer)

                If hfAddMOAddressCode.Value.Trim.Equals(String.Empty) Then
                    intAddressCode = Nothing
                Else
                    intAddressCode = CInt(hfAddMOAddressCode.Value.Trim)
                End If

                'Add new practice information and get latest version of Bank Acct record with practice information
                Try

                    Dim udtMO As MedicalOrganizationModel = New MedicalOrganizationModel

                    With udtMO
                        .EnrolRefNo = hfERN.Value.Trim
                        .MOEngName = txtAddMOName.Text.Trim
                        .MOChiName = txtAddMONameChi.Text.Trim
                        .BrCode = txtAddMOBRCode.Text.Trim
                        .Email = txtAddMOEmail.Text.Trim
                        .PhoneDaytime = txtAddMOPhone.Text.Trim
                        .Fax = txtAddMOFax.Text.Trim
                        .Relationship = rboAddMORelation.SelectedValue.Trim
                        .RelationshipRemark = txtAddMORelationRemark.Text.Trim

                        If udtServiceProviderBLL.Exist Then
                            .SPID = udtServiceProviderBLL.GetSP.SPID
                        End If

                        If intAddressCode.HasValue Then
                            .MOAddress = New AddressModel(txtAddMORoom.Text.Trim, txtAddMOFloor.Text.Trim, txtAddMOBlock.Text.Trim, _
                                                        String.Empty, String.Empty, String.Empty, intAddressCode)
                        Else
                            .MOAddress = New AddressModel(txtAddMORoom.Text.Trim, txtAddMOFloor.Text.Trim, txtAddMOBlock.Text.Trim, _
                                                        txtAddMOBuilding.Text.Trim, String.Empty, ddlAddMODistrict.SelectedValue.Trim, intAddressCode)
                        End If



                    End With

                    If udtSPProfileBLL.AddMedicalOrganizationToStaging(udtMO, hfTableLocation.Value.Trim) Then

                        udtAuditLogEntrySave.WriteEndLog(Common.Component.LogID.LOG00069, "Add MO Completed")


                        If udtSPProfileBLL.GetServiceProviderProfile(hfERN.Value.Trim, TableLocation.Staging) Then
                            hfTableLocation.Value = TableLocation.Staging

                            gvMOInfo.ShowFooter = False
                            BindSPProfile()

                            'Dim intEditPracticeIndex As Integer = setEditIndexPracticeGridview()
                            'gvPracticeInfo.EditIndex = intEditPracticeIndex
                            'gvPracticeInfo.DataBind()

                            'Dim intEditIndex As Integer = setEditIndexBankGridview()
                            'gvBankInfo.EditIndex = intEditIndex
                            'gvBankInfo.DataBind()

                            'Dim intEditSchemeIndex As Integer = setEditIndexSchemeGridview()
                            'gvSchemeInfo.EditIndex = intEditSchemeIndex
                            'gvSchemeInfo.DataBind()

                            Page_Load(sender, e)

                            'Me.tabPracticeInfo.Visible = True

                        End If
                    End If
                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then
                        Dim strmsg As String
                        strmsg = eSQL.Message

                        If Not strmsg.Trim.Equals(String.Empty) Then
                            SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                            msgBox.AddMessage(SM)
                            If msgBox.GetCodeTable.Rows.Count = 0 Then
                                msgBox.Visible = False
                            Else
                                'msgBox.BuildMessageBox("UpdateFail")

                                msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntrySave, Common.Component.LogID.LOG00070, "Add MO Fail.")
                            End If
                        Else
                            udtAuditLogEntrySave.WriteEndLog(Common.Component.LogID.LOG00070, "Add MO Fail.")
                        End If

                    Else
                        Throw eSQL
                    End If

                Catch ex As Exception
                    Throw ex
                End Try

            Else
                'msgBox.BuildMessageBox("ValidationFail")
                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntrySave, Common.Component.LogID.LOG00070, "Add MO Fail.")
            End If

        ElseIf e.CommandName = "CancelAdd" Then
            msgBox.Visible = False
            If udtServiceProviderBLL.GetSP.MOList.Count = 0 Then
                gvMOInfo.ShowFooter = True
                gvMOInfo.DataSource = udtSPProfileBLL.EmptyMOCollection.Values
                gvMOInfo.DataBind()
            Else
                gvMOInfo.ShowFooter = False

                gvMOInfo.DataSource = udtServiceProviderBLL.GetSP.MOList.Values
                gvMOInfo.DataBind()

                ibtnMOPageChecked.Enabled = True
                ibtnMOPageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedBtn")

                ibtnMOAdd.Enabled = True
                ibtnMOAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSBtn")
            End If
        End If
    End Sub

    Private Sub gvMOInfo_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvMOInfo.RowEditing
        gvMOInfo.ShowFooter = False
        gvMOInfo.EditIndex = e.NewEditIndex
        gvMOInfo.DataSource = udtServiceProviderBLL.GetSP.MOList.Values
        gvMOInfo.DataBind()

    End Sub

    Private Sub gvMOInfo_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvMOInfo.RowUpdating
        gvMOInfo.ShowFooter = False
        Dim row As GridViewRow = gvMOInfo.Rows(e.RowIndex)

        msgBox.Visible = False

        If Not row Is Nothing Then
            Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log

            Dim lblEditMODispalySeq As Label = row.FindControl("lblEditMODispalySeq")

            Dim txtEditMOName As TextBox = row.FindControl("txtEditMOName")
            Dim txtEditMONameChi As TextBox = row.FindControl("txtEditMONameChi")
            Dim txtEditMOBRCode As TextBox = row.FindControl("txtEditMOBRCode")
            Dim txtEditMOEmail As TextBox = row.FindControl("txtEditMOEmail")
            Dim txtEditMOPhone As TextBox = row.FindControl("txtEditMOPhone")
            Dim txtEditMOFax As TextBox = row.FindControl("txtEditMOFax")
            Dim txtEditMORoom As TextBox = row.FindControl("txtEditMORoom")
            Dim txtEditMOFloor As TextBox = row.FindControl("txtEditMOFloor")
            Dim txtEditMOBlock As TextBox = row.FindControl("txtEditMOBlock")
            Dim txtEditMOBuilding As TextBox = row.FindControl("txtEditMOBuilding")
            Dim hfEditMOAddressCode As HiddenField = row.FindControl("hfEditMOAddressCode")
            Dim ddlEditMODistrict As DropDownList = row.FindControl("ddlEditMODistrict")
            Dim rbEditMOArea As RadioButtonList = row.FindControl("rbEditMOArea")
            'Dim rbAddPracticeType As RadioButtonList = footerRow.FindControl("rbAddPracticeType")
            Dim rboEditMORelation As RadioButtonList = row.FindControl("rboEditMORelation")
            Dim txtEditMORelationRemark As TextBox = row.FindControl("txtEditMORelationRemark")

            Dim imgEditMONameAlert As Image = row.FindControl("imgEditMONameAlert")
            Dim imgEditMOBRCodeAlert As Image = row.FindControl("imgEditMOBRCodeAlert")
            Dim imgEditMOEmailAlert As Image = row.FindControl("imgEditMOEmailAlert")
            Dim imgEditMOPhoneAlert As Image = row.FindControl("imgEditMOPhoneAlert")
            Dim imgEditMOBuildingAlert As Image = row.FindControl("imgEditMOBuildingAlert")
            Dim imgEditMODistrcitAlert As Image = row.FindControl("imgEditMODistrcitAlert")
            Dim imgEditMOAreaAlert As Image = row.FindControl("imgEditMOAreaAlert")
            Dim imgEditMORelationRemarksAlert As Image = row.FindControl("imgEditMORelationRemarksAlert")
            Dim imgEditMORelationAlert As Image = row.FindControl("imgEditMORelationAlert")

            imgEditMONameAlert.Visible = False
            imgEditMOBRCodeAlert.Visible = False
            imgEditMOEmailAlert.Visible = False
            imgEditMOPhoneAlert.Visible = False
            imgEditMOBuildingAlert.Visible = False
            imgEditMODistrcitAlert.Visible = False
            imgEditMOAreaAlert.Visible = False
            imgEditMORelationRemarksAlert.Visible = False
            imgEditMORelationAlert.Visible = False

            udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)

            If udtServiceProviderBLL.Exist Then
                udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
            Else
                udtAuditLogEntry.AddDescripton("SPID", "")
            End If

            udtAuditLogEntry.AddDescripton("MO No", lblEditMODispalySeq.Text.Trim)
            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00071, "Save MO:")

            'Check the name of practice
            SM = Validator.chkMOEnglishName(txtEditMOName.Text.Trim)
            If Not IsNothing(SM) Then
                imgEditMONameAlert.Visible = True
                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If

            'SM the business registration number of MO
            SM = Validator.chkMOBRCode(txtEditMOBRCode.Text.Trim)
            If Not IsNothing(SM) Then
                imgEditMOBRCodeAlert.Visible = True
                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If

            'check the contact daytime contact tel no of MO
            SM = Validator.chkMOContactNo(txtEditMOPhone.Text.Trim)
            If Not IsNothing(SM) Then
                imgEditMOPhoneAlert.Visible = True
                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If

            'check the contact email address of MO
            If Not txtEditMOEmail.Text.Trim.Equals(String.Empty) Then
                SM = Validator.chkMOEmail(txtEditMOEmail.Text.Trim)
                If Not IsNothing(SM) Then
                    imgEditMOEmailAlert.Visible = True
                    msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
                End If
            End If

            'Check the address of practice
            SM = Validator.chkMOAddress(txtEditMOBuilding.Text.Trim, ddlEditMODistrict.SelectedValue.Trim, rbEditMOArea.SelectedValue.Trim)
            If Not SM Is Nothing Then
                If Validator.IsEmpty(txtEditMOBuilding.Text.Trim) Then
                    imgEditMOBuildingAlert.Visible = True
                End If

                'If Validator.IsEmpty(ddlAddPracticeDistrict.SelectedValue.Trim) Then

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                'If Validator.IsEmpty(ddlEditMODistrict.SelectedValue.Trim) OrElse _
                '   ddlEditMODistrict.SelectedValue.Trim.Equals(".H") OrElse _
                '   ddlEditMODistrict.SelectedValue.Trim.Equals(".K") OrElse _
                '   ddlEditMODistrict.SelectedValue.Trim.Equals(".N") OrElse _
                '   ddlEditMODistrict.SelectedValue.Trim.Equals(".S") Then
                If Validator.IsEmpty(ddlEditMODistrict.SelectedValue.Trim) OrElse _
                   ddlEditMODistrict.SelectedValue.Trim.StartsWith(".") Then
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]
                    imgEditMODistrcitAlert.Visible = True
                End If

                If Validator.IsEmpty(rbEditMOArea.SelectedValue.Trim) Then
                    imgEditMOAreaAlert.Visible = True
                End If

                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If


            ''Check the type of practice
            'SM = Validator.chkPracticeType(rbAddPracticeType.SelectedValue)
            'If Not SM Is Nothing Then
            '    imgAddPracticeTypeAlert.Visible = True
            '    msgBox.AddMessage(SM, "%s", noOfRow)
            'End If
            ''smPractice(2) = SM


            'check the relation of MO
            SM = Validator.chkMORelation(rboEditMORelation.SelectedValue.Trim)
            If Not IsNothing(SM) Then
                imgEditMORelationAlert.Visible = True
                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If


            If rboEditMORelation.SelectedValue.Trim.Equals("O") Then
                txtEditMORelationRemark.BackColor = Nothing
                txtEditMORelationRemark.Attributes.Remove("readonly")

                If Validator.IsEmpty(txtEditMORelationRemark.Text.Trim) Then
                    imgEditMORelationRemarksAlert.Visible = True

                    SM = New Common.ComObject.SystemMessage("990000", Common.Component.SeverityCode.SEVE, Common.Component.MsgCode.MSG00155)
                    msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
                End If
            Else
                txtEditMORelationRemark.BackColor = Drawing.Color.WhiteSmoke
                txtEditMORelationRemark.Attributes.Add("readonly", "readonly")
            End If



            If msgBox.GetCodeTable.Rows.Count = 0 Then
                msgBox.Visible = False

                Dim intAddressCode As Nullable(Of Integer)

                If hfEditMOAddressCode.Value.Trim.Equals(String.Empty) Then
                    intAddressCode = Nothing
                Else
                    intAddressCode = CInt(hfEditMOAddressCode.Value.Trim)
                End If

                'Update practice record and get latest version of Bank Acct record with practice information
                Try
                    Dim udtMO As New MedicalOrganizationModel
                    Call (New MedicalOrganizationBLL).Clone(udtMO, udtServiceProviderBLL.GetSP.MOList.Item(CInt(lblEditMODispalySeq.Text.Trim)))

                    With udtMO
                        .EnrolRefNo = hfERN.Value.Trim
                        .MOEngName = txtEditMOName.Text.Trim
                        .MOChiName = txtEditMONameChi.Text.Trim
                        .BrCode = txtEditMOBRCode.Text.Trim
                        .Email = txtEditMOEmail.Text.Trim
                        .PhoneDaytime = txtEditMOPhone.Text.Trim
                        .Fax = txtEditMOFax.Text.Trim
                        .Relationship = rboEditMORelation.SelectedValue.Trim
                        .RelationshipRemark = txtEditMORelationRemark.Text.Trim

                        If intAddressCode.HasValue Then
                            .MOAddress = New AddressModel(txtEditMORoom.Text.Trim, txtEditMOFloor.Text.Trim, txtEditMOBlock.Text.Trim, _
                                                        String.Empty, String.Empty, String.Empty, intAddressCode)
                        Else
                            .MOAddress = New AddressModel(txtEditMORoom.Text.Trim, txtEditMOFloor.Text.Trim, txtEditMOBlock.Text.Trim, _
                                                        txtEditMOBuilding.Text.Trim, String.Empty, ddlEditMODistrict.SelectedValue.Trim, intAddressCode)
                        End If

                    End With


                    If udtSPProfileBLL.UpdateMedicalOrganizationToStaging(udtMO, hfTableLocation.Value.Trim) Then


                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00072, "Save MO Completed.")

                        If udtSPProfileBLL.GetServiceProviderProfile(hfERN.Value.Trim, TableLocation.Staging) Then
                            hfTableLocation.Value = TableLocation.Staging

                            BindSPProfile()

                        End If
                    End If

                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then
                        Dim strmsg As String
                        strmsg = eSQL.Message

                        If Not strmsg.Trim.Equals(String.Empty) Then
                            SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                            msgBox.AddMessage(SM)
                            If msgBox.GetCodeTable.Rows.Count = 0 Then
                                msgBox.Visible = False
                            Else
                                'msgBox.BuildMessageBox("UpdateFail")
                                msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00073, "Save MO Fail.")
                            End If
                        Else
                            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00073, "Save MO Fail.")
                        End If


                        'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                    Else
                        Throw eSQL
                    End If

                Catch ex As Exception
                    Throw ex
                End Try

            Else
                'msgBox.BuildMessageBox("ValidationFail")
                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00073, "Save MO Fail.")
            End If

        End If
    End Sub

    '

    Protected Sub ibtnMOAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry.WriteLog(LogID.LOG00112, "MO Add Click")

        Me.gvMOInfo.ShowFooter = True
        If udtServiceProviderBLL.Exist Then
            If IsNothing(udtServiceProviderBLL.GetSP.MOList) Then
                gvMOInfo.DataSource = udtSPProfileBLL.EmptyMOCollection.Values
            Else
                If udtServiceProviderBLL.GetSP.MOList.Count = 0 Then
                    gvMOInfo.DataSource = udtSPProfileBLL.EmptyMOCollection.Values
                Else
                    gvMOInfo.DataSource = udtServiceProviderBLL.GetSP.MOList.Values
                End If
            End If

            gvMOInfo.DataBind()
        End If

    End Sub

    Protected Sub ibtnMODelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        udtAuditLogEntry.WriteLog(LogID.LOG00113, "MO Delete Click")
        ' CRE11-021 log the missed essential information [End]

        Dim ibtnMODelete As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = ibtnMODelete.NamingContainer

        If Not row Is Nothing Then
            Dim lblMODispalySeq As Label = CType(gvMOInfo.Rows(row.RowIndex).FindControl("lblMODispalySeq"), Label)

            Me.hfDeleteMO.Value = lblMODispalySeq.Text.Trim

            Dim strDeleteMessage As String = String.Empty

            Dim udtPracticeList As PracticeModelCollection = Nothing
            If udtServiceProviderBLL.Exist Then
                udtPracticeList = New PracticeModelCollection
                udtPracticeList = udtServiceProviderBLL.GetSP.PracticeList.FilterByMO(CInt(lblMODispalySeq.Text.Trim))
            End If

            If IsNothing(udtPracticeList) Then
                SM = New Common.ComObject.SystemMessage("010101", "Q", "00003")
                strDeleteMessage = SM.GetMessage()

                strDeleteMessage = strDeleteMessage.Replace("%s", lblMODispalySeq.Text.Trim)
            Else

                If udtPracticeList.Count = 0 Then
                    SM = New Common.ComObject.SystemMessage("010101", "Q", "00003")
                    strDeleteMessage = SM.GetMessage()

                    strDeleteMessage = strDeleteMessage.Replace("%s", lblMODispalySeq.Text.Trim)
                Else
                    Dim strPractice As String = String.Empty

                    For Each udtPractice As PracticeModel In udtPracticeList.Values
                        strPractice = strPractice + ", " + udtPractice.DisplaySeq.ToString
                    Next

                    If strPractice.Length > 2 Then
                        strPractice = strPractice.Substring(2)
                    End If

                    SM = New Common.ComObject.SystemMessage("010101", "Q", "00002")
                    strDeleteMessage = SM.GetMessage()

                    strDeleteMessage = strDeleteMessage.Replace("%d", strPractice)
                    strDeleteMessage = strDeleteMessage.Replace("%s", lblMODispalySeq.Text.Trim)
                End If

            End If

            lblDeleteMO.Text = strDeleteMessage

            Me.ModalPopupExtenderDeleteMO.Show()

        End If
    End Sub

    Protected Sub ibtnDeleteMOConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log

        Me.ModalPopupExtenderDeleteMO.Hide()

        Dim intMODisplaySeq As Integer

        intMODisplaySeq = CInt(Me.hfDeleteMO.Value.Trim)


        udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)
        If udtServiceProviderBLL.Exist Then
            udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
        Else
            udtAuditLogEntry.AddDescripton("SPID", "")
        End If

        udtAuditLogEntry.AddDescripton("MO No", intMODisplaySeq)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00062, "Delete MO")

        Try
            Dim udtMO As New MedicalOrganizationModel
            Call (New MedicalOrganizationBLL).Clone(udtMO, udtServiceProviderBLL.GetSP.MOList.Item(intMODisplaySeq))

            If udtSPProfileBLL.DeleteMedicalOraganization(udtMO, hfTableLocation.Value.Trim) Then
                Me.hfDeleteMO.Value = String.Empty

                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00063, "Delete MO Completed.")

                If udtSPProfileBLL.GetServiceProviderProfile(hfERN.Value.Trim, TableLocation.Staging) Then
                    hfTableLocation.Value = TableLocation.Staging

                    BindSPProfile()

                    If udtServiceProviderBLL.Exist Then
                        If udtServiceProviderBLL.GetSP.PracticeList.Count = 0 Then
                            ibtnMOAdd_Click(Nothing, Nothing)
                            Me.tabPracticeInfo.Visible = False
                            Me.tabBankAcctInfo.Visible = False
                            Me.tablSchemeInfo.Visible = False
                        Else

                        End If


                    End If


                End If
            End If
        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                If Not strmsg.Trim.Equals(String.Empty) Then
                    SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                    msgBox.AddMessage(SM)
                    If msgBox.GetCodeTable.Rows.Count = 0 Then
                        msgBox.Visible = False
                    Else
                        ' msgBox.BuildMessageBox("UpdateFail")
                        msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00064, "Delete MO Fail.")
                    End If
                Else
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00064, "Delete MO Fail.")
                End If


                'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub ibtnDeleteMOCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.hfDeleteMO.Value = String.Empty
        ModalPopupExtenderDeleteMO.Hide()
    End Sub

    '

    Protected Sub ibtnDuplicateMO_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim ibtnDuplicateMO As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = ibtnDuplicateMO.NamingContainer

        If Not row Is Nothing Then
            Dim lblMODispalySeq As Label

            If IsNothing(CType(row.FindControl("lblMODispalySeq"), Label)) Then
                lblMODispalySeq = CType(row.FindControl("lblEditMODispalySeq"), Label)
            Else
                lblMODispalySeq = CType(row.FindControl("lblMODispalySeq"), Label)
            End If

            MOPracticeLists1.buildMOObject(udtServiceProviderBLL.GetSP.MOList, CInt(lblMODispalySeq.Text.Trim), Me.hfTableLocation.Value.Trim)

            Me.ModalPopupExtenderDuplicated.Show()
        End If
    End Sub

    Protected Sub ibtnDuplicatedClose_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.ModalPopupExtenderDuplicated.Hide()
    End Sub

#End Region

#Region "Tab - Practice Information"

    Protected Sub ddlDistrict_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim ddlDistrict As DropDownList = Nothing
        Dim rboArea As RadioButtonList = Nothing

        If TabContainer1.ActiveTabIndex = 0 Then
            If fvPersonalParticulars.CurrentMode = FormViewMode.Insert Then
                ddlDistrict = CType(fvPersonalParticulars.FindControl("ddlRegDistrict"), DropDownList)
                rboArea = CType(fvPersonalParticulars.FindControl("rboRegArea"), RadioButtonList)

            ElseIf fvPersonalParticulars.CurrentMode = FormViewMode.Edit Then
                ddlDistrict = CType(fvPersonalParticulars.FindControl("ddlSPEditDistrict"), DropDownList)
                rboArea = CType(fvPersonalParticulars.FindControl("rboSPEditArea"), RadioButtonList)
            End If
        ElseIf TabContainer1.ActiveTabIndex = 1 Then
            If Not hfGridviewIndex.Value.Equals(String.Empty) Then
                If gvMOInfo.ShowFooter Then
                    'Add MO
                    ddlDistrict = gvMOInfo.FooterRow.FindControl("ddlAddMODistrict")
                    rboArea = gvMOInfo.FooterRow.FindControl("rbAddMOArea")
                Else
                    'Edit MO
                    ddlDistrict = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlEditMODistrict")
                    rboArea = gvMOInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("rbEditMOArea")
                End If

            End If
        ElseIf TabContainer1.ActiveTabIndex = 2 Then
            If Not hfGridviewIndex.Value.Equals(String.Empty) Then
                If gvPracticeInfo.ShowFooter Then
                    'Add Practice 
                    ddlDistrict = gvPracticeInfo.FooterRow.FindControl("ddlAddPracticeDistrict")
                    rboArea = gvPracticeInfo.FooterRow.FindControl("rbAddPracticeArea")
                Else
                    'Edit Practice
                    ddlDistrict = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("ddlEditPracticeDistrict")
                    rboArea = gvPracticeInfo.Rows(CInt(hfGridviewIndex.Value)).FindControl("rbEditPracticeArea")
                End If

            End If
        End If

        If Not IsNothing(ddlDistrict) And Not IsNothing(rboArea) Then
            'If rboArea.SelectedItem Is Nothing Then
            Dim strSelectedDistrictValue As String
            strSelectedDistrictValue = ddlDistrict.SelectedValue

            If strSelectedDistrictValue.Equals(String.Empty) Then
                rboArea.ClearSelection()
            Else
                Dim strAreaCode As String

                strAreaCode = udtSPProfileBLL.GetAreaByDistrictCode(ddlDistrict.SelectedValue)
                'bindDistrict(ddlDistrict, strAreaCode, False)

                rboArea.SelectedValue = strAreaCode
                ddlDistrict.SelectedValue = strSelectedDistrictValue
            End If


            'End If
        End If

    End Sub

    Protected Sub ibtnPracticeAdd_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        gvPracticeInfo.ShowFooter = True
        If udtPracticeBLL.Exist Then
            If udtPracticeBLL.GetPracticeCollection.Count = 0 Then
                gvPracticeInfo.DataSource = udtSPProfileBLL.EmptyPracticeCollection.Values
            Else
                gvPracticeInfo.DataSource = udtPracticeBLL.GetPracticeCollection.Values
            End If
            gvPracticeInfo.DataBind()
        End If

        Me.rboHadJoinedEHRSS.Enabled = False
        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
        ' --------------------------------------------------------------------------------------------------------------------------------
        Me.rboJoinPCD.Enabled = False
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

        ibtnEHRSSEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
        ibtnEHRSSEdit.Enabled = False
        ibtnEHRSSSave.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SaveSDisableBtn")
        ibtnEHRSSSave.Enabled = False
        ibtnEHRSSCancel.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CancelSDisableBtn")
        ibtnEHRSSCancel.Enabled = False

    End Sub

    Private Sub gvPracticeInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPracticeInfo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If (e.Row.RowState And DataControlRowState.Edit) > 0 Then
                ' Edit Mode
                Dim hfEditPracticeAddressCode As HiddenField = CType(e.Row.FindControl("hfEditPracticeAddressCode"), HiddenField)
                Dim txtEditPracticeBuilding As TextBox = CType(e.Row.FindControl("txtEditPracticeBuilding"), TextBox)
                Dim txtEditPracticeBuildingChi As TextBox = CType(e.Row.FindControl("txtEditPracticeBuildingChi"), TextBox)

                Dim ddlEditPracticeDistrict As DropDownList = CType(e.Row.FindControl("ddlEditPracticeDistrict"), DropDownList)
                Dim hfEditPracticeDistrict As HiddenField = CType(e.Row.FindControl("hfEditPracticeDistrict"), HiddenField)

                Dim rbEditPracticeArea As RadioButtonList = CType(e.Row.FindControl("rbEditPracticeArea"), RadioButtonList)
                Dim hfEditPracticeArea As HiddenField = CType(e.Row.FindControl("hfEditPracticeArea"), HiddenField)

                Dim ddlEditHealthProf As DropDownList = CType(e.Row.FindControl("ddlEditHealthProf"), DropDownList)
                Dim hfEditHealthProf As HiddenField = CType(e.Row.FindControl("hfEditHealthProf"), HiddenField)

                Dim ibtnEditPracticeSearchAddress As ImageButton = CType(e.Row.FindControl("ibtnEditPracticeSearchAddress"), ImageButton)
                Dim ibtnEditClearPracticeSearchAddress As ImageButton = CType(e.Row.FindControl("ibtnEditClearPracticeSearchAddress"), ImageButton)

                Dim lblEditPracticeName As Label = CType(e.Row.FindControl("lblEditPracticeName"), Label)
                Dim txtEditPracticeName As TextBox = CType(e.Row.FindControl("txtEditPracticeName"), TextBox)

                Dim lblEditPracticeNameChi As Label = CType(e.Row.FindControl("lblEditPracticeNameChi"), Label)
                Dim txtEditPracticeNameChi As TextBox = CType(e.Row.FindControl("txtEditPracticeNameChi"), TextBox)

                Dim lblEditHealthProf As Label = CType(e.Row.FindControl("lblEditHealthProf"), Label)

                Dim lblEditRegCode As Label = CType(e.Row.FindControl("lblEditRegCode"), Label)
                Dim txtEditRegCode As TextBox = CType(e.Row.FindControl("txtEditRegCode"), TextBox)

                Dim hfEditPracticeStatus As HiddenField = CType(e.Row.FindControl("hfEditPracticeStatus"), HiddenField)

                Dim txtEditPracticeBlock As TextBox = CType(e.Row.FindControl("txtEditPracticeBlock"), TextBox)

                Dim lblEditPracticeMOName As Label = CType(e.Row.FindControl("lblEditPracticeMOName"), Label)
                Dim ddlEditPracticeMOName As DropDownList = CType(e.Row.FindControl("ddlEditPracticeMOName"), DropDownList)
                Dim hfEditPracticeMOName As HiddenField = CType(e.Row.FindControl("hfEditPracticeMOName"), HiddenField)

                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If Me.hfTableLocation.Value.Trim.Equals(TableLocation.Permanent) Or _
                'hfEditPracticeStatus.Value.Trim.Equals(PracticeStagingStatus.Existing) Or _
                'hfEditPracticeStatus.Value.Trim.Equals(PracticeStagingStatus.Update) Then

                If Me.hfTableLocation.Value.Trim.Equals(TableLocation.Permanent) Or _
                    hfEditPracticeStatus.Value.Trim.Equals(PracticeStagingStatus.Existing) Or _
                    hfEditPracticeStatus.Value.Trim.Equals(PracticeStagingStatus.Update) Or _
                    hfEditPracticeStatus.Value.Trim.Equals(PracticeStagingStatus.Suspended) Then
                    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
                    lblEditPracticeName.Visible = True
                    txtEditPracticeName.Visible = False

                    lblEditPracticeNameChi.Visible = True
                    txtEditPracticeNameChi.Visible = False

                    lblEditHealthProf.Visible = True
                    ddlEditHealthProf.Visible = False

                    lblEditRegCode.Visible = True
                    txtEditRegCode.Visible = False

                    lblEditPracticeMOName.Visible = True
                    ddlEditPracticeMOName.Visible = False

                    If udtServiceProviderBLL.Exist Then
                        If IsNothing(udtServiceProviderBLL.GetSP.MOList) Then
                            lblEditPracticeMOName.Text = Me.GetGlobalResourceObject("Text", "N\A")
                        Else
                            If udtServiceProviderBLL.GetSP.MOList.Count = 0 Then
                                lblEditPracticeMOName.Text = Me.GetGlobalResourceObject("Text", "N\A")
                            Else
                                If IsNothing(udtServiceProviderBLL.GetSP.MOList.Item(CInt(lblEditPracticeMOName.Text.Trim))) Then
                                    lblEditPracticeMOName.Text = Me.GetGlobalResourceObject("Text", "N\A")
                                Else
                                    lblEditPracticeMOName.Text = udtServiceProviderBLL.GetSP.MOList.Item(CInt(lblEditPracticeMOName.Text.Trim)).DisplaySeqMOName
                                End If

                            End If

                        End If

                    End If

                Else
                    lblEditPracticeName.Visible = False
                    txtEditPracticeName.Visible = True

                    lblEditPracticeNameChi.Visible = False
                    txtEditPracticeNameChi.Visible = True

                    lblEditHealthProf.Visible = False
                    ddlEditHealthProf.Visible = True

                    lblEditRegCode.Visible = False
                    txtEditRegCode.Visible = True

                    'Health Profession
                    ddlEditHealthProf.DataSource = udtSPProfileBLL.GetHealthProf

                    ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

                    ' -----------------------------------------------------------------------------------------

                    ddlEditHealthProf.DataValueField = "ServiceCategoryCode"
                    If Session("language") = "zh-tw" Then
                        ddlEditHealthProf.DataTextField = "ServiceCategoryDescChi"
                    Else
                        ddlEditHealthProf.DataTextField = "ServiceCategoryDesc"
                    End If

                    ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

                    ddlEditHealthProf.DataBind()
                    If Not hfEditHealthProf.Value.Equals(String.Empty) Then
                        ddlEditHealthProf.SelectedValue = hfEditHealthProf.Value.Trim
                    End If

                    lblEditPracticeMOName.Visible = False
                    ddlEditPracticeMOName.Visible = True
                End If

                'Area
                rbEditPracticeArea.DataSource = udtSPProfileBLL.GetArea.Values
                rbEditPracticeArea.DataValueField = "Area_ID"
                rbEditPracticeArea.DataTextField = "Area_Name"
                rbEditPracticeArea.DataBind()
                bindDistrict(ddlEditPracticeDistrict, String.Empty, False)

                If hfEditPracticeArea.Value.Equals(String.Empty) Then

                Else
                    hfGridviewIndex.Value = String.Empty
                    rbEditPracticeArea.SelectedValue = hfEditPracticeArea.Value.Trim
                    ddlEditPracticeDistrict.SelectedValue = hfEditPracticeDistrict.Value.Trim
                End If

                If Not hfEditPracticeAddressCode.Value.Trim.Equals(String.Empty) Then
                    ddlEditPracticeDistrict.Enabled = False
                    rbEditPracticeArea.Enabled = False

                    txtEditPracticeBlock.ReadOnly = True
                    txtEditPracticeBuilding.ReadOnly = True
                    txtEditPracticeBuildingChi.ReadOnly = True

                    txtEditPracticeBlock.BackColor = Drawing.Color.WhiteSmoke
                    txtEditPracticeBuilding.BackColor = Drawing.Color.WhiteSmoke
                    txtEditPracticeBuildingChi.BackColor = Drawing.Color.WhiteSmoke

                    ibtnEditPracticeSearchAddress.Enabled = False
                    ibtnEditPracticeSearchAddress.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddressSearchDisableSBtn")
                Else
                    txtEditPracticeBlock.BackColor = Nothing
                    txtEditPracticeBuilding.BackColor = Nothing
                    txtEditPracticeBuildingChi.BackColor = Nothing

                    ibtnEditPracticeSearchAddress.Enabled = True
                    ibtnEditPracticeSearchAddress.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddressSearchSBtn")
                End If
                rbEditPracticeArea.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
                ddlEditPracticeDistrict.Attributes.Add("onfocus", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")


                'Structure Address
                ibtnEditPracticeSearchAddress.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
                ibtnEditClearPracticeSearchAddress.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")

                'MO
                If udtServiceProviderBLL.Exist Then
                    If Not IsNothing(udtServiceProviderBLL.GetSP.MOList) Then
                        ddlEditPracticeMOName.DataSource = udtServiceProviderBLL.GetSP.MOList.GetActiveMO(Me.hfTableLocation.Value.Trim).Values
                        ddlEditPracticeMOName.DataValueField = "DisplaySeq"
                        ddlEditPracticeMOName.DataTextField = "DisplaySeqMOName"
                        ddlEditPracticeMOName.DataBind()

                        If Not IsNothing(hfEditPracticeMOName) Then
                            If Not hfEditPracticeMOName.Equals(String.Empty) Then
                                If hfEditPracticeMOName.Value.Trim.Equals("0") Then
                                    ddlEditPracticeMOName.SelectedValue = ""
                                    ddlEditPracticeMOName.Enabled = True

                                    'If udtServiceProviderBLL.GetSP.MOList.Count = 1 Then
                                    '    ddlEditPracticeMOName.SelectedIndex = 1
                                    '    ddlEditPracticeMOName.Enabled = False
                                    'Else
                                    '    ddlEditPracticeMOName.Enabled = True
                                    'End If
                                Else
                                    ddlEditPracticeMOName.SelectedValue = hfEditPracticeMOName.Value.Trim
                                    If udtServiceProviderBLL.GetSP.MOList.Count = 1 Then
                                        ddlEditPracticeMOName.SelectedIndex = 1
                                        ddlEditPracticeMOName.Enabled = False
                                    Else
                                        ddlEditPracticeMOName.Enabled = True
                                    End If
                                End If

                            End If
                        End If
                    End If
                End If

                ApplyPracticeChangeIndicator(e.Row, True)

            Else
                ' ReadOnly Mode              
                Dim lblPracticeDispalySeq As Label = e.Row.FindControl("lblPracticeDispalySeq")

                If lblPracticeDispalySeq.Text.Trim.Equals(String.Empty) Or lblPracticeDispalySeq.Text.Trim.Equals("0") Then
                    e.Row.Visible = False
                Else
                    Dim lblPracticeMOName As Label = CType(e.Row.FindControl("lblPracticeMOName"), Label)

                    If udtServiceProviderBLL.Exist Then
                        Dim udtMO As MedicalOrganizationModel
                        If Not lblPracticeMOName.Text.Trim.Equals("0") Then
                            udtMO = udtServiceProviderBLL.GetSP.MOList.Item(CInt(lblPracticeMOName.Text.Trim))

                            If IsNothing(udtMO) Then
                                lblPracticeMOName.Text = Me.GetGlobalResourceObject("Text", "N/A")
                            Else
                                lblPracticeMOName.Text = udtMO.DisplaySeqMOName
                            End If


                        Else
                            lblPracticeMOName.Text = Me.GetGlobalResourceObject("Text", "N/A")
                        End If

                    End If

                    Dim ibtnPracticeEdit As ImageButton = CType(e.Row.FindControl("ibtnPracticeEdit"), ImageButton)
                    Dim ibtnPracticeDelete As ImageButton = CType(e.Row.FindControl("ibtnPracticeDelete"), ImageButton)
                    Dim lblPracticeStatus As Label = CType(e.Row.FindControl("lblPracticeStatus"), Label)

                    Dim hfPracticeStatus As HiddenField = CType(e.Row.FindControl("hfPracticeStatus"), HiddenField)

                    Select Case hfTableLocation.Value.Trim
                        Case TableLocation.Permanent
                            Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, lblPracticeStatus.Text.Trim, lblPracticeStatus.Text, String.Empty)

                        Case TableLocation.Staging
                            Status.GetDescriptionFromDBCode(PracticeStagingStatus.ClassCode, lblPracticeStatus.Text.Trim, lblPracticeStatus.Text, String.Empty)

                        Case TableLocation.Enrolment
                            lblPracticeStatus.Text = "Unprocessed"
                    End Select

                    If gvPracticeInfo.ShowFooter Then
                        ibtnPracticeEdit.Enabled = False
                        ibtnPracticeDelete.Enabled = False

                        ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                        ibtnPracticeDelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")
                    Else
                        If gvPracticeInfo.EditIndex > -1 Then
                            ibtnPracticeEdit.Enabled = False
                            ibtnPracticeDelete.Enabled = False

                            ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                            ibtnPracticeDelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")
                        Else
                            If IsNothing(udtServiceProviderBLL.GetSP.MOList) Then
                                ibtnPracticeEdit.Enabled = False
                                ibtnPracticeDelete.Enabled = False
                                ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                                ibtnPracticeDelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")
                            Else
                                If udtServiceProviderBLL.GetSP.MOList.Count = 0 Then
                                    ibtnPracticeEdit.Enabled = False
                                    ibtnPracticeDelete.Enabled = False
                                    ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                                    ibtnPracticeDelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")
                                Else
                                    Select Case hfTableLocation.Value.Trim
                                        Case TableLocation.Permanent
                                            Select Case hfPracticeStatus.Value.Trim
                                                'Case SPMaintenanceDisplayStatus.DelistedInvoluntary
                                                '    ibtnPracticeEdit.Enabled = False
                                                '    ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                                                Case PracticeStatus.Delisted
                                                    ibtnPracticeEdit.Enabled = False
                                                    ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                                                Case Else
                                                    ibtnPracticeEdit.Enabled = True
                                                    ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSBtn")
                                            End Select
                                            ibtnPracticeDelete.Enabled = False
                                            ibtnPracticeDelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")

                                        Case TableLocation.Staging
                                            Select Case hfPracticeStatus.Value.Trim
                                                Case PracticeStagingStatus.Delisted
                                                    ibtnPracticeDelete.Enabled = False
                                                    ibtnPracticeDelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")

                                                    ibtnPracticeEdit.Enabled = False
                                                    ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")

                                                    'Case PracticeDisplayStatus.DelistedVoluntary
                                                Case PracticeStagingStatus.Suspended
                                                    ibtnPracticeDelete.Enabled = False
                                                    ibtnPracticeDelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")

                                                    ibtnPracticeEdit.Enabled = True
                                                    ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSBtn")

                                                Case PracticeStagingStatus.Existing
                                                    ibtnPracticeDelete.Enabled = False
                                                    ibtnPracticeDelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")

                                                    ibtnPracticeEdit.Enabled = True
                                                    ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSBtn")

                                                Case PracticeStagingStatus.Update
                                                    ibtnPracticeDelete.Enabled = False
                                                    ibtnPracticeDelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")

                                                    ibtnPracticeEdit.Enabled = True
                                                    ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSBtn")
                                                Case Else
                                                    ibtnPracticeDelete.Enabled = True
                                                    ibtnPracticeDelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSBtn")

                                                    ibtnPracticeEdit.Enabled = True
                                                    ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSBtn")
                                            End Select

                                        Case TableLocation.Enrolment
                                            If blnExisingHKID Then
                                                ibtnPracticeDelete.Enabled = False
                                                ibtnPracticeDelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSDisableBtn")
                                                ibtnPracticeEdit.Enabled = False
                                                ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                                            Else
                                                ibtnPracticeDelete.Enabled = True
                                                ibtnPracticeDelete.ImageUrl = GetGlobalResourceObject("ImageUrl", "DeleteSBtn")
                                                ibtnPracticeEdit.Enabled = True
                                                ibtnPracticeEdit.ImageUrl = GetGlobalResourceObject("ImageUrl", "EditSBtn")
                                            End If

                                    End Select
                                End If
                            End If

                        End If

                    End If

                End If

                ApplyPracticeChangeIndicator(e.Row)

            End If
        ElseIf e.Row.RowType = DataControlRowType.Footer Then

            'MO
            Dim ddlAddPracticeMOName As DropDownList = CType(e.Row.FindControl("ddlAddPracticeMOName"), DropDownList)
            If udtServiceProviderBLL.Exist Then
                ddlAddPracticeMOName.DataSource = udtServiceProviderBLL.GetSP.MOList.GetActiveMO(Me.hfTableLocation.Value.Trim).Values
                ddlAddPracticeMOName.DataValueField = "DisplaySeq"
                ddlAddPracticeMOName.DataTextField = "DisplaySeqMOName"
                ddlAddPracticeMOName.DataBind()

                If Not IsNothing(udtServiceProviderBLL.GetSP.MOList) Then
                    If udtServiceProviderBLL.GetSP.MOList.Count = 1 Then
                        ddlAddPracticeMOName.SelectedIndex = 1
                        ddlAddPracticeMOName.Enabled = False
                    Else
                        ddlAddPracticeMOName.Enabled = True
                    End If
                End If
            End If

            Dim ddlAddPracticeDistrict As DropDownList = e.Row.FindControl("ddlAddPracticeDistrict")
            Dim rbAddPracticeArea As RadioButtonList = e.Row.FindControl("rbAddPracticeArea")
            Dim ddlAddHealthProf As DropDownList = e.Row.FindControl("ddlAddHealthProf")

            Dim ibtnAddPracticeSearchAddress As ImageButton = e.Row.FindControl("ibtnAddPracticeSearchAddress")
            Dim ibtnAddClearPracticeSearchAddress As ImageButton = e.Row.FindControl("ibtnAddClearPracticeSearchAddress")

            rbAddPracticeArea.DataSource = udtSPProfileBLL.GetArea.Values
            rbAddPracticeArea.DataValueField = "Area_ID"
            rbAddPracticeArea.DataTextField = "Area_Name"
            rbAddPracticeArea.DataBind()

            bindDistrict(ddlAddPracticeDistrict, String.Empty, False)

            rbAddPracticeArea.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
            ddlAddPracticeDistrict.Attributes.Add("onfocus", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")

            ddlAddHealthProf.DataSource = udtSPProfileBLL.GetHealthProf

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

            ' -----------------------------------------------------------------------------------------

            ddlAddHealthProf.DataValueField = "ServiceCategoryCode"
            If Session("language") = "zh-tw" Then
                ddlAddHealthProf.DataTextField = "ServiceCategoryDescChi"
            Else
                ddlAddHealthProf.DataTextField = "ServiceCategoryDesc"
            End If

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            ddlAddHealthProf.DataBind()

            'Structure Address
            ibtnAddPracticeSearchAddress.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")
            ibtnAddClearPracticeSearchAddress.Attributes.Add("onclick", "javascript:getGridviewIndex('" & e.Row.RowIndex & "');")

        End If

    End Sub

    Private Sub gvPracticeInfo_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvPracticeInfo.RowCancelingEdit
        'Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Common.Component.LogID.LOG00001) ''Begin Writing Audit Log

        'udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)
        'If udtServiceProviderBLL.Exist Then
        '    udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
        'Else
        '    udtAuditLogEntry.AddDescripton("SPID", "")
        'End If
        'udtAuditLogEntry.AddDescripton("Practice", (Me.gvPracticeInfo.EditIndex + 1).ToString)
        'udtAuditLogEntry.WriteStartLog("Cancel save practice:")

        msgBox.Visible = False

        gvPracticeInfo.EditIndex = -1
        gvPracticeInfo.DataSource = udtPracticeBLL.GetPracticeCollection.Values
        gvPracticeInfo.DataBind()
        gvPracticeInfo.ShowFooter = False

        LoadPracticeOthersInfo()
        SetPracticeAdditionalInfo(False)

        ' udtAuditLogEntry.WriteStartLog("Cancel save practice Completed.")

    End Sub

    Private Sub gvPracticeInfo_RowCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCommandEventArgs) Handles gvPracticeInfo.RowCommand
        If e.CommandName.Equals("Add") Then
            Dim udtAuditLogEntrySave As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
            msgBox.Visible = False

            Dim footerRow As GridViewRow
            footerRow = gvPracticeInfo.FooterRow

            Dim noOfRow As Integer
            If IsNothing(udtServiceProviderBLL.GetSP.PracticeList) Then
                noOfRow = 1
            Else
                noOfRow = udtServiceProviderBLL.GetSP.PracticeList.Count + 1
            End If

            Dim ddlAddPracticeMOName As DropDownList = footerRow.FindControl("ddlAddPracticeMOName")
            Dim txtAddPracticeName As TextBox = footerRow.FindControl("txtAddPracticeName")
            Dim txtAddPracticeNameChi As TextBox = footerRow.FindControl("txtAddPracticeNameChi")
            Dim txtAddPracticeRoom As TextBox = footerRow.FindControl("txtAddPracticeRoom")
            Dim txtAddPracticeFloor As TextBox = footerRow.FindControl("txtAddPracticeFloor")
            Dim txtAddPracticeBlock As TextBox = footerRow.FindControl("txtAddPracticeBlock")
            Dim txtAddPracticeBuilding As TextBox = footerRow.FindControl("txtAddPracticeBuilding")
            Dim txtAddPracticeBuildingChi As TextBox = footerRow.FindControl("txtAddPracticeBuildingChi")
            Dim hfAddPracticeAddressCode As HiddenField = footerRow.FindControl("hfAddPracticeAddressCode")
            Dim ddlAddPracticeDistrict As DropDownList = footerRow.FindControl("ddlAddPracticeDistrict")
            Dim rbAddPracticeArea As RadioButtonList = footerRow.FindControl("rbAddPracticeArea")
            Dim txtAddPracticePhone As TextBox = footerRow.FindControl("txtAddPracticePhone")
            Dim ddlAddHealthProf As DropDownList = footerRow.FindControl("ddlAddHealthProf")
            Dim txtAddRegCode As TextBox = footerRow.FindControl("txtAddRegCode")

            Dim imgAddPracticeMONameAlert As Image = footerRow.FindControl("imgAddPracticeMONameAlert")
            Dim imgAddPracticeNameAlert As Image = footerRow.FindControl("imgAddPracticeNameAlert")
            Dim imgAddPracticeBuildingAlert As Image = footerRow.FindControl("imgAddPracticeBuildingAlert")
            Dim imgAddPRacticeDistrcitAlert As Image = footerRow.FindControl("imgAddPRacticeDistrcitAlert")
            Dim imgAddPracticeAreaAlert As Image = footerRow.FindControl("imgAddPracticeAreaAlert")
            Dim imgAddPracticePhoneAlert As Image = footerRow.FindControl("imgAddPracticePhoneAlert")
            Dim imgAddHealthProfAlert As Image = footerRow.FindControl("imgAddHealthProfAlert")
            Dim imgAddRegCodeAlert As Image = footerRow.FindControl("imgAddRegCodeAlert")

            imgAddPracticeMONameAlert.Visible = False
            imgAddPracticeNameAlert.Visible = False
            imgAddPracticeBuildingAlert.Visible = False
            imgAddPRacticeDistrcitAlert.Visible = False
            imgAddPracticeAreaAlert.Visible = False
            imgAddPracticePhoneAlert.Visible = False
            imgAddHealthProfAlert.Visible = False
            imgAddRegCodeAlert.Visible = False

            'Check the MO selection
            SM = Validator.chkPracticeMOName(ddlAddPracticeMOName.SelectedValue.Trim)
            If Not IsNothing(SM) Then
                imgAddPracticeMONameAlert.Visible = True
                msgBox.AddMessage(SM, "%s", noOfRow)
            End If

            'Check the name of practice
            SM = Validator.chkPracticeName(txtAddPracticeName.Text.Trim)
            If Not IsNothing(SM) Then
                imgAddPracticeNameAlert.Visible = True
                msgBox.AddMessage(SM, "%s", noOfRow)
            End If

            'Check the address of practice
            SM = Validator.chkPracticeAddress(txtAddPracticeBuilding.Text.Trim, ddlAddPracticeDistrict.SelectedValue.Trim, rbAddPracticeArea.SelectedValue.Trim)
            If Not SM Is Nothing Then
                If Validator.IsEmpty(txtAddPracticeBuilding.Text.Trim) Then
                    imgAddPracticeBuildingAlert.Visible = True
                End If

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                'If Validator.IsEmpty(ddlAddPracticeDistrict.SelectedValue.Trim) OrElse _
                '   ddlAddPracticeDistrict.SelectedValue.Trim.Equals(".H") OrElse _
                '   ddlAddPracticeDistrict.SelectedValue.Trim.Equals(".K") OrElse _
                '   ddlAddPracticeDistrict.SelectedValue.Trim.Equals(".N") Then
                If Validator.IsEmpty(ddlAddPracticeDistrict.SelectedValue.Trim) OrElse _
                   ddlAddPracticeDistrict.SelectedValue.Trim.StartsWith(".") Then
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]

                    imgAddPRacticeDistrcitAlert.Visible = True
                End If

                If Validator.IsEmpty(rbAddPracticeArea.SelectedValue.Trim) Then
                    imgAddPracticeAreaAlert.Visible = True
                End If

                msgBox.AddMessage(SM, "%s", noOfRow)
            End If


            'check the phone of practice
            SM = Validator.chkPracticeTel(txtAddPracticePhone.Text.Trim)
            If Not IsNothing(SM) Then
                imgAddPracticePhoneAlert.Visible = True
                msgBox.AddMessage(SM, "%s", noOfRow)
            End If

            'Check the type of Health Profession
            SM = Validator.chkHealthProf(ddlAddHealthProf.SelectedValue)
            If Not SM Is Nothing Then
                imgAddHealthProfAlert.Visible = True
                msgBox.AddMessage(SM, "%s", noOfRow)
            End If

            'Check the profession registration no.
            SM = Validator.chkRegCode(txtAddRegCode.Text.Trim)
            If Not SM Is Nothing Then
                imgAddRegCodeAlert.Visible = True
                msgBox.AddMessage(SM, "%s", noOfRow)
            End If

            udtAuditLogEntrySave.AddDescripton("ERN", Me.hfERN.Value.Trim)
            If udtServiceProviderBLL.Exist Then
                udtAuditLogEntrySave.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
            Else
                udtAuditLogEntrySave.AddDescripton("SPID", "")
            End If
            udtAuditLogEntrySave.AddDescripton("Practice No", udtServiceProviderBLL.GetSP.PracticeList.Count + 1)
            udtAuditLogEntrySave.WriteStartLog(Common.Component.LogID.LOG00017, "Add Practice")


            If msgBox.GetCodeTable.Rows.Count = 0 Then
                msgBox.Visible = False

                Dim intAddressCode As Nullable(Of Integer)

                If hfAddPracticeAddressCode.Value.Trim.Equals(String.Empty) Then
                    intAddressCode = Nothing
                Else
                    intAddressCode = CInt(hfAddPracticeAddressCode.Value.Trim)
                End If

                'Add new practice information and get latest version of Bank Acct record with practice information
                Try

                    Dim udtPractice As PracticeModel = New PracticeModel

                    With udtPractice
                        .MODisplaySeq = ddlAddPracticeMOName.SelectedValue.Trim
                        .EnrolRefNo = hfERN.Value.Trim
                        .SPID = udtServiceProviderBLL.GetSP.SPID
                        .PracticeName = txtAddPracticeName.Text.Trim
                        .PracticeNameChi = txtAddPracticeNameChi.Text.Trim

                        If udtServiceProviderBLL.Exist Then
                            .SPID = udtServiceProviderBLL.GetSP.SPID
                        End If

                        If intAddressCode.HasValue Then
                            .PracticeAddress = New AddressModel(txtAddPracticeRoom.Text.Trim, txtAddPracticeFloor.Text.Trim, txtAddPracticeBlock.Text.Trim, _
                                                        String.Empty, String.Empty, String.Empty, intAddressCode)
                        Else
                            .PracticeAddress = New AddressModel(txtAddPracticeRoom.Text.Trim, txtAddPracticeFloor.Text.Trim, txtAddPracticeBlock.Text.Trim, _
                                                        txtAddPracticeBuilding.Text.Trim, txtAddPracticeBuildingChi.Text.Trim, ddlAddPracticeDistrict.SelectedValue.Trim, intAddressCode)
                        End If

                        .PhoneDaytime = txtAddPracticePhone.Text.Trim
                    End With

                    If udtSPProfileBLL.AddPracticeProfessionalToStaging(udtPractice, ddlAddHealthProf.SelectedValue.Trim, txtAddRegCode.Text.Trim, hfTableLocation.Value.Trim) Then


                        udtAuditLogEntrySave.WriteEndLog(Common.Component.LogID.LOG00018, "Add Practice Completed")


                        If udtSPProfileBLL.GetServiceProviderProfile(hfERN.Value.Trim, TableLocation.Staging) Then
                            hfTableLocation.Value = TableLocation.Staging

                            gvPracticeInfo.ShowFooter = False
                            BindSPProfile()

                            Me.tabBankAcctInfo.Visible = True
                            Me.tablSchemeInfo.Visible = True

                        End If
                    End If
                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then
                        Dim strmsg As String
                        strmsg = eSQL.Message

                        If Not strmsg.Trim.Equals(String.Empty) Then
                            SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                            msgBox.AddMessage(SM)
                            If msgBox.GetCodeTable.Rows.Count = 0 Then
                                msgBox.Visible = False
                            Else
                                'msgBox.BuildMessageBox("UpdateFail")

                                msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntrySave, Common.Component.LogID.LOG00019, "Add Practice Fail.")
                            End If
                        Else
                            udtAuditLogEntrySave.WriteEndLog(Common.Component.LogID.LOG00019, "Add Practice Fail.")
                        End If

                    Else
                        Throw eSQL
                    End If

                Catch ex As Exception
                    Throw ex
                End Try

            Else
                'msgBox.BuildMessageBox("ValidationFail")
                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntrySave, Common.Component.LogID.LOG00019, "Add Practice Fail.")
            End If

        ElseIf e.CommandName = "CancelAdd" Then
            msgBox.Visible = False
            If udtPracticeBLL.GetPracticeCollection.Count = 0 Then
                gvPracticeInfo.ShowFooter = True
                gvPracticeInfo.DataSource = udtSPProfileBLL.EmptyPracticeCollection.Values
                gvPracticeInfo.DataBind()
            Else
                gvPracticeInfo.ShowFooter = False

                gvPracticeInfo.DataSource = udtPracticeBLL.GetPracticeCollection.Values
                gvPracticeInfo.DataBind()

                ibtnPracticePageChecked.Enabled = True
                ibtnPracticePageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedBtn")

                ibtnPracticeAdd.Enabled = True
                ibtnPracticeAdd.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "AddSBtn")
            End If

            LoadPracticeOthersInfo()

            ' CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]            
            'If Me.ibtnEHRSSEdit.Visible Then
            '    SetPracticeAdditionalInfo(False)
            'Else
            '    SetPracticeAdditionalInfo(True)
            'End If

            SetPracticeAdditionalInfo(False)
            ' CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]

        End If
    End Sub

    Private Sub gvPracticeInfo_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvPracticeInfo.RowEditing
        gvPracticeInfo.ShowFooter = False
        gvPracticeInfo.EditIndex = e.NewEditIndex
        gvPracticeInfo.DataSource = udtPracticeBLL.GetPracticeCollection.Values
        gvPracticeInfo.DataBind()

        Me.rboHadJoinedEHRSS.Enabled = False
        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
        ' --------------------------------------------------------------------------------------------------------------------------------
        Me.rboJoinPCD.Enabled = False
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

        ibtnEHRSSEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
        ibtnEHRSSEdit.Enabled = False
        ibtnEHRSSSave.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SaveSDisableBtn")
        ibtnEHRSSSave.Enabled = False
        ibtnEHRSSCancel.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CancelSDisableBtn")
        ibtnEHRSSCancel.Enabled = False

    End Sub

    Private Sub gvPracticeInfo_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvPracticeInfo.RowUpdating
        gvPracticeInfo.ShowFooter = False
        Dim row As GridViewRow = gvPracticeInfo.Rows(e.RowIndex)

        msgBox.Visible = False

        If Not row Is Nothing Then
            Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log

            Dim lblEditPracticeDispalySeq As Label = row.FindControl("lblEditPracticeDispalySeq")

            Dim ddlEditPracticeMOName As DropDownList = row.FindControl("ddlEditPracticeMOName")
            Dim txtEditPracticeName As TextBox = row.FindControl("txtEditPracticeName")
            Dim txtEditPracticeNameChi As TextBox = row.FindControl("txtEditPracticeNameChi")
            Dim txtEditPracticeRoom As TextBox = row.FindControl("txtEditPracticeRoom")
            Dim txtEditPracticeFloor As TextBox = row.FindControl("txtEditPracticeFloor")
            Dim txtEditPracticeBlock As TextBox = row.FindControl("txtEditPracticeBlock")
            Dim txtEditPracticeBuilding As TextBox = row.FindControl("txtEditPracticeBuilding")
            Dim txtEditPracticeBuildingChi As TextBox = row.FindControl("txtEditPracticeBuildingChi")
            Dim hfEditPracticeAddressCode As HiddenField = row.FindControl("hfEditPracticeAddressCode")
            Dim ddlEditPracticeDistrict As DropDownList = row.FindControl("ddlEditPracticeDistrict")
            Dim rbEditPracticeArea As RadioButtonList = row.FindControl("rbEditPracticeArea")
            Dim txtEditPhone As TextBox = row.FindControl("txtEditPhone")
            Dim ddlEditHealthProf As DropDownList = row.FindControl("ddlEditHealthProf")
            Dim txtEditRegCode As TextBox = row.FindControl("txtEditRegCode")

            Dim imgEditPracticeMONameAlert As Image = row.FindControl("imgEditPracticeMONameAlert")
            Dim imgEditPracticeNameAlert As Image = row.FindControl("imgEditPracticeNameAlert")
            Dim imgEditPracticeBuildingAlert As Image = row.FindControl("imgEditPracticeBuildingAlert")
            Dim imgEditPRacticeDistrcitAlert As Image = row.FindControl("imgEditPRacticeDistrcitAlert")
            Dim imgEditPracticeAreaAlert As Image = row.FindControl("imgEditPracticeAreaAlert")
            Dim imgEditPhoneAlert As Image = row.FindControl("imgEditPhoneAlert")
            Dim imgEditHealthProfAlert As Image = row.FindControl("imgEditHealthProfAlert")
            Dim imgEditRegCodeAlert As Image = row.FindControl("imgEditRegCodeAlert")

            imgEditPracticeMONameAlert.Visible = False
            imgEditPracticeNameAlert.Visible = False
            imgEditPracticeBuildingAlert.Visible = False
            imgEditPRacticeDistrcitAlert.Visible = False
            imgEditPracticeAreaAlert.Visible = False
            imgEditPhoneAlert.Visible = False
            imgEditHealthProfAlert.Visible = False
            imgEditRegCodeAlert.Visible = False

            udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)

            If udtServiceProviderBLL.Exist Then
                udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
            Else
                udtAuditLogEntry.AddDescripton("SPID", "")
            End If

            udtAuditLogEntry.AddDescripton("Practice No", lblEditPracticeDispalySeq.Text.Trim)
            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00014, "Save practice")

            'Check the MO selection
            SM = Validator.chkPracticeMOName(ddlEditPracticeMOName.SelectedValue.Trim)
            If Not IsNothing(SM) Then
                imgEditPracticeMONameAlert.Visible = True
                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If

            'Check the name of practice
            SM = Validator.chkPracticeName(txtEditPracticeName.Text.Trim)
            If Not IsNothing(SM) Then
                imgEditPracticeNameAlert.Visible = True
                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If

            'Check the address of practice
            SM = Validator.chkPracticeAddress(txtEditPracticeBuilding.Text.Trim, ddlEditPracticeDistrict.SelectedValue.Trim, rbEditPracticeArea.SelectedValue.Trim)
            If Not SM Is Nothing Then
                If Validator.IsEmpty(txtEditPracticeBuilding.Text.Trim) Then
                    imgEditPracticeBuildingAlert.Visible = True
                End If

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                'If Validator.IsEmpty(ddlEditPracticeDistrict.SelectedValue.Trim) OrElse _
                '   ddlEditPracticeDistrict.SelectedValue.Trim.Equals(".H") OrElse _
                '   ddlEditPracticeDistrict.SelectedValue.Trim.Equals(".K") OrElse _
                '   ddlEditPracticeDistrict.SelectedValue.Trim.Equals(".N") Then
                If Validator.IsEmpty(ddlEditPracticeDistrict.SelectedValue.Trim) OrElse _
                   ddlEditPracticeDistrict.SelectedValue.Trim.StartsWith(".") Then
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]

                    imgEditPRacticeDistrcitAlert.Visible = True
                End If

                If Validator.IsEmpty(rbEditPracticeArea.SelectedValue.Trim) Then
                    imgEditPracticeAreaAlert.Visible = True
                End If

                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If

            'Check the phone of practice
            SM = Validator.chkPracticeTel(txtEditPhone.Text.Trim)
            If Not IsNothing(SM) Then
                imgEditPhoneAlert.Visible = True
                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If


            'Check the type of Health Profession
            If Not IsNothing(ddlEditHealthProf) AndAlso ddlEditHealthProf.Visible Then
                SM = Validator.chkHealthProf(ddlEditHealthProf.SelectedValue)
                If Not SM Is Nothing Then
                    imgEditHealthProfAlert.Visible = True
                    msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
                Else
                    ' CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]
                    ' Check if the practice scheme eligible for the selected Profession

                    Dim udtPractice As New PracticeModel
                    udtPractice = udtPracticeBLL.GetPracticeCollection.Item(CInt(lblEditPracticeDispalySeq.Text.Trim))

                    If Not udtSPProfileBLL.CheckSchemeEligibleForHealthProf(ddlEditHealthProf.SelectedValue, udtPractice.PracticeSchemeInfoList) Then
                        imgEditHealthProfAlert.Visible = True
                        msgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00395, "%s", (row.RowIndex + 1).ToString)
                    End If
                    ' CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]
                End If
            End If


            'Check the profession registration no.
            SM = Validator.chkRegCode(txtEditRegCode.Text.Trim)
            If Not SM Is Nothing Then
                imgEditRegCodeAlert.Visible = True
                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If

            If msgBox.GetCodeTable.Rows.Count = 0 Then
                msgBox.Visible = False

                Dim intAddressCode As Nullable(Of Integer)

                If hfEditPracticeAddressCode.Value.Trim.Equals(String.Empty) Then
                    intAddressCode = Nothing
                Else
                    intAddressCode = CInt(hfEditPracticeAddressCode.Value.Trim)
                End If

                'Update practice record and get latest version of Bank Acct record with practice information
                Try
                    Dim udtPractice As New PracticeModel
                    udtPracticeBLL.Clone(udtPractice, udtPracticeBLL.GetPracticeCollection.Item(CInt(lblEditPracticeDispalySeq.Text.Trim)))

                    With udtPractice
                        .MODisplaySeq = ddlEditPracticeMOName.SelectedValue.Trim
                        .PracticeName = txtEditPracticeName.Text.Trim
                        .PracticeNameChi = txtEditPracticeNameChi.Text.Trim

                        If intAddressCode.HasValue Then
                            .PracticeAddress = New AddressModel(txtEditPracticeRoom.Text.Trim, txtEditPracticeFloor.Text.Trim, txtEditPracticeBlock.Text.Trim, _
                                                        String.Empty, String.Empty, String.Empty, intAddressCode)
                        Else
                            .PracticeAddress = New AddressModel(txtEditPracticeRoom.Text.Trim, txtEditPracticeFloor.Text.Trim, txtEditPracticeBlock.Text.Trim, _
                                                        txtEditPracticeBuilding.Text.Trim, txtEditPracticeBuildingChi.Text.Trim, ddlEditPracticeDistrict.SelectedValue.Trim, intAddressCode)
                        End If

                        .PhoneDaytime = txtEditPhone.Text.Trim
                    End With


                    If udtSPProfileBLL.UpdatePracticeProfessionalInStaging(udtPractice, ddlEditHealthProf.SelectedValue.Trim, txtEditRegCode.Text.Trim, hfTableLocation.Value.Trim) Then


                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00015, "Save practice Completed.")

                        If udtSPProfileBLL.GetServiceProviderProfile(hfERN.Value.Trim, TableLocation.Staging) Then
                            hfTableLocation.Value = TableLocation.Staging

                            BindSPProfile()
                        End If
                    End If

                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then
                        Dim strmsg As String
                        strmsg = eSQL.Message

                        If Not strmsg.Trim.Equals(String.Empty) Then
                            SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                            msgBox.AddMessage(SM)
                            If msgBox.GetCodeTable.Rows.Count = 0 Then
                                msgBox.Visible = False
                            Else
                                'msgBox.BuildMessageBox("UpdateFail")
                                msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00016, "Save practice Fail.")
                            End If
                        Else
                            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00016, "Save practice Fail.")
                        End If

                        'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                    Else
                        Throw eSQL
                    End If

                Catch ex As Exception
                    Throw ex
                End Try

            Else
                'msgBox.BuildMessageBox("ValidationFail")
                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00016, "Save practice Fail.")
            End If

        End If

    End Sub

    '

    Protected Sub ddlHealthProf_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim blnAskHadJoinedEHRSSProfCode As Boolean = False

        Dim intDisplaySeq As Integer = -1

        Dim lblDisplaySeq As Label

        Dim ddlHealthProf As DropDownList = CType(sender, DropDownList)
        lblEHRSSStatement.Visible = False

        If udtSPProfileBLL.AskHadJoinedEHRSSProfCode(ddlHealthProf.SelectedValue.Trim) Then
            blnAskHadJoinedEHRSSProfCode = True
        End If

        If gvPracticeInfo.EditIndex > -1 Then
            lblDisplaySeq = CType(gvPracticeInfo.Rows(gvPracticeInfo.EditIndex).FindControl("lblEditPracticeDispalySeq"), Label)
            intDisplaySeq = CInt(lblDisplaySeq.Text.Trim)
        End If

        If blnAskHadJoinedEHRSSProfCode Then
            ' CRE15-018 Remove PPIePR Enrolment [Start][Winnie]
            'setPPIePRPanel("Edit", blnAskWillJoinPPIePRProfCode)
            'setEHRSSPanel("Edit")
            ' CRE15-018 Remove PPIePR Enrolment [End][Winnie]
        Else
            If udtSPProfileBLL.IsOtherProfCanJoinEHRSS(intDisplaySeq) Then
                setEHRSSPanel("ReadOnly")
            Else
                Me.rboHadJoinedEHRSS.ClearSelection()

                setEHRSSPanel("ReadOnly")
                Me.ibtnEHRSSEdit.Enabled = False
                ibtnEHRSSEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                lblEHRSSStatement.Visible = True

                ' CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]
                imgHadJoinedEHRSSAlert.Visible = False
                ' CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]
            End If
        End If
    End Sub

    '

    Protected Sub ibtnPracticeDelete_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim ibtnPracticeDelete As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = ibtnPracticeDelete.NamingContainer

        If Not row Is Nothing Then
            Dim lblPracticeDispalySeq As Label = CType(Me.gvPracticeInfo.Rows(row.RowIndex).FindControl("lblPracticeDispalySeq"), Label)

            Me.hfDeletePractice.Value = lblPracticeDispalySeq.Text.Trim

            SM = New Common.ComObject.SystemMessage("010101", "Q", "00001")

            Dim strDeleteMessage As String = SM.GetMessage()
            strDeleteMessage = strDeleteMessage.Replace("%s", lblPracticeDispalySeq.Text.Trim)

            lblDeletePractice.Text = strDeleteMessage

            Me.ModalPopupExtenderDeletePractice.Show()

        End If
    End Sub

    Protected Sub ibtnDeletePracticeConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log

        ModalPopupExtenderDeletePractice.Hide()

        Dim intPracticeDisplaySeq As Integer

        intPracticeDisplaySeq = CInt(Me.hfDeletePractice.Value.Trim)


        udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)
        If udtServiceProviderBLL.Exist Then
            udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
        Else
            udtAuditLogEntry.AddDescripton("SPID", "")
        End If

        udtAuditLogEntry.AddDescripton("Practice No", intPracticeDisplaySeq)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00043, "Delete Practice")

        Try
            Dim udtPractice As New PracticeModel
            udtPracticeBLL.Clone(udtPractice, udtPracticeBLL.GetPracticeCollection.Item(intPracticeDisplaySeq))

            If udtSPProfileBLL.DeletePracticeBankAcct(udtPractice, hfTableLocation.Value.Trim) Then

                Me.hfDeletePractice.Value = String.Empty

                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00044, "Delete Practice Completed.")

                If udtSPProfileBLL.GetServiceProviderProfile(hfERN.Value.Trim, TableLocation.Staging) Then
                    hfTableLocation.Value = TableLocation.Staging

                    BindSPProfile()

                    If udtServiceProviderBLL.Exist Then
                        If udtServiceProviderBLL.GetSP.PracticeList.Count = 0 Then
                            tabBankAcctInfo.Visible = False
                            Me.tablSchemeInfo.Visible = False
                            ibtnPracticeAdd_Click(Nothing, Nothing)
                        Else

                            'Dim intEditPracticeIndex As Integer = setEditIndexPracticeGridview()
                            'gvPracticeInfo.EditIndex = intEditPracticeIndex
                            'gvPracticeInfo.DataBind()

                            'Dim intEditIndex As Integer = setEditIndexBankGridview()
                            'gvBankInfo.EditIndex = intEditIndex
                            'gvBankInfo.DataBind()

                            'Dim intEditSchemeIndex As Integer = setEditIndexSchemeGridview()
                            'gvSchemeInfo.EditIndex = intEditSchemeIndex
                            'gvSchemeInfo.DataBind()

                        End If

                    End If


                End If
            End If
        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                If Not strmsg.Trim.Equals(String.Empty) Then
                    SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                    msgBox.AddMessage(SM)
                    If msgBox.GetCodeTable.Rows.Count = 0 Then
                        msgBox.Visible = False
                    Else
                        ' msgBox.BuildMessageBox("UpdateFail")
                        msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00045, "Delete Practice Fail.")
                    End If
                Else
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00045, "Delete Practice Fail.")
                End If


                'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
            Else
                Throw
            End If

        Catch ex As Exception
            Throw
        End Try
    End Sub

    Protected Sub ibtnDeletePracticeCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.hfDeletePractice.Value = String.Empty
        ModalPopupExtenderDeletePractice.Hide()
    End Sub

    '

    Protected Sub ibtnDuplicatePractice_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim ibtnDuplicatePractice As ImageButton = CType(sender, ImageButton)
        Dim row As GridViewRow = ibtnDuplicatePractice.NamingContainer

        If Not row Is Nothing Then
            Dim lblPracticeDispalySeq As Label

            If IsNothing(CType(row.FindControl("lblPracticeDispalySeq"), Label)) Then
                lblPracticeDispalySeq = CType(row.FindControl("lblEditPracticeDispalySeq"), Label)
            Else
                lblPracticeDispalySeq = CType(row.FindControl("lblPracticeDispalySeq"), Label)
            End If

            MOPracticeLists1.buildPracticeObject(udtServiceProviderBLL.GetSP.PracticeList, CInt(lblPracticeDispalySeq.Text.Trim), Me.hfTableLocation.Value.Trim)

            Me.ModalPopupExtenderDuplicated.Show()
        End If
    End Sub

#End Region

#Region "Tab - Practice Information - EHRSS"

    Private Sub SetPracticeAdditionalInfo(ByVal blnRequestEdit As Boolean)
        If blnExisingHKID Then
            Me.ibtnEHRSSEdit.Visible = True
            Me.ibtnEHRSSSave.Visible = False
            Me.ibtnEHRSSCancel.Visible = False

            ibtnEHRSSEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
            ibtnEHRSSEdit.Enabled = False

            Me.rboHadJoinedEHRSS.Enabled = False

            lblEHRSSStatement.Visible = False

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me.rboJoinPCD.Enabled = False
            lblJoinPCDStatement.Visible = False
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

        Else
            Dim blnAskHadJoinedEHRSSProfCode As Boolean = False
            Dim blnAskJoinIVSS As Boolean = False
            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' --------------------------------------------------------------------------------------------------------------------------------
            Dim blnAskJoinPCD As Boolean = False
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' --------------------------------------------------------------------------------------------------------------------------------
            Dim blnAutoEditModel_EHRSS As Boolean = False
            Dim blnAutoEditModel_JoinPCD As Boolean = False
            'Dim blnAutoEditModel As Boolean = False
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]


            Dim udtProfessionalList As ProfessionalModelCollection
            Dim udtSP As ServiceProviderModel

            lblEHRSSStatement.Visible = False

            If udtServiceProviderBLL.Exist AndAlso udtProfessionalBLL.Exist Then
                udtSP = udtServiceProviderBLL.GetSP
                udtProfessionalList = udtProfessionalBLL.GetProfessionalCollection

                For Each udtProfessionalModel As ProfessionalModel In udtProfessionalList.Values

                    Dim strHealthProf As String = udtProfessionalModel.ServiceCategoryCode
                    If udtSPProfileBLL.AskHadJoinedEHRSSProfCode(strHealthProf.Trim) Then
                        blnAskHadJoinedEHRSSProfCode = True
                    End If

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                    ' --------------------------------------------------------------------------------------------------------------------------------
                    If udtProfessionalModel.Profession.AllowJoinPCD Then
                        blnAskJoinPCD = True
                    End If
                    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

                    If udtSPProfileBLL.AskJoinIVSS(strHealthProf.Trim) Then
                        blnAskJoinIVSS = True
                    End If
                Next

                If blnAskHadJoinedEHRSSProfCode Then
                    If rboHadJoinedEHRSS.SelectedValue.Equals(String.Empty) Or blnRequestEdit Then
                        blnAutoEditModel_EHRSS = True
                        rboHadJoinedEHRSS.Enabled = True
                    Else
                        rboHadJoinedEHRSS.Enabled = False
                    End If

                Else
                    ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [Start]
                    ' If do not select any profession, not to show statement
                    If udtProfessionalList.Count = 0 Then
                        lblEHRSSStatement.Visible = False
                    Else
                        lblEHRSSStatement.Visible = True
                    End If
                    ' CRE16-019 (To implement token sharing between eHS(S) and eHRSS) [End]

                    rboHadJoinedEHRSS.ClearSelection()
                    rboHadJoinedEHRSS.Enabled = False

                    ' CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]
                    imgHadJoinedEHRSSAlert.Visible = False
                    ' CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]
                End If

                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' --------------------------------------------------------------------------------------------------------------------------------
                If blnAskJoinPCD Then
                    If rboJoinPCD.SelectedValue.Equals(String.Empty) Or blnRequestEdit Then
                        blnAutoEditModel_JoinPCD = True
                        rboJoinPCD.Enabled = True
                    Else
                        rboJoinPCD.Enabled = False
                    End If
                    lblJoinPCDStatement.Visible = False
                Else
                    If udtProfessionalList.Count = 0 Then
                        lblJoinPCDStatement.Visible = False
                    Else
                        lblJoinPCDStatement.Visible = True
                    End If

                    rboJoinPCD.ClearSelection()
                    rboJoinPCD.Enabled = False

                    imgJoinPCDAlert.Visible = False
                End If
                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' --------------------------------------------------------------------------------------------------------------------------------
                ' Either need to answer eHRSS question or join PCD question
                If blnAutoEditModel_EHRSS Or blnAutoEditModel_JoinPCD Then
                    'If blnAutoEditModel Then
                    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
                    Me.ibtnEHRSSEdit.Visible = False
                    Me.ibtnEHRSSSave.Visible = True
                    Me.ibtnEHRSSCancel.Visible = True

                    ibtnEHRSSSave.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "SaveSBtn")
                    ibtnEHRSSSave.Enabled = True
                    ibtnEHRSSCancel.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "CancelSBtn")
                    ibtnEHRSSCancel.Enabled = True

                    ibtnPracticePageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                    ibtnPracticePageChecked.Enabled = False

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    If blnAskHadJoinedEHRSSProfCode Then
                        rboHadJoinedEHRSS.Enabled = True
                    End If

                    If blnAskJoinPCD Then
                        rboJoinPCD.Enabled = True
                    End If
                    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]
                Else
                    Me.ibtnEHRSSEdit.Visible = True
                    Me.ibtnEHRSSSave.Visible = False
                    Me.ibtnEHRSSCancel.Visible = False

                    ibtnEHRSSEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSBtn")
                    ibtnEHRSSEdit.Enabled = True
                End If
            End If
            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' --------------------------------------------------------------------------------------------------------------------------------
            If Not blnAskHadJoinedEHRSSProfCode And Not blnAskJoinPCD And Not blnAskJoinIVSS Then
                'If Not blnAskHadJoinedEHRSSProfCode And Not blnAskJoinIVSS Then

                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
                Me.ibtnEHRSSEdit.Visible = True
                Me.ibtnEHRSSSave.Visible = False
                Me.ibtnEHRSSCancel.Visible = False

                ibtnEHRSSEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                ibtnEHRSSEdit.Enabled = False
            End If
        End If

    End Sub

    Private Sub setEHRSSPanel(ByVal strMode As String)
        panEHRSS.Visible = True
        Select Case strMode
            Case "Edit"
                lblEHRSSStatement.Visible = False
                rboHadJoinedEHRSS.Enabled = True

                Me.ibtnEHRSSEdit.Visible = False
                Me.ibtnEHRSSSave.Visible = True
                Me.ibtnEHRSSCancel.Visible = True

                ibtnPracticePageChecked.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "PageCheckedDisableBtn")
                ibtnPracticePageChecked.Enabled = False

            Case "ReadOnly"
                Dim udtPracticeList As PracticeModelCollection = Nothing
                If udtPracticeBLL.Exist Then
                    udtPracticeList = udtPracticeBLL.GetPracticeCollection
                    If udtSPProfileBLL.HadJoinedEHRSS(udtPracticeList) Then
                        lblEHRSSStatement.Visible = False
                    Else
                        lblEHRSSStatement.Visible = True
                    End If
                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                    ' --------------------------------------------------------------------------------------------------------------------------------
                    If udtSPProfileBLL.CanJoinPCD(udtPracticeList) Then
                        lblJoinPCDStatement.Visible = False
                    Else
                        lblJoinPCDStatement.Visible = True
                    End If
                    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
                Else
                    lblEHRSSStatement.Visible = True
                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                    ' --------------------------------------------------------------------------------------------------------------------------------
                    lblJoinPCDStatement.Visible = True
                    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
                End If
                rboHadJoinedEHRSS.Enabled = False
                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' --------------------------------------------------------------------------------------------------------------------------------
                rboJoinPCD.Enabled = False
                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

                Me.ibtnEHRSSEdit.Visible = True
                Me.ibtnEHRSSSave.Visible = False
                Me.ibtnEHRSSCancel.Visible = False

                ibtnEHRSSEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSBtn")
                ibtnEHRSSEdit.Enabled = True
        End Select

    End Sub

    '

    Protected Sub ibtnEHRSSSave_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log

        udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)
        If udtServiceProviderBLL.Exist Then
            udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
        Else
            udtAuditLogEntry.AddDescripton("SPID", "")
        End If

        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00049, "Save eHRSS")

        msgBox.Visible = False
        imgHadJoinedEHRSSAlert.Visible = False
        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
        ' --------------------------------------------------------------------------------------------------------------------------------
        imgJoinPCDAlert.Visible = False
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

        If rboHadJoinedEHRSS.Enabled AndAlso rboHadJoinedEHRSS.SelectedValue.Equals(String.Empty) Then
            If Validator.IsEmpty(rboHadJoinedEHRSS.SelectedValue) Then
                ' Please answer "Is the Service Provider an authorized user of Electronic Health Record Sharing System (eHRSS)?"
                SM = New SystemMessage(strFuncCode, SeverityCode.SEVE, MsgCode.MSG00003)
            End If
            If Not SM Is Nothing Then
                imgHadJoinedEHRSSAlert.Visible = True
                msgBox.AddMessage(SM)
            End If
        End If

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
        ' --------------------------------------------------------------------------------------------------------------------------------
        If rboJoinPCD.Enabled AndAlso rboJoinPCD.SelectedValue.Equals(String.Empty) Then
            If Validator.IsEmpty(rboJoinPCD.SelectedValue) Then
                SM = New SystemMessage(strFuncCode, SeverityCode.SEVE, MsgCode.MSG00007)
            End If
            If Not SM Is Nothing Then
                imgJoinPCDAlert.Visible = True
                msgBox.AddMessage(SM)
            End If
        End If
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

        If msgBox.GetCodeTable.Rows.Count = 0 Then
            msgBox.Visible = False

            Try

                Dim udtSP As ServiceProviderModel = New ServiceProviderModel
                udtServiceProviderBLL.Clone(udtSP, udtServiceProviderBLL.GetSP)

                With udtSP
                    If rboHadJoinedEHRSS.SelectedValue.Equals(JoinEHRSSStatus.Yes) Then
                        .AlreadyJoinEHR = JoinEHRSSStatus.Yes
                    ElseIf rboHadJoinedEHRSS.SelectedValue.Equals(JoinEHRSSStatus.No) Then
                        .AlreadyJoinEHR = JoinEHRSSStatus.No
                    Else
                        .AlreadyJoinEHR = JoinEHRSSStatus.NA
                    End If

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                    ' --------------------------------------------------------------------------------------------------------------------------------
                    If rboJoinPCD.SelectedValue.Equals(JoinPCDStatus.Yes) Then
                        .JoinPCD = JoinPCDStatus.Yes
                    ElseIf rboJoinPCD.SelectedValue.Equals(JoinPCDStatus.Enrolled) Then
                        .JoinPCD = JoinPCDStatus.Enrolled
                    ElseIf rboJoinPCD.SelectedValue.Equals(JoinPCDStatus.No) Then
                        .JoinPCD = JoinPCDStatus.No
                    Else
                        .JoinPCD = JoinEHRSSStatus.NA
                    End If
                    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

                End With

                If udtSPProfileBLL.UpdatePracticeOtherInfo(udtSP, hfTableLocation.Value.Trim) Then
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00050, "Save eHRSS Completed.")

                    If udtSPProfileBLL.GetServiceProviderProfile(hfERN.Value.Trim, TableLocation.Staging) Then
                        hfTableLocation.Value = TableLocation.Staging
                        BindSPProfile()
                    End If

                End If

            Catch eSQL As SqlClient.SqlException
                If eSQL.Number = 50000 Then
                    Dim strmsg As String
                    strmsg = eSQL.Message

                    If Not strmsg.Trim.Equals(String.Empty) Then
                        SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                        msgBox.AddMessage(SM)
                        If msgBox.GetCodeTable.Rows.Count = 0 Then
                            msgBox.Visible = False
                        Else
                            'msgBox.BuildMessageBox("UpdateFail")

                            msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00051, "Save eHRSS Fail")
                        End If
                    Else
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00051, "Save eHRSS Fail")
                    End If

                Else
                    Throw
                End If

            Catch ex As Exception
                Throw
            End Try
        Else
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00051, "Save eHRSS Fail")
        End If

    End Sub

    Protected Sub ibtnEHRSSCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        LoadPracticeOthersInfo()
        SetPracticeAdditionalInfo(False)


    End Sub

    Protected Sub ibtnEHRSSEdit_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        LoadPracticeOthersInfo()
        SetPracticeAdditionalInfo(True)
    End Sub

#End Region

#Region "Tab - Bank Information"

    Private Sub gvBankInfo_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs) Handles gvBankInfo.RowCancelingEdit
        Me.msgBox.Visible = False
        gvBankInfo.EditIndex = -1
        gvBankInfo.DataSource = udtPracticeBLL.GetPracticeCollection.Values
        gvBankInfo.DataBind()
    End Sub

    Private Sub gvBankInfo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvBankInfo.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If (e.Row.RowState And DataControlRowState.Edit) > 0 Then
                'Edit Mode
                Dim txtEditBankCode As TextBox = e.Row.FindControl("txtEditBankCode")
                Dim txtEditBranchCode As TextBox = e.Row.FindControl("txtEditBranchCode")
                Dim txtEditBankAcc As TextBox = e.Row.FindControl("txtEditBankAcc")

                Dim hfEditBankAccNo As HiddenField = e.Row.FindControl("hfEditBankAccNo")

                Dim lblEditBankPracticeStatus As Label = e.Row.FindControl("lblEditBankPracticeStatus")

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]              
                Dim tBankAcc As HtmlControls.HtmlTableCell = e.Row.FindControl("tBankAcc")
                Dim txtEditBankAccNoFreeText As TextBox = e.Row.FindControl("txtEditBankAccNoFreeText")
                Dim chkEditIsFreeTextFormat As CheckBox = e.Row.FindControl("chkEditIsFreeTextFormat")

                'CRE16-003 (Disallow input Chinese Chars) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim txtEditBankOwner As TextBox = e.Row.FindControl("txtEditBankOwner")
                Dim txtEditBankOwnerFreeText As TextBox = e.Row.FindControl("txtEditBankOwnerFreeText")

                chkEditIsFreeTextFormat.Attributes.Add("onclick", "chkFreeTextChanged(this,'" + tBankAcc.ClientID + "', '" +
                                       txtEditBankCode.ClientID + "', '" + txtEditBranchCode.ClientID + "', '" +
                                       txtEditBankAcc.ClientID + "', '" +
                                       txtEditBankAccNoFreeText.ClientID + "', '" +
                                       txtEditBankOwner.ClientID + "', '" + txtEditBankOwnerFreeText.ClientID + "')")

                If chkEditIsFreeTextFormat.Checked Then
                    txtEditBankOwner.Style("display") = "none"
                    txtEditBankOwnerFreeText.Style.Remove("display")

                    'CRE17-013 (Extend bank account name to 300 chars) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    txtEditBankOwnerFreeText.Attributes.Add("onKeyUp", "return LimitLength(this, 300, event);")
                    'CRE17-013 (Extend bank account name to 300 chars) [End][Chris YIM]
                Else
                    txtEditBankOwner.Style.Remove("display")
                    txtEditBankOwnerFreeText.Style("display") = "none"

                    'CRE17-013 (Extend bank account name to 300 chars) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    txtEditBankOwner.Attributes.Add("onKeyUp", "return LimitLength(this, 300, event);")
                    'CRE17-013 (Extend bank account name to 300 chars) [End][Chris YIM]
                End If
                'CRE16-003 (Disallow input Chinese Chars) [End][Chris YIM]




                If chkEditIsFreeTextFormat.Checked Then
                    tBankAcc.Style("display") = "none"
                Else
                    txtEditBankAccNoFreeText.Style("display") = "none"
                End If
                'CRE13-019-02 Extend HCVS to China [End][Winnie]

                txtEditBankCode.Attributes.Add("onKeyDown", "TabNext(this,'down',3)")
                txtEditBankCode.Attributes.Add("onKeyUp", "TabNext(this,'up',3," + txtEditBranchCode.ClientID + ")")

                txtEditBranchCode.Attributes.Add("onKeyDown", "TabNext(this,'down',3)")
                txtEditBranchCode.Attributes.Add("onKeyUp", "TabNext(this,'up',3," + txtEditBankAcc.ClientID + ")")

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                If chkEditIsFreeTextFormat.Checked Then
                    ' I-CRE16-003 Fix XSS [Start][Lawrence]
                    txtEditBankAccNoFreeText.Text = AntiXssEncoder.HtmlEncode(hfEditBankAccNo.Value, True)
                    ' I-CRE16-003 Fix XSS [End][Lawrence]

                ElseIf Not hfEditBankAccNo.Value.Equals(String.Empty) Then
                    ' I-CRE16-003 Fix XSS [Start][Lawrence]
                    txtEditBankCode.Text = udtFormatter.splitBankAcct(AntiXssEncoder.HtmlEncode(hfEditBankAccNo.Value, True))(0)
                    txtEditBranchCode.Text = udtFormatter.splitBankAcct(AntiXssEncoder.HtmlEncode(hfEditBankAccNo.Value, True))(1)
                    txtEditBankAcc.Text = udtFormatter.splitBankAcct(AntiXssEncoder.HtmlEncode(hfEditBankAccNo.Value, True))(2)
                    ' I-CRE16-003 Fix XSS [End][Lawrence]
                End If
                'CRE13-019-02 Extend HCVS to China [End][Winnie]

                Select Case hfTableLocation.Value.Trim
                    Case TableLocation.Permanent

                        Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, lblEditBankPracticeStatus.Text.Trim, lblEditBankPracticeStatus.Text, String.Empty)
                    Case TableLocation.Staging

                        Status.GetDescriptionFromDBCode(PracticeStagingStatus.ClassCode, lblEditBankPracticeStatus.Text.Trim, lblEditBankPracticeStatus.Text, String.Empty)

                    Case TableLocation.Enrolment
                        lblEditBankPracticeStatus.Text = "Unprocessed"
                End Select
            Else
                Dim hfBankAcctStatus As HiddenField = e.Row.FindControl("hfBankAcctStatus")
                Dim ibtnBankEdit As ImageButton = e.Row.FindControl("ibtnBankEdit")
                Dim lblBankPracticeStatus As Label = e.Row.FindControl("lblBankPracticeStatus")

                If Not IsNothing(hfBankAcctStatus) AndAlso Not IsNothing(ibtnBankEdit) Then
                    Select Case hfTableLocation.Value.Trim
                        Case TableLocation.Permanent
                            ibtnBankEdit.Enabled = False
                            ibtnBankEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")


                            Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, lblBankPracticeStatus.Text.Trim, lblBankPracticeStatus.Text, String.Empty)

                        Case TableLocation.Staging
                            If strProgress.Equals(String.Empty) Then
                                If hfBankAcctStatus.Value.Trim.Equals(PracticeStagingStatus.Active) Then
                                    If gvBankInfo.EditIndex = -1 Then
                                        ibtnBankEdit.Enabled = True
                                        ibtnBankEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSBtn")
                                    Else
                                        ibtnBankEdit.Enabled = False
                                        ibtnBankEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                                    End If

                                Else
                                    ibtnBankEdit.Enabled = False
                                    ibtnBankEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                                End If
                            Else
                                ibtnBankEdit.Enabled = False
                                ibtnBankEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                            End If

                            Status.GetDescriptionFromDBCode(PracticeStagingStatus.ClassCode, lblBankPracticeStatus.Text.Trim, lblBankPracticeStatus.Text, String.Empty)

                        Case TableLocation.Enrolment
                            If blnExisingHKID Then
                                ibtnBankEdit.Enabled = False
                                ibtnBankEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                            Else
                                ibtnBankEdit.Enabled = True
                                ibtnBankEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSBtn")
                            End If

                            lblBankPracticeStatus.Text = "Unprocessed"
                    End Select
                End If

            End If
        End If

        Me.ApplyBankAccountChangeIndicator()

    End Sub

    Private Sub gvBankInfo_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles gvBankInfo.RowEditing
        gvBankInfo.EditIndex = e.NewEditIndex
        gvBankInfo.DataSource = udtPracticeBLL.GetPracticeCollection.Values
        gvBankInfo.DataBind()


    End Sub

    Private Sub gvBankInfo_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles gvBankInfo.RowUpdating
        msgBox.Visible = False

        Dim row As GridViewRow = gvBankInfo.Rows(e.RowIndex)

        If Not IsNothing(row) Then
            Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log

            Dim lblEditPracticeBankDispalySeq As Label = row.FindControl("lblEditPracticeBankDispalySeq")
            Dim hfEditBankDisplaySeq As HiddenField = row.FindControl("hfEditBankDisplaySeq")

            Dim txtEditBankName As TextBox = row.FindControl("txtEditBankName")
            Dim txtEditBranchName As TextBox = row.FindControl("txtEditBranchName")
            Dim txtEditBankCode As TextBox = row.FindControl("txtEditBankCode")
            Dim txtEditBranchCode As TextBox = row.FindControl("txtEditBranchCode")
            Dim txtEditBankAcc As TextBox = row.FindControl("txtEditBankAcc")
            Dim txtEditBankOwner As TextBox = row.FindControl("txtEditBankOwner")
            'CRE16-003 (Disallow input Chinese Chars) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Dim lblEditBankOwnerText As Label = row.FindControl("lblEditBankOwnerText")
            Dim txtEditBankOwnerFreeText As TextBox = row.FindControl("txtEditBankOwnerFreeText")
            'CRE16-003 (Disallow input Chinese Chars) [End][Chris YIM]

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Dim tBankAcc As HtmlControls.HtmlTableCell = row.FindControl("tBankAcc")
            Dim chkEditIsFreeTextFormat As CheckBox = row.FindControl("chkEditIsFreeTextFormat")
            Dim txtEditBankAccNoFreeText As TextBox = row.FindControl("txtEditBankAccNoFreeText")

            If chkEditIsFreeTextFormat.Checked Then
                tBankAcc.Style("display") = "none"
                txtEditBankAccNoFreeText.Style("display") = "block"
            Else
                tBankAcc.Style("display") = "block"
                txtEditBankAccNoFreeText.Style("display") = "none"
            End If
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            'CRE16-003 (Disallow input Chinese Chars) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If chkEditIsFreeTextFormat.Checked Then
                txtEditBankOwner.Style("display") = "none"
                txtEditBankOwnerFreeText.Style.Remove("display")
            Else
                txtEditBankOwner.Style.Remove("display")
                txtEditBankOwnerFreeText.Style("display") = "none"
            End If
            'CRE16-003 (Disallow input Chinese Chars) [End][Chris YIM]

            Dim imgEditBankNameAlert As Image = row.FindControl("imgEditBankNameAlert")
            Dim imgEditBranchNameAlert As Image = row.FindControl("imgEditBranchNameAlert")
            Dim imgEditBankAccAlert As Image = row.FindControl("imgEditBankAccAlert")
            Dim imgEditBankOwnerAlert As Image = row.FindControl("imgEditBankOwnerAlert")

            imgEditBankNameAlert.Visible = False
            imgEditBranchNameAlert.Visible = False
            imgEditBankAccAlert.Visible = False
            imgEditBankOwnerAlert.Visible = False

            Dim intBankDisplaySeq As Integer

            If hfEditBankDisplaySeq.Value.Trim.Equals(String.Empty) Then
                intBankDisplaySeq = 1
            Else
                intBankDisplaySeq = CInt(hfEditBankDisplaySeq.Value.Trim)
            End If

            udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)
            If udtServiceProviderBLL.Exist Then
                udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
            Else
                udtAuditLogEntry.AddDescripton("SPID", "")
            End If
            udtAuditLogEntry.AddDescripton("Practice No", lblEditPracticeBankDispalySeq.Text.Trim)


            udtAuditLogEntry.AddDescripton("Bank no", intBankDisplaySeq)

            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00020, "Save bank account")


            'Check Bank Name
            SM = Validator.chkBankName(txtEditBankName.Text.Trim)
            If Not IsNothing(SM) Then
                imgEditBankNameAlert.Visible = True
                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If

            'Check Branch Name
            SM = Validator.chkBranchName(txtEditBranchName.Text.Trim)
            If Not IsNothing(SM) Then
                imgEditBranchNameAlert.Visible = True
                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If

            'Check Bank Acct
            SM = Validator.chkBankAccount(txtEditBankCode.Text.Trim, txtEditBranchCode.Text.Trim, txtEditBankAcc.Text.Trim,
                                          txtEditBankAccNoFreeText.Text.Trim, chkEditIsFreeTextFormat.Checked)
            If Not IsNothing(SM) Then
                imgEditBankAccAlert.Visible = True
                msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
            End If

            'Check Bank Acct Owner
            'CRE16-003 (Disallow input Chinese Chars) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            If chkEditIsFreeTextFormat.Checked Then
                SM = Validator.chkBankOwner(txtEditBankOwnerFreeText.Text.Trim, chkEditIsFreeTextFormat.Checked)
            Else
                SM = Validator.chkBankOwner(txtEditBankOwner.Text.Trim, chkEditIsFreeTextFormat.Checked)
            End If

            If Not IsNothing(SM) Then
                imgEditBankOwnerAlert.Visible = True
                If chkEditIsFreeTextFormat.Checked Then
                    msgBox.AddMessage(SM, "%s", (row.RowIndex + 1).ToString)
                Else
                    msgBox.AddMessage(SM, New String() {"%t", "%s"}, New String() {lblEditBankOwnerText.Text, (row.RowIndex + 1).ToString})
                End If
            End If
            'CRE16-003 (Disallow input Chinese Chars) [End][Chris YIM]

            If msgBox.GetCodeTable.Rows.Count = 0 Then
                msgBox.Visible = False

                'Update Bank Acct Record
                Try

                    Dim blnSuccess As Boolean = False
                    Dim udtBankAcct As BankAcctModel = New BankAcctModel

                    ' Use the Practice Display Seq and Bank Display Seq to search for a bank
                    Dim udtBankAcctBLL As New BankAcctBLL
                    Dim udtTargetBank As BankAcctModel = udtBankAcctBLL.GetBankAcctCollection.Item(lblEditPracticeBankDispalySeq.Text.Trim, intBankDisplaySeq)

                    ' If cannot find a target bank, add a new bank (actually intBankDisplaySeq is a shortcut, because a new bank must be at sequence 1)
                    If intBankDisplaySeq = 1 AndAlso IsNothing(udtTargetBank) Then
                        With udtBankAcct
                            .EnrolRefNo = Me.hfERN.Value.Trim
                            .SPID = udtServiceProviderBLL.GetSP.SPID
                            .DisplaySeq = intBankDisplaySeq
                            .SpPracticeDisplaySeq = CInt(lblEditPracticeBankDispalySeq.Text.Trim)
                            .BankName = txtEditBankName.Text.Trim
                            .BranchName = txtEditBranchName.Text.Trim
                            'CRE16-003 (Disallow input Chinese Chars) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            If chkEditIsFreeTextFormat.Checked Then
                                .BankAcctOwner = txtEditBankOwnerFreeText.Text.Trim
                            Else
                                .BankAcctOwner = txtEditBankOwner.Text.Trim
                            End If
                            'CRE16-003 (Disallow input Chinese Chars) [End][Chris YIM]

                            .SubmitMethod = SubmitChannel.Paper
                            .Remark = String.Empty

                            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                            If chkEditIsFreeTextFormat.Checked Then
                                .BankAcctNo = txtEditBankAccNoFreeText.Text.Trim
                            Else
                                .BankAcctNo = udtFormatter.formatBankAcct(txtEditBankCode.Text.Trim, txtEditBranchCode.Text.Trim, txtEditBankAcc.Text.Trim)
                            End If
                            .IsFreeTextFormat = IIf(chkEditIsFreeTextFormat.Checked, YesNo.Yes, YesNo.No)
                            'CRE13-019-02 Extend HCVS to China [End][Winnie]
                        End With

                        blnSuccess = udtSPProfileBLL.AddBankAcctToStaging(udtBankAcct, Me.hfTableLocation.Value.Trim)

                    Else
                        udtBankAcctBLL.Clone(udtBankAcct, udtTargetBank)

                        With udtBankAcct
                            .SpPracticeDisplaySeq = CInt(lblEditPracticeBankDispalySeq.Text.Trim)
                            .BankName = txtEditBankName.Text.Trim
                            .BranchName = txtEditBranchName.Text.Trim
                            'CRE16-003 (Disallow input Chinese Chars) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            If chkEditIsFreeTextFormat.Checked Then
                                .BankAcctOwner = txtEditBankOwnerFreeText.Text.Trim
                            Else
                                .BankAcctOwner = txtEditBankOwner.Text.Trim
                            End If
                            'CRE16-003 (Disallow input Chinese Chars) [End][Chris YIM]

                            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                            If chkEditIsFreeTextFormat.Checked Then
                                .BankAcctNo = txtEditBankAccNoFreeText.Text.Trim
                            Else
                                .BankAcctNo = udtFormatter.formatBankAcct(txtEditBankCode.Text.Trim, txtEditBranchCode.Text.Trim, txtEditBankAcc.Text.Trim)
                            End If
                            .IsFreeTextFormat = IIf(chkEditIsFreeTextFormat.Checked, YesNo.Yes, YesNo.No)
                            'CRE13-019-02 Extend HCVS to China [End][Winnie]
                        End With

                        blnSuccess = udtSPProfileBLL.UpdateBankAcctInStaging(udtBankAcct, Me.hfTableLocation.Value.Trim)

                    End If

                    If blnSuccess Then
                        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00021, "Save bank account Completed.")

                        If udtSPProfileBLL.GetServiceProviderProfile(hfERN.Value.Trim, TableLocation.Staging) Then
                            hfTableLocation.Value = TableLocation.Staging
                            BindSPProfile()

                        End If
                    End If

                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then
                        Dim strmsg As String
                        strmsg = eSQL.Message

                        If Not strmsg.Trim.Equals(String.Empty) Then
                            SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                            msgBox.AddMessage(SM)
                            If msgBox.GetCodeTable.Rows.Count = 0 Then
                                msgBox.Visible = False
                            Else
                                'msgBox.BuildMessageBox("UpdateFail")
                                msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00022, "Save bank account Fail.")
                            End If
                        Else
                            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00022, "Save bank account Fail.")
                        End If

                        'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
                    Else
                        Throw eSQL
                    End If

                Catch ex As Exception
                    Throw ex
                End Try

            Else
                'msgBox.BuildMessageBox("ValidationFail")
                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00022, "Save bank account Fail.")
            End If

        End If
    End Sub

#End Region

#Region "Tab - Scheme Information"

    Protected Sub gvSI_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            If (e.Row.RowState And DataControlRowState.Edit) > 0 Then
                ' Edit Mode
                Dim lblSchemeEditDispalySeq As Label = CType(e.Row.FindControl("lblSchemeEditDispalySeq"), Label)
                Dim hfSchemeEditHealthProf As HiddenField = CType(e.Row.FindControl("hfSchemeEditHealthProf"), HiddenField)
                Dim hfEditHasScheme As HiddenField = CType(e.Row.FindControl("hfEditHasScheme"), HiddenField)
                Dim lblEditSchemePracticeStatus As Label = CType(e.Row.FindControl("lblEditSchemePracticeStatus"), Label)

                Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = udtPracticeBLL.GetPracticeCollection.Item(CInt(lblSchemeEditDispalySeq.Text.Trim)).PracticeSchemeInfoList

                InitializeCheckedSchemeDataTable()

                Dim dtCheckedScheme As DataTable = Session(SESS_CheckedScheme)

                If Not IsNothing(udtPracticeSchemeInfoList) Then
                    For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                        For Each dr As DataRow In dtCheckedScheme.Rows
                            If CStr(dr.Item("MScheme_Code")).Trim.Equals(udtPracticeSchemeInfo.SchemeCode.Trim) Then

                                Select Case Me.hfTableLocation.Value.Trim
                                    Case TableLocation.Permanent
                                        Select Case udtPracticeSchemeInfo.RecordStatus.Trim
                                            Case PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary, PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary

                                            Case Else
                                                dr.Item("Checked") = "Y"
                                        End Select

                                    Case TableLocation.Staging
                                        Select Case udtPracticeSchemeInfo.RecordStatus.Trim
                                            Case PracticeSchemeInfoStagingStatus.DelistedInvoluntary, PracticeSchemeInfoStagingStatus.DelistedVoluntary

                                            Case PracticeSchemeInfoStagingStatus.Active, _
                                                    PracticeSchemeInfoStagingStatus.Existing, _
                                                    PracticeSchemeInfoStagingStatus.Update, _
                                                    PracticeSchemeInfoStagingStatus.ActivePendingDelist, _
                                                    PracticeSchemeInfoStagingStatus.ActivePendingSuspend, _
                                                    PracticeSchemeInfoStagingStatus.Suspended, _
                                                    PracticeSchemeInfoStagingStatus.SuspendedPendingDelist, _
                                                    PracticeSchemeInfoStagingStatus.SuspendedPendingReactivate
                                                dr.Item("Checked") = "Y"

                                            Case Else

                                        End Select

                                    Case TableLocation.Enrolment
                                        dr.Item("Checked") = "Y"

                                End Select

                            End If
                        Next
                    Next
                End If

                If IsNothing(udtPracticeSchemeInfoList) Then
                    hfEditHasScheme.Value = "N"
                Else
                    If udtPracticeSchemeInfoList.Count = 0 Then
                        hfEditHasScheme.Value = "N"
                    Else
                        hfEditHasScheme.Value = "Y"
                    End If

                End If

                Select Case hfTableLocation.Value.Trim
                    Case TableLocation.Permanent
                        Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, lblEditSchemePracticeStatus.Text.Trim, lblEditSchemePracticeStatus.Text, String.Empty)

                    Case TableLocation.Staging
                        Status.GetDescriptionFromDBCode(PracticeStagingStatus.ClassCode, lblEditSchemePracticeStatus.Text.Trim, lblEditSchemePracticeStatus.Text, String.Empty)

                    Case TableLocation.Enrolment
                        lblEditSchemePracticeStatus.Text = "Unprocessed"

                End Select

                Dim gvEditPracticeSchemeInfo As GridView = CType(e.Row.FindControl("gvEditPracticeSchemeInfo"), GridView)

                gvEditPracticeSchemeInfo.DataSource = udtSchemeBackOfficeBLL.GetAllEffectiveSubsidizeGroupFromCache.ToSPProfileDataTable
                gvEditPracticeSchemeInfo.DataBind()

                ApplyPracticeSchemeChangeIndicator(e.Row, True)

            Else
                ' ReadOnly Mode
                Dim lblSchemeDispalySeq As Label = CType(e.Row.FindControl("lblSchemeDispalySeq"), Label)
                Dim hfSchemeHealthProf As HiddenField = CType(e.Row.FindControl("hfSchemeHealthProf"), HiddenField)
                Dim ibtnSchemeEdit As ImageButton = CType(e.Row.FindControl("ibtnSchemeEdit"), ImageButton)
                Dim hfHasScheme As HiddenField = CType(e.Row.FindControl("hfHasScheme"), HiddenField)
                Dim lblSchemePracticeStatus As Label = CType(e.Row.FindControl("lblSchemePracticeStatus"), Label)

                Dim blnNotEdit As Boolean = False

                Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = udtPracticeBLL.GetPracticeCollection.Item(CInt(lblSchemeDispalySeq.Text.Trim)).PracticeSchemeInfoList
                Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache

                If IsNothing(udtPracticeSchemeInfoList) Then
                    hfHasScheme.Value = "N"
                Else
                    If udtPracticeSchemeInfoList.Count = 0 Then
                        hfHasScheme.Value = "N"
                    Else
                        hfHasScheme.Value = "Y"
                    End If

                End If

                If Not IsNothing(udtPracticeSchemeInfoList) Then
                    If udtPracticeSchemeInfoList.Count = 1 Then
                        If udtSchemeBackOfficeList.FilterByProfCode(hfSchemeHealthProf.Value.Trim).Count = 1 Then
                            Dim udtSchemeBackOfficeTemp As SchemeBackOfficeModel = udtSchemeBackOfficeList.Item(0)

                            If udtSchemeBackOfficeTemp.SubsidizeGroupBackOfficeList.Count = 1 Then
                                Dim udtSubTemp As SubsidizeGroupBackOfficeModel = udtSchemeBackOfficeTemp.SubsidizeGroupBackOfficeList.Item(0)

                                If Not udtSubTemp.ServiceFeeEnabled Then
                                    blnNotEdit = True
                                End If
                            End If
                        End If
                    End If
                End If

                Select Case hfTableLocation.Value.Trim
                    Case TableLocation.Permanent

                        If gvSI.EditIndex > -1 Then
                            ibtnSchemeEdit.Enabled = False
                            ibtnSchemeEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                        Else
                            If lblSchemePracticeStatus.Text.Equals(PracticeStatus.Delisted) Then

                                ibtnSchemeEdit.Enabled = False
                                ibtnSchemeEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                            Else
                                ibtnSchemeEdit.Enabled = True
                                ibtnSchemeEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSBtn")
                            End If
                        End If

                        Status.GetDescriptionFromDBCode(PracticeStatus.ClassCode, lblSchemePracticeStatus.Text.Trim, lblSchemePracticeStatus.Text, String.Empty)

                    Case TableLocation.Staging

                        If gvSI.EditIndex > -1 Then
                            ibtnSchemeEdit.Enabled = False
                            ibtnSchemeEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                        Else

                            If strProgress.Equals(String.Empty) Then

                                If lblSchemePracticeStatus.Text.Equals(PracticeStagingStatus.Delisted) Then
                                    ibtnSchemeEdit.Enabled = False
                                    ibtnSchemeEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                                Else
                                    If blnNotEdit Then
                                        ibtnSchemeEdit.Enabled = False
                                        ibtnSchemeEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                                    Else
                                        ibtnSchemeEdit.Enabled = True
                                        ibtnSchemeEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSBtn")
                                    End If

                                End If
                            Else
                                ibtnSchemeEdit.Enabled = False
                                ibtnSchemeEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                            End If
                        End If

                        Status.GetDescriptionFromDBCode(PracticeStagingStatus.ClassCode, lblSchemePracticeStatus.Text.Trim, lblSchemePracticeStatus.Text, String.Empty)

                    Case TableLocation.Enrolment
                        If gvSI.EditIndex > -1 Then
                            ibtnSchemeEdit.Enabled = False
                            ibtnSchemeEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                        Else
                            If blnExisingHKID Then
                                ibtnSchemeEdit.Enabled = False
                                ibtnSchemeEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                            Else
                                If blnNotEdit Then
                                    ibtnSchemeEdit.Enabled = False
                                    ibtnSchemeEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSDisableBtn")
                                Else
                                    ibtnSchemeEdit.Enabled = True
                                    ibtnSchemeEdit.ImageUrl = Me.GetGlobalResourceObject("ImageUrl", "EditSBtn")
                                End If

                            End If
                        End If

                        lblSchemePracticeStatus.Text = "Unprocessed"
                End Select

                Dim gvPracticeSchemeInfo As GridView = CType(e.Row.FindControl("gvPracticeSchemeInfo"), GridView)
                Dim lblSchemeInfoNA As Label = CType(e.Row.FindControl("lblSchemeInfoNA"), Label)

                If hfHasScheme.Value.Trim.Equals("N") Then
                    gvPracticeSchemeInfo.Visible = False
                    lblSchemeInfoNA.Visible = True
                Else
                    gvPracticeSchemeInfo.Visible = True
                    lblSchemeInfoNA.Visible = False

                    gvPracticeSchemeInfo.DataSource = udtSchemeBackOfficeBLL.GetAllEffectiveSubsidizeGroupFromCache.ToSPProfileDataTable
                    gvPracticeSchemeInfo.DataBind()

                End If

                ApplyPracticeSchemeChangeIndicator(e.Row)

            End If
        End If
    End Sub

    Protected Sub gvSI_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs)
        gvSI.EditIndex = e.NewEditIndex
        gvSI.DataSource = udtPracticeBLL.GetPracticeCollection.Values
        gvSI.DataBind()

    End Sub

    Protected Sub gvSI_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs)
        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)

        udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)
        If udtServiceProviderBLL.Exist Then
            udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
        Else
            udtAuditLogEntry.AddDescripton("SPID", "")
        End If

        udtAuditLogEntry.WriteStartLog(LogID.LOG00065, "Save Scheme")

        Dim row As GridViewRow = DirectCast(sender, GridView).Rows(e.RowIndex)
        Dim gvEditPracticeSchemeInfo As GridView = row.FindControl("gvEditPracticeSchemeInfo")
        Dim lblSchemeEditDispalySeq As Label = row.FindControl("lblSchemeEditDispalySeq")

        ' --- Validation ---

        Dim udtSystemMessage As SystemMessage = Validator.chkGridSelectedNothing(gvEditPracticeSchemeInfo, "chkEditSelect", 1)

        If Not IsNothing(udtSystemMessage) Then
            msgBox.AddMessage(strFuncCode, SeverityCode.SEVE, MsgCode.MSG00005, "%s", lblSchemeEditDispalySeq.Text.Trim)

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)

        End If

        If msgBox.GetCodeTable.Rows.Count > 0 Then
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00067, "Save Scheme Fail.")
            gvServiceFeeShowImageAfterDataBind(gvEditPracticeSchemeInfo)

            Return

        End If

        ' --- End of Validation ---

        Dim udtPracticeSchemeInfoList As New PracticeSchemeInfoModelCollection
        Dim udtExistPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing
        Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel = Nothing

        If udtPracticeBLL.Exist _
                AndAlso Not IsNothing(udtPracticeBLL.GetPracticeCollection.Item(CInt(lblSchemeEditDispalySeq.Text.Trim))) Then
            udtExistPracticeSchemeInfoList = udtPracticeBLL.GetPracticeCollection.Item(CInt(lblSchemeEditDispalySeq.Text.Trim)).PracticeSchemeInfoList
        End If

        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
        Dim udtSubsidizeGroupBackOfficeList As SubsidizeGroupBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSubsidizeGroupFromCache

        Dim dtCheckedScheme As DataTable = Session(SESS_CheckedScheme)

        Dim strPreviousScheme As String = String.Empty

        For Each gvr As GridViewRow In gvEditPracticeSchemeInfo.Rows
            ' Skip the Category Header
            If DirectCast(gvr.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "Y" Then Continue For

            Dim chkEditSelect As CheckBox = gvr.FindControl("chkEditSelect")
            Dim txtEditPracticeSchemeServiceFee As TextBox = Nothing
            Dim chkEditNotProvideServiceFee As CheckBox = Nothing

            txtEditPracticeSchemeServiceFee = CType(gvr.FindControl("txtEditPracticeSchemeServiceFee"), TextBox)
            chkEditNotProvideServiceFee = CType(gvr.FindControl("chkEditNotProvideServiceFee"), CheckBox)


            Dim strSchemeCode As String = gvr.Cells(0).Text.Trim
            Dim strSubsidizeCode As String = gvr.Cells(1).Text.Trim

            Dim udtSchemeBackOffice As SchemeBackOfficeModel = udtSchemeBackOfficeList.Filter(strSchemeCode)
            Dim udtSubsidizeGroupBackOffice As SubsidizeGroupBackOfficeModel = udtSubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode)

            If dtCheckedScheme.Select(String.Format("MScheme_Code = '{0}' AND Checked = 'Y'", strSchemeCode.Trim)).Length = 1 Then
                chkEditSelect.Checked = True
            Else
                chkEditSelect.Checked = False
            End If

            If chkEditSelect.Checked Then
                ' --- Validation ---

                Dim chkEditSelectSubsidize As CheckBox = CType(gvr.FindControl("chkEditSelectSubsidize"), CheckBox)

                ' Validate the subsidy if the subsidies are displayed on the screen
                If udtSchemeBackOffice.DisplaySubsidizeDesc Then
                    If chkEditSelectSubsidize.Checked Then
                        txtEditPracticeSchemeServiceFee = CType(gvr.FindControl("txtEditPracticeSchemeServiceFee"), TextBox)
                        chkEditNotProvideServiceFee = CType(gvr.FindControl("chkEditNotProvideServiceFee"), CheckBox)

                        If udtSubsidizeGroupBackOffice.ServiceFeeEnabled AndAlso chkEditNotProvideServiceFee.Checked = False Then
                            udtSystemMessage = Validator.chkServiceFee(txtEditPracticeSchemeServiceFee.Text.Trim)

                            If Not IsNothing(udtSystemMessage) Then
                                msgBox.AddMessage(udtSystemMessage, New String() {"%s", "%en"}, New String() {lblSchemeEditDispalySeq.Text.Trim, udtSubsidizeGroupBackOffice.SubsidizeDisplayCode.Trim})
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)

                            End If
                        End If

                    End If

                End If

                strPreviousScheme = strSchemeCode

                If msgBox.GetCodeTable.Rows.Count > 0 Then
                    Continue For
                End If

                ' --- End of Validation ---

                If udtSchemeBackOffice.DisplaySubsidizeDesc = False OrElse udtSubsidizeGroupBackOffice.SubsidyCompulsory Then
                    ' If the subsidy is not displayed, or the subsidy is compulsory, forcely select the subsidy to add to the database
                    chkEditSelectSubsidize.Checked = True
                End If

                Dim enumClinicType As PracticeSchemeInfoModel.ClinicTypeEnum = PracticeSchemeInfoModel.ClinicTypeEnum.NA

                If udtSchemeBackOffice.AllowNonClinicSetting = False Then
                    enumClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.Clinic
                Else
                    ' The checkbox is visible, find the input. It is located in the first row of this scheme
                    For Each gvrTemp As GridViewRow In gvEditPracticeSchemeInfo.Rows
                        If gvrTemp.Cells(0).Text.Trim = strSchemeCode Then
                            If DirectCast(gvrTemp.FindControl("chkGNonClinic"), CheckBox).Checked Then
                                enumClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic
                            Else
                                enumClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.Clinic
                            End If

                            Exit For

                        End If

                    Next

                End If

                If IsNothing(udtExistPracticeSchemeInfoList) Then
                    ' All PracticeSchemeInfo from user input are new

                    udtPracticeSchemeInfo = New PracticeSchemeInfoModel

                    With udtPracticeSchemeInfo

                        .EnrolRefNo = hfERN.Value.Trim
                        If udtServiceProviderBLL.Exist Then .SPID = udtServiceProviderBLL.GetSP.SPID
                        .PracticeDisplaySeq = CInt(lblSchemeEditDispalySeq.Text.Trim)
                        .SchemeCode = strSchemeCode
                        .SubsidizeCode = strSubsidizeCode
                        .RecordStatus = PracticeSchemeInfoStagingStatus.Active
                        .SchemeDisplaySeq = udtSchemeBackOffice.DisplaySeq
                        .SubsidizeDisplaySeq = udtSubsidizeGroupBackOffice.DisplaySeq
                        .ProvideService = chkEditSelectSubsidize.Checked
                        .ClinicType = enumClinicType

                        If udtSubsidizeGroupBackOffice.ServiceFeeEnabled Then
                            If chkEditNotProvideServiceFee.Checked Then
                                .ProvideServiceFee = False
                                .ServiceFee = Nothing
                            Else
                                If .ProvideService Then
                                    .ProvideServiceFee = True
                                Else
                                    .ProvideServiceFee = Nothing
                                End If

                                If txtEditPracticeSchemeServiceFee.Text.Trim <> String.Empty Then
                                    .ServiceFee = CInt(txtEditPracticeSchemeServiceFee.Text.Trim)
                                Else
                                    .ServiceFee = Nothing
                                End If

                            End If
                        Else
                            .ProvideServiceFee = Nothing
                            .ServiceFee = Nothing
                        End If

                    End With

                    udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)

                Else
                    Dim udtExistPracticeSchemeInfo As PracticeSchemeInfoModel = udtExistPracticeSchemeInfoList.Item(CInt(lblSchemeEditDispalySeq.Text.Trim), strSubsidizeCode, udtSchemeBackOffice.DisplaySeq, udtSubsidizeGroupBackOffice.DisplaySeq, udtSubsidizeGroupBackOffice.SchemeCode)

                    If IsNothing(udtExistPracticeSchemeInfo) Then
                        ' The PracticeSchemeInfo from user input is new

                        udtPracticeSchemeInfo = New PracticeSchemeInfoModel

                        With udtPracticeSchemeInfo
                            .EnrolRefNo = hfERN.Value.Trim
                            If udtServiceProviderBLL.Exist Then .SPID = udtServiceProviderBLL.GetSP.SPID
                            .PracticeDisplaySeq = CInt(lblSchemeEditDispalySeq.Text.Trim)
                            .SchemeCode = strSchemeCode
                            .SubsidizeCode = strSubsidizeCode
                            .RecordStatus = PracticeSchemeInfoStagingStatus.Active
                            .SchemeDisplaySeq = udtSchemeBackOffice.DisplaySeq
                            .SubsidizeDisplaySeq = udtSubsidizeGroupBackOffice.DisplaySeq
                            .ProvideService = chkEditSelectSubsidize.Checked
                            .ClinicType = enumClinicType

                            If udtSubsidizeGroupBackOffice.ServiceFeeEnabled Then
                                If chkEditNotProvideServiceFee.Checked Then
                                    .ProvideServiceFee = False
                                    .ServiceFee = Nothing
                                Else
                                    If .ProvideService Then
                                        .ProvideServiceFee = True
                                    Else
                                        .ProvideServiceFee = Nothing
                                    End If

                                    If txtEditPracticeSchemeServiceFee.Text.Trim <> String.Empty Then
                                        .ServiceFee = CInt(txtEditPracticeSchemeServiceFee.Text.Trim)
                                    Else
                                        .ServiceFee = Nothing
                                    End If

                                End If
                            Else
                                .ProvideServiceFee = Nothing
                                .ServiceFee = Nothing
                            End If

                        End With

                        udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)


                    Else
                        ' Related PracticeSchemeInfo is found in database
                        ' Check the difference of service fee between existing record and the input
                        ' If there is difference => update the service fee

                        Dim blnCheckReactive As Boolean = False

                        Select Case hfTableLocation.Value.Trim
                            Case TableLocation.Enrolment
                                blnCheckReactive = False

                            Case TableLocation.Staging
                                Select Case udtExistPracticeSchemeInfo.RecordStatus
                                    Case PracticeSchemeInfoStagingStatus.DelistedVoluntary, PracticeSchemeInfoStagingStatus.DelistedInvoluntary
                                        udtPracticeSchemeInfo = New PracticeSchemeInfoModel
                                        udtPracticeSchemeInfoBLL.Clone(udtPracticeSchemeInfo, udtExistPracticeSchemeInfo)

                                        With udtPracticeSchemeInfo
                                            If chkEditSelectSubsidize.Checked Then
                                                .ProvideService = True
                                                If udtSubsidizeGroupBackOffice.ServiceFeeEnabled Then
                                                    If chkEditNotProvideServiceFee.Checked Then
                                                        .ProvideServiceFee = False
                                                        .ServiceFee = Nothing
                                                    Else
                                                        .ProvideServiceFee = True

                                                        If txtEditPracticeSchemeServiceFee.Text.Trim <> String.Empty Then
                                                            .ServiceFee = CInt(txtEditPracticeSchemeServiceFee.Text.Trim)
                                                        Else
                                                            .ServiceFee = Nothing
                                                        End If

                                                    End If
                                                Else
                                                    .ProvideServiceFee = Nothing
                                                    .ServiceFee = Nothing
                                                End If
                                            Else
                                                .ProvideService = False
                                                .ProvideServiceFee = Nothing
                                                .ServiceFee = Nothing
                                            End If

                                        End With

                                        udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                                        blnCheckReactive = True

                                    Case Else
                                        blnCheckReactive = False

                                End Select

                            Case TableLocation.Permanent
                                Select Case udtExistPracticeSchemeInfo.RecordStatus
                                    Case PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary, PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary
                                        udtPracticeSchemeInfo = New PracticeSchemeInfoModel
                                        udtPracticeSchemeInfoBLL.Clone(udtPracticeSchemeInfo, udtExistPracticeSchemeInfo)

                                        With udtPracticeSchemeInfo
                                            If chkEditSelectSubsidize.Checked Then
                                                .ProvideService = True
                                                If udtSubsidizeGroupBackOffice.ServiceFeeEnabled Then
                                                    If chkEditNotProvideServiceFee.Checked Then
                                                        .ProvideServiceFee = False
                                                        .ServiceFee = Nothing
                                                    Else
                                                        .ProvideServiceFee = True
                                                        .ServiceFee = CInt(txtEditPracticeSchemeServiceFee.Text.Trim)
                                                    End If
                                                Else
                                                    .ProvideServiceFee = Nothing
                                                    .ServiceFee = Nothing
                                                End If
                                            Else
                                                .ProvideService = False
                                                .ProvideServiceFee = Nothing
                                                .ServiceFee = Nothing
                                            End If

                                        End With

                                        udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                                        blnCheckReactive = True

                                    Case Else
                                        blnCheckReactive = False

                                End Select

                        End Select

                        If Not blnCheckReactive Then

                            Dim blnRequireUpdate As Boolean = False

                            'Compare Select Subsidize

                            If chkEditSelectSubsidize.Checked Then
                                If udtSubsidizeGroupBackOffice.ServiceFeeEnabled Then
                                    If chkEditNotProvideServiceFee.Checked Then
                                        If udtExistPracticeSchemeInfo.ProvideServiceFee.HasValue Then
                                            If udtExistPracticeSchemeInfo.ProvideServiceFee.Value Then

                                                udtPracticeSchemeInfo = New PracticeSchemeInfoModel
                                                udtPracticeSchemeInfoBLL.Clone(udtPracticeSchemeInfo, udtExistPracticeSchemeInfo)

                                                With udtPracticeSchemeInfo
                                                    .RecordStatus = PracticeSchemeInfoStagingStatus.Update
                                                    .ProvideServiceFee = False
                                                    .ServiceFee = Nothing
                                                    .ProvideService = True
                                                    .ClinicType = enumClinicType
                                                End With

                                                udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                                                blnRequireUpdate = True
                                            End If
                                        Else
                                            udtPracticeSchemeInfo = New PracticeSchemeInfoModel
                                            udtPracticeSchemeInfoBLL.Clone(udtPracticeSchemeInfo, udtExistPracticeSchemeInfo)

                                            With udtPracticeSchemeInfo
                                                .RecordStatus = PracticeSchemeInfoStagingStatus.Update
                                                .ProvideServiceFee = False
                                                .ServiceFee = Nothing
                                                .ProvideService = True
                                                .ClinicType = enumClinicType
                                            End With

                                            udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                                            blnRequireUpdate = True
                                        End If

                                    Else
                                        If udtExistPracticeSchemeInfo.ServiceFee.HasValue Then
                                            If udtExistPracticeSchemeInfo.ServiceFee.Value <> CInt(txtEditPracticeSchemeServiceFee.Text.Trim) Then

                                                udtPracticeSchemeInfo = New PracticeSchemeInfoModel
                                                udtPracticeSchemeInfoBLL.Clone(udtPracticeSchemeInfo, udtExistPracticeSchemeInfo)

                                                With udtPracticeSchemeInfo
                                                    .RecordStatus = PracticeSchemeInfoStagingStatus.Update
                                                    .ProvideServiceFee = True
                                                    .ServiceFee = CInt(txtEditPracticeSchemeServiceFee.Text.Trim)
                                                    .ProvideService = True
                                                    .ClinicType = enumClinicType
                                                End With

                                                udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                                                blnRequireUpdate = True
                                            End If
                                        Else
                                            udtPracticeSchemeInfo = New PracticeSchemeInfoModel
                                            udtPracticeSchemeInfoBLL.Clone(udtPracticeSchemeInfo, udtExistPracticeSchemeInfo)

                                            With udtPracticeSchemeInfo
                                                .RecordStatus = PracticeSchemeInfoStagingStatus.Update
                                                .ProvideServiceFee = True
                                                .ServiceFee = CInt(txtEditPracticeSchemeServiceFee.Text.Trim)
                                                .ProvideService = True
                                                .ClinicType = enumClinicType
                                            End With

                                            udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                                            blnRequireUpdate = True
                                        End If
                                    End If
                                    'CRE15-004 TIV & QIV [Start][Winnie]
                                Else
                                    udtPracticeSchemeInfo = New PracticeSchemeInfoModel
                                    udtPracticeSchemeInfoBLL.Clone(udtPracticeSchemeInfo, udtExistPracticeSchemeInfo)
                                    If Not udtExistPracticeSchemeInfo.ProvideService Then
                                        With udtPracticeSchemeInfo
                                            .RecordStatus = PracticeSchemeInfoStagingStatus.Update
                                            .ProvideService = True
                                            .ClinicType = enumClinicType
                                        End With

                                        udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                                        blnRequireUpdate = True
                                    End If
                                End If
                                'CRE15-004 TIV & QIV [End][Winnie]
                            Else
                                'If not select subsidy, check existing selected
                                If udtExistPracticeSchemeInfo.ProvideService Then

                                    udtPracticeSchemeInfo = New PracticeSchemeInfoModel
                                    udtPracticeSchemeInfoBLL.Clone(udtPracticeSchemeInfo, udtExistPracticeSchemeInfo)

                                    With udtPracticeSchemeInfo
                                        .RecordStatus = PracticeSchemeInfoStagingStatus.Update
                                        .ProvideServiceFee = Nothing
                                        .ServiceFee = Nothing
                                        .ProvideService = False
                                        .ClinicType = enumClinicType
                                    End With

                                    udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                                    blnRequireUpdate = True
                                End If
                            End If

                            ' If not require update, check if the Non-clinic checkbox changed
                            If blnRequireUpdate = False Then
                                If enumClinicType <> udtExistPracticeSchemeInfo.ClinicType Then
                                    udtPracticeSchemeInfo = New PracticeSchemeInfoModel
                                    udtPracticeSchemeInfoBLL.Clone(udtPracticeSchemeInfo, udtExistPracticeSchemeInfo)

                                    With udtPracticeSchemeInfo
                                        .RecordStatus = PracticeSchemeInfoStagingStatus.Update
                                        .ClinicType = enumClinicType
                                    End With

                                    udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)

                                End If

                            End If

                        End If
                    End If
                End If

            Else
                ' Check which practice scheme info should be deleted
                If Not IsNothing(udtPracticeSchemeInfoList) Then

                    Dim udtExistPractice As PracticeSchemeInfoModel
                    If Not IsNothing(udtExistPracticeSchemeInfoList) Then
                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                        ' Add SchemeCode to key
                        udtExistPractice = udtExistPracticeSchemeInfoList.Item(CInt(lblSchemeEditDispalySeq.Text.Trim), strSubsidizeCode, udtSchemeBackOffice.DisplaySeq, udtSubsidizeGroupBackOffice.DisplaySeq, udtSubsidizeGroupBackOffice.SchemeCode)
                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                        Dim blnAbleToDelete As Boolean = False


                        If Not IsNothing(udtExistPractice) Then
                            udtPracticeSchemeInfo = New PracticeSchemeInfoModel
                            udtPracticeSchemeInfoBLL.Clone(udtPracticeSchemeInfo, udtExistPractice)

                            Select Case hfTableLocation.Value.Trim
                                Case TableLocation.Staging
                                    If udtPracticeSchemeInfo.RecordStatus.Trim.Equals(PracticeSchemeInfoStagingStatus.Active) Then
                                        blnAbleToDelete = True
                                    End If
                                Case TableLocation.Enrolment
                                    blnAbleToDelete = True
                            End Select


                            If blnAbleToDelete Then
                                With udtPracticeSchemeInfo
                                    .RecordStatus = PracticeSchemeInfoStagingStatus.Reject
                                End With

                                udtPracticeSchemeInfoList.Add(udtPracticeSchemeInfo)
                            End If
                        End If
                    End If
                End If
            End If
        Next

        If msgBox.GetCodeTable.Rows.Count > 0 Then
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, LogID.LOG00067, "Save Scheme Fail.")
            gvServiceFeeShowImageAfterDataBind(gvEditPracticeSchemeInfo)

            Return

        End If

        Try
            Dim blnSuccess As Boolean = False

            udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)
            If udtServiceProviderBLL.Exist Then
                udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
            Else
                udtAuditLogEntry.AddDescripton("SPID", "")
            End If

            Dim strAuditLog As String = String.Empty

            For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                Select Case udtPracticeScheme.RecordStatus
                    Case PracticeSchemeInfoStagingStatus.Active
                        strAuditLog = strAuditLog + ", " + udtPracticeScheme.SubsidizeCode.Trim + "-" + "Add"
                    Case PracticeSchemeInfoStagingStatus.Update
                        strAuditLog = strAuditLog + ", " + udtPracticeScheme.SubsidizeCode.Trim + "-" + "Update"
                    Case PracticeSchemeInfoStagingStatus.Reject
                        strAuditLog = strAuditLog + ", " + udtPracticeScheme.SubsidizeCode.Trim + "-" + "Delete"
                        'Case ""
                    Case PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary, PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary
                        strAuditLog = strAuditLog + ", " + udtPracticeScheme.SubsidizeCode.Trim + "-" + "ReActive"
                End Select
            Next

            If strAuditLog.Length > 2 Then
                strAuditLog = strAuditLog.Substring(2)
            End If

            udtAuditLogEntry.AddDescripton("Action", strAuditLog)


            If Not IsNothing(udtPracticeSchemeInfoList) Then
                blnSuccess = udtSPProfileBLL.PracticeSchemeInfoListStagingOperation(udtPracticeSchemeInfoList, hfTableLocation.Value.Trim)

            Else
                blnSuccess = True

            End If

            udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00066, "Save Scheme Completed")

            If udtSPProfileBLL.GetServiceProviderProfile(hfERN.Value.Trim, TableLocation.Staging) Then
                hfTableLocation.Value = TableLocation.Staging
                BindSPProfile()

            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                If Not strmsg.Trim.Equals(String.Empty) Then
                    SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                    msgBox.AddMessage(SM)
                    If msgBox.GetCodeTable.Rows.Count = 0 Then
                        msgBox.Visible = False
                    Else
                        msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, LogID.LOG00067, "Save Scheme Fail.")
                    End If
                Else
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00067, "Save Scheme Fail.")
                End If

            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw

        End Try

    End Sub

    Protected Sub gvSI_RowCancelingEdit(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewCancelEditEventArgs)
        Me.msgBox.Visible = False

        gvSI.EditIndex = -1
        gvSI.DataSource = udtPracticeBLL.GetPracticeCollection.Values
        gvSI.DataBind()

    End Sub

    Private Sub InitializeCheckedSchemeDataTable()
        Session.Remove(SESS_CheckedScheme)

        Dim dt As New DataTable
        dt.Columns.Add(New DataColumn("MScheme_Code"))
        dt.Columns.Add(New DataColumn("Checked"))

        For Each udtSchemeBackOffice As SchemeBackOfficeModel In udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
            Dim dr As DataRow = dt.NewRow
            dr(0) = udtSchemeBackOffice.SchemeCode.Trim
            dr(1) = "N"
            dt.Rows.Add(dr)
        Next

        Session(SESS_CheckedScheme) = dt

    End Sub

    Private Sub ApplyPracticeSchemeChangeIndicator(ByVal r As GridViewRow, Optional ByVal blnEditMode As Boolean = False)
        Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP()

        ' Return for New Enrolment
        If udtSP.SPID Is Nothing OrElse udtSP.SPID.Trim = String.Empty Then Return

        ' Return for Load from Permanent
        If hfTableLocation.Value.Trim = TableLocation.Permanent Then Return

        ' Status of Practice
        Dim lblSchemePracticeStatus As Label = CType(r.FindControl(IIf(blnEditMode, "lblEditSchemePracticeStatus", "lblSchemePracticeStatus")), Label)
        If lblSchemePracticeStatus.Text.Trim = ServiceProviderComparator.strNew OrElse lblSchemePracticeStatus.Text.Trim = ServiceProviderComparator.strUnderAmendment Then
            lblSchemePracticeStatus.ForeColor = Drawing.Color.Red
            CType(r.FindControl(IIf(blnEditMode, "lblEditSchemePracticeStatusInd", "lblSchemePracticeStatusInd")), Label).Visible = True
        End If

        Dim blnNewScheme As Boolean = False

        Dim gv As GridView = CType(r.FindControl(IIf(blnEditMode, "gvEditPracticeSchemeInfo", "gvPracticeSchemeInfo")), GridView)

        For Each gr As GridViewRow In gv.Rows
            If gr.RowType = DataControlRowType.DataRow Then
                Dim lblStatus As Label = CType(gr.FindControl(IIf(blnEditMode, "lblEditPracticeSchemeStatus", "lblPracticeSchemeStatus")), Label)

                Select Case lblStatus.Text.Trim
                    Case String.Empty
                        Continue For

                    Case ServiceProviderComparator.strNew
                        lblStatus.ForeColor = Drawing.Color.Red
                        blnNewScheme = True
                        Continue For

                    Case ServiceProviderComparator.strUnderAmendment
                        lblStatus.ForeColor = Drawing.Color.Red
                        blnNewScheme = True

                End Select

                If Not blnEditMode Then
                    Dim hfStatus As HiddenField = gr.FindControl("hfPracticeSchemeStatus")
                    If hfStatus.Value.Trim = "U" OrElse hfStatus.Value.Trim = "A" Then
                        CType(gr.FindControl("lblPracticeSchemeServiceFee"), Label).ForeColor = Drawing.Color.Red
                    End If

                    Dim udtComparator As New ServiceProviderComparator(ServiceProviderPermanent)

                    Dim intSeq As Integer = CInt(CType(r.FindControl("lblSchemeDispalySeq"), Label).Text.Trim)

                    If udtComparator.IsPracticeSchemeInfoChanged(intSeq, gr.Cells(0).Text.Trim, gr.Cells(1).Text.Trim, udtServiceProviderBLL.GetSP()) Then
                        CType(gr.FindControl("lblPracticeSchemeServiceFee"), Label).ForeColor = Drawing.Color.Red
                    End If

                End If

            End If
        Next

        If blnNewScheme Then
            CType(r.FindControl(IIf(blnEditMode, "lblSchemeEditInfoTextInd", "lblSchemeInfoTextInd")), Label).Visible = True
        End If

    End Sub

    '

    Protected Sub chkEditSelect_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        msgBox.Visible = False

        Dim chkEditSelect As CheckBox = CType(sender, CheckBox)
        Dim strSelectedSchemeCode As String = DirectCast(chkEditSelect.NamingContainer, GridViewRow).Cells(0).Text.Trim
        Dim dtCheckedScheme As DataTable = Session(SESS_CheckedScheme)

        For Each dr As DataRow In dtCheckedScheme.Rows
            If dr("MScheme_Code").ToString.Trim = strSelectedSchemeCode Then
                If chkEditSelect.Checked Then
                    dr("Checked") = "Y"
                Else
                    dr("Checked") = "N"
                End If

                Exit For

            End If

        Next

        Session(SESS_CheckedScheme) = dtCheckedScheme

    End Sub

    Protected Sub gvServiceFee_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim gvServiceFee As GridView = CType(sender, GridView)
            Dim gvr_Parent As GridViewRow = gvServiceFee.NamingContainer

            Dim intPracticeDisplaySeq As Integer

            Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
            Dim udtSubsidizeGroupBackOfficeList As SubsidizeGroupBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSubsidizeGroupFromCache

            Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel

            Dim udtSchemeBackOffice As SchemeBackOfficeModel
            Dim udtSubsidizeGroupBackOffice As SubsidizeGroupBackOfficeModel

            Dim strSchemeCode As String
            Dim strSubsidizeCode As String

            Dim strHealthProf As String = String.Empty

            If gvServiceFee.ID.Trim.Equals("gvPracticeSchemeInfo") Then
                ' --- ReadOnly Mode ---

                Dim chkSelect As CheckBox = CType(e.Row.FindControl("chkSelect"), CheckBox)
                Dim lblPracticeSchemeServiceFee As Label = CType(e.Row.FindControl("lblPracticeSchemeServiceFee"), Label)
                Dim lblPracticeSchemeStatus As Label = CType(e.Row.FindControl("lblPracticeSchemeStatus"), Label)
                Dim lblPracticeSchemeRemark As Label = CType(e.Row.FindControl("lblPracticeSchemeRemark"), Label)
                Dim hfPracticeSchemeStatus As HiddenField = e.Row.FindControl("hfPracticeSchemeStatus")
                Dim lblPracticeSchemeEffectiveDtm As Label = CType(e.Row.FindControl("lblPracticeSchemeEffectiveDtm"), Label)
                Dim lblPracticeSchemeDelistDtm As Label = CType(e.Row.FindControl("lblPracticeSchemeDelistDtm"), Label)

                strSchemeCode = e.Row.Cells(0).Text.Trim
                strSubsidizeCode = e.Row.Cells(1).Text.Trim

                strHealthProf = CType(gvr_Parent.FindControl("hfSchemeHealthProf"), HiddenField).Value.Trim
                intPracticeDisplaySeq = CInt(CType(gvr_Parent.FindControl("lblSchemeDispalySeq"), Label).Text.Trim)
                udtPracticeSchemeInfoList = udtPracticeBLL.GetPracticeCollection.Item(intPracticeDisplaySeq).PracticeSchemeInfoList

                udtSchemeBackOffice = udtSchemeBackOfficeList.Filter(strSchemeCode)
                udtSubsidizeGroupBackOffice = udtSubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode)

                e.Row.Visible = False

                If udtSchemeBackOffice.EligibleProfesional(strHealthProf) = False Then
                    Return
                End If

                If IsNothing(udtPracticeSchemeInfoList) OrElse udtPracticeSchemeInfoList.Count = 0 Then
                    Return
                End If

                udtPracticeSchemeInfo = udtPracticeSchemeInfoList.Item(intPracticeDisplaySeq, strSubsidizeCode, udtSchemeBackOffice.DisplaySeq, udtSubsidizeGroupBackOffice.DisplaySeq, udtSubsidizeGroupBackOffice.SchemeCode)

                ' Hide the row if not enrolled or not providing service
                If DirectCast(e.Row.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "Y" Then
                    For Each udtPSINode As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                        If udtPSINode.SchemeCode = strSchemeCode Then
                            udtPracticeSchemeInfo = udtPSINode
                            Exit For
                        End If
                    Next

                    If IsNothing(udtPracticeSchemeInfo) Then
                        Return
                    End If

                Else
                    If IsNothing(udtPracticeSchemeInfoList.Filter(strSchemeCode)) Then
                        Return
                    End If

                    ' Check all not provide service
                    Dim blnAllNotProvideService As Boolean = True

                    For Each udtPSINode As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Filter(strSchemeCode).Values
                        If udtPSINode.ProvideService Then
                            blnAllNotProvideService = False
                            Exit For
                        End If
                    Next

                    If blnAllNotProvideService Then
                        DirectCast(e.Row.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y"

                    Else
                        If IsNothing(udtPracticeSchemeInfo) OrElse udtPracticeSchemeInfo.ProvideService = False Then
                            Return

                        End If

                    End If

                End If

                e.Row.Visible = True

                ' Scheme
                Dim lblGSchemeDisplayCode As Label = e.Row.FindControl("lblGSchemeDisplayCode")
                lblGSchemeDisplayCode.Text = udtSchemeBackOffice.DisplayCode

                If IsNothing(udtPracticeSchemeInfo) Then
                    chkSelect.Enabled = False
                    lblPracticeSchemeServiceFee.Text = "--"

                    If Not udtSubsidizeGroupBackOffice.SubsidyCompulsory Then
                        e.Row.Visible = False
                    End If

                Else
                    chkSelect.Checked = True
                    chkSelect.Enabled = False

                    If udtPracticeSchemeInfo.ProvideService Then
                        If udtSubsidizeGroupBackOffice.ServiceFeeEnabled Then
                            If udtPracticeSchemeInfo.ProvideServiceFee.HasValue Then
                                If udtPracticeSchemeInfo.ProvideServiceFee Then
                                    lblPracticeSchemeServiceFee.Text = udtFormatter.formatMoney(udtPracticeSchemeInfo.ServiceFee.Value.ToString, True)
                                Else
                                    lblPracticeSchemeServiceFee.Text = udtSubsidizeGroupBackOffice.ServiceFeeCompulsoryWording
                                End If
                            Else
                                lblPracticeSchemeServiceFee.Text = "--"
                            End If

                        Else
                            lblPracticeSchemeServiceFee.Text = Me.GetGlobalResourceObject("Text", "ServiceFeeN/A")
                        End If
                    End If

                    ' Non-clinic
                    If udtPracticeSchemeInfo.ClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
                        lblGSchemeDisplayCode.Text += String.Format("<br />({0})", Me.GetGlobalResourceObject("Text", "NonClinic"))
                    End If

                End If

                If Not IsNothing(udtPracticeSchemeInfo) Then
                    hfPracticeSchemeStatus.Value = udtPracticeSchemeInfo.RecordStatus.Trim
                    lblPracticeSchemeStatus.Text = udtSPProfileBLL.GetPracticeSchemeInfoStatus(udtServiceProviderBLL.GetSP.PracticeList.Item(intPracticeDisplaySeq), strSchemeCode, Me.hfTableLocation.Value.Trim)

                    'CRE15-004 TIV & QIV [Start][Winnie]
                    'Clear Remark if scheme is under admendment
                    If hfTableLocation.Value.Trim.Equals(TableLocation.Staging) AndAlso lblPracticeSchemeStatus.Text = PracticeSchemeInfoStagingStatus.Update Then
                        lblPracticeSchemeRemark.Text = String.Empty
                    Else
                        lblPracticeSchemeRemark.Text = udtPracticeSchemeInfo.Remark.Trim
                    End If
                    'lblPracticeSchemeRemark.Text = udtPracticeSchemeInfo.Remark.Trim
                    'CRE15-004 TIV & QIV [End][Winnie]
                Else
                    lblPracticeSchemeStatus.Text = String.Empty
                    lblPracticeSchemeRemark.Text = String.Empty
                End If

                If Not lblPracticeSchemeRemark.Text.Equals(String.Empty) Then
                    lblPracticeSchemeRemark.Text = "[" + lblPracticeSchemeRemark.Text + "]"
                End If

                Select Case hfTableLocation.Value.Trim
                    Case TableLocation.Permanent
                        Status.GetDescriptionFromDBCode(PracticeSchemeInfoMaintenanceDisplayStatus.ClassCode, lblPracticeSchemeStatus.Text.Trim, lblPracticeSchemeStatus.Text, String.Empty)

                    Case TableLocation.Staging
                        Status.GetDescriptionFromDBCode(PracticeSchemeInfoStagingStatus.ClassCode, lblPracticeSchemeStatus.Text.Trim, lblPracticeSchemeStatus.Text, String.Empty)

                    Case TableLocation.Enrolment
                        If IsNothing(udtPracticeSchemeInfo) Then
                            lblPracticeSchemeStatus.Text = String.Empty
                        Else
                            lblPracticeSchemeStatus.Text = "Unprocessed"
                        End If

                End Select

                If lblPracticeSchemeStatus.Text.Trim.Equals(String.Empty) Then
                    lblPracticeSchemeStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If

                udtSPProfileBLL.GetPracticeSchemeInfoEarliestTime(udtPracticeSchemeInfoList, strSchemeCode,
                                                                  lblPracticeSchemeEffectiveDtm.Text, lblPracticeSchemeDelistDtm.Text)

                If lblPracticeSchemeEffectiveDtm.Text.Equals(String.Empty) Then
                    lblPracticeSchemeEffectiveDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If

                If lblPracticeSchemeDelistDtm.Text.Equals(String.Empty) Then
                    lblPracticeSchemeDelistDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
                End If
                'CRE15-004 TIV & QIV [End][Winnie]

            ElseIf gvServiceFee.ID.Trim.Equals("gvEditPracticeSchemeInfo") Then
                ' --- Edit Mode ---

                Dim chkEditSelect As CheckBox = CType(e.Row.FindControl("chkEditSelect"), CheckBox)
                'CRE15-004 TIV & QIV [Start][Winnie]
                Dim chkEditSelectSubsidize As CheckBox = CType(e.Row.FindControl("chkEditSelectSubsidize"), CheckBox)
                Dim pnlEditPracticeSchemeSubsidize As Panel = CType(e.Row.FindControl("pnlEditPracticeSchemeSubsidize"), Panel)
                Dim pnlEditPracticeSchemeServiceFeeDisplay As Panel = CType(e.Row.FindControl("pnlEditPracticeSchemeServiceFeeDisplay"), Panel)
                Dim lblEditPracticeSchemeSubsidizeCode As Label = CType(e.Row.FindControl("lblEditPracticeSchemeSubsidizeCode"), Label)
                'CRE15-004 TIV & QIV [End][Winnie]
                Dim lblEditPracticeSchemeServiceFee As Label = CType(e.Row.FindControl("lblEditPracticeSchemeServiceFee"), Label)
                Dim pnlEditPracticeSchemeServiceFee As Panel = CType(e.Row.FindControl("pnlEditPracticeSchemeServiceFee"), Panel)
                'CRE14-008 - Range of service fee [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim pnlEditPracticeSchemeServiceFeeCompulsory As Panel = CType(e.Row.FindControl("pnlEditPracticeSchemeServiceFeeCompulsory"), Panel)
                'CRE14-008 - Range of service fee [End][Chris YIM]
                Dim txtEditPracticeSchemeServiceFee As TextBox = CType(e.Row.FindControl("txtEditPracticeSchemeServiceFee"), TextBox)
                Dim chkEditNotProvideServiceFee As CheckBox = CType(e.Row.FindControl("chkEditNotProvideServiceFee"), CheckBox)

                Dim lblEditPracticeSchemeStatus As Label = CType(e.Row.FindControl("lblEditPracticeSchemeStatus"), Label)
                Dim lblEditPracticeSchemeRemark As Label = CType(e.Row.FindControl("lblEditPracticeSchemeRemark"), Label)
                Dim hfEditPracticeSchemeStatus As HiddenField = e.Row.FindControl("hfEditPracticeSchemeStatus")
                Dim lblEditPracticeSchemeEffectiveDtm As Label = CType(e.Row.FindControl("lblEditPracticeSchemeEffectiveDtm"), Label)
                Dim lblEditPracticeSchemeDelistDtm As Label = CType(e.Row.FindControl("lblEditPracticeSchemeDelistDtm"), Label)

                strSchemeCode = e.Row.Cells(0).Text.Trim
                strSubsidizeCode = e.Row.Cells(1).Text.Trim

                strHealthProf = CType(gvr_Parent.FindControl("hfSchemeEditHealthProf"), HiddenField).Value.Trim
                intPracticeDisplaySeq = CInt(CType(gvr_Parent.FindControl("lblSchemeEditDispalySeq"), Label).Text.Trim)
                udtPracticeSchemeInfoList = udtPracticeBLL.GetPracticeCollection.Item(intPracticeDisplaySeq).PracticeSchemeInfoList

                udtSchemeBackOffice = udtSchemeBackOfficeList.Filter(strSchemeCode)
                udtSubsidizeGroupBackOffice = udtSubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode)

                ' Scheme Code
                DirectCast(e.Row.FindControl("lblEditPracticeSchemeCode"), Label).Text = udtSchemeBackOffice.DisplayCode

                ' Show/Hide Non-clinic checkbox
                Dim chkGNonClinic As CheckBox = e.Row.FindControl("chkGNonClinic")
                chkGNonClinic.Visible = udtSchemeBackOffice.AllowNonClinicSetting

                ' Disable the Non-clinic checkbox if permanent PracticeSchemeInfo exist (of course same SchemeCode)
                If chkGNonClinic.Visible Then
                    If udtServiceProviderBLL.GetSP.SPID <> String.Empty Then
                        Dim udtSPPerm As ServiceProviderModel = Me.ServiceProviderPermanent

                        If Not IsNothing(udtSPPerm.PracticeList) _
                                AndAlso Not IsNothing(udtSPPerm.PracticeList(intPracticeDisplaySeq)) _
                                AndAlso Not IsNothing((udtSPPerm.PracticeList(intPracticeDisplaySeq).PracticeSchemeInfoList)) Then
                            Dim udtPermPSIList As PracticeSchemeInfoModelCollection = udtSPPerm.PracticeList(intPracticeDisplaySeq).PracticeSchemeInfoList.Filter(strSchemeCode)

                            If Not IsNothing(udtPermPSIList) AndAlso udtPermPSIList.Count > 0 Then
                                chkGNonClinic.Enabled = False
                                chkGNonClinic.Attributes.Add("alwaysdisable", "1")
                            End If

                        End If

                    End If

                End If

                If Not IsNothing(udtPracticeSchemeInfoList) Then
                    If udtPracticeSchemeInfoList.Count > 0 Then
                        udtPracticeSchemeInfo = udtPracticeSchemeInfoList.Item(intPracticeDisplaySeq, strSubsidizeCode, udtSchemeBackOffice.DisplaySeq, udtSubsidizeGroupBackOffice.DisplaySeq, udtSubsidizeGroupBackOffice.SchemeCode)
                    Else
                        udtPracticeSchemeInfo = Nothing
                    End If
                Else
                    udtPracticeSchemeInfo = Nothing
                End If

                chkEditNotProvideServiceFee.Attributes.Add("onclick", "javascript:enableSeviceFeeTextbox('" + chkEditNotProvideServiceFee.ClientID + "', '" + txtEditPracticeSchemeServiceFee.ClientID + "')")

                'CRE15-004 TIV & QIV [Start][Winnie]
                Dim panGNonClinic As Panel = e.Row.FindControl("panGNonClinic")

                chkEditSelect.InputAttributes.Add("scheme", strSchemeCode.Trim)
                pnlEditPracticeSchemeSubsidize.Attributes.Add("schemedepend", strSchemeCode.Trim)
                panGNonClinic.Attributes.Add("schemedepend", strSchemeCode.Trim)
                pnlEditPracticeSchemeServiceFeeDisplay.Attributes.Add("schemedepend", strSchemeCode.Trim)

                pnlEditPracticeSchemeSubsidize.Attributes.Add("subsidize", strSubsidizeCode.Trim)
                pnlEditPracticeSchemeServiceFeeDisplay.Attributes.Add("subsidizedepend", strSubsidizeCode.Trim)
                'CRE15-004 TIV & QIV [End][Winnie]

                If udtSchemeBackOffice.EligibleProfesional(strHealthProf) Then

                    udtSchemeBackOffice = udtSchemeBackOfficeList.Filter(strSchemeCode)
                    udtSubsidizeGroupBackOffice = udtSubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode)

                    Dim dtCheckedScheme As DataTable = Session(SESS_CheckedScheme)

                    chkEditSelect.Checked = False
                    Dim dv As DataView = New DataView(dtCheckedScheme)
                    dv.RowFilter = "MScheme_Code = '" + strSchemeCode.Trim + "' and Checked = 'Y'"
                    If dv.Count = 1 Then
                        chkEditSelect.Checked = True
                    Else
                        chkEditSelect.Checked = False
                    End If

                    If Not IsNothing(udtPracticeSchemeInfo) Then
                        If udtPracticeSchemeInfo.ClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
                            chkGNonClinic.Checked = True
                        End If

                        Select Case Me.hfTableLocation.Value.Trim
                            Case TableLocation.Permanent
                                Select Case udtPracticeSchemeInfo.RecordStatus.Trim
                                    Case PracticeSchemeInfoMaintenanceDisplayStatus.DelistedInvoluntary, PracticeSchemeInfoMaintenanceDisplayStatus.DelistedVoluntary
                                        chkEditSelect.Checked = False
                                        chkEditSelect.Enabled = True
                                    Case Else
                                        chkEditSelect.Enabled = False
                                End Select

                            Case TableLocation.Staging

                                Dim strPracticeSchemeStatus As String = String.Empty
                                strPracticeSchemeStatus = udtSPProfileBLL.GetPracticeSchemeInfoStatus(udtServiceProviderBLL.GetSP.PracticeList.Item(intPracticeDisplaySeq), strSchemeCode, Me.hfTableLocation.Value.Trim)

                                'Select Case udtPracticeSchemeInfo.RecordStatus.Trim
                                Select Case strPracticeSchemeStatus
                                    'CRE15-004 TIV & QIV [End][Winnie]
                                    Case PracticeSchemeInfoStagingStatus.DelistedInvoluntary, PracticeSchemeInfoStagingStatus.DelistedVoluntary
                                        'For Each dr As DataRow In dtCheckedScheme.Rows
                                        '    If CStr(dr.Item("MScheme_Code")).Trim.Equals(strSchemeCode.Trim) Then
                                        '        dr.Item("Checked") = "N"
                                        '        Exit For
                                        '    End If
                                        'Next

                                        chkEditSelect.Checked = False
                                        chkEditSelect.Enabled = True
                                    Case PracticeSchemeInfoStagingStatus.Active
                                        chkEditSelect.Enabled = True
                                    Case Else
                                        chkEditSelect.Enabled = False
                                End Select
                            Case TableLocation.Enrolment
                                chkEditSelect.Enabled = True
                        End Select

                    End If


                    If udtSchemeBackOfficeList.FilterByProfCode(strHealthProf).Count = 1 Then

                        chkEditSelect.Checked = True
                        chkEditSelect.Attributes.Add("onclick", "return false;")

                        For Each dr As DataRow In dtCheckedScheme.Rows
                            If CStr(dr.Item("MScheme_Code")).Trim.Equals(strSchemeCode) Then
                                If chkEditSelect.Checked Then
                                    dr.Item("Checked") = "Y"
                                Else
                                    dr.Item("Checked") = "N"
                                End If

                            End If
                        Next
                    End If

                    'Subsidy Column
                    lblEditPracticeSchemeSubsidizeCode.Text = udtSubsidizeGroupBackOffice.SubsidizeDisplayCode

                    If udtSubsidizeGroupBackOffice.SubsidyCompulsory Then
                        chkEditSelectSubsidize.Visible = False

                        'Make sure Subsidize always Provide Service
                        chkEditSelectSubsidize.Checked = True

                        If chkEditSelect.Checked Then
                            pnlEditPracticeSchemeSubsidize.Enabled = True
                            panGNonClinic.Enabled = True
                        Else
                            pnlEditPracticeSchemeSubsidize.Enabled = False
                            panGNonClinic.Enabled = False
                        End If
                    Else
                        chkEditSelectSubsidize.Visible = True
                        pnlEditPracticeSchemeSubsidize.Style.Add("cursor", "default")

                        If chkEditSelect.Checked Then
                            pnlEditPracticeSchemeSubsidize.Enabled = True
                            panGNonClinic.Enabled = True

                            If Not IsNothing(udtPracticeSchemeInfo) Then
                                If udtPracticeSchemeInfo.ProvideService Then
                                    chkEditSelectSubsidize.Checked = True
                                Else
                                    chkEditSelectSubsidize.Checked = False
                                    'txtEditPracticeSchemeServiceFee.Text = String.Empty
                                End If
                            End If
                        Else
                            pnlEditPracticeSchemeSubsidize.Enabled = False
                            panGNonClinic.Enabled = False
                            chkEditSelectSubsidize.Checked = False
                        End If
                    End If

                    'Hide row in edit mode if subsidy is expired when...
                    If udtSubsidizeGroupBackOffice.RecordStatus = SubsidizeGroupBackOfficeStatus.Expired Then
                        'scheme is delisted or new enrolled 
                        If chkEditSelect.Enabled Then
                            e.Row.Visible = False
                        Else
                            'not provide service
                            If Not IsNothing(udtPracticeSchemeInfo) Then
                                If Not udtPracticeSchemeInfo.ProvideService Then
                                    e.Row.Visible = False
                                End If
                            Else
                                e.Row.Visible = False
                            End If
                        End If
                    End If
                    'CRE15-004 TIV & QIV [End][Winnie]

                    Dim blnBindServiceFee As Boolean = False

                    If udtSubsidizeGroupBackOffice.ServiceFeeEnabled Then
                        pnlEditPracticeSchemeServiceFee.Visible = True
                        'CRE14-008 - Range of service fee [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        pnlEditPracticeSchemeServiceFeeCompulsory.Visible = True
                        'CRE14-008 - Range of service fee [End][Chris YIM]
                        lblEditPracticeSchemeServiceFee.Visible = False

                        chkEditNotProvideServiceFee.Text = udtSubsidizeGroupBackOffice.ServiceFeeCompulsoryWording

                        If udtSubsidizeGroupBackOffice.ServiceFeeCompulsory Then
                            chkEditNotProvideServiceFee.Visible = False

                            'CRE15-004 TIV & QIV [Start][Winnie]
                            'If chkEditSelect.Checked Then
                            If chkEditSelect.Checked AndAlso chkEditSelectSubsidize.Checked Then
                                'CRE15-004 TIV & QIV [End][Winnie]
                                blnBindServiceFee = True
                            Else
                                blnBindServiceFee = False
                            End If

                        Else
                            chkEditNotProvideServiceFee.Visible = True
                            chkEditNotProvideServiceFee.Enabled = True

                            'CRE15-004 TIV & QIV [Start][Winnie]
                            'If chkEditSelect.Checked Then
                            If chkEditSelect.Checked AndAlso chkEditSelectSubsidize.Checked Then
                                'CRE15-004 TIV & QIV [End][Winnie]
                                blnBindServiceFee = True
                            Else
                                blnBindServiceFee = False
                            End If

                        End If

                        If blnBindServiceFee Then
                            If Not IsNothing(udtPracticeSchemeInfo) Then
                                If udtPracticeSchemeInfo.ProvideServiceFee.HasValue Then
                                    If udtPracticeSchemeInfo.ProvideServiceFee Then
                                        txtEditPracticeSchemeServiceFee.Text = udtPracticeSchemeInfo.ServiceFee.Value
                                    Else
                                        txtEditPracticeSchemeServiceFee.Text = String.Empty

                                        If chkEditNotProvideServiceFee.Visible Then
                                            chkEditNotProvideServiceFee.Checked = True
                                        End If

                                    End If
                                Else
                                    txtEditPracticeSchemeServiceFee.Text = String.Empty
                                End If
                            Else
                                txtEditPracticeSchemeServiceFee.Text = String.Empty
                            End If

                            If chkEditNotProvideServiceFee.Checked Then
                                txtEditPracticeSchemeServiceFee.BackColor = Drawing.Color.WhiteSmoke
                                txtEditPracticeSchemeServiceFee.Attributes.Add("readonly", "readonly")
                            Else
                                txtEditPracticeSchemeServiceFee.BackColor = Nothing
                                txtEditPracticeSchemeServiceFee.Attributes.Remove("readonly")
                            End If

                            'CRE15-004 TIV & QIV [Start][Winnie]
                            pnlEditPracticeSchemeServiceFeeDisplay.Enabled = True
                            'CRE15-004 TIV & QIV [End][Winnie]
                        Else
                            chkEditNotProvideServiceFee.Enabled = False

                            txtEditPracticeSchemeServiceFee.Text = String.Empty
                            txtEditPracticeSchemeServiceFee.BackColor = Drawing.Color.WhiteSmoke
                            txtEditPracticeSchemeServiceFee.Attributes.Add("readonly", "readonly")

                            'CRE15-004 TIV & QIV [Start][Winnie]
                            pnlEditPracticeSchemeServiceFeeDisplay.Enabled = False
                            'CRE15-004 TIV & QIV [End][Winnie]
                        End If

                    Else

                        pnlEditPracticeSchemeServiceFee.Visible = False
                        'CRE14-008 - Range of service fee [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        pnlEditPracticeSchemeServiceFeeCompulsory.Visible = False
                        'CRE14-008 - Range of service fee [End][Chris YIM]
                        lblEditPracticeSchemeServiceFee.Visible = True

                        'CRE15-004 TIV & QIV [Start][Winnie]
                        If chkEditSelect.Checked AndAlso chkEditSelectSubsidize.Checked Then
                            pnlEditPracticeSchemeServiceFeeDisplay.Enabled = True
                        Else
                            pnlEditPracticeSchemeServiceFeeDisplay.Enabled = False
                        End If
                        'CRE15-004 TIV & QIV [End][Winnie]
                    End If

                    ' CRE15-004 TIV & QIV [Start][Winnie]
                    Dim udtPractice As PracticeModel = udtServiceProviderBLL.GetSP.PracticeList.Item(intPracticeDisplaySeq)

                    If Not IsNothing(udtPractice.PracticeSchemeInfoList) Then
                        lblEditPracticeSchemeStatus.Text = udtSPProfileBLL.GetPracticeSchemeInfoStatus(udtPractice, strSchemeCode, Me.hfTableLocation.Value.Trim)

                        If hfTableLocation.Value.Trim.Equals(TableLocation.Staging) AndAlso lblEditPracticeSchemeStatus.Text = PracticeSchemeInfoStagingStatus.Update Then
                            ' Clear Remark if scheme is under amendment
                            lblEditPracticeSchemeRemark.Text = String.Empty

                        Else
                            ' Otherwise, get any one of the non-empty remarks from PracticeSchemeInfo
                            Dim strRemark As String = String.Empty

                            For Each udtPSI As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                                If udtPSI.SchemeCode = strSchemeCode Then
                                    If udtPSI.Remark <> String.Empty Then
                                        strRemark = udtPSI.Remark
                                        Exit For
                                    End If
                                End If
                            Next

                            lblEditPracticeSchemeRemark.Text = strRemark

                        End If

                    End If
                    ' CRE15-004 TIV & QIV [End][Winnie]

                    If Not IsNothing(udtPracticeSchemeInfo) Then
                        hfEditPracticeSchemeStatus.Value = udtPracticeSchemeInfo.RecordStatus.Trim
                    End If

                    Select Case hfTableLocation.Value.Trim
                        Case TableLocation.Permanent
                            Status.GetDescriptionFromDBCode(PracticeSchemeInfoMaintenanceDisplayStatus.ClassCode, lblEditPracticeSchemeStatus.Text.Trim, lblEditPracticeSchemeStatus.Text, String.Empty)

                        Case TableLocation.Staging
                            Status.GetDescriptionFromDBCode(PracticeSchemeInfoStagingStatus.ClassCode, lblEditPracticeSchemeStatus.Text.Trim, lblEditPracticeSchemeStatus.Text, String.Empty)

                        Case TableLocation.Enrolment
                            lblEditPracticeSchemeStatus.Text = "Unprocessed"
                    End Select

                    If lblEditPracticeSchemeStatus.Text.Trim.Equals(String.Empty) Then
                        lblEditPracticeSchemeStatus.Text = Me.GetGlobalResourceObject("Text", "N/A")
                    End If

                    If Not lblEditPracticeSchemeRemark.Text.Trim.Equals(String.Empty) Then
                        lblEditPracticeSchemeRemark.Text = "[" + lblEditPracticeSchemeRemark.Text + "]"
                    End If

                    udtSPProfileBLL.GetPracticeSchemeInfoEarliestTime(udtPracticeSchemeInfoList, strSchemeCode,
                                                                      lblEditPracticeSchemeEffectiveDtm.Text, lblEditPracticeSchemeDelistDtm.Text)

                    If lblEditPracticeSchemeEffectiveDtm.Text.Equals(String.Empty) Then
                        lblEditPracticeSchemeEffectiveDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
                    End If

                    If lblEditPracticeSchemeDelistDtm.Text.Equals(String.Empty) Then
                        lblEditPracticeSchemeDelistDtm.Text = Me.GetGlobalResourceObject("Text", "N/A")
                    End If

                Else
                    e.Row.Visible = False
                End If

            End If

        ElseIf e.Row.RowType = DataControlRowType.Header Then
            'e.Row.Cells(3).ColumnSpan = "2"
            'e.Row.Cells.Remove(e.Row.Cells(4))
            e.Row.Cells(6).Text = Me.GetGlobalResourceObject("Text", "Status") + " [" + Me.GetGlobalResourceObject("Text", "Remarks") + "]"

        End If

    End Sub

    Protected Sub gvServiceFee_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)

        e.Row.Cells(0).Visible = False
        e.Row.Cells(1).Visible = False

    End Sub

    Protected Sub gvServiceFee_PreRender(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim gvServiceFee As GridView = CType(sender, GridView)

        gvServiceFee.HeaderRow.Cells(4).ColumnSpan = 2
        gvServiceFee.HeaderRow.Cells.Remove(gvServiceFee.HeaderRow.Cells(5))

        ' Handle Category
        For Each gvr As GridViewRow In gvServiceFee.Rows
            If DirectCast(gvr.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "Y" Then
                ' Check whether this category is visible
                Dim strSchemeCode As String = gvr.Cells(0).Text
                Dim strCategoryName As String = DirectCast(gvr.FindControl("hfGCategoryName"), HiddenField).Value
                Dim blnVisible As Boolean = False

                For Each r As GridViewRow In gvServiceFee.Rows
                    If DirectCast(r.FindControl("hfGIsCategoryHeader"), HiddenField).Value = "N" _
                            AndAlso r.Cells(0).Text = strSchemeCode _
                            AndAlso DirectCast(r.FindControl("hfGCategoryName"), HiddenField).Value = strCategoryName _
                            AndAlso r.Visible Then
                        blnVisible = True
                        Exit For
                    End If

                Next

                If blnVisible Then
                    ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [Start][Lawrence]
                    gvr.Cells(4).Text = AntiXssEncoder.HtmlEncode(DirectCast(gvr.FindControl("hfGCategoryName"), HiddenField).Value, True)
                    ' I-CRE16-007 Fix vulnerabilities found by Checkmarx [End][Lawrence]
                    gvr.Cells(4).ColumnSpan = 2
                    gvr.Cells(4).CssClass = "SubsidizeCategoryHeader"
                    gvr.Cells(5).Visible = False

                Else
                    gvr.Visible = False

                End If

            End If

        Next

        ' End of Handle Category





        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
        Dim strPreviousScheme As String = String.Empty

        For Each gvr As GridViewRow In gvServiceFee.Rows
            If Not gvr.Visible Then
                Continue For
            End If

            Dim strSchemeCode As String = gvr.Cells(0).Text.Trim

            If Not udtSchemeBackOfficeList.Filter(strSchemeCode).DisplaySubsidizeDesc Then
                gvr.Cells(4).ColumnSpan = 2
                gvr.Cells(5).Visible = False
                gvr.Cells(4).Text = Me.GetGlobalResourceObject("Text", "N/A")

            End If

            ' Grouping depends on gridview instead of subsidizelist
            Dim RowCount As Integer = 0

            If Not strPreviousScheme.Equals(strSchemeCode) Then

                For Each gvrow As GridViewRow In gvServiceFee.Rows
                    If gvrow.Visible Then
                        If gvrow.Cells(0).Text.Trim = strSchemeCode Then
                            RowCount += 1
                        End If
                    End If
                Next

                gvr.Cells(2).RowSpan = RowCount
                gvr.Cells(3).RowSpan = RowCount
                gvr.Cells(6).RowSpan = RowCount
                gvr.Cells(7).RowSpan = RowCount
                gvr.Cells(8).RowSpan = RowCount

                Dim blnAllNotProvideService As Boolean = False

                For Each gvrow As GridViewRow In gvServiceFee.Rows
                    If gvrow.Cells(0).Text.Trim = strSchemeCode AndAlso DirectCast(gvrow.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y" Then
                        blnAllNotProvideService = True
                        Exit For
                    End If

                Next

                If blnAllNotProvideService Then
                    gvr.Cells(4).Text = Me.GetGlobalResourceObject("Text", "NoServiceFeesProvided")
                    gvr.Cells(4).CssClass = "tableText"
                    gvr.Cells(4).RowSpan = RowCount
                    gvr.Cells(4).ColumnSpan = 2
                    gvr.Cells(5).Visible = False
                End If

            Else
                gvr.Cells(2).Visible = False
                gvr.Cells(3).Visible = False
                gvr.Cells(6).Visible = False
                gvr.Cells(7).Visible = False
                gvr.Cells(8).Visible = False

                Dim blnAllNotProvideService As Boolean = False

                For Each gvrow As GridViewRow In gvServiceFee.Rows
                    If gvrow.Cells(0).Text.Trim = strSchemeCode AndAlso DirectCast(gvrow.FindControl("hfGAllNotProvideService"), HiddenField).Value = "Y" Then
                        blnAllNotProvideService = True
                        Exit For
                    End If

                Next

                If blnAllNotProvideService Then
                    gvr.Cells(4).Visible = False
                    gvr.Cells(5).Visible = False
                End If

            End If

            strPreviousScheme = strSchemeCode

        Next

    End Sub

    Private Sub gvServiceFeeShowImageAfterDataBind(ByVal gv As GridView)

        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = Nothing
        Dim udtSubsidizeGroupBackOfficeList As SubsidizeGroupBackOfficeModelCollection = Nothing

        udtSchemeBackOfficeList = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache
        udtSubsidizeGroupBackOfficeList = udtSchemeBackOfficeBLL.GetAllEffectiveSubsidizeGroupFromCache

        'If udtSchemeBackOfficeBLL.ExistSession_SchemeBackOfficeWithSubsidizeGroup Then
        '    udtSchemeBackOfficeList = udtSchemeBackOfficeBLL.GetSession_SchemeBackOfficeWithSubsidizeGroup
        'End If

        'If udtSchemeBackOfficeBLL.ExistSession_SubsidizeGroupBackOffice Then
        '    udtSubsidizeGroupBackOfficeList = udtSchemeBackOfficeBLL.GetSession_SubsidizeGroupBackOffice
        'End If

        Dim dtCheckedScheme As New DataTable
        dtCheckedScheme = Session(SESS_CheckedScheme)

        Dim strTempSF As New List(Of String)
        Dim strTempNotProvider As New List(Of String)

        Dim strTempMSchemeCode As New List(Of String)

        Dim udtSchemeBackOffice As SchemeBackOfficeModel
        Dim udtSubsidizeGroupBackOffice As SubsidizeGroupBackOfficeModel

        'CRE15-004 TIV & QIV [Start][Winnie]
        Dim strPreviousScheme As String = String.Empty
        Dim blnSelectSubsidize As Boolean = False
        'CRE15-004 TIV & QIV [End][Winnie]

        'CRE15-004 TIV & QIV [Start][Winnie] Remark
        'For Each gvr As GridViewRow In gv.Rows
        '    Dim strSchemeCode As String = gvr.Cells(0).Text.Trim
        '    Dim strSubsidizeCode As String = gvr.Cells(1).Text.Trim

        '    udtSchemeBackOffice = udtSchemeBackOfficeList.Filter(strSchemeCode)
        '    udtSubsidizeGroupBackOffice = udtSubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode)

        '    Dim txttemp As TextBox = CType(gvr.FindControl("txtEditPracticeSchemeServiceFee"), TextBox)
        '    Dim chkTemp As CheckBox = CType(gvr.FindControl("chkEditNotProvideServiceFee"), CheckBox)
        '    'CRE14-008 - Range of service fee [Start][Chris YIM]
        '    '-----------------------------------------------------------------------------------------
        '    'Dim pnlTemp As Panel = CType(gvr.FindControl("pnlEditPracticeSchemeServiceFee"), Panel)
        '    'CRE14-008 - Range of service fee [End][Chris YIM]

        '    If udtSubsidizeGroupBackOffice.ServiceFeeEnabled Then
        '        If udtSubsidizeGroupBackOffice.ServiceFeeCompulsory Then
        '            strTempNotProvider.Add("N")
        '            strTempSF.Add(txttemp.Text.Trim)
        '        Else
        '            If chkTemp.Checked Then
        '                strTempNotProvider.Add("Y")
        '                strTempSF.Add(String.Empty)
        '            Else
        '                strTempNotProvider.Add("N")
        '                strTempSF.Add(txttemp.Text.Trim)
        '            End If
        '        End If
        '    Else
        '        strTempNotProvider.Add(String.Empty)
        '        strTempSF.Add(String.Empty)
        '    End If

        'Next

        'gv.DataSource = udtSubsidizeGroupBackOfficeList
        'gv.DataBind()
        'CRE15-004 TIV & QIV [End][Winnie] Remark

        Dim strPreviousSchemeCode As String = String.Empty


        SM = Validator.chkGridSelectedNothing(gv, "chkEditSelect", 1)
        If Not SM Is Nothing Then
            gv.HeaderRow.FindControl("imgEditSelectAlert").Visible = True
        Else

            For Each row As GridViewRow In gv.Rows
                Dim chkEditSelect As CheckBox
                'CRE14-008 - Range of service fee [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'Dim pnlEditPracticeSchemeServiceFee As Panel
                'CRE14-008 - Range of service fee [End][Chris YIM]
                Dim txtEditPracticeSchemeServiceFee As TextBox
                Dim chkEditNotProvideServiceFee As CheckBox
                Dim imgEditServiceFeeAlert As Image
                'CRE15-004 TIV & QIV [Start][Winnie]
                Dim imgEditSelectSubsidizeAlert As Image
                'CRE15-004 TIV & QIV [End][Winnie]

                chkEditSelect = CType(row.FindControl("chkEditSelect"), CheckBox)

                Dim strSchemeCode As String = row.Cells(0).Text.Trim
                Dim strSubsidizeCode As String = row.Cells(1).Text.Trim

                udtSchemeBackOffice = udtSchemeBackOfficeList.Filter(strSchemeCode)
                udtSubsidizeGroupBackOffice = udtSubsidizeGroupBackOfficeList.Filter(strSchemeCode, strSubsidizeCode)

                chkEditSelect.Checked = False
                Dim dv As DataView = New DataView(dtCheckedScheme)
                dv.RowFilter = "MScheme_Code = '" + strSchemeCode.Trim + "' and Checked = 'Y'"
                If dv.Count = 1 Then
                    chkEditSelect.Checked = True
                Else
                    chkEditSelect.Checked = False
                End If

                If chkEditSelect.Checked Then

                    'CRE15-004 TIV & QIV [Start][Winnie]  
                    imgEditSelectSubsidizeAlert = CType(row.FindControl("imgEditSelectSubsidizeAlert"), Image)
                    imgEditSelectSubsidizeAlert.Visible = False

                    If Not strPreviousScheme.Trim.Equals(strSchemeCode.Trim) Then
                        blnSelectSubsidize = False

                        For Each gvrTemp As GridViewRow In gv.Rows
                            Dim chkTemp As CheckBox = CType(gvrTemp.FindControl("chkEditSelectSubsidize"), CheckBox)
                            If strSchemeCode.Trim.Equals(gvrTemp.Cells(0).Text.Trim) Then
                                If chkTemp.Checked Then
                                    blnSelectSubsidize = True
                                End If
                            End If
                        Next
                    End If

                    If Not blnSelectSubsidize Then
                        imgEditSelectSubsidizeAlert.Visible = True
                    End If

                    strPreviousScheme = strSchemeCode

                    Dim chkEditSelectSubsidize As CheckBox = CType(row.FindControl("chkEditSelectSubsidize"), CheckBox)
                    If chkEditSelectSubsidize.Checked Then
                        'CRE15-004 TIV & QIV [End][Winnie]

                        'CRE14-008 - Range of service fee [Start][Chris YIM]
                        '-----------------------------------------------------------------------------------------
                        'pnlEditPracticeSchemeServiceFee = CType(row.FindControl("pnlEditPracticeSchemeServiceFee"), Panel)
                        'CRE14-008 - Range of service fee [End][Chris YIM]
                        txtEditPracticeSchemeServiceFee = CType(row.FindControl("txtEditPracticeSchemeServiceFee"), TextBox)
                        chkEditNotProvideServiceFee = CType(row.FindControl("chkEditNotProvideServiceFee"), CheckBox)
                        imgEditServiceFeeAlert = CType(row.FindControl("imgEditServiceFeeAlert"), Image)
                        imgEditServiceFeeAlert.Visible = False

                        If udtSubsidizeGroupBackOffice.ServiceFeeEnabled Then
                            '    'CRE15-004 TIV & QIV [Start][Winnie] Remark
                            '    If udtSubsidizeGroupBackOffice.ServiceFeeCompulsory Then
                            '        chkEditNotProvideServiceFee.Checked = False
                            '        txtEditPracticeSchemeServiceFee.Text = strTempSF(row.RowIndex).Trim

                            '        txtEditPracticeSchemeServiceFee.BackColor = Nothing
                            '        txtEditPracticeSchemeServiceFee.Attributes.Remove("readonly")
                            '    Else
                            '        If strTempNotProvider(row.RowIndex).Trim.Equals("Y") Then
                            '            chkEditNotProvideServiceFee.Checked = True
                            '            txtEditPracticeSchemeServiceFee.Text = String.Empty

                            '            txtEditPracticeSchemeServiceFee.BackColor = Drawing.Color.WhiteSmoke
                            '            txtEditPracticeSchemeServiceFee.Attributes.Add("readonly", "readonly")

                            '        ElseIf strTempNotProvider(row.RowIndex).Trim.Equals("N") Then
                            '            chkEditNotProvideServiceFee.Checked = False
                            '            txtEditPracticeSchemeServiceFee.Text = strTempSF(row.RowIndex).Trim

                            '            txtEditPracticeSchemeServiceFee.BackColor = Nothing
                            '            txtEditPracticeSchemeServiceFee.Attributes.Remove("readonly")
                            '        End If
                            '    End If
                            'CRE15-004 TIV & QIV [End][Winnie] Remark

                            If Not chkEditNotProvideServiceFee.Checked Then
                                SM = Validator.chkServiceFee(txtEditPracticeSchemeServiceFee.Text.Trim)
                                If Not IsNothing(SM) Then
                                    imgEditServiceFeeAlert.Visible = True

                                End If
                            End If
                        End If

                        'If Not strPerviousMScheme.Trim.Equals(strMasterSchemeCode.Trim) Then
                        '    Dim blnHadServiceFee As Boolean = False
                        '    For Each gvrTemp As GridViewRow In gv.Rows
                        '        Dim chkTemp As CheckBox = CType(gvrTemp.FindControl("chkEditNotProvideServiceFee"), CheckBox)
                        '        If strMasterSchemeCode.Trim.Equals(gvrTemp.Cells(0).Text.Trim) Then
                        '            If Not chkTemp.Checked Then
                        '                blnHadServiceFee = True
                        '            End If
                        '        End If
                        '    Next

                        '    If Not blnHadServiceFee Then
                        '        strTempMSchemeCode.Add(strMasterSchemeCode.Trim)
                        '    End If

                        '    strPerviousMScheme = strMasterSchemeCode
                        'End If

                    End If
                    'CRE15-004 TIV & QIV [Start][Winnie]
                End If
                'CRE15-004 TIV & QIV [End][Winnie]
            Next
        End If

        'For Each row As GridViewRow In gv.Rows
        '    Dim imgEditServiceFeeAlert As Image = CType(row.FindControl("imgEditServiceFeeAlert"), Image)
        '    For j As Integer = 0 To strTempMSchemeCode.Count - 1
        '        If row.Cells(0).Text.Trim.Equals(strTempMSchemeCode(j).Trim) Then
        '            imgEditServiceFeeAlert.Visible = True
        '        End If
        '    Next
        'Next

        For Each gvr As GridViewRow In gvSI.Rows
            Dim gvPracticeSchemeInfo As GridView = CType(gvr.FindControl("gvPracticeSchemeInfo"), GridView)
            If Not IsNothing(gvPracticeSchemeInfo) Then
                gvPracticeSchemeInfo.DataSource = udtSubsidizeGroupBackOfficeList.ToSPProfileDataTable
                gvPracticeSchemeInfo.DataBind()

                ApplyPracticeSchemeChangeIndicator(gvr)
            Else
                Dim gvEditPracticeSchemeInfo As GridView = CType(gvr.FindControl("gvEditPracticeSchemeInfo"), GridView)
                If Not IsNothing(gvEditPracticeSchemeInfo) Then

                    ApplyPracticeSchemeChangeIndicator(gvr, True)

                End If
            End If

        Next

    End Sub

#End Region

    Protected Sub ibtnPageChecked_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Me.msgBox.Visible = False

        Dim udtAuditLogEntry As AuditLogEntry = Nothing
        Dim strStartLog As String = String.Empty
        Dim strEndLog As String = String.Empty
        Dim strError As String = String.Empty
        Dim strLogID As String = String.Empty

        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSPPermenant As ServiceProviderModel
        Dim udtSPStaging As ServiceProviderModel

        Try
            Dim udtSPModel As ServiceProviderModel = Me.udtServiceProviderBLL.GetSP()

            Dim blnResInValid As Boolean = False

            If String.IsNullOrEmpty(udtSPModel.SPID) = False Then
                ' Have permanent record, check problem

                udtSPPermenant = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(Me.hfERN.Value.Trim, New Common.DataAccess.Database)
                udtSPStaging = udtServiceProviderBLL.GetServiceProviderStagingByERN(Me.hfERN.Value.Trim, New Common.DataAccess.Database)

                blnResInValid = udtSPProfileBLL.CheckUnsynchronizeRecord(udtSPStaging, udtSPPermenant)

            End If

            'udtSPPermenant = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(Me.hfERN.Value.Trim, New Common.DataAccess.Database)
            'udtSPStaging = udtServiceProviderBLL.GetServiceProviderStagingByERN(Me.hfERN.Value.Trim, New Common.DataAccess.Database)

            If Not blnResInValid Then
                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

                Dim i As Integer = TabContainer1.ActiveTabIndex
                Select Case i
                    Case 0 'Personal Particulars
                        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                        strLogID = Common.Component.LogID.LOG00031
                        strStartLog = "Page checked personal"
                        strEndLog = "Page checked personal Completed."
                        strError = "Page checked personal Fail"
                    Case 1 'Medical Organization
                        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                        strLogID = Common.Component.LogID.LOG00052
                        strStartLog = "Page checked MO"
                        strEndLog = "Page checked MO Completed."
                        strError = "Page checked MO Fail"
                    Case 2 'Practice Info
                        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                        strLogID = Common.Component.LogID.LOG00034
                        strStartLog = "Page checked practice"
                        strEndLog = "Page checked practice Completed."
                        strError = "Page checked practice Fail"
                    Case 3 ' Bank Info
                        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                        strLogID = Common.Component.LogID.LOG00037
                        strStartLog = "Page checked bank account"
                        strEndLog = "Page checked bank Completed."
                        strError = "Page checked bank Fail"
                    Case 4 ' Scheme Info
                        udtAuditLogEntry = New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log
                        strLogID = Common.Component.LogID.LOG00055
                        strStartLog = "Page checked scheme"
                        strEndLog = "Page checked scheme Completed."
                        strError = "Page checked scheme Fail"
                End Select

                udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)
                If udtServiceProviderBLL.Exist Then
                    udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
                Else
                    udtAuditLogEntry.AddDescripton("SPID", "")
                End If

                udtAuditLogEntry.WriteStartLog(strLogID, strStartLog)

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                'New checking for Bank Info Tab 
                If Me.TabContainer1.ActiveTabIndex = 3 Then

                    If udtServiceProviderBLL.Exist Then

                        ' INT17-0012 (Fix EForm Bank Account can Next with no input) [Start] [Winnie]
                        '-----------------------------------------------------------------------------------------
                        ' Add checking for branch name since branch name is optional in eform but mandatory in VU platform
                        '-----------------------------------------------------------------------------------------
                        Dim strPracticeIndex As String = String.Empty

                        ' Check branch name 
                        If Not blnResInValid Then
                            strPracticeIndex = udtSPProfileBLL.CheckBankBranchName(udtServiceProviderBLL.GetSP)

                            If Not strPracticeIndex.Trim.Equals(String.Empty) Then
                                blnResInValid = True
                                msgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00397, "%s", strPracticeIndex.Substring(2))
                                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00039, strError)
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
                            End If
                        End If
                        ' INT17-0012 (Fix EForm Bank Account can Next with no input) [End] [Winnie]

                        If Not blnResInValid Then
                            strPracticeIndex = udtSPProfileBLL.CheckBankAllowFreeFormat(udtServiceProviderBLL.GetSP)

                            If Not strPracticeIndex.Trim.Equals(String.Empty) Then
                                blnResInValid = True
                                msgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00346, "%s", strPracticeIndex.Substring(2))
                                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00039, strError)
                                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
                            End If
                        End If
                    End If
                End If

                'New checking for Scheme Info Tab
                If Me.TabContainer1.ActiveTabIndex = 4 Then

                    If udtServiceProviderBLL.Exist Then
                        Dim strPracticeIndex As String = udtSPProfileBLL.CheckBankAllowFreeFormat(udtServiceProviderBLL.GetSP)

                        If Not strPracticeIndex.Trim.Equals(String.Empty) Then
                            blnResInValid = True
                            msgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00347, "%s", strPracticeIndex.Substring(2))
                            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00057, strError)
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
                        End If
                    End If
                End If
                'CRE13-019-02 Extend HCVS to China [End][Winnie]

                If Not blnResInValid Then
                    If udtSPProfileBLL.PageChecked(i, Me.hfERN.Value.Trim, hfTableLocation.Value.Trim) Then
                        'SetActionImg()
                        If udtSPProfileBLL.GetServiceProviderProfile(hfERN.Value.Trim, TableLocation.Staging) Then
                            hfTableLocation.Value = TableLocation.Staging
                            'udtSPProfileBLL.BindDataToControlForDataEntry(hfERN.Value.Trim, fvPersonalParticulars, gvPracticeInfo, gvMOInfo, gvBankInfo)

                            Me.BindSPProfile()
                        End If

                    End If

                    If strLogID.Equals(Common.Component.LogID.LOG00031) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00032, strEndLog)
                    ElseIf strLogID.Equals(Common.Component.LogID.LOG00034) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00035, strEndLog)
                    ElseIf strLogID.Equals(Common.Component.LogID.LOG00037) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00038, strEndLog)
                    ElseIf strLogID.Equals(Common.Component.LogID.LOG00052) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00053, strEndLog)
                    ElseIf strLogID.Equals(Common.Component.LogID.LOG00055) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00056, strEndLog)
                    End If
                End If

                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
            Else
                msgBox.AddMessage(FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00011, "%s", "")
                msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00115, "Page Checked abort")
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
            End If
            'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                If strmsg.Trim.Equals(String.Empty) Then
                    If strLogID.Equals(Common.Component.LogID.LOG00031) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00033, strError)
                    ElseIf strLogID.Equals(Common.Component.LogID.LOG00034) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00036, strError)
                    ElseIf strLogID.Equals(Common.Component.LogID.LOG00037) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00039, strError)
                    ElseIf strLogID.Equals(Common.Component.LogID.LOG00052) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00054, strError)
                    ElseIf strLogID.Equals(Common.Component.LogID.LOG00055) Then
                        udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00057, strError)
                    End If
                Else
                    SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                    msgBox.AddMessage(SM)
                    If msgBox.GetCodeTable.Rows.Count = 0 Then
                        msgBox.Visible = False
                    Else
                        'msgBox.BuildMessageBox("UpdateFail")
                        If strLogID.Equals(Common.Component.LogID.LOG00031) Then
                            msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00033, strError)
                        ElseIf strLogID.Equals(Common.Component.LogID.LOG00034) Then
                            msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00036, strError)
                        ElseIf strLogID.Equals(Common.Component.LogID.LOG00037) Then
                            msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00039, strError)
                        ElseIf strLogID.Equals(Common.Component.LogID.LOG00052) Then
                            msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00054, strError)
                        ElseIf strLogID.Equals(Common.Component.LogID.LOG00055) Then
                            msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00057, strError)
                        End If

                    End If

                End If



                'CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Information
            Else
                Throw eSQL
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    '

    Protected Sub ibtnBack_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        Dim udtAuditLogEntry As New AuditLogEntry(Me.FunctionCode, Me)
        udtAuditLogEntry.WriteLog(LogID.LOG00103, "Back Click")
        ' CRE11-021 log the missed essential information [End]

        Me.hfERN.Value = String.Empty
        Me.hfGridviewIndex.Value = String.Empty
        Me.hfSelectedAddressAreaCode.Value = String.Empty
        Me.hfSelectedAddressDistrictCode.Value = String.Empty
        Me.hfSelectedAddressEng.Value = String.Empty
        Me.hfSelectedAddressChi.Value = String.Empty
        Me.hfSelectedAddressRecordID.Value = String.Empty
        Me.hfTableLocation.Value = String.Empty

        'udtServiceProviderBLL.ClearSession()
        'udtPracticeBLL.ClearSession()
        'udtBankAcctBLL.ClearSession()
        'udtProfessionalBLL.ClearSession()
        'udtSPVerificationBLL.ClearSession()

        udtSPProfileBLL.ClearSession()
        Session("BackToDataEntryPage") = True

        Response.Redirect("~/ServiceProvider/spDataEntry.aspx")
    End Sub

    Protected Sub ibtnProceedToVetting_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log

        msgBox.Visible = False
        CompleteMsgBox.Visible = False

        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSPPermenant As ServiceProviderModel
        Dim udtSPStaging As ServiceProviderModel

        Dim udtSPModel As ServiceProviderModel = Me.udtServiceProviderBLL.GetSP()

        Dim blnResInValid As Boolean = False

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Clear PCD checking result
        Session(SESS_PCDProfessionalChecked) = Nothing
        Session(SESS_PCDStatusChecked) = Nothing
        Session(SESS_PCDCheckAccountStatusResult) = Nothing

        udtAuditLogEntry.AddDescripton("ERN", udtServiceProviderBLL.GetSP.EnrolRefNo)
        udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00120, "Proceed To Vetting Click")
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

        If String.IsNullOrEmpty(udtSPModel.SPID) = False Then
            ' Have permanent record, check problem

            udtSPPermenant = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(Me.hfERN.Value.Trim, New Common.DataAccess.Database)
            udtSPStaging = udtServiceProviderBLL.GetServiceProviderStagingByERN(Me.hfERN.Value.Trim, New Common.DataAccess.Database)

            blnResInValid = udtSPProfileBLL.CheckUnsynchronizeRecord(udtSPStaging, udtSPPermenant)

        End If

        'udtSPPermenant = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(Me.hfERN.Value.Trim, New Common.DataAccess.Database)
        'udtSPStaging = udtServiceProviderBLL.GetServiceProviderStagingByERN(Me.hfERN.Value.Trim, New Common.DataAccess.Database)

        If Not blnResInValid Then
            'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

            If udtServiceProviderBLL.Exist Then

                ' INT17-0012 (Fix EForm Bank Account can Next with no input) [Start] [Winnie]
                '-----------------------------------------------------------------------------------------
                ' Add checking for branch name since branch name is optional in eform but mandatory in VU platform
                '-----------------------------------------------------------------------------------------
                Dim strPracticeIndex As String = String.Empty

                ' Check branch name 
                If Not blnResInValid Then
                    strPracticeIndex = udtSPProfileBLL.CheckBankBranchName(udtServiceProviderBLL.GetSP)

                    If Not strPracticeIndex.Trim.Equals(String.Empty) Then
                        blnResInValid = True
                        msgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00397, "%s", strPracticeIndex.Substring(2))
                    End If
                End If
                ' INT17-0012 (Fix EForm Bank Account can Next with no input) [End] [Winnie]

                If Not blnResInValid Then
                    strPracticeIndex = udtSPProfileBLL.CheckBankAllowFreeFormat(udtServiceProviderBLL.GetSP)

                    If Not strPracticeIndex.Trim.Equals(String.Empty) Then
                        blnResInValid = True
                        msgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00348, "%s", strPracticeIndex.Substring(2))
                    End If
                End If

                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Check PCD Status
                If Not blnResInValid Then
                    blnResInValid = Me.ValidatePCDStatus(udtServiceProviderBLL.GetSP)
                End If
                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

                If blnResInValid Then
                    udtAuditLogEntry.AddDescripton("ERN", udtServiceProviderBLL.GetSP.EnrolRefNo)
                    udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
                    udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00040, "Proceed Vetting:")

                    msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00042, "Proceed Vetting Fail.")
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
                End If
            End If

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If Not blnResInValid Then
                ' Success
                ProceedToVetting()
            End If
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

            'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
        Else
            msgBox.AddMessage(FunctCode.FUNT010203, SeverityCode.SEVI, MsgCode.MSG00011, "%s", "")
            msgBox.BuildMessageBox("ValidationFail", udtAuditLogEntry, Common.Component.LogID.LOG00116, "Proceed To Vetting abort")
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "ScrollPage", "ResetScrollPosition();", True)
        End If
        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
    End Sub

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Private Sub ProceedToVetting()
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)
        Dim blnResInValid As Boolean = False

        ' PCD Checking
        blnResInValid = Me.CheckPCDWarning(udtServiceProviderBLL.GetSP)

        If blnResInValid Then
            Return
        End If

        Try
            If udtSPProfileBLL.ConfirmSPProfile() Then
                Dim strOld As String() = {"%s"}
                Dim strNew As String() = {""}

                If udtServiceProviderBLL.Exist() Then
                    udtAuditLogEntry.AddDescripton("ERN", udtServiceProviderBLL.GetSP.EnrolRefNo)
                    udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
                    udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00040, "Proceed Vetting:")

                    If udtServiceProviderBLL.GetSP.SPID.Equals(String.Empty) Then
                        strNew(0) = udtServiceProviderBLL.GetSP.EnglishName + udtFormatter.formatChineseName(udtServiceProviderBLL.GetSP.ChineseName) + " [" + _
                                    udtFormatter.formatSystemNumber(udtServiceProviderBLL.GetSP.EnrolRefNo) + "] "
                    Else
                        strNew(0) = udtServiceProviderBLL.GetSP.EnglishName + udtFormatter.formatChineseName(udtServiceProviderBLL.GetSP.ChineseName) + " [" + _
                                    udtServiceProviderBLL.GetSP.SPID + "] "
                    End If
                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00041, "Proceed Vetting Completed.")
                End If


                CompleteMsgBox.AddMessage("990000", "I", "00004", strOld, strNew)
                CompleteMsgBox.BuildMessageBox()
                CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                MultiViewDataEntry.ActiveViewIndex = 1

            End If

        Catch eSQL As SqlClient.SqlException
            If eSQL.Number = 50000 Then
                Dim strmsg As String
                strmsg = eSQL.Message

                SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                msgBox.AddMessage(SM)
                If msgBox.GetCodeTable.Rows.Count = 0 Then
                    msgBox.Visible = False
                Else
                    msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00042, "Proceed Vetting Fail.")
                End If
            Else
                Throw
            End If

        Catch ex As Exception
            Throw
        End Try        
    End Sub
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

    Protected Sub ibtnReject_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry.WriteLog(LogID.LOG00109, "Abort Click")

        Me.ModalPopupExtenderConfirmDelete.Show()

    End Sub

    '

    Protected Sub ibtnDialogConfirm_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        msgBox.Visible = False
        'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]

        'Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me) ''Begin Writing Audit Log

        ' CRE11-021 log the missed essential information [Start]
        ' -----------------------------------------------------------------------------------------
        udtAuditLogEntry.WriteLog(LogID.LOG00110, "Abort Confirm Click")
        ' CRE11-021 log the missed essential information [End]

        udtAuditLogEntry.AddDescripton("ERN", Me.hfERN.Value.Trim)
        If udtServiceProviderBLL.Exist Then
            udtAuditLogEntry.AddDescripton("SPID", udtServiceProviderBLL.GetSP.SPID)
        Else
            udtAuditLogEntry.AddDescripton("SPID", "")
        End If
        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00046, "Abort")

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Dim strChineseName As String = String.Empty
        Dim strEnglishName As String = String.Empty
        Dim strERN As String = String.Empty

        Dim udtSP As ServiceProviderModel = Nothing

        Select Case hfTableLocation.Value.Trim
            Case TableLocation.Enrolment
                Try
                    If udtServiceProviderBLL.Exist Then
                        udtSP = udtServiceProviderBLL.GetSP
                        strChineseName = udtSP.ChineseName
                        strEnglishName = udtSP.EnglishName
                        strERN = udtSP.EnrolRefNo
                    End If

                    If udtSPProfileBLL.RejectSPProfilenNewEnrolment(udtSP, udtHCVUUser.UserID.Trim) Then
                        Dim strOld As String() = {"%s"}
                        Dim strNew As String() = {""}


                        strNew(0) = strEnglishName + udtFormatter.formatChineseName(strChineseName) + " [" + _
                                    udtFormatter.formatSystemNumber(strERN) + "] "

                        CompleteMsgBox.AddMessage("990000", "I", "00009", strOld, strNew)
                        CompleteMsgBox.BuildMessageBox()
                        CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                        MultiViewDataEntry.ActiveViewIndex = 1
                        Me.hfTableLocation.Value = String.Empty
                        Me.hfERN.Value = String.Empty

                        udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00047, "Abort Completed.")

                    End If

                Catch ex As Exception
                    Throw ex
                End Try
            Case TableLocation.Staging
                Try
                    Dim udtSPVerificationBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL
                    Dim udtSPVerification As ServiceProviderVerificationModel = Nothing

                    If udtServiceProviderBLL.Exist Then
                        udtSP = udtServiceProviderBLL.GetSP
                        strChineseName = udtSP.ChineseName
                        strEnglishName = udtSP.EnglishName
                        strERN = udtSP.EnrolRefNo
                    End If

                    If udtSPVerificationBLL.Exist Then
                        udtSPVerification = udtSPVerificationBLL.GetSPVerification
                        If udtSPProfileBLL.RejectSPProfileFromUserA(hfERN.Value.Trim, udtHCVUUser.UserID, udtSPVerification.TSMP) Then

                            Dim strOld As String() = {"%s"}
                            Dim strNew As String() = {""}


                            strNew(0) = strEnglishName + udtFormatter.formatChineseName(strChineseName) + " [" + _
                                        udtFormatter.formatSystemNumber(strERN) + "] "

                            CompleteMsgBox.AddMessage("990000", "I", "00009", strOld, strNew)
                            CompleteMsgBox.BuildMessageBox()
                            CompleteMsgBox.Type = CustomControls.InfoMessageBoxType.Complete
                            MultiViewDataEntry.ActiveViewIndex = 1

                            udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00047, "Abort Completed.")

                        End If
                    End If

                Catch eSQL As SqlClient.SqlException
                    If eSQL.Number = 50000 Then
                        Dim strmsg As String
                        strmsg = eSQL.Message

                        SM = New Common.ComObject.SystemMessage("990001", "D", strmsg)
                        msgBox.AddMessage(SM)
                        If msgBox.GetCodeTable.Rows.Count = 0 Then
                            msgBox.Visible = False
                        Else
                            'msgBox.BuildMessageBox("UpdateFail")
                            msgBox.BuildMessageBox("UpdateFail", udtAuditLogEntry, Common.Component.LogID.LOG00048, "Abort Fail.")
                        End If
                    Else
                        Throw eSQL
                    End If

                Catch ex As Exception
                    Throw ex
                End Try
        End Select
    End Sub

    Protected Sub ibtnDialogCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        udtAuditLogEntry.WriteLog(LogID.LOG00111, "Abort Cancel Click")

        Me.ModalPopupExtenderConfirmDelete.Hide()

    End Sub

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ''' <summary>
    ''' Check whether the PCD status is valid to the Join PCD question
    ''' </summary>
    ''' <param name="udtSP"></param>
    ''' <returns>blnInvalid: True(Invalid), False(Valid) </returns>
    ''' <remarks></remarks>
    Private Function ValidatePCDStatus(ByVal udtSP As ServiceProviderModel) As Boolean
        Dim blnJoinPCDCompulsory As Boolean = False
        Dim blnInvalid As Boolean = False
        Dim udtAuditLogEntry As New AuditLogEntry(strFuncCode, Me)

        blnJoinPCDCompulsory = SPProfileBLL.IsJoinPCDCompulsory(udtSP.SchemeInfoList)

        If blnJoinPCDCompulsory Then

            ' New Enrolment, Block enrolment if compulsory but not to join PCD
            If udtSP.SPID.Equals(String.Empty) AndAlso udtSP.JoinPCD = JoinPCDStatus.No Then
                blnInvalid = True
                msgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00413)
            End If
        End If

        If Not blnInvalid AndAlso blnJoinPCDCompulsory Then
            ' Check PCD Account Status
            Dim udtPCDWebService As PCDWebService = New PCDWebService(Me.FunctionCode)
            Dim udtPCDResult As WebService.Interface.PCDCheckAccountStatusResult = Nothing

            Try
                udtAuditLogEntry.AddDescripton("WebMethod", "PCDCheckAccountStatus")
                udtAuditLogEntry.WriteStartLog(Common.Component.LogID.LOG00117, "CheckPCDAccountStatus Start")

                udtPCDResult = udtPCDWebService.PCDCheckAccountStatus(udtSP.HKID)

                udtAuditLogEntry.AddDescripton("ReturnCode", udtPCDResult.ReturnCode.ToString)
                udtAuditLogEntry.AddDescripton("MessageID", udtPCDResult.MessageID.ToString)

                Session(SESS_PCDCheckAccountStatusResult) = udtPCDResult

                If udtPCDResult.ReturnCode = WebService.Interface.PCDCheckAccountStatusResult.enumReturnCode.Success Then

                    Select Case udtPCDResult.AccountStatusCode


                        Case PCDAccountStatus.NotEnrolled, PCDAccountStatus.Delisted
                            ' PCD Not Enrolled, Delisted also consider as not enrolled

                            If udtSP.JoinPCD = JoinPCDStatus.Enrolled Then
                                ' The service provider has not enrolled in PCD
                                msgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00414)
                                blnInvalid = True
                            End If

                        Case PCDAccountStatus.Enrolled
                            ' Already joined PCD
                            If udtSP.JoinPCD = JoinPCDStatus.Yes Then
                                ' The service provider has enrolled in PCD already
                                msgBox.AddMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00415)
                                blnInvalid = True
                            End If

                    End Select

                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00118, "CheckPCDAccountStatus Success")

                    ' Update Service Provider's Join PCD Status
                    If Not udtSP.SPID.Equals(String.Empty) Then
                        'Get VU User ID
                        Dim udtHCVUUser As HCVUUserModel
                        Dim udtHCVUUserBLL As New HCVUUserBLL
                        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

                        Dim strMessage As String = String.Empty
                        Dim blnRes As Boolean = udtPCDResult.UpdateJoinPCDStatus(udtSP.SPID, udtHCVUUser.UserID, strMessage)
                        If Not blnRes Then
                            Throw New Exception(strMessage)
                        End If
                    End If

                Else
                    ' Get PCD Account Status Fail
                    ' PCD service is temporarily unavailable. Please try again later.
                    ShowPCDConnectionFail()
                    blnInvalid = True

                    udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00119, "CheckPCDAccountStatus Fail")
                End If

            Catch ex As Exception
                udtAuditLogEntry.WriteEndLog(Common.Component.LogID.LOG00119, "CheckPCDAccountStatus Fail")

                blnInvalid = True
                Throw
            End Try
        End If

        Return blnInvalid
    End Function

    ''' <summary>
    ''' Check whether if any PCD warning exist, if yes, show popup, user need to confirm first to process next step
    ''' </summary>
    ''' <param name="udtSP"></param>
    ''' <returns>blnInvalid: True(Invalid), False(Valid) </returns>
    ''' <remarks></remarks>
    Private Function CheckPCDWarning(ByVal udtSP As ServiceProviderModel) As Boolean
        Dim blnJoinPCDCompulsory As Boolean = False
        Dim blnInvalid As Boolean = False
        Dim blnPCDProfessionalChecked As Boolean = False
        Dim blnPCDStatusChecked As Boolean = False

        ' PCD Warning Popup Confirm Click
        If Session(SESS_PCDProfessionalChecked) = YesNo.Yes Then
            blnPCDProfessionalChecked = True
        End If

        If Session(SESS_PCDStatusChecked) = YesNo.Yes Then
            blnPCDStatusChecked = True
        End If

        ' get PCD Account Status from session
        Dim udtPCDAccountStatusResult As PCDCheckAccountStatusResult = Session(SESS_PCDCheckAccountStatusResult)

        If udtPCDAccountStatusResult Is Nothing Then
            ' Bypass PCD checking for non PCD profession
            Session(SESS_PCDProfessionalChecked) = YesNo.Yes
            Session(SESS_PCDStatusChecked) = YesNo.Yes
            blnInvalid = False

            Return blnInvalid
        End If

        blnJoinPCDCompulsory = SPProfileBLL.IsJoinPCDCompulsory(udtSP.SchemeInfoList)

        ' Check PCD Professional, show popup if contain any warning message
        If blnJoinPCDCompulsory Then

            If Not blnInvalid AndAlso Not blnPCDProfessionalChecked Then
                Dim strMessage As String = udtPCDAccountStatusResult.CheckPCDProfessional()

                If strMessage <> String.Empty Then
                    blnInvalid = True
                    ShowPCDProfessionalWarning(strMessage)
                End If

                If Not blnInvalid Then
                    Session(SESS_PCDProfessionalChecked) = YesNo.Yes
                End If
            End If
        Else
            Session(SESS_PCDProfessionalChecked) = YesNo.Yes
        End If

        ' Check PCD Status
        If blnJoinPCDCompulsory Then

            If Not blnInvalid AndAlso Not blnPCDStatusChecked Then

                Select Case udtPCDAccountStatusResult.AccountStatusCode
                    Case PCDAccountStatus.NotEnrolled
                        ' For Scheme Enrolment only
                        If Not udtSP.SPID.Equals(String.Empty) Then
                            Select Case udtPCDAccountStatusResult.EnrolmentStatusCode

                                '  Show PCD not enrolled warning for No PCD enrolment / Unprocessed enrolment
                                Case PCDEnrolmentStatus.NA, PCDEnrolmentStatus.Unprocessed
                                    ShowPCDNotEnrolledWarning()
                                    blnInvalid = True
                            End Select
                        End If

                    Case PCDAccountStatus.Delisted
                        ' Show warning for SP is delisted in PCD and need to re-join PCD                        
                        ShowPCDDelistedWarning()
                        blnInvalid = True

                End Select

                If Not blnInvalid Then
                    Session(SESS_PCDStatusChecked) = YesNo.Yes
                End If
            End If
        Else
            Session(SESS_PCDStatusChecked) = YesNo.Yes
        End If

        Return blnInvalid
    End Function

    Private Sub ucPCDWarningPopup_Success_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ucPCDWarningPopup.Success_Click
        ' Confirm Click, continue process
        Select Case hfPCDWarningPopupType.Value
            Case ucPCDWarningPopup.WarningType.Professional
                Session(SESS_PCDProfessionalChecked) = YesNo.Yes

            Case ucPCDWarningPopup.WarningType.Delisted
                Session(SESS_PCDStatusChecked) = YesNo.Yes

            Case ucPCDWarningPopup.WarningType.NotEnrolled
                Session(SESS_PCDStatusChecked) = YesNo.Yes

        End Select

        ProceedToVetting()
    End Sub

    Private Sub ucPCDWarningPopup_Failure_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ucPCDWarningPopup.Failure_Click
        Me.ModelPopupExtenderPCDWarning.Show()
    End Sub

    Private Sub ShowPCDDelistedWarning()
        hfPCDWarningPopupType.Value = ucPCDWarningPopup.WarningType.Delisted

        Me.ucPCDWarningPopup.Build(ucPCDWarningPopup.WarningType.Delisted)
        Me.ModelPopupExtenderPCDWarning.PopupDragHandleControlID = Me.ucPCDWarningPopup.Header.ClientID
        Me.ModelPopupExtenderPCDWarning.Show()
    End Sub

    Private Sub ShowPCDNotEnrolledWarning()
        hfPCDWarningPopupType.Value = ucPCDWarningPopup.WarningType.NotEnrolled

        Me.ucPCDWarningPopup.Build(ucPCDWarningPopup.WarningType.NotEnrolled)
        Me.ModelPopupExtenderPCDWarning.PopupDragHandleControlID = Me.ucPCDWarningPopup.Header.ClientID
        Me.ModelPopupExtenderPCDWarning.Show()
    End Sub

    Private Sub ShowPCDProfessionalWarning(ByVal strWarningMessage As String)
        hfPCDWarningPopupType.Value = ucPCDWarningPopup.WarningType.Professional

        Me.ucPCDWarningPopup.MessageText = strWarningMessage
        Me.ucPCDWarningPopup.Build(ucPCDWarningPopup.WarningType.Professional)
        Me.ModelPopupExtenderPCDWarning.PopupDragHandleControlID = Me.ucPCDWarningPopup.Header.ClientID
        Me.ModelPopupExtenderPCDWarning.Show()
    End Sub

    Private Sub ShowPCDConnectionFail()
        Me.udtAuditLogEntry.WriteLog(LogID.LOG01136, "Show PCD Connection Fail Popup")

        Me.ucNoticePopup.NoticeMode = HCVU.ucNoticePopUp.enumNoticeMode.Custom
        Me.ucNoticePopup.ButtonMode = HCVU.ucNoticePopUp.enumButtonMode.OK
        Me.ucNoticePopup.IconMode = HCVU.ucNoticePopUp.enumIconMode.ExclamationIcon
        Me.ucNoticePopup.HeaderText = Me.GetGlobalResourceObject("Text", "Notification")
        Me.ucNoticePopup.MessageText = Me.GetGlobalResourceObject("Text", "PCDServiceUnavailable")
        Me.ModalPopupExtenderNotice.PopupDragHandleControlID = Me.ucNoticePopup.Header.ClientID
        Me.ModalPopupExtenderNotice.Show()
    End Sub
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

#Region "Supporting Functions used in ASPX"

    Protected Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String, ByVal strArea As String) As String
        Return udtFormatter.formatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrict, strArea)
    End Function

    Protected Function GetPracticeTypeName(ByVal strPracticeCode As String) As String
        Dim strPracticeTypeName As String

        If IsNothing(strPracticeCode) Then
            strPracticeTypeName = String.Empty
        Else
            If strPracticeCode.Equals(String.Empty) Then
                strPracticeTypeName = String.Empty
            Else
                If Session("language") = "zh-tw" Then
                    strPracticeTypeName = udtSPProfileBLL.GetPracticeTypeName(strPracticeCode).DataValueChi
                Else
                    strPracticeTypeName = udtSPProfileBLL.GetPracticeTypeName(strPracticeCode).DataValue
                End If
            End If
        End If
        Return strPracticeTypeName
    End Function

    Protected Function GetHealthProfName(ByVal strHealthProfCode As String) As String
        Dim strHealthProfName As String

        If IsNothing(strHealthProfCode) Then
            strHealthProfName = String.Empty
        Else
            If strHealthProfCode.Equals(String.Empty) Then
                strHealthProfName = String.Empty
            Else

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

                ' -----------------------------------------------------------------------------------------

                If Session("language") = "zh-tw" Then
                    strHealthProfName = udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDescChi
                Else
                    strHealthProfName = udtSPProfileBLL.GetHealthProfName(strHealthProfCode).ServiceCategoryDesc
                End If

                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            End If
        End If

        Return strHealthProfName
    End Function

    Protected Sub EnableFilteredEditBankOwner(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim chkEditIsFreeTextFormat As CheckBox = CType(sender, CheckBox)
        Dim FilteredEditBankOwner As AjaxControlToolkit.FilteredTextBoxExtender = FindControl("FilteredEditBankOwner")

        If chkEditIsFreeTextFormat.Checked Then
            FilteredEditBankOwner.Enabled = False
        Else
            FilteredEditBankOwner.Enabled = True
        End If

    End Sub

#End Region

#Region "Implement IWorkingData (CRE11-004)"

    Public Overrides Function GetDocCode() As String
        Return Nothing
    End Function

    Public Overrides Function GetEHSAccount() As Common.Component.EHSAccount.EHSAccountModel
        Return Nothing
    End Function

    Public Overrides Function GetEHSTransaction() As Common.Component.EHSTransaction.EHSTransactionModel
        Return Nothing
    End Function

    Public Overrides Function GetServiceProvider() As Common.Component.ServiceProvider.ServiceProviderModel
        Dim udtSP As Common.Component.ServiceProvider.ServiceProviderModel = Nothing

        Try
            udtSP = Me.udtServiceProviderBLL.GetSP
        Catch ex As Exception
            udtSP = Nothing
        End Try

        Return udtSP
    End Function

#End Region

End Class
