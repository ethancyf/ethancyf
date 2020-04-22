Imports System.Web
Imports System.Globalization
Imports System.Xml
Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.Component.Mapping
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Profession
Imports Common.Component.ThirdParty
Imports Common.Format
Imports Common.Component.ClaimCategory
Imports Common.Component.Scheme
Imports Common.Component.SchemeInformation

Public Class WSXmlGenerator

    Private Shared udtFormatter As New Formatter
    Private Shared _XML As New XmlDocument

#Region "Classes"
    Private Class SchemeInfoXML
        Public SchemeCode As String
        Public ClinicType As String
        Public CategoryList As ClaimCategoryModelCollection
    End Class
#End Region


    Public Shared Sub CreateNode(ByVal xml As XmlDocument, ByRef ParentNode As XmlElement, ByVal strTag As String, ByVal strValue As String)
        Dim nodeNew As XmlElement

        nodeNew = xml.CreateElement(strTag)
        nodeNew.InnerText = strValue
        ParentNode.AppendChild(nodeNew)
    End Sub

    Public Shared Function GenMessageID() As String
        Dim strMsgID As String = String.Empty
        Dim udtComGeneral As New Common.ComFunction.GeneralFunction()
        strMsgID = udtComGeneral.generatePCDWSMessageID()
        Return strMsgID
    End Function

    Private Const FORMAT_PCD_DATETIME As String = "dd/MM/yyyy HH:mm:ss"
    Private Const FORMAT_ENROLMENT_SUBMISSION_TIME As String = "dd/MM/yyyy HH:mm:ss"

    Public Property XML() As XmlDocument
        Get
            Return _XML
        End Get
        Set(ByVal value As XmlDocument)
            _XML = value
        End Set
    End Property

    Public Sub New()
    End Sub

#Region "XML Tags"

    Public MustInherit Class XmlTags
        Public Shadows TAG_ROOT As String = String.Empty
        Public Shadows TAG_MESSAGE_ID As String = String.Empty
        Public Shadows TAG_WS_METHOD_NAME As String = String.Empty
        Public Shadows TAG_MESSAGE_DATETIME As String = String.Empty
    End Class

    Public Class XmlTagsListenFromPCD
        Inherits XmlTags

        Public Shadows Const TAG_ROOT As String = "result"
        Public Shadows Const TAG_MESSAGE_ID As String = "message_id"
        Public Shadows Const TAG_WS_METHOD_NAME As String = "ws_method_name"
        Public Shadows Const TAG_MESSAGE_DATETIME As String = "message_datetime"

        Public Const TAG_RETURN_CODE As String = "return_code"
        Public Const TAG_ACTIVATION_LINK As String = "activation_link"
        Public Const TAG_ERN As String = "ern"
        Public Const TAG_ENROL_PDF_URL As String = "enrol_pdf_url"
    End Class

#End Region

