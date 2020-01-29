Imports System.Xml
Imports System.Xml.Serialization

Namespace Model.Xml.eHSService

#Region "Enum Constants"

    Public Enum eHSResultCode
        NA
        R1000_Success
        R1001_NoTokenAssigned
        R1002_TokenNotMatch
        R1005_ExistingTokenNotIssuedBySenderParty
        R1006_NewTokenNotAvailable
        R2001_LoginAliasNotSet
        R9000_InvalidXmlElement
        R9001_InvalidParameter
        R9002_UserNotFound
        R9999_UnexpectedFailure
    End Enum

#End Region

    <XmlRoot("root")>
    Public Class OutSubmitRequestXmlModel

        Public data As String

    End Class

    <XmlRoot("geteHSSTokenInfoResult")>
    Public Class OutGeteHSSTokenInfoXmlModel

        Public HKID As String
        Public Timestamp As String
        Public IsCommonUser As String
        Public ExistingTokenID As String
        Public ExistingTokenIssuer As String
        Public IsExistingTokenShared As String
        Public NewTokenID As String
        Public NewTokenIssuer As String
        Public IsNewTokenShared As String
        Public ResultDescription As String
        Public ResultCode As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutGeteHSSTokenInfoXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode, strHKID As String, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("seteHSSTokenSharedResult")>
    Public Class OutSeteHSSTokenSharedXmlModel

        Public HKID As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutSeteHSSTokenSharedXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode, strHKID As String, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("replaceeHRSSTokenResult")>
    Public Class OutReplaceeHRSSTokenXmlModel

        Public HKID As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutReplaceeHRSSTokenXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode, strHKID As String, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("notifyeHRSSTokenDeactivatedResult")>
    Public Class OutNotifyeHRSSTokenDeactivatedXmlModel

        Public HKID As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutNotifyeHRSSTokenDeactivatedXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode, strHKID As String, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("geteHSSLoginAliasResult")>
    Public Class OutGeteHSSLoginAliasXmlModel

        Public HKID As String
        Public LoginAlias As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutGeteHSSLoginAliasXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode, strHKID As String, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("healthCheckeHSSResult")>
    Public Class OutHealthCheckeHSSXmlModel

        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutHealthCheckeHSSXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode, strTimestamp As String)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("errorResult")>
    Public Class OutErrorResultModel

        Public ResultDescription As String
        Public ResultCode As String

        Public ReadOnly Property ResultCodeEnum As eHSResultCode
            Get
                For Each e As eHSResultCode In System.Enum.GetValues(GetType(eHSResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("OutErrorResultModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Private Sub New()
            ' Not accessible
        End Sub

        Public Sub New(enumeHSResultCode As eHSResultCode)
            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)

        End Sub

    End Class

    '

    Public Class SupportFunction

        Public Shared Sub ConvertResultCode(enumeHSResultCode As eHSResultCode, ByRef strResultCode As String, ByRef strResultDescription As String)
            Select Case enumeHSResultCode
                Case eHSResultCode.NA

                Case eHSResultCode.R1000_Success
                    strResultCode = "1000"
                    strResultDescription = "Success"

                Case eHSResultCode.R1001_NoTokenAssigned
                    strResultCode = "1001"
                    strResultDescription = "No token is assigned"

                Case eHSResultCode.R1002_TokenNotMatch
                    strResultCode = "1002"
                    strResultDescription = "Token not match"

                Case eHSResultCode.R1005_ExistingTokenNotIssuedBySenderParty
                    strResultCode = "1005"
                    strResultDescription = "Existing token is not issued by sender party"

                Case eHSResultCode.R1006_NewTokenNotAvailable
                    strResultCode = "1006"
                    strResultDescription = "New token is not available"

                Case eHSResultCode.R2001_LoginAliasNotSet
                    strResultCode = "2001"
                    strResultDescription = "No username is set"

                Case eHSResultCode.R9000_InvalidXmlElement
                    strResultCode = "9000"
                    strResultDescription = "Invalid XML Element"

                Case eHSResultCode.R9001_InvalidParameter
                    strResultCode = "9001"
                    strResultDescription = "Invalid Parameter"

                Case eHSResultCode.R9002_UserNotFound
                    strResultCode = "9002"
                    strResultDescription = "User is not found"

                Case eHSResultCode.R9999_UnexpectedFailure
                    strResultCode = "9999"
                    strResultDescription = "Unexpected Failure"

                Case Else
                    Throw New Exception(String.Format("OutSubmitRequestXmlModel.SupportFunction.ConvertResultCode: Unexepcted value (enumeHSResultCode={0})", enumeHSResultCode.ToString))

            End Select

        End Sub

    End Class

End Namespace
