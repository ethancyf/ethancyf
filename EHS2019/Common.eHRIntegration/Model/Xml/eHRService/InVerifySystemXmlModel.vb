Imports System.Xml.Serialization
Imports Common.eHRIntegration.BLL.eHRServiceBLL

Namespace Model.Xml.eHRService

    <XmlRoot("returnObj")>
    Public Class InVerifySystemXmlModel

        Public Status As String
        Public StatusDescription As String
        Public VerificationPass As String
        Public VerificationPassExpiry As String
        Public VerificationPassIssueTime As String

        Public ReadOnly Property StatusEnum As enumResultStatus
            Get
                Dim eResultStatus As enumResultStatus = Nothing

                If [Enum].TryParse(String.Format("R{0}", Status), eResultStatus) = False Then
                    Throw New InvalidOperationException(String.Format("InVerifySystemXmlModel.StatusEnum: Unexpected value (Status={0})", Status))
                End If

                Return eResultStatus

            End Get
        End Property

        Public Sub New()
            Status = String.Empty
            StatusDescription = String.Empty
            VerificationPass = String.Empty
            VerificationPassExpiry = String.Empty
            VerificationPassIssueTime = String.Empty
        End Sub

    End Class

End Namespace
