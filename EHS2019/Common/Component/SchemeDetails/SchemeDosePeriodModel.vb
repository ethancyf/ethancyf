Imports System.Data.SqlClient

Namespace Component.SchemeDetails
    <Serializable()> Public Class SchemeDosePeriodModel

#Region "Schema"

        'Scheme_Code	char(10)	Unchecked
        'Scheme_Seq	smallint	Unchecked
        'Subsidize_Code	char(10)	Unchecked
        'Period_Seq	smallint	Unchecked
        'Dose_name	varchar(20)	Unchecked
        'From_dtm	datetime	Unchecked
        'To_dtm	datetime	Unchecked
        'Record_Status	char(1)	Unchecked

#End Region

#Region "Private Member"

        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String
        Private _intPeriod_Seq As Integer
        Private _strDose_name As String

        Private _dtmFrom_dtm As DateTime
        Private _dtmTo_dtm As DateTime
        Private _strRecord_Status As String

#End Region

#Region "Property"

        Public Property SchemeCode() As String
            Get
                Return Me._strScheme_Code
            End Get
            Set(ByVal value As String)
                Me._strScheme_Code = value
            End Set
        End Property

        Public Property SchemeSeq() As Integer
            Get
                Return Me._intScheme_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intScheme_Seq = value
            End Set
        End Property

        Public Property SubsidizeCode() As String
            Get
                Return Me._strSubsidize_Code
            End Get
            Set(ByVal value As String)
                Me._strSubsidize_Code = value
            End Set
        End Property

        Public Property PeriodSeq() As Integer
            Get
                Return Me._intPeriod_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intPeriod_Seq = value
            End Set
        End Property

        Public Property DoseName() As String
            Get
                Return Me._strDose_name
            End Get
            Set(ByVal value As String)
                Me._strDose_name = value
            End Set
        End Property

        Public Property FromDtm() As DateTime
            Get
                Return Me._dtmFrom_dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmFrom_dtm = value
            End Set
        End Property

        Public Property ToDtm() As DateTime
            Get
                Return Me._dtmTo_dtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmTo_dtm = value
            End Set
        End Property

        Public Property RecordStatus() As String
            Get
                Return Me._strRecord_Status
            End Get
            Set(ByVal value As String)
                Me._strRecord_Status = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSchemeDosePeriodModel As SchemeDosePeriodModel)
            Me._strScheme_Code = udtSchemeDosePeriodModel._strScheme_Code
            Me._intScheme_Seq = udtSchemeDosePeriodModel._intScheme_Seq
            Me._strSubsidize_Code = udtSchemeDosePeriodModel._strSubsidize_Code
            Me._intPeriod_Seq = udtSchemeDosePeriodModel._intPeriod_Seq
            Me._strDose_name = udtSchemeDosePeriodModel._strDose_name

            Me._dtmFrom_dtm = udtSchemeDosePeriodModel._dtmFrom_dtm
            Me._dtmTo_dtm = udtSchemeDosePeriodModel._dtmTo_dtm
            Me._strRecord_Status = udtSchemeDosePeriodModel._strRecord_Status
        End Sub

        Public Sub New(ByVal strScheme_Code As String, ByVal intScheme_Seq As Integer, ByVal strSubsidize_Code As String, _
                        ByVal intPeriod_Seq As Integer, ByVal strDose_name As String, _
                        ByVal dtmFrom_dtm As String, ByVal dtmTo_dtm As String, ByVal strRecord_Status As String)

            Me._strScheme_Code = strScheme_Code
            Me._intScheme_Seq = intScheme_Seq
            Me._strSubsidize_Code = strSubsidize_Code
            Me._intPeriod_Seq = intPeriod_Seq
            Me._strDose_name = strDose_name

            Me._dtmFrom_dtm = dtmFrom_dtm
            Me._dtmTo_dtm = dtmTo_dtm
            Me._strRecord_Status = strRecord_Status
        End Sub

#End Region
    End Class
End Namespace


