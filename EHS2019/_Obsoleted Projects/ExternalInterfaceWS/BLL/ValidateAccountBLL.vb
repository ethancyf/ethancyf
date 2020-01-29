Imports System.Data.SqlClient
Imports Common.DataAccess
Imports Common.ComFunction
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountBLL
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports Common.ComObject
Imports Common.Component.DocType
Imports Common.Component.ServiceProvider
Imports Common.Validation
Imports ExternalInterfaceWS.BLL.UploadClaimBLL
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports ExternalInterfaceWS.ComObject

Namespace BLL
    Public Class ValidateAccountBLL

        Private udtSM As Common.ComObject.SystemMessage
        Private udtvalidator As Validator = New Validator
        Private udtformatter As Common.Format.Formatter = New Common.Format.Formatter

        Public Class ValidateAccountLookupResult
            Public Const AccountFound As String = "AccountFound"
            Public Const InfoNotMatch As String = "InfoNotMatch"
            Public Const AccountNotFound As String = "AccountNotFound"
            Public Const TempAccountFound As String = "TempAccountFound"
            Public Const Deceased As String = "Deceased"
        End Class

        Public Class ValidateAccountField
            Public Const Prefix As String = "Prefix"
            Public Const IdentityNum As String = "IdentityNum"
            Public Const DOB As String = "DOB"
            Public Const DateofIssue As String = "DateofIssue"
            Public Const Gender As String = "Gender"
            Public Const EName As String = "EName"
            Public Const ECSerialNo As String = "ECSerialNo"
            Public Const ECReferenceNo As String = "ECReferenceNo"
            Public Const ECAge As String = "ECAge"
            Public Const ECDateOfRegistration As String = "ECDateOfRegistration"
            Public Const CName As String = "CName"
            Public Const DOBTypeSelected As String = "DOBTypeSelected"
            Public Const DOBInWord As String = "DOBInWord"
            Public Const PermitToRemainUntil As String = "PermitToRemainUntil"
            Public Const PassportNo As String = "PassportNo"
            Public Const ExactDOB As String = "Exact DOB"
        End Class

        <Serializable()> _
            Public Class SearchAccountStatus
            Public NotMatchAccountExist As Boolean
            Public ExceedDocTypeLimit As Boolean
            Public TempAccountNotMatchDOBFound As Boolean
            Public TempAccountInputDetailDiffFound As Boolean
            Public OnlyInvalidAccountFound As Boolean

            Public Sub New()
                NotMatchAccountExist = False
                ExceedDocTypeLimit = False
                TempAccountNotMatchDOBFound = False
                TempAccountInputDetailDiffFound = False
                OnlyInvalidAccountFound = False
            End Sub

        End Class


