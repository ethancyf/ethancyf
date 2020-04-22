Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace Component.DHTransaction
    <Serializable()> _
    Public Class DHVaccineL3IdenModel

#Region "Private Members"
        Private _strHkRegNum As String
        Private _strVaccineProdName As String

#End Region

#Region "Properties"
        Public ReadOnly Property HkRegNum() As String
            Get
                Return _strHkRegNum
            End Get
        End Property

        Public ReadOnly Property VaccineProdName() As String
            Get
                Return _strVaccineProdName
            End Get
        End Property

#End Region

#Region "Constructors"
        Public Sub New(ByVal strHkRegNum As String, ByVal strVaccineProdName As String)
            _strHkRegNum = strHkRegNum
            _strVaccineProdName = strVaccineProdName
        End Sub

#End Region

    End Class
End Namespace

