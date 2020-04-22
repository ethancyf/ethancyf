Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Component.HCVUUser
Imports Common.Component.Profession

Partial Public Class udcDescription
    Inherits StatisticsCriteriaUC

#Region "Session and Const"

    Private Class VS
        Public Const Description As String = "Description"
    End Class

    Public Class Field
        Public Const Description As String = "Description"
    End Class

    Public Class FunctionCode
        Public Const Funct_010704 As String = "010704"
    End Class

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

#End Region

#Region "Implement IStatisticsCriteriaUC"

    Public Overrides Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
        MyBase.Build(dicSetting)

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))

    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        Throw New NotImplementedException
    End Sub

#End Region

#Region "Supporting Function"

    Public Overrides Sub InitControl()
        ' Set item - InputText
        SetInputText()

    End Sub

    'Private Function CheckInputTextHasValue() As Boolean
    '    Dim blnHasSelectedValue As Boolean = False

    '    If txtInput.Text <> String.Empty AndAlso IsNumeric(txtInput.Text) Then
    '        blnHasSelectedValue = True
    '    End If

    '    Return blnHasSelectedValue
    'End Function

    'Private Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
    '    imgErrorInput.Visible = blnVisible
    'End Sub

#End Region

#Region "Fields Setting"

    Private Sub SetInputText()
        ' Set field description
        If IsExistValue(Field.Description, FieldSetting.DescResource) Then
            lblDescriptionText.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.Description, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.Description, FieldSetting.Visible) Then
            Select Case GetSetting(Field.Description, FieldSetting.Visible)
                Case Condition.YES
                    panDescription.Visible = True
                Case Condition.NO
                    panDescription.Visible = False
                Case Else
                    panDescription.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.Description, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.Description, FieldSetting.DefaultValue) = String.Empty Then
                Me.tdDescription.InnerHtml = Me.GetGlobalResourceObject("Text", GetSetting(Field.Description, FieldSetting.DefaultValue))
            End If
        End If

    End Sub

#End Region

End Class