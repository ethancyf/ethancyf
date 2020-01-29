Imports Common.Component.CCCode
Imports Common.Component.VoucherRecipientAccount
Imports Common.Component.ClaimTrans
Imports Common.Component.VoucherScheme
Imports Common.Component.ReasonForVisit
Imports Common.Component.ServiceProvider
Imports Common.Component.DataEntryUser
Imports Common.Component
Imports System.Data.SqlClient
Imports System.Data
Imports Common.DataAccess
Imports Common.Format

Namespace BLL

    Public Class VoucherAccountMaintenanceBLL

        Dim udtVRAcctBLL As VoucherRecipientAccountBLL = New VoucherRecipientAccountBLL
        Dim udtClaimTranBLL As ClaimTransBLL = New ClaimTransBLL
        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtDataEntryBLL As DataEntryUserBLL = New DataEntryUserBLL

        Dim udtFormatter As Formatter = New Formatter
        Dim udtGeneralF As New Common.ComFunction.GeneralFunction

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' Obsolete functions that are no longer used
        ' -----------------------------------------------------------------------------------------    

        'Public Function chkDataEntrySessionExist() As Boolean
        '    Return udtDataEntryBLL.Exist
        'End Function

        'Public Function chkVRAcctSessionExist() As Boolean
        '    Return udtVRAcctBLL.Exist
        'End Function

        'Public Function chkVRSchemeSessionExist() As Boolean
        '    Return udtVRSchemeBLL.Exist
        'End Function

        'Public Function chkSPSessionExist() As Boolean
        '    Return udtSPBLL.Exist
        'End Function

        'Public Sub saveDataEntrySession(ByVal udtDataEntry As DataEntryUserModel)
        '    udtDataEntryBLL.SaveToSession(udtDataEntry)
        'End Sub

        'Public Function loadDataEntrySession() As DataEntryUserModel
        '    Return udtDataEntryBLL.GetDataEntry()
        'End Function

        'Public Function loadSP(ByVal strSPID As String) As ServiceProviderModel
        '    Dim udtSP As ServiceProviderModel
        '    udtSP = udtSPBLL.GetServiceProviderBySPID(New Database, strSPID)
        '    Return udtSP
        'End Function

        'Public Sub saveSPSession(ByVal udtSP As ServiceProviderModel)
        '    udtSPBLL.SaveToSession(udtSP)
        'End Sub

        'Public Function loadSPSession() As ServiceProviderModel
        '    Return udtSPBLL.GetSP()
        'End Function

        'Public Sub clearRelatedSession()
        '    udtVRAcctBLL.ClearSession()
        '    udtVRSchemeBLL.ClearSession()
        'End Sub

        'Public Sub saveVRAcctSession(ByVal udtVRAcct As VoucherRecipientAccountModel)
        '    udtVRAcctBLL.SaveToSession(udtVRAcct)
        'End Sub

        'Public Sub saveVRSchemeSession(ByVal udtVRScheme As VoucherSchemeModel)
        '    udtVRSchemeBLL.SaveToSession(udtVRScheme)
        'End Sub

        'Public Function loadVRSchemeSession() As VoucherSchemeModel
        '    Return udtVRSchemeBLL.GetScheme()
        'End Function

        'Public Function loadVRAcctSession() As VoucherRecipientAccountModel
        '    Return udtVRAcctBLL.GetVRAcct()
        'End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

#Region "CCCode Supporting Function"

        Public Function getCCCTail(ByVal strcccode As String, ByRef strDisplay As String) As String
            Dim strRes As String
            Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL
            strRes = String.Empty
            strRes = udtCCCodeBLL.GetCCCodeDesc(strcccode, strDisplay)
            Return strRes
        End Function

        Public Function getCCCTail(ByVal strcccode As String) As DataTable
            Dim dtRes As DataTable
            Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL
            dtRes = udtCCCodeBLL.GetCCCodeDesc(strcccode)
            Return dtRes
        End Function

        Public Function getChiChar(ByVal strcccode As String) As String
            Dim strRes As String
            Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL
            strRes = String.Empty
            strRes = udtCCCodeBLL.GetChiChar(strcccode)
            Return strRes
        End Function

        Public Function getCCCodeBig5(ByVal strCCCode As String) As String
            Dim strCCCodeBig5 As String = String.Empty
            Dim dtRes As DataTable
            Dim strTail As String

            If Not strCCCode Is Nothing AndAlso strCCCode.Length > 0 Then
                If strCCCode.Length <> 5 Then
                    Return " "
                End If

                dtRes = Me.getCCCTail(strCCCode.Substring(0, 4))
                strTail = strCCCode.Substring(4, 1)
                If Not dtRes Is Nothing AndAlso dtRes.Rows.Count > 0 Then

                    For Each dataRow As DataRow In dtRes.Rows
                        If dataRow("ccc_tail").ToString().Equals(strTail) Then
                            Return dataRow("Big5").ToString()
                        End If
                    Next

                    Return " "
                Else
                    Return " "
                End If
            End If

            Return strCCCodeBig5
        End Function

        Public Function getChiChar(ByVal strcccode As String, ByVal strTail As String) As String
            Dim dataTable As DataTable = getCCCTail(strcccode)

            For Each dataRow As DataRow In dataTable.Rows
                If dataRow("ccc_tail").ToString().Equals(strTail) Then
                    Return dataRow("Big5").ToString()
                End If
            Next
            Return String.Empty
        End Function

        Public Shared Function GetCName(ByVal udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel) As String
            Dim udtVAMaintBLL As BLL.VoucherAccountMaintenanceBLL = New BLL.VoucherAccountMaintenanceBLL()
            Dim strCName As String = String.Empty

            If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode1) Then
                strCName += udtVAMaintBLL.getCCCodeBig5(udtEHSPersonalInformation.CCCode1)
            End If

            If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode2) Then
                strCName += udtVAMaintBLL.getCCCodeBig5(udtEHSPersonalInformation.CCCode2)
            End If

            If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode3) Then
                strCName += udtVAMaintBLL.getCCCodeBig5(udtEHSPersonalInformation.CCCode3)
            End If

            If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode4) Then
                strCName += udtVAMaintBLL.getCCCodeBig5(udtEHSPersonalInformation.CCCode4)
            End If

            If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode5) Then
                strCName += udtVAMaintBLL.getCCCodeBig5(udtEHSPersonalInformation.CCCode5)
            End If

            If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode6) Then
                strCName += udtVAMaintBLL.getCCCodeBig5(udtEHSPersonalInformation.CCCode6)
            End If

            Return strCName
        End Function
