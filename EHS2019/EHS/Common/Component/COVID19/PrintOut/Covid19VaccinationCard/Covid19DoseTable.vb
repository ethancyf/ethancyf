Imports GrapeCity.ActiveReports
Imports GrapeCity.ActiveReports.Document
Imports GrapeCity.ActiveReports.SectionReportModel
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.ServiceProvider
Imports Common.Component.EHSAccount
Imports Common.Format
Imports Common.ComFunction
'Imports HCSP.BLL
Imports Common.Component.ClaimCategory
Imports Common.Component.COVID19.PrintOut.Common.Format.Formatter
Imports Common.Component.RVPHomeList

Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    Public Class Covid19DoseTable

        ' Model in use
        Private _udtEHSTransaction As EHSTransactionModel
        Private _udtVaccinationRecordHistory As TransactionDetailVaccineModel
        'Setting for blank sample of vaccination card
        Private _blnIsSample As Boolean
        Private udtCOVID19BLL As New COVID19.COVID19BLL

#Region "Constructor"

        Public Sub New()
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
        End Sub

        Public Sub New(ByRef udtEHSTransaction As EHSTransactionModel, ByRef udtVaccinationRecordHistory As TransactionDetailVaccineModel, ByRef blnIsSample As Boolean)
            ' Invoke default constructor
            Me.New()

            _udtEHSTransaction = udtEHSTransaction
            _udtVaccinationRecordHistory = udtVaccinationRecordHistory
            _blnIsSample = blnIsSample
            LoadReport()
            ChkIsSample()

        End Sub

