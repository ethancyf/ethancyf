Module Module1

    ' CRE16-025 (Lowering voucher eligibility age) [Start][Winnie]
    ' Add argument to specifiy which task would be executed
    Sub Main(ByVal args() As String)

        ProgramMgr.GetInstance().StartProcess(args)

    End Sub
    ' CRE16-025 (Lowering voucher eligibility age) [End][Winnie]
End Module
