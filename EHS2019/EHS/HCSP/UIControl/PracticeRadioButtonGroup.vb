''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''''''''''''''
' Date to be deploy: 2009-10-19
''''''''''''''''''''''''''''''''''''''''''''''''
''''''''''''''''''''''''''''''''''''''''''''''''
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Common.Component
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.SchemeInformation
Imports Common.Component.VoucherScheme
Imports Common.Format
Imports Common.Component.Scheme
Imports HCSP.BLL.SessionHandler
Imports HCSP.BLL

'Imports Common.Component.UserAC
'Imports Common.Component.DataEntryUser
'Imports Common.Component.ServiceProvider

Public Class PracticeRadioButtonGroup
    Inherits System.Web.UI.WebControls.WebControl

    Public Enum DisplayMode
        BankAccount
        Address
    End Enum


    'style
    Private _strHeaderTextCss As String
    Private _strHeaderCss As String
    Private _strPracticeTextCss As String
    Private _strSchemeLabelCss As String
    Private _blnVerticalScrollBar As Boolean

    'text
    Private _strHeaderText As String

    'Values
    Private _blnMaskBankAccountNo As Boolean
    Private _strSelectButtonImgURL As String
    Private _strPraticeTableWidth As Integer
    Private _intPanelHeight As Integer = -1

    'hide/show
    Private _blnShowCloseButton As Boolean
    'CRE20-XXX COVID-19 [Start][Nichole]
    Private _blnCOVIDService As Boolean
    'CRE20-xxx  COVID-19 [End][Nichole]

    'CRE20-XXX DHC integration [Start][Nichole]
    'hide.show
    Private _blnDHCService As Boolean
    'CRE20-xxx DHC integration [End][Nichole]

    ' CRE20-0XX (HA Scheme) [Start][Winnie]
    Private _blnSchemeSelection As Boolean = False
    Private _strSelectedScheme As String = String.Empty

    Private _practiceDisplays As BLL.PracticeDisplayModelCollection
    Private _practices As PracticeModelCollection
    Private _schemeInfoList As SchemeInformationModelCollection
    Private _strlanguage As String
    Private _displayMode As DisplayMode
    ' CRE20-0XX (HA Scheme) [End][Winnie]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private _enumMode As ClaimMode = ClaimMode.All
    ' CRE20-0022 (Immu record) [End][Chris YIM]

    'Events
    Public Event PracticeSelected(ByVal strPracticeName As String, ByVal strBankAcctNo As String, ByVal intBankAccountDisplaySeqas As Integer, ByVal strSchemeCode As String, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
    Public Event SchemeSelected()

    Private Sub PracticeRadioButtonGroup_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim control As Control = Me.FindControl(MyBase.ID & "_rblScheme")
        If Not control Is Nothing Then
            Dim rblScheme As RadioButtonList = CType(control, RadioButtonList)
            SetSchemeSelectValue(rblScheme, ViewState("SchemeSelected"))

            Dim tblPractice As Control = Me.FindControl(MyBase.ID & "_tblPractice")
            BindPractice(rblScheme.SelectedValue, tblPractice, _practiceDisplays, _practices, _schemeInfoList, _strlanguage, _displayMode)
        End If

    End Sub

    Public Sub BuildRadioButtonGroup(ByVal practiceDisplays As BLL.PracticeDisplayModelCollection, _
                                     ByVal practices As PracticeModelCollection, _
                                     ByVal schemeInfoList As SchemeInformationModelCollection, _
                                     ByVal strlanguage As String, _
                                     ByVal displayMode As DisplayMode, _
                                     Optional ByVal enumMode As ClaimMode = ClaimMode.All)

        _practiceDisplays = practiceDisplays
        _practices = practices
        _schemeInfoList = schemeInfoList
        _strlanguage = strlanguage
        _displayMode = displayMode
        _enumMode = enumMode

        ViewState("SchemeSelected") = Me._strSelectedScheme

        If Not practiceDisplays Is Nothing Then
            Dim formatter As Formatter = New Formatter()
            Dim table As Table
            Dim tableRow As TableRow
            Dim tableCell As TableCell
            Dim panel As Panel
            Dim contentWidth As Integer = Me.PracticeTabelWidth - 20
            Dim strSelectedScheme As String = String.Empty

            Me.Controls.Clear()

            table = New Table
            table.CellPadding = 0
            table.CellSpacing = 0

            'Create Header Text Label------------------------------------------------------------------
            'Row 1, Cell 1
            tableRow = New TableRow()
            Dim headerLabel As Label = New Label()
            headerLabel.Text = Me._strHeaderText
            headerLabel.CssClass = Me._strHeaderTextCss
            headerLabel.Width = Me.PracticeTabelWidth - Unit.Pixel(11).Value
            tableCell = New TableCell
            tableCell.CssClass = Me._strHeaderCss
            tableCell.Controls.Add(headerLabel)
            tableRow.Cells.Add(tableCell)

            'Row 1, Cell 2
            Dim ibtnClose As ImageButton = New ImageButton
            ibtnClose.ID = Me.UniqueID + "$ibtnPopupClose"
            ibtnClose.AlternateText = "Close"
            ibtnClose.CssClass = ""
            ibtnClose.ImageUrl = HttpContext.GetGlobalResourceObject("ImageUrl", "TabHeaderClose")
            ibtnClose.Visible = Me._blnShowCloseButton
            AddHandler ibtnClose.Click, AddressOf btnClose_click

            tableCell = New TableCell
            tableCell.CssClass = ""
            tableCell.Controls.Add(ibtnClose)
            tableRow.Cells.Add(tableCell)

            table.Rows.Add(tableRow)

            ' CRE20-0XX (HA Scheme) [Start][Winnie]
            'Row 2 ' Scheme Selection
            Dim rblScheme As RadioButtonList = New RadioButtonList
            rblScheme.ID = MyBase.ID & "_rblScheme"
            rblScheme.RepeatDirection = RepeatDirection.Horizontal
            rblScheme.Width = Me.PracticeTabelWidth

            BindScheme(rblScheme, practiceDisplays, practices, schemeInfoList, strlanguage)

            tableCell = New TableCell
            tableCell.CssClass = ""
            tableCell.ColumnSpan = 2
            tableCell.Controls.Add(rblScheme)

            rblScheme.AutoPostBack = True
            AddHandler rblScheme.SelectedIndexChanged, AddressOf rblScheme_SelectedIndexChanged


            tableRow = New TableRow()
            tableRow.Cells.Add(tableCell)
            table.Rows.Add(tableRow)
            ' CRE20-0XX (HA Scheme) [End][Winnie]

            'Create Header space8
            'Row 2, Cell 1 (Merged)
            tableCell = New TableCell
            tableCell.Height = 15
            tableCell.ColumnSpan = 2
            tableRow = New TableRow()
            tableRow.Cells.Add(tableCell)
            table.Rows.Add(tableRow)
            Me.Controls.Add(table)
            '--------------------------------------------------------------------------------------------

            'New Table for Practice----------------------------------------------------------------------
            panel = New Panel
            panel.Style("PADDING-RIGHT") = "2px"
            panel.Style("PADDING-LEFT") = "2px"
            panel.Style("PADDING-BOTTOM") = "2px"
            panel.Style("PADDING-TOP") = "2px"

            If Me._intPanelHeight > 0 Then
                panel.Height = Me._intPanelHeight
            End If

            panel.Width = Me.PracticeTabelWidth

            If Me._blnVerticalScrollBar Then
                panel.ScrollBars = ScrollBars.Vertical
            End If

            table = New Table
            table.CellPadding = 5
            table.CellSpacing = 0
            ' CRE20-0XX (HA Scheme) [Start][Winnie]
            table.ID = MyBase.ID & "_tblPractice"

            BindPractice(rblScheme.SelectedValue, table, practiceDisplays, practices, schemeInfoList, strlanguage, displayMode)

            panel.Controls.Add(table)
            Me.Controls.Add(panel)
            ' CRE20-0XX (HA Scheme) [End][Winnie]
        End If
    End Sub

#Region "Event"
    Public Sub btnPracticeSelection_click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim imageButton As ImageButton = CType(sender, ImageButton)
        Dim strPracticeName As String = imageButton.Attributes("DataTextField")
        Dim strBankAcctNo As String = imageButton.Attributes("DataValueField")
        Dim intBankAccountDisplaySeq As Integer = CType(imageButton.Attributes("PracticeDisplaySeq"), Integer)
        Dim strSchemeCode As String = String.Empty

        Dim control As Control = Me.FindControl(MyBase.ID & "_rblScheme")
        If Not control Is Nothing Then
            Dim rblScheme As RadioButtonList = CType(control, RadioButtonList)
            strSchemeCode = rblScheme.SelectedValue
        End If

        RaiseEvent PracticeSelected(strPracticeName, strBankAcctNo, intBankAccountDisplaySeq, strSchemeCode, sender, e)
    End Sub

    Private Sub btnClose_click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Return
    End Sub

    ' CRE20-0XX (HA Scheme) [Start][Winnie]
    Protected Sub rblScheme_SelectedIndexChanged(sender As Object, e As EventArgs)
        Dim rblScheme As RadioButtonList = CType(sender, RadioButtonList)
        Dim tblPractice As Control = Me.FindControl(MyBase.ID & "_tblPractice")

        BindPractice(rblScheme.SelectedValue, tblPractice, _practiceDisplays, _practices, _schemeInfoList, _strlanguage, _displayMode)

        ViewState("SchemeSelected") = rblScheme.SelectedValue
        RaiseEvent SchemeSelected()
    End Sub
    ' CRE20-0XX (HA Scheme) [End][Winnie]
#End Region

#Region "Property"
    Public Property HeaderText() As String
        Get
            Return Me._strHeaderText
        End Get
        Set(ByVal value As String)
            Me._strHeaderText = value
        End Set
    End Property

    Public Property HeaderTextCss() As String
        Get
            Return Me._strHeaderTextCss
        End Get
        Set(ByVal value As String)
            Me._strHeaderTextCss = value
        End Set
    End Property

    Public Property HeaderCss() As String
        Get
            Return Me._strHeaderCss
        End Get
        Set(ByVal value As String)
            Me._strHeaderCss = value
        End Set
    End Property

    Public Property PracticeTextCss() As String
        Get
            Return Me._strPracticeTextCss
        End Get
        Set(ByVal value As String)
            Me._strPracticeTextCss = value
        End Set
    End Property

    Public Property SchemeLabelCss() As String
        Get
            Return Me._strSchemeLabelCss
        End Get
        Set(ByVal value As String)
            Me._strSchemeLabelCss = value
        End Set
    End Property

    Public Property MaskBankAccountNo() As Boolean
        Get
            Return Me._blnMaskBankAccountNo
        End Get
        Set(ByVal value As Boolean)
            Me._blnMaskBankAccountNo = value
        End Set
    End Property


    Public Property SelectButtonURL() As String
        Get
            Return Me._strSelectButtonImgURL
        End Get
        Set(ByVal value As String)
            Me._strSelectButtonImgURL = value
        End Set
    End Property

    Public Property PracticeTabelWidth() As Integer
        Get
            If Me._strPraticeTableWidth <= 0 Then
                Return 800
            Else
                Return Me._strPraticeTableWidth
            End If
        End Get
        Set(ByVal value As Integer)
            Me._strPraticeTableWidth = value
        End Set
    End Property

    Public Property VerticalScrollBar() As Boolean
        Get
            Return Me._blnVerticalScrollBar
        End Get
        Set(ByVal value As Boolean)
            Me._blnVerticalScrollBar = value
        End Set
    End Property

    Public Property PanelHeight() As Integer
        Get
            Return Me._intPanelHeight
        End Get
        Set(ByVal value As Integer)
            Me._intPanelHeight = value
        End Set
    End Property

    Public Property ShowCloseButton() As Boolean
        Get
            Return Me._blnShowCloseButton
        End Get
        Set(ByVal value As Boolean)
            Me._blnShowCloseButton = value
        End Set
    End Property

    ' CRE20-0XX (HA Scheme) [Start][Winnie]
    Public Property SchemeSelection() As Boolean
        Get
            Return Me._blnSchemeSelection
        End Get
        Set(ByVal value As Boolean)
            Me._blnSchemeSelection = value
        End Set
    End Property

    Public Property SelectedScheme() As String
        Get
            Return Me._strSelectedScheme
        End Get
        Set(ByVal value As String)
            Me._strSelectedScheme = value
        End Set
    End Property
    ' CRE20-0XX (HA Scheme) [End][Winnie]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Property ClaimMode() As ClaimMode
        Get
            Return Me._enumMode
        End Get
        Set(ByVal value As ClaimMode)
            Me._enumMode = value
        End Set
    End Property
    ' CRE20-0022 (Immu record) [End][Chris YIM]
#End Region

    ' CRE20-0XX (HA Scheme) [Start][Winnie]
    Public Sub BindScheme(ByVal rblScheme As RadioButtonList, ByVal practiceDisplays As BLL.PracticeDisplayModelCollection, ByVal practices As PracticeModelCollection, ByVal schemeInfoList As SchemeInformationModelCollection, ByVal strlanguage As String)
        Dim udtGF As New Common.ComFunction.GeneralFunction()
        Dim dtmDate As DateTime = udtGF.GetSystemDateTime()
        Dim lstSchemeCode As List(Of String) = New List(Of String)
        Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()

        rblScheme.Items.Clear()

        For Each practiceDisplay As BLL.PracticeDisplayModel In practiceDisplays
            If Not practiceDisplay.Profession.IsClaimPeriod(dtmDate) Then Continue For
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing

            ' CRE20-0022 (Immu record) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            ' Practice Scheme Info List Filter by COVID-19
            Dim udtFilterPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing

            udtFilterPracticeSchemeInfoList = udtSchemeClaimBLL.FilterPracticeSchemeInfo(practices, practiceDisplay.PracticeID, Me.ClaimMode)

            udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(udtFilterPracticeSchemeInfoList, schemeInfoList, dtmDate)
            ' CRE20-0022 (Immu record) [End][Chris YIM]

            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
            ' --------------------------------------------------------------------------------------
            udtSchemeClaimModelCollection = udtSchemeClaimModelCollection.FilterWithoutReadonly()
            ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

            ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
            udtSchemeClaimModelCollection = udtSchemeClaimModelCollection.FilterByHCSPSubPlatform(DirectCast(Me.Page, BasePage).SubPlatform)
            ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

            Dim udtSchemeClaimModelList As SchemeClaimModelCollection = Nothing
            For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollection

                If lstSchemeCode.Contains(udtSchemeClaimModel.SchemeCode) = False Then
                    lstSchemeCode.Add(udtSchemeClaimModel.SchemeCode)
                End If
            Next

        Next

        ' Bind Scheme in order
        Dim udtSchemeClaimModelFullList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllSchemeClaim_WithSubsidizeGroup()

        For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelFullList
            If lstSchemeCode.Contains(udtSchemeClaimModel.SchemeCode) Then
                Dim listItem As ListItem = New ListItem()
                listItem = New ListItem(udtSchemeClaimModel.SchemeDesc(strlanguage), udtSchemeClaimModel.SchemeCode)
                rblScheme.Items.Add(listItem)
            End If
        Next

        ' Select Scheme
        If Me._blnSchemeSelection Then
            rblScheme.Visible = True
            rblScheme.SelectedIndex = 0
        Else
            rblScheme.Visible = False
            rblScheme.SelectedIndex = -1
        End If

        ' Hide If only 1 scheme is available
        If rblScheme.Items.Count <= 1 Then
            rblScheme.Visible = False
            rblScheme.SelectedIndex = -1
        End If

    End Sub

    ' CRE20-0XX (HA Scheme) [Start][Winnie]
    Private Sub SetSchemeSelectValue(ByVal rblScheme As RadioButtonList, ByVal strSelectedValue As String)
        ' Select Scheme
        If strSelectedValue <> String.Empty Then
            Dim listItem As ListItem = rblScheme.Items.FindByValue(strSelectedValue)
            If listItem IsNot Nothing Then
                rblScheme.ClearSelection()
                listItem.Selected = True
            End If
        End If
    End Sub
    ' CRE20-0XX (HA Scheme) [End][Winnie]

    ' CRE20-0XX (HA Scheme) [Start][Winnie]
    Public Sub BindPractice(ByVal strSchemeCode As String, ByVal control As Control, ByVal practiceDisplays As BLL.PracticeDisplayModelCollection, ByVal practices As PracticeModelCollection, ByVal schemeInfoList As SchemeInformationModelCollection, ByVal strlanguage As String, ByVal displayMode As DisplayMode)

        If Not control Is Nothing Then
            Dim tblPractice As Table = CType(control, Table)
            tblPractice.Rows.Clear()

            Dim formatter As Formatter = New Formatter()
            Dim tableRow As TableRow
            Dim tableCell As TableCell
            Dim tableInnerPractice As Table
            Dim tableInnerPracticeRow As TableRow
            Dim tableInnerPracticeCell As TableCell
            Dim label As Label
            Dim label2 As Label
            Dim bankAccountNo As String
            Dim practiceIndex As Integer = 0
            Dim udtSchemeClaimBLL = New SchemeClaimBLL()
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing
            Dim contentWidth As Integer = Me.PracticeTabelWidth - 20

            Dim udtGF As New Common.ComFunction.GeneralFunction()
            Dim dtmDate As DateTime = udtGF.GetSystemDateTime()

            'get the scheme list from system parameter to determind that whether show the practice popup in covid-19 program.
            ' CRE20-023  (Immu record) [Start][Raiman]
            Dim strSchemeListForSelectPracticePopup As String = udtGF.getSystemParameter("Covid19_PracticeSelectPopup_Scheme")
            Dim alSchemeListForSelectPracticePopup As ArrayList = New ArrayList(strSchemeListForSelectPracticePopup.Split(";"))
            ' CRE20-023  (Immu record) [End][Raiman]

            For Each practiceDisplay As BLL.PracticeDisplayModel In practiceDisplays
                ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
                ' -----------------------------------------------------------------------------------------
                ' Filter practice if profession is not available for claim
                If Not practiceDisplay.Profession.IsClaimPeriod(dtmDate) Then Continue For
                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

                ' CRE20-0XX (HA Scheme) [Start][Winnie]
                Dim blnContainsSelectedScheme As Boolean = False
                ' CRE20-0XX (HA Scheme) [End][Winnie]

                'Tabel for each practice
                tableInnerPractice = New Table
                tableInnerPractice.CellPadding = 0
                tableInnerPractice.CellSpacing = 0


                'Create Practice Label--------------------------------------------------------------------
                label = New Label
                label2 = New Label
                label.ID = String.Format("{0}_PracticeSchemeLabel_{1}", MyBase.ID, practiceIndex)
                label2.ID = String.Format("{0}_BankPracticeAddressLabel_{1}", MyBase.ID, practiceIndex)

                'label.Width = contentWidth - 90
                If Me._blnMaskBankAccountNo Then
                    bankAccountNo = formatter.maskBankAccount(practiceDisplay.BankAccountNo)
                Else
                    bankAccountNo = practiceDisplay.BankAccountNo
                End If

                Dim strPracticeName As String = String.Empty
                If strlanguage = CultureLanguage.TradChinese OrElse strlanguage = CultureLanguage.SimpChinese Then
                    strPracticeName = practiceDisplay.PracticeNameChi
                Else
                    strPracticeName = practiceDisplay.PracticeName
                End If

                ' Practice Name (Practice ID) [Bank Account]
                ' [Practice Address]
                If (strlanguage = CultureLanguage.TradChinese OrElse strlanguage = CultureLanguage.SimpChinese) AndAlso Not practiceDisplay.DisplayEngOnly AndAlso Not practiceDisplay.BuildingChi Is Nothing AndAlso practiceDisplay.BuildingChi.Trim() <> "" Then
                    label.Text = String.Format("{0} ({1}) ", strPracticeName, practiceDisplay.PracticeID)
                    label2.Text = String.Format("[{0}]{1}[{2}]", bankAccountNo, "<br>", _
                        formatter.formatAddressChi(practiceDisplay.Room, practiceDisplay.Floor, practiceDisplay.Block, practiceDisplay.BuildingChi, practiceDisplay.District, ""))
                Else
                    label.Text = String.Format("{0} ({1}) ", strPracticeName, practiceDisplay.PracticeID)
                    label2.Text = String.Format("[{0}]{1}[{2}]", bankAccountNo, "<br>", _
                        formatter.formatAddress(practiceDisplay.Room, practiceDisplay.Floor, practiceDisplay.Block, practiceDisplay.Building, practiceDisplay.District, ""))
                End If

                'CRE20-xxx COVID-19 Immu record [Start][Nichole]
                '-- label2 has shown the bank account name + address
                If Me.ClaimMode = ClaimMode.COVID19 Then
                    label2.Visible = False
                End If

                'label.Height = 25
                'label.Attributes("value") = String.Format("{0}-{1}-{2}", practice.SPID, practice.DisplaySeq, practice.BankAcct.DisplaySeq)
                If strlanguage = CultureLanguage.TradChinese OrElse strlanguage = CultureLanguage.SimpChinese Then
                    label.CssClass = Me._strPracticeTextCss
                Else
                    label.CssClass = Me._strHeaderTextCss
                End If

                label2.CssClass = Me._strHeaderTextCss

                'Add Practice Label into cells
                tableInnerPracticeCell = New TableCell
                tableInnerPracticeCell.Width = contentWidth - 90
                tableInnerPracticeCell.Controls.Add(label)
                tableInnerPracticeCell.Controls.Add(label2)

                tableInnerPracticeRow = New TableRow()
                tableInnerPracticeRow.Cells.Add(tableInnerPracticeCell)
                tableInnerPractice.Rows.Add(tableInnerPracticeRow)
                '----------------------------------------------------------------------------------------

                ' CRE20-0022 (Immu record) [Start][Chris YIM]
                ' ---------------------------------------------------------------------------------------------------------
                ' Practice Scheme Info List Filter by COVID-19
                Dim udtFilterPracticeSchemeInfoList As PracticeSchemeInfoModelCollection = Nothing

                udtFilterPracticeSchemeInfoList = udtSchemeClaimBLL.FilterPracticeSchemeInfo(practices, practiceDisplay.PracticeID, _enumMode)

                udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(udtFilterPracticeSchemeInfoList, schemeInfoList, dtmDate)
                ' CRE20-0022 (Immu record) [End][Chris YIM]

                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                ' --------------------------------------------------------------------------------------
                udtSchemeClaimModelCollection = udtSchemeClaimModelCollection.FilterWithoutReadonly()
                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                udtSchemeClaimModelCollection = udtSchemeClaimModelCollection.FilterByHCSPSubPlatform(DirectCast(Me.Page, BasePage).SubPlatform)
                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

                ' CRE20-023  (Immu record) [Start][Raiman]
                Dim blnIsContainCovid19Scheme = False
                blnIsContainCovid19Scheme = alSchemeListForSelectPracticePopup.Contains("ALL")
                ' CRE20-023  (Immu record) [End][Raiman]

                For Each udtSchemeClaimModel As SchemeClaimModel In udtSchemeClaimModelCollection
                    'Add Space label
                    label = New Label()
                    label.Width = 25

                    'Create Scheme Label
                    Dim schemeLabel As Label = New Label()

                    schemeLabel.Text = String.Format("- {0}", udtSchemeClaimModel.SchemeDesc(strlanguage))

                    schemeLabel.Height = 20
                    schemeLabel.CssClass = Me._strSchemeLabelCss

                    'Add Cell for scheme label
                    tableInnerPracticeCell = New TableCell
                    tableInnerPracticeCell.Controls.Add(label)
                    'CRE20-0xx COVID-19 hidden the scheme info [Start][Nichole]
                    If Me.ClaimMode = ClaimMode.All Or Me.ClaimMode = ClaimMode.DHC Then
                        tableInnerPracticeCell.Controls.Add(schemeLabel)
                    End If
                    'CRE20-0xx COVID-19 hidden the scheme info [End][Nichole]

                    tableInnerPracticeRow = New TableRow()
                    tableInnerPracticeRow.Cells.Add(tableInnerPracticeCell)
                    tableInnerPractice.Rows.Add(tableInnerPracticeRow)

                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaimModel.SchemeCode)

                    ' CRE20-0XX (HA Scheme) [Start][Winnie]
                    If Me._blnSchemeSelection AndAlso strSchemeCode <> String.Empty Then
                        If udtConvertedSchemeCode = strSchemeCode Then
                            blnContainsSelectedScheme = True
                        End If
                    Else
                        blnContainsSelectedScheme = True
                    End If
                    ' CRE20-0XX (HA Scheme) [End][Winnie]

                    If practices(practiceDisplay.PracticeID).PracticeSchemeInfoList.Filter(udtConvertedSchemeCode).IsNonClinic() Then
                        'Add Space label
                        label = New Label()
                        label.Width = 35

                        'Create Scheme Label
                        schemeLabel = New Label()
                        schemeLabel.Text = String.Format("({0})", HttpContext.GetGlobalResourceObject("Text", "ProvideServiceAtNonClinicSetting"))

                        schemeLabel.Height = 20
                        schemeLabel.CssClass = Me._strSchemeLabelCss

                        'Add Cell for Non-Clinic Statement
                        tableInnerPracticeCell = New TableCell
                        tableInnerPracticeCell.Controls.Add(label)
                        tableInnerPracticeCell.Controls.Add(schemeLabel)

                        tableInnerPracticeRow = New TableRow()
                        tableInnerPracticeRow.Cells.Add(tableInnerPracticeCell)
                        tableInnerPractice.Rows.Add(tableInnerPracticeRow)
                    End If
                    'CRE16-002 (Revamp VSS) [End][Chris YIM]


                    ' CRE20-023  (Immu record) [Start][Raiman]
                    If (Not blnIsContainCovid19Scheme) Then
                        blnIsContainCovid19Scheme = alSchemeListForSelectPracticePopup.Contains(udtSchemeClaimModel.SchemeCode)
                    End If
                    ' CRE20-023  (Immu record) [End][Raiman]
                Next

                If blnContainsSelectedScheme = False Then
                    Continue For
                End If
                ''Add available schemes-------------------------------------------------------------------
                'For Each practiceSchemeInfo As PracticeSchemeInfoModel In practice.PracticeSchemeInfoList.Values



                'Next
                '----------------------------------------------------------------------------------------

                'Add Practice and schemes table
                tableCell = New TableCell
                tableCell.BorderWidth = 1
                tableCell.BorderStyle = WebControls.BorderStyle.Solid
                tableCell.BorderColor = Drawing.Color.Black

                tableCell.Controls.Add(tableInnerPractice)
                tableRow = New TableRow()
                tableRow.Cells.Add(tableCell)


                'Add Selection Button
                Dim btnPracticeSelection As ImageButton = New ImageButton()
                btnPracticeSelection.ID = String.Format("{0}_PracticeSchemeImageButton_{1}", MyBase.ID, practiceIndex)
                btnPracticeSelection.ImageUrl = Me._strSelectButtonImgURL
                btnPracticeSelection.Attributes("DataTextField") = practiceDisplay.PracticeName
                btnPracticeSelection.Attributes("DataValueField") = bankAccountNo
                btnPracticeSelection.Attributes("PracticeDisplaySeq") = practiceDisplay.PracticeID

                ' CRE20-023  (Immu record) [Start][Raiman]
                btnPracticeSelection.Attributes("PracticeDisplayText") = String.Format("{0} ({1}) ", strPracticeName, practiceDisplay.PracticeID)
                btnPracticeSelection.Attributes("blnShowPopUp") = blnIsContainCovid19Scheme
                ' CRE20-023  (Immu record) [End][Raiman]

                AddHandler btnPracticeSelection.Click, AddressOf btnPracticeSelection_click

                tableCell = New TableCell
                tableCell.Width = 70
                tableCell.BorderWidth = 1
                tableCell.BorderStyle = WebControls.BorderStyle.Solid
                tableCell.BorderColor = Drawing.Color.Black

                tableCell.HorizontalAlign = HorizontalAlign.Center
                tableCell.VerticalAlign = VerticalAlign.Top
                tableCell.Controls.Add(btnPracticeSelection)


                tableRow.Cells.Add(tableCell)
                tblPractice.Rows.Add(tableRow)


                practiceIndex += 1
            Next

        End If

    End Sub
    ' CRE20-0XX (HA Scheme) [End][Winnie]



End Class
