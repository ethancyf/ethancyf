Imports Common.ComFunction
Imports Common.DataAccess
Imports Common.Component
Imports Common.Component.Mapping
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.PracticeType_PCD
Imports Common.Component.ServiceProvider
Imports Common.Component.ThirdParty
Imports Common.PCD
Imports Common.PCD.WebService
Imports Common.PCD.WebService.Interface

Imports Microsoft.Web.Services3.Design
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Xml




Partial Public Class TestPCDWS
    Inherits System.Web.UI.Page

    Private Const PCD_INTEGRATION_WEB_SERVICE_URL As String = "PCD_INTEGRATION_WEB_SERVICE_URL"
    Private Const PCD_INTEGRATION_WEB_SERVICE_USER As String = "PCD_INTEGRATION_WEB_SERVICE_USER"
    Private Const PCD_INTEGRATION_WEB_SERVICE_PASSWORD As String = "PCD_INTEGRATION_WEB_SERVICE_PASSWORD"
    Private Const PCD_INTEGRATION_WEB_SERVICE_TIMELIMIT As String = "PCD_INTEGRATION_WEB_SERVICE_TIMELIMIT"
    Private Const PCD_INTEGRATION_WEB_SERVICE_USE_PROXY As String = "PCD_INTEGRATION_WEB_SERVICE_USE_PROXY"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Me.rdolstPlatformCode.Items(0).Attributes.Add("onClick", "document.getElementById('txtPlatFormCode').disabled = true;")
        Me.rdolstPlatformCode.Items(1).Attributes.Add("onClick", "document.getElementById('txtPlatFormCode').disabled = true;")
        Me.rdolstPlatformCode.Items(2).Attributes.Add("onClick", "document.getElementById('txtPlatFormCode').disabled = false;")

        If Not IsPostBack Then
            Me.rdolstTypeOfPractice.SelectedIndex = 1
            Me.rdolstGenRequestXML.SelectedIndex = 0
            Me.rdolstPlatformCode.SelectedIndex = 0
            Me.rdolstProxyChoice.SelectedIndex = 0

            'Me.txtLogin.Text = "WSforEHS"
            'Me.txtPwd.Text = "User1234"
            Me.txtLogin.Text = GetWSUsername()
            Me.txtPwd.Text = GetWSPassword()
            'Me.txtURI.Text = "http://localhost/TestWSforPCD_SmartID_DEV/PcdforEhsWS.asmx"
            'Dim TheRef As Uri
            'If Not TheRef Is Nothing Then
            'Me.txtURI.Text = Request.MapPath("./PcdforEhsWS.asmx")
            'End If
            Me.txtURI.Text = GetWSURI()

            Me.txtPlatFormCode.Enabled = False
        Else

            'Me.rdolstGenRequestXML.SelectedValue = Me.hdnSelectedXMLFcn.Value
            'Me.rdolstTypeOfPractice.SelectedValue = Me.hdnSelectedTypeOfPractice.Value
        End If
    End Sub


    Private Sub btnGenerateRequestXML_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGenerateRequestXML.Click
        Me.lblBeginTime.Text = "Call Start: " & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")
        Me.txtM_DebugMsg.Text = String.Empty
        Select Case Me.rdolstGenRequestXML.SelectedValue
            Case "1"
                GenerateUploadEnrolInfoRequestXML()
            Case "2"
                GenerateCreatePCDSPAcctRequestXML()
            Case "3"
                GenerateTransferPracticeInfoRequestXML()
            Case "4"
                GenerateCheckIsActiveSPRequestXML()
            Case "5"
                GenerateCheckAvailableForVerifiedEnrolmentRequestXML()
            Case "6"
                GenerateUploadVerifiedEnrolmentRequestXML()
            Case "7"
                GenerateCheckAccountStatusRequestXML()
            Case Else
                Me.txtM_DebugMsg.Text = "Unknown selection of Web Service function!"
        End Select
        Me.lblEndTime.Text = "Call End: " & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")
    End Sub


    Private Sub btnSubmitXML_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSubmitXML.Click
        Me.lblBeginTime.Text = "Call Start: " & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")
        Me.txtM_DebugMsg.Text = String.Empty
        Select Case Me.rdolstGenRequestXML.SelectedValue
            Case "1"
                PCDUploadEnrolInfo_Submit()
            Case "2"
                PCDCreatePCDSPAcct_Submit()
            Case "3"
                PCDTransferPracticeInfo_Submit()
            Case "4"
                PCDCheckIsActiveSP_Submit()
            Case "5"
                PCDCheckAvailableForVerifiedEnrolment_Submit()
            Case "6"
                PCDUploadVerifiedEnrolment_Submit()
            Case "7"
                PCDCheckAccountStatus_Submit()
            Case Else
                Me.txtM_DebugMsg.Text = "Unknown selection of Web Service function!"
        End Select
        Me.lblEndTime.Text = "Call End: " & System.DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss:fff")
    End Sub

