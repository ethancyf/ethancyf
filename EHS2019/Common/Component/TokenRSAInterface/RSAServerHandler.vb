Imports System
Imports System.Collections.Generic
Imports System.Configuration
Imports System.Data
Imports System.Net
Imports System.Net.Security
Imports System.Runtime.InteropServices
Imports System.Security.Cryptography.X509Certificates

Imports Common.ComFunction
Imports Common.ComObject

Imports com.rsa.admin
Imports com.rsa.admin.data
Imports com.rsa.authmgr.admin.agentmgt
Imports com.rsa.authmgr.admin.agentmgt.data
Imports com.rsa.authmgr.admin.hostmgt.data
Imports com.rsa.authmgr.admin.principalmgt
Imports com.rsa.authmgr.admin.principalmgt.data
Imports com.rsa.authmgr.admin.tokenmgt
Imports com.rsa.authmgr.admin.tokenmgt.data
Imports com.rsa.authmgr.common
Imports com.rsa.authn
Imports com.rsa.authn.data
Imports com.rsa.command
Imports com.rsa.command.exception
Imports com.rsa.common
Imports com.rsa.common.search
Imports System.IO
Imports com.rsa.ucm.am

' CRE13-029 - RSA Server Upgrade [Start][Lawrence]
Namespace Component.RSA_Manager

    Public Class RSAServerHandler

#Region "Fields and Properties"

        'Public Const RSA_Base_URL As String = "http://160.19.24.84:8080"
        'Public Const RSA_Base_URL As String = "https://rsaauthen2"
        'Public Const RSA_Admin_Api_URL As String = "/client.cgi"

        Private gstrRSAAPIVersionMain As String = String.Empty
        Private gstrRSAAPIVersionSub As String = String.Empty
        Private glstRSAAPIRepeatCategory As List(Of String) = Nothing


        Private gudtRSAIdentitySource As IdentitySourceDTO = Nothing

        Private GeneralFunction As GeneralFunction = New GeneralFunction

        Private SM As SystemMessage

#End Region

#Region "Constants"

        Const FUNCTION_CODE_RSA_Main As String = "990101"
        Const FUNCTION_CODE_RSA_Sub As String = "990102"

        Public Class RSAAgentMethod
            Public Const WebService As String = "WS"
            Public Const DirectCall As String = "C"
        End Class


#End Region

#Region "Constructors"

        Public Sub New()

            GeneralFunction.getSystemParameter("RSAAPIVersion", gstrRSAAPIVersionMain, gstrRSAAPIVersionSub)

            Dim lstrRSAAPIRepeatCategory As String = String.Empty
            GeneralFunction.getSystemParameter("RSAAPIRepeatCategory", lstrRSAAPIRepeatCategory, String.Empty)

            glstRSAAPIRepeatCategory = New List(Of String)
            For Each lstr As String In lstrRSAAPIRepeatCategory.Split(New String() {","}, StringSplitOptions.RemoveEmptyEntries)
                glstRSAAPIRepeatCategory.Add(lstr.Trim)
            Next

            ' CRE15-001 RSA Server Upgrade [Start][Winnie]
            'GeneralFunction.getSystemParameter("RSAAgentConfPath", gstrRSAAgentConfPath, String.Empty)
            ' CRE15-001 RSA Server Upgrade [End][Winnie]

        End Sub

        Private Sub InitRSA(ByVal strRSAAPIVersion As String)
            ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

            Dim lstrRSALink As String = String.Empty
            Dim lstrRSAWebLogicUsername As String = String.Empty
            Dim lstrRSAWebLogicPassword As String = String.Empty
            Dim lstrRSAAMUsername As String = String.Empty
            Dim lstrRSAAMPassword As String = String.Empty

            If strRSAAPIVersion = String.Empty Then
                Throw New Exception("RSAServerHandler.InitRSA: RSA API Version is empty")
            End If

            GeneralFunction.getSystemParameter(String.Format("RSA{0}Link", strRSAAPIVersion), lstrRSALink, String.Empty)
            GeneralFunction.getSystemParameter(String.Format("RSA{0}WebLogicUsername", strRSAAPIVersion), lstrRSAWebLogicUsername, String.Empty)
            GeneralFunction.getSystemParameterPassword(String.Format("RSA{0}WebLogicPassword", strRSAAPIVersion), lstrRSAWebLogicPassword)
            GeneralFunction.getSystemParameter(String.Format("RSA{0}AMUsername", strRSAAPIVersion), lstrRSAAMUsername, String.Empty)
            GeneralFunction.getSystemParameterPassword(String.Format("RSA{0}AMPassword", strRSAAPIVersion), lstrRSAAMPassword)

            Dim conn As New SOAPCommandTarget(lstrRSALink, lstrRSAWebLogicUsername, lstrRSAWebLogicPassword)

            If conn.Login(lstrRSAAMUsername, lstrRSAAMPassword) = False Then
                Throw New Exception(String.Format("RSAServerHandler.InitRSA: Unable to connect to RSA Server (Link={0}, WebLogicUsername={1}, WebLogicPassword={2}, AMUsername={3}, AMPassword={4})", _
                                    lstrRSALink, lstrRSAWebLogicUsername, lstrRSAWebLogicPassword, lstrRSAAMUsername, lstrRSAAMPassword))
            End If

            CommandTargetPolicy.setDefaultCommandTarget(conn)

            Dim c1 As New GetIdentitySourcesCommand
            c1.execute()

            If c1.identitySources.Length <> 1 Then
                Throw New Exception(String.Format("RSAServerHandler.InitRSA: Unexpected value (identitySources.Length={0})", c1.identitySources.Length))
            End If

            gudtRSAIdentitySource = c1.identitySources(0)

        End Sub

        Private Function ValidateServerCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
            Return True
        End Function

#End Region

