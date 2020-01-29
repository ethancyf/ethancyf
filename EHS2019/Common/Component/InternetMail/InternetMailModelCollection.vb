Namespace Component.InternetMail
    <Serializable()> Public Class InternetMailModelCollection
        Inherits System.Collections.SortedList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtInternetMailModel As InternetMailModel)
            MyBase.Add(udtInternetMailModel.GetKeyValue(), udtInternetMailModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtInternetMailModel As InternetMailModel)
            MyBase.Remove(udtInternetMailModel.GetKeyValue())
        End Sub

    End Class
End Namespace