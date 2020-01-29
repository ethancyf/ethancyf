'Integration Start
Namespace Component.ProfessionPracticeType

    <Serializable()> _
    Public Class ProfessionPracticeTypeModel

#Region "Property"

        Private _strServiceCategoryCode As String
        Public ReadOnly Property ServiceCategoryCode() As String
            Get
                Return _strServiceCategoryCode
            End Get
        End Property

        Private _strItemNo As String
        Public ReadOnly Property ItemNo() As String
            Get
                Return _strItemNo
            End Get
        End Property

        Public ReadOnly Property Name() As String
            Get
                If Threading.Thread.CurrentThread.CurrentCulture.Name.ToLower = "zh-tw" Then
                    Return NameChi
                Else
                    Return NameEng
                End If
            End Get
        End Property

        Public ReadOnly Property NameEng() As String
            Get
                Return PracticeTypePCD.DataValue
            End Get
        End Property

        Public ReadOnly Property NameChi() As String
            Get
                Return PracticeTypePCD.DataValueChi
            End Get
        End Property

        Private _objPracticeTypePCD As PracticeType_PCD.PracticeType_PCDModel
        Protected ReadOnly Property PracticeTypePCD() As PracticeType_PCD.PracticeType_PCDModel
            Get
                LoadStaticData()
                Return _objPracticeTypePCD
            End Get
        End Property

#End Region

#Region "Constructor"

        Public Sub New(ByVal strServiceCategoryCode As String, ByVal strItemNo As String)
            _strServiceCategoryCode = strServiceCategoryCode
            _strItemNo = strItemNo
        End Sub

#End Region

#Region "Support Function"

        Private Sub LoadStaticData()
            If _objPracticeTypePCD Is Nothing Then
                _objPracticeTypePCD = Component.PracticeType_PCD.PracticeType_PCDBLL.GetPracticeTypeByCode(ItemNo)
            End If
        End Sub

#End Region

    End Class
End Namespace

'Integration End