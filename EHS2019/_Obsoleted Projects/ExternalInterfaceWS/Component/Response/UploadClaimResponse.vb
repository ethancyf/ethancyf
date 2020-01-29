Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Xml
Imports ExternalInterfaceWS.Component.Request
Imports Common.DataAccess
Imports ExternalInterfaceWS.BLL
Imports ExternalInterfaceWS.BLL.ValidateAccountBLL
Imports Common.Component.DocType
Imports Common.Component.EHSAccount
Imports Common.Component.EHSAccount.EHSAccountModel
Imports Common.Component.EHSTransaction
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports Common.Component
Imports ExternalInterfaceWS.ComObject
Imports Common.Format

Namespace Component.Response

    Public Class UploadClaimResponse

#Region "Private Constant"

        Private Const TAG_EHS_RESPONSE As String = "Response"
        Private Const TAG_OUTPUT As String = "Output"
        Private Const TAG_ERROR_INFO As String = "ErrorInfo"
        Private Const TAG_ERROR_CODE As String = "ErrorCode"
        Private Const TAG_ERROR_MESSAGE As String = "ErrorMessage"

        Private Const TAG_IS_SUCCESS As String = "IsSuccess"
        Private Const TAG_TRAN_INFO As String = "TranInfo"
        Private Const TAG_TRAN_INDEX As String = "TranIndex"
        Private Const TAG_TRAN_ID As String = "TranID"

#End Region

#Region "Properties"

        Private _udtReturnErrorCodes As ErrorInfoModelCollection = New ErrorInfoModelCollection
        Public ReadOnly Property ReturnErrorCodes() As ErrorInfoModelCollection
            Get
                Return _udtReturnErrorCodes
            End Get
        End Property

        Private _udtUploadClaimOutput As UploadClaimOutput = Nothing
        Public ReadOnly Property UploadClaimOutput() As UploadClaimOutput
            Get
                Return _udtUploadClaimOutput
            End Get
        End Property

#End Region

#Region "Constructor"

        Public Sub New()
            'Do Nothing
        End Sub

