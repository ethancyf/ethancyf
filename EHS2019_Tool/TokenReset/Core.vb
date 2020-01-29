Imports System.Configuration

Module Core

    Sub Main()
        Dim strTokenSettingFilePath As String = ConfigurationManager.AppSettings("TokenSettingFilePath")
        Dim a As New TokenSettingCollection(strTokenSettingFilePath)
        TokenReset.Reset(a)
    End Sub

End Module
