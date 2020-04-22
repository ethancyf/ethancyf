Imports System.Data.SqlClient
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates
Imports System.Text.RegularExpressions
Imports Common.ComFunction
Imports Common.Component
Imports Common.Component.RSA_Manager
Imports Common.Component.ServiceProvider
Imports Common.Component.StaticData
Imports Common.Component.Token
Imports Common.Component.Token.TokenBLL
Imports Common.Component.UserAC
Imports Common.DataAccess
Imports Common.eHRIntegration.DAL
Imports Common.eHRIntegration.Model.Xml.eHSService
Imports Common.Format

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

    End Class

End Namespace
