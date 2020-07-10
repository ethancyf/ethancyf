Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.HCVUUser
Imports Common.Component.RedirectParameter
Imports Common.Format
Imports CustomControls
Imports HCVU.BLL


Partial Public Class ucReadOnlyDocumnetType
    Inherits System.Web.UI.UserControl

#Region "Private Members"

    Public Const DEFAULT_WIDTH As Integer = 200
    Public Const DEFAULT_WIDTH2 As Integer = 250
    Private Const strSessionCollectionKey As String = "__ascx_ReadOnlyDocumnetType_Session"

    Private _blnMaskIdentityNo As Boolean = False
    Private _blnVertical As Boolean = False
    Private _blnShowAccID As Boolean = False
    Private _blnShowAccIDAsBtn As Boolean = True ' CRE17-006 Default Show Account ID as Button
    Private _blnShowAccStatus As Boolean = False
    Private _intWidth As Integer = DEFAULT_WIDTH
    Private _intWidth2 As Integer = DEFAULT_WIDTH2
    Private _enumDisplayFormat As EnumDisplayFormat = EnumDisplayFormat.EnquiryFormat
    Private _strDocumentType As String = String.Empty
    Private _udtEHSPersonalInformation As EHSPersonalInformationModel = Nothing
    Private _udtEHSAccountModel As EHSAccount.EHSAccountModel = Nothing
    Private _blnIsInvalidAccount As Boolean = False
    Private _strOriginalAccID As String = String.Empty
    Private _strOriginalAccType As String = String.Empty
    Private _blnShowDateOfDeath As Boolean = True ' CRE11-007
    Private _blnShowDateOfDeathBtn As Boolean = True ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
    Private _blnShowAccountTypeAndStatus As Boolean = False
    Private _blnEnableToShowHKICSymbol As Boolean = False
    Private _blnShowCreationMethod As Boolean = True ' CRE19-026 (HCVS hotline service)

#End Region

#Region "Constants"

    Private Class UserControlPath
        Public Const HKIC As String = "ucReadOnlyHKIC.ascx"
        Public Const EC As String = "ucReadOnlyEC.ascx"
        Public Const DI As String = "ucReadOnlyDI.ascx"
        Public Const HKBC As String = "ucReadOnlyHKBC.ascx"
        Public Const REPMT As String = "ucReadOnlyREPMT.ascx"
        Public Const ID235B As String = "ucReadOnlyID235B.ascx"
        Public Const VISA As String = "ucReadOnlyVISA.ascx"
        Public Const ADOPC As String = "ucReadOnlyADOPC.ascx"
        Public Const Common As String = "ucReadOnlyCommon.ascx" ' CRE11-007
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Const OW As String = "ucReadOnlyOW.ascx"
        Public Const TW As String = "ucReadOnlyTW.ascx"
        Public Const RFNo8 As String = "ucReadOnlyRFNo8.ascx"
        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        Public Const OTHER As String = "ucReadOnlyOTHER.ascx" ' 
        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

    End Class

    Public Enum EnumDisplayFormat
        InputFormat
        EnquiryFormat
    End Enum
