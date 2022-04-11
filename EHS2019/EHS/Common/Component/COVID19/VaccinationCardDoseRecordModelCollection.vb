Imports Microsoft.VisualBasic

Namespace Component.COVID19
    <Serializable()> Public Class VaccinationCardDoseRecordModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtVaccinationCardDoseRecordModel As VaccinationCardDoseRecordModel)
            MyBase.Add(udtVaccinationCardDoseRecordModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtVaccinationCardDoseRecordModel As VaccinationCardDoseRecordModel)
            MyBase.Remove(udtVaccinationCardDoseRecordModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As VaccinationCardDoseRecordModel
            Get
                Return CType(MyBase.Item(intIndex), VaccinationCardDoseRecordModel)
            End Get
        End Property

        Public Function FilterFindNearestRecord() As VaccinationCardDoseRecordModel

            Dim udtResDoseRecordModel As VaccinationCardDoseRecordModel = Nothing

            For Each udtDoseRecordModel As VaccinationCardDoseRecordModel In Me
                If udtResDoseRecordModel Is Nothing Then
                    udtResDoseRecordModel = udtDoseRecordModel
                Else
                    If udtResDoseRecordModel.InjectionDate <= udtDoseRecordModel.InjectionDate Then
                        udtResDoseRecordModel = udtDoseRecordModel
                    End If
                End If
            Next

            Return udtResDoseRecordModel

        End Function
    End Class
End Namespace