#Region "Authenticate (Category A1/A2)"

        ''' <summary>
        ''' !! Deployment Notice !!
        ''' You must place the following dll (RSAAuthAgent.dll) together with the acecInt.dll and sdmsg.dll into the specific project bin folder.
        ''' That is, the HCVU\bin and HCSP\bin.
        ''' Alternatively, you may change the following path to an absolute path.
        ''' </summary>
        <DllImport("RSAAuthAgent.dll", CallingConvention:=CallingConvention.Cdecl)> _
        Public Shared Function auth(ByVal strConfig As String, ByVal strUserID As String, ByVal strPasscode As String, ByRef strStackTrace As StringBuilder) As Integer
        End Function

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        'Public Function authRSAUser(ByVal pstrLoginID As String, ByVal pstrPasscode As String, ByVal pblnHaveReplacement As Boolean, Optional ByVal pstrPlatform As String = "", Optional ByVal pstrUniqueID As String = "") As Boolean
        Public Function authRSAUser(ByVal pstrLoginID As String, ByVal pstrPasscode As String, ByVal pblnHaveReplacement As Boolean, Optional ByVal pstrPlatform As String = "", Optional ByVal pstrUniqueID As String = "") As Integer

            Dim lintResult As Integer = 1

            ' Main RSA server
            lintResult = HandleAuthRSAUser(pstrLoginID, pstrPasscode, pstrPlatform, pstrUniqueID, True)

            ' Sub RSA server
            If gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains(IIf(pblnHaveReplacement, "A2", "A1")) Then
                HandleAuthRSAUser(pstrLoginID, pstrPasscode, pstrPlatform, pstrUniqueID, False)
            End If

            Return lintResult

        End Function
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        ' Return Result Code 
        'Private Function HandleAuthRSAUser(ByVal pstrLoginID As String, ByVal pstrPasscode As String, ByVal pstrPlatform As String, ByVal pstrUniqueID As String, ByVal pblnMain As Boolean) As Boolean
        Private Function HandleAuthRSAUser(ByVal pstrLoginID As String, ByVal pstrPasscode As String, ByVal pstrPlatform As String, ByVal pstrUniqueID As String, ByVal pblnMain As Boolean) As Integer
            ' CRE16-004 (Enable SP to unlock account) [End][Winnie]
            ' Convert field
            pstrLoginID = pstrLoginID.Trim
            pstrPasscode = pstrPasscode.Trim

            Dim lstrRSAAgentConfPath As String = String.Empty
            Dim lstrRSAAgentMethod As String = String.Empty
            Dim udtAuditLogEntry As Object = Nothing
            Dim lstrRSAAPIVersion = String.Empty
            Dim lstrRSAAgentRetryCount As String = String.Empty
            Dim lstrRSAAgentRetryDelay As String = String.Empty
            Dim lintRSAAgentRetryCount As Integer = 0
            Dim lintRSAAgentRetryDelay As Integer = 0

            If pblnMain Then
                lstrRSAAPIVersion = gstrRSAAPIVersionMain

                If pstrPlatform = String.Empty Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
                ElseIf pstrPlatform = "IVRS" Then
                    udtAuditLogEntry = New AuditLogIVRSEntry(FUNCTION_CODE_RSA_Main, IVRS_Entry.IVRS_HCSP, pstrUniqueID)
                End If
            Else
                lstrRSAAPIVersion = gstrRSAAPIVersionSub

                If pstrPlatform = String.Empty Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
                ElseIf pstrPlatform = "IVRS" Then
                    udtAuditLogEntry = New AuditLogIVRSEntry(FUNCTION_CODE_RSA_Sub, IVRS_Entry.IVRS_HCSP, pstrUniqueID)
                End If
            End If

            GeneralFunction.getSystemParameter(String.Format("RSA{0}AgentConfPath", lstrRSAAPIVersion), lstrRSAAgentConfPath, String.Empty)
            GeneralFunction.getSystemParameter(String.Format("RSA{0}AgentMethod", lstrRSAAPIVersion), lstrRSAAgentMethod, String.Empty)

            ' CRE15-001 RSA Server Upgrade - add retry mechanism [Start][Winnie]
            GeneralFunction.getSystemParameter(String.Format("RSA{0}AgentRetryCount", lstrRSAAPIVersion), lstrRSAAgentRetryCount, String.Empty)
            GeneralFunction.getSystemParameter(String.Format("RSA{0}AgentRetryDelay", lstrRSAAPIVersion), lstrRSAAgentRetryDelay, String.Empty)

            If lstrRSAAgentRetryCount <> String.Empty Then
                lintRSAAgentRetryCount = CInt(lstrRSAAgentRetryCount)
            End If

            If lstrRSAAgentRetryDelay <> String.Empty Then
                lintRSAAgentRetryDelay = CInt(lstrRSAAgentRetryDelay)
            End If

            For intCurrentRetry As Integer = 0 To lintRSAAgentRetryCount

                'Add delay when retry
                If intCurrentRetry > 0 Then
                    Threading.Thread.Sleep(lintRSAAgentRetryDelay * 1000)
                End If

                udtAuditLogEntry.AddDescripton("Action", "Auth")
                udtAuditLogEntry.AddDescripton("UserID", pstrLoginID)
                udtAuditLogEntry.AddDescripton("Passcode", pstrPasscode)
                udtAuditLogEntry.AddDescripton("Main", IIf(pblnMain, "Y", "N"))
                udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
                udtAuditLogEntry.AddDescripton("RSAAgentMethod", lstrRSAAgentMethod)

                If intCurrentRetry > 0 Then
                    udtAuditLogEntry.AddDescripton("CurrentRetry", intCurrentRetry)
                    udtAuditLogEntry.AddDescripton("MaxRetry", lstrRSAAgentRetryCount)
                End If
                udtAuditLogEntry.WriteStartLog(LogID.LOG00001, "AuthRSAUser")

                Try
                    ' Check if the config file exist
                    If lstrRSAAgentConfPath <> String.Empty AndAlso File.Exists(lstrRSAAgentConfPath) = False Then
                        Throw New Exception(String.Format("Config File not found (lstrRSAAgentConfPath={0})", lstrRSAAgentConfPath))
                    End If

                    'set up remote certificate validation
                    ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

                    Dim lstrStackTrace As String = String.Empty
                    Dim lintResult As Integer

                    'Call RSAAgent through Web Service
                    If lstrRSAAgentMethod = RSAAgentMethod.WebService Then

                        Dim lstrRSAAgentWSLink As String = String.Empty
                        Dim lstrResult As String = String.Empty
                        Dim ws As New AuthService

                        GeneralFunction.getSystemParameter(String.Format("RSA{0}AgentWSLink", lstrRSAAPIVersion), lstrRSAAgentWSLink, String.Empty)

                        ws.Url = lstrRSAAgentWSLink

                        lstrResult = ws.Authenticate(lstrRSAAgentConfPath, pstrLoginID, pstrPasscode)

                        If Not lstrResult.Contains("|||") Then
                            lintResult = lstrResult
                        Else
                            lintResult = lstrResult.Split(New String() {"|||"}, StringSplitOptions.None)(0)
                            lstrStackTrace = lstrResult.Split(New String() {"|||"}, StringSplitOptions.None)(1)
                        End If

                    ElseIf lstrRSAAgentMethod = RSAAgentMethod.DirectCall Then
                        Dim lsbStackTrace As New StringBuilder(200)

                        lintResult = auth(lstrRSAAgentConfPath, pstrLoginID, pstrPasscode, lsbStackTrace)
                        lstrStackTrace = lsbStackTrace.ToString

                    Else
                        Throw New Exception(String.Format("Unknown Method (lstrRSAAgentMethod={0})", lstrRSAAgentMethod))
                    End If

                    udtAuditLogEntry.AddDescripton("UserID", pstrLoginID)
                    udtAuditLogEntry.AddDescripton("Result", lintResult)

                    If lstrStackTrace = String.Empty Then
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00002, "AuthRSAUser successful")
                        'Return lintResult = "0"
                        Return lintResult
                    Else
                        udtAuditLogEntry.AddDescripton("StackTrace", lstrStackTrace)
                        udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "AuthRSAUser fail")
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("Exception", ex.ToString)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00003, "AuthRSAUser fail")

                    If pblnMain Then Throw

                    Return 1
                End Try

            Next

            Return 1
            ' CRE15-001 RSA Server Upgrade - add retry mechanism [End][Winnie]
        End Function

        '

        Public Function AuthWithNextTokenMode(ByVal pstrLoginID As String, ByVal pstrPasscode As String, ByVal pblnHaveReplacement As Boolean, ByRef pstrSessionIDMain As String, ByRef pstrSessionIDSub As String) As Integer
            Dim lintResult As Integer = 1

            ' Main RSA server
            lintResult = HandleAuthWithNextTokenMode(pstrLoginID, pstrPasscode, True, pstrSessionIDMain)

            ' Sub RSA server
            If gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains(IIf(pblnHaveReplacement, "A2", "A1")) Then
                HandleAuthWithNextTokenMode(pstrLoginID, pstrPasscode, False, pstrSessionIDSub)
            End If

            Return lintResult

        End Function

        Private Function HandleAuthWithNextTokenMode(ByVal pstrLoginID As String, ByVal pstrPasscode As String, ByVal pblnMain As Boolean, ByRef pstrSessionID As String) As Integer

            Dim udtAuditLogEntry As AuditLogEntry

            If pblnMain Then
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
            Else
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
            End If

            udtAuditLogEntry.AddDescripton("Action", "AuthWithNextTokenMode")
            udtAuditLogEntry.AddDescripton("UserID", pstrLoginID)
            udtAuditLogEntry.AddDescripton("Passcode", pstrPasscode)
            udtAuditLogEntry.AddDescripton("SessionID", pstrSessionID)
            udtAuditLogEntry.AddDescripton("Main", IIf(pblnMain, "Y", "N"))
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00034, "AuthWithNextTokenMode")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00036, "AuthWithNextTokenMode fail")

                    Throw

                End Try


                Dim c1 As New LoginCommand
                c1.authenticationMethodId = SecurIDAuthenticationConstants.METHOD_ID

                c1.identitySourceGuid = gudtRSAIdentitySource.guid

                Dim lstParm As New List(Of FieldParameterDTO)
                Dim p As FieldParameterDTO = Nothing

                If pstrSessionID <> String.Empty Then
                    ' Next Token Mode
                    c1.sessionId = pstrSessionID

                    p = New FieldParameterDTO
                    p.promptKey = "NEXT_TOKENCODE"
                    p.value = pstrPasscode

                    lstParm.Add(p)

                Else
                    ' Normal authentication
                    p = New FieldParameterDTO
                    p.promptKey = AuthenticationConstants.PRINCIPAL_ID
                    p.value = pstrLoginID

                    lstParm.Add(p)

                    p = New FieldParameterDTO
                    p.promptKey = AuthenticationConstants.PASSCODE
                    p.value = pstrPasscode

                    lstParm.Add(p)

                End If

                c1.parameters = lstParm.ToArray

                Try
                    c1.execute()

                Catch ex As AuthenticationCommandException
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00036, "AuthWithNextTokenMode fail")

                    Throw

                End Try

                udtAuditLogEntry.AddDescripton("Result", c1.authenticationState)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00035, "AuthWithNextTokenMode successful")

                Select Case c1.authenticationState
                    Case "authenticated"
                        Dim c2 As New LogoutCommand
                        c2.sessionId = c1.sessionId
                        c2.execute()

                        Return 0

                    Case "failed"
                        Return 1

                    Case "in_progress"
                        pstrSessionID = c1.sessionId
                        Return 2

                End Select

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("Exception", ex.ToString)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00036, "AuthWithNextTokenMode fail")

                If pblnMain Then Throw

                Return 1
            End Try


        End Function

