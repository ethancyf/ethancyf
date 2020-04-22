' CRE13-018 - Change Voucher Amount to 1 Dollar [Tommy L]
' -----------------------------------------------------------------------------------------
' Relocated from [FGS]

Imports GrapeCity.ActiveReports.SectionReportModel
Imports System.Reflection
Imports Common.Component.VersionControl
Imports Common.ComFunction
Imports Common.DataAccess
Imports Common.Format

Namespace PrintOut.ConsentFormInformation

    Public Class ConsentFormInformationBLL

#Region "Field"
        Private _Request As HttpRequest
#End Region

#Region "Constructor"
        Public Sub New(ByVal Request As HttpRequest)
            _Request = Request
        End Sub
#End Region

#Region "Function"
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
        ' -----------------------------------------------------------------------------------------
        'Public Function FillConsentFormInformation() As ConsentFormInformationModel
        '    Dim udtCFInfo As New ConsentFormInformationModel

        '    With udtCFInfo
        '        .Platform = ConsentFormInformationModel.EnumPlatform.FGS
        '        .RequestBy = GetPostValue("RequestBy")
        '        .FormType = GetPostValue("FormType")
        '        .Language = GetPostValue("Language")

        '        ' Convert Language from external to internal
        '        If .Language = ConsentFormInformationModel.LanguageClassExternal.Chinese Then
        '            .Language = ConsentFormInformationModel.LanguageClassInternal.Chinese
        '        ElseIf .Language = ConsentFormInformationModel.LanguageClassExternal.English Then
        '            .Language = ConsentFormInformationModel.LanguageClassInternal.English
        '        End If

        '        ' If FormStyle not provided, set to Full
        '        .FormStyle = GetPostValue("FormStyle")
        '        If .FormStyle = String.Empty Then .FormStyle = ConsentFormInformationModel.FormStyleClass.Full

        '        ' If NeedPassword is not provided, set to No
        '        .NeedPassword = GetPostValue("NeedPassword")
        '        If .NeedPassword = String.Empty Then .NeedPassword = ConsentFormInformationModel.NeedPasswordClass.No

        '        .DocType = GetPostValue("DocType")

        '        .SPName = GetPostValue("SPName").ToUpper
        '        .RecipientEName = GetPostValue("RecpName").ToUpper
        '        .RecipientCName = GetPostValue("RecpNameChi")

        '        .ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.No

        '        Select Case .DocType
        '            Case ConsentFormInformationModel.DocTypeClass.HKIC
        '                .DocNo = GetPostValue("HKICNo")
        '                .DOI = GetPostValue("DocDOIStr")
        '                .ReadSmartID = ConsentFormInformationModel.ReadSmartIDClass.Unknown
        '                .ReadSmartID = GetPostValue("UseSmartIC")

        '            Case ConsentFormInformationModel.DocTypeClass.EC
        '                .DocNo = GetPostValue("HKICNo")
        '                .ECSerialNo = GetPostValue("ECSerialNo")
        '                .ECReferenceNo = GetPostValue("ECRefNo")
        '                .DOI = GetPostValue("DocDOIStr")

        '            Case ConsentFormInformationModel.DocTypeClass.HKBC
        '                .DocNo = GetPostValue("HKBCRegNo")

        '            Case ConsentFormInformationModel.DocTypeClass.REPMT
        '                .DocNo = GetPostValue("REPMTPermitNo")
        '                .DOI = GetPostValue("DocDOIStr")

        '            Case ConsentFormInformationModel.DocTypeClass.DocI
        '                .DocNo = GetPostValue("DocIDocNo")
        '                .DOI = GetPostValue("DocDOIStr")

        '            Case ConsentFormInformationModel.DocTypeClass.ID235B
        '                .DocNo = GetPostValue("ID235BBirthEntryNo")
        '                .PermitUntil = GetPostValue("ID235BRemainUntilStr")

        '            Case ConsentFormInformationModel.DocTypeClass.VISA
        '                .DocNo = GetPostValue("VisaNo")
        '                .PassportNo = GetPostValue("PassportNo")

        '            Case ConsentFormInformationModel.DocTypeClass.ADOPC
        '                .DocNo = GetPostValue("ADOPCEntryNo")

        '        End Select

        '        .ServiceDate = GetPostValue("ServiceDateStr")
        '        .SignDate = GetPostValue("SignDateStr")

        '        Select Case .FormType
        '            Case ConsentFormInformationModel.FormTypeClass.HCVS
        '                .VoucherClaim = GetPostValue("VoucherClaimed")

        '                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        '                ' -----------------------------------------------------------------------------------------

        '                .CoPaymentFee = GetPostValue("CoPaymentFee")

        '                ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

        '            Case ConsentFormInformationModel.FormTypeClass.CIVSS
        '                .SubsidyInfo = GetPostValue("SubsidyCode")
        '                .Preschool = GetPostValue("CIVSSPreSchool")

        '                If .SubsidyInfo = ConsentFormInformationModel.CIVSSSubsidyInfoClass.Dose2 Then
        '                    .Preschool = ConsentFormInformationModel.PreschoolClass.Not1stDose
        '                ElseIf .SubsidyInfo = String.Empty Then
        '                    .Preschool = ConsentFormInformationModel.PreschoolClass.Unknown
        '                End If

        '                .Gender = GetPostValue("RecpGender")
        '                .DOB = GetPostValue("RecpDOBStr")

        '            Case ConsentFormInformationModel.FormTypeClass.EVSS
        '                .SubsidyInfo = GetPostValue("SubsidyCode")
        '                .Gender = GetPostValue("RecpGender")
        '                .DOB = GetPostValue("RecpDOBStr")

        '        End Select

        '    End With

        '    Return udtCFInfo

        'End Function

        'Private Function GetPostValue(ByVal strParmName As String) As String
        '    If Not IsNothing(_Request.Form(strParmName)) Then
        '        Return CStr(_Request.Form(strParmName)).Trim
        '    Else
        '        Return String.Empty
        '    End If
        'End Function
        ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
        ' -----------------------------------------------------------------------------------------
        Public Shared Function GetReport(ByVal udtCFInfo As ConsentFormInformationModel) As GrapeCity.ActiveReports.SectionReport
            Dim formatter As Formatter = New Formatter()

            Dim strOption As String = String.Empty
            Dim strDocNo As String = String.Empty

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            If udtCFInfo.FormType = ConsentFormInformationModel.FormTypeClass.HCVS OrElse
                udtCFInfo.FormType = ConsentFormInformationModel.FormTypeClass.HCVSC OrElse
                udtCFInfo.FormType = ConsentFormInformationModel.FormTypeClass.HCVSDHC Then
                ' CRE19-006 (DHC) [End][Winnie]

                If udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.HKIC Or udtCFInfo.DocType = ConsentFormInformationModel.DocTypeClass.EC Then
                    udtCFInfo.DocNo = formatter.formatHKID(udtCFInfo.DocNo, False)
                End If
            End If

            ' CRE19-006 (DHC) [Start][Winnie]
            ' ----------------------------------------------------------------------------------------
            Select Case udtCFInfo.FormType
                Case ConsentFormInformationModel.FormTypeClass.HCVS, ConsentFormInformationModel.FormTypeClass.HCVSDHC
                    ' CRE19-006 (DHC) [End][Winnie]
                    strOption += "H"
                    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                Case ConsentFormInformationModel.FormTypeClass.HCVSC
                    strOption += "V"
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]
                Case ConsentFormInformationModel.FormTypeClass.CIVSS
                    strOption += "C"
                Case ConsentFormInformationModel.FormTypeClass.EVSS
                    strOption += "E"
            End Select

            Select Case udtCFInfo.Language
                Case ConsentFormInformationModel.LanguageClassInternal.Chinese
                    strOption += "C"
                Case ConsentFormInformationModel.LanguageClassInternal.English
                    strOption += "E"
                    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                Case ConsentFormInformationModel.LanguageClassInternal.SimpChinese
                    strOption += "S"
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]
            End Select

            Select Case udtCFInfo.FormStyle
                Case ConsentFormInformationModel.FormStyleClass.Full
                    strOption += "F"
                Case ConsentFormInformationModel.FormStyleClass.Condensed
                    strOption += "C"
            End Select

            Select Case strOption

                ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
                ' -----------------------------------------------------------------------------------------
                Case "HCF"
                    'Return New PrintOut.VoucherConsentForm_CHI.VoucherConsentForm_CHI(udtCFInfo)
                    Return LoadReportVersion(udtCFInfo, "VoucherConsentForm_CHI")
                Case "HCC"
                    'Return New PrintOut.VoucherConsentForm_CHI.VoucherConsentCondensedForm_CHI(udtCFInfo)
                    Return LoadReportVersion(udtCFInfo, "VoucherConsentCondensedForm_CHI")
                Case "HEF"
                    'Return New PrintOut.VoucherConsentForm.VoucherConsentForm(udtCFInfo)
                    Return LoadReportVersion(udtCFInfo, "VoucherConsentForm")
                Case "HEC"
                    'Return New PrintOut.VoucherConsentForm.VoucherConsentCondensedForm(udtCFInfo)
                    Return LoadReportVersion(udtCFInfo, "VoucherConsentCondensedForm")
                    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]

                    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
                Case "VSF"
                    Return LoadReportVersion(udtCFInfo, "VoucherConsentForm_CN")
                    'CRE13-019-02 Extend HCVS to China [End][Winnie]

                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
                    ' -----------------------------------------------------------------------------------------
                    'Case "CCF"
                    '    Return New PrintOut.CIVSSConsentForm_CHI.CIVSSConsentForm_CHI(udtCFInfo)
                    'Case "CCC"
                    '    Return New PrintOut.CIVSSConsentForm_CHI.CIVSSConsentCondensedForm_CHI(udtCFInfo)
                    'Case "CEF"
                    '    Return New PrintOut.CIVSSConsentForm.CIVSSConsentForm(udtCFInfo)
                    'Case "CEC"
                    '    Return New PrintOut.CIVSSConsentForm.CIVSSConsentCondensedForm(udtCFInfo)
                    'Case "ECF"
                    '    Return New PrintOut.EVSSConsentForm_CHI.EVSSConsentForm_CHI(udtCFInfo)
                    'Case "ECC"
                    '    Return New PrintOut.EVSSConsentForm_CHI.EVSSConsentCondensedForm_CHI(udtCFInfo)
                    'Case "EEF"
                    '    Return New PrintOut.EVSSConsentForm.EVSSConsentForm(udtCFInfo)
                    'Case "EEC"
                    '    Return New PrintOut.EVSSConsentForm.EVSSConsentCondensedForm(udtCFInfo)
                    ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
                Case Else
                    Throw New Exception("Unknown report")

            End Select

            Return Nothing
        End Function

        Protected Shared Function LoadReportVersion(ByVal udtCFInfo As ConsentFormInformationModel, ByVal strReportName As String) As GrapeCity.ActiveReports.SectionReport
            Dim ass As Assembly
            Dim assType As Type
            Dim strFullName As String
            ' CRE13-018 - Change Voucher Amount to 1 Dollar [Start][Tommy L]
            ' -----------------------------------------------------------------------------------------
            'Dim db As New Common.DataAccess.Database(DBFlag.dbEVS_InterfaceLog)
            Dim db As New Database()

            ' CRE13-018 - Change Voucher Amount to 1 Dollar [End][Tommy L]
            ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
            'Fix for getting too many DB datetime
            Dim dtmSystem As DateTime = (New GeneralFunction).GetSystemDateTime()
            ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

            ass = [Assembly].GetExecutingAssembly
            For Each assType In ass.GetTypes
                If assType.IsClass Then

                    ' CRE12-014 - Relax 500 rows limit in back office platform [Start][KarlL]
                    'Fix for getting too many DB datetime
                    If assType.FullName.EndsWith(VersionControlBLL.GetVersionControlListByLogicalName(strReportName, db, dtmSystem).PhysicalName) Then
                        'If assType.FullName.EndsWith(VersionControlBLL.GetVersionControlListByLogicalName(strReportName, db).PhysicalName) Then
                        ' CRE12-014 - Relax 500 rows limit in back office platform [End][KarlL]

                        strFullName = assType.FullName
                        Return Activator.CreateInstance(assType, New Object() {udtCFInfo})
                        Exit For
                    End If
                End If
            Next
            Return Nothing
        End Function

        ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
#End Region

    End Class

End Namespace
