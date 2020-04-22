
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Globalization
Imports Common.ComObject
Imports Common.Component.Mapping

Namespace WebService.Interface

    <Serializable()> Public Class PCDCheckAvailableForVerifiedEnrolmentResult
        Inherits BaseResult

        Public Enum enumReturnCode

            ' Available
            Available = 0

            ' Service Provider Already Existed
            ServiceProviderAlreadyExisted = 1

            ' Enrolment Already Existed
            EnrolmentAlreadyExisted = 2

            ' Verified Enrolment Already Existed
            VerifiedEnrolmentAlreadyExisted = 3

            ' Data Validation Fail
            DataValidationFail = 96

            ' Service Authentication Failed
            AuthenticationFailed = 97

            ' Invalid Parameter
            InvalidParameter = 98

            ' Enrolment (All Unexpected Errors)
            ErrorAllUnexpected = 99

            ' [eHS self error code] Fail to connect PCD web service, e.g. Timeout
            CommunicationLinkError = 101
        End Enum

        Private Const FORMAT_CREATE_DATETIME As String = "yyyy/MM/dd HH:mm:ss"
        Private _strMessageID As String = String.Empty
        Private _enumReturnCode As enumReturnCode = enumReturnCode.ErrorAllUnexpected

        Private _strRequest As String = String.Empty
        Private _strResult As String = String.Empty
        Private _objException As Exception
        Private _returnCodeDesc As String = String.Empty


        ''' <summary>
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

        Public Property ReturnCode() As enumReturnCode
            Get
                Return _enumReturnCode
            End Get
            Set(ByVal value As enumReturnCode)
                _enumReturnCode = value
            End Set
        End Property

        Public Property ReturnCodeDesc() As String
            Get
                Return _returnCodeDesc
            End Get
            Set(ByVal value As String)
                _returnCodeDesc = value
            End Set
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

        Public Sub New(ByVal strResulttXml As String)
            ReadXml(strResulttXml, Nothing)
        End Sub

        Public Sub New(ByVal strResulttXml As String, ByVal strMessageID As String)
            ReadXml(strResulttXml, strMessageID)
        End Sub

        Public Sub New(ByVal eReturnCode As enumReturnCode)
            _enumReturnCode = eReturnCode
            _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)
        End Sub

        Public Sub New(ByVal eReturnCode As enumReturnCode, ByVal objException As Exception)
            _enumReturnCode = eReturnCode
            _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)
            _objException = objException
        End Sub

        ''' <summary>
        ''' Require to log message id when meet exception
        ''' </summary>
        ''' <param name="eReturnCode"></param>
        ''' <param name="objException"></param>
        ''' <param name="strMessageID"></param>
        ''' <remarks></remarks>
        Public Sub New(ByVal eReturnCode As enumReturnCode, ByVal objException As Exception, ByVal strMessageID As String)
            _enumReturnCode = eReturnCode
            _objException = objException
            _strMessageID = strMessageID
        End Sub

        ''' <summary>
        ''' Read xml result from PCD web service and convert to object
        ''' </summary>
        ''' <param name="strResulttXml">Xml result from PCD web service</param>
        ''' <param name="strMessageID">Xml request message id</param>
        ''' <remarks></remarks>
        Private Sub ReadXml(ByVal strResulttXml As String, ByVal strMessageID As String)
            Dim ds As DataSet = Nothing
            Dim dt As DataTable = Nothing

            _strResult = strResulttXml

            Try
                ds = Common.ComFunction.XmlFunction.Xml2Dataset(strResulttXml)

                ' Read return code
                dt = ds.Tables("result")
                _strMessageID = GetMessageID(dt.Rows(0))

                If enumReturnCode.IsDefined(GetType(enumReturnCode), CInt(dt.Rows(0)("return_code"))) Then
                    _enumReturnCode = CType(CInt(dt.Rows(0)("return_code")), enumReturnCode)
                Else
                    ' Return code is not defined
                    _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                End If

                'Select Case dt.Rows(0)("return_code").ToString()
                '    Case "0"
                '        _enumReturnCode = enumReturnCode.Available
                '    Case "1"
                '        _enumReturnCode = enumReturnCode.ServiceProviderAlreadyExisted
                '    Case "2"
                '        _enumReturnCode = enumReturnCode.EnrolmentAlreadyExisted
                '    Case "96"
                '        _enumReturnCode = enumReturnCode.DataValidationFail
                '    Case "97"
                '        _enumReturnCode = enumReturnCode.AuthenticationFailed
                '    Case "98"
                '        _enumReturnCode = enumReturnCode.InvalidParameter
                '    Case "99"
                '        _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                '    Case Else
                '        _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                'End Select


                If strMessageID IsNot Nothing AndAlso _strMessageID <> strMessageID Then
                    _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                End If

                _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)

                'Dim dtmMessageDTM As DateTime
                'If Not DateTime.TryParseExact(dr("message_datetime").ToString(), FORMAT_CREATE_DATETIME, New CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, dtmMessageDTM) Then
                '    ThrowException("PCDUploadEnrolInfoResult: Fail to convert <message_datetime> value to datetime")
                'End If

            Catch e As System.Net.WebException
                _enumReturnCode = enumReturnCode.CommunicationLinkError
                _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)
                _objException = e
            Catch e As System.Xml.XmlException
                _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)
                _objException = e
            Catch e As Exception
                _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                _returnCodeDesc = GetReturnCodeDesc(_enumReturnCode)
                _objException = e
            End Try
        End Sub

        ''' <summary>
        ''' CRE10-035
        ''' Retrieve the message_id from result XML
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

        Public Overloads Function GetReturnCodeDesc(ByVal enumN As enumReturnCode) As String
            MyBase.GenReturnCodeSystemMessage(EnumConstant.EnumMappingCodeType.WS_PCD_CheckForVerifiedEnrolment_Return_Code, enumN)
            Return Me.SystemMessage.GetMessage()
        End Function

    End Class

End Namespace

