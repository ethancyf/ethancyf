Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format

Partial Public Class udcMeansOfInput
    Inherits StatisticsCriteriaUC

#Region "Variables"

    'Private _dicSetting As New Dictionary(Of String, Dictionary(Of String, String))

#End Region

#Region "Session and Const"

    Public Class Field
        Public Const MeansOfInput As String = "MeansOfInput"
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

        ' Build submission method component
        BuildMeansOfInputComponent(Me.ddlMeansOfInput)

        MyBase.Build(dicSetting)

        ' Initial control
        'InitControl()

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        SetErrorComponentVisibility(False)

        ' Means of input
        If IsExistValue(Field.MeansOfInput, FieldSetting.Visible) Then
            If GetSetting(Field.MeansOfInput, FieldSetting.Visible) = Condition.YES Then
                ' Check dropdown (Scheme)
                If ddlMeansOfInput.SelectedValue.Trim = DROP_DOWN_EMPTY Then
                    Dim strMeansOfInputText As String = String.Empty

                    If IsExistValue(Field.MeansOfInput, FieldSetting.DescResource) Then
                        strMeansOfInputText = Me.GetGlobalResourceObject("Text", GetSetting(Field.MeansOfInput, FieldSetting.DescResource))
                    Else
                        strMeansOfInputText = Me.GetGlobalResourceObject("Text", "MeansOfInput")
                    End If

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                    lstErrorParam1.Add(strMeansOfInputText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorMeansOfInput.Visible = True
                End If

            End If
        End If


    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strMeansOfInputValue As String = String.Empty

        ' Means of input
        If IsExistValue(Field.MeansOfInput, FieldSetting.Visible) Then
            If GetSetting(Field.MeansOfInput, FieldSetting.Visible) = Condition.YES Then
                Dim strMeansOfInput As String = lblMeansOfInput.Text.Trim
                If ddlMeansOfInput.SelectedIndex = 0 Then
                    strMeansOfInputValue += String.Empty
                Else
                    strMeansOfInputValue += ddlMeansOfInput.SelectedValue.Trim.ToString
                End If

                udtParameterList.AddParam(strMeansOfInput, strMeansOfInputValue)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Means of input
        If IsExistValue(Field.MeansOfInput, FieldSetting.Visible) Then
            If GetSetting(Field.MeansOfInput, FieldSetting.Visible) = Condition.YES Then
                Dim strMeansOfInput As String = lblMeansOfInput.Text.Trim
                udtParameterList.AddParam(strMeansOfInput, ddlMeansOfInput.SelectedItem.Text.Trim)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' Means of input
        If IsExistValue(Field.MeansOfInput, FieldSetting.Visible) Then
            If GetSetting(Field.MeansOfInput, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.MeansOfInput, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty

                    strSPParamName = GetSetting(Field.MeansOfInput, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 20, ddlMeansOfInput.SelectedValue.Trim)
                End If

            ElseIf GetSetting(Field.MeansOfInput, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.MeansOfInput, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.MeansOfInput, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.MeansOfInput, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.MeansOfInput, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.MeansOfInput, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 20, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        imgErrorMeansOfInput.Visible = blnVisible
    End Sub

#End Region

#Region "Supporting Function"

    Protected Overrides Sub BuildMeansOfInputComponent(ByVal ddlComponent As DropDownList)
        MyBase.BuildMeansOfInputComponent(ddlComponent)

    End Sub

    Public Overrides Sub InitControl()
        ' Set item - Means of input    
        SetMeansOfInput()

    End Sub

#End Region

#Region "Fields Setting"

    ' Set item - Means of Input
    Private Sub SetMeansOfInput()
        ' Set field description
        If IsExistValue(Field.MeansOfInput, FieldSetting.DescResource) Then
            lblMeansOfInput.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.MeansOfInput, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.MeansOfInput, FieldSetting.Visible) Then
            Select Case GetSetting(Field.MeansOfInput, FieldSetting.Visible)
                Case Condition.YES
                    panMeansOfInput.Visible = True
                Case Condition.NO
                    panMeansOfInput.Visible = False
                Case Else
                    panMeansOfInput.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.MeansOfInput, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.MeansOfInput, FieldSetting.DefaultValue) = String.Empty Then

                Dim listItem As ListItem = ddlMeansOfInput.Items.FindByValue(GetSetting(Field.MeansOfInput, FieldSetting.DefaultValue))
                If Not listItem Is Nothing Then
                    ddlMeansOfInput.SelectedValue = listItem.Value
                End If

            End If
        End If

    End Sub

#End Region

End Class