Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Xml
Imports Common.ComFunction.Generator
Imports ExternalInterfaceWS.XMLGenerator
Imports ExternalInterfaceWS.BLL
Imports ExternalInterfaceWS.BLL.ValidateAccountBLL
Imports Common.Component.EHSAccount
Imports Common.DataAccess
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.DocType
Imports ExternalInterfaceWS.Component.Request
Imports Common.Component
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports ExternalInterfaceWS.ComObject

Namespace Component.Response

    Public Class eHSValidatedAccountQueryResponse

#Region "Private Constant"

        Private Const TAG_EHS_RESPONSE As String = "Response"
        Private Const TAG_OUTPUT As String = "Output"
        Private Const TAG_ERROR_INFO As String = "ErrorInfo"
        Private Const TAG_ERROR_CODE As String = "ErrorCode"
        Private Const TAG_ERROR_MESSAGE As String = "ErrorMessage"
        Private Const TAG_ACCOUNT_MATCHED As String = "AccountMatched"

#End Region

#Region "Properties"

        Private _udtReturnErrorCodes As ErrorInfoModelCollection = New ErrorInfoModelCollection
        Public ReadOnly Property ReturnErrorCodes() As ErrorInfoModelCollection
            Get
                Return _udtReturnErrorCodes
            End Get
        End Property

        Private _strAccountMatched As String = String.Empty
        Public ReadOnly Property AccountMatched() As String
            Get
                Return _strAccountMatched
            End Get
        End Property

#End Region

#Region "Constructor"

        Public Sub New()
            'Do Nothing
        End Sub

#End Region

        Public Function ProcessRequest(ByVal oRequest As eHSValidatedAccountQueryRequest, ByVal strSystemName As String, ByRef udtAuditLog As ExtAuditLogEntry) As Boolean
            Try
                '------------------------------------------------------
                ' Check Request valid format (Base on XML format, field validation)
                '------------------------------------------------------
                If Not oRequest.IsValid Then
                    _udtReturnErrorCodes = oRequest.Errors
                    Return False
                End If

                Dim udtDB As Database = New Database
                Dim udtEHSPersonalInfo As EHSPersonalInformationModel = New EHSPersonalInformationModel
                Dim udtValidateAccountBLL As ValidateAccountBLL = New ValidateAccountBLL
                Dim strValidateResult As String = String.Empty

                '------------------------------------------------------
                ' Check SP Information
                '------------------------------------------------------
                Dim blnIsValid As Boolean = True
                blnIsValid = SPPracticeBLL.chkServiceProviderInfo(oRequest.SPID, _
                                                        oRequest.PracticeID, _
                                                        oRequest.PracticeName, _
                                                        _udtReturnErrorCodes, _
                                                        String.Empty, _
                                                        strSystemName, _
                                                        False)

                If Not blnIsValid Then
                    Return False
                End If

                udtAuditLog.AddDescripton("IsValid", blnIsValid.ToString())
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00036, _udtReturnErrorCodes)

                '------------------------------------------------------
                ' Check Personal Information
                '------------------------------------------------------
                oRequest.FillEHSPersonalInformationModel(udtEHSPersonalInfo)

                'Select Case oRequest.DocType
                '    Case DocTypeModel.DocTypeCode.HKIC
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.HKIC)
                '    Case DocTypeModel.DocTypeCode.EC
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.HKIC)
                '    Case DocTypeModel.DocTypeCode.HKBC
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.RegNo)
                '    Case DocTypeModel.DocTypeCode.ADOPC
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.EntryNo)
                '    Case DocTypeModel.DocTypeCode.REPMT
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.PermitNo)
                '    Case DocTypeModel.DocTypeCode.DI
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.DocumentNo)
                '    Case DocTypeModel.DocTypeCode.VISA
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.HKIC)
                '    Case DocTypeModel.DocTypeCode.ID235B
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.BirthEntryNo)
                'End Select
                strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, udtEHSPersonalInfo.IdentityNum, Nothing, Nothing, udtAuditLog, Nothing)

                Select Case strValidateResult
                    Case ValidateAccountLookupResult.AccountFound
                        Me._strAccountMatched = "Y"
                    Case ValidateAccountLookupResult.AccountNotFound
                        Me._strAccountMatched = "X"
                    Case ValidateAccountLookupResult.InfoNotMatch, ValidateAccountLookupResult.TempAccountFound, ValidateAccountLookupResult.Deceased
                        ' CRE11-007
                        ' Handle decease case too
                        Me._strAccountMatched = "N"
                End Select

                udtAuditLog.AddDescripton("Found", strValidateResult)
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00037, _udtReturnErrorCodes)

                Return True
            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.eHSValidatedAccountQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
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
                GenAccountMatchResult(xml, nodeResult)
            Else
                GenReturnCode(xml, nodeResult)
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

        Private Sub GenAccountMatchResult(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)
            Dim nodeAccMatched As XmlElement
            nodeAccMatched = xml.CreateElement(TAG_ACCOUNT_MATCHED)
            nodeAccMatched.InnerText = Me._strAccountMatched
            nodeResult.AppendChild(nodeAccMatched)
        End Sub

#End Region

    End Class

End Namespace



