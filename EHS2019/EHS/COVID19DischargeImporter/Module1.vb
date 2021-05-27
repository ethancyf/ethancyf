Module Module1

    Sub Main()
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

End Module


Public Class ScheduleJob
    Inherits CommonScheduleJob.BaseScheduleJob

    ''' <summary>

    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property ScheduleJobID() As String
        Get
            Return Common.Component.ScheduleJobID.COVID19DischargeImporter
        End Get
    End Property

    Protected Overrides ReadOnly Property FunctionCode() As String
        Get
            Return Common.Component.ScheduleJobID.COVID19DischargeImporter
        End Get
    End Property


    ''' <summary>
    ''' Main process of schedule job
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub Process()
        ProgramMgr.GetInstance().StartCOVID19DischargeProcess()
    End Sub




End Class