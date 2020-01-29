<Serializable()> Public Class ParameteMaintenanceModelCollection
    Inherits System.Collections.SortedList

    Public Overloads Sub Add(ByVal udtParameteMaintenanceModel As ParameteMaintenanceModel)
        MyBase.Add(String.Format("{0}-{1}", udtParameteMaintenanceModel.ParameterID, udtParameteMaintenanceModel.Category), udtParameteMaintenanceModel)
    End Sub

    Public Overloads Sub Remove(ByVal udtParameteMaintenanceModel As ParameteMaintenanceModel)
        MyBase.Remove(udtParameteMaintenanceModel)
    End Sub

    Default Public Overloads ReadOnly Property Item(ByVal strKeyIDCategory As String) As ParameteMaintenanceModel
        Get
            Return CType(MyBase.Item(strKeyIDCategory), ParameteMaintenanceModel)
        End Get
    End Property

    Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As ParameteMaintenanceModel
        Get
            Return CType(MyBase.GetByIndex(intIndex), ParameteMaintenanceModel)
        End Get
    End Property

    Public Sub New()
    End Sub

End Class
