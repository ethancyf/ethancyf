Imports GrapeCity.ActiveReports.SectionReportModel 
Imports GrapeCity.ActiveReports.Document
Imports Common.ComFunction
Imports Common.Component.ServiceProvider
Imports Common.Component.Practice
Imports Common.Format


Namespace PrintOut.DH_VSS
    Public Class DH_VSS

        Private _udtFormatter As Formatter
        Private _udfSP As ServiceProviderModel
        Private _practiceSameBankAccount As Dictionary(Of String, PracticeModelCollection)
        Private _strAppendixAVersionCode As String
        Private _strAppendixBVersionCode As String

        Public Sub New(ByVal udfSP As ServiceProviderModel)
            'Create object
            Me._udtFormatter = New Formatter

            'bypass value
            Me._udfSP = udfSP

            ' Retrieve Version Code from system parameters
            Dim udtGeneralFunction As New GeneralFunction
            Dim strParmValue1 As String = String.Empty

            udtGeneralFunction.getSytemParameterByParameterName("VersionCodeApplicationFormAppendixAEng", strParmValue1, String.Empty)
            _strAppendixAVersionCode = strParmValue1.Trim

            udtGeneralFunction.getSytemParameterByParameterName("VersionCodeApplicationFormAppendixBEng", strParmValue1, String.Empty)
            _strAppendixBVersionCode = strParmValue1.Trim

            ' This call is required by the Windows Form Designer.
            InitializeComponent()

            ' Add any initialization after the InitializeComponent() call.

            'Footer
            Me.txtPrintDetail.Text = String.Format("Print on {0}", _udtFormatter.formatDateTime(DateTime.Now(), "en-us"))
        End Sub

        Private Sub EHCVS_P0004_ReportStart(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.ReportStart
            Dim dt As New DataTable
            dt.Columns.Add(New DataColumn("FooterValue"))
            Dim dr As DataRow

            Dim practices As PracticeModelCollection
            Dim strKey As String
            Dim existPractices As PracticeModelCollection

            dr = dt.NewRow
            dr.Item("FooterValue") = _strAppendixAVersionCode
            dt.Rows.Add(_strAppendixAVersionCode)


            If Me._practiceSameBankAccount Is Nothing Then
                Me._practiceSameBankAccount = New Dictionary(Of String, PracticeModelCollection)

                For Each practice As PracticeModel In Me._udfSP.PracticeList.Values

                    'Check Bank account is not empty
                    If Not practice.BankAcct Is Nothing AndAlso Not practice.BankAcct.BranchName.Trim().Equals(String.Empty) AndAlso Not practice.BankAcct.BankName.Trim().Equals(String.Empty) Then

                        strKey = String.Format("{0}{1}{2}{3}", practice.BankAcct.BankName.Trim(), _
                        practice.BankAcct.BranchName.Trim(), _
                        practice.BankAcct.BankAcctNo.Trim(), _
                        practice.BankAcct.BankAcctOwner.Trim())


                        If Not _practiceSameBankAccount.ContainsKey(strKey) Then
                            practices = New PracticeModelCollection()
                            practices.Add(practice)
                            Me._practiceSameBankAccount.Add(strKey, practices)
                        Else
                            existPractices = _practiceSameBankAccount(strKey)
                            Me._practiceSameBankAccount.Remove(strKey)
                            existPractices.Add(practice)
                            Me._practiceSameBankAccount.Add(strKey, existPractices)
                        End If
                    Else
                        practices = New PracticeModelCollection()
                        practices.Add(practice)
                        Me._practiceSameBankAccount.Add(practice.DisplaySeq, practices)
                    End If


                Next
            End If

            If Me._practiceSameBankAccount.Count > 0 Then
                For Each key As String In Me._practiceSameBankAccount.Keys
                    dr = dt.NewRow
                    dr.Item("FooterValue") = String.Format("{0}-{1}", _strAppendixBVersionCode, key)
                    dt.Rows.Add(String.Format("{0}-{1}", _strAppendixBVersionCode, key))

                Next
            End If



            'dr = dt.NewRow
            'dr.Item("FooterValue") = FooterValue.CheckList
            'dt.Rows.Add(FooterValue.CheckList)

            Me.DataSource = dt
        End Sub

        Private Sub dtlHDVSS_AfterPrint(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtlHDVSS.AfterPrint



        End Sub

        'Private Sub detEHCVSP0004_BeforePrint(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtlHDVSS.BeforePrint
        '    Dim strFooterValue As String = Me.Fields.Item("FooterValue").Value

        '    If strFooterValue.Contains(_strAppendixBVersionCode) Then
        '        Me.txtPageName.Text = _strAppendixBVersionCode
        '    Else
        '        Me.txtPageName.Text = strFooterValue
        '    End If
        '    'Me.txtPageName.Text = "DH_HCV001(9/08)"
        'End Sub

        Private Sub detEHCVSP0004_Format(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles dtlHDVSS.Format


            Select Case Me.Fields.Item("FooterValue").Value
                Case _strAppendixAVersionCode
                    Dim subReportAppA As PrintOut.DH_VSS.DH_VSS002 = New PrintOut.DH_VSS.DH_VSS002(Me._udfSP)
                    Me.subReport.Report = subReportAppA
                    Me.txtPageName.Text = _strAppendixAVersionCode
                Case Else

                    Dim strFooterValue As String = Me.Fields.Item("FooterValue").Value

                    If strFooterValue.Contains(_strAppendixBVersionCode) Then

                        For Each strKey As String In _practiceSameBankAccount.Keys

                            If String.Format("{0}-{1}", _strAppendixBVersionCode, strKey) = strFooterValue Then
                                Dim subReportAppE As PrintOut.DH_VSS.DH_VSS006 = New PrintOut.DH_VSS.DH_VSS006(Me._practiceSameBankAccount(strKey), Me._udfSP)

                                Me.subReport.Report = subReportAppE

                                Exit For

                            End If
                        Next
                        Me.txtPageName.Text = _strAppendixBVersionCode
                    End If

                    'Case FooterValue.CheckList
                    '    Dim subReportCheckList As PrintOut.P0004.CheckList = New PrintOut.P0004.CheckList()
                    '    Me.subReport.Report = subReportCheckList
            End Select

        End Sub

    End Class

End Namespace