Imports Common.Component

Namespace ComConfig

    Public Class FunctionCode
        Public Const PCDCheckIsActiveSP As String = FunctCode.FUNT060201
        Public Const PCDCreatePCDSPAcct As String = FunctCode.FUNT060202
        Public Const PCDTransferPracticeInfo As String = FunctCode.FUNT060203
        Public Const PCDUploadEnrolInfo As String = FunctCode.FUNT060204
        Public Const GetEHSPracticeScheme As String = FunctCode.FUNT060205
        Public Const PCDCheckAvailableForVerifiedEnrolment As String = FunctCode.FUNT060206
        Public Const PCDUploadVerifiedEnrolment As String = FunctCode.FUNT060207
        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [Start] (Marco) ---
        Public Const PCDCheckAccountStatus As String = FunctCode.FUNT060208
        ' --- CRE17-016 (Checking of PCD status during VSS enrolment) [End]   (Marco) ---
    End Class

    Public Class SystemLog
        Public Const MessageCode As String = "99999"
    End Class

    Public Class WSRequestSystem
        Public Const SystemCode As String = "EHS"
    End Class


End Namespace