#End Region

#Region "Add/Update/Delete (Category B)"

        Public Function addRSAUser(ByVal pstrLoginID As String, ByVal pstrTokenSerialNo As String, Optional ByVal pstrDBFlag As String = Nothing) As SystemMessage
            Dim udtSystemMessage As SystemMessage = Nothing

            ' Main RSA server
            udtSystemMessage = HandleAddRSAUser(pstrLoginID, pstrTokenSerialNo, True, pstrDBFlag)

            ' Sub RSA server
            If IsNothing(udtSystemMessage) AndAlso gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("B") Then
                HandleAddRSAUser(pstrLoginID, pstrTokenSerialNo, False, pstrDBFlag)
            End If

            Return udtSystemMessage

        End Function

        Private Function HandleAddRSAUser(ByVal pstrLoginID As String, ByVal pstrTokenSerialNo As String, ByVal pblnMain As Boolean, ByVal pstrDBFlag As String) As SystemMessage
            ' Convert field
            pstrLoginID = pstrLoginID.Trim
            pstrTokenSerialNo = formatTokenSerialNo(pstrTokenSerialNo)

            Dim udtAuditLogEntry As AuditLogEntry

            If IsNothing(pstrDBFlag) Then
                If pblnMain Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
                Else
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
                End If
            Else
                ' Call from Interface
                If pblnMain Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main, pstrDBFlag)
                Else
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub, pstrDBFlag)
                End If
            End If

            udtAuditLogEntry.AddDescripton("Action", "Add")
            udtAuditLogEntry.AddDescripton("LoginID", pstrLoginID)
            udtAuditLogEntry.AddDescripton("LastName", "NA")
            udtAuditLogEntry.AddDescripton("FirstName", "NA")
            udtAuditLogEntry.AddDescripton("TokenSerialNo", pstrTokenSerialNo)
            udtAuditLogEntry.AddDescripton("Main", IIf(pblnMain, "Y", "N"))
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00004, "AddRSAUser")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "AddRSAUser fail")

                    ' Token service is temporary not available. Please try again later!
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00001)
                End Try

                ' --- Validation ---
                If pstrLoginID = String.Empty OrElse pstrTokenSerialNo = String.Empty Then
                    Throw New Exception(String.Format("RSAServerHandler.addRSAUser: Missing mandatory field (LoginID={0},TokenSerialNo={1})", pstrLoginID, pstrTokenSerialNo))
                End If

                ' User already exist
                Dim c1 As New SearchPrincipalsCommand
                c1.filter = Filter.equal(PrincipalDTO._LOGINUID, pstrLoginID)
                c1.limit = Integer.MaxValue
                c1.identitySourceGuid = gudtRSAIdentitySource.guid
                c1.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid

                c1.execute()

                If c1.principals.Length <> 0 Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Principal {0} exists already", pstrLoginID))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "AddRSAUser successful")

                    ' The token cannot be added / activated to the token server. The User ID is already existed in the Token server.
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00007)
                End If

                ' Token Serial No. is valid
                If IsNumeric(pstrTokenSerialNo) = False Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Invalid format of TokenSerialNo {0}", pstrTokenSerialNo))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "AddRSAUser successful")

                    ' The token cannot be added / activated to the token server. The token is not a valid registered token.
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00009)
                End If

                ' Token already assigned
                Dim c2 As New LookupTokenCommand

                c2.serialNumber = pstrTokenSerialNo

                Try
                    c2.execute()

                Catch ex As DataNotFoundException
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("TokenSerialNo {0} not found", pstrTokenSerialNo))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "AddRSAUser successful")

                    ' The token cannot be added / activated to the token server. The token is not a valid registered token.
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00009)

                End Try

                If c2.token.assignedUserId <> String.Empty Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("TokenSerialNo {0} assigned to {1} already", pstrTokenSerialNo, c2.token.assignedUserId))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "AddRSAUser successful")

                    ' The token cannot be added / activated to the token server. The token is assigned to another user already.
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00008)
                End If
                ' --- End of Validation ---

                ' Add Principal
                Dim p As New PrincipalDTO
                p.userID = pstrLoginID
                p.firstName = "NA"
                p.lastName = "NA"
                p.password = "eHSSPPass1234!"

                p.enabled = True
                p.accountStartDate = DateTime.Now
                p.canBeImpersonated = False
                p.trustToImpersonate = False

                p.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid
                p.identitySourceGuid = gudtRSAIdentitySource.guid
                p.passwordExpired = False

                Dim c3 As New AddPrincipalsCommand
                c3.principals = New PrincipalDTO() {p}

                Try
                    c3.execute()

                Catch ex1 As DuplicateDataException
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Principal {0} exists already", pstrLoginID))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "AddRSAUser successful")

                    ' The token cannot be added / activated to the token server. The User ID is already existed in the Token server.
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00007)

                Catch ex2 As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Exception from AddPrincipalsCommand: {0}", ex2.ToString))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "AddRSAUser fail")

                    ' The token cannot be added / activated to the token server. The token may be added by other users already, or not a valid registered token.
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00002)

                End Try

                ' Enquire the Principal again to get GUID
                Dim c4 As New SearchPrincipalsCommand
                c4.filter = Filter.equal(PrincipalDTO._LOGINUID, pstrLoginID)
                c4.limit = Integer.MaxValue
                c4.identitySourceGuid = gudtRSAIdentitySource.guid
                c4.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid

                c4.execute()

                ' Assign Token
                Dim c5 As New LinkTokensWithPrincipalCommand
                c5.tokenGuids = New String() {c2.token.id}
                c5.principalGuid = c4.principals(0).guid

                Try
                    c5.execute()

                Catch ex As Exception
                    ' Exception, rollback to delete the principal
                    Dim c6 As New DeletePrincipalsCommand
                    c6.guids = New String() {c5.principalGuid}
                    c6.identitySourceGuid = gudtRSAIdentitySource.guid
                    c6.execute()

                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Exception from LinkTokensWithPrincipalCommand: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "AddRSAUser fail")

                    ' The token cannot be added / activated to the token server. The token may be added by other users already, or not a valid registered token.
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00002)

                End Try

                udtAuditLogEntry.WriteEndLog(LogID.LOG00005, "AddRSAUser successful")

                Return Nothing

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00006, "AddRSAUser fail")

                ' Token service is temporary not available. Please try again later!
                Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00001)

            End Try

        End Function

        '

        Public Function deleteRSAUser(ByVal pstrLoginID As String, Optional ByVal pstrDBFlag As String = Nothing) As Boolean
            Dim lblnResult As Boolean = False

            ' Main RSA server
            lblnResult = HandleDeleteRSAUser(pstrLoginID, True, pstrDBFlag)

            ' Sub RSA server
            If lblnResult = True AndAlso gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("B") Then
                HandleDeleteRSAUser(pstrLoginID, False, pstrDBFlag)
            End If

            Return lblnResult

        End Function

        Private Function HandleDeleteRSAUser(ByVal pstrLoginID As String, ByVal pblnMain As Boolean, ByVal pstrDBFlag As String) As Boolean
            ' Convert field
            pstrLoginID = pstrLoginID.Trim

            Dim udtAuditLogEntry As AuditLogEntry = Nothing

            If IsNothing(pstrDBFlag) Then
                If pblnMain Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
                Else
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
                End If
            Else
                ' Call from Interface
                If pblnMain Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main, pstrDBFlag)
                Else
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub, pstrDBFlag)
                End If
            End If

            udtAuditLogEntry.AddDescripton("Action", "Delete")
            udtAuditLogEntry.AddDescripton("LoginID", pstrLoginID)
            udtAuditLogEntry.AddDescripton("Main", IIf(pblnMain, "Y", "N"))
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00007, "DeleteRSAUser")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "DeleteRSAUser fail")

                    Return False

                End Try

                ' --- Validation ---
                Dim c1 As New SearchPrincipalsCommand

                c1.filter = Filter.equal(PrincipalDTO._LOGINUID, pstrLoginID)
                c1.limit = Integer.MaxValue
                c1.identitySourceGuid = gudtRSAIdentitySource.guid
                c1.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid

                c1.execute()

                If c1.principals.Length = 0 Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Principal {0} not found", pstrLoginID))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "DeleteRSAUser fail")
                    Return False
                End If

                ' --- End of Validation ---

                Dim c2 As New DeletePrincipalsCommand
                c2.guids = New String() {c1.principals(0).guid}
                c2.identitySourceGuid = gudtRSAIdentitySource.guid

                Try
                    c2.execute()
                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("DeletePrincipalsCommand fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "DeleteRSAUser fail")

                    Return False
                End Try

                udtAuditLogEntry.WriteEndLog(LogID.LOG00008, "DeleteRSAUser successful")

                Return True

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00009, "DeleteRSAUser fail")

                If pblnMain Then Throw

                Return False

            End Try

        End Function

        '

        Public Function updateRSAUserToken(ByVal pstrOldTokenSerialNo As String, ByVal pstrNewTokenSerialNo As String) As SystemMessage
            Dim udtSystemMessage As SystemMessage = Nothing

            ' Main RSA server
            udtSystemMessage = HandleUpdateRSAUserToken(pstrOldTokenSerialNo, pstrNewTokenSerialNo, True)

            ' Sub RSA server
            If IsNothing(udtSystemMessage) AndAlso gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("B") Then
                HandleUpdateRSAUserToken(pstrOldTokenSerialNo, pstrNewTokenSerialNo, False)
            End If

            Return udtSystemMessage

        End Function

        Private Function HandleUpdateRSAUserToken(ByVal pstrOldTokenSerialNo As String, ByVal pstrNewTokenSerialNo As String, ByVal pblnMain As Boolean) As SystemMessage
            ' Convert field
            pstrOldTokenSerialNo = formatTokenSerialNo(pstrOldTokenSerialNo)
            pstrNewTokenSerialNo = formatTokenSerialNo(pstrNewTokenSerialNo)

            Dim udtAuditLogEntry As AuditLogEntry

            If pblnMain Then
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
            Else
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
            End If

            udtAuditLogEntry.AddDescripton("Action", "UpdateToken")
            udtAuditLogEntry.AddDescripton("OldSerial", pstrOldTokenSerialNo)
            udtAuditLogEntry.AddDescripton("NewSerial", pstrNewTokenSerialNo)
            udtAuditLogEntry.AddDescripton("Main", IIf(pblnMain, "Y", "N"))
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00010, "UpdateRSAUserToken")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "UpdateRSAUserToken fail")

                    ' Token service is temporary not available. Please try again later!
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00001)

                End Try

                ' --- Validation ---

                ' Old token
                If IsNumeric(pstrOldTokenSerialNo) = False Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Invalid old token {0}", pstrOldTokenSerialNo))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "UpdateRSAUserToken successful")

                    ' The token cannot be refreshed. The original token is not assigned.
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00013)

                End If

                Dim c1 As New LookupTokenCommand
                c1.serialNumber = pstrOldTokenSerialNo

                Try
                    c1.execute()

                Catch ex As DataNotFoundException
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Old token {0} not found", pstrOldTokenSerialNo))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "UpdateRSAUserToken successful")

                    ' The token cannot be refreshed. The original token is not assigned.
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00013)

                End Try

                If c1.token.assignedUserId = String.Empty Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Old token {0} is not assigned to anyone", pstrOldTokenSerialNo))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "UpdateRSAUserToken successful")

                    ' The token cannot be refreshed. The original token is not assigned.
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00013)

                End If

                ' New token
                If IsNumeric(pstrNewTokenSerialNo) = False Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Invalid new token {0}", pstrNewTokenSerialNo))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "UpdateRSAUserToken successful")

                    ' The token cannot be refreshed. The new token is not a valid registered token.
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00014)

                End If

                Dim c2 As New LookupTokenCommand
                c2.serialNumber = pstrNewTokenSerialNo

                Try
                    c2.execute()

                Catch ex As DataNotFoundException
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("New token {0} not found", pstrNewTokenSerialNo))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "UpdateRSAUserToken successful")

                    ' The token cannot be refreshed. The new token is not a valid registered token.
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00014)

                End Try

                If c2.token.assignedUserId <> String.Empty Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("New token {0} is already assigned to {1}", pstrNewTokenSerialNo, c2.token.assignedUserId))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "UpdateRSAUserToken successful")

                    ' The token cannot be refreshed. The new token is already assigned to another user. 
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00012)

                End If
                ' --- End of Validation ---

                Dim c3 As New UnlinkTokensFromPrincipalsCommand

                c3.tokenGuids = New String() {c1.token.id}

                Try
                    c3.execute()

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Exception from UnlinkTokensFromPrincipalsCommand: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "UpdateRSAUserToken fail")

                    ' Token service is temporary not available. Please try again later!
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00001)

                End Try

                ' Find the principal GUID
                Dim c4 As New SearchPrincipalsCommand
                c4.filter = Filter.equal(PrincipalDTO._LOGINUID, c1.token.assignedUserId)
                c4.limit = Integer.MaxValue
                c4.identitySourceGuid = gudtRSAIdentitySource.guid
                c4.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid

                Try
                    c4.execute()

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Exception from SearchPrincipalsCommand: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "UpdateRSAUserToken fail")

                    ' Token service is temporary not available. Please try again later!
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00001)

                End Try

                If c4.principals.Length <> 1 Then
                    Throw New Exception(String.Format("RSAServerHandler.updateRSAUserToken: Unexpected value (c4.principals.Length={0})", c4.principals.Length))
                End If

                ' Link new token
                Dim c5 As New LinkTokensWithPrincipalCommand
                c5.tokenGuids = New String() {c2.token.id}
                c5.principalGuid = c4.principals(0).guid

                Try
                    c5.execute()

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Exception from LinkTokensWithPrincipalCommand: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "UpdateRSAUserToken fail")

                    ' Token service is temporary not available. Please try again later!
                    Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00001)

                End Try

                udtAuditLogEntry.WriteEndLog(LogID.LOG00011, "UpdateRSAUserToken successful")

                Return Nothing

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00012, "UpdateRSAUserToken fail")

                ' Token service is temporary not available. Please try again later!
                Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00001)

            End Try

        End Function

        '

        Public Function replaceRSAUserToken(ByVal pstrTokenSerialNo As String, ByVal pstrTokenSerialNoReplacement As String, Optional ByVal pstrDBFlag As String = Nothing) As String
            Dim lstrResult As String = String.Empty

            ' Main RSA server
            lstrResult = HandleReplaceRSAUserToken(pstrTokenSerialNo, pstrTokenSerialNoReplacement, True, pstrDBFlag)

            ' Sub RSA server
            If lstrResult = "0" AndAlso gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("B") Then
                HandleReplaceRSAUserToken(pstrTokenSerialNo, pstrTokenSerialNoReplacement, False, pstrDBFlag)
            End If

            Return lstrResult

        End Function

        Private Function HandleReplaceRSAUserToken(ByVal pstrTokenSerialNo As String, ByVal pstrTokenSerialNoReplacement As String, ByVal pblnMain As Boolean, Optional ByVal pstrDBFlag As String = Nothing) As String
            ' Convert field
            pstrTokenSerialNo = formatTokenSerialNo(pstrTokenSerialNo)
            pstrTokenSerialNoReplacement = formatTokenSerialNo(pstrTokenSerialNoReplacement)

            Dim udtAuditLogEntry As AuditLogEntry = Nothing

            If IsNothing(pstrDBFlag) Then
                If pblnMain Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
                Else
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
                End If
            Else
                ' Call from Interface
                If pblnMain Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main, pstrDBFlag)
                Else
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub, pstrDBFlag)
                End If
            End If


            udtAuditLogEntry.AddDescripton("Action", "ReplaceToken")
            udtAuditLogEntry.AddDescripton("CurrentSerial", pstrTokenSerialNo)
            udtAuditLogEntry.AddDescripton("ReplacementSerial", pstrTokenSerialNoReplacement)
            udtAuditLogEntry.AddDescripton("Main", IIf(pblnMain, "Y", "N"))
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00013, "ReplaceRSAUserToken")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "ReplaceRSAUserToken fail")

                    ' The token cannot be replaced. The replacement token may be activated by other users already, or not a valid registered token.
                    Return "9"

                End Try

                ' --- Validation ---
                If pstrTokenSerialNo = pstrTokenSerialNoReplacement Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Two numbers cannot be same (CurrentSerial={0},ReplacementSerial={1})", pstrTokenSerialNo, pstrTokenSerialNoReplacement))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "ReplaceRSAUserToken successful")

                    ' Return: Token is already assigned
                    Return "1"
                End If

                If IsNumeric(pstrTokenSerialNo) = False Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Current token invalid (CurrentSerial={0})", pstrTokenSerialNo))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "ReplaceRSAUserToken successful")

                    ' Return: Invalid token
                    Return "3"
                End If

                If IsNumeric(pstrTokenSerialNoReplacement) = False Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Replacement token invalid (ReplacementSerial={0})", pstrTokenSerialNoReplacement))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "ReplaceRSAUserToken successful")

                    ' Return: Invalid token
                    Return "3"
                End If

                ' Validate current token
                Dim c1 As New LookupTokenCommand
                c1.serialNumber = pstrTokenSerialNo

                Try
                    c1.execute()
                Catch ex As DataNotFoundException
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Current token not found (CurrentSerial={0})", c1.serialNumber))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "ReplaceRSAUserToken successful")

                    ' Return: Token is not assigned
                    Return "2"
                End Try

                If c1.token.assignedUserId = String.Empty Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Current token not assigned to any user (CurrentSerial={0})", c1.serialNumber))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "ReplaceRSAUserToken successful")

                    ' Return: Token is not assigned
                    Return "2"
                End If

                If c1.token.replacementMode = TokenDTO._IS_REPLACEMENT_TKN Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Current Token is already a replacement token for {0} (CurrentSerial={1})", c1.token.replaceTknSN, c1.serialNumber))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "ReplaceRSAUserToken successful")

                    ' Return: Token is already assigned
                    Return "1"
                End If

                If c1.token.replaceTknSN = pstrTokenSerialNoReplacement Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("This replacement is already in progress (CurrentSerial={0},ReplacementSerial={1})", c1.serialNumber, c1.token.replaceTknSN))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "ReplaceRSAUserToken successful")

                    ' Return: Token is already assigned
                    Return "1"
                End If

                ' Validate replacement token
                Dim c2 As New LookupTokenCommand
                c2.serialNumber = pstrTokenSerialNoReplacement

                Try
                    c2.execute()
                Catch ex As DataNotFoundException
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Replacement token not found (ReplacementSerial={0})", c2.serialNumber))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "ReplaceRSAUserToken successful")

                    ' Return: Invalid token
                    Return "3"
                End Try

                If c2.token.assignedUserId <> String.Empty AndAlso c2.token.assignedUserId <> c1.token.assignedUserId Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Replacement token is assigned to {0} (ReplacementSerial={1})", c2.token.assignedUserId, c2.serialNumber))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "ReplaceRSAUserToken successful")

                    ' Return: Token is already assigned
                    Return "1"
                End If
                ' --- End of Validation ---

                ' Unassign the previous replacement token
                If c1.token.replacementMode = TokenDTO._HAS_REPLACEMENT_TKN Then
                    Dim c4 As New LookupTokenCommand
                    c4.serialNumber = c1.token.replaceTknSN
                    c4.execute()

                    Dim c5 As New UnlinkTokensFromPrincipalsCommand
                    c5.tokenGuids = New String() {c4.token.id}
                    c5.execute()

                End If

                ' Replace token
                Dim c3 As New ReplaceTokensCommand
                Dim r3 As New ReplaceTokenDTO
                r3.replacingTokenGuid = c1.token.id
                r3.replacementTokenGuid = c2.token.id

                c3.repTknDTO = New ReplaceTokenDTO() {r3}
                c3.execute()

                udtAuditLogEntry.WriteEndLog(LogID.LOG00014, "ReplaceRSAUserToken successful")

                Return "0"

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00015, "ReplaceRSAUserToken fail")

                If pblnMain Then Throw

                Return String.Empty

            End Try

        End Function

        Public Function enableRSAUserToken(ByVal pstrTokenSerialNo As String) As Boolean
            Dim lblnResult As Boolean = False

            ' Main RSA server
            lblnResult = HandleEnableRSAUserToken(pstrTokenSerialNo, True)

            ' Sub RSA server
            If lblnResult = True AndAlso gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("B") Then
                HandleEnableRSAUserToken(pstrTokenSerialNo, False)
            End If

            Return lblnResult

        End Function

        Private Function HandleEnableRSAUserToken(ByVal pstrTokenSerialNo As String, ByVal pblnMain As Boolean) As Boolean
            ' Convert field
            pstrTokenSerialNo = formatTokenSerialNo(pstrTokenSerialNo)

            Dim udtAuditLogEntry As AuditLogEntry = Nothing

            If pblnMain Then
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
            Else
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
            End If

            udtAuditLogEntry.AddDescripton("Action", "EnableToken")
            udtAuditLogEntry.AddDescripton("TokenSerialNo", pstrTokenSerialNo)
            udtAuditLogEntry.AddDescripton("Main", IIf(pblnMain, "Y", "N"))
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00016, "EnableRSAUserToken")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "EnableRSAUserToken fail")

                    Return False

                End Try

                Dim c1 As New LookupTokenCommand

                c1.serialNumber = pstrTokenSerialNo

                Try
                    c1.execute()

                Catch ex As DataNotFoundException
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("TokenSerialNo {0} not found", pstrTokenSerialNo))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "EnableRSAUserToken fail")

                    Return False

                End Try

                Dim c2 As New EnableTokensCommand
                c2.tokenGuids = New String() {c1.token.id}
                c2.enable = True

                c2.execute()

                udtAuditLogEntry.WriteEndLog(LogID.LOG00017, "EnableRSAUserToken successful")

                Return True

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00018, "EnableRSAUserToken fail")

                If pblnMain Then Throw

                Return False

            End Try

        End Function

        Public Function disableRSAUserToken(ByVal pstrTokenSerialNo As String) As Boolean
            Return True
        End Function


        '

        Public Function resetRSAUserToken(ByVal pstrTokenSerialNo As String) As Boolean
            Dim lblnResult As Boolean = False

            ' Main RSA server
            lblnResult = HandleResetRSAUserToken(pstrTokenSerialNo, True)

            ' Sub RSA server
            If lblnResult = True AndAlso gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("B") Then
                HandleResetRSAUserToken(pstrTokenSerialNo, False)
            End If

            Return lblnResult

        End Function

        Private Function HandleResetRSAUserToken(ByVal pstrTokenSerialNo As String, ByVal pblnMain As Boolean) As Boolean
            ' Convert field
            pstrTokenSerialNo = formatTokenSerialNo(pstrTokenSerialNo)

            Dim udtAuditLogEntry As AuditLogEntry = Nothing

            If pblnMain Then
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
            Else
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
            End If

            udtAuditLogEntry.AddDescripton("Action", "ResetToken")
            udtAuditLogEntry.AddDescripton("TokenSerialNo", pstrTokenSerialNo)
            udtAuditLogEntry.AddDescripton("Main", IIf(pblnMain, "Y", "N"))
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00022, "ResetRSAUserToken")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "ResetRSAUserToken fail")

                    Return False

                End Try

                ' --- Validation ---
                Dim c1 As New LookupTokenCommand
                c1.serialNumber = pstrTokenSerialNo

                Try
                    c1.execute()

                Catch ex As DataNotFoundException
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Token not found (TokenSerialNo={0})", pstrTokenSerialNo))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "ResetRSAUserToken fail")

                    Return False

                End Try
                ' --- End of Validation ---

                Dim c2 As New SearchPrincipalsCommand

                c2.filter = Filter.equal(PrincipalDTO._LOGINUID, c1.token.assignedUserId)
                c2.limit = Integer.MaxValue
                c2.identitySourceGuid = gudtRSAIdentitySource.guid
                c2.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid

                c2.execute()

                Dim c3 As New UpdatePrincipalCommand
                c3.identitySourceGuid = gudtRSAIdentitySource.guid

                Dim u As New UpdatePrincipalDTO
                u.guid = c2.principals(0).guid

                ' Copy the rowVersion to satisfy optimistic locking requirements
                u.rowVersion = c2.principals(0).rowVersion

                ' Collect all modifications here
                Dim llstM As New List(Of ModificationDTO)
                Dim m As ModificationDTO

                ' Lockout
                m = New ModificationDTO
                m.operation = ModificationDTO._REPLACE_ATTRIBUTE
                m.name = PrincipalDTO._LOCKOUT_FLAG
                m.values = New Object() {False}
                llstM.Add(m)

                u.modifications = llstM.ToArray()
                c3.principalModification = u
                c3.execute()

                If c1.token.replacementMode = TokenDTO._NO_REPLACEMENT_TKN Then
                    ' No replacement token, delete the RSAUser and add it back to reset all fields
                    udtAuditLogEntry.AddDescripton("Action", "DeleteUser_AddUser")

                    ' Delete
                    Dim c4 As New DeletePrincipalsCommand
                    c4.guids = New String() {c2.principals(0).guid}
                    c4.identitySourceGuid = gudtRSAIdentitySource.guid

                    Try
                        c4.execute()

                    Catch ex As Exception
                        udtAuditLogEntry.AddDescripton("StackTrace", String.Format("DeletePrincipalsCommand fail: {0}", ex.Message))
                        Throw

                    End Try

                    ' Add
                    Dim p As New PrincipalDTO
                    p.userID = c1.token.assignedUserId
                    p.firstName = "NA"
                    p.lastName = "NA"
                    p.password = "eHSSPPass1234!"

                    p.enabled = True
                    p.accountStartDate = DateTime.Now
                    p.canBeImpersonated = False
                    p.trustToImpersonate = False

                    p.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid
                    p.identitySourceGuid = gudtRSAIdentitySource.guid
                    p.passwordExpired = False

                    Dim c5 As New AddPrincipalsCommand
                    c5.principals = New PrincipalDTO() {p}

                    Try
                        c5.execute()

                    Catch ex As Exception
                        udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Exception from AddPrincipalsCommand: {0}", ex.ToString))
                        Throw

                    End Try

                    ' Enquire the Principal again to get GUID
                    Dim c6 As New SearchPrincipalsCommand
                    c6.filter = Filter.equal(PrincipalDTO._LOGINUID, c1.token.assignedUserId)
                    c6.limit = Integer.MaxValue
                    c6.identitySourceGuid = gudtRSAIdentitySource.guid
                    c6.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid

                    c6.execute()

                    ' Assign Token
                    Dim c7 As New LinkTokensWithPrincipalCommand
                    c7.tokenGuids = New String() {c1.token.id}
                    c7.principalGuid = c6.principals(0).guid

                    Try
                        c7.execute()

                    Catch ex As Exception
                        udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Exception from LinkTokensWithPrincipalCommand: {0}", ex.Message))
                        Throw

                    End Try

                End If

                udtAuditLogEntry.WriteEndLog(LogID.LOG00023, "ResetRSAUserToken successful")

                Return True

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00024, "ResetRSAUserToken fail")

                If pblnMain Then Throw

                Return False

            End Try

        End Function

        '
        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Function clearRSAUserTokenFailCount(ByVal pstrLoginID As String) As Boolean
            Dim lblnResult As Boolean = False

            ' Main RSA server
            lblnResult = HandleClearRSAUserTokenFailCount(pstrLoginID, True)

            ' Sub RSA server
            If lblnResult = True AndAlso gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("B") Then
                HandleClearRSAUserTokenFailCount(pstrLoginID, False)
            End If

            Return lblnResult

        End Function

        Private Function HandleClearRSAUserTokenFailCount(ByVal pstrLoginID As String, ByVal pblnMain As Boolean) As Boolean
            ' Convert field
            pstrLoginID = pstrLoginID.Trim

            Dim udtAuditLogEntry As AuditLogEntry = Nothing

            If pblnMain Then
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
            Else
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
            End If

            udtAuditLogEntry.AddDescripton("Action", "ClearTokenFailCount")
            udtAuditLogEntry.AddDescripton("LoginID", pstrLoginID)
            udtAuditLogEntry.AddDescripton("Main", IIf(pblnMain, "Y", "N"))
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00043, "ClearRSAUserTokenFailCount")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00045, "ClearRSAUserTokenFailCount fail")

                    Return False

                End Try

                ' --- Validation ---
                Dim c1 As New SearchPrincipalsCommand

                c1.filter = Filter.equal(PrincipalDTO._LOGINUID, pstrLoginID)
                c1.limit = Integer.MaxValue
                c1.identitySourceGuid = gudtRSAIdentitySource.guid
                c1.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid

                c1.execute()

                If c1.principals.Length = 0 Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Principal {0} not found", pstrLoginID))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00045, "ClearRSAUserTokenFailCount fail")
                    Return False
                End If
                ' --- End of Validation ---

                ' Clear Lockout status
                Dim c2 As New UpdatePrincipalCommand
                c2.identitySourceGuid = gudtRSAIdentitySource.guid

                Dim u As New UpdatePrincipalDTO
                u.guid = c1.principals(0).guid

                ' Copy the rowVersion to satisfy optimistic locking requirements
                u.rowVersion = c1.principals(0).rowVersion

                ' Collect all modifications here
                Dim llstM As New List(Of ModificationDTO)
                Dim m As ModificationDTO

                ' Lockout
                m = New ModificationDTO
                m.operation = ModificationDTO._REPLACE_ATTRIBUTE
                m.name = PrincipalDTO._LOCKOUT_FLAG
                m.values = New Object() {False}
                llstM.Add(m)

                u.modifications = llstM.ToArray()
                c2.principalModification = u
                c2.execute()


                ' Clear Token Fail Count (for both Existing and Replacement token, also remove the NTM status)
                Dim c3 As New ClearBadTokenCountCommand
                c3.principalGuid = c1.principals(0).guid
                c3.execute()

                udtAuditLogEntry.WriteEndLog(LogID.LOG00044, "ClearRSAUserTokenFailCount successful")

                Return True

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00045, "ClearRSAUserTokenFailCount fail")

                If pblnMain Then Throw

                Return False

            End Try

        End Function

        Public Function resetLockoutStatus(ByVal pstrLoginID As String) As Boolean
            Dim lblnResult As Boolean = False

            ' Main RSA server
            lblnResult = HandleResetLockoutStatus(pstrLoginID, True)

            ' Sub RSA server
            If lblnResult = True AndAlso gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("B") Then
                HandleResetLockoutStatus(pstrLoginID, False)
            End If

            Return lblnResult

        End Function

        Private Function HandleResetLockoutStatus(ByVal pstrLoginID As String, ByVal pblnMain As Boolean) As Boolean
            ' Convert field
            pstrLoginID = pstrLoginID.Trim

            Dim udtAuditLogEntry As AuditLogEntry = Nothing

            If pblnMain Then
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
            Else
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
            End If

            udtAuditLogEntry.AddDescripton("Action", "ResetLockoutStatus")
            udtAuditLogEntry.AddDescripton("LoginID", pstrLoginID)
            udtAuditLogEntry.AddDescripton("Main", IIf(pblnMain, "Y", "N"))
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00049, "ResetLockoutStatus")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00051, "ResetLockoutStatus fail")

                    Return False

                End Try

                ' --- Validation ---
                Dim c1 As New SearchPrincipalsCommand

                c1.filter = Filter.equal(PrincipalDTO._LOGINUID, pstrLoginID)
                c1.limit = Integer.MaxValue
                c1.identitySourceGuid = gudtRSAIdentitySource.guid
                c1.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid

                c1.execute()

                If c1.principals.Length = 0 Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Principal {0} not found", pstrLoginID))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00051, "ResetLockoutStatus fail")
                    Return False
                End If
                ' --- End of Validation ---

                ' Reset Lockout status
                Dim c2 As New UpdatePrincipalCommand
                c2.identitySourceGuid = gudtRSAIdentitySource.guid

                Dim u As New UpdatePrincipalDTO
                u.guid = c1.principals(0).guid

                ' Copy the rowVersion to satisfy optimistic locking requirements
                u.rowVersion = c1.principals(0).rowVersion

                ' Collect all modifications here
                Dim llstM As New List(Of ModificationDTO)
                Dim m As ModificationDTO

                ' Set Lockout = False
                m = New ModificationDTO
                m.operation = ModificationDTO._REPLACE_ATTRIBUTE
                m.name = PrincipalDTO._LOCKOUT_FLAG
                m.values = New Object() {False}
                llstM.Add(m)

                u.modifications = llstM.ToArray()
                c2.principalModification = u
                c2.execute()

                udtAuditLogEntry.WriteEndLog(LogID.LOG00050, "ResetLockoutStatus successful")

                Return True

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00051, "ResetLockoutStatus fail")

                If pblnMain Then Throw

                Return False

            End Try

        End Function
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]

