Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace Component.DHTransaction
    <Serializable()> _
    Public Class DHVaccineL2IdenModel

#Region "Private Members"
        Private _strVaccineDesc As String
#End Region

#Region "Properties"
        Public ReadOnly Property VaccineDesc() As String
            Get
                Return _strVaccineDesc
            End Get
        End Property
#End Region

#Region "Constructor"
        Public Sub New(ByVal strVaccineDesc As String)
            _strVaccineDesc = strVaccineDesc
        End Sub
#End Region

    End Class
End Namespace