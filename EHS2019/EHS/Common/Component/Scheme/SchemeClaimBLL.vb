Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.ComFunction
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSTransaction
Imports Common.Component.SchemeInformation
Imports System.Xml

Namespace Component.Scheme
    Public Class SchemeClaimBLL

        Public Class CACHE_STATIC_DATA
            Public Const CACHE_ALL_SchemeClaim As String = "SchemeClaimBLL_ALL_SchemeClaim"
            Public Const CACHE_ALL_SubsidizeGroupClaim As String = "SchemeClaimBLL_ALL_SubsidizeGroupClaim"
            Public Const CACHE_ALL_SchemeEnrolClaimMap As String = "SchemeClaimBLL_ALL_SchemeEnrolClaimMap"
            Public Const CACHE_ALL_SubsidizeEnrolClaimMap As String = "SchemeClaimBLL_ALL_SubsidizeEnrolClaimMap"
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            Public Const CACHE_ALL_SubsidizeFee As String = "SchemeClaimBLL_ALL_SubsidizeFee"
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            'INT13-0033 Missed cache insert [Start][Karl]
            Public Const CACHE_ALL_SubsidizeGroupClaimAll As String = "SchemeClaimBLL_ALL_SubsidizeGroupClaimAll"
            Public Const CACHE_ALL_SchemeClaimAll As String = "SchemeClaimBLL_ALL_SchemeClaimAll"
            'INT13-0033 Missed cache insert [End][Karl]
        End Class

#Region "Table Schema Field"

        Public Class tableSchemeClaim
            Public Const Scheme_Code As String = "Scheme_Code"
            Public Const Scheme_Seq As String = "Scheme_Seq"
            Public Const Scheme_Desc As String = "Scheme_Desc"
            Public Const Scheme_Desc_Chi As String = "Scheme_Desc_Chi"
            Public Const Scheme_Desc_CN As String = "Scheme_Desc_CN"
            Public Const Display_Code As String = "Display_Code"
            Public Const Display_Seq As String = "Display_Seq"
            Public Const BalanceEnquiry_Available As String = "BalanceEnquiry_Available"
            Public Const IVRS_Available As String = "IVRS_Available"
            Public Const TextOnly_Available As String = "TextOnly_Available"
            Public Const Claim_Period_From As String = "Claim_Period_From"
            Public Const Claim_Period_To As String = "Claim_Period_To"
            Public Const Create_By As String = "Create_By"
            Public Const Create_Dtm As String = "Create_Dtm"
            Public Const Update_By As String = "Update_By"
            Public Const Update_Dtm As String = "Update_Dtm"
            Public Const Record_Status As String = "Record_Status"
            Public Const Effective_Dtm As String = "Effective_Dtm"
            Public Const Expiry_Dtm As String = "Expiry_Dtm"
            Public Const TSWCheckingEnable As String = "TSWCheckingEnable"
            Public Const ControlType As String = "Control_Type"
            Public Const ControlSetting As String = "Control_Setting"
            Public Const ConfirmedTransactionStatus As String = "Confirmed_Transaction_Status"
            Public Const Reimbursement_Mode As String = "Reimbursement_Mode"
            Public Const Reimbursement_Currency As String = "Reimbursement_Currency"
            Public Const Available_HCSP_SubPlatform As String = "Available_HCSP_SubPlatform"
            Public Const ProperPractice_Avail As String = "ProperPractice_Avail"
            Public Const ProperPractice_SectionID As String = "ProperPractice_SectionID"

            ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            Public Const Readonly_HCSP As String = "Readonly_HCSP"
            ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        End Class

        Public Class tableSubsidizeGroupClaim

            Public Const Scheme_Code = "Scheme_Code"
            Public Const Scheme_Seq = "Scheme_Seq"
            Public Const Subsidize_Code = "Subsidize_Code"
            Public Const Display_Seq = "Display_Seq"
            Public Const Claim_Period_From = "Claim_Period_From"

            Public Const Claim_Period_To = "Claim_Period_To"
            Public Const Consent_Form_Compulsory = "Consent_Form_Compulsory"
            Public Const Num_Subsidize = "Num_Subsidize"

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'Public Const Subsidize_value = "Subsidize_value"
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            'CRE13-006 HCVS Ceiling [Start][Karl]
            'Public Const Carry_Forward = "Carry_Forward"
            'CRE13-006 HCVS Ceiling [End][Karl]'

            Public Const Create_By = "Create_By"
            Public Const Create_Dtm = "Create_Dtm"
            Public Const Update_By = "Update_By"

            Public Const Update_Dtm = "Update_Dtm"
            Public Const Record_Status = "Record_Status"
            Public Const AdhocPrint_Available = "AdhocPrint_Available"

            Public Const Last_Service_Dtm = "Last_Service_Dtm"

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            Public Const Display_Code_For_Claim = "Display_Code_For_Claim"
            Public Const Legend_Desc_For_Claim = "Legend_Desc_For_Claim"
            Public Const Legend_Desc_For_Claim_Chi = "Legend_Desc_For_Claim_Chi"
            Public Const Legend_Desc_For_Claim_CN = "Legend_Desc_For_Claim_CN"
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            ' CRE13-001 - EHAPP [Start][Tommy L]
            ' -------------------------------------------------------------------------------------
            Public Const ClaimDeclaration_Available = "ClaimDeclaration_Available"
            ' CRE13-001 - EHAPP [End][Tommy L]

            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            Public Const Num_Subsidize_Ceiling = "Num_Subsidize_Ceiling"
            ' CRE13-006 - HCVS Ceiling [End][Tommy L]

            'CRE13-018 Change Voucher Amount to 1 Dollar [Start][Karl]
            Public Const Copayment_Fee = "CoPayment_Fee"
            'CRE13-018 Change Voucher Amount to 1 Dollar [End][Karl]

            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            Public Const Consent_Form_Avail_Version = "Consent_Form_Avail_Version"
            Public Const Consent_Form_Avail_Lang = "Consent_Form_Avail_Lang"
            Public Const Print_Option_Avail = "Print_Option_Avail"

            Public Const Consent_Form_Avail_Version_CN = "Consent_Form_Avail_Version_CN"
            Public Const Consent_Form_Avail_Lang_CN = "Consent_Form_Avail_Lang_CN"
            Public Const Print_Option_Avail_CN = "Print_Option_Avail_CN"
            'CRE13-019-02 Extend HCVS to China [End][Winnie]

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Public Const High_Risk_Option = "High_Risk_Option"
            'CRE16-026 (Add PCV13) [End][Chris YIM]

        End Class

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Class tableSubsidizeFee
            Public Const Scheme_Code = "SBFE_Scheme_Code"
            Public Const Subsidize_Seq = "SBFE_Subsidize_Seq"
            Public Const Subsidize_Code = "SBFE_Subsidize_Code"
            Public Const Effective_Dtm = "SBFE_Effective_Dtm"
            Public Const Expiry_Dtm = "SBFE_Expiry_Dtm"

            Public Const Subsidize_Fee = "SBFE_Subsidize_Fee"
            Public Const Subsidize_Fee_Type = "SBFE_Subsidize_Fee_Type"
            Public Const Subsidize_Fee_Type_Display_Seq = "SBFE_Subsidize_Fee_Type_Display_Seq"
            Public Const Subsidize_Fee_Type_Display_Resource = "SBFE_Subsidize_Fee_Type_Display_Resource"
            Public Const Subsidize_Fee_Visible = "SBFE_Subsidize_Fee_Visible"

            Public Const Create_By = "SBFE_Create_By"
            Public Const Create_Dtm = "SBFE_Create_Dtm"
            Public Const Update_By = "SBFE_Update_By"
            Public Const Update_Dtm = "SBFE_Update_Dtm"
        End Class
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        Public Class tableSubsidize
            Public Const Display_Code = "Display_Code"
            Public Const Lengend_Desc = "Legend_Desc"
            Public Const Lengend_Desc_Chi = "Legend_Desc_Chi"
            Public Const Lengend_Desc_CN = "Legend_Desc_CN"
        End Class

        Public Class tableSubsidizeItem
            Public Const Subsidize_Item_Code = "Subsidize_Item_Code"
            Public Const Subsidize_Item_Desc = "Subsidize_Item_Desc"
            Public Const Subsidize_item_Desc_Chi = "Subsidize_item_Desc_Chi"
            Public Const Subsidize_Type = "Subsidize_Type"
        End Class

        Public Class tableSchemeEnrolClaimMap
            Public Const Scheme_Code_Enrol = "Scheme_Code_Enrol"
            Public Const Scheme_Code_Claim = "Scheme_Code_Claim"
        End Class

        Public Class tableSubsidizeEnrolClaimMap
            Public Const Scheme_Code_Enrol = "Scheme_Code_Enrol"
            Public Const Subsidize_Code_Enrol = "Subsidize_Code_Enrol"
            Public Const Scheme_Code_Claim = "Scheme_Code_Claim"
            Public Const Subsidize_Code_Claim = "Subsidize_Code_Claim"
        End Class

