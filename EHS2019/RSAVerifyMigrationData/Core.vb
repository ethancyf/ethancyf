﻿Imports System
Imports System.IO
Imports System.Configuration
Imports System.Net
Imports System.Net.Security
Imports System.Security.Cryptography.X509Certificates

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
Imports com.rsa.common
Imports com.rsa.command
Imports com.rsa.command.exception
Imports com.rsa.common.search

Module Core

    Public Sub Main(ByVal args() As String)
        Dim blnShowUasge As Boolean = False

        If args.Length <> 2 Then
            blnShowUasge = True
        Else
            If args(0).Trim.ToUpper.Equals("-GET") Then
                GetRSA(args(1).Trim)
            ElseIf args(0).Trim.ToUpper.Equals("-VERIFY") Then
                Verify(args(1).Trim)
            Else
                Console.WriteLine("Error: Unknown action!")
                blnShowUasge = True
            End If
        End If


        If blnShowUasge Then
            Console.WriteLine("Usage: RSAVerifyMigrationData [Action] [Inputfile]")
            Console.WriteLine("Action:")
            Console.WriteLine(String.Format("-get     Get the data from RSA {0}", ConfigurationManager.AppSettings("GetRSAVersion")))
            Console.WriteLine(String.Format("-verify  Use the given data to verify RSA {0}", ConfigurationManager.AppSettings("VerifyRSAVersion")))

            Console.WriteLine("")
            Console.WriteLine("Inputfile:")
            Console.WriteLine("For action '-get'    A text file with token users id")
            Console.WriteLine("For action '-verify' A text file with token information generated by action '-get'")
            Return
        End If

        Return

    End Sub

    Public Sub GetRSA(ByVal InputFile As String)

        Console.WriteLine("Program start at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))

        Dim lstrInputFile As String = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, InputFile)

        Console.Write("Check input file... ")

        If File.Exists(lstrInputFile) = False Then
            Console.WriteLine("Error: Input file not exist! (Path={0})", lstrInputFile)
            Return
        End If


        Dim OutputFile As String = String.Format("RSAGet_{0}_result_{1}_{2}.txt", InputFile.Substring(0, InputFile.Length - 4), DateTime.Now.ToString("yyyyMMdd"), DateTime.Now.ToString("HHmmss"))
        Dim lstrOutputFile As String = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, OutputFile)

        If File.Exists(lstrOutputFile) Then
            Console.WriteLine("Error: Target output file exist! Please delete before run (Path={0})", lstrOutputFile)
            Return
        End If

        Console.WriteLine("OK")

        ' --- Read file ---
        Console.Write("Read input file... ")

        Dim llstRecord As New List(Of String)

        Try
            Dim objStreamReader As New StreamReader(lstrInputFile)
            Dim lstrLine As String = objStreamReader.ReadLine

            Do While Not IsNothing(lstrLine) AndAlso lstrLine <> String.Empty
                llstRecord.Add(lstrLine)
                lstrLine = objStreamReader.ReadLine
            Loop

            objStreamReader.Close()

        Catch ex As Exception
            Console.WriteLine(String.Format("Error! (Exception={0})", ex.Message))
            Return
        End Try

        Console.WriteLine(String.Format("OK (NoOfLine={0})", llstRecord.Count))

        ' --- Validate file ---
        Console.Write("Validate input file... ")

        Dim dt As New DataTable
        dt.Columns.Add("PrincipalID", GetType(String))
        dt.Columns.Add("Token", GetType(String))
        dt.Columns.Add("TokenEnable", GetType(String))
        dt.Columns.Add("TokenNTM", GetType(String))

        Dim dr As DataRow = Nothing

        For Each lstrLine As String In llstRecord
            If lstrLine.Trim.StartsWith("[") = False OrElse lstrLine.Trim.EndsWith("]") = False Then
                Console.WriteLine(String.Format("Error near line {0}: {1}", dt.Rows.Count + 1, lstrLine))
                Return

            End If

            dr = dt.NewRow
            dr("PrincipalID") = lstrLine.Trim
            dr("Token") = String.Empty
            dr("TokenEnable") = String.Empty
            dr("TokenNTM") = String.Empty

            dt.Rows.Add(dr)

        Next

        Console.WriteLine("OK")

        ' --- Init RSA ---
        Console.Write(String.Format("Initialize RSA {0} connection... ", ConfigurationManager.AppSettings("GetRSAVersion")))

        Dim udtRSAIdentitySource As IdentitySourceDTO = InitRSA(ConfigurationManager.AppSettings("GetRSAVersion"))

        Console.WriteLine("OK")

        ' --- Get Token ---r
        Dim i As Integer = 1

        Dim laryResult() As String = Nothing
        Dim t1 As DateTime = Nothing
        Dim t2 As DateTime = Nothing

        For Each dr In dt.Rows
            Console.Write(String.Format("({0} of {1}) Get token of principal {2}... ", i, dt.Rows.Count, dr("PrincipalID")))

            t1 = DateTime.Now

            Try
                Dim lstrPrincipalID As String = dr("PrincipalID").ToString.Substring(1, dr("PrincipalID").ToString.Length - 2)



                Dim c1 As New SearchPrincipalsCommand
                c1.filter = Filter.equal(PrincipalDTO._LOGINUID, lstrPrincipalID)
                c1.limit = Integer.MaxValue
                c1.identitySourceGuid = udtRSAIdentitySource.guid
                c1.securityDomainGuid = udtRSAIdentitySource.securityDomainGuid

                c1.execute()

                If c1.principals.Length = 0 Then
                    dr("Token") = "NoToken"
                Else
                    ' Find token
                    Dim c2 As New ListTokensByPrincipalCommand
                    c2.principalId = c1.principals(0).guid
                    c2.execute()

                    If c2.tokenDTOs.Length = 0 Then
                        dr("Token") = "NoToken"

                    Else
                        Dim llstToken As New List(Of String)
                        Dim llstTokenEnable As New List(Of String)
                        Dim llstTokenNTM As New List(Of String)

                        For Each t As ListTokenDTO In c2.tokenDTOs
                            If t.replacementMode <> TokenDTO._IS_REPLACEMENT_TKN Then
                                llstToken.Add(t.serialNumber)
                                llstTokenEnable.Add(IIf(t.enable, "Y", "N"))
                                llstTokenNTM.Add(IIf(t.nextTokenMode, "Y", "N"))
                            End If
                        Next

                        If c2.tokenDTOs.Length > 1 Then
                            For Each t As ListTokenDTO In c2.tokenDTOs
                                If t.replacementMode = TokenDTO._IS_REPLACEMENT_TKN Then
                                    llstToken.Add(t.serialNumber)
                                    llstTokenEnable.Add(IIf(t.enable, "Y", "N"))
                                    llstTokenNTM.Add(IIf(t.nextTokenMode, "Y", "N"))
                                End If
                            Next
                        End If

                        dr("Token") = String.Format("[{0}]", String.Join("|", llstToken.ToArray))
                        dr("TokenEnable") = String.Join("|", llstTokenEnable.ToArray)
                        dr("TokenNTM") = String.Join("|", llstTokenNTM.ToArray)

                    End If

                End If


                t2 = DateTime.Now

                Console.WriteLine(String.Format("OK ({0}ms)", CInt((t2.Ticks - t1.Ticks) / 10000)))

                dr.AcceptChanges()

            Catch ex As Exception
                dr("Token") = String.Format("Error: {0}", ex.Message)
                dr.AcceptChanges()

                Console.WriteLine(String.Format("Error: {0}", ex.Message))

            End Try

            Dim objStreamWriter As New StreamWriter(lstrOutputFile, True)
            objStreamWriter.WriteLine(String.Format("{0}|||{1}|||{2}|||{3}", dr("PrincipalID"), dr("Token"), dr("TokenEnable"), dr("TokenNTM")))
            objStreamWriter.Close()

            i += 1

        Next

        Console.WriteLine(String.Format("Result file OK (Path={0})", lstrOutputFile))

        ' --- Complete ---
        Console.WriteLine("Program end at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))

    End Sub

    Public Sub Verify(ByVal InputFile As String)

        Console.WriteLine("Program start at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))

        Dim lstrInputFile As String = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, InputFile)

        Console.Write("Check input file... ")

        If File.Exists(lstrInputFile) = False Then
            Console.WriteLine("Error: Input file not exist! (Path={0})", lstrInputFile)
            Return
        End If

        Dim lstrOutputFile As String = String.Format("{0}_unmatched.csv", lstrInputFile.Substring(0, lstrInputFile.Length - 4))
        Dim lstrOutputFileRerun As String = String.Format("{0}_rerun.txt", lstrInputFile.Substring(0, lstrInputFile.Length - 4))

        If File.Exists(lstrOutputFile) Then
            Console.WriteLine("Error: Target output file exist! Please delete before run (Path={0})", lstrOutputFile)
            Return
        End If

        If File.Exists(lstrOutputFileRerun) Then
            Console.WriteLine("Error: Target output file exist! Please delete before run (Path={0})", lstrOutputFileRerun)
            Return
        End If

        Console.WriteLine("OK")

        ' --- Read file ---
        Console.Write("Read input file... ")

        Dim llstRecord As New List(Of String)

        Try
            Dim objStreamReader As New StreamReader(lstrInputFile)
            Dim lstrLine As String = objStreamReader.ReadLine

            Do While Not IsNothing(lstrLine) AndAlso lstrLine <> String.Empty
                llstRecord.Add(lstrLine)
                lstrLine = objStreamReader.ReadLine
            Loop

            objStreamReader.Close()

        Catch ex As Exception
            Console.WriteLine(String.Format("Error! (Exception={0})", ex.Message))
            Return
        End Try

        Console.WriteLine(String.Format("OK (NoOfLine={0})", llstRecord.Count))

        ' --- Validate file ---
        Console.Write("Validate input file... ")

        Dim dt As New DataTable
        dt.Columns.Add("PrincipalID", GetType(String))
        dt.Columns.Add("GetToken", GetType(String))
        dt.Columns.Add("GetTokenEnable", GetType(String))
        dt.Columns.Add("GetTokenNTM", GetType(String))
        dt.Columns.Add("VerifyToken", GetType(String))
        dt.Columns.Add("VerifyTokenEnable", GetType(String))
        dt.Columns.Add("VerifyTokenNTM", GetType(String))

        Dim dr As DataRow = Nothing
        Dim laryLine As String() = Nothing

        For Each lstrLine As String In llstRecord
            laryLine = lstrLine.Trim.Split(New String() {"|||"}, StringSplitOptions.None)

            If laryLine.Length <> 4 _
                    OrElse laryLine(0).Trim.StartsWith("[") = False _
                    OrElse laryLine(0).Trim.EndsWith("]") = False Then
                Console.WriteLine(String.Format("Error near line {0}: {1}", dt.Rows.Count + 1, lstrLine))
                Return

            End If

            dr = dt.NewRow
            dr("PrincipalID") = laryLine(0).Trim
            dr("GetToken") = laryLine(1).Trim
            dr("GetTokenEnable") = laryLine(2).Trim
            dr("GetTokenNTM") = laryLine(3).Trim
            dr("VerifyToken") = String.Empty
            dr("VerifyTokenEnable") = String.Empty
            dr("VerifyTokenNTM") = String.Empty

            dt.Rows.Add(dr)

        Next

        Console.WriteLine("OK")

        ' --- Init RSA ---
        Console.Write(String.Format("Initialize RSA {0} connection... ", ConfigurationManager.AppSettings("VerifyRSAVersion")))

        Dim udtRSAIdentitySource As IdentitySourceDTO = InitRSA(ConfigurationManager.AppSettings("VerifyRSAVersion"))


        Console.WriteLine("OK")

        ' --- Compare RSA Token ---
        Dim i As Integer = 1
        Dim intUnmatch As Integer = 0
        Dim t1 As DateTime = Nothing
        Dim t2 As DateTime = Nothing
        Dim objStreamWriter As StreamWriter = Nothing
        Dim objStreamWriterRerun As StreamWriter = Nothing

        For Each dr In dt.Rows
            t1 = DateTime.Now

            Console.Write(String.Format("({0} of {1}) Comparing principal {2}... ", i, dt.Rows.Count, dr("PrincipalID")))

            Dim lstrPrincipalID As String = dr("PrincipalID").ToString.Substring(1, dr("PrincipalID").ToString.Length - 2)

            Dim c1 As New SearchPrincipalsCommand
            c1.filter = Filter.equal(PrincipalDTO._LOGINUID, lstrPrincipalID)
            c1.limit = Integer.MaxValue
            c1.identitySourceGuid = udtRSAIdentitySource.guid
            c1.securityDomainGuid = udtRSAIdentitySource.securityDomainGuid

            c1.execute()

            If c1.principals.Length = 0 Then
                dr("VerifyToken") = "NoToken"
            Else
                ' Find token
                Dim c2 As New ListTokensByPrincipalCommand
                c2.principalId = c1.principals(0).guid
                c2.execute()

                If c2.tokenDTOs.Length = 0 Then
                    dr("VerifyToken") = "NoToken"

                Else
                    Dim llstToken As New List(Of String)
                    Dim llstTokenEnable As New List(Of String)
                    Dim llstTokenNTM As New List(Of String)

                    For Each t As ListTokenDTO In c2.tokenDTOs
                        If t.replacementMode <> TokenDTO._IS_REPLACEMENT_TKN Then
                            llstToken.Add(t.serialNumber)
                            llstTokenEnable.Add(IIf(t.enable, "Y", "N"))
                            llstTokenNTM.Add(IIf(t.nextTokenMode, "Y", "N"))
                        End If
                    Next
                    If c2.tokenDTOs.Length > 1 Then
                        For Each t As ListTokenDTO In c2.tokenDTOs
                            If t.replacementMode = TokenDTO._IS_REPLACEMENT_TKN Then
                                llstToken.Add(t.serialNumber)
                                llstTokenEnable.Add(IIf(t.enable, "Y", "N"))
                                llstTokenNTM.Add(IIf(t.nextTokenMode, "Y", "N"))
                            End If
                        Next
                    End If

                    dr("VerifyToken") = String.Format("[{0}]", String.Join("|", llstToken.ToArray))
                    dr("VerifyTokenEnable") = String.Join("|", llstTokenEnable.ToArray)
                    dr("VerifyTokenNTM") = String.Join("|", llstTokenNTM.ToArray)

                End If

            End If

            dr.AcceptChanges()

            t2 = DateTime.Now

            If dr("GetToken") = dr("VerifyToken") AndAlso dr("GetTokenEnable") = dr("VerifyTokenEnable") AndAlso dr("GetTokenNTM") = dr("VerifyTokenNTM") Then
                Console.WriteLine(String.Format("Matched ({0}ms)", CInt((t2.Ticks - t1.Ticks) / 10000)))

            Else
                Console.WriteLine(String.Format("Not matched ({0}ms)", CInt((t2.Ticks - t1.Ticks) / 10000)))

                If intUnmatch = 0 Then
                    objStreamWriter = New StreamWriter(lstrOutputFile, True)
                    objStreamWriter.WriteLine(String.Format("User ID,RSA{0} Token,RSA{0} Token Enable,RSA{0} Token NTM,RSA{1} Token,RSA{1} Token Enable,RSA{1} Token NTM",
                                                            ConfigurationManager.AppSettings("GetRSAVersion"),
                                                            ConfigurationManager.AppSettings("VerifyRSAVersion")))
                    objStreamWriter.Close()
                End If

                objStreamWriter = New StreamWriter(lstrOutputFile, True)
                objStreamWriter.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6}", dr("PrincipalID"), _
                                            dr("GetToken"), dr("GetTokenEnable"), dr("GetTokenNTM"), _
                                            dr("VerifyToken"), dr("VerifyTokenEnable"), dr("VerifyTokenNTM")))
                objStreamWriter.Close()

                objStreamWriterRerun = New StreamWriter(lstrOutputFileRerun, True)
                objStreamWriterRerun.WriteLine(String.Format("{0}|||{1}|||{2}|||{3}", _
                                                             dr("PrincipalID"), dr("GetToken"), dr("GetTokenEnable"), dr("GetTokenNTM")))
                objStreamWriterRerun.Close()

                intUnmatch += 1

            End If

            i += 1

        Next

        If intUnmatch <> 0 Then
            Console.WriteLine(String.Format("Unmatched record file OK (Path={0})", lstrOutputFile))
            Console.WriteLine(String.Format("Rerun record file OK (Path={0})", lstrOutputFileRerun))
        End If

        ' --- Complete ---
        Console.WriteLine(String.Format("Summary: {0} records compared, {1} unmatch", dt.Rows.Count, intUnmatch))

        Console.WriteLine("Program end at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))

    End Sub

    '

    Private Function InitRSA(ByVal RSAVersion As String) As IdentitySourceDTO
        'set up remote certificate validation
        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        ' establish a connected session with given credentials from arguments passed
        Dim conn As New SOAPCommandTarget(ConfigurationManager.AppSettings(String.Format("RSA{0}Link", RSAVersion)),
                                          ConfigurationManager.AppSettings(String.Format("RSA{0}WebLogicUsername", RSAVersion)),
                                          ConfigurationManager.AppSettings(String.Format("RSA{0}WebLogicPassword", RSAVersion)))

        If Not conn.Login(ConfigurationManager.AppSettings(String.Format("RSA{0}AMUsername", RSAVersion)),
                          ConfigurationManager.AppSettings(String.Format("RSA{0}AMPassword", RSAVersion))) Then
            Throw New Exception("Error: Unable to connect to the remote server. Please make sure your credentials are correct.")
        End If

        ' make all commands execute imports this target automatically
        CommandTargetPolicy.setDefaultCommandTarget(conn)

        Dim c1 As New GetIdentitySourcesCommand
        c1.execute()

        Return c1.identitySources(0)

    End Function

    Private Function ValidateServerCertificate(ByVal sender As Object, ByVal certificate As X509Certificate, ByVal chain As X509Chain, ByVal sslPolicyErrors As SslPolicyErrors) As Boolean
        Return True
    End Function

End Module
