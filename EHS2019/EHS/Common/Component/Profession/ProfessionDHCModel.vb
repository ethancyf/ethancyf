' CRE19-006 (DHC) [Start][Winnie]
' ----------------------------------------------------------------------------------------
Namespace Component.Profession
    <Serializable()> Public Class ProfessionDHCModel

        Private _strServiceCategoryCode As String
        Private _intMaxClaimAmt As Integer

        Public Const Service_Category_Code As String = "Service_Category_Code"
        Public Const Claim_Amt_Max As String = "Claim_Amt_Max"

        Public Property ServiceCategoryCode() As String
            Get
                Return _strServiceCategoryCode
            End Get
            Set(ByVal value As String)
                _strServiceCategoryCode = value
            End Set
        End Property

        Public Property MaxClaimAmt() As Integer
            Get
                Return _intMaxClaimAmt
            End Get
            Set(value As Integer)
                _intMaxClaimAmt = value
            End Set
        End Property


        Public Sub New(ByVal strServiceCategoryCode As String, ByVal intMaxClaimAmt As Integer)
            _strServiceCategoryCode = strServiceCategoryCode
            _intMaxClaimAmt = intMaxClaimAmt
        End Sub

        Public Sub New(ByVal udtProfessionDHC As ProfessionDHCModel)
            _strServiceCategoryCode = udtProfessionDHC.ServiceCategoryCode
            _intMaxClaimAmt = udtProfessionDHC.MaxClaimAmt
        End Sub

    End Class
End Namespace

' CRE19-006 (DHC) [End][Winnie]


