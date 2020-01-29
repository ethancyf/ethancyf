Public Class ClaimVaccineReadOnlyText
    Inherits System.Web.UI.WebControls.WebControl
    'Table title Text 
    Private _strVaccineText As String
    Private _strDoseText As String
    Private _strAmountText As String
    Private _strTotalAmount As String
    Private _strRemarksText As String

    'Value Text 
    Private _strNAText As String
    Private _blnIsShowRemark As Boolean = True

    'Table Text Style 
    Private _strCssTableTitle As String
    Private _strCssTableText As String

    ' Control only 1 Remark button being generated
    Private _blnIsRemarkGenerated As Boolean = False

    Private _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
    'Public Event VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event RemarkClicked(ByVal sender As Object, ByVal e As EventArgs)

    Public Class AttributeName
        Public Const Subsidize As String = "Subsidize"
    End Class

    Public Class ControlIDPrefix
        Public Const lblClaimDetailsTotal As String = "lblClaimDetailsTotal"
        Public Const btnVaccineLegend As String = "btnVaccineLegend"
        Public Const RemarkButton As String = "btnRemark"
    End Class

    Public Class NonDoseItem
        Public Const Injection As String = "Injection"
    End Class

    Public Sub Build(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel)
        Dim strLanguage As String = Me._udtSessionHandler.Language()
        Dim table As Table = New Table
        Dim tableRow As TableRow = Nothing
        Dim blnBuildAmount As Boolean = False
        Dim intTotalAmount As Integer = 0

        'table.Width = 240
        table.CellPadding = 0
        table.CellSpacing = 0

        ' Indicator for generate remark button
        Me._blnIsRemarkGenerated = False

        If udtEHSClaimVaccine.SubsidizeList.Count > 0 Then

            'Add Haeder
            table.Rows.Add(Me.BuildHeader(Me._strVaccineText, Me._blnIsShowRemark))

            ' Add Vaccine
            BuildVaccine(udtEHSClaimVaccine, table, strLanguage)

            ' Calculate Amount
            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                If udtEHSClaimSubsidize.Selected Then
                    blnBuildAmount = True
                    intTotalAmount = intTotalAmount + udtEHSClaimSubsidize.Amount
                End If
            Next

        End If

        If blnBuildAmount Then
            table.Rows.Add(Me.BuildFooter())
            table.Rows.Add(Me.BuildTotalAmount(intTotalAmount))
        End If
        Me.Controls.Add(table)

    End Sub

    Private Function BuildHeader(ByVal strHeader As String, Optional ByVal blnIsShowRemark As Boolean = False) As TableRow
        Dim tableRow As TableRow = New TableRow
        Dim tableCell As TableCell = New TableCell

        Dim lblHeader As Label = New Label()
        lblHeader.Text = strHeader
        lblHeader.CssClass = Me._strCssTableTitle
        tableCell.Controls.Add(lblHeader)

        If blnIsShowRemark AndAlso Not _blnIsRemarkGenerated Then
            Dim lblSeparator As Label = New Label()
            lblSeparator.Width = Unit.Pixel(5)
            lblSeparator.Text = " "
            tableCell.Controls.Add(lblSeparator)

            Dim btnRemark As Button = New Button()
            btnRemark.ID = ControlIDPrefix.RemarkButton
            btnRemark.SkinID = "TextOnlyVersionLinkButton"
            btnRemark.Text = _strRemarksText
            AddHandler btnRemark.Click, AddressOf btnRemark_Clicked
            tableCell.Controls.Add(btnRemark)

            Me._blnIsRemarkGenerated = True
        End If

        tableRow.Controls.Add(tableCell)

        Return tableRow
    End Function

    Private Function BuildContent(ByVal strContent As String) As TableRow
        Dim tableRow As TableRow
        Dim tableCell As TableCell

        tableRow = New TableRow

        'Vaccine Text
        tableCell = New TableCell
        tableCell.Text = strContent
        tableCell.CssClass = Me._strCssTableText
        tableRow.Controls.Add(tableCell)

        Return tableRow
    End Function

    Private Sub BuildVaccine(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel, ByVal tblTable As Table, ByVal strLanguage As String)
        Dim tableRow As TableRow = New TableRow
        Dim tableCell As TableCell = New TableCell
        Dim strSubsidizeList As List(Of String) = New List(Of String)()
        Dim blnIsShowListItemOnSingleEntry As Boolean = True

        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            If udtEHSClaimSubsidize.Selected Then
                Dim strSubsidize As String = udtEHSClaimSubsidize.SubsidizeDisplayCode
                Dim strDose As String = GetDoseDescription(udtEHSClaimSubsidize, strLanguage)

                strSubsidizeList.Add(IIf(strDose.Length > 0, String.Format("{0} - {1}", strSubsidize, strDose), strSubsidize))
            End If
        Next

        If strSubsidizeList.Count > 1 OrElse (strSubsidizeList.Count = 1 AndAlso Not blnIsShowListItemOnSingleEntry) Then
            For Each strSubsidize As String In strSubsidizeList
                Dim unOrderListItem As HtmlGenericControl = New HtmlGenericControl("li")
                unOrderListItem.InnerText = strSubsidize

                tableCell.Controls.Add(unOrderListItem)
            Next
        ElseIf strSubsidizeList.Count = 1 Then
            Dim label As Label = New Label()
            label.Text = strSubsidizeList(0)

            tableCell.Controls.Add(label)
        End If

        If tableCell.Controls.Count > 0 Then
            tableRow.Controls.Add(tableCell)
            tblTable.Controls.Add(tableRow)
        End If

    End Sub

    Private Function GetDoseDescription(ByVal udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel, ByVal strLanguage As String) As String

        If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
            For Each udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
                If udtEHSClaimSubidizeDetail.Selected Then

                    If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                        Return udtEHSClaimSubidizeDetail.AvailableItemDescChi
                    Else
                        Return udtEHSClaimSubidizeDetail.AvailableItemDesc
                    End If

                End If
            Next
        Else
            Dim udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel = udtEHSClaimSubsidize.SubsidizeDetailList(0)

            'Item is Dose
            If Not NonDoseItem.Injection.Equals(udtEHSClaimSubidizeDetail.AvailableItemDesc) Then
                If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                    Return udtEHSClaimSubidizeDetail.AvailableItemDescChi
                Else
                    Return udtEHSClaimSubidizeDetail.AvailableItemDesc
                End If

            End If

        End If

        Return String.Empty
    End Function

    'Private Function BuildVaccine(ByVal udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel) As TableRow
    '    Dim tableRow As TableRow = New TableRow
    '    Dim tableCell As TableCell = New TableCell
    '    Dim label As Label = New Label()
    '    label.Text = udtEHSClaimSubsidize.SubsidizeDisplayCode

    '    tableCell.Controls.Add(label)
    '    tableCell.CssClass = Me._strCssTableText

    '    tableRow.Controls.Add(tableCell)

    '    Return tableRow
    'End Function

    'Private Function BuildDose(ByVal udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel, ByVal strLanguage As String) As TableRow
    '    Dim tableRow As TableRow = Nothing
    '    Dim tableCell As TableCell = New TableCell()
    '    Dim blnBuildTableRow As Boolean = False

    '    If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
    '        For Each udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
    '            If udtEHSClaimSubidizeDetail.Selected Then

    '                If strLanguage = Common.Component.CultureLanguage.TradChinese Then
    '                    tableCell.Text = udtEHSClaimSubidizeDetail.AvailableItemDescChi
    '                Else
    '                    tableCell.Text = udtEHSClaimSubidizeDetail.AvailableItemDesc
    '                End If
    '                blnBuildTableRow = True
    '                Exit For
    '            End If
    '        Next
    '    Else
    '        Dim udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel = udtEHSClaimSubsidize.SubsidizeDetailList(0)

    '        'Item is Dose
    '        If Not NonDoseItem.Injection.Equals(udtEHSClaimSubidizeDetail.AvailableItemDesc) Then
    '            If strLanguage = Common.Component.CultureLanguage.TradChinese Then
    '                tableCell.Text = udtEHSClaimSubidizeDetail.AvailableItemDescChi
    '            Else
    '                tableCell.Text = udtEHSClaimSubidizeDetail.AvailableItemDesc
    '            End If
    '            blnBuildTableRow = True
    '        End If

    '    End If

    '    If blnBuildTableRow Then
    '        tableRow = New TableRow

    '        tableCell.CssClass = Me._strCssTableText
    '        tableRow.Controls.Add(tableCell)
    '    End If

    '    Return tableRow
    'End Function

    Private Function BuildFooter() As TableRow
        Dim tableRow As TableRow
        Dim tableCell As TableCell

        'Total Amount Footer Text
        tableRow = New TableRow()

        tableCell = New TableCell
        tableCell.Text = Me._strTotalAmount
        tableCell.CssClass = Me._strCssTableTitle
        tableRow.Controls.Add(tableCell)

        Return tableRow
    End Function

    Private Function BuildAmount(ByVal intTotalAmount As Integer) As TableRow
        Dim tableRow As TableRow = New TableRow()
        Dim tableCell As TableCell = New TableCell

        tableCell.Text = String.Format("${0}", intTotalAmount)
        tableCell.CssClass = Me._strCssTableText
        tableRow.Controls.Add(tableCell)

        Return tableRow
    End Function

    Private Function BuildTotalAmount(ByVal intTotalAmount As Integer) As TableRow
        Dim tableRow As TableRow
        Dim tableCell As TableCell

        tableRow = New TableRow()

        'Total Amount Footer Text
        tableCell = New TableCell
        tableCell.Text = String.Format("${0}", intTotalAmount)
        tableCell.CssClass = Me._strCssTableText
        tableRow.Controls.Add(tableCell)

        Return tableRow
    End Function

#Region "Events"

    Protected Sub btnRemark_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent RemarkClicked(sender, e)
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


    Public Property ShowRemark() As Boolean
        Get
            Return Me._blnIsShowRemark
        End Get
        Set(ByVal value As Boolean)
            Me._blnIsShowRemark = value
        End Set
    End Property

#End Region

End Class
