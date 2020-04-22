Imports System.Xml
Imports System.Xml.Serialization
Imports Common.eHRIntegration.BLL.eHRServiceBLL

Namespace Model.Xml.eHRService

#Region "Enum Constants"

    Public Enum eHRResultCode
        NA
        R1000_Success
        R1001_NoTokenAssigned
        R1002_TokenNotMatch
        R1004_TokenIssuedBySenderParty
        R1005_TokenNotIssuedBySenderParty
        R1006_TokenNotAvailable
        R2001_LoginAliasNotSet
        R9000_InvalidXmlElement
        R9001_InvalidParameter
        R9002_UserNotFound
        R9999_UnexpectedFailure
    End Enum

#End Region

    <XmlRoot("returnObj")>
    Public Class InGeteHRWebSXmlModel

        Public Status As String
        Public StatusDescription As String
        Public data As String

        Public ReadOnly Property StatusEnum As enumResultStatus
            Get
                Dim eResultStatus As enumResultStatus = Nothing

                If [Enum].TryParse(String.Format("R{0}", Status), eResultStatus) = False Then
                    Throw New InvalidOperationException(String.Format("InGeteHRWebSXmlModel.StatusEnum: Unexpected value (Status={0})", Status))
                End If

                Return eResultStatus

            End Get
        End Property

        Public Sub New()
            Status = String.Empty
            StatusDescription = String.Empty
            data = String.Empty
        End Sub

    End Class

    <XmlRoot("geteHRSSTokenInfoResult")>
    Public Class InGeteHRTokenInfoXmlModel

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

        Public ReadOnly Property ResultCodeEnum As eHRResultCode
            Get
                For Each e As eHRResultCode In System.Enum.GetValues(GetType(eHRResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("InGeteHRTokenInfoXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Public Sub New()
            HKID = String.Empty
            Timestamp = String.Empty
            IsCommonUser = String.Empty
            ExistingTokenID = String.Empty
            ExistingTokenIssuer = String.Empty
            IsExistingTokenShared = String.Empty
            NewTokenID = String.Empty
            NewTokenIssuer = String.Empty
            IsNewTokenShared = String.Empty
            ResultDescription = String.Empty
            ResultCode = String.Empty
        End Sub

        Public Sub New(enumeHSResultCode As eHRResultCode, strHKID As String, strTimestamp As String)
            Me.New()

            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("seteHRSSTokenSharedResult")>
    Public Class InSeteHRSSTokenSharedXmlModel

        Public HKID As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHRResultCode
            Get
                For Each e As eHRResultCode In System.Enum.GetValues(GetType(eHRResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("InSeteHRSSTokenSharedXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Public Sub New()
            HKID = String.Empty
            ResultDescription = String.Empty
            ResultCode = String.Empty
            Timestamp = String.Empty
        End Sub

        Public Sub New(enumeHSResultCode As eHRResultCode, strHKID As String, strTimestamp As String)
            Me.New()

            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("replaceeHRSSTokenResult")>
    Public Class InReplaceeHRSSTokenXmlModel

        Public HKID As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHRResultCode
            Get
                For Each e As eHRResultCode In System.Enum.GetValues(GetType(eHRResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("InReplaceeHRSSTokenXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Public Sub New()
            HKID = String.Empty
            ResultDescription = String.Empty
            ResultCode = String.Empty
            Timestamp = String.Empty
        End Sub

        Public Sub New(enumeHSResultCode As eHRResultCode, strHKID As String, strTimestamp As String)
            Me.New()

            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("notifyeHRSSTokenDeactivatedResult")>
    Public Class InNotifyeHRSSTokenDeactivatedXmlModel

        Public HKID As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHRResultCode
            Get
                For Each e As eHRResultCode In System.Enum.GetValues(GetType(eHRResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("InNotifyeHRSSTokenDeactivatedXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Public Sub New()
            HKID = String.Empty
            ResultDescription = String.Empty
            ResultCode = String.Empty
            Timestamp = String.Empty
        End Sub

        Public Sub New(enumeHSResultCode As eHRResultCode, strHKID As String, strTimestamp As String)
            Me.New()

            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("geteHRSSLoginAliasResult")>
    Public Class InGeteHRSSLoginAliasXmlModel

        Public HKID As String
        Public LoginAlias As String
        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHRResultCode
            Get
                For Each e As eHRResultCode In System.Enum.GetValues(GetType(eHRResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("InGeteHRSSLoginAliasXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Public Sub New()
            HKID = String.Empty
            LoginAlias = String.Empty
            ResultDescription = String.Empty
            ResultCode = String.Empty
            Timestamp = String.Empty
        End Sub

        Public Sub New(enumeHSResultCode As eHRResultCode, strHKID As String, strTimestamp As String)
            Me.New()

            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            HKID = strHKID
            Timestamp = strTimestamp

        End Sub

    End Class

    <XmlRoot("healthCheckeHRSSResult")>
    Public Class InHealthCheckeHRSSXmlModel

        Public ResultDescription As String
        Public ResultCode As String
        Public Timestamp As String

        Public ReadOnly Property ResultCodeEnum As eHRResultCode
            Get
                For Each e As eHRResultCode In System.Enum.GetValues(GetType(eHRResultCode))
                    If e.ToString.Contains(ResultCode) Then
                        Return e
                    End If
                Next

                Throw New Exception(String.Format("InHealthCheckeHRSSXmlModel: Unexpected value (ResultCode={0})", ResultCode))

            End Get
        End Property

        Public Sub New()
            ResultDescription = String.Empty
            ResultCode = String.Empty
            Timestamp = String.Empty
        End Sub

        Public Sub New(enumeHSResultCode As eHRResultCode, strTimestamp As String)
            Me.New()

            SupportFunction.ConvertResultCode(enumeHSResultCode, ResultCode, ResultDescription)
            Timestamp = strTimestamp

        End Sub

    End Class

    Public Class SupportFunction

        Public Shared Sub ConvertResultCode(enumeHRResultCode As eHRResultCode, ByRef strResultCode As String, ByRef strResultDescription As String)
            Select Case enumeHRResultCode
                Case eHRResultCode.R1000_Success
                    strResultCode = "1000"
                    strResultDescription = "Success"

                Case eHRResultCode.R1001_NoTokenAssigned
                    strResultCode = "1001"
                    strResultDescription = "No token is assigned"

                Case eHRResultCode.R1002_TokenNotMatch
                    strResultCode = "1002"
                    strResultDescription = "Token not match"

                Case eHRResultCode.R1004_TokenIssuedBySenderParty
                    strResultCode = "1004"
                    strResultDescription = "Token is issued by sender party"

                Case eHRResultCode.R1005_TokenNotIssuedBySenderParty
                    strResultCode = "1005"
                    strResultDescription = "Token is not issued by sender party"

                Case eHRResultCode.R1006_TokenNotAvailable
                    strResultCode = "1006"
                    strResultDescription = "Token is not available"

                Case eHRResultCode.R2001_LoginAliasNotSet
                    strResultCode = "2001"
                    strResultDescription = "No username is set"

                Case eHRResultCode.R9000_InvalidXmlElement
                    strResultCode = "9000"
                    strResultDescription = "Invalid XML Element"

                Case eHRResultCode.R9001_InvalidParameter
                    strResultCode = "9001"
                    strResultDescription = "Invalid Parameter"

                Case eHRResultCode.R9002_UserNotFound
                    strResultCode = "9002"
                    strResultDescription = "User is not found"

                Case eHRResultCode.R9999_UnexpectedFailure
                    strResultCode = "9999"
                    strResultDescription = "Unexpected Failure"

                Case Else
                    Throw New Exception(String.Format("InGeteHRWebSXmlModel.SupportFunction.ConvertResultCode: Unexepcted value (enumeHRResultCode={0})", enumeHRResultCode.ToString))

            End Select

        End Sub

    End Class

End Namespace
