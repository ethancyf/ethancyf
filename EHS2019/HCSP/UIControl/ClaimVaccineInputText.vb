Imports Common.Component.ClaimRules
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme

Public Class ClaimVaccineInputText
    Inherits System.Web.UI.WebControls.WebControl

    'Table title Text 
    Private _strVaccineText As String
    Private _strDoseText As String
    Private _strAmountText As String
    Private _strRemarksText As String
    Private _strTotalAmount As String
    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Private _strSubsidizeDisabledDetail As String
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    'Value Text 
    Private _strNAText As String

    'Table Text Style 
    Private _strCssTableTitle As String
    Private _strCssTableText As String

    ' Control only 1 Remark button being generated
    Private _blnIsRemarkGenerated As Boolean = False
    Private _blnIsFillValueFromViewState As Boolean = False
    Private _blnIsSupportedDevice As Boolean = False

    Private _lblDoseError As Label

    Private _udtSessionHandler As BLL.SessionHandler = New BLL.SessionHandler()
    'Public Event VaccineLegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event RemarkClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event SubsidizeDisabledRemarkClicked(ByVal sender As Object, ByVal e As EventArgs)
    Public Event SubsidizeCheckboxClickedEnableRecipientCondition(ByVal blnEnabled As Boolean)

    Public Class AttributeName
        Public Const Subsidize As String = "Subsidize"
    End Class

    Public Class ControlIDPrefix
        Public Const CheckBox As String = "chk_ClaimVaccineInput_"
        Public Const CheckBoxLabel As String = "lbl_chk_ClaimVaccineInput_"
        Public Const RadioButton As String = "rb_ClaimVaccineInput_"
        Public Const RadioButtonLabel As String = "lbl_rb_ClaimVaccineInput_"
        Public Const lblClaimDetailsTotal As String = "lblClaimDetailsTotal"
        Public Const btnVaccineLegend As String = "btnVaccineLegend"
        Public Const lblDoseErrorImage As String = "lblVaccineDoseError"
        Public Const RemarkButton As String = "btnRemark"
        Public Const btnSubsidizeDisabledDetail As String = "btnSubsidizeDisabledDetail"
    End Class

    'CRE15-004 (TIV and QIV) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Const FunctCode As String = Common.Component.FunctCode.FUNT020202
    'CRE15-004 (TIV and QIV) [End][Chris YIM]

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Public Class ResourceNamePrefix
        Public Const CLAIM_NOTELIGIBLE_POPUP_CONTENT As String = "CLAIM_NOTELIGIBLE_POPUP_CONTENT_"
    End Class
    'CRE16-026 (Add PCV13) [End][Chris YIM]

    Public Sub Clear()
        Me.Controls.Clear()
    End Sub

    Public Sub Build(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel, ByVal blnIsFillValueFromViewState As Boolean)
        Dim strLanguage As String = Me._udtSessionHandler.Language()

        Dim table As Table = New Table
        Dim tableRow As TableRow = Nothing
        Dim innerTable As Table = New Table
        Dim intTotalAmount As Integer = 0

        Me.Visible = True

        ' Avoid Control Built more than once
        If Me.Controls.Count > 0 Then
            Return
        End If

        ' Indicate value should be feed from session / view state
        Me._blnIsFillValueFromViewState = blnIsFillValueFromViewState

        ' Indicator for generate remark button
        Me._blnIsRemarkGenerated = False

        'table.Width = 240
        table.CellPadding = 0
        table.CellSpacing = 0

        'Add Haeder
        table.Controls.Add(Me.BuildHeader(Me._strVaccineText, True))

        If udtEHSClaimVaccine.SubsidizeList.Count > 1 Then
            Dim innerRow As TableRow
            Dim innerCell As TableCell

            'innerTable.Width = 240
            innerTable.CellPadding = 0
            innerTable.CellSpacing = 0

            For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
                'Add Vaccine Name
                innerTable.Controls.Add(Me.BuildVaccine(udtEHSClaimSubsidize, intTotalAmount, True))

                'Add Dose Name
                tableRow = Me.BuildDose(udtEHSClaimSubsidize, strLanguage)
                If Not tableRow Is Nothing Then
                    ''Add Dose Haeder
                    'table.Rows.Add(Me.BuildHeader(Me._strDoseText))

                    innerTable.Controls.Add(tableRow)
                End If
            Next

            innerRow = New TableRow
            innerCell = New TableCell
            innerCell.Controls.Add(innerTable)
            innerRow.Cells.Add(innerCell)
            table.Rows.Add(innerRow)
        Else
            intTotalAmount = udtEHSClaimVaccine.SubsidizeList(0).Amount

            'Add Vaccine Name
            table.Controls.Add(Me.BuildVaccine(udtEHSClaimVaccine.SubsidizeList(0), intTotalAmount, False))

            'Add Dose Name
            udtEHSClaimVaccine.SubsidizeList(0).Selected = True
            tableRow = Me.BuildDose(udtEHSClaimVaccine.SubsidizeList(0), strLanguage)
            If Not tableRow Is Nothing Then

                ''Add Dose Haeder
                'table.Rows.Add(Me.BuildHeader(Me._strDoseText))

                table.Controls.Add(tableRow)
            End If
        End If

        table.Controls.Add(Me.BuildFooter())
        table.Controls.Add(Me.BuildTotalAmount(intTotalAmount))

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
            ' Dose Error message
            _lblDoseError = New Label()
            _lblDoseError.Text = "*"
            _lblDoseError.Visible = False
            _lblDoseError.CssClass = "validateFailText"
            tableCell.Controls.Add(Me._lblDoseError)

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

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Modify to support multiple scheme seq subsidy
    Private Function BuildVaccine(ByVal udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel, ByRef intTotalAmount As Integer, ByVal needCheckBox As Boolean) As TableRow
        Dim tableRow As TableRow = New TableRow
        Dim tableCell As TableCell = New TableCell
        Dim checkBox As New CheckBox
        Dim checkBoxLabel As Label
        Dim label As Label
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim strRule As String = String.Empty
        Dim strRemarkKey As String = String.Empty
        Dim strLanguage As String = Me._udtSessionHandler.Language()

        '------------------------------------------------------------------
        'Create a check box for user selecte the vaccine
        '------------------------------------------------------------------
        If needCheckBox Then
            checkBox = New CheckBox()
            'checkBox.Text = String.Format("{0} ", udtEHSClaimSubsidize.SubsidizeDisplayCode)
            checkBox.Text = String.Empty
            checkBox.ID = String.Format("{0}{1}", ControlIDPrefix.CheckBox, udtEHSClaimSubsidize.Key)
            checkBox.Attributes(AttributeName.Subsidize) = udtEHSClaimSubsidize.Key
            checkBox.AutoPostBack = True
            AddHandler checkBox.CheckedChanged, AddressOf chkClaim_CheckedChanged



            If Not _blnIsFillValueFromViewState Then
                'if udtEHSClaimSubsidize is selected -> checkbox is checked also
                If udtEHSClaimSubsidize.Available Then
                    If udtEHSClaimSubsidize.Selected Then
                        checkBox.Checked = True
                        intTotalAmount = intTotalAmount + udtEHSClaimSubsidize.Amount
                    End If
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

                End If
            End If

            checkBoxLabel = New Label()
            checkBoxLabel.ID = String.Format("{0}{1}", ControlIDPrefix.CheckBoxLabel, udtEHSClaimSubsidize.Key)
            checkBoxLabel.Text = String.Format("{0} ", udtEHSClaimSubsidize.SubsidizeDisplayCode)
            checkBoxLabel.Enabled = checkBox.Enabled

            tableCell.Controls.Add(checkBox)
            tableCell.Controls.Add(checkBoxLabel)
            tableCell.Controls.Add(New LiteralControl(" "))

        Else
            label = New Label()
            label.Text = String.Format("{0} ", udtEHSClaimSubsidize.SubsidizeDisplayCode)
            tableCell.Controls.Add(label)
        End If

        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
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
                        LinkBtn.Style.Add("top", "-1px")
                        LinkBtn.Style.Add("font-family", "arial")
                        LinkBtn.Style.Add("font-size", "13px")
                        LinkBtn.Style.Add("color", "blue")

                        LinkBtn.Attributes.Add("remark", String.Format("{0}{1}", ResourceNamePrefix.CLAIM_NOTELIGIBLE_POPUP_CONTENT, strRemarkKey))

                        AddHandler LinkBtn.Click, AddressOf btnSubsidizeDisabledRemark_Click
                    End If
            End Select

            If strReason <> String.Empty Then
                '1. Open Tag
                If strRule <> SubsidizeItemDetailRuleModel.TypeClass.USED Then
                    tableCell.Controls.Add(New LiteralControl("<span style='padding-left:3px'>[" + strReason))
                Else
                    tableCell.Controls.Add(New LiteralControl("<span style='padding-left:3px' disabled>[" + strReason))
                End If

                '2. Add link
                If Not LinkBtn Is Nothing Then
                    tableCell.Controls.Add(New LiteralControl(" "))
                    tableCell.Controls.Add(LinkBtn)
                End If

                '3. Close Tag
                tableCell.Controls.Add(New LiteralControl("]</span>"))
            End If

        End If
        'CRE16-026 (Add PCV13) [End][Chris YIM]

        tableCell.CssClass = Me._strCssTableText
        tableRow.Controls.Add(tableCell)

        Return tableRow
    End Function
    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Modify to support multiple scheme seq subsidy
    Private Function BuildDose(ByVal udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel, ByVal strLanguage As String) As TableRow
        Dim tableRow As TableRow = Nothing
        Dim tableCell As TableCell

        If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
            tableRow = New TableRow
            tableCell = New TableCell

            Dim rdDose As RadioButton = Nothing
            Dim rdDoseLabel As Label = Nothing
            For Each udtEHSClaimSubidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
                If Not rdDose Is Nothing Then
                    tableCell.Controls.Add(New LiteralControl("<br/>"))
                End If

                rdDose = New RadioButton
                rdDose.ID = String.Format("{0}{1}{2}", ControlIDPrefix.RadioButton, udtEHSClaimSubsidize.Key, udtEHSClaimSubidizeDetail.AvailableItemCode)
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [Start][Twinsen]
                'rdDose.GroupName = udtEHSClaimSubsidize.SubsidizeCode.Trim()
                rdDose.GroupName = udtEHSClaimSubsidize.Key.Trim()
                ' CRE12-008-02 Allowing different subsidy level for each scheme at different date period [End][Twinsen]
                rdDose.Checked = False
                rdDose.Attributes("value") = udtEHSClaimSubidizeDetail.AvailableItemCode
                If Not _blnIsFillValueFromViewState Then
                    If udtEHSClaimSubidizeDetail.Available AndAlso udtEHSClaimSubidizeDetail.Selected Then
                        rdDose.Checked = udtEHSClaimSubidizeDetail.Selected
                    End If
                End If

                'rdDose.Enabled = udtEHSClaimSubidizeDetail.Available
                rdDose.Enabled = True
                'CRE16-026 (Add PCV13) [Start][Chris YIM]
                '-----------------------------------------------------------------------------------------
                'If (Me.IsSupportedDevice AndAlso Not udtEHSClaimSubidizeDetail.Available) OrElse udtEHSClaimSubidizeDetail.Received Then
                If Me.IsSupportedDevice AndAlso Not udtEHSClaimSubidizeDetail.Available Then
                    'CRE16-026 (Add PCV13) [End][Chris YIM]

                    rdDose.Enabled = False
                    rdDose.Checked = False
                End If

                tableCell.Controls.Add(rdDose)

                rdDoseLabel = New Label()
                rdDoseLabel.ID = String.Format("{0}{1}{2}", ControlIDPrefix.RadioButtonLabel, udtEHSClaimSubsidize.Key, udtEHSClaimSubidizeDetail.AvailableItemCode)
                rdDoseLabel.Enabled = rdDose.Enabled
                rdDoseLabel.EnableViewState = False
                rdDoseLabel.Text = IIf(strLanguage = Common.Component.CultureLanguage.English, udtEHSClaimSubidizeDetail.AvailableItemDesc, udtEHSClaimSubidizeDetail.AvailableItemDescChi)
                tableCell.Controls.Add(rdDoseLabel)

                If Not _blnIsFillValueFromViewState Then
                    'if Subsidize is not selected -> disable radio button group
                    If Me.IsSupportedDevice AndAlso Not udtEHSClaimSubsidize.Selected Then
                        rdDose.Enabled = False
                        rdDoseLabel.Enabled = False
                    End If
                End If
            Next

            tableCell.CssClass = Me._strCssTableText
            tableCell.VerticalAlign = VerticalAlign.Top
            tableCell.BorderColor = Drawing.Color.DarkGray
            tableRow.Controls.Add(tableCell)

        End If

        Return tableRow
    End Function
    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

    Private Function BuildFooter() As TableRow
        Dim tableRow As TableRow
        Dim tableCell As TableCell

        'Total Amount Footer Text
        tableRow = New TableRow()

        tableCell = New TableCell
        tableCell.Text = Me._strTotalAmount
        tableCell.BorderColor = Drawing.Color.DarkGray
        tableCell.CssClass = Me._strCssTableTitle
        If Not Me.IsSupportedDevice() Then
            tableCell.Visible = False
        End If
        tableRow.Controls.Add(tableCell)

        Return tableRow
    End Function

    Private Function BuildTotalAmount(ByVal intTotalAmount As Integer) As TableRow
        Dim tableRow As TableRow
        Dim tableCell As TableCell

        tableRow = New TableRow()

        ' total amount label
        Dim label As Label = New Label
        label.ID = ControlIDPrefix.lblClaimDetailsTotal
        label.CssClass = Me._strCssTableText

        If Not _blnIsFillValueFromViewState Then
            label.Text = String.Format("${0}", intTotalAmount)
        End If

        'Total Amount Footer Text
        tableCell = New TableCell
        tableCell.Controls.Add(label)
        tableCell.BorderColor = Drawing.Color.DarkGray
        tableCell.CssClass = Me._strCssTableText

        If Not Me.IsSupportedDevice() Then
            tableCell.Visible = False
        End If

        tableRow.Controls.Add(tableCell)

        Return tableRow
    End Function

