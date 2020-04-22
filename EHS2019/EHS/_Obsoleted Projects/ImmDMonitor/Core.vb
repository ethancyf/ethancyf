Module Core

    Sub Main(ByVal cmdArgs() As String)

        Dim objImmdMonitor As New ImmdMonitor
        objImmdMonitor.Start(cmdArgs)

    End Sub

End Module
