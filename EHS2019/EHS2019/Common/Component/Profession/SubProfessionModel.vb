Imports Common.Component.ProfessionPracticeType
Imports Common.ComFunction.GeneralFunction


' CRE12-001 eHS and PCD integration [Start][Koala]
' -----------------------------------------------------------------------------------------

Namespace Component.Profession
    <Serializable()> Public Class SubProfessionModel

        Private _strServiceCategoryCode As String
        Private _strSubServiceCategoryCode As String
        Private _strSubServiceCategoryDesc As String
        Private _strSubServiceCategoryDescChi As String
        Private _intDisplaySeq As Integer

        Public Property ServiceCategoryCode() As String
            Get
                Return _strServiceCategoryCode
            End Get
            Set(ByVal value As String)
                _strServiceCategoryCode = value
            End Set
        End Property

        Public Property SubServiceCategoryCode() As String
            Get
                Return _strSubServiceCategoryCode
            End Get
            Set(ByVal value As String)
                _strSubServiceCategoryCode = value
            End Set
        End Property

        Public Property SubServiceCategoryDesc() As String
            Get
                Return _strSubServiceCategoryDesc
            End Get
            Set(ByVal value As String)
                _strSubServiceCategoryDesc = value
            End Set
        End Property

        Public Property SubServiceCategoryDescChi() As String
            Get
                Return _strSubServiceCategoryDescChi
            End Get
            Set(ByVal value As String)
                _strSubServiceCategoryDescChi = value
            End Set
        End Property

        Public Property DisplaySeq() As Integer
            Get
                Return _intDisplaySeq
            End Get
            Set(ByVal value As Integer)
                _intDisplaySeq = value
            End Set
        End Property


        Public Sub New(ByVal strServiceCategoryCode As String, ByVal strSubServiceCategoryCode As String, _
                       ByVal strSubServiceCategoryDesc As String, ByVal strSubServiceCategoryDescChi As String, ByVal intDisplaySeq As Integer)
            _strServiceCategoryCode = strServiceCategoryCode
            _strSubServiceCategoryCode = strSubServiceCategoryCode
            _strSubServiceCategoryDesc = strSubServiceCategoryDesc
            _strSubServiceCategoryDescChi = strSubServiceCategoryDescChi
            _intDisplaySeq = intDisplaySeq
        End Sub

        Public Sub New(ByVal udtSubProfessionModel As SubProfessionModel)
            _strServiceCategoryCode = udtSubProfessionModel.ServiceCategoryCode
            _strSubServiceCategoryCode = udtSubProfessionModel.SubServiceCategoryCode
            _strSubServiceCategoryDesc = udtSubProfessionModel.SubServiceCategoryDesc
            _strSubServiceCategoryDescChi = udtSubProfessionModel.SubServiceCategoryDescChi
            _intDisplaySeq = udtSubProfessionModel.DisplaySeq
        End Sub

    End Class
End Namespace

' CRE12-001 eHS and PCD integration [End][Koala]

