Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Component.HCVUUser
Imports Common.Component.Profession

Partial Public Class udcNumeric
    Inherits StatisticsCriteriaUC

#Region "Session and Const"

    Private Class VS
        Public Const InputValue As String = "InputValue"
    End Class

    Public Class Field
        Public Const InputValue As String = "InputValue"
        Public Const InputTextLabelWidth As String = "InputTextLabelWidth"
    End Class

    Public Class AdditionalFieldSetting
        Public Const InputLowerLimit As String = "LowerLimit"
        Public Const InputUpperLimit As String = "UpperLimit"
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
        ' Control setting file
        '_dicSetting = dicSetting
        'SetErrorComponentVisibility(False)

        MyBase.Build(dicSetting)

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        SetErrorComponentVisibility(False)

        ' Exist
        If IsExistValue(Field.InputValue, FieldSetting.Visible) AndAlso GetSetting(Field.InputValue, FieldSetting.Visible) = Condition.YES Then
            ' Contains input?
            If txtInput.Text = String.Empty Then
                Dim strInputTxtValue As String = String.Empty

                If IsExistValue(Field.InputValue, FieldSetting.DescResource) Then
                    strInputTxtValue = Me.GetGlobalResourceObject("Text", GetSetting(Field.InputValue, FieldSetting.DescResource))
                End If

                lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028))
                lstErrorParam1.Add(strInputTxtValue)
                lstErrorParam2.Add(String.Empty)
                imgErrorInput.Visible = True
            End If

            ' Is integer?
            If lstError.Count = 0 Then
                If Integer.TryParse(txtInput.Text, 0) = False Then
                    Dim strInputTxtValue As String = String.Empty

                    If IsExistValue(Field.InputValue, FieldSetting.DescResource) Then
                        strInputTxtValue = Me.GetGlobalResourceObject("Text", GetSetting(Field.InputValue, FieldSetting.DescResource))
                    End If

                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00365))
                    lstErrorParam1.Add(strInputTxtValue)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorInput.Visible = True
                End If

            End If

            ' Is within range?
            If lstError.Count = 0 Then
                Dim intInput As Integer = CInt(txtInput.Text)

                If intInput < CInt(GetSetting(Field.InputValue, AdditionalFieldSetting.InputLowerLimit)) _
                        OrElse intInput > CInt(GetSetting(Field.InputValue, AdditionalFieldSetting.InputUpperLimit)) Then
                    Dim strInputTxtValue As String = String.Empty

                    If IsExistValue(Field.InputValue, FieldSetting.DescResource) Then
                        strInputTxtValue = Me.GetGlobalResourceObject("Text", GetSetting(Field.InputValue, FieldSetting.DescResource))
                    End If

                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00366))
                    lstErrorParam1.Add(strInputTxtValue)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorInput.Visible = True

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

        ' Input
        If IsExistValue(Field.InputValue, FieldSetting.Visible) Then
            If GetSetting(Field.InputValue, FieldSetting.Visible) = Condition.YES Then
                Dim strInputTextLabel As String = lblInputText.Text
                Dim strInputTxtValue As String = String.Empty

                'If IsExistValue(Field.InputTxtValue, FieldSetting.DescResource) Then
                '    strInputTextLabel = GetSetting(Field.InputTxtValue, FieldSetting.DescResource)
                'End If

                If txtInput.Text <> String.Empty Then
                    strInputTxtValue = txtInput.Text
                End If

                If strInputTextLabel <> String.Empty AndAlso strInputTxtValue <> String.Empty Then
                    udtParameterList.AddParam(strInputTextLabel, strInputTxtValue)
                End If
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Input
        If IsExistValue(Field.InputValue, FieldSetting.Visible) Then
            If GetSetting(Field.InputValue, FieldSetting.Visible) = Condition.YES Then
                Dim strInputTextLabel As String = lblInputText.Text
                Dim strInputTxtValue As String = String.Empty

                'If IsExistValue(Field.InputTxtValue, FieldSetting.DescResource) Then
                '    strInputTextLabel = GetSetting(Field.InputTxtValue, FieldSetting.DescResource)
                'End If

                If strInputTextLabel <> String.Empty AndAlso txtInput.Text <> String.Empty Then
                    strInputTxtValue = txtInput.Text
                    udtParameterList.AddParam(strInputTextLabel, strInputTxtValue)
                End If

            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' Input
        If IsExistValue(Field.InputValue, FieldSetting.Visible) Then
            If GetSetting(Field.InputValue, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.InputValue, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty
                    strSPParamName = GetSetting(Field.InputValue, FieldSetting.SPParamName)

                    If txtInput.Text <> String.Empty Then
                        Dim strPassValue As String = txtInput.Text.Trim
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Int, 2, strPassValue)
                    End If

                End If

            ElseIf GetSetting(Field.InputValue, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.InputValue, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.InputValue, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.InputValue, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.InputValue, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.InputValue, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Int, 2, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        imgErrorInput.Visible = blnVisible
    End Sub

#End Region

#Region "Supporting Function"

    Public Overrides Sub InitControl()
        ' Set item - InputText
        SetInputText()

    End Sub

    Private Function CheckInputTextHasValue() As Boolean
        Dim blnHasSelectedValue As Boolean = False

        If txtInput.Text <> String.Empty AndAlso IsNumeric(txtInput.Text) Then
            blnHasSelectedValue = True
        End If

        Return blnHasSelectedValue
    End Function

#End Region

#Region "Fields Setting"

    Private Sub SetInputText()
        ' Set field description
        If IsExistValue(Field.InputValue, FieldSetting.DescResource) Then
            lblInputText.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.InputValue, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.InputValue, FieldSetting.Visible) Then
            Select Case GetSetting(Field.InputValue, FieldSetting.Visible)
                Case Condition.YES
                    panInput.Visible = True
                Case Condition.NO
                    panInput.Visible = False
                Case Else
                    panInput.Visible = False
            End Select
        End If

        'Set Input label width
        If IsExistValue(Field.InputTextLabelWidth, FieldSetting.DefaultValue) Then
            If IsNumeric(GetSetting(Field.InputTextLabelWidth, FieldSetting.DefaultValue)) Then
                Me.lblInputText.Width = GetSetting(Field.InputTextLabelWidth, FieldSetting.DefaultValue)
            End If
        End If

        ' Set default value
        If IsExistValue(Field.InputValue, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.InputValue, FieldSetting.DefaultValue) = String.Empty Then
                Me.lblInputText.Text = GetSetting(Field.InputValue, FieldSetting.DefaultValue)
            End If
        End If

        ' Set Description Label
        If IsExistValue(Field.InputValue, AdditionalFieldSetting.InputLowerLimit) And IsExistValue(Field.InputValue, AdditionalFieldSetting.InputUpperLimit) Then
            If Not GetSetting(Field.InputValue, AdditionalFieldSetting.InputLowerLimit) = String.Empty And Not GetSetting(Field.InputValue, AdditionalFieldSetting.InputUpperLimit) = String.Empty Then
                Me.lblInputDesc.Text = "( " & GetSetting(Field.InputValue, AdditionalFieldSetting.InputLowerLimit) & " - " & GetSetting(Field.InputValue, AdditionalFieldSetting.InputUpperLimit) & " )"
                If GetSetting(Field.InputValue, AdditionalFieldSetting.InputUpperLimit).Length > 0 Then
                    Me.txtInput.MaxLength = GetSetting(Field.InputValue, AdditionalFieldSetting.InputUpperLimit).Length
                    Me.txtInput.Width = Unit.Pixel(8 * GetSetting(Field.InputValue, AdditionalFieldSetting.InputUpperLimit).Length + 2)
                Else
                    Me.txtInput.Width = Unit.Pixel(30)
                End If
            End If
        End If

    End Sub

#End Region

End Class