Imports GrapeCity.ActiveReports.SectionReportModel
Imports GrapeCity.ActiveReports.Document
Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.Component.ThirdParty
Imports Common.PCD.EnumConstant
Imports Common.Component.Address
Imports Common.Component.District
Imports Common.Component.Area
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component
Imports System.Web
Imports System.Globalization
Imports Common.Component.SchemeInformation

Namespace Printout.EnrolmentInformation.Component

    Public Class Practice

        Private udtDistrictBLL As DistrictBLL = New DistrictBLL

        Public Sub New()
            InitializeComponent()
        End Sub

        Public Sub New(ByVal udtProvider As ServiceProviderModel, ByVal udtPractice As PracticeModel, ByVal strLanguage As String)
            Me.New()

            DataBind(udtProvider, udtPractice, strLanguage)
        End Sub

        Private Sub DataBind(ByVal udtProvider As ServiceProviderModel, ByVal udtPractice As PracticeModel, ByVal strLanguage As String)
            'Dim udtPracticeDetail As PracticeDetailModel = udtPractice.PracticeDetail

            Dim udtPrintoutHelper As New PrintoutHelper(strLanguage)
            'Dim udtFormatter As New Formatter
            Dim strValue As String = String.Empty
            Dim strValueChi As String = String.Empty

            ' Practice ID
            udtPrintoutHelper.RenderText(txtPracticeID, String.Format("({0})", udtPractice.DisplaySeq))

            ' Type of practice
            udtPrintoutHelper.RenderResource(txtTypeOfPracticeText, "TypeOfPractice", blnColon:=True)

            'Dim strPracticeType As String = String.Empty

            'If Not IsNothing(udtProvider.ThirdPartyAdditionalFieldEnrolmentList) Then
            '    strPracticeType = udtProvider.ThirdPartyAdditionalFieldEnrolmentList.GetByValueCode(ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD, _
            '                                                                         udtPractice.DisplaySeq, EnumAdditionalFieldID.TYPE_OF_PRACTICE.ToString()).AdditionalFieldValueCode
            'End If

            Dim strPracticeTypeDisplay As String = String.Empty

            'If Not strPracticeType = String.Empty Then
            '    If strLanguage = CultureLanguage.TradChinese Then
            '        strPracticeTypeDisplay = udtPractice.Professional.Profession.PracticeTypePCD.GetByPracticeType(strPracticeType).NameChi
            '    Else
            '        strPracticeTypeDisplay = udtPractice.Professional.Profession.PracticeTypePCD.GetByPracticeType(strPracticeType).NameEng
            '    End If
            'End If

            Dim udtThirdPartyList As ThirdPartyAdditionalFieldEnrolmentCollection = udtProvider.ThirdPartyAdditionalFieldEnrolmentList
            If Not udtThirdPartyList Is Nothing AndAlso udtThirdPartyList.GetListBySysCode(ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD).Count > 0 Then
                Dim udtThirdPartyModel As ThirdPartyAdditionalFieldEnrolmentModel = udtThirdPartyList.GetByValueCode(ThirdPartyAdditionalFieldEnrolmentModel.EnumSysCode.PCD, _
                                                                                                                     udtPractice.DisplaySeq, _
                                                                                                                     EnumConstant.EnumAdditionalFieldID.TYPE_OF_PRACTICE.ToString())
                If Not udtThirdPartyModel Is Nothing Then
                    If strLanguage = CultureLanguage.TradChinese Then
                        strPracticeTypeDisplay = udtPractice.Professional.Profession.PracticeTypePCD.GetByPracticeType(udtThirdPartyModel.AdditionalFieldValueCode).NameChi
                    Else
                        strPracticeTypeDisplay = udtPractice.Professional.Profession.PracticeTypePCD.GetByPracticeType(udtThirdPartyModel.AdditionalFieldValueCode).NameEng
                    End If
                End If
            End If

            udtPrintoutHelper.RenderValue(txtTypeOfPractice, strPracticeTypeDisplay)

            ' Name
            udtPrintoutHelper.RenderResource(txtNameText, "PCDPracticeName", blnColon:=True)

            strValue = udtPractice.PracticeName
            strValueChi = udtPractice.PracticeNameChi

            ' CRE16-021 Transfer VSS category to PCD [Start][Winnie]
            ' Check whether non-clinic
            'Dim blnNonClinic As Boolean = False

            'For Each udtPSI As PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
            '    If udtPSI.PracticeDisplaySeq = udtPractice.DisplaySeq AndAlso udtPSI.ClinicType = PracticeSchemeInfoModel.ClinicTypeEnum.NonClinic Then
            '        blnNonClinic = True
            '        Exit For
            '    End If
            'Next

            'If blnNonClinic Then
            '    strValue += String.Format(" ({0})", HttpContext.GetGlobalResourceObject("Text", "NonClinic", New CultureInfo(CultureLanguage.English)))

            '    If strValueChi <> String.Empty Then
            '        strValueChi += String.Format(" ({0})", HttpContext.GetGlobalResourceObject("Text", "NonClinic", New CultureInfo(CultureLanguage.TradChinese)))
            '    End If

            'End If
            ' CRE16-021 Transfer VSS category to PCD [End][Winnie]

            If strValueChi <> String.Empty Then
                If strValue <> String.Empty Then
                    strValue += String.Format(" ({0})", strValueChi)
                Else
                    strValue = strValueChi
                End If
            End If

            udtPrintoutHelper.RenderValue(txtName, strValue, blnMultiLingual:=True)

            ' Address
            udtPrintoutHelper.RenderResource(txtAddressText, "PCDPracticeAddress", blnColon:=True)
            strValue = String.Empty
            strValueChi = String.Empty

            strValue = formatAddress(udtPractice.PracticeAddress.Room, udtPractice.PracticeAddress.Floor, udtPractice.PracticeAddress.Block, udtPractice.PracticeAddress.Building, udtPractice.PracticeAddress.District)

            strValueChi = formatChiAddress(udtPractice.PracticeAddress.Room, udtPractice.PracticeAddress.Floor, udtPractice.PracticeAddress.Block, udtPractice.PracticeAddress.ChiBuilding, udtPractice.PracticeAddress.District)

            If strValueChi <> String.Empty Then
                If strValue <> String.Empty Then
                    strValue += String.Format("{0}({1})", Environment.NewLine, strValueChi)
                Else
                    strValue = strValueChi
                End If
            End If

            udtPrintoutHelper.RenderValue(txtAddress, strValue, blnMultiLingual:=True)

            ' Telephone
            udtPrintoutHelper.RenderResource(txtTelText, "Telephone", blnColon:=True)
            udtPrintoutHelper.RenderValue(txtTel, udtPractice.PhoneDaytime)

            ' Government Primary Care Enhancement Programme
            srptGovProgText.Report = New GovProgText(strLanguage)
            srptGovProg.Report = New GovProgList(udtProvider, udtPractice, strLanguage)

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

