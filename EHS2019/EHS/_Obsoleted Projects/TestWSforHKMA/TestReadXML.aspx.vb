Imports System.Xml
Imports System.io
Imports TestWSforHKMA.Component.ErrorInfo
Imports TestWSforHKMA.Component
Imports TestWSforHKMA.Cryptography

Partial Public Class TestReadXML
    Inherits System.Web.UI.Page


#Region "Load XML Files "
    Private Sub LoadRequest(ByVal sPath As String)
        Dim fr As IO.StreamReader
        Try
            Dim a As IO.FileStream = New IO.FileStream(sPath, IO.FileMode.Open, IO.FileAccess.Read)
            fr = New IO.StreamReader(a)

            txtRequest.Text = ""
            While Not fr.EndOfStream
                txtRequest.Text += fr.ReadLine() + vbCrLf
            End While
            fr.Close()
        Catch ex As Exception
            If fr IsNot Nothing Then
                fr.Close()
            End If
            Throw ex
        End Try
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Me.IsPostBack Then Exit Sub

        Dim strPath As String = Web.Configuration.WebConfigurationManager.AppSettings("XMLSamplePath")
        If Not Directory.Exists(strPath) Then Exit Sub


        Dim di As New DirectoryInfo(strPath)
        For Each fi As FileInfo In di.GetFiles("*.xml")
            ddlSample.Items.Add(fi.Name)
        Next

        LoadSampleXml(ddlSample.Items(0).Text)
    End Sub

    Private Sub ddlSample_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlSample.SelectedIndexChanged
        LoadSampleXml(ddlSample.Text)
    End Sub

    Private Sub LoadSampleXml(ByVal strFileName As String)
        LoadRequest(IO.Path.Combine(Web.Configuration.WebConfigurationManager.AppSettings("XMLSamplePath"), strFileName))
    End Sub

