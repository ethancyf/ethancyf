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


Namespace BLL

#Region "Constants"

    Public Class Constant
        Public Const EHRUPDATEBY As String = "eHRSS"
    End Class

#End Region

    Public Class eHSServiceBLL

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

            udtToken.UpdateBy = Constant.EHRUPDATEBY

            Dim udtDB As New Database
            Dim udtTokenBLL As New TokenBLL

            Try
                udtDB.BeginTransaction()

                udtTokenBLL.UpdateTokenIsShare(udtToken, udtDB)

                Call (New ServiceProviderBLL).UpdateServiceProviderDataInput(udtSP.SPID, Constant.EHRUPDATEBY, Constant.EHRUPDATEBY, udtSP.TSMP, udtDB)

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
                udtToken.LastReplacementBy = Constant.EHRUPDATEBY
                udtToken.ProjectReplacement = TokenProjectType.EHR
                udtToken.IsShareTokenReplacement = True
                udtToken.UpdateBy = Constant.EHRUPDATEBY

                udtTokenBLL.UpdateTokenReplacementNo(udtToken, udtDB)

                Call (New ServiceProviderBLL).UpdateServiceProviderDataInput(udtSP.SPID, Constant.EHRUPDATEBY, Constant.EHRUPDATEBY, udtSP.TSMP, udtDB)

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
                udtNewToken.IssueBy = Constant.EHRUPDATEBY

                udtNewToken.TokenSerialNoReplacement = String.Empty
                udtNewToken.ProjectReplacement = String.Empty
                udtNewToken.IsShareTokenReplacement = Nothing
                udtNewToken.LastReplacementDtm = Nothing
                udtNewToken.LastReplacementBy = String.Empty
                udtNewToken.LastReplacementReason = String.Empty

                udtNewToken.LastReplacementActivateDtm = Nothing

                udtNewToken.UpdateBy = Constant.EHRUPDATEBY

                udtTokenBLL.UpdateTokenReplacementNo(udtNewToken, udtDB)

                Call (New ServiceProviderBLL).UpdateServiceProviderDataInput(udtSP.SPID, Constant.EHRUPDATEBY, Constant.EHRUPDATEBY, udtSP.TSMP, udtDB)

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
        ' CRE18-XXX (Provide data to eHR Portal) [End][Chris YIM]

        ' CRE18-XXX (Provide data to eHR Portal) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        Public Function GeteHSVoucherBalance(udtInXml As InGeteHSSVoucherBalanceXmlModel, _
                                             ByRef strStackTrace As String, _
                                             ByVal enumUpdateDBWriteOff As EHSAccount.WriteOff, _
                                             udtDB As Database) As OutGeteHSSVoucherBalanceXmlModel

            Dim udtOutXml As OutGeteHSSVoucherBalanceXmlModel = Nothing
            Dim udtSchemeClaim As Scheme.SchemeClaimModel = Nothing
            Dim udtSubsidizeGroupClaim As Scheme.SubsidizeGroupClaimModel = Nothing
            Dim udtSM As SystemMessage = Nothing
            Dim blnValid As Boolean = True
            Dim rgx As Regex = Nothing

            Dim dtmCurrentDate As Date = (New GeneralFunction).GetSystemDateTime.Date

            Dim strHKIC As String = String.Empty
            Dim strDocType As String = String.Empty
            Dim strDOB As String = String.Empty
            Dim dtmDOB As DateTime
            'Dim strDOBFormat As String = String.Empty
            Dim intAge As Integer = Nothing
            Dim strDOR As String = String.Empty
            'Dim dtmDOR As DateTime

            ' ---------------------------------------
            ' Validation In Xml
            ' ---------------------------------------
            Dim udtValidator As New Common.Validation.Validator

            ' 1. Input values cannot be null
            If udtInXml.Timestamp = String.Empty Then
                strStackTrace = strStackTrace + "Timestamp is empty. "
                blnValid = False
            End If

            If udtInXml.HKID = String.Empty Then
                strStackTrace = strStackTrace + "HKID is empty. "
                blnValid = False
            End If

            If udtInXml.DocType = String.Empty Then
                strStackTrace = strStackTrace + "DocType is empty. "
                blnValid = False
            End If

            If udtInXml.DOB = String.Empty Then
                strStackTrace = strStackTrace + "DOB is empty. "
                blnValid = False
            End If

            If udtInXml.DOBFormat = String.Empty Then
                strStackTrace = strStackTrace + "DOBFormat is empty. "
                blnValid = False
            End If

            'If (udtInXml.DOB = String.Empty Or udtInXml.DOBFormat = String.Empty) And _
            '    (udtInXml.EC_Age = String.Empty Or udtInXml.EC_RegDate = String.Empty) Then
            '    strStackTrace = strStackTrace + "Either (DOB and DOBFormat) or (Age and DOR) is empty. "
            '    blnValid = False
            'End If

            If Not blnValid Then
                udtOutXml = New OutGeteHSSVoucherBalanceXmlModel(eHSPatientPortalResultCode.R9001_InvalidParameter, udtInXml.Timestamp)
                Return udtOutXml
            End If

            ' Format
            udtInXml.Timestamp = udtInXml.Timestamp.Trim

            ' 2. Input values cannot be invalid
            ' DocType
            Select Case udtInXml.DocType.Trim
                Case DocType.DocTypeModel.DocTypeCode.HKIC, DocType.DocTypeModel.DocTypeCode.EC
                    strDocType = udtInXml.DocType.Trim
                Case Else
                    strStackTrace = strStackTrace + String.Format("DocType({0}) is invalid. ", udtInXml.DocType)
                    blnValid = False
            End Select

            ' HKID
            udtSM = udtValidator.chkIdentityNumber(udtInXml.DocType, udtInXml.HKID, Nothing)

            If Not udtSM Is Nothing OrElse udtInXml.HKID.Length > 9 Then
                strStackTrace = strStackTrace + String.Format("HKID({0}) is invalid. ", udtInXml.HKID)
                blnValid = False
            Else
                strHKIC = udtInXml.HKID
            End If

            ' DOB & DOBFormat
            If udtInXml.DOB <> String.Empty And udtInXml.DOBFormat <> String.Empty Then
                Select Case udtInXml.DOBFormat.Trim
                    Case "DD/MM/YYYY"
                        rgx = New Regex("^\d{2}\/\d{2}\/\d{4}$", RegexOptions.IgnoreCase)
                        If rgx.IsMatch(udtInXml.DOB) Then
                            strDOB = udtInXml.DOB
                            'strDOBFormat = udtInXml.DOBFormat.Trim
                        Else
                            strStackTrace = strStackTrace + String.Format("DOB({0}) is invalid under DOBFormat({1}). ", udtInXml.DOB, udtInXml.DOBFormat)
                            blnValid = False
                        End If

                    Case "MM/YYYY"
                        rgx = New Regex("^\d{2}\/\d{4}$", RegexOptions.IgnoreCase)
                        If rgx.IsMatch(udtInXml.DOB) Then
                            '01/MM/YYYY
                            strDOB = String.Format("01/{0}", udtInXml.DOB)
                            'strDOBFormat = udtInXml.DOBFormat.Trim
                        Else
                            strStackTrace = strStackTrace + String.Format("DOB({0}) is invalid under DOBFormat({1}). ", udtInXml.DOB, udtInXml.DOBFormat)
                            blnValid = False
                        End If

                    Case "YYYY"
                        rgx = New Regex("^\d{4}$", RegexOptions.IgnoreCase)
                        If rgx.IsMatch(udtInXml.DOB) Then
                            '01/01/YYYY
                            strDOB = String.Format("01/01/{0}", udtInXml.DOB)
                            'strDOBFormat = udtInXml.DOBFormat.Trim
                        Else
                            strStackTrace = strStackTrace + String.Format("DOB({0}) is invalid under DOBFormat({1}). ", udtInXml.DOB, udtInXml.DOBFormat)
                            blnValid = False
                        End If

                    Case Else
                        strStackTrace = strStackTrace + String.Format("DOBFormat({0}) is invalid. ", udtInXml.DOBFormat)
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

            '' Age
            'If udtInXml.EC_Age <> String.Empty Then
            '    Try
            '        If IsNumeric(udtInXml.EC_Age) AndAlso udtInXml.EC_Age > 0 Then
            '            intAge = CInt(udtInXml.EC_Age)
            '        Else
            '            Throw New Exception(String.Format("Age({0}) is invalid. ", udtInXml.EC_Age))
            '        End If
            '    Catch ex As Exception
            '        strStackTrace = strStackTrace + ex.Message
            '        blnValid = False
            '    End Try
            'End If

            '' DOR
            'If udtInXml.EC_RegDate <> String.Empty Then
            '    ' DOR Format
            '    rgx = New Regex("^\d{2}\/\d{2}\/\d{4}$", RegexOptions.IgnoreCase)
            '    If rgx.IsMatch(udtInXml.EC_RegDate) Then
            '        strDOR = udtInXml.EC_RegDate
            '    Else
            '        strStackTrace = strStackTrace + String.Format("DOR({0}) is invalid. ", udtInXml.EC_RegDate)
            '        blnValid = False
            '    End If

            '    'Convert DOR from String to Date 
            '    If blnValid Then
            '        Dim strDORDate() As String = Split(strDOR, "/")

            '        If CInt(strDORDate(2)) < DateValidation.YearMinValue Or strDORDate(2) > DateValidation.YearMaxValue Then
            '            strStackTrace = strStackTrace + String.Format("Converted DOR({0}) is invalid. ", strDOR)
            '            blnValid = False
            '        End If

            '        Try
            '            If blnValid Then
            '                dtmDOR = New Date(strDORDate(2), strDORDate(1), strDORDate(0))

            '                If dtmDOR > dtmCurrentDate Then
            '                    strStackTrace = strStackTrace + String.Format("Converted DOR({0}) is future date. ", strDOR)
            '                    blnValid = False
            '                End If
            '            End If
            '        Catch ex As Exception
            '            strStackTrace = strStackTrace + String.Format("Converted DOR({0}) is invalid. ", strDOR)
            '            blnValid = False
            '        End Try
            '    End If
            'End If

            If Not blnValid Then
                udtOutXml = New OutGeteHSSVoucherBalanceXmlModel(eHSPatientPortalResultCode.R9001_InvalidParameter, udtInXml.Timestamp)
                Return udtOutXml
            End If

            ' ---------------------------------------
            ' Generate Out Xml
            ' ---------------------------------------
            udtOutXml = New OutGeteHSSVoucherBalanceXmlModel(eHSPatientPortalResultCode.R1000_Success, udtInXml.Timestamp)

            ' ---------------------------------------
            ' Search validated account
            ' ---------------------------------------
            Dim udtEHSAccountBLL As EHSAccount.EHSAccountBLL = New EHSAccount.EHSAccountBLL
            Dim udtEHSTransactionBLL As New EHSTransaction.EHSTransactionBLL()
            Dim udtFormatter As New Formatter
            Dim udtSchemeClaimBLL As Scheme.SchemeClaimBLL = New Scheme.SchemeClaimBLL()

            Dim udtEHSAccount As EHSAccount.EHSAccountModel = Nothing
            Dim udtEHSPersonalInformation As EHSAccount.EHSAccountModel.EHSPersonalInformationModel = Nothing
            Dim strSearchDocCode As String = String.Empty

            Try
                'Format Doc No.
                strHKIC = udtFormatter.formatDocumentIdentityNumber(strDocType, strHKIC)

                'Search EHS account
                udtEHSAccount = udtEHSAccountBLL.LoadEHSAccountByIdentity(strHKIC, strDocType, udtDB)
                If Not udtEHSAccount Is Nothing Then
                    strSearchDocCode = strDocType
                End If

                '---------------------------------------
                ' Check Validated Account
                '---------------------------------------
                If udtEHSAccount Is Nothing OrElse (udtEHSAccount.AccountSource <> EHSAccount.EHSAccountModel.SysAccountSource.ValidateAccount) Then
                    strStackTrace = strStackTrace + String.Format("No EHSAccount found. ")
                    blnValid = False
                End If

                '---------------------------------------
                ' Check Deceased Status
                '---------------------------------------
                If blnValid Then
                    If udtEHSAccount.Deceased Then
                        ' For deceased account, same behavior as no account found
                        strStackTrace = strStackTrace + String.Format("The EHSAccount marked with deceased status. ")
                        blnValid = False
                    End If
                End If

                '---------------------------------------
                ' Check Account Status
                '---------------------------------------
                If blnValid Then
                    If udtEHSAccount.RecordStatus = EHSAccount.EHSAccountModel.ValidatedAccountRecordStatusClass.Terminated Then
                        strStackTrace = strStackTrace + String.Format("The EHSAccount is terminated. ")
                        blnValid = False
                    End If
                End If


                '---------------------------------------
                ' Check Enquiry Status
                '---------------------------------------
                If blnValid Then
                    If udtEHSAccount.PublicEnquiryStatus <> EHSAccount.EHSAccountModel.EnquiryStatusClass.Available Then
                        strStackTrace = strStackTrace + String.Format("The enquiry status of EHSAccount is not available. ")
                        blnValid = False
                    End If
                End If

                If blnValid Then
                    udtEHSPersonalInformation = udtEHSAccount.EHSPersonalInformationList.Filter(strSearchDocCode)
                    udtSchemeClaim = udtSchemeClaimBLL.getEffectiveSchemeClaimWithSubsidize(Scheme.SchemeClaimModel.HCVS)
                    udtSubsidizeGroupClaim = udtSchemeClaim.SubsidizeGroupClaimList.Filter((New GeneralFunction).GetSystemDateTime())(0)

                    ' Get available voucher
                    If udtEHSAccount.VoucherInfo Is Nothing Then
                        Dim udtVoucherInfo As New VoucherInfoModel(VoucherInfoModel.AvailableVoucher.Include, _
                                                                   VoucherInfoModel.AvailableQuota.Include)

                        ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [Start][Chris YIM]
                        ' ---------------------------------------------------------------------------------------------------------
                        udtVoucherInfo.UpdateDBWriteOff = enumUpdateDBWriteOff

                        udtVoucherInfo.GetInfo(dtmCurrentDate, udtSchemeClaim, udtEHSPersonalInformation, String.Empty, udtDB)
                        ' CRE20-005 (Providing users' data in HCVS to eHR Patient Portal) [End][Chris YIM]	

                        udtEHSAccount.VoucherInfo = udtVoucherInfo
                    End If
                End If

            Catch ex As Exception
                strStackTrace = ex.Message
                udtOutXml = New OutGeteHSSVoucherBalanceXmlModel(eHSPatientPortalResultCode.R9999_UnexpectedFailure, udtInXml.Timestamp)
                Return udtOutXml
            End Try

            If Not blnValid Then
                udtOutXml = New OutGeteHSSVoucherBalanceXmlModel(eHSPatientPortalResultCode.R1001_PatientNotFound, udtInXml.Timestamp)
                Return udtOutXml
            End If

            ' ---------------------------------------
            ' DOB
            ' ---------------------------------------
            Try
                Select Case udtEHSPersonalInformation.ExactDOB
                    Case "D", "T"
                        If udtEHSPersonalInformation.DOB <> dtmDOB Or udtInXml.DOBFormat.Trim <> "DD/MM/YYYY" Then
                            blnValid = False
                        End If

                    Case "M", "U"
                        If udtEHSPersonalInformation.DOB <> dtmDOB Or udtInXml.DOBFormat.Trim <> "MM/YYYY" Then
                            blnValid = False
                        End If

                    Case "Y", "V", "R"
                        If udtEHSPersonalInformation.DOB <> dtmDOB Or udtInXml.DOBFormat.Trim <> "YYYY" Then
                            blnValid = False
                        End If

                    Case "A"
                        If Not udtEHSPersonalInformation.ECAge.HasValue Or Not udtEHSPersonalInformation.ECDateOfRegistration.HasValue Then
                            blnValid = False
                        Else
                            Dim dtmLogicalDOB As Date = DateAdd(DateInterval.Year, CInt(udtEHSPersonalInformation.ECAge.Value) * -1, CDate(udtEHSPersonalInformation.ECDateOfRegistration))

                            If dtmLogicalDOB.Year <> dtmDOB.Year Or _
                                (udtInXml.DOBFormat.Trim <> "DD/MM/YYYY" And udtInXml.DOBFormat.Trim <> "MM/YYYY" And udtInXml.DOBFormat.Trim <> "YYYY") Then

                                blnValid = False
                            End If
                        End If

                        'Case "A"
                        '    blnValid = False

                        'If udtEHSPersonalInformation.ECAge.HasValue And udtEHSPersonalInformation.ECDateOfRegistration.HasValue Then
                        '    If udtEHSPersonalInformation.ECAge.Value <> intAge Or udtEHSPersonalInformation.ECDateOfRegistration <> dtmDOR Then
                        '        blnValid = False
                        '    End If
                        'Else
                        '    blnValid = False
                        'End If

                    Case Else
                        Throw New Exception(String.Format("Invalid Exact DOB Format({0}).", udtEHSPersonalInformation.ExactDOB))
                End Select

            Catch ex As Exception
                strStackTrace = ex.Message
                udtOutXml = New OutGeteHSSVoucherBalanceXmlModel(eHSPatientPortalResultCode.R9999_UnexpectedFailure, udtInXml.Timestamp)
                Return udtOutXml
            End Try

            If Not blnValid Then
                udtOutXml = New OutGeteHSSVoucherBalanceXmlModel(eHSPatientPortalResultCode.R1001_PatientNotFound, udtInXml.Timestamp)
                Return udtOutXml
            End If

            '---------------------------------------
            ' Check eligiblility for HCVS
            '---------------------------------------
            Dim udtClaimRuleBLL As New Common.Component.ClaimRules.ClaimRulesBLL
            Dim udtResult As Common.Component.ClaimRules.ClaimRulesBLL.EligibleResult

            Try
                If blnValid Then
                    udtResult = udtClaimRuleBLL.CheckEligibilityAny(udtSchemeClaim, udtEHSPersonalInformation, dtmCurrentDate)

                    If Not udtResult.IsEligible Then
                        strStackTrace = strStackTrace + String.Format("The EHSAccount is not eligible. ")
                        blnValid = False
                    End If

                End If

            Catch ex As Exception
                strStackTrace = ex.Message
                udtOutXml = New OutGeteHSSVoucherBalanceXmlModel(eHSPatientPortalResultCode.R9999_UnexpectedFailure, udtInXml.Timestamp)
                Return udtOutXml
            End Try

            If Not blnValid Then
                udtOutXml = New OutGeteHSSVoucherBalanceXmlModel(eHSPatientPortalResultCode.R1002_PatientNotEligible, udtInXml.Timestamp)
                Return udtOutXml
            End If

            ' ---------------------------------------
            ' Quota
            ' ---------------------------------------
            Dim arrQuota As New ArrayList

            Try
                If udtEHSAccount.VoucherInfo.VoucherQuotalist.FilterByEffectiveDtm(dtmCurrentDate).Count > 0 Then

                    For Each udtVoucherQuota As VoucherQuotaModel In udtEHSAccount.VoucherInfo.VoucherQuotalist.FilterByEffectiveDtm(dtmCurrentDate)
                        Dim intMaxUsableBalance As Integer = udtEHSAccount.VoucherInfo.GetMaxUsableBalance(udtVoucherQuota.ProfCode)

                        Dim udtQuota As OutGeteHSSVoucherBalanceXmlModel.Quota = New OutGeteHSSVoucherBalanceXmlModel.Quota()
                        udtQuota.QuotaProfCode = GetProfessionMappingForEHRSS(udtVoucherQuota.ProfCode).CodeTarget.Trim
                        udtQuota.QuotaRemain = udtVoucherQuota.AvailableQuota
                        udtQuota.QuotaMaxUsableRemain = IIf(intMaxUsableBalance > 0, intMaxUsableBalance, 0)
                        udtQuota.QuotaRemainEndDate = String.Format("{0:dd/MM/yyyy}", udtVoucherQuota.PeriodEndDtm)

                        arrQuota.Add(udtQuota)

                    Next

                End If

            Catch ex As Exception
                strStackTrace = ex.Message
                udtOutXml = New OutGeteHSSVoucherBalanceXmlModel(eHSPatientPortalResultCode.R9999_UnexpectedFailure, udtInXml.Timestamp)
                Return udtOutXml
            End Try

            ' ---------------------------------------
            ' Forfeit
            ' ---------------------------------------
            Dim udtForfeit As OutGeteHSSVoucherBalanceXmlModel.ForfeitInfo = New OutGeteHSSVoucherBalanceXmlModel.ForfeitInfo()

            Try
                udtForfeit.NextDepositAmount = udtEHSAccount.VoucherInfo.GetNextDepositAmount
                udtForfeit.NextCappingAmount = udtEHSAccount.VoucherInfo.GetNextCappingAmount
                udtForfeit.NextForfeitDate = udtEHSAccount.VoucherInfo.GetNextForfeitDate
                udtForfeit.NextForfeitAmount = udtEHSAccount.VoucherInfo.GetNextForfeitAmount

            Catch ex As Exception
                strStackTrace = ex.Message
                udtOutXml = New OutGeteHSSVoucherBalanceXmlModel(eHSPatientPortalResultCode.R9999_UnexpectedFailure, udtInXml.Timestamp)
                Return udtOutXml
            End Try

            ' ---------------------------------------
            ' Transaction History
            ' ---------------------------------------
            Dim arrTx As New ArrayList

            Try
                Dim dt As DataTable = udtEHSTransactionBLL.getPatientPortalVoucherTransactionHistory(strDocType, strHKIC, _
                                                                                                     String.Empty, _
                                                                                                     udtSubsidizeGroupClaim.SubsidizeCode, _
                                                                                                     EHealthAccountType.Validated)

                If dt.Rows.Count > 0 Then
                    For Each dr As DataRow In dt.Rows
                        Dim udtTx As OutGeteHSSVoucherBalanceXmlModel.Transaction = New OutGeteHSSVoucherBalanceXmlModel.Transaction
                        udtTx.ServiceDate = String.Format("{0:dd/MM/yyyy}", CDate(dr("Service_Receive_Dtm")))
                        udtTx.UsedVoucherAmt = CInt(dr("Total_Amount")).ToString
                        udtTx.ProfCode = GetProfessionMappingForEHRSS(CStr(dr("Service_Type")).Trim).CodeTarget.Trim

                        If Not IsDBNull(dr("SP_Name")) Then
                            udtTx.SPName_en = CStr(dr("SP_Name")).Trim
                        End If
                        If Not IsDBNull(dr("SP_Name_Chi")) Then
                            udtTx.SPName_tc = CStr(dr("SP_Name_Chi")).Trim
                        End If

                        If Not IsDBNull(dr("Practice_Name")) Then
                            udtTx.PracticeName_en = CStr(dr("Practice_Name")).Trim
                        End If
                        If Not IsDBNull(dr("Practice_Name_Chi")) Then
                            udtTx.PracticeName_tc = CStr(dr("Practice_Name_Chi")).Trim
                        End If

                        If Not IsDBNull(dr("Practice_Address")) Then
                            udtTx.PracticeAddr_en = CStr(dr("Practice_Address")).Trim
                        End If
                        If Not IsDBNull(dr("Practice_Address_Chi")) Then
                            udtTx.PracticeAddr_tc = CStr(dr("Practice_Address_Chi")).Trim
                        End If

                        If Not IsDBNull(dr("Phone_Daytime")) Then
                            udtTx.PracticeTelNo = CStr(dr("Phone_Daytime")).Trim
                        End If

                        arrTx.Add(udtTx)

                    Next
                End If

            Catch ex As Exception
                strStackTrace = ex.Message
                udtOutXml = New OutGeteHSSVoucherBalanceXmlModel(eHSPatientPortalResultCode.R9999_UnexpectedFailure, udtInXml.Timestamp)
                Return udtOutXml
            End Try

            ' ---------------------------------------
            ' Assign the value in model 
            ' ---------------------------------------
            ' Special Handle for negative number of available voucher
            If udtEHSAccount.VoucherInfo IsNot Nothing AndAlso udtEHSAccount.VoucherInfo.GetAvailableVoucher() < 0 Then
                udtOutXml.VoucherBalanceAmt = 0
            Else
                udtOutXml.VoucherBalanceAmt = udtEHSAccount.VoucherInfo.GetAvailableVoucher()
            End If

            udtOutXml.MaxAccumulativeAmt = udtSubsidizeGroupClaim.NumSubsidizeCeiling

            If arrQuota.Count > 0 Then
                Dim QuotaList(arrQuota.Count - 1) As OutGeteHSSVoucherBalanceXmlModel.Quota

                For i As Integer = 0 To arrQuota.Count - 1
                    QuotaList(i) = (arrQuota(i))
                Next

                udtOutXml.QuotaList = QuotaList
            Else
                udtOutXml.QuotaList = Nothing
            End If

            udtOutXml.Forfeit = udtForfeit

            If arrTx.Count > 0 Then
                Dim TransactionHistory(arrTx.Count - 1) As OutGeteHSSVoucherBalanceXmlModel.Transaction

                For i As Integer = 0 To arrTx.Count - 1
                    TransactionHistory(i) = (arrTx(i))
                Next

                udtOutXml.TransactionHistory = TransactionHistory
            Else
                udtOutXml.TransactionHistory = Nothing
            End If

            Return udtOutXml

        End Function
        ' CRE18-XXX (Provide data to eHR Portal) [End][Chris YIM]

        ' CRE18-XXX (Provide data to eHR Portal) [Start][Chris YIM]
        ' --------------------------------------------------------------------------------------
        ''' <summary>
        ''' Map EHS code to EHRSS code
        ''' </summary>
        ''' <param name="strSourceCode">EHS code</param>
        ''' <returns>Return EHRSS code</returns>
        ''' <remarks></remarks>
        Public Shared Function GetProfessionMappingForEHRSS(ByVal strSourceCode As String) As CodeMappingModel
            Dim udtCodeMapList As CodeMappingCollection = Nothing
            Dim udtCodeMap As CodeMappingModel = Nothing

            udtCodeMapList = CodeMappingBLL.GetAllCodeMapping
            udtCodeMap = udtCodeMapList.GetMappingByCode(CodeMappingModel.EnumSourceSystem.EHS, _
                                                         CodeMappingModel.EnumTargetSystem.EHRSS, _
                                                         "EHRPatientPortal_ProfessionalCode", _
                                                         strSourceCode)

            If udtCodeMap Is Nothing Then
                Throw New Exception(String.Format("No available mapping in DB [CodeMapping] of professional code({0}) converting from EHS to EHRSS.", strSourceCode))
            End If

            Return udtCodeMap

        End Function
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

    End Class

End Namespace
