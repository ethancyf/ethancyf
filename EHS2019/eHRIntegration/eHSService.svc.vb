Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports Common.ComObject
Imports Common.Component
Imports Common.eHRIntegration
Imports Common.eHRIntegration.BLL
Imports Common.eHRIntegration.Model.Xml.eHSService

' NOTE: You can use the "Rename" command on the context menu to change the class name "eHRService" in code, svc and config file together.
' NOTE: In order to launch WCF Test Client for testing this service, please select eHRService.svc or eHRService.svc.vb at the Solution Explorer and start debugging.

<System.ServiceModel.ServiceBehavior(ValidateMustUnderstand:=False)> _
Public Class eHSService
    Implements eHRClientAPI

    Public Function getExternalWebS(request As getExternalWebSRequest) As getExternalWebSResponse Implements eHRClientAPI.getExternalWebS
        ' Log
        Dim udtAuditLog As New AuditLogEntry(FunctCode.FUNT070201, DBFlagStr.DBFlagInterfaceLog)
        Dim dtmNotificationDtm As DateTime = DateTime.Now

        Try
            udtAuditLog.WriteStartLog(LogID.LOG00001, "[EHRSS>EHS] Receive request")

            udtAuditLog.WriteLogData(LogID.LOG00002, "[EHRSS>EHS] Request body", request.Body.inputParam)

            udtAuditLog.WriteLog(LogID.LOG00003, "[EHRSS>EHS] Deserialize request body start")

            ' Deserialize
            Dim udtInSubmitRequestXML As New InSubmitRequestXmlModel

            Try
                XmlFunction.DeserializeXml(request.Body.inputParam, udtInSubmitRequestXML)

            Catch exIOE As InvalidOperationException
                Dim udtOutXMLError As New OutSubmitRequestXmlModel
                udtOutXMLError.data = XmlFunction.SerializeXml(New OutErrorResultModel(eHSResultCode.R9000_InvalidXmlElement), blnOmitXmlDeclaration:=True)

                udtAuditLog.AddDescripton("InvalidOperationException", exIOE.Message)
                udtAuditLog.WriteLog(LogID.LOG00005, "[EHRSS>EHS] Deserialize request body fail")

                Dim strResponseXmlError As String = XmlFunction.SerializeXml(udtOutXMLError)

                udtAuditLog.WriteLogData(LogID.LOG00009, "[EHRSS>EHS] Response body", strResponseXmlError)
                udtAuditLog.WriteEndLog(LogID.LOG00010, "[EHRSS>EHS] Send response")

                Return New getExternalWebSResponse(New getExternalWebSResponseBody(strResponseXmlError))

            End Try

            ' Convert the data part to XML
            If IsNothing(udtInSubmitRequestXML.data) OrElse udtInSubmitRequestXML.data.Trim = String.Empty Then
                Dim udtOutXMLError As New OutSubmitRequestXmlModel
                udtOutXMLError.data = XmlFunction.SerializeXml(New OutErrorResultModel(eHSResultCode.R9000_InvalidXmlElement), blnOmitXmlDeclaration:=True)

                udtAuditLog.AddDescripton("StackTrace", "udtInSubmitRequestXML.data Is Nothing or Empty")
                udtAuditLog.WriteLog(LogID.LOG00005, "[EHRSS>EHS] Deserialize request body fail")

                Dim strResponseXmlError As String = XmlFunction.SerializeXml(udtOutXMLError)

                udtAuditLog.WriteLogData(LogID.LOG00009, "[EHRSS>EHS] Response body", strResponseXmlError)
                udtAuditLog.WriteEndLog(LogID.LOG00010, "[EHRSS>EHS] Send response")

                Return New getExternalWebSResponse(New getExternalWebSResponseBody(strResponseXmlError))

            End If

            Dim xml As New XmlDocument

            Try
                xml.LoadXml(udtInSubmitRequestXML.data)

            Catch exXE As XmlException
                Dim udtOutXMLError As New OutSubmitRequestXmlModel
                udtOutXMLError.data = XmlFunction.SerializeXml(New OutErrorResultModel(eHSResultCode.R9000_InvalidXmlElement), blnOmitXmlDeclaration:=True)

                udtAuditLog.AddDescripton("XmlException", exXE.Message)
                udtAuditLog.WriteLog(LogID.LOG00005, "[EHRSS>EHS] Deserialize request body fail")

                Dim strResponseXmlError As String = XmlFunction.SerializeXml(udtOutXMLError)

                udtAuditLog.WriteLogData(LogID.LOG00009, "[EHRSS>EHS] Response body", strResponseXmlError)
                udtAuditLog.WriteEndLog(LogID.LOG00010, "[EHRSS>EHS] Send response")

                Return New getExternalWebSResponse(New getExternalWebSResponseBody(strResponseXmlError))

            End Try

            udtAuditLog.WriteLog(LogID.LOG00004, "[EHRSS>EHS] Deserialize request body success")

            ' Check the root element to determine the function
            udtAuditLog.AddDescripton("FunctionName", xml.DocumentElement.Name)
            udtAuditLog.WriteLog(LogID.LOG00006, "[EHRSS>EHS] Process function start")

            Dim udtOutFunctionXML As Object = Nothing
            Dim strStackTrace As String = String.Empty

            Select Case xml.DocumentElement.Name
                Case "geteHSSTokenInfo"
                    ' Deserialize the input
                    Dim udtInFunctionXML As New InGeteHSSTokenInfoXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXML)

                    ' Process data
                    Try
                        udtOutFunctionXML = (New eHSServiceBLL).GeteHSSTokenInfo(udtInFunctionXML, strStackTrace)

                        If strStackTrace <> String.Empty Then
                            udtAuditLog.AddDescripton("StackTrace", strStackTrace)
                        End If

                        udtAuditLog.WriteLog(LogID.LOG00007, "[EHRSS>EHS] Process function success")

                    Catch ex As Exception
                        udtOutFunctionXML = New OutGeteHSSTokenInfoXmlModel(eHSResultCode.R9999_UnexpectedFailure, udtInFunctionXML.HKID, udtInFunctionXML.Timestamp)

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteLog(LogID.LOG00008, "[EHRSS>EHS] Process function fail")

                    End Try

                Case "seteHSSTokenShared"
                    ' Deserialize the input
                    Dim udtInFunctionXML As New InSeteHSSTokenSharedXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXML)

                    ' Process data
                    Try
                        udtOutFunctionXML = (New eHSServiceBLL).SeteHSSTokenShared(udtInFunctionXML, dtmNotificationDtm, strStackTrace)

                        If strStackTrace <> String.Empty Then
                            udtAuditLog.AddDescripton("StackTrace", strStackTrace)
                        End If

                        udtAuditLog.WriteLog(LogID.LOG00007, "[EHRSS>EHS] Process function success")

                    Catch ex As Exception
                        udtOutFunctionXML = New OutSeteHSSTokenSharedXmlModel(eHSResultCode.R9999_UnexpectedFailure, udtInFunctionXML.HKID, udtInFunctionXML.Timestamp)

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteLog(LogID.LOG00008, "[EHRSS>EHS] Process function fail")

                    End Try

                Case "replaceeHSSToken"
                    ' Deserialize the input
                    Dim udtInFunctionXML As New InReplaceeHSSTokenXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXML)

                    ' Process data
                    Try
                        udtOutFunctionXML = (New eHSServiceBLL).ReplaceeHSSToken(udtInFunctionXML, dtmNotificationDtm, strStackTrace)

                        If strStackTrace <> String.Empty Then
                            udtAuditLog.AddDescripton("StackTrace", strStackTrace)
                        End If

                        udtAuditLog.WriteLog(LogID.LOG00007, "[EHRSS>EHS] Process function success")

                    Catch ex As Exception
                        udtOutFunctionXML = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9999_UnexpectedFailure, udtInFunctionXML.HKID, udtInFunctionXML.Timestamp)

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteLog(LogID.LOG00008, "[EHRSS>EHS] Process function fail")

                    End Try

                Case "notifyeHSSTokenDeactivated"
                    ' Deserialize the input
                    Dim udtInFunctionXML As New InNotifyeHSSTokenDeactivatedXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXML)

                    ' Process data
                    Try
                        udtOutFunctionXML = (New eHSServiceBLL).NotifyeHSSTokenDeactivated(udtInFunctionXML, dtmNotificationDtm, strStackTrace)

                        If strStackTrace <> String.Empty Then
                            udtAuditLog.AddDescripton("StackTrace", strStackTrace)
                        End If

                        udtAuditLog.WriteLog(LogID.LOG00007, "[EHRSS>EHS] Process function success")

                    Catch ex As Exception
                        udtOutFunctionXML = New OutNotifyeHRSSTokenDeactivatedXmlModel(eHSResultCode.R9999_UnexpectedFailure, udtInFunctionXML.HKID, udtInFunctionXML.Timestamp)

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteLog(LogID.LOG00008, "[EHRSS>EHS] Process function fail")

                    End Try

                Case "geteHSSLoginAlias"
                    ' Deserialize the input
                    Dim udtInFunctionXML As New InGeteHSSLoginAliasXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXML)

                    ' Process data
                    Try
                        udtOutFunctionXML = (New eHSServiceBLL).GeteHSSLoginAlias(udtInFunctionXML, strStackTrace)

                        If strStackTrace <> String.Empty Then
                            udtAuditLog.AddDescripton("StackTrace", strStackTrace)
                        End If

                        udtAuditLog.WriteLog(LogID.LOG00007, "[EHRSS>EHS] Process function success")

                    Catch ex As Exception
                        udtOutFunctionXML = New OutGeteHSSLoginAliasXmlModel(eHSResultCode.R9999_UnexpectedFailure, udtInFunctionXML.HKID, udtInFunctionXML.Timestamp)

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteLog(LogID.LOG00008, "[EHRSS>EHS] Process function fail")

                    End Try

                Case "healthCheckeHSS"
                    ' Deserialize the input
                    Dim udtInFunctionXML As New InHealthCheckeHSSXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXML)

                    ' Process data
                    Try
                        udtOutFunctionXML = (New eHSServiceBLL).HealthCheckeHSS(udtInFunctionXML, strStackTrace)

                        If strStackTrace <> String.Empty Then
                            udtAuditLog.AddDescripton("StackTrace", strStackTrace)
                        End If

                        udtAuditLog.WriteLog(LogID.LOG00007, "[EHRSS>EHS] Process function success")

                    Catch ex As Exception
                        udtOutFunctionXML = New OutHealthCheckeHSSXmlModel(eHSResultCode.R9999_UnexpectedFailure, udtInFunctionXML.Timestamp)

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteLog(LogID.LOG00008, "[EHRSS>EHS] Process function fail")

                    End Try

                Case Else
                    Dim udtOutXMLError As New OutSubmitRequestXmlModel
                    udtOutXMLError.data = XmlFunction.SerializeXml(New OutErrorResultModel(eHSResultCode.R9001_InvalidParameter), blnOmitXmlDeclaration:=True)

                    udtAuditLog.AddDescripton("StackTrace", String.Format("Unexpected value (FunctionName={0})", xml.DocumentElement.Name))
                    udtAuditLog.WriteLog(LogID.LOG00008, "[EHRSS>EHS] Process function fail")

                    Dim strResponseXmlError As String = XmlFunction.SerializeXml(udtOutXMLError)

                    udtAuditLog.WriteLogData(LogID.LOG00009, "[EHRSS>EHS] Response body", strResponseXmlError)
                    udtAuditLog.WriteEndLog(LogID.LOG00010, "[EHRSS>EHS] Send response")

                    Return New getExternalWebSResponse(New getExternalWebSResponseBody(strResponseXmlError))

            End Select

            ' Check for SCOM Alert
            Dim eResultCode As eHSResultCode = eHSResultCode.NA

            If udtOutFunctionXML.GetType Is GetType(OutGeteHSSTokenInfoXmlModel) Then
                eResultCode = DirectCast(udtOutFunctionXML, OutGeteHSSTokenInfoXmlModel).ResultCode
            ElseIf udtOutFunctionXML.GetType Is GetType(OutSeteHSSTokenSharedXmlModel) Then
                eResultCode = DirectCast(udtOutFunctionXML, OutSeteHSSTokenSharedXmlModel).ResultCode
            ElseIf udtOutFunctionXML.GetType Is GetType(OutReplaceeHRSSTokenXmlModel) Then
                eResultCode = DirectCast(udtOutFunctionXML, OutReplaceeHRSSTokenXmlModel).ResultCode
            ElseIf udtOutFunctionXML.GetType Is GetType(OutNotifyeHRSSTokenDeactivatedXmlModel) Then
                eResultCode = DirectCast(udtOutFunctionXML, OutNotifyeHRSSTokenDeactivatedXmlModel).ResultCode
            ElseIf udtOutFunctionXML.GetType Is GetType(OutGeteHSSLoginAliasXmlModel) Then
                eResultCode = DirectCast(udtOutFunctionXML, OutGeteHSSLoginAliasXmlModel).ResultCode
            ElseIf udtOutFunctionXML.GetType Is GetType(OutHealthCheckeHSSXmlModel) Then
                eResultCode = DirectCast(udtOutFunctionXML, OutHealthCheckeHSSXmlModel).ResultCode
            End If

            Select Case eResultCode
                Case eHSResultCode.R1002_TokenNotMatch,
                     eHSResultCode.R1005_ExistingTokenNotIssuedBySenderParty,
                     eHSResultCode.R1006_NewTokenNotAvailable,
                     eHSResultCode.R9000_InvalidXmlElement,
                     eHSResultCode.R9001_InvalidParameter,
                     eHSResultCode.R9999_UnexpectedFailure

                    udtAuditLog.WriteLog(LogID.LOG00012, "[EHRSS>EHS] Raise alert")

            End Select

            ' Serialize the output
            Dim udtOutXML As New OutSubmitRequestXmlModel
            udtOutXML.data = XmlFunction.SerializeXml(udtOutFunctionXML, blnCreateCDataSection:=True)

            Dim strResponseXml As String = XmlFunction.SerializeXml(udtOutXML)

            udtAuditLog.WriteLogData(LogID.LOG00009, "[EHRSS>EHS] Response body", strResponseXml)
            udtAuditLog.WriteEndLog(LogID.LOG00010, "[EHRSS>EHS] Send response")

            Return New getExternalWebSResponse(New getExternalWebSResponseBody(strResponseXml))

        Catch ex As Exception
            ' Baseline protection
            Dim udtOutXMLError As New OutSubmitRequestXmlModel
            udtOutXMLError.data = XmlFunction.SerializeXml(New OutErrorResultModel(eHSResultCode.R9999_UnexpectedFailure), blnOmitXmlDeclaration:=True)

            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteLog(LogID.LOG00011, "[EHRSS>EHS] Baseline Exception")

            Dim strResponseXmlError As String = XmlFunction.SerializeXml(udtOutXMLError)

            udtAuditLog.WriteLogData(LogID.LOG00009, "[EHRSS>EHS] Response body", strResponseXmlError)
            udtAuditLog.WriteEndLog(LogID.LOG00010, "[EHRSS>EHS] Send response")

            Return New getExternalWebSResponse(New getExternalWebSResponseBody(strResponseXmlError))

        End Try

    End Function

End Class
