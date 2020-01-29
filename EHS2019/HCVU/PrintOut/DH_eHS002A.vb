Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.ServiceProvider
Imports Common.Format
Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document 

Namespace PrintOut.ConfirmationLetter

    Public Class DH_eHS002A

        Public Sub New(ByVal udtSP As ServiceProviderModel)
            InitializeComponent()
            InputVariable(udtSP)
        End Sub

        Public Sub InputVariable(ByVal udtSP As ServiceProviderModel)
            Dim udtGeneralFunction As New GeneralFunction
            txtboxTelEng.Text = udtGeneralFunction.getUserDefinedParameter("Printout", "IVSSConfirmationLetterTelNo")
            txtboxTelChi.Text = txtboxTelEng.Text

            txtboxFaxEng.Text = udtGeneralFunction.getUserDefinedParameter("Printout", "IVSSConfirmationLetterFaxNo")
            txtboxFaxChi.Text = txtboxFaxEng.Text

            txtboxSPNameEng.Text = udtSP.EnglishName
            txtboxSPNameChi.Text = udtSP.ChineseName

            Dim udtFormat As New Formatter
            txtboxAddressEng.Text = udtFormat.formatAddressWithNewline(udtSP.SpAddress.Room, udtSP.SpAddress.Floor, _
                                                                              udtSP.SpAddress.Block, udtSP.SpAddress.Building, _
                                                                              udtSP.SpAddress.District, udtSP.SpAddress.AreaCode)
            txtboxAddressChi.Text = txtboxAddressEng.Text

            'CRE13-019-02 Extend HCVS to China [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            'txtboxDateEng.Text = udtFormat.formatDisplayDate(Today)
            'txtboxDateChi.Text = udtFormat.formatDisplayDate(Today, "zh-tw")
            txtboxDateEng.Text = udtFormat.formatDisplayDate(Today, CultureLanguage.English)
            txtboxDateChi.Text = udtFormat.formatDisplayDate(Today, CultureLanguage.TradChinese)
            'CRE13-019-02 Extend HCVS to China [End][Chris YIM]

            txtboxRecipientEng.Text = "Dear Dr. " + udtSP.EnglishName.Trim + ","
            txtboxRecipientChi.Text = udtSP.ChineseName.Trim + "醫生："

            txtboxSPIDEng.Text = udtSP.SPID
            txtboxSPIDChi.Text = txtboxSPIDEng.Text

            txtboxEmailEng.Text = udtGeneralFunction.getUserDefinedParameter("Printout", "IVSSConfirmationLetterEmail")
            txtboxEmailChi.Text = txtboxEmailEng.Text

        End Sub

    End Class

End Namespace
