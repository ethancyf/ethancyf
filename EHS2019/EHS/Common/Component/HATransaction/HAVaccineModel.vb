Imports Microsoft.VisualBasic
Imports System.Globalization

Namespace Component.HATransaction
    <Serializable()> _
    Public Class HAVaccineModel

        Public Class OnSiteClass
            Public Const Y As String = "Y"
            Public Const N As String = "N"
        End Class

        Private _dCreateDtm As DateTime
        Public ReadOnly Property CreateDtm() As DateTime
            Get
                Return _dCreateDtm
            End Get
        End Property

        Private _dInjectionDtm As DateTime
        Public Property InjectionDtm() As DateTime
            Get
                Return _dInjectionDtm
            End Get
            Set(ByVal value As DateTime)
                _dInjectionDtm = value
            End Set
        End Property

        Private _strVaccineCode As String
        Public Property VaccineCode() As String
            Get
                Return _strVaccineCode
            End Get
            Set(ByVal value As String)
                _strVaccineCode = value
            End Set
        End Property

        Private _strVaccineDesc As String
        Public Property VaccineDesc() As String
            Get
                Return _strVaccineDesc
            End Get
            Set(ByVal value As String)
                _strVaccineDesc = value
            End Set
        End Property

        Private _strVaccineDescChinese As String
        Public Property VaccineDescChinese() As String
            Get
                Return _strVaccineDescChinese
            End Get
            Set(ByVal value As String)
                _strVaccineDescChinese = value
            End Set
        End Property

        Private _strDoseSeqCode As String
        Public Property DoseSeqCode() As String
            Get
                Return _strDoseSeqCode
            End Get
            Set(ByVal value As String)
                _strDoseSeqCode = value
            End Set
        End Property

        Private _strDoseSeqDesc As String
        Public Property DoseSeqDesc() As String
            Get
                Return _strDoseSeqDesc
            End Get
            Set(ByVal value As String)
                _strDoseSeqDesc = value
            End Set
        End Property

        Private _strDoseSeqDescChinese As String
        Public Property DoseSeqDescChinese() As String
            Get
                Return _strDoseSeqDescChinese
            End Get
            Set(ByVal value As String)
                _strDoseSeqDescChinese = value
            End Set
        End Property

        Private _strProvider As String
        Public ReadOnly Property Provider() As String
            Get
                Return _strProvider
            End Get
        End Property

        Private _strLocation As String
        Public ReadOnly Property Location() As String
            Get
                Return _strLocation
            End Get
        End Property

        Private _strLocationChinese As String
        Public ReadOnly Property LocationChinese() As String
            Get
                Return _strLocationChinese
            End Get
        End Property

        Private _strOnSite As String
        Public ReadOnly Property OnSite() As String
            Get
                Return _strOnSite
            End Get
        End Property

        Private _strVaccineBrand As String
        Public ReadOnly Property VaccineBrand() As String
            Get
                Return _strVaccineBrand
            End Get
        End Property

        Private _strVaccineLotNo As String
        Public ReadOnly Property VaccineLotNo() As String
            Get
                Return _strVaccineLotNo
            End Get
        End Property

        Public Sub New(ByVal dtmCreateDtm As DateTime, ByVal dtmInjectionDtm As DateTime, _
                        ByVal strVaccineCode As String, ByVal strVaccineDesc As String, ByVal strVaccineDescChinese As String, _
                        ByVal strDoseSeqCode As String, ByVal strDoseSeqDesc As String, ByVal strDoseSeqDescChinese As String, _
                        ByVal strProvider As String, ByVal strLocation As String, ByVal strLocationChinese As String, ByVal strOnSite As String,
                        ByVal strVaccineBrand As String, ByVal strVaccineLotNo As String)

            _dCreateDtm = dtmCreateDtm
            _dInjectionDtm = dtmInjectionDtm
            _strVaccineCode = strVaccineCode
            _strVaccineDesc = strVaccineDesc
            _strVaccineDescChinese = strVaccineDescChinese
            _strDoseSeqCode = strDoseSeqCode
            _strDoseSeqDesc = strDoseSeqDesc
            _strDoseSeqDescChinese = strDoseSeqDescChinese
            _strProvider = strProvider
            _strLocation = strLocation
            _strLocationChinese = strLocationChinese
            _strVaccineBrand = strVaccineBrand
            _strVaccineLotNo = strVaccineLotNo

            If strOnSite <> OnSiteClass.Y AndAlso strOnSite <> OnSiteClass.N Then
                Throw New Exception(String.Format("HAVaccineModel: Invalid onsite value ({0})", strOnSite))
            End If

            _strOnSite = strOnSite
        End Sub

        Public Function Copy() As HAVaccineModel

            Return New HAVaccineModel(_dCreateDtm, _dInjectionDtm, _
                                        _strVaccineCode, _strVaccineDesc, _strVaccineDescChinese, _
                                        _strDoseSeqCode, _strDoseSeqDesc, _strDoseSeqDescChinese, _
                                        _strProvider, _strLocation, _strLocationChinese, _strOnSite, _
                                        _strVaccineBrand, _strVaccineLotNo)
        End Function
    End Class
End Namespace