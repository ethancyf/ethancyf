Namespace Component.COVID19

    <Serializable()> _
Public Class DischargeResultModel

        Private _strDemographicResult As String
        Private _dtmDischargeDate As Nullable(Of DateTime)
        Private _strFileID As String

#Region "Contants"
        Public Class ResultCode
            Public Const NotMatch As String = "N"
            Public Const ExactMatch As String = "F"
            Public Const PartialMatch As String = "P"
        End Class

        Public Enum Result
            NotMatch
            ExactMatch
            PartialMatch
        End Enum

#End Region

#Region "Property"

        Public Property DemographicResult() As Result
            Get
                Dim enumResult As Result = Result.NotMatch

                Select Case _strDemographicResult
                    Case ResultCode.NotMatch
                        enumResult = Result.NotMatch
                    Case ResultCode.ExactMatch
                        enumResult = Result.ExactMatch
                    Case ResultCode.PartialMatch
                        enumResult = Result.PartialMatch
                End Select

                Return enumResult
            End Get
            Set(ByVal Value As Result)
                Dim strResultCode As String = ResultCode.NotMatch

                Select Case Value
                    Case Result.NotMatch
                        strResultCode = ResultCode.NotMatch
                    Case Result.ExactMatch
                        strResultCode = ResultCode.ExactMatch
                    Case Result.PartialMatch
                        strResultCode = ResultCode.PartialMatch
                End Select

                _strDemographicResult = strResultCode
            End Set
        End Property

        Public ReadOnly Property DemographicResultCode() As String
            Get
                Return _strDemographicResult
            End Get
        End Property

        Public Property DischargeDate() As Nullable(Of DateTime)
            Get
                Return _dtmDischargeDate
            End Get
            Set(ByVal Value As Nullable(Of DateTime))
                _dtmDischargeDate = Value
            End Set
        End Property

        Public Property FileID() As String
            Get
                Return _strFileID
            End Get
            Set(ByVal Value As String)
                _strFileID = Value
            End Set
        End Property

#End Region

#Region "Constructor"

        Public Sub New()
            _strDemographicResult = String.Empty
            _dtmDischargeDate = Nothing
            _strFileID = String.Empty

        End Sub

        Public Sub New(ByVal strDemographicResult As String, _
                       ByVal dtmDischargeDate As Nullable(Of DateTime), _
                       ByVal strFileID As String)

            _strDemographicResult = strDemographicResult
            _dtmDischargeDate = dtmDischargeDate
            _strFileID = strFileID

        End Sub

#End Region

    End Class

End Namespace

