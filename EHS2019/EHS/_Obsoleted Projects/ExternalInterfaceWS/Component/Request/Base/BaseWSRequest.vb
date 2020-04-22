Imports Microsoft.VisualBasic
Imports System.Xml
Imports ExternalInterfaceWS.Component
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports ExternalInterfaceWS.ComObject
Imports System.Globalization
Imports Common.Validation

Namespace Component.Request.Base

    Public MustInherit Class BaseWSRequest

#Region "Private Constant"

        'Private Const ERR_TAG_NOT_FOUND As String = "{0} tag not found"
        'Private Const ERR_TAG_DUPLICATE As String = "Duplicate {0} tag found"
        'Private Const ERR_TAG_INVALID_VALUE As String = "Invalid {0} tag value"
        'Private Const ERR_ITEM_NOT_MATCH_COUNT As String = "Number of {0} is not match {1}"

#End Region

#Region "Protected Constant"

        Protected Const TAG_INPUT As String = "Input"
        Protected Const TAG_MESSAGE_ID As String = "MessageID"

#End Region

#Region "Properties"

        Protected _bIsValid As Boolean = False
        Public ReadOnly Property IsValid() As Boolean
            Get
                Return _bIsValid
            End Get
        End Property

        Protected _udtErrorCodeList As ErrorInfoModelCollection = New ErrorInfoModelCollection
        Public ReadOnly Property Errors() As ErrorInfoModelCollection
            Get
                Return _udtErrorCodeList
            End Get
            'Set(ByVal value As String)
            '    _strErrorCode = value
            'End Set
        End Property

        Protected _udtAuditLog As ExtAuditLogEntry = Nothing
        Public Property ExtAuditLogEntry() As ExtAuditLogEntry
            Get
                Return _udtAuditLog
            End Get
            Set(ByVal value As ExtAuditLogEntry)
                _udtAuditLog = value
            End Set
        End Property

        Protected _strMessageID As String = Nothing
        Private _blnMessageID_Received As Boolean = False
        Public Property MessageID() As String
            Get
                Return _strMessageID
            End Get
            Set(ByVal value As String)
                _strMessageID = value
            End Set
        End Property
#End Region