#End Region



    'Private udtUploadClaimRequest_HL7 As UploadClaimRequest_HL7 = New UploadClaimRequest_HL7()


    '    Private Sub ReadData(ByVal xmlDoc As XmlDocument, ByRef udtErrorList As ErrorInfoModelCollection)

    '        Dim objNamespaceMgr As New XmlNamespaceManager(xmlDoc.NameTable)

    '        objNamespaceMgr.AddNamespace("ab", "urn:hl7-org:v3")
    '        xmlDoc.SelectNodes("//ClinicalDocument", objNamespaceMgr)

    '        '---------------------------
    '        ' Account Info (Name)
    '        '---------------------------
    '        Dim nlPatientEngName As XmlNodeList = xmlDoc.SelectNodes("//ab:recordTarget/ab:patientRole[@classCode='PAT']/ab:patient[@classCode='PSN']/ab:name[@use='ABC']", objNamespaceMgr)

    '        ReadPatientEnglishName_family(nlPatientEngName.Item(0), objNamespaceMgr)
    '        ReadPatientEnglishName_Given(nlPatientEngName.Item(0), objNamespaceMgr)

    '        Dim nlPatientChiName As XmlNodeList = xmlDoc.SelectNodes("//ab:recordTarget/ab:patientRole[@classCode='PAT']/ab:patient[@classCode='PSN']/ab:name[@use='IDE']", objNamespaceMgr)

    '        ReadPatientChineseName(nlPatientChiName.Item(0), objNamespaceMgr)
    '        '---------------------------
    '        ' SP Info 
    '        '---------------------------
    '        Dim nlProviderOrganization As XmlNodeList = xmlDoc.SelectNodes("//ab:recordTarget/ab:patientRole[@classCode='PAT']/ab:providerOrganization", objNamespaceMgr)

    '        ReadSPID(nlProviderOrganization.Item(0), objNamespaceMgr)
    '        ReadSPName(nlProviderOrganization.Item(0), objNamespaceMgr)

    '        '---------------------------
    '        ' Claim Info (Service Date)
    '        '---------------------------
    '        Dim nlComponentOf As XmlNodeList = xmlDoc.SelectNodes("//ab:componentOf/ab:encompassingEncounter", objNamespaceMgr)
    '        ReadServiceDate(nlComponentOf.Item(0), objNamespaceMgr)

    '        '---------------------------
    '        ' SP Info (Practice Info)
    '        '---------------------------
    '        Dim nlHealthCarefacility As XmlNodeList = xmlDoc.SelectNodes("//ab:componentOf/ab:encompassingEncounter/ab:location[@typeCode='LOC']/ab:healthCareFacility[@classCode='SDLOC']", objNamespaceMgr)
    '        ReadPracticeID(nlHealthCarefacility.Item(0), objNamespaceMgr)
    '        ReadPracticeName(nlHealthCarefacility.Item(0), objNamespaceMgr)

    '        '---------------------------
    '        ' Account Info (Document Type Related)
    '        '---------------------------
    '        Dim nlAct As XmlNodeList = xmlDoc.SelectNodes("//ab:component" + _
    '                                                    "/ab:structuredBody" + _
    '                                                    "/ab:component" + _
    '                                                    "/ab:section" + _
    '                                                    "/ab:entry" + _
    '                                                    "/ab:act[@classCode='ACT'][@moodCode='EVN']" + _
    '                                                    "/ab:entryRelationship[@typeCode='COMP']" + _
    '                                                    "/ab:act[@classCode='ACT'][@moodCode='EVN']", objNamespaceMgr)
    '        '"/ab:code[@code='eHS-001'][@codeSystemName='eHS'][@displayName='Claim Submission']" + _
    '        '"/ab:code[@code='eHS-0001a'][@codeSystemName='eHS'][@displayName='Document Type Reference']" + _

    '        ReadDocCode(nlAct.Item(0), objNamespaceMgr)
    '        ReadDocAdditionalInfo(nlAct.Item(0).SelectNodes("./ab:code/ab:qualifier", objNamespaceMgr), objNamespaceMgr)

    '        '---------------------------
    '        ' Voucher / Subsidy Scheme Details
    '        '---------------------------
    '        Dim nlEntryList As XmlNodeList = xmlDoc.SelectNodes("//ab:component" + _
    '                                                    "/ab:structuredBody" + _
    '                                                    "/ab:component" + _
    '                                                    "/ab:section" + _
    '                                                    "/ab:entry[ab:substanceAdministration]", objNamespaceMgr)
    '        For Each nlEntry As XmlNode In nlEntryList
    '            ReadClaimDetail(nlEntry, objNamespaceMgr)
    '        Next

    '        '----------------------------
    '        ' RCH Code  (Apply to all submitted Claim)
    '        '----------------------------
    '        Dim nodeTmp As XmlNode
    '        Dim nlAdditionalEntrylist As XmlNodeList = xmlDoc.SelectNodes("//ab:component" + _
    '                                            "/ab:structuredBody" + _
    '                                            "/ab:component" + _
    '                                            "/ab:section" + _
    '                                            "/ab:entry" + _
    '                                            "/ab:observation[@classCode='OBS'][@moodCode='EVN']", objNamespaceMgr)

    '        For Each node As XmlNode In nlAdditionalEntrylist
    '            nodeTmp = node.SelectSingleNode("./ab:code[@code='rchCode']", objNamespaceMgr)
    '            If Not IsNothing(nodeTmp) Then
    '                nodeTmp = node.SelectSingleNode("./ab:entryRelationship[@typeCode='COMP']" + _
    '                                                "/ab:act[@classCode='ACT'][@moodCode='EVN']", objNamespaceMgr)
    '                ReadRCHCode(nodeTmp, objNamespaceMgr)
    '            End If
    '        Next


    '        'Generate XML
    '        'txtResult.Text = udtUploadClaimRequest_HL7.GenXMLResult().InnerXml
    '    End Sub

    '#Region "Read SP info"
    '    Protected Sub ReadSPID(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
    '        udtUploadClaimRequest_HL7.SPID = ReadStringFromAttribute(node, "id", "extension", objNamespaceMgr)
    '    End Sub

    '    Protected Sub ReadSPName(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
    '        udtUploadClaimRequest_HL7.SPSurname = ReadString(node, "name", objNamespaceMgr)
    '        udtUploadClaimRequest_HL7.SPGivenName = ReadString(node, "name", objNamespaceMgr)
    '    End Sub

    '    Protected Sub ReadPracticeID(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
    '        udtUploadClaimRequest_HL7.PracticeID = ReadStringFromAttribute(node, "code", "code", objNamespaceMgr)
    '    End Sub

    '    Protected Sub ReadPracticeName(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
    '        udtUploadClaimRequest_HL7.PracticeName = ReadStringFromAttribute(node, "code", "displayName", objNamespaceMgr)
    '    End Sub
    '#End Region

    '#Region "Read eHS Account Info"
    '    Protected Sub ReadDocCode(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
    '        udtUploadClaimRequest_HL7.DocType = ReadStringFromAttribute(node, "code", "code", objNamespaceMgr)
    '    End Sub

    '    Protected Sub ReadPatientEnglishName_family(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
    '        udtUploadClaimRequest_HL7.Surname = ReadString(node, "family", objNamespaceMgr)
    '    End Sub

    '    Protected Sub ReadPatientEnglishName_Given(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
    '        udtUploadClaimRequest_HL7.GivenName = ReadString(node, "given", objNamespaceMgr)
    '    End Sub

    '    Protected Sub ReadPatientChineseName(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
    '        Dim strChiName As String = String.Empty
    '        strChiName = ReadString(node, "family", objNamespaceMgr)
    '        strChiName = strChiName + ReadString(node, "given", objNamespaceMgr)
    '        udtUploadClaimRequest_HL7.NameChi = strChiName
    '    End Sub

    '    Protected Sub ReadDocAdditionalInfo(ByVal nodelist As XmlNodeList, ByVal objNamespaceMgr As XmlNamespaceManager)
    '        Dim nodeTmp As XmlNode

    '        Select Case udtUploadClaimRequest_HL7.DocType.Trim
    '            Case "HKIC"
    '                For Each node As XmlNode In nodelist
    '                    'HKIC
    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='docCode']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.HKIC = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='doi']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOI = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='dobType']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOBType = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If
    '                Next
    '            Case "EC"
    '                For Each node As XmlNode In nodelist
    '                    'HKIC
    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='docCode']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.HKIC = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='serialNo']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.SerialNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='reference']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.Reference = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='doi']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOI = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='HKICno']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.HKIC = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='dobType']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOBType = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If
    '                Next
    '            Case "ADOPC"
    '                For Each node As XmlNode In nodelist
    '                    'No of Entry
    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='docCode']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.IdentityNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='noOfEntry']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.EntryNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='dobInWord']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOBInWord = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='dobType']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOBType = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If
    '                Next
    '            Case "HKBC"
    '                For Each node As XmlNode In nodelist
    '                    'Registration No
    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='docCode']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.HKIC = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='dobInWord']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOBInWord = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='dobType']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOBType = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If
    '                Next
    '            Case "DOCI"
    '                For Each node As XmlNode In nodelist
    '                    'Document No
    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='docCode']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DocumentNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='doi']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOI = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='dobType']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOBType = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If
    '                Next
    '            Case "REPMT"
    '                For Each node As XmlNode In nodelist
    '                    'Re-entry permit No
    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='docCode']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.RegNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='doi']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOI = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='dobType']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOBType = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If
    '                Next
    '            Case "ID235B"
    '                For Each node As XmlNode In nodelist
    '                    'Birth Entry No
    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='docCode']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.BirthEntryNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='doi']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOI = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='dobType']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOBType = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If
    '                Next
    '            Case "VISA"
    '                For Each node As XmlNode In nodelist
    '                    'Visa/Reference No
    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='docCode']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.VISANo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If
    '                    'Passport No
    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='passportNo']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.PassportNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='doi']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOI = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If

    '                    nodeTmp = node.SelectSingleNode("./ab:name[@displayName='dobType']", objNamespaceMgr)
    '                    If Not IsNothing(nodeTmp) Then
    '                        udtUploadClaimRequest_HL7.DOBType = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
    '                    End If
    '                Next
    '        End Select
    '    End Sub

    '#End Region

    '#Region "Read Claim Info"
    '    Protected Sub ReadServiceDate(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
    '        Dim strServiceDate As String
    '        strServiceDate = ReadStringFromAttribute(node, "effectiveTime", "extension", objNamespaceMgr)
    '    End Sub

    '    Protected Sub ReadClaimDetail(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
    '        Dim nodeTmp As XmlNode
    '        Dim blnProcceed As Boolean = True
    '        Dim intCurrentIndex As Integer = 0
    '        Dim intItemIndexForVaccine As Integer = 0
    '        Dim strSchemeCode As String = String.Empty
    '        Dim strSubsidyCode As String = String.Empty
    '        Dim strDoseSeq As String = String.Empty
    '        'Scheme Code
    '        nodeTmp = node.SelectSingleNode("./ab:substanceAdministration/ab:entryRelationship[@typeCode='MFST']/ab:act[@classCode='ACT'][@moodCode='EVN']", objNamespaceMgr)
    '        If Not IsNothing(nodeTmp) Then
    '            strSchemeCode = ReadStringFromAttribute(nodeTmp, "code", "code", objNamespaceMgr)
    '        End If
    '        'Dose Sequence
    '        nodeTmp = node.SelectSingleNode("./ab:substanceAdministration/ab:entryRelationship[@typeCode='SUBJ']/ab:observation[@classCode='OBS'][@moodCode='EVN']", objNamespaceMgr)
    '        If Not IsNothing(nodeTmp) Then
    '            strSubsidyCode = ReadString(nodeTmp, "value", objNamespaceMgr)
    '        End If
    '        'Subsidy Code
    '        nodeTmp = node.SelectSingleNode("./ab:substanceAdministration/ab:consumable/ab:manufacturedProduct/ab:manufacturedMaterial", objNamespaceMgr)
    '        If Not IsNothing(nodeTmp) Then
    '            strDoseSeq = ReadStringFromAttribute(nodeTmp, "code", "code", objNamespaceMgr)
    '        End If

    '        '--------------------------
    '        'Fill the values to the model
    '        '--------------------------
    '        If IsNothing(udtUploadClaimRequest_HL7.WSClaimDetailList) Then
    '            udtUploadClaimRequest_HL7.WSClaimDetailList = New WSClaimDetailModelCollection()
    '        End If

    '        If udtUploadClaimRequest_HL7.WSClaimDetailList.Count = 0 Then
    '            udtUploadClaimRequest_HL7.WSClaimDetailList.Add(New WSClaimDetailModel)
    '        Else
    '            intCurrentIndex = udtUploadClaimRequest_HL7.WSClaimDetailList.getItemIndexBySchemeCode(strSchemeCode)
    '            If intCurrentIndex < 0 Then
    '                udtUploadClaimRequest_HL7.WSClaimDetailList.Add(New WSClaimDetailModel)
    '                intCurrentIndex = udtUploadClaimRequest_HL7.WSClaimDetailList.Count - 1
    '            End If
    '        End If

    '        '--> Add Scheme Code
    '        udtUploadClaimRequest_HL7.WSClaimDetailList.Item(intCurrentIndex).SchemeCode = strSchemeCode

    '        If strSchemeCode.Length > 3 AndAlso strSchemeCode.Substring(0, 4) = "HCVS" Then
    '            '--------------------------
    '            'for HCVS
    '            '--------------------------
    '            If IsNothing(udtUploadClaimRequest_HL7.WSClaimDetailList.Item(intCurrentIndex).WSVoucher) Then
    '                udtUploadClaimRequest_HL7.WSClaimDetailList.Item(intCurrentIndex).WSVoucher = New WSVoucherModel()
    '            End If
    '        Else
    '            '--------------------------
    '            'for vaccination
    '            '--------------------------
    '            If IsNothing(udtUploadClaimRequest_HL7.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList) Then
    '                udtUploadClaimRequest_HL7.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList = New WSVaccineDetailModelCollection()
    '            End If
    '            udtUploadClaimRequest_HL7.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList.Add(New WSVaccineDetailModel)
    '            intItemIndexForVaccine = udtUploadClaimRequest_HL7.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList.Count - 1

    '            '--> Add Subsidy Code
    '            '--> Add Dose Seq
    '            udtUploadClaimRequest_HL7.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList.Item(intItemIndexForVaccine).SubsidyCode = strSubsidyCode
    '            udtUploadClaimRequest_HL7.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList.Item(intItemIndexForVaccine).DoseSeq = strDoseSeq
    '        End If

    '    End Sub

    '#End Region

    '#Region "Read RCH Code"

    '    Protected Sub ReadRCHCode(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
    '        Dim strRCHCode As String = String.Empty
    '        strRCHCode = ReadStringFromAttribute(node, "code", "code", objNamespaceMgr)
    '    End Sub

    '#End Region



    Protected Function ReadString(ByVal node As XmlNode, _
                                ByVal sTagName As String, _
                                ByVal objNamespaceMgr As XmlNamespaceManager)
        'ByRef udtErrorList As ErrorInfoModelCollection, _
        'ByRef strCustomError As String, _
        'Optional ByVal blnMandatory As Boolean = True, _
        'Optional ByRef blnTagFound As Boolean = False)
        'Dim nlTemp As XmlNodeList
        'nlTemp = node.SelectNodes("./" + sTagName)
        'If nlTemp.Count = 0 Then
        '    If blnMandatory Then
        '        udtErrorList.Add(strCustomError)  'Incorrect XML format
        '        Return Nothing
        '    Else
        '        Return String.Empty
        '    End If
        'ElseIf nlTemp.Count <> 1 Then
        '    udtErrorList.Add(strCustomError)  'Incorrect XML format
        '    Return Nothing
        'End If

        'blnTagFound = True

        Dim nlTemp As XmlNodeList
        nlTemp = node.SelectNodes("./ab:" + sTagName, objNamespaceMgr)

        Return nlTemp(0).InnerText
    End Function

    Protected Function ReadStringFromAttribute(ByVal node As XmlNode, _
                            ByVal sTagName As String, _
                            ByVal sAttributeName As String, _
                            ByVal objNamespaceMgr As XmlNamespaceManager)

        Dim nlTemp As XmlNode
        nlTemp = node.SelectSingleNode("./ab:" + sTagName, objNamespaceMgr)

        If Not IsNothing(nlTemp.Attributes.GetNamedItem(sAttributeName)) Then
            Return nlTemp.Attributes.GetNamedItem(sAttributeName).InnerText
        End If

        Return Nothing
    End Function


    Protected Function ReadString(ByVal node As XmlNode, _
                            ByVal sTagName As String, _
                            ByVal objNamespaceMgr As XmlNamespaceManager, _
                            ByRef udtErrorList As ErrorInfoModelCollection, _
                            ByRef strCustomError As String, _
                            Optional ByVal blnMandatory As Boolean = True, _
                            Optional ByRef blnTagFound As Boolean = False)
        'Dim nlTemp As XmlNodeList
        'nlTemp = node.SelectNodes("./" + sTagName)
        'If nlTemp.Count = 0 Then
        '    If blnMandatory Then
        '        udtErrorList.Add(strCustomError)  'Incorrect XML format
        '        Return Nothing
        '    Else
        '        Return String.Empty
        '    End If
        'ElseIf nlTemp.Count <> 1 Then
        '    udtErrorList.Add(strCustomError)  'Incorrect XML format
        '    Return Nothing
        'End If

        'blnTagFound = True

        Dim nlTemp As XmlNodeList
        nlTemp = node.SelectNodes("./ab:" + sTagName, objNamespaceMgr)

        Return nlTemp(0).InnerText
    End Function

    Protected Function ReadStringFromAttribute(ByVal node As XmlNode, _
                            ByVal sTagName As String, _
                            ByVal sAttributeName As String, _
                            ByVal objNamespaceMgr As XmlNamespaceManager, _
                            ByRef udtErrorList As ErrorInfoModelCollection, _
                            ByRef strCustomError As String, _
                            Optional ByVal blnMandatory As Boolean = True, _
                            Optional ByRef blnTagFound As Boolean = False)
        Dim nlTemp As XmlNode
        nlTemp = node.SelectSingleNode("./ab:" + sTagName, objNamespaceMgr)

        If Not IsNothing(nlTemp.Attributes.GetNamedItem(sAttributeName)) Then
            Return nlTemp.Attributes.GetNamedItem(sAttributeName).InnerText
        End If

        Return Nothing
    End Function


