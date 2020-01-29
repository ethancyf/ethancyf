Imports System.Data.SqlClient
Imports Common.Component.ServiceProvider


Namespace Component.ThirdParty
    <Serializable()> Public Class ThirdPartyAdditionalFieldEnrolmentModel

#Region "Constant"

        Public Const SysCodeDataType As SqlDbType = SqlDbType.VarChar
        Public Const SysCodeDataSize As Integer = 50

        Public Const EnrolRefNoDataType As SqlDbType = ServiceProviderModel.EnrolRefNoDataType
        Public Const EnrolRefNoDataSize As Integer = ServiceProviderModel.EnrolRefNoDataSize

        Public Const PracticeDisplaySeqDataType As SqlDbType = SqlDbType.SmallInt
        Public Const PracticeDisplaySeqDataSize As Integer = 2

        Public Const AdditionalFieldIDDataType As SqlDbType = SqlDbType.VarChar
        Public Const AdditionalFieldIDDataSize As Integer = 50

        Public Const AdditionalFieldValueCodeDataType As SqlDbType = SqlDbType.VarChar
        Public Const AdditionalFieldValueCodeDataSize As Integer = 50
#End Region

        Public Enum EnumSysCode
            PCD
        End Enum

        Private _enumSysCode As EnumSysCode
        Public Property SysCode() As EnumSysCode
            Get
                Return _enumSysCode
            End Get
            Set(ByVal value As EnumSysCode)
                _enumSysCode = EnumSysCode.PCD
            End Set
        End Property

        Private _strEnrolRefNo As String
        Public Property EnrolRefNo() As String
            Get
                Return _strEnrolRefNo
            End Get
            Set(ByVal value As String)
                _strEnrolRefNo = value
            End Set
        End Property

        Private _intPracticeDisplaySeq As Integer
        Public Property PracticeDisplaySeq() As Integer
            Get
                Return _intPracticeDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intPracticeDisplaySeq = value
            End Set
        End Property

        Private _strAdditionalFieldID As String
        Public Property AdditionalFieldID() As String
            Get
                Return _strAdditionalFieldID
            End Get
            Set(ByVal value As String)
                _strAdditionalFieldID = value
            End Set
        End Property

        Private _strAdditionalFieldValueCode As String
        Public Property AdditionalFieldValueCode() As String
            Get
                Return _strAdditionalFieldValueCode
            End Get
            Set(ByVal value As String)
                _strAdditionalFieldValueCode = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtThirdPartyAdditionalFieldEnrolmentModel As ThirdPartyAdditionalFieldEnrolmentModel)
            Constructor(udtThirdPartyAdditionalFieldEnrolmentModel.SysCode, udtThirdPartyAdditionalFieldEnrolmentModel.EnrolRefNo, udtThirdPartyAdditionalFieldEnrolmentModel.PracticeDisplaySeq, udtThirdPartyAdditionalFieldEnrolmentModel.AdditionalFieldID, udtThirdPartyAdditionalFieldEnrolmentModel.AdditionalFieldValueCode)
        End Sub

        Public Sub New(ByVal strSysCode As String, ByVal strEnrolRefNo As String, _
                        ByVal intPracticeDisplaySeq As Integer, ByVal strAdditionalFieldID As String, ByVal strAdditionalFieldValueCode As String)
            Constructor(strSysCode, strEnrolRefNo, intPracticeDisplaySeq, strAdditionalFieldID, strAdditionalFieldValueCode)
        End Sub

        Public Sub Constructor(ByVal strSysCode As String, ByVal strEnrolRefNo As String, ByVal intPracticeDisplaySeq As Integer, ByVal strAdditionalFieldID As String, ByVal strAdditionalFieldValueCode As String)

            _enumSysCode = DirectCast([Enum].Parse(GetType(EnumSysCode), strSysCode), EnumSysCode)
            _strEnrolRefNo = strEnrolRefNo
            _intPracticeDisplaySeq = intPracticeDisplaySeq
            _strAdditionalFieldID = strAdditionalFieldID
            _strAdditionalFieldValueCode = strAdditionalFieldValueCode

        End Sub

    End Class
End Namespace