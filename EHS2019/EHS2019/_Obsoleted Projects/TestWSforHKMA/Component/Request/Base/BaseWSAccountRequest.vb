Imports Microsoft.VisualBasic
Imports System.Xml
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Validation
Imports Common.Component.StaticData

Namespace Component.Request.Base

    Public MustInherit Class BaseWSAccountRequest
        Inherits BaseWSSPRequest

#Region "Protected Constant"

        Protected Const TAG_EHS_ACCOUNT_INFO As String = "eHSAccountInfo"
        Protected Const TAG_DOC_TYPE As String = "DocType"
        Protected Const TAG_ENTRY_NO As String = "EntryNo"
        Protected Const TAG_DOCUMENT_NO As String = "DocumentNo"
        Protected Const TAG_HKIC As String = "HKIC"
        Protected Const TAG_REG_NO As String = "RegNo"
        Protected Const TAG_BIRTH_ENTRY_NO As String = "BirthEntryNo"
        Protected Const TAG_PERMIT_NO As String = "PermitNo"
        Protected Const TAG_VISA_NO As String = "VisaNo"
        Protected Const TAG_SURNAME As String = "Surname"
        Protected Const TAG_GIVEN_NAME As String = "GivenName"
        Protected Const TAG_GENDER As String = "Gender"
        Protected Const TAG_DOB As String = "DOB"
        Protected Const TAG_DOB_TYPE As String = "DOBType"
        Protected Const TAG_AGE_ON As String = "AgeOn"
        Protected Const TAG_DOREG As String = "DOReg"
        Protected Const TAG_DOB_IN_WORD As String = "DOBInWord"
        Protected Const TAG_NAME_CHI As String = "NameChi"
        Protected Const TAG_DOI As String = "DOI"
        Protected Const TAG_SERIAL_NO As String = "SerialNo"
        Protected Const TAG_REFERENCE As String = "Reference"
        Protected Const TAG_FREE_REF As String = "FreeRef"
        Protected Const TAG_REMAIN_UNTIL As String = "RemainUntil"
        Protected Const TAG_PASSPORT_NO As String = "PassportNo"

        Protected udtvalidator As Validator = New Validator
        Protected udtformatter As Common.Format.Formatter = New Common.Format.Formatter
#End Region

