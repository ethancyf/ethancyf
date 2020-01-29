Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Component.HCVUUser

Partial Public Class udcSubmissionMethod
    Inherits StatisticsCriteriaUC

#Region "Variables"

    'Private _dicSetting As Dictionary(Of String, Dictionary(Of String, String))

#End Region

#Region "Session and Consts"

    Private Class VS
        Public Const SubmissionMethod As String = "SubmissionMethod"
    End Class

    Public Class Field
        Public Const SubmissionMethod As String = "SubmissionMethod"
    End Class

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

    Private Sub rbtnSubmissionMethodType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rbtnSubmissionMethodType.SelectedIndexChanged
        Select Case rbtnSubmissionMethodType.SelectedIndex
            Case MultiSelectionTypeEnum.Any
                If chkSubmissionMethod.Items.Count > 0 Then
                    'For Each boxItem As ListItem In chkSubmissionMethod.Items
                    '    boxItem.Selected = False
                    'Next

                    'popupSubmissionMethod.Show()

                    'If chkSubmissionMethod.Items.Count <= 4 Then
                    '    chkSubmissionMethod.RepeatColumns = 1
                    'Else
                    '    chkSubmissionMethod.RepeatColumns = 2
                    'End If

                    SetSubmissionMethodSelectionToAny()

                End If

            Case MultiSelectionTypeEnum.Specific
                If chkSubmissionMethod.Items.Count > 0 Then
                    For Each boxItem As ListItem In chkSubmissionMethod.Items
                        boxItem.Selected = False
                    Next

                    popupSubmissionMethod.Show()

                    If chkSubmissionMethod.Items.Count <= 8 Then
                        chkSubmissionMethod.RepeatColumns = 1
                    Else
                        chkSubmissionMethod.RepeatColumns = 2
                    End If

                End If

            Case MultiSelectionTypeEnum.NoSelection
                ' Edit
        End Select
    End Sub

#End Region