#Region "Generate XML Functions"

    Private Sub GenerateUploadEnrolInfoRequestXML()
        ' generates the XML and save to DB
        Dim strXML As String = String.Empty
        Dim strResults As String = String.Empty
        Dim strSPID As String = txtSPID.Text.Trim

        Dim PCDWS As New PCDWebService(String.Empty)
        Dim udtSP As ServiceProviderModel
        Dim udtSPBLL As ServiceProviderBLL
        Dim returnCode As WebService.Interface.PCDUploadEnrolInfoResult.enumReturnCode
        Dim strResult As String = String.Empty

        udtSPBLL = New ServiceProviderBLL
        udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(strSPID, New Common.DataAccess.Database)

        ' clear
        Me.txtM_DebugMsg.Text = String.Empty
        Me.txtM_RequestXML.Text = String.Empty

        If udtSP Is Nothing Then
            Me.txtM_DebugMsg.Text = "Cannot locate service provider with SPID " & txtSPID.Text
            Exit Sub
        End If

        InjectThirdParty(udtSP, rdolstTypeOfPractice.SelectedValue)

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtSP.PracticeList = udtSP.PracticeList.FilterByPCD(TableLocation.Permanent)
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

        Dim objRequest As Common.PCD.PCDUploadEnrolInfoRequest
        objRequest = New Common.PCD.PCDUploadEnrolInfoRequest
        strXML = objRequest.GenerateXML(udtSP, "S", False)

        Dim strgenPDF_XML_EN As String = String.Empty
        Dim strenPDF_XML_TC As String = String.Empty
        Dim strreturnPCD_ERN As String = String.Empty
        Dim strreturnPCD_PDF_LINK As String = String.Empty

        Dim returnObj As WebService.Interface.PCDUploadEnrolInfoResult = Nothing

        'Dim udtThirdPartyEnrollRecordBLL As Common.Component.ThirdParty.ThirdPartyBLL = New Common.Component.ThirdParty.ThirdPartyBLL
        'Dim udtThirdPartyEnrollRecordModel As Common.Component.ThirdParty.ThirdPartyEnrollRecordModel = New Common.Component.ThirdParty.ThirdPartyEnrollRecordModel( _
        '                Common.Component.ThirdParty.ThirdPartyEnrollRecordModel.EnumSysCode.PCD.ToString(), _
        '                objRequest.PCD_ERN, _
        '                objRequest.XmlRequest.InnerXml, _
        '                objRequest.EnrolmentSubmissionTime, _
        '                Common.Component.ThirdParty.ThirdPartyEnrollRecordModel.EnumRecordStatus.P.ToString(), _
        '                "", _
        '                System.DateTime.Now, _
        '                "Administrator", _
        '                System.DateTime.Now, _
        '                "Administrator", _
        '                Nothing)

        'Dim blnAddSuccess As Boolean = False

        'blnAddSuccess = ThirdParty.ThirdPartyBLL.AddThirdPartyEnrollRecord(udtThirdPartyEnrollRecordModel, New Common.DataAccess.Database())

        If Not udtSP Is Nothing Then
            'strResults = "Enrolment Saved to table successfully." & vbCrLf & "PCD ERN: " & objRequest.PCD_ERN & "Submission Time: " & objRequest.EnrolmentSubmissionTime.ToString("yyyy/MM/dd HH:mm:ss")
            'Me.txtM_DebugMsg.Text = strResults
            'Me.txtM_ResultXML.Text = Server.HtmlEncode(objRequest.XmlRequest.InnerXml)

            Me.txtM_RequestXML.Text = formatXML(objRequest.XmlRequest.InnerXml)
        Else
            strResults = "Failed to save service provider " & strSPID & " to table ThirdPartyEnrollRecord."
            Me.txtM_DebugMsg.Text = strResults
        End If

    End Sub

    Private Sub GenerateCreatePCDSPAcctRequestXML()
        ' generates the XML
        Dim strXML As String = String.Empty

        Dim PCDWS As New PCDWebService(String.Empty)
        Dim udtSP As ServiceProviderModel
        Dim udtSPBLL As ServiceProviderBLL
        Dim returnCode As WebService.Interface.PCDCreatePCDSPAcctResult.enumReturnCode
        Dim strResult As String = String.Empty

        udtSPBLL = New ServiceProviderBLL
        udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(txtSPID.Text, New Common.DataAccess.Database)

        ' clear
        Me.txtM_DebugMsg.Text = String.Empty
        Me.txtM_RequestXML.Text = String.Empty

        If udtSP Is Nothing Then
            Me.txtM_DebugMsg.Text = "Cannot locate service provider with SPID " & txtSPID.Text
            Exit Sub
        End If

        InjectThirdParty(udtSP, rdolstTypeOfPractice.SelectedValue)

        Dim returnObj As WebService.Interface.PCDCreatePCDSPAcctResult = Nothing
        Dim strActivationLink As String = String.Empty

        If Not udtSP Is Nothing Then
            Dim objRequest As PCDCreatePCDSPAcctRequest
            Dim strPlatform As String = String.Empty
            objRequest = New PCDCreatePCDSPAcctRequest()

            ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtSP.PracticeList = udtSP.PracticeList.FilterByPCD(TableLocation.Permanent)
            ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

            If Me.rdolstPlatformCode.SelectedValue = "01" Then
                strPlatform = "A"
            ElseIf Me.rdolstPlatformCode.SelectedValue = "02" Then
                strPlatform = "H"
            ElseIf rdolstPlatformCode.SelectedValue = "99" Then
                If Not Me.txtPlatFormCode.Text = String.Empty Then
                    strPlatform = Me.txtPlatFormCode.Text
                Else
                    Me.txtM_DebugMsg.Text = "Platform code error!  It should be selected 01 or 02 or manual input."
                    Exit Sub
                End If
            End If
            Me.txtM_RequestXML.Text = formatXML(objRequest.GenerateXML(udtSP, strPlatform))
        Else
            Me.txtM_DebugMsg.Text = "Cannot locate service provider with SPID " & txtSPID.Text
        End If
    End Sub

    Private Sub GenerateTransferPracticeInfoRequestXML()
        ' generates the XML
        Dim _PCDTransferPracticeInfoRequest_XML As String = String.Empty

        Dim strXML As String = String.Empty

        Dim PCDWS As New PCDWebService(String.Empty)
        Dim udtSP As ServiceProviderModel
        Dim udtSPBLL As ServiceProviderBLL
        Dim returnCode As WebService.Interface.PCDTransferPracticeInfoResult.enumReturnCode
        Dim strResult As String = String.Empty

        udtSPBLL = New ServiceProviderBLL
        udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(txtSPID.Text, New Common.DataAccess.Database)

        ' clear
        txtM_DebugMsg.Text = String.Empty
        txtM_RequestXML.Text = String.Empty

        If udtSP Is Nothing Then
            Me.lblDebugMsg.Text = "Cannot locate service provider with SPID " & txtSPID.Text
            Exit Sub
        End If

        InjectThirdParty(udtSP, rdolstTypeOfPractice.SelectedValue)

        Dim returnObj As WebService.Interface.PCDTransferPracticeInfoResult = Nothing

        If Not udtSP Is Nothing Then
            Dim objRequest As PCDTransferPracticeInfoRequest
            'Dim objResult As PCDTransferPracticeInfoResult

            objRequest = New PCDTransferPracticeInfoRequest()

            ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtSP.PracticeList = udtSP.PracticeList.FilterByPCD(TableLocation.Permanent)
            ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

            _PCDTransferPracticeInfoRequest_XML = objRequest.GenerateXML(udtSP)

            ' check if it contains errors
            'If objRequest.HasError Then
            '    objResult = New PCDTransferPracticeInfoResult(PCDTransferPracticeInfoResult.enumReturnCode.ErrorAllUnexpected)
            '    ' copy the error message and code to the result object
            '    objResult.ReturnCodeDesc = objRequest.ErrorMessage
            '    objResult.Request = _PCDTransferPracticeInfoRequest_XML
            '    Return objResult
            'End If

            Me.txtM_RequestXML.Text = formatXML(_PCDTransferPracticeInfoRequest_XML)
        Else
            Me.txtM_DebugMsg.Text = "Cannot locate service provider with SPID " & txtSPID.Text
        End If
    End Sub

    Private Sub GenerateCheckIsActiveSPRequestXML()
        ' generates the XML
        Dim _PCDCheckIsActiveSPRequest_XML As String = String.Empty
        Dim strXML As String = String.Empty

        Dim PCDWS As New PCDWebService(String.Empty)
        Dim udtSP As ServiceProviderModel
        Dim udtSPBLL As ServiceProviderBLL
        Dim returnCode As WebService.Interface.PCDCheckIsActiveSPResult.enumReturnCode
        Dim strResult As String = String.Empty
        Dim tmpDataTable As DataTable
        Dim strSPID As String

        ' clear 
        Me.txtM_DebugMsg.Text = String.Empty
        Me.txtM_RequestXML.Text = String.Empty

        udtSPBLL = New ServiceProviderBLL
        'tmpDataTable = udtSPBLL.GetServiceProviderParticulasPermanentByHKID(txtHKID.Text, New Common.DataAccess.Database)
        'If Not tmpDataTable Is Nothing AndAlso tmpDataTable.Rows.Count > 0 Then
        'strSPID = tmpDataTable.Rows(0).Item("SP_ID")
        strSPID = txtSPID.Text
        udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(strSPID, New Common.DataAccess.Database)
        If udtSP Is Nothing Then
            Me.txtM_DebugMsg.Text = "Cannot locate service provider with SPID " & strSPID
            Exit Sub
        End If

        InjectThirdParty(udtSP, rdolstTypeOfPractice.SelectedValue)

        Dim returnObj As WebService.Interface.PCDCheckIsActiveSPResult = Nothing
        Dim strActivationLink As String = String.Empty


        If Not udtSP Is Nothing Then
            Dim objRequest As PCDCheckIsActiveSPRequest
            objRequest = New PCDCheckIsActiveSPRequest()
            _PCDCheckIsActiveSPRequest_XML = objRequest.GenerateXML(udtSP)

            ' check if it contains errors
            'If objRequest.HasError Then
            '    objResult = New PCDCheckIsActiveSPResult(PCDCheckIsActiveSPResult.enumReturnCode.ErrorAllUnexpected)
            '    ' copy the error message and code to the result object
            '    objResult.ReturnCodeDesc = objRequest.ErrorMessage
            '    objResult.Request = _PCDCheckIsActiveSPRequest_XML
            '    Return objResult
            'End If

            Me.txtM_RequestXML.Text = formatXML(_PCDCheckIsActiveSPRequest_XML)
        Else
            Me.txtM_DebugMsg.Text = "Cannot locate service provider with SPID " & txtSPID.Text
        End If

    End Sub

    Private Sub GenerateCheckAvailableForVerifiedEnrolmentRequestXML()
        ' generates the XML
        Dim strResults As String = String.Empty
        Dim strXML As String = String.Empty

        Dim PCDWS As New PCDWebService(String.Empty)
        Dim udtSP As ServiceProviderModel
        Dim udtSPBLL As ServiceProviderBLL
        Dim strResult As String = String.Empty
        Dim strSPID As String

        ' clear 
        Me.txtM_DebugMsg.Text = String.Empty
        Me.txtM_RequestXML.Text = String.Empty

        udtSPBLL = New ServiceProviderBLL

        strSPID = txtSPID.Text
        udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(strSPID, New Common.DataAccess.Database)
        If udtSP Is Nothing Then
            Me.txtM_DebugMsg.Text = "Cannot locate service provider with SPID " & strSPID
            Exit Sub
        End If

        InjectThirdParty(udtSP, rdolstTypeOfPractice.SelectedValue)

        Dim returnObj As WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult = Nothing
        Dim strActivationLink As String = String.Empty


        If Not udtSP Is Nothing Then
            Dim objRequest As PCDCheckAvailableForVerifiedEnrolmentRequest
            objRequest = New PCDCheckAvailableForVerifiedEnrolmentRequest()
            strResults = objRequest.GenerateXML(udtSP)

            Me.txtM_RequestXML.Text = formatXML(strResults)
        Else
            Me.txtM_DebugMsg.Text = "Cannot locate service provider with SPID " & txtSPID.Text
        End If

    End Sub

    Private Sub GenerateUploadVerifiedEnrolmentRequestXML()
        ' generates the XML and save to DB
        Dim strXML As String = String.Empty
        Dim strResults As String = String.Empty
        Dim strSPID As String = txtSPID.Text.Trim

        Dim PCDWS As New PCDWebService(String.Empty)
        Dim udtSP As ServiceProviderModel
        Dim udtSPBLL As ServiceProviderBLL
        'Dim returnCode As WebService.Interface.PCDUploadEnrolInfoResult.enumReturnCode
        Dim strResult As String = String.Empty

        udtSPBLL = New ServiceProviderBLL
        udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(strSPID, New Common.DataAccess.Database)

        ' clear
        Me.txtM_DebugMsg.Text = String.Empty
        Me.txtM_RequestXML.Text = String.Empty

        If udtSP Is Nothing Then
            Me.txtM_DebugMsg.Text = "Cannot locate service provider with SPID " & txtSPID.Text
            Exit Sub
        End If

        ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtSP.PracticeList = udtSP.PracticeList.FilterByPCD(TableLocation.Permanent)
        ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

        InjectThirdParty(udtSP, rdolstTypeOfPractice.SelectedValue)

        Dim objRequest As Common.PCD.PCDUploadVerifiedEnrolmentRequest
        objRequest = New Common.PCD.PCDUploadVerifiedEnrolmentRequest
        strXML = objRequest.GenerateXML(udtSP, "S")

        Dim strgenPDF_XML_EN As String = String.Empty
        Dim strenPDF_XML_TC As String = String.Empty
        Dim strreturnPCD_ERN As String = String.Empty
        Dim strreturnPCD_PDF_LINK As String = String.Empty

        Dim returnObj As WebService.Interface.PCDUploadEnrolInfoResult = Nothing


        If Not udtSP Is Nothing Then

            Me.txtM_RequestXML.Text = formatXML(objRequest.XmlRequest.InnerXml)
        Else
            strResults = "Failed to save service provider " & strSPID & " to table ThirdPartyEnrollRecord."
            Me.txtM_DebugMsg.Text = strResults
        End If

    End Sub

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
    ' --------------------------------------------------------------------------------------------------------------------------------


    Private Sub GenerateCheckAccountStatusRequestXML()
        ' generates the XML
        Dim _PCDCheckAccountStatusRequest_XML As String = String.Empty
        Dim strXML As String = String.Empty

        Dim PCDWS As New PCDWebService(String.Empty)
        Dim udtSP As ServiceProviderModel
        Dim udtSPBLL As ServiceProviderBLL
        'Dim returnCode As WebService.Interface.PCDCheckIsActiveSPResult.enumReturnCode
        Dim strResult As String = String.Empty
        'Dim tmpDataTable As DataTable
        Dim strSPID As String

        ' clear 
        Me.txtM_DebugMsg.Text = String.Empty
        Me.txtM_RequestXML.Text = String.Empty

        udtSPBLL = New ServiceProviderBLL
        'tmpDataTable = udtSPBLL.GetServiceProviderParticulasPermanentByHKID(txtHKID.Text, New Common.DataAccess.Database)
        'If Not tmpDataTable Is Nothing AndAlso tmpDataTable.Rows.Count > 0 Then
        'strSPID = tmpDataTable.Rows(0).Item("SP_ID")
        strSPID = txtSPID.Text
        udtSP = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(strSPID, New Common.DataAccess.Database)
        If udtSP Is Nothing Then
            Me.txtM_DebugMsg.Text = "Cannot locate service provider with SPID " & strSPID
            Exit Sub
        End If

        InjectThirdParty(udtSP, rdolstTypeOfPractice.SelectedValue)

        Dim returnObj As WebService.Interface.PCDCheckIsActiveSPResult = Nothing
        Dim strActivationLink As String = String.Empty


        If Not udtSP Is Nothing Then
            Dim objRequest As PCDCheckAccountStatusRequest
            objRequest = New PCDCheckAccountStatusRequest()
            _PCDCheckAccountStatusRequest_XML = objRequest.GenerateXML(udtSP)

            ' check if it contains errors
            'If objRequest.HasError Then
            '    objResult = New PCDCheckIsActiveSPResult(PCDCheckIsActiveSPResult.enumReturnCode.ErrorAllUnexpected)
            '    ' copy the error message and code to the result object
            '    objResult.ReturnCodeDesc = objRequest.ErrorMessage
            '    objResult.Request = _PCDCheckIsActiveSPRequest_XML
            '    Return objResult
            'End If

            Me.txtM_RequestXML.Text = formatXML(_PCDCheckAccountStatusRequest_XML)
        Else
            Me.txtM_DebugMsg.Text = "Cannot locate service provider with SPID " & txtSPID.Text
        End If

    End Sub
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
#End Region

    Private Sub btnUploadEnrolInfoRequest_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUploadEnrolInfoRequest.Click
        Call PCDUploadEnrolInfo_Submit()
    End Sub


    Private Sub btnCreatePCDSPAcct_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCreatePCDSPAcct.Click
        Call PCDCreatePCDSPAcct_Submit()
    End Sub


    Private Sub btnTransferPracticeInfo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTransferPracticeInfo.Click
        Call PCDTransferPracticeInfo_Submit()
    End Sub


    Private Sub btnCheckIsActiveSP_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCheckIsActiveSP.Click
        Call PCDCheckIsActiveSP_Submit()
    End Sub

