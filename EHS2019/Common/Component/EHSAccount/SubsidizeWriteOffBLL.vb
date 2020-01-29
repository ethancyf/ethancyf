Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.ComFunction
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.VoucherInfo

Namespace Component.EHSAccount

    Public Class SubsidizeWriteOffBLL

        Private Function GetAllSubsidizeWriteOff(ByVal strDocCode As String, ByVal strDocID As String, ByVal dtmDOB As DateTime, ByVal strExactDOB As String, _
        ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, Optional ByVal udtDB As Database = Nothing) As SubsidizeWriteOffModelCollection

            Dim udtSubsidizeWriteOffList As New SubsidizeWriteOffModelCollection()
            Dim udtSubsidizeWriteOffModel As SubsidizeWriteOffModel = Nothing
            Dim udtFormatter As Formatter = New Formatter
            Dim intPValueCeiling As Nullable(Of Integer)

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            strDocID = udtFormatter.formatDocumentIdentityNumber(strDocCode, strDocID)

            Try
                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", SubsidizeWriteOffModel.DocCodeDataType, SubsidizeWriteOffModel.DocCodeDataSize, strDocCode), _
                    udtDB.MakeInParam("@Doc_ID", SubsidizeWriteOffModel.DocIDDataType, SubsidizeWriteOffModel.DocIDDataSize, strDocID), _
                    udtDB.MakeInParam("@DOB", SubsidizeWriteOffModel.DOBDataType, SubsidizeWriteOffModel.DOBDataSize, dtmDOB), _
                    udtDB.MakeInParam("@Exact_DOB", SubsidizeWriteOffModel.ExactDOBDataType, SubsidizeWriteOffModel.ExactDOBDataSize, strExactDOB), _
                    udtDB.MakeInParam("@Scheme_Code", SubsidizeWriteOffModel.SchemeCodeDataType, SubsidizeWriteOffModel.SchemeCodeDataSize, IIf(String.IsNullOrEmpty(strSchemeCode) = True, DBNull.Value, strSchemeCode)), _
                    udtDB.MakeInParam("@Subsidize_Code", SubsidizeWriteOffModel.SubsidizeCodeDataType, SubsidizeWriteOffModel.SubsidizeCodeDataSize, IIf(String.IsNullOrEmpty(strSubsidizeCode) = True, DBNull.Value, strSubsidizeCode))}

                udtDB.RunProc("proc_eHASubsidizeWriteOff_get_byDocCodeDocIDDOBSchemeSubsidize", params, dt)

                For Each dr As DataRow In dt.Rows

                    If IsDBNull(dr("PValue_Ceiling")) Then
                        intPValueCeiling = Nothing
                    Else
                        intPValueCeiling = CInt(dr.Item("PValue_Ceiling"))
                    End If

                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                    ' -----------------------------------------------------------------------------------------
                    udtSubsidizeWriteOffModel = New SubsidizeWriteOffModel(Trim(dr.Item("Doc_Code")), dr.Item("Doc_ID"), CDate(dr.Item("DOB")), dr.Item("Exact_DOB"), _
                    Trim(CStr(dr.Item("Scheme_Code"))), CInt(dr.Item("Scheme_Seq")), Trim(CStr(dr.Item("Subsidize_Code"))), CInt(dr.Item("WriteOff_Unit")), _
                    CDbl(dr.Item("WriteOff_Per_Unit_Value")), intPValueCeiling, CInt(dr.Item("PValue_TotalEntitlement")), CInt(dr.Item("PValue_SeasonEntitlement")), _
                    CInt(dr.Item("PValue_TotalUsed")), CInt(dr.Item("PValue_SeasonUsed")), CInt(dr.Item("PValue_Available")), _
                    CDate(dr.Item("Create_Dtm")), CStr(dr.Item("Create_Reason")), CType(dr.Item("TSMP"), Byte()), _
                    CInt(dr.Item("PValue_TotalRefund")), CInt(dr.Item("PValue_SeasonRefund")))
                    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                    udtSubsidizeWriteOffList.Add(udtSubsidizeWriteOffModel)
                Next

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

            Return udtSubsidizeWriteOffList

        End Function
        Private Sub InsertWriteOff(ByVal udtSubsidizeWriteOffModel As SubsidizeWriteOffModel, Optional ByVal udtDB As Database = Nothing)
            Dim intPValueCeiling As Integer

            If udtSubsidizeWriteOffModel.PValueCeiling.HasValue = True Then
                intPValueCeiling = CInt(udtSubsidizeWriteOffModel.PValueCeiling)
            End If

            If udtDB Is Nothing Then udtDB = New Database()

            Try
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                ' Add Refund
                Dim params() As SqlParameter = {udtDB.MakeInParam("@Doc_Code", SubsidizeWriteOffModel.DocCodeDataType, SubsidizeWriteOffModel.DocCodeDataSize, udtSubsidizeWriteOffModel.DocCode), _
                                                udtDB.MakeInParam("@Doc_ID", SubsidizeWriteOffModel.DocIDDataType, SubsidizeWriteOffModel.DocIDDataSize, udtSubsidizeWriteOffModel.DocID), _
                                                udtDB.MakeInParam("@DOB", SubsidizeWriteOffModel.DOBDataType, SubsidizeWriteOffModel.DOBDataSize, udtSubsidizeWriteOffModel.DOB), _
                                                udtDB.MakeInParam("@Exact_DOB", SubsidizeWriteOffModel.ExactDOBDataType, SubsidizeWriteOffModel.ExactDOBDataSize, udtSubsidizeWriteOffModel.ExactDOB), _
                                                udtDB.MakeInParam("@Scheme_Code", SubsidizeWriteOffModel.SchemeCodeDataType, SubsidizeWriteOffModel.SchemeCodeDataSize, udtSubsidizeWriteOffModel.SchemeCode), _
                                                udtDB.MakeInParam("@Scheme_Seq", SubsidizeWriteOffModel.SchemeSeqDataType, SubsidizeWriteOffModel.SchemeSeqDataSize, udtSubsidizeWriteOffModel.SchemeSeq), _
                                                udtDB.MakeInParam("@Subsidize_Code", SubsidizeWriteOffModel.SubsidizeCodeDataType, SubsidizeWriteOffModel.SubsidizeCodeDataSize, udtSubsidizeWriteOffModel.SubsidizeCode), _
                                                udtDB.MakeInParam("@Writeoff_Unit", SubsidizeWriteOffModel.WriteOffUnitDataType, SubsidizeWriteOffModel.WriteOffUnitDataSize, udtSubsidizeWriteOffModel.WriteOffUnit), _
                                                udtDB.MakeInParam("@WriteOff_Per_Unit_Value", SubsidizeWriteOffModel.WriteOffPerUnitValueDataType, SubsidizeWriteOffModel.WriteOffPerUnitValueDataSize, udtSubsidizeWriteOffModel.WriteOffPerUnitValue), _
                                                 udtDB.MakeInParam("@PValue_Ceiling", SubsidizeWriteOffModel.PValueCeilingDataType, SubsidizeWriteOffModel.PValueCeilingDataSize, IIf(udtSubsidizeWriteOffModel.PValueCeiling.HasValue = False, DBNull.Value, intPValueCeiling)), _
                                                udtDB.MakeInParam("@PValue_Total_Entitlement", SubsidizeWriteOffModel.PValueTotalEntitlementDataType, SubsidizeWriteOffModel.PValueTotalEntitlementDataSize, udtSubsidizeWriteOffModel.PValueTotalEntitlement), _
                                                udtDB.MakeInParam("@PValue_Season_Entitlement", SubsidizeWriteOffModel.PValueSeasonEntitlementDataType, SubsidizeWriteOffModel.PValueSeasonEntitlementDataSize, udtSubsidizeWriteOffModel.PValueSeasonEntitlement), _
                                                udtDB.MakeInParam("@PValue_Total_Used", SubsidizeWriteOffModel.PValueTotalUsedDataType, SubsidizeWriteOffModel.PValueTotalUsedDataSize, udtSubsidizeWriteOffModel.PValueTotalUsed), _
                                                udtDB.MakeInParam("@PValue_Season_Used", SubsidizeWriteOffModel.PValueSeasonUsedDataType, SubsidizeWriteOffModel.PValueSeasonUsedDataSize, udtSubsidizeWriteOffModel.PValueSeasonUsed), _
                                                udtDB.MakeInParam("@PValue_Available", SubsidizeWriteOffModel.PValueAvailableDataType, SubsidizeWriteOffModel.PValueAvailableDataSize, udtSubsidizeWriteOffModel.PValueAvailable), _
                                                udtDB.MakeInParam("@Create_dtm", SubsidizeWriteOffModel.CreateDtmDataType, SubsidizeWriteOffModel.CreateDtmDataSize, udtSubsidizeWriteOffModel.CreateDtm), _
                                                udtDB.MakeInParam("@Create_Reason", SubsidizeWriteOffModel.CreateReasonDataType, SubsidizeWriteOffModel.CreateReasonDataSize, udtSubsidizeWriteOffModel.CreateReason), _
                                                udtDB.MakeInParam("@PValue_Total_Refund", SubsidizeWriteOffModel.PValueTotalRefundDataType, SubsidizeWriteOffModel.PValueTotalRefundDataSize, udtSubsidizeWriteOffModel.PValueTotalRefund), _
                                                udtDB.MakeInParam("@PValue_Season_Refund", SubsidizeWriteOffModel.PValueSeasonRefundDataType, SubsidizeWriteOffModel.PValueSeasonRefundDataSize, udtSubsidizeWriteOffModel.PValueSeasonRefund)}
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                udtDB.RunProc("proc_eHASubsidizeWriteOff_add", params)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try
        End Sub

        Private Sub DeleteSubsidizeWriteOff(ByVal udtSubsidizeWriteOffModel As SubsidizeWriteOffModel, Optional ByVal udtDB As Database = Nothing)

            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim params() As SqlParameter = { _
                    udtDB.MakeInParam("@Doc_Code", SubsidizeWriteOffModel.DocCodeDataType, SubsidizeWriteOffModel.DocCodeDataSize, udtSubsidizeWriteOffModel.DocCode), _
                    udtDB.MakeInParam("@Doc_ID", SubsidizeWriteOffModel.DocIDDataType, SubsidizeWriteOffModel.DocIDDataSize, udtSubsidizeWriteOffModel.DocID), _
                    udtDB.MakeInParam("@DOB", SubsidizeWriteOffModel.DOBDataType, SubsidizeWriteOffModel.DOBDataSize, udtSubsidizeWriteOffModel.DOB), _
                    udtDB.MakeInParam("@Exact_DOB", SubsidizeWriteOffModel.ExactDOBDataType, SubsidizeWriteOffModel.ExactDOBDataSize, udtSubsidizeWriteOffModel.ExactDOB), _
                    udtDB.MakeInParam("@Scheme_Code", SubsidizeWriteOffModel.SchemeCodeDataType, SubsidizeWriteOffModel.SchemeCodeDataSize, udtSubsidizeWriteOffModel.SchemeCode), _
                    udtDB.MakeInParam("@Subsidize_Code", SubsidizeWriteOffModel.SubsidizeCodeDataType, SubsidizeWriteOffModel.SubsidizeCodeDataSize, udtSubsidizeWriteOffModel.SubsidizeCode), _
                    udtDB.MakeInParam("@Scheme_Seq", SubsidizeWriteOffModel.SchemeSeqDataType, SubsidizeWriteOffModel.SchemeSeqDataSize, udtSubsidizeWriteOffModel.SchemeSeq), _
                    udtDB.MakeInParam("@TSMP", SubsidizeWriteOffModel.TSMPDataType, SubsidizeWriteOffModel.TSMPDataSize, udtSubsidizeWriteOffModel.TSMP)}

                udtDB.RunProc("proc_eHASubsidizeWriteOff_del", params, dt)

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try
        End Sub

        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        Private Sub LogWriteOff(ByVal strActionKey As String, ByVal udtSubsidizeWriteOffModel As SubsidizeWriteOffModel, Optional ByVal udtDB As Database = Nothing)
            Dim intPValueCeiling As Integer

            If udtSubsidizeWriteOffModel.PValueCeiling.HasValue = True Then
                intPValueCeiling = CInt(udtSubsidizeWriteOffModel.PValueCeiling)
            End If

            If udtDB Is Nothing Then udtDB = New Database()

            Try
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                ' -----------------------------------------------------------------------------------------
                ' Add Refund
                Dim params() As SqlParameter = {udtDB.MakeInParam("@Action_Key", SqlDbType.VarChar, 20, strActionKey), _
                                                udtDB.MakeInParam("@Doc_Code", SubsidizeWriteOffModel.DocCodeDataType, SubsidizeWriteOffModel.DocCodeDataSize, udtSubsidizeWriteOffModel.DocCode), _
                                                udtDB.MakeInParam("@Doc_ID", SubsidizeWriteOffModel.DocIDDataType, SubsidizeWriteOffModel.DocIDDataSize, udtSubsidizeWriteOffModel.DocID), _
                                                udtDB.MakeInParam("@DOB", SubsidizeWriteOffModel.DOBDataType, SubsidizeWriteOffModel.DOBDataSize, udtSubsidizeWriteOffModel.DOB), _
                                                udtDB.MakeInParam("@Exact_DOB", SubsidizeWriteOffModel.ExactDOBDataType, SubsidizeWriteOffModel.ExactDOBDataSize, udtSubsidizeWriteOffModel.ExactDOB), _
                                                udtDB.MakeInParam("@Scheme_Code", SubsidizeWriteOffModel.SchemeCodeDataType, SubsidizeWriteOffModel.SchemeCodeDataSize, udtSubsidizeWriteOffModel.SchemeCode), _
                                                udtDB.MakeInParam("@Scheme_Seq", SubsidizeWriteOffModel.SchemeSeqDataType, SubsidizeWriteOffModel.SchemeSeqDataSize, udtSubsidizeWriteOffModel.SchemeSeq), _
                                                udtDB.MakeInParam("@Subsidize_Code", SubsidizeWriteOffModel.SubsidizeCodeDataType, SubsidizeWriteOffModel.SubsidizeCodeDataSize, udtSubsidizeWriteOffModel.SubsidizeCode), _
                                                udtDB.MakeInParam("@WriteOff_Unit", SubsidizeWriteOffModel.WriteOffUnitDataType, SubsidizeWriteOffModel.WriteOffUnitDataSize, udtSubsidizeWriteOffModel.WriteOffUnit), _
                                                udtDB.MakeInParam("@WriteOff_Per_Unit_Value", SubsidizeWriteOffModel.WriteOffPerUnitValueDataType, SubsidizeWriteOffModel.WriteOffPerUnitValueDataSize, udtSubsidizeWriteOffModel.WriteOffPerUnitValue), _
                                                udtDB.MakeInParam("@PValue_Ceiling", SubsidizeWriteOffModel.PValueCeilingDataType, SubsidizeWriteOffModel.PValueCeilingDataSize, IIf(udtSubsidizeWriteOffModel.PValueCeiling.HasValue, intPValueCeiling, DBNull.Value)), _
                                                udtDB.MakeInParam("@PValue_Total_Entitlement", SubsidizeWriteOffModel.PValueTotalEntitlementDataType, SubsidizeWriteOffModel.PValueTotalEntitlementDataSize, udtSubsidizeWriteOffModel.PValueTotalEntitlement), _
                                                udtDB.MakeInParam("@PValue_Season_Entitlement", SubsidizeWriteOffModel.PValueSeasonEntitlementDataType, SubsidizeWriteOffModel.PValueSeasonEntitlementDataSize, udtSubsidizeWriteOffModel.PValueSeasonEntitlement), _
                                                udtDB.MakeInParam("@PValue_Total_Used", SubsidizeWriteOffModel.PValueTotalUsedDataType, SubsidizeWriteOffModel.PValueTotalUsedDataSize, udtSubsidizeWriteOffModel.PValueTotalUsed), _
                                                udtDB.MakeInParam("@PValue_Season_Used", SubsidizeWriteOffModel.PValueSeasonUsedDataType, SubsidizeWriteOffModel.PValueSeasonUsedDataSize, udtSubsidizeWriteOffModel.PValueSeasonUsed), _
                                                udtDB.MakeInParam("@PValue_Available", SubsidizeWriteOffModel.PValueAvailableDataType, SubsidizeWriteOffModel.PValueAvailableDataSize, udtSubsidizeWriteOffModel.PValueAvailable), _
                                                udtDB.MakeInParam("@Create_dtm", SubsidizeWriteOffModel.CreateDtmDataType, SubsidizeWriteOffModel.CreateDtmDataSize, udtSubsidizeWriteOffModel.CreateDtm), _
                                                udtDB.MakeInParam("@Create_Reason", SubsidizeWriteOffModel.CreateReasonDataType, SubsidizeWriteOffModel.CreateReasonDataSize, udtSubsidizeWriteOffModel.CreateReason), _
                                                udtDB.MakeInParam("@PValue_Total_Refund", SubsidizeWriteOffModel.PValueTotalRefundDataType, SubsidizeWriteOffModel.PValueTotalRefundDataSize, udtSubsidizeWriteOffModel.PValueTotalRefund), _
                                                udtDB.MakeInParam("@PValue_Season_Refund", SubsidizeWriteOffModel.PValueSeasonRefundDataType, SubsidizeWriteOffModel.PValueSeasonRefundDataSize, udtSubsidizeWriteOffModel.PValueSeasonRefund)}
                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                udtDB.RunProc("proc_eHASubsidizeWriteOffLog_add", params)

            Catch ex As Exception

                Throw

            End Try
        End Sub
        ' CRE13-006 - HCVS Ceiling [End][Tommy L]

        Private Function IsFullWriteOffProfile(ByVal SubsidizeWriteOffList As SubsidizeWriteOffModelCollection, ByVal SubsidizeGroupClaimList As SubsidizeGroupClaimModelCollection, ByRef intSchemeSeqStartMissing As Integer) As Boolean

            Dim intCount As Integer
            Dim blnFullWriteOff As Boolean
            Dim intMissing As Integer
            Dim blnNotMatch As Boolean

            'intitate return value
            blnFullWriteOff = True
            intMissing = 0

            'Check full profile
            If SubsidizeWriteOffList.Count <= 0 Then
                'the eHealth account do NOT have any write off record
                blnFullWriteOff = False
                intMissing = 0

            ElseIf SubsidizeWriteOffList.Count <= SubsidizeGroupClaimList.Count Then
                'the eHealth account HAVE write off record
                For Each udtSubsidizeWriteOffModel As SubsidizeWriteOffModel In SubsidizeWriteOffList
                    'compare with scheme_seq of both list
                    If udtSubsidizeWriteOffModel.SchemeSeq <> SubsidizeGroupClaimList(intCount).SchemeSeq Then
                        'return false if scheme seq  not match
                        blnNotMatch = True
                        blnFullWriteOff = False
                        intMissing = SubsidizeGroupClaimList(intCount).SchemeSeq
                        Exit For
                    End If
                    intCount += 1
                Next

                If SubsidizeWriteOffList.Count < SubsidizeGroupClaimList.Count And blnNotMatch = False Then
                    blnFullWriteOff = False
                    intMissing = intCount + 1
                End If

            Else
                'CRE13-026 Repository [Start][Karl]
                ''exception handling: recalulate all write off record if write off list have more record than subsidize group list
                ' change exception handling , log record to system log and throw exception

                'blnFullWriteOff = False
                'intMissing = 0

                Dim intSchemeSeq As Integer = 0
                Dim strWOSchemeSeq As String = String.Empty
                Dim strSGCSchemeSeq As String = String.Empty
                Dim strCacheSchemeSeq As String = String.Empty
                Dim strWOCount As String = String.Empty
                Dim strSGCCount As String = String.Empty
                Dim intCacheCount As Integer = 0
                Dim udtCacheSubsidizeGroupClaimList As SubsidizeGroupClaimModelCollection
                Dim udtSchemeClaimBLL As New SchemeClaimBLL
                Dim strExceptionMsg As String

                If Not SubsidizeWriteOffList Is Nothing Then
                    For Each udtSubsidizeWriteOffModel As SubsidizeWriteOffModel In SubsidizeWriteOffList
                        strWOSchemeSeq += udtSubsidizeWriteOffModel.SchemeSeq & " "
                    Next
                    strWOCount = SubsidizeWriteOffList.Count.ToString
                Else
                    strWOCount = "0"
                End If

                If Not SubsidizeGroupClaimList Is Nothing Then
                    For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In SubsidizeGroupClaimList
                        strSGCSchemeSeq += udtSubsidizeGroupClaimModel.SchemeSeq & " "
                    Next
                    strSGCCount = SubsidizeGroupClaimList.Count.ToString
                Else
                    strSGCCount = "0"
                End If

                udtCacheSubsidizeGroupClaimList = CType(HttpRuntime.Cache(udtSchemeClaimBLL.CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupClaim), SubsidizeGroupClaimModelCollection)

                If Not udtCacheSubsidizeGroupClaimList Is Nothing Then
                    For Each udtCacheSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In udtCacheSubsidizeGroupClaimList
                        If udtCacheSubsidizeGroupClaimModel.SchemeCode = SchemeClaimModel.HCVS Then
                            strCacheSchemeSeq += udtCacheSubsidizeGroupClaimModel.SchemeSeq & " "
                            intCacheCount += 1
                        End If
                    Next
                End If

                strExceptionMsg = "Subsidize Write Off list is more than Subsidize Group Claim List: "
                strExceptionMsg += "[Input Write Off List] count: " & strWOCount & " including Scheme_Seq [" & strWOSchemeSeq.Trim & "];"
                strExceptionMsg += "[Input Group Claim List] count: " & strSGCCount & " including Scheme_Seq [" & strSGCSchemeSeq.Trim & "];"
                strExceptionMsg += "[Cache Group Claim List] count: " & intCacheCount.ToString & " including Scheme_Seq [" & strCacheSchemeSeq.Trim & "];"
                strExceptionMsg += "[System DateTime] : [" & Now.ToString("yyyy-MM-dd hh:mm:ss") & "];"
                Throw New Exception("SubsidizeWriteOffBLL: IsFullWriteOffProfile: " & strExceptionMsg)
                'CRE13-026 Repository [End][Karl]
            End If

            'Return values
            IsFullWriteOffProfile = blnFullWriteOff
            intSchemeSeqStartMissing = intMissing

        End Function

        Private Function IsIdenticalAccount(ByVal udtPersonalInfoModel As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, ByVal udtComparePersonalInfoModel As EHSAccount.EHSAccountModel.EHSPersonalInformationModel) As Boolean
            IsIdenticalAccount = False

            If udtPersonalInfoModel.DocCode = udtComparePersonalInfoModel.DocCode AndAlso _
                                    udtPersonalInfoModel.IdentityNum = udtComparePersonalInfoModel.IdentityNum AndAlso _
                                    udtPersonalInfoModel.DOB = udtComparePersonalInfoModel.DOB AndAlso _
                                  udtPersonalInfoModel.ExactDOB = udtComparePersonalInfoModel.ExactDOB Then
                IsIdenticalAccount = True
            End If

        End Function

        Private Function GetRelatedWriteOffAccount(ByVal strDocCode As String, ByVal strDocNo As String, Optional ByVal udtDB As Database = Nothing) As EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection
            Dim udtAccountBLL As New EHSAccount.EHSAccountBLL
            Dim udtFormatter As Formatter = New Formatter
            Dim udtPersonalInfoList As New EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection

            strDocNo = udtFormatter.formatDocumentIdentityNumber(strDocCode, strDocNo)

            'Get Accounts
            udtPersonalInfoList = udtAccountBLL.LoadRelatedWriteOffAccountByIdentity(strDocNo, strDocCode, udtDB)

            Return udtPersonalInfoList

        End Function

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function GetAllRelatedWriteOffAccount(ByVal strDocNo As String, Optional ByVal udtDB As Database = Nothing) As EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection
            Dim udtAccountBLL As New EHSAccount.EHSAccountBLL
            Dim udtFormatter As Formatter = New Formatter
            Dim udtPersonalInfoList As New EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection

            'Not to classify the DocCode but pass HKIC to use the format function
            strDocNo = udtFormatter.formatDocumentIdentityNumber(Component.DocType.DocTypeModel.DocTypeCode.HKIC, strDocNo)

            'Get Accounts
            udtPersonalInfoList = udtAccountBLL.LoadRelatedWriteOffAccountByIdentity(strDocNo, udtDB)

            Return udtPersonalInfoList

        End Function
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Handle write off mode : 
        '       1. MissingSeasonOnly  - only update the missing season
        '       2. SpecificSeason - Recalculate start from specific season (intSpecificStartSchemeSeq)
        Private Function HandleWriteOff(ByVal HandleMode As HandleWriteOffMode, ByVal intSpecificStartSchemeSeq As Nullable(Of Integer), _
                                        ByVal strDocCode As String, ByVal strDocID As String, _
                                        ByVal dtmDOB As DateTime, ByVal strExactDOB As String, _
                                        ByVal dtmDOD As Nullable(Of DateTime), ByVal strExactDOD As String, _
                                        ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, ByVal strCreateReason As String, _
                                        Optional ByVal udtDefaultTransactionDetailList As EHSTransaction.TransactionDetailModelCollection = Nothing, _
                                        Optional ByVal blnNotUpdateDB As Boolean = False, _
                                        Optional ByVal udtDB As Database = Nothing) As SubsidizeWriteOffModelCollection

            Dim udtSubsidizeWriteOffList As New SubsidizeWriteOffModelCollection()
            Dim udtWorkingSubsidizeWriteOffList As New SubsidizeWriteOffModelCollection()
            Dim udtFilteredSubsidizeWriteOffList As New SubsidizeWriteOffModelCollection()
            Dim udtLoggingSubsidizeWriteOffList As New SubsidizeWriteOffModelCollection()
            Dim udtDeleteSubsidizeWriteOffList As New SubsidizeWriteOffModelCollection()
            Dim udtCalculatedSubsidizeWriteOffList As New SubsidizeWriteOffModelCollection()
            Dim udtCalculatedSubsidizeWriteOffModel As SubsidizeWriteOffModel
            Dim udtSchemeClaimModel As SchemeClaimModel
            Dim udtSubsidizeGroupClaimList As SubsidizeGroupClaimModelCollection
            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim intStartMissing As Integer
            Dim blnFullProfile As Boolean
            Dim blnWorkingFullProfile As Boolean
            Dim intWorkingStartMissing As Integer

            Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL
            Dim udtTransactionDetailList As EHSTransaction.TransactionDetailModelCollection
            Dim udtFormatter As Formatter = New Formatter
            Dim strActionKey As String = (New Common.ComFunction.GeneralFunction).GetUniqueKey
            Dim udtResultModelCollection As New SubsidizeWriteOffModelCollection()
            Dim udtVoucherRefundBLL As New VoucherRefund.VoucherRefundBLL

            Dim intTotalEntitlement As Integer = 0
            Dim intTotalUsedVoucher As Integer = 0
            Dim intTotalWriteOff As Integer = 0
            Dim intTotalRefund As Integer = 0

            Dim udtGenFunct As New GeneralFunction()

            Dim udtEHSRelatedAccountPersonalInfoCollection As New EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection
            Dim udtPersonalInfoModel As New EHSAccount.EHSAccountModel.EHSPersonalInformationModel
            Dim udtPersonalInfoModelCollection As New EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection
            Dim blnIdenticalAccount As Boolean
            Dim blnLocalDB As Boolean

            If udtDB Is Nothing Then
                udtDB = New Database()
                blnLocalDB = True
            Else
                blnLocalDB = False
            End If

            strDocID = udtFormatter.formatDocumentIdentityNumber(strDocCode, strDocID)

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            'For other voucher scheme, treat it as HCVS in write off
            If strSchemeCode = SchemeClaimModel.HCVSCHN OrElse strSchemeCode = SchemeClaimModel.HCVSDHC Then
                strSchemeCode = SchemeClaimModel.HCVS
            End If
            ' CRE19-006 (DHC) [End][Winnie]


            udtSchemeClaimModel = udtSchemeClaimBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(strSchemeCode)

            '1 Initialize
            '1.1 Add personal information for target account
            udtPersonalInfoModel.IdentityNum = strDocID
            udtPersonalInfoModel.DocCode = strDocCode
            udtPersonalInfoModel.ExactDOB = strExactDOB
            udtPersonalInfoModel.DOB = dtmDOB
            udtPersonalInfoModel.ExactDOD = strExactDOD
            udtPersonalInfoModel.DOD = dtmDOD
            If Not udtPersonalInfoModel.DOD Is Nothing Then
                udtPersonalInfoModel.Deceased = True
            Else
                udtPersonalInfoModel.Deceased = False
            End If

            udtPersonalInfoModelCollection.Add(udtPersonalInfoModel)

            '1.2 Get subsidizegrouplist by filter out future season
            udtSubsidizeGroupClaimList = udtSchemeClaimModel.SubsidizeGroupClaimList.FilterbyRange(udtSchemeClaimModel.ClaimPeriodFrom, Now).OrderBySchemeSeqASC

            '1.3 get current write off record
            udtSubsidizeWriteOffList = Me.GetAllSubsidizeWriteOff(strDocCode, strDocID, dtmDOB, strExactDOB, strSchemeCode, strSubsidizeCode, udtDB)

            '2. handle eHA accounts
            '2.2. Check full profile for target account
            blnFullProfile = IsFullWriteOffProfile(udtSubsidizeWriteOffList, udtSubsidizeGroupClaimList, intStartMissing)

            If blnFullProfile = False Or HandleMode = HandleWriteOffMode.SpecificSeason Then

                'Get Transaction Records
                If udtDefaultTransactionDetailList Is Nothing Then
                    'CRE13-019-02 Extend HCVS to China [Start][Karl]
                    udtTransactionDetailList = udtEHSTransactionBLL.getTransactionDetailVoucherBySubsidizeCodeOnly(udtPersonalInfoModel.DocCode, udtPersonalInfoModel.IdentityNum, strSubsidizeCode, udtDB)
                    'CRE13-019-02 Extend HCVS to China [End][Karl]
                Else
                    udtTransactionDetailList = udtDefaultTransactionDetailList
                End If

                'Profile got missing season /  write off need to recalculate start from specific season

                If HandleMode = HandleWriteOffMode.SpecificSeason Then
                    If intSpecificStartSchemeSeq.HasValue = False Then
                        'no specific season, recal all
                        intStartMissing = 0
                    Else
                        'if specified recal season before missing season, recal start from specifc season
                        If blnFullProfile Or CInt(intSpecificStartSchemeSeq) < intStartMissing Then
                            intStartMissing = intSpecificStartSchemeSeq
                        End If
                    End If
                End If

                '2.3 Add related account if necessary
                Select Case HandleMode
                    Case HandleWriteOffMode.MissingSeasonOnly
                        ''update related account when it is creating claims and the writeoff profile is incomplete
                        If blnFullProfile = False AndAlso strCreateReason = eHASubsidizeWriteOff_CreateReason.TxCreation Then
                            udtEHSRelatedAccountPersonalInfoCollection = GetRelatedWriteOffAccount(udtPersonalInfoModel.DocCode, udtPersonalInfoModel.IdentityNum, udtDB)
                        End If

                    Case HandleWriteOffMode.SpecificSeason
                        ''must update related account if recalculate from specific season
                        udtEHSRelatedAccountPersonalInfoCollection = GetRelatedWriteOffAccount(udtPersonalInfoModel.DocCode, udtPersonalInfoModel.IdentityNum, udtDB)

                    Case Else
                        Throw New Exception("SubsidizeWriteOffBLL: Unhandled HandleWriteOffMode in [HandleWriteOff] function ")

                End Select

                'only add if the account is not identical (in term of doc code , doc no, dob, exactdob)
                For Each udtRelatedPersonalInfoModel As EHSAccount.EHSAccountModel.EHSPersonalInformationModel In udtEHSRelatedAccountPersonalInfoCollection
                    blnIdenticalAccount = False

                    For Each udtTotalPersonalInfoModel As EHSAccount.EHSAccountModel.EHSPersonalInformationModel In udtPersonalInfoModelCollection
                        If IsIdenticalAccount(udtRelatedPersonalInfoModel, udtTotalPersonalInfoModel) Then
                            blnIdenticalAccount = True
                            Exit For
                        End If
                    Next

                    If blnIdenticalAccount = False Then
                        udtPersonalInfoModelCollection.Add(udtRelatedPersonalInfoModel)
                    End If
                Next

                '3. Loop thru target account + all related account (if any)
                For Each udtWorkingPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel In udtPersonalInfoModelCollection
                    udtWorkingSubsidizeWriteOffList = Nothing
                    blnWorkingFullProfile = False

                    intTotalEntitlement = 0
                    intTotalUsedVoucher = 0
                    intTotalWriteOff = 0
                    intTotalRefund = 0

                    If String.IsNullOrEmpty(udtWorkingPersonalInfo.DocCode) = False AndAlso String.IsNullOrEmpty(udtWorkingPersonalInfo.IdentityNum) = False Then

                        'if working person is the target account, use the subsidize write off information before
                        If IsIdenticalAccount(udtPersonalInfoModel, udtWorkingPersonalInfo) = True Then
                            udtWorkingSubsidizeWriteOffList = udtSubsidizeWriteOffList
                            blnWorkingFullProfile = blnFullProfile
                            intWorkingStartMissing = intStartMissing
                        Else
                            '3.1 Get working account writeoff record 
                            udtWorkingSubsidizeWriteOffList = Me.GetAllSubsidizeWriteOff(udtWorkingPersonalInfo.DocCode, udtWorkingPersonalInfo.IdentityNum, udtWorkingPersonalInfo.DOB, udtWorkingPersonalInfo.ExactDOB, _
                                                                                            strSchemeCode, strSubsidizeCode, udtDB)

                            '3.2 check the working account is in full write off profile or not
                            blnWorkingFullProfile = IsFullWriteOffProfile(udtWorkingSubsidizeWriteOffList, udtSubsidizeGroupClaimList, intWorkingStartMissing)

                            If HandleMode = HandleWriteOffMode.SpecificSeason Then
                                If intSpecificStartSchemeSeq.HasValue = False Then
                                    'no specific season, recal all
                                    intWorkingStartMissing = 0
                                Else
                                    'if specified recal season before missing season, recal start from specifc season
                                    If blnWorkingFullProfile Or CInt(intSpecificStartSchemeSeq) < intWorkingStartMissing Then
                                        intWorkingStartMissing = intSpecificStartSchemeSeq
                                    End If
                                End If
                            End If
                        End If

                        '3.3 start calculation from start missing season
                        If udtWorkingSubsidizeWriteOffList.Count > 0 Then
                            udtFilteredSubsidizeWriteOffList = udtWorkingSubsidizeWriteOffList.FilterBySchemeSeq(intWorkingStartMissing)

                            '3.3.1 get total values for season before missing season for later use
                            For Each udtInitiatingSubsidizeWriteOffModel As SubsidizeWriteOffModel In udtFilteredSubsidizeWriteOffList

                                '------------------------------
                                '*** Total Entitlement ***
                                '------------------------------
                                For Each udtinitiatingSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In udtSubsidizeGroupClaimList
                                    If udtinitiatingSubsidizeGroupClaimModel.SchemeSeq = udtInitiatingSubsidizeWriteOffModel.SchemeSeq Then
                                        'intTotalEntitlement += udtEHSTransactionBLL.getTotalGrantVoucher(strSchemeCode, strSubsidizeCode, udtWorkingPersonalInfo.DOB, udtWorkingPersonalInfo.ExactDOB, udtinitiatingSubsidizeGroupClaimModel.ClaimPeriodFrom, udtinitiatingSubsidizeGroupClaimModel.SchemeSeq)
                                        intTotalEntitlement += udtEHSTransactionBLL.getTotalGrantVoucher(GetTotalEntitlement.ByLastDayOfClaimPeriod, strSchemeCode, strSubsidizeCode, udtWorkingPersonalInfo, udtinitiatingSubsidizeGroupClaimModel.ClaimPeriodFrom, udtinitiatingSubsidizeGroupClaimModel.SchemeSeq).GetTotalEntitlement

                                        Exit For
                                    End If
                                Next

                                '------------------------------
                                '*** Total Used Voucher ***
                                '------------------------------
                                For Each udtInitiatingTransDetailModel As EHSTransaction.TransactionDetailModel In udtTransactionDetailList
                                    If udtInitiatingTransDetailModel.SchemeSeq = udtInitiatingSubsidizeWriteOffModel.SchemeSeq Then
                                        If udtInitiatingTransDetailModel.Unit.HasValue = True Then
                                            intTotalUsedVoucher += udtInitiatingTransDetailModel.Unit.Value
                                        End If
                                    End If
                                Next

                                '------------------------------
                                '*** Total Write Off ***
                                '------------------------------
                                intTotalWriteOff += udtInitiatingSubsidizeWriteOffModel.WriteOffUnit

                                '------------------------------
                                '*** Total Refund ***
                                '------------------------------
                                For Each udtinitiatingSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In udtSubsidizeGroupClaimList
                                    If udtinitiatingSubsidizeGroupClaimModel.SchemeSeq = udtInitiatingSubsidizeWriteOffModel.SchemeSeq Then
                                        intTotalRefund += udtVoucherRefundBLL.getTotalRefundedVoucher(strDocID, udtinitiatingSubsidizeGroupClaimModel.ClaimPeriodTo, udtinitiatingSubsidizeGroupClaimModel.SchemeSeq, udtDB).GetTotalRefund

                                        Exit For
                                    End If
                                Next


                                udtLoggingSubsidizeWriteOffList.Add(udtInitiatingSubsidizeWriteOffModel)
                            Next

                        End If

                        '3.3.2 start calculation
                        Dim udtVoucherDetailListForRefund As VoucherDetailModelCollection = Nothing

                        For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In udtSubsidizeGroupClaimList

                            Dim intCeilingNo As Nullable(Of Integer) = Nothing
                            Dim intSeasonEntitlement As Integer = 0
                            Dim intSeasonUsedVoucher As Integer = 0
                            Dim intSeasonAvailableVoucher As Integer = 0
                            Dim intSeasonRefund As Integer = 0
                            Dim intWriteOff As Integer = 0
                            Dim intPerUnitValue As Integer = 0
                            Dim dtmLogicalServiceDate As DateTime = udtSubsidizeGroupClaimModel.ClaimPeriodFrom
                            Dim dtmCreateDate As DateTime = udtGenFunct.GetSystemDateTime()


                            If udtSubsidizeGroupClaimModel.SchemeSeq >= intWorkingStartMissing Then
                                'start calculation from start missing season

                                intPerUnitValue = udtSubsidizeGroupClaimModel.SubsidizeFeeList.Filter(SubsidizeFeeModel.SubsidizeFeeTypeClass.SubsidizeFeeTypeVoucher, dtmLogicalServiceDate).SubsidizeFee.ToString()

                                '------------------------------------
                                '*** Season & Total Entitlement ***
                                '------------------------------------
                                'intSeasonEntitlement = udtEHSTransactionBLL.getTotalGrantVoucher(strSchemeCode, strSubsidizeCode, udtWorkingPersonalInfo.DOB, udtWorkingPersonalInfo.ExactDOB, dtmLogicalServiceDate, udtSubsidizeGroupClaimModel.SchemeSeq)
                                intSeasonEntitlement = udtEHSTransactionBLL.getTotalGrantVoucher(GetTotalEntitlement.ByLastDayOfClaimPeriod, strSchemeCode, strSubsidizeCode, udtWorkingPersonalInfo, dtmLogicalServiceDate, udtSubsidizeGroupClaimModel.SchemeSeq).GetTotalEntitlement

                                intTotalEntitlement += intSeasonEntitlement

                                '------------------------------------
                                '*** Season Write Off ***
                                '------------------------------------
                                'Calcalate write off first before load the current season used voucher and refund
                                If udtSubsidizeGroupClaimModel.NumSubsidizeCeiling.HasValue = True Then
                                    intCeilingNo = CInt(udtSubsidizeGroupClaimModel.NumSubsidizeCeiling)
                                    If (intTotalEntitlement - intTotalUsedVoucher - intTotalWriteOff + intTotalRefund) > CInt(intCeilingNo) Then
                                        intWriteOff = Math.Max(intTotalEntitlement - intTotalUsedVoucher - intTotalWriteOff + intTotalRefund - CInt(intCeilingNo), 0)
                                        intTotalWriteOff += intWriteOff
                                    End If
                                Else
                                    intCeilingNo = Nothing
                                End If

                                '------------------------------------
                                '*** Season & Total Used Voucher ***
                                '------------------------------------
                                For Each udtCalculatingTransDetailModel As EHSTransaction.TransactionDetailModel In udtTransactionDetailList
                                    If udtCalculatingTransDetailModel.SchemeSeq = udtSubsidizeGroupClaimModel.SchemeSeq Then
                                        If udtCalculatingTransDetailModel.Unit.HasValue = True Then
                                            intSeasonUsedVoucher += udtCalculatingTransDetailModel.Unit.Value
                                        End If
                                    End If
                                Next

                                intTotalUsedVoucher += intSeasonUsedVoucher

                                '------------------------------------
                                '*** Season & Total Refund ***
                                '------------------------------------
                                If udtVoucherDetailListForRefund Is Nothing Then
                                    udtVoucherDetailListForRefund = udtVoucherRefundBLL.getTotalRefundedVoucher(strDocID, dtmCreateDate, Nothing, udtDB)
                                End If

                                Dim udtVoucherDetailForRefund As VoucherDetailModel = udtVoucherDetailListForRefund.Find(udtSubsidizeGroupClaimModel.SchemeSeq)

                                If Not udtVoucherDetailForRefund Is Nothing Then
                                    intSeasonRefund = udtVoucherDetailListForRefund.Find(udtSubsidizeGroupClaimModel.SchemeSeq).Refund
                                Else
                                    intSeasonRefund = 0
                                End If

                                'intSeasonRefund = udtVoucherRefundBLL.getTotalRefundedVoucher(strDocID, udtSubsidizeGroupClaimModel.ClaimPeriodTo, udtSubsidizeGroupClaimModel.SchemeSeq, udtDB).GetTotalRefund

                                intTotalRefund += intSeasonRefund

                                '------------------------------------
                                '*** Season Available Voucher ***
                                '------------------------------------
                                intSeasonAvailableVoucher = intTotalEntitlement - intTotalUsedVoucher - intTotalWriteOff + intTotalRefund

                                ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                                ' -----------------------------------------------------------------------------------------
                                udtCalculatedSubsidizeWriteOffModel = (New SubsidizeWriteOffModel(udtWorkingPersonalInfo.DocCode, udtWorkingPersonalInfo.IdentityNum, udtWorkingPersonalInfo.DOB, udtWorkingPersonalInfo.ExactDOB, udtSubsidizeGroupClaimModel.SchemeCode, _
                                udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeCode, intWriteOff, intPerUnitValue, intCeilingNo, _
                                intTotalEntitlement, intSeasonEntitlement, intTotalUsedVoucher, intSeasonUsedVoucher, intSeasonAvailableVoucher, dtmCreateDate, _
                                strCreateReason, intTotalRefund, intSeasonRefund))
                                ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                                udtCalculatedSubsidizeWriteOffList.Add(udtCalculatedSubsidizeWriteOffModel)
                                udtLoggingSubsidizeWriteOffList.Add(udtCalculatedSubsidizeWriteOffModel)

                            End If
                        Next

                        '3.3.3 insert the season which required to re-calculate to the delete list
                        For Each udtDeleteSubsidizeWriteOffModel As SubsidizeWriteOffModel In udtWorkingSubsidizeWriteOffList
                            If udtDeleteSubsidizeWriteOffModel.SchemeSeq >= intWorkingStartMissing Then
                                udtDeleteSubsidizeWriteOffList.Add(udtDeleteSubsidizeWriteOffModel)
                            End If
                        Next
                    End If
                Next

                '3.4 if this is enquiry case, it may hv case of enquiry for a non-existed account. Check if the target account does not exist, do not update the DB
                If strCreateReason = eHASubsidizeWriteOff_CreateReason.TxEnquiry Then
                    If blnNotUpdateDB = False Then
                        If blnFullProfile = True Then
                            'if full profile, no need update db
                            blnNotUpdateDB = True
                        Else
                            'try to find out identical account
                            Dim blnCheckIdenticalAccount As Boolean = False
                            'Dim udtCheckEHSRelatedAccountCollection As New EHSAccount.EHSAccountModelCollection
                            Dim udtCheckPersonalInfoModel As New EHSAccount.EHSAccountModel.EHSPersonalInformationModel
                            Dim udtCheckPersonalInfoModelCollection As New EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection
                            Dim udtRelatedAccountPersonalInfoModelCollection As New EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection

                            udtCheckPersonalInfoModel.IdentityNum = strDocID
                            udtCheckPersonalInfoModel.DocCode = strDocCode
                            udtCheckPersonalInfoModel.ExactDOB = strExactDOB
                            udtCheckPersonalInfoModel.DOB = dtmDOB
                            udtCheckPersonalInfoModelCollection.Add(udtCheckPersonalInfoModel)

                            udtRelatedAccountPersonalInfoModelCollection = GetRelatedWriteOffAccount(strDocCode, strDocID, udtDB)

                            'check is the account with same doc code doc no dob exists  
                            If udtRelatedAccountPersonalInfoModelCollection.Count = 0 Then
                                blnNotUpdateDB = True
                            End If

                            For Each udtCheckRelatedPersonalInfoModel As EHSAccount.EHSAccountModel.EHSPersonalInformationModel In udtRelatedAccountPersonalInfoModelCollection

                                For Each udtCheckTotalPersonalInfoModel As EHSAccount.EHSAccountModel.EHSPersonalInformationModel In udtCheckPersonalInfoModelCollection
                                    If IsIdenticalAccount(udtCheckRelatedPersonalInfoModel, udtCheckTotalPersonalInfoModel) AndAlso blnCheckIdenticalAccount = False Then
                                        blnCheckIdenticalAccount = True
                                        Exit For
                                    End If
                                Next

                                'if there is no such account, only calculate but not write WriteOff record into DB
                                If blnCheckIdenticalAccount = False Then
                                    blnNotUpdateDB = True
                                Else
                                    blnNotUpdateDB = False
                                End If
                            Next
                        End If
                    End If
                End If

                '4 Insert WriteOff Record
                If blnNotUpdateDB = False Then
                    Try
                        If blnLocalDB = True Then
                            udtDB.BeginTransaction()
                        End If

                        '4.1 Delete WriteOff record for all re-calculated season
                        For Each udtDeleteSubsidizeWriteOffModel As SubsidizeWriteOffModel In udtDeleteSubsidizeWriteOffList
                            Call DeleteSubsidizeWriteOff(udtDeleteSubsidizeWriteOffModel, udtDB)
                        Next

                        '4.2 Insert WriteOff
                        For Each udtInsertSubsidizeWriteOffModel As SubsidizeWriteOffModel In udtCalculatedSubsidizeWriteOffList
                            Call InsertWriteOff(udtInsertSubsidizeWriteOffModel, udtDB)
                        Next

                        '4.3 Logging Full set
                        For Each udtLoggingSubsidizeWriteOffModel As SubsidizeWriteOffModel In udtLoggingSubsidizeWriteOffList
                            Call LogWriteOff(strActionKey, udtLoggingSubsidizeWriteOffModel, udtDB)
                        Next

                        If blnLocalDB = True Then
                            udtDB.CommitTransaction()
                        End If

                    Catch ex As Exception
                        If blnLocalDB = True Then
                            udtDB.RollBackTranscation()
                        End If
                        Throw
                    End Try
                End If

                '5. Prepare Result
                udtResultModelCollection = udtLoggingSubsidizeWriteOffList
            Else
                'Just return write off record if the write off profile is full and no recalculation is required
                udtResultModelCollection = udtSubsidizeWriteOffList
            End If

            'Return result
            Return udtResultModelCollection

        End Function
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        Private Sub RemoveWriteOff(ByVal strDocCode As String, ByVal strDocID As String, ByVal dtmDOB As DateTime, ByVal strExactDOB As String, _
        ByVal strCreateReason As String, Optional ByVal udtDB As Database = Nothing)

            Dim udtSubsidizeWriteOffList As New SubsidizeWriteOffModelCollection()
            Dim udtLogSubsidzieWriteOffModel As SubsidizeWriteOffModel
            Dim strActionKey As String = (New Common.ComFunction.GeneralFunction).GetUniqueKey
            Dim udtGenFunct As New GeneralFunction()
            Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()
            Dim blnLocalDB As Boolean

            If udtDB Is Nothing Then
                udtDB = New Database()
                blnLocalDB = True
            Else
                blnLocalDB = False
            End If


            '1 get current write off record
            udtSubsidizeWriteOffList = Me.GetAllSubsidizeWriteOff(strDocCode, strDocID, dtmDOB, strExactDOB, Nothing, Nothing, udtDB)

            If udtSubsidizeWriteOffList.Count > 0 Then
                Try
                    If blnLocalDB = True Then
                        udtDB.BeginTransaction()
                    End If

                    For Each udtSubsidzieWriteOffModel As SubsidizeWriteOffModel In udtSubsidizeWriteOffList
                        'delete write off
                        Call DeleteSubsidizeWriteOff(udtSubsidzieWriteOffModel, udtDB)
                        'write log
                        udtLogSubsidzieWriteOffModel = udtSubsidzieWriteOffModel

                        udtLogSubsidzieWriteOffModel.CreateReason = strCreateReason
                        udtLogSubsidzieWriteOffModel.CreateDtm = dtmCurrentDate

                        Call LogWriteOff(strActionKey, udtLogSubsidzieWriteOffModel, udtDB)
                    Next

                    If blnLocalDB = True Then
                        udtDB.CommitTransaction()
                    End If

                Catch ex As Exception
                    If blnLocalDB = True Then
                        udtDB.RollBackTranscation()
                    End If
                    Throw
                End Try
            End If

        End Sub

        Private Function MassagePersonalInformationCollectionForWriteOff(ByVal udtPersonalInformationModelCollection As EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection) As EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection
            Dim intCount As Integer = 0
            Dim udtWorkingPersonalInformationModelCollection As New EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection

            If udtPersonalInformationModelCollection.Count > 0 Then
                For intCount = 0 To udtPersonalInformationModelCollection.Count - 1

                    Try
                        If TypeOf udtPersonalInformationModelCollection(intCount) Is EHSAccount.EHSAccountModel.EHSPersonalInformationModel Then
                            udtWorkingPersonalInformationModelCollection.Add(udtPersonalInformationModelCollection(intCount))
                        End If

                    Catch ex As Exception
                    End Try
                Next
            End If

            Return udtWorkingPersonalInformationModelCollection

        End Function

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve Write Off Amount for Specified Period
        ''' </summary>
        ''' <param name="udtSubsidizeWriteOffModelCollection"></param>
        ''' <param name="intStartSchemeSeq"></param>
        ''' <param name="intEndSchemeSeq"></param>        
        ''' <remarks></remarks>
        Public Function GetWriteOffAmount(ByVal udtSubsidizeWriteOffModelCollection As SubsidizeWriteOffModelCollection, _
                                          ByVal intStartSchemeSeq As Nullable(Of Integer), ByVal intEndSchemeSeq As Nullable(Of Integer)) As VoucherDetailModelCollection

            Dim udtVoucherDetailList As New VoucherDetailModelCollection

            'Dim intWriteOff As Integer = 0

            For Each udtSubsidizeWriteOffModel As SubsidizeWriteOffModel In udtSubsidizeWriteOffModelCollection
                If (intStartSchemeSeq.HasValue = False _
                    Or _
                    (intStartSchemeSeq.HasValue = True AndAlso CInt(intStartSchemeSeq) <= udtSubsidizeWriteOffModel.SchemeSeq)) _
                    AndAlso _
                    (intEndSchemeSeq.HasValue = False Or (intEndSchemeSeq.HasValue = True AndAlso udtSubsidizeWriteOffModel.SchemeSeq <= CInt(intEndSchemeSeq)) _
                   ) Then

                    'intWriteOff += udtSubsidizeWriteOffModel.WriteOffUnit

                    Dim udtVoucherDetail As New VoucherDetailModel
                    udtVoucherDetail.SchemeSeq = udtSubsidizeWriteOffModel.SchemeSeq
                    udtVoucherDetail.WriteOff = udtSubsidizeWriteOffModel.WriteOffUnit

                    udtVoucherDetailList.Add(udtVoucherDetail)

                End If
            Next

            Return udtVoucherDetailList
        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve Full Write Off List
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strDocID"></param>
        ''' <param name="dtmDOB"></param>        
        ''' <param name="strExactDOB"></param>     
        ''' <param name="dtmDOD"></param>        
        ''' <param name="strExactDOD"></param>       
        ''' <param name="strSchemeCode"></param>        
        ''' <param name="strSubsidizeCode"></param>        
        ''' <param name="strCreateReason"></param>                
        ''' <param name="udtDB"></param>     
        ''' <remarks></remarks>
        Public Function GetSubsidizeWriteOffList(ByVal strDocCode As String, ByVal strDocID As String, _
                                                 ByVal dtmDOB As DateTime, ByVal strExactDOB As String, _
                                                 ByVal dtmDOD As Nullable(Of DateTime), ByVal strExactDOD As String, _
                                                 ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, _
                                                 ByVal strCreateReason As String, Optional ByVal udtDB As Database = Nothing) As SubsidizeWriteOffModelCollection

            Dim udtSubsidizeWriteOffList As New SubsidizeWriteOffModelCollection()
            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtSchemeClaimModel As SchemeClaimModel
            Dim blnNotUpdateDB As Boolean = False
            Dim blnIdenticalAccount As Boolean = False
            Dim udtEHSRelatedAccountCollection As New EHSAccount.EHSAccountModelCollection
            Dim udtPersonalInfoModel As New EHSAccount.EHSAccountModel.EHSPersonalInformationModel
            Dim udtPersonalInfoModelCollection As New EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection

            udtSchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode)

            If Not udtSchemeClaimModel Is Nothing Then

                Select Case udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType
                    Case SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher
                        udtSubsidizeWriteOffList = HandleWriteOff(HandleWriteOffMode.MissingSeasonOnly, Nothing, _
                                                                  strDocCode, strDocID, dtmDOB, strExactDOB, dtmDOD, strExactDOD, _
                                                                  strSchemeCode, strSubsidizeCode, strCreateReason, Nothing, blnNotUpdateDB, udtDB)

                    Case Else
                        'only handling voucher right now

                End Select
            End If

            Return udtSubsidizeWriteOffList

        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        ' CRE18-021 (Voucher balance Enquiry show forfeited) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        ''' <summary>
        ''' Retrieve Total Write Off  Amount
        ''' </summary>
        ''' <param name="strDocCode"></param>
        ''' <param name="strDocID"></param>
        ''' <param name="dtmDOB"></param>        
        ''' <param name="strExactDOB"></param>     
        ''' <param name="dtmDOD"></param>        
        ''' <param name="strExactDOD"></param>   
        ''' <param name="strSchemeCode"></param>        
        ''' <param name="strSubsidizeCode"></param>        
        ''' <param name="strCreateReason"></param>                
        ''' <param name="udtDB"></param>     
        ''' <remarks></remarks>
        Public Function GetTotalWriteOff(ByVal strDocCode As String, ByVal strDocID As String, _
                                         ByVal dtmDOB As DateTime, ByVal strExactDOB As String, _
                                         ByVal dtmDOD As Nullable(Of DateTime), ByVal strExactDOD As String, _
                                         ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, _
                                         ByVal strCreateReason As String, Optional ByVal udtDB As Database = Nothing) As VoucherDetailModelCollection

            Dim udtSubsidizeWriteOffList As New SubsidizeWriteOffModelCollection()
            Dim udtSchemeClaimBLL As New SchemeClaimBLL

            Dim udtSchemeClaimModel As SchemeClaimModel
            udtSchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode)

            If Not udtSchemeClaimModel Is Nothing Then
                Select Case udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType
                    Case SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher
                        udtSubsidizeWriteOffList = GetSubsidizeWriteOffList(strDocCode, strDocID, dtmDOB, strExactDOB, dtmDOD, strExactDOD, _
                                                                            strSchemeCode, strSubsidizeCode, strCreateReason, udtDB)

                        Return GetWriteOffAmount(udtSubsidizeWriteOffList, Nothing, Nothing)

                    Case Else
                        'only handling voucher right now

                End Select
            End If
        End Function
        ' CRE18-021 (Voucher balance Enquiry show forfeited) [End][Chris YIM]

        ''' <summary>
        ''' Handle Write Off for Newly Created Account
        ''' </summary>
        ''' <param name="udtPersonalInformationModelCollection"></param>
        ''' <param name="strCreateReason"></param>
        ''' <param name="udtDB"></param>        
        ''' <remarks></remarks>
        Public Sub HandleNewAccountWriteOff(ByVal udtPersonalInformationModelCollection As EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection, _
      ByVal strCreateReason As String, Optional ByVal udtDB As Database = Nothing)

            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection
            Dim strCompletedSubsidizeCode As String = Nothing
            Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL
            Dim udtWorkingPersonalInformationModelCollection As EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection
            Dim udtClaimRulesBLL As New Common.Component.ClaimRules.ClaimRulesBLL

            'Make sure no invalid model inside the collection
            udtWorkingPersonalInformationModelCollection = MassagePersonalInformationCollectionForWriteOff(udtPersonalInformationModelCollection)

            udtSchemeClaimModelCollection = udtSchemeClaimBLL.getAllSchemeClaim_WithSubsidizeGroup

            If udtWorkingPersonalInformationModelCollection.Count > 0 Then
                For Each udtPersonalInformationModel As EHSAccount.EHSAccountModel.EHSPersonalInformationModel In udtWorkingPersonalInformationModelCollection

                    'For EC age registration case
                    If udtPersonalInformationModel.ExactDOB = EHSAccount.EHSAccountModel.ExactDOBClass.AgeAndRegistration AndAlso _
                        udtPersonalInformationModel.DOB = Date.MinValue Then

                        If udtPersonalInformationModel.ECDateOfRegistration.HasValue = True AndAlso udtPersonalInformationModel.ECAge.HasValue = True Then
                            udtPersonalInformationModel.DOB = CType(udtPersonalInformationModel.ECDateOfRegistration, DateTime).AddYears(-CType(udtPersonalInformationModel.ECAge, Integer))
                        Else
                            Throw New Exception("SubsidizeWriteOffBLL: No ECDateOfRegistration or ECAge in EC age registration case ")
                        End If
                    End If

                    For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollection

                        'only handling voucher right now
                        Select Case udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType
                            Case SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher

                                For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList

                                    'Each subsidize code only need to do once
                                    If udtSubsidizeGroupClaimModel.SubsidizeCode <> strCompletedSubsidizeCode Then

                                        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                                        ' -----------------------------------------------------------------------------------------
                                        'only create new account write off if the account is eligible for HCVS
                                        If udtClaimRulesBLL.CheckEligibilityPerSubsidize(udtSubsidizeGroupClaimModel, udtPersonalInformationModel, _
                                                                                udtSubsidizeGroupClaimModel.ClaimPeriodTo.AddDays(-1), Nothing, True).IsEligible Then
                                            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                                            'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                                            '-----------------------------------------------------------------------------------------
                                            Call HandleWriteOff(HandleWriteOffMode.MissingSeasonOnly, Nothing, _
                                                                udtPersonalInformationModel.DocCode, udtPersonalInformationModel.IdentityNum, _
                                                                udtPersonalInformationModel.DOB, udtPersonalInformationModel.ExactDOB, _
                                                                udtPersonalInformationModel.DOD, udtPersonalInformationModel.ExactDOD, _
                                                                udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SubsidizeCode, _
                                                                strCreateReason, Nothing, False, udtDB)
                                            'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                                            strCompletedSubsidizeCode = udtSubsidizeGroupClaimModel.SubsidizeCode

                                        End If
                                    End If
                                Next

                            Case Else
                                'only handling voucher right now

                        End Select
                    Next
                Next
            End If
        End Sub

        ''' <summary>
        ''' Delete all subset of write off records for specified account
        ''' </summary>
        ''' <param name="udtPersonalInformationModelCollection"></param>
        ''' <param name="strCreateReason"></param>
        ''' <param name="udtDB"></param>        
        ''' <remarks></remarks>
        Public Sub DeleteAccountWriteOff(ByVal udtPersonalInformationModelCollection As EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection, _
             ByVal strCreateReason As String, Optional ByVal udtDB As Database = Nothing)

            Dim udtWorkingPersonalInformationModelCollection As EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection
            'Make sure no invalid model inside the collection
            udtWorkingPersonalInformationModelCollection = MassagePersonalInformationCollectionForWriteOff(udtPersonalInformationModelCollection)

            If udtWorkingPersonalInformationModelCollection.Count > 0 Then
                For Each udtPersonalInformationModel As EHSAccount.EHSAccountModel.EHSPersonalInformationModel In udtWorkingPersonalInformationModelCollection

                    'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    If IsExistWriteOffAccount(udtPersonalInformationModel, udtDB) Then
                        Exit Sub
                    End If
                    'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                    'continue if no identical account found
                    Call RemoveWriteOff(udtPersonalInformationModel.DocCode, udtPersonalInformationModel.IdentityNum, udtPersonalInformationModel.DOB, udtPersonalInformationModel.ExactDOB, _
                    strCreateReason, udtDB)
                Next
            End If
        End Sub

        ''' <summary>
        ''' Calculate and update write off value, start from specified service date
        ''' </summary>
        ''' <param name="dtmServiceDate"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="strDocID"></param>
        ''' <param name="dtmDOB"></param>        
        ''' <param name="strExactDOB"></param>        
        ''' <param name="strSchemeCode"></param>        
        ''' <param name="strSubsidizeCode"></param>        
        ''' <param name="strCreateReason"></param>                
        ''' <param name="udtDB"></param>     
        ''' <remarks></remarks>
        Public Sub UpdateWriteOff(ByVal dtmServiceDate As DateTime, ByVal strDocCode As String, ByVal strDocID As String, _
                                  ByVal dtmDOB As DateTime, ByVal strExactDOB As String, _
                                  ByVal dtmDOD As Nullable(Of DateTime), ByVal strExactDOD As String, _
                                  ByVal strSchemeCode As String, ByVal strSubsidizeCode As String, _
                                  ByVal strCreateReason As String, Optional ByVal udtDB As Database = Nothing)

            Dim intServiceDateSchemeSeq As Integer
            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtEHSClaimBLL As New EHSClaim.EHSClaimBLL.EHSClaimBLL
            Dim udtSubsidizeWriteOffList As New SubsidizeWriteOffModelCollection()

            Dim udtSchemeClaimModel As SchemeClaimModel
            ' INT15-0012 Fix invalidate claim for subsidy out of claim date [Start][Lawrence]
            udtSchemeClaimModel = udtSchemeClaimBLL.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmServiceDate)
            ' INT15-0012 Fix invalidate claim for subsidy out of claim date [End][Lawrence]

            If Not udtSchemeClaimModel Is Nothing Then
                Select Case udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType
                    Case SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher
                        'check  is current season or not
                        If udtEHSClaimBLL.IsBackSeasonClaim(dtmServiceDate, strSchemeCode, intServiceDateSchemeSeq) = False Then
                            'current season 
                            'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            udtSubsidizeWriteOffList = HandleWriteOff(HandleWriteOffMode.MissingSeasonOnly, Nothing, _
                                                                      strDocCode, strDocID, dtmDOB, strExactDOB, dtmDOD, strExactDOD, _
                                                                      strSchemeCode, strSubsidizeCode, strCreateReason, Nothing, False, udtDB)
                            'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                        Else
                            'date back season 
                            If strCreateReason = eHASubsidizeWriteOff_CreateReason.TxCreation Then
                                strCreateReason = eHASubsidizeWriteOff_CreateReason.TxBackSeasonCreation
                            End If

                            'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            udtSubsidizeWriteOffList = HandleWriteOff(HandleWriteOffMode.SpecificSeason, intServiceDateSchemeSeq, _
                                                                      strDocCode, strDocID, dtmDOB, strExactDOB, dtmDOD, strExactDOD, _
                                                                      strSchemeCode, strSubsidizeCode, strCreateReason, Nothing, False, udtDB)
                            'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

                        End If

                    Case Else
                        'only handling voucher right now
                End Select

            End If
        End Sub

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Calculate and update write off value related deceased, start from specified service date
        ''' </summary>
        ''' <param name="udtPersonalInformationModelCollection"></param>
        ''' <param name="strCreateReason"></param>                
        ''' <param name="udtDB"></param>     
        ''' <remarks></remarks>
        Public Sub UpdateWriteOffRelatedDeceased(ByVal dtmDOD As DateTime, ByVal udtPersonalInformationModelCollection As EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection, _
                                                 ByVal strCreateReason As String, Optional ByVal udtDB As Database = Nothing)

            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection

            udtSchemeClaimModelCollection = udtSchemeClaimBLL.getAllSchemeClaim_WithSubsidizeGroup

            Dim udtEHSPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = udtPersonalInformationModelCollection(0)

            For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollection
                'only handling voucher right now
                Select Case udtSchemeClaimModel.SubsidizeGroupClaimList(0).SubsidizeType
                    Case SubsidizeGroupClaimModel.SubsidizeTypeClass.SubsidizeTypeVoucher
                        Dim udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = udtSchemeClaimModel.SubsidizeGroupClaimList.Filter(dtmDOD)(0)

                        Me.HandleWriteOff(HandleWriteOffMode.SpecificSeason, udtSubsidizeGroupClaimModel.SchemeSeq, _
                                          udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.DOB, udtEHSPersonalInfo.ExactDOB, _
                                          udtEHSPersonalInfo.DOD, udtEHSPersonalInfo.ExactDOD, _
                                          udtSchemeClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SubsidizeCode, strCreateReason, Nothing, False, udtDB)

                        Exit For
                End Select
            Next

        End Sub
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

        ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
#Region "For SubsidizeWriteOffGeneratorQueue"
        Public Function GetSubsidizeWriteOffGeneratorQueue(ByVal strRecordStatus As String, Optional ByVal intPage As Integer = 1, Optional ByVal intTotalPage As Integer = 1) As SubsidizeWriteOffGeneratorQueue
            Dim udtSubsidizeWriteOffGeneratorQueue As New SubsidizeWriteOffGeneratorQueue

            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
            ' -----------------------------------------------------------------------------------------
            Dim udtDB As New Database
            Dim dt As New DataTable
            Dim dtmDOD As Nullable(Of DateTime)

            Try

                Dim params() As SqlParameter = {udtDB.MakeInParam("@record_status", SubsidizeWriteOffGeneratorQueueItem.DATA_TYPE_RECORD_STATUS, SubsidizeWriteOffGeneratorQueueItem.DATA_SIZE_RECORD_STATUS, strRecordStatus), _
                                                udtDB.MakeInParam("@page", SqlDbType.SmallInt, 2, intPage), _
                                                udtDB.MakeInParam("@total_page", SqlDbType.SmallInt, 2, intTotalPage)}

                udtDB.RunProc("proc_SubsidizeWriteOffGeneratorQueue_get", params, dt)

                If dt.Rows.Count > 0 Then
                    Dim udtSubsidizeWriteOffGeneratorQueueItem As SubsidizeWriteOffGeneratorQueueItem

                    For Each dr As DataRow In dt.Rows

                        dtmDOD = Nothing

                        If Not dr.IsNull("DOD") Then
                            dtmDOD = Convert.ToDateTime(dr.Item("DOD"))
                        End If

                        udtSubsidizeWriteOffGeneratorQueueItem = New SubsidizeWriteOffGeneratorQueueItem( _
                            CInt(dr.Item("Row_ID")), _
                            CStr(dr.Item("Doc_Code")).Trim(), _
                            CStr(dr.Item("Doc_No")), _
                            CDate(dr.Item("DOB")), _
                            CStr(dr.Item("Exact_DOB")).Trim(), _
                            CStr(dr.Item("Scheme_Code")).Trim(), _
                            CStr(dr.Item("Subsidize_Code")).Trim(), _
                            CStr(dr.Item("Record_Status")).Trim(), _
                            CType(dr.Item("TSMP"), Byte()), _
                            dtmDOD, _
                            dr("Exact_DOD").ToString())

                        udtSubsidizeWriteOffGeneratorQueue.Enqueue(udtSubsidizeWriteOffGeneratorQueueItem)

                    Next
                End If

            Catch ex As Exception
                Throw
            End Try
            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

            Return udtSubsidizeWriteOffGeneratorQueue
        End Function

        Public Function UpdateSubsidizeWriteOffGeneratorQueue(ByVal udtSubsidizeWriteOffGeneratorQueueItem As SubsidizeWriteOffGeneratorQueueItem, ByVal strRecordStatus As String) As Boolean
            Dim udtDB As New Database

            Try

                Dim params() As SqlParameter = {udtDB.MakeInParam("@row_id", SubsidizeWriteOffGeneratorQueueItem.DATA_TYPE_ROW_ID, SubsidizeWriteOffGeneratorQueueItem.DATA_SIZE_ROW_ID, udtSubsidizeWriteOffGeneratorQueueItem.RowID), _
                                                udtDB.MakeInParam("@record_status", SubsidizeWriteOffGeneratorQueueItem.DATA_TYPE_RECORD_STATUS, SubsidizeWriteOffGeneratorQueueItem.DATA_SIZE_RECORD_STATUS, strRecordStatus), _
                                                udtDB.MakeInParam("@tsmp", SubsidizeWriteOffModel.TSMPDataType, SubsidizeWriteOffModel.TSMPDataSize, udtSubsidizeWriteOffGeneratorQueueItem.TSMP)}

                udtDB.BeginTransaction()

                udtDB.RunProc("proc_SubsidizeWriteOffGeneratorQueue_upd", params)

                udtDB.CommitTransaction()

            Catch ex As Exception

                udtDB.RollBackTranscation()
                Throw

            Finally

                If Not udtDB Is Nothing Then udtDB.Dispose()

            End Try

            Return True
        End Function
#End Region
        ' CRE13-006 - HCVS Ceiling [End][Tommy L]

        'CRE14-016 (To introduce "Deceased" status into eHS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Function IsExistWriteOffAccount(ByVal udtPersonalInformationModel As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, Optional ByVal udtDB As Database = Nothing) As Boolean
            Dim udtRelatedPersonalInformationModelCollection As New EHSAccount.EHSAccountModel.EHSPersonalInformationModelCollection

            'only proceed if identical account not found
            udtRelatedPersonalInformationModelCollection = GetRelatedWriteOffAccount(udtPersonalInformationModel.DocCode, udtPersonalInformationModel.IdentityNum, udtDB)

            For Each udtRelatedPersonalInfoModel As EHSAccount.EHSAccountModel.EHSPersonalInformationModel In udtRelatedPersonalInformationModelCollection
                If IsIdenticalAccount(udtRelatedPersonalInfoModel, udtPersonalInformationModel) Then
                    'Exit imediately if identical account is found
                    Return True
                End If
            Next

            Return False
        End Function
        'CRE14-016 (To introduce "Deceased" status into eHS) [End][Chris YIM]

    End Class
End Namespace
