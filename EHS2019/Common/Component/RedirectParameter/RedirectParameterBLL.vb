Namespace Component.RedirectParameter

    Public Class RedirectParameterBLL

        Public Sub SaveToSession(ByVal udtRedirectParameter As RedirectParameterModel)
            System.Web.HttpContext.Current.Session(SessionVariableName) = udtRedirectParameter
        End Sub

        Public Function GetFromSession() As RedirectParameterModel
            Dim obj As Object = System.Web.HttpContext.Current.Session(SessionVariableName)

            If IsNothing(obj) Then
                Return Nothing
            Else
                Return CType(obj, RedirectParameterModel)
            End If

        End Function

        Public Sub RemoveFromSession()
            SaveReturnToSession(GetFromSession.ReturnParameter)
            System.Web.HttpContext.Current.Session.Remove(SessionVariableName)
        End Sub

        Public Sub SaveReturnToSession(ByVal udtRedirectParameter As RedirectParameterModel)
            System.Web.HttpContext.Current.Session(SessionVariableNameReturnParameter) = udtRedirectParameter
        End Sub

        Public Function GetReturnFromSession() As RedirectParameterModel
            Dim obj As Object = System.Web.HttpContext.Current.Session(SessionVariableNameReturnParameter)

            If IsNothing(obj) Then
                Return Nothing
            Else
                Return CType(obj, RedirectParameterModel)
            End If

        End Function

        Public Sub RemoveReturnFromSession()
            System.Web.HttpContext.Current.Session.Remove(SessionVariableNameReturnParameter)
        End Sub

        Public Sub WriteAuditLog(ByVal strFunctionCode As String, ByVal objWorkingData As ComInterface.IWorkingData, ByVal objRedirectParameter As RedirectParameterModel)
            Dim udtAuditLogEntry As New Common.ComObject.AuditLogEntry(strFunctionCode, objWorkingData)
            udtAuditLogEntry.AddDescripton("Parameter", objRedirectParameter.ToString)
            udtAuditLogEntry.WriteLog(LogID.LOG01130, "Redirect from another page")
        End Sub

        Private Const SessionVariableName As String = "CUSTOMLINKBUTTON_REDIRECTPARAMETER"
        Private Const SessionVariableNameReturnParameter As String = "CUSTOMLINKBUTTON_REDIRECTRETURNPARAMETER"

    End Class

End Namespace
