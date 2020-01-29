Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports Common.Component.ClaimCategory

Partial Public Class ucReadOnlyHSIVSS
    Inherits ucReadOnlyEHSClaimBase

    Public Event VaccineLegendClicked(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    Dim _udtClaimCategoryBLL As ClaimCategoryBLL = New ClaimCategoryBLL()
#Region "Must Override Function"

    Protected Overrides Sub RenderLanguage()
        'Input Vaccine contorl Fields
        Me.udcClaimVaccineReadOnly.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
        Me.udcClaimVaccineReadOnly.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
        Me.udcClaimVaccineReadOnly.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
        Me.udcClaimVaccineReadOnly.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
        Me.udcClaimVaccineReadOnly.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
        Me.udcClaimVaccineReadOnly.NAText = Me.GetGlobalResourceObject("Text", "N/A")
        Me.udcClaimVaccineReadOnly.VaccineLegendALT = Me.GetGlobalResourceObject("Text", "Legend")
        Me.udcClaimVaccineReadOnly.VaccineLegendURL = Me.GetGlobalResourceObject("ImageUrl", "Infobtn")

        'Text Field
        Me.lblPreConditionsText.Text = Me.GetGlobalResourceObject("Text", "PreConditions")
        Me.lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")

        AddHandler udcClaimVaccineReadOnly.VaccineLegendClicked, AddressOf udcClaimVaccineReadOnly_VaccineLegendClicked
    End Sub

    Protected Overrides Sub Setup()
        Dim StaticDataBLL As New StaticDataBLL()
        Dim udtStaticDataModel As StaticDataModel
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim strCategory As String = MyBase.EHSTransaction.CategoryCode
        'CRE16-002 (Revamp VSS) [End][Chris YIM]
        Dim drClaimCategory As DataRow
        Dim udtTransactionAdditionField As TransactionAdditionalFieldModel = Nothing

        '------------------------------------------------------------------------------------
        'Category
        '------------------------------------------------------------------------------------
        drClaimCategory = Me._udtClaimCategoryBLL.getCategoryDesc(strCategory)
        If Not drClaimCategory Is Nothing Then

            If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name_Chi)
            ElseIf Me.SessionHandler.Language = Common.Component.CultureLanguage.SimpChinese Then
                Me.lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name_CN)
            Else
                Me.lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name)
            End If
        End If

        '------------------------------------------------------------------------------------
        'Pre-Conditons
        '------------------------------------------------------------------------------------
        udtTransactionAdditionField = MyBase.EHSTransaction.TransactionAdditionFields.FilterByAdditionFieldID("PreCondition")

        If Not udtTransactionAdditionField Is Nothing Then
            Me.panPreConditions.Visible = True
            udtStaticDataModel = StaticDataBLL.GetStaticDataByColumnNameItemNo("PreCondition", udtTransactionAdditionField.AdditionalFieldValueCode)

            If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                Me.lblPreConditions.Text = udtStaticDataModel.DataValueChi.ToString().Trim()
            ElseIf Me.SessionHandler.Language = Common.Component.CultureLanguage.SimpChinese Then
                Me.lblPreConditions.Text = udtStaticDataModel.DataValueCN.ToString().Trim()
            Else
                Me.lblPreConditions.Text = udtStaticDataModel.DataValue.ToString().Trim()
            End If
        Else
            Me.panPreConditions.Visible = False
        End If

        Me.udcClaimVaccineReadOnly.Build(MyBase.EHSClaimVaccine)
    End Sub

    Public Overrides Sub SetupTableTitle(ByVal width As Integer)
        If width > 0 Then
            Me.tdCategory.Width = width
            Me.tdPreConditions.Width = width
            Me.lblPreConditionsText.Width = width
            Me.lblCategoryText.Width = width
        Else
            Me.tdCategory.Width = 200
            Me.tdPreConditions.Width = 200
            Me.lblPreConditionsText.Width = 200
            Me.lblCategoryText.Width = 200
        End If
    End Sub

#End Region

#Region "Events"

    Protected Sub udcClaimVaccineReadOnly_VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

#End Region

End Class