#Region "Events"

    Protected Sub btnRemark_Clicked(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent RemarkClicked(sender, e)
    End Sub

    Protected Sub chkClaim_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs)
        Dim chkbox As CheckBox = CType(sender, CheckBox)
        Dim strSubsidizeKey As String = chkbox.Attributes(AttributeName.Subsidize).ToString()
        Dim lblClaimDetailsTotal As Label = CType(Me.FindControl(ControlIDPrefix.lblClaimDetailsTotal), Label)
        Dim intAmount As Integer = 0
        Dim intAvailableDose As Integer = 0
        Dim udtEHSClaimVaccine As EHSClaimVaccineModel = Me._udtSessionHandler.EHSClaimVaccineGetFromSession()
        'CRE16-026 (Add PCV13) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Dim blnEnableRecipientCondition As Boolean = False

        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            If udtEHSClaimSubsidize.Key = strSubsidizeKey Then
                udtEHSClaimSubsidize.Selected = chkbox.Checked

                If udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
                    If chkbox.Checked Then
                        SetDoseRadioButtonGroupEnabled(udtEHSClaimSubsidize, True)
                    ElseIf Me.IsSupportedDevice Then
                        SetDoseRadioButtonGroupEnabled(udtEHSClaimSubsidize, False)
                    End If
                End If

            End If

            Select Case udtEHSClaimSubsidize.HighRiskOption
                Case SubsidizeGroupClaimModel.HighRiskOptionClass.ShowForInput
                    If udtEHSClaimSubsidize.Selected Then
                        blnEnableRecipientCondition = True
                    End If
            End Select

            If udtEHSClaimSubsidize.Selected Then
                intAmount = intAmount + udtEHSClaimSubsidize.Amount
            End If
        Next

        Me._udtSessionHandler.EHSClaimVaccineSaveToSession(udtEHSClaimVaccine)

        lblClaimDetailsTotal.Text = String.Format("${0}", intAmount.ToString())

        RaiseEvent SubsidizeCheckboxClickedEnableRecipientCondition(blnEnableRecipientCondition)
        'CRE16-026 (Add PCV13) [End][Chris YIM]
    End Sub

    'CRE16-026 (Add PCV13) [Start][Chris YIM]
    '-----------------------------------------------------------------------------------------
    Protected Sub btnSubsidizeDisabledRemark_Click(ByVal sender As Object, ByVal e As EventArgs)
        RaiseEvent SubsidizeDisabledRemarkClicked(sender, e)
    End Sub
    'CRE16-026 (Add PCV13) [End][Chris YIM]

