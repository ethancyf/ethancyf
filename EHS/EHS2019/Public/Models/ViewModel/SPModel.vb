Imports System.Web.Mvc
Imports System.ComponentModel.DataAnnotations

Public Class FeeList
    Public Property FeeDesc As String
    Public Property Fee As String
    Public Property SubsidizeItemCode As String
End Class
Public Class SPRequest
    Public Property InputServiceProviderName As String
    Public Property InputPracticeName As String
    Public Property InputPracticeAddress As String
    Public Property Subsidy As String
    Public Property District As String
    Public Property Profession As String
    Public Property PageSize As Integer
    Public Property PageActualSize As Integer
    Public Property PageIndex As Integer
    Public Property SortField As String

    Public Property SortType As String
    Public Property RequestType As String
    Public Property sortColName As String
    Public Property ActionReason As String
    'Public Property isSearch As Boolean
End Class
Public Class SPResultModel
    Public Property SPName As String
    Public Property SPEngName As String
    Public Property SPChiName As String
    'SPChiName
    Public Property PracticeID As String
    Public Property PracticeName As String
    Public Property PracticeEngName As String
    Public Property PracticeChiName As String
    'PracticeEngName
    'PracticeChiName
    Public Property PracticeAddress As String
    Public Property PracticeEngAddress As String
    Public Property PracticeChiAddress As String
    'PracticeEngAddress
    'PracticeChiAddress
    Public Property PracticePhoneNo As String
    Public Property Profession As String
    Public Property DistrictCode As String
    Public Property DistrictBoardCode As String
    Public Property AreaCode As String
    Public Property DistrictName As String
    Public Property DistrictEngName As String
    Public Property DistrictChiName As String

    Public Property DistrictBoardName As String
    Public Property DistrictBoardEngName As String
    Public Property DistrictBoardChiName As String
    Public Property AreaName As String
    'DistrictChiName
    'DistrictBoardChiName
    'AreaChiName

    Public Property MobileClinic As String

    'Change for New requirement Mobile clinic
    '************************************
    Public Property NonClinic As String
    Public Property Remark As String
    Public Property RemarkDesc As String
    Public Property RemarkEngDesc As String
    Public Property RemarkChiDesc As String
    '************************************

    Public Property JoinedScheme As String

    Public Property PracticeDetail As String
    Public Property FeeList As List(Of FeeList)
    Public Property SubsidizeFeeScope As String
    Public Property SubsidizeFeeEngScope As String
    Public Property SubsidizeFeeChiScope As String
    Public Property PriceTag As String

    ' By James
    Public Property SubsidizeList As List(Of Subsidize) = New List(Of Subsidize)

End Class

Public Class Subsidize
    Public Property Item As String
    Public Property Fee As String
    Public Property Sort As String
    Public Property SortType As String
End Class

Public Class SPSValidateResult
    Public Property SPSRequestData As SPRequest
    Public Property lstErrCodes As List(Of String)
    Public Property returnValue As Boolean
End Class