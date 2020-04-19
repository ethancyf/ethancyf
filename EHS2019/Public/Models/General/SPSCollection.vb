Public Class ProfessionCodeList
    Public Property SeqNo As Integer
    Public Property ProfessionCode As String
    Public Property ProfessionEngDesc As String
    Public Property ProfessionChiDesc As String
    Public Property EligibleScheme As String
End Class

Public Class SchemeCodeList
    Public Property SeqNo As Integer
    Public Property SchemeCode As String
    Public Property SchemeEngDesc As String
    Public Property SchemeChiDesc As String
    Public Property SchemeEngUrl As String
    Public Property SchemeChiUrl As String
    Public Property SubsidyList As List(Of SubsidyCodeList)
    Public Property SubsidizeItemList As List(Of SubsidizeItemCodeList)
End Class

Public Class SubsidyCodeList
    Public Property SeqNo As Integer
    Public Property SubsidyCode As String
    Public Property SubsidyEngDesc As String
    Public Property SubsidyChiDesc As String
End Class

Public Class AreaCodeList
    Public Property AreaCode As Integer
    Public Property AreaEngDesc As String
    Public Property AreaChiDesc As String
    Public Property DistrictBoardList As List(Of DistrictBoardCodeList)
End Class

Public Class DistrictBoardCodeList
    Public Property SeqNo As Integer
    Public Property DistrictBoardCode As String
    Public Property DistrictBoardEngDesc As String
    Public Property DistrictBoardChiDesc As String
End Class

Public Class PointToNoteCodeList
    Public Property SeqNo As Integer
    Public Property SchemeCode As String
    Public Property NoteEngDesc As String
    Public Property NoteChiDesc As String
End Class


Public Class SubsidizeItemCodeList
    Public Property SubsidizeItemCode As String
    Public Property CategoryCode As String
    Public Property CategoryEngDesc As String
    Public Property CategoryChiDesc As String
    Public Property SubsidyCode As String
    Public Property SubsidizeFeeColumnName As String
End Class