#End Region

    Public Sub Build(Optional ByVal blnAddToOriginalAccPlaceHolder As Boolean = False)

        Me.ShowAccIDWithAccStatus()

        'alternate row in table for displaying document
        Dim blnAlternateRow As Boolean = False

        If _blnIsInvalidAccount Then
            lblInvalidAccount.Visible = True
            ibtnShowOriginalAcc.Visible = True
            tblInvalidAcc.Visible = True

            lblInvalidAccount.Text = GetGlobalResourceObject("Text", "NotApplicable")

            Dim udtFormatter As Formatter = New Formatter

            ' INT20-0014 (Fix unable to open invalidated PPP transaction) [Start][Winnie]
            ' ---------------------------------------------------------------------------
            Select Case _strOriginalAccType.Trim
                Case EHSAccount.EHSAccountModel.OriginalAccTypeClass.ValidateAccount
                    lblOriginalAccIDText.Text = Me.GetGlobalResourceObject("Text", "AccountID")
                    lblOriginalAccID.Text = udtFormatter.formatValidatedEHSAccountNumber(_strOriginalAccID)

                Case EHSAccount.EHSAccountModel.OriginalAccTypeClass.TemporaryAccount,
                    EHSAccount.EHSAccountModel.OriginalAccTypeClass.SpecialAccount
                    lblOriginalAccIDText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
                    lblOriginalAccID.Text = udtFormatter.formatSystemNumber(_strOriginalAccID)
            End Select

            'If _strOriginalAccType.Trim = EHSAccount.EHSAccountModel.OriginalAccTypeClass.SpecialAccount Then
            '    lblOriginalAccIDText.Text = Me.GetGlobalResourceObject("Text", "RefNo")
            '    lblOriginalAccID.Text = udtFormatter.formatSystemNumber(_strOriginalAccID)
            'ElseIf _strOriginalAccType.Trim = EHSAccount.EHSAccountModel.OriginalAccTypeClass.ValidateAccount Then
            '    lblOriginalAccIDText.Text = Me.GetGlobalResourceObject("Text", "AccountID")
            '    lblOriginalAccID.Text = udtFormatter.formatValidatedEHSAccountNumber(_strOriginalAccID)
            'End If
            ' INT20-0014 (Fix unable to open invalidated PPP transaction) [End][Winnie]

            tblInvalidAcc.Rows(0).Cells(0).Width = _intWidth
            tblInvalidAcc.Rows(0).Cells(1).Width = _intWidth2

            'Stores all properties in session (only one session, hastable)
            setSessionValue("_blnIsInvalidAccount", _blnIsInvalidAccount)
            setSessionValue("_strDocumentType", _strDocumentType)
            setSessionValue("_udtEHSPersonalInformation", _udtEHSPersonalInformation)
            setSessionValue("_blnMaskIdentityNo", _blnMaskIdentityNo)
            setSessionValue("_blnVertical", _blnVertical)
            setSessionValue("_intWidth", _intWidth)
            setSessionValue("_intWidth2", _intWidth2)
            setSessionValue("_strOriginalAccID", _strOriginalAccID)
            setSessionValue("_strOriginalAccType", _strOriginalAccType)

            blnAlternateRow = True
        Else
            lblInvalidAccount.Visible = False
            ibtnShowOriginalAcc.Visible = False
            tblInvalidAcc.Visible = False
        End If

        Select Case Me._enumDisplayFormat
            Case EnumDisplayFormat.InputFormat

                Select Case _strDocumentType
                    Case DocTypeCode.HKIC
                        ' Hong Kong Identity Card
                        Dim udcReadOnlyHKIC As ucReadOnlyHKIC = LoadControl(UserControlPath.HKIC)
                        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                        udcReadOnlyHKIC.Build(_udtEHSPersonalInformation, _blnMaskIdentityNo, _blnVertical, _intWidth, _intWidth2, ShowDateOfDeath, _blnShowDateOfDeathBtn, _blnShowCreationMethod, blnAlternateRow)
                        ' CRE19-026 (HCVS hotline service) [End][Winnie]

                        If _blnIsInvalidAccount Then
                            If blnAddToOriginalAccPlaceHolder Then
                                SetOriginal(udcReadOnlyHKIC)
                            End If
                        Else
                            SetNormal(udcReadOnlyHKIC)
                        End If

                    Case DocTypeCode.EC
                        ' Certificate of Exemption
                        Dim udcReadOnlyEC As ucReadOnlyEC = LoadControl(UserControlPath.EC)
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                        udcReadOnlyEC.Build(_udtEHSPersonalInformation, _blnMaskIdentityNo, _blnVertical, _intWidth, _intWidth2, ShowDateOfDeath, _blnShowDateOfDeathBtn, blnAlternateRow)
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
                        If _blnIsInvalidAccount Then
                            If blnAddToOriginalAccPlaceHolder Then
                                SetOriginal(udcReadOnlyEC)
                            End If
                        Else
                            SetNormal(udcReadOnlyEC)
                        End If

                    Case DocTypeCode.HKBC
                        ' Birth Certificate
                        Dim udcReadOnlyHKBC As ucReadOnlyHKBC = LoadControl(UserControlPath.HKBC)
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
                        udcReadOnlyHKBC.Build(_udtEHSPersonalInformation, _blnMaskIdentityNo, _blnVertical, _intWidth, _intWidth2, ShowDateOfDeath, _blnShowDateOfDeathBtn, blnAlternateRow)
                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]
                        If _blnIsInvalidAccount Then
                            If blnAddToOriginalAccPlaceHolder Then
                                SetOriginal(udcReadOnlyHKBC)
                            End If
                        Else
                            SetNormal(udcReadOnlyHKBC)
                        End If

                    Case DocTypeCode.DI
                        ' Document of Identity
                        Dim udcReadOnlyDI As ucReadOnlyDI = LoadControl(UserControlPath.DI)
                        udcReadOnlyDI.Build(_udtEHSPersonalInformation, _blnMaskIdentityNo, _blnVertical, _intWidth, _intWidth2, blnAlternateRow)
                        If _blnIsInvalidAccount Then
                            If blnAddToOriginalAccPlaceHolder Then
                                SetOriginal(udcReadOnlyDI)
                            End If
                        Else
                            SetNormal(udcReadOnlyDI)
                        End If

                    Case DocTypeCode.REPMT
                        ' Hong Kong Re-entry Permit
                        Dim udcReadOnlyREPMT As ucReadOnlyREPMT = LoadControl(UserControlPath.REPMT)
                        udcReadOnlyREPMT.Build(_udtEHSPersonalInformation, _blnMaskIdentityNo, _blnVertical, _intWidth, _intWidth2, blnAlternateRow)
                        If _blnIsInvalidAccount Then
                            If blnAddToOriginalAccPlaceHolder Then
                                SetOriginal(udcReadOnlyREPMT)
                            End If
                        Else
                            SetNormal(udcReadOnlyREPMT)
                        End If

                    Case DocTypeCode.ID235B
                        ' Permit to Remain in HKSAR (ID 235B)
                        Dim udcReadOnlyID235B As ucReadOnlyID235B = LoadControl(UserControlPath.ID235B)
                        udcReadOnlyID235B.Build(_udtEHSPersonalInformation, _blnMaskIdentityNo, _blnVertical, _intWidth, _intWidth2, blnAlternateRow)
                        If _blnIsInvalidAccount Then
                            If blnAddToOriginalAccPlaceHolder Then
                                SetOriginal(udcReadOnlyID235B)
                            End If
                        Else
                            SetNormal(udcReadOnlyID235B)
                        End If

                    Case DocTypeCode.VISA
                        ' Non-Hong Kong Travel Documents
                        Dim udcReadOnlyVISA As ucReadOnlyVISA = LoadControl(UserControlPath.VISA)
                        udcReadOnlyVISA.Build(_udtEHSPersonalInformation, _blnMaskIdentityNo, _blnVertical, _intWidth, _intWidth2, blnAlternateRow)
                        If _blnIsInvalidAccount Then
                            If blnAddToOriginalAccPlaceHolder Then
                                SetOriginal(udcReadOnlyVISA)
                            End If
                        Else
                            SetNormal(udcReadOnlyVISA)
                        End If

                    Case DocTypeCode.ADOPC
                        ' Certificate issued by the Births and Deaths Registry for adopted children
                        Dim udcReadOnlyADOPC As ucReadOnlyADOPC = LoadControl(UserControlPath.ADOPC)
                        udcReadOnlyADOPC.Build(_udtEHSPersonalInformation, _blnMaskIdentityNo, _blnVertical, _intWidth, _intWidth2, blnAlternateRow)
                        If _blnIsInvalidAccount Then
                            If blnAddToOriginalAccPlaceHolder Then
                                SetOriginal(udcReadOnlyADOPC)
                            End If
                        Else
                            SetNormal(udcReadOnlyADOPC)
                        End If

                        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                    Case DocTypeCode.OW,
                         DocTypeCode.TW,
                         DocTypeCode.RFNo8

                        Dim udcReadOnlyOW As ucReadOnlyOW = LoadControl(UserControlPath.OW)
                        udcReadOnlyOW.Build(_udtEHSPersonalInformation, _blnMaskIdentityNo, _blnVertical, _intWidth, _intWidth2, blnAlternateRow)
                        If _blnIsInvalidAccount Then
                            If blnAddToOriginalAccPlaceHolder Then
                                SetOriginal(udcReadOnlyOW)
                            End If
                        Else
                            SetNormal(udcReadOnlyOW)
                        End If
                        ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

                        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]                        
                        ' CRE19-001 (VSS 2019) [Start][Winnie]
                    Case DocTypeCode.OC,
                        DocTypeCode.IR,
                        DocTypeCode.HKP,
                        DocTypeCode.OTHER
                        ' CRE19-001 (VSS 2019) [End][Winnie]

                        ' All new documents for student file upload
                        Dim udcReadOnlyOTHER As ucReadOnlyOTHER = LoadControl(UserControlPath.OTHER)
                        udcReadOnlyOTHER.Build(_udtEHSPersonalInformation, _blnMaskIdentityNo, _blnVertical, _intWidth, _intWidth2, ShowDateOfDeath, _blnShowDateOfDeathBtn, blnAlternateRow)
                        If _blnIsInvalidAccount Then
                            If blnAddToOriginalAccPlaceHolder Then
                                SetOriginal(udcReadOnlyOTHER)
                            End If
                        Else
                            SetNormal(udcReadOnlyOTHER)
                        End If
                        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                End Select
            Case EnumDisplayFormat.EnquiryFormat
                Dim udcReadOnlyCommon As ucReadOnlyCommon = LoadControl(UserControlPath.Common)

                udcReadOnlyCommon.ShowHKICSymbol = _blnEnableToShowHKICSymbol

                ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                udcReadOnlyCommon.Build(_udtEHSPersonalInformation, _blnMaskIdentityNo, _blnVertical, _intWidth, _intWidth2, ShowDateOfDeath, _blnShowDateOfDeathBtn, _blnShowCreationMethod, blnAlternateRow)
                ' CRE19-026 (HCVS hotline service) [End][Winnie]

                If _blnIsInvalidAccount Then
                    If blnAddToOriginalAccPlaceHolder Then
                        SetOriginal(udcReadOnlyCommon)
                    End If
                Else
                    SetNormal(udcReadOnlyCommon)
                End If
        End Select
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="control"></param>
    ''' <remarks></remarks>
    Private Sub SetOriginal(ByVal control As UI.Control)
        phDocumentType.Controls.Clear()
        phOriginalAccDocumentType.Controls.Clear()
        phOriginalAccDocumentType.Controls.Add(control)
    End Sub

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <param name="control"></param>
    ''' <remarks></remarks>
    Private Sub SetNormal(ByVal control As UI.Control)
        phDocumentType.Controls.Clear()
        phOriginalAccDocumentType.Controls.Clear()
        phDocumentType.Controls.Add(control)
    End Sub


    Public Sub Clear()
        phDocumentType.Controls.Clear()
        phOriginalAccDocumentType.Controls.Clear()
        lblInvalidAccount.Visible = False
        ibtnShowOriginalAcc.Visible = False
        Me.lblAccountStatus.Text = String.Empty
        ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Me.lblAccountStatusT.Text = String.Empty
        ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        ' Reset all property
        '_blnMaskIdentityNo = False
        '_blnVertical = False
        '_blnShowAccID = False
        '_blnShowAccStatus = False
        '_intWidth = DEFAULT_WIDTH
        '_intWidth2 = DEFAULT_WIDTH2
        '_enumDisplayFormat = EnumDisplayFormat.EnquiryFormat
        '_strDocumentType = String.Empty
        ''_udtEHSPersonalInformation = Nothing
        ''_udtEHSAccountModel = Nothing
        '_blnIsInvalidAccount = False
        '_strOriginalAccID = String.Empty
        '_strOriginalAccType = String.Empty
        '_blnShowDateOfDeath = True
        '_blnShowAccountTypeAndStatus = False

        'Clear Session
        Me.clearSession()
    End Sub

    Private Sub ShowAccIDWithAccStatus()

        If Not IsNothing(_udtEHSAccountModel) AndAlso Not IsNothing(_udtEHSAccountModel.EHSPersonalInformationList.Filter(_strDocumentType.Trim)) Then

            'Dim udtformatter As Common.Format.Formatter = New Common.Format.Formatter

            _udtEHSPersonalInformation = _udtEHSAccountModel.EHSPersonalInformationList.Filter(_strDocumentType.Trim)

            If _blnShowAccID Then
                pnlShowAccountID.Visible = True

                ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                ' --------------------------------------------------------------------------------------
                'Show Account ID / Ref No.
                If _blnShowAccountTypeAndStatus Then
                    ' Control the width of the column
                    tblAccID.Rows(0).Cells(0).Width = _intWidth
                    tblAccID.Rows(0).Cells(1).Width = _intWidth2
                    tblAccID.Rows(0).Cells(2).Width = _intWidth
                    tblAccID.Rows(0).Cells(3).Width = _intWidth2

                    ConstructAccountID(_udtEHSAccountModel, clbtnAccountIDT, lblAccountIDTText, lblAccountIDT)
                Else
                    ' Control the width of the column
                    tblAccID.Rows(1).Cells(0).Width = _intWidth
                    tblAccID.Rows(1).Cells(1).Width = _intWidth2 + _intWidth + _intWidth2

                    ConstructAccountID(_udtEHSAccountModel, clbtnAccountID, lblAccountIDText, lblAccountID)
                End If

                'Show Account Status 
                If _blnShowAccStatus AndAlso Not _udtEHSAccountModel.RecordStatus.Trim.Equals("A") Then
                    lblAccountStatus.Text = " (" + _udtEHSAccountModel.GetRecordStatusDescription() + ")"
                    lblAccountStatus.ForeColor = Drawing.Color.Red

                    lblAccountStatusT.Text = " (" + _udtEHSAccountModel.GetRecordStatusDescription() + ")"
                    lblAccountStatusT.ForeColor = Drawing.Color.Red
                Else
                    Me.lblAccountStatus.Text = String.Empty
                    Me.lblAccountStatusT.Text = String.Empty
                End If


                ' Account Type and Account Status
                If _blnShowAccountTypeAndStatus Then
                    'Account ID only: Hide
                    trAccountID.Style.Add("display", "none")

                    'Account ID & Type: Show
                    trAccountIDType.Style.Remove("display")
                    lblAccountTypeText.Visible = True
                    lblAccountType.Visible = True

                    'Account Status: Show
                    trAccountStatus.Style.Remove("display")
                    lblEHealthAccountStatusText.Visible = True
                    lblEHealthAccountStatus.Visible = True


                    Status.GetDescriptionFromDBCode("SysAccountSourceClass", _udtEHSAccountModel.AccountSourceString, lblAccountType.Text, String.Empty)
                    lblEHealthAccountStatus.Text = _udtEHSAccountModel.GetRecordStatusDescription()

                Else
                    'Account ID only: Show
                    trAccountID.Style.Remove("display")

                    'Account ID & Type: Hide
                    trAccountIDType.Style.Add("display", "none")
                    lblAccountTypeText.Visible = False
                    lblAccountType.Visible = False

                    'Account Status: Hide
                    trAccountStatus.Style.Add("display", "none")
                    lblEHealthAccountStatusText.Visible = False
                    lblEHealthAccountStatus.Visible = False

                End If
                ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            Else
                pnlShowAccountID.Visible = False
            End If

        Else
            pnlShowAccountID.Visible = False
        End If

    End Sub

    Private Sub ConstructAccountID(ByVal udtEHSAccount As EHSAccount.EHSAccountModel, _
                                   ByVal udcAccountID As CustomControls.CustomLinkButton, _
                                   ByVal udclblAccountIDText As Label, _
                                   ByVal udclblAccountID As Label)

        Dim udtformatter As Common.Format.Formatter = New Common.Format.Formatter

        'Show Account ID / Ref No.
        Select Case udtEHSAccount.AccountSource
            Case SysAccountSource.ValidateAccount
                If IsNothing(udtEHSAccount.VoucherAccID) Then
                    pnlShowAccountID.Visible = False
                Else
                    udclblAccountIDText.Text = Me.GetGlobalResourceObject("Text", "AccountID")

                    ' If _blnShowAccIDAsBtn = true Account ID will show as Button, otherwise show as label
                    If _blnShowAccIDAsBtn Then
                        udcAccountID.Text = udtformatter.formatValidatedEHSAccountNumber(udtEHSAccount.VoucherAccID)
                        udcAccountID.TargetFunctionCode = FunctCode.FUNT010302
                        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                        udcAccountID.TargetUrl = RedirectHandler.AppendPageKeyToURL(GetURL(FUNCTION_CODE_EHEALTH_ACCOUNT_ENQUIRY))
                        ' CRE19-026 (HCVS hotline service) [End][Winnie]

                        If udcAccountID.Build() Then
                            udcAccountID.ConstructNewRedirectParameter()
                            udcAccountID.RedirectParameter.EHealthAccountID = udtEHSAccount.VoucherAccID
                            udcAccountID.RedirectParameter.EHealthAccountDocCode = _strDocumentType
                            udcAccountID.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
                            udcAccountID.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)
                        End If
                    Else
                        udclblAccountID.Text = udtformatter.formatValidatedEHSAccountNumber(udtEHSAccount.VoucherAccID)

                    End If
                End If

            Case SysAccountSource.SpecialAccount, SysAccountSource.TemporaryAccount
                If IsNothing(udtEHSAccount.VoucherAccID) Then
                    pnlShowAccountID.Visible = False
                Else
                    udclblAccountIDText.Text = Me.GetGlobalResourceObject("Text", "RefNo")

                    ' If _blnShowAccIDAsBtn = true Account ID will show as Button, otherwise show as label
                    If _blnShowAccIDAsBtn Then
                        udcAccountID.Text = udtformatter.formatSystemNumber(udtEHSAccount.VoucherAccID)
                        udcAccountID.TargetFunctionCode = FunctCode.FUNT010302
                        ' CRE19-026 (HCVS hotline service) [Start][Winnie]
                        udcAccountID.TargetUrl = RedirectHandler.AppendPageKeyToURL(GetURL(FUNCTION_CODE_EHEALTH_ACCOUNT_ENQUIRY))
                        ' CRE19-026 (HCVS hotline service) [End][Winnie]

                        If udcAccountID.Build() Then
                            udcAccountID.ConstructNewRedirectParameter()
                            udcAccountID.RedirectParameter.EHealthAccountReferenceNo = udtEHSAccount.VoucherAccID
                            udcAccountID.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.Search)
                            udcAccountID.RedirectParameter.ActionList.Add(RedirectParameterModel.EnumRedirectAction.ViewDetail)
                        End If
                    Else
                        udclblAccountID.Text = udtformatter.formatSystemNumber(udtEHSAccount.VoucherAccID)

                    End If

                End If

        End Select
    End Sub