#Region "Generate XML Functions for Requests"

    Public Class Request

        Private Const TAG_ROOT As String = "parameter"
        Private Const TAG_MESSAGE_ID As String = "message_id"
        Private Const TAG_WS_METHOD_NAME As String = "ws_method_name"
        Private Const TAG_ENROLMENT_METHOD As String = "enrolment_method"
        Private Const TAG_MESSAGE_DATETIME As String = "message_datetime"
        Private Const TAG_ERN As String = "ern"
        Private Const TAG_ENROLMENT_SUBMISSION_TIME As String = "enrolment_submission_time"

        Private Const TAG_PROVIDER As String = "provider"
        Private Const TAG_HKIC_NO As String = "hkic_no"
        Private Const TAG_PROVIDER_NAME_EN As String = "provider_name_en"
        Private Const TAG_PROVIDER_NAME_TC As String = "provider_name_tc"
        Private Const TAG_PROVIDER_EMAIL_ADDRESS As String = "email_address"
        Private Const TAG_PROVIDER_CORR_ADDR_EN As String = "corr_addr_en"
        Private Const TAG_PROVIDER_DISTRICT_ID As String = "district_id"

        Private Const TAG_PRACTICE_RECORD As String = "practice_record"
        Private Const TAG_PRACTICE_RECORD_SEQ As String = "seq"
        Private Const TAG_PRACTICE_RECORD_PROF_ID As String = "prof_id"
        Private Const TAG_PRACTICE_RECORD_PROF_EHS_NAME As String = "profession_name"
        Private Const TAG_PRACTICE_RECORD_SUB_PROF_ID As String = "sub_prof_id"
        Private Const TAG_PRACTICE_RECORD_REG_NO As String = "reg_no"
        Private Const TAG_PRACTICE_RECORD_TYPE_OF_PRACTICE_ID As String = "type_of_practice_id"
        Private Const TAG_PRACTICE_RECORD_PRACTICE_NAME_EN As String = "practice_name_en"
        Private Const TAG_PRACTICE_RECORD_PRACTICE_NAME_TC As String = "practice_name_tc"
        Private Const TAG_PRACTICE_RECORD_CORR_ADDR_EN As String = "corr_addr_en"
        Private Const TAG_PRACTICE_RECORD_CORR_ADDR_TC As String = "corr_addr_tc"
        Private Const TAG_PRACTICE_RECORD_DISTRICT_ID As String = "district_id"
        Private Const TAG_PRACTICE_RECORD_PHONE_NO As String = "phone_no"
        Private Const TAG_PRACTICE_RECORD_GOV_PROGRAM As String = "gov_program"
        Private Const TAG_PRACTICE_RECORD_GOV_PROGRAM_GOV_PROGRAM_ID As String = "gov_program_id"
        Private Const TAG_PRACTICE_RECORD_GOV_PROGRAM_COUNT As String = "gov_program_count"
        Private Const TAG_PRACTICE_RECORD_COUNT As String = "practice_record_count"

        ' CRE16-021 Transfer VSS category to PCD [Start][Winnie]
        Private Const TAG_PRACTICE_RECORD_GOV_PROGRAM_Clinic_Type As String = "clinic_type"
        Private Const TAG_PRACTICE_RECORD_GOV_PROGRAM_CATEGORIES As String = "categories"
        Private Const TAG_PRACTICE_RECORD_GOV_PROGRAM_CATEGORIES_CATEGORY As String = "category"
        Private Const TAG_PRACTICE_RECORD_GOV_PROGRAM_CATEGORIES_CATEGORY_CATEGORY_CODE As String = "category_code"
        Private Const TAG_PRACTICE_RECORD_GOV_PROGRAM_CATEGORIES_CATEGORY_COUNT As String = "category_count"
        ' CRE16-021 Transfer VSS category to PCD [End][Winnie]

        Public ReadOnly Property TAGROOT() As String
            Get
                Return TAG_ROOT
            End Get
        End Property

        Public Sub GenerateXMLRequestAttributes(ByVal xml As XmlDocument, ByVal node As XmlElement, ByVal strMessageID As String, ByVal strMethodName As String, ByVal blnNoMessageID As Boolean, Optional ByVal strEnrolmentMethod As String = "", Optional ByVal strERN As String = "", Optional ByVal blnIncludeEnrolSubmissionTime As Boolean = False, Optional ByVal dtmEnrolSubmissionTime As DateTime = Nothing)

            Dim nodeMesssageID As XmlElement

            nodeMesssageID = xml.CreateElement(TAG_MESSAGE_ID)
            If Not blnNoMessageID Then
                nodeMesssageID.InnerText = strMessageID
            End If
            node.AppendChild(nodeMesssageID)

            Dim nodeMethodName As XmlElement

            nodeMethodName = xml.CreateElement(TAG_WS_METHOD_NAME)
            nodeMethodName.InnerText = strMethodName
            node.AppendChild(nodeMethodName)

            Dim nodeEnrolmentMethod As XmlElement

            If strEnrolmentMethod <> "" Then
                nodeEnrolmentMethod = xml.CreateElement(TAG_ENROLMENT_METHOD)
                nodeEnrolmentMethod.InnerText = strEnrolmentMethod
                node.AppendChild(nodeEnrolmentMethod)
            End If

            Dim nodeERN As XmlElement

            If strERN <> "" Then
                nodeERN = xml.CreateElement(TAG_ERN)
                nodeERN.InnerText = strERN
                node.AppendChild(nodeERN)
            End If

            Dim nodeEnrolSubmissionTime As XmlElement

            If blnIncludeEnrolSubmissionTime Then
                nodeEnrolSubmissionTime = xml.CreateElement(TAG_ENROLMENT_SUBMISSION_TIME)
                nodeEnrolSubmissionTime.InnerText = dtmEnrolSubmissionTime.ToString(FORMAT_ENROLMENT_SUBMISSION_TIME)
                node.AppendChild(nodeEnrolSubmissionTime)
            End If

        End Sub

        Public Function AppendMessageIDtoXML(ByVal strXML As String, ByVal strMessageID As String) As String
            Dim xmlDoc As New XmlDocument
            Dim Nodes As XmlNodeList
            Dim Node As XmlNode

            xmlDoc.LoadXml(strXML)
            Nodes = xmlDoc.GetElementsByTagName(TAG_MESSAGE_ID)
            For Each Node In Nodes
                Node.InnerText = strMessageID
            Next

            Nodes = xmlDoc.GetElementsByTagName(TAG_MESSAGE_DATETIME)
            For Each Node In Nodes
                Node.InnerText = DateTime.Now.ToString(FORMAT_PCD_DATETIME)
            Next

            Return xmlDoc.InnerXml
        End Function

        Public Sub GenerateXMLRequestMessageDateTime(ByVal xml As XmlDocument, ByVal node As XmlElement, Optional ByVal blnNoValue As Boolean = False)
            Dim nodeDTM As XmlElement

            nodeDTM = xml.CreateElement(TAG_MESSAGE_DATETIME)
            If Not blnNoValue Then
                nodeDTM.InnerText = DateTime.Now.ToString(FORMAT_PCD_DATETIME)
            End If
            node.AppendChild(nodeDTM)
        End Sub

        Public Sub GenerateXMLServiceProvider(ByVal xml As XmlDocument, ByVal node As XmlElement, ByVal udtSP As ServiceProviderModel, ByVal blnIncludeAllServiceProviderDetails As Boolean, ByVal blnIncludePracticeInfo As Boolean, ByVal blnIncludePracticeSchemeInfo As Boolean)
            ' Structure of hshPracSchemeInfoList: (PracticeDisplaySeq = key, [SchemeCode1, SchemeCode2, ...])
            ' using type Hashtable(key, object=Hashtable)

            If Not udtSP Is Nothing Then
                Dim nodeProvider As XmlElement
                nodeProvider = xml.CreateElement(TAG_PROVIDER)

                ' HKIC_NO
                CreateNode(xml, nodeProvider, TAG_HKIC_NO, udtSP.HKID)

                If blnIncludeAllServiceProviderDetails Then
                    ' Provider Name (English)
                    CreateNode(xml, nodeProvider, TAG_PROVIDER_NAME_EN, udtSP.EnglishName)

                    ' Provider Name (Chinese)
                    If udtSP.ChineseName <> "" Then
                        CreateNode(xml, nodeProvider, TAG_PROVIDER_NAME_TC, udtSP.ChineseName)
                    End If

                    ' email_address
                    CreateNode(xml, nodeProvider, TAG_PROVIDER_EMAIL_ADDRESS, udtSP.Email)

                    ' corresponding address
                    Dim strAddr As String
                    If Not udtSP.SpAddress Is Nothing Then
                        strAddr = udtFormatter.FormatAddressWithoutDistrict(udtSP.SpAddress)
                        If strAddr <> "" Then
                            CreateNode(xml, nodeProvider, TAG_PROVIDER_CORR_ADDR_EN, strAddr)
                        End If
                    End If

                    ' district id
                    If udtSP.SpAddress.District <> "" Then
                        CreateNode(xml, nodeProvider, TAG_PROVIDER_DISTRICT_ID, udtSP.SpAddress.District)
                    End If

                End If

                ' to include practice information in its practice list
                If blnIncludePracticeInfo Then
                    GenerateXMLPracticeInfoCollection(xml, nodeProvider, udtSP.PracticeList, udtSP.ThirdPartyAdditionalFieldEnrolmentList, blnIncludePracticeSchemeInfo)
                End If

                node.AppendChild(nodeProvider)
            End If
        End Sub

        Public Sub GenerateXMLPracticeInfoCollection(ByVal xml As XmlDocument, ByVal node As XmlElement, ByVal udtPracCollection As PracticeModelCollection, ByVal udtThirdPartyList As ThirdPartyAdditionalFieldEnrolmentCollection, ByVal blnIncludeSchemeInfo As Boolean)
            If Not udtPracCollection Is Nothing AndAlso udtPracCollection.Count > 0 AndAlso Not udtThirdPartyList Is Nothing AndAlso udtThirdPartyList.Count > 0 Then
                ' Practice List
                'Dim i As Integer = 1
                Dim practice_record_count As Integer = 0

                ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                'For i = 1 To udtPracCollection.Count
                '    Dim udtPrac As PracticeModel = udtPracCollection.Item(i)

                For Each udtPrac As PracticeModel In udtPracCollection.Values
                    ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

                    ' skip the practice if it is not selected type of practice
                    If udtPrac Is Nothing OrElse udtThirdPartyList.GetListByPractice(ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD, udtPrac.DisplaySeq).Count = 0 Then
                        Continue For
                    End If

                    Dim nodePracticeRecord As XmlElement
                    nodePracticeRecord = xml.CreateElement(TAG_PRACTICE_RECORD)

                    practice_record_count = practice_record_count + 1

                    ' seq
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_RECORD_SEQ, udtPrac.DisplaySeq.ToString())

                    ' prof_id
                    Dim udtCodeMapList As CodeMappingCollection
                    Dim udtCodeMap As CodeMappingModel
                    udtCodeMapList = CodeMappingBLL.GetAllCodeMapping
                    udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.EHS, CodeMappingModel.EnumTargetSystem.PCD, CodeMappingModel.EnumCodeType.Service_Category_Code.ToString, udtPrac.Professional.ServiceCategoryCode)
                    If Not udtCodeMap Is Nothing Then
                        CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_RECORD_PROF_ID, udtCodeMap.CodeTarget)
                    End If

                    ' sub_prof_id
                    Dim strCMPTypeID As String = String.Empty
                    Dim strTmpSubProfModel As SubProfessionModel
                    strTmpSubProfModel = ProfessionBLL.GetSubProfessionByServiceCategoryCode(udtPrac.Professional.ServiceCategoryCode)
                    If Not strTmpSubProfModel Is Nothing Then
                        strCMPTypeID = strTmpSubProfModel.SubServiceCategoryCode
                    End If
                    If strCMPTypeID <> "" Then
                        CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_RECORD_SUB_PROF_ID, strCMPTypeID)
                    End If

                    ' reg_no
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_RECORD_REG_NO, udtPrac.Professional.RegistrationCode)

                    ' type_of_practice_id
                    Dim strTypeOfPracticeID As String = String.Empty
                    If Not udtThirdPartyList Is Nothing AndAlso udtThirdPartyList.GetListBySysCode(ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD).Count > 0 Then
                        Dim udtThirdPartyModel As ThirdPartyAdditionalFieldEnrolmentModel = udtThirdPartyList.GetByValueCode(ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD, _
                                                                                                                             udtPrac.DisplaySeq, _
                                                                                                                             EnumConstant.EnumAdditionalFieldID.TYPE_OF_PRACTICE.ToString())
                        If Not udtThirdPartyModel Is Nothing Then
                            strTypeOfPracticeID = udtThirdPartyModel.AdditionalFieldValueCode
                        End If
                    End If
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_RECORD_TYPE_OF_PRACTICE_ID, strTypeOfPracticeID)

                    ' CRE16-022 (Add optional field "Remarks") [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    ' practice_name_en
                    Dim strPracNameEng As String = udtPrac.PracticeName
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_RECORD_PRACTICE_NAME_EN, strPracNameEng)

                    ' practice_name_tc
                    If udtPrac.PracticeNameChi <> "" Then
                        Dim strPracNameChi As String = udtPrac.PracticeNameChi
                        CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_RECORD_PRACTICE_NAME_TC, strPracNameChi)
                    End If

                    ' corr_addr_en & corr_addr_tc
                    Dim strEngAddr As String = String.Empty
                    Dim strChiAddr As String = String.Empty
                    Dim strPracAddrRemarksEng As String = String.Empty
                    Dim strPracAddrRemarksChi As String = String.Empty

                    Dim intPracticeRemarksMaxLength As Integer = 150

                    If Not udtPrac.PracticeAddress Is Nothing Then
                        strEngAddr = udtFormatter.FormatAddressWithoutDistrict(udtPrac.PracticeAddress)
                        strChiAddr = udtFormatter.FormatAddressChiWithoutDistrict(udtPrac.PracticeAddress)
                    End If

                    '1. corr_addr_en
                    If udtPrac.RemarksDesc <> String.Empty Then
                        strPracAddrRemarksEng = String.Format("({0}) {1}", udtPrac.RemarksDesc, strEngAddr).Trim

                        If strPracAddrRemarksEng.Length > intPracticeRemarksMaxLength Then
                            strEngAddr = String.Format("({0}) {1}", udtPrac.RemarksDesc.Substring(0, intPracticeRemarksMaxLength - strEngAddr.Length - 3), strEngAddr)
                        Else
                            strEngAddr = strPracAddrRemarksEng
                        End If
                    End If

                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_RECORD_CORR_ADDR_EN, strEngAddr)

                    '2 corr_addr_tc
                    If strChiAddr <> "" Or udtPrac.RemarksDescChi <> String.Empty Then

                        If udtPrac.RemarksDescChi <> String.Empty Then
                            strPracAddrRemarksChi = String.Format("{0} ({1})", strChiAddr, udtPrac.RemarksDescChi).Trim

                            If strPracAddrRemarksChi.Length > intPracticeRemarksMaxLength Then
                                If strChiAddr = String.Empty Then
                                    strChiAddr = String.Format("{0} ({1})", strChiAddr, udtPrac.RemarksDescChi.Substring(0, intPracticeRemarksMaxLength - 2)).Trim
                                Else
                                    strChiAddr = String.Format("{0} ({1})", strChiAddr, udtPrac.RemarksDescChi.Substring(0, intPracticeRemarksMaxLength - strChiAddr.Length - 3)).Trim
                                End If
                            Else
                                strChiAddr = strPracAddrRemarksChi
                            End If
                        End If

                        CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_RECORD_CORR_ADDR_TC, strChiAddr)
                    End If
                    ' CRE16-022 (Add optional field "Remarks") [End][Chris YIM]

                    ' district_id
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_RECORD_DISTRICT_ID, udtPrac.PracticeAddress.District)

                    ' phone_no
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_RECORD_PHONE_NO, udtPrac.PhoneDaytime)


                    ' CRE16-021 Transfer VSS category to PCD [Start][Winnie]
                    Dim intSchemeCount As Integer = 0

                    ' Handle Scheme & Category
                    If blnIncludeSchemeInfo AndAlso Not udtPrac.PracticeSchemeInfoList Is Nothing Then

                        ' gov_program
                        Dim dictScheme As New SortedDictionary(Of Integer, SchemeInfoXML)

                        ' Get the Practice scheme info list of correpsonding Practice
                        Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = udtPrac.PracticeSchemeInfoList.FilterByPracticeDisplaySeq(udtPrac.DisplaySeq)
                        Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL


                        For Each udtPracSchemeInfo As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values

                            ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
                            ' ----------------------------------------------------------------------------------------
                            ' The practice and scheme status should be already handled when bind practice
                            ' Only Active (or New) practice and scheme could be go through here
                            ' No need to handle again

                            'If udtPracSchemeInfo.RecordStatus = String.Empty Then
                            '    ' Submitted by eForm
                            'Else
                            '    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                            '    ' ----------------------------------------------------------------------------------------
                            '    ' Since need join PCD in staging stage, so need include Practice Scheme Info staging status for joining PCD
                            '    ' Skip scheme if status is not active
                            '    'If udtPracSchemeInfo.RecordStatus <> PracticeSchemeInfoStatus.Active Then
                            '    If udtPracSchemeInfo.RecordStatus <> PracticeSchemeInfoStatus.Active AndAlso _
                            '        udtPracSchemeInfo.RecordStatus <> PracticeSchemeInfoStagingStatus.Active AndAlso _
                            '        udtPracSchemeInfo.RecordStatus <> PracticeSchemeInfoStagingStatus.Existing AndAlso _
                            '        udtPracSchemeInfo.RecordStatus <> PracticeSchemeInfoStagingStatus.Update Then
                            '        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

                            '        Continue For
                            '    End If
                            'End If
                            ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

                            Dim intSchemeDisplaySeq As Integer = udtPracSchemeInfo.SchemeDisplaySeq

                            ' Gov Program
                            If dictScheme.ContainsKey(intSchemeDisplaySeq) = False Then
                                Dim udtSchemeInfoXML As New SchemeInfoXML

                                udtSchemeInfoXML.SchemeCode = udtPracSchemeInfo.SchemeCode
                                udtSchemeInfoXML.ClinicType = udtPracSchemeInfo.ClinicTypeToString

                                ' Categories
                                Dim udtFilteredPSIList As PracticeSchemeInfoModelCollection = udtPracticeSchemeInfoList.Filter(udtPracSchemeInfo.SchemeCode)
                                Dim udtCategoryList As ClaimCategoryModelCollection = (New ClaimCategoryBLL).getDistinctCategoryByPracticeScheme(udtFilteredPSIList)

                                udtSchemeInfoXML.CategoryList = udtCategoryList

                                dictScheme.Add(intSchemeDisplaySeq, udtSchemeInfoXML)
                            End If
                        Next ' Practice Scheme Info

                        ' Construct Scheme Info XML
                        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = (New SchemeBackOfficeBLL).getAllDistinctSchemeBackOffice

                        For Each udtSchemeInfoXML As SchemeInfoXML In dictScheme.Values
                            Dim nodePracSchemeInfo As XmlElement
                            Dim nodeSchemeCategories As XmlElement
                            Dim intCategoryCount As Integer = 0

                            nodePracSchemeInfo = xml.CreateElement(TAG_PRACTICE_RECORD_GOV_PROGRAM)

                            ' Force EVSSHSIVSS to become scheme code EVSS
                            Dim strSchemeCode As String = IIf(udtSchemeInfoXML.SchemeCode.ToUpper = "EVSSHSIVSS", "EVSS", udtSchemeInfoXML.SchemeCode)

                            ' gov_program_id
                            CreateNode(xml, nodePracSchemeInfo, TAG_PRACTICE_RECORD_GOV_PROGRAM_GOV_PROGRAM_ID, strSchemeCode)

                            ' clinic_type
                            Dim udtSchemeBackOffice As SchemeBackOfficeModel = udtSchemeBackOfficeList.Filter(udtSchemeInfoXML.SchemeCode)
                            If Not udtSchemeBackOffice Is Nothing Then
                                If udtSchemeBackOffice.AllowNonClinicSetting = True Then
                                    CreateNode(xml, nodePracSchemeInfo, TAG_PRACTICE_RECORD_GOV_PROGRAM_Clinic_Type, udtSchemeInfoXML.ClinicType)
                                End If
                            End If

                            ' categories
                            nodeSchemeCategories = xml.CreateElement(TAG_PRACTICE_RECORD_GOV_PROGRAM_CATEGORIES)

                            ' category
                            For Each udtCategoryCode As ClaimCategoryModel In udtSchemeInfoXML.CategoryList
                                Dim nodeSchemeCategory As XmlElement = xml.CreateElement(TAG_PRACTICE_RECORD_GOV_PROGRAM_CATEGORIES_CATEGORY)

                                ' category_code
                                CreateNode(xml, nodeSchemeCategory, TAG_PRACTICE_RECORD_GOV_PROGRAM_CATEGORIES_CATEGORY_CATEGORY_CODE, udtCategoryCode.CategoryCode)

                                nodeSchemeCategories.AppendChild(nodeSchemeCategory)
                                intCategoryCount = intCategoryCount + 1
                            Next

                            ' category_count
                            If intCategoryCount > 0 Then
                                CreateNode(xml, nodeSchemeCategories, TAG_PRACTICE_RECORD_GOV_PROGRAM_CATEGORIES_CATEGORY_COUNT, intCategoryCount)
                                nodePracSchemeInfo.AppendChild(nodeSchemeCategories)
                            End If

                            nodePracticeRecord.AppendChild(nodePracSchemeInfo)
                            intSchemeCount = intSchemeCount + 1
                        Next ' Scheme
                    End If

                    ' gov_program_count
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_RECORD_GOV_PROGRAM_COUNT, intSchemeCount)

                    node.AppendChild(nodePracticeRecord)
                    ' CRE16-021 Transfer VSS category to PCD [End][Winnie]
                Next ' Practice

                ' practice_record_count
                CreateNode(xml, node, TAG_PRACTICE_RECORD_COUNT, practice_record_count.ToString())

            End If
        End Sub
    End Class

