'Public Class SystemMaintenanceModel
'    Public Property SID As String
'    Public Property ExpireDTM As DateTime
'    Public Property EN_Date As String
'    Public Property ZH_Date As String
'    Public Property CN_Date As String
'    Public Property EN_Time As String
'    Public Property ZH_Time As String
'    Public Property CN_Time As String
'    Public Property Type As String
'End Class

Public Class SystemMaintenanceModel
    Public Property SID As String
    Public Property ShowDate As String
    Public Property ShowTime As String
    Public Property Type As String
End Class

Public Class SystemMaintenanceViewModel
    Public Property MonthlyModelList As List(Of SystemMaintenanceModel)
    Public Property UrgentLyModelList As List(Of SystemMaintenanceModel)
End Class
