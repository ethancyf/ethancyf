Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Component.Address
Imports Common.Component.BankAcct
Imports Common.Component.Professional

Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.MedicalOrganization
Imports Common.Component.ERNProcessed
Imports Common.Component.Scheme

Namespace Component.Practice
    Public Class PracticeBLL

        Public Const SESS_PRACTICE As String = "Practice"

        Public Function GetPracticeCollection() As PracticeModelCollection
            Dim udtPracticeModelCollection As PracticeModelCollection
            udtPracticeModelCollection = Nothing

            If Not IsNothing(HttpContext.Current.Session(SESS_PRACTICE)) Then
                Try
                    udtPracticeModelCollection = CType(HttpContext.Current.Session(SESS_PRACTICE), PracticeModelCollection)
                Catch ex As Exception
                    Throw New Exception("Invalid Session Practice")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
            Return udtPracticeModelCollection
        End Function

        Public Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_PRACTICE) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ClearSession()
            HttpContext.Current.Session(SESS_PRACTICE) = Nothing
        End Sub


        Public Sub SaveToSession(ByRef udtPracticeCollection As PracticeModelCollection)
            HttpContext.Current.Session(SESS_PRACTICE) = udtPracticeCollection
        End Sub

        Public Sub Clone(ByRef udtNewPracticeModel As PracticeModel, ByRef udtOldPracticeModel As PracticeModel)
            udtNewPracticeModel.SPID = udtOldPracticeModel.SPID
            udtNewPracticeModel.EnrolRefNo = udtOldPracticeModel.EnrolRefNo
            udtNewPracticeModel.DisplaySeq = udtOldPracticeModel.DisplaySeq
            udtNewPracticeModel.MODisplaySeq = udtOldPracticeModel.MODisplaySeq
            udtNewPracticeModel.PracticeName = udtOldPracticeModel.PracticeName
            udtNewPracticeModel.PracticeNameChi = udtOldPracticeModel.PracticeNameChi
            'udtNewPracticeModel.PracticeType = udtOldPracticeModel.PracticeType
            udtNewPracticeModel.PracticeAddress = udtOldPracticeModel.PracticeAddress
            udtNewPracticeModel.ProfessionalSeq = udtOldPracticeModel.ProfessionalSeq
            udtNewPracticeModel.RecordStatus = udtOldPracticeModel.RecordStatus
            udtNewPracticeModel.SubmitMethod = udtOldPracticeModel.SubmitMethod
            'udtNewPracticeModel.DelistStatus = udtOldPracticeModel.DelistStatus
            udtNewPracticeModel.Remark = udtOldPracticeModel.Remark
            udtNewPracticeModel.CreateDtm = udtOldPracticeModel.CreateDtm
            udtNewPracticeModel.CreateBy = udtOldPracticeModel.CreateBy
            udtNewPracticeModel.UpdateDtm = udtOldPracticeModel.UpdateDtm
            udtNewPracticeModel.UpdateBy = udtOldPracticeModel.UpdateBy
            'udtNewPracticeModel.EffectiveDtm = udtOldPracticeModel.EffectiveDtm
            'udtNewPracticeModel.DelistDtm = udtOldPracticeModel.DelistDtm
            udtNewPracticeModel.TSMP = udtOldPracticeModel.TSMP

            udtNewPracticeModel.Professional = udtOldPracticeModel.Professional
            udtNewPracticeModel.BankAcct = udtOldPracticeModel.BankAcct
            udtNewPracticeModel.PracticeSchemeInfoList = udtOldPracticeModel.PracticeSchemeInfoList
        End Sub


        'Database Connection
        'Private objDB As Database = New Database()

        'Public Property DB() As Database
        '    Get
        '        Return objDB
        '    End Get
        '    Set(ByVal Value As Database)
        '        objDB = Value
        '    End Set
        'End Property

        Public Sub New()

        End Sub

        Public Function GetPracticeBankAcctListFromEnrolmentByERN(ByVal strERN As String, ByVal udtDB As Database) As PracticeModelCollection
            'Dim drPracticeList As SqlDataReader = Nothing
            Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim intMODisplaySeq As Nullable(Of Integer)

            Dim dtRaw As New DataTable

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_PracticeBankAccountEnrolment_get_byERN", prams, dtRaw)

                For Each drPracticeList As DataRow In dtRaw.Rows
                    If IsDBNull(drPracticeList.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drPracticeList.Item("Address_Code")))
                    End If

                    If IsDBNull(drPracticeList.Item("Bank_Display_Seq")) Then
                        intBankDisplaySeq = Nothing
                    Else
                        intBankDisplaySeq = CInt(drPracticeList.Item("Bank_Display_Seq"))
                    End If

                    If IsDBNull(drPracticeList.Item("Display_Seq")) Then
                        intPracticeDisplaySeq = Nothing
                    Else
                        intPracticeDisplaySeq = CInt(drPracticeList.Item("Display_Seq"))
                    End If

                    If IsDBNull(drPracticeList.Item("MO_Display_Seq")) Then
                        intMODisplaySeq = Nothing
                    Else
                        intMODisplaySeq = CInt(drPracticeList.Item("MO_Display_Seq"))
                    End If

                    ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                    ' Add [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
                    udtPracticeModel = New PracticeModel(String.Empty, _
                                                            CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
                                                            intPracticeDisplaySeq, _
                                                            intMODisplaySeq, _
                                                            CType(drPracticeList.Item("Practice_Name"), String).Trim, _
                                                            CStr(IIf((drPracticeList.Item("Practice_Name_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Name_Chi"))), _
                                                            New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))), _
                                                                CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))), _
                                                                CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))), _
                                                                CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))), _
                                                                CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))), _
                                                                CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))), _
                                                                intAddressCode), _
                                                            CInt(drPracticeList.Item("Professional_Seq")), _
                                                            String.Empty, _
                                                            SubmitChannel.Electronic, _
                                                            String.Empty, _
                                                            CStr(drPracticeList.Item("Phone_Daytime")), _
                                                            Nothing, _
                                                            String.Empty, _
                                                            Nothing, _
                                                            String.Empty, _
                                                            Nothing, _
                                                            YesNo.No, _
                                                            String.Empty, _
                                                            String.Empty, _
                                                            New BankAcctModel(String.Empty, _
                                                                CStr(IIf((drPracticeList.Item("Enrolment_Ref_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Enrolment_Ref_No"))), _
                                                                intBankDisplaySeq, _
                                                                intPracticeDisplaySeq, _
                                                                CStr(IIf((drPracticeList.Item("Bank_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Name"))), _
                                                                CStr(IIf((drPracticeList.Item("Branch_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Branch_Name"))), _
                                                                CStr(IIf((drPracticeList.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Acc_Holder"))), _
                                                                CStr(IIf((drPracticeList.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Account_No"))), _
                                                                String.Empty, _
                                                                SubmitChannel.Electronic, _
                                                                String.Empty, _
                                                                Nothing, _
                                                                String.Empty, _
                                                                Nothing, _
                                                                String.Empty, _
                                                                Nothing, _
                                                                YesNo.No), _
                                                            New ProfessionalModel(String.Empty, _
                                                                CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
                                                                CInt(drPracticeList.Item("Professional_Seq")), _
                                                                CType(drPracticeList.Item("Service_Category_Code"), String).Trim, _
                                                                CType(drPracticeList.Item("Registration_Code"), String).Trim, _
                                                                String.Empty, _
                                                                Nothing, _
                                                                Nothing), _
                                                                Nothing)
                    ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

                    ' Get Practice Scheme Information
                    Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
                    udtPracticeModel.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListEnrolmentByERNDisplaySeq(udtPracticeModel.EnrolRefNo, udtPracticeModel.DisplaySeq, udtDB)

                    udtPracticeModelCollection.Add(udtPracticeModel)
                Next

                If Not IsNothing(udtPracticeModelCollection) Then
                    udtPracticeModelCollection.DuplicatePracticeEName()
                End If

                Return udtPracticeModelCollection

            Catch ex As Exception
                Throw ex

            End Try

        End Function

        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function GetPracticeBankAcctListFromEnrolmentByERNInHCVU(ByVal strERN As String, ByVal udtDB As Database) As PracticeModelCollection
            Return GetPracticeBankAcctListFromCopyByERNInHCVU(strERN, EnumEnrolCopy.Enrolment, udtDB)
        End Function
        ' CRE12-001 eHS and PCD integration [End][Koala]

        Public Function GetPracticeBankAcctListFromCopyByERNInHCVU(ByVal strERN As String, ByVal enumEnrolCopy As EnumEnrolCopy, ByVal udtDB As Database) As PracticeModelCollection
            'Dim drPracticeList As SqlDataReader = Nothing
            Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim intMODisplaySeq As Nullable(Of Integer)

            Dim dtRaw As New DataTable

            Try
                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                Select Case enumEnrolCopy
                    Case enumEnrolCopy.Enrolment
                        udtDB.RunProc("proc_PracticeBankAccountEnrolment_get_byERN", prams, dtRaw)
                    Case enumEnrolCopy.Original
                        udtDB.RunProc("proc_PracticeBankAccountOriginal_get_byERN", prams, dtRaw)
                End Select
                ' CRE12-001 eHS and PCD integration [End][Koala]

                For Each drPracticeList As DataRow In dtRaw.Rows
                    If IsDBNull(drPracticeList.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drPracticeList.Item("Address_Code")))
                    End If

                    If IsDBNull(drPracticeList.Item("Bank_Display_Seq")) Then
                        intBankDisplaySeq = Nothing
                    Else
                        intBankDisplaySeq = CInt(drPracticeList.Item("Bank_Display_Seq"))
                    End If

                    If IsDBNull(drPracticeList.Item("Display_Seq")) Then
                        intPracticeDisplaySeq = Nothing
                    Else
                        intPracticeDisplaySeq = CInt(drPracticeList.Item("Display_Seq"))
                    End If

                    If IsDBNull(drPracticeList.Item("MO_Display_Seq")) Then
                        intMODisplaySeq = Nothing
                    Else
                        intMODisplaySeq = CInt(drPracticeList.Item("MO_Display_Seq"))
                    End If

                    ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                    ' Add [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
                    udtPracticeModel = New PracticeModel(String.Empty, _
                                                            CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
                                                            intPracticeDisplaySeq, _
                                                            intMODisplaySeq, _
                                                            CType(drPracticeList.Item("Practice_Name"), String).Trim, _
                                                            CStr(IIf((drPracticeList.Item("Practice_Name_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Name_Chi"))), _
                                                            New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))), _
                                                                CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))), _
                                                                CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))), _
                                                                CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))), _
                                                                CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))), _
                                                                CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))), _
                                                                intAddressCode), _
                                                            CInt(drPracticeList.Item("Professional_Seq")), _
                                                            PracticeModel.PracticeRecordStatus.Active, _
                                                            SubmitChannel.Electronic, _
                                                            String.Empty, _
                                                            CStr(drPracticeList.Item("Phone_Daytime")), _
                                                            Nothing, _
                                                            String.Empty, _
                                                            Nothing, _
                                                            String.Empty, _
                                                             Nothing, _
                                                            YesNo.No, _
                                                            String.Empty, _
                                                            String.Empty, _
                                                            New BankAcctModel(String.Empty, _
                                                                CStr(IIf((drPracticeList.Item("Enrolment_Ref_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Enrolment_Ref_No"))), _
                                                                intBankDisplaySeq, _
                                                                intPracticeDisplaySeq, _
                                                                CStr(IIf((drPracticeList.Item("Bank_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Name"))), _
                                                                CStr(IIf((drPracticeList.Item("Branch_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Branch_Name"))), _
                                                                CStr(IIf((drPracticeList.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Acc_Holder"))), _
                                                                CStr(IIf((drPracticeList.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Account_No"))), _
                                                                String.Empty, _
                                                                SubmitChannel.Electronic, _
                                                                String.Empty, _
                                                                Nothing, _
                                                                String.Empty, _
                                                                Nothing, _
                                                                String.Empty, _
                                                                Nothing, _
                                                                YesNo.No), _
                                                            New ProfessionalModel(String.Empty, _
                                                                CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
                                                                CInt(drPracticeList.Item("Professional_Seq")), _
                                                                CType(drPracticeList.Item("Service_Category_Code"), String).Trim, _
                                                                CType(drPracticeList.Item("Registration_Code"), String).Trim, _
                                                                String.Empty, _
                                                                Nothing, _
                                                                Nothing), _
                                                                Nothing)

                    ' Get Practice Scheme Information
                    Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
                    udtPracticeModel.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListCopyByERNDisplaySeqInHCVU(udtPracticeModel.EnrolRefNo, udtPracticeModel.DisplaySeq, enumEnrolCopy, udtDB)

                    udtPracticeModelCollection.Add(udtPracticeModel)
                Next

                If Not IsNothing(udtPracticeModelCollection) Then
                    udtPracticeModelCollection.DuplicatePracticeEName()
                End If

                Return udtPracticeModelCollection

            Catch ex As Exception
                Throw ex

            End Try

        End Function

        Public Function GetPracticeBankAcctListFromPermanentBySPID(ByVal strSPID As String, ByVal udtDB As Database, Optional ByVal blnCheckServiceDate As Boolean = False, Optional ByVal dtServiceDate As DateTime = Nothing) As PracticeModelCollection
            Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
            Dim udtPracticeModel As PracticeModel = Nothing

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim intMODisplaySeq As Nullable(Of Integer)

            Dim btyBankTsmp As Byte()
            Dim btyPracticeTsmp As Byte()

            Dim dtRaw As New DataTable
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_PracticeBankAccount_get_bySPID", prams, dtRaw)

                For i As Integer = 0 To dtRaw.Rows.Count - 1
                    Dim drRaw As DataRow = dtRaw.Rows(i)

                    If IsDBNull(drRaw.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drRaw.Item("Address_Code")))
                    End If

                    If IsDBNull(drRaw.Item("Bank_Display_Seq")) Then
                        intBankDisplaySeq = Nothing
                    Else
                        intBankDisplaySeq = CInt(drRaw.Item("Bank_Display_Seq"))
                    End If

                    If IsDBNull(drRaw.Item("Display_Seq")) Then
                        intPracticeDisplaySeq = Nothing
                    Else
                        intPracticeDisplaySeq = CInt(drRaw.Item("Display_Seq"))
                    End If

                    If IsDBNull(drRaw.Item("MO_Display_Seq")) Then
                        intMODisplaySeq = Nothing
                    Else
                        intMODisplaySeq = CInt(drRaw.Item("MO_Display_Seq"))
                    End If

                    If drRaw.IsNull("Bank_TSMP") Then
                        btyBankTsmp = Nothing
                    Else
                        btyBankTsmp = CType(drRaw.Item("Bank_TSMP"), Byte())
                    End If

                    If drRaw.IsNull("Practice_TSMP") Then
                        btyPracticeTsmp = Nothing
                    Else
                        btyPracticeTsmp = CType(drRaw.Item("Practice_TSMP"), Byte())
                    End If

                    ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                    ' Add [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
                    udtPracticeModel = New PracticeModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                            String.Empty, _
                                                            intPracticeDisplaySeq, _
                                                            intMODisplaySeq, _
                                                            CType(drRaw.Item("Practice_Name"), String).Trim, _
                                                            CStr(IIf((drRaw.Item("Practice_Name_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Name_Chi"))), _
                                                            New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))), _
                                                                CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))).Trim, _
                                                                CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))).Trim, _
                                                                intAddressCode), _
                                                            CInt(drRaw.Item("Professional_Seq")), _
                                                            CStr(IIf((drRaw.Item("Practice_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Record_Status"))).Trim, _
                                                            CStr(IIf((drRaw.Item("Practice_Submission_Method") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Submission_Method"))).Trim, _
                                                            CStr(IIf((drRaw.Item("Practice_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Remark"))).Trim, _
                                                            CStr(drRaw.Item("Phone_Daytime")), _
                                                            CType(drRaw.Item("Practice_Create_Dtm"), DateTime), _
                                                            CStr(IIf((drRaw.Item("Practice_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Create_By"))).Trim, _
                                                            CType(drRaw.Item("Practice_Update_Dtm"), DateTime), _
                                                            CStr(IIf((drRaw.Item("Practice_Update_By") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Update_By"))).Trim, _
                                                            btyPracticeTsmp, _
                                                            CStr(drRaw.Item("Mobile_Clinic")).Trim, _
                                                            CStr(IIf((drRaw.Item("Remarks_Desc") Is DBNull.Value), String.Empty, drRaw.Item("Remarks_Desc"))).Trim, _
                                                            CStr(IIf((drRaw.Item("Remarks_Desc_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Remarks_Desc_Chi"))).Trim, _
                                                            New BankAcctModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                                String.Empty, _
                                                                intBankDisplaySeq, _
                                                                intPracticeDisplaySeq, _
                                                                CStr(IIf((drRaw.Item("Bank_Name") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Name"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Branch_Name") Is DBNull.Value), String.Empty, drRaw.Item("Branch_Name"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Acc_Holder"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Account_No"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Bank_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Record_Status"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Bank_Submission_Method") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Submission_Method"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Bank_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Remark"))).Trim, _
                                                                Convert.ToDateTime(IIf(drRaw.Item("Bank_Create_Dtm") Is DBNull.Value, Nothing, drRaw.Item("Bank_Create_Dtm"))), _
                                                                CStr(IIf((drRaw.Item("Bank_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Create_By"))).Trim, _
                                                                Convert.ToDateTime(IIf(drRaw.Item("Bank_Update_Dtm") Is DBNull.Value, Nothing, drRaw.Item("Bank_Update_Dtm"))), _
                                                                CStr(IIf((drRaw.Item("Bank_Update_By") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Update_By"))).Trim, _
                                                                btyBankTsmp, _
                                                                CStr(IIf((drRaw.Item("Bank_IsFreeTextFormat") Is DBNull.Value), String.Empty, drRaw.Item("Bank_IsFreeTextFormat"))).Trim), _
                                                            New ProfessionalModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                                String.Empty, _
                                                                CInt(drRaw.Item("Professional_Seq")), _
                                                                CType(drRaw.Item("Service_Category_Code"), String).Trim, _
                                                                CType(drRaw.Item("Registration_Code"), String).Trim, _
                                                                CStr(IIf((drRaw.Item("Professional_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Professional_Record_Status"))).Trim, _
                                                                CType(drRaw.Item("Professional_Create_Dtm"), DateTime), _
                                                                CStr(IIf((drRaw.Item("Professional_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Professional_Create_By"))).Trim), _
                                                                Nothing)
                    ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

                    udtPracticeModelCollection.Add(udtPracticeModel)
                Next

                If dtRaw.Rows.Count > 0 Then

                    ' Get Practice Scheme Information
                    Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
                    Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection
                    If blnCheckServiceDate = False Then
                        udtPracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPID(udtPracticeModel.SPID, udtDB)
                    Else
                        udtPracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPID(udtPracticeModel.SPID, udtDB, True, dtServiceDate)
                    End If

                    For Each udtPractice As PracticeModel In udtPracticeModelCollection.Values
                        Dim udtNewPracticeSchemeInfoList As New PracticeSchemeInfoModelCollection
                        For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                            If udtPractice.DisplaySeq = udtPracticeSchemeInfo.PracticeDisplaySeq Then
                                udtNewPracticeSchemeInfoList.Add(New PracticeSchemeInfoModel(udtPracticeSchemeInfo))
                            End If
                        Next
                        udtPractice.PracticeSchemeInfoList = udtNewPracticeSchemeInfoList
                    Next

                End If


                If Not IsNothing(udtPracticeModelCollection) Then
                    udtPracticeModelCollection.DuplicatePracticeEName()
                End If

                Return udtPracticeModelCollection

            Catch ex As Exception
                Throw ex

            End Try

        End Function

        Public Function GetPracticeBankAcctListFromPermanentMaintenanceBySPID(ByVal strSPID As String, ByVal udtDB As Database) As PracticeModelCollection
            ' Dim drPracticeList As SqlDataReader = Nothing
            Dim udtPracticeModelCollection As New PracticeModelCollection
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim intMOPracticeDisplaySeq As Nullable(Of Integer)

            Dim btyBankTsmp As Byte()
            Dim btyPracticeTsmp As Byte()

            Try
                Dim dt As New DataTable

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_PracticeBankAccountSPAccMaintenance_get_bySPID", prams, dt)

                For Each r As DataRow In dt.Rows
                    If Not IsNothing(udtPracticeModelCollection(CInt(r("Display_Seq")))) Then Continue For

                    If IsDBNull(r("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((r("Address_Code")))
                    End If

                    If IsDBNull(r("Bank_Display_Seq")) Then
                        intBankDisplaySeq = Nothing
                    Else
                        intBankDisplaySeq = CInt(r("Bank_Display_Seq"))
                    End If

                    If IsDBNull(r("Display_Seq")) Then
                        intPracticeDisplaySeq = Nothing
                    Else
                        intPracticeDisplaySeq = CInt(r("Display_Seq"))
                    End If

                    If IsDBNull(r("MO_Display_Seq")) Then
                        intMOPracticeDisplaySeq = Nothing
                    Else
                        intMOPracticeDisplaySeq = CInt(r("MO_Display_Seq"))
                    End If

                    If r("Bank_TSMP") Is DBNull.Value Then
                        btyBankTsmp = Nothing
                    Else
                        btyBankTsmp = CType(r("Bank_TSMP"), Byte())
                    End If

                    If r("Practice_TSMP") Is DBNull.Value Then
                        btyPracticeTsmp = Nothing
                    Else
                        btyPracticeTsmp = CType(r("Practice_TSMP"), Byte())
                    End If

                    ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                    ' Add [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
                    udtPracticeModel = New PracticeModel(CStr(IIf((r("SP_ID") Is DBNull.Value), String.Empty, r("SP_ID"))).Trim, _
                                                            String.Empty, _
                                                            intPracticeDisplaySeq, _
                                                            intMOPracticeDisplaySeq, _
                                                            CType(r("Practice_Name"), String).Trim, _
                                                            CStr(IIf((r("Practice_Name_Chi") Is DBNull.Value), String.Empty, r("Practice_Name_Chi"))), _
                                                            New AddressModel(CStr(IIf((r("Room") Is DBNull.Value), String.Empty, r("Room"))), _
                                                                CStr(IIf((r("Floor") Is DBNull.Value), String.Empty, r("Floor"))).Trim, _
                                                                CStr(IIf((r("Block") Is DBNull.Value), String.Empty, r("Block"))).Trim, _
                                                                CStr(IIf((r("Building") Is DBNull.Value), String.Empty, r("Building"))).Trim, _
                                                                CStr(IIf((r("Building_Chi") Is DBNull.Value), String.Empty, r("Building_Chi"))).Trim, _
                                                                CStr(IIf((r("District") Is DBNull.Value), String.Empty, r("District"))).Trim, _
                                                                intAddressCode), _
                                                            CInt(r("Professional_Seq")), _
                                                            CStr(IIf((r("Practice_Record_Status") Is DBNull.Value), String.Empty, r("Practice_Record_Status"))).Trim, _
                                                            CStr(IIf((r("Practice_Submission_Method") Is DBNull.Value), String.Empty, r("Practice_Submission_Method"))).Trim, _
                                                            CStr(IIf((r("Practice_Remark") Is DBNull.Value), String.Empty, r("Practice_Remark"))).Trim, _
                                                            CStr(r("Phone_Daytime")), _
                                                            CType(r("Practice_Create_Dtm"), DateTime), _
                                                            CStr(IIf((r("Practice_Create_By") Is DBNull.Value), String.Empty, r("Practice_Create_By"))).Trim, _
                                                            CType(r("Practice_Update_Dtm"), DateTime), _
                                                            CStr(IIf((r("Practice_Update_By") Is DBNull.Value), String.Empty, r("Practice_Update_By"))).Trim, _
                                                            btyPracticeTsmp, _
                                                            CStr(r.Item("Mobile_Clinic")).Trim, _
                                                            CStr(IIf((r.Item("Remarks_Desc") Is DBNull.Value), String.Empty, r.Item("Remarks_Desc"))).Trim, _
                                                            CStr(IIf((r.Item("Remarks_Desc_Chi") Is DBNull.Value), String.Empty, r.Item("Remarks_Desc_Chi"))).Trim, _
                                                            New BankAcctModel(CStr(IIf((r("SP_ID") Is DBNull.Value), String.Empty, r("SP_ID"))).Trim, _
                                                                String.Empty, _
                                                                intBankDisplaySeq, _
                                                                intPracticeDisplaySeq, _
                                                                CStr(IIf((r("Bank_Name") Is DBNull.Value), String.Empty, r("Bank_Name"))).Trim, _
                                                                CStr(IIf((r("Branch_Name") Is DBNull.Value), String.Empty, r("Branch_Name"))).Trim, _
                                                                CStr(IIf((r("Bank_Acc_Holder") Is DBNull.Value), String.Empty, r("Bank_Acc_Holder"))).Trim, _
                                                                CStr(IIf((r("Bank_Account_No") Is DBNull.Value), String.Empty, r("Bank_Account_No"))).Trim, _
                                                                CStr(IIf((r("Bank_Record_Status") Is DBNull.Value), String.Empty, r("Bank_Record_Status"))).Trim, _
                                                                CStr(IIf((r("Bank_Submission_Method") Is DBNull.Value), String.Empty, r("Bank_Submission_Method"))).Trim, _
                                                                CStr(IIf((r("Bank_Remark") Is DBNull.Value), String.Empty, r("Bank_Remark"))).Trim, _
                                                                Convert.ToDateTime(IIf(r("Bank_Create_Dtm") Is DBNull.Value, Nothing, r("Bank_Create_Dtm"))), _
                                                                CStr(IIf((r("Bank_Create_By") Is DBNull.Value), String.Empty, r("Bank_Create_By"))).Trim, _
                                                                Convert.ToDateTime(IIf(r("Bank_Update_Dtm") Is DBNull.Value, Nothing, r("Bank_Update_Dtm"))), _
                                                                CStr(IIf((r("Bank_Update_By") Is DBNull.Value), String.Empty, r("Bank_Update_By"))).Trim, _
                                                                btyBankTsmp, _
                                                                CStr(IIf((r("Bank_IsFreeTextFormat") Is DBNull.Value), String.Empty, r("Bank_IsFreeTextFormat"))).Trim), _
                                                            New ProfessionalModel(CStr(IIf((r("SP_ID") Is DBNull.Value), String.Empty, r("SP_ID"))).Trim, _
                                                                String.Empty, _
                                                                CInt(r("Professional_Seq")), _
                                                                CType(r("Service_Category_Code"), String).Trim, _
                                                                CType(r("Registration_Code"), String).Trim, _
                                                                CStr(IIf((r("Professional_Record_Status") Is DBNull.Value), String.Empty, r("Professional_Record_Status"))).Trim, _
                                                                CType(r("Professional_Create_Dtm"), DateTime), _
                                                                CStr(IIf((r("Professional_Create_By") Is DBNull.Value), String.Empty, r("Professional_Create_By"))).Trim), _
                                                                Nothing)
                    ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

                    ' Get Practice Scheme Information
                    Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
                    udtPracticeModel.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPIDPracticeDisplaySeq(udtPracticeModel.SPID, udtPracticeModel.DisplaySeq, udtDB)

                    udtPracticeModelCollection.Add(udtPracticeModel)

                Next

                If Not IsNothing(udtPracticeModelCollection) Then
                    udtPracticeModelCollection.DuplicatePracticeEName()
                End If

                Return udtPracticeModelCollection

            Catch ex As Exception
                Throw ex

            End Try

        End Function

        Public Function GetPracticeBankAcctListFromStagingByERN(ByVal strERN As String, ByVal udtDB As Database) As PracticeModelCollection
            Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim intMODisplaySeq As Nullable(Of Integer)

            Dim btyBankTsmp As Byte()
            Dim btyPracticeTsmp As Byte()

            Dim dtRaw As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_PracticeBankAccountStaging_get_byERN", prams, dtRaw)

                For i As Integer = 0 To dtRaw.Rows.Count - 1
                    Dim drRaw As DataRow = dtRaw.Rows(i)

                    If IsDBNull(drRaw.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drRaw.Item("Address_Code")))
                    End If

                    If IsDBNull(drRaw.Item("Bank_Display_Seq")) Then
                        intBankDisplaySeq = Nothing
                    Else
                        intBankDisplaySeq = CInt(drRaw.Item("Bank_Display_Seq"))
                    End If

                    If IsDBNull(drRaw.Item("Display_Seq")) Then
                        intPracticeDisplaySeq = Nothing
                    Else
                        intPracticeDisplaySeq = CInt(drRaw.Item("Display_Seq"))
                    End If

                    If IsDBNull(drRaw.Item("MO_Display_Seq")) Then
                        intMODisplaySeq = Nothing
                    Else
                        intMODisplaySeq = CInt(drRaw.Item("MO_Display_Seq"))
                    End If

                    If drRaw.IsNull("Bank_TSMP") Then
                        btyBankTsmp = Nothing
                    Else
                        btyBankTsmp = CType(drRaw.Item("Bank_TSMP"), Byte())
                    End If

                    If drRaw.IsNull("Practice_TSMP") Then
                        btyPracticeTsmp = Nothing
                    Else
                        btyPracticeTsmp = CType(drRaw.Item("Practice_TSMP"), Byte())
                    End If

                    ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                    ' Add [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
                    udtPracticeModel = New PracticeModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                            CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
                                                            intPracticeDisplaySeq, _
                                                            intMODisplaySeq, _
                                                            CType(drRaw.Item("Practice_Name"), String).Trim, _
                                                            CStr(IIf((drRaw.Item("Practice_Name_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Name_Chi"))), _
                                                            New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))), _
                                                                CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))).Trim, _
                                                                CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))).Trim, _
                                                                intAddressCode), _
                                                            CInt(drRaw.Item("Professional_Seq")), _
                                                            CStr(IIf((drRaw.Item("Practice_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Record_Status"))).Trim, _
                                                            CStr(IIf((drRaw.Item("Practice_Submission_Method") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Submission_Method"))).Trim, _
                                                            CStr(IIf((drRaw.Item("Practice_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Remark"))).Trim, _
                                                            CStr(drRaw.Item("Phone_Daytime")), _
                                                            CType(drRaw.Item("Practice_Create_Dtm"), DateTime), _
                                                            CStr(IIf((drRaw.Item("Practice_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Create_By"))).Trim, _
                                                            CType(drRaw.Item("Practice_Update_Dtm"), DateTime), _
                                                            CStr(IIf((drRaw.Item("Practice_Update_By") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Update_By"))).Trim, _
                                                            btyPracticeTsmp, _
                                                            CStr(drRaw.Item("Mobile_Clinic")).Trim, _
                                                            CStr(IIf((drRaw.Item("Remarks_Desc") Is DBNull.Value), String.Empty, drRaw.Item("Remarks_Desc"))).Trim, _
                                                            CStr(IIf((drRaw.Item("Remarks_Desc_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Remarks_Desc_Chi"))).Trim, _
                                                            New BankAcctModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Enrolment_Ref_No") Is DBNull.Value), String.Empty, drRaw.Item("Enrolment_Ref_No"))).Trim, _
                                                                intBankDisplaySeq, _
                                                                intPracticeDisplaySeq, _
                                                                CStr(IIf((drRaw.Item("Bank_Name") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Name"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Branch_Name") Is DBNull.Value), String.Empty, drRaw.Item("Branch_Name"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Acc_Holder"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Account_No"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Bank_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Record_Status"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Bank_Submission_Method") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Submission_Method"))).Trim, _
                                                                CStr(IIf((drRaw.Item("Bank_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Remark"))).Trim, _
                                                                Convert.ToDateTime(IIf(drRaw.Item("Bank_Create_Dtm") Is DBNull.Value, Nothing, drRaw.Item("Bank_Create_Dtm"))), _
                                                                CStr(IIf((drRaw.Item("Bank_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Create_By"))).Trim, _
                                                                Convert.ToDateTime(IIf(drRaw.Item("Bank_Update_Dtm") Is DBNull.Value, Nothing, drRaw.Item("Bank_Update_Dtm"))), _
                                                                CStr(IIf((drRaw.Item("Bank_Update_By") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Update_By"))).Trim, _
                                                                btyBankTsmp, _
                                                                CStr(IIf((drRaw.Item("Bank_IsFreeTextFormat") Is DBNull.Value), String.Empty, drRaw.Item("Bank_IsFreeTextFormat"))).Trim), _
                                                            New ProfessionalModel(String.Empty, _
                                                                CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
                                                                CInt(drRaw.Item("Professional_Seq")), _
                                                                CType(drRaw.Item("Service_Category_Code"), String).Trim, _
                                                                CType(drRaw.Item("Registration_Code"), String).Trim, _
                                                                CStr(IIf((drRaw.Item("Professional_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Professional_Record_Status"))).Trim, _
                                                                CType(drRaw.Item("Professional_Create_Dtm"), DateTime), _
                                                                CStr(IIf((drRaw.Item("Professional_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Professional_Create_By"))).Trim), _
                                                                Nothing)
                    ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

                    ' Get Practice Scheme Information
                    Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
                    udtPracticeModel.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq(udtPracticeModel.EnrolRefNo, udtPracticeModel.DisplaySeq, udtDB)

                    udtPracticeModelCollection.Add(udtPracticeModel)

                Next

                If Not IsNothing(udtPracticeModelCollection) Then
                    udtPracticeModelCollection.DuplicatePracticeEName()
                End If

                Return udtPracticeModelCollection

            Catch ex As Exception
                Throw ex

            End Try

        End Function

        ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
        ' ------------------------------------------------------------------------
        ' Function Obsoleted
        'Public Function GetPracticeBankAcctListFromStagingByERN_FromIVSS(ByVal strERN As String, ByVal udtDB As Database, ByVal strUserID As String) As PracticeModelCollection
        '    Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
        '    Dim udtPracticeModel As PracticeModel

        '    Dim intAddressCode As Nullable(Of Integer)
        '    Dim intBankDisplaySeq As Nullable(Of Integer)
        '    Dim intPracticeDisplaySeq As Nullable(Of Integer)
        '    Dim intMODisplaySeq As Nullable(Of Integer)

        '    Dim btyBankTsmp As Byte()
        '    Dim btyPracticeTsmp As Byte()

        '    Dim dtRaw As New DataTable()
        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
        '        udtDB.MakeInParam("@user_id", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, strUserID)}
        '        udtDB.RunProc("proc_PracticeBankAccountStaging_get_byERN_FromIVSS", prams, dtRaw)

        '        For i As Integer = 0 To dtRaw.Rows.Count - 1
        '            Dim drRaw As DataRow = dtRaw.Rows(i)

        '            If IsDBNull(drRaw.Item("Address_Code")) Then
        '                intAddressCode = Nothing
        '            Else
        '                intAddressCode = CInt((drRaw.Item("Address_Code")))
        '            End If

        '            If IsDBNull(drRaw.Item("Bank_Display_Seq")) Then
        '                intBankDisplaySeq = Nothing
        '            Else
        '                intBankDisplaySeq = CInt(drRaw.Item("Bank_Display_Seq"))
        '            End If

        '            If IsDBNull(drRaw.Item("Display_Seq")) Then
        '                intPracticeDisplaySeq = Nothing
        '            Else
        '                intPracticeDisplaySeq = CInt(drRaw.Item("Display_Seq"))
        '            End If

        '            If IsDBNull(drRaw.Item("MO_Display_Seq")) Then
        '                intMODisplaySeq = Nothing
        '            Else
        '                intMODisplaySeq = CInt(drRaw.Item("MO_Display_Seq"))
        '            End If

        '            If drRaw.IsNull("Bank_TSMP") Then
        '                btyBankTsmp = Nothing
        '            Else
        '                btyBankTsmp = CType(drRaw.Item("Bank_TSMP"), Byte())
        '            End If

        '            If drRaw.IsNull("Practice_TSMP") Then
        '                btyPracticeTsmp = Nothing
        '            Else
        '                btyPracticeTsmp = CType(drRaw.Item("Practice_TSMP"), Byte())
        '            End If

        '            udtPracticeModel = New PracticeModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
        '                                                    CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
        '                                                    intPracticeDisplaySeq, _
        '                                                    intMODisplaySeq, _
        '                                                    CType(drRaw.Item("Practice_Name"), String).Trim, _
        '                                                    CStr(IIf((drRaw.Item("Practice_Name_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Name_Chi"))), _
        '                                                    New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))), _
        '                                                        CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))).Trim, _
        '                                                        intAddressCode), _
        '                                                    CInt(drRaw.Item("Professional_Seq")), _
        '                                                    CStr(IIf((drRaw.Item("Practice_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Record_Status"))).Trim, _
        '                                                    CStr(IIf((drRaw.Item("Practice_Submission_Method") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Submission_Method"))).Trim, _
        '                                                    CStr(IIf((drRaw.Item("Practice_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Remark"))).Trim, _
        '                                                    CStr(drRaw.Item("Phone_Daytime")), _
        '                                                    CType(drRaw.Item("Practice_Create_Dtm"), DateTime), _
        '                                                    CStr(IIf((drRaw.Item("Practice_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Create_By"))).Trim, _
        '                                                    CType(drRaw.Item("Practice_Update_Dtm"), DateTime), _
        '                                                    CStr(IIf((drRaw.Item("Practice_Update_By") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Update_By"))).Trim, _
        '                                                    btyPracticeTsmp, _
        '                                                    New BankAcctModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Enrolment_Ref_No") Is DBNull.Value), String.Empty, drRaw.Item("Enrolment_Ref_No"))).Trim, _
        '                                                        intBankDisplaySeq, _
        '                                                        intPracticeDisplaySeq, _
        '                                                        CStr(IIf((drRaw.Item("Bank_Name") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Name"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Branch_Name") Is DBNull.Value), String.Empty, drRaw.Item("Branch_Name"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Acc_Holder"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Account_No"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Bank_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Record_Status"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Bank_Submission_Method") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Submission_Method"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Bank_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Remark"))).Trim, _
        '                                                        Convert.ToDateTime(IIf(drRaw.Item("Bank_Create_Dtm") Is DBNull.Value, Nothing, drRaw.Item("Bank_Create_Dtm"))), _
        '                                                        CStr(IIf((drRaw.Item("Bank_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Create_By"))).Trim, _
        '                                                        Convert.ToDateTime(IIf(drRaw.Item("Bank_Update_Dtm") Is DBNull.Value, Nothing, drRaw.Item("Bank_Update_Dtm"))), _
        '                                                        CStr(IIf((drRaw.Item("Bank_Update_By") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Update_By"))).Trim, _
        '                                                        btyBankTsmp, _
        '                                                        CStr(IIf((drRaw.Item("Bank_IsFreeTextFormat") Is DBNull.Value), String.Empty, drRaw.Item("Bank_IsFreeTextFormat"))).Trim), _
        '                                                    New ProfessionalModel(String.Empty, _
        '                                                        CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
        '                                                        CInt(drRaw.Item("Professional_Seq")), _
        '                                                        CType(drRaw.Item("Service_Category_Code"), String).Trim, _
        '                                                        CType(drRaw.Item("Registration_Code"), String).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Professional_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Professional_Record_Status"))).Trim, _
        '                                                        CType(drRaw.Item("Professional_Create_Dtm"), DateTime), _
        '                                                        CStr(IIf((drRaw.Item("Professional_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Professional_Create_By"))).Trim), _
        '                                                        Nothing)

        '            ' Get Practice Scheme Information
        '            Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
        '            udtPracticeModel.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq_FromIVSS(udtPracticeModel.EnrolRefNo, udtPracticeModel.DisplaySeq, udtDB, strUserID)

        '            udtPracticeModelCollection.Add(udtPracticeModel)

        '        Next

        '        Return udtPracticeModelCollection

        '    Catch ex As Exception
        '        Throw ex

        '    End Try

        'End Function
        ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

        Public Function GetPracticeBankAcctListFromStagingByERNBankStagingStatus(ByVal strERN As String, ByVal udtDB As Database) As PracticeModelCollection
            'Dim drPracticeList As SqlDataReader = Nothing
            Dim udtPracticeModelCollection As New PracticeModelCollection
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim intMODisplaySeq As Nullable(Of Integer)

            Dim btyBankTsmp As Byte()
            Dim btyPracticeTsmp As Byte()

            Try
                Dim dtPractice As New DataTable

                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_PracticeBankAccountStaging_get_byERNBankStagingStatus", prams, dtPractice)

                For Each drPractice As DataRow In dtPractice.Rows

                    If IsDBNull(drPractice.Item("Address_Code")) Then
                        intAddressCode = Nothing
                    Else
                        intAddressCode = CInt((drPractice.Item("Address_Code")))
                    End If

                    If IsDBNull(drPractice.Item("Bank_Display_Seq")) Then
                        intBankDisplaySeq = Nothing
                    Else
                        intBankDisplaySeq = CInt(drPractice.Item("Bank_Display_Seq"))
                    End If

                    If IsDBNull(drPractice.Item("Display_Seq")) Then
                        intPracticeDisplaySeq = Nothing
                    Else
                        intPracticeDisplaySeq = CInt(drPractice.Item("Display_Seq"))
                    End If

                    If IsDBNull(drPractice.Item("MO_Display_Seq")) Then
                        intMODisplaySeq = Nothing
                    Else
                        intMODisplaySeq = CInt(drPractice.Item("MO_Display_Seq"))
                    End If

                    If drPractice.Item("Bank_TSMP") Is DBNull.Value Then
                        btyBankTsmp = Nothing
                    Else
                        btyBankTsmp = CType(drPractice.Item("Bank_TSMP"), Byte())
                    End If

                    If drPractice.Item("Practice_TSMP") Is DBNull.Value Then
                        btyPracticeTsmp = Nothing
                    Else
                        btyPracticeTsmp = CType(drPractice.Item("Practice_TSMP"), Byte())
                    End If

                    ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                    ' Add [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
                    udtPracticeModel = New PracticeModel(CStr(IIf((drPractice.Item("SP_ID") Is DBNull.Value), String.Empty, drPractice.Item("SP_ID"))).Trim, _
                                                            CType(drPractice.Item("Enrolment_Ref_No"), String).Trim, _
                                                            intPracticeDisplaySeq, _
                                                            intMODisplaySeq, _
                                                            CType(drPractice.Item("Practice_Name"), String).Trim, _
                                                            CStr(IIf((drPractice.Item("Practice_Name_Chi") Is DBNull.Value), String.Empty, drPractice.Item("Practice_Name_Chi"))), _
                                                            New AddressModel(CStr(IIf((drPractice.Item("Room") Is DBNull.Value), String.Empty, drPractice.Item("Room"))), _
                                                                CStr(IIf((drPractice.Item("Floor") Is DBNull.Value), String.Empty, drPractice.Item("Floor"))).Trim, _
                                                                CStr(IIf((drPractice.Item("Block") Is DBNull.Value), String.Empty, drPractice.Item("Block"))).Trim, _
                                                                CStr(IIf((drPractice.Item("Building") Is DBNull.Value), String.Empty, drPractice.Item("Building"))).Trim, _
                                                                CStr(IIf((drPractice.Item("Building_Chi") Is DBNull.Value), String.Empty, drPractice.Item("Building_Chi"))).Trim, _
                                                                CStr(IIf((drPractice.Item("District") Is DBNull.Value), String.Empty, drPractice.Item("District"))).Trim, _
                                                                intAddressCode), _
                                                            CInt(drPractice.Item("Professional_Seq")), _
                                                            CStr(IIf((drPractice.Item("Practice_Record_Status") Is DBNull.Value), String.Empty, drPractice.Item("Practice_Record_Status"))).Trim, _
                                                            CStr(IIf((drPractice.Item("Practice_Submission_Method") Is DBNull.Value), String.Empty, drPractice.Item("Practice_Submission_Method"))).Trim, _
                                                            CStr(IIf((drPractice.Item("Practice_Remark") Is DBNull.Value), String.Empty, drPractice.Item("Practice_Remark"))).Trim, _
                                                            CStr(IIf((drPractice.Item("Phone_Daytime") Is DBNull.Value), String.Empty, drPractice.Item("Phone_Daytime"))).Trim, _
                                                            CType(drPractice.Item("Practice_Create_Dtm"), DateTime), _
                                                            CStr(IIf((drPractice.Item("Practice_Create_By") Is DBNull.Value), String.Empty, drPractice.Item("Practice_Create_By"))).Trim, _
                                                            CType(drPractice.Item("Practice_Update_Dtm"), DateTime), _
                                                            CStr(IIf((drPractice.Item("Practice_Update_By") Is DBNull.Value), String.Empty, drPractice.Item("Practice_Update_By"))).Trim, _
                                                            btyPracticeTsmp, _
                                                            CStr(drPractice.Item("Mobile_Clinic")).Trim, _
                                                            CStr(IIf((drPractice.Item("Remarks_Desc") Is DBNull.Value), String.Empty, drPractice.Item("Remarks_Desc"))).Trim, _
                                                            CStr(IIf((drPractice.Item("Remarks_Desc_Chi") Is DBNull.Value), String.Empty, drPractice.Item("Remarks_Desc_Chi"))).Trim, _
                                                            New BankAcctModel(CStr(IIf((drPractice.Item("SP_ID") Is DBNull.Value), String.Empty, drPractice.Item("SP_ID"))).Trim, _
                                                                CStr(IIf((drPractice.Item("Enrolment_Ref_No") Is DBNull.Value), String.Empty, drPractice.Item("Enrolment_Ref_No"))).Trim, _
                                                                intBankDisplaySeq, _
                                                                intPracticeDisplaySeq, _
                                                                CStr(IIf((drPractice.Item("Bank_Name") Is DBNull.Value), String.Empty, drPractice.Item("Bank_Name"))).Trim, _
                                                                CStr(IIf((drPractice.Item("Branch_Name") Is DBNull.Value), String.Empty, drPractice.Item("Branch_Name"))).Trim, _
                                                                CStr(IIf((drPractice.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drPractice.Item("Bank_Acc_Holder"))).Trim, _
                                                                CStr(IIf((drPractice.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drPractice.Item("Bank_Account_No"))).Trim, _
                                                                CStr(IIf((drPractice.Item("Bank_Record_Status") Is DBNull.Value), String.Empty, drPractice.Item("Bank_Record_Status"))).Trim, _
                                                                CStr(IIf((drPractice.Item("Bank_Submission_Method") Is DBNull.Value), String.Empty, drPractice.Item("Bank_Submission_Method"))).Trim, _
                                                                CStr(IIf((drPractice.Item("Bank_Remark") Is DBNull.Value), String.Empty, drPractice.Item("Bank_Remark"))).Trim, _
                                                                Convert.ToDateTime(IIf(drPractice.Item("Bank_Create_Dtm") Is DBNull.Value, Nothing, drPractice.Item("Bank_Create_Dtm"))), _
                                                                CStr(IIf((drPractice.Item("Bank_Create_By") Is DBNull.Value), String.Empty, drPractice.Item("Bank_Create_By"))).Trim, _
                                                                Convert.ToDateTime(IIf(drPractice.Item("Bank_Update_Dtm") Is DBNull.Value, Nothing, drPractice.Item("Bank_Update_Dtm"))), _
                                                                CStr(IIf((drPractice.Item("Bank_Update_By") Is DBNull.Value), String.Empty, drPractice.Item("Bank_Update_By"))).Trim, _
                                                                btyBankTsmp, _
                                                                CStr(IIf((drPractice.Item("Bank_IsFreeTextFormat") Is DBNull.Value), String.Empty, drPractice.Item("Bank_IsFreeTextFormat"))).Trim), _
                                                            New ProfessionalModel(String.Empty, _
                                                                CType(drPractice.Item("Enrolment_Ref_No"), String).Trim, _
                                                                CInt(drPractice.Item("Professional_Seq")), _
                                                                CType(drPractice.Item("Service_Category_Code"), String).Trim, _
                                                                CType(drPractice.Item("Registration_Code"), String).Trim, _
                                                                CStr(IIf((drPractice.Item("Professional_Record_Status") Is DBNull.Value), String.Empty, drPractice.Item("Professional_Record_Status"))).Trim, _
                                                                CType(drPractice.Item("Professional_Create_Dtm"), DateTime), _
                                                                CStr(IIf((drPractice.Item("Professional_Create_By") Is DBNull.Value), String.Empty, drPractice.Item("Professional_Create_By"))).Trim), _
                                                                Nothing)
                    ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

                    ' Get Practice Scheme Information
                    Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
                    udtPracticeModel.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByErnPracticeDisplaySeq(udtPracticeModel.EnrolRefNo, udtPracticeModel.DisplaySeq, udtDB)

                    udtPracticeModelCollection.Add(udtPracticeModel)

                Next

                Return udtPracticeModelCollection

            Catch ex As Exception
                Throw ex

            End Try

        End Function

        'Public Function GetPracticeBankAcctListFromStagingByERN_(ByVal strERN As String, ByVal udtDB As Database) As PracticeModelCollection
        '    Dim drPracticeList As SqlDataReader = Nothing
        '    Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
        '    Dim udtPracticeModel As PracticeModel

        '    Dim intAddressCode As Nullable(Of Integer)
        '    Dim intBankDisplaySeq As Nullable(Of Integer)
        '    Dim intPracticeDisplaySeq As Nullable(Of Integer)
        '    Dim intMODisplaySeq As Nullable(Of Integer)

        '    Dim btyBankTsmp As Byte()
        '    Dim btyPracticeTsmp As Byte()


        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
        '        udtDB.RunProc("proc_PracticeBankAccountStaging_get_byERN", prams, drPracticeList)

        '        While drPracticeList.Read()
        '            If IsDBNull(drPracticeList.Item("Address_Code")) Then
        '                intAddressCode = Nothing
        '            Else
        '                intAddressCode = CInt((drPracticeList.Item("Address_Code")))
        '            End If

        '            If IsDBNull(drPracticeList.Item("Bank_Display_Seq")) Then
        '                intBankDisplaySeq = Nothing
        '            Else
        '                intBankDisplaySeq = CInt(drPracticeList.Item("Bank_Display_Seq"))
        '            End If

        '            If IsDBNull(drPracticeList.Item("Display_Seq")) Then
        '                intPracticeDisplaySeq = Nothing
        '            Else
        '                intPracticeDisplaySeq = CInt(drPracticeList.Item("Display_Seq"))
        '            End If

        '            If IsDBNull(drPracticeList.Item("MO_Display_Seq")) Then
        '                intMODisplaySeq = Nothing
        '            Else
        '                intMODisplaySeq = CInt(drPracticeList.Item("MO_Display_Seq"))
        '            End If

        '            If drPracticeList.Item("Bank_TSMP") Is DBNull.Value Then
        '                btyBankTsmp = Nothing
        '            Else
        '                btyBankTsmp = CType(drPracticeList.Item("Bank_TSMP"), Byte())
        '            End If

        '            If drPracticeList.Item("Practice_TSMP") Is DBNull.Value Then
        '                btyPracticeTsmp = Nothing
        '            Else
        '                btyPracticeTsmp = CType(drPracticeList.Item("Practice_TSMP"), Byte())
        '            End If

        '            udtPracticeModel = New PracticeModel(CStr(IIf((drPracticeList.Item("SP_ID") Is DBNull.Value), String.Empty, drPracticeList.Item("SP_ID"))).Trim, _
        '                                               CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
        '                                               intPracticeDisplaySeq, _
        '                                               intMODisplaySeq, _
        '                                               CType(drPracticeList.Item("Practice_Name"), String).Trim, _
        '                                               CStr(IIf((drPracticeList.Item("Practice_Name_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Name_Chi"))), _
        '                                                 New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))), _
        '                                                                                    CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))).Trim, _
        '                                                                                    CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))).Trim, _
        '                                                                                    CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))).Trim, _
        '                                                                                    CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))).Trim, _
        '                                                                                    CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))).Trim, _
        '                                                                                    intAddressCode), _
        '                                               CInt(drPracticeList.Item("Professional_Seq")), _
        '                                               CStr(IIf((drPracticeList.Item("Practice_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Record_Status"))).Trim, _
        '                                               CStr(IIf((drPracticeList.Item("Practice_Submission_Method") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Submission_Method"))).Trim, _
        '                                               CStr(IIf((drPracticeList.Item("Practice_Remark") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Remark"))).Trim, _
        '                                               CStr(IIf((drPracticeList.Item("Phone_Daytime") Is DBNull.Value), String.Empty, drPracticeList.Item("Phone_Daytime"))), _
        '                                               CType(drPracticeList.Item("Practice_Create_Dtm"), DateTime), _
        '                                               CStr(IIf((drPracticeList.Item("Practice_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Create_By"))).Trim, _
        '                                               CType(drPracticeList.Item("Practice_Update_Dtm"), DateTime), _
        '                                               CStr(IIf((drPracticeList.Item("Practice_Update_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Update_By"))).Trim, _
        '                                               btyPracticeTsmp, _
        '                                               New BankAcctModel(CStr(IIf((drPracticeList.Item("SP_ID") Is DBNull.Value), String.Empty, drPracticeList.Item("SP_ID"))).Trim, _
        '                                                   CStr(IIf((drPracticeList.Item("Enrolment_Ref_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Enrolment_Ref_No"))).Trim, _
        '                                                   intBankDisplaySeq, _
        '                                                   intPracticeDisplaySeq, _
        '                                                   CStr(IIf((drPracticeList.Item("Bank_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Name"))).Trim, _
        '                                                   CStr(IIf((drPracticeList.Item("Branch_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Branch_Name"))).Trim, _
        '                                                   CStr(IIf((drPracticeList.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Acc_Holder"))).Trim, _
        '                                                   CStr(IIf((drPracticeList.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Account_No"))).Trim, _
        '                                                   CStr(IIf((drPracticeList.Item("Bank_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Record_Status"))).Trim, _
        '                                                   CStr(IIf((drPracticeList.Item("Bank_Submission_Method") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Submission_Method"))).Trim, _
        '                                                   CStr(IIf((drPracticeList.Item("Bank_Remark") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Remark"))).Trim, _
        '                                                   Convert.ToDateTime(IIf(drPracticeList.Item("Bank_Create_Dtm") Is DBNull.Value, Nothing, drPracticeList.Item("Bank_Create_Dtm"))), _
        '                                                   CStr(IIf((drPracticeList.Item("Bank_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Create_By"))).Trim, _
        '                                                   Convert.ToDateTime(IIf(drPracticeList.Item("Bank_Update_Dtm") Is DBNull.Value, Nothing, drPracticeList.Item("Bank_Update_Dtm"))), _
        '                                                   CStr(IIf((drPracticeList.Item("Bank_Update_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Update_By"))).Trim, _
        '                                                   btyBankTsmp), _
        '                                                New ProfessionalModel(String.Empty, _
        '                                                    CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
        '                                                    CInt(drPracticeList.Item("Professional_Seq")), _
        '                                                    CType(drPracticeList.Item("Service_Category_Code"), String).Trim, _
        '                                                    CType(drPracticeList.Item("Registration_Code"), String).Trim, _
        '                                                    CStr(IIf((drPracticeList.Item("Professional_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Professional_Record_Status"))).Trim, _
        '                                                    CType(drPracticeList.Item("Professional_Create_Dtm"), DateTime), _
        '                                                   CStr(IIf((drPracticeList.Item("Professional_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Professional_Create_By"))).Trim), _
        '                                                   Nothing)

        '            udtPracticeModelCollection.Add(udtPracticeModel)
        '        End While
        '        drPracticeList.Close()
        '        Return udtPracticeModelCollection
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not drPracticeList Is Nothing Then
        '            drPracticeList.Close()
        '        End If
        '    End Try
        'End Function

        Public Function GetPracticeListPermanentTSMP(ByVal strSPID As String, ByVal udtDB As Database) As Hashtable
            Dim ht As New Hashtable
            Dim dt As New DataTable
            Dim i As Integer = 0

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_PracticeTSMP_get_bySPID", prams, dt)

                For i = 0 To dt.Rows.Count - 1
                    ht.Add(CInt(dt.Rows(i).Item("Display_Seq")), CType(dt.Rows(i).Item("TSMP"), Byte()))
                Next

            Catch ex As Exception
                Throw ex
            End Try
            Return ht
        End Function

        Public Function AddPracticeListToEnrolment(ByVal udtPracticeModelCollection As PracticeModelCollection, ByRef udtDB As Database) As Boolean
            'Dim i As Integer
            'Dim udtPracticeModel As PracticeModel

            Try
                For Each udtPracticeModel As PracticeModel In udtPracticeModelCollection.Values
                    'For i = 0 To udtPracticeModelCollection.Count - 1
                    'udtPracticeModel = New PracticeModel(udtPracticeModelCollection.Item(i + 1))

                    'Dim prams() As SqlParameter = { _
                    '               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                    '               udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                    '               udtDB.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                    '               udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtPracticeModel.PracticeAddress.Room.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Room)), _
                    '               udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtPracticeModel.PracticeAddress.Floor.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Floor)), _
                    '               udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtPracticeModel.PracticeAddress.Block.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Block)), _
                    '               udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtPracticeModel.PracticeAddress.Building.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.Building)), _
                    '               udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtPracticeModel.PracticeAddress.ChiBuilding.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.ChiBuilding)), _
                    '               udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtPracticeModel.PracticeAddress.District.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.District)), _
                    '               udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtPracticeModel.PracticeAddress.Address_Code.HasValue, udtPracticeModel.PracticeAddress.Address_Code, DBNull.Value)), _
                    '               udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, udtPracticeModel.ProfessionalSeq)}

                    'udtDB.RunProc("proc_PracticeEnrolment_add", prams)

                    AddPracticeToEnrolment(udtPracticeModel, udtDB)
                Next
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddPracticeListToStaging(ByVal udtPracticeModelCollection As PracticeModelCollection, ByVal udtDB As Database) As Boolean
            'Dim i As Integer
            'Dim udtPracticeModel As PracticeModel

            Try
                For Each udtPracticeModel As PracticeModel In udtPracticeModelCollection.Values
                    'For i = 0 To udtPracticeModelCollection.Count - 1
                    'udtPracticeModel = New PracticeModel(udtPracticeModelCollection.Item(i + 1))

                    'Dim prams() As SqlParameter = { _
                    '               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                    '               udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                    '               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtPracticeModel.SPID.Equals(String.Empty), DBNull.Value, udtPracticeModel.SPID)), _
                    '               udtDB.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                    '               udtDB.MakeInParam("@practice_type", PracticeModel.PracticeTypeDataType, PracticeModel.PracticeTypeDataSize, udtPracticeModel.PracticeType), _
                    '               udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtPracticeModel.PracticeAddress.Room.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Room)), _
                    '               udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtPracticeModel.PracticeAddress.Floor.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Floor)), _
                    '               udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtPracticeModel.PracticeAddress.Block.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Block)), _
                    '               udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtPracticeModel.PracticeAddress.Building.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Building)), _
                    '               udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtPracticeModel.PracticeAddress.ChiBuilding.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.ChiBuilding)), _
                    '               udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtPracticeModel.PracticeAddress.District.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.District)), _
                    '               udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtPracticeModel.PracticeAddress.Address_Code.HasValue, udtPracticeModel.PracticeAddress.Address_Code, DBNull.Value)), _
                    '               udtDB.MakeInParam("@professional_seq", ProfessionalModel.PofessionalSeqDataType, ProfessionalModel.PofessionalSeqDataSize, udtPracticeModel.ProfessionalSeq), _
                    '               udtDB.MakeInParam("@record_status", PracticeModel.RecordStatusDataType, PracticeModel.RecordStatusDataSize, udtPracticeModel.RecordStatus), _
                    '               udtDB.MakeInParam("@remark", PracticeModel.RemarkDataType, PracticeModel.RemarkDataSize, IIf(udtPracticeModel.Remark.Equals(String.Empty), DBNull.Value, udtPracticeModel.Remark)), _
                    '               udtDB.MakeInParam("@submission_method", PracticeModel.SubmissionMethodDataType, PracticeModel.SubmissionMethodDataSize, udtPracticeModel.SubmitMethod), _
                    '               udtDB.MakeInParam("@create_by", PracticeModel.CreateByDataType, PracticeModel.CreateByDataSize, udtPracticeModel.CreateBy), _
                    '               udtDB.MakeInParam("@update_by", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy)}

                    'udtDB.RunProc("proc_PracticeStaging_add", prams)
                    AddPracticeToStaging(udtPracticeModel, udtDB)
                Next
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddPracticeToEnrolment(ByVal udtPracticeModel As PracticeModel, ByRef udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                                   udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                                   udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                                   udtDB.MakeInParam("@mo_display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, udtPracticeModel.MODisplaySeq), _
                                   udtDB.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                                   udtDB.MakeInParam("@practice_name_chi", PracticeModel.PracticeNameChiDataType, PracticeModel.PracticeNameChiDataSize, IIf(udtPracticeModel.PracticeNameChi.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeNameChi)), _
                                   udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtPracticeModel.PracticeAddress.Room.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Room)), _
                                   udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtPracticeModel.PracticeAddress.Floor.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Floor)), _
                                   udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtPracticeModel.PracticeAddress.Block.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Block)), _
                                   udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtPracticeModel.PracticeAddress.Building.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.Building)), _
                                   udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtPracticeModel.PracticeAddress.ChiBuilding.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.ChiBuilding)), _
                                   udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtPracticeModel.PracticeAddress.District.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.District)), _
                                   udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, udtPracticeModel.ProfessionalSeq), _
                                   udtDB.MakeInParam("@phone_daytime", PracticeModel.PhoneDaytimeDataType, PracticeModel.PhoneDaytimeDataSize, udtPracticeModel.PhoneDaytime)}

                udtDB.RunProc("proc_PracticeEnrolment_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddPracticeToStaging(ByVal udtPracticeModel As PracticeModel, ByVal udtDB As Database) As Boolean
            'Dim i As Integer

            Try
                ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                ' Add [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                               udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtPracticeModel.SPID.Equals(String.Empty), DBNull.Value, udtPracticeModel.SPID)), _
                               udtDB.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                               udtDB.MakeInParam("@practice_name_chi", PracticeModel.PracticeNameChiDataType, PracticeModel.PracticeNameChiDataSize, udtPracticeModel.PracticeNameChi), _
                               udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtPracticeModel.PracticeAddress.Room.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Room)), _
                               udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtPracticeModel.PracticeAddress.Floor.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Floor)), _
                               udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtPracticeModel.PracticeAddress.Block.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Block)), _
                               udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtPracticeModel.PracticeAddress.Building.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.Building)), _
                               udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtPracticeModel.PracticeAddress.ChiBuilding.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.ChiBuilding)), _
                               udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtPracticeModel.PracticeAddress.District.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.District)), _
                               udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtPracticeModel.PracticeAddress.Address_Code.HasValue, udtPracticeModel.PracticeAddress.Address_Code, DBNull.Value)), _
                               udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, udtPracticeModel.ProfessionalSeq), _
                               udtDB.MakeInParam("@record_status", PracticeModel.RecordStatusDataType, PracticeModel.RecordStatusDataSize, udtPracticeModel.RecordStatus), _
                               udtDB.MakeInParam("@remark", PracticeModel.RemarkDataType, PracticeModel.RemarkDataSize, IIf(udtPracticeModel.Remark.Equals(String.Empty), DBNull.Value, udtPracticeModel.Remark)), _
                               udtDB.MakeInParam("@phone_daytime", PracticeModel.PhoneDaytimeDataType, PracticeModel.PhoneDaytimeDataSize, udtPracticeModel.PhoneDaytime), _
                               udtDB.MakeInParam("@mo_display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.MODisplaySeq), _
                               udtDB.MakeInParam("@submission_method", PracticeModel.SubmissionMethodDataType, PracticeModel.SubmissionMethodDataSize, udtPracticeModel.SubmitMethod), _
                               udtDB.MakeInParam("@create_by", PracticeModel.CreateByDataType, PracticeModel.CreateByDataSize, udtPracticeModel.CreateBy), _
                               udtDB.MakeInParam("@update_by", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy), _
                               udtDB.MakeInParam("@Mobile_clinic", PracticeModel.MobileClinicDataType, PracticeModel.MobileClinicDataSize, udtPracticeModel.MobileClinic), _
                               udtDB.MakeInParam("@remarks_desc", PracticeModel.RemarksDescDataType, PracticeModel.RemarksDescDataSize, IIf(udtPracticeModel.RemarksDesc.Equals(String.Empty), DBNull.Value, udtPracticeModel.RemarksDesc)), _
                               udtDB.MakeInParam("@remarks_desc_chi", PracticeModel.RemarksDescChiDataType, PracticeModel.RemarksDescChiDataSize, IIf(udtPracticeModel.RemarksDescChi.Equals(String.Empty), DBNull.Value, udtPracticeModel.RemarksDescChi))}
                ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

                udtDB.RunProc("proc_PracticeStaging_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function UpdatePracticeUnderModificationStatus(ByVal udtPracticeModel As PracticeModel, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                'Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtPracticeModel.SPID), _
                '                                udtDB.MakeInParam("@undermodification", ServiceProviderModel.UnderModificationDataType, ServiceProviderModel.UnderModificationDataSize, IIf(udtPracticeModel.UnderModification = Nothing, DBNull.Value, udtPracticeModel.UnderModification)), _
                '                                udtDB.MakeInParam("@update_by", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, udtPracticeModel.UpdateBy), _
                '                                udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtPracticeModel.TSMP)}
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtPracticeModel.SPID), _
                                                udtDB.MakeInParam("@update_by", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, udtPracticeModel.UpdateBy), _
                                                udtDB.MakeInParam("@tsmp", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, udtPracticeModel.TSMP)}
                udtDB.RunProc("proc_Practice_upd_UnderModify", prams)

                blnRes = True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function UpdatePracticeStaging(ByVal udtPracticeModel As PracticeModel, ByVal udtdb As Database) As Boolean
            Try
                ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                ' Add [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
                Dim prams() As SqlParameter = { _
                               udtdb.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                               udtdb.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                               udtdb.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                               udtdb.MakeInParam("@practice_name_chi", PracticeModel.PracticeNameChiDataType, PracticeModel.PracticeNameChiDataSize, IIf(udtPracticeModel.PracticeNameChi.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeNameChi)), _
                               udtdb.MakeInParam("@mo_display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.MODisplaySeq), _
                               udtdb.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtPracticeModel.PracticeAddress.Room.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Room)), _
                               udtdb.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtPracticeModel.PracticeAddress.Floor.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Floor)), _
                               udtdb.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtPracticeModel.PracticeAddress.Block.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Block)), _
                               udtdb.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtPracticeModel.PracticeAddress.Building.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.Building)), _
                               udtdb.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtPracticeModel.PracticeAddress.ChiBuilding.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.ChiBuilding)), _
                               udtdb.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtPracticeModel.PracticeAddress.District.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.District)), _
                               udtdb.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtPracticeModel.PracticeAddress.Address_Code.HasValue, udtPracticeModel.PracticeAddress.Address_Code, DBNull.Value)), _
                               udtdb.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, udtPracticeModel.ProfessionalSeq), _
                               udtdb.MakeInParam("@record_status", PracticeModel.RecordStatusDataType, PracticeModel.RecordStatusDataSize, udtPracticeModel.RecordStatus), _
                               udtdb.MakeInParam("@remark", PracticeModel.RemarkDataType, PracticeModel.RemarkDataSize, IIf(udtPracticeModel.Remark.Equals(String.Empty), DBNull.Value, udtPracticeModel.Remark)), _
                               udtdb.MakeInParam("@update_by", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy), _
                               udtdb.MakeInParam("@phone_daytime", PracticeModel.PhoneDaytimeDataType, PracticeModel.PhoneDaytimeDataSize, udtPracticeModel.PhoneDaytime), _
                               udtdb.MakeInParam("@tsmp", PracticeModel.TSMPDataType, PracticeModel.TSMPDataSize, udtPracticeModel.TSMP), _
                               udtdb.MakeInParam("@Mobile_clinic", PracticeModel.MobileClinicDataType, PracticeModel.MobileClinicDataSize, udtPracticeModel.MobileClinic), _
                               udtdb.MakeInParam("@remarks_desc", PracticeModel.RemarksDescDataType, PracticeModel.RemarksDescDataSize, IIf(udtPracticeModel.RemarksDesc.Equals(String.Empty), DBNull.Value, udtPracticeModel.RemarksDesc)), _
                               udtdb.MakeInParam("@remarks_desc_chi", PracticeModel.RemarksDescChiDataType, PracticeModel.RemarksDescChiDataSize, IIf(udtPracticeModel.RemarksDescChi.Equals(String.Empty), DBNull.Value, udtPracticeModel.RemarksDescChi))}
                ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

                udtdb.RunProc("proc_PracticeStaging_upd", prams)
                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        Public Function UpdatePracticeStagingMODisplaySeq(ByVal udtPracticeList As PracticeModelCollection, ByRef udtDB As Database) As Boolean
            Dim blnRes As Boolean = False

            Try
                For Each udtPractice As PracticeModel In udtPracticeList.Values
                    Dim prams() As SqlParameter = { _
                                                   udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPractice.EnrolRefNo), _
                                                   udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPractice.DisplaySeq), _
                                                   udtDB.MakeInParam("@mo_display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, udtPractice.MODisplaySeq), _
                                                   udtDB.MakeInParam("@tsmp", PracticeModel.TSMPDataType, PracticeModel.TSMPDataSize, udtPractice.TSMP)}

                    udtDB.RunProc("proc_PracticeStaging_upd_MoDisplaySeq", prams)
                Next

                blnRes = True
            Catch ex As Exception
                Throw ex
            End Try

            Return blnRes
        End Function

        Public Function UpdatePracticePermanentAddress(ByVal udtPracticeModel As PracticeModel, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                ' Add [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtPracticeModel.SPID), _
                               udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                               udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtPracticeModel.PracticeAddress.Room.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Room)), _
                               udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtPracticeModel.PracticeAddress.Floor.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Floor)), _
                               udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtPracticeModel.PracticeAddress.Block.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Block)), _
                               udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtPracticeModel.PracticeAddress.Building.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.Building)), _
                               udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtPracticeModel.PracticeAddress.ChiBuilding.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.ChiBuilding)), _
                               udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtPracticeModel.PracticeAddress.District.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.District)), _
                               udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtPracticeModel.PracticeAddress.Address_Code.HasValue, udtPracticeModel.PracticeAddress.Address_Code, DBNull.Value)), _
                               udtDB.MakeInParam("@update_by", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy), _
                               udtDB.MakeInParam("@tsmp", PracticeModel.TSMPDataType, PracticeModel.TSMPDataSize, udtPracticeModel.TSMP), _
                               udtDB.MakeInParam("@phone_daytime", PracticeModel.PhoneDaytimeDataType, PracticeModel.PhoneDaytimeDataSize, IIf(udtPracticeModel.PhoneDaytime.Equals(String.Empty), DBNull.Value, udtPracticeModel.PhoneDaytime)), _
                               udtDB.MakeInParam("@mo_display_seq", MedicalOrganizationModel.DisplaySeqDataType, MedicalOrganizationModel.DisplaySeqDataSize, udtPracticeModel.MODisplaySeq), _
                               udtDB.MakeInParam("@practice_name_chi", PracticeModel.PracticeNameChiDataType, PracticeModel.PracticeNameChiDataSize, IIf(udtPracticeModel.PracticeNameChi.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeNameChi)), _
                               udtDB.MakeInParam("@Mobile_clinic", PracticeModel.MobileClinicDataType, PracticeModel.MobileClinicDataSize, udtPracticeModel.MobileClinic), _
                               udtDB.MakeInParam("@remarks_desc", PracticeModel.RemarksDescDataType, PracticeModel.RemarksDescDataSize, IIf(udtPracticeModel.RemarksDesc.Equals(String.Empty), DBNull.Value, udtPracticeModel.RemarksDesc)), _
                               udtDB.MakeInParam("@remarks_desc_chi", PracticeModel.RemarksDescChiDataType, PracticeModel.RemarksDescChiDataSize, IIf(udtPracticeModel.RemarksDescChi.Equals(String.Empty), DBNull.Value, udtPracticeModel.RemarksDescChi))}
                ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

                udtDB.RunProc("proc_Practice_upd_Address", prams)
                blnRes = True
            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function UpdatePracticeListPermanentAddress(ByVal udtPracticeModelList As PracticeModelCollection, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
                For Each udtPracticeModel As PracticeModel In udtPracticeModelList.Values
                    UpdatePracticePermanentAddress(udtPracticeModel, udtDB)
                Next
                blnRes = True
            Catch ex As Exception
                Throw ex
                blnRes = False
            End Try

            Return blnRes
        End Function

        Public Function DeletePracticeStaging(ByVal udtPracticeModel As PracticeModel, ByVal udtdb As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                               udtdb.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                               udtdb.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                               udtdb.MakeInParam("@record_status", PracticeModel.RecordStatusDataType, PracticeModel.RecordStatusDataSize, udtPracticeModel.RecordStatus), _
                               udtdb.MakeInParam("@update_by", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy), _
                               udtdb.MakeInParam("@tsmp", PracticeModel.TSMPDataType, PracticeModel.TSMPDataSize, udtPracticeModel.TSMP)}

                udtdb.RunProc("proc_PracticeStaging_del", prams)
                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Sub DeletePracticeStagingByKey(ByRef udtDB As Database, ByVal strERN As String, ByVal intDispSeq As Integer, ByVal TSMP As Byte(), ByVal blnCheckTSMP As Boolean)
            Try
                Dim objTSMP As Object = Nothing
                If TSMP Is Nothing Then
                    objTSMP = DBNull.Value
                Else
                    objTSMP = TSMP
                End If
                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@Enrolment_Ref_No", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                    udtDB.MakeInParam("@Display_Seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, intDispSeq), _
                    udtDB.MakeInParam("@tsmp", PracticeModel.TSMPDataType, PracticeModel.TSMPDataSize, objTSMP), _
                    udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP)}

                udtDB.RunProc("proc_PracticeStaging_del_ByKey", params)

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function AddPracticeListToPermanent(ByVal udtPracticeModelCollection As PracticeModelCollection, ByVal udtDB As Database) As Boolean
            Dim i As Integer
            Dim udtPracticeModel As PracticeModel

            Try
                For i = 0 To udtPracticeModelCollection.Count - 1
                    udtPracticeModel = New PracticeModel(udtPracticeModelCollection.Item(i + 1))

                    AddPracticeToPermanent(udtPracticeModel, udtDB)

                    ' Handle PracticeSchemeInfo

                    If Not udtPracticeModel.PracticeSchemeInfoList Is Nothing Then
                        Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL

                        ' CRE15-004 TIV & QIV [Start][Winnie]
                        udtPracticeSchemeInfoBLL.FillPracticeSchemeInfoPermanent(udtPracticeModel, Nothing, udtDB)
                        ' CRE15-004 TIV & QIV [End][Winnie]

                    End If
                Next
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddPracticeToPermanent(ByVal udtPracticeModel As PracticeModel, ByVal udtDB As Database) As Boolean
            'Dim i As Integer

            Try
                ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                ' Add [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtPracticeModel.SPID.Equals(String.Empty), DBNull.Value, udtPracticeModel.SPID)), _
                               udtDB.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                               udtDB.MakeInParam("@practice_name_chi", PracticeModel.PracticeNameChiDataType, PracticeModel.PracticeNameChiDataSize, udtPracticeModel.PracticeNameChi), _
                               udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtPracticeModel.PracticeAddress.Room.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Room)), _
                               udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtPracticeModel.PracticeAddress.Floor.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Floor)), _
                               udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtPracticeModel.PracticeAddress.Block.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Block)), _
                               udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtPracticeModel.PracticeAddress.Building.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.Building)), _
                               udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtPracticeModel.PracticeAddress.ChiBuilding.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.ChiBuilding)), _
                               udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtPracticeModel.PracticeAddress.District.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.District)), _
                               udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtPracticeModel.PracticeAddress.Address_Code.HasValue, udtPracticeModel.PracticeAddress.Address_Code, DBNull.Value)), _
                               udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, udtPracticeModel.ProfessionalSeq), _
                               udtDB.MakeInParam("@record_status", PracticeModel.RecordStatusDataType, PracticeModel.RecordStatusDataSize, udtPracticeModel.RecordStatus), _
                               udtDB.MakeInParam("@remark", PracticeModel.RemarkDataType, PracticeModel.RemarkDataSize, IIf(udtPracticeModel.Remark.Equals(String.Empty), DBNull.Value, udtPracticeModel.Remark)), _
                               udtDB.MakeInParam("@phone_daytime", PracticeModel.PhoneDaytimeDataType, PracticeModel.PhoneDaytimeDataSize, udtPracticeModel.PhoneDaytime), _
                               udtDB.MakeInParam("@mo_display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.MODisplaySeq), _
                               udtDB.MakeInParam("@submission_method", PracticeModel.SubmissionMethodDataType, PracticeModel.SubmissionMethodDataSize, udtPracticeModel.SubmitMethod), _
                               udtDB.MakeInParam("@create_by", PracticeModel.CreateByDataType, PracticeModel.CreateByDataSize, udtPracticeModel.CreateBy), _
                               udtDB.MakeInParam("@update_by", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy), _
                               udtDB.MakeInParam("@Mobile_clinic", PracticeModel.MobileClinicDataType, PracticeModel.MobileClinicDataSize, udtPracticeModel.MobileClinic), _
                               udtDB.MakeInParam("@remarks_desc", PracticeModel.RemarksDescDataType, PracticeModel.RemarksDescDataSize, IIf(udtPracticeModel.RemarksDesc.Equals(String.Empty), DBNull.Value, udtPracticeModel.RemarksDesc)), _
                               udtDB.MakeInParam("@remarks_desc_chi", PracticeModel.RemarksDescChiDataType, PracticeModel.RemarksDescChiDataSize, IIf(udtPracticeModel.RemarksDescChi.Equals(String.Empty), DBNull.Value, udtPracticeModel.RemarksDescChi))}
                ' CRE16-022 (Add optional field "Remarks") [End][Winnie]                               

                udtDB.RunProc("proc_PracticePermanent_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function UpdatePracticeListInPermanentBySchemeEnrolment(ByVal udtPracticeModelCollection As PracticeModelCollection, ByVal udtPracticeSchemeListPermanent As PracticeSchemeInfoModelCollection, ByVal udtDB As Database) As Boolean
            Dim i As Integer
            'Dim udtPracticeModel As PracticeModel
            Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL

            Try
                For Each udtPracticeModel As PracticeModel In udtPracticeModelCollection.Values
                    ' **** PracticeDisplayStatus ****
                    ' A: New Add: Insert
                    ' U: Update
                    ' Else: Do nothing
                    If udtPracticeModel.RecordStatus = Common.Component.PracticeStagingStatus.Active Then
                        ' Insert
                        'udtPracticeModel.UpdateBy = strUserId
                        AddPracticeToPermanent(udtPracticeModel, udtDB)
                    ElseIf udtPracticeModel.RecordStatus = Common.Component.PracticeStagingStatus.Update Then
                        ' Update
                        'udtPracticeModel.UpdateBy = strUserId
                        UpdatePracticePermanentAddress(udtPracticeModel, udtDB)
                    Else
                        ' Do Nothing
                    End If

                    'Handle PracticeSchemeInfoList
                    If Not IsNothing(udtPracticeModel.PracticeSchemeInfoList) Then

                        ' CRE15-004 TIV & QIV [Start][Winnie]                        
                        udtPracticeSchemeInfoBLL.FillPracticeSchemeInfoPermanent(udtPracticeModel, udtPracticeSchemeListPermanent, udtDB)
                        ' CRE15-004 TIV & QIV [End][Winnie]

                    End If
                Next    'Next udtPracticeModel
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function UpdateRecordStatus(ByVal udtPracticeModel As PracticeModel, ByRef udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtPracticeModel.SPID), _
                udtDB.MakeInParam("@Display_Seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                udtDB.MakeInParam("@Update_By", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy), _
                udtDB.MakeInParam("@Record_Status", PracticeModel.RecordStatusDataType, PracticeModel.RecordStatusDataSize, udtPracticeModel.RecordStatus), _
                udtDB.MakeInParam("@TSMP", PracticeModel.TSMPDataType, PracticeModel.TSMPDataSize, udtPracticeModel.TSMP)}

                udtDB.RunProc("proc_Practice_upd_RecordStatus", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function UpdatePracticeStagingRecordStatus(ByVal udtPracticeModel As PracticeModel, ByRef udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@ERN", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                udtDB.MakeInParam("@Display_Seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                udtDB.MakeInParam("@Update_By", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy), _
                udtDB.MakeInParam("@Record_Status", PracticeModel.RecordStatusDataType, PracticeModel.RecordStatusDataSize, udtPracticeModel.RecordStatus), _
                udtDB.MakeInParam("@TSMP", PracticeModel.TSMPDataType, PracticeModel.TSMPDataSize, udtPracticeModel.TSMP)}

                udtDB.RunProc("proc_PracticeStaging_upd_RecordStatus", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function UpdatePracticeStagingProfSeq(ByVal udtPracticeModel As PracticeModel, ByRef udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@ERN", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                udtDB.MakeInParam("@Display_Seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                udtDB.MakeInParam("@Update_By", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy), _
                udtDB.MakeInParam("@Professional_Seq", SqlDbType.SmallInt, 5, udtPracticeModel.ProfessionalSeq), _
                udtDB.MakeInParam("@TSMP", PracticeModel.TSMPDataType, PracticeModel.TSMPDataSize, udtPracticeModel.TSMP)}

                udtDB.RunProc("proc_PracticeStaging_upd_ProfSeq", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function GetPracticeRowCountBySPIDProfSeq(ByVal udtPracticeModel As PracticeModel, ByRef udtDB As Database) As Integer
            Dim dtResult As DataTable = New DataTable
            Dim intRes As Integer = 0

            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtPracticeModel.SPID), _
                udtDB.MakeInParam("@Professional_Seq", SqlDbType.SmallInt, 5, udtPracticeModel.ProfessionalSeq)}

                udtDB.RunProc("proc_PracticeRowCount_bySPIDProfSeq", prams, dtResult)

                If dtResult.Rows(0)(0) > 0 Then
                    intRes = CInt(dtResult.Rows(0)(0))
                End If
                Return intRes
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        Public Function GetPracticeStagingRowCountByERNProfSeq(ByVal udtPracticeModel As PracticeModel, ByRef udtDB As Database) As Integer
            Dim dtResult As DataTable = New DataTable
            Dim intRes As Integer = 0

            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@ERN", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                udtDB.MakeInParam("@Professional_Seq", SqlDbType.SmallInt, 5, udtPracticeModel.ProfessionalSeq)}

                udtDB.RunProc("proc_PracticeStagingRowCount_byERNProfSeq", prams, dtResult)

                If dtResult.Rows(0)(0) > 0 Then
                    intRes = CInt(dtResult.Rows(0)(0))
                End If
                Return intRes
            Catch ex As Exception
                Throw ex
            End Try

        End Function

        ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
        ' ------------------------------------------------------------------------
        ' ''' <summary>
        ' ''' Load Practice (For MyProfile Only)
        ' ''' Retrieve the Practice Type Also in Practice(s)
        ' ''' To Be Remove After Data migration Complete
        ' ''' </summary>
        ' ''' <param name="strSPID"></param>
        ' ''' <param name="udtDB"></param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GetPracticeBankAcctListFromPermanentBySPID_ForMyProfileV1(ByVal strSPID As String, ByVal udtDB As Database) As PracticeModelCollection
        '    Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
        '    Dim udtPracticeModel As PracticeModel

        '    Dim intAddressCode As Nullable(Of Integer)
        '    Dim intBankDisplaySeq As Nullable(Of Integer)
        '    Dim intPracticeDisplaySeq As Nullable(Of Integer)
        '    Dim intMODisplaySeq As Nullable(Of Integer)

        '    Dim btyBankTsmp As Byte()
        '    Dim btyPracticeTsmp As Byte()

        '    Dim dtRaw As New DataTable
        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
        '        udtDB.RunProc("proc_PracticeBankAccount_get_bySPID_ForMyProfileV1", prams, dtRaw)

        '        For i As Integer = 0 To dtRaw.Rows.Count - 1
        '            Dim drRaw As DataRow = dtRaw.Rows(i)

        '            If IsDBNull(drRaw.Item("Address_Code")) Then
        '                intAddressCode = Nothing
        '            Else
        '                intAddressCode = CInt((drRaw.Item("Address_Code")))
        '            End If

        '            If IsDBNull(drRaw.Item("Bank_Display_Seq")) Then
        '                intBankDisplaySeq = Nothing
        '            Else
        '                intBankDisplaySeq = CInt(drRaw.Item("Bank_Display_Seq"))
        '            End If

        '            If IsDBNull(drRaw.Item("Display_Seq")) Then
        '                intPracticeDisplaySeq = Nothing
        '            Else
        '                intPracticeDisplaySeq = CInt(drRaw.Item("Display_Seq"))
        '            End If

        '            If IsDBNull(drRaw.Item("MO_Display_Seq")) Then
        '                intMODisplaySeq = Nothing
        '            Else
        '                intMODisplaySeq = CInt(drRaw.Item("MO_Display_Seq"))
        '            End If

        '            If drRaw.IsNull("Bank_TSMP") Then
        '                btyBankTsmp = Nothing
        '            Else
        '                btyBankTsmp = CType(drRaw.Item("Bank_TSMP"), Byte())
        '            End If

        '            If drRaw.IsNull("Practice_TSMP") Then
        '                btyPracticeTsmp = Nothing
        '            Else
        '                btyPracticeTsmp = CType(drRaw.Item("Practice_TSMP"), Byte())
        '            End If

        '            udtPracticeModel = New PracticeModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
        '                                                    String.Empty, _
        '                                                    intPracticeDisplaySeq, _
        '                                                    intMODisplaySeq, _
        '                                                    CType(drRaw.Item("Practice_Name"), String).Trim, _
        '                                                    CStr(IIf((drRaw.Item("Practice_Name_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Name_Chi"))), _
        '                                                    New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))), _
        '                                                        CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))).Trim, _
        '                                                        intAddressCode), _
        '                                                    CInt(drRaw.Item("Professional_Seq")), _
        '                                                    CStr(IIf((drRaw.Item("Practice_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Record_Status"))).Trim, _
        '                                                    CStr(IIf((drRaw.Item("Practice_Submission_Method") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Submission_Method"))).Trim, _
        '                                                    CStr(IIf((drRaw.Item("Practice_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Remark"))).Trim, _
        '                                                    CStr(drRaw.Item("Phone_Daytime")), _
        '                                                    CType(drRaw.Item("Practice_Create_Dtm"), DateTime), _
        '                                                    CStr(IIf((drRaw.Item("Practice_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Create_By"))).Trim, _
        '                                                    CType(drRaw.Item("Practice_Update_Dtm"), DateTime), _
        '                                                    CStr(IIf((drRaw.Item("Practice_Update_By") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Update_By"))).Trim, _
        '                                                    btyPracticeTsmp, _
        '                                                    New BankAcctModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
        '                                                        String.Empty, _
        '                                                        intBankDisplaySeq, _
        '                                                        intPracticeDisplaySeq, _
        '                                                        CStr(IIf((drRaw.Item("Bank_Name") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Name"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Branch_Name") Is DBNull.Value), String.Empty, drRaw.Item("Branch_Name"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Acc_Holder"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Account_No"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Bank_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Record_Status"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Bank_Submission_Method") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Submission_Method"))).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Bank_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Remark"))).Trim, _
        '                                                        Convert.ToDateTime(IIf(drRaw.Item("Bank_Create_Dtm") Is DBNull.Value, Nothing, drRaw.Item("Bank_Create_Dtm"))), _
        '                                                        CStr(IIf((drRaw.Item("Bank_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Create_By"))).Trim, _
        '                                                        Convert.ToDateTime(IIf(drRaw.Item("Bank_Update_Dtm") Is DBNull.Value, Nothing, drRaw.Item("Bank_Update_Dtm"))), _
        '                                                        CStr(IIf((drRaw.Item("Bank_Update_By") Is DBNull.Value), String.Empty, drRaw.Item("Bank_Update_By"))).Trim, _
        '                                                        btyBankTsmp, _
        '                                                        CStr(IIf((drRaw.Item("Bank_IsFreeTextFormat") Is DBNull.Value), String.Empty, drRaw.Item("Bank_IsFreeTextFormat"))).Trim), _
        '                                                    New ProfessionalModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
        '                                                        String.Empty, _
        '                                                        CInt(drRaw.Item("Professional_Seq")), _
        '                                                        CType(drRaw.Item("Service_Category_Code"), String).Trim, _
        '                                                        CType(drRaw.Item("Registration_Code"), String).Trim, _
        '                                                        CStr(IIf((drRaw.Item("Professional_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Professional_Record_Status"))).Trim, _
        '                                                        CType(drRaw.Item("Professional_Create_Dtm"), DateTime), _
        '                                                        CStr(IIf((drRaw.Item("Professional_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Professional_Create_By"))).Trim), _
        '                                                        Nothing)

        '            udtPracticeModel.PracticeType = CStr(IIf((drRaw.Item("Practice_Type") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Type"))).Trim()
        '            udtPracticeModel.BankAcct.BrCode = CStr(IIf((drRaw.Item("BR_Code") Is DBNull.Value), String.Empty, drRaw.Item("BR_Code"))).Trim()


        '            ' Get Practice Scheme Information
        '            Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
        '            udtPracticeModel.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPIDPracticeDisplaySeq(udtPracticeModel.SPID, udtPracticeModel.DisplaySeq, udtDB)

        '            udtPracticeModelCollection.Add(udtPracticeModel)

        '        Next

        '        Return udtPracticeModelCollection

        '    Catch ex As Exception
        '        Throw ex

        '    End Try

        'End Function
        ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

        Public Function getRawAllPracticeBankAcct(ByVal strSPID As String, Optional ByVal udtDB As Database = Nothing) As DataTable

            If udtDB Is Nothing Then udtDB = New Database()

            Dim dtPracticeBank As New DataTable()
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", ServiceProvider.ServiceProviderModel.SPIDDataType, ServiceProvider.ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_PracticeBankAccountAll_get_bySPID", prams, dtPracticeBank)


            Catch eSQL As SqlException
                dtPracticeBank = Nothing
                Throw eSQL
            Catch ex As Exception
                dtPracticeBank = Nothing
                Throw ex
            End Try
            Return dtPracticeBank
        End Function

#Region "Internal Class"

        Public Enum PracticeDisplayType
            Practice
            PracticeBankAccount
            PracticeAddress
        End Enum

        Public Class PracticeDisplayField
            Public Const Display_Eng As String = "Display_Name"
            Public Const Display_Chi As String = "Display_Name_Chi"
        End Class

        Public Class tablePracticeBank

            Public Const Practice_Name As String = "Practice_Name"
            Public Const Practice_Name_Chi As String = "Practice_Name_Chi"

            Public Const Building As String = "Building"
            Public Const Building_Chi As String = "Building_Chi"

            Public Const PracticeID As String = "PracticeID"
            Public Const Bank_Account_No As String = "Bank_Account_No"

        End Class
#End Region

#Region "Support function"
        Public Shared Sub ConcatePracticeDisplayColumn(ByRef dtSource As DataTable, ByVal enumPracticeDisplayType As PracticeDisplayType)

            Dim udtformatter As Common.Format.Formatter = New Common.Format.Formatter()

            ' Display Column for UI
            dtSource.Columns.Add(PracticeDisplayField.Display_Eng, GetType(String))
            dtSource.Columns.Add(PracticeDisplayField.Display_Chi, GetType(String))

            Dim udtAddress As AddressModel = Nothing

            For Each drRow As DataRow In dtSource.Rows
                Select Case enumPracticeDisplayType

                    Case PracticeDisplayType.Practice
                        ' Practice Name
                        drRow(PracticeDisplayField.Display_Eng) = drRow(tablePracticeBank.Practice_Name).ToString().Trim() + "(" + drRow(tablePracticeBank.PracticeID).ToString().Trim() + ") "
                        drRow(PracticeDisplayField.Display_Chi) = drRow(tablePracticeBank.Practice_Name_Chi).ToString().Trim() + "(" + drRow(tablePracticeBank.PracticeID).ToString().Trim() + ") "
                    Case PracticeDisplayType.PracticeBankAccount
                        ' Practice Name (Practice ID) [Mask Bank Account Num]
                        drRow(PracticeDisplayField.Display_Eng) = drRow(tablePracticeBank.Practice_Name).ToString().Trim() + "(" + drRow(tablePracticeBank.PracticeID).ToString().Trim() + ") [" + udtformatter.maskBankAccount(drRow(tablePracticeBank.Bank_Account_No).ToString().Trim()) + "]"
                        drRow(PracticeDisplayField.Display_Chi) = drRow(tablePracticeBank.Practice_Name_Chi).ToString().Trim() + "(" + drRow(tablePracticeBank.PracticeID).ToString().Trim() + ") [" + udtformatter.maskBankAccount(drRow(tablePracticeBank.Bank_Account_No).ToString().Trim()) + "]"
                    Case PracticeDisplayType.PracticeAddress
                        ' Practice Name (Practice ID) [Format Practice Address]
                        udtAddress = New AddressModel(drRow("Room").ToString(), drRow("Floor").ToString(), drRow("Block").ToString(), drRow("Building").ToString(), drRow("Building_Chi"), drRow("District").ToString(), Nothing)
                        With udtAddress
                            drRow(PracticeDisplayField.Display_Eng) = drRow(tablePracticeBank.Practice_Name).ToString().Trim() + "(" + drRow(tablePracticeBank.PracticeID).ToString().Trim() + ") [" + udtformatter.formatAddress(.Room, .Floor, .Block, .Building, .District, .AreaCode) + "]"
                            drRow(PracticeDisplayField.Display_Chi) = drRow(tablePracticeBank.Practice_Name_Chi).ToString().Trim() + "(" + drRow(tablePracticeBank.PracticeID).ToString().Trim() + ") [" + udtformatter.formatAddressChi(.Room, .Floor, .Block, .ChiBuilding, .District, .AreaCode) + "]"
                        End With
                End Select
            Next

            dtSource.AcceptChanges()

        End Sub

        Public Function convertPractice(ByVal dtSource As DataTable) As PracticeDisplayModelCollection

            ' Retrieve SystemParameter HCSP
            Dim strPara As String = String.Empty
            Dim strDummy As String = String.Empty

            Dim udtCommFunctBLL As New Common.ComFunction.GeneralFunction()
            udtCommFunctBLL.getSystemParameter("HCSPDataMirgrationCompleteTurnOn", strPara, strDummy)

            Dim udtPracticeDisplayModelList As New PracticeDisplayModelCollection()

            For Each drRow As DataRow In dtSource.Rows
                Dim udtPracticeDisplayMode As New PracticeDisplayModel(drRow)

                If strPara.Trim().ToUpper = "Y" Then
                    udtPracticeDisplayMode.DisplayEngOnly = False
                    If udtPracticeDisplayMode.PracticeName.Trim() <> udtPracticeDisplayMode.PracticeNameChi.Trim() Then
                        udtPracticeDisplayMode.PracticeNameDisplayChi = True
                    End If
                Else
                    udtPracticeDisplayMode.DisplayEngOnly = True
                End If
                udtPracticeDisplayModelList.Add(udtPracticeDisplayMode)
            Next

            Return udtPracticeDisplayModelList
        End Function

#End Region

#Region "Model Class"

        <Serializable()> Partial Public Class PracticeDisplayModel

#Region "Private Member"

            Private _strSP_ID As String
            Private _intPracticeID As Integer
            Private _strPractice_Name As String
            Private _strPractice_Name_Chi As String
            Private _strService_Category_Code As String

            Private _strPractice_Status As String
            Private _intBankAcctID As Integer
            Private _strBank_Account_No As String
            Private _strBank_Acc_Holder As String
            Private _strBankAcct_Status As String

            Private _strBankAccountKey As String
            Private _strRoom As String
            Private _strFloor As String
            Private _strBlock As String
            Private _strBuilding As String

            Private _strBuilding_Chi As String
            Private _strDistrict As String
            Private _intAddress_Code As Nullable(Of Integer)

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
            ' -----------------------------------------------------------------------------------------
            Private _udtProfession As Profession.ProfessionModel
            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            Private _strPhone_Daytime As String
#End Region

#Region "Property"

            Public Property SPID() As String
                Get
                    Return Me._strSP_ID
                End Get
                Set(ByVal value As String)
                    Me._strSP_ID = value
                End Set
            End Property

            Public Property PracticeID() As Integer
                Get
                    Return Me._intPracticeID
                End Get
                Set(ByVal value As Integer)
                    Me._intPracticeID = value
                End Set
            End Property

            Public Property PracticeName() As String
                Get
                    Return Me._strPractice_Name
                End Get
                Set(ByVal value As String)
                    Me._strPractice_Name = value
                End Set
            End Property

            Public Property PracticeNameChi() As String
                Get
                    Return Me._strPractice_Name_Chi
                End Get
                Set(ByVal value As String)
                    Me._strPractice_Name_Chi = value
                End Set
            End Property

            Public Property ServiceCategoryCode() As String
                Get
                    Return Me._strService_Category_Code
                End Get
                Set(ByVal value As String)
                    Me._strService_Category_Code = value
                End Set
            End Property

            Public Property PracticeStatus() As String
                Get
                    Return Me._strPractice_Status
                End Get
                Set(ByVal value As String)
                    Me._strPractice_Status = value
                End Set
            End Property

            Public Property BankAcctID() As Integer
                Get
                    Return Me._intBankAcctID
                End Get
                Set(ByVal value As Integer)
                    Me._intBankAcctID = value
                End Set
            End Property

            Public Property BankAccountNo() As String
                Get
                    Return Me._strBank_Account_No
                End Get
                Set(ByVal value As String)
                    Me._strBank_Account_No = value
                End Set
            End Property

            Public Property BankAccHolder() As String
                Get
                    Return Me._strBank_Acc_Holder
                End Get
                Set(ByVal value As String)
                    Me._strBank_Acc_Holder = value
                End Set
            End Property

            Public Property BankAcctStatus() As String
                Get
                    Return Me._strBankAcct_Status
                End Get
                Set(ByVal value As String)
                    Me._strBankAcct_Status = value
                End Set
            End Property

            Public Property BankAccountKey() As String
                Get
                    Return Me._strBankAccountKey
                End Get
                Set(ByVal value As String)
                    Me._strBankAccountKey = value
                End Set
            End Property

            Public Property Room() As String
                Get
                    Return Me._strRoom
                End Get
                Set(ByVal value As String)
                    Me._strRoom = value
                End Set
            End Property

            Public Property Floor() As String
                Get
                    Return Me._strFloor
                End Get
                Set(ByVal value As String)
                    Me._strFloor = value
                End Set
            End Property

            Public Property Block() As String
                Get
                    Return Me._strBlock
                End Get
                Set(ByVal value As String)
                    Me._strBlock = value
                End Set
            End Property

            Public Property Building() As String
                Get
                    Return Me._strBuilding
                End Get
                Set(ByVal value As String)
                    Me._strBuilding = value
                End Set
            End Property

            Public Property BuildingChi() As String
                Get
                    Return Me._strBuilding_Chi
                End Get
                Set(ByVal value As String)
                    Me._strBuilding_Chi = value
                End Set
            End Property

            Public Property District() As String
                Get
                    Return Me._strDistrict
                End Get
                Set(ByVal value As String)
                    Me._strDistrict = value
                End Set
            End Property

            Public Property AddressCode() As Nullable(Of Integer)
                Get
                    Return Me._intAddress_Code
                End Get
                Set(ByVal value As Nullable(Of Integer))
                    Me._intAddress_Code = value
                End Set
            End Property

            Public Property PhoneDaytime() As String
                Get
                    Return _strPhone_Daytime
                End Get
                Set(ByVal value As String)
                    _strPhone_Daytime = value
                End Set
            End Property


            ' Addition Field

            Private _blnDisplayEngOnly As Boolean = False
            ' Indicate the Mirgration Complete Setting is turn off or not: Turn off: Display Eng Only = True
            Public Property DisplayEngOnly() As Boolean
                Get
                    Return Me._blnDisplayEngOnly
                End Get
                Set(ByVal value As Boolean)
                    Me._blnDisplayEngOnly = value
                End Set
            End Property

            Private _blnPracticeNameDisplayChi As Boolean = False
            ' Indicate the Current Display is Chinese Description or not
            Public Property PracticeNameDisplayChi() As Boolean
                Get
                    Return Me._blnPracticeNameDisplayChi
                End Get
                Set(ByVal value As Boolean)
                    Me._blnPracticeNameDisplayChi = value
                End Set
            End Property

            ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
            ' -----------------------------------------------------------------------------------------
            Public ReadOnly Property Profession() As Profession.ProfessionModel
                Get
                    If Me._udtProfession Is Nothing Then
                        _udtProfession = (New Profession.ProfessionBLL).GetProfessionListByServiceCategoryCode(Me._strService_Category_Code)
                    End If

                    Return _udtProfession
                End Get
            End Property
            ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Private _strRegistrationCode As String = String.Empty
            Public Property RegistrationCode() As String
                Get
                    Return _strRegistrationCode
                End Get
                Set(ByVal value As String)
                    _strRegistrationCode = UCase(value).Trim
                End Set
            End Property

            Private _strProvideDHCService As String = String.Empty
            Public ReadOnly Property ProvideDHCService() As String
                Get
                    If String.IsNullOrEmpty(_strProvideDHCService) Then
                        Dim udtProfessionalBLL As New Professional.ProfessionalBLL

                        If udtProfessionalBLL.CheckDHCSPMapping(Me.ServiceCategoryCode, Me.RegistrationCode) Then
                            _strProvideDHCService = YesNo.Yes
                        Else
                            _strProvideDHCService = YesNo.No
                        End If
                    End If

                    Return _strProvideDHCService
                End Get
            End Property
            ' CRE19-006 (DHC) [End][Winnie]

#End Region

#Region "Constructor"

            Private Sub New()
            End Sub

            Sub New(ByVal drPracticeBank As DataRow)
                Me._strSP_ID = drPracticeBank("SP_ID").ToString().Trim()
                Me._intPracticeID = CInt(drPracticeBank("PracticeID"))
                Me._strPractice_Name = drPracticeBank("Practice_Name").ToString().Trim()
                Me._strPractice_Name_Chi = drPracticeBank("Practice_Name_Chi").ToString().Trim()
                Me._strService_Category_Code = drPracticeBank("Service_Category_Code").ToString().Trim()

                ' CRE19-006 (DHC) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                Me._strRegistrationCode = drPracticeBank("Registration_Code").ToString().Trim()
                ' CRE19-006 (DHC) [End][Winnie]

                Me._strPractice_Status = drPracticeBank("Practice_Status").ToString().Trim()
                Me._intBankAcctID = drPracticeBank("BankAcctID").ToString().Trim()
                Me._strBank_Account_No = drPracticeBank("Bank_Account_No").ToString().Trim()
                Me._strBank_Acc_Holder = drPracticeBank("Bank_Acc_Holder").ToString().Trim()
                Me._strBankAcct_Status = drPracticeBank("BankAcct_Status").ToString().Trim()

                Me._strBankAccountKey = drPracticeBank("BankAccountKey").ToString().Trim()


                If drPracticeBank.IsNull("Room") Then
                    Me._strRoom = String.Empty
                Else
                    Me._strRoom = drPracticeBank("Room").ToString().Trim()
                End If

                If drPracticeBank.IsNull("Floor") Then
                    Me._strFloor = String.Empty
                Else
                    Me._strFloor = drPracticeBank("Floor").ToString().Trim()
                End If

                If drPracticeBank.IsNull("Block") Then
                    Me._strBlock = String.Empty
                Else
                    Me._strBlock = drPracticeBank("Block").ToString().Trim()
                End If

                If drPracticeBank.IsNull("Building") Then
                    Me._strBuilding = String.Empty
                Else
                    Me._strBuilding = drPracticeBank("Building").ToString().Trim()
                End If

                If drPracticeBank.IsNull("Building_Chi") Then
                    Me._strBuilding_Chi = String.Empty
                Else
                    Me._strBuilding_Chi = drPracticeBank("Building_Chi").ToString().Trim()
                End If

                If drPracticeBank.IsNull("District") Then
                    Me._strDistrict = String.Empty
                Else
                    Me._strDistrict = drPracticeBank("District").ToString().Trim()
                End If


                If drPracticeBank.IsNull("Address_Code") Then
                    Me._intAddress_Code = Nothing
                Else
                    Me._intAddress_Code = CInt(drPracticeBank("Address_Code"))
                End If

                If drPracticeBank.IsNull("Phone_Daytime") Then
                    Me._strPhone_Daytime = Nothing
                Else
                    Me._strPhone_Daytime = drPracticeBank("Phone_Daytime").ToString().Trim()
                End If


            End Sub

#End Region

        End Class

        <Serializable()> Partial Public Class PracticeDisplayModelCollection
            Inherits System.Collections.ArrayList

            Public Sub New()
            End Sub

            Public Overloads Sub Add(ByVal udtPracticeDisplayModel As PracticeDisplayModel)
                MyBase.Add(udtPracticeDisplayModel)
            End Sub

            Public Overloads Sub Remove(ByVal udtPracticeDisplayModel As PracticeDisplayModel)
                MyBase.Remove(udtPracticeDisplayModel)
            End Sub

            Public Function Filter(ByVal intPracticeID As Integer) As PracticeDisplayModel
                Dim returnPracticeDisplayModel As PracticeDisplayModel = Nothing
                For Each practiceDisplayModel As PracticeDisplayModel In Me
                    If practiceDisplayModel.PracticeID = intPracticeID Then
                        returnPracticeDisplayModel = practiceDisplayModel
                        Exit For
                    End If
                Next

                Return returnPracticeDisplayModel
            End Function

            Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As PracticeDisplayModel
                Get
                    Return CType(MyBase.Item(intIndex), PracticeDisplayModel)
                End Get
            End Property
        End Class
#End Region

    End Class
End Namespace

