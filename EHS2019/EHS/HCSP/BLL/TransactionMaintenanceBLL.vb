Imports Common.ComObject
Imports common.Component
Imports Common.Component.ClaimTrans
Imports Common.Component.UserAC
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.ComFunction
Imports system
Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports HCSP.BLL

Public Class TransactionMaintenanceBLL

    Enum PageState
        ConfirmDetail
        SelectTransaction
        VoidBefore
        VoidAfter
    End Enum

    Public Class SessionObject
        Public Const SESS_VoidableClaimTran As String = "VoidableClaimTran"
        Public Const SESS_VoidableClaimTrans As String = "VoidableClaimTrans"
    End Class

    Dim udtClaimVoucherBLL As ClaimVoucherBLL
    Dim formatter As Formatter
    Dim udtSP As ServiceProviderModel = Nothing
    Dim udtDataEntry As DataEntryUserModel = Nothing
    Dim validator As Validator

    Public Sub New()

    End Sub

    Public Function OnVoid(ByVal udtEHSTransaction As EHSTransaction.EHSTransactionModel) As Boolean
        Dim udtDB As New Database
        Dim udtGeneralFunction As New GeneralFunction

        If IsNothing(udtEHSTransaction.VoidTranNo) OrElse udtEHSTransaction.VoidTranNo.Trim = String.Empty Then
            udtEHSTransaction.VoidTranNo = udtGeneralFunction.generateSystemNum("V")
        Else
            Return False
        End If

        ' Void the temporary / special account
        Dim udtEHSAccount As EHSAccount.EHSAccountModel = udtEHSTransaction.EHSAcct

        Dim blnErasedAmendHistroy As Boolean = False

        '==================================================================== Code for SmartID ============================================================================
        ' Get the temp account with account purpose = 'O'
        Dim udtEHSAccountBLL As EHSAccount.EHSAccountBLL = New EHSAccount.EHSAccountBLL
        Dim udtEHSAccount_Original As EHSAccount.EHSAccountModel = Nothing

        If udtEHSAccount.AccountSource = EHSAccount.EHSAccountModel.SysAccountSource.TemporaryAccount Then
            If Not IsNothing(udtEHSAccount.OriginalAmendAccID) AndAlso Not udtEHSAccount.OriginalAmendAccID.Equals(String.Empty) AndAlso udtEHSAccount.AccountPurpose.Trim = EHSAccount.EHSAccountModel.AccountPurposeClass.ForAmendment Then
                udtEHSAccount_Original = udtEHSAccountBLL.LoadTempEHSAccountByVRID(udtEHSAccount.OriginalAmendAccID)
                blnErasedAmendHistroy = True
            End If
        End If
        '==================================================================================================================================================================

        Try
            udtDB.BeginTransaction()

            'CRE13-006 HCVS Ceiling [Start][Karl]
            Dim udtTransactionBLL As New EHSTransactionBLL
            Dim udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = udtEHSTransaction.EHSAcct.EHSPersonalInformationList.Filter(udtEHSTransaction.DocCode)

            Dim drTSMPRow As DataRow = udtTransactionBLL.getEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum)

            'CRE13-006 HCVS Ceiling [End][Karl]

            ' Update [VoucherTransaction]
            Dim prams() As SqlParameter = { _
                udtDB.MakeInParam("@transaction_id", SqlDbType.Char, 20, udtEHSTransaction.TransactionID), _
                udtDB.MakeInParam("@void_transaction_id", SqlDbType.Char, 20, udtEHSTransaction.VoidTranNo), _
                udtDB.MakeInParam("@void_remark", SqlDbType.NVarChar, 255, udtEHSTransaction.VoidReason), _
                udtDB.MakeInParam("@void_by", SqlDbType.Char, 20, udtEHSTransaction.VoidUser), _
                udtDB.MakeInParam("@void_by_DataEntry", SqlDbType.Char, 20, IIf(udtEHSTransaction.VoidByDataEntry = String.Empty, DBNull.Value, udtEHSTransaction.VoidByDataEntry)), _
                udtDB.MakeInParam("@tsmp", SqlDbType.Timestamp, 20, udtEHSTransaction.TSMP)}

            udtDB.RunProc("proc_VoucherTransaction_update_void", prams)

            Dim udtSubsidizeWriteOffBLL As New SubsidizeWriteOffBLL()

            'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            udtSubsidizeWriteOffBLL.UpdateWriteOff(udtEHSTransaction.ServiceDate, _
                                                   udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, _
                                                   udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, _
                                                   udtEHSPersonalInfo.DOD, udtEHSPersonalInfo.ExactDOD, _
                                                   udtEHSTransaction.SchemeCode, udtEHSTransaction.TransactionDetails(0).SubsidizeCode, _
                                                   eHASubsidizeWriteOff_CreateReason.TxRemoval, udtDB)
            'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

            Select Case udtEHSAccount.AccountSource
                Case EHSAccount.EHSAccountModel.SysAccountSource.TemporaryAccount
                    'Dim udtEHSAccountBLL As New EHSAccount.EHSAccountBLL
                    Dim dtmVoid As Date = udtGeneralFunction.GetSystemDateTime()
                    udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccount, udtEHSTransaction.ServiceProviderID, dtmVoid)

                    '==================================================================== Code for SmartID ============================================================================
                    ' Also remove the temp account with account purpose = 'O'
                    If Not IsNothing(udtEHSAccount_Original) AndAlso udtEHSAccount_Original.AccountPurpose.Trim = EHSAccount.EHSAccountModel.AccountPurposeClass.ForAmendmentOld Then
                        udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccount_Original, udtEHSTransaction.ServiceProviderID, dtmVoid)
                    End If

                    If blnErasedAmendHistroy Then
                        ' Update PersonalInfoAmendHistory RecordStats = 'E' (Erased) and SubmitToVerify = 'N' (Doesn't verify)
                        If Not IsNothing(udtEHSAccount.ValidatedAccID) AndAlso Not udtEHSAccount.ValidatedAccID.Equals(String.Empty) Then
                            udtEHSAccountBLL.UpdatePersonalInfoAmendHistoryWithdrawAmendment(udtDB, udtEHSAccount, udtEHSTransaction.ServiceProviderID)
                        End If
                    End If
                    '==================================================================================================================================================================

                Case EHSAccount.EHSAccountModel.SysAccountSource.SpecialAccount
                    'Dim udtEHSAccountBLL As New EHSAccount.EHSAccountBLL
                    Dim dtmVoid As Date = udtGeneralFunction.GetSystemDateTime()
                    udtEHSAccountBLL.UpdateSpecialEHSAccountReject(udtDB, udtEHSAccount, udtEHSTransaction.ServiceProviderID, dtmVoid)

            End Select


            'CRE13-006 HCVS Ceiling [Start][Karl]
            If drTSMPRow Is Nothing Then
                udtTransactionBLL.insertEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSTransaction.ServiceProviderID)
            Else
                udtTransactionBLL.updateEHSAccountTSMP(udtDB, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSTransaction.ServiceProviderID, CType(drTSMPRow("TSMP"), Byte()))
            End If

            'CRE13-006 HCVS Ceiling [End][Karl]

            udtDB.CommitTransaction()

            Return True

        Catch eSQL As SqlException
            udtDB.RollBackTranscation()
            Throw eSQL
            'Throw New TransactionVoidSqlException(eSQL.Message, eSQL.Number)

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex

        End Try

    End Function

    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
    ' Obsolete functions that are no longer used
    ' -----------------------------------------------------------------------------------------  

    'Public Sub OnVoid(ByVal currentClaimTran As ClaimTransModel)
    '    Dim dt As DataTable
    '    Dim udtdb As Database = New Database
    '    Dim generalFunction As GeneralFunction = New GeneralFunction
    '    Dim dataEntryAccount As Object
    '    Dim strRefNo As String

    '    If currentClaimTran.DataEntryAccount.Equals(String.Empty) Or currentClaimTran.DataEntryAccount Is Nothing Then
    '        dataEntryAccount = DBNull.Value
    '    Else
    '        dataEntryAccount = currentClaimTran.DataEntryAccount
    '    End If

    '    strRefNo = currentClaimTran.VoidTranNo
    '    If strRefNo Is Nothing Or strRefNo.Trim.Equals(String.Empty) Then
    '        strRefNo = generalFunction.generateSystemNum("V")
    '    End If

    '    dt = New DataTable
    '    udtdb.BeginTransaction()

    '    'Update Voucher Transaction table
    '    Dim prams() As SqlParameter = { _
    '        udtdb.MakeInParam("@transaction_id", SqlDbType.Char, 20, currentClaimTran.TranNo), _
    '        udtdb.MakeInParam("@void_transaction_id", SqlDbType.Char, 20, strRefNo), _
    '        udtdb.MakeInParam("@void_remark", SqlDbType.NVarChar, 255, currentClaimTran.VoidReason), _
    '        udtdb.MakeInParam("@void_by", SqlDbType.Char, 20, currentClaimTran.ServiceProviderID), _
    '        udtdb.MakeInParam("@void_by_DataEntry", SqlDbType.Char, 20, dataEntryAccount), _
    '        udtdb.MakeInParam("@tsmp", SqlDbType.Timestamp, 20, currentClaimTran.TSMP)}
    '    Try
    '        udtdb.RunProc("proc_VoucherTransaction_update_void", prams, dt)
    '    Catch eSQL As SqlException
    '        udtdb.RollBackTranscation()
    '        Throw New TransactionVoidSqlException(eSQL.Message, eSQL.Number)
    '    Catch ex As Exception
    '        udtdb.RollBackTranscation()
    '        Throw ex
    '    End Try


    '    Dim strStorProc As String
    '    If currentClaimTran.VoucherRecipientAcct.AcctType.Equals(VRAcctType.Validated) Then
    '        strStorProc = "proc_VoucherAccount_update_void"
    '    Else
    '        strStorProc = "proc_TempVoucherAccount_update_void"
    '    End If

    '    'Update Voucher Account table
    '    prams = New SqlParameter() { _
    '        udtdb.MakeInParam("@voucher_acc_id", SqlDbType.Char, 15, currentClaimTran.VoucherRecipientAcct.VRAcctID), _
    '        udtdb.MakeInParam("@voucher_used", SqlDbType.SmallInt, 4, currentClaimTran.VoucherRecipientAcct.VoucherRedeem - currentClaimTran.VoucherRedeem), _
    '        udtdb.MakeInParam("@total_voucher_amt_used", SqlDbType.Money, 4, currentClaimTran.VoucherRecipientAcct.TotalUsedVoucherAmount - (currentClaimTran.VoucherRedeem * currentClaimTran.VoucherAmount)), _
    '        udtdb.MakeInParam("@update_by", SqlDbType.Char, 20, currentClaimTran.ServiceProviderID), _
    '        udtdb.MakeInParam("@dataEntry_by", SqlDbType.Char, 20, dataEntryAccount), _
    '        udtdb.MakeInParam("@tsmp", SqlDbType.Timestamp, 20, currentClaimTran.VoucherRecipientAcct.TSMP)}
    '    Try
    '        udtdb.RunProc(strStorProc, prams, dt)
    '        'udtdb.CommitTransaction()
    '    Catch eSQL As SqlException
    '        udtdb.RollBackTranscation()
    '        Throw New TransactionVoidSqlException(eSQL.Message, eSQL.Number)
    '    Catch ex As Exception
    '        udtdb.RollBackTranscation()
    '        Throw ex
    '    End Try

    '    'Clear the TempVRAcctPendingVerify table
    '    Try
    '        Dim udtVRAcctBll As New Common.Component.VoucherRecipientAccount.VoucherRecipientAccountBLL
    '        udtVRAcctBll.DeleteTempVRAcctPendingVerify(udtdb, currentClaimTran.VoucherRecipientAcct.VRAcctID)
    '        udtdb.CommitTransaction()
    '    Catch eSQL As SqlException
    '        udtdb.RollBackTranscation()
    '        Throw New TransactionVoidSqlException(eSQL.Message, eSQL.Number)
    '    Catch ex As Exception
    '        udtdb.RollBackTranscation()
    '        Throw ex
    '    End Try

    'End Sub
    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]    

    Public Function LoadClaimTrans(ByVal strHKID As String, ByVal strDOB As String) As VoidableClaimTranModelCollection
        Dim formatter As Formatter = New Formatter
        Dim commfunct As GeneralFunction = New GeneralFunction
        Dim udtVoidableClaimTrans As VoidableClaimTranModelCollection = Nothing
        Dim isValid As Boolean = True

        If isValid Then
            Dim isExactDate As String = String.Empty
            Dim dateDOB As DateTime

            'If strDOB.Length() = 4 Then
            '    isExactDate = "N"
            '    strDOB = String.Format("1 JAN {0}", strDOB)4
            'Else
            '    isExactDate = "Y"
            '    strDOB = formatter.convertDate(strDOB, Nothing)
            'End If
            'dateDOB = CType(strDOB, Date)

            commfunct.chkDOBtype(strDOB, dateDOB, isExactDate)
            Dim strDataEntryAccount As String = String.Empty
            If Not Me.udtDataEntry Is Nothing Then strDataEntryAccount = Me.udtDataEntry.DataEntryAccount

            udtVoidableClaimTrans = Me.LoadVoidableClaimTran(strHKID, dateDOB, isExactDate, Me.udtSP.SPID, strDataEntryAccount)

        End If
        Return udtVoidableClaimTrans
    End Function

    Public Function LoadVoidableClaimTran(ByVal strHKID As String, ByVal dateDOB As DateTime, ByVal isExactDOB As String, ByVal strSPID As String, ByVal strDataEntryAcc As String) As VoidableClaimTranModelCollection
        Dim dt As DataTable = New DataTable
        Dim udtdb As Database = New Database()

        Dim prams() As SqlParameter = { _
            udtdb.MakeInParam("@HKID", SqlDbType.Char, 9, strHKID), _
            udtdb.MakeInParam("@DOB", SqlDbType.DateTime, 8, dateDOB), _
            udtdb.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, isExactDOB), _
            udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
            udtdb.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, strDataEntryAcc)}
        udtdb.RunProc("proc_VoucherTransactionVoid_get_ByHKIDDOB", prams, dt)

        Dim udtVoidableClaimTrans As VoidableClaimTranModelCollection = New VoidableClaimTranModelCollection

        If dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                udtVoidableClaimTrans.Add(Me.FillVoidableClaimTran(dr))
            Next
            Return udtVoidableClaimTrans
        Else
            Return Nothing
        End If
    End Function

    Public Function LoadClaimTrans(ByVal strHKID As String, ByVal strAge As String, ByVal dtmRegDate As DateTime) As VoidableClaimTranModelCollection
        Dim udtVoidableClaimTrans As VoidableClaimTranModelCollection = Nothing

        Dim strDataEntryAccount As String = String.Empty
        If Not Me.udtDataEntry Is Nothing Then strDataEntryAccount = Me.udtDataEntry.DataEntryAccount

        udtVoidableClaimTrans = Me.LoadVoidableClaimTran(strHKID, strAge, dtmRegDate, "A", Me.udtSP.SPID, strDataEntryAccount)

        Return udtVoidableClaimTrans
    End Function

    Public Function LoadVoidableClaimTran(ByVal strHKID As String, ByVal strAge As String, ByVal dtmRegDate As DateTime, ByVal isExactDOB As String, ByVal strSPID As String, ByVal strDataEntryAcc As String) As VoidableClaimTranModelCollection
        Dim dt As DataTable = New DataTable
        Dim udtdb As Database = New Database()

        Dim prams() As SqlParameter = { _
            udtdb.MakeInParam("@HKID", SqlDbType.Char, 9, strHKID), _
            udtdb.MakeInParam("@EC_Age", SqlDbType.SmallInt, 0, strAge), _
            udtdb.MakeInParam("@EC_Date_of_Registration", SqlDbType.DateTime, 8, dtmRegDate), _
            udtdb.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, isExactDOB), _
            udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
            udtdb.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, strDataEntryAcc)}
        udtdb.RunProc("proc_VoucherTransactionVoid_get_ByHKIDAgeDOR", prams, dt)

        Dim udtVoidableClaimTrans As VoidableClaimTranModelCollection = New VoidableClaimTranModelCollection

        If dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                udtVoidableClaimTrans.Add(Me.FillVoidableClaimTran(dr))
            Next
            Return udtVoidableClaimTrans
        Else
            Return Nothing
        End If
    End Function

    Public Function LoadVoidableClaimTran(ByVal strSPID As String, ByVal strPartialTransNo As String) As VoidableClaimTranModelCollection
        Dim dt As DataTable = New DataTable
        Dim udtdb As Database = New Database()
        Dim udtVoidableClaimTrans As VoidableClaimTranModelCollection = New VoidableClaimTranModelCollection

        Dim prams() As SqlParameter = { _
            udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
            udtdb.MakeInParam("@Partial_Trans_No", SqlDbType.VarChar, 20, strPartialTransNo)}
        udtdb.RunProc("proc_VoucherTransactionVoid_get_ByPartialTransNo", prams, dt)

        If dt.Rows.Count > 0 Then
            For Each dr As DataRow In dt.Rows
                udtVoidableClaimTrans.Add(Me.FillVoidableClaimTran(dr))
            Next
        End If

        Return udtVoidableClaimTrans
    End Function

    Public Function FillVoidableClaimTran(ByVal dr As DataRow) As VoidableClaimTranModel
        Dim udtVoidableClaimTran As VoidableClaimTranModel = New VoidableClaimTranModel
        udtVoidableClaimTran.TranDate = CType(dr("Transaction_Dtm"), DateTime)
        If Not dr("Confirmed_Dtm").Equals(DBNull.Value) Then
            udtVoidableClaimTran.ConfirmedDtm = CType(dr("Confirmed_Dtm"), DateTime)
        End If
        If Not dr("DataEntry_By").Equals(String.Empty) Then
            udtVoidableClaimTran.DataEntryBy = dr("DataEntry_By")
        End If
        udtVoidableClaimTran.RecordStatus = dr("Record_Status")
        udtVoidableClaimTran.SPID = dr("SP_ID")
        udtVoidableClaimTran.TranNo = dr("Transaction_ID")
        udtVoidableClaimTran.VoucherAccID = dr("Voucher_Acc_ID")

        Return udtVoidableClaimTran
    End Function

    Shared Sub saveVoidableClaimTranCollectionSession(ByVal udtVoidClaimTrans As VoidableClaimTranModelCollection)
        HttpContext.Current.Session(SessionObject.SESS_VoidableClaimTrans) = udtVoidClaimTrans
    End Sub

    Shared Sub cleanVoidableClaimTranCollectionSession()
        HttpContext.Current.Session.Remove(SessionObject.SESS_VoidableClaimTrans)
    End Sub

    Shared Function loadVoidableClaimTranSession() As VoidableClaimTranModelCollection
        Dim udtVoidableClaimTranModels As VoidableClaimTranModelCollection = Nothing
        If Not HttpContext.Current.Session(SessionObject.SESS_VoidableClaimTrans) Is Nothing Then
            Try
                udtVoidableClaimTranModels = CType(HttpContext.Current.Session(SessionObject.SESS_VoidableClaimTrans), VoidableClaimTranModelCollection)
            Catch ex As Exception
                Throw New Exception("Invalid Session Claim Tran Collection!")
            End Try
        Else
            Throw New Exception("Session Expired!")
        End If
        Return udtVoidableClaimTranModels
    End Function

    Public ReadOnly Property ServiceProvider() As ServiceProviderModel
        Get
            Return Me.udtSP
        End Get
    End Property

    Public ReadOnly Property DataEntry() As DataEntryUserModel
        Get
            Return Me.udtDataEntry
        End Get
    End Property

    Public Class TransactionVoidSqlException
        Inherits Exception

        Private _udtSystemMessage As SystemMessage

        Public Sub New(ByVal strMessageCode As String, ByVal strSQLNumber As Integer)
            MyBase.New(strMessageCode)
            If strSQLNumber = 50000 Then
                Me._udtSystemMessage = New SystemMessage("990001", "D", strMessageCode)
            End If
        End Sub

        Public Sub New(ByVal strMessageCode As String)
            Me._udtSystemMessage = New SystemMessage("990001", "D", strMessageCode)
        End Sub

        Public ReadOnly Property SystemMessage() As SystemMessage
            Get
                Return Me._udtSystemMessage
            End Get
        End Property
    End Class

    Public Class TransactionVoidException
        Inherits System.Exception

        Public Sub New(ByVal strMessage As String)
            MyBase.New(strMessage)
        End Sub
    End Class

    Public Function SearchClaimTrans(ByVal strSPID As String, ByVal strDataEntry As String, ByVal intPracticeSeq As Integer, ByVal intBankAcctSeq As Integer, ByVal strStatus As String, ByVal dtTranDateFrom As Date, ByVal dtTranDateTo As Date, ByVal strTranNo As String, ByVal strSchemeCode As String, ByVal strDocType As String, ByVal strDocumentNo1 As String, ByVal strDocumentNo2 As String, ByVal enumSubPlatform As [Enum]) As DataTable
        Dim ds As New DataSet
        Dim udtDB As Database = New Database

        Dim strSubPlatform As String = String.Empty
        If Not enumSubPlatform Is Nothing Then
            strSubPlatform = enumSubPlatform.ToString
        End If

        Dim prams() As SqlParameter = { _
            udtDB.MakeInParam("@TransactionID", SqlDbType.Char, 20, IIf(strTranNo = String.Empty, DBNull.Value, strTranNo)), _
            udtDB.MakeInParam("@TranDtmFrom", SqlDbType.DateTime, 4, dtTranDateFrom), _
            udtDB.MakeInParam("@TranDtmTo", SqlDbType.DateTime, 4, dtTranDateTo), _
            udtDB.MakeInParam("@SPID", SqlDbType.Char, 8, IIf(strSPID = String.Empty, DBNull.Value, strSPID)), _
            udtDB.MakeInParam("@DataEntry", SqlDbType.VarChar, 20, IIf(strDataEntry = String.Empty, DBNull.Value, strDataEntry)), _
            udtDB.MakeInParam("@Practice_Seq", SqlDbType.SmallInt, 2, IIf(intPracticeSeq = 0, DBNull.Value, intPracticeSeq)), _
            udtDB.MakeInParam("@Record_Status", SqlDbType.Char, 1, IIf(strStatus = String.Empty, DBNull.Value, strStatus)), _
            udtDB.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, IIf(strSchemeCode = String.Empty, DBNull.Value, strSchemeCode)), _
            udtDB.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform)), _
            udtDB.MakeInParam("@doc_code", SqlDbType.Char, 20, IIf(strDocType = String.Empty, DBNull.Value, strDocType)), _
            udtDB.MakeInParam("@identity_no1", SqlDbType.VarChar, 20, IIf(strDocumentNo1 = String.Empty, DBNull.Value, strDocumentNo1)), _
            udtDB.MakeInParam("@Adoption_Prefix_Num", SqlDbType.Char, 7, IIf(strDocumentNo2 = String.Empty, DBNull.Value, strDocumentNo2))}

        udtDB.RunProc("proc_VoucherTransaction_get_BySPIDBankAcctTransID", prams, ds)

        ' Massage the data
        Dim dtData As DataTable = ds.Tables(0)
        Dim dtOtherInfo As DataTable = ds.Tables(1)

        dtData.Columns.Add("Information_Code", GetType(String))
        dtData.Columns.Add("Information_Code_Chi", GetType(String))
        dtData.Columns.Add("Information_Code_CN", GetType(String))

        Dim lstOtherInfoEN As List(Of String) = Nothing
        Dim lstOtherInfoTC As List(Of String) = Nothing
        Dim lstOtherInfoSC As List(Of String) = Nothing

        For Each drData As DataRow In dtData.Rows
            lstOtherInfoEN = New List(Of String)
            lstOtherInfoTC = New List(Of String)
            lstOtherInfoSC = New List(Of String)

            For Each drOtherInfo As DataRow In dtOtherInfo.Select(String.Format("Transaction_ID = '{0}'", drData("Transaction_ID").ToString.Trim), _
                                                                  "Item_Group_Seq, Display_Seq")
                lstOtherInfoEN.Add(drOtherInfo("Content_EN").ToString.Trim)
                lstOtherInfoTC.Add(drOtherInfo("Content_TC").ToString.Trim)
                lstOtherInfoSC.Add(drOtherInfo("Content_SC").ToString.Trim)

            Next

            ' INT16-0032 Long term fix for HCSP Record Confirmation [Start][Winnie]
            If lstOtherInfoEN.Count > 0 Then
                drData("Information_Code") = String.Join(vbCrLf, lstOtherInfoEN.ToArray)
            Else
                drData("Information_Code") = String.Empty
            End If

            If lstOtherInfoTC.Count > 0 Then
                drData("Information_Code_Chi") = String.Join(vbCrLf, lstOtherInfoTC.ToArray)
            Else
                drData("Information_Code_Chi") = String.Empty
            End If

            If lstOtherInfoSC.Count > 0 Then
                drData("Information_Code_CN") = String.Join(vbCrLf, lstOtherInfoSC.ToArray)
            Else
                drData("Information_Code_CN") = String.Empty
            End If
            ' INT16-0032 Long term fix for HCSP Record Confirmation [End][Winnie]
        Next

        Return dtData

    End Function

    Public Function chkVoidReason(ByVal strVoidReason As String) As SystemMessage
        Dim sm As SystemMessage

        If strVoidReason.Trim().Equals(String.Empty) Then
            sm = New SystemMessage(Common.Component.FunctCode.FUNT020301, "E", Common.Component.MsgCode.MSG00002)
        Else
            sm = Nothing
        End If

        Return sm

    End Function

    Public Function CheckTransactionVoidable(ByVal strTransactionNo As String) As Boolean
        Dim udtEHSTransactionBLL As New EHSTransactionBLL
        Dim udtEHSTransaction As EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(strTransactionNo)

        Return CheckTransactionVoidable(udtEHSTransaction)

    End Function

    Public Function CheckTransactionVoidable(ByVal udtEHSTransaction As EHSTransactionModel) As Boolean
        Dim udtGeneralFunction As New GeneralFunction
        Dim dtmNow As DateTime = udtGeneralFunction.GetSystemDateTime()

        Dim VoidablePeriodHour As Integer = 24

        If IsNothing(udtEHSTransaction) Then Return False

        Select Case udtEHSTransaction.RecordStatus

            'CRE13-001 EHAPP [Start][Karl]
            '----------------------------------------------------------------------------------------------------------------
            'Case ClaimTransStatus.Active
            Case ClaimTransStatus.Active, ClaimTransStatus.Joined
                If udtEHSTransaction.RecordStatus = ClaimTransStatus.Joined Then
                    If udtEHSTransaction.ManualReimburse = True Then Return False
                End If
                'CRE13-001 EHAPP [End][Karl]

                If IsNothing(udtEHSTransaction.AuthorisedStatus) OrElse udtEHSTransaction.AuthorisedStatus.Trim = String.Empty Then
                    If udtEHSTransaction.EHSAcct.AccountSource = EHSAccount.EHSAccountModel.SysAccountSource.InvalidAccount Then Return False

                    If dtmNow <= udtEHSTransaction.ConfirmDate.Value.AddHours(VoidablePeriodHour) Then Return True

                    If udtEHSTransaction.EHSAcct.AccountSource = EHSAccount.EHSAccountModel.SysAccountSource.SpecialAccount _
                            AndAlso udtEHSTransaction.EHSAcct.RecordStatus = VRAcctValidatedStatus.Invalid Then Return True
                End If

            Case ClaimTransStatus.Pending
                If Not udtEHSTransaction.ConfirmDate.HasValue Then Return True

            Case ClaimTransStatus.PendingVRValidate
                If udtEHSTransaction.EHSAcct.RecordStatus = VRAcctValidatedStatus.Invalid _
                        OrElse dtmNow <= udtEHSTransaction.ConfirmDate.Value.AddHours(VoidablePeriodHour) Then
                    Return True
                End If
            Case ClaimTransStatus.Incomplete
                Return True
        End Select

        Return False

    End Function

    Public Function CheckTransactionEditable(ByVal udtEHSTransaction As EHSTransactionModel, ByVal udtUserACModel As UserAC.UserACModel) As Boolean
        ' No transaction model
        If IsNothing(udtEHSTransaction) Then Return False

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------

        'Dim udtGeneralFunction As New GeneralFunction
        'If Not udtGeneralFunction.IsCoPaymentFeeEnabled(udtEHSTransaction.ServiceDate) Then Return False

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        Dim blnEditable As Boolean = False
        Select Case udtEHSTransaction.RecordStatus
            Case EHSTransactionModel.TransRecordStatusClass.Pending
                ' Only Allow Service Provider to edit in [Pending Confirmation] status for HCVS
                If udtUserACModel.UserType = SPAcctType.ServiceProvider Then
                    'CRE13-019-02 Extend HCVS to China [Start][Karl]
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                    If New Scheme.SchemeClaimBLL().ConvertControlTypeFromSchemeClaimCode(udtEHSTransaction.SchemeCode) = Scheme.SchemeClaimModel.EnumControlType.VOUCHER Then                        
                        'CRE13-019-02 Extend HCVS to China [End][Karl]
                        blnEditable = True
                    Else
                        blnEditable = False
                    End If
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                Else
                    blnEditable = False
                End If

            Case EHSTransactionModel.TransRecordStatusClass.Incomplete
                ' Allow edit in [Incomplete] status
                ' Edit transaction created after co-payment fee effective
                blnEditable = True

            Case EHSTransactionModel.TransRecordStatusClass.Active
                ' Not allow edit in [Ready to Reimburse] status for Service Provider only
                If udtUserACModel.UserType = SPAcctType.ServiceProvider Then
                    ' Service Provider is allow edit in [Ready to Reimburse] status if reimburse process is not yet started
                    If udtEHSTransaction.AuthorisedStatus = String.Empty Then
                        blnEditable = False
                    Else
                        blnEditable = False
                    End If
                Else
                    ' Data Entry is not allow edit in [Ready to Reimburse] status
                    blnEditable = False
                End If
            Case EHSTransactionModel.TransRecordStatusClass.PendingVRValidate
                If udtUserACModel.UserType = SPAcctType.ServiceProvider Then
                    ' Service Provider is not allow edit in [Ready eHealth Account Validation] status if reimburse process is not yet started
                    blnEditable = False
                Else
                    ' Data Entry is not allow edit after Servie Provider completed and confirmed transaction
                    blnEditable = False
                End If

            Case Else
                blnEditable = False

        End Select


        'If blnEditable Then

        '    ' Edit transaction created after co-payment fee effective
        '    If udtEHSTransaction.ServiceDate < New Date(2012, 1, 1) Then
        '        blnEditable = False
        '    End If

        'End If

        Return blnEditable
    End Function

