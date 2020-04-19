Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Component

Imports Common.Component.Scheme

Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.Component.BankAcct
Imports Common.Component.Professional
Imports Common.Component.Address
Imports Common.Component.District
Imports Common.Component.Area
Imports Common.Component.SchemeInformation
Imports Common.Component.MedicalOrganization

Imports Common.Component.ERNProcessed
Imports Common.Component.PracticeSchemeInfo

Imports Common.Component.StaticData

' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

' -----------------------------------------------------------------------------------------

Imports Common.Component.Profession

' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

Imports System.Web.UI

Imports Common.Validation
Imports Common.ComFunction

Public Class eFormBLL
    Private _strERN As String
    Private validator As Common.Validation.Validator = New Common.Validation.Validator
    Private Formatter As Common.Format.Formatter = New Common.Format.Formatter
    Private GeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

    Private udtDistrictBLL As DistrictBLL = New DistrictBLL
    Private udtAreaBLL As AreaBLL = New AreaBLL

    Private udtStaticDataBLL As StaticDataBLL = New StaticDataBLL

    ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

    ' -----------------------------------------------------------------------------------------

    Private udtProfessionBLL As ProfessionBLL = New ProfessionBLL

    ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

    Private udtSchemeEFormBLL As SchemeEFormBLL = New SchemeEFormBLL

    Private udtDB As Database = New Database()

    ' CRE17-015-02 (Disallow public using WinXP) [Start][Chris YIM]
    ' ----------------------------------------------------------
    Public Const SESS_Disclaimer As String = "eForm_Disclaimer"
    ' CRE17-015-02 (Disallow public using WinXP) [End][Chris YIM]
    Public Const SESS_PersonalParticular As String = "eForm_PersonalParticular"
    Public Const SESS_MedicalOrganization As String = "eForm_MedicalOrganization"
    Public Const SESS_Practice As String = "eForm_Partice"
    Public Const SESS_Bank As String = "eForm_Bank"
    Public Const SESS_SchemeSelection As String = "eForm_Scheme"
    Public Const SESS_ConfirmDetails As String = "eForm_ConfirmDetails"

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private Class ClinicType
        Public Const Clinic As String = "C"
        Public Const NonClinic As String = "N"
    End Class
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    Public Property DB() As Database
        Get
            Return udtDB
        End Get
        Set(ByVal Value As Database)
            udtDB = Value
        End Set
    End Property

    Public Property EnroRefNo() As String
        Get
            Return _strERN
        End Get
        Set(ByVal value As String)
            _strERN = value
        End Set
    End Property

    Public Sub New()

    End Sub

    'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'Private Sub UpdateMOScheme(ByVal blnHCVS As Boolean, ByVal blnIVSS As Boolean, ByVal dtPractice As DataTable)

    '    If Not IsNothing(dtPractice) Then
    '        If dtPractice.Rows.Count > 0 Then
    '            For Each rowP As DataRow In dtPractice.Rows
    '                Dim strHealthProf As String = rowP.Item("ServiceCategoryCode")
    '                If AskJoinIVSS(strHealthProf.Trim) Then
    '                    If blnIVSS Then
    '                        rowP.Item("IVSS") = "Y"
    '                    Else
    '                        rowP.Item("IVSS") = "N"
    '                    End If

    '                End If

    '                If blnHCVS Then
    '                    rowP.Item("HCVS") = "Y"
    '                Else
    '                    rowP.Item("HCVS") = "N"
    '                End If
    '            Next
    '        End If
    '    End If
    'End Sub
    'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]

    Public Sub convertToPracticeBankProfModelCollection(ByVal strERN As String, ByVal udtInputSchemeList As SchemeInformationModelCollection, ByVal dt As DataTable, ByVal blnCreateBank As Boolean, _
                                                        ByRef udtSchemeList As SchemeInformationModelCollection, ByRef udtPracticeList As PracticeModelCollection, ByRef udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, _
                                                        ByRef udtBankList As BankAcctModelCollection, ByRef udtProfList As ProfessionalModelCollection, ByVal udtThirdPartyEnrolmentCollection As ThirdParty.ThirdPartyAdditionalFieldEnrolmentCollection, ByRef udtThirdPartyEnrolmentList As ThirdParty.ThirdPartyAdditionalFieldEnrolmentCollection, ByVal intIndex As Integer)
        udtSchemeList = Nothing
        udtPracticeList = Nothing
        udtPracticeSchemeInfoList = Nothing
        udtBankList = Nothing
        udtProfList = Nothing
        udtThirdPartyEnrolmentList = Nothing

        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL

        If Not IsNothing(dt) Then
            If dt.Rows.Count > 0 Then
                Dim intPIndex As Integer = 1
                Dim intProfIndex As Integer = 1
                Dim intBankIndex As Integer = 1

                udtSchemeList = New SchemeInformationModelCollection
                udtPracticeList = New PracticeModelCollection
                udtPracticeSchemeInfoList = New PracticeSchemeInfoModelCollection
                udtBankList = New BankAcctModelCollection
                udtProfList = New ProfessionalModelCollection
                udtThirdPartyEnrolmentList = New ThirdParty.ThirdPartyAdditionalFieldEnrolmentCollection

                Dim udtPracticeModel As PracticeModel = Nothing
                Dim udtPracticeSchemeInfo As PracticeSchemeInfoModel = Nothing
                Dim udtProfessionalModel As ProfessionalModel = Nothing
                Dim udtBankModel As BankAcctModel = Nothing
                Dim udtThirdPartyEnrolmentModel As ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel = Nothing

                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                    Dim udtSchemeEFormList As SchemeEFormModelCollection = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                    For Each udtSchemeModel As SchemeInformationModel In udtInputSchemeList.Values
                        Dim udtSchemeEForm As SchemeEFormModel

                        udtSchemeEForm = udtSchemeEFormList.Filter(udtSchemeModel.SchemeCode.Trim)

                        Dim blnSchemeAdd As Boolean = False

                        For Each dr As DataRow In dt.Rows
                            If CStr(dr.Item(udtSchemeEForm.SchemeCode + "_EligibleForProfession")).Trim = YesNo.Yes And _
                                CStr(dr.Item(udtSchemeEForm.SchemeCode + "_Selected")).Trim = YesNo.Yes Then
                                blnSchemeAdd = True
                            End If
                        Next

                        If blnSchemeAdd Then
                            Dim udtNewScheme As SchemeInformationModel = New SchemeInformationModel

                            udtSchemeInfoBLL.Clone(udtNewScheme, udtSchemeModel)
                            udtNewScheme.EnrolRefNo = strERN

                            udtSchemeList.Add(udtNewScheme)
                        End If

                    Next
                End If
                'CRE16-002 (Revamp VSS) [End][Chris YIM]

                For Each row As DataRow In dt.Rows
                    If CInt(row.Item("MOIndex")) - 1 = intIndex Then
                        intBankIndex = 1
                        intProfIndex = udtProfList.GetProfessionalSeq(row.Item("ServiceCategoryCode"), row.Item("RegistrationCode"))
                        If intProfIndex > udtProfList.Count Then

                            udtProfessionalModel = New ProfessionalModel(String.Empty, strERN, intProfIndex, row.Item("ServiceCategoryCode"), _
                                                row.Item("RegistrationCode"), String.Empty, Nothing, String.Empty)

                            If Not IsNothing(udtProfessionalModel) Then
                                udtProfList.Add(udtProfessionalModel)
                            End If
                        End If

                        ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                        udtPracticeModel = New PracticeModel(String.Empty, strERN, intPIndex, 1, row.Item("PracticeName"), row.Item("PracticeNameChi"), _
                                                            New AddressModel(row.Item("Room"), row.Item("Floor"), row.Item("Block"), row.Item("Building"), row.Item("ChiBuilding"), row.Item("District"), Nothing), _
                                                            intProfIndex, String.Empty, SubmitChannel.Electronic, String.Empty, _
                                                            row.Item("PhoneDaytime"), Nothing, String.Empty, Nothing, String.Empty, Nothing, YesNo.No, String.Empty, String.Empty, _
                                                            Nothing, Nothing, Nothing)
                        ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

                        udtPracticeList.Add(udtPracticeModel)

                        Dim udtTempThirdPartyAdditionalFieldEnrolmentModel As ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel = Nothing

                        If Not IsNothing(udtThirdPartyEnrolmentCollection) Then
                            For Each udtThirdPartyEnrolmentModel In udtThirdPartyEnrolmentCollection.Values
                                If udtThirdPartyEnrolmentModel.PracticeDisplaySeq = CType(row.Item("PracticeIndex"), Integer) + 1 Then
                                    udtTempThirdPartyAdditionalFieldEnrolmentModel = New ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel(udtThirdPartyEnrolmentModel)
                                    udtTempThirdPartyAdditionalFieldEnrolmentModel.EnrolRefNo = strERN
                                    udtTempThirdPartyAdditionalFieldEnrolmentModel.PracticeDisplaySeq = intPIndex
                                    udtThirdPartyEnrolmentList.Add(udtTempThirdPartyAdditionalFieldEnrolmentModel)
                                End If
                            Next
                        End If

                        If blnCreateBank Then
                            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                            udtBankModel = New BankAcctModel(String.Empty, strERN, intBankIndex, intPIndex, row.Item("Bank"), row.Item("Branch"), _
                                                                                row.Item("Holder"), Formatter.formatBankAcct(row.Item("BankCode"), row.Item("BranchCode"), row.Item("BankAcc")), _
                                                                                String.Empty, SubmitChannel.Electronic, String.Empty, Nothing, String.Empty, Nothing, String.Empty, Nothing, YesNo.No)
                            'CRE13-019-02 Extend HCVS to China [End][Winnie]
                            If Not IsNothing(udtBankModel) Then
                                udtBankList.Add(udtBankModel)
                            End If

                        End If


                        If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                            Dim udtSchemeEFormList As SchemeEFormModelCollection = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup

                            For Each udtSchemeModel As SchemeInformationModel In udtInputSchemeList.Values
                                Dim udtSchemeEForm As SchemeEFormModel

                                udtSchemeEForm = udtSchemeEFormList.Filter(udtSchemeModel.SchemeCode.Trim)

                                If Not IsNothing(udtSchemeEForm) Then
                                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                                    '-----------------------------------------------------------------------------------------
                                    If row(udtSchemeEForm.SchemeCode.Trim + "_EligibleForProfession") = YesNo.Yes And _
                                        row(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.Yes Then
                                        If udtSchemeEForm.EligibleProfesional(CStr(row.Item("ServiceCategoryCode")).Trim) Then

                                            Dim strClinicType As String = String.Empty

                                            If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes AndAlso CStr(row.Item(udtSchemeEForm.SchemeCode.ToString.Trim + "_NonClinicSetting_Selected")).Trim.Equals(YesNo.Yes) Then
                                                strClinicType = ClinicType.NonClinic
                                            Else
                                                strClinicType = ClinicType.Clinic
                                            End If

                                            For Each udtSubsidizeGroupEForm As SubsidizeGroupEFormModel In udtSchemeEForm.SubsidizeGroupEFormList
                                                If Not IsNothing(udtSubsidizeGroupEForm) Then
                                                    Dim intServiceFee As Nullable(Of Integer)
                                                    Dim strProvideServiceFee As String = String.Empty
                                                    Dim strProvideService As String = String.Empty

                                                    'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                                                    '-----------------------------------------------------------------------------------------
                                                    If udtSubsidizeGroupEForm.ServiceFeeEnabled Then
                                                        'If CStr(row.Item(udtSubsidizeGroupEForm.SubsidizeCode.Trim)).Trim.Equals(String.Empty) Then
                                                        If CStr(row.Item(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsProvided")).Trim.Equals(YesNo.Yes) Then
                                                            strProvideService = YesNo.Yes
                                                        Else
                                                            strProvideService = YesNo.No
                                                        End If

                                                        If CStr(row.Item(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_ServiceFee")).Trim.Equals(String.Empty) Then
                                                            intServiceFee = Nothing
                                                            strProvideServiceFee = String.Empty
                                                            'ElseIf CStr(row.Item(udtSubsidizeGroupEForm.SubsidizeCode.Trim)).Trim.Equals(strNotProvided) Then
                                                        ElseIf CStr(row.Item(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsNoServiceFee")).Trim.Equals(YesNo.Yes) Then

                                                            intServiceFee = Nothing
                                                            'strProvideServiceFee = "N"
                                                            strProvideServiceFee = YesNo.No
                                                        Else
                                                            'intServiceFee = CInt(row.Item(udtSubsidizeGroupEForm.SubsidizeCode.Trim))
                                                            intServiceFee = CInt(row.Item(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_ServiceFee"))
                                                            'strProvideServiceFee = "Y"
                                                            strProvideServiceFee = YesNo.Yes
                                                        End If
                                                    Else
                                                        intServiceFee = Nothing
                                                        strProvideServiceFee = String.Empty
                                                        strProvideService = YesNo.Yes
                                                    End If
                                                    'CRE15-004 (TIV and QIV) [End][Chris YIM]

                                                    'udtPracticeSchemeInfoList.Add(New PracticeSchemeInfoModel(String.Empty, _
                                                    '                                strERN, _
                                                    '                                intPIndex, _
                                                    '                                udtSubsidizeGroupEForm.SchemeCode.Trim, _
                                                    '                                intServiceFee, _
                                                    '                                String.Empty, _
                                                    '                                String.Empty, _
                                                    '                                String.Empty, _
                                                    '                                Nothing, _
                                                    '                                String.Empty, _
                                                    '                                Nothing, _
                                                    '                                String.Empty, _
                                                    '                                Nothing, _
                                                    '                                Nothing, _
                                                    '                                Nothing, _
                                                    '                                udtSubsidizeGroupEForm.SubsidizeCode.Trim, _
                                                    '                                strProvideServiceFee, _
                                                    '                                udtSchemeEForm.DisplaySeq, _
                                                    '                                udtSubsidizeGroupEForm.DisplaySeq, _
                                                    '                                strProvideService))

                                                    ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
                                                    ' ----------------------------------------------------------------------------------------
                                                    '' Mark Practice Scheme Info Record Status as "A" for new enrolment (Before empty)

                                                    'udtPracticeSchemeInfoList.Add(New PracticeSchemeInfoModel(String.Empty, _
                                                    '                                strERN, _
                                                    '                                intPIndex, _
                                                    '                                udtSubsidizeGroupEForm.SchemeCode.Trim, _
                                                    '                                intServiceFee, _
                                                    '                                String.Empty, _
                                                    '                                String.Empty, _
                                                    '                                String.Empty, _
                                                    '                                Nothing, _
                                                    '                                String.Empty, _
                                                    '                                Nothing, _
                                                    '                                String.Empty, _
                                                    '                                Nothing, _
                                                    '                                Nothing, _
                                                    '                                Nothing, _
                                                    '                                udtSubsidizeGroupEForm.SubsidizeCode.Trim, _
                                                    '                                strProvideServiceFee, _
                                                    '                                udtSchemeEForm.DisplaySeq, _
                                                    '                                udtSubsidizeGroupEForm.DisplaySeq, _
                                                    '                                strProvideService, _
                                                    '                                strClinicType))

                                                    udtPracticeSchemeInfoList.Add(New PracticeSchemeInfoModel(String.Empty, _
                                                                                    strERN, _
                                                                                    intPIndex, _
                                                                                    udtSubsidizeGroupEForm.SchemeCode.Trim, _
                                                                                    intServiceFee, _
                                                                                    PracticeSchemeInfoStagingStatus.Active, _
                                                                                    String.Empty, _
                                                                                    String.Empty, _
                                                                                    Nothing, _
                                                                                    String.Empty, _
                                                                                    Nothing, _
                                                                                    String.Empty, _
                                                                                    Nothing, _
                                                                                    Nothing, _
                                                                                    Nothing, _
                                                                                    udtSubsidizeGroupEForm.SubsidizeCode.Trim, _
                                                                                    strProvideServiceFee, _
                                                                                    udtSchemeEForm.DisplaySeq, _
                                                                                    udtSubsidizeGroupEForm.DisplaySeq, _
                                                                                    strProvideService, _
                                                                                    strClinicType))
                                                    ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]
                                                End If
                                            Next
                                        End If
                                    End If
                                    'CRE16-002 (Revamp VSS) [End][Chris YIM]
                                End If
                            Next

                        End If
                        intPIndex = intPIndex + 1
                    End If

                Next

            End If
        End If

    End Sub

    Public Sub convertToMOModelCollection(ByVal strERN As String, ByVal dtMO As DataTable, ByRef udtMOList As MedicalOrganizationModelCollection, ByVal intIndex As Integer)

        udtMOList = Nothing
        'udtERNProcessedList = Nothing

        If Not IsNothing(dtMO) Then
            If dtMO.Rows.Count > 0 Then

                Dim udtMO As MedicalOrganizationModel = Nothing
                'Dim udtERNProcessed As ERNProcessedModel = Nothing

                udtMOList = New MedicalOrganizationModelCollection
                'udtERNProcessedList = New ERNProcessedModelCollection

                Dim row As DataRow = dtMO.Rows(intIndex)

                udtMO = New MedicalOrganizationModel(strERN, String.Empty, 1, row.Item("MOEName"), row.Item("MOCName"), _
                            New AddressModel(row.Item("MORoom"), row.Item("MOFloor"), row.Item("MOBlock"), row.Item("MOEAddress"), String.Empty, row.Item("MODistrict"), Nothing), _
                            row.Item("MOBRCode"), row.Item("MOContactNo"), row.Item("MOEmail"), row.Item("MOFax"), row.Item("MORelation"), row.Item("MORelationRemarks"), String.Empty, _
                            Nothing, String.Empty, Nothing, String.Empty, Nothing)

                If Not IsNothing(udtMO) Then
                    udtMOList.Add(udtMO)
                End If

            End If
        End If

    End Sub

    'Public Function convertToSPSchemeList(ByVal strERN As String, ByVal blnHCVS As Boolean, ByVal blnIVSS As Boolean) As SchemeInformationModelCollection
    '    Dim udtSPSchemeInfoList As SchemeInformationModelCollection = New SchemeInformationModelCollection
    '    Dim udtSPSchemeInfo As SchemeInformationModel

    '    If blnHCVS Then
    '        udtSPSchemeInfo = New SchemeInformationModel(strERN, String.Empty, SchemeCode.EHCVS, String.Empty, String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, String.Empty, Nothing, String.Empty, Nothing, Nothing)
    '        udtSPSchemeInfoList.Add(udtSPSchemeInfo)
    '    End If

    '    If blnIVSS Then
    '        udtSPSchemeInfo = New SchemeInformationModel(strERN, String.Empty, SchemeCode.IVSS, String.Empty, String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, String.Empty, Nothing, String.Empty, Nothing, Nothing)
    '        udtSPSchemeInfoList.Add(udtSPSchemeInfo)
    '    End If

    '    Return udtSPSchemeInfoList

    'End Function

    Public Sub UpdateSPModel(ByRef udtInputSP As ServiceProviderModel, _
                                            ByVal udtInputSchemeInfoList As SchemeInformationModelCollection, _
                                            ByVal dtMO As DataTable, _
                                            ByVal dtPracticeBank As DataTable, _
                                            ByVal blnBank As Boolean)

        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL

        Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection
        Dim udtPracticeModel As PracticeModel = Nothing

        Dim udtProfessionalModelCollection As ProfessionalModelCollection = New ProfessionalModelCollection
        Dim udtProfessionalModel As ProfessionalModel = Nothing

        Dim udtPracticeSchemeInfoModelCollection As PracticeSchemeInfoModelCollection = New PracticeSchemeInfoModelCollection
        Dim udtPracticeSchemeInfoModel As PracticeSchemeInfoModel = Nothing

        Dim intPIndex As Integer = 1
        Dim intProfIndex As Integer = 1
        If Not IsNothing(dtPracticeBank) Then
            For Each row As DataRow In dtPracticeBank.Rows
                intProfIndex = udtProfessionalModelCollection.GetProfessionalSeq(row.Item("ServiceCategoryCode"), row.Item("RegistrationCode"))
                If intProfIndex > udtProfessionalModelCollection.Count Then

                    udtProfessionalModel = New ProfessionalModel(String.Empty, String.Empty, intProfIndex, row.Item("ServiceCategoryCode"), _
                                        row.Item("RegistrationCode"), String.Empty, Nothing, String.Empty)

                    If Not IsNothing(udtProfessionalModel) Then
                        udtProfessionalModelCollection.Add(udtProfessionalModel)
                    End If
                End If

                If udtSchemeEFormBLL.ExistSession_SchemeEFormWithSubsidizeGroup Then
                    Dim udtSchemeEFormList As SchemeEFormModelCollection = udtSchemeEFormBLL.GetSession_SchemeEFormWithSubsidizeGroup
                    For Each udtSchemeInformationModel As SchemeInformationModel In udtInputSchemeInfoList.Values

                        Dim udtSchemeEForm As SchemeEFormModel
                        udtSchemeEForm = udtSchemeEFormList.Filter(udtSchemeInformationModel.SchemeCode.Trim)

                        If Not IsNothing(udtSchemeEForm) Then
                            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                            '-----------------------------------------------------------------------------------------
                            If row(udtSchemeEForm.SchemeCode.Trim + "_EligibleForProfession") = YesNo.Yes And _
                                row(udtSchemeEForm.SchemeCode.Trim + "_Selected") = YesNo.Yes Then

                                If udtSchemeEForm.EligibleProfesional(CStr(row.Item("ServiceCategoryCode")).Trim) Then

                                    'If intPIndex = 1 Then
                                    '    Dim udtNewScheme As SchemeInformationModel = New SchemeInformationModel

                                    '    udtSchemeInfoBLL.Clone(udtNewScheme, udtSchemeInformationModel)
                                    '    udtNewScheme.EnrolRefNo = String.Empty

                                    '    udtInputSchemeInfoList.Add(udtNewScheme)
                                    'End If

                                    Dim strClinicType As String = String.Empty

                                    If udtSchemeEForm.AllowNonClinicSetting = YesNo.Yes AndAlso CStr(row.Item(udtSchemeEForm.SchemeCode.ToString.Trim + "_NonClinicSetting_Selected")).Trim.Equals(YesNo.Yes) Then
                                        strClinicType = ClinicType.NonClinic
                                    Else
                                        strClinicType = ClinicType.Clinic
                                    End If

                                    For Each udtSubsidizeGroupEForm As SubsidizeGroupEFormModel In udtSchemeEForm.SubsidizeGroupEFormList
                                        If Not IsNothing(udtSubsidizeGroupEForm) Then
                                            Dim intServiceFee As Nullable(Of Integer)
                                            Dim strProvideServiceFee As String = String.Empty
                                            'CRE15-004 (TIV and QIV) [Start][Chris YIM]
                                            '-----------------------------------------------------------------------------------------
                                            Dim strProvideService As String = String.Empty

                                            If udtSubsidizeGroupEForm.ServiceFeeEnabled Then

                                                If CStr(row.Item(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsProvided")).Trim.Equals(YesNo.Yes) Then
                                                    strProvideService = YesNo.Yes
                                                Else
                                                    strProvideService = YesNo.No
                                                End If

                                                If CStr(row.Item(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_ServiceFee")).Trim.Equals(String.Empty) Then
                                                    intServiceFee = Nothing


                                                    'ElseIf CStr(row.Item(udtSubsidizeGroupEForm.SubsidizeCode.Trim)).Trim.Equals(strNotProvided) Then
                                                ElseIf CStr(row.Item(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_IsNoServiceFee")).Trim.Equals(YesNo.Yes) Then

                                                    intServiceFee = Nothing
                                                    strProvideServiceFee = "N"

                                                Else
                                                    intServiceFee = CInt(row.Item(udtSubsidizeGroupEForm.SubsidizeCode.Trim + "_ServiceFee"))
                                                    strProvideServiceFee = "Y"
                                                End If
                                            Else
                                                intServiceFee = Nothing
                                                ' CRE16-021 Transfer VSS category to PCD [Start][Winnie]
                                                ' Default Provide Service 
                                                strProvideService = YesNo.Yes
                                                ' CRE16-021 Transfer VSS category to PCD [End][Winnie]
                                            End If

                                            'udtPracticeSchemeInfoModelCollection.Add(New PracticeSchemeInfoModel(String.Empty, _
                                            '                                String.Empty, _
                                            '                                intPIndex, _
                                            '                                udtSubsidizeGroupEForm.SchemeCode.Trim, _
                                            '                                intServiceFee, _
                                            '                                String.Empty, _
                                            '                                String.Empty, _
                                            '                                String.Empty, _
                                            '                                Nothing, _
                                            '                                String.Empty, _
                                            '                                Nothing, _
                                            '                                String.Empty, _
                                            '                                Nothing, _
                                            '                                Nothing, _
                                            '                                Nothing, _
                                            '                                udtSubsidizeGroupEForm.SubsidizeCode.Trim, _
                                            '                                strProvideServiceFee, _
                                            '                                udtSchemeEForm.DisplaySeq, _
                                            '                                udtSubsidizeGroupEForm.DisplaySeq, _
                                            '                                strProvideService))

                                            ' INT18-0035 (Fix join PCD without Practice Scheme) [Start][Winnie]
                                            ' ----------------------------------------------------------------------------------------
                                            '' Mark Practice Scheme Info Record Status as "A" for new enrolment (Before empty)

                                            'udtPracticeSchemeInfoModelCollection.Add(New PracticeSchemeInfoModel(String.Empty, _
                                            '                                String.Empty, _
                                            '                                intPIndex, _
                                            '                                udtSubsidizeGroupEForm.SchemeCode.Trim, _
                                            '                                intServiceFee, _
                                            '                                String.Empty, _
                                            '                                String.Empty, _
                                            '                                String.Empty, _
                                            '                                Nothing, _
                                            '                                String.Empty, _
                                            '                                Nothing, _
                                            '                                String.Empty, _
                                            '                                Nothing, _
                                            '                                Nothing, _
                                            '                                Nothing, _
                                            '                                udtSubsidizeGroupEForm.SubsidizeCode.Trim, _
                                            '                                strProvideServiceFee, _
                                            '                                udtSchemeEForm.DisplaySeq, _
                                            '                                udtSubsidizeGroupEForm.DisplaySeq, _
                                            '                                strProvideService, _
                                            '                                strClinicType))

                                            udtPracticeSchemeInfoModelCollection.Add(New PracticeSchemeInfoModel(String.Empty, _
                                                                            String.Empty, _
                                                                            intPIndex, _
                                                                            udtSubsidizeGroupEForm.SchemeCode.Trim, _
                                                                            intServiceFee, _
                                                                            PracticeSchemeInfoStagingStatus.Active, _
                                                                            String.Empty, _
                                                                            String.Empty, _
                                                                            Nothing, _
                                                                            String.Empty, _
                                                                            Nothing, _
                                                                            String.Empty, _
                                                                            Nothing, _
                                                                            Nothing, _
                                                                            Nothing, _
                                                                            udtSubsidizeGroupEForm.SubsidizeCode.Trim, _
                                                                            strProvideServiceFee, _
                                                                            udtSchemeEForm.DisplaySeq, _
                                                                            udtSubsidizeGroupEForm.DisplaySeq, _
                                                                            strProvideService, _
                                                                            strClinicType))
                                            ' INT18-0035 (Fix join PCD without Practice Scheme) [End][Winnie]

                                            'CRE15-004 (TIV and QIV) [End][Chris YIM]
                                        End If
                                    Next
                                End If
                            End If
                            'CRE16-002 (Revamp VSS) [End][Chris YIM]
                        End If
                    Next
                End If

                ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
                ' Add [Mobile_Clinic],[Remarks_Desc] & [Remarks_Desc_Chi]
                udtPracticeModel = New PracticeModel(String.Empty, String.Empty, intPIndex, 1, row.Item("PracticeName"), row.Item("PracticeNameChi"), _
                                                    New AddressModel(row.Item("Room"), row.Item("Floor"), row.Item("Block"), row.Item("Building"), row.Item("ChiBuilding"), row.Item("District"), Nothing), _
                                                    intProfIndex, "A", SubmitChannel.Electronic, String.Empty, _
                                                    row.Item("PhoneDaytime"), Nothing, String.Empty, Nothing, String.Empty, Nothing, YesNo.No, String.Empty, String.Empty,
                                                    Nothing, udtProfessionalModel, udtPracticeSchemeInfoModelCollection)

                udtPracticeModelCollection.Add(udtPracticeModel)
                intPIndex = intPIndex + 1
            Next
        End If

        udtInputSP.PracticeList = New PracticeModelCollection
        udtInputSP.PracticeList = udtPracticeModelCollection

        'Dim intIndex As Integer = 0
        'Dim intNoOfMO As Integer = 0

        'Dim blnSuccess As Boolean = False

        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        'Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL
        'Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
        'Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        'Dim udtPracticeSchemeInfoBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
        'Dim udtProfBLL As ProfessionalBLL = New ProfessionalBLL
        'Dim udtBankBLL As BankAcctBLL = New BankAcctBLL

        ''Dim udtERNProcessedBLL As ERNProcessedBLL = New ERNProcessedBLL
        'If Not IsNothing(dtMO) Then
        '    intNoOfMO = dtMO.Rows.Count
        'End If

        ''Dim strERN(intNoOfMO) As String = New String {}
        'Dim strERN As New List(Of String)

        'Dim udtSP(intNoOfMO) As ServiceProviderModel
        'Dim udtSchemeInfoList(intNoOfMO) As SchemeInformationModelCollection
        'Dim udtMOList(intNoOfMO) As MedicalOrganizationModelCollection
        ''Dim udtERNProcessedList As ERNProcessedModelCollection
        'Dim udtPracticeList(intNoOfMO) As PracticeModelCollection
        'Dim udtPracticeSchemeInfoList(intNoOfMO) As PracticeSchemeInfoModelCollection
        'Dim udtBankList(intNoOfMO) As BankAcctModelCollection
        'Dim udtProfList(intNoOfMO) As ProfessionalModelCollection

        'Dim str1ERN As String = String.Empty

        'If Not IsNothing(dtMO) AndAlso Not IsNothing(dtPracticeBank) Then
        '    'Get the ERN (1 MO will be created 1 application form (1 ERN)

        '    For Each dr As DataRow In dtMO.Rows

        '        'strERN(intIndex) = GeneralFunction.generateSystemNum("A")
        '        strERN.Add(GeneralFunction.generateSystemNum("A"))

        '        'Modify SP model
        '        Dim udtTempSP As ServiceProviderModel = New ServiceProviderModel
        '        udtSPBLL.Clone(udtTempSP, udtInputSP)
        '        udtTempSP.EnrolRefNo = strERN(intIndex)

        '        udtSP(intIndex) = New ServiceProviderModel
        '        udtSP(intIndex) = udtTempSP

        '        'Convert to medical organization model
        '        convertToMOModelCollection(strERN(intIndex), dtMO, udtMOList(intIndex), intIndex)

        '        'Convert to practice, practice scheme, bank and professional model 
        '        convertToPracticeBankProfModelCollection(strERN(intIndex), udtInputSchemeInfoList, dtPracticeBank, blnBank, udtSchemeInfoList(intIndex), udtPracticeList(intIndex), _
        '                                                udtPracticeSchemeInfoList(intIndex), udtBankList(intIndex), udtProfList(intIndex), intIndex)

        '        intIndex = intIndex + 1

        '    Next

        '    'Medical Organization
        '    'udtInputSP.MOList = New MedicalOrganizationModelCollection
        '    'For i As Integer = 0 To intIndex - 1
        '    '    If Not IsNothing(udtMOList(i)) Then
        '    '        For Each udtMedicalOrganizationModel As MedicalOrganizationModel In udtMOList(i).Values
        '    '            udtInputSP.MOList.Add(udtMedicalOrganizationModel)
        '    '        Next
        '    '    End If
        '    'Next

        '    'Practice
        '    'udtInputSP.PracticeList = New PracticeModelCollection
        '    'For i As Integer = 0 To intIndex - 1
        '    '    If Not IsNothing(udtPracticeList(i)) Then
        '    '        For Each udtPracticeModel As PracticeModel In udtPracticeList(i).Values
        '    '            udtPracticeModel.Professional = udtProfList(i)(udtPracticeModel.ProfessionalSeq)
        '    '            udtPracticeModel.RecordStatus = "A"
        '    '            udtInputSP.PracticeList.Add(udtPracticeModel)
        '    '        Next
        '    '    End If
        '    'Next

        '    'For i As Integer = 0 To intIndex - 1
        '    '    If Not IsNothing(udtProfList(i)) Then
        '    '        For Each udtProfessionalModel As ProfessionalModel In udtProfList(i).Values

        '    '        Next
        '    '    End If
        '    'Next

        'Scheme
        udtInputSP.SchemeInfoList = udtInputSchemeInfoList

        udtSPBLL.SaveToSession(udtInputSP)

        'End If

    End Sub

    Public Function AddServiceProviderProfileToEnrolment(ByVal udtInputSP As ServiceProviderModel, ByVal udtInputSchemeInfoList As SchemeInformationModelCollection, ByVal dtMO As DataTable, ByVal dtPracticeBank As DataTable, ByVal blnBank As Boolean) As List(Of String)
        Dim udtDB As Database = New Database

        Dim intIndex As Integer = 0
        Dim intNoOfMO As Integer = 0

        Dim blnSuccess As Boolean = False

        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL
        Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtPracticeSchemeInfoBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
        Dim udtProfBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtBankBLL As BankAcctBLL = New BankAcctBLL

        'Dim udtERNProcessedBLL As ERNProcessedBLL = New ERNProcessedBLL
        If Not IsNothing(dtMO) Then
            intNoOfMO = dtMO.Rows.Count
        End If

        'Dim strERN(intNoOfMO) As String = New String {}
        Dim strERN As New List(Of String)

        Dim udtSP(intNoOfMO) As ServiceProviderModel
        Dim udtSchemeInfoList(intNoOfMO) As SchemeInformationModelCollection
        Dim udtMOList(intNoOfMO) As MedicalOrganizationModelCollection
        'Dim udtERNProcessedList As ERNProcessedModelCollection
        Dim udtPracticeList(intNoOfMO) As PracticeModelCollection
        Dim udtPracticeSchemeInfoList(intNoOfMO) As PracticeSchemeInfoModelCollection
        Dim udtBankList(intNoOfMO) As BankAcctModelCollection
        Dim udtProfList(intNoOfMO) As ProfessionalModelCollection
        Dim udtThirdPartyEnrolmentList(intNoOfMO) As ThirdParty.ThirdPartyAdditionalFieldEnrolmentCollection

        Dim str1ERN As String = String.Empty

        Try
            If Not IsNothing(dtMO) AndAlso Not IsNothing(dtPracticeBank) Then
                'Get the ERN (1 MO will be created 1 application form (1 ERN)

                For Each dr As DataRow In dtMO.Rows

                    'strERN(intIndex) = GeneralFunction.generateSystemNum("A")
                    strERN.Add(GeneralFunction.generateSystemNum("A"))

                    'Modify SP model
                    Dim udtTempSP As ServiceProviderModel = New ServiceProviderModel
                    udtSPBLL.Clone(udtTempSP, udtInputSP)
                    udtTempSP.EnrolRefNo = strERN(intIndex)

                    udtSP(intIndex) = New ServiceProviderModel
                    udtSP(intIndex) = udtTempSP

                    'Convert to medical organization model
                    convertToMOModelCollection(strERN(intIndex), dtMO, udtMOList(intIndex), intIndex)

                    'Convert to practice, practice scheme, bank and professional model 
                    convertToPracticeBankProfModelCollection(strERN(intIndex), udtInputSchemeInfoList, dtPracticeBank, blnBank, udtSchemeInfoList(intIndex), udtPracticeList(intIndex), _
                                                            udtPracticeSchemeInfoList(intIndex), udtBankList(intIndex), udtProfList(intIndex), udtInputSP.ThirdPartyAdditionalFieldEnrolmentList, udtThirdPartyEnrolmentList(intIndex), intIndex)

                    intIndex = intIndex + 1

                Next


            End If

            udtDB.BeginTransaction()
            '----------- 1. Add Personal Particulars -> ServiceProviderEnrolment ------------------------------'

            For i As Integer = 0 To intIndex - 1
                str1ERN = strERN(0)
                blnSuccess = udtSPBLL.AddServiceProviderParticularsToEnrolment(udtSP(i), str1ERN, udtDB)
            Next



            '----------- 2. Add Service Provider Scheme info -> SPSchemeInformationEnrolment -------------------'

            If blnSuccess Then

                For i As Integer = 0 To intIndex - 1
                    If Not IsNothing(udtSchemeInfoList(i)) Then
                        blnSuccess = udtSchemeInfoBLL.AddSchemeInfoListToEnrolment(udtSchemeInfoList(i), udtDB)
                    End If
                Next
            End If

            '----------- 3. Medical Organization & Practice & Bank & Professional  --------------------------------'
            If IsNothing(dtMO) Then

                '----------- 3.1 dtMO = Nothing => Finished e-enrolment (only input personal particulars) ------'

            Else
                '----------- 3.2 Add Medical organiztion information -> MedcialOrganizationEnrolment --------------------'
                If blnSuccess Then
                    For i As Integer = 0 To intIndex - 1
                        If Not IsNothing(udtMOList(i)) Then
                            blnSuccess = udtMOBLL.AddMOListToEnrolment(udtMOList(i), udtDB)
                        End If
                    Next

                End If

                ''----------- 3.3 Add ERN Mapping based on each MO -> ERNMappingEnrolment --------------------'

                'If blnSuccess Then
                '    If Not IsNothing(udtERNProcessedList) Then
                '        blnSuccess = udtERNProcessedBLL.AddERNProcessedListToEnrolment(udtERNProcessedList, udtDB)
                '    End If
                'End If


                If IsNothing(dtPracticeBank) Then
                    '----------- 3.4 dtPracticeBank = Nothing => Finished e-enrolment (only input personal particulars & MO) ------'
                Else
                    '----------- 3.5 Add practice information -> PracticeEnrolment --------------------'
                    If blnSuccess Then
                        For i As Integer = 0 To intIndex - 1
                            If Not IsNothing(udtPracticeList(i)) Then
                                blnSuccess = udtPracticeBLL.AddPracticeListToEnrolment(udtPracticeList(i), udtDB)
                            End If
                        Next

                    End If

                    '----------- 3.6 Add practice scheme information -> PracticeSchemeInformationEnrolment --------------------'
                    If blnSuccess Then
                        For i As Integer = 0 To intIndex - 1
                            If Not IsNothing(udtPracticeSchemeInfoList(i)) Then
                                blnSuccess = udtPracticeSchemeInfoBLL.AddPracticeSchemeInfoListToEnrolment(udtPracticeSchemeInfoList(i), udtDB)
                            End If
                        Next
                    End If

                    '----------- 3.7 Add professional information -> ProfessionalEnrolment --------------------'
                    If blnSuccess Then
                        For i As Integer = 0 To intIndex - 1
                            If Not IsNothing(udtProfList(i)) Then
                                blnSuccess = udtProfBLL.AddProfessionalListToEnrolment(udtProfList(i), udtDB)
                            End If
                        Next

                    End If

                    If Not blnBank Then
                        '----------- 3.8 blnBank = False => Finished e-enrolment (only input personal particulars & MO & Practice & Professional) ----'
                    Else
                        '----------- 3.9 Add bank information -> BankEnrolment --------------------'
                        If blnSuccess Then
                            For i As Integer = 0 To intIndex - 1
                                If Not IsNothing(udtBankList(i)) Then
                                    blnSuccess = udtBankBLL.AddBankAcctListToEnrolment(udtBankList(i), udtDB)
                                End If
                            Next
                        End If
                    End If

                End If
            End If

            If blnSuccess Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
                strERN = Nothing
            End If

            If blnSuccess Then
                For i As Integer = 0 To intIndex - 1
                    If Not IsNothing(udtThirdPartyEnrolmentList(i)) Then
                        For Each udtThirdPartyModel As ThirdParty.ThirdPartyAdditionalFieldEnrolmentModel In udtThirdPartyEnrolmentList(i).Values
                            ThirdParty.ThirdPartyBLL.AddThirdPartyAdditionalFieldEnrolment(udtThirdPartyModel, udtDB)
                        Next
                    End If
                Next
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw ex
        End Try

        Return strERN
    End Function

    Public Function GetServiceProviderProfile(ByVal strERN As String) As Boolean
        Dim udtDB As New Database

        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        'Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
        'Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        'Dim udtPracticeSchemeInfoBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
        'Dim udtProfBLL As ProfessionalBLL = New ProfessionalBLL
        'Dim udtBankBLL As BankAcctBLL = New BankAcctBLL
        'Dim udtERNProcessedBLL As ERNProcessedBLL = New ERNProcessedBLL

        Dim udtSP As ServiceProviderModel = Nothing

        'Dim udtMOList As MedicalOrganizationModelCollection = Nothing
        'Dim udtERNProcessedList As ERNProcessedModelCollection = Nothing

        'Dim udtPracticeList As PracticeModelCollection = Nothing


        Try
            '--------- 1. Get SP Personal Particulars ----------------'
            udtSPBLL.ClearSession()

            udtSP = udtSPBLL.GetServiceProviderParticulasEnrolmentByERN(strERN, udtDB)

            udtSPBLL.SaveToSession(udtSP)

            ''--------- 2. Get ERNProcessed List ----------------'
            'udtERNProcessedBLL.ClearSession()

            'udtERNProcessedList = udtERNProcessedBLL.GetERNProcessedListEnrolmentByERN(strERN, udtDB)

            'udtERNProcessedBLL.SaveToSession(udtERNProcessedList)

            ''--------- 2. Get Practice & Professional & Bank List ----------------'
            'udtPracticeBLL.ClearSession()

            'udtPracticeList = udtPracticeBLL.GetPracticeBankAcctListFromEnrolmentByERN(strERN, udtDB)

            'For Each udtPractice As PracticeModel In udtPracticeList.Values
            '    udtPractice.PracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListEnrolmentByERN(strERN, udtDB)
            'Next

            'udtPracticeBLL.SaveToSession(udtPracticeList)

            Return True
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function


    ''' <summary>
    ''' Get the lists of district by area code
    ''' </summary>
    ''' <param name="strAreaCode"></param>
    ''' <returns>DistrictModelCollection</returns>
    ''' <remarks></remarks>
    Public Function GetDistrict(Optional ByVal strAreaCode As String = "") As DistrictModelCollection
        Dim udtDistrictModelCollection As DistrictModelCollection = New DistrictModelCollection
        If strAreaCode.Equals(String.Empty) Then
            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            'udtDistrictModelCollection = udtDistrictBLL.GetDistrictList
            udtDistrictModelCollection = udtDistrictBLL.GetDistrictInput(PlatformCode.EForm)
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
        Else
            'CRE13-019-02 Extend HCVS to China [Start][Winnie]            
            'udtDistrictModelCollection = udtDistrictBLL.GetDistrictListByAreaCode(strAreaCode)
            udtDistrictModelCollection = udtDistrictBLL.GetDistrictListByPlatformByAreaCode(PlatformCode.EForm, strAreaCode)
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
        End If

        Return udtDistrictModelCollection
    End Function

    ''' <summary>
    ''' Get the lists of area
    ''' </summary>
    ''' <returns>AreaModelCollection</returns>
    ''' <remarks></remarks>
    Public Function GetArea() As AreaModelCollection
        Dim udtAreaModelCollection As AreaModelCollection = New AreaModelCollection
        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'udtAreaModelCollection = udtAreaBLL.GetAreaList
        udtAreaModelCollection = udtAreaBLL.GetAreaInput(PlatformCode.EForm)

        'Return udtAreaBLL.GetAreaList
        Return udtAreaModelCollection
        'CRE13-019-02 Extend HCVS to China [End][Winnie]
    End Function

    ''' <summary>
    ''' Get the area by district code
    ''' </summary>
    ''' <param name="strDistrictCode"></param>
    ''' <returns>area name</returns>
    ''' <remarks></remarks>
    Public Function GetAreaByDistrictCode(ByVal strDistrictCode As String) As String
        Dim udtDistrictModelCollection As DistrictModelCollection = GetDistrict()
        Dim udtDistrictModel As DistrictModel

        udtDistrictModel = udtDistrictModelCollection.Item(strDistrictCode)

        Return udtDistrictModel.Area_ID
    End Function

    ''' <summary>
    ''' Get the lists of Health Profession
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 

    Public Function GetHealthProf() As ProfessionModelCollection

        
        Dim udtProfessionModelCollection As ProfessionModelCollection = New ProfessionModelCollection
        udtProfessionModelCollection = ProfessionBLL.GetProfessionList

        ' CRE19-006 (DHC) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        'Return udtProfessionModelCollection.FilterByPeriod(ProfessionModelCollection.EnumPeriodType.Enrollment)

        Dim udtFilteredProfessionModelCollection As ProfessionModelCollection = New ProfessionModelCollection

        For Each udtProfessionModel As ProfessionModel In udtProfessionModelCollection
            If udtProfessionModel.EFormAvail = YesNo.Yes Then
                udtFilteredProfessionModelCollection.add(udtProfessionModel)
            End If
        Next

        Return udtFilteredProfessionModelCollection.FilterByPeriod(ProfessionModelCollection.EnumPeriodType.Enrollment)
        ' CRE19-006 (DHC) [End][Winnie]
    End Function

    ''' <summary>
    ''' Get the lists of practice type
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPracticeType() As StaticDataModelCollection

        Return udtStaticDataBLL.GetStaticDataListByColumnName("PRACTICETYPE")
    End Function

    ''' <summary>
    ''' Get the health profession by health profession code
    ''' </summary>
    ''' <param name="strServiceCategoryCode"></param>
    ''' <returns>Health Profession Name</returns>
    ''' <remarks></remarks>
    Public Function GetHealthProfName(ByVal strServiceCategoryCode As String) As ProfessionModel

        ' CRE11-024-02 HCVS Pilot Extension Part 1 [Start]

        ' -----------------------------------------------------------------------------------------

        'Return udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PROFESSION", strItemNo)
        Return ProfessionBLL.GetProfessionListByServiceCategoryCode(strServiceCategoryCode)

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

    End Function

    ''' <summary>
    ''' Get the practice type by practice type code
    ''' </summary>
    ''' <param name="strItemNo"></param>
    ''' <returns>Practice Type Name</returns>
    ''' <remarks></remarks>
    Public Function GetPracticeTypeName(ByVal strItemNo As String) As StaticDataModel
        Return udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PRACTICETYPE", strItemNo)

    End Function

    'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    'Public Function AskJoinIVSS(ByVal selectedHealthProf As String) As Boolean
    '    Dim strJoinIVSS() As String
    '    Dim value1 As String = String.Empty
    '    Dim blnJoinIVSS As Boolean = False

    '    GeneralFunction.getSystemParameter("JoinIVSSProf", value1, String.Empty)
    '    strJoinIVSS = value1.Split(";")
    '    Dim i As Integer

    '    For i = 0 To strJoinIVSS.Length - 1
    '        If selectedHealthProf = strJoinIVSS(i) Then
    '            blnJoinIVSS = True
    '            'Else
    '            '    blnAskHadJoinedPPIePRProfCode = False
    '        End If
    '    Next


    '    Return blnJoinIVSS
    'End Function
    'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]

    Public Function AskHadJoinedEHRSSProfCode(ByVal selectedHealthProf As String) As Boolean
        Dim strValue1 As String = (New GeneralFunction).getSystemParameter("AskHadJoinedEHRSSProfCode")

        If strValue1 = "ALL" Then Return True

        For Each strProf As String In strValue1.Split(";".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            If strProf = selectedHealthProf Then
                Return True
            End If

        Next

        Return False

    End Function

    Public Function getAreaString(ByVal strDistrict As String) As String
        Dim strAreaCode As String

        If strDistrict.Equals(String.Empty) Then
            strAreaCode = String.Empty
        Else
            strAreaCode = GetAreaByDistrictCode(strDistrict)
        End If

        Return strAreaCode
    End Function

    Public Function PrintOutDataTableByERNList(ByVal strERN As List(Of String)) As DataTable
        Dim dt As DataTable = New DataTable
        Dim strLongERN As String = String.Empty

        For i As Integer = 0 To strERN.Count - 1
            strLongERN = strLongERN + "," + strERN(i)
        Next

        If strLongERN.Length > 1 Then
            strLongERN = strLongERN.Substring(1)
        End If

        Try
            Dim prams() As SqlParameter = { _
                            udtDB.MakeInParam("@enrolment_ref_no", SqlDbType.Char, 8000, strLongERN)}
            udtDB.RunProc("proc_MedicalOrganizationEnrolment_get_byERNList", prams, dt)

            If dt.Rows.Count = 0 Then
                dt = Nothing
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return dt
    End Function

    Public Function GetBatchOfERNList(ByVal strERN As String) As List(Of String)
        Dim strRes As New List(Of String)
        Dim dt As DataTable = New DataTable

        Try
            Dim prams() As SqlParameter = { _
                            udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN)}
            udtDB.RunProc("proc_ServiceProviderEnrolmentBatch_get_byERN", prams, dt)

            If dt.Rows.Count = 0 Then
                dt = Nothing
            End If

            If Not IsNothing(dt) Then
                For Each dr As DataRow In dt.Rows
                    strRes.Add(CStr(dr.Item("enrolment_ref_no")).Trim)
                Next
            End If

            If strRes.Count = 0 Then
                strRes = Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try


        Return strRes
    End Function

    Public Function CombineSPModel(ByVal strERN As List(Of String)) As ServiceProviderModel
        Dim udtSP As New ServiceProviderModel
        Dim udtSPTemp As ServiceProviderModel

        Dim udtSPBLL As New ServiceProviderBLL

        Dim intMOIndex As Integer = 1
        Dim intPracticeIndex As Integer = 1

        For i As Integer = 0 To strERN.Count - 1
            udtSPTemp = udtSPBLL.GetServiceProviderEnrolmentProfileByERN(strERN(i), udtDB)

            If i = 0 Then
                udtSPBLL.Clone(udtSP, udtSPTemp)

                udtSP.PracticeList = Nothing
                udtSP.MOList = Nothing

                udtSP.PracticeList = New PracticeModelCollection
                udtSP.MOList = New MedicalOrganizationModelCollection
            End If

            For Each udtMO As MedicalOrganizationModel In udtSPTemp.MOList.Values
                udtMO.DisplaySeq = intMOIndex

                For Each udtPractice As PracticeModel In udtSPTemp.PracticeList.Values
                    udtPractice.MODisplaySeq = intMOIndex
                    udtPractice.DisplaySeq = intPracticeIndex
                    udtPractice.BankAcct.SpPracticeDisplaySeq = intPracticeIndex
                    udtSP.PracticeList.Add(udtPractice)
                    intPracticeIndex = intPracticeIndex + 1
                Next

                udtSP.MOList.Add(udtMO)
                intMOIndex = intMOIndex + 1
            Next
        Next

        Return udtSP
    End Function

    Public Sub ClearRedirectPageSession()
        HttpContext.Current.Session(SESS_Disclaimer) = Nothing
        HttpContext.Current.Session(SESS_PersonalParticular) = Nothing
        HttpContext.Current.Session(SESS_MedicalOrganization) = Nothing
        HttpContext.Current.Session(SESS_Practice) = Nothing
        HttpContext.Current.Session(SESS_Bank) = Nothing
        HttpContext.Current.Session(SESS_SchemeSelection) = Nothing
        HttpContext.Current.Session(SESS_ConfirmDetails) = Nothing

        HttpContext.Current.Session.Remove(SESS_Disclaimer)
        HttpContext.Current.Session.Remove(SESS_PersonalParticular)
        HttpContext.Current.Session.Remove(SESS_MedicalOrganization)
        HttpContext.Current.Session.Remove(SESS_Practice)
        HttpContext.Current.Session.Remove(SESS_Bank)
        HttpContext.Current.Session.Remove(SESS_SchemeSelection)
        HttpContext.Current.Session.Remove(SESS_ConfirmDetails)

    End Sub

End Class
