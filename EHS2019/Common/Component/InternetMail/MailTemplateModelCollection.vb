Namespace Component.InternetMail
    <Serializable()> Public Class MailTemplateModelCollection
        Inherits System.Collections.SortedList

        Public Sub New()
        End Sub

        Public Overloads Sub Add(ByVal udtMailTemplateModel As MailTemplateModel)
            MyBase.Add(udtMailTemplateModel.GetKeyValue(), udtMailTemplateModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtMailTemplateModel As MailTemplateModel)
            MyBase.Remove(udtMailTemplateModel.GetKeyValue())
        End Sub
    End Class
End Namespace