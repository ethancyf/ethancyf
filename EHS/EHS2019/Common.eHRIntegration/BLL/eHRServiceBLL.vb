Imports System.Configuration
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Text.RegularExpressions
Imports System.Web.Services.Protocols
Imports System.Xml
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.DataAccess
Imports Common.eHRIntegration.DAL
Imports Common.eHRIntegration.Dummy
Imports Common.eHRIntegration.Model.Xml.eHRService

Namespace BLL

    Public Class eHRServiceBLL

#Region "Constants"

        Public Class Constant
            Public Const EHSUPDATEBY As String = "eHS"
        End Class

#End Region

#Region "Enum"

        ' Note: For simplicity, this field will be also used for the audit log field FunctionName, with manually removing the word "Result"
        Public Enum enumEhrFunctionResult
            NA
            geteHRSSTokenInfoResult
            seteHRSSTokenSharedResult
            replaceeHRSSTokenResult
            notifyeHRSSTokenDeactivatedResult
            geteHRSSLoginAliasResult
            healthCheckeHRSSResult
        End Enum

        Public Enum enumEhrIntegrationInterfaceQueueType
            NA
            SETSHARE
            REPLACETOKEN
            DEACTIVATETOKEN
        End Enum

        Public Enum enumResultStatus
            NA
            R10000 ' Success (for VP)
            R70000 ' Success
            R79003 ' Parse data failure
            R79004 ' Build data xml failure
            R79999 ' Unexpected Failure
        End Enum

#End Region

#Region "Model"

        Public Class VerificationPassModel
            Public VerificationPassValue As String
            Public GetFromEHR As Boolean
        End Class

#End Region

