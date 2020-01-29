Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Component.HCVUUser

Partial Public Class udcScheme
    Inherits StatisticsCriteriaUC

#Region "Variable"

    'Private _dicSetting As Dictionary(Of String, Dictionary(Of String, String))

#End Region

#Region "Session and Const"

    Public Class Field
        Public Const Scheme As String = "Scheme"
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
        SetErrorComponentVisibility(False)

        ' Build scheme component
        BuildSchemeComponent(Me.ddlScheme)
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

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                    lstErrorParam1.Add(strSchemeText)
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

        ' Scheme (Can configure visibility)
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                Dim strSchemeLabelText As String = lblScheme.Text.Trim
                If ddlScheme.SelectedIndex = 0 Then
                    strSchemeValue += String.Empty
                Else
                    strSchemeValue += ddlScheme.SelectedValue.Trim.ToString
                End If

                udtParameterList.AddParam(strSchemeLabelText, strSchemeValue)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Scheme (Can configure visibility)
        If IsExistValue(Field.Scheme, FieldSetting.Visible) Then
            If GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.YES Then
                Dim strSchemeLabelText As String = lblScheme.Text.Trim
                udtParameterList.AddParam(strSchemeLabelText, ddlScheme.SelectedItem.Text.Trim)
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
                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 10, ddlScheme.SelectedValue.Trim)
                End If

            ElseIf GetSetting(Field.Scheme, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.Scheme, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.Scheme, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.Scheme, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.Scheme, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.Scheme, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 10, strDefaultValue)
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

    Protected Overrides Sub BuildSchemeComponent(ByVal ddlComponent As DropDownList)
        MyBase.BuildSchemeComponent(ddlComponent)
    End Sub

    Public Overrides Sub InitControl()
        ' Set item - Scheme    
        SetScheme()

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

#End Region

End Class