Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.Component.VoucherRecipientAccount
Imports Common.Component

Namespace BLL
    Public Class VoucherAccEnquiryFailRecordBLL

        Private udcGeneralF As New Common.ComFunction.GeneralFunction
        Private Const ConnectionString_Replication As String = DBFlagStr.DBFlag3
        Private Const ConnectionString_Public As String = DBFlagStr.DBFlag2

        Public Sub New()

        End Sub

        'Public Sub UpdateVRAcctEnqFailRecord(ByRef udtVoucherRecipientAcct As VoucherRecipientAccountModel, Optional ByVal udtdb As Database = Nothing)
        '    Dim udtVoucherAccEnquiryFailRecordBLL As New VoucherAccEnquiryFailRecordBLL

        '    If IsNothing(udtdb) Then
        '        udtdb = New Database()
        '    End If
        '    'Dim db As New Database
        '    Try
        '        udtdb.BeginTransaction()
        '        Me.UpdateVoucherAccEnquiryFailRecord(udtdb, udtVoucherRecipientAcct.VRAcctID)
        '        udtdb.CommitTransaction()
        '    Catch ex As Exception
        '        udtdb.RollBackTranscation()
        '        Throw ex
        '    Finally
        '        If Not udtdb Is Nothing Then udtdb.Dispose()
        '    End Try
        'End Sub

        'Private Sub UpdateVoucherAccEnquiryFailRecord(ByVal db As Database, ByVal strVoucherAccID As String)

        '    Dim prams() As SqlParameter = { _
        '    db.MakeInParam("@Voucher_Acc_ID", SqlDbType.VarChar, 15, strVoucherAccID)}
        '    db.RunProc("proc_VoucherAccEnquiryFailRecord_add", prams)
        'End Sub

        Public Function UpdateFailCount(ByVal strVRAccID As String) As Boolean
            'Dim blnRes As Boolean = False
            'Dim strtemp As String = String.Empty
            'Dim blnSuccess As Boolean = False
            'Dim intUserFailCount As Integer = 0

            'Dim udtDB As New Database(ConnectionString_Replication)

            'udcGeneralF.getSystemParameter("MaxPublicEnquiryFailCount", strtemp, String.Empty, udtDB)
            'Dim intFailCount As Integer = CInt(strtemp)
            'Dim strExistingPublicEnquiryStatus As String

            'Dim udtVRAcctBLL As VoucherRecipientAccountBLL = New VoucherRecipientAccountBLL

            'udtDB = New Database(ConnectionString_Replication)
            'blnSuccess = udtVRAcctBLL.UpdateVoucherAccEnquiryFailCount(strVRAccID, udtDB)
            'If blnSuccess Then
            '    Try
            '        Dim udtPrimaryDB As Database = New Database(ConnectionString_Public)

            '        intUserFailCount = udtVRAcctBLL.GetVoucherAccEnquiryFailCount(strVRAccID, udtDB)
            '        strExistingPublicEnquiryStatus = udtVRAcctBLL.GetPublicEnquiryStatus(strVRAccID, udtPrimaryDB)
            '        If intUserFailCount >= intFailCount AndAlso Not strExistingPublicEnquiryStatus.Equals(VRAcctEnquiryStatus.ManualSuspended) Then
            '            blnRes = udtVRAcctBLL.UpdatePublicEnquiryStatus(strVRAccID, udtPrimaryDB)
            '        End If

            '    Catch ex As Exception

            '    End Try

            'End If
            'Return blnRes
            Dim blnRes As Boolean = False
            Dim strtemp As String = String.Empty
            Dim blnSuccess As Boolean = False
            Dim intUserFailCount As Integer = 0

            Dim udtDB As New Database(ConnectionString_Replication)
            udcGeneralF.getSystemParameter("MaxPublicEnquiryFailCount", strtemp, String.Empty, udtDB)

            Dim intFailCount As Integer = CInt(strtemp)
            Dim udtVRAcctBLL As VoucherRecipientAccountBLL = New VoucherRecipientAccountBLL
            Dim udtEHSAccountBLL As EHSAccount.EHSAccountBLL = New EHSAccount.EHSAccountBLL()
            Dim udtEHSAccountModel As EHSAccount.EHSAccountModel = udtEHSAccountBLL.LoadEHSAccountByVRID(strVRAccID)

            blnSuccess = udtVRAcctBLL.UpdateVoucherAccEnquiryFailCount(strVRAccID, udtDB)
            If blnSuccess Then
                Try
                    Dim udtPrimaryDB As Database = New Database(ConnectionString_Public)
                    intUserFailCount = udtVRAcctBLL.GetVoucherAccEnquiryFailCount(strVRAccID, udtDB)
                    If intUserFailCount >= intFailCount AndAlso udtEHSAccountModel.PublicEnquiryStatus <> EHSAccount.EHSAccountModel.EnquiryStatusClass.ManualSuspend Then
                        blnRes = udtVRAcctBLL.UpdatePublicEnquiryStatus(strVRAccID, udtPrimaryDB)
                    End If
                Catch ex As Exception

                End Try

            End If
            Return blnRes
        End Function

    End Class
End Namespace
