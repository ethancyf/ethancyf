Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Validation
Imports TestWSforHKMA.Component.Request.Base

Namespace Component.Request

    Public Class eHSAccountSubsidyQueryRequest
        Inherits BaseWSAccountRequest

#Region "Private Constant"

        Private Const TAG_CLAIM_INFO As String = "ClaimInfo"
        Private Const TAG_CLAIM_DETAIL As String = "ClaimDetail"
        Private Const TAG_SERVICE_DATE As String = "ServiceDate"
        Private Const TAG_SCHEME_CODE As String = "SchemeCode"

        Private Const TAG_VOUCHER_INFO As String = "VoucherInfo"
        Private Const TAG_VOUCHER_CLAIMED As String = "VoucherClaimed"
        Private Const TAG_REASON_FOR_VISIT As String = "ReasonForVisit"
        Private Const TAG_PROF_CODE As String = "ProfCode"
        Private Const TAG_L1_CODE As String = "L1Code"
        Private Const TAG_L1_DESC_ENG As String = "L1DescEng"
        Private Const TAG_L2_CODE As String = "L2Code"
        Private Const TAG_L2_DESC_ENG As String = "L2DescEng"

        Private Const TAG_VACCINE_INFO As String = "VaccineInfo"
        Private Const TAG_SUBSIDY_CODE As String = "SubsidyCode"
        Private Const TAG_DOSE_SEQ As String = "DoseSeq"
        Private Const TAG_RCH_CODE As String = "RCHCode"

        Private Const TAG_INDICATOR As String = "Indicator"
        Private Const TAG_WARN_CODE As String = "WarnCode"
        Private Const TAG_WARN_INDICATOR As String = "WarnIndicator"

        'Private Const ERR_TAG_NOT_FOUND As String = "{0} tag not found"
        'Private Const ERR_TAG_DUPLICATE As String = "Duplicate {0} tag found"
        'Private Const ERR_TAG_INVALID_VALUE As String = "Invalid {0} tag value"
        'Private Const ERR_ITEM_NOT_MATCH_COUNT As String = "Number of {0} is not match {1}"

#End Region

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
            Dim nodeMessageID As XmlElement
            nodeMessageID = xml.CreateElement("MessageID")
            nodeMessageID.InnerText = RandomKey
            nodeInput.AppendChild(nodeMessageID)


            'SP Info
            If Me.SPInfo_inXML Then
                GenSPResult(xml, nodeInput)
            End If

            'Account Info
            If Me.AccountInfo_inXML Then
                GenAccountResult(xml, nodeInput)
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

#End Region

    End Class

End Namespace


