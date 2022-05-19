Imports System.Configuration
'Imports PHF.Constants
'Imports PHF.DataAccess
'Imports PHF.Log
'Imports PHF_iAMSmartService.Log
'Imports PHF.SetupBLL
'Imports PHF.Setup

Imports Common.Component
Imports Common.ComObject
Imports iAMSmartService.Log
Imports Common.ComObject.UserAgentInfoMapping

Namespace Service
    Public Class BaseService

#Region "Fields and Properties"
        Private _objAuditLog As AuditLogService
        Private _strFunctionCode As String = Common.Component.FunctCode.FUNT120101

        Public Property AuditLog() As AuditLogService
            Get
                Return _objAuditLog
            End Get
            Set(ByVal value As AuditLogService)
                _objAuditLog = value
            End Set
        End Property

#End Region

#Region "Functions & Methods"
        Function OnInit() As AuditLogService
            _objAuditLog = New AuditLogService(_strFunctionCode)

            'If ServiceSessionHandler.Base.AllEmpty Then
            '    UserAgentMappingSetupBLL.ConvertUserAgent(ServiceSessionHandler.Base.OS, ServiceSessionHandler.Base.Browser, ServiceSessionHandler.Base.UndefinedUserAgent)
            'End If
            If ServiceSessionHandler.Base.OS = UserAgentInfoModel.UA_Unknown OrElse ServiceSessionHandler.Base.Browser = UserAgentInfoModel.UA_Unknown Then

                ServiceSessionHandler.Base.UndefinedUserAgent = HttpContext.Current.Request.UserAgent

                If IsNothing(ServiceSessionHandler.Base.UndefinedUserAgent) Then ServiceSessionHandler.Base.UndefinedUserAgent = String.Empty

            End If

            Return _objAuditLog

        End Function

#End Region
    End Class


End Namespace
