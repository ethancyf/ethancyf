Public Partial Class PracticeSelection
    Inherits System.Web.UI.UserControl

    ' !!!'
    ' To Be Replace By Another Control

    Private DataTextFieldEngDisplay As String = "Practice_Name_Display"
    Private DataTextFieldChiDisplay As String = "Practice_Name_Display_Chi"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    'Public Sub BuildPracticeList(ByVal dtPracticeBankAcct As DataTable)
    '    Me.rbPracticeSelection.DataSource = dtPracticeBankAcct
    '    Me.rbPracticeSelection.DataTextField = "Practice_Name_Display"
    '    Me.rbPracticeSelection.DataValueField = "BankAccountKey"
    '    Me.rbPracticeSelection.DataBind()
    'End Sub

    Public Sub BuildPracticeList(ByVal dtPracticeBankAcct As DataTable, ByVal strLanguage As String)
        Me.rbPracticeSelection.DataSource = dtPracticeBankAcct

        If strLanguage.Trim().ToLower().Equals("zh-tw") Then
            Me.rbPracticeSelection.DataTextField = DataTextFieldChiDisplay
        Else
            Me.rbPracticeSelection.DataTextField = DataTextFieldEngDisplay
        End If

        Me.rbPracticeSelection.DataValueField = "BankAccountKey"
        Me.rbPracticeSelection.DataBind()
    End Sub

    Public Sub getSelection(ByRef strPracticeSelection As String, ByRef strName As String)
        If Me.rbPracticeSelection.SelectedIndex > -1 Then
            strPracticeSelection = Me.rbPracticeSelection.SelectedValue
            strName = Me.rbPracticeSelection.SelectedItem.Text
        Else
            strPracticeSelection = String.Empty
            strName = String.Empty
        End If
    End Sub

    Public Sub setSelectValue(ByVal strSelectedValue As String)
        Me.rbPracticeSelection.SelectedValue = strSelectedValue
    End Sub

    Public Sub setTitle(ByVal strTitleText As String)
        Me.lblSelectPracticeText.Text = strTitleText
    End Sub

    Public Sub setSelectedIndex(ByVal intIndex As Integer)
        Me.rbPracticeSelection.SelectedIndex = intIndex
    End Sub

End Class