Imports Common.ComFunction
Imports Common.Component
Imports Common.DataAccess
Imports Common.Format
Imports Common.Component.Practice
Imports Common.Component.Scheme
Imports System.Data
Imports System.Data.SqlClient

Namespace BLL

    Public Class ConsentFormPrintOptionBLL

        Dim udtGenFunct As New Common.ComFunction.GeneralFunction
        Dim udtSchemeClaimBLL As New Scheme.SchemeClaimBLL()

        Private udtDB As Database = New Database

        Public Const SESS_ConsentFormPrintOption As String = "ConsentFormPrintOption"

        Public Function Exist() As Boolean
            If HttpContext.Current.Session Is Nothing Then Return False
            If Not HttpContext.Current.Session(SESS_ConsentFormPrintOption) Is Nothing Then
                Return True
            Else
                Return False
            End If
        End Function

        Public Sub ClearSession()
            HttpContext.Current.Session(SESS_ConsentFormPrintOption) = Nothing
        End Sub


        Public Sub SaveToSession(ByRef strConsentFormPrintOption As String)
            HttpContext.Current.Session(SESS_ConsentFormPrintOption) = strConsentFormPrintOption
        End Sub

        Public Function LoadToSession()
            Return HttpContext.Current.Session(SESS_ConsentFormPrintOption)
        End Function

        'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        ''' <summary>
        ''' Get the lists of practice type
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CheckPrintOptionEnabled(ByVal strSPID As String, _
                                               ByVal enumSubPlatform As [Enum], _
                                               Optional ByVal strDataEntry As String = "") As String

            ' Reminder: Only 1 Scheme Seq Entry of SchemeClaim Effective at a time.
            '           Effective of SchemeClaim should have no grap between
            '           Check the SchemeClaim Effective & Look down to related SubsidizeGroupClaim

            Dim udtPracticeSchemeInfoModelCollection As PracticeSchemeInfo.PracticeSchemeInfoModelCollection
            Dim udtPracticeSchemeInfoBLL As New PracticeSchemeInfo.PracticeSchemeInfoBLL
            'Dim udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel
            Dim dtmCurrentDate As Date
            Dim blnPrintOptionBothVersion As Boolean = False
            Dim blnPrintOptionFullVersion As Boolean = False
            Dim blnPrintOptionCondensedVersion As Boolean = False
            Dim blnSubsidizeExist As Boolean


            ' Retrieve Scheme Code and Subsidize Code
            If Not strDataEntry.Equals(String.Empty) Then
                udtPracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.getActivePracticeSchemeInfoBySPDataEntry(strSPID, strDataEntry)
            Else
                udtPracticeSchemeInfoModelCollection = udtPracticeSchemeInfoBLL.getActivePracticeSchemeInfoBySP(strSPID)
            End If

            Dim strSchemeAndSubsidy(udtPracticeSchemeInfoModelCollection.Count, 2) As String
            Dim cnt As Integer = 1
            For Each udtPracticeSchemeInfoModel As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPracticeSchemeInfoModelCollection.Values
                strSchemeAndSubsidy(cnt, 1) = udtPracticeSchemeInfoModel.SchemeCode
                strSchemeAndSubsidy(cnt, 2) = udtPracticeSchemeInfoModel.SubsidizeCode
                cnt += 1
            Next

            ' Retrieve ServiceProvider Enrolled Scheme (SchemeBackOffice)
            Dim udtSchemeInfoBLL As New SchemeInformation.SchemeInformationBLL()
            Dim udtSchemeInformationModelCollection As SchemeInformation.SchemeInformationModelCollection = udtSchemeInfoBLL.GetSchemeInfoListPermanent(strSPID, New Common.DataAccess.Database)

            ' Convert the Enrolled Scheme (SchemeBackOffice) to SchemeClaim
            Dim lstStrSchemeClaimCode As List(Of String) = udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtSchemeInformationModelCollection)

            Dim udtSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllEffectiveSchemeClaimBySchemeCodeList(lstStrSchemeClaimCode)

            'Fill all active subsidize group in SubsidizeGroupClaimList of each SchemeClaim from Cache
            udtSchemeClaimBLL.FillAllActiveSubsidizeGroup(udtSchemeClaimList)

            ''Filter
            'For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList
            '    udtSchemeClaim.SubsidizeGroupClaimList = udtSchemeClaim.SubsidizeGroupClaimList.FilterBySchemeCodeAndSubsidizeCode()
            'Next

            dtmCurrentDate = udtGenFunct.GetSystemDateTime()

            ' Any of the Entitled SubsidizeGroupClaim Contain PrintOption, then return true
            For Each udtSchemeClaim As SchemeClaimModel In udtSchemeClaimList
                'INT15-0011 (Fix empty print option in HCSP Account) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList.Filter(dtmCurrentDate)
                For Each udtSubsidizeGroupClaim As SubsidizeGroupClaimModel In udtSchemeClaim.SubsidizeGroupClaimList.FilterLatestSchemeSeqWithoutFutureDate(dtmCurrentDate)
                    blnSubsidizeExist = False
                    For cntSubsidizeGroup As Integer = 1 To strSchemeAndSubsidy.GetUpperBound(0)
                        'If udtSubsidizeGroupClaim.SchemeCode = strSchemeAndSubsidy(cntSubsidizeGroup, 1) And _
                        'udtSubsidizeGroupClaim.SubsidizeCode = strSchemeAndSubsidy(cntSubsidizeGroup, 2) Then
                        If udtSubsidizeGroupClaim.SchemeCode = Trim(udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(strSchemeAndSubsidy(cntSubsidizeGroup, 1))) And _
                            udtSubsidizeGroupClaim.SubsidizeCode = strSchemeAndSubsidy(cntSubsidizeGroup, 2) Then
                            'INT15-0011 (Fix empty print option in HCSP Account) [End][Chris YIM]
                            blnSubsidizeExist = True
                        End If
                    Next

                    If enumSubPlatform.Equals(EnumHCSPSubPlatform.CN) And blnSubsidizeExist Then
                        If udtSubsidizeGroupClaim.PrintOptionAvailable_CN Then
                            Select Case udtSubsidizeGroupClaim.ConsentFormAvailableVersion_CN
                                Case PrintFormAvailableVersion.Both
                                    blnPrintOptionBothVersion = True
                                Case PrintFormAvailableVersion.Full
                                    blnPrintOptionFullVersion = True
                                Case PrintFormAvailableVersion.Condense
                                    blnPrintOptionCondensedVersion = True
                            End Select
                        End If
                    ElseIf enumSubPlatform.Equals(EnumHCSPSubPlatform.HK) And blnSubsidizeExist Then
                        If udtSubsidizeGroupClaim.PrintOptionAvailable Then
                            Select Case udtSubsidizeGroupClaim.ConsentFormAvailableVersion
                                Case PrintFormAvailableVersion.Both
                                    blnPrintOptionBothVersion = True
                                Case PrintFormAvailableVersion.Full
                                    blnPrintOptionFullVersion = True
                                Case PrintFormAvailableVersion.Condense
                                    blnPrintOptionCondensedVersion = True
                            End Select
                        End If
                    End If
                Next
            Next

            If blnPrintOptionBothVersion Or (blnPrintOptionFullVersion And blnPrintOptionCondensedVersion) Then
                Return PrintFormAvailableVersion.Both
            End If

            If blnPrintOptionFullVersion Then
                Return PrintFormAvailableVersion.Full
            End If

            If blnPrintOptionCondensedVersion Then
                Return PrintFormAvailableVersion.Condense
            End If

            Return Nothing
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Public Function GetCurrentPrintOption(ByVal blnPrintOptionAvailable As Boolean, ByVal strConsentFormAvailableVersion As String, ByVal strUserPrintOption As String) As String

            If blnPrintOptionAvailable Then

                Select Case strConsentFormAvailableVersion
                    Case PrintFormAvailableVersion.Both
                        '
                    Case PrintFormAvailableVersion.Full
                        If strUserPrintOption = PrintFormOptionValue.PrintConsentOnly Then
                            Return PrintFormOptionValue.PrintPurposeAndConsent
                        End If

                    Case PrintFormAvailableVersion.Condense
                        If strUserPrintOption = PrintFormOptionValue.PrintPurposeAndConsent Then
                            Return PrintFormOptionValue.PrintConsentOnly
                        End If
                End Select

                'If no change, return user print option
                Return strUserPrintOption
            Else
                'Print Option not available, change Print Option by Consent Form Available Version
                Select Case strConsentFormAvailableVersion
                    Case PrintFormAvailableVersion.Both
                        Throw New Exception(String.Format("Unexpected print option setting (Consent Form Available Version = {0}, Print Option Available = {1} ", PrintFormAvailableVersion.Both, IIf(blnPrintOptionAvailable, "Y", "N")))

                    Case PrintFormAvailableVersion.Full
                        Return PrintFormOptionValue.PrintPurposeAndConsent

                    Case PrintFormAvailableVersion.Condense
                        Return PrintFormOptionValue.PrintConsentOnly
                End Select
            End If

            Return Nothing
        End Function
        'CRE13-019-02 Extend HCVS to China [End][Winnie]
    End Class

End Namespace