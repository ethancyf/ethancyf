Module Module1

    Sub Main()
        ' CRE11-006
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

End Module

''' <summary>
''' CRE11-006
''' </summary>
''' <remarks></remarks>
Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

    ''' <summary>
    ''' CRE11-006
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return Common.Component.ScheduleJobID.ImmdValidation
        End Get
    End Property

    ''' <summary>
    ''' CRE11-006
    ''' Main process of schedule job
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Process()
        ProgramMgr.GetInstance().StartImmDProcess()
    End Sub

End Class