#Region "Properties"

        Protected _strDocType As String = String.Empty
        Public Property DocType() As String
            Get
                Return _strDocType
            End Get
            Set(ByVal value As String)
                _strDocType = value
            End Set
        End Property

        Protected _strEntryNo As String = String.Empty
        Public Property EntryNo() As String
            Get
                Return _strEntryNo
            End Get
            Set(ByVal value As String)
                _strEntryNo = value
            End Set
        End Property

        'Protected _strIdentityNo As String = String.Empty
        Public Property IdentityNo() As String
            Get
                Return _strIdentityNo
            End Get
            Set(ByVal value As String)
                _strIdentityNo = value
            End Set
        End Property



        Protected _strDocumentNo As String = String.Empty
        Public Property DocumentNo() As String
            Get
                Return _strDocumentNo
            End Get
            Set(ByVal value As String)
                _strDocumentNo = value
            End Set
        End Property

        Protected _strHKIC As String = String.Empty
        Public Property HKIC() As String
            Get
                Return _strHKIC
            End Get
            Set(ByVal value As String)
                _strHKIC = value
            End Set
        End Property

        Protected _strRegNo As String = String.Empty
        Public Property RegNo() As String
            Get
                Return _strRegNo
            End Get
            Set(ByVal value As String)
                _strRegNo = value
            End Set
        End Property

        Protected _strBirthEntryNo As String = String.Empty
        Public Property BirthEntryNo() As String
            Get
                Return _strBirthEntryNo
            End Get
            Set(ByVal value As String)
                _strBirthEntryNo = value
            End Set
        End Property

        Protected _strPermitNo As String = String.Empty
        Public Property PermitNo() As String
            Get
                Return _strPermitNo
            End Get
            Set(ByVal value As String)
                _strPermitNo = value
            End Set
        End Property

        Protected _strVISANo As String = String.Empty
        Public Property VISANo() As String
            Get
                Return _strVISANo
            End Get
            Set(ByVal value As String)
                _strVISANo = value
            End Set
        End Property

        Protected _strSurname As String = String.Empty
        Public Property Surname() As String
            Get
                Return _strSurname
            End Get
            Set(ByVal value As String)
                _strSurname = value
            End Set
        End Property

        Protected _strGivenName As String = String.Empty
        Public Property GivenName() As String
            Get
                Return _strGivenName
            End Get
            Set(ByVal value As String)
                _strGivenName = value
            End Set
        End Property

        Protected _strGender As String = String.Empty
        Public Property Gender() As String
            Get
                Return _strGender
            End Get
            Set(ByVal value As String)
                _strGender = value
            End Set
        End Property

        Protected _strDOB As String = String.Empty
        Public Property DOB() As String
            Get
                Return _strDOB
            End Get
            Set(ByVal value As String)
                _strDOB = value
            End Set
        End Property

        Protected _strDOBType As String = String.Empty
        Public Property DOBType() As String
            Get
                Return _strDOBType
            End Get
            Set(ByVal value As String)
                _strDOBType = value
            End Set
        End Property

        Protected _strExactDOB As String = String.Empty
        Public Property ExactDOB() As String
            Get
                Return _strExactDOB
            End Get
            Set(ByVal value As String)
                _strExactDOB = value
            End Set
        End Property

        Protected _strAgeOn As String = String.Empty
        Public Property AgeOn() As String
            Get
                Return _strAgeOn
            End Get
            Set(ByVal value As String)
                _strAgeOn = value
            End Set
        End Property

        Protected _strDOReg As String = String.Empty
        Public Property DOReg() As String
            Get
                Return _strDOReg
            End Get
            Set(ByVal value As String)
                _strDOReg = value
            End Set
        End Property

        Protected _strDOBInWord As String = String.Empty
        Public Property DOBInWord() As String
            Get
                Return _strDOBInWord
            End Get
            Set(ByVal value As String)
                _strDOBInWord = value
            End Set
        End Property

        Protected _strNameChi As String = String.Empty
        Public Property NameChi() As String
            Get
                Return _strNameChi
            End Get
            Set(ByVal value As String)
                _strNameChi = value
            End Set
        End Property

        Protected _strDOI As String = String.Empty
        Public Property DOI() As String
            Get
                Return _strDOI
            End Get
            Set(ByVal value As String)
                _strDOI = value
            End Set
        End Property

        Protected _strSerialNo As String = String.Empty
        Public Property SerialNo() As String
            Get
                Return _strSerialNo
            End Get
            Set(ByVal value As String)
                _strSerialNo = value
            End Set
        End Property

        Protected _strReference As String = String.Empty
        Public Property Reference() As String
            Get
                Return _strReference
            End Get
            Set(ByVal value As String)
                _strReference = value
            End Set
        End Property

        Protected _strFreeRef As String = False
        Public Property FreeReference() As String
            Get
                Return _strFreeRef
            End Get
            Set(ByVal value As String)
                _strFreeRef = value
            End Set
        End Property

        Protected _strRemainUntil As String = String.Empty
        Public Property RemainUntil() As String
            Get
                Return _strRemainUntil
            End Get
            Set(ByVal value As String)
                _strRemainUntil = value
            End Set
        End Property

        Protected _strPassportNo As String = String.Empty
        Public Property PassportNo() As String
            Get
                Return _strPassportNo
            End Get
            Set(ByVal value As String)
                _strPassportNo = value
            End Set
        End Property

        Protected _strENameSurName As String = String.Empty
        Protected _strENameFirstName As String = String.Empty

        Protected _strPrefix As String = String.Empty
        Protected _strIdentityNo As String = String.Empty



        Private _blnDocType_Received As Boolean = False
        Public Property DocType_Included() As Boolean
            Get
                Return _blnDocType_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDocType_Received = value
            End Set
        End Property

        Private _blnEntryNo_Received As Boolean = False
        Public Property EntryNo_Included() As Boolean
            Get
                Return _blnEntryNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnEntryNo_Received = value
            End Set
        End Property

        Private _blnDocumentNo_Received As Boolean = False
        Public Property DocumentNo_Included() As Boolean
            Get
                Return _blnDocumentNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDocumentNo_Received = value
            End Set
        End Property

        Private _blnHKIC_Received As Boolean = False
        Public Property HKIC_Included() As Boolean
            Get
                Return _blnHKIC_Received
            End Get
            Set(ByVal value As Boolean)
                _blnHKIC_Received = value
            End Set
        End Property

        Private _blnRegNo_Received As Boolean = False
        Public Property RegNo_Included() As Boolean
            Get
                Return _blnRegNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnRegNo_Received = value
            End Set
        End Property

        Private _blnBirthEntryNo_Received As Boolean = False
        Public Property BirthEntryNo_Included() As Boolean
            Get
                Return _blnBirthEntryNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnBirthEntryNo_Received = value
            End Set
        End Property

        Private _blnPermitNo_Received As Boolean = False
        Public Property PermitNo_Included() As Boolean
            Get
                Return _blnPermitNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnPermitNo_Received = value
            End Set
        End Property

        Private _blnVISANo_Received As Boolean = False
        Public Property VISANo_Included() As Boolean
            Get
                Return _blnVISANo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnVISANo_Received = value
            End Set
        End Property

        Private _blnSurname_Received As Boolean = False
        Public Property Surname_Included() As Boolean
            Get
                Return _blnSurname_Received
            End Get
            Set(ByVal value As Boolean)
                _blnSurname_Received = value
            End Set
        End Property

        Private _blnGivenName_Received As Boolean = False
        Public Property GivenName_Included() As Boolean
            Get
                Return _blnGivenName_Received
            End Get
            Set(ByVal value As Boolean)
                _blnGivenName_Received = value
            End Set
        End Property

        Private _blnGender_Received As Boolean = False
        Public Property Gender_Included() As Boolean
            Get
                Return _blnGender_Received
            End Get
            Set(ByVal value As Boolean)
                _blnGender_Received = value
            End Set
        End Property

        Private _blnDOB_Received As Boolean = False
        Public Property DOB_Included() As Boolean
            Get
                Return _blnDOB_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDOB_Received = value
            End Set
        End Property

        Private _blnDOBType_Received As Boolean = False
        Public Property DOBType_Included() As Boolean
            Get
                Return _blnDOBType_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDOBType_Received = value
            End Set
        End Property

        Private _blnAgeOn_Received As Boolean = False
        Public Property AgeOn_Included() As Boolean
            Get
                Return _blnAgeOn_Received
            End Get
            Set(ByVal value As Boolean)
                _blnAgeOn_Received = value
            End Set
        End Property

        Private _blnDOReg_Received As Boolean = False
        Public Property DOReg_Included() As Boolean
            Get
                Return _blnDOReg_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDOReg_Received = value
            End Set
        End Property

        Private _blnDOBInWord_Received As Boolean = False
        Public Property DOBInWord_Included() As Boolean
            Get
                Return _blnDOBInWord_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDOBInWord_Received = value
            End Set
        End Property

        Private _blnNameChi_Received As Boolean = False
        Public Property NameChi_Included() As Boolean
            Get
                Return _blnNameChi_Received
            End Get
            Set(ByVal value As Boolean)
                _blnNameChi_Received = value
            End Set
        End Property

        Private _blnDOI_Received As Boolean = False
        Public Property DOI_Included() As Boolean
            Get
                Return _blnDOI_Received
            End Get
            Set(ByVal value As Boolean)
                _blnDOI_Received = value
            End Set
        End Property

        Private _blnSerialNo_Received As Boolean = False
        Public Property SerialNo_Included() As Boolean
            Get
                Return _blnSerialNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnSerialNo_Received = value
            End Set
        End Property

        Private _blnReference_Received As Boolean = False
        Public Property Reference_Included() As Boolean
            Get
                Return _blnReference_Received
            End Get
            Set(ByVal value As Boolean)
                _blnReference_Received = value
            End Set
        End Property

        Private _blnFreeRef_Received As Boolean = False
        Public Property FreeRef_Included() As Boolean
            Get
                Return _blnFreeRef_Received
            End Get
            Set(ByVal value As Boolean)
                _blnFreeRef_Received = value
            End Set
        End Property

        Private _blnRemainUntil_Received As Boolean = False
        Public Property RemainUntil_Included() As Boolean
            Get
                Return _blnRemainUntil_Received
            End Get
            Set(ByVal value As Boolean)
                _blnRemainUntil_Received = value
            End Set
        End Property

        Private _blnPassportNo_Received As Boolean = False
        Public Property PassportNo_Included() As Boolean
            Get
                Return _blnPassportNo_Received
            End Get
            Set(ByVal value As Boolean)
                _blnPassportNo_Received = value
            End Set
        End Property

#End Region


    End Class

End Namespace


