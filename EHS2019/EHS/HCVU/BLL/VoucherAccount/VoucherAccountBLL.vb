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

    Public Shared Function GetCName(ByVal udtEHSPersonalInformation As EHSAccountModel.EHSPersonalInformationModel) As String
        Dim udtVoucherAccountBLL As VoucherAccountBLL = New VoucherAccountBLL()
        Dim strCName As String = String.Empty

        If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode1) Then
            strCName += udtVoucherAccountBLL.getCCCodeBig5(udtEHSPersonalInformation.CCCode1)
        End If

        If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode2) Then
            strCName += udtVoucherAccountBLL.getCCCodeBig5(udtEHSPersonalInformation.CCCode2)
        End If

        If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode3) Then
            strCName += udtVoucherAccountBLL.getCCCodeBig5(udtEHSPersonalInformation.CCCode3)
        End If

        If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode4) Then
            strCName += udtVoucherAccountBLL.getCCCodeBig5(udtEHSPersonalInformation.CCCode4)
        End If

        If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode5) Then
            strCName += udtVoucherAccountBLL.getCCCodeBig5(udtEHSPersonalInformation.CCCode5)
        End If

        If Not String.IsNullOrEmpty(udtEHSPersonalInformation.CCCode6) Then
            strCName += udtVoucherAccountBLL.getCCCodeBig5(udtEHSPersonalInformation.CCCode6)
        End If

        Return strCName
    End Function

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
