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

    Public Class ClaimVoucherBLL

        ' CRE14-016 (To introduce 'Deceased' status into eHS) [Start][Winnie]
        ' Obsolete functions that are no longer used
        ' -----------------------------------------------------------------------------------------  

        'Dim udtVRAcctBLL As VoucherRecipientAccountBLL = New VoucherRecipientAccountBLL
        'Dim udtClaimTranBLL As ClaimTransBLL = New ClaimTransBLL
        Dim udtSPBLL As ServiceProviderBLL = New ServiceProviderBLL
        Dim udtDataEntryBLL As DataEntryUserBLL = New DataEntryUserBLL

        Public Function loadSP(ByVal strSPID As String, Optional ByVal enumSubPlatform As EnumHCSPSubPlatform = EnumHCSPSubPlatform.HK) As ServiceProviderModel
            Dim udtSP As ServiceProviderModel
            udtSP = udtSPBLL.GetServiceProviderBySPID(New Database, strSPID)

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            ' Filter schemes by Subplatform
            udtSP.FilterByHCSPSubPlatform(enumSubPlatform)
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            Return udtSP
        End Function

        'Public Function loadSPSession() As ServiceProviderModel
        '    Return udtSPBLL.GetSP()
        'End Function
       
        Public Sub updatePrintOption(ByVal strSPID As String, ByVal strDataEntryAccount As String, ByVal strPrintOption As String)
            If strDataEntryAccount Is Nothing Then
                strDataEntryAccount = String.Empty
            End If

            If strDataEntryAccount.Equals(String.Empty) Then
                Me.udtSPBLL.UpdatePrintOption(strSPID, strPrintOption)
            Else
                Me.udtDataEntryBLL.UpdatePrintOption(strSPID, strDataEntryAccount, strPrintOption)
            End If
        End Sub

        'Public Function loadVRAcctSession() As VoucherRecipientAccountModel
        '    Return udtVRAcctBLL.GetVRAcct()
        'End Function

        'Public Function getAvailVoucher(ByVal udtVRAcct As VoucherRecipientAccountModel, ByVal strSchemeCode As String) As Integer
        '    Dim intRes As Integer = 0
        '    Dim intAvailVoucher As Integer = 0
        '    Dim udtVRAcctBLL As VoucherRecipientAccountBLL = New VoucherRecipientAccountBLL

        '    intAvailVoucher = udtVRAcctBLL.getAvailVoucher(udtVRAcct, strSchemeCode)

        '    intRes = intAvailVoucher
        '    Return intRes
        'End Function
        ' CRE14-016 (To introduce 'Deceased' status into eHS) [End][Winnie]

    End Class


End Namespace
