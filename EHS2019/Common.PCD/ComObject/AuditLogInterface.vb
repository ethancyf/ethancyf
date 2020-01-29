Imports Common.ComObject

''' <summary>
''' Common audit log creation interface for this project, create own audit log class for self module
''' </summary>
''' <remarks></remarks>
Public Class AuditLogInterface

    Public Enum EnumAuditLogModule
        PCDCheckIsActiveSP
        PCDCreatePCDSPAcct
        PCDTransferPracticeInfo
        PCDUploadEnrolInfo
        GetEHSPracticeScheme
        PCDCheckAvailableForVerifiedEnrolment
        PCDUploadVerifiedEnrolment
        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
        PCDCheckAccountStatus
        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---
    End Enum

    Private _AuditLogEntry As AuditLogEntry

    Public Shared Function GetAuditLogEntry(ByVal enumAuditLogModule As EnumAuditLogModule) As AuditLogBase
        Select Case enumAuditLogModule
            Case AuditLogInterface.EnumAuditLogModule.PCDCheckIsActiveSP
                Return New AuditLogPCDCheckIsActiveSP(ComConfig.FunctionCode.PCDCheckIsActiveSP, "DBFlag_dbEVS_InterfaceLog")

            Case AuditLogInterface.EnumAuditLogModule.PCDCreatePCDSPAcct
                Return New AuditLogPCDCreatePCDSPAcct(ComConfig.FunctionCode.PCDCreatePCDSPAcct, "DBFlag_dbEVS_InterfaceLog")

            Case AuditLogInterface.EnumAuditLogModule.PCDTransferPracticeInfo
                Return New AuditLogPCDTransferPracticeInfo(ComConfig.FunctionCode.PCDTransferPracticeInfo, "DBFlag_dbEVS_InterfaceLog")

            Case AuditLogInterface.EnumAuditLogModule.PCDUploadEnrolInfo
                Return New AuditLogPCDUploadEnrolInfo(ComConfig.FunctionCode.PCDUploadEnrolInfo, "DBFlag_dbEVS_InterfaceLog")

            Case AuditLogInterface.EnumAuditLogModule.GetEHSPracticeScheme
                Return New AuditLogGetEHSPracticeScheme(ComConfig.FunctionCode.GetEHSPracticeScheme, "DBFlag_dbEVS_InterfaceLog")

            Case AuditLogInterface.EnumAuditLogModule.PCDCheckAvailableForVerifiedEnrolment
                Return New AuditLogPCDCheckAvailableForVerifiedEnrolment(ComConfig.FunctionCode.PCDCheckAvailableForVerifiedEnrolment, "DBFlag_dbEVS_InterfaceLog")

            Case AuditLogInterface.EnumAuditLogModule.PCDUploadVerifiedEnrolment
                Return New AuditLogPCDUploadVerifiedEnrolment(ComConfig.FunctionCode.PCDUploadVerifiedEnrolment, "DBFlag_dbEVS_InterfaceLog")

                ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
            Case AuditLogInterface.EnumAuditLogModule.PCDCheckAccountStatus
                Return New AuditLogPCDCheckAccountStatus(ComConfig.FunctionCode.PCDCheckAccountStatus, "DBFlag_dbEVS_InterfaceLog")
                ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---

            Case Else
                Throw New Exception(String.Format("Undefined AuditLogModule ({0})", enumAuditLogModule.ToString))
        End Select
    End Function

End Class