#End Region

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' Obsolete functions that are no longer used
        ' -----------------------------------------------------------------------------------------    

        'Public Function chkReadyCreateVRAcct(ByVal strHKID As String, ByVal strDOB As String, ByVal strSchemeCode As String, ByRef udtVRAcct As VoucherRecipientAccountModel, ByRef udtVoucherScheme As VoucherSchemeModel, ByVal strInputAgeDate As String, ByVal strInputAge As String) As Common.ComObject.SystemMessage

        '    Dim udtVRAcctBLL As VoucherRecipientAccountBLL = New VoucherRecipientAccountBLL
        '    Dim udtVRAcctCollection As VoucherRecipientAccountModelCollection
        '    Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        '    Dim sm As Common.ComObject.SystemMessage
        '    Dim dtDOB As Date
        '    Dim strExactDOB As String
        '    Dim strFunctCode, strSeverity, strMsgCode As String
        '    Dim formatter As Formatter = New Formatter
        '    Dim intUsedVoucher As Integer = 0
        '    Dim intAvailVoucher As Integer = 0
        '    Dim i As Integer = 0

        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = ""

        '    strExactDOB = "N"
        '    udtVoucherScheme = getVoucherScheme(strSchemeCode)
        '    If udtVoucherScheme Is Nothing Then
        '        'Throw error if the Scheme setting is invalid
        '        strMsgCode = "00105"
        '    Else
        '        'If strDOB.Length = 4 Then
        '        '    strExactDOB = "N"
        '        '    dtDOB = CDate("01 Jan " + strDOB)
        '        'Else
        '        '    strExactDOB = "Y"
        '        '    dtDOB = CDate(formatter.convertDate(strDOB, "E"))
        '        'End If

        '        commfunct.chkDOBtype(strDOB, dtDOB, strExactDOB)

        '        udtVRAcct = udtVRAcctBLL.LoadVRAcct(strHKID, strSchemeCode)
        '        If udtVRAcct Is Nothing Then

        '            udtVRAcctCollection = udtVRAcctBLL.LoadTempVRAcct(strHKID, strSchemeCode)
        '            For Each udtTempVRAcct As VoucherRecipientAccountModel In udtVRAcctCollection
        '                If udtTempVRAcct.AcctStatus <> "D" Then
        '                    i = i + 1
        '                End If
        '            Next

        '            'If udtVRAcctCollection.Count < 1 Then
        '            If i < 1 Then
        '                'No Validated Voucher Recipient Account found
        '                udtVRAcct = New VoucherRecipientAccountModel()
        '                With udtVRAcct
        '                    .HKID = strHKID
        '                    .IsExactDOB = strExactDOB
        '                    .DOB = dtDOB

        '                    If strInputAge.Trim() <> "" Then
        '                        .HKIDCard = "N"
        '                        .IsExactDOB = "A"
        '                        .ECAge = Convert.ToInt32(strInputAge)
        '                        .ECDateOfRegistration = CDate(formatter.convertDate(strInputAgeDate, "E"))
        '                    End If
        '                End With

        '                If Not udtVRSchemeBLL.IsEligible(strSchemeCode, dtDOB) Then
        '                    strMsgCode = "00106"
        '                Else
        '                    intAvailVoucher = getAvailVoucher(udtVRAcct, udtVoucherScheme.SchemeCode)
        '                    If intAvailVoucher < 1 Then
        '                        strMsgCode = "00107"
        '                    Else
        '                        sm = Nothing
        '                    End If
        '                End If
        '            Else
        '                strMsgCode = "00112"
        '            End If

        '        Else
        '            strMsgCode = "00112"
        '        End If
        '    End If

        '    If strMsgCode.Equals(String.Empty) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        'Public Function getVoucherScheme(ByVal strSchemeCode As String) As VoucherSchemeModel
        '    Dim udtVoucherScheme As VoucherSchemeModel = New VoucherSchemeModel
        '    Dim udtVoucherSchemeBLL As VoucherSchemeBLL = New VoucherSchemeBLL
        '    udtVoucherScheme = udtVoucherSchemeBLL.LoadVoucheScheme(strSchemeCode)
        '    Return udtVoucherScheme
        'End Function

        'Public Function getAvailVoucher(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strSchemeCode As String) As Integer
        '    Dim intRes As Integer = 0
        '    Dim intAvailVoucher As Integer = 0
        '    Dim udtVRAcctBLL As VoucherRecipientAccountBLL = New VoucherRecipientAccountBLL

        '    intAvailVoucher = udtVRAcctBLL.getAvailVoucher(udtVRAcct, strSchemeCode)
        '    intRes = intAvailVoucher

        '    Return intRes
        'End Function

        'Public Sub sepSPPracticeBankAcct(ByVal strBankAccountKey As String, ByRef strSPID As String, ByRef intPracticeID As Integer, ByRef intBankAcctID As Integer)
        '    Dim strtemp As String
        '    Dim strSPIDtemp, strPracticeIDtemp, strBankAcctIDtemp As String
        '    strSPIDtemp = String.Empty
        '    strPracticeIDtemp = String.Empty
        '    strBankAcctIDtemp = String.Empty
        '    strtemp = strBankAccountKey.Trim()
        '    strSPIDtemp = strtemp.Substring(0, 8)
        '    strtemp = strtemp.Substring(9)
        '    strPracticeIDtemp = strtemp.Substring(0, strtemp.IndexOf("-"))
        '    strBankAcctIDtemp = strtemp.Substring(strtemp.IndexOf("-") + 1)
        '    strSPID = strSPIDtemp
        '    intPracticeID = CType(strPracticeIDtemp, Integer)
        '    intBankAcctID = CType(strBankAcctIDtemp, Integer)
        'End Sub

        Public Function loadRectifyList(ByVal strSPID As String, _
                                ByVal strDataEntry As String, ByVal strStatus As String, ByVal enumSubPlatform As [Enum]) As DataTable
            Dim dt As DataTable
            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'dt = udtVRAcctBLL.LoadRectifyTempVRAcct(strSPID, strDataEntry, strStatus)
            dt = udtVRAcctBLL.LoadRectifyTempVRAcct(strSPID, strDataEntry, strStatus, enumSubPlatform)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            Return dt

        End Function

        '    Public Function chkEnameIdentical(ByVal strEnameSurname1 As String, ByVal strEnameFirstname1 As String, _
        'ByVal strEnameSurname2 As String, ByVal strEnameFirstname2 As String) As Common.ComObject.SystemMessage
        '        Dim sm As Common.ComObject.SystemMessage
        '        Dim strFunctCode, strSeverity, strMsgCode As String

        '        strFunctCode = "990000"
        '        strSeverity = "E"
        '        strMsgCode = ""
        '        If Not strEnameSurname1.Equals(strEnameSurname2) Or Not strEnameFirstname1.Equals(strEnameFirstname2) Then
        '            strMsgCode = "00115"
        '        Else
        '            strMsgCode = ""
        '        End If

        '        If strMsgCode.Equals(String.Empty) Then
        '            sm = Nothing
        '        Else
        '            sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '        End If

        '        Return sm
        '    End Function

        'Public Function chkDOBIdentical(ByVal strDOB1 As String, ByVal strDOB2 As String) As Common.ComObject.SystemMessage
        '    Dim sm As Common.ComObject.SystemMessage
        '    Dim strFunctCode, strSeverity, strMsgCode As String

        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = ""
        '    If Not strDOB1.Equals(strDOB2) Then
        '        strMsgCode = "00114"
        '    Else
        '        strMsgCode = ""
        '    End If

        '    If strMsgCode.Equals(String.Empty) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        'Public Function chkHKIDIdentical(ByVal strHKID1 As String, ByVal strHKID2 As String) As Common.ComObject.SystemMessage
        '    Dim sm As Common.ComObject.SystemMessage
        '    Dim strFunctCode, strSeverity, strMsgCode As String

        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = ""
        '    If Not strHKID1.Equals(strHKID2) Then
        '        strMsgCode = "00113"
        '    Else
        '        strMsgCode = ""
        '    End If

        '    If strMsgCode.Equals(String.Empty) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        'Public Function chkHKIDIssueDateIdentical(ByVal strIssueDate As String, ByVal dtOriIssueDate As Date) As Common.ComObject.SystemMessage
        '    Dim sm As Common.ComObject.SystemMessage
        '    Dim strFunctCode, strSeverity, strMsgCode As String

        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = "00116"

        '    If strIssueDate.Trim.Equals(dtOriIssueDate.ToString("dd-MM-yy")) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        'Public Sub saveRectifyTempVA(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdateBy As String)
        '    Dim udtdb As Database = New Database
        '    Dim objECDOI As Object = DBNull.Value
        '    Dim objECDOR As Object = DBNull.Value
        '    Dim objHKICDOI As Object = DBNull.Value
        '    Dim objECAge As Object = DBNull.Value
        '    Dim objECSerialNo As Object = DBNull.Value
        '    Dim objECRefNo As Object = DBNull.Value

        '    Try
        '        With udtVRAcct
        '            udtdb.BeginTransaction()

        '            If .ECDate.HasValue Then
        '                objECDOI = .ECDate.Value
        '            End If

        '            If .ECDateOfRegistration.HasValue Then
        '                objECDOR = .ECDateOfRegistration.Value
        '            End If

        '            If .HKIDIssuseDate.HasValue Then
        '                objHKICDOI = .HKIDIssuseDate.Value
        '            End If

        '            If .ECAge > -1 Then
        '                objECAge = .ECAge
        '            End If

        '            If Not .ECSerialNo Is Nothing AndAlso Not .ECSerialNo.Equals(String.Empty) Then
        '                objECSerialNo = .ECSerialNo
        '            End If

        '            If Not .ECReferenceNo Is Nothing AndAlso Not .ECReferenceNo.Equals(String.Empty) Then
        '                objECRefNo = .ECReferenceNo
        '            End If

        '            Dim parms() As SqlParameter = { _
        '                udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, .VRAcctID), _
        '                udtdb.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, .SchemeCode), _
        '                udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
        '                udtdb.MakeInParam("@Record_Status", SqlDbType.Char, 1, .AcctValidatedStatus), _
        '                udtdb.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, .TSMP) _
        '            }
        '            udtdb.RunProc("proc_TempVoucherAccountRectify_upd", parms)

        '            ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
        '            Dim parms2() As SqlParameter = { _
        '                udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, .VRAcctID), _
        '                udtdb.MakeInParam("@HKID", SqlDbType.Char, 9, .HKID), _
        '                udtdb.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, udtFormatter.formatEnglishName(.ENameSurName, .ENameFirstName)), _
        '                udtdb.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, .CName), _
        '                udtdb.MakeInParam("@CCcode1", SqlDbType.Char, 5, .CCCode1), _
        '                udtdb.MakeInParam("@CCcode2", SqlDbType.Char, 5, .CCCode2), _
        '                udtdb.MakeInParam("@CCcode3", SqlDbType.Char, 5, .CCCode3), _
        '                udtdb.MakeInParam("@CCcode4", SqlDbType.Char, 5, .CCCode4), _
        '                udtdb.MakeInParam("@CCcode5", SqlDbType.Char, 5, .CCCode5), _
        '                udtdb.MakeInParam("@CCcode6", SqlDbType.Char, 5, .CCCode6), _
        '                udtdb.MakeInParam("@DOB", SqlDbType.DateTime, 8, .DOB), _
        '                udtdb.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, .IsExactDOB), _
        '                udtdb.MakeInParam("@Sex", SqlDbType.Char, 1, .Gender), _
        '                udtdb.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, objHKICDOI), _
        '                udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
        '                udtdb.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, objECSerialNo), _
        '                udtdb.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 15, objECRefNo), _
        '                udtdb.MakeInParam("@EC_Date", SqlDbType.DateTime, 8, objECDOI), _
        '                udtdb.MakeInParam("@EC_Age", SqlDbType.SmallInt, 2, objECAge), _
        '                udtdb.MakeInParam("@EC_Date_of_Registration", SqlDbType.DateTime, 8, objECDOR), _
        '                udtdb.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, .PITSMP) _
        '            }
        '            udtdb.RunProc("proc_TempPersonalInformation_upd_PersonalInfo", parms2)
        '            ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

        '            udtdb.CommitTransaction()
        '        End With
        '    Catch ex As Exception
        '        udtdb.RollBackTranscation()
        '        Throw ex
        '    End Try
        'End Sub

        'Public Function sendLevel1Level3AlertInboxMessage() As Boolean

        '    Dim udtdb As Database = New Database
        '    Dim dt3Level, dt1Level As New DataTable
        '    Dim i As Integer
        '    Dim arrStrSPIDLevel3 As New List(Of String)
        '    Dim arrStrSPIDLevel1 As New List(Of String)

        '    Try
        '        udtdb.BeginTransaction()

        '        udtdb.RunProc("proc_GetAllSPIDWithLevel3", dt3Level)

        '        If dt3Level.Rows.Count > 0 Then
        '            For i = 0 To dt3Level.Rows.Count - 1
        '                If Not arrStrSPIDLevel3.Contains(dt3Level.Rows(i)("SP_ID").ToString.Trim) Then
        '                    arrStrSPIDLevel3.Add(dt3Level.Rows(i)("SP_ID").ToString.Trim)
        '                End If
        '            Next

        '            Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
        '            Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
        '            Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing
        '            Me.ConstructHCSPRectifyNotificationMessages(udtdb, arrStrSPIDLevel3, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.Level3NotificationIn4LevelAlert)

        '            udtInboxBLL.AddMessageAndMessageReaderList(udtdb, udtMessageCollection, udtMessageReaderCollection)
        '        End If

        '        udtdb.RunProc("proc_GetAllSPIDWithLevel1", dt1Level)
        '        If dt1Level.Rows.Count > 0 Then

        '            For i = 0 To dt1Level.Rows.Count - 1
        '                If Not arrStrSPIDLevel1.Contains(dt1Level.Rows(i)("SP_ID").ToString.Trim) And Not arrStrSPIDLevel3.Contains(dt1Level.Rows(i)("SP_ID").ToString.Trim) Then
        '                    arrStrSPIDLevel1.Add(dt1Level.Rows(i)("SP_ID").ToString.Trim)
        '                End If
        '            Next

        '            Dim udtInboxBLL As New Common.Component.Inbox.InboxBLL()
        '            Dim udtMessageCollection As Common.Component.Inbox.MessageModelCollection = Nothing
        '            Dim udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection = Nothing
        '            Me.ConstructHCSPRectifyNotificationMessages(udtdb, arrStrSPIDLevel1, udtMessageCollection, udtMessageReaderCollection, Common.Component.InboxMsgTemplateID.HCSPRectifyNotification)

        '            udtInboxBLL.AddMessageAndMessageReaderList(udtdb, udtMessageCollection, udtMessageReaderCollection)
        '        End If

        '        udtdb.CommitTransaction()

        '        Return True
        '    Catch ex As Exception
        '        udtdb.RollBackTranscation()
        '        Throw ex
        '    End Try
        'End Function

        'Private Sub ConstructHCSPRectifyNotificationMessages(ByRef udtDB As Common.DataAccess.Database, ByRef arrStrSPID As List(Of String), ByRef udtMessageCollection As Common.Component.Inbox.MessageModelCollection, ByRef udtMessageReaderCollection As Common.Component.Inbox.MessageReaderModelCollection, ByVal strMailTemplateID As String)

        '    udtMessageCollection = New Common.Component.Inbox.MessageModelCollection()
        '    udtMessageReaderCollection = New Common.Component.Inbox.MessageReaderModelCollection()

        '    ' Retrieve Message Template
        '    Dim udtInternetMailBLL As New Common.Component.InternetMail.InternetMailBLL()
        '    Dim udtMailTemplate As Common.Component.InternetMail.MailTemplateModel = udtInternetMailBLL.GetMailTemplate(udtDB, strMailTemplateID)
        '    Dim dtmCurrent As DateTime = Me.udtGeneralF.GetSystemDateTime()

        '    For Each strSPID As String In arrStrSPID

        '        ' Retrieve SP Defaul Language
        '        Dim dtSP As DataTable = Me.RetrieveSPDefaultLanguage(udtDB, strSPID)

        '        Dim strLang As String = Common.Component.InternetMailLanguage.EngHeader
        '        If Not dtSP.Rows(0).IsNull("Default_Language") Then strLang = dtSP.Rows(0)("Default_Language").ToString().Trim()

        '        Dim strSubject As String = ""
        '        If strLang = Common.Component.InternetMailLanguage.EngHeader Then
        '            strSubject = udtMailTemplate.MailSubjectEng
        '        Else
        '            strSubject = udtMailTemplate.MailSubjectChi
        '        End If

        '        Dim strChiContent As String = udtMailTemplate.MailBodyChi
        '        Dim strEngContent As String = udtMailTemplate.MailBodyEng
        '        Dim udtMessage As New Common.Component.Inbox.MessageModel()
        '        udtMessage.MessageID = Me.udtGeneralF.generateInboxMsgID()


        '        udtMessage.Subject = strSubject
        '        udtMessage.Message = strChiContent + " " + strEngContent

        '        udtMessage.CreateBy = "EHCVS"
        '        udtMessage.CreateDtm = dtmCurrent
        '        udtMessageCollection.Add(udtMessage)

        '        Dim udtMessageReader As New Common.Component.Inbox.MessageReaderModel()
        '        udtMessageReader.MessageID = udtMessage.MessageID
        '        udtMessageReader.MessageReader = strSPID
        '        udtMessageReader.UpdateBy = "EHCVS"
        '        udtMessageReader.UpdateDtm = dtmCurrent

        '        udtMessageReaderCollection.Add(udtMessageReader)
        '    Next

        'End Sub

        'Private Function RetrieveSPDefaultLanguage(ByRef udtDB As Common.DataAccess.Database, ByVal strSPID As String) As DataTable
        '    Dim dtResult As New DataTable()
        '    Try
        '        Dim prams() As SqlParameter = {udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID)}
        '        udtDB.RunProc("proc_HCSPUserAC_get_BySPID", prams, dtResult)

        '        Return dtResult
        '    Catch ex As Exception
        '        Throw ex
        '    End Try
        'End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

        Public Function getLevel1InboxCount(ByVal strSPID As String, ByVal enumSubPlatform As [Enum]) As Integer
            Dim udtDB As Database = New Database
            Dim intCount As Integer = 0
            Dim dt As New DataTable

            Try
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim strSubPlatform As String = String.Empty
                If Not enumSubPlatform Is Nothing Then
                    strSubPlatform = enumSubPlatform.ToString
                End If

                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                    udtDB.MakeInParam("@level", SqlDbType.Int, 8, 1), _
                    udtDB.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                udtdb.RunProc("proc_TempVRAcctCountFor4LevelAlert_get", parms, dt)

                If dt.Rows.Count > 0 Then
                    Return dt.Rows.Count
                Else
                    Return 0
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function getLevel2TaskListCount(ByVal strSPID As String, ByVal enumSubPlatform As [Enum]) As Integer
            Dim udtDB As Database = New Database
            Dim intCount As Integer = 0
            Dim dt As New DataTable

            Try
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim strSubPlatform As String = String.Empty
                If Not enumSubPlatform Is Nothing Then
                    strSubPlatform = enumSubPlatform.ToString
                End If

                Dim parms() As SqlParameter = { _
                    udtDB.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                    udtDB.MakeInParam("@level", SqlDbType.Int, 8, 2), _
                    udtDB.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                udtdb.RunProc("proc_TempVRAcctCountFor4LevelAlert_get", parms, dt)

                If dt.Rows.Count > 0 Then
                    Return dt.Rows.Count
                Else
                    Return 0
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function getLevel4PopupVoucherAccount(ByVal strSPID As String, ByVal enumSubPlatform As [Enum]) As DataTable
            Dim udtdb As Database = New Database
            Dim dt As New DataTable


            Try
                'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                Dim strSubPlatform As String = String.Empty
                If Not enumSubPlatform Is Nothing Then
                    strSubPlatform = enumSubPlatform.ToString
                End If

                Dim parms() As SqlParameter = { _
                    udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 8, strSPID), _
                    udtdb.MakeInParam("@level", SqlDbType.Int, 8, 4), _
                    udtdb.MakeInParam("@Available_HCSP_SubPlatform", SqlDbType.VarChar, 2, IIf(strSubPlatform.Trim.Equals(String.Empty), DBNull.Value, strSubPlatform))}
                'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

                udtdb.RunProc("proc_TempVRAcctCountFor4LevelAlert_get", parms, dt)

                Return dt
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' Obsolete functions that are no longer used
        ' -----------------------------------------------------------------------------------------    

        'Step 1a. Create a new C VR Acct
        'Step 1b. Delete the original X VR Acct
        'Step 2a. Create a new TempPersonalInformation
        'Step 2b. Delete the old TempPersonalInformation
        'Step 3. Update the VR Acct field in VoucherTransaction with the new C VR Acct
        'Step 4. Delete the TempVoucherPendifyVerify table about the first validation fail date of the old X VR Acct ID
        'Step 5. Insert into VoucherAccountCreation Log for the new VRAccount
        'Public Sub deleteNcreateRectifyTempVA(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdateBy As String, ByVal strNewVRAcctID As String, ByVal strNewPractice As String)
        '    Dim udtdb As Database = New Database
        '    Dim objECDOI As Object = DBNull.Value
        '    Dim objECDOR As Object = DBNull.Value
        '    Dim objHKICDOI As Object = DBNull.Value
        '    Dim objECAge As Object = DBNull.Value
        '    Dim objECSerialNo As Object = DBNull.Value
        '    Dim objECRefNo As Object = DBNull.Value

        '    Try
        '        With udtVRAcct
        '            udtdb.BeginTransaction()

        '            If .ECDate.HasValue Then
        '                objECDOI = .ECDate.Value
        '            End If

        '            If .ECDateOfRegistration.HasValue Then
        '                objECDOR = .ECDateOfRegistration.Value
        '            End If

        '            If .HKIDIssuseDate.HasValue Then
        '                objHKICDOI = .HKIDIssuseDate.Value
        '            End If

        '            If .ECAge > -1 Then
        '                objECAge = .ECAge
        '            End If

        '            If Not .ECSerialNo Is Nothing AndAlso Not .ECSerialNo.Equals(String.Empty) Then
        '                objECSerialNo = .ECSerialNo
        '            End If

        '            If Not .ECReferenceNo Is Nothing AndAlso Not .ECReferenceNo.Equals(String.Empty) Then
        '                objECRefNo = .ECReferenceNo
        '            End If

        '            'Step 1a
        '            Dim parms3() As SqlParameter = { _
        '                udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strNewVRAcctID), _
        '                udtdb.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, .SchemeCode), _
        '                udtdb.MakeInParam("@Voucher_Used", SqlDbType.SmallInt, 2, .VoucherRedeem), _
        '                udtdb.MakeInParam("@Total_Voucher_Amt_Used", SqlDbType.Money, 4, .TotalUsedVoucherAmount), _
        '                udtdb.MakeInParam("@Validated_Acc_ID", SqlDbType.Char, 15, IIf(.ValidatedAccID Is Nothing, String.Empty, .ValidatedAccID)), _
        '                udtdb.MakeInParam("@Record_Status", SqlDbType.Char, 1, "P"), _
        '                udtdb.MakeInParam("@Account_Purpose", SqlDbType.Char, 1, .AcctPurpose), _
        '                udtdb.MakeInParam("@Create_By", SqlDbType.VarChar, 20, strUpdateBy), _
        '                udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
        '                udtdb.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, .DataEntry) _
        '            }
        '            udtdb.RunProc("proc_TempVoucherAccount_add", parms3)

        '            'Step 1b
        '            Dim parms() As SqlParameter = { _
        '                udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, .VRAcctID), _
        '                udtdb.MakeInParam("@Scheme_Code", SqlDbType.Char, 10, .SchemeCode), _
        '                udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
        '                udtdb.MakeInParam("@Record_Status", SqlDbType.Char, 1, "D"), _
        '                udtdb.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, .TSMP) _
        '            }
        '            udtdb.RunProc("proc_TempVoucherAccountRectify_upd", parms)

        '            'Step 2a
        '            ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
        '            Dim parms5() As SqlParameter = { _
        '                udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strNewVRAcctID), _
        '                udtdb.MakeInParam("@HKID", SqlDbType.Char, 9, .HKID), _
        '                udtdb.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, udtFormatter.formatEnglishName(.ENameSurName, .ENameFirstName)), _
        '                udtdb.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, .CName), _
        '                udtdb.MakeInParam("@CCcode1", SqlDbType.Char, 5, .CCCode1), _
        '                udtdb.MakeInParam("@CCcode2", SqlDbType.Char, 5, .CCCode2), _
        '                udtdb.MakeInParam("@CCcode3", SqlDbType.Char, 5, .CCCode3), _
        '                udtdb.MakeInParam("@CCcode4", SqlDbType.Char, 5, .CCCode4), _
        '                udtdb.MakeInParam("@CCcode5", SqlDbType.Char, 5, .CCCode5), _
        '                udtdb.MakeInParam("@CCcode6", SqlDbType.Char, 5, .CCCode6), _
        '                udtdb.MakeInParam("@DOB", SqlDbType.DateTime, 8, .DOB), _
        '                udtdb.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, .IsExactDOB), _
        '                udtdb.MakeInParam("@Sex", SqlDbType.Char, 1, .Gender), _
        '                udtdb.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, objHKICDOI), _
        '                udtdb.MakeInParam("@HKID_Card", SqlDbType.Char, 1, .HKIDCard), _
        '                udtdb.MakeInParam("@Create_By", SqlDbType.VarChar, 20, strUpdateBy), _
        '                udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
        '                udtdb.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, .DataEntry), _
        '                udtdb.MakeInParam("@Record_Status", SqlDbType.Char, 1, "N"), _
        '                udtdb.MakeInParam("@EC_Serial_No", SqlDbType.VarChar, 10, objECSerialNo), _
        '                udtdb.MakeInParam("@EC_Reference_No", SqlDbType.VarChar, 15, objECRefNo), _
        '                udtdb.MakeInParam("@EC_Date", SqlDbType.DateTime, 8, objECDOI), _
        '                udtdb.MakeInParam("@EC_Age", SqlDbType.SmallInt, 1, objECAge), _
        '                udtdb.MakeInParam("@EC_Date_of_Registration", SqlDbType.DateTime, 8, objECDOR) _
        '            }
        '            ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

        '            udtdb.RunProc("proc_TempPersonalInformaion_add", parms5)

        '            ''Step 2b
        '            'Dim parms2() As SqlParameter = { _
        '            '    udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, .VRAcctID), _
        '            '    udtdb.MakeInParam("@Record_Status", SqlDbType.Char, 1, "D"), _
        '            '    udtdb.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, .PITSMP) _
        '            '}
        '            'udtdb.RunProc("proc_TempPersonalInformation_upd_PersonalInfoRecordStatus", parms2)

        '            'Step 3
        '            Dim practiceBankAcct As String()
        '            practiceBankAcct = strNewPractice.Split("-")
        '            Dim prams4() As SqlParameter = {udtdb.MakeInParam("@Transaction_ID", SqlDbType.Char, 20, .RelatedTranID.Trim), _
        '                udtdb.MakeInParam("@Temp_Voucher_Acc_ID", SqlDbType.Char, 15, strNewVRAcctID.Trim), _
        '                udtdb.MakeInParam("@Bank_Acc_Display_Seq", SqlDbType.SmallInt, 4, CInt(practiceBankAcct(2))), _
        '                udtdb.MakeInParam("@Practice_Display_Seq", SqlDbType.SmallInt, 4, CInt(practiceBankAcct(1)))}
        '            udtdb.RunProc("proc_VoucherTransaction_upd_ReplaceVRAccIDbyTranID", prams4)

        '            'Step 4
        '            Dim udtVRAcctBLL As New Common.Component.VoucherRecipientAccount.VoucherRecipientAccountBLL
        '            udtVRAcctBLL.DeleteTempVRAcctPendingVerify(udtdb, .VRAcctID.Trim)

        '            'Step 5
        '            Dim parms6() As SqlParameter = { _
        '                    udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, strNewVRAcctID), _
        '                    udtdb.MakeInParam("@Voucher_Acc_Type", SqlDbType.Char, 1, .AcctType), _
        '                    udtdb.MakeInParam("@Consent_Form_Printed", SqlDbType.Char, 1, .PrintedConsentForm), _
        '                    udtdb.MakeInParam("@SP_ID", SqlDbType.Char, 8, .CreateSP), _
        '                    udtdb.MakeInParam("@SP_Practice_Display_Seq", SqlDbType.SmallInt, 2, CInt(practiceBankAcct(1))), _
        '                    udtdb.MakeInParam("@Create_By", SqlDbType.VarChar, 20, strUpdateBy), _
        '                    udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdateBy), _
        '                    udtdb.MakeInParam("@DataEntry_By", SqlDbType.VarChar, 20, .DataEntry) _
        '                }
        '            udtdb.RunProc("proc_VoucherAccountCreationLOG_add", parms6)


        '            udtdb.CommitTransaction()
        '        End With
        '    Catch ex As Exception
        '        udtdb.RollBackTranscation()
        '        Throw ex
        '    End Try
        'End Sub

