Namespace BLL

    <Serializable()> Public Class StudentModelCollection
        Inherits System.Collections.ArrayList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtStudentModel As StudentModel)
            MyBase.Add(udtStudentModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtStudentModel As StudentModel)
            MyBase.Remove(udtStudentModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As StudentModel
            Get
                Return CType(MyBase.Item(intIndex), StudentModel)
            End Get
        End Property

    End Class

End Namespace