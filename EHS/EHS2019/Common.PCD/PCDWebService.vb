
Imports Common.ComFunction
Imports Common.DataAccess
Imports Common.Component
Imports Common.Component.Mapping
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.PracticeType_PCD
Imports Common.Component.ServiceProvider
Imports Common.Component.ThirdParty
Imports Common.PCD.WebService.Interface

Imports Microsoft.Web.Services3.Design
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Xml


Public Class PCDWebService

    Private _strSourceFunctionCode As String = String.Empty

    Private _PCDUploadEnrolInfoRequest_XML As String = String.Empty
    Private _PCDUploadEnrolInfoResult_XML As String = String.Empty

    Private _PCDCreatePCDSPAcctRequest_XML As String = String.Empty
    Private _PCDCreatePCDSPAcctResult_XML As String = String.Empty

    Private _PCDTransferPracticeInfoRequest_XML As String = String.Empty
    Private _PCDTransferPracticeInfoResult_XML As String = String.Empty

    Private _PCDCheckIsActiveSPRequest_XML As String = String.Empty
    Private _PCDCheckIsActiveSPResult_XML As String = String.Empty

    Private _PCDHealthCheckRequest_XML As String = String.Empty
    Private _PCDHealthCheckResult_XML As String = String.Empty

    Private _PCDCheckAvailableForVerifiedEnrolmentRequest_XML As String = String.Empty
    Private _PCDCheckAvailableForVerifiedEnrolmentResult_XML As String = String.Empty

    Private _PCDUploadVerifiedEnrolmentRequest_XML As String = String.Empty
    Private _PCDUploadVerifiedEnrolmentResult_XML As String = String.Empty

    ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
    Private _PCDCheckAccountStatusRequest_XML As String = String.Empty
    Private _PCDCheckAccountStatusResult_XML As String = String.Empty
    ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---

    Private Const PCD_INTEGRATION_WEB_SERVICE_URL As String = "PCD_INTEGRATION_WEB_SERVICE_URL"
    Private Const PCD_INTEGRATION_WEB_SERVICE_USER As String = "PCD_INTEGRATION_WEB_SERVICE_USER"
    Private Const PCD_INTEGRATION_WEB_SERVICE_PASSWORD As String = "PCD_INTEGRATION_WEB_SERVICE_PASSWORD"
    Private Const PCD_INTEGRATION_WEB_SERVICE_TIMELIMIT As String = "PCD_INTEGRATION_WEB_SERVICE_TIMELIMIT"
    Private Const PCD_INTEGRATION_WEB_SERVICE_USE_PROXY As String = "PCD_INTEGRATION_WEB_SERVICE_USE_PROXY"

    Public ReadOnly Property PCDUploadEnrolInfoRequest_XML() As String
        Get
            Return _PCDUploadEnrolInfoRequest_XML
        End Get
    End Property

    Public ReadOnly Property PCDUploadEnrolInfoResult_XML() As String
        Get
            Return _PCDUploadEnrolInfoResult_XML
        End Get
    End Property

    Public ReadOnly Property PCDCreatePCDSPAcctRequest_XML() As String
        Get
            Return _PCDCreatePCDSPAcctRequest_XML
        End Get
    End Property

    Public ReadOnly Property PCDCreatePCDSPAcctResult_XML() As String
        Get
            Return _PCDCreatePCDSPAcctResult_XML
        End Get
    End Property

    Public ReadOnly Property PCDTransferPracticeInfoRequest_XML() As String
        Get
            Return _PCDTransferPracticeInfoRequest_XML
        End Get
    End Property

    Public ReadOnly Property PCDTransferPracticeInfoResult_XML() As String
        Get
            Return _PCDTransferPracticeInfoResult_XML
        End Get
    End Property

    Public ReadOnly Property PCDCheckIsActiveSPRequest_XML() As String
        Get
            Return _PCDCheckIsActiveSPRequest_XML
        End Get
    End Property

    Public ReadOnly Property PCDCheckIsActiveSPResult_XML() As String
        Get
            Return _PCDCheckIsActiveSPResult_XML
        End Get
    End Property

    Public ReadOnly Property PCDCheckAvailableForVerifiedEnrolmentRequest_XML() As String
        Get
            Return _PCDCheckAvailableForVerifiedEnrolmentRequest_XML
        End Get
    End Property

    Public ReadOnly Property PCDCheckAvailableForVerifiedEnrolmentResult_XML() As String
        Get
            Return _PCDCheckAvailableForVerifiedEnrolmentResult_XML
        End Get
    End Property

    Public ReadOnly Property PCDUploadVerifiedEnrolmentRequest_XML() As String
        Get
            Return _PCDUploadVerifiedEnrolmentRequest_XML
        End Get
    End Property

    Public ReadOnly Property PCDUploadVerifiedEnrolmentResult_XML() As String
        Get
            Return _PCDCheckAvailableForVerifiedEnrolmentResult_XML
        End Get
    End Property


    Public ReadOnly Property SourceFunctionCode() As String
        Get
            Return Me._strSourceFunctionCode
        End Get
    End Property

    Public Sub New(ByVal strSourceFunctionCode As String)
        Me._strSourceFunctionCode = strSourceFunctionCode
    End Sub

    'Public Function PCDUploadEnrolInfo(ByVal udtSPModel As ServiceProviderModel) As PCDUploadEnrolInfoResult
    '    Dim objRequest As PCDUploadEnrolInfoRequest
    '    Dim objResult As PCDUploadEnrolInfoResult

    '    objRequest = New PCDUploadEnrolInfoRequest()
    '    _PCDUploadEnrolInfoRequest_XML = objRequest.GenerateXML(udtSPModel, True)    ' no message ID to be generated

    '    ' check if it contains errors
    '    If objRequest.HasError Then
    '        objResult = New PCDUploadEnrolInfoResult(PCDUploadEnrolInfoResult.enumReturnCode.ErrorAllUnexpected)
    '        ' copy the error message and code to the result object
    '        objResult.ReturnCodeDesc = objRequest.ErrorMessage
    '        objResult.Request = _PCDUploadEnrolInfoRequest_XML
    '        Return objResult
    '    End If

    '    ' insert into ThirdPartyEnrollRecord table
    '    Dim udtThirdPartyEnrollRecordBLL As ThirdPartyEnrollRecordBLL = New ThirdPartyEnrollRecordBLL
    '    Dim udtThirdPartyEnrollRecordModel As ThirdPartyEnrollRecordModel = New ThirdPartyEnrollRecordModel( _
    '                    ThirdPartyEnrollRecordBLL.EnumThirdPartySystemCode.PCD.ToString(), _
    '                    objRequest.PCD_ERN, _
    '                    objRequest.XmlRequest.InnerXml, _
    '                    objRequest.EnrolmentSubmissionTime, _
    '                    Common.Component.ThirdParty.ThirdPartyEnrollRecordModel.EnumRecordStatus.PENDING, _
    '                    "", _
    '                    0, _
    '                    Nothing, _
    '                    "Administrator", _
    '                    Nothing, _
    '                    "Administrator", _
    '                    Nothing)
    '    Dim blnAddSuccess As Boolean = False

    '    blnAddSuccess = udtThirdPartyEnrollRecordBLL.AddThirdPartyEnrollRecord(udtThirdPartyEnrollRecordModel, New Common.DataAccess.Database())

    '    If blnAddSuccess Then
    '        objResult = New PCDUploadEnrolInfoResult(PCDUploadEnrolInfoResult.enumReturnCode.SuccessWithData)
    '        objResult.Request = _PCDUploadEnrolInfoRequest_XML
    '    Else
    '        objResult = New PCDUploadEnrolInfoResult(PCDUploadEnrolInfoResult.enumReturnCode.ErrorAllUnexpected)
    '        objResult.Request = _PCDUploadEnrolInfoRequest_XML
    '    End If

    '    Return objResult

    'End Function

    '#Region "Audit Log Description"
    '    Public Class AuditLogDesc
    '        Public Class PCDUploadEnrolInfo
    '            Public Const Start As String = "PCDUploadEnrolInfo - Start""
    '            Public Const Start_ID As String = LogID.LOG00000

    '            Public Const InvokePCDStart As String = "PCDUploadEnrolInfo - Invoke PCD Start"
    '            Public Const InvokePCDStart_ID As String = LogID.LOG00001

    '            Public Const InvokePCDEnd_Success As String = "PCDUploadEnrolInfo - Invoke PCD End [Success]"
    '            Public Const InvokePCDEnd_Success_ID As String = LogID.LOG00002

    '            Public Const InvokePCDEnd_Fail As String = "PCDUploadEnrolInfo - Invoke PCD End [Fail]"
    '            Public Const InvokePCDEnd_Fail_ID As String = LogID.LOG00002

    '            Public Const [End] As String = "PCDUploadEnrolInfo - End"""
    '            Public Const End_ID As String = LogID.LOG00004
    '        End Class
    '    End Class
    '#End Region

    Public Function PCDUploadEnrolInfo(ByVal udtModel As ThirdPartyEnrollRecordModel) As PCDUploadEnrolInfoResult
        Dim objAuditLog As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.PCDUploadEnrolInfo)
        objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
        objAuditLog.WriteLog(LogID.LOG00000)

        Dim objRequest As PCDUploadEnrolInfoRequest
        Dim objResult As PCDUploadEnrolInfoResult

        objRequest = New PCDUploadEnrolInfoRequest()
        _PCDUploadEnrolInfoRequest_XML = objRequest.GenerateXML(udtModel)


        Dim sResult As String = String.Empty

        Try
            ' bypass cert validation on callback
            InitServicePointManager()

            Dim objProxy As ProxyClass.PcdForEhsWS

            objProxy = CreateWebServiceEndPoint(GetWS_URL())

            If Not objProxy Is Nothing Then
                ' Log Start
                objAuditLog.AddDescripton("PCD ERN", udtModel.EnrolmentRefNo)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.WriteStartLog(LogID.LOG00001, String.Empty, objRequest.MessageID)

                ' Log Request XML
                objAuditLog.WriteLogData(LogID.LOG00002, "PCDUploadEnrolInfo - eHS Request XML", _PCDUploadEnrolInfoRequest_XML, _
                                         Nothing, Nothing, objRequest.MessageID)

                ' Invoke PCD
                _PCDUploadEnrolInfoResult_XML = objProxy.UploadEnrolInfo(_PCDUploadEnrolInfoRequest_XML, ComConfig.WSRequestSystem.SystemCode)

                ' Log Result XML
                objAuditLog.WriteLogData(LogID.LOG00003, "PCDUploadEnrolInfo - PCD Result XML", _PCDUploadEnrolInfoResult_XML, _
                                         Nothing, Nothing, objRequest.MessageID)
            End If

            objResult = New PCDUploadEnrolInfoResult(_PCDUploadEnrolInfoResult_XML, objRequest.MessageID)

            'If objRequest.PCD_ERN <> String.Empty Then
            '    objResult.PDF_XML_EN = GenPDFXML("EN", objRequest.PCD_ERN, objRequest.EnrolmentSubmissionTime, udtSPModel)
            '    objResult.PDF_XML_TC = GenPDFXML("TC", objRequest.PCD_ERN, objRequest.EnrolmentSubmissionTime, udtSPModel)
            'End If


            objResult.Request = _PCDUploadEnrolInfoRequest_XML

            ' Log End
            objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
            objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
            objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)

        Catch exWeb As System.Net.WebException
            objAuditLog.WriteSystemLog(exWeb, Nothing)

            objResult = New PCDUploadEnrolInfoResult(PCDUploadEnrolInfoResult.enumReturnCode.CommunicationLinkError)
            objResult.Request = _PCDUploadEnrolInfoRequest_XML

            objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
            objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
            objAuditLog.AddDescripton(exWeb)
            objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
        Catch ex As Exception
            objAuditLog.WriteSystemLog(ex, Nothing)

            objResult = New PCDUploadEnrolInfoResult(PCDUploadEnrolInfoResult.enumReturnCode.ErrorAllUnexpected)
            objResult.Request = _PCDUploadEnrolInfoRequest_XML

            objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
            objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
            objAuditLog.AddDescripton(ex)
            objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
        End Try

        objAuditLog.WriteLog(LogID.LOG00005, objRequest.MessageID)
        Return objResult

    End Function


    'Public Function PCDUploadEnrolInfo2(ByVal udtSPModel As ServiceProviderModel) As PCDUploadEnrolInfoResult
    '    Dim objRequest As PCDUploadEnrolInfoRequest
    '    Dim objResult As PCDUploadEnrolInfoResult

    '    objRequest = New PCDUploadEnrolInfoRequest()
    '    _PCDUploadEnrolInfoRequest_XML = objRequest.GenerateXML(udtSPModel)


    '    ' check if it contains errors
    '    If objRequest.HasError Then
    '        objResult = New PCDUploadEnrolInfoResult(PCDUploadEnrolInfoResult.enumReturnCode.ErrorAllUnexpected)
    '        ' copy the error message and code to the result object
    '        objResult.ReturnCodeDesc = objRequest.ErrorMessage
    '        objResult.Request = _PCDUploadEnrolInfoRequest_XML
    '        Return objResult
    '    End If

    '    Dim sResult As String = String.Empty

    '    Try
    '        ' bypass cert validation on callback
    '        InitServicePointManager()

    '        Dim objProxy As ProxyClass.PcdForEhsWS

    '        objProxy = CreateWebServiceEndPoint(GetWS_URL())

    '        If Not objProxy Is Nothing Then
    '            _PCDUploadEnrolInfoResult_XML = objProxy.UploadEnrolInfo(_PCDUploadEnrolInfoRequest_XML)
    '        End If


    '        objResult = New PCDUploadEnrolInfoResult(_PCDUploadEnrolInfoResult_XML, objRequest.MessageID)

    '        If objRequest.PCD_ERN <> String.Empty Then
    '            objResult.PDF_XML_EN = GenPDFXML("EN", objRequest.PCD_ERN, objRequest.EnrolmentSubmissionTime, udtSPModel)
    '            objResult.PDF_XML_TC = GenPDFXML("TC", objRequest.PCD_ERN, objRequest.EnrolmentSubmissionTime, udtSPModel)
    '        End If

    '        objResult.Request = _PCDUploadEnrolInfoRequest_XML

    '        Return objResult
    '    Catch exXML As System.Xml.XmlException
    '        objResult = New PCDUploadEnrolInfoResult(PCDUploadEnrolInfoResult.enumReturnCode.ErrorAllUnexpected)
    '        objResult.Request = _PCDUploadEnrolInfoRequest_XML
    '    Catch exWeb As System.Net.WebException
    '        objResult = New PCDUploadEnrolInfoResult(PCDUploadEnrolInfoResult.enumReturnCode.CommunicationLinkError)
    '        objResult.Request = _PCDUploadEnrolInfoRequest_XML
    '    Catch ex As Exception
    '        objResult = New PCDUploadEnrolInfoResult(PCDUploadEnrolInfoResult.enumReturnCode.ErrorAllUnexpected)
    '        objResult.Request = _PCDUploadEnrolInfoRequest_XML
    '    End Try

    '    Return objResult

    'End Function


    Public Function PCDCreatePCDSPAcct(ByVal udtSPModel As ServiceProviderModel) As PCDCreatePCDSPAcctResult
        Dim objAuditLog As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.PCDCreatePCDSPAcct)
        objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
        objAuditLog.WriteLog(LogID.LOG00000)

        Dim strPlatFormCode As String = GetPlatformCode()

        Dim objRequest As PCDCreatePCDSPAcctRequest
        Dim objResult As PCDCreatePCDSPAcctResult

        objRequest = New PCDCreatePCDSPAcctRequest()
        _PCDCreatePCDSPAcctRequest_XML = objRequest.GenerateXML(udtSPModel, strPlatFormCode)

        ' check if it contains errors
        If objRequest.HasError Then
            objResult = New PCDCreatePCDSPAcctResult(PCDCreatePCDSPAcctResult.enumReturnCode.ErrorAllUnexpected)
            ' copy the error message and code to the result object
            objResult.ReturnCodeDesc = objRequest.ErrorMessage
            objResult.Request = _PCDCreatePCDSPAcctRequest_XML
            Return objResult
        End If

        Dim sResult As String = String.Empty

        Try
            ' bypass cert validation on callback
            InitServicePointManager()

            Dim objProxy As ProxyClass.PcdForEhsWS

            objProxy = CreateWebServiceEndPoint(GetWS_URL())

            If Not objProxy Is Nothing Then

                ' Log Start
                objAuditLog.AddDescripton("SPID", udtSPModel.SPID)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.WriteStartLog(LogID.LOG00001, String.Empty, objRequest.MessageID)

                ' Log Request XML
                objAuditLog.WriteLogData(LogID.LOG00002, "PCDCreatePCDSPAcct - eHS Request XML", _PCDCreatePCDSPAcctRequest_XML, _
                                         Nothing, Nothing, objRequest.MessageID)

                _PCDCreatePCDSPAcctResult_XML = objProxy.CreatePCDSPAcct(_PCDCreatePCDSPAcctRequest_XML, ComConfig.WSRequestSystem.SystemCode)

                ' Log Result XML
                objAuditLog.WriteLogData(LogID.LOG00003, "PCDCreatePCDSPAcct - PCD Result XML", _PCDCreatePCDSPAcctResult_XML, _
                                         Nothing, Nothing, objRequest.MessageID)
            End If


            objResult = New PCDCreatePCDSPAcctResult(_PCDCreatePCDSPAcctResult_XML, objRequest.MessageID)
            objResult.Request = _PCDCreatePCDSPAcctRequest_XML

            ' Log End
            objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
            objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
            objAuditLog.WriteEndLog(LogID.LOG00004, Nothing, objRequest.MessageID)

        Catch exWeb As System.Net.WebException
            objAuditLog.WriteSystemLog(exWeb, Nothing)

            objResult = New PCDCreatePCDSPAcctResult(PCDCreatePCDSPAcctResult.enumReturnCode.CommunicationLinkError)
            objResult.Request = _PCDCreatePCDSPAcctRequest_XML

            objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
            objAuditLog.AddDescripton(exWeb)
            objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
        Catch ex As Exception
            objAuditLog.WriteSystemLog(ex, Nothing)

            objResult = New PCDCreatePCDSPAcctResult(PCDCreatePCDSPAcctResult.enumReturnCode.ErrorAllUnexpected)
            objResult.Request = _PCDCreatePCDSPAcctRequest_XML

            objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
            objAuditLog.AddDescripton(ex)
            objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
        End Try

        objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
        objAuditLog.WriteLogAndMessageID(LogID.LOG00005, objRequest.MessageID)
        Return objResult
    End Function


    Public Function PCDTransferPracticeInfo(ByVal udtSPModel As ServiceProviderModel) As PCDTransferPracticeInfoResult
        Dim objAuditLog As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.PCDTransferPracticeInfo)
        objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
        objAuditLog.WriteLog(LogID.LOG00000)

        Dim objRequest As PCDTransferPracticeInfoRequest = nothing
        Dim objResult As PCDTransferPracticeInfoResult = Nothing

        objRequest = New PCDTransferPracticeInfoRequest()
        _PCDTransferPracticeInfoRequest_XML = objRequest.GenerateXML(udtSPModel)

        ' check if it contains errors
        If objRequest.HasError Then
            objResult = New PCDTransferPracticeInfoResult(PCDTransferPracticeInfoResult.enumReturnCode.ErrorAllUnexpected)
            ' copy the error message and code to the result object
            objResult.ReturnCodeDesc = objRequest.ErrorMessage
            objResult.Request = _PCDTransferPracticeInfoRequest_XML
            Return objResult
        End If

        Dim sResult As String = String.Empty

        Try
            ' bypass cert validation on callback
            InitServicePointManager()

            Dim objProxy As ProxyClass.PcdForEhsWS

            objProxy = CreateWebServiceEndPoint(GetWS_URL())

            If Not objProxy Is Nothing Then
                ' Log Start
                objAuditLog.AddDescripton("SPID", udtSPModel.SPID)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.WriteStartLog(LogID.LOG00001, Nothing, objRequest.MessageID)

                ' Log Request XML
                objAuditLog.WriteLogData(LogID.LOG00002, "PCDTransferPracticeInfo - eHS Request XML", _PCDTransferPracticeInfoRequest_XML, _
                                         Nothing, Nothing, objRequest.MessageID)

                ' Invoke PCD
                _PCDTransferPracticeInfoResult_XML = objProxy.TransferPracticeInfo(_PCDTransferPracticeInfoRequest_XML, ComConfig.WSRequestSystem.SystemCode)

                ' Log Result XML
                objAuditLog.WriteLogData(LogID.LOG00003, "PCDTransferPracticeInfo - PCD Result XML", _PCDTransferPracticeInfoResult_XML, _
                                         Nothing, Nothing, objRequest.MessageID)
            End If

            objResult = New PCDTransferPracticeInfoResult(_PCDTransferPracticeInfoResult_XML, objRequest.MessageID)
            objResult.Request = _PCDTransferPracticeInfoRequest_XML

            ' Log End
            objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
            objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
            objAuditLog.WriteEndLog(LogID.LOG00004, Nothing, objRequest.MessageID)

        Catch exWeb As System.Net.WebException
            objAuditLog.WriteSystemLog(exWeb, Nothing)

            objResult = New PCDTransferPracticeInfoResult(PCDTransferPracticeInfoResult.enumReturnCode.CommunicationLinkError)
            objResult.Request = _PCDTransferPracticeInfoRequest_XML

            objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
            objAuditLog.AddDescripton(exWeb)
            objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
        Catch ex As Exception
            objAuditLog.WriteSystemLog(ex, Nothing)

            objResult = New PCDTransferPracticeInfoResult(PCDTransferPracticeInfoResult.enumReturnCode.ErrorAllUnexpected)
            objResult.Request = _PCDTransferPracticeInfoRequest_XML

            objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
            objAuditLog.AddDescripton(ex)
            objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
        End Try

        objAuditLog.WriteLogAndMessageID(LogID.LOG00005, objRequest.MessageID)
        Return objResult

    End Function

    Public Function PCDCheckIsActiveSP(ByVal strHKID As String) As PCDCheckIsActiveSPResult
        ' using the HKID, build a new ServiceProviderModel object
        Dim udtSP As ServiceProviderModel

        udtSP = New ServiceProviderModel()
        udtSP.HKID = strHKID

        Return PCDCheckIsActiveSP(udtSP)
    End Function

    Public Function PCDCheckIsActiveSP(ByVal udtSP As ServiceProviderModel) As PCDCheckIsActiveSPResult
        Dim objAuditLog As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.PCDCheckIsActiveSP)
        objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
        objAuditLog.WriteLog(LogID.LOG00000)

        Dim objRequest As PCDCheckIsActiveSPRequest
        Dim objResult As PCDCheckIsActiveSPResult
        Dim sResult As String = String.Empty

        objRequest = New PCDCheckIsActiveSPRequest()
        _PCDCheckIsActiveSPRequest_XML = objRequest.GenerateXML(udtSP)

        ' check if it contains errors
        If objRequest.HasError Then
            objResult = New PCDCheckIsActiveSPResult(PCDCheckIsActiveSPResult.enumReturnCode.ErrorAllUnexpected)
            ' copy the error message and code to the result object
            objResult.ReturnCodeDesc = objRequest.ErrorMessage
            objResult.Request = _PCDCheckIsActiveSPRequest_XML
        Else

            Try
                ' bypass cert validation on callback
                InitServicePointManager()

                Dim objProxy As ProxyClass.PcdForEhsWS

                objProxy = CreateWebServiceEndPoint(GetWS_URL())

                If Not objProxy Is Nothing Then
                    ' Log Start
                    objAuditLog.AddDescripton("SPID", udtSP.SPID)
                    objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                    objAuditLog.WriteStartLog(LogID.LOG00001, String.Empty, objRequest.MessageID)

                    ' Log Request XML
                    objAuditLog.WriteLogData(LogID.LOG00002, "PCDCheckIsActiveSP - eHS Request XML", _PCDCheckIsActiveSPRequest_XML, _
                                             Nothing, Nothing, objRequest.MessageID)

                    _PCDCheckIsActiveSPResult_XML = objProxy.CheckIsActiveSP(_PCDCheckIsActiveSPRequest_XML, ComConfig.WSRequestSystem.SystemCode)

                    ' Log Result XML
                    objAuditLog.WriteLogData(LogID.LOG00003, "PCDCheckIsActiveSP - PCD Result XML", _PCDCheckIsActiveSPResult_XML, _
                                             Nothing, Nothing, objRequest.MessageID)
                End If

                objResult = New PCDCheckIsActiveSPResult(_PCDCheckIsActiveSPResult_XML, objRequest.MessageID)
                objResult.Request = _PCDCheckIsActiveSPRequest_XML

                ' Log End
                objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.WriteEndLog(LogID.LOG00004, Nothing, objRequest.MessageID)

            Catch exWeb As System.Net.WebException
                objAuditLog.WriteSystemLog(exWeb, Nothing)

                objResult = New PCDCheckIsActiveSPResult(PCDCheckIsActiveSPResult.enumReturnCode.CommunicationLinkError)
                objResult.Request = _PCDCheckIsActiveSPRequest_XML

                objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.AddDescripton(exWeb)
                objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
            Catch ex As Exception
                objAuditLog.WriteSystemLog(ex, Nothing)

                objResult = New PCDCheckIsActiveSPResult(PCDCheckIsActiveSPResult.enumReturnCode.ErrorAllUnexpected)
                objResult.Request = _PCDCheckIsActiveSPRequest_XML

                objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.AddDescripton(ex)
                objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
            End Try
        End If

        objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
        objAuditLog.WriteLogAndMessageID(LogID.LOG00005, objRequest.MessageID)
        Return objResult

    End Function

    Public Function PCDCheckAccountStatus(ByVal strHKID As String) As PCDCheckAccountStatusResult
        ' using the HKID, build a new ServiceProviderModel object
        Dim udtSP As ServiceProviderModel

        udtSP = New ServiceProviderModel()
        udtSP.HKID = strHKID

        Return PCDCheckAccountStatus(udtSP)
    End Function

    Public Function PCDCheckAccountStatus(ByVal udtSP As ServiceProviderModel) As PCDCheckAccountStatusResult
        'Dim objResult As PCDCheckAccountStatusResult = Nothing

        Dim objAuditLog As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.PCDCheckAccountStatus)
        objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
        objAuditLog.WriteLog(LogID.LOG00000)

        Dim objRequest As PCDCheckAccountStatusRequest
        Dim objResult As PCDCheckAccountStatusResult
        Dim sResult As String = String.Empty

        objRequest = New PCDCheckAccountStatusRequest()
        _PCDCheckAccountStatusRequest_XML = objRequest.GenerateXML(udtSP)

        ' check if it contains errors
        If objRequest.HasError Then
            objResult = New PCDCheckAccountStatusResult(PCDCheckAccountStatusResult.enumReturnCode.ErrorAllUnexpected)
            ' copy the error message and code to the result object
            'objResult.ReturnCodeDesc = objRequest.ErrorMessage
            objResult.Request = _PCDCheckAccountStatusRequest_XML
        Else

            Try
                ' bypass cert validation on callback
                InitServicePointManager()

                Dim objProxy As ProxyClass.PcdForEhsWS

                objProxy = CreateWebServiceEndPoint(GetWS_URL())

                If Not objProxy Is Nothing Then
                    ' Log Start
                    objAuditLog.AddDescripton("SPID", udtSP.SPID)
                    objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                    objAuditLog.WriteStartLog(LogID.LOG00001, String.Empty, objRequest.MessageID)

                    ' Log Request XML
                    objAuditLog.WriteLogData(LogID.LOG00002, "PCDCheckAccountStatus - eHS Request XML", _PCDCheckAccountStatusRequest_XML, _
                                             Nothing, Nothing, objRequest.MessageID)

                    _PCDCheckAccountStatusResult_XML = objProxy.CheckAccountStatus(_PCDCheckAccountStatusRequest_XML, ComConfig.WSRequestSystem.SystemCode)

                    ' Log Result XML
                    objAuditLog.WriteLogData(LogID.LOG00003, "PCDCheckAccountStatus - PCD Result XML", _PCDCheckAccountStatusResult_XML, _
                                             Nothing, Nothing, objRequest.MessageID)
                End If

                objResult = New PCDCheckAccountStatusResult(_PCDCheckAccountStatusResult_XML, objRequest.MessageID)
                objResult.Request = _PCDCheckAccountStatusRequest_XML

                ' Log End
                objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.WriteEndLog(LogID.LOG00004, Nothing, objRequest.MessageID)

            Catch exWeb As System.Net.WebException
                objAuditLog.WriteSystemLog(exWeb, Nothing)

                objResult = New PCDCheckAccountStatusResult(PCDCheckAccountStatusResult.enumReturnCode.CommunicationLinkError)
                objResult.Request = _PCDCheckAccountStatusRequest_XML

                objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.AddDescripton(exWeb)
                objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
            Catch ex As Exception
                objAuditLog.WriteSystemLog(ex, Nothing)

                objResult = New PCDCheckAccountStatusResult(PCDCheckAccountStatusResult.enumReturnCode.ErrorAllUnexpected)
                objResult.Request = _PCDCheckIsActiveSPRequest_XML

                objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.AddDescripton(ex)
                objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
            End Try
        End If

        objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
        objAuditLog.WriteLogAndMessageID(LogID.LOG00005, objRequest.MessageID)
        Return objResult


        Return objResult
    End Function

    Public Function CreateEnrolRecord(ByVal udtSP As ServiceProviderModel, ByVal udtDB As Database) As ThirdPartyEnrollRecordModel
        Dim objGenFunc As New GeneralFunction
        Dim objRequest As PCDUploadEnrolInfoRequest
        Dim objEnrolRecord As ThirdPartyEnrollRecordModel

        Dim strPlatFormCode As String = GetPlatformCode()

        objRequest = New PCDUploadEnrolInfoRequest()
        _PCDUploadEnrolInfoRequest_XML = objRequest.GenerateXML(udtSP, strPlatFormCode, True)

        objEnrolRecord = New ThirdPartyEnrollRecordModel()
        objEnrolRecord.SysCode = ThirdPartyEnrollRecordModel.EnumSysCode.PCD
        objEnrolRecord.EnrolmentRefNo = objRequest.PCD_ERN
        objEnrolRecord.EnrolmentSubmissionDate = objGenFunc.GetSystemDateTime()
        objEnrolRecord.Data = _PCDUploadEnrolInfoRequest_XML
        objEnrolRecord.CreateBy = "System"
        objEnrolRecord.UpdateBy = "System"

        If ThirdPartyBLL.AddThirdPartyEnrollRecord(objEnrolRecord, udtDB) Then
            Return objEnrolRecord
        Else
            Return Nothing
        End If
    End Function

    Public Function PCDCheckAvailableForVerifiedEnrolment(ByVal udtSP As ServiceProviderModel) As PCDCheckAvailableForVerifiedEnrolmentResult
        Dim objAuditLog As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.PCDCheckAvailableForVerifiedEnrolment)
        objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
        objAuditLog.WriteLog(LogID.LOG00000)

        Dim objRequest As PCDCheckAvailableForVerifiedEnrolmentRequest
        Dim objResult As PCDCheckAvailableForVerifiedEnrolmentResult
        Dim sResult As String = String.Empty

        objRequest = New PCDCheckAvailableForVerifiedEnrolmentRequest()
        _PCDCheckAvailableForVerifiedEnrolmentRequest_XML = objRequest.GenerateXML(udtSP)

        ' check if it contains errors
        If objRequest.HasError Then
            objResult = New PCDCheckAvailableForVerifiedEnrolmentResult(PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.ErrorAllUnexpected)
            ' copy the error message and code to the result object
            objResult.ReturnCodeDesc = objRequest.ErrorMessage
            objResult.Request = _PCDCheckAvailableForVerifiedEnrolmentRequest_XML
        Else

            Try
                ' bypass cert validation on callback
                InitServicePointManager()

                Dim objProxy As ProxyClass.PcdForEhsWS

                objProxy = CreateWebServiceEndPoint(GetWS_URL())

                If Not objProxy Is Nothing Then
                    ' Log Start
                    objAuditLog.AddDescripton("SPID", udtSP.SPID)
                    objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                    objAuditLog.WriteStartLog(LogID.LOG00001, String.Empty, objRequest.MessageID)

                    ' Log Request XML
                    objAuditLog.WriteLogData(LogID.LOG00002, "PCDCheckAvailableForVerifiedEnrolment - eHS Request XML", _PCDCheckAvailableForVerifiedEnrolmentRequest_XML, _
                                             Nothing, Nothing, objRequest.MessageID)

                    _PCDCheckAvailableForVerifiedEnrolmentResult_XML = objProxy.CheckAvailableForVerifiedEnrolment(_PCDCheckAvailableForVerifiedEnrolmentRequest_XML, ComConfig.WSRequestSystem.SystemCode)

                    ' Log Result XML
                    objAuditLog.WriteLogData(LogID.LOG00003, "PCDCheckAvailableForVerifiedEnrolment - PCD Result XML", _PCDCheckAvailableForVerifiedEnrolmentResult_XML, _
                                             Nothing, Nothing, objRequest.MessageID)
                End If

                objResult = New PCDCheckAvailableForVerifiedEnrolmentResult(_PCDCheckAvailableForVerifiedEnrolmentResult_XML, objRequest.MessageID)
                objResult.Request = _PCDCheckAvailableForVerifiedEnrolmentRequest_XML

                ' Log End
                objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.WriteEndLog(LogID.LOG00004, Nothing, objRequest.MessageID)

            Catch exWeb As System.Net.WebException
                objAuditLog.WriteSystemLog(exWeb, Nothing)

                objResult = New PCDCheckAvailableForVerifiedEnrolmentResult(PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.CommunicationLinkError)
                objResult.Request = _PCDCheckAvailableForVerifiedEnrolmentRequest_XML

                objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.AddDescripton(exWeb)
                objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
            Catch ex As Exception
                objAuditLog.WriteSystemLog(ex, Nothing)

                objResult = New PCDCheckAvailableForVerifiedEnrolmentResult(PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.ErrorAllUnexpected)
                objResult.Request = _PCDCheckAvailableForVerifiedEnrolmentRequest_XML

                objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.AddDescripton(ex)
                objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
            End Try
        End If

        objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
        objAuditLog.WriteLogAndMessageID(LogID.LOG00005, objRequest.MessageID)
        Return objResult

    End Function

    Public Function PCDUploadVerifiedEnrolment(ByVal udtSP As ServiceProviderModel) As PCDUploadVerifiedEnrolmentResult
        Dim objAuditLog As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.PCDUploadVerifiedEnrolment)
        objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
        objAuditLog.WriteLog(LogID.LOG00000)

        Dim strPlatFormCode As String = GetPlatformCode()

        Dim objRequest As PCDUploadVerifiedEnrolmentRequest
        Dim objResult As PCDUploadVerifiedEnrolmentResult

        objRequest = New PCDUploadVerifiedEnrolmentRequest()
        _PCDUploadVerifiedEnrolmentRequest_XML = objRequest.GenerateXML(udtSP, strPlatFormCode)


        Dim sResult As String = String.Empty

        Try
            ' bypass cert validation on callback
            InitServicePointManager()

            Dim objProxy As ProxyClass.PcdForEhsWS

            objProxy = CreateWebServiceEndPoint(GetWS_URL())

            If Not objProxy Is Nothing Then
                ' Log Start
                objAuditLog.AddDescripton("SPID", udtSP.SPID)
                objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
                objAuditLog.WriteStartLog(LogID.LOG00001, String.Empty, objRequest.MessageID)

                ' Log Request XML
                objAuditLog.WriteLogData(LogID.LOG00002, "PCDUploadVerifiedEnrolment - eHS Request XML", _PCDUploadVerifiedEnrolmentRequest_XML, _
                                         Nothing, Nothing, objRequest.MessageID)

                ' Invoke PCD
                _PCDUploadVerifiedEnrolmentResult_XML = objProxy.UploadVerifiedEnrolment(_PCDUploadVerifiedEnrolmentRequest_XML, ComConfig.WSRequestSystem.SystemCode)

                ' Log Result XML
                objAuditLog.WriteLogData(LogID.LOG00003, "PCDUploadVerifiedEnrolment - PCD Result XML", _PCDUploadVerifiedEnrolmentResult_XML, _
                                         Nothing, Nothing, objRequest.MessageID)
            End If

            objResult = New PCDUploadVerifiedEnrolmentResult(_PCDUploadVerifiedEnrolmentResult_XML, objRequest.MessageID)

            objResult.Request = _PCDUploadVerifiedEnrolmentRequest_XML

            ' Log End
            objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
            objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
            objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)

        Catch exWeb As System.Net.WebException
            objAuditLog.WriteSystemLog(exWeb, Nothing)

            objResult = New PCDUploadVerifiedEnrolmentResult(PCDUploadVerifiedEnrolmentResult.enumReturnCode.CommunicationLinkError)
            objResult.Request = _PCDUploadEnrolInfoRequest_XML

            objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
            objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
            objAuditLog.AddDescripton(exWeb)
            objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
        Catch ex As Exception
            objAuditLog.WriteSystemLog(ex, Nothing)

            objResult = New PCDUploadVerifiedEnrolmentResult(PCDUploadVerifiedEnrolmentResult.enumReturnCode.ErrorAllUnexpected)
            objResult.Request = _PCDUploadEnrolInfoRequest_XML

            objAuditLog.AddDescripton("ReturnCode", objResult.ReturnCode)
            objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
            objAuditLog.AddDescripton(ex)
            objAuditLog.WriteEndLog(LogID.LOG00004, String.Empty, objRequest.MessageID)
        End Try

        objAuditLog.AddDescripton("SourceFunction", Me.SourceFunctionCode)
        objAuditLog.WriteLogAndMessageID(LogID.LOG00005, objRequest.MessageID)
        Return objResult

    End Function
    'Public Function PCDHealthCheck() As PCDHealthCheckResult

    '    Dim objRequest As PCDHealthCheckRequest
    '    Dim objResult As PCDHealthCheckResult

    '    objRequest = New PCDHealthCheckRequest()
    '    _PCDHealthCheckRequest_XML = objRequest.GenerateXML()

    '    ' check if it contains errors
    '    If objRequest.HasError Then
    '        objResult = New PCDHealthCheckResult(PCDHealthCheckResult.enumReturnCode.ErrorAllUnexpected)
    '        ' copy the error message and code to the result object
    '        objResult.ReturnCodeDesc = objRequest.ErrorMessage
    '        objResult.Request = _PCDHealthCheckRequest_XML
    '        Return objResult
    '    End If

    '    Dim sResult As String = String.Empty

    '    Try
    '        ' bypass cert validation on callback
    '        InitServicePointManager()

    '        Dim objProxy As ProxyClass.PcdForEhsWS

    '        objProxy = CreateWebServiceEndPoint(GetWS_URL())

    '        If Not objProxy Is Nothing Then
    '            _PCDHealthCheckResult_XML = objProxy.HealthCheck(_PCDHealthCheckRequest_XML)
    '        End If

    '        objResult = New PCDHealthCheckResult(_PCDHealthCheckResult_XML, objRequest.MessageID)
    '        objResult.Request = _PCDHealthCheckRequest_XML

    '        Return objResult
    '    Catch exXML As System.Xml.XmlException
    '        objResult = New PCDHealthCheckResult(PCDHealthCheckResult.enumReturnCode.ErrorAllUnexpected)
    '        objResult.Request = _PCDHealthCheckRequest_XML
    '    Catch exWeb As System.Net.WebException
    '        objResult = New PCDHealthCheckResult(PCDHealthCheckResult.enumReturnCode.CommunicationLinkError)
    '        objResult.Request = _PCDHealthCheckRequest_XML
    '    Catch ex As Exception
    '        objResult = New PCDHealthCheckResult(PCDHealthCheckResult.enumReturnCode.ErrorAllUnexpected)
    '        objResult.Request = _PCDHealthCheckRequest_XML
    '    End Try

    '    Return objResult

    'End Function

    Private Function GenPDFXML(ByVal strLang As String, ByVal strPCD_ERN As String, ByVal dtmSubmissionTime As DateTime, ByVal udtSP As ServiceProviderModel) As String
        Dim objXmlGenerator As WSXmlGenerator
        Dim objGenPDF As WSXmlGenerator.GenPDF

        objXmlGenerator = New WSXmlGenerator()
        objGenPDF = New WSXmlGenerator.GenPDF

        ' append parent node
        Dim nodeROOT As XmlElement
        Dim xmlDeclaration As XmlDeclaration = objXmlGenerator.XML.CreateXmlDeclaration("1.0", "utf-8", Nothing)

        ' clear the XML document
        objXmlGenerator.XML.RemoveAll()

        nodeROOT = objXmlGenerator.XML.CreateElement(objGenPDF.TAGROOT)
        objXmlGenerator.XML.InsertBefore(xmlDeclaration, objXmlGenerator.XML.DocumentElement)
        objXmlGenerator.XML.AppendChild(nodeROOT)

        objGenPDF.GenerateXMLPDFAttributes(objXmlGenerator.XML, nodeROOT, strLang)
        objGenPDF.GenerateXMLPDFServiceProvider(objXmlGenerator.XML, nodeROOT, strPCD_ERN, dtmSubmissionTime, udtSP)
        objGenPDF.GenerateXMLPDFMessageDateTime(objXmlGenerator.XML, nodeROOT)

        Return objXmlGenerator.XML.InnerXml
    End Function



    Private Function CreateWebServiceEndPoint(ByVal strURL As String) As ProxyClass.PcdForEhsWS
        Dim ws As ProxyClass.PcdForEhsWS = New ProxyClass.PcdForEhsWS
        ws.Url = strURL

        ' Create Client Policy (Specify that the policy uses the username over transport security assertion)
        Dim strUserName As String = GetWS_Username()
        Dim strPassword As String = GetWS_Password()

        If strUserName <> String.Empty And strPassword <> String.Empty Then
            ws.ServiceAuthHeaderValue = New ProxyClass.ServiceAuthHeader
            ws.ServiceAuthHeaderValue.Username = strUserName
            ws.ServiceAuthHeaderValue.Password = strPassword
            'Dim oClientPolicy As New Microsoft.Web.Services3.Design.Policy()
            'Dim oAssertion As UsernameOverTransportAssertion = New UsernameOverTransportAssertion()
            'oAssertion.UsernameTokenProvider = New Microsoft.Web.Services3.Design.UsernameTokenProvider(strUserName, strPassword)
            'oClientPolicy.Assertions.Add(oAssertion)
            'ws.SetPolicy(oClientPolicy)

            ' Use windows proxy for access endpoint
            If GetWS_UseProxy() Then
                ws.Proxy = New System.Net.WebProxy()
            End If
        End If

        ws.Timeout = GetWS_Timeout()
        Return ws
    End Function



    Private Function GetWS_Username() As String
        Dim oGenFunc As New GeneralFunction()
        Dim sValue As String = String.Empty

        oGenFunc.getSystemParameter(PCD_INTEGRATION_WEB_SERVICE_USER, sValue, Nothing)
        Return sValue
    End Function

    Private Function GetWS_Password() As String
        Dim oGenFunc As New GeneralFunction()
        Dim sValue As String = String.Empty

        oGenFunc.getSystemParameterPassword(PCD_INTEGRATION_WEB_SERVICE_PASSWORD, sValue)
        Return sValue
    End Function

    Private Function GetWS_URL() As String
        Dim oGenFunc As New GeneralFunction()
        Dim sValue As String = String.Empty

        oGenFunc.getSystemParameter(PCD_INTEGRATION_WEB_SERVICE_URL, sValue, Nothing)
        Return sValue
    End Function

    Private Function GetWS_Timeout() As Integer
        Dim oGenFunc As New GeneralFunction()
        Dim sValue As String = String.Empty
        oGenFunc.getSystemParameter(PCD_INTEGRATION_WEB_SERVICE_TIMELIMIT, sValue, Nothing)

        Return CInt(sValue) * 1000
    End Function

    Private Function GetWS_UseProxy() As Boolean
        Dim oGenFunc As New GeneralFunction()
        Dim sValue As String = String.Empty
        oGenFunc.getSystemParameter(PCD_INTEGRATION_WEB_SERVICE_USE_PROXY, sValue, String.Empty)

        Return sValue = "Y"
    End Function

    Private Sub InitServicePointManager()
        Dim callback As New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
        System.Net.ServicePointManager.ServerCertificateValidationCallback = callback
    End Sub


    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean

        'Return True to force the certificate to be accepted.

        Return True

    End Function

    Private Function GetPlatformCode() As String
        Dim strPlatFormCode As String = System.Configuration.ConfigurationManager.AppSettings("Platform")

        ' Map the platform code
        Dim udtCodeMapList As CodeMappingCollection
        Dim udtCodeMap As CodeMappingModel
        udtCodeMapList = CodeMappingBLL.GetAllCodeMapping
        udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.PCD, CodeMappingModel.EnumTargetSystem.EHS, EnumConstant.EnumMappingCodeType.WS_PCD_Platform_Code.ToString(), strPlatFormCode)
        If Not udtCodeMap Is Nothing Then
            strPlatFormCode = udtCodeMap.CodeTarget
        End If

        Return strPlatFormCode
    End Function
End Class
