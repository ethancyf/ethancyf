Imports Common.ComFunction
Imports Common.Component.EHSAccount

Namespace UIControl.DocTypeHCSP

    Public MustInherit Class ucReadOnlyDocTypeBase
        Inherits System.Web.UI.UserControl

        Private _isVertical As Boolean
        Private _showAccountRefNo As Boolean
        Private _showAccountCreationDate As Boolean
        Private _maskEntryNo As Boolean
        Private _udtEHSAccountPersonalInfo As EHSAccountModel.EHSPersonalInformationModel
        Private _udtEHSAccount As EHSAccountModel
        Private _showTempAccountNotice As Boolean
        Private _highLightDocType As Boolean
        Private _udtSessionHandler As BLL.SessionHandlerBLL = New BLL.SessionHandlerBLL
        Private _mode As ucInputDocTypeBase.BuildMode = ucInputDocTypeBase.BuildMode.Creation
        'Private _udtSmartIDContent As BLL.SmartIDContentModel
        Private _blnSmartID As Boolean
        Private _blnEnableToShowHKICSymbol As Boolean

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            Me.RenderLanguage()
            Me.Setup(Me._mode)
        End Sub

        Protected MustOverride Sub RenderLanguage()
        Protected MustOverride Sub Setup(ByVal mode As ucInputDocTypeBase.BuildMode)
        Public MustOverride Sub SetupTableTitle(ByVal width As Integer)

        Public Function GetEHSAccountReferenceNo() As String
            Dim formatter As Common.Format.Formatter = New Common.Format.Formatter()
            Dim strReferenceNo As String = String.Empty
            If Me._udtEHSAccount.AccountSource = EHSAccountModel.SysAccountSource.SpecialAccount Then
                strReferenceNo = formatter.formatSystemNumber(Me._udtEHSAccount.OriginalAccID)
            Else
                strReferenceNo = formatter.formatSystemNumber(Me._udtEHSAccountPersonalInfo.VoucherAccID)
            End If

            Return strReferenceNo
        End Function


#Region "Property"

        Protected ReadOnly Property SessionHandler() As BLL.SessionHandlerBLL
            Get
                Return Me._udtSessionHandler
            End Get
        End Property


        Public Property IsVertical() As Boolean
            Get
                Return Me._isVertical
            End Get
            Set(ByVal value As Boolean)
                Me._isVertical = value
            End Set
        End Property

        Public Property ShowAccountRefNo() As Boolean
            Get
                Return Me._showAccountRefNo
            End Get
            Set(ByVal value As Boolean)
                Me._showAccountRefNo = value
            End Set
        End Property

        Public Property ShowAccountCreationDate() As Boolean
            Get
                Return Me._showAccountCreationDate
            End Get
            Set(ByVal value As Boolean)
                Me._showAccountCreationDate = value
            End Set
        End Property

        Public Property ShowTempAccountNotice() As Boolean
            Get
                Return Me._showTempAccountNotice
            End Get
            Set(ByVal value As Boolean)
                Me._showTempAccountNotice = value
            End Set
        End Property

        Public Property MaskIdentityNumber() As Boolean
            Get
                Return Me._maskEntryNo
            End Get
            Set(ByVal value As Boolean)
                Me._maskEntryNo = value
            End Set
        End Property

        Public Property EHSAccountPersonalInfo() As EHSAccountModel.EHSPersonalInformationModel
            Get
                Return Me._udtEHSAccountPersonalInfo
            End Get
            Set(ByVal value As EHSAccountModel.EHSPersonalInformationModel)
                Me._udtEHSAccountPersonalInfo = value
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

        Public Property HightLightDocType() As Boolean
            Get
                Return Me._highLightDocType
            End Get
            Set(ByVal value As Boolean)
                Me._highLightDocType = value
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

        'Public Property SmartIDContent() As BLL.SmartIDContentModel
        '    Get
        '        Return Me._udtSmartIDContent
        '    End Get
        '    Set(ByVal value As BLL.SmartIDContentModel)
        '        Me._udtSmartIDContent = value
        '    End Set
        'End Property

        Public Property IsSmartID() As Boolean
            Get
                If _blnSmartID = False Then
                    Return False

                Else
                    Dim udtGeneralFunction As New GeneralFunction
                    Dim strParmValue As String = String.Empty
                    udtGeneralFunction.getSystemParameter("SmartIDShowRealID", strParmValue, String.Empty)
                    Return strParmValue.Trim = "Y"

                End If

            End Get
            Set(ByVal value As Boolean)
                _blnSmartID = value
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

#End Region

    End Class

End Namespace