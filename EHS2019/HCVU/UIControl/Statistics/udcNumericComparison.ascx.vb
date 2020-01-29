Imports Common.ComFunction.ParameterFunction
Imports Common.ComFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.EHSTransaction
Imports Common.Component.FileGeneration
Imports Common.Component.HCVUUser
Imports Common.Component.Profession
Imports Common.Component.Scheme
Imports Common.Component.UserRole
Imports Common.Format

Partial Public Class udcNumericComparison
    Inherits StatisticsCriteriaUC

#Region "Session and Const"

    Public Class Field
        Public Const InputTextLabelWidth As String = "InputTextLabelWidth"
        Public Const CompareItem As String = "CompareItem"
        Public Const CompareOperator As String = "CompareOperator"
        Public Const CompareValue As String = "CompareValue"
    End Class

    Public Class AdditionalFieldSetting
        Public Const InputLowerLimit As String = "LowerLimit"
        Public Const InputUpperLimit As String = "UpperLimit"
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

        Dim blnError As Boolean = False

        ' Exist
        If IsExistValue(Field.CompareValue, FieldSetting.Visible) AndAlso GetSetting(Field.CompareValue, FieldSetting.Visible) = Condition.YES Then
            ' Contains input?
            If txtInput.Text = String.Empty Then
                Dim strInputTxtValue As String = String.Empty

                If IsExistValue(Field.CompareItem, FieldSetting.DescResource) Then
                    strInputTxtValue = Me.GetGlobalResourceObject("Text", GetSetting(Field.CompareItem, FieldSetting.DescResource))
                End If

                lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00028))
                lstErrorParam1.Add(strInputTxtValue)
                lstErrorParam2.Add(String.Empty)
                imgErrorInput.Visible = True
                blnError = True
            End If

            ' Is integer?
            If Not blnError Then
                If Integer.TryParse(txtInput.Text, 0) = False Then
                    Dim strInputTxtValue As String = String.Empty

                    If IsExistValue(Field.CompareItem, FieldSetting.DescResource) Then
                        strInputTxtValue = Me.GetGlobalResourceObject("Text", GetSetting(Field.CompareItem, FieldSetting.DescResource))
                    End If

                    lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00365))
                    lstErrorParam1.Add(strInputTxtValue)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorInput.Visible = True
                    blnError = True
                End If

            End If

            ' Is within range?
            If Not blnError Then
                'Define the lower and upper limit
                Dim intLowerLimit As Nullable(Of Integer) = Nothing
                Dim intUpperLimit As Nullable(Of Integer) = Nothing

                'Load settings from DB
                If IsExistValue(Field.CompareValue, AdditionalFieldSetting.InputLowerLimit) _
                    AndAlso GetSetting(Field.CompareValue, AdditionalFieldSetting.InputLowerLimit) <> String.Empty Then
                    intLowerLimit = CInt(GetSetting(Field.CompareValue, AdditionalFieldSetting.InputLowerLimit))
                End If

                If IsExistValue(Field.CompareValue, AdditionalFieldSetting.InputUpperLimit) _
                    AndAlso GetSetting(Field.CompareValue, AdditionalFieldSetting.InputUpperLimit) <> String.Empty Then
                    intUpperLimit = CInt(GetSetting(Field.CompareValue, AdditionalFieldSetting.InputUpperLimit))
                End If

                'Load default settings if DB has no settings
                If intLowerLimit Is Nothing Or intUpperLimit Is Nothing Then
                    'Use the sum of all entitlement to be the upper limit of voucher amount for claim 
                    Dim dtmToday As DateTime = (New GeneralFunction).GetSystemDateTime().Date
                    Dim udtSchemeClaim As SchemeClaimModel = (New SchemeClaimBLL).getAllEffectiveSchemeClaim_WithSubsidizeGroup(dtmToday).Filter(SchemeClaimModel.HCVS)

                    intLowerLimit = 0
                    intUpperLimit = (New EHSTransactionBLL).getTotalGrantVoucher(udtSchemeClaim.SchemeCode, udtSchemeClaim.SubsidizeGroupClaimList(0).SubsidizeCode, dtmToday)
                End If

                'Validate the range
                Dim intInput As Integer = CInt(txtInput.Text)

                If intLowerLimit IsNot Nothing AndAlso intUpperLimit IsNot Nothing Then
                    If intInput < intLowerLimit Or intInput > intUpperLimit Then
                        Dim strInputTxtValue As String = String.Empty

                        If IsExistValue(Field.CompareItem, FieldSetting.DescResource) Then
                            strInputTxtValue = Me.GetGlobalResourceObject("Text", GetSetting(Field.CompareItem, FieldSetting.DescResource))
                        End If

                        lstError.Add(New SystemMessage(FunctCode.FUNT990000, SeverityCode.SEVE, MsgCode.MSG00366))
                        lstErrorParam1.Add(strInputTxtValue)
                        lstErrorParam2.Add(String.Empty)
                        imgErrorInput.Visible = True
                        blnError = True
                    End If
                End If

            End If

        End If

    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Input Value
        If IsExistValue(Field.CompareValue, FieldSetting.Visible) Then
            If GetSetting(Field.CompareValue, FieldSetting.Visible) = Condition.YES Then
                Dim strInputTextLabel As String = String.Format("{0} {1}", lblCompareItem.Text.Trim, lblOperator.Text.Trim)
                Dim strInputTxtValue As String = String.Empty

                If strInputTextLabel <> String.Empty AndAlso txtInput.Text <> String.Empty Then
                    strInputTxtValue = CInt(txtInput.Text.Trim).ToString
                    udtParameterList.AddParam(strInputTextLabel, strInputTxtValue)
                End If
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Input Value
        If IsExistValue(Field.CompareValue, FieldSetting.Visible) Then
            If GetSetting(Field.CompareValue, FieldSetting.Visible) = Condition.YES Then
                Dim strInputTextLabel As String = String.Format("{0} {1}", lblCompareItem.Text.Trim, lblOperator.Text.Trim)
                Dim strInputTxtValue As String = String.Empty

                If strInputTextLabel <> String.Empty AndAlso txtInput.Text <> String.Empty Then
                    strInputTxtValue = CInt(txtInput.Text.Trim).ToString
                    udtParameterList.AddParam(strInputTextLabel, strInputTxtValue)
                End If
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' Operator
        If IsExistValue(Field.CompareOperator, FieldSetting.Visible) Then
            If GetSetting(Field.CompareOperator, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.CompareOperator, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty
                    strSPParamName = GetSetting(Field.CompareOperator, FieldSetting.SPParamName)

                    If lblOperator.Text <> String.Empty Then
                        Dim strOperator As String = lblOperator.Text.Trim
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 2, strOperator)
                    End If

                End If

            ElseIf GetSetting(Field.CompareOperator, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.CompareOperator, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.CompareOperator, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.CompareOperator, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.CompareOperator, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.CompareOperator, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.VarChar, 2, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        ' Input Value
        If IsExistValue(Field.CompareValue, FieldSetting.Visible) Then
            If GetSetting(Field.CompareValue, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.CompareValue, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty
                    strSPParamName = GetSetting(Field.CompareValue, FieldSetting.SPParamName)

                    If txtInput.Text <> String.Empty Then
                        Dim strPassValue As String = CInt(txtInput.Text.Trim).ToString
                        udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Int, 2, strPassValue)
                    End If

                End If

            ElseIf GetSetting(Field.CompareValue, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.CompareValue, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.CompareValue, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.CompareValue, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.CompareValue, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.CompareValue, FieldSetting.SPParamName)
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
        If IsExistValue(Field.InputTextLabelWidth, FieldSetting.DescResource) Then
            lblText.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.InputTextLabelWidth, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.InputTextLabelWidth, FieldSetting.Visible) Then
            Select Case GetSetting(Field.InputTextLabelWidth, FieldSetting.Visible)
                Case Condition.YES
                    panInput.Visible = True
                Case Condition.NO
                    panInput.Visible = False
                Case Else
                    panInput.Visible = False
            End Select
        End If

        ' Set compare item
        If IsExistValue(Field.CompareItem, FieldSetting.DescResource) Then
            lblCompareItem.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.CompareItem, FieldSetting.DescResource))
        End If

        ' Set compare operator
        If IsExistValue(Field.CompareOperator, FieldSetting.DescResource) Then
            lblOperator.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.CompareOperator, FieldSetting.DescResource))
        End If

        ' Set default compare value
        If IsExistValue(Field.CompareValue, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.CompareValue, FieldSetting.DefaultValue) = String.Empty Then
                Me.txtInput.Text = GetSetting(Field.CompareValue, FieldSetting.DefaultValue)
            End If
        End If

    End Sub

#End Region

End Class