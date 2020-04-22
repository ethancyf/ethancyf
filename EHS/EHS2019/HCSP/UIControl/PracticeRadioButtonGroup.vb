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

    'Events
    Public Event PracticeSelected(ByVal strPracticeName As String, ByVal strBankAcctNo As String, ByVal intBankAccountDisplaySeqas As Integer, ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

    Private Sub PracticeRadioButtonGroup_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Dim table As Table
        'Dim tableRow As TableRow
        'Dim tableCell As TableCell

        'table = New Table
        'table.CellPadding = 0
        'table.CellSpacing = 0
        'table.Style.Item("width") = "100%"

        'tableRow = New TableRow()
        'Dim headerLabel As Label = New Label()
        'headerLabel.Text = "Practice Radio Button Group"
        'tableCell = New TableCell
        'tableCell.CssClass = Me._strHeaderCss
        'tableCell.Controls.Add(headerLabel)

        'tableRow.Cells.Add(tableCell)
        'table.Rows.Add(tableRow)

        ''Create Header space
        'tableCell = New TableCell
        'tableCell.Height = 15
        'tableRow = New TableRow()
        'tableRow.Cells.Add(tableCell)
        'table.Rows.Add(tableRow)
        'Me.Controls.Add(table)
        'Dim udtUserAC As UserACModel = New UserACModel
        'udtUserAC = UserACBLL.GetUserAC

        'If Not udtUserAC Is Nothing Then

        '    If udtUserAC.UserType = Common.Component.SPAcctType.ServiceProvider Then
        '        Dim serviceProvider As ServiceProviderModel
        '        serviceProvider = CType(udtUserAC, ServiceProviderModel)
        '        Me.BuildRadioButtonGroup(serviceProvider.PracticeList)

        '    ElseIf udtUserAC.UserType = Common.Component.SPAcctType.DataEntryAcct Then
        '        Dim dataEntry As DataEntryUserModel
        '        dataEntry = CType(udtUserAC, DataEntryUserModel)
        '        Me.BuildRadioButtonGroup(dataEntry.PracticeList)
        '    End If

        'Else
        'End If
    End Sub

    Public Sub BuildRadioButtonGroup(ByVal practiceDisplays As BLL.PracticeDisplayModelCollection, ByVal practices As PracticeModelCollection, ByVal schemeInfoList As SchemeInformationModelCollection, ByVal strlanguage As String, ByVal displayMode As DisplayMode)

        If Not practiceDisplays Is Nothing Then
            Dim formatter As Formatter = New Formatter()
            Dim table As Table
            Dim tableRow As TableRow
            Dim tableCell As TableCell
            Dim panel As Panel
            Dim contentWidth As Integer = Me.PracticeTabelWidth - 20

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

            Dim tableInnerPractice As Table
            Dim tableInnerPracticeRow As TableRow
            Dim tableInnerPracticeCell As TableCell
            Dim label As Label
            Dim label2 As Label
            Dim bankAccountNo As String
            Dim practiceIndex As Integer = 0
            Dim udtSchemeClaimBLL As SchemeClaimBLL = New SchemeClaimBLL()
            Dim udtSchemeClaimModelCollection As SchemeClaimModelCollection = Nothing

            Dim udtGF As New Common.ComFunction.GeneralFunction()
            Dim dtmDate As DateTime = udtGF.GetSystemDateTime()

            For Each practiceDisplay As BLL.PracticeDisplayModel In practiceDisplays
                ' CRE11-024-01 HCVS Pilot Extension Part 1 [Start]
                ' -----------------------------------------------------------------------------------------
                ' Filter practice if profession is not available for claim
                If Not practiceDisplay.Profession.IsClaimPeriod(dtmDate) Then Continue For
                ' CRE11-024-01 HCVS Pilot Extension Part 1 [End]

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

                udtSchemeClaimModelCollection = udtSchemeClaimBLL.searchValidClaimPeriodSchemeClaimByPracticeSchemeInfoSubsidizeCode(practices(practiceDisplay.PracticeID).PracticeSchemeInfoList, schemeInfoList, dtmDate)

                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
                ' --------------------------------------------------------------------------------------
                udtSchemeClaimModelCollection = udtSchemeClaimModelCollection.FilterWithoutReadonly()
                ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

                ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
                udtSchemeClaimModelCollection = udtSchemeClaimModelCollection.FilterByHCSPSubPlatform(DirectCast(Me.Page, BasePage).SubPlatform)
                ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

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
                    tableInnerPracticeCell.Controls.Add(schemeLabel)

                    tableInnerPracticeRow = New TableRow()
                    tableInnerPracticeRow.Cells.Add(tableInnerPracticeCell)
                    tableInnerPractice.Rows.Add(tableInnerPracticeRow)

                    'CRE16-002 (Revamp VSS) [Start][Chris YIM]
                    '-----------------------------------------------------------------------------------------
                    Dim udtConvertedSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeEnrolFromSchemeClaimCode(udtSchemeClaimModel.SchemeCode)

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

                Next

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
                btnPracticeSelection.Attributes("DataValueField") = practiceDisplay.BankAccountNo
                btnPracticeSelection.Attributes("PracticeDisplaySeq") = practiceDisplay.PracticeID
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
                table.Rows.Add(tableRow)

                practiceIndex += 1
            Next

            panel.Controls.Add(table)
            Me.Controls.Add(panel)
        End If
    End Sub

    Private Sub btnPracticeSelection_click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim imageButton As ImageButton = CType(sender, ImageButton)
        Dim strPracticeName As String = imageButton.Attributes("DataTextField")
        Dim strBankAcctNo As String = imageButton.Attributes("DataValueField")
        Dim intBankAccountDisplaySeq As Integer = CType(imageButton.Attributes("PracticeDisplaySeq"), Integer)

        RaiseEvent PracticeSelected(strPracticeName, strBankAcctNo, intBankAccountDisplaySeq, sender, e)
    End Sub

    Private Sub btnClose_click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Return
    End Sub

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

    'Private Function GetSelectedValue() As String

    '    Dim table As Table = CType(Me.Controls(0), Table)

    '    For Each tableRow As TableRow In table.Rows
    '        For Each tableCell As TableCell In tableRow.Cells
    '            For Each control As Control In tableCell.Controls
    '                If TypeOf control Is RadioButton Then
    '                    Dim radioButton As RadioButton = CType(control, RadioButton)
    '                    If radioButton.Checked Then
    '                        Return radioButton.Attributes("value").ToString()
    '                    End If
    '                End If
    '            Next
    '        Next
    '    Next

    '    Return String.Empty
    'End Function

    'Private Function GetSelectedText() As String

    '    Dim table As Table = CType(Me.Controls(0), Table)

    '    For Each tableRow As TableRow In table.Rows
    '        For Each tableCell As TableCell In tableRow.Cells
    '            For Each control As Control In tableCell.Controls
    '                If TypeOf control Is RadioButton Then
    '                    Dim radioButton As RadioButton = CType(control, RadioButton)
    '                    If radioButton.Checked Then
    '                        Return radioButton.Text
    '                    End If
    '                End If
    '            Next
    '        Next
    '    Next

    '    Return String.Empty
    'End Function


    'Public Property AutoPostBack() As Boolean
    '    Get
    '        Return Me._blnAutoPostBack
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me._blnAutoPostBack = value
    '    End Set
    'End Property

    'Public Property SPSessionName() As String
    '    Get
    '        Return Me._strSPSessionName
    '    End Get
    '    Set(ByVal value As String)
    '        Me._strSPSessionName = value
    '    End Set
    'End Property

    'Public Property SelectedValue() As String
    '    Get
    '        Return Me.GetSelectedValue()
    '    End Get
    '    Set(ByVal value As String)
    '        Me._strSelectedValue = value
    '    End Set
    'End Property

    'Public Property SelectedIndex() As Integer
    '    Get
    '        Return Me._intSelectedIndex
    '    End Get
    '    Set(ByVal value As Integer)
    '        Me._intSelectedIndex = value
    '    End Set
    'End Property

    'Public Property SelectedText() As String
    '    Get
    '        Return Me.GetSelectedText()
    '    End Get
    '    Set(ByVal value As String)
    '        Me._strSelectedText = value
    '    End Set
    'End Property
End Class
