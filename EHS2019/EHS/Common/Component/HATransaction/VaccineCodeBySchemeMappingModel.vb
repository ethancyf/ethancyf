Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace Component.HATransaction
    <Serializable()> _
    Public Class VaccineCodeBySchemeMappingModel

        Private _strSchemeCode As String
        Public ReadOnly Property SchemeCode() As String
            Get
                Return _strSchemeCode
            End Get
        End Property

        Private _strVaccineCodeSource As String
        Public ReadOnly Property VaccineCodeSource() As String
            Get
                Return _strVaccineCodeSource
            End Get
        End Property

        Private _strVaccineCodeTarget As String
        Public ReadOnly Property VaccineCodeTarget() As String
            Get
                Return _strVaccineCodeTarget
            End Get
        End Property

        Public Sub New(ByVal strSchemeCode As String, _
                        ByVal strVaccineCodeSource As String, ByVal strVaccineCodeTarget As String)
            _strSchemeCode = strSchemeCode.Trim()
            _strVaccineCodeSource = strVaccineCodeSource.Trim()
            _strVaccineCodeTarget = strVaccineCodeTarget.Trim()
        End Sub
    End Class
End Namespace