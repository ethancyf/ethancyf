Namespace Component.ServiceProviderT
    <Serializable()> Public Class ServiceProviderModelCollection
        Inherits System.Collections.SortedList

        Public Overloads Sub Add(ByVal udtServiceProviderModel As ServiceProviderModel)
            MyBase.Add(udtServiceProviderModel.EnrolRefNo, udtServiceProviderModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtServiceProviderModel As ServiceProviderModel)
            MyBase.Remove(udtServiceProviderModel)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strEnrolRefNo As String) As ServiceProviderModel
            Get
                Return CType(MyBase.Item(strEnrolRefNo), ServiceProviderModel)
            End Get
        End Property

        Public Sub New()
        End Sub

    End Class
End Namespace

