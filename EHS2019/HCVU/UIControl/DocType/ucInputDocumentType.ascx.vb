Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.ComObject

Partial Public Class ucInputDocumentType
    Inherits System.Web.UI.UserControl

    Private Class DocumentControlID
        Public Const HKID As String = "ucInputDocumentType_HKID"
        Public Const EC As String = "ucInputDocumentType_EC"
        Public Const HKBC As String = "ucInputDocumentType_HKBC"
        Public Const DI As String = "ucInputDocumentType_DI"
        Public Const REPMT As String = "ucInputDocumentType_REPMT"
        Public Const ID235B As String = "ucInputDocumentType_ID235B"
        Public Const VISA As String = "ucInputDocumentType_VISA"
        Public Const ADOPC As String = "ucInputDocumentType_ADOPC"
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Const OW As String = "ucInputDocumentType_OW"
        Public Const TW As String = "ucInputDocumentType_TW"
        Public Const RFNo8 As String = "ucInputDocumentType_RFNo8"
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        Public Const OTHER As String = "ucInputDocumentType_OTHER"
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
    End Class

    Public Event SelectChineseName_HKIC(ByVal udcInputHKID As ucInputHKID, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event SelectChineseName_HKIC_mode(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal udcInputHKID As ucInputHKID, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Private _docType As String
    Private _mode As ucInputDocTypeBase.BuildMode
    Private _fillValue As Boolean
    Private _udtEHSAccountOriginal As EHSAccountModel
    Private _udtEHSAccountAmend As EHSAccountModel
    Private _useDefaultAmendingHeader As Boolean = False
    Private _activeViewChanged As Boolean
    Private _udtAuditLogEntry As AuditLogEntry

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Setup Function"

    Public Sub Built()
        'Me.Controls.Clear()

        Select Case Me._docType
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKID As ucInputHKID = Me.LoadControl("~/UIControl/DocType/ucInputHKID.ascx")
                udcInputHKID.ID = DocumentControlID.HKID

                If Not Me._mode = ucInputDocTypeBase.BuildMode.Creation Then
                    udcInputHKID.UpdateValue = Me._fillValue
                    udcInputHKID.EHSPersonalInfoOriginal = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)
                    If IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputHKID.EHSPersonalInfoAmend = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)
                    Else
                        udcInputHKID.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)
                    End If
                Else
                    If Not IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputHKID.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKIC)
                        'Special handling for CCC code (when returing to detail page from confirm page (Account Creation))
                        udcInputHKID.SetCnameAmend(udcInputHKID.EHSPersonalInfoAmend.CName)
                    End If
                End If
                udcInputHKID.Mode = Me._mode
                udcInputHKID.ActiveViewChanged = Me.ActiveViewChanged
                udcInputHKID.UseDefaultAmendingHeader = _useDefaultAmendingHeader
                AddHandler udcInputHKID.SelectChineseName, AddressOf udcInputHKID_SelectChineseName
                AddHandler udcInputHKID.SelectChineseName_CreateMode, AddressOf udcInputHKID_SelectChineseName_mode
                Me.Built(udcInputHKID)
            Case DocTypeModel.DocTypeCode.EC

                Dim udcInputEC As ucInputEC = Me.LoadControl("~/UIControl/DocType/ucInputEC.ascx")
                udcInputEC.ID = DocumentControlID.EC
                If Not Me._mode = ucInputDocTypeBase.BuildMode.Creation Then
                    udcInputEC.UpdateValue = Me._fillValue
                    udcInputEC.EHSPersonalInfoOriginal = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.EC)
                    If IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputEC.EHSPersonalInfoAmend = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.EC)
                    Else
                        udcInputEC.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.EC)
                    End If
                Else
                    If Not IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputEC.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.EC)
                    End If
                End If
                udcInputEC.Mode = Me._mode
                udcInputEC.ActiveViewChanged = Me.ActiveViewChanged
                udcInputEC.UseDefaultAmendingHeader = _useDefaultAmendingHeader
                udcInputEC.AuditLogEntry = _udtAuditLogEntry
                Me.Built(udcInputEC)

            Case DocTypeModel.DocTypeCode.HKBC

                Dim udcInputHKBC As ucInputHKBC = Me.LoadControl("~/UIControl/DocType/ucInputHKBC.ascx")
                udcInputHKBC.ID = DocumentControlID.HKBC
                If Not Me._mode = ucInputDocTypeBase.BuildMode.Creation Then
                    udcInputHKBC.UpdateValue = Me._fillValue
                    udcInputHKBC.EHSPersonalInfoOriginal = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKBC)
                    If IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputHKBC.EHSPersonalInfoAmend = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKBC)
                    Else
                        udcInputHKBC.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKBC)
                    End If
                Else
                    If Not IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputHKBC.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.HKBC)
                    End If
                End If
                udcInputHKBC.Mode = Me._mode
                udcInputHKBC.ActiveViewChanged = Me.ActiveViewChanged
                udcInputHKBC.UseDefaultAmendingHeader = _useDefaultAmendingHeader
                Me.Built(udcInputHKBC)

            Case DocTypeModel.DocTypeCode.DI

                Dim udcInputDI As ucInputDI = Me.LoadControl("~/UIControl/DocType/ucInputDI.ascx")
                udcInputDI.ID = DocumentControlID.DI
                If Not Me._mode = ucInputDocTypeBase.BuildMode.Creation Then
                    udcInputDI.Visible = Me._fillValue
                    udcInputDI.EHSPersonalInfoOriginal = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.DI)
                    If IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputDI.EHSPersonalInfoAmend = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.DI)
                    Else
                        udcInputDI.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.DI)
                    End If
                Else
                    If Not IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputDI.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.DI)
                    End If
                End If
                udcInputDI.Mode = Me._mode
                udcInputDI.ActiveViewChanged = Me.ActiveViewChanged
                udcInputDI.UseDefaultAmendingHeader = _useDefaultAmendingHeader
                Me.Built(udcInputDI)

            Case DocTypeModel.DocTypeCode.REPMT

                Dim udcInputREPMT As ucInputReentryPermit = Me.LoadControl("~/UIControl/DocType/ucInputReentryPermit.ascx")
                udcInputREPMT.ID = DocumentControlID.REPMT
                If Not Me._mode = ucInputDocTypeBase.BuildMode.Creation Then
                    udcInputREPMT.Visible = Me._fillValue
                    udcInputREPMT.EHSPersonalInfoOriginal = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.REPMT)
                    If IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputREPMT.EHSPersonalInfoAmend = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.REPMT)
                    Else
                        udcInputREPMT.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.REPMT)
                    End If
                Else
                    If Not IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputREPMT.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.REPMT)
                    End If
                End If
                udcInputREPMT.Mode = Me._mode
                udcInputREPMT.ActiveViewChanged = Me.ActiveViewChanged
                udcInputREPMT.UseDefaultAmendingHeader = _useDefaultAmendingHeader
                Me.Built(udcInputREPMT)

            Case DocTypeModel.DocTypeCode.ID235B

                Dim udcInputID235B As ucInputID235B = Me.LoadControl("~/UIControl/DocType/ucInputID235B.ascx")
                udcInputID235B.ID = DocumentControlID.ID235B
                If Not Me._mode = ucInputDocTypeBase.BuildMode.Creation Then
                    udcInputID235B.Visible = Me._fillValue
                    udcInputID235B.EHSPersonalInfoOriginal = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ID235B)
                    If IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputID235B.EHSPersonalInfoAmend = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ID235B)
                    Else
                        udcInputID235B.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ID235B)
                    End If
                Else
                    If Not IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputID235B.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ID235B)
                    End If
                End If
                udcInputID235B.Mode = Me._mode
                udcInputID235B.ActiveViewChanged = Me.ActiveViewChanged
                udcInputID235B.UseDefaultAmendingHeader = _useDefaultAmendingHeader
                Me.Built(udcInputID235B)

            Case DocTypeModel.DocTypeCode.VISA

                Dim udcInputVISA As ucInputVISA = Me.LoadControl("~/UIControl/DocType/ucInputVISA.ascx")
                udcInputVISA.ID = DocumentControlID.VISA
                If Not Me._mode = ucInputDocTypeBase.BuildMode.Creation Then
                    udcInputVISA.Visible = Me._fillValue
                    udcInputVISA.EHSPersonalInfoOriginal = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.VISA)
                    If IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputVISA.EHSPersonalInfoAmend = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.VISA)
                    Else
                        udcInputVISA.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.VISA)
                    End If
                Else
                    If Not IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputVISA.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.VISA)
                    End If
                End If
                udcInputVISA.Mode = Me._mode
                udcInputVISA.ActiveViewChanged = Me.ActiveViewChanged
                udcInputVISA.UseDefaultAmendingHeader = _useDefaultAmendingHeader
                Me.Built(udcInputVISA)

            Case DocTypeModel.DocTypeCode.ADOPC

                Dim udcInputADOPC As ucInputAdoption = Me.LoadControl("~/UIControl/DocType/ucInputAdoption.ascx")
                udcInputADOPC.ID = DocumentControlID.ADOPC
                If Not Me._mode = ucInputDocTypeBase.BuildMode.Creation Then
                    udcInputADOPC.Visible = Me._fillValue
                    udcInputADOPC.EHSPersonalInfoOriginal = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ADOPC)
                    If IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputADOPC.EHSPersonalInfoAmend = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ADOPC)
                    Else
                        udcInputADOPC.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ADOPC)
                    End If
                Else
                    If Not IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputADOPC.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ADOPC)
                    End If
                End If
                udcInputADOPC.Mode = Me._mode
                udcInputADOPC.ActiveViewChanged = Me.ActiveViewChanged
                udcInputADOPC.UseDefaultAmendingHeader = _useDefaultAmendingHeader
                Me.Built(udcInputADOPC)

                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
            Case DocTypeModel.DocTypeCode.OW,
                 DocTypeModel.DocTypeCode.TW,
                 DocTypeModel.DocTypeCode.RFNo8

                Dim udcInputOW As ucInputOW = Me.LoadControl("~/UIControl/DocType/ucInputOW.ascx")
                udcInputOW.ID = DocumentControlID.OW
                If Not Me._mode = ucInputDocTypeBase.BuildMode.Creation Then
                    udcInputOW.UpdateValue = Me._fillValue
                    udcInputOW.EHSPersonalInfoOriginal = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(Me._docType)
                    If IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputOW.EHSPersonalInfoAmend = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(Me._docType)
                    Else
                        udcInputOW.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(Me._docType)
                    End If
                Else
                    If Not IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputOW.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(Me._docType)
                    End If
                End If
                udcInputOW.Mode = Me._mode
                udcInputOW.ActiveViewChanged = Me.ActiveViewChanged
                udcInputOW.UseDefaultAmendingHeader = _useDefaultAmendingHeader
                Me.Built(udcInputOW)

                'Case DocTypeModel.DocTypeCode.TW
                '    Dim udcInputTW As ucInputTW = Me.LoadControl("~/UIControl/DocType/ucInputTW.ascx")
                '    udcInputTW.ID = DocumentControlID.TW
                '    If Not Me._mode = ucInputDocTypeBase.BuildMode.Creation Then
                '        udcInputTW.UpdateValue = Me._fillValue
                '        udcInputTW.EHSPersonalInfoOriginal = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.TW)
                '        If IsNothing(Me._udtEHSAccountAmend) Then
                '            udcInputTW.EHSPersonalInfoAmend = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.TW)
                '        Else
                '            udcInputTW.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.TW)
                '        End If
                '    Else
                '        If Not IsNothing(Me._udtEHSAccountAmend) Then
                '            udcInputTW.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.TW)
                '        End If
                '    End If
                '    udcInputTW.Mode = Me._mode
                '    udcInputTW.ActiveViewChanged = Me.ActiveViewChanged
                '    udcInputTW.UseDefaultAmendingHeader = _useDefaultAmendingHeader
                '    Me.Built(udcInputTW)

                'Case DocTypeModel.DocTypeCode.RFNo8
                '    Dim udcInputRFNo8 As ucInputRFNo8 = Me.LoadControl("~/UIControl/DocType/ucInputRFNo8.ascx")
                '    udcInputRFNo8.ID = DocumentControlID.RFNo8
                '    If Not Me._mode = ucInputDocTypeBase.BuildMode.Creation Then
                '        udcInputRFNo8.UpdateValue = Me._fillValue
                '        udcInputRFNo8.EHSPersonalInfoOriginal = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.RFNo8)
                '        If IsNothing(Me._udtEHSAccountAmend) Then
                '            udcInputRFNo8.EHSPersonalInfoAmend = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.RFNo8)
                '        Else
                '            udcInputRFNo8.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.RFNo8)
                '        End If
                '    Else
                '        If Not IsNothing(Me._udtEHSAccountAmend) Then
                '            udcInputRFNo8.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.RFNo8)
                '        End If
                '    End If
                '    udcInputRFNo8.Mode = Me._mode
                '    udcInputRFNo8.ActiveViewChanged = Me.ActiveViewChanged
                '    udcInputRFNo8.UseDefaultAmendingHeader = _useDefaultAmendingHeader
                '    Me.Built(udcInputRFNo8)

                ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

                ' CRE19-001 (VSS 2019) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
            Case DocTypeModel.DocTypeCode.OC,
                 DocTypeModel.DocTypeCode.IR,
                 DocTypeModel.DocTypeCode.HKP,
                 DocTypeModel.DocTypeCode.OTHER
                ' CRE19-001 (VSS 2019) [End][Winnie]

                Dim udcInputOTHER As ucInputOTHER = Me.LoadControl("~/UIControl/DocType/ucInputOTHER.ascx")
                udcInputOTHER.ID = DocumentControlID.OTHER
                If Not Me._mode = ucInputDocTypeBase.BuildMode.Creation Then
                    udcInputOTHER.UpdateValue = Me._fillValue
                    udcInputOTHER.EHSPersonalInfoOriginal = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(Me._docType)
                    If IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputOTHER.EHSPersonalInfoAmend = Me._udtEHSAccountOriginal.EHSPersonalInformationList.Filter(Me._docType)
                    Else
                        udcInputOTHER.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(Me._docType)
                    End If
                Else
                    If Not IsNothing(Me._udtEHSAccountAmend) Then
                        udcInputOTHER.EHSPersonalInfoAmend = Me._udtEHSAccountAmend.EHSPersonalInformationList.Filter(Me._docType)
                    End If
                End If
                udcInputOTHER.Mode = Me._mode
                udcInputOTHER.ActiveViewChanged = Me.ActiveViewChanged
                udcInputOTHER.UseDefaultAmendingHeader = _useDefaultAmendingHeader
                Me.Built(udcInputOTHER)

        End Select

    End Sub

    ''' <summary>
    ''' Clear All Control
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Clear()
        Me.phInputDocumentType.Controls.Clear()
    End Sub

    Public Sub Built(ByVal udcControl As Control)
        Me.phInputDocumentType.Controls.Add(udcControl)
    End Sub

