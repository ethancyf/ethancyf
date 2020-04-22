Imports Common.Component
Imports Common.Component.ClaimCategory
Imports Common.Component.EHSAccount
Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData

Partial Public Class ucReadOnlyRVP
    Inherits ucReadOnlyEHSClaimBase

    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    Dim _udtClaimCategoryBLL As ClaimCategoryBLL = New ClaimCategoryBLL()
#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        'Input Vaccine contorl Fields
        Me.udcClaimVaccineReadOnly.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcClaimVaccineReadOnly.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcClaimVaccineReadOnly.AmountText = Me.GetGlobalResourceObject("Text", "InjectionCost")
        Me.udcClaimVaccineReadOnly.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcClaimVaccineReadOnly.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalInjectionCost")
        Me.udcClaimVaccineReadOnly.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcClaimVaccineReadOnly.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        Me.udcClaimVaccineReadOnly.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")

        'Text Field
        Me.lblRCHCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
        Me.lblRCHNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")
        Me.lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")
        Me.lblRecipientConditionText.Text = Me.GetGlobalResourceObject("Text", "RecipientCondition")

        AddHandler udcClaimVaccineReadOnly.VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked
    End Sub

    Protected Overrides Sub Setup()
        trRecipientCondition.Visible = False

        'BLL
        Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
        Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

        Dim dtRVPhomeList As DataTable
        Dim strRCHCode As String
        Dim drClaimCategory As DataRow
        Dim strEnableClaimCategory As String = Nothing

        udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)

        ' Category
        If strEnableClaimCategory = "Y" Then
            Me.panClaimCategory.Visible = True
            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            drClaimCategory = Me._udtClaimCategoryBLL.getCategoryDesc(MyBase.EHSTransaction.CategoryCode)
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
            If Not drClaimCategory Is Nothing Then

                If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                    Me.lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name_Chi)
                ElseIf Me.SessionHandler.Language = Common.Component.CultureLanguage.SimpChinese Then
                    Me.lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name_CN)
                Else
                    Me.lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name)
                End If
            End If
        Else
            Me.panClaimCategory.Visible = False
        End If


        ' RCH Code
        strRCHCode = MyBase.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("RHCCode").AdditionalFieldValueCode
        dtRVPhomeList = udtRVPHomeListBLL.getRVPHomeListByCode(strRCHCode)
        Me.lblRCHCode.Text = strRCHCode

        If dtRVPhomeList.Rows.Count > 0 Then
            Dim dr As DataRow = dtRVPhomeList.Rows(0)

            If MyBase.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.lblRCHName.Text = dr("Homename_Chi").ToString().Trim()
                Me.lblRCHName.CssClass = "tableTextChi"
            Else
                Me.lblRCHName.Text = dr("Homename_Eng").ToString().Trim()
                Me.lblRCHName.CssClass = "tableText"
            End If
        End If

        ' Vaccine
        Me.udcClaimVaccineReadOnly.Build(MyBase.EHSClaimVaccine)

        ' Recipient Condition
        Dim strRecipientCondition As String = String.Empty

        If Not MyBase.EHSTransaction.HighRisk Is Nothing Then
            Select Case MyBase.EHSTransaction.HighRisk
                Case YesNo.Yes
                    strRecipientCondition = HighRiskOption.HIGHRISK
                Case YesNo.No
                    strRecipientCondition = HighRiskOption.NOHIGHRISK
            End Select
        End If

        'Display Recipient Condition
        If strRecipientCondition <> String.Empty Then

            trRecipientCondition.Visible = True

            Dim udtStaticDataModel As StaticDataModel = (New StaticDataBLL).GetStaticDataByColumnNameItemNo("VSS_RECIPIENTCONDITION", strRecipientCondition)

            Select Case MyBase.SessionHandler.Language
                Case CultureLanguage.TradChinese
                    lblRecipientCondition.Text = udtStaticDataModel.DataValueChi
                Case CultureLanguage.SimpChinese
                    lblRecipientCondition.Text = udtStaticDataModel.DataValueCN
                Case Else
                    lblRecipientCondition.Text = udtStaticDataModel.DataValue
            End Select

        End If

    End Sub

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            Me.tdCategory.Width = width
            Me.tdRCHCode.Width = width
            Me.tdRCHName.Width = width
            Me.lblCategoryText.Width = width
            Me.lblRCHCodeText.Width = width
            Me.lblRCHNameText.Width = width
        Else
            Me.tdCategory.Width = 200
            Me.tdRCHCode.Width = 200
            Me.tdRCHName.Width = 200
            Me.lblCategoryText.Width = 200
            Me.lblRCHCodeText.Width = 200
            Me.lblRCHNameText.Width = 200
        End If
    End Sub

#End Region

#Region "Events"

    Protected Sub udcClaimVaccineReadOnly_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

#End Region

End Class