#Region "Submit XML to PCD Web Service Functions"

    Private Sub PCDUploadEnrolInfo_Submit()
        Dim sResult As String = String.Empty
        Dim _PCDUploadEnrolInfoResult_XML As String = String.Empty

        Try
            ' bypass cert validation on callback
            InitServicePointManager()

            Call SaveMessageID()

            If Me.rdolstProxyChoice.SelectedValue = "1" Then
                Dim objProxy As Common.PCD.ProxyClass.PcdForEhsWS
                objProxy = CreateWebServiceEndPoint1(GetWS_URL())
                _PCDUploadEnrolInfoResult_XML = objProxy.UploadEnrolInfo(txtM_RequestXML.Text.Trim, ComConfig.WSRequestSystem.SystemCode)
            ElseIf Me.rdolstProxyChoice.SelectedValue = "2" Then
                Dim objProxy As ProxyClass.PcdforEhsWS
                objProxy = CreateWebServiceEndPoint2(GetWS_URL())
                _PCDUploadEnrolInfoResult_XML = objProxy.UploadEnrolInfo(txtM_RequestXML.Text.Trim)
            End If
            Me.txtM_ResultXML.Text = formatXML(_PCDUploadEnrolInfoResult_XML)

        Catch exWeb As System.Net.WebException
            Dim objResult As Common.PCD.WebService.Interface.PCDUploadEnrolInfoResult

            objResult = New Common.PCD.WebService.Interface.PCDUploadEnrolInfoResult(Common.PCD.WebService.Interface.PCDUploadEnrolInfoResult.enumReturnCode.CommunicationLinkError)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & exWeb.Message
            Me.txtM_ResultXML.Text = String.Empty
        Catch ex As Exception
            Dim objResult As Common.PCD.WebService.Interface.PCDUploadEnrolInfoResult

            objResult = New Common.PCD.WebService.Interface.PCDUploadEnrolInfoResult(Common.PCD.WebService.Interface.PCDUploadEnrolInfoResult.enumReturnCode.ErrorAllUnexpected)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & ex.Message
            Me.txtM_ResultXML.Text = String.Empty
        End Try

    End Sub

    Private Sub PCDCreatePCDSPAcct_Submit()
        Dim _PCDCreatePCDSPAcctRequest_XML As String = String.Empty
        Dim _PCDCreatePCDSPAcctResult_XML As String = String.Empty
        'Dim objResult As PCDCreatePCDSPAcctResult

        _PCDCreatePCDSPAcctRequest_XML = Me.txtM_RequestXML.Text


        Try
            Call SaveMessageID()

            ' bypass cert validation on callback
            InitServicePointManager()

            If Me.rdolstProxyChoice.SelectedValue = "1" Then
                Dim objProxy As Common.PCD.ProxyClass.PcdForEhsWS
                objProxy = CreateWebServiceEndPoint1(GetWS_URL())
                _PCDCreatePCDSPAcctResult_XML = objProxy.CreatePCDSPAcct(_PCDCreatePCDSPAcctRequest_XML, ComConfig.WSRequestSystem.SystemCode)
            ElseIf Me.rdolstProxyChoice.SelectedValue = "2" Then
                Dim objProxy As ProxyClass.PcdforEhsWS
                objProxy = CreateWebServiceEndPoint2(GetWS_URL())
                _PCDCreatePCDSPAcctResult_XML = objProxy.CreatePCDSPAcct(_PCDCreatePCDSPAcctRequest_XML)
            End If

            Me.txtM_ResultXML.Text = formatXML(_PCDCreatePCDSPAcctResult_XML)

        Catch exWeb As System.Net.WebException
            Dim objResult As Common.PCD.WebService.Interface.PCDCreatePCDSPAcctResult

            objResult = New Common.PCD.WebService.Interface.PCDCreatePCDSPAcctResult(Common.PCD.WebService.Interface.PCDCreatePCDSPAcctResult.enumReturnCode.CommunicationLinkError)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & exWeb.Message
            Me.txtM_ResultXML.Text = String.Empty
        Catch ex As Exception
            Dim objResult As Common.PCD.WebService.Interface.PCDCreatePCDSPAcctResult

            objResult = New Common.PCD.WebService.Interface.PCDCreatePCDSPAcctResult(Common.PCD.WebService.Interface.PCDCreatePCDSPAcctResult.enumReturnCode.ErrorAllUnexpected)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & ex.Message
            Me.txtM_ResultXML.Text = String.Empty
        End Try

        'Catch exXML As System.Xml.XmlException
        '    objResult = New PCDCreatePCDSPAcctResult(PCDCreatePCDSPAcctResult.enumReturnCode.ErrorAllUnexpected)
        '    Me.txtM_ResultXML.Text = "Error: " & PCDCreatePCDSPAcctResult.enumReturnCode.ErrorAllUnexpected.ToString()
        'Catch exWeb As System.Net.WebException
        '    objResult = New PCDCreatePCDSPAcctResult(PCDCreatePCDSPAcctResult.enumReturnCode.CommunicationLinkError)
        '    Me.txtM_ResultXML.Text = "Error: " & PCDCreatePCDSPAcctResult.enumReturnCode.CommunicationLinkError.ToString()
        'Catch ex As Exception
        '    objResult = New PCDCreatePCDSPAcctResult(PCDCreatePCDSPAcctResult.enumReturnCode.ErrorAllUnexpected)
        '    Me.txtM_ResultXML.Text = "Error: " & PCDCreatePCDSPAcctResult.enumReturnCode.ErrorAllUnexpected.ToString()
        'End Try
        ' write result XML to result box
    End Sub


    Private Sub PCDTransferPracticeInfo_Submit()
        Dim sResult As String = String.Empty
        Dim _PCDTransferPracticeInfoResult_XML As String = String.Empty
        'Dim objResult As Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult


        Try
            Call SaveMessageID()

            ' bypass cert validation on callback
            InitServicePointManager()

            If Me.rdolstProxyChoice.SelectedValue = "1" Then
                Dim objProxy As Common.PCD.ProxyClass.PcdForEhsWS
                objProxy = CreateWebServiceEndPoint1(GetWS_URL())
                _PCDTransferPracticeInfoResult_XML = objProxy.TransferPracticeInfo(Me.txtM_RequestXML.Text.Trim, ComConfig.WSRequestSystem.SystemCode)
            ElseIf Me.rdolstProxyChoice.SelectedValue = "2" Then
                Dim objProxy As ProxyClass.PcdforEhsWS
                objProxy = CreateWebServiceEndPoint2(GetWS_URL())
                _PCDTransferPracticeInfoResult_XML = objProxy.TransferPracticeInfo(Me.txtM_RequestXML.Text.Trim)
            End If

            Me.txtM_ResultXML.Text = formatXML(_PCDTransferPracticeInfoResult_XML)
        Catch exWeb As System.Net.WebException
            Dim objResult As Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult

            objResult = New Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult(Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult.enumReturnCode.CommunicationLinkError)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & exWeb.Message
            Me.txtM_ResultXML.Text = String.Empty
        Catch ex As Exception
            Dim objResult As Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult

            objResult = New Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult(Common.PCD.WebService.Interface.PCDTransferPracticeInfoResult.enumReturnCode.ErrorAllUnexpected)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & ex.Message
            Me.txtM_ResultXML.Text = String.Empty
        End Try

        'If Not objProxy Is Nothing Then
        '    objResult = New PCDTransferPracticeInfoResult(_PCDTransferPracticeInfoResult_XML, Me.hdnMessageID.Value)
        '    Me.txtM_ResultXML.Text = formatXML(objResult.Result)
        'End If

    End Sub


    Private Sub PCDCheckIsActiveSP_Submit()
        Dim _PCDCheckIsActiveSPResult_XML As String = String.Empty

        Try
            Call SaveMessageID()

            ' bypass cert validation on callback
            InitServicePointManager()

            If Me.rdolstProxyChoice.SelectedValue = "1" Then
                Dim objProxy As Common.PCD.ProxyClass.PcdForEhsWS
                objProxy = CreateWebServiceEndPoint1(GetWS_URL())
                _PCDCheckIsActiveSPResult_XML = objProxy.CheckIsActiveSP(txtM_RequestXML.Text.Trim, ComConfig.WSRequestSystem.SystemCode)
            ElseIf Me.rdolstProxyChoice.SelectedValue = "2" Then
                Dim objProxy As ProxyClass.PcdforEhsWS
                objProxy = CreateWebServiceEndPoint2(GetWS_URL())
                _PCDCheckIsActiveSPResult_XML = objProxy.CheckIsActiveSP(txtM_RequestXML.Text.Trim)
            End If

            Me.txtM_ResultXML.Text = formatXML(_PCDCheckIsActiveSPResult_XML)
        Catch exWeb As System.Net.WebException
            Dim objResult As Common.PCD.WebService.Interface.PCDCheckIsActiveSPResult

            objResult = New Common.PCD.WebService.Interface.PCDCheckIsActiveSPResult(Common.PCD.WebService.Interface.PCDCheckIsActiveSPResult.enumReturnCode.CommunicationLinkError)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & exWeb.Message
            Me.txtM_ResultXML.Text = String.Empty
        Catch ex As Exception
            Dim objResult As Common.PCD.WebService.Interface.PCDCheckIsActiveSPResult

            objResult = New Common.PCD.WebService.Interface.PCDCheckIsActiveSPResult(Common.PCD.WebService.Interface.PCDCheckIsActiveSPResult.enumReturnCode.ErrorAllUnexpected)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & ex.Message
            Me.txtM_ResultXML.Text = String.Empty
        End Try
        'If Not objProxy Is Nothing Then
        '    Me.txtM_ResultXML.Text = formatXML(_PCDCheckIsActiveSPResult_XML)
        'End If
    End Sub

    Private Sub PCDCheckAvailableForVerifiedEnrolment_Submit()
        Dim strResult As String = String.Empty

        Try
            Call SaveMessageID()

            ' bypass cert validation on callback
            InitServicePointManager()

            'If Me.rdolstProxyChoice.SelectedValue = "1" Then
            Dim objProxy As Common.PCD.ProxyClass.PcdForEhsWS
            objProxy = CreateWebServiceEndPoint1(GetWS_URL())
            strResult = objProxy.CheckAvailableForVerifiedEnrolment(txtM_RequestXML.Text.Trim, ComConfig.WSRequestSystem.SystemCode)
            'ElseIf Me.rdolstProxyChoice.SelectedValue = "2" Then
            '    Dim objProxy As ProxyClass.PcdforEhsWS
            '    objProxy = CreateWebServiceEndPoint2(GetWS_URL())
            '    _PCDCheckIsActiveSPResult_XML = objProxy.CheckIsActiveS(txtM_RequestXML.Text.Trim)
            'End If

            Me.txtM_ResultXML.Text = formatXML(strResult)
        Catch exWeb As System.Net.WebException
            Dim objResult As Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult

            objResult = New Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult(Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.CommunicationLinkError)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & exWeb.Message
            Me.txtM_ResultXML.Text = String.Empty
        Catch ex As Exception
            Dim objResult As Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult

            objResult = New Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult(Common.PCD.WebService.Interface.PCDCheckAvailableForVerifiedEnrolmentResult.enumReturnCode.ErrorAllUnexpected)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & ex.Message
            Me.txtM_ResultXML.Text = String.Empty
        End Try
        'If Not objProxy Is Nothing Then
        '    Me.txtM_ResultXML.Text = formatXML(_PCDCheckIsActiveSPResult_XML)
        'End If
    End Sub

    Private Sub PCDUploadVerifiedEnrolment_Submit()
        Dim strResult As String = String.Empty

        Try
            ' bypass cert validation on callback
            InitServicePointManager()

            Call SaveMessageID()

            'If Me.rdolstProxyChoice.SelectedValue = "1" Then
            Dim objProxy As Common.PCD.ProxyClass.PcdForEhsWS
            objProxy = CreateWebServiceEndPoint1(GetWS_URL())
            strResult = objProxy.UploadVerifiedEnrolment(txtM_RequestXML.Text.Trim, ComConfig.WSRequestSystem.SystemCode)
            'ElseIf Me.rdolstProxyChoice.SelectedValue = "2" Then
            '    Dim objProxy As ProxyClass.PcdforEhsWS
            '    objProxy = CreateWebServiceEndPoint2(GetWS_URL())
            '    _PCDUploadEnrolInfoResult_XML = objProxy.UploadEnrolInfo(txtM_RequestXML.Text.Trim)
            'End If
            Me.txtM_ResultXML.Text = formatXML(strResult)

        Catch exWeb As System.Net.WebException
            Dim objResult As Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult

            objResult = New Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult(Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult.enumReturnCode.CommunicationLinkError)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & exWeb.Message
            Me.txtM_ResultXML.Text = String.Empty
        Catch ex As Exception
            Dim objResult As Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult

            objResult = New Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult(Common.PCD.WebService.Interface.PCDUploadVerifiedEnrolmentResult.enumReturnCode.ErrorAllUnexpected)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & ex.Message
            Me.txtM_ResultXML.Text = String.Empty
        End Try

    End Sub

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
    ' --------------------------------------------------------------------------------------------------------------------------------
    Private Sub PCDCheckAccountStatus_Submit()
        Dim _PCDCheckAccountStatus_XML As String = String.Empty

        Try
            Call SaveMessageID()

            ' bypass cert validation on callback
            InitServicePointManager()

            'If Me.rdolstProxyChoice.SelectedValue = "1" Then
            Dim objProxy As Common.PCD.ProxyClass.PcdForEhsWS
            objProxy = CreateWebServiceEndPoint1(GetWS_URL())
            _PCDCheckAccountStatus_XML = objProxy.CheckAccountStatus(txtM_RequestXML.Text.Trim, ComConfig.WSRequestSystem.SystemCode)
            'ElseIf Me.rdolstProxyChoice.SelectedValue = "2" Then
            'Dim objProxy As ProxyClass.PcdforEhsWS
            'objProxy = CreateWebServiceEndPoint2(GetWS_URL())
            '_PCDCheckAccountStatus_XML = objProxy.CheckAccountStatus(txtM_RequestXML.Text.Trim)
            'End If

            Me.txtM_ResultXML.Text = formatXML(_PCDCheckAccountStatus_XML)
        Catch exWeb As System.Net.WebException
            Dim objResult As Common.PCD.WebService.Interface.PCDCheckAccountStatusResult

            objResult = New Common.PCD.WebService.Interface.PCDCheckAccountStatusResult(Common.PCD.WebService.Interface.PCDCheckAccountStatusResult.enumReturnCode.CommunicationLinkError)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & exWeb.Message
            Me.txtM_ResultXML.Text = String.Empty
        Catch ex As Exception
            Dim objResult As Common.PCD.WebService.Interface.PCDCheckAccountStatusResult

            objResult = New Common.PCD.WebService.Interface.PCDCheckAccountStatusResult(Common.PCD.WebService.Interface.PCDCheckAccountStatusResult.enumReturnCode.ErrorAllUnexpected)
            Me.txtM_DebugMsg.Text = "Return code " & (CInt(objResult.ReturnCode)).ToString & " is generated for reason: " & ex.Message
            Me.txtM_ResultXML.Text = String.Empty
        End Try
        'If Not objProxy Is Nothing Then
        '    Me.txtM_ResultXML.Text = formatXML(_PCDCheckIsActiveSPResult_XML)
        'End If
    End Sub
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
#End Region


    Public Function GenMessageID() As String
        Dim strMsgID As String = String.Empty
        Dim udtComGeneral As New Common.ComFunction.GeneralFunction()
        strMsgID = udtComGeneral.generatePCDWSMessageID()
        Return strMsgID
    End Function

    Private Function CreateWebServiceEndPoint1(ByVal strURL As String) As Common.PCD.ProxyClass.PcdForEhsWS
        Dim ws As Common.PCD.ProxyClass.PcdForEhsWS = New Common.PCD.ProxyClass.PcdForEhsWS
        ws.Url = strURL

        ' Create Client Policy (Specify that the policy uses the username over transport security assertion)
        Dim strUserName As String = GetWS_Username()
        Dim strPassword As String = GetWS_Password()

        If strUserName <> String.Empty And strPassword <> String.Empty Then
            ws.ServiceAuthHeaderValue = New Common.PCD.ProxyClass.ServiceAuthHeader
            ws.ServiceAuthHeaderValue.Username = strUserName
            ws.ServiceAuthHeaderValue.Password = strPassword
            ' Use windows proxy for access endpoint
            If GetWS_UseProxy() Then
                ws.Proxy = New System.Net.WebProxy()
            End If
        End If

        ws.Timeout = GetWS_Timeout()
        Return ws
    End Function


    Private Function CreateWebServiceEndPoint2(ByVal strURL As String) As ProxyClass.PcdforEhsWS
        Dim ws As ProxyClass.PcdforEhsWS = New ProxyClass.PcdforEhsWS
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
        'oGenFunc.getSystemParameter(PCD_INTEGRATION_WEB_SERVICE_USER, sValue, Nothing)
        sValue = Me.txtLogin.Text
        Return sValue
    End Function

    Private Function GetWS_Password() As String
        Dim oGenFunc As New GeneralFunction()
        Dim sValue As String = String.Empty
        'oGenFunc.getSystemParameterPassword(PCD_INTEGRATION_WEB_SERVICE_PASSWORD, sValue)
        sValue = Me.txtPwd.Text
        Return sValue
    End Function

    Private Function GetWS_URL() As String
        Dim oGenFunc As New GeneralFunction()
        Dim sValue As String = String.Empty

        'oGenFunc.getSystemParameter(PCD_INTEGRATION_WEB_SERVICE_URL, sValue, Nothing)
        sValue = Me.txtURI.Text
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

