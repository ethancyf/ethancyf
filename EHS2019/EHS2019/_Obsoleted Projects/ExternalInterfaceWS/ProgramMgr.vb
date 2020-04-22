Imports ExternalInterfaceWS.Cryptography
Imports ExternalInterfaceWS
Imports System.Xml
Imports ExternalInterfaceWS.Component.Request
Imports ExternalInterfaceWS.Component.Response
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports Common.ComObject
Imports Common.Component
Imports ExternalInterfaceWS.Component
Imports ExternalInterfaceWS.ComObject

Public Class ProgramMgr


    Public Shared Function UploadClaim_HL7(ByVal xmlRequest As String, ByVal SystemName As String) As String

        Dim AuditLog As ExtAuditLogEntry = ComFunction.GetAuditLogEntry(EnumAuditLog.UploadClaim)
        AuditLog.IncomingSystemID = SystemName
        AuditLog.WriteLog_Ext(LogID.LOG00001)

        '------------------------------------------------------------------------
        ' Check the request is from an authorized party
        '------------------------------------------------------------------------
        If Not CheckAuthorizedExternalParty(SystemName) Then
            AuditLog.WriteLog_Ext(LogID.LOG00080)
            Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I00001), SystemName)
        End If

        '------------------------------------------------------------------------
        ' Resolve request XML
        '------------------------------------------------------------------------
        Dim strEnableSecuredWS As String
        strEnableSecuredWS = AppConfigMgr.getEnableSecuredWS()
        If strEnableSecuredWS.Trim.ToUpper = "Y" Then
            Try
                xmlRequest = SecurityHelper.ExtractContentFromSecuredXML(xmlRequest, SystemName)
            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.UploadClaim).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I00002), SystemName)  ''Fail to decrypt or verify the signature of input XML
            End Try
        End If
        '------------------------------------------------------------------------

        Try
            AuditLog.AddDescripton_Ext("SystemName", SystemName)

            Dim intMaxCharForAudit As Integer = 0
            Dim strMaxCharForAudit As String = String.Empty
            Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
            udtGeneralFunction.getSystemParameter("ExtWSMaxCharForAuditLogInOneRec", strMaxCharForAudit, String.Empty)
            If Not Integer.TryParse(strMaxCharForAudit, intMaxCharForAudit) Then
                intMaxCharForAudit = 3900
            End If

            If xmlRequest.Length > intMaxCharForAudit Then
                AuditLog.WriteLogData_Ext(LogID.LOG00013, xmlRequest.Substring(0, intMaxCharForAudit))
                If xmlRequest.Length > (intMaxCharForAudit * 2) Then
                    AuditLog.WriteLogData_Ext(LogID.LOG00013, xmlRequest.Substring(intMaxCharForAudit, intMaxCharForAudit))
                    AuditLog.WriteLogData_Ext(LogID.LOG00013, xmlRequest.Substring(intMaxCharForAudit * 2))
                Else
                    AuditLog.WriteLogData_Ext(LogID.LOG00013, xmlRequest.Substring(intMaxCharForAudit))
                End If
            Else
                AuditLog.WriteLogData_Ext(LogID.LOG00013, xmlRequest)
            End If


            Dim request As New UploadClaimRequest_HL7(xmlRequest, AuditLog)
            Dim response As New UploadClaimResponse

            response.ProcessRequest(request, SystemName, AuditLog)
            Dim xml As XmlDocument = response.GenXMLResult()

            AuditLog.WriteLogData_Ext(LogID.LOG00014, xml.InnerXml)

            '------------------------------------------------------------------------
            'Encrypt and Sign response XML
            '------------------------------------------------------------------------
            Dim xmlResponse As String = xml.InnerXml
            If strEnableSecuredWS.Trim.ToUpper = "Y" Then
                xmlResponse = SecurityHelper.CreateSecuredXMLFromPlainXML(xmlResponse, SystemName)
            End If
            '------------------------------------------------------------------------

            AuditLog.WriteEndLog_Ext(LogID.LOG00002, response.ReturnErrorCodes.RetrieveMessageCodeListForAuditLog())

            Return xmlResponse
            'Return "XML inject successfully"
        Catch ex As Exception
            ErrorLogHandler.getInstance(EnumAuditLog.UploadClaim).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
            Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I99999), SystemName)   'internal Error
        End Try

    End Function

    Public Shared Function GetReasonForVisitList(ByVal xmlRequest As String, ByVal SystemName As String) As String

        Dim AuditLog As ExtAuditLogEntry = ComFunction.GetAuditLogEntry(EnumAuditLog.GetReasonForVisitList)
        AuditLog.IncomingSystemID = SystemName
        AuditLog.WriteLog_Ext(LogID.LOG00003)

        '------------------------------------------------------------------------
        ' Check the request is from an authorized party
        '------------------------------------------------------------------------
        If Not CheckAuthorizedExternalParty(SystemName) Then
            AuditLog.WriteLog_Ext(LogID.LOG00080)
            Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I00001), SystemName)
        End If

        '------------------------------------------------------------------------
        ' Resolve request XML
        '------------------------------------------------------------------------
        Dim strEnableSecuredWS As String = String.Empty
        strEnableSecuredWS = AppConfigMgr.getEnableSecuredWS()
        If strEnableSecuredWS.Trim.ToUpper = "Y" Then
            Try
                xmlRequest = SecurityHelper.ExtractContentFromSecuredXML(xmlRequest, SystemName)
            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.GetReasonForVisitList).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I00002), SystemName)  ''Fail to decrypt or verify the signature of input XML
            End Try
        End If
        '------------------------------------------------------------------------

        Try
            AuditLog.AddDescripton_Ext("SystemName", SystemName)
            AuditLog.WriteLogData_Ext(LogID.LOG00015, xmlRequest)

            Dim response As New GetReasonForVisitListResponse()

            response.ProcessRequest(AuditLog)
            Dim xml As XmlDocument = response.GenXMLResult()

            'AuditLog.WriteLogData_Ext(LogID.LOG00016, xml.InnerText)
            '------------------------------------------------------------------------
            'Encrypt and Sign response XML
            '------------------------------------------------------------------------
            strEnableSecuredWS = AppConfigMgr.getEnableSecuredWS()
            Dim xmlResponse As String = xml.InnerXml
            If strEnableSecuredWS.Trim.ToUpper = "Y" Then
                xmlResponse = SecurityHelper.CreateSecuredXMLFromPlainXML(xmlResponse, SystemName)
            End If
            '------------------------------------------------------------------------

            AuditLog.WriteEndLog_Ext(LogID.LOG00004, response.ReturnErrorCodes.RetrieveMessageCodeListForAuditLog())

            Return xmlResponse

        Catch ex As Exception
            ErrorLogHandler.getInstance(EnumAuditLog.GetReasonForVisitList).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
            Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I99999), SystemName)   'internal Error
        End Try
    End Function

    Public Shared Function RCHNameEnquiry(ByVal xmlRequest As String, ByVal SystemName As String) As String

        Dim AuditLog As ExtAuditLogEntry = ComFunction.GetAuditLogEntry(EnumAuditLog.RCHNameQuery)
        AuditLog.IncomingSystemID = SystemName
        AuditLog.WriteLog_Ext(LogID.LOG00005)

        '------------------------------------------------------------------------
        ' Check the request is from an authorized party
        '------------------------------------------------------------------------
        If Not CheckAuthorizedExternalParty(SystemName) Then
            AuditLog.WriteLog_Ext(LogID.LOG00080)
            Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I00001), SystemName)
        End If

        '------------------------------------------------------------------------
        ' Resolve request XML
        '------------------------------------------------------------------------
        Dim strEnableSecuredWS As String = String.Empty
        strEnableSecuredWS = AppConfigMgr.getEnableSecuredWS()
        If strEnableSecuredWS.Trim.ToUpper = "Y" Then
            Try
                xmlRequest = SecurityHelper.ExtractContentFromSecuredXML(xmlRequest, SystemName)
            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.RCHNameQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I00002), SystemName)  ''Fail to decrypt or verify the signature of input XML
            End Try
        End If
        '------------------------------------------------------------------------
        Try
            AuditLog.AddDescripton_Ext("SystemName", SystemName)
            AuditLog.WriteLogData_Ext(LogID.LOG00017, xmlRequest)

            Dim request As New RCHNameQueryRequest(xmlRequest, AuditLog)
            Dim response As New RCHNameQueryResponse

            response.ProcessRequest(request, SystemName, AuditLog)
            Dim xml As XmlDocument = response.GenXMLResult()

            AuditLog.WriteLogData_Ext(LogID.LOG00018, xml.InnerXml)

            '------------------------------------------------------------------------
            'Encrypt and Sign response XML
            '------------------------------------------------------------------------
            Dim xmlResponse As String = xml.InnerXml
            If strEnableSecuredWS.Trim.ToUpper = "Y" Then
                xmlResponse = SecurityHelper.CreateSecuredXMLFromPlainXML(xmlResponse, SystemName)
            End If
            '------------------------------------------------------------------------

            AuditLog.WriteEndLog_Ext(LogID.LOG00006, response.ReturnErrorCodes.RetrieveMessageCodeListForAuditLog())

            Return xmlResponse

        Catch ex As Exception
            ErrorLogHandler.getInstance(EnumAuditLog.RCHNameQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
            Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I99999), SystemName)   'internal Error
        End Try
    End Function

    Public Shared Function eHSValidatedAccountQuery(ByVal xmlRequest As String, ByVal SystemName As String) As String

        Dim AuditLog As ExtAuditLogEntry = ComFunction.GetAuditLogEntry(EnumAuditLog.eHSValidatedAccountQuery)
        AuditLog.IncomingSystemID = SystemName
        AuditLog.WriteLog_Ext(LogID.LOG00007)

        '------------------------------------------------------------------------
        ' Check the request is from an authorized party
        '------------------------------------------------------------------------
        If Not CheckAuthorizedExternalParty(SystemName) Then
            AuditLog.WriteLog_Ext(LogID.LOG00080)
            Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I00001), SystemName)
        End If

        '------------------------------------------------------------------------
        ' Resolve request XML
        '------------------------------------------------------------------------
        Dim strEnableSecuredWS As String = String.Empty
        strEnableSecuredWS = AppConfigMgr.getEnableSecuredWS()
        If strEnableSecuredWS.Trim.ToUpper = "Y" Then
            Try
                xmlRequest = SecurityHelper.ExtractContentFromSecuredXML(xmlRequest, SystemName)
            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.eHSValidatedAccountQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I00002), SystemName)  ''Fail to decrypt or verify the signature of input XML
            End Try
        End If
        '------------------------------------------------------------------------

        Try
            AuditLog.AddDescripton_Ext("SystemName", SystemName)
            AuditLog.WriteLogData_Ext(LogID.LOG00019, xmlRequest)

            Dim request As New eHSValidatedAccountQueryRequest(xmlRequest, AuditLog)
            Dim response As New eHSValidatedAccountQueryResponse

            response.ProcessRequest(request, SystemName, AuditLog)
            Dim xml As XmlDocument = response.GenXMLResult()

            AuditLog.WriteLogData_Ext(LogID.LOG00020, xml.InnerXml)
            '------------------------------------------------------------------------
            'Encrypt and Sign response XML
            '------------------------------------------------------------------------
            Dim xmlResponse As String = xml.InnerXml
            If strEnableSecuredWS.Trim.ToUpper = "Y" Then
                xmlResponse = SecurityHelper.CreateSecuredXMLFromPlainXML(xmlResponse, SystemName)
            End If
            '------------------------------------------------------------------------

            AuditLog.WriteEndLog_Ext(LogID.LOG00008, response.ReturnErrorCodes.RetrieveMessageCodeListForAuditLog())

            Return xmlResponse

        Catch ex As Exception
            ErrorLogHandler.getInstance(EnumAuditLog.eHSValidatedAccountQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
            Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I99999), SystemName)   'internal Error
        End Try

    End Function

    Public Shared Function eHSAccountVoucherQuery(ByVal xmlRequest As String, ByVal SystemName As String) As String

        Dim AuditLog As ExtAuditLogEntry = ComFunction.GetAuditLogEntry(EnumAuditLog.eHSAccountSubsidyQuery)
        AuditLog.IncomingSystemID = SystemName
        AuditLog.WriteLog_Ext(LogID.LOG00009)

        '------------------------------------------------------------------------
        ' Check the request is from an authorized party
        '------------------------------------------------------------------------
        If Not CheckAuthorizedExternalParty(SystemName) Then
            AuditLog.WriteLog_Ext(LogID.LOG00080)
            Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I00001), SystemName)
        End If

        '------------------------------------------------------------------------
        ' Resolve request XML
        '------------------------------------------------------------------------
        Dim strEnableSecuredWS As String = String.Empty
        strEnableSecuredWS = AppConfigMgr.getEnableSecuredWS()
        If strEnableSecuredWS.Trim.ToUpper = "Y" Then
            Try
                xmlRequest = SecurityHelper.ExtractContentFromSecuredXML(xmlRequest, SystemName)
            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.eHSValidatedAccountQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I00002), SystemName)  ''Fail to decrypt or verify the signature of input XML
            End Try
        End If
        '------------------------------------------------------------------------

        Try
            AuditLog.AddDescripton_Ext("SystemName", SystemName)
            AuditLog.WriteLogData_Ext(LogID.LOG00022, xmlRequest)

            Dim request As New eHSAccountSubsidyQueryRequest(xmlRequest, AuditLog)
            Dim response As New eHSAccountSubsidyQueryResponse

            response.ProcessRequest(request, SystemName, AuditLog)
            Dim xml As XmlDocument = response.GenXMLResult()

            AuditLog.WriteLogData_Ext(LogID.LOG00022, xml.InnerXml)
            '------------------------------------------------------------------------
            'Encrypt and Sign response XML
            '------------------------------------------------------------------------
            Dim xmlResponse As String = xml.InnerXml
            If strEnableSecuredWS.Trim.ToUpper = "Y" Then
                xmlResponse = SecurityHelper.CreateSecuredXMLFromPlainXML(xmlResponse, SystemName)
            End If
            '------------------------------------------------------------------------

            AuditLog.WriteEndLog_Ext(LogID.LOG00010, response.ReturnErrorCodes.RetrieveMessageCodeListForAuditLog())

            Return xmlResponse

        Catch ex As Exception
            ErrorLogHandler.getInstance(EnumAuditLog.eHSAccountSubsidyQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
            Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I99999), SystemName)   'internal Error
        End Try

    End Function

    Public Shared Function SPPracticeValidation(ByVal xmlRequest As String, ByVal SystemName As String) As String

        Dim AuditLog As ExtAuditLogEntry = ComFunction.GetAuditLogEntry(EnumAuditLog.SPPracticeValidation)
        AuditLog.IncomingSystemID = SystemName
        AuditLog.WriteLog_Ext(LogID.LOG00011)

        '------------------------------------------------------------------------
        ' Check the request is from an authorized party
        '------------------------------------------------------------------------
        If Not CheckAuthorizedExternalParty(SystemName) Then
            AuditLog.WriteLog_Ext(LogID.LOG00080)
            Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I00001), SystemName)
        End If

        '------------------------------------------------------------------------
        ' Resolve request XML
        '------------------------------------------------------------------------
        Dim strEnableSecuredWS As String = String.Empty
        strEnableSecuredWS = AppConfigMgr.getEnableSecuredWS()
        If strEnableSecuredWS.Trim.ToUpper = "Y" Then
            Try
                xmlRequest = SecurityHelper.ExtractContentFromSecuredXML(xmlRequest, SystemName)
            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.SPPracticeValidation).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I00002), SystemName)  ''Fail to decrypt or verify the signature of input XML
            End Try
        End If
        '------------------------------------------------------------------------

        Try
            AuditLog.AddDescripton_Ext("SystemName", SystemName)
            AuditLog.WriteLogData_Ext(LogID.LOG00023, xmlRequest)

            Dim request As New SPPracticeValidationRequest(xmlRequest, AuditLog)
            Dim response As New SPPracticeValidationResponse

            response.ProcessRequest(request, SystemName, AuditLog)
            Dim xml As XmlDocument = response.GenXMLResult()

            AuditLog.WriteLogData_Ext(LogID.LOG00024, xml.InnerXml)

            '------------------------------------------------------------------------
            'Encrypt and Sign response XML
            '------------------------------------------------------------------------
            Dim xmlResponse As String = xml.InnerXml
            If strEnableSecuredWS.Trim.ToUpper = "Y" Then
                xmlResponse = SecurityHelper.CreateSecuredXMLFromPlainXML(xmlResponse, SystemName)
            End If
            '------------------------------------------------------------------------

            AuditLog.WriteEndLog_Ext(LogID.LOG00012, response.ReturnErrorCodes.RetrieveMessageCodeListForAuditLog())

            Return xmlResponse

        Catch ex As Exception
            ErrorLogHandler.getInstance(EnumAuditLog.SPPracticeValidation).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
            Return GenerateErrorXMLResponse(New ErrorInfoModel(ErrorCodeList.I99999), SystemName)   'internal Error
        End Try

    End Function



