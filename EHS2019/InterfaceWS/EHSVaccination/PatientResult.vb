Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace EHSVaccination

    Public Class PatientResult

#Region "Private Member"
        Private _intPatientID As Integer
        Private _intPatientResultCode As Integer
        Private _intVaccineResultCode As Integer
        Private _dtResult As New DataTable
#End Region

#Region "Property"
        Public ReadOnly Property PatientID() As Integer
            Get
                Return _intPatientID
            End Get
        End Property

        Public ReadOnly Property PatientResultCode() As Integer
            Get
                Return _intPatientResultCode
            End Get
        End Property

        Public ReadOnly Property VaccineResultCode() As Integer
            Get
                Return _intVaccineResultCode
            End Get
        End Property

        Public Property VaccinationRecordList() As DataTable
            Get
                Return _dtResult
            End Get
            Set(value As DataTable)
                _dtResult = value
            End Set
        End Property

        Public ReadOnly Property VaccinationRecordCount() As Integer
            Get
                Return _dtResult.Rows.Count
            End Get
        End Property
#End Region

#Region "Constructor"
        Public Sub New(ByVal intPatientID As Integer, ByVal intPatientResultCode As Integer, ByVal intVaccineResultCode As Integer)
            _intPatientID = intPatientID
            _intPatientResultCode = intPatientResultCode
            _intVaccineResultCode = intVaccineResultCode
        End Sub
#End Region

    End Class

End Namespace