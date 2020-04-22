Module Core

    Sub Main(ByVal cmdArgs() As String)

        Dim objImmdTransfer As New ImmdTransfer

        objImmdTransfer.ShowCmdArgs(cmdArgs)

        If objImmdTransfer.NeedCmdArgsHelp(cmdArgs, 3, 5, "Action,Setting File Path,Log File Root Folder,Section,Execution Date") Then

        Else
            objImmdTransfer.Start(cmdArgs)
        End If

    End Sub

End Module
