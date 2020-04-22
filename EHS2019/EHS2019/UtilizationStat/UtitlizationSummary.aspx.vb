Imports Common.ComFunction.AccountSecurity

Partial Public Class UtitlizationSummary
    Inherits System.Web.UI.Page

    Private udtReportBLL As New Report.ReportBLL

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Me.lblErr.Text = String.Empty
            lblErr.Visible = False

        End If

    End Sub
    'CRE13-016 Upgrade to excel 2007 [Start][Karl]
    'Protected Sub btnGenerateXLS_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerateXLS.Click
    '    Dim strRes As String = String.Empty

    '    Dim udtDataExport As New DataExportBLL

    '    Try
    '        strRes = udtDataExport.GetData("Enrol")
    '        lbl1.Text = "Path:" + strRes
    '        If strRes.Equals(String.Empty) Then

    '        Else
    '            Response.Redirect("~/EnrolmentStat.aspx?id=" + strRes)
    '        End If

    '    Catch ex As Exception
    '        lbl1.Text = ex.Message
    '    End Try
    'End Sub

    'Private Sub btnMO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnMO.Click
    '    Dim strRes As String = String.Empty

    '    Dim udtDataExport As New DataExportBLL

    '    Try
    '        strRes = udtDataExport.GetData("MO")
    '        lbl1.Text = "Path:" + strRes
    '        If strRes.Equals(String.Empty) Then

    '        Else
    '            Response.Redirect("~/EnrolmentStat.aspx?id=" + strRes)
    '        End If

    '    Catch ex As Exception
    '        lbl1.Text = ex.Message
    '    End Try
    'End Sub

    'Private Sub btnEnrolledPractice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnEnrolledPractice.Click
    '    Dim strRes As String = String.Empty

    '    Dim udtDataExport As New DataExportBLL

    '    Try
    '        strRes = udtDataExport.GetData("EnrolledPractice")
    '        lbl1.Text = "Path:" + strRes
    '        If strRes.Equals(String.Empty) Then

    '        Else
    '            Response.Redirect("~/EnrolmentStat.aspx?id=" + strRes)
    '        End If

    '    Catch ex As Exception
    '        lbl1.Text = ex.Message
    '    End Try
    'End Sub
    'CRE13-016 Upgrade to excel 2007 [End][Karl]
    Protected Sub btnGen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGen.Click
        Dim blnAbleToDownload As Boolean = False
        Dim strPassword As String = String.Empty

        If Me.cboPassword.Checked Then
            If Me.txtPassword.Text.Trim.Equals(String.Empty) Then
                lbl1.Text = "Please input password for downloading excel."
            Else
                blnAbleToDownload = True
                strPassword = Me.txtPassword.Text.Trim
            End If
        Else
            blnAbleToDownload = True
        End If

        If blnAbleToDownload Then
            Dim strReportID As String = Me.ddlReport.SelectedValue

            Dim strRes As String = String.Empty

            Dim udtDataExport As New DataExportBLL

            Try
                strRes = udtDataExport.GetReportData(strReportID, strPassword)
                lbl1.Text = "Path:" + strRes
                If strRes.Equals(String.Empty) Then

                Else
                    Response.Redirect("~/EnrolmentStat.aspx?id=" + strRes)
                End If

            Catch ex As Exception
                lbl1.Text = ex.Message
            End Try
        End If

    End Sub

    Protected Sub btnLogin_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLogin.Click
        Dim blnErr As Boolean = False

        lblErr.Text = String.Empty
        lblErr.Visible = False

        If Me.txtLoginID.Text.Trim.Equals(String.Empty) Then
            blnErr = True
        End If

        If Me.txtLoginPassword.Text.Trim.Equals(String.Empty) Then
            blnErr = True
        End If

        If blnErr Then
            lblErr.Text = "Please input ""Login ID"" or ""Password""."
            lblErr.Visible = True
        Else

            Dim strLoginID As String = ConfigurationManager.AppSettings("LoginID")
            Dim strPassword As String = ConfigurationManager.AppSettings("Password")

            ' I-CRE16-007-02 (Refine system from CheckMarx findings) [Start][Winnie]
            If Not Hash(Me.txtLoginID.Text.Trim.ToUpper).HashedValue.Equals(strLoginID.Trim) Then
                blnErr = True
            Else
                If Not Hash(Me.txtLoginPassword.Text.Trim).HashedValue.Equals(strPassword.Trim) Then
                    blnErr = True
                End If
            End If
            ' I-CRE16-007-02 (Refine system from CheckMarx findings) [End][Winnie]

            If blnErr Then
                lblErr.Text = "Incorrect ""Login ID"", ""Password""."
                lblErr.Visible = True
            Else
                Me.mvReport.ActiveViewIndex = 1
            End If
        End If

    End Sub

    Private Sub mvReport_ActiveViewChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles mvReport.ActiveViewChanged
        Select Case mvReport.ActiveViewIndex
            Case 0
                Me.lblErr.Text = String.Empty
                lblErr.Visible = False

            Case 1

                ddlReport.Items.Clear()

                Dim dtReportList As DataTable = New DataTable

                dtReportList = udtReportBLL.GetReportList()
                Me.ddlReport.DataTextField = "ReportName"
                Me.ddlReport.DataValueField = "ReportID"
                Me.ddlReport.DataSource = dtReportList
                Me.ddlReport.DataBind()


                If Me.cboPassword.Checked Then
                    Me.txtPassword.Enabled = True
                Else
                    Me.txtPassword.Enabled = False
                End If

                lbl1.Text = String.Empty

                ' CRE12-010 Add 'eHS' to the daily zip filename [Start][Koala]
                Me.lnkbtnDaily.Text = "eHS" + DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".zip"
                ' CRE12-010 Add 'eHS' to the daily zip filename [End][Koala]
                Me.lnkbtnDaily.OnClientClick = "javascript:window.open('GeneratedXLS/" + lnkbtnDaily.Text.Trim + "');"

                'CRE13-016 Upgrade to excel 2007 [Start][Karl]
                'Dim strFilePath As String = ConfigurationManager.AppSettings("ReportLink")

                '' Download Voucher Statistic Report (HCVS)
                'Dim strHCVSPathPrefix As String = "eHSD0001-HCVS Claim_"

                'Dim strlnkbtn1_HCVS As String = strFilePath + strHCVSPathPrefix + DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn2_HCVS As String = strFilePath + strHCVSPathPrefix + DateTime.Now.AddDays(-2).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn3_HCVS As String = strFilePath + strHCVSPathPrefix + DateTime.Now.AddDays(-3).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn4_HCVS As String = strFilePath + strHCVSPathPrefix + DateTime.Now.AddDays(-4).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn5_HCVS As String = strFilePath + strHCVSPathPrefix + DateTime.Now.AddDays(-5).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn6_HCVS As String = strFilePath + strHCVSPathPrefix + DateTime.Now.AddDays(-6).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn7_HCVS As String = strFilePath + strHCVSPathPrefix + DateTime.Now.AddDays(-7).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn8_HCVS As String = strFilePath + strHCVSPathPrefix + DateTime.Now.AddDays(-8).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn9_HCVS As String = strFilePath + strHCVSPathPrefix + DateTime.Now.AddDays(-9).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn10_HCVS As String = strFilePath + strHCVSPathPrefix + DateTime.Now.AddDays(-10).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"


                'Me.lnkbtn1_HCVS.OnClientClick = "javascript:window.open('" + strlnkbtn1_HCVS.Trim + "');"
                'Me.lnkbtn2_HCVS.OnClientClick = "javascript:window.open('" + strlnkbtn2_HCVS.Trim + "');"
                'Me.lnkbtn3_HCVS.OnClientClick = "javascript:window.open('" + strlnkbtn3_HCVS.Trim + "');"
                'Me.lnkbtn4_HCVS.OnClientClick = "javascript:window.open('" + strlnkbtn4_HCVS.Trim + "');"
                'Me.lnkbtn5_HCVS.OnClientClick = "javascript:window.open('" + strlnkbtn5_HCVS.Trim + "');"
                'Me.lnkbtn6_HCVS.OnClientClick = "javascript:window.open('" + strlnkbtn6_HCVS.Trim + "');"
                'Me.lnkbtn7_HCVS.OnClientClick = "javascript:window.open('" + strlnkbtn7_HCVS.Trim + "');"
                'Me.lnkbtn8_HCVS.OnClientClick = "javascript:window.open('" + strlnkbtn8_HCVS.Trim + "');"
                'Me.lnkbtn9_HCVS.OnClientClick = "javascript:window.open('" + strlnkbtn9_HCVS.Trim + "');"
                'Me.lnkbtn10_HCVS.OnClientClick = "javascript:window.open('" + strlnkbtn10_HCVS.Trim + "');"

                'Me.lnkbtn1_HCVS.Text = strHCVSPathPrefix + DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn2_HCVS.Text = strHCVSPathPrefix + DateTime.Now.AddDays(-2).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn3_HCVS.Text = strHCVSPathPrefix + DateTime.Now.AddDays(-3).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn4_HCVS.Text = strHCVSPathPrefix + DateTime.Now.AddDays(-4).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn5_HCVS.Text = strHCVSPathPrefix + DateTime.Now.AddDays(-5).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn6_HCVS.Text = strHCVSPathPrefix + DateTime.Now.AddDays(-6).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn7_HCVS.Text = strHCVSPathPrefix + DateTime.Now.AddDays(-7).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn8_HCVS.Text = strHCVSPathPrefix + DateTime.Now.AddDays(-8).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn9_HCVS.Text = strHCVSPathPrefix + DateTime.Now.AddDays(-9).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn10_HCVS.Text = strHCVSPathPrefix + DateTime.Now.AddDays(-10).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"

                '' Download Vaccination Statistic Report (EVSS)
                'Dim strEVSSPathPrefix As String = "eHSD0002-EVSS Claim_"

                'Dim strlnkbtn1_EVSS As String = strFilePath + strEVSSPathPrefix + DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn2_EVSS As String = strFilePath + strEVSSPathPrefix + DateTime.Now.AddDays(-2).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn3_EVSS As String = strFilePath + strEVSSPathPrefix + DateTime.Now.AddDays(-3).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn4_EVSS As String = strFilePath + strEVSSPathPrefix + DateTime.Now.AddDays(-4).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn5_EVSS As String = strFilePath + strEVSSPathPrefix + DateTime.Now.AddDays(-5).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn6_EVSS As String = strFilePath + strEVSSPathPrefix + DateTime.Now.AddDays(-6).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn7_EVSS As String = strFilePath + strEVSSPathPrefix + DateTime.Now.AddDays(-7).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn8_EVSS As String = strFilePath + strEVSSPathPrefix + DateTime.Now.AddDays(-8).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn9_EVSS As String = strFilePath + strEVSSPathPrefix + DateTime.Now.AddDays(-9).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn10_EVSS As String = strFilePath + strEVSSPathPrefix + DateTime.Now.AddDays(-10).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"

                'Me.lnkbtn1_EVSS.OnClientClick = "javascript:window.open('" + strlnkbtn1_EVSS.Trim + "');"
                'Me.lnkbtn2_EVSS.OnClientClick = "javascript:window.open('" + strlnkbtn2_EVSS.Trim + "');"
                'Me.lnkbtn3_EVSS.OnClientClick = "javascript:window.open('" + strlnkbtn3_EVSS.Trim + "');"
                'Me.lnkbtn4_EVSS.OnClientClick = "javascript:window.open('" + strlnkbtn4_EVSS.Trim + "');"
                'Me.lnkbtn5_EVSS.OnClientClick = "javascript:window.open('" + strlnkbtn5_EVSS.Trim + "');"
                'Me.lnkbtn6_EVSS.OnClientClick = "javascript:window.open('" + strlnkbtn6_EVSS.Trim + "');"
                'Me.lnkbtn7_EVSS.OnClientClick = "javascript:window.open('" + strlnkbtn7_EVSS.Trim + "');"
                'Me.lnkbtn8_EVSS.OnClientClick = "javascript:window.open('" + strlnkbtn8_EVSS.Trim + "');"
                'Me.lnkbtn9_EVSS.OnClientClick = "javascript:window.open('" + strlnkbtn9_EVSS.Trim + "');"
                'Me.lnkbtn10_EVSS.OnClientClick = "javascript:window.open('" + strlnkbtn10_EVSS.Trim + "');"

                'Me.lnkbtn1_EVSS.Text = strEVSSPathPrefix + DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn2_EVSS.Text = strEVSSPathPrefix + DateTime.Now.AddDays(-2).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn3_EVSS.Text = strEVSSPathPrefix + DateTime.Now.AddDays(-3).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn4_EVSS.Text = strEVSSPathPrefix + DateTime.Now.AddDays(-4).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn5_EVSS.Text = strEVSSPathPrefix + DateTime.Now.AddDays(-5).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn6_EVSS.Text = strEVSSPathPrefix + DateTime.Now.AddDays(-6).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn7_EVSS.Text = strEVSSPathPrefix + DateTime.Now.AddDays(-7).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn8_EVSS.Text = strEVSSPathPrefix + DateTime.Now.AddDays(-8).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn9_EVSS.Text = strEVSSPathPrefix + DateTime.Now.AddDays(-9).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn10_EVSS.Text = strEVSSPathPrefix + DateTime.Now.AddDays(-10).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"


                '' Download Vaccination Statistic Report (CIVSS)
                'Dim strCIVSSPathPrefix As String = "eHSD0003-CIVSS Claim_"

                'Dim strlnkbtn1_CIVSS As String = strFilePath + strCIVSSPathPrefix + DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn2_CIVSS As String = strFilePath + strCIVSSPathPrefix + DateTime.Now.AddDays(-2).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn3_CIVSS As String = strFilePath + strCIVSSPathPrefix + DateTime.Now.AddDays(-3).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn4_CIVSS As String = strFilePath + strCIVSSPathPrefix + DateTime.Now.AddDays(-4).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn5_CIVSS As String = strFilePath + strCIVSSPathPrefix + DateTime.Now.AddDays(-5).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn6_CIVSS As String = strFilePath + strCIVSSPathPrefix + DateTime.Now.AddDays(-6).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn7_CIVSS As String = strFilePath + strCIVSSPathPrefix + DateTime.Now.AddDays(-7).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn8_CIVSS As String = strFilePath + strCIVSSPathPrefix + DateTime.Now.AddDays(-8).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn9_CIVSS As String = strFilePath + strCIVSSPathPrefix + DateTime.Now.AddDays(-9).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn10_CIVSS As String = strFilePath + strCIVSSPathPrefix + DateTime.Now.AddDays(-10).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"

                'Me.lnkbtn1_CIVSS.OnClientClick = "javascript:window.open('" + strlnkbtn1_CIVSS.Trim + "');"
                'Me.lnkbtn2_CIVSS.OnClientClick = "javascript:window.open('" + strlnkbtn2_CIVSS.Trim + "');"
                'Me.lnkbtn3_CIVSS.OnClientClick = "javascript:window.open('" + strlnkbtn3_CIVSS.Trim + "');"
                'Me.lnkbtn4_CIVSS.OnClientClick = "javascript:window.open('" + strlnkbtn4_CIVSS.Trim + "');"
                'Me.lnkbtn5_CIVSS.OnClientClick = "javascript:window.open('" + strlnkbtn5_CIVSS.Trim + "');"
                'Me.lnkbtn6_CIVSS.OnClientClick = "javascript:window.open('" + strlnkbtn6_CIVSS.Trim + "');"
                'Me.lnkbtn7_CIVSS.OnClientClick = "javascript:window.open('" + strlnkbtn7_CIVSS.Trim + "');"
                'Me.lnkbtn8_CIVSS.OnClientClick = "javascript:window.open('" + strlnkbtn8_CIVSS.Trim + "');"
                'Me.lnkbtn9_CIVSS.OnClientClick = "javascript:window.open('" + strlnkbtn9_CIVSS.Trim + "');"
                'Me.lnkbtn10_CIVSS.OnClientClick = "javascript:window.open('" + strlnkbtn10_CIVSS.Trim + "');"

                'Me.lnkbtn1_CIVSS.Text = strCIVSSPathPrefix + DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn2_CIVSS.Text = strCIVSSPathPrefix + DateTime.Now.AddDays(-2).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn3_CIVSS.Text = strCIVSSPathPrefix + DateTime.Now.AddDays(-3).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn4_CIVSS.Text = strCIVSSPathPrefix + DateTime.Now.AddDays(-4).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn5_CIVSS.Text = strCIVSSPathPrefix + DateTime.Now.AddDays(-5).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn6_CIVSS.Text = strCIVSSPathPrefix + DateTime.Now.AddDays(-6).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn7_CIVSS.Text = strCIVSSPathPrefix + DateTime.Now.AddDays(-7).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn8_CIVSS.Text = strCIVSSPathPrefix + DateTime.Now.AddDays(-8).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn9_CIVSS.Text = strCIVSSPathPrefix + DateTime.Now.AddDays(-9).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn10_CIVSS.Text = strCIVSSPathPrefix + DateTime.Now.AddDays(-10).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"

                '' Download Vaccination Statistic Report (RVP)
                'Dim strRVPPathPrefix As String = "eHSD0004-RVP Claim_"

                'Dim strlnkbtn1_RVP As String = strFilePath + strRVPPathPrefix + DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn2_RVP As String = strFilePath + strRVPPathPrefix + DateTime.Now.AddDays(-2).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn3_RVP As String = strFilePath + strRVPPathPrefix + DateTime.Now.AddDays(-3).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn4_RVP As String = strFilePath + strRVPPathPrefix + DateTime.Now.AddDays(-4).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn5_RVP As String = strFilePath + strRVPPathPrefix + DateTime.Now.AddDays(-5).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn6_RVP As String = strFilePath + strRVPPathPrefix + DateTime.Now.AddDays(-6).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn7_RVP As String = strFilePath + strRVPPathPrefix + DateTime.Now.AddDays(-7).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn8_RVP As String = strFilePath + strRVPPathPrefix + DateTime.Now.AddDays(-8).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn9_RVP As String = strFilePath + strRVPPathPrefix + DateTime.Now.AddDays(-9).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn10_RVP As String = strFilePath + strRVPPathPrefix + DateTime.Now.AddDays(-10).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"


                'Me.lnkbtn1_RVP.OnClientClick = "javascript:window.open('" + strlnkbtn1_RVP.Trim + "');"
                'Me.lnkbtn2_RVP.OnClientClick = "javascript:window.open('" + strlnkbtn2_RVP.Trim + "');"
                'Me.lnkbtn3_RVP.OnClientClick = "javascript:window.open('" + strlnkbtn3_RVP.Trim + "');"
                'Me.lnkbtn4_RVP.OnClientClick = "javascript:window.open('" + strlnkbtn4_RVP.Trim + "');"
                'Me.lnkbtn5_RVP.OnClientClick = "javascript:window.open('" + strlnkbtn5_RVP.Trim + "');"
                'Me.lnkbtn6_RVP.OnClientClick = "javascript:window.open('" + strlnkbtn6_RVP.Trim + "');"
                'Me.lnkbtn7_RVP.OnClientClick = "javascript:window.open('" + strlnkbtn7_RVP.Trim + "');"
                'Me.lnkbtn8_RVP.OnClientClick = "javascript:window.open('" + strlnkbtn8_RVP.Trim + "');"
                'Me.lnkbtn9_RVP.OnClientClick = "javascript:window.open('" + strlnkbtn9_RVP.Trim + "');"
                'Me.lnkbtn10_RVP.OnClientClick = "javascript:window.open('" + strlnkbtn10_RVP.Trim + "');"

                'Me.lnkbtn1_RVP.Text = strRVPPathPrefix + DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn2_RVP.Text = strRVPPathPrefix + DateTime.Now.AddDays(-2).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn3_RVP.Text = strRVPPathPrefix + DateTime.Now.AddDays(-3).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn4_RVP.Text = strRVPPathPrefix + DateTime.Now.AddDays(-4).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn5_RVP.Text = strRVPPathPrefix + DateTime.Now.AddDays(-5).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn6_RVP.Text = strRVPPathPrefix + DateTime.Now.AddDays(-6).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn7_RVP.Text = strRVPPathPrefix + DateTime.Now.AddDays(-7).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn8_RVP.Text = strRVPPathPrefix + DateTime.Now.AddDays(-8).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn9_RVP.Text = strRVPPathPrefix + DateTime.Now.AddDays(-9).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn10_RVP.Text = strRVPPathPrefix + DateTime.Now.AddDays(-10).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"

                '' CRE13-001 - EHAPP [Start][Koala]
                '' -------------------------------------------------------------------------------------
                '' Download EHAPP Statistic Report (EHAPP)
                'Dim strEHAPPPathPrefix As String = "eHSD0023-EHAPP Claim_"

                'Dim strlnkbtn1_EHAPP As String = strFilePath + strEHAPPPathPrefix + DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn2_EHAPP As String = strFilePath + strEHAPPPathPrefix + DateTime.Now.AddDays(-2).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn3_EHAPP As String = strFilePath + strEHAPPPathPrefix + DateTime.Now.AddDays(-3).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn4_EHAPP As String = strFilePath + strEHAPPPathPrefix + DateTime.Now.AddDays(-4).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn5_EHAPP As String = strFilePath + strEHAPPPathPrefix + DateTime.Now.AddDays(-5).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn6_EHAPP As String = strFilePath + strEHAPPPathPrefix + DateTime.Now.AddDays(-6).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn7_EHAPP As String = strFilePath + strEHAPPPathPrefix + DateTime.Now.AddDays(-7).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn8_EHAPP As String = strFilePath + strEHAPPPathPrefix + DateTime.Now.AddDays(-8).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn9_EHAPP As String = strFilePath + strEHAPPPathPrefix + DateTime.Now.AddDays(-9).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Dim strlnkbtn10_EHAPP As String = strFilePath + strEHAPPPathPrefix + DateTime.Now.AddDays(-10).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"


                'Me.lnkbtn1_EHAPP.OnClientClick = "javascript:window.open('" + strlnkbtn1_EHAPP.Trim + "');"
                'Me.lnkbtn2_EHAPP.OnClientClick = "javascript:window.open('" + strlnkbtn2_EHAPP.Trim + "');"
                'Me.lnkbtn3_EHAPP.OnClientClick = "javascript:window.open('" + strlnkbtn3_EHAPP.Trim + "');"
                'Me.lnkbtn4_EHAPP.OnClientClick = "javascript:window.open('" + strlnkbtn4_EHAPP.Trim + "');"
                'Me.lnkbtn5_EHAPP.OnClientClick = "javascript:window.open('" + strlnkbtn5_EHAPP.Trim + "');"
                'Me.lnkbtn6_EHAPP.OnClientClick = "javascript:window.open('" + strlnkbtn6_EHAPP.Trim + "');"
                'Me.lnkbtn7_EHAPP.OnClientClick = "javascript:window.open('" + strlnkbtn7_EHAPP.Trim + "');"
                'Me.lnkbtn8_EHAPP.OnClientClick = "javascript:window.open('" + strlnkbtn8_EHAPP.Trim + "');"
                'Me.lnkbtn9_EHAPP.OnClientClick = "javascript:window.open('" + strlnkbtn9_EHAPP.Trim + "');"
                'Me.lnkbtn10_EHAPP.OnClientClick = "javascript:window.open('" + strlnkbtn10_EHAPP.Trim + "');"

                'Me.lnkbtn1_EHAPP.Text = strEHAPPPathPrefix + DateTime.Now.AddDays(-1).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn2_EHAPP.Text = strEHAPPPathPrefix + DateTime.Now.AddDays(-2).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn3_EHAPP.Text = strEHAPPPathPrefix + DateTime.Now.AddDays(-3).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn4_EHAPP.Text = strEHAPPPathPrefix + DateTime.Now.AddDays(-4).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn5_EHAPP.Text = strEHAPPPathPrefix + DateTime.Now.AddDays(-5).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn6_EHAPP.Text = strEHAPPPathPrefix + DateTime.Now.AddDays(-6).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn7_EHAPP.Text = strEHAPPPathPrefix + DateTime.Now.AddDays(-7).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn8_EHAPP.Text = strEHAPPPathPrefix + DateTime.Now.AddDays(-8).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn9_EHAPP.Text = strEHAPPPathPrefix + DateTime.Now.AddDays(-9).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                'Me.lnkbtn10_EHAPP.Text = strEHAPPPathPrefix + DateTime.Now.AddDays(-10).ToString("yyyyMMdd", New System.Globalization.CultureInfo("en-US")) + ".xls"
                '' CRE13-001 - EHAPP [End][Koala]
                'CRE13-016 Upgrade to excel 2007 [End][Karl]
        End Select
    End Sub
    'CRE13-016 Upgrade to excel 2007 [Start][Karl]
    '#Region "Generate Ad-hoc Report"

    '    ' Events

    '    Protected Sub lbtnAROpen_Click(ByVal sender As Object, ByVal e As System.EventArgs) ' Handles lbtnAROpen.Click
    '        panAR.Visible = True
    '        lbtnARClose.Visible = True
    '        lbtnAROpen.Visible = False

    '        SetupAR()

    '    End Sub

    '    Protected Sub lbtnARClose_Click(ByVal sender As Object, ByVal e As System.EventArgs) 'Handles lbtnARClose.Click
    '        panAR.Visible = False
    '        lbtnARClose.Visible = False
    '        lbtnAROpen.Visible = True
    '    End Sub

    '    Protected Sub btnARGenerate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
    '        lblARError.Visible = False

    '        ' --- Validation ---

    '        ' DBFlag
    '        Dim strDBFlag As String = ddlARDBFlag.SelectedValue

    '        ' Stored Procedure Name
    '        Dim strStoredProcedureName As String = txtARStoredProcedureName.Text.Trim

    '        ' Excel Template Name
    '        Dim strExcelTemplateName As String = txtARExcelTemplateName.Text.Trim

    '        If Not strExcelTemplateName.EndsWith(".xls") Then strExcelTemplateName = String.Format("{0}.xls", strExcelTemplateName)

    '        ' Command Timeout
    '        Dim intCommandTimeout As Integer = 0

    '        If txtARCommandTimeout.Text.Trim <> String.Empty AndAlso Not Integer.TryParse(txtARCommandTimeout.Text, intCommandTimeout) Then
    '            lblARError.Text = String.Format("Command Timeout {0} is invalid", txtARCommandTimeout.Text)
    '            lblARError.Visible = True

    '            Return
    '        End If

    '        ' --- End of Validation ---

    '        ' Generate file
    '        Try
    '            GenerateAR(strDBFlag, strStoredProcedureName, strExcelTemplateName, intCommandTimeout)

    '        Catch ex As Exception
    '            lblARError.Text = ex.Message
    '            lblARError.Visible = True

    '            Return
    '        End Try

    '    End Sub

    '    Private Sub GenerateAR(ByVal strDBFlag As String, ByVal strStoredProcedureName As String, ByVal strExcelTemplateName As String, ByVal intCommandTimeout As Integer)
    '        Dim strOutputFilePath As String = (New DataExportBLL).GetAdhocReportData(strDBFlag, strStoredProcedureName, strExcelTemplateName, intCommandTimeout)

    '        Dim file As New IO.FileInfo(strOutputFilePath)

    '        If Not file.Exists Then Throw New Exception(String.Format("File {0} not found!", strOutputFilePath))

    '        Response.Redirect("~/EnrolmentStat.aspx?id=" + Strings.Replace(strExcelTemplateName, "_Template.xls", String.Empty, Compare:=CompareMethod.Text))

    '    End Sub

    '    ''Setup

    '    Private Sub SetupAR()
    '        ' DBFlag
    '        ddlARDBFlag.DataSource = GetAllDBFlag()
    '        ddlARDBFlag.DataValueField = "DBFlagKey"
    '        ddlARDBFlag.DataTextField = "DBFlagValue"
    '        ddlARDBFlag.DataBind()

    '    End Sub

    '    Private Function GetAllDBFlag() As DataTable
    '        Dim dt As New DataTable
    '        dt.Columns.Add(New Data.DataColumn("DBFlagKey", GetType(String)))
    '        dt.Columns.Add(New Data.DataColumn("DBFlagValue", GetType(String)))

    '        Dim dr As DataRow = Nothing

    '        Dim strKey As String = String.Empty

    '        For i As Integer = 0 To ConfigurationManager.AppSettings.Count - 1
    '            strKey = ConfigurationManager.AppSettings.GetKey(i)
    '            If strKey.StartsWith("DBFlag") Then
    '                dr = dt.NewRow
    '                dr("DBFlagKey") = strKey
    '                dr("DBFlagValue") = ConfigurationManager.AppSettings(i)
    '                dt.Rows.Add(dr)
    '            End If
    '        Next

    '        Return dt

    '    End Function

    '#End Region
    'CRE13-016 Upgrade to excel 2007 [End][Karl]
End Class