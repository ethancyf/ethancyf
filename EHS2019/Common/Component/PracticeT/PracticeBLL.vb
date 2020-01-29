Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Component.Address
Imports Common.Component.BankAcct
Imports Common.Component.Professional

Imports Common.Component.ServiceProviderT
Imports Common.Component.PracticeT

Namespace Component.PracticeT
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
            udtNewPracticeModel.PracticeName = udtOldPracticeModel.PracticeName
            udtNewPracticeModel.PracticeType = udtOldPracticeModel.PracticeType
            udtNewPracticeModel.PracticeAddress = udtOldPracticeModel.PracticeAddress
            udtNewPracticeModel.ProfessionalSeq = udtOldPracticeModel.ProfessionalSeq
            udtNewPracticeModel.RecordStatus = udtOldPracticeModel.RecordStatus
            udtNewPracticeModel.SubmitMethod = udtOldPracticeModel.SubmitMethod
            udtNewPracticeModel.DelistStatus = udtOldPracticeModel.DelistStatus
            udtNewPracticeModel.Remark = udtOldPracticeModel.Remark
            udtNewPracticeModel.CreateDtm = udtOldPracticeModel.CreateDtm
            udtNewPracticeModel.CreateBy = udtOldPracticeModel.CreateBy
            udtNewPracticeModel.UpdateDtm = udtOldPracticeModel.UpdateDtm
            udtNewPracticeModel.UpdateBy = udtOldPracticeModel.UpdateBy
            udtNewPracticeModel.EffectiveDtm = udtOldPracticeModel.EffectiveDtm
            udtNewPracticeModel.DelistDtm = udtOldPracticeModel.DelistDtm
            udtNewPracticeModel.TSMP = udtOldPracticeModel.TSMP

            udtNewPracticeModel.Professional = udtOldPracticeModel.Professional
            udtNewPracticeModel.BankAcct = udtOldPracticeModel.BankAcct
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

            Dim drPracticeList As SqlDataReader = Nothing
            Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)


            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_PracticeBankAccountEnrolment_get_byERN", prams, drPracticeList)

                While drPracticeList.Read()
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

                    'If drPracticeList.Item("Bank_TSMP") Is DBNull.Value Then
                    '    btyBankTsmp = Nothing
                    'Else
                    '    btyBankTsmp = CType(drPracticeList.Item("Bank_TSMP"), Byte())
                    'End If


                    'If drPracticeList.Item("Practice_TSMP") Is DBNull.Value Then
                    '    btyPracticeTsmp = Nothing
                    'Else
                    '    btyPracticeTsmp = CType(drPracticeList.Item("Practice_TSMP"), Byte())
                    'End If

                    udtPracticeModel = New PracticeModel(String.Empty, _
                                                       CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
                                                       intPracticeDisplaySeq, _
                                                       CType(drPracticeList.Item("Practice_Name"), String).Trim, _
                                                       CType(drPracticeList.Item("Practice_Type"), String).Trim, _
                                                         New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))), _
                                                                                            CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))), _
                                                                                            CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))), _
                                                                                            CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))), _
                                                                                            CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))), _
                                                                                            CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))), _
                                                                                            intAddressCode), _
                                                       CInt(drPracticeList.Item("Professional_Seq")), _
                                                       String.Empty, _
                                                       String.Empty, _
                                                       String.Empty, _
                                                       String.Empty, _
                                                       Nothing, _
                                                       String.Empty, _
                                                       Nothing, _
                                                       String.Empty, _
                                                       Nothing, _
                                                       Nothing, _
                                                       Nothing, _
                                                       New BankAcctModel(String.Empty, _
                                                           CStr(IIf((drPracticeList.Item("Enrolment_Ref_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Enrolment_Ref_No"))), _
                                                           intBankDisplaySeq, _
                                                           intPracticeDisplaySeq, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Name"))), _
                                                           CStr(IIf((drPracticeList.Item("Branch_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Branch_Name"))), _
                                                           CStr(IIf((drPracticeList.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Acc_Holder"))), _
                                                           CStr(IIf((drPracticeList.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Account_No"))), _
                                                           String.Empty, _
                                                           String.Empty, _
                                                           String.Empty, _
                                                           Nothing, _
                                                           String.Empty, _
                                                           Nothing, _
                                                           String.Empty, _
                                                           Nothing), _
                                                        New ProfessionalModel(String.Empty, _
                                                            CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
                                                            CInt(drPracticeList.Item("Professional_Seq")), _
                                                            CType(drPracticeList.Item("Service_Category_Code"), String).Trim, _
                                                            CType(drPracticeList.Item("Registration_Code"), String).Trim, _
                                                            String.Empty, _
                                                            Nothing, _
                                                            Nothing))

                    udtPracticeModelCollection.Add(udtPracticeModel)
                End While
                drPracticeList.Close()
                Return udtPracticeModelCollection
            Catch ex As Exception
                Throw ex
            Finally
                If Not drPracticeList Is Nothing Then
                    drPracticeList.Close()
                End If
            End Try
        End Function

        Public Function GetPracticeBankAcctListFromEnrolmentBOTHByERN(ByVal strERN As String, ByVal udtDB As Database, ByVal strSchemeCode As String) As PracticeModelCollection

            Dim drPracticeList As SqlDataReader = Nothing
            Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)


            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                udtDB.MakeInParam("@scheme", SqlDbType.Char, 5, strSchemeCode)}
                udtDB.RunProc("proc_PracticeBankAccountEnrolmentBOTH_get_byERN", prams, drPracticeList)

                While drPracticeList.Read()
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

                    'If drPracticeList.Item("Bank_TSMP") Is DBNull.Value Then
                    '    btyBankTsmp = Nothing
                    'Else
                    '    btyBankTsmp = CType(drPracticeList.Item("Bank_TSMP"), Byte())
                    'End If


                    'If drPracticeList.Item("Practice_TSMP") Is DBNull.Value Then
                    '    btyPracticeTsmp = Nothing
                    'Else
                    '    btyPracticeTsmp = CType(drPracticeList.Item("Practice_TSMP"), Byte())
                    'End If

                    udtPracticeModel = New PracticeModel(String.Empty, _
                                                       CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
                                                       intPracticeDisplaySeq, _
                                                       CType(drPracticeList.Item("Practice_Name"), String).Trim, _
                                                       CType(drPracticeList.Item("Practice_Type"), String).Trim, _
                                                         New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))), _
                                                                                            CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))), _
                                                                                            CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))), _
                                                                                            CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))), _
                                                                                            CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))), _
                                                                                            CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))), _
                                                                                            intAddressCode), _
                                                       CInt(drPracticeList.Item("Professional_Seq")), _
                                                       String.Empty, _
                                                       String.Empty, _
                                                       String.Empty, _
                                                       String.Empty, _
                                                       Nothing, _
                                                       String.Empty, _
                                                       Nothing, _
                                                       String.Empty, _
                                                       Nothing, _
                                                       Nothing, _
                                                       Nothing, _
                                                       New BankAcctModel(String.Empty, _
                                                           CStr(IIf((drPracticeList.Item("Enrolment_Ref_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Enrolment_Ref_No"))), _
                                                           intBankDisplaySeq, _
                                                           intPracticeDisplaySeq, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Name"))), _
                                                           CStr(IIf((drPracticeList.Item("Branch_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Branch_Name"))), _
                                                           CStr(IIf((drPracticeList.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Acc_Holder"))), _
                                                           CStr(IIf((drPracticeList.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Account_No"))), _
                                                           String.Empty, _
                                                           String.Empty, _
                                                           String.Empty, _
                                                           Nothing, _
                                                           String.Empty, _
                                                           Nothing, _
                                                           String.Empty, _
                                                           Nothing), _
                                                        New ProfessionalModel(String.Empty, _
                                                            CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
                                                            CInt(drPracticeList.Item("Professional_Seq")), _
                                                            CType(drPracticeList.Item("Service_Category_Code"), String).Trim, _
                                                            CType(drPracticeList.Item("Registration_Code"), String).Trim, _
                                                            String.Empty, _
                                                            Nothing, _
                                                            Nothing))

                    udtPracticeModelCollection.Add(udtPracticeModel)
                End While
                drPracticeList.Close()
                Return udtPracticeModelCollection
            Catch ex As Exception
                Throw ex
            Finally
                If Not drPracticeList Is Nothing Then
                    drPracticeList.Close()
                End If
            End Try
        End Function

        Public Function GetPracticeBankAcctListFromPermanentBySPID_NoReader(ByVal strSPID As String, ByVal udtDB As Database) As PracticeModelCollection
            Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim dtmPracticeEffectiveDtm As Nullable(Of DateTime)
            Dim dtmPracticeDelistDtm As Nullable(Of DateTime)
            Dim dtmBankEffectiveDtm As Nullable(Of DateTime)
            Dim dtmBankDelistDtm As Nullable(Of DateTime)

            Dim btyBankTsmp As Byte()
            Dim btyPracticeTsmp As Byte()

            Dim dtRaw As New DataTable
            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
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

                    If IsDBNull(drRaw.Item("Practice_Effective_Dtm")) Then
                        dtmPracticeEffectiveDtm = Nothing
                    Else
                        dtmPracticeEffectiveDtm = Convert.ToDateTime(drRaw.Item("Practice_Effective_Dtm"))
                    End If

                    If IsDBNull(drRaw.Item("Practice_Delist_Dtm")) Then
                        dtmPracticeDelistDtm = Nothing
                    Else
                        dtmPracticeDelistDtm = Convert.ToDateTime(drRaw.Item("Practice_Delist_Dtm"))
                    End If

                    If IsDBNull(drRaw.Item("Bank_Effective_Dtm")) Then
                        dtmBankEffectiveDtm = Nothing
                    Else
                        dtmBankEffectiveDtm = Convert.ToDateTime(drRaw.Item("Bank_Effective_Dtm"))
                    End If

                    If IsDBNull(drRaw.Item("Bank_Delist_Dtm")) Then
                        dtmBankDelistDtm = Nothing
                    Else
                        dtmBankDelistDtm = Convert.ToDateTime(drRaw.Item("Bank_Delist_Dtm"))
                    End If

                    'If IsDBNull(drRaw.Item("Bank_TSMP")) Then
                    '    btyBankTsmp = Nothing
                    'Else
                    '    btyBankTsmp = drRaw.Item("Bank_TSMP")
                    'End If

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


                    'If IsDBNull(drRaw.Item("Practice_TSMP")) Then

                    'Else

                    'End If

                    udtPracticeModel = New PracticeModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                       String.Empty, _
                                                       CInt(drRaw.Item("Display_Seq")), _
                                                       CType(drRaw.Item("Practice_Name"), String).Trim, _
                                                       String.Empty, _
                                                         New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))), _
                                                                                            CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))).Trim, _
                                                                                            intAddressCode), _
                                                       CInt(drRaw.Item("Professional_Seq")), _
                                                       CStr(IIf((drRaw.Item("Practice_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Record_Status"))).Trim, _
                                                       CStr(IIf((drRaw.Item("Practice_Delist_Status") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Delist_Status"))).Trim, _
                                                       CStr(IIf((drRaw.Item("Practice_Submission_Method") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Submission_Method"))).Trim, _
                                                       CStr(IIf((drRaw.Item("Practice_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Remark"))).Trim, _
                                                       CType(drRaw.Item("Practice_Create_Dtm"), DateTime), _
                                                       CStr(IIf((drRaw.Item("Practice_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Create_By"))).Trim, _
                                                       CType(drRaw.Item("Practice_Update_Dtm"), DateTime), _
                                                       CStr(IIf((drRaw.Item("Practice_Update_By") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Update_By"))).Trim, _
                                                       dtmPracticeEffectiveDtm, _
                                                       dtmPracticeDelistDtm, _
                                                       btyPracticeTsmp, _
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
                                                           btyBankTsmp), _
                                                        New ProfessionalModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                            String.Empty, _
                                                            CInt(drRaw.Item("Professional_Seq")), _
                                                            CType(drRaw.Item("Service_Category_Code"), String).Trim, _
                                                            CType(drRaw.Item("Registration_Code"), String).Trim, _
                                                            CStr(IIf((drRaw.Item("Professional_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Professional_Record_Status"))).Trim, _
                                                            CType(drRaw.Item("Professional_Create_Dtm"), DateTime), _
                                                           CStr(IIf((drRaw.Item("Professional_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Professional_Create_By"))).Trim))

                    udtPracticeModelCollection.Add(udtPracticeModel)
                Next
                Return udtPracticeModelCollection
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function GetPracticeBankAcctListFromPermanentBySPID(ByVal strSPID As String, ByVal udtDB As Database) As PracticeModelCollection
            Dim drPracticeList As SqlDataReader = Nothing
            Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)

            Dim dtmPracticeEffectiveDtm As Nullable(Of DateTime)
            Dim dtmPracticeDelistDtm As Nullable(Of DateTime)
            Dim dtmBankEffectiveDtm As Nullable(Of DateTime)
            Dim dtmBankDelistDtm As Nullable(Of DateTime)

            Dim btyBankTsmp As Byte()
            Dim btyPracticeTsmp As Byte()

            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_PracticeBankAccount_get_bySPID", prams, drPracticeList)

                While drPracticeList.Read()
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

                    'If IsDBNull(drPracticeList.Item("Practice_Effective_Dtm")) Then
                    '    dtmPracticeEffectiveDtm = Nothing
                    'Else
                    '    dtmPracticeEffectiveDtm = Convert.ToDateTime(drPracticeList.Item("Practice_Effective_Dtm"))
                    'End If
                    dtmPracticeEffectiveDtm = Nothing
                    'If IsDBNull(drPracticeList.Item("Practice_Delist_Dtm")) Then
                    '    dtmPracticeDelistDtm = Nothing
                    'Else
                    '    dtmPracticeDelistDtm = Convert.ToDateTime(drPracticeList.Item("Practice_Delist_Dtm"))
                    'End If
                    dtmPracticeDelistDtm = Nothing
                    'If IsDBNull(drPracticeList.Item("Bank_Effective_Dtm")) Then
                    '    dtmBankEffectiveDtm = Nothing
                    'Else
                    '    dtmBankEffectiveDtm = Convert.ToDateTime(drPracticeList.Item("Bank_Effective_Dtm"))
                    'End If
                    dtmBankEffectiveDtm = Nothing
                    'If IsDBNull(drPracticeList.Item("Bank_Delist_Dtm")) Then
                    '    dtmBankDelistDtm = Nothing
                    'Else
                    '    dtmBankDelistDtm = Convert.ToDateTime(drPracticeList.Item("Bank_Delist_Dtm"))
                    'End If
                    dtmBankDelistDtm = Nothing
                    If drPracticeList.Item("Bank_TSMP") Is DBNull.Value Then

                        btyBankTsmp = Nothing
                    Else
                        btyBankTsmp = CType(drPracticeList.Item("Bank_TSMP"), Byte())
                    End If

                    If drPracticeList.Item("Practice_TSMP") Is DBNull.Value Then
                        btyPracticeTsmp = Nothing
                    Else
                        btyPracticeTsmp = CType(drPracticeList.Item("Practice_TSMP"), Byte())
                    End If

                    udtPracticeModel = New PracticeModel(CStr(IIf((drPracticeList.Item("SP_ID") Is DBNull.Value), String.Empty, drPracticeList.Item("SP_ID"))).Trim, _
                                                       String.Empty, _
                                                       CInt(drPracticeList.Item("Display_Seq")), _
                                                       CType(drPracticeList.Item("Practice_Name"), String).Trim, _
                                                       String.Empty, _
                                                         New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))), _
                                                                                            CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))).Trim, _
                                                                                            intAddressCode), _
                                                       CInt(drPracticeList.Item("Professional_Seq")), _
                                                       CStr(IIf((drPracticeList.Item("Practice_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Record_Status"))).Trim, _
                                                       String.Empty, _
                                                       CStr(IIf((drPracticeList.Item("Practice_Submission_Method") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Submission_Method"))).Trim, _
                                                       CStr(IIf((drPracticeList.Item("Practice_Remark") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Remark"))).Trim, _
                                                       CType(drPracticeList.Item("Practice_Create_Dtm"), DateTime), _
                                                       CStr(IIf((drPracticeList.Item("Practice_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Create_By"))).Trim, _
                                                       CType(drPracticeList.Item("Practice_Update_Dtm"), DateTime), _
                                                       CStr(IIf((drPracticeList.Item("Practice_Update_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Update_By"))).Trim, _
                                                       dtmPracticeEffectiveDtm, _
                                                       dtmPracticeDelistDtm, _
                                                       btyPracticeTsmp, _
                                                       New BankAcctModel(CStr(IIf((drPracticeList.Item("SP_ID") Is DBNull.Value), String.Empty, drPracticeList.Item("SP_ID"))).Trim, _
                                                           String.Empty, _
                                                           intBankDisplaySeq, _
                                                           intPracticeDisplaySeq, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Name"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Branch_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Branch_Name"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Acc_Holder"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Account_No"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Record_Status"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Submission_Method") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Submission_Method"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Remark") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Remark"))).Trim, _
                                                           Convert.ToDateTime(IIf(drPracticeList.Item("Bank_Create_Dtm") Is DBNull.Value, Nothing, drPracticeList.Item("Bank_Create_Dtm"))), _
                                                           CStr(IIf((drPracticeList.Item("Bank_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Create_By"))).Trim, _
                                                           Convert.ToDateTime(IIf(drPracticeList.Item("Bank_Update_Dtm") Is DBNull.Value, Nothing, drPracticeList.Item("Bank_Update_Dtm"))), _
                                                           CStr(IIf((drPracticeList.Item("Bank_Update_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Update_By"))).Trim, _
                                                           btyBankTsmp), _
                                                        New ProfessionalModel(CStr(IIf((drPracticeList.Item("SP_ID") Is DBNull.Value), String.Empty, drPracticeList.Item("SP_ID"))).Trim, _
                                                            String.Empty, _
                                                            CInt(drPracticeList.Item("Professional_Seq")), _
                                                            CType(drPracticeList.Item("Service_Category_Code"), String).Trim, _
                                                            CType(drPracticeList.Item("Registration_Code"), String).Trim, _
                                                            CStr(IIf((drPracticeList.Item("Professional_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Professional_Record_Status"))).Trim, _
                                                            CType(drPracticeList.Item("Professional_Create_Dtm"), DateTime), _
                                                           CStr(IIf((drPracticeList.Item("Professional_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Professional_Create_By"))).Trim))

                    udtPracticeModelCollection.Add(udtPracticeModel)
                End While
                drPracticeList.Close()
                Return udtPracticeModelCollection
            Catch ex As Exception
                Throw ex
            Finally
                If Not drPracticeList Is Nothing Then
                    drPracticeList.Close()
                End If
            End Try
        End Function


        Public Function GetPracticeBankAcctListFromPermanentMaintenanceBySPID(ByVal strSPID As String, ByVal udtDB As Database) As PracticeModelCollection
            Dim drPracticeList As SqlDataReader = Nothing
            Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)

            Dim dtmPracticeEffectiveDtm As Nullable(Of DateTime)
            Dim dtmPracticeDelistDtm As Nullable(Of DateTime)
            Dim dtmBankEffectiveDtm As Nullable(Of DateTime)
            Dim dtmBankDelistDtm As Nullable(Of DateTime)

            Dim btyBankTsmp As Byte()
            Dim btyPracticeTsmp As Byte()

            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_PracticeBankAccountSPAccMaintenance_get_bySPID", prams, drPracticeList)

                While drPracticeList.Read()
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

                    If IsDBNull(drPracticeList.Item("Practice_Effective_Dtm")) Then
                        dtmPracticeEffectiveDtm = Nothing
                    Else
                        dtmPracticeEffectiveDtm = Convert.ToDateTime(drPracticeList.Item("Practice_Effective_Dtm"))
                    End If

                    If IsDBNull(drPracticeList.Item("Practice_Delist_Dtm")) Then
                        dtmPracticeDelistDtm = Nothing
                    Else
                        dtmPracticeDelistDtm = Convert.ToDateTime(drPracticeList.Item("Practice_Delist_Dtm"))
                    End If

                    If IsDBNull(drPracticeList.Item("Bank_Effective_Dtm")) Then
                        dtmBankEffectiveDtm = Nothing
                    Else
                        dtmBankEffectiveDtm = Convert.ToDateTime(drPracticeList.Item("Bank_Effective_Dtm"))
                    End If

                    If IsDBNull(drPracticeList.Item("Bank_Delist_Dtm")) Then
                        dtmBankDelistDtm = Nothing
                    Else
                        dtmBankDelistDtm = Convert.ToDateTime(drPracticeList.Item("Bank_Delist_Dtm"))
                    End If


                    If drPracticeList.Item("Bank_TSMP") Is DBNull.Value Then
                        btyBankTsmp = Nothing
                    Else
                        btyBankTsmp = CType(drPracticeList.Item("Bank_TSMP"), Byte())
                    End If

                    If drPracticeList.Item("Practice_TSMP") Is DBNull.Value Then
                        btyPracticeTsmp = Nothing
                    Else
                        btyPracticeTsmp = CType(drPracticeList.Item("Practice_TSMP"), Byte())
                    End If

                    udtPracticeModel = New PracticeModel(CStr(IIf((drPracticeList.Item("SP_ID") Is DBNull.Value), String.Empty, drPracticeList.Item("SP_ID"))).Trim, _
                                                       String.Empty, _
                                                       CInt(drPracticeList.Item("Display_Seq")), _
                                                       CType(drPracticeList.Item("Practice_Name"), String).Trim, _
                                                       CType(drPracticeList.Item("Practice_Type"), String).Trim, _
                                                         New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))), _
                                                                                            CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))).Trim, _
                                                                                            intAddressCode), _
                                                       CInt(drPracticeList.Item("Professional_Seq")), _
                                                       CStr(IIf((drPracticeList.Item("Practice_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Record_Status"))).Trim, _
                                                       CStr(IIf((drPracticeList.Item("Practice_Delist_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Delist_Status"))).Trim, _
                                                       CStr(IIf((drPracticeList.Item("Practice_Submission_Method") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Submission_Method"))).Trim, _
                                                       CStr(IIf((drPracticeList.Item("Practice_Remark") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Remark"))).Trim, _
                                                       CType(drPracticeList.Item("Practice_Create_Dtm"), DateTime), _
                                                       CStr(IIf((drPracticeList.Item("Practice_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Create_By"))).Trim, _
                                                       CType(drPracticeList.Item("Practice_Update_Dtm"), DateTime), _
                                                       CStr(IIf((drPracticeList.Item("Practice_Update_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Update_By"))).Trim, _
                                                       dtmPracticeEffectiveDtm, _
                                                       dtmPracticeDelistDtm, _
                                                       btyPracticeTsmp, _
                                                       New BankAcctModel(CStr(IIf((drPracticeList.Item("SP_ID") Is DBNull.Value), String.Empty, drPracticeList.Item("SP_ID"))).Trim, _
                                                           String.Empty, _
                                                           intBankDisplaySeq, _
                                                           intPracticeDisplaySeq, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Name"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Branch_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Branch_Name"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Acc_Holder"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Account_No"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Record_Status"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Submission_Method") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Submission_Method"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Remark") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Remark"))).Trim, _
                                                           Convert.ToDateTime(IIf(drPracticeList.Item("Bank_Create_Dtm") Is DBNull.Value, Nothing, drPracticeList.Item("Bank_Create_Dtm"))), _
                                                           CStr(IIf((drPracticeList.Item("Bank_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Create_By"))).Trim, _
                                                           Convert.ToDateTime(IIf(drPracticeList.Item("Bank_Update_Dtm") Is DBNull.Value, Nothing, drPracticeList.Item("Bank_Update_Dtm"))), _
                                                           CStr(IIf((drPracticeList.Item("Bank_Update_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Update_By"))).Trim, _
                                                           btyBankTsmp), _
                                                        New ProfessionalModel(CStr(IIf((drPracticeList.Item("SP_ID") Is DBNull.Value), String.Empty, drPracticeList.Item("SP_ID"))).Trim, _
                                                            String.Empty, _
                                                            CInt(drPracticeList.Item("Professional_Seq")), _
                                                            CType(drPracticeList.Item("Service_Category_Code"), String).Trim, _
                                                            CType(drPracticeList.Item("Registration_Code"), String).Trim, _
                                                            CStr(IIf((drPracticeList.Item("Professional_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Professional_Record_Status"))).Trim, _
                                                            CType(drPracticeList.Item("Professional_Create_Dtm"), DateTime), _
                                                           CStr(IIf((drPracticeList.Item("Professional_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Professional_Create_By"))).Trim))

                    udtPracticeModelCollection.Add(udtPracticeModel)
                End While
                drPracticeList.Close()
                Return udtPracticeModelCollection
            Catch ex As Exception
                Throw ex
            Finally
                If Not drPracticeList Is Nothing Then
                    drPracticeList.Close()
                End If
            End Try
        End Function

        Public Function GetPracticeBankAcctListFromStagingByERN_NoReader(ByVal strERN As String, ByVal udtDB As Database) As PracticeModelCollection

            Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)

            Dim btyBankTsmp As Byte()
            Dim btyPracticeTsmp As Byte()

            Dim dtRaw As New DataTable()
            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
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

                    udtPracticeModel = New PracticeModel(CStr(IIf((drRaw.Item("SP_ID") Is DBNull.Value), String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                       CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
                                                       CInt(drRaw.Item("Display_Seq")), _
                                                       CType(drRaw.Item("Practice_Name"), String).Trim, _
                                                       CType(drRaw.Item("Practice_Type"), String).Trim, _
                                                         New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))), _
                                                                                            CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))).Trim, _
                                                                                            CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))).Trim, _
                                                                                            intAddressCode), _
                                                       CInt(drRaw.Item("Professional_Seq")), _
                                                       CStr(IIf((drRaw.Item("Practice_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Record_Status"))).Trim, _
                                                       String.Empty, _
                                                       CStr(IIf((drRaw.Item("Practice_Submission_Method") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Submission_Method"))).Trim, _
                                                       CStr(IIf((drRaw.Item("Practice_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Remark"))).Trim, _
                                                       CType(drRaw.Item("Practice_Create_Dtm"), DateTime), _
                                                       CStr(IIf((drRaw.Item("Practice_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Create_By"))).Trim, _
                                                       CType(drRaw.Item("Practice_Update_Dtm"), DateTime), _
                                                       CStr(IIf((drRaw.Item("Practice_Update_By") Is DBNull.Value), String.Empty, drRaw.Item("Practice_Update_By"))).Trim, _
                                                       Nothing, _
                                                       Nothing, _
                                                       btyPracticeTsmp, _
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
                                                           btyBankTsmp), _
                                                        New ProfessionalModel(String.Empty, _
                                                            CType(drRaw.Item("Enrolment_Ref_No"), String).Trim, _
                                                            CInt(drRaw.Item("Professional_Seq")), _
                                                            CType(drRaw.Item("Service_Category_Code"), String).Trim, _
                                                            CType(drRaw.Item("Registration_Code"), String).Trim, _
                                                            CStr(IIf((drRaw.Item("Professional_Record_Status") Is DBNull.Value), String.Empty, drRaw.Item("Professional_Record_Status"))).Trim, _
                                                            CType(drRaw.Item("Professional_Create_Dtm"), DateTime), _
                                                           CStr(IIf((drRaw.Item("Professional_Create_By") Is DBNull.Value), String.Empty, drRaw.Item("Professional_Create_By"))).Trim))

                    udtPracticeModelCollection.Add(udtPracticeModel)
                Next

                Return udtPracticeModelCollection
            Catch ex As Exception
                Throw ex
            Finally
            End Try
        End Function

        Public Function GetPracticeBankAcctListFromStagingByERNBankStagingStatus(ByVal strERN As String, ByVal udtDB As Database) As PracticeModelCollection
            Dim drPracticeList As SqlDataReader = Nothing
            Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)

            Dim btyBankTsmp As Byte()
            Dim btyPracticeTsmp As Byte()

            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_PracticeBankAccountStaging_get_byERNBankStagingStatus", prams, drPracticeList)

                While drPracticeList.Read()
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

                    If drPracticeList.Item("Bank_TSMP") Is DBNull.Value Then
                        btyBankTsmp = Nothing
                    Else
                        btyBankTsmp = CType(drPracticeList.Item("Bank_TSMP"), Byte())
                    End If

                    If drPracticeList.Item("Practice_TSMP") Is DBNull.Value Then
                        btyPracticeTsmp = Nothing
                    Else
                        btyPracticeTsmp = CType(drPracticeList.Item("Practice_TSMP"), Byte())
                    End If

                    udtPracticeModel = New PracticeModel(CStr(IIf((drPracticeList.Item("SP_ID") Is DBNull.Value), String.Empty, drPracticeList.Item("SP_ID"))).Trim, _
                                                       CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
                                                       CInt(drPracticeList.Item("Display_Seq")), _
                                                       CType(drPracticeList.Item("Practice_Name"), String).Trim, _
                                                       CType(drPracticeList.Item("Practice_Type"), String).Trim, _
                                                         New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))), _
                                                                                            CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))).Trim, _
                                                                                            intAddressCode), _
                                                       CInt(drPracticeList.Item("Professional_Seq")), _
                                                       CStr(IIf((drPracticeList.Item("Practice_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Record_Status"))).Trim, _
                                                       String.Empty, _
                                                       CStr(IIf((drPracticeList.Item("Practice_Submission_Method") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Submission_Method"))).Trim, _
                                                       CStr(IIf((drPracticeList.Item("Practice_Remark") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Remark"))).Trim, _
                                                       CType(drPracticeList.Item("Practice_Create_Dtm"), DateTime), _
                                                       CStr(IIf((drPracticeList.Item("Practice_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Create_By"))).Trim, _
                                                       CType(drPracticeList.Item("Practice_Update_Dtm"), DateTime), _
                                                       CStr(IIf((drPracticeList.Item("Practice_Update_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Update_By"))).Trim, _
                                                       Nothing, _
                                                       Nothing, _
                                                       btyPracticeTsmp, _
                                                       New BankAcctModel(CStr(IIf((drPracticeList.Item("SP_ID") Is DBNull.Value), String.Empty, drPracticeList.Item("SP_ID"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Enrolment_Ref_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Enrolment_Ref_No"))).Trim, _
                                                           intBankDisplaySeq, _
                                                           intPracticeDisplaySeq, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Name"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Branch_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Branch_Name"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Acc_Holder"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Account_No"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Record_Status"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Submission_Method") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Submission_Method"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Remark") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Remark"))).Trim, _
                                                           Convert.ToDateTime(IIf(drPracticeList.Item("Bank_Create_Dtm") Is DBNull.Value, Nothing, drPracticeList.Item("Bank_Create_Dtm"))), _
                                                           CStr(IIf((drPracticeList.Item("Bank_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Create_By"))).Trim, _
                                                           Convert.ToDateTime(IIf(drPracticeList.Item("Bank_Update_Dtm") Is DBNull.Value, Nothing, drPracticeList.Item("Bank_Update_Dtm"))), _
                                                           CStr(IIf((drPracticeList.Item("Bank_Update_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Update_By"))).Trim, _
                                                           btyBankTsmp), _
                                                        New ProfessionalModel(String.Empty, _
                                                            CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
                                                            CInt(drPracticeList.Item("Professional_Seq")), _
                                                            CType(drPracticeList.Item("Service_Category_Code"), String).Trim, _
                                                            CType(drPracticeList.Item("Registration_Code"), String).Trim, _
                                                            CStr(IIf((drPracticeList.Item("Professional_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Professional_Record_Status"))).Trim, _
                                                            CType(drPracticeList.Item("Professional_Create_Dtm"), DateTime), _
                                                           CStr(IIf((drPracticeList.Item("Professional_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Professional_Create_By"))).Trim))

                    udtPracticeModelCollection.Add(udtPracticeModel)
                End While
                drPracticeList.Close()
                Return udtPracticeModelCollection
            Catch ex As Exception
                Throw ex
            Finally
                If Not drPracticeList Is Nothing Then
                    drPracticeList.Close()
                End If
            End Try
        End Function

        Public Function GetPracticeBankAcctListFromStagingByERN(ByVal strERN As String, ByVal udtDB As Database) As PracticeModelCollection
            Dim drPracticeList As SqlDataReader = Nothing
            Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection()
            Dim udtPracticeModel As PracticeModel

            Dim intAddressCode As Nullable(Of Integer)
            Dim intBankDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)

            Dim btyBankTsmp As Byte()
            Dim btyPracticeTsmp As Byte()


            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_PracticeBankAccountStaging_get_byERN", prams, drPracticeList)

                While drPracticeList.Read()
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

                    If drPracticeList.Item("Bank_TSMP") Is DBNull.Value Then
                        btyBankTsmp = Nothing
                    Else
                        btyBankTsmp = CType(drPracticeList.Item("Bank_TSMP"), Byte())
                    End If

                    If drPracticeList.Item("Practice_TSMP") Is DBNull.Value Then
                        btyPracticeTsmp = Nothing
                    Else
                        btyPracticeTsmp = CType(drPracticeList.Item("Practice_TSMP"), Byte())
                    End If

                    udtPracticeModel = New PracticeModel(CStr(IIf((drPracticeList.Item("SP_ID") Is DBNull.Value), String.Empty, drPracticeList.Item("SP_ID"))).Trim, _
                                                       CType(drPracticeList.Item("Enrolment_Ref_No"), String).Trim, _
                                                       CInt(drPracticeList.Item("Display_Seq")), _
                                                       CType(drPracticeList.Item("Practice_Name"), String).Trim, _
                                                       String.Empty, _
                                                         New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))), _
                                                                                            CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))).Trim, _
                                                                                            CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))).Trim, _
                                                                                            intAddressCode), _
                                                       CInt(drPracticeList.Item("Professional_Seq")), _
                                                       CStr(IIf((drPracticeList.Item("Practice_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Record_Status"))).Trim, _
                                                       String.Empty, _
                                                       CStr(IIf((drPracticeList.Item("Practice_Submission_Method") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Submission_Method"))).Trim, _
                                                       CStr(IIf((drPracticeList.Item("Practice_Remark") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Remark"))).Trim, _
                                                       CType(drPracticeList.Item("Practice_Create_Dtm"), DateTime), _
                                                       CStr(IIf((drPracticeList.Item("Practice_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Create_By"))).Trim, _
                                                       CType(drPracticeList.Item("Practice_Update_Dtm"), DateTime), _
                                                       CStr(IIf((drPracticeList.Item("Practice_Update_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Update_By"))).Trim, _
                                                       Nothing, _
                                                       Nothing, _
                                                       btyPracticeTsmp, _
                                                       New BankAcctModel(CStr(IIf((drPracticeList.Item("SP_ID") Is DBNull.Value), String.Empty, drPracticeList.Item("SP_ID"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Enrolment_Ref_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Enrolment_Ref_No"))).Trim, _
                                                           intBankDisplaySeq, _
                                                           intPracticeDisplaySeq, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Name"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Branch_Name") Is DBNull.Value), String.Empty, drPracticeList.Item("Branch_Name"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Acc_Holder") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Acc_Holder"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Account_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Account_No"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Record_Status"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Submission_Method") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Submission_Method"))).Trim, _
                                                           CStr(IIf((drPracticeList.Item("Bank_Remark") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Remark"))).Trim, _
                                                           Convert.ToDateTime(IIf(drPracticeList.Item("Bank_Create_Dtm") Is DBNull.Value, Nothing, drPracticeList.Item("Bank_Create_Dtm"))), _
                                                           CStr(IIf((drPracticeList.Item("Bank_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Create_By"))).Trim, _
                                                           Convert.ToDateTime(IIf(drPracticeList.Item("Bank_Update_Dtm") Is DBNull.Value, Nothing, drPracticeList.Item("Bank_Update_Dtm"))), _
                                                           CStr(IIf((drPracticeList.Item("Bank_Update_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Bank_Update_By"))).Trim, _
                                                           btyBankTsmp), _
                                                            New ProfessionalModel(String.Empty, _
                                                            CStr(IIf((drPracticeList.Item("Enrolment_Ref_No") Is DBNull.Value), String.Empty, drPracticeList.Item("Enrolment_Ref_No"))).Trim, _
                                                            CInt(IIf((drPracticeList.Item("Professional_Seq") Is DBNull.Value), 0, drPracticeList.Item("Professional_Seq"))), _
                                                            CStr(IIf((drPracticeList.Item("Service_Category_Code") Is DBNull.Value), String.Empty, drPracticeList.Item("Service_Category_Code"))).Trim, _
                                                            CStr(IIf((drPracticeList.Item("Registration_Code") Is DBNull.Value), String.Empty, drPracticeList.Item("Registration_Code"))).Trim, _
                                                            CStr(IIf((drPracticeList.Item("Professional_Record_Status") Is DBNull.Value), String.Empty, drPracticeList.Item("Professional_Record_Status"))).Trim, _
                                                            Convert.ToDateTime(IIf(drPracticeList.Item("Professional_Create_Dtm") Is DBNull.Value, Nothing, drPracticeList.Item("Professional_Create_Dtm"))), _
                                                           CStr(IIf((drPracticeList.Item("Professional_Create_By") Is DBNull.Value), String.Empty, drPracticeList.Item("Professional_Create_By"))).Trim))

                    udtPracticeModelCollection.Add(udtPracticeModel)
                End While
                drPracticeList.Close()
                Return udtPracticeModelCollection
            Catch ex As Exception
                Throw ex
            Finally
                If Not drPracticeList Is Nothing Then
                    drPracticeList.Close()
                End If
            End Try
        End Function

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

        Public Function GetPracticeListMOPracticeEnrolmentByERN(ByVal strERN As String, ByVal udtDB As Database) As DataTable
            Dim dt As DataTable
            dt = New DataTable
            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_MOPracticeEnrolment_get_byERN", prams, dt)
            Catch ex As Exception
                Throw ex
            End Try

            Return dt
        End Function

        Public Function AddPracticeListToEnrolment(ByVal udtPracticeModelCollection As PracticeModelCollection, ByVal udtDB As Database) As Boolean
            'Dim i As Integer
            'Dim udtPracticeModel As PracticeModel

            Try
                For Each udtPracticeModel As PracticeModel In udtPracticeModelCollection.Values
                    'For i = 0 To udtPracticeModelCollection.Count - 1
                    'udtPracticeModel = New PracticeModel(udtPracticeModelCollection.Item(i + 1))

                    Dim prams() As SqlParameter = { _
                                   udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                                   udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                                   udtDB.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                                   udtDB.MakeInParam("@practice_type", PracticeModel.PracticeTypeDataType, PracticeModel.PracticeTypeDataSize, udtPracticeModel.PracticeType), _
                                   udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtPracticeModel.PracticeAddress.Room.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Room)), _
                                   udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtPracticeModel.PracticeAddress.Floor.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Floor)), _
                                   udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtPracticeModel.PracticeAddress.Block.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Block)), _
                                   udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtPracticeModel.PracticeAddress.Building.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.Building)), _
                                   udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtPracticeModel.PracticeAddress.ChiBuilding.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.ChiBuilding)), _
                                   udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtPracticeModel.PracticeAddress.District.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.District)), _
                                   udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtPracticeModel.PracticeAddress.Address_Code.HasValue, udtPracticeModel.PracticeAddress.Address_Code, DBNull.Value)), _
                                   udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, udtPracticeModel.ProfessionalSeq)}

                    udtDB.RunProc("proc_PracticeEnrolment_add", prams)
                Next
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddPracticeListToEnrolmentBOTH(ByVal udtPracticeModelCollection As PracticeModelCollection, ByVal udtDB As Database, ByVal strSchemeCode As String) As Boolean
            'Dim i As Integer
            'Dim udtPracticeModel As PracticeModel

            Try
                For Each udtPracticeModel As PracticeModel In udtPracticeModelCollection.Values
                    'For i = 0 To udtPracticeModelCollection.Count - 1
                    'udtPracticeModel = New PracticeModel(udtPracticeModelCollection.Item(i + 1))

                    Dim prams() As SqlParameter = { _
                                   udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                                   udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                                   udtDB.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                                   udtDB.MakeInParam("@practice_type", PracticeModel.PracticeTypeDataType, PracticeModel.PracticeTypeDataSize, udtPracticeModel.PracticeType), _
                                   udtDB.MakeInParam("@room", AddressModel.RoomDataType, AddressModel.RoomDataSize, IIf(udtPracticeModel.PracticeAddress.Room.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Room)), _
                                   udtDB.MakeInParam("@floor", AddressModel.FloorDataType, AddressModel.FloorDataSize, IIf(udtPracticeModel.PracticeAddress.Floor.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Floor)), _
                                   udtDB.MakeInParam("@block", AddressModel.BlockDataType, AddressModel.BlockDataSize, IIf(udtPracticeModel.PracticeAddress.Block.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeAddress.Block)), _
                                   udtDB.MakeInParam("@building", AddressModel.BuildingDataType, AddressModel.BuildingDataSize, IIf(udtPracticeModel.PracticeAddress.Building.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.Building)), _
                                   udtDB.MakeInParam("@building_chi", AddressModel.BuildingChiDataType, AddressModel.BuildingChiDataSize, IIf(udtPracticeModel.PracticeAddress.ChiBuilding.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.ChiBuilding)), _
                                   udtDB.MakeInParam("@district", AddressModel.DistrictDataType, AddressModel.DistrictDataSize, IIf(udtPracticeModel.PracticeAddress.District.Equals(String.Empty) Or udtPracticeModel.PracticeAddress.Address_Code.HasValue, DBNull.Value, udtPracticeModel.PracticeAddress.District)), _
                                   udtDB.MakeInParam("@address_code", AddressModel.AddressCodeDataType, AddressModel.AddressCodeDataSize, IIf(udtPracticeModel.PracticeAddress.Address_Code.HasValue, udtPracticeModel.PracticeAddress.Address_Code, DBNull.Value)), _
                                   udtDB.MakeInParam("@professional_seq", ProfessionalModel.ProfessionalSeqDataType, ProfessionalModel.ProfessionalSeqDataSize, udtPracticeModel.ProfessionalSeq), _
                                    udtDB.MakeInParam("@scheme", SqlDbType.Char, 5, strSchemeCode)}

                    udtDB.RunProc("proc_PracticeEnrolmentBOTH_add", prams)
                Next
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddPracticeListToMOPracticeEnrolment(ByVal udtPracticeModelCollection As PracticeModelCollection, ByVal udtDB As Database) As Boolean
            'Dim i As Integer
            'Dim udtPracticeModel As PracticeModel

            Try
                For Each udtPracticeModel As PracticeModel In udtPracticeModelCollection.Values
                    'For i = 0 To udtPracticeModelCollection.Count - 1
                    'udtPracticeModel = New PracticeModel(udtPracticeModelCollection.Item(i + 1))

                    Dim prams() As SqlParameter = { _
                                   udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                                   udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                                   udtDB.MakeInParam("@practice_name_chi", PracticeModel.PracticeNameChiDataType, PracticeModel.PracticeNameChiDataSize, udtPracticeModel.PracticeNameChi), _
                                   udtDB.MakeInParam("@phone_daytime", PracticeModel.PhoneDaytimeDataType, PracticeModel.PhoneDaytimeDataSize, IIf(udtPracticeModel.PhoneDaytime.Equals(String.Empty), DBNull.Value, udtPracticeModel.PhoneDaytime)), _
                                   udtDB.MakeInParam("@practice_type_remark", PracticeModel.PracticeTypeRemarksDataType, PracticeModel.PracticeTypeRemarksDataSize, IIf(udtPracticeModel.PracticeTypeRemarks.Equals(String.Empty), DBNull.Value, udtPracticeModel.PracticeTypeRemarks))}

                    udtDB.RunProc("proc_MOPracticeEnrolment_add", prams)
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

        Public Function AddPracticeToStaging(ByVal udtPracticeModel As PracticeModel, ByVal udtDB As Database) As Boolean
            'Dim i As Integer

            Try
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                               udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtPracticeModel.SPID.Equals(String.Empty), DBNull.Value, udtPracticeModel.SPID)), _
                               udtDB.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                               udtDB.MakeInParam("@practice_type", PracticeModel.PracticeTypeDataType, PracticeModel.PracticeTypeDataSize, udtPracticeModel.PracticeType), _
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
                               udtDB.MakeInParam("@submission_method", PracticeModel.SubmissionMethodDataType, PracticeModel.SubmissionMethodDataSize, udtPracticeModel.SubmitMethod), _
                               udtDB.MakeInParam("@create_by", PracticeModel.CreateByDataType, PracticeModel.CreateByDataSize, udtPracticeModel.CreateBy), _
                               udtDB.MakeInParam("@update_by", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy)}

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
                Dim prams() As SqlParameter = { _
                               udtdb.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtPracticeModel.EnrolRefNo), _
                               udtdb.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                               udtdb.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                               udtdb.MakeInParam("@practice_type", PracticeModel.PracticeTypeDataType, PracticeModel.PracticeTypeDataSize, udtPracticeModel.PracticeType), _
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
                               udtdb.MakeInParam("@tsmp", PracticeModel.TSMPDataType, PracticeModel.TSMPDataSize, udtPracticeModel.TSMP)}

                udtdb.RunProc("proc_PracticeStaging_upd", prams)
                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        Public Function UpdatePracticePermanentAddress(ByVal udtPracticeModel As PracticeModel, ByVal udtDB As Database) As Boolean
            Dim blnRes As Boolean = False
            Try
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
                               udtDB.MakeInParam("@tsmp", PracticeModel.TSMPDataType, PracticeModel.TSMPDataSize, udtPracticeModel.TSMP)}

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
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq), _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtPracticeModel.SPID.Equals(String.Empty), DBNull.Value, udtPracticeModel.SPID)), _
                               udtDB.MakeInParam("@practice_name", PracticeModel.PracticeNameDataType, PracticeModel.PracticeNameDataSize, udtPracticeModel.PracticeName), _
                               udtDB.MakeInParam("@practice_type", PracticeModel.PracticeTypeDataType, PracticeModel.PracticeTypeDataSize, udtPracticeModel.PracticeType), _
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
                               udtDB.MakeInParam("@submission_method", PracticeModel.SubmissionMethodDataType, PracticeModel.SubmissionMethodDataSize, udtPracticeModel.SubmitMethod), _
                               udtDB.MakeInParam("@create_by", PracticeModel.CreateByDataType, PracticeModel.CreateByDataSize, udtPracticeModel.CreateBy), _
                               udtDB.MakeInParam("@update_by", PracticeModel.UpdateByDataType, PracticeModel.UpdateByDataSize, udtPracticeModel.UpdateBy)}

                udtDB.RunProc("proc_PracticePermanent_add", prams)

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
                udtDB.MakeInParam("@Delist_Status", PracticeModel.DelistStatusDataType, PracticeModel.DelistStatusDataSize, udtPracticeModel.DelistStatus), _
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
    End Class
End Namespace

