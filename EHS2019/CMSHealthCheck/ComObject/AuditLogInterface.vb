Imports Common.ComObject
Imports Common.Component

''' <summary>
''' Common audit log creation interface for this project, create own audit log class for self module
''' </summary>
''' <remarks></remarks>
Public Class AuditLogInterface

    Public Enum EnumAuditLogModule
        CMSHealthCheck
    End Enum

    Private _AuditLogEntry As AuditLogEntry

    Public Shared Function GetAuditLogEntry(ByVal enumAuditLogModule As EnumAuditLogModule) As AuditLogBase
        Select Case enumAuditLogModule
            Case AuditLogInterface.EnumAuditLogModule.CMSHealthCheck
                Return New AuditLogCMSHealthCheck(ComConfig.FunctionCode.CMSHealthCheck, "DBFlag_dbEVS_InterfaceLog")

            Case Else
                Throw New Exception(String.Format("Undefined AuditLogModule ({0})", enumAuditLogModule.ToString))
        End Select
    End Function

End Class