#Region "Popup Function"

    Public Sub ibtnAddSubmissionMethod_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        If chkSubmissionMethod.Items.Count > 0 Then
            popupSubmissionMethod.Show()
        End If
    End Sub

    Public Sub ibtnSubmissionMethodPopupOK_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        ' Add selected value to list
        AddSubmissionMethodIntoList()
    End Sub

    Public Sub ibtnSubmissionMethodPopupCancel_Click(ByVal sender As System.Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim items As New Dictionary(Of String, Boolean)
        If Not ViewState(VS.SubmissionMethod) Is Nothing Then
            items = CType(ViewState(VS.SubmissionMethod), Dictionary(Of String, Boolean))
        End If

        If items.Count = 0 Then
            ' First cancel, no value is selected before
            For Each boxItem As ListItem In chkSubmissionMethod.Items
                boxItem.Selected = False
            Next

        Else
            ' Have value already
            For Each boxItem As ListItem In chkSubmissionMethod.Items
                If items.ContainsKey(boxItem.Value.ToString.Trim) Then
                    boxItem.Selected = items(boxItem.Value.ToString.Trim)
                End If
            Next

        End If

        ' Submission method checkboxlist item

        Dim strChkBoxItemString As String = ""
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkSubmissionMethod.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        '' If all items are selected
        'If intIsSelectedCount = chkSubmissionMethod.Items.Count Then
        '    rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Any
        '    strChkBoxItemString = String.Empty
        'Else
        '    ' Have selected items
        '    If intIsSelectedCount > 0 Then
        '        rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Specific
        '    Else
        '        rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        '    End If

        'End If

        ' Have selected items (if any, no need to do counting)
        If rbtnSubmissionMethodType.SelectedIndex <> MultiSelectionTypeEnum.Any Then
            If intIsSelectedCount > 0 Then
                rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Specific
            Else
                rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
            End If

        End If

        lblAddSubmissionMethodDisplay.Text = strChkBoxItemString

    End Sub

#End Region

#Region "Implement IStatisticsCriteriaUC"

    Public Overrides Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
        ' Control setting file
        '_dicSetting = dicSetting

        ' Initial control
        'InitControl()
        SetErrorSubmissionMethod(False)

        ' Init check box list
        BuildSubmissionMethodComponent(chkSubmissionMethod)
        MyBase.Build(dicSetting)

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        SetErrorSubmissionMethod(False)

        ' Submission method
        If IsExistValue(Field.SubmissionMethod, FieldSetting.Visible) Then
            If GetSetting(Field.SubmissionMethod, FieldSetting.Visible) = Condition.YES Then
                ' Check checkbox list (Submission method)
                If CheckSubmissionMethodHasSelectedValue() = False Then
                    Dim strSubmissionMethodText As String = String.Empty

                    If IsExistValue(Field.SubmissionMethod, FieldSetting.DescResource) Then
                        strSubmissionMethodText = Me.GetGlobalResourceObject("Text", GetSetting(Field.SubmissionMethod, FieldSetting.DescResource))
                    Else
                        strSubmissionMethodText = Me.GetGlobalResourceObject("Text", "SubmissionMethod")
                    End If

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                    lstErrorParam1.Add(strSubmissionMethodText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorSubmissionMethod.Visible = True
                End If
            End If
        End If


    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strSubmissionMethodValue As String = String.Empty

        If IsExistValue(Field.SubmissionMethod, FieldSetting.Visible) Then
            If GetSetting(Field.SubmissionMethod, FieldSetting.Visible) = Condition.YES Then
                Dim strSubmissionMethodLabel As String = lblSubmissionMethod.Text.Trim
                Dim strChkBoxItemString As String = String.Empty

                Select Case rbtnSubmissionMethodType.SelectedIndex
                    Case MultiSelectionTypeEnum.Any
                        strChkBoxItemString += "Any"
                        strSubmissionMethodValue += strChkBoxItemString

                    Case MultiSelectionTypeEnum.Specific
                        ' Submission method checkboxlist item
                        For Each boxItem As ListItem In chkSubmissionMethod.Items
                            If boxItem.Selected = True Then
                                strSubmissionMethodValue += String.Format("{0}{1}", boxItem.Value.Trim, ",")
                            End If
                        Next
                        strSubmissionMethodValue = strSubmissionMethodValue.Substring(0, strSubmissionMethodValue.Length - 1)

                    Case Else
                        ' Nothing
                        strSubmissionMethodValue += ""
                End Select

                udtParameterList.AddParam(strSubmissionMethodLabel, strSubmissionMethodValue)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        If IsExistValue(Field.SubmissionMethod, FieldSetting.Visible) Then
            If GetSetting(Field.SubmissionMethod, FieldSetting.Visible) = Condition.YES Then
                Dim strSubmissionMethodLabel As String = lblSubmissionMethod.Text.Trim
                Dim strChkBoxItemString As String = String.Empty

                Select Case rbtnSubmissionMethodType.SelectedIndex
                    Case MultiSelectionTypeEnum.Any
                        strChkBoxItemString += "Any"
                        udtParameterList.AddParam(strSubmissionMethodLabel, strChkBoxItemString)

                    Case MultiSelectionTypeEnum.Specific
                        ' Submission method checkboxlist item
                        'strChkBoxItemString += "<ul style='padding-left: 15px; margin: 0px'>"
                        'For Each boxItem As ListItem In chkSubmissionMethod.Items
                        '    If boxItem.Selected = True Then
                        '        strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                        '    End If
                        'Next
                        'strChkBoxItemString += "</ul>"

                        ' Submission method checkboxlist item
                        Dim listParam As New ParameterObjectList(strSubmissionMethodLabel)
                        For Each boxItem As ListItem In chkSubmissionMethod.Items
                            If boxItem.Selected = True Then
                                listParam.ParamValueList.Add(boxItem.Text.ToString.Trim)
                            End If
                        Next

                        udtParameterList.AddParam(listParam)

                    Case Else
                        ' Nothing
                End Select

                'udtParameterList.AddParam(strSubmissionMethodLabel, strChkBoxItemString)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' Submission method
        If IsExistValue(Field.SubmissionMethod, FieldSetting.Visible) Then
            If GetSetting(Field.SubmissionMethod, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.SubmissionMethod, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty
                    strSPParamName = GetSetting(Field.SubmissionMethod, FieldSetting.SPParamName)

                    If rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Any Then
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, Nothing)
                    Else
                        Dim strPassValue As String = String.Empty

                        For Each boxItem As ListItem In chkSubmissionMethod.Items
                            If boxItem.Selected = True Then

                                ' Special case, If value = "WEB-FULL", pass the value "WEB,WEB-FULL"
                                If boxItem.Value.ToString.Trim = "WEB-FULL" Then
                                    strPassValue += "WEB," + boxItem.Value.ToString.Trim + ","
                                Else
                                    strPassValue += boxItem.Value.ToString.Trim + ","
                                End If

                            End If
                        Next

                        strPassValue = strPassValue.Substring(0, strPassValue.Length - 1)
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, strPassValue)
                    End If

                End If

            ElseIf GetSetting(Field.SubmissionMethod, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.SubmissionMethod, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.SubmissionMethod, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.SubmissionMethod, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.SubmissionMethod, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.SubmissionMethod, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        SetErrorSubmissionMethod(blnVisible)
    End Sub

#End Region

#Region "Supporting Function"

    Protected Overrides Sub BuildSubmissionMethodComponent(ByVal cboxListComponent As CheckBoxList)
        MyBase.BuildSubmissionMethodComponent(cboxListComponent)
    End Sub

    Public Overrides Sub InitControl()
        ' Set item - Submission Method    
        SetSubmissionMethod()

    End Sub

    Private Function CheckSubmissionMethodHasSelectedValue() As Boolean
        Dim blnHasSelectedValue As Boolean = False

        If rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Any Then
            blnHasSelectedValue = True
        Else
            For Each boxItem As ListItem In chkSubmissionMethod.Items
                If boxItem.Selected = True Then
                    blnHasSelectedValue = True
                    Exit For
                End If
            Next
        End If

        Return blnHasSelectedValue
    End Function

    Private Sub SetErrorSubmissionMethod(ByVal blnVisible As Boolean)
        imgErrorSubmissionMethod.Visible = blnVisible
    End Sub

    Private Sub AddSubmissionMethodIntoList()
        Dim items As New Dictionary(Of String, Boolean)

        ' Submission method checkboxlist item
        For Each boxItem As ListItem In chkSubmissionMethod.Items
            items.Add(boxItem.Value.ToString.Trim, boxItem.Selected)
        Next

        ViewState(VS.SubmissionMethod) = items

        ' Submission method checkboxlist item

        Dim strChkBoxItemString As String = ""
        Dim intIsSelectedCount As Integer = 0

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkSubmissionMethod.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
                intIsSelectedCount += 1
            End If
        Next
        strChkBoxItemString += "</ul>"

        'If intIsSelectedCount = chkSubmissionMethod.Items.Count Then
        '    rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Any
        '    strChkBoxItemString = String.Empty
        'Else
        '    ' Have selected items
        '    If intIsSelectedCount > 0 Then
        '        rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Specific
        '    Else
        '        rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        '    End If

        'End If

        ' Have selected items
        If intIsSelectedCount > 0 Then
            rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Specific
        Else
            rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.NoSelection
        End If

        lblAddSubmissionMethodDisplay.Text = strChkBoxItemString

    End Sub

    Private Sub SetSubmissionMethodSelectionToAny()
        'BuildProfessionComponent(ddlScheme, chkProfession, Field.Profession)
        For Each boxItem As ListItem In chkSubmissionMethod.Items
            boxItem.Selected = False
        Next

        ViewState(VS.SubmissionMethod) = Nothing

        rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Any

        ' Submission method checkbox list item
        Dim strChkBoxItemString As String = String.Empty

        strChkBoxItemString += "<ul style='padding-left: 20px; margin: 0px'>"
        For Each boxItem As ListItem In chkSubmissionMethod.Items
            If boxItem.Selected = True Then
                strChkBoxItemString += "<li>" + boxItem.Text.ToString.Trim + "</li>"
            End If
        Next
        strChkBoxItemString += "</ul>"

        lblAddSubmissionMethodDisplay.Text = strChkBoxItemString
    End Sub

#End Region

#Region "Fields Setting"

    ' Set item - Submission Method
    Private Sub SetSubmissionMethod()
        ' Set field description
        If IsExistValue(Field.SubmissionMethod, FieldSetting.DescResource) Then
            lblSubmissionMethod.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.SubmissionMethod, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.SubmissionMethod, FieldSetting.Visible) Then
            Select Case GetSetting(Field.SubmissionMethod, FieldSetting.Visible)
                Case Condition.YES
                    panSubmissionMethod.Visible = True
                Case Condition.NO
                    panSubmissionMethod.Visible = False
                Case Else
                    panSubmissionMethod.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.SubmissionMethod, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.SubmissionMethod, FieldSetting.DefaultValue) = String.Empty Then

                If GetSetting(Field.SubmissionMethod, FieldSetting.DefaultValue) = "ANY" Then
                    For Each listItem As ListItem In chkSubmissionMethod.Items
                        listItem.Selected = True
                    Next
                Else
                    Dim strDefaultValue As String = GetSetting(Field.SubmissionMethod, FieldSetting.DefaultValue)
                    Dim valueList As String() = strDefaultValue.Split(New Char() {","c})

                    For Each valueItem As String In valueList
                        Dim listItem As ListItem = chkSubmissionMethod.Items.FindByValue(valueItem)
                        If Not listItem Is Nothing Then
                            listItem.Selected = True
                        End If
                    Next
                    
                End If

            End If
        End If

        ' Add selected value to list 
        AddSubmissionMethodIntoList()

    End Sub

#End Region

End Class