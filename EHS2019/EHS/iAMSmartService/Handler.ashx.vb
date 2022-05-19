Imports eID.Bussiness
Imports eID.Bussiness.Impl
Imports eID.Bussiness.Interface
Imports eService.Common
Imports eService.DTO.Enum
Imports eService.DTO.Request
Imports eService.DTO.Response
Imports Org.BouncyCastle.Asn1
Imports Org.BouncyCastle.Asn1.Pkcs
Imports Org.BouncyCastle.Asn1.X509
Imports Org.BouncyCastle.Crypto
Imports Org.BouncyCastle.Crypto.Parameters
Imports Org.BouncyCastle.Math
Imports Org.BouncyCastle.Pkcs
Imports Org.BouncyCastle.X509
Imports System.IO
Imports System.Security.Cryptography
Imports System.Security.Cryptography.X509Certificates
Imports System.Web

Imports Common.Component
Imports Common.DataAccess
Imports iAMSmartService.BLL.IAMSmartService
Imports iAMSmartService.Log
Imports iAMSmartService.Service
Imports Common.ComObject
Imports Common

Namespace iAMSmartService

    Public Class Handler
        Inherits BaseService
        Implements System.Web.IHttpHandler

        Sub ProcessRequest(ByVal context As HttpContext) Implements IHttpHandler.ProcessRequest

            Dim udtAuditLogEntry As New AuditLogEntry(FunctCode.FUNT070501, DBFlagStr.DBFlagInterfaceLog)

            udtAuditLogEntry.WriteStartLog(LogID.LOG00000, "[iAMSmart>EHS] Receive callback")

            Me.OnInit()

            Try
                Dim strResult As String = String.Empty

                If context.Request.Url.AbsoluteUri.ToLower.Contains("authcallback") Then
                    strResult = (New IAMSmartServiceBLL).HandleAuthCallback(context)
                ElseIf context.Request.Url.AbsoluteUri.ToLower.Contains("profilecallback") Then
                    strResult = (New IAMSmartServiceBLL).HandleProfileCallback(context)
                ElseIf context.Request.Url.AbsoluteUri.ToLower.Contains("getstate") Then
                    strResult = (New IAMSmartServiceBLL).GetState(context)
                Else
                    'check the broswer Type
                End If
            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("ex", ex.ToString)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "[iAMSmart>EHS] Callback fail")
                Throw
            End Try
            udtAuditLogEntry.WriteEndLog(LogID.LOG00001, "[iAMSmart>EHS] Callback end")
        End Sub

#Region "Fields and Properties"
        ReadOnly Property IsReusable() As Boolean Implements IHttpHandler.IsReusable
            Get
                Return False
            End Get
        End Property
#End Region
    End Class

End Namespace
