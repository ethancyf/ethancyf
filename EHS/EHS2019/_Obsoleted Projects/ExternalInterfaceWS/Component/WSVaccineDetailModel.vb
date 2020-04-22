
Namespace Component

    <Serializable()> _
    Public Class WSVaccineDetailModel

        Private _strSubsidyCode As String
        Public Property SubsidyCode() As String
            Get
                Return Me._strSubsidyCode
            End Get
            Set(ByVal value As String)
                Me._strSubsidyCode = value
            End Set
        End Property

        Private _strDoseSeq As String
        Public Property DoseSeq() As String
            Get
                Return Me._strDoseSeq
            End Get
            Set(ByVal value As String)
                Me._strDoseSeq = value
            End Set
        End Property




        Private _blnSubsidyCode_Received As Boolean = False
        Public Property SubsidyCode_Received() As Boolean
            Get
                Return Me._blnSubsidyCode_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnSubsidyCode_Received = value
            End Set
        End Property

        Private _blnDoseSeq_Received As Boolean = False
        Public Property DoseSeq_Received() As Boolean
            Get
                Return Me._blnDoseSeq_Received
            End Get
            Set(ByVal value As Boolean)
                Me._blnDoseSeq_Received = value
            End Set
        End Property

    End Class

End Namespace

