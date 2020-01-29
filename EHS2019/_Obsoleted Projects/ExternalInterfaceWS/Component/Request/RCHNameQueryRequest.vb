Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports ExternalInterfaceWS.Component.Request.Base
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports ExternalInterfaceWS.ComObject
Imports Common.Component

Namespace Component.Request

    Public Class RCHNameQueryRequest
        Inherits BaseWSSPRequest

#Region "Private Constant"

        Private Const TAG_RCH_CODE As String = "RCHCode"

        'Private Const ERR_TAG_NOT_FOUND As String = "{0} tag not found"
        'Private Const ERR_TAG_DUPLICATE As String = "Duplicate {0} tag found"
        'Private Const ERR_TAG_INVALID_VALUE As String = "Invalid {0} tag value"
        'Private Const ERR_ITEM_NOT_MATCH_COUNT As String = "Number of {0} is not match {1}"

#End Region

#Region "Properties"

        Private _RCHCode As String = String.Empty
        Public Property RCHCode() As String
            Get
                Return _RCHCode
            End Get
            Set(ByVal value As String)
                _RCHCode = value
            End Set
        End Property

#End Region

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
                WriteLogWithErrorList(LogID.LOG00047)
                'Assign SP ID to Audit Log
                If Not SPID Is Nothing AndAlso Not SPID.Trim = String.Empty Then
                    udtAuditLog.UserID = SPID
                End If

                ReadRCHCode(xml, Me.Errors)
                WriteLogWithErrorList(LogID.LOG00048)
                '---------------------------------
                '(Step 2) Check whether there is missing or duplicate fields 
                '---------------------------------
                If Me.Errors.Count = 0 Then
                    Me._bIsValid = CheckSPXMLField(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00049)
                End If

                '---------------------------------
                '(Step 3) Check SP info
                '---------------------------------
                If Me._bIsValid Then
                    Me._bIsValid = ValidatServiceProviderInfo(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00050)
                End If

            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.RCHNameQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                Me.Errors.Add(ErrorCodeList.I99999)
                Me._bIsValid = False
            End Try

        End Sub

#End Region

#Region "Read XML"

        Private Sub ReadRCHCode(ByVal xml As XmlDocument, ByRef udtErrorList As ErrorInfoModelCollection)

            Dim nlInput As XmlNodeList = xml.GetElementsByTagName(TAG_INPUT)

            If nlInput.Count = 0 Then
                udtErrorList.Add(ErrorCodeList.I00004)
                Exit Sub
            End If

            RCHCode = ReadString(nlInput.Item(0), TAG_RCH_CODE, udtErrorList)
        End Sub

#End Region

    End Class

End Namespace
