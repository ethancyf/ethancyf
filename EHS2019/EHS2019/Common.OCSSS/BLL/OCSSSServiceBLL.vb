Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.IO
Imports System.Text
Imports Common.ComFunction
Imports Common.Component
Imports Common.DataAccess
Imports System.Data.SqlClient

Public Class OCSSSServiceBLL

#Region "Constants"
    Private Const CONST_SYS_PARAM_TurnOnOCSSS As String = "TurnOnOCSSS"
    Private Const CONST_SYS_PARAM_OCSSSStartDate As String = "OCSSSStartDate"
    Private Const CONST_SYS_PARAM_OCSSS_Scheme As String = "OCSSS_Scheme"
    Private Const CONST_SYS_PARAM_OCSSS_WS_Timeout As String = "OCSSS_WS_Timeout"
    Private Const CONST_SYS_PARAM_OCSSS_WS_RetryTurnOn As String = "OCSSS_WS_RetryTurnOn"
    Private Const CONST_SYS_PARAM_OCSSS_WS_Link As String = "OCSSS_WS_Link"
    Private Const CONST_SYS_PARAM_OCSSS_WS_PassPhrase As String = "OCSSS_WS_PassPhrase"

    Private Const CONST_SYS_PARAM_OCSSS_WS_DemoFilePath As String = "OCSSS_WS_DemoFilePath"
    Private Const CONST_SYS_PARAM_OCSSS_WS_DemoTurnOn As String = "OCSSS_WS_DemoTurnOn"

    Private Const CONST_DUMMY_HKIC_FOR_HEALTHCHECK As String = "DUMMYHKIC"