#End Region


#Region "Generate Response XML Functions when the eHSPracticeScheme web service is called"

    Public Class eHSPracticeSchemeResponse

        Private Const TAG_ROOT As String = "result"
        Private Const TAG_MESSAGE_ID As String = "message_id"
        Private Const TAG_WS_METHOD_NAME As String = "ws_method_name"
        Private Const TAG_MESSAGE_DATETIME As String = "message_datetime"

        Private Const TAG_RETURN_CODE As String = "return_code"

        Private Const TAG_PRACTICE As String = "practice"
        Private Const TAG_PRACTICE_SEQ As String = "seq"
        Private Const TAG_PRACTICE_PRACTICE_NAME_EN As String = "practice_name_eng"
        Private Const TAG_PRACTICE_PRACTICE_NAME_TC As String = "practice_name_chi"
        Private Const TAG_PRACTICE_PROFESSION_NAME As String = "practice_profession_name"
        Private Const TAG_PRACTICE_CORR_ADDR_EN As String = "corr_addr_en"
        Private Const TAG_PRACTICE_CORR_ADDR_TC As String = "corr_addr_tc"
        Private Const TAG_PRACTICE_DISTRICT_ID As String = "district_code"
        Private Const TAG_PRACTICE_PHONE_NO As String = "practice_phone_no"
        Private Const TAG_PRACTICE_REG_NO As String = "practice_prof_reg_no"
        Private Const TAG_PRACTICE_SCHEME As String = "scheme"
        Private Const TAG_PRACTICE_SCHEME_SCHEME_CODE As String = "scheme_code"
        Private Const TAG_PRACTICE_SCHEME_SCHEME_NAME_EN As String = "scheme_name_en"
        ' CRE16-021 Transfer VSS category to PCD [Start][Winnie]
        Private Const TAG_PRACTICE_SCHEME_Clinic_Type As String = "clinic_type"
        Private Const TAG_PRACTICE_SCHEME_CATEGORIES As String = "categories"
        Private Const TAG_PRACTICE_SCHEME_CATEGORIES_CATEGORY As String = "category"
        Private Const TAG_PRACTICE_SCHEME_CATEGORIES_CATEGORY_CATEGORY_CODE As String = "category_code"
        Private Const TAG_PRACTICE_SCHEME_CATEGORIES_CATEGORY_CATEGORY_NAME_EN As String = "category_name_en"
        Private Const TAG_PRACTICE_SCHEME_CATEGORIES_CATEGORY_COUNT As String = "category_count"
        ' CRE16-021 Transfer VSS category to PCD [End][Winnie]
        Private Const TAG_PRACTICE_SCHEME_COUNT As String = "scheme_count"
        Private Const TAG_PRACTICE_COUNT As String = "practice_count"

        Public ReadOnly Property TAGROOT() As String
            Get
                Return TAG_ROOT
            End Get
        End Property

        Public Sub GenerateXMLResponseAttributes(ByVal xml As XmlDocument, ByVal node As XmlElement, ByVal strMessageID As String, ByVal strMethodName As String, ByVal strReturnCode As String)
            CreateNode(xml, node, TAG_MESSAGE_ID, strMessageID)
            CreateNode(xml, node, TAG_WS_METHOD_NAME, strMethodName)
            CreateNode(xml, node, TAG_RETURN_CODE, strReturnCode)
        End Sub

        Public Sub GenerateXMLResponseMessageDateTime(ByVal xml As XmlDocument, ByVal node As XmlElement)
            CreateNode(xml, node, TAG_MESSAGE_DATETIME, DateTime.Now.ToString(FORMAT_PCD_DATETIME))
        End Sub

        Public Sub GenerateXMLPracticeInfoCollection(ByVal xml As XmlDocument, ByVal node As XmlElement, ByVal udtPracCollection As PracticeModelCollection, ByVal blnIncludeSchemeInfo As Boolean)
            Dim iPracticeCount As Integer = 0
            Dim i As Integer = 0

            If Not udtPracCollection Is Nothing AndAlso udtPracCollection.Count > 0 Then
                ' Practice List

                For i = 1 To udtPracCollection.Count

                    Dim udtPrac As PracticeModel = udtPracCollection.Item(i)
                    Dim nodePracticeRecord As XmlElement
                    Dim udtSchemeInfo As SchemeInformation.SchemeInformationModel = Nothing
                    Dim blnHasActiveScheme As Boolean = False

                    ' Skip non-active practice
                    If udtPrac.RecordStatus <> PracticeStatus.Active Then
                        Continue For
                    End If

                    ' Skip non-active practice scheme
                    blnHasActiveScheme = False
                    For Each udtPracticeSchemeInfo As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPrac.PracticeSchemeInfoList.Values
                        If udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStatus.Active Then
                            blnHasActiveScheme = True
                            Exit For
                        End If
                    Next

                    If Not blnHasActiveScheme Then
                        Continue For
                    End If

                    iPracticeCount += 1

                    ' Start add node
                    nodePracticeRecord = xml.CreateElement(TAG_PRACTICE)

                    ' seq
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_SEQ, udtPrac.DisplaySeq.ToString())

                    ' practice_name_en
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_PRACTICE_NAME_EN, udtPrac.PracticeName)

                    ' practice_name_tc
                    If udtPrac.PracticeNameChi <> "" Then
                        CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_PRACTICE_NAME_TC, udtPrac.PracticeNameChi)
                    End If

                    ' practice_profession_name
                    Dim udtProfessionModel As ProfessionModel
                    udtProfessionModel = ProfessionBLL.GetProfessionListByServiceCategoryCode(udtPrac.Professional.ServiceCategoryCode)
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_PROFESSION_NAME, udtProfessionModel.ServiceCategoryDesc)

                    ' corr_addr_en

                    ' CRE16-021 Transfer VSS category to PCD [Start][Winnie] ' Show Clinic Type in scheme level
                    '' Check whether non-clinic
                    'Dim blnNonClinic As Boolean = False

                    'For Each udtPSI As PracticeSchemeInfoModel In udtPrac.PracticeSchemeInfoList.Values
                    '    If udtPSI.PracticeDisplaySeq = udtPrac.DisplaySeq AndAlso udtPSI.ClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
                    '        blnNonClinic = True
                    '        Exit For
                    '    End If
                    'Next

                    Dim strAddr As String = String.Empty
                    If Not udtPrac.PracticeAddress Is Nothing Then
                        strAddr = udtFormatter.FormatAddressWithoutDistrict(udtPrac.PracticeAddress)

                        'If blnNonClinic Then
                        '    strAddr = String.Format("({0}) {1}", HttpContext.GetGlobalResourceObject("Text", "NonClinic", New CultureInfo(CultureLanguage.English)), strAddr)
                        'End If

                    End If
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_CORR_ADDR_EN, strAddr)

                    ' corr_addr_tc
                    strAddr = ""
                    If Not udtPrac.PracticeAddress Is Nothing Then
                        strAddr = udtFormatter.FormatAddressChiWithoutDistrict(udtPrac.PracticeAddress)
                    End If
                    If strAddr <> "" Then
                        'If blnNonClinic Then
                        '    strAddr += String.Format(" ({0})", HttpContext.GetGlobalResourceObject("Text", "NonClinic", New CultureInfo(CultureLanguage.TradChinese)))
                        'End If

                        CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_CORR_ADDR_TC, strAddr)
                    End If
                    ' CRE16-021 Transfer VSS category to PCD [End][Winnie]

                    ' district_id
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_DISTRICT_ID, udtPrac.PracticeAddress.District)

                    ' phone_no
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_PHONE_NO, udtPrac.PhoneDaytime)

                    ' reg_no
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_REG_NO, udtPrac.Professional.RegistrationCode)


                    'If blnIncludeSchemeInfo Then
                    '    GenerateXMLSchemeInfoCollection(xml, nodePracticeRecord, udtPrac.PracticeSchemeInfoList)
                    'End If

                    ' CRE16-021 Transfer VSS category to PCD [Start][Winnie]
                    Dim intSchemeCount As Integer = 0

                    ' Handle Scheme & Category
                    If blnIncludeSchemeInfo AndAlso Not udtPrac.PracticeSchemeInfoList Is Nothing Then
                        Dim dictScheme As New SortedDictionary(Of Integer, SchemeInfoXML)
                        Dim dtCategory As DataTable = (New ClaimCategoryBLL).GetSubsidizeGroupCategoryCache

                        For Each udtPracSchemeInfo As PracticeSchemeInfoModel In udtPrac.PracticeSchemeInfoList.Values
                            ' Skip scheme if status is not active
                            If udtPracSchemeInfo.RecordStatus <> PracticeSchemeInfoStatus.Active Then
                                Continue For
                            End If

                            Dim intSchemeDisplaySeq As Integer = udtPracSchemeInfo.SchemeDisplaySeq

                            ' Scheme
                            If dictScheme.ContainsKey(intSchemeDisplaySeq) = False Then
                                Dim udtSchemeInfoXML As New SchemeInfoXML

                                udtSchemeInfoXML.SchemeCode = udtPracSchemeInfo.SchemeCode
                                udtSchemeInfoXML.ClinicType = udtPracSchemeInfo.ClinicTypeToString

                                ' Categories
                                Dim udtFilteredPSIList As PracticeSchemeInfoModelCollection = udtPrac.PracticeSchemeInfoList.Filter(udtPracSchemeInfo.SchemeCode)
                                Dim udtCategoryList As ClaimCategoryModelCollection = (New ClaimCategoryBLL).getDistinctCategoryByPracticeScheme(udtFilteredPSIList)

                                udtSchemeInfoXML.CategoryList = udtCategoryList

                                dictScheme.Add(intSchemeDisplaySeq, udtSchemeInfoXML)
                            End If
                        Next ' Practice Scheme Info

                        ' Construct Scheme Info XML
                        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = (New SchemeBackOfficeBLL).GetAllSchemeBackOfficeWithSubsidizeGroup()

                        For Each udtSchemeInfoXML As SchemeInfoXML In dictScheme.Values
                            Dim nodePracSchemeInfo As XmlElement
                            Dim nodeSchemeCategories As XmlElement
                            Dim intCategoryCount As Integer = 0

                            nodePracSchemeInfo = xml.CreateElement(TAG_PRACTICE_SCHEME)

                            ' Force EVSSHSIVSS to become scheme code EVSS
                            Dim strSchemeCode As String = IIf(udtSchemeInfoXML.SchemeCode.ToUpper = "EVSSHSIVSS", "EVSS", udtSchemeInfoXML.SchemeCode)

                            ' scheme_code
                            CreateNode(xml, nodePracSchemeInfo, TAG_PRACTICE_SCHEME_SCHEME_CODE, strSchemeCode)

                            Dim udtSchemeBackOffice As SchemeBackOfficeModel = udtSchemeBackOfficeList.Filter(udtSchemeInfoXML.SchemeCode)
                            If Not udtSchemeBackOffice Is Nothing Then
                                ' scheme_name_en
                                CreateNode(xml, nodePracSchemeInfo, TAG_PRACTICE_SCHEME_SCHEME_NAME_EN, udtSchemeBackOffice.SchemeDesc)

                                ' clinic_type
                                If udtSchemeBackOffice.AllowNonClinicSetting = True Then
                                    CreateNode(xml, nodePracSchemeInfo, TAG_PRACTICE_SCHEME_Clinic_Type, udtSchemeInfoXML.ClinicType)
                                End If
                            End If

                            ' categories
                            nodeSchemeCategories = xml.CreateElement(TAG_PRACTICE_SCHEME_CATEGORIES)

                            ' category
                            For Each udtCategoryCode As ClaimCategoryModel In udtSchemeInfoXML.CategoryList
                                Dim nodeSchemeCategory As XmlElement = xml.CreateElement(TAG_PRACTICE_SCHEME_CATEGORIES_CATEGORY)

                                ' category_code
                                CreateNode(xml, nodeSchemeCategory, TAG_PRACTICE_SCHEME_CATEGORIES_CATEGORY_CATEGORY_CODE, udtCategoryCode.CategoryCode)
                                ' category_name_en
                                CreateNode(xml, nodeSchemeCategory, TAG_PRACTICE_SCHEME_CATEGORIES_CATEGORY_CATEGORY_NAME_EN, udtCategoryCode.CategoryName)

                                nodeSchemeCategories.AppendChild(nodeSchemeCategory)
                                intCategoryCount = intCategoryCount + 1
                            Next

                            ' category_count
                            If intCategoryCount > 0 Then
                                CreateNode(xml, nodeSchemeCategories, TAG_PRACTICE_SCHEME_CATEGORIES_CATEGORY_COUNT, intCategoryCount)
                                nodePracSchemeInfo.AppendChild(nodeSchemeCategories)
                            End If

                            nodePracticeRecord.AppendChild(nodePracSchemeInfo)
                            intSchemeCount = intSchemeCount + 1
                        Next ' Scheme
                    End If
                    ' scheme_count
                    CreateNode(xml, nodePracticeRecord, TAG_PRACTICE_SCHEME_COUNT, intSchemeCount.ToString())

                    node.AppendChild(nodePracticeRecord)
                    ' CRE16-021 Transfer VSS category to PCD [End][Winnie]
                Next ' Practice

                ' practice_record_count
                CreateNode(xml, node, TAG_PRACTICE_COUNT, IIf(Not udtPracCollection Is Nothing, iPracticeCount, "0"))

            End If
        End Sub

        'Public Sub GenerateXMLSchemeInfoCollection(ByVal xml As XmlDocument, ByVal node As XmlElement, ByVal udtPracSchemeInfoList As PracticeSchemeInfoModelCollection)
        '    ' gov_program
        '    If Not udtPracSchemeInfoList Is Nothing AndAlso udtPracSchemeInfoList.Count > 0 Then
        '        Dim nodePracSchemeInfo As XmlElement
        '        nodePracSchemeInfo = xml.CreateElement(TAG_PRACTICE_SCHEME)

        '        Dim j As Integer = 1

        '        For j = 1 To udtPracSchemeInfoList.Count
        '            Dim udtScheme As PracticeSchemeInfoModel = udtPracSchemeInfoList.GetByIndex(j - 1)
        '            CreateNode(xml, nodePracSchemeInfo, TAG_PRACTICE_SCHEME_SCHEME_CODE, udtScheme.SchemeCode)
        '        Next
        '        node.AppendChild(nodePracSchemeInfo)
        '    End If

        '    ' gov_program_count
        '    CreateNode(xml, node, TAG_PRACTICE_SCHEME_COUNT, IIf(Not udtPracSchemeInfoList Is Nothing, udtPracSchemeInfoList.Count.ToString(), "0"))
        'End Sub

    End Class

