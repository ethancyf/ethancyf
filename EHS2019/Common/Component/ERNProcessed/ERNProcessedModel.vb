Namespace Component.ERNProcessed
    <Serializable()> Public Class ERNProcessedModel

        Private _strERN As String
        Private _strSPID As String
        Private _dtmCreateDtm As Nullable(Of DateTime)
        Private _strCreateBy As String
        Private _strSubERN As String

        Public Const CreateByDataType As SqlDbType = SqlDbType.VarChar
        Public Const CreateByDataSize As Integer = 20


        Public Property EnrolRefNo() As String
            Get
                Return _strERN
            End Get
            Set(ByVal value As String)
                _strERN = value
            End Set
        End Property

        Public Property SPID() As String
            Get
                Return _strSPID
            End Get
            Set(ByVal value As String)
                _strSPID = value
            End Set
        End Property

        Public Property CreateDtm() As Nullable(Of DateTime)
            Get
                Return _dtmCreateDtm
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                _dtmCreateDtm = value
            End Set
        End Property

        Public Property CreateBy() As String
            Get
                Return _strCreateBy
            End Get
            Set(ByVal value As String)
                _strCreateBy = value
            End Set
        End Property

        Public Property SubEnrolRefNo() As String
            Get
                Return _strSubERN
            End Get
            Set(ByVal value As String)
                _strSubERN = value
            End Set
        End Property

       

        Public Sub New()

        End Sub

        Public Sub New(ByVal udtERNProcessed As ERNProcessedModel)
            _strERN = udtERNProcessed.EnrolRefNo
            _strSPID = udtERNProcessed.SPID
            _dtmCreateDtm = udtERNProcessed.CreateDtm
            _strCreateBy = udtERNProcessed.CreateBy
            _strSubERN = udtERNProcessed.SubEnrolRefNo
            
        End Sub

        Public Sub New(ByVal strERN As String, ByVal strSPID As String, ByVal strCreateBy As String, ByVal dtmCreateDtm As Nullable(Of DateTime), _
                        ByVal strSubERN As String)

            _strERN = strERN
            _strSPID = strSPID
            _dtmCreateDtm = dtmCreateDtm
            _strCreateBy = strCreateBy
            _strSubERN = strSubERN
        End Sub


    End Class
End Namespace

