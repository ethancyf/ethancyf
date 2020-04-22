Imports Microsoft.VisualBasic
Imports System.Globalization
Imports Common.WebService.Interface.HAVaccineResult

Namespace Component.HATransaction
    <Serializable()> _
    Public Class HAPatientModel

#Region "Constants"

#End Region

#Region "Private Member"
        Private _intPatientId As Integer
        Private _enumPatientResultCode As enumPatientResultCode = enumPatientResultCode.PatientNotFound
        Private _enumVaccineResultCode As enumVaccineResultCode = enumVaccineResultCode.NoRecordReturned

        Private _udtVaccineList As New Component.HATransaction.HAVaccineModelCollection

#End Region

#Region "Properties"
        ''' <summary>
        ''' Patient ID retrieve from CMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property PatientId() As Integer
            Get
                Return _intPatientId
            End Get
        End Property

        ''' <summary>
        ''' Get or Set Patient result code retrieve from CMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PatientResultCode() As enumPatientResultCode
            Get
                Return _enumPatientResultCode
            End Get
            Set(ByVal value As enumPatientResultCode)
                _enumPatientResultCode = value
            End Set
        End Property

        ''' <summary>
        ''' Vaccination records retrieve from CMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property VaccineResultCode() As enumVaccineResultCode
            Get
                Return _enumVaccineResultCode
            End Get
            Set(ByVal value As enumVaccineResultCode)
                _enumVaccineResultCode = value
            End Set
        End Property

        ''' <summary>
        ''' Vaccination records retrieve from CMS web service
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property VaccineList() As Component.HATransaction.HAVaccineModelCollection
            Get
                Return _udtVaccineList
            End Get
        End Property

#End Region

#Region "Constructor"
        Public Sub New(ByVal intPatientId As Integer, _
                       ByVal enumPatientResultCode As enumPatientResultCode, _
                       ByVal enumVaccineResultCode As enumVaccineResultCode)

            _intPatientId = intPatientId
            _enumPatientResultCode = enumPatientResultCode
            _enumVaccineResultCode = enumVaccineResultCode

        End Sub

#End Region

#Region "Supported Function"
        Public Function Copy() As HAPatientModel
            Dim udtPatient As HAPatientModel = New HAPatientModel(_intPatientId, _enumPatientResultCode, _enumVaccineResultCode)

            For Each udtVaccine As HAVaccineModel In Me.VaccineList
                udtPatient.VaccineList.Add(udtVaccine.Copy())
            Next

            Return udtPatient

        End Function
#End Region

    End Class

End Namespace