#End Region


    ''' <summary>
    ''' Check validty of stay by HKIC no. (For residential status is C or U only) - Full / Text-only Version
    ''' </summary>
    ''' <param name="strHKID">HKIC no. on HKID card</param>
    ''' <param name="strHKICSymbol">Inputted HKIC Symbol (A,C,R,U)</param>
    ''' <param name="strSPID">The logined SP</param>
    ''' <param name="strSchemeCode">Scheme to be claimed</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsEligible(ByVal strHKID As String, ByVal strHKICSymbol As String, ByVal strSPID As String, ByVal strSchemeCode As String) As OCSSSResult
        Return IsEligible(New OCSSSSetting(), strHKID, strHKICSymbol, strSPID, strSchemeCode, String.Empty)
    End Function

    ''' <summary>
    ''' Check validty of stay by HKIC no. (For residential status is C or U only) - IVRS
    ''' </summary>
    ''' <param name="strHKID">HKIC no. on HKID card</param>
    ''' <param name="strHKICSymbol">Inputted HKIC Symbol (A,C,R,U)</param>
    ''' <param name="strSPID">The logined SP</param>
    ''' <param name="strSchemeCode">Scheme to be claimed</param>
    ''' <param name="strIVRSUniqueID">IVRS Unique ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsEligible(ByVal strHKID As String, ByVal strHKICSymbol As String, ByVal strSPID As String, ByVal strSchemeCode As String, ByVal strIVRSUniqueID As String) As OCSSSResult
        Return IsEligible(New OCSSSSetting(), strHKID, strHKICSymbol, strSPID, strSchemeCode, strIVRSUniqueID)
    End Function

    ''' <summary>
    ''' Check validty of stay by HKIC no. (For residential status is C or U only)
    ''' </summary>
    ''' <param name="objOCSSSSetting">Stored OCSSS setting</param>
    ''' <param name="strHKID">HKIC no. on HKID card</param>
    ''' <param name="strHKICSymbol">Inputted HKIC Symbol (A,C,R,U)</param>
    ''' <param name="strSPID">The logined SP</param>
    ''' <param name="strSchemeCode">Scheme to be claimed</param>
    ''' <param name="strIVRSUniqueID">IVRS Unique ID</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsEligible(ByVal objOCSSSSetting As OCSSSSetting, _
                                ByVal strHKID As String, _
                                ByVal strHKICSymbol As String, _
                                ByVal strSPID As String, _
                                ByVal strSchemeCode As String, _
                                ByVal strIVRSUniqueID As String) As OCSSSResult

        Dim objOcsssResult As OCSSSResult = Nothing
        Dim objeligibilityRequest As New eligibilityRequest
        objeligibilityRequest.patientId = RTrim(strHKID)

        Select Case strHKICSymbol
            ' [CRE18-020] (HKIC Symbol Others) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Case "A", "R", "O"
                ' [CRE18-020] (HKIC Symbol Others) [End][Winnie]
                objOcsssResult = New OCSSSResult(OCSSSResult.OCSSSConnection.SkipForChecking, Nothing)

            Case "C", "U"
                ' First call
                objOcsssResult = CallOCSSS(objOCSSSSetting, objeligibilityRequest, False, FunctCode.FUNT060401)

                ' Write OCSSS Result to DB
                If objOCSSSSetting.TurnOnOcsssCheckResultLog Then
                    Me.InsertOCSSSCheckResult(strHKID, strHKICSymbol, strSPID, strSchemeCode, objOcsssResult.OCSSSStatus, strIVRSUniqueID)
                End If

                ' Retry Call
                If objOcsssResult.ConnectionStatus = OCSSSResult.OCSSSConnection.Fail Then
                    If objOCSSSSetting.OcsssRetryTurnOn Then
                        objOcsssResult = CallOCSSS(objOCSSSSetting, objeligibilityRequest, True, FunctCode.FUNT060401)

                        ' Write OCSSS Result to DB
                        If objOCSSSSetting.TurnOnOcsssCheckResultLog Then
                            Me.InsertOCSSSCheckResult(strHKID, strHKICSymbol, strSPID, strSchemeCode, objOcsssResult.OCSSSStatus, strIVRSUniqueID)
                        End If

                    End If
                End If
        End Select

        Return objOcsssResult
    End Function

    ''' <summary>
    ''' For interface control function to perform healh check to OCSSS Web Service 
    ''' (If system parameters "TurnOnOCSSS" is not equal to "Y", checking will be skipped)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function HealthCheck() As OCSSSResult
        Dim strHKID As String = CONST_DUMMY_HKIC_FOR_HEALTHCHECK ' Dummy HKIC no. for health check
        Return CallOCSSSInternalUse(New OCSSSSetting(), strHKID)
    End Function

    ''' <summary>
    ''' For interface control function to perform healh check to OCSSS Web Service 
    ''' (Ignore system parameters, call OCSSS directly)
    ''' </summary>
    ''' <param name="strOcsssURL">Specify OCSSS web service URL</param>
    ''' <param name="strOcsssPassPhrase">Specify OCSSS Pass Phrase</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function HealthCheck(ByVal strOcsssURL As String, ByVal strOcsssPassPhrase As String) As OCSSSResult
        Dim strHKID As String = CONST_DUMMY_HKIC_FOR_HEALTHCHECK ' Dummy HKIC no. for health check
        Return CallOCSSSInternalUse(New OCSSSSetting(strOcsssURL, strOcsssPassPhrase), strHKID)
    End Function

    ''' <summary>
    ''' For interface control function to check validty of stay by HKIC no. (For residential status is C or U only)
    ''' (Ignore system parameters, call OCSSS directly)
    ''' </summary>    
    ''' <param name="strOcsssURL">Specify OCSSS web service URL</param>
    ''' <param name="strOcsssPassPhrase">Specify OCSSS Pass Phrase</param>
    ''' <param name="strHKID">HKIC no. on HKID card</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsEligibleForInternalUse(ByVal strOcsssURL As String, ByVal strOcsssPassPhrase As String, ByVal strHKID As String) As OCSSSResult
        Return CallOCSSSInternalUse(New OCSSSSetting(strOcsssURL, strOcsssPassPhrase), strHKID)
    End Function

    ''' <summary>
    ''' Perform OCSSS Web Service 
    ''' (If system parameters "TurnOnOCSSS" is not equal to "Y", checking will be skipped)
    ''' </summary>
    ''' <param name="objOCSSSSetting">Stored OCSSS setting</param>
    ''' <param name="strHKID">HKIC no. on HKID card</param>    
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CallOCSSSInternalUse(ByVal objOCSSSSetting As OCSSSSetting, ByVal strHKID As String) As OCSSSResult
        Dim objOcsssResult As OCSSSResult = Nothing
        Dim objeligibilityRequest As New eligibilityRequest

        objeligibilityRequest.patientId = strHKID
        objOcsssResult = CallOCSSS(objOCSSSSetting, objeligibilityRequest, False, FunctCode.FUNT060402)

        Return objOcsssResult
    End Function

    Private Function CallOCSSS(ByVal objOCSSSSetting As OCSSSSetting, ByVal objeligibilityRequest As eligibilityRequest, ByVal blnRetry As Boolean, ByVal strFunctionCode As String) As OCSSSResult
        ' Initial audit log 
        Dim strDBFlag As String = DBFlagStr.DBFlagInterfaceLog
        Dim udtAuditLog As AuditLogBase
        Dim blnIsHealthCheck As Boolean = False

        udtAuditLog = New AuditLogBase(strFunctionCode, strDBFlag)

        If objeligibilityRequest.patientId = CONST_DUMMY_HKIC_FOR_HEALTHCHECK Then
            blnIsHealthCheck = True
        Else
            blnIsHealthCheck = False
        End If

        Dim objOcsssResult As OCSSSResult = Nothing
        Dim enumTurnOnOcsss As OCSSSSetting.ConfigTurnOnOcsss = objOCSSSSetting.TurnOnOcsss

        Select Case enumTurnOnOcsss
            Case OCSSSSetting.ConfigTurnOnOcsss.TurnOn

                Dim strRequestXml As String = XmlFunction.ConvertObjectToXML(objeligibilityRequest)

                udtAuditLog.AddDescripton("HKID", objeligibilityRequest.patientId)
                udtAuditLog.AddDescripton("Retry", IIf(blnRetry, "Y", "N"))
                udtAuditLog.WriteStartLogData(LogID.LOG00001, "[EHS>OCSSS] Send request start", strRequestXml)

                Dim objeligibilityResponse As eligibilityResponse = Nothing
                Dim strResponseXml As String = String.Empty

                Try
                    ' Call OCSSS
                    If objOCSSSSetting.OcsssDemoTurnOn Then
                        objeligibilityResponse = CheckDemoData(objeligibilityRequest.patientId)
                    Else
                        objeligibilityResponse = CreateOcsssProxy(objOCSSSSetting).isEligible(objeligibilityRequest)
                    End If

                    ' Call OCSSS Success
                    strResponseXml = XmlFunction.ConvertObjectToXML(objeligibilityResponse)

                    objOcsssResult = New OCSSSResult(OCSSSResult.OCSSSConnection.Success, objeligibilityResponse)

                    If objeligibilityResponse.checkingResult = "E" Then
                        ' OCSSS return error
                        Throw New Exception(String.Format("OCSSS returns error. checkingResult={0}, errorMessage={1}",
                                                          objeligibilityResponse.checkingResult,
                                                          objeligibilityResponse.errorMessage))
                    End If

                    udtAuditLog.AddDescripton("HKID", objeligibilityRequest.patientId)
                    udtAuditLog.AddDescripton("checkingResult", objeligibilityResponse.checkingResult)
                    udtAuditLog.AddDescripton("errorMessage", objeligibilityResponse.errorMessage)
                    udtAuditLog.AddDescripton("messageId", objeligibilityResponse.messageId)
                    udtAuditLog.AddDescripton("replyDateTime", objeligibilityResponse.replyDateTime)
                    udtAuditLog.AddDescripton("HealthCheck", IIf(blnIsHealthCheck, "Y", "N"))
                    udtAuditLog.AddDescripton("Retry", IIf(blnRetry, "Y", "N"))
                    udtAuditLog.WriteEndLogData(LogID.LOG00002, "[EHS>OCSSS] Receive response success", strResponseXml)

                    Return objOcsssResult

                Catch ex As Exception
                    ' Call OCSSS failed
                    udtAuditLog.WriteSystemLog(ex, strFunctionCode, String.Empty)
                    udtAuditLog.AddDescripton("HKID", objeligibilityRequest.patientId)
                    udtAuditLog.AddDescripton("HealthCheck", IIf(blnIsHealthCheck, "Y", "N"))
                    udtAuditLog.AddDescripton("Retry", IIf(blnRetry, "Y", "N"))
                    udtAuditLog.WriteEndLogData(LogID.LOG00003, "[EHS>OCSSS] Receive response fail", strResponseXml)
                    objOcsssResult = New OCSSSResult(OCSSSResult.OCSSSConnection.Fail, Nothing, ex)
                    Return objOcsssResult
                End Try

            Case OCSSSSetting.ConfigTurnOnOcsss.TurnOff
                objOcsssResult = New OCSSSResult(OCSSSResult.OCSSSConnection.TurnOff, Nothing)
            Case OCSSSSetting.ConfigTurnOnOcsss.BeforeEffectiveDate
                objOcsssResult = New OCSSSResult(OCSSSResult.OCSSSConnection.TurnOff, Nothing)
            Case Else
                Throw New Exception(String.Format("Common.OCSSS: Unhandled enumTurnOnOcsss' {0}", enumTurnOnOcsss.ToString))
        End Select

        Return objOcsssResult
    End Function

    Private Function CreateOcsssProxy(ByVal objOCSSSSetting As OCSSSSetting) As OCSSSProxy
        System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        Dim oProxy As New OCSSSProxy
        oProxy.Timeout = objOCSSSSetting.OcsssTimeout
        oProxy.Url = objOCSSSSetting.OcsssURL
        oProxy.HashedPassPhrase = objOCSSSSetting.OcsssPassPhrase

        Return oProxy
    End Function

    Private Function ValidateRemoteCertificate(sender As Object, certification As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function

    ''' <summary>
    ''' Check whether enable HKIC symbol input for the scheme (e.g. no HKIC symbol input for RVP claim)
    ''' </summary>
    ''' <param name="strSchemeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function EnableHKICSymbolInput(ByVal strSchemeCode As String) As Boolean
        Dim objGenFunc As New GeneralFunction()
        Dim strValue As String = String.Empty

        'Validation input scheme code
        If strSchemeCode Is Nothing Then
            Return False
        End If

        If strSchemeCode = String.Empty Then
            Return False
        End If

        'Check DB scheme code
        objGenFunc.getSystemParameter(CONST_SYS_PARAM_OCSSS_Scheme, strValue, Nothing)

        If strValue = String.Empty Then
            Return False
        End If

        Dim blnMatch As Boolean = False
        If (strValue + ";").Contains(strSchemeCode + ";") Then
            blnMatch = True
        End If

        'Check Start Date
        If blnMatch Then
            Return (objGenFunc.GetSystemDateTime >= OCSSSSetting.GetOcsssStartDate())
        End If

        Return False

    End Function

    ''' <summary>
    ''' Check whether enable HKIC symbol input for the scheme in back office use (e.g. no HKIC symbol input for RVP claim)
    ''' </summary>
    ''' <param name="strSchemeCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function EnableHKICSymbolInputForBackOffice(ByVal strSchemeCode As String, ByVal dtmServiceDate As Date) As Boolean
        Dim objGenFunc As New GeneralFunction()
        Dim strValue As String = String.Empty

        'Check Scheme
        objGenFunc.getSystemParameter(CONST_SYS_PARAM_OCSSS_Scheme, strValue, Nothing)

        If strValue = String.Empty Then
            Return False
        End If

        Dim blnMatch As Boolean = False
        If (strValue + ";").Contains(strSchemeCode + ";") Then
            blnMatch = True
        End If

        'Check Start Date
        If blnMatch Then
            Return (dtmServiceDate >= OCSSSSetting.GetOcsssStartDate())
        End If

        Return False

    End Function

    ''' <summary>
    ''' Check residential status by reading demo data
    ''' </summary>
    ''' <param name="strHKID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CheckDemoData(ByVal strHKID As String) As eligibilityResponse
        Dim strFile As String = OCSSSSetting.GetOcsssDemoFilePath()
        Dim ds As New DataSet
        Dim sr As New StringReader(File.ReadAllText(strFile))
        ds.ReadXml(sr)
        sr.Close()

        Dim objeligibilityResponse As eligibilityResponse
        Dim drs() As DataRow = ds.Tables(0).Select("HKID = '" + Trim(strHKID) + "'")
        If drs.Length > 0 Then
            objeligibilityResponse = New eligibilityResponse
            objeligibilityResponse.checkingResult = drs(0)("checkingResult")
            objeligibilityResponse.errorMessage = drs(0)("errorMessage")
            objeligibilityResponse.messageId = "DEMO"
            objeligibilityResponse.replyDateTime = "DEMO"
        Else
            objeligibilityResponse = New eligibilityResponse
            objeligibilityResponse.checkingResult = "N" ' Default invalid
            objeligibilityResponse.errorMessage = String.Empty
            objeligibilityResponse.messageId = "DEMO"
            objeligibilityResponse.replyDateTime = "DEMO"
        End If

        If objeligibilityResponse.errorMessage <> String.Empty Then
            Throw New Exception(objeligibilityResponse.errorMessage)
        End If

        Return objeligibilityResponse
    End Function

    Private Sub InsertOCSSSCheckResult(ByVal strHKID As String, ByVal strHKICSymbol As String, ByVal strSPID As String, ByVal strSchemeCode As String, _
                                            ByVal strOCSSSResult As String, ByVal strIVRSUniqueID As String, Optional ByVal udtDB As Database = Nothing)

        If udtDB Is Nothing Then udtDB = New Database()

        Try
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@IdentityNum", SqlDbType.Char, 9, strHKID), _
                udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, strSchemeCode), _
                udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                udtDB.MakeInParam("@HKIC_Symbol", SqlDbType.Char, 1, strHKICSymbol), _
                udtDB.MakeInParam("@OCSSS_Ref_Status", SqlDbType.Char, 1, strOCSSSResult), _
                udtDB.MakeInParam("@IVRS_Unique_ID", SqlDbType.VarChar, 40, IIf(strIVRSUniqueID = String.Empty, DBNull.Value, strIVRSUniqueID))
            }

            udtDB.RunProc("proc_OCSSSCheckResult_add", prams)


        Catch ex As Exception
            Try
                udtDB.Close()
            Catch ex2 As Exception
                'Do nothing
            End Try

            Throw
        End Try

    End Sub

    Public Function GetOCSSSCheckResultByIVRSUniqueID(ByVal strHKID As String, ByVal strHKICSymbol As String, _
                                                  ByVal strIVRSUniqueID As String, Optional ByVal udtDB As Database = Nothing) As String

        If udtDB Is Nothing Then udtDB = New Database()

        Dim dt As DataTable = New DataTable()
        Dim strRes As String = String.Empty

        Try
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@IdentityNum", SqlDbType.Char, 9, strHKID), _
                udtDB.MakeInParam("@HKIC_Symbol", SqlDbType.Char, 1, strHKICSymbol), _
                udtDB.MakeInParam("@IVRS_Unique_ID", SqlDbType.VarChar, 40, IIf(strIVRSUniqueID = String.Empty, DBNull.Value, strIVRSUniqueID))
            }


            udtDB.RunProc("proc_OCSSSCheckResult_get_ByIVRSUniqueID", prams, dt)

            If dt.Rows().Count > 0 Then
                strRes = dt.Rows(0).Item("OCSSS_Ref_Status")
            End If

        Catch ex As Exception
            Try
                udtDB.Close()
            Catch ex2 As Exception
                'Do nothing
            End Try

            Throw
        End Try

        Return strRes

    End Function


    ''' <summary>
    ''' Setting class for connecting OCSSS
    ''' </summary>
    ''' <remarks></remarks>
    Private Class OCSSSSetting

        Public Enum ConfigTurnOnOcsss
            TurnOn
            TurnOff
            BeforeEffectiveDate
        End Enum

        Private _TurnOnOcsssCheckResultLog As Boolean
        Public ReadOnly Property TurnOnOcsssCheckResultLog As Boolean
            Get
                Return _TurnOnOcsssCheckResultLog
            End Get
        End Property

        Private _TurnOnOcsss As ConfigTurnOnOcsss
        Public ReadOnly Property TurnOnOcsss As ConfigTurnOnOcsss
            Get
                Return _TurnOnOcsss
            End Get
        End Property

        Private _OcsssRetryTurnOn As Boolean
        Public ReadOnly Property OcsssRetryTurnOn As Boolean
            Get
                Return _OcsssRetryTurnOn
            End Get
        End Property

        Private _OcsssPassPhrase As String
        Public ReadOnly Property OcsssPassPhrase As String
            Get
                Return _OcsssPassPhrase
            End Get
        End Property

        Private _OcsssURL As String
        Public ReadOnly Property OcsssURL As String
            Get
                Return _OcsssURL
            End Get
        End Property

        Private _OcsssStartDate As DateTime
        Public ReadOnly Property OcsssStartDate As DateTime
            Get
                Return _OcsssStartDate
            End Get
        End Property

        Private _OcsssDemoTurnOn As Boolean
        Public ReadOnly Property OcsssDemoTurnOn As Boolean
            Get
                Return _OcsssDemoTurnOn
            End Get
        End Property

        Private _OcsssTimeout As Integer
        Public ReadOnly Property OcsssTimeout As Integer
            Get
                Return _OcsssTimeout
            End Get
        End Property


        ''' <summary>
        ''' Use setting in System Parmaters
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
            _TurnOnOcsssCheckResultLog = True
            _TurnOnOcsss = GetTurnOnOcsss()
            _OcsssRetryTurnOn = GetOcsssRetryTurnOn()
            _OcsssPassPhrase = GetOcsssPassPhrase()
            _OcsssURL = GetOcsssURL()
            _OcsssStartDate = GetOcsssStartDate()
            _OcsssDemoTurnOn = GetOcsssDemoTurnOn()
            _OcsssTimeout = GetOcsssTimeout()
        End Sub

        ''' <summary>
        ''' Use specific setting for test OCSSS connection [force connect OCSSS, no retry]
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByVal strOcsssURL As String, ByVal strOcsssPassPhrase As String)
            _TurnOnOcsssCheckResultLog = False
            _TurnOnOcsss = ConfigTurnOnOcsss.TurnOn
            _OcsssRetryTurnOn = False
            _OcsssPassPhrase = strOcsssPassPhrase
            _OcsssURL = strOcsssURL
            _OcsssStartDate = New Date(2018, 1, 1)
            _OcsssDemoTurnOn = False
            _OcsssTimeout = GetOcsssTimeout()
        End Sub

        Private Function GetTurnOnOcsss() As ConfigTurnOnOcsss
            Dim objGenFunc As New GeneralFunction()
            Dim strValue As String = String.Empty
            objGenFunc.getSystemParameter(CONST_SYS_PARAM_TurnOnOCSSS, strValue, Nothing)

            If (New GeneralFunction()).GetSystemDateTime >= GetOcsssStartDate() Then
                Select Case strValue.ToUpper
                    Case "Y"
                        Return ConfigTurnOnOcsss.TurnOn
                    Case "N"
                        Return ConfigTurnOnOcsss.TurnOff
                    Case Else
                        Throw New Exception(String.Format("Common.OCSSS: Unknown system parameter 'TurnOnOcsss' {0}", ""))
                End Select
            Else
                Return ConfigTurnOnOcsss.BeforeEffectiveDate
            End If

        End Function

        Private Function GetOcsssRetryTurnOn() As Boolean
            Dim objGenFunc As New GeneralFunction()
            Dim strValue As String = String.Empty
            objGenFunc.getSystemParameter(CONST_SYS_PARAM_OCSSS_WS_RetryTurnOn, strValue, Nothing)
            Return IIf(Trim(strValue).ToUpper = "Y", True, False)
        End Function

        Private Function GetOcsssPassPhrase() As String
            Dim objGenFunc As New GeneralFunction()
            Dim strValue As String = String.Empty
            objGenFunc.getSystemParameter(CONST_SYS_PARAM_OCSSS_WS_PassPhrase, strValue, Nothing)
            Return strValue
        End Function

        Private Function GetOcsssURL() As String
            Dim objGenFunc As New GeneralFunction()
            Dim strValue As String = String.Empty
            objGenFunc.getSystemParameter(CONST_SYS_PARAM_OCSSS_WS_Link, strValue, Nothing)
            Return strValue
        End Function

        Private Function GetOcsssTimeout() As Integer
            Dim objGenFunc As New GeneralFunction()
            Dim strValue As String = String.Empty
            objGenFunc.getSystemParameter(CONST_SYS_PARAM_OCSSS_WS_Timeout, strValue, Nothing)
            Return CInt(strValue)
        End Function

        Public Shared Function GetOcsssStartDate() As DateTime
            Dim objGenFunc As New GeneralFunction()
            Dim strValue As String = String.Empty
            objGenFunc.getSystemParameter(CONST_SYS_PARAM_OCSSSStartDate, strValue, Nothing)
            Return Convert.ToDateTime(strValue)
        End Function

        Public Shared Function GetOcsssDemoFilePath() As String
            Dim objGenFunc As New GeneralFunction()
            Dim strValue As String = String.Empty
            objGenFunc.getSystemParameter(CONST_SYS_PARAM_OCSSS_WS_DemoFilePath, strValue, Nothing)
            Return strValue
        End Function

        Private Function GetOcsssDemoTurnOn() As Boolean
            Dim objGenFunc As New GeneralFunction()
            Dim strValue As String = String.Empty
            objGenFunc.getSystemParameter(CONST_SYS_PARAM_OCSSS_WS_DemoTurnOn, strValue, Nothing)
            Return IIf(Trim(strValue).ToUpper = "Y", True, False)
        End Function
    End Class
End Class
