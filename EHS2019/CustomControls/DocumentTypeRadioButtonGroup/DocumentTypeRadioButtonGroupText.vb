Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Text
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports Common.Component
Imports Common.Component.DocType

Public Class DocumentTypeRadioButtonGroupText
    Inherits System.Web.UI.WebControls.WebControl

    'Events
    Public Event CheckedChanged(ByVal sender As Object, ByVal documentTypeRadioButtonGroupArgs As DocumentTypeRadioButtonGroupArgs)

#Region "Constants"

    Public Enum FilterDocCode
        None
        VaccinationRecordEnquriySearch
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
    Private _intRow As Integer = 1
    Private _blnAutoPostBack As Boolean
    Private _strScheme As String
    Private _strPopularDocType As String
    Private _blnShowAll As Boolean

    Private _enumFilterDocCode As FilterDocCode = FilterDocCode.None

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

    Public Property ShowAll() As Boolean
        Get
            Return Me._blnShowAll
        End Get
        Set(ByVal value As Boolean)
            Me._blnShowAll = value
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

    Public Function ContainValue(ByVal strValue As String) As Boolean

        Dim blnResult As Boolean = False

        Dim table As Table = CType(Me.Controls(0), Table)
        If Me._strDocumentTypes Is Nothing OrElse Me._strDocumentTypes.Count = 0 Then
            Me.Build()
        Else
            Me.BuildRadioButtonGroup()
        End If

        Dim documentTypeIndex As Integer = 0
        For documentTypeIndex = 0 To Me._strDocumentTypes.Count
            Dim control As Control = Me.FindControl(String.Format("DocumentTypeRadioButton_{0}", documentTypeIndex))
            If Not control Is Nothing Then
                Dim radioButton As RadioButton = CType(control, RadioButton)
                If radioButton.Attributes("value").ToString() = strValue Then
                    blnResult = True
                    Exit For
                End If
            End If

        Next

        Return blnResult

    End Function

    Private Function GetSelectedValue() As String
        'Only one control in me.controls
        Dim table As Table = CType(Me.Controls(0), Table)
        If Me._strDocumentTypes Is Nothing OrElse Me._strDocumentTypes.Count = 0 Then
            Me.Build()
        Else
            Me.BuildRadioButtonGroup()
        End If

        Dim documentTypeIndex As Integer = 0
        For documentTypeIndex = 0 To Me._strDocumentTypes.Count
            Dim control As Control = Me.FindControl(String.Format("DocumentTypeRadioButton_{0}", documentTypeIndex))
            If Not control Is Nothing Then
                Dim radioButton As RadioButton = CType(control, RadioButton)
                If radioButton.Checked Then
                    Me.Attributes("SelectedValue") = radioButton.Attributes("value").ToString()
                    Return Me.Attributes("SelectedValue")
                End If
            End If

        Next

        Return String.Empty
    End Function

    Public Property SelectedValue() As String
        Get
            Return Me.GetSelectedValue()
        End Get
        Set(ByVal value As String)
            Me._strSelectedValue = value
            Me.Attributes("SelectedValue") = value
            CheckSelectedValue()
        End Set
    End Property

    Public ReadOnly Property PopularDocType() As String
        Get
            Return Me._strPopularDocType
        End Get
    End Property

    Private Sub CheckSelectedValue()
        If Not Me._strDocumentTypes Is Nothing Then
            Dim documentTypeIndex As Integer = 0
            For documentTypeIndex = 0 To Me._strDocumentTypes.Count
                Dim control As Control = Me.FindControl(String.Format("DocumentTypeRadioButton_{0}", documentTypeIndex))
                If Not control Is Nothing Then
                    ' Clear selection first
                    Dim radioButton As RadioButton = CType(control, RadioButton)
                    radioButton.Checked = False
                End If
            Next

            For documentTypeIndex = 0 To Me._strDocumentTypes.Count
                Dim control As Control = Me.FindControl(String.Format("DocumentTypeRadioButton_{0}", documentTypeIndex))
                If Not control Is Nothing Then
                    Dim radioButton As RadioButton = CType(control, RadioButton)
                    If Me._strSelectedValue = radioButton.Attributes("value").ToString() Then
                        radioButton.Checked = True
                        Exit For
                    End If
                End If
            Next
        End If
    End Sub

    Private Function GetSelectedText() As String
        'Only one control in me.controls
        Dim table As Table = CType(Me.Controls(0), Table)
        Me.Build()
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

            If Me._strScheme <> Me.Attributes("Scheme") Then
                Me.Attributes("SelectedValue") = String.Empty
            End If

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

#End Region

#Region "Internal Class"


    Public Class DocumentTypeRadioButtonGroupArgs
        Inherits System.EventArgs

        Private _radioButton As RadioButton

        Public Sub New(ByVal radioButton As RadioButton)
            Me._radioButton = radioButton
        End Sub

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
        Dim radioButton As RadioButton = CType(sender, RadioButton)

        Me.Build()
        Me.SelectedValue = radioButton.Attributes("value").ToString()

        Dim documentTypeRadioButtonGroupArgs As DocumentTypeRadioButtonGroupArgs = New DocumentTypeRadioButtonGroupArgs(radioButton)

        RaiseEvent CheckedChanged(sender, documentTypeRadioButtonGroupArgs)
    End Sub
