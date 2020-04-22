Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType

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
    End Class

    Public Event SelectChineseName_HKIC(ByVal udcInputHKID As ucInputHKID, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Private _docType As String
    Private _mode As ucInputDocTypeBase.BuildMode
    Private _fillValue As Boolean
    Private _udtEHSAccount As EHSAccountModel

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#Region "Setup Function"

    Public Sub Built()
        'Me.Controls.Clear()

        Select Case Me._docType
            Case DocTypeModel.DocTypeCode.HKIC
                Dim udcInputHKID As ucInputHKID = Me.LoadControl("~/UIControl/DocType/ucInputHKID.ascx")
                udcInputHKID.ID = DocumentControlID.HKID
                udcInputHKID.UpdateValue = Me._fillValue
                If Not Me._udtEHSAccount Is Nothing Then
                    udcInputHKID.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList(0)
                End If
                udcInputHKID.Mode = Me._mode
                AddHandler udcInputHKID.SelectChineseName, AddressOf udcInputHKID_SelectChineseName
                Me.Built(udcInputHKID)
            Case DocTypeModel.DocTypeCode.EC

                Dim udcInputEC As ucInputEC = Me.LoadControl("~/UIControl/DocType/ucInputEC.ascx")
                udcInputEC.ID = DocumentControlID.EC
                udcInputEC.UpdateValue = Me._fillValue
                'udcInputEC.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.EC)
                udcInputEC.Mode = Me._mode
                Me.Built(udcInputEC)

            Case DocTypeModel.DocTypeCode.HKBC

                Dim udcInputHKBC As ucInputHKBC = Me.LoadControl("~/UIControl/DocType/ucInputHKBC.ascx")
                udcInputHKBC.ID = DocumentControlID.HKBC
                udcInputHKBC.UpdateValue = Me._fillValue
                If Not Me._udtEHSAccount Is Nothing Then
                    udcInputHKBC.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList(0)
                End If
                udcInputHKBC.Mode = Me._mode
                Me.Built(udcInputHKBC)

            Case DocTypeModel.DocTypeCode.DI

                Dim udcInputDI As ucInputDI = Me.LoadControl("~/UIControl/DocType/ucInputDI.ascx")
                udcInputDI.ID = DocumentControlID.DI
                udcInputDI.Visible = Me._fillValue
                If Not Me._udtEHSAccount Is Nothing Then
                    udcInputDI.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.DI)
                End If
                udcInputDI.Mode = Me._mode
                Me.Built(udcInputDI)

            Case DocTypeModel.DocTypeCode.REPMT

                Dim udcInputREPMT As ucInputReentryPermit = Me.LoadControl("~/UIControl/DocType/ucInputReentryPermit.ascx")
                udcInputREPMT.ID = DocumentControlID.REPMT
                udcInputREPMT.Visible = Me._fillValue
                If Not Me._udtEHSAccount Is Nothing Then
                    udcInputREPMT.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.REPMT)
                End If
                udcInputREPMT.Mode = Me._mode
                Me.Built(udcInputREPMT)

            Case DocTypeModel.DocTypeCode.ID235B

                Dim udcInputID235B As ucInputID235B = Me.LoadControl("~/UIControl/DocType/ucInputID235B.ascx")
                udcInputID235B.ID = DocumentControlID.ID235B
                udcInputID235B.Visible = Me._fillValue
                If Not Me._udtEHSAccount Is Nothing Then
                    udcInputID235B.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ID235B)
                End If
                udcInputID235B.Mode = Me._mode
                Me.Built(udcInputID235B)

            Case DocTypeModel.DocTypeCode.VISA

                Dim udcInputVISA As ucInputVISA = Me.LoadControl("~/UIControl/DocType/ucInputVISA.ascx")
                udcInputVISA.ID = DocumentControlID.VISA
                udcInputVISA.Visible = Me._fillValue
                If Not Me._udtEHSAccount Is Nothing Then
                    udcInputVISA.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.VISA)
                End If
                udcInputVISA.Mode = Me._mode
                Me.Built(udcInputVISA)

            Case DocTypeModel.DocTypeCode.ADOPC

                Dim udcInputADOPC As ucInputAdoption = Me.LoadControl("~/UIControl/DocType/ucInputAdoption.ascx")
                udcInputADOPC.ID = DocumentControlID.ADOPC
                udcInputADOPC.Visible = Me._fillValue
                If Not Me._udtEHSAccount Is Nothing Then
                    udcInputADOPC.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ADOPC)
                End If
                udcInputADOPC.Mode = Me._mode
                Me.Built(udcInputADOPC)
        End Select

    End Sub

    ''' <summary>
    ''' Clear All Control
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Clear()
        Me.phInputDocumentType.Controls.Clear()
    End Sub

    Public Sub ClearControl()
        Me.Controls.Clear()
        Me.Controls.Add(phInputDocumentType)
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

    Public Property EHSAccount() As EHSAccountModel
        Get
            Return Me._udtEHSAccount
        End Get
        Set(ByVal value As EHSAccountModel)
            Me._udtEHSAccount = value
        End Set
    End Property

#End Region

End Class