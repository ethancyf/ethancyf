
Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Globalization
Imports Common.ComObject
Imports Common.Component.Mapping

Namespace WebService.Interface

    <Serializable()> Public Class PCDCheckIsActiveSPResult
        Inherits BaseResult

        Public Enum enumReturnCode

            ' Active
            IsActive = 0

            ' Not Active
            NotActive = 1

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

            ' Response from PCD for invalid results (e.g. cannot find a mandatory tag)
            'InvalidResult = 103

            ' [eHS / PCD error] Message ID returned from PCD is mismatched with EHS Request
            'MessageIDMismatch = 104
        End Enum

        Private Const FORMAT_CREATE_DATETIME As String = "yyyy/MM/dd HH:mm:ss"
        Private _strMessageID As String = String.Empty
        Private _enumReturnCode As enumReturnCode = enumReturnCode.ErrorAllUnexpected

        Private _strRequest As String = String.Empty
        Private _strResult As String = String.Empty
        Private _objException As Exception
        Private _returnCodeDesc As String = String.Empty
        

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

        ''' <summary>
        ''' Communication result on CMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
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

        Public Sub New(ByVal strPCDUploadEnrolInfoResultXml As String)
            ReadXml(strPCDUploadEnrolInfoResultXml, Nothing)
        End Sub

        Public Sub New(ByVal strPCDUploadEnrolInfoResultXml As String, ByVal strMessageID As String)
            ReadXml(strPCDUploadEnrolInfoResultXml, strMessageID)
        End Sub

        Public Sub New(ByVal eReturnCode As enumReturnCode)
            _enumReturnCode = eReturnCode
            _returnCodeDesc = getReturnCodeDesc(_enumReturnCode)
        End Sub

        Public Sub New(ByVal eReturnCode As enumReturnCode, ByVal objException As Exception)
            _enumReturnCode = eReturnCode
            _returnCodeDesc = getReturnCodeDesc(_enumReturnCode)
            _objException = objException
        End Sub

        ''' <summary>
        ''' INT11-0021
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
        ''' <param name="strPCDUploadEnrolInfoResultXml">Xml result from PCD web service</param>
        ''' <param name="strMessageID">Xml request message id</param>
        ''' <remarks></remarks>
        Private Sub ReadXml(ByVal strPCDUploadEnrolInfoResultXml As String, ByVal strMessageID As String)
            Dim ds As DataSet = Nothing
            Dim dt As DataTable = Nothing

            _strResult = strPCDUploadEnrolInfoResultXml
            
            Try
                ds = Common.ComFunction.XmlFunction.Xml2Dataset(strPCDUploadEnrolInfoResultXml)

                ' Read return code
                dt = ds.Tables("result")
                _strMessageID = GetMessageID(dt.Rows(0))

                Select Case dt.Rows(0)("return_code").ToString()
                    Case "0"
                        _enumReturnCode = enumReturnCode.IsActive
                    Case "1"
                        _enumReturnCode = enumReturnCode.NotActive
                    Case "96"
                        _enumReturnCode = enumReturnCode.DataValidationFail
                    Case "97"
                        _enumReturnCode = enumReturnCode.AuthenticationFailed
                    Case "98"
                        _enumReturnCode = enumReturnCode.InvalidParameter
                    Case "99"
                        _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                    Case Else
                        _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                End Select


                If strMessageID IsNot Nothing AndAlso _strMessageID <> strMessageID Then
                    _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                End If

                _returnCodeDesc = getReturnCodeDesc(_enumReturnCode)

                'Dim dtmMessageDTM As DateTime
                'If Not DateTime.TryParseExact(dr("message_datetime").ToString(), FORMAT_CREATE_DATETIME, New CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, dtmMessageDTM) Then
                '    ThrowException("PCDUploadEnrolInfoResult: Fail to convert <message_datetime> value to datetime")
                'End If

            Catch e As System.Net.WebException
                _enumReturnCode = enumReturnCode.CommunicationLinkError
                _returnCodeDesc = getReturnCodeDesc(_enumReturnCode)
                _objException = e
            Catch e As System.Xml.XmlException
                _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                _returnCodeDesc = getReturnCodeDesc(_enumReturnCode)
                _objException = e
            Catch e As Exception
                _enumReturnCode = enumReturnCode.ErrorAllUnexpected
                _returnCodeDesc = getReturnCodeDesc(_enumReturnCode)
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

        Public Overloads Function getReturnCodeDesc(ByVal enumN As enumReturnCode) As String
            MyBase.GenReturnCodeSystemMessage(EnumConstant.EnumMappingCodeType.WS_PCD_CheckIsActiveSP_Return_Code, enumN)
            Return Me.SystemMessage.GetMessage()
            'Dim strreturnCodeDesc As String = String.Empty
            'Const FUNCTION_CODE_HCVU As String = "990001"
            'Const FUNCTION_CODE_HCSP As String = "990002"
            'Const SEVERITY_CODE As String = "E"

            '' Function Code depends on platform
            'Dim strFunctCode As String = System.Configuration.ConfigurationManager.AppSettings("Platform")
            'strFunctCode = IIf(strFunctCode = Component.EVSPlatform.HCVU, FUNCTION_CODE_HCVU, IIf(strFunctCode = Component.EVSPlatform.HCSP, FUNCTION_CODE_HCSP, strFunctCode))

            'Dim udtCodeMapList As CodeMappingCollection
            'Dim udtCodeMap As CodeMappingModel
            'udtCodeMapList = CodeMappingBLL.GetAllCodeMapping

            'Select Case enumN
            '    Case enumReturnCode.IsActive
            '        udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.PCD, CodeMappingModel.EnumTargetSystem.EHS, CodeMappingModel.EnumCodeType.WS_PCD_CheckIsActiveSP_Return_Code, "0")
            '        _sysMsg = New SystemMessage(strFunctCode, SEVERITY_CODE, udtCodeMap.CodeTarget)
            '        strreturnCodeDesc = _sysMsg.GetMessage()
            '    Case enumReturnCode.NotActive
            '        udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.PCD, CodeMappingModel.EnumTargetSystem.EHS, CodeMappingModel.EnumCodeType.WS_PCD_CheckIsActiveSP_Return_Code, "1")
            '        _sysMsg = New SystemMessage(strFunctCode, SEVERITY_CODE, udtCodeMap.CodeTarget)
            '        strreturnCodeDesc = _sysMsg.GetMessage()
            '    Case enumReturnCode.InvalidParameter
            '        udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.PCD, CodeMappingModel.EnumTargetSystem.EHS, CodeMappingModel.EnumCodeType.WS_PCD_CheckIsActiveSP_Return_Code, "98")
            '        _sysMsg = New SystemMessage(strFunctCode, SEVERITY_CODE, udtCodeMap.CodeTarget)
            '        strreturnCodeDesc = _sysMsg.GetMessage()
            '    Case enumReturnCode.CommunicationLinkError
            '        udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.PCD, CodeMappingModel.EnumTargetSystem.EHS, CodeMappingModel.EnumCodeType.WS_PCD_CheckIsActiveSP_Return_Code, "101")
            '        _sysMsg = New SystemMessage(strFunctCode, SEVERITY_CODE, udtCodeMap.CodeTarget)
            '        strreturnCodeDesc = _sysMsg.GetMessage()
            '    Case Else
            '        udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.PCD, CodeMappingModel.EnumTargetSystem.EHS, CodeMappingModel.EnumCodeType.WS_PCD_CheckIsActiveSP_Return_Code, "99")
            '        _sysMsg = New SystemMessage(strFunctCode, SEVERITY_CODE, udtCodeMap.CodeTarget)
            '        strreturnCodeDesc = _sysMsg.GetMessage()
            'End Select

            'Return strreturnCodeDesc
        End Function

    End Class

End Namespace

