Imports Common.DataAccess
Imports Common.Component.CCCode
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.VoucherRecipientAccount
Imports System.Data.SqlClient

Public Class VoucherAccountBLL

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


#Region "CCCode Supporting Function"

    Public Shared Function GetCName(ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel) As String
        Dim udtCCCodeBLL As New CCCodeBLL
        Dim strCName As String = String.Empty

        If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode1) Then
            strCName += udtCCCodeBLL.getChiCharByCCCode(udtEHSPersonalInformation.CCCode1)
        End If

        If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode2) Then
            strCName += udtCCCodeBLL.getChiCharByCCCode(udtEHSPersonalInformation.CCCode2)
        End If

        If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode3) Then
            strCName += udtCCCodeBLL.getChiCharByCCCode(udtEHSPersonalInformation.CCCode3)
        End If

        If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode4) Then
            strCName += udtCCCodeBLL.getChiCharByCCCode(udtEHSPersonalInformation.CCCode4)
        End If

        If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode5) Then
            strCName += udtCCCodeBLL.getChiCharByCCCode(udtEHSPersonalInformation.CCCode5)
        End If

        If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode6) Then
            strCName += udtCCCodeBLL.getChiCharByCCCode(udtEHSPersonalInformation.CCCode6)
        End If

        Return strCName
    End Function

#End Region

    ' EHS Temp Voucher Account -----------------------

    ''' <summary>
    ''' Create New Temporary EHS Account Model after no record found
    ''' </summary>
    ''' <param name="strIdentityNum"></param>
    ''' <param name="strDocCode"></param>
    ''' <param name="strExactDOB"></param>
    ''' <param name="dtmDOB"></param>
    ''' <param name="strSchemeCode"></param>
    ''' <param name="strAdoptionPrefixNum"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ConstructEHSTemporaryVoucherAccount(ByVal strIdentityNum As String, _
                                                        ByVal strDocCode As String, _
                                                        ByVal strExactDOB As String, _
                                                        ByVal dtmDOB As DateTime, _
                                                        ByVal strSchemeCode As String, _
                                                        ByVal strAdoptionPrefixNum As String, _
                                                        Optional ByVal udtEHSPersonalInfo As EHSAccountModel.EHSPersonalInformationModel = Nothing) As EHSAccountModel
        Dim udtEHSAccount As New EHSAccountModel()

        udtEHSAccount.SchemeCode = strSchemeCode
        udtEHSAccount.EHSPersonalInformationList(0).VoucherAccID = String.Empty
        udtEHSAccount.EHSPersonalInformationList(0).IdentityNum = strIdentityNum
        udtEHSAccount.EHSPersonalInformationList(0).ExactDOB = strExactDOB
        udtEHSAccount.EHSPersonalInformationList(0).DOB = dtmDOB
        udtEHSAccount.EHSPersonalInformationList(0).DocCode = strDocCode
        If strAdoptionPrefixNum Is Nothing Then
            strAdoptionPrefixNum = String.Empty
        End If
        udtEHSAccount.EHSPersonalInformationList(0).AdoptionPrefixNum = strAdoptionPrefixNum
        udtEHSAccount.EHSPersonalInformationList(0).SetDOBTypeSelected(False)

        If strDocCode = DocTypeModel.DocTypeCode.EC AndAlso Not udtEHSPersonalInfo Is Nothing Then
            udtEHSAccount.EHSPersonalInformationList(0).ECSerialNoNotProvided = udtEHSPersonalInfo.ECSerialNoNotProvided
            udtEHSAccount.EHSPersonalInformationList(0).ECSerialNo = udtEHSPersonalInfo.ECSerialNo
        End If

        Return udtEHSAccount

    End Function

End Class
