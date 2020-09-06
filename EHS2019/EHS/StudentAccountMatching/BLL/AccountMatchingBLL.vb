Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.ClaimRules
Imports Common.Component.DocType
Imports Common.Component.StudentFile
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSAccount
Imports Common.Component.Scheme
Imports Common.Component.StudentFile.StudentFileBLL
Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.Format
Imports Common.Validation
Imports System.Web.Script.Serialization

Public Class AccountMatchingBLL

#Region "Fields"
    Private udtCommonFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
    Private udtGeneralFunction As New GeneralFunction
    Private udtValidator As New Validator
    Private udtImmDBLL As New ImmD.ImmDBLL
    Private udtEHSAccountBLL As New EHSAccountBLL

    ' CRE19-001 (VSS 2019) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Class Mode
        Public Const INITIAL = "INITIAL"    ' Before ImmD
        Public Const RECHECK = "RECHECK"    ' After ImmD
    End Class

    Public Class AccProcessStage
        Public Const INITIAL = "INITIAL"    ' Before ImmD
        Public Const RECHECK = "RECHECK"    ' After ImmD
        Public Const RECHECKTEMPACCOUNT = "RECHECKTEMPACCOUNT"    ' After ImmD (Check TA -> VA only)
    End Class

    Public Enum enumVAcctSearchResult
        Exist
        DOB_Not_Match
        Not_Found
    End Enum
    ' CRE19-001 (VSS 2019) [End][Winnie]

    Public Class StudentAccountResultDesc
        ' Acc Validation Result
        Public Validated As String ' Validated account found
        Public Validated_PartialMatch As String ' Validated account found, some fields not match
        Public Pending_Manual_Validation As String ' Pending manual validation
        Public Pending_ImmD_Validation As String ' Pending ImmD validation
        Public Doc_Type_Not_ImmD As String ' Doc. types not for ImmD validation
        Public Removed As String ' Removed
        Public Immd_Validation_Fail As String ' ImmD validation fail
        Public Incorrect_Missing_Info As String ' Incorrect format/Missing Information
        Public Doc_Type_HKBC_IC As String ' with original doc. type of hkbc/hkic
        Public Doc_Type_HKBC_Found As String ' an validated account with the same 'HKIC No.' of doc. type 'HKBC' is found
        Public Doc_Type_HKIC_Found As String ' an validated account with the same 'HKIC No.' of doc. type 'HKIC' is found
        Public Deceased As String ' deceased record found with same doc no.
        Public Terminated As String ' account is terminated
        Public Suspended As String ' account is suspended
        Public Unknown As String ' Unknown
        Public Create_Acct_Fail As String ' Create account failed
        Public Rectify_Acct_Fail As String ' Rectify account failed
        Public Fail_Reason_EC_DocNo_Found As String ' an account with the same 'HKIC No.' with doc. type 'EC' is found
        Public Fail_Reason_IC_DocNo_Found As String ' an account with the same 'HKIC No.' of doc. type 'HKIC' is found
        Public Fail_Reason_EC_Duplicate As String ' an account with same 'Serial No.' and 'Reference' has been located in the System
        Public Fail_Reason_ADOPC_Duplicate As String ' an account with the same identity of Adoption Certificate has been located in the System
        Public Fail_Reason_ADOPC_Incorrect_Format As String ' the format of Doc No. is invalid
        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        Public Fail_Reason_Other_DocType As String ' no account creation for the doc type 'Other'
        Public Fail_Reason_DOB_Not_Match As String ' validated account exist but DOB not match
        ' CRE19-001 (VSS 2019) [End][Winnie]
    End Class

    Public Class StudentAccountMatchField
        Public Field_DocType As String
        Public Field_DocNo As String
        Public Field_DOB As String
        Public Field_EName As String
        Public Field_CName As String
        Public Field_Sex As String
        Public Field_DOI As String
        Public Field_ECSerialNo As String
        Public Field_ECRefNo As String
        Public Field_ForeignPassportNo As String
        Public Field_PermitToRemainUntil As String
        Public Field_AdoptionPrefixNum As String
    End Class
#End Region


    Public Sub New()
    End Sub

#Region "Data Validation Function"
    ''' <summary>
    ''' Check personal information Match on provided field
    ''' </summary>
    ''' <param name="blnCheckDocCode"></param>
    ''' <param name="udtStudentPersonalInfo"></param>
    ''' <param name="udtEHSPersonalInfo"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CheckPersonalInfoMatch(ByVal blnCheckDocCode As Boolean, _
                                          ByVal udtStudentPersonalInfo As EHSPersonalInformationModel, _
                                          ByVal udtEHSPersonalInfo As EHSPersonalInformationModel) As String

        Dim blnAllMatch As Boolean = True
        Dim strUnmatchField As New List(Of String)

        Dim udtStudentAccountMatchField As StudentAccountMatchField = Me.GetStudentAccountMatchField

        If blnCheckDocCode AndAlso udtStudentPersonalInfo.DocCode.Trim <> udtEHSPersonalInfo.DocCode.Trim Then
            blnAllMatch = False
            strUnmatchField.Add(udtStudentAccountMatchField.Field_DocType)
        End If

        If udtStudentPersonalInfo.IdentityNum.Trim <> udtEHSPersonalInfo.IdentityNum.Trim Then
            blnAllMatch = False
            strUnmatchField.Add(udtStudentAccountMatchField.Field_DocNo)
        End If

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        ' Add checking on [ExactDOB]
        If udtStudentPersonalInfo.DOB <> udtEHSPersonalInfo.DOB OrElse udtStudentPersonalInfo.ExactDOB <> udtEHSPersonalInfo.ExactDOB Then
            blnAllMatch = False
            strUnmatchField.Add(udtStudentAccountMatchField.Field_DOB)
        End If
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

        If udtStudentPersonalInfo.EName.Trim <> udtEHSPersonalInfo.EName.Trim Then
            blnAllMatch = False
            strUnmatchField.Add(udtStudentAccountMatchField.Field_EName)
        End If

        If udtStudentPersonalInfo.Gender.Trim <> udtEHSPersonalInfo.Gender.Trim Then
            blnAllMatch = False
            strUnmatchField.Add(udtStudentAccountMatchField.Field_Sex)
        End If

        Select Case udtEHSPersonalInfo.DocCode
            Case DocTypeModel.DocTypeCode.ADOPC

                If udtStudentPersonalInfo.AdoptionPrefixNum.Trim <> udtEHSPersonalInfo.AdoptionPrefixNum.Trim Then
                    blnAllMatch = False
                    strUnmatchField.Add(udtStudentAccountMatchField.Field_AdoptionPrefixNum)
                End If

            Case DocTypeModel.DocTypeCode.ID235B
                If Not IsNothing(udtStudentPersonalInfo.PermitToRemainUntil) Then
                    If Not udtStudentPersonalInfo.PermitToRemainUntil.Equals(udtEHSPersonalInfo.PermitToRemainUntil) Then
                        blnAllMatch = False
                        strUnmatchField.Add(udtStudentAccountMatchField.Field_PermitToRemainUntil)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.DI
                If Not IsNothing(udtStudentPersonalInfo.DateofIssue) Then
                    If Not udtStudentPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue) Then
                        blnAllMatch = False
                        strUnmatchField.Add(udtStudentAccountMatchField.Field_DOI)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.EC
                If udtStudentPersonalInfo.CName <> String.Empty Then
                    If udtStudentPersonalInfo.CName.Trim <> udtEHSPersonalInfo.CName.Trim Then
                        blnAllMatch = False
                        strUnmatchField.Add(udtStudentAccountMatchField.Field_CName)
                    End If
                End If

                If udtStudentPersonalInfo.ECSerialNo <> String.Empty Then
                    If udtStudentPersonalInfo.ECSerialNo.Trim <> udtEHSPersonalInfo.ECSerialNo.Trim Then
                        blnAllMatch = False
                        strUnmatchField.Add(udtStudentAccountMatchField.Field_ECSerialNo)
                    End If
                End If

                If udtStudentPersonalInfo.ECReferenceNo <> String.Empty Then
                    ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                    'If udtEHSPersonalInfo.ECReferenceNo Is Nothing OrElse _
                    '    udtStudentPersonalInfo.ECReferenceNo.Trim <> udtEHSPersonalInfo.ECReferenceNo.Trim Then
                    If udtEHSPersonalInfo.ECReferenceNo Is Nothing OrElse _
                        udtStudentPersonalInfo.ECReferenceNo.Trim <> udtEHSPersonalInfo.ECReferenceNo.Trim OrElse _
                        udtStudentPersonalInfo.ECReferenceNoOtherFormat <> udtEHSPersonalInfo.ECReferenceNoOtherFormat Then
                        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

                        blnAllMatch = False
                        strUnmatchField.Add(udtStudentAccountMatchField.Field_ECRefNo)
                    End If
                End If

                If Not IsNothing(udtStudentPersonalInfo.DateofIssue) Then
                    If Not udtStudentPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue) Then
                        blnAllMatch = False
                        strUnmatchField.Add(udtStudentAccountMatchField.Field_DOI)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.HKIC
                If udtStudentPersonalInfo.CName <> String.Empty Then
                    If udtStudentPersonalInfo.CName.Trim <> udtEHSPersonalInfo.CName.Trim Then
                        blnAllMatch = False
                        strUnmatchField.Add(udtStudentAccountMatchField.Field_CName)
                    End If
                End If

                If Not IsNothing(udtStudentPersonalInfo.DateofIssue) Then
                    If Not udtStudentPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue) Then
                        blnAllMatch = False
                        strUnmatchField.Add(udtStudentAccountMatchField.Field_DOI)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.REPMT
                If Not IsNothing(udtStudentPersonalInfo.DateofIssue) Then
                    If Not udtStudentPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue) Then
                        blnAllMatch = False
                        strUnmatchField.Add(udtStudentAccountMatchField.Field_DOI)
                    End If
                End If

            Case DocTypeModel.DocTypeCode.VISA
                If udtStudentPersonalInfo.Foreign_Passport_No <> String.Empty Then
                    If udtStudentPersonalInfo.Foreign_Passport_No.Trim <> udtEHSPersonalInfo.Foreign_Passport_No.Trim Then
                        blnAllMatch = False
                        strUnmatchField.Add(udtStudentAccountMatchField.Field_ForeignPassportNo)
                    End If
                End If

        End Select

        Return String.Join(", ", strUnmatchField)

    End Function

#End Region

