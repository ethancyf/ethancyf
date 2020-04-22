Imports Common.ComObject
Imports Common.ComFunction

Namespace EHSVaccination

    Public Class ComFunction

        Private Const SYS_PARAM_ENABLE_MSG_ID As String = "CMS_Get_Vaccine_WS_Enable_MessageID" ' CRE10-035

        Public Shared Function GetAuditLogEntry() As AuditLogEntry
            Return New AuditLogEntry(ComConfig.FunctionCode.EHSVaccination, Common.Component.DBFlagStr.DBFlag2)
        End Function

        ''' <summary>
        ''' CRE10-035
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function EnableMessageID() As Boolean
            Dim oGenFunc As New GeneralFunction()
            Dim sValue As String = String.Empty
            If oGenFunc.getSystemParameter(SYS_PARAM_ENABLE_MSG_ID, sValue, String.Empty) Then
                If sValue = "Y" Then
                    Return True
                Else
                    Return False
                End If
            Else
                Throw New Exception(String.Format("Fail to query SystemParameters[{0}]", SYS_PARAM_ENABLE_MSG_ID))
            End If
        End Function
    End Class

End Namespace
