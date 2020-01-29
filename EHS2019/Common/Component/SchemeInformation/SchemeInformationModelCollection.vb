Imports Common.Component.Scheme
Imports Common.Component.Scheme.SchemeClaimModel

Namespace Component.SchemeInformation
    <Serializable()> Public Class SchemeInformationModelCollection
        Inherits System.Collections.SortedList

        Public Overloads Sub Add(ByVal udtSchemeInformationModel As SchemeInformationModel)
            MyBase.Add(CStr(udtSchemeInformationModel.SchemeDisplaySeq).PadLeft(5, "0") + "-" + udtSchemeInformationModel.SchemeCode.Trim, udtSchemeInformationModel)
        End Sub

        Public Overloads Sub Remove(ByVal udtSchemeInformationModel As SchemeInformationModel)
            MyBase.Remove(CStr(udtSchemeInformationModel.SchemeDisplaySeq).PadLeft(5, "0") + "-" + udtSchemeInformationModel.SchemeCode.Trim)
        End Sub

        Default Public Overloads ReadOnly Property Item(ByVal strSchemeCode As String, ByVal intSchemeDisplaySeq As Integer) As SchemeInformationModel
            Get
                Return CType(MyBase.Item(CStr(intSchemeDisplaySeq).PadLeft(5, "0") + "-" + strSchemeCode), SchemeInformationModel)
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Function FilterBySchemeCode(ByVal strSchemeCode As String) As SchemeInformationModelCollection
            Dim udtSchemeList As SchemeInformationModelCollection = New SchemeInformationModelCollection
            Dim udtScheme As SchemeInformationModel
            For Each udtScheme In Me.Values
                If udtScheme.SchemeCode.Trim.Equals(strSchemeCode.Trim) Then
                    udtSchemeList.Add(udtScheme)
                End If
            Next

            Return udtSchemeList
        End Function

        Public Function Filter(ByVal strSchemeCode As String) As SchemeInformationModel
            For Each udtScheme As SchemeInformationModel In Me.Values
                If udtScheme.SchemeCode.Trim = strSchemeCode.Trim Then Return udtScheme
            Next

            Return Nothing
        End Function

        ' CRE13-019-02 Extend HCVS to China [Start][Lawrence]
        Public Function FilterByHCSPSubPlatform(ByVal enumSubPlatform As EnumHCSPSubPlatform) As SchemeInformationModelCollection
            Dim udtSchemeClaimBLL As New SchemeClaimBLL
            Dim udtSchemeList As New SchemeInformationModelCollection
            Dim udtSchemeClaimList As SchemeClaimModelCollection = udtSchemeClaimBLL.getAllDistinctSchemeClaim

            For Each udtScheme As SchemeInformationModel In Me.Values
                Dim strSchemeCode As String = udtSchemeClaimBLL.ConvertSchemeClaimCodeFromSchemeEnrol(udtScheme.SchemeCode)
                Dim udtSchemeClaim As SchemeClaimModel = udtSchemeClaimList.Filter(strSchemeCode)

                If IsNothing(udtSchemeClaim) Then Continue For

                If udtSchemeClaim.AvailableHCSPSubPlatform = EnumAvailableHCSPSubPlatform.ALL _
                        OrElse udtSchemeClaim.AvailableHCSPSubPlatform.ToString = enumSubPlatform.ToString Then
                    udtSchemeList.Add(New SchemeInformationModel(udtScheme))
                End If

            Next

            Return udtSchemeList

        End Function
        ' CRE13-019-02 Extend HCVS to China [End][Lawrence]

    End Class
End Namespace


