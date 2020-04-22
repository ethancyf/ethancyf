Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Component.HCVUUser
Imports Common.Component.Profession

Partial Public Class udcSchemeProf
    Inherits StatisticsCriteriaUC

#Region "Variables"

    'Private _dicSetting As Dictionary(Of String, Dictionary(Of String, String))

#End Region

#Region "Session and Const"

    Private Class VS
        Public Const Profession As String = "Profession"
    End Class

    Public Class Field
        Public Const Scheme As String = "Scheme"
        Public Const Profession As String = "Profession"
        'CRE13-003 Token Replacement [Start][Karl]
        Public Const ProfessionLabelWidth As String = "ProfessionLabelWidth"
        'CRE13-003 Token Replacement [End][Karl]

    End Class

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim blnAtLeastOneSelected As Boolean = False

        For Each li As ListItem In chkProfession.Items
            If li.Selected Then
                blnAtLeastOneSelected = True
                Exit For
            End If
        Next

        If blnAtLeastOneSelected Then
            tdValue.Attributes("class") = "HealthProfession_ValueWidth"
            tdValue03.Visible = True
        Else
            tdValue.Attributes("class") = "HealthProfession_ValueWidth_NoSelection"
            tdValue03.Visible = False
        End If

    End Sub

    Private Sub ddlScheme_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlScheme.SelectedIndexChanged
        ' All selected profession will be cleared when dropdown (scheme) selected item is changed
        ResetProfessionSelection()
    End Sub

    Private Sub rbtnProfessionType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnProfessionType.SelectedIndexChanged
        Select Case rbtnProfessionType.SelectedIndex
            Case MultiSelectionTypeEnum.Any
                If chkProfession.Items.Count > 0 Then
                    'For Each boxItem As ListItem In chkProfession.Items
                    '    boxItem.Selected = True
                    'Next

                    'popupProfession.Show()

                    'If chkProfession.Items.Count <= 4 Then
                    '    chkProfession.RepeatColumns = 1
                    'Else
                    '    chkProfession.RepeatColumns = 2
                    'End If

                    SetProfessionSelectionToAny()

                End If

            Case MultiSelectionTypeEnum.Specific
                If chkProfession.Items.Count > 0 Then
                    For Each boxItem As ListItem In chkProfession.Items
                        boxItem.Selected = False
                    Next

                    popupProfession.Show()

                    If chkProfession.Items.Count <= 4 Then
                        chkProfession.RepeatColumns = 1
                    Else
                        chkProfession.RepeatColumns = 2
                    End If

                End If

            Case MultiSelectionTypeEnum.NoSelection
                ' Edit
        End Select
    End Sub

#End Region

#Region "Popup Function"

    Public Sub ibtnAddProfession_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If chkProfession.Items.Count > 0 Then
            popupProfession.Show()

            If chkProfession.Items.Count <= 4 Then
                chkProfession.RepeatColumns = 1
            Else
                chkProfession.RepeatColumns = 2
            End If

        End If
    End Sub

    Public Sub ibtnProfessionPopupOK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' Add selected value to list
        AddProfessionIntoList()
    End Sub

    Public Sub ibtnProfessionPopupCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim items As New Dictionary(Of String, Boolean)
        If Not ViewState(VS.Profession) Is Nothing Then
            items = CType(ViewState(VS.Profession), Dictionary(Of String, Boolean))
        End If

        If items.Count = 0 Then
            ' First cancel, no value is selected before
            For Each boxItem As ListItem In chkProfession.Items
                boxItem.Selected = False
            Next

        Else
            ' Have value already
            For Each boxItem As ListItem In chkProfession.Items
                If items.ContainsKey(boxItem.Value.ToString.Trim) Then
                    boxItem.Selected = items(boxItem.Value.ToString.Trim)
                End If
            Next

        End If

        ' Profession checkboxlist item

        Dim strChkBoxItemString As String = ""
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkProfession.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        '' If all items are selected
        'If intIsSelectedCount = chkProfession.Items.Count Then
        '    rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Any
        '    strChkBoxItemString = String.Empty
        'Else
        '    ' Have selected items
        '    If intIsSelectedCount > 0 Then
        '        rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Specific
        '    Else
        '        rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        '    End If

        'End If

        ' Have selected items (if any, no need to do counting)
        If rbtnProfessionType.SelectedIndex <> MultiSelectionTypeEnum.Any Then
            If intIsSelectedCount > 0 Then
                rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Specific
            Else
                rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
            End If

        End If

        lblAddProfessionDisplay.Text = strChkBoxItemString

    End Sub

