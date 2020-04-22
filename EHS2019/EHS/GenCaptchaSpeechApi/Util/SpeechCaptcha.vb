Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Drawing.Text
Imports System.Speech
Imports System.Speech.Synthesis.TtsEngine
Imports System.Speech.Synthesis
Imports System.Speech.AudioFormat
Imports System.Threading.Tasks
Imports System.Configuration
Imports NAudio.Wave
Imports NAudio.Lame

Public Class CultureLanguage
    Public Const TradChinese As String = "zh-tw"
    Public Const SimpChinese As String = "zh-cn"
    Public Const English As String = "en-us"
End Class

Public Class Voice
    Public Const English_Man_David As String = "Microsoft David Desktop"
    Public Const English_Woman_Zira As String = "Microsoft Zira Desktop"
    Public Const Chinese_Man_Danny As String = "Microsoft Danny Desktop"
    Public Const Chinese_Woman_Tracy As String = "Microsoft Tracy Desktop"
End Class

Public Class SpeechCaptcha

    Public Shared Sub Caller(speech As String, captcha As String, language As String)
        Dim t As Task = Task.Factory.StartNew(Sub()
                                                  SpeechCaptcha.GenerateSpeechFile(speech, captcha, language)
                                              End Sub)
        While Not t.IsCompleted
            Threading.Thread.Sleep(10)
        End While
        't.Wait(200)
    End Sub

    Public Shared Function CallerForStream(captcha As String, language As String) As MemoryStream
        Dim t As Task(Of MemoryStream) = Task(Of MemoryStream).Factory.StartNew(Function()
                                                                                    Dim cantonVoiceName As String = ConfigurationManager.AppSettings("CantonVoiceName").ToString()
                                                                                    Dim englishVoiceName As String = ConfigurationManager.AppSettings("EnglishVoiceName").ToString()
                                                                                    Dim VoiceName As String = String.Empty
                                                                                    Dim audioStream As MemoryStream = Nothing
                                                                                    Dim strLang As String = language

                                                                                    If cantonVoiceName <> Voice.Chinese_Man_Danny And cantonVoiceName <> Voice.Chinese_Woman_Tracy Then
                                                                                        strLang = CultureLanguage.English
                                                                                    End If

                                                                                    Try
                                                                                        Dim synth As SpeechSynthesizer = New SpeechSynthesizer()
                                                                                        Select Case language
                                                                                            Case CultureLanguage.TradChinese
                                                                                                VoiceName = cantonVoiceName
                                                                                            Case CultureLanguage.English
                                                                                                VoiceName = englishVoiceName
                                                                                            Case Else
                                                                                                VoiceName = englishVoiceName
                                                                                        End Select
                                                                                        If VoiceName <> String.Empty Then
                                                                                            synth.SelectVoice(VoiceName)
                                                                                            synth.Rate = -5
                                                                                            Dim ms = New MemoryStream()
                                                                                            synth.SetOutputToWaveStream(ms)
                                                                                            synth.Speak(FormatCaptcha(captcha, strLang))
                                                                                            audioStream = ConvertWavStreamToMp3Stream(ms)
                                                                                            synth.SetOutputToNull()
                                                                                        End If
                                                                                        synth.Dispose()
                                                                                    Catch ex As Exception
                                                                                        'LogHelper.WriteLineToday(ex.Message + "Source:" + ex.Source + "StackTrace:" + ex.StackTrace)
                                                                                        'LogHelper.WriteLineToday("Current language : " + Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() + ";English Voice Name : " + englishVoiceName + ";Cantonese Voice Name : " + cantonVoiceName)
                                                                                        Throw
                                                                                    End Try
                                                                                    Return audioStream
                                                                                End Function)
        While Not t.IsCompleted
            Threading.Thread.Sleep(10)
        End While
        Dim stream As MemoryStream = t.Result
        Return stream
    End Function


    'Add captcha speech
    Public Shared Sub GenerateSpeechFile(speech As String, captcha As String, language As String)
        Dim folder As String = ConfigurationManager.AppSettings("CaptchaAudioFolder").ToString()
        Dim cantonVoiceName As String = ConfigurationManager.AppSettings("CantonVoiceName").ToString()
        Dim englishVoiceName As String = ConfigurationManager.AppSettings("EnglishVoiceName").ToString()
        Dim VoiceName As String = String.Empty
        Dim FileName As String = String.Empty
        Try
            Dim synth As SpeechSynthesizer = New SpeechSynthesizer()
            Select Case language
                Case CultureLanguage.TradChinese
                    VoiceName = cantonVoiceName
                    FileName = folder + speech + CultureLanguage.TradChinese.Replace("-", "") + ".mp3"
                Case CultureLanguage.English
                    VoiceName = englishVoiceName
                    FileName = folder + speech + CultureLanguage.English.Replace("-", "") + ".mp3"
            End Select
            If VoiceName <> String.Empty And FileName <> String.Empty Then
                synth.SelectVoice(VoiceName)
                synth.Rate = -5
                Dim ms = New MemoryStream()
                synth.SetOutputToWaveStream(ms)
                synth.Speak(FormatCaptcha(captcha, language))
                ConvertWavStreamToMp3File(ms, FileName)
                synth.SetOutputToNull()
            End If
            synth.Dispose()
        Catch ex As Exception
            Throw
            'LogHelper.WriteLineToday(ex.Message + "Source:" + ex.Source + "StackTrace:" + ex.StackTrace)
            'LogHelper.WriteLineToday("Current language : " + Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() + ";English Voice Name : " + englishVoiceName + ";Cantonese Voice Name : " + cantonVoiceName)
        End Try
    End Sub
    Private Shared Function FormatCaptcha(ByVal captcha As String, ByVal lang As String) As String
        Dim newCaptcha As String = ""
        For i As Integer = 0 To captcha.Length - 1
            Dim charAt = captcha.Substring(i, 1)
            'If Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower() = CultureLanguage.TradChinese Then
            If lang = CultureLanguage.TradChinese Then
                Select Case charAt
                    Case "1"
                        charAt = "一"
                    Case "2"
                        charAt = "二"
                    Case "3"
                        charAt = "三"
                    Case "4"
                        charAt = "四"
                    Case "5"
                        charAt = "五"
                    Case "6"
                        charAt = "六"
                    Case "7"
                        charAt = "七"
                    Case "8"
                        charAt = "八"
                    Case "9"
                        charAt = "九"
                End Select
            End If
            newCaptcha = newCaptcha & charAt & " "
        Next

        Return newCaptcha
    End Function

    Public Shared Sub ConvertWavStreamToMp3File(ByRef ms As MemoryStream, ByVal savetofilename As String)
        ms.Seek(0, SeekOrigin.Begin)
        Using retMs = New MemoryStream()
            Using rdr = New WaveFileReader(ms)
                Using wtr = New LameMP3FileWriter(savetofilename, rdr.WaveFormat, LAMEPreset.VBR_90)
                    rdr.CopyTo(wtr)
                End Using
            End Using
        End Using
    End Sub


    Public Shared Function ConvertWavStreamToMp3Stream(ByRef wavStream As MemoryStream) As MemoryStream
        Dim mp3Stream As MemoryStream = New MemoryStream
        'Dim bytes As Byte()
        wavStream.Seek(0, SeekOrigin.Begin)
        Using rdr = New WaveFileReader(wavStream)
            Using wtr = New LameMP3FileWriter(mp3Stream, rdr.WaveFormat, LAMEPreset.VBR_90)
                rdr.CopyTo(wtr)
            End Using
        End Using
        mp3Stream.Position = 0
        Return mp3Stream
    End Function

End Class
