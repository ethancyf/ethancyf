Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Validation
Imports ExternalInterfaceWS.Component.Request.Base
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports ExternalInterfaceWS.ComObject
Imports Common.ComFunction.GeneralFunction

Namespace Component.Request

    Public Class UploadClaimRequest_HL7
        Inherits BaseWSClaimRequest

#Region "Private constant"
        Private Const DATE_FORMAT As String = "yyyyMMdd"
#End Region

#Region "Properties"

        Private _SPName As String = String.Empty
        Public Property SPName() As String
            Get
                Return _SPName
            End Get
            Set(ByVal value As String)
                _SPName = value
            End Set
        End Property

        ''' <summary>
        ''' Since service date (effective time) and identity number can be found more than one time,
        ''' Checking on consistence of values is required
        ''' </summary>
        ''' <remarks></remarks>
        Private _ServiceDateConsistent As String = String.Empty
        Private _IdentityNoConsistent As String = String.Empty
        Private _IdentityNoFound As Boolean = False

        Private _udtErrorList As ErrorInfoModelCollection = Nothing
        Public Property udtErrorList() As ErrorInfoModelCollection
            Get
                Return _udtErrorList
            End Get
            Set(ByVal value As ErrorInfoModelCollection)
                _udtErrorList = value
            End Set
        End Property

#End Region

#Region "Read XML"

        Private Sub ReadData(ByVal xmlDoc As XmlDocument, ByRef udtErrorList As ErrorInfoModelCollection)

            _udtErrorList = udtErrorList

            Dim objNamespaceMgr As New XmlNamespaceManager(xmlDoc.NameTable)

            objNamespaceMgr.AddNamespace("ab", "urn:hl7-org:v3")
            Dim nlClinicalDocument As XmlNodeList = xmlDoc.SelectNodes("//ab:ClinicalDocument", objNamespaceMgr)

            '------------------------------------------------------------------------------------------------------------
            ' Message ID
            '------------------------------------------------------------------------------------------------------------
            If IsNothing(nlClinicalDocument.Item(0)) Then
                'Tag not found
                udtErrorList.Add(ErrorCodeList.I00100)
                Exit Sub
            Else
                'Failed to read the Message ID (Request)
                Me._strMessageID = ReadStringFromAttribute(nlClinicalDocument.Item(0), "id", "extension", objNamespaceMgr, udtErrorList, ErrorCodeList.I00100, True)
                If Me.udtErrorList.Count > 0 Then
                    Exit Sub
                Else
                    Me._strMessageID = Me._strMessageID.Trim
                End If
            End If

            '------------------------------------------------------------------------------------------------------------
            ' Account Info (Name)
            '------------------------------------------------------------------------------------------------------------
            Dim nlPatientEngName As XmlNodeList = xmlDoc.SelectNodes("//ab:recordTarget/ab:patientRole[@classCode='PAT']/ab:patient[@classCode='PSN']/ab:name[@use='ABC']", objNamespaceMgr)
            If IsNothing(nlPatientEngName.Item(0)) Then
                udtErrorList.Add(ErrorCodeList.I00064)
                Exit Sub
            End If

            'Read English Name
            ReadPatientEnglishName_family(nlPatientEngName.Item(0), objNamespaceMgr, udtErrorList)
            ReadPatientEnglishName_Given(nlPatientEngName.Item(0), objNamespaceMgr, udtErrorList)

            Dim nlPatientInfo As XmlNodeList = xmlDoc.SelectNodes("//ab:recordTarget/ab:patientRole[@classCode='PAT']/ab:patient[@classCode='PSN']", objNamespaceMgr)
            If IsNothing(nlPatientInfo.Item(0)) Then
                udtErrorList.Add(ErrorCodeList.I00067)
                Exit Sub
            End If

            'Identity Number
            ReadIdentityNo(nlPatientInfo.Item(0), objNamespaceMgr)
            If Not _IdentityNoFound Then
                Exit Sub
            End If

            'Read Chinese Name
            ReadPatientChineseName(nlPatientInfo.Item(0), objNamespaceMgr)

            'Read CCC Code
            ReadPatientCCCCode(nlPatientInfo.Item(0), objNamespaceMgr)

            'Read Gender
            ReadPatientGender(nlPatientInfo.Item(0), objNamespaceMgr)

            'Read Birth Date
            ReadPatientBirthTime(nlPatientInfo.Item(0), objNamespaceMgr)

            ''''''''''''''''''Continue?'''''''''''''''''''
            If Not IsNothing(_udtErrorList) AndAlso _udtErrorList.Count > 0 Then
                Exit Sub
            End If

            '------------------------------------------------------------------------------------------------------------
            ' SP Info 
            '------------------------------------------------------------------------------------------------------------
            Dim nlProviderOrganization As XmlNodeList = xmlDoc.SelectNodes("//ab:recordTarget/ab:patientRole[@classCode='PAT']/ab:providerOrganization", objNamespaceMgr)
            If IsNothing(nlProviderOrganization.Item(0)) Then
                udtErrorList.Add(ErrorCodeList.I00073)
                Exit Sub
            End If

            ReadSPID(nlProviderOrganization.Item(0), objNamespaceMgr)
            ReadSPName(nlProviderOrganization.Item(0), objNamespaceMgr)

            ''''''''''''''''''Continue?'''''''''''''''''''
            If Not IsNothing(_udtErrorList) AndAlso _udtErrorList.Count > 0 Then
                Exit Sub
            End If

            '------------------------------------------------------------------------------------------------------------
            ' Claim Info (Service Date)
            '------------------------------------------------------------------------------------------------------------
            Dim nlComponentOf As XmlNodeList = xmlDoc.SelectNodes("//ab:componentOf/ab:encompassingEncounter", objNamespaceMgr)
            If IsNothing(nlComponentOf.Item(0)) Then
                udtErrorList.Add(ErrorCodeList.I00076)
                Exit Sub
            End If

            ReadServiceDate(nlComponentOf.Item(0), objNamespaceMgr)

            ''''''''''''''''''Continue?'''''''''''''''''''
            If Not IsNothing(_udtErrorList) AndAlso _udtErrorList.Count > 0 Then
                Exit Sub
            End If

            '------------------------------------------------------------------------------------------------------------
            ' SP Info (Practice Info)
            '------------------------------------------------------------------------------------------------------------
            Dim nlHealthCarefacility As XmlNodeList = xmlDoc.SelectNodes("//ab:componentOf/ab:encompassingEncounter/ab:location[@typeCode='LOC']/ab:healthCareFacility[@classCode='SDLOC']", objNamespaceMgr)
            If IsNothing(nlHealthCarefacility.Item(0)) Then
                udtErrorList.Add(ErrorCodeList.I00078)
                Exit Sub
            End If

            ReadPracticeID(nlHealthCarefacility.Item(0), objNamespaceMgr)
            ReadPracticeName(nlHealthCarefacility.Item(0), objNamespaceMgr)

            ''''''''''''''''''Continue?'''''''''''''''''''
            If Not IsNothing(_udtErrorList) AndAlso _udtErrorList.Count > 0 Then
                Exit Sub
            End If

            '---------------------------
            ' Account Info (Document Type Related)
            '---------------------------
            Dim nlAct As XmlNodeList = xmlDoc.SelectNodes("//ab:component" + _
                                                        "/ab:structuredBody" + _
                                                        "/ab:component" + _
                                                        "/ab:section" + _
                                                        "/ab:entry" + _
                                                        "/ab:act[@classCode='ACT'][@moodCode='EVN']" + _
                                                        "/ab:entryRelationship[@typeCode='COMP']" + _
                                                        "/ab:act[@classCode='ACT'][@moodCode='EVN']", objNamespaceMgr)
            If IsNothing(nlAct.Item(0)) Then
                udtErrorList.Add(ErrorCodeList.I00081)
                Exit Sub
            End If

            ReadDocCode(nlAct.Item(0), objNamespaceMgr)
            If IsNothing(Me.DocType) Then
                Exit Sub
            End If

            'Read Account Detail Info
            Dim nlQualifier As XmlNodeList = nlAct.Item(0).SelectNodes("./ab:code/ab:qualifier", objNamespaceMgr)
            If IsNothing(nlQualifier) Then
                udtErrorList.Add(ErrorCodeList.I00083)
                Exit Sub
            End If
            ReadDocAdditionalInfo(nlQualifier, objNamespaceMgr)

            ''''''''''''''''''Continue?'''''''''''''''''''
            If Not IsNothing(_udtErrorList) AndAlso _udtErrorList.Count > 0 Then
                Exit Sub
            End If

            '------------------------------------------------------------------------------------------------------------
            ' Voucher / Subsidy Scheme Details
            '------------------------------------------------------------------------------------------------------------
            Dim nlEntryList As XmlNodeList = xmlDoc.SelectNodes("//ab:component" + _
                                                        "/ab:structuredBody" + _
                                                        "/ab:component" + _
                                                        "/ab:section" + _
                                                        "/ab:entry[ab:substanceAdministration]", objNamespaceMgr)
            'For Vaccine
            If Not IsNothing(nlEntryList.Item(0)) Then
                For Each nlEntry As XmlNode In nlEntryList
                    ReadClaimDetail(nlEntry, objNamespaceMgr)
                Next
            End If

            Dim nlEntryNodeList As XmlNodeList = xmlDoc.SelectNodes("//ab:component" + _
                                            "/ab:structuredBody" + _
                                            "/ab:component" + _
                                            "/ab:section" + _
                                            "/ab:entry" + _
                                            "/ab:observation[@classCode='OBS'][@moodCode='EVN'][ab:code[@code='eHS-070101-sch']]", objNamespaceMgr)

            'For eVoucher
            If Not IsNothing(nlEntryNodeList) Then
                For Each nlEntry As XmlNode In nlEntryNodeList
                    ReadClaimDetail(nlEntry, objNamespaceMgr)
                Next
            End If

        End Sub

#Region "Read SP info"
        Protected Overloads Sub ReadSPID(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
            Me.SPID = ReadStringFromAttribute(node, "id", "extension", objNamespaceMgr, udtErrorList, ErrorCodeList.I00074, True, SPID_Received)
        End Sub

        Protected Sub ReadSPName(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
            Me.SPName = ReadString(node, "name", objNamespaceMgr, udtErrorList, ErrorCodeList.I00075, False)
        End Sub

        Protected Overloads Sub ReadPracticeID(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
            Me.PracticeID = ReadStringFromAttribute(node, "code", "code", objNamespaceMgr, udtErrorList, ErrorCodeList.I00079, True, PracticeID_Received)
        End Sub

        Protected Overloads Sub ReadPracticeName(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
            Me.PracticeName = ReadStringFromAttribute(node, "code", "displayName", objNamespaceMgr, udtErrorList, ErrorCodeList.I00080, True, PracticeName_Received)
        End Sub
#End Region

#Region "Read eHS Account Info"
        Protected Sub ReadDocCode(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
            Me.DocType = ReadStringFromAttribute(node, "code", "code", objNamespaceMgr, udtErrorList, ErrorCodeList.I00082, True, DocType_Received)
        End Sub

        Protected Sub ReadPatientEnglishName_family(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager, ByRef udtErrorList As ErrorInfoModelCollection)
            Me.ENameSurName = ReadString(node, "family", objNamespaceMgr, udtErrorList, ErrorCodeList.I00065, True, SurName_Received)
        End Sub

        Protected Sub ReadPatientEnglishName_Given(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager, ByRef udtErrorList As ErrorInfoModelCollection)
            Me.ENameGivenName = ReadString(node, "given", objNamespaceMgr, udtErrorList, ErrorCodeList.I00066, True, GivenName_Received)
        End Sub

        Protected Sub ReadPatientChineseName(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
            Me.NameChi = ReadStringWithAttributeSpecified(node, "name", "use", "IDE", objNamespaceMgr, udtErrorList, ErrorCodeList.I00069, False, NameChi_Received)
        End Sub

        Protected Sub ReadPatientCCCCode(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
            'Me.CCCCode = ReadStringWithAttributeSpecified(node, "name", "use", "C", objNamespaceMgr)
        End Sub

        Protected Sub ReadPatientGender(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
            Me.Gender = ReadStringFromAttribute(node, "administrativeGenderCode", "code", objNamespaceMgr, udtErrorList, ErrorCodeList.I00071, True, Gender_Received)
        End Sub

        Protected Sub ReadIdentityNo(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
            Me._IdentityNoConsistent = ReadStringFromAttributeWithAttributeSpecified(node, "id", "extension", "root", "2.16.840.1.113883.2.36.1.1.1", objNamespaceMgr, udtErrorList, ErrorCodeList.I00068, True, _IdentityNoFound)
        End Sub

        Protected Sub ReadPatientBirthTime(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
            Dim strDOB As String
            Dim dtmDOB As DateTime
            strDOB = ReadStringFromAttribute(node, "birthTime", "value", objNamespaceMgr, udtErrorList, ErrorCodeList.I00072, False, DOB_Received)
            If Date.TryParseExact(strDOB, DATE_FORMAT, Nothing, System.Globalization.DateTimeStyles.None, dtmDOB) Then
                Me.DOB = dtmDOB
            Else
                If strDOB.Trim.Length = 6 Then
                    'Reformat for DOB format (change from yyyyMM to MM-yyyy)
                    DOBinStringFormat = strDOB.Trim.Substring(4, 2) + "-" + strDOB.Trim.Substring(0, 4)
                Else
                    DOBinStringFormat = strDOB
                End If
            End If
        End Sub

        Protected Sub ReadDocAdditionalInfo(ByVal nodelist As XmlNodeList, ByVal objNamespaceMgr As XmlNamespaceManager)
            Dim nodeTmp As XmlNode

            Select Case Me.DocType.Trim
                Case "HKIC"
                    For Each node As XmlNode In nodelist
                        'HKIC
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='HKIC']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.HKIC = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.HKIC_Received = True
                            'Check consistent of identity number
                            If Not _IdentityNoConsistent.Trim = Me.HKIC.Trim Then
                                udtErrorList.Add(ErrorCodeList.I00084)
                            End If
                        End If

                        'DOI
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='DOI']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Dim sDate As String = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Dim dDate As String = Date.MinValue
                            If Date.TryParseExact(sDate, DATE_FORMAT, Nothing, System.Globalization.DateTimeStyles.None, dDate) Then
                                Me.DOI = dDate
                                Me.DOI_Received = True
                            Else
                                udtErrorList.Add(ErrorCodeList.I00032)
                                Me.DOI_Received = True
                            End If
                        End If
                    Next
                Case "EC"
                    For Each node As XmlNode In nodelist
                        'HKIC
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='HKIC']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.HKIC = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.HKIC_Received = True
                            'Check consistent of identity number
                            If Not _IdentityNoConsistent.Trim = Me.HKIC.Trim Then
                                udtErrorList.Add(ErrorCodeList.I00084)
                            End If
                        End If

                        'DOB Type
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='DOBType']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.DOBType = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.DOBType_Received = True
                        End If

                        'Age On
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='AgeOn']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.AgeOn = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.AgeOn_Received = True
                        End If

                        'Date of Registration
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='DOReg']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Dim sDate As String = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Dim dDate As String = Date.MinValue
                            If Date.TryParseExact(sDate, DATE_FORMAT, Nothing, System.Globalization.DateTimeStyles.None, dDate) Then
                                Me.DOReg = dDate
                                Me.DOReg_Received = True
                            End If
                        End If

                        'Date of Issue
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='DOI']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Dim sDate As String = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Dim dDate As String = Date.MinValue
                            If Date.TryParseExact(sDate, DATE_FORMAT, Nothing, System.Globalization.DateTimeStyles.None, dDate) Then
                                Me.DOI = dDate
                                Me.DOI_Received = True
                            Else
                                udtErrorList.Add(ErrorCodeList.I00032)
                                Me.DOI_Received = True
                            End If
                        End If

                        'Serial No
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='SerialNo']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.SerialNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.SerialNo_Received = True
                        End If

                        'Reference
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='Reference']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.Reference = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.Reference_Received = True
                        End If

                        'Free Reference
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='FreeRef']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Dim strFreeReference As String = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            If Not IsNothing(strFreeReference) AndAlso strFreeReference.Trim.ToUpper() = "Y" Then
                                Me.FreeReference = True
                                Me.FreeRef_Received = True
                            End If
                            If Not IsNothing(strFreeReference) AndAlso strFreeReference.Trim.ToUpper() = "N" Then
                                Me.FreeReference = False
                                Me.FreeRef_Received = True
                            End If
                            'Dim blnFreeReference As Boolean
                            'If Boolean.TryParse(strFreeReference, blnFreeReference) Then
                            '    Me.FreeReference = blnFreeReference
                            '    Me.FreeRef_Received = True
                            'End If
                        End If
                    Next
                Case "ADOPC"
                    For Each node As XmlNode In nodelist
                        'No of Entry
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='EntryNo']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.EntryNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.EntryNo_Received = True
                            'Check consistent of identity number
                            If Not _IdentityNoConsistent.Trim = Me.EntryNo.Trim Then
                                udtErrorList.Add(ErrorCodeList.I00084)
                            End If
                        End If

                        'DOB In Word
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='DOBInWord']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.DOBInWord = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.DOBInWord_Received = True
                        End If
                    Next
                Case "HKBC"
                    For Each node As XmlNode In nodelist
                        'Registration No
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='RegNo']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.RegNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.RegNo_Received = True
                            'Check consistent of identity number
                            If Not _IdentityNoConsistent.Trim = Me.RegNo.Trim Then
                                udtErrorList.Add(ErrorCodeList.I00084)
                            End If
                        End If

                        'DOB In Word
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='DOBInWord']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.DOBInWord = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.DOBInWord_Received = True
                        End If
                    Next
                Case "DOC/I"
                    For Each node As XmlNode In nodelist
                        'Document No
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='DocumentNo']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.DocumentNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.DocumentNo_Received = True
                            'Check consistent of identity number
                            If Not _IdentityNoConsistent.Trim = Me.DocumentNo.Trim Then
                                udtErrorList.Add(ErrorCodeList.I00084)
                            End If
                        End If

                        'Date of Issue
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='DOI']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Dim sDate As String = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Dim dDate As String = Date.MinValue
                            If Date.TryParseExact(sDate, DATE_FORMAT, Nothing, System.Globalization.DateTimeStyles.None, dDate) Then
                                Me.DOI = dDate
                                Me.DOI_Received = True
                            Else
                                udtErrorList.Add(ErrorCodeList.I00032)
                                Me.DOI_Received = True
                            End If
                        End If
                    Next
                Case "REPMT"
                    For Each node As XmlNode In nodelist
                        'Re-entry permit No
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='PermitNo']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.PermitNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.PermitNo_Received = True
                            'Check consistent of identity number
                            If Not _IdentityNoConsistent.Trim = Me.PermitNo.Trim Then
                                udtErrorList.Add(ErrorCodeList.I00084)
                            End If
                        End If

                        'Date of Issue
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='DOI']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Dim sDate As String = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Dim dDate As String = Date.MinValue
                            If Date.TryParseExact(sDate, DATE_FORMAT, Nothing, System.Globalization.DateTimeStyles.None, dDate) Then
                                Me.DOI = dDate
                                Me.DOI_Received = True
                            Else
                                udtErrorList.Add(ErrorCodeList.I00032)
                                Me.DOI_Received = True
                            End If
                        End If
                    Next
                Case "ID235B"
                    For Each node As XmlNode In nodelist
                        'Birth Entry No
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='BirthEntryNo']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.BirthEntryNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.BirthEntryNo_Received = True
                            'Check consistent of identity number
                            If Not _IdentityNoConsistent.Trim = Me.BirthEntryNo.Trim Then
                                udtErrorList.Add(ErrorCodeList.I00084)
                            End If
                        End If

                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='RemainUntil']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Dim sDate As String = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Dim dDate As String = Date.MinValue
                            If Date.TryParseExact(sDate, DATE_FORMAT, Nothing, System.Globalization.DateTimeStyles.None, dDate) Then
                                Me.RemainUntil = dDate
                                Me.RemainUntil_Received = True
                            End If
                        End If
                    Next
                Case "VISA"
                    For Each node As XmlNode In nodelist
                        'Visa/Reference No
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='VISANo']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.VISANo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.VISANo_Received = True
                            'Check consistent of identity number
                            If Not _IdentityNoConsistent.Trim = Me.VISANo.Trim Then
                                udtErrorList.Add(ErrorCodeList.I00084)
                            End If
                        End If
                        'Passport No
                        nodeTmp = node.SelectSingleNode("./ab:name[@displayName='PassportNo']", objNamespaceMgr)
                        If Not IsNothing(nodeTmp) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes) AndAlso Not IsNothing(nodeTmp.NextSibling.Attributes.GetNamedItem("code")) Then
                            Me.PassportNo = nodeTmp.NextSibling.Attributes.GetNamedItem("code").Value
                            Me.PassportNo_Received = True
                        End If
                    Next
            End Select
        End Sub

#End Region

#Region "Read Claim Info"
        Protected Sub ReadServiceDate(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
            Dim strServiceDate As String
            Dim dtmServiceDate As DateTime
            strServiceDate = ReadStringFromAttribute(node, "effectiveTime", "value", objNamespaceMgr, udtErrorList, ErrorCodeList.I00010, True, ServiceDate_Received)

            'For later checking (consistent)
            If IsNothing(strServiceDate) Then
                Exit Sub
            Else
                _ServiceDateConsistent = strServiceDate
            End If

            If Date.TryParseExact(strServiceDate, DATE_FORMAT, Nothing, System.Globalization.DateTimeStyles.None, dtmServiceDate) Then
                Me.ServiceDate = dtmServiceDate
            Else
                Me.udtErrorList.Add(ErrorCodeList.I00077)
            End If
        End Sub

        Protected Sub ReadClaimDetail(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager)
            Dim nodeTmp As XmlNode
            Dim blnProcceed As Boolean = True
            Dim intCurrentIndex As Integer = 0
            Dim intItemIndexForVoucher As Integer = 0
            Dim intItemIndexForVaccine As Integer = 0
            Dim strSchemeCode As String = String.Empty
            Dim strSubsidyCode As String = String.Empty
            Dim strDoseSeq As String = String.Empty
            Dim strVoucherClaimed As String = String.Empty

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

            ' -----------------------------------------------------------------------------------------

            Dim strCoPaymentFee As String = String.Empty

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            Dim IsEVoucher As Boolean = False
            Dim strServiceDateRetrievedFromClaimInfo As String = String.Empty

            '------------------------------------------
            ' HCVS
            '------------------------------------------
            'Scheme Code
            nodeTmp = node.SelectSingleNode("./ab:entryRelationship[@typeCode='COMP']/ab:act[@classCode='ACT'][@moodCode='EVN']/ab:code[@code='HCVS'][@displayName='SchemeCode']", objNamespaceMgr)
            If Not IsNothing(nodeTmp) Then
                strSchemeCode = "HCVS"
            End If
            'Subsidy Code
            nodeTmp = node.SelectSingleNode("./ab:entryRelationship[@typeCode='COMP']/ab:act[@classCode='ACT'][@moodCode='EVN']/ab:code[@code='EHCV'][@displayName='SubsidyCode']", objNamespaceMgr)
            If Not IsNothing(nodeTmp) Then
                strSubsidyCode = "EHCVS"
            End If
            'Voucher Claimed
            nodeTmp = node.SelectSingleNode("./ab:entryRelationship[@typeCode='COMP']/ab:act[@classCode='ACT'][@moodCode='EVN']/ab:code[@displayName='VoucherClaimed']", objNamespaceMgr)
            If Not IsNothing(nodeTmp) Then
                strVoucherClaimed = nodeTmp.Attributes.GetNamedItem("code").Value
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

            ' -----------------------------------------------------------------------------------------

            'Co-Payment Fee
            nodeTmp = node.SelectSingleNode("./ab:entryRelationship[@typeCode='COMP']/ab:observation[@classCode='OBS'][@moodCode='EVN']", objNamespaceMgr)
            If Not IsNothing(nodeTmp) Then
                strCoPaymentFee = ReadString(nodeTmp, "value", objNamespaceMgr, udtErrorList, ErrorCodeList.I00113, False)
            Else
                strCoPaymentFee = Nothing
            End If

            If Not (New Common.ComFunction.GeneralFunction).IsCoPaymentFeeEnabled(Me.ServiceDate) And strCoPaymentFee IsNot Nothing Then
                udtErrorList.Add(ErrorCodeList.I00114)
                Exit Sub
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            If Not strSchemeCode.Trim = String.Empty And Not strSubsidyCode.Trim = String.Empty And Not strVoucherClaimed.Trim = String.Empty Then
                IsEVoucher = True
                'Service Date
                strServiceDateRetrievedFromClaimInfo = ReadStringFromAttribute(node, "effectiveTime", "value", objNamespaceMgr, udtErrorList, ErrorCodeList.I00010, True)
                If IsNothing(strServiceDateRetrievedFromClaimInfo) Then
                    udtErrorList.Add(ErrorCodeList.I00077)
                    Exit Sub
                Else
                    If Not _ServiceDateConsistent.Trim = strServiceDateRetrievedFromClaimInfo Then
                        udtErrorList.Add(ErrorCodeList.I00085)
                        Exit Sub
                    End If
                End If
            Else
                If Not (strSchemeCode.Trim = String.Empty And strSubsidyCode.Trim = String.Empty And strVoucherClaimed.Trim = String.Empty) Then
                    udtErrorList.Add(ErrorCodeList.I00086)
                    Exit Sub
                End If
            End If

            '------------------------------------------
            ' Vaccination
            '------------------------------------------
            If Not IsEVoucher Then
                'Service Date
                nodeTmp = node.SelectSingleNode("./ab:substanceAdministration", objNamespaceMgr)
                If Not IsNothing(nodeTmp) Then
                    strServiceDateRetrievedFromClaimInfo = ReadStringFromAttribute(nodeTmp, "effectiveTime", "value", objNamespaceMgr, udtErrorList, ErrorCodeList.I00010, True)
                    If IsNothing(strServiceDateRetrievedFromClaimInfo) Then
                        udtErrorList.Add(ErrorCodeList.I00077)
                        Exit Sub
                    Else
                        If Not _ServiceDateConsistent.Trim = strServiceDateRetrievedFromClaimInfo Then
                            udtErrorList.Add(ErrorCodeList.I00085)
                            Exit Sub
                        End If
                    End If
                End If

                'Scheme Code
                nodeTmp = node.SelectSingleNode("./ab:substanceAdministration/ab:entryRelationship[@typeCode='COMP']/ab:act[@classCode='ACT'][@moodCode='EVN']", objNamespaceMgr)
                If Not IsNothing(nodeTmp) Then
                    strSchemeCode = ReadStringFromAttribute(nodeTmp, "code", "code", objNamespaceMgr, udtErrorList, ErrorCodeList.I00087, False)
                End If
                'Subsidy Code
                nodeTmp = node.SelectSingleNode("./ab:substanceAdministration/ab:consumable/ab:manufacturedProduct/ab:manufacturedMaterial", objNamespaceMgr)
                If Not IsNothing(nodeTmp) Then
                    strSubsidyCode = ReadStringFromAttribute(nodeTmp, "code", "code", objNamespaceMgr, udtErrorList, ErrorCodeList.I00088, False)
                End If
                'Dose Sequence
                nodeTmp = node.SelectSingleNode("./ab:substanceAdministration/ab:entryRelationship[@typeCode='SUBJ']/ab:observation[@classCode='OBS'][@moodCode='EVN']", objNamespaceMgr)
                If Not IsNothing(nodeTmp) Then
                    strDoseSeq = ReadString(nodeTmp, "value", objNamespaceMgr, udtErrorList, ErrorCodeList.I00089, False)
                End If
            End If

            '--------------------------
            'Fill the values to the model
            '--------------------------
            If IsNothing(Me.WSClaimDetailList) Then
                Me.WSClaimDetailList = New WSClaimDetailModelCollection()
            End If

            If Me.WSClaimDetailList.Count = 0 Then
                Me.WSClaimDetailList.Add(New WSClaimDetailModel)
            Else
                intCurrentIndex = Me.WSClaimDetailList.getItemIndexBySchemeCode(strSchemeCode)
                If intCurrentIndex < 0 Then
                    Me.WSClaimDetailList.Add(New WSClaimDetailModel)
                    intCurrentIndex = Me.WSClaimDetailList.Count - 1
                End If
            End If

            'Scheme Code
            Me.WSClaimDetailList.Item(intCurrentIndex).SchemeCode = strSchemeCode
            If Not strSchemeCode.Trim = String.Empty Then
                Me.WSClaimDetailList.Item(intCurrentIndex).SchemeCode_Received = True
            End If

            If IsEVoucher Then
                '--------------------------
                'for HCVS
                '--------------------------
                If IsNothing(Me.WSClaimDetailList.Item(intCurrentIndex).WSVoucherList) Then
                    Me.WSClaimDetailList.Item(intCurrentIndex).WSVoucherList = New WSVoucherModelCollection()
                End If
                Me.WSClaimDetailList.Item(intCurrentIndex).WSVoucherList.Add(New WSVoucherModel)
                intItemIndexForVoucher = Me.WSClaimDetailList.Item(intCurrentIndex).WSVoucherList.Count - 1

                'Voucher Claimed
                Dim intVoucherClaimed As Integer = 0
                If Integer.TryParse(strVoucherClaimed, intVoucherClaimed) Then
                    Me.WSClaimDetailList.Item(intCurrentIndex).WSVoucherList(intItemIndexForVoucher).VoucherClaimed = intVoucherClaimed
                    Me.WSClaimDetailList.Item(intCurrentIndex).WSVoucherList(intItemIndexForVoucher).VoucherClaimed_Received = True
                Else
                    udtErrorList.Add(ErrorCodeList.I00090)
                    Exit Sub
                End If

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

                ' -----------------------------------------------------------------------------------------

                'Co Payment Fee
                Dim intCoPaymentFee As Integer = 0
                If Not strCoPaymentFee = String.Empty Then
                    If Integer.TryParse(strCoPaymentFee, intCoPaymentFee) Then
                        Me.WSClaimDetailList.Item(intCurrentIndex).WSVoucherList(intItemIndexForVoucher).CoPaymentFee = intCoPaymentFee
                        Me.WSClaimDetailList.Item(intCurrentIndex).WSVoucherList(intItemIndexForVoucher).CoPaymentFee_Received = True
                    Else
                        udtErrorList.Add(ErrorCodeList.I00113)
                        Exit Sub
                    End If
                Else
                    Me.WSClaimDetailList.Item(intCurrentIndex).WSVoucherList(intItemIndexForVoucher).CoPaymentFee = Nothing
                    Me.WSClaimDetailList.Item(intCurrentIndex).WSVoucherList(intItemIndexForVoucher).CoPaymentFee_Received = False
                End If

                'For retrieving Prof Code and Reason for visit
                ReadProfCodeANDReasonForVisit(node, objNamespaceMgr, Me.WSClaimDetailList.Item(intCurrentIndex).WSVoucherList(intItemIndexForVoucher))

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                'For TSWInd
                ReadTSWIndicator(node, objNamespaceMgr, Me.WSClaimDetailList.Item(intCurrentIndex))
            Else
                '--------------------------
                'for Vaccination
                '--------------------------
                If IsNothing(Me.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList) Then
                    Me.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList = New WSVaccineDetailModelCollection()
                End If
                Me.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList.Add(New WSVaccineDetailModel)
                intItemIndexForVaccine = Me.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList.Count - 1

                'Subsidy Code
                Me.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList.Item(intItemIndexForVaccine).SubsidyCode = strSubsidyCode
                If Not strSubsidyCode.Trim = String.Empty Then
                    Me.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList.Item(intItemIndexForVaccine).SubsidyCode_Received = True
                End If
                'Dose Seq
                Me.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList.Item(intItemIndexForVaccine).DoseSeq = strDoseSeq
                If Not strDoseSeq.Trim = String.Empty Then
                    Me.WSClaimDetailList.Item(intCurrentIndex).WSVaccineDetailList.Item(intItemIndexForVaccine).DoseSeq_Received = True
                End If

                'For retrieving RCH Code
                ReadRCHCode(node, objNamespaceMgr, Me.WSClaimDetailList.Item(intCurrentIndex))
                'For PreSchool indicator
                ReadDoseIntervalIndicator(node, objNamespaceMgr, Me.WSClaimDetailList.Item(intCurrentIndex))
                'For DoseInterval
                ReadPreSchoolIndicator(node, objNamespaceMgr, Me.WSClaimDetailList.Item(intCurrentIndex))
            End If

        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

        ' -----------------------------------------------------------------------------------------

        Protected Sub ReadProfCodeANDReasonForVisit(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager, ByRef udtWSVoucherModel As WSVoucherModel)

            Dim nlTemp As XmlNodeList
            Dim nodeTmp As XmlNode
            Dim subNodeTmp As XmlNode

            nlTemp = node.SelectNodes("./ab:entryRelationship[@typeCode='RSON']" + _
                                        "/ab:observation[@classCode='OBS'][@moodCode='EVN']", objNamespaceMgr)

            Dim udtReasonForVisitModel As ReasonForVisitModel

            For Each nodeTmp In nlTemp

                udtReasonForVisitModel = New ReasonForVisitModel

                udtReasonForVisitModel.ProfCode = ReadStringFromAttributeWithAttributeSpecified(nodeTmp, "code", "code", "displayName", "ProfessionalCode", objNamespaceMgr, udtErrorList, ErrorCodeList.I00091, True, udtReasonForVisitModel.ProfCode_Received)

                subNodeTmp = nodeTmp.SelectSingleNode("./ab:entryRelationship[@typeCode='RSON']" + _
                                                        "/ab:act[@classCode='ACT'][@moodCode='EVN']", objNamespaceMgr)

                If Not IsNothing(subNodeTmp) Then

                    udtReasonForVisitModel.PriorityCode = ReadStringFromAttribute(subNodeTmp, "priorityCode", "code", objNamespaceMgr, udtErrorList, ErrorCodeList.I00091, False, udtReasonForVisitModel.PriorityCode_Received)

                    If udtReasonForVisitModel.PriorityCode = String.Empty Then
                        udtReasonForVisitModel.PriorityCode = 1
                        udtReasonForVisitModel.PriorityCode_Received = True
                    End If

                    udtReasonForVisitModel.L1Code = ReadStringFromAttributeWithAttributeSpecified(subNodeTmp, "code", "code", "displayName", "L1Code", objNamespaceMgr, udtErrorList, ErrorCodeList.I00092, True, udtReasonForVisitModel.L1Code_Received)
                    udtReasonForVisitModel.L1DescEng = ReadString(subNodeTmp, "text", objNamespaceMgr, udtErrorList, ErrorCodeList.I00093, True, udtReasonForVisitModel.L1DescEng_Received)

                End If

                subNodeTmp = subNodeTmp.SelectSingleNode("./ab:entryRelationship[@typeCode='COMP']" + _
                                                        "/ab:act[@classCode='ACT'][@moodCode='EVN']", objNamespaceMgr)

                If Not IsNothing(subNodeTmp) Then

                    udtReasonForVisitModel.L2Code = ReadStringFromAttributeWithAttributeSpecified(subNodeTmp, "code", "code", "displayName", "L2Code", objNamespaceMgr, udtErrorList, ErrorCodeList.I00094, True, udtReasonForVisitModel.L2Code_Received)
                    udtReasonForVisitModel.L2DescEng = ReadString(subNodeTmp, "text", objNamespaceMgr, udtErrorList, ErrorCodeList.I00095, True, udtReasonForVisitModel.L2DescEng_Received)

                End If

                udtWSVoucherModel.ReasonForVisitModelCollection.Add(udtReasonForVisitModel)
            Next

            If Not (New Common.ComFunction.GeneralFunction).IsCoPaymentFeeEnabled(Me.ServiceDate) And udtWSVoucherModel.ReasonForVisitModelCollection.Count > 1 Then
                udtErrorList.Add(ErrorCodeList.I00114)
                Exit Sub
            End If

        End Sub

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Protected Sub ReadRCHCode(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager, ByRef udtWSClaimDetail As WSClaimDetailModel)
            Dim nodeTmp As XmlNode
            '--------------------------
            'RCH Code
            '--------------------------
            nodeTmp = node.SelectSingleNode("./ab:substanceAdministration/ab:entryRelationship[@typeCode='COMP']" + _
                                            "/ab:observation[@classCode='OBS'][@moodCode='EVN'][ab:code[@displayName='RCHCode']]", objNamespaceMgr)
            If Not IsNothing(nodeTmp) Then
                udtWSClaimDetail.RCHCode = ReadStringFromAttributeWithAttributeSpecified(nodeTmp, "code", "code", "displayName", "RCHCode", objNamespaceMgr, udtErrorList, ErrorCodeList.I00099, False, udtWSClaimDetail.RCHCode_Received)
            End If
        End Sub

        Protected Sub ReadDoseIntervalIndicator(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager, ByRef udtWSClaimDetail As WSClaimDetailModel)
            Dim nodeTmp As XmlNode
            '--------------------------
            'Dose Interval Indicator
            '--------------------------
            nodeTmp = node.SelectSingleNode("./ab:substanceAdministration/ab:entryRelationship[@typeCode='COMP']" + _
                                            "/ab:observation[@classCode='OBS'][@moodCode='EVN'][ab:code[@displayName='DoseIntervalInd']]", objNamespaceMgr)
            If Not IsNothing(nodeTmp) Then
                udtWSClaimDetail.DoseIntervalInd = ReadStringFromAttributeWithAttributeSpecified(nodeTmp, "code", "code", "displayName", "DoseIntervalInd", objNamespaceMgr, udtErrorList, ErrorCodeList.I00098, False, udtWSClaimDetail.DoseIntervalInd_Received)
            End If
        End Sub

        Protected Sub ReadPreSchoolIndicator(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager, ByRef udtWSClaimDetail As WSClaimDetailModel)
            Dim nodeTmp As XmlNode
            '--------------------------
            'Pre-School Indicator
            '--------------------------
            nodeTmp = node.SelectSingleNode("./ab:substanceAdministration/ab:entryRelationship[@typeCode='COMP']" + _
                                            "/ab:observation[@classCode='OBS'][@moodCode='EVN'][ab:code[@displayName='PreSchoolInd']]", objNamespaceMgr)
            If Not IsNothing(nodeTmp) Then
                udtWSClaimDetail.PreSchoolInd = ReadStringFromAttributeWithAttributeSpecified(nodeTmp, "code", "code", "displayName", "PreSchoolInd", objNamespaceMgr, udtErrorList, ErrorCodeList.I00097, False, udtWSClaimDetail.PreSchoolInd_Received)
            End If
        End Sub

        Protected Sub ReadTSWIndicator(ByVal node As XmlNode, ByVal objNamespaceMgr As XmlNamespaceManager, ByRef udtWSClaimDetail As WSClaimDetailModel)
            Dim nodeTmp As XmlNode
            '--------------------------
            'TSW Indicator
            '--------------------------
            nodeTmp = node.SelectSingleNode("./ab:entryRelationship[@typeCode='COMP']" + _
                                            "/ab:observation[@classCode='OBS'][@moodCode='EVN'][ab:code[@displayName='TSWInd']]", objNamespaceMgr)
            If Not IsNothing(nodeTmp) Then
                udtWSClaimDetail.TSWInd = ReadStringFromAttributeWithAttributeSpecified(nodeTmp, "code", "code", "displayName", "TSWInd", objNamespaceMgr, udtErrorList, ErrorCodeList.I00096, False, udtWSClaimDetail.TSWInd_Received)
            End If
        End Sub

#End Region

#End Region

#Region "Supporting functions (Retrieving Data)"
        'Protected Overloads Function ReadString(ByVal node As XmlNode, _
        '                    ByVal sTagName As String, _
        '                    ByVal objNamespaceMgr As XmlNamespaceManager)

        '    Dim nlTemp As XmlNodeList
        '    nlTemp = node.SelectNodes("./ab:" + sTagName, objNamespaceMgr)

        '    Return nlTemp(0).InnerText
        'End Function

        'Protected Function ReadStringWithAttributeSpecified(ByVal node As XmlNode, _
        '                ByVal sTagName As String, _
        '                ByVal sAttributeName As String, _
        '                ByVal sAttributeValue As String, _
        '                ByVal objNamespaceMgr As XmlNamespaceManager)

        '    Dim nlTemp As XmlNode
        '    nlTemp = node.SelectSingleNode("./ab:" + sTagName + "[@" + sAttributeName + "='" + sAttributeValue + "']", objNamespaceMgr)

        '    Return nlTemp.InnerText
        'End Function

        'Protected Function ReadStringFromAttribute(ByVal node As XmlNode, _
        '                        ByVal sTagName As String, _
        '                        ByVal sAttributeName As String, _
        '                        ByVal objNamespaceMgr As XmlNamespaceManager)

        '    Dim nlTemp As XmlNode
        '    nlTemp = node.SelectSingleNode("./ab:" + sTagName, objNamespaceMgr)

        '    If Not IsNothing(nlTemp.Attributes.GetNamedItem(sAttributeName)) Then
        '        Return nlTemp.Attributes.GetNamedItem(sAttributeName).InnerText
        '    End If

        '    Return Nothing
        'End Function

        'Protected Function ReadStringFromAttributeWithAttributeSpecified(ByVal node As XmlNode, _
        '                        ByVal sTagName As String, _
        '                        ByVal sAttributeName As String, _
        '                        ByVal sSpecifiedAttributeName As String, _
        '                        ByVal sSpecifiedAttributeValue As String, _
        '                        ByVal objNamespaceMgr As XmlNamespaceManager)

        '    Dim nlTemp As XmlNode
        '    nlTemp = node.SelectSingleNode("./ab:" + sTagName + "[@" + sSpecifiedAttributeName + "='" + sSpecifiedAttributeValue + "']", objNamespaceMgr)

        '    If Not IsNothing(nlTemp.Attributes.GetNamedItem(sAttributeName)) Then
        '        Return nlTemp.Attributes.GetNamedItem(sAttributeName).InnerText
        '    End If

        '    Return Nothing
        'End Function

        Protected Overloads Function ReadString(ByVal node As XmlNode, _
                        ByVal sTagName As String, _
                        ByVal objNamespaceMgr As XmlNamespaceManager, _
                        ByRef udtErrorList As ErrorInfoModelCollection, _
                        ByRef strCustomError As String, _
                        Optional ByVal blnMandatory As Boolean = True, _
                        Optional ByRef blnTagFound As Boolean = False)
            Dim nlTemp As XmlNodeList
            nlTemp = node.SelectNodes("./ab:" + sTagName, objNamespaceMgr)

            If nlTemp.Count = 0 Then
                If blnMandatory Then
                    udtErrorList.Add(strCustomError)  'Incorrect XML format
                    Return Nothing
                Else
                    Return String.Empty
                End If
            ElseIf nlTemp.Count <> 1 Then
                udtErrorList.Add(strCustomError)  'Incorrect XML format
                Return Nothing
            End If

            blnTagFound = True
            Return nlTemp(0).InnerText
        End Function

        Protected Function ReadStringWithAttributeSpecified(ByVal node As XmlNode, _
                                                            ByVal sTagName As String, _
                                                            ByVal sAttributeName As String, _
                                                            ByVal sAttributeValue As String, _
                                                            ByVal objNamespaceMgr As XmlNamespaceManager, _
                                                            ByRef udtErrorList As ErrorInfoModelCollection, _
                                                            ByRef strCustomError As String, _
                                                            Optional ByVal blnMandatory As Boolean = True, _
                                                            Optional ByRef blnTagFound As Boolean = False)

            Dim nlTemp As XmlNode
            nlTemp = node.SelectSingleNode("./ab:" + sTagName + "[@" + sAttributeName + "='" + sAttributeValue + "']", objNamespaceMgr)

            If IsNothing(nlTemp) Then
                If blnMandatory Then
                    udtErrorList.Add(strCustomError)  'Incorrect XML format
                    Return Nothing
                Else
                    Return String.Empty
                End If
            Else
                blnTagFound = True
                Return nlTemp.InnerText
            End If

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

            If IsNothing(nlTemp) Then
                If blnMandatory Then
                    udtErrorList.Add(strCustomError)  'Incorrect XML format
                    Return Nothing
                Else
                    Return String.Empty
                End If
            Else
                If IsNothing(nlTemp.Attributes.GetNamedItem(sAttributeName)) Then
                    If blnMandatory Then
                        udtErrorList.Add(strCustomError)  'Incorrect XML format
                        Return Nothing
                    Else
                        Return String.Empty
                    End If
                Else
                    blnTagFound = True
                    Return nlTemp.Attributes.GetNamedItem(sAttributeName).InnerText
                End If
            End If

        End Function

        Protected Function ReadStringFromAttributeWithAttributeSpecified(ByVal node As XmlNode, _
                                                                        ByVal sTagName As String, _
                                                                        ByVal sAttributeName As String, _
                                                                        ByVal sSpecifiedAttributeName As String, _
                                                                        ByVal sSpecifiedAttributeValue As String, _
                                                                        ByVal objNamespaceMgr As XmlNamespaceManager, _
                                                                        ByRef udtErrorList As ErrorInfoModelCollection, _
                                                                        ByRef strCustomError As String, _
                                                                        Optional ByVal blnMandatory As Boolean = True, _
                                                                        Optional ByRef blnTagFound As Boolean = False)

            Dim nlTemp As XmlNode
            nlTemp = node.SelectSingleNode("./ab:" + sTagName + "[@" + sSpecifiedAttributeName + "='" + sSpecifiedAttributeValue + "']", objNamespaceMgr)

            If IsNothing(nlTemp) Then
                If blnMandatory Then
                    udtErrorList.Add(strCustomError)  'Incorrect XML format
                    Return Nothing
                Else
                    Return String.Empty
                End If
            Else
                If IsNothing(nlTemp.Attributes.GetNamedItem(sAttributeName)) Then
                    If blnMandatory Then
                        udtErrorList.Add(strCustomError)  'Incorrect XML format
                        Return Nothing
                    Else
                        Return String.Empty
                    End If
                Else
                    blnTagFound = True
                    Return nlTemp.Attributes.GetNamedItem(sAttributeName).InnerText
                End If
            End If

            Return Nothing
        End Function


#End Region

#Region "Constructor"

        Public Sub New()
            Me._bIsValid = True
        End Sub

        Public Sub New(ByVal xmlRequest As String, ByRef udtAuditLog As ExtAuditLogEntry)
            Dim xml As New XmlDocument()
            'For Logging
            Me.ExtAuditLogEntry = udtAuditLog

            Try
                '---------------------------------
                '(Step 1) Read XML
                '---------------------------------
                xml.LoadXml(xmlRequest)
            Catch ex As Exception
                Me._bIsValid = False
                Me.Errors.Add(ErrorCodeList.I00003)
                Exit Sub
            End Try

            Try
                ReadData(xml, Me.Errors)

                CheckMessageID(Me.Errors)
                'Assign Message ID to Audit Log
                If Me.Errors.Count = 0 Then
                    udtAuditLog.MessageID = _strMessageID
                End If
                'Assign SP ID to Audit Log
                If Not SPID Is Nothing AndAlso Not SPID.Trim = String.Empty Then
                    udtAuditLog.UserID = SPID
                End If

                '---------------------------------
                '(Step 2) Check whether there is missing or duplicate fields 
                '---------------------------------
                If Me.Errors.Count = 0 Then
                    Me._bIsValid = CheckSPXMLField(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00057)
                Else
                    WriteLogWithErrorList(LogID.LOG00057)
                End If

                If Me._bIsValid Then
                    Me._bIsValid = CheckEHSAccountXMLField(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00058)
                End If

                If Me._bIsValid Then
                    Me._bIsValid = CheckClaimXMLField(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00059)
                End If

                '---------------------------------
                '(Step 3) Check SP / eHS account / Claim fields format 
                '---------------------------------
                If Me._bIsValid Then
                    Me._bIsValid = ValidatServiceProviderInfo(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00060)
                End If

                If Me._bIsValid Then
                    'Special handling on Date Fields
                    SpecialHandlingOnDOB()
                    SpecialHandlingOnDOI()
                    SpecialHandlingOnDOReg()
                    SpecialHandlingOnRemaiUntil()

                    Me._bIsValid = ValidateEHSAccountInfo(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00061)
                End If


                If Me._bIsValid Then
                    Me._bIsValid = ValidateClaimInfo(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00062)
                End If


                '---------------------------------
                '(Step 4) Addtional Checking on Document Limit
                'e.g. Certificate of Exemption would be accepted for person at the age of 11 or above. / Document Limit
                '---------------------------------
                If Me._bIsValid Then
                    Me._bIsValid = Me.CheckDocumentLimitByAge(Me.Errors, WSClaimDetailList, Me.ServiceDate)
                    WriteLogWithErrorList(LogID.LOG00065)
                End If

            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.UploadClaim).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                Me.Errors.Add(ErrorCodeList.I99999)
                Me._bIsValid = False
            End Try

        End Sub

#End Region

    End Class



End Namespace