#Region "Constructors"

        Public Sub New()
            Dim udtGeneralFunction As New GeneralFunction

            _strMode = udtGeneralFunction.GetSystemParameterParmValue1("eHRSS_Mode")
            _strPrimarySite = udtGeneralFunction.GetSystemVariableValue("eHRSS_PrimarySite")
            _strVPLink = udtGeneralFunction.GetSystemParameterParmValue1(String.Format("eHRSS_{0}_VerifySystemLink_{1}", _strMode, _strPrimarySite))
            _strGetWebSLink = udtGeneralFunction.GetSystemParameterParmValue1(String.Format("eHRSS_{0}_GetEhrWebSLink_{1}", _strMode, _strPrimarySite))
            _strSysID = udtGeneralFunction.GetSystemParameterParmValue1(String.Format("eHRSS_{0}_SystemID", _strMode))
            _strEHRCertificationMark = udtGeneralFunction.GetSystemParameterParmValue1(String.Format("eHRSS_{0}_EHRCertificationMark", _strMode))
            _strServiceCode = udtGeneralFunction.GetSystemParameterParmValue1(String.Format("eHRSS_{0}_ServiceCode", _strMode))

            _strCustomDBFlag = String.Empty
            _blnCheckDCEnable = True
            _blnAutoResilience = True

            System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate

        End Sub

        Public Sub New(Optional strMode As String = "", Optional strPrimarySite As String = "", Optional strVPLink As String = "", _
                       Optional strGetWebSLink As String = "", Optional strSysID As String = "", Optional strEHRCertificationMark As String = "", _
                       Optional strServiceCode As String = "")
            Me.New()

            If strMode <> String.Empty Then _strMode = strMode
            If strPrimarySite <> String.Empty Then _strPrimarySite = strPrimarySite
            If strVPLink <> String.Empty Then _strVPLink = strVPLink
            If strGetWebSLink <> String.Empty Then _strGetWebSLink = strGetWebSLink
            If strSysID <> String.Empty Then _strSysID = strSysID
            If strEHRCertificationMark <> String.Empty Then _strEHRCertificationMark = strEHRCertificationMark
            If strServiceCode <> String.Empty Then _strServiceCode = strServiceCode

        End Sub

        '

        Public Function ValidateRemoteCertificate(sender As Object, certification As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean
            Return True
        End Function

#End Region

#Region "Fields"

        Private _strMode As String
        Private _strPrimarySite As String
        Private _strVPLink As String
        Private _strGetWebSLink As String
        Private _strSysID As String
        Private _strEHRCertificationMark As String
        Private _strServiceCode As String
        Private _strCustomDBFlag As String
        Private _blnCheckDCEnable As String
        Private _blnAutoResilience As Boolean

#End Region

#Region "Properties"

        Public Property CustomDBFlag() As String
            Get
                Return _strCustomDBFlag
            End Get
            Set(ByVal value As String)
                _strCustomDBFlag = value
            End Set
        End Property

        Public Property CheckDBEnable() As String
            Get
                Return _blnCheckDCEnable
            End Get
            Set(ByVal value As String)
                _blnCheckDCEnable = value
            End Set
        End Property

        Public Property AutoResilience() As Boolean
            Get
                Return _blnAutoResilience
            End Get
            Set(ByVal value As Boolean)
                _blnAutoResilience = value
            End Set
        End Property

#End Region

#Region "Web Service Functions"

        Public Function VerifySystem() As InVerifySystemXmlModel
            ' Prepare the Xml model
            Dim udtOutXml As New OutVerifySystemXmlModel

            ' Fill with data
            udtOutXml.SysID = _strSysID
            udtOutXml.Timestamp = GenerateTimestamp(DateTime.Now)
            udtOutXml.EHRCertificationMark = _strEHRCertificationMark

            ' Serialize
            Dim strRequestXml As String = XmlFunction.SerializeXml(udtOutXml)

            ' Send to eHR
            Dim strResponseXml As String = VerifySystem(strRequestXml)

            ' Deserialize
            Dim udtInXml As New InVerifySystemXmlModel
            XmlFunction.DeserializeXml(strResponseXml, udtInXml)

            If udtInXml.StatusEnum <> enumResultStatus.R10000 Then
                Dim strStackTrace As String = String.Format("eHRServiceBLL.VerifySystem: eHR does not return Status 10000-Success (Status={0})", udtInXml.Status)

                Throw New InvalidOperationException(strStackTrace)

            Else
                If udtInXml.VerificationPass.Trim = String.Empty Then
                    Dim strStackTrace As String = String.Format("eHRServiceBLL.VerifySystem: Unexpected value (Status={0},VerificationPass={1})", udtInXml.Status, udtInXml.VerificationPass)

                    Throw New InvalidOperationException(strStackTrace)

                End If

            End If

            Return udtInXml

        End Function

        Public Function VerifySystem(strRequestXml As String) As String
            Dim strDBFlag As String = DBFlagStr.DBFlagInterfaceLog
            If _strCustomDBFlag <> String.Empty Then strDBFlag = _strCustomDBFlag

            Dim udtAuditLog As New AuditLogBase(FunctCode.FUNT070301, strDBFlag)

            udtAuditLog.WriteLogData(LogID.LOG00001, "[EHS>EHRSS-VP] Request body", strRequestXml)

            udtAuditLog.AddDescripton("DC", _strPrimarySite)
            udtAuditLog.AddDescripton("VPLink", _strVPLink)

            udtAuditLog.WriteStartLog(LogID.LOG00002, "[EHS>EHRSS-VP] Send request", Nothing, Nothing)

            Dim strResponseXml As String = String.Empty

            Try
                Select Case _strMode
                    Case "WS"
                        Dim udtSystemVerificationWS As New SystemVerificationWebS
                        udtSystemVerificationWS.Url = _strVPLink
                        udtSystemVerificationWS.Timeout = CInt((New GeneralFunction).GetSystemParameterParmValue1("eHRSS_WS_VerifySystemLink_Timeout"))

                        strResponseXml = udtSystemVerificationWS.verifySystem(strRequestXml)

                    Case "EMULATE"
                        strResponseXml = (New DummyeHRServiceBLL).VerifySystem(strRequestXml)

                    Case Else
                        Throw New Exception(String.Format("eHRServiceBLL.VerifySystem: Unexpected value (strMode={0})", _strMode))

                End Select

                udtAuditLog.WriteEndLog(LogID.LOG00003, "[EHS>EHRSS-VP] Receive response success", Nothing, Nothing)

                udtAuditLog.WriteLogData(LogID.LOG00005, "[EHS>EHRSS-VP] Response body", strResponseXml)

            Catch ex As Exception
                udtAuditLog.AddDescripton("Exception", ex.ToString)
                udtAuditLog.WriteEndLog(LogID.LOG00004, "[EHS>EHRSS-VP] Receive response fail", Nothing, Nothing)

                Throw

            End Try

            Return strResponseXml

        End Function

        '

        Public Function GeteHRSSTokenInfo(strHKID As String) As InGeteHRTokenInfoXmlModel
            Dim strTimestamp As String = GenerateTimestamp(DateTime.Now)

            Return GeteHRSSTokenInfo(strHKID, strTimestamp)

        End Function

        Public Function GeteHRSSTokenInfo(strHKID As String, strTimestamp As String) As InGeteHRTokenInfoXmlModel
            ' Prepare the Xml model
            Dim udtOutFunctionXml As New OutGeteHRSSTokenInfoXmlModel
            udtOutFunctionXml.HKID = strHKID
            udtOutFunctionXml.Timestamp = strTimestamp

            Dim strData As String = XmlFunction.SerializeXml(udtOutFunctionXml, blnCreateCDataSection:=True)

            ' Send to eHR
            Dim udtInFunctionXml As InGeteHRTokenInfoXmlModel = Nothing

            Try
                udtInFunctionXml = GetEhrWebS(strData, enumEhrFunctionResult.geteHRSSTokenInfoResult)

                ' Error result code
                Select Case udtInFunctionXml.ResultCodeEnum
                    Case eHRResultCode.R9000_InvalidXmlElement, eHRResultCode.R9001_InvalidParameter, eHRResultCode.R9999_UnexpectedFailure
                        Dim strMessage As String = String.Format("eHRServiceBLL.GeteHRSSTokenInfo: eHRSS returns {0}. eHS parameters: " + _
                                                                 "Timestamp={1}", _
                                                                 udtInFunctionXml.ResultCodeEnum.ToString, _
                                                                 udtOutFunctionXml.Timestamp)

                        Throw New Exception(strMessage)

                End Select

                ' Validation
                If udtInFunctionXml.ResultCodeEnum = eHRResultCode.R1000_Success Then
                    If udtInFunctionXml.ExistingTokenID.Trim = String.Empty _
                            OrElse udtInFunctionXml.ExistingTokenIssuer.Trim = String.Empty _
                            OrElse udtInFunctionXml.IsExistingTokenShared.Trim = String.Empty Then
                        Throw New Exception(String.Format("eHRServiceBLL.GeteHRSSTokenInfo: Unexpected result (ResultCode={0},ExistingTokenID={1},ExistingTokenIssuer={2},IsExistingTokenShared={3})", _
                                                          udtInFunctionXml.ResultCode.ToString, udtInFunctionXml.ExistingTokenID, _
                                                          udtInFunctionXml.ExistingTokenIssuer, udtInFunctionXml.IsExistingTokenShared))
                    End If
                End If

            Catch ex As Exception
                WriteSystemLog(FunctCode.FUNT070302, ex.Message)

                Throw

            End Try

            ' Return
            Return udtInFunctionXml

        End Function

        '

        Public Function SeteHRSSTokenShared(strHKID As String, strExistingTokenID As String, strNewTokenID As String, blnShared As Boolean, _
                                            strTimestamp As String, ByRef strReferenceQueueIDOut As String) As InSeteHRSSTokenSharedXmlModel
            ' Prepare the Xml model
            Dim udtOutFunctionXml As New OutSeteHRSSTokenSharedXmlModel
            udtOutFunctionXml.HKID = strHKID
            udtOutFunctionXml.ExistingTokenID = strExistingTokenID.TrimStart("0".ToCharArray)

            If Not IsNothing(strNewTokenID) AndAlso strNewTokenID <> String.Empty Then
                udtOutFunctionXml.NewTokenID = strNewTokenID.TrimStart("0".ToCharArray)
            End If

            udtOutFunctionXml.Shared = IIf(blnShared, "Y", "N")
            udtOutFunctionXml.Timestamp = strTimestamp

            Dim strData As String = XmlFunction.SerializeXml(udtOutFunctionXml, blnCreateCDataSection:=True)

            ' Send to eHR
            Dim udtInFunctionXml As InSeteHRSSTokenSharedXmlModel = Nothing

            Try
                udtInFunctionXml = GetEhrWebS(strData, enumEhrFunctionResult.seteHRSSTokenSharedResult)

                Select Case udtInFunctionXml.ResultCodeEnum
                    Case eHRResultCode.R9000_InvalidXmlElement, eHRResultCode.R9001_InvalidParameter, eHRResultCode.R9999_UnexpectedFailure
                        Dim strMessage As String = String.Format("eHRServiceBLL.SeteHRSSTokenShared: eHRSS returns {0}. eHS parameters: " + _
                                                                 "ExistingTokenID={1},NewTokenID={2},Shared={3},Timestamp={4}", _
                                                                 udtInFunctionXml.ResultCodeEnum.ToString, _
                                                                 udtOutFunctionXml.ExistingTokenID, _
                                                                 udtOutFunctionXml.NewTokenID, _
                                                                 udtOutFunctionXml.Shared, _
                                                                 udtOutFunctionXml.Timestamp)

                        If udtInFunctionXml.ResultCodeEnum = eHRResultCode.R9999_UnexpectedFailure Then
                            Throw New Exception(strMessage)
                        Else
                            WriteSystemLog(FunctCode.FUNT070302, strMessage)
                        End If

                End Select

            Catch ex As Exception
                ' Write to queue (only for the set share to N. For the Y case, user must redo immediately)
                If blnShared = False Then
                    AddeHRInterfaceIntegrationQueue(enumEhrIntegrationInterfaceQueueType.SETSHARE, strData, strReferenceQueueIDOut)
                End If

                WriteSystemLog(FunctCode.FUNT070302, ex.Message)

                Throw

            End Try

            ' Return
            Return udtInFunctionXml

        End Function

        '

        Public Function ReplaceeHRSSToken(strHKID As String, strExistingTokenID As String, strNewTokenID As String, strReplaceReasonCode As String, _
                                          strTimestamp As String, ByRef strReferenceQueueIDOut As String) As InReplaceeHRSSTokenXmlModel
            ' Prepare the Xml model
            Dim udtOutFunctionXml As New OutReplaceeHRSSTokenXmlModel
            udtOutFunctionXml.HKID = strHKID
            udtOutFunctionXml.ExistingTokenID = strExistingTokenID.TrimStart("0".ToCharArray)
            udtOutFunctionXml.NewTokenID = strNewTokenID.TrimStart("0".ToCharArray)
            udtOutFunctionXml.ReplaceReasonCode = strReplaceReasonCode
            udtOutFunctionXml.Timestamp = strTimestamp

            Dim strData As String = XmlFunction.SerializeXml(udtOutFunctionXml, blnCreateCDataSection:=True)

            ' Send to eHR
            Dim udtInFunctionXml As InReplaceeHRSSTokenXmlModel = Nothing

            Try
                udtInFunctionXml = GetEhrWebS(strData, enumEhrFunctionResult.replaceeHRSSTokenResult)

                Select Case udtInFunctionXml.ResultCodeEnum
                    Case eHRResultCode.R9000_InvalidXmlElement, eHRResultCode.R9001_InvalidParameter, eHRResultCode.R9999_UnexpectedFailure
                        Dim strMessage As String = String.Format("eHRServiceBLL.ReplaceeHRSSToken: eHRSS returns {0}. eHS parameters: " + _
                                                                 "ExistingTokenID={1},NewTokenID={2},ReplaceReasonCode={3},Timestamp={4}", _
                                                                 udtInFunctionXml.ResultCodeEnum.ToString, _
                                                                 udtOutFunctionXml.ExistingTokenID, _
                                                                 udtOutFunctionXml.NewTokenID, _
                                                                 udtOutFunctionXml.ReplaceReasonCode, _
                                                                 udtOutFunctionXml.Timestamp)

                        If udtInFunctionXml.ResultCodeEnum = eHRResultCode.R9999_UnexpectedFailure Then
                            Throw New Exception(strMessage)
                        Else
                            WriteSystemLog(FunctCode.FUNT070302, strMessage)
                        End If

                End Select

            Catch ex As Exception
                ' Write to queue
                AddeHRInterfaceIntegrationQueue(enumEhrIntegrationInterfaceQueueType.REPLACETOKEN, strData, strReferenceQueueIDOut)

                WriteSystemLog(FunctCode.FUNT070302, ex.Message)

                Throw

            End Try

            ' Return
            Return udtInFunctionXml

        End Function

        '

        Public Function NotifyeHRSSTokenDeactivated(strHKID As String, strExistingTokenID As String, strNewTokenID As String, strDeactivateReasonCode As String, _
                                                    strTimestamp As String, ByRef strReferenceQueueIDOut As String) As InNotifyeHRSSTokenDeactivatedXmlModel
            ' Prepare the Xml model
            Dim udtOutFunctionXml As New OutNotifyeHRSSTokenDeactivatedXmlModel
            udtOutFunctionXml.HKID = strHKID
            udtOutFunctionXml.ExistingTokenID = strExistingTokenID.TrimStart("0".ToCharArray)

            If Not IsNothing(strNewTokenID) AndAlso strNewTokenID <> String.Empty Then
                udtOutFunctionXml.NewTokenID = strNewTokenID.TrimStart("0".ToCharArray)
            End If

            udtOutFunctionXml.DeactivateReasonCode = "D" ' Hard code to be D
            udtOutFunctionXml.Timestamp = strTimestamp

            Dim strData As String = XmlFunction.SerializeXml(udtOutFunctionXml, blnCreateCDataSection:=True)

            ' Send to eHR
            Dim udtInFunctionXml As InNotifyeHRSSTokenDeactivatedXmlModel = Nothing

            Try
                udtInFunctionXml = GetEhrWebS(strData, enumEhrFunctionResult.notifyeHRSSTokenDeactivatedResult)

                Select Case udtInFunctionXml.ResultCodeEnum
                    Case eHRResultCode.R9000_InvalidXmlElement, eHRResultCode.R9001_InvalidParameter, eHRResultCode.R9999_UnexpectedFailure
                        Dim strMessage As String = String.Format("eHRServiceBLL.NotifyeHRSSTokenDeactivated: eHRSS returns {0}. eHS parameters: " + _
                                                                 "ExistingTokenID={1},NewTokenID={2},DeactivateReasonCode={3},Timestamp={4}", _
                                                                 udtInFunctionXml.ResultCodeEnum.ToString, _
                                                                 udtOutFunctionXml.ExistingTokenID, _
                                                                 udtOutFunctionXml.NewTokenID, _
                                                                 udtOutFunctionXml.DeactivateReasonCode, _
                                                                 udtOutFunctionXml.Timestamp)

                        If udtInFunctionXml.ResultCodeEnum = eHRResultCode.R9999_UnexpectedFailure Then
                            Throw New Exception(strMessage)
                        Else
                            WriteSystemLog(FunctCode.FUNT070302, strMessage)
                        End If

                End Select

            Catch ex As Exception
                ' Write to queue
                AddeHRInterfaceIntegrationQueue(enumEhrIntegrationInterfaceQueueType.DEACTIVATETOKEN, strData, strReferenceQueueIDOut)

                WriteSystemLog(FunctCode.FUNT070302, ex.Message)

                Throw

            End Try

            ' Return
            Return udtInFunctionXml

        End Function

        '

        Public Function GeteHRSSLoginAlias(strHKID As String) As InGeteHRSSLoginAliasXmlModel
            Dim strTimestamp As String = GenerateTimestamp(DateTime.Now)

            Return GeteHRSSLoginAlias(strHKID, strTimestamp)

        End Function

        Public Function GeteHRSSLoginAlias(strHKID As String, strTimestamp As String) As InGeteHRSSLoginAliasXmlModel
            ' Prepare the Xml model
            Dim udtOutFunctionXml As New OutGeteHRSSLoginAliasXmlModel
            udtOutFunctionXml.HKID = strHKID
            udtOutFunctionXml.Timestamp = strTimestamp

            Dim strData As String = XmlFunction.SerializeXml(udtOutFunctionXml, blnCreateCDataSection:=True)

            ' Send to eHR
            Dim udtInFunctionXml As InGeteHRSSLoginAliasXmlModel = Nothing

            Try
                udtInFunctionXml = GetEhrWebS(strData, enumEhrFunctionResult.geteHRSSLoginAliasResult)

                Select Case udtInFunctionXml.ResultCodeEnum
                    Case eHRResultCode.R9000_InvalidXmlElement, eHRResultCode.R9001_InvalidParameter, eHRResultCode.R9999_UnexpectedFailure
                        Dim strMessage As String = String.Format("eHRServiceBLL.GeteHRSSLoginAlias: eHRSS returns {0}. eHS parameters: " + _
                                                                 "Timestamp={1}", _
                                                                 udtInFunctionXml.ResultCodeEnum.ToString, _
                                                                 udtOutFunctionXml.Timestamp)

                        Throw New Exception(strMessage)

                End Select

                ' Validation
                If udtInFunctionXml.ResultCodeEnum = eHRResultCode.R1000_Success Then
                    If udtInFunctionXml.LoginAlias.Trim = String.Empty Then
                        Throw New Exception(String.Format("eHRServiceBLL.GeteHRSSLoginAlias: Unexpected result (ResultCode={0},LoginAlias={1})", _
                                                          udtInFunctionXml.ResultCode.ToString, udtInFunctionXml.LoginAlias))
                    End If
                End If

            Catch ex As Exception
                WriteSystemLog(FunctCode.FUNT070302, ex.Message)

                Throw

            End Try

            ' Return
            Return udtInFunctionXml

        End Function

        '

        Public Function HealthCheckeHRSS() As InHealthCheckeHRSSXmlModel
            Dim strTimestamp As String = GenerateTimestamp(DateTime.Now)

            Return HealthCheckeHRSS(strTimestamp)

        End Function

        Public Function HealthCheckeHRSS(strTimestamp As String) As InHealthCheckeHRSSXmlModel
            ' Prepare the Xml model
            Dim udtOutFunctionXml As New OutHealthCheckeHRSSXmlModel
            udtOutFunctionXml.Timestamp = strTimestamp

            Dim strData As String = XmlFunction.SerializeXml(udtOutFunctionXml, blnCreateCDataSection:=True)

            ' Send to eHR
            Dim udtInFunctionXml As InHealthCheckeHRSSXmlModel = GetEhrWebS(strData, enumEhrFunctionResult.healthCheckeHRSSResult)

            ' Return
            Return udtInFunctionXml

        End Function

        '

        Public Function GetEhrWebS(strData As String, eFunctionName As enumEhrFunctionResult) As Object
            Dim strDBFlag As String = DBFlagStr.DBFlagInterfaceLog
            If _strCustomDBFlag <> String.Empty Then strDBFlag = _strCustomDBFlag

            Dim udtGeneralFunction As New GeneralFunction
            Dim udtAuditLog As New AuditLogBase(FunctCode.FUNT070302, strDBFlag)

            Try
                If _blnCheckDCEnable = False OrElse udtGeneralFunction.GetSystemParameterParmValue1(String.Format("eHRSS_{0}_{1}_Enable", _strMode, _strPrimarySite)) = YesNo.Yes Then
                    ' Use Primary DC to call
                    Dim objResult As Object = HandleGetEhrWebS(strData, eFunctionName)
                    Return objResult

                End If

            Catch exSoap As SoapException
                ' Retry with another DC
                udtAuditLog.AddDescripton("SoapException", exSoap.Message)
                udtAuditLog.WriteLog(LogID.LOG00101, String.Format("Primary site {0} fail in {1} platform", _strPrimarySite, ConfigurationManager.AppSettings("Platform")))

                If _blnAutoResilience = False Then Throw

            Catch exIOE As InvalidOperationException
                ' Retry with another DC
                udtAuditLog.AddDescripton("InvalidOperationException", exIOE.Message)
                udtAuditLog.WriteLog(LogID.LOG00101, String.Format("Primary site {0} fail in {1} platform", _strPrimarySite, ConfigurationManager.AppSettings("Platform")))

                If _blnAutoResilience = False Then Throw

            Catch ex As Exception
                udtAuditLog.AddDescripton("Exception", ex.Message)
                udtAuditLog.WriteLog(LogID.LOG00101, String.Format("Primary site {0} fail in {1} platform", _strPrimarySite, ConfigurationManager.AppSettings("Platform")))

                Throw

            End Try

            Dim strPriority As String = udtGeneralFunction.GetSystemParameterParmValue1(String.Format("eHRSS_{0}_Priority", _strMode))
            Dim lstUsedSite As New List(Of String)
            Dim strOldPrimarySite As String = _strPrimarySite

            lstUsedSite.Add(_strPrimarySite)

            For Each strSite As String In strPriority.Split(",".ToArray, StringSplitOptions.RemoveEmptyEntries)
                If lstUsedSite.Contains(strSite) Then Continue For

                If udtGeneralFunction.getSystemParameterValue1(String.Format("eHRSS_{0}_{1}_Enable", _strMode, strSite)) <> YesNo.Yes Then
                    Continue For
                End If

                _strPrimarySite = strSite
                RefreshLinkFromNewPrimarySite()

                Try
                    Dim objResult As Object = HandleGetEhrWebS(strData, eFunctionName)

                    UpdateNewPrimarySite(strOldPrimarySite, strSite, udtAuditLog)

                    Return objResult

                Catch exSoap As SoapException
                    ' Retry with another DC
                    udtAuditLog.AddDescripton("SoapException", exSoap.Message)
                    udtAuditLog.WriteLog(LogID.LOG00103, String.Format("Secondary site {0} fail in {1} platform", _strPrimarySite, ConfigurationManager.AppSettings("Platform")))

                Catch exIOE As InvalidOperationException
                    ' Retry with another DC
                    udtAuditLog.AddDescripton("InvalidOperationException", exIOE.Message)
                    udtAuditLog.WriteLog(LogID.LOG00103, String.Format("Secondary site {0} fail in {1} platform", _strPrimarySite, ConfigurationManager.AppSettings("Platform")))

                Catch ex As Exception
                    udtAuditLog.AddDescripton("Exception", ex.Message)
                    udtAuditLog.WriteLog(LogID.LOG00103, String.Format("Secondary site {0} fail in {1} platform", _strPrimarySite, ConfigurationManager.AppSettings("Platform")))

                    Throw

                End Try

                lstUsedSite.Add(strSite)

            Next

            Dim strMessage As String = String.Format("All sites fail in {0} platform", ConfigurationManager.AppSettings("Platform"))

            udtAuditLog.WriteLog(LogID.LOG00102, strMessage)

            Throw New Exception(strMessage)

        End Function

        Public Function HandleGetEhrWebS(strData As String, eFunctionName As enumEhrFunctionResult) As Object
            ' Prepare the Xml model
            Dim udtOutXml As New OutGeteHRWebSXmlModel

            ' Get VerificationPass from DB/EHR
            Dim udtVP As VerificationPassModel = GetVerificationPass()

            ' Fill with data
            udtOutXml.VerificationPass = udtVP.VerificationPassValue
            udtOutXml.SysID = _strSysID
            udtOutXml.servicecode = _strServiceCode

            ' Prepare the Xml model
            udtOutXml.data = strData

            ' Serialize
            Dim strRequestXml As String = XmlFunction.SerializeXml(udtOutXml)

            ' Send to eHR
            Dim strResponseXml As String = String.Empty

            Try
                strResponseXml = GetEhrWebS(strRequestXml, eFunctionName.ToString)

            Catch ex As SoapHeaderException
                If ex.Message = "Invalid VP" AndAlso udtVP.GetFromEHR = False Then
                    ' Retry with a fresh VP
                    udtOutXml.VerificationPass = RefreshVerificationPassFromEHRSS.VerificationPassValue

                    strRequestXml = XmlFunction.SerializeXml(udtOutXml)

                    strResponseXml = GetEhrWebS(strRequestXml, eFunctionName.ToString)

                Else
                    Throw

                End If

            End Try

            ' Deserialize
            Dim udtInXml As New InGeteHRWebSXmlModel

            XmlFunction.DeserializeXml(strResponseXml, udtInXml)

            Select Case udtInXml.StatusEnum
                Case enumResultStatus.R70000
                    ' Fine, proceed

                Case enumResultStatus.R79999
                    Dim strStackTrace As String = String.Format("eHRServiceBLL.GetEhrWebS: eHR returns Status 79999-UnexpectedFailure (Status={0})", udtInXml.Status)

                    Throw New InvalidOperationException(strStackTrace)

                Case Else
                    Dim strStackTrace As String = String.Format("eHRServiceBLL.GetEhrWebS: Unexpected Status value (Status={0})", udtInXml.Status)

                    Throw New Exception(strStackTrace)

            End Select

            ' Convert the data part to Xml
            Dim xml As New XmlDocument
            xml.LoadXml(udtInXml.data)

            ' Deserialize the input
            Dim udtInFunctionXml As Object = Nothing

            Select Case xml.DocumentElement.Name
                Case enumEhrFunctionResult.geteHRSSTokenInfoResult.ToString
                    Dim udtXml As New InGeteHRTokenInfoXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtXml)

                    If udtXml.ResultCodeEnum = eHRResultCode.R9999_UnexpectedFailure Then
                        Throw New InvalidOperationException(String.Format("eHRServiceBLL.GetEhrWebS: eHR returns ResultCode {0}", udtXml.ResultCodeEnum.ToString))
                    End If

                    udtInFunctionXml = udtXml

                Case enumEhrFunctionResult.seteHRSSTokenSharedResult.ToString
                    Dim udtXml As New InSeteHRSSTokenSharedXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtXml)

                    If udtXml.ResultCodeEnum = eHRResultCode.R9999_UnexpectedFailure Then
                        Throw New InvalidOperationException(String.Format("eHRServiceBLL.GetEhrWebS: eHR returns ResultCode {0}", udtXml.ResultCodeEnum.ToString))
                    End If

                    udtInFunctionXml = udtXml

                Case enumEhrFunctionResult.replaceeHRSSTokenResult.ToString
                    Dim udtXml As New InReplaceeHRSSTokenXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtXml)

                    If udtXml.ResultCodeEnum = eHRResultCode.R9999_UnexpectedFailure Then
                        Throw New InvalidOperationException(String.Format("eHRServiceBLL.GetEhrWebS: eHR returns ResultCode {0}", udtXml.ResultCodeEnum.ToString))
                    End If

                    udtInFunctionXml = udtXml

                Case enumEhrFunctionResult.notifyeHRSSTokenDeactivatedResult.ToString
                    Dim udtXml As New InNotifyeHRSSTokenDeactivatedXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtXml)

                    If udtXml.ResultCodeEnum = eHRResultCode.R9999_UnexpectedFailure Then
                        Throw New InvalidOperationException(String.Format("eHRServiceBLL.GetEhrWebS: eHR returns ResultCode {0}", udtXml.ResultCodeEnum.ToString))
                    End If

                    udtInFunctionXml = udtXml

                Case enumEhrFunctionResult.geteHRSSLoginAliasResult.ToString
                    Dim udtXml As New InGeteHRSSLoginAliasXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtXml)

                    If udtXml.ResultCodeEnum = eHRResultCode.R9999_UnexpectedFailure Then
                        Throw New InvalidOperationException(String.Format("eHRServiceBLL.GetEhrWebS: eHR returns ResultCode {0}", udtXml.ResultCodeEnum.ToString))
                    End If

                    udtInFunctionXml = udtXml

                Case enumEhrFunctionResult.healthCheckeHRSSResult.ToString
                    Dim udtXml As New InHealthCheckeHRSSXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtXml)

                    If udtXml.ResultCodeEnum = eHRResultCode.R9999_UnexpectedFailure Then
                        Throw New InvalidOperationException(String.Format("eHRServiceBLL.GetEhrWebS: eHR returns ResultCode {0}", udtXml.ResultCodeEnum.ToString))
                    End If

                    udtInFunctionXml = udtXml

                Case Else
                    Throw New NotImplementedException

            End Select

            ' Return
            Return udtInFunctionXml

        End Function

        Public Function GetEhrWebS(strRequestXml As String, strFunctionName As String) As String
            Dim strDBFlag As String = DBFlagStr.DBFlagInterfaceLog
            If _strCustomDBFlag <> String.Empty Then strDBFlag = _strCustomDBFlag

            Dim udtAuditLog As New AuditLogBase(FunctCode.FUNT070302, strDBFlag)

            udtAuditLog.WriteLogData(LogID.LOG00001, "[EHS>EHRSS-WS] Request body", strRequestXml)

            udtAuditLog.AddDescripton("DC", _strPrimarySite)
            udtAuditLog.AddDescripton("WSLink", _strGetWebSLink)
            udtAuditLog.AddDescripton("FunctionName", strFunctionName.Replace("Result", String.Empty))

            udtAuditLog.WriteStartLog(LogID.LOG00002, "[EHS>EHRSS-WS] Send request", Nothing, Nothing)

            Dim strResponseXml As String = String.Empty

            Try
                Select Case _strMode
                    Case "WS"
                        Dim udtExternalCallinWS As New ExternalCallinWebS
                        udtExternalCallinWS.Url = _strGetWebSLink
                        udtExternalCallinWS.Timeout = CInt((New GeneralFunction).GetSystemParameterParmValue1("eHRSS_WS_GetEhrWebSLink_Timeout"))

                        strResponseXml = udtExternalCallinWS.getEhrWebS(strRequestXml)

                    Case "EMULATE"
                        strResponseXml = (New DummyeHRServiceBLL).GetEhrWebS(strRequestXml)

                    Case Else
                        Throw New Exception(String.Format("eHRServiceBLL.GetEhrWebS: Unexpected value (strMode={0})", _strMode))

                End Select

                udtAuditLog.WriteEndLog(LogID.LOG00003, "[EHS>EHRSS-WS] Receive response success", Nothing, Nothing)

                udtAuditLog.WriteLogData(LogID.LOG00005, "[EHS>EHRSS-WS] Response body", strResponseXml)

            Catch ex As Exception
                udtAuditLog.AddDescripton("Exception", ex.ToString)
                udtAuditLog.WriteEndLog(LogID.LOG00004, "[EHS>EHRSS-WS] Receive response fail", Nothing, Nothing)

                Throw

            End Try

            Return strResponseXml

        End Function

