Imports Common.Component.EHSTransaction
Imports Common.Component.Scheme

Public Class ClaimVaccineReadOnly
    Inherits System.Web.UI.WebControls.WebControl

#Region "Fields"

    Private udtSchemeClaimBLL As New SchemeClaimBLL

#End Region

#Region "Constants"

    Private Const AvailableItemDescInjection As String = "Injection"

#End Region

    Public Sub Build(ByVal udtEHSTransaction As EHSTransactionModel)
        Dim udtTransactionDetailList As TransactionDetailModelCollection = udtEHSTransaction.TransactionDetails
        Dim dblTotalAmount As Double = 0.0

        ' Padding top
        Dim tblPaddingTop As New Table()
        Dim tbrPaddingTop As New TableRow
        tbrPaddingTop.Height = 5
        tbrPaddingTop.Cells.Add(New TableCell)
        tblPaddingTop.Rows.Add(tbrPaddingTop)

        Controls.Add(tblPaddingTop)

        Dim table As New Table()

        'table.Width = 500
        table.CellPadding = 2
        table.CellSpacing = 0

        ' Header row
        Dim tblHRow As New TableRow()
        tblHRow.Height = 20

        ' Vaccine
        Dim tblHCell1 As New TableCell()
        tblHCell1.Width = Unit.Pixel(197)
        tblHCell1.Text = HttpContext.GetGlobalResourceObject("Text", "Vaccine")
        tblHCell1.HorizontalAlign = HorizontalAlign.Center
        tblHCell1.BorderStyle = BorderStyle.Solid
        tblHCell1.BorderWidth = New System.Web.UI.WebControls.Unit(1)
        tblHCell1.BorderColor = Drawing.Color.Gray
        tblHRow.Cells.Add(tblHCell1)

        ' Dose
        Dim tblHCell2 As New TableCell()
        tblHCell2.Width = Unit.Pixel(275)
        tblHCell2.Text = HttpContext.GetGlobalResourceObject("Text", "Dose")
        tblHCell2.HorizontalAlign = HorizontalAlign.Center
        tblHCell2.BorderStyle = BorderStyle.Solid
        tblHCell2.BorderWidth = New System.Web.UI.WebControls.Unit(1)
        tblHCell2.BorderColor = Drawing.Color.Gray
        tblHRow.Cells.Add(tblHCell2)

        ' Amount
        Dim tblHCell3 As New TableCell()
        tblHCell3.Width = Unit.Pixel(143)
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        tblHCell3.Text = IIf(udtEHSTransaction.SchemeCode = SchemeClaimModel.RVP, HttpContext.GetGlobalResourceObject("Text", "InjectionCost"), HttpContext.GetGlobalResourceObject("Text", "SubsidyAmount"))
        'CRE16-026 (Add PCV13) [End][Chris YIM]
        tblHCell3.HorizontalAlign = HorizontalAlign.Center
        tblHCell3.BorderStyle = BorderStyle.Solid
        tblHCell3.BorderWidth = New System.Web.UI.WebControls.Unit(1)
        tblHCell3.BorderColor = Drawing.Color.Gray
        tblHRow.Cells.Add(tblHCell3)

        table.Rows.Add(tblHRow)

        ' Content rows
        'Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim_WithSubsidizeGroup()
        Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllSchemeClaim_WithSubsidizeGroup

        For Each udtTransactionDetail As TransactionDetailModel In udtTransactionDetailList

            Dim tblCRow As New TableRow()
            tblCRow.Height = 20

            ' Vaccine
            Dim tblCCell1 As New TableCell()
            tblCCell1.BorderStyle = BorderStyle.Solid
            tblCCell1.BorderWidth = New System.Web.UI.WebControls.Unit(1)
            tblCCell1.BorderColor = Drawing.Color.Gray
            tblCCell1.CssClass = "tableText"
            tblCCell1.Style.Add("padding-left", "2px")

            'tblCCell1.Text = udtSchemeCList.Filter(udtTransactionDetail.SchemeCode).SubsidizeGroupClaimList.Filter( _
            '                                            udtTransactionDetail.SchemeCode, udtTransactionDetail.SubsidizeCode).SubsidizeDisplayCode

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Use new Display Code for claim as display
            'tblCCell1.Text = udtSchemeCList.FilterKey(udtTransactionDetail.SchemeCode, udtTransactionDetail.SchemeSeq).SubsidizeGroupClaimList.Filter( _
            '                                            udtTransactionDetail.SchemeCode, udtTransactionDetail.SchemeSeq, udtTransactionDetail.SubsidizeCode).DisplayCodeForClaim
            tblCCell1.Text = udtSchemeCList.Filter(udtTransactionDetail.SchemeCode).SubsidizeGroupClaimList.Filter( _
                                                        udtTransactionDetail.SchemeCode, udtTransactionDetail.SchemeSeq, udtTransactionDetail.SubsidizeCode).DisplayCodeForClaim
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

            tblCRow.Cells.Add(tblCCell1)

            ' Dose
            Dim tblCCell2 As New TableCell()
            tblCCell2.BorderStyle = BorderStyle.Solid
            tblCCell2.BorderWidth = New System.Web.UI.WebControls.Unit(1)
            tblCCell2.BorderColor = Drawing.Color.Gray
            tblCCell2.CssClass = "tableText"
            tblCCell2.HorizontalAlign = HorizontalAlign.Center
            tblCRow.Cells.Add(tblCCell2)

            If udtTransactionDetail.AvailableItemDesc = String.Empty OrElse udtTransactionDetail.AvailableItemDesc = AvailableItemDescInjection Then
                tblCCell2.Text = HttpContext.GetGlobalResourceObject("Text", "N/A")
            Else
                tblCCell2.Text = udtTransactionDetail.AvailableItemDesc
            End If

            ' Amount
            Dim tblCCell3 As New TableCell()
            tblCCell3.BorderStyle = BorderStyle.Solid
            tblCCell3.BorderWidth = New System.Web.UI.WebControls.Unit(1)
            tblCCell3.BorderColor = Drawing.Color.Gray
            tblCCell3.CssClass = "tableText"
            tblCCell3.HorizontalAlign = HorizontalAlign.Center
            tblCRow.Cells.Add(tblCCell3)

            tblCCell3.Text = "$" + CDbl(udtTransactionDetail.TotalAmount).ToString()

            table.Rows.Add(tblCRow)

            dblTotalAmount += CDbl(udtTransactionDetail.TotalAmount)

        Next

        ' Footer row
        Dim tblFRow As New TableRow()
        tblFRow.Height = 20

        ' Empty cell
        Dim tblFCell1 As New TableCell()
        tblFCell1.Height = 20
        tblFCell1.Text = String.Empty
        tblFCell1.BorderStyle = BorderStyle.None
        tblFRow.Cells.Add(tblFCell1)

        ' Total Amount text
        Dim tblFCell2 As New TableCell()
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        tblFCell2.Text = IIf(udtEHSTransaction.SchemeCode = SchemeClaimModel.RVP, HttpContext.GetGlobalResourceObject("Text", "TotalInjectionCost"), HttpContext.GetGlobalResourceObject("Text", "TotalSubsidyAmount"))
        'CRE16-026 (Add PCV13) [End][Chris YIM]
        tblFCell2.HorizontalAlign = HorizontalAlign.Center
        tblFCell2.BorderStyle = BorderStyle.Solid
        tblFCell2.BorderWidth = New System.Web.UI.WebControls.Unit(1)
        tblFCell2.BorderColor = Drawing.Color.Gray
        tblFRow.Cells.Add(tblFCell2)

        ' Total Amount
        Dim tblFCell3 As New TableCell()
        tblFCell3.Text = "$" + dblTotalAmount.ToString
        tblFCell3.HorizontalAlign = HorizontalAlign.Center
        tblFCell3.CssClass = "tableText"
        tblFCell3.BorderStyle = BorderStyle.Solid
        tblFCell3.BorderWidth = New System.Web.UI.WebControls.Unit(1)
        tblFCell3.BorderColor = Drawing.Color.Gray
        tblFRow.Cells.Add(tblFCell3)

        table.Rows.Add(tblFRow)

        Controls.Add(table)

        ' Padding bottom row
        Dim tblPaddingBottom As New Table()
        Dim tbrPaddingBottom As New TableRow
        tbrPaddingBottom.Height = 5
        tbrPaddingBottom.Cells.Add(New TableCell)
        tblPaddingBottom.Rows.Add(tbrPaddingBottom)

        Controls.Add(tblPaddingBottom)

    End Sub

End Class
