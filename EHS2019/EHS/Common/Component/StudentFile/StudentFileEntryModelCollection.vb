Namespace Component.StudentFile

    <Serializable()> Public Class StudentFileEntryModelCollection
        Inherits ArrayList

#Region "Constructors"

        Public Sub New()
            MyBase.New()
        End Sub

#End Region

#Region "Methods"

        Public Overloads Sub Add(ByVal udtStudentFileEntry As StudentFileEntryModel)
            MyBase.Add(udtStudentFileEntry)
        End Sub

        Public Overloads Sub Remove(ByVal udtStudentFileEntry As StudentFileEntryModel)
            MyBase.Remove(udtStudentFileEntry)
        End Sub

#End Region

#Region "Properties"

        Default Public Overloads ReadOnly Property Item(intIndex As Integer) As StudentFileEntryModel
            Get
                Return CType(MyBase.Item(intIndex), StudentFileEntryModel)
            End Get
        End Property

#End Region

    End Class

End Namespace
