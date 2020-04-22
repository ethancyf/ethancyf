Namespace Component.Scheme
    <Serializable()> Public Class SubsidizeGroupEFormModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtSubsidizeGroupEFormModel As SubsidizeGroupEFormModel)
            MyBase.Add(udtSubsidizeGroupEFormModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSubsidizeGroupEFormModel As SubsidizeGroupEFormModel)
            MyBase.Remove(udtSubsidizeGroupEFormModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As SubsidizeGroupEFormModel
            Get
                Return CType(MyBase.Item(intIndex), SubsidizeGroupEFormModel)
            End Get
        End Property

        Public Function Filter(ByVal strSchemeCode As String, ByVal strSubsidizeCode As String) As SubsidizeGroupEFormModel
            Dim udtResSubsidizeGroupEFormModel As SubsidizeGroupEFormModel = Nothing
            For Each udtSubsidizeGroupEFormModel As SubsidizeGroupEFormModel In Me
                If udtSubsidizeGroupEFormModel.SchemeCode.Trim.Equals(strSchemeCode.Trim) AndAlso _
                   udtSubsidizeGroupEFormModel.SubsidizeCode.Trim.Equals(strSubsidizeCode.Trim) Then

                    udtResSubsidizeGroupEFormModel = udtSubsidizeGroupEFormModel
                End If
            Next

            Return udtResSubsidizeGroupEFormModel
        End Function

        Public Function Filter(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal dtmCurrentTime As DateTime) As SubsidizeGroupEFormModelCollection
            Dim udtResSubsidizeGroupEFormModelCollection As SubsidizeGroupEFormModelCollection = New SubsidizeGroupEFormModelCollection()
            Dim udtResSubsidizeGroupEFormModel As SubsidizeGroupEFormModel = Nothing

            For Each udtSubsidizeGroupEFormModel As SubsidizeGroupEFormModel In Me
                If udtSubsidizeGroupEFormModel.SchemeCode.Trim.Equals(strSchemeCode.Trim) AndAlso _
                   udtSubsidizeGroupEFormModel.SchemeSeq = intSchemeSeq Then

                    If DateTime.Compare(dtmCurrentTime, udtSubsidizeGroupEFormModel.EnrolPeriodFrom) >= 0 AndAlso DateTime.Compare(dtmCurrentTime, udtSubsidizeGroupEFormModel.EnrolPeriodTo) < 0 Then
                        udtResSubsidizeGroupEFormModel = New SubsidizeGroupEFormModel(udtSubsidizeGroupEFormModel)
                        udtResSubsidizeGroupEFormModelCollection.Add(udtResSubsidizeGroupEFormModel)
                    End If

                End If

            Next
            Return udtResSubsidizeGroupEFormModelCollection
        End Function

    End Class

End Namespace
