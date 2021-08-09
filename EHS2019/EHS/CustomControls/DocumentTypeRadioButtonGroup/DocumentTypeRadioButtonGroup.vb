Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Common.Component
Imports Common.Component.DocType

Public Class DocumentTypeRadioButtonGroup
    Inherits System.Web.UI.WebControls.WebControl

    'Events
    Public Event CheckedChanged(ByVal sender As Object, ByVal documentTypeRadioButtonGroupArgs As DocumentTypeRadioButtonGroupArgs)
    Public Event LegendClicked(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

#Region "Constants"

    Public Enum FilterDocCode
        None
        Scheme
        Scheme_WithDisabled
        VaccinationRecordEnquriySearch
        VSS
        VSS_COVID19
        VSS_NIA_MMR
        COVID19OR
    End Enum

#End Region

#Region "Private Memeber"

    Private _strDocumentTypes As Dictionary(Of String, DocumentInfo)

    'Csss
    Private _strHeaderCss As String
    Private _strHeaderTextCss As String
    Private _strRadioButtonLabelTextCss As String

    'Text 
    Private _strHeaderText As String

    'Values
    Private _strSelectedValue As String = String.Empty
    Private _intRow As Integer = 3
    Private _blnAutoPostBack As Boolean
    Private _strScheme As String
    Private _strPopularDocType As String
    Private _blnShowLegend As Boolean = True
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private _blnSelectPopularDocType As Boolean = True
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    Private _strLegendImageURL As String
    Private _strLegendImageALT As String

    Private _udtSchemeDocTypeList As SchemeDocTypeModelCollection

    'Settings
    Private _blnShowAllSelection As Boolean = False

    Private _enumHCSPSubPlatform As EnumHCSPSubPlatform = EnumHCSPSubPlatform.NA

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private _enumFilterDocCode As FilterDocCode = FilterDocCode.None
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    ' CRE20-0022 (Immu record) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Private _blnCOVID19 As Boolean = False
    ' CRE20-0022 (Immu record) [End][Chris YIM]
#End Region

#Region "Internal Class"


    Public Class DocumentTypeRadioButtonGroupArgs
        Inherits System.EventArgs

        Private _relatedLabel As Label
        Private _radioButton As RadioButton

        Public Sub New(ByVal label As Label, ByVal radioButton As RadioButton)
            Me._relatedLabel = label
            Me._radioButton = radioButton
        End Sub

        Public ReadOnly Property RelatedLabel() As Label
            Get
                Return Me._relatedLabel
            End Get
        End Property

        Public ReadOnly Property RadioButton() As RadioButton
            Get
                Return Me._radioButton
            End Get
        End Property
    End Class

#End Region

#Region "Event"
    Protected Sub DocumentTypeRadioButtonGroup_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim table As Table
        Dim tableRow As TableRow
        Dim tableCell As TableCell

        table = New Table
        table.CellPadding = 0
        table.CellSpacing = 0
        table.Style.Item("width") = "100%"

        tableRow = New TableRow()
        Dim headerLabel As Label = New Label()
        headerLabel.Text = "Document Type Radio Button Group"
        tableCell = New TableCell
        tableCell.CssClass = Me._strHeaderCss
        tableCell.Controls.Add(headerLabel)
        tableRow.Cells.Add(tableCell)
        table.Rows.Add(tableRow)

        'Create Header space
        tableCell = New TableCell
        tableCell.Height = 15
        tableRow = New TableRow()
        tableRow.Cells.Add(tableCell)
        table.Rows.Add(tableRow)
        Me.Controls.Add(table)
    End Sub

    Protected Sub radioButton_Click(ByVal sender As Object, ByVal e As System.EventArgs)

        ' If Radio Button Check Changed, Bold / Highlight the Related Label
        ' Set Label Bold

        Dim table As Table = CType(Me.Controls(0), Table)
        Dim selectedLabel As Label = Nothing
        Dim radioButton As RadioButton = CType(sender, RadioButton)

        Dim documentTypeIndex As Integer = 0

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Me.Build(_enumFilterDocCode)
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]
        Me.SelectedValue = radioButton.Attributes("value").ToString()

        ' Clear Other Label (Bold)
        For documentTypeIndex = 0 To Me._strDocumentTypes.Count - 1
            Dim tempRadioButton As RadioButton = CType(Me.FindControl(String.Format("DocumentTypeRadioButton_{0}", documentTypeIndex)), RadioButton)
            Dim label As Label = CType(Me.FindControl(String.Format("DocumentTypeRadioButtonText_{0}", documentTypeIndex)), Label)

            tempRadioButton.Checked = False
            label.Font.Bold = False

            If radioButton.Attributes("RelatedLabel") = label.ID Then
                selectedLabel = label
                tempRadioButton.Checked = True

            End If

        Next

        ' To Be Remove
        If Not selectedLabel Is Nothing Then
            selectedLabel.Font.Bold = True
        Else
            Throw New Exception("DocumentTypeRadioButtonGroup.radioButton_Click: Selected Label Not Found")
        End If

        Dim documentTypeRadioButtonGroupArgs As DocumentTypeRadioButtonGroupArgs = New DocumentTypeRadioButtonGroupArgs(selectedLabel, radioButton)

        RaiseEvent CheckedChanged(sender, documentTypeRadioButtonGroupArgs)
    End Sub

    Protected Sub legendButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        RaiseEvent LegendClicked(sender, e)
    End Sub
