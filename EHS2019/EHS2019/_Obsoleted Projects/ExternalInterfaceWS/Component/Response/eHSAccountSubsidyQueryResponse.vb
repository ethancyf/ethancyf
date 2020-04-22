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
Imports Common.Component.Scheme
Imports ExternalInterfaceWS.Component.ErrorInfo
Imports ExternalInterfaceWS.ComObject
Imports Common.Component
Imports Common.ComFunction

Namespace Component.Response

    Public Class eHSAccountSubsidyQueryResponse

#Region "Private Constant"

        Private Const TAG_EHS_RESPONSE As String = "Response"
        Private Const TAG_OUTPUT As String = "Output"
        Private Const TAG_ERROR_INFO As String = "ErrorInfo"
        Private Const TAG_ERROR_CODE As String = "ErrorCode"
        Private Const TAG_ERROR_MESSAGE As String = "ErrorMessage"

        Private Const TAG_SCHEME_INFO As String = "SchemeInfo"
        Private Const TAG_SCHEME_CODE As String = "SchemeCode"
        Private Const TAG_VOUCHER_REMAINED As String = "VoucherRemained"

        Private Const HCVS As String = "HCVS"
        Private Const EHCVS As String = "EHCVS"

#End Region

#Region "Properties"

        Private _udtReturnErrorCodes As ErrorInfoModelCollection = New ErrorInfoModelCollection
        Public ReadOnly Property ReturnErrorCodes() As ErrorInfoModelCollection
            Get
                Return _udtReturnErrorCodes
            End Get
        End Property

        Private _strSchemeCode As String = String.Empty
        Public ReadOnly Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
        End Property

        Private _intVoucherRemained As Integer = 0
        Public ReadOnly Property VoucherRemained() As String
            Get
                Return _intVoucherRemained
            End Get
        End Property


#End Region

#Region "Constructor"

        Public Sub New()
            'Do Nothing
        End Sub

