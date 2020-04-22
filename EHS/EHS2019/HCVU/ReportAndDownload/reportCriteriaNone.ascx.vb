Imports Common.ComFunction.ParameterFunction

Partial Public Class reportCriteriaNone
    Inherits System.Web.UI.UserControl
    Implements IReportCriteriaUC


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Public Function GetParameterString() As String Implements IReportCriteriaUC.GetParameterString
        Return ""
    End Function

    Public Function GetParameterList() As ParameterCollection Implements IReportCriteriaUC.GetParameterList
        Return New ParameterCollection()
    End Function

    Public Function GetCriteriaInput() As StoreProcParamCollection Implements IReportCriteriaUC.GetCriteriaInput
        Return New StoreProcParamCollection()
    End Function

    Public Function ValidateCriteriaInput(ByVal strReportID As String) As System.Collections.Generic.List(Of Common.ComObject.SystemMessage) Implements IReportCriteriaUC.ValidateCriteriaInput
        Return Nothing
    End Function

    Public Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As System.Collections.Generic.List(Of Common.ComObject.SystemMessage), ByRef lstErrorParam As System.Collections.Generic.List(Of String)) Implements IReportCriteriaUC.ValidateCriteriaInput

    End Sub
End Class