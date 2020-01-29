Namespace Component.Scheme

    <Serializable()> Public Class SchemeNoticeModel

#Region "DB Data Type Mapping"
        Public Const DATA_TYPE_SCHEME_CODE As SqlDbType = SqlDbType.Char
        Public Const DATA_SIZE_SCHEME_CODE As Integer = 10

        Public Const DATA_TYPE_NOTICE_SEQ As SqlDbType = SqlDbType.SmallInt
        Public Const DATA_SIZE_NOTICE_SEQ As Integer = 2

        Public Const DATA_TYPE_NEW_PERIOD_FROM As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_NEW_PERIOD_FROM As Integer = 8

        Public Const DATA_TYPE_NEW_PERIOD_TO As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_NEW_PERIOD_TO As Integer = 8

        Public Const DATA_TYPE_DISPLAY_PERIOD_FROM As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_DISPLAY_PERIOD_FROM As Integer = 8

        Public Const DATA_TYPE_DISPLAY_PERIOD_TO As SqlDbType = SqlDbType.DateTime
        Public Const DATA_SIZE_DISPLAY_PERIOD_TO As Integer = 8

        Public Const DATA_TYPE_HTML_CONTENT As SqlDbType = SqlDbType.VarChar
        Public Const DATA_SIZE_HTML_CONTENT As Integer = -1

        Public Const DATA_TYPE_HTML_CONTENT_CHI As SqlDbType = SqlDbType.NVarChar
        Public Const DATA_SIZE_HTML_CONTENT_CHI As Integer = -1
#End Region

#Region "Private Member"
        Private _strSchemeCode As String
        Private _intNoticeSeq As Integer
        Private _dtmNewPeriodFrom As Nullable(Of DateTime)
        Private _dtmNewPeriodTo As Nullable(Of DateTime)
        Private _dtmDisplayPeriodFrom As DateTime
        Private _dtmDisplayPeriodTo As DateTime
        Private _strHTMLContent As String
        Private _strHTMLContentChi As String
        Private _strHTMLContentCN As String
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

        Public Property NoticeSeq() As Integer
            Get
                Return Me._intNoticeSeq
            End Get
            Set(ByVal value As Integer)
                Me._intNoticeSeq = value
            End Set
        End Property

        Public Property NewPeriodFrom() As Nullable(Of DateTime)
            Get
                Return _dtmNewPeriodFrom
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmNewPeriodFrom = value
            End Set
        End Property

        Public Property NewPeriodTo() As Nullable(Of DateTime)
            Get
                Return _dtmNewPeriodTo
            End Get
            Set(ByVal value As Nullable(Of DateTime))
                Me._dtmNewPeriodTo = value
            End Set
        End Property

        Public Property DisplayPeriodFrom() As DateTime
            Get
                Return _dtmDisplayPeriodFrom
            End Get
            Set(ByVal value As DateTime)
                Me._dtmDisplayPeriodFrom = value
            End Set
        End Property

        Public Property DisplayPeriodTo() As DateTime
            Get
                Return _dtmDisplayPeriodTo
            End Get
            Set(ByVal value As DateTime)
                Me._dtmDisplayPeriodTo = value
            End Set
        End Property

        Public Property HTMLContent() As String
            Get
                Return Me._strHTMLContent
            End Get
            Set(ByVal value As String)
                Me._strHTMLContent = value
            End Set
        End Property

        Public Property HTMLContentChi() As String
            Get
                Return Me._strHTMLContentChi
            End Get
            Set(ByVal value As String)
                Me._strHTMLContentChi = value
            End Set
        End Property

        Public Property HTMLContentCN() As String
            Get
                Return Me._strHTMLContentCN
            End Get
            Set(ByVal value As String)
                Me._strHTMLContentCN = value
            End Set
        End Property

        'addional property
        Public ReadOnly Property WithinNewPeriod(ByVal dtmCurrentDateTime As DateTime) As Boolean
            Get
                Dim blnWithinPeriod As Boolean = False

                If Me._dtmNewPeriodFrom.HasValue = True AndAlso Me._dtmNewPeriodTo.HasValue = True Then
                    If CType(_dtmNewPeriodFrom, DateTime) <= dtmCurrentDateTime AndAlso dtmCurrentDateTime < CType(_dtmNewPeriodTo, DateTime) Then
                        blnWithinPeriod = True
                    End If
                End If

                Return blnWithinPeriod
            End Get
        End Property

        Public ReadOnly Property WithinDisplayPeriod(ByVal dtmCurrentDateTime As DateTime) As Boolean
            Get
                Dim blnWithinPeriod As Boolean = False

                    If CType(_dtmDisplayPeriodFrom, DateTime) <= dtmCurrentDateTime AndAlso dtmCurrentDateTime < CType(_dtmDisplayPeriodTo, DateTime) Then
                        blnWithinPeriod = True
                    End If

                Return blnWithinPeriod
            End Get
        End Property

#End Region

#Region "Constructor"
        Public Sub New(ByVal strSchemeCode As String, _
            ByVal intNoticeSeq As Integer, _
            ByVal dtmNewPeriodFrom As Nullable(Of DateTime), _
            ByVal dtmNewPeriodTo As Nullable(Of DateTime), _
            ByVal dtmDisplayPeriodFrom As DateTime, _
            ByVal dtmDisplayPeriodTo As DateTime, _
            ByVal strHTMLContent As String, _
            ByVal strHTMLContentChi As String, _
            ByVal strHTMLContentCN As String)

            _strSchemeCode = strSchemeCode
            _intNoticeSeq = intNoticeSeq
            _dtmNewPeriodFrom = dtmNewPeriodFrom
            _dtmNewPeriodTo = dtmNewPeriodTo
            _dtmDisplayPeriodFrom = dtmDisplayPeriodFrom
            _dtmDisplayPeriodTo = dtmDisplayPeriodTo
            _strHTMLContent = strHTMLContent
            _strHTMLContentChi = strHTMLContentChi
            _strHTMLContentCN = strHTMLContentCN
        End Sub
#End Region

    End Class
End Namespace