#End Region

#Region "Implement IStatisticsCriteriaUC"

    Public Overrides Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
        ' Control setting file
        '_dicSetting = dicSetting
        SetErrorComponentVisibility(False)

        ' Build scheme component
        BuildSchemeComponent(Me.ddlScheme)

        ' Init check box list
        BuildProfessionComponent(ddlScheme, chkProfession, Field.Profession)

        MyBase.Build(dicSetting)

        ' Initial control
        'InitControl()

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        SetErrorComponentVisibility(False)

        ' Scheme
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                ' Check dropdown (Scheme)
                If ddlScheme.SelectedValue.Trim = DROP_DOWN_EMPTY Then
                    Dim strSchemeText As String = String.Empty

                    If IsExistValue(Field.Scheme, FieldSetting.DescResource) Then
                        strSchemeText = Me.GetGlobalResourceObject("Text", GetSetting(Field.Scheme, FieldSetting.DescResource))
                    Else
                        strSchemeText = Me.GetGlobalResourceObject("Text", "Scheme")
                    End If

                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367))
                    lstErrorParam1.Add(strSchemeText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorScheme.Visible = True
                End If

            End If
        End If

        ' Health Profession
        If IsExistValue(Field.Profession, FieldSetting.Visible) Then
            If GetSetting(Field.Profession, FieldSetting.Visible) = Condition.YES Then
                ' Check checkbox list (Profession)
                If CheckProfessionHasSelectedValue() = False Then
                    Dim strProfessionText As String = String.Empty

                    If IsExistValue(Field.Profession, FieldSetting.DescResource) Then
                        strProfessionText = Me.GetGlobalResourceObject("Text", GetSetting(Field.Profession, FieldSetting.DescResource))
                    Else
                        strProfessionText = Me.GetGlobalResourceObject("Text", "HealthProf")
                    End If

                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367))
                    lstErrorParam1.Add(strProfessionText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorProfession.Visible = True
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
        Dim strHealthProfessionValue As String = String.Empty

        ' Scheme
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                Dim strSchemeLabel As String = lblScheme.Text.Trim
                If ddlScheme.SelectedIndex = 0 Then
                    strSchemeValue = String.Empty
                Else
                    strSchemeValue = ddlScheme.SelectedValue.Trim
                End If

                udtParameterList.AddParam(strSchemeLabel, strSchemeValue)
            End If
        End If

        ' Health Profession
        If IsExistValue(Field.Profession, FieldSetting.Visible) Then
            If GetSetting(Field.Profession, FieldSetting.Visible) = Condition.YES Then
                Dim strProfessionLabel As String = lblProfession.Text.Trim
                Dim strChkBoxItemString As String = ""

                Select Case rbtnProfessionType.SelectedIndex
                    Case MultiSelectionTypeEnum.Any
                        strChkBoxItemString += "Any"
                        strHealthProfessionValue += strChkBoxItemString

                    Case MultiSelectionTypeEnum.Specific
                        ' Profession checkboxlist item
                        Dim lstSelected As New List(Of String)

                        For Each boxItem As ListItem In chkProfession.Items
                            If boxItem.Selected = True Then
                                lstSelected.Add(boxItem.Value.Trim)
                            End If
                        Next
                        strHealthProfessionValue = String.Join(",", lstSelected.ToArray)

                    Case Else
                        ' Nothing
                        strHealthProfessionValue += "---"
                End Select

                udtParameterList.AddParam(strProfessionLabel, strHealthProfessionValue)
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
                udtParameterList.AddParam(strSchemeLabel, ddlScheme.SelectedValue.Trim)
            End If
        End If

        ' Health Profession
        If IsExistValue(Field.Profession, FieldSetting.Visible) Then
            If GetSetting(Field.Profession, FieldSetting.Visible) = Condition.YES Then
                Dim strProfessionLabel As String = lblProfession.Text.Trim
                Dim strChkBoxItemString As String = ""

                Select Case rbtnProfessionType.SelectedIndex
                    Case MultiSelectionTypeEnum.Any
                        strChkBoxItemString += "Any"
                        udtParameterList.AddParam(strProfessionLabel, strChkBoxItemString)

                    Case MultiSelectionTypeEnum.Specific
                        ' Submission method checkboxlist item
                        'strChkBoxItemString += "<ul style='padding-left: 15px; margin: 0px'>"
                        'For Each boxItem As ListItem In chkProfession.Items
                        '    If boxItem.Selected = True Then
                        '        strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                        '    End If
                        'Next
                        'strChkBoxItemString += "</ul>"

                        ' Profession checkboxlist item
                        Dim listParam As New ParameterObjectList(strProfessionLabel)
                        For Each boxItem As ListItem In chkProfession.Items
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
                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, ddlScheme.SelectedValue.Trim)
                End If

            ElseIf GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.Scheme, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.Scheme, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.Scheme, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.Scheme, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.Scheme, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        ' Health Profession
        If IsExistValue(Field.Profession, FieldSetting.Visible) Then
            If GetSetting(Field.Profession, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.Profession, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty
                    strSPParamName = GetSetting(Field.Profession, FieldSetting.SPParamName)

                    Dim lstValue As New List(Of String)

                    If rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Any Then
                        For Each li As ListItem In chkProfession.Items
                            lstValue.Add(li.Value.Trim)
                        Next

                    Else
                        For Each li As ListItem In chkProfession.Items
                            If li.Selected Then lstValue.Add(li.Value.Trim)
                        Next

                    End If

                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, String.Join(",", lstValue.ToArray))

                End If

            ElseIf GetSetting(Field.Profession, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.Profession, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.Profession, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.Profession, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.Profession, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.Profession, FieldSetting.SPParamName)
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
        imgErrorProfession.Visible = blnVisible
    End Sub