#Region "Supporting Function"

    Public Function GetStudentAccountResultDesc() As StudentAccountResultDesc
        Return (New JavaScriptSerializer).Deserialize(Of StudentAccountResultDesc)(Common.Resource.CustomResourceProviderFactory.GetGlobalResourceObject("Text", "StudentAccountResultDesc"))
    End Function

    Public Function GetStudentAccountResultDescChi() As StudentAccountResultDesc
        Return (New JavaScriptSerializer).Deserialize(Of StudentAccountResultDesc)(Common.Resource.CustomResourceProviderFactory.GetGlobalResourceObject("Text", "StudentAccountResultDesc_Chi"))
    End Function

    Public Function GetStudentAccountMatchField() As StudentAccountMatchField
        Return (New JavaScriptSerializer).Deserialize(Of StudentAccountMatchField)(Common.Resource.CustomResourceProviderFactory.GetGlobalResourceObject("Text", "StudentAccountMatchField"))
    End Function

    Public Sub SetPersonalInfo(ByVal udtStudent As StudentFileEntryModel, _
                                   ByRef udtStudentAmend As StudentFileEntryModel, _
                                   ByRef udtNewEHSAccount As EHSAccountModel)

        ' Copy Student Personal Info
        udtStudentAmend.StudentFileID = udtStudent.StudentFileID
        udtStudentAmend.StudentSeq = udtStudent.StudentSeq

        udtStudentAmend.DocCode = udtStudent.DocCode
        udtStudentAmend.DocNo = udtStudent.DocNo
        udtStudentAmend.NameEN = udtStudent.NameEN
        udtStudentAmend.SurnameENOriginal = udtStudent.SurnameENOriginal
        udtStudentAmend.GivenNameENOriginal = udtStudent.GivenNameENOriginal
        udtStudentAmend.NameCH = udtStudent.NameCH
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
        udtStudentAmend.NameCHExcel = udtStudent.NameCHExcel
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
        udtStudentAmend.DOB = udtStudent.DOB
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
        udtStudentAmend.Exact_DOB = udtStudent.Exact_DOB
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]
        udtStudentAmend.Sex = udtStudent.Sex
        udtStudentAmend.DateOfIssue = udtStudent.DateOfIssue
        udtStudentAmend.PermitToRemainUntil = udtStudent.PermitToRemainUntil
        udtStudentAmend.ForeignPassportNo = udtStudent.ForeignPassportNo
        udtStudentAmend.ECSerialNo = udtStudent.ECSerialNo
        udtStudentAmend.ECReferenceNo = udtStudent.ECReferenceNo
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
        udtStudentAmend.ECReferenceNoOtherFormat = udtStudent.ECReferenceNoOtherFormat
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

        udtStudentAmend.CreateBy = udtStudent.CreateBy
        udtStudentAmend.UpdateDtm = udtStudent.UpdateDtm
        udtStudentAmend.UpdateBy = udtStudent.UpdateBy
        udtStudentAmend.UpdateDtm = udtStudent.UpdateDtm
        udtStudentAmend.TSMP = udtStudent.TSMP

        ' Fill Student Personal Info to EHSAccount        
        FillEHSAccountPersonalInfo(udtStudentAmend, udtNewEHSAccount, True)
    End Sub

    ''' <summary>
    ''' Fill Student Account Info by the new temp account
    ''' </summary>
    ''' <param name="udtNewEHSAccount"></param>
    ''' <param name="udtStudentAmend"></param>
    ''' <remarks>Some fields are already filled when matching</remarks>
    Public Sub convertTempAccountInfo(ByVal udtNewEHSAccount As EHSAccountModel, _
                                      ByRef udtStudentAmend As StudentFileEntryModel)

        If udtNewEHSAccount.RecordStatus = TempAccountRecordStatusClass.Validated Then
            udtStudentAmend.VoucherAccID = udtNewEHSAccount.ValidatedAccID.Trim
            udtStudentAmend.TempVoucherAccID = udtNewEHSAccount.VoucherAccID.Trim
            udtStudentAmend.AccType = EHealthAccountType.Validated
            udtStudentAmend.ValidatedAccFound = YesNo.Yes

        Else
            udtStudentAmend.TempVoucherAccID = udtNewEHSAccount.VoucherAccID.Trim
            udtStudentAmend.AccType = EHealthAccountType.Temporary

            If udtStudentAmend.ValidatedAccFound = String.Empty Then
                udtStudentAmend.ValidatedAccFound = YesNo.No
            End If
        End If

        Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtNewEHSAccount.EHSPersonalInformationList(0)

        udtStudentAmend.TempAccRecordStatus = udtNewEHSAccount.RecordStatus
        udtStudentAmend.TempAccValidateDtm = udtEHSPersonalInfo.CheckDtm

        udtStudentAmend.AccDocCode = udtEHSPersonalInfo.DocCode

        ' AccValidationResult
        Dim strResult As New List(Of String)
        Dim strResultChi As New List(Of String)

        Dim udtStudentAccountResultDesc As StudentAccountResultDesc = Me.GetStudentAccountResultDesc
        Dim udtStudentAccountResultDescChi As StudentAccountResultDesc = Me.GetStudentAccountResultDescChi

        Select Case udtNewEHSAccount.RecordStatus

            Case TempAccountRecordStatusClass.Validated
                strResult.Add(udtStudentAccountResultDesc.Validated)
                strResultChi.Add(udtStudentAccountResultDescChi.Validated)

            Case TempAccountRecordStatusClass.PendingVerify

                ' Check Manual or ImmD
                If IsManualValidation(udtEHSPersonalInfo) Then
                    strResult.Add(udtStudentAccountResultDesc.Pending_Manual_Validation)
                    strResultChi.Add(udtStudentAccountResultDescChi.Pending_Manual_Validation)
                Else
                    strResult.Add(udtStudentAccountResultDesc.Pending_ImmD_Validation)
                    strResultChi.Add(udtStudentAccountResultDescChi.Pending_ImmD_Validation)
                End If

            Case TempAccountRecordStatusClass.NotForImmDValidation

                ' CRE19-001 (VSS 2019) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                'Select Case udtStudentAmend.DocCode
                '    Case StudentFileBLL.StudentFileDocTypeCode.HKIC, _
                '        StudentFileBLL.StudentFileDocTypeCode.HKBC, _
                '        StudentFileBLL.StudentFileDocTypeCode.HKBC_IC, _
                '        StudentFileBLL.StudentFileDocTypeCode.EC, _
                '        StudentFileBLL.StudentFileDocTypeCode.ADOPC, _
                '        StudentFileBLL.StudentFileDocTypeCode.DI, _
                '        StudentFileBLL.StudentFileDocTypeCode.VISA, _
                '        StudentFileBLL.StudentFileDocTypeCode.REPMT, _
                '        StudentFileBLL.StudentFileDocTypeCode.ID235B

                '        strResult.Add(udtStudentAccountResultDesc.Incorrect_Missing_Info)

                '    Case Else
                '        strResult.Add(udtStudentAccountResultDesc.Doc_Type_Not_ImmD)
                '   End Select

                strResult.Add(udtStudentAccountResultDesc.Incorrect_Missing_Info)
                strResultChi.Add(udtStudentAccountResultDescChi.Incorrect_Missing_Info)

                ' CRE19-001 (VSS 2019) [End][Winnie]


            Case TempAccountRecordStatusClass.InValid
                strResult.Add(udtStudentAccountResultDesc.Immd_Validation_Fail)
                strResultChi.Add(udtStudentAccountResultDescChi.Immd_Validation_Fail)

            Case TempAccountRecordStatusClass.Removed
                strResult.Add(udtStudentAccountResultDesc.Removed)
                strResultChi.Add(udtStudentAccountResultDescChi.Removed)

            Case Else
                strResult.Add(udtStudentAccountResultDesc.Unknown)
                strResultChi.Add(udtStudentAccountResultDescChi.Unknown)

        End Select

        ' IC -> BC
        If udtStudentAmend.DocCode = StudentFileBLL.StudentFileDocTypeCode.HKIC AndAlso udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.HKBC Then
            strResult.Add(udtStudentAccountResultDesc.Doc_Type_HKBC_Found)
            strResultChi.Add(udtStudentAccountResultDescChi.Doc_Type_HKBC_Found)

        End If

        ' BC -> IC
        If udtStudentAmend.DocCode = StudentFileBLL.StudentFileDocTypeCode.HKBC AndAlso udtEHSPersonalInfo.DocCode = DocTypeModel.DocTypeCode.HKIC Then
            strResult.Add(udtStudentAccountResultDesc.Doc_Type_HKIC_Found)
            strResultChi.Add(udtStudentAccountResultDescChi.Doc_Type_HKIC_Found)
        End If

        ' HKBC_IC
        If udtNewEHSAccount.RecordStatus <> TempAccountRecordStatusClass.Validated Then
            If udtStudentAmend.DocCode = StudentFileBLL.StudentFileDocTypeCode.HKBC_IC Then
                strResult.Add(udtStudentAccountResultDesc.Doc_Type_HKBC_IC)
                strResultChi.Add(udtStudentAccountResultDescChi.Doc_Type_HKBC_IC)
            End If
        End If

        ' Deceased
        If udtNewEHSAccount.Deceased = True Then
            strResult.Add(udtStudentAccountResultDesc.Deceased)
            strResultChi.Add(udtStudentAccountResultDescChi.Deceased)
        End If

        If strResult.Count > 0 Then
            udtStudentAmend.AccValidationResult = String.Format("{0}|||{1}", String.Join(", ", strResult), String.Join(", ", strResultChi))
        Else
            udtStudentAmend.AccValidationResult = String.Empty
        End If

    End Sub

    ''' <summary>
    ''' Fill Student Account Info by the validated account
    ''' </summary>
    ''' <param name="udtValidatedAccount"></param>
    ''' <param name="strDocCode"></param>
    ''' <param name="strUnmatchField"></param>
    ''' <param name="udtStudentAmend"></param>
    ''' <remarks>Some fields are already filled when matching</remarks>
    Public Sub convertValidatedAccountInfo(ByVal udtValidatedAccount As EHSAccountModel, _
                                           ByVal strDocCode As String, _
                                           ByVal strUnmatchField As String, _
                                           ByRef udtStudentAmend As StudentFileEntryModel)

        udtStudentAmend.VoucherAccID = udtValidatedAccount.VoucherAccID.Trim
        udtStudentAmend.AccType = EHealthAccountType.Validated
        udtStudentAmend.ValidatedAccFound = YesNo.Yes
        udtStudentAmend.AccDocCode = strDocCode

        ' Unmatch field
        udtStudentAmend.ValidatedAccUnmatchResult = strUnmatchField

        ' AccValidationResult
        Dim strResult As New List(Of String)
        Dim strResultChi As New List(Of String)
        Dim udtStudentAccountResultDesc As StudentAccountResultDesc = Me.GetStudentAccountResultDesc
        Dim udtStudentAccountResultDescChi As StudentAccountResultDesc = Me.GetStudentAccountResultDescChi

        If strUnmatchField = String.Empty Then
            strResult.Add(udtStudentAccountResultDesc.Validated)
            strResultChi.Add(udtStudentAccountResultDescChi.Validated)

        Else
            strResult.Add(udtStudentAccountResultDesc.Validated_PartialMatch)
            strResultChi.Add(udtStudentAccountResultDescChi.Validated_PartialMatch)

        End If

        ' IC -> BC
        If udtStudentAmend.DocCode = StudentFileBLL.StudentFileDocTypeCode.HKIC AndAlso strDocCode = DocTypeModel.DocTypeCode.HKBC Then
            strResult.Add(udtStudentAccountResultDesc.Doc_Type_HKBC_Found)
            strResultChi.Add(udtStudentAccountResultDescChi.Doc_Type_HKBC_Found)
        End If

        ' BC -> IC
        If udtStudentAmend.DocCode = StudentFileBLL.StudentFileDocTypeCode.HKBC AndAlso strDocCode = DocTypeModel.DocTypeCode.HKIC Then
            strResult.Add(udtStudentAccountResultDesc.Doc_Type_HKIC_Found)
            strResultChi.Add(udtStudentAccountResultDescChi.Doc_Type_HKIC_Found)
        End If

        ' Deceased
        If udtValidatedAccount.Deceased = True Then
            strResult.Add(udtStudentAccountResultDesc.Deceased)
            strResultChi.Add(udtStudentAccountResultDescChi.Deceased)
        End If

        ' Record Status: Suspend / Terminated
        Select Case udtValidatedAccount.RecordStatus
            Case ValidatedAccountRecordStatusClass.Suspended
                strResult.Add(udtStudentAccountResultDesc.Suspended)
                strResultChi.Add(udtStudentAccountResultDescChi.Suspended)

            Case ValidatedAccountRecordStatusClass.Terminated
                strResult.Add(udtStudentAccountResultDesc.Terminated)
                strResultChi.Add(udtStudentAccountResultDescChi.Terminated)
        End Select

        If strResult.Count > 0 Then
            udtStudentAmend.AccValidationResult = String.Format("{0}|||{1}", String.Join(", ", strResult), String.Join(", ", strResultChi))
        Else
            udtStudentAmend.AccValidationResult = String.Empty
        End If

    End Sub

    Private Function getCCCode(ByVal strChineseName As String, ByVal intPosition As Integer) As String

        If strChineseName.Length >= intPosition Then
            Dim udtCCCodeBLL As New CCCode.CCCodeBLL
            Dim strCCCode As String = String.Empty

            strCCCode = udtCCCodeBLL.GetCCCodeByChar(strChineseName.Substring(intPosition - 1, 1))

            Return strCCCode
        Else
            Return ""
        End If
    End Function