#End Region

#Region "Other functions"

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Modify to use Subsidize Key rather tahn Subsidize Code as a unique key
    Private Sub SetDoseRadioButtonGroupEnabled(ByVal udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel, ByVal blnIsEnable As Boolean)
        Dim ctrl As Control = Nothing
        For Each udtEHSClaimSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
            ' Find Radio Button
            ctrl = Me.FindControl(String.Format("{0}{1}{2}", ControlIDPrefix.RadioButton, udtEHSClaimSubsidize.Key, udtEHSClaimSubsidizeDetail.AvailableItemCode))
            If Not ctrl Is Nothing Then
                With CType(ctrl, RadioButton)
                    If blnIsEnable AndAlso udtEHSClaimSubsidizeDetail.Available Then
                        .Enabled = True
                    Else
                        .Checked = False
                        .Enabled = False
                    End If
                End With
            End If

            ' Find Label
            ctrl = Me.FindControl(String.Format("{0}{1}{2}", ControlIDPrefix.RadioButtonLabel, udtEHSClaimSubsidize.Key, udtEHSClaimSubsidizeDetail.AvailableItemCode))
            If Not ctrl Is Nothing Then
                If blnIsEnable AndAlso udtEHSClaimSubsidizeDetail.Available Then
                    CType(ctrl, Label).Enabled = True
                Else
                    CType(ctrl, Label).Enabled = False
                End If
            End If
        Next
    End Sub
    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Modify to use Subsidize Key rather tahn Subsidize Code as a unique key
    Private Sub ClearDoseRadioButtonGroup(ByVal udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel)
        Dim ctrl As Control = Nothing
        For Each udtEHSClaimSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList
            ctrl = Me.FindControl(String.Format("{0}{1}{2}", ControlIDPrefix.RadioButton, udtEHSClaimSubsidize.Key, udtEHSClaimSubsidizeDetail.AvailableItemCode))
            If Not ctrl Is Nothing Then
                With CType(ctrl, RadioButton)
                    .Checked = False
                End With
            End If
        Next
    End Sub
    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]

    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [Start][Koala]
    ' -----------------------------------------------------------------------------------------
    ' Modify to use Subsidize Key rather tahn Subsidize Code as a unique key
    Public Function SetEHSVaccineDoseSelected(ByVal udtEHSClaimVaccine As EHSClaimVaccineModel) As EHSClaimVaccineModel
        For Each udtEHSClaimSubsidize As EHSClaimVaccineModel.EHSClaimSubsidizeModel In udtEHSClaimVaccine.SubsidizeList
            If udtEHSClaimSubsidize.Selected AndAlso udtEHSClaimSubsidize.SubsidizeDetailList.Count > 1 Then
                For Each udtSubsidizeDetail As EHSClaimVaccineModel.EHSClaimSubidizeDetailModel In udtEHSClaimSubsidize.SubsidizeDetailList

                    Dim ctrl As Control = Me.FindControl(String.Format("{0}{1}{2}", ControlIDPrefix.RadioButton, udtEHSClaimSubsidize.Key, udtSubsidizeDetail.AvailableItemCode))
                    If Not ctrl Is Nothing AndAlso CType(ctrl, RadioButton).Checked Then
                        udtSubsidizeDetail.Selected = True
                    Else
                        udtSubsidizeDetail.Selected = False
                    End If
                Next
            End If
        Next

        Return udtEHSClaimVaccine
    End Function
    ' CRE12-008-01 Allowing different subsidy level for each scheme at different date period [End][Koala]


    Public Sub SetDoseErrorImage(ByVal blnVisible As Boolean)
        If Not Me._lblDoseError Is Nothing Then
            Me._lblDoseError.Visible = blnVisible
        End If
    End Sub

#End Region

#Region "Properties"

    Public Property IsSupportedDevice() As Boolean
        Get
            Return Me._blnIsSupportedDevice
        End Get
        Set(ByVal value As Boolean)
            Me._blnIsSupportedDevice = value
        End Set
    End Property

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

End Class