#End Region

#Region "VerificationPass Functions"

        Public Function GetVerificationPass() As VerificationPassModel
            Dim udtGeneralFunction As New GeneralFunction
            Dim dt As DataTable = udtGeneralFunction.GetSystemVariable("eHRSS_VerificationPass")

            If dt.Rows.Count <> 1 Then
                Throw New Exception(String.Format("eHRServiceBLL.GetVerificationPass: Unexpected value (dt.Rows.Count={0})", dt.Rows.Count))
            End If

            Dim dr As DataRow = dt.Rows(0)

            If IsDBNull(dr("Variable_Value")) Then
                Return RefreshVerificationPassFromEHRSS()

            End If

            ' Check the VP still in valid duration
            Dim dtmUpdate As DateTime = dr("Update_Dtm")
            Dim strMode As String = udtGeneralFunction.GetSystemParameterParmValue1("eHRSS_Mode")
            Dim intDuration As Integer = CInt(udtGeneralFunction.GetSystemParameterParmValue1(String.Format("eHRSS_{0}_VerPassValidDurationMinute", strMode)))

            If DateTime.Now > dtmUpdate.AddMinutes(intDuration) Then
                Return RefreshVerificationPassFromEHRSS()

            End If

            Dim udtVP As New VerificationPassModel
            udtVP.VerificationPassValue = dr("Variable_Value")
            udtVP.GetFromEHR = False

            Return udtVP

        End Function

        Public Function RefreshVerificationPassFromEHRSS() As VerificationPassModel
            Dim udtInXml As InVerifySystemXmlModel = VerifySystem()

            Call (New GeneralFunction).UpdateSystemVariable("eHRSS_VerificationPass", udtInXml.VerificationPass, Constant.EHSUPDATEBY, Nothing)

            Dim udtVP As New VerificationPassModel
            udtVP.VerificationPassValue = udtInXml.VerificationPass
            udtVP.GetFromEHR = True

            Return udtVP

        End Function