#End Region

#Region "Init Function"

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    ''' <summary>
    '''  Build Document Type According to Different Status
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Build(Optional ByVal enumFilterDocCodeForVRE As FilterDocCode = FilterDocCode.None)

        Dim udtDocTypeBLL As New DocTypeBLL()
        Dim udtDocTypeModelList As DocTypeModelCollection = udtDocTypeBLL.getAllDocType()

        Me.EnableFilterDocCode = enumFilterDocCodeForVRE

        Select Case enumFilterDocCodeForVRE
            Case FilterDocCode.VaccinationRecordEnquriySearch
                udtDocTypeModelList = udtDocTypeModelList.FilterForVaccinationRecordEnquriySearch()

            Case FilterDocCode.Scheme_WithDisabled
                If Me._udtSchemeDocTypeList Is Nothing OrElse Me._udtSchemeDocTypeList.Count = 0 OrElse (Me._udtSchemeDocTypeList.Count > 0 AndAlso Me._udtSchemeDocTypeList(0).SchemeCode <> Me._strScheme.Trim()) Then
                    Me._udtSchemeDocTypeList = udtDocTypeBLL.getSchemeDocTypeByScheme(Me._strScheme.Trim())
                End If

            Case FilterDocCode.Scheme
                If Me._udtSchemeDocTypeList Is Nothing OrElse Me._udtSchemeDocTypeList.Count = 0 OrElse (Me._udtSchemeDocTypeList.Count > 0 AndAlso Me._udtSchemeDocTypeList(0).SchemeCode <> Me._strScheme.Trim()) Then
                    Me._udtSchemeDocTypeList = udtDocTypeBLL.getSchemeDocTypeByScheme(Me._strScheme.Trim())
                End If

                udtDocTypeModelList = udtDocTypeModelList.FilterBySchemeDocTypeList(_udtSchemeDocTypeList)

                udtDocTypeModelList = udtDocTypeModelList.SortByDisplaySeq()

            Case FilterDocCode.VSS_NIA_MMR
                Dim udtDocTypeHKIC As DocTypeModel = udtDocTypeModelList.Filter(DocType.DocTypeModel.DocTypeCode.HKIC)
                Dim udtDocTypeEC As DocTypeModel = udtDocTypeModelList.Filter(DocType.DocTypeModel.DocTypeCode.EC)

                Dim udtDocTypeList As New DocTypeModelCollection

                udtDocTypeList.Add(udtDocTypeHKIC)
                udtDocTypeList.Add(udtDocTypeEC)

                udtDocTypeModelList = udtDocTypeList.SortByDisplaySeq()

            Case FilterDocCode.VSS
                If Me._udtSchemeDocTypeList Is Nothing OrElse Me._udtSchemeDocTypeList.Count = 0 Then
                    Me._udtSchemeDocTypeList = udtDocTypeBLL.getSchemeDocTypeByScheme(Me._strScheme.Trim())
                End If

                udtDocTypeModelList = udtDocTypeModelList.FilterForVSSClaim()

            Case FilterDocCode.VSS_COVID19
                If Me._udtSchemeDocTypeList Is Nothing OrElse Me._udtSchemeDocTypeList.Count = 0 Then
                    Me._udtSchemeDocTypeList = udtDocTypeBLL.getSchemeDocTypeByScheme(Me._strScheme.Trim())
                End If

                udtDocTypeModelList = udtDocTypeModelList.FilterForVSSCOVID19Claim()

            Case FilterDocCode.COVID19OR
                If Me._udtSchemeDocTypeList Is Nothing OrElse Me._udtSchemeDocTypeList.Count = 0 Then
                    Me._udtSchemeDocTypeList = udtDocTypeBLL.getSchemeDocTypeByScheme(Me._strScheme.Trim())
                End If

                udtDocTypeModelList = udtDocTypeModelList.FilterBySchemeDocTypeList(_udtSchemeDocTypeList)

                udtDocTypeModelList = udtDocTypeModelList.FilterForCOVID19ORClaim()

                udtDocTypeModelList = udtDocTypeModelList.SortByDisplaySeq()

            Case FilterDocCode.None
                'Nothing to do

        End Select

        If Me.HCSPSubPlatform <> EnumHCSPSubPlatform.NA Then
            udtDocTypeModelList = udtDocTypeModelList.FilterByHCSPSubPlatform(Me.HCSPSubPlatform)
        End If

        If Not Me._strScheme Is Nothing AndAlso Me._strScheme.Trim() <> "" Then
            ' Scheme Selected
            ' Change Scheme, Cached old scheme value from Page Load 
            If Me._udtSchemeDocTypeList Is Nothing OrElse (Me._udtSchemeDocTypeList.Count > 0 AndAlso Me._udtSchemeDocTypeList(0).SchemeCode <> Me._strScheme.Trim()) Then
                Me._udtSchemeDocTypeList = udtDocTypeBLL.getSchemeDocTypeByScheme(Me._strScheme.Trim())
            End If

            ' CRE20-015 (Special Support Scheme) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If Me.HCSPSubPlatform = EnumHCSPSubPlatform.CN Then

                Dim udtDocTypeList As New DocTypeModelCollection

                'DocType(HKIC, EC, HKBC) Filter by SchemeDocType(e.g. HCVSCHN, [HKIC, EC])
                For Each udtSchemeDocType As SchemeDocTypeModel In Me._udtSchemeDocTypeList
                    Dim udtDocType As DocTypeModel = udtDocTypeModelList.Filter(udtSchemeDocType.DocCode)

                    If udtDocType IsNot Nothing Then
                        udtDocTypeList.Add(udtDocType)
                    End If

                Next

                udtDocTypeModelList = udtDocTypeList.SortByDisplaySeq()
            End If
            ' CRE20-015 (Special Support Scheme) [End][Chris YIM]

            Me.Build(udtDocTypeModelList, Me._udtSchemeDocTypeList)

        Else
            If Me._blnShowAllSelection Then
                BuildWithScheme(udtDocTypeModelList)
            Else
                ' No Scheme Selected, Disable all document type status
                Me.Build(udtDocTypeModelList)
            End If

        End If

        Me.BuildRadioButtonGroup()
    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    ''' <summary>
    '''  Build Specified Document Type 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub BuildSpecifiedDocType(ByVal udtDocTypeModelList As DocType.DocTypeModelCollection)

        Dim udtDocTypeBLL As New DocTypeBLL()

        If Not Me._strScheme Is Nothing AndAlso Me._strScheme.Trim() <> "" Then
            ' Scheme Selected
            Dim udtSchemeDocTypeList As SchemeDocTypeModelCollection = udtDocTypeBLL.getSchemeDocTypeByScheme(Me._strScheme.Trim())
            Me.Build(udtDocTypeModelList, udtSchemeDocTypeList)
        Else
            ' No Scheme Selected, Disable all document type status
            Me.Build(udtDocTypeModelList)
        End If

        Me.BuildRadioButtonGroup()
    End Sub

    ''' <summary>
    ''' Build DocType When Not Yet Select Scheme
    ''' </summary>
    ''' <param name="udtDocTypeModelList"></param>
    ''' <remarks></remarks>
    Private Sub Build(ByVal udtDocTypeModelList As DocType.DocTypeModelCollection)

        Me._strDocumentTypes = New Dictionary(Of String, DocumentInfo)
        Dim documentInfo As DocumentInfo = Nothing

        ' Init Document Type Status, set all disable
        For Each udtDocTypeModel As DocType.DocTypeModel In udtDocTypeModelList
            documentInfo = New DocumentInfo(udtDocTypeModel)
            Me._strDocumentTypes.Add(documentInfo.DocCode, documentInfo)
        Next
    End Sub

    ''' <summary>
    ''' Build DocType When Selected Scheme
    ''' </summary>
    ''' <param name="udtDocTypeModelList"></param>
    ''' <param name="udtSchemeDocTypeList"></param>
    ''' <remarks></remarks>
    Private Sub Build(ByVal udtDocTypeModelList As DocType.DocTypeModelCollection, ByVal udtSchemeDocTypeList As DocType.SchemeDocTypeModelCollection)
        Me._strDocumentTypes = New Dictionary(Of String, DocumentInfo)
        Dim documentInfo As DocumentInfo = Nothing

        ' Init Document Type Status, set all disable
        For Each udtDocTypeModel As DocType.DocTypeModel In udtDocTypeModelList
            documentInfo = New DocumentInfo(udtDocTypeModel)
            Me._strDocumentTypes.Add(documentInfo.DocCode, documentInfo)
        Next

        ' Save the Popular Doc Code
        Dim strPopularDoc As String = String.Empty

        ' Enable Document Type Status, according to Scheme Doc Type
        For Each udtSchemeDocTypeModel As DocType.SchemeDocTypeModel In udtSchemeDocTypeList
            ' CRE20-003 (Batch Upload) [Start][Chris YIM]
            ' ---------------------------------------------------------------------------------------------------------
            If Me._strDocumentTypes.ContainsKey(udtSchemeDocTypeModel.DocCode.Trim()) Then
                documentInfo = Me._strDocumentTypes.Item(udtSchemeDocTypeModel.DocCode.Trim())
                If Not documentInfo Is Nothing Then
                    documentInfo.IsEnable = True
                    If udtSchemeDocTypeModel.IsMajorDoc Then
                        strPopularDoc = udtSchemeDocTypeModel.DocCode
                    End If
                End If
            End If
            ' CRE20-003 (Batch Upload) [End][Chris YIM]

        Next

        If Me._strSelectedValue Is Nothing OrElse Me._strSelectedValue.Trim() = "" Then
            'Dim a As String = Me.SelectedValue()
            ' UI Follow up to Rebind the Claim Search Control When default document is programatically select
            Me._strPopularDocType = strPopularDoc
            'Me._strSelectedValue = strPopularDoc
        End If
    End Sub

    Private Sub BuildWithScheme(ByVal udtDocTypeModelList As DocType.DocTypeModelCollection)
        Me._strDocumentTypes = New Dictionary(Of String, DocumentInfo)
        Dim documentInfo As DocumentInfo = Nothing

        ' Init Document Type Status, set all disable
        For Each udtDocTypeModel As DocType.DocTypeModel In udtDocTypeModelList
            documentInfo = New DocumentInfo(udtDocTypeModel)
            Me._strDocumentTypes.Add(documentInfo.DocCode, documentInfo)
        Next

        For Each doctype As DocumentInfo In _strDocumentTypes.Values
            doctype.IsEnable = True
        Next


    End Sub

    Public Sub BuildWithFixedDocument(ByVal strDocCode As String)
        Me._strDocumentTypes = New Dictionary(Of String, DocumentInfo)
        Dim documentInfo As DocumentInfo = Nothing

        Dim udtDocTypeModelList As DocTypeModelCollection = (New DocTypeBLL).getAllDocType()

        If Me.HCSPSubPlatform <> EnumHCSPSubPlatform.NA Then
            udtDocTypeModelList = udtDocTypeModelList.FilterByHCSPSubPlatform(Me.HCSPSubPlatform)
        End If

        ' CRE20-0022 (Immu record) [Start][Chris YIM]
        ' ---------------------------------------------------------------------------------------------------------
        udtDocTypeModelList = udtDocTypeModelList.FilterForVaccinationRecordEnquriySearch()
        ' CRE20-0022 (Immu record) [End][Chris YIM]

        For Each udtDocType As DocTypeModel In udtDocTypeModelList
            documentInfo = New DocumentInfo(udtDocType)
            Me._strDocumentTypes.Add(documentInfo.DocCode, documentInfo)
        Next

        For Each udtDocumentInfo As DocumentInfo In Me._strDocumentTypes.Values
            If udtDocumentInfo.DocCode.Trim = strDocCode.Trim Then
                udtDocumentInfo.IsEnable = True
                Me._strPopularDocType = udtDocumentInfo.DocCode
                Exit For
            End If
        Next

        ' Auto select the only value
        Me.SelectedValue = strDocCode

        Me.BuildRadioButtonGroup()

    End Sub

#End Region

#Region "Render UI"

    Private Sub BuildRadioButtonGroup()
        Dim table As Table
        Dim tableRow As TableRow
        Dim tableCell As TableCell
        Dim radioButton As RadioButton
        Dim labelRadioButtons As Dictionary(Of Label, RadioButton) = New Dictionary(Of Label, RadioButton)

        Me.Controls.Clear()

        table = New Table
        table.CellPadding = 0
        table.CellSpacing = 0


        tableCell = New TableCell()
        tableCell.VerticalAlign = VerticalAlign.Top
        tableCell.ColumnSpan = Me._intRow
        tableCell.CssClass = Me._strHeaderCss

        'Add Header-------------------------------------------------------------------
        Dim headerLabel As Label = New Label()
        headerLabel.Text = String.Format("{0} ", Me._strHeaderText)
        headerLabel.CssClass = Me._strHeaderTextCss
        tableCell.Controls.Add(headerLabel)

        If Me._blnShowLegend Then

            Dim imageButton As ImageButton = New ImageButton()
            imageButton.ID = "imgLengeSchemCode"
            imageButton.ImageUrl = Me._strLegendImageURL
            imageButton.AlternateText = Me._strLegendImageALT
            tableCell.Controls.Add(imageButton)

            AddHandler imageButton.Click, AddressOf legendButton_Click
        End If

        tableRow = New TableRow()
        tableRow.Cells.Add(tableCell)
        table.Rows.Add(tableRow)

        '------------------------------------------------------------------------------

        ''Space Between header and radio buttons----------------------------------------
        'tableCell = New TableCell()
        'tableCell.ColumnSpan = Me._intRow
        'tableCell.Height = 5

        'tableRow = New TableRow()
        'tableRow.Cells.Add(tableCell)
        'table.Rows.Add(tableRow)
        '------------------------------------------------------------------------------

        'Radio Buttons-----------------------------------------------------------------
        Dim practiceIndex As Integer = 0
        Dim blnSelectedDocTypeExist As Boolean = False

        'Dim udtDocumentInfo As DocumentInfo = Nothing

        For Each docType As DocumentInfo In Me._strDocumentTypes.Values

            'udtDocumentInfo = Me._strDocumentTypes.Item(strDocType)

            If practiceIndex Mod Me._intRow = 0 Then
                tableRow = New TableRow()
            End If

            Dim innerTable As Table = New Table
            Dim innerRow As TableRow = New TableRow
            Dim innerCell As TableCell = New TableCell

            'Add inner Radio button into cell--------------------------------------------------------------
            radioButton = New RadioButton
            radioButton.Width = 20
            radioButton.ID = String.Format("DocumentTypeRadioButton_{0}", practiceIndex)
            radioButton.GroupName = "DocumentTypeRadioButton"
            radioButton.Attributes("value") = docType.DocCode.Trim()
            radioButton.Attributes("RelatedLabel") = String.Format("DocumentTypeRadioButtonText_{0}", practiceIndex)

            radioButton.AutoPostBack = Me._blnAutoPostBack
            radioButton.Enabled = docType.IsEnable Or Me._blnShowAllSelection
            radioButton.Checked = False

            If radioButton.Enabled Then
                'Set selected Value. if value not assiged, no radiobutton checked
                If Me.SelectedValue <> Nothing AndAlso Me.SelectedValue <> String.Empty Then
                    If docType.DocCode.Trim() = Me.SelectedValue.Trim() Then
                        radioButton.Checked = True
                        blnSelectedDocTypeExist = True
                    End If

                Else
                    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
                    ' ---------------------------------------------------------------------------------------------------------
                    If docType.DocCode.Trim() = Me._strPopularDocType.Trim() AndAlso _blnSelectPopularDocType Then
                        Me.SelectedValue = Me._strPopularDocType.Trim()
                        radioButton.Checked = True
                    End If
                    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]
                End If

            End If

            'add RadioButton into cell
            innerCell.VerticalAlign = VerticalAlign.Top
            innerCell.Controls.Add(radioButton)
            innerRow.Cells.Add(innerCell)

            AddHandler radioButton.CheckedChanged, AddressOf radioButton_Click
            '-----------------------------------------------------------------------------------------------

            'Add inner label into cell----------------------------------------------------------------------
            Dim label As Label = New Label

            'label.Text = udtDocumentInfo.DocName
            Me.SetLabelDocumentName(label, docType)

            label.ID = String.Format("DocumentTypeRadioButtonText_{0}", practiceIndex)
            label.CssClass = Me._strRadioButtonLabelTextCss
            If Not docType.IsEnable Then
                'label.ForeColor = Drawing.Color.Gray
                label.ForeColor = Drawing.Color.LightGray
            Else
                If radioButton.Checked Then
                    label.Font.Bold = True
                End If
            End If

            innerCell = New TableCell
            innerCell.Width = 295

            'add Label into Cell
            innerCell.VerticalAlign = VerticalAlign.Middle
            innerCell.Controls.Add(label)
            innerRow.Cells.Add(innerCell)
            innerTable.Rows.Add(innerRow)
            '-----------------------------------------------------------------------------------------------

            tableCell = New TableCell
            tableCell.Controls.Add(innerTable)
            tableRow.Cells.Add(tableCell)
            table.Rows.Add(tableRow)

            practiceIndex += 1

            'Add label into collection
            'reason: Client ID is not valid, Client ID will re-Gen after added in to this control
            labelRadioButtons.Add(label, radioButton)
        Next

        If Not blnSelectedDocTypeExist Then
            Me.SelectedValue = String.Empty
        End If

        If Not blnSelectedDocTypeExist AndAlso _strPopularDocType IsNot Nothing Then
            Me.SelectedValue = Me._strPopularDocType.Trim()

            Dim strLabelID As String = String.Empty

            For Each rbl As RadioButton In labelRadioButtons.Values
                If rbl.Attributes.Item("value").ToUpper.Trim = Me.SelectedValue.ToUpper.Trim Then
                    rbl.Checked = True
                    strLabelID = rbl.Attributes.Item("RelatedLabel").Trim
                    Exit For
                End If
            Next

            For Each lbl As Label In labelRadioButtons.Keys
                If lbl.ID = strLabelID Then
                    lbl.Font.Bold = True
                    Exit For
                End If
            Next
        End If

        '------------------------------------------------------------------------------
        Me.Controls.Add(table)

        'Client Event for radio Button Labels------------------------------------------
        For Each label As Label In labelRadioButtons.Keys

            ' Append a Javascript to each Radio Button:
            ' OnCheckChanged: if radio.checked = false, Set fontWeight to Normal (Not Bold)

            radioButton = labelRadioButtons.Item(label)
            If radioButton.Enabled = True Then
                If Me._blnAutoPostBack Then
                    label.Attributes("onclick") = String.Format("javascript:document.getElementById('{0}').checked = true;document.getElementById('{0}').click();", _
                                                radioButton.ClientID)
                End If

                ' label.Attributes("onclick") = String.Format("javascript:document.getElementById('{0}').checked = true;document.getElementById('{0}').document.OnCheckedChanged();getElementById('{1}').style.fontWeight = 'bold'", _
                '                                                    radioButton.ClientID, label.ClientID)
                'radioButton.Attributes("OnCheckedChanged") = String.Format("javascript:document.getElementById('{0}').style.fontWeight = 'normal';", _

            End If
        Next
        '------------------------------------------------------------------------------
    End Sub

#End Region

#Region "Property"

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

    Public Property LabelTextCss() As String
        Get
            Return Me._strRadioButtonLabelTextCss
        End Get
        Set(ByVal value As String)
            Me._strRadioButtonLabelTextCss = value
        End Set
    End Property

    Public Property Row() As Integer
        Get
            Return Me._intRow
        End Get
        Set(ByVal value As Integer)
            Me._intRow = value
        End Set
    End Property

    Public Property AutoPostBack() As Boolean
        Get
            Return Me._blnAutoPostBack
        End Get
        Set(ByVal value As Boolean)
            Me._blnAutoPostBack = value
        End Set
    End Property

    Public Property SelectedValue() As String
        Get
            If Me.Attributes("SelectedValue") Is Nothing Then
                Return String.Empty
            Else
                Return Me.Attributes("SelectedValue")
            End If
            'Return Me.GetSelectedValue()
        End Get
        Set(ByVal value As String)
            Me._strSelectedValue = value
            Me.Attributes("SelectedValue") = value
        End Set
    End Property

    Public ReadOnly Property PopularDocType() As String
        Get
            Return Me._strPopularDocType
        End Get
    End Property

    Public ReadOnly Property SelectedText() As String
        Get
            Return Me.GetSelectedText()
        End Get
    End Property

    Public Property Scheme() As String
        Get
            Return Me._strScheme
        End Get
        Set(ByVal value As String)

            Me._strScheme = value

            'If Me._strScheme <> Me.Attributes("Scheme") Then
            '    Me.Attributes("SelectedValue") = String.Empty
            'End If

            Me.Attributes("Scheme") = value
        End Set
    End Property

    Public Property HeaderText() As String
        Get
            Return Me._strHeaderText
        End Get
        Set(ByVal value As String)
            Me._strHeaderText = value
        End Set
    End Property

    Public Property LegendImageURL() As String
        Get
            Return Me._strLegendImageURL
        End Get
        Set(ByVal value As String)
            Me._strLegendImageURL = value
        End Set
    End Property

    Public Property LegendImageALT() As String
        Get
            Return Me._strLegendImageALT
        End Get
        Set(ByVal value As String)
            Me._strLegendImageALT = value
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

    Public Property SchemeDocTypeList() As SchemeDocTypeModelCollection
        Get
            Return Me._udtSchemeDocTypeList
        End Get
        Set(ByVal value As SchemeDocTypeModelCollection)
            Me._udtSchemeDocTypeList = value
        End Set
    End Property

    Public Property ShowAllSelection() As Boolean
        Get
            Return Me._blnShowAllSelection
        End Get
        Set(ByVal value As Boolean)
            _blnShowAllSelection = value
        End Set
    End Property

    Public Property HCSPSubPlatform() As EnumHCSPSubPlatform
        Get
            Return Me._enumHCSPSubPlatform
        End Get
        Set(value As EnumHCSPSubPlatform)
            Me._enumHCSPSubPlatform = value
        End Set
    End Property

    Public Property EnableFilterDocCode() As FilterDocCode
        Get
            Return Me._enumFilterDocCode
        End Get
        Set(value As FilterDocCode)
            Me._enumFilterDocCode = value
        End Set
    End Property

    Public Property SelectPopularDocType() As Boolean
        Get
            Return Me._blnSelectPopularDocType
        End Get
        Set(value As Boolean)
            Me._blnSelectPopularDocType = value
        End Set
    End Property

    Public ReadOnly Property DocumentTypeDictionary() As Dictionary(Of String, DocumentInfo)
        Get
            Return _strDocumentTypes
        End Get
    End Property


#End Region

#Region "Supported Functions"
    Private Sub SetLabelDocumentName(ByRef lblDocType As Label, ByRef udtDocumentInfo As DocumentInfo)
        If HttpContext.Current.Session("Language").ToString().ToLower() = CultureLanguage.TradChinese.ToLower() Then
            lblDocType.Text = udtDocumentInfo.DocNameChi
        ElseIf HttpContext.Current.Session("Language").ToString().ToLower() = CultureLanguage.SimpChinese.ToLower() Then
            lblDocType.Text = udtDocumentInfo.DocNameCN
        Else
            lblDocType.Text = udtDocumentInfo.DocName
        End If
    End Sub

    Private Function GetSelectedText() As String
        'Only one control in me.controls
        Dim table As Table = CType(Me.Controls(0), Table)

        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Me.Build(_enumFilterDocCode)
        ' CRE17-018-04 (New initiatives for VSS and RVP in 2018-19) [End][Chris YIM]

        Dim documentTypeIndex As Integer = 0
        For documentTypeIndex = 0 To Me._strDocumentTypes.Count
            Dim control As Control = Me.FindControl(String.Format("DocumentTypeRadioButton_{0}", documentTypeIndex))
            If Not control Is Nothing Then
                Dim radioButton As RadioButton = CType(control, RadioButton)
                If radioButton.Checked Then
                    Dim label As Label = CType(Me.FindControl(String.Format("DocumentTypeRadioButtonText_{0}", documentTypeIndex)), Label)
                    Return label.Text
                End If
            End If
        Next
        Return String.Empty
    End Function

#End Region


End Class
