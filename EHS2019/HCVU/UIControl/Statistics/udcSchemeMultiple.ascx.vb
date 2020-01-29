Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Component.HCVUUser
Imports Common.Component.Profession

Partial Public Class udcSchemeMultiple
    Inherits StatisticsCriteriaUC

#Region "Session and Const"

    Private Class VS
        Public Const Scheme As String = "Scheme"
    End Class

    Public Class Field
        Public Const Scheme As String = "Scheme"
        Public Const SchemeLabelWidth As String = "SchemeLabelWidth"

    End Class

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim blnAtLeastOneSelected As Boolean = False

        For Each li As ListItem In chkScheme.Items
            If li.Selected Then
                blnAtLeastOneSelected = True
                Exit For
            End If
        Next

        If blnAtLeastOneSelected Then
            tdValue.Attributes("class") = "SchemeMultiple_ValueWidth"
            tdValue03.Visible = True
        Else
            tdValue.Attributes("class") = "SchemeMultiple_ValueWidth_NoSelection"
            tdValue03.Visible = False
        End If

    End Sub

    Private Sub rbtnScheme_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnScheme.SelectedIndexChanged
        Select Case rbtnScheme.SelectedIndex
            Case MultiSelectionTypeEnum.Any
                If chkScheme.Items.Count > 0 Then
                    SetSchemeSelectionToAny()

                End If

            Case MultiSelectionTypeEnum.Specific
                If chkScheme.Items.Count > 0 Then
                    For Each boxItem As ListItem In chkScheme.Items
                        boxItem.Selected = False
                    Next

                    popupScheme.Show()

                    If chkScheme.Items.Count <= 4 Then
                        chkScheme.RepeatColumns = 1
                    Else
                        chkScheme.RepeatColumns = 2
                    End If

                End If

            Case MultiSelectionTypeEnum.NoSelection
                ' Edit
        End Select
    End Sub

#End Region

#Region "Popup Function"

    Protected Sub imgAddScheme_Click(sender As Object, e As ImageClickEventArgs)
        If chkScheme.Items.Count > 0 Then
            popupScheme.Show()

            If chkScheme.Items.Count <= 4 Then
                chkScheme.RepeatColumns = 1
            Else
                chkScheme.RepeatColumns = 2
            End If

        End If
    End Sub

    Protected Sub ibtnSchemePopupOK_Click(sender As Object, e As ImageClickEventArgs)
        ' Add selected value to list
        AddSchemeIntoList()
    End Sub

    Protected Sub ibtnSchemePopupCancel_Click(sender As Object, e As ImageClickEventArgs)
        Dim items As New Dictionary(Of String, Boolean)
        If Not ViewState(VS.Scheme) Is Nothing Then
            items = CType(ViewState(VS.Scheme), Dictionary(Of String, Boolean))
        End If

        If items.Count = 0 Then
            ' First cancel, no value is selected before
            For Each boxItem As ListItem In chkScheme.Items
                boxItem.Selected = False
            Next

        Else
            ' Have value already
            For Each boxItem As ListItem In chkScheme.Items
                If items.ContainsKey(boxItem.Value.ToString.Trim) Then
                    boxItem.Selected = items(boxItem.Value.ToString.Trim)
                End If
            Next

        End If

        ' Scheme checkboxlist item

        Dim strChkBoxItemString As String = ""
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkScheme.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        ' Have selected items (if any, no need to do counting)
        If rbtnScheme.SelectedIndex <> MultiSelectionTypeEnum.Any Then
            If intIsSelectedCount > 0 Then
                rbtnScheme.SelectedIndex = MultiSelectionTypeEnum.Specific
            Else
                rbtnScheme.SelectedIndex = MultiSelectionTypeEnum.NoSelection
            End If

        End If

        lblAddSchemeDisplay.Text = strChkBoxItemString

    End Sub

#End Region

