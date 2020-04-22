Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace EHSVaccination

    Public Class PatientRequest

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
#Region "Constant"
        Public Enum PatientRequestResultCode
            Success = 0
            InvalidDocumentNo = 98
            UnexpectedError = 99
        End Enum

#End Region
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]


#Region "Private Member"
        Private _intPatientID As Integer
        Private _strDocumentNo As String
        Private _intDocumentCount As Integer
        Private _listPatientDocument As New List(Of PatientDocument)

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private _enumPatientRequestResultCode As PatientRequestResultCode
        Private _exException As Exception
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

#Region "Property"
        Public ReadOnly Property PatientID() As Integer
            Get
                Return _intPatientID
            End Get
        End Property

        Public ReadOnly Property DocumentNo() As String
            Get
                Return _strDocumentNo
            End Get
        End Property

        Public ReadOnly Property DocumentCount() As Integer
            Get
                Return _intDocumentCount
            End Get
        End Property

        Public Property PatientDocumentList() As List(Of PatientDocument)
            Get
                Return _listPatientDocument
            End Get
            Set(value As List(Of PatientDocument))
                _listPatientDocument = value
            End Set
        End Property

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Property PatientRequestResult() As PatientRequestResultCode
            Get
                Return _enumPatientRequestResultCode
            End Get
            Set(value As PatientRequestResultCode)
                _enumPatientRequestResultCode = value
            End Set
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Property Exception() As Exception
            Get
                Return _exException
            End Get
            Set(value As Exception)
                _exException = value
            End Set
        End Property
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

#Region "Constructor"
        Public Sub New(ByVal intPatientID As Integer, ByVal strDocumentNo As String, ByVal intDocumentCount As Integer)
            _intPatientID = intPatientID
            _strDocumentNo = strDocumentNo
            _intDocumentCount = intDocumentCount

            ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
            ' ----------------------------------------------------------
            _enumPatientRequestResultCode = PatientRequestResultCode.Success
            ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        End Sub

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Sub New(ByVal intPatientID As Integer, ByVal enumPatientRequestResultCode As PatientRequestResultCode, ByVal ex As Exception)
            _intPatientID = intPatientID
            _enumPatientRequestResultCode = enumPatientRequestResultCode
            _exException = ex

        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

#End Region

    End Class

End Namespace