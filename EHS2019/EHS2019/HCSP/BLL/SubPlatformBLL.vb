Imports Common.Component
Imports System.Threading

Namespace BLL
    Public Class SubPlatformBLL

        Private ReadOnly Property SubPlatform() As EnumHCSPSubPlatform
            Get
                Dim strSubPlatform As String = ConfigurationManager.AppSettings("SubPlatform")

                If Not IsNothing(strSubPlatform) Then
                    Return [Enum].Parse(GetType(EnumHCSPSubPlatform), strSubPlatform)
                End If

                Return EnumHCSPSubPlatform.HK
            End Get
        End Property

        Public Function GetDateFormatLocale() As String
            Dim strSubPlatform As String = SubPlatform()

            Return GetDateFormatLocale(strSubPlatform)
        End Function

        Public Function GetDateFormatLocale(ByVal strSubPlatform As String) As String
            Dim strRes As String = String.Empty

            Select Case strSubPlatform
                Case EnumHCSPSubPlatform.HK
                    Select Case Thread.CurrentThread.CurrentUICulture.Name.ToLower()
                        Case CultureLanguage.English
                            strRes = CultureLanguage.English
                        Case CultureLanguage.TradChinese
                            strRes = CultureLanguage.TradChinese
                        Case CultureLanguage.SimpChinese
                            strRes = CultureLanguage.TradChinese
                        Case Else
                            strRes = CultureLanguage.English
                    End Select
                Case EnumHCSPSubPlatform.CN
                    Select Case Thread.CurrentThread.CurrentUICulture.Name.ToLower()
                        Case CultureLanguage.English
                            strRes = CultureLanguage.English
                        Case CultureLanguage.TradChinese
                            strRes = CultureLanguage.TradChinese
                        Case CultureLanguage.SimpChinese
                            strRes = CultureLanguage.SimpChinese
                        Case Else
                            strRes = CultureLanguage.English
                    End Select
                Case EnumHCSPSubPlatform.NA
                    Select Case Thread.CurrentThread.CurrentUICulture.Name.ToLower()
                        Case CultureLanguage.English
                            strRes = CultureLanguage.English
                        Case CultureLanguage.TradChinese
                            strRes = CultureLanguage.TradChinese
                        Case CultureLanguage.SimpChinese
                            strRes = CultureLanguage.TradChinese
                        Case Else
                            strRes = CultureLanguage.English
                    End Select
                Case Else
                    Select Case Thread.CurrentThread.CurrentUICulture.Name.ToLower()
                        Case CultureLanguage.English
                            strRes = CultureLanguage.English
                        Case CultureLanguage.TradChinese
                            strRes = CultureLanguage.TradChinese
                        Case CultureLanguage.SimpChinese
                            strRes = CultureLanguage.TradChinese
                        Case Else
                            strRes = CultureLanguage.English
                    End Select
            End Select

            Return strRes
        End Function

    End Class
End Namespace

