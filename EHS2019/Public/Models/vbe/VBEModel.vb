Public Class VBEResult
    Public Property HKIC_No As String = "A123XXX(X)"
    Public Property DateOfBirth As String = "2019/01/01"
    Public Property AvailableAmount As Decimal = 4000
    Public Property MaximumOptometry As Decimal = 2000
    Public Property ProjectedAvailableAmount As Decimal = 2000
    Public Property ForfeitedAmount As Decimal = 0
    Public Property AvailableQuotaOptometry As Decimal = 2000
    Public Property PrjPosExceedLimit As Decimal = 8000
    Public Property UpToDate As String

    Public Property DateType As String
    Public Property Age As Integer
    Public Property Day As Integer
    Public Property Month As Integer
    Public Property Year As Integer

    Public Property VBERequestData As VBERequest
    'Dim lstErrCodes As List(Of String)
End Class

Public Class VBERequest
    Public Property HKICNo As String = ""
    Public Property DateOfBirth As String = ""
    'IC/CE
    Public Property InputType As String
    Public Property DateOfBirth_IC As String = ""
    Public Property DateOfBirth_CE As String = ""
    'DOB/YOB
    Public Property DateType As String
    Public Property Age As Integer
    Public Property Day As Integer
    Public Property Month As Integer
    Public Property Year As Integer

    Public Property Captcha As String = ""
    Public Property lang As String = "en"
End Class

Public Class VBEValidateResult
    Public Property VBERequestData As VBERequest
    Public Property lstErrCodes As List(Of String)
    Public Property returnValue As Boolean
End Class