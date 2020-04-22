Imports System
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
        If args.Length <> 1 Then
            Console.WriteLine("Usage: RSA7Compare [Inputile]")
            Return
        End If

        Console.WriteLine("Program start at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))

        Dim lstrInputFile As String = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, args(0).Trim)

        Console.Write("Check input file... ")

        If File.Exists(lstrInputFile) = False Then
            Console.WriteLine("Error: Input file not exist! (Path={0})", lstrInputFile)
            Return
        End If

        Dim lstrOutputFile As String = String.Format("{0}_result.csv", lstrInputFile.Substring(0, lstrInputFile.Length - 4))
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
        dt.Columns.Add("6Token", GetType(String))
        dt.Columns.Add("6TokenRaw", GetType(String))
        dt.Columns.Add("6TokenEnable", GetType(String))
        dt.Columns.Add("6TokenNTM", GetType(String))
        dt.Columns.Add("7Token", GetType(String))
        dt.Columns.Add("7TokenEnable", GetType(String))
        dt.Columns.Add("7TokenNTM", GetType(String))

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
            dr("6Token") = String.Format("[{0}]", laryLine(1).Trim)
            dr("6TokenRaw") = laryLine(1).Trim
            dr("6TokenEnable") = laryLine(2).Trim
            dr("6TokenNTM") = laryLine(3).Trim
            dr("7Token") = String.Empty
            dr("7TokenEnable") = String.Empty
            dr("7TokenNTM") = String.Empty

            dt.Rows.Add(dr)

        Next

        Console.WriteLine("OK")

        ' --- Init RSA7 ---
        Console.Write("Initialize RSA 7.1 connection... ")

        Dim udtRSA7IdentitySource As IdentitySourceDTO = InitRSA7()

        Console.WriteLine("OK")

        ' --- Compare RSA7 Token ---
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
            c1.identitySourceGuid = udtRSA7IdentitySource.guid
            c1.securityDomainGuid = udtRSA7IdentitySource.securityDomainGuid

            c1.execute()

            If c1.principals.Length = 0 Then
                dr("7Token") = "NoToken"
            Else
                ' Find token
                Dim c2 As New ListTokensByPrincipalCommand
                c2.principalId = c1.principals(0).guid
                c2.execute()

                If c2.tokenDTOs.Length = 0 Then
                    dr("7Token") = "NoToken"

                Else
                    Dim llstToken As New List(Of String)
                    Dim llstTokenEnable As New List(Of String)
                    Dim llstTokenNTM As New List(Of String)

                    For Each t As ListTokenDTO In c2.tokenDTOs
                        llstToken.Add(t.serialNumber)
                        llstTokenEnable.Add(IIf(t.enable, "Y", "N"))
                        llstTokenNTM.Add(IIf(t.nextTokenMode, "Y", "N"))
                    Next

                    dr("7Token") = String.Format("[{0}]", String.Join("|", llstToken.ToArray))
                    dr("7TokenEnable") = String.Join("|", llstTokenEnable.ToArray)
                    dr("7TokenNTM") = String.Join("|", llstTokenNTM.ToArray)

                End If

            End If

            dr.AcceptChanges()

            t2 = DateTime.Now

            If dr("6Token") = dr("7Token") AndAlso dr("6TokenEnable") = dr("7TokenEnable") AndAlso dr("6TokenNTM") = dr("7TokenNTM") Then
                Console.WriteLine(String.Format("Matched ({0}ms)", CInt((t2.Ticks - t1.Ticks) / 10000)))

            Else
                Console.WriteLine(String.Format("Not matched ({0}ms)", CInt((t2.Ticks - t1.Ticks) / 10000)))

                If intUnmatch = 0 Then
                    objStreamWriter = New StreamWriter(lstrOutputFile, True)
                    objStreamWriter.WriteLine("User ID,RSA6 Token,RSA6 Token Enable,RSA6 Token NTM,RSA7 Token,RSA7 Token Enable,RSA7 Token NTM")
                    objStreamWriter.Close()
                End If

                objStreamWriter = New StreamWriter(lstrOutputFile, True)
                objStreamWriter.WriteLine(String.Format("{0},{1},{2},{3},{4},{5},{6}", dr("PrincipalID"), _
                                            dr("6Token"), dr("6TokenEnable"), dr("6TokenNTM"), _
                                            dr("7Token"), dr("7TokenEnable"), dr("7TokenNTM")))
                objStreamWriter.Close()

                objStreamWriterRerun = New StreamWriter(lstrOutputFileRerun, True)
                objStreamWriterRerun.WriteLine(String.Format("{0}|||{1}|||{2}|||{3}", _
                                                             dr("PrincipalID"), dr("6TokenRaw"), dr("6TokenEnable"), dr("6TokenNTM")))
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

    Private Function InitRSA7() As IdentitySourceDTO
        'set up remote certificate validation
        ServicePointManager.ServerCertificateValidationCallback = New RemoteCertificateValidationCallback(AddressOf ValidateServerCertificate)

        ' establish a connected session with given credentials from arguments passed
        Dim conn As New SOAPCommandTarget(ConfigurationManager.AppSettings("RSA7Link"), ConfigurationManager.AppSettings("RSA7WebLogicUsername"), ConfigurationManager.AppSettings("RSA7WebLogicPassword"))

        If Not conn.Login(ConfigurationManager.AppSettings("RSA7AMUsername"), ConfigurationManager.AppSettings("RSA7AMPassword")) Then
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