#Region "Implement IStatisticsCriteriaUC"

    Public Overrides Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
        ' Control setting file
        '_dicSetting = dicSetting
        'SetErrorComponentVisibility(False)

        ' Init check box list
        BuildSchemeMultipleComponent(chkScheme, Field.Scheme)

        MyBase.Build(dicSetting)

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        SetErrorComponentVisibility(False)

        ' Scheme
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                ' Check checkbox list (Scheme)
                If CheckSchemeHasSelectedValue() = False Then
                    Dim strProfessionText As String = String.Empty

                    If IsExistValue(Field.Scheme, FieldSetting.DescResource) Then
                        strProfessionText = Me.GetGlobalResourceObject("Text", GetSetting(Field.Scheme, FieldSetting.DescResource))
                    End If

                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367))
                    lstErrorParam1.Add(strProfessionText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorScheme.Visible = True
                End If
            End If
        End If

    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strSchemeValue As String = String.Empty

        ' Scheme
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                Dim strSchemeLabel As String = lblScheme.Text.Trim
                Dim strChkBoxItemString As String = ""

                Select Case rbtnScheme.SelectedIndex
                    Case MultiSelectionTypeEnum.Any
                        strChkBoxItemString += "Any"
                        strSchemeValue += strChkBoxItemString

                    Case MultiSelectionTypeEnum.Specific
                        ' Profession checkboxlist item
                        Dim lstSelected As New List(Of String)

                        For Each boxItem As ListItem In chkScheme.Items
                            If boxItem.Selected = True Then
                                lstSelected.Add(boxItem.Value.Trim)
                            End If
                        Next
                        strSchemeValue = String.Join(",", lstSelected.ToArray)

                    Case Else
                        ' Nothing
                        strSchemeValue += "---"
                End Select

                udtParameterList.AddParam(strSchemeLabel, strSchemeValue)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Scheme
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                Dim strSchemeLabel As String = lblScheme.Text.Trim
                Dim strChkBoxItemString As String = ""

                Select Case rbtnScheme.SelectedIndex
                    Case MultiSelectionTypeEnum.Any
                        strChkBoxItemString += "Any"
                        udtParameterList.AddParam(strSchemeLabel, strChkBoxItemString)

                    Case MultiSelectionTypeEnum.Specific
                        ' Scheme checkboxlist item
                        Dim listParam As New ParameterObjectList(strSchemeLabel)
                        For Each boxItem As ListItem In chkScheme.Items
                            If boxItem.Selected = True Then
                                listParam.ParamValueList.Add(boxItem.Text.ToString.Trim)
                            End If
                        Next

                        udtParameterList.AddParam(listParam)

                    Case Else
                        ' Nothing
                End Select

                'udtParameterList.AddParam(strProfessionLabel, strChkBoxItemString)

            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' Scheme
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.Scheme, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty
                    strSPParamName = GetSetting(Field.Scheme, FieldSetting.SPParamName)

                    Dim lstValue As New List(Of String)

                    If rbtnScheme.SelectedIndex = MultiSelectionTypeEnum.Any Then
                        For Each li As ListItem In chkScheme.Items
                            lstValue.Add(li.Value.Trim)
                        Next

                    Else
                        For Each li As ListItem In chkScheme.Items
                            If li.Selected Then lstValue.Add(li.Value.Trim)
                        Next
                     
                    End If

                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, String.Join(",", lstValue.ToArray))

                End If

            ElseIf GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.Scheme, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.Scheme, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.Scheme, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.Scheme, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.Scheme, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        imgErrorScheme.Visible = blnVisible
    End Sub

#End Region

#Region "Supporting Function"

    Protected Overrides Sub BuildSchemeMultipleComponent(ByVal cboxListComponent As CheckBoxList, ByVal strFieldID As String)
        MyBase.BuildSchemeMultipleComponent(cboxListComponent, strFieldID)

    End Sub

    Public Overrides Sub InitControl()
        ' Set item - Scheme
        SetScheme()

    End Sub

    Private Function CheckSchemeHasSelectedValue() As Boolean
        Dim blnHasSelectedValue As Boolean = False

        If rbtnScheme.SelectedIndex = MultiSelectionTypeEnum.Any Then
            blnHasSelectedValue = True
        Else
            For Each boxItem As ListItem In chkScheme.Items
                If boxItem.Selected = True Then
                    blnHasSelectedValue = True
                    Exit For
                End If
            Next
        End If

        Return blnHasSelectedValue
    End Function

    Private Sub ResetSchemeSelection()
        BuildSchemeMultipleComponent(chkScheme, Field.Scheme)

        ViewState(VS.Scheme) = Nothing

        rbtnScheme.SelectedIndex = MultiSelectionTypeEnum.NoSelection

        ' Profession checkbox list item
        Dim strChkBoxItemString As String = String.Empty

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkScheme.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
            End If
        Next
        strChkBoxItemString += "</ul>"

        lblAddSchemeDisplay.Text = strChkBoxItemString
    End Sub

    Private Sub AddSchemeIntoList()
        Dim items As New Dictionary(Of String, Boolean)

        ' Health profession checkboxlist item
        For Each boxItem As ListItem In chkScheme.Items
            items.Add(boxItem.Value.ToString.Trim, boxItem.Selected)
        Next

        ViewState(VS.Scheme) = items

        ' Profession checkboxlist item

        Dim strChkBoxItemString As String = ""
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkScheme.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        ' Have selected items
        If intIsSelectedCount > 0 Then
            rbtnScheme.SelectedIndex = MultiSelectionTypeEnum.Specific
        Else
            rbtnScheme.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        End If

        lblAddSchemeDisplay.Text = strChkBoxItemString

    End Sub

    Private Sub SetSchemeSelectionToAny()
        'BuildProfessionComponent(ddlScheme, chkScheme, Field.Profession)
        For Each boxItem As ListItem In chkScheme.Items
            boxItem.Selected = False
        Next

        ViewState(VS.Scheme) = Nothing

        rbtnScheme.SelectedIndex = MultiSelectionTypeEnum.Any

        ' Profession checkbox list item
        Dim strChkBoxItemString As String = String.Empty

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkScheme.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
            End If
        Next
        strChkBoxItemString += "</ul>"

        lblAddSchemeDisplay.Text = strChkBoxItemString
    End Sub