#End Region

#Region "Supporting Function"

    Protected Overrides Sub BuildSchemeComponent(ByVal ddlComponent As DropDownList)
        MyBase.BuildSchemeComponent(ddlComponent)
    End Sub

    Protected Overrides Sub BuildProfessionComponent(ByVal ddlSchemeComponent As DropDownList, ByVal cboxListComponent As CheckBoxList, ByVal strFieldID As String)
        ' 2 case
        ' Profession with scheme (Get profession data with criteria: Scheme)
        ' Profession only (Get all profession data)
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                MyBase.BuildProfessionComponent(ddlSchemeComponent, cboxListComponent, strFieldID)
            Else
                ' If scheme has default value case
                If IsExistValue(Field.Scheme, FieldSetting.DefaultValue) Then
                    If GetSetting(Field.Scheme, FieldSetting.DefaultValue) <> String.Empty Then
                        ' Case Any, bind get all profession data
                        If GetSetting(Field.Scheme, FieldSetting.DefaultValue) = "ANY" Then
                            MyBase.BuildProfessionComponent(cboxListComponent, strFieldID)
                        Else
                            ' Case other, set ddlScheme selected value and get related data
                            Dim strDefaultValue As String = GetSetting(Field.Scheme, FieldSetting.DefaultValue)
                            'Dim listItem As ListItem = chkProfession.Items.FindByValue(valueItem)
                            Dim listItem As ListItem = ddlSchemeComponent.Items.FindByValue(strDefaultValue)
                            If Not listItem Is Nothing Then
                                listItem.Selected = True
                            End If

                            MyBase.BuildProfessionComponent(ddlSchemeComponent, cboxListComponent, strFieldID)
                        End If
                    Else
                        MyBase.BuildProfessionComponent(cboxListComponent, strFieldID)
                    End If
                Else
                    MyBase.BuildProfessionComponent(cboxListComponent, strFieldID)
                End If
                ' Exit (If scheme has default value case)
            End If
            ' Exit (If Scheme visibility is Yes)
        Else
            ' No scheme.Visible, then get all profession data
            MyBase.BuildProfessionComponent(cboxListComponent, strFieldID)

        End If

    End Sub

    Public Overrides Sub InitControl()
        ' Set item - Scheme
        SetScheme()
        ' Set item - Health profession
        SetProfession()

    End Sub

    Private Function CheckProfessionHasSelectedValue() As Boolean
        Dim blnHasSelectedValue As Boolean = False

        If rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Any Then
            blnHasSelectedValue = True
        Else
            For Each boxItem As ListItem In chkProfession.Items
                If boxItem.Selected = True Then
                    blnHasSelectedValue = True
                    Exit For
                End If
            Next
        End If

        Return blnHasSelectedValue
    End Function

    Private Sub ResetProfessionSelection()
        BuildProfessionComponent(ddlScheme, chkProfession, Field.Profession)

        ViewState(VS.Profession) = Nothing

        rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.NoSelection

        ' Profession checkbox list item
        Dim strChkBoxItemString As String = String.Empty

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkProfession.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
            End If
        Next
        strChkBoxItemString += "</ul>"

        lblAddProfessionDisplay.Text = strChkBoxItemString
    End Sub

    Private Sub AddProfessionIntoList()
        Dim items As New Dictionary(Of String, Boolean)

        ' Health profession checkboxlist item
        For Each boxItem As ListItem In chkProfession.Items
            items.Add(boxItem.Value.ToString.Trim, boxItem.Selected)
        Next

        ViewState(VS.Profession) = items

        ' Profession checkboxlist item

        Dim strChkBoxItemString As String = ""
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkProfession.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        '' If all items are selected
        'If intIsSelectedCount = chkProfession.Items.Count Then
        '    rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Any
        '    strChkBoxItemString = String.Empty
        'Else
        '    ' Have selected items
        '    If intIsSelectedCount > 0 Then
        '        rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Specific
        '    Else
        '        rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        '    End If

        'End If

        ' Have selected items
        If intIsSelectedCount > 0 Then
            rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Specific
        Else
            rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        End If

        lblAddProfessionDisplay.Text = strChkBoxItemString

    End Sub

    Private Sub SetProfessionSelectionToAny()
        'BuildProfessionComponent(ddlScheme, chkProfession, Field.Profession)
        For Each boxItem As ListItem In chkProfession.Items
            boxItem.Selected = False
        Next

        ViewState(VS.Profession) = Nothing

        rbtnProfessionType.SelectedIndex = MultiSelectionTypeEnum.Any

        ' Profession checkbox list item
        Dim strChkBoxItemString As String = String.Empty

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkProfession.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
            End If
        Next
        strChkBoxItemString += "</ul>"

        lblAddProfessionDisplay.Text = strChkBoxItemString
    End Sub

