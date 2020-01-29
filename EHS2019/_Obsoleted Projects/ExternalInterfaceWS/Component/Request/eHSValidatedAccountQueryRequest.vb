Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports Common.Component
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Validation
Imports ExternalInterfaceWS.Component.Request.Base
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports ExternalInterfaceWS.ComObject


Namespace Component.Request

    Public Class eHSValidatedAccountQueryRequest
        Inherits BaseWSAccountRequest

#Region "Constructor"

        Public Sub New(ByVal xmlRequest As String, ByRef udtAuditLog As ExtAuditLogEntry)
            Dim xml As New XmlDocument()
            'For Logging
            Me.ExtAuditLogEntry = udtAuditLog

            Try
                '---------------------------------
                '(Step 1) Read XML
                '---------------------------------
                xml.LoadXml(xmlRequest)
            Catch ex As Exception
                Me._bIsValid = False
                Me.Errors.Add(ErrorCodeList.I00003)
                Exit Sub
            End Try

            Try
                'Message ID
                ReadMessageIDandValidate(xml, Me.Errors)
                'Assign Message ID to Audit Log
                If Me.Errors.Count = 0 Then
                    udtAuditLog.MessageID = _strMessageID
                End If

                'SP Info
                ReadSPInfo(xml, Me.Errors)
                'Assign SP ID to Audit Log
                If Not SPID Is Nothing AndAlso Not SPID.Trim = String.Empty Then
                    udtAuditLog.UserID = SPID
                End If
                WriteLogWithErrorList(LogID.LOG00030)

                'Account Info
                ReadAccountInfo(xml, Me.Errors)
                WriteLogWithErrorList(LogID.LOG00031)
                '---------------------------------
                '(Step 2) Check whether there is missing or duplicate fields 
                '---------------------------------
                If Me.Errors.Count = 0 Then
                    Me._bIsValid = CheckSPXMLField(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00032)
                End If

                If Me._bIsValid Then
                    Me._bIsValid = CheckEHSAccountXMLField(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00033)
                End If
                '---------------------------------
                '(Step 3) Check eHS account fields format & SP info
                '---------------------------------
                If Me._bIsValid Then
                    Me._bIsValid = ValidatServiceProviderInfo(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00034)
                End If

                If Me._bIsValid Then
                    Me._bIsValid = ValidateEHSAccountInfo(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00035)
                End If

            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.eHSValidatedAccountQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                Me.Errors.Add(ErrorCodeList.I99999)
                Me._bIsValid = False
            End Try

        End Sub

#End Region

    End Class

End Namespace


