Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Xml
Imports Common.ComFunction.Generator
Imports ExternalInterfaceWS.XMLGenerator
Imports ExternalInterfaceWS.Component.Request
Imports ExternalInterfaceWS.BLL
Imports Common.Component
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports ExternalInterfaceWS.ComObject

Namespace Component.Response

    Public Class SPPracticeValidationResponse

#Region "Private Constant"

        Private Const TAG_EHS_RESPONSE As String = "Response"
        Private Const TAG_OUTPUT As String = "Output"
        Private Const TAG_IS_CORRECT As String = "IsCorrect"
        Private Const TAG_ERROR_INFO As String = "ErrorInfo"
        Private Const TAG_ERROR_CODE As String = "ErrorCode"
        Private Const TAG_ERROR_MESSAGE As String = "ErrorMessage"

#End Region

#Region "Properties"

        Private _udtReturnErrorCodes As ErrorInfoModelCollection = New ErrorInfoModelCollection
        Public ReadOnly Property ReturnErrorCodes() As ErrorInfoModelCollection
            Get
                Return _udtReturnErrorCodes
            End Get
        End Property

        Private _strIsCorrect As String = Nothing
        Public ReadOnly Property IsCorrect() As String
            Get
                Return _strIsCorrect
            End Get
        End Property

        Private _strErrorCode As String = Nothing
        Public ReadOnly Property ErrorCode() As String
            Get
                Return _strErrorCode
            End Get
        End Property

        Private _strErrorMessage As String = Nothing
        Public ReadOnly Property ErrorMessage() As String
            Get
                Return _strErrorMessage
            End Get
        End Property


#End Region

#Region "Constructor"

        Public Sub New()
            'Do Nothing
        End Sub

#End Region

        Public Function ProcessRequest(ByVal oRequest As SPPracticeValidationRequest, ByVal strSystemName As String, ByRef udtAuditLog As ExtAuditLogEntry) As Boolean
            Try
                '------------------------------------------------------
                ' Check Request valid format (Base on XML format, field validation)
                '------------------------------------------------------
                If Not oRequest.IsValid Then
                    _udtReturnErrorCodes = oRequest.Errors
                    Return False
                End If

                '------------------------------------------------------
                ' Check SP Information
                '------------------------------------------------------
                Dim blnIsValid As Boolean = True
                Dim strSPName As String = oRequest.SPSurname.Trim + ", " + oRequest.SPGivenName.Trim

                blnIsValid = SPPracticeBLL.chkServiceProviderInfo(oRequest.SPID, _
                                                        oRequest.PracticeID, _
                                                        oRequest.PracticeName, _
                                                        _udtReturnErrorCodes, _
                                                        strSPName, _
                                                        strSystemName, _
                                                        True)

                If blnIsValid Then
                    _strIsCorrect = YesNo.Yes
                Else
                    _strIsCorrect = YesNo.No
                End If

                udtAuditLog.AddDescripton("IsCorrect", _strIsCorrect)
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00029, _udtReturnErrorCodes)

                Return True
            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.SPPracticeValidation).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                _udtReturnErrorCodes.Add(ErrorCodeList.I99999)
                Return False
            End Try


        End Function

#Region "Generate XML Result"

        Public Function GenXMLResult() As XmlDocument
            Dim xml As New XmlDocument()

            Dim xmlDeclaration As XmlDeclaration = xml.CreateXmlDeclaration("1.0", "utf-8", Nothing)
            xml.InsertBefore(xmlDeclaration, xml.DocumentElement)

            Dim nodeResponse As XmlElement
            nodeResponse = xml.CreateElement(TAG_EHS_RESPONSE)
            xml.AppendChild(nodeResponse)

            Dim nodeResult As XmlElement
            nodeResult = xml.CreateElement(TAG_OUTPUT)
            nodeResponse.AppendChild(nodeResult)

            If _udtReturnErrorCodes.Count = 0 Then
                GenInfoCorrectResult(xml, nodeResult)
            Else
                If _udtReturnErrorCodes.Count = 1 And _udtReturnErrorCodes.Contains(ErrorCodeList.I00046) Then
                    GenInfoCorrectResult(xml, nodeResult)
                Else
                    GenReturnCode(xml, nodeResult)
                End If
            End If

            Return xml
        End Function

        Private Sub GenReturnCode(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)

            Dim nodeInfo As XmlElement
            nodeInfo = xml.CreateElement(TAG_ERROR_INFO)
            nodeResult.AppendChild(nodeInfo)

            Dim nodeTmp As XmlElement

            '----------------------------------------------
            'Retrieve External Code and Message
            '----------------------------------------------
            Dim strErrorCode As String = String.Empty
            Dim strErrorMessage As String = String.Empty
            _udtReturnErrorCodes.RetrieveExternalCodeAndMessageToOutside(strErrorCode, strErrorMessage)
            '----------------------------------------------

            'Error Code
            nodeTmp = xml.CreateElement(TAG_ERROR_CODE)
            'nodeTmp.InnerText = udtErrorInfo.ErrorCode
            nodeTmp.InnerText = strErrorCode
            nodeInfo.AppendChild(nodeTmp)

            'Error Message
            nodeTmp = xml.CreateElement(TAG_ERROR_MESSAGE)
            'nodeTmp.InnerText = udtErrorInfo.ErrorMessage
            nodeTmp.InnerText = strErrorMessage
            nodeInfo.AppendChild(nodeTmp)

        End Sub

        Private Sub GenInfoCorrectResult(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)
            Dim nodeIsCorrect As XmlElement
            nodeIsCorrect = xml.CreateElement(TAG_IS_CORRECT)
            nodeIsCorrect.InnerText = Me.IsCorrect
            nodeResult.AppendChild(nodeIsCorrect)
        End Sub


#End Region

    End Class

End Namespace

