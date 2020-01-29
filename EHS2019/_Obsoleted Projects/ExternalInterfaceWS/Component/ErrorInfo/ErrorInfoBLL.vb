Imports System.Xml
Imports ExternalInterfaceWS.Component
Imports ExternalInterfaceWS.ComObject

Namespace Component.ErrorInfo

    <Serializable()> Public Class ErrorMessageModel
        Private _strInternalErrorCode As String
        Private _strExternalErrorCode As String
        Private _strInternalErrorMessage As String
        Private _strExternalErrorMessage As String

        Public Sub New(ByVal strInternalMessage As String, ByVal strExternalMessage As String, ByVal strExternalCode As String)

            Me.InternalErrorMessage = strInternalMessage
            Me.ExternalErrorMessage = strExternalMessage
            Me.ExternalErrorCode = strExternalCode
        End Sub


        Public Property InternalErrorCode() As String
            Get
                Return Me._strInternalErrorCode
            End Get
            Set(ByVal value As String)
                Me._strInternalErrorCode = value
            End Set
        End Property

        Public Property ExternalErrorCode() As String
            Get
                Return Me._strExternalErrorCode
            End Get
            Set(ByVal value As String)
                Me._strExternalErrorCode = value
            End Set
        End Property

        Public Property InternalErrorMessage() As String
            Get
                Return Me._strInternalErrorMessage
            End Get
            Set(ByVal value As String)
                Me._strInternalErrorMessage = value
            End Set
        End Property

        Public Property ExternalErrorMessage() As String
            Get
                Return Me._strExternalErrorMessage
            End Get
            Set(ByVal value As String)
                Me._strExternalErrorMessage = value
            End Set
        End Property

    End Class


    Public Class ErrorInfoBLL

        Public Const CACHE_ErrorMessageXmlDocument As String = "ErrorMessage_ExternalInterfaceWS.xml"
        Private _udtReturnErrorCodes As ErrorInfoModelCollection = New ErrorInfoModelCollection

        ''' <summary>
        '''  Retrieve Error Message Model 
        ''' </summary>
        ''' <param name="strInternalErrorCode">Internal Error Code</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetErrorMessageModel(ByVal strInternalErrorCode) As ErrorMessageModel
            Dim xmlDocErrorMessage As New XmlDocument()

            Try
                If Not IsNothing(HttpContext.Current.Cache(CACHE_ErrorMessageXmlDocument)) Then
                    xmlDocErrorMessage = CType(HttpContext.Current.Cache(CACHE_ErrorMessageXmlDocument), XmlDocument)
                Else
                    xmlDocErrorMessage = GetErrorMessageXmlDocument()
                    Common.ComObject.CacheHandler.InsertCache(CACHE_ErrorMessageXmlDocument, xmlDocErrorMessage)
                End If

                Dim NodeMessageItem As XmlNode = xmlDocErrorMessage.SelectSingleNode("/ErrorMessage/MessageItem[@InternalCode='" + strInternalErrorCode + "']")

                Dim strInternalMessage As String = ""
                Dim strExternalMessage As String = ""
                Dim strExternalCode As String = ""
                If Not NodeMessageItem Is Nothing Then
                    strInternalMessage = NodeMessageItem.SelectSingleNode("InternalMessage").InnerText
                    strExternalMessage = NodeMessageItem.SelectSingleNode("ExternalMessage").InnerText
                    strExternalCode = NodeMessageItem.SelectSingleNode("ExternalCode").InnerText

                    Return New ErrorMessageModel(strInternalMessage, strExternalMessage, strExternalCode)
                Else
                    Return Nothing
                End If

            Catch ex As Exception
                ErrorLogHandler.getInstance().WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                '_udtReturnErrorCodes.Add(ErrorCodeList.I99999)
                ' If this message is add by providing internal error code, there may be a chance go into a dead loop of looking for error code
                Return New ErrorMessageModel("Error When Building Error Message Model", "E99999", "Internal Error (Unexpected Error)")
                Return Nothing
            End Try
        
        End Function

        ''' <summary>
        '''  Retrieve XML document for error messages
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetErrorMessageXmlDocument() As XmlDocument
            Dim strErrorMessageXmlPath As String = System.Configuration.ConfigurationManager.AppSettings("ErrorMessageXML")
            Dim xmlDocErrorMessage As New XmlDocument()
            xmlDocErrorMessage.Load(strErrorMessageXmlPath)
            Return xmlDocErrorMessage
        End Function

    End Class

End Namespace