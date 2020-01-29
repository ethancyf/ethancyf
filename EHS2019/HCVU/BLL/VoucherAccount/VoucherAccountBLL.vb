Imports Common.Component.CCCode
Imports Common.Component.VoucherRecipientAccount
Imports Common.DataAccess
Imports System.Data.SqlClient

Public Class VoucherAccountBLL

    ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
    ' Obsolete functions that are no longer used
    ' -----------------------------------------------------------------------------------------    

    'Const strSchemeCode As String = "EHCVS"
    'Dim udtVRAcctBLL As VoucherRecipientAccountBLL = New VoucherRecipientAccountBLL

    'Public Sub terminateVRAcct(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String)
    '    udtVRAcctBLL.terminateVRAcct(udtVRAcct, strUpdate_by)
    'End Sub

    'Public Sub suspendVRAcct(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String)
    '    udtVRAcct.AcctSuspendUser = strUpdate_by
    '    udtVRAcctBLL.suspendVRAcct(udtVRAcct)
    'End Sub

    'Public Sub reactivateVRAcct(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String)
    '    udtVRAcctBLL.reactivateVRAcct(udtVRAcct, strUpdate_by)
    'End Sub

    'Public Sub suspendVRAcctEnquiry(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String)
    '    udtVRAcct.EnquirySuspendUser = strUpdate_by
    '    udtVRAcctBLL.suspendVRAcctEnquiry(udtVRAcct)
    'End Sub

    'Public Sub reactivateVRAcctEnquiry(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String)
    '    udtVRAcctBLL.reactivateVRAcctEnquiry(udtVRAcct, strUpdate_by)
    'End Sub

    'Public Sub saveAmendment(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String, ByVal strNeedVerify As String, ByVal strActionType As String, ByVal strRecordstatus As String)
    '    udtVRAcctBLL.addAmendHistory(udtVRAcct, strUpdate_by, strNeedVerify, strActionType, strRecordStatus)
    'End Sub

    'Public Sub cancelAmendment(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String, ByVal strNeedVerify As String, ByVal strRecordStatus As String)
    '    udtVRAcctBLL.updAmendHistory(udtVRAcct, strUpdate_by, strNeedVerify, strRecordStatus)
    'End Sub

    'Public Function getAmendHistory(ByVal strVRAcctID As String) As DataTable
    '    Dim dtRes As DataTable
    '    dtRes = udtVRAcctBLL.getAmendHistory(strVRAcctID)
    '    Return dtRes
    'End Function

    'Public Sub updatePersonalInfo(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String)
    '    udtVRAcctBLL.updatePersonalInfo(udtVRAcct, strUpdate_by)
    'End Sub

    'Public Sub createTempAcct(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strCreate_by As String)
    '    Dim udtcommonfunct As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

    '    udtVRAcct.VRAcctID = udtcommonfunct.generateSystemNum("C")
    '    udtVRAcct.SchemeCode = strSchemeCode
    '    udtVRAcct.TotalUsedVoucherAmount = 0
    '    udtVRAcct.VoucherRedeem = 0
    '    udtVRAcct.AcctStatus = Common.Component.VRAcctValidatedStatus.PendingForVerify
    '    'udtVRAcct.AcctPurpose = Common.Component.VRACreationPurpose.ForAmendment

    '    udtVRAcctBLL.SaveTempVRAcct(udtVRAcct, strCreate_by)
    'End Sub

    'Public Sub updatePersonalInfoStatus(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String, ByVal strRecord_Status As String)
    '    udtVRAcctBLL.updatePersonalInfoStatus(udtVRAcct, strUpdate_by, strRecord_Status)
    'End Sub

    'Public Function getAmendedTempAccount(ByVal strHKID As String, ByVal strSchemeCode As String) As VoucherRecipientAccountModel
    '    Dim udtVRAcctRes As VoucherRecipientAccountModel = New VoucherRecipientAccountModel
    '    Dim udtVRAcctCollection As VoucherRecipientAccountModelCollection
    '    udtVRAcctCollection = udtVRAcctBLL.LoadTempVRAcct(strHKID, strSchemeCode)
    '    For Each udtVRAcct As VoucherRecipientAccountModel In udtVRAcctCollection
    '        If udtVRAcct.PIStatus = "A" And udtVRAcct.AcctPurpose = Common.Component.VRACreationPurpose.ForAmendment Then
    '            udtVRAcctRes = udtVRAcct
    '        End If
    '    Next
    '    Return udtVRAcctRes
    'End Function

    'Public Function getAmendedTempAccountCollection(ByVal strHKID As String, ByVal strSchemeCode As String) As VoucherRecipientAccountModelCollection
    '    Dim udtVRAcctRes As VoucherRecipientAccountModel = New VoucherRecipientAccountModel
    '    Dim udtVRAcctCollection As VoucherRecipientAccountModelCollection
    '    udtVRAcctCollection = udtVRAcctBLL.LoadTempVRAcct(strHKID, strSchemeCode)

    '    Return udtVRAcctCollection
    'End Function

    'Public Function getAmendedTempAccount(ByVal strVRAcctID As String) As VoucherRecipientAccountModel
    '    Dim udtVRAcctRes As VoucherRecipientAccountModel = New VoucherRecipientAccountModel
    '    udtVRAcctRes = udtVRAcctBLL.LoadTempVRAcctByID(strVRAcctID, strSchemeCode)  
    '    Return udtVRAcctRes
    'End Function


    'Public Sub updateTempPersonalInfo(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strUpdate_by As String)
    '    Dim udtFormatter As Common.Format.Formatter = New Common.Format.Formatter
    '    Dim udtdb As Database = New Database

    '    ' CRE15-014 HA_MingLiu UTF32 [Start][Winnie]
    '    Dim parms2() As SqlParameter = { _
    '        udtdb.MakeInParam("@Voucher_Acc_ID", SqlDbType.Char, 15, udtVRAcct.VRAcctID), _
    '        udtdb.MakeInParam("@HKID", SqlDbType.Char, 9, udtVRAcct.HKID), _
    '        udtdb.MakeInParam("@Eng_Name", SqlDbType.VarChar, 40, udtFormatter.formatEnglishName(udtVRAcct.ENameSurName, udtVRAcct.ENameFirstName)), _
    '        udtdb.MakeInParam("@Chi_Name", SqlDbType.NVarChar, 12, udtVRAcct.CName), _
    '        udtdb.MakeInParam("@CCcode1", SqlDbType.Char, 5, udtVRAcct.CCCode1), _
    '        udtdb.MakeInParam("@CCcode2", SqlDbType.Char, 5, udtVRAcct.CCCode2), _
    '        udtdb.MakeInParam("@CCcode3", SqlDbType.Char, 5, udtVRAcct.CCCode3), _
    '        udtdb.MakeInParam("@CCcode4", SqlDbType.Char, 5, udtVRAcct.CCCode4), _
    '        udtdb.MakeInParam("@CCcode5", SqlDbType.Char, 5, udtVRAcct.CCCode5), _
    '        udtdb.MakeInParam("@CCcode6", SqlDbType.Char, 5, udtVRAcct.CCCode6), _
    '        udtdb.MakeInParam("@DOB", SqlDbType.DateTime, 8, udtVRAcct.DOB), _
    '        udtdb.MakeInParam("@Exact_DOB", SqlDbType.Char, 1, udtVRAcct.IsExactDOB), _
    '        udtdb.MakeInParam("@Sex", SqlDbType.Char, 1, udtVRAcct.Gender), _
    '        udtdb.MakeInParam("@Date_of_Issue", SqlDbType.DateTime, 8, udtVRAcct.HKIDIssuseDate), _
    '        udtdb.MakeInParam("@Update_By", SqlDbType.VarChar, 20, strUpdate_by), _
    '        udtdb.MakeInParam("@TSMP", SqlDbType.Timestamp, 16, udtVRAcct.PITSMP) _
    '    }
    '    ' CRE15-014 HA_MingLiu UTF32 [End][Winnie]

    '    udtdb.RunProc("proc_TempPersonalInformation_upd_PersonalInfo", parms2)
    'End Sub

    'Public Function getCCCTail(ByVal strcccode As String, ByRef strDisplay As String) As String
    '    Dim strRes As String
    '    Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL
    '    strRes = String.Empty
    '    strRes = udtCCCodeBLL.GetCCCodeDesc(strcccode, strDisplay)
    '    Return strRes
    'End Function

    'Public Function getChiChar(ByVal strcccode As String) As String
    '    Dim strRes As String
    '    Dim udtCCCodeBLL As CCCodeBLL = New CCCodeBLL
    '    strRes = String.Empty
    '    strRes = udtCCCodeBLL.GetChiChar(strcccode)
    '    Return strRes
    'End Function

    'Public Function getRectifyList() As DataTable
    '    Dim dtRes As DataTable
    '    dtRes = Me.udtVRAcctBLL.getVRAcctRectifyListVU(strSchemeCode)
    '    Return dtRes

    'End Function

    'Public Function getOutstandingVRListFor29days() As DataTable
    '    Dim dtRes As DataTable
    '    dtRes = Me.udtVRAcctBLL.getOutstandingVRAcctListForVUDeletion(strSchemeCode)
    '    Return dtRes

    'End Function

    'Public Function getDeletedList() As DataTable
    '    Dim dtRes As DataTable
    '    dtRes = Me.udtVRAcctBLL.getDeletedList(strSchemeCode)
    '    Return dtRes

    'End Function
    ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

    Public Function GetOutstandingVRAcctRectification() As Integer

        Dim dt As New DataTable
        Dim udtdb As Database = New Database

        Try
            udtdb.RunProc("proc_VoucherAccountRectifyListCnt_get", dt)

            Return CType(dt.Rows(0)(0), Integer)
        Catch eSQL As SqlException
            Throw eSQL
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