#Region "Commented Code"

    '-------------------------------------------------------------------------------------------------------------
    ' check availabel for void functions
    '-------------------------------------------------------------------------------------------------------------
    'Public Function chkClaimTran(ByVal udtClaimTran As ClaimTransModel) As SystemMessage
    '    Dim systemMessage As SystemMessage = Nothing
    '    Dim isValid As Boolean = True
    '    If udtClaimTran Is Nothing Then
    '        'Show system message
    '        isValid = False
    '        systemMessage = New SystemMessage("020302", "E", "00002")
    '    Else

    '        If udtClaimTran.Status.Equals(Common.Component.ClaimTransStatus.Active) Then ''added by tim
    '            systemMessage = Me.chkVailDateForVoid(udtClaimTran.ConfirmDate)
    '            If Not systemMessage Is Nothing Then
    '                isValid = False
    '            End If
    '        Else
    '            isValid = True
    '        End If

    '        If isValid Then
    '            systemMessage = Me.chkAllowVoid(udtClaimTran)
    '            If Not systemMessage Is Nothing Then
    '                isValid = False
    '            End If
    '        End If
    '    End If
    '    Return systemMessage
    'End Function

    'Private Function chkVailDateForVoid(ByVal confirmDate As Nullable(Of DateTime)) As SystemMessage
    '    Dim systemMessage As SystemMessage = Nothing
    '    If confirmDate.HasValue Then
    '        Dim datConfirmDate As Date = CType(confirmDate, DateTime)

    '        Dim generalFunction As GeneralFunction = New GeneralFunction()

    '        If datConfirmDate.AddDays(1) < generalFunction.GetSystemDateTime() Then
    '            systemMessage = New SystemMessage("020302", "E", "00003")
    '        End If
    '    End If

    '    Return systemMessage
    'End Function

    'Private Function chkValidRoleForVoid(ByVal udtClaimTran As ClaimTransModel, ByVal udtUserAC As UserACModel) As SystemMessage
    '    Dim udtClaimVoucherBLL As ClaimVoucherBLL = New ClaimVoucherBLL
    '    Dim systemMessage As SystemMessage = Nothing
    '    Dim udtSP As ServiceProviderModel
    '    Dim udtDataEntry As DataEntryUserModel
    '    Dim vailResult As Boolean = True

    '    'Check using account
    '    If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
    '        udtSP = CType(udtUserAC, ServiceProviderModel)
    '        'udtSP = udtClaimVoucherBLL.loadSP(udtSP.SPID)

    '        If udtClaimTran.ServiceProviderID.Equals(udtSP.SPID) Then
    '            If Not udtClaimTran.Status.Equals("P") And Not udtClaimTran.Status.Equals("A") And Not udtClaimTran.Status.Equals("V") Then
    '                systemMessage = New SystemMessage("020302", "E", "00003")
    '            End If
    '        Else
    '            vailResult = False
    '        End If
    '    Else
    '        udtDataEntry = CType(udtUserAC, DataEntryUserModel)
    '        udtSP = udtClaimVoucherBLL.loadSP(udtDataEntry.SPID)

    '        If Not udtClaimTran.DataEntryAccount.Equals(udtDataEntry.DataEntryAccount) Or Not udtClaimTran.Status.Equals("P") Then
    '            vailResult = False
    '        End If

    '    End If
    '    If Not vailResult Then
    '        systemMessage = New SystemMessage("020302", "E", "00004")
    '    End If

    '    Return systemMessage
    'End Function

    'Private Function chkAllowVoid(ByVal udtClaimTran As ClaimTransModel) As SystemMessage
    '    Dim systemMessage As SystemMessage = Nothing

    '    If udtClaimTran.Status.Equals(Common.Component.ClaimTransStatus.Inactive) Then 'not VoidTranNo.Equals(String.Empty) And Not udtClaimTran.VoidTranNo Is Nothing Then
    '        systemMessage = New SystemMessage("020302", "E", "00006")
    '    Else
    '        If udtClaimTran.VoucherRecipientAcct.AcctValidatedStatus.Equals(Common.Component.VRAcctValidatedStatus.PendingForVerify) Then
    '            systemMessage = Me.chkVailDateForVoid(udtClaimTran.ConfirmDate)
    '        Else
    '            systemMessage = Nothing
    '        End If
    '    End If

    '    If systemMessage Is Nothing Then
    '        If Me.udtDataEntry Is Nothing Then
    '            systemMessage = chkValidRoleForVoid(udtClaimTran, Me.udtSP)
    '        Else
    '            systemMessage = chkValidRoleForVoid(udtClaimTran, Me.udtDataEntry)
    '        End If
    '    End If

    '    Return systemMessage
    'End Function

#End Region
End Class