#End Region

#Region "Fields Setting"

    ' Set item - Scheme
    'Private Sub SetScheme()
    '    ' Set field description
    '    If IsExistValue(Field.Scheme, FieldSetting.DescResource) Then
    '        lblScheme.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.Scheme, FieldSetting.DescResource))
    '    End If

    '    ' Set field visibility
    '    If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
    '        Select Case GetSetting(Field.Scheme, FieldSetting.Visible)
    '            Case Condition.YES
    '                panScheme.Visible = True
    '            Case Condition.NO
    '                panScheme.Visible = False
    '            Case Else
    '                panScheme.Visible = False
    '        End Select
    '    End If

    '    ' Set default value
    '    If IsExistValue(Field.Scheme, FieldSetting.DefaultValue) Then
    '        If Not GetSetting(Field.Scheme, FieldSetting.DefaultValue) = String.Empty Then

    '            Dim listItem As ListItem = ddlScheme.Items.FindByValue(GetSetting(Field.Scheme, FieldSetting.DefaultValue))
    '            If Not listItem Is Nothing Then
    '                ddlScheme.SelectedValue = listItem.Value
    '            End If

    '        End If
    '    End If

    'End Sub

    ' Set item - Health profession
    Private Sub SetScheme()
        ' Set field description
        If IsExistValue(Field.Scheme, FieldSetting.DescResource) Then
            lblScheme.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.Scheme, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            Select Case GetSetting(Field.Scheme, FieldSetting.Visible)
                Case Condition.YES
                    panScheme.Visible = True
                Case Condition.NO
                    panScheme.Visible = False
                Case Else
                    panScheme.Visible = False
            End Select
        End If

        'CRE13-003 Token Replacement [Start][Karl]
        'Set Profession label width
        If IsExistValue(Field.SchemeLabelWidth, FieldSetting.DefaultValue) Then
            If IsNumeric(GetSetting(Field.SchemeLabelWidth, FieldSetting.DefaultValue)) Then
                Me.lblScheme.Width = GetSetting(Field.SchemeLabelWidth, FieldSetting.DefaultValue)
            End If
        End If
        'CRE13-003 Token Replacement [End][Karl]

        ' Set default value
        If IsExistValue(Field.Scheme, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.Scheme, FieldSetting.DefaultValue) = String.Empty Then

                If GetSetting(Field.Scheme, FieldSetting.DefaultValue) = "ANY" Then
                    For Each listItem As ListItem In chkScheme.Items
                        listItem.Selected = True
                    Next
                Else
                    Dim strDefaultValue As String = GetSetting(Field.Scheme, FieldSetting.DefaultValue)
                    Dim valueList As String() = strDefaultValue.Split(New Char() {","c})

                    For Each valueItem As String In valueList
                        Dim listItem As ListItem = chkScheme.Items.FindByValue(valueItem)
                        If Not listItem Is Nothing Then
                            listItem.Selected = True
                        End If
                    Next

                End If

            End If
        End If

        ' Add selected value to list
        AddSchemeIntoList()

    End Sub

#End Region

End Class