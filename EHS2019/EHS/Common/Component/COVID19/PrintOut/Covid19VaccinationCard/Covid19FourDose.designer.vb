
Namespace Component.COVID19.PrintOut.Covid19VaccinationCard
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Public Class Covid19FourDose
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
        Dim resources As System.Resources.ResourceManager = New System.Resources.ResourceManager(GetType(Covid19FourDose))
        Me.Detail = New GrapeCity.ActiveReports.SectionReportModel.Detail()
            Me.VaccinationCenter = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.VaccinationCenterEngOnly = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label11 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.InjectionDate = New GrapeCity.ActiveReports.SectionReportModel.RichTextBox()
            Me.Label1 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseVaccineTitle = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseVaccineNameChiLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.VaccinationCenterChi = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label13 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseVaccinationCenterChiLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseVaccineNameLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label4 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label2 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label10 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label6 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseLotNumberLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseInjectionDateLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseInjectionDateChiLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseVaccinationCenterLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.NumDose = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.VaccineName = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.LotNumber = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.NumDoseChi = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.VaccineNameChi = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.FirstDoseLotNumberChiLabel = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Cover = New GrapeCity.ActiveReports.SectionReportModel.Label()
            Me.Label3 = New GrapeCity.ActiveReports.SectionReportModel.Label()
            CType(Me.VaccinationCenter, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.VaccinationCenterEngOnly, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label11, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label1, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseVaccineTitle, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseVaccineNameChiLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.VaccinationCenterChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label13, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseVaccinationCenterChiLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseVaccineNameLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label4, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label10, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label6, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseLotNumberLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseInjectionDateLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseInjectionDateChiLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseVaccinationCenterLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.NumDose, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.VaccineName, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LotNumber, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.NumDoseChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.VaccineNameChi, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.FirstDoseLotNumberChiLabel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Cover, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.Label3, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            '
            'Detail
            '
            Me.Detail.BackColor = System.Drawing.Color.White
            Me.Detail.Controls.AddRange(New GrapeCity.ActiveReports.SectionReportModel.ARControl() {Me.VaccinationCenter, Me.VaccinationCenterEngOnly, Me.Label11, Me.InjectionDate, Me.Label1, Me.FirstDoseVaccineTitle, Me.FirstDoseVaccineNameChiLabel, Me.VaccinationCenterChi, Me.Label13, Me.FirstDoseVaccinationCenterChiLabel, Me.FirstDoseVaccineNameLabel, Me.Label4, Me.Label2, Me.Label10, Me.Label6, Me.FirstDoseLotNumberLabel, Me.FirstDoseInjectionDateLabel, Me.FirstDoseInjectionDateChiLabel, Me.FirstDoseVaccinationCenterLabel, Me.NumDose, Me.VaccineName, Me.LotNumber, Me.NumDoseChi, Me.VaccineNameChi, Me.FirstDoseLotNumberChiLabel, Me.Label3, Me.Cover})
            Me.Detail.Height = 1.662666!
            Me.Detail.KeepTogether = True
            Me.Detail.Name = "Detail"
            '
            'VaccinationCenter
            '
            Me.VaccinationCenter.Height = 0.3620001!
            Me.VaccinationCenter.HyperLink = Nothing
            Me.VaccinationCenter.Left = 2.477!
            Me.VaccinationCenter.Name = "VaccinationCenter"
            Me.VaccinationCenter.ShrinkToFit = True
            Me.VaccinationCenter.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "top; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.VaccinationCenter.Text = ""
            Me.VaccinationCenter.Top = 1.316!
            Me.VaccinationCenter.Width = 5.313!
            '
            'VaccinationCenterEngOnly
            '
            Me.VaccinationCenterEngOnly.Height = 0.4920001!
            Me.VaccinationCenterEngOnly.HyperLink = Nothing
            Me.VaccinationCenterEngOnly.Left = 2.475!
            Me.VaccinationCenterEngOnly.Name = "VaccinationCenterEngOnly"
            Me.VaccinationCenterEngOnly.ShrinkToFit = True
            Me.VaccinationCenterEngOnly.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "top; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.VaccinationCenterEngOnly.Text = ""
            Me.VaccinationCenterEngOnly.Top = 1.186!
            Me.VaccinationCenterEngOnly.Visible = False
            Me.VaccinationCenterEngOnly.Width = 5.313!
            '
            'Label11
            '
            Me.Label11.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label11.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label11.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label11.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label11.Height = 0.268!
            Me.Label11.HyperLink = Nothing
            Me.Label11.Left = 2.427!
            Me.Label11.Name = "Label11"
            Me.Label11.Style = ""
            Me.Label11.Text = ""
            Me.Label11.Top = 0.836!
            Me.Label11.Width = 5.362!
            '
            'InjectionDate
            '
            Me.InjectionDate.AutoReplaceFields = True
            Me.InjectionDate.CanGrow = False
            Me.InjectionDate.Font = New System.Drawing.Font("Arial", 10.0!)
            Me.InjectionDate.Height = 0.2!
            Me.InjectionDate.Left = 2.475!
            Me.InjectionDate.Multiline = False
            Me.InjectionDate.Name = "InjectionDate"
            Me.InjectionDate.Top = 0.881!
            Me.InjectionDate.Width = 4.031!
            '
            'Label1
            '
            Me.Label1.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label1.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label1.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label1.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label1.Height = 0.425!
            Me.Label1.HyperLink = Nothing
            Me.Label1.Left = 0.6799999!
            Me.Label1.Name = "Label1"
            Me.Label1.Style = ""
            Me.Label1.Text = ""
            Me.Label1.Top = 0.411!
            Me.Label1.Width = 1.747!
            '
            'FirstDoseVaccineTitle
            '
            Me.FirstDoseVaccineTitle.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.FirstDoseVaccineTitle.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.FirstDoseVaccineTitle.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.FirstDoseVaccineTitle.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.FirstDoseVaccineTitle.Height = 0.4109999!
            Me.FirstDoseVaccineTitle.HyperLink = Nothing
            Me.FirstDoseVaccineTitle.Left = 0.6799999!
            Me.FirstDoseVaccineTitle.Name = "FirstDoseVaccineTitle"
            Me.FirstDoseVaccineTitle.Style = ""
            Me.FirstDoseVaccineTitle.Text = ""
            Me.FirstDoseVaccineTitle.Top = 0.0000001192093!
            Me.FirstDoseVaccineTitle.Width = 1.747!
            '
            'FirstDoseVaccineNameChiLabel
            '
            Me.FirstDoseVaccineNameChiLabel.Height = 0.2!
            Me.FirstDoseVaccineNameChiLabel.HyperLink = Nothing
            Me.FirstDoseVaccineNameChiLabel.Left = 0.73!
            Me.FirstDoseVaccineNameChiLabel.Name = "FirstDoseVaccineNameChiLabel"
            Me.FirstDoseVaccineNameChiLabel.Style = "font-family: PMingLiU; font-size: 10pt; text-align: left; vertical-align: middle;" & _
        " ddo-char-set: 1"
            Me.FirstDoseVaccineNameChiLabel.Text = "疫苗名稱"
            Me.FirstDoseVaccineNameChiLabel.Top = 0.03100012!
            Me.FirstDoseVaccineNameChiLabel.Width = 1.658!
            '
            'VaccinationCenterChi
            '
            Me.VaccinationCenterChi.Height = 0.2000001!
            Me.VaccinationCenterChi.HyperLink = Nothing
            Me.VaccinationCenterChi.Left = 2.476!
            Me.VaccinationCenterChi.Name = "VaccinationCenterChi"
            Me.VaccinationCenterChi.ShrinkToFit = True
            Me.VaccinationCenterChi.Style = "font-family: PMingLiU; font-size: 10pt; text-align: left; vertical-align: bottom;" & _
        " ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.VaccinationCenterChi.Text = ""
            Me.VaccinationCenterChi.Top = 1.146!
            Me.VaccinationCenterChi.Width = 5.313!
            '
            'Label13
            '
            Me.Label13.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label13.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label13.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label13.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label13.Height = 0.5740001!
            Me.Label13.HyperLink = Nothing
            Me.Label13.Left = 0.6770003!
            Me.Label13.Name = "Label13"
            Me.Label13.Style = ""
            Me.Label13.Text = ""
            Me.Label13.Top = 1.104!
            Me.Label13.Width = 1.75!
            '
            'FirstDoseVaccinationCenterChiLabel
            '
            Me.FirstDoseVaccinationCenterChiLabel.Height = 0.2!
            Me.FirstDoseVaccinationCenterChiLabel.HyperLink = Nothing
            Me.FirstDoseVaccinationCenterChiLabel.Left = 0.73!
            Me.FirstDoseVaccinationCenterChiLabel.Name = "FirstDoseVaccinationCenterChiLabel"
            Me.FirstDoseVaccinationCenterChiLabel.Style = "font-family: PMingLiU; font-size: 10pt; text-align: left; vertical-align: middle;" & _
        " ddo-char-set: 1"
            Me.FirstDoseVaccinationCenterChiLabel.Text = "接種地點"
            Me.FirstDoseVaccinationCenterChiLabel.Top = 1.146!
            Me.FirstDoseVaccinationCenterChiLabel.Width = 1.658!
            '
            'FirstDoseVaccineNameLabel
            '
            Me.FirstDoseVaccineNameLabel.Height = 0.2!
            Me.FirstDoseVaccineNameLabel.HyperLink = Nothing
            Me.FirstDoseVaccineNameLabel.Left = 0.73!
            Me.FirstDoseVaccineNameLabel.Name = "FirstDoseVaccineNameLabel"
            Me.FirstDoseVaccineNameLabel.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "middle; ddo-char-set: 1"
            Me.FirstDoseVaccineNameLabel.Text = "Vaccine Name"
            Me.FirstDoseVaccineNameLabel.Top = 0.191!
            Me.FirstDoseVaccineNameLabel.Width = 1.658!
            '
            'Label4
            '
            Me.Label4.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label4.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label4.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label4.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label4.Height = 0.425!
            Me.Label4.HyperLink = Nothing
            Me.Label4.Left = 2.427!
            Me.Label4.Name = "Label4"
            Me.Label4.Style = ""
            Me.Label4.Text = ""
            Me.Label4.Top = 0.411!
            Me.Label4.Width = 5.361001!
            '
            'Label2
            '
            Me.Label2.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label2.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label2.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label2.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label2.Height = 0.4109997!
            Me.Label2.HyperLink = Nothing
            Me.Label2.Left = 2.427!
            Me.Label2.Name = "Label2"
            Me.Label2.Style = ""
            Me.Label2.Text = ""
            Me.Label2.Top = 0.0000002980232!
            Me.Label2.Width = 5.361001!
            '
            'Label10
            '
            Me.Label10.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label10.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label10.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label10.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label10.Height = 0.268!
            Me.Label10.HyperLink = Nothing
            Me.Label10.Left = 0.6770003!
            Me.Label10.Name = "Label10"
            Me.Label10.Style = ""
            Me.Label10.Text = ""
            Me.Label10.Top = 0.8359999!
            Me.Label10.Width = 1.75!
            '
            'Label6
            '
            Me.Label6.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label6.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label6.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label6.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label6.Height = 1.678!
            Me.Label6.HyperLink = Nothing
            Me.Label6.Left = 0.0!
            Me.Label6.Name = "Label6"
            Me.Label6.Style = ""
            Me.Label6.Text = "" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
            Me.Label6.Top = 0.0!
            Me.Label6.Width = 0.677!
            '
            'FirstDoseLotNumberLabel
            '
            Me.FirstDoseLotNumberLabel.Height = 0.2!
            Me.FirstDoseLotNumberLabel.HyperLink = Nothing
            Me.FirstDoseLotNumberLabel.Left = 0.73!
            Me.FirstDoseLotNumberLabel.Name = "FirstDoseLotNumberLabel"
            Me.FirstDoseLotNumberLabel.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "middle; ddo-char-set: 1"
            Me.FirstDoseLotNumberLabel.Text = "Manufacturer / Lot No."
            Me.FirstDoseLotNumberLabel.Top = 0.6059999!
            Me.FirstDoseLotNumberLabel.Width = 1.658!
            '
            'FirstDoseInjectionDateLabel
            '
            Me.FirstDoseInjectionDateLabel.Height = 0.2000003!
            Me.FirstDoseInjectionDateLabel.HyperLink = Nothing
            Me.FirstDoseInjectionDateLabel.Left = 1.336!
            Me.FirstDoseInjectionDateLabel.Name = "FirstDoseInjectionDateLabel"
            Me.FirstDoseInjectionDateLabel.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "middle; ddo-char-set: 1"
            Me.FirstDoseInjectionDateLabel.Text = "Vaccination Date"
            Me.FirstDoseInjectionDateLabel.Top = 0.8710001!
            Me.FirstDoseInjectionDateLabel.Width = 1.051!
            '
            'FirstDoseInjectionDateChiLabel
            '
            Me.FirstDoseInjectionDateChiLabel.Height = 0.2!
            Me.FirstDoseInjectionDateChiLabel.HyperLink = Nothing
            Me.FirstDoseInjectionDateChiLabel.Left = 0.729!
            Me.FirstDoseInjectionDateChiLabel.Name = "FirstDoseInjectionDateChiLabel"
            Me.FirstDoseInjectionDateChiLabel.Style = "font-family: PMingLiU; font-size: 10pt; text-align: left; vertical-align: middle;" & _
        " ddo-char-set: 1"
            Me.FirstDoseInjectionDateChiLabel.Text = "接種日期"
            Me.FirstDoseInjectionDateChiLabel.Top = 0.881!
            Me.FirstDoseInjectionDateChiLabel.Width = 0.6070002!
            '
            'FirstDoseVaccinationCenterLabel
            '
            Me.FirstDoseVaccinationCenterLabel.Height = 0.1999998!
            Me.FirstDoseVaccinationCenterLabel.HyperLink = Nothing
            Me.FirstDoseVaccinationCenterLabel.Left = 0.73!
            Me.FirstDoseVaccinationCenterLabel.Name = "FirstDoseVaccinationCenterLabel"
            Me.FirstDoseVaccinationCenterLabel.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "middle; ddo-char-set: 1"
            Me.FirstDoseVaccinationCenterLabel.Text = "Vaccination Premises"
            Me.FirstDoseVaccinationCenterLabel.Top = 1.316!
            Me.FirstDoseVaccinationCenterLabel.Width = 1.658!
            '
            'NumDose
            '
            Me.NumDose.Height = 0.2495!
            Me.NumDose.HyperLink = Nothing
            Me.NumDose.Left = 0.0!
            Me.NumDose.Name = "NumDose"
            Me.NumDose.Style = "font-family: Times New Roman; font-size: 11.75pt; font-weight: bold; text-align: " & _
        "center; vertical-align: middle; ddo-char-set: 1"
            Me.NumDose.Text = "1st Dose"
            Me.NumDose.Top = 0.8360001!
            Me.NumDose.Width = 0.677!
            '
            'VaccineName
            '
            Me.VaccineName.Height = 0.2!
            Me.VaccineName.HyperLink = Nothing
            Me.VaccineName.Left = 2.476!
            Me.VaccineName.Name = "VaccineName"
            Me.VaccineName.ShrinkToFit = True
            Me.VaccineName.Style = "font-family: Times New Roman; font-size: 10pt; font-weight: bold; text-align: lef" & _
        "t; vertical-align: middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.VaccineName.Text = ""
            Me.VaccineName.Top = 0.201!
            Me.VaccineName.Width = 5.250001!
            '
            'LotNumber
            '
            Me.LotNumber.Height = 0.355!
            Me.LotNumber.HyperLink = Nothing
            Me.LotNumber.Left = 2.476!
            Me.LotNumber.Name = "LotNumber"
            Me.LotNumber.ShrinkToFit = True
            Me.LotNumber.Style = "font-family: Times New Roman; font-size: 10pt; text-align: left; vertical-align: " & _
        "middle; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.LotNumber.Text = ""
            Me.LotNumber.Top = 0.451!
            Me.LotNumber.Width = 5.313!
            '
            'NumDoseChi
            '
            Me.NumDoseChi.Height = 0.255!
            Me.NumDoseChi.HyperLink = Nothing
            Me.NumDoseChi.Left = 0.0!
            Me.NumDoseChi.Name = "NumDoseChi"
            Me.NumDoseChi.Style = "font-family: PMingLiU; font-size: 11.75pt; font-weight: bold; text-align: center;" & _
        " vertical-align: middle; ddo-char-set: 1"
            Me.NumDoseChi.Text = "第一針"
            Me.NumDoseChi.Top = 0.581!
            Me.NumDoseChi.Width = 0.677!
            '
            'VaccineNameChi
            '
            Me.VaccineNameChi.Height = 0.2!
            Me.VaccineNameChi.HyperLink = Nothing
            Me.VaccineNameChi.Left = 2.476!
            Me.VaccineNameChi.Name = "VaccineNameChi"
            Me.VaccineNameChi.ShrinkToFit = True
            Me.VaccineNameChi.Style = "font-family: PMingLiU; font-size: 10pt; font-weight: bold; text-align: left; vert" & _
        "ical-align: top; ddo-char-set: 1; ddo-shrink-to-fit: true"
            Me.VaccineNameChi.Text = ""
            Me.VaccineNameChi.Top = 0.041!
            Me.VaccineNameChi.Width = 5.313!
            '
            'FirstDoseLotNumberChiLabel
            '
            Me.FirstDoseLotNumberChiLabel.Height = 0.2!
            Me.FirstDoseLotNumberChiLabel.HyperLink = Nothing
            Me.FirstDoseLotNumberChiLabel.Left = 0.73!
            Me.FirstDoseLotNumberChiLabel.Name = "FirstDoseLotNumberChiLabel"
            Me.FirstDoseLotNumberChiLabel.Style = "font-family: PMingLiU; font-size: 10pt; text-align: left; vertical-align: middle;" & _
        " ddo-char-set: 1"
            Me.FirstDoseLotNumberChiLabel.Text = "生產商 / 批號"
            Me.FirstDoseLotNumberChiLabel.Top = 0.451!
            Me.FirstDoseLotNumberChiLabel.Width = 1.658!
            '
            'Cover
            '
            Me.Cover.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Cover.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Cover.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Cover.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Cover.Height = 1.678!
            Me.Cover.HyperLink = Nothing
            Me.Cover.Left = 2.427!
            Me.Cover.Name = "Cover"
            Me.Cover.Style = "background-color: White; font-family: PMingLiU; font-size: 10pt; text-align: left" & _
        "; vertical-align: middle; ddo-char-set: 1"
            Me.Cover.Text = Nothing
            Me.Cover.Top = 0.0!
            Me.Cover.Visible = False
            Me.Cover.Width = 5.361001!
            '
            'Label3
            '
            Me.Label3.Border.BottomStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label3.Border.LeftStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label3.Border.RightStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label3.Border.TopStyle = GrapeCity.ActiveReports.BorderLineStyle.Solid
            Me.Label3.Height = 0.574!
            Me.Label3.HyperLink = Nothing
            Me.Label3.Left = 2.427!
            Me.Label3.Name = "Label3"
            Me.Label3.Style = ""
            Me.Label3.Text = ""
            Me.Label3.Top = 1.104!
            Me.Label3.Width = 5.361001!
            '
            'Covid19FourDose
            '
            Me.MasterReport = False
            Me.PageSettings.PaperHeight = 11.0!
            Me.PageSettings.PaperWidth = 8.5!
            Me.PrintWidth = 7.946!
            Me.Sections.Add(Me.Detail)
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-style: normal; text-decoration: none; font-weight: normal; font-size: 10pt; " & _
                "color: Black; font-family: ""PMingLiU""; ddo-char-set: 136", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 16pt; font-weight: bold", "Heading1", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-family: Times New Roman; font-size: 14pt; font-weight: bold; font-style: ita" & _
                "lic", "Heading2", "Normal"))
            Me.StyleSheet.Add(New DDCssLib.StyleSheetRule("font-size: 13pt; font-weight: bold", "Heading3", "Normal"))
            CType(Me.VaccinationCenter, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.VaccinationCenterEngOnly, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label11, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label1, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseVaccineTitle, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseVaccineNameChiLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.VaccinationCenterChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label13, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseVaccinationCenterChiLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseVaccineNameLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label4, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label10, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label6, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseLotNumberLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseInjectionDateLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseInjectionDateChiLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseVaccinationCenterLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.NumDose, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.VaccineName, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LotNumber, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.NumDoseChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.VaccineNameChi, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.FirstDoseLotNumberChiLabel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Cover, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.Label3, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

        End Sub


        Private WithEvents Label6 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseVaccineNameChiLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseLotNumberLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseInjectionDateLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseInjectionDateChiLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseVaccinationCenterLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseVaccinationCenterChiLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents NumDose As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents VaccinationCenterChi As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseVaccineTitle As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents NumDoseChi As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label2 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label11 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label10 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label13 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents VaccineName As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents LotNumber As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents VaccineNameChi As GrapeCity.ActiveReports.SectionReportModel.Label


        Private WithEvents VaccinationCenter As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents VaccinationCenterEngOnly As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Cover As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseLotNumberChiLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents FirstDoseVaccineNameLabel As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label1 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents Label4 As GrapeCity.ActiveReports.SectionReportModel.Label
        Private WithEvents InjectionDate As GrapeCity.ActiveReports.SectionReportModel.RichTextBox
        Private WithEvents Label3 As GrapeCity.ActiveReports.SectionReportModel.Label
    End Class
End Namespace
