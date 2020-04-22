<Serializable()> Public Class ProfessionalVerificationModelCollection
    Inherits System.Collections.SortedList

#Region "Constuctor"

    Public Sub New()
    End Sub

#End Region

    Public Overloads Sub Add(ByVal udtProfessionalVerificationModel As ProfessionalVerificationModel)
        MyBase.Add(udtProfessionalVerificationModel.EnrolmentRefNo + "-" + udtProfessionalVerificationModel.ProfessionalSeq.ToString(), udtProfessionalVerificationModel)
    End Sub

    Public Overloads Sub Remove(ByVal udtProfessionalVerificationModel As ProfessionalVerificationModel)
        If Me.ContainsKey(udtProfessionalVerificationModel.EnrolmentRefNo + "-" + udtProfessionalVerificationModel.ProfessionalSeq.ToString()) Then
            MyBase.Remove(udtProfessionalVerificationModel.EnrolmentRefNo + "-" + udtProfessionalVerificationModel.ProfessionalSeq.ToString())
        End If
    End Sub
End Class