#End Region

        Public Function ProcessRequest(ByVal oRequest As eHSAccountSubsidyQueryRequest, ByVal strSystemName As String, ByRef udtAuditLog As ExtAuditLogEntry) As Boolean
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
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00044, _udtReturnErrorCodes)
                '------------------------------------------------------
                ' Check Personal Information
                '------------------------------------------------------
                Dim udtDB As Database = New Database
                Dim udtEHSPersonalInfo As EHSPersonalInformationModel = New EHSPersonalInformationModel
                Dim udtValidateAccountBLL As ValidateAccountBLL = New ValidateAccountBLL
                Dim strValidateResult As String = String.Empty
                Dim udtEHSAccount As EHSAccountModel = Nothing

                oRequest.FillEHSPersonalInformationModel(udtEHSPersonalInfo)

                'Select Case oRequest.DocType
                '    Case DocTypeModel.DocTypeCode.HKIC
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.HKIC, udtEHSAccount)
                '    Case DocTypeModel.DocTypeCode.EC
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.HKIC, udtEHSAccount)
                '    Case DocTypeModel.DocTypeCode.HKBC
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.RegNo, udtEHSAccount)
                '    Case DocTypeModel.DocTypeCode.ADOPC
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.EntryNo, udtEHSAccount)
                '    Case DocTypeModel.DocTypeCode.REPMT
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.PermitNo, udtEHSAccount)
                '    Case DocTypeModel.DocTypeCode.DI
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.DocumentNo, udtEHSAccount)
                '    Case DocTypeModel.DocTypeCode.VISA
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.HKIC, udtEHSAccount)
                '    Case DocTypeModel.DocTypeCode.ID235B
                '        strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, oRequest.BirthEntryNo, udtEHSAccount)
                'End Select
                strValidateResult = udtValidateAccountBLL.ValidatedAccountQuery(udtDB, udtEHSPersonalInfo, oRequest.DocType, udtEHSPersonalInfo.IdentityNum, udtEHSAccount, Nothing, udtAuditLog, Nothing)

                If Not strValidateResult = ValidateAccountLookupResult.AccountFound Then
                    _udtReturnErrorCodes.Add(ErrorCodeList.I00047) 'Invalid eHS account information or eHS account status
                    Return False
                End If

                udtAuditLog.AddDescripton("Found", strValidateResult)
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00045, _udtReturnErrorCodes)
                '------------------------------------------------------
                ' Check if the patient is eligible for HCVS (Aged 70 or above)
                '------------------------------------------------------
                Dim udtGeneralFunction As New GeneralFunction
                Dim udtClaimRuleBLL As New Common.Component.ClaimRules.ClaimRulesBLL
                Dim udtResult As Common.Component.ClaimRules.ClaimRulesBLL.EligibleResult

                ' ---------------------------
                ' INT10-0003: Eligibility Checking Bug on Year End
                ' ---------------------------

                'udtResult = udtClaimRuleBLL.CheckEligibilityFromHCVR(HCVS, udtEHSAccount.getPersonalInformation(oRequest.DocType), udtGeneralFunction.GetSystemDateTime())
                udtResult = udtClaimRuleBLL.CheckEligibilityFromHCVR(HCVS, udtEHSAccount.getPersonalInformation(oRequest.DocType), udtGeneralFunction.GetSystemDateTime().Date)

                ' ---------------------------
                ' End of INT10-0003
                ' ---------------------------

                If Not udtResult.IsEligible Then
                    _udtReturnErrorCodes.Add(ErrorCodeList.I00059)
                    Return False
                End If

                '------------------------------------------------------
                ' Check Voucher Remained 
                '------------------------------------------------------
                Dim udtEHSTransactionBLL As New EHSTransactionBLL
                Dim udtSchemeClaimBLL As New SchemeClaimBLL
                Dim udtSchemeCList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllEffectiveSchemeClaim_WithSubsidizeGroup()
                Dim udtSchemeC As SchemeClaimModel = udtSchemeCList.Filter(HCVS)
                Dim udtSubsidizeGroupC As SubsidizeGroupClaimModel = udtSchemeC.SubsidizeGroupClaimList.Filter(HCVS, EHCVS)
                Dim intAvailableVoucher As Integer = udtEHSTransactionBLL.getAvailableVoucher(udtSchemeC, udtEHSAccount.getPersonalInformation(oRequest.DocType))

                _strSchemeCode = HCVS
                _intVoucherRemained = intAvailableVoucher

                udtAuditLog.AddDescripton("VoucherRemained", _intVoucherRemained.ToString())
                udtAuditLog.WriteLogWithErrorList_Ext(LogID.LOG00046, _udtReturnErrorCodes)

                Return True
            Catch ex As Exception
                ErrorLogHandler.getInstance(EnumAuditLog.eHSAccountSubsidyQuery).WriteSystemLogToDB("[Message]:" + ex.Message + " [Stack]:" + ex.StackTrace)
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
                GenQueryResult(xml, nodeResult)
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

        Private Sub GenQueryResult(ByVal xml As XmlDocument, ByVal nodeResult As XmlElement)

            Dim nodeSchemeInfo As XmlElement
            nodeSchemeInfo = xml.CreateElement(TAG_SCHEME_INFO)
            nodeResult.AppendChild(nodeSchemeInfo)

            Dim nodeSchemeCode As XmlElement
            nodeSchemeCode = xml.CreateElement(TAG_SCHEME_CODE)
            nodeSchemeCode.InnerText = Me.SchemeCode
            nodeSchemeInfo.AppendChild(nodeSchemeCode)

            Dim nodeVoucherRemained As XmlElement
            nodeVoucherRemained = xml.CreateElement(TAG_VOUCHER_REMAINED)
            nodeVoucherRemained.InnerText = Me.VoucherRemained
            nodeSchemeInfo.AppendChild(nodeVoucherRemained)
        End Sub

#End Region

    End Class

End Namespace


