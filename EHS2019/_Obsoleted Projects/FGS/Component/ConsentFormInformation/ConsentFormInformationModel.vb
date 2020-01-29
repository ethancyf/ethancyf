Public Class ConsentFormInformationModel

#Region "Field"

    Private _strRequestBy As String
    Private _strFormType As String
    Private _strLanguage As String
    Private _strFormStyle As String
    Private _strNeedPassword As String
    Private _strDocType As String
    Private _strSPName As String
    Private _strRecipientEName As String
    Private _strRecipientCName As String
    Private _strDOB As String
    Private _strGender As String
    Private _strDocNo As String
    Private _strDOI As String
    Private _strECSerialNo As String
    Private _strECReferenceNo As String
    Private _strServiceDate As String
    Private _strVoucherClaim As String

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------
    Private _strVoucherBeforeRedeem As String
    Private _strVoucherAfterRedeem As String
    Private _strCoPaymentFee As String

    Private _enumPlatform As EnumPlatform

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Private _strReadSmartID As String
    Private _strPermitUntil As String
    Private _strPassportNo As String
    Private _strSignDate As String
    Private _strSubsidyInfo As String
    Private _strPreschool As String

#End Region

#Region "Class"

    Public Class FormTypeClass
        Public Const HCVS As String = "HCVS"
        Public Const CIVSS As String = "CIVSS"
        Public Const EVSS As String = "EVSS"
    End Class

    Public Class LanguageClassExternal
        Public Const Chinese As String = "Chinese"
        Public Const English As String = "English"
    End Class

    Public Class LanguageClassInternal
        Public Const Chinese As String = "zh-tw"
        Public Const English As String = "en-us"
    End Class

    Public Class DocTypeClass
        Public Const HKIC As String = "HKIC"
        Public Const EC As String = "EC"
        Public Const HKBC As String = "HKBC"
        Public Const REPMT As String = "REPMT"
        Public Const DocI As String = "Doc/I"
        Public Const ID235B As String = "ID235B"
        Public Const VISA As String = "VISA"
        Public Const ADOPC As String = "ADOPC"
    End Class

    Public Class FormStyleClass
        Public Const Full As String = "Full"
        Public Const Condensed As String = "Condensed"
    End Class

    Public Class NeedPasswordClass
        Public Const Yes As String = "Yes"
        Public Const No As String = "No"
    End Class

    Public Class GenderClass
        Public Const Male As String = "M"
        Public Const Female As String = "F"
    End Class

    Public Class ReadSmartIDClass
        Public Const Yes As String = "Yes"
        Public Const No As String = "No"
        Public Const Unknown As String = ""
    End Class

    Public Class CIVSSSubsidyInfoClass
        Public Const Dose1 As String = "CSIV-1ST"
        Public Const Dose2 As String = "CSIV-2ND"
        Public Const DoseOnly As String = "CSIV"
    End Class

    Public Class PreschoolClass
        Public Const Not1stDose As String = "Not1stDose"
        Public Const Preschool As String = "PreSchool"
        Public Const NonPreschool As String = "NonPreSchool"
        Public Const Unknown As String = ""
    End Class

    Public Class EVSSSubsidyInfoClass
        Public Const PV As String = "23vPPV"
        Public Const SIV As String = "ESIV"
    End Class

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------
    Public Enum EnumPlatform
        None
        FGS
        HCSP
    End Enum
    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

#End Region

#Region "Constructor"

    Public Sub New()
        _strRequestBy = String.Empty
        _strFormType = String.Empty
        _strLanguage = String.Empty
        _strFormStyle = String.Empty
        _strNeedPassword = String.Empty
        _strDocType = String.Empty
        _strSPName = String.Empty
        _strRecipientEName = String.Empty
        _strRecipientCName = String.Empty
        _strDOB = String.Empty
        _strGender = String.Empty
        _strDocNo = String.Empty
        _strDOI = String.Empty
        _strECSerialNo = String.Empty
        _strECReferenceNo = String.Empty
        _strServiceDate = String.Empty
        _strVoucherClaim = String.Empty

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        _strVoucherAfterRedeem = String.Empty
        _strCoPaymentFee = String.Empty
        _enumPlatform = EnumPlatform.None

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        _strReadSmartID = String.Empty
        _strPermitUntil = String.Empty
        _strPassportNo = String.Empty
        _strSignDate = String.Empty
        _strSubsidyInfo = String.Empty
        _strPreschool = String.Empty

    End Sub

#End Region

