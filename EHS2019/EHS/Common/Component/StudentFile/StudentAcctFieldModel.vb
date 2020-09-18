Namespace Component.StudentFile

    <Serializable()> Partial Public Class StudentAcctFieldModel

#Region "Constructors"

        Public Sub New()
            _strClassNo = String.Empty
            _strContactNo = String.Empty
            _strConfirmToInject = String.Empty
            _strHKICSymbol = String.Empty
            _strServiceDate = String.Empty

            _strDocCode = String.Empty
            _strIdentityNum = String.Empty
            _strENameSurName = String.Empty
            _strENameFirstName = String.Empty
            _strCName = String.Empty
            _strCCCode1 = String.Empty
            _strCCCode2 = String.Empty
            _strCCCode3 = String.Empty
            _strCCCode4 = String.Empty
            _strCCCode5 = String.Empty
            _strCCCode6 = String.Empty
            _strDOB = String.Empty
            _strExactDOB = String.Empty
            _strGender = String.Empty
            _strDateofIssue = String.Empty
            _strPermitToRemainUntil = String.Empty
            _strForeign_Passport_No = String.Empty
            _strECSerialNo = String.Empty
            _strECReferenceNo = String.Empty
            _strECReferenceNoOtherFormat = String.Empty
            _strAdoptionPrefixNum = String.Empty

            _intECDOBType = 0
            _strECDateDay = String.Empty
            _strECDateMonth = String.Empty
            _strECDateYear = String.Empty
            _strECAge = String.Empty
            _strECDateOfRegDay = String.Empty
            _strECDateOfRegMonth = String.Empty
            _strECDateOfRegYear = String.Empty
        End Sub

#End Region

#Region "Private Member"
        Private _strClassNo As String
        Private _strContactNo As String
        Private _strConfirmToInject As String
        Private _strHKICSymbol As String
        Private _strServiceDate As String

        Private _strDocCode As String
        Private _strIdentityNum As String
        Private _strENameSurName As String
        Private _strENameFirstName As String
        Private _strCName As String
        Private _strCCCode1 As String
        Private _strCCCode2 As String
        Private _strCCCode3 As String
        Private _strCCCode4 As String
        Private _strCCCode5 As String
        Private _strCCCode6 As String
        Private _strDOB As String
        Private _strExactDOB As String
        Private _strGender As String
        Private _strDateofIssue As String
        Private _strPermitToRemainUntil As String
        Private _strForeign_Passport_No As String
        Private _strECSerialNo As String
        Private _strECReferenceNo As String
        Private _strECReferenceNoOtherFormat As String
        Private _strAdoptionPrefixNum As String

        'EC
        Private _intECDOBType As Integer
        Private _strECDateDay As String
        Private _strECDateMonth As String
        Private _strECDateYear As String
        Private _strECAge As String
        Private _strECDateOfRegDay As String
        Private _strECDateOfRegMonth As String
        Private _strECDateOfRegYear As String

#End Region