#Region "Match DB Validated Account"
        Private Function MatchAccount_HKID(ByVal udtDBEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtInputEHSPersonalInfo As EHSPersonalInformationModel) As List(Of String)
            'Dim blnIsValid As Boolean = True
            Dim udtUnmatchFieldList As New List(Of String)

            If udtDBEHSPersonalInfo.IdentityNum.Trim() <> udtInputEHSPersonalInfo.IdentityNum.Trim() Then
                udtUnmatchFieldList.Add(ValidateAccountField.IdentityNum)
            End If
            If udtDBEHSPersonalInfo.DOB <> udtInputEHSPersonalInfo.DOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.DOB)
            End If
            If udtDBEHSPersonalInfo.DateofIssue.Value <> udtInputEHSPersonalInfo.DateofIssue.Value Then
                udtUnmatchFieldList.Add(ValidateAccountField.DateofIssue)
            End If
            If udtDBEHSPersonalInfo.Gender <> udtInputEHSPersonalInfo.Gender Then
                udtUnmatchFieldList.Add(ValidateAccountField.Gender)
            End If
            If Not IsNameMatch(udtDBEHSPersonalInfo.ENameSurName + udtDBEHSPersonalInfo.ENameFirstName, udtInputEHSPersonalInfo.ENameSurName + udtInputEHSPersonalInfo.ENameFirstName) Then
                udtUnmatchFieldList.Add(ValidateAccountField.EName)
            End If
            If udtDBEHSPersonalInfo.ExactDOB <> udtInputEHSPersonalInfo.ExactDOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.ExactDOB)
            End If

            Return udtUnmatchFieldList
        End Function
        Private Function MatchAccount_EC(ByVal udtDBEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtInputEHSPersonalInfo As EHSPersonalInformationModel) As List(Of String)
            'Dim blnIsValid As Boolean = True
            Dim udtUnmatchFieldList As New List(Of String)

            If udtDBEHSPersonalInfo.ECSerialNo.Trim() <> udtInputEHSPersonalInfo.ECSerialNo.Trim() Then
                udtUnmatchFieldList.Add(ValidateAccountField.ECSerialNo)
            End If
            If udtDBEHSPersonalInfo.ECReferenceNo.Trim() <> udtInputEHSPersonalInfo.ECReferenceNo.Trim() Then
                udtUnmatchFieldList.Add(ValidateAccountField.ECReferenceNo)
            End If
            If udtDBEHSPersonalInfo.DateofIssue.Value <> udtInputEHSPersonalInfo.DateofIssue.Value Then
                udtUnmatchFieldList.Add(ValidateAccountField.DateofIssue)
            End If
            If Not IsNameMatch(udtDBEHSPersonalInfo.ENameSurName + udtDBEHSPersonalInfo.ENameFirstName, udtInputEHSPersonalInfo.ENameSurName + udtInputEHSPersonalInfo.ENameFirstName) Then
                udtUnmatchFieldList.Add(ValidateAccountField.EName)
            End If
            If udtDBEHSPersonalInfo.Gender <> udtInputEHSPersonalInfo.Gender Then
                udtUnmatchFieldList.Add(ValidateAccountField.Gender)
            End If
            If udtDBEHSPersonalInfo.IdentityNum.Trim() <> udtInputEHSPersonalInfo.IdentityNum.Trim() Then
                udtUnmatchFieldList.Add(ValidateAccountField.IdentityNum)
            End If

            ''If udtDBEHSPersonalInfo.DOBTypeSelected <> udtInputEHSPersonalInfo.DOBTypeSelected Then
            ''    udtUnmatchFieldList.Add(ValidateAccountField.DOBTypeSelected)
            ''End If

            If udtDBEHSPersonalInfo.DOB <> udtInputEHSPersonalInfo.DOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.DOB)
            End If
            'If udtDBEHSPersonalInfo.DOBTypeSelected Then
            '    If udtDBEHSPersonalInfo.DOB <> udtInputEHSPersonalInfo.DOB Then
            '        udtUnmatchFieldList.Add(ValidateAccountField.DOB)
            '    End If
            'End If

            If udtInputEHSPersonalInfo.ECAge.HasValue Then
                If udtDBEHSPersonalInfo.ECAge.HasValue <> udtInputEHSPersonalInfo.ECAge.HasValue Then
                    udtUnmatchFieldList.Add(ValidateAccountField.ECAge)
                ElseIf udtDBEHSPersonalInfo.ECAge.Value <> udtInputEHSPersonalInfo.ECAge.Value Then
                    udtUnmatchFieldList.Add(ValidateAccountField.ECAge)
                End If
            End If

            If udtInputEHSPersonalInfo.ECDateOfRegistration.HasValue Then
                If udtDBEHSPersonalInfo.ECDateOfRegistration.HasValue <> udtInputEHSPersonalInfo.ECDateOfRegistration.HasValue Then
                    udtUnmatchFieldList.Add(ValidateAccountField.ECDateOfRegistration)
                ElseIf udtDBEHSPersonalInfo.ECDateOfRegistration.Value <> udtInputEHSPersonalInfo.ECDateOfRegistration.Value Then
                    udtUnmatchFieldList.Add(ValidateAccountField.ECDateOfRegistration)
                End If
            End If

            'If Not udtInputEHSPersonalInfo.CName Is Nothing Then
            '    If udtDBEHSPersonalInfo.CName.Trim <> udtInputEHSPersonalInfo.CName.Trim Then
            '        udtUnmatchFieldList.Add(ValidateAccountField.CName)
            '    End If
            'End If

            If udtDBEHSPersonalInfo.ExactDOB <> udtInputEHSPersonalInfo.ExactDOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.ExactDOB)
            End If

            Return udtUnmatchFieldList
        End Function
        Private Function MatchAccount_HKBC(ByVal udtDBEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtInputEHSPersonalInfo As EHSPersonalInformationModel) As List(Of String)
            Dim udtUnmatchFieldList As New List(Of String)

            If udtDBEHSPersonalInfo.IdentityNum.Trim() <> udtInputEHSPersonalInfo.IdentityNum.Trim() Then
                udtUnmatchFieldList.Add(ValidateAccountField.IdentityNum)
            End If
            If Not IsNameMatch(udtDBEHSPersonalInfo.ENameSurName + udtDBEHSPersonalInfo.ENameFirstName, udtInputEHSPersonalInfo.ENameSurName + udtInputEHSPersonalInfo.ENameFirstName) Then
                udtUnmatchFieldList.Add(ValidateAccountField.EName)
            End If
            If udtDBEHSPersonalInfo.Gender <> udtInputEHSPersonalInfo.Gender Then
                udtUnmatchFieldList.Add(ValidateAccountField.Gender)
            End If
            If udtDBEHSPersonalInfo.DOB <> udtInputEHSPersonalInfo.DOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.DOB)
            End If
            If Not udtDBEHSPersonalInfo.OtherInfo Is Nothing Then
                If udtDBEHSPersonalInfo.OtherInfo <> udtInputEHSPersonalInfo.OtherInfo Then
                    udtUnmatchFieldList.Add(ValidateAccountField.DOBInWord)
                End If
            Else
                If Not udtInputEHSPersonalInfo.OtherInfo Is Nothing Then
                    udtUnmatchFieldList.Add(ValidateAccountField.DOBInWord)
                End If
            End If

            If udtDBEHSPersonalInfo.ExactDOB <> udtInputEHSPersonalInfo.ExactDOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.ExactDOB)
            End If

            Return udtUnmatchFieldList
        End Function
        Private Function MatchAccount_ID235B(ByVal udtDBEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtInputEHSPersonalInfo As EHSPersonalInformationModel) As List(Of String)
            Dim udtUnmatchFieldList As New List(Of String)

            If udtDBEHSPersonalInfo.IdentityNum.Trim() <> udtInputEHSPersonalInfo.IdentityNum.Trim() Then
                udtUnmatchFieldList.Add(ValidateAccountField.IdentityNum)
            End If
            If Not IsNameMatch(udtDBEHSPersonalInfo.ENameSurName + udtDBEHSPersonalInfo.ENameFirstName, udtInputEHSPersonalInfo.ENameSurName + udtInputEHSPersonalInfo.ENameFirstName) Then
                udtUnmatchFieldList.Add(ValidateAccountField.EName)
            End If
            If udtDBEHSPersonalInfo.Gender <> udtInputEHSPersonalInfo.Gender Then
                udtUnmatchFieldList.Add(ValidateAccountField.Gender)
            End If
            If udtDBEHSPersonalInfo.DOB <> udtInputEHSPersonalInfo.DOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.DOB)
            End If
            If udtDBEHSPersonalInfo.PermitToRemainUntil.Value <> udtInputEHSPersonalInfo.PermitToRemainUntil.Value Then
                udtUnmatchFieldList.Add(ValidateAccountField.PermitToRemainUntil)
            End If
            If udtDBEHSPersonalInfo.ExactDOB <> udtInputEHSPersonalInfo.ExactDOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.ExactDOB)
            End If

            Return udtUnmatchFieldList
        End Function
        Private Function MatchAccount_REPMT(ByVal udtDBEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtInputEHSPersonalInfo As EHSPersonalInformationModel) As List(Of String)
            Dim udtUnmatchFieldList As New List(Of String)

            If udtDBEHSPersonalInfo.IdentityNum.Trim() <> udtInputEHSPersonalInfo.IdentityNum.Trim() Then
                udtUnmatchFieldList.Add(ValidateAccountField.IdentityNum)
            End If
            If Not IsNameMatch(udtDBEHSPersonalInfo.ENameSurName + udtDBEHSPersonalInfo.ENameFirstName, udtInputEHSPersonalInfo.ENameSurName + udtInputEHSPersonalInfo.ENameFirstName) Then
                udtUnmatchFieldList.Add(ValidateAccountField.EName)
            End If
            If udtDBEHSPersonalInfo.Gender <> udtInputEHSPersonalInfo.Gender Then
                udtUnmatchFieldList.Add(ValidateAccountField.Gender)
            End If
            If udtDBEHSPersonalInfo.DOB <> udtInputEHSPersonalInfo.DOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.DOB)
            End If
            If udtDBEHSPersonalInfo.DateofIssue.Value <> udtInputEHSPersonalInfo.DateofIssue.Value Then
                udtUnmatchFieldList.Add(ValidateAccountField.DateofIssue)
            End If
            If udtDBEHSPersonalInfo.ExactDOB <> udtInputEHSPersonalInfo.ExactDOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.ExactDOB)
            End If

            Return udtUnmatchFieldList
        End Function
        Private Function MatchAccount_VISA(ByVal udtDBEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtInputEHSPersonalInfo As EHSPersonalInformationModel) As List(Of String)
            Dim udtUnmatchFieldList As New List(Of String)
            If udtDBEHSPersonalInfo.IdentityNum.Trim() <> udtInputEHSPersonalInfo.IdentityNum.Trim() Then
                udtUnmatchFieldList.Add(ValidateAccountField.IdentityNum)
            End If
            If Not IsNameMatch(udtDBEHSPersonalInfo.ENameSurName + udtDBEHSPersonalInfo.ENameFirstName, udtInputEHSPersonalInfo.ENameSurName + udtInputEHSPersonalInfo.ENameFirstName) Then
                udtUnmatchFieldList.Add(ValidateAccountField.EName)
            End If
            If udtDBEHSPersonalInfo.Gender <> udtInputEHSPersonalInfo.Gender Then
                udtUnmatchFieldList.Add(ValidateAccountField.Gender)
            End If
            If udtDBEHSPersonalInfo.DOB <> udtInputEHSPersonalInfo.DOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.DOB)
            End If
            If udtDBEHSPersonalInfo.Foreign_Passport_No.Trim() <> udtInputEHSPersonalInfo.Foreign_Passport_No.Trim() Then
                udtUnmatchFieldList.Add(ValidateAccountField.PassportNo)
            End If
            If udtDBEHSPersonalInfo.ExactDOB <> udtInputEHSPersonalInfo.ExactDOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.ExactDOB)
            End If

            Return udtUnmatchFieldList
        End Function
        Private Function MatchAccount_ADOPC(ByVal udtDBEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtInputEHSPersonalInfo As EHSPersonalInformationModel) As List(Of String)
            Dim udtUnmatchFieldList As New List(Of String)
            If udtDBEHSPersonalInfo.AdoptionPrefixNum.Trim() <> udtInputEHSPersonalInfo.AdoptionPrefixNum.Trim() Then
                udtUnmatchFieldList.Add(ValidateAccountField.Prefix)
            End If
            If udtDBEHSPersonalInfo.IdentityNum.Trim() <> udtInputEHSPersonalInfo.IdentityNum.Trim() Then
                udtUnmatchFieldList.Add(ValidateAccountField.IdentityNum)
            End If
            If Not IsNameMatch(udtDBEHSPersonalInfo.ENameSurName + udtDBEHSPersonalInfo.ENameFirstName, udtInputEHSPersonalInfo.ENameSurName + udtInputEHSPersonalInfo.ENameFirstName) Then
                udtUnmatchFieldList.Add(ValidateAccountField.EName)
            End If
            If udtDBEHSPersonalInfo.Gender <> udtInputEHSPersonalInfo.Gender Then
                udtUnmatchFieldList.Add(ValidateAccountField.Gender)
            End If
            If udtDBEHSPersonalInfo.DOB <> udtInputEHSPersonalInfo.DOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.DOB)
            End If
            If Not udtDBEHSPersonalInfo.OtherInfo Is Nothing Then
                If udtDBEHSPersonalInfo.OtherInfo <> udtInputEHSPersonalInfo.OtherInfo Then
                    udtUnmatchFieldList.Add(ValidateAccountField.DOBInWord)
                End If
            Else
                If Not udtInputEHSPersonalInfo.OtherInfo Is Nothing Then
                    udtUnmatchFieldList.Add(ValidateAccountField.DOBInWord)
                End If
            End If
            If udtDBEHSPersonalInfo.ExactDOB <> udtInputEHSPersonalInfo.ExactDOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.ExactDOB)
            End If

            Return udtUnmatchFieldList
        End Function
        Private Function MatchAccount_DI(ByVal udtDBEHSPersonalInfo As EHSPersonalInformationModel, ByVal udtInputEHSPersonalInfo As EHSPersonalInformationModel) As List(Of String)
            Dim udtUnmatchFieldList As New List(Of String)

            If udtDBEHSPersonalInfo.IdentityNum.Trim() <> udtInputEHSPersonalInfo.IdentityNum.Trim() Then
                udtUnmatchFieldList.Add(ValidateAccountField.IdentityNum)
            End If
            If Not IsNameMatch(udtDBEHSPersonalInfo.ENameSurName + udtDBEHSPersonalInfo.ENameFirstName, udtInputEHSPersonalInfo.ENameSurName + udtInputEHSPersonalInfo.ENameFirstName) Then
                udtUnmatchFieldList.Add(ValidateAccountField.EName)
            End If
            If udtDBEHSPersonalInfo.Gender <> udtInputEHSPersonalInfo.Gender Then
                udtUnmatchFieldList.Add(ValidateAccountField.Gender)
            End If
            If udtDBEHSPersonalInfo.DOB <> udtInputEHSPersonalInfo.DOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.DOB)
            End If
            If udtDBEHSPersonalInfo.DateofIssue.Value <> udtInputEHSPersonalInfo.DateofIssue.Value Then
                udtUnmatchFieldList.Add(ValidateAccountField.DateofIssue)
            End If
            If udtDBEHSPersonalInfo.ExactDOB <> udtInputEHSPersonalInfo.ExactDOB Then
                udtUnmatchFieldList.Add(ValidateAccountField.ExactDOB)
            End If

            Return udtUnmatchFieldList
        End Function