#Region "Helper Functions"

    Private Function formatXML(ByVal strXML As String) As String

        Dim tmpPosL As Integer = 1      ' position that found '<'
        Dim tmpPosR As Integer = 1      ' position that found '>'
        Dim tmpTag As String = String.Empty
        Dim tmpTagName As String = String.Empty
        Dim intNumOfTabs As Integer = 0
        Dim strFinal As String = String.Empty
        Dim strLastReadTagName As String = String.Empty
        Dim intContentPos As Integer = 1
        Dim blnIsLastCloseTag As Boolean = True
        ' ---- ---- ---- ----
        Dim intLeftRightNone As Integer = 0
        ' intLeftRightNone = 0 ' means not found
        ' intLeftRightNone = 1 ' means found an open tag
        ' intLeftRightNone = 2 ' means found a close tag
        ' intLeftRightNone = 3 ' means found an empty tag
        ' ---- ---- ---- ----

        tmpPosL = 1
        While tmpPosL <= strXML.Length

            ' first scan for the next tag
            tmpTag = getNextNodeInXML(strXML, tmpPosL, tmpPosR, intLeftRightNone)

            ' did not found any tag
            If intLeftRightNone = 0 Then
                ' it is garbage or the XML has syntax error;
                ' leave the loop
                blnIsLastCloseTag = False
                Exit While

                ' found an open tag
            ElseIf intLeftRightNone = 1 Then
                If Not blnIsLastCloseTag Then
                    intNumOfTabs = intNumOfTabs + 1
                End If
                ' print the left tag only
                strFinal = strFinal & vbCrLf & preceedTab(intNumOfTabs, tmpTag)
                tmpPosL = tmpPosR + 1
                intContentPos = tmpPosR + 1
                blnIsLastCloseTag = False

                ' found a close tag
            ElseIf intLeftRightNone = 2 Then
                ' print the content
                If Not blnIsLastCloseTag Then
                    If intContentPos < tmpPosL Then
                        strFinal = strFinal & Mid(strXML, intContentPos, tmpPosL - intContentPos) & tmpTag
                    Else
                        strFinal = strFinal & tmpTag
                    End If
                    tmpPosL = tmpPosR + 1
                    intContentPos = tmpPosR + 1
                    blnIsLastCloseTag = True
                Else
                    intNumOfTabs = intNumOfTabs - 1
                    strFinal = strFinal & vbCrLf & preceedTab(intNumOfTabs, tmpTag)
                    tmpPosL = tmpPosR + 1
                    intContentPos = tmpPosR + 1
                End If

                blnIsLastCloseTag = True

                ' found a tag with empty content
            ElseIf intLeftRightNone = 3 Then
                ' print the line recorded in last position
                If blnIsLastCloseTag Then
                    strFinal = strFinal & vbCrLf
                End If
                strFinal = strFinal & preceedTab(intNumOfTabs, tmpTag) & vbCrLf
                tmpPosL = tmpPosR + 1
                blnIsLastCloseTag = True

                ' found a <? XML definition tag
            ElseIf intLeftRightNone = 4 Then
                strFinal = strFinal & preceedTab(intNumOfTabs, tmpTag)
                tmpPosL = tmpPosR + 1
                intContentPos = tmpPosR + 1
                blnIsLastCloseTag = True
            End If

        End While

        Return strFinal

    End Function

    Private Function getNextNodeInXML(ByVal strXML As String, ByRef leftPos As Integer, ByRef rightPos As Integer, ByRef intLeftRightNone As Integer) As String
        ' intLeftRightNone = 0 ' means not found
        ' intLeftRightNone = 1 ' means found an open tag
        ' intLeftRightNone = 2 ' means found a close tag
        ' intLeftRightNone = 3 ' means found an empty tag
        ' intLeftRightNone = 4 ' means found an XML definition tag

        Dim i As Integer
        Dim leftMark As Integer = 1
        Dim rightMark As Integer = 1
        Dim blnXMLDefn As Boolean = False
        Dim tagName As String = String.Empty

        For i = leftPos To strXML.Length
            If (i + 1) <= strXML.Length Then
                ' ignores '<?' syntax
                If Mid(strXML, i, 2) = "<?" Then
                    leftMark = i
                    i = i + 1       ' skip 1 more position
                    blnXMLDefn = True
                End If
            End If
            If Mid(strXML, i, 1) = "<" Then
                leftMark = i
            End If
            If Mid(strXML, i, 1) = ">" Then
                If blnXMLDefn Then
                    ' found XML definition tag
                    rightMark = i
                    Exit For
                ElseIf leftMark < i Then
                    ' found a tag!
                    rightMark = i
                    Exit For
                End If
            End If
        Next

        ' default, set not found
        intLeftRightNone = 0

        If rightMark > leftMark Then
            ' tagName includes '<' and '>'
            tagName = Mid(strXML, leftMark, rightMark - leftMark + 1)
            ' remove spaces in the tag
            tagName.Replace(" ", "")

            If blnXMLDefn Then
                ' it is an XML defnition tag!
                intLeftRightNone = 4
            End If
            If Mid(tagName, 1, 2) = "</" Then
                ' it is a close tag
                intLeftRightNone = 2
            ElseIf Mid(tagName, tagName.Length - 1, 2) = "/>" Then
                ' it is an empty tag
                intLeftRightNone = 3
            Else
                ' it is an open tag
                intLeftRightNone = 1
            End If
            leftPos = leftMark
            rightPos = rightMark
        End If
        Return tagName
    End Function

    Public Function getTagName(ByVal strTag As String) As String
        Return strTag.Replace("/", "").Replace("<", "").Replace(">", "")
    End Function

    Public Function preceedTab(ByVal intNumOfTabs As Integer, ByVal tagName As String) As String
        Dim strTmp As String = tagName
        Dim i As Integer = intNumOfTabs
        While i > 0
            'strTmp = vbTab & strTmp
            strTmp = "  " & strTmp
            i = i - 1
        End While
        Return strTmp
    End Function

    Private Sub ClearOutputs()
        ' clear all outputs
        txtM_DebugMsg.Text = String.Empty
        txtM_RequestXML.Text = String.Empty
        txtM_ResultXML.Text = String.Empty
    End Sub

    Private Function InjectThirdParty(ByVal udtSP As ServiceProviderModel, ByVal strTypeOfPracticeCode As String) As Boolean
        Dim blnInjected As Boolean = False
        Dim udtThirdPartyList As ThirdParty.ThirdPartyAdditionalFieldEnrolmentCollection
        Dim udtThirdParty As ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel
        Dim udtPrac As Practice.PracticeModel
        Dim i As Integer = 0
        'Dim strTypeOfPracticeCode As String = "PRIV"    ' always inject this type of practice

        Dim udtCodeMapList As Mapping.CodeMappingCollection
        Dim udtCodeMap As Mapping.CodeMappingModel
        udtCodeMapList = Mapping.CodeMappingBLL.GetAllCodeMapping


        If Not udtSP Is Nothing AndAlso Not udtSP.PracticeList Is Nothing AndAlso udtSP.PracticeList.Count > 0 Then
            udtThirdPartyList = New ThirdParty.ThirdPartyAdditionalFieldEnrolmentCollection()
            For i = 1 To udtSP.PracticeList.Count
                udtPrac = udtSP.PracticeList.Item(i)

                If Not udtPrac Is Nothing Then

                    udtCodeMap = udtCodeMapList.GetMappingByCode(Mapping.CodeMappingModel.EnumSourceSystem.EHS, Mapping.CodeMappingModel.EnumTargetSystem.PCD, Mapping.CodeMappingModel.EnumCodeType.Service_Category_Code.ToString(), udtPrac.Professional.ServiceCategoryCode)

                    If Not udtCodeMap Is Nothing Then
                        udtThirdParty = New ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel(ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD, udtSP.EnrolRefNo, udtPrac.DisplaySeq, Common.PCD.EnumConstant.EnumAdditionalFieldID.TYPE_OF_PRACTICE.ToString(), strTypeOfPracticeCode)
                        'udtThirdParty = New ThirdParty.ThirdPartyEnrolmentModel(ThirdParty.ThirdPartyEnrolmentModel.EnumSysCode.PCD, udtSP.EnrolRefNo, udtPrac.DisplaySeq, strTypeOfPracticeCode)
                        udtThirdPartyList.Add(udtThirdParty)
                    End If
                End If
            Next
            udtSP.ThirdPartyAdditionalFieldEnrolmentList = udtThirdPartyList
            blnInjected = True
        End If
    End Function

    Private Sub SaveMessageID()
        ' get MessageID from XML
        Dim strXML As String
        Dim strMessageID As String
        Const TAG_MESSAGE_ID As String = "message_id"

        Dim xmlDoc As New XmlDocument
        Dim messageIDNodes As XmlNodeList
        Dim messageIDNode As XmlNode

        strXML = Me.txtM_RequestXML.Text.Trim
        Try
            xmlDoc.LoadXml(strXML)
            messageIDNodes = xmlDoc.GetElementsByTagName(TAG_MESSAGE_ID)
            For Each messageIDNode In messageIDNodes
                strMessageID = messageIDNode.InnerText
                Me.hdnMessageID.Value = strMessageID
            Next
        Catch ex As Exception
            ' dont do anything
        End Try
    End Sub

    Public Function GetWSUsername() As String
        Const PCD_INTEGRATION_WEB_SERVICE_USER = "PCD_INTEGRATION_WEB_SERVICE_USER"
        Dim objGenFunc As New Common.ComFunction.GeneralFunction()
        'Dim sValue As String = "WSforEHS"
        Dim sValue As String = String.Empty
        objGenFunc.getSystemParameter(PCD_INTEGRATION_WEB_SERVICE_USER, sValue, Nothing)
        Return sValue
    End Function


    Public Function GetWSPassword() As String
        Const PCD_INTEGRATION_WEB_SERVICE_PASSWORD = "PCD_INTEGRATION_WEB_SERVICE_PASSWORD"
        Dim objGenFunc As New Common.ComFunction.GeneralFunction()
        'Dim sValue As String = "User1234"
        Dim sValue As String = String.Empty
        objGenFunc.getSystemParameterPassword(PCD_INTEGRATION_WEB_SERVICE_PASSWORD, sValue)
        Return sValue
    End Function

    Public Function GetWSURI() As String
        Const PCD_INTEGRATION_WEB_SERVICE_URL = "PCD_INTEGRATION_WEB_SERVICE_URL"
        Dim objGenFunc As New Common.ComFunction.GeneralFunction()
        'Dim sValue As String = "WSforEHS"
        Dim sValue As String = String.Empty
        objGenFunc.getSystemParameter(PCD_INTEGRATION_WEB_SERVICE_URL, sValue, Nothing)
        Return sValue
    End Function
