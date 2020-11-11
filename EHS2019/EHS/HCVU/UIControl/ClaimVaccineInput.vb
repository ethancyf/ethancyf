Imports Common.Component.ClaimRules
Imports Common.Component.EHSClaimVaccine
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.VoucherScheme
Imports Common.Format
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls

Public Class ClaimVaccineInput
    Inherits System.Web.UI.WebControls.WebControl

    'Table title Text 
    Private _strVaccineText As String
    Private _strDoseText As String
    Private _strAmountText As String
    Private _strRemarksText As String
    Private _strTotalAmount As String
    Private _strVaccineLegendURL As String
    Private _strVaccineLegendALT As String
    Private _blnShowLegend As Boolean
    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private _strSubsidizeDisabledDetail As String
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    'Value Text 
    Private _strNAText As String

    'Table Text Style 
    Private _strCssTableTitle As String
    Private _strCssTableText As String

    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private _blnShowRemark As Boolean = False
    'CRE16-002 (Revamp VSS) [End][Chris YIM]

    Private _imageDoseError As Image

    Private _udtSessionHandler As BLL.SessionHandlerBLL = New BLL.SessionHandlerBLL

    'function code
    Private _strFunctionCode As String

    Public Event VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event SubsidizeDisabledRemarkClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event SubsidizeCheckboxClickedEnableRecipientCondition(ByVal blnEnabled As Boolean)

    Public Class AttributeName
        Public Const Subsidize As String = "Subsidize"
    End Class

    Public Class ControlIDPrefix
        Public Const CheckBox As String = "chk_ClaimVaccineInput_"
        Public Const RadioButton As String = "rb_ClaimVaccineInput_"
        Public Const lblClaimDetailsTotal As String = "lblClaimDetailsTotal"
        Public Const btnVaccineLegend As String = "btnVaccineLegend"
        Public Const imgDoseErrorImage As String = "imgVaccineDoseError"
        Public Const btnSubsidizeDisabledDetail As String = "btnSubsidizeDisabledDetail"
    End Class

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Class ResourceNamePrefix
        Public Const CLAIM_NOTELIGIBLE_POPUP_CONTENT As String = "CLAIM_NOTELIGIBLE_POPUP_CONTENT_"
    End Class
    'CRE16-026 (Add PCV13) [End][Chris YIM]

