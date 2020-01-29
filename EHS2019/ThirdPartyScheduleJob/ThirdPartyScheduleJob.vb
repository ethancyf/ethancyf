Imports Common.Component
Imports Common.DataAccess
Imports CommonScheduleJob.Logger

Public Class ThirdPartyScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return Common.Component.ScheduleJobID.ThirdPartyScheduleJob
        End Get
    End Property

    Protected Overrides Sub Process()
        ' Process PCD Enrolment Record (Send enrolment information to PCD)
        Job.PCDEnrollRecord.Start(Me)
    End Sub
End Class
