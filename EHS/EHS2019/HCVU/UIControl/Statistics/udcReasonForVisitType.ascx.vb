Imports Common.ComFunction.ParameterFunction
Imports Common.ComObject
Imports Common.Component
Imports Common.Component.FileGeneration
Imports Common.Format

Partial Public Class udcReasonForVisitType
    Inherits StatisticsCriteriaUC

#Region "Variables"

    'Private _dicSetting As New Dictionary(Of String, Dictionary(Of String, String))

#End Region

#Region "Session and Const"

    Public Class Field
        Public Const ReasonForVisitType As String = "ReasonForVisitType"
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

        ' Build reason for visit component
        BuildReasonForVisitComponent(Me.ddlReasonForVisit)
        MyBase.Build(dicSetting)

        ' Initial control
        'InitControl()

    End Sub

    Public Overrides Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As List(Of SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        SetErrorComponentVisibility(False)

        ' Reason for visit
        If IsExistValue(Field.ReasonForVisitType, FieldSetting.Visible) Then
            If GetSetting(Field.ReasonForVisitType, FieldSetting.Visible) = Condition.YES Then
                ' Check dropdown (Reason for visit)
                If ddlReasonForVisit.SelectedValue.Trim = DROP_DOWN_EMPTY Then
                    Dim strReasonForVisitText As String = String.Empty

                    If IsExistValue(Field.ReasonForVisitType, FieldSetting.DescResource) Then
                        strReasonForVisitText = Me.GetGlobalResourceObject("Text", GetSetting(Field.ReasonForVisitType, FieldSetting.DescResource))
                    Else
                        strReasonForVisitText = Me.GetGlobalResourceObject("Text", "ReasonVisit")
                    End If

                    lstError.Add(New SystemMessage(FUNCTION_CODE, SeverityCode.SEVE, MsgCode.MSG00002))
                    lstErrorParam1.Add(strReasonForVisitText)
                    lstErrorParam2.Add(String.Empty)
                    imgErrorReasonForVisit.Visible = True
                End If

            End If
        End If

    End Sub

    Public Overrides Function ValidateCriteriaInput(ByVal strReportID As String) As List(Of SystemMessage)
        Throw New NotImplementedException
    End Function

    Public Overrides Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection
        Dim strReasonForVisitTypeValue As String = String.Empty

        ' Reason for visit
        If IsExistValue(Field.ReasonForVisitType, FieldSetting.Visible) Then
            If GetSetting(Field.ReasonForVisitType, FieldSetting.Visible) = Condition.YES Then
                Dim strReasonForVisit As String = lblReasonForVisit.Text.Trim
                If ddlReasonForVisit.SelectedIndex = 0 Then
                    strReasonForVisitTypeValue += String.Empty
                Else
                    strReasonForVisitTypeValue += ddlReasonForVisit.SelectedValue.Trim.ToString
                End If

                udtParameterList.AddParam(strReasonForVisit, strReasonForVisitTypeValue)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        ' Reason for visit
        If IsExistValue(Field.ReasonForVisitType, FieldSetting.Visible) Then
            If GetSetting(Field.ReasonForVisitType, FieldSetting.Visible) = Condition.YES Then
                Dim strReasonForVisit As String = lblReasonForVisit.Text.Trim
                udtParameterList.AddParam(strReasonForVisit, ddlReasonForVisit.SelectedItem.Text.Trim)
            End If
        End If

        Return udtParameterList
    End Function

    Public Overrides Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        ' Reason for visit
        If IsExistValue(Field.ReasonForVisitType, FieldSetting.Visible) Then
            If GetSetting(Field.ReasonForVisitType, FieldSetting.Visible) = Condition.YES Then

                If IsExistValue(Field.ReasonForVisitType, FieldSetting.SPParamName) Then
                    Dim strSPParamName As String = String.Empty

                    strSPParamName = GetSetting(Field.ReasonForVisitType, FieldSetting.SPParamName)
                    udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, ddlReasonForVisit.SelectedValue.Trim)
                End If

            ElseIf GetSetting(Field.ReasonForVisitType, FieldSetting.Visible) = Condition.NO Then

                If IsExistValue(Field.ReasonForVisitType, FieldSetting.DefaultValue) Then
                    If Not GetSetting(Field.ReasonForVisitType, FieldSetting.DefaultValue) Is String.Empty Then

                        If IsExistValue(Field.ReasonForVisitType, FieldSetting.SPParamName) Then
                            Dim strDefaultValue As String = String.Empty
                            Dim strSPParamName As String = String.Empty

                            strDefaultValue = GetSetting(Field.ReasonForVisitType, FieldSetting.DefaultValue)
                            strSPParamName = GetSetting(Field.ReasonForVisitType, FieldSetting.SPParamName)
                            udtStoreProcParamList.AddParam(strSPParamName, System.Data.SqlDbType.Char, 10, strDefaultValue)
                        End If

                    End If
                End If

            End If
        End If

        Return udtStoreProcParamList
    End Function

    Public Overrides Sub SetErrorComponentVisibility(ByVal blnVisible As Boolean)
        imgErrorReasonForVisit.Visible = blnVisible
    End Sub

#End Region

#Region "Supporting Function"

    Protected Overrides Sub BuildReasonForVisitComponent(ByVal ddlComponent As DropDownList)
        MyBase.BuildReasonForVisitComponent(ddlComponent)
    End Sub

    Public Overrides Sub InitControl()
        ' Set item - Reason for visit    
        SetReasonForVisit()

    End Sub

#End Region

#Region "Fields Setting"

    ' Set item - Reason for visit
    Private Sub SetReasonForVisit()
        ' Set field description
        If IsExistValue(Field.ReasonForVisitType, FieldSetting.DescResource) Then
            lblReasonForVisit.Text = Me.GetGlobalResourceObject("Text", GetSetting(Field.ReasonForVisitType, FieldSetting.DescResource))
        End If

        ' Set field visibility
        If IsExistValue(Field.ReasonForVisitType, FieldSetting.Visible) Then
            Select Case GetSetting(Field.ReasonForVisitType, FieldSetting.Visible)
                Case Condition.YES
                    panReasonForVisit.Visible = True
                Case Condition.NO
                    panReasonForVisit.Visible = False
                Case Else
                    panReasonForVisit.Visible = False
            End Select
        End If

        ' Set default value
        If IsExistValue(Field.ReasonForVisitType, FieldSetting.DefaultValue) Then
            If Not GetSetting(Field.ReasonForVisitType, FieldSetting.DefaultValue) = String.Empty Then

                Dim listItem As ListItem = ddlReasonForVisit.Items.FindByValue(GetSetting(Field.ReasonForVisitType, FieldSetting.DefaultValue))
                If Not listItem Is Nothing Then
                    ddlReasonForVisit.SelectedValue = listItem.Value
                End If

            End If
        End If

    End Sub

#End Region

End Class