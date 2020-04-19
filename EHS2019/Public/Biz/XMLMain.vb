Imports System.Xml
Imports System.IO
Imports Common.Component.EHSAccount
Imports Common
Imports Common.DataAccess
Imports Common.Component
Imports System.Data.SqlClient
Imports Common.Component.DistrictBoard
Imports Common.ComObject
Imports Common.Component.Profession

Public Class XMLMain
    Private Shared XmlFilePath As String = HttpContext.Current.Server.MapPath("~/XMLData/")
    Public Shared DBLink As Boolean = ConfigurationManager.AppSettings("DBLink") = "Y"
    Private Shared mainFormatter As Common.Format.Formatter = New Format.Formatter

    Public Shared Function XmlStringToDataTable(ByVal strXML As String) As DataTable
        Dim docXML As XmlDocument = New XmlDocument()

        docXML.Load(XmlFilePath + strXML + ".xml")
        Dim readerXML As XmlReader = XmlReader.Create(New System.IO.StringReader(docXML.OuterXml))

        Dim ds As DataSet = New DataSet()
        ds.ReadXml(readerXML)

        Dim dt As DataTable = ds.Tables(0)
        Return dt
    End Function
    Public Shared Function XmlStringToDataSet(ByVal strXML As String) As DataSet
        Dim docXML As XmlDocument = New XmlDocument()

        docXML.Load(XmlFilePath + strXML + ".xml")
        Dim readerXML As XmlReader = XmlReader.Create(New System.IO.StringReader(docXML.OuterXml))

        Dim ds As DataSet = New DataSet()
        ds.ReadXml(readerXML)

        Return ds
    End Function
    Public Shared Sub DataTableToXMLString(ByVal dt As DataTable)
        If (String.IsNullOrEmpty(dt.TableName)) Then
            dt.TableName = "NewTable"
        End If
        Dim write As StringWriter = New StringWriter
        dt.WriteXml(write)
        Dim strXML = write.ToString()
        File.WriteAllText("D:\James\ehealth\Public\XMLData\1.xml", strXML, Encoding.UTF8)
        write.Close()
    End Sub
    Public Shared Sub DataSetToXMLString(ByVal ds As DataSet)
        Dim write As StringWriter = New StringWriter
        ds.WriteXml(write)
        Dim strXML = write.ToString()
        File.WriteAllText("D:\James\ehealth\Public\XMLData\1.xml", strXML, Encoding.UTF8)
        write.Close()
    End Sub

    'Profession
    Public Shared Function GetProfessionList() As ProfessionModelCollection
        Dim udtProfessionModelCollection As ProfessionModelCollection = New ProfessionModelCollection
        Dim udtProfessionModel As ProfessionModel

        Dim dtmEnrolPeriodFrom As Nullable(Of DateTime)
        Dim dtmEnrolPeriodTo As Nullable(Of DateTime)

        Dim dtmClaimPeriodFrom As Nullable(Of DateTime)
        Dim dtmClaimPeriodTo As Nullable(Of DateTime)

        Dim dtmSDPeriodFrom As Nullable(Of DateTime)
        Dim dtmSDPeriodTo As Nullable(Of DateTime)

        dtmEnrolPeriodFrom = New Nullable(Of DateTime)
        dtmEnrolPeriodTo = New Nullable(Of DateTime)

        dtmClaimPeriodFrom = New Nullable(Of DateTime)
        dtmClaimPeriodTo = New Nullable(Of DateTime)

        dtmSDPeriodFrom = New Nullable(Of DateTime)
        dtmSDPeriodTo = New Nullable(Of DateTime)

        ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
        If HttpRuntime.Cache("Profession") Is Nothing Then
            'If HttpContext.Current.Cache("Profession") Is Nothing Then
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Dim dt As New DataTable

            Try
                'DB.RunProc("proc_Profession_get_cache", dt)
                dt = XmlStringToDataTable("Profession")

                For Each row As DataRow In dt.Rows

                    ' CRE19-006 (DHC) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    'Add column [EForm_Avail]
                    udtProfessionModel = New ProfessionModel(CType(row.Item("Service_Category_Code"), String).Trim, _
                                                            CType(row.Item("Service_Category_Desc"), String).Trim, _
                                                            CStr(IIf(row.Item("Service_Category_Desc_Chi") Is DBNull.Value, String.Empty, row.Item("Service_Category_Desc_Chi"))), _
                                                            CStr(IIf(row.Item("Service_Category_Desc_CN") Is DBNull.Value, String.Empty, row.Item("Service_Category_Desc_CN"))), _
                                                            dtmEnrolPeriodFrom, _
                                                            dtmEnrolPeriodTo, _
                                                            dtmClaimPeriodFrom, _
                                                            dtmClaimPeriodTo, _
                                                            CType(row.Item("Service_Category_Code_SD"), String).Trim, _
                                                            CType(row.Item("Service_Category_Desc_SD"), String).Trim, _
                                                            CStr(IIf(row.Item("Service_Category_Desc_SD_Chi") Is DBNull.Value, String.Empty, row.Item("Service_Category_Desc_SD_Chi"))), _
                                                            CType(row.Item("SD_Display_Seq"), Integer), _
                                                            dtmSDPeriodFrom, _
                                                            dtmSDPeriodTo, _
                                                            CType(row.Item("EForm_Avail"), String).Trim)
                    ' CRE19-006 (DHC) [End][Winnie]

                    udtProfessionModelCollection.add(udtProfessionModel)
                Next

                Common.ComObject.CacheHandler.InsertCache("Profession", udtProfessionModelCollection)

            Catch ex As Exception
                Throw ex
            End Try
        Else
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            udtProfessionModelCollection = CType(HttpRuntime.Cache("Profession"), ProfessionModelCollection)
            'udtProfessionModelCollection = CType(HttpContext.Current.Cache("Profession"), ProfessionModelCollection)
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
        End If

        Return udtProfessionModelCollection

    End Function
    'Area
    Public Shared Function GetAreaListXML(ByVal enumPlatform As Area.PlatformCode) As DistrictBoardModelCollection
        If Not IsNothing(HttpContext.Current.Cache("HCSDDistrictBoard")) Then
            Return HttpContext.Current.Cache("HCSDDistrictBoard")

        Else
            Dim udtDB As New Database
            Dim dt As New DataTable
            dt = XmlStringToDataTable("Area")

            Dim udtDistrictBoardList As New DistrictBoardModelCollection

            For Each dr As DataRow In dt.Rows
                Select Case enumPlatform
                    Case Area.PlatformCode.EForm
                        If CStr(dr("EForm_Input_Avail")).Trim <> "Y" Then Continue For

                    Case Area.PlatformCode.BO
                        If CStr(dr("BO_Input_Avail")).Trim <> "Y" Then Continue For

                    Case Area.PlatformCode.SD
                        If CStr(dr("SD_Input_Avail")).Trim <> "Y" Then Continue For

                End Select

                Dim udtDistrictBoard As New DistrictBoardModel( _
                    CStr(dr("district_board")).Trim, _
                    CStr(IIf(IsDBNull(dr("district_board_chi")), String.Empty, dr("district_board_chi"))).Trim, _
                    CStr(IIf(IsDBNull(dr("district_board_shortname_SD")), String.Empty, dr("district_board_shortname_SD"))).Trim, _
                    CStr(IIf(IsDBNull(dr("area_name")), String.Empty, dr("area_name"))).Trim, _
                    CStr(IIf(IsDBNull(dr("area_name_chi")), String.Empty, dr("area_name_chi"))).Trim, _
                    CStr(IIf(IsDBNull(dr("area_code")), String.Empty, dr("area_code"))).Trim, _
                    CStr(IIf(IsDBNull(dr("EForm_Input_Avail")), String.Empty, dr("EForm_Input_Avail"))).Trim, _
                    CStr(IIf(IsDBNull(dr("BO_Input_Avail")), String.Empty, dr("BO_Input_Avail"))).Trim, _
                    CStr(IIf(IsDBNull(dr("SD_Input_Avail")), String.Empty, dr("SD_Input_Avail"))).Trim)


                udtDistrictBoardList.Add(udtDistrictBoard)
            Next

            CacheHandler.InsertCache("HCSDDistrictBoard", udtDistrictBoardList)

            Return udtDistrictBoardList
        End If
    End Function

    'Scheme
    Public Shared Function GetSDSchemeXML() As DataTable
        Dim dtSDScheme As DataTable = HttpContext.Current.Cache(SessionHelper.SDSchemeDT)

        If IsNothing(dtSDScheme) Then
            dtSDScheme = New DataTable
            dtSDScheme = XmlStringToDataTable("Scheme")
            CacheHandler.InsertCache(SessionHelper.SDSchemeDT, dtSDScheme)
        End If
        Return dtSDScheme.Copy
    End Function

    'Subsidize Group
    Public Shared Function GetSDSubsidizeGroupXML() As DataTable
        Dim dtSDSubsidizeGroup As DataTable = HttpContext.Current.Cache(SessionHelper.SDSubsidizeGroupDT + "1")

        If IsNothing(dtSDSubsidizeGroup) Then
            dtSDSubsidizeGroup = New DataTable
            dtSDSubsidizeGroup = XmlStringToDataTable("SubsidizeGroup")

            CacheHandler.InsertCache(SessionHelper.SDSubsidizeGroupDT, dtSDSubsidizeGroup)

        End If

        Return dtSDSubsidizeGroup.Copy

    End Function

    'Eligible Scheme
    Public Shared Function GetEligibleSchemeXML() As DataTable
        Dim dtEligibleScheme = New DataTable()
        dtEligibleScheme = XmlStringToDataTable("EligibleScheme")
        Return dtEligibleScheme.Copy
    End Function

    'Point to Note
    Public Shared Function GetPointToNoteXML() As DataTable
        Dim lstPointToNote As List(Of PointToNoteList) = New List(Of PointToNoteList)

        Dim dtRemarks As DataTable = New DataTable
        dtRemarks = XmlStringToDataTable("PointToNote")
        Return dtRemarks
    End Function
    'System Message
    Public Shared Function GetSystemMessageXML(ByVal strLang As String) As DataTable
        Dim dt As DataTable
        Select Case strLang.ToLower()
            Case CultureLanguage.TradChinese
                dt = XmlStringToDataTable("SystemMessage_zh_HK")
            Case Else
                dt = XmlStringToDataTable("SystemMessage")
        End Select
        Return dt
    End Function

    'Account Enquiry
    Public Shared Function LoadEHSAccountByIdentity(ByVal strIdentityNum As String, ByVal strDocCode As String, Optional ByVal udtDB As Database = Nothing) As EHSAccountModel
        Dim udtEHSAccountModel As EHSAccountModel = Nothing

        If udtDB Is Nothing Then udtDB = New Database()
        Dim ds As New DataSet()

        strIdentityNum = mainFormatter.formatDocumentIdentityNumber(strDocCode, strIdentityNum)

        ds = XmlStringToDataSet("VoucherAccount")
        'DataSetToXMLString(ds)
        udtEHSAccountModel = FillVoucherAccountInformation(ds)

        Return udtEHSAccountModel
    End Function

    'Fill Voucher AccountInformation (Return Account Information)
    Private Shared Function FillVoucherAccountInformation(ByRef dsEHSAccount As DataSet) As EHSAccountModel

        Dim udtEHSAccountModel As EHSAccountModel = Nothing

        ' Two DataTable : 1 VoucherAccount, 2 PersonalInformation
        If dsEHSAccount.Tables.Count > 0 Then

            If dsEHSAccount.Tables(0).Rows.Count > 0 Then
                ' VoucherAccount
                Dim dr As DataRow = dsEHSAccount.Tables(0).Rows(0)

                Dim dtmEffectiveDtm As Nullable(Of DateTime) = Nothing
                Dim dtmTerminateDtm As Nullable(Of DateTime) = Nothing
                Dim strDataEntryBy As String = Nothing
                Dim strRemark As String = Nothing
                Dim strPublicEnqStatusRemark As String = Nothing

                Dim strSPID As String = Nothing

                If Not dr.IsNull("Effective_Dtm") And Not String.IsNullOrEmpty(dr("Effective_Dtm")) Then
                    dtmEffectiveDtm = Convert.ToDateTime(dr("Effective_Dtm"))
                End If

                If Not dr.IsNull("Terminate_Dtm") And Not String.IsNullOrEmpty(dr("Terminate_Dtm")) Then
                    dtmTerminateDtm = Convert.ToDateTime(dr("Terminate_Dtm"))
                End If

                If Not dr.IsNull("Remark") Then
                    strRemark = dr("Remark").ToString().Trim()
                End If
                If Not dr.IsNull("Public_Enq_Status_Remark") Then
                    strPublicEnqStatusRemark = dr("Public_Enq_Status_Remark").ToString().Trim()
                End If
                If Not dr.IsNull("DataEntry_By") Then
                    strDataEntryBy = dr("DataEntry_By").ToString().Trim()
                End If

                'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim strDeceased As String = String.Empty
                If Not dr.IsNull("Deceased") Then
                    strDeceased = dr("Deceased").ToString().Trim()
                End If
                'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                udtEHSAccountModel = New EHSAccountModel( _
                    dr("Voucher_Acc_ID").ToString(), _
                    dr("Scheme_Code").ToString(), _
                    dr("Record_Status").ToString(), _
                    strRemark, _
                    dr("Public_Enquiry_Status").ToString(), _
                    strPublicEnqStatusRemark, _
                    dtmEffectiveDtm, _
                    dtmTerminateDtm, _
                    Convert.ToDateTime(dr("Create_Dtm")), _
                    dr("Create_By").ToString(), _
                    Convert.ToDateTime(dr("Update_Dtm")), _
                    dr("Update_By").ToString(), _
                    strDataEntryBy, _
                    CType(Nothing, Byte()), _
                    dr("SP_ID").ToString(), _
                    CInt(dr("SP_Practice_Display_Seq")), _
                    strDeceased)
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]
            End If

            If dsEHSAccount.Tables.Count > 1 Then

                'PersonalInformation
                For Each dr As DataRow In dsEHSAccount.Tables(1).Rows

                    Dim dtmDateOfIssue As Nullable(Of DateTime) = Nothing
                    If Not dr.IsNull("Date_of_Issue") Then
                        dtmDateOfIssue = Convert.ToDateTime(dr("Date_of_Issue"))
                    End If

                    Dim strDataEntryBy As String = Nothing
                    If Not dr.IsNull("DataEntry_By") Then
                        strDataEntryBy = dr("DataEntry_By").ToString().Trim()
                    End If

                    Dim strSurName As String = String.Empty
                    Dim strFirstName As String = String.Empty
                    Dim strEName As String = dr("Eng_Name").ToString().Trim()

                    Dim udtFormater As New Format.Formatter()
                    udtFormater.seperateEName(strEName, strSurName, strFirstName)


                    Dim strCName As String = Nothing
                    Dim strCCCode1 As String = Nothing
                    Dim strCCCode2 As String = Nothing
                    Dim strCCCode3 As String = Nothing
                    Dim strCCCode4 As String = Nothing
                    Dim strCCCode5 As String = Nothing
                    Dim strCCCode6 As String = Nothing

                    ' Handle CCCode Not Order Property Problem
                    HandleCCCode(dr, strCCCode1, strCCCode2, strCCCode3, strCCCode4, strCCCode5, strCCCode6, strCName)

                    If strCName Is Nothing Then
                        If Not dr.IsNull("Chi_Name") Then
                            strCName = dr("Chi_Name").ToString().Trim()
                        Else
                            strCName = String.Empty
                        End If
                    End If

                    Dim strECSerialNo As String = String.Empty
                    Dim strECReferenceNo As String = Nothing
                    Dim intECAge As Nullable(Of Integer) = Nothing
                    Dim dtmECDateofRegistration As Nullable(Of DateTime)
                    Dim dtmPermitToRemainUntil As Nullable(Of DateTime)
                    Dim strOtherInfo As String = Nothing
                    Dim blnECSerialNoNotProvided As Boolean = False
                    Dim blnECReferenceNoOtherFormat As Boolean = False

                    If Not dr.IsNull("EC_Serial_No") Then
                        strECSerialNo = dr("EC_Serial_No").ToString().Trim()
                    End If
                    If Not dr.IsNull("EC_Reference_No") Then
                        strECReferenceNo = dr("EC_Reference_No").ToString().Trim()
                    End If
                    If Not dr.IsNull("EC_Age") Then
                        intECAge = CInt(dr("EC_Age"))
                    End If
                    If Not dr.IsNull("EC_Date_of_Registration") Then
                        dtmECDateofRegistration = Convert.ToDateTime(dr("EC_Date_of_Registration"))
                    End If
                    If Not dr.IsNull("Permit_To_Remain_Until") Then
                        dtmPermitToRemainUntil = Convert.ToDateTime(dr("Permit_To_Remain_Until"))
                    End If

                    If Not dr.IsNull("Other_Info") Then
                        strOtherInfo = dr("Other_Info").ToString().Trim()
                    End If

                    Dim strAdoptionPrefixNum As String = String.Empty
                    If Not dr.IsNull("AdoptionPrefixNum") Then
                        strAdoptionPrefixNum = dr("AdoptionPrefixNum").ToString().Trim()
                    End If

                    If dr.IsNull("EC_Serial_No") Then
                        blnECSerialNoNotProvided = True
                    End If

                    If Not dr.IsNull("EC_Reference_No_Other_Format") AndAlso CStr(dr("EC_Reference_No_Other_Format")).Trim = "Y" Then
                        blnECReferenceNoOtherFormat = True
                    End If

                    'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Dim strDeceased As String = String.Empty
                    If Not dr.IsNull("Deceased") Then
                        strDeceased = dr("Deceased").ToString().Trim()
                    End If
                    'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                    Dim dtmDOD As Nullable(Of DateTime)
                    If Not dr.IsNull("DOD") Then
                        dtmDOD = Convert.ToDateTime(dr("DOD"))
                    End If

                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    Dim strSmartIDVer As String = String.Empty
                    If Not dr.IsNull("SmartID_Ver") Then
                        strSmartIDVer = dr("SmartID_Ver").ToString().Trim()
                    End If
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    ' Add [SmartID_Ver]
                    udtEHSAccountModel.AddPersonalInformation( _
                    dr("Voucher_Acc_ID").ToString(), _
                    Convert.ToDateTime(dr("DOB")), _
                    dr("Exact_DOB").ToString(), _
                    dr("Sex").ToString(), _
                    dtmDateOfIssue, _
                    dr("Create_By_SmartID").ToString(), _
                    dr("Record_Status").ToString(), _
                    Convert.ToDateTime(dr("Create_Dtm")), _
                    dr("Create_By").ToString(), _
                    Convert.ToDateTime(dr("Update_Dtm")), _
                    dr("Update_By").ToString(), _
                    strDataEntryBy, _
                    dr("IdentityNum").ToString(), _
                    strSurName, _
                    strFirstName, _
                    strCName, _
                    strCCCode1, _
                    strCCCode2, _
                    strCCCode3, _
                    strCCCode4, _
                    strCCCode5, _
                    strCCCode6, _
                    CType(Nothing, Byte()), _
                    strECSerialNo, _
                    strECReferenceNo, _
                    intECAge, _
                    dtmECDateofRegistration, _
                    dr("Doc_Code").ToString().Trim(), _
                    dr("Foreign_Passport_No").ToString(), _
                    dtmPermitToRemainUntil, _
                    strAdoptionPrefixNum, _
                    strOtherInfo, _
                    blnECSerialNoNotProvided, _
                    blnECReferenceNoOtherFormat, _
                    strDeceased, _
                    dtmDOD,
                    dr("Exact_DOD").ToString(), _
                    strSmartIDVer)
                    ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]
                Next
            End If
        End If

        Return udtEHSAccountModel

    End Function
    Private Shared Sub HandleCCCode(ByRef dr As DataRow, ByRef strCCCode1 As String, ByRef strCCCode2 As String, ByRef strCCCode3 As String, _
            ByRef strCCCode4 As String, ByRef strCCCode5 As String, ByRef strCCCode6 As String, ByRef strCName As String)

        Dim intSetIndex As Integer = 1

        ' Handle CCCode Not Order Property Problem

        For i As Integer = 1 To 6
            If Not dr.IsNull("CCcode" + i.ToString()) AndAlso dr("CCcode" + i.ToString()).ToString().Trim() <> "" Then

                Select Case intSetIndex
                    Case 1
                        strCCCode1 = dr("CCcode" + i.ToString()).ToString().Trim()
                    Case 2
                        strCCCode2 = dr("CCcode" + i.ToString()).ToString().Trim()
                    Case 3
                        strCCCode3 = dr("CCcode" + i.ToString()).ToString().Trim()
                    Case 4
                        strCCCode4 = dr("CCcode" + i.ToString()).ToString().Trim()
                    Case 5
                        strCCCode5 = dr("CCcode" + i.ToString()).ToString().Trim()
                    Case 6
                        strCCCode6 = dr("CCcode" + i.ToString()).ToString().Trim()
                End Select

                intSetIndex = intSetIndex + 1

                ' CRE15-014 HA_MingLiu UTF32 - Remove CCValue [Start]

                '' CCCode contain value
                'If Not dr.IsNull("CCValue" + i.ToString()) Then

                '    ' CCCode contain value and look up the chinese name
                '    If strCName Is Nothing Then
                '        strCName = dr("CCValue" + i.ToString()).ToString().Trim()
                '    Else
                '        strCName = strCName + dr("CCValue" + i.ToString()).ToString().Trim()
                '    End If
                'Else
                '    ' CCCode contain value but cannot look up the chinese name
                '    If strCName Is Nothing Then
                '        strCName = "  "
                '    Else
                '        strCName = strCName + "  "
                '    End If
                'End If

                ' CRE15-014 HA_MingLiu UTF32 - Remove CCValue [End]

            End If
        Next
    End Sub
End Class
