Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Format

Partial Public Class udcAgeRange
    Inherits StatisticsCriteriaUC
   
#Region "Variables"

#End Region

#Region "Session and Consts"

    Public Class Field
        Public Const MinAge As String = "MinAge"
        Public Const MaxAge As String = "MaxAge"
    End Class

    Public Class CustomFieldSetting
        Public Const RangeMin As String = "RangeMin"
        Public Const RangeMax As String = "RangeMax"
    End Class

#End Region

#Region "Page Event"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    End Sub

#End Region

#Region "Implement IStatisticsCriteriaUC"

    Public Overrides Sub Build(ByVal dicSetting As Dictionary(Of String, Dictionary(Of String, String)))
        MyBase.Build(dicSetting)

        'SetMinAgeErrorImageVisibility(False)
        'SetMaxAgeErrorImageVisibility(False)
        SetErrorComponentVisibility(False)
    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        Dim blnMinAgeMiss As Boolean = False
        Dim blnMaxAgeMiss As Boolean = False

        Dim blnMinAgeValid As Boolean = True
        Dim blnMaxAgeValid As Boolean = True

        Dim blnMissInput As Boolean = False
        Dim blnInvalid As Boolean = False
        Dim blnIncomplete As Boolean = False

        'SetMinAgeErrorImageVisibility(False)
        'SetMaxAgeErrorImageVisibility(False)
        SetErrorComponentVisibility(False)

        ' Check both are empty
        If IsExistValue(Field.MinAge, FieldSetting.Visible) AndAlso IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES AndAlso GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then

                If txtMinAge.Text.Trim = String.Empty AndAlso txtMaxAge.Text.Trim = String.Empty Then
                    'lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00007))
                    'lstErrorParam1.Add(GetSetting(Field.MinAge, FieldSetting.DescResource))
                    'lstErrorParam2.Add(GetSetting(Field.MaxAge, FieldSetting.DescResource))
                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003))
                    lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource)))
                    lstErrorParam2.Add(String.Empty)

                    blnMinAgeValid = False
                    blnMaxAgeValid = False

                    'imgErrorMinAge.Visible = True
                    'imgErrorMaxAge.Visible = True
                    imgErrorAge.Visible = True
                End If

            End If
        End If

        ' Min age
        If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Then
                ' Cannot empty (If max age is invisible)
                If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
                    If GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.NO Then
                        If txtMinAge.Text.Trim = String.Empty Then
                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003))
                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource)))
                            lstErrorParam2.Add(String.Empty)

                            blnMissInput = True
                            blnMinAgeValid = False
                            imgErrorAge.Visible = True
                        End If
                    End If
                End If

            End If
        End If

        ' Max age
        If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then
                ' Cannot empty (if min age is invisible)
                If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
                    If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.NO Then
                        If txtMaxAge.Text.Trim = String.Empty Then
                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00003))
                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MaxAge, FieldSetting.DescResource)))
                            lstErrorParam2.Add(String.Empty)

                            blnMissInput = True
                            blnMaxAgeValid = False
                            imgErrorAge.Visible = True
                        End If
                    End If
                End If

            End If
        End If

        'If blnMissInput Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00023))
        '    lstErrorParam1.Add("Age range")
        '    lstErrorParam2.Add(String.Empty)
        'End If

        'If blnInvalid Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00024))
        '    lstErrorParam1.Add("Age range")
        '    lstErrorParam2.Add(String.Empty)
        'End If

        'If blnIncomplete Then
        '    lstError.Add(New SystemMessage(FunctCode.FUNT010701, SeverityCode.SEVE, MsgCode.MSG00027))
        '    lstErrorParam1.Add("Age range")
        '    lstErrorParam2.Add(String.Empty)
        'End If

        ' Check min age boundary [Start]
        If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Then

                If txtMinAge.Text <> String.Empty AndAlso blnMinAgeValid Then
                    ' Range min
                    If IsExistValue(Field.MinAge, CustomFieldSetting.RangeMin) Then
                        Dim strRangeMin As String = GetSetting(Field.MinAge, CustomFieldSetting.RangeMin)
                        If Not strRangeMin Is String.Empty Then
                            Dim intValidationMinAge As Integer = CType(strRangeMin, Integer)

                            Dim intInputRangeMin As Integer = CType(txtMinAge.Text.Trim, Integer)
                            If intInputRangeMin.CompareTo(intValidationMinAge) < 0 Then
                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00010))
                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource)))
                                lstErrorParam2.Add(intValidationMinAge.ToString)
                                'imgErrorMinAge.Visible = True
                                imgErrorAge.Visible = True
                            End If
                        End If
                    End If

                    ' Range max
                    If IsExistValue(Field.MinAge, CustomFieldSetting.RangeMax) Then
                        Dim strRangeMax As String = GetSetting(Field.MinAge, CustomFieldSetting.RangeMax)
                        If Not strRangeMax Is String.Empty Then
                            Dim intValidationMaxAge As Integer = CType(strRangeMax, Integer)

                            Dim intInputRangeMax As Integer = CType(txtMinAge.Text.Trim, Integer)
                            If intInputRangeMax.CompareTo(intValidationMaxAge) > 0 Then
                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00011))
                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource)))
                                lstErrorParam2.Add(intValidationMaxAge.ToString)
                                'imgErrorMinAge.Visible = True
                                imgErrorAge.Visible = True
                            End If
                        End If
                    End If

                End If

            End If
        End If
        ' Check min age boundary [End]

        ' Check max age boundary [Start]
        If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then

                If txtMaxAge.Text <> String.Empty AndAlso blnMaxAgeValid Then
                    ' Range min
                    If IsExistValue(Field.MaxAge, CustomFieldSetting.RangeMin) Then
                        Dim strRangeMin As String = GetSetting(Field.MaxAge, CustomFieldSetting.RangeMin)
                        If Not strRangeMin Is String.Empty Then
                            Dim intValidationMinAge As Integer = CType(strRangeMin, Integer)

                            Dim intInputRangeMin As Integer = CType(txtMaxAge.Text.Trim, Integer)
                            If intInputRangeMin.CompareTo(intValidationMinAge) < 0 Then
                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00010))
                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MaxAge, FieldSetting.DescResource)))
                                lstErrorParam2.Add(intValidationMinAge.ToString)
                                'imgErrorMaxAge.Visible = True
                                imgErrorAge.Visible = True
                            End If
                        End If
                    End If

                    ' Range max
                    If IsExistValue(Field.MaxAge, CustomFieldSetting.RangeMax) Then
                        Dim strRangeMax As String = GetSetting(Field.MaxAge, CustomFieldSetting.RangeMax)
                        If Not strRangeMax Is String.Empty Then
                            Dim intValidationMaxAge As Integer = CType(strRangeMax, Integer)

                            Dim intInputRangeMax As Integer = CType(txtMaxAge.Text.Trim, Integer)
                            If intInputRangeMax.CompareTo(intValidationMaxAge) > 0 Then
                                lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00011))
                                lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MaxAge, FieldSetting.DescResource)))
                                lstErrorParam2.Add(intValidationMaxAge.ToString)
                                'imgErrorMaxAge.Visible = True
                                imgErrorAge.Visible = True
                            End If
                        End If
                    End If

                End If

            End If
        End If
        ' Check max age boundary [End]

        ' Check relation between min age and max age [Start]
        If IsExistValue(Field.MinAge, FieldSetting.Visible) AndAlso IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES AndAlso GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then

                If blnMinAgeValid AndAlso blnMaxAgeValid Then
                    Dim intMinAge As Integer = 0
                    Dim intMaxAge As Integer = 0

                    ' Min age
                    If Not txtMinAge.Text.Trim Is String.Empty Then
                        intMinAge = CType(txtMinAge.Text.Trim, Integer)
                    End If

                    ' Max age
                    If Not txtMaxAge.Text.Trim Is String.Empty Then
                        intMaxAge = CType(txtMaxAge.Text.Trim, Integer)
                    End If

                    ' Check: Min age cannot later than Max age (if two value have input)
                    If Not txtMinAge.Text.Trim Is String.Empty AndAlso Not txtMaxAge.Text.Trim Is String.Empty Then
                        If intMinAge.CompareTo(intMaxAge) > 0 Then
                            lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00011))
                            lstErrorParam1.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource)) + " From")
                            lstErrorParam2.Add(Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource)) + " To")

                            'imgErrorMinAge.Visible = True
                            'imgErrorMaxAge.Visible = True
                            imgErrorAge.Visible = True
                        End If
                    End If
                    
                End If

            End If
        End If
        ' Check relation between min age and max age [End]

    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strAgeRange As String = String.Empty
        Dim strAgeRangeLabel As String = Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource))

        ' Min age
        If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Then
                If txtMinAge.Text.Trim = String.Empty Then
                    strAgeRange += String.Empty
                Else
                    strAgeRange += CType(txtMinAge.Text.Trim, Integer).ToString
                End If
            End If
        End If

        ' Max age
        If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then
                strAgeRange += " to "

                If txtMaxAge.Text.Trim = String.Empty Then
                    strAgeRange += String.Empty
                Else
                    strAgeRange += CType(txtMaxAge.Text.Trim, Integer).ToString
                End If
            End If
        End If

        udtParameterList.AddParam(strAgeRangeLabel, strAgeRange)

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strAgeRange As String = String.Empty

        ' Min age
        If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Then
                If txtMinAge.Text.Trim = String.Empty Then
                    strAgeRange += "0"
                Else
                    strAgeRange += CType(txtMinAge.Text.Trim, Integer).ToString
                End If
            End If
        End If

        ' Max age
        If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then
                strAgeRange += " to "

                If txtMaxAge.Text.Trim = String.Empty Then
                    strAgeRange += "Max Age"
                Else
                    strAgeRange += CType(txtMaxAge.Text.Trim, Integer).ToString
                End If

            End If
        End If

        If IsExistValue(Field.MinAge, FieldSetting.Visible) Or IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Or GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then

                Dim strAgeRangeLabel As String = Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource))
                udtParameterList.AddParam(strAgeRangeLabel, strAgeRange)

            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' Min age
        If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
            If GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.YES Then

                Dim intMinAge As Integer = 0
                If IsExistValue(Field.MinAge, FieldSetting.SPParamName) Then
                    Dim strParamMinAge As String = String.Empty
                    strParamMinAge = GetSetting(Field.MinAge, FieldSetting.SPParamName)

                    If txtMinAge.Text.Trim Is String.Empty Then
                        udtStoreProcParamList.AddParam(strParamMinAge, System.Data.SqlDbType.Int, 2, intMinAge)
                    Else
                        intMinAge = CType(txtMinAge.Text.Trim, Integer)
                        udtStoreProcParamList.AddParam(strParamMinAge, System.Data.SqlDbType.Int, 2, intMinAge)
                    End If

                End If

            ElseIf GetSetting(Field.MinAge, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.MinAge, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.MinAge, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.MinAge, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As Integer = 0
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = CType(GetSetting(Field.MinAge, FieldSetting.DefaultValue), Integer)
                            strSPParamName = GetSetting(Field.MinAge, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Int, 2, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        ' Max age
        If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            If GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.YES Then

                Dim intMaxAge As Object
                If IsExistValue(Field.MaxAge, FieldSetting.SPParamName) Then
                    Dim strParamMaxAge As String = String.Empty
                    strParamMaxAge = GetSetting(Field.MaxAge, FieldSetting.SPParamName)

                    If txtMaxAge.Text.Trim Is String.Empty Then
                        udtStoreProcParamList.AddParam(strParamMaxAge, System.Data.SqlDbType.Int, 2, Nothing)
                    Else
                        intMaxAge = CType(txtMaxAge.Text.Trim, Integer)
                        udtStoreProcParamList.AddParam(strParamMaxAge, System.Data.SqlDbType.Int, 2, intMaxAge)
                    End If

                End If

            ElseIf GetSetting(Field.MaxAge, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.MaxAge, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.MaxAge, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.MaxAge, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As Integer = 0
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = CType(GetSetting(Field.MaxAge, FieldSetting.DefaultValue), Integer)
                            strSPParamName = GetSetting(Field.MaxAge, FieldSetting.SPParamName)

                            If strDefaultValue > 999 Then
                                udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 2, Nothing)
                            Else
                                udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 2, CType(strDefaultValue, Integer))
                            End If

                        End If

                    End If
                End If

            End If
        End If


        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        imgErrorAge.Visible = blnVisible
    End Sub

#End Region

#Region "Supporting Function"

    Public Overrides Sub InitControl()
        ' Set item - Min Age
        SetMinAge()
        ' Set item - Max Age
        SetMaxAge()

    End Sub

#End Region

#Region "Field Setting"

    ' Set item - Min Age 
    Private Sub SetMinAge()
        ' Set field description
        If IsExistValue(Field.MinAge, FieldSetting.DescResource) Then
            lblMinAge.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.MinAge, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.MinAge, FieldSetting.Visible) Then
            Select Case GetSetting(Field.MinAge, FieldSetting.Visible)
                Case Condition.YES
                    panMinAge.Visible = True
                Case Condition.NO
                    panMinAge.Visible = False
                Case Else
                    panMinAge.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.MinAge, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.MinAge, FieldSetting.DefaultValue) = String.Empty Then
                txtMinAge.Text = GetSetting(Field.MinAge, FieldSetting.DefaultValue)
            End If
        End If

    End Sub

    ' Set item - Max Age
    Private Sub SetMaxAge()
        ' Set field description
        If IsExistValue(Field.MaxAge, FieldSetting.DescResource) Then
            lblMaxAge.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.MaxAge, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.MaxAge, FieldSetting.Visible) Then
            Select Case GetSetting(Field.MaxAge, FieldSetting.Visible)
                Case Condition.YES
                    panMaxAge.Visible = True
                Case Condition.NO
                    panMaxAge.Visible = False
                Case Else
                    panMaxAge.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.MaxAge, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.MaxAge, FieldSetting.DefaultValue) = String.Empty Then
                txtMaxAge.Text = GetSetting(Field.MaxAge, FieldSetting.DefaultValue)
            End If
        End If

    End Sub

#End Region
    
End Class