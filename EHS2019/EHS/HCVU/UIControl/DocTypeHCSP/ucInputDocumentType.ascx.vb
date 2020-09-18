Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.DocType
Imports Common.Component.Scheme
Imports Common.ComObject

Namespace UIControl.DocTypeHCSP

    Partial Public Class ucInputDocumentType
        Inherits System.Web.UI.UserControl

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private Class DocumentControlID
            Public Const HKID As String = "ucInputDocumentType_HKID"
            Public Const HKIDSmartID As String = "ucInputDocumentType_HKIDSmartID"
            Public Const HKIDSmartIDSignal As String = "ucInputDocumentType_HKIDSmartIDSignal"
            Public Const EC As String = "ucInputDocumentType_EC"
            Public Const HKBC As String = "ucInputDocumentType_HKBC"
            Public Const DI As String = "ucInputDocumentType_DI"
            Public Const REPMT As String = "ucInputDocumentType_REPMT"
            Public Const ID235B As String = "ucInputDocumentType_ID235B"
            Public Const VISA As String = "ucInputDocumentType_VISA"
            Public Const ADOPC As String = "ucInputDocumentType_ADOPC"
            Public Const OW As String = "ucInputDocumentType_OW"
            Public Const TW As String = "ucInputDocumentType_TW"
            Public Const RFNo8 As String = "ucInputDocumentType_RFNo8"
            Public Const OTHER As String = "ucInputDocumentType_OTHER"
        End Class
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        Public Event SelectChineseName_HKIC(ByVal udcInputHKID As ucInputHKID, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Public Event SelectGender(ByVal udcInputHKIC As ucInputDocTypeBase, ByVal sender As Object, ByVal e As System.EventArgs)
        Public Event SmartIDGenderTips(ByVal sender As Object, ByVal e As System.EventArgs)

        Private _docType As String
        Private _mode As ucInputDocTypeBase.BuildMode
        Private _fillValue As Boolean
        Private _udtEHSAccount As EHSAccountModel
        Private _udtSchemeClaim As SchemeClaimModel
        Private _textOnlyVersion As Boolean
        Private _activeViewChanged As Boolean
        Private _controlBuilded As Boolean
        'Private _udtSmartIDContent As BLL.SmartIDContentModel
        Private _udtAuditLogEntry As AuditLogEntry
        Private _blnFixEnglishNameGender As Boolean
        Private _udtOrgEHSAccount As EHSAccountModel
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Private _blnEditDocumentNo As Boolean = False
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        End Sub

#Region "Setup Function"

        Public Sub Built()
            'Me.Controls.Clear()

            Dim strFolderPath As String = String.Empty
            Dim udcInputDocumentType As ucInputDocTypeBase = Nothing

            strFolderPath = "~/UIControl/DocTypeHCSP/"

            Select Case Me._docType
                Case DocTypeModel.DocTypeCode.HKIC
                    udcInputDocumentType = Me.LoadControl(String.Format("{0}/ucInputHKID.ascx", strFolderPath))
                    udcInputDocumentType.ID = DocumentControlID.HKID
                    udcInputDocumentType.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList(0)
                    If Not Me._textOnlyVersion Then
                        AddHandler CType(udcInputDocumentType, ucInputHKID).SelectChineseName, AddressOf udcInputHKID_SelectChineseName
                    End If

                Case DocTypeModel.DocTypeCode.EC
                    udcInputDocumentType = Me.LoadControl(String.Format("{0}/ucInputEC.ascx", strFolderPath))
                    udcInputDocumentType.ID = DocumentControlID.EC
                    udcInputDocumentType.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.EC)

                Case DocTypeModel.DocTypeCode.HKBC

                    udcInputDocumentType = Me.LoadControl(String.Format("{0}/ucInputHKBC.ascx", strFolderPath))
                    udcInputDocumentType.ID = DocumentControlID.HKBC
                    udcInputDocumentType.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList(0)

                    ' CRE16-012 Removal of DOB InWord [Start][Winnie]
                    If Me._udtOrgEHSAccount Is Nothing Then
                        udcInputDocumentType.OrgEHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList(0)
                    Else
                        udcInputDocumentType.OrgEHSPersonalInfo = Me._udtOrgEHSAccount.EHSPersonalInformationList(0)
                    End If
                    ' CRE16-012 Removal of DOB InWord [End][Winnie]

                Case DocTypeModel.DocTypeCode.DI

                    udcInputDocumentType = Me.LoadControl(String.Format("{0}/ucInputDI.ascx", strFolderPath))
                    udcInputDocumentType.ID = DocumentControlID.DI
                    udcInputDocumentType.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.DI)


                Case DocTypeModel.DocTypeCode.REPMT

                    udcInputDocumentType = Me.LoadControl(String.Format("{0}/ucInputReentryPermit.ascx", strFolderPath))
                    udcInputDocumentType.ID = DocumentControlID.REPMT
                    udcInputDocumentType.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.REPMT)


                Case DocTypeModel.DocTypeCode.ID235B

                    udcInputDocumentType = Me.LoadControl(String.Format("{0}/ucInputID235B.ascx", strFolderPath))
                    udcInputDocumentType.ID = DocumentControlID.ID235B
                    udcInputDocumentType.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ID235B)


                Case DocTypeModel.DocTypeCode.VISA

                    udcInputDocumentType = Me.LoadControl(String.Format("{0}/ucInputVISA.ascx", strFolderPath))
                    udcInputDocumentType.ID = DocumentControlID.VISA
                    udcInputDocumentType.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.VISA)


                Case DocTypeModel.DocTypeCode.ADOPC

                    udcInputDocumentType = Me.LoadControl(String.Format("{0}/ucInputAdoption.ascx", strFolderPath))
                    udcInputDocumentType.ID = DocumentControlID.ADOPC
                    udcInputDocumentType.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ADOPC)

                    If Me._udtOrgEHSAccount Is Nothing Then
                        udcInputDocumentType.OrgEHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.ADOPC)
                    Else
                        udcInputDocumentType.OrgEHSPersonalInfo = Me._udtOrgEHSAccount.EHSPersonalInformationList(0)
                    End If

                Case DocTypeModel.DocTypeCode.OW

                    udcInputDocumentType = Me.LoadControl(String.Format("{0}/ucInputOW.ascx", strFolderPath))
                    udcInputDocumentType.ID = DocumentControlID.OW
                    udcInputDocumentType.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.OW)

                Case DocTypeModel.DocTypeCode.TW

                    udcInputDocumentType = Me.LoadControl(String.Format("{0}/ucInputTW.ascx", strFolderPath))
                    udcInputDocumentType.ID = DocumentControlID.TW
                    udcInputDocumentType.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.TW)

                Case DocTypeModel.DocTypeCode.RFNo8

                    udcInputDocumentType = Me.LoadControl(String.Format("{0}/ucInputRFNo8.ascx", strFolderPath))
                    udcInputDocumentType.ID = DocumentControlID.RFNo8
                    udcInputDocumentType.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.RFNo8)

                Case DocTypeModel.DocTypeCode.OTHER

                    udcInputDocumentType = Me.LoadControl(String.Format("{0}/ucInputOTHER.ascx", strFolderPath))
                    udcInputDocumentType.ID = DocumentControlID.OTHER
                    udcInputDocumentType.EHSPersonalInfo = Me._udtEHSAccount.EHSPersonalInformationList.Filter(DocTypeModel.DocTypeCode.OTHER)

                    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

            End Select

            udcInputDocumentType.UpdateValue = Me._fillValue
            udcInputDocumentType.Mode = Me._mode
            udcInputDocumentType.SchemeClaim = Me._udtSchemeClaim
            udcInputDocumentType.ActiveViewChanged = Me.ActiveViewChanged
            udcInputDocumentType.AuditLogEntry = Me._udtAuditLogEntry
            udcInputDocumentType.FixEnglishNameGender = Me._blnFixEnglishNameGender
            udcInputDocumentType.EditDocumentNo = Me.EditDocumentNo

            Me.Built(udcInputDocumentType)

        End Sub

        ''' <summary>
        ''' Clear All Control
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Clear()
            Me.phInputDocumentType.Controls.Clear()
        End Sub

        Public Sub Built(ByVal udcControl As Control)
            If Not Me._controlBuilded Then

                Me.phInputDocumentType.Controls.Add(udcControl)
                Me._controlBuilded = True

            End If
        End Sub

