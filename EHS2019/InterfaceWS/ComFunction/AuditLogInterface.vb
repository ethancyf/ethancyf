Imports Common.ComObject

''' <summary>
''' Common audit log creation interface for this project, create own audit log class for self module
''' </summary>
''' <remarks></remarks>
Public Class AuditLogInterface

    Public Enum EnumAuditLogModule
        EHSVaccination
        TokenReplacementIsCommonUser
        TokenReplacementReplaceToken
        PCDInterfaceGetEHSPracticeScheme
    End Enum

    Private _AuditLogEntry As AuditLogEntry

    'Public Sub New(ByVal enumAuditLogModule As EnumAuditLogModule)
    '    Select Case enumAuditLogModule
    '        Case AuditLogInterface.EnumAuditLogModule.EHSVaccination
    '            _AuditLogEntry = New AuditLogBase(ComConfig.FunctionCode.EHSVaccination, Common.Component.DBFlagStr.DBFlag2)
    '    End Select
    'End Sub

    Public Shared Function GetAuditLogEntry(ByVal enumAuditLogModule As EnumAuditLogModule) As AuditLogBase
        Select Case enumAuditLogModule
            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Koala CHENG]
            ' ----------------------------------------------------------
            Case AuditLogInterface.EnumAuditLogModule.EHSVaccination
                Return New AuditLogEHSVaccination(ComConfig.FunctionCode.EHSVaccination, Common.Component.DBFlagStr.DBFlag2)
                'Return New AuditLogEHSVaccination(ComConfig.FunctionCode.EHSVaccination, Common.Component.DBFlagStr.DBFlag2)
                'Case AuditLogInterface.EnumAuditLogModule.TokenReplacementIsCommonUser
                '    Return New AuditLogTokenReplacementIsCommonUser(ComConfig.FunctionCode.TokenReplacementIsCommonUser, Common.Component.DBFlagStr.DBFlag2)
                'Case AuditLogInterface.EnumAuditLogModule.TokenReplacementReplaceToken
                '    Return New AuditLogTokenReplacementReplaceToken(ComConfig.FunctionCode.TokenReplacementReplaceToken, Common.Component.DBFlagStr.DBFlag2)
                ' CRE18-004 (CIMS Vaccination Sharing) [End][Koala CHENG]
            Case AuditLogInterface.EnumAuditLogModule.PCDInterfaceGetEHSPracticeScheme
                Return New AuditLogGetEHSPracticeScheme(Common.PCD.ComConfig.FunctionCode.GetEHSPracticeScheme, Common.Component.DBFlagStr.DBFlag2)
            Case Else
                Throw New Exception(String.Format("Undefined AuditLogModule ({0})", enumAuditLogModule.ToString))
        End Select
    End Function

    '#Region "Public Function"

    '    Public Sub WriteLog(ByVal strLogID As String)
    '        _AuditLogEntry.WriteLog(strLogID)
    '    End Sub

    '    ''' <summary>
    '    ''' Write log with raw data, e.g. Interface module log input and output xml
    '    ''' </summary>
    '    ''' <param name="strLogID"></param>
    '    ''' <param name="strData"></param>
    '    ''' <remarks></remarks>
    '    Public Sub WriteLogData(ByVal strLogID As String, ByVal strData As String)
    '        _AuditLogEntry.WriteLog(strLogID, )
    '    End Sub
    '#End Region

End Class
