Imports System.Data.SqlClient

Namespace Component.ClaimCategory

    <Serializable()> Public Class ClaimCategoryModel
        Implements IComparable
        Implements IComparer

        Public Const _Category_Code As String = "Category_Code"
        Public Const _Category_Name As String = "Category_Name"
        Public Const _Category_Name_Chi As String = "Category_Name_Chi"
        Public Const _Category_Name_CN As String = "Category_Name_CN"
        Public Const _IsMedicalCondition As String = "IsMedicalCondition"

#Region "Private Member"

        Private _strScheme_Code As String
        Private _intScheme_Seq As Integer
        Private _strSubsidize_Code As String

        Private _strCategory_Code As String
        Private _strCategory_Name As String
        Private _strCategory_Name_Chi As String
        Private _strCategory_Name_CN As String
        Private _intDisplay_Seq As Integer

        Private _strIsMedicalCondition As String

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

        Public Property CategoryCode() As String
            Get
                Return Me._strCategory_Code
            End Get
            Set(ByVal value As String)
                Me._strCategory_Code = value
            End Set
        End Property

        Public Property CategoryName() As String
            Get
                Return Me._strCategory_Name
            End Get
            Set(ByVal value As String)
                Me._strCategory_Name = value
            End Set
        End Property

        Public Property CategoryNameChi() As String
            Get
                Return Me._strCategory_Name_Chi
            End Get
            Set(ByVal value As String)
                Me._strCategory_Name_Chi = value
            End Set
        End Property

        Public Property CategoryNameCN() As String
            Get
                Return Me._strCategory_Name_CN
            End Get
            Set(ByVal value As String)
                Me._strCategory_Name_CN = value
            End Set
        End Property

        Public Property DisplaySeq() As Integer
            Get
                Return Me._intDisplay_Seq
            End Get
            Set(ByVal value As Integer)
                Me._intDisplay_Seq = value
            End Set
        End Property

        Public Property IsMedicalCondition() As String
            Get
                Return Me._strIsMedicalCondition
            End Get
            Set(ByVal value As String)
                Me._strIsMedicalCondition = value
            End Set
        End Property
#End Region

#Region "Constructor"

        Public Sub New()
        End Sub

        Public Sub New(ByVal udtClaimCategoryModel As ClaimCategoryModel)

            Me._strScheme_Code = udtClaimCategoryModel._strScheme_Code
            Me._intScheme_Seq = udtClaimCategoryModel._intScheme_Seq
            Me._strSubsidize_Code = udtClaimCategoryModel._strSubsidize_Code
            Me._strCategory_Code = udtClaimCategoryModel._strCategory_Code
            Me._strCategory_Name = udtClaimCategoryModel._strCategory_Name

            Me._strCategory_Name_Chi = udtClaimCategoryModel._strCategory_Name_Chi
            Me._strCategory_Name_CN = udtClaimCategoryModel._strCategory_Name_CN
            Me._intDisplay_Seq = udtClaimCategoryModel._intDisplay_Seq

            Me._strIsMedicalCondition = udtClaimCategoryModel._strIsMedicalCondition
        End Sub

#End Region

        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare
            If x.GetType Is GetType(ClaimCategoryModel) AndAlso y.GetType Is GetType(ClaimCategoryModel) Then
                Return CType(x, ClaimCategoryModel).DisplaySeq.CompareTo(CType(y, ClaimCategoryModel).DisplaySeq)
            Else
                If x.GetType Is GetType(ClaimCategoryModel) Then
                    Return -1
                End If
                If y.GetType Is GetType(ClaimCategoryModel) Then
                    Return 1
                End If
                Return 0
            End If
        End Function

        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
            If obj.GetType Is GetType(ClaimCategoryModel) Then
                Return Me.DisplaySeq.CompareTo(CType(obj, ClaimCategoryModel).DisplaySeq)
            Else
                Return -1
            End If
        End Function

        '

        Public Function GetCategoryName(Optional ByVal strLanguage As String = "") As String
            Select Case strLanguage
                Case CultureLanguage.TradChinese
                    Return CategoryNameChi
                Case CultureLanguage.SimpChinese
                    Return CategoryNameCN
                Case Else
                    Return CategoryName
            End Select
        End Function

    End Class
End Namespace