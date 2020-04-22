Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Component.HCVUUser
Imports Common.Component.Profession

Partial Public Class udcPercentile
    Inherits StatisticsCriteriaUC

#Region "Session and Const"

    Public Class Field
        Public Const PercentileOption As String = "PercentileOption"
        Public Const OptionValue1 As String = "OptionValue1"
        Public Const OptionValue2 As String = "OptionValue2"
        Public Const OptionValue3 As String = "OptionValue3"
    End Class

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim strScript As New StringBuilder
        strScript.Append("function CheckboxTextboxRelation(chk, textboxId) {")
        strScript.Append("  var txt = document.getElementById(textboxId);")
        strScript.Append("  if (chk.checked) {")
        strScript.Append("      txt.disabled = '';")
        strScript.Append("  } else {")
        strScript.Append("      txt.disabled = 'disabled';")
        strScript.Append("  }};")

        ScriptManager.RegisterStartupScript(Me, GetType(Page), "Script", strScript.ToString, True)
    End Sub

    Protected Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        ' Reinit the disabled textboxes
        If chkP1.Checked Then
            txtP1.Attributes.Remove("disabled")
        Else
            txtP1.Attributes("disabled") = "disabled"
        End If

        If chkP2.Checked Then
            txtP2.Attributes.Remove("disabled")
        Else
            txtP2.Attributes("disabled") = "disabled"
        End If

        If chkP3.Checked Then
            txtP3.Attributes.Remove("disabled")
        Else
            txtP3.Attributes("disabled") = "disabled"
        End If

        chkP1.Attributes("onclick") = String.Format("javascript: CheckboxTextboxRelation(this, '{0}');", txtP1.ClientID)
        chkP2.Attributes("onclick") = String.Format("javascript: CheckboxTextboxRelation(this, '{0}');", txtP2.ClientID)
        chkP3.Attributes("onclick") = String.Format("javascript: CheckboxTextboxRelation(this, '{0}');", txtP3.ClientID)
    End Sub

#End Region

