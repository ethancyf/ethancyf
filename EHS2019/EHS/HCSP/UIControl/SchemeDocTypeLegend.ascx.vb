Imports Common.Component.DocType
Imports Common.Component.DocType.DocTypeModel
Imports Common.Component.Scheme

Partial Public Class SchemeDocTypeLegend
    Inherits System.Web.UI.UserControl

#Region "Members"

    Private _enumClaimType As SchemeDocTypeModel.ClaimTypeEnumClass

#End Region

#Region "Fields"

    Private udtDocTypeBLL As New DocTypeBLL
    Private udtSchemeClaimBLL As New SchemeClaimBLL

#End Region

#Region "Constants"

    Private Const BR As String = "<br />"

#End Region

#Region "Session Constants"

    Private Const SESS_SchemeCount As String = "020201_SchemeCount"
    Private Const SESS_DictionaryIndexToSchemeCode As String = "020201_DictionaryIndexToSchemeCode"

#End Region

    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [Start][Chris YIM]
    ' ---------------------------------------------------------------------------------------------------------
    Public Sub Build(ByVal strLanguage As String, ByVal udtDocTypeList As DocTypeModelCollection, ByVal enumClaimType As SchemeDocTypeModel.ClaimTypeEnumClass)
        _enumClaimType = enumClaimType

        lblHSIVSSRemark.Visible = False

        Dim dt As New DataTable
        dt.Columns.Add("Document_Code", GetType(String))
        dt.Columns.Add("Document_Name", GetType(String))

        ' Document Accept
        For Each udtDocType As DocTypeModel In udtDocTypeList
            Dim dr As DataRow = dt.NewRow
            dr("Document_Code") = udtDocType.DocCode
            dr("Document_Name") = String.Format("{0}" + BR + "[{1}]", udtDocType.DocName(strLanguage), udtDocType.DocIdentityDesc(strLanguage))
            dt.Rows.Add(dr)
        Next

        ' Header text
        gvDocument.Columns(0).HeaderText = String.Format("{0}" + BR + "[{1}]", _
                                            Me.GetGlobalResourceObject("Text", "DocumentType"), Me.GetGlobalResourceObject("Text", "IdentityDocNo"))

        Dim dicIndexToSchemeCode As New Dictionary(Of Integer, String)
        Dim intScheme As Integer = 1

        ' Scheme
        For Each udtSchemeC As SchemeClaimModel In udtSchemeClaimBLL.getValidClaimPeriodScheme()
            If Not dicIndexToSchemeCode.ContainsValue(udtSchemeC.SchemeCode.Trim) Then
                dicIndexToSchemeCode.Add(intScheme, udtSchemeC.SchemeCode.Trim)
                gvDocument.Columns(intScheme).HeaderText = udtSchemeC.SchemeDesc(strLanguage)

                'If udtSchemeC.ControlType = SchemeClaimModel.EnumControlType.HSIVSS OrElse udtSchemeC.ControlType = SchemeClaimModel.EnumControlType.RVP Then
                If udtSchemeC.SchemeCode = SchemeClaimModel.HSIVSS OrElse udtSchemeC.SchemeCode = SchemeClaimModel.RVP Then
                    lblHSIVSSRemark.Visible = True
                    lblHSIVSSRemark.Text = Me.GetGlobalResourceObject("Text", "AcceptDocumentHSIVSSBelow11YearOld")
                End If

                intScheme += 1
            End If

        Next

        Session(SESS_SchemeCount) = intScheme
        Session(SESS_DictionaryIndexToSchemeCode) = dicIndexToSchemeCode

        ' Hide the extra columns
        For i As Integer = intScheme To gvDocument.Columns.Count - 1
            gvDocument.Columns(i).Visible = False
        Next

        gvDocument.DataSource = dt
        gvDocument.DataBind()

        ' Add the extra header row at the beginning
        Dim gvHeaderRow As New GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal)

        Dim tblCell As TableCell = Nothing

        ' Empty cell for first column
        tblCell = New TableCell
        tblCell.BackColor = Drawing.Color.Transparent
        tblCell.BorderStyle = BorderStyle.None
        gvHeaderRow.Cells.Add(tblCell)

        ' Cell: Scheme
        tblCell = New TableCell
        tblCell.Text = Me.GetGlobalResourceObject("Text", "Scheme")

        tblCell.ColumnSpan = intScheme - 1

        gvHeaderRow.Cells.Add(tblCell)

        gvDocument.Controls(0).Controls.AddAt(0, gvHeaderRow)

        ' Hide the gridview border (so as to hide the border at the top-left cell)
        gvDocument.BorderStyle = BorderStyle.None

    End Sub
    ' CRE19-001 (New initiatives for VSS and PPP in 2019-20) [End][Chris YIM]

    Protected Sub gvDocument_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs)
        If e.Row.RowType = DataControlRowType.DataRow Then
            Dim dicIndexToSchemeCode As Dictionary(Of Integer, String) = Session(SESS_DictionaryIndexToSchemeCode)

            For i As Integer = 1 To CInt(Session(SESS_SchemeCount)) - 1
                Dim hfDocumentCode As HiddenField = e.Row.FindControl("hfDocumentCode")

                If IsDocumentAcceptedForScheme(hfDocumentCode.Value.Trim, dicIndexToSchemeCode(i), _enumClaimType) Then
                    Dim imgScheme As Image = e.Row.FindControl("imgScheme" + i.ToString)
                    imgScheme.Visible = True
                End If
            Next

        End If
    End Sub

    Private Function IsDocumentAcceptedForScheme(ByVal strDocCode As String, ByVal strSchemeCode As String, _
                                                 ByVal enumClaimType As SchemeDocTypeModel.ClaimTypeEnumClass) As Boolean

        For Each udtSchemeDocType As SchemeDocTypeModel In udtDocTypeBLL.getSchemeDocTypeBySchemeClaimType(strSchemeCode, enumClaimType)
            If udtSchemeDocType.DocCode.Trim = strDocCode.Trim Then Return True
        Next

        Return False

    End Function

End Class