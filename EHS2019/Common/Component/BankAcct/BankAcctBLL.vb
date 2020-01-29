Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
'Imports Common.Component

Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.Component.BankAcct
Imports Common.Component.ERNProcessed

Namespace Component.BankAcct
    Public Class BankAcctBLL

        Public Const SESS_BANK As String = "Bank"

        Public Function GetBankAcctCollection() As BankAcctModelCollection
            Dim udtBankAcctModelCollection As BankAcctModelCollection
            udtBankAcctModelCollection = Nothing

            If Not IsNothing(HttpContext.Current.Session(SESS_BANK)) Then
                Try
                    udtBankAcctModelCollection = CType(HttpContext.Current.Session(SESS_BANK), BankAcctModelCollection)
                Catch ex As Exception
                    Throw New Exception("Invalid Session BankAcct")
                End Try
            Else
                Throw New Exception("Session Expired!")
            End If
            Return udtBankAcctModelCollection
        End Function

        Public Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_BANK) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ClearSession()
            HttpContext.Current.Session(SESS_BANK) = Nothing
        End Sub


        Public Sub SaveToSession(ByRef udtBankAcctModelCollection As BankAcctModelCollection)
            HttpContext.Current.Session(SESS_BANK) = udtBankAcctModelCollection
        End Sub

        Public Sub Clone(ByRef udtNewBankAccModel As BankAcctModel, ByRef udtOldBankAccModel As BankAcctModel)
            udtNewBankAccModel.SPID = udtOldBankAccModel.SPID
            udtNewBankAccModel.EnrolRefNo = udtOldBankAccModel.EnrolRefNo
            udtNewBankAccModel.DisplaySeq = udtOldBankAccModel.DisplaySeq
            udtNewBankAccModel.SpPracticeDisplaySeq = udtOldBankAccModel.SpPracticeDisplaySeq
            'udtNewBankAccModel.BrCode = udtOldBankAccModel.BrCode
            udtNewBankAccModel.BankName = udtOldBankAccModel.BankName
            udtNewBankAccModel.BranchName = udtOldBankAccModel.BranchName
            udtNewBankAccModel.BankAcctOwner = udtOldBankAccModel.BankAcctOwner
            udtNewBankAccModel.BankAcctNo = udtOldBankAccModel.BankAcctNo
            udtNewBankAccModel.RecordStatus = udtOldBankAccModel.RecordStatus
            udtNewBankAccModel.SubmitMethod = udtOldBankAccModel.SubmitMethod
            udtNewBankAccModel.Remark = udtOldBankAccModel.Remark
            udtNewBankAccModel.CreateDtm = udtOldBankAccModel.CreateDtm
            udtNewBankAccModel.CreateBy = udtOldBankAccModel.CreateBy
            udtNewBankAccModel.UpdateDtm = udtOldBankAccModel.UpdateDtm
            udtNewBankAccModel.UpdateBy = udtOldBankAccModel.UpdateBy
            udtNewBankAccModel.TSMP = udtOldBankAccModel.TSMP
            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            udtNewBankAccModel.IsFreeTextFormat = udtOldBankAccModel.IsFreeTextFormat
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
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

        'Public Function GetBankAcctListByERN(ByVal strERN As String, ByVal udtDB As Database) As BankAcctModelCollection
        '    Dim drBankAcctList As SqlDataReader = Nothing
        '    Dim udtBankAcctModelCollection As BankAcctModelCollection = New BankAcctModelCollection()
        '    Dim udtBankAcctModel As BankAcctModel

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
        '        udtDB.RunProc("proc_BankAccountEnrolment_get_byERN", prams, drBankAcctList)

        '        While drBankAcctList.Read()

        '            udtBankAcctModel = New BankAcctModel(String.Empty, _
        '                            CType(drBankAcctList.Item("Enrolment_Ref_No"), String).Trim, _
        '                            CType(drBankAcctList.Item("Display_Seq"), String).Trim, _
        '                            CType(drBankAcctList.Item("SP_Practice_Display_Seq"), String).Trim, _
        '                            CStr(IIf((drBankAcctList.Item("BR_Code") Is DBNull.Value), String.Empty, drBankAcctList.Item("BR_Code"))), _
        '                            CType(drBankAcctList.Item("Bank_Name"), String).Trim, _
        '                            CType(drBankAcctList.Item("Branch_Name"), String).Trim, _
        '                            CType(drBankAcctList.Item("Bank_Account_No"), String).Trim, _
        '                            CType(drBankAcctList.Item("Bank_Acc_Holder"), String).Trim, _
        '                            String.Empty, _
        '                            String.Empty, _
        '                            String.Empty, _
        '                            String.Empty, _
        '                            Nothing, _
        '                            String.Empty, _
        '                            Nothing, _
        '                            String.Empty, _
        '                            Nothing)
        '            udtBankAcctModelCollection.Add(udtBankAcctModel)
        '        End While
        '        drBankAcctList.Close()
        '        Return udtBankAcctModelCollection
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not drBankAcctList Is Nothing Then
        '            drBankAcctList.Close()
        '        End If
        '    End Try
        'End Function

        'Public Function GetBankAcctListByERN(ByVal strERN As String, ByVal table As String, ByVal udtDB As Database) As BankAcctModelCollection
        '    Dim procName As String = String.Empty

        '    Select Case table
        '        Case TableLocation.Enrolment
        '            procName = "proc_BankAccountEnrolment_get_byERN"

        '        Case TableLocation.Staging
        '            procName = ""
        '    End Select

        '    Dim drBankAcctList As SqlDataReader = Nothing
        '    Dim udtBankAcctModelCollection As BankAcctModelCollection = New BankAcctModelCollection()
        '    Dim udtBankAcctModel As BankAcctModel

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtDB.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 15, strERN)}
        '        udtDB.RunProc(procName, prams, drBankAcctList)

        '        While drBankAcctList.Read()
        '            'objBankAcctModel = New BankAcctModel(CType(drBankAcctList.Item("SP_ID"), String).Trim, _
        '            '                                    CType(drBankAcctList.Item("Enrolment_Ref_No"), String).Trim, _
        '            '                                    CType(drBankAcctList.Item("DisplaySeq"), String).Trim, _
        '            '                                    CType(drBankAcctList.Item("SpPracticeDisplaySeq"), String).Trim, _
        '            '                                    CType(drBankAcctList.Item("BrCode"), String).Trim, _
        '            '                                    CType(drBankAcctList.Item("BankName"), String).Trim, _
        '            '                                    CType(drBankAcctList.Item("BranchName"), String).Trim, _
        '            '                                    CType(drBankAcctList.Item("BankAcctOwner"), String).Trim, _
        '            '                                    CType(drBankAcctList.Item("BankAcctNo"), String).Trim, _
        '            '                                    IIf(table.Equals(TableLocation.Enrolment), String.Empty, CType(drBankAcctList.Item("RecordStatus"), String).Trim), _
        '            '                                    IIf(table.Equals(TableLocation.Enrolment), String.Empty, CType(drBankAcctList.Item("SubmitMethod"), String).Trim), _
        '            '                                    IIf(table.Equals(TableLocation.Enrolment), String.Empty, CType(drBankAcctList.Item("Remark"), String).Trim), _
        '            '                                    IIf(table.Equals(TableLocation.Enrolment), String.Empty, CType(drBankAcctList.Item("UnderModification"), String).Trim), _
        '            '                                    IIf(table.Equals(TableLocation.Enrolment), String.Empty, CType(drBankAcctList.Item("CreateDtm"), DateTime)), _
        '            '                                    IIf(table.Equals(TableLocation.Enrolment), String.Empty, CType(drBankAcctList.Item("CreateBy"), String).Trim), _
        '            '                                    IIf(table.Equals(TableLocation.Enrolment), String.Empty, CType(drBankAcctList.Item("UpdateDtm"), DateTime)), _
        '            '                                    IIf(table.Equals(TableLocation.Enrolment), String.Empty, CType(drBankAcctList.Item("UpdateBy"), String).Trim), _
        '            '                                    IIf(table.Equals(TableLocation.Enrolment), Nothing, CType(drBankAcctList.Item("TSMP"), Byte())))

        '            udtBankAcctModel = New BankAcctModel(String.Empty, _
        '                            CType(drBankAcctList.Item("Enrolment_Ref_No"), String).Trim, _
        '                            CType(drBankAcctList.Item("Display_Seq"), String).Trim, _
        '                            CType(drBankAcctList.Item("SP_Practice_Display_Seq"), String).Trim, _
        '                           CStr(IIf((drBankAcctList.Item("BR_Code") Is DBNull.Value), String.Empty, drBankAcctList.Item("BR_Code"))), _
        '                            CType(drBankAcctList.Item("Bank_Name"), String).Trim, _
        '                            CType(drBankAcctList.Item("Branch_Name"), String).Trim, _
        '                            CType(drBankAcctList.Item("Bank_Account_No"), String).Trim, _
        '                            CType(drBankAcctList.Item("Bank_Acc_Holder"), String).Trim, _
        '                            String.Empty, _
        '                            String.Empty, _
        '                            String.Empty, _
        '                            String.Empty, _
        '                            Nothing, _
        '                            String.Empty, _
        '                            Nothing, _
        '                            String.Empty, _
        '                            Nothing)
        '            udtBankAcctModelCollection.Add(udtBankAcctModel)
        '        End While
        '        drBankAcctList.Close()
        '        Return udtBankAcctModelCollection
        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        'End Function

        Public Function AddBankAcctListToEnrolment(ByVal udtBankAcctModelCollection As BankAcctModelCollection, ByRef udtDB As Database) As Boolean

            Try

                For Each udtBankAcctModel As BankAcctModel In udtBankAcctModelCollection.Values

                    AddBankAcctToEnrolment(udtBankAcctModel, udtDB)
                Next
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddBankAcctToEnrolment(ByVal udtBankAcctModel As BankAcctModel, ByRef udtDB As Database) As Boolean

            Try

                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtBankAcctModel.EnrolRefNo), _
                               udtDB.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, udtBankAcctModel.DisplaySeq), _
                               udtDB.MakeInParam("@sp_practice_display_seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, udtBankAcctModel.SpPracticeDisplaySeq), _
                               udtDB.MakeInParam("@bank_name", BankAcctModel.BankNameDataType, BankAcctModel.BankNameDataSize, udtBankAcctModel.BankName), _
                               udtDB.MakeInParam("@branch_name", BankAcctModel.BranchNameDataType, BankAcctModel.BranchNameDataSize, udtBankAcctModel.BranchName), _
                               udtDB.MakeInParam("@bank_account_no", BankAcctModel.BankAcctNoDataType, BankAcctModel.BankAcctNoDataSize, udtBankAcctModel.BankAcctNo), _
                               udtDB.MakeInParam("@bank_acc_holder", BankAcctModel.BankAcctOwnerDataType, BankAcctModel.BankAcctOwnerDataSize, udtBankAcctModel.BankAcctOwner)}

                udtDB.RunProc("proc_BankAccountEnrolment_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        'Public Function AddBankAcctListToEnrolmentBOTH(ByVal udtBankAcctModelCollection As BankAcctModelCollection, ByVal udtDB As Database, ByVal strSchemeCode As String) As Boolean
        '    'Dim i As Integer
        '    'Dim udtBankAcctModel As BankAcctModel

        '    Try
        '        'For i = 0 To udtBankAcctModelCollection.Count - 1
        '        For Each udtBankAcctModel As BankAcctModel In udtBankAcctModelCollection.Values

        '            'udtBankAcctModel = New BankAcctModel(udtBankAcctModelCollection.Item(i + 1))

        '            Dim prams() As SqlParameter = { _
        '                           udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtBankAcctModel.EnrolRefNo), _
        '                           udtDB.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, udtBankAcctModel.DisplaySeq), _
        '                           udtDB.MakeInParam("@sp_practice_display_seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, udtBankAcctModel.SpPracticeDisplaySeq), _
        '                           udtDB.MakeInParam("@br_code", BankAcctModel.BrCodeDataType, BankAcctModel.BrCodeDataSize, IIf(udtBankAcctModel.BrCode.Equals(String.Empty), DBNull.Value, udtBankAcctModel.BrCode)), _
        '                           udtDB.MakeInParam("@bank_name", BankAcctModel.BankNameDataType, BankAcctModel.BankNameDataSize, udtBankAcctModel.BankName), _
        '                           udtDB.MakeInParam("@branch_name", BankAcctModel.BranchNameDataType, BankAcctModel.BranchNameDataSize, udtBankAcctModel.BranchName), _
        '                           udtDB.MakeInParam("@bank_account_no", BankAcctModel.BankAcctNoDataType, BankAcctModel.BankAcctNoDataSize, udtBankAcctModel.BankAcctNo), _
        '                           udtDB.MakeInParam("@bank_acc_holder", BankAcctModel.BankAcctOwnerDataType, BankAcctModel.BankAcctOwnerDataSize, udtBankAcctModel.BankAcctOwner), _
        '                           udtDB.MakeInParam("@scheme", SqlDbType.Char, 5, strSchemeCode)}

        '            udtDB.RunProc("proc_BankAccountEnrolmentBOTH_add", prams)
        '        Next
        '        Return True
        '    Catch eSQL As SqlException
        '        Throw eSQL
        '    Catch ex As Exception
        '        Throw ex
        '        Return False
        '    End Try
        'End Function

        Public Function AddBankAcctListToStaging(ByVal udtBankAcctModelCollection As BankAcctModelCollection, ByVal udtDB As Database) As Boolean
            'Dim i As Integer
            Dim udtBankAcctModel As BankAcctModel

            Try
                For Each udtBankAcctModel In udtBankAcctModelCollection.Values
                    AddBankAcctToStaging(udtBankAcctModel, udtDB)
                Next
                'For i = 0 To udtBankAcctModelCollection.Count - 1
                '    udtBankAcctModel = New BankAcctModel(udtBankAcctModelCollection.Item(i + 1))

                '    'Dim prams() As SqlParameter = { _
                '    '               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtBankAcctModel.EnrolRefNo), _
                '    '               udtDB.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, udtBankAcctModel.DisplaySeq), _
                '    '               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtBankAcctModel.SPID.Equals(String.Empty), DBNull.Value, udtBankAcctModel.SPID)), _
                '    '               udtDB.MakeInParam("@sp_practice_display_seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, udtBankAcctModel.SpPracticeDisplaySeq), _
                '    '               udtDB.MakeInParam("@br_code", BankAcctModel.BrCodeDataType, BankAcctModel.BrCodeDataSize, IIf(udtBankAcctModel.BrCode.Equals(String.Empty), DBNull.Value, udtBankAcctModel.BrCode)), _
                '    '               udtDB.MakeInParam("@bank_name", BankAcctModel.BankNameDataType, BankAcctModel.BankNameDataSize, udtBankAcctModel.BankName), _
                '    '               udtDB.MakeInParam("@branch_name", BankAcctModel.BranchNameDataType, BankAcctModel.BranchNameDataSize, udtBankAcctModel.BankName), _
                '    '               udtDB.MakeInParam("@bank_account_no", BankAcctModel.BankAcctNoDataType, BankAcctModel.BankAcctNoDataSize, udtBankAcctModel.BankAcctNo), _
                '    '               udtDB.MakeInParam("@bank_acc_holder", BankAcctModel.BankAcctOwnerDataType, BankAcctModel.BankAcctOwnerDataSize, udtBankAcctModel.BankAcctOwner), _
                '    '               udtDB.MakeInParam("@record_status", BankAcctModel.RecordStatusDataType, BankAcctModel.RecordStatusDataSize, udtBankAcctModel.RecordStatus), _
                '    '               udtDB.MakeInParam("@remark", BankAcctModel.RemarkDataType, BankAcctModel.RemarkDataSize, IIf(udtBankAcctModel.Remark.Equals(String.Empty), DBNull.Value, udtBankAcctModel.Remark)), _
                '    '               udtDB.MakeInParam("@submission_method", BankAcctModel.SubmissionMethodDataType, BankAcctModel.SubmissionMethodDataSize, udtBankAcctModel.SubmitMethod), _
                '    '               udtDB.MakeInParam("@create_by", BankAcctModel.CreateByDataType, BankAcctModel.CreateByDataSize, udtBankAcctModel.CreateBy), _
                '    '               udtDB.MakeInParam("@update_by", BankAcctModel.UpdateByDataType, BankAcctModel.UpdateByDataSize, udtBankAcctModel.UpdateBy)}

                '    'udtDB.RunProc("proc_BankAccountStaging_add", prams)
                '    AddBankAcctToStaging(udtBankAcctModel, udtDB)
                'Next
                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddBankAcctToStaging(ByVal udtBankAcctModel As BankAcctModel, ByVal udtDB As Database) As Boolean
            Try
                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtBankAcctModel.EnrolRefNo), _
                               udtDB.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, udtBankAcctModel.DisplaySeq.Value), _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtBankAcctModel.SPID.Equals(String.Empty), DBNull.Value, udtBankAcctModel.SPID)), _
                               udtDB.MakeInParam("@sp_practice_display_seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, udtBankAcctModel.SpPracticeDisplaySeq.Value), _
                               udtDB.MakeInParam("@bank_name", BankAcctModel.BankNameDataType, BankAcctModel.BankNameDataSize, udtBankAcctModel.BankName), _
                               udtDB.MakeInParam("@branch_name", BankAcctModel.BranchNameDataType, BankAcctModel.BranchNameDataSize, udtBankAcctModel.BranchName), _
                               udtDB.MakeInParam("@bank_account_no", BankAcctModel.BankAcctNoDataType, BankAcctModel.BankAcctNoDataSize, udtBankAcctModel.BankAcctNo), _
                               udtDB.MakeInParam("@bank_acc_holder", BankAcctModel.BankAcctOwnerDataType, BankAcctModel.BankAcctOwnerDataSize, udtBankAcctModel.BankAcctOwner), _
                               udtDB.MakeInParam("@record_status", BankAcctModel.RecordStatusDataType, BankAcctModel.RecordStatusDataSize, udtBankAcctModel.RecordStatus), _
                               udtDB.MakeInParam("@remark", BankAcctModel.RemarkDataType, BankAcctModel.RemarkDataSize, IIf(udtBankAcctModel.Remark.Equals(String.Empty), DBNull.Value, udtBankAcctModel.Remark)), _
                               udtDB.MakeInParam("@submission_method", BankAcctModel.SubmissionMethodDataType, BankAcctModel.SubmissionMethodDataSize, udtBankAcctModel.SubmitMethod), _
                               udtDB.MakeInParam("@create_by", BankAcctModel.CreateByDataType, BankAcctModel.CreateByDataSize, udtBankAcctModel.CreateBy), _
                               udtDB.MakeInParam("@update_by", BankAcctModel.UpdateByDataType, BankAcctModel.UpdateByDataSize, udtBankAcctModel.UpdateBy), _
                               udtDB.MakeInParam("@isfreetextformat", BankAcctModel.IsFreeTextFormatDataType, BankAcctModel.IsFreeTextFormatDataSize, udtBankAcctModel.IsFreeTextFormat)}
                udtDB.RunProc("proc_BankAccountStaging_add", prams)
                'CRE13-019-02 Extend HCVS to China [End][Winnie]

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function UpdateBankAcctStaging(ByVal udtBankAcctModel As BankAcctModel, ByVal udtdb As Database) As Boolean
            Try
                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                Dim prams() As SqlParameter = { _
                                                udtdb.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtBankAcctModel.EnrolRefNo), _
                                                udtdb.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, udtBankAcctModel.DisplaySeq), _
                                                udtdb.MakeInParam("@SP_Practice_Display_Seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, udtBankAcctModel.SpPracticeDisplaySeq), _
                                                udtdb.MakeInParam("@bank_name", BankAcctModel.BankNameDataType, BankAcctModel.BankNameDataSize, udtBankAcctModel.BankName), _
                                                udtdb.MakeInParam("@branch_name", BankAcctModel.BranchNameDataType, BankAcctModel.BranchNameDataSize, udtBankAcctModel.BranchName), _
                                                udtdb.MakeInParam("@bank_account_no", BankAcctModel.BankAcctNoDataType, BankAcctModel.BankAcctNoDataSize, udtBankAcctModel.BankAcctNo), _
                                                udtdb.MakeInParam("@bank_acc_holder", BankAcctModel.BankAcctOwnerDataType, BankAcctModel.BankAcctOwnerDataSize, udtBankAcctModel.BankAcctOwner), _
                                                udtdb.MakeInParam("@record_status", BankAcctModel.RecordStatusDataType, BankAcctModel.RecordStatusDataSize, udtBankAcctModel.RecordStatus), _
                                                udtdb.MakeInParam("@remark", BankAcctModel.RemarkDataType, BankAcctModel.RemarkDataSize, IIf(udtBankAcctModel.Remark.Equals(String.Empty), DBNull.Value, udtBankAcctModel.Remark)), _
                                                udtdb.MakeInParam("@update_by", BankAcctModel.UpdateByDataType, BankAcctModel.UpdateByDataSize, udtBankAcctModel.UpdateBy), _
                                                udtdb.MakeInParam("@tsmp", BankAcctModel.TSMPDataType, BankAcctModel.TSMPDataSize, udtBankAcctModel.TSMP), _
                                                udtdb.MakeInParam("@isfreetextformat", BankAcctModel.IsFreeTextFormatDataType, BankAcctModel.IsFreeTextFormatDataSize, udtBankAcctModel.IsFreeTextFormat)}
                'CRE13-019-02 Extend HCVS to China [End][Winnie]

                udtdb.RunProc("proc_BankAccountStaging_upd", prams)
                Return True
            Catch ex As Exception
                Throw ex
                Return False
            End Try

        End Function

        Public Function GetBankAcctListFromStagingByERN(ByVal strERN As String, ByVal udtDB As Database) As BankAcctModelCollection
            Dim udtBankAcctModelCollection As BankAcctModelCollection = New BankAcctModelCollection
            Dim udtBankAcctModel As BankAcctModel

            Dim intDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim dtmCreateDtm As Nullable(Of DateTime)
            Dim dtmUpdateDtm As Nullable(Of DateTime)

            Dim dtRaw As New DataTable()

            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                udtDB.RunProc("proc_BankAccountStaging_get_byERN", prams, dtRaw)


                For i As Integer = 0 To dtRaw.Rows.Count - 1

                    Dim drRaw As DataRow = dtRaw.Rows(i)


                    If IsDBNull(drRaw.Item("Display_Seq")) Then
                        intDisplaySeq = Nothing
                    Else
                        intDisplaySeq = CInt(drRaw.Item("Display_Seq"))
                    End If

                    If IsDBNull(drRaw.Item("SP_Practice_Display_Seq")) Then
                        intPracticeDisplaySeq = Nothing
                    Else
                        intPracticeDisplaySeq = CInt(drRaw.Item("SP_Practice_Display_Seq"))
                    End If

                    If IsDBNull(drRaw.Item("Create_Dtm")) Then
                        dtmCreateDtm = Nothing
                    Else
                        dtmCreateDtm = Convert.ToDateTime(drRaw.Item("Create_Dtm"))
                    End If

                    If IsDBNull(drRaw.Item("Update_Dtm")) Then
                        dtmUpdateDtm = Nothing
                    Else
                        dtmUpdateDtm = Convert.ToDateTime(drRaw.Item("Update_Dtm"))
                    End If

                    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                    udtBankAcctModel = New BankAcctModel(CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Enrolment_Ref_No") Is DBNull.Value, String.Empty, drRaw.Item("Enrolment_Ref_No"))).Trim, _
                                                         intDisplaySeq, _
                                                         intPracticeDisplaySeq, _
                                                         CStr(IIf(drRaw.Item("Bank_Name") Is DBNull.Value, String.Empty, drRaw.Item("Bank_Name"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Branch_Name") Is DBNull.Value, String.Empty, drRaw.Item("Branch_Name"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Bank_Acc_Holder") Is DBNull.Value, String.Empty, drRaw.Item("Bank_Acc_Holder"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Bank_Account_No") Is DBNull.Value, String.Empty, drRaw.Item("Bank_Account_No"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Record_Status") Is DBNull.Value, String.Empty, drRaw.Item("Record_Status"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Submission_Method") Is DBNull.Value, String.Empty, drRaw.Item("Submission_Method"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Remark") Is DBNull.Value, String.Empty, drRaw.Item("Remark"))).Trim, _
                                                         dtmCreateDtm, _
                                                         CStr(IIf(drRaw.Item("Create_By") Is DBNull.Value, String.Empty, drRaw.Item("Create_By"))).Trim, _
                                                         dtmUpdateDtm, _
                                                         CStr(IIf(drRaw.Item("Update_By") Is DBNull.Value, String.Empty, drRaw.Item("Update_By"))).Trim, _
                                                         IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, CType(drRaw.Item("TSMP"), Byte())), _
                                                         CStr(IIf(drRaw.Item("IsFreeTextFormat") Is DBNull.Value, String.Empty, drRaw.Item("IsFreeTextFormat"))).Trim)
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]

                    udtBankAcctModelCollection.Add(udtBankAcctModel)
                Next
                Return udtBankAcctModelCollection
            Catch ex As Exception
                Throw ex
            Finally

            End Try
        End Function

        'Public Function GetBankAcctListFromStagingByERN_(ByVal strERN As String, ByVal udtDB As Database) As BankAcctModelCollection
        '    Dim drBankAcctList As SqlDataReader = Nothing
        '    Dim udtBankAcctModelCollection As BankAcctModelCollection = New BankAcctModelCollection
        '    Dim udtBankAcctModel As BankAcctModel

        '    Dim intDisplaySeq As Nullable(Of Integer)
        '    Dim intPracticeDisplaySeq As Nullable(Of Integer)
        '    Dim dtmCreateDtm As Nullable(Of DateTime)
        '    Dim dtmUpdateDtm As Nullable(Of DateTime)

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
        '        udtDB.RunProc("proc_BankAccountStaging_get_byERN", prams, drBankAcctList)

        '        While drBankAcctList.Read()
        '            If IsDBNull(drBankAcctList.Item("Display_Seq")) Then
        '                intDisplaySeq = Nothing
        '            Else
        '                intDisplaySeq = CInt(drBankAcctList.Item("Display_Seq"))
        '            End If

        '            If IsDBNull(drBankAcctList.Item("SP_Practice_Display_Seq")) Then
        '                intPracticeDisplaySeq = Nothing
        '            Else
        '                intPracticeDisplaySeq = CInt(drBankAcctList.Item("SP_Practice_Display_Seq"))
        '            End If

        '            If IsDBNull(drBankAcctList.Item("Create_Dtm")) Then
        '                dtmCreateDtm = Nothing
        '            Else
        '                dtmCreateDtm = Convert.ToDateTime(drBankAcctList.Item("Create_Dtm"))
        '            End If

        '            If IsDBNull(drBankAcctList.Item("Update_Dtm")) Then
        '                dtmUpdateDtm = Nothing
        '            Else
        '                dtmUpdateDtm = Convert.ToDateTime(drBankAcctList.Item("Update_Dtm"))
        '            End If

        '            udtBankAcctModel = New BankAcctModel(CStr(IIf(drBankAcctList.Item("SP_ID") Is DBNull.Value, String.Empty, drBankAcctList.Item("SP_ID"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Enrolment_Ref_No") Is DBNull.Value, String.Empty, drBankAcctList.Item("Enrolment_Ref_No"))).Trim, _
        '                                                 intDisplaySeq, _
        '                                                 intPracticeDisplaySeq, _
        '                                                 CStr(IIf(drBankAcctList.Item("Bank_Name") Is DBNull.Value, String.Empty, drBankAcctList.Item("Bank_Name"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Branch_Name") Is DBNull.Value, String.Empty, drBankAcctList.Item("Branch_Name"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Bank_Acc_Holder") Is DBNull.Value, String.Empty, drBankAcctList.Item("Bank_Acc_Holder"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Bank_Account_No") Is DBNull.Value, String.Empty, drBankAcctList.Item("Bank_Account_No"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Record_Status") Is DBNull.Value, String.Empty, drBankAcctList.Item("Record_Status"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Submission_Method") Is DBNull.Value, String.Empty, drBankAcctList.Item("Submission_Method"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Remark") Is DBNull.Value, String.Empty, drBankAcctList.Item("Remark"))).Trim, _
        '                                                 dtmCreateDtm, _
        '                                                 CStr(IIf(drBankAcctList.Item("Create_By") Is DBNull.Value, String.Empty, drBankAcctList.Item("Create_By"))).Trim, _
        '                                                 dtmUpdateDtm, _
        '                                                 CStr(IIf(drBankAcctList.Item("Update_By") Is DBNull.Value, String.Empty, drBankAcctList.Item("Update_By"))).Trim, _
        '                                                 IIf(drBankAcctList.Item("TSMP") Is DBNull.Value, Nothing, CType(drBankAcctList.Item("TSMP"), Byte())))
        '            udtBankAcctModelCollection.Add(udtBankAcctModel)
        '        End While
        '        drBankAcctList.Close()
        '        Return udtBankAcctModelCollection
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not drBankAcctList Is Nothing Then
        '            drBankAcctList.Close()
        '        End If
        '    End Try
        'End Function


        ' CRE12-001 eHS and PCD integration [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        Public Function GetBankAcctListFromEnrolmentByERN(ByVal strERN As String, ByVal udtDB As Database) As BankAcctModelCollection
            Return GetBankAcctListFromCopyByERN(strERN, EnumEnrolCopy.Enrolment, udtDB)
        End Function
        ' CRE12-001 eHS and PCD integration [End][Koala]

        Public Function GetBankAcctListFromCopyByERN(ByVal strERN As String, ByVal enumEnrolCopy As EnumEnrolCopy, ByVal udtDB As Database) As BankAcctModelCollection
            ' Dim drBankAcctList As SqlDataReader = Nothing
            Dim udtBankAcctModelCollection As BankAcctModelCollection = New BankAcctModelCollection
            Dim udtBankAcctModel As BankAcctModel

            Try
                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                Dim dtBank As New DataTable
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
                Select Case enumEnrolCopy
                    Case enumEnrolCopy.Enrolment
                        udtDB.RunProc("proc_BankAccountEnrolment_get_byERN", prams, dtBank)
                    Case enumEnrolCopy.Original
                        udtDB.RunProc("proc_BankAccountOriginal_get_byERN", prams, dtBank)
                End Select
                ' CRE12-001 eHS and PCD integration [End][Koala]

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                For Each drBank As DataRow In dtBank.Rows
                    udtBankAcctModel = New BankAcctModel(String.Empty, _
                                                                CStr(drBank.Item("Enrolment_Ref_No")).Trim, _
                                                                CInt(drBank.Item("Display_Seq")), _
                                                                CInt(drBank.Item("SP_Practice_Display_Seq")), _
                                                                CStr(drBank.Item("Bank_Name")).Trim, _
                                                                CStr(drBank.Item("Branch_Name")).Trim, _
                                                                CStr(drBank.Item("Bank_Acc_Holder")).Trim, _
                                                                CStr(drBank.Item("Bank_Account_No")).Trim, _
                                                                String.Empty, _
                                                                SubmitChannel.Electronic, _
                                                                String.Empty, _
                                                                Nothing, _
                                                                String.Empty, _
                                                                Nothing, _
                                                                String.Empty, _
                                                                Nothing, _
                                                                YesNo.No)
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]

                    udtBankAcctModelCollection.Add(udtBankAcctModel)

                Next

                Return udtBankAcctModelCollection

            Catch ex As Exception
                Throw ex

            End Try

        End Function

        'Public Function GetBankAcctListFromPermanentBySPID_(ByVal strSPID As String, ByVal udtDB As Database) As BankAcctModelCollection
        '    Dim drBankAcctList As SqlDataReader = Nothing
        '    Dim udtBankAcctModelCollection As BankAcctModelCollection = New BankAcctModelCollection
        '    Dim udtBankAcctModel As BankAcctModel

        '    Dim intDisplaySeq As Nullable(Of Integer)
        '    Dim intPracticeDisplaySeq As Nullable(Of Integer)
        '    Dim dtmCreateDtm As Nullable(Of DateTime)
        '    Dim dtmUpdateDtm As Nullable(Of DateTime)

        '    Try
        '        Dim prams() As SqlParameter = { _
        '        udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
        '        udtDB.RunProc("proc_BankAccount_get_bySPID", prams, drBankAcctList)

        '        While drBankAcctList.Read()
        '            If IsDBNull(drBankAcctList.Item("Display_Seq")) Then
        '                intDisplaySeq = Nothing
        '            Else
        '                intDisplaySeq = CInt(drBankAcctList.Item("Display_Seq"))
        '            End If

        '            If IsDBNull(drBankAcctList.Item("SP_Practice_Display_Seq")) Then
        '                intPracticeDisplaySeq = Nothing
        '            Else
        '                intPracticeDisplaySeq = CInt(drBankAcctList.Item("SP_Practice_Display_Seq"))
        '            End If

        '            If IsDBNull(drBankAcctList.Item("Create_Dtm")) Then
        '                dtmCreateDtm = Nothing
        '            Else
        '                dtmCreateDtm = Convert.ToDateTime(drBankAcctList.Item("Create_Dtm"))
        '            End If

        '            If IsDBNull(drBankAcctList.Item("Update_Dtm")) Then
        '                dtmUpdateDtm = Nothing
        '            Else
        '                dtmUpdateDtm = Convert.ToDateTime(drBankAcctList.Item("Update_Dtm"))
        '            End If

        '            udtBankAcctModel = New BankAcctModel(CStr(IIf(drBankAcctList.Item("SP_ID") Is DBNull.Value, String.Empty, drBankAcctList.Item("SP_ID"))).Trim, _
        '                                                 String.Empty, _
        '                                                 intDisplaySeq, _
        '                                                 intPracticeDisplaySeq, _
        '                                                 CStr(IIf(drBankAcctList.Item("Bank_Name") Is DBNull.Value, String.Empty, drBankAcctList.Item("Bank_Name"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Branch_Name") Is DBNull.Value, String.Empty, drBankAcctList.Item("Branch_Name"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Bank_Acc_Holder") Is DBNull.Value, String.Empty, drBankAcctList.Item("Bank_Acc_Holder"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Bank_Account_No") Is DBNull.Value, String.Empty, drBankAcctList.Item("Bank_Account_No"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Record_Status") Is DBNull.Value, String.Empty, drBankAcctList.Item("Record_Status"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Submission_Method") Is DBNull.Value, String.Empty, drBankAcctList.Item("Submission_Method"))).Trim, _
        '                                                 CStr(IIf(drBankAcctList.Item("Remark") Is DBNull.Value, String.Empty, drBankAcctList.Item("Remark"))).Trim, _
        '                                                 dtmCreateDtm, _
        '                                                 CStr(IIf(drBankAcctList.Item("Create_By") Is DBNull.Value, String.Empty, drBankAcctList.Item("Create_By"))).Trim, _
        '                                                 dtmUpdateDtm, _
        '                                                 CStr(IIf(drBankAcctList.Item("Update_By") Is DBNull.Value, String.Empty, drBankAcctList.Item("Update_By"))).Trim, _
        '                                                 IIf(drBankAcctList.Item("TSMP") Is DBNull.Value, Nothing, CType(drBankAcctList.Item("TSMP"), Byte())))
        '            udtBankAcctModelCollection.Add(udtBankAcctModel)
        '        End While
        '        drBankAcctList.Close()
        '        Return udtBankAcctModelCollection
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        If Not drBankAcctList Is Nothing Then
        '            drBankAcctList.Close()
        '        End If
        '    End Try
        'End Function

        Public Function DeleteBankAccountStaging(ByVal udtBankAcctModel As BankAcctModel, ByVal udtDB As Database) As Boolean
            Try

                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtBankAcctModel.EnrolRefNo), _
                               udtDB.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, udtBankAcctModel.DisplaySeq), _
                               udtDB.MakeInParam("@sp_practice_display_seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, udtBankAcctModel.SpPracticeDisplaySeq), _
                               udtDB.MakeInParam("@record_status", BankAcctModel.RecordStatusDataType, BankAcctModel.RecordStatusDataSize, udtBankAcctModel.RecordStatus), _
                               udtDB.MakeInParam("@update_by", BankAcctModel.UpdateByDataType, BankAcctModel.UpdateByDataSize, udtBankAcctModel.UpdateBy), _
                               udtDB.MakeInParam("@tsmp", BankAcctModel.TSMPDataType, BankAcctModel.TSMPDataSize, udtBankAcctModel.TSMP)}

                udtDB.RunProc("proc_BankAccountStaging_del", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Sub DeleteBankAccountStagingByKey(ByRef udtDB As Database, ByVal strERN As String, ByVal intDispSeq As Integer, ByVal intSPPractDispSeq As Integer, ByVal TSMP As Byte(), ByVal blnCheckTSMP As Boolean)
            Try
                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@Enrolment_Ref_No", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                    udtDB.MakeInParam("@Display_Seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, intDispSeq), _
                    udtDB.MakeInParam("@sp_practice_display_seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, intSPPractDispSeq), _
                    udtDB.MakeInParam("@tsmp", BankAcctModel.TSMPDataType, BankAcctModel.TSMPDataSize, TSMP), _
                    udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP)}

                udtDB.RunProc("proc_BankAccountStaging_del_ByKey", params)

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Function AddBankAcctListToPermanent(ByVal udtBankAcctModelCollection As BankAcctModelCollection, ByVal udtDB As Database) As Boolean
            Dim udtBankAcctModel As BankAcctModel

            Try
                For Each udtBankAcctModel In udtBankAcctModelCollection.Values
                    AddBankAcctToPermanent(udtBankAcctModel, udtDB)
                Next

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function AddBankAcctToPermanent(ByVal udtBankAcctModel As BankAcctModel, ByVal udtDB As Database) As Boolean
            Try

                'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                Dim prams() As SqlParameter = { _
                               udtDB.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, udtBankAcctModel.DisplaySeq), _
                               udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, IIf(udtBankAcctModel.SPID.Equals(String.Empty), DBNull.Value, udtBankAcctModel.SPID)), _
                               udtDB.MakeInParam("@sp_practice_display_seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, udtBankAcctModel.SpPracticeDisplaySeq), _
                               udtDB.MakeInParam("@bank_name", BankAcctModel.BankNameDataType, BankAcctModel.BankNameDataSize, udtBankAcctModel.BankName), _
                               udtDB.MakeInParam("@branch_name", BankAcctModel.BranchNameDataType, BankAcctModel.BranchNameDataSize, udtBankAcctModel.BranchName), _
                               udtDB.MakeInParam("@bank_account_no", BankAcctModel.BankAcctNoDataType, BankAcctModel.BankAcctNoDataSize, udtBankAcctModel.BankAcctNo), _
                               udtDB.MakeInParam("@bank_acc_holder", BankAcctModel.BankAcctOwnerDataType, BankAcctModel.BankAcctOwnerDataSize, udtBankAcctModel.BankAcctOwner), _
                               udtDB.MakeInParam("@record_status", BankAcctModel.RecordStatusDataType, BankAcctModel.RecordStatusDataSize, udtBankAcctModel.RecordStatus), _
                               udtDB.MakeInParam("@remark", BankAcctModel.RemarkDataType, BankAcctModel.RemarkDataSize, IIf(udtBankAcctModel.Remark.Equals(String.Empty), DBNull.Value, udtBankAcctModel.Remark)), _
                               udtDB.MakeInParam("@submission_method", BankAcctModel.SubmissionMethodDataType, BankAcctModel.SubmissionMethodDataSize, udtBankAcctModel.SubmitMethod), _
                               udtDB.MakeInParam("@create_by", BankAcctModel.CreateByDataType, BankAcctModel.CreateByDataSize, udtBankAcctModel.CreateBy), _
                               udtDB.MakeInParam("@update_by", BankAcctModel.UpdateByDataType, BankAcctModel.UpdateByDataSize, udtBankAcctModel.UpdateBy), _
                               udtDB.MakeInParam("@isfreetextformat", BankAcctModel.IsFreeTextFormatDataType, BankAcctModel.IsFreeTextFormatDataSize, udtBankAcctModel.IsFreeTextFormat)}
                'CRE13-019-02 Extend HCVS to China [End][Winnie]

                udtDB.RunProc("proc_BankAccountPermanent_add", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function GetBankAcctListFromPermanentBySPID(ByVal strSPID As String, ByVal udtDB As Database) As BankAcctModelCollection
            'Dim drBankAcctList As SqlDataReader = Nothing
            Dim udtBankAcctModelCollection As BankAcctModelCollection = New BankAcctModelCollection
            Dim udtBankAcctModel As BankAcctModel

            Dim intDisplaySeq As Nullable(Of Integer)
            Dim intPracticeDisplaySeq As Nullable(Of Integer)
            Dim dtmCreateDtm As Nullable(Of DateTime)
            Dim dtmUpdateDtm As Nullable(Of DateTime)
            'Dim dtmEffectiveDtm As Nullable(Of DateTime)
            'Dim dtmDelistDtm As Nullable(Of DateTime)

            Dim dtRaw As New DataTable

            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@sp_id", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}
                udtDB.RunProc("proc_BankAccount_get_bySPID", prams, dtRaw)

                For i As Integer = 0 To dtRaw.Rows.Count - 1

                    Dim drRaw As DataRow = dtRaw.Rows(i)

                    If IsDBNull(drRaw.Item("Display_Seq")) Then
                        intDisplaySeq = Nothing
                    Else
                        intDisplaySeq = CInt(drRaw.Item("Display_Seq"))
                    End If

                    If IsDBNull(drRaw.Item("SP_Practice_Display_Seq")) Then
                        intPracticeDisplaySeq = Nothing
                    Else
                        intPracticeDisplaySeq = CInt(drRaw.Item("SP_Practice_Display_Seq"))
                    End If

                    If IsDBNull(drRaw.Item("Create_Dtm")) Then
                        dtmCreateDtm = Nothing
                    Else
                        dtmCreateDtm = Convert.ToDateTime(drRaw.Item("Create_Dtm"))
                    End If

                    If IsDBNull(drRaw.Item("Update_Dtm")) Then
                        dtmUpdateDtm = Nothing
                    Else
                        dtmUpdateDtm = Convert.ToDateTime(drRaw.Item("Update_Dtm"))
                    End If

                    'If IsDBNull(drRaw.Item("Effective_Dtm")) Then
                    '    dtmEffectiveDtm = Nothing
                    'Else
                    '    dtmEffectiveDtm = Convert.ToDateTime(drRaw.Item("Effective_Dtm"))
                    'End If

                    'If IsDBNull(drRaw.Item("Delist_Dtm")) Then
                    '    dtmDelistDtm = Nothing
                    'Else
                    '    dtmDelistDtm = Convert.ToDateTime(drRaw.Item("Delist_Dtm"))
                    'End If

                    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                    udtBankAcctModel = New BankAcctModel(CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                         String.Empty, _
                                                         intDisplaySeq, _
                                                         intPracticeDisplaySeq, _
                                                         CStr(IIf(drRaw.Item("Bank_Name") Is DBNull.Value, String.Empty, drRaw.Item("Bank_Name"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Branch_Name") Is DBNull.Value, String.Empty, drRaw.Item("Branch_Name"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Bank_Acc_Holder") Is DBNull.Value, String.Empty, drRaw.Item("Bank_Acc_Holder"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Bank_Account_No") Is DBNull.Value, String.Empty, drRaw.Item("Bank_Account_No"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Record_Status") Is DBNull.Value, String.Empty, drRaw.Item("Record_Status"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Submission_Method") Is DBNull.Value, String.Empty, drRaw.Item("Submission_Method"))).Trim, _
                                                         CStr(IIf(drRaw.Item("Remark") Is DBNull.Value, String.Empty, drRaw.Item("Remark"))).Trim, _
                                                         dtmCreateDtm, _
                                                         CStr(IIf(drRaw.Item("Create_By") Is DBNull.Value, String.Empty, drRaw.Item("Create_By"))).Trim, _
                                                         dtmUpdateDtm, _
                                                         CStr(IIf(drRaw.Item("Update_By") Is DBNull.Value, String.Empty, drRaw.Item("Update_By"))).Trim, _
                                                         IIf(drRaw.Item("TSMP") Is DBNull.Value, Nothing, CType(drRaw.Item("TSMP"), Byte())), _
                                                         CStr(IIf(drRaw.Item("IsFreeTextFormat") Is DBNull.Value, String.Empty, drRaw.Item("IsFreeTextFormat"))).Trim)
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]

                    udtBankAcctModelCollection.Add(udtBankAcctModel)
                Next
                Return udtBankAcctModelCollection
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function UpdateRecordStatus(ByVal udtBankAcctModel As BankAcctModel, ByRef udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtBankAcctModel.SPID), _
                udtDB.MakeInParam("@SP_Practice_Display_Seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, udtBankAcctModel.SpPracticeDisplaySeq), _
                udtDB.MakeInParam("@Update_By", BankAcctModel.UpdateByDataType, BankAcctModel.UpdateByDataSize, udtBankAcctModel.UpdateBy), _
                udtDB.MakeInParam("@Record_Status", BankAcctModel.RecordStatusDataType, BankAcctModel.RecordStatusDataSize, udtBankAcctModel.RecordStatus), _
                udtDB.MakeInParam("@TSMP", BankAcctModel.TSMPDataType, BankAcctModel.TSMPDataSize, udtBankAcctModel.TSMP)}

                udtDB.RunProc("proc_BankAccount_upd_RecordStatus", prams)

                Return True


            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function

        Public Function UpdateBankAcctStagingRecordStatus(ByVal udtBankAcctModel As BankAcctModel, ByRef udtDB As Database) As Boolean
            Try
                Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, udtBankAcctModel.EnrolRefNo), _
                udtDB.MakeInParam("@display_seq", BankAcctModel.DisplaySeqDataType, BankAcctModel.DisplaySeqDataSize, udtBankAcctModel.DisplaySeq), _
                udtDB.MakeInParam("@SP_Practice_Display_Seq", BankAcctModel.SpPracticeDisplaySeqDataType, BankAcctModel.SpPracticeDisplaySeqDataSize, udtBankAcctModel.SpPracticeDisplaySeq), _
                udtDB.MakeInParam("@record_status", BankAcctModel.RecordStatusDataType, BankAcctModel.BankAcctNoDataSize, udtBankAcctModel.RecordStatus), _
                udtDB.MakeInParam("@update_by", BankAcctModel.UpdateByDataType, BankAcctModel.UpdateByDataSize, udtBankAcctModel.UpdateBy), _
                udtDB.MakeInParam("@tsmp", BankAcctModel.TSMPDataType, BankAcctModel.TSMPDataSize, udtBankAcctModel.TSMP)}

                udtDB.RunProc("proc_BankAccountStaging_upd_Status", prams)

                Return True

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw ex
                Return False
            End Try
        End Function
    End Class
End Namespace