#Region "Properties"

    Public WriteOnly Property MaskIdentityNo() As Boolean
        Set(ByVal value As Boolean)
            _blnMaskIdentityNo = value
        End Set
    End Property

    Public WriteOnly Property Vertical() As Boolean
        Set(ByVal value As Boolean)
            _blnVertical = value
        End Set
    End Property

    Public WriteOnly Property Width() As Integer
        Set(ByVal value As Integer)
            _intWidth = value
        End Set
    End Property

    Public WriteOnly Property Width2() As Integer
        Set(ByVal value As Integer)
            _intWidth2 = value
        End Set
    End Property

    Public WriteOnly Property DocumentType() As String
        Set(ByVal value As String)
            _strDocumentType = value
        End Set
    End Property

    Public WriteOnly Property EHSPersonalInformation() As EHSPersonalInformationModel
        Set(ByVal value As EHSPersonalInformationModel)
            _udtEHSPersonalInformation = value
        End Set
    End Property

    Public WriteOnly Property EHSAccountModel() As EHSAccount.EHSAccountModel
        Set(ByVal value As EHSAccount.EHSAccountModel)
            _udtEHSAccountModel = value
        End Set
    End Property

    Public WriteOnly Property IsInvalidAccount() As Boolean
        Set(ByVal value As Boolean)
            _blnIsInvalidAccount = value
        End Set
    End Property

    Public WriteOnly Property OriginalAccType() As String
        Set(ByVal value As String)
            _strOriginalAccType = value
        End Set
    End Property

    Public WriteOnly Property OriginalAccID() As String
        Set(ByVal value As String)
            _strOriginalAccID = value
        End Set
    End Property

    Public WriteOnly Property ShowAccountID() As Boolean
        Set(ByVal value As Boolean)
            _blnShowAccID = value
        End Set
    End Property

    ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [Start][Dickson]
    Public WriteOnly Property ShowAccountIDAsBtn() As Boolean
        Set(ByVal value As Boolean)
            _blnShowAccIDAsBtn = value
        End Set
    End Property
    ' CRE17-006 Add eHA ID to eHA enquiry-scheme information [End][Dickson]

    Public WriteOnly Property ShowAccountStatus() As Boolean
        Set(ByVal value As Boolean)
            _blnShowAccStatus = value
        End Set
    End Property

    Public WriteOnly Property DisplayFormat() As EnumDisplayFormat
        Set(ByVal value As EnumDisplayFormat)
            _enumDisplayFormat = value
        End Set
    End Property

    ''' <summary>
    ''' CRE11-007
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ShowDateOfDeath() As Boolean
        Get
            Return _blnShowDateOfDeath
        End Get
        Set(ByVal value As Boolean)
            _blnShowDateOfDeath = value
        End Set
    End Property

    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Dickson]
    Public WriteOnly Property ShowDateOfDeathBtn() As Boolean
        Set(ByVal value As Boolean)
            _blnShowDateOfDeathBtn = value
        End Set
    End Property
    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Dickson]

    Public WriteOnly Property ShowAccountTypeAndStatus() As Boolean
        Set(ByVal value As Boolean)
            _blnShowAccountTypeAndStatus = value
        End Set
    End Property

    ' CRE17-010 (OCSSS integration) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Property ShowHKICSymbol() As Boolean
        Get
            Return _blnEnableToShowHKICSymbol
        End Get
        Set(value As Boolean)
            _blnEnableToShowHKICSymbol = value
        End Set
    End Property
    ' CRE17-010 (OCSSS integration) [End][Chris YIM]

    ' CRE19-026 (HCVS hotline service) [Start][Winnie]
    ' ------------------------------------------------------------------------
    Public WriteOnly Property ShowCreationMethod() As Boolean
        Set(value As Boolean)
            _blnShowCreationMethod = value
        End Set
    End Property
    ' CRE19-026 (HCVS hotline service) [End][Winnie]
