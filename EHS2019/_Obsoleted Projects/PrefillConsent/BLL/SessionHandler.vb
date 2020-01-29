Imports Common.Component.Practice
Imports Common.Component.Scheme
Imports Common.Component.EHSAccount
Imports Common.Component.DataEntryUser
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSTransaction

Namespace BLL

    Public Class SessionHandler

        Public Class SessionName
            Public Const SESS_LANGUAGE As String = "language"
            Public Const SESS_EHSAccount As String = "SESS_EHSACCOUNT"
            Public Const SESS_SelectedPracticeDsiplay As String = "SESS_SELECTEDPRACTICEDISPLAY"
            Public Const SESS_SelectedPracticeDsiplayList As String = "SESS_SELECTEDPRACTICEDISPLAYLIST"
            Public Const SESS_AccountCreationProceedClaim As String = "SESS_ACCOUNTCREATIONPROCEEDCLAIM"
            Public Const SESS_SchemeSelected As String = "SESS_SCHEMESELECTED"

            Public Const SESS_ServiceProvider As String = "SESS_SERVICEPROVIDER"
            Public Const SESS_DataEntry As String = "SESS_DATAENTRY"

            '------------------------------------------------------------------------------------------
            'EHS Claim Session
            '------------------------------------------------------------------------------------------
            Public Const SESS_NotMatchTempAccountExist As String = "SESS_NOTMATCHTEMPACCOUNTEXIST"
            Public Const SESS_ExceedDocTypeLimit As String = "SESS_EXCEEDDOCTYPELIMIT"

            Public Const SESS_EHSClaimVaccine As String = "SESS_EHSCLAIMVACCINE"
            Public Const SESS_EHSTransaction As String = "SESS_EHSTRANSACTION"

            Public Const SESS_CCCode1 As String = "SESS_CHOOSECCCODE_CCCODE1"
            Public Const SESS_CCCode2 As String = "SESS_CHOOSECCCODE_CCCODE2"
            Public Const SESS_CCCode3 As String = "SESS_CHOOSECCCODE_CCCODE3"
            Public Const SESS_CCCode4 As String = "SESS_CHOOSECCCODE_CCCODE4"
            Public Const SESS_CCCode5 As String = "SESS_CHOOSECCCODE_CCCODE5"
            Public Const SESS_CCCode6 As String = "SESS_CHOOSECCCODE_CCCODE6"

            Public Const SESS_PreFillConsentID As String = "SESS_PRE_FILL_CONSENT_ID"
            Public Const SESS_PreFillSubmitTime As String = "SESS_PRE_FILL_SUBMIT_TIME"
            Public Const SESS_PreFillChinese As String = "SESS_PRE_FILL_CHINESE"
            Public Const SESS_PreFillPressConfirm As String = "SESS_PRE_FILL_PRESSCONFIRM"

        End Class

        Public Function Language()
            Return CType(HttpContext.Current.Session(SessionName.SESS_LANGUAGE), String)
        End Function


#Region "Current User Service Provider/Data Entry"
        Public Sub CurrentUserSaveToSession(ByVal udtSP As ServiceProviderModel, ByVal udtDataEntry As DataEntryUserModel)
            HttpContext.Current.Session(SessionName.SESS_ServiceProvider) = udtSP
            If Not udtDataEntry Is Nothing Then
                HttpContext.Current.Session(SessionName.SESS_DataEntry) = udtDataEntry
            End If
        End Sub

        Public Sub CurrentUserGetFromSession(ByRef udtSP As ServiceProviderModel, ByRef udtDataEntry As DataEntryUserModel)
            udtSP = CType(HttpContext.Current.Session(SessionName.SESS_ServiceProvider), ServiceProviderModel)
            udtDataEntry = CType(HttpContext.Current.Session(SessionName.SESS_DataEntry), DataEntryUserModel)
        End Sub

        Public Sub CurrentUserRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_ServiceProvider)
            HttpContext.Current.Session.Remove(SessionName.SESS_DataEntry)
        End Sub
#End Region

#Region "EHS Claim"
        Public Sub EHSClaimSessionRemove()
            'Status
            Me.AccountCreationProceedClaimRemoveFromSession()
            Me.NotMatchAccountExistRemoveFromSession()
            Me.ExceedDocTypeLimitRemoveFromSession()

            'Account
            Me.EHSAccountRemoveFromSession(Common.Component.FunctCode.FUNT020201)
            Me.CCCodeRemoveFromSession(Common.Component.FunctCode.FUNT020201)

            'Transaction
            Me.EHSClaimVaccineRemoveFromSession()
            Me.EHSTransactionRemoveFromSession()
        End Sub
