Module Module1

    Private m_strExportFolderPath As String = ""
    Private m_strPassword As String = ""

    Sub Main()

        Console.WriteLine("Retrieve From DataBase:")
        Console.WriteLine("1. Primary Database")
        Console.WriteLine("2. Secondary Database")
        Console.WriteLine("3. Exit")
        Console.Write("?_")

        Dim strDB As String = Console.ReadLine()
        Dim intDB As Integer = -1
        While Not Integer.TryParse(strDB, intDB) OrElse (intDB < 0 And intDB > 3)
            Console.Write("?_")
            strDB = Console.ReadLine()
        End While

        Console.WriteLine("")

        Select Case intDB
            Case 1
                Console.WriteLine("Select Primary Database")
            Case 2
                Console.WriteLine("Select Secondary Database")
            Case 3
                Console.WriteLine("Exit....")
                Exit Sub
            Case Else
                Console.WriteLine("Unknown Selection")
                Exit Sub
        End Select

        Console.WriteLine("")
        Console.WriteLine("")
        Console.WriteLine("Export File..........")

        LoadParameters()

        Dim udtDB As Common.DataAccess.Database
        If intDB = 1 Then
            udtDB = New Common.DataAccess.Database(Common.Component.DBFlagStr.DBFlag)
        Else
            udtDB = New Common.DataAccess.Database(Common.Component.DBFlagStr.DBFlag2)
        End If

        ' Retrieve ImmD File
        ' 

        Try
            Dim params() As SqlClient.SqlParameter = {udtDB.MakeInParam("@curDate", SqlDbType.DateTime, 8, Now.Date)}
            Dim dtResult As New DataTable()
            udtDB.RunProc("proc_ImmdFile_get", params, dtResult)

            Dim arrByteFileContent As Byte()

            If dtResult.Rows.Count > 0 Then
                Console.WriteLine("Immd Entry Found: " + dtResult.Rows.Count.ToString())
                For i As Integer = 0 To dtResult.Rows.Count - 1


                    If Not IsDBNull(dtResult.Rows(i)("File_Export_Content")) Then
                        Dim strFileName As String = dtResult.Rows(i)("File_Name").ToString().Trim()
                        arrByteFileContent = CType(dtResult.Rows(i)("File_Export_Content"), Byte())

                        System.IO.File.WriteAllBytes(m_strExportFolderPath + strFileName, arrByteFileContent)

                        If Common.Encryption.Encrypt.EncryptWinRAR(m_strPassword, m_strExportFolderPath, strFileName) Then
                            ' Create Control File
                            System.IO.File.Create(m_strExportFolderPath + strFileName.Substring(0, strFileName.IndexOf(".")) + ".cf")
                            Console.WriteLine("File Export Success: " + m_strExportFolderPath + strFileName)
                        End If
                    Else
                        Console.WriteLine("No Immd Import File Found for entry:" + i.ToString())
                    End If
                Next
            Else
                Console.WriteLine("No Immd Entry Found")
            End If
        Catch ex As Exception
            Console.WriteLine("File Export Error: " + ex.ToString())
        End Try
    End Sub

    Private Sub LoadParameters()
        Dim strPath As String = String.Empty
        Dim udtCommonGeneralFunction As New Common.ComFunction.GeneralFunction()

        udtCommonGeneralFunction.getSystemParameter("ImmdExportFilePath", strPath, String.Empty)

        If strPath.Trim() = "" Then
            Throw New ArgumentException("ImmdExportFilePath Empty!")
        Else
            If strPath.EndsWith("\") Then
                m_strExportFolderPath = strPath
            Else
                m_strExportFolderPath = strPath & "\"
            End If
        End If

        udtCommonGeneralFunction.getSystemParameterPassword("ImmdExportFilePassword", m_strPassword)

    End Sub
End Module
