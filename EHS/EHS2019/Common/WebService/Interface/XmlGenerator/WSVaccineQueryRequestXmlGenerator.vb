Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Data
Imports System.Xml
Imports common.ComFunction.Generator
Imports Common
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.ComFunction

Namespace WebService.Interface.XmlGenerator
    Public Class WSVaccineQueryRequestXmlGenerator
        Inherits AbstractXmlGenerator

        Private Const SYS_PARAM_ENABLE_MSG_ID As String = "CMS_Get_Vaccine_WS_Enable_MessageID"

        Private Const FORMAT_DOB_DATE As String = "dd/MM/yyyy"
        Private Const REQUEST_SYSTEN As String = "eHS"
        Private Const VALUE_HEALTH_CHECK_NO As String = "N" ' CRE11-002

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private _objRequestPatient As Object
        Private _intCMSWSVersion As Integer
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE10-035
        Private _strMessageID As String = String.Empty
        Public ReadOnly Property MessageID() As String
            Get
                Return _strMessageID
            End Get
        End Property

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Sub New(ByVal udtRequestPatientList As EHSPersonalInformationModelCollection, ByVal intCMSWSVersion As Integer)
            _objRequestPatient = udtRequestPatientList
            _intCMSWSVersion = intCMSWSVersion
        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Public Overrides Function Convert() As System.Xml.XmlDocument
            If TypeOf _objRequestPatient Is EHSPersonalInformationModelCollection Then
                Dim udtPatientList As EHSPersonalInformationModelCollection = CType(_objRequestPatient, EHSPersonalInformationModelCollection)
                Return HandleConvert(udtPatientList)
            Else
                Throw New Exception("WSVaccineQueryRequestXmlGenerator: Invalid Patient Object Type")
            End If
        End Function

        Private Function HandleConvert(ByVal udtPatientList As EHSPersonalInformationModelCollection) As System.Xml.XmlDocument
            Try
                Dim objFormatter As New Format.Formatter
                Dim udtComGeneral As New Common.ComFunction.GeneralFunction()
                ' CRE10-035
                Dim blnEnableMessageID As Boolean = EnableMessageID()
                _strMessageID = IIf(blnEnableMessageID, udtComGeneral.generateEVaccineMessageID(), String.Empty)

                ' Create empty dataset by schema
                ' -------------------------------------------------------------------------
                Dim ds As DataSet = CreateEmptyDataSet(_intCMSWSVersion)

                ' Fill value to dataset
                ' -------------------------------------------------------------------------
                Dim dt As DataTable
                Dim dr As DataRow

                ' Fill parameter record
                dt = ds.Tables("parameter")
                dr = dt.NewRow

                dr("parameter_Id") = 0 'Key
                dr("message_id") = _strMessageID ' CRE10-035
                dr("health_check") = VALUE_HEALTH_CHECK_NO ' CRE11-002
                dr("injection_date_start") = ""
                dr("injection_date_end") = ""
                dr("request_system") = REQUEST_SYSTEN

                dt.Rows.Add(dr)

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
                ' ----------------------------------------------------------
                Select Case _intCMSWSVersion
                    'Existing Version
                    Case WSProxyCMS.CMS_XML_Version.ONE
                        ' Fill patient record
                        dt = ds.Tables("patient")
                        dr = dt.NewRow
                        dr("parameter_Id") = 0 'Key
                        dr("patient_Id") = 0 'Key
                        dr("document_no") = objFormatter.formatDocumentIdentityNumber(udtPatientList(0).DocCode, udtPatientList(0).IdentityNum)
                        dr("document_count") = udtPatientList.Count

                        dt.Rows.Add(dr)

                        ' Fill patient_document
                        dt = ds.Tables("patient_document")

                        For Each udtPatient As EHSPersonalInformationModel In udtPatientList
                            dr = dt.NewRow

                            dr("patient_Id") = 0 'Key
                            dr("document_type") = MapDocCode(udtPatient, WSProxyCMS.CMS_XML_Version.ONE)
                            dr("name") = udtPatient.EName
                            dr("sex") = udtPatient.Gender
                            dr("dob") = udtPatient.DOB.ToString(FORMAT_DOB_DATE)
                            dr("dob_format") = MapDOBFormat(udtPatient)

                            dt.Rows.Add(dr)
                        Next

                        'New Version
                    Case WSProxyCMS.CMS_XML_Version.TWO
                        'Validation
                        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                        'If udtPatientList.Count <> 1 Then
                        '    Throw New Exception(String.Format("Incorrect number of personal information in generating XML request: {0}", udtPatientList.Count))
                        'End If
                        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]

                        ' Fill patient list
                        dt = ds.Tables("patient_list")
                        dr = dt.NewRow
                        dr("patient_list_Id") = 0 'Key
                        dr("patient_count") = udtPatientList.Count
                        dr("parameter_Id") = 0

                        dt.Rows.Add(dr)


                        ' Fill patient record
                        For idx As Integer = 1 To dr("patient_count")

                            dt = ds.Tables("patient")
                            dr = dt.NewRow
                            dr("patient_list_Id") = 0 'Key
                            dr("patient_id") = idx
                            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
                            dr("document_no") = objFormatter.formatDocumentIdentityNumber(udtPatientList(idx - 1).DocCode, udtPatientList(idx - 1).IdentityNum)
                            'dr("document_no") = objFormatter.formatDocumentIdentityNumber(udtPatientList(0).DocCode, udtPatientList(0).IdentityNum)
                            'For Each udtPatient As EHSPersonalInformationModel In udtPatientList
                            dr("document_type") = MapDocCode(udtPatientList(idx - 1), WSProxyCMS.CMS_XML_Version.TWO)
                            dr("name") = udtPatientList(idx - 1).EName
                            dr("sex") = udtPatientList(idx - 1).Gender

                            dr("dob") = udtPatientList(idx - 1).DOB.ToString(FORMAT_DOB_DATE, New System.Globalization.CultureInfo("en-US"))
                            'dr("dob") = udtPatientList(idx - 1).DOB.ToString(FORMAT_DOB_DATE)

                            dr("dob_format") = MapDOBFormat(udtPatientList(idx - 1))
                            'Next
                            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
                            dt.Rows.Add(dr)

                        Next

                    Case Else
                        Throw New Exception(String.Format("Incorrect SystemParameters value of CMS_Get_Vaccine_WS_Version: {0}", WSProxyCMS.GetCMSWSVersion()))

                End Select
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

                ' Fill vaccine record
                Dim udtHAVaccinceBLL As New Component.HATransaction.HAVaccineBLL()
                Dim listVaccineCodeMapping As List(Of Component.HATransaction.VaccineCodeMappingModel) = udtHAVaccinceBLL.GetAllVaccineCodeMapping().GetListBySystem(Component.HATransaction.VaccineCodeMappingModel.SourceSystemClass.CMS, _
                                                                                                         Component.HATransaction.VaccineCodeMappingModel.TargetSystemClass.EHS)

                If listVaccineCodeMapping Is Nothing Then
                    ' Fill vaccine record
                    dt = ds.Tables("vaccine")
                    dr = dt.NewRow
                    dr("vaccine_count") = 0
                    dr("parameter_Id") = 0
                    dr("vaccine_Id") = 0
                    dt.Rows.Add(dr)
                Else

                    ' Fill vaccine record
                    Dim drVaccine As DataRow
                    dt = ds.Tables("vaccine")
                    drVaccine = dt.NewRow
                    drVaccine("vaccine_count") = 0 ' Update it after add all mapped vaccine code
                    drVaccine("parameter_Id") = 0
                    drVaccine("vaccine_id") = 0
                    dt.Rows.Add(drVaccine)


                    ' Fill vaccine record
                    Dim iEnquiryCodeCount As Integer = 0
                    dt = ds.Tables("vaccine_code")
                    For Each mapping As Component.HATransaction.VaccineCodeMappingModel In listVaccineCodeMapping
                        If mapping.ForEnquiry Then
                            dr = dt.NewRow
                            dr("vaccine_code_Text") = mapping.VaccineCodeSource
                            dr("vaccine_id") = 0
                            dt.Rows.Add(dr)
                            iEnquiryCodeCount += 1
                        End If
                    Next

                    ' Update vaccine count
                    drVaccine("vaccine_count") = iEnquiryCodeCount
                End If

                ' CRE10-035
                ' Handle disable message ID
                ' -------------------------------------------------------------------------
                If blnEnableMessageID = False Then
                    ds.Tables("parameter").Columns.Remove("message_id")
                End If

                ' Convert Dataset to XML
                ' -------------------------------------------------------------------------
                Dim xmlDoc As XmlDocument = ComFunction.XmlFunction.Dataset2Xml(ds, XmlWriteMode.IgnoreSchema)
                Me.ReplaceRootBy2ndRoot(xmlDoc)

                Return xmlDoc
            Catch ex As Exception
                Throw
            End Try
        End Function

        ''' <summary>
        ''' CRE10-035
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EnableMessageID() As Boolean
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            If oGenFunc.getSystemParameter(SYS_PARAM_ENABLE_MSG_ID, sValue, String.Empty) Then
                If sValue = "Y" Then
                    Return True
                Else
                    Return False
                End If
            Else
                Throw New Exception(String.Format("Fail to query SystemParameters[{0}]", SYS_PARAM_ENABLE_MSG_ID))
            End If
        End Function

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Function MapDocCode(ByVal udtPatient As EHSPersonalInformationModel, ByVal enumCMSWSVersion As WSProxyCMS.CMS_XML_Version) As String
            ' Map HKIC to HKID for CMS web service
            Dim strDocCode As String = String.Empty
            Select Case udtPatient.DocCode
                Case Common.Component.DocType.DocTypeModel.DocTypeCode.HKIC
                    Select Case enumCMSWSVersion
                        Case WSProxyCMS.CMS_XML_Version.ONE
                            strDocCode = "HKID"
                        Case WSProxyCMS.CMS_XML_Version.TWO
                            strDocCode = udtPatient.DocCode
                        Case Else
                            strDocCode = udtPatient.DocCode
                    End Select

                Case Common.Component.DocType.DocTypeModel.DocTypeCode.EC
                    Select Case enumCMSWSVersion
                        Case WSProxyCMS.CMS_XML_Version.ONE
                            strDocCode = "CE"
                        Case WSProxyCMS.CMS_XML_Version.TWO
                            strDocCode = udtPatient.DocCode
                        Case Else
                            strDocCode = udtPatient.DocCode
                    End Select

                Case Else
                    strDocCode = udtPatient.DocCode
            End Select

            Return strDocCode

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Function MapDOBFormat(ByVal udtPatient As EHSPersonalInformationModel) As String
            Dim strDOBFormat As String = String.Empty
            Select Case udtPatient.ExactDOB
                Case ExactDOBClass.AgeAndRegistration
                    strDOBFormat = "DD/MM/YYYY"
                Case ExactDOBClass.ExactDate
                    strDOBFormat = "DD/MM/YYYY"
                Case ExactDOBClass.ExactMonth
                    strDOBFormat = "MM/YYYY"
                Case ExactDOBClass.ExactYear
                    strDOBFormat = "YYYY"
                Case ExactDOBClass.ManualExactDate
                    strDOBFormat = "DD/MM/YYYY"
                Case ExactDOBClass.ManualExactMonth
                    strDOBFormat = "MM/YYYY"
                Case ExactDOBClass.ManualExactYear
                    strDOBFormat = "YYYY"
                Case ExactDOBClass.ReportedYear
                    strDOBFormat = "YYYY"
            End Select

            Return strDOBFormat

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Function CreateEmptyDataSet(ByVal intCMSWSVersion As Integer) As DataSet
            Dim ds As DataSet = New DataSet
            Dim dt As DataTable = Nothing
            Dim dc As DataColumn = Nothing
            Dim dcPrimaryKeyCol(0) As DataColumn

            ' Table "parameter"
            dt = New DataTable("parameter")
            dt.Columns.Add("message_id", System.Type.GetType("System.String"))
            dt.Columns.Add("health_check", System.Type.GetType("System.String"))
            dt.Columns.Add("injection_date_start", System.Type.GetType("System.String"))
            dt.Columns.Add("injection_date_end", System.Type.GetType("System.String"))
            dt.Columns.Add("request_system", System.Type.GetType("System.String"))

            dcPrimaryKeyCol(0) = dt.Columns.Add("parameter_Id", System.Type.GetType("System.Int32"))
            dcPrimaryKeyCol(0).Unique = True
            dcPrimaryKeyCol(0).AutoIncrement = True
            dcPrimaryKeyCol(0).ColumnMapping = MappingType.Hidden
            dt.PrimaryKey = dcPrimaryKeyCol

            ds.Tables.Add(dt)

            '--------------------------------------------------

            Select Case intCMSWSVersion
                Case WSProxyCMS.CMS_XML_Version.ONE
                    ' Table "patient"
                    dt = New DataTable("patient")
                    dt.Columns.Add("document_no", System.Type.GetType("System.String"))
                    dt.Columns.Add("document_count", System.Type.GetType("System.String"))

                    dcPrimaryKeyCol(0) = dt.Columns.Add("patient_Id", System.Type.GetType("System.Int32"))
                    dcPrimaryKeyCol(0).Unique = True
                    dcPrimaryKeyCol(0).AutoIncrement = True
                    dcPrimaryKeyCol(0).ColumnMapping = MappingType.Hidden
                    dt.PrimaryKey = dcPrimaryKeyCol

                    dc = dt.Columns.Add("parameter_Id", System.Type.GetType("System.Int32"))
                    dc.ColumnMapping = MappingType.Hidden

                    ds.Tables.Add(dt)

                    '--------------------------------------------------

                    ' Table "patient_document"
                    dt = New DataTable("patient_document")
                    dt.Columns.Add("document_type", System.Type.GetType("System.String"))
                    dt.Columns.Add("name", System.Type.GetType("System.String"))
                    dt.Columns.Add("sex", System.Type.GetType("System.String"))
                    dt.Columns.Add("dob", System.Type.GetType("System.String"))
                    dt.Columns.Add("dob_format", System.Type.GetType("System.String"))

                    dc = dt.Columns.Add("patient_Id", System.Type.GetType("System.Int32"))
                    dc.ColumnMapping = MappingType.Hidden

                    ds.Tables.Add(dt)

                    'Create Data Relation
                    ds.Relations.Add(New DataRelation("parameter_patient", _
                                                      ds.Tables("parameter").Columns("parameter_Id"), _
                                                      ds.Tables("patient").Columns("parameter_Id")))
                    ds.Relations("parameter_patient").Nested = True

                    ds.Relations.Add(New DataRelation("patient_patient_document", _
                                                      ds.Tables("patient").Columns("patient_Id"), _
                                                      ds.Tables("patient_document").Columns("patient_Id")))
                    ds.Relations("patient_patient_document").Nested = True

                Case WSProxyCMS.CMS_XML_Version.TWO
                    ' Table "patient_list"
                    dt = New DataTable("patient_list")
                    dt.Columns.Add("patient_count", System.Type.GetType("System.String"))

                    dcPrimaryKeyCol(0) = dt.Columns.Add("patient_list_Id", System.Type.GetType("System.Int32"))
                    dcPrimaryKeyCol(0).Unique = True
                    dcPrimaryKeyCol(0).AutoIncrement = True
                    dcPrimaryKeyCol(0).ColumnMapping = MappingType.Hidden
                    dt.PrimaryKey = dcPrimaryKeyCol

                    dc = dt.Columns.Add("parameter_Id", System.Type.GetType("System.Int32"))
                    dc.ColumnMapping = MappingType.Hidden

                    ds.Tables.Add(dt)

                    '--------------------------------------------------

                    ' Table "patient"
                    dt = New DataTable("patient")
                    dt.Columns.Add("patient_id", System.Type.GetType("System.String"))
                    dt.Columns.Add("document_no", System.Type.GetType("System.String"))
                    dt.Columns.Add("document_type", System.Type.GetType("System.String"))
                    dt.Columns.Add("name", System.Type.GetType("System.String"))
                    dt.Columns.Add("sex", System.Type.GetType("System.String"))
                    dt.Columns.Add("dob", System.Type.GetType("System.String"))
                    dt.Columns.Add("dob_format", System.Type.GetType("System.String"))

                    dc = dt.Columns.Add("patient_list_Id", System.Type.GetType("System.Int32"))
                    dc.ColumnMapping = MappingType.Hidden

                    ds.Tables.Add(dt)

                    'Create Data Relation
                    ds.Relations.Add(New DataRelation("parameter_patient_list", _
                                                      ds.Tables("parameter").Columns("parameter_Id"), _
                                                      ds.Tables("patient_list").Columns("parameter_Id")))
                    ds.Relations("parameter_patient_list").Nested = True

                    ds.Relations.Add(New DataRelation("patient_list_patient", _
                                                      ds.Tables("patient_list").Columns("patient_list_Id"), _
                                                      ds.Tables("patient").Columns("patient_list_Id")))
                    ds.Relations("patient_list_patient").Nested = True
            End Select

            '--------------------------------------------------

            ' Table "vaccine"
            dt = New DataTable("vaccine")
            dt.Columns.Add("vaccine_count", System.Type.GetType("System.String"))

            dcPrimaryKeyCol(0) = dt.Columns.Add("vaccine_Id", System.Type.GetType("System.Int32"))
            dcPrimaryKeyCol(0).Unique = True
            dcPrimaryKeyCol(0).AutoIncrement = True
            dcPrimaryKeyCol(0).ColumnMapping = MappingType.Hidden
            dt.PrimaryKey = dcPrimaryKeyCol

            dc = dt.Columns.Add("parameter_Id", System.Type.GetType("System.Int32"))
            dc.ColumnMapping = MappingType.Hidden

            ds.Tables.Add(dt)

            'Create Data Relation
            ds.Relations.Add(New DataRelation("parameter_vaccine", _
                                              ds.Tables("parameter").Columns("parameter_Id"), _
                                              ds.Tables("vaccine").Columns("parameter_Id")))
            ds.Relations("parameter_vaccine").Nested = True

            '--------------------------------------------------

            ' Table "vaccine_code"
            dt = New DataTable("vaccine_code")

            dc = dt.Columns.Add("vaccine_code_Text", System.Type.GetType("System.String"))
            dc.ColumnMapping = MappingType.SimpleContent

            dc = New DataColumn("vaccine_Id")
            dc.DataType = System.Type.GetType("System.Int32")
            dc.ColumnMapping = MappingType.Hidden
            dt.Columns.Add(dc)

            ds.Tables.Add(dt)

            'Create Data Relation
            ds.Relations.Add(New DataRelation("vaccine_vaccine_code", _
                                              ds.Tables("vaccine").Columns("vaccine_Id"), _
                                              ds.Tables("vaccine_code").Columns("vaccine_Id")))
            ds.Relations("vaccine_vaccine_code").Nested = True
            '--------------------------------------------------

            Return ds

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

    End Class



End Namespace
