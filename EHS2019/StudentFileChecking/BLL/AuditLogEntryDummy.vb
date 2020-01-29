Imports Common.Component
Imports Common.Component.EHSAccount.EHSAccountModel

Namespace BLL

    Public Class AuditLogEntryDummy
        Inherits Common.ComObject.AuditLogEntry

        Public Overrides ReadOnly Property Platform As String
            Get
                ' Use non-exist platform to avoid unnecessary logging to AudtiLogHCVS table
                Return "DUMMY"
            End Get
        End Property

        Public Sub New(ByVal strFunctionCode As String)
            MyBase.New(strFunctionCode)
        End Sub
    End Class
End Namespace
