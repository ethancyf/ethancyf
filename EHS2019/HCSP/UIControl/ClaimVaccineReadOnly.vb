Public Class ClaimVaccineReadOnly
    Inherits System.Web.UI.WebControls.WebControl

    'Table title Text 
    Private _strVaccineText As String
    Private _strDoseText As String
    Private _strAmountText As String
    Private _strRemarksText As String
    Private _strTotalAmount As String
    Private _strNAText As String
    Private _strVaccineLegendURL As String
    Private _strVaccineLegendALT As String

    'Table Text Style 
    Private _strCssTableTitle As String
    Private _strCssTableText As String

    Private _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
    Public Event VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    Public Class ControlIDPrefix
        Public Const lblClaimDetailsTotal As String = "lblClaimDetailsTotal"
        Public Const btnVaccineLegend As String = "btnVaccineLegend"
    End Class

    Public Class NonDoseItem
        Public Const Injection As String = "Injection"
    End Class

    Public Sub Build(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel)
        Dim strLanguage As String = Me._udtSessionHandler.Language()
        Dim dblTotalAmount As Double = 0.0
        Dim table As Table = New Table()
        Dim tblRow As TableRow = New TableRow()
        Dim tblCell1 As TableCell
        Dim tblCell2 As TableCell
        Dim tblCell3 As TableCell
        Dim tblCell4 As TableCell

        table.Width = 800
        table.CellPadding = 2
        table.CellSpacing = 0

        Dim imageButton As ImageButton
        Dim lable As Label

        tblRow = New TableRow
        lable = New Label
        lable.Text = String.Format("{0} ", Me._strVaccineText)
        lable.CssClass = Me._strCssTableTitle

        'Vaccine Header Text
        imageButton = New ImageButton
        imageButton.ID = ControlIDPrefix.btnVaccineLegend
        imageButton.ImageUrl = Me._strVaccineLegendURL
        imageButton.AlternateText = Me._strVaccineLegendALT
        imageButton.ImageAlign = ImageAlign.Top
        imageButton.Style.Add("vertical-align", "top")
        AddHandler imageButton.Click, AddressOf btnVaccineLegend_Click

        tblCell1 = New TableCell()
        tblCell1.Height = 20
        tblCell1.HorizontalAlign = HorizontalAlign.Center
        tblCell1.Controls.Add(lable)
        tblCell1.Controls.Add(imageButton)

        tblCell2 = New TableCell()
        tblCell2.Height = 20
        tblCell2.Text = Me._strDoseText
        tblCell2.CssClass = Me._strCssTableTitle
        tblCell2.HorizontalAlign = HorizontalAlign.Center

        tblCell3 = New TableCell()
        tblCell3.Height = 20
        tblCell3.Text = Me._strAmountText
        tblCell3.CssClass = Me._strCssTableTitle
        tblCell3.HorizontalAlign = HorizontalAlign.Center

        tblCell4 = New TableCell()
        tblCell4.Height = 20
        tblCell4.Text = Me._strRemarksText
        tblCell4.CssClass = Me._strCssTableTitle
        tblCell4.HorizontalAlign = HorizontalAlign.Center

        tblCell1.BorderStyle = BorderStyle.Solid
        tblCell2.BorderStyle = BorderStyle.Solid
        tblCell3.BorderStyle = BorderStyle.Solid
        tblCell4.BorderStyle = BorderStyle.Solid
        tblCell1.BorderWidth = New System.Web.UI.WebControls.Unit(1)
        tblCell2.BorderWidth = New System.Web.UI.WebControls.Unit(1)
        tblCell3.BorderWidth = New System.Web.UI.WebControls.Unit(1)
        tblCell4.BorderWidth = New System.Web.UI.WebControls.Unit(1)
        tblCell1.BorderColor = Drawing.Color.Gray
        tblCell2.BorderColor = Drawing.Color.Gray
        tblCell3.BorderColor = Drawing.Color.Gray
        tblCell4.BorderColor = Drawing.Color.Gray

        tblRow.Cells.Add(tblCell1)
        tblRow.Cells.Add(tblCell2)
        tblRow.Cells.Add(tblCell3)
        tblRow.Cells.Add(tblCell4)
        table.Rows.Add(tblRow)

        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList

            If udtEHSClaimSubsidize.Selected Then
                tblRow = New TableRow()
                tblRow.VerticalAlign = VerticalAlign.Top

                tblCell1 = New TableCell()
                tblCell1.Height = 20
                tblCell1.Width = 180
                tblCell1.CssClass = Me._strCssTableText

                tblCell2 = New TableCell()
                tblCell2.Height = 20
                tblCell2.Width = 190
                tblCell2.CssClass = Me._strCssTableText

                tblCell3 = New TableCell()
                tblCell3.Height = 20
                tblCell3.Width = 140
                tblCell3.CssClass = Me._strCssTableText

                tblCell4 = New TableCell()
                tblCell4.Height = 20
                tblCell4.Width = 320
                tblCell4.CssClass = Me._strCssTableText

                tblCell1.Text = udtEHSClaimSubsidize.SubsidizeDisplayCode
                'If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                '    tblCell1.Text = udtEHSClaimSubsidize.SubsidizeItemDescChi
                'Else
                '    tblCell1.Text = udtEHSClaimSubsidize.SubsidizeItemDesc
                'End If

                tblCell1.Font.Bold = True
                tblCell1.Style.Add("padding-left", "2px")

                If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
                    For Each udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
                        If udtEHSClaimSubidizeDetail.Selected Then

                            If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                                tblCell2.Text = udtEHSClaimSubidizeDetail.AvailableItemDescChi
                            ElseIf strLanguage = Common.Component.CultureLanguage.SimpChinese Then
                                tblCell2.Text = udtEHSClaimSubidizeDetail.AvailableItemDescCN
                            Else
                                tblCell2.Text = udtEHSClaimSubidizeDetail.AvailableItemDesc
                            End If

                            Exit For
                        End If
                    Next
                Else
                    Dim udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel = udtEHSClaimSubsidize.SubsidizeDetailList(0)

                    'Item is Dose
                    If Not NonDoseItem.Injection.Equals(udtEHSClaimSubidizeDetail.AvailableItemDesc) Then
                        If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                            tblCell2.Text = udtEHSClaimSubidizeDetail.AvailableItemDescChi
                        ElseIf strLanguage = Common.Component.CultureLanguage.SimpChinese Then
                            tblCell2.Text = udtEHSClaimSubidizeDetail.AvailableItemDescCN
                        Else
                            tblCell2.Text = udtEHSClaimSubidizeDetail.AvailableItemDesc
                        End If
                    Else
                        tblCell2.Text = _strNAText
                    End If

                End If
                tblCell2.Font.Bold = True

                tblCell3.Text = "$" & udtEHSClaimSubsidize.Amount

                'CRE16-026 (Add PCV13) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                '    tblCell4.Text = udtEHSClaimSubsidize.RemarkChi
                'ElseIf strLanguage = Common.Component.CultureLanguage.SimpChinese Then
                '    tblCell4.Text = udtEHSClaimSubsidize.RemarkCN
                'Else
                '    tblCell4.Text = udtEHSClaimSubsidize.Remark
                'End If
                'CRE16-026 (Add PCV13) [End][Chris YIM]

                tblCell4.Style.Add("padding-left", "2px")

                tblCell1.BorderStyle = BorderStyle.Solid
                tblCell2.BorderStyle = BorderStyle.Solid
                tblCell3.BorderStyle = BorderStyle.Solid
                tblCell4.BorderStyle = BorderStyle.Solid
                tblCell1.BorderWidth = New System.Web.UI.WebControls.Unit(1)
                tblCell2.BorderWidth = New System.Web.UI.WebControls.Unit(1)
                tblCell3.BorderWidth = New System.Web.UI.WebControls.Unit(1)
                tblCell4.BorderWidth = New System.Web.UI.WebControls.Unit(1)
                tblCell1.BorderColor = Drawing.Color.Gray
                tblCell2.BorderColor = Drawing.Color.Gray
                tblCell3.BorderColor = Drawing.Color.Gray
                tblCell4.BorderColor = Drawing.Color.Gray

                tblCell2.HorizontalAlign = HorizontalAlign.Center
                tblCell3.HorizontalAlign = HorizontalAlign.Center

                tblRow.Cells.Add(tblCell1)
                tblRow.Cells.Add(tblCell2)
                tblRow.Cells.Add(tblCell3)
                tblRow.Cells.Add(tblCell4)

                dblTotalAmount = dblTotalAmount + udtEHSClaimSubsidize.Amount
                table.Rows.Add(tblRow)

            End If

        Next


        tblRow = New TableRow()
        tblRow.VerticalAlign = VerticalAlign.Top
        tblCell1 = New TableCell()
        tblCell1.Height = 20
        tblCell1.Text = ""

        tblCell2 = New TableCell()
        tblCell2.Height = 20
        tblCell2.Text = Me._strTotalAmount
        tblCell2.CssClass = Me._strCssTableTitle

        tblCell3 = New TableCell()
        tblCell3.Height = 20
        tblCell3.Text = "$" & dblTotalAmount.ToString
        tblCell3.Font.Bold = True
        tblCell3.CssClass = Me._strCssTableText

        tblCell4 = New TableCell()
        tblCell4.Height = 20
        tblCell4.Text = ""

        tblCell1.BorderStyle = BorderStyle.None
        tblCell2.BorderStyle = BorderStyle.Solid
        tblCell3.BorderStyle = BorderStyle.Solid
        tblCell4.BorderStyle = BorderStyle.None

        tblCell2.BorderWidth = New System.Web.UI.WebControls.Unit(1)
        tblCell3.BorderWidth = New System.Web.UI.WebControls.Unit(1)

        tblCell2.BorderColor = Drawing.Color.Gray
        tblCell3.BorderColor = Drawing.Color.Gray

        tblCell2.HorizontalAlign = HorizontalAlign.Center
        tblCell3.HorizontalAlign = HorizontalAlign.Center

        tblRow.Cells.Add(tblCell1)
        tblRow.Cells.Add(tblCell2)
        tblRow.Cells.Add(tblCell3)
        tblRow.Cells.Add(tblCell4)
        table.Rows.Add(tblRow)

        ' Hide the Remarks column if all row empty (empty the text and border)
        Dim blnContainRemark As Boolean = False

        For r As Integer = 1 To table.Rows.Count - 2
            If table.Rows(r).Cells(3).Text <> String.Empty Then
                blnContainRemark = True
                Exit For
            End If
        Next

        If blnContainRemark = False Then
            For Each r As TableRow In table.Rows
                r.Cells(3).Text = String.Empty
                r.Cells(3).BorderStyle = BorderStyle.None
            Next
        End If

        Me.Controls.Add(table)

    End Sub

