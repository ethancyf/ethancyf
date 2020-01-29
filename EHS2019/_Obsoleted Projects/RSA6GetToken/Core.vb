Imports System.IO
Imports System.Configuration
Imports RSA6GetToken.Component.RSA_Manager

Module Core

    Public Sub Main(ByVal args() As String)
        If args.Length <> 1 Then
            Console.WriteLine("Usage: RSA6GetToken [Inputile]")
            Return
        End If

        Console.WriteLine("Program start at {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))

        Dim lstrInputFile As String = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, args(0).Trim)

        Console.Write("Check input file... ")

        If File.Exists(lstrInputFile) = False Then
            Console.WriteLine("Error: Input file not exist! (Path={0})", lstrInputFile)
            Return
        End If

        Dim lstrOutputFile As String = String.Format("{0}_result.txt", lstrInputFile.Substring(0, lstrInputFile.Length - 4))

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

        ' --- Get Token ---
        Dim i As Integer = 1
        Dim ludtHttp As New ASPHTTP
        ludtHttp.URL = ConfigurationManager.AppSettings("RSA6Link")
        Dim laryResult() As String = Nothing
        Dim t1 As DateTime = Nothing
        Dim t2 As DateTime = Nothing

        For Each dr In dt.Rows
            Console.Write(String.Format("({0} of {1}) Get token of principal {2}... ", i, dt.Rows.Count, dr("PrincipalID")))

            t1 = DateTime.Now

            Try
                Dim lstrPrincipalID As String = dr("PrincipalID").ToString.Substring(1, dr("PrincipalID").ToString.Length - 2)

                ludtHttp.FormField = New FormField() {New FormField("p_action", "ListSerialByID"), New FormField("p_userID", lstrPrincipalID)}

                dr("Token") = ludtHttp.getResult()

                If dr("Token") = "1" Then
                    dr("Token") = "NoToken"

                Else
                    ' Check Next Token Mode
                    If dr("Token").Contains(",") Then
                        ' Two tokens
                        Dim llstToken As String() = dr("Token").Split(New String() {","}, StringSplitOptions.None)

                        dr("Token") = String.Format("{0}|{1}", llstToken(0).Trim, llstToken(1).Trim)

                        ludtHttp.FormField = New FormField() {New FormField("p_action", "ListTokenInfo"), New FormField("p_serial", llstToken(0).Trim)}

                        laryResult = ludtHttp.getResult().Split(New String() {","}, StringSplitOptions.None)

                        If laryResult(12).Trim = "1" Then
                            dr("TokenEnable") = "Y"
                        Else
                            dr("TokenEnable") = "N"
                        End If

                        If laryResult(15).Trim = "0" Then
                            dr("TokenNTM") = "N"
                        Else
                            dr("TokenNTM") = "Y"
                        End If

                        ludtHttp.FormField = New FormField() {New FormField("p_action", "ListTokenInfo"), New FormField("p_serial", llstToken(1).Trim)}

                        laryResult = ludtHttp.getResult().Split(New String() {","}, StringSplitOptions.None)

                        If laryResult(12).Trim = "1" Then
                            dr("TokenEnable") += "|Y"
                        Else
                            dr("TokenEnable") += "|N"
                        End If

                        If laryResult(15).Trim = "0" Then
                            dr("TokenNTM") += "|N"
                        Else
                            dr("TokenNTM") += "|Y"
                        End If

                    Else
                        ' One token
                        ludtHttp.FormField = New FormField() {New FormField("p_action", "ListTokenInfo"), New FormField("p_serial", dr("Token"))}

                        laryResult = ludtHttp.getResult().Split(New String() {","}, StringSplitOptions.None)

                        If laryResult(12).Trim = "1" Then
                            dr("TokenEnable") = "Y"
                        Else
                            dr("TokenEnable") = "N"
                        End If

                        If laryResult(15).Trim = "0" Then
                            dr("TokenNTM") = "N"
                        Else
                            dr("TokenNTM") = "Y"
                        End If

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

End Module
