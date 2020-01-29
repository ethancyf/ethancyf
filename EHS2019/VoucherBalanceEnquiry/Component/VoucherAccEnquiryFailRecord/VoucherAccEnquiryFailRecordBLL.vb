Imports Common.Component.VoucherRecipientAccount
Imports Common.DataAccess
Imports System.Data.SqlClient
Imports Common.Component

Namespace Component.VoucherAccEnquiryFailRecord
    Public Class VoucherAccEnquiryFailRecordBLL

        Private udcGeneralF As New Common.ComFunction.GeneralFunction

        Public Sub New()

        End Sub

        'Public Sub UpdateVoucherAccEnquiryFailRecord(ByVal db As Database, ByVal strVoucherAccID As String)

        '    Dim prams() As SqlParameter = { _
        '    db.MakeInParam("@Voucher_Acc_ID", SqlDbType.VarChar, 15, strVoucherAccID)}
        '    db.RunProc("proc_VoucherAccEnquiryFailRecord_add", prams)

        'End Sub

        Public Function UpdateFailCount(ByVal strVRAccID As String) As Boolean
            Dim blnRes As Boolean = False
            Dim strtemp As String = String.Empty
            Dim blnSuccess As Boolean = False
            Dim intUserFailCount As Integer = 0

            udcGeneralF.getSystemParameter("MaxPublicEnquiryFailCount", strtemp, String.Empty)
            Dim intFailCount As Integer = CInt(strtemp)
            Dim strExistingPublicEnquiryStatus As String

            Dim udtVRAcctBLL As VoucherRecipientAccountBLL = New VoucherRecipientAccountBLL

            blnSuccess = udtVRAcctBLL.UpdateVoucherAccEnquiryFailCount(strVRAccID)
            If blnSuccess Then
                Try
                    Dim udtPrimaryDB As Database = New Database(DBFlagStr.DBFlag2)

                    intUserFailCount = udtVRAcctBLL.GetVoucherAccEnquiryFailCount(strVRAccID)
                    strExistingPublicEnquiryStatus = udtVRAcctBLL.GetPublicEnquiryStatus(strVRAccID, udtPrimaryDB)
                    If intUserFailCount >= intFailCount AndAlso Not strExistingPublicEnquiryStatus.Equals(VRAcctEnquiryStatus.ManualSuspended) Then
                        blnRes = udtVRAcctBLL.UpdatePublicEnquiryStatus(strVRAccID, udtPrimaryDB)
                    End If

                Catch ex As Exception

                End Try

            End If
            Return blnRes
        End Function

    End Class
End Namespace