#Region "Build control"

    Public Sub Build(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel, ByVal strFunctionCode As String) ', ByVal strLanguage As String)
        Me._strFunctionCode = strFunctionCode
        Dim strLanguage As String = Me._udtSessionHandler.Language()
        Dim table As Table = New Table
        Dim intTotalAmount As Integer = 0

        Me.Visible = True

        If Not udtEHSClaimVaccine Is Nothing Then
            table.CellPadding = 2
            table.CellSpacing = 0

            'Add Header Text -> | Vaccine | Dose | Amount | Remarks |
            table.Controls.Add(Me.BuildHeaderRow())

            'Vaccine Content -> for more then one Subsidize only
            If udtEHSClaimVaccine.SubsidizeList.Count > 1 Then

                For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList

                    table.Controls.Add(Me.BuildContent(udtEHSClaimSubsidize, strLanguage, intTotalAmount))
                Next
            ElseIf udtEHSClaimVaccine.SubsidizeList.Count = 1 Then
                table.Controls.Add(Me.BuildContent(udtEHSClaimVaccine.SubsidizeList(0), strLanguage))
                intTotalAmount = udtEHSClaimVaccine.SubsidizeList(0).Amount
            End If

            'Add Header Text -> |    | Total Amount  | $XXX |   |
            table.Controls.Add(Me.BuildFooterRow(intTotalAmount.ToString()))

            If Not _blnShowRemark Then
                For Each tr As TableRow In table.Rows
                    'tr.Cells(0).Width = Unit.Pixel(195)
                    tr.Cells(1).Width = Unit.Pixel(300)
                    tr.Cells(2).Width = Unit.Pixel(145)
                    tr.Cells(3).Visible = False
                Next
            Else
                For Each tr As TableRow In table.Rows
                    'tr.Cells(0).Width = Unit.Pixel(195)
                    tr.Cells(1).Width = Unit.Pixel(300)
                    tr.Cells(2).Width = Unit.Pixel(145)
                    tr.Cells(3).Width = Unit.Pixel(320)
                Next
            End If

            Me.Controls.Clear()
            Me.Controls.Add(table)
        End If
    End Sub


    Private Function BuildContent(ByVal udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel, ByVal strLanguage As String, ByRef intTotalAmount As Double) As TableRow
        Dim tableRow As TableRow
        Dim tableCell As TableCell
        tableRow = New TableRow
        tableCell = New TableCell

        '------------------------------------------------------------------
        'Create a check box for user selecte the vaccine
        '------------------------------------------------------------------
        Dim checkBox As New CheckBox

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Use Subsidize Key rather than SubsidizeCode
        checkBox.ID = String.Format("{0}{1}", ControlIDPrefix.CheckBox, udtEHSClaimSubsidize.Key)
        checkBox.Attributes(AttributeName.Subsidize) = udtEHSClaimSubsidize.Key
        checkBox.AutoPostBack = True
        AddHandler checkBox.CheckedChanged, AddressOf chkClaim_CheckedChanged
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        'if udtEHSClaimSubsidize is selected -> checkbox is checked also
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim strRule As String = String.Empty
        Dim strRemarkKey As String = String.Empty

        If udtEHSClaimSubsidize.Available Then
            checkBox.Text = udtEHSClaimSubsidize.SubsidizeDisplayCode
            If udtEHSClaimSubsidize.Selected Then
                checkBox.Checked = True
                intTotalAmount = intTotalAmount + udtEHSClaimSubsidize.Amount
            End If

            tableCell.Controls.Add(checkBox)
        Else
            udtEHSClaimSubsidize.Selected = False

            checkBox.Text = String.Empty
            checkBox.Checked = False
            checkBox.Enabled = False

            'Generate remark for why the subsidy is disabled
            strRemarkKey = udtEHSClaimSubsidize.Key

            Dim lstRemark As List(Of String) = udtEHSClaimSubsidize.SubsidizeDisabledRemark
            Dim strRemark As String = String.Empty

            '
            If lstRemark.Count > 0 Then
                For i As Integer = 0 To lstRemark.Count - 1
                    strRule = strRule + lstRemark.Item(i) + "|"
                Next

                If Len(strRule) > 0 Then
                    strRule = Mid(strRule, 1, Len(strRule) - 1)
                End If
            End If

            Dim strRemarkList() As String = Split(HttpContext.GetGlobalResourceObject("Text", strRemarkKey, New System.Globalization.CultureInfo(strLanguage)), "|")
            If strRemarkList.Length > 0 Then
                For i As Integer = 0 To strRemarkList.Length - 1
                    strRemark = strRemark + strRemarkList(i) + Environment.NewLine
                Next
            End If

            'Color Label
            Dim lbl As New Label
            lbl.Text = udtEHSClaimSubsidize.SubsidizeDisplayCode
            lbl.Style.Add("color", "#AAAAAA")

            tableCell.Controls.Add(checkBox)
            tableCell.Controls.Add(lbl)
            tableCell.Controls.Add(New LiteralControl(" "))

        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        tableCell.VerticalAlign = VerticalAlign.Top
        tableCell.CssClass = Me._strCssTableText
        tableCell.BorderWidth = 1
        tableCell.BorderColor = Drawing.Color.DarkGray
        tableRow.Controls.Add(tableCell)

        '------------------------------------------------------------------
        'Create content for subsidize 
        '-> it will be radio button, if more then one dose for selected -> case of udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1
        '-> it will be string "N/A" only, if no need to select -> case of udtEHSClaimSubsidize.SubsidizeDetailList.Count = 1
        '------------------------------------------------------------------
        tableCell = New TableCell
        If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
            'Create radio button
            Dim radioButton As New RadioButtonList

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Use Subsidize Key rather than SubsidizeCode
            radioButton.ID = String.Format("{0}{1}", ControlIDPrefix.RadioButton, udtEHSClaimSubsidize.Key)
            radioButton.RepeatDirection = RepeatDirection.Horizontal
            radioButton.RepeatLayout = RepeatLayout.Table
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            'Create ListItem for RadioButton
            For Each udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList

                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                If udtEHSClaimSubidizeDetail.InternalUse Then
                    Continue For
                End If
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                Dim listItem As ListItem = New ListItem()

                'Add description for the radio button, may be  "1st", "2nd" <- case for dose
                If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                    'listItem = New ListItem(udtEHSClaimSubidizeDetail.AvailableItemDescChi, udtEHSClaimSubidizeDetail.AvailableItemCode, udtEHSClaimSubidizeDetail.Available)
                    listItem = New ListItem(udtEHSClaimSubidizeDetail.AvailableItemDescChi, udtEHSClaimSubidizeDetail.AvailableItemCode)
                Else
                    'listItem = New ListItem(udtEHSClaimSubidizeDetail.AvailableItemDesc, udtEHSClaimSubidizeDetail.AvailableItemCode, udtEHSClaimSubidizeDetail.Available)
                    listItem = New ListItem(udtEHSClaimSubidizeDetail.AvailableItemDesc, udtEHSClaimSubidizeDetail.AvailableItemCode)
                End If

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                ' Obsolete the mark dose display name logic
                'If udtEHSClaimSubidizeDetail.AvailableItemCode.Trim().ToUpper() = "VACCINE" OrElse udtEHSClaimSubidizeDetail.AvailableItemCode.Trim.ToUpper = "ONLYDOSE" Then
                '    listItem.Text = Me._strNAText
                'End If
                ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]

                'If udtEHSClaimSubidizeDetail.Available Then
                If udtEHSClaimSubidizeDetail.Selected Then
                    listItem.Selected = udtEHSClaimSubidizeDetail.Selected
                End If
                'Else

                'End If

                radioButton.Items.Add(listItem)
            Next

            'if Subsidize is not selected -> disable radio button group

            '  ---- All the button should be able to select for Back Office user input 2010-07-13 ---- '
            radioButton.Enabled = True
            'radioButton.ClearSelection()

            '' 2009-12-01: Do not disable the button
            'If Not udtEHSClaimSubsidize.Selected Then
            '    'radioButton.Enabled = False
            '    radioButton.ClearSelection()
            'End If

            '  ---- End 2010-07-13 ---- '

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Use Subsidize Key rather than SubsidizeCode
            radioButton.Attributes(AttributeName.Subsidize) = udtEHSClaimSubsidize.Key
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
            tableCell.Controls.Add(radioButton)
            tableCell.CssClass = Me._strCssTableText

            radioButton.AutoPostBack = True
            AddHandler radioButton.SelectedIndexChanged, AddressOf rb_Claim_SelectedindexChanged

        ElseIf udtEHSClaimSubsidize.SubsidizeDetailList.Count = 1 Then

            Dim udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel = udtEHSClaimSubsidize.SubsidizeDetailList(0)

            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
            If Not udtSubsidizeDetail.InternalUse Then

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                ' Obsolete the mark dose display name logic
                'If udtSubsidizeDetail.AvailableItemCode.Trim().ToUpper() = "VACCINE" OrElse udtSubsidizeDetail.AvailableItemCode.Trim().ToUpper() = "ONLYDOSE" Then
                '    tableCell.Text = Me._strNAText
                'Else
                If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                    tableCell.Text = udtSubsidizeDetail.AvailableItemDescChi
                Else
                    tableCell.Text = udtSubsidizeDetail.AvailableItemDesc
                End If
                'End If
                ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]

                'If udtSubsidizeDetail.AvailableItemCode = "VACCINE" Then
                'Only one Subsidize -> add a N/A text only

                'Else
                '    If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                '        tableCell.Text = udtSubsidizeDetail.AvailableItemDescChi
                '    Else
                '        tableCell.Text = udtSubsidizeDetail.AvailableItemDesc
                '    End If
                'End If
                tableCell.CssClass = Me._strCssTableText

        End If
            ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
        Else
            'Only one Subsidize -> add a N/A text only
            tableCell.Text = Me._strNAText
            tableCell.CssClass = Me._strCssTableText
        End If

        tableCell.VerticalAlign = VerticalAlign.Top
        tableCell.HorizontalAlign = HorizontalAlign.Center
        tableCell.BorderWidth = 1
        tableCell.BorderColor = Drawing.Color.DarkGray

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If Not udtEHSClaimSubsidize.Available Then
            tableCell.Style.Add("color", "#AAAAAA")
        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        tableRow.Controls.Add(tableCell)

        '------------------------------------------------------------------
        'Create Amount text
        '------------------------------------------------------------------
        tableCell = New TableCell
        tableCell.Text = String.Format("${0}", udtEHSClaimSubsidize.Amount.ToString())
        tableCell.VerticalAlign = VerticalAlign.Top
        tableCell.HorizontalAlign = HorizontalAlign.Center
        tableCell.BorderWidth = 1
        tableCell.BorderColor = Drawing.Color.DarkGray
        tableCell.CssClass = Me._strCssTableTitle

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If Not udtEHSClaimSubsidize.Available Then
            tableCell.Style.Add("color", "#AAAAAA")
        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        tableRow.Controls.Add(tableCell)

        '------------------------------------------------------------------
        'Create Remarks text
        '------------------------------------------------------------------
        tableCell = New TableCell

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Show VACCINE FEE and/or INJECTION FEE
        If udtEHSClaimSubsidize.SchemeCode <> SchemeClaimModel.RVP Then
            Dim strSubsidizeRemark As String = String.Empty

            If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                strSubsidizeRemark = udtEHSClaimSubsidize.RemarkChi
            Else
                strSubsidizeRemark = udtEHSClaimSubsidize.Remark
            End If

            Dim span As New HtmlGenericControl("span")
            span.InnerHtml = strSubsidizeRemark
            span.Style.Add("padding-left", "3px")

            If strSubsidizeRemark <> String.Empty Then
                tableCell.Controls.Add(span)
            End If
        End If

        'Show SUBSIDY USED or NOT ELIGIBLE (DETAILS)
        If Not udtEHSClaimSubsidize.Available Then
            Dim strReason As String = String.Empty
            Dim strDetailContent As String = String.Empty
            Dim LinkBtn As LinkButton = Nothing

            Select Case strRule
                Case SubsidizeItemDetailRuleModel.TypeClass.USED
                    strReason = HttpContext.GetGlobalResourceObject("Text", SubsidizeItemDetailRuleModel.TypeClass.USED, New System.Globalization.CultureInfo(strLanguage))
                Case String.Empty
                    'Nothing to do, no display in remarks
                Case Else
                    strReason = HttpContext.GetGlobalResourceObject("Text", SubsidizeItemDetailRuleModel.TypeClass.NOTELIGIBLE, New System.Globalization.CultureInfo(strLanguage))
                    strDetailContent = HttpContext.GetGlobalResourceObject("Text", String.Format("{0}{1}", ResourceNamePrefix.CLAIM_NOTELIGIBLE_POPUP_CONTENT, strRemarkKey), New System.Globalization.CultureInfo(strLanguage))

                    'If subsidy is not contained the detail when it is disabled, the hyperlink do not show.
                    If Not strDetailContent Is Nothing AndAlso strDetailContent <> String.Empty Then
                        'Details LinkButton
                        LinkBtn = New LinkButton
                        LinkBtn.ID = String.Format("{0}{1}", ControlIDPrefix.btnSubsidizeDisabledDetail, udtEHSClaimSubsidize.Key)
                        LinkBtn.Text = "(" + Me._strSubsidizeDisabledDetail + ")"
                        LinkBtn.Style.Add("vertical-align", "middle")
                        LinkBtn.Style.Add("position", "relative")
                        LinkBtn.Style.Add("top", "-2px")
                        LinkBtn.Style.Add("font-family", "arial")
                        LinkBtn.Style.Add("font-size", "16px")
                        LinkBtn.Style.Add("color", "rgb(0, 102, 209)")

                        LinkBtn.Attributes.Add("remark", String.Format("{0}{1}", ResourceNamePrefix.CLAIM_NOTELIGIBLE_POPUP_CONTENT, strRemarkKey))

                        AddHandler LinkBtn.Click, AddressOf btnSubsidizeDisabledRemark_Click
                    End If
            End Select

            '1. Open Tag
            If tableCell.Controls.Count > 0 Then
                tableCell.Controls.Add(New LiteralControl("<BR />"))
            End If
            tableCell.Controls.Add(New LiteralControl("<span style='padding-left:3px'>" + strReason))

            '2. Add link
            If Not LinkBtn Is Nothing Then
                tableCell.Controls.Add(New LiteralControl(" "))
                tableCell.Controls.Add(LinkBtn)
            End If

            '3. Close Tag
            tableCell.Controls.Add(New LiteralControl("</span>"))
        End If

        If tableCell.Controls.Count > 0 Then
            _blnShowRemark = True
        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        tableCell.BorderWidth = 1
        tableCell.VerticalAlign = VerticalAlign.Top
        tableCell.BorderColor = Drawing.Color.DarkGray
        tableCell.CssClass = Me._strCssTableTitle
        tableRow.Controls.Add(tableCell)
        Return tableRow
    End Function

    Private Function BuildContent(ByVal udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel, ByVal strLanguage As String) As TableRow
        Dim tableRow As TableRow
        Dim tableCell As TableCell

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim strRule As String = String.Empty
        Dim strRemarkKey As String = String.Empty

        '------------------------------------------------------------------
        'Create a check box for user selecte the vaccine
        '------------------------------------------------------------------
        tableRow = New TableRow
        tableCell = New TableCell

        tableCell.Text = udtEHSClaimSubsidize.SubsidizeDisplayCode
        tableCell.VerticalAlign = VerticalAlign.Top
        tableCell.CssClass = Me._strCssTableText
        tableCell.BorderWidth = 1
        tableCell.BorderColor = Drawing.Color.DarkGray

        If Not udtEHSClaimSubsidize.Available Then
            udtEHSClaimSubsidize.Selected = False

            'Generate remark for why the subsidy is disabled
            strRemarkKey = udtEHSClaimSubsidize.Key

            Dim lstRemark As List(Of String) = udtEHSClaimSubsidize.SubsidizeDisabledRemark
            Dim strRemark As String = String.Empty

            '
            If lstRemark.Count > 0 Then
                For i As Integer = 0 To lstRemark.Count - 1
                    strRule = strRule + lstRemark.Item(i) + "|"
                    'strRemark = strRemark + HttpContext.GetGlobalResourceObject("Text", lstRemark.Item(i), New System.Globalization.CultureInfo(strLanguage)) + ", "
                Next

                If Len(strRule) > 0 Then
                    strRule = Mid(strRule, 1, Len(strRule) - 1)
                    'strRemark = Mid(strRemark, 1, Len(strRemark) - 2)
                End If
            End If

            Dim strRemarkList() As String = Split(HttpContext.GetGlobalResourceObject("Text", strRemarkKey, New System.Globalization.CultureInfo(strLanguage)), "|")
            If strRemarkList.Length > 0 Then
                For i As Integer = 0 To strRemarkList.Length - 1
                    strRemark = strRemark + strRemarkList(i) + Environment.NewLine
                Next

            End If

            tableCell.Style.Add("color", "#AAAAAA")
        End If

        tableRow.Controls.Add(tableCell)
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        '------------------------------------------------------------------
        'Create content for subsidize 
        '-> it will be radio button, if more then one dose for selected -> case of udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1
        '-> it will be string "N/A" only, if no need to select -> case of udtEHSClaimSubsidize.SubsidizeDetailList.Count = 1
        '------------------------------------------------------------------
        tableCell = New TableCell
        If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
            'Create radio button
            Dim radioButton As New RadioButtonList
            Dim intAvailItem As Integer = 0

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Use Subsidize Key rather than SubsidizeCode
            radioButton.ID = String.Format("{0}{1}", ControlIDPrefix.RadioButton, udtEHSClaimSubsidize.Key)
            radioButton.RepeatDirection = RepeatDirection.Horizontal
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

            'Create ListItem for RadioButton
            For Each udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList

                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                If udtEHSClaimSubidizeDetail.InternalUse Then
                    Continue For
                End If
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]

                Dim listItem As ListItem = New ListItem()

                'Add description for the radio button, may be  "1st", "2nd" <- case for dose
                If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                    'listItem = New ListItem(udtEHSClaimSubidizeDetail.AvailableItemDescChi, udtEHSClaimSubidizeDetail.AvailableItemCode, udtEHSClaimSubidizeDetail.Available)
                    listItem = New ListItem(udtEHSClaimSubidizeDetail.AvailableItemDescChi, udtEHSClaimSubidizeDetail.AvailableItemCode)
                Else
                    'listItem = New ListItem(udtEHSClaimSubidizeDetail.AvailableItemDesc, udtEHSClaimSubidizeDetail.AvailableItemCode, udtEHSClaimSubidizeDetail.Available)
                    listItem = New ListItem(udtEHSClaimSubidizeDetail.AvailableItemDesc, udtEHSClaimSubidizeDetail.AvailableItemCode)
                End If

                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                '' Obsolete the mark dose display name logic
                'If udtEHSClaimSubidizeDetail.AvailableItemCode.Trim().ToUpper() = "VACCINE" OrElse udtEHSClaimSubidizeDetail.AvailableItemCode.Trim.ToUpper = "ONLYDOSE" Then
                '    listItem.Text = Me._strNAText
                'End If
                ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]

                'If udtEHSClaimSubidizeDetail.Available Then
                '    intAvailItem += 1
                If udtEHSClaimSubidizeDetail.Selected Then
                    listItem.Selected = udtEHSClaimSubidizeDetail.Selected
                End If
                'End If

                radioButton.Items.Add(listItem)
            Next

            '  ---- All the button should be able to select for Back Office user input 2010-07-13 ---- '
            radioButton.Enabled = True
            'radioButton.ClearSelection()

            ''if Subsidize is not selected -> disable radio button group
            'If Not udtEHSClaimSubsidize.Available Then
            '    radioButton.Enabled = False
            '    radioButton.ClearSelection()
            'Else
            '    'If intAvailItem = 1 Then
            '    '    For Each listItem As ListItem In radioButton.Items
            '    '        If listItem.Enabled Then
            '    '            listItem.Selected = True
            '    '            Exit For
            '    '        End If
            '    '    Next
            '    'End If
            'End If

            '  ---- End 2010-07-13 ---- '

            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
            ' -----------------------------------------------------------------------------------------
            ' Use Subsidize Key rather than SubsidizeCode
            radioButton.Attributes(AttributeName.Subsidize) = udtEHSClaimSubsidize.Key
            ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
            tableCell.Controls.Add(radioButton)
            tableCell.CssClass = Me._strCssTableText

        ElseIf udtEHSClaimSubsidize.SubsidizeDetailList.Count = 1 Then

            Dim udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel = udtEHSClaimSubsidize.SubsidizeDetailList(0)

            If Not udtSubsidizeDetail.InternalUse Then
                'Only one Subsidize -> add a N/A text only
                ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
                ' ----------------------------------------------------------
                ' Obsolete the mark dose display name logic
                'If udtSubsidizeDetail.AvailableItemCode.Trim().ToUpper() = "VACCINE" OrElse udtSubsidizeDetail.AvailableItemCode.Trim().ToUpper() = "ONLYDOSE" Then
                '    tableCell.Text = Me._strNAText
                'Else
                If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                    tableCell.Text = udtSubsidizeDetail.AvailableItemDescChi
                Else
                    tableCell.Text = udtSubsidizeDetail.AvailableItemDesc
                End If
                'End If
                ' CRE18-004(CIMS Vaccination Sharing) [End][Koala CHENG]

                tableCell.CssClass = Me._strCssTableText
        End If
        Else
            'Only one Subsidize -> add a N/A text only
            tableCell.Text = Me._strNAText
            tableCell.CssClass = Me._strCssTableText
        End If

        tableCell.VerticalAlign = VerticalAlign.Top
        tableCell.HorizontalAlign = HorizontalAlign.Center
        tableCell.BorderWidth = 1
        tableCell.BorderColor = Drawing.Color.DarkGray

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If Not udtEHSClaimSubsidize.Available Then
            tableCell.Style.Add("color", "#AAAAAA")
        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        tableRow.Controls.Add(tableCell)

        '------------------------------------------------------------------
        'Create Amount text
        '------------------------------------------------------------------
        tableCell = New TableCell
        tableCell.Text = String.Format("${0}", udtEHSClaimSubsidize.Amount.ToString())
        tableCell.VerticalAlign = VerticalAlign.Top
        tableCell.HorizontalAlign = HorizontalAlign.Center
        tableCell.BorderWidth = 1
        tableCell.BorderColor = Drawing.Color.DarkGray
        tableCell.CssClass = Me._strCssTableTitle
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        If Not udtEHSClaimSubsidize.Available Then
            tableCell.Style.Add("color", "#AAAAAA")
        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        tableRow.Controls.Add(tableCell)

        '------------------------------------------------------------------
        'Create Remarks text
        '------------------------------------------------------------------
        tableCell = New TableCell

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        'Show VACCINE FEE and/or INJECTION FEE
        If udtEHSClaimSubsidize.SchemeCode <> SchemeClaimModel.RVP Then
            Dim strSubsidizeRemark As String = String.Empty

            If strLanguage = Common.Component.CultureLanguage.TradChinese Then
                tableCell.Text = udtEHSClaimSubsidize.RemarkChi
            Else
                tableCell.Text = udtEHSClaimSubsidize.Remark
            End If


            Dim span As New HtmlGenericControl("span")
            span.InnerHtml = strSubsidizeRemark
            span.Style.Add("padding-left", "3px")

            If strSubsidizeRemark <> String.Empty Then
                tableCell.Controls.Add(span)
            End If
        End If

        'Show SUBSIDY USED or NOT ELIGIBLE (DETAILS)
        If Not udtEHSClaimSubsidize.Available Then
            Dim strReason As String = String.Empty
            Dim strDetailContent As String = String.Empty
            Dim LinkBtn As LinkButton = Nothing

            Select Case strRule
                Case SubsidizeItemDetailRuleModel.TypeClass.USED
                    strReason = HttpContext.GetGlobalResourceObject("Text", SubsidizeItemDetailRuleModel.TypeClass.USED, New System.Globalization.CultureInfo(strLanguage))
                Case String.Empty
                    'Nothing to do, no display in remarks
                Case Else
                    strReason = HttpContext.GetGlobalResourceObject("Text", SubsidizeItemDetailRuleModel.TypeClass.NOTELIGIBLE, New System.Globalization.CultureInfo(strLanguage))
                    strDetailContent = HttpContext.GetGlobalResourceObject("Text", String.Format("{0}{1}", ResourceNamePrefix.CLAIM_NOTELIGIBLE_POPUP_CONTENT, strRemarkKey), New System.Globalization.CultureInfo(strLanguage))

                    'If subsidy is not contained the detail when it is disabled, the hyperlink do not show.
                    If Not strDetailContent Is Nothing AndAlso strDetailContent <> String.Empty Then
                        'Details LinkButton
                        LinkBtn = New LinkButton
                        LinkBtn.ID = String.Format("{0}{1}", ControlIDPrefix.btnSubsidizeDisabledDetail, udtEHSClaimSubsidize.Key)
                        LinkBtn.Text = "(" + Me._strSubsidizeDisabledDetail + ")"
                        LinkBtn.Style.Add("vertical-align", "middle")
                        LinkBtn.Style.Add("position", "relative")
                        LinkBtn.Style.Add("top", "-2px")
                        LinkBtn.Style.Add("font-family", "arial")
                        LinkBtn.Style.Add("font-size", "16px")
                        LinkBtn.Style.Add("color", "rgb(0, 102, 209)")

                        LinkBtn.Attributes.Add("remark", String.Format("{0}{1}", ResourceNamePrefix.CLAIM_NOTELIGIBLE_POPUP_CONTENT, strRemarkKey))

                        AddHandler LinkBtn.Click, AddressOf btnSubsidizeDisabledRemark_Click
                    End If
            End Select

            '1. Open Tag
            If tableCell.Controls.Count > 0 Then
                tableCell.Controls.Add(New LiteralControl("<BR />"))
            End If
            tableCell.Controls.Add(New LiteralControl("<span style='padding-left:3px'>" + strReason))

            '2. Add link
            If Not LinkBtn Is Nothing Then
                tableCell.Controls.Add(New LiteralControl(" "))
                tableCell.Controls.Add(LinkBtn)
            End If

            '3. Close Tag
            tableCell.Controls.Add(New LiteralControl("</span>"))
        End If

        If tableCell.Controls.Count > 0 Then
            _blnShowRemark = True
        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        tableCell.BorderWidth = 1
        tableCell.VerticalAlign = VerticalAlign.Top
        tableCell.BorderColor = Drawing.Color.DarkGray
        tableCell.CssClass = Me._strCssTableTitle
        tableRow.Controls.Add(tableCell)

        Return tableRow
    End Function

    Private Function BuildHeaderRow() As TableRow

        Dim tableRow As TableRow
        Dim tableCell As TableCell
        Dim imageButton As ImageButton
        Dim lable As Label
        Dim image As Image

        tableRow = New TableRow
        lable = New Label
        lable.Text = String.Format("{0} ", Me._strVaccineText)
        lable.CssClass = Me._strCssTableTitle

        'Vaccine Header Text
        If _blnShowLegend Then
            imageButton = New ImageButton
            imageButton.ID = ControlIDPrefix.btnVaccineLegend
            imageButton.ImageUrl = Me._strVaccineLegendURL
            imageButton.AlternateText = Me._strVaccineLegendALT
            imageButton.ImageAlign = ImageAlign.AbsMiddle
            AddHandler imageButton.Click, AddressOf btnVaccineLegend_Click
        End If

        tableCell = New TableCell
        tableCell.HorizontalAlign = HorizontalAlign.Center
        tableCell.BorderWidth = 1
        tableCell.BorderColor = Drawing.Color.DarkGray
        'tableCell.Width = 170
        tableCell.Style.Add("min-width", "195px")
        tableCell.Controls.Add(lable)

        If _blnShowLegend AndAlso Not IsNothing(imageButton) Then
            tableCell.Controls.Add(imageButton)
        End If

        tableRow.Controls.Add(tableCell)

        'Dose Header Text
        lable = New Label()
        lable.Text = String.Format("{0} ", Me._strDoseText)
        lable.CssClass = Me._strCssTableTitle

        image = New Image()
        image.ID = ControlIDPrefix.imgDoseErrorImage
        image.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "ErrorBtn")
        image.AlternateText = HttpContext.GetGlobalResourceObject("AlternateText", "ErrorBtn")
        image.ImageAlign = ImageAlign.AbsMiddle
        image.Visible = False

        Me._imageDoseError = image

        tableCell = New TableCell
        'tableCell.Text = Me._strDoseText
        tableCell.HorizontalAlign = HorizontalAlign.Center
        tableCell.BorderWidth = 1
        tableCell.BorderColor = Drawing.Color.DarkGray
        tableCell.Width = 280
        tableCell.CssClass = Me._strCssTableTitle
        tableCell.Controls.Add(lable)
        tableCell.Controls.Add(image)
        tableRow.Controls.Add(tableCell)

        'Amount Header Text
        tableCell = New TableCell
        tableCell.Text = Me._strAmountText
        tableCell.HorizontalAlign = HorizontalAlign.Center
        tableCell.BorderWidth = 1
        tableCell.BorderColor = Drawing.Color.DarkGray
        tableCell.Width = 120
        tableCell.CssClass = Me._strCssTableTitle
        tableRow.Controls.Add(tableCell)

        'Remarks Header Text
        tableCell = New TableCell
        tableCell.Text = Me._strRemarksText
        tableCell.HorizontalAlign = HorizontalAlign.Center
        tableCell.BorderWidth = 1
        tableCell.BorderColor = Drawing.Color.DarkGray
        tableCell.Width = 320
        tableCell.CssClass = Me._strCssTableTitle
        tableRow.Controls.Add(tableCell)

        Return tableRow
    End Function

    Private Function BuildFooterRow(ByVal totalAmount As String) As TableRow
        Dim tableRow As TableRow
        Dim tableCell As TableCell

        'Vaccine Footer Text -> no value inside
        tableRow = New TableRow
        tableCell = New TableCell
        tableRow.Controls.Add(tableCell)

        'Total Amount Footer Text
        tableCell = New TableCell
        tableCell.Text = Me._strTotalAmount
        tableCell.HorizontalAlign = HorizontalAlign.Center
        tableCell.BorderWidth = 1
        tableCell.BorderColor = Drawing.Color.DarkGray
        tableCell.CssClass = Me._strCssTableTitle
        tableRow.Controls.Add(tableCell)

        'Total Amount Value Footer Text
        Dim label As Label = New Label
        label.ID = ControlIDPrefix.lblClaimDetailsTotal
        label.Text = String.Format("${0}", totalAmount)
        label.CssClass = Me._strCssTableText
        tableCell = New TableCell
        tableCell.Controls.Add(label)
        tableCell.HorizontalAlign = HorizontalAlign.Center
        tableCell.BorderWidth = 1
        tableCell.BorderColor = Drawing.Color.DarkGray
        tableRow.Controls.Add(tableCell)

        'Remarks Footer Text -> no value inside
        tableCell = New TableCell
        tableRow.Controls.Add(tableCell)
        Return tableRow
    End Function

