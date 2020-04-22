Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
Imports Common.Format
Imports common.ComFunction
Imports Common.Validation


Public Class spEnrolmentPrintOutEHCVS
    Private Enrol_Ref_No As String
    Private TokenSerialNo As String
    Private SP_ID As String
    Private formatter As New Formatter


    Public Sub New(ByVal strEnrol_Ref_No As String, ByVal strTokenSerialNo As String, ByVal strSP_ID As String)

        Enrol_Ref_No = strEnrol_Ref_No
        TokenSerialNo = strTokenSerialNo
        SP_ID = strSP_ID

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.DataSource = Nothing

    End Sub

    Private Sub spTokenManagementPrintOut_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart

        Dim udtTokenManagementBLL As TokenManagement.TokenManagementBLL = New TokenManagement.TokenManagementBLL
        Dim dt As New DataTable
        Dim txtAppLink As String = String.Empty
        Dim txtEmail As String = String.Empty
        Dim txtActivationWeeks As String = String.Empty
        Dim GeneralFunction As GeneralFunction = New GeneralFunction

        'GeneralFunction.getSystemParameter("AppLink", txtAppLink, String.Empty)
        txtEmail = GeneralFunction.getUserDefinedParameter("Printout", "HCVUEmail")
        GeneralFunction.getSystemParameter("ActivationPeriod", txtActivationWeeks, String.Empty)

        dt = udtTokenManagementBLL.EnrollmentPrintOut(Enrol_Ref_No, TokenSerialNo, SP_ID)

        Me.DataSource = dt

        'Me.lblPrintDetail.Text = String.Format("Print on {0}", formatter.formatDateTime(DateTime.Now(), "us-en"))
        'Me.txtboxWebsite.Text = txtAppLink
        'Me.txtboxWebsite2.Text = txtAppLink
        Me.txtboxHCVUEmail.Text = txtEmail
        Me.txtboxHCVUEmail2.Text = txtEmail
        Me.txtboxActivationWeeks.Text = txtActivationWeeks
        Me.txtboxActivationWeeks2.Text = txtActivationWeeks
    End Sub

    'Private Sub PageFooter1_Format(ByVal sender As Object, ByVal e As System.EventArgs) Handles PageFooter1.Format
    '    PageFooter1.Visible = False
    '    'If Me.txtPageNo.Text = 1 Then
    '    '    Me.lblPageText.Text = "Page"
    '    '    Me.lblOfText.Text = "of"
    '    '    Me.lblPageNumberOfText.Visible = False
    '    'Else
    '    '    Dim f As New System.Drawing.Font("PMingLiu", 10, Drawing.FontStyle.Regular)

    '    '    Me.txtboxRefNo.Font = f
    '    '    Me.lblPageText.Font = f
    '    '    Me.lblPageText.Text = "第"
    '    '    Me.lblOfText.Font = f
    '    '    Me.lblOfText.Text = "頁，共"
    '    '    Me.lblPageNumberOfText.Font = f
    '    '    Me.lblPageNumberOfText.Text = "頁"
    '    '    Me.lblPageNumberOfText.Visible = True

    '    '    Me.txtPageNo.Font = f
    '    '    Me.txtTotalPageNo.Font = f
    '    'End If


    'End Sub

    Private Sub Detail1_Format(ByVal sender As Object, ByVal e As System.EventArgs) Handles Detail1.Format
        Dim udtValidator As New Validator
        Dim strChiName As String = Me.txtboxSPNameChi.Text.Replace(",", String.Empty)
        If udtValidator.IsValidEngName(strChiName) Then
            Dim f As New System.Drawing.Font("Arial", 12, Drawing.FontStyle.Regular)
            Me.txtboxSPNameChi.Font = f
        End If
    End Sub
End Class