#End Region

#Region "Enquire (Category C)"

        Public Function listRSAUserTokenByLoginID(ByVal pstrLoginID As String, Optional ByVal pstrPlatform As String = "", Optional ByVal pstrUniqueID As String = "") As String
            Dim lstrResult As String = String.Empty

            ' Main RSA server
            lstrResult = HandleListRSAUserTokenByLoginID(pstrLoginID, pstrPlatform, pstrUniqueID, True)

            ' Sub RSA server
            If gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("C") Then
                HandleListRSAUserTokenByLoginID(pstrLoginID, pstrPlatform, pstrUniqueID, False)
            End If

            Return lstrResult

        End Function

        Private Function HandleListRSAUserTokenByLoginID(ByVal pstrLoginID As String, ByVal pstrPlatform As String, ByVal pstrUniqueID As String, ByVal pblnMain As Boolean) As String
            ' Convert field
            pstrLoginID = pstrLoginID.Trim

            Dim udtAuditLogEntry As Object = Nothing

            If pstrPlatform = String.Empty Then

                If pblnMain Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
                Else
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
                End If
            ElseIf pstrPlatform = "IVRS" Then

                If pblnMain Then
                    udtAuditLogEntry = New AuditLogIVRSEntry(FUNCTION_CODE_RSA_Main, IVRS_Entry.IVRS_HCSP, pstrUniqueID)
                Else
                    udtAuditLogEntry = New AuditLogIVRSEntry(FUNCTION_CODE_RSA_Sub, IVRS_Entry.IVRS_HCSP, pstrUniqueID)
                End If
            End If

            udtAuditLogEntry.AddDescripton("Action", "ListSerialByID")
            udtAuditLogEntry.AddDescripton("LoginID", pstrLoginID)
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00025, "ListRSAUserTokenByLoginID")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00027, "ListRSAUserTokenByLoginID fail")

                    If pblnMain Then Throw

                    Return String.Empty

                End Try

                Dim c1 As New SearchPrincipalsCommand
                c1.filter = Filter.equal(PrincipalDTO._LOGINUID, pstrLoginID)
                c1.limit = Integer.MaxValue
                c1.identitySourceGuid = gudtRSAIdentitySource.guid
                c1.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid

                c1.execute()

                If c1.principals.Length = 0 Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("No principal found (LoginID={0})", pstrLoginID))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00026, "ListRSAUserTokenByLoginID successful")

                    Return String.Empty

                End If

                Dim c2 As New ListTokensByPrincipalCommand
                c2.principalId = c1.principals(0).guid
                c2.execute()

                If c2.tokenDTOs.Length = 0 Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("No token found (LoginID={0})", pstrLoginID))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00026, "ListRSAUserTokenByLoginID successful")

                    Return String.Empty

                Else
                    ' Exclude replacement token
                    Dim lstrToken As String = String.Empty
                    Dim lstrReplacementToken As String = String.Empty

                    For Each t As ListTokenDTO In c2.tokenDTOs
                        Select Case t.replacementMode
                            Case TokenDTO._NO_REPLACEMENT_TKN, TokenDTO._HAS_REPLACEMENT_TKN
                                lstrToken = t.serialNumber.TrimStart("0")

                            Case TokenDTO._IS_REPLACEMENT_TKN
                                lstrReplacementToken = t.serialNumber.TrimStart("0")

                        End Select

                    Next

                    udtAuditLogEntry.AddDescripton("TokenSerial", lstrToken)
                    udtAuditLogEntry.AddDescripton("ReplacementTokenSerial", lstrReplacementToken)
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00026, "ListRSAUserTokenByLoginID successful")

                    If lstrReplacementToken = String.Empty Then
                        Return lstrToken
                    Else
                        Return String.Format("{0},{1}", lstrToken, lstrReplacementToken)
                    End If

                End If

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00027, "ListRSAUserTokenByLoginID fail")

                If pblnMain Then Throw

                Return String.Empty

            End Try

        End Function

        '

        Public Function listRSAUserByTokenSerialNo(ByVal pstrTokenSerialNo As String, Optional ByVal pstrDBFlag As String = Nothing) As String
            Dim lstrResult As String = String.Empty

            ' Main RSA server
            lstrResult = HandleListRSAUserByTokenSerialNo(pstrTokenSerialNo, pstrDBFlag, True)

            ' Sub RSA server
            If gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("C") Then
                HandleListRSAUserByTokenSerialNo(pstrTokenSerialNo, pstrDBFlag, False)
            End If

            Return lstrResult

        End Function

        Private Function HandleListRSAUserByTokenSerialNo(ByVal pstrTokenSerialNo As String, ByVal pstrDBFlag As String, ByVal pblnMain As Boolean) As String
            ' Convert field
            pstrTokenSerialNo = formatTokenSerialNo(pstrTokenSerialNo)

            Dim udtAuditLogEntry As AuditLogEntry = Nothing

            If IsNothing(pstrDBFlag) Then
                If pblnMain Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
                Else
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
                End If
            Else
                If pblnMain Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main, pstrDBFlag)
                Else
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub, pstrDBFlag)
                End If
            End If

            udtAuditLogEntry.AddDescripton("Action", "List")
            udtAuditLogEntry.AddDescripton("TokenSerialNo", pstrTokenSerialNo)
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00028, "listRSAUserByTokenSerialNo")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00030, "listRSAUserByTokenSerialNo fail")

                    If pblnMain Then Throw

                    Return String.Empty

                End Try

                Dim c1 As New LookupTokenCommand
                c1.serialNumber = pstrTokenSerialNo

                Try
                    c1.execute()

                Catch ex As DataNotFoundException
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Token not found (TokenSerialNo={0})", pstrTokenSerialNo))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00029, "listRSAUserByTokenSerialNo successful")

                    Return String.Empty

                End Try

                Dim lstrAssignedUserID As String = String.Empty

                If Not IsNothing(c1.token.assignedUserId) Then
                    lstrAssignedUserID = c1.token.assignedUserId
                End If

                udtAuditLogEntry.AddDescripton("AssignedUserID", lstrAssignedUserID)
                udtAuditLogEntry.WriteEndLog(LogID.LOG00029, "listRSAUserByTokenSerialNo successful")

                Return lstrAssignedUserID

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00030, "listRSAUserByTokenSerialNo fail")

                If pblnMain Then Throw

                Return String.Empty

            End Try

        End Function

        '

        Public Function IsTokenInNextTokenMode(ByVal pstrTokenSerialNo As String) As Boolean
            Dim lstrResult As String = String.Empty

            ' Main RSA server
            lstrResult = HandleIsTokenInNextTokenMode(pstrTokenSerialNo, True)

            ' Sub RSA server
            If gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("C") Then
                HandleIsTokenInNextTokenMode(pstrTokenSerialNo, False)
            End If

            Return lstrResult

        End Function

        Private Function HandleIsTokenInNextTokenMode(ByVal pstrTokenSerialNo As String, ByVal pblnMain As Boolean) As Boolean
            ' Convert field
            pstrTokenSerialNo = formatTokenSerialNo(pstrTokenSerialNo)

            Dim udtAuditLogEntry As AuditLogEntry = Nothing

            If pblnMain Then
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
            Else
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
            End If

            udtAuditLogEntry.AddDescripton("Action", "IsTokenInNextTokenMode")
            udtAuditLogEntry.AddDescripton("TokenSerialNo", pstrTokenSerialNo)
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00031, "IsTokenInNextTokenMode")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00033, "IsTokenInNextTokenMode fail")

                    Throw

                End Try

                Dim c1 As New LookupTokenCommand
                c1.serialNumber = pstrTokenSerialNo

                Try
                    c1.execute()

                Catch ex As DataNotFoundException
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Token not found (TokenSerialNo={0})", pstrTokenSerialNo))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00033, "IsTokenInNextTokenMode fail")

                    Throw

                End Try

                Dim lblnNextTokenMode As Boolean = c1.token.nextTokenCodeMode

                udtAuditLogEntry.AddDescripton("NextTokenMode", IIf(lblnNextTokenMode, "Y", "N"))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00032, "IsTokenInNextTokenMode successful")

                Return lblnNextTokenMode

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00033, "IsTokenInNextTokenMode fail")

                If pblnMain Then Throw

                Return False

            End Try

        End Function

        '

        Public Function IsUserIDAndTokenAvailable(ByVal pstrLoginID As String, ByVal pstrTokenSerialNo As String) As SystemMessage
            Dim udtSystemMessage As SystemMessage = Nothing

            ' Main RSA server
            udtSystemMessage = HandleIsUserIDAndTokenAvailable(pstrLoginID, pstrTokenSerialNo, True)

            ' Sub RSA server
            If IsNothing(udtSystemMessage) AndAlso gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("C") Then
                HandleIsUserIDAndTokenAvailable(pstrLoginID, pstrTokenSerialNo, False)
            End If

            Return udtSystemMessage

        End Function

        Public Function HandleIsUserIDAndTokenAvailable(ByVal pstrLoginID As String, ByVal pstrTokenSerialNo As String, ByVal pblnMain As Boolean) As SystemMessage
            ' Convert field
            pstrLoginID = pstrLoginID.Trim
            pstrTokenSerialNo = formatTokenSerialNo(pstrTokenSerialNo)

            Dim udtAuditLogEntry As AuditLogEntry = Nothing

            If pblnMain Then
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
            Else
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
            End If

            udtAuditLogEntry.AddDescripton("LoginID", pstrLoginID)
            udtAuditLogEntry.AddDescripton("TokenSerialNo", pstrTokenSerialNo)
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00037, "IsUserIDAndTokenAvailable")

            Try
                If pblnMain Then
                    InitRSA(gstrRSAAPIVersionMain)
                Else
                    InitRSA(gstrRSAAPIVersionSub)
                End If

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00039, "IsUserIDAndTokenAvailable fail")

                ' Token service is temporary not available. Please try again later!
                Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00001)

            End Try

            ' --- Validation ---

            ' User already exist
            Dim c1 As New SearchPrincipalsCommand
            c1.filter = Filter.equal(PrincipalDTO._LOGINUID, pstrLoginID)
            c1.limit = Integer.MaxValue
            c1.identitySourceGuid = gudtRSAIdentitySource.guid
            c1.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid

            c1.execute()

            If c1.principals.Length <> 0 Then
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Principal {0} exists already", pstrLoginID))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00038, "IsUserIDAndTokenAvailable successful")

                ' The token cannot be added / activated to the token server. The User ID is already existed in the Token server.
                Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00007)
            End If

            ' Token Serial No. is valid
            If IsNumeric(pstrTokenSerialNo) = False Then
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Invalid format of TokenSerialNo {0}", pstrTokenSerialNo))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00038, "IsUserIDAndTokenAvailable successful")

                ' The token cannot be added / activated to the token server. The token is not a valid registered token.
                Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00009)
            End If

            ' Token already assigned
            Dim c2 As New LookupTokenCommand

            c2.serialNumber = pstrTokenSerialNo

            Try
                c2.execute()

            Catch ex As DataNotFoundException
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("TokenSerialNo {0} not found", pstrTokenSerialNo))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00038, "IsUserIDAndTokenAvailable successful")

                ' The token cannot be added / activated to the token server. The token is not a valid registered token.
                Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00009)

            End Try

            If c2.token.assignedUserId <> String.Empty Then
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("TokenSerialNo {0} assigned to {1} already", pstrTokenSerialNo, c2.token.assignedUserId))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00038, "IsUserIDAndTokenAvailable successful")

                ' The token cannot be added / activated to the token server. The token is assigned to another user already.
                Return New SystemMessage(FunctCode.FUNT010202, "E", MsgCode.MSG00008)
            End If
            ' --- End of Validation ---

            udtAuditLogEntry.WriteEndLog(LogID.LOG00038, "IsUserIDAndTokenAvailable successful")

            Return Nothing

        End Function

        '

        Public Function IsTokenExistAndFreeToAssign(ByVal pstrTokenSerialNo As String, Optional ByVal pstrDBFlag As String = Nothing) As Boolean
            ' Main RSA server
            Dim blnResult As Boolean = HandleIsTokenExistAndFreeToAssign(pstrTokenSerialNo, True, pstrDBFlag)

            ' Sub RSA server
            If gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("C") Then
                HandleIsTokenExistAndFreeToAssign(pstrTokenSerialNo, False, pstrDBFlag)
            End If

            Return blnResult

        End Function

        Private Function HandleIsTokenExistAndFreeToAssign(ByVal pstrTokenSerialNo As String, ByVal pblnMain As Boolean, ByVal pstrDBFlag As String) As Boolean
            ' Convert field
            pstrTokenSerialNo = formatTokenSerialNo(pstrTokenSerialNo)

            Dim udtAuditLogEntry As AuditLogEntry = Nothing

            If IsNothing(pstrDBFlag) Then
                If pblnMain Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
                Else
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
                End If
            Else
                ' Call from Interface
                If pblnMain Then
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main, pstrDBFlag)
                Else
                    udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub, pstrDBFlag)
                End If
            End If

            udtAuditLogEntry.AddDescripton("TokenSerialNo", pstrTokenSerialNo)
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00040, "IsTokenExistAndFreeToAssign")

            Try
                If pblnMain Then
                    InitRSA(gstrRSAAPIVersionMain)
                Else
                    InitRSA(gstrRSAAPIVersionSub)
                End If

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00042, "IsTokenExistAndFreeToAssign fail")

                If pblnMain Then Throw

            End Try

            ' --- Validation ---

            ' Token Serial No. is valid
            If IsNumeric(pstrTokenSerialNo) = False Then
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Invalid format of TokenSerialNo {0}", pstrTokenSerialNo))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00041, "IsTokenExistAndFreeToAssign successful")

                Return False

            End If

            ' Token already assigned
            Dim c2 As New LookupTokenCommand

            c2.serialNumber = pstrTokenSerialNo

            Try
                c2.execute()

            Catch ex As DataNotFoundException
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("TokenSerialNo {0} not found", pstrTokenSerialNo))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00041, "IsTokenExistAndFreeToAssign successful")

                Return False

            End Try

            If c2.token.assignedUserId <> String.Empty Then
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("TokenSerialNo {0} assigned to {1} already", pstrTokenSerialNo, c2.token.assignedUserId))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00041, "IsTokenExistAndFreeToAssign successful")

                Return False

            End If
            ' --- End of Validation ---

            udtAuditLogEntry.WriteEndLog(LogID.LOG00041, "IsTokenExistAndFreeToAssign successful")

            Return True

        End Function

        '
        ' CRE16-004 (Enable SP to unlock account) [Start][Winnie]
        ' -----------------------------------------------------------------------------------------
        Public Function IsUserLockout(ByVal pstrLoginID As String) As Boolean
            Dim lstrResult As String = String.Empty

            ' Main RSA server
            lstrResult = HandleIsUserLockout(pstrLoginID, True)

            ' Sub RSA server
            If gstrRSAAPIVersionSub <> String.Empty AndAlso glstRSAAPIRepeatCategory.Contains("C") Then
                HandleIsUserLockout(pstrLoginID, False)
            End If

            Return lstrResult

        End Function

        Private Function HandleIsUserLockout(ByVal pstrLoginID As String, ByVal pblnMain As Boolean) As Boolean
            ' Convert field
            pstrLoginID = pstrLoginID.Trim

            Dim udtAuditLogEntry As AuditLogEntry = Nothing

            If pblnMain Then
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Main)
            Else
                udtAuditLogEntry = New AuditLogEntry(FUNCTION_CODE_RSA_Sub)
            End If

            udtAuditLogEntry.AddDescripton("Action", "IsUserLockout")
            udtAuditLogEntry.AddDescripton("LoginID", pstrLoginID)
            udtAuditLogEntry.AddDescripton("RSAVersion", IIf(pblnMain, gstrRSAAPIVersionMain, gstrRSAAPIVersionSub))
            udtAuditLogEntry.WriteStartLog(LogID.LOG00046, "IsUserLockout")

            Try
                Try
                    If pblnMain Then
                        InitRSA(gstrRSAAPIVersionMain)
                    Else
                        InitRSA(gstrRSAAPIVersionSub)
                    End If

                Catch ex As Exception
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("InitRSA fail: {0}", ex.Message))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00048, "IsUserLockout fail")

                    Throw

                End Try

                Dim c1 As New SearchPrincipalsCommand

                c1.filter = Filter.equal(PrincipalDTO._LOGINUID, pstrLoginID)
                c1.limit = Integer.MaxValue
                c1.identitySourceGuid = gudtRSAIdentitySource.guid
                c1.securityDomainGuid = gudtRSAIdentitySource.securityDomainGuid

                c1.execute()

                If c1.principals.Length = 0 Then
                    udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Principal {0} not found", pstrLoginID))
                    udtAuditLogEntry.WriteEndLog(LogID.LOG00048, "IsUserLockout fail")
                    Return False
                End If

                Dim p As PrincipalDTO = c1.principals(0)

                Dim blnLockout As Boolean = p.lockoutStatus

                udtAuditLogEntry.AddDescripton("PrincipalLockout", IIf(blnLockout, "Y", "N"))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00047, "IsUserLockout successful")

                Return blnLockout

            Catch ex As Exception
                udtAuditLogEntry.AddDescripton("StackTrace", String.Format("Unknown exception: {0}", ex.ToString))
                udtAuditLogEntry.WriteEndLog(LogID.LOG00048, "IsUserLockout fail")

                If pblnMain Then Throw

                Return False

            End Try

        End Function
        ' CRE16-004 (Enable SP to unlock account) [End][Winnie]
#End Region

#Region "Supporting Functions"

        Private Function formatTokenSerialNo(ByVal TokenSerialNo As String) As String
            Return TokenSerialNo.Trim().PadLeft(12, "0")
        End Function

        Private Function IsNumeric(ByVal strInput As String) As Boolean
            Return New Regex("^[0-9]+$").IsMatch(strInput.Trim)
        End Function

        '

        Public Function GetRSAAPIVersionMain() As String
            Return gstrRSAAPIVersionMain
        End Function

        Public Function IsParallelRun() As String
            Return (gstrRSAAPIVersionSub <> String.Empty)
        End Function
        ' CRE15-001 RSA Server Upgrade [End][Winnie]

#End Region

    End Class

End Namespace
' CRE13-029 - RSA Server Upgrade [End][Lawrence]
