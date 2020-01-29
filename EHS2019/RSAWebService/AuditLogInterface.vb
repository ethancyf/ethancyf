Imports Common.ComObject

''' <summary>
''' Common audit log creation interface for this project, create own audit log class for self module
''' </summary>
''' <remarks></remarks>
Public Class AuditLogInterface

    Private _AuditLogEntry As AuditLogEntry

    Public Shared Function GetAuditLogEntry() As AuditLogBase
        Return New AuditLogRSAWS(Common.Component.FunctCode.FUNT060301, Common.Component.DBFlagStr.DBFlag2)
    End Function


End Class