#End Region

#Region "Temporary Account Related Function"

    ''' <summary>
    ''' Remove unused Temporary EHS Account
    ''' </summary>
    ''' <param name="udtEHSAccount"></param>
    ''' <param name="strUpdateBy"></param>
    ''' <param name="udtDB"></param>
    ''' <remarks>Simplified version of Back Office eHA remove temp acct</remarks>
    Public Sub RemoveTempAcct(ByVal udtEHSAccount As EHSAccountModel, ByVal strUpdateBy As String, Optional udtDB As Database = Nothing)
        If IsNothing(udtDB) Then udtDB = New Database

        Dim dtmCurrent As DateTime = udtGeneralFunction.GetSystemDateTime

        ' Update Temp EHS Account Status to "D"
        udtEHSAccountBLL.UpdateTempEHSAccountReject(udtDB, udtEHSAccount, strUpdateBy, dtmCurrent)

        ' Delete related record in table "TempVoucherAccPendingVerify"
        udtImmDBLL.DeleteTempVRAcctInPendingTable(udtDB, udtEHSAccount.VoucherAccID)
    End Sub

    ''' <summary>
    ''' Create Temporary EHS Account, Save to Database
    ''' </summary>
    ''' <param name="udtEHSAccount"></param>
    ''' <param name="udtStudent"></param>
    ''' <param name="udtStudentFileHeader"></param>
    ''' <param name="eVASearchResult"></param>
    ''' <param name="udtDB"></param>
    ''' <returns>blnSuccess</returns>
    ''' <remarks></remarks>
    Public Function CreateTemporaryEHSAccount(ByVal udtEHSAccount As EHSAccountModel, _
                                              ByVal udtStudent As StudentFileEntryModel, _
                                              ByVal udtStudentFileHeader As StudentFileHeaderModel, _
                                              ByVal eVASearchResult As enumVAcctSearchResult, _
                                              Optional udtDB As Database = Nothing) As Boolean

        If IsNothing(udtDB) Then udtDB = New Database

        Dim blnValid As Boolean = True

        Dim udtEHSAccountBLL As New Common.Component.EHSAccount.EHSAccountBLL()
        Dim udtGF As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        Dim udtErrorMsg As SystemMessage = Nothing
        Dim strFailReason As String = String.Empty
        Dim strFailReasonChi As String = String.Empty

        Dim udtStudentAccountResultDesc As StudentAccountResultDesc = Me.GetStudentAccountResultDesc
        Dim udtStudentAccountResultDescChi As StudentAccountResultDesc = Me.GetStudentAccountResultDescChi

        ' Fill Personal Info
        FillEHSAccountPersonalInfo(udtStudent, udtEHSAccount, False)

        Dim udtPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

        '------------- Validation -----------------
        ' 
        ' CRE19-001 (VSS 2019) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        If blnValid Then
            If eVASearchResult = enumVAcctSearchResult.DOB_Not_Match Then
                strFailReason = udtStudentAccountResultDesc.Fail_Reason_DOB_Not_Match
                strFailReasonChi = udtStudentAccountResultDescChi.Fail_Reason_DOB_Not_Match

                blnValid = False
            End If
        End If

        ' Not to create temp account for doc type 'Other'
        If blnValid Then
            Select Case udtPersonalInfo.DocCode
                Case DocTypeModel.DocTypeCode.OTHER
                    strFailReason = udtStudentAccountResultDesc.Fail_Reason_Other_DocType
                    strFailReasonChi = udtStudentAccountResultDescChi.Fail_Reason_Other_DocType
                    blnValid = False
            End Select
        End If
        ' CRE19-001 (VSS 2019) [End][Winnie]

        ' Check field exceed size limit
        If blnValid Then
            blnValid = CheckFieldLimit(udtPersonalInfo)

            If blnValid = False Then

                Select Case udtPersonalInfo.DocCode
                    Case DocTypeModel.DocTypeCode.ADOPC
                        strFailReason = udtStudentAccountResultDesc.Fail_Reason_ADOPC_Incorrect_Format
                        strFailReasonChi = udtStudentAccountResultDescChi.Fail_Reason_ADOPC_Incorrect_Format
                End Select
            End If
        End If

        ' Check HKIC VS EC
        If blnValid Then
            blnValid = (New ClaimRulesBLL).chkEHSAccountDocNoExistOtherDocType(udtDB, udtPersonalInfo.DocCode, udtPersonalInfo.IdentityNum, "", SysAccountSource.TemporaryAccount)

            If blnValid = False Then

                ' Same doc no. is found with HKIC/EC, mark result without create account
                Select Case udtPersonalInfo.DocCode
                    Case DocTypeModel.DocTypeCode.EC
                        strFailReason = udtStudentAccountResultDesc.Fail_Reason_IC_DocNo_Found
                        strFailReasonChi = udtStudentAccountResultDescChi.Fail_Reason_IC_DocNo_Found

                    Case DocTypeModel.DocTypeCode.HKIC
                        strFailReason = udtStudentAccountResultDesc.Fail_Reason_EC_DocNo_Found
                        strFailReasonChi = udtStudentAccountResultDescChi.Fail_Reason_EC_DocNo_Found

                End Select
            End If
        End If

        ' Check Unique
        If blnValid Then
            udtErrorMsg = (New ClaimRulesBLL).chkEHSAccountUniqueField(udtDB, udtPersonalInfo, "", EHSAccountModel.SysAccountSource.TemporaryAccount)

            If Not udtErrorMsg Is Nothing Then

                blnValid = False

                ' Duplicate record found, mark result without create account
                Select Case udtPersonalInfo.DocCode
                    Case DocTypeModel.DocTypeCode.EC
                        strFailReason = udtStudentAccountResultDesc.Fail_Reason_EC_Duplicate
                        strFailReasonChi = udtStudentAccountResultDescChi.Fail_Reason_EC_Duplicate

                    Case DocTypeModel.DocTypeCode.ADOPC
                        strFailReason = udtStudentAccountResultDesc.Fail_Reason_ADOPC_Duplicate
                        strFailReasonChi = udtStudentAccountResultDescChi.Fail_Reason_ADOPC_Duplicate

                End Select
            End If
        End If

        '------------- Create Temp Account -----------------
        If blnValid Then

            ' Create new voucher account id 
            udtEHSAccount.VoucherAccID = udtGF.generateSystemNum("C")

            ' CRE19-031 (VSS MMR Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If udtStudentFileHeader.SchemeCode = SchemeClaimModel.VSS And udtStudentFileHeader.SubsidizeCode = SubsidizeGroupClaimModel.SubsidizeCodeClass.VNIAMMR Then
                udtEHSAccount.CreateByBO = True

                udtEHSAccount.SchemeCode = String.Empty
                udtEHSAccount.CreateSPID = String.Empty
                udtEHSAccount.CreateSPPracticeDisplaySeq = 0
                udtEHSAccount.CreateBy = udtStudent.CreateBy    'upload back office user

            Else
                udtEHSAccount.CreateByBO = False

                udtEHSAccount.SchemeCode = udtStudentFileHeader.SchemeCode
                udtEHSAccount.CreateSPID = udtStudentFileHeader.SPID
                udtEHSAccount.CreateSPPracticeDisplaySeq = udtStudentFileHeader.PracticeDisplaySeq
                udtEHSAccount.CreateBy = udtStudentFileHeader.SPID    'SP  

            End If
            ' CRE19-031 (VSS MMR Upload) [End][Chris YIM]

            ' Fill in account information 
            FillEHSAccountInfo(udtEHSAccount)

            udtEHSAccountBLL.InsertEHSAccount_Core(udtDB, udtEHSAccount)

        Else
            If udtStudent.TempVoucherAccID = String.Empty Then
                udtStudent.AccValidationResult = String.Format("{0}, {1}|||{2}, {3}", _
                                                               udtStudentAccountResultDesc.Create_Acct_Fail, _
                                                               strFailReason, _
                                                               udtStudentAccountResultDescChi.Create_Acct_Fail, _
                                                               strFailReasonChi)
            Else
                udtStudent.AccValidationResult = String.Format("{0}, {1}|||{2}, {3}", _
                                                               udtStudentAccountResultDesc.Rectify_Acct_Fail, _
                                                               strFailReason, _
                                                               udtStudentAccountResultDescChi.Rectify_Acct_Fail, _
                                                               strFailReasonChi)
            End If

        End If

        Return blnValid
    End Function

    ''' <summary>
    ''' Rectify Temporary EHS Account, Save to Database
    ''' </summary>
    ''' <param name="udtOrgEHSAccount"></param>
    ''' <param name="udtEHSAccount"></param>
    ''' <param name="udtStudent"></param>
    ''' <param name="udtDB"></param>
    ''' <remarks></remarks>
    Public Function RectifyTemporaryEHSAccount(ByVal udtOrgEHSAccount As EHSAccountModel, _
                                               ByRef udtEHSAccount As EHSAccountModel, _
                                               ByVal udtStudent As StudentFileEntryModel, _
                                               Optional udtDB As Database = Nothing) As Boolean


        If IsNothing(udtDB) Then udtDB = New Database

        Dim udtEHSAccountBLL As New Common.Component.EHSAccount.EHSAccountBLL()
        Dim udtStudentAccountResultDesc As StudentAccountResultDesc = Me.GetStudentAccountResultDesc
        Dim udtStudentAccountResultDescChi As StudentAccountResultDesc = Me.GetStudentAccountResultDescChi

        Dim blnValid As Boolean = True

        ' Fill EHS Account with new Personal Info
        FillEHSAccountPersonalInfo(udtStudent, udtEHSAccount, False)

        Dim udtPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

        '------------- Validation -----------------
        If blnValid Then
            ' Check field exceed size limit
            blnValid = CheckFieldLimit(udtPersonalInfo)

            If blnValid = False Then

                Select Case udtPersonalInfo.DocCode
                    Case DocTypeModel.DocTypeCode.ADOPC
                        udtStudent.AccValidationResult = String.Format("{0}, {1}|||{2}, {3}", _
                                                                       udtStudentAccountResultDesc.Rectify_Acct_Fail, _
                                                                       udtStudentAccountResultDesc.Fail_Reason_ADOPC_Incorrect_Format, _
                                                                       udtStudentAccountResultDescChi.Rectify_Acct_Fail, _
                                                                       udtStudentAccountResultDescChi.Fail_Reason_ADOPC_Incorrect_Format)
                End Select
            End If
        End If

        If blnValid Then

            ' Check HKIC VS EC
            blnValid = (New ClaimRulesBLL).chkEHSAccountDocNoExistOtherDocType(udtDB, udtPersonalInfo.DocCode, udtPersonalInfo.IdentityNum, udtEHSAccount.VoucherAccID, SysAccountSource.TemporaryAccount)

            If blnValid = False Then

                ' Same doc no. is found with HKIC/EC, mark result without create account
                Select Case udtPersonalInfo.DocCode
                    Case DocTypeModel.DocTypeCode.EC
                        udtStudent.AccValidationResult = String.Format("{0}, {1}|||{2}, {3}", _
                                                                       udtStudentAccountResultDesc.Rectify_Acct_Fail, _
                                                                       udtStudentAccountResultDesc.Fail_Reason_IC_DocNo_Found, _
                                                                       udtStudentAccountResultDescChi.Rectify_Acct_Fail, _
                                                                       udtStudentAccountResultDescChi.Fail_Reason_IC_DocNo_Found)

                    Case DocTypeModel.DocTypeCode.HKIC
                        udtStudent.AccValidationResult = String.Format("{0}, {1}|||{2}, {3}", _
                                                                       udtStudentAccountResultDesc.Rectify_Acct_Fail, _
                                                                       udtStudentAccountResultDesc.Fail_Reason_EC_DocNo_Found, _
                                                                       udtStudentAccountResultDescChi.Rectify_Acct_Fail, _
                                                                       udtStudentAccountResultDescChi.Fail_Reason_EC_DocNo_Found)
                End Select
            End If
        End If

        '------------- Update Account with new Personal Information -----------------
        If blnValid Then

            'Fill in account information 
            FillEHSAccountInfo(udtEHSAccount)

            Dim dtmCurrent As DateTime = udtGeneralFunction.GetSystemDateTime
            udtEHSAccountBLL.UpdateEHSAccountRectify(udtOrgEHSAccount, udtEHSAccount, udtStudent.CreateBy, dtmCurrent, udtDB)

            '------------- Update Deceased Status-----------------
            Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

            ' Check and update Deceased Status if Document type or Document No. has been changed
            If udtOrgEHSAccount.EHSPersonalInformationList(0).IdentityNum.Trim <> udtEHSPersonalInfo.IdentityNum.Trim OrElse _
                udtOrgEHSAccount.EHSPersonalInformationList(0).DocCode <> udtEHSPersonalInfo.DocCode Then

                'If (New DocTypeBLL).getDocTypeByAvailable(DocTypeBLL.EnumAvailable.DeathRecordAvailable).Filter(udtEHSPersonalInfo.DocCode) IsNot Nothing Then

                ' 1. Update Personal Information, Temp Personal Information & Special Personal Information
                '    => Deceased = 'Y', DOD = XX-XX-XXXX, Exact_DOD = X   or NULL
                ' 2. Update Voucher Account, Temp Voucher Account & Special Account
                '    => Deceased = 'Y' or NULL

                Dim udtDeathRecordBLL As New eHealthAccountDeathRecord.eHealthAccountDeathRecordBLL
                udtDeathRecordBLL.UpdateDeceasedStatus(udtEHSPersonalInfo.IdentityNum, IIf(udtEHSPersonalInfo.Deceased, YesNo.Yes, YesNo.No), _
                                                       udtEHSPersonalInfo.DOD, udtEHSPersonalInfo.ExactDOD, udtStudent.CreateBy, udtDB)
                'End If
            End If
        End If

        Return blnValid
    End Function

    Private Sub FillEHSAccountInfo(ByRef udtEHSAccount As EHSAccountModel)

        Dim blnIsValid As Boolean = False
        Dim udtEHSPersonalInfo As EHSPersonalInformationModel = udtEHSAccount.EHSPersonalInformationList(0)

        'Fill in account information ---------------------------------------------------------------

        Select Case udtEHSPersonalInfo.DocCode
            Case DocTypeModel.DocTypeCode.HKIC
                blnIsValid = Validate_HKIC(udtEHSPersonalInfo)

            Case DocTypeModel.DocTypeCode.HKBC
                blnIsValid = Validate_HKBC(udtEHSPersonalInfo)

            Case DocTypeModel.DocTypeCode.DI
                blnIsValid = Validate_DI(udtEHSPersonalInfo)

            Case DocTypeModel.DocTypeCode.REPMT
                blnIsValid = Validate_ReEntryPermit(udtEHSPersonalInfo)

            Case DocTypeModel.DocTypeCode.ID235B
                blnIsValid = Validate_ID235B(udtEHSPersonalInfo)

            Case DocTypeModel.DocTypeCode.VISA
                blnIsValid = Validate_Visa(udtEHSPersonalInfo)

            Case DocTypeModel.DocTypeCode.ADOPC
                blnIsValid = Validate_ADOPT(udtEHSPersonalInfo)

            Case DocTypeModel.DocTypeCode.EC
                blnIsValid = Validate_EC(udtEHSPersonalInfo)

                ' CRE19-001 (VSS 2019) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Non eHS Doc Type
            Case DocTypeModel.DocTypeCode.OC,
                DocTypeModel.DocTypeCode.OW,
                DocTypeModel.DocTypeCode.TW,
                DocTypeModel.DocTypeCode.IR,
                DocTypeModel.DocTypeCode.HKP,
                DocTypeModel.DocTypeCode.RFNo8
                blnIsValid = Validate_NonEHSDocType(udtEHSPersonalInfo)

            Case DocTypeModel.DocTypeCode.OTHER
                blnIsValid = False
                ' CRE19-001 (VSS 2019) [End][Winnie]
        End Select


        If blnIsValid Then
            udtEHSAccount.RecordStatus = TempAccountRecordStatusClass.PendingVerify
        Else
            ' Missing information
            udtEHSAccount.RecordStatus = TempAccountRecordStatusClass.NotForImmDValidation
        End If

        udtEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForClaim

        udtEHSAccount.DataEntryBy = String.Empty
        udtEHSAccount.EHSPersonalInformationList(0).DataEntryBy = String.Empty
        udtEHSAccount.EHSPersonalInformationList(0).RecordStatus = TempPersonalInformationRecordStatusClass.ForClaim
        udtEHSAccount.EHSPersonalInformationList(0).CreateBy = udtEHSAccount.CreateBy
        udtEHSAccount.EHSPersonalInformationList(0).UpdateBy = udtEHSAccount.UpdateBy
        udtEHSAccount.EHSPersonalInformationList(0).VoucherAccID = udtEHSAccount.VoucherAccID

        ' Fill Deceased Information
        If udtEHSAccount.DeathRecord.IsDead Then
            udtEHSAccount.Deceased = True
            udtEHSAccount.EHSPersonalInformationList(0).Deceased = True
            udtEHSAccount.EHSPersonalInformationList(0).DOD = udtEHSAccount.DeathRecord.DOD
            udtEHSAccount.EHSPersonalInformationList(0).ExactDOD = udtEHSAccount.DeathRecord.ExactDOD
        End If

        udtEHSAccount.VoucherAccID = udtEHSAccount.EHSPersonalInformationList(0).VoucherAccID
        udtEHSAccount.ValidatedAccID = String.Empty
        udtEHSAccount.EHSPersonalInformationList(0).CreateBySmartID = False

        ' [CRE18-019] To read new Smart HKIC in eHS(S) [Start][Winnie]
        ' ----------------------------------------------------------------------------------------
        udtEHSAccount.EHSPersonalInformationList(0).SmartIDVer = String.Empty
        ' [CRE18-019] To read new Smart HKIC in eHS(S) [End][Winnie]

        udtEHSAccount.SubsidizeWriteOff_CreateReason = eHASubsidizeWriteOff_CreateReason.PersonalInfoCreation

        udtEHSAccount.SourceApp = SourceAppClass.SFUpload

    End Sub

    Public Sub FillEHSAccountPersonalInfo(ByVal udtStudent As StudentFileEntryModel, _
                                          ByRef udtNewEHSAccount As EHSAccountModel, _
                                          ByVal blnFillAll As Boolean)

        ' Fill Student Personal Info to EHSAccount
        Dim udtFormatter As New Formatter
        Dim strIdentityNo As String = String.Empty
        Dim strIdentityNumFull() As String
        Dim strAdoptionPrefixNo As String = String.Empty
        Dim udtValidator As New Common.Validation.Validator
        Dim udtSM As SystemMessage = Nothing

        Dim udtEHSPersonalInfo As New EHSPersonalInformationModel

        ' Doc Code        
        Select Case udtStudent.DocCode
            Case StudentFileBLL.StudentFileDocTypeCode.HKBC_IC
                ' Create temp account with document type "HKBC"
                udtEHSPersonalInfo.DocCode = StudentFileBLL.StudentFileDocTypeCode.HKBC

            Case Else
                udtEHSPersonalInfo.DocCode = udtStudent.DocCode
        End Select

        ' Doc No.
        ' Check format, Valid -> store massage doc no.; Invalid -> store uploaded doc no.

        strIdentityNo = udtStudent.DocNo

        Select Case udtEHSPersonalInfo.DocCode
            Case DocTypeModel.DocTypeCode.ADOPC
                ' Split PrefixNo & DocNo
                strIdentityNumFull = udtStudent.DocNo.Trim.Split("/")

                If strIdentityNumFull.Length = 2 Then
                    strIdentityNo = strIdentityNumFull(1).Trim
                    strAdoptionPrefixNo = strIdentityNumFull(0).Trim

                    udtSM = udtValidator.chkIdentityNumber(udtEHSPersonalInfo.DocCode, strIdentityNo, strAdoptionPrefixNo)

                    If udtSM Is Nothing Then
                        strIdentityNo = udtFormatter.formatDocumentIdentityNumber(udtEHSPersonalInfo.DocCode, strIdentityNo)
                    End If
                End If


            Case DocTypeModel.DocTypeCode.DI, _
                DocTypeModel.DocTypeCode.ID235B, _
                DocTypeModel.DocTypeCode.REPMT, _
                DocTypeModel.DocTypeCode.VISA, _
                DocTypeModel.DocTypeCode.HKIC, _
                DocTypeModel.DocTypeCode.HKBC, _
                DocTypeModel.DocTypeCode.EC

                ' Note: it replace the char by corresponding Document Type before checking 
                udtSM = udtValidator.chkIdentityNumber(udtEHSPersonalInfo.DocCode, strIdentityNo, String.Empty)

                If udtSM Is Nothing Then
                    ' Replace -() before store to DB
                    strIdentityNo = strIdentityNo.Replace("-", "").Replace("(", "").Replace(")", "")
                    strIdentityNo = udtFormatter.formatDocumentIdentityNumber(udtEHSPersonalInfo.DocCode, strIdentityNo)
                End If

            Case Else
                ' New Doc Type, no format
                strIdentityNo = udtStudent.DocNo

        End Select

        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
        'Dim strExactDOB As String = ExactDOBClass.ExactDate  ' Must be exact date
        ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

        If Not blnFillAll Then
            ' Fill info according to Doc Type
            Select Case udtEHSPersonalInfo.DocCode
                Case DocTypeModel.DocTypeCode.ADOPC
                    udtEHSPersonalInfo.IdentityNum = strIdentityNo
                    udtEHSPersonalInfo.AdoptionPrefixNum = strAdoptionPrefixNo
                    udtEHSPersonalInfo.ENameSurName = udtStudent.SurnameENOriginal
                    udtEHSPersonalInfo.ENameFirstName = udtStudent.GivenNameENOriginal
                    udtEHSPersonalInfo.DOB = udtStudent.DOB
                    udtEHSPersonalInfo.ExactDOB = udtStudent.Exact_DOB
                    udtEHSPersonalInfo.Gender = udtStudent.Sex
                    udtEHSPersonalInfo.OtherInfo = Nothing

                Case DocTypeModel.DocTypeCode.DI
                    udtEHSPersonalInfo.IdentityNum = strIdentityNo
                    udtEHSPersonalInfo.ENameSurName = udtStudent.SurnameENOriginal
                    udtEHSPersonalInfo.ENameFirstName = udtStudent.GivenNameENOriginal
                    udtEHSPersonalInfo.DOB = udtStudent.DOB
                    udtEHSPersonalInfo.ExactDOB = udtStudent.Exact_DOB
                    udtEHSPersonalInfo.Gender = udtStudent.Sex
                    udtEHSPersonalInfo.DateofIssue = udtStudent.DateOfIssue

                Case DocTypeModel.DocTypeCode.EC
                    udtEHSPersonalInfo.IdentityNum = strIdentityNo
                    udtEHSPersonalInfo.ENameSurName = udtStudent.SurnameENOriginal
                    udtEHSPersonalInfo.ENameFirstName = udtStudent.GivenNameENOriginal
                    udtEHSPersonalInfo.CName = udtStudent.NameCH
                    udtEHSPersonalInfo.DOB = udtStudent.DOB
                    udtEHSPersonalInfo.ExactDOB = udtStudent.Exact_DOB
                    udtEHSPersonalInfo.Gender = udtStudent.Sex
                    udtEHSPersonalInfo.DateofIssue = udtStudent.DateOfIssue
                    udtEHSPersonalInfo.ECSerialNo = udtStudent.ECSerialNo
                    udtEHSPersonalInfo.ECSerialNoNotProvided = IIf(udtStudent.ECSerialNo = String.Empty, True, False)
                    udtEHSPersonalInfo.ECReferenceNo = udtStudent.ECReferenceNo
                    udtEHSPersonalInfo.ECReferenceNoOtherFormat = udtStudent.ECReferenceNoOtherFormat

                    'If IsNothing(udtValidator.chkReferenceNo(udtStudent.ECReferenceNo, False)) Then
                    '    ' EC Reference is valid, set Other Format as false
                    '    udtEHSPersonalInfo.ECReferenceNoOtherFormat = False
                    'Else
                    '    udtEHSPersonalInfo.ECReferenceNoOtherFormat = True
                    'End If

                    'If Not udtEHSPersonalInfo.ECReferenceNoOtherFormat Then
                    '    udtEHSPersonalInfo.ECReferenceNo = udtStudent.ECReferenceNo.Replace("-", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty)
                    'End If

                Case DocTypeModel.DocTypeCode.HKIC
                    udtEHSPersonalInfo.IdentityNum = strIdentityNo
                    udtEHSPersonalInfo.ENameSurName = udtStudent.SurnameENOriginal
                    udtEHSPersonalInfo.ENameFirstName = udtStudent.GivenNameENOriginal
                    udtEHSPersonalInfo.CName = udtStudent.NameCH

                    udtEHSPersonalInfo.CCCode1 = getCCCode(udtStudent.NameCH, 1)
                    udtEHSPersonalInfo.CCCode2 = getCCCode(udtStudent.NameCH, 2)
                    udtEHSPersonalInfo.CCCode3 = getCCCode(udtStudent.NameCH, 3)
                    udtEHSPersonalInfo.CCCode4 = getCCCode(udtStudent.NameCH, 4)
                    udtEHSPersonalInfo.CCCode5 = getCCCode(udtStudent.NameCH, 5)
                    udtEHSPersonalInfo.CCCode6 = getCCCode(udtStudent.NameCH, 6)

                    udtEHSPersonalInfo.DOB = udtStudent.DOB
                    udtEHSPersonalInfo.ExactDOB = udtStudent.Exact_DOB
                    udtEHSPersonalInfo.Gender = udtStudent.Sex
                    udtEHSPersonalInfo.DateofIssue = udtStudent.DateOfIssue

                Case DocTypeModel.DocTypeCode.HKBC
                    udtEHSPersonalInfo.IdentityNum = strIdentityNo
                    udtEHSPersonalInfo.ENameSurName = udtStudent.SurnameENOriginal
                    udtEHSPersonalInfo.ENameFirstName = udtStudent.GivenNameENOriginal
                    udtEHSPersonalInfo.DOB = udtStudent.DOB
                    udtEHSPersonalInfo.ExactDOB = udtStudent.Exact_DOB
                    udtEHSPersonalInfo.Gender = udtStudent.Sex
                    udtEHSPersonalInfo.OtherInfo = Nothing

                Case DocTypeModel.DocTypeCode.ID235B
                    udtEHSPersonalInfo.IdentityNum = strIdentityNo
                    udtEHSPersonalInfo.ENameSurName = udtStudent.SurnameENOriginal
                    udtEHSPersonalInfo.ENameFirstName = udtStudent.GivenNameENOriginal
                    udtEHSPersonalInfo.DOB = udtStudent.DOB
                    udtEHSPersonalInfo.ExactDOB = udtStudent.Exact_DOB
                    udtEHSPersonalInfo.Gender = udtStudent.Sex
                    udtEHSPersonalInfo.PermitToRemainUntil = udtStudent.PermitToRemainUntil

                Case DocTypeModel.DocTypeCode.REPMT
                    udtEHSPersonalInfo.IdentityNum = strIdentityNo
                    udtEHSPersonalInfo.ENameSurName = udtStudent.SurnameENOriginal
                    udtEHSPersonalInfo.ENameFirstName = udtStudent.GivenNameENOriginal
                    udtEHSPersonalInfo.DOB = udtStudent.DOB
                    udtEHSPersonalInfo.ExactDOB = udtStudent.Exact_DOB
                    udtEHSPersonalInfo.Gender = udtStudent.Sex
                    udtEHSPersonalInfo.DateofIssue = udtStudent.DateOfIssue

                Case DocTypeModel.DocTypeCode.VISA
                    udtEHSPersonalInfo.IdentityNum = strIdentityNo
                    udtEHSPersonalInfo.ENameSurName = udtStudent.SurnameENOriginal
                    udtEHSPersonalInfo.ENameFirstName = udtStudent.GivenNameENOriginal
                    udtEHSPersonalInfo.DOB = udtStudent.DOB
                    udtEHSPersonalInfo.ExactDOB = udtStudent.Exact_DOB
                    udtEHSPersonalInfo.Gender = udtStudent.Sex
                    udtEHSPersonalInfo.Foreign_Passport_No = udtStudent.ForeignPassportNo

                    ' CRE19-001 (VSS 2019) [Start][Winnie]
                    ' ----------------------------------------------------------------------------------------
                Case DocTypeModel.DocTypeCode.OC,
                    DocTypeModel.DocTypeCode.OW,
                    DocTypeModel.DocTypeCode.TW,
                    DocTypeModel.DocTypeCode.IR,
                    DocTypeModel.DocTypeCode.HKP,
                    DocTypeModel.DocTypeCode.RFNo8

                    udtEHSPersonalInfo.IdentityNum = strIdentityNo
                    udtEHSPersonalInfo.ENameSurName = udtStudent.SurnameENOriginal
                    udtEHSPersonalInfo.ENameFirstName = udtStudent.GivenNameENOriginal
                    udtEHSPersonalInfo.DOB = udtStudent.DOB
                    udtEHSPersonalInfo.ExactDOB = udtStudent.Exact_DOB
                    udtEHSPersonalInfo.Gender = udtStudent.Sex

                Case DocTypeModel.DocTypeCode.OTHER
                    blnFillAll = True
                    ' CRE19-001 (VSS 2019) [End][Winnie]
            End Select
        End If

        ' Fill all info from StudentFileEntry
        If blnFillAll Then
            udtEHSPersonalInfo.IdentityNum = strIdentityNo
            udtEHSPersonalInfo.ENameSurName = udtStudent.SurnameENOriginal
            udtEHSPersonalInfo.ENameFirstName = udtStudent.GivenNameENOriginal
            udtEHSPersonalInfo.CName = udtStudent.NameCH
            udtEHSPersonalInfo.DOB = udtStudent.DOB
            udtEHSPersonalInfo.ExactDOB = udtStudent.Exact_DOB
            udtEHSPersonalInfo.Gender = udtStudent.Sex
            udtEHSPersonalInfo.DateofIssue = udtStudent.DateOfIssue
            udtEHSPersonalInfo.PermitToRemainUntil = udtStudent.PermitToRemainUntil
            udtEHSPersonalInfo.Foreign_Passport_No = udtStudent.ForeignPassportNo
            udtEHSPersonalInfo.AdoptionPrefixNum = strAdoptionPrefixNo
            udtEHSPersonalInfo.ECSerialNo = udtStudent.ECSerialNo
            udtEHSPersonalInfo.ECReferenceNo = udtStudent.ECReferenceNo.Trim
            udtEHSPersonalInfo.ECReferenceNoOtherFormat = udtStudent.ECReferenceNoOtherFormat

            'If IsNothing(udtValidator.chkReferenceNo(udtStudent.ECReferenceNo, False)) Then
            '    ' EC Reference is valid, set Other Format as false
            '    udtEHSPersonalInfo.ECReferenceNoOtherFormat = False
            'Else
            '    udtEHSPersonalInfo.ECReferenceNoOtherFormat = True
            'End If

            'If Not udtEHSPersonalInfo.ECReferenceNoOtherFormat Then
            '    udtEHSPersonalInfo.ECReferenceNo = udtStudent.ECReferenceNo.Replace("-", String.Empty).Replace("(", String.Empty).Replace(")", String.Empty)
            'End If
        End If

        udtEHSPersonalInfo.TSMP = udtNewEHSAccount.EHSPersonalInformationList(0).TSMP

        udtNewEHSAccount.EHSPersonalInformationList.Clear()
        udtNewEHSAccount.EHSPersonalInformationList.Add(udtEHSPersonalInfo)

    End Sub



#End Region

#Region "Validated Account Related Function"
    Public Function IsVAcctExisted(ByRef udtStudent As StudentFileEntryModel, _
                                   ByVal udtNewEHSPersonalInfo As EHSPersonalInformationModel) As enumVAcctSearchResult

        Dim eVASearchResult As enumVAcctSearchResult = enumVAcctSearchResult.Not_Found

        Dim udtFormatter As New Formatter
        Dim udtValidator As Common.Validation.Validator = New Common.Validation.Validator
        Dim udtSM As SystemMessage = Nothing

        Dim strIdentityNo As String = udtNewEHSPersonalInfo.IdentityNum

        ' Check Doc No. format, if Doc no. format is invalid, assume no validate account will be found (Except ADOPC)
        ' Special handle: by pass checking for ADOPC as search account with IdentityNum only
        ' e.g. Upload: A/12345, can find an validated account: A123456/12345
        If udtStudent.DocCode <> StudentFileBLL.StudentFileDocTypeCode.ADOPC Then

            udtSM = udtValidator.chkIdentityNumber(udtNewEHSPersonalInfo.DocCode, udtNewEHSPersonalInfo.IdentityNum, String.Empty)

            If Not udtSM Is Nothing Then
                Return enumVAcctSearchResult.Not_Found

            End If
        End If

        ' Search Validated Account
        Select Case udtStudent.DocCode
            Case StudentFileBLL.StudentFileDocTypeCode.ADOPC, _
                StudentFileBLL.StudentFileDocTypeCode.DI, _
                StudentFileBLL.StudentFileDocTypeCode.EC, _
                StudentFileBLL.StudentFileDocTypeCode.ID235B, _
                StudentFileBLL.StudentFileDocTypeCode.REPMT, _
                StudentFileBLL.StudentFileDocTypeCode.VISA

                eVASearchResult = SearchValidatedAccount(strIdentityNo, udtStudent.DocCode, udtStudent, udtNewEHSPersonalInfo)


            Case StudentFileBLL.StudentFileDocTypeCode.HKBC

                ' CRE19-001 (VSS 2019) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                ' Check Against with HKIC first
                eVASearchResult = SearchValidatedAccount(strIdentityNo, DocTypeModel.DocTypeCode.HKIC, udtStudent, udtNewEHSPersonalInfo)

                ' Check Against with HKBC if VA with HKIC is not exist (i.e. DOB not matched)
                If eVASearchResult <> enumVAcctSearchResult.Exist Then
                    eVASearchResult = SearchValidatedAccount(strIdentityNo, DocTypeModel.DocTypeCode.HKBC, udtStudent, udtNewEHSPersonalInfo)

                End If
                ' CRE19-001 (VSS 2019) [End][Winnie]

            Case StudentFileBLL.StudentFileDocTypeCode.HKIC
                ' Check HKIC first
                eVASearchResult = SearchValidatedAccount(strIdentityNo, DocTypeModel.DocTypeCode.HKIC, udtStudent, udtNewEHSPersonalInfo)

                ' CRE19-001 (VSS 2019) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
                '' No checking Against with HKBC
                'If eVASearchResult = enumVAcctSearchResult.Not_Found Then
                '    eVASearchResult = SearchValidatedAccount(strIdentityNo, DocTypeModel.DocTypeCode.HKBC, udtStudent, udtNewEHSPersonalInfo)
                'End If
                ' CRE19-001 (VSS 2019) [End][Winnie]

            Case StudentFileBLL.StudentFileDocTypeCode.HKBC_IC

                ' Assume the student using "HKIC" first, if no account found, check "HKBC"
                eVASearchResult = SearchValidatedAccount(strIdentityNo, DocTypeModel.DocTypeCode.HKIC, udtStudent, udtNewEHSPersonalInfo)

                If eVASearchResult = enumVAcctSearchResult.Exist Then
                    eVASearchResult = SearchValidatedAccount(strIdentityNo, DocTypeModel.DocTypeCode.HKBC, udtStudent, udtNewEHSPersonalInfo)
                End If

                ' CRE19-001 (VSS 2019) [Start][Winnie]
            Case StudentFileDocTypeCode.OC,
                StudentFileDocTypeCode.OW,
                StudentFileDocTypeCode.TW,
                StudentFileDocTypeCode.IR,
                StudentFileDocTypeCode.HKP,
                StudentFileDocTypeCode.RFNo8,
                StudentFileDocTypeCode.OTHER
                ' Doc Type not available in eHS

                eVASearchResult = SearchValidatedAccount(strIdentityNo, udtStudent.DocCode, udtStudent, udtNewEHSPersonalInfo)
                ' CRE19-001 (VSS 2019) [End][Winnie]

            Case Else
                Return enumVAcctSearchResult.Not_Found

        End Select

        Return eVASearchResult
    End Function

    Private Function SearchValidatedAccount(ByVal strIdentityNo As String, _
                                            ByVal strDocCode As String, _
                                            ByRef udtStudent As StudentFileEntryModel, _
                                            ByVal udtNewEHSPersonalInfo As EHSPersonalInformationModel) As enumVAcctSearchResult
        Dim udtFormatter As New Formatter
        Dim udtEHSAccountBLL As New EHSAccountBLL
        Dim udtSystemMessage As SystemMessage = Nothing

        Dim eVASearchResult As enumVAcctSearchResult = enumVAcctSearchResult.Not_Found

        ' Get the EHS Validated Account
        Dim udtValidatedAccount As EHSAccountModel = udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNo, strDocCode)

        If Not IsNothing(udtValidatedAccount) Then
            ' VAcct with same doc no. found            
            udtStudent.ValidatedAccFound = YesNo.Yes

            Dim udtEHSPersonalInfo As EHSPersonalInformationModel
            udtEHSPersonalInfo = udtValidatedAccount.getPersonalInformation(strDocCode)

            ' Check all provided fields match with VA
            Dim strUnmatchField As String = String.Empty
            Dim blnCheckDocType As Boolean = False

            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            ' No checking on Doc Type
            'Select Case udtStudent.DocCode
            '    Case StudentFileBLL.StudentFileDocTypeCode.HKBC, StudentFileBLL.StudentFileDocTypeCode.HKIC
            '        blnCheckDocType = True
            '    Case Else
            '        blnCheckDocType = False
            'End Select
            ' CRE19-001-04 (PPP 2019-20 - RVP Pre-check) [End][Winnie]

            strUnmatchField = CheckPersonalInfoMatch(blnCheckDocType, udtNewEHSPersonalInfo, udtEHSPersonalInfo)

            ' CRE19-001 (VSS 2019) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If strUnmatchField.Contains("DOB") Then
                ' DOB not matched but same doc no. exist, not using the VA and not allow to create temp acct
                eVASearchResult = enumVAcctSearchResult.DOB_Not_Match
            Else
                ' DOB matched (Other fields may be different) use the VA directly
                eVASearchResult = enumVAcctSearchResult.Exist
            End If

            'If strUnmatchField = String.Empty Then
            '    ' All fields match, use the Validate Account directly
            '    eVASearchResult = enumVAcctSearchResult.Exist

            'ElseIf IsImmDFieldMatch(udtNewEHSPersonalInfo, udtEHSPersonalInfo) Then
            '    ' All ImmD Validation fields match, use the Validate Account directly
            '    eVASearchResult = enumVAcctSearchResult.Exist

            'Else
            '    ' DOB match but other field not match, create temp acct
            '    eVASearchResult = enumVAcctSearchResult.Not_Found

            'End If
            ' CRE19-001 (VSS 2019) [End][Winnie]

            If eVASearchResult = enumVAcctSearchResult.Exist Then
                Me.convertValidatedAccountInfo(udtValidatedAccount, strDocCode, strUnmatchField, udtStudent)
            End If

        Else
            ' VA with same doc no. not exist
            eVASearchResult = enumVAcctSearchResult.Not_Found

            If udtStudent.ValidatedAccFound = String.Empty Then
                udtStudent.ValidatedAccFound = YesNo.No
            End If

        End If

        Return eVASearchResult
    End Function
#End Region

#Region "ImmD Validation Function"
    ''' <summary>
    ''' Check if all fields for ImmD validation are matched
    ''' </summary>
    ''' <param name="udtNewEHSPersonalInfo"></param>
    ''' <param name="udtEHSPersonalInfo"></param>
    ''' <returns>blnImmDFieldMatch, True = All Match</returns>
    ''' <remarks></remarks>
    Public Function IsImmDFieldMatch(ByVal udtNewEHSPersonalInfo As EHSPersonalInformationModel, _
                                      ByVal udtEHSPersonalInfo As EHSPersonalInformationModel) As Boolean

        Dim blnImmDFieldMatch As Boolean = False

        Select Case udtEHSPersonalInfo.DocCode
            Case DocType.DocTypeModel.DocTypeCode.ADOPC

                If udtNewEHSPersonalInfo.DOB = udtEHSPersonalInfo.DOB Then
                    blnImmDFieldMatch = True
                End If

            Case DocType.DocTypeModel.DocTypeCode.DI
                'Dim dtmPermit_DOI As Date = New Date(2003, 9, 1)
                Dim strDI_DOI As String = String.Empty
                udtCommonFunction.getSystemParameter("DI_DOI", strDI_DOI, String.Empty)

                Dim dtmDI_DOI As New Date
                dtmDI_DOI = CDate(strDI_DOI)

                If udtEHSPersonalInfo.DateofIssue < dtmDI_DOI Then

                    If udtNewEHSPersonalInfo.DOB = udtEHSPersonalInfo.DOB AndAlso _
                        udtNewEHSPersonalInfo.EName = udtEHSPersonalInfo.EName AndAlso _
                        udtNewEHSPersonalInfo.Gender = udtEHSPersonalInfo.Gender AndAlso _
                        (udtNewEHSPersonalInfo.DateofIssue Is Nothing OrElse _
                         udtNewEHSPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue)) Then

                        blnImmDFieldMatch = True
                    End If

                Else
                    If udtNewEHSPersonalInfo.DOB = udtEHSPersonalInfo.DOB AndAlso _
                        (udtNewEHSPersonalInfo.DateofIssue Is Nothing OrElse _
                         udtNewEHSPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue)) Then

                        blnImmDFieldMatch = True
                    End If
                End If

            Case DocType.DocTypeModel.DocTypeCode.EC

                If udtNewEHSPersonalInfo.DOB = udtEHSPersonalInfo.DOB AndAlso _
                    (udtNewEHSPersonalInfo.ECSerialNo = String.Empty OrElse udtNewEHSPersonalInfo.ECSerialNo = udtEHSPersonalInfo.ECSerialNo) AndAlso _
                    (udtNewEHSPersonalInfo.ECReferenceNo = String.Empty OrElse udtNewEHSPersonalInfo.ECReferenceNo = udtEHSPersonalInfo.ECReferenceNo) Then

                    blnImmDFieldMatch = True
                End If

            Case DocType.DocTypeModel.DocTypeCode.HKBC

                If udtNewEHSPersonalInfo.DOB = udtEHSPersonalInfo.DOB Then
                    blnImmDFieldMatch = True
                End If

            Case DocType.DocTypeModel.DocTypeCode.HKIC
                If udtNewEHSPersonalInfo.DOB = udtEHSPersonalInfo.DOB AndAlso _
                    (udtNewEHSPersonalInfo.DateofIssue Is Nothing OrElse _
                     udtNewEHSPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue)) Then

                    blnImmDFieldMatch = True
                End If

            Case DocType.DocTypeModel.DocTypeCode.ID235B

                If udtNewEHSPersonalInfo.DOB = udtEHSPersonalInfo.DOB Then
                    blnImmDFieldMatch = True
                End If

            Case DocType.DocTypeModel.DocTypeCode.REPMT
                'Dim dtmPermit_DOI As Date = New Date(2007, 6, 4)
                Dim strREPMT_DOI As String = String.Empty
                udtCommonFunction.getSystemParameter("REPMT_DOI", strREPMT_DOI, String.Empty)

                Dim dtmPermit_DOI As New Date
                dtmPermit_DOI = CDate(strREPMT_DOI)

                If udtEHSPersonalInfo.DateofIssue < dtmPermit_DOI Then

                    If udtNewEHSPersonalInfo.DOB = udtEHSPersonalInfo.DOB AndAlso _
                        udtNewEHSPersonalInfo.EName = udtEHSPersonalInfo.EName AndAlso _
                        udtNewEHSPersonalInfo.Gender = udtEHSPersonalInfo.Gender AndAlso _
                        (udtNewEHSPersonalInfo.DateofIssue Is Nothing OrElse _
                         udtNewEHSPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue)) Then

                        blnImmDFieldMatch = True
                    End If
                Else
                    If udtNewEHSPersonalInfo.DOB = udtEHSPersonalInfo.DOB AndAlso _
                        (udtNewEHSPersonalInfo.DateofIssue Is Nothing OrElse _
                         udtNewEHSPersonalInfo.DateofIssue.Equals(udtEHSPersonalInfo.DateofIssue)) Then

                        blnImmDFieldMatch = True
                    End If
                End If

            Case DocType.DocTypeModel.DocTypeCode.VISA
                If udtNewEHSPersonalInfo.DOB = udtEHSPersonalInfo.DOB Then
                    blnImmDFieldMatch = True
                End If

                ' CRE19-001 (VSS 2019) [Start][Winnie]
                ' ----------------------------------------------------------------------------------------
            Case DocTypeModel.DocTypeCode.OC,
                DocTypeModel.DocTypeCode.OW,
                DocTypeModel.DocTypeCode.TW,
                DocTypeModel.DocTypeCode.IR,
                DocTypeModel.DocTypeCode.HKP,
                DocTypeModel.DocTypeCode.RFNo8,
                DocTypeModel.DocTypeCode.OTHER

                If udtNewEHSPersonalInfo.DOB = udtEHSPersonalInfo.DOB Then
                    blnImmDFieldMatch = True
                End If
                ' CRE19-001 (VSS 2019) [End][Winnie]

            Case Else
                blnImmDFieldMatch = False

        End Select

        Return blnImmDFieldMatch

    End Function

    Public Function IsManualValidation(ByVal udtEHSPersonalInfo As EHSPersonalInformationModel) As Boolean

        Dim blnManualValidate As Boolean = False

        Dim udtDocTypeList As DocTypeModelCollection
        udtDocTypeList = (New DocTypeBLL).getAllDocType

        If udtDocTypeList.Filter(udtEHSPersonalInfo.DocCode).ForceManualValidate Then
            blnManualValidate = True
            Return blnManualValidate
        End If

        Select Case udtEHSPersonalInfo.DocCode
            Case DocType.DocTypeModel.DocTypeCode.ADOPC
                If udtEHSPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactDate) OrElse _
                    udtEHSPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactMonth) OrElse _
                    udtEHSPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactYear) Then

                    blnManualValidate = True
                End If

            Case DocType.DocTypeModel.DocTypeCode.DI
                blnManualValidate = False

            Case DocType.DocTypeModel.DocTypeCode.EC
                'Dim dtmEC_DOI As Date = New Date(2003, 6, 23)

                Dim strEC_DOI As String = String.Empty
                udtCommonFunction.getSystemParameter("EC_DOI", strEC_DOI, String.Empty)

                Dim dtmEC_DOI As New Date
                dtmEC_DOI = CDate(strEC_DOI)

                If udtEHSPersonalInfo.DateofIssue < dtmEC_DOI OrElse _
                   udtEHSPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactDate) OrElse _
                   udtEHSPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactMonth) OrElse _
                   udtEHSPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactYear) Then

                    blnManualValidate = True
                End If

            Case DocType.DocTypeModel.DocTypeCode.HKBC
                If udtEHSPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactDate) OrElse _
                    udtEHSPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactMonth) OrElse _
                    udtEHSPersonalInfo.ExactDOB.Trim.Equals(EHSAccountModel.ExactDOBClass.ManualExactYear) Then

                    blnManualValidate = True
                End If

            Case DocType.DocTypeModel.DocTypeCode.HKIC
                blnManualValidate = False

            Case DocType.DocTypeModel.DocTypeCode.ID235B
                blnManualValidate = True

            Case DocType.DocTypeModel.DocTypeCode.REPMT
                blnManualValidate = False

            Case DocType.DocTypeModel.DocTypeCode.VISA
                blnManualValidate = False

            Case DocTypeModel.DocTypeCode.OC,
                DocTypeModel.DocTypeCode.OW,
                DocTypeModel.DocTypeCode.TW,
                DocTypeModel.DocTypeCode.IR,
                DocTypeModel.DocTypeCode.HKP,
                DocTypeModel.DocTypeCode.RFNo8,
                DocTypeModel.DocTypeCode.OTHER
                ' Non EHS Doc Type
                blnManualValidate = True

        End Select

        Return blnManualValidate

    End Function