#Region "Property"

    Public Property RequestBy() As String
        Get
            Return _strRequestBy
        End Get
        Set(ByVal value As String)
            _strRequestBy = value
        End Set
    End Property

    Public Property FormType() As String
        Get
            Return _strFormType
        End Get
        Set(ByVal value As String)
            _strFormType = value
        End Set
    End Property

    Public Property Language() As String
        Get
            Return _strLanguage
        End Get
        Set(ByVal value As String)
            _strLanguage = value
        End Set
    End Property

    Public Property FormStyle() As String
        Get
            Return _strFormStyle
        End Get
        Set(ByVal value As String)
            _strFormStyle = value
        End Set
    End Property

    Public Property DocType() As String
        Get
            Return _strDocType
        End Get
        Set(ByVal value As String)
            _strDocType = value
        End Set
    End Property

    Public Property SPName() As String
        Get
            Return _strSPName
        End Get
        Set(ByVal value As String)
            _strSPName = value
        End Set
    End Property

    Public Property RecipientEName() As String
        Get
            Return _strRecipientEName
        End Get
        Set(ByVal value As String)
            _strRecipientEName = value
        End Set
    End Property

    Public Property RecipientCName() As String
        Get
            Return _strRecipientCName
        End Get
        Set(ByVal value As String)
            _strRecipientCName = value
        End Set
    End Property

    Public Property DocNo() As String
        Get
            Return _strDocNo
        End Get
        Set(ByVal value As String)
            _strDocNo = value
        End Set
    End Property

    Public Property DOI() As String
        Get
            Return _strDOI
        End Get
        Set(ByVal value As String)
            _strDOI = value
        End Set
    End Property

    Public Property ECSerialNo() As String
        Get
            Return _strECSerialNo
        End Get
        Set(ByVal value As String)
            _strECSerialNo = value
        End Set
    End Property

    Public Property ECReferenceNo() As String
        Get
            Return _strECReferenceNo
        End Get
        Set(ByVal value As String)
            _strECReferenceNo = value
        End Set
    End Property

    Public Property ServiceDate() As String
        Get
            Return _strServiceDate
        End Get
        Set(ByVal value As String)
            _strServiceDate = value
        End Set
    End Property

    Public Property VoucherClaim() As String
        Get
            Return _strVoucherClaim
        End Get
        Set(ByVal value As String)
            _strVoucherClaim = value
        End Set
    End Property


    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------


    Public Property VoucherBeforeRedeem() As String
        Get
            Return _strVoucherBeforeRedeem
        End Get
        Set(ByVal value As String)
            _strVoucherBeforeRedeem = value
        End Set
    End Property

    Public Property VoucherAfterRedeem() As String
        Get
            Return _strVoucherAfterRedeem
        End Get
        Set(ByVal value As String)
            _strVoucherAfterRedeem = value
        End Set
    End Property

    Public Property CoPaymentFee() As String
        Get
            Return _strCoPaymentFee
        End Get
        Set(ByVal value As String)
            _strCoPaymentFee = value
        End Set
    End Property

    ''' <summary>
    ''' The platform request to generate consent form (Default: None)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Platform() As EnumPlatform
        Get
            Return _enumPlatform
        End Get
        Set(ByVal value As EnumPlatform)
            _enumPlatform = value
        End Set
    End Property

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

    Public Property ReadSmartID() As String
        Get
            Return _strReadSmartID
        End Get
        Set(ByVal value As String)
            _strReadSmartID = value
        End Set
    End Property

    Public Property Gender() As String
        Get
            Return _strGender
        End Get
        Set(ByVal value As String)
            _strGender = value
        End Set
    End Property

    Public Property PermitUntil() As String
        Get
            Return _strPermitUntil
        End Get
        Set(ByVal value As String)
            _strPermitUntil = value
        End Set
    End Property

    Public Property PassportNo() As String
        Get
            Return _strPassportNo
        End Get
        Set(ByVal value As String)
            _strPassportNo = value
        End Set
    End Property

    Public Property SignDate() As String
        Get
            Return _strSignDate
        End Get
        Set(ByVal value As String)
            _strSignDate = value
        End Set
    End Property

    Public Property DOB() As String
        Get
            Return _strDOB
        End Get
        Set(ByVal value As String)
            _strDOB = value
        End Set
    End Property

    Public Property SubsidyInfo() As String
        Get
            Return _strSubsidyInfo
        End Get
        Set(ByVal value As String)
            _strSubsidyInfo = value
        End Set
    End Property

    Public Property Preschool() As String
        Get
            Return _strPreschool
        End Get
        Set(ByVal value As String)
            _strPreschool = value
        End Set
    End Property

    Public Property NeedPassword() As String
        Get
            Return _strNeedPassword
        End Get
        Set(ByVal value As String)
            _strNeedPassword = value
        End Set
    End Property

#End Region

End Class
