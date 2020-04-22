Imports Common.Component
Imports Common.Component.PrintFormOptionValue

Partial Public Class PrintFormOptionSelection
    Inherits System.Web.UI.UserControl
    Dim staticDataBll As StaticData.StaticDataBLL
    Dim strPrintOptionTableWidth As String = "600px"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Me.lblSelectPrintOptionText.Text = Me.GetGlobalResourceObject("Text", "SelectPrintFormOption")

        Me.rbNotPrint.Text = Me.GetGlobalResourceObject("Text", "NotToPrint")
        Me.rbPrintFull.Text = Me.GetGlobalResourceObject("Text", "PrintFullVersion")
        Me.rbPrintCondensed.Text = Me.GetGlobalResourceObject("Text", "PrintCondensedVersion")

        Me.txtNotPrintRemind.Text = Me.GetGlobalResourceObject("Text", "NotToPrintRemind")
        Me.txtPrintCondensedRemind.Text = Me.GetGlobalResourceObject("Text", "PrintCondensedVersionRemind")

        Me.tblSelectPinrOption.Style("Width") = Me.strPrintOptionTableWidth
    End Sub

    'Public Sub BuildPrintFormOptionList(ByVal udtStaticData As StaticData.StaticDataModelCollection)
    '    'Me.rbPrintFormSelection.DataSource = udtStaticData
    '    'Me.rbPrintFormSelection.DataValueField = "ItemNo"
    '    'If Session("language") = CultureLanguage.TradChinese Then
    '    '    Me.rbPrintFormSelection.DataTextField = "DataValueChi"
    '    'Else
    '    '    Me.rbPrintFormSelection.DataTextField = "DataValue"
    '    'End If

    '    'Me.rbPrintFormSelection.DataBind()
    'End Sub

    Public Function getSelection() As String
        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        'If rbNotPrint.Checked Then
        If rbNotPrint.Checked AndAlso Me.rbNotPrint.Visible Then
            Return PrintFormOptionValue.PreprintForm
            'ElseIf Me.rbPrintFull.Checked Then
        ElseIf Me.rbPrintFull.Checked AndAlso Me.rbPrintFull.Visible Then
            Return PrintFormOptionValue.PrintPurposeAndConsent
            'ElseIf Me.rbPrintCondensed.Checked Then
        ElseIf Me.rbPrintCondensed.Checked AndAlso Me.rbPrintCondensed.Visible Then
            Return PrintFormOptionValue.PrintConsentOnly
        Else
            Return ""
        End If
        'CRE13-019-02 Extend HCVS to China [End][Winnie]
    End Function

    'CRE13-019-02 Extend HCVS to China [Start][Winnie]
    Public Sub setPrintOption(ByVal strAvailableVersion As String)

        Me.tbNotPrint.Visible = True
        Me.tbPrintFull.Visible = False
        Me.tbPrintCondensed.Visible = False

        Select Case strAvailableVersion
            Case PrintFormAvailableVersion.Both
                Me.tbPrintFull.Visible = True
                Me.tbPrintCondensed.Visible = True

            Case PrintFormAvailableVersion.Full
                Me.tbPrintFull.Visible = True

            Case PrintFormAvailableVersion.Condense
                Me.tbPrintCondensed.Visible = True
        End Select

    End Sub

    'CRE13-019-02 Extend HCVS to China [End][Winnie]

    Public Sub setSelectedValue(ByVal strSelectedValue As String)
        Me.rbNotPrint.Checked = False
        Me.rbPrintFull.Checked = False
        Me.rbPrintCondensed.Checked = False

        'CRE13-019-02 Extend HCVS to China [Start][Winnie]
        Select Case strSelectedValue
            Case PrintFormOptionValue.PreprintForm
                Me.rbNotPrint.Checked = True
            Case PrintFormOptionValue.PrintPurposeAndConsent
                Me.rbPrintFull.Checked = True
            Case PrintFormOptionValue.PrintConsentOnly
                Me.rbPrintCondensed.Checked = True
        End Select

        'If strSelectedValue = PrintFormOptionValue.PreprintForm AndAlso Me.tbNotPrint.Visible Then
        '    Me.rbNotPrint.Checked = True
        'ElseIf strSelectedValue = PrintFormOptionValue.PrintPurposeAndConsent AndAlso Me.tbPrintFull.Visible Then
        '    Me.rbPrintFull.Checked = True
        'ElseIf strSelectedValue = PrintFormOptionValue.PrintConsentOnly AndAlso Me.tbPrintCondensed.Visible Then
        '    Me.rbPrintCondensed.Checked = True
        'End If
        'CRE13-019-02 Extend HCVS to China [End][Winnie]

        'Me.rbPrintFormSelection.SelectedValue = strSelectedValue
    End Sub

    Public Sub setTitle(ByVal strTitleText As String)
        Me.lblSelectPrintOptionText.Text = strTitleText
    End Sub

    Public Property PrintOptionTableWidth() As String
        Get
            Return Me.strPrintOptionTableWidth
        End Get
        Set(ByVal value As String)
            Me.strPrintOptionTableWidth = value
        End Set
    End Property

End Class