Imports Common.Component.ServiceProvider
Imports Common.Component.UserAC

Namespace Component.DataEntryUser
    <Serializable()> Public Class DataEntryUserModel
        Inherits UserACModel

        Private _strDataEntryAccount As String
        Private _strSPID As String
        Private _intPracticeDisplaySeq As Integer
        Private _intBankAccDisplaySeq As Integer
        Private _strSPEngName As String
        Private _strSPChiName As String
        Private _strSPRecordStatus As String
        Private _strHCSPUserACRecordStatus As String
        Private _intPracticeCnt As Integer
        Private _blnLocked As Boolean
        'Private _strDefaultLanguage As String
		Private _udtSP As ServiceProviderModel
        Private _aryPractice As ArrayList

        Public Sub New()

        End Sub

        Public Property DataEntryAccount() As String
            Get
                Return _strDataEntryAccount
            End Get
            Set(ByVal value As String)
                _strDataEntryAccount = value
            End Set
        End Property

        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value
            End Set
        End Property

        Public Property SPEngName() As String
            Get
                Return _strSPEngName
            End Get
            Set(ByVal value As String)
                _strSPEngName = value
            End Set
        End Property

        Public Property SPChiName() As String
            Get
                Return _strSPChiName
            End Get
            Set(ByVal value As String)
                _strSPChiName = value
            End Set
        End Property

        Public Property SPRecordStatus() As String
            Get
                Return _strSPRecordStatus
            End Get
            Set(ByVal value As String)
                _strSPRecordStatus = value
            End Set
        End Property

        Public Property Locked() As Boolean
            Get
                Return _blnLocked
            End Get
            Set(ByVal value As Boolean)
                _blnLocked = value
            End Set
        End Property

        Public Property HCSPUserACRecordStatus() As String
            Get
                Return _strHCSPUserACRecordStatus
            End Get
            Set(ByVal value As String)
                _strHCSPUserACRecordStatus = value
            End Set
        End Property

        Public Property PracticeCnt() As Integer
            Get
                Return _intPracticeCnt
            End Get
            Set(ByVal value As Integer)
                _intPracticeCnt = value
            End Set
        End Property

        Public Property ServiceProvider() As ServiceProviderModel
            Get
                Return _udtSP
            End Get
            Set(ByVal value As ServiceProviderModel)
                _udtSP = value
            End Set
        End Property

        Public Property PracticeList() As ArrayList
            Get
                Return _aryPractice
            End Get
            Set(ByVal value As ArrayList)
                _aryPractice = value
            End Set
        End Property

    End Class
End Namespace

