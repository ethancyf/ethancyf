Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Validation
Imports TestWSforHKMA.Component.Request.Base
Imports TestWSforHKMA.Component

Namespace Component.Request

    Public Class UploadClaimRequest
        Inherits BaseWSClaimRequest


#Region "Constructor"

        Public Sub New()


        End Sub

#End Region

#Region "Generate XML Result"

        Public Function GenXMLResult() As XmlDocument
            Dim xml As New XmlDocument()



            Dim nodeResult As XmlElement
            nodeResult = xml.CreateElement("Request")
            Dim xmlDeclaration As XmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", Nothing)
            xml.InsertBefore(xmlDeclaration, xml.DocumentElement)
            xml.AppendChild(nodeResult)

            Dim nodeInput As XmlElement
            nodeInput = xml.CreateElement(TAG_INPUT)
            nodeResult.AppendChild(nodeInput)


            'SP Info
            If Me.SPInfo_inXML Then
                GenSPResult(xml, nodeInput)
            End If

            'Account Info
            If Me.AccountInfo_inXML Then
                GenAccountResult(xml, nodeInput)
            End If

            'Claim Info
            If Me.ClaimInfo_inXML Then
                GenClaimInfoResult(xml, nodeInput)
            End If

            Return xml
        End Function


        Private Sub GenSPResult(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)

            Dim nodeSPInfo As XmlElement
            nodeSPInfo = xml.CreateElement(TAG_SP_INFO)

            If Me.SPID_Included Then
                Dim nodeSPID As XmlElement
                nodeSPID = xml.CreateElement(TAG_SPID)
                nodeSPID.InnerText = Me.SPID
                nodeSPInfo.AppendChild(nodeSPID)
            End If

            If Me.PracticeID_included Then
                Dim nodePracticeID As XmlElement
                nodePracticeID = xml.CreateElement(TAG_PRACTICE_ID)
                nodePracticeID.InnerText = Me.PracticeID
                nodeSPInfo.AppendChild(nodePracticeID)
            End If

            If Me.PracticeName_included Then
                Dim nodePracticeName As XmlElement
                nodePracticeName = xml.CreateElement(TAG_PRACTICE_NAME)
                nodePracticeName.InnerText = Me.PracticeName
                nodeSPInfo.AppendChild(nodePracticeName)
            End If

            If Me.SPSurname_Included Then
                Dim nodeSPName As XmlElement
                nodeSPName = xml.CreateElement("SPSurname")
                nodeSPName.InnerText = Me.SPSurname
                nodeSPInfo.AppendChild(nodeSPName)
            End If

            If Me.SPSurname_Included Then
                Dim nodeSPName As XmlElement
                nodeSPName = xml.CreateElement("SPGivenName")
                nodeSPName.InnerText = Me.SPGivenName
                nodeSPInfo.AppendChild(nodeSPName)
            End If

            nodeResult.AppendChild(nodeSPInfo)
        End Sub

        Private Sub GenAccountResult(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)

            Dim nodeAccountInfo As XmlElement
            nodeAccountInfo = xml.CreateElement(TAG_EHS_ACCOUNT_INFO)

            If Me.DocType_Included Then
                Dim nodeDocType As XmlElement
                nodeDocType = xml.CreateElement(TAG_DOC_TYPE)
                nodeDocType.InnerText = Me.DocType
                nodeAccountInfo.AppendChild(nodeDocType)
            End If

            If Me.EntryNo_Included Then
                Dim nodeEntryNo As XmlElement
                nodeEntryNo = xml.CreateElement(TAG_ENTRY_NO)
                nodeEntryNo.InnerText = Me.EntryNo
                nodeAccountInfo.AppendChild(nodeEntryNo)
            End If

            If Me.DocumentNo_Included Then
                Dim nodeDocumentNo As XmlElement
                nodeDocumentNo = xml.CreateElement(TAG_DOCUMENT_NO)
                nodeDocumentNo.InnerText = Me.DocumentNo
                nodeAccountInfo.AppendChild(nodeDocumentNo)
            End If

            If Me.HKIC_Included Then
                Dim nodeHKIC As XmlElement
                nodeHKIC = xml.CreateElement(TAG_HKIC)
                nodeHKIC.InnerText = Me.HKIC
                nodeAccountInfo.AppendChild(nodeHKIC)
            End If

            If Me.RegNo_Included Then
                Dim nodeRegNo As XmlElement
                nodeRegNo = xml.CreateElement(TAG_REG_NO)
                nodeRegNo.InnerText = Me.RegNo
                nodeAccountInfo.AppendChild(nodeRegNo)
            End If

            If Me.BirthEntryNo_Included Then
                Dim nodeBirthEntryNo As XmlElement
                nodeBirthEntryNo = xml.CreateElement(TAG_BIRTH_ENTRY_NO)
                nodeBirthEntryNo.InnerText = Me.BirthEntryNo
                nodeAccountInfo.AppendChild(nodeBirthEntryNo)
            End If

            If Me.PermitNo_Included Then
                Dim nodePermitNo As XmlElement
                nodePermitNo = xml.CreateElement(TAG_PERMIT_NO)
                nodePermitNo.InnerText = Me.PermitNo
                nodeAccountInfo.AppendChild(nodePermitNo)
            End If

            If Me.VISANo_Included Then
                Dim nodeVISANo As XmlElement
                nodeVISANo = xml.CreateElement(TAG_VISA_NO)
                nodeVISANo.InnerText = Me.VISANo
                nodeAccountInfo.AppendChild(nodeVISANo)
            End If

            If Me.Surname_Included Then
                Dim nodeNameEng As XmlElement
                nodeNameEng = xml.CreateElement(TAG_SURNAME)
                nodeNameEng.InnerText = Me.Surname
                nodeAccountInfo.AppendChild(nodeNameEng)
            End If

            If Me.GivenName_Included Then
                Dim nodeNameEng As XmlElement
                nodeNameEng = xml.CreateElement(TAG_GIVEN_NAME)
                nodeNameEng.InnerText = Me.GivenName
                nodeAccountInfo.AppendChild(nodeNameEng)
            End If

            If Me.Gender_Included Then
                Dim nodeGender As XmlElement
                nodeGender = xml.CreateElement(TAG_GENDER)
                nodeGender.InnerText = Me.Gender
                nodeAccountInfo.AppendChild(nodeGender)
            End If

            If Me.DOB_Included Then
                Dim nodeDOB As XmlElement
                nodeDOB = xml.CreateElement(TAG_DOB)
                nodeDOB.InnerText = Me.DOB
                nodeAccountInfo.AppendChild(nodeDOB)
            End If

            If Me.DOBType_Included Then
                Dim nodeDOBType As XmlElement
                nodeDOBType = xml.CreateElement(TAG_DOB_TYPE)
                nodeDOBType.InnerText = Me.DOBType
                nodeAccountInfo.AppendChild(nodeDOBType)
            End If

            If Me.AgeOn_Included Then
                Dim nodeAgeOn As XmlElement
                nodeAgeOn = xml.CreateElement(TAG_AGE_ON)
                nodeAgeOn.InnerText = Me.AgeOn
                nodeAccountInfo.AppendChild(nodeAgeOn)
            End If

            If Me.DOReg_Included Then
                Dim nodeDOReg As XmlElement
                nodeDOReg = xml.CreateElement(TAG_DOREG)
                nodeDOReg.InnerText = Me.DOReg
                nodeAccountInfo.AppendChild(nodeDOReg)
            End If

            If Me.DOBInWord_Included Then
                Dim nodeDOBInWord As XmlElement
                nodeDOBInWord = xml.CreateElement(TAG_DOB_IN_WORD)
                nodeDOBInWord.InnerText = Me.DOBInWord
                nodeAccountInfo.AppendChild(nodeDOBInWord)
            End If

            If Me.NameChi_Included Then
                Dim nodeNameChi As XmlElement
                nodeNameChi = xml.CreateElement(TAG_NAME_CHI)
                nodeNameChi.InnerText = Me.NameChi
                nodeAccountInfo.AppendChild(nodeNameChi)
            End If

            If Me.DOI_Included Then
                Dim nodeDOI As XmlElement
                nodeDOI = xml.CreateElement(TAG_DOI)
                nodeDOI.InnerText = Me.DOI
                nodeAccountInfo.AppendChild(nodeDOI)
            End If

            If Me.SerialNo_Included Then
                Dim nodeSerialNo As XmlElement
                nodeSerialNo = xml.CreateElement(TAG_SERIAL_NO)
                nodeSerialNo.InnerText = Me.SerialNo
                nodeAccountInfo.AppendChild(nodeSerialNo)
            End If

            If Me.Reference_Included Then
                Dim nodeReference As XmlElement
                nodeReference = xml.CreateElement(TAG_REFERENCE)
                nodeReference.InnerText = Me.Reference
                nodeAccountInfo.AppendChild(nodeReference)
            End If

            If Me.FreeRef_Included Then
                Dim nodeFreeRef As XmlElement
                nodeFreeRef = xml.CreateElement(TAG_FREE_REF)
                nodeFreeRef.InnerText = Me.FreeReference
                nodeAccountInfo.AppendChild(nodeFreeRef)
            End If

            If Me.RemainUntil_Included Then
                Dim nodeRemainUntil As XmlElement
                nodeRemainUntil = xml.CreateElement(TAG_REMAIN_UNTIL)
                nodeRemainUntil.InnerText = Me.RemainUntil
                nodeAccountInfo.AppendChild(nodeRemainUntil)
            End If

            If Me.PassportNo_Included Then
                Dim nodePassportNo As XmlElement
                nodePassportNo = xml.CreateElement(TAG_PASSPORT_NO)
                nodePassportNo.InnerText = Me.PassportNo
                nodeAccountInfo.AppendChild(nodePassportNo)
            End If

            nodeResult.AppendChild(nodeAccountInfo)
        End Sub

        Private Sub GenClaimInfoResult(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)

            Dim nodeClaimInfo As XmlElement
            nodeClaimInfo = xml.CreateElement(TAG_CLAIM_INFO)

            If Me.ServiceDate_Included Then
                Dim nodeServiceDate As XmlElement
                nodeServiceDate = xml.CreateElement(TAG_SERVICE_DATE)
                'nodeServiceDate.InnerText = Me.ServiceDate.Value.ToString("dd-MM-yyyy")
                nodeServiceDate.InnerText = Me.ServiceDate
                nodeClaimInfo.AppendChild(nodeServiceDate)
            End If

            If Me.WSClaimDetailList.Count > 0 Then
                For Each udtWSClaimDetail As WSClaimDetailModel In WSClaimDetailList
                    GenClaimDetailResult(xml, nodeClaimInfo, udtWSClaimDetail)
                Next
            End If

            nodeResult.AppendChild(nodeClaimInfo)

        End Sub

        Private Sub GenClaimDetailResult(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement, ByVal udtWSClaimModel As WSClaimDetailModel)

            Dim nodeClaimDetail As XmlElement
            nodeClaimDetail = xml.CreateElement(TAG_CLAIM_DETAIL)

            If udtWSClaimModel.SchemeCode_Included Then
                Dim nodeSchemeCode As XmlElement
                nodeSchemeCode = xml.CreateElement(TAG_SCHEME_CODE)
                nodeSchemeCode.InnerText = udtWSClaimModel.SchemeCode
                nodeClaimDetail.AppendChild(nodeSchemeCode)
            End If

            If udtWSClaimModel.RCHCode_Included Then
                Dim nodeRCHCode As XmlElement
                nodeRCHCode = xml.CreateElement(TAG_RCH_CODE)
                nodeRCHCode.InnerText = udtWSClaimModel.RCHCode
                nodeClaimDetail.AppendChild(nodeRCHCode)
            End If

            If udtWSClaimModel.HCVS_Included Then
                GenVoucherInfoResult(xml, nodeClaimDetail, udtWSClaimModel.WSVoucher)
            End If

            If udtWSClaimModel.Vaccine_Included Then
                GenVaccineInfoResult(xml, nodeClaimDetail, udtWSClaimModel.WSVaccineDetailList)
            End If

            'Indicator
            GenIndicatorInfoResult(xml, nodeClaimDetail, udtWSClaimModel)

            nodeResult.AppendChild(nodeClaimDetail)

        End Sub

        Private Sub GenVoucherInfoResult(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement, ByVal udtWSVoucherModel As WSVoucherModel)

            Dim nodeVoucherInfo As XmlElement
            nodeVoucherInfo = xml.CreateElement(TAG_VOUCHER_INFO)

            If udtWSVoucherModel.VoucherClaimed_Included Then
                Dim nodeVoucherClaimed As XmlElement
                nodeVoucherClaimed = xml.CreateElement(TAG_VOUCHER_CLAIMED)
                nodeVoucherClaimed.InnerText = udtWSVoucherModel.VoucherClaimed
                nodeVoucherInfo.AppendChild(nodeVoucherClaimed)
            End If


            If udtWSVoucherModel.ReasonForVisit_Included Then
                Dim nodeReasonForVisit As XmlElement
                nodeReasonForVisit = xml.CreateElement(TAG_REASON_FOR_VISIT)

                If udtWSVoucherModel.ProfCode_Included Then
                    Dim nodeProfCode As XmlElement
                    nodeProfCode = xml.CreateElement(TAG_PROF_CODE)
                    nodeProfCode.InnerText = udtWSVoucherModel.ProfCode
                    nodeReasonForVisit.AppendChild(nodeProfCode)
                End If

                If udtWSVoucherModel.L1Code_Included Then
                    Dim nodeL1Code As XmlElement
                    nodeL1Code = xml.CreateElement(TAG_L1_CODE)
                    nodeL1Code.InnerText = udtWSVoucherModel.L1Code
                    nodeReasonForVisit.AppendChild(nodeL1Code)
                End If

                If udtWSVoucherModel.L1DescEng_Included Then
                    Dim nodeL1DescEng As XmlElement
                    nodeL1DescEng = xml.CreateElement(TAG_L1_DESC_ENG)
                    nodeL1DescEng.InnerText = udtWSVoucherModel.L1DescEng
                    nodeReasonForVisit.AppendChild(nodeL1DescEng)
                End If

                If udtWSVoucherModel.L2Code_Included Then
                    Dim nodeL2Code As XmlElement
                    nodeL2Code = xml.CreateElement(TAG_L2_CODE)
                    nodeL2Code.InnerText = udtWSVoucherModel.L2Code
                    nodeReasonForVisit.AppendChild(nodeL2Code)
                End If

                If udtWSVoucherModel.L2DescEng_Included Then
                    Dim nodeL2DescEng As XmlElement
                    nodeL2DescEng = xml.CreateElement(TAG_L2_DESC_ENG)
                    nodeL2DescEng.InnerText = udtWSVoucherModel.L2DescEng
                    nodeReasonForVisit.AppendChild(nodeL2DescEng)
                End If

                nodeVoucherInfo.AppendChild(nodeReasonForVisit)
            End If

            nodeResult.AppendChild(nodeVoucherInfo)
        End Sub

        Private Sub GenVaccineInfoResult(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement, ByVal udtWSVaccineList As WSVaccineDetailModelCollection)

            For Each udtVaccine As WSVaccineDetailModel In udtWSVaccineList
                Dim nodeVaccineInfo As XmlElement
                nodeVaccineInfo = xml.CreateElement(TAG_VACCINE_INFO)

                If udtVaccine.SubsidyCode_included Then
                    Dim nodeSubsidyCode As XmlElement
                    nodeSubsidyCode = xml.CreateElement(TAG_SUBSIDY_CODE)
                    nodeSubsidyCode.InnerText = udtVaccine.SubsidyCode
                    nodeVaccineInfo.AppendChild(nodeSubsidyCode)
                End If

                If udtVaccine.DoseSeq_included Then
                    Dim nodeDoseSeq As XmlElement
                    nodeDoseSeq = xml.CreateElement(TAG_DOSE_SEQ)
                    nodeDoseSeq.InnerText = udtVaccine.DoseSeq
                    nodeVaccineInfo.AppendChild(nodeDoseSeq)
                End If

                nodeResult.AppendChild(nodeVaccineInfo)
            Next

        End Sub

        Private Sub GenIndicatorInfoResult(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement, ByVal udtWSClaimDetail As WSClaimDetailModel)

            If udtWSClaimDetail.PreSchoolInd_Included Then
                Dim nodePreSchoolInd As XmlElement
                nodePreSchoolInd = xml.CreateElement(TAG_PRE_SCHOOL_IND)
                nodePreSchoolInd.InnerText = udtWSClaimDetail.PreSchoolInd
                nodeResult.AppendChild(nodePreSchoolInd)
            End If

            If udtWSClaimDetail.DoseIntervalInd_Included Then
                Dim nodeDoseIntervalInd As XmlElement
                nodeDoseIntervalInd = xml.CreateElement(TAG_DOSE_INTERVAL_IND)
                nodeDoseIntervalInd.InnerText = udtWSClaimDetail.DoseIntervalInd
                nodeResult.AppendChild(nodeDoseIntervalInd)
            End If

            If udtWSClaimDetail.TSWInd_Included Then
                Dim nodeTSWInd As XmlElement
                nodeTSWInd = xml.CreateElement(TAG_TSW_IND)
                nodeTSWInd.InnerText = udtWSClaimDetail.TSWInd
                nodeResult.AppendChild(nodeTSWInd)
            End If
        End Sub