#Region "Private Constant"

    Private Const TAG_EHS_RESPONSE As String = "Response"
    Private Const TAG_OUTPUT As String = "Output"
    Private Const TAG_IS_CORRECT As String = "IsCorrect"
    Private Const TAG_ERROR_INFO As String = "ErrorInfo"
    Private Const TAG_ERROR_CODE As String = "ErrorCode"
    Private Const TAG_ERROR_MESSAGE As String = "ErrorMessage"

#End Region

#Region "Private Functions"

    Private Shared Function GenerateErrorXMLResponse(ByVal udtErrorInfo As ErrorInfoModel, ByVal strSystemName As String) As String
        Dim xml As New XmlDocument()

        Dim xmlDeclaration As XmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", Nothing)
        xml.InsertBefore(xmlDeclaration, xml.DocumentElement)

        Dim nodeResponse As XmlElement
        nodeResponse = xml.CreateElement(TAG_EHS_RESPONSE)
        xml.AppendChild(nodeResponse)

        Dim nodeResult As XmlElement
        nodeResult = xml.CreateElement(TAG_OUTPUT)
        nodeResponse.AppendChild(nodeResult)

        Dim nodeInfo As XmlElement
        nodeInfo = xml.CreateElement(TAG_ERROR_INFO)
        nodeResult.AppendChild(nodeInfo)

        Dim nodeTmp As XmlElement

        'Error Code
        nodeTmp = xml.CreateElement(TAG_ERROR_CODE)
        nodeTmp.InnerText = udtErrorInfo.ExternalErrorCode
        nodeInfo.AppendChild(nodeTmp)

        'Error Message
        nodeTmp = xml.CreateElement(TAG_ERROR_MESSAGE)
        nodeTmp.InnerText = udtErrorInfo.ExternalErrorMessage
        nodeInfo.AppendChild(nodeTmp)

        '------------------------------------------------------------------------
        'Encrypt and Sign response XML
        '------------------------------------------------------------------------
        Dim strEnableSecuredWS As String = String.Empty
        strEnableSecuredWS = AppConfigMgr.getEnableSecuredWS()
        Dim xmlResponse As String = xml.InnerXml
        If strEnableSecuredWS.Trim.ToUpper = "Y" Then
            xmlResponse = SecurityHelper.CreateSecuredXMLFromPlainXML_WithDefaultKey(xmlResponse, strSystemName)
        End If
        '------------------------------------------------------------------------

        Return xmlResponse
    End Function

    Private Shared Function CheckAuthorizedExternalParty(ByVal strSystemName As String) As Boolean

        'Dim strAuthorizedParties As String = String.Empty
        'strAuthorizedParties = AppConfigMgr.getAuthorizedParties()

        'Dim arrAuthorizedParties As String() = Nothing
        'arrAuthorizedParties = strAuthorizedParties.Split(New Char() {";"c})
        'For intCounter As Integer = 0 To arrAuthorizedParties.Length - 1
        '    If strSystemName.Trim = arrAuthorizedParties(intCounter).Trim() Then
        '        Return True
        '    End If
        'Next

        'Return False
        Return AppConfigMgr.CheckAuthorizedParties(strSystemName)
    End Function

#End Region



End Class