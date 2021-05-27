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
        Public Function GenerateQRCodeString(ByRef udtEHSTransaction As EHSTransactionModel, _
                                             ByRef udtVaccinationRecordHistory As TransactionDetailVaccineModel, _
                                             ByRef udtEHSAccount As EHSAccountModel, _
                                             ByRef dtmPrintTime As Date, _
                                             ByVal blnDischarge As Boolean) As String

            Dim udtGeneralFunction As New ComFunction.GeneralFunction
            Dim udtCOVID19BLL As New COVID19.COVID19BLL

            Dim udtPatientInformation As EHSPersonalInformationModel = udtEHSAccount.getPersonalInformation(udtEHSAccount.SearchDocCode())

            Dim strRawData As String = String.Empty
            Dim strPrefix1AndPrefix2 As String = "HKSARG|VAC"
            'Dim strQRCodeVersion As String = "|3"
            Dim strQRCodeVersion As String = "|-"
            'Dim strKeyVersion As String = "|1"
            Dim strKeyVersion As String = "|-"
            Dim strTransId As String = "|" + (New Formatter).formatSystemNumber(udtEHSTransaction.TransactionID)
            Dim strMaskedDocId As String = "|" + FormatDocIdentityNoForQrCodeDisplay(udtPatientInformation.DocCode, udtPatientInformation.IdentityNum, True, udtPatientInformation.AdoptionPrefixNum)
            Dim strMaskedName As String = "|" + maskEnglishNameByStar(udtPatientInformation.EName)
            Dim strDoseDate_1st As String = "|-"
            Dim strVaccineName_1st As String = "|"
            Dim strVaccineNameTC_1st As String = "|-"
            Dim strBrandName_1st As String = "|"
            Dim strBrandNameTC_1st As String = "|"
            Dim strDoseDate_2nd As String = "|-"
            Dim strVaccineName_2nd As String = "|"
            Dim strVaccineNameTC_2nd As String = "|-"
            Dim strBrandName_2nd As String = "|"
            Dim strBrandNameTC_2nd As String = "|"

            Dim strPrintDate As String = "|" + dtmPrintTime.ToString((New Formatter).DisplayVaccinationRecordClockFormat()) + "|"
            'Dim DigitialSignature As String = "|MEUCIQCvLIjK8IpB5Uk0TNgbVQoQy/I3z19yTeVohVeYsmfcnwIgcWF482uALTQUt9Oe8VJ/0K6FxWM1NAmGzDaqa5f9V94="
            Dim strDigitialSignature As String = "-"

            Dim strCurrentDose As String = udtEHSTransaction.TransactionDetails(0).AvailableItemDesc
            Dim strVaccineLotNo As String = udtEHSTransaction.TransactionAdditionFields.VaccineLotNo
            Dim dt As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingByVaccineLotNo(strVaccineLotNo)

            'Get QRCode Version
            If udtCOVID19BLL.GetQRCodeVersion() <> String.Empty Then
                strQRCodeVersion = "|" + udtCOVID19BLL.GetQRCodeVersion()
            End If

            'Get Key Version
            If udtCOVID19BLL.GetKeyVersion() <> String.Empty Then
                strKeyVersion = "|" + udtCOVID19BLL.GetKeyVersion()
            End If

            'Get Vaccine Name & Injection date
            If (strCurrentDose = "1st Dose") Then
                '===== Normal Case =====
                strVaccineName_1st = "|" + dt.Rows(0)("Brand_Printout_Name")
                strVaccineNameTC_1st = "|" + dt.Rows(0)("Brand_Printout_Name_Chi")
                strDoseDate_1st = "|" + FormatDate(udtEHSTransaction.ServiceDate, EnumDateFormat.DDMMYYYY)
                If blnDischarge Then
                    strBrandName_1st = "|001"
                End If
                '===== Normal Case =====

                '===== date dateback dose record ====
                If (Not udtVaccinationRecordHistory Is Nothing) Then
                    'date dateback fill second dose record
                    If (udtVaccinationRecordHistory.AvailableItemDesc = "2nd Dose") Then
                        strVaccineName_2nd = "|" + udtCOVID19BLL.GetVaccineBrandPrintoutName(udtVaccinationRecordHistory.VaccineBrand)
                        strVaccineNameTC_2nd = "|" + udtCOVID19BLL.GetVaccineBrandPrintoutNameChi(udtVaccinationRecordHistory.VaccineBrand)
                        strDoseDate_2nd = "|" + FormatDate(udtVaccinationRecordHistory.ServiceReceiveDtm, EnumDateFormat.DDMMYYYY)
                        If blnDischarge Then
                            strBrandName_2nd = "|001"
                        End If
                    End If
                Else
                    If blnDischarge Then
                        strDoseDate_2nd = "|NA"
                    End If
                End If
                '===== date dateback dose record ====
            Else
                '===== Normal Case =====
                strVaccineName_2nd = "|" + dt.Rows(0)("Brand_Printout_Name")
                strVaccineNameTC_2nd = "|" + dt.Rows(0)("Brand_Printout_Name_Chi")
                strDoseDate_2nd = "|" + FormatDate(udtEHSTransaction.ServiceDate, EnumDateFormat.DDMMYYYY)
                If blnDischarge Then
                    strBrandName_2nd = "|001"
                End If
                '===== Normal Case =====

                If (Not udtVaccinationRecordHistory Is Nothing) Then
                    If (udtVaccinationRecordHistory.AvailableItemDesc = "1st Dose") Then
                        strVaccineName_1st = "|" + udtCOVID19BLL.GetVaccineBrandPrintoutName(udtVaccinationRecordHistory.VaccineBrand)
                        strVaccineNameTC_1st = "|" + udtCOVID19BLL.GetVaccineBrandPrintoutNameChi(udtVaccinationRecordHistory.VaccineBrand)
                        strDoseDate_1st = "|" + FormatDate(udtVaccinationRecordHistory.ServiceReceiveDtm, EnumDateFormat.DDMMYYYY)
                        If blnDischarge Then
                            strBrandName_1st = "|001"
                        End If
                    End If
                End If
            End If

            'Generate Digital Signature
            Dim udtQRcode As QRCodeModel = Nothing

            strRawData = String.Concat(strPrefix1AndPrefix2, strQRCodeVersion, strKeyVersion, strTransId, strMaskedDocId, strMaskedName, strDoseDate_1st, strVaccineName_1st, strVaccineNameTC_1st, strBrandName_1st, strBrandNameTC_1st, strDoseDate_2nd, strVaccineName_2nd, strVaccineNameTC_2nd, strBrandName_2nd, strBrandNameTC_2nd, strPrintDate)

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

    End Class

End Namespace