#End Region

#Region "Generate XML Result (HL7)"

        Public Function GenXMLResult_HL7() As XmlDocument
            Dim xml As New XmlDocument()

            'Clinical Document
            Const XiNs As String = "http://www.w3.org/2000/xmlns/"
            Dim xmlDeclaration As XmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", Nothing)
            xml.InsertBefore(xmlDeclaration, xml.DocumentElement)

            xml.AppendChild(xml.CreateElement("ClinicalDocument"))
            Dim Attribute As XmlAttribute = xml.CreateAttribute("xmlns", XiNs)
            Attribute.Value = "urn:hl7-org:v3"
            xml.DocumentElement.SetAttributeNode(Attribute)

            Attribute = xml.CreateAttribute("xmlns:xsi")
            Attribute.Value = "http://www.3w.org/2001/XMLSchema-instance"
            xml.DocumentElement.SetAttributeNode(Attribute)

            Attribute = xml.CreateAttribute("xsi:schemaLocation", "http://www.3w.org/2001/XMLSchema-instance")
            Attribute.Value = "urn:hl7-org:v3 Z:\AI5\MESSAG~1\EHHL7SAM~1\CDA\CDA.xsd"
            xml.DocumentElement.SetAttributeNode(Attribute)

            'type id
            Dim typeID As XmlElement = xml.CreateElement("typeId")
            Attribute = xml.CreateAttribute("root")
            Attribute.Value = "2.16.840.1.113883.1.3"
            typeID.SetAttributeNode(Attribute)
            Attribute = xml.CreateAttribute("extension")
            Attribute.Value = "POCD_HD000040"
            typeID.SetAttributeNode(Attribute)
            xml.DocumentElement.AppendChild(typeID)

            'id
            'generate dynamic document ID
            '---------------------------------------------------------------
            Dim KeyGen As RandomKeyGenerator
            Dim RandomKey As String

            KeyGen = New RandomKeyGenerator
            KeyGen.KeyLetters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ-"
            KeyGen.KeyNumbers = "0123456789"
            KeyGen.KeyChars = 40
            RandomKey = KeyGen.Generate()
            '---------------------------------------------------------------
            Dim id As XmlElement = xml.CreateElement("id")
            Attribute = xml.CreateAttribute("assigningAuthorityName")
            Attribute.Value = "Happy HealthCare Clinic"
            id.SetAttributeNode(Attribute)
            Attribute = xml.CreateAttribute("extension")
            'Attribute.Value = "DD12345678"
            Attribute.Value = RandomKey
            id.SetAttributeNode(Attribute)
            xml.DocumentElement.AppendChild(id)

            'code
            Dim code As XmlElement = xml.CreateElement("code")
            Attribute = xml.CreateAttribute("code")
            Attribute.Value = "eHS-070101"
            code.SetAttributeNode(Attribute)
            Attribute = xml.CreateAttribute("displayName")
            Attribute.Value = "Upload Claim"
            code.SetAttributeNode(Attribute)
            xml.DocumentElement.AppendChild(code)
            Attribute = xml.CreateAttribute("codeSystemName")
            Attribute.Value = "eHS"
            code.SetAttributeNode(Attribute)
            xml.DocumentElement.AppendChild(code)

            'code
            AddTagWithAttributeValue(xml, Nothing, "effectiveTime", "value", Date.Now.ToString("yyyyMMddhhMMss"))

            'Header
            GenRecordTarget(xml)
            GenAuthor(xml)
            GenCustodian(xml)
            GenPracticeInfo(xml)
            'Body
            GenBodyContent(xml)

            Return xml

        End Function

        Private Sub GenAuthor(ByVal xml As XmlDocument)

            Dim author As XmlElement
            author = xml.CreateElement("author")
            AddAttributeValues2xmlElement(xml, author, "typeCode", "AUT")

            'time
            AddTagWithAttributeValue(xml, author, "time", "value", Date.Now.ToString("yyyyMMddhhmmss"))

            'assignedAuthor
            Dim assignedAuthor As XmlElement = xml.CreateElement("assignedAuthor")

            'id
            AddTagWithAttributeValue(xml, assignedAuthor, "id", "root", "2.16.840.1.113883.2.36.1.2.2", "extension", "CWS992")

            'AssignedPerson
            Dim AssignedPerson As XmlElement = xml.CreateElement("AssignedPerson")
            'Name
            Dim Name As XmlElement = xml.CreateElement("Name")
            Dim family As XmlElement = xml.CreateElement("family")
            family.InnerText = "Lee"
            Dim given As XmlElement = xml.CreateElement("given")
            given.InnerText = "Longevity"
            Name.AppendChild(family)
            Name.AppendChild(given)
            AssignedPerson.AppendChild(Name)
            'representedOrganization
            Dim representedOrganization As XmlElement = xml.CreateElement("representedOrganization")
            'id
            AddTagWithAttributeValue(xml, representedOrganization, "id", "root", "2.16.840.1.113883.2.36.1.2.2", "extension", "01")
            'name
            Dim OrgName As XmlElement = xml.CreateElement("name")
            OrgName.InnerText = "Good Health Clinic"
            representedOrganization.AppendChild(OrgName)


            assignedAuthor.AppendChild(AssignedPerson)
            assignedAuthor.AppendChild(representedOrganization)

            author.AppendChild(assignedAuthor)

            xml.DocumentElement.AppendChild(author)

        End Sub

        Private Sub GenCustodian(ByVal xml As XmlDocument)

            Dim custodain As XmlElement
            custodain = xml.CreateElement("custodain")

            'assignedAuthor
            Dim assignedCustodain As XmlElement = xml.CreateElement("assignedCustodain")

            'representedOrganization
            Dim representedOrganization As XmlElement = xml.CreateElement("representedOrganization")
            'id
            AddTagWithAttributeValue(xml, representedOrganization, "id", "root", "2.16.840.1.113883.2.36.1.2.2", "extension", "01")
            'name
            Dim OrgName As XmlElement = xml.CreateElement("name")
            OrgName.InnerText = "Good Health Clinic"
            representedOrganization.AppendChild(OrgName)

            assignedCustodain.AppendChild(representedOrganization)

            custodain.AppendChild(assignedCustodain)

            xml.DocumentElement.AppendChild(custodain)

        End Sub

        Private Sub GenRecordTarget(ByVal xml As XmlDocument)

            Dim noderecordTarget As XmlElement
            noderecordTarget = xml.CreateElement("recordTarget")

            'patienRole
            Dim patienRole As XmlElement = xml.CreateElement("patientRole")
            AddAttributeValues2xmlElement(xml, patienRole, "classCode", "PAT")

            'id
            AddTagWithAttributeValue(xml, patienRole, "id", "root", "2.16.840.1.113883.2.36.1.1.5", "extension", "00123123")

            '----------
            'Patient
            '----------
            Dim patient As XmlElement = xml.CreateElement("patient")
            AddAttributeValues2xmlElement(xml, patient, "classCode", "PSN")

            'identity no ---------------------------------------------------------------------------------
            Dim patientID As XmlElement = xml.CreateElement("id")
            Dim Attribute As XmlAttribute = xml.CreateAttribute("root")
            Attribute.Value = "2.16.840.1.113883.2.36.1.1.1"
            patientID.SetAttributeNode(Attribute)

            Select Case Me.DocType
                Case "HKIC"
                    If Me.HKIC_Included Then
                        Attribute = xml.CreateAttribute("extension")
                        Attribute.Value = Me.HKIC
                        patientID.SetAttributeNode(Attribute)
                    End If
                Case "EC"
                    If Me.HKIC_Included Then
                        Attribute = xml.CreateAttribute("extension")
                        Attribute.Value = Me.HKIC
                        patientID.SetAttributeNode(Attribute)
                    End If
                Case "HKBC"
                    If Me.RegNo_Included Then
                        Attribute = xml.CreateAttribute("extension")
                        Attribute.Value = Me.RegNo
                        patientID.SetAttributeNode(Attribute)
                    End If
                Case "ADOPC"
                    If Me.EntryNo_Included Then
                        Attribute = xml.CreateAttribute("extension")
                        Attribute.Value = Me.EntryNo
                        patientID.SetAttributeNode(Attribute)
                    End If
                Case "Doc/I"
                    If Me.DocumentNo_Included Then
                        Attribute = xml.CreateAttribute("extension")
                        Attribute.Value = Me.DocumentNo
                        patientID.SetAttributeNode(Attribute)
                    End If
                Case "REPMT"
                    If Me.PermitNo_Included Then
                        Attribute = xml.CreateAttribute("extension")
                        Attribute.Value = Me.PermitNo
                        patientID.SetAttributeNode(Attribute)
                    End If
                Case "VISA"
                    If Me.VISANo_Included Then
                        Attribute = xml.CreateAttribute("extension")
                        Attribute.Value = Me.VISANo
                        patientID.SetAttributeNode(Attribute)
                    End If
                Case "ID235B"
                    If Me.BirthEntryNo_Included Then
                        Attribute = xml.CreateAttribute("extension")
                        Attribute.Value = Me.BirthEntryNo
                        patientID.SetAttributeNode(Attribute)
                    End If
            End Select

            patient.AppendChild(patientID)
            '----------------------------------------------------------------------------------------------

            'English Name
            Dim EngName As XmlElement = xml.CreateElement("name")
            Attribute = xml.CreateAttribute("use")
            Attribute.Value = "ABC"
            EngName.SetAttributeNode(Attribute)

            If Me.Surname_Included Then
                Dim family As XmlElement
                family = xml.CreateElement("family")
                family.InnerText = Me.Surname
                EngName.AppendChild(family)
            End If

            If Me.GivenName_Included Then
                Dim Given As XmlElement
                Given = xml.CreateElement("given")
                Given.InnerText = Me.GivenName
                EngName.AppendChild(Given)
            End If

            patient.AppendChild(EngName)

            'Chinese Name
            If Me.NameChi_Included Then
                Dim ChiName As XmlElement
                ChiName = xml.CreateElement("Name")
                Attribute = xml.CreateAttribute("use")
                Attribute.Value = "IDE"
                ChiName.SetAttributeNode(Attribute)
                ChiName.InnerText = Me.NameChi
                patient.AppendChild(ChiName)
            End If

            'CCC Code
            Dim CCCCode As XmlElement
            CCCCode = xml.CreateElement("Name")
            Attribute = xml.CreateAttribute("use")
            Attribute.Value = "C"
            CCCCode.SetAttributeNode(Attribute)
            CCCCode.InnerText = "00234:03241:04411"
            patient.AppendChild(CCCCode)

            'Gender
            Dim GenderCode As XmlElement
            GenderCode = xml.CreateElement("administrativeGenderCode")
            If Me.Gender_Included Then
                Attribute = xml.CreateAttribute("code")
                Attribute.Value = Me.Gender
                GenderCode.SetAttributeNode(Attribute)
            End If
            patient.AppendChild(GenderCode)

            'Birth Date
            Dim DOB As XmlElement
            DOB = xml.CreateElement("birthTime")
            If Me.DOB_Included Then
                Attribute = xml.CreateAttribute("value")
                Attribute.Value = Me.DOB
                DOB.SetAttributeNode(Attribute)
            End If
            patient.AppendChild(DOB)

            '------
            'ProvderOrganization
            '------
            Dim providerOrganization As XmlElement = xml.CreateElement("providerOrganization")

            'SP ID
            Dim SPID As XmlElement = xml.CreateElement("id")
            Attribute = xml.CreateAttribute("root")
            Attribute.Value = "2.16.840.1.113883.2.36.1.2.2"
            SPID.SetAttributeNode(Attribute)

            If Me.SPID_Included Then
                Attribute = xml.CreateAttribute("extension")
                Attribute.Value = Me.SPID
                SPID.SetAttributeNode(Attribute)
            End If
            providerOrganization.AppendChild(SPID)

            'SP Name
            If Me.SPSurname_Included Or Me.SPGivenName_Included Then
                Dim SPName As XmlElement
                Dim strSPName As String = String.Empty

                If SPSurname_Included Then
                    strSPName = Me.SPSurname
                    If SPGivenName_Included Then
                        strSPName = strSPName.Trim + ", " + SPGivenName
                    End If
                Else
                    strSPName = Me.SPGivenName
                End If

                SPName = xml.CreateElement("Name")
                SPName.InnerText = strSPName
                providerOrganization.AppendChild(SPName)
            End If



            patienRole.AppendChild(patient)
            patienRole.AppendChild(providerOrganization)
            noderecordTarget.AppendChild(patienRole)
            xml.DocumentElement.AppendChild(noderecordTarget)

        End Sub

        Private Sub GenPracticeInfo(ByVal xml As XmlDocument)

            Dim Attribute As XmlAttribute
            Dim componentOf As XmlElement
            componentOf = xml.CreateElement("componentOf")

            'encompassingEncounter
            Dim encompassingEncounter As XmlElement = xml.CreateElement("encompassingEncounter")

            'effectiveTime
            Dim effectiveTime As XmlElement = xml.CreateElement("effectiveTime")
            If Me.ServiceDate_Included Then
                Attribute = xml.CreateAttribute("value")
                Attribute.Value = Me.ServiceDate
                effectiveTime.SetAttributeNode(Attribute)
            End If
            encompassingEncounter.AppendChild(effectiveTime)

            'location
            Dim location As XmlElement = xml.CreateElement("location")
            AddAttributeValues2xmlElement(xml, location, "typeCode", "LOC")

            'healthCareFacility
            'AddTagWithAttributeValue(xml, location, "healthCareFacility", "classCode", "SDLOC")
            Dim healthCareFacility As XmlElement = xml.CreateElement("healthCareFacility")
            AddAttributeValues2xmlElement(xml, healthCareFacility, "classCode", "SDLOC")

            'Code --> practice ID and practice name
            Dim code As XmlElement = xml.CreateElement("code")
            If Me.PracticeID_included Then
                Attribute = xml.CreateAttribute("code")
                Attribute.Value = Me.PracticeID
                code.SetAttributeNode(Attribute)
            End If
            If Me.PracticeName_included Then
                Attribute = xml.CreateAttribute("displayName")
                Attribute.Value = Me.PracticeName
                code.SetAttributeNode(Attribute)
            End If
            healthCareFacility.AppendChild(code)


            location.AppendChild(healthCareFacility)
            encompassingEncounter.AppendChild(location)
            componentOf.AppendChild(encompassingEncounter)

            xml.DocumentElement.AppendChild(componentOf)
        End Sub

        Private Sub GenBodyContent(ByVal xml As XmlDocument)

            'component
            Dim component As XmlElement
            component = xml.CreateElement("component")

            'structuredBody
            Dim structuredBody As XmlElement
            structuredBody = xml.CreateElement("structuredBody")
            component.AppendChild(structuredBody)

            'component2
            Dim component2 As XmlElement
            component2 = xml.CreateElement("component")
            structuredBody.AppendChild(component2)

            'section
            Dim section As XmlElement
            section = xml.CreateElement("section")
            component2.AppendChild(section)

            'code
            AddTagWithAttributeValue(xml, section, "code", "code", "eHS-070101", "codeSystemName", "eHS", "displayName", "Upload Claim")


            '------------------------------------------------------------------------
            ' Document (eHS Account)
            '------------------------------------------------------------------------

            'entry
            Dim entry As XmlElement
            entry = xml.CreateElement("entry")
            section.AppendChild(entry)

            'act
            Dim act As XmlElement
            act = xml.CreateElement("act")
            AddAttributeValues2xmlElement(xml, act, "classCode", "ACT", "moodCode", "EVN")
            entry.AppendChild(act)

            'code
            AddTagWithAttributeValue(xml, act, "code", "code", "eHS-070101-doc", "codeSystemName", "eHS", "displayName", "Document Type Reference")


            AddDocumentContent(xml, act, Me.DocType)

            '------------------------------------------------------------------------
            ' Voucher / Subsidy Schemes Details
            '------------------------------------------------------------------------
            If Me.WSClaimDetailList.Count > 0 Then
                For Each udtWSClaimDetail As WSClaimDetailModel In WSClaimDetailList
                    AddClaimInfo(xml, section, udtWSClaimDetail)
                Next
            End If

            xml.DocumentElement.AppendChild(component)
        End Sub


        Private Sub AddDocumentContent(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement, ByVal strDocType as String)

            'entryRelationship
            Dim entryRelationship As XmlElement
            entryRelationship = xml.CreateElement("entryRelationship")
            AddAttributeValues2xmlElement(xml, entryRelationship, "typeCode", "COMP")

            'act
            Dim act As XmlElement
            act = xml.CreateElement("act")
            AddAttributeValues2xmlElement(xml, act, "classCode", "ACT", "moodCode", "EVN")
            entryRelationship.AppendChild(act)

            'code
            Dim code As XmlElement
            code = xml.CreateElement("code")
            AddAttributeValues2xmlElement(xml, code, "code", strDocType.ToUpper(), "displayName", "DocType", "codeSystemName", "eHS")
            act.AppendChild(code)

            'For HKIC
            Select Case strDocType
                Case "HKIC"
                    If Me.HKIC_Included Then
                        AddQualifierPair(xml, code, "HKIC", Me.HKIC)
                    End If
                    If Me.DOI_Included Then
                        AddQualifierPair(xml, code, "DOI", Me.DOI)
                    End If
                Case "ADOPC"
                    If Me.EntryNo_Included Then
                        AddQualifierPair(xml, code, "EntryNo", Me.EntryNo)
                    End If
                    If Me.DOBInWord_Included Then
                        AddQualifierPair(xml, code, "DOBInWord", Me.DOBInWord)
                    End If
                Case "EC"
                    If Me.DOBType_Included Then
                        AddQualifierPair(xml, code, "DOBType", Me.DOBType)
                    End If
                    If Me.AgeOn_Included Then
                        AddQualifierPair(xml, code, "AgeOn", Me.AgeOn)
                    End If
                    If Me.DOReg_Included Then
                        AddQualifierPair(xml, code, "DOReg", Me.DOReg)
                    End If
                    If Me.DOI_Included Then
                        AddQualifierPair(xml, code, "DOI", Me.DOI)
                    End If
                    If Me.HKIC_Included Then
                        AddQualifierPair(xml, code, "HKIC", Me.HKIC)
                    End If
                    If Me.SerialNo_Included Then
                        AddQualifierPair(xml, code, "SerialNo", Me.SerialNo)
                    End If
                    If Me.Reference_Included Then
                        AddQualifierPair(xml, code, "Reference", Me.Reference)
                    End If
                    If Me.FreeRef_Included Then
                        AddQualifierPair(xml, code, "FreeRef", Me.FreeReference)
                    End If
                Case "HKBC"
                    If Me.RegNo_Included Then
                        AddQualifierPair(xml, code, "RegNo", Me.RegNo)
                    End If
                    If Me.DOBInWord_Included Then
                        AddQualifierPair(xml, code, "DOBInWord", Me.DOBInWord)
                    End If
                Case "Doc/I"
                    If Me.DocumentNo_Included Then
                        AddQualifierPair(xml, code, "DocumentNo", Me.DocumentNo)
                    End If
                    If Me.DOI_Included Then
                        AddQualifierPair(xml, code, "DOI", Me.DOI)
                    End If
                Case "REPMT"
                    If Me.PermitNo_Included Then
                        AddQualifierPair(xml, code, "PermitNo", Me.PermitNo)
                    End If
                    If Me.DOI_Included Then
                        AddQualifierPair(xml, code, "DOI", Me.DOI)
                    End If
                Case "VISA"
                    If Me.VISANo_Included Then
                        AddQualifierPair(xml, code, "VISANo", Me.VISANo)
                    End If
                    If Me.PassportNo_Included Then
                        AddQualifierPair(xml, code, "PassportNo", Me.PassportNo)
                    End If
                Case "ID235B"
                    If Me.BirthEntryNo_Included Then
                        AddQualifierPair(xml, code, "BirthEntryNo", Me.BirthEntryNo)
                    End If
                    If Me.RemainUntil_Included Then
                        AddQualifierPair(xml, code, "RemainUntil", Me.RemainUntil)
                    End If
            End Select

            nodeResult.AppendChild(entryRelationship)
        End Sub

        Private Sub AddClaimInfo(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement, ByVal udtWSClaimModel As WSClaimDetailModel)


            '------------------------------------------------------------------------
            ' Voucher
            '------------------------------------------------------------------------
            If udtWSClaimModel.HCVS_Included AndAlso Not IsNothing(udtWSClaimModel) Then
                'entry
                Dim entry As XmlElement
                entry = xml.CreateElement("entry")
                nodeResult.AppendChild(entry)

                AddVoucherInfo(xml, entry, udtWSClaimModel.WSVoucher, _
                            udtWSClaimModel.TSWInd_Included, udtWSClaimModel.TSWInd, _
                            udtWSClaimModel.PreSchoolInd, udtWSClaimModel.PreSchoolInd, _
                            udtWSClaimModel.DoseIntervalInd_Included, udtWSClaimModel.DoseIntervalInd)
            End If

            '------------------------------------------------------------------------
            ' Vaccination
            '------------------------------------------------------------------------
            If udtWSClaimModel.Vaccine_Included Then
                AddVaccineInfo(xml, nodeResult, udtWSClaimModel.WSVaccineDetailList, _
                                udtWSClaimModel.SchemeCode_Included, udtWSClaimModel.SchemeCode, _
                                udtWSClaimModel.PreSchoolInd, udtWSClaimModel.PreSchoolInd, _
                                udtWSClaimModel.DoseIntervalInd_Included, udtWSClaimModel.DoseIntervalInd, _
                                udtWSClaimModel.TSWInd_Included, udtWSClaimModel.TSWInd, _
                                udtWSClaimModel.RCHCode_Included, udtWSClaimModel.RCHCode)
            End If

        End Sub

        Private Sub AddVoucherInfo(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement, ByVal udtWSVoucherModel As WSVoucherModel, _
                                    Optional ByVal TSWInd_Included As Boolean = False, Optional ByVal TSWIndValue As String = "", _
                                    Optional ByVal PreSchoolInd_Included As String = "N", Optional ByVal PreSchoolIndValue As String = "", _
                                    Optional ByVal IntervalInd_Included As Boolean = False, Optional ByVal IntervalIndValue As String = "")

            Dim Attribute As XmlAttribute

            'observation
            Dim observation As XmlElement
            observation = xml.CreateElement("observation")
            AddAttributeValues2xmlElement(xml, observation, "classCode", "OBS", "moodCode", "EVN")
            nodeResult.AppendChild(observation)

            'code
            AddTagWithAttributeValue(xml, observation, "code", "code", "eHS-070101-sch", "displayName", "Voucher and Subsidy Scheme Information", "codeSystemName", "eHS")

            'statusCode
            AddTagWithAttributeValue(xml, observation, "statusCode", "code", "completed")

            'effectiveTime
            If Me.ServiceDate_Included Then
                AddTagWithAttributeValue(xml, observation, "effectiveTime", "value", Me.ServiceDate)
            End If

            'Scheme Type
            AddEntryRelationshipPair(xml, observation, "COMP", "ACT", "EVN", "HCVS", "SchemeCode", "eHS")

            'Subsidy Type
            AddEntryRelationshipPair(xml, observation, "COMP", "ACT", "EVN", "EHCV", "SubsidyCode", "eHS")

            'No of Voucher Claimed
            If udtWSVoucherModel.VoucherClaimed_Included Then
                AddEntryRelationshipPair(xml, observation, "COMP", "ACT", "EVN", udtWSVoucherModel.VoucherClaimed, "VoucherClaimed", "eHS")
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

            ' -----------------------------------------------------------------------------------------

            'Co-Payment Fee
            If udtWSVoucherModel.CoPaymentFee_Included Then
                'entryRelationship
                Dim entryRelationshipCoPaymentFee As XmlElement
                entryRelationshipCoPaymentFee = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationshipCoPaymentFee, "typeCode", "COMP")

                'observation 
                Dim observationCoPaymentFee As XmlElement
                observationCoPaymentFee = xml.CreateElement("observation")
                AddAttributeValues2xmlElement(xml, observationCoPaymentFee, "classCode", "OBS", "moodCode", "EVN")
                entryRelationshipCoPaymentFee.AppendChild(observationCoPaymentFee)

                'code
                Dim codeCoPaymentFee As XmlElement
                codeCoPaymentFee = xml.CreateElement("code")
                AddAttributeValues2xmlElement(xml, codeCoPaymentFee, "code", "CopaymentFee")
                observationCoPaymentFee.AppendChild(codeCoPaymentFee)

                'value
                Dim textCoPaymentFee As XmlElement
                textCoPaymentFee = xml.CreateElement("value")
                Attribute = xml.CreateAttribute("xsi:type", "http://www.3w.org/2001/XMLSchema-instance")
                Attribute.Value = "AD"
                textCoPaymentFee.SetAttributeNode(Attribute)
                textCoPaymentFee.InnerText = udtWSVoucherModel.CoPaymentFee
                observationCoPaymentFee.AppendChild(textCoPaymentFee)

                observation.AppendChild(entryRelationshipCoPaymentFee)
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            'Reason For Visit
            If udtWSVoucherModel.ReasonForVisit_Included Then
                'entryRelationship
                Dim entryRelationship As XmlElement
                entryRelationship = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationship, "typeCode", "RSON")

                'observation 
                Dim observationR As XmlElement
                observationR = xml.CreateElement("observation")
                AddAttributeValues2xmlElement(xml, observationR, "classCode", "OBS", "moodCode", "EVN")
                entryRelationship.AppendChild(observationR)

                'professionalCode
                If udtWSVoucherModel.ProfCode_Included Then
                    AddTagWithAttributeValue(xml, observationR, "code", "code", udtWSVoucherModel.ProfCode, "displayName", "ProfessionalCode", "codeSystemName", "eHS")
                End If

                '------------------------------------------------------------------------------
                'entryRelationship
                Dim entryRelationshipL1 As XmlElement
                entryRelationshipL1 = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationshipL1, "typeCode", "RSON")
                observationR.AppendChild(entryRelationshipL1)

                'act 
                Dim actL1 As XmlElement
                actL1 = xml.CreateElement("act")
                AddAttributeValues2xmlElement(xml, actL1, "classCode", "ACT", "moodCode", "EVN")
                entryRelationshipL1.AppendChild(actL1)

                'code (L1 Code)
                If udtWSVoucherModel.L1Code_Included Then
                    AddTagWithAttributeValue(xml, actL1, "code", "code", udtWSVoucherModel.L1Code, "displayName", "L1Code", "codeSystemName", "eHS")
                End If

                'text (L1 Eng Desc)
                If udtWSVoucherModel.L1DescEng_Included Then
                    Dim text As XmlElement
                    text = xml.CreateElement("text")
                    Attribute = xml.CreateAttribute("xsi:type", "http://www.3w.org/2001/XMLSchema-instance")
                    Attribute.Value = "ST"
                    text.SetAttributeNode(Attribute)
                    text.InnerText = udtWSVoucherModel.L1DescEng
                    actL1.AppendChild(text)
                End If

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

                ' -----------------------------------------------------------------------------------------

                'Priority Code
                If udtWSVoucherModel.PriorityCode_Included Then
                    AddTagWithAttributeValue(xml, actL1, "priorityCode", "code", udtWSVoucherModel.PriorityCode)
                End If

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                '------------------------------------------------------------------------------------------------------------------------------------------------------------
                'entryRelationship
                Dim entryRelationshipL2 As XmlElement
                entryRelationshipL2 = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationshipL2, "typeCode", "COMP")
                actL1.AppendChild(entryRelationshipL2)

                'act 
                Dim actL2 As XmlElement
                actL2 = xml.CreateElement("act")
                AddAttributeValues2xmlElement(xml, actL2, "classCode", "ACT", "moodCode", "EVN")
                entryRelationshipL2.AppendChild(actL2)

                'code (L2 Code)
                If udtWSVoucherModel.L2Code_Included Then
                    AddTagWithAttributeValue(xml, actL2, "code", "code", udtWSVoucherModel.L2Code, "displayName", "L2Code", "codeSystemName", "eHS")
                End If

                'text (L2 Eng Desc)
                If udtWSVoucherModel.L2DescEng_Included Then
                    Dim text As XmlElement
                    text = xml.CreateElement("text")
                    Attribute = xml.CreateAttribute("xsi:type", "http://www.3w.org/2001/XMLSchema-instance")
                    Attribute.Value = "ST"
                    text.SetAttributeNode(Attribute)
                    text.InnerText = udtWSVoucherModel.L2DescEng
                    actL2.AppendChild(text)
                End If
                '------------------------------------------------------------------------------------------------------------------------------------------------------------
                '------------------------------------------------------------------------------
                observation.AppendChild(entryRelationship)
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]

            ' -----------------------------------------------------------------------------------------

            'Reason For Visit
            If udtWSVoucherModel.ReasonForVisit_S1_Included Then
                'entryRelationship
                Dim entryRelationship_S1 As XmlElement
                entryRelationship_S1 = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationship_S1, "typeCode", "RSON")

                'observation 
                Dim observationR_S1 As XmlElement
                observationR_S1 = xml.CreateElement("observation")
                AddAttributeValues2xmlElement(xml, observationR_S1, "classCode", "OBS", "moodCode", "EVN")
                entryRelationship_S1.AppendChild(observationR_S1)

                'professionalCode
                If udtWSVoucherModel.ProfCode_S1_Included Then
                    AddTagWithAttributeValue(xml, observationR_S1, "code", "code", udtWSVoucherModel.ProfCode_S1, "displayName", "ProfessionalCode", "codeSystemName", "eHS")
                End If

                '------------------------------------------------------------------------------
                'entryRelationship
                Dim entryRelationshipL1_S1 As XmlElement
                entryRelationshipL1_S1 = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationshipL1_S1, "typeCode", "RSON")
                observationR_S1.AppendChild(entryRelationshipL1_S1)

                'act 
                Dim actL1_S1 As XmlElement
                actL1_S1 = xml.CreateElement("act")
                AddAttributeValues2xmlElement(xml, actL1_S1, "classCode", "ACT", "moodCode", "EVN")
                entryRelationshipL1_S1.AppendChild(actL1_S1)

                'code (L1 Code)
                If udtWSVoucherModel.L1Code_S1_Included Then
                    AddTagWithAttributeValue(xml, actL1_S1, "code", "code", udtWSVoucherModel.L1Code_S1, "displayName", "L1Code", "codeSystemName", "eHS")
                End If

                'text (L1 Eng Desc)
                If udtWSVoucherModel.L1DescEng_S1_Included Then
                    Dim text_S1 As XmlElement
                    text_S1 = xml.CreateElement("text")
                    Attribute = xml.CreateAttribute("xsi:type", "http://www.3w.org/2001/XMLSchema-instance")
                    Attribute.Value = "ST"
                    text_S1.SetAttributeNode(Attribute)
                    text_S1.InnerText = udtWSVoucherModel.L1DescEng_S1
                    actL1_S1.AppendChild(text_S1)
                End If

                'Priority Code
                If udtWSVoucherModel.PriorityCode_S1_Included Then
                    AddTagWithAttributeValue(xml, actL1_S1, "priorityCode", "code", udtWSVoucherModel.PriorityCode_S1)
                End If

                '------------------------------------------------------------------------------------------------------------------------------------------------------------
                'entryRelationship
                Dim entryRelationshipL2_S1 As XmlElement
                entryRelationshipL2_S1 = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationshipL2_S1, "typeCode", "COMP")
                actL1_S1.AppendChild(entryRelationshipL2_S1)

                'act 
                Dim actL2_S1 As XmlElement
                actL2_S1 = xml.CreateElement("act")
                AddAttributeValues2xmlElement(xml, actL2_S1, "classCode", "ACT", "moodCode", "EVN")
                entryRelationshipL2_S1.AppendChild(actL2_S1)

                'code (L2 Code)
                If udtWSVoucherModel.L2Code_S1_Included Then
                    AddTagWithAttributeValue(xml, actL2_S1, "code", "code", udtWSVoucherModel.L2Code_S1, "displayName", "L2Code", "codeSystemName", "eHS")
                End If

                'text (L2 Eng Desc)
                If udtWSVoucherModel.L2DescEng_S1_Included Then
                    Dim text_S1 As XmlElement
                    text_S1 = xml.CreateElement("text")
                    Attribute = xml.CreateAttribute("xsi:type", "http://www.3w.org/2001/XMLSchema-instance")
                    Attribute.Value = "ST"
                    text_S1.SetAttributeNode(Attribute)
                    text_S1.InnerText = udtWSVoucherModel.L2DescEng_S1
                    actL2_S1.AppendChild(text_S1)
                End If
                '------------------------------------------------------------------------------------------------------------------------------------------------------------
                '------------------------------------------------------------------------------
                observation.AppendChild(entryRelationship_S1)
            End If

            'Reason For Visit
            If udtWSVoucherModel.ReasonForVisit_S2_Included Then
                'entryRelationship
                Dim entryRelationship_S2 As XmlElement
                entryRelationship_S2 = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationship_S2, "typeCode", "RSON")

                'observation 
                Dim observationR_S2 As XmlElement
                observationR_S2 = xml.CreateElement("observation")
                AddAttributeValues2xmlElement(xml, observationR_S2, "classCode", "OBS", "moodCode", "EVN")
                entryRelationship_S2.AppendChild(observationR_S2)

                'professionalCode
                If udtWSVoucherModel.ProfCode_S2_Included Then
                    AddTagWithAttributeValue(xml, observationR_S2, "code", "code", udtWSVoucherModel.ProfCode_S2, "displayName", "ProfessionalCode", "codeSystemName", "eHS")
                End If

                '------------------------------------------------------------------------------
                'entryRelationship
                Dim entryRelationshipL1_S2 As XmlElement
                entryRelationshipL1_S2 = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationshipL1_S2, "typeCode", "RSON")
                observationR_S2.AppendChild(entryRelationshipL1_S2)

                'act 
                Dim actL1_S2 As XmlElement
                actL1_S2 = xml.CreateElement("act")
                AddAttributeValues2xmlElement(xml, actL1_S2, "classCode", "ACT", "moodCode", "EVN")
                entryRelationshipL1_S2.AppendChild(actL1_S2)

                'code (L1 Code)
                If udtWSVoucherModel.L1Code_S2_Included Then
                    AddTagWithAttributeValue(xml, actL1_S2, "code", "code", udtWSVoucherModel.L1Code_S2, "displayName", "L1Code", "codeSystemName", "eHS")
                End If

                'text (L1 Eng Desc)
                If udtWSVoucherModel.L1DescEng_S2_Included Then
                    Dim text_S2 As XmlElement
                    text_S2 = xml.CreateElement("text")
                    Attribute = xml.CreateAttribute("xsi:type", "http://www.3w.org/2001/XMLSchema-instance")
                    Attribute.Value = "ST"
                    text_S2.SetAttributeNode(Attribute)
                    text_S2.InnerText = udtWSVoucherModel.L1DescEng_S2
                    actL1_S2.AppendChild(text_S2)
                End If

                'Priority Code
                If udtWSVoucherModel.PriorityCode_S2_Included Then
                    AddTagWithAttributeValue(xml, actL1_S2, "priorityCode", "code", udtWSVoucherModel.PriorityCode_S2)
                End If

                '------------------------------------------------------------------------------------------------------------------------------------------------------------
                'entryRelationship
                Dim entryRelationshipL2_S2 As XmlElement
                entryRelationshipL2_S2 = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationshipL2_S2, "typeCode", "COMP")
                actL1_S2.AppendChild(entryRelationshipL2_S2)

                'act 
                Dim actL2_S2 As XmlElement
                actL2_S2 = xml.CreateElement("act")
                AddAttributeValues2xmlElement(xml, actL2_S2, "classCode", "ACT", "moodCode", "EVN")
                entryRelationshipL2_S2.AppendChild(actL2_S2)

                'code (L2 Code)
                If udtWSVoucherModel.L2Code_S2_Included Then
                    AddTagWithAttributeValue(xml, actL2_S2, "code", "code", udtWSVoucherModel.L2Code_S2, "displayName", "L2Code", "codeSystemName", "eHS")
                End If

                'text (L2 Eng Desc)
                If udtWSVoucherModel.L2DescEng_S2_Included Then
                    Dim text_S2 As XmlElement
                    text_S2 = xml.CreateElement("text")
                    Attribute = xml.CreateAttribute("xsi:type", "http://www.3w.org/2001/XMLSchema-instance")
                    Attribute.Value = "ST"
                    text_S2.SetAttributeNode(Attribute)
                    text_S2.InnerText = udtWSVoucherModel.L2DescEng_S2
                    actL2_S2.AppendChild(text_S2)
                End If
                '------------------------------------------------------------------------------------------------------------------------------------------------------------
                '------------------------------------------------------------------------------
                observation.AppendChild(entryRelationship_S2)
            End If


            'Reason For Visit
            If udtWSVoucherModel.ReasonForVisit_S3_Included Then
                'entryRelationship
                Dim entryRelationship_S3 As XmlElement
                entryRelationship_S3 = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationship_S3, "typeCode", "RSON")

                'observation 
                Dim observationR_S3 As XmlElement
                observationR_S3 = xml.CreateElement("observation")
                AddAttributeValues2xmlElement(xml, observationR_S3, "classCode", "OBS", "moodCode", "EVN")
                entryRelationship_S3.AppendChild(observationR_S3)

                'professionalCode
                If udtWSVoucherModel.ProfCode_S3_Included Then
                    AddTagWithAttributeValue(xml, observationR_S3, "code", "code", udtWSVoucherModel.ProfCode_S3, "displayName", "ProfessionalCode", "codeSystemName", "eHS")
                End If

                '------------------------------------------------------------------------------
                'entryRelationship
                Dim entryRelationshipL1_S3 As XmlElement
                entryRelationshipL1_S3 = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationshipL1_S3, "typeCode", "RSON")
                observationR_S3.AppendChild(entryRelationshipL1_S3)

                'act 
                Dim actL1_S3 As XmlElement
                actL1_S3 = xml.CreateElement("act")
                AddAttributeValues2xmlElement(xml, actL1_S3, "classCode", "ACT", "moodCode", "EVN")
                entryRelationshipL1_S3.AppendChild(actL1_S3)

                'code (L1 Code)
                If udtWSVoucherModel.L1Code_S3_Included Then
                    AddTagWithAttributeValue(xml, actL1_S3, "code", "code", udtWSVoucherModel.L1Code_S3, "displayName", "L1Code", "codeSystemName", "eHS")
                End If

                'text (L1 Eng Desc)
                If udtWSVoucherModel.L1DescEng_S3_Included Then
                    Dim text_S3 As XmlElement
                    text_S3 = xml.CreateElement("text")
                    Attribute = xml.CreateAttribute("xsi:type", "http://www.3w.org/2001/XMLSchema-instance")
                    Attribute.Value = "ST"
                    text_S3.SetAttributeNode(Attribute)
                    text_S3.InnerText = udtWSVoucherModel.L1DescEng_S3
                    actL1_S3.AppendChild(text_S3)
                End If

                'Priority Code
                If udtWSVoucherModel.PriorityCode_S3_Included Then
                    AddTagWithAttributeValue(xml, actL1_S3, "priorityCode", "code", udtWSVoucherModel.PriorityCode_S3)
                End If

                '------------------------------------------------------------------------------------------------------------------------------------------------------------
                'entryRelationship
                Dim entryRelationshipL2_S3 As XmlElement
                entryRelationshipL2_S3 = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationshipL2_S3, "typeCode", "COMP")
                actL1_S3.AppendChild(entryRelationshipL2_S3)

                'act 
                Dim actL2_S3 As XmlElement
                actL2_S3 = xml.CreateElement("act")
                AddAttributeValues2xmlElement(xml, actL2_S3, "classCode", "ACT", "moodCode", "EVN")
                entryRelationshipL2_S3.AppendChild(actL2_S3)

                'code (L2 Code)
                If udtWSVoucherModel.L2Code_S3_Included Then
                    AddTagWithAttributeValue(xml, actL2_S3, "code", "code", udtWSVoucherModel.L2Code_S3, "displayName", "L2Code", "codeSystemName", "eHS")
                End If

                'text (L2 Eng Desc)
                If udtWSVoucherModel.L2DescEng_S3_Included Then
                    Dim text_S3 As XmlElement
                    text_S3 = xml.CreateElement("text")
                    Attribute = xml.CreateAttribute("xsi:type", "http://www.3w.org/2001/XMLSchema-instance")
                    Attribute.Value = "ST"
                    text_S3.SetAttributeNode(Attribute)
                    text_S3.InnerText = udtWSVoucherModel.L2DescEng_S3
                    actL2_S3.AppendChild(text_S3)
                End If
                '------------------------------------------------------------------------------------------------------------------------------------------------------------
                '------------------------------------------------------------------------------
                observation.AppendChild(entryRelationship_S3)
            End If

            ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

            'TSW Tag
            If TSWInd_Included Then
                AddEntryRelationshipPair2(xml, observation, "COMP", "OBS", "EVN", TSWIndValue, "TSWInd", "eHS")
            End If

            '------------------------------------------------------------------------------
            ' Suppose not for eVoucher
            '------------------------------------------------------------------------------
            'PreSchool Tag
            If PreSchoolInd_Included = "Y" Then
                AddEntryRelationshipPair2(xml, observation, "COMP", "OBS", "EVN", PreSchoolIndValue, "PreSchoolInd", "eHS")
            End If

            'Interval Tag
            If IntervalInd_Included Then
                'entryRelationship
                Dim entryRelationship As XmlElement
                entryRelationship = xml.CreateElement("entryRelationship")
                AddAttributeValues2xmlElement(xml, entryRelationship, "typeCode", "COMP")

                'observation 
                Dim observation2 As XmlElement
                observation2 = xml.CreateElement("observation")
                AddAttributeValues2xmlElement(xml, observation2, "classCode", "OBS", "moodCode", "EVN")
                entryRelationship.AppendChild(observation2)

                'code
                AddTagWithAttributeValue(xml, observation2, "code", "code", IntervalIndValue, "displayName", "DoseIntervalInd", "codeSystemName", "eHS")

                'related subsidy code
                AddEntryRelationshipPair2(xml, observation2, "COMP", "OBS", "EVN", "EHCV", "SubsidyCode", "eHS")

                observation.AppendChild(entryRelationship)
            End If
            '------------------------------------------------------------------------------

            nodeResult.AppendChild(observation)
        End Sub

        Private Sub AddVaccineInfo(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement, ByVal udtWSVaccineList As WSVaccineDetailModelCollection, _
                                 ByVal SchemeCode_Included As Boolean, ByVal SchemeCode As String, _
                                 Optional ByVal PreSchoolInd_Included As String = "N", Optional ByVal PreSchoolIndValue As String = "", _
                                 Optional ByVal IntervalInd_Included As Boolean = False, Optional ByVal IntervalIndValue As String = "", _
                                 Optional ByVal TSWInd_Included As Boolean = False, Optional ByVal TSWIndValue As String = "", _
                                 Optional ByVal RCHCode_Included As Boolean = False, Optional ByVal RCHCodevalue As String = "")

            Dim Attribute As XmlAttribute

            For Each udtVaccine As WSVaccineDetailModel In udtWSVaccineList

                'entry
                Dim entry As XmlElement
                entry = xml.CreateElement("entry")
                nodeResult.AppendChild(entry)

                'substanceAdministration
                Dim substanceAdministration As XmlElement
                substanceAdministration = xml.CreateElement("substanceAdministration")
                AddAttributeValues2xmlElement(xml, substanceAdministration, "classCode", "SBADM", "moodCode", "EVN")
                entry.AppendChild(substanceAdministration)

                'code
                AddTagWithAttributeValue(xml, substanceAdministration, "code", "code", "eHS-070101-sch", "displayName", "Voucher and Subsidy Scheme Information")

                'statusCode
                AddTagWithAttributeValue(xml, substanceAdministration, "statusCode", "code", "completed")

                'effectiveDate
                If Me.ServiceDate_Included Then
                    AddTagWithAttributeValue(xml, substanceAdministration, "effectiveTime", "value", Me.ServiceDate)
                End If

                'Subsidy Code
                If udtVaccine.SubsidyCode_included Then
                    Select Case udtVaccine.SubsidyCode
                        Case "CSIV"
                            'Consumable > manufacturedProduct > manufacturedMaterial > code
                            AddConsumablePair(xml, substanceAdministration, "CSIV", "Childhood Seasonal Influenza Vaccination")
                        Case "23vPP"
                            'Consumable > manufacturedProduct > manufacturedMaterial > code
                            AddConsumablePair(xml, substanceAdministration, "23vPP", "23-valent Pneumococcal Polysaccharide vaccination")
                        Case "ESIV"
                            'Consumable > manufacturedProduct > manufacturedMaterial > code
                            AddConsumablePair(xml, substanceAdministration, "ESIV", "Elderly Seasonal nfluenza Vaccination")
                        Case "RSIV"
                            'Consumable > manufacturedProduct > manufacturedMaterial > code
                            AddConsumablePair(xml, substanceAdministration, "RSIV", "Residential Care Home Seasonal Influenza Vaccination")
                        Case "RSIV-HCW"
                            'Consumable > manufacturedProduct > manufacturedMaterial > code
                            AddConsumablePair(xml, substanceAdministration, "RSIV-HCW", "Residential Care Home Seasonal Influenza Vaccination for Health Care Worker")
                        Case Else
                            AddConsumablePair(xml, substanceAdministration, udtVaccine.SubsidyCode, "Not Specified")
                    End Select
                End If

                'Vaccine dose sequence
                If udtVaccine.DoseSeq_included Then
                    'entryRelationship
                    Dim entryRelationship As XmlElement
                    entryRelationship = xml.CreateElement("entryRelationship")
                    AddAttributeValues2xmlElement(xml, entryRelationship, "typeCode", "SUBJ")

                    'observation 
                    Dim observation As XmlElement
                    observation = xml.CreateElement("observation")
                    AddAttributeValues2xmlElement(xml, observation, "classCode", "OBS", "moodCode", "EVN")
                    entryRelationship.AppendChild(observation)

                    'code
                    AddTagWithAttributeValue(xml, observation, "code", "code", "1001812", "displayName", "Vaccine dose sequence")

                    'status code
                    AddTagWithAttributeValue(xml, observation, "statusCode", "code", "completed")

                    'dose info
                    Dim value As XmlElement
                    value = xml.CreateElement("value")
                    Attribute = xml.CreateAttribute("xsi:type", "http://www.3w.org/2001/XMLSchema-instance")
                    Attribute.Value = "ST"
                    value.SetAttributeNode(Attribute)
                    value.InnerText = udtVaccine.DoseSeq
                    observation.AppendChild(value)

                    substanceAdministration.AppendChild(entryRelationship)
                End If

                'Scheme Type Claimed
                If SchemeCode_Included Then
                    Select Case SchemeCode
                        Case "CIVSS"
                            AddEntryRelationshipPair(xml, substanceAdministration, "COMP", "ACT", "EVN", "CIVSS", Nothing, "eHS")
                        Case "EVSS"
                            AddEntryRelationshipPair(xml, substanceAdministration, "COMP", "ACT", "EVN", "EVSS", Nothing, "eHS")
                        Case "RVP"
                            AddEntryRelationshipPair(xml, substanceAdministration, "COMP", "ACT", "EVN", "RVP", Nothing, "eHS")
                        Case Else
                            AddEntryRelationshipPair(xml, substanceAdministration, "COMP", "ACT", "EVN", SchemeCode, Nothing, "eHS")
                    End Select
                End If

                'RCH Code
                If RCHCode_Included Then
                    AddEntryRelationshipPair2(xml, substanceAdministration, "COMP", "OBS", "EVN", RCHCodevalue, "RCHCode", "eHS")
                End If

                '-------------------
                ' Indicator
                '-------------------
                'PreSchool Tag
                If PreSchoolInd_Included = "Y" Then
                    AddEntryRelationshipPair2(xml, substanceAdministration, "COMP", "OBS", "EVN", PreSchoolIndValue, "PreSchoolInd", "eHS")
                End If

                'TSW Tag (Suppose not for vaccine)
                If TSWInd_Included Then
                    AddEntryRelationshipPair2(xml, substanceAdministration, "COMP", "OBS", "EVN", TSWIndValue, "TSWInd", "eHS")
                End If

                'Interval Tag
                If IntervalInd_Included Then
                    'entryRelationship
                    Dim entryRelationship As XmlElement
                    entryRelationship = xml.CreateElement("entryRelationship")
                    AddAttributeValues2xmlElement(xml, entryRelationship, "typeCode", "COMP")

                    'observation 
                    Dim observation As XmlElement
                    observation = xml.CreateElement("observation")
                    AddAttributeValues2xmlElement(xml, observation, "classCode", "OBS", "moodCode", "EVN")
                    entryRelationship.AppendChild(observation)

                    'code
                    AddTagWithAttributeValue(xml, observation, "code", "code", IntervalIndValue, "displayName", "DoseIntervalInd", "codeSystemName", "eHS")

                    'related subsidy code
                    AddEntryRelationshipPair2(xml, observation, "COMP", "OBS", "EVN", udtVaccine.SubsidyCode, "SubsidyCode", "eHS")

                    substanceAdministration.AppendChild(entryRelationship)
                End If
            Next

        End Sub

