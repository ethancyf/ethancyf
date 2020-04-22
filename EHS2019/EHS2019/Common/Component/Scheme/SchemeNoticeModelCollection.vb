Namespace Component.Scheme
    <Serializable()> Public Class SchemeNoticeModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSchemeNotice As SchemeNoticeModel)
            MyBase.Add(udtSchemeNotice)
        End Sub

        Public Overloads Sub Remove(ByVal udtSchemeNotice As SchemeNoticeModel)
            MyBase.Remove(udtSchemeNotice)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SchemeNoticeModel
            Get
                Return CType(MyBase.Item(intIndex), SchemeNoticeModel)
            End Get
        End Property

        Public Function FilterBySchemeCodeWithinDisplayPeriod(ByVal strSchemeCode As String) As SchemeNoticeModel
            Dim udtSchemeNoticeModelReturn As SchemeNoticeModel = Nothing
            Dim intPreviousNoticeSeq As Integer = 0
            Dim dtmCurrentDateTime As DateTime = (New Common.ComFunction.GeneralFunction).GetSystemDateTime

            For Each udtSchemeNoticeModel As SchemeNoticeModel In Me
                If udtSchemeNoticeModel.SchemeCode.Trim.ToUpper = strSchemeCode.Trim.ToUpper Then

                    'within display period
                    If udtSchemeNoticeModel.WithinDisplayPeriod(dtmCurrentDateTime) = True Then
                        'Take the one with max notice seq
                        If udtSchemeNoticeModel.NoticeSeq > intPreviousNoticeSeq Then

                            udtSchemeNoticeModelReturn = udtSchemeNoticeModel

                            intPreviousNoticeSeq = udtSchemeNoticeModel.NoticeSeq
                        End If
                    End If
                End If
            Next

            Return udtSchemeNoticeModelReturn

        End Function


    End Class
End Namespace