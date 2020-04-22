Namespace Component.EHSAccount
    Partial Public Class EHSAccountModel
        <Serializable()> Public Class EHSPersonalInformationDemographicModelCollection
            Inherits System.Collections.ArrayList

            Public Sub New()
            End Sub

            Public Overloads Sub Add(ByVal udtEHSPersonalInformationDemographicModel As EHSPersonalInformationDemographicModel)
                MyBase.Add(udtEHSPersonalInformationDemographicModel)
            End Sub

            Public Overloads Sub Remove(ByVal udtEHSPersonalInformationDemographicModel As EHSPersonalInformationDemographicModel)
                MyBase.Remove(udtEHSPersonalInformationDemographicModel)
            End Sub

            Default Public Overloads ReadOnly Property Item(ByVal intIndex As Integer) As EHSPersonalInformationDemographicModel
                Get
                    Return CType(MyBase.Item(intIndex), EHSPersonalInformationDemographicModel)
                End Get
            End Property

            Public Function Filter(ByVal strDocCode As String) As EHSPersonalInformationDemographicModel
                Dim udtResEHSPersonalInformationDemographicModel As EHSPersonalInformationDemographicModel = Nothing

                For Each udtEHSPersonalInformationDemographicModel As EHSPersonalInformationDemographicModel In Me
                    If udtEHSPersonalInformationDemographicModel.DocCode.Trim().ToUpper() = strDocCode.Trim().ToUpper() Then
                        udtResEHSPersonalInformationDemographicModel = udtEHSPersonalInformationDemographicModel
                        Exit For
                    End If
                Next
                Return udtResEHSPersonalInformationDemographicModel
            End Function
        End Class
    End Class
End Namespace