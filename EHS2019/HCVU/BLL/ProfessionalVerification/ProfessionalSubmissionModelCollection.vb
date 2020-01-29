<Serializable()> Public Class ProfessionalSubmissionModelCollection
    Inherits System.Collections.SortedList

#Region "Constuctor"

    Public Sub New()
    End Sub

#End Region

    Public Overloads Sub Add(ByVal udtProfessionalSubmissionModel As ProfessionalSubmissionModel)

        MyBase.Add(udtProfessionalSubmissionModel.DisplaySeq, udtProfessionalSubmissionModel)
    End Sub

    Public Overloads Sub Remove(ByVal udtProfessionalSubmissionModel As ProfessionalSubmissionModel)
        If Me.ContainsKey(udtProfessionalSubmissionModel.DisplaySeq) Then
            MyBase.Remove(udtProfessionalSubmissionModel.DisplaySeq)
        End If

    End Sub
End Class