#Region "EC"

        'Public Function chkReadyCreateECAccount(ByVal strSerialNo As String, ByVal strReferenceNo As String, ByVal strSchemeCode As String, ByRef udtVRAcct As VoucherRecipientAccountModel, ByRef udtVoucherScheme As VoucherSchemeModel) As Common.ComObject.SystemMessage

        '    Dim udtVRAcctBLL As VoucherRecipientAccountBLL = New VoucherRecipientAccountBLL
        '    Dim udtVRAcctCollection As VoucherRecipientAccountModelCollection
        '    Dim commfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction
        '    Dim sm As Common.ComObject.SystemMessage

        '    Dim strFunctCode, strSeverity, strMsgCode As String
        '    Dim formatter As Formatter = New Formatter
        '    Dim intUsedVoucher As Integer = 0
        '    Dim intAvailVoucher As Integer = 0
        '    Dim i As Integer = 0

        '    strFunctCode = "020202"
        '    strSeverity = "E"
        '    strMsgCode = ""

        '    ' To Do: Check HKID Exit in Temp & Validated Account

        '    udtVoucherScheme = getVoucherScheme(strSchemeCode)
        '    If udtVoucherScheme Is Nothing Then
        '        'Throw error if the Scheme setting is invalid
        '        strMsgCode = "00004"
        '    Else

        '        udtVRAcct = udtVRAcctBLL.LoadVRAcctByECDetail(strSerialNo, strReferenceNo, strSchemeCode)

        '        If udtVRAcct Is Nothing Then

        '            udtVRAcctCollection = udtVRAcctBLL.LoadTempVRAcctByECDetail(strSerialNo, strReferenceNo, strSchemeCode)
        '            For Each udtTempVRAcct As VoucherRecipientAccountModel In udtVRAcctCollection
        '                If udtTempVRAcct.AcctStatus <> "D" Then
        '                    i = i + 1
        '                End If
        '            Next

        '            'If udtVRAcctCollection.Count < 1 Then
        '            If i < 1 Then
        '                'No Validated Voucher Recipient Account found
        '            Else
        '                strFunctCode = "020402"
        '                strMsgCode = "00002"
        '            End If

        '        Else
        '            strFunctCode = "020402"
        '            strMsgCode = "00002"
        '        End If
        '    End If

        '    If strMsgCode.Equals(String.Empty) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        'Public Function chkSerialNoIdentical(ByVal strSerialNo1 As String, ByVal strSerialNo2 As String) As Common.ComObject.SystemMessage
        '    Dim sm As Common.ComObject.SystemMessage
        '    Dim strFunctCode, strSeverity, strMsgCode As String

        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = ""
        '    If Not strSerialNo1.Equals(strSerialNo2) Then
        '        strMsgCode = "00126"
        '    Else
        '        strMsgCode = ""
        '    End If

        '    If strMsgCode.Equals(String.Empty) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        'Public Function chkReferenceNoIdentical(ByVal strReferenceNo1 As String, ByVal strReferenceNo2 As String) As Common.ComObject.SystemMessage
        '    Dim sm As Common.ComObject.SystemMessage
        '    Dim strFunctCode, strSeverity, strMsgCode As String

        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = ""
        '    If Not strReferenceNo1.Equals(strReferenceNo2) Then
        '        strMsgCode = "00127"
        '    Else
        '        strMsgCode = ""
        '    End If

        '    If strMsgCode.Equals(String.Empty) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        'Public Function chkAgeIdentical(ByVal strAge1 As String, ByVal strAge2 As String) As Common.ComObject.SystemMessage
        '    Dim sm As Common.ComObject.SystemMessage
        '    Dim strFunctCode, strSeverity, strMsgCode As String

        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = ""
        '    If Not strAge1.Equals(strAge2) Then
        '        strMsgCode = "00129"
        '    Else
        '        strMsgCode = ""
        '    End If

        '    If strMsgCode.Equals(String.Empty) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        'Public Function chkDateOfRegistrationIdentical(ByVal strAgeDate1 As String, ByVal strAgeDate2 As String) As Common.ComObject.SystemMessage
        '    Dim sm As Common.ComObject.SystemMessage
        '    Dim strFunctCode, strSeverity, strMsgCode As String

        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = ""
        '    If Not strAgeDate1.Equals(strAgeDate2) Then
        '        strMsgCode = "00130"
        '    Else
        '        strMsgCode = ""
        '    End If

        '    If strMsgCode.Equals(String.Empty) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function

        'Public Function chkECDateIdentical(ByVal strAgeDate1 As String, ByVal strAgeDate2 As String) As Common.ComObject.SystemMessage
        '    Dim sm As Common.ComObject.SystemMessage
        '    Dim strFunctCode, strSeverity, strMsgCode As String

        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = ""
        '    If Not strAgeDate1.Equals(strAgeDate2) Then
        '        strMsgCode = "00128"
        '    Else
        '        strMsgCode = ""
        '    End If

        '    If strMsgCode.Equals(String.Empty) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm

        'End Function

        'Public Function chkDOBTypeIdentical(ByVal strDOBType1 As String, ByVal strDOBType2 As String) As Common.ComObject.SystemMessage
        '    Dim sm As Common.ComObject.SystemMessage
        '    Dim strFunctCode, strSeverity, strMsgCode As String

        '    strFunctCode = "990000"
        '    strSeverity = "E"
        '    strMsgCode = ""
        '    If Not strDOBType1.Equals(strDOBType2) Then
        '        strMsgCode = "00131"
        '    Else
        '        strMsgCode = ""
        '    End If

        '    If strMsgCode.Equals(String.Empty) Then
        '        sm = Nothing
        '    Else
        '        sm = New Common.ComObject.SystemMessage(strFunctCode, strSeverity, strMsgCode)
        '    End If

        '    Return sm
        'End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

#End Region
    End Class

End Namespace
