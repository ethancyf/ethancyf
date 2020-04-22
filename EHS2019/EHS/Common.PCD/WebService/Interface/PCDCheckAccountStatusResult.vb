Imports Common.ComObject
Imports Common.Component
Imports Common.Component.Mapping
Imports Common.Component.ServiceProvider
Imports Common.DataAccess
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Globalization
Imports System.Web

Namespace WebService.Interface

    <Serializable()> Public Class PCDCheckAccountStatusResult
        Inherits BaseResult

#Region "Constants"
        Public Enum enumReturnCode
            ' Active
            Success = 0

            ' Data Validation Fail
            DataValidationFail = 96

            ' Service Authentication Failed
            AuthenticationFailed = 97

            ' Invalid Parameter
            InvalidParameter = 98

            ' Enrolment (All Unexpected Errors)
            ErrorAllUnexpected = 99

            ' [eHS self error code] Fail to connect PCD web service, e.g. Timeout
            CommunicationLinkError = 101

        End Enum

        Public Enum enumAccountStatus
            ' Not Enrolled
            NotEnrolled = 1

            ' Enrolled
            Enrolled = 2

            ' Delisted
            Delisted = 3

            ' Connection Failed
            ConnectionFail = 4
        End Enum

        Public Enum enumEnrolmentStatus
            ' Unprocessed
            Unprocessed = 1

            ' Processing
            Processing = 2

            ' N/A
            NA = 3

            ' Connection Failed
            ConnectionFail = 4

        End Enum

#End Region

#Region "Private Members"
        Private _strMessageID As String = String.Empty
        Private _enumReturnCode As enumReturnCode = enumReturnCode.ErrorAllUnexpected
        Private _returnCodeDesc As String = String.Empty
        Private _enumAccountStatus As Nullable(Of enumAccountStatus) = Nothing
        Private _enumEnrolmentStatus As Nullable(Of enumEnrolmentStatus) = Nothing
        Private _strProfID As String = String.Empty
        Private _dtmLastCheckDtm As DateTime
        Private _objException As Exception
        Private _strRequest As String = String.Empty
        Private _strResult As String = String.Empty

#End Region