#End Region

#Region "Events"

    Private Sub udcInputHKID_SelectChineseName(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent SelectChineseName_HKIC(Me.GetHKICControl(), sender, e)
    End Sub

    Private Sub udcInputHKID_SelectChineseName_mode(ByVal mode As ucInputDocTypeBase.BuildMode, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent SelectChineseName_HKIC_mode(mode, Me.GetHKICControl(), sender, e)
    End Sub
#End Region

#Region "Get Function"

    Public Function GetHKICControl() As ucInputHKID
        Dim control As Control = Me.FindControl(DocumentControlID.HKID)
        If Not control Is Nothing Then
            Return CType(control, ucInputHKID)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetECControl() As ucInputEC
        Dim control As Control = Me.FindControl(DocumentControlID.EC)
        If Not control Is Nothing Then
            Return CType(control, ucInputEC)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetHKBCControl() As ucInputHKBC
        Dim control As Control = Me.FindControl(DocumentControlID.HKBC)
        If Not control Is Nothing Then
            Return CType(control, ucInputHKBC)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetDIControl() As ucInputDI
        Dim control As Control = Me.FindControl(DocumentControlID.DI)
        If Not control Is Nothing Then
            Return CType(control, ucInputDI)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetREPMTControl() As ucInputReentryPermit
        Dim control As Control = Me.FindControl(DocumentControlID.REPMT)
        If Not control Is Nothing Then
            Return CType(control, ucInputReentryPermit)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetID235BControl() As ucInputID235B
        Dim control As Control = Me.FindControl(DocumentControlID.ID235B)
        If Not control Is Nothing Then
            Return CType(control, ucInputID235B)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetVISAControl() As ucInputVISA
        Dim control As Control = Me.FindControl(DocumentControlID.VISA)
        If Not control Is Nothing Then
            Return CType(control, ucInputVISA)
        Else
            Return Nothing
        End If
    End Function

    Public Function GetADOPCControl() As ucInputAdoption
        Dim control As Control = Me.FindControl(DocumentControlID.ADOPC)
        If Not control Is Nothing Then
            Return CType(control, ucInputAdoption)
        Else
            Return Nothing
        End If
    End Function

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Function GetOWControl() As ucInputOW
        Dim control As Control = Me.FindControl(DocumentControlID.OW)
        If Not control Is Nothing Then
            Return CType(control, ucInputOW)
        Else
            Return Nothing
        End If
    End Function
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
    Public Function GetOTHERControl() As ucInputOTHER
        Dim control As Control = Me.FindControl(DocumentControlID.OTHER)
        If Not control Is Nothing Then
            Return CType(control, ucInputOTHER)
        Else
            Return Nothing
        End If
    End Function
    ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
#End Region

#Region "Property"

    Public Property DocType() As String
        Get
            Return Me._docType
        End Get
        Set(ByVal value As String)
            Me._docType = value
        End Set
    End Property

    Public Property Mode() As ucInputDocTypeBase.BuildMode
        Get
            Return Me._mode
        End Get
        Set(ByVal value As ucInputDocTypeBase.BuildMode)
            Me._mode = value
        End Set
    End Property

    Public Property FillValue() As Boolean
        Get
            Return Me._fillValue
        End Get
        Set(ByVal value As Boolean)
            Me._fillValue = value
        End Set
    End Property

    Public Property EHSAccountOriginal() As EHSAccountModel
        Get
            Return Me._udtEHSAccountOriginal
        End Get
        Set(ByVal value As EHSAccountModel)
            Me._udtEHSAccountOriginal = value
        End Set
    End Property

    Public Property EHSAccountAmend() As EHSAccountModel
        Get
            Return Me._udtEHSAccountAmend
        End Get
        Set(ByVal value As EHSAccountModel)
            Me._udtEHSAccountAmend = value
        End Set
    End Property

    Public Property UseDefaultAmendingHeader() As Boolean
        Get
            Return Me._useDefaultAmendingHeader
        End Get
        Set(ByVal value As Boolean)
            _useDefaultAmendingHeader = value
        End Set
    End Property

    Public Property ActiveViewChanged() As Boolean
        Get
            If String.IsNullOrEmpty(Me.Attributes("ActiveViewChanged")) Then
                Return True
            Else
                Return CType(Me.Attributes("ActiveViewChanged"), Boolean)
            End If
        End Get

        Set(ByVal value As Boolean)
            Me.Attributes("ActiveViewChanged") = value
            Me._activeViewChanged = CType(Me.Attributes("ActiveViewChanged"), Boolean)
        End Set
    End Property

    Public Property AuditLogEntry() As AuditLogEntry
        Get
            Return Me._udtAuditLogEntry
        End Get
        Set(ByVal value As AuditLogEntry)
            Me._udtAuditLogEntry = value
        End Set
    End Property

#End Region

End Class