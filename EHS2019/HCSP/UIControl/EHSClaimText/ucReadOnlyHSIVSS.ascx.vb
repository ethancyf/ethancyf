Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports Common.Component.ClaimCategory

Namespace UIControl.EHCClaimText
    Partial Public Class ucReadOnlyHSIVSS
        Inherits ucReadOnlyEHSClaimBase

        Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim _udtClaimCategoryBLL As ClaimCategoryBLL = New ClaimCategoryBLL()
#Region "Must Override Function"

        Protected Overrides Sub RenderLanguage()
            'Input Vaccine contorl Fields
            Me.udcClaimVaccineReadOnlyText.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
            Me.udcClaimVaccineReadOnlyText.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
            Me.udcClaimVaccineReadOnlyText.AmountText = Me.GetGlobalResourceObject("Text", "SubsidyAmount")
            Me.udcClaimVaccineReadOnlyText.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
            Me.udcClaimVaccineReadOnlyText.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalSubsidyAmount")
            Me.udcClaimVaccineReadOnlyText.NAText = Me.GetGlobalResourceObject("Text", "N/A")

            'Text Field
            Me.lblPreConditionsText.Text = Me.GetGlobalResourceObject("Text", "PreConditions")
            Me.lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")

            AddHandler udcClaimVaccineReadOnlyText.RemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked

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
                Else
                    Me.lblPreConditions.Text = udtStaticDataModel.DataValue.ToString().Trim()
                End If
            Else
                Me.panPreConditions.Visible = False
            End If


            Me.udcClaimVaccineReadOnlyText.Build(MyBase.EHSClaimVaccine)
        End Sub

        Public Overrides Sub SetupTableTitle(ByVal width As Integer)

        End Sub
#End Region

#Region "Events"

        Protected Sub udcClaimVaccineReadOnlyText_RemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)
            RaiseEvent VaccineRemarkClicked(sender, e)
        End Sub

#End Region

    End Class
End Namespace


