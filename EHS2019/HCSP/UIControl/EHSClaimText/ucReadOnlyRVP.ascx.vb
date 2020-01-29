Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme
Imports Common.Component.StaticData
Imports Common.Component.ClaimCategory

Namespace UIControl.EHCClaimText
    Partial Public Class ucReadOnlyRVP
        Inherits ucReadOnlyEHSClaimBase

        Public Event VaccineRemarkClicked(ByVal sender As Object, ByVal e As System.EventArgs)

        Dim _udtClaimCategoryBLL As ClaimCategoryBLL = New ClaimCategoryBLL()
#Region "Must Override Function"

        Protected Overrides Sub RenderLanguage()
            'Input Vaccine contorl Fields
            Me.udcClaimVaccineReadOnlyText.VaccineText = Me.GetGlobalResourceObject("Text", "Vaccine")
            Me.udcClaimVaccineReadOnlyText.DoseText = Me.GetGlobalResourceObject("Text", "Dose")
            Me.udcClaimVaccineReadOnlyText.AmountText = Me.GetGlobalResourceObject("Text", "InjectionCost")
            Me.udcClaimVaccineReadOnlyText.RemarksText = Me.GetGlobalResourceObject("Text", "Remarks")
            Me.udcClaimVaccineReadOnlyText.TotalAmount = Me.GetGlobalResourceObject("Text", "TotalInjectionCost")
            Me.udcClaimVaccineReadOnlyText.NAText = Me.GetGlobalResourceObject("Text", "N/A")

            'Text Field
            Me.lblRCHCodeText.Text = Me.GetGlobalResourceObject("Text", "RCHCode")
            Me.lblRCHNameText.Text = Me.GetGlobalResourceObject("Text", "RCHName")
            Me.lblCategoryText.Text = Me.GetGlobalResourceObject("Text", "Category")

            AddHandler udcClaimVaccineReadOnlyText.RemarkClicked, AddressOf udcClaimVaccineReadOnlyText_RemarkClicked

        End Sub

        Protected Overrides Sub Setup()
            Dim udtRVPHomeListBLL As New Common.Component.RVPHomeList.RVPHomeListBLL()
            Dim udtGeneralFunction As Common.ComFunction.GeneralFunction = New Common.ComFunction.GeneralFunction

            Dim dtRVPhomeList As DataTable
            Dim strRCHCode As String
            Dim drClaimCategory As DataRow
            Dim strEnableClaimCategory As String = Nothing

            udtGeneralFunction.getSytemParameterByParameterNameSchemeCode("RVPEnableClaimCategory", strEnableClaimCategory, String.Empty, SchemeClaimModel.RVP)

            '------------------------------------------------------------------------------------
            'Category
            '------------------------------------------------------------------------------------
            If strEnableClaimCategory = "Y" Then
                Me.panClaimCategory.Visible = True
                'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                drClaimCategory = Me._udtClaimCategoryBLL.getCategoryDesc(MyBase.EHSTransaction.CategoryCode)
                'CRE16-002 (Revamp VSS) [End][Chris YIM]
                If Not drClaimCategory Is Nothing Then
                    If Me.SessionHandler.Language = Common.Component.CultureLanguage.TradChinese Then
                        Me.lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name_Chi)
                    Else
                        Me.lblCategory.Text = drClaimCategory(ClaimCategoryModel._Category_Name)
                    End If
                End If
            Else
                Me.panClaimCategory.Visible = False
            End If

            '------------------------------------------------------------------------------------
            'RCH Code
            '------------------------------------------------------------------------------------
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


