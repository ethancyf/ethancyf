Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Component.HCVUUser
Imports Common.Component.Profession

Partial Public Class udcTransStatusMultiple
    Inherits StatisticsCriteriaUC

#Region "Session and Const"

    Private Class VS
        Public Const TransStatus As String = "TransStatus"
    End Class

    Public Class Field
        Public Const TransStatus As String = "TransStatus"
        Public Const TransStatusLabelWidth As String = "TransStatusLabelWidth"

    End Class

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim blnAtLeastOneSelected As Boolean = False

        For Each li As ListItem In chkTransStatus.Items
            If li.Selected Then
                blnAtLeastOneSelected = True
                Exit For
            End If
        Next

        If blnAtLeastOneSelected Then
            tdValue.Attributes("class") = "TransStatusMultiple_ValueWidth"
            tdValue03.Visible = True
        Else
            tdValue.Attributes("class") = "TransStatusMultiple_ValueWidth_NoSelection"
            tdValue03.Visible = False
        End If

    End Sub

    Private Sub rbtnTransStatus_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnTransStatus.SelectedIndexChanged
        Select Case rbtnTransStatus.SelectedIndex
            Case MultiSelectionTypeEnum.Any
                If chkTransStatus.Items.Count > 0 Then
                    SetTransStatusSelectionToAny()

                End If

            Case MultiSelectionTypeEnum.Specific
                If chkTransStatus.Items.Count > 0 Then
                    For Each boxItem As ListItem In chkTransStatus.Items
                        boxItem.Selected = False
                    Next

                    popupTransStatus.Show()

                    If chkTransStatus.Items.Count <= 4 Then
                        chkTransStatus.RepeatColumns = 1
                    Else
                        chkTransStatus.RepeatColumns = 2
                    End If

                End If

            Case MultiSelectionTypeEnum.NoSelection
                ' Edit
        End Select
    End Sub

#End Region

#Region "Popup Function"

    Protected Sub imgAddTransStatus_Click(sender As Object, e As ImageClickEventArgs)
        If chkTransStatus.Items.Count > 0 Then
            popupTransStatus.Show()

            If chkTransStatus.Items.Count <= 4 Then
                chkTransStatus.RepeatColumns = 1
            Else
                chkTransStatus.RepeatColumns = 2
            End If

        End If
    End Sub

    Protected Sub ibtnTransStatusPopupOK_Click(sender As Object, e As ImageClickEventArgs)
        ' Add selected value to list
        AddTransStatusIntoList()
    End Sub

    Protected Sub ibtnTransStatusPopupCancel_Click(sender As Object, e As ImageClickEventArgs)
        Dim items As New Dictionary(Of String, Boolean)
        If Not ViewState(VS.TransStatus) Is Nothing Then
            items = CType(ViewState(VS.TransStatus), Dictionary(Of String, Boolean))
        End If

        If items.Count = 0 Then
            ' First cancel, no value is selected before
            For Each boxItem As ListItem In chkTransStatus.Items
                boxItem.Selected = False
            Next

        Else
            ' Have value already
            For Each boxItem As ListItem In chkTransStatus.Items
                If items.ContainsKey(boxItem.Value.ToString.Trim) Then
                    boxItem.Selected = items(boxItem.Value.ToString.Trim)
                End If
            Next

        End If

        ' TransStatus checkboxlist item

        Dim strChkBoxItemString As String = ""
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkTransStatus.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        ' Have selected items (if any, no need to do counting)
        If rbtnTransStatus.SelectedIndex <> MultiSelectionTypeEnum.Any Then
            If intIsSelectedCount > 0 Then
                rbtnTransStatus.SelectedIndex = MultiSelectionTypeEnum.Specific
            Else
                rbtnTransStatus.SelectedIndex = MultiSelectionTypeEnum.NoSelection
            End If

        End If

        lblAddTransStatusDisplay.Text = strChkBoxItemString

    End Sub

#End Region