#Region "Implement IStatisticsCriteriaUC"

    Public Overrides Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
        MyBase.Build(dicSetting)
        SetErrorComponentVisibility(False)
    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        SetErrorComponentVisibility(False)

        Dim lstSelected As New List(Of String)
        Dim blnDuplicate As Boolean = False

        If IsExistValue(Field.OptionValue1, FieldSetting.Visible) Then
            If GetSetting(Field.OptionValue1, FieldSetting.Visible) = Condition.YES Then
                If chkP1.Checked Then
                    If Not ValidateTextboxInput(txtP1.Text, 1, lstError, lstErrorParam1, lstErrorParam2) Then
                        imgError.Visible = True
                    Else
                        txtP1.Text = CInt(txtP1.Text)
                        If lstSelected.Contains(txtP1.Text) Then blnDuplicate = True
                        lstSelected.Add(txtP1.Text)
                    End If
                End If
            End If
        End If

        If IsExistValue(Field.OptionValue2, FieldSetting.Visible) Then
            If GetSetting(Field.OptionValue2, FieldSetting.Visible) = Condition.YES Then
                If chkP2.Checked Then
                    If Not ValidateTextboxInput(txtP2.Text, 2, lstError, lstErrorParam1, lstErrorParam2) Then
                        imgError.Visible = True
                    Else
                        txtP2.Text = CInt(txtP2.Text)
                        If lstSelected.Contains(txtP2.Text) Then blnDuplicate = True
                        lstSelected.Add(txtP2.Text)
                    End If
                End If
            End If
        End If

        If IsExistValue(Field.OptionValue3, FieldSetting.Visible) Then
            If GetSetting(Field.OptionValue3, FieldSetting.Visible) = Condition.YES Then
                If chkP3.Checked Then
                    If Not ValidateTextboxInput(txtP3.Text, 3, lstError, lstErrorParam1, lstErrorParam2) Then
                        imgError.Visible = True
                    Else
                        txtP3.Text = CInt(txtP3.Text)
                        If lstSelected.Contains(txtP3.Text) Then blnDuplicate = True
                        lstSelected.Add(txtP3.Text)
                    End If
                End If
            End If
        End If

        ' Duplicate Value
        If blnDuplicate Then
            imgError.Visible = True
            lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00033))
            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.PercentileOption, FieldSetting.DescResource)))
            lstErrorParam2.Add(String.Empty)
        End If

    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strSelectedPercentile As String = String.Empty
        Dim lstSelected As New List(Of String)

        If IsExistValue(Field.OptionValue1, FieldSetting.Visible) Then
            If GetSetting(Field.OptionValue1, FieldSetting.Visible) = Condition.YES Then
                If chkP1.Checked Then
                    lstSelected.Add(txtP1.Text)
                End If
            End If
        End If

        If IsExistValue(Field.OptionValue2, FieldSetting.Visible) Then
            If GetSetting(Field.OptionValue2, FieldSetting.Visible) = Condition.YES Then
                If chkP2.Checked Then
                    lstSelected.Add(txtP2.Text)
                End If
            End If
        End If

        If IsExistValue(Field.OptionValue3, FieldSetting.Visible) Then
            If GetSetting(Field.OptionValue3, FieldSetting.Visible) = Condition.YES Then
                If chkP3.Checked Then
                    lstSelected.Add(txtP3.Text)
                End If
            End If
        End If

        If lstSelected.Count > 0 Then
            strSelectedPercentile = String.Join(",", lstSelected.ToArray)
        Else
            strSelectedPercentile = "---"
        End If

        udtParameterList.AddParam(lblPercentileText.Text.Trim, strSelectedPercentile)

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strSelectedPercentile As String = String.Empty
        Dim lstSelected As New List(Of String)

        ' Input
        If IsExistValue(Field.OptionValue1, FieldSetting.Visible) Then
            If GetSetting(Field.OptionValue1, FieldSetting.Visible) = Condition.YES Then
                If chkP1.Checked Then
                    lstSelected.Add(String.Format("{0}%", txtP1.Text))
                End If
            End If
        End If

        If IsExistValue(Field.OptionValue2, FieldSetting.Visible) Then
            If GetSetting(Field.OptionValue2, FieldSetting.Visible) = Condition.YES Then
                If chkP2.Checked Then
                    lstSelected.Add(String.Format("{0}%", txtP2.Text))
                End If
            End If
        End If

        If IsExistValue(Field.OptionValue3, FieldSetting.Visible) Then
            If GetSetting(Field.OptionValue3, FieldSetting.Visible) = Condition.YES Then
                If chkP3.Checked Then
                    lstSelected.Add(String.Format("{0}%", txtP3.Text))
                End If
            End If
        End If

        If lstSelected.Count > 0 Then
            strSelectedPercentile = String.Join(",", lstSelected.ToArray)
        Else
            strSelectedPercentile = "---"
        End If

        udtParameterList.AddParam(lblPercentileText.Text.Trim, strSelectedPercentile)

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection
        Dim lstSelected As New List(Of String)

        If IsExistValue(Field.OptionValue1, FieldSetting.Visible) Then
            If GetSetting(Field.OptionValue1, FieldSetting.Visible) = Condition.YES Then
                If chkP1.Checked Then
                    lstSelected.Add(txtP1.Text)
                End If
            End If
        End If

        If IsExistValue(Field.OptionValue2, FieldSetting.Visible) Then
            If GetSetting(Field.OptionValue2, FieldSetting.Visible) = Condition.YES Then
                If chkP2.Checked Then
                    lstSelected.Add(txtP2.Text)
                End If
            End If
        End If

        If IsExistValue(Field.OptionValue3, FieldSetting.Visible) Then
            If GetSetting(Field.OptionValue3, FieldSetting.Visible) = Condition.YES Then
                If chkP3.Checked Then
                    lstSelected.Add(txtP3.Text)
                End If
            End If
        End If

        If IsExistValue(Field.PercentileOption, FieldSetting.SPParamName) Then
            Dim strSPParamName As String = String.Empty
            strSPParamName = GetSetting(Field.PercentileOption, FieldSetting.SPParamName)

            If lstSelected.Count > 0 Then
                udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 5000, String.Join(",", lstSelected.ToArray))
            End If
        End If

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        imgError.Visible = blnVisible
    End Sub

