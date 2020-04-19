Imports System.Web.Mvc
Imports System.ComponentModel.DataAnnotations

Public Class SPSViewModel
    Public Property ProfessionList As List(Of ProfessionList) = New List(Of ProfessionList)
    Public Property SchemeList As List(Of SchemeList) = New List(Of SchemeList)
    Public Property AreaList As List(Of AreaList) = New List(Of AreaList)
    Public Property PointToNoteList As List(Of PointToNoteList) = New List(Of PointToNoteList)
    Public Property selectedProfession As String
    Public Property selectedScheme As String
    Public Property selectedDistrict As String
    Public Property InputServiceProviderName As String
    Public Property InputPracticeName As String
    Public Property InputPracticeAddress As String
    Public Property IsValid As Boolean
    Public Property lstErrorCodes As List(Of String) = New List(Of String)
    Public Property ResultList As List(Of SPResultModel) = New List(Of SPResultModel)
    Public Property RecordTotal As Integer
    Public Property PageSize As Integer
    Public Property PageActualSize As Integer
    Public Property PageIndex As Integer
    Public Property PageTotal As Integer
    Public Property SortField As String
    Public Property SortColName As String
    Public Property SortFieldDesc As String
    Public Property SortType As String 'ASC DESC
    Public Property RequestType As String
    Public Property queryLang As String
    Public Property HasVSS As Boolean
    Public Property ActionReason As String
    Public Property IsReset As Boolean
    Public Property HasResult As Boolean

    ' by James
    Public Property HeaderList As List(Of SchemeHeader) = New List(Of SchemeHeader)
    Public Property SubHeaderList As List(Of SchemeItemHeader) = New List(Of SchemeItemHeader)

    'For Form input value
    Public Property selectedProfessionByForm As String
    Public Property selectedSchemeByForm As String
    Public Property selectedDistrictByForm As String
    Public Property InputServiceProviderNameByForm As String
    Public Property InputPracticeNameByForm As String
    Public Property InputPracticeAddressByForm As String
    'For Selected Tab
    Public Property SelectedTab As String

    Public Property LastUpdateDate As String
    Public Property PageSizeList As List(Of Integer)
End Class

Public Class SchemeHeader
    Public Property Header As String
    Public Property ColSpan As Integer = 1
End Class
Public Class SchemeItemHeader
    Public Property Header As String
    Public Property SortItem As String
    Public Property SubsidizeFeeColumnName As String
End Class
Public Class HeaderList
    Public Property Title As String
    Public Property Count As Integer
End Class

Public Class ProfessionList
    Inherits SelectListItem
    Public Property ProfessionSeqNo As Integer
    Public Property ProfessionList As IEnumerable(Of SelectListItem)
    Public Property EligibleScheme As String
End Class

Public Class SchemeList
    Inherits SelectListItem
    Public Property SchemeCode As String
    Public Property SchemeDesc As String
    Public Property SchemeUrl As String
    Public Property SubsidyList As IEnumerable(Of SelectListItem)
    Public Property SubsidizeItemList As List(Of SubsidizeItemList)
End Class

Public Class AreaList
    Inherits SelectListItem
    Public Property DistrictBoardCode As String
    Public Property DistrictBoardDesc As String
    Public Property DistrictBoardList As List(Of DistrictBoardList)
End Class

Public Class DistrictBoardList
    Inherits SelectListItem
    Public Property SeqNo As Integer
    Public Property DistrictBoardList As IEnumerable(Of SelectListItem)
    Public Property SubsidyCode As String
    Public Property SubsidizeFeeColumnName As String
End Class

Public Class PointToNoteList
    Public Property SeqNo As Integer
    Public Property SchemeCode As String
    Public Property NoteDesc As String
End Class

Public Class SubsidizeItemList
    Public Property SubsidizeItemCode As String
    Public Property CategoryCode As String
    Public Property CategoryDesc As String
    Public Property SearchGroup As String
    Public Property SubsidizeShortForm As String
    Public Property SubsidizeFeeColumnName As String
End Class

Public Class EligibleSchemeList
    Public Property ProfessionCode As String
    Public Property EligibleScheme As String
End Class