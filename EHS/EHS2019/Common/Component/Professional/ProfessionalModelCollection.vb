Namespace Component.Professional
    <Serializable()> Public Class ProfessionalModelCollection
        Inherits System.Collections.SortedList
        Public Overloads Sub Add(ByVal udtProfessionalModel As ProfessionalModel)
            MyBase.Add(udtProfessionalModel.ProfessionalSeq, udtProfessionalModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtProfessionalModel As ProfessionalModel)
            MyBase.Remove(udtProfessionalModel.ProfessionalSeq)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strProfessionalSeq As String) As ProfessionalModel
            Get
                Return CType(MyBase.Item(strProfessionalSeq), ProfessionalModel)
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Function GetProfessionalSeq(ByVal strServiceCategoryCode As String, ByVal strRegistrationCode As String) As Integer
            Dim i As Nullable(Of Integer)

            Dim intLastValue As Integer = 0

            Dim udtProfessionalModel As ProfessionalModel
            For Each udtProfessionalModel In MyBase.Values
                If udtProfessionalModel.ServiceCategoryCode = strServiceCategoryCode And udtProfessionalModel.RegistrationCode = strRegistrationCode And _
                    (udtProfessionalModel.RecordStatus = ProfessionalStagingStatus.Active Or udtProfessionalModel.RecordStatus.Equals(String.Empty) Or _
                     udtProfessionalModel.RecordStatus = ProfessionalStagingStatus.Existing) Then
                    i = udtProfessionalModel.ProfessionalSeq
                End If
                intLastValue = udtProfessionalModel.ProfessionalSeq
            Next

            If Not i.HasValue Then
                'i = MyBase.Count + 1
                i = intLastValue + 1
            End If

            Return i
        End Function

    End Class
End Namespace