#End Region

        Public Function ProcessRequest(ByVal oRequest As UploadClaimRequest_HL7, ByVal strSystemName As String, ByRef udtAuditLog As ExtAuditLogEntry) As Boolean
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
                udtAuditLog.AddDescripton_Ext("SPID", oRequest.SPID)
                udtAuditLog.AddDescripton_Ext("PracticeID", oRequest.PracticeID)
                udtAuditLog.AddDescripton_Ext("PracticeName", oRequest.PracticeName)
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00066, _udtReturnErrorCodes)

                Dim blnIsValid As Boolean = True
                blnIsValid = SPPracticeBLL.chkServiceProviderInfo(oRequest.SPID, _
                                                        oRequest.PracticeID, _
                                                        oRequest.PracticeName, _
                                                        _udtReturnErrorCodes, _
                                                        String.Empty, _
                                                        strSystemName, _
                                                        False)

                Dim strIsValid = "Yes"
                If Not blnIsValid Then
                    strIsValid = "No"
                End If
                

                If Not blnIsValid Then
                    Return False
                End If

                udtAuditLog.AddDescripton("IsValid", blnIsValid.ToString())
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00063, _udtReturnErrorCodes)

                '------------------------------------------------------
                ' Check Personal Information
                '------------------------------------------------------
                Dim udtDB As Database = New Database
                Dim udtEHSPersonalInfo As EHSPersonalInformationModel = New EHSPersonalInformationModel
                Dim udtValidateAccountBLL As ValidateAccountBLL = New ValidateAccountBLL
                Dim strValidateResult As String = String.Empty
                Dim blnPersonalInfoValid As Boolean = False

                udtAuditLog.WriteLog_Ext(LogID.LOG00067)
                oRequest.FillEHSPersonalInformationModel(udtEHSPersonalInfo)
                udtAuditLog.WriteLog_Ext(LogID.LOG00068)

                strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, udtEHSPersonalInfo.IdentityNum, Nothing, Nothing, udtAuditLog, Nothing)
                udtAuditLog.AddDescripton_Ext("Account Validate Result", strValidateResult)
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00069, _udtReturnErrorCodes)

                Select Case strValidateResult
                    Case ValidateAccountLookupResult.AccountFound, ValidateAccountLookupResult.AccountNotFound, ValidateAccountLookupResult.TempAccountFound
                        blnPersonalInfoValid = True
                    Case ValidateAccountLookupResult.InfoNotMatch, ValidateAccountLookupResult.Deceased
                        ' CRE11-007
                        ' Handle decease case too
                        blnPersonalInfoValid = False
                End Select


                If blnPersonalInfoValid Then

                    '------------------------------------------------------
                    ' Retrieve EHS Transaction Model
                    '------------------------------------------------------
                    Dim udtEHSAccountBLL As EHSAccountBLL = New EHSAccountBLL()
                    Dim udtEHSTransactions As EHSTransactionModelCollection = New EHSTransactionModelCollection()
                    Dim udtEHSAccount As New EHSAccountModel()
                    Dim udtWarningIndicatorList As Hashtable = New Hashtable()

                    If strValidateResult = ValidateAccountLookupResult.AccountFound Then
                        udtAuditLog.AddDescripton_Ext("Identity Num", udtEHSPersonalInfo.IdentityNum)
                        udtAuditLog.AddDescripton_Ext("Doc Code", udtEHSPersonalInfo.DocCode)
                        udtAuditLog.WriteLog_Ext(LogID.LOG00070)
                        udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(udtEHSPersonalInfo.IdentityNum, udtEHSPersonalInfo.DocCode)
                        udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00071, _udtReturnErrorCodes)
                    ElseIf strValidateResult = ValidateAccountLookupResult.AccountNotFound Or strValidateResult = ValidateAccountLookupResult.TempAccountFound Then
                        udtEHSAccount.SetPersonalInformation(udtEHSPersonalInfo)
                        udtEHSAccount.RecordStatus = EHSAccountModel.TempAccountRecordStatusClass.PendingVerify
                        udtEHSAccount.SchemeCode = oRequest.WSClaimDetailList.Item(0).SchemeCode
                        udtEHSAccount.CreateSPID = oRequest.SPID
                        udtEHSAccount.CreateSPPracticeDisplaySeq = oRequest.PracticeID
                    End If

                    udtAuditLog.WriteLog_Ext(LogID.LOG00072)
                    oRequest.FillEHSTransactionModel(udtEHSTransactions, udtEHSAccount, oRequest.SPID, oRequest.PracticeID, Nothing, _udtReturnErrorCodes)
                    If (Not _udtReturnErrorCodes Is Nothing AndAlso _udtReturnErrorCodes.Count > 0) = False Then


                        udtAuditLog.WriteLog_Ext(LogID.LOG00073)
                        oRequest.FillWarningIndicatorList(udtWarningIndicatorList)
                        udtAuditLog.WriteLog_Ext(LogID.LOG00074)
                        '------------------------------------------------------
                        ' Check Claim Information and Create Claim
                        '------------------------------------------------------
                        Dim udtUploadClaimBLL As UploadClaimBLL = New UploadClaimBLL()

                        udtAuditLog.WriteLog_Ext(LogID.LOG00075)
                        _udtUploadClaimOutput = udtUploadClaimBLL.UploadClaimMain(udtEHSTransactions, udtEHSPersonalInfo.DocCode, udtEHSPersonalInfo.IdentityNum, udtWarningIndicatorList, strSystemName, udtAuditLog)
                        If Not _udtUploadClaimOutput.ErrorInfoModelCollection Is Nothing Then
                            udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00076, _udtUploadClaimOutput.ErrorInfoModelCollection)
                        Else
                            udtAuditLog.WriteLog_Ext(LogID.LOG00076)
                        End If
                        If Not IsNothing(_udtUploadClaimOutput) Then
                            If _udtUploadClaimOutput.IsSuccess = True Then
                                'Nothing to do
                            Else
                                _udtReturnErrorCodes = _udtUploadClaimOutput.ErrorInfoModelCollection
                            End If
                        Else
                            _udtReturnErrorCodes.Add(ErrorCodeList.I99999)
                        End If
                    Else
                        _udtReturnErrorCodes.Add(ErrorCodeList.I00047)
                    End If

                    Return True
                Else
                    _udtReturnErrorCodes.Add(ErrorCodeList.I00047)
                    Return False
                End If
            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.UploadClaim).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
                _udtReturnErrorCodes.Add(ErrorCodeList.I99999)
                Return False
            End Try
            Return True
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
                GenQueryResult(xml, nodeResult)
            Else
                GenErrorInfo(xml, nodeResult)
            End If

            Return xml
        End Function

        Private Sub GenErrorInfo(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)

            'Is Success
            Dim nodeIsSuccess As XmlElement
            nodeIsSuccess = xml.CreateElement(TAG_IS_SUCCESS)
            nodeIsSuccess.InnerText = "N"
            nodeResult.AppendChild(nodeIsSuccess)



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

        Private Sub GenQueryResult(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)

            Dim udtFormatter As New Formatter

            Dim nodeIsSuccess As XmlElement
            nodeIsSuccess = xml.CreateElement(TAG_IS_SUCCESS)
            nodeIsSuccess.InnerText = _udtUploadClaimOutput.IsSuccess
            nodeResult.AppendChild(nodeIsSuccess)

            For Each udtTranInfo As TranInfoModel In _udtUploadClaimOutput.TranInfoCollection
                Dim nodeInfo As XmlElement
                nodeInfo = xml.CreateElement(TAG_TRAN_INFO)
                nodeResult.AppendChild(nodeInfo)

                Dim nodeTmp As XmlElement

                'Tran Index
                nodeTmp = xml.CreateElement(TAG_TRAN_INDEX)
                nodeTmp.InnerText = udtTranInfo.TranIndex
                nodeInfo.AppendChild(nodeTmp)

                'Tran ID
                nodeTmp = xml.CreateElement(TAG_TRAN_ID)
                nodeTmp.InnerText = udtFormatter.formatSystemNumber(udtTranInfo.TranID)
                nodeInfo.AppendChild(nodeTmp)
            Next
        End Sub

#End Region
    End Class

End Namespace


