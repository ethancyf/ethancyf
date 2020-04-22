
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


        Private _blnSubsidyCode_included As Boolean
        Public Property SubsidyCode_included() As Boolean
            Get
                Return Me._blnSubsidyCode_included
            End Get
            Set(ByVal value As Boolean)
                Me._blnSubsidyCode_included = value
            End Set
        End Property

        Private _blnDoseSeq_included As Boolean
        Public Property DoseSeq_included() As Boolean
            Get
                Return Me._blnDoseSeq_included
            End Get
            Set(ByVal value As Boolean)
                Me._blnDoseSeq_included = value
            End Set
        End Property


        Public Sub New()

        End Sub


    End Class

End Namespace

