Imports Common.ComObject


Namespace ComObject

    Public Enum EnumAuditLog
        UploadClaim
        GetReasonForVisitList
        RCHNameQuery
        eHSValidatedAccountQuery
        eHSAccountSubsidyQuery
        SPPracticeValidation
    End Enum

    Public Class ComFunction

        Public Shared Function GetAuditLogEntry(ByVal auditFunc As EnumAuditLog) As AuditLogEntry

            Select Case auditFunc
                Case EnumAuditLog.UploadClaim
                    Return New ExtAuditLogEntry(Common.Component.FunctCode.FUNT070101, Common.Component.DBFlagStr.DBFlag2)
                Case EnumAuditLog.GetReasonForVisitList
                    Return New ExtAuditLogEntry(Common.Component.FunctCode.FUNT070102, Common.Component.DBFlagStr.DBFlag2)
                Case EnumAuditLog.RCHNameQuery
                    Return New ExtAuditLogEntry(Common.Component.FunctCode.FUNT070103, Common.Component.DBFlagStr.DBFlag2)
                Case EnumAuditLog.eHSValidatedAccountQuery
                    Return New ExtAuditLogEntry(Common.Component.FunctCode.FUNT070104, Common.Component.DBFlagStr.DBFlag2)
                Case EnumAuditLog.eHSAccountSubsidyQuery
                    Return New ExtAuditLogEntry(Common.Component.FunctCode.FUNT070105, Common.Component.DBFlagStr.DBFlag2)
                Case EnumAuditLog.SPPracticeValidation
                    Return New ExtAuditLogEntry(Common.Component.FunctCode.FUNT070106, Common.Component.DBFlagStr.DBFlag2)
                Case Else
                    Return New ExtAuditLogEntry(Common.Component.FunctCode.FUNT070101, Common.Component.DBFlagStr.DBFlag2)
            End Select
        End Function

    End Class

End Namespace




