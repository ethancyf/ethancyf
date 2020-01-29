Imports System.Runtime.Serialization
Namespace Component.RedirectParameter


    ' CRE11-024-02 HCVS Pilot Extension Part 2 [Start]
    ' -----------------------------------------------------------------------------------------
    <Serializable()> _
    Public Class SearchCriteriaCollection
        Inherits Dictionary(Of String, Object)

        Public Sub New()
            MyBase.New()
        End Sub

        Protected Sub New(ByVal info As SerializationInfo, ByVal context As StreamingContext)
            MyBase.New(info, context)
        End Sub

        Public Function AddDatetime(ByVal strCriteria As String, ByVal dtm As DateTime) As Nullable(Of DateTime)
            If Me.ContainsKey(strCriteria) Then Me.Remove(strCriteria)

            Me.Add(strCriteria, dtm)
        End Function

        Public Function GetDatetime(ByVal strCriteria As String) As Nullable(Of DateTime)
            If Not Me.ContainsKey(strCriteria) Then Return Nothing

            Return CType(Me.Item(strCriteria), DateTime)
        End Function

    End Class

    ' CRE11-024-02 HCVS Pilot Extension Part 2 [End]
End Namespace