#End Region

#Region "Primary Site Functions"

        Private Sub RefreshLinkFromNewPrimarySite()
            Dim udtGeneralFunction As New GeneralFunction

            _strVPLink = udtGeneralFunction.GetSystemParameterParmValue1(String.Format("eHRSS_{0}_VerifySystemLink_{1}", _strMode, _strPrimarySite))
            _strGetWebSLink = udtGeneralFunction.GetSystemParameterParmValue1(String.Format("eHRSS_{0}_GetEhrWebSLink_{1}", _strMode, _strPrimarySite))

        End Sub

        Public Sub UpdateNewPrimarySite(strOldPrimarySite As String, strNewPrimarySite As String, udtAuditLog As AuditLogBase)
            Call (New GeneralFunction).UpdateSystemVariable("eHRSS_PrimarySite", strNewPrimarySite, Constant.EHSUPDATEBY, Nothing)

            udtAuditLog.WriteLog(LogID.LOG00104, String.Format("Primary site switched from {0} to {1}", strOldPrimarySite, strNewPrimarySite))

        End Sub

#End Region

#Region "Supporting Functions"

        Public Shared Function GenerateTimestamp() As String
            Return GenerateTimestamp(DateTime.Now)
        End Function

        Public Shared Function GenerateTimestamp(dtm As DateTime) As String
            Return String.Format("{0}{1}", _
                                 dtm.ToString("yyyy-MM-dd HH:mm:ss.fff"), _
                                 dtm.ToString("zzzz").Replace(":", String.Empty))
        End Function

        Private Sub AddeHRInterfaceIntegrationQueue(eQueueType As enumEhrIntegrationInterfaceQueueType, strQueueContent As String, _
                                                    ByRef strQueueIDOut As String)
            ' --- Validation ---
            If eQueueType = enumEhrIntegrationInterfaceQueueType.NA Then
                Throw New Exception(String.Format("eHRServiceBLL.AddeHRInterfaceIntegrationQueue: Unexpected value (eQueueType={0})", eQueueType.ToString))
            End If
            ' --- End of Validation ---

            Dim strQueueID As String = (New GeneralFunction).GenerateeHRIntegrationInterfaceQueueID

            Call (New eHRServiceDAL).AddeHRInterfaceIntegrationQueue(strQueueID, eQueueType.ToString, strQueueContent)

            strQueueIDOut = strQueueID

        End Sub

        Private Sub WriteSystemLog(strFunctionCode As String, strMessage As String)
            Dim strDBFlag As String = DBFlagStr.DBFlagInterfaceLog
            If _strCustomDBFlag <> String.Empty Then strDBFlag = _strCustomDBFlag

            Dim udtAuditLog As New AuditLogBase(strFunctionCode, strDBFlag)

            udtAuditLog.WriteSystemLog(New Exception(strMessage), strFunctionCode, String.Empty)

        End Sub

#End Region

    End Class

End Namespace