#Region "UI internal functions"

    Protected Sub BtnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnBack.Click
        Response.Redirect("~\Main.aspx")
    End Sub

    Protected Sub BtnQuery2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BtnQuery2.Click

        Try
            Dim ws As Service1 = New Service1
            'ws.Url = "http://localhost/ExternalInterfaceWS/ExternalInterface.asmx"
            ws.Url = System.Configuration.ConfigurationManager.AppSettings("WebServicesURL")
            Dim strSystemName As String = AppConfigMgr.getSystemName()

            Dim sResult As String = String.Empty
            Dim xml As New XmlDocument()

            Dim callback As New System.Net.Security.RemoteCertificateValidationCallback(AddressOf ValidateCertificate)
            System.Net.ServicePointManager.ServerCertificateValidationCallback = callback

            xml.LoadXml(txtRequest.Text)
            sResult = ws.UploadClaim_HL7(xml.InnerXml.ToString(), strSystemName)

            txtResult.Text = sResult
        Catch ex As Exception
            txtResult.Text = "Errors"
        End Try


    End Sub

    Protected Sub BtnQuery_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnQuery.Click

        Dim sResult As String = String.Empty
        Dim xml As New XmlDocument()
        xml.LoadXml(txtRequest.Text)

        Dim udtErrorList As New ErrorInfoModelCollection()
        'ReadData(xml, udtErrorList)
        'ReadSPInfo(xml)
        'sResult = WSProxyCMS.GetVaccine(xml.InnerXml)
        'txtResult.Text = sResult
    End Sub

    Private Function ValidateCertificate(ByVal sender As Object, ByVal certificate As System.Security.Cryptography.X509Certificates.X509Certificate, ByVal chain As System.Security.Cryptography.X509Certificates.X509Chain, ByVal sslPolicyErrors As Net.Security.SslPolicyErrors) As Boolean
        'Return True to force the certificate to be accepted.
        Return True
    End Function

#End Region



End Class