#Region "Properties"

        Public ReadOnly Property MessageID() As String
            Get
                Return _strMessageID
            End Get
        End Property

        Public ReadOnly Property ReturnCode() As enumReturnCode
            Get
                Return _enumReturnCode
            End Get
        End Property

        Public ReadOnly Property ReturnCodeDesc() As String
            Get
                Return _returnCodeDesc
            End Get
        End Property

        Public ReadOnly Property AccountStatus() As enumAccountStatus
            Get
                Return _enumAccountStatus
            End Get
        End Property

        Public ReadOnly Property EnrolmentStatus() As enumEnrolmentStatus
            Get
                Return _enumEnrolmentStatus
            End Get
        End Property

        Public ReadOnly Property AccountStatusCode() As String
            Get
                Select Case Me.AccountStatus
                    Case enumAccountStatus.NotEnrolled
                        Return PCDAccountStatus.NotEnrolled
                    Case enumAccountStatus.Enrolled
                        Return PCDAccountStatus.Enrolled
                    Case enumAccountStatus.Delisted
                        Return PCDAccountStatus.Delisted
                    Case enumAccountStatus.ConnectionFail
                        Return PCDAccountStatus.ConnectionFail
                    Case Else
                        Throw New Exception(String.Format("Invalid Account Status: {0}.", Me.AccountStatus))
                End Select
            End Get
        End Property

        Public ReadOnly Property EnrolmentStatusCode() As String
            Get
                Select Case Me.EnrolmentStatus
                    Case enumEnrolmentStatus.NA
                        Return PCDEnrolmentStatus.NA
                    Case enumEnrolmentStatus.Unprocessed
                        Return PCDEnrolmentStatus.Unprocessed
                    Case enumEnrolmentStatus.Processing
                        Return PCDEnrolmentStatus.Processing
                    Case enumEnrolmentStatus.ConnectionFail
                        Return PCDEnrolmentStatus.ConnectionFail
                    Case Else
                        Throw New Exception(String.Format("Invalid Enrolment Status: {0}.", Me.EnrolmentStatus))
                End Select
            End Get
        End Property

        Public ReadOnly Property ProfID() As String
            Get
                Return _strProfID
            End Get
        End Property

        Public ReadOnly Property IsMultipleProfessional() As Boolean
            Get
                Return _strProfID.Contains(",")
            End Get
        End Property

        ''' <summary>
        ''' Check whether has professional "RMP" in PCD
        ''' </summary>
        ''' <value></value>
        ''' <returns>Return True if has RMP professional, otherwise return false</returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ContainProfessionalRMP() As Boolean
            Get
                Return _strProfID.Contains(ServiceCategoryCode.RMP)
            End Get
        End Property

        Public ReadOnly Property LastCheckTime() As DateTime
            Get
                Return _dtmLastCheckDtm
            End Get
        End Property

        Public ReadOnly Property Exception() As Exception
            Get
                Return _objException
            End Get
        End Property

        Public Property Request() As String
            Get
                Return _strRequest
            End Get
            Set(ByVal value As String)
                _strRequest = value
            End Set
        End Property

        Public ReadOnly Property Result() As String
            Get
                Return _strResult
            End Get
        End Property

#End Region

#Region "Constructors"
        Public Sub New(ByVal strResultXML As String)
            ReadXml(strResultXML, Nothing)
        End Sub

        Public Sub New(ByVal strResultXML As String, ByVal strMessageID As String)
            ReadXml(strResultXML, strMessageID)
        End Sub

        Public Sub New(ByVal enumReturnCode As enumReturnCode, ByVal enumAccountStatus As enumAccountStatus, ByVal enumEnrolmentStatus As enumAccountStatus)
            _enumReturnCode = enumReturnCode
            _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)
            _enumAccountStatus = enumAccountStatus
            _enumEnrolmentStatus = enumEnrolmentStatus
        End Sub

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Sub New(ByVal enumReturnCode As enumReturnCode)
            _enumReturnCode = enumReturnCode
            _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)
            _enumAccountStatus = enumAccountStatus.ConnectionFail
            _enumEnrolmentStatus = enumEnrolmentStatus.ConnectionFail
        End Sub

        Public Sub New(ByVal eReturnCode As enumReturnCode, ByVal objException As Exception)
            _enumReturnCode = eReturnCode
            _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)
            _objException = objException
            _enumAccountStatus = enumAccountStatus.ConnectionFail
            _enumEnrolmentStatus = enumEnrolmentStatus.ConnectionFail
        End Sub

        Public Sub New(ByVal eReturnCode As enumReturnCode, ByVal objException As Exception, ByVal strMessageID As String)
            _enumReturnCode = eReturnCode
            _objException = objException
            _strMessageID = strMessageID
            _enumAccountStatus = enumAccountStatus.ConnectionFail
            _enumEnrolmentStatus = enumEnrolmentStatus.ConnectionFail
        End Sub
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
#End Region

#Region "Supported Functions"
        ''' <summary>
        ''' Read xml result from PCD web service and convert to object
        ''' </summary>
        ''' <param name="strResultXml">Xml result from PCD web service</param>
        ''' <param name="strMessageID">Xml request message id</param>
        ''' <remarks></remarks>
        Private Sub ReadXml(ByVal strResultXml As String, ByVal strMessageID As String)
            Dim ds As DataSet = Nothing
            Dim dt As DataTable = Nothing

            _strResult = strResultXml

            Try
                ds = Common.ComFunction.XmlFunction.Xml2Dataset(strResultXml)

                ' Read return code
                dt = ds.Tables("result")

                ' ----------------------------------------
                ' Message ID
                ' ----------------------------------------
                _strMessageID = GetMessageID(dt.Rows(0))

                If strMessageID IsNot Nothing AndAlso _strMessageID <> strMessageID Then
                    ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    '' Fix missing assign the status when msg id is not matched

                    '_enumReturnCode = enumReturnCode.ErrorAllUnexpected
                    '_returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)
                    'Return

                    Throw New Exception(String.Format("Message ID not matched: Request Msg ID = {0}, Return Msg ID = {1}.", strMessageID, _strMessageID))
                    ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
                End If

                ' ----------------------------------------
                ' Return Code
                ' ----------------------------------------
                Select Case dt.Rows(0)("return_code").ToString()
                    Case "0"
                        _enumReturnCode = enumReturnCode.Success
                    Case "96"
                        _enumReturnCode = enumReturnCode.DataValidationFail
                        _enumAccountStatus = enumAccountStatus.ConnectionFail
                        _enumEnrolmentStatus = enumEnrolmentStatus.ConnectionFail
                    Case "97"
                        _enumReturnCode = enumReturnCode.AuthenticationFailed
                        _enumAccountStatus = enumAccountStatus.ConnectionFail
                        _enumEnrolmentStatus = enumEnrolmentStatus.ConnectionFail
                    Case "98"
                        _enumReturnCode = enumReturnCode.InvalidParameter
                        _enumAccountStatus = enumAccountStatus.ConnectionFail
                        _enumEnrolmentStatus = enumEnrolmentStatus.ConnectionFail
                    Case "99"
                        _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                        _enumAccountStatus = enumAccountStatus.ConnectionFail
                        _enumEnrolmentStatus = enumEnrolmentStatus.ConnectionFail
                    Case Else
                        _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                        _enumAccountStatus = enumAccountStatus.ConnectionFail
                        _enumEnrolmentStatus = enumEnrolmentStatus.ConnectionFail
                End Select

                _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)

                If _enumReturnCode <> enumReturnCode.Success Then
                    Return
                End If
                ' ----------------------------------------
                ' Account Status
                ' ----------------------------------------
                Select Case dt.Rows(0)("account_status").ToString()
                    Case "1"
                        _enumAccountStatus = enumAccountStatus.NotEnrolled
                    Case "2"
                        _enumAccountStatus = enumAccountStatus.Enrolled
                    Case "3"
                        _enumAccountStatus = enumAccountStatus.Delisted
                    Case Else
                        Throw New Exception(String.Format("Invalid Account Status: {0}.", dt.Rows(0)("account_status").ToString()))
                End Select

                ' ----------------------------------------
                ' Enrolment Status
                ' ----------------------------------------
                Select Case dt.Rows(0)("enrolment_status").ToString()
                    Case "1"
                        _enumEnrolmentStatus = enumEnrolmentStatus.Unprocessed
                    Case "2"
                        _enumEnrolmentStatus = enumEnrolmentStatus.Processing
                    Case "3"
                        _enumEnrolmentStatus = enumEnrolmentStatus.NA
                    Case Else
                        Throw New Exception(String.Format("Invalid Enrolment Status: {0}.", dt.Rows(0)("enrolment_status").ToString()))
                End Select

                ' ----------------------------------------
                ' PCD Profession
                ' ----------------------------------------
                _strProfID = dt.Rows(0)("prof_id").ToString()
                _strProfID = ConvertProfID(_strProfID)


                ' ----------------------------------------
                ' Last Check Time
                ' ----------------------------------------
                ' Convert record_creation_dtm to datetime object
                If Not DateTime.TryParseExact(dt.Rows(0)("message_datetime").ToString(), "dd/MM/yyyy HH:mm:ss", New CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, _dtmLastCheckDtm) Then
                    Throw New Exception(String.Format("Invalid Last Check Time: {0}.", dt.Rows(0)("enrolment_status").ToString()))

                End If

            Catch e As System.Net.WebException
                _enumReturnCode = enumReturnCode.CommunicationLinkError
                _enumAccountStatus = enumAccountStatus.ConnectionFail
                _enumEnrolmentStatus = enumEnrolmentStatus.ConnectionFail
                _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)
                _objException = e

            Catch e As System.Xml.XmlException
                _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                _enumAccountStatus = enumAccountStatus.ConnectionFail
                _enumEnrolmentStatus = enumEnrolmentStatus.ConnectionFail
                _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)
                _objException = e

            Catch e As Exception
                _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                _enumAccountStatus = enumAccountStatus.ConnectionFail
                _enumEnrolmentStatus = enumEnrolmentStatus.ConnectionFail
                _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)
                _objException = e

            End Try

        End Sub

        ''' <summary>
        ''' CRE10-035
        ''' Retrieve the message_id from result XML
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetMessageID(ByVal dr As DataRow) As String
            If dr.Table.Columns.Contains("message_id") Then
                If dr("message_id") <> "" Then
                    Return dr("message_id")
                End If
            End If

            Return String.Empty
        End Function

        Private Sub ThrowException(ByVal strMessage As String)
            Throw New Exception(strMessage)
        End Sub

        Public Overloads Function GetReturnCodeDesc(ByVal enumN As enumReturnCode) As String
            MyBase.GenReturnCodeSystemMessage(EnumConstant.EnumMappingCodeType.WS_PCD_CheckAccountStatus_Return_Code, enumN)
            Return Me.SystemMessage.GetMessage()

        End Function

        Public Function GetAccountStatus() As String
            Select Case Me.AccountStatus
                Case enumAccountStatus.Enrolled
                    Return PCDAccountStatus.Enrolled

                Case enumAccountStatus.NotEnrolled
                    Return PCDAccountStatus.NotEnrolled

                Case enumAccountStatus.Delisted
                    Return PCDAccountStatus.Delisted

                Case Else
                    Throw New Exception(String.Format("Invalid Account Status: {0}.", Me.AccountStatus))

            End Select

        End Function

        Public Function GetEnrolmentStatus() As String
            Select Case Me.EnrolmentStatus
                Case enumEnrolmentStatus.Processing
                    Return PCDEnrolmentStatus.Processing

                Case enumEnrolmentStatus.Unprocessed
                    Return PCDEnrolmentStatus.Unprocessed

                Case enumEnrolmentStatus.NA
                    Return PCDEnrolmentStatus.NA

                Case Else
                    Throw New Exception(String.Format("Invalid Enrolment Status: {0}.", Me.EnrolmentStatus))

            End Select

        End Function

        Private Shared Function GetAccountStatusDesc(ByVal strAccountStatusCode As String) As String
            Dim strDescription As String = String.Empty

            Status.GetDescriptionFromDBCode("PCD_AccountStatus", strAccountStatusCode, strDescription, String.Empty)

            Return strDescription
        End Function

        Private Shared Function GetEnrolmentStatusDesc(ByVal strEnrolmentStatusCode As String) As String
            Dim strDescription As String = String.Empty

            Status.GetDescriptionFromDBCode("PCD_EnrolmentStatus", strEnrolmentStatusCode, strDescription, String.Empty)

            Return strDescription
        End Function

        ''' <summary>
        ''' Get PCD Status for displaying in UI
        ''' </summary>
        ''' <returns>e.g. Enrolled (Processing), Delisted (Unprocessed), Failed to connect PCD</returns>
        ''' <remarks></remarks>
        Public Function GetPCDStatusDesc() As String
            Dim strDescription As String = String.Empty

            Select Case AccountStatus
                Case enumAccountStatus.ConnectionFail
                    ' Failed to connect PCD
                    Return GetAccountStatusDesc(Me.AccountStatusCode)
                Case Else

                    Select Case EnrolmentStatus
                        Case enumEnrolmentStatus.NA
                            ' e.g. Enrolled
                            Return String.Format("{0}", GetAccountStatusDesc(Me.AccountStatusCode))

                        Case enumEnrolmentStatus.Processing, enumEnrolmentStatus.Unprocessed
                            ' e.g. Enrolled (Processing)
                            Return String.Format("{0} ({1})", GetAccountStatusDesc(Me.AccountStatusCode), GetEnrolmentStatusDesc(Me.EnrolmentStatusCode))

                    End Select

            End Select

            Return strDescription
        End Function

        ''' <summary>
        ''' Get PCD Status for displaying in UI
        ''' </summary>
        ''' <param name="strAccountStatusCode">N, E, D, C</param>
        ''' <param name="strEnrolmentStatusCode">N, U, P, C</param>
        ''' <returns>e.g. Enrolled (Processing), Delisted (Unprocessed), Failed to connect PCD</returns>
        ''' <remarks></remarks>
        Public Shared Function GetPCDStatusDescByValue(ByVal strAccountStatusCode As String, ByVal strEnrolmentStatusCode As String) As String
            Dim strOutputText As String = String.Empty

            If Not String.IsNullOrEmpty(strAccountStatusCode) Then
                Select Case strAccountStatusCode
                    Case PCDAccountStatus.ConnectionFail
                        strOutputText = GetAccountStatusDesc(strAccountStatusCode)

                    Case Else
                        Select Case strEnrolmentStatusCode
                            Case PCDEnrolmentStatus.NA
                                ' e.g. Enrolled
                                strOutputText = String.Format("{0}", GetAccountStatusDesc(strAccountStatusCode))

                            Case PCDEnrolmentStatus.Processing, PCDEnrolmentStatus.Unprocessed
                                ' e.g. Enrolled (Processing)
                                strOutputText = String.Format("{0} ({1})", GetAccountStatusDesc(strAccountStatusCode), GetEnrolmentStatusDesc(strEnrolmentStatusCode))

                        End Select

                End Select
            Else
                strOutputText = HttpContext.GetGlobalResourceObject("Text", "Unknown", New System.Globalization.CultureInfo(CultureLanguage.English))
            End If

            Return strOutputText
        End Function

        Public Function GetPCDProfessionalDesc() As String
            Return GetPCDProfessionalDescByValue(ProfID)
        End Function

        ''' <summary>
        ''' Convert PCD prof_id for UI display,
        ''' Convert "RCM,RDT,RMP" to "RCM, RDT, RMP", 
        ''' Convert "" to "-" 
        ''' </summary>
        ''' <param name="strProfID">e.g. RCM,RDT,RMP</param>
        ''' <returns>e.g. RCM, RDT, RMP</returns>
        ''' <remarks></remarks>
        Public Shared Function GetPCDProfessionalDescByValue(ByVal strProfID As String) As String
            If strProfID.Trim = String.Empty Then
                Return HttpContext.GetGlobalResourceObject("Text", "PCDProfessionalEmpty", New System.Globalization.CultureInfo(CultureLanguage.English))
            Else
                Return strProfID.Replace(",", ", ")
            End If
        End Function

        Public Function UpdateJoinPCDStatus(ByVal strSPID As String, ByVal strUpdateBy As String, ByRef strMessage As String, Optional ByRef udtSPModel As ServiceProviderModel = Nothing) As Boolean
            Dim udtServiceProviderBLL As New ServiceProviderBLL
            Dim blnRes As Boolean = False

            Select Case Me.GetAccountStatus
                Case PCDAccountStatus.Enrolled, _
                    PCDAccountStatus.NotEnrolled, _
                    PCDAccountStatus.Delisted

                    'Get the Latest SP's TSMP
                    Dim byteTSMP As Byte() = udtServiceProviderBLL.GetserviceProviderPermanentTSMP(strSPID, New Database)

                    ' Update Service Provider's Join PCD Status
                    If udtServiceProviderBLL.UpdateServiceProviderJoinPCDStatus(strSPID, Me.GetAccountStatus, Me.GetEnrolmentStatus, Me.ProfID, Me.LastCheckTime, strUpdateBy, byteTSMP) Then
                        strMessage = String.Format("Success, SPID({0}) updated PCDStatus.", strSPID)

                        If Not udtSPModel Is Nothing Then
                            udtSPModel.PCDAccountStatus = Me.GetAccountStatus
                            udtSPModel.PCDEnrolmentStatus = Me.GetEnrolmentStatus
                            udtSPModel.PCDProfessional = Me.ProfID
                            udtSPModel.PCDStatusLastCheckDtm = Me.LastCheckTime
                        End If

                        blnRes = True
                    Else
                        strMessage = String.Format("Fail, SPID({0}) is not updated PCDStatus.", strSPID)
                    End If

                Case Else
                    'Nothing to do

            End Select

            Return blnRes

        End Function

        ''' <summary>
        ''' Convert PCD professional code to EHS professional code
        ''' e.g. CMP to RCM
        ''' </summary>
        ''' <param name="strProfID">CMP, RDT, RMP</param>
        ''' <returns>RCM, RDT, RMP</returns>
        ''' <remarks></remarks>
        Private Function ConvertProfID(ByVal strProfID As String) As String
            If strProfID.Trim = String.Empty Then
                Return String.Empty
            End If

            ' Initial CodeMapping
            Dim udtCodeMapList As CodeMappingCollection
            Dim udtCodeMap As CodeMappingModel
            udtCodeMapList = CodeMappingBLL.GetAllCodeMapping

            Dim arrProfID As String() = strProfID.Split(",")

            For i As Integer = 0 To (arrProfID.Length - 1)
                udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.PCD, CodeMappingModel.EnumTargetSystem.EHS, CodeMappingModel.EnumCodeType.Service_Category_Code.ToString, arrProfID(i))
                If Not udtCodeMap Is Nothing Then
                    arrProfID(i) = udtCodeMap.CodeTarget
                End If
            Next

            Return String.Join(",", arrProfID)
        End Function


        ''' <summary>
        ''' Check PCD Professional Status
        ''' </summary>
        ''' <returns>The warning message</returns>
        ''' <remarks></remarks>
        Public Function CheckPCDProfessional() As String
            Dim blnShowWarning As Boolean = False
            Dim strMessage As String = String.Empty

            '| PCD enrolled    |   PCD enrolment  |    Warning                                         |
            '| ----------------|------------------|----------------------------------------------------|
            '| -               |   Unprocessed    |    No warning                                      |
            '| -               |   Processing     |    Warning for all except single professional MP   |
            '| Enrolled	       |   -	          |    Prof without MP                                 |
            '| Enrolled	       |   With enrolment |    Prof. with non MP                               |

            Select Case Me.AccountStatusCode
                Case PCDAccountStatus.NotEnrolled, PCDAccountStatus.Delisted
                    Select Case Me.GetEnrolmentStatus
                        Case PCDEnrolmentStatus.Unprocessed, PCDEnrolmentStatus.NA
                            blnShowWarning = False

                        Case PCDEnrolmentStatus.Processing
                            blnShowWarning = True
                    End Select

                Case PCDAccountStatus.Enrolled
                    Select Case Me.GetEnrolmentStatus
                        Case PCDEnrolmentStatus.NA
                            ' Enrolled
                            If Not ContainProfessionalRMP Then
                                blnShowWarning = True
                            End If

                        Case PCDEnrolmentStatus.Processing, PCDEnrolmentStatus.Unprocessed
                            ' With Enrolment
                            blnShowWarning = True
                    End Select

                Case PCDAccountStatus.ConnectionFail
                    ' Do Nothing
            End Select

            ' No warning for Single RMP Profession
            If Not Me.IsMultipleProfessional AndAlso Me.ContainProfessionalRMP Then
                blnShowWarning = False
            End If

            If blnShowWarning Then
                If Me.ContainProfessionalRMP Then
                    ' Message: The service provider has multiple professional in PCD
                    strMessage = HttpContext.GetGlobalResourceObject("Text", "PCDProfession_WithRMP_Message")
                Else
                    ' Message: The service provider has no enrolment of professional of Registered Medical Practitioners in PCD
                    strMessage = HttpContext.GetGlobalResourceObject("Text", "PCDProfession_NoRMP_Message")
                End If
            End If

            Return strMessage
        End Function
#End Region

    End Class

End Namespace