#End Region

#Region "Personal Info Validation for document type"
    'HKIC
    Private Function Validate_HKIC(ByRef udtEHSPersonalInfo As EHSPersonalInformationModel) As Boolean
        Dim isValid As Boolean = True
        Dim udtSM As SystemMessage = Nothing
        Dim udtformatter As New Common.Format.Formatter

        'HKIC
        udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.HKIC, udtEHSPersonalInfo.IdentityNum, String.Empty)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'DOB
        Dim strExactDOB As String = udtEHSPersonalInfo.ExactDOB
        Dim strDOB As String = udtformatter.formatInputDate(udtEHSPersonalInfo.DOB)
        Dim dtmDOB As Date = udtEHSPersonalInfo.DOB

        udtSM = Me.udtValidator.chkDOB(DocTypeModel.DocTypeCode.HKIC, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'English Name
        udtSM = Me.udtValidator.chkEngName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'Gender
        udtSM = Me.udtValidator.chkGender(udtEHSPersonalInfo.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'DOI
        If udtEHSPersonalInfo.DateofIssue Is Nothing Then
            isValid = False
        Else
            Dim strDOI As String = String.Empty
            strDOI = udtformatter.formatInputDate(udtEHSPersonalInfo.DateofIssue.Value)

            udtSM = udtValidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.REPMT, strDOI, dtmDOB)
            If Not IsNothing(udtSM) Then
                isValid = False
            End If
        End If

        Return isValid
    End Function

    'EC
    Private Function Validate_EC(ByRef udtEHSPersonalInfo As EHSPersonalInformationModel) As Boolean
        Dim isValid As Boolean = True
        Dim udtSM As SystemMessage = Nothing
        Dim udtformatter As New Common.Format.Formatter

        ' Serial No.
        udtSM = Me.udtValidator.chkSerialNo(udtEHSPersonalInfo.ECSerialNo, udtEHSPersonalInfo.ECSerialNoNotProvided)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        ' Reference
        udtSM = Me.udtValidator.chkReferenceNo(udtEHSPersonalInfo.ECReferenceNo, udtEHSPersonalInfo.ECReferenceNoOtherFormat)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'DOB
        Dim strExactDOB As String = udtEHSPersonalInfo.ExactDOB
        Dim strDOB As String = udtformatter.formatInputDate(udtEHSPersonalInfo.DOB)
        Dim dtmDOB As Date = udtEHSPersonalInfo.DOB

        udtSM = Me.udtValidator.chkDOB(DocTypeModel.DocTypeCode.EC, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'HKIC No
        udtSM = udtValidator.chkHKID(udtEHSPersonalInfo.IdentityNum)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'English Name
        udtSM = Me.udtValidator.chkEngName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'Chinese Name
        udtSM = udtValidator.chkChiName(udtEHSPersonalInfo.CName)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'Gender
        udtSM = Me.udtValidator.chkGender(udtEHSPersonalInfo.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'DOI
        If udtEHSPersonalInfo.DateofIssue Is Nothing Then
            isValid = False
        Else
            Dim strDOI As String = String.Empty
            strDOI = udtformatter.formatInputDate(udtEHSPersonalInfo.DateofIssue.Value)

            udtSM = udtValidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.REPMT, strDOI, dtmDOB)
            If Not IsNothing(udtSM) Then
                isValid = False
            End If

            udtSM = udtValidator.chkSerialNoNotProvidedAllow(udtEHSPersonalInfo.DateofIssue, udtEHSPersonalInfo.ECSerialNoNotProvided)
            If Not IsNothing(udtSM) Then
                isValid = False
            End If

            ' Try parse the Reference
            If udtEHSPersonalInfo.ECReferenceNoOtherFormat Then
                udtValidator.TryParseECReference(udtEHSPersonalInfo.ECReferenceNo, udtEHSPersonalInfo.ECReferenceNoOtherFormat, udtEHSPersonalInfo.DateofIssue)
            End If

            udtSM = udtValidator.chkReferenceOtherFormatAllow(udtEHSPersonalInfo.DateofIssue, udtEHSPersonalInfo.ECReferenceNoOtherFormat)
            If Not IsNothing(udtSM) Then
                isValid = False
            End If

        End If

        Return isValid
    End Function

    'HKBC
    Private Function Validate_HKBC(ByRef udtEHSPersonalInfo As EHSPersonalInformationModel) As Boolean
        Dim isValid As Boolean = True
        Dim udtSM As SystemMessage = Nothing
        Dim udtformatter As New Common.Format.Formatter

        'RegNo.
        udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.HKBC, udtEHSPersonalInfo.IdentityNum, String.Empty)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'DOB
        Dim strExactDOB As String = udtEHSPersonalInfo.ExactDOB
        Dim strDOB As String = udtformatter.formatInputDate(udtEHSPersonalInfo.DOB)
        Dim dtmDOB As Date = udtEHSPersonalInfo.DOB

        udtSM = Me.udtValidator.chkDOB(DocTypeModel.DocTypeCode.HKBC, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'English Name
        udtSM = Me.udtValidator.chkEngName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'Gender
        udtSM = Me.udtValidator.chkGender(udtEHSPersonalInfo.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        Return isValid
    End Function

    'Adoption
    Private Function Validate_ADOPT(ByRef udtEHSPersonalInfo As EHSPersonalInformationModel) As Boolean
        Dim isValid As Boolean = True
        Dim udtSM As SystemMessage = Nothing
        Dim udtformatter As New Common.Format.Formatter

        'No. of Entry
        udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.ADOPC, udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.AdoptionPrefixNum)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'English Name
        udtSM = Me.udtValidator.chkEngName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'Gender
        udtSM = Me.udtValidator.chkGender(udtEHSPersonalInfo.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'DOB
        Dim strExactDOB As String = udtEHSPersonalInfo.ExactDOB
        Dim strDOB As String = udtformatter.formatInputDate(udtEHSPersonalInfo.DOB)
        Dim dtmDOB As Date = udtEHSPersonalInfo.DOB

        udtSM = Me.udtValidator.chkDOB(DocTypeModel.DocTypeCode.ADOPC, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        Return isValid
    End Function

    'DI
    Private Function Validate_DI(ByRef udtEHSPersonalInfo As EHSPersonalInformationModel) As Boolean
        Dim isValid As Boolean = True
        Dim udtSM As SystemMessage = Nothing
        Dim udtformatter As New Common.Format.Formatter

        'TravelDocNo
        udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.DI, udtEHSPersonalInfo.IdentityNum, String.Empty)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'DOB
        Dim strExactDOB As String = udtEHSPersonalInfo.ExactDOB
        Dim strDOB As String = udtformatter.formatInputDate(udtEHSPersonalInfo.DOB)
        Dim dtmDOB As Date = udtEHSPersonalInfo.DOB

        udtSM = Me.udtValidator.chkDOB(DocTypeModel.DocTypeCode.DI, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'English Name
        udtSM = Me.udtValidator.chkEngName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'Gender
        udtSM = Me.udtValidator.chkGender(udtEHSPersonalInfo.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'DOI
        If udtEHSPersonalInfo.DateofIssue Is Nothing Then
            isValid = False
        Else
            Dim strDOI As String = String.Empty
            strDOI = udtformatter.formatInputDate(udtEHSPersonalInfo.DateofIssue.Value)

            udtSM = udtValidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.REPMT, strDOI, dtmDOB)
            If Not IsNothing(udtSM) Then
                isValid = False
            End If
        End If
        Return isValid
    End Function

    'ID235B
    Private Function Validate_ID235B(ByRef udtEHSPersonalInfo As EHSPersonalInformationModel) As Boolean
        Dim isValid As Boolean = True
        Dim udtSM As SystemMessage = Nothing
        Dim udtformatter As New Common.Format.Formatter

        'BirthEntryNo
        udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.ID235B, udtEHSPersonalInfo.IdentityNum, String.Empty)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'DOB
        Dim strExactDOB As String = udtEHSPersonalInfo.ExactDOB
        Dim strDOB As String = udtformatter.formatInputDate(udtEHSPersonalInfo.DOB)
        Dim dtmDOB As Date = udtEHSPersonalInfo.DOB

        udtSM = Me.udtValidator.chkDOB(DocTypeModel.DocTypeCode.ID235B, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'English Name
        udtSM = Me.udtValidator.chkEngName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'Gender
        udtSM = Me.udtValidator.chkGender(udtEHSPersonalInfo.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        ''Permit to remain until        
        If udtEHSPersonalInfo.PermitToRemainUntil Is Nothing Then
            isValid = False
        Else
            Dim strPermit As String = String.Empty
            strPermit = udtformatter.formatInputDate(udtEHSPersonalInfo.PermitToRemainUntil.Value)

            udtSM = udtValidator.chkPremitToRemainUntil(strPermit, dtmDOB, DocType.DocTypeModel.DocTypeCode.ID235B)
            If Not IsNothing(udtSM) Then
                isValid = False
            End If
        End If

        Return isValid
    End Function

    'Re-entry Permit
    Private Function Validate_ReEntryPermit(ByRef udtEHSPersonalInfo As EHSPersonalInformationModel) As Boolean
        Dim isValid As Boolean = True
        Dim udtSM As SystemMessage = Nothing
        Dim udtformatter As New Common.Format.Formatter

        'REPMT No.
        udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.REPMT, udtEHSPersonalInfo.IdentityNum, String.Empty)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'DOB
        Dim strExactDOB As String = udtEHSPersonalInfo.ExactDOB
        Dim strDOB As String = udtformatter.formatInputDate(udtEHSPersonalInfo.DOB)
        Dim dtmDOB As Date = udtEHSPersonalInfo.DOB

        udtSM = Me.udtValidator.chkDOB(DocTypeModel.DocTypeCode.REPMT, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'English Name
        udtSM = Me.udtValidator.chkEngName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'Gender
        udtSM = Me.udtValidator.chkGender(udtEHSPersonalInfo.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'DOI        
        If udtEHSPersonalInfo.DateofIssue Is Nothing Then
            isValid = False
        Else
            Dim strDOI As String = String.Empty
            strDOI = udtformatter.formatInputDate(udtEHSPersonalInfo.DateofIssue.Value)

            udtSM = udtValidator.chkDataOfIssue(DocType.DocTypeModel.DocTypeCode.REPMT, strDOI, dtmDOB)
            If Not IsNothing(udtSM) Then
                isValid = False
            End If
        End If

        Return isValid
    End Function

    'Visa
    Private Function Validate_Visa(ByRef udtEHSPersonalInfo As EHSPersonalInformationModel) As Boolean
        Dim isValid As Boolean = True
        Dim udtSM As SystemMessage = Nothing
        Dim udtformatter As New Common.Format.Formatter

        'VISA No.
        udtSM = Me.udtValidator.chkIdentityNumber(DocTypeModel.DocTypeCode.VISA, udtEHSPersonalInfo.IdentityNum, String.Empty)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'VISA
        If udtEHSPersonalInfo.Foreign_Passport_No.Equals(String.Empty) Then
            isValid = False
        End If

        'DOB
        Dim strExactDOB As String = udtEHSPersonalInfo.ExactDOB
        Dim strDOB As String = udtformatter.formatInputDate(udtEHSPersonalInfo.DOB)
        Dim dtmDOB As Date = udtEHSPersonalInfo.DOB

        udtSM = Me.udtValidator.chkDOB(DocTypeModel.DocTypeCode.VISA, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'English Name
        udtSM = Me.udtValidator.chkEngName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'Gender
        udtSM = Me.udtValidator.chkGender(udtEHSPersonalInfo.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        Return isValid
    End Function

    ' CRE19-001 (VSS 2019) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    ' Non EHS Doc Type
    Private Function Validate_NonEHSDocType(ByRef udtEHSPersonalInfo As EHSPersonalInformationModel) As Boolean
        Dim isValid As Boolean = True
        Dim udtSM As SystemMessage = Nothing
        Dim udtformatter As New Common.Format.Formatter

        'Doc No.
        udtSM = Me.udtValidator.chkIdentityNumber(udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, String.Empty)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'DOB
        Dim strExactDOB As String = udtEHSPersonalInfo.ExactDOB
        Dim strDOB As String = udtformatter.formatInputDate(udtEHSPersonalInfo.DOB)
        Dim dtmDOB As Date = udtEHSPersonalInfo.DOB

        udtSM = Me.udtValidator.chkDOB(DocTypeModel.DocTypeCode.HKBC, strDOB, dtmDOB, strExactDOB)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'English Name
        udtSM = Me.udtValidator.chkEngName(udtEHSPersonalInfo.ENameSurName, udtEHSPersonalInfo.ENameFirstName)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        'Gender
        udtSM = Me.udtValidator.chkGender(udtEHSPersonalInfo.Gender)
        If Not IsNothing(udtSM) Then
            isValid = False
        End If

        Return isValid
    End Function
    ' CRE19-001 (VSS 2019) [End][Winnie]

    Public Function CheckFieldLimit(ByVal udtPersonalInfo As EHSPersonalInformationModel) As Boolean
        Dim blnValid As Boolean = True

        If udtPersonalInfo.DocCode = DocTypeModel.DocTypeCode.ADOPC Then
            If udtPersonalInfo.AdoptionPrefixNum = String.Empty OrElse udtPersonalInfo.AdoptionPrefixNum.Length > EHSAccountModel.AdoptionPrefixNum_DataSize Then
                blnValid = False
            End If

            If udtPersonalInfo.IdentityNum = String.Empty Then
                blnValid = False
            End If
        End If

        Return blnValid
    End Function

    ''' <summary>
    ''' Check for temp account is converted to validated acct
    ''' Overwrite student entry with validate acct personal info
    ''' Will not update Acc_Validation_Result, Validated_Acc_Unmatch_Result
    ''' </summary>
    ''' <param name="strAccProcessStage"></param>
    ''' <param name="udtDB"></param>
    ''' <remarks></remarks>
    Public Sub CheckTempAccountLatestResult(ByVal strAccProcessStage As String, Optional udtDB As Database = Nothing)

        If IsNothing(udtDB) Then udtDB = New Database

        Dim prams() As SqlParameter = {
            udtDB.MakeInParam("@Acc_Process_Stage", SqlDbType.VarChar, 20, strAccProcessStage)
        }

        udtDB.RunProc("proc_StudentAccountMatching_check_TempAcc", prams)

    End Sub
#End Region

#Region "Student Function"

    Public Function GetStudentFileEntryList(ByVal strFileID As String, ByVal eStudentFileLocation As StudentFileLocation) As StudentFileEntryModelCollection

        Dim udtStudentFileEntryList As New StudentFileEntryModelCollection
        Dim udtStudentFileBLL As New StudentFileBLL

        Select Case eStudentFileLocation
            Case StudentFileLocation.Staging
                udtStudentFileEntryList = udtStudentFileBLL.GetStudentFileEntryStaging(strFileID)

            Case StudentFileLocation.Permanence
                udtStudentFileEntryList = udtStudentFileBLL.GetStudentFileEntry(strFileID)

        End Select

        Return udtStudentFileEntryList
    End Function
#End Region
End Class
