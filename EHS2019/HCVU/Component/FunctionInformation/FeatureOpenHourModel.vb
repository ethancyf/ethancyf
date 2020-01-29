Imports Common.ComFunction

<Serializable()> Public Class FeatureOpenHourModel

    Private _strFeatureCode As String
    Private _strFromTime As String
    Private _strToTime As String

    Public Sub New()

    End Sub

    Public Property FeatureCode() As String
        Get
            Return _strFeatureCode
        End Get
        Set(ByVal value As String)
            _strFeatureCode = value
        End Set
    End Property

    Public Property FromTime() As String
        Get
            Return _strFromTime
        End Get
        Set(ByVal value As String)
            _strFromTime = value
        End Set
    End Property

    Public Property ToTime() As String
        Get
            Return _strToTime
        End Get
        Set(ByVal value As String)
            _strToTime = value
        End Set
    End Property

    Public ReadOnly Property IsOpeningHour() As Boolean
        Get
            Dim udtGeneralFunction As New GeneralFunction
            Dim blnOpeningHour As Boolean = False
            Dim strStartTime As String = String.Empty
            Dim strEndTime As String = String.Empty
            Dim dtmNow As DateTime = udtGeneralFunction.GetSystemDateTime()

            strStartTime = FromTime
            strEndTime = ToTime

            If strStartTime = String.Empty And strEndTime = String.Empty Then
                Return False
            End If

            If strStartTime < strEndTime Then ' 04:00 - 17:00
                If dtmNow.ToString("HH:mm") >= strStartTime AndAlso _
                   dtmNow.ToString("HH:mm") <= strEndTime Then
                    Return True
                End If
            ElseIf strStartTime > strEndTime Then ' 17:00 - 04:00
                If dtmNow.ToString("HH:mm") >= strStartTime OrElse _
                   dtmNow.ToString("HH:mm") <= strEndTime Then
                    Return True
                End If
            ElseIf strStartTime = strEndTime Then ' 04:00 - 04:00
                Return True
            Else
                Return False
            End If

        End Get
    End Property

End Class

