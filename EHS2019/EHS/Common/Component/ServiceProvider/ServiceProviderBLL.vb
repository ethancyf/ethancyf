Imports Common.Component.Address
Imports Common.Component.BankAcct
Imports Common.Component.MedicalOrganization
Imports Common.Component.Practice
Imports Common.Component.Professional
Imports Common.Component.SchemeInformation
Imports Common.Component.ServiceProvider
Imports Common.Component.ERNProcessed
Imports Common.DataAccess
Imports System.Data
Imports System.Data.SqlClient
Imports Common.ComFunction
Imports Common.ComFunction.AccountSecurity

Namespace Component.ServiceProvider
    Public Class ServiceProviderBLL

        Public Const SESS_SP As String = "ServiceProvider"

        Public Function GetSP() As ServiceProviderModel
            Dim udtSP As ServiceProviderModel
            udtSP = Nothing
            If Not HttpContext.Current.Session(SESS_SP) Is Nothing Then
                Try
                    udtSP = CType(HttpContext.Current.Session(SESS_SP), ServiceProviderModel)
                Catch ex As Exception
                    Throw New Exception("Invalid Session Service Provider!")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
            Return udtSP
        End Function

        Public Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_SP) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ClearSession()
            HttpContext.Current.Session(SESS_SP) = Nothing
        End Sub


        Public Sub SaveToSession(ByRef udtSP As ServiceProviderModel)
            HttpContext.Current.Session(SESS_SP) = udtSP
        End Sub

        Public Sub Clone(ByRef udtNewSP As ServiceProviderModel, ByRef udtOldSP As ServiceProviderModel)
            udtNewSP.EnrolRefNo = udtOldSP.EnrolRefNo
            udtNewSP.EnrolDate = udtOldSP.EnrolDate
            udtNewSP.SPID = udtOldSP.SPID
            udtNewSP.AliasAccount = udtOldSP.AliasAccount
            udtNewSP.HKID = udtOldSP.HKID
            udtNewSP.EnglishName = udtOldSP.EnglishName
            udtNewSP.ChineseName = udtOldSP.ChineseName
            udtNewSP.SpAddress = udtOldSP.SpAddress
            udtNewSP.Phone = udtOldSP.Phone
            udtNewSP.Fax = udtOldSP.Fax
            udtNewSP.Email = udtOldSP.Email
            udtNewSP.TentativeEmail = udtOldSP.TentativeEmail
            udtNewSP.EmailChanged = udtOldSP.EmailChanged
            udtNewSP.RecordStatus = udtOldSP.RecordStatus
            udtNewSP.Remark = udtOldSP.Remark
            udtNewSP.SubmitMethod = udtOldSP.SubmitMethod
            udtNewSP.AlreadyJoinEHR = udtOldSP.AlreadyJoinEHR
            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' ==========================================================
            udtNewSP.JoinPCD = udtOldSP.JoinPCD
            ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
            udtNewSP.UnderModification = udtOldSP.UnderModification
            udtNewSP.ApplicationPrinted = udtOldSP.ApplicationPrinted
            udtNewSP.CreateDtm = udtOldSP.CreateDtm
            udtNewSP.CreateBy = udtOldSP.CreateBy
            udtNewSP.UpdateDtm = udtOldSP.UpdateDtm
            udtNewSP.UpdateBy = udtOldSP.UpdateBy
            udtNewSP.TokenReturnDtm = udtOldSP.TokenReturnDtm
            udtNewSP.TSMP = udtOldSP.TSMP

            udtNewSP.PracticeList = udtOldSP.PracticeList
            udtNewSP.SchemeInfoList = udtOldSP.SchemeInfoList

        End Sub

        Public Function GetServiceProviderParticulasEnrolmentByERN(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderModel
            Dim dt As New DataTable
            Dim udtSP As ServiceProviderModel = Nothing

            Try
                Dim prams() As SqlParameter = { _
                    udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProviderEnrolment_get_byERN", prams, dt)

                If Not IsNothing(dt) Then
                    If dt.Rows.Count > 0 Then
                        For Each row As DataRow In dt.Rows

                            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                            ' ==========================================================
                            ' Add [Join_PCD]
                            udtSP = New ServiceProviderModel(CType(row.Item("Enrolment_Ref_No"), String).Trim, _
                                                                                    CType(row.Item("Enrolment_Dtm"), DateTime), _
                                                                                    String.Empty, _
                                                                                    String.Empty, _
                                                                                    CType(row.Item("SP_HKID"), String).Trim, _
                                                                                    CType(row.Item("SP_Eng_Name"), String).Trim, _
                                                                                    CStr(IIf((row.Item("SP_Chi_Name") Is DBNull.Value), String.Empty, row.Item("SP_Chi_Name"))), _
                                                                                    New AddressModel(CStr(IIf((row.Item("Room") Is DBNull.Value), String.Empty, row.Item("Room"))), _
                                                                                                    CStr(IIf((row.Item("Floor") Is DBNull.Value), String.Empty, row.Item("Floor"))), _
                                                                                                    CStr(IIf((row.Item("Block") Is DBNull.Value), String.Empty, row.Item("Block"))), _
                                                                                                    CStr(IIf((row.Item("Building") Is DBNull.Value), String.Empty, row.Item("Building"))), _
                                                                                                    CStr(IIf((row.Item("Building_Chi") Is DBNull.Value), String.Empty, row.Item("Building_Chi"))), _
                                                                                                    CStr(IIf((row.Item("District") Is DBNull.Value), String.Empty, row.Item("District"))), _
                                                                                                    Nothing), _
                                                                                    CType(row.Item("Phone_Daytime"), String).Trim, _
                                                                                    CStr(IIf((row.Item("Fax") Is DBNull.Value), String.Empty, row.Item("Fax"))), _
                                                                                    CType(row.Item("Email"), String).Trim, _
                                                                                    String.Empty,
                                                                                    String.Empty, _
                                                                                    String.Empty, _
                                                                                    String.Empty, _
                                                                                    String.Empty, _
                                                                                    CType(row.Item("Already_Joined_EHR"), String), _
                                                                                    CType(row.Item("Join_PCD"), String), _
                                                                                    String.Empty, _
                                                                                    CStr(IIf((row.Item("Application_Printed") Is DBNull.Value), String.Empty, row.Item("Application_Printed"))), _
                                                                                    Nothing, _
                                                                                    String.Empty, _
                                                                                    Nothing, _
                                                                                    String.Empty, _
                                                                                    Nothing, _
                                                                                    Nothing, _
                                                                                    Nothing, _
                                                                                    Nothing, _
                                                                                    Nothing, _
                                                                                    Nothing, _
                                                                                    "", _
                                                                                    Nothing, _
                                                                                    String.Empty,
                                                                                    String.Empty,
                                                                                    String.Empty,
                                                                                    Nothing)
                            ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
                        Next
                    End If

                End If

            Catch ex As Exception
                Throw
            End Try


            Return udtSP
        End Function

        Public Function GetServiceProviderSchemeInfoEnrolmentByERN(ByVal strERN As String, ByRef udtDB As Database) As DataTable
            Dim dt As New DataTable

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProviderSchemeInfo_get_byERN", prams, dt)

                If dt.Rows.Count = 0 Then
                    dt = Nothing
                End If
            Catch ex As Exception
                Throw
            End Try

            Return dt
        End Function

        Public Function GetServiceProviderHKIDByERNOrSPID(ByVal strERN As String, ByVal strSPID As String, ByRef udtDB As Database) As DataTable
            Dim dt As New DataTable

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, IIf(strERN.Trim.Equals(String.Empty), DBNull.Value, strERN.Trim)), _
                                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(strSPID.Trim.Equals(String.Empty), DBNull.Value, strSPID.Trim))}
                udtDB.RunProc("proc_ServiceProviderHKIC_get_byERNSPID", prams, dt)

                If dt.Rows.Count = 0 Then
                    dt = Nothing
                End If
            Catch ex As Exception
                Throw
            End Try

            Return dt
        End Function

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function GetServiceProviderEnrolmentProfileByERNInHCVU(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderModel
            Return GetServiceProviderCopyProfileByERNInHCVU(strERN, EnumEnrolCopy.Enrolment, udtDB)
        End Function
        ' CRE12-001 eHS and PCD integration [End][Koala]

        Public Function GetServiceProviderCopyProfileByERNInHCVU(ByVal strERN As String, ByVal enumEnrolCopy As EnumEnrolCopy, ByVal udtDB As Database) As ServiceProviderModel
            ' Dim drSP As SqlDataReader = Nothing
            Dim udtSP As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)

            Try
                Dim dtSP As New DataTable

                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                Select Case enumEnrolCopy
                    Case enumEnrolCopy.Enrolment
                        udtDB.RunProc("proc_ServiceProviderEnrolment_get_byERN", prams, dtSP)
                    Case enumEnrolCopy.Original
                        udtDB.RunProc("proc_ServiceProviderOriginal_get_byERN", prams, dtSP)
                End Select
                ' CRE12-001 eHS and PCD integration [End][Koala]

                If dtSP.Rows.Count = 1 Then
                    Dim drSP As DataRow = dtSP.Rows(0)

                    If IsDBNull(drSP.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drSP.Item("Address_Code")))
                    End If

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                    ' ==========================================================
                    ' Add [Join_PCD]
                    udtSP = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
                                                        CType(drSP.Item("Enrolment_Dtm"), DateTime), _
                                                        String.Empty, _
                                                        String.Empty, _
                                                        CType(drSP.Item("SP_HKID"), String).Trim, _
                                                        CType(drSP.Item("SP_Eng_Name"), String).Trim, _
                                                        CStr(IIf((drSP.Item("SP_Chi_Name") Is DBNull.Value), String.Empty, drSP.Item("SP_Chi_Name"))), _
                                                        New AddressModel(CStr(IIf((drSP.Item("Room") Is DBNull.Value), String.Empty, drSP.Item("Room"))), _
                                                            CStr(IIf((drSP.Item("Floor") Is DBNull.Value), String.Empty, drSP.Item("Floor"))), _
                                                            CStr(IIf((drSP.Item("Block") Is DBNull.Value), String.Empty, drSP.Item("Block"))), _
                                                            CStr(IIf((drSP.Item("Building") Is DBNull.Value), String.Empty, drSP.Item("Building"))), _
                                                            CStr(IIf((drSP.Item("Building_Chi") Is DBNull.Value), String.Empty, drSP.Item("Building_Chi"))), _
                                                            CStr(IIf((drSP.Item("District") Is DBNull.Value), String.Empty, drSP.Item("District"))), _
                                                            intAddressCode), _
                                                        CType(drSP.Item("Phone_Daytime"), String).Trim, _
                                                        CStr(IIf((drSP.Item("Fax") Is DBNull.Value), String.Empty, drSP.Item("Fax"))), _
                                                        CType(drSP.Item("Email"), String).Trim, _
                                                        String.Empty,
                                                        String.Empty, _
                                                        String.Empty, _
                                                        String.Empty, _
                                                        SubmitChannel.Electronic, _
                                                        CType(drSP.Item("Already_Joined_EHR"), String), _
                                                        CType(drSP.Item("Join_PCD"), String), _
                                                        String.Empty, _
                                                        CStr(IIf((drSP.Item("Application_Printed") Is DBNull.Value), String.Empty, drSP.Item("Application_Printed"))), _
                                                        Nothing, _
                                                        String.Empty, _
                                                        Nothing, _
                                                        String.Empty, _
                                                        Nothing, _
                                                        Nothing, _
                                                        Nothing, _
                                                        Nothing, _
                                                        Nothing, _
                                                        Nothing, _
                                                        "", _
                                                        Nothing, _
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        Nothing)
                    ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

                    ' Get Scheme Information
                    Dim udtSchemeInformationBLL As New SchemeInformationBLL
                    udtSP.SchemeInfoList = udtSchemeInformationBLL.GetSchemeInfoListCopyInHCVU(udtSP.EnrolRefNo, enumEnrolCopy, udtDB)

                    ' Get Medical Organization
                    Dim udtMedicalOrganizationBLL As New MedicalOrganizationBLL
                    udtSP.MOList = udtMedicalOrganizationBLL.GetMOListFromCopyByERN(udtSP.EnrolRefNo, enumEnrolCopy, udtDB)

                    ' Get Practice, Bank, Practice Scheme Information
                    Dim udtPracticeBLL As New PracticeBLL
                    udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromCopyByERNInHCVU(udtSP.EnrolRefNo, enumEnrolCopy, udtDB)

                    ' CRE12-001 eHS and PCD integration [Start][Koala]
                    ' -----------------------------------------------------------------------------------------
                    ' Get Third Party Additional FIeld Enrollment Information
                    udtSP.ThirdPartyAdditionalFieldEnrolmentList = ThirdParty.ThirdPartyBLL.GetThirdPartyAdditionalFieldCopyByERN(udtSP.EnrolRefNo, enumEnrolCopy, udtDB)
                    ' CRE12-001 eHS and PCD integration [End][Koala]
                Else
                    udtSP = Nothing

                End If


                Return udtSP

            Catch ex As Exception
                Throw

            End Try
        End Function

        Public Function GetServiceProviderEnrolmentProfileByERN(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderModel
            ' Dim drSP As SqlDataReader = Nothing
            Dim udtSP As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)

            Try
                Dim dtSP As New DataTable

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProviderEnrolment_get_byERN", prams, dtSP)

                If dtSP.Rows.Count = 1 Then
                    Dim drSP As DataRow = dtSP.Rows(0)

                    If IsDBNull(drSP.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drSP.Item("Address_Code")))
                    End If

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                    ' ==========================================================
                    ' Add [Join_PCD]
                    udtSP = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
                                                        CType(drSP.Item("Enrolment_Dtm"), DateTime), _
                                                        String.Empty, _
                                                        String.Empty, _
                                                        CType(drSP.Item("SP_HKID"), String).Trim, _
                                                        CType(drSP.Item("SP_Eng_Name"), String).Trim, _
                                                        CStr(IIf((drSP.Item("SP_Chi_Name") Is DBNull.Value), String.Empty, drSP.Item("SP_Chi_Name"))), _
                                                        New AddressModel(CStr(IIf((drSP.Item("Room") Is DBNull.Value), String.Empty, drSP.Item("Room"))), _
                                                            CStr(IIf((drSP.Item("Floor") Is DBNull.Value), String.Empty, drSP.Item("Floor"))), _
                                                            CStr(IIf((drSP.Item("Block") Is DBNull.Value), String.Empty, drSP.Item("Block"))), _
                                                            CStr(IIf((drSP.Item("Building") Is DBNull.Value), String.Empty, drSP.Item("Building"))), _
                                                            CStr(IIf((drSP.Item("Building_Chi") Is DBNull.Value), String.Empty, drSP.Item("Building_Chi"))), _
                                                            CStr(IIf((drSP.Item("District") Is DBNull.Value), String.Empty, drSP.Item("District"))), _
                                                            intAddressCode), _
                                                        CType(drSP.Item("Phone_Daytime"), String).Trim, _
                                                        CStr(IIf((drSP.Item("Fax") Is DBNull.Value), String.Empty, drSP.Item("Fax"))), _
                                                        CType(drSP.Item("Email"), String).Trim, _
                                                        String.Empty,
                                                        String.Empty, _
                                                        String.Empty, _
                                                        String.Empty, _
                                                        SubmitChannel.Electronic, _
                                                        CType(drSP.Item("Already_Joined_EHR"), String), _
                                                        CType(drSP.Item("Join_PCD"), String), _
                                                        String.Empty, _
                                                        CStr(IIf((drSP.Item("Application_Printed") Is DBNull.Value), String.Empty, drSP.Item("Application_Printed"))), _
                                                        Nothing, _
                                                        String.Empty, _
                                                        Nothing, _
                                                        String.Empty, _
                                                        Nothing, _
                                                        Nothing, _
                                                        Nothing, _
                                                        Nothing, _
                                                        Nothing, _
                                                        Nothing, _
                                                        "", _
                                                        Nothing, _
                                                        String.Empty,
                                                        String.Empty,
                                                        String.Empty,
                                                        Nothing)
                    ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

                    ' Get Scheme Information
                    Dim udtSchemeInformationBLL As New SchemeInformationBLL
                    udtSP.SchemeInfoList = udtSchemeInformationBLL.GetSchemeInfoListEnrolment(udtSP.EnrolRefNo, udtDB)

                    ' Get Medical Organization
                    Dim udtMedicalOrganizationBLL As New MedicalOrganizationBLL
                    udtSP.MOList = udtMedicalOrganizationBLL.GetMOListFromEnrolmentByERN(udtSP.EnrolRefNo, udtDB)

                    ' Get Practice, Bank, Practice Scheme Information
                    Dim udtPracticeBLL As New PracticeBLL
                    udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromEnrolmentByERN(udtSP.EnrolRefNo, udtDB)
                Else
                    udtSP = Nothing

                End If


                Return udtSP

            Catch ex As Exception
                Throw

            End Try
        End Function


        Public Function GetServiceProviderStagingProfileByERNSPIDSPHKID(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderModel
            'Dim drSP As SqlDataReader = Nothing
            Dim udtSP As ServiceProviderModel = Nothing

            Try
                Dim dtSP As New DataTable

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProviderStaging_get_byERNSPIDSPHKID", prams, dtSP)

                'udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                '                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strSPHKID), _
                '                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, strSPEngName)

                Dim drSP As DataRow = dtSP.Rows(0)

                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' --------------------------------------------------------------------------------------------------------------------------------
                ' Add [Join_PCD]
                udtSP = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
                                                    CType(drSP.Item("Enrolment_Dtm"), DateTime), _
                                                    CStr(IIf((drSP.Item("SP_ID") Is DBNull.Value), String.Empty, drSP.Item("SP_ID"))), _
                                                    String.Empty, _
                                                    CType(drSP.Item("SP_HKID"), String).Trim, _
                                                    CType(drSP.Item("SP_Eng_Name"), String).Trim, _
                                                    CStr(IIf((drSP.Item("SP_Chi_Name") Is DBNull.Value), String.Empty, drSP.Item("SP_Chi_Name"))), _
                                                    New AddressModel(CStr(IIf((drSP.Item("Room") Is DBNull.Value), String.Empty, drSP.Item("Room"))), _
                                                        CStr(IIf((drSP.Item("Floor") Is DBNull.Value), String.Empty, drSP.Item("Floor"))), _
                                                        CStr(IIf((drSP.Item("Block") Is DBNull.Value), String.Empty, drSP.Item("Block"))), _
                                                        CStr(IIf((drSP.Item("Building") Is DBNull.Value), String.Empty, drSP.Item("Building"))), _
                                                        CStr(IIf((drSP.Item("Building_Chi") Is DBNull.Value), String.Empty, drSP.Item("Building_Chi"))), _
                                                        CStr(IIf((drSP.Item("District") Is DBNull.Value), String.Empty, drSP.Item("District"))), _
                                                        CInt(IIf((drSP.Item("Address_Code") Is DBNull.Value), Nothing, drSP.Item("Address_Code")))), _
                                                    CType(drSP.Item("Phone_Daytime"), String).Trim, _
                                                    CStr(IIf((drSP.Item("Fax") Is DBNull.Value), String.Empty, drSP.Item("Fax"))), _
                                                    CType(drSP.Item("Email"), String).Trim, _
                                                    String.Empty,
                                                    String.Empty, _
                                                    String.Empty, _
                                                    String.Empty, _
                                                    String.Empty, _
                                                    CType(drSP.Item("Already_Joined_EHR"), String), _
                                                    CType(drSP.Item("Join_PCD"), String), _
                                                    String.Empty, _
                                                    CStr(IIf((drSP.Item("Application_Printed") Is DBNull.Value), String.Empty, drSP.Item("Application_Printed"))), _
                                                    Nothing, _
                                                    String.Empty, _
                                                    Nothing, _
                                                    String.Empty, _
                                                    Nothing, _
                                                    Nothing, _
                                                    Nothing, _
                                                    Nothing, _
                                                    Nothing, _
                                                    Nothing, _
                                                    "", _
                                                    Nothing, _
                                                    String.Empty,
                                                    String.Empty,
                                                    String.Empty,
                                                    Nothing)
                ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

                ' Get Scheme Information
                Dim udtSchemeInformationBLL As New SchemeInformationBLL
                udtSP.SchemeInfoList = udtSchemeInformationBLL.GetSchemeInfoListStaging(udtSP.EnrolRefNo, udtDB)

                ' Get Medical Organization
                Dim udtMOBLL As New MedicalOrganization.MedicalOrganizationBLL
                udtSP.MOList = udtMOBLL.GetMOListFromStagingByERN(udtSP.EnrolRefNo, udtDB)

                ' Get Practice, Bank, Practice Scheme Information
                Dim udtPracticeBLL As New PracticeBLL
                udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERNBankStagingStatus(udtSP.EnrolRefNo, udtDB)

                ' Get ERN Processed
                Dim udtERNProcessedBLL As New ERNProcessedBLL
                udtSP.ERNProcessedList = udtERNProcessedBLL.GetERNProcessedListStagingByERN(udtSP.EnrolRefNo, udtDB)

                Return udtSP

            Catch ex As Exception
                Throw

            End Try

        End Function

        Public Function CheckServiceProviderPracticeEnroledExternalUpload(ByRef udtDB As Database, ByVal strSPID As String, ByVal intPracticeDisplaySeq As Integer, ByVal strSystemName As String) As Boolean

            Dim udtGeneralFunction As New ComFunction.GeneralFunction
            Dim strTurnOnInterfaceSPPractifceCheck As String = String.Empty

            udtGeneralFunction.getSystemParameter("TurnOnInterfaceSPPracticeCheck", strTurnOnInterfaceSPPractifceCheck, String.Empty)

            If strTurnOnInterfaceSPPractifceCheck = "Y" Then
                Dim blnIsExist As Boolean = False

                Dim procName As String = "proc_InterfaceSPPractice_get_bySPIDPractice"

                Dim dtRaw As New DataTable()

                Try
                    Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                                                    udtDB.MakeInParam("@Practice_Display_Seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, intPracticeDisplaySeq), _
                                                    udtDB.MakeInParam("@System_Name", SqlDbType.Char, 20, strSystemName) _
                                                  }

                    udtDB.RunProc(procName, prams, dtRaw)

                    If dtRaw.Rows.Count > 0 Then
                        blnIsExist = True
                    End If

                    Return blnIsExist

                Catch ex As Exception
                    Throw

                End Try
            Else
                Return True
            End If
        End Function

        Public Function GetServiceProviderBySPID(ByRef udtDB As Database, ByVal strSPID As String, Optional ByVal blnPracticeSchemeInfoByServiceDate As Boolean = False, Optional ByVal dtServiceDate As DateTime = Nothing) As ServiceProviderModel
            Dim procName As String = "proc_ServiceProvider_get_bySPID"

            Dim udtSP As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmTokenReturnDtm As Nullable(Of DateTime)
            Dim strConsentPrintOption As String = String.Empty
            Dim udtGeneralFunction As New ComFunction.GeneralFunction

            Dim dtRaw As New DataTable()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc(procName, prams, dtRaw)

                If dtRaw.Rows.Count > 0 Then
                    Dim drRaw As DataRow = dtRaw.Rows(0)

                    If IsDBNull(drRaw.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drRaw.Item("Address_Code")))
                    End If

                    If IsDBNull(drRaw.Item("Effective_Dtm")) Then
                        dtmEffectiveDtm = Nothing
                    Else
                        dtmEffectiveDtm = Convert.ToDateTime(drRaw.Item("Effective_Dtm"))
                    End If

                    If IsDBNull(drRaw.Item("Token_Return_Dtm")) Then
                        dtmTokenReturnDtm = Nothing
                    Else
                        dtmTokenReturnDtm = Convert.ToDateTime(drRaw.Item("Token_Return_Dtm"))
                    End If

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                    ' --------------------------------------------------------------------------------------------------------------------------------
                    ' Add dummy [Join_PCD]
                    udtSP = New ServiceProviderModel(CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
                                                                            Nothing, _
                                                                            CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))), _
                                                                            CStr(IIf((drRaw.Item("Alias_Account") Is DBNull.Value), String.Empty, drRaw.Item("Alias_Account"))), _
                                                                            CType(drRaw.Item("SP_HKID"), String).Trim, _
                                                                            CType(drRaw.Item("SP_Eng_Name"), String).Trim, _
                                                                            CStr(IIf((drRaw.Item("SP_Chi_Name") Is DBNull.Value), String.Empty, drRaw.Item("SP_Chi_Name"))), _
                                                                            New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))), _
                                                                                            CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))), _
                                                                                            CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))), _
                                                                                            CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))), _
                                                                                            CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))), _
                                                                                            CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))), _
                                                                                            intAddressCode), _
                                                                            CType(drRaw.Item("Phone_Daytime"), String).Trim, _
                                                                            CStr(IIf((drRaw.Item("Fax") Is DBNull.Value), String.Empty, drRaw.Item("Fax"))), _
                                                                            CType(drRaw.Item("Email"), String).Trim, _
                                                                            CStr(IIf((drRaw.Item("Tentative_Email") Is DBNull.Value), String.Empty, drRaw.Item("Tentative_Email"))), _
                                                                            String.Empty, _
                                                                            CType(drRaw.Item("Record_Status"), String).Trim, _
                                                                            CStr(IIf((drRaw.Item("Remark") Is DBNull.Value), String.Empty, drRaw.Item("Remark"))), _
                                                                            CType(drRaw.Item("Submission_Method"), String).Trim, _
                                                                            CType(drRaw.Item("Already_Joined_EHR"), String), _
                                                                            JoinPCDStatus.NA,
                                                                            CStr(IIf((drRaw.Item("UnderModification") Is DBNull.Value), String.Empty, drRaw.Item("UnderModification"))), _
                                                                            CStr(IIf((drRaw.Item("Application_Printed") Is DBNull.Value), String.Empty, drRaw.Item("Application_Printed"))), _
                                                                            CType(drRaw.Item("Create_Dtm"), DateTime), _
                                                                            CType(drRaw.Item("Create_By"), String), _
                                                                            CType(drRaw.Item("Update_Dtm"), DateTime), _
                                                                            CType(drRaw.Item("Update_By"), String), _
                                                                            dtmEffectiveDtm, _
                                                                            dtmTokenReturnDtm, _
                                                                            IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, CType(drRaw.Item("TSMP"), Byte())), _
                                                                            Nothing, _
                                                                            Nothing, _
                                                                            Nothing, _
                                                                            CStr(drRaw.Item("Data_Input_By")).Trim(), _
                                                                            CDate(drRaw.Item("Data_Input_Effective_Dtm")), _
                                                                            String.Empty,
                                                                            String.Empty,
                                                                            String.Empty,
                                                                            Nothing)
                    ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

                    If drRaw.Item("ConsentPrintOption") Is DBNull.Value Then
                        udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)
                        udtSP.PrintOption = strConsentPrintOption
                    Else
                        udtSP.PrintOption = drRaw.Item("ConsentPrintOption")
                    End If
                End If

                ' Get Scheme Information
                Dim udtSchemeInfoBLL As New SchemeInformationBLL
                If blnPracticeSchemeInfoByServiceDate = False Then
                    udtSP.SchemeInfoList = udtSchemeInfoBLL.GetSchemeInfoListPermanent(udtSP.SPID, udtDB)
                Else
                    ' call by common EHSClaimmBLL validation rule
                    ' CRE15-004 TIV & QIV [Start][Lawrence]
                    udtSP.SchemeInfoList = udtSchemeInfoBLL.GetSchemeInfoListPermanent(udtSP.SPID, udtDB)
                    ' CRE15-004 TIV & QIV [End][Lawrence]
                End If


                ' Get Medical Organization
                Dim udtMOBLL As New MedicalOrganizationBLL
                udtSP.MOList = udtMOBLL.GetMOListFromPermanentBySPID(udtSP.SPID, udtDB)

                ' Get Practice, Bank, Practice Bank Information
                Dim udtPracticeBLL As New PracticeBLL
                If blnPracticeSchemeInfoByServiceDate = False Then
                    udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtSP.SPID, udtDB)
                Else
                    udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtSP.SPID, udtDB, True, dtServiceDate)
                End If

                'Get ERN Processed 
                Dim udtERNProcessedBLL As New ERNProcessedBLL
                udtSP.ERNProcessedList = udtERNProcessedBLL.GetERNProcessedListPermanentBySPID(udtSP.SPID, udtDB)

                Return udtSP

            Catch ex As Exception
                Throw ex

            End Try

        End Function

        ' CRE15-016 Randomly generate the valid claim transaction [Start][Lawrence]
        Public Function GetServiceProviderBySPID(ByVal strSPID As String, ByRef udtDB As Database) As DataTable
            Dim dt As New DataTable
            Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
            udtDB.RunProc("proc_ServiceProvider_get_bySPID_Simple", prams, dt)
            Return dt
        End Function
        ' CRE15-016 Randomly generate the valid claim transaction [End][Lawrence]

        Public Function GetServiceProviderPermanentProfileByERN(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderModel
            'Dim drSP As SqlDataReader = Nothing
            Dim udtSP As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)
            Dim dtmEnrolmentDtm As Nullable(Of DateTime)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmTokenReturnDtm As Nullable(Of DateTime)
            Dim strConsentPrintOption As String = String.Empty
            Dim udtGeneralFunction As New ComFunction.GeneralFunction

            Try
                Dim dtSP As New DataTable

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProvider_get_byERN", prams, dtSP)

                Dim drSP As DataRow = dtSP.Rows(0)

                If IsDBNull(drSP.Item("Address_Code")) Then
                    intAddressCode = Nothing
                Else
                    intAddressCode = CInt((drSP.Item("Address_Code")))
                End If

                If IsDBNull(drSP.Item("Enrolment_Dtm")) Then
                    dtmEnrolmentDtm = Nothing
                Else
                    dtmEnrolmentDtm = Convert.ToDateTime(drSP.Item("Enrolment_Dtm"))
                End If

                'If IsDBNull(drSP.Item("Effective_Dtm")) Then
                '    dtmEffectiveDtm = Nothing
                'Else
                '    dtmEffectiveDtm = Convert.ToDateTime(drSP.Item("Effective_Dtm"))
                'End If

                If IsDBNull(drSP.Item("Token_Return_Dtm")) Then
                    dtmTokenReturnDtm = Nothing
                Else
                    dtmTokenReturnDtm = Convert.ToDateTime(drSP.Item("Token_Return_Dtm"))
                End If

                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' --------------------------------------------------------------------------------------------------------------------------------
                ' Add dummy [Join_PCD]
                udtSP = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
                                                    dtmEnrolmentDtm, _
                                                    CStr(IIf((drSP.Item("SP_ID") Is DBNull.Value), String.Empty, drSP.Item("SP_ID"))), _
                                                    String.Empty, _
                                                    CType(drSP.Item("SP_HKID"), String).Trim, _
                                                    CType(drSP.Item("SP_Eng_Name"), String).Trim, _
                                                    CStr(IIf((drSP.Item("SP_Chi_Name") Is DBNull.Value), String.Empty, drSP.Item("SP_Chi_Name"))), _
                                                    New AddressModel(CStr(IIf((drSP.Item("Room") Is DBNull.Value), String.Empty, drSP.Item("Room"))), _
                                                        CStr(IIf((drSP.Item("Floor") Is DBNull.Value), String.Empty, drSP.Item("Floor"))), _
                                                        CStr(IIf((drSP.Item("Block") Is DBNull.Value), String.Empty, drSP.Item("Block"))), _
                                                        CStr(IIf((drSP.Item("Building") Is DBNull.Value), String.Empty, drSP.Item("Building"))), _
                                                        CStr(IIf((drSP.Item("Building_Chi") Is DBNull.Value), String.Empty, drSP.Item("Building_Chi"))), _
                                                        CStr(IIf((drSP.Item("District") Is DBNull.Value), String.Empty, drSP.Item("District"))), _
                                                        intAddressCode), _
                                                    CType(drSP.Item("Phone_Daytime"), String).Trim, _
                                                    CStr(IIf((drSP.Item("Fax") Is DBNull.Value), String.Empty, drSP.Item("Fax"))), _
                                                    CType(drSP.Item("Email"), String).Trim, _
                                                    String.Empty, _
                                                    String.Empty, _
                                                    CType(drSP.Item("Record_Status"), String).Trim, _
                                                    CStr(IIf((drSP.Item("Remark") Is DBNull.Value), String.Empty, drSP.Item("Remark"))), _
                                                    CType(drSP.Item("Submission_Method"), String).Trim, _
                                                    CType(drSP.Item("Already_Joined_EHR"), String), _
                                                    JoinPCDStatus.NA,
                                                    CStr(IIf((drSP.Item("UnderModification") Is DBNull.Value), String.Empty, drSP.Item("UnderModification"))), _
                                                    String.Empty, _
                                                    CType(drSP.Item("Create_Dtm"), DateTime), _
                                                    CType(drSP.Item("Create_By"), String), _
                                                    CType(drSP.Item("Update_Dtm"), DateTime), _
                                                    CType(drSP.Item("Update_By"), String), _
                                                    dtmEffectiveDtm, _
                                                    dtmTokenReturnDtm, _
                                                    CType(IIf(drSP.Item("TSMP") Is DBNull.Value, Nothing, drSP.Item("TSMP")), Byte()), _
                                                    Nothing, _
                                                    Nothing, _
                                                    Nothing, _
                                                    CStr(drSP.Item("Data_Input_By")).Trim(), _
                                                    CDate(drSP.Item("Data_Input_Effective_Dtm")), _
                                                    String.Empty,
                                                    String.Empty,
                                                    String.Empty,
                                                    Nothing)
                ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

                If drSP.Item("ConsentPrintOption") Is DBNull.Value Then
                    udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)
                    udtSP.PrintOption = strConsentPrintOption
                Else
                    udtSP.PrintOption = drSP.Item("ConsentPrintOption")
                End If

                ' Get Scheme Information
                Dim udtSchemeInfoBLL As New SchemeInformationBLL
                udtSP.SchemeInfoList = udtSchemeInfoBLL.GetSchemeInfoListPermanent(udtSP.SPID, udtDB)

                ' Get Medical Organization
                Dim udtMOBLL As New MedicalOrganizationBLL
                udtSP.MOList = udtMOBLL.GetMOListFromPermanentBySPID(udtSP.SPID, udtDB)

                ' Get Practice, Bank, Practice Bank Information
                Dim udtPracticeBLL As New PracticeBLL
                udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtSP.SPID, udtDB)

                ' Get ERN Processed
                Dim udtERNProcessedBLL As New ERNProcessedBLL
                udtSP.ERNProcessedList = udtERNProcessedBLL.GetERNProcessedListPermanentBySPID(udtSP.SPID, udtDB)

                Return udtSP

            Catch ex As Exception
                Throw

            End Try

        End Function

        Public Function GetServiceProviderPermanentProfileWithMaintenanceBySPID(ByVal strSPID As String, ByVal udtDB As Database) As ServiceProviderModel
            ' Dim drSP As SqlDataReader = Nothing
            Dim udtSP As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)
            Dim dtmEnrolmentDtm As Nullable(Of DateTime)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmTokenReturnDtm As Nullable(Of DateTime)
            ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
            Dim dtmPCDStatusLastCheckDtm As Nullable(Of DateTime)
            ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---

            Try
                Dim dtSP As New DataTable

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_ServiceProviderSPAccMaintenance_get_bySPID", prams, dtSP)

                If dtSP.Rows.Count = 1 Then
                    Dim drSP As DataRow = dtSP.Rows(0)

                    If IsDBNull(drSP.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drSP.Item("Address_Code")))
                    End If

                    If IsDBNull(drSP.Item("Enrolment_Dtm")) Then
                        dtmEnrolmentDtm = Nothing
                    Else
                        dtmEnrolmentDtm = Convert.ToDateTime(drSP.Item("Enrolment_Dtm"))
                    End If

                    If IsDBNull(drSP.Item("Effective_Dtm")) Then
                        dtmEffectiveDtm = Nothing
                    Else
                        dtmEffectiveDtm = Convert.ToDateTime(drSP.Item("Effective_Dtm"))
                    End If

                    If IsDBNull(drSP.Item("Token_Return_Dtm")) Then
                        dtmTokenReturnDtm = Nothing
                    Else
                        dtmTokenReturnDtm = Convert.ToDateTime(drSP.Item("Token_Return_Dtm"))
                    End If

                    ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
                    If IsDBNull(drSP.Item("PCD_Status_Last_Check_Dtm")) Then
                        dtmPCDStatusLastCheckDtm = Nothing
                    Else
                        dtmPCDStatusLastCheckDtm = Convert.ToDateTime(drSP.Item("PCD_Status_Last_Check_Dtm"))
                    End If
                    ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                    ' --------------------------------------------------------------------------------------------------------------------------------
                    ' Add dummy [Join_PCD]
                    udtSP = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
                                                        dtmEnrolmentDtm, _
                                                        CStr(IIf((drSP.Item("SP_ID") Is DBNull.Value), String.Empty, drSP.Item("SP_ID"))), _
                                                        String.Empty, _
                                                        CType(drSP.Item("SP_HKID"), String).Trim, _
                                                        CType(drSP.Item("SP_Eng_Name"), String).Trim, _
                                                        CStr(IIf((drSP.Item("SP_Chi_Name") Is DBNull.Value), String.Empty, drSP.Item("SP_Chi_Name"))), _
                                                        New AddressModel(CStr(IIf((drSP.Item("Room") Is DBNull.Value), String.Empty, drSP.Item("Room"))), _
                                                            CStr(IIf((drSP.Item("Floor") Is DBNull.Value), String.Empty, drSP.Item("Floor"))), _
                                                            CStr(IIf((drSP.Item("Block") Is DBNull.Value), String.Empty, drSP.Item("Block"))), _
                                                            CStr(IIf((drSP.Item("Building") Is DBNull.Value), String.Empty, drSP.Item("Building"))), _
                                                            CStr(IIf((drSP.Item("Building_Chi") Is DBNull.Value), String.Empty, drSP.Item("Building_Chi"))), _
                                                            CStr(IIf((drSP.Item("District") Is DBNull.Value), String.Empty, drSP.Item("District"))), _
                                                            intAddressCode), _
                                                        CType(drSP.Item("Phone_Daytime"), String).Trim, _
                                                        CStr(IIf((drSP.Item("Fax") Is DBNull.Value), String.Empty, drSP.Item("Fax"))), _
                                                        CType(drSP.Item("Email"), String).Trim, _
                                                        CStr(IIf((drSP.Item("Tentative_Email") Is DBNull.Value), String.Empty, drSP.Item("Tentative_Email"))), _
                                                        CType(drSP.Item("Email_Change"), String).Trim, _
                                                        CType(drSP.Item("Record_Status"), String).Trim, _
                                                        CStr(IIf((drSP.Item("Remark") Is DBNull.Value), String.Empty, drSP.Item("Remark"))), _
                                                        CType(drSP.Item("Submission_Method"), String).Trim, _
                                                        CType(drSP.Item("Already_Joined_EHR"), String), _
                                                        JoinPCDStatus.NA,
                                                        CStr(IIf((drSP.Item("UnderModification") Is DBNull.Value), String.Empty, drSP.Item("UnderModification"))), _
                                                        String.Empty, _
                                                        CType(drSP.Item("Create_Dtm"), DateTime), _
                                                        CType(drSP.Item("Create_By"), String), _
                                                        CType(drSP.Item("Update_Dtm"), DateTime), _
                                                        CType(drSP.Item("Update_By"), String), _
                                                        dtmEffectiveDtm, _
                                                        dtmTokenReturnDtm, _
                                                        CType(IIf(drSP.Item("TSMP") Is DBNull.Value, Nothing, drSP.Item("TSMP")), Byte()), _
                                                        Nothing, _
                                                        Nothing, _
                                                        Nothing, _
                                                        CStr(drSP.Item("Data_Input_By")).Trim(), _
                                                        CDate(drSP.Item("Data_Input_Effective_Dtm")), _
                                                        CStr(IIf((drSP.Item("PCD_Account_Status") Is DBNull.Value), String.Empty, drSP.Item("PCD_Account_Status"))), _
                                                        CStr(IIf((drSP.Item("PCD_Enrolment_Status") Is DBNull.Value), String.Empty, drSP.Item("PCD_Enrolment_Status"))), _
                                                        CStr(IIf((drSP.Item("PCD_Professional") Is DBNull.Value), String.Empty, drSP.Item("PCD_Professional"))), _
                                                        dtmPCDStatusLastCheckDtm)
                    ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

                    ' Get Scheme Information
                    Dim udtSchemeInfoBLL As New SchemeInformationBLL
                    udtSP.SchemeInfoList = udtSchemeInfoBLL.GetSchemeInfoListPermanent(udtSP.SPID, udtDB)

                    ' Get Medical Organization
                    Dim udtMOBLL As New MedicalOrganizationBLL
                    udtSP.MOList = udtMOBLL.GetMOListFromPermanentBySPID(udtSP.SPID, udtDB)

                    ' Get Practice, Bank, Practice Scheme Information
                    Dim udtPracticeBLL As New PracticeBLL
                    udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromPermanentMaintenanceBySPID(udtSP.SPID, udtDB)

                    ' Get ERN Processed
                    Dim udtERNProcessedBLL As New ERNProcessedBLL
                    udtSP.ERNProcessedList = udtERNProcessedBLL.GetERNProcessedListPermanentBySPID(udtSP.SPID, udtDB)
                Else
                    udtSP = Nothing
                End If

                Return udtSP

            Catch ex As Exception
                Throw

            End Try

        End Function

        Public Function GetServiceProviderStagingByERN(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderModel
            Dim udtServiceProviderModel As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)
            Dim dtmEnrolmentDtm As Nullable(Of DateTime)

            Dim dtRaw As New DataTable()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProviderStaging_get_byERN", prams, dtRaw)

                If dtRaw.Rows.Count > 0 Then
                    Dim drRaw As DataRow = dtRaw.Rows(0)

                    If IsDBNull(drRaw.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drRaw.Item("Address_Code")))
                    End If

                    If IsDBNull(drRaw.Item("Enrolment_Dtm")) Then
                        dtmEnrolmentDtm = Nothing
                    Else
                        dtmEnrolmentDtm = Convert.ToDateTime(drRaw.Item("Enrolment_Dtm"))
                    End If

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                    ' --------------------------------------------------------------------------------------------------------------------------------
                    ' Add [Join_PCD]
                    udtServiceProviderModel = New ServiceProviderModel(CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
                                                                        dtmEnrolmentDtm, _
                                                                        CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))), _
                                                                        String.Empty, _
                                                                        CType(drRaw.Item("SP_HKID"), String).Trim, _
                                                                        CType(drRaw.Item("SP_Eng_Name"), String).Trim, _
                                                                        CStr(IIf((drRaw.Item("SP_Chi_Name") Is DBNull.Value), String.Empty, drRaw.Item("SP_Chi_Name"))), _
                                                                        New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))), _
                                                                            CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))), _
                                                                            CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))), _
                                                                            CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))), _
                                                                            CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))), _
                                                                            CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))), _
                                                                            intAddressCode), _
                                                                        CType(drRaw.Item("Phone_Daytime"), String).Trim, _
                                                                        CStr(IIf((drRaw.Item("Fax") Is DBNull.Value), String.Empty, drRaw.Item("Fax"))), _
                                                                        CType(drRaw.Item("Email"), String).Trim, _
                                                                        String.Empty, _
                                                                        CStr(IIf((drRaw.Item("Email_Changed") Is DBNull.Value), String.Empty, drRaw.Item("Email_Changed"))), _
                                                                        CType(drRaw.Item("Record_Status"), String).Trim, _
                                                                        CStr(IIf((drRaw.Item("Remark") Is DBNull.Value), String.Empty, drRaw.Item("Remark"))), _
                                                                        CType(drRaw.Item("Submission_Method"), String).Trim, _
                                                                        CType(drRaw.Item("Already_Joined_EHR"), String), _
                                                                        CType(IIf(drRaw.Item("Join_PCD") Is DBNull.Value, JoinPCDStatus.NA, drRaw.Item("Join_PCD")), String), _
                                                                        String.Empty, _
                                                                        String.Empty, _
                                                                        CType(drRaw.Item("Create_Dtm"), DateTime), _
                                                                        CType(drRaw.Item("Create_By"), String), _
                                                                        CType(drRaw.Item("Update_Dtm"), DateTime), _
                                                                        CType(drRaw.Item("Update_By"), String), _
                                                                        Nothing, _
                                                                        Nothing, _
                                                                        CType(IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, drRaw.Item("TSMP")), Byte()), _
                                                                        Nothing, _
                                                                        Nothing, _
                                                                        Nothing, _
                                                                        "", _
                                                                        Nothing, _
                                                                        String.Empty,
                                                                        String.Empty,
                                                                        String.Empty,
                                                                        Nothing)
                    ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

                    ' Get Scheme Information
                    Dim udtSchemeInfoBLL As New SchemeInformationBLL
                    udtServiceProviderModel.SchemeInfoList = udtSchemeInfoBLL.GetSchemeInfoListStaging(udtServiceProviderModel.EnrolRefNo, udtDB)

                    ' Get Medical Organization
                    Dim udtMOBLL As New MedicalOrganizationBLL
                    udtServiceProviderModel.MOList = udtMOBLL.GetMOListFromStagingByERN(udtServiceProviderModel.EnrolRefNo, udtDB)

                    ' Will not get Practice, Bank, Practice Scheme Information

                End If

                Return udtServiceProviderModel

            Catch ex As Exception
                Throw

            End Try

        End Function

        Public Function GetServiceProviderStagingProfileByERN(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderModel
            Dim udtSP As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)
            Dim dtmEnrolmentDtm As Nullable(Of DateTime)

            Try
                Dim dtSP As New DataTable

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProviderStaging_get_byERN", prams, dtSP)

                If dtSP.Rows.Count = 1 Then
                    Dim drSP As DataRow = dtSP.Rows(0)

                    If IsDBNull(drSP.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drSP.Item("Address_Code")))
                    End If

                    If IsDBNull(drSP.Item("Enrolment_Dtm")) Then
                        dtmEnrolmentDtm = Nothing
                    Else
                        dtmEnrolmentDtm = Convert.ToDateTime(drSP.Item("Enrolment_Dtm"))
                    End If

                    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                    ' --------------------------------------------------------------------------------------------------------------------------------
                    ' Add [Join_PCD]
                    udtSP = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
                                                                            dtmEnrolmentDtm, _
                                                                            CStr(IIf((drSP.Item("SP_ID") Is DBNull.Value), String.Empty, drSP.Item("SP_ID"))), _
                                                                            String.Empty, _
                                                                            CType(drSP.Item("SP_HKID"), String).Trim, _
                                                                            CType(drSP.Item("SP_Eng_Name"), String).Trim, _
                                                                            CStr(IIf((drSP.Item("SP_Chi_Name") Is DBNull.Value), String.Empty, drSP.Item("SP_Chi_Name"))), _
                                                                            New AddressModel(CStr(IIf((drSP.Item("Room") Is DBNull.Value), String.Empty, drSP.Item("Room"))), _
                                                                                            CStr(IIf((drSP.Item("Floor") Is DBNull.Value), String.Empty, drSP.Item("Floor"))), _
                                                                                            CStr(IIf((drSP.Item("Block") Is DBNull.Value), String.Empty, drSP.Item("Block"))), _
                                                                                            CStr(IIf((drSP.Item("Building") Is DBNull.Value), String.Empty, drSP.Item("Building"))), _
                                                                                            CStr(IIf((drSP.Item("Building_Chi") Is DBNull.Value), String.Empty, drSP.Item("Building_Chi"))), _
                                                                                            CStr(IIf((drSP.Item("District") Is DBNull.Value), String.Empty, drSP.Item("District"))), _
                                                                                            intAddressCode), _
                                                                            CType(drSP.Item("Phone_Daytime"), String).Trim, _
                                                                            CStr(IIf((drSP.Item("Fax") Is DBNull.Value), String.Empty, drSP.Item("Fax"))), _
                                                                            CType(drSP.Item("Email"), String).Trim, _
                                                                            String.Empty, _
                                                                            CStr(IIf((drSP.Item("Email_Changed") Is DBNull.Value), String.Empty, drSP.Item("Email_Changed"))), _
                                                                            CType(drSP.Item("Record_Status"), String).Trim, _
                                                                            CStr(IIf((drSP.Item("Remark") Is DBNull.Value), String.Empty, drSP.Item("Remark"))), _
                                                                            CType(drSP.Item("Submission_Method"), String).Trim, _
                                                                            CType(drSP.Item("Already_Joined_EHR"), String), _
                                                                            CType(IIf(drSP.Item("Join_PCD") Is DBNull.Value, JoinPCDStatus.NA, drSP.Item("Join_PCD")), String), _
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            CType(drSP.Item("Create_Dtm"), DateTime), _
                                                                            CType(drSP.Item("Create_By"), String), _
                                                                            CType(drSP.Item("Update_Dtm"), DateTime), _
                                                                            CType(drSP.Item("Update_By"), String), _
                                                                            Nothing, _
                                                                            Nothing, _
                                                                            CType(IIf(drSP.Item("TSMP") Is DBNull.Value, Nothing, drSP.Item("TSMP")), Byte()), _
                                                                            Nothing, _
                                                                            Nothing, _
                                                                            Nothing, _
                                                                            "", _
                                                                            Nothing, _
                                                                            String.Empty,
                                                                            String.Empty,
                                                                            String.Empty,
                                                                            Nothing)
                    ' CRE17-016 Remove PPIePR Enrolment [End][Koala]


                    ' Get Scheme Information
                    Dim udtSchemeInformationBLL As New SchemeInformationBLL
                    udtSP.SchemeInfoList = udtSchemeInformationBLL.GetSchemeInfoListStaging(udtSP.EnrolRefNo, udtDB)

                    ' Get Medical Organization
                    Dim udtMOBLL As New MedicalOrganization.MedicalOrganizationBLL
                    udtSP.MOList = udtMOBLL.GetMOListFromStagingByERN(udtSP.EnrolRefNo, udtDB)

                    ' Get Practice, Bank, Practice Scheme Information
                    Dim udtPracticeBLL As New PracticeBLL
                    udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSP.EnrolRefNo, udtDB)

                    ' Get ERN Processed
                    Dim udtERNProcessedBLL As New ERNProcessedBLL
                    udtSP.ERNProcessedList = udtERNProcessedBLL.GetERNProcessedListStagingByERN(udtSP.EnrolRefNo, udtDB)
                Else
                    udtSP = Nothing
                End If



                Return udtSP

            Catch ex As Exception
                Throw

            End Try

        End Function

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Public Function GetServiceProviderDataEntrySearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal strScheme As String, ByVal udtDB As Database) As DataTable
        Public Function GetServiceProviderDataEntrySearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal strScheme As String, ByVal udtDB As Database, ByVal blnOverrideResultLimit As Boolean, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
            'Dim dtResult As DataTable = New DataTable
            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, IIf(strERN.Trim.Equals(String.Empty), DBNull.Value, strERN.Trim)), _
                                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(strSPID.Trim.Equals(String.Empty), DBNull.Value, strSPID.Trim)), _
                                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, IIf(strHKID.Trim.Equals(String.Empty), DBNull.Value, strHKID.Trim)), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, IIf(strEname.Trim.Equals(String.Empty), DBNull.Value, strEname.Trim)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, IIf(strPhone.Trim.Equals(String.Empty), DBNull.Value, strPhone.Trim)), _
                                                udtDB.MakeInParam("@service_category_code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, IIf(strServiceCategoryCode.Trim.Equals(String.Empty), DBNull.Value, strServiceCategoryCode.Trim)), _
                                                udtDB.MakeInParam("@status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, IIf(strStatus.Trim.Equals(String.Empty), DBNull.Value, strStatus.Trim)), _
                                                udtDB.MakeInParam("@scheme_code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, IIf(strScheme.Trim.Equals(String.Empty), DBNull.Value, strScheme.Trim))}

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'udtDB.RunProc("proc_ServiceProviderAll_get_bySPInfo", prams, dtResult)
                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_ServiceProviderAll_get_bySPInfo", prams, blnOverrideResultLimit, udtDB, blnForceUnlimitResult)

                'Return dtResult
                Return udtBLLSearchResult
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Catch ex As Exception
                Throw
            End Try
        End Function

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Public Function GetServiceProviderVettingSearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal strSchemeCode As String, ByVal strProgressStatus As String, ByVal udtDB As Database) As DataTable
        Public Function GetServiceProviderVettingSearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal strSchemeCode As String, ByVal strProgressStatus As String, ByVal udtDB As Database, ByVal blnOverrideResultLimit As Boolean, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
            'Dim dtResult As DataTable = New DataTable
            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, IIf(strERN.Trim.Equals(String.Empty), DBNull.Value, strERN.Trim)), _
                                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(strSPID.Trim.Equals(String.Empty), DBNull.Value, strSPID.Trim)), _
                                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, IIf(strHKID.Trim.Equals(String.Empty), DBNull.Value, strHKID.Trim)), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, IIf(strEname.Trim.Equals(String.Empty), DBNull.Value, strEname.Trim)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, IIf(strPhone.Trim.Equals(String.Empty), DBNull.Value, strPhone.Trim)), _
                                                udtDB.MakeInParam("@service_category_code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, IIf(strServiceCategoryCode.Trim.Equals(String.Empty), DBNull.Value, strServiceCategoryCode.Trim)), _
                                                udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, IIf(strStatus.Trim.Equals(String.Empty), DBNull.Value, strStatus.Trim)), _
                                                udtDB.MakeInParam("@progress_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, IIf(strProgressStatus.Trim.Equals(String.Empty), DBNull.Value, strProgressStatus.Trim)), _
                                                udtDB.MakeInParam("@scheme_code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, IIf(strSchemeCode.Trim.Equals(String.Empty), DBNull.Value, strSchemeCode.Trim))}

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'udtDB.RunProc("proc_ServiceProviderStagingSPAccUpd_get_bySPInfo", prams, dtResult)
                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_ServiceProviderStagingSPAccUpd_get_bySPInfo", prams, blnOverrideResultLimit, udtDB, blnForceUnlimitResult)

                'Return dtResult
                Return udtBLLSearchResult
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Catch ex As Exception
                Throw
            End Try
        End Function

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Public Function GetServiceProviderEnquirySearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strSchemeCode As String, ByVal udtDB As Database) As DataTable

        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        'Public Function GetServiceProviderEnquirySearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strSchemeCode As String, ByVal udtDB As Database, ByVal blnOverrideResultLimit As Boolean, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
        Public Function GetServiceProviderEnquirySearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strCname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strSchemeCode As String, ByVal udtDB As Database, ByVal blnOverrideResultLimit As Boolean, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
            'Dim dtResult As DataTable = New DataTable
            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, IIf(strERN.Trim.Equals(String.Empty), DBNull.Value, strERN.Trim)), _
                                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(strSPID.Trim.Equals(String.Empty), DBNull.Value, strSPID.Trim)), _
                                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, IIf(strHKID.Trim.Equals(String.Empty), DBNull.Value, strHKID.Trim)), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, IIf(strEname.Trim.Equals(String.Empty), DBNull.Value, strEname.Trim)), _
                                                udtDB.MakeInParam("@sp_chi_name", ServiceProviderModel.CNameDataType, ServiceProviderModel.CNameDataSize, IIf(strCname.Trim.Equals(String.Empty), DBNull.Value, strCname.Trim)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, IIf(strPhone.Trim.Equals(String.Empty), DBNull.Value, strPhone.Trim)), _
                                                udtDB.MakeInParam("@service_category_code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, IIf(strServiceCategoryCode.Trim.Equals(String.Empty), DBNull.Value, strServiceCategoryCode.Trim)), _
                                                udtDB.MakeInParam("@scheme_code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, IIf(strSchemeCode.Trim.Equals(String.Empty), DBNull.Value, strSchemeCode.Trim))}
                ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'udtDB.RunProc("proc_ServiceProviderAllEnquiry_get_bySPInfo", prams, dtResult)
                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_ServiceProviderAllEnquiry_get_bySPInfo", prams, blnOverrideResultLimit, udtDB, blnForceUnlimitResult)

                'Return dtResult
                Return udtBLLSearchResult
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function GetServiceProviderStagingPermanentHKICRowCountByERN(ByVal strERN As String, ByVal udtDB As Database) As DataTable
            Dim dtResult As DataTable = New DataTable
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}

                udtDB.RunProc("proc_ServiceProviderHKICRowCount_get_byERN", prams, dtResult)

                Return dtResult
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Function GetServiceProviderStagingPermanentHKICRowCountByHKID(ByVal strHKID As String, ByVal udtDB As Database) As DataTable
            Dim dtResult As DataTable = New DataTable
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID)}

                udtDB.RunProc("proc_ServiceProviderHKICRowCount_get_byHKIC", prams, dtResult)

                Return dtResult
            Catch ex As Exception
                Throw
            End Try

        End Function

        Public Function GetServiceProviderEnrolmentRowCount(ByVal udtDB As Database) As DataTable
            Dim dtResult As DataTable = New DataTable
            Try

                udtDB.RunProc("proc_ServiceProviderEnrolmentRowCount", dtResult)

                Return dtResult
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Function GetserviceProviderPermanentTSMP(ByVal strSPID As String, ByVal udtDB As Database) As Byte()
            Dim byteRes() As Byte = Nothing

            Dim dt As New DataTable
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_ServiceProviderTSMP_get_bySPID", prams, dt)

                If dt.Rows.Count > 0 Then
                    byteRes = dt.Rows(0).Item("TSMP")
                End If
            Catch ex As Exception
                Throw
            End Try

            Return byteRes

        End Function

        Public Function GetServiceProviderParticulasPermanentByHKID(ByVal strHKID As String, ByVal udtDB As Database) As DataTable
            Dim dt As New DataTable


            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@HKID", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID)}
                udtDB.RunProc("proc_ServiceProvider_get_byHKID", prams, dt)

                If Not IsNothing(dt) Then
                    If dt.Rows.Count > 0 Then

                    Else
                        dt = Nothing
                    End If

                End If

            Catch ex As Exception
                Throw
            End Try


            Return dt
        End Function


        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Get permanent service provider model with active status by HKID
        ''' </summary>
        ''' <param name="strHKID"></param>
        ''' <param name="udtDB"></param>
        ''' <returns>Service provider model if exist and active; otherwise return nothing</returns>
        ''' <remarks></remarks>
        Public Function GetServiceProviderActivePermanentByHKID(ByVal strHKID As String, ByVal udtDB As Database) As ServiceProviderModel
            Dim dt As DataTable = Nothing
            Dim strSPID As String = String.Empty

            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult

            'dt = GetServiceProviderMaintenanceSearch(String.Empty, String.Empty, strHKID, String.Empty, String.Empty, String.Empty, String.Empty, udtDB)

            ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
            'udtBLLSearchResult = GetServiceProviderMaintenanceSearch(String.Empty, String.Empty, String.Empty, strHKID, String.Empty, String.Empty, String.Empty, String.Empty, udtDB, True, True)
            udtBLLSearchResult = GetServiceProviderMaintenanceSearch(String.Empty, String.Empty, String.Empty, strHKID, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, udtDB, True, True)
            dt = CType(udtBLLSearchResult.Data, DataTable)
            ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            If dt.Rows.Count = 0 Then Return Nothing

            For Each dr As DataRow In dt.Rows
                If Trim(dr("Record_Status")) <> ServiceProviderStatus.Active Then Continue For

                strSPID = dr("SP_ID")
                Return GetServiceProviderPermanentProfileWithMaintenanceBySPID(strSPID, udtDB)
            Next

            Return Nothing
        End Function
        ' CRE12-001 eHS and PCD integration [End][Koala]

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
        'Public Function GetServiceProviderMaintenanceSearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strSchemeCode As String, ByVal udtDB As Database) As DataTable
        Public Function GetServiceProviderMaintenanceSearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strCname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strSchemeCode As String, ByVal udtDB As Database, Optional ByVal blnOverrideResultLimit As Boolean = False, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
            'Dim dtResult As DataTable = New DataTable
            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, IIf(strERN.Trim.Equals(String.Empty), DBNull.Value, strERN.Trim)), _
                                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(strSPID.Trim.Equals(String.Empty), DBNull.Value, strSPID.Trim)), _
                                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, IIf(strHKID.Trim.Equals(String.Empty), DBNull.Value, strHKID.Trim)), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, IIf(strEname.Trim.Equals(String.Empty), DBNull.Value, strEname.Trim)), _
                                                udtDB.MakeInParam("@sp_chi_name", ServiceProviderModel.CNameDataType, ServiceProviderModel.CNameDataSize, IIf(strCname.Trim.Equals(String.Empty), DBNull.Value, strCname.Trim)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, IIf(strPhone.Trim.Equals(String.Empty), DBNull.Value, strPhone.Trim)), _
                                                udtDB.MakeInParam("@service_category_code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, IIf(strServiceCategoryCode.Trim.Equals(String.Empty), DBNull.Value, strServiceCategoryCode.Trim)), _
                                                udtDB.MakeInParam("@scheme_code", SchemeInformationModel.SchemeCodeDataType, SchemeInformationModel.SchemeCodemDataSize, IIf(strSchemeCode.Trim.Equals(String.Empty), DBNull.Value, strSchemeCode.Trim))}

                ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]
                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'udtDB.RunProc("proc_ServiceProviderSPAccMaintenance_get_bySPInfo", prams, dtResult)
                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_ServiceProviderSPAccMaintenance_get_bySPInfo", prams, blnOverrideResultLimit, udtDB, blnForceUnlimitResult)

                'Return dtResult
                Return udtBLLSearchResult
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function CheckServiceProviderOriginalProfileExistByERN(ByVal strERN As String, Optional ByVal udtDB As Database = Nothing) As Boolean

            Try

                If udtDB Is Nothing Then
                    udtDB = New Database
                End If

                Dim dtSP As New DataTable

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProviderOriginal_check_byERN", prams, dtSP)

                If dtSP.Rows.Count >= 1 Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Throw

            End Try

        End Function
        '

        Public Function AddServiceProviderParticularsToEnrolment(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal strBatchID As String, ByRef udtDB As Database) As Boolean
            Try
                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' ==========================================================
                ' Add [Join_PCD]
                Dim prams() As SqlParameter = { _
                                                  udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtServiceProviderModel.EnrolRefNo), _
                                                  udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, udtServiceProviderModel.HKID), _
                                                  udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, udtServiceProviderModel.EnglishName), _
                                                  udtDB.MakeInParam("@sp_chi_name", ServiceProviderModel.CNameDataType, ServiceProviderModel.CNameDataSize, IIf(udtServiceProviderModel.ChineseName.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.ChineseName)), _
                                                  udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtServiceProviderModel.SpAddress.Room.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Room)), _
                                                  udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtServiceProviderModel.SpAddress.Floor.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Floor)), _
                                                  udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtServiceProviderModel.SpAddress.Block.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Block)), _
                                                  udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtServiceProviderModel.SpAddress.Address_Code.HasValue OrElse udtServiceProviderModel.SpAddress.Building.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Building)), _
                                                  udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtServiceProviderModel.SpAddress.Address_Code.HasValue OrElse udtServiceProviderModel.SpAddress.ChiBuilding.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.ChiBuilding)), _
                                                  udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtServiceProviderModel.SpAddress.Address_Code.HasValue OrElse udtServiceProviderModel.SpAddress.District.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.District)), _
                                                  udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtServiceProviderModel.SpAddress.Address_Code.HasValue, udtServiceProviderModel.SpAddress.Address_Code, DBNull.Value)), _
                                                  udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, udtServiceProviderModel.Phone), _
                                                  udtDB.MakeInParam("@fax", ServiceProviderModel.FaxDataType, ServiceProviderModel.FaxDataSize, IIf(udtServiceProviderModel.Fax.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.Fax)), _
                                                  udtDB.MakeInParam("@email", ServiceProviderModel.EmailDataType, ServiceProviderModel.EmailDataSize, udtServiceProviderModel.Email), _
                                                  udtDB.MakeInParam("@already_joined_EHR", ServiceProviderModel.AlreadyJoinedEHRDataType, ServiceProviderModel.AlreadyJoinedEHRDataSize, IIf(udtServiceProviderModel.AlreadyJoinEHR.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.AlreadyJoinEHR)), _
                                                  udtDB.MakeInParam("@Join_PCD", ServiceProviderModel.JoinPCDDataType, ServiceProviderModel.JoinPCDDataSize, IIf(udtServiceProviderModel.JoinPCD.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.JoinPCD)), _
                                                  udtDB.MakeInParam("@batch_id", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, IIf(strBatchID.Trim.Equals(String.Empty), DBNull.Value, strBatchID))}

                udtDB.RunProc("proc_ServiceProviderEnrolment_add", prams)
                Return True
                ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
            Catch ex As Exception
                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' ==========================================================
                Throw
                'Throw ex
                'Return False
                ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
            End Try
        End Function

        Public Function AddServiceProviderParticularsToStaging(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean
            Try
                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' ==========================================================
                ' Add [Join_PCD]
                Dim prams() As SqlParameter = { _
                                                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtServiceProviderModel.EnrolRefNo), _
                                                udtDB.MakeInParam("@enrolment_dtm", ServiceProviderModel.EnrolDateDataType, ServiceProviderModel.EnrolDateDataSize, IIf(udtServiceProviderModel.EnrolDate.HasValue, udtServiceProviderModel.EnrolDate, DBNull.Value)), _
                                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtServiceProviderModel.SPID.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SPID)), _
                                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, udtServiceProviderModel.HKID), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, udtServiceProviderModel.EnglishName), _
                                                udtDB.MakeInParam("@sp_chi_name", ServiceProviderModel.CNameDataType, ServiceProviderModel.CNameDataSize, IIf(udtServiceProviderModel.ChineseName.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.ChineseName)), _
                                                udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtServiceProviderModel.SpAddress.Room.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Room)), _
                                                udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtServiceProviderModel.SpAddress.Floor.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Floor)), _
                                                udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtServiceProviderModel.SpAddress.Block.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Block)), _
                                                udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtServiceProviderModel.SpAddress.Building.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.Building)), _
                                                udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtServiceProviderModel.SpAddress.ChiBuilding.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.ChiBuilding)), _
                                                udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtServiceProviderModel.SpAddress.District.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.District)), _
                                                udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtServiceProviderModel.SpAddress.Address_Code.HasValue, udtServiceProviderModel.SpAddress.Address_Code, DBNull.Value)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, udtServiceProviderModel.Phone), _
                                                udtDB.MakeInParam("@fax", ServiceProviderModel.FaxDataType, ServiceProviderModel.FaxDataSize, IIf(udtServiceProviderModel.Fax.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.Fax)), _
                                                udtDB.MakeInParam("@email", ServiceProviderModel.EmailDataType, ServiceProviderModel.EmailDataSize, udtServiceProviderModel.Email), _
                                                udtDB.MakeInParam("@email_changed", ServiceProviderModel.EmailChangedDataType, ServiceProviderModel.EmailChangedDataSize, IIf((udtServiceProviderModel.EmailChanged.Equals(EmailChanged.Unchanged) Or udtServiceProviderModel.EmailChanged.Equals(String.Empty)), DBNull.Value, udtServiceProviderModel.EmailChanged)), _
                                                udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, udtServiceProviderModel.RecordStatus), _
                                                udtDB.MakeInParam("@remark", ServiceProviderModel.RemarkDataType, ServiceProviderModel.RemarkDataSize, IIf(udtServiceProviderModel.Remark.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.Remark)), _
                                                udtDB.MakeInParam("@submission_method", ServiceProviderModel.SubmissionMethodDataType, ServiceProviderModel.SubmissionMethodDataSize, udtServiceProviderModel.SubmitMethod), _
                                                udtDB.MakeInParam("@already_joined_EHR", ServiceProviderModel.AlreadyJoinedEHRDataType, ServiceProviderModel.AlreadyJoinedEHRDataSize, IIf(udtServiceProviderModel.AlreadyJoinEHR.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.AlreadyJoinEHR)), _
                                                udtDB.MakeInParam("@Join_PCD", ServiceProviderModel.JoinPCDDataType, ServiceProviderModel.JoinPCDDataSize, IIf(udtServiceProviderModel.JoinPCD.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.JoinPCD)), _
                                                udtDB.MakeInParam("@application_printed", ServiceProviderModel.ApplicationPrintedDataType, ServiceProviderModel.ApplicationPrintedDataSize, IIf(udtServiceProviderModel.ApplicationPrinted.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.ApplicationPrinted)), _
                                                udtDB.MakeInParam("@create_by", ServiceProviderModel.CreateByDataType, ServiceProviderModel.CreateByDataSize, udtServiceProviderModel.CreateBy), _
                                                udtDB.MakeInParam("@update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, IIf(udtServiceProviderModel.UpdateBy.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.UpdateBy))}

                udtDB.RunProc("proc_ServiceProviderStaging_add", prams)
                Return True
                ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
            Catch ex As Exception
                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' ==========================================================
                Throw
                'Return False
                ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
            End Try
        End Function

        Public Function AddServiceProviderParticularsToStagingForReject(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean
            Try
                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' ==========================================================
                Dim prams() As SqlParameter = { _
                                                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtServiceProviderModel.EnrolRefNo), _
                                                udtDB.MakeInParam("@enrolment_dtm", ServiceProviderModel.EnrolDateDataType, ServiceProviderModel.EnrolDateDataSize, IIf(udtServiceProviderModel.EnrolDate.HasValue, udtServiceProviderModel.EnrolDate, DBNull.Value)), _
                                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtServiceProviderModel.SPID.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SPID)), _
                                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, udtServiceProviderModel.HKID), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, udtServiceProviderModel.EnglishName), _
                                                udtDB.MakeInParam("@sp_chi_name", ServiceProviderModel.CNameDataType, ServiceProviderModel.CNameDataSize, IIf(udtServiceProviderModel.ChineseName.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.ChineseName)), _
                                                udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtServiceProviderModel.SpAddress.Room.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Room)), _
                                                udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtServiceProviderModel.SpAddress.Floor.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Floor)), _
                                                udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtServiceProviderModel.SpAddress.Block.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Block)), _
                                                udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtServiceProviderModel.SpAddress.Building.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.Building)), _
                                                udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtServiceProviderModel.SpAddress.ChiBuilding.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.ChiBuilding)), _
                                                udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtServiceProviderModel.SpAddress.District.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.District)), _
                                                udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtServiceProviderModel.SpAddress.Address_Code.HasValue, udtServiceProviderModel.SpAddress.Address_Code, DBNull.Value)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, udtServiceProviderModel.Phone), _
                                                udtDB.MakeInParam("@fax", ServiceProviderModel.FaxDataType, ServiceProviderModel.FaxDataSize, IIf(udtServiceProviderModel.Fax.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.Fax)), _
                                                udtDB.MakeInParam("@email", ServiceProviderModel.EmailDataType, ServiceProviderModel.EmailDataSize, udtServiceProviderModel.Email), _
                                                udtDB.MakeInParam("@email_changed", ServiceProviderModel.EmailChangedDataType, ServiceProviderModel.EmailChangedDataSize, IIf((udtServiceProviderModel.EmailChanged.Equals(EmailChanged.Unchanged) Or udtServiceProviderModel.EmailChanged.Equals(String.Empty)), DBNull.Value, udtServiceProviderModel.EmailChanged)), _
                                                udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, udtServiceProviderModel.RecordStatus), _
                                                udtDB.MakeInParam("@remark", ServiceProviderModel.RemarkDataType, ServiceProviderModel.RemarkDataSize, IIf(udtServiceProviderModel.Remark.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.Remark)), _
                                                udtDB.MakeInParam("@submission_method", ServiceProviderModel.SubmissionMethodDataType, ServiceProviderModel.SubmissionMethodDataSize, udtServiceProviderModel.SubmitMethod), _
                                                udtDB.MakeInParam("@already_joined_EHR", ServiceProviderModel.AlreadyJoinedEHRDataType, ServiceProviderModel.AlreadyJoinedEHRDataSize, IIf(udtServiceProviderModel.AlreadyJoinEHR.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.AlreadyJoinEHR)), _
                                                udtDB.MakeInParam("@join_PCD", ServiceProviderModel.JoinPCDDataType, ServiceProviderModel.JoinPCDDataSize, IIf(udtServiceProviderModel.JoinPCD.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.JoinPCD)), _
                                                udtDB.MakeInParam("@application_printed", ServiceProviderModel.ApplicationPrintedDataType, ServiceProviderModel.ApplicationPrintedDataSize, IIf(udtServiceProviderModel.ApplicationPrinted.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.ApplicationPrinted)), _
                                                udtDB.MakeInParam("@create_by", ServiceProviderModel.CreateByDataType, ServiceProviderModel.CreateByDataSize, udtServiceProviderModel.CreateBy), _
                                                udtDB.MakeInParam("@update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, IIf(udtServiceProviderModel.UpdateBy.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.UpdateBy))}

                udtDB.RunProc("proc_ServiceProviderStaging_add_forReject", prams)
                Return True
                ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
            Catch ex As Exception
                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' ==========================================================
                Throw
                'Return False
                ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
            End Try
        End Function

        Public Function AddServiceProviderProfileToStaging(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtPracticeList As PracticeModelCollection, ByVal udtBankAcctList As BankAcctModelCollection, ByVal udtProfessionalList As ProfessionalModelCollection, ByVal udtDB As Database) As Boolean
            Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
            Dim udtBankAcctBLL As BankAcctBLL = New BankAcctBLL
            Dim udtProfessionalBLL As ProfessionalBLL = New ProfessionalBLL

            Dim udtSchememInfoBLL As SchemeInformationBLL = New SchemeInformationBLL

            Dim blnRes As Boolean

            Try
                If udtPracticeList Is Nothing Then
                    If AddServiceProviderParticularsToStaging(udtServiceProviderModel, udtDB) And _
                        udtSchememInfoBLL.AddSchemeInfoListToStaging(udtServiceProviderModel.SchemeInfoList, udtDB) Then
                        blnRes = True
                    Else
                        blnRes = False
                    End If
                Else
                    If udtBankAcctList Is Nothing Then
                        If AddServiceProviderParticularsToStaging(udtServiceProviderModel, udtDB) And _
                           udtSchememInfoBLL.AddSchemeInfoListToStaging(udtServiceProviderModel.SchemeInfoList, udtDB) And _
                           udtPracticeBLL.AddPracticeListToStaging(udtPracticeList, udtDB) And udtProfessionalBLL.AddProfessionalListToStaging(udtProfessionalList, udtDB) Then
                            blnRes = True
                        Else
                            blnRes = False
                        End If
                    Else
                        If AddServiceProviderParticularsToStaging(udtServiceProviderModel, udtDB) And _
                           udtSchememInfoBLL.AddSchemeInfoListToStaging(udtServiceProviderModel.SchemeInfoList, udtDB) And _
                           udtPracticeBLL.AddPracticeListToStaging(udtPracticeList, udtDB) And udtBankAcctBLL.AddBankAcctListToStaging(udtBankAcctList, udtDB) And udtProfessionalBLL.AddProfessionalListToStaging(udtProfessionalList, udtDB) Then
                            blnRes = True
                        Else
                            blnRes = False
                        End If
                    End If
                End If

            Catch ex As Exception
                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                ' ==========================================================
                Throw
                'Return False
                ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
            End Try

            Return blnRes
        End Function

        Public Function UpdateServiceProviderEnrolmentPrintStatus(ByVal strERN As String, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProviderEnrolment_upd_Printed", prams)

                blnRes = True

            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function UpdateServiceProviderStagingParticulars(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                Dim prams() As SqlParameter = { _
                                                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtServiceProviderModel.EnrolRefNo), _
                                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, udtServiceProviderModel.HKID), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, udtServiceProviderModel.EnglishName), _
                                                udtDB.MakeInParam("@sp_chi_name", ServiceProviderModel.CNameDataType, ServiceProviderModel.CNameDataSize, IIf(udtServiceProviderModel.ChineseName.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.ChineseName)), _
                                                udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtServiceProviderModel.SpAddress.Room.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Room)), _
                                                udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtServiceProviderModel.SpAddress.Floor.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Floor)), _
                                                udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtServiceProviderModel.SpAddress.Block.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Block)), _
                                                udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtServiceProviderModel.SpAddress.Building.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.Building)), _
                                                udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtServiceProviderModel.SpAddress.ChiBuilding.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.ChiBuilding)), _
                                                udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtServiceProviderModel.SpAddress.District.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.District)), _
                                                udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtServiceProviderModel.SpAddress.Address_Code.HasValue, udtServiceProviderModel.SpAddress.Address_Code, DBNull.Value)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, udtServiceProviderModel.Phone), _
                                                udtDB.MakeInParam("@fax", ServiceProviderModel.FaxDataType, ServiceProviderModel.FaxDataSize, IIf(udtServiceProviderModel.Fax.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.Fax)), _
                                                udtDB.MakeInParam("@email", ServiceProviderModel.EmailDataType, ServiceProviderModel.EmailDataSize, udtServiceProviderModel.Email), _
                                                udtDB.MakeInParam("@email_changed", ServiceProviderModel.EmailChangedDataType, ServiceProviderModel.EmailChangedDataSize, IIf(udtServiceProviderModel.EmailChanged.Equals(EmailChanged.Unchanged) OrElse udtServiceProviderModel.EmailChanged.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.EmailChanged)), _
                                                udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, udtServiceProviderModel.RecordStatus), _
                                                udtDB.MakeInParam("@remark", ServiceProviderModel.RemarkDataType, ServiceProviderModel.RemarkDataSize, IIf(udtServiceProviderModel.Remark.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.Remark)), _
                                                udtDB.MakeInParam("@update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, IIf(udtServiceProviderModel.UpdateBy.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.UpdateBy)), _
                                                udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtServiceProviderModel.TSMP)}

                udtDB.RunProc("proc_ServiceProviderStaging_upd", prams)
                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw
            End Try

            Return True
        End Function

        Public Function UpdateServiceProviderUnderModificationStatus(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtServiceProviderModel.SPID), _
                                                udtDB.MakeInParam("@undermodification", ServiceProviderModel.UnderModificationDataType, ServiceProviderModel.UnderModificationDataSize, IIf(udtServiceProviderModel.UnderModification = Nothing, DBNull.Value, udtServiceProviderModel.UnderModification)), _
                                                udtDB.MakeInParam("@update_by", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, udtServiceProviderModel.UpdateBy), _
                                                udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtServiceProviderModel.TSMP)}
                udtDB.RunProc("proc_ServiceProvider_upd_UnderModify", prams)

                blnRes = True

            Catch ex As Exception
                Throw
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function UpdateServiceProviderStagingEHRSSStatus(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtServiceProviderModel.EnrolRefNo), _
                                                udtDB.MakeInParam("@already_joined_EHR", ServiceProviderModel.AlreadyJoinedEHRDataType, ServiceProviderModel.AlreadyJoinedEHRDataSize, udtServiceProviderModel.AlreadyJoinEHR), _
                                                udtDB.MakeInParam("@update_by", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, udtServiceProviderModel.UpdateBy), _
                                                udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtServiceProviderModel.TSMP)}
                udtDB.RunProc("proc_ServiceProviderStaging_upd_EHRSS", prams)

                blnRes = True

            Catch ex As Exception
                Throw
                blnRes = False
            End Try

            Return blnRes
        End Function

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
        ' --------------------------------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Update Join PCD value when there is any changes on Profession
        ''' </summary>
        ''' <param name="udtServiceProviderModel"></param>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function UpdateServiceProviderStagingJoinPCD(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False

            Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtServiceProviderModel.EnrolRefNo), _
                                                udtDB.MakeInParam("@join_PCD", ServiceProviderModel.JoinPCDDataType, ServiceProviderModel.JoinPCDDataSize, udtServiceProviderModel.JoinPCD), _
                                                udtDB.MakeInParam("@update_by", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, udtServiceProviderModel.UpdateBy), _
                                                udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtServiceProviderModel.TSMP)}
            udtDB.RunProc("proc_ServiceProviderStaging_upd_JoinPCD", prams)

            blnRes = True

            Return blnRes
        End Function
        ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

        Public Function DeleteServiceProviderEnrolmentProfile(ByVal strERN As String, ByVal udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}

                udtDB.RunProc("proc_ServiceProviderEnrolmentProfile_del", prams)
                Return True
            Catch ex As Exception
                Throw
                Return False
            End Try
        End Function

        Public Function UpdateServiceProviderRecordStatus(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtServiceProviderModel.SPID), _
                '                                udtDB.MakeInParam("@Record_Status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, udtServiceProviderModel.RecordStatus), _
                '                                udtDB.MakeInParam("@Update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, udtServiceProviderModel.UpdateBy), _
                '                                udtDB.MakeInParam("@TSMP", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtServiceProviderModel.TSMP)}
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtServiceProviderModel.SPID), _
                                    udtDB.MakeInParam("@Record_Status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, udtServiceProviderModel.RecordStatus), _
                                    udtDB.MakeInParam("@Update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, udtServiceProviderModel.UpdateBy), _
                                    udtDB.MakeInParam("@Data_Input_By", ServiceProviderModel.DataInputByDataType, ServiceProviderModel.DataInputByDataSize, udtServiceProviderModel.DataInputBy), _
                                    udtDB.MakeInParam("@TSMP", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtServiceProviderModel.TSMP)}
                ' INT13-0028 - SP Amendment Report [End][Tommy L]
                udtDB.RunProc("proc_ServiceProvider_upd_RecordStatus", prams)

                blnRes = True

            Catch ex As Exception
                Throw
                blnRes = False
            End Try

            Return blnRes

        End Function

        Public Function UpdateServiceProviderPermanentParticulars(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean

            Dim blnRes As Boolean = False
            Try
                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                ' Add [@data_input_dtm], [@data_input_by]
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtServiceProviderModel.SPID), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, udtServiceProviderModel.EnglishName), _
                                                udtDB.MakeInParam("@sp_chi_name", ServiceProviderModel.CNameDataType, ServiceProviderModel.CNameDataSize, udtServiceProviderModel.ChineseName), _
                                                udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtServiceProviderModel.SpAddress.Room.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Room)), _
                                                udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtServiceProviderModel.SpAddress.Floor.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Floor)), _
                                                udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtServiceProviderModel.SpAddress.Block.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Block)), _
                                                udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtServiceProviderModel.SpAddress.Building.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.Building)), _
                                                udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtServiceProviderModel.SpAddress.ChiBuilding.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.ChiBuilding)), _
                                                udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtServiceProviderModel.SpAddress.District.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.District)), _
                                                udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtServiceProviderModel.SpAddress.Address_Code.HasValue, udtServiceProviderModel.SpAddress.Address_Code, DBNull.Value)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, udtServiceProviderModel.Phone), _
                                                udtDB.MakeInParam("@fax", ServiceProviderModel.FaxDataType, ServiceProviderModel.FaxDataSize, IIf(udtServiceProviderModel.Fax.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.Fax)), _
                                                udtDB.MakeInParam("@tentative_email", ServiceProviderModel.EmailDataType, ServiceProviderModel.EmailDataSize, IIf(udtServiceProviderModel.EmailChanged.Equals(EmailChanged.Changed), udtServiceProviderModel.Email, DBNull.Value)), _
                                                udtDB.MakeInParam("@update_by", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, IIf(udtServiceProviderModel.UpdateBy.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.UpdateBy)), _
                                                udtDB.MakeInParam("@data_input_by", ServiceProviderModel.DataInputByDataType, ServiceProviderModel.DataInputByDataSize, udtServiceProviderModel.DataInputBy), _
                                                udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtServiceProviderModel.TSMP)}
                ' INT13-0028 - SP Amendment Report [End][Tommy L]
                udtDB.RunProc("proc_ServiceProvider_upd_SPInfo", prams)

                blnRes = True

            Catch ex As Exception
                Throw
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function UpdateServiceProviderPermanentParticularsBySchemeEnrolment(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean

            Dim blnRes As Boolean = False
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtServiceProviderModel.SPID), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, udtServiceProviderModel.EnglishName), _
                                                udtDB.MakeInParam("@sp_chi_name", ServiceProviderModel.CNameDataType, ServiceProviderModel.CNameDataSize, udtServiceProviderModel.ChineseName), _
                                                udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtServiceProviderModel.SpAddress.Room.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Room)), _
                                                udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtServiceProviderModel.SpAddress.Floor.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Floor)), _
                                                udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtServiceProviderModel.SpAddress.Block.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Block)), _
                                                udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtServiceProviderModel.SpAddress.Building.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.Building)), _
                                                udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtServiceProviderModel.SpAddress.ChiBuilding.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.ChiBuilding)), _
                                                udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtServiceProviderModel.SpAddress.District.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.District)), _
                                                udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtServiceProviderModel.SpAddress.Address_Code.HasValue, udtServiceProviderModel.SpAddress.Address_Code, DBNull.Value)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, udtServiceProviderModel.Phone), _
                                                udtDB.MakeInParam("@fax", ServiceProviderModel.FaxDataType, ServiceProviderModel.FaxDataSize, IIf(udtServiceProviderModel.Fax.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.Fax)), _
                                                udtDB.MakeInParam("@tentative_email", ServiceProviderModel.TentativeEmailDataType, ServiceProviderModel.TentativeEmailDataSize, IIf(udtServiceProviderModel.EmailChanged.Equals(EmailChanged.Changed), udtServiceProviderModel.Email, DBNull.Value)), _
                                                udtDB.MakeInParam("@update_by", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, IIf(udtServiceProviderModel.UpdateBy.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.UpdateBy)), _
                                                udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtServiceProviderModel.TSMP)}
                udtDB.RunProc("proc_ServiceProvider_upd_SPInfoKeepUnderModification", prams)

                blnRes = True

            Catch ex As Exception
                Throw
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function UpdateServiceProviderPermanentUnderModification(ByVal strSP_ID As String, ByVal strUpdateBy As String, ByVal udtDB As Database) As Boolean

            Dim blnRes As Boolean = False
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSP_ID), _
                                                udtDB.MakeInParam("@update_by", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, IIf(strUpdateBy.Equals(String.Empty), DBNull.Value, strUpdateBy)) _
                                                }
                udtDB.RunProc("proc_ServiceProvider_upd_SPInfoClearModificationStatus", prams)

                blnRes = True

            Catch ex As Exception
                Throw
                blnRes = False
            End Try

            Return blnRes
        End Function

        ' CRE17-016 (Checking of PCD status during VSS enrolment) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Function UpdateServiceProviderJoinPCDStatus(ByVal strSPID As String, _
                                                           ByVal strPCDAccountStatus As String, _
                                                           ByVal strPCDEnrolmentStatus As String, _
                                                           ByVal strPCDProfessional As String, _
                                                           ByVal dtmLastCheckDtm As String, _
                                                           ByVal strUpdateBy As String, _
                                                           ByVal byteTSMP As Byte() _
                                                           ) As Boolean
            Dim udtDB As New Database

            Try

                Dim params() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                                                udtDB.MakeInParam("@PCD_Account_Status", ServiceProviderModel.PCDAccountStatusDataType, ServiceProviderModel.PCDAccountStatusDataSize, IIf(strPCDAccountStatus = String.Empty, DBNull.Value, strPCDAccountStatus)), _
                                                udtDB.MakeInParam("@PCD_Enrolment_Status", ServiceProviderModel.PCDEnrolmentStatusDataType, ServiceProviderModel.PCDEnrolmentStatusDataSize, IIf(strPCDEnrolmentStatus = String.Empty, DBNull.Value, strPCDEnrolmentStatus)), _
                                                udtDB.MakeInParam("@PCD_Professional", ServiceProviderModel.PCDProfessionalDataType, ServiceProviderModel.PCDProfessionalDataSize, IIf(strPCDProfessional = String.Empty, DBNull.Value, strPCDProfessional)), _
                                                udtDB.MakeInParam("@PCD_Status_Last_Check_Dtm", ServiceProviderModel.LastCheckDtmDataType, ServiceProviderModel.LastCheckDtmDataSize, dtmLastCheckDtm), _
                                                udtDB.MakeInParam("@Update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, strUpdateBy), _
                                                udtDB.MakeInParam("@TSMP", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, byteTSMP) _
                                               }

                udtDB.BeginTransaction()

                udtDB.RunProc("proc_ServiceProvider_upd_PCDStatus", params)

                udtDB.CommitTransaction()

            Catch ex As Exception

                udtDB.RollBackTranscation()
                Throw

            Finally

                If Not udtDB Is Nothing Then udtDB.Dispose()

            End Try

            Return True
        End Function
        ' CRE17-016 (Checking of PCD status during VSS enrolment) [End][Chris YIM]



        Public Sub DeleteServiceProviderStagingByKey(ByRef udtDB As Database, ByVal strERN As String, ByVal TSMP As Byte(), ByVal blnCheckTSMP As Boolean)
            Try
                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@Enrolment_Ref_No", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                    udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, TSMP), _
                    udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP)}

                udtDB.RunProc("proc_ServiceProviderStaging_del_ByKey", params)

            Catch ex As Exception
                Throw
            End Try
        End Sub

        Public Function AddServiceProviderProfileToPermanent(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtPracticeList As PracticeModelCollection, ByVal udtBankAcctList As BankAcctModelCollection, ByVal udtProfessionalList As ProfessionalModelCollection, ByVal udtSchemeInformationCollection As SchemeInformationModelCollection, ByVal udtDB As Database, ByVal udtERNProcessedModelCollection As ERNProcessedModelCollection) As Boolean
            Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
            Dim udtBankAcctBLL As BankAcctBLL = New BankAcctBLL
            Dim udtProfessionalBLL As ProfessionalBLL = New ProfessionalBLL
            Dim udtSchemeInformationBLL As SchemeInformationBLL = New SchemeInformationBLL
            Dim udtERNProcessedBLL As New ERNProcessedBLL

            Dim blnRes As Boolean

            Try
                If udtPracticeList Is Nothing Then
                    If AddServiceProviderParticularsToPermanent(udtServiceProviderModel, udtDB) Then
                        blnRes = True
                    Else
                        blnRes = False
                    End If
                Else
                    If udtBankAcctList Is Nothing Then
                        If AddServiceProviderParticularsToPermanent(udtServiceProviderModel, udtDB) And udtPracticeBLL.AddPracticeListToPermanent(udtPracticeList, udtDB) And udtProfessionalBLL.AddProfessionalListToPermanent(udtProfessionalList, udtDB) And udtSchemeInformationBLL.AddSchemeInfoListToPermanent(udtSchemeInformationCollection, udtDB) Then
                            blnRes = True
                        Else
                            blnRes = False
                        End If
                    Else
                        If AddServiceProviderParticularsToPermanent(udtServiceProviderModel, udtDB) And udtPracticeBLL.AddPracticeListToPermanent(udtPracticeList, udtDB) And udtBankAcctBLL.AddBankAcctListToPermanent(udtBankAcctList, udtDB) And udtProfessionalBLL.AddProfessionalListToPermanent(udtProfessionalList, udtDB) And udtSchemeInformationBLL.AddSchemeInfoListToPermanent(udtSchemeInformationCollection, udtDB) Then
                            blnRes = True
                        Else
                            blnRes = False
                        End If
                    End If
                End If

                'If the ERNProcessedModelCollection in staging is not empty, then add them back to permanent
                If Not IsNothing(udtERNProcessedModelCollection) Then
                    For Each udtERNProcessedModel As ERNProcessedModel In udtERNProcessedModelCollection.Values
                        udtERNProcessedBLL.AddERNProcessedToPermanent(udtERNProcessedModel, udtDB)
                    Next
                End If
            Catch eSQL As SqlException
                Throw
            Catch ex As Exception
                Throw
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function AddServiceProviderParticularsToPermanent(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean
            Try
                ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Add [@PCD_Status], [@PCD_Status_Last_Check_Dtm]
                Dim prams() As SqlParameter = { _
                                                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtServiceProviderModel.EnrolRefNo), _
                                                udtDB.MakeInParam("@enrolment_dtm", ServiceProviderModel.EnrolDateDataType, ServiceProviderModel.EnrolDateDataSize, IIf(udtServiceProviderModel.EnrolDate.HasValue, udtServiceProviderModel.EnrolDate, DBNull.Value)), _
                                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtServiceProviderModel.SPID.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SPID)), _
                                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, udtServiceProviderModel.HKID), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, udtServiceProviderModel.EnglishName), _
                                                udtDB.MakeInParam("@sp_chi_name", ServiceProviderModel.CNameDataType, ServiceProviderModel.CNameDataSize, IIf(udtServiceProviderModel.ChineseName.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.ChineseName)), _
                                                udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtServiceProviderModel.SpAddress.Room.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Room)), _
                                                udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtServiceProviderModel.SpAddress.Floor.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Floor)), _
                                                udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtServiceProviderModel.SpAddress.Block.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.SpAddress.Block)), _
                                                udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtServiceProviderModel.SpAddress.Building.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.Building)), _
                                                udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtServiceProviderModel.SpAddress.ChiBuilding.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.ChiBuilding)), _
                                                udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtServiceProviderModel.SpAddress.District.Equals(String.Empty) Or udtServiceProviderModel.SpAddress.Address_Code.HasValue, DBNull.Value, udtServiceProviderModel.SpAddress.District)), _
                                                udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtServiceProviderModel.SpAddress.Address_Code.HasValue, udtServiceProviderModel.SpAddress.Address_Code, DBNull.Value)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, udtServiceProviderModel.Phone), _
                                                udtDB.MakeInParam("@fax", ServiceProviderModel.FaxDataType, ServiceProviderModel.FaxDataSize, IIf(udtServiceProviderModel.Fax.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.Fax)), _
                                                udtDB.MakeInParam("@email", ServiceProviderModel.EmailDataType, ServiceProviderModel.EmailDataSize, udtServiceProviderModel.Email), _
                                                udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, udtServiceProviderModel.RecordStatus), _
                                                udtDB.MakeInParam("@remark", ServiceProviderModel.RemarkDataType, ServiceProviderModel.RemarkDataSize, IIf(udtServiceProviderModel.Remark.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.Remark)), _
                                                udtDB.MakeInParam("@submission_method", ServiceProviderModel.SubmissionMethodDataType, ServiceProviderModel.SubmissionMethodDataSize, udtServiceProviderModel.SubmitMethod), _
                                                udtDB.MakeInParam("@already_joined_EHR", ServiceProviderModel.AlreadyJoinedEHRDataType, ServiceProviderModel.AlreadyJoinedEHRDataSize, IIf(udtServiceProviderModel.AlreadyJoinEHR.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.AlreadyJoinEHR)), _
                                                udtDB.MakeInParam("@application_printed", ServiceProviderModel.ApplicationPrintedDataType, ServiceProviderModel.ApplicationPrintedDataSize, IIf(udtServiceProviderModel.ApplicationPrinted.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.ApplicationPrinted)), _
                                                udtDB.MakeInParam("@create_by", ServiceProviderModel.CreateByDataType, ServiceProviderModel.CreateByDataSize, udtServiceProviderModel.CreateBy), _
                                                udtDB.MakeInParam("@update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, IIf(udtServiceProviderModel.UpdateBy.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.UpdateBy)), _
                                                udtDB.MakeInParam("@data_input_by", ServiceProviderModel.DataInputByDataType, ServiceProviderModel.DataInputByDataSize, udtServiceProviderModel.DataInputBy), _
                                                udtDB.MakeInParam("@PCD_Account_Status", ServiceProviderModel.PCDAccountStatusDataType, ServiceProviderModel.PCDAccountStatusDataSize, IIf(udtServiceProviderModel.PCDAccountStatus = String.Empty, DBNull.Value, udtServiceProviderModel.PCDAccountStatus)), _
                                                udtDB.MakeInParam("@PCD_Enrolment_Status", ServiceProviderModel.PCDEnrolmentStatusDataType, ServiceProviderModel.PCDEnrolmentStatusDataSize, IIf(udtServiceProviderModel.PCDEnrolmentStatus = String.Empty, DBNull.Value, udtServiceProviderModel.PCDEnrolmentStatus)), _
                                                udtDB.MakeInParam("@PCD_Professional", ServiceProviderModel.PCDProfessionalDataType, ServiceProviderModel.PCDProfessionalDataSize, IIf(udtServiceProviderModel.PCDProfessional = String.Empty, DBNull.Value, udtServiceProviderModel.PCDProfessional)), _
                                                udtDB.MakeInParam("@PCD_Status_Last_Check_Dtm", ServiceProviderModel.LastCheckDtmDataType, ServiceProviderModel.LastCheckDtmDataSize, IIf(udtServiceProviderModel.PCDStatusLastCheckDtm.HasValue, udtServiceProviderModel.PCDStatusLastCheckDtm, DBNull.Value))}
                ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

                udtDB.RunProc("proc_ServiceProviderPermanent_add", prams)

                'Handle the MO List
                If Not udtServiceProviderModel.MOList Is Nothing Then
                    Dim udtMOBLL As New MedicalOrganization.MedicalOrganizationBLL
                    udtMOBLL.AddMOListToPermanent(udtServiceProviderModel.MOList, udtDB)
                End If

                Return True
            Catch eSQL As SqlException
                Throw
            Catch ex As Exception
                Throw
                Return False
            End Try
        End Function

        Public Function UpdateServiceProviderProfileToPermanentBySchemeEnrolment(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtPracticeList As PracticeModelCollection, ByVal udtBankAcctList As BankAcctModelCollection, ByVal udtProfessionalList As ProfessionalModelCollection, ByVal udtSchemeInformationCollection As SchemeInformationModelCollection, ByVal udtDB As Database, ByVal udtERNProcessedModelCollection As ERNProcessedModelCollection, ByVal udtSchemeInfoPermanentList As SchemeInformationModelCollection, ByVal udtPermanentERNList As ERNProcessed.ERNProcessedModelCollection, ByVal udtPracticeSchemeListPermanent As PracticeSchemeInfo.PracticeSchemeInfoModelCollection) As Boolean
            Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
            Dim udtBankAcctBLL As BankAcctBLL = New BankAcctBLL
            Dim udtProfessionalBLL As ProfessionalBLL = New ProfessionalBLL
            Dim udtSchemeInformationBLL As SchemeInformationBLL = New SchemeInformationBLL
            Dim udtERNProcessedBLL As New ERNProcessedBLL

            Dim blnRes As Boolean

            Try
                If udtPracticeList Is Nothing Then
                    If UpdateServiceProviderPermanentParticulars(udtServiceProviderModel, udtDB) Then
                        blnRes = True
                    Else
                        blnRes = False
                    End If
                Else
                    If udtBankAcctList Is Nothing Then
                        'If UpdateServiceProviderParticularsInPermanent(udtServiceProviderModel, udtDB) And udtPracticeBLL.UpdatePracticeListInPermanentBySchemeEnrolment(udtPracticeList, udtDB) And udtProfessionalBLL.AddProfessionalListToPermanent(udtProfessionalList, udtDB) And udtSchemeInformationBLL.AddSchemeInfoListToPermanent(udtSchemeInformationCollection, udtDB) Then
                        'No need to handle Professional since they should be handled during the partially accept case
                        If UpdateServiceProviderPermanentParticulars(udtServiceProviderModel, udtDB) AndAlso _
                                udtPracticeBLL.UpdatePracticeListInPermanentBySchemeEnrolment(udtPracticeList, udtPracticeSchemeListPermanent, udtDB) AndAlso _
                                udtSchemeInformationBLL.AddNewAddedSchemeInfoListToPermanent(udtSchemeInformationCollection, udtDB, udtSchemeInfoPermanentList) Then
                            blnRes = True
                        Else
                            blnRes = False
                        End If
                    Else
                        'If UpdateServiceProviderParticularsInPermanent(udtServiceProviderModel, udtDB) And udtPracticeBLL.UpdatePracticeListInPermanentBySchemeEnrolment(udtPracticeList, udtDB) And udtBankAcctBLL.AddBankAcctListToPermanent(udtBankAcctList, udtDB) And udtProfessionalBLL.AddProfessionalListToPermanent(udtProfessionalList, udtDB) And udtSchemeInformationBLL.AddSchemeInfoListToPermanent(udtSchemeInformationCollection, udtDB) Then
                        'No need to handle Bank Account and Professional since they should be handled during the partially accept case
                        If UpdateServiceProviderPermanentParticulars(udtServiceProviderModel, udtDB) AndAlso _
                                udtPracticeBLL.UpdatePracticeListInPermanentBySchemeEnrolment(udtPracticeList, udtPracticeSchemeListPermanent, udtDB) AndAlso _
                                udtSchemeInformationBLL.AddNewAddedSchemeInfoListToPermanent(udtSchemeInformationCollection, udtDB, udtSchemeInfoPermanentList) Then
                            blnRes = True
                        Else
                            blnRes = False
                        End If
                    End If
                End If

                'If the ERNProcessedModelCollection is staging is not empty, then add back those not existed in permanent to permanent
                If Not IsNothing(udtERNProcessedModelCollection) Then
                    'Dim udtPermanentERNList As ERNProcessedModelCollection
                    'udtPermanentERNList = udtERNProcessedBLL.GetERNProcessedListPermanentBySPID(udtServiceProviderModel.SPID, udtDB)
                    For Each udtERNProcessedModel As ERNProcessedModel In udtERNProcessedModelCollection.Values
                        If Not IsNothing(udtPermanentERNList) Then
                            If IsNothing(udtPermanentERNList(udtERNProcessedModel.SubEnrolRefNo)) Then
                                udtERNProcessedBLL.AddERNProcessedToPermanent(udtERNProcessedModel, udtDB)
                            End If
                        Else
                            'Empty in permanent
                            udtERNProcessedBLL.AddERNProcessedToPermanent(udtERNProcessedModel, udtDB)
                        End If
                    Next
                End If
            Catch eSQL As SqlException
                Throw
            Catch ex As Exception
                Throw
                blnRes = False
            End Try

            Return blnRes
        End Function

        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [Start] (Marco) ---
        Public Sub UpdatePassword(ByVal strSPID As String, ByVal udtHashedPasswordModel As HashModel, ByVal tsmp As Byte(), ByRef db As Database)
            Try
                db.BeginTransaction()
                Dim parms() As SqlParameter = { _
                    db.MakeInParam("@sp_ID", SqlDbType.Char, 8, strSPID), _
                    db.MakeInParam("@sp_password", SqlDbType.VarChar, 100, udtHashedPasswordModel.HashedValue), _
                    db.MakeInParam("@sp_password_level", SqlDbType.Int, 4, udtHashedPasswordModel.PasswordLevel), _
                    db.MakeInParam("@tsmp", SqlDbType.Timestamp, 16, tsmp) _
                }
                db.RunProc("proc_HCSPUserAC_upd_Password", parms)
                db.CommitTransaction()
            Catch ex As Exception
                db.RollBackTranscation()
                Throw
            Finally
                If Not db Is Nothing Then db.Dispose()
            End Try
        End Sub
        ' --- I-CRE16-007-02 (Refine system from CheckMarx findings) [End] (Marco) ---


        Public Function CheckEmailLinkByCode(ByVal strCode As String) As DataTable
            Dim udtdb As Database = New Database
            Dim dt As New DataTable

            Try
                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@code", SqlDbType.VarChar, 100, strCode), _
                    udtdb.MakeInParam("@status", SqlDbType.Char, 1, "A")}

                udtdb.RunProc("proc_ServiceProvider_get_byCodeStatus", parms, dt)

                Return dt
            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function UpdateServiceProviderEmailActivationCode(ByRef udtDB As Database, ByVal strSPID As String, ByVal strUserID As String, ByVal udtHashPassword As HashModel, ByVal TSMP As Byte(), ByVal blnCheckTSMP As Boolean) As Boolean
            Dim blnRes As Boolean = False
            Try

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                                               udtDB.MakeInParam("@Update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, strUserID), _
                                               udtDB.MakeInParam("@Activation_Code", ServiceProviderModel.Activation_CodeDataType, ServiceProviderModel.Activation_CodeDataSize, udtHashPassword.HashedValue), _
                                               udtDB.MakeInParam("@TSMP", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, TSMP), _
                                               udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP), _
                                               udtDB.MakeInParam("@Activation_Code_Level", SqlDbType.Int, 4, udtHashPassword.PasswordLevel)}
                udtDB.RunProc("proc_ServiceProvider_upd_ActivationCode", prams)

                blnRes = True

            Catch ex As Exception
                Throw
                blnRes = False
            End Try

            Return blnRes
        End Function


        Public Function UpdateServiceProviderUnderModificationAndRecordStatus(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean

            Dim blnRes As Boolean = False
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtServiceProviderModel.SPID), _
                                                udtDB.MakeInParam("@Record_Status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, udtServiceProviderModel.RecordStatus), _
                                                udtDB.MakeInParam("@UnderModification", ServiceProviderModel.UnderModificationDataType, ServiceProviderModel.UnderModificationDataSize, IIf(udtServiceProviderModel.UnderModification = Nothing, DBNull.Value, udtServiceProviderModel.UnderModification)), _
                                                udtDB.MakeInParam("@Update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, udtServiceProviderModel.UpdateBy), _
                                                udtDB.MakeInParam("@TSMP", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtServiceProviderModel.TSMP)}
                udtDB.RunProc("proc_ServiceProvider_upd_UnderModifyAndRecordStatus", prams)

                blnRes = True

            Catch ex As Exception
                Throw
                blnRes = False
            End Try

            Return blnRes

        End Function

        Public Sub UpdatePrintOption(ByVal strSPID As String, ByVal strPrintOption As String)
            Dim udtDB As Database = New Database
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                udtDB.MakeInParam("@ConsentPrintOption", SqlDbType.Char, 1, strPrintOption)}

                udtDB.RunProc("proc_HCSPUserAC_udp_ConsentPrinOption", prams)

            Catch ex As Exception
                Throw ex

            End Try

        End Sub

        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        Public Sub UpdateServiceProviderDataInput(ByVal strSPID As String, ByVal strDataInputBy As String, ByVal strUpdateBy As String, ByVal byteTSMP As Byte(), Optional ByRef udtDB As Database = Nothing)
            If udtDB Is Nothing Then
                udtDB = New Database()
            End If

            Try
                Dim params() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID.Trim()), _
                                                udtDB.MakeInParam("@data_input_by", ServiceProviderModel.DataInputByDataType, ServiceProviderModel.DataInputByDataSize, strDataInputBy.Trim()), _
                                                udtDB.MakeInParam("@update_by", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, strUpdateBy.Trim()), _
                                                udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, byteTSMP)}

                udtDB.RunProc("proc_ServiceProvider_upd_DataInput", params)

            Catch ex As Exception
                Throw

            End Try
        End Sub
        ' INT13-0028 - SP Amendment Report [End][Tommy L]

        Public Function GetSPSchemeInformationStagingSchemeName(ByVal strERN As String) As String
            Dim udtDB As New Database
            Dim dtResult As DataTable = New DataTable
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, IIf(strERN.Trim.Equals(String.Empty), DBNull.Value, strERN.Trim))}

                udtDB.RunProc("proc_SchemeInformationStaging_get_SchemeName", prams, dtResult)

                If dtResult.Rows.Count = 1 Then
                    Return dtResult.Rows(0)(0).ToString.Trim.Replace(SchemeCode.EHCVS, "HCVS")
                Else
                    Return ""
                End If
            Catch ex As Exception
                Throw
            End Try
        End Function

        'Public Function GetServiceProviderStagingProfileByERN_FromIVSS(ByVal strIVSS_ERN As String, ByVal udtDB As Database, ByVal strUserID As String) As ServiceProviderModel
        '    Dim udtSP As ServiceProviderModel = Nothing

        '    Dim intAddressCode As Nullable(Of Integer)
        '    Dim dtmEnrolmentDtm As Nullable(Of DateTime)

        '    Try
        '        Dim dtSP As New DataTable

        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strIVSS_ERN), _
        '                                        udtDB.MakeInParam("@user_id", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, strUserID)}
        '        udtDB.RunProc("proc_ServiceProviderStaging_get_byERN_FromIVSS", prams, dtSP)

        '        Dim drSP As DataRow = dtSP.Rows(0)

        '        If IsDBNull(drSP.Item("Address_Code")) Then
        '            intAddressCode = Nothing
        '        Else
        '            intAddressCode = CInt((drSP.Item("Address_Code")))
        '        End If

        '        If IsDBNull(drSP.Item("Enrolment_Dtm")) Then
        '            dtmEnrolmentDtm = Nothing
        '        Else
        '            dtmEnrolmentDtm = Convert.ToDateTime(drSP.Item("Enrolment_Dtm"))
        '        End If

        '        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        '        ' -------------------------------------------------------------------------
        '        ' Add [dtmDataInputDtm], [strDataInputBy], [dtmDataInputEffectDtm]
        '        udtSP = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
        '                                                                dtmEnrolmentDtm, _
        '                                                                CStr(IIf((drSP.Item("SP_ID") Is DBNull.Value), String.Empty, drSP.Item("SP_ID"))), _
        '                                                                String.Empty, _
        '                                                                CType(drSP.Item("SP_HKID"), String).Trim, _
        '                                                                CType(drSP.Item("SP_Eng_Name"), String).Trim, _
        '                                                                CStr(IIf((drSP.Item("SP_Chi_Name") Is DBNull.Value), String.Empty, drSP.Item("SP_Chi_Name"))), _
        '                                                                New AddressModel(CStr(IIf((drSP.Item("Room") Is DBNull.Value), String.Empty, drSP.Item("Room"))), _
        '                                                                                CStr(IIf((drSP.Item("Floor") Is DBNull.Value), String.Empty, drSP.Item("Floor"))), _
        '                                                                                CStr(IIf((drSP.Item("Block") Is DBNull.Value), String.Empty, drSP.Item("Block"))), _
        '                                                                                CStr(IIf((drSP.Item("Building") Is DBNull.Value), String.Empty, drSP.Item("Building"))), _
        '                                                                                CStr(IIf((drSP.Item("Building_Chi") Is DBNull.Value), String.Empty, drSP.Item("Building_Chi"))), _
        '                                                                                CStr(IIf((drSP.Item("District") Is DBNull.Value), String.Empty, drSP.Item("District"))), _
        '                                                                                intAddressCode), _
        '                                                                CType(drSP.Item("Phone_Daytime"), String).Trim, _
        '                                                                CStr(IIf((drSP.Item("Fax") Is DBNull.Value), String.Empty, drSP.Item("Fax"))), _
        '                                                                CType(drSP.Item("Email"), String).Trim, _
        '                                                                CStr(IIf((drSP.Item("Email_Changed") Is DBNull.Value), String.Empty, drSP.Item("Email_Changed"))), _
        '                                                                CType(drSP.Item("Record_Status"), String).Trim, _
        '                                                                CStr(IIf((drSP.Item("Remark") Is DBNull.Value), String.Empty, drSP.Item("Remark"))), _
        '                                                                CType(drSP.Item("Submission_Method"), String).Trim, _
        '                                                                CType(drSP.Item("Already_Joined_HA_PPI"), String), _
        '                                                                CType(drSP.Item("Join_HA_PPI"), String), _
        '                                                                String.Empty, _
        '                                                                String.Empty, _
        '                                                                CType(drSP.Item("Create_Dtm"), DateTime), _
        '                                                                CType(drSP.Item("Create_By"), String), _
        '                                                                CType(drSP.Item("Update_Dtm"), DateTime), _
        '                                                                CType(drSP.Item("Update_By"), String), _
        '                                                                Nothing, _
        '                                                                Nothing, _
        '                                                                CType(IIf(drSP.Item("TSMP") Is DBNull.Value, Nothing, drSP.Item("TSMP")), Byte()), _
        '                                                                Nothing, _
        '                                                                Nothing, _
        '                                                                Nothing, _
        '                                                                "", _
        '                                                                Nothing)
        '        ' INT13-0028 - SP Amendment Report [End][Tommy L]


        '        ' Get Scheme Information
        '        Dim udtSchemeInformationBLL As New SchemeInformationBLL
        '        udtSP.SchemeInfoList = udtSchemeInformationBLL.GetSchemeInfoListStaging_FromIVSS(udtSP.EnrolRefNo, udtDB, strUserID)

        '        ' Get Medical Organization from MOTransition, not from Staging
        '        Dim udtMOBLL As New MedicalOrganization.MedicalOrganizationBLL
        '        udtSP.MOList = udtMOBLL.GetMOListFromStagingByERN_FromIVSS(udtSP.EnrolRefNo, udtDB, strUserID)

        '        ' Get Practice, Bank, Practice Scheme Information [Practice will be updated with PracticeTransition]
        '        Dim udtPracticeBLL As New PracticeBLL
        '        udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN_FromIVSS(udtSP.EnrolRefNo, udtDB, strUserID)

        '        ' Get ERN Processed [No need to handle]
        '        'Dim udtERNProcessedBLL As New ERNProcessedBLL
        '        'udtSP.ERNProcessedList = udtERNProcessedBLL.GetERNProcessedListStagingByERN(udtSP.EnrolRefNo, udtDB)

        '        Return udtSP

        '    Catch ex As Exception
        '        Throw ex

        '    End Try

        'End Function


        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
        ' ==========================================================
        ' Obsolet function

        'Public Function GetProfessionalVerificationByERN_FromIVSS(ByVal strIVSS_ERN As String, ByVal udtDB As Database, ByVal strUserID As String) As DataTable
        '    Try
        '        Dim dtSP As New DataTable

        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strIVSS_ERN)}

        '        udtDB.RunProc("proc_ProfessionalVerification_get_ByERN_FromIVSS", prams, dtSP)
        '        Return dtSP
        '    Catch ex As Exception
        '        Throw
        '    End Try
        'End Function

        ' CRE17-016 Remove PPIePR Enrolment [End][Koala]
        Public Function CheckServiceProviderExistInIVSS(ByVal strHKID As String, ByVal udtDB As Database) As Boolean
            Try
                Dim udtGeneralFunction As GeneralFunction = New GeneralFunction
                Dim strCheckCIVSSRenew As String = String.Empty
                udtGeneralFunction.getSystemParameter("CheckCIVSSRenew", strCheckCIVSSRenew, String.Empty)
                If strCheckCIVSSRenew.Trim.Equals("Y") Then
                    Dim dtSP As New DataTable

                    Dim prams() As SqlParameter = {udtDB.MakeInParam("@hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID)}

                    udtDB.RunProc("proc_ServiceProviderStaging_get_byHKID_FromIVSS", prams, dtSP)

                    If dtSP.Rows.Count > 0 Then
                        Return True
                    Else
                        Return False
                    End If
                Else
                    Return False
                End If
            Catch ex As Exception
                Throw
            End Try
        End Function

        ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
        ' ------------------------------------------------------------------------
        ' Function Obsoleted
        ' ''' <summary>
        ' ''' Load Service Provider Model (For MyProfile Only)
        ' ''' Retrieve the Practice Type Also in Practice(s)
        ' ''' To Be Remove After Data migration Complete
        ' ''' </summary>
        ' ''' <param name="udtDB"></param>
        ' ''' <param name="strSPID"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetServiceProviderBySPID_ForMyProfileV1(ByRef udtDB As Database, ByVal strSPID As String) As ServiceProviderModel
        '    Dim procName As String = "proc_ServiceProvider_get_bySPID"

        '    Dim udtSP As ServiceProviderModel = Nothing

        '    Dim intAddressCode As Nullable(Of Integer)
        '    Dim dtmEffectiveDtm As Nullable(Of DateTime)
        '    Dim dtmTokenReturnDtm As Nullable(Of DateTime)
        '    Dim strConsentPrintOption As String = String.Empty
        '    Dim udtGeneralFunction As New ComFunction.GeneralFunction

        '    Dim dtRaw As New DataTable()

        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
        '        udtDB.RunProc(procName, prams, dtRaw)

        '        If dtRaw.Rows.Count > 0 Then
        '            Dim drRaw As DataRow = dtRaw.Rows(0)

        '            If IsDBNull(drRaw.Item("Address_Code")) Then
        '                intAddressCode = Nothing
        '            Else
        '                intAddressCode = CInt((drRaw.Item("Address_Code")))
        '            End If

        '            If IsDBNull(drRaw.Item("Effective_Dtm")) Then
        '                dtmEffectiveDtm = Nothing
        '            Else
        '                dtmEffectiveDtm = Convert.ToDateTime(drRaw.Item("Effective_Dtm"))
        '            End If

        '            If IsDBNull(drRaw.Item("Token_Return_Dtm")) Then
        '                dtmTokenReturnDtm = Nothing
        '            Else
        '                dtmTokenReturnDtm = Convert.ToDateTime(drRaw.Item("Token_Return_Dtm"))
        '            End If

        '            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
        '            ' --------------------------------------------------------------------------------------------------------------------------------
        '            ' Add dummy [Join_PCD]
        '            udtSP = New ServiceProviderModel(CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
        '                                                                    Nothing, _
        '                                                                    CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))), _
        '                                                                    CStr(IIf((drRaw.Item("Alias_Account") Is DBNull.Value), String.Empty, drRaw.Item("Alias_Account"))), _
        '                                                                    CType(drRaw.Item("SP_HKID"), String).Trim, _
        '                                                                    CType(drRaw.Item("SP_Eng_Name"), String).Trim, _
        '                                                                    CStr(IIf((drRaw.Item("SP_Chi_Name") Is DBNull.Value), String.Empty, drRaw.Item("SP_Chi_Name"))), _
        '                                                                    New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))), _
        '                                                                                    CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))), _
        '                                                                                    CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))), _
        '                                                                                    CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))), _
        '                                                                                    CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))), _
        '                                                                                    CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))), _
        '                                                                                    intAddressCode), _
        '                                                                    CType(drRaw.Item("Phone_Daytime"), String).Trim, _
        '                                                                    CStr(IIf((drRaw.Item("Fax") Is DBNull.Value), String.Empty, drRaw.Item("Fax"))), _
        '                                                                    CType(drRaw.Item("Email"), String).Trim, _
        '                                                                    CStr(IIf((drRaw.Item("Tentative_Email") Is DBNull.Value), String.Empty, drRaw.Item("Tentative_Email"))), _
        '                                                                    String.Empty, _
        '                                                                    CType(drRaw.Item("Record_Status"), String).Trim, _
        '                                                                    CStr(IIf((drRaw.Item("Remark") Is DBNull.Value), String.Empty, drRaw.Item("Remark"))), _
        '                                                                    CType(drRaw.Item("Submission_Method"), String).Trim, _
        '                                                                    CType(drRaw.Item("Already_Joined_EHR"), String), _
        '                                                                    String.Empty,
        '                                                                    CStr(IIf((drRaw.Item("UnderModification") Is DBNull.Value), String.Empty, drRaw.Item("UnderModification"))), _
        '                                                                    CStr(IIf((drRaw.Item("Application_Printed") Is DBNull.Value), String.Empty, drRaw.Item("Application_Printed"))), _
        '                                                                    CType(drRaw.Item("Create_Dtm"), DateTime), _
        '                                                                    CType(drRaw.Item("Create_By"), String), _
        '                                                                    CType(drRaw.Item("Update_Dtm"), DateTime), _
        '                                                                    CType(drRaw.Item("Update_By"), String), _
        '                                                                    dtmEffectiveDtm, _
        '                                                                    dtmTokenReturnDtm, _
        '                                                                    IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, CType(drRaw.Item("TSMP"), Byte())), _
        '                                                                    Nothing, _
        '                                                                    Nothing, _
        '                                                                    Nothing, _
        '                                                                    CStr(drRaw.Item("Data_Input_By")).Trim(), _
        '                                                                    CDate(drRaw.Item("Data_Input_Effective_Dtm")), _
        '                                                                    String.Empty,
        '                                                                    String.Empty,
        '                                                                    String.Empty,
        '                                                                    Nothing)
        '            ' CRE17-016 Remove PPIePR Enrolment [End][Koala]

        '            If drRaw.Item("ConsentPrintOption") Is DBNull.Value Then
        '                udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)
        '                udtSP.PrintOption = strConsentPrintOption
        '            Else
        '                udtSP.PrintOption = drRaw.Item("ConsentPrintOption")
        '            End If
        '        End If

        '        ' Get Scheme Information
        '        Dim udtSchemeInfoBLL As New SchemeInformationBLL
        '        udtSP.SchemeInfoList = udtSchemeInfoBLL.GetSchemeInfoListPermanent(udtSP.SPID, udtDB)

        '        ' Get Medical Organization
        '        Dim udtMOBLL As New MedicalOrganizationBLL
        '        udtSP.MOList = udtMOBLL.GetMOListFromPermanentBySPID(udtSP.SPID, udtDB)


        '        ' !!!Retrieve the Practice Type Also in Practice(s)
        '        ' Get Practice, Bank, Practice Bank Information
        '        Dim udtPracticeBLL As New PracticeBLL
        '        udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID_ForMyProfileV1(udtSP.SPID, udtDB)

        '        'Get ERN Processed 
        '        Dim udtERNProcessedBLL As New ERNProcessedBLL
        '        udtSP.ERNProcessedList = udtERNProcessedBLL.GetERNProcessedListPermanentBySPID(udtSP.SPID, udtDB)

        '        Return udtSP

        '    Catch ex As Exception
        '        Throw

        '    End Try

        'End Function
        ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function CopyServiceProviderEnrolmentProfileToOriginal(ByVal strERN As String, ByVal udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}

                udtDB.RunProc("proc_ServiceProviderOriginalProfile_Add", prams)
                Return True
            Catch ex As Exception
                Throw
            End Try
        End Function

        Public Function DeleteServiceProviderOriginalProfile(ByVal strERN As String, Optional ByVal udtDB As Database = Nothing) As Boolean
            Try
                If udtDB Is Nothing Then
                    udtDB = New Database()
                End If

                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}

                udtDB.RunProc("proc_ServiceProviderOriginalProfile_del", prams)
                Return True
            Catch ex As Exception
                Throw
            End Try
        End Function
        ' CRE12-001 eHS and PCD integration [End][Koala]
    End Class
End Namespace