#Region "Implement IStatisticsCriteriaUC"

    Public Overrides Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
        ' Control setting file
        '_dicSetting = dicSetting
        'SetErrorComponentVisibility(False)

        ' Init check box list
        BuildTransStatusMultipleComponent(chkTransStatus, Field.TransStatus)

        MyBase.Build(dicSetting)

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        SetErrorComponentVisibility(False)

        ' TransStatus
        If IsExistValue(Field.TransStatus, FieldSetting.Visible) Then
            If GetSetting(Field.TransStatus, FieldSetting.Visible) = Condition.YES Then
                ' Check checkbox list (TransStatus)
                If CheckTransStatusHasSelectedValue() = False Then
                    Dim strProfessionText As String = String.Empty

                    If IsExistValue(Field.TransStatus, FieldSetting.DescResource) Then
                        strProfessionText = Me.GetGlobalResourceObject("Text", GetSetting(Field.TransStatus, FieldSetting.DescResource))
                    End If

                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367))
                    lstErrorParam1.Add(strProfessionText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorTransStatus.Visible = True
                End If
            End If
        End If

    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strTransStatusValue As String = String.Empty

        ' TransStatus
        If IsExistValue(Field.TransStatus, FieldSetting.Visible) Then
            If GetSetting(Field.TransStatus, FieldSetting.Visible) = Condition.YES Then
                Dim strTransStatusLabel As String = lblTransStatus.Text.Trim
                Dim strChkBoxItemString As String = ""

                Select Case rbtnTransStatus.SelectedIndex
                    Case MultiSelectionTypeEnum.Any
                        strChkBoxItemString += "Any"
                        strTransStatusValue += strChkBoxItemString

                    Case MultiSelectionTypeEnum.Specific
                        ' Profession checkboxlist item
                        Dim lstSelected As New List(Of String)

                        For Each boxItem As ListItem In chkTransStatus.Items
                            If boxItem.Selected = True Then
                                lstSelected.Add(boxItem.Value.Trim)
                            End If
                        Next
                        strTransStatusValue = String.Join(",", lstSelected.ToArray)

                    Case Else
                        ' Nothing
                        strTransStatusValue += "---"
                End Select

                udtParameterList.AddParam(strTransStatusLabel, strTransStatusValue)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Health Profession
        If IsExistValue(Field.TransStatus, FieldSetting.Visible) Then
            If GetSetting(Field.TransStatus, FieldSetting.Visible) = Condition.YES Then
                Dim strTransStatusLabel As String = lblTransStatus.Text.Trim
                Dim strChkBoxItemString As String = ""

                Select Case rbtnTransStatus.SelectedIndex
                    Case MultiSelectionTypeEnum.Any
                        strChkBoxItemString += "Any"
                        udtParameterList.AddParam(strTransStatusLabel, strChkBoxItemString)

                    Case MultiSelectionTypeEnum.Specific
                        ' TransStatus checkboxlist item
                        Dim listParam As New ParameterObjectList(strTransStatusLabel)
                        For Each boxItem As ListItem In chkTransStatus.Items
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

        ' TransStatus
        If IsExistValue(Field.TransStatus, FieldSetting.Visible) Then
            If GetSetting(Field.TransStatus, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.TransStatus, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty
                    strSPParamName = GetSetting(Field.TransStatus, FieldSetting.SPParamName)

                    Dim lstValue As New List(Of String)

                    If rbtnTransStatus.SelectedIndex = MultiSelectionTypeEnum.Any Then
                        For Each li As ListItem In chkTransStatus.Items
                            lstValue.Add(li.Value.Trim)
                        Next

                    Else
                        For Each li As ListItem In chkTransStatus.Items
                            If li.Selected Then lstValue.Add(li.Value.Trim)
                        Next

                    End If

                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, String.Join(",", lstValue.ToArray))

                End If

            ElseIf GetSetting(Field.TransStatus, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.TransStatus, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.TransStatus, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.TransStatus, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.TransStatus, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.TransStatus, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        imgErrorTransStatus.Visible = blnVisible
    End Sub

#End Region

#Region "Supporting Function"

    Protected Overrides Sub BuildTransStatusMultipleComponent(ByVal cboxListComponent As CheckBoxList, ByVal strFieldID As String)
        MyBase.BuildTransStatusMultipleComponent(cboxListComponent, strFieldID)

    End Sub

    Public Overrides Sub InitControl()
        ' Set item - TransStatus
        SetTransStatus()

    End Sub

    Private Function CheckTransStatusHasSelectedValue() As Boolean
        Dim blnHasSelectedValue As Boolean = False

        If rbtnTransStatus.SelectedIndex = MultiSelectionTypeEnum.Any Then
            blnHasSelectedValue = True
        Else
            For Each boxItem As ListItem In chkTransStatus.Items
                If boxItem.Selected = True Then
                    blnHasSelectedValue = True
                    Exit For
                End If
            Next
        End If

        Return blnHasSelectedValue
    End Function

    Private Sub ResetTransStatusSelection()
        BuildTransStatusMultipleComponent(chkTransStatus, Field.TransStatus)

        ViewState(VS.TransStatus) = Nothing

        rbtnTransStatus.SelectedIndex = MultiSelectionTypeEnum.NoSelection

        ' Profession checkbox list item
        Dim strChkBoxItemString As String = String.Empty

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkTransStatus.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
            End If
        Next
        strChkBoxItemString += "</ul>"

        lblAddTransStatusDisplay.Text = strChkBoxItemString
    End Sub

    Private Sub AddTransStatusIntoList()
        Dim items As New Dictionary(Of String, Boolean)

        ' Health profession checkboxlist item
        For Each boxItem As ListItem In chkTransStatus.Items
            items.Add(boxItem.Value.ToString.Trim, boxItem.Selected)
        Next

        ViewState(VS.TransStatus) = items

        ' Profession checkboxlist item

        Dim strChkBoxItemString As String = ""
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkTransStatus.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        ' Have selected items
        If intIsSelectedCount > 0 Then
            rbtnTransStatus.SelectedIndex = MultiSelectionTypeEnum.Specific
        Else
            rbtnTransStatus.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        End If

        lblAddTransStatusDisplay.Text = strChkBoxItemString

    End Sub

    Private Sub SetTransStatusSelectionToAny()

        For Each boxItem As ListItem In chkTransStatus.Items
            boxItem.Selected = False
        Next

        ViewState(VS.TransStatus) = Nothing

        rbtnTransStatus.SelectedIndex = MultiSelectionTypeEnum.Any

        ' Profession checkbox list item
        Dim strChkBoxItemString As String = String.Empty

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkTransStatus.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
            End If
        Next
        strChkBoxItemString += "</ul>"

        lblAddTransStatusDisplay.Text = strChkBoxItemString
    End Sub

#End Region

#Region "Fields Setting"

    Private Sub SetTransStatus()
        ' Set field description
        If IsExistValue(Field.TransStatus, FieldSetting.DescResource) Then
            lblTransStatus.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.TransStatus, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.TransStatus, FieldSetting.Visible) Then
            Select Case GetSetting(Field.TransStatus, FieldSetting.Visible)
                Case Condition.YES
                    panTransStatus.Visible = True
                Case Condition.NO
                    panTransStatus.Visible = False
                Case Else
                    panTransStatus.Visible = False
            End Select
        End If

        'CRE13-003 Token Replacement [Start][Karl]
        'Set Profession label width
        If IsExistValue(Field.TransStatusLabelWidth, FieldSetting.DefaultValue) Then
            If IsNumeric(GetSetting(Field.TransStatusLabelWidth, FieldSetting.DefaultValue)) Then
                Me.lblTransStatus.Width = GetSetting(Field.TransStatusLabelWidth, FieldSetting.DefaultValue)
            End If
        End If
        'CRE13-003 Token Replacement [End][Karl]

        ' Set default value
        If IsExistValue(Field.TransStatus, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.TransStatus, FieldSetting.DefaultValue) = String.Empty Then

                If GetSetting(Field.TransStatus, FieldSetting.DefaultValue) = "ANY" Then
                    For Each listItem As ListItem In chkTransStatus.Items
                        listItem.Selected = True
                    Next
                Else
                    Dim strDefaultValue As String = GetSetting(Field.TransStatus, FieldSetting.DefaultValue)
                    Dim valueList As String() = strDefaultValue.Split(New Char() {","c})

                    For Each valueItem As String In valueList
                        Dim listItem As ListItem = chkTransStatus.Items.FindByValue(valueItem)
                        If Not listItem Is Nothing Then
                            listItem.Selected = True
                        End If
                    Next

                End If

            End If
        End If

        ' Add selected value to list
        AddTransStatusIntoList()

    End Sub

#End Region

End Class