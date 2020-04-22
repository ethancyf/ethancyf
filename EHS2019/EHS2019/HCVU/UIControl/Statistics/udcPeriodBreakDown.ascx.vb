Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format

Partial Public Class udcPeriodBreakDown
    Inherits StatisticsCriteriaUC

#Region "Variables"

    'Private _dicSetting As New Dictionary(Of String, Dictionary(Of String, String))

#End Region

#Region "Session and Const"

    Public Class Field
        Public Const PeriodBreakDown As String = "PeriodBreakDown"
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

        ' Build period break down component
        BuildPeriodBreakDownComponent(Me.ddlPeriodBreakDown)

        MyBase.Build(dicSetting)

        ' Initial control
        'InitControl()

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        SetErrorComponentVisibility(False)

        ' Period break down
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.Visible) Then
            If GetSetting(Field.PeriodBreakDown, FieldSetting.Visible) = Condition.YES Then
                ' Check dropdown (Period break down)
                If ddlPeriodBreakDown.SelectedValue.Trim = DROP_DOWN_EMPTY Then
                    Dim strPeriodBreakDownText As String = String.Empty

                    If IsExistValue(Field.PeriodBreakDown, FieldSetting.DescResource) Then
                        strPeriodBreakDownText = Me.GetGlobalResourceObject("Text", GetSetting(Field.PeriodBreakDown, FieldSetting.DescResource))
                    Else
                        strPeriodBreakDownText = Me.GetGlobalResourceObject("Text", "BreakDownType")
                    End If

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                    lstErrorParam1.Add(strPeriodBreakDownText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorPeriodBreakDown.Visible = True
                End If

            End If
        End If


    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strPeriodBreakDownValue As String = String.Empty

        ' Period break down
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.Visible) Then
            If GetSetting(Field.PeriodBreakDown, FieldSetting.Visible) = Condition.YES Then
                Dim strPeriodBreakDown As String = lblPeriodBreakDown.Text.Trim
                If ddlPeriodBreakDown.SelectedIndex = 0 Then
                    strPeriodBreakDownValue += String.Empty
                Else
                    strPeriodBreakDownValue += ddlPeriodBreakDown.SelectedValue.Trim.ToString
                End If

                udtParameterList.AddParam(strPeriodBreakDown, strPeriodBreakDownValue)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Period break down
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.Visible) Then
            If GetSetting(Field.PeriodBreakDown, FieldSetting.Visible) = Condition.YES Then
                Dim strPeriodBreakDown As String = lblPeriodBreakDown.Text.Trim
                udtParameterList.AddParam(strPeriodBreakDown, ddlPeriodBreakDown.SelectedItem.Text.Trim)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' Period break down
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.Visible) Then
            If GetSetting(Field.PeriodBreakDown, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.PeriodBreakDown, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty

                    strSPParamName = GetSetting(Field.PeriodBreakDown, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 1, ddlPeriodBreakDown.SelectedValue.Trim)
                End If

            ElseIf GetSetting(Field.PeriodBreakDown, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.PeriodBreakDown, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.PeriodBreakDown, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.PeriodBreakDown, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.PeriodBreakDown, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.PeriodBreakDown, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 1, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        imgErrorPeriodBreakDown.Visible = blnVisible

    End Sub

#End Region

#Region "Supporting Function"

    Protected Overrides Sub BuildPeriodBreakDownComponent(ByVal ddlComponent As DropDownList)
        MyBase.BuildPeriodBreakDownComponent(ddlComponent)
    End Sub

    Public Overrides Sub InitControl()
        ' Set item - Period break down    
        SetPeriodBreakDown()
    End Sub

#End Region

#Region "Fields Setting"

    ' Set item - Period break down
    Private Sub SetPeriodBreakDown()
        ' Set field description
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.DescResource) Then
            lblPeriodBreakDown.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.PeriodBreakDown, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.Visible) Then
            Select Case GetSetting(Field.PeriodBreakDown, FieldSetting.Visible)
                Case Condition.YES
                    panPeriodBreakDown.Visible = True
                Case Condition.NO
                    panPeriodBreakDown.Visible = False
                Case Else
                    panPeriodBreakDown.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.PeriodBreakDown, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.PeriodBreakDown, FieldSetting.DefaultValue) = String.Empty Then

                Dim listItem As ListItem = ddlPeriodBreakDown.Items.FindByValue(GetSetting(Field.PeriodBreakDown, FieldSetting.DefaultValue))
                If Not listItem Is Nothing Then
                    ddlPeriodBreakDown.SelectedValue = listItem.Value
                End If

            End If
        End If

    End Sub

#End Region

End Class