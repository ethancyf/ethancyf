Imports System.Data.SqlClient
Imports Common.Component.SchemeDetails

Namespace Component.Scheme
    <Serializable()> Public Class SubsidizeFeeModel
        Implements IComparable
        Implements IComparer

        Private Const strYES As String = "Y"
        Private Const strNO As String = "N"

        Public Class SubsidizeFeeTypeClass
            Public Const SubsidizeFeeTypeVoucher As String = "VOUCHER"
            Public Const SubsidizeFeeTypeVaccine As String = "VACCINE_FEE"
            Public Const SubsidizeFeeTypeInjection As String = "INJECTION_FEE"
            Public Const SubsidizeFeeTypeNA As String = "NA"
            Public Const SubsidizeFeeTypeSubsidize As String = "SUBSIDIZE_FEE"

        End Class

#Region "Private Member"

        Private _strSchemeCode As String
        Private _intSubsidizeSeq As Integer
        Private _strSubsidizeCode As String
        Private _dtmEffectiveDtm As DateTime
        Private _dtmExpiryDtm As DateTime

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        'Private _dblSubsidizeFee As Double
        Private _dblSubsidizeFee As Nullable(Of Double)
        ' CRE13-001 - EHAPP [End][Tommy L]
        Private _strSubsidizeFeeType As String
        Private _intSubsidizeFeeTypeDisplaySeq As Integer
        Private _strSubsidizeFeeTypeDisplayResource As String
        Private _strSubsidizeFeeVisible As String

        Private _strCreateBy As String
        Private _dtmCreateDtm As DateTime
        Private _strUpdateBy As String
        Private _dtmUpdateDtm As DateTime

#End Region

#Region "SQL Data Type"
        ' ----------------------------------------------
        Public Const Scheme_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Scheme_Code_DataSize As Integer = 10

        Public Const Subsidize_Seq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const Subsidize_Seq_DataSize As Integer = 2

        Public Const Subsidize_Code_DataType As SqlDbType = SqlDbType.Char
        Public Const Subsidize_Code_DataSize As Integer = 10

        Public Const Effective_Dtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Effective_Dtm_DataSize As Integer = 8

        Public Const Expiry_Dtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Expiry_Dtm_DataSize As Integer = 8

        Public Const Sibsidize_Fee_DataType As SqlDbType = SqlDbType.Money
        Public Const Subsidize_Fee_DataSize As Integer = 8

        Public Const Subsidize_Fee_Type_DataType As SqlDbType = SqlDbType.Char
        Public Const Subsidize_Fee_Type_DataSize As Integer = 20

        Public Const Subsidize_Fee_Type_Display_Seq_DataType As SqlDbType = SqlDbType.SmallInt
        Public Const Subsidize_Fee_Type_Display_Seq_DataSize As Integer = 2

        Public Const Subsidize_Fee_Type_Display_Resource_DataType As SqlDbType = SqlDbType.Char
        Public Const Subsidize_Fee_Type_Display_Resource_DataSize As Integer = 50

        Public Const Subsidize_Fee_Visible_DataType As SqlDbType = SqlDbType.Char
        Public Const Subsidize_Fee_Visible_DataSize As Integer = 1

        Public Const Create_By_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Create_By_DataSize As Integer = 20

        Public Const Create_Dtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Create_Dtm_DataSize As Integer = 8

        Public Const Update_By_DataType As SqlDbType = SqlDbType.VarChar
        Public Const Update_By_DataSize As Integer = 20

        Public Const Update_Dtm_DataType As SqlDbType = SqlDbType.DateTime
        Public Const Update_Dtm_DataSize As Integer = 8

#End Region