#End Region

#Region "Supporting Function"

    Public Overrides Sub InitControl()
        ' Set item - InputText
        SetInputText()

    End Sub

    Private Function ValidateTextboxInput(strInput As String, intIndex As Integer, _
                                          ByRef lstError As List(Of SystemMessage), _
                                          ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String)) As Boolean

        Dim strLabelText As String = String.Format("{0} ({1})", _
                                                   Me.GetGlobalResourceObject("Text", GetSetting(Field.PercentileOption, FieldSetting.DescResource)), _
                                                   intIndex)


        ' Empty?
        If strInput = String.Empty Then
            lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028))
            lstErrorParam1.Add(strLabelText)
            lstErrorParam2.Add(String.Empty)

            Return False
        End If

        ' Is numeric?
        If IsNumeric(strInput) = False Then
            lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00365))
            lstErrorParam1.Add(strLabelText)
            lstErrorParam2.Add(String.Empty)

            Return False
        End If

        ' Within range?
        If CInt(strInput) < 0 OrElse CInt(strInput) > 100 Then
            lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00366))
            lstErrorParam1.Add(strLabelText)
            lstErrorParam2.Add(String.Empty)

            Return False
        End If

        Return True
    End Function

#End Region

#Region "Fields Setting"

    Private Sub SetInputText()
        ' Set field description
        If IsExistValue(Field.PercentileOption, FieldSetting.DescResource) Then
            lblPercentileText.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.PercentileOption, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.PercentileOption, FieldSetting.Visible) Then
            Select Case GetSetting(Field.PercentileOption, FieldSetting.Visible)
                Case Condition.YES
                    panPercentile.Visible = True
                Case Condition.NO
                    panPercentile.Visible = False
                Case Else
                    panPercentile.Visible = False
            End Select
        End If

        If IsExistValue(Field.OptionValue1, FieldSetting.Visible) Then
            Select Case GetSetting(Field.OptionValue1, FieldSetting.Visible)
                Case Condition.YES
                    panP1.Visible = True
                Case Condition.NO
                    panP1.Visible = False
                Case Else
                    panP1.Visible = False
            End Select
        End If

        If IsExistValue(Field.OptionValue2, FieldSetting.Visible) Then
            Select Case GetSetting(Field.OptionValue2, FieldSetting.Visible)
                Case Condition.YES
                    panP2.Visible = True
                Case Condition.NO
                    panP2.Visible = False
                Case Else
                    panP2.Visible = False
            End Select
        End If

        If IsExistValue(Field.OptionValue3, FieldSetting.Visible) Then
            Select Case GetSetting(Field.OptionValue3, FieldSetting.Visible)
                Case Condition.YES
                    panP3.Visible = True
                Case Condition.NO
                    panP3.Visible = False
                Case Else
                    panP3.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.OptionValue1, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.OptionValue1, FieldSetting.DefaultValue) = String.Empty Then
                txtP1.Text = GetSetting(Field.OptionValue1, FieldSetting.DefaultValue)
            End If
        End If

        If IsExistValue(Field.OptionValue2, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.OptionValue2, FieldSetting.DefaultValue) = String.Empty Then
                txtP2.Text = GetSetting(Field.OptionValue2, FieldSetting.DefaultValue)
            End If
        End If

        If IsExistValue(Field.OptionValue3, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.OptionValue3, FieldSetting.DefaultValue) = String.Empty Then
                txtP3.Text = GetSetting(Field.OptionValue3, FieldSetting.DefaultValue)
            End If
        End If

        ' Set checkbox default check value
        chkP1.Checked = False
        chkP2.Checked = False
        chkP3.Checked = False

        If IsExistValue(Field.PercentileOption, FieldSetting.DefaultValue) Then
            If GetSetting(Field.PercentileOption, FieldSetting.DefaultValue) = Condition.YES Then
                chkP1.Checked = True
                chkP2.Checked = True
                chkP3.Checked = True
            End If
        End If

    End Sub

#End Region

End Class