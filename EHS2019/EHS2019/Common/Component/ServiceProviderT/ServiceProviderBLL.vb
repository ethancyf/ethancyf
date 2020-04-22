Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess

Imports Common.Component.Address
Imports Common.Component.PracticeT
Imports Common.Component.BankAcct
Imports Common.Component.Professional
Imports Common.Component.SchemeInformation

Imports Common.Component.ServiceProviderT

Namespace Component.ServiceProviderT
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
            udtNewSP.EmailChanged = udtOldSP.EmailChanged
            udtNewSP.RecordStatus = udtOldSP.RecordStatus
            udtNewSP.DelistStatus = udtOldSP.DelistStatus
            udtNewSP.Remark = udtOldSP.Remark
            udtNewSP.SubmitMethod = udtOldSP.SubmitMethod
            udtNewSP.AlreadyJoinHAPPI = udtOldSP.AlreadyJoinHAPPI
            udtNewSP.JoinHAPPI = udtOldSP.JoinHAPPI
            udtNewSP.UnderModification = udtOldSP.UnderModification
            udtNewSP.ApplicationPrinted = udtOldSP.ApplicationPrinted
            udtNewSP.CreateDtm = udtOldSP.CreateDtm
            udtNewSP.CreateBy = udtOldSP.CreateBy
            udtNewSP.UpdateDtm = udtOldSP.UpdateDtm
            udtNewSP.UpdateBy = udtOldSP.UpdateBy
            udtNewSP.EffectiveDtm = udtOldSP.EffectiveDtm
            udtNewSP.DelistDtm = udtOldSP.DelistDtm
            udtNewSP.TSMP = udtOldSP.TSMP

            udtNewSP.PracticeList = udtOldSP.PracticeList
            udtNewSP.SchemeInfoList = udtOldSP.SchemeInfoList
        End Sub


        Public Function GetServiceProviderEnrolmentProfileByERN(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderModel
            Dim drSP As SqlDataReader = Nothing
            Dim udtServiceProviderModel As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProviderEnrolment_get_byERN", prams, drSP)

                While drSP.Read()

                    If IsDBNull(drSP.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drSP.Item("Address_Code")))
                    End If

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    ' Add [dtmDataInputDtm], [strDataInputBy], [dtmDataInputEffectDtm]
                    udtServiceProviderModel = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
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
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            CType(drSP.Item("Already_Joined_HA_PPI"), String), _
                                                                            CStr(IIf((drSP.Item("Join_HA_PPI") Is DBNull.Value), String.Empty, drSP.Item("Join_HA_PPI"))), _
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
                                                                            "", _
                                                                            Nothing)
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                End While
                drSP.Close()

                Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
                'Dim objBankAcctBLL As BankAcctBLL = New BankAcctBLL

                If Not IsNothing(udtServiceProviderModel) Then
                    udtServiceProviderModel.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromEnrolmentByERN(udtServiceProviderModel.EnrolRefNo, udtDB)
                End If
                'objServiceProviderModel.BankAcctList = objBankAcctBLL.GetBankAcctListByERN(objServiceProviderModel.EnrolRefNo, table, objDB)
                Return udtServiceProviderModel
            Catch ex As Exception
                Throw ex
            Finally
                If Not drSP Is Nothing Then
                    drSP.Close()
                End If
            End Try
        End Function

        Public Function GetServiceProviderEnrolmentProfileBOTHByERN(ByVal strERN As String, ByVal udtDB As Database, ByVal strSchemeCode As String) As ServiceProviderModel
            Dim drSP As SqlDataReader = Nothing
            Dim udtServiceProviderModel As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                                                udtDB.MakeInParam("@scheme", SqlDbType.Char, 5, strSchemeCode)}
                udtDB.RunProc("proc_ServiceProviderEnrolmentBOTH_get_byERN", prams, drSP)

                While drSP.Read()

                    If IsDBNull(drSP.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drSP.Item("Address_Code")))
                    End If

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    ' Add [dtmDataInputDtm], [strDataInputBy], [dtmDataInputEffectDtm]
                    udtServiceProviderModel = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
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
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            CType(drSP.Item("Already_Joined_HA_PPI"), String), _
                                                                            CStr(IIf((drSP.Item("Join_HA_PPI") Is DBNull.Value), String.Empty, drSP.Item("Join_HA_PPI"))), _
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
                                                                            "", _
                                                                            Nothing)
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                End While
                drSP.Close()

                Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
                'Dim objBankAcctBLL As BankAcctBLL = New BankAcctBLL

                udtServiceProviderModel.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromEnrolmentBOTHByERN(udtServiceProviderModel.EnrolRefNo, udtDB, strSchemeCode)
                'objServiceProviderModel.BankAcctList = objBankAcctBLL.GetBankAcctListByERN(objServiceProviderModel.EnrolRefNo, table, objDB)
                Return udtServiceProviderModel
            Catch ex As Exception
                Throw ex
            Finally
                If Not drSP Is Nothing Then
                    drSP.Close()
                End If
            End Try
        End Function

        Public Function GetServiceProviderEnrolmentProfileByERNSPIDSPHKID(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderModel
            Dim drSP As SqlDataReader = Nothing
            Dim udtServiceProviderModel As ServiceProviderModel = Nothing

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProviderEnrolment_get_byERNSPIDSPHKID", prams, drSP)

                'udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                '                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strSPHKID), _
                '                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, strSPEngName)

                While drSP.Read()

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    ' Add [dtmDataInputDtm], [strDataInputBy], [dtmDataInputEffectDtm]
                    udtServiceProviderModel = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
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
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            CType(drSP.Item("Already_Joined_HA_PPI"), String), _
                                                                            CStr(IIf((drSP.Item("Join_HA_PPI") Is DBNull.Value), String.Empty, drSP.Item("Join_HA_PPI"))), _
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
                                                                            "", _
                                                                            Nothing)
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                End While
                drSP.Close()

                Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
                'Dim objBankAcctBLL As BankAcctBLL = New BankAcctBLL

                If Not IsNothing(udtServiceProviderModel) Then
                    udtServiceProviderModel.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERNBankStagingStatus(udtServiceProviderModel.EnrolRefNo, udtDB)
                End If

                'objServiceProviderModel.BankAcctList = objBankAcctBLL.GetBankAcctListByERN(objServiceProviderModel.EnrolRefNo, table, objDB)
                Return udtServiceProviderModel
            Catch ex As Exception
                Throw ex
            Finally
                If Not drSP Is Nothing Then
                    drSP.Close()
                End If
            End Try
        End Function

        Public Function GetServiceProviderBySPID_NoReader(ByRef udtDB As Database, ByVal strSPID As String) As ServiceProviderModel
            Dim procName As String = "proc_ServiceProvider_get_bySPID"

            Dim udtServiceProviderModel As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmDelistDtm As Nullable(Of DateTime)
            Dim strConsentPrintOption As String
            Dim udtGeneralFunction As New ComFunction.GeneralFunction

            Dim dtRaw As New DataTable()

            Try
                Dim prams() As SqlParameter = { _
                                udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
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

                    If IsDBNull(drRaw.Item("Delist_Dtm")) Then
                        dtmDelistDtm = Nothing
                    Else
                        dtmDelistDtm = Convert.ToDateTime(drRaw.Item("Delist_Dtm"))
                    End If

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    ' Add [dtmDataInputDtm], [strDataInputBy], [dtmDataInputEffectDtm]
                    udtServiceProviderModel = New ServiceProviderModel(CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
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
                                                                            String.Empty, _
                                                                            CType(drRaw.Item("Record_Status"), String).Trim, _
                                                                            CStr(IIf((drRaw.Item("Delist_Status") Is DBNull.Value), String.Empty, drRaw.Item("Delist_Status"))), _
                                                                            CStr(IIf((drRaw.Item("Remark") Is DBNull.Value), String.Empty, drRaw.Item("Remark"))), _
                                                                            CType(drRaw.Item("Submission_Method"), String).Trim, _
                                                                            CType(drRaw.Item("Already_Joined_HA_PPI"), String), _
                                                                            CType(drRaw.Item("Join_HA_PPI"), String), _
                                                                            CStr(IIf((drRaw.Item("UnderModification") Is DBNull.Value), String.Empty, drRaw.Item("UnderModification"))), _
                                                                            CStr(IIf((drRaw.Item("Application_Printed") Is DBNull.Value), String.Empty, drRaw.Item("Application_Printed"))), _
                                                                            CType(drRaw.Item("Create_Dtm"), DateTime), _
                                                                            CType(drRaw.Item("Create_By"), String), _
                                                                            CType(drRaw.Item("Update_Dtm"), DateTime), _
                                                                            CType(drRaw.Item("Update_By"), String), _
                                                                            dtmEffectiveDtm, _
                                                                            dtmDelistDtm, _
                                                                            IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, CType(drRaw.Item("TSMP"), Byte())), _
                                                                            Nothing, _
                                                                            CStr(drRaw.Item("Data_Input_By")).Trim(), _
                                                                            CDate(drRaw.Item("Data_Input_Effective_Dtm")))
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    If drRaw.Item("ConsentPrintOption") Is DBNull.Value Then
                        udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)
                        udtServiceProviderModel.PrintOption = strConsentPrintOption
                    Else
                        udtServiceProviderModel.PrintOption = drRaw.Item("ConsentPrintOption")
                    End If
                End If

                Return udtServiceProviderModel
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function GetServiceProviderBySPID(ByVal strID As String, ByVal table As String) As ServiceProviderModel
            Dim procName As String = String.Empty

            Select Case table
                Case TableLocation.Permanent
                    procName = "proc_ServiceProvider_get_bySPID"

                Case TableLocation.Staging
                    procName = ""
            End Select

            Dim drSP As SqlDataReader = Nothing
            Dim udtDB As Database = New Database
            Dim udtServiceProviderModel As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmDelistDtm As Nullable(Of DateTime)
            Dim strConsentPrintOption As String
            Dim udtGeneralFunction As New ComFunction.GeneralFunction
            Try
                Dim prams() As SqlParameter = { _
                                udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strID)}
                udtDB.RunProc(procName, prams, drSP)

                While drSP.Read()

                    If IsDBNull(drSP.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drSP.Item("Address_Code")))
                    End If

                    If IsDBNull(drSP.Item("Effective_Dtm")) Then
                        dtmEffectiveDtm = Nothing
                    Else
                        dtmEffectiveDtm = Convert.ToDateTime(drSP.Item("Effective_Dtm"))
                    End If

                    If IsDBNull(drSP.Item("Delist_Dtm")) Then
                        dtmDelistDtm = Nothing
                    Else
                        dtmDelistDtm = Convert.ToDateTime(drSP.Item("Delist_Dtm"))
                    End If

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    ' Add [dtmDataInputDtm], [strDataInputBy], [dtmDataInputEffectDtm]
                    udtServiceProviderModel = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
                                                                            Nothing, _
                                                                            CStr(IIf((drSP.Item("SP_ID") Is DBNull.Value), String.Empty, drSP.Item("SP_ID"))), _
                                                                            CStr(IIf((drSP.Item("Alias_Account") Is DBNull.Value), String.Empty, drSP.Item("Alias_Account"))), _
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
                                                                            CType(drSP.Item("Record_Status"), String).Trim, _
                                                                            CStr(IIf((drSP.Item("Delist_Status") Is DBNull.Value), String.Empty, drSP.Item("Delist_Status"))), _
                                                                            CStr(IIf((drSP.Item("Remark") Is DBNull.Value), String.Empty, drSP.Item("Remark"))), _
                                                                            CType(drSP.Item("Submission_Method"), String).Trim, _
                                                                            CType(drSP.Item("Already_Joined_HA_PPI"), String), _
                                                                            CType(drSP.Item("Join_HA_PPI"), String), _
                                                                            CStr(IIf((drSP.Item("UnderModification") Is DBNull.Value), String.Empty, drSP.Item("UnderModification"))), _
                                                                            CStr(IIf((drSP.Item("Application_Printed") Is DBNull.Value), String.Empty, drSP.Item("Application_Printed"))), _
                                                                            CType(drSP.Item("Create_Dtm"), DateTime), _
                                                                            CType(drSP.Item("Create_By"), String), _
                                                                            CType(drSP.Item("Update_Dtm"), DateTime), _
                                                                            CType(drSP.Item("Update_By"), String), _
                                                                            dtmEffectiveDtm, _
                                                                            dtmDelistDtm, _
                                                                            IIf(drSP.Item("TSMP") Is DBNull.Value, Nothing, CType(drSP.Item("TSMP"), Byte())), _
                                                                            Nothing, _
                                                                            CStr(drSP.Item("Data_Input_By")).Trim(), _
                                                                            CDate(drSP.Item("Data_Input_Effective_Dtm")))
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    If drSP.Item("ConsentPrintOption") Is DBNull.Value Then
                        udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)
                        udtServiceProviderModel.PrintOption = strConsentPrintOption
                    Else
                        udtServiceProviderModel.PrintOption = drSP.Item("ConsentPrintOption")
                    End If

                    'udtServiceProviderModel = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
                    '                                                        CType(drSP.Item("Enrolment_Dtm"), DateTime), _
                    '                                                        String.Empty, _
                    '                                                        CType(drSP.Item("SP_HKID"), String).Trim, _
                    '                                                        CType(drSP.Item("SP_Eng_Name"), String).Trim, _
                    '                                                        CStr(IIf((drSP.Item("SP_Chi_Name") Is DBNull.Value), String.Empty, drSP.Item("SP_Chi_Name"))), _
                    '                                                        New AddressModel(CStr(IIf((drSP.Item("Room") Is DBNull.Value), String.Empty, drSP.Item("Room"))), _
                    '                                                                        CStr(IIf((drSP.Item("Floor") Is DBNull.Value), String.Empty, drSP.Item("Floor"))), _
                    '                                                                        CStr(IIf((drSP.Item("Block") Is DBNull.Value), String.Empty, drSP.Item("Block"))), _
                    '                                                                        CStr(IIf((drSP.Item("Building") Is DBNull.Value), String.Empty, drSP.Item("Building"))), _
                    '                                                                        CStr(IIf((drSP.Item("Building_Chi") Is DBNull.Value), String.Empty, drSP.Item("Building_Chi"))), _
                    '                                                                        CStr(IIf((drSP.Item("District") Is DBNull.Value), String.Empty, drSP.Item("District"))), _
                    '                                                                        CInt(IIf((drSP.Item("Address_Code") Is DBNull.Value), Nothing, drSP.Item("Address_Code")))), _
                    '                                                        CType(drSP.Item("Phone_Daytime"), String).Trim, _
                    '                                                        CStr(IIf((drSP.Item("Fax") Is DBNull.Value), String.Empty, drSP.Item("Fax"))), _
                    '                                                        CType(drSP.Item("Email"), String).Trim, _
                    '                                                        String.Empty, _
                    '                                                        String.Empty, _
                    '                                                        String.Empty, _
                    '                                                        String.Empty, _
                    '                                                        CType(drSP.Item("Already_Joined_HA_PPI"), String), _
                    '                                                        CStr(IIf((drSP.Item("Join_HA_PPI") Is DBNull.Value), String.Empty, drSP.Item("Join_HA_PPI"))), _
                    '                                                        String.Empty, _
                    '                                                        Nothing, _
                    '                                                        String.Empty, _
                    '                                                        Nothing, _
                    '                                                        String.Empty, _
                    '                                                        Nothing, _
                    '                                                        Nothing)

                End While
                drSP.Close()

                'Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
                ''Dim objBankAcctBLL As BankAcctBLL = New BankAcctBLL

                'udtServiceProviderModel.PracticeList = udtPracticeBLL.GetPracticeBankAcctListByERN(udtServiceProviderModel.EnrolRefNo, table, udtDB)
                ''objServiceProviderModel.BankAcctList = objBankAcctBLL.GetBankAcctListByERN(objServiceProviderModel.EnrolRefNo, table, objDB)
                Return udtServiceProviderModel
            Catch ex As Exception
                Throw ex
            Finally
                If Not drSP Is Nothing Then
                    drSP.Close()
                End If
            End Try
        End Function

        Public Function GetServiceProviderBySPID(ByRef udtDB As Database, ByVal strSPID As String) As ServiceProviderModel
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

                    'If IsDBNull(drRaw.Item("Token_Return_Dtm")) Then
                    '    dtmTokenReturnDtm = Nothing
                    'Else
                    '    dtmTokenReturnDtm = Convert.ToDateTime(drRaw.Item("Token_Return_Dtm"))
                    'End If

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    ' Add [dtmDataInputDtm], [strDataInputBy], [dtmDataInputEffectDtm]
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
                                                                            String.Empty, _
                                                                            CType(drRaw.Item("Record_Status"), String).Trim, _
                                                                            String.Empty, _
                                                                            CStr(IIf((drRaw.Item("Remark") Is DBNull.Value), String.Empty, drRaw.Item("Remark"))), _
                                                                            CType(drRaw.Item("Submission_Method"), String).Trim, _
                                                                            CType(drRaw.Item("Already_Joined_HA_PPI"), String), _
                                                                            CType(drRaw.Item("Join_HA_PPI"), String), _
                                                                            CStr(IIf((drRaw.Item("UnderModification") Is DBNull.Value), String.Empty, drRaw.Item("UnderModification"))), _
                                                                            CStr(IIf((drRaw.Item("Application_Printed") Is DBNull.Value), String.Empty, drRaw.Item("Application_Printed"))), _
                                                                            CType(drRaw.Item("Create_Dtm"), DateTime), _
                                                                            CType(drRaw.Item("Create_By"), String), _
                                                                            CType(drRaw.Item("Update_Dtm"), DateTime), _
                                                                            CType(drRaw.Item("Update_By"), String), _
                                                                            dtmEffectiveDtm, _
                                                                            Nothing, _
                                                                            IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, CType(drRaw.Item("TSMP"), Byte())), _
                                                                            Nothing, _
                                                                            CStr(drRaw.Item("Data_Input_By")).Trim(), _
                                                                            CDate(drRaw.Item("Data_Input_Effective_Dtm")))
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                    If drRaw.Item("ConsentPrintOption") Is DBNull.Value Then
                        udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)
                        udtSP.PrintOption = strConsentPrintOption
                    Else
                        udtSP.PrintOption = drRaw.Item("ConsentPrintOption")
                    End If
                End If

                ' Get Scheme Information
                Dim udtSchemeInfoBLL As New SchemeInformationBLL
                udtSP.SchemeInfoList = udtSchemeInfoBLL.GetSchemeInfoListPermanent(udtSP.SPID, udtDB)

                ' Get Medical Organization
                'Dim udtMOBLL As New Common.Component.MedicalOrganization.MedicalOrganizationBLL
                'udtSP.MOList = udtMOBLL.GetMOListFromPermanentBySPID(udtSP.SPID, udtDB)

                ' Get Practice, Bank, Practice Bank Information
                Dim udtPracticeBLL As New PracticeBLL
                udtSP.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtSP.SPID, udtDB)

                Return udtSP

            Catch ex As Exception
                Throw ex

            End Try

        End Function

        Public Function GetServiceProviderPermanentProfileByERN(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderModel
            Dim drSP As SqlDataReader = Nothing
            Dim udtServiceProviderModel As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)
            Dim dtmEnrolmentDtm As Nullable(Of DateTime)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmDelistDtm As Nullable(Of DateTime)
            Dim strConsentPrintOption As String
            Dim udtGeneralFunction As New ComFunction.GeneralFunction
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProvider_get_byERN", prams, drSP)

                While drSP.Read()
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

                    If IsDBNull(drSP.Item("Delist_Dtm")) Then
                        dtmDelistDtm = Nothing
                    Else
                        dtmDelistDtm = Convert.ToDateTime(drSP.Item("Delist_Dtm"))
                    End If

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    ' Add [dtmDataInputDtm], [strDataInputBy], [dtmDataInputEffectDtm]
                    udtServiceProviderModel = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
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
                                                                            CType(drSP.Item("Record_Status"), String).Trim, _
                                                                            String.Empty, _
                                                                            CStr(IIf((drSP.Item("Remark") Is DBNull.Value), String.Empty, drSP.Item("Remark"))), _
                                                                            CType(drSP.Item("Submission_Method"), String).Trim, _
                                                                            CType(drSP.Item("Already_Joined_HA_PPI"), String), _
                                                                            CType(drSP.Item("Join_HA_PPI"), String), _
                                                                            CStr(IIf((drSP.Item("UnderModification") Is DBNull.Value), String.Empty, drSP.Item("UnderModification"))), _
                                                                            String.Empty, _
                                                                            CType(drSP.Item("Create_Dtm"), DateTime), _
                                                                            CType(drSP.Item("Create_By"), String), _
                                                                            CType(drSP.Item("Update_Dtm"), DateTime), _
                                                                            CType(drSP.Item("Update_By"), String), _
                                                                            dtmEffectiveDtm, _
                                                                            dtmDelistDtm, _
                                                                            CType(IIf(drSP.Item("TSMP") Is DBNull.Value, Nothing, drSP.Item("TSMP")), Byte()), _
                                                                            Nothing, _
                                                                            CStr(drSP.Item("Data_Input_By")).Trim(), _
                                                                            CDate(drSP.Item("Data_Input_Effective_Dtm")))
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]
                    If drSP.Item("ConsentPrintOption") Is DBNull.Value Then
                        udtGeneralFunction.getSystemParameter("DefaultConsentPrintOption", strConsentPrintOption, String.Empty)
                        udtServiceProviderModel.PrintOption = strConsentPrintOption
                    Else
                        udtServiceProviderModel.PrintOption = drSP.Item("ConsentPrintOption")
                    End If

                End While
                drSP.Close()
                Dim udtPracticeBLL As PracticeBLL = New PracticeBLL

                udtServiceProviderModel.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtServiceProviderModel.SPID, udtDB)

                Return udtServiceProviderModel
            Catch ex As Exception
                Throw ex
            Finally
                If Not drSP Is Nothing Then
                    drSP.Close()
                End If
            End Try
        End Function

        Public Function GetServiceProviderPermanentProfileWithMaintenanceBySPID(ByVal strSPID As String, ByVal udtDB As Database) As ServiceProviderModel
            Dim drSP As SqlDataReader = Nothing
            Dim udtServiceProviderModel As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)
            Dim dtmEnrolmentDtm As Nullable(Of DateTime)
            Dim dtmEffectiveDtm As Nullable(Of DateTime)
            Dim dtmDelistDtm As Nullable(Of DateTime)

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_ServiceProviderSPAccMaintenance_get_bySPID", prams, drSP)

                While drSP.Read()
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

                    If IsDBNull(drSP.Item("Delist_Dtm")) Then
                        dtmDelistDtm = Nothing
                    Else
                        dtmDelistDtm = Convert.ToDateTime(drSP.Item("Delist_Dtm"))
                    End If

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    ' Add [dtmDataInputDtm], [strDataInputBy], [dtmDataInputEffectDtm]
                    udtServiceProviderModel = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
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
                                                                            CType(drSP.Item("Email_Change"), String).Trim, _
                                                                            CType(drSP.Item("Record_Status"), String).Trim, _
                                                                            CStr(IIf((drSP.Item("Delist_Status") Is DBNull.Value), String.Empty, drSP.Item("Delist_Status"))), _
                                                                            CStr(IIf((drSP.Item("Remark") Is DBNull.Value), String.Empty, drSP.Item("Remark"))), _
                                                                            CType(drSP.Item("Submission_Method"), String).Trim, _
                                                                            CType(drSP.Item("Already_Joined_HA_PPI"), String), _
                                                                            CType(drSP.Item("Join_HA_PPI"), String), _
                                                                            CStr(IIf((drSP.Item("UnderModification") Is DBNull.Value), String.Empty, drSP.Item("UnderModification"))), _
                                                                            String.Empty, _
                                                                            CType(drSP.Item("Create_Dtm"), DateTime), _
                                                                            CType(drSP.Item("Create_By"), String), _
                                                                            CType(drSP.Item("Update_Dtm"), DateTime), _
                                                                            CType(drSP.Item("Update_By"), String), _
                                                                            dtmEffectiveDtm, _
                                                                            dtmDelistDtm, _
                                                                            CType(IIf(drSP.Item("TSMP") Is DBNull.Value, Nothing, drSP.Item("TSMP")), Byte()), _
                                                                            Nothing, _
                                                                            CStr(drSP.Item("Data_Input_By")).Trim(), _
                                                                            CDate(drSP.Item("Data_Input_Effective_Dtm")))
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                End While
                drSP.Close()
                Dim udtPracticeBLL As PracticeBLL = New PracticeBLL

                udtServiceProviderModel.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromPermanentMaintenanceBySPID(udtServiceProviderModel.SPID, udtDB)

                Return udtServiceProviderModel
            Catch ex As Exception
                Throw ex
            Finally
                If Not drSP Is Nothing Then
                    drSP.Close()
                End If
            End Try
        End Function

        Public Function GetServiceProviderStagingByERN_NoReader(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderModel

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

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    ' Add [dtmDataInputDtm], [strDataInputBy], [dtmDataInputEffectDtm]
                    udtServiceProviderModel = New ServiceProviderModel(CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
                                                                            dtmEnrolmentDtm, _
                                                                            CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))), _
                                                                            String.Empty, _
                                                                            CStr(IIf((drRaw.Item("SP_HKID") Is DBNull.Value), String.Empty, drRaw.Item("SP_HKID"))), _
                                                                            CStr(IIf((drRaw.Item("SP_Eng_Name") Is DBNull.Value), String.Empty, drRaw.Item("SP_Eng_Name"))), _
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
                                                                            CStr(IIf((drRaw.Item("Email_Changed") Is DBNull.Value), String.Empty, drRaw.Item("Email_Changed"))), _
                                                                            CType(drRaw.Item("Record_Status"), String).Trim, _
                                                                            String.Empty, _
                                                                            CStr(IIf((drRaw.Item("Remark") Is DBNull.Value), String.Empty, drRaw.Item("Remark"))), _
                                                                            CType(drRaw.Item("Submission_Method"), String).Trim, _
                                                                            CType(drRaw.Item("Already_Joined_HA_PPI"), String), _
                                                                            CType(drRaw.Item("Join_HA_PPI"), String), _
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
                                                                            "", _
                                                                            Nothing)
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                End If

                Return udtServiceProviderModel
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function GetServiceProviderStagingProfileByERN(ByVal strERN As String, ByVal udtDB As Database) As ServiceProviderModel
            Dim drSP As SqlDataReader = Nothing
            Dim udtServiceProviderModel As ServiceProviderModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)
            Dim dtmEnrolmentDtm As Nullable(Of DateTime)

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_ServiceProviderStaging_get_byERN", prams, drSP)

                While drSP.Read()
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

                    ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                    ' -------------------------------------------------------------------------
                    ' Add [dtmDataInputDtm], [strDataInputBy], [dtmDataInputEffectDtm]
                    udtServiceProviderModel = New ServiceProviderModel(CType(drSP.Item("Enrolment_Ref_No"), String).Trim, _
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
                                                                            CStr(IIf((drSP.Item("Email_Changed") Is DBNull.Value), String.Empty, drSP.Item("Email_Changed"))), _
                                                                            CType(drSP.Item("Record_Status"), String).Trim, _
                                                                            String.Empty, _
                                                                            CStr(IIf((drSP.Item("Remark") Is DBNull.Value), String.Empty, drSP.Item("Remark"))), _
                                                                            CType(drSP.Item("Submission_Method"), String).Trim, _
                                                                            CType(drSP.Item("Already_Joined_HA_PPI"), String), _
                                                                            CType(drSP.Item("Join_HA_PPI"), String), _
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
                                                                            "", _
                                                                            Nothing)
                    ' INT13-0028 - SP Amendment Report [End][Tommy L]

                End While
                drSP.Close()
                Dim udtPracticeBLL As PracticeBLL = New PracticeBLL

                If Not IsNothing(udtServiceProviderModel) Then
                    udtServiceProviderModel.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtServiceProviderModel.EnrolRefNo, udtDB)
                End If

                Return udtServiceProviderModel
            Catch ex As Exception
                Throw ex
            Finally
                If Not drSP Is Nothing Then
                    drSP.Close()
                End If
            End Try
        End Function

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Public Function GetServiceProviderDataEntrySearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal udtDB As Database) As DataTable
        Public Function GetServiceProviderDataEntrySearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal udtDB As Database, ByVal blnOverrideResultLimit As Boolean, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
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
                                                udtDB.MakeInParam("@status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, IIf(strStatus.Trim.Equals(String.Empty), DBNull.Value, strStatus.Trim))}

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'udtDB.RunProc("proc_ServiceProviderAll_get_bySPInfo", prams, dtResult)
                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_ServiceProviderAll_get_bySPInfo", prams, blnOverrideResultLimit, udtDB, blnForceUnlimitResult)

                'Return dtResult
                Return udtBLLSearchResult
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Public Function GetServiceProviderVettingSearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal strProgressStatus As String, ByVal udtDB As Database) As DataTable
        Public Function GetServiceProviderVettingSearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal strProgressStatus As String, ByVal udtDB As Database, ByVal blnOverrideResultLimit As Boolean, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
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
                                                udtDB.MakeInParam("@progress_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, IIf(strProgressStatus.Trim.Equals(String.Empty), DBNull.Value, strProgressStatus.Trim))}

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'udtDB.RunProc("proc_ServiceProviderStagingSPAccUpd_get_bySPInfo", prams, dtResult)
                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_ServiceProviderStagingSPAccUpd_get_bySPInfo", prams, blnOverrideResultLimit, udtDB, blnForceUnlimitResult)

                'Return dtResult
                Return udtBLLSearchResult
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Public Function GetServiceProviderEnquirySearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal udtDB As Database) As DataTable
        Public Function GetServiceProviderEnquirySearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal udtDB As Database, ByVal blnOverrideResultLimit As Boolean, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
            'Dim dtResult As DataTable = New DataTable
            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, IIf(strERN.Trim.Equals(String.Empty), DBNull.Value, strERN.Trim)), _
                                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(strSPID.Trim.Equals(String.Empty), DBNull.Value, strSPID.Trim)), _
                                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, IIf(strHKID.Trim.Equals(String.Empty), DBNull.Value, strHKID.Trim)), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, IIf(strEname.Trim.Equals(String.Empty), DBNull.Value, strEname.Trim)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, IIf(strPhone.Trim.Equals(String.Empty), DBNull.Value, strPhone.Trim)), _
                                                udtDB.MakeInParam("@service_category_code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, IIf(strServiceCategoryCode.Trim.Equals(String.Empty), DBNull.Value, strServiceCategoryCode.Trim))}

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'udtDB.RunProc("proc_ServiceProviderAllEnquiry_get_bySPInfo", prams, dtResult)
                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_ServiceProviderAllEnquiry_get_bySPInfo", prams, blnOverrideResultLimit, udtDB, blnForceUnlimitResult)

                'Return dtResult
                Return udtBLLSearchResult
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Catch ex As Exception
                Throw ex
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
                Throw ex
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
                Throw ex
            End Try

            Return byteRes

        End Function

        Public Function GetserviceProviderPermanentTentativeEmail(ByVal strSPID As String, ByVal udtDB As Database) As String
            Dim strRes As String = String.Empty

            Dim dt As New DataTable
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_ServiceProviderTentativeEmail_get_bySPID", prams, dt)

                If dt.Rows.Count > 0 Then
                    strRes = dt.Rows(0).Item("Tentative_Email")
                End If
            Catch ex As Exception
                Throw ex
            End Try

            Return strRes

        End Function

        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Public Function GetServiceProviderMaintenanceSearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal udtDB As Database) As DataTable
        Public Function GetServiceProviderMaintenanceSearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal udtDB As Database, Optional ByVal blnOverrideResultLimit As Boolean = False, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
            'Dim dtResult As DataTable = New DataTable
            Dim udtBLLSearchResult As BaseBLL.BLLSearchResult
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, IIf(strERN.Trim.Equals(String.Empty), DBNull.Value, strERN.Trim)), _
                                                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(strSPID.Trim.Equals(String.Empty), DBNull.Value, strSPID.Trim)), _
                                                udtDB.MakeInParam("@sp_hkid", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, IIf(strHKID.Trim.Equals(String.Empty), DBNull.Value, strHKID.Trim)), _
                                                udtDB.MakeInParam("@sp_eng_name", ServiceProviderModel.ENameDataType, ServiceProviderModel.ENameDataSize, IIf(strEname.Trim.Equals(String.Empty), DBNull.Value, strEname.Trim)), _
                                                udtDB.MakeInParam("@phone_daytime", ServiceProviderModel.PhoneDataType, ServiceProviderModel.PhoneDataSize, IIf(strPhone.Trim.Equals(String.Empty), DBNull.Value, strPhone.Trim)), _
                                                udtDB.MakeInParam("@service_category_code", ProfessionalModel.ServiceCategoryCodeDataType, ProfessionalModel.ServiceCategoryCodeDataSize, IIf(strServiceCategoryCode.Trim.Equals(String.Empty), DBNull.Value, strServiceCategoryCode.Trim))}

                ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
                ' -------------------------------------------------------------------------
                'udtDB.RunProc("proc_ServiceProviderSPAccMaintenance_get_bySPInfo", prams, dtResult)
                udtBLLSearchResult = BaseBLL.ExeSearchProc(strFunctionCode, "proc_ServiceProviderSPAccMaintenance_get_bySPInfo", prams, blnOverrideResultLimit, udtDB, blnForceUnlimitResult)

                'Return dtResult
                Return udtBLLSearchResult
                ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function AddServiceProviderParticularsToEnrolment(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean
            Try
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
                                                  udtDB.MakeInParam("@already_joined_ha_ppi", ServiceProviderModel.AlreadyJoinedHAPPIDataType, ServiceProviderModel.AlreadyJoinedHAPPIDataSize, IIf(udtServiceProviderModel.AlreadyJoinHAPPI.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.AlreadyJoinHAPPI)), _
                                                  udtDB.MakeInParam("@join_ha_ppi", ServiceProviderModel.JoinHAPPIDataType, ServiceProviderModel.JoinHAPPIDataSize, IIf(udtServiceProviderModel.JoinHAPPI.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.JoinHAPPI))}

                udtDB.RunProc("proc_ServiceProviderEnrolment_add", prams)
                Return True

            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddServiceProviderParticularsToEnrolmentBOTH(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database, ByVal strSchemeCode As String) As Boolean
            Try
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
                                                  udtDB.MakeInParam("@email", ServiceProviderModel.EmailDataType, ServiceProviderModel.ENameDataSize, udtServiceProviderModel.Email), _
                                                  udtDB.MakeInParam("@already_joined_ha_ppi", ServiceProviderModel.AlreadyJoinedHAPPIDataType, ServiceProviderModel.AlreadyJoinedHAPPIDataSize, IIf(udtServiceProviderModel.AlreadyJoinHAPPI.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.AlreadyJoinHAPPI)), _
                                                  udtDB.MakeInParam("@join_ha_ppi", ServiceProviderModel.JoinHAPPIDataType, ServiceProviderModel.JoinHAPPIDataSize, IIf(udtServiceProviderModel.JoinHAPPI.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.JoinHAPPI)), _
                                                  udtDB.MakeInParam("@scheme", SqlDbType.Char, 5, strSchemeCode)}

                udtDB.RunProc("proc_ServiceProviderEnrolmentBOTH_add", prams)
                Return True

            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddServiceProviderParticularsToStaging(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean
            Try
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
                                                udtDB.MakeInParam("@already_joined_ha_ppi", ServiceProviderModel.AlreadyJoinedHAPPIDataType, ServiceProviderModel.AlreadyJoinedHAPPIDataSize, IIf(udtServiceProviderModel.AlreadyJoinHAPPI.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.AlreadyJoinHAPPI)), _
                                                udtDB.MakeInParam("@join_ha_ppi", ServiceProviderModel.JoinHAPPIDataType, ServiceProviderModel.JoinHAPPIDataSize, IIf(udtServiceProviderModel.JoinHAPPI.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.JoinHAPPI)), _
                                                udtDB.MakeInParam("@application_printed", ServiceProviderModel.ApplicationPrintedDataType, ServiceProviderModel.ApplicationPrintedDataSize, IIf(udtServiceProviderModel.ApplicationPrinted.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.ApplicationPrinted)), _
                                                udtDB.MakeInParam("@create_by", ServiceProviderModel.CreateByDataType, ServiceProviderModel.CreateByDataSize, udtServiceProviderModel.CreateBy), _
                                                udtDB.MakeInParam("@update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, IIf(udtServiceProviderModel.UpdateBy.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.UpdateBy))}

                udtDB.RunProc("proc_ServiceProviderStaging_add", prams)
                Return True

            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddServiceProviderProfileToEnrolment(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtPracticeList As PracticeModelCollection, ByVal udtBankAcctList As BankAcctModelCollection, ByVal udtProfessionalList As ProfessionalModelCollection, ByVal udtDB As Database) As Boolean
            Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
            Dim udtBankAcctBLL As BankAcctBLL = New BankAcctBLL
            Dim udtProfessionalBLL As ProfessionalBLL = New ProfessionalBLL

            Dim blnRes As Boolean

            Try
                'comment for testing only
                If udtPracticeList Is Nothing Then
                    If AddServiceProviderParticularsToEnrolment(udtServiceProviderModel, udtDB) Then
                        blnRes = True
                    Else
                        blnRes = False
                    End If
                Else
                    If udtBankAcctList Is Nothing Then
                        If AddServiceProviderParticularsToEnrolment(udtServiceProviderModel, udtDB) And udtPracticeBLL.AddPracticeListToEnrolment(udtPracticeList, udtDB) And udtProfessionalBLL.AddProfessionalListToEnrolment(udtProfessionalList, udtDB) Then
                            blnRes = True
                        Else
                            blnRes = False
                        End If
                    Else
                        If AddServiceProviderParticularsToEnrolment(udtServiceProviderModel, udtDB) And udtPracticeBLL.AddPracticeListToEnrolment(udtPracticeList, udtDB) And udtBankAcctBLL.AddBankAcctListToEnrolment(udtBankAcctList, udtDB) And udtProfessionalBLL.AddProfessionalListToEnrolment(udtProfessionalList, udtDB) Then
                            blnRes = True
                        Else
                            blnRes = False
                        End If
                    End If
                End If

            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        'Remark by Clark for make the DM reborn
        'Public Function AddServiceProviderProfileToEnrolmentBOTH(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtPracticeList As PracticeModelCollection, ByVal udtBankAcctList As BankAcctModelCollection, ByVal udtProfessionalList As ProfessionalModelCollection, ByVal udtDB As Database, ByVal strSchemeCode As String) As Boolean
        '    Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        '    Dim udtBankAcctBLL As BankAcctBLL = New BankAcctBLL
        '    Dim udtProfessionalBLL As ProfessionalBLL = New ProfessionalBLL

        '    Dim blnRes As Boolean

        '    Try
        '        If udtPracticeList Is Nothing Then
        '            If AddServiceProviderParticularsToEnrolmentBOTH(udtServiceProviderModel, udtDB, strSchemeCode) Then
        '                blnRes = True
        '            Else
        '                blnRes = False
        '            End If
        '        Else
        '            If udtBankAcctList Is Nothing Then
        '                If AddServiceProviderParticularsToEnrolmentBOTH(udtServiceProviderModel, udtDB, strSchemeCode) And udtPracticeBLL.AddPracticeListToEnrolmentBOTH(udtPracticeList, udtDB, strSchemeCode) And udtProfessionalBLL.AddProfessionalListToEnrolmentBOTH(udtProfessionalList, udtDB, strSchemeCode) Then
        '                    blnRes = True
        '                Else
        '                    blnRes = False
        '                End If
        '            Else
        '                If AddServiceProviderParticularsToEnrolmentBOTH(udtServiceProviderModel, udtDB, strSchemeCode) And udtPracticeBLL.AddPracticeListToEnrolmentBOTH(udtPracticeList, udtDB, strSchemeCode) And udtBankAcctBLL.AddBankAcctListToEnrolmentBOTH(udtBankAcctList, udtDB, strSchemeCode) And udtProfessionalBLL.AddProfessionalListToEnrolmentBOTH(udtProfessionalList, udtDB, strSchemeCode) Then
        '                    blnRes = True
        '                Else
        '                    blnRes = False
        '                End If
        '            End If
        '        End If

        '    Catch ex As Exception
        '        Throw ex
        '        blnRes = False
        '    End Try

        '    Return blnRes
        'End Function

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
                Throw ex
                blnRes = False
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
                                                udtDB.MakeInParam("@email_changed", ServiceProviderModel.EmailChangedDataType, ServiceProviderModel.EmailChangedDataSize, IIf(udtServiceProviderModel.EmailChanged.Equals(EmailChanged.Unchanged), DBNull.Value, udtServiceProviderModel.EmailChanged)), _
                                                udtDB.MakeInParam("@record_status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, udtServiceProviderModel.RecordStatus), _
                                                udtDB.MakeInParam("@remark", ServiceProviderModel.RemarkDataType, ServiceProviderModel.RemarkDataSize, IIf(udtServiceProviderModel.Remark.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.Remark)), _
                                                udtDB.MakeInParam("@update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, IIf(udtServiceProviderModel.UpdateBy.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.UpdateBy)), _
                                                udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtServiceProviderModel.TSMP)}

                udtDB.RunProc("proc_ServiceProviderStaging_upd", prams)
                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw ex
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
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function UpdateServiceProviderStagingPPIePRStatus(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtServiceProviderModel.EnrolRefNo), _
                                                udtDB.MakeInParam("@already_joined_ha_ppi", ServiceProviderModel.AlreadyJoinedHAPPIDataType, ServiceProviderModel.AlreadyJoinedHAPPIDataSize, udtServiceProviderModel.AlreadyJoinHAPPI), _
                                                udtDB.MakeInParam("@join_ha_ppi", ServiceProviderModel.JoinHAPPIDataType, ServiceProviderModel.JoinHAPPIDataSize, udtServiceProviderModel.JoinHAPPI), _
                                                udtDB.MakeInParam("@update_by", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, udtServiceProviderModel.UpdateBy), _
                                                udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtServiceProviderModel.TSMP)}
                udtDB.RunProc("proc_ServiceProviderStaging_upd_PPIePR", prams)

                blnRes = True

            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function DeleteServiceProviderEnrolmentProfile(ByVal strERN As String, ByVal udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}

                udtDB.RunProc("proc_ServiceProviderEnrolmentProfile_del", prams)
                Return True
            Catch ex As Exception
                Throw ex
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
                '                                udtDB.MakeInParam("@Delist_Status", ServiceProviderModel.DelistStatusDataType, ServiceProviderModel.DelistStatusDataSize, udtServiceProviderModel.DelistStatus), _
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
                Throw ex
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
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Sub DeleteServiceProviderStagingByKey(ByRef udtDB As Database, ByVal strERN As String, ByVal TSMP As Byte(), ByVal blnCheckTSMP As Boolean)
            Try
                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@Enrolment_Ref_No", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                    udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, TSMP), _
                    udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP)}

                udtDB.RunProc("proc_ServiceProviderStaging_del_ByKey", params)

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function AddServiceProviderProfileToPermanent(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtPracticeList As PracticeModelCollection, ByVal udtBankAcctList As BankAcctModelCollection, ByVal udtProfessionalList As ProfessionalModelCollection, ByVal udtSchemeInformationCollection As SchemeInformationModelCollection, ByVal udtDB As Database) As Boolean
            Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
            Dim udtBankAcctBLL As BankAcctBLL = New BankAcctBLL
            Dim udtProfessionalBLL As ProfessionalBLL = New ProfessionalBLL
            Dim udtSchemeInformationBLL As SchemeInformationBLL = New SchemeInformationBLL

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
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function AddServiceProviderParticularsToPermanent(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean
            Try
                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                ' Add [@data_input_dtm], [@data_input_by]
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
                                                udtDB.MakeInParam("@already_joined_ha_ppi", ServiceProviderModel.AlreadyJoinedHAPPIDataType, ServiceProviderModel.AlreadyJoinedHAPPIDataSize, IIf(udtServiceProviderModel.AlreadyJoinHAPPI.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.AlreadyJoinHAPPI)), _
                                                udtDB.MakeInParam("@join_ha_ppi", ServiceProviderModel.JoinHAPPIDataType, ServiceProviderModel.JoinHAPPIDataSize, IIf(udtServiceProviderModel.JoinHAPPI.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.JoinHAPPI)), _
                                                udtDB.MakeInParam("@application_printed", ServiceProviderModel.ApplicationPrintedDataType, ServiceProviderModel.ApplicationPrintedDataSize, IIf(udtServiceProviderModel.ApplicationPrinted.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.ApplicationPrinted)), _
                                                udtDB.MakeInParam("@create_by", ServiceProviderModel.CreateByDataType, ServiceProviderModel.CreateByDataSize, udtServiceProviderModel.CreateBy), _
                                                udtDB.MakeInParam("@update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, IIf(udtServiceProviderModel.UpdateBy.Equals(String.Empty), DBNull.Value, udtServiceProviderModel.UpdateBy)), _
                                                udtDB.MakeInParam("@data_input_by", ServiceProviderModel.DataInputByDataType, ServiceProviderModel.DataInputByDataSize, udtServiceProviderModel.DataInputBy)}
                ' INT13-0028 - SP Amendment Report [End][Tommy L]

                udtDB.RunProc("proc_ServiceProviderPermanent_add", prams)
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Sub UpdatePassword(ByVal strSPID As String, ByVal strSPPassword As String, ByVal tsmp As Byte(), ByRef db As Database)

            Try
                Dim parms() As SqlParameter = { _
                    db.MakeInParam("@sp_ID", SqlDbType.Char, 8, strSPID), _
                    db.MakeInParam("@sp_password", SqlDbType.VarChar, 100, strSPPassword), _
                    db.MakeInParam("@tsmp", SqlDbType.Timestamp, 16, tsmp) _
                }
                db.RunProc("proc_HCSPUserAC_upd_Password", parms)
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
            End Try
        End Sub

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
                Throw ex
            End Try
        End Function

        Public Function UpdateServiceProviderEmailActivationCode(ByRef udtDB As Database, ByVal strSPID As String, ByVal strUserID As String, ByVal strHashedActivationCode As String, ByVal TSMP As Byte(), ByVal blnCheckTSMP As Boolean) As Boolean
            Dim blnRes As Boolean = False
            Try

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID), _
                                    udtDB.MakeInParam("@Update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, strUserID), _
                                    udtDB.MakeInParam("@Activation_Code", ServiceProviderModel.Activation_CodeDataType, ServiceProviderModel.Activation_CodeDataSize, strHashedActivationCode), _
                                    udtDB.MakeInParam("@TSMP", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, TSMP), _
                                    udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP)}
                udtDB.RunProc("proc_ServiceProvider_upd_ActivationCode", prams)

                blnRes = True

            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function


        Public Function UpdateServiceProviderUnderModificationAndRecordStatus(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtDB As Database) As Boolean

            Dim blnRes As Boolean = False
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtServiceProviderModel.SPID), _
                                                udtDB.MakeInParam("@Record_Status", ServiceProviderModel.RecordStatusDataType, ServiceProviderModel.RecordStatusDataSize, udtServiceProviderModel.RecordStatus), _
                                                udtDB.MakeInParam("@Delist_Status", ServiceProviderModel.DelistStatusDataType, ServiceProviderModel.DelistStatusDataSize, udtServiceProviderModel.DelistStatus), _
                                                udtDB.MakeInParam("@UnderModification", ServiceProviderModel.UnderModificationDataType, ServiceProviderModel.UnderModificationDataSize, IIf(udtServiceProviderModel.UnderModification = Nothing, DBNull.Value, udtServiceProviderModel.UnderModification)), _
                                                udtDB.MakeInParam("@Update_By", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, udtServiceProviderModel.UpdateBy), _
                                                udtDB.MakeInParam("@TSMP", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtServiceProviderModel.TSMP)}
                udtDB.RunProc("proc_ServiceProvider_upd_UnderModifyAndRecordStatus", prams)

                blnRes = True

            Catch ex As Exception
                Throw ex
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

    End Class
End Namespace

