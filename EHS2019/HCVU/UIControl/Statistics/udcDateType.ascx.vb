Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format

Partial Public Class udcDateType
    Inherits StatisticsCriteriaUC

#Region "Variables"

    'Private _dicSetting As New Dictionary(Of String, Dictionary(Of String, String))

#End Region

#Region "Session and Const"

    Public Class Field
        Public Const DateType As String = "DateType"
    End Class

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

#End Region

#Region "Implement IStatisticsCriteriaUC"

    Public Overrides Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
        ' Control setting file
        '_dicSetting = dicSetting

        MyBase.Build(dicSetting)

        ' Initial control
        'InitControl()

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        SetErrorComponentVisibility(False)

        ' Date Type
        If IsExistValue(Field.DateType, FieldSetting.Visible) Then
            If GetSetting(Field.DateType, FieldSetting.Visible) = Condition.YES Then
                ' Check dropdown (Scheme)
                If CheckDateTypeHasSelectedValue() = False Then
                    Dim strDateType As String = String.Empty

                    If IsExistValue(Field.DateType, FieldSetting.DescResource) Then
                        strDateType = Me.GetGlobalResourceObject("Text", GetSetting(Field.DateType, FieldSetting.DescResource))
                    Else
                        strDateType = Me.GetGlobalResourceObject("Text", "TypeOfDate")
                    End If

                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00367))
                    lstErrorParam1.Add(strDateType)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorDateType.Visible = True
                End If

            End If
        End If


    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strDataTypeValue As String = String.Empty

        ' Date Type
        If IsExistValue(Field.DateType, FieldSetting.Visible) Then
            If GetSetting(Field.DateType, FieldSetting.Visible) = Condition.YES Then
                Dim strDateType As String = lblDateType.Text.Trim
                If rbtnDateType.SelectedItem Is Nothing Then
                    strDataTypeValue += String.Empty
                Else
                    strDataTypeValue += rbtnDateType.SelectedValue.Trim.ToString
                End If

                udtParameterList.AddParam(strDateType, strDataTypeValue)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Date Type
        If IsExistValue(Field.DateType, FieldSetting.Visible) Then
            If GetSetting(Field.DateType, FieldSetting.Visible) = Condition.YES Then
                Dim strDateType As String = lblDateType.Text.Trim
                If rbtnDateType.SelectedItem Is Nothing Then
                    udtParameterList.AddParam(strDateType, String.Empty)
                Else
                    udtParameterList.AddParam(strDateType, rbtnDateType.SelectedItem.Text.Trim)
                End If
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' Date Type
        If IsExistValue(Field.DateType, FieldSetting.Visible) Then
            If GetSetting(Field.DateType, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.DateType, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty

                    strSPParamName = GetSetting(Field.DateType, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 1, rbtnDateType.SelectedValue.Trim)
                End If

            ElseIf GetSetting(Field.DateType, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.DateType, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.DateType, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.DateType, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.DateType, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.DateType, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 20, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        imgErrorDateType.Visible = blnVisible
    End Sub

#End Region

#Region "Supporting Function"

    Public Overrides Sub InitControl()
        ' Set item - Date Type    
        SetDateType()

    End Sub

    Private Function CheckDateTypeHasSelectedValue() As Boolean
        Dim blnHasSelectedValue As Boolean = False

        'If rbtnSubmissionMethodType.SelectedIndex = MultiSelectionTypeEnum.Any Then
        '    blnHasSelectedValue = True
        'Else
        '    For Each boxItem As ListItem In chkSubmissionMethod.Items
        '        If boxItem.Selected = True Then
        '            blnHasSelectedValue = True
        '            Exit For
        '        End If
        '    Next
        'End If

        For Each rbtnItem As ListItem In rbtnDateType.Items
            If rbtnItem.Selected = True Then
                blnHasSelectedValue = True
                Exit For
            End If
        Next

        Return blnHasSelectedValue
    End Function

#End Region

#Region "Fields Setting"

    ' Set item - Date Type
    Private Sub SetDateType()
        ' Set field description
        If IsExistValue(Field.DateType, FieldSetting.DescResource) Then
            lblDateType.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.DateType, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.DateType, FieldSetting.Visible) Then
            Select Case GetSetting(Field.DateType, FieldSetting.Visible)
                Case Condition.YES
                    panDateType.Visible = True
                Case Condition.NO
                    panDateType.Visible = False
                Case Else
                    panDateType.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.DateType, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.DateType, FieldSetting.DefaultValue) = String.Empty Then

                Dim listItem As ListItem = rbtnDateType.Items.FindByValue(GetSetting(Field.DateType, FieldSetting.DefaultValue))
                If Not listItem Is Nothing Then
                    rbtnDateType.SelectedValue = listItem.Value
                End If

            End If
        End If

    End Sub

#End Region

End Class