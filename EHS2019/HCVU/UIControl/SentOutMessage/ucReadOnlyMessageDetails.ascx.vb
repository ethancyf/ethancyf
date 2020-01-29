' [CRE12-012] Infrastructure on Sending Messages through eHealth System Inbox

Imports System.Text
Imports Common.Component.Profession
Imports Common.Component.Scheme
Imports Common.Component.SentOutMessage
Imports Common.Format

Partial Public Class ucReadOnlyMessageDetails
    Inherits System.Web.UI.UserControl

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ' Load message method for [New Message] creation
    Public Sub LoadMessage(ByVal udtSentOutMessageModel As SentOutMessageModel, ByVal udtMessageTemplateModel As MessageTemplateModel)
        trTemplateID.Visible = True
        trMessageID.Visible = False
        trStatus.Visible = False
        trCreatedBy.Visible = False

        lblTemplateID.Text = udtMessageTemplateModel.MsgTemplateID

        LoadMessage(udtSentOutMessageModel)
    End Sub

    ' Load message method for the existing [SentOutMessage]
    Public Sub LoadMessage(ByVal udtSentOutMessageModel As SentOutMessageModel)
        Dim udtSentOutMsgRecipientModelCollection As SentOutMsgRecipientModelCollection
        Dim udtSentOutMsgRecipientModel As SentOutMsgRecipientModel
        Dim udtProfessionModel As ProfessionModel
        Dim udtSchemeBackOfficeBLL As SchemeBackOfficeBLL = New SchemeBackOfficeBLL()
        Dim udtFormatter As Formatter = New Formatter()
        Dim dtRecipient As DataTable
        Dim dcRecipient As DataColumn
        Dim drRecipient As DataRow

        lblRecipient.Text = ""
        lblRecipient.Visible = False

        dtRecipient = New DataTable()

        dcRecipient = New DataColumn("SOMR_Profession_DisplayText")
        dcRecipient.DataType = Type.GetType("System.String")
        dtRecipient.Columns.Add(dcRecipient)

        dcRecipient = New DataColumn("SOMR_Scheme_DisplayText")
        dcRecipient.DataType = Type.GetType("System.String")
        dtRecipient.Columns.Add(dcRecipient)

        udtSentOutMsgRecipientModelCollection = udtSentOutMessageModel.SentOutMsgRecipients
        'strRecipient.Append("<ul style='margin:0'>")
        For Each udtSentOutMsgRecipientModel In udtSentOutMsgRecipientModelCollection

            'strRecipient.Append("<li>")

            ' If the recipient is [All Enrolled Service Provider]
            If udtSentOutMsgRecipientModel.Profession = SentOutMsgRecipientModel.PROFESSION_NA AndAlso udtSentOutMsgRecipientModel.Scheme = SentOutMsgRecipientModel.SCHEME_NA Then

                lblRecipient.Text = GetGlobalResourceObject("Text", "AllEnrolledSP")
                lblRecipient.Visible = True
                dtRecipient = Nothing
                Exit For

                ' If the recipient is not [All Enrolled Service Provider]
            Else

                drRecipient = dtRecipient.NewRow()

                If udtSentOutMsgRecipientModel.Profession = SentOutMsgRecipientModel.PROFESSION_NA Then
                    drRecipient("SOMR_Profession_DisplayText") = GetGlobalResourceObject("Text", "Any")
                Else
                    udtProfessionModel = ProfessionBLL.GetProfessionListByServiceCategoryCode(udtSentOutMsgRecipientModel.Profession)
                    drRecipient("SOMR_Profession_DisplayText") = udtProfessionModel.ServiceCategoryDesc
                End If

                If udtSentOutMsgRecipientModel.Scheme = SentOutMsgRecipientModel.SCHEME_NA Then
                    drRecipient("SOMR_Scheme_DisplayText") = GetGlobalResourceObject("Text", SentOutMsgRecipientModel.SCHEME_NA)
                Else
                    drRecipient("SOMR_Scheme_DisplayText") = udtSchemeBackOfficeBLL.GetSchemeBackOfficeDisplayCodeFromSchemeCode(udtSentOutMsgRecipientModel.Scheme)
                End If

                dtRecipient.Rows.Add(drRecipient)

            End If
            'strRecipient.Append("</li>")

        Next
        'strRecipient.Append("</ul>")

        ' If the recipient is [All Enrolled Service Provider]
        If dtRecipient Is Nothing Then

            gvRecipient.DataSource = Nothing

            ' If the recipient is not [All Enrolled Service Provider]
        Else

            gvRecipient.DataSource = dtRecipient.DefaultView

        End If
        gvRecipient.DataBind()

        ' If the [SentOutMessage] is created
        If Not udtSentOutMessageModel.CreateDtm = Nothing Then
            lblMessageID.Text = udtSentOutMessageModel.SentOutMsgID
            lblStatus.Text = udtSentOutMessageModel.GetRecordStatusDisplayText()
            lblCreatedBy.Text = udtSentOutMessageModel.CreateBy
            lblCreatedDateTime.Text = "(" & udtFormatter.convertDateTime(udtSentOutMessageModel.CreateDtm) & ")"

            ' If the status of [SentOutMessage] is [Ready to Send] or [Sent]
            If udtSentOutMessageModel.RecordStatus = SentOutMessageModel.SO_MSG_RECORD_STATUS_T OrElse udtSentOutMessageModel.RecordStatus = SentOutMessageModel.SO_MSG_RECORD_STATUS_S Then
                lblApprovedBy.Text = udtSentOutMessageModel.ConfirmBy
                lblApprovedDateTime.Text = "(" & udtFormatter.convertDateTime(udtSentOutMessageModel.ConfirmDtm) & ")"
                trApprovedBy.Visible = True

                ' If the status of [SentOutMessage] is [Sent]
                If udtSentOutMessageModel.RecordStatus = SentOutMessageModel.SO_MSG_RECORD_STATUS_S Then
                    lblSentDateTime.Text = udtFormatter.convertDateTime(udtSentOutMessageModel.SentDtm)
                    trSentDateTime.Visible = True
                Else
                    lblSentDateTime.Text = ""
                    trSentDateTime.Visible = False
                End If
            Else
                lblApprovedBy.Text = ""
                lblApprovedDateTime.Text = ""
                trApprovedBy.Visible = False
            End If

            ' If the status of [SentOutMessage] is [Rejected]
            If udtSentOutMessageModel.RecordStatus = SentOutMessageModel.SO_MSG_RECORD_STATUS_R Then
                lblRejectedReason.Text = "(" & udtSentOutMessageModel.RejectReason & ")"
                lblRejectedBy.Text = udtSentOutMessageModel.RejectBy
                lblRejectedDateTime.Text = "(" & udtFormatter.convertDateTime(udtSentOutMessageModel.RejectDtm) & ")"
                trRejectedBy.Visible = True
            Else
                lblRejectedReason.Text = ""
                lblRejectedBy.Text = ""
                lblRejectedDateTime.Text = ""
                trRejectedBy.Visible = False
            End If
        End If

        lblCategory.Text = udtSentOutMessageModel.GetSentOutMsgCategoryDisplayText()
        lblSubject.Text = udtSentOutMessageModel.SentOutMsgSubject
        lblContent.Text = udtSentOutMessageModel.SentOutMsgContent
    End Sub

    ' Reset all status of this User Control
    Public Sub ResetAll()
        trTemplateID.Visible = False
        trMessageID.Visible = True
        trStatus.Visible = True
        trSentDateTime.Visible = False
        trCreatedBy.Visible = True
        trApprovedBy.Visible = False
        trRejectedBy.Visible = False

        lblTemplateID.Text = ""
        lblMessageID.Text = ""
        lblStatus.Text = ""
        lblSentDateTime.Text = ""
        lblRejectedReason.Text = ""
        lblCreatedBy.Text = ""
        lblApprovedBy.Text = ""
        lblApprovedDateTime.Text = ""
        lblRejectedBy.Text = ""
        lblRejectedDateTime.Text = ""
        lblCreatedDateTime.Text = ""
        lblCategory.Text = ""
        lblRecipient.Visible = False
        gvRecipient.DataSource = Nothing
        gvRecipient.DataBind()
        lblRecipient.Text = ""
        lblSubject.Text = ""
        lblContent.Text = ""
    End Sub

End Class