#End Region

#Region "Events"

    Protected Sub ibtnShowOriginalAcc_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnShowOriginalAcc.Click
        'Retrieve all properties
        Me._blnIsInvalidAccount = getSession("_blnIsInvalidAccount")
        Me._udtEHSPersonalInformation = getSession("_udtEHSPersonalInformation")
        Me._blnMaskIdentityNo = getSession("_blnMaskIdentityNo")
        Me._blnVertical = getSession("_blnVertical")
        Me._intWidth = getSession("_intWidth")
        Me._intWidth2 = getSession("_intWidth2")
        Me._strDocumentType = getSession("_strDocumentType")
        Me._strOriginalAccID = getSession("_strOriginalAccID")
        Me._strOriginalAccType = getSession("_strOriginalAccType")

        'Force re-bulid of ucReadOnlyEHSClaim
        Session(ucReadOnlyEHSClaim.strForceRebuildEHSClaim) = "Y"

        Me.Build(True)
        popupOriginalAcc.Show()
    End Sub

    Protected Sub ibtnCloseOriginalAcc_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ibtnCloseOriginalAcc.Click

        'Force re-bulid of ucReadOnlyEHSClaim
        Session(ucReadOnlyEHSClaim.strForceRebuildEHSClaim) = "Y"

        popupOriginalAcc.Hide()
    End Sub