#Region "Property"
        Public Property ClassNo() As String
            Get
                Return _strClassNo
            End Get
            Set(ByVal value As String)
                _strClassNo = value
            End Set
        End Property

        Public Property ContactNo() As String
            Get
                Return _strContactNo
            End Get
            Set(ByVal value As String)
                _strContactNo = value
            End Set
        End Property

        Public Property ConfirmToInject() As Boolean
            Get
                Return IIf(_strConfirmToInject = YesNo.Yes, True, False)
            End Get
            Set(ByVal value As Boolean)
                _strConfirmToInject = IIf(value, YesNo.Yes, YesNo.No)
            End Set
        End Property

        Public Property HKICSymbol() As String
            Get
                Return _strHKICSymbol
            End Get
            Set(ByVal value As String)
                _strHKICSymbol = value
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

        Public Property DocCode() As String
            Get
                Return _strDocCode
            End Get
            Set(ByVal value As String)
                _strDocCode = value
            End Set
        End Property

        Public Property IdentityNum() As String
            Get
                Return _strIdentityNum
            End Get
            Set(ByVal value As String)
                _strIdentityNum = value
            End Set
        End Property

        Public Property ENameSurName() As String
            Get
                Return _strENameSurName
            End Get
            Set(ByVal value As String)
                _strENameSurName = value
            End Set
        End Property

        Public Property ENameFirstName() As String
            Get
                Return _strENameFirstName
            End Get
            Set(ByVal value As String)
                _strENameFirstName = value
            End Set
        End Property

        Public Property CName() As String
            Get
                Return _strCName
            End Get
            Set(ByVal value As String)
                _strCName = value
            End Set
        End Property

        Public Property CCCode1() As String
            Get
                Return _strCCCode1
            End Get
            Set(ByVal value As String)
                _strCCCode1 = value
            End Set
        End Property

        Public Property CCCode2() As String
            Get
                Return _strCCCode2
            End Get
            Set(ByVal value As String)
                _strCCCode2 = value
            End Set
        End Property

        Public Property CCCode3() As String
            Get
                Return _strCCCode3
            End Get
            Set(ByVal value As String)
                _strCCCode3 = value
            End Set
        End Property

        Public Property CCCode4() As String
            Get
                Return _strCCCode4
            End Get
            Set(ByVal value As String)
                _strCCCode4 = value
            End Set
        End Property

        Public Property CCCode5() As String
            Get
                Return _strCCCode5
            End Get
            Set(ByVal value As String)
                _strCCCode5 = value
            End Set
        End Property

        Public Property CCCode6() As String
            Get
                Return _strCCCode6
            End Get
            Set(ByVal value As String)
                _strCCCode6 = value
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

        Public Property ExactDOB() As String
            Get
                Return _strExactDOB
            End Get
            Set(ByVal value As String)
                _strExactDOB = value
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

        Public Property DateofIssue() As String
            Get
                Return _strDateofIssue
            End Get
            Set(ByVal value As String)
                _strDateofIssue = value
            End Set
        End Property

        Public Property PermitToRemainUntil() As String
            Get
                Return _strPermitToRemainUntil
            End Get
            Set(ByVal value As String)
                _strPermitToRemainUntil = value
            End Set
        End Property

        Public Property Foreign_Passport_No() As String
            Get
                Return _strForeign_Passport_No
            End Get
            Set(ByVal value As String)
                _strForeign_Passport_No = value
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

        Public Property ECReferenceNoOtherFormat() As String
            Get
                Return _strECReferenceNoOtherFormat
            End Get
            Set(ByVal value As String)
                _strECReferenceNoOtherFormat = value
            End Set
        End Property

        Public Property AdoptionPrefixNum() As String
            Get
                Return _strAdoptionPrefixNum
            End Get
            Set(ByVal value As String)
                _strAdoptionPrefixNum = value
            End Set
        End Property

        Public Property ECDOBType() As Integer
            Get
                Return _intECDOBType
            End Get
            Set(ByVal value As Integer)
                _intECDOBType = value
            End Set
        End Property

        Public Property ECDateDay() As String
            Get
                Return _strECDateDay
            End Get
            Set(ByVal value As String)
                _strECDateDay = value
            End Set
        End Property

        Public Property ECDateMonth() As String
            Get
                Return _strECDateMonth
            End Get
            Set(ByVal value As String)
                _strECDateMonth = value
            End Set
        End Property

        Public Property ECDateYear() As String
            Get
                Return _strECDateYear
            End Get
            Set(ByVal value As String)
                _strECDateYear = value
            End Set
        End Property

        Public Property ECAge() As String
            Get
                Return _strECAge
            End Get
            Set(ByVal value As String)
                _strECAge = value
            End Set
        End Property

        Public Property ECDateOfRegDay() As String
            Get
                Return _strECDateOfRegDay
            End Get
            Set(ByVal value As String)
                _strECDateOfRegDay = value
            End Set
        End Property

        Public Property ECDateOfRegMonth() As String
            Get
                Return _strECDateOfRegMonth
            End Get
            Set(ByVal value As String)
                _strECDateOfRegMonth = value
            End Set
        End Property

        Public Property ECDateOfRegYear() As String
            Get
                Return _strECDateOfRegYear
            End Get
            Set(ByVal value As String)
                _strECDateOfRegYear = value
            End Set
        End Property

#End Region

    End Class

End Namespace
