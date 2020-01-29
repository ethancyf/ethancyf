Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Imports Common.Component.Scheme
Imports Common.Component
Imports HCSP.BLL

Public Class SchemeRadioButtonGroupText
    Inherits System.Web.UI.WebControls.RadioButtonList

    Public Sub PopulateScheme(ByVal blnRetainSelection As Boolean, ByVal udtSchemeClaimModelCollection As SchemeClaimModelCollection, ByVal language As String, ByVal strFunctCode As String, ByVal udtSP As ServiceProvider.ServiceProviderModel)
        Dim udtSchemeClaimModelDisplayList As SchemeClaimModelCollection
        Dim strSelectedValue As String = Me.SelectedValue
        Dim blnIsFound As Boolean = False
        Dim udtSessionHandler As New SessionHandler

        Me.Items.Clear()

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------

        ' CRE16-026 (Add PCV13) [Start][Winnie]
        udtSchemeClaimModelDisplayList = udtSchemeClaimModelCollection.FilterByTextOnlyAvailable(True)
        ' CRE16-026 (Add PCV13) [End][Winnie]

        'For Each schemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollection
        For Each schemeClaimModel As SchemeClaimModel In udtSchemeClaimModelDisplayList
            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtPracticeDisplay As BLL.PracticeDisplayModel = udtSessionHandler.PracticeDisplayGetFromSession(strFunctCode)

            ' Non Clinic Setting
            Dim strNonClinicSetting As String = String.Empty
            Dim udtPracticeSchemeInfoList As PracticeSchemeInfo.PracticeSchemeInfoModelCollection = udtSP.PracticeList(udtPracticeDisplay.PracticeID).PracticeSchemeInfoList

            If Not udtPracticeSchemeInfoList Is Nothing AndAlso udtPracticeSchemeInfoList.Count > 0 Then
                If udtPracticeSchemeInfoList.Filter(udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(schemeClaimModel.SchemeCode)).IsNonClinic Then

                    If language = CultureLanguage.TradChinese Then
                        strNonClinicSetting = String.Format("<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.TradChinese)))
                    ElseIf language = CultureLanguage.SimpChinese Then
                        strNonClinicSetting = String.Format("<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.SimpChinese)))
                    Else
                        strNonClinicSetting = String.Format("<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting", New System.Globalization.CultureInfo(CultureLanguage.English)))
                    End If
                End If
            End If

            ' CRE13-001 - EHAPP [End][Tommy L]
            Dim listItem = New ListItem

            If language = CultureLanguage.TradChinese Then
                listItem.Text = schemeClaimModel.SchemeDescChi + strNonClinicSetting
            ElseIf language = CultureLanguage.SimpChinese Then
                listItem.Text = schemeClaimModel.SchemeDescCN + strNonClinicSetting
            Else
                listItem.Text = schemeClaimModel.SchemeDesc + strNonClinicSetting
            End If
            listItem.Value = schemeClaimModel.SchemeCode

            If schemeClaimModel.SchemeCode = strSelectedValue Then
                blnIsFound = True
            End If

            Me.Items.Add(listItem)
        Next

        If blnRetainSelection AndAlso blnIsFound Then
            Me.SelectedValue = strSelectedValue
        End If

    End Sub

End Class