#End Region

#Region "Events"

        Private Sub udcInputHKID_SelectChineseName(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
            RaiseEvent SelectChineseName_HKIC(Me.GetHKICControl(), sender, e)
        End Sub

        Private Sub udcInputHKID_GenderTips(ByVal sender As Object, ByVal e As System.EventArgs)
            RaiseEvent SmartIDGenderTips(sender, e)
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

        Public Function GetOWControl() As ucInputOW
            Dim control As Control = Me.FindControl(DocumentControlID.OW)
            If Not control Is Nothing Then
                Return CType(control, ucInputOW)
            Else
                Return Nothing
            End If
        End Function

        Public Function GetTWControl() As ucInputTW
            Dim control As Control = Me.FindControl(DocumentControlID.TW)
            If Not control Is Nothing Then
                Return CType(control, ucInputTW)
            Else
                Return Nothing
            End If
        End Function

        Public Function GetRFNo8Control() As ucInputRFNo8
            Dim control As Control = Me.FindControl(DocumentControlID.RFNo8)
            If Not control Is Nothing Then
                Return CType(control, ucInputRFNo8)
            Else
                Return Nothing
            End If
        End Function

        Public Function GetOTHERControl() As ucInputOTHER
            Dim control As Control = Me.FindControl(DocumentControlID.OTHER)
            If Not control Is Nothing Then
                Return CType(control, ucInputOTHER)
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
                If Not value.Equals(Me._docType) Then
                    Me._controlBuilded = False
                End If
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

        ' CRE16-012 Removal of DOB InWord [Start][Winnie]
        Public Property OrgEHSAccount() As EHSAccountModel
            Get
                Return Me._udtOrgEHSAccount
            End Get
            Set(ByVal value As EHSAccountModel)
                Me._udtOrgEHSAccount = value
            End Set
        End Property
        ' CRE16-012 Removal of DOB InWord [End][Winnie]

        Public Property SchemeClaim() As SchemeClaimModel
            Get
                Return Me._udtSchemeClaim
            End Get
            Set(ByVal value As SchemeClaimModel)
                Me._udtSchemeClaim = value
            End Set
        End Property

        Public Property TextOnlyVersion() As Boolean
            Get
                Return Me._textOnlyVersion
            End Get
            Set(ByVal value As Boolean)
                Me._textOnlyVersion = value
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

        'Public Property SmartIDContent() As BLL.SmartIDContentModel
        '    Get
        '        Return Me._udtSmartIDContent
        '    End Get
        '    Set(ByVal value As BLL.SmartIDContentModel)
        '        Me._udtSmartIDContent = value
        '    End Set
        'End Property

        Public Property AuditLogEntry() As AuditLogEntry
            Get
                Return Me._udtAuditLogEntry
            End Get
            Set(ByVal value As AuditLogEntry)
                Me._udtAuditLogEntry = value
            End Set
        End Property

        Public Property FixEnglishNameGender() As Boolean
            Get
                Return _blnFixEnglishNameGender
            End Get
            Set(ByVal value As Boolean)
                _blnFixEnglishNameGender = value
            End Set
        End Property

        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Property EditDocumentNo() As Boolean
            Get
                Return _blnEditDocumentNo
            End Get
            Set(ByVal value As Boolean)
                _blnEditDocumentNo = value
            End Set
        End Property
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

#End Region

    End Class

End Namespace