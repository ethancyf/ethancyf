Imports Common.ComFunction.ParameterFunction
Imports Common.Component.FileGeneration

Partial Public Class StatisticsCriteriaBase
    Inherits System.Web.UI.UserControl

    Public Sub Build(ByVal strXMLString As String)
        phStatisticsCriteria.Controls.Clear()

        Dim udcCreator As New Creator
        udcCreator.ControlPath = "~/UIControl/Statistics/"
        Dim newList As List(Of Control) = udcCreator.CreateControl(strXMLString)
        For Each conItem As Control In newList
            phStatisticsCriteria.Controls.Add(conItem)
        Next

    End Sub

    Public Sub ValidateCriteriaInput(ByVal strStatisticsID As String, ByRef lstError As List(Of Common.ComObject.SystemMessage), ByRef lstErrorParam1 As List(Of String), ByRef lstErrorParam2 As List(Of String))
        For Each con As Control In phStatisticsCriteria.Controls
            CType(con, IStatisticsCriteriaUC).ValidateCriteriaInput(strStatisticsID, lstError, lstErrorParam1, lstErrorParam2)
        Next
    End Sub

    Public Function ValidateCriteriaInput(ByVal strStatisticsID As String) As List(Of Common.ComObject.SystemMessage)
        Throw New NotImplementedException("ValidateCriteriaInput")
    End Function

    Public Function GetParameterString() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        For Each con As Control In phStatisticsCriteria.Controls
            For Each udtParameter As ParameterObject In CType(con, IStatisticsCriteriaUC).GetParameterString
                udtParameterList.Add(udtParameter)
            Next
        Next

        Return udtParameterList
    End Function

    Public Function GetParameterList() As ParameterCollection
        Dim udtParameterList As New ParameterCollection

        For Each con As Control In phStatisticsCriteria.Controls
            For Each udtParameter As ParameterObject In CType(con, IStatisticsCriteriaUC).GetParameterList
                udtParameterList.Add(udtParameter)
            Next
        Next

        Return udtParameterList
    End Function

    Public Function GetCriteriaInput() As StoreProcParamCollection
        Dim udtStoreProcParamList As New StoreProcParamCollection

        For Each con As Control In phStatisticsCriteria.Controls
            For Each udtStoreProcParam As StoreProcParamObject In CType(con, IStatisticsCriteriaUC).GetCriteriaInput
                udtStoreProcParamList.Add(udtStoreProcParam)
            Next
        Next

        Return udtStoreProcParamList
    End Function

    Public Sub SetErrorComponentVisibility(blnVisible As Boolean)
        For Each con As Control In phStatisticsCriteria.Controls
            DirectCast(con, IStatisticsCriteriaUC).SetErrorComponentVisibility(blnVisible)
        Next
    End Sub

    Public Sub Clear()
        Me.phStatisticsCriteria.Controls.Clear()
    End Sub

    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [Start][Winnie]
    ' ----------------------------------------------------------------------------------------
    Public Function GetReportGenerationDate() As DateTime?
        Dim dtmReportGenDate As DateTime? = Nothing

        For Each con As Control In phStatisticsCriteria.Controls
            Dim dtmCompareDate As DateTime? = CType(con, IStatisticsCriteriaUC).GetReportGenerationDate

            If dtmCompareDate.HasValue Then
                If dtmReportGenDate.HasValue Then
                    If dtmCompareDate.Value > dtmReportGenDate.Value Then
                        dtmReportGenDate = dtmCompareDate
                    End If
                Else
                    dtmReportGenDate = dtmCompareDate
                End If
            End If
        Next

        Return dtmReportGenDate
    End Function
    ' [CRE18-015] (Enable PCV13 weekly report eHS(S)W003 upon request) [End][Winnie]
End Class