#End Region


#Region "Generate PDF XML results"

    Public Class GenPDF

        Public Const LANGUAGE_SELECTION_EN As String = "EN"
        Public Const LANGUAGE_SELECTION_TC As String = "TC"

        Private Const TAG_ROOT As String = "createenrollprintout"

        Private Const TAG_PRINTOUTLANGUAGE = "printoutlanguage"
        Private Const TAG_LANGUAGE As String = "language"
        Private Const TAG_MESSAGE_DATETIME As String = "message_datetime"

        Private Const TAG_PROVIDER As String = "provider"
        Private Const TAG_PROVIDER_PCD_ERN As String = "pcd_ern"
        Private Const TAG_PROVIDER_PCD_SUBMISSION_TIME As String = "pcd_submission_time"
        Private Const TAG_PROVIDER_NAME_EN As String = "provider_name_en"
        Private Const TAG_PROVIDER_NAME_TC As String = "provider_name_tc"
        Private Const TAG_PROVIDER_EMAIL As String = "email_address"
        Private Const TAG_PROVIDER_CORR_ADDR_EN As String = "corr_addr_en"
        Private Const TAG_PROVIDER_DISTRICT_ID As String = "district_id"

        Private Const TAG_PROVIDER_PRACTICE As String = "practice_record"
        Private Const TAG_PROVIDER_PRACTICE_SEQ As String = "seq"
        Private Const TAG_PROVIDER_PRACTICE_PROF_ID As String = "prof_id"
        Private Const TAG_PROVIDER_PRACTICE_SUB_PROF_ID As String = "sub_prof_id"
        Private Const TAG_PROVIDER_PRACTICE_REG_NO As String = "reg_no"
        Private Const TAG_PROVIDER_PRACTICE_TYPE_OF_PRACTICE_ID As String = "type_of_practice_id"
        Private Const TAG_PROVIDER_PRACTICE_PRACTICE_NAME_EN As String = "practice_name_en"
        Private Const TAG_PROVIDER_PRACTICE_PRACTICE_NAME_TC As String = "practice_name_tc"
        Private Const TAG_PROVIDER_PRACTICE_CORR_ADDR_EN As String = "corr_addr_en"
        Private Const TAG_PROVIDER_PRACTICE_CORR_ADDR_TC As String = "corr_addr_tc"
        Private Const TAG_PROVIDER_PRACTICE_DISTRICT_ID As String = "district_id"
        Private Const TAG_PROVIDER_PRACTICE_PHONE_NO As String = "phone_no"

        Private Const TAG_PROVIDER_PRACTICE_GOV_PROGRAM As String = "gov_program"
        Private Const TAG_PROVIDER_PRACTICE_GOV_PROGRAM_ID As String = "gov_program_id"
        Private Const TAG_PROVIDER_PRACTICE_GOV_PROGRAM_COUNT As String = "gov_program_count"
        Private Const TAG_PROVIDER_PRACTICE_RECORD_COUNT As String = "practice_count"

        Public ReadOnly Property TAGROOT() As String
            Get
                Return TAG_ROOT
            End Get
        End Property

        Public Sub GenerateXMLPDFAttributes(ByVal xml As XmlDocument, ByVal node As XmlElement, ByVal strlanguage As String)
            ' printoutlanguage
            Dim nodePrintOutLanguage As XmlElement
            nodePrintOutLanguage = xml.CreateElement(TAG_PRINTOUTLANGUAGE)

            ' language
            Dim nodeLanguage As XmlElement

            nodeLanguage = xml.CreateElement(TAG_LANGUAGE)
            nodeLanguage.InnerText = strlanguage
            nodePrintOutLanguage.AppendChild(nodeLanguage)

            node.AppendChild(nodePrintOutLanguage)

        End Sub

        Public Sub GenerateXMLPDFMessageDateTime(ByVal xml As XmlDocument, ByVal node As XmlElement)
            Dim nodeDTM As XmlElement

            nodeDTM = xml.CreateElement(TAG_MESSAGE_DATETIME)
            nodeDTM.InnerText = DateTime.Now.ToString(FORMAT_PCD_DATETIME)
            node.AppendChild(nodeDTM)
        End Sub

        Public Sub GenerateXMLPDFServiceProvider(ByVal xml As XmlDocument, ByVal node As XmlElement, ByVal strPCD_ERN As String, ByVal dtmPCD_SUBMISSION_TIME As DateTime, ByVal udtSP As ServiceProviderModel)
            'Dim udtComGeneral As New Common.PCD.ComFunction

            If Not udtSP Is Nothing Then
                Dim nodeProvider As XmlElement
                nodeProvider = xml.CreateElement(TAG_PROVIDER)

                ' pcd_ern
                'CreateNode(xml, nodeProvider, TAG_PROVIDER_PCD_ERN, udtComGeneral.FormatPCDSystemNumber(strPCD_ERN))
                CreateNode(xml, nodeProvider, TAG_PROVIDER_PCD_ERN, strPCD_ERN)

                ' pcd_submission_time
                CreateNode(xml, nodeProvider, TAG_PROVIDER_PCD_SUBMISSION_TIME, dtmPCD_SUBMISSION_TIME.ToString(FORMAT_PCD_DATETIME))

                ' provider_name_en
                CreateNode(xml, nodeProvider, TAG_PROVIDER_NAME_EN, udtSP.EnglishName)

                ' provider_name_tc
                If udtSP.ChineseName <> String.Empty Then
                    CreateNode(xml, nodeProvider, TAG_PROVIDER_NAME_TC, udtSP.ChineseName)
                End If

                ' email_addres
                CreateNode(xml, nodeProvider, TAG_PROVIDER_EMAIL, udtSP.Email)

                ' corr_addr_en & district_id
                Dim strAddr As String
                If Not udtSP.SpAddress Is Nothing Then
                    strAddr = udtFormatter.FormatAddressWithoutDistrict(udtSP.SpAddress)
                    If strAddr <> "" Then
                        CreateNode(xml, nodeProvider, TAG_PROVIDER_CORR_ADDR_EN, strAddr)
                        CreateNode(xml, nodeProvider, TAG_PROVIDER_DISTRICT_ID, udtSP.SpAddress.District)
                    End If
                End If


                ' practice_record
                Dim i As Integer = 1
                Dim practice_record_count As Integer = 0
                Dim udtPracCollection As PracticeModelCollection
                udtPracCollection = udtSP.PracticeList

                Dim udtThirdParty As ThirdPartyAdditionalFieldEnrolmentCollection
                udtThirdParty = udtSP.ThirdPartyAdditionalFieldEnrolmentList
                If Not udtPracCollection Is Nothing AndAlso udtPracCollection.Count > 0 AndAlso Not udtThirdParty Is Nothing AndAlso udtThirdParty.Count > 0 Then
                    Dim udtPrac As PracticeModel
                    For i = 1 To udtPracCollection.Count
                        udtPrac = udtPracCollection.Item(i)

                        ' skip the practice if it is not selected type of practice
                        If udtPrac Is Nothing OrElse udtSP.ThirdPartyAdditionalFieldEnrolmentList.GetByValueCode(ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD, _
                                                                                                                  udtPrac.DisplaySeq, _
                                                                                                                  EnumConstant.EnumAdditionalFieldID.TYPE_OF_PRACTICE.ToString) Is Nothing Then
                            Continue For
                        End If


                        'For Each udtPrac In udtPracCollection
                        Dim nodePractice As XmlElement
                        nodePractice = xml.CreateElement(TAG_PROVIDER_PRACTICE)

                        practice_record_count = practice_record_count + 1

                        ' seq
                        CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_SEQ, udtPrac.DisplaySeq)

                        ' prof_id
                        Dim udtCodeMapList As CodeMappingCollection
                        Dim udtCodeMap As CodeMappingModel
                        udtCodeMapList = CodeMappingBLL.GetAllCodeMapping
                        udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.EHS, CodeMappingModel.EnumTargetSystem.PCD, CodeMappingModel.EnumCodeType.Service_Category_Code.ToString, udtPrac.Professional.ServiceCategoryCode)
                        If Not udtCodeMap Is Nothing Then
                            CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_PROF_ID, udtCodeMap.CodeTarget)
                        End If


                        ' sub_prof_id
                        Dim strCMPTypeID As String = String.Empty
                        Dim strTmpSubProfModel As SubProfessionModel
                        strTmpSubProfModel = ProfessionBLL.GetSubProfessionByServiceCategoryCode(udtPrac.Professional.ServiceCategoryCode)
                        If Not strTmpSubProfModel Is Nothing Then
                            strCMPTypeID = strTmpSubProfModel.SubServiceCategoryCode
                        End If
                        If strCMPTypeID <> "" Then
                            CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_SUB_PROF_ID, strCMPTypeID)
                        End If

                        ' reg_no
                        CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_REG_NO, udtPrac.Professional.RegistrationCode)

                        ' type_of_practice_id
                        Dim strTypeOfPracticeID As String = String.Empty
                        Dim udtThirdPartyList As ThirdPartyAdditionalFieldEnrolmentCollection = udtSP.ThirdPartyAdditionalFieldEnrolmentList

                        If Not udtThirdPartyList Is Nothing AndAlso udtThirdPartyList.Count > 0 Then
                            Dim udtThirdPartyModel As ThirdPartyAdditionalFieldEnrolmentModel = udtThirdPartyList.GetByValueCode(ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD, _
                                                                                                                                 udtPrac.DisplaySeq, _
                                                                                                                                 EnumConstant.EnumAdditionalFieldID.TYPE_OF_PRACTICE.ToString())
                            If Not udtThirdPartyModel Is Nothing Then
                                strTypeOfPracticeID = udtThirdPartyModel.AdditionalFieldValueCode
                            End If
                        End If
                        CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_TYPE_OF_PRACTICE_ID, strTypeOfPracticeID)

                        ' practice_name_en
                        CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_PRACTICE_NAME_EN, udtPrac.PracticeName)

                        ' practice_name_en
                        If udtPrac.PracticeNameChi <> "" Then
                            CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_PRACTICE_NAME_TC, udtPrac.PracticeNameChi)
                        End If

                        ' corr_addr_en
                        strAddr = ""
                        If Not udtPrac.PracticeAddress Is Nothing Then
                            strAddr = udtFormatter.FormatAddressWithoutDistrict(udtPrac.PracticeAddress)
                            If strAddr <> "" Then
                                CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_CORR_ADDR_EN, strAddr)
                            End If
                        Else
                            CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_CORR_ADDR_EN, strAddr)
                        End If

                        ' corr_addr_tc
                        strAddr = ""
                        If Not udtPrac.PracticeAddress Is Nothing Then

                            strAddr = udtFormatter.FormatAddressChiWithoutDistrict(udtPrac.PracticeAddress)
                            If strAddr <> "" Then
                                CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_CORR_ADDR_TC, strAddr)
                            End If
                        End If

                        ' district_id
                        CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_DISTRICT_ID, udtPrac.PracticeAddress.District)

                        ' phone_no
                        CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_PHONE_NO, udtPrac.PhoneDaytime)

                        ' gov_program
                        Dim count_SchemeCode As Integer = 0
                        Dim j As Integer = 0
                        Dim hash As Hashtable = New Hashtable

                        If Not udtPrac.PracticeSchemeInfoList Is Nothing Then
                            Dim nodePracSchemeInfo As XmlElement
                            Dim udtPracSchemeInfo As PracticeSchemeInfoModel

                            For j = 0 To udtPrac.PracticeSchemeInfoList.Count - 1
                                udtPracSchemeInfo = udtPrac.PracticeSchemeInfoList.GetByIndex(j)
                                ' Force EVSSHSIVSS to become scheme code EVSS
                                Dim strSchemeCode As String = IIf(udtPracSchemeInfo.SchemeCode.ToUpper = "EVSSHSIVSS", "EVSS", udtPracSchemeInfo.SchemeCode)
                                If Not udtPracSchemeInfo Is Nothing AndAlso Not hash.ContainsValue(strSchemeCode) Then
                                    hash.Add(udtPracSchemeInfo.SchemeDisplaySeq, strSchemeCode)
                                    nodePracSchemeInfo = xml.CreateElement(TAG_PROVIDER_PRACTICE_GOV_PROGRAM)
                                    CreateNode(xml, nodePracSchemeInfo, TAG_PROVIDER_PRACTICE_GOV_PROGRAM_ID, strSchemeCode)
                                    nodePractice.AppendChild(nodePracSchemeInfo)
                                    count_SchemeCode = count_SchemeCode + 1
                                End If
                            Next
                        End If

                        ' gov_program_count
                        CreateNode(xml, nodePractice, TAG_PROVIDER_PRACTICE_GOV_PROGRAM_COUNT, count_SchemeCode.ToString())

                        nodeProvider.AppendChild(nodePractice)
                    Next

                    ' practice_record_count
                    CreateNode(xml, nodeProvider, TAG_PROVIDER_PRACTICE_RECORD_COUNT, practice_record_count.ToString())
                End If
                node.AppendChild(nodeProvider)
            End If
        End Sub
    End Class


#End Region



End Class