#Region "Supporting Functions / Public Functions"

        ''' <summary>
        ''' Get node string value which under current node
        ''' </summary>
        ''' <param name="node">Current node contain value</param>
        ''' <param name="sTagName">The tag name of node which under current node </param>
        ''' <returns>Node string value</returns>
        ''' <remarks></remarks>
        Protected Function ReadString(ByVal node As XmlNode, _
                                        ByVal sTagName As String, _
                                        ByRef udtErrorList As ErrorInfoModelCollection, _
                                        Optional ByVal blnMandatory As Boolean = True, _
                                        Optional ByRef blnTagFound As Boolean = False)
            Dim nlTemp As XmlNodeList
            nlTemp = node.SelectNodes("./" + sTagName)
            If nlTemp.Count <> 1 Then
                If blnMandatory Then
                    udtErrorList.Add(ErrorCodeList.I00004)  'Incorrect XML format
                    Return Nothing
                Else
                    Return String.Empty
                End If
            End If

            blnTagFound = True

            Return nlTemp(0).InnerText
        End Function

        ''' <summary>
        ''' Get node integer value which under current node
        ''' </summary>
        ''' <param name="node">Current node contain value</param>
        ''' <param name="sTagName">The tag name of node which under current node </param>
        ''' <returns>Node integer value</returns>
        ''' <remarks></remarks>
        Protected Function ReadInteger(ByVal node As XmlNode, _
                                        ByVal sTagName As String, _
                                        ByRef udtErrorList As ErrorInfoModelCollection, _
                                        Optional ByVal blnMandatory As Boolean = True, _
                                        Optional ByRef blnTagFound As Boolean = False)
            Dim nlTemp As XmlNodeList = node.SelectNodes("./" + sTagName)

            If nlTemp.Count <> 1 Then
                If blnMandatory Then
                    udtErrorList.Add(ErrorCodeList.I00004)  'Incorrect XML format
                    Return Nothing
                Else
                    Return 0
                End If
            End If

            Dim sValue As String = nlTemp.Item(0).InnerText
            Dim iValue As Integer
            If Not Integer.TryParse(sValue, iValue) Then
                If blnMandatory Then
                    udtErrorList.Add(ErrorCodeList.I00005)  'Incorrect input parameter
                    Return Nothing
                Else
                    Return 0
                End If
            End If

            blnTagFound = True

            Return iValue
        End Function

        ''' <summary>
        ''' Get node date value which under current node
        ''' </summary>
        ''' <param name="node">Current node contain value</param>
        ''' <param name="sTagName">The tag name of node which under current node </param>
        ''' <returns>Node integer value</returns>
        ''' <remarks></remarks>
        Protected Function ReadDate(ByVal node As XmlNode, _
                                    ByVal sTagName As String, _
                                    ByRef udtErrorList As ErrorInfoModelCollection, _
                                    Optional ByVal blnMandatory As Boolean = True, _
                                    Optional ByRef blnTagFound As Boolean = False)
            Dim nlTemp As XmlNodeList = node.SelectNodes("./" + sTagName)

            If nlTemp.Count = 0 Then
                If blnMandatory Then
                    udtErrorList.Add(ErrorCodeList.I00004)  'Incorrect XML format
                    Return Nothing
                Else
                    Return 0
                End If
            ElseIf nlTemp.Count > 1 Then
                udtErrorList.Add(ErrorCodeList.I00004)  'Incorrect XML format
                Return Nothing
            End If

            Dim sValue As String = nlTemp.Item(0).InnerText
            Dim iValue As Date
            If Not Date.TryParse(sValue, New CultureInfo(Common.Component.CultureLanguage.English), "dd-MM-yyyy", iValue) Then
                If blnMandatory Then
                    udtErrorList.Add(ErrorCodeList.I00005)  'Incorrect input parameter
                    Return Nothing
                Else
                    Return Date.Now
                End If
            End If

            blnTagFound = True

            Return iValue
        End Function




        ''' <summary>
        ''' Get node string value which under current node
        ''' </summary>
        ''' <param name="node">Current node contain value</param>
        ''' <param name="sTagName">The tag name of node which under current node </param>
        ''' <returns>Node string value</returns>
        ''' <remarks></remarks>
        Protected Function ReadString(ByVal node As XmlNode, _
                                        ByVal sTagName As String, _
                                        ByRef udtErrorList As ErrorInfoModelCollection, _
                                        ByRef strCustomError As String, _
                                        Optional ByVal blnMandatory As Boolean = True, _
                                        Optional ByRef blnTagFound As Boolean = False)
            Dim nlTemp As XmlNodeList
            nlTemp = node.SelectNodes("./" + sTagName)
            If nlTemp.Count <> 1 Then
                If blnMandatory Then
                    udtErrorList.Add(strCustomError)  'Incorrect XML format
                    Return Nothing
                Else
                    Return String.Empty
                End If
            End If

            blnTagFound = True

            Return nlTemp(0).InnerText
        End Function

        ''' <summary>
        ''' Get node integer value which under current node
        ''' </summary>
        ''' <param name="node">Current node contain value</param>
        ''' <param name="sTagName">The tag name of node which under current node </param>
        ''' <returns>Node integer value</returns>
        ''' <remarks></remarks>
        Protected Function ReadInteger(ByVal node As XmlNode, _
                                        ByVal sTagName As String, _
                                        ByRef udtErrorList As ErrorInfoModelCollection, _
                                        ByRef strCustomError As String, _
                                        Optional ByVal blnMandatory As Boolean = True, _
                                        Optional ByRef blnTagFound As Boolean = False)
            Dim nlTemp As XmlNodeList = node.SelectNodes("./" + sTagName)

            If nlTemp.Count <> 1 Then
                If blnMandatory Then
                    udtErrorList.Add(strCustomError)  'Incorrect XML format
                    Return Nothing
                Else
                    Return 0
                End If
            End If

            Dim sValue As String = nlTemp.Item(0).InnerText
            Dim iValue As Integer
            If Not Integer.TryParse(sValue, iValue) Then
                If blnMandatory Then
                    udtErrorList.Add(strCustomError)  'Incorrect input parameter
                    Return Nothing
                Else
                    Return 0
                End If
            End If

            blnTagFound = True

            Return iValue
        End Function

        ''' <summary>
        ''' Get node date value which under current node
        ''' </summary>
        ''' <param name="node">Current node contain value</param>
        ''' <param name="sTagName">The tag name of node which under current node </param>
        ''' <returns>Node integer value</returns>
        ''' <remarks></remarks>
        Protected Function ReadDate(ByVal node As XmlNode, _
                                    ByVal sTagName As String, _
                                    ByRef udtErrorList As ErrorInfoModelCollection, _
                                    ByRef strCustomError As String, _
                                    Optional ByVal blnMandatory As Boolean = True, _
                                    Optional ByRef blnTagFound As Boolean = False)
            Dim nlTemp As XmlNodeList = node.SelectNodes("./" + sTagName)

            If nlTemp.Count <> 1 Then
                If blnMandatory Then
                    udtErrorList.Add(strCustomError)  'Incorrect XML format
                    Return Nothing
                Else
                    Return Date.Today
                End If
            End If

            Dim sValue As String = nlTemp.Item(0).InnerText
            Dim iValue As Date
            If Not Date.TryParse(sValue, New CultureInfo(Common.Component.CultureLanguage.English), "dd-MM-yyyy", iValue) Then
                If blnMandatory Then
                    udtErrorList.Add(strCustomError)  'Incorrect input parameter
                    Return Nothing
                Else
                    Return Date.Today
                End If
            End If

            blnTagFound = True

            Return iValue
        End Function


        Protected Function ReadMessageIDandValidate(ByVal xml As XmlDocument, ByRef udtErrorList As ErrorInfoModelCollection)

            Me.ReadMessageID(xml, udtErrorList)

            If _blnMessageID_Received Then
                Return CheckMessageID(udtErrorList)
            Else
                Return False
            End If
        End Function

