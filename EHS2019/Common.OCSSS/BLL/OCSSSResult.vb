Public Class OCSSSResult

    Public Enum OCSSSConnection
        Success
        Fail
        TurnOff
        BeforeEffectiveDate
        SkipForChecking
    End Enum

    Public Enum OCSSSEligibleResult
        Valid
        Invalid
        Unknown
    End Enum

#Region "Private members"

    Private _enumConnectionStatus As OCSSSConnection = OCSSSConnection.Fail
    Private _enumEligibleResult As OCSSSEligibleResult = OCSSSEligibleResult.Unknown
    Private _strOCSSSStatus As String = String.Empty
    Private _ex As Exception = Nothing

#End Region

#Region "Public properties"

    Public ReadOnly Property ConnectionStatus As OCSSSConnection
        Get
            Return _enumConnectionStatus
        End Get
    End Property

    Public ReadOnly Property EligibleResult As OCSSSEligibleResult
        Get
            Return _enumEligibleResult
        End Get
    End Property

    Public ReadOnly Property Exception As Exception
        Get
            Return _ex
        End Get
    End Property

    ''' <summary>
    ''' OCSSS checking status for storing in [VoucherTransaction].[OCSSS_Status]
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public ReadOnly Property OCSSSStatus As String
        Get
            Return _strOCSSSStatus
        End Get
    End Property
#End Region


#Region "Constructor"

    ''' <summary>
    ''' Create OCSSS connection result
    ''' </summary>
    ''' <param name="enumOcsssConnection">Actual OCSSS connection status</param>
    ''' <param name="objEligibilityResponse">Actual OCSSS response object, pass Nothing if connection fail</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal enumOcsssConnection As OCSSSConnection, ByVal objEligibilityResponse As eligibilityResponse, Optional ByVal ex As Exception = Nothing)
        Me._enumConnectionStatus = enumOcsssConnection
        Me._ex = ex

        If objEligibilityResponse IsNot Nothing Then
            ' OCSSS has result returned
            Select Case objEligibilityResponse.checkingResult
                Case "Y" ' Patient is valid
                    Me._enumEligibleResult = OCSSSEligibleResult.Valid
                Case "N" ' Patient is invalid
                    Me._enumEligibleResult = OCSSSEligibleResult.Invalid
                Case "E" ' OCSSS return error
                    Me._enumConnectionStatus = OCSSSConnection.Fail
                    Me._enumEligibleResult = OCSSSEligibleResult.Unknown
            End Select
        Else
            ' OCSSS has no result returned
            Me._enumEligibleResult = OCSSSEligibleResult.Unknown
        End If

        BuildOCSSSStatus()
    End Sub

#End Region

    ''' <summary>
    ''' Build OCSSSStatus base on ConnectionStatus and EligibleResult,
    ''' Finally the OCSSSStatus will be stored in table [VoucherTransaction].[OCSSS_Status]
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BuildOCSSSStatus()
        _strOCSSSStatus = String.Empty
        Select Case ConnectionStatus
            Case OCSSSConnection.Fail
                _strOCSSSStatus = "C" ' Connection Fail
            Case OCSSSConnection.TurnOff
                _strOCSSSStatus = "N" ' Turned off OCSSS feature
            Case OCSSSConnection.SkipForChecking
                _strOCSSSStatus = "N" ' No need OCSSS checking for permanant resident (HKIC Symbol is A or R) or back office claim
            Case OCSSSConnection.BeforeEffectiveDate
                _strOCSSSStatus = ""   ' Before effective date of OCSSS checking
            Case OCSSSConnection.Success
                Select Case EligibleResult
                    Case OCSSSEligibleResult.Valid
                        _strOCSSSStatus = "V" ' OCSSS return valid
                    Case OCSSSEligibleResult.Invalid
                        _strOCSSSStatus = "I" ' OCSSS return invalid
                    Case OCSSSEligibleResult.Unknown
                        Throw New Exception(String.Format("Unhandled OCSSS EligibleResult when connection success {0}", EligibleResult.ToString))
                End Select
            Case Else
                Throw New Exception(String.Format("Unhandled OCSSS OCSSSConnection {0}", ConnectionStatus.ToString))
        End Select

    End Sub
End Class
