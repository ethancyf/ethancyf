Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.ServiceProvider
Imports Common.Component.Address
Imports Common.Component.District
Imports Common.Component.Area
Imports System.Web
Imports Common.Component.Practice

Namespace Printout.EnrolmentInformation.Component

    Public Class Core

        Private udtDistrictBLL As DistrictBLL = New DistrictBLL

        Public Const intHeaderFontSize As Integer = 12
        Public Const intRemarksFontSize As Integer = 8

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal udtProvider As ServiceProviderModel, ByVal strLanguage As String, ByVal strPCD_ERN As String, ByVal strPCD_SubmissionTime As String)
            Me.New()
            If Not IsNothing(udtProvider) Then
                DataBind(udtProvider, strLanguage, strPCD_ERN, strPCD_SubmissionTime)
            End If
        End Sub

        Private Sub DataBind(ByVal udtProvider As ServiceProviderModel, ByVal strLanguage As String, ByVal strPCD_ERN As String, ByVal strPCD_SubmissionTime As String)

            Dim cultureInfo As System.Globalization.CultureInfo
            cultureInfo = New System.Globalization.CultureInfo(strLanguage)

            Dim udtPrintoutHelper As New PrintoutHelper(strLanguage)

            ' --- Enrolment Information ---
            udtPrintoutHelper.RenderLabel(lblTitle, "EnrolmentInfoOfPCD", intFontSize:=intHeaderFontSize, blnBold:=True)

            ' Note
            udtPrintoutHelper.RenderResource(txtEnrolmentInformationPrintoutNote, "EnrolmentInformationPrintoutNote")

            ' Reminder
            udtPrintoutHelper.RenderResource(txtEnrolmentInformationPrintoutReminder, "EnrolmentInformationPrintoutReminder")

            ' Enrolment Reference No.
            udtPrintoutHelper.RenderResource(txtERNText, "EnrolmentReferenceNo", intFontSize:=intHeaderFontSize, blnColon:=True, blnBold:=True)
            udtPrintoutHelper.RenderValue(txtERN, strPCD_ERN, intFontSize:=intHeaderFontSize)

            ' Submission Time
            udtPrintoutHelper.RenderResource(txtSubmissionTimeText, "SubmissionDtmTime", intFontSize:=intHeaderFontSize, blnColon:=True, blnBold:=True)
            udtPrintoutHelper.RenderValue(txtSubmissionTime, String.Format(HttpContext.GetGlobalResourceObject("Text", "SubmissionTimeFormatString", cultureInfo), CType(strPCD_SubmissionTime, DateTime)), intFontSize:=intHeaderFontSize)

            ' --- Personal Particulars ---
            udtPrintoutHelper.RenderLabel(lblPersonalParticularHead, "PersonalParticulars", intFontSize:=intHeaderFontSize, blnBold:=True)

            ' Name (in English)
            udtPrintoutHelper.RenderResource(txtNameENText, "NameInEnglish", blnColon:=True)

            udtPrintoutHelper.RenderValue(txtNameEN, udtProvider.EnglishName)

            ' Name (in Chinese)
            udtPrintoutHelper.RenderResource(txtNameCHText, "NameInChinese", blnColon:=True)

            udtPrintoutHelper.RenderValue(txtNameCH, udtProvider.ChineseName, blnMultiLingual:=True)

            ' Email
            udtPrintoutHelper.RenderResource(txtEmailText, "PCDEmail", blnAddNotDiscloseNotation:=True, blnColon:=True)

            udtPrintoutHelper.RenderValue(txtEmail, udtProvider.Email)

            ' Correspondence Address 
            udtPrintoutHelper.RenderResource(txtCorrAddressText, "SPAddress", blnAddNotDiscloseNotation:=True, blnColon:=True)

            udtPrintoutHelper.RenderValue(txtCorrAddress, formatAddress(udtProvider.SpAddress.Room, udtProvider.SpAddress.Floor, udtProvider.SpAddress.Block, udtProvider.SpAddress.Building, udtProvider.SpAddress.District))

            ' Not Disclose Notation Explanation
            udtPrintoutHelper.RenderResource(txtNotDiscloseNotationExplanation, "NotDiscloseNotationExplanation", intFontSize:=intRemarksFontSize)

            'Get the Profession from the first practice
            Dim strProfCode As String = CType(udtProvider.PracticeList.GetByIndex(0), PracticeModel).Professional.Profession.ServiceCategoryCode

            ' --- Professional Information ---
            udtPrintoutHelper.RenderLabel(lblProfessionalInformationTitle, "ProfInfo", intFontSize:=intHeaderFontSize, blnBold:=True)

            srptProfessionList.Report = New ProfessionList(udtProvider, strLanguage)

            ' --- Footer ---

            ' Print on
            udtPrintoutHelper.RenderText(riPrintOn, String.Format(HttpContext.GetGlobalResourceObject("Text", "PrintOnFormatString", cultureInfo), DateTime.Now))

            ' Page X of Y
            udtPrintoutHelper.RenderText(riPageNo, HttpContext.GetGlobalResourceObject("Text", "PageNoFormatString", cultureInfo))

        End Sub

#Region "Address Formatting"

        Protected Function formatAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String) As String
            Dim strAreacode As String
            strAreacode = getAreaString(strDistrict)
            Return (New Common.Format.Formatter).formatAddress(strRoom, strFloor, strBlock, strBuilding, strDistrict, strAreacode)
        End Function

        Protected Function formatChiAddress(ByVal strRoom As String, ByVal strFloor As String, ByVal strBlock As String, ByVal strBuilding As String, ByVal strDistrict As String) As String
            Dim strAreacode As String
            strAreacode = getAreaString(strDistrict)
            Return (New Common.Format.Formatter).formatAddressChi(strRoom, strFloor, strBlock, strBuilding, strDistrict, strAreacode)
        End Function

        Protected Function formatChineseString(ByVal strChineseString) As String
            Return (New Common.Format.Formatter).formatChineseName(strChineseString)
        End Function

        Public Function getAreaString(ByVal strDistrict As String) As String
            Dim strAreaCode As String

            If strDistrict.Equals(String.Empty) Then
                strAreaCode = String.Empty
            Else
                strAreaCode = GetAreaByDistrictCode(strDistrict)
            End If

            Return strAreaCode
        End Function

        Public Function GetAreaByDistrictCode(ByVal strDistrictCode As String) As String
            Dim udtDistrictModelCollection As DistrictModelCollection = GetDistrict()
            Dim udtDistrictModel As DistrictModel

            udtDistrictModel = udtDistrictModelCollection.Item(strDistrictCode)

            Return udtDistrictModel.Area_ID
        End Function

        ''' <summary>
        ''' Get the lists of district by area code
        ''' </summary>
        ''' <param name="strAreaCode"></param>
        ''' <returns>DistrictModelCollection</returns>
        ''' <remarks></remarks>
        Public Function GetDistrict(Optional ByVal strAreaCode As String = "") As DistrictModelCollection
            Dim udtDistrictModelCollection As DistrictModelCollection = New DistrictModelCollection
            If strAreaCode.Equals(String.Empty) Then
                udtDistrictModelCollection = udtDistrictBLL.GetDistrictList
            Else
                udtDistrictModelCollection = udtDistrictBLL.GetDistrictListByAreaCode(strAreaCode)
            End If

            Return udtDistrictModelCollection
        End Function

#End Region

    End Class

End Namespace

