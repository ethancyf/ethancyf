Imports System.Data.SqlClient

Namespace Component.SchemeDetails
    <Serializable()> Public Class SchemeVaccineDetailModel

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

#Region "Schema"

        'Scheme_Code	char(10)	Unchecked
        'Scheme_Seq	smallint	Unchecked
        'Subsidize_Code	char(10)	Unchecked
        'Vaccine_Fee	money	Checked
        'Vaccine_Fee_Display_Enabled	char(1)	Checked
        'Injection_Fee	money	Checked
        'Injection_Fee_Display_Enabled	char(1)	Checked
        'Subsidize_Fee	money	Checked
        'Subsidize_Fee_Display_Enabled	char(1)	Checked

#End Region

#Region "Private Member"

        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String
        Private _dblVaccine_Fee As Nullable(Of Double)
        Private _strVaccine_Fee_Display_Enabled As String
        Private _dblInjection_Fee As Nullable(Of Double)
        Private _strInjection_Fee_Display_Enabled As String
        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Private _dblSubsidize_Fee As Nullable(Of Double)
        Private _strSubsidize_Fee_Display_Enabled As String
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

#End Region

#Region "SQL Data Type"

        Public Const Scheme_CodeDataType As SqlDbType = SqlDbType.Char
        Public Const Scheme_CodeDataSize As Integer = 10

        Public Const Scheme_SeqDataType As SqlDbType = SqlDbType.SmallInt
        Public Const Scheme_SeqDataSize As Integer = 2

        Public Const Subsidize_CodeDataType As SqlDbType = SqlDbType.Char
        Public Const Subsidize_CodeDataSize As Integer = 10

        Public Const Vaccine_FeeDataType As SqlDbType = SqlDbType.Money
        Public Const Vaccine_FeeDataSize As Integer = 8

        Public Const Vaccine_Display_EnabledDataType As SqlDbType = SqlDbType.Char
        Public Const Vaccine_Display_EnabledDataSize As Integer = 1

        Public Const Injection_FeeDataType As SqlDbType = SqlDbType.Money
        Public Const Injection_FeeDataSize As Integer = 8

        Public Const Injection_Display_EnabledDataType As SqlDbType = SqlDbType.Char
        Public Const Injection_Display_EnabledDataSize As Integer = 1

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Const Subsidize_FeeDataType As SqlDbType = SqlDbType.Money
        Public Const Subsidize_FeeDataSize As Integer = 8

        Public Const Subsidize_Display_EnabledDataType As SqlDbType = SqlDbType.Char
        Public Const Subsidize_Display_EnabledDataSize As Integer = 1
        'CRE16-002 (Revamp VSS) [End][Chris YIM]
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

        Public Property VaccineFee() As Nullable(Of Double)
            Get
                Return Me._dblVaccine_Fee
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblVaccine_Fee = value
            End Set
        End Property

        Public Property VaccineFeeDisplayEnabled() As Boolean
            Get
                If Me._strVaccine_Fee_Display_Enabled.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strVaccine_Fee_Display_Enabled = strYES
                Else
                    Me._strVaccine_Fee_Display_Enabled = strNO
                End If
            End Set
        End Property

        Public Property InjectionFee() As Nullable(Of Double)
            Get
                Return Me._dblInjection_Fee
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblInjection_Fee = value
            End Set
        End Property

        Public Property InjectionFeeDisplayEnabled() As Boolean
            Get
                If Me._strInjection_Fee_Display_Enabled.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strInjection_Fee_Display_Enabled = strYES
                Else
                    Me._strInjection_Fee_Display_Enabled = strNO
                End If
            End Set
        End Property

        'CRE16-002 (Revamp VSS) [Start][Chris YIM]
        '-----------------------------------------------------------------------------------------
        Public Property SubsidizeFee() As Nullable(Of Double)
            Get
                Return Me._dblInjection_Fee
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblInjection_Fee = value
            End Set
        End Property

        Public Property SubsidizeFeeDisplayEnabled() As Boolean
            Get
                If Me._strInjection_Fee_Display_Enabled.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strInjection_Fee_Display_Enabled = strYES
                Else
                    Me._strInjection_Fee_Display_Enabled = strNO
                End If
            End Set
        End Property
        'CRE16-002 (Revamp VSS) [End][Chris YIM]

#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSchemeVaccineDetailModel As SchemeVaccineDetailModel)
            Me._strScheme_Code = udtSchemeVaccineDetailModel._strScheme_Code
            Me._intScheme_Seq = udtSchemeVaccineDetailModel._intScheme_Seq
            Me._strSubsidize_Code = udtSchemeVaccineDetailModel._strSubsidize_Code
            Me._dblVaccine_Fee = udtSchemeVaccineDetailModel._dblVaccine_Fee
            Me._strVaccine_Fee_Display_Enabled = udtSchemeVaccineDetailModel._strVaccine_Fee_Display_Enabled

            Me._dblInjection_Fee = udtSchemeVaccineDetailModel._dblInjection_Fee
            Me._strInjection_Fee_Display_Enabled = udtSchemeVaccineDetailModel._strInjection_Fee_Display_Enabled

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me._dblSubsidize_Fee = udtSchemeVaccineDetailModel._dblSubsidize_Fee
            Me._strSubsidize_Fee_Display_Enabled = udtSchemeVaccineDetailModel._strSubsidize_Fee_Display_Enabled
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End Sub

        Public Sub New(ByVal strSchemeCode As String, ByVal intSchemeSeq As Integer, ByVal strSubsidizeCode As String, _
            ByVal dblVaccineFee As Nullable(Of Double), ByVal strVaccineDisplayEnabled As String, _
            ByVal dblInjectionFee As Nullable(Of Double), ByVal strInjectionDisplayEnabled As String, _
            ByVal dblSubsidizeFee As Nullable(Of Double), ByVal strSubsidizeDisplayEnabled As String)

            Me._strScheme_Code = strSchemeCode
            Me._intScheme_Seq = intSchemeSeq
            Me._strSubsidize_Code = strSubsidizeCode
            Me._dblVaccine_Fee = dblVaccineFee
            Me._strVaccine_Fee_Display_Enabled = strVaccineDisplayEnabled

            Me._dblInjection_Fee = dblInjectionFee
            Me._strInjection_Fee_Display_Enabled = strInjectionDisplayEnabled

            'CRE16-002 (Revamp VSS) [Start][Chris YIM]
            '-----------------------------------------------------------------------------------------
            Me._dblSubsidize_Fee = dblSubsidizeFee
            Me._strSubsidize_Fee_Display_Enabled = strSubsidizeDisplayEnabled
            'CRE16-002 (Revamp VSS) [End][Chris YIM]
        End Sub

#End Region
    End Class
End Namespace