#End Region


#Region "Internal Session Handling"

    Private Sub setSessionValue(ByVal strKey As String, ByVal objValue As Object)

        Dim objInternalSession As Hashtable = Nothing
        Dim objSession As HttpSessionState = Nothing

        objSession = HttpContext.Current.Session

        If objSession(strSessionCollectionKey) Is Nothing Then
            objInternalSession = New System.Collections.Hashtable(100)
            objInternalSession.Add(strKey, objValue)
            objSession(strSessionCollectionKey) = objInternalSession
        Else
            objInternalSession = CType(objSession(strSessionCollectionKey), Hashtable)
            If (objInternalSession.ContainsKey(strKey)) Then
                objInternalSession(strKey) = objValue
            Else
                objInternalSession.Add(strKey, objValue)
            End If
        End If

    End Sub

    Private Function getSession(ByVal strKey As String)

        Dim objInternalSession As Hashtable = Nothing
        Dim objSession As HttpSessionState = Nothing

        objSession = HttpContext.Current.Session

        If Not (objSession(strSessionCollectionKey) Is Nothing) Then
            objInternalSession = CType(objSession(strSessionCollectionKey), Hashtable)
        End If

        If (objInternalSession Is Nothing) Then
            Return Nothing
        End If

        Return objInternalSession(strKey)

    End Function

    Private Sub clearSession()
        If Not IsNothing(Session(strSessionCollectionKey)) Then
            Session(strSessionCollectionKey) = Nothing
        End If
    End Sub