#End Region

#Region "Events"


    Protected Sub rb_Claim_SelectedindexChanged(ByVal sender As Object, ByVal e As EventArgs)

        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Use Subsidize Key rather than SubsidizeCode
        Dim rbButtonList As RadioButtonList = CType(sender, RadioButtonList)
        Dim strSubsidizeKey As String = rbButtonList.Attributes(AttributeName.Subsidize).ToString()
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession(Me._strFunctionCode)
        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            If udtEHSClaimSubsidize.Key = strSubsidizeKey Then

                If Not rbButtonList.SelectedValue Is Nothing AndAlso rbButtonList.SelectedValue <> "" Then
                    Dim chkBox As CheckBox = CType(Me.FindControl(String.Format("{0}{1}", ControlIDPrefix.CheckBox, udtEHSClaimSubsidize.Key.Trim())), CheckBox)
                    If chkBox.Checked = False Then
                        chkBox.Checked = True
                        HandleVaccineCheckBoxChanged(chkBox)
                    End If
                Else
                End If
            End If
        Next
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]
    End Sub

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Sub chkClaim_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chkbox As CheckBox = CType(sender, CheckBox)
        HandleVaccineCheckBoxChanged(chkbox)
    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    Protected Sub btnVaccineLegend_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent VaccineLegendClicked(sender, e)
    End Sub

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Sub btnSubsidizeDisabledRemark_Click(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent SubsidizeDisabledRemarkClicked(sender, e)
    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

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

    Public Property TotalAmount() As String
        Get
            Return Me._strTotalAmount
        End Get
        Set(ByVal value As String)
            Me._strTotalAmount = value
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



    Public Property FunctionCode() As String
        Get
            Return Me._strFunctionCode
        End Get
        Set(ByVal value As String)
            Me._strFunctionCode = value
        End Set
    End Property

    Public Property ShowLegend() As Boolean
        Get
            Return Me._blnShowLegend
        End Get
        Set(ByVal value As Boolean)
            Me._blnShowLegend = value
        End Set
    End Property

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Property SubsidizeDisableDetail() As String
        Get
            Return Me._strSubsidizeDisabledDetail
        End Get
        Set(ByVal value As String)
            Me._strSubsidizeDisabledDetail = value
        End Set
    End Property
    'CRE16-026 (Add PCV13) [End][Chris YIM]

#End Region

#Region "Other functions"

    Private Sub HandleVaccineCheckBoxChanged(ByVal chkBox As CheckBox)
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
        ' -----------------------------------------------------------------------------------------
        ' Use Subsidize Key rather than SubsidizeCode
        Dim strSubsidizeKey As String = chkBox.Attributes(AttributeName.Subsidize).ToString()
        Dim lblClaimDetailsTotal As Label = CType(Me.FindControl(ControlIDPrefix.lblClaimDetailsTotal), Label)
        Dim intAmount As Integer = 0
        Dim intAvailableDose As Integer = 0
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession(Me._strFunctionCode)
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim blnEnableRecipientCondition As Boolean = False
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList

            If udtEHSClaimSubsidize.Key = strSubsidizeKey Then
                udtEHSClaimSubsidize.Selected = chkBox.Checked

                ' 2009-12-01: 
                ' CheckBox = Checked: Do Nothing
                ' CheckBox = UnChecked: If radio button exist, clear the radio button selection 

                If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
                    Dim radioButtons As RadioButtonList = Me.FindControl(String.Format("{0}{1}", ControlIDPrefix.RadioButton, udtEHSClaimSubsidize.Key.Trim()))
                    If chkBox.Checked Then
                        'radioButtons.Enabled = True

                        'For Each listItem As ListItem In radioButtons.Items
                        '    If listItem.Enabled Then

                        '        intAvailableDose += 1

                        '        'If intAvailableDose = 1 Then
                        '        '    listItem.Selected = True
                        '        'Else
                        '        If intAvailableDose > 1 Then
                        '            radioButtons.ClearSelection()
                        '            Exit For
                        '        End If
                        '    End If
                        'Next
                    Else

                        For Each udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
                            udtEHSClaimSubidizeDetail.Selected = False
                        Next
                        'radioButtons.Enabled = False
                        radioButtons.ClearSelection()
                    End If
                End If

            End If

            'CRE16-026 (Add PCV13) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Select Case udtEHSClaimSubsidize.HighRiskOption
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput
                    If udtEHSClaimSubsidize.Selected Then
                        blnEnableRecipientCondition = True
                    End If
            End Select
            'CRE16-026 (Add PCV13) [End][Chris YIM]

            If udtEHSClaimSubsidize.Selected Then
                intAmount = intAmount + udtEHSClaimSubsidize.Amount
            End If
        Next
        ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

        Me._udtSessionHandler.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine, Me._strFunctionCode)

        lblClaimDetailsTotal.Text = String.Format("${0}", intAmount.ToString())

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        RaiseEvent SubsidizeCheckboxClickedEnableRecipientCondition(blnEnableRecipientCondition)
        'CRE16-026 (Add PCV13) [End][Chris YIM]
    End Sub


    Public Function SetEHSVaccineModelDoseSelectedFromUIInput(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel

        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList

            If udtEHSClaimSubsidize.Selected Then
                If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
                    Dim radioButton As RadioButtonList = Me.FindControl(String.Format("{0}{1}", ControlIDPrefix.RadioButton, udtEHSClaimSubsidize.Key.Trim()))

                    If udtEHSClaimSubsidize.Selected AndAlso Not radioButton.SelectedValue.Equals(String.Empty) Then


                        For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList

                            If udtSubsidizeDetail.AvailableItemCode = radioButton.SelectedValue.Trim() Then
                                udtSubsidizeDetail.Selected = True
                            Else
                                udtSubsidizeDetail.Selected = False
                            End If

                        Next
                        'udtEHSClaimSubsidize.SubsidizeDetailList.Filter(radioButton.SelectedValue.Trim()).Selected = True
                    End If
                End If
            End If
        Next

        Return udtEHSClaimVaccine
    End Function

    Public Sub SetDoseErrorImage(ByVal blnVisible As Boolean)
        If Not Me._imageDoseError Is Nothing Then
            Me._imageDoseError.Visible = blnVisible
        End If
    End Sub
#End Region
End Class

