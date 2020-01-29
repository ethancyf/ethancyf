Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Component.HCVUUser
Imports Common.Component.Profession

Partial Public Class udcCheckBox
    Inherits StatisticsCriteriaUC

#Region "Session and Const"

    Private Class VS
        Public Const ChkItem As String = "ChkItem"
    End Class

    Public Class Field
        Public Const ChkItem As String = "ChkItem"
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

        ' TransStatus
        If IsExistValue(Field.ChkItem, FieldSetting.Visible) Then
            If GetSetting(Field.ChkItem, FieldSetting.Visible) = Condition.YES Then
                ' Check checkbox list (TransStatus)
                'If CheckInputTextHasValue() = False Then
                '    Dim strInputTxtValue As String = String.Empty

                '    If IsExistValue(Field.ChkItem, FieldSetting.DescResource) Then
                '        strInputTxtValue = Me.GetGlobalResourceObject("Text", GetSetting(Field.ChkItem, FieldSetting.DescResource))
                '    End If

                '    lstError.Add(New SystemMessage(FunctionCode.Funct_010704, SeverityCode.SEVE, MsgCode.MSG00002))
                '    lstErrorParam1.Add(strInputTxtValue)
                '    lstErrorParam2.Add(String.Empty)
                '    imgErrorChkItem.Visible = True
                'End If
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
        If IsExistValue(Field.ChkItem, FieldSetting.Visible) Then
            If GetSetting(Field.ChkItem, FieldSetting.Visible) = Condition.YES Then
                Dim strChkItemLabel As String = Replace(lblChkItemText.Text.Trim, "<br>", " ")
                Dim strChkItemValue As String = String.Empty

                If chkItem.Checked Then
                    strChkItemValue = YesNo.Yes
                Else
                    strChkItemValue = YesNo.No
                End If

                udtParameterList.AddParam(strChkItemLabel, strChkItemValue)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Input
        If IsExistValue(Field.ChkItem, FieldSetting.Visible) Then
            If GetSetting(Field.ChkItem, FieldSetting.Visible) = Condition.YES Then
                Dim strChkItemLabel As String = Replace(lblChkItemText.Text.Trim, "<br>", " ")
                Dim strChkItemValue As String = String.Empty

                If chkItem.Checked Then
                    strChkItemValue = YesNo.Yes
                Else
                    strChkItemValue = YesNo.No
                End If

                udtParameterList.AddParam(strChkItemLabel, strChkItemValue)

            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' Input
        If IsExistValue(Field.ChkItem, FieldSetting.Visible) Then
            If GetSetting(Field.ChkItem, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.ChkItem, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty
                    strSPParamName = GetSetting(Field.ChkItem, FieldSetting.SPParamName)

                    If chkItem.Checked Then
                        Dim strPassValue As String = YesNo.Yes
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Bit, 1, True)
                    Else
                        Dim strPassValue As String = YesNo.No
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Bit, 1, False)
                    End If

                End If

            ElseIf GetSetting(Field.ChkItem, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.ChkItem, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.ChkItem, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.ChkItem, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.ChkItem, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.ChkItem, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 1, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
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
        If IsExistValue(Field.ChkItem, FieldSetting.DescResource) Then
            lblchkItemText.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.ChkItem, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.ChkItem, FieldSetting.Visible) Then
            Select Case GetSetting(Field.ChkItem, FieldSetting.Visible)
                Case Condition.YES
                    panChkItem.Visible = True
                Case Condition.NO
                    panChkItem.Visible = False
                Case Else
                    panChkItem.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.ChkItem, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.ChkItem, FieldSetting.DefaultValue) = String.Empty Then
                Me.lblchkItemText.Text = GetSetting(Field.ChkItem, FieldSetting.DefaultValue)
            End If
        End If

    End Sub

#End Region

End Class