#End Region

#Region "Init Function"

    ''' <summary>
    '''  Build Document Type According to Different Status
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Build()

        Dim udtDocTypeBLL As New DocTypeBLL()
        Dim udtDocTypeModelList As DocTypeModelCollection = udtDocTypeBLL.getAllDocType()

        Select Case _enumFilterDocCode
            Case FilterDocCode.VaccinationRecordEnquriySearch
                udtDocTypeModelList = udtDocTypeModelList.FilterForVaccinationRecordEnquriySearch()

            Case FilterDocCode.None
                'Nothing to do

        End Select

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
            If Me._blnShowAll Then

                documentInfo.IsEnable = True
            End If

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
            documentInfo = Me._strDocumentTypes.Item(udtSchemeDocTypeModel.DocCode.Trim())
            If Not documentInfo Is Nothing Then
                documentInfo.IsEnable = True
                If udtSchemeDocTypeModel.IsMajorDoc Then
                    strPopularDoc = udtSchemeDocTypeModel.DocCode
                End If
            End If
        Next

        If Me._strSelectedValue Is Nothing OrElse Me._strSelectedValue.Trim() = "" Then
            'Dim a As String = Me.SelectedValue()
            ' UI Follow up to Rebind the Claim Search Control When default document is programatically select
            Me._strPopularDocType = strPopularDoc
            'Me._strSelectedValue = strPopularDoc
        End If
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

        'Add Header-------------------------------------------------------------------
        If Not String.IsNullOrEmpty(_strHeaderText) Then
            Dim headerLabel As Label = New Label()
            headerLabel.Text = Me._strHeaderText
            headerLabel.CssClass = Me._strHeaderTextCss

            tableCell = New TableCell()
            tableCell.VerticalAlign = VerticalAlign.Top
            tableCell.ColumnSpan = Me._intRow
            tableCell.CssClass = Me._strHeaderCss
            tableCell.Controls.Add(headerLabel)

            tableRow = New TableRow()
            tableRow.Cells.Add(tableCell)
            table.Rows.Add(tableRow)
        End If
        '------------------------------------------------------------------------------

        'Radio Buttons-----------------------------------------------------------------
        Dim practiceIndex As Integer = 0
        For Each docType As DocumentInfo In Me._strDocumentTypes.Values
            ' only include enabled doc type
            If docType.IsEnable Then

                If practiceIndex Mod Me._intRow = 0 Then
                    tableRow = New TableRow()
                End If

                Dim innerTable As Table = New Table
                Dim innerRow As TableRow = New TableRow
                Dim innerCell As TableCell = New TableCell

                'Add inner Radio button into cell--------------------------------------------------------------
                radioButton = New RadioButton
                radioButton.Width = Unit.Pixel(240)
                radioButton.ID = String.Format("DocumentTypeRadioButton_{0}", practiceIndex)
                radioButton.GroupName = "DocumentTypeRadioButton"
                radioButton.Attributes("value") = docType.DocCode.Trim()
                radioButton.AutoPostBack = Me._blnAutoPostBack
                radioButton.Checked = False
                'label.Text = udtDocumentInfo.DocName
                Me.SetControlDocumentName(radioButton, docType)

                'Set selected Value. if value not assiged, no radiobutton checked
                If Not String.IsNullOrEmpty(Me.Attributes("SelectedValue")) Then
                    If docType.DocCode.Trim() = Me.Attributes("SelectedValue").Trim() Then
                        radioButton.Checked = True
                    End If
                Else
                    If Me._blnShowAll Then
                        If docType.DocCode.Trim() = DocTypeModel.DocTypeCode.HKIC Then
                            radioButton.Checked = True
                        End If
                    Else
                        If docType.DocCode.Trim() = Me._strPopularDocType.Trim() Then
                            radioButton.Checked = True
                        End If

                    End If

                End If

                AddHandler radioButton.CheckedChanged, AddressOf radioButton_Click

                '-----------------------------------------------------------------------------------------------
                'add RadioButton into cell
                innerCell.VerticalAlign = VerticalAlign.Top
                innerCell.CssClass = _strRadioButtonLabelTextCss
                innerCell.Controls.Add(radioButton)
                innerRow.Cells.Add(innerCell)
                innerTable.Rows.Add(innerRow)
                '-----------------------------------------------------------------------------------------------
                tableCell = New TableCell
                tableCell.Controls.Add(innerTable)
                tableRow.Cells.Add(tableCell)
                table.Rows.Add(tableRow)

                practiceIndex += 1
            End If
        Next
        '------------------------------------------------------------------------------
        Me.Controls.Add(table)

    End Sub
#End Region

    Private Sub SetControlDocumentName(ByRef ctrl As RadioButton, ByRef udtDocumentInfo As DocumentInfo)
        If HttpContext.Current.Session("Language").ToString().ToLower() = CultureLanguage.TradChinese.ToLower() Then
            ctrl.Text = udtDocumentInfo.DocNameChi
        Else
            ctrl.Text = udtDocumentInfo.DocName
        End If
    End Sub

End Class