#End Region

        Private Sub LoadReport()

            Dim strCurrentDose As String = _udtEHSTransaction.TransactionDetails(0).AvailableItemDesc

            If (strCurrentDose = "1st Dose") Then

                '===== Normal Case =====
                Dim strVaccineLotNo As String = _udtEHSTransaction.TransactionAdditionFields.VaccineLotNo
                Dim dt As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingByVaccineLotNo(strVaccineLotNo)

                FirstDoseLotNumber.Text = "Lot No. : " + dt.Rows(0)("Vaccine_Lot_No")
                FirstDoseVaccineNameChi.Text = dt.Rows(0)("Brand_Printout_Name_Chi")
                FirstDoseVaccineName.Text = dt.Rows(0)("Brand_Printout_Name")

                FirstDoseInjectionDate.Text = FormatDate(_udtEHSTransaction.ServiceDate, EnumDateFormat.DDMMYYYY)
                setVaccinationCenterTextLabel(True, FirstDoseVaccinationCenter, FirstDoseVaccinationCenterChi)
 

                If (FirstDoseVaccinationCenterChi.Text = String.Empty) Then
                    'hide FirstDoseVaccinationCenter and FirstDoseVaccinationCenterChi label and show FirstDoseVaccinationCenterEngOnly label
                    ShowCenterEngOnlyLabel(FirstDoseVaccinationCenter, FirstDoseVaccinationCenterChi, FirstDoseVaccinationCenterEngOnly)
                Else
                    HideCenterEngOnlyLabel(FirstDoseVaccinationCenter, FirstDoseVaccinationCenterChi, FirstDoseVaccinationCenterEngOnly)
                End If

                FirstDoseCover.Visible = False
                SecondDoseCover.Visible = True
                '===== Normal Case =====


                '===== date dateback second dose record ====
                If (Not _udtVaccinationRecordHistory Is Nothing) Then

                    'date dateback fill second dose record
                    If (_udtVaccinationRecordHistory.AvailableItemDesc = "2nd Dose") Then
                        SecondDoseCover.Visible = False
                        SecondDoseLotNumber.Text = "Lot No. : " + _udtVaccinationRecordHistory.VaccineLotNo

                        'get second does by history object brand id
                        SecondDoseVaccineNameChi.Text = udtCOVID19BLL.GetVaccineBrandPrintoutNameChi(_udtVaccinationRecordHistory.VaccineBrand)
                        SecondDoseVaccineName.Text = udtCOVID19BLL.GetVaccineBrandPrintoutName(_udtVaccinationRecordHistory.VaccineBrand)
                        SecondDoseInjectionDate.Text = FormatDate(_udtVaccinationRecordHistory.ServiceReceiveDtm, EnumDateFormat.DDMMYYYY)
                        setVaccinationCenterTextLabel(False, SecondDoseVaccinationCenter, SecondDoseVaccinationCenterChi)

                        If (SecondDoseVaccinationCenterChi.Text = String.Empty) Then
                            'hide SecondDoseVaccinationCenter and SecondDoseVaccinationCenterChi label and show SecondDoseVaccinationCenterEngOnly label
                            ShowCenterEngOnlyLabel(SecondDoseVaccinationCenter, SecondDoseVaccinationCenterChi, SecondDoseVaccinationCenterEngOnly)
                        Else
                            HideCenterEngOnlyLabel(SecondDoseVaccinationCenter, SecondDoseVaccinationCenterChi, SecondDoseVaccinationCenterEngOnly)
                        End If

                    End If

                End If
                '===== date dateback second dose record ====

            Else

                '===== Normal Case =====
                Dim strVaccineLotNo As String = _udtEHSTransaction.TransactionAdditionFields.VaccineLotNo
                Dim dt As DataTable = udtCOVID19BLL.GetCOVID19VaccineLotMappingByVaccineLotNo(strVaccineLotNo)

                SecondDoseLotNumber.Text = "Lot No. : " + dt.Rows(0)("Vaccine_Lot_No")
                SecondDoseVaccineNameChi.Text = dt.Rows(0)("Brand_Printout_Name_Chi")
                SecondDoseVaccineName.Text = dt.Rows(0)("Brand_Printout_Name")

                SecondDoseInjectionDate.Text = FormatDate(_udtEHSTransaction.ServiceDate, EnumDateFormat.DDMMYYYY)
                setVaccinationCenterTextLabel(True, SecondDoseVaccinationCenter, SecondDoseVaccinationCenterChi)


                If (SecondDoseVaccinationCenterChi.Text = String.Empty) Then
                    'hide SecondDoseVaccinationCenter and SecondDoseVaccinationCenterChi label and show SecondDoseVaccinationCenterEngOnly label
                    ShowCenterEngOnlyLabel(SecondDoseVaccinationCenter, SecondDoseVaccinationCenterChi, SecondDoseVaccinationCenterEngOnly)
                Else
                    HideCenterEngOnlyLabel(SecondDoseVaccinationCenter, SecondDoseVaccinationCenterChi, SecondDoseVaccinationCenterEngOnly)
                End If

                SecondDoseCover.Visible = False
                FirstDoseCover.Visible = True
                '===== Normal Case =====


                '===== First dose record ====
                If (Not _udtVaccinationRecordHistory Is Nothing) Then
                    'fill first dose record if _udtVaccinationRecordHistory not empty and  _udtVaccinationRecordHistory = first dose
                    If (_udtVaccinationRecordHistory.AvailableItemDesc = "1st Dose") Then
                        FirstDoseCover.Visible = False
                        FirstDoseLotNumber.Text = "Lot No. : " + _udtVaccinationRecordHistory.VaccineLotNo

                        'get first does by history object brand id
                        FirstDoseVaccineNameChi.Text = udtCOVID19BLL.GetVaccineBrandPrintoutNameChi(_udtVaccinationRecordHistory.VaccineBrand)
                        FirstDoseVaccineName.Text = udtCOVID19BLL.GetVaccineBrandPrintoutName(_udtVaccinationRecordHistory.VaccineBrand)
                        FirstDoseInjectionDate.Text = FormatDate(_udtVaccinationRecordHistory.ServiceReceiveDtm, EnumDateFormat.DDMMYYYY)
                        setVaccinationCenterTextLabel(False, FirstDoseVaccinationCenter, FirstDoseVaccinationCenterChi)

                        If (FirstDoseVaccinationCenterChi.Text = String.Empty) Then
                            'hide FirstDoseVaccinationCenter and FirstDoseVaccinationCenterChi label and show FirstDoseVaccinationCenterEngOnly label
                            ShowCenterEngOnlyLabel(FirstDoseVaccinationCenter, FirstDoseVaccinationCenterChi, FirstDoseVaccinationCenterEngOnly)
                        Else
                            HideCenterEngOnlyLabel(FirstDoseVaccinationCenter, FirstDoseVaccinationCenterChi, FirstDoseVaccinationCenterEngOnly)
                        End If
                    End If
                End If
                '===== First dose record ====

            End If




        End Sub

        Private Sub ShowCenterEngOnlyLabel(ByRef VaccinationCenter As Label, ByRef VaccinationCenterChi As Label, ByRef VaccinationCenterEngOnly As Label)

            VaccinationCenterEngOnly.Text = VaccinationCenter.Text
            VaccinationCenter.Visible = False
            VaccinationCenterChi.Visible = False
            VaccinationCenterEngOnly.Visible = True

        End Sub

        Private Sub HideCenterEngOnlyLabel(ByRef VaccinationCenter As Label, ByRef VaccinationCenterChi As Label, ByRef VaccinationCenterEngOnly As Label)

            VaccinationCenter.Visible = True
            VaccinationCenterChi.Visible = True
            VaccinationCenterEngOnly.Visible = False

        End Sub

        Private Sub setVaccinationCenterTextLabel(ByVal IsEHSTransactionObj As Boolean, ByRef VaccinationCenter As Label, ByRef VaccinationCenterChi As Label)

            If (IsEHSTransactionObj) Then
                If (_udtEHSTransaction.SchemeCode = SchemeClaimModel.COVID19CVC) Then
                    'booth and centre 
                    Dim dtVaccineCenter As DataTable = udtCOVID19BLL.GetCOVID19VaccineCentreBySPIDPracticeDisplaySeq(_udtEHSTransaction.ServiceProviderID, _udtEHSTransaction.PracticeID)
                    VaccinationCenter.Text = dtVaccineCenter.Rows(0)("Centre_Name")
                    VaccinationCenterChi.Text = dtVaccineCenter.Rows(0)("Centre_Name_Chi")

                ElseIf (_udtEHSTransaction.SchemeCode = SchemeClaimModel.COVID19RVP) Then
                    Dim strRCHCode As String = _udtEHSTransaction.TransactionAdditionFields.RCHCode
                    Dim udtRVP As RVPHomeListModel = (New Component.RVPHomeList.RVPHomeListBLL()).GetRVPHomeListModalByCode(strRCHCode)
                    VaccinationCenter.Text = udtRVP.HomenameEng
                    VaccinationCenterChi.Text = udtRVP.HomenameChi

                Else
                    VaccinationCenter.Text = _udtEHSTransaction.PracticeName
                    VaccinationCenterChi.Text = _udtEHSTransaction.PracticeNameChi
                End If
            Else
                If (_udtVaccinationRecordHistory.SchemeCode = SchemeClaimModel.COVID19CVC) Then
                    'booth and centre 
                    If _udtVaccinationRecordHistory.TransactionID IsNot Nothing AndAlso _udtVaccinationRecordHistory.TransactionID <> String.Empty Then
                        'EHS Transaction
                        Dim udtEHSTransactionBLL As New EHSTransactionBLL
                        Dim udtHistoryEHSTransaction As EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(_udtVaccinationRecordHistory.TransactionID)
                        Dim dtVaccineCenterHistory As DataTable = udtCOVID19BLL.GetCOVID19VaccineCentreBySPIDPracticeDisplaySeq(udtHistoryEHSTransaction.ServiceProviderID, udtHistoryEHSTransaction.PracticeID)
                        VaccinationCenter.Text = dtVaccineCenterHistory.Rows(0)("Centre_Name")
                        VaccinationCenterChi.Text = dtVaccineCenterHistory.Rows(0)("Centre_Name_Chi")
                    Else
                        'CMS/CMIS Vaccination Record
                        VaccinationCenter.Text = _udtVaccinationRecordHistory.PracticeName
                        VaccinationCenterChi.Text = _udtVaccinationRecordHistory.PracticeNameChi
                    End If

                ElseIf (_udtVaccinationRecordHistory.SchemeCode = SchemeClaimModel.COVID19RVP) Then
                    If _udtVaccinationRecordHistory.TransactionID IsNot Nothing AndAlso _udtVaccinationRecordHistory.TransactionID <> String.Empty Then
                        'EHS Transaction
                        Dim udtEHSTransactionBLL As New EHSTransactionBLL
                        Dim udtHistoryEHSTransaction As EHSTransactionModel = udtEHSTransactionBLL.LoadClaimTran(_udtVaccinationRecordHistory.TransactionID)

                        If udtHistoryEHSTransaction.TransactionAdditionFields.RCHCode IsNot Nothing AndAlso udtHistoryEHSTransaction.TransactionAdditionFields.RCHCode <> String.Empty Then
                            Dim udtRVP As RVPHomeListModel = (New Component.RVPHomeList.RVPHomeListBLL()).GetRVPHomeListModalByCode(udtHistoryEHSTransaction.TransactionAdditionFields.RCHCode)
                            VaccinationCenter.Text = udtRVP.HomenameEng
                            VaccinationCenterChi.Text = udtRVP.HomenameChi
                        Else
                            VaccinationCenter.Text = _udtEHSTransaction.PracticeName
                            VaccinationCenterChi.Text = _udtEHSTransaction.PracticeNameChi
                        End If
                    Else
                        'CMS/CMIS Vaccination Record
                        VaccinationCenter.Text = _udtVaccinationRecordHistory.PracticeName
                        VaccinationCenterChi.Text = _udtVaccinationRecordHistory.PracticeNameChi
                    End If
                Else
                    VaccinationCenter.Text = _udtVaccinationRecordHistory.PracticeName
                    VaccinationCenterChi.Text = _udtVaccinationRecordHistory.PracticeNameChi
                End If
            End If
        End Sub


        Private Sub ChkIsSample()
            If (_blnIsSample) Then
                FirstDoseCover.Visible = False
                FirstDoseVaccineNameChi.Visible = False
                FirstDoseVaccineName.Visible = False
                FirstDoseLotNumber.Visible = False
                FirstDoseInjectionDate.Visible = False
                FirstDoseVaccinationCenter.Visible = False
                FirstDoseVaccinationCenterChi.Visible = False
                FirstDoseVaccinationCenterEngOnly.Visible = False


                SecondDoseCover.Visible = False
                SecondDoseVaccineNameChi.Visible = False
                SecondDoseVaccineName.Visible = False
                SecondDoseLotNumber.Visible = False
                SecondDoseInjectionDate.Visible = False
                SecondDoseVaccinationCenter.Visible = False
                SecondDoseVaccinationCenterChi.Visible = False
                SecondDoseVaccinationCenterEngOnly.Visible = False
            End If
        End Sub

        Private Sub Covid19DoseTable_ReportStart(sender As Object, e As EventArgs) Handles MyBase.ReportStart

        End Sub
    End Class
End Namespace
