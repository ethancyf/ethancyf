Imports System.Security.Cryptography.X509Certificates
Imports System.Net.Security
Imports System.IO
Imports System.Xml
Imports System.Xml.Serialization

Public Class CIMSEnquiry
    Inherits System.Web.UI.Page

    Private ReadOnly Property DemoPatientList(ByVal strPatientListFileName As String) As DataTable
        Get
            If Session("PatientListFileName") Is Nothing OrElse Session("PatientListFileName") <> strPatientListFileName Then
                ' Excel formula
                ' =CONCATENATE("<patient>","<patient_ID>",A2,"</patient_ID>","<docType>",B2,"</docType>","<docNo>",C2,"</docNo>","<englishName>",D2,"</englishName>","<sex>",E2,"</sex>","<dob>",F2,"</dob>","<dobFlag>",G2,"</dobFlag>","</patient>")
                Dim strFile As String = IO.Path.Combine(GetPatientListPath(), strPatientListFileName)
                Dim ds As New DataSet
                Dim sr As New StringReader(File.ReadAllText(strFile))
                ds.ReadXml(sr)
                sr.Close()

                Session("PatientListFileName") = strPatientListFileName
                Session("PatientList") = ds.Tables(0)
            End If
            Return Session("PatientList")
        End Get
    End Property

