Imports System.Text

Public Class RandomInputString

    Private Builder As StringBuilder = New StringBuilder
    Private Rand As Random = New Random
    Private Length As Integer


    Public Sub New()
        'Dim Builder As StringBuilder = New StringBuilder
        'Dim Rand As Random = New Random
        Length = 30
    End Sub

    Public Sub New(ByVal len As Integer)
        'Dim Builder As StringBuilder = New StringBuilder
        'Dim Rand As Random = New Random
        Length = len
    End Sub

    Public Function GetRandomIntegerString() As String
        Dim i As Integer
        Dim j As Integer

        For j = 0 To Length - 1
            i = Rand.Next(0, 10)
            Builder.Append(i)
        Next

        Return Builder.ToString
    End Function

    Public Function GetRandomString() As String
        'Dim Builder As StringBuilder = New StringBuilder
        'Dim Rand As Random = New Random

        Dim i As Integer
        Dim j As Integer

        For j = 0 To Length - 1
            i = Rand.Next(0, 2)
            If i = 0 Then
                Builder.Append(GetInteger())
            Else
                Builder.Append(GetChar())
            End If
        Next

        Return Builder.ToString

    End Function

    Private Function GetInteger() As Integer
        Dim i As Integer
        'Do Until i <> 0 Or i <> 1 Or i <> 8
        '    i = Rand.Next(0, 10)
        'Loop

        Dim checker As Boolean = False

        Do
            i = Rand.Next(0, 10)
            If i = 0 Or i = 1 Or i = 8 Then
                checker = False
            Else
                checker = True
            End If
        Loop Until checker = True

        Return i

    End Function

    Private Function GetChar() As Char
        Dim ch As Char

        Dim checker As Boolean = False

        'Do Until ch <> "B"c Or ch <> "I"c Or ch <> "O"c
        '    ch = Convert.ToChar(Convert.ToInt32(Rand.Next(0, 26) + 65))
        'Loop

        Do
            ch = Convert.ToChar(Convert.ToInt32(Rand.Next(0, 26) + 65))
            If ch = "B"c Or ch = "I"c Or ch = "O"c Then
                checker = False
            Else
                checker = True
            End If
        Loop Until checker = True

        Return ch

    End Function
End Class
