Module Module1

    ''' <summary>
    ''' CRE11-014
    ''' Update to use CommonScheduleJob to maintain schedule job framework
    ''' </summary>
    ''' <remarks></remarks>
    Sub Main()
        Dim objScheduleJob As New ScheduleJob
        objScheduleJob.Start()
    End Sub

    Public Class ScheduleJob
        Inherits CommonScheduleJob.BaseScheduleJob


        Public Overrides ReadOnly Property ScheduleJobID() As String
            Get
                Return Common.Component.ScheduleJobID.ExcelGenerator
            End Get
        End Property

        Protected Overrides Sub Process()
            GeneratorMgr.GetInstance().ProcessExcelGenerationQueues()
        End Sub
    End Class

End Module
