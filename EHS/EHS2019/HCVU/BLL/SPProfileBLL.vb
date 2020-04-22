Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.Component.BankAcct
Imports Common.Component.Professional
Imports Common.Component.Address
Imports Common.Component.Area
Imports Common.Component.District
Imports Common.Component.ERNProcessed
Imports Common.Component.HCVUUser
Imports Common.Component.MedicalOrganization
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
'Imports Common.Component.SchemeDetails
Imports Common.Component.SchemeInformation
Imports Common.Component.StaticData

' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

' -----------------------------------------------------------------------------------------

Imports Common.Component.Profession

' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

Imports Common.Component.Token
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports System.Data.SqlClient
Imports Common.ComObject
Imports Common.ComFunction.GeneralFunction
Imports Common.ComFunction.AccountSecurity

Public Class SPProfileBLL

    Private udtAreaBLL As New AreaBLL
    Private udtBankAcctBLL As New BankAcctBLL
    Private udtDistrictBLL As New DistrictBLL
    Private udtERNProcessedBLL As New ERNProcessedBLL
    Private udtFormatter As New Formatter
    Private udtGeneralFunction As New GeneralFunction
    Private udtHCVUUserBLL As New HCVUUserBLL
    Private udtMedicalOrganizationBLL As New MedicalOrganizationBLL
    Private udtPracticeBLL As New PracticeBLL
    Private udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL
    Private udtProfessionalBLL As New ProfessionalBLL
    Private udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
    'Private udtSchemeDetailBLL As New SchemeDetailBLL
    Private udtSchemeInformationBLL As New SchemeInformationBLL
    Private udtServiceProviderBLL As New ServiceProviderBLL
    Private udtServiceProviderVerificationBLL As New ServiceProviderVerificationBLL
    Private udtSPAccountUpdateBLL As New SPAccountUpdateBLL
    Private udtStaticDataBLL As New StaticDataBLL

    ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

    ' -----------------------------------------------------------------------------------------

    Private udtProfessionBLL As ProfessionBLL = New ProfessionBLL

    ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

    Private udtTokenBLL As New TokenBLL
    Private udtValidator As New Validator

    Private udtDB As New Database

    Private Const UpdateSPInfo As Integer = 1
    Private Const UpdatePractice As Integer = 2
    Private Const UpdateProfessional As Integer = 3
    Private Const UpdateBank As Integer = 4
    Private Const UpdatePracticeBank As Integer = 5
    Private Const UpdateMO As Integer = 6
    Private Const UpdateScheme As Integer = 7
    Private Const UpdateMOPractice As Integer = 8

    Public Property DB() As Database
        Get
            Return udtDB
        End Get
        Set(ByVal Value As Database)
            udtDB = Value
        End Set
    End Property

    Public Sub New()
    End Sub

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
            udtDistrictModelCollection = udtDistrictBLL.GetDistrictInput(PlatformCode.BO)
            'CRE13-019-02 Extend HCVS to China [End][Winnie]
        Else
            'CRE13-019-02 Extend HCVS to China [Start][Winnie]
            'udtDistrictModelCollection = udtDistrictBLL.GetDistrictListByAreaCode(strAreaCode)
            udtDistrictModelCollection = udtDistrictBLL.GetDistrictListByPlatformByAreaCode(PlatformCode.BO, strAreaCode)
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
        udtAreaModelCollection = udtAreaBLL.GetAreaInput(PlatformCode.BO)

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
    Public Function GetHealthProf() As ProfessionModelCollection

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

        ' -----------------------------------------------------------------------------------------

        'Return udtStaticDataBLL.GetStaticDataListByColumnName("PROFESSION")

        Dim udtProfessionModelCollection As ProfessionModelCollection = New ProfessionModelCollection
        udtProfessionModelCollection = ProfessionBLL.GetProfessionList
        'Return udtProfessionModelCollection
        Return udtProfessionModelCollection.FilterByPeriod(ProfessionModelCollection.EnumPeriodType.Enrollment)

        ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

    End Function

    ''' <summary>
    ''' Get the list of schemes
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetScheme() As StaticDataModelCollection
        Return udtStaticDataBLL.GetStaticDataListByColumnName("SCHEME")
    End Function

    ''' <summary>
    ''' Get the lists of practice type
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPracticeType() As StaticDataModelCollection
        Return udtStaticDataBLL.GetStaticDataListByColumnName("PRACTICETYPE")
    End Function

    ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]

    ' -----------------------------------------------------------------------------------------

    ''' <summary>
    ''' Get the health profession by health profession code
    ''' </summary>
    ''' <param name="strServiceCategoryCode"></param>
    ''' <returns>Health Profession Name</returns>
    ''' <remarks></remarks>
    Public Function GetHealthProfName(ByVal strServiceCategoryCode As String) As ProfessionModel
        'Return udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PROFESSION", strItemNo)
        Return ProfessionBLL.GetProfessionListByServiceCategoryCode(strServiceCategoryCode)
    End Function

    ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

    ''' <summary>
    ''' Get the practice type by practice type code
    ''' </summary>
    ''' <param name="strItemNo"></param>
    ''' <returns>Practice Type Name</returns>
    ''' <remarks></remarks>
    Public Function GetPracticeTypeName(ByVal strItemNo As String) As StaticDataModel
        Return udtStaticDataBLL.GetStaticDataByColumnNameItemNo("PRACTICETYPE", strItemNo)
    End Function

    Public Function GetSchemeCode() As String
        Dim strSchemeCode As String = String.Empty
        udtGeneralFunction.getSystemParameter("SchemeCode", strSchemeCode, String.Empty)

        Return strSchemeCode

    End Function

    Public Function GetMasterScheme() As SchemeBackOfficeModelCollection
        Dim udtSchemeBOList As SchemeBackOfficeModelCollection = Nothing

        If Not udtSchemeBackOfficeBLL.ExistSession_SchemeBackOfficeWithSubsidizeGroup Then
            Dim dtmCurrentDtm As DateTime = udtGeneralFunction.GetSystemDateTime
            udtSchemeBOList = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache

            'If Not IsNothing(udtSchemeBOList) Then
            '    For Each udtSchemeBO As SchemeBackOfficeModel In udtSchemeBOList
            '        For Each udtSubsidizeGpBO As SubsidizeGroupBackOfficeModel In udtSchemeBO.SubsidizeGroupBackOfficeList
            '            If Not IsNothing(udtSubsidizeGpBO) Then
            '                Dim udtSchemeDetailList As New SchemeDetailModelCollection
            '                udtSchemeDetailList.Add(udtSchemeDetailBLL.getCurrentSubSchemeDetail(udtSubsidizeGpBO.SchemeCode, dtmCurrentDtm))
            '                udtSubsidizeGpBO. = udtSchemeDetailList
            '            End If
            '        Next
            '    Next
            'End If
            'udtSchemeBackOfficeBLL.SaveToSession_SchemeBackOfficeWithSubsidizeGroup(udtSchemeBOList)

        Else
            udtSchemeBOList = udtSchemeBackOfficeBLL.GetSession_SchemeBackOfficeWithSubsidizeGroup

        End If

        Return udtSchemeBOList
    End Function

    Public Function AskHadJoinedEHRSSProfCode(ByVal selectedHealthProf As String) As Boolean
        Dim strValue As String = (New GeneralFunction).getSystemParameter("AskHadJoinedEHRSSProfCode")

        If strValue = "ALL" Then Return True

        For Each strProf As String In strValue.Split(";".ToCharArray, StringSplitOptions.RemoveEmptyEntries)
            If strProf = selectedHealthProf Then
                Return True
            End If
        Next

        Return False

    End Function

    Public Function AskJoinIVSS(ByVal selectedHealthProf As String) As Boolean
        Dim strJoinIVSS() As String
        Dim value1 As String = String.Empty
        Dim blnJoinIVSS As Boolean = False

        udtGeneralFunction.getSystemParameter("JoinIVSSProf", value1, String.Empty)

        'If Not GeneralFunction.getSystemParameter("JoinIVSSProf", value1, String.Empty) Then
        strJoinIVSS = value1.Split(";")
        Dim i As Integer

        For i = 0 To strJoinIVSS.Length - 1
            If selectedHealthProf = strJoinIVSS(i) Then
                blnJoinIVSS = True
                'Else
                '    blnAskHadJoinedPPIePRProfCode = False
            End If
        Next
        ' End If

        Return blnJoinIVSS
    End Function

    Public Sub BindDataToControlForDataEntry(ByVal strERN As String, ByRef fvSP As FormView, ByRef gvPractice As GridView, ByRef gvMO As GridView, ByRef gvBank As GridView)
        'If GetServiceProviderProfileFromStaging(strERN) Then
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        If udtServiceProviderBLL.Exist Then
            Dim udtServiceProviderCollection As ServiceProviderModelCollection = New ServiceProviderModelCollection
            Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetSP
            udtServiceProviderCollection.Add(udtSP.EnrolRefNo, udtSP)
            fvSP.DataSource = udtServiceProviderCollection.Values
            'fvSP.DataBind()

            If udtSP.PracticeList.Count = 0 Then
                gvPractice.DataSource = EmptyPracticeCollection.Values
                gvBank.DataSource = Nothing
            Else
                gvPractice.DataSource = udtSP.PracticeList.Values
                'gvPractice.DataBind()

                gvBank.DataSource = udtSP.PracticeList.Values
                ' gvBank.DataBind()
            End If

            If udtSP.MOList.Count = 0 Then
                gvMO.DataSource = EmptyMOCollection.Values
            Else
                gvMO.DataSource = udtSP.MOList.Values
            End If


        End If

        'End If
    End Sub

    Public Function SetPageCheckedImg() As Boolean()

        Dim blnRes(5) As Boolean
        Dim udtSPVerificationBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL

        If udtSPVerificationBLL.Exist Then
            Dim udtSPVerificationModel As ServiceProviderVerificationModel
            udtSPVerificationModel = udtSPVerificationBLL.GetSPVerification

            blnRes(0) = udtSPVerificationModel.SPConfirmed
            blnRes(1) = udtSPVerificationModel.MOConfirmed
            blnRes(2) = udtSPVerificationModel.PracticeConfirmed
            blnRes(3) = udtSPVerificationModel.BankAcctConfirmed
            blnRes(4) = udtSPVerificationModel.SchemeConfirmed

        Else
            blnRes(0) = False
            blnRes(1) = False
            blnRes(2) = False
            blnRes(3) = False
            blnRes(4) = False
        End If

        Return blnRes

    End Function

    ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

    Public Function GetTokenModelBySPID(ByVal strSPID As String, Optional ByVal GetTokenSerialNoFromEHRSS As Boolean = True) As TokenModel

        ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

        Dim udtTokenModel As New TokenModel
        Dim udtTokenBLL As New TokenBLL

        Try

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

            udtTokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(strSPID, udtDB, GetTokenSerialNoFromEHRSS)

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

        Catch ex As Exception
            Throw
        End Try

        Return udtTokenModel
    End Function

    ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

    Public Function GetTokenTokenSerialNoBySPID(ByVal strSPID As String, Optional ByVal GetTokenSerialNoFromEHRSS As Boolean = True) As String

        ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

        Dim udtTokenBLL As New TokenBLL
        Dim udtTokenModel As New TokenModel

        Dim dt As DataTable
        dt = New DataTable

        Dim strRes As String = String.Empty

        Try

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [Start][Tommy]

            udtTokenModel = GetTokenModelBySPID(strSPID, GetTokenSerialNoFromEHRSS)

            ' [CRE12-011] Not to display PPI-ePR token serial no. for service provider who use PPIePR issued token in eHS [End][Tommy]

            If IsNothing(udtTokenModel) Then
                dt = udtTokenBLL.GetTokenSerialNoByUserID(strSPID, udtDB)

                If dt.Rows.Count = 1 Then
                    If CStr(dt.Rows(0).Item("Remark")).Equals(String.Empty) Then
                        strRes = CStr(dt.Rows(0).Item("Token_Serial_No")).Trim
                    Else
                        Dim udtStaticModel As StaticDataModel
                        udtStaticModel = udtStaticDataBLL.GetStaticDataByColumnNameItemNo("TOKENDISABLE", CStr(dt.Rows(0).Item("Remark")).Trim)
                        strRes = CStr(dt.Rows(0).Item("Token_Serial_No")).Trim + " Deactivated (" + udtStaticModel.DataValue + ")"
                    End If
                End If
            Else
                strRes = udtTokenModel.TokenSerialNo
            End If


        Catch ex As Exception
            Throw
        End Try

        Return strRes
    End Function

    Public Function GetServiceProviderPermanentProfileNoSession(ByVal strERN As String) As ServiceProviderModel
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim udtUserACBLL As UserAC.UserACBLL = New UserAC.UserACBLL

        Dim dt As DataTable = New DataTable

        Try
            udtSP = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(strERN, udtDB)
            udtSP = udtServiceProviderBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(udtSP.SPID, udtDB)

            dt = udtUserACBLL.GetUserACForLogin(udtSP.SPID, String.Empty, SPAcctType.ServiceProvider)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Alias_Account")) Then
                    udtSP.AliasAccount = String.Empty
                Else
                    udtSP.AliasAccount = CStr(dt.Rows(0).Item("Alias_Account"))
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
        Return udtSP
    End Function

    Public Function GetServiceProviderPermanentProfile(ByVal strERN) As ServiceProviderModel
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim udtUserACBLL As UserAC.UserACBLL = New UserAC.UserACBLL

        Dim dt As DataTable = New DataTable

        Try
            udtSP = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(strERN, udtDB)
            udtSP = udtServiceProviderBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(udtSP.SPID, udtDB)

            dt = udtUserACBLL.GetUserACForLogin(udtSP.SPID, String.Empty, SPAcctType.ServiceProvider)
            If dt.Rows.Count > 0 Then
                If IsDBNull(dt.Rows(0).Item("Alias_Account")) Then
                    udtSP.AliasAccount = String.Empty
                Else
                    udtSP.AliasAccount = CStr(dt.Rows(0).Item("Alias_Account"))
                End If
            End If

            If Not IsNothing(udtSP) Then
                udtServiceProviderBLL.SaveToSession(udtSP)
            End If

        Catch ex As Exception
            Throw
        End Try
        Return udtSP
    End Function

    Public Function GetServiceProviderPermanentProfileWithMaintenance(ByVal strSPID As String) As ServiceProviderModel
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL

        Dim udtUserACBLL As UserAC.UserACBLL = New UserAC.UserACBLL

        Dim dt As DataTable = New DataTable

        Try
            udtSP = udtServiceProviderBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(strSPID, udtDB)
            If Not IsNothing(udtSP) Then
                dt = udtUserACBLL.GetUserACForLogin(udtSP.SPID, String.Empty, SPAcctType.ServiceProvider)
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0).Item("Alias_Account")) Then
                        udtSP.AliasAccount = String.Empty
                    Else
                        udtSP.AliasAccount = CStr(dt.Rows(0).Item("Alias_Account"))
                    End If
                End If

                If Not IsNothing(udtSP) Then
                    udtServiceProviderBLL.SaveToSession(udtSP)
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
        Return udtSP
    End Function

    Public Function GetServiceProviderStagingProfileNoSession(ByVal strERN) As ServiceProviderModel
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtUserACBLL As UserAC.UserACBLL = New UserAC.UserACBLL

        Dim dt As DataTable = New DataTable

        Try
            udtSP = udtServiceProviderBLL.GetServiceProviderStagingProfileByERN(strERN, udtDB)
            If Not IsNothing(udtSP) Then
                dt = udtUserACBLL.GetUserACForLogin(udtSP.SPID, String.Empty, SPAcctType.ServiceProvider)
                If dt.Rows.Count > 0 Then
                    If IsDBNull(dt.Rows(0).Item("Alias_Account")) Then
                        udtSP.AliasAccount = String.Empty
                    Else
                        udtSP.AliasAccount = CStr(dt.Rows(0).Item("Alias_Account"))
                    End If
                End If
            End If

        Catch ex As Exception
            Throw
        End Try
        Return udtSP
    End Function

    Public Function GetServiceProviderStagingProfileNoSessionForMigration(ByVal strERN As String, ByVal udtDatabase As Database) As ServiceProviderModel
        Dim udtSP As ServiceProviderModel = Nothing
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtUserACBLL As UserAC.UserACBLL = New UserAC.UserACBLL

        Dim dt As DataTable = New DataTable

        Try
            udtSP = udtServiceProviderBLL.GetServiceProviderStagingByERN(strERN, udtDatabase)
            If Not IsNothing(udtSP) Then
                udtSP = udtServiceProviderBLL.GetServiceProviderStagingProfileByERN(strERN, udtDatabase)
                If Not IsNothing(udtSP) Then
                    dt = udtUserACBLL.GetUserACForLoginWithDBsupplied(udtSP.SPID, String.Empty, SPAcctType.ServiceProvider, udtDatabase)
                    If dt.Rows.Count > 0 Then
                        If IsDBNull(dt.Rows(0).Item("Alias_Account")) Then
                            udtSP.AliasAccount = String.Empty
                        Else
                            udtSP.AliasAccount = CStr(dt.Rows(0).Item("Alias_Account"))
                        End If
                    End If
                End If
            Else
                udtSP = Nothing
            End If

        Catch ex As Exception
            Throw
        End Try
        Return udtSP
    End Function

    ' CRE12-001 eHS and PCD integration [Start][Koala]
    ' -----------------------------------------------------------------------------------------

    ''' <summary>
    ''' Retrieve Service provider Original record in Enrollment
    ''' </summary>
    ''' <param name="strERN"></param>
    ''' <param name="udtServiceProviderModel"></param>
    ''' <param name="udtProfessionalModelCollection"></param>
    ''' <param name="udtBankAcctModelCollection"></param>
    ''' <param name="udtPracticeSchemeInfoModelCollection"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetServiceProviderOriginalProfile(ByVal strERN As String, _
                                                    ByRef udtServiceProviderModel As ServiceProviderModel, _
                                                    ByRef udtProfessionalModelCollection As ProfessionalModelCollection, _
                                                    ByRef udtBankAcctModelCollection As BankAcctModelCollection, _
                                                    ByRef udtPracticeSchemeInfoModelCollection As PracticeSchemeInfoModelCollection) As Boolean

        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtProfBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtBankBLL As BankAcctBLL = New BankAcctBLL

        Try
            udtServiceProviderModel = udtSPBLL.GetServiceProviderCopyProfileByERNInHCVU(strERN, EnumEnrolCopy.Original, udtDB)
            If Not IsNothing(udtServiceProviderModel) Then
                udtProfessionalModelCollection = udtProfBLL.GetProfessionalListFromCopyByERN(strERN, EnumEnrolCopy.Original, udtDB)
                udtBankAcctModelCollection = udtBankBLL.GetBankAcctListFromCopyByERN(strERN, EnumEnrolCopy.Original, udtDB)
                udtServiceProviderModel.SubmitMethod = SubmitChannel.Electronic
                udtPracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListCopyByERNInHCVU(strERN, EnumEnrolCopy.Original, udtDB)
            End If

            Return True
        Catch ex As Exception
            Throw
        End Try

    End Function
    ' CRE12-001 eHS and PCD integration [End][Koala]

    Public Function GetServiceProviderProfile(ByVal strERN As String, ByVal strTableLocation As String) As Boolean
        Dim blnRes As Boolean = False

        Dim udtSp As ServiceProviderModel = New ServiceProviderModel
        Dim udtProfessionalList As ProfessionalModelCollection = New ProfessionalModelCollection
        Dim udtBankAcctList As BankAcctModelCollection = New BankAcctModelCollection
        Dim udtSchemeList As SchemeInformationModelCollection = New SchemeInformationModelCollection
        'Dim udtERNProcessedList As ERNProcessedModelCollection = New ERNProcessedModelCollection
        Dim udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = New PracticeSchemeInfoModelCollection

        Try
            Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
            Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL
            Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
            'Dim udtERNProcessedBLL As ERNProcessedBLL = New ERNProcessedBLL
            Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
            Dim udtPracticeSchemeInfoBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
            Dim udtProfBLL As ProfessionalBLL = New ProfessionalBLL
            Dim udtBankBLL As BankAcctBLL = New BankAcctBLL

            Dim udtSPAccountUpateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
            Dim udtSPAcctUpdModel As SPAccountUpdateModel = New SPAccountUpdateModel

            Dim udtSPVerificationBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL
            Dim udtSPVerificationModel As ServiceProviderVerificationModel = New ServiceProviderVerificationModel

            Select Case strTableLocation
                Case TableLocation.Enrolment
                    udtSp = udtSPBLL.GetServiceProviderEnrolmentProfileByERNInHCVU(strERN, udtDB)
                    If Not IsNothing(udtSp) Then
                        udtProfessionalList = udtProfBLL.GetProfessionalListFromEnrolmentByERN(strERN, udtDB)
                        udtBankAcctList = udtBankBLL.GetBankAcctListFromEnrolmentByERN(strERN, udtDB)

                        udtSp.SubmitMethod = SubmitChannel.Electronic

                        'udtSp.SchemeInfoList = udtSchemeInfoBLL.GetSchemeInfoListEnrolmentInHCVU(strERN, udtDB)

                        'udtERNProcessedList = udtERNProcessedBLL.GetERNProcessedListEnrolmentByERN(strERN, udtDB)

                        udtPracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListEnrolmentByERNInHCVU(strERN, udtDB)
                        blnRes = True
                    End If

                Case TableLocation.Staging
                    udtSp = udtSPBLL.GetServiceProviderStagingProfileByERN(strERN, udtDB)
                    If Not IsNothing(udtSp) Then
                        udtProfessionalList = udtProfBLL.GetProfessinalListFromStagingByERN(strERN, udtDB)
                        udtBankAcctList = udtBankBLL.GetBankAcctListFromStagingByERN(strERN, udtDB)

                        udtSPVerificationModel = udtSPVerificationBLL.GetSerivceProviderVerificationByERN(udtSp.EnrolRefNo, udtDB)
                        udtSPAcctUpdModel = udtSPAccountUpateBLL.GetSPAccountUpdateByERN(udtSp.EnrolRefNo, udtDB)

                        If IsNothing(udtSPAcctUpdModel) Then
                            udtSPAcctUpdModel = New SPAccountUpdateModel()
                        End If

                        'udtSp.SchemeInfoList = udtSchemeInfoBLL.GetSchemeInfoListStaging(strERN, udtDB)

                        'udtERNProcessedList = udtERNProcessedBLL.GetERNProcessedListStagingByERN(strERN, udtDB)

                        udtPracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByERN(strERN, udtDB)
                        blnRes = True
                    End If


                Case TableLocation.Permanent
                    udtSp = udtSPBLL.GetServiceProviderPermanentProfileByERN(strERN, udtDB)
                    udtSp = udtSPBLL.GetServiceProviderPermanentProfileWithMaintenanceBySPID(udtSp.SPID, udtDB)

                    udtSp.EmailChanged = String.Empty

                    udtProfessionalList = udtProfBLL.GetProfessinalListFromPermanentBySPID(udtSp.SPID, udtDB)
                    udtBankAcctList = udtBankBLL.GetBankAcctListFromPermanentBySPID(udtSp.SPID, udtDB)

                    'udtSp.SchemeInfoList = udtSchemeInfoBLL.GetSchemeInfoListPermanent(udtSp.SPID, udtDB)

                    'udtERNProcessedList = udtERNProcessedBLL.GetERNProcessedListBySPID(udtSp.SPID, udtDB)

                    udtPracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListBySPID(udtSp.SPID, udtDB)

                    For Each udtSchemeInfoModel As SchemeInformationModel In udtSp.SchemeInfoList.Values
                        udtSchemeInfoModel.EnrolRefNo = udtSp.EnrolRefNo
                    Next

                    For Each udtMO As MedicalOrganizationModel In udtSp.MOList.Values
                        udtMO.EnrolRefNo = udtSp.EnrolRefNo
                    Next

                    For Each udtPracticeModel As PracticeModel In udtSp.PracticeList.Values
                        udtPracticeModel.EnrolRefNo = udtSp.EnrolRefNo
                        For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
                            udtPracticeSchemeInfo.EnrolRefNo = udtSp.EnrolRefNo
                        Next
                    Next

                    For Each udtProfessionalModel As ProfessionalModel In udtProfessionalList.Values
                        udtProfessionalModel.EnrolRefNo = udtSp.EnrolRefNo
                    Next

                    For Each udtBankAcctModel As BankAcctModel In udtBankAcctList.Values
                        udtBankAcctModel.EnrolRefNo = udtSp.EnrolRefNo
                    Next

                    For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                        udtPracticeSchemeInfo.EnrolRefNo = udtSp.EnrolRefNo
                    Next

                    blnRes = True

            End Select


            If Not IsNothing(udtSp) Then
                udtSPVerificationBLL.SaveToSession(udtSPVerificationModel)
                udtSPBLL.SaveToSession(udtSp)
                udtPracticeBLL.SaveToSession(udtSp.PracticeList)
                udtProfBLL.SaveToSession(udtProfessionalList)
                udtBankBLL.SaveToSession(udtBankAcctList)
                udtSPAccountUpateBLL.SaveToSession(udtSPAcctUpdModel)
                'udtERNProcessedBLL.SaveToSession(udtERNProcessedList)
                udtPracticeSchemeInfoBLL.SaveToSession(udtPracticeSchemeInfoList)
            End If

            'blnRes = True
        Catch ex As Exception
            blnRes = False
            Throw
        End Try

        Return blnRes
    End Function

    Public Function GetServiceProviderParticularsByERN(ByVal strHKLID As String) As DataTable
        'Dim udtSP As ServiceProviderModel = Nothing
        Dim dt As New DataTable
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        Try
            dt = udtServiceProviderBLL.GetServiceProviderParticulasPermanentByHKID(strHKLID, udtDB)
        Catch ex As Exception
            Throw
        End Try


        Return dt
    End Function


    Public Function PageChecked(ByVal intActiveTab As Integer, ByVal strERN As String, ByVal strTableLocation As String) As Boolean
        Dim blnRes As Boolean = False

        Dim udtSPVerificationBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL

        Dim udtSPVerificationModel As ServiceProviderVerificationModel = Nothing

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL


        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)

                udtSPVerificationModel = udtSPVerificationBLL.GetSerivceProviderVerificationByERN(strERN, udtDB)
            Else
                udtSPVerificationModel = udtSPVerificationBLL.GetSPVerification
            End If

            If Not IsNothing(udtSPVerificationModel) Then
                'blnRes = True
                udtSPVerificationModel.UpdateBy = udtHCVUUser.UserID
                Select Case intActiveTab

                    Case 0
                        'SP Info Page Checked
                        udtSPVerificationModel.SPConfirmed = True

                    Case 1
                        'MO Page Checked
                        udtSPVerificationModel.MOConfirmed = True

                    Case 2
                        'Practice Page Checked
                        udtSPVerificationModel.PracticeConfirmed = True

                    Case 3
                        'Bank Page Checked
                        udtSPVerificationModel.BankAcctConfirmed = True

                    Case 4
                        'Scheme Page Checked
                        udtSPVerificationModel.SchemeConfirmed = True
                End Select
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then
                If Not IsNothing(udtSPVerificationModel) Then
                    Select Case intActiveTab
                        Case 0
                            'SP Info Page Checked
                            blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationSPConfirm(udtSPVerificationModel, udtDB)
                        Case 1
                            'MO Page Checked
                            blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationMOConfirm(udtSPVerificationModel, udtDB)
                        Case 2
                            'Practice Page Checked
                            blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationPracticeConfirm(udtSPVerificationModel, udtDB)
                        Case 3
                            'Bank Page Checked
                            blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationBankConfirm(udtSPVerificationModel, udtDB)
                        Case 4
                            'Schenme Page Checked
                            blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationSchemeConfirm(udtSPVerificationModel, udtDB)
                    End Select
                End If
            End If

            'If blnRes AndAlso Not IsNothing(udtSPVerificationModel) Then
            '    Select Case intActiveTab
            '        Case 0
            '            'SP Info Page Checked
            '            blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationSPConfirm(udtSPVerificationModel, udtDB)
            '        Case 1
            '            'MO Page Checked
            '            blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationMOConfirm(udtSPVerificationModel, udtDB)
            '        Case 2
            '            'Practice Page Checked
            '            blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationPracticeConfirm(udtSPVerificationModel, udtDB)
            '        Case 3
            '            'Bank Page Checked
            '            blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationBankConfirm(udtSPVerificationModel, udtDB)
            '        Case 4
            '            'Schenme Page Checked
            '            blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationSchemeConfirm(udtSPVerificationModel, udtDB)
            '    End Select
            'End If

            If blnRes Then
                udtDB.CommitTransaction()
                blnRes = True
                udtSPVerificationModel = udtSPVerificationBLL.GetSerivceProviderVerificationByERN(udtSPVerificationModel.EnrolRefNo, udtDB)
                udtSPVerificationBLL.SaveToSession(udtSPVerificationModel)
            Else
                udtDB.RollBackTranscation()
                blnRes = False
            End If
        Catch ex As Exception
            udtDB.RollBackTranscation()
            blnRes = False
            Throw
        End Try

        Return blnRes

    End Function

    Public Function ConfirmSPProfile() As Boolean
        Dim blnRes As Boolean = False
        Dim blnCheckProfessional As Boolean = False

        Dim blnUpdSPInfo As Boolean = True
        Dim blnUpdProfessional As Boolean = False
        Dim blnUpdBankAcct As Boolean = False
        Dim blnIssueToken As Boolean = False
        Dim blnSchemeConfirm As Boolean = False

        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtPracticeProviderBLL As PracticeBLL = New PracticeBLL
        Dim udtBankAcctBLL As BankAcctBLL = New BankAcctBLL
        Dim udtProfessionalBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtSPAccountUpdateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
        Dim udtProfessionalVerificationBLL As ProfessionalVerificationBLL = New ProfessionalVerificationBLL
        Dim udtBankAccVerificationBLL As BankAccVerificationBLL = New BankAccVerificationBLL
        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL

        Dim udtServiceProviderModel As ServiceProviderModel
        Dim udtSchemeInfoList As SchemeInformationModelCollection
        Dim udtProfessionalList As ProfessionalModelCollection
        Dim udtBankAcctList As BankAcctModelCollection
        ' CRP13-002 - Fix professional verification [Start][Koala]
        ' -------------------------------------------------------------------------------------
        Dim udtProfessionalVerificationList As ProfessionalVerificationModelCollection
        ' CRP13-002 - Fix professional verification [End][Koala]

        Dim udtSPAcctUpdModel As SPAccountUpdateModel = New SPAccountUpdateModel

        Dim udtProfessionalUpdateList As ProfessionalModelCollection = New ProfessionalModelCollection
        Dim udtBankAcctUpdateList As BankAcctModelCollection = New BankAcctModelCollection

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser


        If udtServiceProviderBLL.Exist AndAlso udtPracticeProviderBLL.Exist AndAlso udtProfessionalBLL.Exist AndAlso udtBankAcctBLL.Exist Then
            udtServiceProviderModel = udtServiceProviderBLL.GetSP

            Try
                udtDB.BeginTransaction()
                blnCheckProfessional = udtProfessionalBLL.DeleteProfessionalStaging(udtServiceProviderModel.EnrolRefNo, ProfessionalStagingStatus.Delete, udtDB)
                udtDB.CommitTransaction()
                udtDB.Dispose()
            Catch ex As Exception
                udtDB.RollBackTranscation()
                Throw
            End Try

            If blnCheckProfessional Then

                If udtServiceProviderModel.SPID.Trim.Equals(String.Empty) Then
                    blnUpdSPInfo = True
                    ' CRP13-002 - Fix professional verification [Start][Koala]
                    ' -------------------------------------------------------------------------------------
                    'blnUpdProfessional = True
                    ' CRP13-002 - Fix professional verification [End][Koala]
                    blnUpdBankAcct = True
                    blnIssueToken = True

                    udtBankAcctUpdateList = udtBankAcctBLL.GetBankAcctCollection
                    udtProfessionalUpdateList = udtProfessionalBLL.GetProfessinalListFromStagingByERN(udtServiceProviderModel.EnrolRefNo, udtDB)

                Else
                    udtProfessionalList = udtProfessionalBLL.GetProfessinalListFromStagingByERN(udtServiceProviderModel.EnrolRefNo, udtDB)

                    If Not IsNothing(udtProfessionalList) Then
                        For Each udtProfessionalModel As ProfessionalModel In udtProfessionalList.Values
                            If udtProfessionalModel.RecordStatus = ProfessionalStagingStatus.Active Then
                                ' CRP13-002 - Fix professional verification [Start][Koala]
                                ' -------------------------------------------------------------------------------------
                                'blnUpdProfessional = True
                                ' CRP13-002 - Fix professional verification [End][Koala]
                                udtProfessionalUpdateList.Add(udtProfessionalModel)
                            End If
                        Next
                    End If

                    udtBankAcctList = udtBankAcctBLL.GetBankAcctCollection

                    For Each udtBankAcctModel As BankAcctModel In udtBankAcctList.Values
                        If udtBankAcctModel.RecordStatus = BankAcctStagingStatus.Active Then
                            blnUpdBankAcct = True
                            udtBankAcctUpdateList.Add(udtBankAcctModel)
                        End If
                    Next
                End If

                ' CRP13-002 - Fix professional verification [Start][Koala]
                ' -------------------------------------------------------------------------------------
                ' Match ProfessionalStaging with ProfessionalVerification, if the the professional is confirmed, 
                ' then no need to process professional verification again
                udtProfessionalVerificationList = udtProfessionalVerificationBLL.GetProfessionalVerificationListByERN(udtServiceProviderModel.EnrolRefNo, udtDB)


                If Not IsNothing(udtProfessionalUpdateList) Then
                    For Each udtProfessionalModel As ProfessionalModel In udtProfessionalUpdateList.Values
                        If udtProfessionalModel.RecordStatus = ProfessionalStagingStatus.Active Then

                            Dim blnFoundPV As Boolean = False
                            For Each udtPV As ProfessionalVerificationModel In udtProfessionalVerificationList.Values
                                If udtPV.ProfessionalSeq = udtProfessionalModel.ProfessionalSeq Then
                                    blnFoundPV = True
                                    If udtPV.ConfirmBy = String.Empty And Not udtPV.ConfirmDtm.HasValue Then
                                        blnUpdProfessional = True
                                    End If
                                End If
                            Next

                            ' If new profession is no professional verification record found, that mean must process professional verification process
                            If Not blnFoundPV Then
                                blnUpdProfessional = True
                                Exit For
                            End If

                        End If
                    Next
                End If
                ' CRP13-002 - Fix professional verification [End][Koala]
            End If


            If udtServiceProviderModel.SPID.Trim.Equals(String.Empty) Then
                blnSchemeConfirm = False
            Else
                udtSchemeInfoList = udtServiceProviderModel.SchemeInfoList

                For Each udtschemeinfo As SchemeInformationModel In udtSchemeInfoList.Values
                    If udtschemeinfo.RecordStatus = SchemeInformationStagingStatus.Active Then
                        blnSchemeConfirm = True
                    End If
                Next
            End If

            udtSPAcctUpdModel = AddSPAccountUpdate(udtServiceProviderModel.EnrolRefNo, udtServiceProviderModel.SPID, blnUpdSPInfo, blnUpdBankAcct, blnUpdProfessional, blnIssueToken, blnSchemeConfirm)

            'Dim udtNewDB As Database = New Database
            Try
                udtDB.BeginTransaction()
                If UpdateServiceProviderVerificaton(ServiceProviderVerificationStatus.DataEntryConfirmed, udtDB) AndAlso _
                    udtSPAccountUpdateBLL.AddSPAccountUpdate(udtSPAcctUpdModel, udtDB) AndAlso _
                    udtProfessionalVerificationBLL.AddProfessionalVerification(udtServiceProviderModel.EnrolRefNo, udtProfessionalUpdateList, udtDB) AndAlso _
                    udtBankAccVerificationBLL.AddBankAccVerification(udtBankAcctUpdateList, udtDB) AndAlso _
                    Me.UpdateSchemeInfoStagingALL(udtServiceProviderModel.EnrolRefNo, udtServiceProviderModel.SPID, udtHCVUUser.UserID, udtDB) Then
                    'If udtSPAccountUpdateBLL.AddSPAccountUpdate(udtSPAcctUpdModel, udtNewDB) Then

                    udtDB.CommitTransaction()
                    blnRes = True
                Else
                    udtDB.RollBackTranscation()
                    blnRes = False
                End If

            Catch ex As Exception
                udtDB.RollBackTranscation()
                blnRes = False
                Throw
            End Try


        End If

        Return blnRes
    End Function

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Public Function VetSPProfile()
    Public Function VetSPProfile(ByVal strSPAccUpdateProgressStatus As String)
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

        Dim blnRes As Boolean = False
        Dim udtSPAccountUpdateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
        Dim udtSPAcctUpdModel As SPAccountUpdateModel

        If udtSPAccountUpdateBLL.Exist Then
            udtSPAcctUpdModel = udtSPAccountUpdateBLL.GetSPAccountUpdate

            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            udtSPAcctUpdModel.UpdateBy = udtHCVUUserBLL.GetHCVUUser.UserID.Trim
            udtSPAcctUpdModel.ProgressStatus = strSPAccUpdateProgressStatus 'SPAccountUpdateProgressStatus.BankAcctVerification
            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

            Try
                udtDB.BeginTransaction()
                If UpdateServiceProviderVerificaton(ServiceProviderVerificationStatus.Vetted, udtDB) AndAlso _
                    udtSPAccountUpdateBLL.UpdateSPAccountUpdateProgressStatus(udtSPAcctUpdModel, udtDB) Then

                    udtDB.CommitTransaction()
                    blnRes = True
                Else
                    udtDB.RollBackTranscation()
                    blnRes = False
                End If

            Catch ex As Exception
                udtDB.RollBackTranscation()
                blnRes = False
                Throw
            End Try
        Else
            blnRes = False
        End If

        Return blnRes
    End Function

    Public Function DeferSPProfile() As Boolean
        Dim blnRes As Boolean = False

        Try
            udtDB.BeginTransaction()
            If UpdateServiceProviderVerificaton(ServiceProviderVerificationStatus.Defer, udtDB) Then

                udtDB.CommitTransaction()
                blnRes = True
            Else
                udtDB.RollBackTranscation()
                blnRes = False
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            blnRes = False
            Throw
        End Try


        Return blnRes
    End Function

    Public Function ReturnAmendSPProfile() As Boolean
        Dim blnRes As Boolean = False
        Dim udtSPAccountUpdateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
        Dim udtSPAcctUpdModel As SPAccountUpdateModel

        If udtSPAccountUpdateBLL.Exist Then
            udtSPAcctUpdModel = udtSPAccountUpdateBLL.GetSPAccountUpdate

            udtSPAcctUpdModel.ProgressStatus = SPAccountUpdateProgressStatus.BankAcctVerification
            Try
                udtDB.BeginTransaction()
                If UpdateServiceProviderVerificaton(ServiceProviderVerificationStatus.ReturnForAmendment, udtDB) AndAlso _
                    udtSPAccountUpdateBLL.UpdateSPAccountUpdateProgressStatus(udtSPAcctUpdModel, udtDB) Then

                    udtDB.CommitTransaction()
                    blnRes = True
                Else
                    udtDB.RollBackTranscation()
                    blnRes = False
                End If

            Catch ex As Exception
                udtDB.RollBackTranscation()
                blnRes = False
                Throw
            End Try
        Else
            blnRes = False
        End If

        Return blnRes
    End Function

    Public Function RejectSPProfile() As Boolean
        Dim blnRes As Boolean = False
        Dim udtSPAccountUpdateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
        Dim udtSPAcctUpdModel As SPAccountUpdateModel

        If udtSPAccountUpdateBLL.Exist Then
            udtSPAcctUpdModel = udtSPAccountUpdateBLL.GetSPAccountUpdate

            udtSPAcctUpdModel.ProgressStatus = SPAccountUpdateProgressStatus.BankAcctVerification
            Try
                udtDB.BeginTransaction()
                If UpdateServiceProviderVerificaton(ServiceProviderVerificationStatus.Reject, udtDB) AndAlso _
                    udtSPAccountUpdateBLL.UpdateSPAccountUpdateProgressStatus(udtSPAcctUpdModel, udtDB) Then

                    udtDB.CommitTransaction()
                    blnRes = True
                Else
                    udtDB.RollBackTranscation()
                    blnRes = False
                End If

            Catch ex As Exception
                udtDB.RollBackTranscation()
                blnRes = False
                Throw
            End Try
        Else
            blnRes = False
        End If

        Return blnRes

    End Function

    Private Function UpdateServiceProviderVerificaton(ByVal strVerificationStatus As String, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtBankAcctBLL As BankAcctBLL = New BankAcctBLL

        Dim udtSPVerificationBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Dim udtSPVerificationModel As ServiceProviderVerificationModel

        Try
            'udtDB.BeginTransaction()
            If udtSPVerificationBLL.Exist Then
                udtSPVerificationModel = udtSPVerificationBLL.GetSPVerification
                udtSPVerificationModel.UpdateBy = udtHCVUUser.UserID

                Select Case strVerificationStatus
                    'Data Entry Confirmed
                    Case ServiceProviderVerificationStatus.DataEntryConfirmed
                        udtSPVerificationModel.EnterConfirmBy = udtHCVUUser.UserID
                        udtSPVerificationModel.RecordStatus = ServiceProviderVerificationStatus.DataEntryConfirmed
                        blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationEnterConfirmed(udtSPVerificationModel, udtDB)

                        'Vetting
                    Case ServiceProviderVerificationStatus.Vetted
                        udtSPVerificationModel.VettingBy = udtHCVUUser.UserID
                        udtSPVerificationModel.RecordStatus = ServiceProviderVerificationStatus.Vetted
                        blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationVetting(udtSPVerificationModel, udtDB)

                        'Defer
                    Case ServiceProviderVerificationStatus.Defer
                        udtSPVerificationModel.DeferBy = udtHCVUUser.UserID
                        udtSPVerificationModel.RecordStatus = ServiceProviderVerificationStatus.Defer
                        blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationDefer(udtSPVerificationModel, udtDB)

                        'Reture for amendment
                    Case ServiceProviderVerificationStatus.ReturnForAmendment
                        udtSPVerificationModel.ReturnForAmendmentBy = udtHCVUUser.UserID
                        udtSPVerificationModel.RecordStatus = ServiceProviderVerificationStatus.ReturnForAmendment
                        blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationReturnAmend(udtSPVerificationModel, udtDB)

                        'Reject
                    Case ServiceProviderVerificationStatus.Reject
                        udtSPVerificationModel.VoidBy = udtHCVUUser.UserID
                        udtSPVerificationModel.RecordStatus = ServiceProviderVerificationStatus.Reject
                        blnRes = udtSPVerificationBLL.UpdateServiceProviderVerificationReject(udtSPVerificationModel, udtDB)
                End Select

                'If blnRes Then
                'udtDB.CommitTransaction()
                blnRes = True
                'udtSPVerificationModel = udtSPVerificationBLL.GetSerivceProviderVerificationByERN(udtSPVerificationModel.EnrolRefNo, udtDB)
                'udtSPVerificationBLL.SaveToSession(udtSPVerificationModel)
            Else
                'udtDB.RollBackTranscation()
                blnRes = False
                'End If
            End If

        Catch ex As Exception
            'udtDB.RollBackTranscation()
            blnRes = False
            Throw
        End Try

        Return blnRes
    End Function


    Private Function AddSPAccountUpdate(ByVal strERN As String, ByVal strSPID As String, ByVal blnUpdSP As Boolean, ByVal blnUpdBankAcct As Boolean, _
                                                ByVal blnUpdProfessional As Boolean, ByVal blnIssueToken As Boolean, ByVal blnSchemeConfirm As Boolean) As SPAccountUpdateModel
        'Dim udtSPAcctUpdBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
        Dim udtSpAcctUpdModel As SPAccountUpdateModel = Nothing

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        ' INT13-0028 - SP Amendment Report [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'udtSpAcctUpdModel = New SPAccountUpdateModel(strERN, strSPID, blnUpdSP, blnUpdBankAcct, blnUpdProfessional, blnIssueToken, blnSchemeConfirm, SPAccountUpdateProgressStatus.VettingStage, _
        '                                                    udtHCVUUser.UserID, Nothing, Nothing)
        udtSpAcctUpdModel = New SPAccountUpdateModel(strERN, strSPID, blnUpdSP, blnUpdBankAcct, blnUpdProfessional, blnIssueToken, blnSchemeConfirm, SPAccountUpdateProgressStatus.VettingStage, _
                                                     udtHCVUUser.UserID, Nothing, Nothing, udtHCVUUser.UserID)
        ' INT13-0028 - SP Amendment Report [End][Tommy L]

        Return udtSpAcctUpdModel
    End Function

    Private Function UpdateSPVerifyAndAcctUpdSPInfo(ByVal intUpdateInfo As Integer, ByVal strUpdateBy As String, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Dim udtSPAcctUpdBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
        Dim udtSPVerificationBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL

        Dim udtSPVerificationModel As ServiceProviderVerificationModel

        If udtSPAcctUpdBLL.Exist AndAlso udtSPVerificationBLL.Exist Then
            udtSPVerificationModel = udtSPVerificationBLL.GetSPVerification

            Try
                Select Case intUpdateInfo
                    Case UpdateSPInfo

                        udtSPVerificationModel.SPConfirmed = False
                        udtSPVerificationModel.UpdateBy = strUpdateBy

                        udtSPVerificationBLL.UpdateServiceProviderVerificationSPConfirm(udtSPVerificationModel, udtDB)

                    Case UpdatePractice
                        udtSPVerificationModel.PracticeConfirmed = False
                        udtSPVerificationModel.UpdateBy = strUpdateBy
                        udtSPVerificationBLL.UpdateServiceProviderVerificationPracticeConfirm(udtSPVerificationModel, udtDB)

                    Case UpdateProfessional
                        udtSPVerificationModel.PracticeConfirmed = False
                        udtSPVerificationModel.UpdateBy = strUpdateBy

                        udtSPVerificationBLL.UpdateServiceProviderVerificationPracticeConfirm(udtSPVerificationModel, udtDB)

                    Case UpdatePracticeBank
                        udtSPVerificationModel.PracticeConfirmed = False
                        udtSPVerificationModel.BankAcctConfirmed = False
                        udtSPVerificationModel.SchemeConfirmed = False
                        udtSPVerificationModel.UpdateBy = strUpdateBy

                        udtSPVerificationBLL.UpdateServiceProviderVerificationPracticeBankConfirm(udtSPVerificationModel, udtDB)
                    Case UpdateBank
                        udtSPVerificationModel.BankAcctConfirmed = False
                        udtSPVerificationModel.UpdateBy = strUpdateBy

                        udtSPVerificationBLL.UpdateServiceProviderVerificationBankConfirm(udtSPVerificationModel, udtDB)

                    Case UpdateMO
                        udtSPVerificationModel.MOConfirmed = False
                        udtSPVerificationModel.UpdateBy = strUpdateBy

                        udtSPVerificationBLL.UpdateServiceProviderVerificationMOConfirm(udtSPVerificationModel, udtDB)

                    Case UpdateMOPractice
                        udtSPVerificationModel.MOConfirmed = False
                        udtSPVerificationModel.PracticeConfirmed = False
                        udtSPVerificationModel.UpdateBy = strUpdateBy

                        udtSPVerificationBLL.UpdateServiceProviderVerificationMOPracticeConfirm(udtSPVerificationModel, udtDB)

                    Case UpdateScheme
                        udtSPVerificationModel.SchemeConfirmed = False
                        udtSPVerificationModel.UpdateBy = strUpdateBy

                        udtSPVerificationBLL.UpdateServiceProviderVerificationSchemeConfirm(udtSPVerificationModel, udtDB)
                End Select
                blnRes = True
            Catch ex As Exception
                blnRes = False
                Throw
            End Try
        Else
            blnRes = False
        End If

        Return blnRes
    End Function

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Public Function DataEntrySearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal strScheme As String) As DataTable
    Public Function DataEntrySearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal strScheme As String, ByVal blnOverrideResultLimit As Boolean, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
        'Dim dtResult As DataTable
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Try
            'udtDB.BeginTransaction()
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'dtResult = udtServiceProviderBLL.GetServiceProviderDataEntrySearch(strERN, strSPID, strHKID, strEname, strPhone, strServiceCategoryCode, strStatus, strScheme, udtDB)
            Return udtServiceProviderBLL.GetServiceProviderDataEntrySearch(strFunctionCode, strERN, strSPID, strHKID, strEname, strPhone, strServiceCategoryCode, strStatus, strScheme, udtDB, blnOverrideResultLimit, blnForceUnlimitResult)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            'udtDB.CommitTransaction()

        Catch ex As Exception
            Throw
        End Try
        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Return dtResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    End Function

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Public Function VettingSearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal strSchemeCode As String) As DataTable
    Public Function VettingSearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strStatus As String, ByVal strSchemeCode As String, ByVal blnOverrideResultLimit As Boolean, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
        'Dim dtResult As DataTable
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Try
            'udtDB.BeginTransaction()
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'dtResult = udtServiceProviderBLL.GetServiceProviderVettingSearch(strERN, strSPID, strHKID, strEname, strPhone, strServiceCategoryCode, strStatus, strSchemeCode, SPAccountUpdateProgressStatus.VettingStage, udtDB)
            Return udtServiceProviderBLL.GetServiceProviderVettingSearch(strFunctionCode, strERN, strSPID, strHKID, strEname, strPhone, strServiceCategoryCode, strStatus, strSchemeCode, SPAccountUpdateProgressStatus.VettingStage, udtDB, blnOverrideResultLimit, blnForceUnlimitResult)
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            'udtDB.CommitTransaction()

        Catch ex As Exception
            Throw
        End Try
        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Return dtResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    End Function

    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
    ' -------------------------------------------------------------------------
    'Public Function EnquirySearch(ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strSchemeCode As String) As DataTable
    ' -----
    ' CRE17-012 (Add Chinese Search for SP and EHA) [Start][Marco]
    'Public Function EnquirySearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strSchemeCode As String, Optional ByVal blnOverrideResultLimit As Boolean = False, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
    Public Function EnquirySearch(ByVal strFunctionCode As String, ByVal strERN As String, ByVal strSPID As String, ByVal strHKID As String, ByVal strEname As String, ByVal strCname As String, ByVal strPhone As String, ByVal strServiceCategoryCode As String, ByVal strSchemeCode As String, Optional ByVal blnOverrideResultLimit As Boolean = False, Optional ByVal blnForceUnlimitResult As Boolean = False) As BaseBLL.BLLSearchResult
        'Dim dtResult As DataTable
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Try
            'udtDB.BeginTransaction()
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
            ' -------------------------------------------------------------------------
            'dtResult = udtServiceProviderBLL.GetServiceProviderEnquirySearch(strERN, strSPID, strHKID, strEname, strPhone, strServiceCategoryCode, strSchemeCode, udtDB)
            'Return udtServiceProviderBLL.GetServiceProviderEnquirySearch(strFunctionCode, strERN, strSPID, strHKID, strEname, strPhone, strServiceCategoryCode, strSchemeCode, udtDB, blnOverrideResultLimit, blnForceUnlimitResult)
            Return udtServiceProviderBLL.GetServiceProviderEnquirySearch(strFunctionCode, strERN, strSPID, strHKID, strEname, strCname, strPhone, strServiceCategoryCode, strSchemeCode, udtDB, blnOverrideResultLimit, blnForceUnlimitResult)
            ' CRE17-012 (Add Chinese Search for SP and EHA) [End]  [Marco]
            ' -----
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]
            'udtDB.CommitTransaction()

        Catch ex As Exception
            Throw
        End Try
        ' CRE12-014 - Relax 500 rows limit in back office platform [Start][Tommy L]
        ' -------------------------------------------------------------------------
        'Return dtResult
        ' CRE12-014 - Relax 500 rows limit in back office platform [End][Tommy L]

    End Function

    Public Function DataEntryBatchResultSearch(ByVal strERN As String)
        Dim dtResult As DataTable
        'Dim udtERNProcessedBLL As ERNProcessedBLL = New ERNProcessedBLL
        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL

        Try
            'dtResult = udtERNProcessedBLL.GetERNProcessedListWithSPByERN(strERN, udtDB)
            dtResult = udtSPBLL.GetServiceProviderSchemeInfoEnrolmentByERN(strERN, udtDB)
        Catch ex As Exception
            Throw
        End Try

        Return dtResult
    End Function

    Public Sub GetServiceProviderHKID(ByVal strERN As String, ByVal strSPID As String, ByRef strHKID As String, ByRef strResERN As String)
        Dim dtRes As DataTable


        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Try
            dtRes = udtSPBLL.GetServiceProviderHKIDByERNOrSPID(strERN, strSPID, udtDB)

            If Not IsNothing(dtRes) Then
                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                ' Handle returned null HKID when query by not exist SPID
                If dtRes.Rows.Count = 1 AndAlso dtRes.Rows(0).Item("HKID") IsNot DBNull.Value Then
                    strHKID = CStr(dtRes.Rows(0).Item("HKID")).Trim
                    strResERN = CStr(dtRes.Rows(0).Item("Enrolment_Ref_No")).Trim
                Else
                    strHKID = String.Empty
                    strResERN = String.Empty
                End If
                ' CRE12-001 eHS and PCD integration [End][Koala]
            Else
                strHKID = String.Empty
                strResERN = String.Empty
            End If

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub ClearSession()

        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtProfessionalBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtBankAcctBLL As BankAcctBLL = New BankAcctBLL
        Dim udtSPVerificationBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL
        Dim udtSPAcctUpdateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL

        udtServiceProviderBLL.ClearSession()
        udtPracticeBLL.ClearSession()
        udtBankAcctBLL.ClearSession()
        udtProfessionalBLL.ClearSession()
        udtSPVerificationBLL.ClearSession()
        udtSPAcctUpdateBLL.ClearSession()
    End Sub

    'Public Sub SetPPIePRStatus(ByVal strAlreadyJoin As String, ByVal strWillJoin As String, ByRef udtSP As ServiceProviderModel)
    '    'Dim blnRes As Boolean = False
    '    'Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

    '    'Dim udtSP As ServiceProviderModel

    '    Dim udtHCVUUser As HCVUUserModel
    '    Dim udtHCVUUserBLL As New HCVUUserBLL
    '    udtHCVUUser = udtHCVUUserBLL.GetHCVUUser


    '    'If udtServiceProviderBLL.Exist Then
    '    'udtSP = udtServiceProviderBLL.GetSP

    '    udtSP.UpdateBy = udtHCVUUser.UserID

    '    If strAlreadyJoin.Equals(String.Empty) Then
    '        udtSP.AlreadyJoinHAPPI = JoinPPIePRStatus.NA
    '        udtSP.JoinHAPPI = JoinPPIePRStatus.NA
    '    Else
    '        udtSP.AlreadyJoinHAPPI = strAlreadyJoin
    '        If strWillJoin.Equals(String.Empty) Then
    '            udtSP.JoinHAPPI = JoinPPIePRStatus.NA
    '        Else
    '            udtSP.JoinHAPPI = strWillJoin
    '        End If
    '    End If
    '    'End If
    'End Sub

    Public Sub SetServiceFee(ByVal intServiceFeeFrom As Integer, ByVal intServiceFeeTo As Integer, ByRef udtSchemeInfoList As SchemeInformationModelCollection)
        'Dim udtServiceProviderBLl As ServiceProviderBLL = New ServiceProviderBLL
        'Dim udtSP As ServiceProviderModel
        Dim udtSchemeModel As SchemeInformationModel = Nothing

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        'If udtServiceProviderBLl.Exist Then
        'udtSP = udtServiceProviderBLl.GetSP

        For Each udtScheme As SchemeInformationModel In udtSchemeInfoList.Values 'udtSP.SchemeInfoList.Values
            If udtScheme.SchemeCode.Equals(SchemeCode.IVSS) Then
                'udtScheme.ServiceFeeFrom = intServiceFeeFrom
                'udtScheme.ServiceFeeTo = intServiceFeeTo
                udtScheme.UpdateBy = udtHCVUUser.UserID
            End If
        Next
        'End If
    End Sub



    Public Function IsHKIDExistingInServiceProviderStagingPermanentByERN(ByVal strERN As String) As Boolean
        Dim blnRes As Boolean = False
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim dt As DataTable = New DataTable

        Try
            dt = udtServiceProviderBLL.GetServiceProviderStagingPermanentHKICRowCountByERN(strERN, udtDB)

            If dt.Rows(0)(0) > 0 Then
                'There is existing HKID in Table "ServiceProvider" or "ServiceProviderStaging"
                blnRes = True
            Else
                blnRes = False
            End If
        Catch ex As Exception
            Throw
        End Try

        Return blnRes
    End Function

    Public Function IsHKIDExistingInServiceProviderStagingPermanentByHKID(ByVal strHKID As String) As Boolean
        Dim blnRes As Boolean = False
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim dt As DataTable = New DataTable

        strHKID = udtFormatter.formatHKIDInternal(strHKID)

        Try
            dt = udtServiceProviderBLL.GetServiceProviderStagingPermanentHKICRowCountByHKID(strHKID, udtDB)

            If dt.Rows(0)(0) > 0 Then
                'There is existing HKID in Table "ServiceProvider" or "ServiceProviderStaging"
                blnRes = True
            Else
                blnRes = False
            End If
        Catch ex As Exception
            Throw
        End Try

        Return blnRes
    End Function

    Public Function GetEnrolmentProcessStatus(ByVal strERN As String) As String
        Dim strRes As String = String.Empty
        Dim udtSPAccountUpateBLL As SPAccountUpdateBLL = New SPAccountUpdateBLL
        Dim udtSPAcctUpdModel As SPAccountUpdateModel

        udtSPAcctUpdModel = udtSPAccountUpateBLL.GetSPAccountUpdateByERN(strERN, udtDB)

        If Not IsNothing(udtSPAcctUpdModel) Then
            'If udtSPAccountUpateBLL.Exist Then
            'udtSPAcctUpdModel = udtSPAccountUpateBLL.GetSPAccountUpdate

            Select Case udtSPAcctUpdModel.ProgressStatus
                Case SPAccountUpdateProgressStatus.VettingStage
                    strRes = "00004"
                Case SPAccountUpdateProgressStatus.BankAcctVerification
                    strRes = "00005"
                Case SPAccountUpdateProgressStatus.ProfessionalVerification
                    strRes = "00006"
                Case SPAccountUpdateProgressStatus.WaitingForIssueToken
                    strRes = "00007"
                Case Else
                    strRes = String.Empty
            End Select
        Else
            strRes = Nothing
        End If

        Return strRes

    End Function

    Public Function getNoOfRecordSPEnrolment() As Integer
        Dim intRes As Integer = 0
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim dt As DataTable = New DataTable

        Try
            dt = udtServiceProviderBLL.GetServiceProviderEnrolmentRowCount(udtDB)

            If dt.Rows(0)(0) > 0 Then
                intRes = CInt(dt.Rows(0)(0))
            End If
        Catch ex As Exception
            Throw
        End Try

        Return intRes
    End Function


    Public Function AddServiceProvideProfileToPermanent(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtPracticeModelCollection As PracticeModelCollection, ByVal udtBankAcctModelCollection As BankAcctModelCollection, ByVal udtProfessionalModelCollection As ProfessionalModelCollection, ByVal udtSchemeInformationCollection As SchemeInformationModelCollection, ByRef udtDB As Database, ByVal udtERNProcessedModelCollection As ERNProcessedModelCollection) As Boolean
        Dim blnRes As Boolean = False
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        Try
            If udtServiceProviderBLL.AddServiceProviderProfileToPermanent(udtServiceProviderModel, udtPracticeModelCollection, udtBankAcctModelCollection, udtProfessionalModelCollection, udtSchemeInformationCollection, udtDB, udtERNProcessedModelCollection) Then
                blnRes = True
            Else
                blnRes = False
            End If
        Catch ex As Exception
            blnRes = False
            Throw
        End Try

        Return blnRes
    End Function

    Public Function UpdateServiceProvideProfileToPermanent(ByVal udtServiceProviderModel As ServiceProviderModel, ByVal udtPracticeModelCollection As PracticeModelCollection, ByVal udtBankAcctModelCollection As BankAcctModelCollection, ByVal udtProfessionalModelCollection As ProfessionalModelCollection, ByVal udtSchemeInformationCollection As SchemeInformationModelCollection, ByRef udtDB As Database, ByVal udtERNProcessedModelCollection As ERNProcessedModelCollection, ByVal udtSchemeInfoPermanentList As SchemeInformationModelCollection, ByVal udtPermanentERNList As ERNProcessed.ERNProcessedModelCollection, ByVal udtPracticeSchemeListPermanent As PracticeSchemeInfoModelCollection) As Boolean
        Dim blnRes As Boolean = False
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        Try
            If udtServiceProviderBLL.UpdateServiceProviderProfileToPermanentBySchemeEnrolment(udtServiceProviderModel, udtPracticeModelCollection, udtBankAcctModelCollection, udtProfessionalModelCollection, udtSchemeInformationCollection, udtDB, udtERNProcessedModelCollection, udtSchemeInfoPermanentList, udtPermanentERNList, udtPracticeSchemeListPermanent) Then
                blnRes = True
            Else
                blnRes = False
            End If
        Catch ex As Exception
            blnRes = False
            Throw
        End Try

        Return blnRes
    End Function

    Public Function IsAbleToVet() As Boolean
        Dim blnRes As Boolean = False

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL
        udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

        Dim udtSPVerificationBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL
        Dim udtSPVerificationModel As ServiceProviderVerificationModel = Nothing

        If udtSPVerificationBLL.Exist Then
            udtSPVerificationModel = udtSPVerificationBLL.GetSPVerification

            If udtSPVerificationModel.EnterConfirmBy.Trim = udtHCVUUser.UserID.Trim Then
                blnRes = False
            Else
                blnRes = True
            End If
        End If

        Return blnRes
    End Function


    Public Function isAbleToJoinIVSS(ByVal udtPracticeList As PracticeModelCollection, ByVal intEditPracticeSeq As Integer) As Boolean
        Dim blnRes As Boolean = False
        For Each udtPracticeModel As PracticeModel In udtPracticeList.Values
            If Not IsNothing(udtPracticeModel) AndAlso Not IsNothing(udtPracticeModel.Professional.ServiceCategoryCode) AndAlso Not udtPracticeModel.Professional.ServiceCategoryCode.Equals(String.Empty) Then
                If udtPracticeModel.DisplaySeq = intEditPracticeSeq Then
                Else
                    If Me.AskJoinIVSS(udtPracticeModel.Professional.ServiceCategoryCode) Then
                        blnRes = True
                    End If
                End If
            End If
        Next

        Return blnRes
    End Function

    Private Function UpdateSPEmail(ByRef udtDB As Database, ByVal udtSP As ServiceProviderModel, ByVal blnCheckTSMP As Boolean, ByVal strUpdateBy As String) As Boolean
        Dim blnRes As Boolean = False

        Try
            Dim parms() As SqlParameter = { _
                udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, udtSP.SPID), _
                udtDB.MakeInParam("@update_by", ServiceProviderModel.UpdateByDataType, ServiceProviderModel.UpdateByDataSize, strUpdateBy), _
                udtDB.MakeInParam("@email", ServiceProviderModel.EmailDataType, ServiceProviderModel.EmailDataSize, udtSP.Email), _
                udtDB.MakeInParam("@TSMP", ServiceProviderModel.TSMPDataType, ServiceProviderModel.TSMPDataSize, IIf(blnCheckTSMP, udtSP.TSMP, DBNull.Value)), _
                udtDB.MakeInParam("@checkTSMP", SqlDbType.TinyInt, 1, blnCheckTSMP) _
                }

            udtDB.RunProc("proc_ServiceProvider_upd_EmailByHCVU", parms)
            blnRes = True
        Catch ex As Exception
            Throw
        End Try

        Return blnRes
    End Function

    Public Function GetHCSPUserACStatus(ByVal strSPID As String, ByRef udtDB As Database) As DataTable

        Dim dtUser As New DataTable

        Dim parms() As SqlParameter = { _
            udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
        udtDB.RunProc("proc_HCSPUserAC_get_status_bySPID", parms, dtUser)

        Return dtUser
    End Function

    Public Function GetSPSchemeInformationStagingSchemeName(ByVal strERN As String) As String

        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        Try
            Return udtServiceProviderBLL.GetSPSchemeInformationStagingSchemeName(strERN)
        Catch ex As Exception
            Throw
        End Try
    End Function

#Region "Data Validation"

    ' INT14-0022 - Fix delisting the last active schemes with all other suspended [Start][Lawrence]
    Public Function CheckUnsynchronizeRecord(ByVal udtSPStag As ServiceProviderModel, ByVal udtSPPerm As ServiceProviderModel) As Boolean

        ' Unsynchronize case:
        ' (1) Practice or Practice Scheme is under amendment but which is already delisted
        ' (2) Adding a practice scheme where the Practice is already delisted 

        udtSPPerm.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID(udtSPPerm.SPID, udtDB)
        udtSPStag.PracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtSPStag.EnrolRefNo, udtDB)

        For Each udtPracticeStag As PracticeModel In udtSPStag.PracticeList.Values
            Dim udtPracticePerm As PracticeModel = udtSPPerm.PracticeList(udtPracticeStag.DisplaySeq)

            If IsNothing(udtPracticePerm) Then Continue For

            If udtPracticeStag.RecordStatus = PracticeStagingStatus.Update AndAlso udtPracticePerm.RecordStatus = PracticeStatus.Delisted Then
                Return True
            End If

            Dim udtPracticeSchemeListPerm As New PracticeSchemeInfoModelCollection

            For Each udtPracticeSchemePerm As PracticeSchemeInfoModel In udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListBySPID(udtSPPerm.SPID, udtDB).Values
                If udtPracticeSchemePerm.PracticeDisplaySeq = udtPracticePerm.DisplaySeq Then
                    udtPracticeSchemeListPerm.Add(udtPracticeSchemePerm)
                End If
            Next

            Dim udtPracticeSchemeListStag As New PracticeSchemeInfoModelCollection

            For Each udtPracticeSchemeStag As PracticeSchemeInfoModel In udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByERN(udtSPStag.EnrolRefNo, udtDB).Values
                If udtPracticeSchemeStag.PracticeDisplaySeq = udtPracticeStag.DisplaySeq Then
                    udtPracticeSchemeListStag.Add(udtPracticeSchemeStag)
                End If
            Next

            udtPracticePerm.PracticeSchemeInfoList = udtPracticeSchemeListPerm
            udtPracticeStag.PracticeSchemeInfoList = udtPracticeSchemeListStag

            For Each udtPracticeSchemeStag As PracticeSchemeInfoModel In udtPracticeStag.PracticeSchemeInfoList.Values
                Dim udtPracticeSchemePerm As PracticeSchemeInfoModel = udtPracticePerm.PracticeSchemeInfoList.Filter(udtPracticeSchemeStag.SchemeCode, udtPracticeSchemeStag.SubsidizeCode)

                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Case (2) Adding a practice scheme where the Practice is already delisted
                If udtPracticeSchemeStag.RecordStatus = PracticeSchemeInfoStagingStatus.Active AndAlso udtPracticePerm.RecordStatus = PracticeStatus.Delisted Then
                    Return True
                End If
                ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                If IsNothing(udtPracticeSchemePerm) Then Continue For

                If udtPracticeSchemeStag.RecordStatus = PracticeSchemeInfoStagingStatus.Update AndAlso udtPracticeSchemePerm.RecordStatus = PracticeSchemeInfoStatus.Delisted Then
                    Return True
                End If

            Next
        Next

        Return False

    End Function
    ' INT14-0022 - Fix delisting the last active schemes with all other suspended [End][Lawrence]

    ' CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]
    Public Function CheckSchemeEligibleForHealthProf(ByVal strHealthProf As String, ByVal PracticeSchemeInfoList As PracticeSchemeInfoModelCollection) As Boolean

        If Not IsNothing(PracticeSchemeInfoList) Then

            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In PracticeSchemeInfoList.Values

                Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = Nothing
                udtSchemeBackOfficeList = udtSchemeBackOfficeBLL.GetAllEffectiveSchemeBackOfficeWithSubsidizeGroupFromCache

                If IsNothing(udtSchemeBackOfficeList) Then Return True

                udtSchemeBackOfficeList = udtSchemeBackOfficeList.FilterByProfCode(strHealthProf)

                'If any scheme is not eligible for Profession, validation fail
                If Not IsNothing(udtSchemeBackOfficeList) Then
                    If IsNothing(udtSchemeBackOfficeList.Filter(udtPracticeSchemeInfo.SchemeCode)) Then
                        Return False
                    End If
                Else
                    Return False
                End If


            Next
        End If

        Return True

    End Function
    ' CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]

    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    Public Function CheckBankAllowFreeFormat(ByVal GetSP As ServiceProviderModel) As String
        Dim strPracticeIndex As String = String.Empty

        If Not IsNothing(GetSP.PracticeList) AndAlso GetSP.PracticeList.Count > 0 Then
            For Each udtPractice As PracticeModel In GetSP.PracticeList.Values
                If Not IsNothing(udtPractice.BankAcct) AndAlso Not IsNothing(udtPractice.PracticeSchemeInfoList) Then

                    If udtPractice.BankAcct.IsFreeTextFormat.Trim = YesNo.Yes Then
                        For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                            'Dim udtSchemeBO As New SchemeBackOfficeBLL
                            'udtSchemeBO.getAllDistinctSchemeBackOffice()

                            Dim udtSchemeBackOffice As SchemeBackOfficeModel
                            Dim udtSchemeBackOfficeBLL As New SchemeBackOfficeBLL
                            udtSchemeBackOffice = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup().Filter(udtPracticeSchemeInfo.SchemeCode)

                            'If at least one scheme not allow free format, validation fail
                            If Not IsNothing(udtSchemeBackOffice) Then
                                If udtSchemeBackOffice.AllowFreeTextBankACNo.Trim <> YesNo.Yes Then
                                    strPracticeIndex = strPracticeIndex + ", " + udtPractice.DisplaySeq.ToString
                                    Exit For
                                End If
                            End If
                        Next
                    End If
                End If
            Next
        End If

        Return strPracticeIndex

    End Function
    'CRE13-019-02 Extend HCVS to China [End][Winnie]

    ' INT17-0012 (Fix EForm Bank Account can Next with no input) [Start] [Winnie]
    Public Function CheckBankBranchName(ByVal GetSP As ServiceProviderModel) As String
        Dim strPracticeIndex As String = String.Empty

        If Not IsNothing(GetSP.PracticeList) AndAlso GetSP.PracticeList.Count > 0 Then
            For Each udtPractice As PracticeModel In GetSP.PracticeList.Values
                'Check Branch Name for new record only
                If udtPractice.RecordStatus = PracticeStagingStatus.Active AndAlso Not IsNothing(udtPractice.BankAcct) Then
                    Dim udtSM As SystemMessage = udtValidator.chkBranchName(udtPractice.BankAcct.BranchName)

                    If Not IsNothing(udtSM) Then
                        strPracticeIndex = strPracticeIndex + ", " + udtPractice.DisplaySeq.ToString
                    End If
                End If
            Next
        End If

        Return strPracticeIndex

    End Function
    ' INT17-0012 (Fix EForm Bank Account can Next with no input) [End] [Winnie]

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ''' <summary>
    ''' Check whether the enrolling scheme is compulsory to join PCD
    ''' </summary>
    ''' <param name="udtSchemeInfoList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsJoinPCDCompulsory(ByVal udtSchemeInfoList As SchemeInformationModelCollection) As Boolean
        Dim blnCompulsory As Boolean = False
        Dim udtSchemeBackOfficeBLL As SchemeBackOfficeBLL = New SchemeBackOfficeBLL
        Dim udtSchemeBackOfficeList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup

        For Each udtSchemeInfoModel As SchemeInformationModel In udtSchemeInfoList.Values

            ' New scheme only
            If udtSchemeInfoModel.RecordStatus <> SchemeInformationStagingStatus.Active Then
                Continue For
            End If

            Dim udtSchemeBackOfficeModel As SchemeBackOfficeModel = udtSchemeBackOfficeList.Filter(udtSchemeInfoModel.SchemeCode)
            If udtSchemeBackOfficeModel.JoinPCDCompulsory = YesNo.Yes Then
                blnCompulsory = True
                Exit For
            End If
        Next

        Return blnCompulsory
    End Function
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Winnie]

    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Function CheckChangeEmailWithoutToken(ByRef udtSP As ServiceProviderModel) As Boolean
        Dim blnRes As Boolean = False

        Dim udtServiceProviderBLL As New ServiceProviderBLL

        Dim udtSPStaging As ServiceProviderModel
        udtSPStaging = udtServiceProviderBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, New Common.DataAccess.Database)

        Dim udtTokenBLL As TokenBLL = New TokenBLL
        Dim udtToken As TokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtSP.SPID, New Common.DataAccess.Database)

        If udtSPStaging.EmailChanged = Common.Component.EmailChanged.Changed And IsNothing(udtToken) Then
            Return True
        End If

        Return blnRes
    End Function
    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

    'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Function CheckUnsynchronizeRecord(ByRef udtSP As ServiceProviderModel) As Boolean
        Dim blnRes As Boolean = False

        If String.IsNullOrEmpty(udtSP.SPID) = False Then

            Dim udtServiceProviderBLL As New ServiceProviderBLL

            Dim udtSPStaging As ServiceProviderModel
            udtSPStaging = udtServiceProviderBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, New Common.DataAccess.Database)

            Dim udtSPPermenant As ServiceProviderModel
            udtSPPermenant = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(udtSP.EnrolRefNo, New Common.DataAccess.Database)

            Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL
            If udtSPProfileBLL.CheckUnsynchronizeRecord(udtSPStaging, udtSPPermenant) Then
                Return True
            End If
        End If

        Return blnRes
    End Function
    'CRE14-002 - PPI-ePR Migration [End][Chris YIM]
#End Region

#Region "Reject Enrolment"

    'Public Function RejectSPProfilenNewEnrolment(ByVal udtSP As ServiceProviderModel, ByVal udtSchemeInfoList As SchemeInformationModelCollection, _
    '                                            ByVal udtMOList As MedicalOrganizationModelCollection, ByVal udtERNProcessedList As ERNProcessedModelCollection, _
    '                                            ByVal udtPracticeList As PracticeModelCollection, ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, _
    '                                            ByVal udtProfessionalList As ProfessionalModelCollection, ByVal udtBankAcctList As BankAcctModelCollection, ByVal strUserId As String) As Boolean

    Public Function RejectSPProfilenNewEnrolment(ByVal udtSP As ServiceProviderModel, ByVal strUserId As String) As Boolean

        Dim blnRes As Boolean = False
        Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
        Dim dtBAVer As DataTable = Nothing
        Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing

        Dim udtSPStagingModel As ServiceProviderModel = Nothing
        Dim udtPractStagingModelCollection As PracticeModelCollection = Nothing
        Dim udtBAStagingModelCollection As BankAcctModelCollection = Nothing
        Dim udtPStagingModelCollection As ProfessionalModelCollection = Nothing

        Dim udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection = Nothing

        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing



        'UpdateProfileInfoForSaveToStaging(udtSP, udtPracticeList, udtBankAcctList, udtProfessionalList)

        Try
            udtDB.BeginTransaction()


            blnRes = SaveSessionObjectToStagingForReject(TableLocation.Enrolment, udtDB)

            If blnRes Then
                ' Retrieve Profile
                Me.RetrieveSPProfileForReject(Me.udtDB, udtSP.EnrolRefNo, _
                    udtSPVerModel, dtBAVer, udtPVModelCollection, _
                    udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                    udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

                udtSPAccUpdateModel = Nothing

                blnRes = Me.RejectSPProfileCore(Me.udtDB, udtSP.EnrolRefNo, strUserId, _
                               udtSPVerModel, dtBAVer, udtPVModelCollection, _
                               udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                               udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If


            'If AddServiceProvideProfileToStaging(udtSP, udtSchemeInfoList, udtMOList, udtERNProcessedList, udtPracticeList, udtPracticeSchemeInfoList, _
            '                                    udtBankAcctList, udtProfessionalList) AndAlso _
            '    udtServiceProviderBLL.DeleteServiceProviderEnrolmentProfile(udtSP.EnrolRefNo, udtDB) Then

            '    ' Retrieve Profile
            '    Me.RetrieveSPProfileForReject(Me.udtDB, udtSP.EnrolRefNo, _
            '        udtSPVerModel, dtBAVer, udtPVModelCollection, _
            '        udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
            '        udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

            '    udtSPAccUpdateModel = Nothing

            '    blnRes = Me.RejectSPProfileCore(Me.udtDB, udtSP.EnrolRefNo, strUserId, _
            '                   udtSPVerModel, dtBAVer, udtPVModelCollection, _
            '                   udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
            '                   udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

            '    If blnRes Then
            '        udtDB.CommitTransaction()
            '    Else
            '        udtDB.RollBackTranscation()
            '    End If

            'Else
            '    udtDB.RollBackTranscation()
            '    blnRes = False
            'End If

        Catch ex As InvalidOperationException
            Me.udtDB.RollBackTranscation()
            Throw
        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw
            Return False
        End Try

        Return blnRes
    End Function



    ''' <summary>
    ''' [Public] Reject Single SP Profile From User A
    ''' </summary>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="tsmpSPVerification">SP Verification TSMP</param>
    ''' <returns></returns>
    ''' <remarks>User A Reject, SPAccountUpdate Not Yet Create, Any Update on User A result in SP Verification Updated</remarks>
    Public Function RejectSPProfileFromUserA(ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
        ByVal tsmpSPVerification As Byte()) As Boolean
        ' Concurrency Validation
        ' UserA: Validate on [ServiceProviderVerification].TSMP  (No SPAccountUpdate)

        Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
        Dim dtBAVer As DataTable = Nothing
        Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing

        Dim udtSPStagingModel As ServiceProviderModel = Nothing
        Dim udtPractStagingModelCollection As PracticeModelCollection = Nothing
        Dim udtBAStagingModelCollection As BankAcctModelCollection = Nothing
        Dim udtPStagingModelCollection As ProfessionalModelCollection = Nothing
        Dim udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection = Nothing

        Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

        Dim blnSuccess As Boolean = True
        Try
            Me.udtDB.BeginTransaction()

            ' Retrieve Profile
            Me.RetrieveSPProfileForReject(Me.udtDB, strEnrolmentRefNo, _
                udtSPVerModel, dtBAVer, udtPVModelCollection, _
                udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

            udtSPAccUpdateModel = Nothing

            ' Validation
            ' UserA: Validate on [ServiceProviderVerification].TSMP  (No SPAccountUpdate)
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

            If Not IsNothing(udtSPVerModel) Then
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPVerModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSPVerification)}
                udtDB.RunProc("proc_checkTSMP", params)
            Else
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSPVerification)}
                udtDB.RunProc("proc_checkTSMP", params)
            End If

            ' Reject Enrolment
            blnSuccess = Me.RejectSPProfileCore(Me.udtDB, strEnrolmentRefNo, strUserId, _
                            udtSPVerModel, dtBAVer, udtPVModelCollection, _
                            udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                            udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

            If blnSuccess Then
                Me.udtDB.CommitTransaction()
            Else
                Me.udtDB.RollBackTranscation()
            End If
            Return blnSuccess

        Catch ex As InvalidOperationException
            Me.udtDB.RollBackTranscation()
            Throw
        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw
            Return False
        End Try
    End Function

    ''' <summary>
    '''  [Public] Reject Single SP Profile From User B
    ''' </summary>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="tsmpSpAccUpdate">SPAccountUpdate TSMP</param>
    ''' <param name="tsmpSPVerification">SP Verification TSMP</param>
    ''' <returns></returns>
    ''' <remarks>User B Reject, Check SPAccount, Any Update on User B result in SP Account Updated</remarks>
    Public Function RejectSPProfileFromUserB(ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
        ByVal tsmpSpAccUpdate As Byte(), ByVal tsmpSPVerification As Byte()) As Boolean
        ' Concurrency Validation
        ' UserB: Validate on [SPAccountUpdate].TSMP + [ServiceProviderVerification].TSMP

        Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
        Dim dtBAVer As DataTable = Nothing
        Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing

        Dim udtSPStagingModel As ServiceProviderModel = Nothing
        Dim udtPractStagingModelCollection As PracticeModelCollection = Nothing
        Dim udtBAStagingModelCollection As BankAcctModelCollection = Nothing
        Dim udtPStagingModelCollection As ProfessionalModelCollection = Nothing
        Dim udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection = Nothing

        '    udtSP.ApplicationPrinted = String.Empty
        Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

        Dim blnSuccess As Boolean = True
        Try
            Me.udtDB.BeginTransaction()

            ' Retrieve Profile
            Me.RetrieveSPProfileForReject(Me.udtDB, strEnrolmentRefNo, _
                udtSPVerModel, dtBAVer, udtPVModelCollection, _
                udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

            ' Validation
            ' UserB: Validate on [SPAccountUpdate].TSMP + [ServiceProviderVerification].TSMP
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

            If Not IsNothing(udtSPVerModel) Then
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPVerModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSPVerification)}
                udtDB.RunProc("proc_checkTSMP", params)
            Else
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSPVerification)}
                udtDB.RunProc("proc_checkTSMP", params)
            End If

            If Not IsNothing(udtSPAccUpdateModel) Then
                Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPAccUpdateModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params2)
            Else
                Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params2)
            End If

            ' Reject Enrolment
            blnSuccess = Me.RejectSPProfileCore(Me.udtDB, strEnrolmentRefNo, strUserId, _
                            udtSPVerModel, dtBAVer, udtPVModelCollection, _
                            udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                            udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

            If blnSuccess Then
                Me.udtDB.CommitTransaction()
            Else
                Me.udtDB.RollBackTranscation()
            End If
            Return blnSuccess

        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw
            Return False
        End Try
    End Function

    ''' <summary>
    ''' [Public] Reject Single SP Profile From User C
    ''' </summary>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="tsmpSpAccUpdate">SPAccountUpdate TSMP</param>
    ''' <param name="lstTsmpBankAccVerification">Bank Account Verification List</param>
    ''' <param name="lstIntDisplaySeq">Seq List to Indentify the Bank Account Verification Record</param>
    ''' <returns></returns>
    ''' <remarks>User C Reject, Check SPAccount + Check Each of Bank Account Verfiication, Both May be Update in Different Case</remarks>
    Public Function RejectSPProfileFromUserC(ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
        ByVal tsmpSpAccUpdate As Byte(), ByVal lstTsmpBankAccVerification As List(Of Byte()), ByVal lstIntDisplaySeq As List(Of Integer), ByVal lstSPPracticeDisplaySeq As List(Of Integer)) As Boolean
        ' Concurrency Validation
        ' UserC: Validate on [SPAccountUpdate].TSMP + [BankAccVerification].TSMP (s)

        Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
        Dim dtBAVer As DataTable = Nothing
        Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing

        Dim udtSPStagingModel As ServiceProviderModel = Nothing
        Dim udtPractStagingModelCollection As PracticeModelCollection = Nothing
        Dim udtBAStagingModelCollection As BankAcctModelCollection = Nothing
        Dim udtPStagingModelCollection As ProfessionalModelCollection = Nothing
        Dim udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection = Nothing

        Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

        Dim blnSuccess As Boolean = True
        Try
            Me.udtDB.BeginTransaction()

            ' Retrieve Profile
            Me.RetrieveSPProfileForReject(Me.udtDB, strEnrolmentRefNo, _
                udtSPVerModel, dtBAVer, udtPVModelCollection, _
                udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

            ' Validation
            ' UserC: Validate on [SPAccountUpdate].TSMP + [BankAccVerification].TSMP (s)
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

            If lstSPPracticeDisplaySeq.Count <> lstTsmpBankAccVerification.Count Then
                Throw New ArgumentException("BankAccVerification TSMP Argument Invalid")
            End If

            For Each drBankAccVer As DataRow In dtBAVer.Rows
                'Dim intSeq As Integer = Convert.ToInt32(drBankAccVer("display_Seq"))
                Dim intSeq As Integer = Convert.ToInt32(drBankAccVer("sp_practice_display_Seq"))
                Dim arrByteTSMP As Byte() = CType(drBankAccVer("TSMP"), Byte())

                Dim blnFound As Boolean = False
                'For j As Integer = 0 To lstIntDisplaySeq.Count - 1
                For j As Integer = 0 To lstSPPracticeDisplaySeq.Count - 1
                    'If lstIntDisplaySeq(j) = intSeq Then
                    If lstSPPracticeDisplaySeq(j) = intSeq Then
                        blnFound = True

                        If Not IsNothing(arrByteTSMP) Then
                            Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, lstTsmpBankAccVerification(j)), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, arrByteTSMP)}
                            udtDB.RunProc("proc_checkTSMP", params)
                        Else
                            Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, lstTsmpBankAccVerification(j)), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, DBNull.Value)}
                            udtDB.RunProc("proc_checkTSMP", params)
                        End If

                        Exit For
                    End If
                Next
                If Not blnFound Then
                    Throw New ArgumentException("BankAccVerification TSMP Argument Invalid")
                End If
            Next

            If Not IsNothing(udtSPAccUpdateModel) Then
                Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPAccUpdateModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params2)
            Else
                Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params2)
            End If

            ' Reject Enrolment
            blnSuccess = Me.RejectSPProfileCore(Me.udtDB, strEnrolmentRefNo, strUserId, _
                            udtSPVerModel, dtBAVer, udtPVModelCollection, _
                            udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                            udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

            If blnSuccess Then
                Me.udtDB.CommitTransaction()
            Else
                Me.udtDB.RollBackTranscation()
            End If
            Return blnSuccess

        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw
            Return False
        End Try

    End Function

    ''' <summary>
    ''' [Public] Reject Mutil SP Profile From User D
    ''' </summary>
    ''' <param name="udtPVMCollection"></param>
    ''' <param name="dicSPAccountUpdateTsmp"></param>
    ''' <param name="strUserID"></param>
    ''' <param name="strErrorERN">Out: Error Enrolment Reference No</param>
    ''' <returns>Success=1, Fail=-1, Validation Fail=0</returns>
    ''' <remarks></remarks>
    Public Function RejectSPPofileFromUserDByBatch(ByVal udtPVMCollection As ProfessionalVerificationModelCollection, _
        ByVal dicSPAccountUpdateTsmp As Dictionary(Of String, Byte()), ByVal strUserID As String, ByRef strErrorERN As String) As Integer

        ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
        ' Since can reject a SP with multiple professional, so this checking is no longer valid
        'If udtPVMCollection.Values.Count <> dicSPAccountUpdateTsmp.Count Then
        '    Throw New ArgumentException("ProfessionalVerification Count Does not match with SPAccountUpdate Count")
        'End If
        ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

        Dim lstEnrolmentRefNo As New List(Of String)

        Try
            Me.udtDB.BeginTransaction()

            ' Group By Enrolment Ref No.
            For i As Integer = 0 To udtPVMCollection.Count - 1
                Dim udtPVModel As ProfessionalVerificationModel = CType(udtPVMCollection.GetByIndex(i), ProfessionalVerificationModel)
                If Not lstEnrolmentRefNo.Contains(udtPVModel.EnrolmentRefNo) Then

                    ' Next Matched Enrolment Ref No to be Reject
                    Dim strERN As String = udtPVModel.EnrolmentRefNo
                    lstEnrolmentRefNo.Add(strERN)

                    ' For Same Enrolment Ref No, Base on the First Entries of SPAccountUpdate TSMP
                    Dim tsmpSPAccUpd As Byte() = dicSPAccountUpdateTsmp(strERN)

                    ' Professional Verification Pair
                    Dim lstParamPVMTSMP As New List(Of Byte())
                    Dim lstParamPVProfSeq As New List(Of Integer)
                    lstParamPVMTSMP.Add(udtPVModel.TSMP)
                    lstParamPVProfSeq.Add(udtPVModel.ProfessionalSeq)

                    ' Look for Remaining PV in the list with Same Enrolment Ref No
                    For j As Integer = i + 1 To udtPVMCollection.Count - 1
                        Dim udtSearchPVModel As ProfessionalVerificationModel = CType(udtPVMCollection.GetByIndex(j), ProfessionalVerificationModel)

                        If udtPVModel.EnrolmentRefNo.Trim() = udtSearchPVModel.EnrolmentRefNo.Trim() Then
                            ' Professional Verification Pair
                            lstParamPVMTSMP.Add(udtSearchPVModel.TSMP)
                            lstParamPVProfSeq.Add(udtSearchPVModel.ProfessionalSeq)
                        End If
                    Next

                    ' Refer to Current Enrolment Ref No, Reject
                    strErrorERN = strERN
                    Dim intReturn As Integer = Me.RejectSPProfileFromUserD(Me.udtDB, strERN, strUserID, tsmpSPAccUpd, lstParamPVMTSMP, lstParamPVProfSeq)

                    If intReturn <> 1 Then
                        Me.udtDB.RollBackTranscation()
                        Return intReturn
                    End If
                End If
            Next

            Me.udtDB.CommitTransaction()

            Return 1
        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw
        End Try

    End Function

    ''' <summary>
    ''' [Private Only!] Reject Single SP Profile From User D
    ''' </summary>
    ''' <param name="udtDB"></param>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="tsmpSpAccUpdate">SPAccountUpdate TSMP</param>
    ''' <param name="lstTsmpProfVerification">Professional Verification TSMP List</param>
    ''' <param name="lstIntProfSeq">Identify Professional Verification Record</param>
    ''' <returns>Success=1, Fail=-1, Validation Fail=0</returns>
    ''' <remarks></remarks>
    Private Function RejectSPProfileFromUserD(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
        ByVal tsmpSpAccUpdate As Byte(), ByVal lstTsmpProfVerification As List(Of Byte()), ByVal lstIntProfSeq As List(Of Integer)) As Integer
        ' Concurrency Validation
        ' UserD: Validate on [SPAccountUpdate].TSMP + [ProfessionalVerification].TSMP

        Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
        Dim dtBAVer As DataTable = Nothing
        Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing

        Dim udtSPStagingModel As ServiceProviderModel = Nothing
        Dim udtPractStagingModelCollection As PracticeModelCollection = Nothing
        Dim udtBAStagingModelCollection As BankAcctModelCollection = Nothing
        Dim udtPStagingModelCollection As ProfessionalModelCollection = Nothing
        Dim udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection = Nothing

        Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

        Dim intReturn As Integer = 1
        Try
            'Me.udtDB.BeginTransaction()
            ' Retrieve Profile
            Me.RetrieveSPProfileForReject(udtDB, strEnrolmentRefNo, _
                udtSPVerModel, dtBAVer, udtPVModelCollection, _
                udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

            ' Validation:
            ' All ProfessionalVerification Record should be Imported before Reject

            For Each udtPVModel As ProfessionalVerificationModel In udtPVModelCollection.Values
                If Not udtPVModel.ExportBy Is Nothing AndAlso udtPVModel.ExportBy.Trim() <> "" AndAlso udtPVModel.ExportDtm.HasValue Then
                    If udtPVModel.ImportBy Is Nothing OrElse udtPVModel.ImportBy.Trim() = "" OrElse Not udtPVModel.ImportDtm.HasValue Then

                        ' Fail On Validation
                        intReturn = 0
                        Return intReturn
                    End If
                End If
            Next

            ' Concurrency Validation
            ' UserD: Validate on [SPAccountUpdate].TSMP + [ProfessionalVerification].TSMP
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

            If lstIntProfSeq.Count <> lstTsmpProfVerification.Count Then
                Throw New ArgumentException("ProfessionalVerification TSMP Argument Invalid")
            End If

            For Each udtPVModel As ProfessionalVerificationModel In udtPVModelCollection.Values
                Dim blnFound As Boolean = False

                For j As Integer = 0 To lstIntProfSeq.Count - 1
                    If udtPVModel.ProfessionalSeq = lstIntProfSeq(j) Then
                        blnFound = True

                        If Not IsNothing(udtPVModel) Then
                            Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, lstTsmpProfVerification(j)), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, udtPVModel.TSMP)}
                            udtDB.RunProc("proc_checkTSMP", params2)
                        Else
                            Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, lstTsmpProfVerification(j)), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, DBNull.Value)}
                            udtDB.RunProc("proc_checkTSMP", params2)
                        End If

                        Exit For
                    End If
                Next

                'If Not blnFound Then
                '    Throw New ArgumentException("ProfessionalVerification TSMP Argument Invalid")
                'End If
            Next

            If Not IsNothing(udtSPAccUpdateModel) Then
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPAccUpdateModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params)
            Else
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params)
            End If

            ' Reject Enrolment
            Dim blnSuccess As Boolean = Me.RejectSPProfileCore(udtDB, strEnrolmentRefNo, strUserId, _
                            udtSPVerModel, dtBAVer, udtPVModelCollection, _
                            udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                            udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

            If blnSuccess Then
                'Me.udtDB.CommitTransaction()
            Else
                'Me.udtDB.RollBackTranscation()
                intReturn = -1
            End If
            Return intReturn

        Catch ex As Exception
            'Me.udtDB.RollBackTranscation()
            Throw
            Return -1
        End Try

    End Function

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ''' <summary>
    '''  [Public] Reject Single SP Profile From User E
    ''' </summary>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="tsmpSpAccUpdate">SPAccountUpdate TSMP</param>
    ''' <returns></returns>
    ''' <remarks>User E Reject, Check SPAccount, Any Update on User E result in SP Account Updated</remarks>
    Public Function RejectSPProfileFromUserE(ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
        ByVal tsmpSpAccUpdate As Byte()) As Boolean
        ' Concurrency Validation
        ' UserE: Validate on [SPAccountUpdate].TSMP

        Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
        Dim dtBAVer As DataTable = Nothing
        Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing

        Dim udtSPStagingModel As ServiceProviderModel = Nothing
        Dim udtPractStagingModelCollection As PracticeModelCollection = Nothing
        Dim udtBAStagingModelCollection As BankAcctModelCollection = Nothing
        Dim udtPStagingModelCollection As ProfessionalModelCollection = Nothing
        Dim udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection = Nothing

        '    udtSP.ApplicationPrinted = String.Empty
        Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

        Dim blnSuccess As Boolean = True
        Try
            Me.udtDB.BeginTransaction()

            ' Retrieve Profile
            Me.RetrieveSPProfileForReject(Me.udtDB, strEnrolmentRefNo, _
                udtSPVerModel, dtBAVer, udtPVModelCollection, _
                udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

            ' Validation
            ' UserE: Validate on [SPAccountUpdate].TSMP
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

            If Not IsNothing(udtSPAccUpdateModel) Then
                Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPAccUpdateModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params2)
            Else
                Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params2)
            End If

            ' Reject Enrolment
            blnSuccess = Me.RejectSPProfileCore(Me.udtDB, strEnrolmentRefNo, strUserId, _
                            udtSPVerModel, dtBAVer, udtPVModelCollection, _
                            udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                            udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

            If blnSuccess Then
                Me.udtDB.CommitTransaction()
            Else
                Me.udtDB.RollBackTranscation()
            End If
            Return blnSuccess

        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw
            Return False
        End Try
    End Function
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    ''' <summary>
    ''' [Private Only!] Retrieve SP Profile For Reject Purpose
    ''' </summary>
    ''' <param name="udtDB"></param>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="udtSPVerModel">Out: ServiceProviderVerification</param>
    ''' <param name="dtBAVer">Out: BankAccVerification</param>
    ''' <param name="udtPVModelCollection">Out: ProfessionalVerification</param>
    ''' <param name="udtSPStagingModel">Out: ServiceProviderStaging</param>
    ''' <param name="udtPractStagingModelCollection">Out: PracticeStaging</param>
    ''' <param name="udtBAStagingModelCollection">Out: BankAccountStaging</param>
    ''' <param name="udtPStagingModelCollection">Out: ProfessionalStaging</param>
    ''' <param name="udtSPAccUpdateModel">Out: SPAccountUpdate</param>
    ''' <remarks></remarks>
    Private Sub RetrieveSPProfileForReject(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, _
        ByRef udtSPVerModel As ServiceProviderVerificationModel, ByRef dtBAVer As DataTable, ByRef udtPVModelCollection As ProfessionalVerificationModelCollection, _
        ByRef udtSPStagingModel As ServiceProviderModel, ByRef udtPractStagingModelCollection As PracticeModelCollection, _
        ByRef udtBAStagingModelCollection As BankAcctModelCollection, ByRef udtPStagingModelCollection As ProfessionalModelCollection, _
        ByRef udtSPAccUpdateModel As SPAccountUpdateModel, _
        ByRef udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection)

        ' Retrieve *.Verification Records
        Dim udtSPVerBLL As New ServiceProviderVerificationBLL()
        Dim udtBAVerBLL As New BankAccVerificationBLL()
        Dim udtPVerBLL As New ProfessionalVerificationBLL()

        udtSPVerModel = udtSPVerBLL.GetSerivceProviderVerificationByERN(strEnrolmentRefNo, udtDB)
        dtBAVer = udtBAVerBLL.GetBankAccVerificationListByERN(strEnrolmentRefNo, udtDB)
        udtPVModelCollection = udtPVerBLL.GetProfessionalVerificationListByERN(strEnrolmentRefNo, udtDB)

        ' Retrieve *.Staging Records
        Dim udtSPBLL As New ServiceProviderBLL()
        Dim udtPracticeBLL As New PracticeBLL()
        Dim udtBABLL As New BankAcctBLL()
        Dim udtPBLL As New ProfessionalBLL()
        Dim udtSchemeInfoBLL As New SchemeInformationBLL()

        udtSPStagingModel = udtSPBLL.GetServiceProviderStagingByERN(strEnrolmentRefNo, udtDB)
        udtPractStagingModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(strEnrolmentRefNo, udtDB)
        udtBAStagingModelCollection = udtBABLL.GetBankAcctListFromStagingByERN(strEnrolmentRefNo, udtDB)
        udtPStagingModelCollection = udtPBLL.GetProfessinalListFromStagingByERN(strEnrolmentRefNo, udtDB)

        ' Scheme Info For Different Enrolment Scheme 
        udtSchemeInfoStagingModelCollection = udtSchemeInfoBLL.GetSchemeInfoListStaging(strEnrolmentRefNo, udtDB)

        ' Retrieve SPAccountUpdate
        Dim udtSPAccUpdateBLL As New SPAccountUpdateBLL()
        udtSPAccUpdateModel = udtSPAccUpdateBLL.GetSPAccountUpdateByERN(strEnrolmentRefNo, udtDB)

    End Sub

    ''' <summary>
    ''' [Private Only] Reject SP Profile Logic, Call by RejectSPProfile*
    ''' </summary>
    ''' <param name="udtDB"></param>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="udtSPVerModel"></param>
    ''' <param name="dtBAVer"></param>
    ''' <param name="udtPVModelCollection"></param>
    ''' <param name="udtSPStagingModel"></param>
    ''' <param name="udtPractStagingModelCollection"></param>
    ''' <param name="udtBAStagingModelCollection"></param>
    ''' <param name="udtPStagingModelCollection"></param>
    ''' <param name="udtSPAccUpdateModel"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RejectSPProfileCore(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
        ByRef udtSPVerModel As ServiceProviderVerificationModel, ByRef dtBAVer As DataTable, ByRef udtPVModelCollection As ProfessionalVerificationModelCollection, _
        ByRef udtSPStagingModel As ServiceProviderModel, ByRef udtPractStagingModelCollection As PracticeModelCollection, _
        ByRef udtBAStagingModelCollection As BankAcctModelCollection, ByRef udtPStagingModelCollection As ProfessionalModelCollection, _
        ByRef udtSPAccUpdateModel As SPAccountUpdateModel, ByRef udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection) As Boolean

        ' Two Scenario: New Enrolment (No SPID) / Existing SP (With SPID)
        ' 1. By Service Provider (Enrolment_Ref_No), Update [ServiceProviderVerification], [BankAccVerification], [ProfessionalVerification] Reject By & Reject Date
        '1.1 By Service Provider (Enrolment_Ref_No), Update [ServiceProviderStaging], [PracticeStaging], [BankAccountStaging], [ProfessionalStaging] Record_Status = 'R'
        ' ----- [MedicalOrganizationStaging], [PracticeSchemeInfoStaging], [SchemeInformationStaging] Record_Status = 'R'
        '1.2 Update [SPAccountUpdate].ProgressStatus To Reject
        ' 2. By Service Provider (Enrolment_Ref_No), Delete Staging + Verification Tables Record + SPAccountUpdate
        ' ----- [ServiceProviderVerification], [BankAccVerification], [ProfessionalVerification]
        ' ----- [ServiceProviderStaging], [PracticeStaging], [BankAccountStaging], [ProfessionalStaging], [SchemeInformationStaging],
        ' ----- [MedicalOrganizationStaging], [PracticeSchemeInfoStaging], [ERNProcessedStaging]
        ' ----- [SPAccountUpdate]
        ' 3. For Existing SP Only: Set UnderModification to Null for
        ' ----- [ServiceProvider], [Practice]
        Dim blnSuccess As Boolean = True
        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()
        Dim strSPID As String = ""

        ' Existing SP Or New Enrolment
        If Not udtSPStagingModel.SPID Is Nothing AndAlso udtSPStagingModel.SPID.Trim() <> "" Then
            strSPID = udtSPStagingModel.SPID.Trim()
        End If

        Try
            'udtDB.BeginTransaction()
            ' -------------------------------------------------------------------------------------
            '' Init BLL
            Dim udtSPVerBLL As New ServiceProviderVerificationBLL()
            Dim udtBAVerBLL As New BankAccVerificationBLL()
            Dim udtPVerBLL As New ProfessionalVerificationBLL()

            Dim udtSPBLL As New ServiceProviderBLL()
            Dim udtPracticeBLL As New PracticeBLL()
            Dim udtBABLL As New BankAcctBLL()
            Dim udtPBLL As New ProfessionalBLL()

            Dim udtSchemeInfoBLL As New SchemeInformationBLL()
            Dim udtMOBLL As New MedicalOrganizationBLL()
            Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfoBLL()
            Dim udtERNProcessedBLL As New ERNProcessedBLL

            Dim udtSPAccUpdBLL As New SPAccountUpdateBLL()

            ' -------------------------------------------------------------------------------------
            ' Update *.Verification Void By & Reject Date & Record Status = 'R'
            If blnSuccess Then
                ' [ServiceProviderVerification]
                udtSPVerModel.VoidBy = strUserId
                udtSPVerModel.RecordStatus = ServiceProviderVerificationStatus.Reject
                udtSPVerModel.VoidDtm = dtmCurrent
                blnSuccess = udtSPVerBLL.UpdateServiceProviderVerificationReject(udtSPVerModel, udtDB)
            End If

            If blnSuccess Then
                ' [BankAccVerification]         
                For Each drBAVer As DataRow In dtBAVer.Rows
                    If blnSuccess Then
                        'display_Seq, tsmp
                        Dim intSeq As Integer = Convert.ToInt32(drBAVer("display_Seq"))
                        Dim intSPPracticeDisplaySeq As Integer = Convert.ToInt32(drBAVer("sp_practice_display_Seq"))
                        Dim arrByteTSMP As Byte() = CType(drBAVer("TSMP"), Byte())
                        blnSuccess = udtBAVerBLL.RejectBankAccount(udtDB, strEnrolmentRefNo, intSeq, intSPPracticeDisplaySeq, strUserId, dtmCurrent, arrByteTSMP)
                        'RejectBankAccount()
                    Else
                        Exit For
                    End If
                Next
            End If

            If blnSuccess Then
                ' [ProfessionalVerification]
                For Each udtPVerModel As ProfessionalVerificationModel In udtPVModelCollection.Values
                    udtPVerBLL.UpdateProfessionalVerificationReject(udtDB, strEnrolmentRefNo, udtPVerModel.ProfessionalSeq, strUserId, dtmCurrent, udtPVerModel.TSMP)
                Next
            End If

            ' -------------------------------------------------------------------------------------
            ' Update *.Staging Record_Status = 'R'

            If blnSuccess Then
                ' [ServiceProviderStaging]
                udtSPStagingModel.RecordStatus = Common.Component.ServiceProviderStagingStatus.Reject
                udtSPStagingModel.UpdateBy = strUserId.Trim()
                ' To Do: Write A Store Proc To Update [ServiceProviderStaging] Record Status Only
                blnSuccess = udtSPBLL.UpdateServiceProviderStagingParticulars(udtSPStagingModel, udtDB)
            End If

            If blnSuccess Then
                ' [PracticeStaging]
                For Each udtPracticeModel As PracticeModel In udtPractStagingModelCollection.Values
                    If blnSuccess Then
                        udtPracticeModel.RecordStatus = Common.Component.PracticeStagingStatus.Reject
                        udtPracticeModel.UpdateBy = strUserId.Trim()
                        blnSuccess = udtPracticeBLL.UpdatePracticeStaging(udtPracticeModel, udtDB)

                        If blnSuccess Then
                            ' [PracticeSchemeInfoStaging]
                            If Not IsNothing(udtPracticeModel.PracticeSchemeInfoList) Then
                                For Each udtPracticeSchemeInfoStagingModel As PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
                                    If blnSuccess Then
                                        blnSuccess = udtPracticeSchemeInfoBLL.UpdateStagingRecordStatus(udtPracticeSchemeInfoStagingModel, Common.Component.PracticeSchemeInfoStagingStatus.Reject, udtDB)
                                    End If
                                Next
                            End If

                        End If
                    End If
                Next
            End If

            If blnSuccess Then
                ' [BankAccountStaging]
                For Each udtBAModel As BankAcctModel In udtBAStagingModelCollection.Values
                    If blnSuccess Then
                        ' To Do: Write A Store Proc To Update [BankAccountStaging] Record Status Only
                        udtBAModel.RecordStatus = Common.Component.BankAcctStagingStatus.Reject
                        udtBAModel.UpdateBy = strUserId.Trim()
                        blnSuccess = udtBABLL.UpdateBankAcctStaging(udtBAModel, udtDB)
                    End If
                Next
            End If

            If blnSuccess Then
                ' [ProfessionalStaging]
                For Each udtPStagingModel As ProfessionalModel In udtPStagingModelCollection.Values
                    If blnSuccess Then
                        blnSuccess = udtPBLL.UpdateProfessionalStagingStatus(strEnrolmentRefNo, udtPStagingModel.ProfessionalSeq, Common.Component.ProfessionalStagingStatus.Reject, udtDB)
                    End If
                Next
            End If

            If blnSuccess Then
                ' [MedicalOrganizationStaging]
                For Each udtMOStagingModel As MedicalOrganizationModel In udtSPStagingModel.MOList.Values
                    If blnSuccess Then
                        blnSuccess = udtMOBLL.UpdateMOStagingStatus(strEnrolmentRefNo, udtMOStagingModel.DisplaySeq, Common.Component.MedicalOrganizationStagingStatus.Reject, strUserId.Trim(), udtMOStagingModel.TSMP, udtDB)
                    End If
                Next
            End If

            If blnSuccess Then
                ' [SchemeInformationStaging]
                For Each udtSchemeInfoStagingModel As SchemeInformationModel In udtSchemeInfoStagingModelCollection.Values
                    If blnSuccess Then
                        blnSuccess = udtSchemeInfoBLL.UpdateSchemeInfoStagingStatus(udtDB, strEnrolmentRefNo, udtSchemeInfoStagingModel.SchemeCode, Common.Component.SchemeInformationStagingStatus.Reject, strUserId.Trim(), udtSchemeInfoStagingModel.Remark, udtSchemeInfoStagingModel.TSMP)
                    End If
                Next
            End If

            ' -------------------------------------------------------------------------------------
            ' Update SPAccountUpdate Status To Reject

            If blnSuccess Then
                If Not IsNothing(udtSPAccUpdateModel) Then
                    udtSPAccUpdateModel.ProgressStatus = Common.Component.SPAccountUpdateProgressStatus.Reject
                    udtSPAccUpdateModel.UpdateBy = strUserId
                    blnSuccess = udtSPAccUpdBLL.UpdateSPAccountUpdateProgressStatus(udtSPAccUpdateModel, udtDB)
                End If
            End If

            ' -------------------------------------------------------------------------------------
            ' Delete *.Verification

            If blnSuccess Then
                ' [ProfessionalVerification]
                For Each udtPVerModel As ProfessionalVerificationModel In udtPVModelCollection.Values
                    udtPVerBLL.DeleteProfessionalVerification(udtDB, strEnrolmentRefNo, udtPVerModel.ProfessionalSeq, udtPVerModel.TSMP, False)
                Next
            End If

            If blnSuccess Then
                ' [BankAccVerification]
                For Each drBAVer As DataRow In dtBAVer.Rows
                    If blnSuccess Then
                        'display_Seq, tsmp
                        Dim intSeq As Integer = Convert.ToInt32(drBAVer("display_Seq"))
                        Dim intSPPracticeDisplaySeq As Integer = Convert.ToInt32(drBAVer("sp_practice_display_Seq"))
                        Dim arrByteTSMP As Byte() = CType(drBAVer("TSMP"), Byte())
                        udtBAVerBLL.DeleteBankAccVerification(udtDB, strEnrolmentRefNo, intSeq, intSPPracticeDisplaySeq, arrByteTSMP, False)
                    Else
                        Exit For
                    End If
                Next
            End If

            If blnSuccess Then
                ' [ServiceProviderVerification]
                udtSPVerBLL.DeleteServiceProviderVerification(udtDB, strEnrolmentRefNo, udtSPVerModel.TSMP, False)
            End If

            ' -------------------------------------------------------------------------------------
            ' Delete *.Staging
            If blnSuccess Then
                ' [ProfessionalStaging]
                For Each udtPStagingModel As ProfessionalModel In udtPStagingModelCollection.Values
                    udtPBLL.DeleteProfessionalStagingByKey(udtDB, strEnrolmentRefNo, udtPStagingModel.ProfessionalSeq)
                Next
            End If

            If blnSuccess Then
                ' [BankAccountStaging]
                For Each udtBAModel As BankAcctModel In udtBAStagingModelCollection.Values
                    udtBABLL.DeleteBankAccountStagingByKey(udtDB, strEnrolmentRefNo, udtBAModel.DisplaySeq, udtBAModel.SpPracticeDisplaySeq, udtBAModel.TSMP, False)
                Next
            End If

            If blnSuccess Then
                ' [PracticeStaging]
                For Each udtPracticeModel As PracticeModel In udtPractStagingModelCollection.Values
                    udtPracticeBLL.DeletePracticeStagingByKey(udtDB, strEnrolmentRefNo, udtPracticeModel.DisplaySeq, udtPracticeModel.TSMP, False)

                    ' [PracticeSchemeInfoStaging]
                    If Not IsNothing(udtPracticeModel.PracticeSchemeInfoList) Then
                        For Each udtPracticeSchemeInfoStagingModel As PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
                            udtPracticeSchemeInfoBLL.DeletePracticeSchemeInfoStagingByKey(udtDB, udtPracticeSchemeInfoStagingModel, Nothing, False)
                        Next
                    End If
                Next
            End If

            If blnSuccess Then
                ' [ServiceProviderStaging]
                udtSPBLL.DeleteServiceProviderStagingByKey(udtDB, strEnrolmentRefNo, udtSPStagingModel.TSMP, False)
            End If

            If blnSuccess Then
                ' [SchemeInformationStaging]
                For Each udtSchemeInfoStagingModel As SchemeInformationModel In udtSchemeInfoStagingModelCollection.Values
                    udtSchemeInfoBLL.DeleteSchemeInfoStaging(udtDB, udtSchemeInfoStagingModel.EnrolRefNo, udtSchemeInfoStagingModel.SchemeCode)
                Next
            End If

            If blnSuccess Then
                ' [MedicalOrganizationStaging]
                For Each udtMOStagingModel As MedicalOrganizationModel In udtSPStagingModel.MOList.Values
                    udtMOBLL.DeleteMOStagingByKey(udtDB, strEnrolmentRefNo, udtMOStagingModel.DisplaySeq, udtMOStagingModel.TSMP, False)
                Next
            End If

            If blnSuccess Then
                ' [ERNProcessedStaging]
                Dim udtERNProcessedList As ERNProcessedModelCollection
                udtERNProcessedList = udtERNProcessedBLL.GetERNProcessedListStagingByERN(udtSPStagingModel.EnrolRefNo, udtDB)
                If Not IsNothing(udtERNProcessedList) Then
                    For Each udtERNProcessedModel As ERNProcessedModel In udtERNProcessedList.Values
                        udtERNProcessedBLL.DeleteERNProcessedStaging(udtERNProcessedModel, udtDB)
                    Next
                End If
            End If

            ' -------------------------------------------------------------------------------------
            ' Delete SPAccountUpdate
            If blnSuccess Then
                ' Delete [SPAccountUpdate]
                If Not IsNothing(udtSPAccUpdateModel) Then
                    udtSPAccUpdBLL.DeleteSPAccountUpdate(strEnrolmentRefNo, udtSPAccUpdateModel.TSMP, udtDB, False)
                End If
            End If
            ' -------------------------------------------------------------------------------------

            ' 3. For Existing SP Only: Set UnderModification to Null for
            ' ----- [ServiceProvider], [Practice]

            If strSPID.Trim() <> "" Then
                Dim udtSPModel As ServiceProviderModel = udtSPBLL.GetServiceProviderBySPID(udtDB, strSPID.Trim())
                udtSPModel.UnderModification = Nothing
                udtSPModel.UpdateBy = strUserId
                udtSPBLL.UpdateServiceProviderUnderModificationStatus(udtSPModel, udtDB)

                ' Remove, as UnderModificationStatus should refer to ServiceProvider.UnderModificationStatus
                'Dim udtPracticeModelCollection As PracticeModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromPermanentBySPID_NoReader(strSPID, udtDB)
                'For Each udtPracticeModel As PracticeModel In udtPracticeModelCollection.Values
                '    If blnSuccess Then
                '        udtPracticeModel.UnderModification = Nothing
                '        udtPracticeModel.UpdateBy = strUserId
                '        blnSuccess = udtPracticeBLL.UpdatePracticeUnderModificationStatus(udtPracticeModel, udtDB)
                '    End If
                'Next

            End If

            ' CRE12-001 eHS and PCD integration [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Delete *.Original
            If blnSuccess Then
                blnSuccess = udtSPBLL.DeleteServiceProviderOriginalProfile(strEnrolmentRefNo, udtDB)
            End If
            ' CRE12-001 eHS and PCD integration [End][Koala]

            Return blnSuccess

        Catch ex As Exception
            'udtDB.RollBackTranscation()
            Throw
        End Try

        Return True

    End Function

#End Region

#Region "Return For Amendment"

    ''' <summary>
    ''' SP Profile Return For Amendment From User B
    ''' </summary>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="tsmpSpAccUpdate">SPAccountUpdate TSMP</param>
    ''' <param name="tsmpSPVerification">SP Verification TSMP</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReturnForAmendmentFromUserB(ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
        ByVal tsmpSpAccUpdate As Byte(), ByVal tsmpSPVerification As Byte())

        Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
        Dim dtBAVer As DataTable = Nothing
        Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing
        Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

        Dim blnSuccess As Boolean = True
        Try
            Me.udtDB.BeginTransaction()

            Me.RetrieveReturnForAmendmentSPProfile(Me.udtDB, strEnrolmentRefNo, udtSPVerModel, dtBAVer, udtPVModelCollection, udtSPAccUpdateModel)

            ' Validation
            ' UserB: Validate on [SPAccountUpdate].TSMP + [ServiceProviderVerification].TSMP
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

            If Not IsNothing(udtSPVerModel) Then
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPVerModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSPVerification)}
                udtDB.RunProc("proc_checkTSMP", params)
            Else
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSPVerification)}
                udtDB.RunProc("proc_checkTSMP", params)
            End If

            If Not IsNothing(udtSPAccUpdateModel) Then
                Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPAccUpdateModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params2)
            Else
                Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params2)
            End If

            ' Return For Amendment
            blnSuccess = Me.ReturnForAmendmentCore(Me.udtDB, strEnrolmentRefNo, strUserId, udtSPVerModel, dtBAVer, udtPVModelCollection, udtSPAccUpdateModel)

            If blnSuccess Then
                Me.udtDB.CommitTransaction()
            Else
                Me.udtDB.RollBackTranscation()
            End If

            Return blnSuccess

        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw
            Return False
        End Try
    End Function

    ''' <summary>
    ''' SP Profile Return For Amendment From User C
    ''' </summary>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="tsmpSpAccUpdate">SPAccountUpdate TSMP</param>
    ''' <param name="lstTsmpBankAccVerification">Bank Account Verification TSMP List</param>
    ''' <param name="lstIntDisplaySeq">Seq List to Indentify the Bank Account Verification Record</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReturnForAmendmentFromUserC(ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
        ByVal tsmpSpAccUpdate As Byte(), ByVal lstTsmpBankAccVerification As List(Of Byte()), ByVal lstIntDisplaySeq As List(Of Integer), ByVal lstSPPracticeDisplaySeq As List(Of Integer))

        Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
        Dim dtBAVer As DataTable = Nothing
        Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing
        Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

        Dim blnSuccess As Boolean = True
        Try
            Me.udtDB.BeginTransaction()

            Me.RetrieveReturnForAmendmentSPProfile(Me.udtDB, strEnrolmentRefNo, udtSPVerModel, dtBAVer, udtPVModelCollection, udtSPAccUpdateModel)

            ' Validation
            ' UserC: Validate on [SPAccountUpdate].TSMP + [BankAccVerification].TSMP (s)
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

            If lstIntDisplaySeq.Count <> lstTsmpBankAccVerification.Count Then
                Throw New ArgumentException("BankAccVerification TSMP Argument Invalid")
            End If

            If dtBAVer.Rows.Count > 0 Then
                For Each drBankAccVer As DataRow In dtBAVer.Rows
                    Dim intSeq As Integer = Convert.ToInt32(drBankAccVer("display_Seq"))
                    Dim intSPPracticeDisplaySeq As Integer = Convert.ToInt32(drBankAccVer("sp_practice_display_Seq"))
                    Dim arrByteTSMP As Byte() = CType(drBankAccVer("TSMP"), Byte())

                    Dim blnFound As Boolean = False
                    For j As Integer = 0 To lstIntDisplaySeq.Count - 1
                        If lstIntDisplaySeq(j) = intSeq And lstSPPracticeDisplaySeq(j) = intSPPracticeDisplaySeq Then
                            blnFound = True

                            If Not IsNothing(arrByteTSMP) Then
                                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, lstTsmpBankAccVerification(j)), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, arrByteTSMP)}
                                udtDB.RunProc("proc_checkTSMP", params)
                            Else
                                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, lstTsmpBankAccVerification(j)), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, DBNull.Value)}
                                udtDB.RunProc("proc_checkTSMP", params)
                            End If

                            Exit For
                        End If
                    Next
                    If Not blnFound Then
                        Throw New ArgumentException("BankAccVerification TSMP Argument Invalid")
                    End If
                Next

                ' Return For Amendment
                blnSuccess = Me.ReturnForAmendmentCore(Me.udtDB, strEnrolmentRefNo, strUserId, udtSPVerModel, dtBAVer, udtPVModelCollection, udtSPAccUpdateModel)

                If blnSuccess Then
                    Me.udtDB.CommitTransaction()
                Else
                    Me.udtDB.RollBackTranscation()
                End If

                Return blnSuccess
            Else
                'Throw a concurrent update exception from db
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, DBNull.Value)}
                udtDB.RunProc("proc_checkTSMP", params)
            End If
        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw
            Return False
        End Try
    End Function

    Public Function ReturnForAmendmentFromUserDByBatch(ByVal udtPVMCollection As ProfessionalVerificationModelCollection, _
        ByVal dicSPAccountUpdateTsmp As Dictionary(Of String, Byte()), ByVal strUserID As String, ByRef strErrorERN As String) As Integer

        If udtPVMCollection.Values.Count <> dicSPAccountUpdateTsmp.Count Then
            Throw New ArgumentException("ProfessionalVerification Count Does not match with SPAccountUpdate Count")
        End If

        Dim lstEnrolmentRefNo As New List(Of String)

        Try
            Me.udtDB.BeginTransaction()

            ' Group By Enrolment Ref No.
            For i As Integer = 0 To udtPVMCollection.Count - 1
                Dim udtPVModel As ProfessionalVerificationModel = CType(udtPVMCollection.GetByIndex(i), ProfessionalVerificationModel)
                If Not lstEnrolmentRefNo.Contains(udtPVModel.EnrolmentRefNo) Then

                    ' Next Matched Enrolment Ref No to be Reject
                    Dim strERN As String = udtPVModel.EnrolmentRefNo
                    lstEnrolmentRefNo.Add(strERN)

                    ' For Same Enrolment Ref No, Base on the First Entries of SPAccountUpdate TSMP
                    Dim tsmpSPAccUpd As Byte() = dicSPAccountUpdateTsmp(strERN)

                    ' Professional Verification Pair
                    Dim lstParamPVMTSMP As New List(Of Byte())
                    Dim lstParamPVProfSeq As New List(Of Integer)
                    lstParamPVMTSMP.Add(udtPVModel.TSMP)
                    lstParamPVProfSeq.Add(udtPVModel.ProfessionalSeq)

                    ' Look for Remaining PV in the list with Same Enrolment Ref No
                    For j As Integer = i + 1 To udtPVMCollection.Count - 1
                        Dim udtSearchPVModel As ProfessionalVerificationModel = CType(udtPVMCollection.GetByIndex(j), ProfessionalVerificationModel)

                        If udtPVModel.EnrolmentRefNo.Trim() = udtSearchPVModel.EnrolmentRefNo.Trim() Then
                            ' Professional Verification Pair
                            lstParamPVMTSMP.Add(udtSearchPVModel.TSMP)
                            lstParamPVProfSeq.Add(udtSearchPVModel.ProfessionalSeq)
                        End If
                    Next

                    ' Refer to Current Enrolment Ref No, Reject
                    strErrorERN = strERN
                    Dim intReturn As Integer = Me.ReturnForAmendmentFromUserD(Me.udtDB, strERN, strUserID, tsmpSPAccUpd, lstParamPVMTSMP, lstParamPVProfSeq)

                    If intReturn <> 1 Then
                        Me.udtDB.RollBackTranscation()
                        Return intReturn
                    End If
                End If
            Next

            Me.udtDB.CommitTransaction()

            Return 1
        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw
        End Try

    End Function

    ''' <summary>
    ''' [Private Only] SP Profile Return For Amendment From User D
    ''' </summary>
    ''' <param name="udtDB"></param>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="tsmpSpAccUpdate">SPAccountUpdate TSMP</param>
    ''' <param name="lstTsmpProfVerification">Professional Verification TSMP List</param>
    ''' <param name="lstIntProfSeq">Identify Professional Verification Record</param>
    ''' <returns>Success=1, Fail=-1, Validation Fail=0</returns>
    ''' <remarks></remarks>
    Private Function ReturnForAmendmentFromUserD(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
        ByVal tsmpSpAccUpdate As Byte(), ByVal lstTsmpProfVerification As List(Of Byte()), ByVal lstIntProfSeq As List(Of Integer)) As Integer

        Dim intReturn As Integer = 1

        Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
        Dim dtBAVer As DataTable = Nothing
        Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing
        Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

        Try
            Me.RetrieveReturnForAmendmentSPProfile(udtDB, strEnrolmentRefNo, udtSPVerModel, dtBAVer, udtPVModelCollection, udtSPAccUpdateModel)

            ' Validation:
            ' All ProfessionalVerification Record should be Imported before Return For Amendment
            For Each udtPVModel As ProfessionalVerificationModel In udtPVModelCollection.Values
                If Not udtPVModel.ExportBy Is Nothing AndAlso udtPVModel.ExportBy.Trim() <> "" AndAlso udtPVModel.ExportDtm.HasValue Then
                    If udtPVModel.ImportBy Is Nothing OrElse udtPVModel.ImportBy.Trim() = "" OrElse Not udtPVModel.ImportDtm.HasValue Then

                        ' Fail On Validation
                        intReturn = 0
                        Return intReturn
                    End If
                End If
            Next

            ' Concurrency Validation
            ' UserD: Validate on [SPAccountUpdate].TSMP + [ProfessionalVerification].TSMP
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

            If lstIntProfSeq.Count <> lstTsmpProfVerification.Count Then
                Throw New ArgumentException("ProfessionalVerification TSMP Argument Invalid")
            End If

            For Each udtPVModel As ProfessionalVerificationModel In udtPVModelCollection.Values
                Dim blnFound As Boolean = False

                For j As Integer = 0 To lstIntProfSeq.Count - 1
                    If udtPVModel.ProfessionalSeq = lstIntProfSeq(j) Then
                        blnFound = True

                        If Not IsNothing(udtPVModel) Then
                            Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, lstTsmpProfVerification(j)), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, udtPVModel.TSMP)}
                            udtDB.RunProc("proc_checkTSMP", params2)
                        Else
                            Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, lstTsmpProfVerification(j)), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, DBNull.Value)}
                            udtDB.RunProc("proc_checkTSMP", params2)
                        End If

                        Exit For
                    End If
                Next

                'If Not blnFound Then
                '    Throw New ArgumentException("ProfessionalVerification TSMP Argument Invalid")
                'End If
            Next

            If Not IsNothing(udtSPAccUpdateModel) Then
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPAccUpdateModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params)
            Else
                Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params)
            End If

            ' Return For Amendment
            Dim blnSuccess As Boolean = Me.ReturnForAmendmentCore(Me.udtDB, strEnrolmentRefNo, strUserId, udtSPVerModel, dtBAVer, udtPVModelCollection, udtSPAccUpdateModel)

            If blnSuccess Then
            Else
                intReturn = -1
            End If

            Return intReturn
        Catch ex As Exception
            Throw
        End Try

    End Function

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ''' <summary>
    ''' SP Profile Return For Amendment From User E (Token Scheme Magement)
    ''' </summary>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="tsmpSpAccUpdate">SPAccountUpdate TSMP</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReturnForAmendmentFromUserE(ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
        ByVal tsmpSpAccUpdate As Byte())

        Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
        Dim dtBAVer As DataTable = Nothing
        Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing
        Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

        Dim blnSuccess As Boolean = True
        Try
            Me.udtDB.BeginTransaction()

            Me.RetrieveReturnForAmendmentSPProfile(Me.udtDB, strEnrolmentRefNo, udtSPVerModel, dtBAVer, udtPVModelCollection, udtSPAccUpdateModel)

            ' Validation
            ' UserE: Validate on [SPAccountUpdate].TSMP
            Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

            If Not IsNothing(udtSPAccUpdateModel) Then
                Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPAccUpdateModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params2)
            Else
                Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
                udtDB.RunProc("proc_checkTSMP", params2)
            End If

            ' Return For Amendment
            blnSuccess = Me.ReturnForAmendmentCore(Me.udtDB, strEnrolmentRefNo, strUserId, udtSPVerModel, dtBAVer, udtPVModelCollection, udtSPAccUpdateModel)

            If blnSuccess Then
                Me.udtDB.CommitTransaction()
            Else
                Me.udtDB.RollBackTranscation()
            End If

            Return blnSuccess

        Catch ex As Exception
            Me.udtDB.RollBackTranscation()
            Throw
            Return False
        End Try
    End Function
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]


    ''' <summary>
    ''' [Private Only!] Retrieve SP Profile For Return For Amendment Purpose
    ''' </summary>
    ''' <param name="udtDB"></param>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="udtSPVerModel">Out: ServiceProviderVerification</param>
    ''' <param name="dtBAVer">Out: BankAccVerificatio</param>
    ''' <param name="udtPVModelCollection">Out: ProfessionalVerification</param>
    ''' <param name="udtSPAccUpdateModel">Out: SPAccountUpdate</param>
    ''' <remarks></remarks>
    Private Sub RetrieveReturnForAmendmentSPProfile(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, _
        ByRef udtSPVerModel As ServiceProviderVerificationModel, ByRef dtBAVer As DataTable, ByRef udtPVModelCollection As ProfessionalVerificationModelCollection, _
        ByRef udtSPAccUpdateModel As SPAccountUpdateModel)

        ' Retrieve *.Verification Records
        Dim udtSPVerBLL As New ServiceProviderVerificationBLL()
        Dim udtBAVerBLL As New BankAccVerificationBLL()
        Dim udtPVerBLL As New ProfessionalVerificationBLL()

        udtSPVerModel = udtSPVerBLL.GetSerivceProviderVerificationByERN(strEnrolmentRefNo, udtDB)
        dtBAVer = udtBAVerBLL.GetBankAccVerificationListByERN(strEnrolmentRefNo, udtDB)
        udtPVModelCollection = udtPVerBLL.GetProfessionalVerificationListByERN(strEnrolmentRefNo, udtDB)

        ' Retrieve SPAccountUpdate
        Dim udtSPAccUpdateBLL As New SPAccountUpdateBLL()
        udtSPAccUpdateModel = udtSPAccUpdateBLL.GetSPAccountUpdateByERN(strEnrolmentRefNo, udtDB)
    End Sub

    ''' <summary>
    ''' [Private Only!] Return For Amendment on SP Profile Logic, Call By ReturnForAmendmentSPProfile*
    ''' </summary>
    ''' <param name="udtDB"></param>
    ''' <param name="strEnrolmentRefNo"></param>
    ''' <param name="strUserId"></param>
    ''' <param name="udtSPVerModel">Out: ServiceProviderVerification</param>
    ''' <param name="dtBAVer">Out: BankAccVerification</param>
    ''' <param name="udtPVModelCollection">Out: ProfessionalVerification</param>
    ''' <param name="udtSPAccUpdateModel">Out: SPAccountUpdate</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ReturnForAmendmentCore(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
        ByRef udtSPVerModel As ServiceProviderVerificationModel, ByRef dtBAVer As DataTable, ByRef udtPVModelCollection As ProfessionalVerificationModelCollection, _
        ByRef udtSPAccUpdateModel As SPAccountUpdateModel) As Boolean

        ' 1. Update [ServiceProviderVerification]: Return for Amendment Status + Clear Other Values
        ' 2. Delete [BankAccVerification]
        ' 3. Update/Delete [ProfessionalVerification]: 
        '3.1 No Result Record, Delete
        '3.2 Result Record, Update Return for Amendment Status
        ' 4. Update [SPAccountUpdate] Status
        ' 5. Delete [SPAccountUpdate] Status (SPAccountUpdate Create Only When User A -> User B)

        Dim udtGeneral As New Common.ComFunction.GeneralFunction()
        Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()
        Dim blnSuccess As Boolean = True
        ' -------------------------------------------------------------------------------------
        '' Init BLL
        Dim udtSPVerBLL As New ServiceProviderVerificationBLL()
        Dim udtBAVerBLL As New BankAccVerificationBLL()
        Dim udtPVerBLL As New ProfessionalVerificationBLL()

        Dim udtSPAccUpdBLL As New SPAccountUpdateBLL()

        ' -------------------------------------------------------------------------------------
        ' Update [ServiceProviderVerification]: Return for Amendment Status + Clear Other Values

        If blnSuccess Then
            ' Update [ServiceProviderVerification]
            udtSPVerModel.UpdateBy = strUserId
            udtSPVerModel.ReturnForAmendmentBy = strUserId
            udtSPVerModel.RecordStatus = Common.Component.ServiceProviderVerificationStatus.ReturnForAmendment
            blnSuccess = udtSPVerBLL.UpdateServiceProviderVerificationReturnAmend(udtSPVerModel, udtDB)
        End If

        If blnSuccess Then
            ' Delete [BankAccVerification]
            For Each drBAVer As DataRow In dtBAVer.Rows
                'display_Seq, tsmp
                Dim intSeq As Integer = Convert.ToInt32(drBAVer("display_Seq"))
                Dim intSPPracticeDisplaySeq As Integer = Convert.ToInt32(drBAVer("sp_practice_display_Seq"))
                Dim arrByteTSMP As Byte() = CType(drBAVer("TSMP"), Byte())
                udtBAVerBLL.DeleteBankAccVerification(udtDB, strEnrolmentRefNo, intSeq, intSPPracticeDisplaySeq, arrByteTSMP, False)
            Next
        End If

        If blnSuccess Then
            ' Update/Delete [ProfessionalVerification]
            For Each udtPVerModel As ProfessionalVerificationModel In udtPVModelCollection.Values
                If Not udtPVerModel.VerificationResult Is Nothing AndAlso udtPVerModel.VerificationResult.Trim() <> "" _
                    AndAlso Not udtPVerModel.ImportBy Is Nothing AndAlso udtPVerModel.ImportBy.Trim() <> "" _
                    AndAlso udtPVerModel.ImportDtm.HasValue Then
                    ' Update
                    udtPVerBLL.UpdateProfessionalVerificationStatus(udtDB, strEnrolmentRefNo, udtPVerModel.ProfessionalSeq, strUserId, Common.Component.ProfessionalVerificationRecordStatus.ReturnForAmendment, udtPVerModel.TSMP)
                Else
                    ' Delete
                    udtPVerBLL.DeleteProfessionalVerification(udtDB, strEnrolmentRefNo, udtPVerModel.ProfessionalSeq, udtPVerModel.TSMP, True)
                End If
            Next
        End If



        If blnSuccess Then
            ' Update [SPAccountUpdate] Status

            If blnSuccess Then
                udtSPAccUpdateModel.ProgressStatus = Common.Component.SPAccountUpdateProgressStatus.DataEntryStage
                udtSPAccUpdateModel.UpdateBy = strUserId
                blnSuccess = udtSPAccUpdBLL.UpdateSPAccountUpdateProgressStatus(udtSPAccUpdateModel, udtDB)
            End If

            ' Delete SPAccountUpdate
            If blnSuccess Then
                ' Delete [SPAccountUpdate]
                If Not IsNothing(udtSPAccUpdateModel) Then
                    udtSPAccUpdBLL.DeleteSPAccountUpdate(strEnrolmentRefNo, udtSPAccUpdateModel.TSMP, udtDB, False)
                End If
            End If

        End If

        Return blnSuccess

    End Function
#End Region

#Region "Accept Enrolment"

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    'Public Function AcceptSPProfileUserCUserD(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal strUserId As String, ByVal tsmpSpAccUpdate As Byte(), ByVal alEnrolledSchemeCode As ArrayList)
    Public Function AcceptSPProfile(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal strUserId As String, ByVal tsmpSpAccUpdate As Byte(), ByVal alEnrolledSchemeCode As ArrayList) As Boolean
        ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

        ' Remark: This Function will not Update Verification , Please Update By YourSelf out of this function.
        ' This Function Will Validate SPAccountUpdate TSMP Before Accept Enrolment

        ' Concurrency Validation
        ' Validate on [SPAccountUpdate].TSMP

        ' -------------------------------------------------------------------------------------
        '' Init 
        Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
        Dim dtBAVer As DataTable = Nothing
        Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing

        Dim udtSPStagingModel As ServiceProviderModel = Nothing
        Dim udtPractStagingModelCollection As PracticeModelCollection = Nothing
        Dim udtBAStagingModelCollection As BankAcctModelCollection = Nothing
        Dim udtPStagingModelCollection As ProfessionalModelCollection = Nothing
        Dim udtMOStagingModelCollection As MedicalOrganizationModelCollection = Nothing
        Dim udtERNProcessedModelCollection As ERNProcessedModelCollection = Nothing
        Dim udtExistingPracticeSchemeInfoModelCollection As PracticeSchemeInfoModelCollection = Nothing


        Dim udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection = Nothing

        Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

        ' -------------------------------------------------------------------------------------

        Dim blnSuccess As Boolean = True

        ' Retrieve Profile
        Me.RetrieveSPProfileForAcceptAndSetPermanentTSMP(udtDB, strEnrolmentRefNo, _
            udtSPVerModel, dtBAVer, udtPVModelCollection, _
            udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
            udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection, udtMOStagingModelCollection, udtERNProcessedModelCollection)

        If Not IsNothing(udtSPStagingModel) Then
            If Not udtSPStagingModel.SPID.Trim.Equals(String.Empty) Then
                udtExistingPracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListBySPID(udtSPStagingModel.SPID, udtDB)
            End If
        End If

        ' Validation on [SPAccountUpdate].TSMP
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

        If Not IsNothing(udtSPAccUpdateModel) Then
            Dim params1() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPAccUpdateModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
            udtDB.RunProc("proc_checkTSMP", params1)
        Else
            'If model is nothing, that means the record is removed, so pass a null to db to raise an error
            Dim params1() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
            udtDB.RunProc("proc_checkTSMP", params1)
        End If

        ' Accept Enrolment
        blnSuccess = Me.AcceptSPProfileCore(udtDB, strEnrolmentRefNo, strUserId, _
                        udtSPVerModel, dtBAVer, udtPVModelCollection, _
                        udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
                        udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection, alEnrolledSchemeCode, udtMOStagingModelCollection, _
                        udtERNProcessedModelCollection, udtExistingPracticeSchemeInfoModelCollection)

        Return blnSuccess

    End Function

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Remove partial accept flow (Pass to Scheme Enrolment to complete whole enrolment)
    'Public Function PartiallyAcceptSPProfileUserCUserD(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal strUserId As String, ByVal tsmpSpAccUpdate As Byte(), ByVal alEnrolledSchemeCode As ArrayList)


    '    ' Remark: This Function will not Update Verification , Please Update By YourSelf out of this function.
    '    ' This Function Will Validate SPAccountUpdate TSMP Before Accept Enrolment

    '    ' -------------------------------------------------------------------------------------
    '    '' Init 
    '    Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
    '    Dim dtBAVer As DataTable = Nothing
    '    Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing

    '    Dim udtSPStagingModel As ServiceProviderModel = Nothing
    '    Dim udtPractStagingModelCollection As PracticeModelCollection = Nothing
    '    Dim udtBAStagingModelCollection As BankAcctModelCollection = Nothing
    '    Dim udtPStagingModelCollection As ProfessionalModelCollection = Nothing

    '    Dim udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection = Nothing

    '    Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

    '    Dim udtSPVerBLL As New ServiceProviderVerificationBLL()
    '    Dim udtBAVerBLL As New BankAccVerificationBLL()
    '    Dim udtPVerBLL As New ProfessionalVerificationBLL()

    '    Dim udtSPAccUpdBLL As New SPAccountUpdateBLL()

    '    ' -------------------------------------------------------------------------------------

    '    Dim blnSuccess As Boolean = True


    '    ' Retrieve Profile
    '    Me.RetrieveSPProfileForPartialAcceptForStagingUpdate(udtDB, strEnrolmentRefNo, _
    '        udtSPVerModel, dtBAVer, udtPVModelCollection, _
    '        udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
    '        udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

    '    ' Retrieve Permanent Model of SP (with Practice and MO) for Accept Scheme Enrolment update
    '    Dim udtSPPermanent As ServiceProviderModel
    '    udtSPPermanent = udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, udtSPVerModel.SPID)

    '    ' Validation
    '    ' User C & D: Validate on [SPAccountUpdate].TSMP
    '    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

    '    If Not IsNothing(udtSPAccUpdateModel) Then
    '        Dim params1() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPAccUpdateModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
    '        udtDB.RunProc("proc_checkTSMP", params1)
    '    Else
    '        Dim params1() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
    '        udtDB.RunProc("proc_checkTSMP", params1)
    '    End If

    '    ' Partially Accept Enrolment
    '    blnSuccess = Me.PartiallyAcceptSPProfileCore(udtDB, strEnrolmentRefNo, strUserId, _
    '                    udtSPVerModel, dtBAVer, udtPVModelCollection, _
    '                    udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
    '                    udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection, alEnrolledSchemeCode, udtSPPermanent)

    '    Return blnSuccess

    'End Function
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    Private Sub RetrieveSPProfileForAcceptAndSetPermanentTSMP(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, _
      ByRef udtSPVerModel As ServiceProviderVerificationModel, ByRef dtBAVer As DataTable, ByRef udtPVModelCollection As ProfessionalVerificationModelCollection, _
      ByRef udtSPStagingModel As ServiceProviderModel, ByRef udtPractStagingModelCollection As PracticeModelCollection, _
      ByRef udtBAStagingModelCollection As BankAcctModelCollection, ByRef udtPStagingModelCollection As ProfessionalModelCollection, _
      ByRef udtSPAccUpdateModel As SPAccountUpdateModel, ByRef udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection, _
      ByRef udtMOStagingModelCollection As MedicalOrganizationModelCollection, ByVal udtERNProcessedModelCollection As ERNProcessedModelCollection)

        ' Retrieve *.Verification Records
        Dim udtSPVerBLL As New ServiceProviderVerificationBLL()
        Dim udtBAVerBLL As New BankAccVerificationBLL()
        Dim udtPVerBLL As New ProfessionalVerificationBLL()

        udtSPVerModel = udtSPVerBLL.GetSerivceProviderVerificationByERN(strEnrolmentRefNo, udtDB)
        dtBAVer = udtBAVerBLL.GetBankAccVerificationListByERN(strEnrolmentRefNo, udtDB)
        udtPVModelCollection = udtPVerBLL.GetProfessionalVerificationListByERN(strEnrolmentRefNo, udtDB)

        ' Retrieve *.Staging Records
        Dim udtSPBLL As New ServiceProviderBLL()
        Dim udtPracticeBLL As New PracticeBLL()
        Dim udtBABLL As New BankAcctBLL()
        Dim udtPBLL As New ProfessionalBLL()
        Dim udtMOBLL As New MedicalOrganizationBLL()
        Dim udtERNBLL As New ERNProcessedBLL

        Dim udtSchemeInfoBLL As New SchemeInformationBLL()

        udtSchemeInfoStagingModelCollection = udtSchemeInfoBLL.GetSchemeInfoListStaging(strEnrolmentRefNo, udtDB)

        udtSPStagingModel = udtSPBLL.GetServiceProviderStagingByERN(strEnrolmentRefNo, udtDB)

        If Not IsNothing(udtSPStagingModel) Then
            ' Get ServiceProvider TSMP
            udtSPStagingModel.TSMP = udtSPBLL.GetserviceProviderPermanentTSMP(udtSPStagingModel.SPID, udtDB)

            ' Get (1) PracticeStaging ;(2) Practice TSMP; (3) PracticeSchemeInfoStaging; (4) PracticeSchemeInfo TSMP
            udtPractStagingModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(strEnrolmentRefNo, udtDB)

            Dim htPracticeTSMP As Hashtable = udtPracticeBLL.GetPracticeListPermanentTSMP(udtSPStagingModel.SPID, udtDB)

            For Each udtPracticeStaging As PracticeModel In udtPractStagingModelCollection.Values
                udtPracticeStaging.TSMP = htPracticeTSMP.Item(udtPracticeStaging.DisplaySeq)

                Dim htPracticeSchemeTSMP As Hashtable = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListPermanentTSMPBySPIDPracticeDisplaySeq(udtPracticeStaging.SPID, udtPracticeStaging.DisplaySeq, udtDB)

                For Each udtPracticeSchemeStaging As PracticeSchemeInfoModel In udtPracticeStaging.PracticeSchemeInfoList.Values
                    udtPracticeSchemeStaging.TSMP = htPracticeSchemeTSMP(udtPracticeSchemeStaging.SchemeCode + "-" + udtPracticeSchemeStaging.SubsidizeCode)
                Next
            Next

            ' Get BankAccountStaging
            udtBAStagingModelCollection = udtBABLL.GetBankAcctListFromStagingByERN(strEnrolmentRefNo, udtDB)

            ' Get ProfessionalStaging
            udtPStagingModelCollection = udtPBLL.GetProfessinalListFromStagingByERN(strEnrolmentRefNo, udtDB)

            ' Get (1) MedicalOrganizationStaging; (2) MedicalOrganization TSMP
            udtMOStagingModelCollection = udtMOBLL.GetMOListFromStagingByERN(strEnrolmentRefNo, udtDB)

            Dim htMOTSMP As Hashtable = udtMOBLL.GetMOListPermanentTSMP(udtSPStagingModel.SPID, udtDB)

            For Each udtMOStaging As MedicalOrganizationModel In udtMOStagingModelCollection.Values
                udtMOStaging.TSMP = htMOTSMP.Item(udtMOStaging.DisplaySeq)
            Next

            ' Get SPAccountUpdate
            Dim udtSPAccUpdateBLL As New SPAccountUpdateBLL()
            udtSPAccUpdateModel = udtSPAccUpdateBLL.GetSPAccountUpdateByERN(strEnrolmentRefNo, udtDB)

            ' Get ERNProcessdStaging
            udtERNProcessedModelCollection = udtERNBLL.GetERNProcessedListStagingByERN(strEnrolmentRefNo, udtDB)
        Else
            'Do nothing            
        End If
    End Sub

    Private Sub RetrieveSPProfileForPartialAcceptForStagingUpdate(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, _
      ByRef udtSPVerModel As ServiceProviderVerificationModel, ByRef dtBAVer As DataTable, ByRef udtPVModelCollection As ProfessionalVerificationModelCollection, _
      ByRef udtSPStagingModel As ServiceProviderModel, ByRef udtPractStagingModelCollection As PracticeModelCollection, _
      ByRef udtBAStagingModelCollection As BankAcctModelCollection, ByRef udtPStagingModelCollection As ProfessionalModelCollection, _
      ByRef udtSPAccUpdateModel As SPAccountUpdateModel, ByRef udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection)

        ' Retrieve *.Verification Records
        Dim udtSPVerBLL As New ServiceProviderVerificationBLL()
        Dim udtBAVerBLL As New BankAccVerificationBLL()
        Dim udtPVerBLL As New ProfessionalVerificationBLL()


        udtSPVerModel = udtSPVerBLL.GetSerivceProviderVerificationByERN(strEnrolmentRefNo, udtDB)
        dtBAVer = udtBAVerBLL.GetBankAccVerificationListByERN(strEnrolmentRefNo, udtDB)
        udtPVModelCollection = udtPVerBLL.GetProfessionalVerificationListByERN(strEnrolmentRefNo, udtDB)

        ' Retrieve *.Staging Records
        Dim udtSPBLL As New ServiceProviderBLL()
        Dim udtPracticeBLL As New PracticeBLL()
        Dim udtBABLL As New BankAcctBLL()
        Dim udtPBLL As New ProfessionalBLL()
        Dim udtMOBLL As New MedicalOrganizationBLL()

        Dim udtSchemeInfoBLL As New SchemeInformationBLL()

        udtSchemeInfoStagingModelCollection = udtSchemeInfoBLL.GetSchemeInfoListStaging(strEnrolmentRefNo, udtDB)

        udtSPStagingModel = udtSPBLL.GetServiceProviderStagingByERN(strEnrolmentRefNo, udtDB)
        udtPractStagingModelCollection = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(strEnrolmentRefNo, udtDB)
        udtBAStagingModelCollection = udtBABLL.GetBankAcctListFromStagingByERN(strEnrolmentRefNo, udtDB)
        udtPStagingModelCollection = udtPBLL.GetProfessinalListFromStagingByERN(strEnrolmentRefNo, udtDB)

        udtSPStagingModel.TSMP = udtSPBLL.GetserviceProviderPermanentTSMP(udtSPStagingModel.SPID, udtDB)

        ' Retrieve SPAccountUpdate
        Dim udtSPAccUpdateBLL As New SPAccountUpdateBLL()
        udtSPAccUpdateModel = udtSPAccUpdateBLL.GetSPAccountUpdateByERN(strEnrolmentRefNo, udtDB)

    End Sub

    Private Function AcceptSPProfileCore(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
    ByRef udtSPVerModel As ServiceProviderVerificationModel, ByRef dtBAVer As DataTable, ByRef udtPVModelCollection As ProfessionalVerificationModelCollection, _
    ByRef udtSPStagingModel As ServiceProviderModel, ByRef udtPractStagingModelCollection As PracticeModelCollection, _
    ByRef udtBAStagingModelCollection As BankAcctModelCollection, ByRef udtPStagingModelCollection As ProfessionalModelCollection, _
    ByRef udtSPAccUpdateModel As SPAccountUpdateModel, ByRef udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection, ByVal alEnrolledSchemeCode As ArrayList, _
    ByRef udtMOStagingModelCollection As MedicalOrganizationModelCollection, ByRef udtERNProcessedModelCollection As ERNProcessedModelCollection, _
    ByRef udtExistingPracticeSchemeInfoModelCollection As PracticeSchemeInfoModelCollection) As Boolean

        ' Newly Add (2008-07-15): If Email Changed, Add an Email Change confirmation Mail to Mail queue 

        Dim blnEmailChanged As Boolean = False
        If udtSPStagingModel.EmailChanged = Common.Component.EmailChanged.Changed Then
            blnEmailChanged = True
        End If

        ' <1> Update SPAccountUpdate.Progress_Status = 'C'
        ' <2> Update *.Staging to Service Provider*   New Add: [SchemeInformationStaging]
        ' <3> Delete *.Verification
        ' <4> Delete *.Staging ' New Add: [SchemeInformationStaging]
        ' <5> Delete SPAccountUpdate
        ' <6> Send Email for email changed

        Dim blnSuccess As Boolean = True

        Try
            'udtDB.BeginTransaction()
            ' -------------------------------------------------------------------------------------
            '' Init BLL
            Dim udtSPVerBLL As New ServiceProviderVerificationBLL()
            Dim udtBAVerBLL As New BankAccVerificationBLL()
            Dim udtPVerBLL As New ProfessionalVerificationBLL()

            ' Retrieve *.Staging Records
            Dim udtSPBLL As New ServiceProviderBLL()
            Dim udtPracticeBLL As New PracticeBLL()
            Dim udtBABLL As New BankAcctBLL()
            Dim udtPBLL As New ProfessionalBLL()
            Dim udtMOBLL As New MedicalOrganizationBLL()
            Dim udtERNProcessedBLL As New ERNProcessedBLL
            Dim udtPracticeSchemeBLL As New PracticeSchemeInfoBLL

            Dim udtSchemeInfoBLL As New SchemeInformationBLL()

            Dim udtSPAccUpdBLL As New SPAccountUpdateBLL()

            ' -------------------------------------------------------------------------------------
            '<1> Update SPAccountUpdate.Progress_Status = 'C'

            If blnSuccess Then
                If Not IsNothing(udtSPAccUpdateModel) Then
                    udtSPAccUpdateModel.ProgressStatus = Common.Component.SPAccountUpdateProgressStatus.CompletionStageWithTokenIssued
                    udtSPAccUpdateModel.UpdateBy = strUserId
                    blnSuccess = udtSPAccUpdBLL.UpdateSPAccountUpdateProgressStatus(udtSPAccUpdateModel, udtDB)
                End If
            End If

            ' -------------------------------------------------------------------------------------
            '<2> Update *.Staging to Service Provider*

            If blnSuccess Then
                udtSPStagingModel.UpdateBy = strUserId
                ' INT13-0028 - SP Amendment Report [Start][Tommy L]
                ' -------------------------------------------------------------------------
                If Not IsNothing(udtSPAccUpdateModel) Then
                    udtSPStagingModel.DataInputBy = udtSPAccUpdateModel.DataInputBy
                Else
                    udtSPStagingModel.DataInputBy = udtSPStagingModel.CreateBy
                End If
                ' INT13-0028 - SP Amendment Report [End][Tommy L]
                blnSuccess = udtSPBLL.UpdateServiceProviderPermanentParticulars(udtSPStagingModel, udtDB)
            End If

            ' [MedicalOriganization Staging] -> MedicalOrganization*
            For Each udtMOModel As MedicalOrganizationModel In udtMOStagingModelCollection.Values 'udtSPStagingModel.MOList.Values
                If blnSuccess Then
                    ' **** MODisplayStatus ****
                    ' A: New Add: Insert
                    ' U: Update
                    ' Else: Do nothing
                    If udtMOModel.RecordStatus = Common.Component.MedicalOrganizationStagingStatus.Active Then
                        ' Insert
                        udtMOModel.UpdateBy = strUserId
                        blnSuccess = udtMOBLL.AddMOToPermanent(udtMOModel, udtDB)
                    ElseIf udtMOModel.RecordStatus = Common.Component.MedicalOrganizationStagingStatus.Update Then
                        ' Update
                        udtMOModel.UpdateBy = strUserId
                        blnSuccess = udtMOBLL.UpdateMOPermanentDetails(udtMOModel, udtDB)
                    Else
                        ' Do Nothing
                    End If

                End If
            Next

            ' [Practice Staging] -> Practice*
            For Each udtPracticeModel As PracticeModel In udtPractStagingModelCollection.Values
                If blnSuccess Then
                    ' **** PracticeDisplayStatus ****
                    ' A: New Add: Insert
                    ' U: Update
                    ' Else: Do nothing
                    If udtPracticeModel.RecordStatus = Common.Component.PracticeStagingStatus.Active Then
                        ' Insert
                        udtPracticeModel.UpdateBy = strUserId
                        blnSuccess = udtPracticeBLL.AddPracticeToPermanent(udtPracticeModel, udtDB)
                    ElseIf udtPracticeModel.RecordStatus = Common.Component.PracticeStagingStatus.Update Then
                        ' Update
                        udtPracticeModel.UpdateBy = strUserId
                        blnSuccess = udtPracticeBLL.UpdatePracticePermanentAddress(udtPracticeModel, udtDB)
                    Else
                        ' Do Nothing
                    End If


                    ' [PracticeSchemeInfo Staging] -> [PracticeSchemeInfo]*
                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------                    
                    ' Not passing Enrolled scheme list as all added scheme should be handlded no matter existing or new enroll scheme
                    blnSuccess = (New PracticeSchemeInfoBLL).FillPracticeSchemeInfoPermanent(udtPracticeModel, udtExistingPracticeSchemeInfoModelCollection, udtDB, strUserId)
                    'blnSuccess = (New PracticeSchemeInfoBLL).FillPracticeSchemeInfoPermanent(udtPracticeModel, udtExistingPracticeSchemeInfoModelCollection, udtDB, strUserId, alEnrolledSchemeCode)
                    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

                End If
            Next    'Next udtPracticeModel

            ' BankAccountStaging -> Bank Account*
            For Each udtBAModel As BankAcctModel In udtBAStagingModelCollection.Values
                If blnSuccess Then
                    If udtBAModel.RecordStatus = Common.Component.BankAcctStagingStatus.Active Then
                        ' Insert
                        udtBABLL.AddBankAcctToPermanent(udtBAModel, udtDB)
                    End If
                End If
            Next

            ' Professional Staging -> Professional
            For Each udtPModel As ProfessionalModel In udtPStagingModelCollection.Values
                If blnSuccess Then
                    If udtPModel.RecordStatus = Common.Component.ProfessionalStagingStatus.Active Then
                        udtPBLL.AddProfessionalToPermanent(udtPModel, udtDB)
                    End If
                End If
            Next

            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' Scheme Information Staging -> Scheme Information
            'Get the Permanent Scheme Info List for checking whether the scheme is new added or rejoin 
            Dim udtSchemeInfoListPermanent As New SchemeInformationModelCollection
            If Not IsNothing(udtSPStagingModel) Then
                If Not udtSPStagingModel.SPID.Trim.Equals(String.Empty) Then
                    udtSchemeInfoListPermanent = udtSchemeInformationBLL.GetSchemeInfoListPermanent(udtSPStagingModel.SPID, udtDB)
                End If
            End If

            ' Update Table SchemeInformation - Record_Status -> "A"
            udtSchemeInformationBLL.AddNewAddedSchemeInfoListToPermanent(udtSchemeInfoStagingModelCollection, udtDB, udtSchemeInfoListPermanent)

            'Update newly added scheme to suspend if the SP is suspended
            Dim udtSPPermanent As ServiceProvider.ServiceProviderModel
            Dim udtSchemeInformationCollectionPermanent As SchemeInformation.SchemeInformationModelCollection
            Dim strSuspendRemark As String = Nothing

            udtSPPermanent = udtSPBLL.GetServiceProviderPermanentProfileByERN(udtSPStagingModel.EnrolRefNo, udtDB)

            If udtSPPermanent.RecordStatus = ServiceProviderTokenStatus.Suspeneded Then

                '"Due to the service provider is suspended before scheme enrolment
                strSuspendRemark = HttpContext.GetGlobalResourceObject("Text", "AutoSchemeSuspendRemark").ToString
                udtSchemeInformationCollectionPermanent = udtSPPermanent.SchemeInfoList

                For Each udtSchemeInformation As SchemeInformationModel In udtSchemeInformationCollectionPermanent.Values
                    'only update active scheme
                    If udtSchemeInformation.RecordStatus = SchemeInformationStatus.Active Then

                        udtSchemeInformationBLL.UpdateSchemeInfoPermanentStatus(udtSPStagingModel.SPID, udtSchemeInformation.SchemeCode, SchemeInformationStatus.Suspended, Nothing, _
                        strSuspendRemark, udtSPStagingModel.UpdateBy, udtSchemeInformation.TSMP, udtDB)

                    End If
                Next
            End If
            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]


            ' ERNProcessed Staging -> ERNProcessed
            'If the ERNProcessedModelCollection is staging is not empty, then add back those not existed in permanent to permanent
            If Not IsNothing(udtERNProcessedModelCollection) AndAlso blnSuccess Then
                If Not IsNothing(udtSPStagingModel.SPID) AndAlso Not udtSPStagingModel.SPID.Trim.Equals(String.Empty) Then
                    'if Have SPID, try to search for permanent
                    Dim udtPermanentERNList As ERNProcessedModelCollection
                    udtPermanentERNList = udtERNProcessedBLL.GetERNProcessedListPermanentBySPID(udtSPStagingModel.SPID, udtDB)
                    For Each udtERNProcessedModel As ERNProcessedModel In udtERNProcessedModelCollection.Values
                        If Not IsNothing(udtPermanentERNList) Then
                            'If have permanent list, check not exist and then add
                            If IsNothing(udtPermanentERNList(udtERNProcessedModel.SubEnrolRefNo)) Then
                                udtERNProcessedBLL.AddERNProcessedToPermanent(udtERNProcessedModel, udtDB)
                            Else
                                ' If Found (Merged before!), Do Nothing
                            End If
                        Else
                            'Empty in permanent, then add
                            udtERNProcessedBLL.AddERNProcessedToPermanent(udtERNProcessedModel, udtDB)
                        End If
                    Next
                Else
                    'Without SPID case, permanent should be empty, then add
                    For Each udtERNProcessedModel As ERNProcessedModel In udtERNProcessedModelCollection.Values
                        udtERNProcessedBLL.AddERNProcessedToPermanent(udtERNProcessedModel, udtDB)
                    Next
                End If
            End If

            ' -------------------------------------------------------------------------------------
            '<3> Delete *.Verification

            If blnSuccess Then
                ' [ProfessionalVerification]
                For Each udtPVerModel As ProfessionalVerificationModel In udtPVModelCollection.Values
                    udtPVerBLL.DeleteProfessionalVerification(udtDB, strEnrolmentRefNo, udtPVerModel.ProfessionalSeq, udtPVerModel.TSMP, False)
                Next
            End If

            If blnSuccess Then
                ' [BankAccVerification]
                For Each drBAVer As DataRow In dtBAVer.Rows
                    If blnSuccess Then
                        'display_Seq, tsmp
                        Dim intSeq As Integer = Convert.ToInt32(drBAVer("display_Seq"))
                        Dim intSPPracticeDisplaySeq As Integer = Convert.ToInt32(drBAVer("sp_practice_display_Seq"))
                        Dim arrByteTSMP As Byte() = CType(drBAVer("TSMP"), Byte())
                        udtBAVerBLL.DeleteBankAccVerification(udtDB, strEnrolmentRefNo, intSeq, intSPPracticeDisplaySeq, arrByteTSMP, False)
                    Else
                        Exit For
                    End If
                Next
            End If

            If blnSuccess Then
                ' [ServiceProviderVerification]
                udtSPVerBLL.DeleteServiceProviderVerification(udtDB, strEnrolmentRefNo, udtSPVerModel.TSMP, False)
            End If

            ' -------------------------------------------------------------------------------------
            ' <4> Delete *.Staging
            If blnSuccess Then
                ' [ProfessionalStaging]
                For Each udtPStagingModel As ProfessionalModel In udtPStagingModelCollection.Values
                    udtPBLL.DeleteProfessionalStagingByKey(udtDB, strEnrolmentRefNo, udtPStagingModel.ProfessionalSeq)
                Next
            End If

            If blnSuccess Then
                ' [BankAccountStaging]
                For Each udtBAModel As BankAcctModel In udtBAStagingModelCollection.Values
                    udtBABLL.DeleteBankAccountStagingByKey(udtDB, strEnrolmentRefNo, udtBAModel.DisplaySeq, udtBAModel.SpPracticeDisplaySeq, udtBAModel.TSMP, False)
                Next
            End If

            If blnSuccess Then
                ' [PracticeStaging]
                For Each udtPracticeModel As PracticeModel In udtPractStagingModelCollection.Values
                    udtPracticeBLL.DeletePracticeStagingByKey(udtDB, strEnrolmentRefNo, udtPracticeModel.DisplaySeq, udtPracticeModel.TSMP, False)

                    For Each udtPracticeSchemeStaging As PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
                        udtPracticeSchemeInfoBLL.DeletePracticeSchemeInfoStagingByKey(udtDB, udtPracticeSchemeStaging, Nothing, False)
                    Next
                Next
            End If

            If blnSuccess Then
                ' [MedicalOrganizationStaging]
                For Each udtMOModel As MedicalOrganizationModel In udtSPStagingModel.MOList.Values
                    udtMOBLL.DeleteMOStagingByKey(udtDB, udtMOModel.EnrolRefNo, udtMOModel.DisplaySeq, udtMOModel.TSMP, False)
                Next
            End If

            If blnSuccess Then
                ' [ServiceProviderStaging]
                udtSPBLL.DeleteServiceProviderStagingByKey(udtDB, strEnrolmentRefNo, udtSPStagingModel.TSMP, False)
            End If

            If blnSuccess Then
                ' [SchemeInformationStaging]
                For Each udtSchemeInfoStagingModel As SchemeInformationModel In udtSchemeInfoStagingModelCollection.Values
                    udtSchemeInfoBLL.DeleteSchemeInfoStaging(udtDB, udtSchemeInfoStagingModel.EnrolRefNo, udtSchemeInfoStagingModel.SchemeCode)
                Next
            End If

            If blnSuccess Then
                ' [ERNProcessedStaging]
                If Not IsNothing(udtERNProcessedModelCollection) Then
                    For Each udtERNPModel As ERNProcessedModel In udtERNProcessedModelCollection.Values
                        udtERNProcessedBLL.DeleteERNProcessedStaging(udtERNPModel, udtDB)
                    Next
                End If
            End If

            ' -------------------------------------------------------------------------------------
            ' <5> Delete SPAccountUpdate
            If blnSuccess Then
                ' Delete [SPAccountUpdate]
                If Not IsNothing(udtSPAccUpdateModel) Then
                    udtSPAccUpdBLL.DeleteSPAccountUpdate(strEnrolmentRefNo, udtSPAccUpdateModel.TSMP, udtDB, False)
                End If
            End If
            ' -------------------------------------------------------------------------------------

            ' -------------------------------------------------------------------------------------
            ' <6> Send Email for email changed
            ' Update Activate Code & Add Mail Queue

            If blnEmailChanged Then

                If blnSuccess Then

                    '<1> If the SP does not activate his/her account, resend the activation email to his/her new email.
                    '<2> If the SP activates his/her account, send the confirmation email to his/her new email.

                    Dim dtUserAcc As DataTable = New DataTable

                    Dim blnActivated As Boolean = False

                    ' CRE16-026 (Change email for locked SP) [Start][Winnie]
                    Dim udtUserACBLL As New UserAC.UserACBLL

                    If udtUserACBLL.IsUserACPendingActivation(udtSPStagingModel.SPID) Then
                        blnActivated = True
                    End If
                    ' CRE16-026 (Change email for locked SP) [End][Winnie]

                    If blnActivated Then
                        '<1>
                        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()
                        Dim strActivationCode As String = udtGeneralFunction.generateAccountActivationCode()
                        Dim udtInternetMailBLL As New InternetMail.InternetMailBLL()

                        If blnSuccess Then
                            Dim udtACBLL As UserAC.UserACBLL = New UserAC.UserACBLL
                            blnSuccess = udtACBLL.UpdateUserACActivationCode(udtSPStagingModel.SPID, Hash(strActivationCode), strUserId, udtDB)
                        End If

                        If blnSuccess Then
                            blnSuccess = UpdateSPEmail(udtDB, udtSPStagingModel, False, strUserId)
                        End If

                        'Send Email
                        Dim MSchemeCodeArrayList As ArrayList = New ArrayList

                        ' CRE16-018 (Display SP tentative email in HCVU) [Start][Lawrence]
                        For Each udtSchemeInfoModel As SchemeInformationModel In udtSchemeInfoStagingModelCollection.Values
                            If udtSchemeInfoModel.RecordStatus = SchemeInformationStagingStatus.Active _
                                OrElse udtSchemeInfoModel.RecordStatus = SchemeInformationStagingStatus.Existing _
                                    OrElse udtSchemeInfoModel.RecordStatus = SchemeInformationStagingStatus.Suspended Then
                                MSchemeCodeArrayList.Add(udtSchemeInfoModel.SchemeCode)
                            End If
                        Next
                        ' CRE16-018 (Display SP tentative email in HCVU) [End][Lawrence]

                        ' Get token project from [Token]
                        Dim strProject As String = Nothing

                        Dim udtToken As TokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtSPStagingModel.SPID, udtDB)

                        ' If the token cannot be found, get from [TokenDeactivated]
                        If IsNothing(udtToken) Then
                            Dim dt As DataTable = udtTokenBLL.GetTokenDeactivatedByUserID(udtSPStagingModel.SPID, udtDB)

                            If dt.Rows.Count > 0 Then
                                If CStr(dt.Rows(0)("Project")).Trim = TokenProjectType.EHCVS Then
                                    strProject = TokenProjectType.EHCVS
                                Else
                                    strProject = TokenProjectType.EHR
                                End If

                            Else
                                Throw New Exception("Token not found for User ID " + udtSPStagingModel.SPID.Trim)
                            End If

                        Else
                            strProject = udtToken.Project
                        End If

                        If strProject = TokenProjectType.EHCVS Then
                            blnSuccess = udtInternetMailBLL.SubmitAccountActivationEmail(udtDB, udtSPStagingModel.SPID, strActivationCode, False, True, MSchemeCodeArrayList)
                        Else
                            blnSuccess = udtInternetMailBLL.SubmitAccountActivationEmail(udtDB, udtSPStagingModel.SPID, strActivationCode, True, True, MSchemeCodeArrayList)
                        End If

                    Else
                        '<2>
                        ' Update Activation Code to Service Provider
                        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()
                        Dim strActivationCode As String = udtGeneralFunction.generateAccountActivationCode()

                        ' Update ActivationCode
                        blnSuccess = udtSPBLL.UpdateServiceProviderEmailActivationCode(udtDB, udtSPStagingModel.SPID, strUserId, Hash(strActivationCode), udtSPStagingModel.TSMP, False)

                        If blnSuccess Then
                            ' Add Mail Queue
                            Dim udtInternetMailBLL As New InternetMail.InternetMailBLL()
                            blnSuccess = udtInternetMailBLL.SubmitEmailAddressChangeConfirmationEmail(udtDB, udtSPStagingModel.SPID, strActivationCode)
                        End If
                    End If


                End If
            End If

            Return blnSuccess

        Catch ex As Exception
            'udtDB.RollBackTranscation()
            Throw
        End Try

        Return True

    End Function

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Remove partial accept flow (Pass to Scheme Enrolment to complete whole enrolment)
    'Private Function PartiallyAcceptSPProfileCore(ByRef udtDB As Database, ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
    '    ByRef udtSPVerModel As ServiceProviderVerificationModel, ByRef dtBAVer As DataTable, ByRef udtPVModelCollection As ProfessionalVerificationModelCollection, _
    '    ByRef udtSPStagingModel As ServiceProviderModel, ByRef udtPractStagingModelCollection As PracticeModelCollection, _
    '    ByRef udtBAStagingModelCollection As BankAcctModelCollection, ByRef udtPStagingModelCollection As ProfessionalModelCollection, _
    '    ByRef udtSPAccUpdateModel As SPAccountUpdateModel, ByRef udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection, ByVal alEnrolledSchemeCode As ArrayList, _
    '    ByVal udtSPPermanent As ServiceProviderModel) As Boolean

    '    ' Newly Add (2008-07-15): If Email Changed, Add an Email Change confirmation Mail to Mail queue 
    '    Dim blnEmailChanged As Boolean = False
    '    If udtSPStagingModel.EmailChanged = Common.Component.EmailChanged.Changed Then
    '        blnEmailChanged = True
    '    End If

    '    ' <1> Update SPAccountUpdate.Progress_Status = 'S' - Waiting for SchemeEnrolment
    '    ' <2> Update *.Staging to Service Provider*, EXCEPT the SchemeInformationStaging, PracticeSchemeInfoStaging
    '    ' <3> Update *.Staging status from NewAdd to Existing, EXCEPT the SchemeInformationStaging, PracticeSchemeInfoStaging
    '    ' --<3o> Delete *.Verification
    '    ' --<4> Delete *.Staging ' New Add: [SchemeInformationStaging]
    '    ' --<5> Delete SPAccountUpdate

    '    Dim blnSuccess As Boolean = True
    '    'Dim udtGeneral As New Common.ComFunction.GeneralFunction()
    '    'Dim dtmCurrent As DateTime = udtGeneral.GetSystemDateTime()

    '    Try
    '        'udtDB.BeginTransaction()
    '        ' -------------------------------------------------------------------------------------
    '        '' Init BLL
    '        Dim udtSPVerBLL As New ServiceProviderVerificationBLL()
    '        Dim udtBAVerBLL As New BankAccVerificationBLL()
    '        Dim udtPVerBLL As New ProfessionalVerificationBLL()

    '        ' Retrieve *.Staging Records
    '        Dim udtSPBLL As New ServiceProviderBLL()
    '        Dim udtPracticeBLL As New PracticeBLL()
    '        Dim udtBABLL As New BankAcctBLL()
    '        Dim udtPBLL As New ProfessionalBLL()
    '        Dim udtMOBLL As New MedicalOrganizationBLL()
    '        Dim udtPracticeSchemeBLL As New PracticeSchemeInfo.PracticeSchemeInfoBLL
    '        Dim udtERNProcessedBLL As New ERNProcessedBLL

    '        Dim udtSchemeInfoBLL As New SchemeInformationBLL()

    '        Dim udtSPAccUpdBLL As New SPAccountUpdateBLL()

    '        ' -------------------------------------------------------------------------------------
    '        ' 1. Update SPAccountUpdate Status To Completion Stage With Scheme Enrolment

    '        If blnSuccess Then
    '            If Not IsNothing(udtSPAccUpdateModel) Then
    '                udtSPAccUpdateModel.ProgressStatus = Common.Component.SPAccountUpdateProgressStatus.WaitingForSchemeEnrolment
    '                udtSPAccUpdateModel.UpdateBy = strUserId
    '                blnSuccess = udtSPAccUpdBLL.UpdateSPAccountUpdateProgressStatus(udtSPAccUpdateModel, udtDB)
    '            End If
    '        End If

    '        ' -------------------------------------------------------------------------------------
    '        ' 2. Update *.Staging to Service Provider*
    '        If blnSuccess Then
    '            udtSPStagingModel.UpdateBy = strUserId
    '            blnSuccess = udtSPBLL.UpdateServiceProviderPermanentParticularsBySchemeEnrolment(udtSPStagingModel, udtDB)
    '        End If

    '        ' [Practice Staging] -> Practice*; and [PracticeSchemeEnrolment Staging]
    '        For Each udtPracticeModel As PracticeModel In udtPractStagingModelCollection.Values
    '            If blnSuccess Then
    '                ' **** PracticeDisplayStatus ****
    '                ' A: New Add: Insert
    '                ' U: Update
    '                ' Else: Do nothing
    '                If udtPracticeModel.RecordStatus = Common.Component.PracticeStagingStatus.Active Then
    '                    ' Insert
    '                    udtPracticeModel.UpdateBy = strUserId
    '                    udtPracticeModel.SPID = udtSPStagingModel.SPID
    '                    blnSuccess = udtPracticeBLL.AddPracticeToPermanent(udtPracticeModel, udtDB)
    '                ElseIf udtPracticeModel.RecordStatus = Common.Component.PracticeStagingStatus.Update Then
    '                    ' Update
    '                    Dim udtPermanentPracticeModelList As PracticeModelCollection
    '                    Dim tsmpStaging As Byte()
    '                    tsmpStaging = udtPracticeModel.TSMP
    '                    'udtPermanentPracticeModelList = udtPracticeBLL.GetPracticeBankAcctListFromPermanentMaintenanceBySPID(udtPracticeModel.SPID, udtDB)
    '                    udtPermanentPracticeModelList = udtSPPermanent.PracticeList
    '                    If Not IsNothing(udtPermanentPracticeModelList) AndAlso Not IsNothing(udtPermanentPracticeModelList(udtPracticeModel.DisplaySeq)) Then
    '                        udtPracticeModel.TSMP = udtPermanentPracticeModelList(udtPracticeModel.DisplaySeq).TSMP
    '                    End If
    '                    udtPracticeModel.UpdateBy = strUserId
    '                    blnSuccess = udtPracticeBLL.UpdatePracticePermanentAddress(udtPracticeModel, udtDB)
    '                    'Restore the tsmp to staging
    '                    udtPracticeModel.TSMP = tsmpStaging
    '                Else
    '                    ' Do Nothing
    '                End If

    '                'For Each udtPracticeSchemeInfo As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
    '                '    Dim i As Integer
    '                '    For i = 0 To alEnrolledSchemeCode.Count - 1
    '                '        If udtPracticeSchemeInfo.MSchemeCode.Trim.Equals(alEnrolledSchemeCode(i)) Then
    '                '            If udtPracticeSchemeInfo.RecordStatus = Common.Component.PracticeDisplayStatus.NewAdd Then
    '                '                'Insert
    '                '                udtPracticeSchemeInfo.UpdateBy = strUserId
    '                '                udtPracticeSchemeInfo.SPID = udtSPStagingModel.SPID
    '                '                blnSuccess = udtPracticeSchemeBLL.AddPracticeSchemeInfoToPermanent(udtPracticeSchemeInfo, udtDB)

    '                '            ElseIf udtPracticeSchemeInfo.RecordStatus = Common.Component.PracticeDisplayStatus.Update Then
    '                '                'Update case only applied to non-EHCVS scheme
    '                '                'If Not udtPracticeSchemeInfo.SchemeCode.Trim.Equals(SchemeCode.EHCVS) Then
    '                '                udtPracticeSchemeInfo.UpdateBy = strUserId
    '                '                blnSuccess = udtPracticeSchemeBLL.UpdatePracticeSchemeInfoServiceFee(udtPracticeSchemeInfo, udtDB)
    '                '                'End If
    '                '            Else
    '                '                'Do nothing
    '                '            End If
    '                '        End If
    '                '    Next    'Next i: Next Enrolled Scheme
    '                'Next    'Next PracticeSchemeInfoModel
    '            End If
    '        Next

    '        ' [MedicalOriganization Staging] -> MedicalOrganization*
    '        ' Get MedicalOrganization TSMP
    '        'Dim htMOTSMP As Hashtable = udtMOBLL.GetMOListPermanentTSMP(udtSPStagingModel.SPID, udtDB)
    '        Dim udtMOPermanentList As MedicalOrganizationModelCollection
    '        udtMOPermanentList = udtSPPermanent.MOList

    '        For Each udtMOModel As MedicalOrganizationModel In udtSPStagingModel.MOList.Values
    '            If blnSuccess Then
    '                ' **** MODisplayStatus ****
    '                ' A: New Add: Insert
    '                ' U: Update
    '                ' Else: Do nothing
    '                If udtMOModel.RecordStatus = Common.Component.MedicalOrganizationStagingStatus.Active Then
    '                    ' Insert
    '                    udtMOModel.UpdateBy = strUserId
    '                    udtMOModel.SPID = udtSPStagingModel.SPID
    '                    blnSuccess = udtMOBLL.AddMOToPermanent(udtMOModel, udtDB)

    '                ElseIf udtMOModel.RecordStatus = Common.Component.MedicalOrganizationStagingStatus.Update Then
    '                    Dim bytMOStagingTSMP As Byte() = udtMOModel.TSMP

    '                    udtMOModel.UpdateBy = strUserId
    '                    'udtMOModel.TSMP = htMOTSMP(udtMOModel.DisplaySeq)
    '                    udtMOModel.TSMP = udtMOPermanentList.Item(udtMOModel.DisplaySeq).TSMP
    '                    blnSuccess = udtMOBLL.UpdateMOPermanentDetails(udtMOModel, udtDB)

    '                    udtMOModel.TSMP = bytMOStagingTSMP
    '                Else
    '                    ' Do Nothing
    '                End If

    '            End If
    '        Next

    '        ' BankAccountStaging -> Bank Account*
    '        For Each udtBAModel As BankAcctModel In udtBAStagingModelCollection.Values
    '            If blnSuccess Then
    '                If udtBAModel.RecordStatus = Common.Component.BankAcctStagingStatus.Active Then
    '                    ' Insert
    '                    udtBABLL.AddBankAcctToPermanent(udtBAModel, udtDB)
    '                End If
    '            End If
    '        Next

    '        ' Professional Staging -> Professional
    '        For Each udtPModel As ProfessionalModel In udtPStagingModelCollection.Values
    '            If blnSuccess Then
    '                If udtPModel.RecordStatus = Common.Component.ProfessionalStagingStatus.Active Then
    '                    udtPBLL.AddProfessionalToPermanent(udtPModel, udtDB)
    '                End If
    '            End If
    '        Next

    '        'Leave the ERNProcessedStaging record to be moved/removed at complete scheme enrolment
    '        '' ERNProcessed Staging -> ERNProcessed
    '        'Dim udtERNProcessedList As New ERNProcessedModelCollection
    '        'udtERNProcessedList = udtERNProcessedBLL.GetERNProcessedListStagingByERN(udtSPStagingModel.EnrolRefNo, udtDB)
    '        'If Not IsNothing(udtERNProcessedList) Then
    '        '    For Each udtERNPModel As ERNProcessedModel In udtERNProcessedList.Values
    '        '        If blnSuccess Then
    '        '            udtERNProcessedBLL.AddERNProcessedToPermanent(udtERNPModel, udtDB)
    '        '        End If
    '        '    Next
    '        'End If

    '        ' -------------------------------------------------------------------------------------
    '        ' 3. Update *.Staging status from NewAdd to Existing

    '        ' [Practice Staging]
    '        For Each udtPracticeModel As PracticeModel In udtPractStagingModelCollection.Values
    '            If blnSuccess Then
    '                ' **** PracticeDisplayStatus ****
    '                ' A: New Add: Insert
    '                ' U: Update
    '                ' Else: Do nothing
    '                If udtPracticeModel.RecordStatus = Common.Component.PracticeStagingStatus.Active Then
    '                    ' Insert
    '                    udtPracticeModel.UpdateBy = strUserId
    '                    udtPracticeModel.RecordStatus = PracticeStagingStatus.Existing
    '                    blnSuccess = udtPracticeBLL.UpdatePracticeStagingRecordStatus(udtPracticeModel, udtDB)
    '                ElseIf udtPracticeModel.RecordStatus = Common.Component.PracticeStagingStatus.Update Then
    '                    ' Update
    '                    udtPracticeModel.UpdateBy = strUserId
    '                    udtPracticeModel.RecordStatus = PracticeStagingStatus.Existing
    '                    blnSuccess = udtPracticeBLL.UpdatePracticeStagingRecordStatus(udtPracticeModel, udtDB)
    '                Else
    '                    ' Do Nothing
    '                End If

    '            End If
    '        Next

    '        ' [MedicalOriganization Staging]
    '        For Each udtMOModel As MedicalOrganizationModel In udtSPStagingModel.MOList.Values
    '            If blnSuccess Then
    '                ' **** MODisplayStatus ****
    '                ' A: New Add: Insert
    '                ' U: Update
    '                ' Else: Do nothing
    '                If udtMOModel.RecordStatus = Common.Component.MedicalOrganizationStagingStatus.Active Then
    '                    ' Insert
    '                    udtMOModel.UpdateBy = strUserId
    '                    udtMOModel.RecordStatus = MedicalOrganizationStagingStatus.Existing
    '                    blnSuccess = udtMOBLL.UpdateMOStagingStatus(udtMOModel.EnrolRefNo, udtMOModel.DisplaySeq, udtMOModel.RecordStatus, udtMOModel.UpdateBy, udtMOModel.TSMP, udtDB)
    '                ElseIf udtMOModel.RecordStatus = Common.Component.MedicalOrganizationStagingStatus.Update Then
    '                    ' Update
    '                    udtMOModel.UpdateBy = strUserId
    '                    udtMOModel.RecordStatus = MedicalOrganizationStagingStatus.Existing
    '                    blnSuccess = udtMOBLL.UpdateMOStagingStatus(udtMOModel.EnrolRefNo, udtMOModel.DisplaySeq, udtMOModel.RecordStatus, udtMOModel.UpdateBy, udtMOModel.TSMP, udtDB)
    '                Else
    '                    ' Do Nothing
    '                End If

    '            End If
    '        Next

    '        ' BankAccountStaging
    '        For Each udtBAModel As BankAcctModel In udtBAStagingModelCollection.Values
    '            If blnSuccess Then
    '                If udtBAModel.RecordStatus = Common.Component.BankAcctStagingStatus.Active Then
    '                    ' Insert
    '                    udtBAModel.RecordStatus = BankAcctStagingStatus.Existing
    '                    blnSuccess = udtBABLL.UpdateBankAcctStagingRecordStatus(udtBAModel, udtDB)
    '                End If
    '            End If
    '        Next

    '        ' Professional Staging
    '        For Each udtPModel As ProfessionalModel In udtPStagingModelCollection.Values
    '            If blnSuccess Then
    '                If udtPModel.RecordStatus = Common.Component.ProfessionalStagingStatus.Active Then
    '                    udtPModel.RecordStatus = Common.Component.ProfessionalStagingStatus.Existing
    '                    blnSuccess = udtPBLL.UpdateProfessionalStagingStatus(udtPModel.EnrolRefNo, udtPModel.ProfessionalSeq, udtPModel.RecordStatus, udtDB)
    '                End If
    '            End If
    '        Next

    '        ' -------------------------------------------------------------------------------------
    '        ' Update Activate Code & Add Mail Queue

    '        If blnEmailChanged Then

    '            If blnSuccess Then

    '                '<1> If the SP does not activate his/her account, resend the activation email to his/her new email.
    '                '<2> If the SP activates his/her account, send the confirmation email to his/her new email.

    '                Dim dtUserAcc As DataTable = New DataTable

    '                Dim blnActivated As Boolean = False

    '                ' CRE16-026 (Change email for locked SP) [Start][Winnie]
    '                'dtUserAcc = GetHCSPUserACStatus(udtSPStagingModel.SPID, udtDB)

    '                'If dtUserAcc.Rows.Count > 0 Then
    '                '    If dtUserAcc.Rows(0).Item("UserAcc_RecordStatus") = "P" Then
    '                '        blnActivated = True
    '                '    End If
    '                'End If
    '                Dim udtUserACBLL As New UserAC.UserACBLL

    '                If udtUserACBLL.IsUserACPendingActivation(udtSPStagingModel.SPID) Then
    '                    blnActivated = True
    '                End If
    '                ' CRE16-026 (Change email for locked SP) [End][Winnie]

    '                If blnActivated Then
    '                    '<1>
    '                    'Dim blnTempSuccess As Boolean = False

    '                    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()
    '                    Dim strActivationCode As String = udtGeneralFunction.generateAccountActivationCode()
    '                    Dim udtInternetMailBLL As New InternetMail.InternetMailBLL()

    '                    If blnSuccess Then
    '                        Dim udtACBLL As UserAC.UserACBLL = New UserAC.UserACBLL

    '                        ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
    '                        blnSuccess = udtACBLL.UpdateUserACActivationCode(udtSPStagingModel.SPID, Hash(strActivationCode), strUserId, udtDB)
    '                        ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]
    '                    End If

    '                    If blnSuccess Then
    '                        blnSuccess = UpdateSPEmail(udtDB, udtSPStagingModel, False, strUserId)
    '                    End If

    '                    'Send Email
    '                    Dim MSchemeCodeArrayList As ArrayList = New ArrayList

    '                    ' CRE16-018 (Display SP tentative email in HCVU) [Start][Lawrence]
    '                    For Each udtSchemeInfoModel As SchemeInformationModel In udtSchemeInfoStagingModelCollection.Values
    '                        If udtSchemeInfoModel.RecordStatus = SchemeInformationStagingStatus.Existing _
    '                                OrElse udtSchemeInfoModel.RecordStatus = SchemeInformationStagingStatus.Suspended Then
    '                            MSchemeCodeArrayList.Add(udtSchemeInfoModel.SchemeCode)
    '                        End If
    '                    Next
    '                    ' CRE16-018 (Display SP tentative email in HCVU) [End][Lawrence]

    '                    ' Get token project from [Token]
    '                    Dim strProject As String = Nothing

    '                    Dim udtToken As TokenModel = udtTokenBLL.GetTokenSerialNoProjectByUserID(udtSPStagingModel.SPID, udtDB)

    '                    ' If the token cannot be found, get from [TokenDeactivated]
    '                    If IsNothing(udtToken) Then
    '                        Dim dt As DataTable = udtTokenBLL.GetTokenDeactivatedByUserID(udtSPStagingModel.SPID, udtDB)

    '                        If dt.Rows.Count > 0 Then
    '                            'CRE14-002 - PPI-ePR Migration [Start][Chris YIM]
    '                            '-----------------------------------------------------------------------------------------
    '                            'If CStr(dt.Rows(0)("Token_Serial_No")).Trim = "******" Then
    '                            '    strProject = TokenProjectType.PPIEPR
    '                            'Else
    '                            '    strProject = TokenProjectType.EHCVS
    '                            'End If
    '                            If CStr(dt.Rows(0)("Project")).Trim = TokenProjectType.EHCVS Then
    '                                strProject = TokenProjectType.EHCVS
    '                            Else
    '                                strProject = TokenProjectType.EHR
    '                            End If

    '                            'CRE14-002 - PPI-ePR Migration [End][Chris YIM]

    '                        Else
    '                            Throw New Exception("Token not found for User ID " + udtSPStagingModel.SPID.Trim)
    '                        End If

    '                    Else
    '                        strProject = udtToken.Project
    '                    End If

    '                    'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Chris YIM]
    '                    '-----------------------------------------------------------------------------------------
    '                    'If strProject = TokenProjectType.PPIEPR Then
    '                    '    blnSuccess = udtInternetMailBLL.SubmitAccountActivationEmail(udtDB, udtSPStagingModel.SPID, strActivationCode, True, True, MSchemeCodeArrayList)
    '                    'Else
    '                    '    blnSuccess = udtInternetMailBLL.SubmitAccountActivationEmail(udtDB, udtSPStagingModel.SPID, strActivationCode, False, True, MSchemeCodeArrayList)
    '                    'End If

    '                    If strProject = TokenProjectType.EHCVS Then
    '                        blnSuccess = udtInternetMailBLL.SubmitAccountActivationEmail(udtDB, udtSPStagingModel.SPID, strActivationCode, False, True, MSchemeCodeArrayList)
    '                    Else
    '                        blnSuccess = udtInternetMailBLL.SubmitAccountActivationEmail(udtDB, udtSPStagingModel.SPID, strActivationCode, True, True, MSchemeCodeArrayList)
    '                    End If
    '                    'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Chris YIM]
    '                Else
    '                    '<2>
    '                    ' Update Activation Code to Service Provider
    '                    Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()
    '                    Dim strActivationCode As String = udtGeneralFunction.generateAccountActivationCode()

    '                    ' Update ActivationCode                        
    '                    ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
    '                    blnSuccess = udtSPBLL.UpdateServiceProviderEmailActivationCode(udtDB, udtSPStagingModel.SPID, strUserId, Hash(strActivationCode), udtSPStagingModel.TSMP, False)
    '                    ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

    '                    If blnSuccess Then
    '                        ' Add Mail Queue
    '                        Dim udtInternetMailBLL As New InternetMail.InternetMailBLL()
    '                        blnSuccess = udtInternetMailBLL.SubmitEmailAddressChangeConfirmationEmail(udtDB, udtSPStagingModel.SPID, strActivationCode)
    '                    End If
    '                End If


    '            End If
    '        End If

    '        Return blnSuccess

    '    Catch ex As Exception
    '        'udtDB.RollBackTranscation()
    '        Throw
    '    End Try

    '    Return True

    'End Function
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    Public Function AcceptVettingAndCompleteApplication(ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
    ByVal tsmpSpAccUpdate As Byte(), ByVal alEnrolledSchemeCode As ArrayList) As Boolean
        Dim udtDB As New Database
        Dim udtSPProfileBLL As SPProfileBLL = New SPProfileBLL

        Try
            udtDB.BeginTransaction()

            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Me.UpdateServiceProviderVerificaton(ServiceProviderVerificationStatus.Vetted, udtDB)
            udtSPProfileBLL.AcceptSPProfile(udtDB, strEnrolmentRefNo, strUserId, tsmpSpAccUpdate, alEnrolledSchemeCode)
            ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

            udtDB.CommitTransaction()
            Return True

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try
    End Function

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Removed, use common function [AcceptSPProfile] 
    'Public Function AcceptSPProfileFromUserB(ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
    'ByVal tsmpSpAccUpdate As Byte(), ByVal tsmpSPVerification As Byte(), ByVal alEnrolledSchemeCode As ArrayList) As Boolean
    '    ' Concurrency Validation
    '    ' UserB: Validate on [SPAccountUpdate].TSMP + [ServiceProviderVerification].TSMP

    '    Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
    '    Dim dtBAVer As DataTable = Nothing
    '    Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing

    '    Dim udtSPStagingModel As ServiceProviderModel = Nothing
    '    Dim udtPractStagingModelCollection As PracticeModelCollection = Nothing
    '    Dim udtBAStagingModelCollection As BankAcctModelCollection = Nothing
    '    Dim udtPStagingModelCollection As ProfessionalModelCollection = Nothing
    '    Dim udtMOStagingModelCollection As MedicalOrganizationModelCollection = Nothing
    '    Dim udtERNProcessedModelCollection As ERNProcessedModelCollection = Nothing

    '    Dim udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection = Nothing
    '    Dim udtExistingPracticeSchemeInfoModelCollection As PracticeSchemeInfoModelCollection = Nothing

    '    Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

    '    Dim blnSuccess As Boolean = True
    '    Try
    '        Me.udtDB.BeginTransaction()

    '        ' Retrieve Profile
    '        Me.RetrieveSPProfileForAcceptAndSetPermanentTSMP(Me.udtDB, strEnrolmentRefNo, _
    '            udtSPVerModel, dtBAVer, udtPVModelCollection, _
    '            udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
    '            udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection, udtMOStagingModelCollection, udtERNProcessedModelCollection)

    '        If Not IsNothing(udtSPStagingModel) Then
    '            If Not udtSPStagingModel.SPID.Trim.Equals(String.Empty) Then
    '                udtExistingPracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListBySPID(udtSPStagingModel.SPID, udtDB)
    '            End If
    '        End If

    '        ' Validation
    '        ' UserB: Validate on [SPAccountUpdate].TSMP + [ServiceProviderVerification].TSMP
    '        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

    '        If Not IsNothing(udtSPVerModel) Then
    '            Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPVerModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSPVerification)}
    '            udtDB.RunProc("proc_checkTSMP", params)
    '        Else
    '            Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSPVerification)}
    '            udtDB.RunProc("proc_checkTSMP", params)
    '        End If

    '        If Not IsNothing(udtSPAccUpdateModel) Then
    '            Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPAccUpdateModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
    '            udtDB.RunProc("proc_checkTSMP", params2)
    '        Else
    '            Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
    '            udtDB.RunProc("proc_checkTSMP", params2)
    '        End If

    '        ' Accept Enrolment
    '        blnSuccess = Me.AcceptSPProfileCore(Me.udtDB, strEnrolmentRefNo, strUserId, _
    '                        udtSPVerModel, dtBAVer, udtPVModelCollection, _
    '                        udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
    '                        udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection, alEnrolledSchemeCode, udtMOStagingModelCollection, _
    '                        udtERNProcessedModelCollection, udtExistingPracticeSchemeInfoModelCollection)

    '        If blnSuccess Then
    '            Me.udtDB.CommitTransaction()
    '        Else
    '            Me.udtDB.RollBackTranscation()
    '        End If
    '        Return blnSuccess

    '    Catch ex As Exception
    '        Me.udtDB.RollBackTranscation()
    '        Throw
    '        Return False
    '    End Try
    'End Function
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]

    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Remove partial accept flow (Pass to Scheme Enrolment to complete whole enrolment)
    'Public Function PartiallyAcceptSPProfileFromUserBBySchemeEnrolment(ByVal strEnrolmentRefNo As String, ByVal strUserId As String, _
    '   ByVal tsmpSpAccUpdate As Byte(), ByVal tsmpSPVerification As Byte(), ByVal alEnrolledSchemeCode As ArrayList) As Boolean
    '    ' Concurrency Validation
    '    ' UserB: Validate on [SPAccountUpdate].TSMP + [ServiceProviderVerification].TSMP

    '    Dim udtSPVerModel As ServiceProviderVerificationModel = Nothing
    '    Dim dtBAVer As DataTable = Nothing
    '    Dim udtPVModelCollection As ProfessionalVerificationModelCollection = Nothing

    '    Dim udtSPStagingModel As ServiceProviderModel = Nothing
    '    Dim udtPractStagingModelCollection As PracticeModelCollection = Nothing
    '    Dim udtBAStagingModelCollection As BankAcctModelCollection = Nothing
    '    Dim udtPStagingModelCollection As ProfessionalModelCollection = Nothing
    '    Dim udtMOStagingModelCollection As MedicalOrganizationModelCollection = Nothing
    '    Dim udtERNProcessedModelCollection As ERNProcessedModelCollection = Nothing

    '    Dim udtSchemeInfoStagingModelCollection As SchemeInformationModelCollection = Nothing

    '    Dim udtSPAccUpdateModel As SPAccountUpdateModel = Nothing

    '    Dim blnSuccess As Boolean = True
    '    Try
    '        Me.udtDB.BeginTransaction()

    '        '' Retrieve Profile
    '        'Me.RetrieveSPProfileForAcceptAndSetPermanentTSMP(Me.udtDB, strEnrolmentRefNo, _
    '        '    udtSPVerModel, dtBAVer, udtPVModelCollection, _
    '        '    udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
    '        '    udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

    '        ' Retrieve Profile
    '        Me.RetrieveSPProfileForPartialAcceptForStagingUpdate(udtDB, strEnrolmentRefNo, _
    '            udtSPVerModel, dtBAVer, udtPVModelCollection, _
    '            udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
    '            udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)

    '        ' Retrieve Permanent Model of SP (with Practice and MO) for Accept Scheme Enrolment update
    '        Dim udtSPPermanent As ServiceProviderModel
    '        udtSPPermanent = udtServiceProviderBLL.GetServiceProviderBySPID(udtDB, udtSPVerModel.SPID)

    '        ' Validation
    '        ' UserB: Validate on [SPAccountUpdate].TSMP + [ServiceProviderVerification].TSMP
    '        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction()

    '        If Not IsNothing(udtSPVerModel) Then
    '            Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPVerModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSPVerification)}
    '            udtDB.RunProc("proc_checkTSMP", params)
    '        Else
    '            Dim params() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSPVerification)}
    '            udtDB.RunProc("proc_checkTSMP", params)
    '        End If

    '        If Not IsNothing(udtSPAccUpdateModel) Then
    '            Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, udtSPAccUpdateModel.TSMP), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
    '            udtDB.RunProc("proc_checkTSMP", params2)
    '        Else
    '            Dim params2() As SqlParameter = New SqlParameter() {udtDB.MakeInParam("@tsmp_1", SqlDbType.Timestamp, 8, DBNull.Value), udtDB.MakeInParam("@tsmp_2", SqlDbType.Timestamp, 8, tsmpSpAccUpdate)}
    '            udtDB.RunProc("proc_checkTSMP", params2)
    '        End If

    '        '' Accept Enrolment
    '        'blnSuccess = Me.AcceptSPProfileCore(Me.udtDB, strEnrolmentRefNo, strUserId, _
    '        '                udtSPVerModel, dtBAVer, udtPVModelCollection, _
    '        '                udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
    '        '                udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection)
    '        ' Partially Accept Enrolment
    '        blnSuccess = Me.PartiallyAcceptSPProfileCore(udtDB, strEnrolmentRefNo, strUserId, _
    '                        udtSPVerModel, dtBAVer, udtPVModelCollection, _
    '                        udtSPStagingModel, udtPractStagingModelCollection, udtBAStagingModelCollection, udtPStagingModelCollection, _
    '                        udtSPAccUpdateModel, udtSchemeInfoStagingModelCollection, alEnrolledSchemeCode, udtSPPermanent)

    '        If blnSuccess Then
    '            Me.udtDB.CommitTransaction()
    '        Else
    '            Me.udtDB.RollBackTranscation()
    '        End If
    '        Return blnSuccess

    '    Catch ex As Exception
    '        Me.udtDB.RollBackTranscation()
    '        Throw
    '        Return False
    '    End Try
    'End Function
    ' CRE18-016 (Enable Reject at Token Scheme Management Stage) [End][Winnie]
#End Region

#Region "SP Profile In Staging"

    Public Function AddServiceProvideProfileToStaging(ByVal udtSP As ServiceProviderModel, ByVal udtSchemeInfoList As SchemeInformationModelCollection, _
                                                    ByVal udtMOList As MedicalOrganizationModelCollection, ByVal udtERNProcessedList As ERNProcessedModelCollection, _
                                                    ByVal udtPracticeList As PracticeModelCollection, ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, _
                                                    ByVal udtBankAcctList As BankAcctModelCollection, ByVal udtProfList As ProfessionalModelCollection, ByVal udtDB As Database) As Boolean
        Dim blnSuccess As Boolean = False
        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL
        Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
        Dim udtERNProcessedBLL As ERNProcessedBLL = New ERNProcessedBLL
        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtPracticeSchemeInfoBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
        Dim udtProfBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtBankBLL As BankAcctBLL = New BankAcctBLL


        Try
            'udtDB.BeginTransaction()
            '----------- 1. Add Personal Particulars -> ServiceProviderStaging ------------------------------'


            blnSuccess = udtSPBLL.AddServiceProviderParticularsToStaging(udtSP, udtDB)

            '----------- 2. Add Service Provider Scheme info -> SPSchemeInformationStaging -------------------'

            If blnSuccess Then
                If Not IsNothing(udtSchemeInfoList) Then
                    blnSuccess = udtSchemeInfoBLL.AddSchemeInfoListToStaging(udtSchemeInfoList, udtDB)
                End If
            End If

            '----------- 3 Add Medical organiztion information -> MedcialOrganizationStaging --------------------'
            If blnSuccess Then
                If Not IsNothing(udtMOList) Then
                    blnSuccess = udtMOBLL.AddMOListToStaging(udtMOList, udtDB)
                End If
            End If

            '----------- 3 Add ERN Processed based on each MO -> ERNProcessedStaging --------------------'

            If blnSuccess Then
                If Not IsNothing(udtERNProcessedList) Then
                    blnSuccess = udtERNProcessedBLL.AddERNProcessedListToStaging(udtERNProcessedList, udtDB)
                End If
            End If


            '----------- 4 Add practice information -> PracticeStaging --------------------'
            If blnSuccess Then
                If Not IsNothing(udtPracticeList) Then
                    blnSuccess = udtPracticeBLL.AddPracticeListToStaging(udtPracticeList, udtDB)
                End If
            End If

            '----------- 5 Add practice scheme information -> PracticeSchemeInformationStaging --------------------'
            If blnSuccess Then
                If Not IsNothing(udtPracticeSchemeInfoList) Then
                    blnSuccess = udtPracticeSchemeInfoBLL.AddPracticeSchemeInfoListToStaging(udtPracticeSchemeInfoList, udtDB)
                End If
            End If

            '----------- 6 Add professional information -> ProfessionalEnrolment --------------------'
            If blnSuccess Then
                If Not IsNothing(udtProfList) Then
                    blnSuccess = udtProfBLL.AddProfessionalListToStaging(udtProfList, udtDB)
                End If
            End If


            If blnSuccess Then
                If Not IsNothing(udtBankAcctList) Then
                    blnSuccess = udtBankBLL.AddBankAcctListToStaging(udtBankAcctList, udtDB)
                End If
            End If

            'If blnSuccess Then
            '    udtDB.CommitTransaction()
            'Else
            '    udtDB.RollBackTranscation()
            'End If

        Catch ex As Exception
            'udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnSuccess
    End Function

    Public Function AddServiceProvideProfileToStagingForReject(ByVal udtSP As ServiceProviderModel, ByVal udtSchemeInfoList As SchemeInformationModelCollection, _
                                                    ByVal udtMOList As MedicalOrganizationModelCollection, ByVal udtERNProcessedList As ERNProcessedModelCollection, _
                                                    ByVal udtPracticeList As PracticeModelCollection, ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, _
                                                    ByVal udtBankAcctList As BankAcctModelCollection, ByVal udtProfList As ProfessionalModelCollection, ByVal udtDB As Database) As Boolean
        Dim blnSuccess As Boolean = False
        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL
        Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
        Dim udtERNProcessedBLL As ERNProcessedBLL = New ERNProcessedBLL
        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtPracticeSchemeInfoBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
        Dim udtProfBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtBankBLL As BankAcctBLL = New BankAcctBLL


        Try
            'udtDB.BeginTransaction()
            '----------- 1. Add Personal Particulars -> ServiceProviderStaging ------------------------------'


            blnSuccess = udtSPBLL.AddServiceProviderParticularsToStagingForReject(udtSP, udtDB)

            '----------- 2. Add Service Provider Scheme info -> SPSchemeInformationStaging -------------------'

            If blnSuccess Then
                If Not IsNothing(udtSchemeInfoList) Then
                    blnSuccess = udtSchemeInfoBLL.AddSchemeInfoListToStaging(udtSchemeInfoList, udtDB)
                End If
            End If

            '----------- 3 Add Medical organiztion information -> MedcialOrganizationStaging --------------------'
            If blnSuccess Then
                If Not IsNothing(udtMOList) Then
                    blnSuccess = udtMOBLL.AddMOListToStaging(udtMOList, udtDB)
                End If
            End If

            '----------- 3 Add ERN Processed based on each MO -> ERNProcessedStaging --------------------'

            If blnSuccess Then
                If Not IsNothing(udtERNProcessedList) Then
                    blnSuccess = udtERNProcessedBLL.AddERNProcessedListToStaging(udtERNProcessedList, udtDB)
                End If
            End If


            '----------- 4 Add practice information -> PracticeStaging --------------------'
            If blnSuccess Then
                If Not IsNothing(udtPracticeList) Then
                    blnSuccess = udtPracticeBLL.AddPracticeListToStaging(udtPracticeList, udtDB)
                End If
            End If

            '----------- 5 Add practice scheme information -> PracticeSchemeInformationStaging --------------------'
            If blnSuccess Then
                If Not IsNothing(udtPracticeSchemeInfoList) Then
                    blnSuccess = udtPracticeSchemeInfoBLL.AddPracticeSchemeInfoListToStaging(udtPracticeSchemeInfoList, udtDB)
                End If
            End If

            '----------- 6 Add professional information -> ProfessionalEnrolment --------------------'
            If blnSuccess Then
                If Not IsNothing(udtProfList) Then
                    blnSuccess = udtProfBLL.AddProfessionalListToStaging(udtProfList, udtDB)
                End If
            End If


            If blnSuccess Then
                If Not IsNothing(udtBankAcctList) Then
                    blnSuccess = udtBankBLL.AddBankAcctListToStaging(udtBankAcctList, udtDB)
                End If
            End If

            'If blnSuccess Then
            '    udtDB.CommitTransaction()
            'Else
            '    udtDB.RollBackTranscation()
            'End If

        Catch ex As Exception
            'udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnSuccess
    End Function

    Public Function AddServiceProvideProfileToStagingWithinTransition(ByVal udtSP As ServiceProviderModel, ByVal udtSchemeInfoList As SchemeInformationModelCollection, _
                                                    ByVal udtMOList As MedicalOrganizationModelCollection, ByVal udtERNProcessedList As ERNProcessedModelCollection, _
                                                    ByVal udtPracticeList As PracticeModelCollection, ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, _
                                                    ByVal udtBankAcctList As BankAcctModelCollection, ByVal udtProfList As ProfessionalModelCollection, ByVal udtDB As Database) As Boolean
        Dim blnSuccess As Boolean = False
        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL
        Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
        Dim udtERNProcessedBLL As ERNProcessedBLL = New ERNProcessedBLL
        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtPracticeSchemeInfoBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
        Dim udtProfBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtBankBLL As BankAcctBLL = New BankAcctBLL


        Try
            'udtDB.BeginTransaction()
            '----------- 1. Add Personal Particulars -> ServiceProviderStaging ------------------------------'


            blnSuccess = udtSPBLL.AddServiceProviderParticularsToStaging(udtSP, udtDB)

            '----------- 2. Add Service Provider Scheme info -> SPSchemeInformationStaging -------------------'

            If blnSuccess Then
                If Not IsNothing(udtSchemeInfoList) Then
                    blnSuccess = udtSchemeInfoBLL.AddSchemeInfoListToStaging(udtSchemeInfoList, udtDB)
                End If
            End If

            '----------- 3 Add Medical organiztion information -> MedcialOrganizationStaging --------------------'
            If blnSuccess Then
                If Not IsNothing(udtMOList) Then
                    blnSuccess = udtMOBLL.AddMOListToStaging(udtMOList, udtDB)
                End If
            End If

            '----------- 3 Add ERN Processed based on each MO -> ERNProcessedStaging --------------------'

            If blnSuccess Then
                If Not IsNothing(udtERNProcessedList) Then
                    blnSuccess = udtERNProcessedBLL.AddERNProcessedListToStaging(udtERNProcessedList, udtDB)
                End If
            End If


            '----------- 4 Add practice information -> PracticeStaging --------------------'
            If blnSuccess Then
                If Not IsNothing(udtPracticeList) Then
                    blnSuccess = udtPracticeBLL.AddPracticeListToStaging(udtPracticeList, udtDB)
                End If
            End If

            '----------- 5 Add practice scheme information -> PracticeSchemeInformationStaging --------------------'
            If blnSuccess Then
                If Not IsNothing(udtPracticeSchemeInfoList) Then
                    blnSuccess = udtPracticeSchemeInfoBLL.AddPracticeSchemeInfoListToStaging(udtPracticeSchemeInfoList, udtDB)
                End If
            End If

            '----------- 6 Add professional information -> ProfessionalEnrolment --------------------'
            If blnSuccess Then
                If Not IsNothing(udtProfList) Then
                    blnSuccess = udtProfBLL.AddProfessionalListToStaging(udtProfList, udtDB)
                End If
            End If


            If blnSuccess Then
                If Not IsNothing(udtBankAcctList) Then
                    blnSuccess = udtBankBLL.AddBankAcctListToStaging(udtBankAcctList, udtDB)
                End If
            End If

            'If blnSuccess Then
            '    udtDB.CommitTransaction()
            'Else
            '    udtDB.RollBackTranscation()
            'End If

        Catch ex As Exception
            'udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnSuccess
    End Function

    Public Function SaveSessionObjectToStaging(ByVal strTableLocation As String, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False

        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL
        Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
        Dim udtERNProcessedBLL As ERNProcessedBLL = New ERNProcessedBLL
        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtPracticeSchemeInfoBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
        Dim udtProfBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtBankBLL As BankAcctBLL = New BankAcctBLL

        Dim udtExistSP As ServiceProviderModel = Nothing
        Dim udtExistSchemeInfoList As SchemeInformationModelCollection = Nothing
        Dim udtExistMOList As MedicalOrganizationModelCollection = Nothing
        Dim udtExistERNProcessedList As ERNProcessedModelCollection = Nothing
        Dim udtExistPracticeList As PracticeModelCollection = Nothing
        Dim udtExistPracticeSchemeList As PracticeSchemeInfoModelCollection = Nothing
        Dim udtExistBankAccList As BankAcctModelCollection = Nothing
        Dim udtExistProfList As ProfessionalModelCollection = Nothing

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            If udtSPBLL.Exist Then
                udtExistSP = udtSPBLL.GetSP
                udtExistSP.CreateBy = udtHCVUUser.UserID
                udtExistSP.UpdateBy = udtHCVUUser.UserID

                If udtExistSP.SPID.Equals(String.Empty) Then
                    udtExistSP.RecordStatus = ServiceProviderStagingStatus.Active
                End If

                udtExistSchemeInfoList = udtExistSP.SchemeInfoList
                For Each udtSchemeInfo As SchemeInformationModel In udtExistSchemeInfoList.Values
                    udtSchemeInfo.CreateBy = udtHCVUUser.UserID
                    udtSchemeInfo.UpdateBy = udtHCVUUser.UserID

                    If udtSchemeInfo.SPID.Equals(String.Empty) Then
                        If udtSchemeInfo.RecordStatus.Equals(String.Empty) Then
                            udtSchemeInfo.RecordStatus = SchemeInformationStagingStatus.Active
                        End If
                    Else
                        If udtSchemeInfo.RecordStatus = SchemeInformationStatus.Active Then
                            udtSchemeInfo.RecordStatus = SchemeInformationStagingStatus.Existing
                        End If
                    End If
                Next

                udtExistMOList = udtExistSP.MOList
                For Each udtMOInfo As MedicalOrganizationModel In udtExistMOList.Values
                    udtMOInfo.CreateBy = udtHCVUUser.UserID
                    udtMOInfo.UpdateBy = udtHCVUUser.UserID

                    If udtMOInfo.SPID.Equals(String.Empty) Then
                        If udtMOInfo.RecordStatus.Equals(String.Empty) Then
                            udtMOInfo.RecordStatus = MedicalOrganizationStagingStatus.Active
                        End If
                    Else
                        If udtMOInfo.RecordStatus = MedicalOrganizationStatus.Active Then
                            udtMOInfo.RecordStatus = MedicalOrganizationStagingStatus.Existing
                        End If
                    End If
                Next

                udtExistPracticeList = udtExistSP.PracticeList
                For Each udtPractice As PracticeModel In udtExistPracticeList.Values
                    udtPractice.CreateBy = udtHCVUUser.UserID
                    udtPractice.UpdateBy = udtHCVUUser.UserID

                    If udtPractice.SPID.Equals(String.Empty) Then
                        If udtPractice.RecordStatus.Equals(String.Empty) Then
                            udtPractice.RecordStatus = PracticeStagingStatus.Active
                        End If
                    Else
                        If udtPractice.RecordStatus = PracticeStatus.Active Then
                            udtPractice.RecordStatus = PracticeStagingStatus.Existing
                        End If
                    End If
                Next
            End If

            If Not IsNothing(udtExistSP.ERNProcessedList) Then
                udtExistERNProcessedList = udtExistSP.ERNProcessedList
                For Each udtERNProcessed As ERNProcessedModel In udtExistERNProcessedList.Values
                    udtERNProcessed.CreateBy = udtHCVUUser.UserID
                    'udtERNProcessed.UpdateBy = udtHCVUUser.UserID
                Next
            End If

            If udtPracticeSchemeInfoBLL.Exist Then
                udtExistPracticeSchemeList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoCollection
                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtExistPracticeSchemeList.Values
                    udtPracticeSchemeInfo.CreateBy = udtHCVUUser.UserID
                    udtPracticeSchemeInfo.UpdateBy = udtHCVUUser.UserID

                    If udtPracticeSchemeInfo.SPID.Equals(String.Empty) Then
                        If udtPracticeSchemeInfo.RecordStatus.Equals(String.Empty) Then
                            udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Active
                        End If
                    Else
                        If udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStatus.Active Then
                            udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Existing
                        ElseIf udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStatus.Delisted Then
                            udtPracticeSchemeInfo.RecordStatus = udtPracticeSchemeInfo.DelistStatus
                        ElseIf udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStatus.Suspended Then
                            udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Suspended
                        End If
                    End If
                Next
            End If

            If udtBankBLL.Exist Then
                udtExistBankAccList = udtBankBLL.GetBankAcctCollection
                For Each udtBankAcc As BankAcctModel In udtExistBankAccList.Values
                    udtBankAcc.CreateBy = udtHCVUUser.UserID
                    udtBankAcc.UpdateBy = udtHCVUUser.UserID

                    If udtBankAcc.SPID.Equals(String.Empty) Then
                        If udtBankAcc.RecordStatus.Equals(String.Empty) Then
                            udtBankAcc.RecordStatus = BankAcctStagingStatus.Active
                        End If
                    Else
                        If udtBankAcc.RecordStatus = BankAccountStatus.Active Then
                            udtBankAcc.RecordStatus = BankAcctStagingStatus.Existing
                        End If
                    End If
                Next
            End If

            If udtProfBLL.Exist Then
                udtExistProfList = udtProfBLL.GetProfessionalCollection
                For Each udtProf As ProfessionalModel In udtExistProfList.Values
                    udtProf.CreateBy = udtHCVUUser.UserID
                    'udtProf.UpdateBy = udtHCVUUser.UserID

                    If udtProf.SPID.Equals(String.Empty) Then
                        'If udtProf.RecordStatus.Equals(String.Empty) Then
                        udtProf.RecordStatus = ProfessionalStagingStatus.Active
                        'End If
                    Else
                        ' Fix the problem about handling the professional record status = 'D' (Delisted) in Permanent Table
                        If udtProf.RecordStatus = ProfessionalStatus.Active Then
                            udtProf.RecordStatus = ProfessionalStagingStatus.Existing
                        End If

                    End If
                Next
            End If

            blnRes = AddServiceProvideProfileToStaging(udtExistSP, udtExistSchemeInfoList, udtExistMOList, udtExistERNProcessedList, _
                                                    udtExistPracticeList, udtExistPracticeSchemeList, udtExistBankAccList, udtExistProfList, udtDB)

            If blnRes Then
                Select Case strTableLocation
                    Case TableLocation.Enrolment

                        ' CRE12-001 eHS and PCD integration [Start][Koala]
                        ' -----------------------------------------------------------------------------------------
                        ' Copy Enrolment table record to Original record before delete
                        blnRes = udtSPBLL.CopyServiceProviderEnrolmentProfileToOriginal(udtExistSP.EnrolRefNo, udtDB)

                        If blnRes Then
                            blnRes = udtSPBLL.DeleteServiceProviderEnrolmentProfile(udtExistSP.EnrolRefNo, udtDB)
                        End If
                        ' CRE12-001 eHS and PCD integration [End][Koala]
                    Case TableLocation.Permanent
                        udtExistSP.UnderModification = "Y"
                        blnRes = udtSPBLL.UpdateServiceProviderUnderModificationStatus(udtExistSP, udtDB)
                End Select
            End If

            'If blnRes Then
            '    Dim udtSPMigrationBLL As New SPMigrationBLL
            '    'Dim recordTSMP As Byte() = udtSPMigrationBLL.GetSPMigrationRecordTSMP("", udtExistSP.HKID, "", udtDB)
            '    Dim recordTSMP As Byte() = HttpContext.Current.Session(SPMigrationBLL.SESS_SPMigrationTSMP)
            '    blnRes = udtSPMigrationBLL.UpdateSPMigrationStatusWithDBsupplied(udtExistSP.HKID, SPMigrationStatus.Migrating, udtExistSP.EnrolRefNo, recordTSMP, udtDB)
            'End If

            If blnRes Then
                Dim udtSPVerifyBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL
                Dim udtSPVerify As ServiceProviderVerificationModel

                udtSPVerify = udtSPVerifyBLL.GetSerivceProviderVerificationByERN(udtExistSP.EnrolRefNo, udtDB)
                udtSPVerifyBLL.SaveToSession(udtSPVerify)

            End If


        Catch ex As Exception
            blnRes = False
            Throw
        End Try
        Return blnRes
    End Function


    Public Function SaveSessionObjectToStagingForReject(ByVal strTableLocation As String, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False

        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL
        Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
        Dim udtERNProcessedBLL As ERNProcessedBLL = New ERNProcessedBLL
        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtPracticeSchemeInfoBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
        Dim udtProfBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtBankBLL As BankAcctBLL = New BankAcctBLL

        Dim udtExistSP As ServiceProviderModel = Nothing
        Dim udtExistSchemeInfoList As SchemeInformationModelCollection = Nothing
        Dim udtExistMOList As MedicalOrganizationModelCollection = Nothing
        Dim udtExistERNProcessedList As ERNProcessedModelCollection = Nothing
        Dim udtExistPracticeList As PracticeModelCollection = Nothing
        Dim udtExistPracticeSchemeList As PracticeSchemeInfoModelCollection = Nothing
        Dim udtExistBankAccList As BankAcctModelCollection = Nothing
        Dim udtExistProfList As ProfessionalModelCollection = Nothing

        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim strAbortERN As String = Nothing
        Dim strOldERN As String = Nothing


        Try
            Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID.Trim

            If udtSPBLL.Exist Then
                udtExistSP = udtSPBLL.GetSP

                strOldERN = udtExistSP.EnrolRefNo.Trim

                strAbortERN = udtExistSP.EnrolRefNo.Trim.Substring(0, udtExistSP.EnrolRefNo.Trim.Length - 1) + "A"

                udtExistSP.EnrolRefNo = strAbortERN
                udtExistSP.CreateBy = strUserID
                udtExistSP.UpdateBy = strUserID

                If udtExistSP.SPID.Equals(String.Empty) Then
                    udtExistSP.RecordStatus = ServiceProviderStagingStatus.Active
                End If

                udtExistSchemeInfoList = udtExistSP.SchemeInfoList
                For Each udtScheme As SchemeInformationModel In udtExistSchemeInfoList.Values
                    udtScheme.EnrolRefNo = strAbortERN
                    udtScheme.CreateBy = strUserID
                    udtScheme.UpdateBy = strUserID

                    If udtScheme.SPID.Equals(String.Empty) Then
                        If udtScheme.RecordStatus.Equals(String.Empty) Then
                            udtScheme.RecordStatus = SchemeInformationStagingStatus.Active
                        End If
                    Else
                        If udtScheme.RecordStatus = SchemeInformationStatus.Active Then
                            udtScheme.RecordStatus = SchemeInformationStagingStatus.Existing
                        End If
                    End If
                Next

                udtExistMOList = udtExistSP.MOList
                For Each udtMO As MedicalOrganizationModel In udtExistMOList.Values
                    udtMO.EnrolRefNo = strAbortERN
                    udtMO.CreateBy = strUserID
                    udtMO.UpdateBy = strUserID

                    If udtMO.SPID.Equals(String.Empty) Then
                        If udtMO.RecordStatus.Equals(String.Empty) Then
                            udtMO.RecordStatus = MedicalOrganizationStagingStatus.Active
                        End If
                    Else
                        If udtMO.RecordStatus = MedicalOrganizationStatus.Active Then
                            udtMO.RecordStatus = MedicalOrganizationStagingStatus.Existing
                        End If
                    End If
                Next

                udtExistPracticeList = udtExistSP.PracticeList
                For Each udtPractice As PracticeModel In udtExistPracticeList.Values
                    udtPractice.EnrolRefNo = strAbortERN
                    udtPractice.CreateBy = strUserID
                    udtPractice.UpdateBy = strUserID

                    If udtPractice.SPID.Equals(String.Empty) Then
                        If udtPractice.RecordStatus.Equals(String.Empty) Then
                            udtPractice.RecordStatus = PracticeStagingStatus.Active
                        End If
                    Else
                        If udtPractice.RecordStatus = PracticeStatus.Active Then
                            udtPractice.RecordStatus = PracticeStagingStatus.Existing
                        End If
                    End If
                Next
            End If

            If Not IsNothing(udtExistSP.ERNProcessedList) Then
                udtExistERNProcessedList = udtExistSP.ERNProcessedList
                For Each udtERNProcessed As ERNProcessedModel In udtExistERNProcessedList.Values
                    udtERNProcessed.EnrolRefNo = strAbortERN
                    udtERNProcessed.CreateBy = strUserID
                    'udtERNProcessed.UpdateBy = udtHCVUUser.UserID
                Next
            End If

            If udtPracticeSchemeInfoBLL.Exist Then
                udtExistPracticeSchemeList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoCollection
                For Each udtPracticeScheme As PracticeSchemeInfoModel In udtExistPracticeSchemeList.Values
                    udtPracticeScheme.EnrolRefNo = strAbortERN
                    udtPracticeScheme.CreateBy = strUserID
                    udtPracticeScheme.UpdateBy = strUserID

                    If udtPracticeScheme.SPID.Equals(String.Empty) Then
                        If udtPracticeScheme.RecordStatus.Equals(String.Empty) Then
                            udtPracticeScheme.RecordStatus = PracticeSchemeInfoStagingStatus.Active
                        End If
                    Else
                        If udtPracticeScheme.RecordStatus = PracticeSchemeInfoStatus.Active Then
                            udtPracticeScheme.RecordStatus = PracticeSchemeInfoStagingStatus.Existing
                        ElseIf udtPracticeScheme.RecordStatus = PracticeSchemeInfoStatus.Delisted Then
                            udtPracticeScheme.RecordStatus = udtPracticeScheme.DelistStatus
                        ElseIf udtPracticeScheme.RecordStatus = PracticeSchemeInfoStatus.Suspended Then
                            udtPracticeScheme.RecordStatus = PracticeSchemeInfoStagingStatus.Suspended
                        End If
                    End If
                Next
            End If

            If udtBankBLL.Exist Then
                udtExistBankAccList = udtBankBLL.GetBankAcctCollection
                For Each udtBankAcc As BankAcctModel In udtExistBankAccList.Values
                    udtBankAcc.EnrolRefNo = strAbortERN
                    udtBankAcc.CreateBy = strUserID
                    udtBankAcc.UpdateBy = strUserID

                    If udtBankAcc.SPID.Equals(String.Empty) Then
                        If udtBankAcc.RecordStatus.Equals(String.Empty) Then
                            udtBankAcc.RecordStatus = BankAcctStagingStatus.Active
                        End If
                    Else
                        If udtBankAcc.RecordStatus = BankAccountStatus.Active Then
                            udtBankAcc.RecordStatus = BankAcctStagingStatus.Existing
                        End If
                    End If
                Next
            End If

            If udtProfBLL.Exist Then
                udtExistProfList = udtProfBLL.GetProfessionalCollection
                For Each udtProf As ProfessionalModel In udtExistProfList.Values
                    udtProf.EnrolRefNo = strAbortERN
                    udtProf.CreateBy = strUserID
                    'udtProf.UpdateBy = udtHCVUUser.UserID

                    If udtProf.SPID.Equals(String.Empty) Then
                        'If udtProf.RecordStatus.Equals(String.Empty) Then
                        udtProf.RecordStatus = ProfessionalStagingStatus.Active
                        'End If
                    Else
                        ' Fix the problem about handling the professional record status = 'D' (Delisted) in Permanent Table
                        If udtProf.RecordStatus = ProfessionalStatus.Active Then
                            udtProf.RecordStatus = ProfessionalStagingStatus.Existing
                        End If

                    End If
                Next
            End If

            blnRes = AddServiceProvideProfileToStagingForReject(udtExistSP, udtExistSchemeInfoList, udtExistMOList, udtExistERNProcessedList, _
                                                    udtExistPracticeList, udtExistPracticeSchemeList, udtExistBankAccList, udtExistProfList, udtDB)

            If blnRes Then
                Select Case strTableLocation
                    Case TableLocation.Enrolment
                        'blnRes = udtSPBLL.DeleteServiceProviderEnrolmentProfile(udtExistSP.EnrolRefNo, udtDB)
                        blnRes = udtSPBLL.DeleteServiceProviderEnrolmentProfile(strOldERN, udtDB)

                    Case TableLocation.Permanent
                        udtExistSP.UnderModification = "Y"
                        blnRes = udtSPBLL.UpdateServiceProviderUnderModificationStatus(udtExistSP, udtDB)
                End Select
            End If

            ' CRE12-001 eHS and PCD integration [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Delete original record when reject enrolment
            If blnRes Then
                blnRes = udtSPBLL.DeleteServiceProviderOriginalProfile(strOldERN, udtDB)
            End If
            ' CRE12-001 eHS and PCD integration [End][Koala]

            'If blnRes Then
            '    Dim udtSPMigrationBLL As New SPMigrationBLL
            '    'Dim recordTSMP As Byte() = udtSPMigrationBLL.GetSPMigrationRecordTSMP("", udtExistSP.HKID, "", udtDB)
            '    Dim recordTSMP As Byte() = HttpContext.Current.Session(SPMigrationBLL.SESS_SPMigrationTSMP)
            '    blnRes = udtSPMigrationBLL.UpdateSPMigrationStatusWithDBsupplied(udtExistSP.HKID, SPMigrationStatus.Migrating, udtExistSP.EnrolRefNo, recordTSMP, udtDB)
            'End If

            If blnRes Then
                Dim udtSPVerifyBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL
                Dim udtSPVerify As ServiceProviderVerificationModel

                udtSPVerify = udtSPVerifyBLL.GetSerivceProviderVerificationByERN(udtExistSP.EnrolRefNo, udtDB)
                udtSPVerifyBLL.SaveToSession(udtSPVerify)

            End If


        Catch ex As Exception
            blnRes = False
            Throw
        End Try
        Return blnRes
    End Function

    Public Function SaveSessionObjectToStagingWithinTransition(ByVal strTableLocation As String, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False

        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL
        Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
        Dim udtERNProcessedBLL As ERNProcessedBLL = New ERNProcessedBLL
        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtPracticeSchemeInfoBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
        Dim udtProfBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtBankBLL As BankAcctBLL = New BankAcctBLL

        Dim udtExistSP As ServiceProviderModel = Nothing
        Dim udtExistSchemeInfoList As SchemeInformationModelCollection = Nothing
        Dim udtExistMOList As MedicalOrganizationModelCollection = Nothing
        Dim udtExistERNProcessedList As ERNProcessedModelCollection = Nothing
        Dim udtExistPracticeList As PracticeModelCollection = Nothing
        Dim udtTmpExistPracticeSchemeList As PracticeSchemeInfoModelCollection = Nothing
        Dim udtExistPracticeSchemeList As PracticeSchemeInfoModelCollection = Nothing
        Dim udtExistBankAccList As BankAcctModelCollection = Nothing
        Dim udtExistProfList As ProfessionalModelCollection = Nothing

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            If udtSPBLL.Exist Then
                udtExistSP = udtSPBLL.GetSP
                udtExistSP.CreateBy = udtHCVUUser.UserID
                udtExistSP.UpdateBy = udtHCVUUser.UserID

                If udtExistSP.SPID.Equals(String.Empty) Then
                    udtExistSP.RecordStatus = ServiceProviderStagingStatus.Active
                End If

                udtExistSchemeInfoList = udtExistSP.SchemeInfoList
                For Each udtSchemeInfo As SchemeInformationModel In udtExistSchemeInfoList.Values
                    udtSchemeInfo.CreateBy = udtHCVUUser.UserID
                    udtSchemeInfo.UpdateBy = udtHCVUUser.UserID

                    If udtSchemeInfo.SPID.Equals(String.Empty) Then
                        If udtSchemeInfo.RecordStatus.Equals(String.Empty) Then
                            udtSchemeInfo.RecordStatus = SchemeInformationStagingStatus.Active
                        End If
                    Else
                        If udtSchemeInfo.RecordStatus = SchemeInformationStatus.Active Then
                            udtSchemeInfo.RecordStatus = SchemeInformationStagingStatus.Existing
                        End If
                    End If
                Next

                udtExistMOList = udtExistSP.MOList
                For Each udtMOInfo As MedicalOrganizationModel In udtExistMOList.Values
                    udtMOInfo.CreateBy = udtHCVUUser.UserID
                    udtMOInfo.UpdateBy = udtHCVUUser.UserID

                    If udtMOInfo.SPID.Equals(String.Empty) Then
                        If udtMOInfo.RecordStatus.Equals(String.Empty) Then
                            udtMOInfo.RecordStatus = MedicalOrganizationStagingStatus.Active
                        End If
                    Else
                        If udtMOInfo.RecordStatus = MedicalOrganizationStatus.Active Then
                            udtMOInfo.RecordStatus = MedicalOrganizationStagingStatus.Existing
                        End If
                    End If
                Next

                udtExistPracticeList = udtExistSP.PracticeList
                udtExistPracticeSchemeList = New PracticeSchemeInfoModelCollection
                udtExistBankAccList = New BankAcctModelCollection
                udtExistProfList = New ProfessionalModelCollection
                For Each udtPractice As PracticeModel In udtExistPracticeList.Values
                    udtPractice.CreateBy = udtHCVUUser.UserID
                    udtPractice.UpdateBy = udtHCVUUser.UserID

                    If udtPractice.SPID.Equals(String.Empty) Then
                        If udtPractice.RecordStatus.Equals(String.Empty) Then
                            udtPractice.RecordStatus = PracticeStagingStatus.Active
                        End If
                    Else
                        If udtPractice.RecordStatus = PracticeStatus.Active Then
                            udtPractice.RecordStatus = PracticeStagingStatus.Existing
                        End If
                    End If

                    If Not IsNothing(udtPractice.PracticeSchemeInfoList) Then
                        udtTmpExistPracticeSchemeList = udtPractice.PracticeSchemeInfoList
                        For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtTmpExistPracticeSchemeList.Values
                            udtPracticeSchemeInfo.CreateBy = udtHCVUUser.UserID
                            udtPracticeSchemeInfo.UpdateBy = udtHCVUUser.UserID

                            If udtPracticeSchemeInfo.SPID.Equals(String.Empty) Then
                                If udtPracticeSchemeInfo.RecordStatus.Equals(String.Empty) Then
                                    udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Active
                                End If
                            Else
                                If udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStatus.Active Then
                                    udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Existing
                                End If
                            End If

                            udtExistPracticeSchemeList.Add(udtPracticeSchemeInfo)
                        Next
                    End If

                    If Not IsNothing(udtPractice.BankAcct) Then
                        udtExistBankAccList.Add(udtPractice.BankAcct)

                        udtPractice.BankAcct.CreateBy = udtHCVUUser.UserID
                        udtPractice.BankAcct.UpdateBy = udtHCVUUser.UserID

                        If udtPractice.BankAcct.SPID.Equals(String.Empty) Then
                            If udtPractice.BankAcct.RecordStatus.Equals(String.Empty) Then
                                udtPractice.BankAcct.RecordStatus = BankAcctStagingStatus.Active
                            End If
                        Else
                            If udtPractice.BankAcct.RecordStatus = BankAccountStatus.Active Then
                                udtPractice.BankAcct.RecordStatus = BankAcctStagingStatus.Existing
                            End If
                        End If

                    End If

                    If Not IsNothing(udtPractice.Professional) AndAlso IsNothing(udtExistProfList(udtPractice.Professional.ProfessionalSeq)) Then
                        udtExistProfList.Add(udtPractice.Professional)

                        udtPractice.Professional.CreateBy = udtHCVUUser.UserID
                        'udtProf.UpdateBy = udtHCVUUser.UserID

                        If udtPractice.Professional.SPID.Equals(String.Empty) Then
                            'If udtProf.RecordStatus.Equals(String.Empty) Then
                            udtPractice.Professional.RecordStatus = ProfessionalStagingStatus.Active
                            'End If
                        Else
                            ' Fix the problem about handling the professional record status = 'D' (Delisted) in Permanent Table
                            If udtPractice.Professional.RecordStatus = ProfessionalStatus.Active Then
                                udtPractice.Professional.RecordStatus = ProfessionalStagingStatus.Existing
                            End If

                        End If

                    End If
                Next
            End If

            If udtERNProcessedBLL.Exist Then
                udtExistERNProcessedList = udtERNProcessedBLL.GetERNProcessedList
                For Each udtERNProcessed As ERNProcessedModel In udtExistERNProcessedList.Values
                    udtERNProcessed.CreateBy = udtHCVUUser.UserID
                    'udtERNProcessed.UpdateBy = udtHCVUUser.UserID
                Next
            End If

            'If udtBankBLL.Exist Then
            '    udtExistBankAccList = udtBankBLL.GetBankAcctCollection
            '    For Each udtBankAcc As BankAcctModel In udtExistBankAccList.Values
            '        udtBankAcc.CreateBy = udtHCVUUser.UserID
            '        udtBankAcc.UpdateBy = udtHCVUUser.UserID

            '        If udtBankAcc.SPID.Equals(String.Empty) Then
            '            If udtBankAcc.RecordStatus.Equals(String.Empty) Then
            '                udtBankAcc.RecordStatus = BankAcctStagingStatus.Active
            '            End If
            '        Else
            '            If udtBankAcc.RecordStatus = BankAcccountStatus.Active Then
            '                udtBankAcc.RecordStatus = BankAcctStagingStatus.Existing
            '            End If
            '        End If
            '    Next
            'End If

            'If udtProfBLL.Exist Then
            '    udtExistProfList = udtProfBLL.GetProfessionalCollection
            '    For Each udtProf As ProfessionalModel In udtExistProfList.Values
            '        udtProf.CreateBy = udtHCVUUser.UserID
            '        'udtProf.UpdateBy = udtHCVUUser.UserID

            '        If udtProf.SPID.Equals(String.Empty) Then
            '            'If udtProf.RecordStatus.Equals(String.Empty) Then
            '            udtProf.RecordStatus = ProfessionalStagingStatus.Active
            '            'End If
            '        Else
            '            ' If udtProf.RecordStatus = ProfessionalStatus.Active Then
            '            udtProf.RecordStatus = ProfessionalStagingStatus.Existing
            '            'End If
            '        End If
            '    Next
            'End If

            blnRes = AddServiceProvideProfileToStagingWithinTransition(udtExistSP, udtExistSchemeInfoList, udtExistMOList, udtExistERNProcessedList, _
                                                    udtExistPracticeList, udtExistPracticeSchemeList, udtExistBankAccList, udtExistProfList, udtDB)

            If blnRes Then
                Select Case strTableLocation
                    Case TableLocation.Enrolment
                        blnRes = udtSPBLL.DeleteServiceProviderEnrolmentProfile(udtExistSP.EnrolRefNo, udtDB)

                    Case TableLocation.Permanent
                        udtExistSP.UnderModification = "Y"
                        blnRes = udtSPBLL.UpdateServiceProviderUnderModificationStatus(udtExistSP, udtDB)
                End Select
            End If

            If blnRes Then
                Dim udtSPVerifyBLL As ServiceProviderVerificationBLL = New ServiceProviderVerificationBLL
                Dim udtSPVerify As ServiceProviderVerificationModel

                udtSPVerify = udtSPVerifyBLL.GetSerivceProviderVerificationByERN(udtExistSP.EnrolRefNo, udtDB)
                udtSPVerifyBLL.SaveToSession(udtSPVerify)

            End If


        Catch ex As Exception
            blnRes = False
            Throw
        End Try
        Return blnRes
    End Function

#End Region

#Region "Service Provider Particulars"

    Public Function AddServiceProviderParticularsToStaging(ByVal udtSP As ServiceProviderModel) As String
        Dim blnRes As Boolean = False
        Dim strERN As String = String.Empty

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL

        Try

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            strERN = udtGeneralFunction.generateSystemNum("A")

            udtSP.EnrolRefNo = strERN
            udtSP.SPID = String.Empty
            udtSP.CreateBy = udtHCVUUser.UserID
            udtSP.UpdateBy = udtHCVUUser.UserID
            udtSP.RecordStatus = ServiceProviderStagingStatus.Active
            udtSP.SubmitMethod = SubmitChannel.Paper

            udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' --------------------------------------------------------------------------------------------------------------------------------
            udtSP.JoinPCD = JoinPCDStatus.NA
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

            'udtSP.JoinHAPPI = JoinPPIePRStatus.NA
            udtSP.Remark = String.Empty
            udtSP.ApplicationPrinted = String.Empty
            udtSP.EmailChanged = EmailChanged.Unchanged

            udtDB.BeginTransaction()
            blnRes = Me.AddServiceProvideProfileToStaging(udtSP, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, udtDB)

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
                strERN = String.Empty
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            strERN = String.Empty
            Throw
        End Try

        Return strERN

    End Function

    Public Function UpdateServiceProviderParticularStaging(ByVal udtSP As ServiceProviderModel, ByVal strTableLocation As String)
        Dim blnRes As Boolean = False
        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtSP.UpdateBy = udtHCVUUser.UserID
            udtDB.BeginTransaction()

            'Save the session object to staging before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)

                Dim udtTempSP As ServiceProviderModel
                udtTempSP = udtSPBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, udtDB)
                udtSP.TSMP = udtTempSP.TSMP

                If udtSP.RecordStatus.Trim.Equals(String.Empty) Then
                    udtSP.RecordStatus = udtTempSP.RecordStatus
                End If
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then

                blnRes = udtSPBLL.UpdateServiceProviderStagingParticulars(udtSP, udtDB)
            End If

            If blnRes Then
                blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdateSPInfo, udtHCVUUser.UserID, udtDB)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try


        Return blnRes
    End Function

#End Region

#Region "Scheme Information"

    Public Function AddSchemeInfoToStaging(ByVal udtSchemeInfo As SchemeInformationModel, ByRef udtDB As Database) As Boolean
        Dim blnRes As Boolean = False

        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Try

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtSchemeInfo.CreateBy = udtHCVUUser.UserID
            udtSchemeInfo.UpdateBy = udtHCVUUser.UserID

            udtSchemeInfo.RecordStatus = SchemeInformationStagingStatus.Active

            blnRes = udtSchemeInfoBLL.AddSchemeInfoToStaging(udtSchemeInfo, udtDB)
        Catch ex As Exception
            Throw
        End Try

        Return blnRes
    End Function

    Public Function DeleteSchemeInfoToStaging(ByVal udtSchemeInfo As SchemeInformationModel, ByRef udtDB As Database) As Boolean
        Dim blnRes As Boolean = False

        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Try

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtSchemeInfo.CreateBy = udtHCVUUser.UserID
            udtSchemeInfo.UpdateBy = udtHCVUUser.UserID

            udtSchemeInfo.RecordStatus = SchemeInformationStagingStatus.Reject

            blnRes = udtSchemeInfoBLL.DeleteSchemeInfoStaging(udtSchemeInfo, udtDB)

        Catch ex As Exception
            Throw
        End Try

        Return blnRes
    End Function

    Public Function UpdateSchemeInfoToStaging_DelistToActive(ByVal udtSchemeInfo As SchemeInformationModel, ByRef udtDB As Database) As Boolean
        Dim blnRes As Boolean = False

        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Try

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtSchemeInfo.CreateBy = udtHCVUUser.UserID
            udtSchemeInfo.UpdateBy = udtHCVUUser.UserID

            udtSchemeInfo.RecordStatus = SchemeInformationStagingStatus.Active

            '''' Need to be change after UAT
            blnRes = udtSchemeInfoBLL.UpdateSchemeInfoStagingStatus_DelistToActive(udtSchemeInfo.EnrolRefNo, udtSchemeInfo.SchemeCode, udtSchemeInfo.UpdateBy, udtSchemeInfo.TSMP, udtDB)

        Catch ex As Exception
            Throw
        End Try

        Return blnRes
    End Function

    Public Function SchemeInfoListStagingOperation(ByVal udtSchemeInformationList As SchemeInformationModelCollection, ByVal udtDB As Database) As Boolean

        Dim blnRes As Boolean = False

        For Each udtScheme As SchemeInformationModel In udtSchemeInformationList.Values
            Select Case udtScheme.RecordStatus
                Case SchemeInformationStagingStatus.Active
                    blnRes = AddSchemeInfoToStaging(udtScheme, udtDB)
                Case SchemeInformationStagingStatus.Reject
                    blnRes = DeleteSchemeInfoToStaging(udtScheme, udtDB)
                Case String.Empty
                    blnRes = UpdateSchemeInfoToStaging_DelistToActive(udtScheme, udtDB)
            End Select
        Next

        Return blnRes
    End Function

    Private Function UpdateSchemeInfoStagingALL(ByVal strERN As String, ByVal strSPID As String, ByVal strUserID As String, ByRef udtDB As Database) As Boolean
        Dim blnRes As Boolean = False
        Dim udtSchemeInfoBLL As New SchemeInformationBLL

        blnRes = udtSchemeInfoBLL.UpdateSchemeInfoStagingStatusALL(udtDB, strERN, strSPID, strUserID)

        Return blnRes

    End Function

#End Region

#Region "Medical Organization"

    Public Function AddMedicalOrganizationToStaging(ByVal udtMO As MedicalOrganizationModel, ByVal strTableLocation As String) As Boolean
        Dim blnRes As Boolean = False

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim intMOSeq As Integer

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            If udtSPBLL.Exist Then
                intMOSeq = udtSPBLL.GetSP.MOList.GetDisplaySeq()
            Else
                intMOSeq = 1
            End If

            udtMO.DisplaySeq = intMOSeq
            udtMO.RecordStatus = MedicalOrganizationStagingStatus.Active
            udtMO.CreateBy = udtHCVUUser.UserID
            udtMO.UpdateBy = udtHCVUUser.UserID

            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then
                blnRes = udtMOBLL.AddMOToStaging(udtMO, udtDB)
            End If

            If blnRes Then
                blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdateMO, udtHCVUUser.UserID, udtDB)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnRes
    End Function

    Public Function UpdateMedicalOrganizationToStaging(ByVal udtMO As MedicalOrganizationModel, ByVal strTableLocation As String) As Boolean
        Dim blnRes As Boolean = False

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtMO.UpdateBy = udtHCVUUser.UserID

            If strTableLocation.Equals(TableLocation.Permanent) Then
                udtMO.RecordStatus = MedicalOrganizationStagingStatus.Update
            ElseIf strTableLocation.Equals(TableLocation.Staging) Then
                If udtMO.RecordStatus = MedicalOrganizationStagingStatus.Existing Then
                    udtMO.RecordStatus = MedicalOrganizationStagingStatus.Update
                End If
            End If

            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)

                Dim udtTempMOList As MedicalOrganizationModelCollection
                udtTempMOList = udtMOBLL.GetMOListFromStagingByERN(udtMO.EnrolRefNo, udtDB)

                Dim udtTempMO As MedicalOrganizationModel
                udtTempMO = udtTempMOList.Item(udtMO.DisplaySeq)

                udtMO.TSMP = udtTempMO.TSMP

                If udtMO.RecordStatus.Equals(String.Empty) Then
                    udtMO.RecordStatus = udtTempMO.RecordStatus
                End If
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then
                blnRes = udtMOBLL.UpdateMOStaging(udtMO, udtDB)
            End If

            If blnRes Then
                blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdateMO, udtHCVUUser.UserID, udtDB)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnRes

    End Function

    Public Function DeleteMedicalOraganization(ByVal udtMO As MedicalOrganizationModel, ByVal strTableLocation As String) As Boolean
        Dim blnRes As Boolean = False

        Dim blnPracticeUpdated As Boolean = False

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL
        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim udtUpdatePracticeList As PracticeModelCollection = New PracticeModelCollection

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtMO.RecordStatus = MedicalOrganizationStagingStatus.Reject
            udtMO.CreateBy = udtHCVUUser.UserID
            udtMO.UpdateBy = udtHCVUUser.UserID

            ' Re order MO Display Seq in Practice
            Dim udtPracticeList As PracticeModelCollection


            If udtSPBLL.Exist Then
                udtPracticeList = udtSPBLL.GetSP.PracticeList

                If Not IsNothing(udtPracticeList) Then
                    If udtPracticeList.Count > 0 Then
                        For Each udtPractice As PracticeModel In udtPracticeList.Values
                            If udtPractice.MODisplaySeq = udtMO.DisplaySeq.Value Then
                                Dim udtUpdatePractice As PracticeModel = New PracticeModel
                                udtPracticeBLL.Clone(udtUpdatePractice, udtPractice)

                                udtUpdatePractice.MODisplaySeq = 0

                                udtUpdatePracticeList.Add(udtUpdatePractice)

                            ElseIf udtPractice.MODisplaySeq >= udtMO.DisplaySeq.Value Then
                                Dim udtUpdatePractice As PracticeModel = New PracticeModel
                                udtPracticeBLL.Clone(udtUpdatePractice, udtPractice)

                                udtUpdatePractice.MODisplaySeq = udtUpdatePractice.MODisplaySeq - 1

                                udtUpdatePracticeList.Add(udtUpdatePractice)

                            End If

                        Next
                    End If

                End If
            End If


            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)

                ' INT18-0006 (Fix deletion on Data Entry for processing e-enrolment) [Start][Chris YIM]
                ' --------------------------------------------------------------------------------------
                '-------------------------
                'Medical Organization
                '-------------------------
                Dim udtTempMOList As MedicalOrganizationModelCollection
                udtTempMOList = udtMOBLL.GetMOListFromStagingByERN(udtMO.EnrolRefNo, udtDB)

                Dim udtTempMO As MedicalOrganizationModel
                udtTempMO = udtTempMOList.Item(udtMO.DisplaySeq)

                udtMO.TSMP = udtTempMO.TSMP

                '-------------------------
                'Practice
                '-------------------------
                Dim udtTempPracticeList As PracticeModelCollection
                udtTempPracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtMO.EnrolRefNo, udtDB)
                For Each udtPractice As PracticeModel In udtUpdatePracticeList.Values
                    udtPractice.TSMP = udtTempPracticeList.Item(udtPractice.DisplaySeq).TSMP
                Next
                ' INT18-0006 (Fix deletion on Data Entry for processing e-enrolment) [End][Chris YIM]

            End If

            blnRes = udtMOBLL.DeleteMOStaging(udtMO, udtDB)

            If blnRes Then
                If Not IsNothing(udtUpdatePracticeList) Then
                    If udtUpdatePracticeList.Count > 0 Then
                        blnRes = udtPracticeBLL.UpdatePracticeStagingMODisplaySeq(udtUpdatePracticeList, udtDB)
                        blnPracticeUpdated = True
                    End If
                End If
            End If


            If blnRes Then
                If blnPracticeUpdated Then
                    blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdateMOPractice, udtHCVUUser.UserID, udtDB)
                Else
                    blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdateMO, udtHCVUUser.UserID, udtDB)
                End If

            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnRes

    End Function

    Public Function DeleteMedicalOraganizationPermanentWithinTransition(ByVal strSPID As String, ByVal udtDB As Database) As Boolean
        Dim blnRes As Boolean = False

        Try
            Dim prams() As SqlParameter = { _
                                          udtDB.MakeInParam("@SP_ID", ServiceProviderModel.SPIDDataType, ServiceProviderModel.SPIDDataSize, strSPID)}

            udtDB.RunProc("proc_MedicalOrganization_del_All", prams)
            blnRes = True
        Catch ex As Exception
            Throw
        End Try

        Return blnRes
    End Function

    Public Function EmptyMOCollection() As MedicalOrganizationModelCollection
        Dim udtMOModel As MedicalOrganizationModel
        udtMOModel = New MedicalOrganizationModel(String.Empty, String.Empty, 0, String.Empty, String.Empty, Nothing, String.Empty, String.Empty, _
                                                 String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, Nothing, String.Empty, Nothing, _
                                                 String.Empty, Nothing)
        Dim udtMOList As MedicalOrganizationModelCollection = New MedicalOrganizationModelCollection
        udtMOList.Add(udtMOModel)

        Return udtMOList

    End Function

#End Region

#Region "Practice"

    Public Function AddPracticeProfessionalToStaging(ByVal udtPractice As PracticeModel, ByVal strServiceCategoryCode As String, ByVal strRegCode As String, _
                                                 ByVal strTableLocation As String) As String
        Dim blnRes As Boolean = False


        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtProfessionalBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtPracticeSchemeBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL

        Dim udtProfModel As ProfessionalModel = Nothing
        Dim udtProfList As ProfessionalModelCollection = Nothing
        Dim udtPracticeScheme As PracticeSchemeInfoModel = Nothing

        Dim intPracticeSeq As Integer
        Dim intProfSeq As Integer

        Dim blnNeedAddProf As Boolean = False
        Dim blnNeedUpdateEHRSS As Boolean = False

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            Dim udtSchemeBOList As SchemeBackOfficeModelCollection = Nothing
            Dim udtSubsidizeGpBOList As SubsidizeGroupBackOfficeModelCollection = Nothing

            If udtSchemeBackOfficeBLL.ExistSession_SchemeBackOfficeWithSubsidizeGroup Then
                udtSchemeBOList = udtSchemeBackOfficeBLL.GetSession_SchemeBackOfficeWithSubsidizeGroup
            End If

            If udtSchemeBackOfficeBLL.ExistSession_SubsidizeGroupBackOffice Then
                udtSubsidizeGpBOList = udtSchemeBackOfficeBLL.GetSession_SubsidizeGroupBackOffice
            End If

            If udtPracticeBLL.Exist Then
                intPracticeSeq = udtPracticeBLL.GetPracticeCollection.GetPracticeSeq()
            Else
                intPracticeSeq = 1
            End If

            If udtProfessionalBLL.Exist Then
                intProfSeq = udtProfessionalBLL.GetProfessionalCollection.GetProfessionalSeq(strServiceCategoryCode, strRegCode)

                If IsNothing(udtProfessionalBLL.GetProfessionalCollection.Item(intProfSeq)) Then
                    udtProfModel = New ProfessionalModel(udtPractice.SPID, udtPractice.EnrolRefNo, intProfSeq, strServiceCategoryCode, strRegCode, ProfessionalStagingStatus.Active, Nothing, udtHCVUUser.UserID)

                    blnNeedAddProf = True
                End If
            End If

            udtPractice.DisplaySeq = intPracticeSeq
            udtPractice.ProfessionalSeq = intProfSeq
            udtPractice.RecordStatus = PracticeStagingStatus.Active
            udtPractice.CreateBy = udtHCVUUser.UserID
            udtPractice.UpdateBy = udtHCVUUser.UserID

            udtPractice.Remark = String.Empty
            udtPractice.SubmitMethod = SubmitChannel.Paper

            'If blnNeedAddProf Then
            '    If Not AskJoinIVSS(udtProfModel.ServiceCategoryCode.Trim) Then
            '        'udtPracticeScheme = New PracticeSchemeInfoModel(udtPractice.SPID, udtPractice.EnrolRefNo, udtPractice.DisplaySeq, SchemeCode.EHCVS, _
            '        '                                                Nothing, PracticeSchemeInfoStagingStatus.NewAdd, String.Empty, String.Empty, _
            '        '                                                Nothing, udtHCVUUser.UserID, Nothing, udtHCVUUser.UserID, Nothing, Nothing, Nothing)
            '    End If
            'Else
            '    Dim udtTempProf As ProfessionalModel
            '    If udtProfessionalBLL.Exist Then
            '        If udtProfessionalBLL.GetProfessionalCollection.Count > 0 Then
            '            udtTempProf = udtProfessionalBLL.GetProfessionalCollection.Item(intProfSeq)
            '            If Not AskJoinIVSS(udtTempProf.ServiceCategoryCode.Trim) Then
            '                'udtPracticeScheme = New PracticeSchemeInfoModel(udtPractice.SPID, udtPractice.EnrolRefNo, udtPractice.DisplaySeq, SchemeCode.EHCVS, _
            '                '                                               Nothing, PracticeSchemeInfoStagingStatus.NewAdd, String.Empty, String.Empty, _
            '                '                                                Nothing, udtHCVUUser.UserID, Nothing, udtHCVUUser.UserID, Nothing, Nothing, Nothing)
            '            End If
            '        End If

            '    End If

            'End If


            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then
                blnRes = udtPracticeBLL.AddPracticeToStaging(udtPractice, udtDB)
            End If

            'If blnRes Then
            '    If Not IsNothing(udtPracticeScheme) Then
            '        blnRes = udtPracticeSchemeBLL.AddPracticeSchemeInfoToStaging(udtPracticeScheme, udtDB)
            '    End If
            'End If

            If blnRes AndAlso blnNeedAddProf Then
                blnRes = udtProfessionalBLL.AddProfessionalToStaging(udtProfModel, udtDB)
            End If

            If blnRes Then
                udtProfList = udtProfessionalBLL.GetProfessinalListFromStagingByERN(udtPractice.EnrolRefNo, udtDB)

                Dim udtSP As ServiceProviderModel

                Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL

                'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]                
                blnNeedUpdateEHRSS = NeedUpdateEHRSSStatus(udtProfList)

                If udtSPBLL.Exist Then

                    udtSP = udtSPBLL.GetSP

                    If blnNeedUpdateEHRSS Then
                        udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
                        udtSP.UpdateBy = udtHCVUUser.UserID

                        blnRes = udtSPBLL.UpdateServiceProviderStagingEHRSSStatus(udtSP, udtDB)
                    End If

                End If
                'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]

            End If

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' --------------------------------------------------------------------------------------------------------------------------------
            If blnRes Then
                blnRes = HandleServiceProviderJoinPCDStatus(udtPractice, udtHCVUUser)
            End If
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

            If blnRes Then
                blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdatePracticeBank, udtHCVUUser.UserID, udtDB)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnRes

    End Function

    Public Function UpdatePracticeProfessionalInStaging(ByVal udtPractice As PracticeModel, ByVal strServiceCategoryCode As String, ByVal strRegCode As String, _
                                                     ByVal strTableLocation As String) As Boolean

        Dim blnRes As Boolean = False


        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtProfessionalBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtMOBLL As MedicalOrganizationBLL = New MedicalOrganizationBLL

        Dim udtProfModel As ProfessionalModel = Nothing
        Dim udtProfList As ProfessionalModelCollection = Nothing

        Dim udtPracticeList As PracticeModelCollection = Nothing
        Dim udtMOList As MedicalOrganizationModelCollection = Nothing

        Dim intPracticeSeq As Integer
        Dim intProfSeq As Integer

        Dim blnNeedAddProf As Boolean = False
        Dim blnNeedUpdateEHRSS As Boolean = False
        Dim blnEditMO As Boolean = False

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser


            If strTableLocation.Equals(TableLocation.Permanent) Then
                udtPractice.RecordStatus = PracticeStagingStatus.Update
            ElseIf strTableLocation.Equals(TableLocation.Staging) Or strTableLocation.Equals(TableLocation.Enrolment) Then
                'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If udtPractice.RecordStatus.Equals(PracticeStagingStatus.Existing) Then
                If udtPractice.RecordStatus.Equals(PracticeStagingStatus.Existing) Or _
                    udtPractice.RecordStatus.Equals(PracticeStagingStatus.Suspended) Then
                    'INT14-0022 - Fix issue of updating permanent table from staging for suspended practice [End][Chris YIM]
                    udtPractice.RecordStatus = PracticeStagingStatus.Update
                End If

                If udtPractice.RecordStatus.Equals(PracticeStagingStatus.Active) Then
                    If udtProfessionalBLL.Exist Then
                        intProfSeq = udtProfessionalBLL.GetProfessionalCollection.GetProfessionalSeq(strServiceCategoryCode.Trim, strRegCode.Trim)

                        If IsNothing(udtProfessionalBLL.GetProfessionalCollection.Item(intProfSeq)) Then
                            udtProfModel = New ProfessionalModel(udtPractice.SPID, udtPractice.EnrolRefNo, intProfSeq, strServiceCategoryCode, strRegCode, ProfessionalStagingStatus.Active, Nothing, udtHCVUUser.UserID)

                            blnNeedAddProf = True
                        End If
                    End If
                    udtPractice.ProfessionalSeq = intProfSeq
                End If
            End If

            udtPractice.CreateBy = udtHCVUUser.UserID
            udtPractice.UpdateBy = udtHCVUUser.UserID

            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)

                Dim udtTempPracticeList As PracticeModelCollection
                udtTempPracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtPractice.EnrolRefNo, udtDB)

                Dim udtTempPractice As PracticeModel
                udtTempPractice = udtTempPracticeList.Item(udtPractice.DisplaySeq)

                udtPractice.TSMP = udtTempPractice.TSMP

                If udtPractice.RecordStatus.Trim.Equals(String.Empty) Then
                    udtPractice.RecordStatus = udtTempPractice.RecordStatus
                End If
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then
                blnRes = udtPracticeBLL.UpdatePracticeStaging(udtPractice, udtDB)
            End If

            If blnRes Then
                blnRes = udtProfessionalBLL.DeleteProfessionalStaging(udtPractice.EnrolRefNo, ProfessionalStagingStatus.Delete, udtDB)
            End If

            If blnRes AndAlso blnNeedAddProf Then
                blnRes = udtProfessionalBLL.AddProfessionalToStaging(udtProfModel, udtDB)
            End If

            If blnRes Then
                udtProfList = udtProfessionalBLL.GetProfessinalListFromStagingByERN(udtPractice.EnrolRefNo, udtDB)

                Dim udtSP As ServiceProviderModel

                Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL

                'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]                
                blnNeedUpdateEHRSS = NeedUpdateEHRSSStatus(udtProfList)

                If udtSPBLL.Exist Then

                    udtSP = udtSPBLL.GetSP

                    If blnNeedUpdateEHRSS Then
                        udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
                        udtSP.UpdateBy = udtHCVUUser.UserID

                        blnRes = udtSPBLL.UpdateServiceProviderStagingEHRSSStatus(udtSP, udtDB)
                    End If

                End If
                'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]
            End If

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' --------------------------------------------------------------------------------------------------------------------------------
            If blnRes Then
                blnRes = HandleServiceProviderJoinPCDStatus(udtPractice, udtHCVUUser)
            End If
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

            If blnRes Then
                udtPracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtPractice.EnrolRefNo, udtDB)
                udtMOList = udtMOBLL.GetMOListFromStagingByERN(udtPractice.EnrolRefNo, udtDB)

                For Each udtMO As MedicalOrganizationModel In udtMOList.Values

                    If Not IsNothing(udtPracticeList) Then
                        If Not IsNothing(udtPracticeList.FilterByMO(udtMO.DisplaySeq.Value)) Then
                            If udtPracticeList.FilterByMO(udtMO.DisplaySeq.Value).Count = 0 Then
                                blnEditMO = True
                            End If
                        Else
                            blnEditMO = True
                        End If
                    End If
                Next

                If blnEditMO Then
                    blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdateMOPractice, udtHCVUUser.UserID, udtDB)
                Else
                    blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdatePractice, udtHCVUUser.UserID, udtDB)
                End If


            End If


            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnRes

    End Function

    Public Function DeletePracticeBankAcct(ByVal udtPractice As PracticeModel, ByVal strTableLocation As String) As Boolean
        Dim blnRes As Boolean = False


        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL
        Dim udtProfessionalBLL As ProfessionalBLL = New ProfessionalBLL
        Dim udtBankAccBLL As BankAcctBLL = New BankAcctBLL
        Dim udtPracticeSchemeInfoBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
        Dim udtSchemeInfoBLL As SchemeInformationBLL = New SchemeInformationBLL

        Dim udtSP As ServiceProviderModel = New ServiceProviderModel
        Dim udtProfList As ProfessionalModelCollection = New ProfessionalModelCollection
        Dim udtProcessSchemeList As SchemeInformationModelCollection = New SchemeInformationModelCollection

        Dim udtSchemeList As SchemeInformationModelCollection = Nothing

        Dim blnNeedAddProf As Boolean = False
        Dim blnNeedUpdateEHRSS As Boolean = False

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtPractice.RecordStatus = PracticeStagingStatus.Reject
            udtPractice.CreateBy = udtHCVUUser.UserID
            udtPractice.UpdateBy = udtHCVUUser.UserID

            'Update Practice Scheme Info Record Status -> 'R' (Reject)
            If Not IsNothing(udtPractice.PracticeSchemeInfoList) Then
                For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    udtPracticeScheme.RecordStatus = PracticeSchemeInfoStagingStatus.Reject
                Next
            End If

            If udtSPBLL.Exist Then
                udtSchemeList = udtSPBLL.GetSP.SchemeInfoList
            End If

            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            ' INT18-0006 (Fix deletion on Data Entry for processing e-enrolment) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                'Save session and copy detail from enrolment to staging if no record is in staging.
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)

                'Refresh data to get the latest one from DB staging tables
                '-----------------
                'Practice
                '-----------------
                Dim udtTempPracticeList As PracticeModelCollection
                udtTempPracticeList = udtPracticeBLL.GetPracticeBankAcctListFromStagingByERN(udtPractice.EnrolRefNo, udtDB)

                Dim udtTempPractice As PracticeModel
                udtTempPractice = udtTempPracticeList.Item(udtPractice.DisplaySeq)

                udtPractice.TSMP = udtTempPractice.TSMP

                '---------------------
                'Practice Scheme Info
                '---------------------
                Dim udtTempPracticeSchemeInfoList As PracticeSchemeInfoModelCollection
                udtTempPracticeSchemeInfoList = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoListStagingByERN(udtPractice.EnrolRefNo, udtDB)

                For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    udtPracticeSchemeInfo.TSMP = udtTempPracticeSchemeInfoList.Filter(udtPracticeSchemeInfo.SchemeCode.Trim, udtPracticeSchemeInfo.SubsidizeCode.Trim).TSMP
                Next

                '---------------------
                'Bank Account
                '---------------------
                Dim udtTempBankAcctList As BankAcctModelCollection
                udtTempBankAcctList = udtBankAccBLL.GetBankAcctListFromStagingByERN(udtPractice.EnrolRefNo, udtDB)

                Dim udtBankAcct As BankAcctModel = udtPractice.BankAcct
                udtBankAcct.TSMP = udtTempBankAcctList.Item(udtBankAcct.SpPracticeDisplaySeq, udtBankAcct.DisplaySeq).TSMP

            End If
            ' INT18-0006 (Fix deletion on Data Entry for processing e-enrolment) [End][Chris YIM]

            'Delete Practice Scheme Information

            If Not IsNothing(udtPractice.PracticeSchemeInfoList) Then
                blnRes = udtPracticeSchemeInfoBLL.DeletePracticeSchemeInfoListStagingByERNDisplaySeq(udtPractice.PracticeSchemeInfoList, udtDB)
            Else
                blnRes = True
            End If

            'Delete Practice
            If blnRes Then
                blnRes = udtPracticeBLL.DeletePracticeStaging(udtPractice, udtDB)
            End If

            'Delete Professional
            If blnRes Then
                blnRes = udtProfessionalBLL.DeleteProfessionalStaging(udtPractice.EnrolRefNo, ProfessionalStagingStatus.Delete, udtDB)
            End If

            'Delete Bank Account
            If blnRes Then
                If Not IsNothing(udtPractice.BankAcct) Then
                    If Not udtPractice.BankAcct.BankName.Trim.Equals(String.Empty) Then
                        blnRes = udtBankAccBLL.DeleteBankAccountStaging(udtPractice.BankAcct, udtDB)
                    End If
                End If

            End If

            ' Check Scheme Information
            If blnRes Then
                'Dim dtSelectedSchemeCodeFormPractice As New DataTable
                'dtSelectedSchemeCodeFormPractice = udtPracticeSchemeInfoBLL.GetPracticeSchemeInfoStagingSchemeCode(udtPractice.EnrolRefNo, udtDB)

                'If Not IsNothing(dtSelectedSchemeCodeFormPractice) Then
                '    If Not IsNothing(udtSchemeList) Then

                '        For Each udtScheme As SchemeInformationModel In udtSchemeList.Values
                '            Dim dv As DataView = New DataView(dtSelectedSchemeCodeFormPractice)

                '            dv.RowFilter = "Scheme_Code = '" & udtScheme.SchemeCode.Trim & "'"

                '            If dv.Count = 0 Then
                '                If udtScheme.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Active) Then
                '                    udtScheme.RecordStatus = SchemeInformationStagingStatus.Reject
                '                    udtProcessSchemeList.Add(udtScheme)
                '                End If
                '            End If
                '        Next
                '    End If
                'End If

                'If udtProcessSchemeList.Count > 0 Then
                '    blnRes = SchemeInfoListStagingOperation(udtProcessSchemeList, udtDB)
                'End If

                blnRes = Me.UpdateSchemeInfoStagingALL(udtPractice.EnrolRefNo, udtPractice.SPID, udtHCVUUser.UserID, udtDB)

            End If

            'Update eHRSS
            If blnRes Then
                udtProfList = udtProfessionalBLL.GetProfessinalListFromStagingByERN(udtPractice.EnrolRefNo, udtDB)

                'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]                
                blnNeedUpdateEHRSS = NeedUpdateEHRSSStatus(udtProfList)

                If udtSPBLL.Exist Then

                    udtSP = udtSPBLL.GetSP

                    If blnNeedUpdateEHRSS Then
                        udtSP.AlreadyJoinEHR = JoinEHRSSStatus.NA
                        udtSP.UpdateBy = udtHCVUUser.UserID

                        blnRes = udtSPBLL.UpdateServiceProviderStagingEHRSSStatus(udtSP, udtDB)
                    End If

                End If
                'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]

            End If

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' --------------------------------------------------------------------------------------------------------------------------------
            If blnRes Then
                blnRes = HandleServiceProviderJoinPCDStatus(udtPractice, udtHCVUUser)
            End If
            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

            If blnRes Then
                blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdatePractice, udtHCVUUser.UserID, udtDB)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnRes

    End Function

    Public Function EmptyPracticeCollection() As PracticeModelCollection
        Dim udtPracticeModel As PracticeModel
        ' CRE16-022 (Add optional field "Remarks") [Start][Winnie]
        udtPracticeModel = New PracticeModel(String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, Nothing, 0, String.Empty, String.Empty, _
                                            String.Empty, String.Empty, Nothing, String.Empty, Nothing, String.Empty, Nothing, _
                                            YesNo.No, String.Empty, String.Empty, _
                                            Nothing, Nothing, Nothing)
        ' CRE16-022 (Add optional field "Remarks") [End][Winnie]

        Dim udtPracticeModelCollection As PracticeModelCollection = New PracticeModelCollection
        udtPracticeModelCollection.Add(udtPracticeModel)

        Return udtPracticeModelCollection

    End Function

#End Region

#Region "Practice Scheme Info"

    Public Function AddPracticeSchemeInfoToStaging(ByVal udtPracticeSchemeInfo As PracticeSchemeInfoModel, ByVal strTableLocation As String) As Boolean
        Dim blnRes As Boolean = False

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtPracticeSchemeBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtPracticeSchemeInfo.RecordStatus = PracticeStagingStatus.Active
            udtPracticeSchemeInfo.CreateBy = udtHCVUUser.UserID
            udtPracticeSchemeInfo.UpdateBy = udtHCVUUser.UserID

            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)

                Dim udtTempPracticeSchemeList As PracticeSchemeInfoModelCollection
                udtTempPracticeSchemeList = udtPracticeSchemeBLL.GetPracticeSchemeInfoListStagingByERN(udtPracticeSchemeInfo.EnrolRefNo, udtDB)

                Dim udtTempPracticeScheme As PracticeSchemeInfoModel
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                ' Add SchemeCode to key
                udtTempPracticeScheme = udtTempPracticeSchemeList.Item(udtPracticeSchemeInfo.PracticeDisplaySeq, udtPracticeSchemeInfo.SubsidizeCode, udtPracticeSchemeInfo.SchemeDisplaySeq, udtPracticeSchemeInfo.SubsidizeDisplaySeq, udtPracticeSchemeInfo.SchemeCode)
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                udtPracticeSchemeInfo.TSMP = udtTempPracticeScheme.TSMP
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then
                blnRes = udtPracticeSchemeBLL.AddPracticeSchemeInfoToStaging(udtPracticeSchemeInfo, udtDB)
            End If

            If blnRes Then
                blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdateScheme, udtHCVUUser.UserID, udtDB)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else

                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            blnRes = False
            Throw
        End Try

        Return blnRes

    End Function

    Public Function AddPracticeSchemeInfoToStaging(ByVal udtPracticeSchemeInfo As PracticeSchemeInfoModel, ByRef udtDB As Database) As Boolean
        Dim blnRes As Boolean = False


        Dim udtPracticeSchemeBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Try

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtPracticeSchemeInfo.CreateBy = udtHCVUUser.UserID
            udtPracticeSchemeInfo.UpdateBy = udtHCVUUser.UserID
            udtPracticeSchemeInfo.DelistStatus = String.Empty
            udtPracticeSchemeInfo.Remark = String.Empty


            blnRes = udtPracticeSchemeBLL.AddPracticeSchemeInfoToStaging(udtPracticeSchemeInfo, udtDB)

        Catch ex As Exception

            blnRes = False
            Throw
        End Try

        Return blnRes

    End Function

    Public Function UpdatePracticeSchemeInfoInStaging(ByVal udtPracticeSchemeInfo As PracticeSchemeInfoModel, ByVal strTableLocation As String) As Boolean

        Dim blnRes As Boolean = False

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtPracticeSchemeBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtPracticeSchemeInfo.UpdateBy = udtHCVUUser.UserID

            If udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Existing Then
                udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Update
            End If

            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)

                Dim udtTempPracticeSchemeList As PracticeSchemeInfoModelCollection
                udtTempPracticeSchemeList = udtPracticeSchemeBLL.GetPracticeSchemeInfoListStagingByERN(udtPracticeSchemeInfo.EnrolRefNo, udtDB)

                Dim udtTempPracticeScheme As PracticeSchemeInfoModel
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                ' Add SchemeCode to key
                udtTempPracticeScheme = udtTempPracticeSchemeList.Item(udtPracticeSchemeInfo.PracticeDisplaySeq, udtPracticeSchemeInfo.SubsidizeCode, udtPracticeSchemeInfo.SchemeDisplaySeq, udtPracticeSchemeInfo.SubsidizeDisplaySeq, udtPracticeSchemeInfo.SchemeCode)
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                udtPracticeSchemeInfo.TSMP = udtTempPracticeScheme.TSMP
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then
                blnRes = udtPracticeSchemeBLL.UpdatePracticeSchemeInfoStaging(udtPracticeSchemeInfo, udtDB)
            End If

            If blnRes Then
                blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdateScheme, udtHCVUUser.UserID, udtDB)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnRes

    End Function

    Public Function UpdatePracticeSchemeInfoInStaging(ByVal udtPracticeSchemeInfo As PracticeSchemeInfoModel, ByRef udtDB As Database) As Boolean

        Dim blnRes As Boolean = False

        Dim udtPracticeSchemeBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Try

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtPracticeSchemeInfo.UpdateBy = udtHCVUUser.UserID

            blnRes = udtPracticeSchemeBLL.UpdatePracticeSchemeInfoStaging(udtPracticeSchemeInfo, udtDB)

        Catch ex As Exception
            Throw
        End Try

        Return blnRes

    End Function

    Public Function UpdatePracticeSchemeInfoStaging_DelistToActive(ByVal udtPracticeSchemeInfo As PracticeSchemeInfoModel, ByRef udtDB As Database) As Boolean
        Dim blnRes As Boolean = False

        Dim udtPracticeSchemeBll As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Try

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtPracticeSchemeInfo.UpdateBy = udtHCVUUser.UserID
            udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Active

            blnRes = udtPracticeSchemeBll.UpdatePracticeSchemeInfoStaging_DelistToActive(udtPracticeSchemeInfo, udtDB)

        Catch ex As Exception
            Throw
        End Try

        Return blnRes
    End Function

    Public Function DeletePracticeSchemeInfoInStaging(ByVal udtPracticeSchemeInfo As PracticeSchemeInfoModel, ByVal strTableLocation As String) As Boolean
        Dim blnRes As Boolean = False

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtPracticeSchemeBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtPracticeSchemeInfo.UpdateBy = udtHCVUUser.UserID


            udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Reject


            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)

                Dim udtTempPracticeSchemeList As PracticeSchemeInfoModelCollection
                udtTempPracticeSchemeList = udtPracticeSchemeBLL.GetPracticeSchemeInfoListStagingByERN(udtPracticeSchemeInfo.EnrolRefNo, udtDB)

                Dim udtTempPracticeScheme As PracticeSchemeInfoModel
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                ' Add SchemeCode to key
                udtTempPracticeScheme = udtTempPracticeSchemeList.Item(udtPracticeSchemeInfo.PracticeDisplaySeq, udtPracticeSchemeInfo.SubsidizeCode, udtPracticeSchemeInfo.SchemeDisplaySeq, udtPracticeSchemeInfo.SubsidizeDisplaySeq, udtPracticeSchemeInfo.SchemeCode)
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                udtPracticeSchemeInfo.TSMP = udtTempPracticeScheme.TSMP
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then
                blnRes = udtPracticeSchemeBLL.UpdatePracticeSchemeInfoStaging(udtPracticeSchemeInfo, udtDB)
            End If

            If blnRes Then
                blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdateScheme, udtHCVUUser.UserID, udtDB)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnRes
    End Function

    Public Function DeletePracticeSchemeInfoInStaging(ByVal udtPracticeSchemeInfo As PracticeSchemeInfoModel, ByRef udtDB As Database) As Boolean
        Dim blnRes As Boolean = False

        Dim udtPracticeSchemeBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL


        Try

            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtPracticeSchemeInfo.UpdateBy = udtHCVUUser.UserID

            blnRes = udtPracticeSchemeBLL.DeletePracticeSchemeInfoStagingByERNDisplaySeq(udtPracticeSchemeInfo, udtDB)

        Catch ex As Exception
            Throw
        End Try

        Return blnRes
    End Function


    Public Function PracticeSchemeInfoListStagingOperation(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, ByVal strTableLocation As String) As Boolean
        Dim blnRes As Boolean = False

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtPracticeSchemeBLL As PracticeSchemeInfoBLL = New PracticeSchemeInfoBLL
        Dim udtPracticeBLL As PracticeBLL = New PracticeBLL

        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim intPracticeDisplaySeq As Nullable(Of Integer) = Nothing
        Dim strERN As String = String.Empty
        Dim strSPID As String = String.Empty

        Dim udtSchemeList As SchemeInformationModelCollection = Nothing
        Dim udtProcessSchemeList As SchemeInformationModelCollection = New SchemeInformationModelCollection

        Dim strReactiveScheme As New List(Of String)

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            If udtSPBLL.Exist Then
                udtSchemeList = udtSPBLL.GetSP.SchemeInfoList
                strERN = udtSPBLL.GetSP.EnrolRefNo
                strSPID = udtSPBLL.GetSP.SPID
            End If

            If IsNothing(udtSchemeList) Then
                udtSchemeList = New SchemeInformationModelCollection
            End If

            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                If Not strTableLocation.Equals(TableLocation.Staging) Then
                    udtPracticeSchemeInfo.CreateBy = udtHCVUUser.UserID
                End If
                udtPracticeSchemeInfo.UpdateBy = udtHCVUUser.UserID

                If Not intPracticeDisplaySeq.HasValue Then
                    intPracticeDisplaySeq = udtPracticeSchemeInfo.PracticeDisplaySeq
                End If

                'If Not strERN.Trim.Equals(String.Empty) Then
                '    strERN = udtPracticeSchemeInfo.EnrolRefNo
                'End If
            Next

            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)

                ' Reload the SP Schemes to get timestamp
                Dim udtTempSchemeList As SchemeInformationModelCollection = Nothing
                udtTempSchemeList = udtSchemeInformationBLL.GetSchemeInfoListStaging(strERN, udtDB)

                Dim udtTempScheme As SchemeInformationModel = Nothing

                For Each udtScheme As SchemeInformationModel In udtSchemeList.Values
                    udtTempScheme = udtTempSchemeList.Item(udtScheme.SchemeCode, udtScheme.SchemeDisplaySeq)

                    If Not IsNothing(udtTempScheme) Then
                        udtScheme.TSMP = udtTempScheme.TSMP
                    End If
                Next

                ' Reload the Practice Schemes to get timestamp
                Dim udtTempPracticeSchemeList As PracticeSchemeInfoModelCollection = Nothing
                udtTempPracticeSchemeList = udtPracticeSchemeBLL.GetPracticeSchemeInfoListStagingByERN(strERN, udtDB)

                Dim udtTempPracticeScheme As PracticeSchemeInfoModel = Nothing

                For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                    ' Add SchemeCode to key
                    udtTempPracticeScheme = udtTempPracticeSchemeList.Item(udtPracticeScheme.PracticeDisplaySeq, udtPracticeScheme.SubsidizeCode, udtPracticeScheme.SchemeDisplaySeq, udtPracticeScheme.SubsidizeDisplaySeq, udtPracticeScheme.SchemeCode)
                    ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                    If Not IsNothing(udtTempPracticeScheme) Then
                        udtPracticeScheme.TSMP = udtTempPracticeScheme.TSMP
                    End If
                Next
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then

                For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                    Select Case udtPracticeScheme.RecordStatus
                        Case PracticeSchemeInfoStagingStatus.Active
                            blnRes = AddPracticeSchemeInfoToStaging(udtPracticeScheme, udtDB)
                        Case PracticeSchemeInfoStagingStatus.Update
                            blnRes = UpdatePracticeSchemeInfoInStaging(udtPracticeScheme, udtDB)
                        Case PracticeSchemeInfoStagingStatus.Reject
                            ' Special Handling logic is built in Stored Procedure
                            blnRes = DeletePracticeSchemeInfoInStaging(udtPracticeScheme, udtDB)
                        Case PracticeSchemeInfoStagingStatus.DelistedVoluntary, PracticeSchemeInfoStagingStatus.DelistedInvoluntary
                            blnRes = UpdatePracticeSchemeInfoStaging_DelistToActive(udtPracticeScheme, udtDB)
                            strReactiveScheme.Add(udtPracticeScheme.SchemeCode)
                    End Select
                Next

            End If

            If blnRes Then
                ' Contsturt the scheme information based on practice scheme infomation
                blnRes = UpdateSchemeInfoStagingALL(strERN, strSPID, udtHCVUUser.UserID, udtDB)
            End If

            'If blnRes Then
            '    Dim dtSelectedSchemeCodeFormPractice As New DataTable
            '    dtSelectedSchemeCodeFormPractice = udtPracticeSchemeBLL.GetPracticeSchemeInfoStagingSchemeCode(strERN, udtDB)

            '    If Not IsNothing(dtSelectedSchemeCodeFormPractice) Then
            '        If IsNothing(udtSchemeList) Then
            '            For Each dr As DataRow In dtSelectedSchemeCodeFormPractice.Rows
            '                If IsNothing(udtProcessSchemeList.FilterBySchemeCode(CStr(dr.Item("Scheme_Code")).Trim)) OrElse _
            '                    udtProcessSchemeList.FilterBySchemeCode(CStr(dr.Item("Scheme_Code")).Trim).Count = 0 Then

            '                    udtProcessSchemeList.Add(New SchemeInformationModel(strERN, strSPID, CStr(dr.Item("Scheme_Code")).Trim, SchemeInformationStagingStatus.Active, _
            '                                                                        String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, udtHCVUUser.UserID, _
            '                                                                        Nothing, udtHCVUUser.UserID, Nothing, Nothing))

            '                End If

            '            Next
            '        Else
            '            For Each dr As DataRow In dtSelectedSchemeCodeFormPractice.Rows
            '                If udtSchemeList.FilterBySchemeCode(CStr(dr.Item("Scheme_Code")).Trim).Count = 0 Then
            '                    If IsNothing(udtProcessSchemeList.FilterBySchemeCode(CStr(dr.Item("Scheme_Code")).Trim)) OrElse _
            '                         udtProcessSchemeList.FilterBySchemeCode(CStr(dr.Item("Scheme_Code")).Trim).Count = 0 Then
            '                        udtProcessSchemeList.Add(New SchemeInformationModel(strERN, strSPID, CStr(dr.Item("Scheme_Code")).Trim, SchemeInformationStagingStatus.Active, _
            '                                                                            String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, udtHCVUUser.UserID, _
            '                                                                            Nothing, udtHCVUUser.UserID, Nothing, Nothing))
            '                    End If

            '                Else
            '                    Dim udtSchemeInfo As SchemeInformationModel
            '                    udtSchemeInfo = udtSchemeList.Filter(CStr(dr.Item("Scheme_Code")).Trim)
            '                    If Not IsNothing(udtSchemeInfo) Then
            '                        If udtSchemeInfo.RecordStatus = SchemeInformationStagingStatus.DelistedVoluntary OrElse udtSchemeInfo.RecordStatus = SchemeInformationStagingStatus.DelistedInvoluntary Then
            '                            For Each str As String In strReactiveScheme
            '                                If str.Trim.Equals(CStr(dr.Item("Scheme_Code")).Trim) Then
            '                                    If IsNothing(udtProcessSchemeList.FilterBySchemeCode(CStr(dr.Item("Scheme_Code")).Trim)) OrElse _
            '                                        udtProcessSchemeList.FilterBySchemeCode(CStr(dr.Item("Scheme_Code")).Trim).Count = 0 Then
            '                                        udtProcessSchemeList.Add(New SchemeInformationModel(strERN, strSPID, CStr(dr.Item("Scheme_Code")).Trim, String.Empty, _
            '                                                                 String.Empty, String.Empty, Nothing, Nothing, Nothing, Nothing, udtHCVUUser.UserID, _
            '                                                                 Nothing, udtHCVUUser.UserID, udtSchemeInfo.TSMP, Nothing))
            '                                    End If

            '                                End If
            '                            Next
            '                        End If
            '                    End If
            '                End If
            '            Next

            '            For Each udtScheme As SchemeInformationModel In udtSchemeList.Values
            '                Dim dv As DataView = New DataView(dtSelectedSchemeCodeFormPractice)

            '                dv.RowFilter = "Scheme_Code = '" & udtScheme.SchemeCode.Trim & "'"

            '                If dv.Count = 0 Then
            '                    If udtScheme.RecordStatus.Trim.Equals(SchemeInformationStagingStatus.Active) Then
            '                        udtScheme.RecordStatus = SchemeInformationStagingStatus.Reject
            '                        udtProcessSchemeList.Add(udtScheme)
            '                    End If

            '                End If
            '            Next
            '        End If
            '    End If

            '    For Each udtScheme As SchemeInformationModel In udtProcessSchemeList.Values
            '        Select Case udtScheme.RecordStatus
            '            Case SchemeInformationStagingStatus.Active
            '                blnRes = AddSchemeInfoToStaging(udtScheme, udtDB)
            '            Case SchemeInformationStagingStatus.Reject
            '                blnRes = DeleteSchemeInfoToStaging(udtScheme, udtDB)
            '            Case String.Empty
            '                blnRes = UpdateSchemeInfoToStaging_DelistToActive(udtScheme, udtDB)
            '        End Select
            '    Next

            'End If

            If blnRes Then
                blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdateScheme, udtHCVUUser.UserID, udtDB)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else

                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            blnRes = False
            Throw
        End Try

        Return blnRes
    End Function

    Public Function GetPracticeSchemeInfoStatus(ByVal udtPractice As PracticeModel, ByVal strSchemeCode As String, ByVal strTableLocation As String) As String
        Dim strRes As String = String.Empty

        Dim udtPracticeSchemeInfoListBySchemeCode As PracticeSchemeInfoModelCollection
        udtPracticeSchemeInfoListBySchemeCode = udtPractice.PracticeSchemeInfoList.Filter(strSchemeCode)

        ' CRE15-004 TIV & QIV [Start][Winnie]
        If IsNothing(udtPracticeSchemeInfoListBySchemeCode) Then Return String.Empty
        ' CRE15-004 TIV & QIV [Emd][Winnie]

        Select Case strTableLocation
            Case TableLocation.Enrolment
                strRes = "Unprocessed"

            Case TableLocation.Staging

                'Dim udtSchemeInfoBLL As New SchemeInformationBLL
                'Dim udtSchemeList As SchemeInformationModelCollection
                'udtSchemeList = udtSchemeInfoBLL.GetSchemeInfoListStaging(udtPractice.EnrolRefNo, udtDB)

                'Dim udtSchemeInfo As SchemeInformationModel
                'udtSchemeInfo = udtSchemeList.Filter(strSchemeCode)

                Select Case udtPractice.RecordStatus.Trim
                    Case PracticeStagingStatus.Active
                        strRes = PracticeSchemeInfoStagingStatus.Active

                    Case PracticeStagingStatus.Delisted

                        Dim udtPracticeScheme As PracticeSchemeInfoModel

                        udtPracticeScheme = udtPracticeSchemeInfoListBySchemeCode.GetByIndex(0)


                        'strRes = udtPracticeScheme.DelistStatus
                        strRes = udtPracticeScheme.RecordStatus

                    Case PracticeStagingStatus.Existing, PracticeStagingStatus.Update, PracticeStagingStatus.Suspended


                        'CRE15-004 TIV & QIV [Start][Winnie]
                        ''If there is any subsidy does not match with others within same scheme, Change status to Update

                        'Dim blnUpdate As Boolean = False
                        'Dim blnAdd As Boolean = False
                        Dim strTempStatus As String = String.Empty

                        For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPracticeSchemeInfoListBySchemeCode.Values
                            'Select Case udtPracticeScheme.RecordStatus
                            '    Case PracticeSchemeInfoStagingStatus.Update
                            '        blnUpdate = True
                            '    Case PracticeSchemeInfoStagingStatus.Active
                            '        blnAdd = True
                            '    Case Else
                            '        strTempStatus = udtPracticeScheme.RecordStatus
                            'End Select
                            If strTempStatus <> String.Empty Then
                                If strTempStatus <> udtPracticeScheme.RecordStatus Then
                                    strTempStatus = PracticeSchemeInfoStagingStatus.Update
                                    Exit For
                                End If
                            End If
                            strTempStatus = udtPracticeScheme.RecordStatus
                        Next

                        strRes = strTempStatus

                        'If blnUpdate And blnAdd Then
                        '    strRes = PracticeSchemeInfoStagingStatus.Update
                        'ElseIf Not blnUpdate And blnAdd Then
                        '    strRes = PracticeSchemeInfoStagingStatus.Active
                        'ElseIf blnUpdate And Not blnAdd Then
                        '    strRes = PracticeSchemeInfoStagingStatus.Update
                        'Else
                        '    strRes = strTempStatus
                        'End If
                        'CRE15-004 TIV & QIV [End][Winnie]

                End Select

            Case TableLocation.Permanent
                Dim udtPracticeScheme As PracticeSchemeInfoModel

                udtPracticeScheme = udtPracticeSchemeInfoListBySchemeCode.GetByIndex(0)

                Select Case udtPracticeScheme.RecordStatus
                    Case PracticeSchemeInfoStatus.Delisted
                        strRes = udtPracticeScheme.DelistStatus
                    Case Else
                        strRes = udtPracticeScheme.RecordStatus
                End Select

        End Select

        Return strRes
    End Function

    'CRE15-004 TIV & QIV [Start][Winnie]
    'Combine login for Effective Time and Delisting Time

    'Public Function GetPracticeSchemeInfoEarliestEffectiveTime(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, ByVal strSchemeCode As String) As String
    '    Dim dtmTargetTime As DateTime = Date.Now
    '    Dim blnAdjusted As Boolean = False

    '    For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
    '        If udtPracticeScheme.SchemeCode.Trim <> strSchemeCode.Trim Then Continue For

    '        If Not IsNothing(udtPracticeScheme.EffectiveDtm) AndAlso udtPracticeScheme.EffectiveDtm < dtmTargetTime Then
    '            dtmTargetTime = udtPracticeScheme.EffectiveDtm
    '            blnAdjusted = True
    '        End If
    '    Next

    '    If blnAdjusted Then
    '        Return udtFormatter.convertDateTime(dtmTargetTime)
    '    Else
    '        Return String.Empty
    '    End If

    'End Function

    Public Function GetPracticeSchemeInfoEarliestTime(ByVal udtPracticeSchemeInfoList As PracticeSchemeInfoModelCollection, ByVal strSchemeCode As String, ByRef strEffectiveTime As String, ByRef strDelistTime As String) As String
        Dim dtmTargetEffectiveTime As DateTime = Date.Now
        Dim dtmTargetDelistTime As DateTime = Date.Now

        Dim blnAdjustedEffectiveTime As Boolean = False
        Dim blnAdjustedDelistTime As Boolean = False

        If Not IsNothing(udtPracticeSchemeInfoList) Then
            For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPracticeSchemeInfoList.Values
                If udtPracticeScheme.SchemeCode.Trim <> strSchemeCode.Trim Then Continue For

                If Not IsNothing(udtPracticeScheme.EffectiveDtm) AndAlso udtPracticeScheme.EffectiveDtm < dtmTargetEffectiveTime Then
                    dtmTargetEffectiveTime = udtPracticeScheme.EffectiveDtm
                    blnAdjustedEffectiveTime = True
                End If

                If Not IsNothing(udtPracticeScheme.DelistDtm) AndAlso udtPracticeScheme.DelistDtm < dtmTargetDelistTime Then
                    dtmTargetDelistTime = udtPracticeScheme.DelistDtm
                    blnAdjustedDelistTime = True
                End If
            Next
        End If

        If blnAdjustedEffectiveTime Then
            strEffectiveTime = udtFormatter.convertDateTime(dtmTargetEffectiveTime)
        Else
            strEffectiveTime = String.Empty
        End If

        If blnAdjustedDelistTime Then
            strDelistTime = udtFormatter.convertDateTime(dtmTargetDelistTime)
        Else
            strDelistTime = String.Empty
        End If

    End Function
    'CRE15-004 TIV & QIV [End][Winnie]
#End Region

#Region "Bank Account"

    Public Function AddBankAcctToStaging(ByVal udtBankAcct As BankAcctModel, ByVal strTableLocation As String) As Boolean

        Dim blnRes As Boolean = False

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtBankAcctBLL As BankAcctBLL = New BankAcctBLL

        Dim intBankDisplaySeq As Integer

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            intBankDisplaySeq = udtBankAcctBLL.GetBankAcctCollection.GetDisplaySeq(udtBankAcct.SpPracticeDisplaySeq.Value)
            udtBankAcct.DisplaySeq = intBankDisplaySeq
            udtBankAcct.CreateBy = udtHCVUUser.UserID
            udtBankAcct.UpdateBy = udtHCVUUser.UserID
            udtBankAcct.RecordStatus = BankAcctStagingStatus.Active

            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then
                blnRes = udtBankAcctBLL.AddBankAcctToStaging(udtBankAcct, udtDB)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnRes

    End Function

    Public Function UpdateBankAcctInStaging(ByVal udtBankAcct As BankAcctModel, ByVal strTableLocation As String) As Boolean
        Dim blnRes As Boolean = False

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Dim udtBankAcctBLL As BankAcctBLL = New BankAcctBLL

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtBankAcct.UpdateBy = udtHCVUUser.UserID

            udtDB.BeginTransaction()

            'Save the session object to stagin before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)

                Dim udtTempBankList As BankAcctModelCollection
                udtTempBankList = udtBankAcctBLL.GetBankAcctListFromStagingByERN(udtBankAcct.EnrolRefNo, udtDB)

                Dim udtTempBank As BankAcctModel
                udtTempBank = udtTempBankList.Item(udtBankAcct.SpPracticeDisplaySeq, udtBankAcct.DisplaySeq)

                udtBankAcct.TSMP = udtTempBank.TSMP

                If udtBankAcct.RecordStatus.Trim.Equals(String.Empty) Then
                    udtBankAcct.RecordStatus = udtTempBank.RecordStatus
                End If
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then
                blnRes = udtBankAcctBLL.UpdateBankAcctStaging(udtBankAcct, udtDB)
            End If

            If blnRes Then
                blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdateBank, udtHCVUUser.UserID, udtDB)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try

        Return blnRes
    End Function

#End Region

#Region "Structure Address"

    Public Function SearchStructureAddress(ByVal strBuilding As String, ByVal strAreaCode As String, ByVal strDistrictCode As String) As DataTable
        Dim udtAddressBLL As AddressBLL = New AddressBLL

        Dim strDistrict As String = String.Empty
        Dim strAddressCode As String = String.Empty

        strDistrict = strDistrictCode

        If Not strAreaCode.Equals(String.Empty) Then
            strAddressCode = udtAddressBLL.GetAddCharCode(Integer.Parse(strAreaCode))
        Else
            strAddressCode = udtAddressBLL.GetAddCharCode(0)
        End If

        Dim dt As DataTable = New DataTable

        dt = udtAddressBLL.GetStructureAddress(strBuilding, strAddressCode, strDistrict)
        Return dt
    End Function

    Public Function GetMaxNoOfRowRetrieve() As Integer
        Dim i As Integer = 0

        Dim value1 As String = String.Empty
        udtGeneralFunction.getSystemParameter("MaxRowRetrieveStructureAddress", value1, String.Empty)

        If Not value1.Equals(String.Empty) Then
            i = CInt(value1.Trim)
        Else
            i = 100
        End If

        Return i
    End Function

    Public Function GetMinNoOfWordToSearch() As String
        Dim i As Integer = 0

        Dim strRes As String = String.Empty
        udtGeneralFunction.getSystemParameter("MinWordForSearchAddress", strRes, String.Empty)

        Return strRes
    End Function

    Public Function GetResidentialAddressFrom() As Integer
        Dim str As String = String.Empty
        Dim i As Integer
        udtGeneralFunction.getSystemParameter("ResidentialAddressFrom", str, String.Empty)

        i = CInt(str)

        Return i
    End Function

    Public Function GetResidentialAddressTo() As Integer
        Dim str As String = String.Empty
        Dim i As Integer
        udtGeneralFunction.getSystemParameter("ResidentialAddressTo", str, String.Empty)

        i = CInt(str)

        Return i
    End Function

    Public Function GetBusinessAddressFrom() As Integer
        Dim str As String = String.Empty
        Dim i As Integer
        udtGeneralFunction.getSystemParameter("BusinessAddressFrom", str, String.Empty)

        i = CInt(str)

        Return i
    End Function

    Public Function GetBusinessAddressTo() As Integer
        Dim str As String = String.Empty
        Dim i As Integer
        udtGeneralFunction.getSystemParameter("BusinessAddressTo", str, String.Empty)

        i = CInt(str)

        Return i
    End Function

#End Region

#Region "EHRSS"

    Public Function UpdatePracticeOtherInfo(ByVal udtSP As ServiceProviderModel, ByVal strTableLocation As String) As Boolean
        Dim blnRes As Boolean = False
        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim udtHCVUUser As HCVUUserModel
        Dim udtHCVUUserBLL As New HCVUUserBLL

        Try
            udtHCVUUser = udtHCVUUserBLL.GetHCVUUser

            udtSP.UpdateBy = udtHCVUUser.UserID
            udtDB.BeginTransaction()

            'Save the session object to staging before amendment
            If Not strTableLocation.Equals(TableLocation.Staging) Then
                blnRes = SaveSessionObjectToStaging(strTableLocation, udtDB)

                Dim udtTempSP As ServiceProviderModel
                udtTempSP = udtSPBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, udtDB)
                udtSP.TSMP = udtTempSP.TSMP

                If udtSP.RecordStatus.Trim.Equals(String.Empty) Then
                    udtSP.RecordStatus = udtTempSP.RecordStatus
                End If
            End If

            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then
                blnRes = udtSPBLL.UpdateServiceProviderStagingEHRSSStatus(udtSP, udtDB)
            End If

            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
            ' --------------------------------------------------------------------------------------------------------------------------------
            If strTableLocation.Equals(TableLocation.Staging) OrElse blnRes Then
                ' Get again the timestamp for updating the Join PCD value
                Dim udtTempSP As ServiceProviderModel
                udtTempSP = udtSPBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, udtDB)
                If udtSP.JoinPCD <> udtTempSP.JoinPCD Then
                    ' Update DB if Join PCD value changed
                    udtSP.TSMP = udtTempSP.TSMP
                    blnRes = udtSPBLL.UpdateServiceProviderStagingJoinPCD(udtSP, udtDB)
                Else
                    blnRes = True
                End If
            End If

            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

            If blnRes Then
                blnRes = UpdateSPVerifyAndAcctUpdSPInfo(UpdatePractice, udtHCVUUser.UserID, udtDB)
            End If

            If blnRes Then
                udtDB.CommitTransaction()
            Else
                udtDB.RollBackTranscation()
            End If

        Catch ex As Exception
            udtDB.RollBackTranscation()
            Throw
        End Try


        Return blnRes
    End Function

    Public Function IsOtherProfCanJoinEHRSS(ByVal intEditDisplaySeq As Integer) As Boolean
        Dim blnRes As Boolean = False
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim udtPracticeList As PracticeModelCollection

        If udtServiceProviderBLL.Exist Then
            udtPracticeList = udtServiceProviderBLL.GetSP.PracticeList
            If udtPracticeList.Count > 0 Then
                For Each udtPracticeModel As PracticeModel In udtPracticeList.Values
                    If udtPracticeModel.DisplaySeq <> intEditDisplaySeq Then
                        If AskHadJoinedEHRSSProfCode(udtPracticeModel.Professional.ServiceCategoryCode) Then

                            blnRes = True
                            ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
                            ' --------------------------------------------------------------------------------------------------------------------------------
                            Exit For
                            ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]
                        End If
                    End If
                Next
            End If
        End If

        Return blnRes

    End Function

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
    ' --------------------------------------------------------------------------------------------------------------------------------
    Public Function IsOtherProfCanJoinPCD(ByVal intEditDisplaySeq As Integer) As Boolean
        Dim blnRes As Boolean = False
        Dim udtServiceProviderBLL As ServiceProviderBLL = New ServiceProviderBLL

        Dim udtPracticeList As PracticeModelCollection

        If udtServiceProviderBLL.Exist Then
            udtPracticeList = udtServiceProviderBLL.GetSP.PracticeList
            If udtPracticeList.Count > 0 Then
                For Each udtPracticeModel As PracticeModel In udtPracticeList.Values
                    If udtPracticeModel.DisplaySeq <> intEditDisplaySeq Then
                        If udtPracticeModel.Professional.Profession.AllowJoinPCD Then
                            blnRes = True
                            Exit For
                        End If
                    End If
                Next
            End If
        End If

        Return blnRes
    End Function
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

    Public Function AlreadyJoinEHRSS(ByVal udtProfList As ProfessionalModelCollection) As Boolean
        Dim blnRes As Boolean = False
        For Each udtProf As ProfessionalModel In udtProfList.Values
            If AskHadJoinedEHRSSProfCode(udtProf.ServiceCategoryCode) Then
                blnRes = True
            End If
        Next

        Return blnRes
    End Function

    'Public Function JoinPPIePR(ByVal udtProfList As ProfessionalModelCollection) As Boolean
    '    Dim blnRes As Boolean = False

    '    For Each udtProf As ProfessionalModel In udtProfList.Values
    '        If AskWillJoinPPIePRProfCode(udtProf.ServiceCategoryCode) Then
    '            blnRes = True
    '        End If
    '    Next

    '    Return blnRes
    'End Function

    Public Function HadJoinedEHRSS(ByVal udtPracticeList As PracticeModelCollection) As Boolean
        Dim blnRes As Boolean = False

        ' Checking to Avoid [Professional = nothing] For New Practice using Existing Professional
        For Each udtPractice As PracticeModel In udtPracticeList.Values
            If Not udtPractice.Professional Is Nothing AndAlso Not udtPractice.Professional.ServiceCategoryCode Is Nothing Then
                If AskHadJoinedEHRSSProfCode(udtPractice.Professional.ServiceCategoryCode) Then
                    blnRes = True
                End If
            End If
        Next
        Return blnRes
    End Function
    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
    ' --------------------------------------------------------------------------------------------------------------------------------
    Public Function CanJoinPCD(ByVal udtPracticeList As PracticeModelCollection) As Boolean
        Dim blnRes As Boolean = False

        ' Checking to Avoid [Professional = nothing] For New Practice using Existing Professional
        For Each udtPractice As PracticeModel In udtPracticeList.Values
            If Not udtPractice.Professional Is Nothing AndAlso Not udtPractice.Professional.ServiceCategoryCode Is Nothing Then
                If udtPractice.Professional.Profession.AllowJoinPCD Then
                    blnRes = True
                    Exit For
                End If
            End If
        Next
        Return blnRes
    End Function
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]


    'Public Function WillJoinPPIePR(ByVal udtPracticeList As PracticeModelCollection) As Boolean
    '    Dim blnRes As Boolean = False

    '    ' Checking to Avoid [Professional = nothing] For New Practice using Existing Professional
    '    For Each udtPractice As PracticeModel In udtPracticeList.Values
    '        If Not udtPractice.Professional Is Nothing AndAlso Not udtPractice.Professional.ServiceCategoryCode Is Nothing Then
    '            If AskWillJoinPPIePRProfCode(udtPractice.Professional.ServiceCategoryCode) Then
    '                blnRes = True
    '            End If
    '        End If
    '    Next
    '    Return blnRes
    'End Function
#End Region

    Public Sub HCVSRecordDataMigration(ByVal strHKID As String, ByVal strERN As String, ByVal strSPID As String, ByVal strUserID As String, ByVal udtPracticeModelCollection As PracticeModelCollection, ByVal udtDatabase As Database)
        'Dim udtDatabase As New Database
        'Try
        '    udtDatabase.BeginTransaction()
        '1a. Update Practice in staging
        UpdatePracticeTransitionDetailsToStaging(strHKID, strUserID, udtPracticeModelCollection, udtDatabase)

        '1b. Update Medical Organization in staging
        UpdateMODetailsToStagingForMigration(strHKID, strERN, strSPID, strUserID, udtDatabase)

        'Update MO's SPID 


        '1c. Update SPMigration Record Status
        'TODO

        '    udtDatabase.CommitTransaction()
        'Catch eSQL As SqlException
        '    udtDatabase.RollBackTranscation()
        '    Throw eSQL
        'Catch ex As Exception
        '    udtDatabase.RollBackTranscation()
        '    Throw ex
        'End Try
    End Sub

    Private Sub UpdatePracticeTransitionDetailsToStaging(ByVal strHKID As String, ByVal strUserID As String, ByVal udtPracticeModelCollection As PracticeModelCollection, ByVal udtDB As Database)
        Dim udtPracticeBLL As New PracticeBLL
        Dim udtPracticeModel As PracticeModel

        Dim intAddressCode As Nullable(Of Integer)
        Dim intMODisplaySeq As Nullable(Of Integer)

        Try
            If Not IsNothing(udtPracticeModelCollection) Then
                For Each udtPracticeModel In udtPracticeModelCollection.Values

                    'Do not migrate the practice if the Record_status is Delisted
                    If Not udtPracticeModel.RecordStatus.Trim.Equals(PracticeStatus.Delisted) Then
                        Dim dtRaw As New DataTable

                        Dim prams() As SqlParameter = { _
                                                    udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID), _
                                                    udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq)}
                        udtDB.RunProc("proc_PracticeTransition_get", prams, dtRaw)

                        For Each drPracticeList As DataRow In dtRaw.Rows
                            If IsDBNull(drPracticeList.Item("Address_Code")) Then
                                intAddressCode = Nothing
                            Else
                                intAddressCode = CInt((drPracticeList.Item("Address_Code")))
                            End If

                            If IsDBNull(drPracticeList.Item("MO_Display_Seq")) Then
                                intMODisplaySeq = Nothing
                            Else
                                intMODisplaySeq = CInt(drPracticeList.Item("MO_Display_Seq"))
                            End If

                            'Update the existing Practice Model with the new values
                            With udtPracticeModel
                                .PracticeNameChi = CStr(IIf((drPracticeList.Item("Practice_Name_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Name_Chi"))).Trim
                                .PracticeAddress = New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))).Trim, _
                                                                        CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))).Trim, _
                                                                        CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))).Trim, _
                                                                        CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))).Trim, _
                                                                        CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))).Trim, _
                                                                        CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))).Trim, _
                                                                        intAddressCode)
                                .PhoneDaytime = CStr(IIf((drPracticeList.Item("Phone_Daytime") Is DBNull.Value), String.Empty, drPracticeList.Item("Phone_Daytime"))).Trim
                                .MODisplaySeq = intMODisplaySeq

                                .UpdateBy = strUserID
                                .UpdateDtm = Now
                                '.RecordStatus = PracticeStagingStatus.Update
                            End With
                            If Not IsNothing(udtPracticeModel.SPID) AndAlso Not udtPracticeModel.SPID.Trim.Equals(String.Empty) AndAlso _
                                    udtPracticeModel.RecordStatus <> PracticeStagingStatus.Active Then
                                udtPracticeModel.RecordStatus = PracticeStagingStatus.Update
                            End If

                            'Update TMSP in order to proceed


                            udtPracticeBLL.UpdatePracticeStaging(udtPracticeModel, udtDB)
                        Next
                    End If
                Next
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub UpdateMODetailsToStaging(ByVal strHKID As String, ByVal strERN As String, ByVal strUserID As String, ByVal udtDB As Database, Optional ByVal bByPassDeleteMO As Boolean = False)
        Dim udtMOBLL As New MedicalOrganizationBLL

        If Not bByPassDeleteMO Then udtMOBLL.DeleteAllMOStaging(strERN, udtDB)

        Dim udtMedicalOrganizationModelCollection As MedicalOrganizationModelCollection = New MedicalOrganizationModelCollection
        Dim udtMedicalOrganizationModel As MedicalOrganizationModel

        Dim intDisplaySeq As Nullable(Of Integer)
        Dim intPracticeDisplaySeq As Nullable(Of Integer) = Nothing
        Dim intAddressCode As Nullable(Of Integer)

        Dim dtRaw As New DataTable()
        Try
            Dim prams() As SqlParameter = { _
                                        udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID)}
            udtDB.RunProc("proc_MOTransition_get", prams, dtRaw)

            For i As Integer = 0 To dtRaw.Rows.Count - 1
                Dim drRaw As DataRow = dtRaw.Rows(i)

                If IsDBNull(drRaw.Item("Address_Code")) Then
                    intAddressCode = Nothing
                Else
                    intAddressCode = CInt((drRaw.Item("Address_Code")))
                End If

                If IsDBNull(drRaw.Item("Display_Seq")) Then
                    intDisplaySeq = Nothing
                Else
                    intDisplaySeq = CInt(drRaw.Item("Display_Seq"))
                End If

                udtMedicalOrganizationModel = New MedicalOrganizationModel(strERN.Trim, _
                                                                            CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                                            intDisplaySeq, _
                                                                            CType(drRaw.Item("MO_Eng_Name"), String).Trim, _
                                                                            CStr(IIf((drRaw.Item("MO_Chi_Name") Is DBNull.Value), String.Empty, drRaw.Item("MO_Chi_Name"))).Trim, _
                                                                            New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))).Trim, _
                                                                                        intAddressCode), _
                                                                            CStr(IIf(drRaw.Item("BR_Code") Is DBNull.Value, String.Empty, drRaw.Item("BR_Code"))).Trim, _
                                                                            CStr(IIf(drRaw.Item("Phone_Daytime") Is DBNull.Value, String.Empty, drRaw.Item("Phone_Daytime"))).Trim, _
                                                                            CStr(IIf(drRaw.Item("Email") Is DBNull.Value, String.Empty, drRaw.Item("Email"))).Trim, _
                                                                            CStr(IIf((drRaw.Item("Fax") Is DBNull.Value), String.Empty, drRaw.Item("Fax"))).Trim, _
                                                                            CStr(drRaw.Item("Relationship")).Trim, _
                                                                            CStr(IIf((drRaw.Item("Relationship_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Relationship_Remark"))).Trim, _
                                                                            CStr(drRaw.Item("Record_Status")).Trim, _
                                                                            Now, _
                                                                            strUserID.Trim, _
                                                                            Now, _
                                                                            strUserID.Trim, _
                                                                            Nothing)

                udtMedicalOrganizationModelCollection.Add(udtMedicalOrganizationModel)
            Next

            udtMOBLL.AddMOListToStaging(udtMedicalOrganizationModelCollection, udtDB)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Private Sub UpdateMODetailsToStagingForMigration(ByVal strHKID As String, ByVal strERN As String, ByVal strSPID As String, ByVal strUserID As String, ByVal udtDB As Database, Optional ByVal bByPassDeleteMO As Boolean = False)
        Dim udtMOBLL As New MedicalOrganizationBLL

        If Not bByPassDeleteMO Then udtMOBLL.DeleteAllMOStaging(strERN, udtDB)

        Dim udtMedicalOrganizationModelCollection As MedicalOrganizationModelCollection = New MedicalOrganizationModelCollection
        Dim udtMedicalOrganizationModel As MedicalOrganizationModel

        Dim intDisplaySeq As Nullable(Of Integer)
        Dim intPracticeDisplaySeq As Nullable(Of Integer) = Nothing
        Dim intAddressCode As Nullable(Of Integer)


        Dim dtRaw As New DataTable()
        Try
            Dim prams() As SqlParameter = { _
                                        udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID)}
            udtDB.RunProc("proc_MOTransition_get", prams, dtRaw)

            For i As Integer = 0 To dtRaw.Rows.Count - 1
                Dim drRaw As DataRow = dtRaw.Rows(i)

                If IsDBNull(drRaw.Item("Address_Code")) Then
                    intAddressCode = Nothing
                Else
                    intAddressCode = CInt((drRaw.Item("Address_Code")))
                End If

                If IsDBNull(drRaw.Item("Display_Seq")) Then
                    intDisplaySeq = Nothing
                Else
                    intDisplaySeq = CInt(drRaw.Item("Display_Seq"))
                End If

                udtMedicalOrganizationModel = New MedicalOrganizationModel(strERN.Trim, _
                                                                            IIf(IsNothing(strSPID), String.Empty, strSPID.Trim), _
                                                                            intDisplaySeq, _
                                                                            CType(drRaw.Item("MO_Eng_Name"), String).Trim, _
                                                                            CStr(IIf((drRaw.Item("MO_Chi_Name") Is DBNull.Value), String.Empty, drRaw.Item("MO_Chi_Name"))).Trim, _
                                                                            New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))).Trim, _
                                                                                        intAddressCode), _
                                                                            CStr(IIf(drRaw.Item("BR_Code") Is DBNull.Value, String.Empty, drRaw.Item("BR_Code"))).Trim, _
                                                                            CStr(IIf(drRaw.Item("Phone_Daytime") Is DBNull.Value, String.Empty, drRaw.Item("Phone_Daytime"))).Trim, _
                                                                            CStr(IIf(drRaw.Item("Email") Is DBNull.Value, String.Empty, drRaw.Item("Email"))).Trim, _
                                                                            CStr(IIf((drRaw.Item("Fax") Is DBNull.Value), String.Empty, drRaw.Item("Fax"))).Trim, _
                                                                            CStr(drRaw.Item("Relationship")).Trim, _
                                                                            CStr(IIf((drRaw.Item("Relationship_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Relationship_Remark"))).Trim, _
                                                                            CStr(drRaw.Item("Record_Status")).Trim, _
                                                                            Now, _
                                                                            strUserID.Trim, _
                                                                            Now, _
                                                                            strUserID.Trim, _
                                                                            Nothing)

                udtMedicalOrganizationModelCollection.Add(udtMedicalOrganizationModel)
            Next

            udtMOBLL.AddMOListToStaging(udtMedicalOrganizationModelCollection, udtDB)

        Catch ex As Exception
            Throw
        End Try

    End Sub

    Public Sub IVSSRecordDataMigration(ByVal udtSPModel As ServiceProviderModel)
        Dim udtDatabase As New Database
        Dim udtServiceProviderBLL As New ServiceProviderBLL
        Try
            ''2a. Add SPModel into Enrolment tables (With Transition data updated), include insert record in SPMigration_IVSS
            'AddServiceProviderProfileFromIVSSToEnrolment(strERN, strERN, udtDatabase)

            ''2b. Load the SP from Enrolment to Staging
            'Dim udtNewSP As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderEnrolmentProfileByERN(strERN, udtDatabase)
            'udtServiceProviderBLL.SaveToSession(udtNewSP)
            'SaveSessionObjectToStagingWithinTransition(TableLocation.Enrolment, udtDatabase)

            'Case 2.a. Add the "Complete Model to Staging"
            Dim udtPracticeSchemeInfoList As New PracticeSchemeInfoModelCollection
            Dim udtBankList As New BankAcctModelCollection
            Dim udtProfessionalList As New ProfessionalModelCollection

            For Each udtPracticeModel As PracticeModel In udtSPModel.PracticeList.Values
                If Not IsNothing(udtPracticeModel.BankAcct) Then udtBankList.Add(udtPracticeModel.BankAcct)
                If Not IsNothing(udtPracticeModel.Professional) Then
                    If IsNothing(udtProfessionalList(udtPracticeModel.Professional.ProfessionalSeq)) Then
                        udtProfessionalList.Add(udtPracticeModel.Professional)
                    End If
                End If
                If Not IsNothing(udtPracticeModel.PracticeSchemeInfoList) Then
                    For Each udtPSModel As PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
                        udtPracticeSchemeInfoList.Add(udtPSModel)
                    Next
                End If
            Next

            udtDatabase.BeginTransaction()
            Me.AddServiceProvideProfileToStagingWithinTransition(udtSPModel, udtSPModel.SchemeInfoList, udtSPModel.MOList, Nothing, udtSPModel.PracticeList, _
                                                               udtPracticeSchemeInfoList, udtBankList, udtProfessionalList, udtDatabase)

            udtDatabase.CommitTransaction()
        Catch eSQL As SqlException
            udtDatabase.RollBackTranscation()
            Throw
        Catch ex As Exception
            udtDatabase.RollBackTranscation()
            Throw
        End Try
    End Sub

    Private Function AddServiceProviderProfileFromIVSSToEnrolment(ByVal strERN As String, ByVal strNewERN As String, ByVal udtDB As Database) As Boolean
        Try

            Dim prams() As SqlParameter = { _
                           udtDB.MakeInParam("@enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strERN), _
                           udtDB.MakeInParam("@new_enrolment_ref_no", ServiceProviderModel.EnrolRefNoDataType, ServiceProviderModel.EnrolRefNoDataSize, strNewERN) _
                           }

            udtDB.RunProc("proc_DataMigration_CopyFromIVSS", prams)
            Return True

        Catch eSQL As SqlException
            Throw
        Catch ex As Exception
            Throw
            Return False
        End Try
    End Function

    'CRE15-019 (Rename PPI-ePR to eHRSS) [Start][Winnie]
    Private Function NeedUpdateEHRSSStatus(ByVal udtProfList As ProfessionalModelCollection) As Boolean
        Dim blnNeedUpdateEHRSS As Boolean = False

        Dim EHRSSProfCodeList As String = String.Empty
        Dim udtGeneralFunction As New Common.ComFunction.GeneralFunction

        udtGeneralFunction.getSystemParameter("AskHadJoinedEHRSSProfCode", EHRSSProfCodeList, String.Empty)

        ' No Need to update status if the list is empty
        If Not EHRSSProfCodeList.Equals(String.Empty) Then

            Dim blnAlreadyJoinEHRSS As Boolean
            Dim udtSP As ServiceProviderModel
            Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL

            blnAlreadyJoinEHRSS = AlreadyJoinEHRSS(udtProfList)

            If udtSPBLL.Exist Then

                udtSP = udtSPBLL.GetSP
                'No Profession could trigger the already join EHRSS question
                If Not blnAlreadyJoinEHRSS Then
                    If Not udtSP.AlreadyJoinEHR.Equals(JoinEHRSSStatus.NA) Then
                        blnNeedUpdateEHRSS = True
                    End If
                End If

            End If
        End If

        Return blnNeedUpdateEHRSS
    End Function
    'CRE15-019 (Rename PPI-ePR to eHRSS) [End][Winnie]

    ' CRE17-016 Checking of PCD status during VSS enrolment [Start][Koala]
    ' --------------------------------------------------------------------------------------------------------------------------------
    ''' <summary>
    ''' Check any need to update "Join PCD" if there is any changes on profession.
    ''' If all professions are not support PCD then reset the "Join PCD" to "I" (No need anaswer join PCD question)
    ''' </summary>
    ''' <param name="udtPractice"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function HandleServiceProviderJoinPCDStatus(ByVal udtPractice As PracticeModel, ByVal udtHCVUUser As HCVUUserModel) As Boolean
        Dim blnRes As Boolean = True

        Dim udtProfList As ProfessionalModelCollection = Nothing
        udtProfList = udtProfessionalBLL.GetProfessinalListFromStagingByERN(udtPractice.EnrolRefNo, udtDB)


        ' Check any profession is allow to join PCD
        Dim blnAllowJoinPCD As Boolean = False
        For Each udtProfessional As ProfessionalModel In udtProfList.Values
            If udtProfessional.Profession.AllowJoinPCD Then
                blnAllowJoinPCD = True
                Exit For
            End If
        Next

        ' If all professions are not allow to join PCD, update "Join PCD" to "I" (No need answer join PCD question)
        Dim udtSP As ServiceProviderModel
        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        If udtSPBLL.Exist Then

            udtSP = udtSPBLL.GetSP

            If Not blnAllowJoinPCD Then

                ' Get again the timestamp for updating the Join PCD value
                Dim udtTempSP As ServiceProviderModel
                udtTempSP = udtSPBLL.GetServiceProviderStagingByERN(udtSP.EnrolRefNo, udtDB)

                udtSP.TSMP = udtTempSP.TSMP
                udtSP.JoinPCD = JoinPCDStatus.NA
                udtSP.UpdateBy = udtHCVUUser.UserID

                blnRes = udtSPBLL.UpdateServiceProviderStagingJoinPCD(udtSP, udtDB)
            End If
        End If

        Return blnRes
    End Function
    ' CRE17-016 Checking of PCD status during VSS enrolment [End][Koala]

#Region "Data Migration Preview"

    Public Sub UpdateSPModelAfterHCVSDataMigrationForPreviewPurpose(ByVal strHKID As String, ByVal strERN As String, ByVal strUserID As String, ByRef udtSPmodel As ServiceProviderModel, ByVal udtDatabase As Database, ByVal blnConvertToStaging As Boolean)
        Try
            strHKID = strHKID.PadRight(9, " ")
            'Convert permanent SP to staging SP
            If blnConvertToStaging Then
                ConvertEHCVSPermanentSPtoStagingSP(udtSPmodel)
            End If

            '1a. Update Practice by Practice Transition
            UpdatePracticeTransitionDetailsToPracticeModel(strHKID, strUserID, udtSPmodel.PracticeList, udtDatabase)

            '1b. Add Medical Organization Transion 
            udtSPmodel.MOList = GetMOTransitionList(strHKID, strERN, strUserID, udtDatabase)

        Catch eSQL As SqlException
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub UpdatePracticeTransitionDetailsToPracticeModel(ByVal strHKID As String, ByVal strUserID As String, ByRef udtPracticeModelCollection As PracticeModelCollection, ByVal udtDB As Database)
        Dim udtPracticeBLL As New PracticeBLL
        Dim udtPracticeModel As PracticeModel

        Dim intAddressCode As Nullable(Of Integer)
        Dim intMODisplaySeq As Nullable(Of Integer)

        Try
            If Not IsNothing(udtPracticeModelCollection) Then
                For Each udtPracticeModel In udtPracticeModelCollection.Values

                    'Do not migrate the practice if the Record_status is Delisted
                    If Not udtPracticeModel.RecordStatus.Trim.Equals(PracticeStatus.Delisted) Then
                        Dim dtRaw As New DataTable

                        Dim prams() As SqlParameter = { _
                                                    udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID), _
                                                    udtDB.MakeInParam("@display_seq", PracticeModel.DisplaySeqDataType, PracticeModel.DisplaySeqDataSize, udtPracticeModel.DisplaySeq)}
                        udtDB.RunProc("proc_PracticeTransition_get", prams, dtRaw)

                        For Each drPracticeList As DataRow In dtRaw.Rows
                            If IsDBNull(drPracticeList.Item("Address_Code")) Then
                                intAddressCode = Nothing
                            Else
                                intAddressCode = CInt((drPracticeList.Item("Address_Code")))
                            End If

                            If IsDBNull(drPracticeList.Item("MO_Display_Seq")) Then
                                intMODisplaySeq = Nothing
                            Else
                                intMODisplaySeq = CInt(drPracticeList.Item("MO_Display_Seq"))
                            End If

                            'Update the existing Practice Model with the new values
                            With udtPracticeModel
                                .PracticeNameChi = CStr(IIf((drPracticeList.Item("Practice_Name_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Practice_Name_Chi"))).Trim
                                .PracticeAddress = New AddressModel(CStr(IIf((drPracticeList.Item("Room") Is DBNull.Value), String.Empty, drPracticeList.Item("Room"))).Trim, _
                                                                        CStr(IIf((drPracticeList.Item("Floor") Is DBNull.Value), String.Empty, drPracticeList.Item("Floor"))).Trim, _
                                                                        CStr(IIf((drPracticeList.Item("Block") Is DBNull.Value), String.Empty, drPracticeList.Item("Block"))).Trim, _
                                                                        CStr(IIf((drPracticeList.Item("Building") Is DBNull.Value), String.Empty, drPracticeList.Item("Building"))).Trim, _
                                                                        CStr(IIf((drPracticeList.Item("Building_Chi") Is DBNull.Value), String.Empty, drPracticeList.Item("Building_Chi"))).Trim, _
                                                                        CStr(IIf((drPracticeList.Item("District") Is DBNull.Value), String.Empty, drPracticeList.Item("District"))).Trim, _
                                                                        intAddressCode)
                                .PhoneDaytime = CStr(IIf((drPracticeList.Item("Phone_Daytime") Is DBNull.Value), String.Empty, drPracticeList.Item("Phone_Daytime"))).Trim
                                .MODisplaySeq = intMODisplaySeq

                                .UpdateBy = strUserID
                                .UpdateDtm = Now
                                '.RecordStatus = PracticeStagingStatus.Update
                            End With
                            If Not IsNothing(udtPracticeModel.SPID) AndAlso Not udtPracticeModel.SPID.Trim.Equals(String.Empty) Then
                                udtPracticeModel.RecordStatus = PracticeStagingStatus.Update
                            End If
                        Next
                    End If
                Next
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Function GetMOTransitionList(ByVal strHKID As String, ByVal strERN As String, ByVal strUserID As String, ByVal udtDB As Database) As MedicalOrganizationModelCollection
        Dim udtMOBLL As New MedicalOrganizationBLL

        Dim udtMedicalOrganizationModelCollection As MedicalOrganizationModelCollection = New MedicalOrganizationModelCollection
        Dim udtMedicalOrganizationModel As MedicalOrganizationModel

        Dim intDisplaySeq As Nullable(Of Integer)
        Dim intPracticeDisplaySeq As Nullable(Of Integer) = Nothing
        Dim intAddressCode As Nullable(Of Integer)

        Dim dtRaw As New DataTable()
        Try
            Dim prams() As SqlParameter = { _
                                        udtDB.MakeInParam("@hk_id", ServiceProviderModel.HKIDDataType, ServiceProviderModel.HKIDDataSize, strHKID)}
            udtDB.RunProc("proc_MOTransition_get_Active", prams, dtRaw)

            For i As Integer = 0 To dtRaw.Rows.Count - 1
                Dim drRaw As DataRow = dtRaw.Rows(i)

                If IsDBNull(drRaw.Item("Address_Code")) Then
                    intAddressCode = Nothing
                Else
                    intAddressCode = CInt((drRaw.Item("Address_Code")))
                End If

                If IsDBNull(drRaw.Item("Display_Seq")) Then
                    intDisplaySeq = Nothing
                Else
                    intDisplaySeq = CInt(drRaw.Item("Display_Seq"))
                End If

                udtMedicalOrganizationModel = New MedicalOrganizationModel(strERN.Trim, _
                                                                            CStr(IIf(drRaw.Item("SP_ID") Is DBNull.Value, String.Empty, drRaw.Item("SP_ID"))).Trim, _
                                                                            intDisplaySeq, _
                                                                            CType(drRaw.Item("MO_Eng_Name"), String).Trim, _
                                                                            CStr(IIf((drRaw.Item("MO_Chi_Name") Is DBNull.Value), String.Empty, drRaw.Item("MO_Chi_Name"))).Trim, _
                                                                            New AddressModel(CStr(IIf((drRaw.Item("Room") Is DBNull.Value), String.Empty, drRaw.Item("Room"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Floor") Is DBNull.Value), String.Empty, drRaw.Item("Floor"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Block") Is DBNull.Value), String.Empty, drRaw.Item("Block"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Building") Is DBNull.Value), String.Empty, drRaw.Item("Building"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("Building_Chi") Is DBNull.Value), String.Empty, drRaw.Item("Building_Chi"))).Trim, _
                                                                                        CStr(IIf((drRaw.Item("District") Is DBNull.Value), String.Empty, drRaw.Item("District"))).Trim, _
                                                                                        intAddressCode), _
                                                                            CStr(IIf(drRaw.Item("BR_Code") Is DBNull.Value, String.Empty, drRaw.Item("BR_Code"))).Trim, _
                                                                            CStr(IIf(drRaw.Item("Phone_Daytime") Is DBNull.Value, String.Empty, drRaw.Item("Phone_Daytime"))).Trim, _
                                                                            CStr(IIf(drRaw.Item("Email") Is DBNull.Value, String.Empty, drRaw.Item("Email"))).Trim, _
                                                                            CStr(IIf((drRaw.Item("Fax") Is DBNull.Value), String.Empty, drRaw.Item("Fax"))).Trim, _
                                                                            CStr(drRaw.Item("Relationship")).Trim, _
                                                                            CStr(IIf((drRaw.Item("Relationship_Remark") Is DBNull.Value), String.Empty, drRaw.Item("Relationship_Remark"))).Trim, _
                                                                            MedicalOrganizationStagingStatus.Active, _
                                                                            Now, _
                                                                            strUserID.Trim, _
                                                                            Now, _
                                                                            strUserID.Trim, _
                                                                            Nothing)
                udtMedicalOrganizationModelCollection.Add(udtMedicalOrganizationModel)
            Next

        Catch ex As Exception
            Throw
        End Try
        Return udtMedicalOrganizationModelCollection
    End Function

    Private Sub ConvertEHCVSPermanentSPtoStagingSP(ByRef udtSP As ServiceProviderModel)
        'MO
        If Not IsNothing(udtSP.MOList) Then
            For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
                If udtMO.RecordStatus = MedicalOrganizationStatus.Active Then
                    udtMO.RecordStatus = MedicalOrganizationStagingStatus.Existing
                End If
            Next
        End If

        'Practice
        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            If udtPractice.RecordStatus = PracticeStatus.Active Then
                udtPractice.RecordStatus = PracticeStagingStatus.Existing
            End If
            'bank acc
            If udtPractice.BankAcct.RecordStatus = BankAccountStatus.Active Then
                udtPractice.BankAcct.RecordStatus = BankAcctStagingStatus.Existing
            End If
            'professionl
            If udtPractice.Professional.RecordStatus = ProfessionalStatus.Active Then
                udtPractice.Professional.RecordStatus = ProfessionalStagingStatus.Existing
            End If
            'practiceSchemeInto
            For Each udtPracticeSchemeInfo As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                If udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStatus.Active Then
                    udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStagingStatus.Existing
                End If
            Next
        Next

        'Scheme Infomation
        For Each udtSchemeInformation As SchemeInformationModel In udtSP.SchemeInfoList.Values
            If udtSchemeInformation.RecordStatus = SchemeInformationStatus.Active Then
                udtSchemeInformation.RecordStatus = SchemeInformationStagingStatus.Existing
            End If
        Next

    End Sub


    Private Function UpdatePracticeStagingByPreview(ByVal udtPracticeModel As PracticeModel, ByVal udtDB As Database)
        Try
            udtPracticeBLL.UpdatePracticeStaging(udtPracticeModel, udtDB)
            Return True
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw
            Return False
        End Try
    End Function

    'Public Function HCVSIVSSRecordDataMigrationPreview(ByVal strERN_HCVS As String, ByVal strERN_IVSS As String, ByVal strUserID As String, _
    'ByRef udtSPModel_HCVS As ServiceProviderModel, ByRef udtSPModel_IVSS As ServiceProviderModel, _
    'ByRef udtNewProfessionalList As ProfessionalModelCollection, ByRef udtNewSchemeInfoList As SchemeInformationModelCollection, _
    'ByRef strHCVSLocation As String, ByRef dtProfessionalVer As DataTable) As ServiceProviderModel
    '    Dim udtCompletedSPModel As ServiceProviderModel

    '    Dim udtProfessionalBLL As New ProfessionalBLL
    '    Dim udtProfessionalList As ProfessionalModelCollection

    '    Dim udtDatabase As New Database

    '    Try
    '        udtSPModel_IVSS = udtServiceProviderBLL.GetServiceProviderStagingProfileByERN_FromIVSS(strERN_IVSS, udtDatabase, strUserID)

    '        If Not strERN_HCVS.Trim.Equals(String.Empty) Then
    '            'Find out the HCVS record is in staging or permanent
    '            udtSPModel_HCVS = GetServiceProviderStagingProfileNoSessionForMigration(strERN_HCVS, udtDatabase)

    '            If IsNothing(udtSPModel_HCVS) Then
    '                'HCVS record in permanent
    '                strHCVSLocation = TableLocation.Permanent
    '                udtSPModel_HCVS = GetServiceProviderPermanentProfile(strERN_HCVS)

    '                udtProfessionalList = udtProfessionalBLL.GetProfessinalListFromPermanentBySPID(udtSPModel_HCVS.SPID, udtDatabase)

    '                'set ref_no of practice and mo model
    '                For Each udtMOModel As MedicalOrganizationModel In udtSPModel_HCVS.MOList.Values
    '                    udtMOModel.EnrolRefNo = strERN_HCVS
    '                Next
    '                For Each udtPracticeModel As PracticeModel In udtSPModel_HCVS.PracticeList.Values
    '                    udtPracticeModel.EnrolRefNo = strERN_HCVS
    '                    If Not IsNothing(udtPracticeModel.BankAcct) Then
    '                        udtPracticeModel.BankAcct.EnrolRefNo = strERN_HCVS
    '                    End If
    '                    If Not IsNothing(udtPracticeModel.Professional) Then
    '                        udtPracticeModel.Professional.EnrolRefNo = strERN_HCVS
    '                    End If
    '                    If Not IsNothing(udtPracticeModel.PracticeSchemeInfoList) Then
    '                        For Each udtPracticeSchemeModel As PracticeSchemeInfoModel In udtPracticeModel.PracticeSchemeInfoList.Values
    '                            udtPracticeSchemeModel.EnrolRefNo = strERN_HCVS
    '                        Next
    '                    End If
    '                Next
    '                For Each udtSchemeInformationModel As SchemeInformationModel In udtSPModel_HCVS.SchemeInfoList.Values
    '                    udtSchemeInformationModel.EnrolRefNo = strERN_HCVS
    '                Next

    '                'Unpdate the permanent model to staging status
    '                ConvertEHCVSPermanentSPtoStagingSP(udtSPModel_HCVS)
    '            Else
    '                'HCVS record in staging
    '                strHCVSLocation = TableLocation.Staging
    '                udtProfessionalList = udtProfessionalBLL.GetProfessinalListFromStagingByERN(udtSPModel_HCVS.EnrolRefNo, udtDatabase)
    '            End If

    '            ''Handle Professional Verification record from IVSS
    '            'dtProfessionalVer = udtServiceProviderBLL.GetProfessionalVerificationByERN_FromIVSS(udtSPModel_IVSS.EnrolRefNo, udtDatabase, strUserID)

    '            ''Prepare the "Completed SPModel"
    '            'If dtProfessionalVer.Rows.Count > 0 Then
    '            '    udtCompletedSPModel = Me.MergeSPModel(udtSPModel_HCVS, udtProfessionalList, udtSPModel_IVSS, udtNewProfessionalList, udtNewSchemeInfoList, Nothing, dtProfessionalVer)
    '            'Else
    '            '    udtCompletedSPModel = Me.MergeSPModel(udtSPModel_HCVS, udtProfessionalList, udtSPModel_IVSS, udtNewProfessionalList, udtNewSchemeInfoList, Nothing)
    '            'End If
    '            udtCompletedSPModel = Me.MergeSPModel(udtSPModel_HCVS, udtProfessionalList, udtSPModel_IVSS, udtNewProfessionalList, udtNewSchemeInfoList, Nothing, Nothing)
    '        Else
    '            'Handle Professional Verification record from IVSS
    '            'dtProfessionalVer = udtServiceProviderBLL.GetProfessionalVerificationByERN_FromIVSS(udtSPModel_IVSS.EnrolRefNo, udtDatabase, strUserID)

    '            udtCompletedSPModel = udtSPModel_IVSS
    '        End If

    '        Return udtCompletedSPModel

    '    Catch ex As Exception
    '        Throw ex
    '    End Try

    'End Function

#End Region


#Region "Merge SP"

    Public Function MergeSPModel(ByVal udtOldSP As ServiceProviderModel, ByVal udtOldProfList As ProfessionalModelCollection, ByRef udtNewSP As ServiceProviderModel, ByRef udtNewProfList As ProfessionalModelCollection, ByRef udtSchemeListToAdd As SchemeInformationModelCollection, ByRef arySchemeCodeListToActive As ArrayList, ByRef arySchemeCodeListToActiveTSMP As ArrayList, Optional ByRef dtProfessionalVer As DataTable = Nothing) As ServiceProviderModel
        ' Safety checking - check the ByRef objects are null
        If IsNothing(udtNewProfList) Then udtNewProfList = New ProfessionalModelCollection
        If IsNothing(udtSchemeListToAdd) Then udtSchemeListToAdd = New SchemeInformationModelCollection
        If IsNothing(arySchemeCodeListToActive) Then arySchemeCodeListToActive = New ArrayList
        If IsNothing(arySchemeCodeListToActiveTSMP) Then arySchemeCodeListToActiveTSMP = New ArrayList

        Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID

        ' (Step 1) Add MOs
        Dim intNextMOSeq As Integer = udtOldSP.MOList.GetDisplaySeq()
        Dim aryHandledPractice As New ArrayList

        For Each udtNewMO As MedicalOrganizationModel In udtNewSP.MOList.Values
            UpdatePracticeMODisplaySeq(udtNewSP, udtNewMO.DisplaySeq, intNextMOSeq, aryHandledPractice)

            udtNewMO.DisplaySeq = intNextMOSeq
            udtNewMO.EnrolRefNo = udtOldSP.EnrolRefNo
            udtNewMO.SPID = udtOldSP.SPID
            udtNewMO.CreateBy = strUserID
            udtNewMO.UpdateBy = strUserID
            udtNewMO.RecordStatus = MedicalOrganizationStagingStatus.Active

            intNextMOSeq += 1
        Next

        Dim intNextPracticeSeq As Integer = udtOldSP.PracticeList.GetPracticeSeq()
        Dim intNextProfessionalSeq As Integer = udtOldProfList.GetProfessionalSeq(Nothing, Nothing)

        ' Prepare a list of Scheme Code to add (use ArrayList rather than SchemeInformationModelCollection to speed up searching)
        Dim arySchemeCodeListToAdd As New ArrayList

        ' Get the Master Scheme list (to avoid over-accessing SQL)
        Dim udtSchemeBOList As SchemeBackOfficeModelCollection = udtSchemeBackOfficeBLL.GetAllSchemeBackOfficeWithSubsidizeGroup

        For Each udtNewPractice As PracticeModel In udtNewSP.PracticeList.Values
            ' (Step 2) Add Professional
            ' Check the Professional exists
            Dim udtNewProfessional As ProfessionalModel = udtNewPractice.Professional
            Dim intFindProfSeq As Integer = IsProfessionalExist(udtOldSP, udtNewProfessional.ServiceCategoryCode, udtNewProfessional.RegistrationCode)

            If intFindProfSeq <> -1 Then
                ' If the Professional is found, assign the number to ProfessionalSeq
                udtNewPractice.ProfessionalSeq = intFindProfSeq

                'remove the professional verification record (if any)
                If Not IsNothing(dtProfessionalVer) Then
                    Dim i As Integer
                    For i = 0 To dtProfessionalVer.Rows.Count - 1
                        If Not IsNothing(dtProfessionalVer.Rows(i)("Professional_Seq")) AndAlso CInt(dtProfessionalVer.Rows(i)("Professional_Seq")) = intFindProfSeq Then
                            dtProfessionalVer.Rows(i).Delete()
                            dtProfessionalVer.AcceptChanges()
                            Exit For
                        End If
                    Next
                End If
            Else
                ' If the Professional is not found (-1), add a new Professional

                ' Check whether this professional is added to the New Professional List
                Dim iFoundProfessionalSeq As Integer = -1
                If Not IsProfessionalExist(udtNewProfList, udtNewProfessional.ServiceCategoryCode, udtNewProfessional.RegistrationCode, iFoundProfessionalSeq) Then
                    Dim udtProfessionalToAdd As ProfessionalModel = New ProfessionalModel(udtOldSP.SPID, udtOldSP.EnrolRefNo, intNextProfessionalSeq, _
                            udtNewProfessional.ServiceCategoryCode, udtNewProfessional.RegistrationCode, ProfessionalStagingStatus.Active, _
                            Nothing, strUserID)

                    udtNewProfList.Add(udtProfessionalToAdd)

                    'Update the Professional Seq in Professional Verification
                    If Not IsNothing(dtProfessionalVer) Then
                        Dim i As Integer
                        If Not IsNothing(dtProfessionalVer.Rows(i)("Professional_Seq")) AndAlso CInt(dtProfessionalVer.Rows(i)("Professional_Seq")) = udtNewProfessional.ProfessionalSeq Then
                            dtProfessionalVer.Rows(i)("Professional_Seq") = intNextProfessionalSeq
                            dtProfessionalVer.Rows(i)("Enrolment_ref_no") = udtOldSP.EnrolRefNo
                            dtProfessionalVer.Rows(i)("SP_ID") = udtOldSP.SPID
                        End If
                    End If

                    udtNewPractice.ProfessionalSeq = intNextProfessionalSeq

                    intNextProfessionalSeq += 1
                Else
                    ' CRE12-001 eHS and PCD integration [Start][Koala]
                    ' -----------------------------------------------------------------------------------------
                    ' Assign new professional seq to practice if this professional is added to the New Professional List
                    udtNewPractice.ProfessionalSeq = iFoundProfessionalSeq
                    ' CRE12-001 eHS and PCD integration [End][Koala]
                End If
            End If

            ' (Step 3) Add Practices
            UpdatePracticeSchemeInfoPracticeDisplaySeq(udtNewSP, udtNewPractice.DisplaySeq, intNextPracticeSeq)

            udtNewPractice.DisplaySeq = intNextPracticeSeq
            udtNewPractice.EnrolRefNo = udtOldSP.EnrolRefNo
            udtNewPractice.SPID = udtOldSP.SPID
            udtNewPractice.CreateBy = strUserID
            udtNewPractice.UpdateBy = strUserID
            udtNewPractice.RecordStatus = PracticeStagingStatus.Active

            ' (Step 4) Add Bank Accounts
            Dim udtNewBankAcct As BankAcctModel = udtNewPractice.BankAcct
            udtNewBankAcct.EnrolRefNo = udtOldSP.EnrolRefNo
            udtNewBankAcct.SPID = udtOldSP.SPID
            udtNewBankAcct.SpPracticeDisplaySeq = intNextPracticeSeq
            udtNewBankAcct.CreateBy = strUserID
            udtNewBankAcct.UpdateBy = strUserID
            udtNewBankAcct.RecordStatus = BankAcctStagingStatus.Active

            ' (Step 5) Add Practice Schemes
            If Not IsNothing(udtNewPractice.PracticeSchemeInfoList) Then
                For Each udtNewPractScheme As PracticeSchemeInfoModel In udtNewPractice.PracticeSchemeInfoList.Values
                    udtNewPractScheme.EnrolRefNo = udtOldSP.EnrolRefNo
                    udtNewPractScheme.SPID = udtOldSP.SPID
                    udtNewPractScheme.CreateBy = strUserID
                    udtNewPractScheme.UpdateBy = strUserID
                    udtNewPractScheme.RecordStatus = PracticeSchemeInfoStagingStatus.Active

                    ' (Step 6) Update SP Schemes
                    Dim udtScheme As SchemeInformationModel = udtOldSP.SchemeInfoList.Filter(udtNewPractScheme.SchemeCode.Trim)

                    If IsNothing(udtScheme) Then
                        If Not arySchemeCodeListToAdd.Contains(udtNewPractScheme.SchemeCode.Trim) Then
                            arySchemeCodeListToAdd.Add(udtNewPractScheme.SchemeCode.Trim)
                        End If
                    Else
                        If udtScheme.RecordStatus = SchemeInformationMaintenanceDisplayStatus.DelistedVoluntary _
                                OrElse udtScheme.RecordStatus = SchemeInformationMaintenanceDisplayStatus.DelistedInvoluntary Then
                            If Not arySchemeCodeListToActive.Contains(udtScheme.SchemeCode.Trim) Then
                                arySchemeCodeListToActive.Add(udtScheme.SchemeCode.Trim)
                            End If
                        End If
                    End If
                Next

            End If

            intNextPracticeSeq += 1

        Next

        ' Convert the arySchemeCodeListToAdd to udtSchemeListToAdd
        For Each strSchemeCode As String In arySchemeCodeListToAdd
            ' Get the Sequence No
            Dim intSeqNo As Integer = udtSchemeBOList.Filter(strSchemeCode).DisplaySeq

            udtSchemeListToAdd.Add(New SchemeInformationModel(udtOldSP.EnrolRefNo, udtOldSP.SPID, strSchemeCode, SchemeInformationStagingStatus.Active, _
                                                                String.Empty, String.Empty, Nothing, Nothing, Nothing, Date.Now, strUserID, Date.Now, _
                                                                strUserID, Nothing, intSeqNo))
        Next

        ' Prepare the Merged SP

        ' Add Scheme
        For Each udtNewScheme As SchemeInformationModel In udtSchemeListToAdd.Values
            udtOldSP.SchemeInfoList.Add(udtNewScheme)
        Next

        ' Add MO
        For Each udtNewMO As MedicalOrganizationModel In udtNewSP.MOList.Values
            udtOldSP.MOList.Add(udtNewMO)
        Next

        ' Add Practice
        For Each udtNewPractice As PracticeModel In udtNewSP.PracticeList.Values
            udtOldSP.PracticeList.Add(udtNewPractice)
        Next

        Return udtOldSP

    End Function

    Public Sub MergeSPToDatabase(ByVal udtOldSP As ServiceProviderModel, ByVal blnMoveOldSPToStaging As Boolean, ByVal udtNewSP As ServiceProviderModel, ByVal udtNewProfList As ProfessionalModelCollection, ByVal udtNewSchemeList As SchemeInformationModelCollection, ByVal arySchemeCodeListToActive As ArrayList, ByVal arySchemeCodeListToActiveTSMP As ArrayList, ByVal udtDB As Database)
        Dim strUserID As String = udtHCVUUserBLL.GetHCVUUser.UserID

        ' First check whether the two SPs' particulars are different (to update the ServiceProviderVerification table)
        Dim blnSPParticularChanged As Boolean = IsSPParticularChanged(udtOldSP, udtNewSP)

        ' If the old record is Permanent, move the record to Staging and set Under Modification
        If blnMoveOldSPToStaging Then
            Dim udtSPPermanent As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderPermanentProfileByERN(udtOldSP.EnrolRefNo, udtDB)

            udtSPPermanent.UnderModification = "Y"
            udtSPPermanent.UpdateBy = strUserID

            udtServiceProviderBLL.UpdateServiceProviderUnderModificationStatus(udtSPPermanent, udtDB)

            MoveSPPermanentToStaging(udtSPPermanent, strUserID, udtDB)

            ' Reload the SP
            udtOldSP = udtServiceProviderBLL.GetServiceProviderStagingProfileByERN(udtOldSP.EnrolRefNo, udtDB)

        End If

        ' Update SP particulars
        Dim udtUpdateSP As New ServiceProviderModel
        udtServiceProviderBLL.Clone(udtUpdateSP, udtNewSP)

        udtUpdateSP.EnrolRefNo = udtOldSP.EnrolRefNo
        udtUpdateSP.TSMP = udtOldSP.TSMP
        udtUpdateSP.UpdateBy = strUserID
        udtUpdateSP.RecordStatus = udtOldSP.RecordStatus

        ' If the old SP has SP_ID, keep the fields "Already_Joined_EHR" and "Join_EHR"
        If udtOldSP.SPID <> String.Empty Then
            udtUpdateSP.AlreadyJoinEHR = udtOldSP.AlreadyJoinEHR
            'udtUpdateSP.JoinHAPPI = udtOldSP.JoinHAPPI
        End If

        udtServiceProviderBLL.UpdateServiceProviderStagingParticulars(udtUpdateSP, udtDB)

        ' (Step 1) Add MO
        If Not IsNothing(udtNewSP.MOList) Then udtMedicalOrganizationBLL.AddMOListToStaging(udtNewSP.MOList, udtDB)

        ' (Step 2) Add Professional
        If Not IsNothing(udtNewProfList) Then
            For Each udtProfessional As ProfessionalModel In udtNewProfList.Values
                udtProfessionalBLL.AddProfessionalToStaging(udtProfessional, udtDB)
            Next
        End If

        If Not IsNothing(udtNewSP.PracticeList) Then
            For Each udtPractice As PracticeModel In udtNewSP.PracticeList.Values
                ' (Step 3) Add Practice
                udtPracticeBLL.AddPracticeToStaging(udtPractice, udtDB)

                ' (Step 4) Add Bank
                If Not IsNothing(udtPractice.BankAcct) Then
                    If udtPractice.BankAcct.DisplaySeq.HasValue Then
                        udtBankAcctBLL.AddBankAcctToStaging(udtPractice.BankAcct, udtDB)
                    End If
                End If

                ' (Step 5) Add Practice Scheme
                If Not IsNothing(udtPractice.PracticeSchemeInfoList) Then
                    udtPracticeSchemeInfoBLL.AddPracticeSchemeInfoListToStaging(udtPractice.PracticeSchemeInfoList, udtDB)
                End If
            Next
        End If

        ' (Step 6) Update SP Schemes
        If Not IsNothing(udtNewSchemeList) Then
            For Each udtNewScheme As SchemeInformationModel In udtNewSchemeList.Values
                udtSchemeInformationBLL.AddSchemeInfoToStaging(udtNewScheme, udtDB)
            Next
        End If

        If Not IsNothing(arySchemeCodeListToActive) Then
            For Each strSchemeCode As String In arySchemeCodeListToActive
                udtSchemeInformationBLL.UpdateSchemeInfoStagingStatus_DelistToActive(udtOldSP.EnrolRefNo, strSchemeCode, strUserID, udtOldSP.SchemeInfoList.Filter(strSchemeCode).TSMP, udtDB)
            Next
        End If

        ' (Step 7) Update Service Provider Verification
        udtSPAccountUpdateBLL.SaveToSession(udtSPAccountUpdateBLL.GetSPAccountUpdateByERN(udtOldSP.EnrolRefNo, udtDB))
        udtServiceProviderVerificationBLL.SaveToSession(udtServiceProviderVerificationBLL.GetSerivceProviderVerificationByERN(udtOldSP.EnrolRefNo, udtDB))

        ' Un-page-check Personal Particulars
        If blnSPParticularChanged Then UpdateSPVerifyAndAcctUpdSPInfo(UpdateSPInfo, strUserID, udtDB)

        ' Un-page-check Medical Organization
        udtServiceProviderVerificationBLL.SaveToSession(udtServiceProviderVerificationBLL.GetSerivceProviderVerificationByERN(udtOldSP.EnrolRefNo, udtDB))
        UpdateSPVerifyAndAcctUpdSPInfo(UpdateMO, strUserID, udtDB)

        ' Un-page-check Practice, Bank, Scheme
        udtServiceProviderVerificationBLL.SaveToSession(udtServiceProviderVerificationBLL.GetSerivceProviderVerificationByERN(udtOldSP.EnrolRefNo, udtDB))
        UpdateSPVerifyAndAcctUpdSPInfo(UpdatePracticeBank, strUserID, udtDB)

    End Sub

    Public Sub WriteEnrolmentToStagingAndRemove(ByVal strERN As String, ByVal strUserID As String, ByVal udtDB As Database)
        Dim udtSP As ServiceProviderModel = udtServiceProviderBLL.GetServiceProviderEnrolmentProfileByERNInHCVU(strERN, udtDB)
        Dim strAddRecordStatus As String = "A"
        Dim strMergeRecordStatus As String = "M"

        Dim strMergeERN As String = ReplaceEnrolmentReferenceNoCheckDigit(strERN)

        ' ----- Add records to staging -----

        ' Add [ServiceProviderStaging]
        udtSP.EnrolRefNo = strMergeERN
        udtSP.RecordStatus = strAddRecordStatus
        udtSP.CreateBy = strUserID
        udtSP.UpdateBy = strUserID
        udtServiceProviderBLL.AddServiceProviderParticularsToStagingForReject(udtSP, udtDB)

        ' Add [SchemeInformationStaging]
        For Each udtScheme As SchemeInformationModel In udtSP.SchemeInfoList.Values
            udtScheme.EnrolRefNo = strMergeERN
            udtScheme.RecordStatus = strAddRecordStatus
            udtScheme.CreateBy = strUserID
            udtScheme.UpdateBy = strUserID
            udtSchemeInformationBLL.AddSchemeInfoToStaging(udtScheme, udtDB)
        Next

        ' Add [MedicalOrganizationStaging]
        For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
            udtMO.EnrolRefNo = strMergeERN
            udtMO.RecordStatus = strAddRecordStatus
            udtMO.CreateBy = strUserID
            udtMO.UpdateBy = strUserID
            udtMedicalOrganizationBLL.AddMOToStaging(udtMO, udtDB)
        Next

        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            ' Add [PracticeStaging]
            udtPractice.EnrolRefNo = strMergeERN
            udtPractice.RecordStatus = strAddRecordStatus
            udtPractice.CreateBy = strUserID
            udtPractice.UpdateBy = strUserID
            udtPracticeBLL.AddPracticeToStaging(udtPractice, udtDB)

            ' Add [BankAccountStaging]
            Dim udtBank As BankAcctModel = udtPractice.BankAcct

            If Not IsNothing(udtBank) Then
                If udtBank.DisplaySeq.HasValue Then
                    udtBank.EnrolRefNo = strMergeERN
                    udtBank.RecordStatus = strAddRecordStatus
                    udtBank.CreateBy = strUserID
                    udtBank.UpdateBy = strUserID
                    udtBankAcctBLL.AddBankAcctToStaging(udtBank, udtDB)
                End If
            End If

            ' Add [PracticeSchemeInfo]
            For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                udtPracticeScheme.EnrolRefNo = strMergeERN
                udtPracticeScheme.RecordStatus = strAddRecordStatus
                udtPracticeScheme.CreateBy = strUserID
                udtPracticeScheme.UpdateBy = strUserID
                udtPracticeSchemeInfoBLL.AddPracticeSchemeInfoToStaging(udtPracticeScheme, udtDB)
            Next
        Next

        ' Add [ProfessionalStaging]
        Dim udtProfList As ProfessionalModelCollection = udtProfessionalBLL.GetProfessionalListFromEnrolmentByERN(strERN, udtDB)
        For Each udtProf As ProfessionalModel In udtProfList.Values
            udtProf.EnrolRefNo = strMergeERN
            udtProf.RecordStatus = strAddRecordStatus
            udtProf.CreateBy = strUserID
            udtProfessionalBLL.AddProfessionalToStaging(udtProf, udtDB)
        Next

        ' ----- Update Record_Status to "M" -----

        udtSP = udtServiceProviderBLL.GetServiceProviderStagingProfileByERN(strMergeERN, udtDB)
        udtProfList = udtProfessionalBLL.GetProfessinalListFromStagingByERN(strMergeERN, udtDB)

        ' Update [ServiceProviderStaging]
        udtSP.RecordStatus = strMergeRecordStatus
        udtServiceProviderBLL.UpdateServiceProviderStagingParticulars(udtSP, udtDB)

        ' Update [SchemeInformationStaging]
        For Each udtScheme As SchemeInformationModel In udtSP.SchemeInfoList.Values
            udtScheme.RecordStatus = strMergeRecordStatus
            udtSchemeInformationBLL.UpdateSchemeInfoStagingStatus(udtDB, udtScheme.EnrolRefNo, udtScheme.SchemeCode, udtScheme.RecordStatus, udtScheme.UpdateBy, udtScheme.Remark, udtScheme.TSMP)
        Next

        ' Update [MedicalOrganizationStaging]
        For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
            udtMO.RecordStatus = strMergeRecordStatus
            udtMedicalOrganizationBLL.UpdateMOStagingStatus(udtMO.EnrolRefNo, udtMO.DisplaySeq, udtMO.RecordStatus, udtMO.UpdateBy, udtMO.TSMP, udtDB)
        Next

        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            ' Update [PracticeStaging]
            udtPractice.RecordStatus = strMergeRecordStatus
            udtPracticeBLL.UpdatePracticeStagingRecordStatus(udtPractice, udtDB)

            ' Update [BankAccountStaging]
            Dim udtBank As BankAcctModel = udtPractice.BankAcct

            If Not IsNothing(udtBank) Then
                If udtBank.DisplaySeq.HasValue Then
                    udtBank.RecordStatus = strMergeRecordStatus
                    udtBankAcctBLL.UpdateBankAcctStagingRecordStatus(udtBank, udtDB)
                End If
            End If

            ' Update [PracticeSchemeInfoStaging]
            For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                udtPracticeScheme.RecordStatus = strMergeRecordStatus
                udtPracticeSchemeInfoBLL.UpdateStagingRecordStatus(udtPracticeScheme, udtPracticeScheme.RecordStatus, udtDB)
            Next
        Next

        ' Update [ProfessionalStaging]
        For Each udtProf As ProfessionalModel In udtProfList.Values
            udtProf.RecordStatus = strMergeRecordStatus
            udtProfessionalBLL.UpdateProfessionalStagingStatus(udtProf.EnrolRefNo, udtProf.ProfessionalSeq, udtProf.RecordStatus, udtDB)
        Next

        ' Update [ServiceProviderVerification]
        Dim udtSPVer As ServiceProviderVerificationModel = udtServiceProviderVerificationBLL.GetSerivceProviderVerificationByERN(strMergeERN, udtDB)
        udtSPVer.UpdateBy = strUserID
        udtSPVer.VoidBy = strUserID
        udtSPVer.VoidDtm = udtGeneralFunction.GetSystemDateTime
        udtSPVer.RecordStatus = strMergeRecordStatus
        udtServiceProviderVerificationBLL.UpdateServiceProviderVerificationReject(udtSPVer, udtDB)

        ' ----- Delete all staging records -----

        udtSP = udtServiceProviderBLL.GetServiceProviderStagingProfileByERN(strMergeERN, udtDB)
        udtProfList = udtProfessionalBLL.GetProfessinalListFromStagingByERN(strMergeERN, udtDB)
        udtSPVer = udtServiceProviderVerificationBLL.GetSerivceProviderVerificationByERN(strMergeERN, udtDB)

        ' Delete [ServiceProviderStaging]
        udtServiceProviderBLL.DeleteServiceProviderStagingByKey(udtDB, udtSP.EnrolRefNo, udtSP.TSMP, True)

        ' Delete [SchemeInformationStaging] (Not require timestamp check)
        For Each udtScheme As SchemeInformationModel In udtSP.SchemeInfoList.Values
            udtSchemeInformationBLL.DeleteSchemeInfoStaging(udtScheme, udtDB)
        Next

        ' Delete [MedicalOrganizationStaging]
        For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
            udtMedicalOrganizationBLL.DeleteMOStagingByKey(udtDB, udtMO.EnrolRefNo, udtMO.DisplaySeq, udtMO.TSMP, True)
        Next

        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            ' Delete [PracticeStaging]
            udtPracticeBLL.DeletePracticeStagingByKey(udtDB, udtPractice.EnrolRefNo, udtPractice.DisplaySeq, udtPractice.TSMP, True)

            ' Delete [BankAccountStaging]
            Dim udtBank As BankAcctModel = udtPractice.BankAcct
            If Not IsNothing(udtBank) Then
                If udtBank.DisplaySeq.HasValue Then
                    udtBankAcctBLL.DeleteBankAccountStagingByKey(udtDB, udtBank.EnrolRefNo, udtBank.DisplaySeq, udtBank.SpPracticeDisplaySeq, udtBank.TSMP, True)
                End If
            End If

            ' Delete [PracticeSchemeInfoStaging]
            For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                udtPracticeSchemeInfoBLL.DeletePracticeSchemeInfoStagingByKey(udtDB, udtPracticeScheme, udtPracticeScheme.TSMP, True)
            Next
        Next

        ' Delete [ProfessionalStaging] (Not require timestamp check)
        For Each udtProf As ProfessionalModel In udtProfList.Values
            udtProfessionalBLL.DeleteProfessionalStagingByKey(udtDB, udtProf.EnrolRefNo, udtProf.ProfessionalSeq)
        Next

        ' Delete [ServiceProviderVerification]
        udtServiceProviderVerificationBLL.DeleteServiceProviderVerification(udtDB, udtSPVer.EnrolRefNo, udtSPVer.TSMP, True)

    End Sub

    Private Sub MoveSPPermanentToStaging(ByVal udtSP As ServiceProviderModel, ByVal strUserID As String, ByVal udtDB As Database)
        Try
            ' Service Provider
            udtSP.CreateBy = strUserID
            udtSP.UpdateBy = strUserID

            If udtSP.SPID = String.Empty Then
                udtSP.RecordStatus = ServiceProviderStagingStatus.Active
            End If

            udtServiceProviderBLL.AddServiceProviderParticularsToStaging(udtSP, udtDB)

            ' Scheme
            For Each udtScheme As SchemeInformationModel In udtSP.SchemeInfoList.Values
                udtScheme.EnrolRefNo = udtSP.EnrolRefNo
                udtScheme.CreateBy = strUserID
                udtScheme.UpdateBy = strUserID

                If udtScheme.RecordStatus = SchemeInformationStatus.Active Then udtScheme.RecordStatus = SchemeInformationStagingStatus.Existing
            Next

            udtSchemeInformationBLL.AddSchemeInfoListToStaging(udtSP.SchemeInfoList, udtDB)

            ' Medical Organization
            For Each udtMO As MedicalOrganizationModel In udtSP.MOList.Values
                udtMO.EnrolRefNo = udtSP.EnrolRefNo
                udtMO.CreateBy = strUserID
                udtMO.UpdateBy = strUserID

                If udtMO.RecordStatus = MedicalOrganizationStatus.Active Then udtMO.RecordStatus = MedicalOrganizationStagingStatus.Existing
            Next

            udtMedicalOrganizationBLL.AddMOListToStaging(udtSP.MOList, udtDB)

            ' Practice
            For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
                udtPractice.EnrolRefNo = udtSP.EnrolRefNo
                udtPractice.CreateBy = strUserID
                udtPractice.UpdateBy = strUserID

                If udtPractice.RecordStatus = PracticeStatus.Active Then udtPractice.RecordStatus = PracticeStagingStatus.Existing

                udtPracticeBLL.AddPracticeToStaging(udtPractice, udtDB)

                ' Bank
                Dim udtBank As BankAcctModel = udtPractice.BankAcct
                udtBank.EnrolRefNo = udtSP.EnrolRefNo
                udtBank.CreateBy = strUserID
                udtBank.UpdateBy = strUserID

                If udtBank.RecordStatus = BankAccountStatus.Active Then udtBank.RecordStatus = BankAcctStagingStatus.Existing

                udtBankAcctBLL.AddBankAcctToStaging(udtBank, udtDB)

                ' Practice Scheme
                For Each udtPracticeScheme As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                    udtPracticeScheme.EnrolRefNo = udtSP.EnrolRefNo
                    udtPracticeScheme.CreateBy = strUserID
                    udtPracticeScheme.UpdateBy = strUserID

                    If udtPracticeScheme.RecordStatus = PracticeSchemeInfoStatus.Active Then udtPracticeScheme.RecordStatus = PracticeSchemeInfoStagingStatus.Existing
                Next

                udtPracticeSchemeInfoBLL.AddPracticeSchemeInfoListToStaging(udtPractice.PracticeSchemeInfoList, udtDB)

            Next

            ' Professional
            Dim udtProfessionalCollection As ProfessionalModelCollection = udtProfessionalBLL.GetProfessinalListFromPermanentBySPID(udtSP.SPID, udtDB)
            For Each udtProf As ProfessionalModel In udtProfessionalCollection.Values
                udtProf.EnrolRefNo = udtSP.EnrolRefNo
                udtProf.CreateBy = strUserID

                If udtProf.RecordStatus = ProfessionalStatus.Active Then udtProf.RecordStatus = ProfessionalStagingStatus.Existing
            Next

            udtProfessionalBLL.AddProfessionalListToStaging(udtProfessionalCollection, udtDB)

            ' ERN Processed
            Dim udtERNProcessedList As ERNProcessedModelCollection = udtERNProcessedBLL.GetERNProcessedListPermanentBySPID(udtSP.SPID, udtDB)
            If Not IsNothing(udtERNProcessedList) Then udtERNProcessedBLL.AddERNProcessedListToStaging(udtERNProcessedList, udtDB)

        Catch ex As Exception
            Throw

        End Try

    End Sub

    Private Function IsSPParticularChanged(ByVal udtSPA As ServiceProviderModel, ByVal udtSPB As ServiceProviderModel) As Boolean
        If udtSPA.EnglishName.Trim <> udtSPB.EnglishName.Trim Then Return True
        If udtSPA.ChineseName.Trim <> udtSPB.ChineseName.Trim Then Return True
        If Not udtSPA.SpAddress.Equals(udtSPB.SpAddress) Then Return True
        If udtSPA.Email.Trim <> udtSPB.Email.Trim Then Return True
        If udtSPA.Phone.Trim <> udtSPB.Phone.Trim Then Return True
        If udtSPA.Fax.Trim <> udtSPB.Fax.Trim Then Return True

        Return False

    End Function

    Private Sub UpdatePracticeMODisplaySeq(ByRef udtSP As ServiceProviderModel, ByVal intOldMOSeq As Integer, ByVal intNewMOSeq As Integer, ByRef aryHandledPractice As ArrayList)
        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            If aryHandledPractice.Contains(udtPractice.DisplaySeq) Then Continue For

            If udtPractice.MODisplaySeq = intOldMOSeq Then
                udtPractice.MODisplaySeq = intNewMOSeq
                aryHandledPractice.Add(udtPractice.DisplaySeq)
            End If

        Next
    End Sub

    Private Function IsProfessionalExist(ByVal udtSP As ServiceProviderModel, ByVal strServiceCategory As String, ByVal strRegistrationCode As String) As Integer
        For Each udtPractice As PracticeModel In udtSP.PracticeList.Values
            Dim udtProfessional As ProfessionalModel = udtPractice.Professional
            If udtProfessional.ServiceCategoryCode = strServiceCategory AndAlso udtProfessional.RegistrationCode = strRegistrationCode AndAlso IsProfessionalValidStatus(udtProfessional) Then Return udtProfessional.ProfessionalSeq
            'INT13-0016 Fix HCVU data entry merge professional [Karl] -- added IsProfessionalValidStatus(udtProfessional)  in the condition above
        Next

        Return -1
    End Function

    Private Function IsProfessionalExist(ByVal udtProfList As ProfessionalModelCollection, ByVal strServiceCategory As String, ByVal strRegistrationCode As String, ByRef iFoundProfessionalSeq As Integer) As Boolean
        For Each udtProfessional As ProfessionalModel In udtProfList.Values
            If udtProfessional.ServiceCategoryCode = strServiceCategory AndAlso udtProfessional.RegistrationCode = strRegistrationCode AndAlso IsProfessionalValidStatus(udtProfessional) Then
                'INT13-0016 Fix HCVU data entry merge professional [Karl] -- added IsProfessionalValidStatus(udtProfessional)  in the condition above

                ' CRE12-001 eHS and PCD integration [Start][Koala]
                ' -----------------------------------------------------------------------------------------
                ' Return found professional seq for update another new practice which using same professional registration code
                iFoundProfessionalSeq = udtProfessional.ProfessionalSeq
                ' CRE12-001 eHS and PCD integration [End][Koala]
                Return True
            End If
        Next

        Return False

    End Function
    'INT13-0016 Fix HCVU data entry merge professional [Start] [Karl]
    Private Function IsProfessionalValidStatus(ByVal udtProfessional As ProfessionalModel) As Boolean
        Dim strStatus As String = Trim(udtProfessional.RecordStatus)

        If Not String.IsNullOrEmpty(strStatus) Then
            If strStatus = ProfessionalStatus.Active Or strStatus = ProfessionalStagingStatus.Active Or strStatus = ProfessionalStagingStatus.Existing Then
                Return True
            End If
        End If

        Return False
    End Function
    'INT13-0016 Fix HCVU data entry merge professional [End] [Karl]
    Private Sub UpdatePracticeSchemeInfoPracticeDisplaySeq(ByRef udtSP As ServiceProviderModel, ByVal intOldPracticeSeq As Integer, ByVal intNewPracticeSeq As Integer)
        If IsNothing(udtSP.PracticeList(intOldPracticeSeq).PracticeSchemeInfoList) Then Return

        For Each udtPracticeScheme As PracticeSchemeInfoModel In udtSP.PracticeList(intOldPracticeSeq).PracticeSchemeInfoList.Values
            udtPracticeScheme.PracticeDisplaySeq = intNewPracticeSeq
        Next
    End Sub

    Private Function ReplaceEnrolmentReferenceNoCheckDigit(ByVal strERN As String)
        Dim strNewCheckDigit As String = "M"

        strERN = strERN.Trim

        strERN = strERN.Substring(0, strERN.Length - 1) + strNewCheckDigit

        Return strERN
    End Function

#End Region

End Class
