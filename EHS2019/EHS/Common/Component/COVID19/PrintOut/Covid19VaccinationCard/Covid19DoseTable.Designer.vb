
Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Covid19DoseTable
        Inherits GrapeCity.ActiveReports.SectionReport

        'Form overrides dispose to clean up the component list.
        Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
            If disposing Then
            End If
            MyBase.Dispose(disposing)
        End Sub

        'NOTE: The following procedure is required by the ActiveReports Designer
        'It can be modified using the ActiveReports Designer.
        'Do not modify it using the code editor.
        Private WithEvents Detail As GrapeCity.ActiveReports.SectionReportModel.Detail
        <System.Diagnostics.DebuggerStepThrough()> _
        Private Sub InitializeComponent()
            Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Covid19DoseTable))
            Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.FirstDoseVaccineTitle = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label11 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label10 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label14 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label13 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseVaccineNameChiLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseLotNumberLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseInjectionDateLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseInjectionDateChiLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseVaccinationCenterLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseVaccinationCenterChiLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.NoDose = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.VaccineName = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.LotNumber = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.InjectionDate = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.VaccinationCenterChi = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.NoDoseChi = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.VaccineNameChi = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.VaccinationCenter = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.VaccinationCenterEngOnly = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseLotNumberChiLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseVaccineNameLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.InjectionDateChi = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Cover = New GrapeCity.ActiveReports.SectionReportModel.Label()
            CType(Me.FirstDoseVaccineTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label11, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label10, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label14, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label13, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseVaccineNameChiLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseLotNumberLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseInjectionDateLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseInjectionDateChiLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseVaccinationCenterLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseVaccinationCenterChiLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.NoDose, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.VaccineName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LotNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.InjectionDate, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.VaccinationCenterChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.NoDoseChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.VaccineNameChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.VaccinationCenter, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.VaccinationCenterEngOnly, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseLotNumberChiLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseVaccineNameLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.InjectionDateChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Cover, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.BackColor = System.Drawing.Color.White
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.Label14, Me.VaccinationCenter, Me.VaccinationCenterEngOnly, Me.FirstDoseVaccineTitle, Me.Label2, Me.Label11, Me.Label10, Me.Label13, Me.Label6, Me.FirstDoseVaccineNameChiLabel, Me.FirstDoseLotNumberLabel, Me.FirstDoseInjectionDateLabel, Me.FirstDoseInjectionDateChiLabel, Me.FirstDoseVaccinationCenterLabel, Me.FirstDoseVaccinationCenterChiLabel, Me.NoDose, Me.VaccineName, Me.LotNumber, Me.InjectionDate, Me.VaccinationCenterChi, Me.NoDoseChi, Me.VaccineNameChi, Me.FirstDoseLotNumberChiLabel, Me.FirstDoseVaccineNameLabel, Me.InjectionDateChi, Me.Cover})
            Me.Detail.Height = 2.260417!
            Me.Detail.KeepTogether = True
            Me.Detail.Name = "Detail"
            '
            'FirstDoseVaccineTitle
            '
            Me.FirstDoseVaccineTitle.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.FirstDoseVaccineTitle.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.FirstDoseVaccineTitle.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.FirstDoseVaccineTitle.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.FirstDoseVaccineTitle.Height = 0.825!
            Me.FirstDoseVaccineTitle.HyperLink = Nothing
            Me.FirstDoseVaccineTitle.Left = 0.0!
            Me.FirstDoseVaccineTitle.Name = "FirstDoseVaccineTitle"
            Me.FirstDoseVaccineTitle.Style = ""
            Me.FirstDoseVaccineTitle.Text = ""
            Me.FirstDoseVaccineTitle.Top = 0.3099999!
            Me.FirstDoseVaccineTitle.Width = 2.064!
            '
            'Label2
            '
            Me.Label2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label2.Height = 0.825!
            Me.Label2.HyperLink = Nothing
            Me.Label2.Left = 2.064!
            Me.Label2.Name = "Label2"
            Me.Label2.Style = ""
            Me.Label2.Text = ""
            Me.Label2.Top = 0.3100002!
            Me.Label2.Width = 5.811!
            '
            'Label11
            '
            Me.Label11.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label11.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label11.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label11.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label11.Height = 0.39325!
            Me.Label11.HyperLink = Nothing
            Me.Label11.Left = 2.065!
            Me.Label11.Name = "Label11"
            Me.Label11.Style = ""
            Me.Label11.Text = ""
            Me.Label11.Top = 1.135!
            Me.Label11.Width = 5.81!
            '
            'Label10
            '
            Me.Label10.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label10.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label10.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label10.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label10.Height = 0.39325!
            Me.Label10.HyperLink = Nothing
            Me.Label10.Left = 0.001000437!
            Me.Label10.Name = "Label10"
            Me.Label10.Style = ""
            Me.Label10.Text = ""
            Me.Label10.Top = 1.135!
            Me.Label10.Width = 2.064!
            '
            'Label14
            '
            Me.Label14.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label14.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label14.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label14.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label14.Height = 0.748!
            Me.Label14.HyperLink = Nothing
            Me.Label14.Left = 2.064!
            Me.Label14.Name = "Label14"
            Me.Label14.Style = ""
            Me.Label14.Text = ""
            Me.Label14.Top = 1.528!
            Me.Label14.Width = 5.811!
            '
            'Label13
            '
            Me.Label13.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label13.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label13.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label13.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label13.Height = 0.748!
            Me.Label13.HyperLink = Nothing
            Me.Label13.Left = 0.001000437!
            Me.Label13.Name = "Label13"
            Me.Label13.Style = ""
            Me.Label13.Text = ""
            Me.Label13.Top = 1.528!
            Me.Label13.Width = 2.064!
            '
            'Label6
            '
            Me.Label6.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label6.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label6.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label6.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label6.Height = 0.31!
            Me.Label6.HyperLink = Nothing
            Me.Label6.Left = 0.0!
            Me.Label6.Name = "Label6"
            Me.Label6.Style = ""
            Me.Label6.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            Me.Label6.Top = 0.0!
            Me.Label6.Width = 7.875!
            '
            'FirstDoseVaccineNameChiLabel
            '
            Me.FirstDoseVaccineNameChiLabel.Height = 0.2!
            Me.FirstDoseVaccineNameChiLabel.HyperLink = Nothing
            Me.FirstDoseVaccineNameChiLabel.Left = 0.05!
            Me.FirstDoseVaccineNameChiLabel.Name = "FirstDoseVaccineNameChiLabel"
            Me.FirstDoseVaccineNameChiLabel.Style = "font-family: PMingLiU; font-size: 10pt; text-align: left; vertical-align: middle;" & _
        " ddo-char-set: 1"
            Me.FirstDoseVaccineNameChiLabel.Text = "疫苗名稱"
            Me.FirstDoseVaccineNameChiLabel.Top = 0.351!
            Me.FirstDoseVaccineNameChiLabel.Width = 1.313!
            '
            'FirstDoseLotNumberLabel
            '
            Me.FirstDoseLotNumberLabel.Height = 0.2!
            Me.FirstDoseLotNumberLabel.HyperLink = Nothing
            Me.FirstDoseLotNumberLabel.Left = 0.05!
            Me.FirstDoseLotNumberLabel.Name = "FirstDoseLotNumberLabel"
            Me.FirstDoseLotNumberLabel.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "middle; ddo-char-set: 1"
            Me.FirstDoseLotNumberLabel.Text = "Manufacturer / Lot No."
            Me.FirstDoseLotNumberLabel.Top = 0.9459999!
            Me.FirstDoseLotNumberLabel.Width = 1.75!
            '
            'FirstDoseInjectionDateLabel
            '
            Me.FirstDoseInjectionDateLabel.Height = 0.2000003!
            Me.FirstDoseInjectionDateLabel.HyperLink = Nothing
            Me.FirstDoseInjectionDateLabel.Left = 0.05!
            Me.FirstDoseInjectionDateLabel.Name = "FirstDoseInjectionDateLabel"
            Me.FirstDoseInjectionDateLabel.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "middle; ddo-char-set: 1"
            Me.FirstDoseInjectionDateLabel.Text = "Vaccination Date"
            Me.FirstDoseInjectionDateLabel.Top = 1.334!
            Me.FirstDoseInjectionDateLabel.Width = 1.132!
            '
            'FirstDoseInjectionDateChiLabel
            '
            Me.FirstDoseInjectionDateChiLabel.Height = 0.2!
            Me.FirstDoseInjectionDateChiLabel.HyperLink = Nothing
            Me.FirstDoseInjectionDateChiLabel.Left = 0.05!
            Me.FirstDoseInjectionDateChiLabel.Name = "FirstDoseInjectionDateChiLabel"
            Me.FirstDoseInjectionDateChiLabel.Style = "font-family: PMingLiU; font-size: 10pt; text-align: left; vertical-align: middle;" & _
        " ddo-char-set: 1"
            Me.FirstDoseInjectionDateChiLabel.Text = "接種日期"
            Me.FirstDoseInjectionDateChiLabel.Top = 1.17!
            Me.FirstDoseInjectionDateChiLabel.Width = 0.6210001!
            '
            'FirstDoseVaccinationCenterLabel
            '
            Me.FirstDoseVaccinationCenterLabel.Height = 0.1999998!
            Me.FirstDoseVaccinationCenterLabel.HyperLink = Nothing
            Me.FirstDoseVaccinationCenterLabel.Left = 0.05000001!
            Me.FirstDoseVaccinationCenterLabel.Name = "FirstDoseVaccinationCenterLabel"
            Me.FirstDoseVaccinationCenterLabel.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "middle; ddo-char-set: 1"
            Me.FirstDoseVaccinationCenterLabel.Text = "Vaccination Premises"
            Me.FirstDoseVaccinationCenterLabel.Top = 1.907!
            Me.FirstDoseVaccinationCenterLabel.Width = 1.372!
            '
            'FirstDoseVaccinationCenterChiLabel
            '
            Me.FirstDoseVaccinationCenterChiLabel.Height = 0.2!
            Me.FirstDoseVaccinationCenterChiLabel.HyperLink = Nothing
            Me.FirstDoseVaccinationCenterChiLabel.Left = 0.05!
            Me.FirstDoseVaccinationCenterChiLabel.Name = "FirstDoseVaccinationCenterChiLabel"
            Me.FirstDoseVaccinationCenterChiLabel.Style = "font-family: PMingLiU; font-size: 10pt; text-align: left; vertical-align: middle;" & _
        " ddo-char-set: 1"
            Me.FirstDoseVaccinationCenterChiLabel.Text = "接種地點"
            Me.FirstDoseVaccinationCenterChiLabel.Top = 1.699!
            Me.FirstDoseVaccinationCenterChiLabel.Width = 0.6210001!
            '
            'NoDose
            '
            Me.NoDose.Height = 0.2495!
            Me.NoDose.HyperLink = Nothing
            Me.NoDose.Left = 0.692!
            Me.NoDose.Name = "NoDose"
            Me.NoDose.Style = "font-family: Times New Roman; font-size: 11.75pt; font-weight: bold; text-align: " & _
        "left; vertical-align: middle; ddo-char-set: 1"
            Me.NoDose.Text = "1st Dose"
            Me.NoDose.Top = 0.02999997!
            Me.NoDose.Width = 0.874!
            '
            'VaccineName
            '
            Me.VaccineName.Height = 0.22!
            Me.VaccineName.HyperLink = Nothing
            Me.VaccineName.Left = 2.12!
            Me.VaccineName.Name = "VaccineName"
            Me.VaccineName.ShrinkToFit = True
            Me.VaccineName.Style = "font-family: Times New Roman; font-size: 10pt; font-weight: bold; text-align: lef" & _
        "t; vertical-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.VaccineName.Text = ""
            Me.VaccineName.Top = 0.531!
            Me.VaccineName.Width = 5.693!
            '
            'LotNumber
            '
            Me.LotNumber.Height = 0.24!
            Me.LotNumber.HyperLink = Nothing
            Me.LotNumber.Left = 2.12!
            Me.LotNumber.Name = "LotNumber"
            Me.LotNumber.ShrinkToFit = True
            Me.LotNumber.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.LotNumber.Text = ""
            Me.LotNumber.Top = 0.8110001!
            Me.LotNumber.Width = 5.756!
            '
            'InjectionDate
            '
            Me.InjectionDate.Height = 0.22!
            Me.InjectionDate.HyperLink = Nothing
            Me.InjectionDate.Left = 2.12!
            Me.InjectionDate.Name = "InjectionDate"
            Me.InjectionDate.ShrinkToFit = True
            Me.InjectionDate.Style = "font-family: PMingLiU; font-size: 10pt; text-align: left; vertical-align: middle;" & _
        " ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.InjectionDate.Text = ""
            Me.InjectionDate.Top = 1.334!
            Me.InjectionDate.Width = 5.463!
            '
            'VaccinationCenterChi
            '
            Me.VaccinationCenterChi.Height = 0.35!
            Me.VaccinationCenterChi.HyperLink = Nothing
            Me.VaccinationCenterChi.Left = 2.12!
            Me.VaccinationCenterChi.Name = "VaccinationCenterChi"
            Me.VaccinationCenterChi.ShrinkToFit = True
            Me.VaccinationCenterChi.Style = "font-family: PMingLiU; font-size: 10pt; text-align: left; vertical-align: bottom;" & _
        " ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.VaccinationCenterChi.Text = ""
            Me.VaccinationCenterChi.Top = 1.537!
            Me.VaccinationCenterChi.Width = 5.756!
            '
            'NoDoseChi
            '
            Me.NoDoseChi.Height = 0.255!
            Me.NoDoseChi.HyperLink = Nothing
            Me.NoDoseChi.Left = 0.071!
            Me.NoDoseChi.Name = "NoDoseChi"
            Me.NoDoseChi.Style = "font-family: PMingLiU; font-size: 11.75pt; font-weight: bold; text-align: left; v" & _
        "ertical-align: middle; ddo-char-set: 1"
            Me.NoDoseChi.Text = "第一針"
            Me.NoDoseChi.Top = 0.02700007!
            Me.NoDoseChi.Width = 0.6210001!
            '
            'VaccineNameChi
            '
            Me.VaccineNameChi.Height = 0.22!
            Me.VaccineNameChi.HyperLink = Nothing
            Me.VaccineNameChi.Left = 2.12!
            Me.VaccineNameChi.Name = "VaccineNameChi"
            Me.VaccineNameChi.ShrinkToFit = True
            Me.VaccineNameChi.Style = "font-family: PMingLiU; font-size: 10pt; font-weight: bold; text-align: left; vert" & _
        "ical-align: top; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.VaccineNameChi.Text = ""
            Me.VaccineNameChi.Top = 0.351!
            Me.VaccineNameChi.Width = 5.756!
            '
            'VaccinationCenter
            '
            Me.VaccinationCenter.Height = 0.35!
            Me.VaccinationCenter.HyperLink = Nothing
            Me.VaccinationCenter.Left = 2.12!
            Me.VaccinationCenter.Name = "VaccinationCenter"
            Me.VaccinationCenter.ShrinkToFit = True
            Me.VaccinationCenter.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "top; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.VaccinationCenter.Text = ""
            Me.VaccinationCenter.Top = 1.916!
            Me.VaccinationCenter.Width = 5.756!
            '
            'VaccinationCenterEngOnly
            '
            Me.VaccinationCenterEngOnly.Height = 0.715!
            Me.VaccinationCenterEngOnly.HyperLink = Nothing
            Me.VaccinationCenterEngOnly.Left = 2.12!
            Me.VaccinationCenterEngOnly.Name = "VaccinationCenterEngOnly"
            Me.VaccinationCenterEngOnly.ShrinkToFit = True
            Me.VaccinationCenterEngOnly.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "top; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.VaccinationCenterEngOnly.Text = ""
            Me.VaccinationCenterEngOnly.Top = 1.561!
            Me.VaccinationCenterEngOnly.Visible = False
            Me.VaccinationCenterEngOnly.Width = 5.756!
            '
            'FirstDoseLotNumberChiLabel
            '
            Me.FirstDoseLotNumberChiLabel.Height = 0.2!
            Me.FirstDoseLotNumberChiLabel.HyperLink = Nothing
            Me.FirstDoseLotNumberChiLabel.Left = 0.05!
            Me.FirstDoseLotNumberChiLabel.Name = "FirstDoseLotNumberChiLabel"
            Me.FirstDoseLotNumberChiLabel.Style = "font-family: PMingLiU; font-size: 10pt; text-align: left; vertical-align: middle;" & _
        " ddo-char-set: 1"
            Me.FirstDoseLotNumberChiLabel.Text = "生產商 / 批號"
            Me.FirstDoseLotNumberChiLabel.Top = 0.7710001!
            Me.FirstDoseLotNumberChiLabel.Width = 1.313!
            '
            'FirstDoseVaccineNameLabel
            '
            Me.FirstDoseVaccineNameLabel.Height = 0.2!
            Me.FirstDoseVaccineNameLabel.HyperLink = Nothing
            Me.FirstDoseVaccineNameLabel.Left = 0.05!
            Me.FirstDoseVaccineNameLabel.Name = "FirstDoseVaccineNameLabel"
            Me.FirstDoseVaccineNameLabel.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "middle; ddo-char-set: 1"
            Me.FirstDoseVaccineNameLabel.Text = "Vaccine Name"
            Me.FirstDoseVaccineNameLabel.Top = 0.531!
            Me.FirstDoseVaccineNameLabel.Width = 1.75!
            '
            'InjectionDateChi
            '
            Me.InjectionDateChi.Height = 0.22!
            Me.InjectionDateChi.HyperLink = Nothing
            Me.InjectionDateChi.Left = 2.12!
            Me.InjectionDateChi.Name = "InjectionDateChi"
            Me.InjectionDateChi.ShrinkToFit = True
            Me.InjectionDateChi.Style = "font-family: PMingLiU; font-size: 10pt; font-weight: normal; text-align: left; ve" & _
        "rtical-align: top; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.InjectionDateChi.Text = ""
            Me.InjectionDateChi.Top = 1.17!
            Me.InjectionDateChi.Width = 5.463!
            '
            'Cover
            '
            Me.Cover.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Cover.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Cover.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Cover.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Cover.Height = 1.966!
            Me.Cover.HyperLink = Nothing
            Me.Cover.Left = 2.064!
            Me.Cover.Name = "Cover"
            Me.Cover.Style = "background-color: White; font-family: PMingLiU; font-size: 10pt; text-align: left" & _
        "; vertical-align: middle; ddo-char-set: 1"
            Me.Cover.Text = HttpContext.GetGlobalResourceObject("Text", "NoInformation", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)) & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & HttpContext.GetGlobalResourceObject("Text", "NoInformation", New System.Globalization.CultureInfo(CultureLanguage.English))
            Me.Cover.Top = 0.3099999!
            Me.Cover.Visible = False
            Me.Cover.Width = 5.811999!
            '
            'Covid19DoseTable
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.0!
            Me.PageSettings.PaperWidth = 8.5!
            Me.PrintWidth = 7.998!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
                "color: Black; font-family: ""PMingLiU""; ddo-char-set: 136", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.FirstDoseVaccineTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label11, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label10, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label14, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label13, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseVaccineNameChiLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseLotNumberLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseInjectionDateLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseInjectionDateChiLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseVaccinationCenterLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseVaccinationCenterChiLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.NoDose, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.VaccineName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LotNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.InjectionDate, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.VaccinationCenterChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.NoDoseChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.VaccineNameChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.VaccinationCenter, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.VaccinationCenterEngOnly, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseLotNumberChiLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseVaccineNameLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.InjectionDateChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Cover, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub


        Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseVaccineNameChiLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseLotNumberLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseInjectionDateLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseInjectionDateChiLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseVaccinationCenterLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseVaccinationCenterChiLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents NoDose As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents InjectionDate As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents VaccinationCenterChi As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseVaccineTitle As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents NoDoseChi As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label11 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label10 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label14 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label13 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents VaccineName As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents LotNumber As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents VaccineNameChi As GrapeCity.ActiveReports.SectionReportModel.Label


        Private WithEvents VaccinationCenter As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents VaccinationCenterEngOnly As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Cover As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseLotNumberChiLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseVaccineNameLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents InjectionDateChi As GrapeCity.ActiveReports.SectionReportModel.Label
    End Class
End Namespace
