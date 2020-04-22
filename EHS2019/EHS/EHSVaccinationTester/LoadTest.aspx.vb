Imports System.Xml
Imports Common.WebService.Interface
Imports Common.ComFunction
Imports Common.ComObject
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports Common.Component
Imports Common
Imports Common.Component.EHSTransaction

Partial Public Class LoadTest
    Inherits System.Web.UI.Page

    Public Enum Mode
        HealthCheck = 1
        InterimUseOnly = 2
        FinalReturnAll = 3
    End Enum

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblURL.Text = Web.Configuration.WebConfigurationManager.AppSettings("LoadTest_WS_EHSVaccination_Url_CMS")
        lblCMSURL.Text = Web.Configuration.WebConfigurationManager.AppSettings("LoadTest_WS_CMSVaccination_Url")
        lblCIMSURL.Text = Web.Configuration.WebConfigurationManager.AppSettings("LoadTest_WS_CIMSVaccination_Url")

        System.Net.ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
    End Sub

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Return True
    End Function

    Protected Sub btnCallEHSCMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCallEHSCMS.Click
        Dim objAuditLog As New AuditLogEntry(Common.Component.FunctCode.FUNT020201)
        Dim strMessageID As String = String.Empty
        Dim strRequest As String = String.Empty
        Dim strResult As String = String.Empty
        Dim dtmStart As DateTime = DateTime.MinValue
        Dim dtmEnd As DateTime = DateTime.MinValue
        Dim intNumOfPatient As Integer = 0
        Dim rndNum As New Random(CInt(Now.Millisecond))

        Try
            If rbSingle.Checked Then
                objAuditLog.WriteStartLog("90001", "LoadTest.CallEHS(CMS) Single Start")
                intNumOfPatient = GetLoadTestNumOfPatient("SampleLoadTest_Patient_CallEHSSingle")

                If intNumOfPatient > 0 Then
                    lblRandomFileName.Text = (rndNum.Next(1, 5000) Mod intNumOfPatient) + 1
                    strRequest = GetLoadTestRequest("SampleLoadTestPath_CallEHSSingleCMS", strMessageID, lblRandomFileName.Text).InnerXml
                Else
                    strRequest = GetLoadTestRequest("SampleLoadTestPath_CallEHSSingleCMS", strMessageID).InnerXml
                End If

            End If

            'If rbBatch.Checked Then
            '    objAuditLog.WriteStartLog("90001", "LoadTest.CallEHS Batch Start")
            '    strRequest = GetLoadTestRequest("SampleLoadTestPath_CallEHSBatchCMS").InnerXml
            'End If

            objAuditLog.WriteStartLog("90002", strRequest)
            Dim ws As GetEHSVaccine.GetEHSVaccine = New GetEHSVaccine.GetEHSVaccine
            ws.ServiceAuthHeaderValue = New GetEHSVaccine.ServiceAuthHeader

            ws.ServiceAuthHeaderValue.Username = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_LoginID_CMS")
            ws.ServiceAuthHeaderValue.Password = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Password_CMS")
            ws.Url = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Url_CMS")

            Dim xml As New XmlDocument()
            xml.LoadXml(strRequest)

            dtmStart = Now
            strResult = ws.geteHSVaccineRecord(xml.InnerXml.ToString())
            dtmEnd = Now

            objAuditLog.WriteEndLog("90004", String.Format("LoadTest.CallEHS(CMS) End: {0}", strResult))

            ShowResult(Me.lblResultCallCMS, dtmStart, dtmEnd, Nothing)
        Catch ex As Exception
            dtmEnd = Now
            ShowResult(Me.lblResultCallCMS, dtmStart, dtmEnd, ex)

            If objAuditLog IsNot Nothing Then
                objAuditLog.AddDescripton("Stack Trace", ex.Message)
                objAuditLog.WriteEndLog("90004", "LoadTest.CallEHS(CMS) End")
            End If
        End Try
    End Sub

    Protected Sub btnCallEHSCIMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCallEHSCIMS.Click
        Dim objAuditLog As New AuditLogEntry(Common.Component.FunctCode.FUNT020201)
        Dim strMessageID As String = String.Empty
        Dim strRequest As String = String.Empty
        Dim strResult As String = String.Empty
        Dim dtmStart As DateTime = DateTime.MinValue
        Dim dtmEnd As DateTime = DateTime.MinValue
        Dim intNumOfPatient As Integer = 0
        Dim rndNum As New Random(CInt(Now.Millisecond))

        Try
            If rbSingle.Checked Then
                objAuditLog.WriteStartLog("90001", "LoadTest.CallEHS(CIMS) Single Start")
                intNumOfPatient = GetLoadTestNumOfPatient("SampleLoadTest_Patient_CallEHSSingle")

                If intNumOfPatient > 0 Then
                    lblRandomFileName.Text = (rndNum.Next(1, 5000) Mod intNumOfPatient) + 1
                    strRequest = GetLoadTestRequest("SampleLoadTestPath_CallEHSSingleCIMS", strMessageID, lblRandomFileName.Text).InnerXml
                Else
                    strRequest = GetLoadTestRequest("SampleLoadTestPath_CallEHSSingleCIMS", strMessageID).InnerXml
                End If

            End If

            If rbBatch.Checked Then
                objAuditLog.WriteStartLog("90001", "LoadTest.CallEHS(CIMS) Batch Start")
                strRequest = GetLoadTestRequest("SampleLoadTestPath_CallEHSBatchCIMS", strMessageID).InnerXml
                lblRandomFileName.Text = String.Empty
            End If


            objAuditLog.WriteStartLog("90002", strRequest)
            Dim ws As GetEHSVaccine.GetEHSVaccine = New GetEHSVaccine.GetEHSVaccine
            ws.ServiceAuthHeaderValue = New GetEHSVaccine.ServiceAuthHeader

            ws.ServiceAuthHeaderValue.Username = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_LoginID_CIMS")
            ws.ServiceAuthHeaderValue.Password = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Password_CIMS")
            ws.Url = Web.Configuration.WebConfigurationManager.AppSettings("WS_EHSVaccination_Url_CIMS")

            Dim xml As New XmlDocument()
            xml.LoadXml(strRequest)

            dtmStart = Now
            strResult = ws.geteHSVaccineRecord(xml.InnerXml.ToString())
            dtmEnd = Now

            objAuditLog.WriteEndLog("90004", String.Format("LoadTest.CallEHS(CIMS) End: {0}", strResult))

            ShowResult(Me.lblResultCallCMS, dtmStart, dtmEnd, Nothing)
        Catch ex As Exception
            dtmEnd = Now
            ShowResult(Me.lblResultCallCMS, dtmStart, dtmEnd, ex)

            If objAuditLog IsNot Nothing Then
                objAuditLog.AddDescripton("Stack Trace", ex.Message)
                objAuditLog.WriteEndLog("90004", "LoadTest.CallEHS(CIMS) End")
            End If
        End Try
    End Sub

    Protected Sub btnCallCMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCallCMS.Click
        Dim objAuditLog As New AuditLogEntry(Common.Component.FunctCode.FUNT020201)
        Dim udtAuditLogInterface As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.EVACC_CMS)
        Dim strMessageID As String = String.Empty
        Dim objProxyCMS As Common.WebService.Interface.WSProxyCMS
        Dim strRequest As String = String.Empty
        Dim strResult As String = String.Empty
        Dim dtmStart As DateTime = DateTime.MinValue
        Dim dtmEnd As DateTime = DateTime.MinValue
        Dim intNumOfPatient As Integer = 0
        Dim rndNum As New Random(CInt(Now.Millisecond))


        ' Add InterfaceLog
        udtAuditLogInterface.SetCallSystem(VaccinationBLL.VaccineRecordSystem.CMS.ToString)

        ' CRE18-001 CIMS Vaccination Sharing [Start][Dickson]
        If rbSingle.Checked Then
            objAuditLog.WriteStartLog("90001", "LoadTest.CallCMS Single Start")
            intNumOfPatient = GetLoadTestNumOfPatient("SampleLoadTest_Patient_CallCMSSingle")

            If intNumOfPatient > 0 Then
                lblRandomFileName.Text = (rndNum.Next(1, 5000) Mod intNumOfPatient) + 1
                strRequest = GetLoadTestRequest("SampleLoadTestPath_CallCMSSingle", strMessageID, lblRandomFileName.Text).InnerXml
            Else
                strRequest = GetLoadTestRequest("SampleLoadTestPath_CallCMSSingle", strMessageID).InnerXml
            End If

        End If

        If rbBatch.Checked Then
            objAuditLog.WriteStartLog("90001", "LoadTest.CallCMS Batch Start")
            strRequest = GetLoadTestRequest("SampleLoadTestPath_CallCMSBatch", strMessageID).InnerXml
            lblRandomFileName.Text = String.Empty
        End If
        ' CRE18-001 CIMS Vaccination Sharing [End][Dickson]

        'Add Interface log
        udtAuditLogInterface.AddDescripton("Site", "Primary")
        udtAuditLogInterface.AddDescripton("Link", lblCMSURL.Text)
        udtAuditLogInterface.WriteStartLog(LogID.LOG00001)

        objAuditLog.WriteStartLog("90002", strRequest)
        'Add Interface log
        udtAuditLogInterface.WriteLogData(LogID.LOG00002, strRequest)

        Try
            objProxyCMS = New Common.WebService.Interface.WSProxyCMS(Nothing)

            dtmStart = Now
            strResult = objProxyCMS.GetVaccineInvoke(strRequest, lblCMSURL.Text, GetLoadTestCMSEndpoint("LoadTest_WS_CMSVaccination_Endpoint"))
            dtmEnd = Now

            'objAuditLog.WriteStartLog("90003", strResult)
            objAuditLog.WriteEndLog("90004", String.Format("LoadTest.CallCMS End: {0}", strResult))
            'Add Result to Interface log
            udtAuditLogInterface.WriteLogData(LogID.LOG00003, strResult)

            udtAuditLogInterface.AddDescripton("CallSystem", VaccinationBLL.VaccineRecordSystem.CMS)
            udtAuditLogInterface.AddDescripton("MessageID", strMessageID)
            udtAuditLogInterface.AddDescripton("HealthCheck", YesNo.No)
            udtAuditLogInterface.AddDescripton("BatchEnquiry", IIf(rbSingle.Checked, YesNo.No, YesNo.Yes))
            udtAuditLogInterface.WriteEndLog(LogID.LOG00005)

            ShowResult(Me.lblResultCallCMS, dtmStart, dtmEnd, Nothing)

        Catch ex As Exception
            dtmEnd = Now
            ShowResult(Me.lblResultCallCMS, dtmStart, dtmEnd, ex)

            If objAuditLog IsNot Nothing Then
                objAuditLog.AddDescripton("Stack Trace", ex.Message)
                objAuditLog.WriteEndLog("90004", "LoadTest.CallCMS End")

                'Add Result to Interface log
                udtAuditLogInterface.WriteLogData(LogID.LOG00003, String.Empty)

                udtAuditLogInterface.AddDescripton("CallSystem", VaccinationBLL.VaccineRecordSystem.CMS)
                udtAuditLogInterface.AddDescripton("MessageID", strMessageID)
                udtAuditLogInterface.AddDescripton("HealthCheck", YesNo.No)
                udtAuditLogInterface.AddDescripton("BatchEnquiry", IIf(rbSingle.Checked, YesNo.No, YesNo.Yes))
                udtAuditLogInterface.AddDescripton("Exception", ex.ToString)
                udtAuditLogInterface.WriteEndLog(LogID.LOG00004)
            End If
        End Try
    End Sub

    Protected Sub btnCallCIMS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCallCIMS.Click
        Dim objAuditLog As New AuditLogEntry(Common.Component.FunctCode.FUNT020201)
        Dim udtAuditLogInterface As AuditLogBase = AuditLogInterface.GetAuditLogEntry(AuditLogInterface.EnumAuditLogModule.EVACC_CIMS)
        Dim xmlRequest As XmlDocument = Nothing
        Dim strResult As String = String.Empty
        Dim dtmStart As DateTime = DateTime.MinValue
        Dim dtmEnd As DateTime = DateTime.MinValue
        Dim intNumOfPatient As Integer = 0
        Dim rndNum As New Random(CInt(Now.Millisecond))


        ' Add InterfaceLog
        udtAuditLogInterface.SetCallSystem(VaccinationBLL.VaccineRecordSystem.CIMS.ToString)

        If rbSingle.Checked Then
            objAuditLog.WriteStartLog("90001", "LoadTest.CallCIMS Single Start")
            intNumOfPatient = GetLoadTestNumOfPatient("SampleLoadTest_Patient_CallCIMSSingle")

            If intNumOfPatient > 0 Then
                lblRandomFileName.Text = (rndNum.Next(1, 5000) Mod intNumOfPatient) + 1
                xmlRequest = GetLoadTestCIMSRequest("SampleLoadTestPath_CallCIMSSingle", lblRandomFileName.Text)
            Else
                xmlRequest = GetLoadTestCIMSRequest("SampleLoadTestPath_CallCIMSSingle")
            End If
        End If

        If rbBatch.Checked Then
            objAuditLog.WriteStartLog("90001", "LoadTest.CallCIMS Batch Start")
            xmlRequest = GetLoadTestCIMSRequest("SampleLoadTestPath_CallCIMSBatch")
            lblRandomFileName.Text = String.Empty
        End If

        'Add Interface log
        udtAuditLogInterface.AddDescripton("Site", "Primary")
        udtAuditLogInterface.AddDescripton("Link", lblCIMSURL.Text)
        udtAuditLogInterface.WriteStartLog(LogID.LOG00001)

        'Load client from XML
        Dim ds As DataSet = Common.ComFunction.XmlFunction.Xml2Dataset(xmlRequest.InnerXml)

        objAuditLog.WriteStartLog("90002", xmlRequest.InnerXml)
        'Add Interface log
        udtAuditLogInterface.WriteLogData(LogID.LOG00002, xmlRequest.InnerXml)

        Try
            dtmStart = Now
            strResult = ProcessCIMSRequest(ds)
            dtmEnd = Now

            'objAuditLog.WriteStartLog("90003", strResult)
            objAuditLog.WriteEndLog("90004", String.Format("LoadTest.CallCIMS End: {0}", strResult))
            'Add Result to Interface log
            udtAuditLogInterface.WriteLogData(LogID.LOG00003, strResult)

            udtAuditLogInterface.AddDescripton("CallSystem", VaccinationBLL.VaccineRecordSystem.CIMS)
            udtAuditLogInterface.AddDescripton("HealthCheck", IIf(ds.Tables("vaccineEnqReq").Rows(0).Item("mode") = Mode.HealthCheck, YesNo.Yes, YesNo.No))
            udtAuditLogInterface.AddDescripton("BatchEnquiry", IIf(ds.Tables("vaccineEnqReq").Rows(0).Item("clientCnt") > 1, YesNo.Yes, YesNo.No))
            udtAuditLogInterface.WriteEndLog(LogID.LOG00005)

            ShowResult(Me.lblResultCallCMS, dtmStart, dtmEnd, Nothing)

        Catch ex As Exception
            dtmEnd = Now
            ShowResult(Me.lblResultCallCMS, dtmStart, dtmEnd, ex)

            If objAuditLog IsNot Nothing Then
                objAuditLog.AddDescripton("Stack Trace", ex.Message)
                objAuditLog.WriteEndLog("90004", "LoadTest.CallCIMS End")

                'Add Result to Interface log
                udtAuditLogInterface.WriteLogData(LogID.LOG00003, String.Empty)

                udtAuditLogInterface.AddDescripton("CallSystem", VaccinationBLL.VaccineRecordSystem.CIMS)
                udtAuditLogInterface.AddDescripton("HealthCheck", IIf(ds.Tables("vaccineEnqReq").Rows(0).Item("mode") = Mode.HealthCheck, YesNo.Yes, YesNo.No))
                udtAuditLogInterface.AddDescripton("BatchEnquiry", IIf(ds.Tables("vaccineEnqReq").Rows(0).Item("clientCnt") > 1, YesNo.Yes, YesNo.No))
                udtAuditLogInterface.AddDescripton("Exception", ex.ToString)
                udtAuditLogInterface.WriteEndLog(LogID.LOG00004)
            End If
        End Try
    End Sub

    Private Function ProcessCIMSRequest(ByVal ds As DataSet) As String
        Dim objService As HKIPVREnqService = New HKIPVREnqService
        Dim objRequest As vaccineEnqReq = Nothing
        Dim objReqClient As reqClient = Nothing
        Dim objResponse As vaccineEnqRsp = Nothing

        Try
            objService.Url = lblCIMSURL.Text
            objService.RequestEncrypthionCertThumbprint = Web.Configuration.WebConfigurationManager.AppSettings("WS_CIMS_ServerCert_Thumbprint")
            objService.RequestSignatureCertThumbprint = Web.Configuration.WebConfigurationManager.AppSettings("WS_CIMS_ClientCert_Thumbprint")
            objService.Timeout = Convert.ToInt32(Web.Configuration.WebConfigurationManager.AppSettings("WS_CIMS_Timeout"))

            Dim dtVaccineEnqReq As DataTable = ds.Tables("vaccineEnqReq")

            objRequest = New vaccineEnqReq

            objRequest.mode = dtVaccineEnqReq.Rows(0).Item("mode")
            objRequest.reqSystem = dtVaccineEnqReq.Rows(0).Item("reqSystem")
            objRequest.vaccineType = dtVaccineEnqReq.Rows(0).Item("vaccineType")
            objRequest.reqRecordCnt = dtVaccineEnqReq.Rows(0).Item("reqRecordCnt")
            objRequest.reqRecordCntSpecified = True
            objRequest.clientCnt = dtVaccineEnqReq.Rows(0).Item("clientCnt")
            objRequest.clientCntSpecified = True

            Dim dtClient As DataTable = ds.Tables("client")

            Dim arrClient(dtClient.Rows.Count - 1) As reqClient
            objRequest.reqClientList = arrClient

            For i As Integer = 0 To dtClient.Rows.Count - 1
                Dim drPatient As DataRow = dtClient.Rows(i)

                objReqClient = New reqClient
                objReqClient.engName = drPatient("engName")
                objReqClient.docType = drPatient("docType")
                objReqClient.docNum = drPatient("docNum")
                objReqClient.dob = drPatient("dob")
                objReqClient.dobInd = drPatient("dobInd")
                objReqClient.sex = drPatient("sex")

                objRequest.reqClientList(i) = objReqClient
            Next

            objResponse = objService.vaccineEnquiry(objRequest)

            If objResponse Is Nothing Then
                Throw New Exception("Invalid response.")
            End If

        Catch ex As Exception
            Throw
        End Try

        Return Common.ComFunction.XmlFunction.ConvertObjectToXML(objResponse)

    End Function

    Protected Sub ShowResult(ByVal lbl As Label, ByVal dtmStart As DateTime, ByVal dtmEnd As DateTime, ByVal ex As Exception)
        Dim dtmDiff As DateTime = New DateTime(dtmEnd.Subtract(dtmStart).Ticks)
        lbl.Text = "Time Elapsed: " + dtmDiff.ToString("mm:ss:fff")
        lbl.Text += "<br/>"

        If ex IsNot Nothing Then
            lbl.Text += "Result: Fail<br/>"
            lbl.Text += "Stack Trace: " + ex.Message
        Else
            lbl.Text += "Result: Success"
        End If
    End Sub

    Protected Function GetLoadTestRequest(ByVal strAppSettingKey As String, ByRef strMessageID As String, Optional ByVal strFileNo As String = "") As XmlDocument
        Dim strRequestXMLPath As String = System.Configuration.ConfigurationManager.AppSettings(strAppSettingKey)
        Dim xmlRequest As New XmlDocument
        Dim lstNode As XmlNodeList = Nothing

        If strFileNo <> String.Empty Then
            strRequestXMLPath = String.Format(strRequestXMLPath, strFileNo)
        Else
            strRequestXMLPath = String.Format(strRequestXMLPath, String.Empty)
        End If

        xmlRequest.Load(strRequestXMLPath)

        ' Assign new message ID to request xml
        ' -------------------------------------------------------
        strMessageID = (New GeneralFunction).generateEVaccineMessageID()
        lstNode = xmlRequest.SelectNodes("/parameter/message_id")
        If lstNode IsNot Nothing AndAlso lstNode.Count > 0 Then
            lstNode(0).InnerText = strMessageID
        End If

        Return xmlRequest
    End Function

    Protected Function GetLoadTestCIMSRequest(ByVal strAppSettingKey As String, Optional ByVal strFileNo As String = "") As XmlDocument
        Dim strRequestXMLPath As String = System.Configuration.ConfigurationManager.AppSettings(strAppSettingKey)
        Dim xmlRequest As New XmlDocument
        Dim lstNode As XmlNodeList = Nothing
        Dim strMessageID As String = String.Empty

        If strFileNo <> String.Empty Then
            strRequestXMLPath = String.Format(strRequestXMLPath, strFileNo)
        Else
            strRequestXMLPath = String.Format(strRequestXMLPath, String.Empty)
        End If

        xmlRequest.Load(strRequestXMLPath)

        Return xmlRequest
    End Function

    Protected Function GetLoadTestNumOfPatient(ByVal strAppSettingKey As String) As Integer
        Dim strNumOfPatient As String = System.Configuration.ConfigurationManager.AppSettings(strAppSettingKey)

        Return CInt(strNumOfPatient)

    End Function

    Protected Function GetLoadTestCMSEndpoint(ByVal strAppSettingKey As String) As Integer
        Dim strEndpoint As String = UCase(System.Configuration.ConfigurationManager.AppSettings(strAppSettingKey))

        Select Case strEndpoint
            Case EndpointEnum.EMULATE.ToString
                Return EndpointEnum.EMULATE
            Case EndpointEnum.WEBLOGIC.ToString
                Return EndpointEnum.WEBLOGIC
            Case EndpointEnum.EAIWSPROXY.ToString
                Return EndpointEnum.EAIWSPROXY
            Case Else
                Throw New Exception(String.Format("The endpoint({0}) is not supported. Please set the correct value at key({1}) in Web.config.)", strEndpoint, strAppSettingKey))
        End Select

    End Function

    Private Sub rbBatch_CheckedChanged(sender As Object, e As EventArgs) Handles rbBatch.CheckedChanged
        Me.btnCallEHSCMS.Enabled = False
    End Sub

    Private Sub rbSingle_CheckedChanged(sender As Object, e As EventArgs) Handles rbSingle.CheckedChanged
        Me.btnCallEHSCMS.Enabled = True
    End Sub
End Class