Imports Common.ComObject
Imports Common.Component
Imports Common.Component.HATransaction
Imports Common.Component.ClaimRules
Imports Common.Component.EHSAccount
Imports Common.Component.Scheme
Imports Common.Component.SchemeDetails
Imports Common.WebService.Interface

Namespace Component.EHSTransaction
    <Serializable()> Public Class TransactionDetailVaccineModelCollection
        Inherits TransactionDetailModelCollection

        Public Enum COVID19Category
            None = 0
            MainCategory = 1
            SubCategory = 2
        End Enum

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtTranDetailVaccineModel As TransactionDetailVaccineModel)
            MyBase.Add(udtTranDetailVaccineModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtTranDetailVaccineModel As TransactionDetailVaccineModel)
            MyBase.Remove(udtTranDetailVaccineModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As TransactionDetailVaccineModel
            Get
                Return CType(MyBase.Item(intIndex), TransactionDetailVaccineModel)
            End Get
        End Property

        Public Function Copy() As TransactionDetailVaccineModelCollection
            Dim udtList As New TransactionDetailVaccineModelCollection()
            For i As Integer = 0 To Count - 1
                udtList.Add(Item(i))
            Next

            Return udtList
        End Function

        ''' <summary>
        ''' Join EHS and HA vaccination record for display / for claim
        ''' </summary>
        ''' <param name="udtHAVaccineModelCollection">HA vaccination records</param>
        ''' <param name="strSchemeCode">If empty value,For display only; If pass scheme code, For claim rule check</param>
        ''' <remarks></remarks>
        Public Sub Join(ByVal udtPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, ByVal udtHAVaccineModelCollection As HAVaccineModelCollection, Optional ByVal strSchemeCode As String = "")
            If udtHAVaccineModelCollection.Count = 0 Then Exit Sub

            ' Initial Mapping Class
            ' ---------------------------------------------------------------------------
            Dim udtHAVaccineBLL As New HAVaccineBLL
            Dim cCode As VaccineCodeMappingCollection = udtHAVaccineBLL.GetAllVaccineCodeMapping()
            Dim cCodeByScheme As VaccineCodeBySchemeMappingCollection = udtHAVaccineBLL.GetAllVaccineCodeBySchemeMapping()
            Dim cDoseSeqCode As VaccineDoseSeqCodeMappingCollection = udtHAVaccineBLL.GetAllVaccineDoseSeqCodeMapping()
            Dim udtCodeModel As VaccineCodeMappingModel
            Dim udtCodeBySchemeModel As VaccineCodeBySchemeMappingModel
            Dim udtDoseSeqCodeModel As VaccineDoseSeqCodeMappingModel

            ' Initial Scheme Class
            ' ---------------------------------------------------------------------------
            Dim udtSchemeBLL As New Common.Component.Scheme.SchemeClaimBLL
            Dim udtSchemeModel As Common.Component.Scheme.SchemeClaimModel
            Dim udtSubsidizeGroupClaimModel As Common.Component.Scheme.SubsidizeGroupClaimModel

            ' Initial Temp object
            ' ---------------------------------------------------------------------------
            Dim strScheme() As String
            Dim udtTransactionDetailModelCopy As TransactionDetailVaccineModel

            ' Convert CMS vaccine record to EHS vaccine record (Map scheme, description, bar desision)
            For Each udtCurrentHAVaccineModel As HAVaccineModel In udtHAVaccineModelCollection

                ' if scheme code provided (ready for use on bar claim) and current HA record is not onsite record, then skip this vaccination
                If strSchemeCode.Length <> 0 And udtCurrentHAVaccineModel.OnSite = HAVaccineModel.OnSiteClass.N Then Continue For

                ' Retrieve vaccine code mapping information
                udtCodeModel = cCode.GetMappingByCode(VaccineCodeMappingModel.SourceSystemClass.CMS, _
                                                            VaccineCodeMappingModel.TargetSystemClass.EHS, _
                                                            udtCurrentHAVaccineModel.VaccineCode, _
                                                            udtCurrentHAVaccineModel.VaccineBrand)
                If udtCodeModel Is Nothing Then
                    Throw New Exception(String.Format("TransactionDetailModelCollection: HA Vaccine Record miss VaccineCodeMapping({0})", udtCurrentHAVaccineModel.VaccineCode))
                End If

                ' if scheme code provided (ready for use on bar claim) and current HA record is not for bar, then skip this vaccination
                If strSchemeCode.Length <> 0 And Not udtCodeModel.ForBar Then
                    Continue For
                End If

                udtTransactionDetailModelCopy = New TransactionDetailVaccineModel(udtPersonalInfo, udtCurrentHAVaccineModel)

                ' --------------------------------------------------------------------------------
                ' Mapping vaccine code & dose seq information to EHS value before join EHS Transaction details
                ' --------------------------------------------------------------------------------
                strScheme = udtCodeModel.VaccineCodeTarget.Split("|")
                udtTransactionDetailModelCopy.SchemeCode = strScheme(0).PadRight(10, " "c)
                udtTransactionDetailModelCopy.SchemeSeq = strScheme(1)
                udtTransactionDetailModelCopy.SubsidizeCode = strScheme(2).PadRight(10, " "c)

                udtTransactionDetailModelCopy.SubsidizeItemCode = strScheme(2)
                udtTransactionDetailModelCopy.SubsidizeDesc = udtCodeModel.VaccineCodeDesc
                udtTransactionDetailModelCopy.SubsidizeDescChi = udtCodeModel.VaccineCodeDescChinese
                udtTransactionDetailModelCopy.ForBar = udtCodeModel.ForBar

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                ' If the HA vaccine is not for Bar, eHS(S) will show as History record "for reference only"
                ' Only pneumococcal vaccine has "not for Bar" setting
                If Not udtTransactionDetailModelCopy.ForBar Then
                    udtTransactionDetailModelCopy.RecordType = TransactionDetailVaccineModel.RecordTypeClass.History
                End If

                ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]

                ' --------------------------------------------------------------------------------
                ' Map source scheme information to target scheme information for claim rule check 
                ' --------------------------------------------------------------------------------
                If strSchemeCode <> String.Empty Then
                    ' Retrieve vaccine code mapping from non scheme code(SIV|1|SIV) to scheme code(CIVSS|1|CIV)
                    udtCodeBySchemeModel = cCodeByScheme.GetMappingByCode(strSchemeCode, udtCodeModel.VaccineCodeTarget)
                    If udtCodeBySchemeModel Is Nothing Then
                        Throw New Exception(String.Format("TransactionDetailModelCollection: HA Vaccine Record miss VaccineCodeBySchemeMapping({0})", strSchemeCode))
                    End If

                    If udtCodeBySchemeModel IsNot Nothing Then
                        strScheme = udtCodeBySchemeModel.VaccineCodeTarget.Split("|")
                        udtTransactionDetailModelCopy.SchemeCode = strScheme(0).PadRight(10, " "c)
                        udtTransactionDetailModelCopy.SchemeSeq = strScheme(1)
                        udtTransactionDetailModelCopy.SubsidizeCode = strScheme(2).PadRight(10, " "c)

                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                        ' Remove SchemeSeq
                        udtSchemeModel = udtSchemeBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtTransactionDetailModelCopy.SchemeCode)
                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                        udtSubsidizeGroupClaimModel = udtSchemeModel.SubsidizeGroupClaimList.Filter(udtTransactionDetailModelCopy.SchemeCode, udtTransactionDetailModelCopy.SchemeSeq, udtTransactionDetailModelCopy.SubsidizeCode)

                        ' SubsidizeGroupClaim maybe inactive if some scheme/subsidize period passed
                        If udtSubsidizeGroupClaimModel IsNot Nothing Then
                            udtTransactionDetailModelCopy.SubsidizeItemCode = udtSubsidizeGroupClaimModel.SubsidizeItemCode
                        End If
                    Else
                        ' Vaccine is not support on current selected scheme, that mean it should not use for Bar
                        udtTransactionDetailModelCopy.ForBar = False
                    End If
                End If


                ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Retrieve vaccine dose seq code mapping information
                udtDoseSeqCodeModel = cDoseSeqCode.GetMappingByCode(VaccineDoseSeqCodeMappingModel.SourceSystemClass.CMS, _
                                                                    VaccineDoseSeqCodeMappingModel.TargetSystemClass.EHS, _
                                                                    udtCurrentHAVaccineModel.DoseSeqCode, _
                                                                    udtTransactionDetailModelCopy.SubsidizeItemCode)
                'udtDoseSeqCodeModel = cDoseSeqCode.GetMappingByCode(VaccineDoseSeqCodeMappingModel.SourceSystemClass.CMS, _
                '                                                    VaccineDoseSeqCodeMappingModel.TargetSystemClass.EHS, _
                '                                                    udtCurrentHAVaccineModel.DoseSeqCode)
                ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]

                If udtDoseSeqCodeModel Is Nothing Then
                    Throw New Exception(String.Format("TransactionDetailModelCollection: HA Vaccine Record miss VaccineDoseSeqCodeMapping({0})", udtCurrentHAVaccineModel.DoseSeqCode))
                End If

                ' --------------------------------------------------------------------------------
                ' CMS result may contain empty dose sequence code
                ' --------------------------------------------------------------------------------
                'If udtTransactionDetailModelCopy.SubsidizeItemCode.Trim.ToUpper = "PV" Then
                '    udtTransactionDetailModelCopy.AvailableItemCode = "VACCINE   "
                'Else
                udtTransactionDetailModelCopy.AvailableItemCode = udtDoseSeqCodeModel.VaccineDoseSeqCodeTarget
                'End If

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                If udtDoseSeqCodeModel.DisplaySourceVaccineDoseDesc Then
                    If udtTransactionDetailModelCopy.AvailableItemDesc = udtDoseSeqCodeModel.VaccineDoseSeqCodeDesc Then
                        udtTransactionDetailModelCopy.AvailableItemDescChi = udtDoseSeqCodeModel.VaccineDoseSeqCodeDescChinese
                    Else
                        udtTransactionDetailModelCopy.AvailableItemDescChi = udtTransactionDetailModelCopy.AvailableItemDesc
                    End If
                Else
                    udtTransactionDetailModelCopy.AvailableItemDesc = udtDoseSeqCodeModel.VaccineDoseSeqCodeDesc
                    udtTransactionDetailModelCopy.AvailableItemDescChi = udtDoseSeqCodeModel.VaccineDoseSeqCodeDescChinese
                End If
                ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]

                ' CRE20-0023 (Immu record) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                udtTransactionDetailModelCopy.NonLocalRecovered = udtCurrentHAVaccineModel.NonLocalRecovery
                ' CRE20-0023 (Immu record) [End][Chris YIM]

                Me.Add(udtTransactionDetailModelCopy)
            Next
        End Sub

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
        ' ----------------------------------------------------------
        ''' <summary>
        ''' Join EHS and DH vaccination record for display / for claim
        ''' </summary>
        ''' <param name="udtDHVaccineModelCollection">DH vaccination records</param>
        ''' <param name="strSchemeCode">If empty value,For display only; If pass scheme code, For claim rule check</param>
        ''' <remarks></remarks>
        Public Sub Join(ByVal udtPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, ByVal udtDHVaccineModelCollection As DHTransaction.DHVaccineModelCollection, Optional ByVal strSchemeCode As String = "")
            If udtDHVaccineModelCollection.Count = 0 Then Exit Sub

            ' Initial Mapping Class
            ' ---------------------------------------------------------------------------
            Dim udtDHVaccineBLL As New DHTransaction.DHVaccineBLL

            Dim cCodeByScheme As HATransaction.VaccineCodeBySchemeMappingCollection = udtDHVaccineBLL.GetAllVaccineCodeBySchemeMapping()
            Dim cDoseSeqCode As HATransaction.VaccineDoseSeqCodeMappingCollection = udtDHVaccineBLL.GetAllVaccineDoseSeqCodeMapping()
            Dim udtHKMTTMappingResult As DHTransaction.DHVaccineBLL.HKMTTMappingResult
            Dim udtVaccineSeasonMapping As DHTransaction.HKMTTVaccineSeasonMappingModel
            Dim udtCodeBySchemeModel As HATransaction.VaccineCodeBySchemeMappingModel
            Dim udtDoseSeqCodeModel As HATransaction.VaccineDoseSeqCodeMappingModel

            ' Initial Scheme Class
            ' ---------------------------------------------------------------------------
            Dim udtSchemeBLL As New Common.Component.Scheme.SchemeClaimBLL
            Dim udtSchemeModel As Common.Component.Scheme.SchemeClaimModel
            Dim udtSubsidizeGroupClaimModel As Common.Component.Scheme.SubsidizeGroupClaimModel

            ' Initial Temp object
            ' ---------------------------------------------------------------------------
            Dim strScheme() As String
            Dim udtTransactionDetailModelCopy As TransactionDetailVaccineModel

            ' Convert CIMS vaccine record to EHS vaccine record (Map scheme, description, bar desision)
            For Each udtCurrentDHVaccineModel As DHTransaction.DHVaccineModel In udtDHVaccineModelCollection

                ' Ignore all invalid dose
                If udtCurrentDHVaccineModel.ValidDoseInd = "N" Then Continue For

                udtTransactionDetailModelCopy = New TransactionDetailVaccineModel(udtPersonalInfo, udtCurrentDHVaccineModel)

                ' Retrieve vaccine code mapping information
                udtHKMTTMappingResult = udtDHVaccineBLL.GetVaccineSeasonMapping(udtCurrentDHVaccineModel)
                udtVaccineSeasonMapping = udtHKMTTMappingResult.HKMTTVaccineSeasonMapping
                If udtVaccineSeasonMapping Is Nothing Then
                    Throw New Exception(String.Format("TransactionDetailModelCollection: DH Vaccine Record miss HKMTTVaccineSeasonMapping({0}|{1})", udtCurrentDHVaccineModel.VaccineL3Iden.HkRegNum, udtCurrentDHVaccineModel.VaccineL3Iden.VaccineProdName))
                End If


                ' if scheme code provided (ready for use on bar claim) and current DH record is not for bar, then skip this vaccination
                If strSchemeCode.Length <> 0 And Not udtVaccineSeasonMapping.ForBar Then
                    Continue For
                End If

                ' --------------------------------------------------------------------------------
                ' Mapping vaccine code & dose seq information to EHS value before join EHS Transaction details
                ' --------------------------------------------------------------------------------
                strScheme = udtVaccineSeasonMapping.VaccineCodeTarget.Split("|")
                udtTransactionDetailModelCopy.SchemeCode = strScheme(0).PadRight(10, " "c)
                udtTransactionDetailModelCopy.SchemeSeq = strScheme(1)
                udtTransactionDetailModelCopy.SubsidizeCode = strScheme(2).PadRight(10, " "c)

                udtTransactionDetailModelCopy.SubsidizeItemCode = strScheme(2)

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                udtTransactionDetailModelCopy.IsUnknownVaccine = udtHKMTTMappingResult.HKMTTVaccineMapping.IsUnknownVaccine

                If udtHKMTTMappingResult.HKMTTVaccineMapping.IsUnknownVaccine Then
                    ' If DH CIMS vaccine is unknown vaccine, display the original vaccine name of DH CIMS vaccine
                    Select Case udtCurrentDHVaccineModel.VaccineIdenType
                        Case DHTransaction.DHVaccineModel.VaccineIdenifierType.L2
                            udtTransactionDetailModelCopy.SubsidizeDesc = udtCurrentDHVaccineModel.VaccineL2Iden.VaccineDesc
                            udtTransactionDetailModelCopy.SubsidizeDescChi = udtCurrentDHVaccineModel.VaccineL2Iden.VaccineDesc
                        Case DHTransaction.DHVaccineModel.VaccineIdenifierType.L3
                            udtTransactionDetailModelCopy.SubsidizeDesc = udtCurrentDHVaccineModel.VaccineL3Iden.VaccineProdName
                            udtTransactionDetailModelCopy.SubsidizeDescChi = udtCurrentDHVaccineModel.VaccineL3Iden.VaccineProdName
                        Case Else
                            Throw New Exception(String.Format("Unknown DHVaccineModel.VaccineIdenType ({0})", udtCurrentDHVaccineModel.VaccineIdenType))
                    End Select
                Else
                    ' If DH CIMS vaccine is known and mapped vaccine, display the mapped vaccine name
                    udtTransactionDetailModelCopy.SubsidizeDesc = udtVaccineSeasonMapping.VaccineCodeDesc
                    udtTransactionDetailModelCopy.SubsidizeDescChi = udtVaccineSeasonMapping.VaccineCodeDescChinese
                End If

                udtTransactionDetailModelCopy.ForBar = udtVaccineSeasonMapping.ForBar

                If udtHKMTTMappingResult.HKMTTVaccineMapping.IsUnknownVaccine And udtHKMTTMappingResult.HKMTTVaccineMapping.VaccineType = DHTransaction.HKMTTVaccineMappingModel.EnumVaccineType.PNEUMOCOCCAL Then
                    udtTransactionDetailModelCopy.RecordType = TransactionDetailVaccineModel.RecordTypeClass.History
                End If
                ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]

                ' --------------------------------------------------------------------------------
                ' Map source scheme information to target scheme information for claim rule check 
                ' --------------------------------------------------------------------------------
                If strSchemeCode <> String.Empty Then
                    ' Retrieve vaccine code mapping from non scheme code(SIV|1|SIV) to scheme code(CIVSS|1|CIV)
                    udtCodeBySchemeModel = cCodeByScheme.GetMappingByCode(strSchemeCode, udtVaccineSeasonMapping.VaccineCodeTarget)
                    If udtCodeBySchemeModel Is Nothing Then
                        Throw New Exception(String.Format("TransactionDetailModelCollection: DH Vaccine Record miss VaccineCodeBySchemeMapping({0})", strSchemeCode))
                    End If

                    If udtCodeBySchemeModel IsNot Nothing Then
                        strScheme = udtCodeBySchemeModel.VaccineCodeTarget.Split("|")
                        udtTransactionDetailModelCopy.SchemeCode = strScheme(0).PadRight(10, " "c)
                        udtTransactionDetailModelCopy.SchemeSeq = strScheme(1)
                        udtTransactionDetailModelCopy.SubsidizeCode = strScheme(2).PadRight(10, " "c)

                        udtSchemeModel = udtSchemeBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtTransactionDetailModelCopy.SchemeCode)
                        udtSubsidizeGroupClaimModel = udtSchemeModel.SubsidizeGroupClaimList.Filter(udtTransactionDetailModelCopy.SchemeCode, udtTransactionDetailModelCopy.SchemeSeq, udtTransactionDetailModelCopy.SubsidizeCode)

                        ' SubsidizeGroupClaim maybe inactive if some scheme/subsidize period passed
                        If udtSubsidizeGroupClaimModel IsNot Nothing Then
                            udtTransactionDetailModelCopy.SubsidizeItemCode = udtSubsidizeGroupClaimModel.SubsidizeItemCode
                        End If
                    Else
                        ' Vaccine is not support on current selected scheme, that mean it should not use for Bar
                        udtTransactionDetailModelCopy.ForBar = False
                    End If
                End If

                ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Retrieve vaccine dose seq code mapping information
                udtDoseSeqCodeModel = cDoseSeqCode.GetMappingByCode(DHTransaction.HKMTTVaccineMappingModel.SourceSystemClass.CIMS, _
                                                                    HATransaction.VaccineDoseSeqCodeMappingModel.TargetSystemClass.EHS, _
                                                                    udtCurrentDHVaccineModel.DoseSeq, _
                                                                    udtTransactionDetailModelCopy.SubsidizeItemCode)
                'udtDoseSeqCodeModel = cDoseSeqCode.GetMappingByCode(DHTransaction.HKMTTVaccineMappingModel.SourceSystemClass.CIMS, _
                '   HATransaction.VaccineDoseSeqCodeMappingModel.TargetSystemClass.EHS, _
                '   udtCurrentDHVaccineModel.DoseSeq)
                ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]

                If udtDoseSeqCodeModel Is Nothing Then
                    Throw New Exception(String.Format("TransactionDetailModelCollection: DH Vaccine Record miss VaccineDoseSeqCodeMapping({0})", udtCurrentDHVaccineModel.DoseSeq))
                End If

                udtTransactionDetailModelCopy.AvailableItemCode = udtDoseSeqCodeModel.VaccineDoseSeqCodeTarget
                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                If udtDoseSeqCodeModel.DisplaySourceVaccineDoseDesc Then
                    If udtTransactionDetailModelCopy.AvailableItemDesc = udtDoseSeqCodeModel.VaccineDoseSeqCodeDesc Then
                        udtTransactionDetailModelCopy.AvailableItemDescChi = udtDoseSeqCodeModel.VaccineDoseSeqCodeDescChinese
                    Else
                        udtTransactionDetailModelCopy.AvailableItemDescChi = udtTransactionDetailModelCopy.AvailableItemDesc
                    End If
                Else
                    udtTransactionDetailModelCopy.AvailableItemDesc = udtDoseSeqCodeModel.VaccineDoseSeqCodeDesc
                    udtTransactionDetailModelCopy.AvailableItemDescChi = udtDoseSeqCodeModel.VaccineDoseSeqCodeDescChinese
                End If
                ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]


                Me.Add(udtTransactionDetailModelCopy)
            Next
        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]


        ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Join EHS and HA vaccination record which will be stored into [StudentFileEntryVaccine] for display and claim
        ''' Keep all vaccine record for display in Vaccine File Report
        ''' </summary>
        ''' <param name="udtHAVaccineModelCollection">HA vaccination records</param>
        ''' <param name="strSchemeCode">If empty value,For display only; If pass scheme code, For claim rule check</param>
        ''' <remarks></remarks>
        Public Sub JoinVaccineForStudent(ByVal udtPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, ByVal udtHAVaccineModelCollection As HAVaccineModelCollection, Optional ByVal strSchemeCode As String = "")
            If udtHAVaccineModelCollection.Count = 0 Then Exit Sub

            ' Initial Mapping Class
            ' ---------------------------------------------------------------------------
            Dim udtHAVaccineBLL As New HAVaccineBLL
            Dim cCode As VaccineCodeMappingCollection = udtHAVaccineBLL.GetAllVaccineCodeMapping()
            Dim cCodeByScheme As VaccineCodeBySchemeMappingCollection = udtHAVaccineBLL.GetAllVaccineCodeBySchemeMapping()
            Dim cDoseSeqCode As VaccineDoseSeqCodeMappingCollection = udtHAVaccineBLL.GetAllVaccineDoseSeqCodeMapping()
            Dim udtCodeModel As VaccineCodeMappingModel
            Dim udtCodeBySchemeModel As VaccineCodeBySchemeMappingModel
            Dim udtDoseSeqCodeModel As VaccineDoseSeqCodeMappingModel

            ' Initial Scheme Class
            ' ---------------------------------------------------------------------------
            Dim udtSchemeBLL As New Common.Component.Scheme.SchemeClaimBLL
            Dim udtSchemeModel As Common.Component.Scheme.SchemeClaimModel
            Dim udtSubsidizeGroupClaimModel As Common.Component.Scheme.SubsidizeGroupClaimModel

            ' Initial Temp object
            ' ---------------------------------------------------------------------------
            Dim strScheme() As String
            Dim udtTransactionDetailModelCopy As TransactionDetailVaccineModel

            ' Convert CMS vaccine record to EHS vaccine record (Map scheme, description, bar desision)
            For Each udtCurrentHAVaccineModel As HAVaccineModel In udtHAVaccineModelCollection

                ' Retrieve vaccine code mapping information
                udtCodeModel = cCode.GetMappingByCode(VaccineCodeMappingModel.SourceSystemClass.CMS, _
                                                            VaccineCodeMappingModel.TargetSystemClass.EHS, _
                                                            udtCurrentHAVaccineModel.VaccineCode, _
                                                            udtCurrentHAVaccineModel.VaccineBrand)
                If udtCodeModel Is Nothing Then
                    Throw New Exception(String.Format("TransactionDetailModelCollection: HA Vaccine Record miss VaccineCodeMapping({0})", udtCurrentHAVaccineModel.VaccineCode))
                End If

                ' Keep "Not For Bar" record for display in Vaccine File Report
                'If strSchemeCode.Length <> 0 And Not udtCodeModel.ForBar Then
                '    Continue For
                'End If

                udtTransactionDetailModelCopy = New TransactionDetailVaccineModel(udtPersonalInfo, udtCurrentHAVaccineModel)

                ' --------------------------------------------------------------------------------
                ' Mapping vaccine code & dose seq information to EHS value before join EHS Transaction details
                ' --------------------------------------------------------------------------------
                strScheme = udtCodeModel.VaccineCodeTarget.Split("|")
                udtTransactionDetailModelCopy.SchemeCode = strScheme(0).PadRight(10, " "c)
                udtTransactionDetailModelCopy.SchemeSeq = strScheme(1)
                udtTransactionDetailModelCopy.SubsidizeCode = strScheme(2).PadRight(10, " "c)

                udtTransactionDetailModelCopy.SubsidizeItemCode = strScheme(2)
                udtTransactionDetailModelCopy.SubsidizeDesc = udtCodeModel.VaccineCodeDesc
                udtTransactionDetailModelCopy.SubsidizeDescChi = udtCodeModel.VaccineCodeDescChinese
                udtTransactionDetailModelCopy.ForBar = udtCodeModel.ForBar

                '' if scheme code provided (ready for use on bar claim) and current HA record is not onsite record, then Not For Bar
                If strSchemeCode.Length <> 0 And udtCurrentHAVaccineModel.OnSite = HAVaccineModel.OnSiteClass.N Then
                    udtTransactionDetailModelCopy.ForBar = False
                End If

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                ' If the HA vaccine is not for Bar, eHS(S) will show as History record "for reference only"
                ' Only pneumococcal vaccine has "not for Bar" setting
                If Not udtTransactionDetailModelCopy.ForBar Then
                    udtTransactionDetailModelCopy.RecordType = TransactionDetailVaccineModel.RecordTypeClass.History
                End If

                ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]

                ' --------------------------------------------------------------------------------
                ' Map source scheme information to target scheme information for claim rule check 
                ' --------------------------------------------------------------------------------
                If strSchemeCode <> String.Empty Then
                    ' Retrieve vaccine code mapping from non scheme code(SIV|1|SIV) to scheme code(CIVSS|1|CIV)
                    udtCodeBySchemeModel = cCodeByScheme.GetMappingByCode(strSchemeCode, udtCodeModel.VaccineCodeTarget)
                    If udtCodeBySchemeModel Is Nothing Then
                        Throw New Exception(String.Format("TransactionDetailModelCollection: HA Vaccine Record miss VaccineCodeBySchemeMapping({0})", strSchemeCode))
                    End If

                    If udtCodeBySchemeModel IsNot Nothing Then
                        strScheme = udtCodeBySchemeModel.VaccineCodeTarget.Split("|")
                        udtTransactionDetailModelCopy.SchemeCode = strScheme(0).PadRight(10, " "c)
                        udtTransactionDetailModelCopy.SchemeSeq = strScheme(1)
                        udtTransactionDetailModelCopy.SubsidizeCode = strScheme(2).PadRight(10, " "c)

                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                        ' Remove SchemeSeq
                        udtSchemeModel = udtSchemeBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtTransactionDetailModelCopy.SchemeCode)
                        ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                        udtSubsidizeGroupClaimModel = udtSchemeModel.SubsidizeGroupClaimList.Filter(udtTransactionDetailModelCopy.SchemeCode, udtTransactionDetailModelCopy.SchemeSeq, udtTransactionDetailModelCopy.SubsidizeCode)

                        ' SubsidizeGroupClaim maybe inactive if some scheme/subsidize period passed
                        If udtSubsidizeGroupClaimModel IsNot Nothing Then
                            udtTransactionDetailModelCopy.SubsidizeItemCode = udtSubsidizeGroupClaimModel.SubsidizeItemCode
                        End If
                    Else
                        ' Vaccine is not support on current selected scheme, that mean it should not use for Bar
                        udtTransactionDetailModelCopy.ForBar = False
                    End If
                End If


                ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Retrieve vaccine dose seq code mapping information
                udtDoseSeqCodeModel = cDoseSeqCode.GetMappingByCode(VaccineDoseSeqCodeMappingModel.SourceSystemClass.CMS, _
                                                                    VaccineDoseSeqCodeMappingModel.TargetSystemClass.EHS, _
                                                                    udtCurrentHAVaccineModel.DoseSeqCode, _
                                                                    udtTransactionDetailModelCopy.SubsidizeItemCode)
                'udtDoseSeqCodeModel = cDoseSeqCode.GetMappingByCode(VaccineDoseSeqCodeMappingModel.SourceSystemClass.CMS, _
                '                                                    VaccineDoseSeqCodeMappingModel.TargetSystemClass.EHS, _
                '                                                    udtCurrentHAVaccineModel.DoseSeqCode)
                ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]

                If udtDoseSeqCodeModel Is Nothing Then
                    Throw New Exception(String.Format("TransactionDetailModelCollection: HA Vaccine Record miss VaccineDoseSeqCodeMapping({0})", udtCurrentHAVaccineModel.DoseSeqCode))
                End If

                ' --------------------------------------------------------------------------------
                ' CMS result may contain empty dose sequence code
                ' --------------------------------------------------------------------------------
                'If udtTransactionDetailModelCopy.SubsidizeItemCode.Trim.ToUpper = "PV" Then
                '    udtTransactionDetailModelCopy.AvailableItemCode = "VACCINE   "
                'Else
                udtTransactionDetailModelCopy.AvailableItemCode = udtDoseSeqCodeModel.VaccineDoseSeqCodeTarget
                'End If

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                If udtDoseSeqCodeModel.DisplaySourceVaccineDoseDesc Then
                    If udtTransactionDetailModelCopy.AvailableItemDesc = udtDoseSeqCodeModel.VaccineDoseSeqCodeDesc Then
                        udtTransactionDetailModelCopy.AvailableItemDescChi = udtDoseSeqCodeModel.VaccineDoseSeqCodeDescChinese
                    Else
                        udtTransactionDetailModelCopy.AvailableItemDescChi = udtTransactionDetailModelCopy.AvailableItemDesc
                    End If
                Else
                    udtTransactionDetailModelCopy.AvailableItemDesc = udtDoseSeqCodeModel.VaccineDoseSeqCodeDesc
                    udtTransactionDetailModelCopy.AvailableItemDescChi = udtDoseSeqCodeModel.VaccineDoseSeqCodeDescChinese
                End If
                ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]

                Me.Add(udtTransactionDetailModelCopy)
            Next
        End Sub

        ''' <summary>
        ''' Join EHS and DH vaccination record which will be stored into [StudentFileEntryVaccine] for display and claim
        ''' Keep all vaccine record for display in Vaccine File Report
        ''' </summary>
        ''' <param name="udtDHVaccineModelCollection">DH vaccination records</param>
        ''' <param name="strSchemeCode"></param>
        ''' <remarks></remarks>
        Public Sub JoinVaccineForStudent(ByVal udtPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, ByVal udtDHVaccineModelCollection As DHTransaction.DHVaccineModelCollection, ByVal strSchemeCode As String)
            If udtDHVaccineModelCollection.Count = 0 Then Exit Sub

            ' Initial Mapping Class
            ' ---------------------------------------------------------------------------
            Dim udtDHVaccineBLL As New DHTransaction.DHVaccineBLL

            Dim cCodeByScheme As HATransaction.VaccineCodeBySchemeMappingCollection = udtDHVaccineBLL.GetAllVaccineCodeBySchemeMapping()
            Dim cDoseSeqCode As HATransaction.VaccineDoseSeqCodeMappingCollection = udtDHVaccineBLL.GetAllVaccineDoseSeqCodeMapping()
            Dim udtHKMTTMappingResult As DHTransaction.DHVaccineBLL.HKMTTMappingResult
            Dim udtVaccineSeasonMapping As DHTransaction.HKMTTVaccineSeasonMappingModel
            Dim udtCodeBySchemeModel As HATransaction.VaccineCodeBySchemeMappingModel
            Dim udtDoseSeqCodeModel As HATransaction.VaccineDoseSeqCodeMappingModel

            ' Initial Scheme Class
            ' ---------------------------------------------------------------------------
            Dim udtSchemeBLL As New Common.Component.Scheme.SchemeClaimBLL
            Dim udtSchemeModel As Common.Component.Scheme.SchemeClaimModel
            Dim udtSubsidizeGroupClaimModel As Common.Component.Scheme.SubsidizeGroupClaimModel

            ' Initial Temp object
            ' ---------------------------------------------------------------------------
            Dim strScheme() As String
            Dim udtTransactionDetailModelCopy As TransactionDetailVaccineModel

            ' Convert CIMS vaccine record to EHS vaccine record (Map scheme, description, bar desision)
            For Each udtCurrentDHVaccineModel As DHTransaction.DHVaccineModel In udtDHVaccineModelCollection

                ' Ignore all invalid dose
                If udtCurrentDHVaccineModel.ValidDoseInd = "N" Then Continue For

                udtTransactionDetailModelCopy = New TransactionDetailVaccineModel(udtPersonalInfo, udtCurrentDHVaccineModel)

                ' Retrieve vaccine code mapping information
                udtHKMTTMappingResult = udtDHVaccineBLL.GetVaccineSeasonMapping(udtCurrentDHVaccineModel)
                udtVaccineSeasonMapping = udtHKMTTMappingResult.HKMTTVaccineSeasonMapping
                If udtVaccineSeasonMapping Is Nothing Then
                    Throw New Exception(String.Format("TransactionDetailModelCollection: DH Vaccine Record miss HKMTTVaccineSeasonMapping({0}|{1})", udtCurrentDHVaccineModel.VaccineL3Iden.HkRegNum, udtCurrentDHVaccineModel.VaccineL3Iden.VaccineProdName))
                End If

                ' Keep "Not For Bar" record for display in Vaccine File Report
                'If strSchemeCode.Length <> 0 And Not udtVaccineSeasonMapping.ForBar Then
                '    Continue For
                'End If

                ' --------------------------------------------------------------------------------
                ' Mapping vaccine code & dose seq information to EHS value before join EHS Transaction details
                ' --------------------------------------------------------------------------------
                strScheme = udtVaccineSeasonMapping.VaccineCodeTarget.Split("|")
                udtTransactionDetailModelCopy.SchemeCode = strScheme(0).PadRight(10, " "c)
                udtTransactionDetailModelCopy.SchemeSeq = strScheme(1)
                udtTransactionDetailModelCopy.SubsidizeCode = strScheme(2).PadRight(10, " "c)

                udtTransactionDetailModelCopy.SubsidizeItemCode = strScheme(2)

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                udtTransactionDetailModelCopy.IsUnknownVaccine = udtHKMTTMappingResult.HKMTTVaccineMapping.IsUnknownVaccine

                If udtHKMTTMappingResult.HKMTTVaccineMapping.IsUnknownVaccine Then
                    ' If DH CIMS vaccine is unknown vaccine, display the original vaccine name of DH CIMS vaccine
                    Select Case udtCurrentDHVaccineModel.VaccineIdenType
                        Case DHTransaction.DHVaccineModel.VaccineIdenifierType.L2
                            udtTransactionDetailModelCopy.SubsidizeDesc = udtCurrentDHVaccineModel.VaccineL2Iden.VaccineDesc
                            udtTransactionDetailModelCopy.SubsidizeDescChi = udtCurrentDHVaccineModel.VaccineL2Iden.VaccineDesc
                        Case DHTransaction.DHVaccineModel.VaccineIdenifierType.L3
                            udtTransactionDetailModelCopy.SubsidizeDesc = udtCurrentDHVaccineModel.VaccineL3Iden.VaccineProdName
                            udtTransactionDetailModelCopy.SubsidizeDescChi = udtCurrentDHVaccineModel.VaccineL3Iden.VaccineProdName
                        Case Else
                            Throw New Exception(String.Format("Unknown DHVaccineModel.VaccineIdenType ({0})", udtCurrentDHVaccineModel.VaccineIdenType))
                    End Select
                Else
                    ' If DH CIMS vaccine is known and mapped vaccine, display the mapped vaccine name
                    udtTransactionDetailModelCopy.SubsidizeDesc = udtVaccineSeasonMapping.VaccineCodeDesc
                    udtTransactionDetailModelCopy.SubsidizeDescChi = udtVaccineSeasonMapping.VaccineCodeDescChinese
                End If

                udtTransactionDetailModelCopy.ForBar = udtVaccineSeasonMapping.ForBar

                If udtHKMTTMappingResult.HKMTTVaccineMapping.IsUnknownVaccine And udtHKMTTMappingResult.HKMTTVaccineMapping.VaccineType = DHTransaction.HKMTTVaccineMappingModel.EnumVaccineType.PNEUMOCOCCAL Then
                    udtTransactionDetailModelCopy.RecordType = TransactionDetailVaccineModel.RecordTypeClass.History
                End If
                ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]

                ' --------------------------------------------------------------------------------
                ' Map source scheme information to target scheme information for claim rule check 
                ' --------------------------------------------------------------------------------
                If strSchemeCode <> String.Empty Then
                    ' Retrieve vaccine code mapping from non scheme code(SIV|1|SIV) to scheme code(CIVSS|1|CIV)
                    udtCodeBySchemeModel = cCodeByScheme.GetMappingByCode(strSchemeCode, udtVaccineSeasonMapping.VaccineCodeTarget)
                    If udtCodeBySchemeModel Is Nothing Then
                        Throw New Exception(String.Format("TransactionDetailModelCollection: DH Vaccine Record miss VaccineCodeBySchemeMapping({0})", strSchemeCode))
                    End If

                    If udtCodeBySchemeModel IsNot Nothing Then
                        strScheme = udtCodeBySchemeModel.VaccineCodeTarget.Split("|")
                        udtTransactionDetailModelCopy.SchemeCode = strScheme(0).PadRight(10, " "c)
                        udtTransactionDetailModelCopy.SchemeSeq = strScheme(1)
                        udtTransactionDetailModelCopy.SubsidizeCode = strScheme(2).PadRight(10, " "c)

                        udtSchemeModel = udtSchemeBLL.getAllActiveSchemeClaimWithSubsidizeGroupBySchemeCodeSchemeSeq(udtTransactionDetailModelCopy.SchemeCode)
                        udtSubsidizeGroupClaimModel = udtSchemeModel.SubsidizeGroupClaimList.Filter(udtTransactionDetailModelCopy.SchemeCode, udtTransactionDetailModelCopy.SchemeSeq, udtTransactionDetailModelCopy.SubsidizeCode)

                        ' SubsidizeGroupClaim maybe inactive if some scheme/subsidize period passed
                        If udtSubsidizeGroupClaimModel IsNot Nothing Then
                            udtTransactionDetailModelCopy.SubsidizeItemCode = udtSubsidizeGroupClaimModel.SubsidizeItemCode
                        End If
                    Else
                        ' Vaccine is not support on current selected scheme, that mean it should not use for Bar
                        udtTransactionDetailModelCopy.ForBar = False
                    End If
                End If

                ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Retrieve vaccine dose seq code mapping information
                udtDoseSeqCodeModel = cDoseSeqCode.GetMappingByCode(DHTransaction.HKMTTVaccineMappingModel.SourceSystemClass.CIMS, _
                                                                    HATransaction.VaccineDoseSeqCodeMappingModel.TargetSystemClass.EHS, _
                                                                    udtCurrentDHVaccineModel.DoseSeq, _
                                                                    udtTransactionDetailModelCopy.SubsidizeItemCode)
                'udtDoseSeqCodeModel = cDoseSeqCode.GetMappingByCode(DHTransaction.HKMTTVaccineMappingModel.SourceSystemClass.CIMS, _
                '   HATransaction.VaccineDoseSeqCodeMappingModel.TargetSystemClass.EHS, _
                '   udtCurrentDHVaccineModel.DoseSeq)
                ' CRE19-005-02 (Share MMR between DH CIMS and eHS(S) - Phase 2 - Interface) [End][Winnie]

                If udtDoseSeqCodeModel Is Nothing Then
                    Throw New Exception(String.Format("TransactionDetailModelCollection: DH Vaccine Record miss VaccineDoseSeqCodeMapping({0})", udtCurrentDHVaccineModel.DoseSeq))
                End If

                udtTransactionDetailModelCopy.AvailableItemCode = udtDoseSeqCodeModel.VaccineDoseSeqCodeTarget
                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                If udtDoseSeqCodeModel.DisplaySourceVaccineDoseDesc Then
                    If udtTransactionDetailModelCopy.AvailableItemDesc = udtDoseSeqCodeModel.VaccineDoseSeqCodeDesc Then
                        udtTransactionDetailModelCopy.AvailableItemDescChi = udtDoseSeqCodeModel.VaccineDoseSeqCodeDescChinese
                    Else
                        udtTransactionDetailModelCopy.AvailableItemDescChi = udtTransactionDetailModelCopy.AvailableItemDesc
                    End If
                Else
                    udtTransactionDetailModelCopy.AvailableItemDesc = udtDoseSeqCodeModel.VaccineDoseSeqCodeDesc
                    udtTransactionDetailModelCopy.AvailableItemDescChi = udtDoseSeqCodeModel.VaccineDoseSeqCodeDescChinese
                End If
                ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]


                Me.Add(udtTransactionDetailModelCopy)
            Next
        End Sub
        ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [End][Winnie]


        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Function JoinVaccineList(ByVal udtPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, _
                                        ByVal udtHAVaccineModelCollection As HATransaction.HAVaccineModelCollection, _
                                        ByVal udtAuditLogEntry As Common.ComObject.AuditLogEntry, _
                                        Optional ByVal strSchemeCode As String = "") As Exception

            Dim udtOriginalCollection As TransactionDetailVaccineModelCollection = Me.Copy

            Try
                Me.Join(udtPersonalInfo, udtHAVaccineModelCollection, strSchemeCode)
            Catch ex As Exception
                ErrorHandler.Log(udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

                'Restore original vaccination record
                Me.Clear()

                For Each udtTransactionDetailVaccineModel As TransactionDetailVaccineModel In udtOriginalCollection
                    Me.Add(udtTransactionDetailVaccineModel)
                Next

                Return ex

            End Try

            Return Nothing

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Function JoinVaccineList(ByVal udtPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, _
                                        ByVal udtDHVaccineModelCollection As DHTransaction.DHVaccineModelCollection, _
                                        ByVal udtAuditLogEntry As Common.ComObject.AuditLogEntry, _
                                        Optional ByVal strSchemeCode As String = "") As Exception

            Dim udtOriginalCollection As TransactionDetailVaccineModelCollection = Me.Copy

            Try
                Me.Join(udtPersonalInfo, udtDHVaccineModelCollection, strSchemeCode)
            Catch ex As Exception
                ErrorHandler.Log(udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

                'Restore original vaccination record
                Me.Clear()

                For Each udtTransactionDetailVaccineModel As TransactionDetailVaccineModel In udtOriginalCollection
                    Me.Add(udtTransactionDetailVaccineModel)
                Next

                Return ex

            End Try

            Return Nothing

        End Function
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' HA
        Public Function JoinVaccineListForStudent(ByVal udtPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, _
                                        ByVal udtHAVaccineModelCollection As HATransaction.HAVaccineModelCollection, _
                                        ByVal udtAuditLogEntry As Common.ComObject.AuditLogEntry, _
                                        Optional ByVal strSchemeCode As String = "") As Exception

            Dim udtOriginalCollection As TransactionDetailVaccineModelCollection = Me.Copy

            Try
                Me.JoinVaccineForStudent(udtPersonalInfo, udtHAVaccineModelCollection, strSchemeCode)
            Catch ex As Exception
                ErrorHandler.Log(udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

                'Restore original vaccination record
                Me.Clear()

                For Each udtTransactionDetailVaccineModel As TransactionDetailVaccineModel In udtOriginalCollection
                    Me.Add(udtTransactionDetailVaccineModel)
                Next

                Return ex

            End Try

            Return Nothing

        End Function

        ' DH
        Public Function JoinVaccineListForStudent(ByVal udtPersonalInfo As EHSAccount.EHSAccountModel.EHSPersonalInformationModel, _
                                                  ByVal udtDHVaccineModelCollection As DHTransaction.DHVaccineModelCollection, _
                                                  ByVal udtAuditLogEntry As Common.ComObject.AuditLogEntry, _
                                                  Optional ByVal strSchemeCode As String = "") As Exception

            Dim udtOriginalCollection As TransactionDetailVaccineModelCollection = Me.Copy

            Try
                Me.JoinVaccineForStudent(udtPersonalInfo, udtDHVaccineModelCollection, strSchemeCode)
            Catch ex As Exception
                ErrorHandler.Log(udtAuditLogEntry.FunctionCode, SeverityCode.SEVE, "99999", HttpContext.Current.Request.PhysicalPath, HttpContext.Current.Request.UserHostAddress, ex.ToString)

                'Restore original vaccination record
                Me.Clear()

                For Each udtTransactionDetailVaccineModel As TransactionDetailVaccineModel In udtOriginalCollection
                    Me.Add(udtTransactionDetailVaccineModel)
                Next

                Return ex

            End Try

            Return Nothing
        End Function
        ' CRE19-025 (Display of unmatched PV for batch upload under RVP) [End][Winnie]

        Public Overloads Sub Sort(ByVal eSortBy As enumSortBy, ByVal oSortDirection As SortDirection)
            Select Case eSortBy
                Case enumSortBy.ServiceDate
                    MyBase.Sort(New ServiceDateComparer(oSortDirection))
            End Select
        End Sub

        ''' <summary>
        ''' Sort TransactionDetailModel by Service date + english subsidize name
        ''' </summary>
        ''' <remarks></remarks>
        Private Class ServiceDateComparer
            Implements System.Collections.IComparer

            Private _oSortDirection As SortDirection

            Public Sub New(ByVal oSortDirection As SortDirection)
                _oSortDirection = oSortDirection
            End Sub

            Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
                Dim iResult As Integer = 0

                If x.GetType Is GetType(TransactionDetailVaccineModel) AndAlso y.GetType Is GetType(TransactionDetailVaccineModel) Then
                    iResult = CType(x, TransactionDetailVaccineModel).ServiceReceiveDtm.CompareTo(CType(y, TransactionDetailVaccineModel).ServiceReceiveDtm)
                    If iResult = 0 Then
                        ' if same service date then sort by subsidize name 
                        ' Subsidize name always sort ascending
                        Return CType(x, TransactionDetailVaccineModel).SubsidizeDesc.CompareTo(CType(y, TransactionDetailVaccineModel).SubsidizeDesc)
                    End If
                Else
                    If x.GetType Is GetType(TransactionDetailModel) Then
                        iResult = -1
                    End If
                    If y.GetType Is GetType(TransactionDetailModel) Then
                        iResult = 1
                    End If
                End If

                Select Case _oSortDirection
                    Case SortDirection.Ascending
                        Return iResult
                    Case SortDirection.Descending
                        Return iResult * -1
                End Select
            End Function
        End Class

        Public Function FilterFindNearestRecord(Optional ByVal strInfoProvider As String = "") As TransactionDetailVaccineModel
            Dim udtResTranDetailVaccineModel As TransactionDetailVaccineModel = Nothing

            For Each udtTranDetailVaccineModel As TransactionDetailVaccineModel In Me
                If strInfoProvider.Trim.ToUpper = udtTranDetailVaccineModel.Provider.Trim.ToUpper OrElse strInfoProvider = String.Empty Then
                    If udtResTranDetailVaccineModel Is Nothing Then
                        udtResTranDetailVaccineModel = udtTranDetailVaccineModel
                    Else
                        If udtResTranDetailVaccineModel.ServiceReceiveDtm < udtTranDetailVaccineModel.ServiceReceiveDtm Then
                            udtResTranDetailVaccineModel = udtTranDetailVaccineModel
                        End If

                        If udtResTranDetailVaccineModel.ServiceReceiveDtm = udtTranDetailVaccineModel.ServiceReceiveDtm Then
                            If udtResTranDetailVaccineModel.TransactionDtm < udtTranDetailVaccineModel.TransactionDtm Then
                                udtResTranDetailVaccineModel = udtTranDetailVaccineModel
                            End If
                        End If
                    End If
                End If
            Next

            Return udtResTranDetailVaccineModel

        End Function

        ' CRE20-023-60 (Immu record - 3rd Dose) [Start][Winnie SUEN]
        ' -------------------------------------------------------------
        Public Function FilterFindNearestRecordByDose(ByVal strAvailableItemCode As String) As TransactionDetailVaccineModel

            Dim udtResTranDetailVaccineModel As TransactionDetailVaccineModel = Nothing

            For Each udtTranDetailVaccineModel As TransactionDetailVaccineModel In Me
                If (strAvailableItemCode.Trim.ToUpper = udtTranDetailVaccineModel.AvailableItemCode.Trim.ToUpper) Then

                    If udtResTranDetailVaccineModel Is Nothing Then
                        udtResTranDetailVaccineModel = udtTranDetailVaccineModel
                    Else
                        If udtResTranDetailVaccineModel.ServiceReceiveDtm <= udtTranDetailVaccineModel.ServiceReceiveDtm Then
                            udtResTranDetailVaccineModel = udtTranDetailVaccineModel
                        End If
                    End If
                End If
            Next

            Return udtResTranDetailVaccineModel

        End Function
        ' CRE20-023-60 (Immu record - 3rd Dose) [End][Winnie SUEN]

        Public Function FilterIncludeBySubsidizeItemCode(ByVal strSubsidizeItemCode As String) As TransactionDetailVaccineModelCollection
            Dim udtResTranDetailVaccineList As New TransactionDetailVaccineModelCollection

            For Each udtTranDetailVaccineModel As TransactionDetailVaccineModel In Me
                If udtTranDetailVaccineModel.SubsidizeItemCode = strSubsidizeItemCode Then
                    udtResTranDetailVaccineList.Add(udtTranDetailVaccineModel)
                End If
            Next

            Return udtResTranDetailVaccineList

        End Function

        Public Function FilterExcludeBySubsidizeItemCode(ByVal strSubsidizeItemCode As String) As TransactionDetailVaccineModelCollection
            Dim udtResTranDetailVaccineList As New TransactionDetailVaccineModelCollection

            For Each udtTranDetailVaccineModel As TransactionDetailVaccineModel In Me
                If udtTranDetailVaccineModel.SubsidizeItemCode <> strSubsidizeItemCode Then
                    udtResTranDetailVaccineList.Add(udtTranDetailVaccineModel)
                End If
            Next

            Return udtResTranDetailVaccineList

        End Function

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function FilterFindNearestCategary() As Dictionary(Of COVID19Category, String)
            Dim udtResTranDetailVaccineModel As TransactionDetailVaccineModel = Nothing
            Dim dicNearestCategory As New Dictionary(Of COVID19Category, String)

            For Each udtTranDetailVaccineModel As TransactionDetailVaccineModel In Me
                If udtResTranDetailVaccineModel Is Nothing Then
                    If udtTranDetailVaccineModel.MainCategory <> String.Empty Then
                        'Vaccine
                        udtResTranDetailVaccineModel = udtTranDetailVaccineModel

                        'Category
                        dicNearestCategory.Add(COVID19Category.MainCategory, udtResTranDetailVaccineModel.MainCategory)
                        dicNearestCategory.Add(COVID19Category.SubCategory, udtResTranDetailVaccineModel.SubCategory)

                    End If

                Else
                    If udtResTranDetailVaccineModel.ServiceReceiveDtm < udtTranDetailVaccineModel.ServiceReceiveDtm Then
                        'Vaccine
                        udtResTranDetailVaccineModel = udtTranDetailVaccineModel

                        'Category
                        If udtResTranDetailVaccineModel.MainCategory <> String.Empty Then
                            dicNearestCategory.Remove(COVID19Category.MainCategory)
                            dicNearestCategory.Remove(COVID19Category.SubCategory)
                            dicNearestCategory.Add(COVID19Category.MainCategory, udtResTranDetailVaccineModel.MainCategory)
                            dicNearestCategory.Add(COVID19Category.SubCategory, udtResTranDetailVaccineModel.SubCategory)
                        End If

                    End If

                    If udtResTranDetailVaccineModel.ServiceReceiveDtm = udtTranDetailVaccineModel.ServiceReceiveDtm Then
                        If udtResTranDetailVaccineModel.TransactionDtm < udtTranDetailVaccineModel.TransactionDtm Then
                            'Vaccine
                            udtResTranDetailVaccineModel = udtTranDetailVaccineModel

                            'Category
                            If udtResTranDetailVaccineModel.MainCategory <> String.Empty Then
                                dicNearestCategory.Remove(COVID19Category.MainCategory)
                                dicNearestCategory.Remove(COVID19Category.SubCategory)
                                dicNearestCategory.Add(COVID19Category.MainCategory, udtResTranDetailVaccineModel.MainCategory)
                                dicNearestCategory.Add(COVID19Category.SubCategory, udtResTranDetailVaccineModel.SubCategory)
                            End If

                        End If
                    End If

                End If
            Next

            Return dicNearestCategory

        End Function
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function FilterFindNearestContactNo() As String
            Dim udtResTranDetailVaccineModel As TransactionDetailVaccineModel = Nothing
            Dim strNearestContactNo As String = String.Empty

            For Each udtTranDetailVaccineModel As TransactionDetailVaccineModel In Me
                If udtResTranDetailVaccineModel Is Nothing Then
                    If udtTranDetailVaccineModel.ContactNo <> String.Empty Then
                        'Vaccine
                        udtResTranDetailVaccineModel = udtTranDetailVaccineModel
                        'Contact No.
                        strNearestContactNo = udtResTranDetailVaccineModel.ContactNo
                    End If

                Else
                    If udtResTranDetailVaccineModel.ServiceReceiveDtm < udtTranDetailVaccineModel.ServiceReceiveDtm Then
                        'Vaccine
                        udtResTranDetailVaccineModel = udtTranDetailVaccineModel

                        'Contact No.
                        If udtResTranDetailVaccineModel.ContactNo <> String.Empty Then
                            strNearestContactNo = udtResTranDetailVaccineModel.ContactNo
                        End If

                    End If

                    If udtResTranDetailVaccineModel.ServiceReceiveDtm = udtTranDetailVaccineModel.ServiceReceiveDtm Then
                        If udtResTranDetailVaccineModel.TransactionDtm < udtTranDetailVaccineModel.TransactionDtm Then
                            'Vaccine
                            udtResTranDetailVaccineModel = udtTranDetailVaccineModel

                            'Contact No.
                            If udtResTranDetailVaccineModel.ContactNo <> String.Empty Then
                                strNearestContactNo = udtResTranDetailVaccineModel.ContactNo
                            End If
                        End If
                    End If

                End If
            Next

            Return strNearestContactNo

        End Function
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function FilterFindNearestRecipientType() As String
            Dim udtResTranDetailVaccineModel As TransactionDetailVaccineModel = Nothing
            Dim strNearestRecipientType As String = String.Empty

            For Each udtTranDetailVaccineModel As TransactionDetailVaccineModel In Me
                If udtResTranDetailVaccineModel Is Nothing Then
                    If udtTranDetailVaccineModel.RecipientType <> String.Empty Then
                        'Vaccine
                        udtResTranDetailVaccineModel = udtTranDetailVaccineModel
                        'Recipient Type
                        strNearestRecipientType = udtResTranDetailVaccineModel.RecipientType
                    End If

                Else
                    If udtResTranDetailVaccineModel.ServiceReceiveDtm < udtTranDetailVaccineModel.ServiceReceiveDtm Then
                        'Vaccine
                        udtResTranDetailVaccineModel = udtTranDetailVaccineModel

                        'Recipient Type
                        If udtResTranDetailVaccineModel.RecipientType <> String.Empty Then
                            strNearestRecipientType = udtResTranDetailVaccineModel.RecipientType
                        End If

                    End If

                    If udtResTranDetailVaccineModel.ServiceReceiveDtm = udtTranDetailVaccineModel.ServiceReceiveDtm Then
                        If udtResTranDetailVaccineModel.TransactionDtm < udtTranDetailVaccineModel.TransactionDtm Then
                            'Vaccine
                            udtResTranDetailVaccineModel = udtTranDetailVaccineModel

                            'Recipient Type
                            If udtResTranDetailVaccineModel.RecipientType <> String.Empty Then
                                strNearestRecipientType = udtResTranDetailVaccineModel.RecipientType
                            End If
                        End If
                    End If

                End If
            Next

            Return strNearestRecipientType

        End Function
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function FilterJoinEHRSSHistory() As Boolean
            Dim blnRes As Boolean = False

            For Each udtTranDetailVaccineModel As TransactionDetailVaccineModel In Me
                If udtTranDetailVaccineModel.JoinEHRSS = YesNo.Yes Then
                    blnRes = True
                End If
            Next

            Return blnRes

        End Function
        ' CRE20-0023 (Immu record) [End][Chris YIM]

        ' CRE20-0023 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        Public Function FilterNonLocalRecoveredHistory() As Boolean
            Dim blnRes As Boolean = False

            For Each udtTranDetailVaccineModel As TransactionDetailVaccineModel In Me
                If udtTranDetailVaccineModel.NonLocalRecovered = YesNo.Yes Then
                    blnRes = True
                End If
            Next

            Return blnRes

        End Function
        ' CRE20-0023 (Immu record) [End][Chris YIM]

    End Class
End Namespace