#Region "Property"

        Public Property SchemeCode() As String
            Get
                Return Me._strSchemeCode
            End Get
            Set(ByVal value As String)
                Me._strSchemeCode = value
            End Set
        End Property

        Public Property SubsidizeSeq() As Integer
            Get
                Return Me._intSubsidizeSeq
            End Get
            Set(ByVal value As Integer)
                Me._intSubsidizeSeq = value
            End Set
        End Property

        Public Property SubsidizeCode() As String
            Get
                Return Me._strSubsidizeCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeCode = value
            End Set
        End Property

        Public Property EffectiveDtm() As DateTime
            Get
                Return Me._dtmEffectiveDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmEffectiveDtm = value
            End Set
        End Property

        Public Property ExpiryDtm() As DateTime
            Get
                Return Me._dtmExpiryDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmExpiryDtm = value
            End Set
        End Property

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        'Public Property SubsidizeFee() As Double
        '   Get
        '       Return Me._dblSubsidizeFee
        '    End Get
        '    Set(ByVal value As Double)
        '        Me._dblSubsidizeFee = value
        '    End Set
        'End Property
        Public Property SubsidizeFee() As Nullable(Of Double)
            Get
                Return Me._dblSubsidizeFee
            End Get
            Set(ByVal value As Nullable(Of Double))
                Me._dblSubsidizeFee = value
            End Set
        End Property
        ' CRE13-001 - EHAPP [End][Tommy L]

        Public Property SubsidizeFeeType() As String
            Get
                Return Me._strSubsidizeFeeType
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeFeeType = value
            End Set
        End Property

        Public Property SubsidizeFeeTypeDisplaySeq() As Integer
            Get
                Return Me._intSubsidizeFeeTypeDisplaySeq
            End Get
            Set(ByVal value As Integer)
                Me._intSubsidizeFeeTypeDisplaySeq = value
            End Set
        End Property

        Public Property SubsidizeFeeTypeDisplayResource() As String
            Get
                Return Me._strSubsidizeFeeTypeDisplayResource
            End Get
            Set(ByVal value As String)
                Me._strSubsidizeFeeTypeDisplayResource = value
            End Set
        End Property

        Public Property SubsidizeFeeVisible() As Boolean
            Get
                If Me._strSubsidizeFeeVisible.Trim().ToUpper().Equals(strYES.Trim().ToUpper()) Then
                    Return True
                Else
                    Return False
                End If
            End Get
            Set(ByVal value As Boolean)
                If value Then
                    Me._strSubsidizeFeeVisible = strYES
                Else
                    Me._strSubsidizeFeeVisible = strNO
                End If
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return Me._strCreateBy
            End Get
            Set(ByVal value As String)
                Me._strCreateBy = value
            End Set
        End Property

        Public Property CreateDtm() As DateTime
            Get
                Return Me._dtmCreateDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmCreateDtm = value
            End Set
        End Property

        Public Property UpdateBy() As String
            Get
                Return Me._strUpdateBy
            End Get
            Set(ByVal value As String)
                Me._strUpdateBy = value
            End Set
        End Property

        Public Property UpdateDtm() As DateTime
            Get
                Return Me._dtmUpdateDtm
            End Get
            Set(ByVal value As DateTime)
                Me._dtmUpdateDtm = value
            End Set
        End Property

#End Region