#Region "Event"
    Protected Sub btnVaccineLegend_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

#End Region

#Region "Properties"

    Public Property VaccineText() As String
        Get
            Return Me._strVaccineText
        End Get
        Set(ByVal value As String)
            Me._strVaccineText = value
        End Set
    End Property

    Public Property DoseText() As String
        Get
            Return Me._strDoseText
        End Get
        Set(ByVal value As String)
            Me._strDoseText = value
        End Set
    End Property

    Public Property AmountText() As String
        Get
            Return Me._strAmountText
        End Get
        Set(ByVal value As String)
            Me._strAmountText = value
        End Set
    End Property

    Public Property RemarksText() As String
        Get
            Return Me._strRemarksText
        End Get
        Set(ByVal value As String)
            Me._strRemarksText = value
        End Set
    End Property

    Public Property NAText() As String
        Get
            Return Me._strNAText
        End Get
        Set(ByVal value As String)
            Me._strNAText = value
        End Set
    End Property

    Public Property TotalAmount() As String
        Get
            Return Me._strTotalAmount
        End Get
        Set(ByVal value As String)
            Me._strTotalAmount = value
        End Set
    End Property

    Public Property CssTableTitle() As String
        Get
            Return Me._strCssTableTitle
        End Get
        Set(ByVal value As String)
            Me._strCssTableTitle = value
        End Set
    End Property

    Public Property CssTableText() As String
        Get
            Return Me._strCssTableText
        End Get
        Set(ByVal value As String)
            Me._strCssTableText = value
        End Set
    End Property

    Public Property VaccineLegendURL() As String
        Get
            Return Me._strVaccineLegendURL
        End Get
        Set(ByVal value As String)
            Me._strVaccineLegendURL = value
        End Set
    End Property

    Public Property VaccineLegendALT() As String
        Get
            Return Me._strVaccineLegendALT
        End Get
        Set(ByVal value As String)
            Me._strVaccineLegendALT = value
        End Set
    End Property

#End Region
End Class
