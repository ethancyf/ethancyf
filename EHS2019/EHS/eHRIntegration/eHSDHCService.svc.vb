Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization
Imports Common.ComObject
Imports Common.Component
Imports Common.eHRIntegration
Imports Common.eHRIntegration.BLL
Imports Common.eHRIntegration.Model.Xml.eHSService

' NOTE: You can use the "Rename" command on the context menu to change the class name "eHSServicePatientPortal" in code, svc and config file together.
' NOTE: In order to launch WCF Test Client for testing this service, please select eHSServicePatientPortal.svc or eHSServicePatientPortal.svc.vb at the Solution Explorer and start debugging.

<System.ServiceModel.ServiceBehavior(ValidateMustUnderstand:=False)> _
Public Class eHSDHCService
    Implements eHRClientAPI

    Public Function getExternalWebS(request As getExternalWebSRequest) As getExternalWebSResponse Implements eHRClientAPI.getExternalWebS
        ' Log
        Dim udtAuditLog As New AuditLogEntry(FunctCode.FUNT070401, DBFlagStr.DBFlagInterfaceLog)
        Dim dtmNotificationDtm As DateTime = DateTime.Now

        Dim strEndLogID As String = LogID.LOG00010
        Dim strEndLogDesc As String = "[EHRSS>EHS] Send response"


        'CRE20-006 DHC Claim service [Start][Nichole]
        'To activate eHS(S) service provider as a DHC NSP in eHS(S), it supports to upload all district NSP or particular district NSP
        If request.Body.inputParam.Contains("seteHSSDHCNSP") Then
            strEndLogID = LogID.LOG00011
            strEndLogDesc = "[EHRSS>EHS] Send response - DHC NSP"
        End If

        'To get authorize one-time access to eHS(S) for making claim for DHC related service
        If request.Body.inputParam.Contains("geteHSSDHCClaimAccess") Then
            strEndLogID = LogID.LOG00012
            strEndLogDesc = "[EHRSS>EHS] Send response - DHC Claim Access"
        End If
        'CRE20-006 DHC Claim service [End][Nichole]

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
                udtAuditLog.WriteLog(LogID.LOG00014, "[EHRSS>EHS] Raise alert")
                udtAuditLog.WriteEndLog(LogID.LOG00013, String.Concat(strEndLogDesc, " (Error)"))

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
                udtAuditLog.WriteEndLog(strEndLogID, strEndLogDesc)

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
                udtAuditLog.WriteLog(LogID.LOG00014, "[EHRSS>EHS] Raise alert")
                udtAuditLog.WriteEndLog(LogID.LOG00013, String.Concat(strEndLogDesc, " (Error)"))

                Return New getExternalWebSResponse(New getExternalWebSResponseBody(strResponseXmlError))

            End Try

            udtAuditLog.WriteLog(LogID.LOG00004, "[EHRSS>EHS] Deserialize request body success")

            ' Check the root element to determine the function
            udtAuditLog.AddDescripton("FunctionName", xml.DocumentElement.Name)
            udtAuditLog.WriteLog(LogID.LOG00006, "[EHRSS>EHS] Process function start")

            Dim udtOutFunctionXML As Object = Nothing
            Dim strStackTrace As String = String.Empty

            Dim strConnectReplicationDB As String = ConfigurationManager.AppSettings("ConnectReplicationDB")
            Dim udtDB As Common.DataAccess.Database
            Dim enumUpdateDBWriteOff As EHSAccount.WriteOff

            If strConnectReplicationDB = YesNo.Yes Then
                udtDB = New Common.DataAccess.Database(Common.Component.DBFlagStr.ReadDBFlag)
                enumUpdateDBWriteOff = EHSAccount.WriteOff.NotUpdateDB
            Else
                udtDB = New Common.DataAccess.Database(Common.Component.DBFlagStr.DBFlag)
                enumUpdateDBWriteOff = EHSAccount.WriteOff.UpdateDB
            End If

            Select Case xml.DocumentElement.Name
                'CRE20-006 get the DHC NSP & Claim access interface [Start][Nichole]
                Case "seteHSSDHCNSP"
                    ' Deserialize the input
                    Dim udtInFunctionXML As New InSeteHSSDHCNSPXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXML)

                    ' Process data
                    Try
                        udtOutFunctionXML = (New eHSDHCServiceBLL).SeteHSSDHCNSP(udtInFunctionXML, strStackTrace, enumUpdateDBWriteOff, udtDB)

                        If strStackTrace <> String.Empty Then
                            udtAuditLog.AddDescripton("StackTrace", strStackTrace)
                            udtAuditLog.WriteLog(LogID.LOG00008, "[EHRSS>EHS] Process function fail")
                        Else
                            udtAuditLog.WriteLog(LogID.LOG00007, "[EHRSS>EHS] Process function complete")
                        End If
 

                    Catch ex As Exception
                        udtOutFunctionXML = New OutSeteHSSDHCNSPXmlModel(eHSResultCode.R9999_UnexpectedFailure, udtInFunctionXML.Timestamp)

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteLog(LogID.LOG00008, "[EHRSS>EHS] Process function fail")

                    End Try

                Case "geteHSSDHCClaimAccess"
                    ' Deserialize the input
                    Dim udtInFunctionXML As New IngeteHSSDHCClaimAccessXmlModel
                    XmlFunction.DeserializeXml(xml.OuterXml, udtInFunctionXML)

                    ' Process data
                    Try
                        udtOutFunctionXML = (New eHSDHCServiceBLL).GeteHSSDHCClaimAccess(udtInFunctionXML, strStackTrace, enumUpdateDBWriteOff, udtDB)

                        If strStackTrace <> String.Empty Then
                            udtAuditLog.AddDescripton("StackTrace", strStackTrace)
                            udtAuditLog.WriteLog(LogID.LOG00008, "[EHRSS>EHS] Process function fail")
                        Else
                            udtAuditLog.WriteLog(LogID.LOG00007, "[EHRSS>EHS] Process function complete")
                        End If



                    Catch ex As Exception
                        udtOutFunctionXML = New OutgeteHSSDHCClaimAccessXmlModel(eHSResultCode.R9999_UnexpectedFailure, udtInFunctionXML.Timestamp)

                        udtAuditLog.AddDescripton("Exception", ex.ToString)
                        udtAuditLog.WriteLog(LogID.LOG00008, "[EHRSS>EHS] Process function fail")

                    End Try
                    ' CRE20-006 get the DHC NSP & claim access interface [End][Nichole]
                Case Else
                    Dim udtOutXMLError As New OutSubmitRequestXmlModel
                    udtOutXMLError.data = XmlFunction.SerializeXml(New OutErrorResultModel(eHSResultCode.R9001_InvalidParameter), blnOmitXmlDeclaration:=True)

                    udtAuditLog.AddDescripton("StackTrace", String.Format("Unexpected value (FunctionName={0})", xml.DocumentElement.Name))
                    udtAuditLog.WriteLog(LogID.LOG00008, "[EHRSS>EHS] Process function fail")

                    Dim strResponseXmlError As String = XmlFunction.SerializeXml(udtOutXMLError)

                    udtAuditLog.WriteLogData(LogID.LOG00009, "[EHRSS>EHS] Response body", strResponseXmlError)
                    udtAuditLog.WriteEndLog(strEndLogID, strEndLogDesc)

                    Return New getExternalWebSResponse(New getExternalWebSResponseBody(strResponseXmlError))

            End Select

            ' Check for SCOM Alert
            Dim eResultCode As eHSPatientPortalResultCode = eHSPatientPortalResultCode.NA

            If udtOutFunctionXML.GetType Is GetType(OutGeteHSSDoctorListXmlModel) Then
                eResultCode = DirectCast(udtOutFunctionXML, OutGeteHSSDoctorListXmlModel).ResultCode
            ElseIf udtOutFunctionXML.GetType Is GetType(OutGeteHSSVoucherBalanceXmlModel) Then
                eResultCode = DirectCast(udtOutFunctionXML, OutGeteHSSVoucherBalanceXmlModel).ResultCode
            End If

            Select Case eResultCode
                Case eHSPatientPortalResultCode.R9000_InvalidXmlElement,
                     eHSPatientPortalResultCode.R9001_InvalidParameter,
                     eHSPatientPortalResultCode.R9990_InternalError,
                     eHSPatientPortalResultCode.R9999_UnexpectedFailure

                    udtAuditLog.WriteLog(LogID.LOG00014, "[EHRSS>EHS] Raise alert")

            End Select


            ' Serialize the output
            Dim udtOutXML As New OutSubmitRequestXmlModel
            'udtOutXML.data = XmlFunction.SerializeXml(udtOutFunctionXML, blnCreateCDataSection:=True)
            Dim strOutXML = XmlFunction.SerializeXml(udtOutFunctionXML, blnCreateCDataSection:=True)

            'Dim strResponseXml As String = XmlFunction.SerializeXml(udtOutXML)
            Dim strResponseXml As String = String.Format("{0}{1}{2}", "<?xml version=""1.0"" encoding=""utf-8""?><root><data>", strOutXML, "</data></root>")

            udtAuditLog.WriteLogData(LogID.LOG00009, "[EHRSS>EHS] Response body", strResponseXml)
            udtAuditLog.WriteEndLog(strEndLogID, strEndLogDesc)

            Return New getExternalWebSResponse(New getExternalWebSResponseBody(strResponseXml))

        Catch ex As Exception
            ' Baseline protection
            Dim udtOutXMLError As New OutSubmitRequestXmlModel
            udtOutXMLError.data = XmlFunction.SerializeXml(New OutErrorResultModel(eHSResultCode.R9999_UnexpectedFailure), blnOmitXmlDeclaration:=True)

            udtAuditLog.AddDescripton("Exception", ex.Message)
            udtAuditLog.WriteLog(LogID.LOG00015, "[EHRSS>EHS] Baseline Exception")

            Dim strResponseXmlError As String = XmlFunction.SerializeXml(udtOutXMLError)

            udtAuditLog.WriteLogData(LogID.LOG00009, "[EHRSS>EHS] Response body", strResponseXmlError)
            udtAuditLog.WriteEndLog(LogID.LOG00013, String.Concat(strEndLogDesc, " - Error"))

            Return New getExternalWebSResponse(New getExternalWebSResponseBody(strResponseXmlError))

        End Try

    End Function

End Class