#End Region

    Private Sub btnRequest_FormatXML_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRequest_FormatXML.Click
        Me.txtM_RequestXML.Text = formatXML(txtM_RequestXML.Text)
    End Sub

    Private Sub btnResultXML_FormatXML_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResult_FormatXML.Click
        Me.txtM_ResultXML.Text = formatXML(txtM_ResultXML.Text)
    End Sub

    Private Sub WebForm3_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        Me.hdnSelectedXMLFcn.Value = Me.rdolstGenRequestXML.SelectedValue
        Me.hdnSelectedTypeOfPractice.Value = Me.rdolstTypeOfPractice.SelectedValue
    End Sub



    Private Sub btnGetLogin_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetLogin.Click
        Me.txtLogin.Text = Me.GetWSUsername()
    End Sub

    Private Sub btnGetPassword_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetPassword.Click
        Me.txtPwd.Text = Me.GetWSPassword()
    End Sub


    Private Sub btnGetURI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetURI.Click
        Me.txtURI.Text = Me.GetWSURI()
    End Sub

    Private Sub btnGetTesterURI_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnGetTesterURI.Click
        'Me.txtURI.Text = Request.MapPath("./PcdforEhsWS.asmx")
        Me.txtURI.Text = ConfigurationManager.AppSettings("PCDInterfaceEmulatorURL")
    End Sub

    Private Sub btnResult_Clear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnResult_Clear.Click
        Me.txtM_ResultXML.Text = String.Empty
    End Sub
End Class