#Region "Setting"
    Private Function GetPatientListPath() As String
        Dim strPath As String = ""
        strPath = Web.Configuration.WebConfigurationManager.AppSettings("SamplePath_CallCIMS")
        'strPath = IO.Path.Combine(strPath, "PatientList.xml")
        Return strPath
    End Function

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Me.IsPostBack Then
            Me.txtURL.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_CIMSVaccination_Url")
            Me.txtServerCertThumbprint.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_CIMS_ServerCert_Thumbprint")
            Me.txtClientCertThumbprint.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_CIMS_ClientCert_Thumbprint")
            Me.txtTimeout.Text = Web.Configuration.WebConfigurationManager.AppSettings("WS_CIMS_Timeout")

            LoadPaitentDropDown()
            If ddlDemoPatientList.Items.Count > 0 Then
                LoadPaitent(ddlDemoPatientList.SelectedItem.Text)
            End If
        End If
    End Sub

    Private Sub LoadPaitentDropDown()
        Dim strPath As String = GetPatientListPath()
        If Not Directory.Exists(strPath) Then Exit Sub

        Me.ddlDemoPatientList.Items.Clear()

        Dim di As New DirectoryInfo(strPath)
        For Each fi As FileInfo In di.GetFiles("*.xml")
            Me.ddlDemoPatientList.Items.Add(fi.Name)
        Next

    End Sub

    Private Sub LoadPaitent(ByVal strPatientListFileName As String)
        gvPatient.DataSource = DemoPatientList(strPatientListFileName)
        gvPatient.DataBind()
    End Sub

    Protected Sub gvPatient_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvPatient.SelectedIndexChanged
        Dim dtPatient As DataTable = Session("PatientList")
        Dim drPatient As DataRow = dtPatient.Rows(gvPatient.SelectedIndex)

        If dtPatient.Columns.Contains("Mode") Then
            Me.txtReqMode.Text = drPatient("Mode")
            Me.txtReqSys.Text = drPatient("System")
            Me.txtVaccineType.Text = drPatient("VaccineType")
            Me.txtInfluenzaCount.Text = drPatient("Influenza")
            Me.txtClientCount.Text = drPatient("Client")
        End If

        Me.txtPatientID.Text = drPatient("patient_ID")
        Me.txtName.Text = drPatient("englishName")
        Me.txtDocType.Text = drPatient("docType")
        Me.txtDocNo.Text = drPatient("docNo")
        Me.txtDOB.Text = drPatient("dob")
        Me.txtDOBFlag.Text = drPatient("dobFlag")
        Me.txtSex.Text = drPatient("sex")
    End Sub

    Protected Sub btnQuerySingle_Click(sender As Object, e As EventArgs) Handles btnQuerySingle.Click
        System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate


        Dim objService As HKIPVREnqService = New HKIPVREnqService
        Dim objRequest As vaccineEnqReq = Nothing
        Dim objReqClient As reqClient = Nothing


        Try
            ClearResult()

            objService.Url = txtURL.Text
            objService.RequestEncrypthionCertThumbprint = Me.txtServerCertThumbprint.Text
            objService.RequestSignatureCertThumbprint = Me.txtClientCertThumbprint.Text
            objService.Timeout = Convert.ToInt32(Me.txtTimeout.Text)

            objRequest = New vaccineEnqReq
            'objRequest.mode = 1 'Health Check
            objRequest.mode = txtReqMode.Text
            objRequest.reqSystem = txtReqSys.Text
            objRequest.vaccineType = txtVaccineType.Text
            objRequest.reqRecordCnt = txtInfluenzaCount.Text
            objRequest.reqRecordCntSpecified = True
            objRequest.clientCnt = txtClientCount.Text
            objRequest.clientCntSpecified = True

            objReqClient = New reqClient
            objReqClient.engName = Me.txtName.Text
            objReqClient.docType = Me.txtDocType.Text
            objReqClient.docNum = Me.txtDocNo.Text
            objReqClient.dob = Me.txtDOB.Text

            objReqClient.dobInd = txtDOBFlag.Text
            objReqClient.sex = txtSex.Text
            objRequest.reqClientList = New reqClient() {objReqClient}

            Dim dtmStart As DateTime = Now()
            Dim objResponse As vaccineEnqRsp = objService.vaccineEnquiry(objRequest)
            Me.txtResult.Text = "Start Time: " + dtmStart.ToString() _
                                 + Environment.NewLine + "Elapsed Time: " + ((Now() - dtmStart).TotalMilliseconds).ToString _
                                 + Environment.NewLine + "returnCode: " + objResponse.returnCode _
                                 + Environment.NewLine + "returnCodeDesc: " + objResponse.returnCodeDesc
            If objResponse.rspClientList IsNot Nothing Then
                Me.txtResult.Text += Environment.NewLine + "rspClientList.Count: " + objResponse.rspClientList.Length().ToString() _
                            + Environment.NewLine + "rspClient.returnCode: " + objResponse.rspClientList(0).returnCode() _
                            + Environment.NewLine + "rspClient.returnCodeDesc: " + objResponse.rspClientList(0).returnCodeDesc() _
                            + Environment.NewLine + "rspClient.returnCIMSCode: " + objResponse.rspClientList(0).returnCIMSCode() _
                            + Environment.NewLine + "rspClient.returnEHSSCode: " + objResponse.rspClientList(0).returnEHSSCode() _
                            + Environment.NewLine + "rspClient.returnHACMSCode: " + objResponse.rspClientList(0).returnHACMSCode() _
                            + Environment.NewLine + "rspClient.returnRecordCnt: " + objResponse.rspClientList(0).returnRecordCnt().ToString()
            End If

            BindResult(objResponse)

            Me.txtRawData.Text = DataSerialize(objResponse)

        Catch exSoapHeader As System.Web.Services.Protocols.SoapException
            Me.txtResult.Text = exSoapHeader.ToString() _
                + Environment.NewLine + Environment.NewLine
            If exSoapHeader.InnerException IsNot Nothing Then
                Me.txtResult.Text += exSoapHeader.InnerException.ToString()
            End If

        Catch ex As Exception
            Me.txtResult.Text = ex.ToString() _
                + Environment.NewLine + Environment.NewLine
            If ex.InnerException IsNot Nothing Then
                Me.txtResult.Text += ex.InnerException.ToString()
            End If

        End Try
    End Sub

    Protected Sub btnQueryBatch_Click(sender As Object, e As EventArgs) Handles btnQueryBatch.Click
        System.Net.ServicePointManager.ServerCertificateValidationCallback = AddressOf ValidateRemoteCertificate


        Dim objService As HKIPVREnqService = New HKIPVREnqService
        Dim objRequest As vaccineEnqReq = Nothing
        Dim objReqClient As reqClient = Nothing


        Try
            ClearResult()

            objService.Url = txtURL.Text
            objService.RequestEncrypthionCertThumbprint = Me.txtServerCertThumbprint.Text
            objService.RequestSignatureCertThumbprint = Me.txtClientCertThumbprint.Text
            objService.Timeout = Convert.ToInt32(Me.txtTimeout.Text)

            Dim dtPatient As DataTable = Session("PatientList")

            objRequest = New vaccineEnqReq
            'objRequest.mode = 1 'Health Check
            objRequest.mode = txtReqMode.Text
            objRequest.reqSystem = txtReqSys.Text
            objRequest.vaccineType = txtVaccineType.Text
            objRequest.reqRecordCnt = txtInfluenzaCount.Text
            objRequest.reqRecordCntSpecified = True
            objRequest.clientCnt = dtPatient.Rows.Count
            objRequest.clientCntSpecified = True

            Dim arrClient(dtPatient.Rows.Count - 1) As reqClient
            objRequest.reqClientList = arrClient

            For i As Integer = 0 To dtPatient.Rows.Count - 1
                Dim drPatient As DataRow = dtPatient.Rows(i)

                objReqClient = New reqClient
                objReqClient.engName = drPatient("englishName")
                objReqClient.docType = drPatient("docType")
                objReqClient.docNum = drPatient("docNo")
                objReqClient.dob = drPatient("dob")
                objReqClient.dobInd = drPatient("dobFlag")
                objReqClient.sex = drPatient("sex")
                objRequest.reqClientList(i) = objReqClient
            Next

            Dim dtmStart As DateTime = Now()
            Dim objResponse As vaccineEnqRsp = objService.vaccineEnquiry(objRequest)
            Me.txtResult.Text = "Start Time: " + dtmStart.ToString() _
                                 + Environment.NewLine + "Elapsed Time: " + ((Now() - dtmStart).TotalMilliseconds).ToString _
                                + Environment.NewLine + "returnCode: " + objResponse.returnCode _
                                + Environment.NewLine + "returnCodeDesc: " + objResponse.returnCodeDesc


            'BindResult(objResponse)

            Me.txtRawData.Text = DataSerialize(objResponse)

        Catch ex As Exception
            Me.txtResult.Text = ex.ToString() _
                + Environment.NewLine + Environment.NewLine
            If ex.InnerException IsNot Nothing Then
                Me.txtResult.Text += ex.InnerException.ToString()
            End If
        End Try
    End Sub

    Private Function FormatXml(ByVal xmlDoc As XmlDocument) As String
        Dim sb As New StringBuilder()
        'We will use stringWriter to push the formated xml into our StringBuilder sb.`  
        Using stringWriter As New StringWriter(sb)
            'We will use the Formatting of our xmlTextWriter to provide our indentation.`  
            Using xmlTextWriter As New XmlTextWriter(stringWriter)
                xmlTextWriter.Formatting = Formatting.Indented
                xmlDoc.WriteTo(xmlTextWriter)
            End Using
        End Using
        Return sb.ToString()
    End Function
    Private Function ValidateRemoteCertificate(sender As Object, certification As X509Certificate, chain As X509Chain, sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function

    Private Sub ClearResult()
        txtResult.Text = String.Empty
        txtRawData.Text = String.Empty
        gvVaccine.DataSource = Nothing
        gvVaccine.DataBind()
    End Sub

    Private Sub BindResult(ByVal objResponse As vaccineEnqRsp)
        If objResponse.rspClientList Is Nothing Then
            Return
        End If

        If objResponse.rspClientList(0).vaccineGroupList Is Nothing Then
            Return
        End If

        Dim dtVaccine As New DataTable
        dtVaccine.Columns.Add("vaccineType")
        dtVaccine.Columns.Add("vaccineIdenType")
        dtVaccine.Columns.Add("L3_hkRegNum")
        dtVaccine.Columns.Add("L3_ProdName")
        dtVaccine.Columns.Add("L2_Desc")
        dtVaccine.Columns.Add("validDoseInd")
        dtVaccine.Columns.Add("vaccineProviderEng")
        dtVaccine.Columns.Add("vaccineProviderChi")
        dtVaccine.Columns.Add("admDate", GetType(System.DateTime))
        dtVaccine.Columns.Add("admLocEng")
        dtVaccine.Columns.Add("admLocChi")
        dtVaccine.Columns.Add("doseSeq")
        dtVaccine.Columns.Add("doseSeqDescEng")
        dtVaccine.Columns.Add("doseSeqDescChi")

        For i As Integer = 0 To objResponse.rspClientList(0).vaccineGroupList.Length - 1

            For j As Integer = 0 To objResponse.rspClientList(0).vaccineGroupList(i).vaccineRecordList.Length - 1
                Dim objVaccineRecord As vaccineRecord = objResponse.rspClientList(0).vaccineGroupList(i).vaccineRecordList(j)
                Dim dr As DataRow = dtVaccine.NewRow
                dr("vaccineType") = objResponse.rspClientList(0).vaccineGroupList(i).vaccineType
                dr("admDate") = New Date(objVaccineRecord.admDate.Substring(6, 4), objVaccineRecord.admDate.Substring(3, 2), objVaccineRecord.admDate.Substring(0, 2))
                dr("admLocChi") = objVaccineRecord.admLocChi
                dr("admLocEng") = objVaccineRecord.admLocEng
                dr("doseSeq") = objVaccineRecord.doseSeq
                dr("doseSeqDescChi") = objVaccineRecord.doseSeqDescChi
                dr("doseSeqDescEng") = objVaccineRecord.doseSeqDescEng
                dr("vaccineIdenType") = objVaccineRecord.vaccineIdenType

                If objVaccineRecord.vaccineIdenType = "L3" Then
                    dr("L3_hkRegNum") = objVaccineRecord.vaccineL3Iden.hkRegNum
                    dr("L3_ProdName") = objVaccineRecord.vaccineL3Iden.vaccineProdName
                End If

                If objVaccineRecord.vaccineIdenType = "L2" Then
                    dr("L2_Desc") = objVaccineRecord.vaccineL2Iden.vaccineDesc
                End If

                dr("vaccineProviderChi") = objVaccineRecord.vaccineProviderChi
                dr("vaccineProviderEng") = objVaccineRecord.vaccineProviderEng
                dr("validDoseInd") = objVaccineRecord.validDoseInd
                dtVaccine.Rows.Add(dr)
            Next
        Next

        Dim dv As New DataView(dtVaccine)
        dv.Sort = "vaccineType ASC, admDate ASC"
        Me.gvVaccine.DataSource = dv
        Me.gvVaccine.DataBind()

    End Sub

    

    Private Function DataSerialize(ByVal myList As vaccineEnqRsp) As String
        Dim sw As StringWriter = New StringWriter()
        Dim s As New XmlSerializer(myList.GetType())
        s.Serialize(sw, myList)
        Return sw.ToString()
    End Function

   
    Private Sub ddlDemoPatientList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlDemoPatientList.SelectedIndexChanged
        btnQueryBatch.Enabled = True

        LoadPaitent(ddlDemoPatientList.SelectedItem.Text)

        If ddlDemoPatientList.SelectedItem.Text = "PatientListException.xml" Then
            btnQueryBatch.Enabled = False
        End If
    End Sub
End Class