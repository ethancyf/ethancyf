Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.EHSAccount
Imports Common.Component.Mapping
Imports Common.Component.RSA_Manager
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.Token
Imports Common.Component.Token.TokenBLL
Imports Common.Component.UserAC
Imports Common.Component.VoucherInfo
Imports Common.DataAccess
Imports Common.eHRIntegration.DAL
Imports Common.eHRIntegration.Model.Xml.eHSService
Imports Common.Format
Imports Common.ComObject
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Text
Imports System.Text.RegularExpressions
Imports Common.ComFunction.AccountSecurity
Imports System.Web


Namespace BLL

#Region "Constants"

    Public Class DHCConstant
        Public Const EHRUPDATEBY As String = "eHRSS"
    End Class
    'CRE20-006 DHC integeration interface [Start][Nichole]
    Public Enum EnumDHCSearchType
        SPID
        DELISTED
        NSP
        PROF
        VALIDSP
    End Enum
    'CRE20-006 DHC integeration interface [End][Nichole]
#End Region


    Public Class eHSDHCServiceBLL

        Public Function GeteHSSTokenInfo(udtInXml As InGeteHSSTokenInfoXmlModel, ByRef strStackTrace As String) As OutGeteHSSTokenInfoXmlModel
            Dim udtOutXml As OutGeteHSSTokenInfoXmlModel = Nothing

            ' --- Validate In Xml ---

            ' HKID cannot be null
            If IsNothing(udtInXml.HKID) Then
                udtOutXml = New OutGeteHSSTokenInfoXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "HKID is empty"
                Return udtOutXml

            End If

            ' Timestamp cannot be null
            If IsNothing(udtInXml.Timestamp) Then
                udtOutXml = New OutGeteHSSTokenInfoXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Timestamp is empty"
                Return udtOutXml

            End If

            ' HKID must be in length 8 or 9 (after trimmed and excluding parentheses)
            If (New Regex("^.{8,9}$")).IsMatch(New Regex("[()]").Replace(udtInXml.HKID.Trim, String.Empty)) = False Then
                udtOutXml = New OutGeteHSSTokenInfoXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Error in HKID"
                Return udtOutXml

            End If

            ' Format
            udtInXml.HKID = (New Formatter).formatHKIDInternal(udtInXml.HKID)
            udtInXml.Timestamp = udtInXml.Timestamp.Trim

            ' --- End of validation ---

            Dim udtToken As TokenModel = Nothing

            Dim blnIsCommonUser As Boolean = IsCommonUser(udtInXml.HKID, String.Empty, udtToken, Nothing)

            If blnIsCommonUser = False Then
                ' R9003_UserNotFound
                udtOutXml = New OutGeteHSSTokenInfoXmlModel(eHSResultCode.R9002_UserNotFound, udtInXml.HKID, udtInXml.Timestamp)

                udtOutXml.IsCommonUser = YesNo.No

                Return udtOutXml

            End If

            If IsNothing(udtToken) Then
                ' R1001_NoTokenAssigned
                udtOutXml = New OutGeteHSSTokenInfoXmlModel(eHSResultCode.R1001_NoTokenAssigned, udtInXml.HKID, udtInXml.Timestamp)

                udtOutXml.IsCommonUser = YesNo.Yes

                Return udtOutXml

            End If

            ' R1000_Success
            udtOutXml = New OutGeteHSSTokenInfoXmlModel(eHSResultCode.R1000_Success, udtInXml.HKID, udtInXml.Timestamp)

            udtOutXml.IsCommonUser = YesNo.Yes
            udtOutXml.ExistingTokenID = udtToken.TokenSerialNo.TrimStart("0".ToCharArray)
            udtOutXml.ExistingTokenIssuer = udtToken.Project
            udtOutXml.IsExistingTokenShared = IIf(udtToken.IsShareToken, YesNo.Yes, YesNo.No)

            If udtToken.TokenSerialNoReplacement <> String.Empty Then
                udtOutXml.NewTokenID = udtToken.TokenSerialNoReplacement.TrimStart("0".ToCharArray)
                udtOutXml.NewTokenIssuer = udtToken.ProjectReplacement
                udtOutXml.IsNewTokenShared = IIf(udtToken.IsShareTokenReplacement, YesNo.Yes, YesNo.No)
            End If

            Return udtOutXml

        End Function

        '

        Public Function SeteHSSTokenShared(udtInXml As InSeteHSSTokenSharedXmlModel, dtmNotificationDtm As DateTime, ByRef strStackTrace As String) As OutSeteHSSTokenSharedXmlModel
            Dim strUserID As String = String.Empty
            Dim udtOutXml As OutSeteHSSTokenSharedXmlModel = HandleSeteHSSTokenShared(udtInXml, dtmNotificationDtm, strUserID, strStackTrace)

            Dim eResultCode As EnumTokenActionActionResult = Nothing

            Select Case udtOutXml.ResultCodeEnum
                Case eHSResultCode.R1000_Success
                    eResultCode = EnumTokenActionActionResult.C
                Case Else
                    eResultCode = EnumTokenActionActionResult.R
            End Select

            Call (New TokenBLL).AddTokenAction(EnumTokenActionParty.EHR, EnumTokenActionParty.EHS, EnumTokenActionActionType.NOTIFYSETSHARE, _
                                               strUserID, udtInXml.ExistingTokenID, udtInXml.NewTokenID, udtInXml.Shared, _
                                               False, eResultCode, udtInXml.Timestamp, dtmNotificationDtm, udtInXml.Timestamp, String.Empty)

            Return udtOutXml

        End Function

        Private Function HandleSeteHSSTokenShared(udtInXml As InSeteHSSTokenSharedXmlModel, dtmNotificationDtm As DateTime, _
                                                  ByRef strUserID As String, ByRef strStackTrace As String) As OutSeteHSSTokenSharedXmlModel
            ' --- Validate In Xml ---

            Dim udtOutXml As OutSeteHSSTokenSharedXmlModel = Nothing

            ' HKID cannot be null
            If IsNothing(udtInXml.HKID) Then
                udtOutXml = New OutSeteHSSTokenSharedXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "HKID is empty"
                Return udtOutXml

            End If

            ' Timestamp cannot be null
            If IsNothing(udtInXml.Timestamp) Then
                udtOutXml = New OutSeteHSSTokenSharedXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Timestamp is empty"
                Return udtOutXml

            End If

            ' HKID must be in length 8 or 9 (after trimmed and excluding parentheses)
            If (New Regex("^.{8,9}$")).IsMatch(New Regex("[()]").Replace(udtInXml.HKID.Trim, String.Empty)) = False Then
                udtOutXml = New OutSeteHSSTokenSharedXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Error in HKID"
                Return udtOutXml

            End If

            ' Shared must be Y or N
            If (New Regex("^[YN]$")).IsMatch(udtInXml.Shared.Trim) = False Then
                udtOutXml = New OutSeteHSSTokenSharedXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Error in Shared"
                Return udtOutXml

            End If

            ' Format
            udtInXml.HKID = (New Formatter).formatHKIDInternal(udtInXml.HKID)
            udtInXml.ExistingTokenID = udtInXml.ExistingTokenID.Trim

            If IsNothing(udtInXml.NewTokenID) Then
                udtInXml.NewTokenID = String.Empty
            Else
                udtInXml.NewTokenID = udtInXml.NewTokenID.Trim
            End If

            udtInXml.Shared = udtInXml.Shared.Trim
            udtInXml.Timestamp = udtInXml.Timestamp.Trim

            ' Check user and token
            Dim udtToken As TokenModel = Nothing
            Dim udtSP As ServiceProviderModel = Nothing
            Dim blnIsCommonUser As Boolean = IsCommonUser(udtInXml.HKID, strUserID, udtToken, udtSP)

            If blnIsCommonUser = False Then
                udtOutXml = New OutSeteHSSTokenSharedXmlModel(eHSResultCode.R9002_UserNotFound, udtInXml.HKID, udtInXml.Timestamp)
                Return udtOutXml

            End If

            If IsNothing(udtToken) Then
                udtOutXml = New OutSeteHSSTokenSharedXmlModel(eHSResultCode.R1001_NoTokenAssigned, udtInXml.HKID, udtInXml.Timestamp)
                Return udtOutXml

            End If

            If udtToken.Project <> TokenProjectType.EHCVS Then
                Throw New Exception("eHSServiceBLL.SeteHSSTokenShared: Token Project in EHS database should be EHS")
            End If

            If udtToken.IsShareToken = False AndAlso udtInXml.Shared = YesNo.Yes Then
                ' If the request party wants to set from N to Y, the latest Token Serial No. must be correct
                Dim strLatestEHSToken As String = IIf(udtToken.TokenSerialNoReplacement <> String.Empty, udtToken.TokenSerialNoReplacement, udtToken.TokenSerialNo)
                Dim strLatestEHRToken As String = IIf(udtInXml.NewTokenID <> String.Empty, udtInXml.NewTokenID, udtInXml.ExistingTokenID)

                If strLatestEHSToken <> strLatestEHRToken Then
                    udtOutXml = New OutSeteHSSTokenSharedXmlModel(eHSResultCode.R1002_TokenNotMatch, udtInXml.HKID, udtInXml.Timestamp)
                    Return udtOutXml

                End If

            End If

            ' --- End of validation ---

            ' Update the token Shared flag
            udtToken.IsShareToken = IIf(udtInXml.Shared = YesNo.Yes, True, False)

            If udtToken.IsShareTokenReplacement.HasValue Then
                udtToken.IsShareTokenReplacement = IIf(udtInXml.Shared = YesNo.Yes, True, False)
            End If

            udtToken.UpdateBy = DHCConstant.EHRUPDATEBY

            Dim udtDB As New Database
            Dim udtTokenBLL As New TokenBLL

            Try
                udtDB.BeginTransaction()

                udtTokenBLL.UpdateTokenIsShare(udtToken, udtDB)

                Call (New ServiceProviderBLL).UpdateServiceProviderDataInput(udtSP.SPID, DHCConstant.EHRUPDATEBY, DHCConstant.EHRUPDATEBY, udtSP.TSMP, udtDB)

                udtDB.CommitTransaction()

            Catch ex As System.Exception
                udtDB.RollBackTranscation()
                Throw

            End Try

            udtOutXml = New OutSeteHSSTokenSharedXmlModel(eHSResultCode.R1000_Success, udtInXml.HKID, udtInXml.Timestamp)
            Return udtOutXml

        End Function

        '

        Public Function ReplaceeHSSToken(udtInXml As InReplaceeHSSTokenXmlModel, dtmNotificationDtm As DateTime, ByRef strStackTrace As String) As OutReplaceeHRSSTokenXmlModel
            Dim strUserID As String = String.Empty
            Dim blnUpdateImmediate As Boolean = False
            Dim udtOutXml As OutReplaceeHRSSTokenXmlModel = HandleReplaceeHSSToken(udtInXml, dtmNotificationDtm, strUserID, blnUpdateImmediate, strStackTrace)

            Dim eResultCode As EnumTokenActionActionResult = Nothing

            Select Case udtOutXml.ResultCodeEnum
                Case eHSResultCode.R1000_Success
                    eResultCode = EnumTokenActionActionResult.C
                Case Else
                    eResultCode = EnumTokenActionActionResult.R
            End Select

            Call (New TokenBLL).AddTokenAction(EnumTokenActionParty.EHR, EnumTokenActionParty.EHS, _
                                               IIf(blnUpdateImmediate, EnumTokenActionActionType.NOTIFYREPLACETOKENIMMEDIATE, EnumTokenActionActionType.NOTIFYREPLACETOKEN), _
                                               strUserID, udtInXml.ExistingTokenID, udtInXml.NewTokenID, udtInXml.ReplaceReasonCode, _
                                               False, eResultCode, udtInXml.Timestamp, dtmNotificationDtm, udtInXml.Timestamp, String.Empty)

            Return udtOutXml

        End Function

        Private Function HandleReplaceeHSSToken(udtInXml As InReplaceeHSSTokenXmlModel, dtmNotificationDtm As DateTime, _
                                                ByRef strUserID As String, ByRef blnUpdateImmediate As Boolean, ByRef strStackTrace As String) As OutReplaceeHRSSTokenXmlModel
            ' --- Validate In Xml ---

            Dim udtOutXml As OutReplaceeHRSSTokenXmlModel = Nothing

            ' HKID cannot be null
            If IsNothing(udtInXml.HKID) OrElse udtInXml.HKID.Trim = String.Empty Then
                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "HKID is empty"
                Return udtOutXml

            End If

            ' ExistingTokenID cannot be null
            If IsNothing(udtInXml.ExistingTokenID) OrElse udtInXml.ExistingTokenID.Trim = String.Empty Then
                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "ExistingTokenID is empty"
                Return udtOutXml

            End If

            ' NewTokenID cannot be null
            If IsNothing(udtInXml.NewTokenID) OrElse udtInXml.NewTokenID.Trim = String.Empty Then
                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "NewTokenID is empty"
                Return udtOutXml

            End If

            ' ReplaceReasonCode cannot be null
            If IsNothing(udtInXml.ReplaceReasonCode) OrElse udtInXml.ReplaceReasonCode.Trim = String.Empty Then
                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "ReplaceReasonCode is empty"
                Return udtOutXml

            End If

            ' Timestamp cannot be null
            If IsNothing(udtInXml.Timestamp) OrElse udtInXml.Timestamp.Trim = String.Empty Then
                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Timestamp is empty"
                Return udtOutXml

            End If

            ' HKID must be in length 8 or 9 (after trimmed and excluding parentheses)
            If (New Regex("^.{8,9}$")).IsMatch(New Regex("[()]").Replace(udtInXml.HKID.Trim, String.Empty)) = False Then
                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Error in HKID"
                Return udtOutXml

            End If

            ' ReplaceReasonCode must be in our static data
            Dim blnReplaceFoundReason As Boolean = False
            Dim blnUpdateFoundReason As Boolean = False

            For Each udtStaticData As StaticDataModel In (New StaticDataBLL).GetStaticDataListByColumnName("TokenReplaceReasonEHR_Replace")
                If udtStaticData.ItemNo = udtInXml.ReplaceReasonCode.Trim Then
                    blnReplaceFoundReason = True
                    Exit For
                End If
            Next

            If blnReplaceFoundReason = False Then
                For Each udtStaticData As StaticDataModel In (New StaticDataBLL).GetStaticDataListByColumnName("TokenReplaceReasonEHR_Update")
                    If udtStaticData.ItemNo = udtInXml.ReplaceReasonCode.Trim Then
                        blnUpdateFoundReason = True
                        blnUpdateImmediate = True
                        Exit For
                    End If
                Next

            End If

            If blnReplaceFoundReason = False AndAlso blnUpdateFoundReason = False Then
                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Error in ReplaceReasonCode"
                Return udtOutXml

            End If

            ' Format
            udtInXml.HKID = (New Formatter).formatHKIDInternal(udtInXml.HKID)
            udtInXml.ExistingTokenID = udtInXml.ExistingTokenID.Trim
            udtInXml.NewTokenID = udtInXml.NewTokenID.Trim
            udtInXml.ReplaceReasonCode = udtInXml.ReplaceReasonCode.Trim
            udtInXml.Timestamp = udtInXml.Timestamp.Trim

            ' --- End of validation ---

            Dim udtToken As TokenModel = Nothing
            Dim udtSP As ServiceProviderModel = Nothing
            Dim blnIsCommonUser As Boolean = IsCommonUser(udtInXml.HKID, strUserID, udtToken, udtSP)

            If blnIsCommonUser = False Then
                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9002_UserNotFound, udtInXml.HKID, udtInXml.Timestamp)
                Return udtOutXml

            End If

            If IsNothing(udtToken) Then
                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R1001_NoTokenAssigned, udtInXml.HKID, udtInXml.Timestamp)
                Return udtOutXml

            End If

            If udtToken.Project <> TokenProjectType.EHR Then
                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R1005_ExistingTokenNotIssuedBySenderParty, udtInXml.HKID, udtInXml.Timestamp)
                Return udtOutXml

            End If

            Dim udtTokenBLL As New TokenBLL
            Dim udtRSAServerHandler As New RSAServerHandler

            If udtTokenBLL.IsEnableToken Then
                ' Check the new token in RSA server
                If udtRSAServerHandler.IsTokenExistAndFreeToAssign(udtInXml.NewTokenID, DBFlagStr.DBFlagInterfaceLog) = False Then
                    udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R1006_NewTokenNotAvailable, udtInXml.HKID, udtInXml.Timestamp)
                    Return udtOutXml

                End If

            Else
                ' Just check if empty
                If udtInXml.NewTokenID = String.Empty Then
                    udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R1006_NewTokenNotAvailable, udtInXml.HKID, udtInXml.Timestamp)
                    Return udtOutXml

                End If

            End If

            ' Replace Token

            ' Update eHS Server
            Dim udtDB As New Database

            If blnReplaceFoundReason Then
                udtDB.BeginTransaction()

                udtToken.TokenSerialNoReplacement = udtInXml.NewTokenID
                udtToken.LastReplacementDtm = (New GeneralFunction).GetSystemDateTime
                udtToken.LastReplacementActivateDtm = Nothing
                udtToken.LastReplacementReason = udtInXml.ReplaceReasonCode
                udtToken.LastReplacementBy = DHCConstant.EHRUPDATEBY
                udtToken.ProjectReplacement = TokenProjectType.EHR
                udtToken.IsShareTokenReplacement = True
                udtToken.UpdateBy = DHCConstant.EHRUPDATEBY

                udtTokenBLL.UpdateTokenReplacementNo(udtToken, udtDB)

                Call (New ServiceProviderBLL).UpdateServiceProviderDataInput(udtSP.SPID, DHCConstant.EHRUPDATEBY, DHCConstant.EHRUPDATEBY, udtSP.TSMP, udtDB)

                Try
                    If udtTokenBLL.IsEnableToken Then
                        Dim strResultCode As String = udtRSAServerHandler.replaceRSAUserToken(udtToken.TokenSerialNo, udtInXml.NewTokenID, DBFlagStr.DBFlagInterfaceLog)

                        Select Case strResultCode
                            Case "0"
                                ' Fine

                            Case "1"
                                ' Two numbers same (CurrentSerial = ReplacementSerial)
                                ' Current Token is already a replacement token
                                ' This replacement is already in progress
                                ' Replacement token is assigned to someone else
                                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9999_UnexpectedFailure, udtInXml.HKID, udtInXml.Timestamp)
                                strStackTrace = String.Format("Unexpected return from RSA (ResultCode={0})", strResultCode)
                                Return udtOutXml

                            Case "2"
                                ' Current token not found
                                ' Current token not assigned to any user
                                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9999_UnexpectedFailure, udtInXml.HKID, udtInXml.Timestamp)
                                strStackTrace = String.Format("Unexpected return from RSA (ResultCode={0})", strResultCode)
                                Return udtOutXml

                            Case "3"
                                ' Current token non-numeric
                                ' Replacement token non-numeric
                                ' Replacement token not found
                                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9999_UnexpectedFailure, udtInXml.HKID, udtInXml.Timestamp)
                                strStackTrace = String.Format("Unexpected return from RSA (ResultCode={0})", strResultCode)
                                Return udtOutXml

                            Case "9"
                                ' InitRSA fail
                                udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R9999_UnexpectedFailure, udtInXml.HKID, udtInXml.Timestamp)
                                strStackTrace = String.Format("Unexpected return from RSA (ResultCode={0})", strResultCode)
                                Return udtOutXml

                        End Select

                    End If

                    udtDB.CommitTransaction()

                Catch eSQL As SqlException
                    udtDB.RollBackTranscation()
                    Throw

                Catch ex As System.Exception
                    udtDB.RollBackTranscation()
                    Throw

                End Try

            ElseIf blnUpdateFoundReason Then
                udtDB.BeginTransaction()

                Dim udtNewToken As New TokenModel(udtToken)
                udtNewToken.TokenSerialNo = udtInXml.NewTokenID
                udtNewToken.Project = TokenProjectType.EHR
                udtNewToken.IssueBy = DHCConstant.EHRUPDATEBY

                udtNewToken.TokenSerialNoReplacement = String.Empty
                udtNewToken.ProjectReplacement = String.Empty
                udtNewToken.IsShareTokenReplacement = Nothing
                udtNewToken.LastReplacementDtm = Nothing
                udtNewToken.LastReplacementBy = String.Empty
                udtNewToken.LastReplacementReason = String.Empty

                udtNewToken.LastReplacementActivateDtm = Nothing

                udtNewToken.UpdateBy = DHCConstant.EHRUPDATEBY

                udtTokenBLL.UpdateTokenReplacementNo(udtNewToken, udtDB)

                Call (New ServiceProviderBLL).UpdateServiceProviderDataInput(udtSP.SPID, DHCConstant.EHRUPDATEBY, DHCConstant.EHRUPDATEBY, udtSP.TSMP, udtDB)

                Try
                    If udtTokenBLL.IsEnableToken Then
                        ' Delete the RSA record
                        If udtRSAServerHandler.deleteRSAUser(udtToken.UserID, DBFlagStr.DBFlagInterfaceLog) = False Then
                            Throw New System.Exception("Fail in RSAServerHandler.deleteRSAUser")
                        End If

                        ' Add the RSA record
                        Dim udtSystemMessage As Object = udtRSAServerHandler.addRSAUser(udtNewToken.UserID, udtNewToken.TokenSerialNo, DBFlagStr.DBFlagInterfaceLog)

                        If Not IsNothing(udtSystemMessage) Then
                            Throw New System.Exception("Fail in RSAServerHandler.addRSAUser")

                        End If

                    End If

                    udtDB.CommitTransaction()

                Catch eSQL As SqlException
                    udtDB.RollBackTranscation()
                    Throw

                Catch ex As System.Exception
                    udtDB.RollBackTranscation()
                    Throw

                End Try

            End If

            udtOutXml = New OutReplaceeHRSSTokenXmlModel(eHSResultCode.R1000_Success, udtInXml.HKID, udtInXml.Timestamp)
            Return udtOutXml

        End Function

        '

        Public Function NotifyeHSSTokenDeactivated(udtInXml As InNotifyeHSSTokenDeactivatedXmlModel, dtmNotificationDtm As DateTime, ByRef strStackTrace As String) As OutNotifyeHRSSTokenDeactivatedXmlModel
            Dim strUserID As String = String.Empty
            Dim udtOutXml As OutNotifyeHRSSTokenDeactivatedXmlModel = HandleNotifyeHSSTokenDeactivated(udtInXml, dtmNotificationDtm, strUserID, strStackTrace)

            Dim eResultCode As EnumTokenActionActionResult = Nothing

            Select Case udtOutXml.ResultCodeEnum
                Case eHSResultCode.R1000_Success
                    eResultCode = EnumTokenActionActionResult.C
                Case Else
                    eResultCode = EnumTokenActionActionResult.R
            End Select

            Call (New TokenBLL).AddTokenAction(EnumTokenActionParty.EHR, EnumTokenActionParty.EHS, EnumTokenActionActionType.NOTIFYDELETETOKEN, _
                                               strUserID, udtInXml.ExistingTokenID, udtInXml.NewTokenID, udtInXml.DeactivateReasonCode, _
                                               False, eResultCode, udtInXml.Timestamp, dtmNotificationDtm, udtInXml.Timestamp, String.Empty)

            Return udtOutXml

        End Function

        Private Function HandleNotifyeHSSTokenDeactivated(udtInXml As InNotifyeHSSTokenDeactivatedXmlModel, dtmNotificationDtm As DateTime, _
                                                          ByRef strUserID As String, ByRef strStackTrace As String) As OutNotifyeHRSSTokenDeactivatedXmlModel
            ' --- Validate In Xml ---

            Dim udtOutXml As OutNotifyeHRSSTokenDeactivatedXmlModel = Nothing

            ' HKID cannot be null
            If IsNothing(udtInXml.HKID) OrElse udtInXml.HKID.Trim = String.Empty Then
                udtOutXml = New OutNotifyeHRSSTokenDeactivatedXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "HKID is empty"
                Return udtOutXml

            End If

            ' DeactivateReasonCode cannot be null
            If IsNothing(udtInXml.DeactivateReasonCode) OrElse udtInXml.DeactivateReasonCode.Trim = String.Empty Then
                udtOutXml = New OutNotifyeHRSSTokenDeactivatedXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "DeactivateReasonCode is empty"
                Return udtOutXml

            End If

            ' Timestamp cannot be null
            If IsNothing(udtInXml.Timestamp) OrElse udtInXml.Timestamp.Trim = String.Empty Then
                udtOutXml = New OutNotifyeHRSSTokenDeactivatedXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Timestamp is empty"
                Return udtOutXml

            End If

            ' HKID must be in length 8 or 9 (after trimmed and excluding parentheses)
            If (New Regex("^.{8,9}$")).IsMatch(New Regex("[()]").Replace(udtInXml.HKID.Trim, String.Empty)) = False Then
                udtOutXml = New OutNotifyeHRSSTokenDeactivatedXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Error in HKID"
                Return udtOutXml

            End If

            ' DeactivateReasonCode must be D or U
            If (New Regex("^[DU]$")).IsMatch(udtInXml.DeactivateReasonCode.Trim) = False Then
                udtOutXml = New OutNotifyeHRSSTokenDeactivatedXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Error in DeactivateReasonCode"
                Return udtOutXml

            End If

            ' Format
            udtInXml.HKID = (New Formatter).formatHKIDInternal(udtInXml.HKID)
            udtInXml.ExistingTokenID = udtInXml.ExistingTokenID.Trim

            If IsNothing(udtInXml.NewTokenID) Then
                udtInXml.NewTokenID = String.Empty
            Else
                udtInXml.NewTokenID = udtInXml.NewTokenID.Trim
            End If

            udtInXml.DeactivateReasonCode = udtInXml.DeactivateReasonCode.Trim
            udtInXml.Timestamp = udtInXml.Timestamp.Trim

            ' --- End of validation ---

            Dim udtToken As TokenModel = Nothing
            Dim udtSP As ServiceProviderModel = Nothing
            Dim blnIsCommonUser As Boolean = IsCommonUser(udtInXml.HKID, strUserID, udtToken, udtSP)

            If blnIsCommonUser = False Then
                udtOutXml = New OutNotifyeHRSSTokenDeactivatedXmlModel(eHSResultCode.R9002_UserNotFound, udtInXml.HKID, udtInXml.Timestamp)
                Return udtOutXml

            End If

            If IsNothing(udtToken) Then
                udtOutXml = New OutNotifyeHRSSTokenDeactivatedXmlModel(eHSResultCode.R1001_NoTokenAssigned, udtInXml.HKID, udtInXml.Timestamp)
                Return udtOutXml

            End If

            If udtToken.Project <> TokenProjectType.EHR Then
                udtOutXml = New OutNotifyeHRSSTokenDeactivatedXmlModel(eHSResultCode.R1005_ExistingTokenNotIssuedBySenderParty, udtInXml.HKID, udtInXml.Timestamp)
                Return udtOutXml

            End If

            ' Insert database
            Dim udtDB As New Database
            Dim udtTokenBLL As New TokenBLL
            Dim udtRSAServerHandler As New RSAServerHandler

            Try
                udtDB.BeginTransaction()

                udtTokenBLL.DeleteTokenRecordByKey(udtToken, udtDB)

                udtTokenBLL.AddTokenDeactivateRecord(udtToken.UserID, udtToken.TokenSerialNo, Constant.EHRUPDATEBY, udtInXml.DeactivateReasonCode, _
                                                     udtToken.Project, udtToken.IsShareToken, udtDB)

                ' Reject all the pending Token Deactivate request in Account Change Confirmation (if any)
                Call (New eHSServiceDAL).ForceRejectTokenDeactivationRequest(udtToken.UserID, Constant.EHRUPDATEBY, udtDB)

                If udtRSAServerHandler.IsParallelRun Then
                    udtTokenBLL.UpdateRSASingletonTSMP(udtDB)
                End If

                Dim lblnRSAResult As Boolean = False

                If udtTokenBLL.IsEnableToken Then
                    lblnRSAResult = udtRSAServerHandler.deleteRSAUser(udtToken.UserID, DBFlagStr.DBFlagInterfaceLog)
                Else
                    lblnRSAResult = True
                End If

                If lblnRSAResult Then
                    Call (New ServiceProviderBLL).UpdateServiceProviderDataInput(udtSP.SPID, Constant.EHRUPDATEBY, Constant.EHRUPDATEBY, udtSP.TSMP, udtDB)

                    udtDB.CommitTransaction()

                Else
                    udtDB.RollBackTranscation()

                End If

            Catch ex As System.Exception
                udtDB.RollBackTranscation()
                Throw

            End Try

            udtOutXml = New OutNotifyeHRSSTokenDeactivatedXmlModel(eHSResultCode.R1000_Success, udtInXml.HKID, udtInXml.Timestamp)
            Return udtOutXml

        End Function

        '

        Public Function GeteHSSLoginAlias(udtInXml As InGeteHSSLoginAliasXmlModel, ByRef strStackTrace As String) As OutGeteHSSLoginAliasXmlModel
            ' --- Validate In Xml ---

            Dim udtOutXml As OutGeteHSSLoginAliasXmlModel

            ' HKID cannot be null
            If IsNothing(udtInXml.HKID) Then
                udtOutXml = New OutGeteHSSLoginAliasXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "HKID is empty"
                Return udtOutXml

            End If

            ' Timestamp cannot be null
            If IsNothing(udtInXml.Timestamp) Then
                udtOutXml = New OutGeteHSSLoginAliasXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Timestamp is empty"
                Return udtOutXml

            End If

            ' HKID must be in length 8 or 9 (after trimmed and excluding parentheses)
            If (New Regex("^.{8,9}$")).IsMatch(New Regex("[()]").Replace(udtInXml.HKID.Trim, String.Empty)) = False Then
                udtOutXml = New OutGeteHSSLoginAliasXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.HKID, udtInXml.Timestamp)
                strStackTrace = "Error in HKID"
                Return udtOutXml

            End If

            ' Format
            udtInXml.HKID = (New Formatter).formatHKIDInternal(udtInXml.HKID)
            udtInXml.Timestamp = udtInXml.Timestamp.Trim

            ' --- End of validation ---

            Dim udtDB As New Database

            Dim strTargetSPID As String = String.Empty

            IsCommonUser(udtInXml.HKID, strTargetSPID, Nothing, Nothing)

            If strTargetSPID = String.Empty Then
                udtOutXml = New OutGeteHSSLoginAliasXmlModel(eHSResultCode.R9002_UserNotFound, udtInXml.HKID, udtInXml.Timestamp)
                Return udtOutXml

            End If

            Dim dtHCSPUserAC As DataTable = (New UserACBLL).GetUserACForLogin(strTargetSPID, String.Empty, SPAcctType.ServiceProvider)

            If dtHCSPUserAC.Rows.Count <> 1 Then
                Throw New Exception(String.Format("eHSServiceBLL.GeteHSSLoginAlias: Unexpected return from GetUserACForLogin (SPID={0},RowCount={1})", strTargetSPID, dtHCSPUserAC.Rows.Count))
            End If

            Dim drHCSPUserAC As DataRow = dtHCSPUserAC.Rows(0)

            If IsDBNull(drHCSPUserAC("Alias_Account")) Then
                udtOutXml = New OutGeteHSSLoginAliasXmlModel(eHSResultCode.R2001_LoginAliasNotSet, udtInXml.HKID, udtInXml.Timestamp)
                Return udtOutXml

            End If

            udtOutXml = New OutGeteHSSLoginAliasXmlModel(eHSResultCode.R1000_Success, udtInXml.HKID, udtInXml.Timestamp)
            udtOutXml.LoginAlias = drHCSPUserAC("Alias_Account").ToString.Trim

            Return udtOutXml

        End Function

        Public Function HealthCheckeHSS(udtInXml As InHealthCheckeHSSXmlModel, ByRef strStackTrace As String) As OutHealthCheckeHSSXmlModel
            ' --- Validate In Xml ---

            Dim udtOutXml As OutHealthCheckeHSSXmlModel

            ' Timestamp cannot be null
            If IsNothing(udtInXml.Timestamp) Then
                udtOutXml = New OutHealthCheckeHSSXmlModel(eHSResultCode.R9001_InvalidParameter, udtInXml.Timestamp)
                strStackTrace = "Timestamp is empty"
                Return udtOutXml

            End If

            ' Format
            udtInXml.Timestamp = udtInXml.Timestamp.Trim

            ' --- End of validation ---

            udtOutXml = New OutHealthCheckeHSSXmlModel(eHSResultCode.R1000_Success, udtInXml.Timestamp)

            Return udtOutXml

        End Function

        '

        Private Function IsCommonUser(strHKID As String, ByRef strTargetSPIDOut As String, _
                                      ByRef udtTokenOut As TokenModel, ByRef udtSPOut As ServiceProviderModel) As Boolean
            Dim udtDB As New Database

            ' Find Service Provider
            Dim udtServiceProviderBLL As New ServiceProviderBLL
            Dim dt As DataTable = udtServiceProviderBLL.GetServiceProviderParticulasPermanentByHKID(strHKID, udtDB)
            strTargetSPIDOut = String.Empty

            If Not IsNothing(dt) Then
                For Each dr As DataRow In dt.Rows
                    If dr("Record_Status").ToString.Trim <> ServiceProviderStatus.Delisted Then
                        strTargetSPIDOut = dr("SP_ID").ToString.Trim
                        Exit For
                    End If

                Next

            End If

            If strTargetSPIDOut = String.Empty Then
                Return False

            End If

            ' Find Token
            udtTokenOut = (New TokenBLL).GetTokenProfileByUserID(strTargetSPIDOut, String.Empty, udtDB)

            ' Find SP
            udtSPOut = udtServiceProviderBLL.GetServiceProviderBySPID(New Database, strTargetSPIDOut)

            Return True

        End Function

        ' CRE18-XXX (Provide data to eHR Portal) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Function GeteHSDoctorList(udtInXml As InGeteHSSDoctorListXmlModel, _
                                         ByRef strStackTrace As String, _
                                         ByVal strConnectReplicationDB As String, _
                                         udtDB As Database) As OutGeteHSSDoctorListXmlModel

            Dim udtOutXml As OutGeteHSSDoctorListXmlModel

            ' ---------------------------------------
            ' Validation In Xml
            ' ---------------------------------------
            ' Timestamp cannot be null
            If udtInXml.Timestamp = String.Empty Then
                udtOutXml = New OutGeteHSSDoctorListXmlModel(eHSPatientPortalResultCode.R9001_InvalidParameter, udtInXml.Timestamp)
                strStackTrace = "Timestamp is empty. "
                Return udtOutXml

            End If

            ' Format
            udtInXml.Timestamp = udtInXml.Timestamp.Trim

            ' ---------------------------------------
            ' Generate Out Xml
            ' ---------------------------------------

            udtOutXml = New OutGeteHSSDoctorListXmlModel(eHSPatientPortalResultCode.R1000_Success, udtInXml.Timestamp)

            ' ---------------------------------------
            ' Convert the XML file to byte
            ' ---------------------------------------
            Dim udtGeneralFunction As New GeneralFunction

            ' 1. Find the target XML file
            Dim strXMLPath As String = udtGeneralFunction.getSystemParameter("EHRSS_PP_DoctorList_ExportPath")
            Dim strArchiveFormat As String = udtGeneralFunction.getSystemParameter("EHRSS_PP_DoctorList_ArchiveFormat")

            ' Validation: 
            ' a. Path is not found; or 
            ' b. Archive format is not found
            If strXMLPath = String.Empty Or strArchiveFormat = String.Empty Then
                udtOutXml = New OutGeteHSSDoctorListXmlModel(eHSPatientPortalResultCode.R9999_UnexpectedFailure, udtInXml.Timestamp)
                If strXMLPath = String.Empty Then
                    strStackTrace = strStackTrace + String.Format("The value of ""EHRSS_PP_DoctorList_ExportPath"" is not found in DB table[SystemParameters]. ")
                End If

                If strArchiveFormat = String.Empty Then
                    strStackTrace = strStackTrace + String.Format("The value of ""EHRSS_PP_DoctorList_ArchiveFormat"" is not found in DB table[SystemParameters]. ")
                End If

                Return udtOutXml

            End If

            ' 2. Convert byte to Base64 string

            'Dim byteXML() As Byte = System.IO.File.ReadAllBytes(String.Concat(strXMLPath, String.Format("{0}.{1}", strFileName, strArchiveFormat)))

            Dim byteXML() As Byte = Me.GetXMLFileContent(udtDB, strArchiveFormat, strConnectReplicationDB)

            If byteXML Is Nothing Then
                Throw New Exception(String.Format("File is not generated in the {0} format.", strArchiveFormat))
            End If

            Dim strBase64 As String = Convert.ToBase64String(byteXML)

            'Dim strHex As StringBuilder = New StringBuilder(byteXML.Length * 2)

            'For Each byteHex As Byte In byteXML
            '    strHex.AppendFormat("{0:x2}", byteHex)
            'Next

            ' 3. Assign the value in model 
            udtOutXml.Result = strBase64
            'udtOutXml.Result = strHex.ToString

            Return udtOutXml

        End Function
        ' CRE20-006 DHC NSP interface [Start][Nichole]
        ' --------------------------------------------------------------------------------------
        Public Function SeteHSSDHCNSP(udtInXml As InSeteHSSDHCNSPXmlModel, _
                                             ByRef strStackTrace As String, _
                                             ByVal enumUpdateDBWriteOff As EHSAccount.WriteOff, _
                                             udtDB As Database) As OutSeteHSSDHCNSPXmlModel

            Dim udtOutXml As OutSeteHSSDHCNSPXmlModel = Nothing
            Dim udtSchemeClaim As Scheme.SchemeClaimModel = Nothing
            Dim udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel = Nothing
            Dim udtSM As SystemMessage = Nothing
            Dim blnValid As Boolean = True
            Dim dt As DataTable = New DataTable
            Dim udtSPBLL As New ServiceProviderBLL
            Dim udtDistrictBoardBLL As New DistrictBoard.DistrictBoardBLL

            ' ---------------------------------------
            ' Validation In Xml
            ' ---------------------------------------
            Dim udtValidator As New Common.Validation.Validator

            'Check the compulsory fields  

            ' 1. Input values cannot be null
            If udtInXml.Timestamp = String.Empty Then
                strStackTrace = strStackTrace + "Timestamp is empty. "
                blnValid = False
            End If

            If udtInXml.UploadDistrictCode = String.Empty Then
                strStackTrace = strStackTrace + "Upload DistrictCode is empty. "
                blnValid = False
            End If


            If udtInXml.ProfList.ProfCount < 0 Then
                strStackTrace = strStackTrace + "Prof Count is empty. "
                blnValid = False
            End If


            '----------------------------------------------
            '-- Check the validation on districtcode + prof code + prof regno
            '----------------------------------------------

            ' Dim udtCodeMapList As CodeMappingCollection
            ' Dim udtCodeMap As CodeMappingModel
            'udtCodeMapList = CodeMappingBLL.GetAllCodeMapping

            For y As Integer = 0 To udtInXml.ProfList.Prof.Count - 1
                If udtInXml.ProfList.Prof(y).ProfDistrictCode = String.Empty Then
                    strStackTrace = strStackTrace + "Prof District Code is empty. "
                    blnValid = False
                End If

                'check the district code is valid
                'dt= udtSPBLL.GetServiceProviderDistrictbyDistrictCode(udtInXml.ProfList.Prof(y).ProfDistrictCode)
                'If dt Is Nothing Then
                '    strStackTrace = strStackTrace + String.Format("Prof District Code ({0}) is invalid. ", udtInXml.ProfList.Prof(y).ProfDistrictCode)
                '    blnValid = False
                'End If

                'Check the district code
                'udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.EHRSS, CodeMappingModel.EnumTargetSystem.EHS, CodeMappingModel.EnumCodeType.DHC_DistrictCode.ToString, udtInXml.ProfList.Prof(y).ProfDistrictCode)
                If udtDistrictBoardBLL.GetDistrictNameByDistrictCode(udtInXml.ProfList.Prof(y).ProfDistrictCode) Is Nothing Then
                    strStackTrace = strStackTrace + String.Format("Prof District Code ({0}) is invalid. ", udtInXml.ProfList.Prof(y).ProfDistrictCode)
                    blnValid = False
                End If

                'check the Professional code is empty or not
                If udtInXml.ProfList.Prof(y).ProfCode = String.Empty Then
                    strStackTrace = strStackTrace + "Prof Code is empty. "
                    blnValid = False
                End If

                'check the Registration Code is empty or not
                If udtInXml.ProfList.Prof(y).ProfRegNo = String.Empty Then
                    strStackTrace = strStackTrace + "Prof Reg No is empty. "
                    blnValid = False
                End If
            Next
            'check the districtcode + prof code + prof regno



            If Not blnValid Then
                udtOutXml = New OutSeteHSSDHCNSPXmlModel(eHSDHCNSPResultCode.R9001_InvalidParameter, udtInXml.Timestamp)
                Return udtOutXml
            End If

            '----------------------------------------------
            '-- Check the Professional code is valid or not
            '----------------------------------------------

            For y As Integer = 0 To udtInXml.ProfList.Prof.Count - 1
                'check the Prof code is valid
                dt = udtSPBLL.GetDHCProfessionbyProfCode(udtInXml.ProfList.Prof(y).ProfCode)
                If CInt(dt.Rows(0).Item("isProf")) < 1 Then
                    strStackTrace = strStackTrace + String.Format("Prof Code({0}) is invalid. ", dt.Rows(0).Item("isProf"))
                    blnValid = False
                End If
            Next


            If Not blnValid Then
                udtOutXml = New OutSeteHSSDHCNSPXmlModel(eHSDHCNSPResultCode.R1001_ProfessionalNotFound, udtInXml.Timestamp)
                Return udtOutXml
            End If

            ' Format
            udtInXml.Timestamp = udtInXml.Timestamp.Trim

            'Check the the NSP has practice enrolled HCVS voucher scheme in eHS(S) or not 
            Dim EnrolledArray(udtInXml.ProfList.Prof.Count - 1) As String

            For y As Integer = 0 To udtInXml.ProfList.Prof.Count - 1
                dt = udtSPBLL.GetServiceProviderSPIDbyProfRegNo(udtInXml.ProfList.Prof(y).ProfCode, udtInXml.ProfList.Prof(y).ProfRegNo, EnumDHCSearchType.SPID)
                If Not dt Is Nothing AndAlso dt.Rows.Count > 0 Then
                    EnrolledArray(y) = CStr(dt.Rows(0).Item("EnrolledInEHS")).Trim
                Else
                    EnrolledArray(y) = "N"
                End If
            Next



            'Check the delisted professional
            'For y As Integer = 0 To udtInXml.ProfList.Prof.Count - 1
            '    dtCLAIM = udtSPBLL.GetServiceProviderDelistedbyProfRegNo(udtInXml.ProfList.Prof(y).ProfCode, udtInXml.ProfList.Prof(y).ProfRegNo)
            '    If Not dtCLAIM Is Nothing Then
            '        strStackTrace = strStackTrace + "Professional has been delisted "
            '        blnValid = False
            '        Exit For
            '    End If
            'Next
            'If Not blnValid Then
            '    udtOutXml = New OutSeteHSSDHCNSPXmlModel(eHSDHCNSPResultCode.R1002_ProfessionalDelisted, udtInXml.Timestamp)
            '    Return udtOutXml
            'End If




            ' ---------------------------------------
            ' Insert the XML data into table 
            ' ---------------------------------------
            Dim strResult As String = String.Empty
            Dim blnRemoveValid As Boolean = True

            If udtInXml.ProfList.Prof.Count < 1 Then
                udtOutXml = New OutSeteHSSDHCNSPXmlModel(eHSDHCNSPResultCode.R9999_UnexpectedFailure, udtInXml.Timestamp)

                strStackTrace = strStackTrace + String.Format("The value of Profession list is not found. ")

                Return udtOutXml
            End If

            For y As Integer = 0 To udtInXml.ProfList.Prof.Count - 1
                'tCLAIM = udtSPBLL.GetServiceProviderDelistedbyProfRegNo(udtInXml.ProfList.Prof(y).ProfCode, udtInXml.ProfList.Prof(y).ProfRegNo)
                strResult = udtSPBLL.AddNSPToClaimAccess(blnRemoveValid, udtInXml.UploadDistrictCode, udtInXml.ProfList.Prof(y).ProfDistrictCode, udtInXml.ProfList.Prof(y).ProfCode, udtInXml.ProfList.Prof(y).ProfRegNo)
                If y < 1 Then
                    blnRemoveValid = False
                End If
            Next



            ' ---------------------------------------
            ' Generate Out Xml
            ' ---------------------------------------
            udtOutXml = New OutSeteHSSDHCNSPXmlModel(eHSDHCNSPResultCode.R1000_Success, udtInXml.Timestamp)

            'Assign the ProfCount into the OutXml
            udtOutXml.ProfList.ProfCount = udtInXml.ProfList.ProfCount

            Dim ProfArray(udtInXml.ProfList.Prof.Count - 1) As OutSeteHSSDHCNSPXmlModel.ProfClass



            For y As Integer = 0 To udtInXml.ProfList.Prof.Count - 1
                ProfArray(y) = New OutSeteHSSDHCNSPXmlModel.ProfClass
                ProfArray(y).ProfCode = udtInXml.ProfList.Prof(y).ProfCode
                ProfArray(y).ProfDistrictCode = udtInXml.ProfList.Prof(y).ProfDistrictCode
                ProfArray(y).ProfRegNo = udtInXml.ProfList.Prof(y).ProfRegNo
                'ProfArray(y).EnrolledInEHS = udtInXml.ProfList.Prof(y).EnrolledInEHS
                ProfArray(y).EnrolledInEHS = EnrolledArray(y)

            Next


            

            'assign the array of outseteHSSDHCNSPXmlModel.ProfClass to outXml
            udtOutXml.ProfList.Prof = ProfArray

            Return udtOutXml

        End Function


        Public Function GeteHSSDHCClaimAccess(udtInXml As InGeteHSSDHCClaimAccessXMLModel, _
                                            ByRef strStackTrace As String, _
                                            ByVal enumUpdateDBWriteOff As EHSAccount.WriteOff, _
                                            udtDB As Database) As OutgeteHSSDHCClaimAccessXmlModel

            Dim udtOutXml As OutgeteHSSDHCClaimAccessXmlModel = Nothing
            Dim udtSchemeClaim As Scheme.SchemeClaimModel = Nothing
            Dim udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel = Nothing
            Dim udtSM As SystemMessage = Nothing
            Dim blnValid As Boolean = True
            Dim strDocType As String = String.Empty
            Dim strHKIC As String = String.Empty
            Dim strDOB As String = String.Empty
            Dim dtmDOB As DateTime
            Dim rgx As Regex = Nothing
            Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime
            Dim dt As DataTable = New DataTable
            Dim udtSPBLL As New ServiceProviderBLL
            'Dim udtCodeMapList As CodeMappingCollection
            'Dim udtCodeMap As CodeMappingModel
            Dim udtDistrictBoardBLL As New DistrictBoard.DistrictBoardBLL
            Dim dv As DataView = Nothing

            'udtCodeMapList = CodeMappingBLL.GetAllCodeMapping

            ' ---------------------------------------
            ' Validation In Xml
            ' ---------------------------------------
            Dim udtValidator As New Common.Validation.Validator

            'Check the compulsory fields 

            '1. Input values cannot be null
            If udtInXml.Timestamp = String.Empty Then
                strStackTrace = strStackTrace + "Timestamp is empty. "
                blnValid = False
            End If

            If udtInXml.ServiceProvider.ProfCode = String.Empty Then
                strStackTrace = strStackTrace + "Prof Code is empty. "
                blnValid = False
            End If

            'Dim Profession = New List(Of String)({"RMP", "RCM", "RMT", "ROT", "RPT", "ROP"})

            'If Not Profession.Contains(udtInXml.ServiceProvider.ProfCode) Then
            '    strStackTrace = strStackTrace + "Prof Code is invalid. "
            '    blnValid = False
            'End If

            If udtInXml.ServiceProvider.ProfRegNo = String.Empty Then
                strStackTrace = strStackTrace + "Prof Reg No. is empty. "
                blnValid = False
            End If

            If udtInXml.Patient.HKID = String.Empty Then
                strStackTrace = strStackTrace + "HKID is empty. "
                blnValid = False
            End If

            If udtInXml.Patient.DocType = String.Empty Then
                strStackTrace = strStackTrace + "Doc Type is empty. "
                blnValid = False
            End If

            If udtInXml.Patient.DHCDistrictCode = String.Empty Then
                strStackTrace = strStackTrace + "District Code is empty. "
                blnValid = False
            End If

            If Not blnValid Then
                udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R9001_InvalidParameter, udtInXml.Timestamp)
                Return udtOutXml
            End If
            ' ---------------------------------------
            ' Validation In Prof Code
            ' ---------------------------------------
            dt = udtSPBLL.GetDHCProfessionbyProfCode(udtInXml.ServiceProvider.ProfCode)
            If CInt(dt.Rows(0).Item("isProf")) < 1 Then
                strStackTrace = strStackTrace + String.Format("Prof Code({0}) is invalid. ", dt.Rows(0).Item("isProf"))
                blnValid = False
            End If

            If Not blnValid Then
                udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R1001_ProfessionalNotFound, udtInXml.Timestamp)
                Return udtOutXml
            End If


            ' Format
            udtInXml.Timestamp = udtInXml.Timestamp.Trim

            ' ---------------------------------------
            ' Validation In DocumentType
            ' ---------------------------------------

            Select Case udtInXml.Patient.DocType.Trim
                Case DocType.DocTypeModel.DocTypeCode.HKIC, DocType.DocTypeModel.DocTypeCode.EC
                    strDocType = udtInXml.Patient.DocType.Trim
                Case Else
                    strStackTrace = strStackTrace + String.Format("DocType({0}) is invalid. ", udtInXml.Patient.DocType)
                    blnValid = False
            End Select

            ' ---------------------------------------
            ' Validation In HKID
            ' ---------------------------------------
            udtSM = udtValidator.chkIdentityNumber(udtInXml.Patient.DocType, udtInXml.Patient.HKID, Nothing)

            If Not udtSM Is Nothing OrElse udtInXml.Patient.HKID.Length > 9 Then
                strStackTrace = strStackTrace + String.Format("HKID({0}) is invalid. ", udtInXml.Patient.HKID)
                blnValid = False
            Else
                strHKIC = udtInXml.Patient.HKID
            End If

            ' ---------------------------------------
            ' Validation In DOB & DOBFormat
            ' ---------------------------------------
            If udtInXml.Patient.DOB <> String.Empty And udtInXml.Patient.DOBFormat <> String.Empty Then
                Select Case udtInXml.Patient.DOBFormat.Trim
                    Case "DD/MM/YYYY"
                        rgx = New Regex("^\d{2}\/\d{2}\/\d{4}$", RegexOptions.IgnoreCase)
                        If rgx.IsMatch(udtInXml.Patient.DOB) Then
                            strDOB = udtInXml.Patient.DOB

                        Else
                            strStackTrace = strStackTrace + String.Format("DOB({0}) is invalid under DOBFormat({1}). ", udtInXml.Patient.DOB, udtInXml.Patient.DOBFormat)
                            blnValid = False
                        End If

                    Case "MM/YYYY"
                        rgx = New Regex("^\d{2}\/\d{4}$", RegexOptions.IgnoreCase)
                        If rgx.IsMatch(udtInXml.Patient.DOB) Then
                            '01/MM/YYYY
                            strDOB = String.Format("01/{0}", udtInXml.Patient.DOB)

                        Else
                            strStackTrace = strStackTrace + String.Format("DOB({0}) is invalid under DOBFormat({1}). ", udtInXml.Patient.DOB, udtInXml.Patient.DOBFormat)
                            blnValid = False
                        End If

                    Case "YYYY"
                        rgx = New Regex("^\d{4}$", RegexOptions.IgnoreCase)
                        If rgx.IsMatch(udtInXml.Patient.DOB) Then
                            '01/01/YYYY
                            strDOB = String.Format("01/01/{0}", udtInXml.Patient.DOB)

                        Else
                            strStackTrace = strStackTrace + String.Format("DOB({0}) is invalid under DOBFormat({1}). ", udtInXml.Patient.DOB, udtInXml.Patient.DOBFormat)
                            blnValid = False
                        End If

                    Case Else
                        strStackTrace = strStackTrace + String.Format("DOBFormat({0}) is invalid. ", udtInXml.Patient.DOBFormat)
                        blnValid = False

                End Select

                'Convert DOB from String to Date 
                If blnValid Then
                    Dim strDate() As String = Split(strDOB, "/")

                    If CInt(strDate(2)) < DateValidation.YearMinValue Or strDate(2) > DateValidation.YearMaxValue Then
                        strStackTrace = strStackTrace + String.Format("Converted DOB({0}) is invalid. ", strDOB)
                        blnValid = False
                    End If

                    Try
                        If blnValid Then
                            dtmDOB = New Date(strDate(2), strDate(1), strDate(0))

                            If dtmDOB > dtmCurrentDate Then
                                strStackTrace = strStackTrace + String.Format("Converted DOB({0}) is future date. ", strDOB)
                                blnValid = False
                            End If
                        End If

                    Catch ex As Exception
                        strStackTrace = strStackTrace + String.Format("Converted DOB({0}) is invalid. ", strDOB)
                        blnValid = False
                    End Try
                End If

            End If

            If Not blnValid Then
                udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R9001_InvalidParameter, udtInXml.Timestamp)
                Return udtOutXml
            End If

            '---------------------------------------
            ' Check Professional
            '---------------------------------------
            
            Dim strSPID As String = String.Empty
            '---1003---'
            'dtClaim = udtSPBLL.GetServiceProviderNSPbyProfRegno(udtInXml.ServiceProvider.ProfCode, udtInXml.ServiceProvider.ProfRegNo)
            dt = udtSPBLL.GetServiceProviderSPIDbyProfRegNo(udtInXml.ServiceProvider.ProfCode, udtInXml.ServiceProvider.ProfRegNo, EnumDHCSearchType.NSP)
            If dt Is Nothing Then
                strStackTrace = strStackTrace + String.Format("Profession ({0},{1}) is not a enabled NSP in eHS(S). ", udtInXml.ServiceProvider.ProfCode, udtInXml.ServiceProvider.ProfRegNo)
                blnValid = False
            End If

            If Not blnValid Then
                udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R1003_ProfessionalDisabled, udtInXml.Timestamp)
                Return udtOutXml
            End If

            ' ---------------------------------------
            '  check District Code
            ' ---------------------------------------
            'udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.EHRSS, CodeMappingModel.EnumTargetSystem.EHS, CodeMappingModel.EnumCodeType.DHC_DistrictCode.ToString, udtInXml.Patient.DHCDistrictCode)
            If udtDistrictBoardBLL.GetDistrictNameByDistrictCode(udtInXml.Patient.DHCDistrictCode) Is Nothing Then
                strStackTrace = strStackTrace + String.Format("District Code ({0}) is invalid. ", udtInXml.Patient.DHCDistrictCode)
                blnValid = False
            End If

            If Not blnValid Then
                udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R9001_InvalidParameter, udtInXml.Timestamp)
                Return udtOutXml
            End If

            dv = New DataView(GetDHC_SP_Mapping())
            dv.RowFilter = "[Service_Category_Code] ='" + udtInXml.ServiceProvider.ProfCode + "' AND [Registration_Code] ='" + udtInXml.ServiceProvider.ProfRegNo + "' AND [District_Code]='" + udtInXml.Patient.DHCDistrictCode + "'"

            If dv.Count < 1 Then
                strStackTrace = strStackTrace + String.Format("District Code ({0}) is not same as Service Provider District Code ({1}). ", udtInXml.Patient.DHCDistrictCode, dv.Item("District_code"))
                blnValid = False
            End If

            If Not blnValid Then
                udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R9001_InvalidParameter, udtInXml.Timestamp)
                Return udtOutXml
            End If

            '---1000---'
            dt = udtSPBLL.GetServiceProviderSPIDbyProfRegNo(udtInXml.ServiceProvider.ProfCode, udtInXml.ServiceProvider.ProfRegNo, EnumDHCSearchType.VALIDSP)
            'If dt.Rows.Count > 0 Then
            If dt Is Nothing Then
                

                '---1001---'
                dt = udtSPBLL.GetServiceProviderSPIDbyProfRegNo(udtInXml.ServiceProvider.ProfCode, udtInXml.ServiceProvider.ProfRegNo, EnumDHCSearchType.PROF)
                If dt Is Nothing Then
                    strStackTrace = strStackTrace + String.Format("Profession registration no. ({0},{1}) not found in eHS(S). ", udtInXml.ServiceProvider.ProfCode, udtInXml.ServiceProvider.ProfRegNo)
                    blnValid = False
                    udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R1001_ProfessionalNotFound, udtInXml.Timestamp)
                    Return udtOutXml
                Else
                    If CStr(dt.Rows(0).Item("NotExisted")).Trim = "Y" Then
                        strStackTrace = strStackTrace + String.Format("Profession registration no. ({0},{1}) not found in eHS(S). ", udtInXml.ServiceProvider.ProfCode, udtInXml.ServiceProvider.ProfRegNo)
                        blnValid = False
                        udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R1001_ProfessionalNotFound, udtInXml.Timestamp)
                        Return udtOutXml
                    Else
                        '---1002---'
                        strSPID = CStr(dt.Rows(0).Item("NotExisted")).Trim
                        strStackTrace = strStackTrace + String.Format("Profession registration no. ({0},{1}) is suspended/delisted in eHS(S). ", udtInXml.ServiceProvider.ProfCode, udtInXml.ServiceProvider.ProfRegNo)
                        blnValid = False
                        udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R1002_ProfessionalDelisted, udtInXml.Timestamp)
                        Return udtOutXml
                    End If
                End If
            Else
                '--success--'
                '---------------------------------------
                ' Insert into table DHCClaimAccess
                '---------------------------------------
                Dim strResult As String = String.Empty
                Dim udtGeneralFunction As New GeneralFunction
                Dim strActivationCode As String = udtGeneralFunction.generateAccountActivationCode()
                Dim strAppLink As String = String.Empty
                udtGeneralFunction.getSystemParameter("AppLink", strAppLink, String.Empty)

                Dim strHCSPLink As String = String.Empty
                udtGeneralFunction.getSystemParameter("HCSPAppPath", strHCSPLink, String.Empty)

                'udtGeneralFunction.getSystemParameter("EHR_DHCLoginURLPath")
                Dim strEHSLoginURL As String = strAppLink + "/" + strHCSPLink + "/login.aspx?lang=en&artifact=" + strActivationCode

                strSPID = CStr(dt.Rows(0).Item("SP_ID")).Trim
                '-------
                '---https://apps.hcv.gov.hk/HCSP/login.aspx?artifact=


                If strEHSLoginURL = String.Empty Then
                    udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R9999_UnexpectedFailure, udtInXml.Timestamp)
                    If strEHSLoginURL = String.Empty Then
                        strStackTrace = strStackTrace + String.Format("The value of EHR_DHCLoginPath is not found in DB table[SystemParameters]. ")
                    End If

                    Return udtOutXml
                End If
 
                ' strDOB.Replace("/", "-")
                strResult = udtSPBLL.AddDHCClaimAccess(Hash(strActivationCode), strSPID, udtInXml.ServiceProvider.ProfCode, udtInXml.ServiceProvider.ProfRegNo, udtInXml.Patient.HKID, udtInXml.Patient.DocType, udtInXml.Patient.HKICSymbol, udtInXml.Patient.DOBFormat, udtInXml.Patient.DOB.Replace("/", "-"), udtInXml.VoucherClaim.ClaimAmount, udtInXml.Patient.DHCDistrictCode, strEHSLoginURL, strActivationCode)

                ' ---------------------------------------
                ' Generate Out Xml
                ' ---------------------------------------


                udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R1000_Success, udtInXml.Timestamp, strEHSLoginURL)


                Return udtOutXml
               
            End If
            'If Not blnValid Then
            '    udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R1001_ProfessionalNotFound, udtInXml.Timestamp)
            '    Return udtOutXml
            'Else
            '    udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R1002_ProfessionalNotFound, udtInXml.Timestamp)
            '    Return udtOutXml
            'End If

            '-----1002-----'
            'dt = udtSPBLL.GetServiceProviderSPIDbyProfRegNo(udtInXml.ServiceProvider.ProfCode, udtInXml.ServiceProvider.ProfRegNo, EnumDHCSearchType.DELISTED)
            'If Not dt Is Nothing Then
            '    strStackTrace = strStackTrace + String.Format("Profession ({0},{1}) is delisted. ", udtInXml.ServiceProvider.ProfCode, udtInXml.ServiceProvider.ProfRegNo)
            '    blnValid = False
            'End If

            'If Not blnValid Then
            '    udtOutXml = New OutgeteHSSDHCClaimAccessXmlModel(eHSDHCClaimAccessResultCode.R1002_ProfessionalDelisted, udtInXml.Timestamp)
            '    Return udtOutXml
            'End If


            Return udtOutXml
        End Function
        'CRE20-006 DHC NSP and Claim Access interface [End][Nichole]

        ' CRE18-XXX (Provide data to eHR Portal) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        ''' <summary>
        ''' Map EHS code to EHRSS code
        ''' </summary>
        ''' <param name="strSourceCode">EHS code</param>
        ''' <returns>Return EHRSS code</returns>
        ''' <remarks></remarks>
         
        ' CRE18-XXX (Provide data to eHR Portal) [End][Chris YIM]

        Public Function UpdateXMLFileContent(ByRef udtDB As Database, ByVal dtmSDIRLastUpdate As DateTime, ByVal arrByteContent As Byte(), ByVal strFileType As String) As Boolean
            Dim SDIRLastUpdateDtm_DataType As SqlDbType = SqlDbType.DateTime
            Dim SDIRLastUpdateDtm_DataSize As Integer = 8

            Dim FileContent_DataType As SqlDbType = SqlDbType.Image
            Dim FileContent_DataSize As Integer = 2147483647

            Dim FileType_DataType As SqlDbType = SqlDbType.VarChar
            Dim FileType_DataSize As Integer = 3

            Try
                Dim prams() As SqlParameter = {udtDB.MakeInParam("@SDIR_Last_Update_Dtm", SDIRLastUpdateDtm_DataType, SDIRLastUpdateDtm_DataSize, dtmSDIRLastUpdate), _
                                               udtDB.MakeInParam("@File_Content", FileContent_DataType, FileContent_DataSize, arrByteContent), _
                                               udtDB.MakeInParam("@File_Type", FileType_DataType, FileType_DataSize, strFileType)}

                udtDB.RunProc("proc_PatientPortalDoctorList_upd_FileContent", prams)

                Return True
            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
                Return False
            End Try
        End Function

        Public Function GetXMLFileContent(ByRef udtDB As Database, ByVal strFileType As String, ByVal strConnectReplicationDB As String) As Byte()
            Dim FileType_DataType As SqlDbType = SqlDbType.VarChar
            Dim FileType_DataSize As Integer = 3

            Dim arrByteFileContent As Byte() = Nothing

            Try
                Dim dtResult As New DataTable()

                Dim params() As SqlParameter = {udtDB.MakeInParam("@File_Type", FileType_DataType, FileType_DataSize, strFileType)}

                If strConnectReplicationDB = YesNo.Yes Then
                    udtDB.RunProc("proc_PatientPortalDoctorList_get_WithFileContent_Replication", params, dtResult)
                Else
                    udtDB.RunProc("proc_PatientPortalDoctorList_get_WithFileContent", params, dtResult)
                End If

                If dtResult.Rows.Count > 0 Then

                    Dim drRow As DataRow = dtResult.Rows(0)

                    If Not IsDBNull(drRow("File_Content")) Then
                        arrByteFileContent = CType(drRow("File_Content"), Byte())
                    End If

                End If

            Catch ex As Exception
                Throw
            End Try

            Return arrByteFileContent

        End Function


        Public Function GetDHC_SP_Mapping(Optional ByVal udtDB As Database = Nothing) As DataTable
            If udtDB Is Nothing Then udtDB = New Database()
            Dim dt As New DataTable()

            Try
                
                udtDB.RunProc("proc_DHCSPMapping_getAll", dt)
                Return dt

            Catch eSQL As SqlException
                Throw eSQL
            Catch ex As Exception
                Throw
            End Try

        End Function
    End Class

End Namespace