#End Region

        Public Function ConvertSchemeEnrolFromSchemeClaimCode(ByVal strSchemeCode As String) As String
            Dim dt As DataTable = Me.getAllActiveSchemeEnrolClaimMapCache()

            Dim arrDr As DataRow() = dt.Select(tableSchemeEnrolClaimMap.Scheme_Code_Claim + "='" + strSchemeCode.Trim() + "'")

            If arrDr.Length > 0 Then
                Return arrDr(0)(tableSchemeEnrolClaimMap.Scheme_Code_Enrol).ToString().Trim()
            Else
                Return ""
            End If
        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Function ConvertControlTypeFromSchemeClaimCode(ByVal strSchemeCode As String) As SchemeClaimModel.EnumControlType
            Dim udtSchemeClaimModel As SchemeClaimModel = Me.getAllActiveSchemeClaimCache().Filter(strSchemeCode)
            If Not udtSchemeClaimModel Is Nothing Then
                Return udtSchemeClaimModel.ControlType
            Else
                Return Nothing
            End If
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Public Function ConvertSchemeClaimCodeFromSchemeEnrol(ByVal strSchemeCode As String) As String
            Dim dr As DataRow() = Me.getAllActiveSchemeEnrolClaimMapCache().Select(String.Format("{0} = '{1}'", tableSchemeEnrolClaimMap.Scheme_Code_Enrol, strSchemeCode))

            If dr.Length = 1 Then
                Return dr(0)(tableSchemeEnrolClaimMap.Scheme_Code_Claim)
            End If

            Return String.Empty

        End Function
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

        ''' <summary>
        ''' Convert Scheme Enrol to SchemeClaim.[SchemeCode]
        ''' </summary>
        ''' <param name="udtSchemeInformationList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ConvertSchemeClaimCodeFromSchemeEnrol(ByVal udtSchemeInformationList As SchemeInformation.SchemeInformationModelCollection) As List(Of String)

            Dim lstStrSchemeClaimCode As New List(Of String)
            Dim dt As DataTable = Me.getAllActiveSchemeEnrolClaimMapCache()

            For Each udtSchemeInformation As SchemeInformation.SchemeInformationModel In udtSchemeInformationList.Values
                If udtSchemeInformation.RecordStatus.Trim() = Common.Component.SchemeInformationMaintenanceDisplayStatus.Active _
                        OrElse udtSchemeInformation.RecordStatus.Trim() = Common.Component.SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist _
                        OrElse udtSchemeInformation.RecordStatus.Trim() = Common.Component.SchemeInformationMaintenanceDisplayStatus.ActivePendingSuspend Then

                    Dim arrDr As DataRow() = dt.Select(tableSchemeEnrolClaimMap.Scheme_Code_Enrol + "='" + udtSchemeInformation.SchemeCode.Trim() + "'")

                    For Each dr As DataRow In arrDr
                        Dim strSchemeCode As String = dr(tableSchemeEnrolClaimMap.Scheme_Code_Claim).ToString().Trim()
                        If strSchemeCode.Trim() <> "" AndAlso Not lstStrSchemeClaimCode.Contains(strSchemeCode) Then
                            lstStrSchemeClaimCode.Add(strSchemeCode)
                        End If
                    Next
                End If
            Next
            Return lstStrSchemeClaimCode
        End Function

        ''' <summary>
        ''' Convert Scheme Enrol to Scheme Claim List (Sort by Display Seq)
        ''' </summary>
        ''' <param name="udtPracticeSchemeInfoList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ConvertSchemeClaimCodeFromSchemeEnrol(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfo.PracticeSchemeInfoModelCollection) As SchemeClaimModelCollection
            Dim lstStrSchemeClaimCode As New List(Of String)
            Dim dt As DataTable = Me.getAllActiveSchemeEnrolClaimMapCache()

            Dim udtGenFunct As New GeneralFunction()
            Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()

            ' Ever enrolled and the scheme has been effective
            For Each udtPracticeSchemeInfo As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values

                'If udtPracticeSchemeInfo.RecordStatus.Trim() = Common.Component.SchemeInformationStatus.Active Then
                Dim arrDr As DataRow() = dt.Select(tableSchemeEnrolClaimMap.Scheme_Code_Enrol + "='" + udtPracticeSchemeInfo.SchemeCode.Trim() + "'")

                For Each dr As DataRow In arrDr
                    Dim strSchemeCode As String = dr(tableSchemeEnrolClaimMap.Scheme_Code_Claim).ToString().Trim()
                    If strSchemeCode.Trim() <> "" AndAlso Not lstStrSchemeClaimCode.Contains(strSchemeCode) Then
                        lstStrSchemeClaimCode.Add(strSchemeCode)
                    End If
                Next
                'End If
            Next

            Dim udtSchemeClaimModelList As New SchemeClaimModelCollection()

            For Each strSchemeClaim As String In lstStrSchemeClaimCode
                Dim udtSchemeClaim As SchemeClaimModel = Me.getAllActiveSchemeClaimCache().FilterLastEffective(strSchemeClaim.Trim(), dtmCurrentDate)
                If Not udtSchemeClaim Is Nothing Then
                    udtSchemeClaimModelList.Add(New SchemeClaimModel(udtSchemeClaim))
                End If
            Next
            udtSchemeClaimModelList.Sort()
            Return udtSchemeClaimModelList
        End Function

