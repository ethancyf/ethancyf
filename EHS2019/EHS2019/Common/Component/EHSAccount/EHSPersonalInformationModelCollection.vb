Namespace Component.EHSAccount
    Partial Public Class EHSAccountModel
        <Serializable()> Public Class EHSPersonalInformationModelCollection
            Inherits System.Collections.ArrayList

            Public Sub New()
            End Sub

            Public Overloads Sub Add(ByVal udtEHSPersonalInformationModel As EHSPersonalInformationModel)
                MyBase.Add(udtEHSPersonalInformationModel)
            End Sub

            Public Overloads Sub Remove(ByVal udtEHSPersonalInformationModel As EHSPersonalInformationModel)
                MyBase.Remove(udtEHSPersonalInformationModel)
            End Sub

            Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EHSPersonalInformationModel
                Get
                    Return CType(MyBase.Item(intIndex), EHSPersonalInformationModel)
                End Get
            End Property

            Public Function Filter(ByVal strDocCode As String) As EHSPersonalInformationModel
                Dim udtResEHSPersonalInformationModel As EHSPersonalInformationModel = Nothing

                For Each udtEHSPersonalInformationModel As EHSPersonalInformationModel In Me
                    If udtEHSPersonalInformationModel.DocCode.Trim().ToUpper() = strDocCode.Trim().ToUpper() Then
                        udtResEHSPersonalInformationModel = udtEHSPersonalInformationModel
                        Exit For
                    End If
                Next
                Return udtResEHSPersonalInformationModel
            End Function
        End Class
    End Class
End Namespace