Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace EHSVaccination

    Public Class PatientDocument

        Public Enum enumExactDOB
            D
            M
            Y
            A
            T
            U
            V
            NotSupported
        End Enum

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Const EXACT_DOB_D As String = "DD/MM/YYYY"
        Private Const EXACT_DOB_M As String = "MM/YYYY"
        Private Const EXACT_DOB_Y As String = "YYYY"
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        Private Const CMS_DOB_FORMAT As String = "dd/MM/yyyy"

        Private _sDocumentType As String
        Public ReadOnly Property DocumentType() As String
            Get
                Return _sDocumentType
            End Get
        End Property

        Private _sName As String
        Public ReadOnly Property Name() As String
            Get
                Return _sName
            End Get
        End Property

        Private _sSex As String
        Public ReadOnly Property Sex() As String
            Get
                Return _sSex
            End Get
        End Property

        Private _sDOB As String
        Public ReadOnly Property DOB() As String
            Get
                Return _sDOB
            End Get
        End Property

        Private _sDOBFormat As String
        Public ReadOnly Property DOBFormat() As String
            Get
                Return _sDOBFormat
            End Get
        End Property

        Private _eExcatDOB As enumExactDOB
        Public ReadOnly Property ExcatDOB() As enumExactDOB
            Get
                Return _eExcatDOB
            End Get
        End Property

        Private _dDOBDate As Nullable(Of Date) = Nothing
        Public ReadOnly Property DOBDate() As Nullable(Of Date)
            Get
                Return _dDOBDate
            End Get
        End Property

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Public Sub New(ByVal sDocumentType As String, ByVal sName As String, ByVal sSex As String, ByVal sDOB As String, ByVal sDOBFormat As String)
            _sDocumentType = sDocumentType
            _sName = sName
            _sSex = sSex
            _sDOB = sDOB
            _sDOBFormat = sDOBFormat

            ConvertDate()
        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

        ' CRE18-004 (CIMS Vaccination Sharing) [Start][Chris YIM]
        ' ----------------------------------------------------------
        Private Sub ConvertDate()

            ' Get exact DOB
            Select Case _sDOBFormat
                Case EXACT_DOB_D
                    _eExcatDOB = enumExactDOB.D
                Case EXACT_DOB_M
                    _eExcatDOB = enumExactDOB.M
                Case EXACT_DOB_Y
                    _eExcatDOB = enumExactDOB.Y
                Case Else
                    _eExcatDOB = enumExactDOB.NotSupported
            End Select

            ' Get DOB date value
            Dim dTmp As Date = Date.MinValue
            ' CMS DOB must in "DD/MM/YYYY" format
            If Date.TryParseExact(_sDOB, CMS_DOB_FORMAT, New CultureInfo("en-US"), System.Globalization.DateTimeStyles.None, dTmp) Then
                _dDOBDate = dTmp
            Else
                _dDOBDate = Date.MinValue
            End If

        End Sub
        ' CRE18-004 (CIMS Vaccination Sharing) [End][Chris YIM]

    End Class

End Namespace