Imports Microsoft.VisualBasic
Imports System.Collections.Generic
Imports System.Xml
Imports ExternalInterfaceWS.Component.Request.Base
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports Common.Validation
Imports Common.Component
Imports ExternalInterfaceWS.ComObject

Namespace Component.Request

    Public Class SPPracticeValidationRequest
        Inherits BaseWSSPRequest

#Region "Private Constant"

        Private Const TAG_SP_SURNAME As String = "SPSurname"
        Private Const TAG_SP_GIVENNAME As String = "SPGivenName"

#End Region

#Region "Properties"

        Private _SPSurname As String = String.Empty
        Public Property SPSurname() As String
            Get
                Return _SPSurname
            End Get
            Set(ByVal value As String)
                _SPSurname = value
            End Set
        End Property

        Private _SPGivenName As String = String.Empty
        Public Property SPGivenName() As String
            Get
                Return _SPGivenName
            End Get
            Set(ByVal value As String)
                _SPGivenName = value
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
                'Assign SP ID to Audit Log
                If Not SPID Is Nothing AndAlso Not SPID.Trim = String.Empty Then
                    udtAuditLog.UserID = SPID
                End If

                WriteLogWithErrorList(LogID.LOG00025)
                '---------------------------------
                '(Step 2) Check whether there is missing or duplicate fields 
                '---------------------------------
                If Me.Errors.Count = 0 Then
                    Me._bIsValid = CheckSPXMLField(Me.Errors)

                    WriteLogWithErrorList(LogID.LOG00026)
                End If
                '---------------------------------
                '(Step 3) Check SP info
                '---------------------------------
                If Me._bIsValid Then
                    Me._bIsValid = ValidatServiceProviderInfo(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00027)
                End If
                If Me._bIsValid Then
                    Me._bIsValid = ValidateSPName(Me.Errors)
                    WriteLogWithErrorList(LogID.LOG00028)
                End If

            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.SPPracticeValidation).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                Me.Errors.Add(ErrorCodeList.I99999)
                Me._bIsValid = False
            End Try

        End Sub

#End Region

#Region "Read XML"

        Protected Overrides Sub ReadSPInfo(ByVal xml As XmlDocument, ByRef udtErrorList As ErrorInfoModelCollection)
            Dim nlSPinfo As XmlNodeList = xml.GetElementsByTagName(TAG_SP_INFO)

            If nlSPinfo.Count = 0 Then
                udtErrorList.Add(ErrorCodeList.I00004)
            Else
                ReadSPID(nlSPinfo.Item(0), udtErrorList)
                ReadPracticeID(nlSPinfo.Item(0), udtErrorList)
                ReadPracticeName(nlSPinfo.Item(0), udtErrorList)
                ReadSPSurname(nlSPinfo.Item(0), udtErrorList)
                ReadSPGivenName(nlSPinfo.Item(0), udtErrorList)
            End If

        End Sub

        Private Sub ReadSPSurname(ByVal nodePatientDocument As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            SPSurname = ReadString(nodePatientDocument, TAG_SP_SURNAME, udtErrorList)
        End Sub

        Private Sub ReadSPGivenName(ByVal nodePatientDocument As XmlNode, ByRef udtErrorList As ErrorInfoModelCollection)
            SPGivenName = ReadString(nodePatientDocument, TAG_SP_GIVENNAME, udtErrorList)
        End Sub

#End Region

#Region "Additional Functions"

        Private Function ValidateSPName(ByRef udtErrorList As ErrorInfoModelCollection)

            'If Me._SPName = String.Empty Then
            '    udtErrorList.Add(ErrorCodeList.E00013) 'Incorrect Input parameter of "SP Name"
            '    Return False
            'End If
            Dim udtSM As Common.ComObject.SystemMessage = Nothing
            Dim udtvalidator As Validator = New Validator

            udtSM = udtvalidator.chkEngName(Me.SPSurname, Me.SPGivenName)
            If Not IsNothing(udtSM) Then
                udtErrorList.Add(ErrorCodeList.I00013) 'Incorrect input parameter of "Name in English" 
                Return False
            End If

            Return True

        End Function

#End Region

    End Class

End Namespace


