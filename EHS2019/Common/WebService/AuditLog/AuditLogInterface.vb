Imports Common.ComObject
Imports Common.Component

''' <summary>
''' Common audit log creation interface for this project, create own audit log class for self module
''' </summary>
''' <remarks></remarks>
Public Class AuditLogInterface

    Public Enum EnumAuditLogModule
        EVACC_CMS
        EVACC_CIMS
        CMSHealthCheck
        CIMSHealthCheck
    End Enum

    Private _AuditLogEntry As AuditLogEntry

    Public Shared Function GetAuditLogEntry(ByVal enumAuditLogModule As EnumAuditLogModule) As AuditLogBase
        Select Case enumAuditLogModule
            Case AuditLogInterface.EnumAuditLogModule.EVACC_CMS
                Return New AuditLogEVacc(FunctCode.FUNT060104, DBFlagStr.DBFlagInterfaceLog)

            Case AuditLogInterface.EnumAuditLogModule.EVACC_CIMS
                Return New AuditLogEVacc(FunctCode.FUNT060105, DBFlagStr.DBFlagInterfaceLog)

            Case AuditLogInterface.EnumAuditLogModule.CMSHealthCheck
                Return New AuditLogCMSHealthCheck(FunctCode.FUNT060102, DBFlagStr.DBFlagInterfaceLog)

            Case AuditLogInterface.EnumAuditLogModule.CIMSHealthCheck
                Return New AuditLogCMSHealthCheck(FunctCode.FUNT060103, DBFlagStr.DBFlagInterfaceLog)

            Case Else
                Throw New Exception(String.Format("Undefined AuditLogModule ({0})", enumAuditLogModule.ToString))
        End Select
    End Function

End Class