#End Region

    Private Const FUNCTION_CODE_EHEALTH_ACCOUNT_ENQUIRY As String = Common.Component.FunctCode.FUNT010302

    Protected Sub clbtnAccountID_Click(ByVal sender As LinkButton, ByVal e As System.EventArgs)
        Dim clbtn As CustomLinkButton = sender.Parent

        Dim udtAuditLog As New AuditLogEntry(CType(Me.Page, BasePage).FunctionCode)
        udtAuditLog.AddDescripton("Account ID", clbtn.RedirectParameter.EHealthAccountID)
        udtAuditLog.AddDescripton("Doc Code", clbtn.RedirectParameter.EHealthAccountDocCode)
        udtAuditLog.AddDescripton("Account Reference No.", clbtn.RedirectParameter.EHealthAccountReferenceNo)
        udtAuditLog.WriteLog(LogID.LOG01131, "eHealth Account ID Hyperlink click")

        clbtn.TargetUrl = RedirectHandler.AppendPageKeyToURL(GetURL(FUNCTION_CODE_EHEALTH_ACCOUNT_ENQUIRY))
        clbtn.Redirect()

    End Sub

    ''' <summary>
    ''' Get corresponding page URL by function code
    ''' </summary>
    ''' <param name="strFunctionCode">Function Code</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Shared Function GetURL(ByVal strFunctionCode As String) As String
        Dim udtMenuBLL As New Component.Menu.MenuBLL
        Dim drs() As DataRow = udtMenuBLL.GetMenuItemTable.Select(String.Format("Function_Code='{0}'", strFunctionCode))
        If drs.Length = 0 Then Return String.Empty

        ' Record_Status must 'A' if get from cache
        Return drs(0)("URL")
    End Function

End Class