#End Region


        Public Function ValidatedAccountQuery(ByVal udtDB As Database, ByVal udtInputEHSAccount As EHSAccountModel, _
                                     ByVal strDocCode As String, ByVal strIdentityNum As String, _
                                     Optional ByRef udtDBEHSAccount As EHSAccountModel = Nothing, _
                                     Optional ByRef udtUploadErrorList As ErrorInfoModelCollection = Nothing, _
                                     Optional ByRef _udtAuditLogEntry As AuditLogEntry = Nothing, _
                                     Optional ByRef udtUnmatchFieldList As List(Of String) = Nothing) As String

            Return ValidatedAccountQuery(udtDB, udtInputEHSAccount.getPersonalInformation(strDocCode), strDocCode, strIdentityNum, udtDBEHSAccount, _
                                  udtUploadErrorList, _udtAuditLogEntry, udtUnmatchFieldList)
        End Function

        Public Function ValidatedAccountQuery(ByVal udtDB As Database, ByVal udtInputPersonalInfo As EHSPersonalInformationModel, _
                             ByVal strDocCode As String, ByVal strIdentityNum As String, _
                             Optional ByRef udtDBEHSAccount As EHSAccountModel = Nothing, _
                             Optional ByRef udtUploadErrorList As ErrorInfoModelCollection = Nothing, _
                             Optional ByRef _udtAuditLogEntry As ExtAuditLogEntry = Nothing, _
                             Optional ByRef udtUnmatchFieldList As List(Of String) = Nothing) As String

            If Not _udtAuditLogEntry Is Nothing Then
                _udtAuditLogEntry.AddDescripton("IdentityNum", strIdentityNum)
                _udtAuditLogEntry.AddDescripton("DocCode", strDocCode)
                _udtAuditLogEntry.WriteLog_Ext(Common.Component.LogID.LOG00140)
            End If

            'If Not udtUploadErrorList Is Nothing Then
            '    udtUploadErrorList = New ErrorInfoModelCollection()
            'End If

            ValidateInformation(udtUploadErrorList, strDocCode, strIdentityNum, udtInputPersonalInfo)

            ' Load Account from database
            Dim strValidateAccountLookupResult As String = ""
            Dim udtEHSAccountBLL As New EHSAccountBLL
            udtDBEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(strIdentityNum, strDocCode)

            If Not _udtAuditLogEntry Is Nothing Then
                _udtAuditLogEntry.WriteLog_Ext(Common.Component.LogID.LOG00141)
            End If
            If udtDBEHSAccount Is Nothing Then
                ' Account Not Found

                ' -------------------------------------------------------------------------------
                ' Search Temporary Account, Check Account Status
                ' -------------------------------------------------------------------------------
                Dim strSearchDocCode As String = ""
                Dim udtSearchAccountStatus As New SearchAccountStatus()
                Dim udtDBTempEHSAccount As EHSAccountModel = Nothing
                Dim blnNoMatchTempAccountFound As Boolean = True

                'Me.SearchTemporaryAccount(strDocCode, strIdentityNum, udtInputPersonalInfo.DOB, udtInputPersonalInfo.ExactDOB, udtDBEHSAccount, _
                '    udtSearchAccountStatus, Nothing, Nothing, udtInputPersonalInfo.AdoptionPrefixNum, strSearchDocCode, _
                '    udtInputPersonalInfo)        

                If udtInputPersonalInfo.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                    Me.SearchTemporaryAccount(strDocCode, strIdentityNum, Nothing, Nothing, udtDBTempEHSAccount, _
                        udtSearchAccountStatus, udtInputPersonalInfo.ECAge.Value, udtInputPersonalInfo.ECDateOfRegistration, Nothing, strSearchDocCode, Nothing)
                Else
                    'Me.SearchTemporaryAccount(strDocCode, strIdentityNum, udtInputPersonalInfo.DOB, udtInputPersonalInfo.ExactDOB, udtDBTempEHSAccount, _
                    'udtSearchAccountStatus, Nothing, Nothing, udtInputPersonalInfo.AdoptionPrefixNum.Trim(), strSearchDocCode, Nothing)

                    If udtInputPersonalInfo.AdoptionPrefixNum Is Nothing Then
                        Me.SearchTemporaryAccount(strDocCode, strIdentityNum, udtInputPersonalInfo.DOB, udtInputPersonalInfo.ExactDOB, udtDBTempEHSAccount, _
                        udtSearchAccountStatus, Nothing, Nothing, "", strSearchDocCode, Nothing)
                    Else
                        Me.SearchTemporaryAccount(strDocCode, strIdentityNum, udtInputPersonalInfo.DOB, udtInputPersonalInfo.ExactDOB, udtDBTempEHSAccount, _
                        udtSearchAccountStatus, Nothing, Nothing, udtInputPersonalInfo.AdoptionPrefixNum, strSearchDocCode, Nothing)
                    End If
                End If

                If Not _udtAuditLogEntry Is Nothing Then
                    _udtAuditLogEntry.WriteLog_Ext(Common.Component.LogID.LOG00142)
                End If
                'blnNoMatchTempAccountFound = udtSearchAccountStatus.OnlyInvalidAccountFound OrElse udtSearchAccountStatus.TempAccountNotMatchDOBFound

                'blnNoMatchTempAccountFound = udtSearchAccountStatus.OnlyInvalidAccountFound

                If Not udtDBTempEHSAccount Is Nothing Then
                    'If blnNoMatchTempAccountFound Or (Not IsPersonalInfoMatchWithDB(udtUnmatchFieldList, strDocCode, udtDBTempEHSAccount.getPersonalInformation(strDocCode), udtInputPersonalInfo)) Then
                    'If blnNoMatchTempAccountFound Then
                    '    strValidateAccountLookupResult = ValidateAccountLookupResult.AccountNotFound
                    'Else

                    ' CRE11-007
                    ' Check death record
                    If udtDBTempEHSAccount.DeathRecord.IsDead Then
                        strValidateAccountLookupResult = ValidateAccountLookupResult.Deceased
                    Else
                        strValidateAccountLookupResult = ValidateAccountLookupResult.TempAccountFound
                    End If

                    'End If
                Else
                    strValidateAccountLookupResult = ValidateAccountLookupResult.AccountNotFound
                End If
            Else
                Dim udtDBPersonalInfo As EHSPersonalInformationModel
                udtDBPersonalInfo = udtDBEHSAccount.getPersonalInformation(strDocCode)

                If Not udtDBEHSAccount.RecordStatus.Equals(EHSAccountModel.ValidatedAccountRecordStatusClass.Active) Then
                    ' If the account found is inactive
                    strValidateAccountLookupResult = ValidateAccountLookupResult.InfoNotMatch
                Else
                    If Not IsPersonalInfoMatchWithDB(udtUnmatchFieldList, strDocCode, udtDBPersonalInfo, udtInputPersonalInfo) Then
                        If Not udtUploadErrorList Is Nothing Then
                            udtUploadErrorList.Add(UploadErrorCode.ValidateAcInfoNotMatch)
                        End If
                        strValidateAccountLookupResult = ValidateAccountLookupResult.InfoNotMatch
                    Else

                        ' CRE11-007
                        ' Check death record
                        If udtDBEHSAccount.DeathRecord.IsDead Then
                            strValidateAccountLookupResult = ValidateAccountLookupResult.Deceased
                        Else
                            strValidateAccountLookupResult = ValidateAccountLookupResult.AccountFound
                        End If

                    End If

                End If
            End If

            If Not _udtAuditLogEntry Is Nothing Then
                _udtAuditLogEntry.AddDescripton("ValidateAccountLookupResult", strValidateAccountLookupResult)
                If Not udtUnmatchFieldList Is Nothing Then
                    For Each strUnmatchField As String In udtUnmatchFieldList
                        _udtAuditLogEntry.AddDescripton("Field Not Match", strUnmatchField)
                    Next
                End If
                _udtAuditLogEntry.WriteLog_Ext(Common.Component.LogID.LOG00143)
            End If

            Return strValidateAccountLookupResult
        End Function

        Public Function IsNameMatch(ByVal strName1, ByVal strName2) As Boolean
            Dim blnIsMatch As Boolean = False
            strName1 = strName1.Replace(" ", "").Replace("-", "").Replace("/", "")
            strName2 = strName2.Replace(" ", "").Replace("-", "").Replace("/", "")
            If strName1 = strName2 Then
                blnIsMatch = True
            End If

            Return blnIsMatch
        End Function


        Private Function IsPersonalInfoMatchWithDB(ByRef udtUnmatchFieldList As List(Of String), ByVal strDocCode As String, ByVal udtDBPersonalInfo As EHSPersonalInformationModel, ByVal udtInputPersonalInfo As EHSPersonalInformationModel)
            ' Check if the information of validation account match with database value
            Select Case strDocCode
                Case DocTypeModel.DocTypeCode.HKIC
                    udtUnmatchFieldList = Me.MatchAccount_HKID(udtDBPersonalInfo, udtInputPersonalInfo)
                Case DocTypeModel.DocTypeCode.EC
                    udtUnmatchFieldList = Me.MatchAccount_EC(udtDBPersonalInfo, udtInputPersonalInfo)
                Case DocTypeModel.DocTypeCode.HKBC
                    udtUnmatchFieldList = Me.MatchAccount_HKBC(udtDBPersonalInfo, udtInputPersonalInfo)
                Case DocTypeModel.DocTypeCode.ADOPC
                    udtUnmatchFieldList = Me.MatchAccount_ADOPC(udtDBPersonalInfo, udtInputPersonalInfo)
                Case DocTypeModel.DocTypeCode.DI
                    udtUnmatchFieldList = Me.MatchAccount_DI(udtDBPersonalInfo, udtInputPersonalInfo)
                Case DocTypeModel.DocTypeCode.ID235B
                    udtUnmatchFieldList = Me.MatchAccount_ID235B(udtDBPersonalInfo, udtInputPersonalInfo)
                Case DocTypeModel.DocTypeCode.REPMT
                    udtUnmatchFieldList = Me.MatchAccount_REPMT(udtDBPersonalInfo, udtInputPersonalInfo)
                Case DocTypeModel.DocTypeCode.VISA
                    udtUnmatchFieldList = Me.MatchAccount_VISA(udtDBPersonalInfo, udtInputPersonalInfo)
                    ' To Do: Add other document type
            End Select
            If udtUnmatchFieldList.Count > 0 Then
                Return False
            Else
                Return True
            End If
        End Function

        Private Sub ValidateInformation(ByRef udtUploadErrorList As ErrorInfoModelCollection, ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal udtInputPersonalInfo As EHSPersonalInformationModel)
            Dim udtClaimRulesBLL As New Common.Component.ClaimRules.ClaimRulesBLL()
            Dim strMsgCode As String = ""
            ' -------------------------------------------------------------------------------
            ' 3. Check HKIC VS EC ' return messagecode 141 - EC record found, 142 - HKIC record found
            ' -------------------------------------------------------------------------------
            If strMsgCode.Trim() = "" Then
                strMsgCode = udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.ValidateAccount, strDocCode, strIdentityNum)
            End If
            If strMsgCode.Trim() = "" Then
                strMsgCode = udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.TemporaryAccount, strDocCode, strIdentityNum)
            End If
            If strMsgCode.Trim() = "" Then
                strMsgCode = udtClaimRulesBLL.chkEHSAccountHKICVsEC(EHSAccountModel.SysAccountSource.SpecialAccount, strDocCode, strIdentityNum)
            End If

            If Not udtUploadErrorList Is Nothing AndAlso strMsgCode.Trim() <> "" Then
                udtUploadErrorList.Add(UploadErrorCode.HKIC_ECCheck)
            End If
            ' -------------------------------------------------------------------------------
            ' 4. Adoption Checking (Check the Adoption Detail when account is searched  return messagecode 00186 - adoption cert record found
            ' -------------------------------------------------------------------------------
            strMsgCode = ""
            If strMsgCode.Trim() = "" AndAlso strDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                strMsgCode = udtClaimRulesBLL.chkEHSAdoptionCertDetail(strDocCode, strIdentityNum, udtInputPersonalInfo.AdoptionPrefixNum.Trim(), String.Empty, EHSAccountModel.SysAccountSource.ValidateAccount)
            End If

            If strMsgCode.Trim() = "" AndAlso strDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                strMsgCode = udtClaimRulesBLL.chkEHSAdoptionCertDetail(strDocCode, strIdentityNum, udtInputPersonalInfo.AdoptionPrefixNum.Trim(), String.Empty, EHSAccountModel.SysAccountSource.TemporaryAccount)
            End If

            If strMsgCode.Trim() = "" AndAlso strDocCode = DocTypeModel.DocTypeCode.ADOPC Then
                strMsgCode = udtClaimRulesBLL.chkEHSAdoptionCertDetail(strDocCode, strIdentityNum, udtInputPersonalInfo.AdoptionPrefixNum.Trim(), String.Empty, EHSAccountModel.SysAccountSource.SpecialAccount)
            End If

            If Not udtUploadErrorList Is Nothing AndAlso strMsgCode.Trim() <> "" Then
                udtUploadErrorList.Add(UploadErrorCode.AdoptionCheck)
            End If
        End Sub


