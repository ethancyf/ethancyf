Imports System
Imports System.IO
Imports System.Configuration
Imports System.Collections.Generic
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
            Console.WriteLine("Usage: RSA7PasswordUpdate [InputCSVFile]")
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
        dt.Columns.Add("Status", GetType(String))
        dt.Columns.Add("ErrorMessage", GetType(String))

        Dim dr As DataRow = Nothing

        For Each lstrLine As String In llstRecord
            If lstrLine.Contains(",") = False _
                    OrElse lstrLine.Split(",".ToCharArray, StringSplitOptions.None)(0).Trim = String.Empty _
                    OrElse lstrLine.Split(",".ToCharArray, StringSplitOptions.None)(1).Trim = String.Empty _
                    OrElse lstrLine.Split(",".ToCharArray, StringSplitOptions.None)(0).Trim.StartsWith("[") = False _
                    OrElse lstrLine.Split(",".ToCharArray, StringSplitOptions.None)(0).Trim.EndsWith("]") = False Then
                Console.WriteLine(String.Format("Error near line {0}: {1}", dt.Rows.Count + 1, lstrLine))
                Return

            End If

            dr = dt.NewRow
            dr("PrincipalID") = lstrLine.Split(",".ToCharArray, StringSplitOptions.None)(0).Trim
            dr("Status") = lstrLine.Split(",".ToCharArray, StringSplitOptions.None)(1).Trim
            dr("ErrorMessage") = String.Empty

            dt.Rows.Add(dr)

        Next

        Console.WriteLine("OK")

        ' --- Init RSA7 ---
        Console.Write("Initialize connection to RSA Server... ")

        Dim ludtIdentitySource As IdentitySourceDTO = Nothing

        Try
            ludtIdentitySource = InitRSA7()

        Catch ex As Exception
            Console.WriteLine(String.Format("Error: {0}", ex.Message))
            Return

        End Try

        Console.WriteLine("OK")

        ' --- Update Password ---
        Dim lintSkip As Integer = 0
        Dim lintDone As Integer = 0
        Dim lintFail As Integer = 0
        Dim t1 As DateTime = Nothing
        Dim t2 As DateTime = Nothing

        Dim objStreamWriter As StreamWriter = Nothing

        For Each dr In dt.Rows
            Console.Write(String.Format("({0} of {1}) Updating principal {2}... ", lintSkip + lintDone + lintFail + 1, dt.Rows.Count, dr("PrincipalID")))

            ' Only process the Status = 'P', 'E' records
            If dr("Status") <> "P" AndAlso dr("Status") <> "E" Then
                lintSkip += 1
                Console.WriteLine(String.Format("Skip status {0}", dr("Status")))

                objStreamWriter = New StreamWriter(lstrOutputFile, True)
                objStreamWriter.WriteLine(String.Format("{0},{1},{2}", dr("PrincipalID"), dr("Status"), dr("ErrorMessage")))
                objStreamWriter.Close()

                Continue For
            End If

            Try
                t1 = DateTime.Now

                Dim c1 As New SearchPrincipalsCommand

                Dim lstrPrincipalID As String = dr("PrincipalID").ToString.Substring(1, dr("PrincipalID").ToString.Length - 2)

                c1.filter = Filter.equal(PrincipalDTO._LOGINUID, lstrPrincipalID)
                c1.limit = Integer.MaxValue
                c1.identitySourceGuid = ludtIdentitySource.guid
                c1.securityDomainGuid = ludtIdentitySource.securityDomainGuid

                c1.execute()

                If c1.principals.Length = 0 Then
                    dr("Status") = "E"
                    dr("ErrorMessage") = "User ID not found"
                    dr.AcceptChanges()

                    lintFail += 1

                    Console.WriteLine(String.Format("Error: {0}", dr("ErrorMessage")))

                ElseIf c1.principals.Length > 1 Then
                    dr("Status") = "E"
                    dr("ErrorMessage") = "User ID found more than once"
                    dr.AcceptChanges()

                    lintFail += 1

                    Console.WriteLine(String.Format("Error: {0}", dr("ErrorMessage")))

                Else
                    Dim p As PrincipalDTO = c1.principals(0)

                    Dim c2 As New UpdatePrincipalCommand
                    c2.identitySourceGuid = ludtIdentitySource.guid

                    Dim u As New UpdatePrincipalDTO
                    u.guid = p.guid

                    ' Copy the rowVersion to satisfy optimistic locking requirements
                    u.rowVersion = p.rowVersion

                    ' collect all modifications here
                    Dim lstM As New List(Of ModificationDTO)
                    Dim m As ModificationDTO

                    ' Password
                    m = New ModificationDTO
                    m.operation = ModificationDTO._REPLACE_ATTRIBUTE
                    m.name = PrincipalDTO._PASSWORD
                    m.values = New Object() {"eHSSPPass1234!"}
                    lstM.Add(m)

                    u.modifications = lstM.ToArray()
                    c2.principalModification = u

                    c2.execute()

                    dr("Status") = "C"
                    dr.AcceptChanges()

                    lintDone += 1

                    t2 = DateTime.Now

                    Console.WriteLine(String.Format("Done ({0}ms)", CInt((t2.Ticks - t1.Ticks) / 10000)))

                End If

            Catch ex As Exception
                dr("Status") = "E"
                dr("ErrorMessage") = ex.Message
                dr.AcceptChanges()

                lintFail += 1

                Console.WriteLine(String.Format("Error: {0}", dr("ErrorMessage")))

            End Try

            objStreamWriter = New StreamWriter(lstrOutputFile, True)
            objStreamWriter.WriteLine(String.Format("{0},{1},{2}", dr("PrincipalID"), dr("Status"), dr("ErrorMessage")))
            objStreamWriter.Close()

        Next

        Console.WriteLine(String.Format("Result file OK (Path={0})", lstrOutputFile))

        ' --- Complete ---
        Console.WriteLine("Summary: {0} skipped / {1} updated / {2} error", lintSkip, lintDone, lintFail)

        Console.WriteLine("Program end at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))

    End Sub

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