#Region "Search Function"

        ''' <summary>
        ''' Retrieve SchemeClaim List For EHS Claim Enter Claim Detail
        ''' Check with Eligible Rule
        ''' Check with available document
        ''' </summary>
        ''' <param name="udtEHSAccount"></param>
        ''' <param name="strDocCode"></param>
        ''' <param name="udtSchemeClaimModelCollection"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function searchEligibleClaimScheme(ByVal udtEHSAccount As EHSAccount.EHSAccountModel, ByVal strDocCode As String, ByVal udtSchemeClaimModelCollection As SchemeClaimModelCollection) As SchemeClaimModelCollection

            Dim udtGenFunct As New GeneralFunction()
            Dim dtmCurrentDateTime As Date = udtGenFunct.GetSystemDateTime()
            Dim dtmCurrentDate = dtmCurrentDateTime.Date

            Dim udtReturnSchemeClaimModelList As New SchemeClaimModelCollection()

            Dim udtDocTypeBLL As New DocType.DocTypeBLL()
            Dim udtSchemeDocTypeList As DocType.SchemeDocTypeModelCollection = udtDocTypeBLL.getSchemeDocTypeByDocType(strDocCode)

            Dim udtClaimRulesBLL As New ClaimRules.ClaimRulesBLL()
            ' Check Eligible
            ' Check SchemeDocType
            For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollection

                If udtSchemeDocTypeList.Contain(udtSchemeClaimModel.SchemeCode) Then

                    ' [2009Oct22] Performance Tuning
                    Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL()
                    Dim udtTranBenefitList As EHSTransaction.TransactionDetailModelCollection = Nothing

                    Dim strCheckPeriod As String = String.Empty
                    Dim strDummy As String = String.Empty

                    udtGenFunct.getSystemParameter("CheckEligibilityTransDate", strCheckPeriod, strDummy)
                    Dim dtmCheckDate As DateTime = CDate(strCheckPeriod)

                    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                    ' --------------------------------------------------------------------------------------
                    If (udtSchemeClaimModel.ControlType = SchemeClaimModel.EnumControlType.CIVSS OrElse _
                        udtSchemeClaimModel.ControlType = SchemeClaimModel.EnumControlType.EVSS OrElse _
                        udtSchemeClaimModel.ControlType = SchemeClaimModel.EnumControlType.HSIVSS OrElse _
                        udtSchemeClaimModel.ControlType = SchemeClaimModel.EnumControlType.RVP OrElse _
                        udtSchemeClaimModel.ControlType = SchemeClaimModel.EnumControlType.PIDVSS OrElse _
                        udtSchemeClaimModel.ControlType = SchemeClaimModel.EnumControlType.VSS OrElse _
                        udtSchemeClaimModel.ControlType = SchemeClaimModel.EnumControlType.ENHVSSO OrElse _
                        udtSchemeClaimModel.ControlType = SchemeClaimModel.EnumControlType.PPP _
                        ) AndAlso dtmCheckDate <= dtmCurrentDateTime Then

                        udtTranBenefitList = udtEHSTransactionBLL.getTransactionDetailBenefit(strDocCode, udtEHSAccount.getPersonalInformation(strDocCode).IdentityNum)

                    End If
                    ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                    Dim udtEligibleResult As ClaimRules.ClaimRulesBLL.EligibleResult = udtClaimRulesBLL.CheckEligibilityFromEHSClaimSearch(udtSchemeClaimModel, udtEHSAccount.getPersonalInformation(strDocCode), dtmCurrentDate, udtTranBenefitList)

                    If udtEligibleResult.IsEligible Then
                        Dim udtResSchemeClaimModel As New SchemeClaimModel(udtSchemeClaimModel)
                        udtResSchemeClaimModel.SubsidizeGroupClaimList = New SubsidizeGroupClaimModelCollection()
                        For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In udtSchemeClaimModel.SubsidizeGroupClaimList

                            ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
                            ' -----------------------------------------------------------------------------------------
                            udtEligibleResult = udtClaimRulesBLL.CheckEligibilityPerSubsidizeFromEHSClaim(udtSubsidizeGroupClaimModel, udtEHSAccount.getPersonalInformation(strDocCode), dtmCurrentDate, udtTranBenefitList)
                            ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

                            If udtEligibleResult.IsEligible Then
                                Dim udtResSubsidizeGroupClaimModel As New SubsidizeGroupClaimModel(udtSubsidizeGroupClaimModel)
                                udtResSchemeClaimModel.SubsidizeGroupClaimList.Add(udtResSubsidizeGroupClaimModel)
                            End If
                        Next

                        udtReturnSchemeClaimModelList.Add(udtResSchemeClaimModel)
                    End If
                End If
            Next

            Return udtReturnSchemeClaimModelList
        End Function

        ''' <summary>
        ''' Search Effective SchemeClaim List By PracticeSchemeInfo.[SubsidizeCode]
        ''' </summary>
        ''' <param name="udtPracticeSchemeInfoList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function searchEffectiveSchemeClaimByPracticeSchemeInfoSubsidizeCode(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfo.PracticeSchemeInfoModelCollection) As SchemeClaimModelCollection
            Dim lstStrSubsidizeCode As New List(Of String)
            For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                If Not lstStrSubsidizeCode.Contains(udtPracticeSchemeInfoModel.SubsidizeCode.Trim()) Then
                    lstStrSubsidizeCode.Add(udtPracticeSchemeInfoModel.SubsidizeCode.Trim())
                End If
            Next
            Return Me.searchEffectiveSchemeClaimBySubsidizeCodeList(lstStrSubsidizeCode)
        End Function

        ''' <summary>
        ''' Search Valid Claim Period SchemeClaim List By PracticeSchemeInfo.[SubsidizeCode]
        ''' </summary>
        ''' <param name="udtPracticeSchemeInfoList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfo.PracticeSchemeInfoModelCollection, ByVal udtSchemeInfoList As SchemeInformation.SchemeInformationModelCollection) As SchemeClaimModelCollection
            Return Me.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(udtPracticeSchemeInfoList, udtSchemeInfoList, (New GeneralFunction).GetSystemDateTime)
        End Function

        ''' <summary>
        ''' Search Valid Claim Period SchemeClaim List By PracticeSchemeInfo.[SubsidizeCode]
        ''' </summary>
        ''' <param name="udtPracticeSchemeInfoList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfo.PracticeSchemeInfoModelCollection, _
                                                                                           ByVal udtSchemeInfoList As SchemeInformation.SchemeInformationModelCollection, _
                                                                                           ByVal dtmServiceDate As Date) As SchemeClaimModelCollection

            ' 1. Check SchemeBackOffice / SubsidizeGroupBackOffice, check if ServiceFee is compulsory and the SP has provided service fee
            ' (For HSIVSS [2009-12-15]) : Handle SubsidizeMapping
            ' 2. Check SchemeClaim / SubsidizeGroupClaim within the Claim peroid

            Dim lstStrSchemeCodeClaim As New List(Of String)
            Dim lstStrSubsidizeCodeClaim As New List(Of String)

            Dim dtSubsidizeMap As DataTable = Me.getAllActiveSubsidizeEnrolClaimMapCache()

            'Dim udtGF As New Common.ComFunction.GeneralFunction()
            'Dim dtmDate As DateTime = udtGF.GetSystemDateTime()

            Dim udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL()
            Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.getAllDistinctSchemeBackOffice(dtmServiceDate)

            '' '' To Handle: if the SchemeBackOffice expired, the PracticeSchemeInformation will not include the corresponding scheme
            '' '' To fix: retrieve all practice scheme information

            ' ''If udtPracticeSchemeInfoList.Count > 0 Then
            ' ''    Dim strSPID As String = CType(udtPracticeSchemeInfoList.GetByIndex(0), PracticeSchemeInfo.PracticeSchemeInfoModel).SPID
            ' ''    Dim intPracticeID As String = CType(udtPracticeSchemeInfoList.GetByIndex(0), PracticeSchemeInfo.PracticeSchemeInfoModel).PracticeDisplaySeq

            ' ''    ' CRE15-004 TIV & QIV [Start][Lawrence]
            ' ''    udtSchemeInfoList = (New SchemeInformationBLL).GetSchemeInfoListPermanent(strSPID, New Database())
            ' ''    ' CRE15-004 TIV & QIV [End][Lawrence]

            ' ''    Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfo.PracticeSchemeInfoBLL()
            ' ''    udtPracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentBySPID_All(strSPID, intPracticeID, New Database())
            ' ''End If

            For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values

                'If udtPracticeSchemeInfoModel.RecordStatus = "A" Then
                If udtPracticeSchemeInfoModel.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.Active OrElse _
                    udtPracticeSchemeInfoModel.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingDelist OrElse _
                    udtPracticeSchemeInfoModel.RecordStatus = PracticeSchemeInfoMaintenanceDisplayStatus.ActivePendingSuspend Then

                    Dim udtSchemeInfo As SchemeInformation.SchemeInformationModel = udtSchemeInfoList.Filter(udtPracticeSchemeInfoModel.SchemeCode)
                    If Not udtSchemeInfo Is Nothing Then
                        If udtSchemeInfo.RecordStatus = SchemeInformationMaintenanceDisplayStatus.Active OrElse _
                            udtSchemeInfo.RecordStatus = SchemeInformationMaintenanceDisplayStatus.ActivePendingDelist OrElse _
                            udtSchemeInfo.RecordStatus = SchemeInformationMaintenanceDisplayStatus.ActivePendingSuspend Then

                            ' INT20-0023 (Fix to hide SIV on season end) [Start][Chris YIM]
                            ' ---------------------------------------------------------------------------------------------------------
                            Dim udtSchemeBackOfficeModel As SchemeBackOfficeModel = udtSchemeBackOfficeList.FilterLastEffective(udtPracticeSchemeInfoModel.SchemeCode, dtmServiceDate)

                            If Not udtSchemeBackOfficeModel Is Nothing Then
                                Dim udtSubdizeGroupBackOfficeModel As SubsidizeGroupBackOfficeModel = udtSchemeBackOfficeModel.SubsidizeGroupBackOfficeList().Filter(udtPracticeSchemeInfoModel.SchemeCode, udtPracticeSchemeInfoModel.SubsidizeCode)

                                If Not udtSubdizeGroupBackOfficeModel.ServiceFeeCompulsory OrElse _
                                    (udtSubdizeGroupBackOfficeModel.ServiceFeeCompulsory AndAlso udtPracticeSchemeInfoModel.ProvideServiceFee.HasValue AndAlso udtPracticeSchemeInfoModel.ProvideServiceFee.Value = True) Then

                                    Dim arrDr As DataRow() = dtSubsidizeMap.Select( _
                                        tableSubsidizeEnrolClaimMap.Scheme_Code_Enrol + "='" + udtPracticeSchemeInfoModel.SchemeCode.Trim() + "' AND " _
                                        + tableSubsidizeEnrolClaimMap.Subsidize_Code_Enrol + "='" + udtPracticeSchemeInfoModel.SubsidizeCode.Trim() + "'")

                                    For Each drRow As DataRow In arrDr
                                        lstStrSchemeCodeClaim.Add(drRow(tableSubsidizeEnrolClaimMap.Scheme_Code_Claim).ToString().Trim())
                                        lstStrSubsidizeCodeClaim.Add(drRow(tableSubsidizeEnrolClaimMap.Subsidize_Code_Claim).ToString().Trim())
                                    Next

                                End If

                            End If
                            ' INT20-0023 (Fix to hide SIV on season end) [End][Chris YIM]
                        End If
                    End If
                End If
            Next

            Return Me.searchValidClaimPeriodSchemeClaimBySchemeSubsidizeCodeList(lstStrSchemeCodeClaim, lstStrSubsidizeCodeClaim, dtmServiceDate)

        End Function

        ''' <summary>
        ''' Search Effective SchemeClaim By: 
        '''     -> Subsidize Code List -> SubsidizeGroupClaim -> SchemeClaim
        ''' </summary>
        ''' <param name="lstStrSubsidizeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function searchEffectiveSchemeClaimBySubsidizeCodeList(ByVal lstStrSubsidizeCode As List(Of String)) As SchemeClaimModelCollection

            Dim udtGenFunct As New GeneralFunction()
            Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()

            Dim udtResSchemeClaimModelCollection As New SchemeClaimModelCollection()
            Dim lstStrSchemeClaimCode As New List(Of String)

            ' Retrieve All SubsidizeGroupClaim.[SchemeCode] by Subsidize Code & Append the [SchemeCode]
            Dim udtSubsidizeGroupClaimModelCollection As SubsidizeGroupClaimModelCollection = Me.getAllActiveSubsidizeGroupCache()

            For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In udtSubsidizeGroupClaimModelCollection
                If lstStrSubsidizeCode.Contains(udtSubsidizeGroupClaimModel.SubsidizeCode.Trim()) Then
                    If Not lstStrSchemeClaimCode.Contains(udtSubsidizeGroupClaimModel.SchemeCode.Trim()) Then
                        lstStrSchemeClaimCode.Add(udtSubsidizeGroupClaimModel.SchemeCode.Trim())
                    End If
                End If
            Next

            ' Retrieve Effective SchemeClaim & append [SchemeClaim] to the return list
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Me.getAllEffectiveSchemeClaimFromCache(dtmCurrentDate)

            For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollection
                If lstStrSchemeClaimCode.Contains(udtSchemeClaimModel.SchemeCode.Trim()) Then
                    udtResSchemeClaimModelCollection.Add(New SchemeClaimModel(udtSchemeClaimModel))
                End If
            Next
            Return udtResSchemeClaimModelCollection
        End Function

        Private Function searchValidClaimPeriodSchemeClaimBySchemeSubsidizeCodeList(ByVal lstStrSchemeCode As List(Of String), ByVal lstStrSubsidizeCode As List(Of String)) As SchemeClaimModelCollection
            Return searchValidClaimPeriodSchemeClaimBySchemeSubsidizeCodeList(lstStrSchemeCode, lstStrSubsidizeCode, (New GeneralFunction).GetSystemDateTime)

        End Function

        Private Function searchValidClaimPeriodSchemeClaimBySchemeSubsidizeCodeList(ByVal lstStrSchemeCode As List(Of String), ByVal lstStrSubsidizeCode As List(Of String), ByVal dtmServiceDate As Date) As SchemeClaimModelCollection
            If lstStrSchemeCode.Count <> lstStrSubsidizeCode.Count Then
                Throw New Exception("SchemeClaimBLL.searchValidClaimPeriodSchemeClaimBySchemeSubsidizeCodeList: Scheme & Subsidize Not Match")
            End If

            'Dim udtGenFunct As New GeneralFunction()
            'Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()

            Dim udtResSchemeClaimModelCollection As New SchemeClaimModelCollection()
            Dim lstStrSchemeClaimCode As New List(Of String)

            Dim lstFindSchemeCode As New List(Of String)

            ' Retrieve Valid Claim Period SubsidizeGroupClaim.[SchemeCode] by Subsidize Code & Append the [SchemeCode]
            Dim udtSubsidizeGroupClaimModelCollection As SubsidizeGroupClaimModelCollection = Me.getAllValidSubsidizeGroupClaimFromCache(dtmServiceDate)

            ' Retrieve Valid Claim Period SchemeClaim & append [SchemeClaim] to the return list
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Me.getAllValidClaimPeriodSchemeClaimFromCache(dtmServiceDate)

            For i As Integer = 0 To lstStrSchemeCode.Count - 1
                For Each udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In udtSubsidizeGroupClaimModelCollection
                    If udtSubsidizeGroupClaimModel.SchemeCode.Trim() = lstStrSchemeCode(i).Trim() AndAlso udtSubsidizeGroupClaimModel.SubsidizeCode.Trim() = lstStrSubsidizeCode(i).Trim() Then
                        If Not lstFindSchemeCode.Contains(lstStrSchemeCode(i).Trim()) Then
                            lstFindSchemeCode.Add(lstStrSchemeCode(i).Trim())
                            Dim udtAddSchemeClaimModel As SchemeClaimModel = Nothing
                            Dim udtFindSchemeClaimModel As SchemeClaimModel = udtSchemeClaimModelCollection.Filter(lstStrSchemeCode(i).Trim())
                            If Not udtFindSchemeClaimModel Is Nothing Then
                                udtAddSchemeClaimModel = New SchemeClaimModel(udtFindSchemeClaimModel)
                                udtAddSchemeClaimModel.SubsidizeGroupClaimList = New SubsidizeGroupClaimModelCollection()
                                udtAddSchemeClaimModel.SubsidizeGroupClaimList.Add(New SubsidizeGroupClaimModel(udtSubsidizeGroupClaimModel))
                                udtResSchemeClaimModelCollection.Add(udtAddSchemeClaimModel)
                            End If
                        Else
                            Dim udtAddedSchemeClaimModel As SchemeClaimModel = udtResSchemeClaimModelCollection.Filter(lstStrSchemeCode(i).Trim())
                            If Not udtAddedSchemeClaimModel Is Nothing Then
                                udtAddedSchemeClaimModel.SubsidizeGroupClaimList.Add(New SubsidizeGroupClaimModel(udtSubsidizeGroupClaimModel))
                            End If
                        End If

                    End If
                Next
            Next

            udtResSchemeClaimModelCollection.Sort()
            For Each udtSchemeClaimModel As SchemeClaimModel In udtResSchemeClaimModelCollection
                udtSchemeClaimModel.SubsidizeGroupClaimList.Sort()
            Next
            Return udtResSchemeClaimModelCollection

        End Function

#End Region

#Region "Checking Function"

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Sub FillAllActiveSubsidizeGroup(ByRef udtSchemeClaimList As SchemeClaimModelCollection)
            Me.FillAllActiveSubsidizeGroupFromCache(udtSchemeClaimList)
        End Sub
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        Public Function CheckSchemeClaimIVRSEnabled(ByVal strSPID As String) As Boolean

            ' Reminder: Only 1 Scheme Seq Entry of SchemeClaim Effective at a time.
            '           Effective of SchemeClaim should have no grap between
            Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfo.PracticeSchemeInfoBLL()

            ' Retrieve ServiceProvider Enrolled Scheme (SchemeBackOffice)
            Dim udtSchemeInfoBLL As New SchemeInformation.SchemeInformationBLL()
            Dim udtSchemeInformationModelCollection As SchemeInformation.SchemeInformationModelCollection = udtSchemeInfoBLL.GetSchemeInfoListPermanent(strSPID, New Common.DataAccess.Database)

            ' Convert the Enrolled Scheme (SchemeBackOffice) to SchemeClaim
            Dim lstStrSchemeClaimCode As List(Of String) = Me.ConvertSchemeClaimCodeFromSchemeEnrol(udtSchemeInformationModelCollection)

            Dim udtSchemeClaimList As SchemeClaimModelCollection = Me.getAllEffectiveSchemeClaimBySchemeCodeList(lstStrSchemeClaimCode)

            ' Any of the Entitled Scheme Contain IVRS Setting, then return true
            For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList
                If udtSchemeClaim.IVRSAvailable Then
                    Return True
                End If
            Next

            Return False
        End Function

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Function CheckSchemeClaimReadonly(ByVal udtPracticeSchemeInfo As PracticeSchemeInfo.PracticeSchemeInfoModel) As Boolean
            ' Convert the Enrolled Scheme (SchemeBackOffice) to SchemeClaim
            Dim strSchemeClaimCode As String = Me.ConvertSchemeClaimCodeFromSchemeEnrol(udtPracticeSchemeInfo.SchemeCode)

            'Dim udtSchemeClaim As SchemeClaimModel = Me.getEffectiveSchemeClaim(strSchemeClaimCode)
            Dim udtSchemeClaim As SchemeClaimModel = Me.getAllActiveSchemeClaimCache().Filter(strSchemeClaimCode)

            'If scheme contains Readonly Setting, then return true
            If udtSchemeClaim.ReadonlyHCSP Then
                Return True
            End If

            Return False
        End Function
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

#End Region

#Region "Retrieve Function"

        ' All 
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ' Remove SchemeSeq
        ''' <summary>
        ''' Retrieve SchemeClaim with SubsidizeGroupClaim for Display Purpose
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(ByVal strSchemeCode As String) As SchemeClaimModel
            Dim udtSchemeClaimModel As SchemeClaimModel = Me.getAllActiveSchemeClaimCache().Filter(strSchemeCode)
            udtSchemeClaimModel.SubsidizeGroupClaimList = Me.getAllActiveSubsidizeGroupCache().Filter(udtSchemeClaimModel.SchemeCode)

            Return udtSchemeClaimModel
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ''' <summary>
        ''' Retrieve all Scheme Claim With SubsidizeGroup by Scheme Code + SubsidizeCode
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="strSubsidizeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSubsidizeCode(ByVal strSchemeCode As String, ByVal strSubsidizeCode As String) As SchemeClaimModelCollection
            Dim udtSchemeClaimModelList As SchemeClaimModelCollection = Me.getAllActiveSchemeClaimCache().FilterList(strSchemeCode)
            Me.FillAllActiveSubsidizeGroupFromCache(strSubsidizeCode, udtSchemeClaimModelList)
            Return udtSchemeClaimModelList
        End Function

        ''' <summary>
        ''' Retrieve the first record (largest Scheme_Seq with Record_Status 'A') for all distinct Scheme Codes which are effective currently or in the past
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllDistinctSchemeClaim() As SchemeClaimModelCollection
            Dim udtGeneralFunction As New GeneralFunction
            Dim dtmCurrentDate As Date = udtGeneralFunction.GetSystemDateTime()

            Dim arySchemeCodeEffective As New ArrayList ' The schemes that are effective currently or in the past
            Dim udtSchemeClaimModelCollectionAll As SchemeClaimModelCollection = Me.getAllActiveSchemeClaimCache()

            For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollectionAll
                Dim strSchemeCode As String = udtSchemeClaimModel.SchemeCode.Trim

                If udtSchemeClaimModel.EffectiveDtm <= dtmCurrentDate _
                        AndAlso Not arySchemeCodeEffective.Contains(strSchemeCode) Then
                    arySchemeCodeEffective.Add(strSchemeCode)
                End If
            Next

            Dim udtSchemeClaimModelCollectionFiltered As New SchemeClaimModelCollection

            For Each strSchemeCodeEffective As String In arySchemeCodeEffective
                Dim udtSchemeClaimModel As SchemeClaimModel = udtSchemeClaimModelCollectionAll.FilterLastEffective(strSchemeCodeEffective, dtmCurrentDate)
                If Not IsNothing(udtSchemeClaimModel) Then udtSchemeClaimModelCollectionFiltered.Add(udtSchemeClaimModel)
            Next

            Return udtSchemeClaimModelCollectionFiltered
        End Function

        ''' <summary>
        ''' Retrieve the first record (largest Scheme_Seq with Record_Status 'A') for all distinct Scheme Codes which are effective currently or in the past, with subsidize groups
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllDistinctSchemeClaim_WithSubsidizeGroup() As SchemeClaimModelCollection
            Dim udtResSchemeClaimModelList As SchemeClaimModelCollection = Me.getAllDistinctSchemeClaim()
            Me.FillAllActiveSubsidizeGroupFromCache(udtResSchemeClaimModelList)
            Return udtResSchemeClaimModelList
        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Function getAllDistinctSchemeClaim_WithEffectiveSubsidizeGroup(ByVal dtmServiceDate As DateTime) As SchemeClaimModelCollection
            Dim udtResSchemeClaimModelList As SchemeClaimModelCollection = Me.getAllDistinctSchemeClaim()
            FillLastServiceDtmSubsidizeGroup(dtmServiceDate, udtResSchemeClaimModelList)
            Return udtResSchemeClaimModelList
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        '-------------------------------------------------
        ''' <summary>
        ''' Retrieve all SchemeClaim, with SubsidizeGroupClaim Info (No matter it is effective or active)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllSchemeClaim_WithSubsidizeGroup() As SchemeClaimModelCollection
            Dim udtGenFunct As New GeneralFunction()

            Dim udtResSchemeClaimModelList As SchemeClaimModelCollection = Me.getAllSchemeClaim()
            Me.FillAllSubsidizeGroup(udtResSchemeClaimModelList)
            Return udtResSchemeClaimModelList
        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Function getAllSubsidizeFee() As SubsidizeFeeModelCollection
            Return Me.getAllSubsidizeFeeCache()
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        '-------------------------------------------------
        ' Valid Claim Period

        ' -------------------------

        ''' <summary>
        ''' Retreive all Valid Claim Period SchemeClaim with SubsidizeGroupClaim info By Current Date
        ''' (e.g For EHSClaim Screen, Load Available Scheme + SubsidizeGroup For Claim)
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllValidClaimPeriodSchemeClaim_WithSubsidizeGroup() As SchemeClaimModelCollection

            Dim udtGenFunct As New GeneralFunction()
            Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()

            Return Me.getAllValidClaimPeriodSchemeClaim_WithSubsidizeGroup(dtmCurrentDate)

        End Function

        ''' <summary>
        ''' Retrieve all Valid Claim Period SchemeClaim with SubsidizeGroupClaim Info By Reference Date
        ''' </summary>
        ''' <param name="dtmCurDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllValidClaimPeriodSchemeClaim_WithSubsidizeGroup(ByVal dtmCurDate As Date) As SchemeClaimModelCollection
            Dim udtResSchemeClaimModelList As SchemeClaimModelCollection = Me.getAllValidClaimPeriodSchemeClaimFromCache(dtmCurDate)
            Me.FillValidClaimPeriodSubsidizeGroupFromCache(dtmCurDate, udtResSchemeClaimModelList)
            Return udtResSchemeClaimModelList
        End Function

        ' -------------------------

        ''' <summary>
        ''' Retrieve Valid Claim Period SchemeClaim By SchemeCode + Current Date
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getValidClaimPeriodSchemeClaimBySchemeCode(ByVal strSchemeCode As String) As SchemeClaimModel

            Dim udtGenFunct As New GeneralFunction()
            Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()

            Return Me.getValidClaimPeriodSchemeClaimBySchemeCode(strSchemeCode, dtmCurrentDate)
        End Function

        ''' <summary>
        ''' Retrieve The Valid Claim Period SchemeClaim By Scheme Code + Reference Date
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="dtmCurdate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getValidClaimPeriodSchemeClaimBySchemeCode(ByVal strSchemeCode As String, ByVal dtmCurdate As Date) As SchemeClaimModel
            Return Me.getAllValidClaimPeriodSchemeClaimFromCache(dtmCurdate).Filter(strSchemeCode)
        End Function

        ' -------------------------

        ''' <summary>
        ''' Retrieve Valid Claim Period SchemeClaim By SchemeCode + Current Date
        ''' (e.g For EHSClaim Screen, load the selected available Scheme + SubsidizeGroup for check eligility)
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getValidClaimPeriodSchemeClaimWithSubsidizeGroup(ByVal strSchemeCode As String) As SchemeClaimModel

            Dim udtGenFunct As New GeneralFunction()
            Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()
            Return Me.getValidClaimPeriodSchemeClaimWithSubsidizeGroup(strSchemeCode, dtmCurrentDate)

        End Function

        ''' <summary>
        ''' Retrieve Valid Claim Period SchemeClaim with SubsidizeGroup By Scheme Code + Reference Date
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getValidClaimPeriodSchemeClaimWithSubsidizeGroup(ByVal strSchemeCode As String, ByVal dtmCurdate As DateTime) As SchemeClaimModel

            Dim udtSchemeClaim As SchemeClaimModel = Me.getAllValidClaimPeriodSchemeClaimFromCache(dtmCurdate).Filter(strSchemeCode)
            Me.FillValidClaimPeriodSubsidizeGroupFromCache(dtmCurdate, udtSchemeClaim)

            Return udtSchemeClaim

        End Function

        ' CRE13-001 - EHAPP [Start][Koala]
        ' -------------------------------------------------------------------------------------
        ''' <summary>
        ''' Check service date within any subsidize claim period 
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <param name="dtmServicedate"></param>yu
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsServiceDateWithinClaimPeriod(ByVal strSchemeCode As String, ByVal dtmServicedate As DateTime) As Boolean
            Return Me.getAllActiveSubsidizeGroupCache().FilterLastServiceDtm(strSchemeCode, dtmServicedate).Count > 0
        End Function
        ' CRE13-001 - EHAPP [End][Koala]


        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Public Function getLatestValidClaimPeriodSchemeClaimWithSubsidizeGroup(ByVal strSchemeCode As String, ByVal dtmCurdate As DateTime) As SchemeClaimModel

            Dim udtSchemeClaim As SchemeClaimModel = Me.getAllValidClaimPeriodSchemeClaimFromCache(dtmCurdate).Filter(strSchemeCode)
            Me.FillLatestValidClaimPeriodSubsidizeGroupFromCache(dtmCurdate, udtSchemeClaim)

            Return udtSchemeClaim

        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ' -------------------------

        ' Effective Peroid

        ''' <summary>
        ''' Retrieve Single Effective Scheme Claim
        ''' </summary>
        ''' <param name="strSchemeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getEffectiveSchemeClaim(ByVal strSchemeCode As String) As SchemeClaimModel
            Return Me.getAllEffectiveSchemeClaim.Filter(strSchemeCode.Trim())
        End Function

        Public Function getEffectiveSchemeClaimWithSubsidize(ByVal strSchemeCode As String) As SchemeClaimModel
            Dim udtSchemeClaimModel As SchemeClaimModel = Me.getAllEffectiveSchemeClaim.Filter(strSchemeCode)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'udtSchemeClaimModel.SubsidizeGroupClaimList = Me.getAllActiveSubsidizeGroupCache().Filter(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)
            Dim udtGenFunct As New GeneralFunction()
            Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()
            udtSchemeClaimModel.SubsidizeGroupClaimList = Me.getAllActiveSubsidizeGroupCache().FilterLastServiceDtm(udtSchemeClaimModel.SchemeCode, dtmCurrentDate)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            Return udtSchemeClaimModel
        End Function

        ''' <summary>
        ''' Retrieve all Effective SchemeClaim By Current Date
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllEffectiveSchemeClaim() As SchemeClaimModelCollection

            Dim udtGenFunct As New GeneralFunction()
            Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()
            Return Me.getAllEffectiveSchemeClaim(dtmCurrentDate)
        End Function

        ''' <summary>
        ''' Retrieve all Effective SchemeClaim By Reference Date
        ''' </summary>
        ''' <param name="dtmCurDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllEffectiveSchemeClaim(ByVal dtmCurDate As Date) As SchemeClaimModelCollection
            Return Me.getAllEffectiveSchemeClaimFromCache(dtmCurDate)
        End Function

        ' CRE16-026 (Add PCV13)(Hide expired scheme in HCSP Doc Type Scheme popup)[Start][Dickson]
        ''' <summary>
        ''' Retrieve Valid Claim Period SchemeClaim By Current Date
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getValidClaimPeriodScheme() As SchemeClaimModelCollection
            Dim udtGenFunct As New GeneralFunction()
            Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()
            Return Me.getValidClaimPeriodScheme(dtmCurrentDate)
        End Function

        ''' <summary>
        ''' Retrieve Valid Claim Period SchemeClaim By Reference Date
        ''' </summary>
        ''' <param name="dtmCurDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getValidClaimPeriodScheme(ByVal dtmCurDate As Date) As SchemeClaimModelCollection
            Return Me.getAllValidClaimPeriodSchemeClaimFromCache(dtmCurDate)
        End Function
        ' CRE16-026 (Add PCV13)(Hide expired scheme in HCSP Doc Type Scheme popup)[End][Dickson]

        ''' <summary>
        ''' Retrieve all Effect Scheme Claim By SchemeCode List
        ''' </summary>
        ''' <param name="lstStrSchemeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllEffectiveSchemeClaimBySchemeCodeList(ByVal lstStrSchemeCode As List(Of String)) As SchemeClaimModelCollection

            Dim udtResSchemeClaimModelCollection As SchemeClaimModelCollection = New SchemeClaimModelCollection()

            Dim udtGenFunct As New GeneralFunction()
            Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()

            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Me.getAllEffectiveSchemeClaimFromCache(dtmCurrentDate)
            For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimModelCollection
                If lstStrSchemeCode.Contains(udtSchemeClaim.SchemeCode.Trim()) Then
                    udtResSchemeClaimModelCollection.Add(New SchemeClaimModel(udtSchemeClaim))
                End If
            Next

            Return udtResSchemeClaimModelCollection

        End Function

        ''' <summary>
        ''' Retrieve all Effective SchemeClaim, with SubsidizeGroupClaim Info
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllEffectiveSchemeClaim_WithSubsidizeGroup() As SchemeClaimModelCollection

            Dim udtGenFunct As New GeneralFunction()
            Dim dtmCurrentDate = udtGenFunct.GetSystemDateTime()

            Dim udtResSchemeClaimModelList As SchemeClaimModelCollection = Me.getAllEffectiveSchemeClaimFromCache(dtmCurrentDate)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            'Me.FillAllActiveSubsidizeGroupFromCache(udtResSchemeClaimModelList)
            FillLastServiceDtmSubsidizeGroup(dtmCurrentDate, udtResSchemeClaimModelList)
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            Return udtResSchemeClaimModelList

        End Function

        ''' <summary>
        ''' Retrieve all Effective SchemeClaim, with SubsidizeGroupClaim Info, by Reference Date
        ''' </summary>
        ''' <param name="dtmCurDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAllEffectiveSchemeClaim_WithSubsidizeGroup(ByVal dtmCurDate As Date) As SchemeClaimModelCollection
            Dim udtResSchemeClaimModelList As SchemeClaimModelCollection = Me.getAllEffectiveSchemeClaimFromCache(dtmCurDate)
            Me.FillAllActiveSubsidizeGroupFromCache(udtResSchemeClaimModelList)
            Return udtResSchemeClaimModelList

        End Function

        Public Function getSchemeClaimFromBackOfficeUserAndPractice(ByVal strUserID As String, ByVal strFunctionCode As String, ByVal strSPID As String, ByVal intPracticeID As Integer, ByVal dtmServiceDate As DateTime) As SchemeClaimModelCollection
            Dim udtResSchemeClaimModelList As SchemeClaimModelCollection = getSchemeClaim_By_BOUserID_PracticeID_FunctionCode_ServiceDate(strUserID, strFunctionCode, strSPID, intPracticeID, dtmServiceDate)
            Me.FillLastServiceDtmSubsidizeGroup(dtmServiceDate, udtResSchemeClaimModelList)
            Return udtResSchemeClaimModelList

        End Function

        ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Function GetAllEnrolledSchemeClaimWithoutSubsidizeGroup(ByVal strUserID As String, ByVal strFunctionCode As String, ByVal strSPID As String, ByVal intPracticeID As Integer) As SchemeClaimModelCollection
            Dim udtResSchemeClaimModelList As SchemeClaimModelCollection = GetSchemeClaim_By_BOUserID_SPID_PracticeID_FunctionCode(strUserID, strFunctionCode, strSPID, intPracticeID)
            Return udtResSchemeClaimModelList

        End Function
        ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

#End Region

#Region "Private Retrieve"

        ' [Private] SchemeClaim Function

        ''' <summary>
        ''' Retrieve Effective SchemeClaim List (Effective + Expiry)
        ''' </summary>
        ''' <param name="dtmCurrentDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllEffectiveSchemeClaimFromCache(ByVal dtmCurrentDate As DateTime) As SchemeClaimModelCollection
            Dim udtSchemeClaimModelList As SchemeClaimModelCollection = Me.getAllActiveSchemeClaimCache()
            Dim udtResSchemeClaimModelList As SchemeClaimModelCollection = udtSchemeClaimModelList.FilterByEffectivePeriod(dtmCurrentDate)
            Return udtResSchemeClaimModelList
        End Function

        ''' <summary>
        ''' Retrieve Valid Claim Period SchemeClaim List (Claim Period From + To)
        ''' </summary>
        ''' <param name="dtmCurrentDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllValidClaimPeriodSchemeClaimFromCache(ByVal dtmCurrentDate As DateTime) As SchemeClaimModelCollection
            Dim udtSchemeClaimModelList As SchemeClaimModelCollection = Me.getAllActiveSchemeClaimCache()
            Dim udtResSchemeClaimModelList As SchemeClaimModelCollection = udtSchemeClaimModelList.FilterByClaimPeriod(dtmCurrentDate)
            Return udtResSchemeClaimModelList
        End Function

        ' [Private] SubsidizeGroupClaim

        ''' <summary>
        ''' Retrieve Valid Claim Period SubsidizeGroupClaim List (Claim Period From + To)
        ''' </summary>
        ''' <param name="dtmCurrentDate"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllValidSubsidizeGroupClaimFromCache(ByVal dtmCurrentDate As DateTime) As SubsidizeGroupClaimModelCollection
            Dim udtSubsidizeGroupClaimModelList As SubsidizeGroupClaimModelCollection = Me.getAllActiveSubsidizeGroupCache()
            Dim udtResSubsidizeGroupClaimModelList As SubsidizeGroupClaimModelCollection = udtSubsidizeGroupClaimModelList.Filter(dtmCurrentDate)
            Return udtResSubsidizeGroupClaimModelList
        End Function

        ' [Private] Supporting Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ' Remove SchemeSeq
        ''' <summary>
        ''' Fill the Active SubsidizeGroupClaim to SchemeClaim
        ''' </summary>
        ''' <param name="udtSchemeClaimModelList"></param>
        ''' <remarks></remarks>
        Private Sub FillAllActiveSubsidizeGroupFromCache(ByRef udtSchemeClaimModelList As SchemeClaimModelCollection)
            For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelList
                udtSchemeClaimModel.SubsidizeGroupClaimList = Me.getAllActiveSubsidizeGroupCache().Filter(udtSchemeClaimModel.SchemeCode)
            Next
        End Sub
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ' Remove SchemeSeq
        ''' <summary>
        ''' Fill the SubsidizeGroupClaim to SchemeClaim
        ''' </summary>
        ''' <param name="udtSchemeClaimModelList"></param>
        ''' <remarks></remarks>
        Private Sub FillAllSubsidizeGroup(ByRef udtSchemeClaimModelList As SchemeClaimModelCollection)
            For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelList
                udtSchemeClaimModel.SubsidizeGroupClaimList = Me.getAllSubsidizeGroup().Filter(udtSchemeClaimModel.SchemeCode)
            Next
        End Sub
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ''' <summary>
        ''' Fill the Active SubsidizeGroupClaim (By SubsidizeCode) to SchemeClaim
        ''' </summary>
        ''' <param name="strSubsidizeCode"></param>
        ''' <param name="udtSchemeClaimModelList"></param>
        ''' <remarks></remarks>
        Private Sub FillAllActiveSubsidizeGroupFromCache(ByVal strSubsidizeCode As String, ByRef udtSchemeClaimModelList As SchemeClaimModelCollection)
            For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelList

                udtSchemeClaimModel.SubsidizeGroupClaimList = New SubsidizeGroupClaimModelCollection()
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                'Dim udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Me.getAllActiveSubsidizeGroupCache().Filter(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq, strSubsidizeCode)
                'If Not udtResSubsidizeGroupClaimModel Is Nothing Then
                '    udtSchemeClaimModel.SubsidizeGroupClaimList.Add(New SubsidizeGroupClaimModel(udtResSubsidizeGroupClaimModel))
                'End If
                Dim udtResSubsidizeGroupClaimModelCollection As SubsidizeGroupClaimModelCollection = Me.getAllActiveSubsidizeGroupCache().FilterBySchemeCodeAndSubsidizeCode(udtSchemeClaimModel.SchemeCode, strSubsidizeCode)
                For Each udtResSubsidizeGroupClaimModel As SubsidizeGroupClaimModel In udtResSubsidizeGroupClaimModelCollection
                    udtSchemeClaimModel.SubsidizeGroupClaimList.Add(New SubsidizeGroupClaimModel(udtResSubsidizeGroupClaimModel))
                Next
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
            Next
        End Sub

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ' Remove SchemeSeq
        ''' <summary>
        ''' Fill the Valid Claim period SubsidizeGroupClaim to SchemeClaim
        ''' </summary>
        ''' <param name="dtmCurrentDate"></param>
        ''' <param name="udtSchemeClaimModelList"></param>
        ''' <remarks></remarks>
        Private Sub FillValidClaimPeriodSubsidizeGroupFromCache(ByVal dtmCurrentDate As DateTime, ByRef udtSchemeClaimModelList As SchemeClaimModelCollection)
            For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelList
                udtSchemeClaimModel.SubsidizeGroupClaimList = Me.getAllActiveSubsidizeGroupCache().Filter(dtmCurrentDate).Filter(udtSchemeClaimModel.SchemeCode)
            Next
        End Sub
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Not filter by SchemeSeq
        ''' <summary>
        ''' Fill the Valid Claim Period SubsidizeGroupClaim to SchemeClaim
        ''' </summary>
        ''' <param name="dtmCurrentDate"></param>
        ''' <param name="udtSchemeClaimModel"></param>
        ''' <remarks></remarks>
        Private Sub FillValidClaimPeriodSubsidizeGroupFromCache(ByVal dtmCurrentDate As DateTime, ByRef udtSchemeClaimModel As SchemeClaimModel)
            If Not udtSchemeClaimModel Is Nothing Then
                udtSchemeClaimModel.SubsidizeGroupClaimList = Me.getAllActiveSubsidizeGroupCache().Filter(dtmCurrentDate).Filter(udtSchemeClaimModel.SchemeCode)
                'udtSchemeClaimModel.SubsidizeGroupClaimList = Me.getAllActiveSubsidizeGroupCache().Filter(dtmCurrentDate).Filter(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)
            End If
        End Sub
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Not filter by SchemeSeq
        Private Sub FillLastServiceDtmSubsidizeGroup(ByVal dtmCurrentDate As DateTime, ByRef udtSchemeClaimModelList As SchemeClaimModelCollection)
            For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelList
                'udtSchemeClaimModel.SubsidizeGroupClaimList = Me.getAllActiveSubsidizeGroupCache().FilterLastServiceDtm(udtSchemeClaimModel.SchemeCode, udtSchemeClaimModel.SchemeSeq)
                udtSchemeClaimModel.SubsidizeGroupClaimList = Me.getAllActiveSubsidizeGroupCache().FilterLastServiceDtm(udtSchemeClaimModel.SchemeCode, dtmCurrentDate)
            Next
        End Sub
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Private Sub FillLatestValidClaimPeriodSubsidizeGroupFromCache(ByVal dtmCurrentDate As DateTime, ByRef udtSchemeClaimModel As SchemeClaimModel)
            If Not udtSchemeClaimModel Is Nothing Then
                udtSchemeClaimModel.SubsidizeGroupClaimList = Me.getAllActiveSubsidizeGroupCache().FilterLatestClaimPeriodGroupBySubsidizeCode(udtSchemeClaimModel.SchemeCode, dtmCurrentDate)
            End If
        End Sub
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        Private Function getSchemeClaim_By_BOUserID_PracticeID_FunctionCode_ServiceDate(ByVal strUserID As String, ByVal strFunctionCode As String, ByVal strSPID As String, ByVal intPracticeID As Integer, ByVal dtmServiceDate As DateTime, Optional ByVal udtDB As Database = Nothing) As SchemeClaimModelCollection

            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing
            Dim udtSchemeClaimModel As SchemeClaimModel = Nothing

            udtSchemeClaimModelCollection = New SchemeClaimModelCollection()
            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@userid", SqlDbType.VarChar, 20, strUserID), _
                                               udtDB.MakeInParam("@functin_code", SqlDbType.Char, 6, strFunctionCode), _
                                               udtDB.MakeInParam("@sp_id", ServiceProvider.ServiceProviderModel.SPIDDataType, ServiceProvider.ServiceProviderModel.SPIDDataSize, strSPID), _
                                               udtDB.MakeInParam("@practice_id", Practice.PracticeModel.DisplaySeqDataType, Practice.PracticeModel.DisplaySeqDataSize, intPracticeID), _
                                               udtDB.MakeInParam("@service_dtm", SqlDbType.DateTime, 8, dtmServiceDate)}
                udtDB.RunProc("proc_SchemeClaim_get_byBO_SP", prams, dt)

                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows

                        Dim dicControlSetting As New Dictionary(Of String, String)

                        If Not IsDBNull(dr.Item(tableSchemeClaim.ControlSetting)) Then
                            Dim xml As New XmlDocument
                            xml.LoadXml(dr.Item(tableSchemeClaim.ControlSetting).ToString.Trim)

                            For Each node As XmlNode In xml.GetElementsByTagName("Setting")(0).ChildNodes
                                dicControlSetting.Add(node.Name, node.InnerText)
                            Next

                        End If

                        ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                        ' --------------------------------------------------------------------------------------
                        udtSchemeClaimModel = New SchemeClaimModel( _
                            CStr(dr.Item(tableSchemeClaim.Scheme_Code)).Trim(), _
                            CInt(dr.Item(tableSchemeClaim.Scheme_Seq)), _
                            CStr(dr.Item(tableSchemeClaim.Scheme_Desc)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.Scheme_Desc_Chi)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.Scheme_Desc_CN)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.Display_Code)).Trim(), _
                            CInt(dr.Item(tableSchemeClaim.Display_Seq)), _
                            CStr(dr.Item(tableSchemeClaim.BalanceEnquiry_Available)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.IVRS_Available)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.TextOnly_Available)).Trim(), _
                            CType(dr.Item(tableSchemeClaim.Claim_Period_From), DateTime), _
                            CType(dr.Item(tableSchemeClaim.Claim_Period_To), DateTime), _
                            CStr(dr.Item(tableSchemeClaim.Create_By)).Trim(), _
                            CType(dr.Item(tableSchemeClaim.Create_Dtm), DateTime), _
                            CStr(dr.Item(tableSchemeClaim.Update_By)).Trim(), _
                            CType(dr.Item(tableSchemeClaim.Update_Dtm), DateTime), _
                            CStr(dr.Item(tableSchemeClaim.Record_Status)).Trim(), _
                            CType(dr.Item(tableSchemeClaim.Effective_Dtm), DateTime), _
                            CType(dr.Item(tableSchemeClaim.Expiry_Dtm), DateTime), _
                            CStr(dr.Item(tableSchemeClaim.TSWCheckingEnable)), _
                            CStr(dr.Item(tableSchemeClaim.ControlType)).Trim(), _
                            dicControlSetting, _
                            CStr(dr.Item(tableSchemeClaim.ConfirmedTransactionStatus)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.Reimbursement_Mode)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.Reimbursement_Currency)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.Available_HCSP_SubPlatform)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.ProperPractice_Avail)).Trim, _
                            CStr(IIf(IsDBNull(dr(tableSchemeClaim.ProperPractice_SectionID)), String.Empty, dr(tableSchemeClaim.ProperPractice_SectionID))).Trim, _
                            CStr(dr(tableSchemeClaim.Readonly_HCSP))
                            )
                        ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                        udtSchemeClaimModelCollection.Add(udtSchemeClaimModel)
                    Next

                End If
            Catch ex As Exception
                Throw
            End Try

            Return udtSchemeClaimModelCollection
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Private Function GetSchemeClaim_By_BOUserID_SPID_PracticeID_FunctionCode(ByVal strUserID As String, ByVal strFunctionCode As String, ByVal strSPID As String, ByVal intPracticeID As Integer, Optional ByVal udtDB As Database = Nothing) As SchemeClaimModelCollection

            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing
            Dim udtSchemeClaimModel As SchemeClaimModel = Nothing

            udtSchemeClaimModelCollection = New SchemeClaimModelCollection()
            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@userid", SqlDbType.VarChar, 20, strUserID), _
                                               udtDB.MakeInParam("@functin_code", SqlDbType.Char, 6, strFunctionCode), _
                                               udtDB.MakeInParam("@sp_id", ServiceProvider.ServiceProviderModel.SPIDDataType, ServiceProvider.ServiceProviderModel.SPIDDataSize, strSPID), _
                                               udtDB.MakeInParam("@practice_id", Practice.PracticeModel.DisplaySeqDataType, Practice.PracticeModel.DisplaySeqDataSize, intPracticeID)}

                udtDB.RunProc("proc_SchemeClaim_get_EnrolledScheme_bySPID_PracticeID", prams, dt)

                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows

                        Dim dicControlSetting As New Dictionary(Of String, String)

                        If Not IsDBNull(dr.Item(tableSchemeClaim.ControlSetting)) Then
                            Dim xml As New XmlDocument
                            xml.LoadXml(dr.Item(tableSchemeClaim.ControlSetting).ToString.Trim)

                            For Each node As XmlNode In xml.GetElementsByTagName("Setting")(0).ChildNodes
                                dicControlSetting.Add(node.Name, node.InnerText)
                            Next

                        End If

                        udtSchemeClaimModel = New SchemeClaimModel( _
                            CStr(dr.Item(tableSchemeClaim.Scheme_Code)).Trim(), _
                            CInt(dr.Item(tableSchemeClaim.Scheme_Seq)), _
                            CStr(dr.Item(tableSchemeClaim.Scheme_Desc)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.Scheme_Desc_Chi)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.Scheme_Desc_CN)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.Display_Code)).Trim(), _
                            CInt(dr.Item(tableSchemeClaim.Display_Seq)), _
                            CStr(dr.Item(tableSchemeClaim.BalanceEnquiry_Available)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.IVRS_Available)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.TextOnly_Available)).Trim(), _
                            CType(dr.Item(tableSchemeClaim.Claim_Period_From), DateTime), _
                            CType(dr.Item(tableSchemeClaim.Claim_Period_To), DateTime), _
                            CStr(dr.Item(tableSchemeClaim.Create_By)).Trim(), _
                            CType(dr.Item(tableSchemeClaim.Create_Dtm), DateTime), _
                            CStr(dr.Item(tableSchemeClaim.Update_By)).Trim(), _
                            CType(dr.Item(tableSchemeClaim.Update_Dtm), DateTime), _
                            CStr(dr.Item(tableSchemeClaim.Record_Status)).Trim(), _
                            CType(dr.Item(tableSchemeClaim.Effective_Dtm), DateTime), _
                            CType(dr.Item(tableSchemeClaim.Expiry_Dtm), DateTime), _
                            CStr(dr.Item(tableSchemeClaim.TSWCheckingEnable)), _
                            CStr(dr.Item(tableSchemeClaim.ControlType)).Trim(), _
                            dicControlSetting, _
                            CStr(dr.Item(tableSchemeClaim.ConfirmedTransactionStatus)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.Reimbursement_Mode)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.Reimbursement_Currency)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.Available_HCSP_SubPlatform)).Trim(), _
                            CStr(dr.Item(tableSchemeClaim.ProperPractice_Avail)).Trim, _
                            CStr(IIf(IsDBNull(dr(tableSchemeClaim.ProperPractice_SectionID)), String.Empty, dr(tableSchemeClaim.ProperPractice_SectionID))).Trim, _
                            CStr(dr(tableSchemeClaim.Readonly_HCSP))
                            )

                        udtSchemeClaimModelCollection.Add(udtSchemeClaimModel)
                    Next

                End If
            Catch ex As Exception
                Throw
            End Try

            Return udtSchemeClaimModelCollection
        End Function
        ' CRE17-018-07 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]
        
#End Region

#Region "Cache Function"

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ''' <summary>
        ''' Retrieve all SubsidizeFee and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllSubsidizeFeeCache(Optional ByVal udtDB As Database = Nothing) As SubsidizeFeeModelCollection
            Dim udtSubsidizeFeeModelCollection As SubsidizeFeeModelCollection = Nothing
            Dim udtSubsidizeFeeModel As SubsidizeFeeModel = Nothing

            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeFee)) Then
            'udtSubsidizeFeeModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeFee), SubsidizeFeeModelCollection)

            ' Console Schedule Job requires to access Cache by HttpRuntime
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeFee)) Then
                udtSubsidizeFeeModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeFee), SubsidizeFeeModelCollection)
                ' CRE13-006 - HCVS Ceiling [End][Tommy L]
            Else

                udtSubsidizeFeeModelCollection = New SubsidizeFeeModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_SubsidizeFee_get_all", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim dblSubsidizeFee As Nullable(Of Double)

                            If IsDBNull(dr.Item(tableSubsidizeFee.Subsidize_Fee)) Then
                                dblSubsidizeFee = Nothing
                            Else
                                dblSubsidizeFee = CDbl(dr.Item(tableSubsidizeFee.Subsidize_Fee))
                            End If

                            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            Dim strSubsidizeFeeTypeDisplayResource As String = String.Empty

                            If IsDBNull(dr.Item(tableSubsidizeFee.Subsidize_Fee_Type_Display_Resource)) Then
                                strSubsidizeFeeTypeDisplayResource = String.Empty
                            Else
                                strSubsidizeFeeTypeDisplayResource = CStr(dr.Item(tableSubsidizeFee.Subsidize_Fee_Type_Display_Resource)).Trim()
                            End If

                            udtSubsidizeFeeModel = New SubsidizeFeeModel( _
                                CStr(dr.Item(tableSubsidizeFee.Scheme_Code)).Trim(), _
                                CInt(dr.Item(tableSubsidizeFee.Subsidize_Seq)), _
                                CStr(dr.Item(tableSubsidizeFee.Subsidize_Code)).Trim(), _
                                CType(dr.Item(tableSubsidizeFee.Effective_Dtm), DateTime), _
                                CType(dr.Item(tableSubsidizeFee.Expiry_Dtm), DateTime), _
                                dblSubsidizeFee, _
                                CStr(dr.Item(tableSubsidizeFee.Subsidize_Fee_Type)).Trim(), _
                                CInt(dr.Item(tableSubsidizeFee.Subsidize_Fee_Type_Display_Seq)), _
                                strSubsidizeFeeTypeDisplayResource, _
                                CStr(dr.Item(tableSubsidizeFee.Subsidize_Fee_Visible)).Trim(), _
                                CStr(dr.Item(tableSubsidizeFee.Create_By)).Trim(), _
                                CType(dr.Item(tableSubsidizeFee.Create_Dtm), DateTime), _
                                CStr(dr.Item(tableSubsidizeFee.Update_By)).Trim(), _
                                CType(dr.Item(tableSubsidizeFee.Update_Dtm), DateTime))
                            'CRE16-002 (Revamp VSS) [End][Chris YIM]

                            udtSubsidizeFeeModelCollection.Add(udtSubsidizeFeeModel)
                        Next
                    End If

                    'INT13-0033 Missed cache insert [Start][Karl]
                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeFee, udtSubsidizeFeeModelCollection)
                    'INT13-0033 Missed cache insert [End][Karl]

                Catch ex As Exception
                    Throw
                End Try
            End If
            Return udtSubsidizeFeeModelCollection
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ''' <summary>
        ''' Retrieve all Active SubsidizeGroupClaim (Record_Status = 'A') and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllActiveSubsidizeGroupCache(Optional ByVal udtDB As Database = Nothing) As SubsidizeGroupClaimModelCollection

            Dim udtSubsidizeGroupClaimModelCollection As SubsidizeGroupClaimModelCollection = Nothing
            Dim udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing

            ' Console Schedule Job requires to access Cache by HttpRuntime
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupClaim)) Then
                udtSubsidizeGroupClaimModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupClaim), SubsidizeGroupClaimModelCollection)

            Else

                udtSubsidizeGroupClaimModelCollection = New SubsidizeGroupClaimModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_SubsidizeGroupClaimActive_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim intNumSubsidizeCeiling As Nullable(Of Integer) = Nothing

                            If Not IsDBNull(dr.Item(tableSubsidizeGroupClaim.Num_Subsidize_Ceiling)) Then
                                intNumSubsidizeCeiling = CInt(dr.Item(tableSubsidizeGroupClaim.Num_Subsidize_Ceiling))
                            End If

                            Dim intCopayment_Fee As Nullable(Of Integer) = Nothing

                            If Not IsDBNull(dr.Item(tableSubsidizeGroupClaim.Copayment_Fee)) Then
                                intCopayment_Fee = CInt(dr.Item(tableSubsidizeGroupClaim.Copayment_Fee))
                            End If

                            'CRE16-026 (Add PCV13) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            udtSubsidizeGroupClaimModel = New SubsidizeGroupClaimModel( _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Scheme_Code)).Trim(), _
                                CInt(dr.Item(tableSubsidizeGroupClaim.Scheme_Seq)), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Subsidize_Code)).Trim(), _
                                CInt(dr.Item(tableSubsidizeGroupClaim.Display_Seq)), _
                                CType(dr.Item(tableSubsidizeGroupClaim.Claim_Period_From), DateTime), _
                                CType(dr.Item(tableSubsidizeGroupClaim.Claim_Period_To), DateTime), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Consent_Form_Compulsory)).Trim(), _
                                CInt(dr.Item(tableSubsidizeGroupClaim.Num_Subsidize)), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Create_By)).Trim(), _
                                CType(dr.Item(tableSubsidizeGroupClaim.Create_Dtm), DateTime), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Update_By)).Trim(), _
                                CType(dr.Item(tableSubsidizeGroupClaim.Update_Dtm), DateTime), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Record_Status)).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.AdhocPrint_Available)).Trim(), _
                                CType(dr.Item(tableSubsidizeGroupClaim.Last_Service_Dtm), DateTime), _
                                CStr(dr.Item(tableSubsidize.Display_Code)).Trim(), _
                                CStr(dr.Item(tableSubsidize.Lengend_Desc)).Trim(), _
                                CStr(dr.Item(tableSubsidize.Lengend_Desc_Chi)).Trim(), _
                                CStr(dr.Item(tableSubsidize.Lengend_Desc_CN)).Trim(), _
                                CStr(dr.Item(tableSubsidizeItem.Subsidize_Item_Code)).Trim(), _
                                CStr(dr.Item(tableSubsidizeItem.Subsidize_Item_Desc)).Trim(), _
                                CStr(dr.Item(tableSubsidizeItem.Subsidize_item_Desc_Chi)).Trim(), _
                                CStr(dr.Item(tableSubsidizeItem.Subsidize_Type)).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Display_Code_For_Claim)).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Legend_Desc_For_Claim)).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Legend_Desc_For_Claim_Chi)).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Legend_Desc_For_Claim_CN)).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.ClaimDeclaration_Available)).Trim(), _
                                intNumSubsidizeCeiling, _
                                intCopayment_Fee, _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Consent_Form_Avail_Version).ToString).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Consent_Form_Avail_Lang).ToString).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Print_Option_Avail).ToString).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Consent_Form_Avail_Version_CN).ToString).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Consent_Form_Avail_Lang_CN).ToString).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Print_Option_Avail_CN).ToString).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.High_Risk_Option).ToString).Trim() _
                            )
                            'CRE16-026 (Add PCV13) [End][Chris YIM]

                            udtSubsidizeGroupClaimModel.SubsidizeFeeList = getAllSubsidizeFeeCache().Filter(udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeCode)

                            udtSubsidizeGroupClaimModelCollection.Add(udtSubsidizeGroupClaimModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupClaim, udtSubsidizeGroupClaimModelCollection)
                Catch ex As Exception
                    Throw
                End Try
            End If
            Return udtSubsidizeGroupClaimModelCollection
        End Function

        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
        ''' <summary>
        ''' Retrieve all Active SchemeClaim (Record_Status = 'A') and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllActiveSchemeClaimCache(Optional ByVal udtDB As Database = Nothing) As SchemeClaimModelCollection

            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing
            Dim udtSchemeClaimModel As SchemeClaimModel = Nothing

            ' CRE13-006 - HCVS Ceiling [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeClaim)) Then
            'udtSchemeClaimModelCollection = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeClaim), SchemeClaimModelCollection)

            ' Console Schedule Job requires to access Cache by HttpRuntime
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeClaim)) Then
                udtSchemeClaimModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeClaim), SchemeClaimModelCollection)
                ' CRE13-006 - HCVS Ceiling [End][Tommy L]
            Else
                udtSchemeClaimModelCollection = New SchemeClaimModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()
                Try
                    udtDB.RunProc("proc_SchemeClaimActive_get_all_cache", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim dicControlSetting As New Dictionary(Of String, String)

                            If Not IsDBNull(dr.Item(tableSchemeClaim.ControlSetting)) Then
                                Dim xml As New XmlDocument
                                xml.LoadXml(dr.Item(tableSchemeClaim.ControlSetting).ToString.Trim)

                                For Each node As XmlNode In xml.GetElementsByTagName("Setting")(0).ChildNodes
                                    dicControlSetting.Add(node.Name, node.InnerText)
                                Next

                            End If

                            ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                            ' --------------------------------------------------------------------------------------
                            udtSchemeClaimModel = New SchemeClaimModel( _
                                CStr(dr.Item(tableSchemeClaim.Scheme_Code)).Trim(), _
                                CInt(dr.Item(tableSchemeClaim.Scheme_Seq)), _
                                CStr(dr.Item(tableSchemeClaim.Scheme_Desc)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.Scheme_Desc_Chi)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.Scheme_Desc_CN)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.Display_Code)).Trim(), _
                                CInt(dr.Item(tableSchemeClaim.Display_Seq)), _
                                CStr(dr.Item(tableSchemeClaim.BalanceEnquiry_Available)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.IVRS_Available)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.TextOnly_Available)).Trim(), _
                                CType(dr.Item(tableSchemeClaim.Claim_Period_From), DateTime), _
                                CType(dr.Item(tableSchemeClaim.Claim_Period_To), DateTime), _
                                CStr(dr.Item(tableSchemeClaim.Create_By)).Trim(), _
                                CType(dr.Item(tableSchemeClaim.Create_Dtm), DateTime), _
                                CStr(dr.Item(tableSchemeClaim.Update_By)).Trim(), _
                                CType(dr.Item(tableSchemeClaim.Update_Dtm), DateTime), _
                                CStr(dr.Item(tableSchemeClaim.Record_Status)).Trim(), _
                                CType(dr.Item(tableSchemeClaim.Effective_Dtm), DateTime), _
                                CType(dr.Item(tableSchemeClaim.Expiry_Dtm), DateTime), _
                                CStr(dr.Item(tableSchemeClaim.TSWCheckingEnable)), _
                                CStr(dr.Item(tableSchemeClaim.ControlType)).Trim(), _
                                dicControlSetting, _
                                CStr(dr.Item(tableSchemeClaim.ConfirmedTransactionStatus)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.Reimbursement_Mode)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.Reimbursement_Currency)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.Available_HCSP_SubPlatform)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.ProperPractice_Avail)).Trim, _
                                CStr(IIf(IsDBNull(dr(tableSchemeClaim.ProperPractice_SectionID)), String.Empty, dr(tableSchemeClaim.ProperPractice_SectionID))).Trim, _
                                CStr(dr(tableSchemeClaim.Readonly_HCSP))
                                )

                            ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]


                            udtSchemeClaimModelCollection.Add(udtSchemeClaimModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SchemeClaim, udtSchemeClaimModelCollection)

                Catch ex As Exception
                    Throw
                End Try
            End If

            Return udtSchemeClaimModelCollection
        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ''' <summary>
        ''' Retrieve all SubsidizeGroupClaim 
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllSubsidizeGroup(Optional ByVal udtDB As Database = Nothing) As SubsidizeGroupClaimModelCollection

            Dim udtSubsidizeGroupClaimModelCollection As SubsidizeGroupClaimModelCollection = Nothing
            Dim udtSubsidizeGroupClaimModel As SubsidizeGroupClaimModel = Nothing

            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupClaimAll)) Then
                ' Console Schedule Job requires to access Cache by HttpRuntime
                udtSubsidizeGroupClaimModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupClaimAll), SubsidizeGroupClaimModelCollection)
            Else
                udtSubsidizeGroupClaimModelCollection = New SubsidizeGroupClaimModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()

                Try
                    udtDB.RunProc("proc_SubsidizeGroupClaim_get_all", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim intNumSubsidizeCeiling As Nullable(Of Integer) = Nothing

                            If Not IsDBNull(dr.Item(tableSubsidizeGroupClaim.Num_Subsidize_Ceiling)) Then
                                intNumSubsidizeCeiling = CInt(dr.Item(tableSubsidizeGroupClaim.Num_Subsidize_Ceiling))
                            End If

                            Dim intCopayment_Fee As Nullable(Of Integer) = Nothing

                            If Not IsDBNull(dr.Item(tableSubsidizeGroupClaim.Copayment_Fee)) Then
                                intCopayment_Fee = CInt(dr.Item(tableSubsidizeGroupClaim.Copayment_Fee))
                            End If

                            'CRE16-026 (Add PCV13) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            udtSubsidizeGroupClaimModel = New SubsidizeGroupClaimModel( _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Scheme_Code)).Trim(), _
                                CInt(dr.Item(tableSubsidizeGroupClaim.Scheme_Seq)), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Subsidize_Code)).Trim(), _
                                CInt(dr.Item(tableSubsidizeGroupClaim.Display_Seq)), _
                                CType(dr.Item(tableSubsidizeGroupClaim.Claim_Period_From), DateTime), _
                                CType(dr.Item(tableSubsidizeGroupClaim.Claim_Period_To), DateTime), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Consent_Form_Compulsory)).Trim(), _
                                CInt(dr.Item(tableSubsidizeGroupClaim.Num_Subsidize)), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Create_By)).Trim(), _
                                CType(dr.Item(tableSubsidizeGroupClaim.Create_Dtm), DateTime), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Update_By)).Trim(), _
                                CType(dr.Item(tableSubsidizeGroupClaim.Update_Dtm), DateTime), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Record_Status)).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.AdhocPrint_Available)).Trim(), _
                                CType(dr.Item(tableSubsidizeGroupClaim.Last_Service_Dtm), DateTime), _
                                CStr(dr.Item(tableSubsidize.Display_Code)).Trim(), _
                                CStr(dr.Item(tableSubsidize.Lengend_Desc)).Trim(), _
                                CStr(dr.Item(tableSubsidize.Lengend_Desc_Chi)).Trim(), _
                                CStr(dr.Item(tableSubsidize.Lengend_Desc_CN)).Trim(), _
                                CStr(dr.Item(tableSubsidizeItem.Subsidize_Item_Code)).Trim(), _
                                CStr(dr.Item(tableSubsidizeItem.Subsidize_Item_Desc)).Trim(), _
                                CStr(dr.Item(tableSubsidizeItem.Subsidize_item_Desc_Chi)).Trim(), _
                                CStr(dr.Item(tableSubsidizeItem.Subsidize_Type)).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Display_Code_For_Claim)).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Legend_Desc_For_Claim)).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Legend_Desc_For_Claim_Chi)).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Legend_Desc_For_Claim_CN)).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.ClaimDeclaration_Available)).Trim(), _
                                intNumSubsidizeCeiling, _
                                intCopayment_Fee, _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Consent_Form_Avail_Version).ToString).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Consent_Form_Avail_Lang).ToString).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Print_Option_Avail).ToString).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Consent_Form_Avail_Version_CN).ToString).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Consent_Form_Avail_Lang_CN).ToString).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.Print_Option_Avail_CN).ToString).Trim(), _
                                CStr(dr.Item(tableSubsidizeGroupClaim.High_Risk_Option).ToString).Trim() _
                            )
                            'CRE16-026 (Add PCV13) [End][Chris YIM]

                            udtSubsidizeGroupClaimModel.SubsidizeFeeList = getAllSubsidizeFeeCache().Filter(udtSubsidizeGroupClaimModel.SchemeCode, udtSubsidizeGroupClaimModel.SchemeSeq, udtSubsidizeGroupClaimModel.SubsidizeCode)

                            udtSubsidizeGroupClaimModelCollection.Add(udtSubsidizeGroupClaimModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeGroupClaimAll, udtSubsidizeGroupClaimModelCollection)

                Catch ex As Exception
                    Throw
                End Try

            End If

            Return udtSubsidizeGroupClaimModelCollection
        End Function

        ''' <summary>
        ''' Retrieve all SchemeClaim 
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllSchemeClaim(Optional ByVal udtDB As Database = Nothing) As SchemeClaimModelCollection

            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing
            Dim udtSchemeClaimModel As SchemeClaimModel = Nothing

            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeClaimAll)) Then
                ' Console Schedule Job requires to access Cache by HttpRuntime
                udtSchemeClaimModelCollection = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeClaimAll), SchemeClaimModelCollection)
            Else
                udtSchemeClaimModelCollection = New SchemeClaimModelCollection()
                If udtDB Is Nothing Then udtDB = New Database()
                Dim dt As New DataTable()
                Try
                    udtDB.RunProc("proc_SchemeClaim_get_all", dt)

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows

                            Dim dicControlSetting As New Dictionary(Of String, String)

                            If Not IsDBNull(dr.Item(tableSchemeClaim.ControlSetting)) Then
                                Dim xml As New XmlDocument
                                xml.LoadXml(dr.Item(tableSchemeClaim.ControlSetting).ToString.Trim)

                                For Each node As XmlNode In xml.GetElementsByTagName("Setting")(0).ChildNodes
                                    dicControlSetting.Add(node.Name, node.InnerText)
                                Next

                            End If

                            ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                            ' --------------------------------------------------------------------------------------
                            udtSchemeClaimModel = New SchemeClaimModel( _
                                CStr(dr.Item(tableSchemeClaim.Scheme_Code)).Trim(), _
                                CInt(dr.Item(tableSchemeClaim.Scheme_Seq)), _
                                CStr(dr.Item(tableSchemeClaim.Scheme_Desc)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.Scheme_Desc_Chi)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.Scheme_Desc_CN)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.Display_Code)).Trim(), _
                                CInt(dr.Item(tableSchemeClaim.Display_Seq)), _
                                CStr(dr.Item(tableSchemeClaim.BalanceEnquiry_Available)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.IVRS_Available)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.TextOnly_Available)).Trim(), _
                                CType(dr.Item(tableSchemeClaim.Claim_Period_From), DateTime), _
                                CType(dr.Item(tableSchemeClaim.Claim_Period_To), DateTime), _
                                CStr(dr.Item(tableSchemeClaim.Create_By)).Trim(), _
                                CType(dr.Item(tableSchemeClaim.Create_Dtm), DateTime), _
                                CStr(dr.Item(tableSchemeClaim.Update_By)).Trim(), _
                                CType(dr.Item(tableSchemeClaim.Update_Dtm), DateTime), _
                                CStr(dr.Item(tableSchemeClaim.Record_Status)).Trim(), _
                                CType(dr.Item(tableSchemeClaim.Effective_Dtm), DateTime), _
                                CType(dr.Item(tableSchemeClaim.Expiry_Dtm), DateTime), _
                                CStr(dr.Item(tableSchemeClaim.TSWCheckingEnable)), _
                                CStr(dr.Item(tableSchemeClaim.ControlType)).Trim(), _
                                dicControlSetting, _
                                CStr(dr.Item(tableSchemeClaim.ConfirmedTransactionStatus)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.Reimbursement_Mode)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.Reimbursement_Currency)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.Available_HCSP_SubPlatform)).Trim(), _
                                CStr(dr.Item(tableSchemeClaim.ProperPractice_Avail)).Trim, _
                                CStr(IIf(IsDBNull(dr(tableSchemeClaim.ProperPractice_SectionID)), String.Empty, dr(tableSchemeClaim.ProperPractice_SectionID))).Trim, _
                                CStr(dr(tableSchemeClaim.Readonly_HCSP))
                                )
                            ' CRE17-018 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                            udtSchemeClaimModelCollection.Add(udtSchemeClaimModel)
                        Next
                    End If

                    Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SchemeClaimAll, udtSchemeClaimModelCollection)

                Catch ex As Exception
                    Throw

                End Try

            End If

            Return udtSchemeClaimModelCollection

        End Function
        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

        ''' <summary>
        ''' Retrieve All Scheme Back Office - Scheme Claim Mapping, and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllActiveSchemeEnrolClaimMapCache(Optional ByVal udtDB As Database = Nothing) As DataTable

            Dim dt As DataTable = Nothing
            ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [Start][Koala]
            If Not IsNothing(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeEnrolClaimMap)) Then
                dt = CType(HttpRuntime.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeEnrolClaimMap), DataTable)
                'If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeEnrolClaimMap)) Then
                '    dt = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SchemeEnrolClaimMap), DataTable)
                ' CRE17-018-03 Enhancement to meet the new initiatives for VSS and RVP starting from 2018-19 - Phase 3 - Claim [End][Koala]
            Else

                If udtDB Is Nothing Then udtDB = New Database()
                dt = New DataTable()
                Try
                    udtDB.RunProc("proc_SchemeEnrolClaimMap_get_all_cache", dt)
                    If dt.Rows.Count > 0 Then
                        Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SchemeEnrolClaimMap, dt)
                    End If
                Catch ex As Exception
                    Throw
                End Try
            End If
            Return dt
        End Function

        ''' <summary>
        ''' Retrieve All Scheme + Subsidize Back Office = Scheme + Subsidize Claim Mapping, and put in cache
        ''' </summary>
        ''' <param name="udtDB"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getAllActiveSubsidizeEnrolClaimMapCache(Optional ByVal udtDB As Database = Nothing) As DataTable
            Dim dt As DataTable = Nothing

            If Not IsNothing(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeEnrolClaimMap)) Then
                dt = CType(HttpContext.Current.Cache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeEnrolClaimMap), DataTable)
            Else

                If udtDB Is Nothing Then udtDB = New Database()
                dt = New DataTable()
                Try
                    udtDB.RunProc("proc_SubsidizeEnrolClaimMap_get_all_cache", dt)
                    If dt.Rows.Count > 0 Then
                        Common.ComObject.CacheHandler.InsertCache(CACHE_STATIC_DATA.CACHE_ALL_SubsidizeEnrolClaimMap, dt)
                    End If
                Catch ex As Exception
                    Throw
                End Try
            End If
            Return dt
        End Function
#End Region

    End Class
End Namespace
