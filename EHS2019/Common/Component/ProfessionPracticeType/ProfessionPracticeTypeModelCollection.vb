Imports System.Collections.Generic

'Integration Start
Namespace Component.ProfessionPracticeType

    <Serializable()> _
    Public Class ProfessionPracticeTypeModelCollection
        Inherits List(Of ProfessionPracticeType.ProfessionPracticeTypeModel)

        Private _objProfession As Profession.ProfessionModel
        Public Property Profession() As Profession.ProfessionModel
            Get
                Return _objProfession
            End Get
            Set(ByVal value As Profession.ProfessionModel)
                _objProfession = value
            End Set
        End Property

        Public Sub New(ByVal objProfession As Profession.ProfessionModel)
            MyBase.New()

            _objProfession = objProfession
        End Sub

        Public Sub New(ByVal objProfession As Profession.ProfessionModel, ByVal capacity As Integer)
            MyBase.New(capacity)

            _objProfession = objProfession
        End Sub

        Public Function GetByPracticeType(ByVal strPracticeType As String) As ProfessionPracticeTypeModel
            For Each objModel As ProfessionPracticeTypeModel In Me
                If objModel.ItemNo.Trim.ToUpper = strPracticeType.Trim.ToUpper Then
                    Return objModel
                End If
            Next
            Return Nothing
        End Function
    End Class

End Namespace
'Integration End