#End Region

        '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        'only For EHS Claim
        '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
#Region "Temp Account record is exist in DataBase but DOB Not match (Detail not match) (For EHS Claim Only) (Can be Validated/Tempoary Account)"
        Public Sub NotMatchAccountExistSaveToSession(ByVal blnNotMatchAccountExist As Boolean)
            HttpContext.Current.Session(SessionName.SESS_NotMatchTempAccountExist) = blnNotMatchAccountExist
        End Sub

        Public Function NotMatchAccountExistGetFromSession() As Boolean
            If HttpContext.Current.Session(SessionName.SESS_NotMatchTempAccountExist) Is Nothing Then
                Return False
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_NotMatchTempAccountExist), Boolean)
            End If
        End Function

        Public Sub NotMatchAccountExistRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_NotMatchTempAccountExist)
        End Sub
#End Region

#Region "Cross Search: Sreach By HKID -> Return HKBC Personal Informtion but the recipient age is exceed DocType Limit"
        Public Sub ExceedDocTypeLimitSaveToSession(ByVal blnExceedDocTypeLimit As Boolean)
            HttpContext.Current.Session(SessionName.SESS_ExceedDocTypeLimit) = blnExceedDocTypeLimit
        End Sub

        Public Function ExceedDocTypeLimitGetFromSession() As Boolean
            If HttpContext.Current.Session(SessionName.SESS_ExceedDocTypeLimit) Is Nothing Then
                Return False
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_ExceedDocTypeLimit), Boolean)
            End If
        End Function

        Public Sub ExceedDocTypeLimitRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_ExceedDocTypeLimit)
        End Sub
#End Region

#Region "Account Creation Status (For EHS Claim Only)"
        Public Sub AccountCreationProceedClaimSaveToSession(ByVal isAccountCreationProceedClaim As Boolean)
            HttpContext.Current.Session(SessionName.SESS_AccountCreationProceedClaim) = isAccountCreationProceedClaim
        End Sub

        Public Function AccountCreationProceedClaimGetFromSession() As Boolean
            If HttpContext.Current.Session(SessionName.SESS_AccountCreationProceedClaim) Is Nothing Then
                Return False
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_AccountCreationProceedClaim), Boolean)
            End If
        End Function

        Public Sub AccountCreationProceedClaimRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_AccountCreationProceedClaim)
        End Sub
#End Region

#Region "Transaction"
        Public Sub EHSTransactionSaveToSession(ByVal udtEHSClaimVaccine As EHSTransactionModel)
            HttpContext.Current.Session(SessionName.SESS_EHSTransaction) = udtEHSClaimVaccine
        End Sub

        Public Function EHSTransactionGetFromSession() As EHSTransactionModel
            If HttpContext.Current.Session(SessionName.SESS_EHSTransaction) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_EHSTransaction), EHSTransactionModel)
            End If
        End Function

        Public Sub EHSTransactionRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_EHSClaimVaccine)
        End Sub
#End Region

#Region "EHS Claim Vaccine (For EHS Claim Only)"
        Public Sub EHSClaimVaccineSaveToSession(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel)
            HttpContext.Current.Session(SessionName.SESS_EHSClaimVaccine) = udtEHSClaimVaccine
        End Sub

        Public Function EHSClaimVaccineGetFromSession() As EHSClaimVaccineModel
            If HttpContext.Current.Session(SessionName.SESS_EHSClaimVaccine) Is Nothing Then
                Return Nothing
            Else
                Return CType(HttpContext.Current.Session(SessionName.SESS_EHSClaimVaccine), EHSClaimVaccineModel)
            End If
        End Function

        Public Sub EHSClaimVaccineRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_EHSClaimVaccine)
        End Sub
#End Region

#Region "CCCode control"
        Public Sub CCCodeSaveToSession(ByVal strFunctionCode As String, ByVal strCCCodeSessionName As String, ByVal strCCCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, strCCCodeSessionName)) = strCCCode
        End Sub

        Public Function CCCodeGetFormSession(ByVal strFunctionCode As String, ByVal strCCCodeSessionName As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, strCCCodeSessionName))
        End Function

        Public Sub CCCodeRemoveFromSession(ByVal strFunctionCode As String, ByVal strCCCodeSessionName As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, strCCCodeSessionName))
        End Sub

        Public Sub CCCodeRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode1))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode2))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode3))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode4))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode5))
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_CCCode6))
        End Sub
