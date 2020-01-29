Imports Common.ComFunction.ParameterFunction

Partial Public Class reportCriteriaPeriodFromTo
    Inherits System.Web.UI.UserControl
    Implements IReportCriteriaUC

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim udtFormatter As New Common.Format.Formatter()

        Me.calExFromDate.Format = udtFormatter.EnterDateFormat
        Me.calExToDate.Format = udtFormatter.EnterDateFormat

        Me.imgPeriodFromError.Visible = False
        Me.imgPeriodToError.Visible = False

    End Sub

    Public Function GetParameterString() As String Implements IReportCriteriaUC.GetParameterString
        Return "PeriodFrom:" + Me.txtPeriodFrom.Text + ",PeriodTo:" + Me.txtPeriodTo.Text
    End Function

    Public Function GetParameterList() As ParameterCollection Implements IReportCriteriaUC.GetParameterList
        Dim udtParameterCollection As New ParameterCollection()
        udtParameterCollection.AddParam("A", Me.txtPeriodFrom.Text)
        udtParameterCollection.AddParam("B", Me.txtPeriodTo.Text)
        Return udtParameterCollection
    End Function

    Public Function GetCriteriaInput() As StoreProcParamCollection Implements IReportCriteriaUC.GetCriteriaInput
        Dim udtStoreProcParamCollection As New StoreProcParamCollection()

        Dim udtFormatter As New Common.Format.Formatter()

        Dim dtmFrom As DateTime
        Dim dtmTo As DateTime

        If IsDate(udtFormatter.convertDate(Me.txtPeriodFrom.Text.Trim(), "E")) Then
            dtmFrom = udtFormatter.convertDate(Me.txtPeriodFrom.Text.Trim(), "E")
        End If

        If IsDate(udtFormatter.convertDate(Me.txtPeriodTo.Text.Trim(), "E")) Then
            dtmTo = udtFormatter.convertDate(Me.txtPeriodTo.Text.Trim(), "E")
        End If

        udtStoreProcParamCollection.AddParam("@PeriodFrom", System.Data.SqlDbType.DateTime, 8, dtmFrom.ToString(udtFormatter.DisplayDateFormat))
        udtStoreProcParamCollection.AddParam("@PeriodTo", System.Data.SqlDbType.DateTime, 8, dtmTo.ToString(udtFormatter.DisplayDateFormat))
        Return udtStoreProcParamCollection
    End Function

    Public Function ValidateCriteriaInput(ByVal strReportID As String) As System.Collections.Generic.List(Of Common.ComObject.SystemMessage) Implements IReportCriteriaUC.ValidateCriteriaInput

        Me.imgPeriodFromError.Visible = False
        Me.imgPeriodToError.Visible = False

        Dim udtFormatter As New Common.Format.Formatter()
        Me.txtPeriodFrom.Text = udtFormatter.formatInputDate(Me.txtPeriodFrom.Text)
        Me.txtPeriodTo.Text = udtFormatter.formatInputDate(Me.txtPeriodTo.Text)

        Dim udtValidator As New Common.Validation.Validator()
        Dim lstError As List(Of Common.ComObject.SystemMessage) = udtValidator.chkReportSubmissionPeriodFromToDate(strReportID, Me.txtPeriodFrom.Text, Me.txtPeriodTo.Text)
        For Each errorObj As Common.ComObject.SystemMessage In lstError
            If errorObj.MessageCode = "00001" Or errorObj.MessageCode = "00003" Or errorObj.MessageCode = "00005" Or errorObj.MessageCode = "00007" Then
                Me.imgPeriodFromError.Visible = True
            End If

            If errorObj.MessageCode = "00002" Or errorObj.MessageCode = "00004" Or errorObj.MessageCode = "00006" Or errorObj.MessageCode = "00007" Then
                Me.imgPeriodToError.Visible = True
            End If
        Next

        Return lstError
    End Function

    Public Sub ValidateCriteriaInput(ByVal strReportID As String, ByRef lstError As System.Collections.Generic.List(Of Common.ComObject.SystemMessage), ByRef lstErrorParam As System.Collections.Generic.List(Of String)) Implements IReportCriteriaUC.ValidateCriteriaInput
        Me.imgPeriodFromError.Visible = False
        Me.imgPeriodToError.Visible = False

        Dim udtFormatter As New Common.Format.Formatter()
        Me.txtPeriodFrom.Text = udtFormatter.formatInputDate(Me.txtPeriodFrom.Text)
        Me.txtPeriodTo.Text = udtFormatter.formatInputDate(Me.txtPeriodTo.Text)

        Dim udtValidator As New Common.Validation.Validator()
        lstError = udtValidator.chkReportSubmissionPeriodFromToDate(strReportID, Me.txtPeriodFrom.Text, Me.txtPeriodTo.Text)
        lstErrorParam = New List(Of String)
        For i As Integer = 0 To lstError.Count - 1
            lstErrorParam.Add(String.Empty)
        Next

        For Each errorObj As Common.ComObject.SystemMessage In lstError
            If errorObj.MessageCode = "00001" Or errorObj.MessageCode = "00003" Or errorObj.MessageCode = "00005" Or errorObj.MessageCode = "00007" Then
                Me.imgPeriodFromError.Visible = True
            End If

            If errorObj.MessageCode = "00002" Or errorObj.MessageCode = "00004" Or errorObj.MessageCode = "00006" Or errorObj.MessageCode = "00007" Then
                Me.imgPeriodToError.Visible = True
            End If
        Next

    End Sub
End Class