#Region "Search Temp Account"
        Private Sub SearchTemporaryAccount(ByVal strDocCode As String, ByVal strIdentityNum As String, ByVal dtmDOB As Date, ByVal strExactDOB As String, _
        ByRef udtEHSAccount As EHSAccountModel, ByRef udtSearchAccountStatus As SearchAccountStatus, _
        ByVal intAge As Nullable(Of Integer), ByVal dtmDOR As Nullable(Of Date), ByVal strAdoptionPrefixNum As String, ByRef strSearchDocCode As String, _
        ByVal udtEHSPersonalInfo As EHSPersonalInformationModel)

            Dim udtEHSAccountBLL As New EHSAccountBLL()

            Dim udtEHSAccountModelList As EHSAccountModelCollection = udtEHSAccountBLL.LoadTempEHSAccountByIdentity(strIdentityNum, strDocCode)
            Dim udtTempEHSAccount As EHSAccountModel = Nothing
            Dim blnInvalidTempAccountFound As Boolean = False
            strSearchDocCode = strDocCode.Trim()

            ' -----------------------------------------------------------------------------------
            ' Temporary Account
            ' -----------------------------------------------------------------------------------
            For Each udtCurEHSAccount As EHSAccountModel In udtEHSAccountModelList
                ' --- Checking 1: For Claim or For Validate only ----
                If udtCurEHSAccount.AccountPurpose <> AccountPurposeClass.ForClaim AndAlso _
                        udtCurEHSAccount.AccountPurpose <> AccountPurposeClass.ForValidate Then
                    Continue For
                End If

                ' --- Checking 2: Pending For Confirmation Or Pending For Verify ----
                If udtCurEHSAccount.RecordStatus <> TempAccountRecordStatusClass.PendingConfirmation AndAlso _
                        udtCurEHSAccount.RecordStatus <> TempAccountRecordStatusClass.PendingVerify Then
                    If udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.InValid Then
                        ' Any Invalid Record Found!
                        blnInvalidTempAccountFound = True
                    End If

                    'Continue For
                End If

                ' ---- Checking 3: Not X Account ----
                'If Not IsNothing(udtCurEHSAccount.OriginalAccID) AndAlso udtCurEHSAccount.OriginalAccID.Trim <> String.Empty Then
                '    ' X Account Found: Invalid Record Found
                '    'blnInvalidTempAccountFound = True

                '    'Continue For
                'End If

                ' ---- Checking 4: Match DOB ----
                If intAge.HasValue AndAlso dtmDOR.HasValue Then
                    ' EC Case Report Age on Date of Registration
                    If Not chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), intAge.Value, dtmDOR.Value) Then
                        ' Any Temp Account Not Match DOB Found
                        udtSearchAccountStatus.TempAccountNotMatchDOBFound = True
                    End If

                    If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                        udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                        Continue For
                    End If

                    If udtTempEHSAccount Is Nothing Then
                        udtTempEHSAccount = udtCurEHSAccount
                    Else
                        If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                            udtTempEHSAccount = udtCurEHSAccount
                        End If
                    End If

                Else
                    If Not chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then
                        ' Any Temp Account Not Match DOB Found
                        udtSearchAccountStatus.TempAccountNotMatchDOBFound = True

                        Continue For
                    End If

                    If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                        udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                        Continue For
                    End If

                    If udtTempEHSAccount Is Nothing Then
                        udtTempEHSAccount = udtCurEHSAccount
                    Else
                        '==================================================================== Code for SmartID ============================================================================
                        If udtTempEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocTypeModel.DocTypeCode.HKIC) AndAlso _
                           udtCurEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocTypeModel.DocTypeCode.HKIC) Then

                            If udtTempEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                    If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                        udtTempEHSAccount = udtCurEHSAccount
                                    End If
                                End If
                            Else
                                If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                    udtTempEHSAccount = udtCurEHSAccount
                                End If
                            End If
                        Else
                            If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                udtTempEHSAccount = udtCurEHSAccount
                            End If
                        End If
                        '==================================================================================================================================================================

                        'If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                        '    udtTempEHSAccount = udtCurEHSAccount
                        'End If
                    End If

                End If

            Next

            ' -----------------------------------------------------------------------------------
            ' Special Account
            ' -----------------------------------------------------------------------------------
            udtEHSAccountModelList = udtEHSAccountBLL.LoadSpecialEHSAccountByIdentity(strIdentityNum, strDocCode)
            For Each udtCurEHSAccount As EHSAccountModel In udtEHSAccountModelList
                ' --- Checking 1: For Claim or For Validate only ----
                If udtCurEHSAccount.AccountPurpose <> EHSAccountModel.AccountPurposeClass.ForClaim _
                        AndAlso udtCurEHSAccount.AccountPurpose <> EHSAccountModel.AccountPurposeClass.ForValidate Then
                    Continue For
                End If

                ' --- Checking 2: Pending For Confirmation Or Pending For Verify ----
                If udtCurEHSAccount.RecordStatus <> TempAccountRecordStatusClass.PendingConfirmation AndAlso _
                        udtCurEHSAccount.RecordStatus <> TempAccountRecordStatusClass.PendingVerify Then
                    If udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.InValid Then
                        ' Any Invalid Record Found!
                        blnInvalidTempAccountFound = True
                    End If

                    'Continue For
                End If

                ' ---- Checking 3: Not X Account ----
                'If Not IsNothing(udtCurEHSAccount.TempVouhcerAccID) AndAlso udtCurEHSAccount.TempVouhcerAccID.Trim <> String.Empty Then
                '    ' X Account Found: Invalid Record Found
                '    'blnInvalidTempAccountFound = True

                '    'Continue For
                'End If

                ' ---- Checking 4: Match DOB ----
                If intAge.HasValue AndAlso dtmDOR.HasValue Then
                    ' Actually special account won't have EC

                    ' EC Case Report Age on Date of Registration
                    If Not chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), intAge.Value, dtmDOR.Value) Then
                        ' Any Temp Account Not Match DOB Found
                        udtSearchAccountStatus.TempAccountNotMatchDOBFound = True

                        Continue For
                    End If

                    If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                        udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                        Continue For
                    End If

                    If udtTempEHSAccount Is Nothing Then
                        udtTempEHSAccount = udtCurEHSAccount
                    Else
                        If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                            udtTempEHSAccount = udtCurEHSAccount
                        End If
                    End If

                Else
                    If Not chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then
                        ' Any Temp Account Not Match DOB Found
                        udtSearchAccountStatus.TempAccountNotMatchDOBFound = True

                        Continue For
                    End If

                    If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                        udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                        Continue For
                    End If

                    If udtTempEHSAccount Is Nothing Then
                        udtTempEHSAccount = udtCurEHSAccount
                    Else
                        '==================================================================== Code for SmartID ============================================================================
                        If udtTempEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocTypeModel.DocTypeCode.HKIC) AndAlso _
                           udtCurEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocTypeModel.DocTypeCode.HKIC) Then

                            If udtTempEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                    If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                        udtTempEHSAccount = udtCurEHSAccount
                                    End If
                                End If
                            Else
                                If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                    udtTempEHSAccount = udtCurEHSAccount
                                End If
                            End If
                        Else
                            If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                udtTempEHSAccount = udtCurEHSAccount
                            End If
                        End If
                        '==================================================================================================================================================================

                        'If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                        '    udtTempEHSAccount = udtCurEHSAccount
                        'End If
                    End If

                End If

            Next

            If Not udtTempEHSAccount Is Nothing Then
                udtEHSAccount = udtTempEHSAccount
            End If

            ' -----------------------------------------------------------
            ' Load Temporary Account with HKIC / BirthCert if not Found (HKIC <==> BirthCert)
            ' -----------------------------------------------------------
            If udtEHSAccount Is Nothing Then
                ' -----------------------------------------------------------------------------------
                ' Temporary Account
                ' -----------------------------------------------------------------------------------
                udtEHSAccountModelList = Nothing
                udtTempEHSAccount = Nothing

                If strDocCode = DocTypeModel.DocTypeCode.HKIC Then
                    strSearchDocCode = DocTypeModel.DocTypeCode.HKBC
                    udtEHSAccountModelList = udtEHSAccountBLL.LoadTempEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
                ElseIf strDocCode = DocTypeModel.DocTypeCode.HKBC Then
                    strSearchDocCode = DocTypeModel.DocTypeCode.HKIC
                    udtEHSAccountModelList = udtEHSAccountBLL.LoadTempEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
                End If
                If Not udtEHSAccountModelList Is Nothing AndAlso udtEHSAccountModelList.Count > 0 Then
                    For Each udtCurEHSAccount As EHSAccountModel In udtEHSAccountModelList
                        ' Temporary Account
                        If (udtCurEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForClaim Or _
                            udtCurEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForValidate) Then

                            ' Pending For Confirmation Or Pending For Verify
                            If (udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation Or _
                                udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify) Then

                                ' Not X Account
                                If (udtCurEHSAccount.OriginalAccID Is Nothing OrElse udtCurEHSAccount.OriginalAccID.Trim() = "") Then

                                    ' Match DOB
                                    If intAge.HasValue AndAlso dtmDOR.HasValue Then
                                        'EC Case Report Age on Date of Registration
                                        If Me.chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), intAge.Value, dtmDOR.Value) Then

                                            If udtTempEHSAccount Is Nothing Then
                                                udtTempEHSAccount = udtCurEHSAccount
                                            Else
                                                If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                    udtTempEHSAccount = udtCurEHSAccount
                                                End If
                                            End If
                                        Else
                                            ' Any Temp Account Not Match DOB Found
                                            udtSearchAccountStatus.TempAccountNotMatchDOBFound = True
                                        End If
                                    Else
                                        If Me.chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then
                                            If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                                                udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                                                Continue For
                                            End If

                                            If udtTempEHSAccount Is Nothing Then
                                                udtTempEHSAccount = udtCurEHSAccount
                                            Else
                                                '==================================================================== Code for SmartID ============================================================================
                                                If udtTempEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocTypeModel.DocTypeCode.HKIC) AndAlso _
                                                   udtCurEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocTypeModel.DocTypeCode.HKIC) Then

                                                    If udtTempEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                                        If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                                            If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                                udtTempEHSAccount = udtCurEHSAccount
                                                            End If
                                                        End If
                                                    Else
                                                        If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                                            udtTempEHSAccount = udtCurEHSAccount
                                                        End If
                                                    End If
                                                Else
                                                    If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                        udtTempEHSAccount = udtCurEHSAccount
                                                    End If
                                                End If
                                                '==================================================================================================================================================================

                                                'If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                '    udtTempEHSAccount = udtCurEHSAccount
                                                'End If
                                            End If
                                        Else
                                            ' Any Temp Account Not Match DOB Found
                                            udtSearchAccountStatus.TempAccountNotMatchDOBFound = True
                                        End If
                                    End If
                                Else
                                    ' X Account Found: Invalid Record Found
                                    'blnInvalidTempAccountFound = True
                                End If
                            ElseIf udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.InValid Then
                                ' Any Invalid Record Found!
                                blnInvalidTempAccountFound = True
                            End If
                        End If
                    Next
                End If
                ' -----------------------------------------------------------------------------------
                ' Special Account
                ' -----------------------------------------------------------------------------------

                If strDocCode = DocTypeModel.DocTypeCode.HKIC Then
                    strSearchDocCode = DocTypeModel.DocTypeCode.HKBC
                    udtEHSAccountModelList = udtEHSAccountBLL.LoadSpecialEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
                ElseIf strDocCode = DocTypeModel.DocTypeCode.HKBC Then
                    strSearchDocCode = DocTypeModel.DocTypeCode.HKIC
                    udtEHSAccountModelList = udtEHSAccountBLL.LoadSpecialEHSAccountByIdentity(strIdentityNum, strSearchDocCode)
                End If

                If Not udtEHSAccountModelList Is Nothing AndAlso udtEHSAccountModelList.Count > 0 Then
                    For Each udtCurEHSAccount As EHSAccountModel In udtEHSAccountModelList
                        ' Special Account
                        If (udtCurEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForClaim Or _
                            udtCurEHSAccount.AccountPurpose = EHSAccountModel.AccountPurposeClass.ForValidate) Then

                            ' Special Account must Confiremd
                            ' Pending For Confirmation Or Pending For Verify
                            If (udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingConfirmation Or _
                                udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify) Then

                                ' Not X Account
                                If (udtCurEHSAccount.TempVouhcerAccID Is Nothing OrElse udtCurEHSAccount.TempVouhcerAccID.Trim() = "") Then
                                    ' Match DOB
                                    If intAge.HasValue AndAlso dtmDOR.HasValue Then
                                        'EC Case Report Age on Date of Registration
                                        If Me.chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), intAge.Value, dtmDOR.Value) Then

                                            If udtTempEHSAccount Is Nothing Then
                                                udtTempEHSAccount = udtCurEHSAccount
                                            Else
                                                If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                    udtTempEHSAccount = udtCurEHSAccount
                                                End If
                                            End If
                                        Else
                                            ' Any Temp Account Not Match DOB Found
                                            udtSearchAccountStatus.TempAccountNotMatchDOBFound = True
                                        End If
                                    Else
                                        If Me.chkEHSAccountInputDOBMatch(udtCurEHSAccount.EHSPersonalInformationList(0), dtmDOB, strExactDOB) Then
                                            If Not ChkEHSAccountInputDetailMatch(udtEHSPersonalInfo, udtCurEHSAccount.EHSPersonalInformationList(0)) Then
                                                udtSearchAccountStatus.TempAccountInputDetailDiffFound = True

                                                Continue For
                                            End If

                                            If udtTempEHSAccount Is Nothing Then
                                                udtTempEHSAccount = udtCurEHSAccount
                                            Else
                                                '==================================================================== Code for SmartID ============================================================================
                                                If udtTempEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocTypeModel.DocTypeCode.HKIC) AndAlso _
                                                   udtCurEHSAccount.EHSPersonalInformationList(0).DocCode.Trim.Equals(DocTypeModel.DocTypeCode.HKIC) Then

                                                    If udtTempEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                                        If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                                            If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                                udtTempEHSAccount = udtCurEHSAccount
                                                            End If
                                                        End If
                                                    Else
                                                        If udtCurEHSAccount.EHSPersonalInformationList(0).CreateBySmartID Then
                                                            udtTempEHSAccount = udtCurEHSAccount
                                                        End If
                                                    End If
                                                Else
                                                    If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                        udtTempEHSAccount = udtCurEHSAccount
                                                    End If
                                                End If
                                                '==================================================================================================================================================================

                                                'If udtTempEHSAccount.CreateDtm < udtCurEHSAccount.CreateDtm Then
                                                '    udtTempEHSAccount = udtCurEHSAccount
                                                'End If
                                            End If
                                        Else
                                            ' Any Temp Account Not Match DOB Found
                                            udtSearchAccountStatus.TempAccountNotMatchDOBFound = True
                                        End If
                                    End If
                                Else
                                    ' X Account Record Found!
                                    'blnInvalidTempAccountFound = True
                                End If

                            ElseIf udtCurEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.InValid Then
                                ' Any Invalid Record Found!
                                blnInvalidTempAccountFound = True
                            End If
                        End If
                    Next
                End If

                If Not udtTempEHSAccount Is Nothing Then
                    udtEHSAccount = udtTempEHSAccount
                End If
            End If

            If udtTempEHSAccount Is Nothing Then
                ' Not Match Record Found & Invalid Record Found!
                If blnInvalidTempAccountFound Then
                    udtSearchAccountStatus.OnlyInvalidAccountFound = True
                End If
            End If

        End Sub

        Public Function chkEHSAccountInputDOBMatch(ByRef udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal intAge As Integer, ByVal dtmDOR As Date) As Boolean

            If (udtEHSPersonalInformation.ECAge.HasValue AndAlso udtEHSPersonalInformation.ECDateOfRegistration.HasValue) Then
                If udtEHSPersonalInformation.ECAge.Value = intAge AndAlso udtEHSPersonalInformation.ECDateOfRegistration.Value.Equals(dtmDOR) Then
                    Return True
                Else
                    Return False
                End If
            Else
                Return False
            End If
        End Function

        Public Function chkEHSAccountInputDOBMatch(ByRef udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel, ByVal dtmDOB As Date, ByVal strExactDOB As String) As Boolean

            If (udtEHSPersonalInformation.DOB.Equals(dtmDOB) AndAlso strExactDOB = "Y" AndAlso _
                (udtEHSPersonalInformation.ExactDOB = "V" OrElse udtEHSPersonalInformation.ExactDOB = "Y" OrElse udtEHSPersonalInformation.ExactDOB = "R")) _
                OrElse _
                (udtEHSPersonalInformation.DOB.Equals(dtmDOB) AndAlso strExactDOB = "M" AndAlso _
                (udtEHSPersonalInformation.ExactDOB = "U" OrElse udtEHSPersonalInformation.ExactDOB = "M")) _
                OrElse _
                (udtEHSPersonalInformation.DOB.Equals(dtmDOB) AndAlso strExactDOB = "D" AndAlso _
                (udtEHSPersonalInformation.ExactDOB = "T" OrElse udtEHSPersonalInformation.ExactDOB = "D")) Then
                Return True
            Else
                Return False
            End If

        End Function

        Private Function ChkEHSAccountInputDetailMatch(ByVal udtEHSPersonalInfoPass As EHSPersonalInformationModel, ByVal udtEHSPersonalInfoDB As EHSPersonalInformationModel) As Boolean
            ' If no EHSPersonalInformation is passed, consider it is matched
            If IsNothing(udtEHSPersonalInfoPass) Then Return True

            ' Compare (1) Exact DOB; (2) Gender; (3) Name in English
            If udtEHSPersonalInfoPass.ExactDOB <> udtEHSPersonalInfoDB.ExactDOB Then Return False
            If udtEHSPersonalInfoPass.Gender <> udtEHSPersonalInfoDB.Gender Then Return False
            If udtEHSPersonalInfoPass.EName <> udtEHSPersonalInfoDB.EName Then Return False

            Return True

        End Function
#End Region

    End Class
End Namespace