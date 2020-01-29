Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace Component.DHTransaction
    <Serializable()> _
    Public Class DHVaccineModel

#Region "Constants"
        Public Class VaccineIdenifierType
            Public Const L2 As String = "L2"
            Public Const L3 As String = "L3"
        End Class

#End Region

#Region "Private Members"
        Private _strVaccineType As String
        Private _strVaccineIdenType As String
        Private _strValidDoseInd As String
        Private _strVaccineProviderEng As String
        Private _strVaccineProviderChi As String
        Private _dtmAdmDate As DateTime
        Private _strAdmLocEng As String
        Private _strAdmLocChi As String
        Private _strDoseSeq As String
        Private _strDoseSeqDescEng As String
        Private _strDoseSeqDescChi As String
        Private _udtVaccineL3IdenModel As DHVaccineL3IdenModel
        Private _udtVaccineL2IdenModel As DHVaccineL2IdenModel

#End Region

#Region "Properties"
        Public ReadOnly Property VaccineType As String
            Get
                Return _strVaccineType
            End Get
        End Property

        Public ReadOnly Property VaccineIdenType As String
            Get
                Return _strVaccineIdenType
            End Get
        End Property

        Public ReadOnly Property ValidDoseInd As String
            Get
                Return _strValidDoseInd
            End Get
        End Property

        Public ReadOnly Property VaccineProviderEng As String
            Get
                Return _strVaccineProviderEng

            End Get
        End Property

        Public ReadOnly Property VaccineProviderChi As String
            Get
                Return _strVaccineProviderChi
            End Get
        End Property

        Public ReadOnly Property AdmDate As DateTime
            Get
                Return _dtmAdmDate
            End Get
        End Property

        Public ReadOnly Property AdmLocEng As String
            Get
                Return _strAdmLocEng
            End Get
        End Property

        Public ReadOnly Property AdmLocChi As String
            Get
                Return _strAdmLocChi
            End Get
        End Property

        Public ReadOnly Property DoseSeq As String
            Get
                Return _strDoseSeq
            End Get
        End Property

        Public ReadOnly Property DoseSeqDescEng As String
            Get
                Return _strDoseSeqDescEng
            End Get
        End Property

        Public ReadOnly Property DoseSeqDescChi As String
            Get
                Return _strDoseSeqDescChi
            End Get
        End Property

        Public Property VaccineL3Iden As DHVaccineL3IdenModel
            Get
                Return _udtVaccineL3IdenModel
            End Get
            Set(value As DHVaccineL3IdenModel)
                _udtVaccineL3IdenModel = value
            End Set
        End Property

        Public Property VaccineL2Iden As DHVaccineL2IdenModel
            Get
                Return _udtVaccineL2IdenModel
            End Get
            Set(value As DHVaccineL2IdenModel)
                _udtVaccineL2IdenModel = value
            End Set
        End Property

#End Region

#Region "Constructors"
        Public Sub New(ByVal strVaccineType As String, _
                        ByVal strVaccineIdenType As String, _
                        ByVal strValidDoseInd As String, _
                        ByVal strVaccineProviderEng As String, _
                        ByVal strVaccineProviderChi As String, _
                        ByVal dtmAdmDate As DateTime, _
                        ByVal strAdmLocEng As String, _
                        ByVal strAdmLocChi As String, _
                        ByVal strDoseSeq As String, _
                        ByVal strDoseSeqDescEng As String, _
                        ByVal strDoseSeqDescChi As String
                        )

            _strVaccineType = strVaccineType
            _strVaccineIdenType = strVaccineIdenType
            _strValidDoseInd = strValidDoseInd
            _strVaccineProviderEng = strVaccineProviderEng
            _strVaccineProviderChi = strVaccineProviderChi
            _dtmAdmDate = dtmAdmDate
            _strAdmLocEng = strAdmLocEng
            _strAdmLocChi = strAdmLocChi
            _strDoseSeq = strDoseSeq
            _strDoseSeqDescEng = strDoseSeqDescEng
            _strDoseSeqDescChi = strDoseSeqDescChi

        End Sub
#End Region

#Region "Supported Functions"
        Public Function Copy() As DHVaccineModel
            Dim udtDHVaccineRecord As New DHVaccineModel(Me.VaccineType, _
                                                          Me.VaccineIdenType, _
                                                          Me.ValidDoseInd, _
                                                          Me.VaccineProviderEng, _
                                                          Me.VaccineProviderChi, _
                                                          Me.AdmDate, _
                                                          Me.AdmLocEng, _
                                                          Me.AdmLocChi, _
                                                          Me.DoseSeq, _
                                                          Me.DoseSeqDescEng, _
                                                          Me.DoseSeqDescChi)

            Dim udtVaccineL3Iden As DHVaccineL3IdenModel = Nothing
            Dim udtVaccineL2Iden As DHVaccineL2IdenModel = Nothing

            If Not Me.VaccineL3Iden Is Nothing Then
                udtVaccineL3Iden = New DHVaccineL3IdenModel(VaccineL3Iden.HkRegNum, VaccineL3Iden.VaccineProdName)
            End If

            If Not Me.VaccineL2Iden Is Nothing Then
                udtVaccineL2Iden = New DHVaccineL2IdenModel(VaccineL2Iden.VaccineDesc)
            End If

            udtDHVaccineRecord.VaccineL3Iden = udtVaccineL3Iden
            udtDHVaccineRecord.VaccineL2Iden = udtVaccineL2Iden

            Return udtDHVaccineRecord

        End Function
#End Region

    End Class
End Namespace