#End Region

#Region "Fields Setting"

    ' Set item - Scheme
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

        ' Set default value
        If IsExistValue(Field.Scheme, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.Scheme, FieldSetting.DefaultValue) = String.Empty Then

                Dim listItem As ListItem = ddlScheme.Items.FindByValue(GetSetting(Field.Scheme, FieldSetting.DefaultValue))
                If Not listItem Is Nothing Then
                    ddlScheme.SelectedValue = listItem.Value
                End If

            End If
        End If

    End Sub

    ' Set item - Health profession
    Private Sub SetProfession()
        ' Set field description
        If IsExistValue(Field.Profession, FieldSetting.DescResource) Then
            lblProfession.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.Profession, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.Profession, FieldSetting.Visible) Then
            Select Case GetSetting(Field.Profession, FieldSetting.Visible)
                Case Condition.YES
                    panProfession.Visible = True
                Case Condition.NO
                    panProfession.Visible = False
                Case Else
                    panProfession.Visible = False
            End Select
        End If

        'CRE13-003 Token Replacement [Start][Karl]
        'Set Profession label width
        If IsExistValue(Field.ProfessionLabelWidth, FieldSetting.DefaultValue) Then
            If IsNumeric(GetSetting(Field.ProfessionLabelWidth, FieldSetting.DefaultValue)) Then
                Me.lblProfession.Width = GetSetting(Field.ProfessionLabelWidth, FieldSetting.DefaultValue)
            End If
        End If
        'CRE13-003 Token Replacement [End][Karl]

        ' Set default value
        If IsExistValue(Field.Profession, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.Profession, FieldSetting.DefaultValue) = String.Empty Then

                If GetSetting(Field.Profession, FieldSetting.DefaultValue) = "ANY" Then
                    For Each listItem As ListItem In chkProfession.Items
                        listItem.Selected = True
                    Next
                Else
                    Dim strDefaultValue As String = GetSetting(Field.Profession, FieldSetting.DefaultValue)
                    Dim valueList As String() = strDefaultValue.Split(New Char() {","c})

                    For Each valueItem As String In valueList
                        Dim listItem As ListItem = chkProfession.Items.FindByValue(valueItem)
                        If Not listItem Is Nothing Then
                            listItem.Selected = True
                        End If
                    Next

                End If

            End If
        End If

        ' Add selected value to list
        AddProfessionIntoList()

    End Sub

#End Region

End Class