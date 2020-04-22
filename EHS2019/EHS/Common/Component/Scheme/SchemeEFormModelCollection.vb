Namespace Component.Scheme
    <Serializable()> Public Class SchemeEFormModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSchemeEFormModel As SchemeEFormModel)
            MyBase.Add(udtSchemeEFormModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSchemeEFormModel As SchemeEFormModel)
            MyBase.Remove(udtSchemeEFormModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SchemeEFormModel
            Get
                Return CType(MyBase.Item(intIndex), SchemeEFormModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String) As SchemeEFormModel
            Dim udtResSchemeEFormModel As SchemeEFormModel = Nothing
            For Each udtSchemeEFormModel As SchemeEFormModel In Me
                If udtSchemeEFormModel.SchemeCode.Trim.Equals(strSchemeCode) Then
                    udtResSchemeEFormModel = udtSchemeEFormModel
                End If
            Next

            Return udtResSchemeEFormModel
        End Function

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer) As SchemeEFormModel
            Dim udtResSchemeEFormModel As SchemeEFormModel = Nothing
            For Each udtSchemeEFormModel As SchemeEFormModel In Me
                If udtSchemeEFormModel.SchemeCode.Trim.Equals(strSchemeCode) AndAlso udtSchemeEFormModel.SchemeSeq = intSchemeSeq Then
                    udtResSchemeEFormModel = udtSchemeEFormModel
                End If
            Next

            Return udtResSchemeEFormModel
        End Function

        Public Function Filter(ByVal dtmCurrentTime As DateTime) As SchemeEFormModelCollection
            Dim udtResSchemeEFormModelCollection As SchemeEFormModelCollection = New SchemeEFormModelCollection()
            Dim udtResSchemeEFormModel As SchemeEFormModel = Nothing

            For Each udtSchemeEForm As SchemeEFormModel In Me
                If DateTime.Compare(dtmCurrentTime, udtSchemeEForm.EnrolPeriodFrom) >= 0 AndAlso DateTime.Compare(dtmCurrentTime, udtSchemeEForm.EnrolPeriodTo) < 0 Then
                    udtResSchemeEFormModel = New SchemeEFormModel(udtSchemeEForm)
                    udtResSchemeEFormModelCollection.Add(udtResSchemeEFormModel)
                End If
            Next
            Return udtResSchemeEFormModelCollection
        End Function

        Public Function Filter(ByVal strSchemeCode As String, ByVal dtmCurrentTime As DateTime) As SchemeEFormModel
            Return Me.Filter(dtmCurrentTime).Filter(strSchemeCode)
        End Function

    End Class
End Namespace