#End Region


        '<qualifier>
        '   <name displayName="strName">
        '   <value code="strValue">
        '</qualifier>
        Private Sub AddQualifierPair(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement, _
                                    ByVal strName As String, ByVal strValue As String)
            'qualifier
            Dim qualifier As XmlElement
            qualifier = xml.CreateElement("qualifier")

            'Name
            AddTagWithAttributeValue(xml, qualifier, "name", "displayName", strName)

            'Value
            AddTagWithAttributeValue(xml, qualifier, "value", "code", strValue)

            nodeResult.AppendChild(qualifier)
        End Sub


        '<entryRelationship typeCode='strTypeCode'>
        '   <act classCode='strClassCode' moodCode='strMoodCode'>
        '       <code code="strCode" displayname='strDisplayname' codeSystemName='strCodeSystemName'>
        '   </act>
        '</entryRelationship>
        Private Sub AddEntryRelationshipPair(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement, _
                                    ByVal strTypeCode As String, ByVal strClassCode As String, ByVal strMoodCode As String, _
                                    ByVal strCode As String, ByVal strDisplayname As String, ByVal strCodeSystemName As String)
            'entryRelationship
            Dim entryRelationship As XmlElement
            entryRelationship = xml.CreateElement("entryRelationship")
            AddAttributeValues2xmlElement(xml, entryRelationship, "typeCode", strTypeCode)

            'act 
            Dim act As XmlElement
            act = xml.CreateElement("act")
            AddAttributeValues2xmlElement(xml, act, "classCode", strClassCode, "moodCode", strMoodCode)
            entryRelationship.AppendChild(act)

            'code
            If IsNothing(strDisplayname) Then
                AddTagWithAttributeValue(xml, act, "code", "code", strCode, "codeSystemName", strCodeSystemName)
            Else
                AddTagWithAttributeValue(xml, act, "code", "code", strCode, "displayName", strDisplayname, "codeSystemName", strCodeSystemName)
            End If



            nodeResult.AppendChild(entryRelationship)
        End Sub

        '<entryRelationship typeCode='strTypeCode'>
        '   <observation classCode='strClassCode' moodCode='strMoodCode'>
        '       <code code="strCode" displayname='strDisplayname' codeSystemName='strCodeSystemName'>
        '   </act>
        '</entryRelationship>
        Private Sub AddEntryRelationshipPair2(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement, _
                                    ByVal strTypeCode As String, ByVal strClassCode As String, ByVal strMoodCode As String, _
                                    ByVal strCode As String, ByVal strDisplayname As String, ByVal strCodeSystemName As String)
            'entryRelationship
            Dim entryRelationship As XmlElement
            entryRelationship = xml.CreateElement("entryRelationship")
            AddAttributeValues2xmlElement(xml, entryRelationship, "typeCode", strTypeCode)

            'observation 
            Dim observation As XmlElement
            observation = xml.CreateElement("observation")
            AddAttributeValues2xmlElement(xml, observation, "classCode", strClassCode, "moodCode", strMoodCode)
            entryRelationship.AppendChild(observation)

            'code
            AddTagWithAttributeValue(xml, observation, "code", "code", strCode, "displayName", strDisplayname, "codeSystemName", strCodeSystemName)

            nodeResult.AppendChild(entryRelationship)
        End Sub

        '<consumable>
        '   <manufacturedProduct>
        '       <manufacturedMaterial>
        '           <code code="strCode" displayname='strDisplayname'>
        '       </manufacturedMaterial>
        '   </manufacturedProduct>
        '</consumable>
        Private Sub AddConsumablePair(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement, _
                                    ByVal strCode As String, ByVal strDisplayname As String)
            'consumable
            Dim consumable As XmlElement
            consumable = xml.CreateElement("consumable")

            'manufacturedProduct
            Dim manufacturedProduct As XmlElement
            manufacturedProduct = xml.CreateElement("manufacturedProduct")
            consumable.AppendChild(manufacturedProduct)

            'manufacturedProduct
            Dim manufacturedMaterial As XmlElement
            manufacturedMaterial = xml.CreateElement("manufacturedMaterial")
            manufacturedProduct.AppendChild(manufacturedMaterial)

            'code
            AddTagWithAttributeValue(xml, manufacturedMaterial, "code", "code", strCode, "displayName", strDisplayname)

            nodeResult.AppendChild(consumable)
        End Sub


        Private Sub AddTagWithAttributeValue(ByRef xml As XmlDocument, ByRef AppendTo As XmlElement, ByVal tagName As String, _
                                             ByVal AttributeName1 As String, ByVal AttributeValue1 As String, _
                                             Optional ByVal AttributeName2 As String = "", Optional ByVal AttributeValue2 As String = "", _
                                              Optional ByVal AttributeName3 As String = "", Optional ByVal AttributeValue3 As String = "")
            Dim code As XmlElement = xml.CreateElement(tagName)
            Dim Attribute As XmlAttribute = xml.CreateAttribute(AttributeName1)
            Attribute.Value = AttributeValue1
            code.SetAttributeNode(Attribute)

            If Not AttributeName2.Trim = "" Then
                Attribute = xml.CreateAttribute(AttributeName2)
                Attribute.Value = AttributeValue2
                code.SetAttributeNode(Attribute)
            End If

            If Not AttributeName3.Trim = "" Then
                Attribute = xml.CreateAttribute(AttributeName3)
                Attribute.Value = AttributeValue3
                code.SetAttributeNode(Attribute)
            End If

            If AppendTo Is Nothing Then
                xml.DocumentElement.AppendChild(code)
            Else
                AppendTo.AppendChild(code)
            End If

        End Sub

        Private Sub AddAttributeValues2xmlElement(ByRef xml As XmlDocument, ByRef ElementAppendTo As XmlElement, _
                                     ByVal AttributeName1 As String, ByVal AttributeValue1 As String, _
                                     Optional ByVal AttributeName2 As String = "", Optional ByVal AttributeValue2 As String = "", _
                                      Optional ByVal AttributeName3 As String = "", Optional ByVal AttributeValue3 As String = "")

            Dim Attribute As XmlAttribute = xml.CreateAttribute(AttributeName1)
            Attribute.Value = AttributeValue1
            ElementAppendTo.SetAttributeNode(Attribute)

            If Not AttributeName2.Trim = "" Then
                Attribute = xml.CreateAttribute(AttributeName2)
                Attribute.Value = AttributeValue2
                ElementAppendTo.SetAttributeNode(Attribute)
            End If

            If Not AttributeName3.Trim = "" Then
                Attribute = xml.CreateAttribute(AttributeName3)
                Attribute.Value = AttributeValue3
                ElementAppendTo.SetAttributeNode(Attribute)
            End If

        End Sub


    End Class

End Namespace
