
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Globalization
Imports Common.Component.ServiceProvider
Imports Common.Component
Imports System.Xml
Imports Common.DataAccess

Namespace WebService.Interface

    <Serializable()> Public Class EHSPracticeSchemeResult

        Public Enum enumReturnCode

            ' Service Provider is active
            SuccessWithData = 0

            ' Service Provider Status is not active / not found
            NotActiveOrNotFound = 1

            ' Data Validation Fail
            DataValidationFail = 96

            ' Service Authentication Failed
            AuthenticationFailed = 97

            ' Invalid Parameter
            InvalidParameter = 98

            ' getEHSPracticeScheme (All Unexpected Errors)
            ErrorAllUnexpected = 99
        End Enum

        Private Const WS_METHOD_NAME As String = "GetEHSPracticeScheme"
        Private Const FORMAT_CREATE_DATETIME As String = "yyyy/MM/dd HH:mm:ss"
        Private _strMessageID As String = String.Empty
        Private _ws_method_name As String = String.Empty
        Private _hkid As String = String.Empty
        Private _blnIsValid As Boolean = True

        Private _enumReturnCode As enumReturnCode = enumReturnCode.ErrorAllUnexpected

        Private _strRequest As String = String.Empty
        Private _strResult As String = String.Empty
        Private _objException As Exception

        Private _CallerRequest_XML As String = String.Empty
        Private _GeneratedResult_XML As String = String.Empty

        ''' <summary>
        ''' CRE10-035
        ''' Message ID for match outgoing request and return result
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property MessageID() As String
            Get
                Return _strMessageID
            End Get
        End Property

        Public ReadOnly Property CallerRequest_XML() As String
            Get
                Return _CallerRequest_XML
            End Get
        End Property

        Public ReadOnly Property GeneratedResult_XML() As String
            Get
                Return _GeneratedResult_XML
            End Get
        End Property

        ''' <summary>
        ''' Communication result on CMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ReturnCode() As enumReturnCode
            Get
                Return _enumReturnCode
            End Get
        End Property

        Public ReadOnly Property Exception() As Exception
            Get
                Return _objException
            End Get
        End Property


        Public ReadOnly Property Result() As String
            Get
                Return _strResult
            End Get
        End Property

        Public Property Request() As String
            Get
                Return _strRequest
            End Get
            Set(ByVal value As String)
                _strRequest = value
            End Set
        End Property


        Public Sub New(ByVal eReturnCode As enumReturnCode)
            _enumReturnCode = eReturnCode
        End Sub

        Public Sub New(ByVal eReturnCode As enumReturnCode, ByVal objException As Exception)
            _enumReturnCode = eReturnCode
            _objException = objException
        End Sub

        ''' <summary>
        ''' INT11-0021
        ''' Require to log message id when meet exception
        ''' </summary>
        ''' <param name="strgetEHSPracticeSchemeXML"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal strgetEHSPracticeSchemeXML As String)
            ReadXml(strgetEHSPracticeSchemeXML)
            '_enumReturnCode = eReturnCode
            '_objException = objException
            '_strMessageID = strMessageID
        End Sub

        ''' <summary>
        ''' Read xml request from PCD functions, process it
        ''' </summary>
        ''' <param name="strgetEHSPracticeSchemeXML">Xml result from PCD web service</param>
        ''' <remarks></remarks>
        Private Sub ReadXml(ByVal strgetEHSPracticeSchemeXML As String)
            Dim ds As DataSet = Nothing
            Dim dt As DataTable = Nothing
            _blnIsValid = True

            _strRequest = strgetEHSPracticeSchemeXML
            _CallerRequest_XML = strgetEHSPracticeSchemeXML

            Try
                ds = Common.ComFunction.XmlFunction.Xml2Dataset(strgetEHSPracticeSchemeXML)

                ' Read Parameter
                dt = ds.Tables("parameter")
                _strMessageID = GetMessageID(dt.Rows(0))
                _ws_method_name = dt.Rows(0)("ws_method_name")
                _hkid = dt.Rows(0)("hkid")

                'validate HKID
                Dim objValidator As New Common.Validation.Validator
                Dim objSysMsg As ComObject.SystemMessage = objValidator.chkHKID(_hkid)
                If Not objSysMsg Is Nothing Then
                    ' invalid HKID
                    _enumReturnCode = enumReturnCode.DataValidationFail
                    _blnIsValid = False
                End If

                ' provided method name should match
                If _blnIsValid And WS_METHOD_NAME.ToUpper > _ws_method_name.ToUpper Then
                    _enumReturnCode = enumReturnCode.InvalidParameter
                    _blnIsValid = False
                End If

            Catch exXML As System.Xml.XmlException
                Me._objException = exXML
                _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                _blnIsValid = False
            Catch ex As Exception
                Me._objException = ex
                _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                _blnIsValid = False
        End Try
            'Dim dtmMessageDTM As DateTime
            'If Not DateTime.TryParseExact(dr("message_datetime").ToString(), FORMAT_CREATE_DATETIME, New CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, dtmMessageDTM) Then
            '    ThrowException("PCDUploadEnrolInfoResult: Fail to convert <message_datetime> value to datetime")
            'End If
        End Sub

        Public Function GenResponseXML() As String

            Dim SP As ServiceProviderBLL
            Dim udtSP As ServiceProviderModel = Nothing
            Dim udtDB As Database
            Dim cReturnCode As enumReturnCode = enumReturnCode.NotActiveOrNotFound
            Dim dtSPparticulars As DataTable
            Dim strSPID As String = String.Empty
            Dim udtSchemeInfo As SchemeInformation.SchemeInformationModel = Nothing

            SP = New ServiceProviderBLL
            udtDB = New Database

            Try
                If _blnIsValid Then
                    'dtSPparticulars = SP.GetServiceProviderParticulasPermanentByHKID(_hkid, udtDB)

                    'If Not dtSPparticulars Is Nothing AndAlso dtSPparticulars.Rows.Count > 0 Then
                    '    strSPID = dtSPparticulars.Rows(0)("SP_ID")

                    udtSP = SP.GetServiceProviderActivePermanentByHKID(_hkid, udtDB)

                    If udtSP IsNot Nothing Then
                        ' Service Provider Status is active only
                        If (udtSP.RecordStatus = Common.Component.ServiceProviderStatus.Active) Then

                            ' Practice Status is active only
                            For Each udtPractice As Practice.PracticeModel In udtSP.PracticeList.Values
                                If udtPractice.RecordStatus = PracticeStatus.Active Then
                                    ' Practice Scheme Information Status is active only
                                    For Each udtPracticeSchemeInfo As PracticeSchemeInfo.PracticeSchemeInfoModel In udtPractice.PracticeSchemeInfoList.Values
                                        If udtPracticeSchemeInfo.RecordStatus = PracticeSchemeInfoStatus.Active Then
                                            ' Enrolled scheme status is active too
                                            udtSchemeInfo = udtSP.SchemeInfoList.Filter(udtPracticeSchemeInfo.SchemeCode)
                                            If udtSchemeInfo.RecordStatus = SchemeInformationStatus.Active Then
                                                cReturnCode = enumReturnCode.SuccessWithData
                                                Exit For
                                            End If
                                        End If
                                    Next

                                End If
                            Next
                        End If
                    Else
                        cReturnCode = enumReturnCode.NotActiveOrNotFound
                    End If
                Else
                    cReturnCode = _enumReturnCode
                End If


            Catch exW As System.Xml.XmlException
                _objException = exW
                cReturnCode = enumReturnCode.InvalidParameter
            Catch ex As Exception
                _objException = ex
                cReturnCode = enumReturnCode.ErrorAllUnexpected
            End Try

            Return GenXMLResult(_strMessageID, cReturnCode, udtSP)
        End Function

        Public Function GenXMLResult(ByVal strMessageID As String, ByVal cReturnCode As enumReturnCode, ByVal udtSP As ServiceProviderModel) As String
            ' This function generates the XML Result to the caller

            Dim xmlResponse As XmlDocument
            Dim objXmlGenerator As WSXmlGenerator
            Dim objResponse As WSXmlGenerator.eHSPracticeSchemeResponse

            Me._enumReturnCode = cReturnCode

            objXmlGenerator = New WSXmlGenerator()
            objResponse = New WSXmlGenerator.eHSPracticeSchemeResponse

            ' append parent node
            Dim nodeROOT As XmlElement
            Dim xmlDeclaration As XmlDeclaration = objXmlGenerator.XML.CreateXmlDeclaration("1.0", "utf-8", Nothing)

            ' clear the XML document
            objXmlGenerator.XML.RemoveAll()

            nodeROOT = objXmlGenerator.XML.CreateElement(objResponse.TAGROOT)
            objXmlGenerator.XML.InsertBefore(xmlDeclaration, objXmlGenerator.XML.DocumentElement)
            objXmlGenerator.XML.AppendChild(nodeROOT)

            objResponse.GenerateXMLResponseAttributes(objXmlGenerator.XML, nodeROOT, strMessageID, WS_METHOD_NAME, (CInt(cReturnCode)).ToString())
            If cReturnCode = enumReturnCode.SuccessWithData Then
                objResponse.GenerateXMLPracticeInfoCollection(objXmlGenerator.XML, nodeROOT, udtSP.PracticeList, True)
            End If

            objResponse.GenerateXMLResponseMessageDateTime(objXmlGenerator.XML, nodeROOT)

            xmlResponse = objXmlGenerator.XML

            Return xmlResponse.InnerXml

        End Function

        ''' <summary>
        ''' CRE10-035
        ''' Retrieve the message_id from XML
        ''' </summary>
        ''' <param name="dr"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetMessageID(ByVal dr As DataRow) As String
            If dr.Table.Columns.Contains("message_id") Then
                If dr("message_id") <> "" Then
                    Return dr("message_id")
                End If
            End If

            Return String.Empty
        End Function

        Private Sub ThrowException(ByVal strMessage As String)
            Throw New Exception(strMessage)
        End Sub
    End Class

End Namespace