#End Region

        '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
        'Permanent Session
        '-----------------------------------------------------------------------------------------------------------------------------------------------------------------
#Region "Practice Display Collection"
        Public Sub PracticeDisplayListSaveToSession(ByVal udtPracticeDisplay As PracticeDisplayModelCollection)
            HttpContext.Current.Session(SessionName.SESS_SelectedPracticeDsiplayList) = udtPracticeDisplay
        End Sub

        Public Function PracticeDisplayListGetFromSession() As PracticeDisplayModelCollection
            Return CType(HttpContext.Current.Session(SessionName.SESS_SelectedPracticeDsiplayList), PracticeDisplayModelCollection)
        End Function

        Public Sub PracticeDisplayListRemoveFromSession()
            HttpContext.Current.Session.Remove(SessionName.SESS_SelectedPracticeDsiplayList)
        End Sub
#End Region

#Region "Selected Practice Display"
        Public Sub PracticeDisplaySaveToSession(ByVal udtPracticeDisplay As PracticeDisplayModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SelectedPracticeDsiplay)) = udtPracticeDisplay
        End Sub

        Public Function PracticeDisplayGetFromSession(ByVal strFunctionCode As String) As PracticeDisplayModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SelectedPracticeDsiplay)), PracticeDisplayModel)
        End Function

        Public Sub PracticeDisplayRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SelectedPracticeDsiplay))
        End Sub
#End Region

#Region "eHealth Account"
        Public Sub EHSAccountSaveToSession(ByVal udtEHSAccount As EHSAccountModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount)) = udtEHSAccount
        End Sub

        Public Function EHSAccountGetFromSession(ByVal strFunctionCode As String) As EHSAccountModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount)), EHSAccountModel)
        End Function

        Public Sub EHSAccountRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_EHSAccount))
        End Sub
#End Region

#Region "Scheme Selected"
        Public Sub SchemeSelectedSaveToSession(ByVal udtSchemeClaim As SchemeClaimModel, ByVal strFunctionCode As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeSelected)) = udtSchemeClaim
        End Sub

        Public Function SchemeSelectedGetFromSession(ByVal strFunctionCode As String) As SchemeClaimModel
            Return CType(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeSelected)), SchemeClaimModel)
        End Function

        Public Sub SchemeSelectedRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_SchemeSelected))
        End Sub
#End Region

#Region "PreFill Consent"
        Public Sub PreFillConsentIDSaveToSession(ByVal strFunctionCode As String, ByVal strPreFillConsentID As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PreFillConsentID)) = strPreFillConsentID
        End Sub

        Public Function PreFillConsentIDGetFormSession(ByVal strFunctionCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PreFillConsentID))
        End Function

        Public Sub PreFillConsentIDRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PreFillConsentID))
        End Sub

        Public Sub PreFillSubmitTimeSaveToSession(ByVal strFunctionCode As String, ByVal dtPreFillSubmit As Date)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PreFillSubmitTime)) = dtPreFillSubmit
        End Sub

        Public Function PreFillSubmitTimeGetFormSession(ByVal strFunctionCode As String) As Date
            Return Convert.ToDateTime(HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PreFillSubmitTime)))
        End Function

        Public Sub PreFillSubmitTimeRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PreFillSubmitTime))
        End Sub

        Public Sub PreFillChineseSaveToSession(ByVal strFunctionCode As String, ByVal strPreFillChinese As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PreFillChinese)) = strPreFillChinese
        End Sub

        Public Function PreFillChineseGetFormSession(ByVal strFunctionCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PreFillChinese))
        End Function

        Public Sub PreFillChineseRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PreFillChinese))
        End Sub

        Public Sub PreFillPressConfirmSaveToSession(ByVal strFunctionCode As String, ByVal strConfirmSave As String)
            HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PreFillPressConfirm)) = strConfirmSave
        End Sub

        Public Function PreFillPressConfirmGetFormSession(ByVal strFunctionCode As String) As String
            Return HttpContext.Current.Session(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PreFillPressConfirm))
        End Function

        Public Sub PreFillPressConfirmRemoveFromSession(ByVal strFunctionCode As String)
            HttpContext.Current.Session.Remove(String.Format("{0}_{1}", strFunctionCode, SessionName.SESS_PreFillPressConfirm))
        End Sub

#End Region

    End Class
End Namespace