#End Region

#Region "For Logging"

        Protected Sub WriteLogWithErrorList(ByVal strLogID As String)

            If _udtErrorCodeList.Count > 0 Then
                Dim strErrorList As String = String.Empty
                For Each udtError As ErrorInfoModel In _udtErrorCodeList
                    If strErrorList <> String.Empty Then
                        strErrorList = strErrorList + " ,"
                    End If
                    strErrorList = strErrorList + udtError.ErrorMessage
                Next

                _udtAuditLog.AddDescripton("ErrorList", strErrorList)
                _udtAuditLog.WriteLog_Ext(strLogID)
            Else
                _udtAuditLog.WriteLog_Ext(strLogID)
            End If

        End Sub

#End Region



#Region "Read Message ID"

        Private Sub ReadMessageID(ByVal xml As XmlDocument, ByRef udtErrorList As ErrorInfoModelCollection)

            Dim nlInput As XmlNodeList = xml.GetElementsByTagName(TAG_INPUT)

            If nlInput.Count = 0 Then
                udtErrorList.Add(ErrorCodeList.I00004)
                Exit Sub
            End If

            MessageID = ReadString(nlInput.Item(0), TAG_MESSAGE_ID, udtErrorList, ErrorCodeList.I00111, True, _blnMessageID_Received)

        End Sub

#End Region

#Region "Check Message ID"

        ''' <summary>
        '''  Validate the format of Message ID
        '''  Public --> for Upload Claim HL7 function      
        ''' </summary>
        ''' <param name="udtErrorList"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Function CheckMessageID(ByRef udtErrorList As ErrorInfoModelCollection) As Boolean

            Dim udtValidator As New Validator

            If Not (MessageID) Is Nothing _
                AndAlso Not MessageID.Trim.Equals(String.Empty) _
                AndAlso udtValidator.IsAlphanumericHyphen(MessageID.Trim) Then

                MessageID = MessageID.Trim
                Return True
            Else
                udtErrorList.Add(ErrorCodeList.I00111)
                Return False
            End If

        End Function


#End Region


    End Class

End Namespace