#Region "Constructor"

        Private Sub New()
        End Sub

        Public Sub New(ByVal udtSubsidizeFeeModel As SubsidizeFeeModel)

            Me._strSchemeCode = udtSubsidizeFeeModel._strSchemeCode
            Me._intSubsidizeSeq = udtSubsidizeFeeModel._intSubsidizeSeq
            Me._strSubsidizeCode = udtSubsidizeFeeModel._strSubsidizeCode
            Me._dtmEffectiveDtm = udtSubsidizeFeeModel._dtmEffectiveDtm
            Me._dtmExpiryDtm = udtSubsidizeFeeModel._dtmExpiryDtm

            Me._dblSubsidizeFee = udtSubsidizeFeeModel._dblSubsidizeFee
            Me._strSubsidizeFeeType = udtSubsidizeFeeModel._strSubsidizeFeeType
            Me._intSubsidizeFeeTypeDisplaySeq = udtSubsidizeFeeModel._intSubsidizeFeeTypeDisplaySeq
            Me._strSubsidizeFeeTypeDisplayResource = udtSubsidizeFeeModel._strSubsidizeFeeTypeDisplayResource
            Me._strSubsidizeFeeVisible = udtSubsidizeFeeModel._strSubsidizeFeeVisible

            Me._strCreateBy = udtSubsidizeFeeModel._strCreateBy
            Me._dtmCreateDtm = udtSubsidizeFeeModel._dtmCreateDtm
            Me._strUpdateBy = udtSubsidizeFeeModel._strUpdateBy
            Me._dtmUpdateDtm = udtSubsidizeFeeModel._dtmUpdateDtm

        End Sub

        ' CRE13-001 - EHAPP [Start][Tommy L]
        ' -------------------------------------------------------------------------------------
        'Public Sub New(ByVal strSchemeCode As String, ByVal intSubsidizeSeq As Integer, ByVal strSubsidizeCode As String, ByVal dtmEffectiveDtm As DateTime, _
        '        ByVal dtmExpiryDtm As DateTime, ByVal dblSubsidizeFee As Double, ByVal strSubsidizeFeeType As String, _
        '        ByVal intSubsidizeFeeTypeDisplaySeq As Integer, ByVal strSubsidizeFeeTypeDisplayResource As String, ByVal strSubsidizeFeeVisible As String, _
        '        ByVal strCreateBy As String, ByVal dtmCreateDtm As String, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As String)
        Public Sub New(ByVal strSchemeCode As String, ByVal intSubsidizeSeq As Integer, ByVal strSubsidizeCode As String, ByVal dtmEffectiveDtm As DateTime, _
                        ByVal dtmExpiryDtm As DateTime, ByVal dblSubsidizeFee As Nullable(Of Double), ByVal strSubsidizeFeeType As String, _
                        ByVal intSubsidizeFeeTypeDisplaySeq As Integer, ByVal strSubsidizeFeeTypeDisplayResource As String, ByVal strSubsidizeFeeVisible As String, _
                        ByVal strCreateBy As String, ByVal dtmCreateDtm As String, ByVal strUpdateBy As String, ByVal dtmUpdateDtm As String)
            ' CRE13-001 - EHAPP [End][Tommy L]

            Me._strSchemeCode = strSchemeCode
            Me._intSubsidizeSeq = intSubsidizeSeq
            Me._strSubsidizeCode = strSubsidizeCode
            Me._dtmEffectiveDtm = dtmEffectiveDtm
            Me._dtmExpiryDtm = dtmExpiryDtm

            Me._dblSubsidizeFee = dblSubsidizeFee
            Me._strSubsidizeFeeType = strSubsidizeFeeType
            Me._intSubsidizeFeeTypeDisplaySeq = intSubsidizeFeeTypeDisplaySeq
            Me._strSubsidizeFeeTypeDisplayResource = strSubsidizeFeeTypeDisplayResource
            Me._strSubsidizeFeeVisible = strSubsidizeFeeVisible

            Me._strCreateBy = strCreateBy
            Me._dtmCreateDtm = dtmCreateDtm
            Me._strUpdateBy = strUpdateBy
            Me._dtmUpdateDtm = dtmUpdateDtm

        End Sub

#End Region

        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
            If obj.GetType Is GetType(SubsidizeFeeModel) Then
                Return Me.SubsidizeFeeTypeDisplaySeq.CompareTo(CType(obj, SubsidizeFeeModel).SubsidizeFeeTypeDisplaySeq)
            Else
                Return -1
            End If
        End Function

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            If x.GetType Is GetType(SubsidizeFeeModel) AndAlso y.GetType Is GetType(SubsidizeFeeModel) Then
                Return CType(x, SubsidizeFeeModel).SubsidizeFeeTypeDisplaySeq.CompareTo(CType(y, SubsidizeFeeModel).SubsidizeFeeTypeDisplaySeq)
            Else
                If x.GetType Is GetType(SubsidizeFeeModel) Then
                    Return -1
                End If
                If y.GetType Is GetType(SubsidizeFeeModel) Then
                    Return 1
                End If
                Return 0
            End If
        End Function
    End Class
End Namespace
