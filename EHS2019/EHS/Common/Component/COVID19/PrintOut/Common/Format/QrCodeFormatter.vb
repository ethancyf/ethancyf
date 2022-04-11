' CRE13-018 - Change Voucher Amount to 1 Dollar [Tommy L]
' -----------------------------------------------------------------------------------------
' Relocated from [FGS]
Imports Common.Format
Imports GrapeCity.ActiveReports.SectionReportModel
Imports Common.Component.EHSTransaction
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSAccount
Imports Common.Component.COVID19.PrintOut.Common.Format.Formatter

Namespace Component.COVID19.PrintOut.Common.QrCodeFormatter

    Public Class QrcodeFormatter
        Public Function GenerateQRCodeStringForVaccinationRecord(ByRef udtEHSTransaction As EHSTransactionModel, _
                                                                 ByRef udtVaccinationRecord As VaccinationCardRecordModel, _
                                                                 ByRef udtEHSAccount As EHSAccountModel, _
                                                                 ByRef dtmPrintTime As Date, _
                                                                 ByVal blnDischarge As Boolean) As String

            Dim udtGeneralFunction As New ComFunction.GeneralFunction
            Dim udtCOVID19BLL As New COVID19.COVID19BLL
            Dim udtPrintoutHelper As New PrintoutHelper

            Dim udtPatientInformation As EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode())

            Dim strRawData As String = String.Empty
            Dim strPrefix1AndPrefix2 As String = "HKSARG|VAC"
            'Dim strQRCodeVersion As String = "|3"
            Dim strQRCodeVersion As String = "|-"
            'Dim strKeyVersion As String = "|1"
            Dim strKeyVersion As String = "|-"
            Dim strRefNo As String = "|"
            Dim strMaskedDocId As String = "|" + FormatDocIdentityNoForQrCodeDisplay(udtPatientInformation.DocCode, udtPatientInformation.IdentityNum, True, udtPatientInformation.AdoptionPrefixNum)
            Dim strMaskedName As String = "|" + maskEnglishNameByStar(udtPatientInformation.EName)


            Dim strSecondLast_DoseDate As String = "|"
            Dim strSecondLast_VaccineName As String = "|"
            Dim strSecondLast_VaccineNameTC As String = "|"
            Dim strSpecialIndicator_1 As String = "|"
            Dim strThirdLast_VaccinePt_1 As String = "|"

            Dim strLast_DoseDate As String = "|"
            Dim strLast_VaccineName As String = "|"
            Dim strLast_VaccineNameTC As String = "|"
            Dim strSpecialIndicator_2 As String = "|"
            Dim strThirdLast_VaccinePt_2 As String = "|"

            Dim strPrintDate As String = "|" + dtmPrintTime.ToString((New Formatter).DisplayVaccinationRecordClockFormat()) + "|"
            'Dim DigitialSignature As String = "|MEUCIQCvLIjK8IpB5Uk0TNgbVQoQy/I3z19yTeVohVeYsmfcnwIgcWF482uALTQUt9Oe8VJ/0K6FxWM1NAmGzDaqa5f9V94="
            Dim strDigitialSignature As String = "-"

            'Get QRCode Version
            Dim strQRCodeVerionValue As String = udtCOVID19BLL.GetQRCodeVersion(PrintoutHelper.FormType.Vaccination)
            If strQRCodeVerionValue <> String.Empty Then
                strQRCodeVersion = "|" + strQRCodeVerionValue
            End If

            'Get Key Version
            If udtCOVID19BLL.GetKeyVersion() <> String.Empty Then
                strKeyVersion = "|" + udtCOVID19BLL.GetKeyVersion()
            End If


            'Ref No.            
            If udtPrintoutHelper.DisplayQRCodeRefNo(PrintoutHelper.FormType.Vaccination) Then
                strRefNo = "|" + (New Formatter).formatSystemNumber(udtEHSTransaction.TransactionID)
            End If

            'Get Vaccine Name & Injection date

            ' Defination:
            ' udtLastDose = Last Dose (Not applicable to 1st dose only case), otherwise, = nothing
            ' udtSecondLastDose = 2nd Last Dose (Not applicable to 1st dose only case), otherwise, show 1st Dose
            ' udtThirdLastDose = 3rd Last Dose = 1st dose when 3rd dose exist
            Dim udtLastDose As VaccinationCardDoseRecordModel = Nothing
            Dim udtSecondLastDose As VaccinationCardDoseRecordModel = Nothing
            Dim udtThirdLastDose As VaccinationCardDoseRecordModel = Nothing

            Dim blnNonLocalRecovered As Boolean = False

            If udtVaccinationRecord IsNot Nothing Then
                udtLastDose = udtVaccinationRecord.getLastDoseRecordForQRCode(VaccinationCardRecordModel.LastDoseSeq.FirstLastDose)
                udtSecondLastDose = udtVaccinationRecord.getLastDoseRecordForQRCode(VaccinationCardRecordModel.LastDoseSeq.SecondLastDose)
                udtThirdLastDose = udtVaccinationRecord.getLastDoseRecordForQRCode(VaccinationCardRecordModel.LastDoseSeq.ThirdLastDose)
            End If

            '===== Last Dose =====
            If (Not udtLastDose Is Nothing) Then
                strLast_VaccineName = "|" + udtLastDose.VaccineCodeForQRCode
                strLast_VaccineNameTC = "|" '+ udtLastDose.VaccineNameChi
                strLast_DoseDate = "|" + FormatDate(udtLastDose.InjectionDate, EnumDateFormat.DDMMYYYY)

                If udtLastDose.NonLocalRecoveredHistory Then blnNonLocalRecovered = True
            End If
            '===== Last Dose =====

            '===== 2nd Last Dose =====
            If (Not udtSecondLastDose Is Nothing) Then
                strSecondLast_VaccineName = "|" + udtSecondLastDose.VaccineCodeForQRCode
                strSecondLast_VaccineNameTC = "|" '+ udtSecondLastDose.VaccineNameChi
                strSecondLast_DoseDate = "|" + FormatDate(udtSecondLastDose.InjectionDate, EnumDateFormat.DDMMYYYY)

                If udtSecondLastDose.NonLocalRecoveredHistory Then blnNonLocalRecovered = True
            End If
            '===== 2nd Last Dose =====

            '===== 3rd Last Dose =====
            If (Not udtThirdLastDose Is Nothing) Then
                strThirdLast_VaccinePt_1 = "|" + FormatDate(udtThirdLastDose.InjectionDate, EnumDateFormat.DDMMYYYY)
                strThirdLast_VaccinePt_1 += "," + udtThirdLastDose.VaccineCodeForQRCode
                strThirdLast_VaccinePt_2 = "|" '+ udtThirdLastDose.VaccineNameChi

                If udtThirdLastDose.NonLocalRecoveredHistory Then blnNonLocalRecovered = True
            End If
            '===== 3rd Last Dose =====

            '===== SpecialIndicator 1 =====
            'Get COVID recovered indicator
            If (blnDischarge OrElse blnNonLocalRecovered) Then
                strSpecialIndicator_1 = "|001"
            End If
            '===== SpecialIndicator 1 =====

            '===== SpecialIndicator 2 =====
            strSpecialIndicator_2 = "|" + FormatDOB_QRCodeVersion(udtPatientInformation) + "," + udtVaccinationRecord.MaxDoseSeq.ToString

            '===== SpecialIndicator 2 =====

            'Generate Digital Signature
            Dim udtQRcode As QRCodeModel = Nothing

            strRawData = String.Concat(strPrefix1AndPrefix2, strQRCodeVersion, strKeyVersion, strRefNo, strMaskedDocId, strMaskedName, _
                                       strSecondLast_DoseDate, strSecondLast_VaccineName, strSecondLast_VaccineNameTC, strSpecialIndicator_1, strThirdLast_VaccinePt_1, _
                                       strLast_DoseDate, strLast_VaccineName, strLast_VaccineNameTC, strSpecialIndicator_2, strThirdLast_VaccinePt_2, strPrintDate)

            udtQRcode = udtCOVID19BLL.GenerateDigitalSignature(strRawData)

            If udtQRcode IsNot Nothing AndAlso udtQRcode.Signature <> String.Empty Then
                strDigitialSignature = udtQRcode.Signature
            End If

            Try
                Dim blnValid As Boolean = udtCOVID19BLL.VerifyDigitalSignature(strRawData, strDigitialSignature)
            Catch ex As Exception

            End Try

            Return String.Concat(strRawData, strDigitialSignature)

        End Function


        Public Function GenerateQRCodeStringForExemptionRecord(ByRef udtEHSTransaction As EHSTransactionModel, _
                                                               ByRef udtEHSAccount As EHSAccountModel, _
                                                               ByRef dtmPrintTime As Date) As String

            Dim udtGeneralFunction As New ComFunction.GeneralFunction
            Dim udtCOVID19BLL As New COVID19.COVID19BLL
            Dim udtPrintoutHelper As New PrintoutHelper

            Dim udtPatientInformation As EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode())

            Dim strRawData As String = String.Empty
            Dim strPrefix1AndPrefix2 As String = "HKSARG|MEC"
            'Dim strQRCodeVersion As String = "|3"
            Dim strQRCodeVersion As String = "|-"
            'Dim strKeyVersion As String = "|1"
            Dim strKeyVersion As String = "|-"
            Dim strRefNo As String = "|"
            Dim strMaskedDocId As String = "|" + FormatDocIdentityNoForQrCodeDisplay(udtPatientInformation.DocCode, udtPatientInformation.IdentityNum, True, udtPatientInformation.AdoptionPrefixNum)
            Dim strMaskedName As String = "|" + maskEnglishNameByStar(udtPatientInformation.EName)

            'Unused for MEC
            Dim strSecondLast_DoseDate As String = "|"
            Dim strSecondLast_VaccineName As String = "|"
            Dim strSecondLast_VaccineNameTC As String = "|"

            Dim strSpecialIndicator_1 As String = "|"
            Dim strSpecialindicator002ValidityEndDate As String = "|"

            'Unused for MEC
            Dim strThirdLast_VaccinePt_1 As String = "|"
            Dim strLast_DoseDate As String = "|"
            Dim strLast_VaccineName As String = "|"
            Dim strLast_VaccineNameTC As String = "|"
            Dim strSpecialIndicator_2 As String = "|"
            Dim strThirdLast_VaccinePt_2 As String = "|"

            Dim strPrintDate As String = "|" + dtmPrintTime.ToString((New Formatter).DisplayVaccinationRecordClockFormat()) + "|"
            'Dim DigitialSignature As String = "|MEUCIQCvLIjK8IpB5Uk0TNgbVQoQy/I3z19yTeVohVeYsmfcnwIgcWF482uALTQUt9Oe8VJ/0K6FxWM1NAmGzDaqa5f9V94="
            Dim strDigitialSignature As String = "-"

            'Get QRCode Version
            Dim strQRCodeVerionValue As String = udtCOVID19BLL.GetQRCodeVersion(PrintoutHelper.FormType.Exemption)
            If strQRCodeVerionValue <> String.Empty Then
                strQRCodeVersion = "|" + strQRCodeVerionValue
            End If

            'Get Key Version
            If udtCOVID19BLL.GetKeyVersion() <> String.Empty Then
                strKeyVersion = "|" + udtCOVID19BLL.GetKeyVersion()
            End If

            'Ref No.
            If udtPrintoutHelper.DisplayQRCodeRefNo(PrintoutHelper.FormType.Exemption) Then
                strRefNo = "|" + (New Formatter).formatSystemNumber(udtEHSTransaction.TransactionID)
            End If

            '===== SpecialIndicator 1 =====
            'Indicator of Exemption Record
            strSpecialIndicator_1 = "|002"
            '===== SpecialIndicator 1 =====

            '===== SpecialIndicator 2 =====
            strSpecialIndicator_2 = "|" + FormatDOB_QRCodeVersion(udtPatientInformation)

            '===== SpecialIndicator 2 =====

            'Valid Until
            strSpecialindicator002ValidityEndDate = "|" + FormatDate(udtEHSTransaction.TransactionAdditionFields.ValidUntil, EnumDateFormat.DDMMYYYY)

            'Generate Digital Signature
            Dim udtQRcode As QRCodeModel = Nothing

            strRawData = String.Concat(strPrefix1AndPrefix2, strQRCodeVersion, strKeyVersion, strRefNo, strMaskedDocId, strMaskedName, _
                                       strSecondLast_DoseDate, strSecondLast_VaccineName, strSecondLast_VaccineNameTC, strSpecialIndicator_1, strSpecialindicator002ValidityEndDate, _
                                       strThirdLast_VaccinePt_1, strLast_DoseDate, strLast_VaccineName, strLast_VaccineNameTC, strSpecialIndicator_2, strThirdLast_VaccinePt_2, strPrintDate)

            udtQRcode = udtCOVID19BLL.GenerateDigitalSignature(strRawData)

            If udtQRcode IsNot Nothing AndAlso udtQRcode.Signature <> String.Empty Then
                strDigitialSignature = udtQRcode.Signature
            End If

            Try
                Dim blnValid As Boolean = udtCOVID19BLL.VerifyDigitalSignature(strRawData, strDigitialSignature)
            Catch ex As Exception

            End Try

            Return String.Concat(strRawData, strDigitialSignature)

        End Function

        Private Function FormatDOB_QRCodeVersion(udtPatientInformation As EHSPersonalInformationModel) As String
            Dim strDOB As String = String.Empty
            Dim strExactDOB As String = udtPatientInformation.ExactDOB

            If udtPatientInformation.DocCode = DocType.DocTypeModel.DocTypeCode.EC AndAlso udtPatientInformation.ExactDOB = EHSAccountModel.ExactDOBClass.AgeAndRegistration Then
                'strDOB = (New Formatter).formatDOB(udtPatientInformation.DOB, udtPatientInformation.ExactDOB, CultureLanguage.English, udtPatientInformation.ECAge, udtPatientInformation.ECDateOfRegistration)
                Dim dtmDOR As Date = CDate(udtPatientInformation.ECDateOfRegistration.Value)
                Dim intAge As Integer = CInt(udtPatientInformation.ECAge)
                Dim dtmDOB As Date

                dtmDOB = DateAdd(DateInterval.Year, -1 * intAge, dtmDOR)

                strDOB = Year(dtmDOB)
            Else
                strDOB = (New Formatter).formatDOB(udtPatientInformation.DOB, udtPatientInformation.ExactDOB, CultureLanguage.English, Nothing, Nothing)
            End If

            Select Case strExactDOB
                Case EHSAccountModel.ExactDOBClass.ExactYear,
                     EHSAccountModel.ExactDOBClass.ManualExactYear,
                     EHSAccountModel.ExactDOBClass.AgeAndRegistration,
                     EHSAccountModel.ExactDOBClass.ReportedYear

                    'Year Only: 01-01-yyyy
                    strDOB = String.Format("01-01-{0}", strDOB)

                Case EHSAccountModel.ExactDOBClass.ExactMonth,
                     EHSAccountModel.ExactDOBClass.ManualExactMonth

                    'Year and Month: 01-MM-yyyy
                    strDOB = String.Format("01-{0}", strDOB)

                Case EHSAccountModel.ExactDOBClass.ExactDate,
                     EHSAccountModel.ExactDOBClass.ManualExactDate

                    'Exact Day: dd-MM-yyyy
                    strDOB = String.Format("{0}", strDOB)

            End Select

            Return strDOB

        End Function

    End Class

End Namespace
