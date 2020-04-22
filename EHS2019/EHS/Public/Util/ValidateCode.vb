Imports System
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Drawing.Text
Imports NAudio.Wave
Imports NAudio.Lame
Imports System.Speech.Synthesis

Public Class ValidateCode
    Private _myGraphics As Graphics
    Private _myFontFamily As Font
    Private _myText As String
    Private _myImage As Image
    Private _myColor As Color
    Private _myRandom As Random
    Public LINE_THICKNESS As Single = 4.0F
    Public OVAL_THICKNESS As Single = 2.0F
    Public FONT_FAMILY_NAME As String = "Arial"
    Public FONT_SIZE As Single = 38.0F
    Public FONT_STYLE As FontStyle = FontStyle.Bold
    Public DEBUG_MODE As Boolean = False

    Public Sub New(Optional ByVal text As String = "")
        _myFontFamily = New Font(FONT_FAMILY_NAME, FONT_SIZE, FONT_STYLE)
        _myRandom = New Random(DateTime.Now.Millisecond)
        _myImage = New Bitmap(1, 1)
        _myGraphics = Graphics.FromImage(_myImage)
        _myColor = GetRandomColor()

        If String.IsNullOrEmpty(text) Then
            _myText = ToRandomCase(GetRandomText(5))
        Else
            _myText = ToRandomCase(text)
        End If
    End Sub

    ReadOnly Property MaxLength As Integer
        Get
            Return 10
        End Get
    End Property

    ReadOnly Property MinLength As Integer
        Get
            Return 1
        End Get
    End Property

    Public Shared Function Str(ByVal Length As Integer) As String
        'Changed for Sprint 6
        'Revise the captcha character set to below capital letter only
        'ACDEFGHJKLNPQRTUVXYZ2346789()
        '(Removed: BIMOSW10)
        'Dim Pattern As Char() = New Char() {"0"c, "1"c, "2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c, "A"c, "B"c, "C"c, "D"c, "E"c, "F"c, "G"c, "H"c, "I"c, "J"c, "K"c, "L"c, "M"c, "N"c, "O"c, "P"c, "Q"c, "R"c, "S"c, "T"c, "U"c, "V"c, "W"c, "X"c, "Y"c, "Z"c}
        Dim Pattern As Char() = New Char() {"2"c, "3"c, "4"c, "5"c, "6"c, "7"c, "8"c, "9"c, "A"c, "C"c, "D"c, "E"c, "F"c, "G"c, "H"c, "J"c, "K"c, "L"c, "N"c, "P"c, "Q"c, "R"c, "T"c, "U"c, "V"c, "X"c, "Y"c, "Z"c}
        Dim result As String = ""
        Dim n As Integer = Pattern.Length

        Dim guid As Guid = Guid.NewGuid()
        Dim hashCode As Integer = guid.GetHashCode()
        'Dim random As System.Random = New Random(CInt(DateTime.Now.Ticks / 1024 / 1024 / 1024))
        Dim random As System.Random = New Random(hashCode)

        For i As Integer = 0 To Length - 1
            Dim rnd As Integer = random.[Next](0, n)
            result += Pattern(rnd)
        Next

        Return result
    End Function
    Public Shared Function CreateValidateCode(length As Integer) As String
        'CreateValidateCode = "ABCD"
        CreateValidateCode = Str(length)
    End Function

    Function CreateValidateGraphic(validateCode As String) As Byte()
        '        Bitmap image = new Bitmap((int)Math.Ceiling(validateCode.Length * 12.0), 22);
        'Graphics g = Graphics.FromImage(image);
        Dim image As Bitmap = New Bitmap(Convert.ToInt32(Math.Ceiling(validateCode.Length * 30.0)), 45)

        Dim g As Graphics = Graphics.FromImage(image)
        g.Clear(Color.White)
        Try
            'Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
            '    LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
            '     Color.Blue, Color.DarkRed, 1.2f, true);
            '    g.DrawString(validateCode, font, brush, 3, 2);
            Dim font As Font = New Font("Arial", 24, (FontStyle.Bold Or FontStyle.Italic))
            'Dim brush As LinearGradientBrush = New LinearGradientBrush(New Rectangle(0, 0, image.Width, image.Height), Color.Black, Color.White, 1.2F, True)
            Dim brush As SolidBrush = New SolidBrush(Color.Blue)
            g.DrawString(validateCode, font, brush, 10, 5)
            'MemoryStream stream = new MemoryStream();

            image = TwistImage(image, True, 3, 20)

            Dim stream As MemoryStream = New MemoryStream()
            image.Save(stream, ImageFormat.Jpeg)
            CreateValidateGraphic = stream.ToArray()
            Return CreateValidateGraphic
        Catch ex As Exception
            g.Dispose()
            image.Dispose()
            Return Nothing
        End Try

    End Function

    Public Function TwistImage(ByVal srcBmp As Bitmap, ByVal bXDir As Boolean, ByVal dMultValue As Double, ByVal dPhase As Double) As System.Drawing.Bitmap
        Dim PI As Double = 6.2831853071795862
        Dim destBmp As Bitmap = New Bitmap(srcBmp.Width, srcBmp.Height)
        Dim graph As Graphics = Graphics.FromImage(destBmp)
        graph.FillRectangle(New SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height)
        graph.Dispose()
        Dim dBaseAxisLen As Double = If(bXDir, CDbl(destBmp.Height), CDbl(destBmp.Width))

        For i As Integer = 0 To destBmp.Width - 1

            For j As Integer = 0 To destBmp.Height - 1
                Dim dx As Double = 0
                dx = If(bXDir, (PI * CDbl(j)) / dBaseAxisLen, (PI * CDbl(i)) / dBaseAxisLen)
                dx += dPhase
                Dim dy As Double = Math.Sin(dx)
                Dim nOldX As Integer = 0, nOldY As Integer = 0
                nOldX = If(bXDir, i + CInt((dy * dMultValue)), i)
                nOldY = If(bXDir, j, j + CInt((dy * dMultValue)))
                Dim color As Color = srcBmp.GetPixel(i, j)

                If nOldX >= 0 AndAlso nOldX < destBmp.Width AndAlso nOldY >= 0 AndAlso nOldY < destBmp.Height Then
                    destBmp.SetPixel(nOldX, nOldY, color)
                End If
            Next
        Next

        srcBmp.Dispose()
        Return destBmp
    End Function


    Public Function Generate() As Byte()       ' Bitmap()
        Dim size As SizeF = _myGraphics.MeasureString(_myText, _myFontFamily)
        _myImage = New Bitmap(CInt((size.Width * 1.5)), CInt(size.Height))
        _myGraphics = Graphics.FromImage(_myImage)
        _myGraphics.Clear(Color.White)
        _myGraphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias
        AddRandomLines(_myGraphics)
        GenerateText()
        _myGraphics.Save()
        Dim stream As MemoryStream = New MemoryStream()
        _myImage.Save(stream, ImageFormat.Jpeg)
        Generate = stream.ToArray()
        Return Generate
        'Return _myImage
    End Function

    Private Function GetRandomColor() As Color
        Return Color.FromArgb(_myRandom.[Next](150), _myRandom.[Next](150), _myRandom.[Next](150))
    End Function

    Private Sub AddRandomLines(ByVal graphics As Graphics)
        'For i = 0 To _myImage.Width / 50 - 1
        '    Dim point1 As Point = GetRandomPoint()
        '    Dim point2 As Point = New Point(_myRandom.[Next](point1.X - 100, point1.X + 100), _myRandom.[Next](point1.Y - 100, point1.Y + 100))
        '    graphics.DrawLine(New Pen(_myColor, LINE_THICKNESS), point1, point2)
        'Next
        For i = 1 To _myImage.Width / 100 - 1
            Dim point1 As Point = GetRandomPoint()
            Dim point2 As Point = New Point(_myRandom.[Next](point1.X - 100, point1.X + 100), _myRandom.[Next](point1.Y - 100, point1.Y + 100))
            graphics.DrawLine(New Pen(_myColor, LINE_THICKNESS), point1, point2)
        Next
    End Sub

    Private Function GetRandomPoint(Optional ByVal marginW As Integer = 0, Optional ByVal marginH As Integer = 0) As Point
        Dim randomX As Integer = _myRandom.[Next](marginW, _myImage.Width + marginW)
        Dim randomY As Integer = _myRandom.[Next](marginH, _myImage.Height + marginH)
        Return New Point(randomX, randomY)
    End Function

    Private Function ToRandomCase(ByVal text As String) As String
        Dim result = New StringBuilder()

        For i = 0 To text.Length - 1
            'Dim random As Integer = _myRandom.[Next](0, 2)

            'If random > 0 Then
            '    result.Append(text(i).ToString().ToLower())
            'Else
            '    result.Append(text(i).ToString().ToUpper())
            'End If
            result.Append(text(i).ToString().ToUpper())
        Next

        Return result.ToString()
    End Function

    Private Function GetRandomText(ByVal v As Integer) As String
        Dim alphabet As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789"
        Dim result As StringBuilder = New StringBuilder()

        For i = 0 To v - 1
            result.Append(alphabet(_myRandom.[Next](0, alphabet.Length - 1)))
        Next

        Return result.ToString()
    End Function

    Private Sub GenerateText(Optional ByVal debug As Boolean = False)
        Dim lastLetterLeft As Integer = 5
        Dim Letters As List(Of Image) = GetTextAsImageList(_myText)

        For i = 0 To Letters.Count - 1
            Dim Letter As Image = Letters(i)

            If Not DEBUG_MODE Then
                'Letter = GetRotatedLetter(Letter, _myRandom.[Next](-60, 60))
                Letter = GetRotatedLetter(Letter, _myRandom.[Next](-20, 20))
            Else
                Letter = GetDebugLetter(Letter)
            End If

            _myGraphics.DrawImage(Letter, New Point(lastLetterLeft, _myRandom.[Next](-10, 10)))
            lastLetterLeft += Letter.Width
        Next
    End Sub

    Private Function GetTextAsImageList(ByVal text As String) As List(Of Image)
        Dim Letters As List(Of Image) = New List(Of Image)()

        For i = 0 To _myText.Length - 1
            Dim LetterSize As Size = _myGraphics.MeasureString(text(i).ToString(), _myFontFamily).ToSize()
            Dim Letter As Image = New Bitmap(LetterSize.Width, LetterSize.Height)
            Dim LetterGraph As Graphics = Graphics.FromImage(Letter)
            LetterGraph.TextRenderingHint = TextRenderingHint.AntiAlias
            LetterGraph.DrawString(text(i).ToString(), _myFontFamily, New SolidBrush(_myColor), 0, 0)
            Letters.Add(Letter)
        Next

        Return Letters
    End Function

    Private Function GetRotatedLetter(ByVal image As Image, ByVal angle As Integer) As Image
        Dim rotatedImage As Image = New Bitmap(image.Width, image.Height)
        Dim graphics As Graphics = Graphics.FromImage(rotatedImage)
        graphics.TranslateTransform(CSng(image.Width) / 2, CSng(image.Height) / 2)
        graphics.RotateTransform(angle)
        graphics.TranslateTransform(-CSng(image.Width) / 2, -CSng(image.Height) / 2)
        graphics.DrawImage(image, 0, 0)
        Return rotatedImage
    End Function

    Private Function GetDebugLetter(ByVal image As Image) As Image
        Dim debugImage As Image = New Bitmap(image)
        Dim graphics As Graphics = Graphics.FromImage(debugImage)
        graphics.DrawRectangle(New Pen(Color.Red, 1), New Rectangle(0, 0, image.Width - 1, image.Height - 1))
        graphics.DrawRectangle(New Pen(Color.Red), CSng(image.Width) / 2, CSng(image.Height) / 2, 1, 1)
        Return debugImage
    End Function

    Public Shared Sub GenerateSpeech(speech As String, captcha As String)
        Dim folder As String = ConfigurationManager.AppSettings("CaptchaAudioFolder").ToString()
        Dim cantonVoiceName As String = ConfigurationManager.AppSettings("CantonVoiceName").ToString()
        Dim englishVoiceName As String = ConfigurationManager.AppSettings("EnglishVoiceName").ToString()
        Dim VoiceName As String = String.Empty
        Dim FileName As String = String.Empty
        Try
            Dim synth As SpeechSynthesizer = New SpeechSynthesizer()
            Select Case Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower()
                Case Common.Component.CultureLanguage.TradChinese
                    VoiceName = cantonVoiceName
                    FileName = folder + speech + Common.Component.CultureLanguage.TradChinese.Replace("-", "") + ".mp3"
                Case Common.Component.CultureLanguage.English
                    VoiceName = englishVoiceName
                    FileName = folder + speech + Common.Component.CultureLanguage.English.Replace("-", "") + ".mp3"
            End Select
            If VoiceName <> String.Empty And FileName <> String.Empty Then
                synth.SelectVoice(VoiceName)
                synth.Rate = -5
                Dim ms = New MemoryStream()
                synth.SetOutputToWaveStream(ms)
                synth.Speak(FormatCaptcha(captcha))
                ConvertWavStreamToMp3File(ms, FileName)
                synth.SetOutputToNull()
            End If
            synth.Dispose()
        Catch ex As Exception
            'LogHelper.WriteLineToday(ex.Message + "Source:" + ex.Source + "StackTrace:" + ex.StackTrace)
            'LogHelper.WriteLineToday(Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() + englishVoiceName + cantonVoiceName)
            Throw
        End Try
    End Sub
    Private Shared Function FormatCaptcha(ByVal captcha As String) As String
        Dim newCaptcha As String = ""

        For i As Integer = 0 To captcha.Length - 1
            Dim charAt = captcha.Substring(i, 1)
            If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower() = Common.Component.CultureLanguage.TradChinese Then
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
End Class
