Imports Common.Component
Imports Common.Component.BankAcct
Imports Common.Component.MedicalOrganization
Imports Common.Component.Practice
Imports Common.Component.PracticeSchemeInfo
Imports Common.Component.Scheme
Imports Common.Component.SchemeInformation
Imports Common.Component.ServiceProvider
Imports Common.Component.Token
Imports Common.Component.VoucherScheme
Imports Common.DataAccess
Imports Common.Format

Imports HCVU.AccountChangeMaintenance
Imports HCVU.spProfile

Partial Public Class MOPracticeLists
    Inherits System.Web.UI.UserControl

    Public udtSPProfileBLL As New SPProfileBLL

    Private udtFormatter As New Formatter
    Private udtPracticeBLL As New PracticeBLL
    Private udtMOBLL As New MedicalOrganizationBLL

    Private intMOIndex As Integer
    Private intPracticeIndex As Integer

    Private strMOEname As String = String.Empty
    Private strPracticeEname As String = String.Empty

    Public Sub buildMOObject(ByVal udtMOList As MedicalOrganizationModelCollection, ByVal intCurrentMOIndex As Integer, ByVal strTableLocation As String)
        Me.gvMO.Visible = True
        Me.gvPractice.Visible = False

        intMOIndex = intCurrentMOIndex

        strMOEname = udtMOList.Item(intMOIndex).MOEngName.Trim

        Dim strMOENameList As New List(Of String)
        Dim strDistinctMOEname As New List(Of String)
        Dim strDuplicationMOEname As New List(Of String)
        Dim strDMOEname As String = String.Empty


        If Not IsNothing(udtMOList) Then
            If udtMOList.Count > 0 Then
                For Each udtMO As MedicalOrganizationModel In udtMOList.Values
                    If strTableLocation.Equals(TableLocation.Permanent) Then
                        If Not udtMO.RecordStatus.Trim.Equals(MedicalOrganizationStatus.Delisted) Then
                            strMOENameList.Add(udtMO.MOEngName.Trim)
                        End If
                    ElseIf strTableLocation.Equals(TableLocation.Staging) Then
                        If Not udtMO.RecordStatus.Trim.Equals(MedicalOrganizationStagingStatus.Delisted) Then
                            strMOENameList.Add(udtMO.MOEngName.Trim)
                        End If
                    Else
                        strMOENameList.Add(udtMO.MOEngName.Trim)
                    End If

                Next
                For Each item As String In strMOENameList
                    If Not strDistinctMOEname.Contains(item) Then
                        strDistinctMOEname.Add(item)
                    Else
                        If Not strDuplicationMOEname.Contains(item) Then
                            strDuplicationMOEname.Add(item)
                        End If
                    End If
                Next

            End If
        End If

        Dim dtMO As New DataTable
        dtMO.Columns.Add(New DataColumn("DisplaySeq"))
        dtMO.Columns.Add(New DataColumn("MOEngName"))
        dtMO.Columns.Add(New DataColumn("SortOrder", System.Type.GetType("System.Int32")))

        Dim dr As DataRow

        For Each udtMO As MedicalOrganizationModel In udtMOList.Values
            If udtMO.IsDuplicated Then

                dr = dtMO.NewRow
                dr(0) = udtMO.DisplaySeq.ToString
                dr(1) = udtMO.MOEngName
                If udtMO.DisplaySeq.Value = intMOIndex Then
                    dr(2) = 0
                Else
                    If strMOEname.Trim.Equals(udtMO.MOEngName.Trim) Then
                        dr(2) = udtMO.DisplaySeq
                    Else
                        dr(2) = (udtMO.DisplaySeq.Value * 10)
                    End If
                End If
                dtMO.Rows.Add(dr)
            End If
        Next

        Dim dtFMO As New DataTable
        dtFMO.Columns.Add(New DataColumn("DisplaySeq"))
        dtFMO.Columns.Add(New DataColumn("MOEngName"))
        dtFMO.Columns.Add(New DataColumn("SortOrder", System.Type.GetType("System.Int32")))

        Dim drF As DataRow

        Dim i As Integer = 1
        For Each item As String In strDuplicationMOEname
            drF = dtFMO.NewRow
            drF(1) = item
            drF(0) = String.Empty
            If item.Trim.Equals(strMOEname.Trim) Then
                drF(2) = 0
            Else
                drF(2) = i
                i = i + 1
            End If

            dtFMO.Rows.Add(drF)
        Next

        For Each drN As DataRow In dtFMO.Rows
            For Each drT As DataRow In dtMO.Rows
                If CStr(drN.Item("MOEngName")).Trim.Equals(CStr(drT.Item("MOEngName"))) Then
                    drN.Item("DisplaySeq") = drN.Item("DisplaySeq") + ", " + CStr(drT.Item("DisplaySeq"))
                End If
            Next

            If CStr(drN.Item("DisplaySeq")).Length > 2 Then
                drN.Item("DisplaySeq") = CStr(drN.Item("DisplaySeq")).Substring(2)
            End If
        Next



        If Not IsNothing(dtFMO) Then
            If dtFMO.Rows.Count > 0 Then
                Dim dvMO As DataView = New DataView(dtFMO)
                dvMO.Sort = "SortOrder, MOEngName ASC"
                Me.gvMO.DataSource = dvMO
                Me.gvMO.DataBind()
            End If
        End If

        lblTitle.Text = Me.GetGlobalResourceObject("Text", "DuplicateMO")

    End Sub

    Public Sub buildPracticeObject(ByVal udtPracticeList As PracticeModelCollection, ByVal intCurrentPracticeIndex As Integer, ByVal strTablelocation As String)
        Me.gvMO.Visible = False
        Me.gvPractice.Visible = True

        intPracticeIndex = intCurrentPracticeIndex

        strPracticeEname = udtPracticeList.Item(intPracticeIndex).PracticeName.Trim

        Dim strPracticeENameList As New List(Of String)
        Dim strDistinctPracticeEname As New List(Of String)
        Dim strDuplicationPracticeEname As New List(Of String)
        Dim strDPracticeEname As String = String.Empty

        If Not IsNothing(udtPracticeList) Then
            If udtPracticeList.Count > 0 Then
                For Each udtPractice As PracticeModel In udtPracticeList.Values

                    If strTableLocation.Equals(TableLocation.Permanent) Then
                        If Not udtPractice.RecordStatus.Trim.Equals(PracticeStatus.Delisted) Then
                            strPracticeENameList.Add(udtPractice.PracticeName.Trim)
                        End If
                    ElseIf strTableLocation.Equals(TableLocation.Staging) Then
                        If Not udtPractice.RecordStatus.Trim.Equals(PracticeStagingStatus.Delisted) Then
                            strPracticeENameList.Add(udtPractice.PracticeName.Trim)
                        End If
                    Else
                        strPracticeENameList.Add(udtPractice.PracticeName.Trim)
                    End If
                Next

                For Each item As String In strPracticeENameList
                    If Not strDistinctPracticeEname.Contains(item) Then
                        strDistinctPracticeEname.Add(item)
                    Else
                        If Not strDuplicationPracticeEname.Contains(item) Then
                            strDuplicationPracticeEname.Add(item)
                        End If
                    End If
                Next

            End If
        End If

        Dim dtPractice As New DataTable
        dtPractice.Columns.Add(New DataColumn("DisplaySeq"))
        dtPractice.Columns.Add(New DataColumn("PracticeName"))
        dtPractice.Columns.Add(New DataColumn("SortOrder"))


        Dim dr As DataRow

        For Each udtPractice As PracticeModel In udtPracticeList.Values
            If udtPractice.IsDuplicated Then
                dr = dtPractice.NewRow
                dr(0) = udtPractice.DisplaySeq.ToString
                dr(1) = udtPractice.PracticeName
                If udtPractice.DisplaySeq = intPracticeIndex Then
                    dr(2) = 0
                Else
                    If strPracticeEname.Trim.Equals(udtPractice.PracticeName.Trim) Then
                        dr(2) = udtPractice.DisplaySeq
                    Else
                        dr(2) = (udtPractice.DisplaySeq * 10)
                    End If

                End If

                dtPractice.Rows.Add(dr)
            End If
        Next

        Dim dtFPractice As New DataTable
        dtFPractice.Columns.Add(New DataColumn("DisplaySeq"))
        dtFPractice.Columns.Add(New DataColumn("PracticeName"))
        dtFPractice.Columns.Add(New DataColumn("SortOrder", System.Type.GetType("System.Int32")))

        Dim drF As DataRow

        Dim i As Integer = 1
        For Each item As String In strDuplicationPracticeEname
            drF = dtFPractice.NewRow
            drF(1) = item
            drF(0) = String.Empty
            If item.Trim.Equals(strPracticeEname.Trim) Then
                drF(2) = 0
            Else
                drF(2) = i
                i = i + 1
            End If

            dtFPractice.Rows.Add(drF)
        Next

        For Each drN As DataRow In dtFPractice.Rows
            For Each drT As DataRow In dtPractice.Rows
                If CStr(drN.Item("PracticeName")).Trim.Equals(CStr(drT.Item("PracticeName"))) Then
                    drN.Item("DisplaySeq") = drN.Item("DisplaySeq") + ", " + CStr(drT.Item("DisplaySeq"))
                End If
            Next

            If CStr(drN.Item("DisplaySeq")).Length > 2 Then
                drN.Item("DisplaySeq") = CStr(drN.Item("DisplaySeq")).Substring(2)
            End If
        Next


        If Not IsNothing(dtFPractice) Then
            If dtFPractice.Rows.Count > 0 Then
                Dim dvPractice As DataView = New DataView(dtFPractice)
                dvPractice.Sort = "SortOrder, PracticeName ASC"
                Me.gvPractice.DataSource = dvPractice
                Me.gvPractice.DataBind()
            End If
        End If

        lblTitle.Text = Me.GetGlobalResourceObject("Text", "DuplicatePractice")

    End Sub

    Private Sub gvMO_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvMO.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            If e.Row.RowIndex = 0 Then
                e.Row.Cells(0).CssClass = "tableText"
                e.Row.Cells(1).CssClass = "tableText"

            End If
        End If
    End Sub

    Private Sub gvPractice_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvPractice.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            If e.Row.RowIndex = 0 Then
                e.Row.Cells(0).CssClass = "tableText"
                e.Row.Cells(1).CssClass = "tableText"

            End If

        End If
    End Sub
End Class